using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OMMCDP;
using SerialPort;
using System.Deployment.Application;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using KFA.MES.LOG;

namespace OMMCDP
{

    public partial class frmMain : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime([In] ref SYSTEMTIME st);

        private DataTable _dtPCINFO = null;
        private DataTable _dtPARTS = null;
        private List<ucPartCell> _partCells = new List<ucPartCell>();
        private SerialPortHandler _scanner = null;
        private static LogHandler _logHandler = null;

        private Database gDb;           //데이터 베이스 클래스
        private bool bScreenChange;
        private frmWorkInst fWorkInst;                    // QC Alert Form
        private Form gChild;
        private frmLogin fLogin;
        public BasicInfo gOrder;       //현재 공정 도착 SKID 정보
        private frmQCAlert fQCAlert;

        /// <summary>
        /// Part no 가 없는 part pass 처리
        /// Parameter 0 : scan barcode, 1 : Part no
        /// </summary>
        private string[] _autoSkipParam = Properties.Settings.Default.AUTO_SKIP_BARCODE_TO_PART_NO.Split('|');

        private bool _isScanProcessing = false;

        private int _prevPageNo = 0;

        public frmMain()
        {
            InitializeComponent();

            _logHandler = new LogHandler(System.Environment.CurrentDirectory + @"\LOG", "MatrialsFeeding", false);
            _logHandler.WriteLog(KFA.MES.LOG.LogType.Information, "Program start");
        }

        protected override void OnLoad(EventArgs e)
        {
            

#if DEBUG
            btnPass.Visible = true;
#else
            btnPass.Visible = false;

#endif
            
            BasicInfo.CONNSTRING = Common.ConnectionString = Properties.Settings.Default.CONNECTIONSTRING;
#if DEBUG
            Common.IPv4 = "127.0.0.1";
            FrmMessageBox msg = new FrmMessageBox(
                "Notice", 
                string.Format("{0}{1}{2}", "Debug mode", Environment.NewLine, BasicInfo.CONNSTRING), 
                FrmMessageBox.MessageStatus.OK, 
                FrmMessageBox.AutoClose.Yes, 
                MessageBoxButtons.OK,
                3);
            msg.ShowDialog();
#else
            System.Net.IPHostEntry IPHost = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName());
            Common.IPv4 = IPHost.AddressList[0].ToString();
#endif

            //Common.IPv4 = "172.17.40.70";

            btnPass.Visible = false;

            if (_autoSkipParam.Length < 2)
            {
                FrmMessageBox msg1 = new FrmMessageBox(
                "Error",
                string.Format("{0}{1}{2}", "AUTO_SKIP_BARCODE_TO_PART_NO data error", Environment.NewLine, Properties.Settings.Default.AUTO_SKIP_BARCODE_TO_PART_NO),
                FrmMessageBox.MessageStatus.OK,
                FrmMessageBox.AutoClose.No,
                MessageBoxButtons.OK,
                3);
                msg1.ShowDialog();
                this.Close();
                return;
            }

            LoadBasicInfo();

            gDb = new Database(BasicInfo.CONNSTRING);
            // 네트워크 드라이브 설정 
            InitNetDrive();
            // 바탕화면 아이콘 생성
            CreateDesktopIcon();

            //PC Local Time 변경하기
#if DEBUG
#else
            setLocalTime();
#endif
            lblTitle.Text = Common.Title;

            LoadPCInfo();

            try
            {
                _scanner = new SerialPortHandler(string.Format("COM{0}", Common.ScannerPort), Common.ScannerBaud, "NONE", "8", "1");
                _scanner.OnDataReceived += new EventSerialPortHandler(_scanner_OnDataReceived);
                _scanner.Open();
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);
                FrmMessageBox frm = new FrmMessageBox(
                                "COM port error",
                                ex.Message,
                                FrmMessageBox.MessageStatus.NG,
                                FrmMessageBox.AutoClose.No,
                                MessageBoxButtons.OK,
                                Properties.Settings.Default.MSGBOX_HIDE_DELAY);
                frm.ShowDialog();
            }

            LogInApp();

            InitPartsControls();

            LoadPartsData();

            tmrAutoSkip.Interval = (int)(Properties.Settings.Default.AUTO_SKIP_SECOND * 1000);
            tmrAutoSkip.Start();

            base.OnLoad(e);
        }

        private void LogInApp()
        {
            try
            {
                if (BasicInfo.LOGIN_FLAG)
                {
                    FrmMessageBox frm = new FrmMessageBox(
                                "Logout",
                                "Would you like to logOut?",
                                FrmMessageBox.MessageStatus.QUESTION,
                                FrmMessageBox.AutoClose.No,
                                MessageBoxButtons.YesNo,
                                Properties.Settings.Default.MSGBOX_HIDE_DELAY);
                    frm.ShowDialog();

                    if (frm.DialogResult == DialogResult.No) return;

                    foreach (var obj in _partCells.AsEnumerable())
                    {
                        obj.Visible = false;
                    }


                    if (BasicInfo.USER_ID.Length > 0)
                    {
                        DataTable table = new DataTable();

                        string query = string.Format(" select top 1 log_idx from wkh_login where logout_time is null and login_time is not null and user_id = '{0}' " +
                                                     " order by login_time desc, log_idx desc ", BasicInfo.USER_ID);

                        gDb.GetData(query, ref table);

                        if (table.Rows.Count > 0)
                        {
                            int nLog_idx = Int32.Parse(table.Rows[0]["log_idx"].ToString().Trim());
                            query = string.Format(" update wkh_login set logout_time = getdate() where logout_time is null and login_time is not null and user_id = '{0}' and log_idx = '{1}' "
                                                        , BasicInfo.USER_ID, nLog_idx);
                            gDb.ExecuteNonQuery(query);
                        }
                    }
                }


                try
                {
                    fLogin = new frmLogin(lbMsg);
                    _scanner.OnDataReceived -= _scanner_OnDataReceived;
                    _scanner.OnDataReceived += fLogin._scanner_OnDataReceived;

                    fLogin.ShowDialog();

                    if (BasicInfo.LOGIN_FLAG)
                    {
                        btn_UserLogin.ForeColor = Color.Black;
                        btn_UserLogin.Text = "User LogOut";

                        if (_partCells.Count() > 0)
                        {
                            foreach (var obj in _partCells.AsEnumerable())
                            {
                                obj.Visible = true;
                            }

                            LoadPartsData();
                        }

                    }
                    else
                    {
                        btn_UserLogin.ForeColor = Color.Black;
                        btn_UserLogin.Text = "User LogIn";
                    }

                    if (fLogin.DialogResult == DialogResult.Cancel)
                    {
                        this.Close();
                    }
                }
                finally
                {
                    _scanner.OnDataReceived += _scanner_OnDataReceived;
                    _scanner.OnDataReceived -= fLogin._scanner_OnDataReceived;
                }
            }
            catch
            {
            }
            finally
            {
               
            }
        }

        // 네트워크 드라이브 설정 
        private void InitNetDrive()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @"C:\";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            process.EnableRaisingEvents = false;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.Write(@"net use z: \\10.120.175.46\mes-share /user:10.120.175.46\ppc Mobis1" + Environment.NewLine);
            process.StandardInput.Close();
            process.WaitForExit();
            process.Close();

        }

        // 배포시 바탕화면에 shotcut 생성
        private void CreateDesktopIcon()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                if (ad.IsFirstRun)
                {
                    string desktopPath = string.Empty;
                    desktopPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                        , "\\", "SubLinePPC", ".appref-ms");
                    string shotcutName = string.Empty;
                    shotcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs)
                        , "\\", "OMMCDP", "\\", "SubLinePPC", ".appref-ms");
                    File.Copy(shotcutName, desktopPath, true);
                }
            }

        }
        
        public void setLocalTime()
        {
            DataTable table = new DataTable();
            string query = string.Empty;
            string sErrMsg = string.Empty;
            string sTime = string.Empty;

            try
            {
                query = "select convert(varchar, getdate(), 120) as dual";

                gDb.GetData(query, ref table);

                if (table.Rows.Count > 0)
                    sTime = table.Rows[0]["dual"].ToString().Trim();
                else
                    return;

                DateTime DT = Convert.ToDateTime(sTime);

                SYSTEMTIME st = new SYSTEMTIME();
                st.wYear = (ushort)DT.Year;
                st.wMonth = (ushort)DT.Month;
                st.wDay = (ushort)DT.Day;
                st.wHour = (ushort)(DT.Hour + 5);
                st.wMinute = (ushort)DT.Minute;
                st.wSecond = (ushort)DT.Second;
                st.wDayOfWeek = (ushort)0;
                st.wMilliseconds = (ushort)0;

                if (!SetSystemTime(ref st))
                {
                    sErrMsg = "Change to PCTime Failed";
                    return;
                }

                return;
            }
            catch (Exception ex)
            {
                sErrMsg = "setLocalTime()" + ex.Message;
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Warning, ex.Message);
                return;
            }
        }
        
        private void _scanner_OnDataReceived(object sender, object e)
        {
            string data = _scanner.RemoveStxEtx(string.Format("{0}", e).ToUpper());

            System.Diagnostics.Debug.WriteLine(data);

            if (_isScanProcessing) return;

            if (data == "PASS")
                data = _partCells[ucPartCell.CurrentWorkingCellIndex].Part;
            
            if (_prevPageNo == 0)
                PartDataMachingCheck(data);
        }

        public static void LogMsg(string data)
        {
            if (_logHandler != null)
            {
                _logHandler.WriteLog(LogType.Information, data);
            }
        }

        private void PartDataMachingCheck(string data)
        {
            string strCheckData = "";

            _isScanProcessing = true;

            data = data.Trim();

            if (Properties.Settings.Default.AUTO_SKIP_ENABLE && _autoSkipParam[0] == data)
            {
                data = _autoSkipParam[1];
            }

            if (ucPartCell.CurrentWorkingCellIndex >= 0)
            {
                if (Properties.Settings.Default.PART_GROUP_CHECK)
                {
                    var cells = _partCells.Where(
                        x => 
                            x.GroupVINIndex == _partCells[ucPartCell.CurrentWorkingCellIndex].GroupVINIndex && 
                            x.Part.Equals(data) &&
                            x.Status != CellStatus.Ok
                            );

                    if (cells.Count() > 0)
                    {
                        ucPartCell.CurrentWorkingCellIndex = cells.First().WorkingIndex;
                    }
                }

                if (data.Substring(0, 1) == "P")
                    strCheckData = data.Substring(1);
                else
                    strCheckData = data;

                if (!_partCells[ucPartCell.CurrentWorkingCellIndex].Part.Equals(strCheckData)
                    && !_partCells[ucPartCell.CurrentWorkingCellIndex].Part_CN_New.Equals(strCheckData)
                    && !_partCells[ucPartCell.CurrentWorkingCellIndex].Part_CN_Old.Equals(strCheckData))
                {
                    _partCells[ucPartCell.CurrentWorkingCellIndex].Status = CellStatus.NG;

                    _logHandler.WriteLog(
                        KFA.MES.LOG.LogType.Warning,
                        "Mismatching",
                        String.Format("SEQ : {0}, PART : {1}, SCAN {2}, CELL [{3},{4}]",
                        _dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["SEQ_NO"],
                        _partCells[ucPartCell.CurrentWorkingCellIndex].Part,
                        data,
                        Convert.ToInt32(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["M"]) + 1,
                        Convert.ToInt32(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["REVS_COUNT"]) - Convert.ToInt32(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["N"]) + 1));

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;

                            ChangeCellColors();

                            this.Invalidate(true);

                            tsslblMessage.Text = String.Format("Mismatching SCAN : {0}, MES  : {1}", data, _partCells[ucPartCell.CurrentWorkingCellIndex].Part);

                            FrmMessageBox frm = new FrmMessageBox(
                                "Mismatching",
                                String.Format("SCAN : {1}{0}MES  : {2}",
                                Environment.NewLine,
                                data.Length > 5 ? data.Substring(data.Length - 5, 5) : data,
                                _partCells[ucPartCell.CurrentWorkingCellIndex].DisplayPart, 
                                Environment.NewLine),
                                FrmMessageBox.MessageStatus.NG,
                                FrmMessageBox.AutoClose.No,
                                MessageBoxButtons.OK,
                                Properties.Settings.Default.MSGBOX_HIDE_DELAY);

                            frm.BackColor = Color.Red;
                            frm.ShowDialog();
                            
                            return;

                        }
                        catch (Exception ex)
                        {
                            tsslblMessage.Text = ex.Message;
                            _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);

                            FrmMessageBox msgbox = new FrmMessageBox(
                                                                        "Error",
                                                                        ex.Message,
                                                                        FrmMessageBox.MessageStatus.NG,
                                                                        FrmMessageBox.AutoClose.No,
                                                                        MessageBoxButtons.OK,
                                                                        0);

                            msgbox.ShowDialog();
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                            _isScanProcessing = false;
                        }
                    });
                }
                else
                {

                    _logHandler.WriteLog(
                        KFA.MES.LOG.LogType.Information,
                        "Matching ok",
                        String.Format("SEQ : {0}, PART : {1}, SCAN {2}, CELL [{3},{4}]",
                        _partCells[ucPartCell.CurrentWorkingCellIndex].Seq,
                        _partCells[ucPartCell.CurrentWorkingCellIndex].Part,
                        data,
                        _dtPARTS.Rows.Count > ucPartCell.CurrentWorkingCellIndex ? Convert.ToInt32(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["M"]) + 1 : 0,
                        _dtPARTS.Rows.Count > ucPartCell.CurrentWorkingCellIndex ? Convert.ToInt32(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["REVS_COUNT"]) - Convert.ToInt32(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["N"]) + 1 : 0));

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;

                            UpdatePartsMatchingDoneFlag();

                            _partCells[ucPartCell.CurrentWorkingCellIndex].Status = CellStatus.Ok;

                            //--- 작업 안한 파트 수량
                            var query = _partCells.AsEnumerable().Where(x => x.Status != CellStatus.Ok).Count();

                            if (query == 0)
                            {
                                LoadPartsData();
                            }

                            //--- 다음 작업할 파트 표시 처리
                            var cell = _partCells.AsEnumerable().Where(x => x.Status == CellStatus.NextWorks || x.Status == CellStatus.CurrentWork || x.Status == CellStatus.NG).OrderBy(o => o.WorkingIndex);

                            if (cell.Count() > 0)
                            {
                                ucPartCell c = cell.First();
                                ucPartCell.CurrentWorkingCellIndex = c.WorkingIndex;
                                ucPartCell.CurrentWorkingHVGroupIndex = c.GroupHVIndex;
                                ucPartCell.CurrentWorkingVINGroupIndex = c.GroupVINIndex;
                                c.Status = CellStatus.CurrentWork;
                            }

                            ChangeCellColors();

                            this.Invalidate(true);

                            tsslblMessage.Text = String.Format("Matching ok current : {0}, next  : {1}", data, _partCells[ucPartCell.CurrentWorkingCellIndex].Part);

                            if (_partCells[ucPartCell.CurrentWorkingCellIndex].Part != _autoSkipParam[1] && Properties.Settings.Default.MSGBOX_ENABLE)
                            {
                                FrmMessageBox frm = new FrmMessageBox(
                                    "Matching ok",
                                    String.Format("NEXT : {0}",
                                    cell.Count() > 0 ? _partCells[ucPartCell.CurrentWorkingCellIndex].DisplayPart : "",
                                    Environment.NewLine),
                                    FrmMessageBox.MessageStatus.OK,
                                    FrmMessageBox.AutoClose.Yes,
                                    MessageBoxButtons.OK,
                                    Properties.Settings.Default.MSGBOX_HIDE_DELAY);

                                frm.ShowDialog();
                            }
                            
                            return;

                        }
                        catch (Exception ex)
                        {
                            _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);

                            tsslblMessage.Text = ex.Message;

                            FrmMessageBox msgbox = new FrmMessageBox(
                                                                        "Error",
                                                                        ex.Message,
                                                                        FrmMessageBox.MessageStatus.NG,
                                                                        FrmMessageBox.AutoClose.No,
                                                                        MessageBoxButtons.OK,
                                                                        0);

                            msgbox.ShowDialog();
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;

                            if (Properties.Settings.Default.AUTO_SKIP_ENABLE && _autoSkipParam[1].Equals(_partCells[ucPartCell.CurrentWorkingCellIndex].Part))
                            {
                                tmrAutoSkip.Enabled = true;
                            }

                            _isScanProcessing = false;
                        }

                    });
                }
            }
        }

        private void LoadBasicInfo()
        {
            Database DB = new Database(Common.ConnectionString);
            DataTable dtPC = new DataTable();
            string query = string.Empty;

            try
            {
                query = string.Format(" EXEC prLinePPC_PCInfo '{0}' ", Common.IPv4);


                DB.Fill(query, ref dtPC);

                if (dtPC.Rows.Count > 0)
                {
                    Common.LineCd = dtPC.Rows[0]["line_code"].ToString().Trim();
                    Common.StationCd = dtPC.Rows[0]["station_code"].ToString().Trim();
                    Common.PC.Code = dtPC.Rows[0]["pc_code"].ToString().Trim();
                    Common.PC.Index = dtPC.Rows[0]["pc_idx"].ToString().Trim();
                    Common.PC.Name = dtPC.Rows[0]["pc_name"].ToString().Trim();
                    Common.PC.Kind = dtPC.Rows[0]["PC_KIND"].ToString().Trim();
                    Common.ScannerPort = dtPC.Rows[0]["scanner_port"].ToString().Trim();
                    Common.ScannerBaud = dtPC.Rows[0]["scanner_baud"].ToString().Trim();
                    Common.Title = dtPC.Rows[0]["main_title"].ToString().Trim();


                    if (dtPC.Rows.Count > 0)
                    {
                        BasicInfo.PCCD = dtPC.Rows[0]["pc_code"].ToString().Trim();
                        //BasicInfo.LINE_CODE = dtPC.Rows[0]["line_code"].ToString().Trim();
                        //BasicInfo.STATION_CODE = dtPC.Rows[0]["station_code"].ToString().Trim();
                        //BasicInfo.PC_IDX = Int32.Parse(dtPC.Rows[0]["pc_idx"].ToString().Trim());
                        //BasicInfo.PC_NAME = dtPC.Rows[0]["pc_name"].ToString().Trim();
                        //BasicInfo.PC_KIND = dtPC.Rows[0]["pc_kind"].ToString().Trim();
                        //BasicInfo.MAIN_TITLE = dtPC.Rows[0]["main_title"].ToString().Trim();
                        //BasicInfo.SCANNER_PORT = Int32.Parse(dtPC.Rows[0]["scanner_port"].ToString().Trim());
                    }
                   
                }
                else
                {
                    MessageBox.Show(" LoadBasicInfo Failed!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Common.WriteLog(" LoadBasicInfo Failed result count is 0");
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);
                MessageBox.Show(" Initialize Failed!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Common.WriteLog(" XML Initialize Failed " + ex.Message + "[ReadXMLfile()] " + Common.IPv4 + "[" + query + "]");
                this.Close();
            }

        }

        private void LoadPCInfo()
        {
            Database DB = new Database(Common.ConnectionString);
            _dtPCINFO = new DataTable();
            string query = string.Empty;

            try
            {

                query = string.Format(Properties.Settings.Default.SQL_GET_PCINFO, Common.IPv4);


                DB.Fill(query, ref _dtPCINFO);

                if (_dtPCINFO.Rows.Count == 0)
                {
                    MessageBox.Show(" Initialize Failed!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Common.WriteLog(" XML Initialize Failed [ReadXMLfile()]");
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);
                MessageBox.Show(" Initialize Failed!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Common.WriteLog(" XML Initialize Failed " + ex.Message + "[ReadXMLfile()] " + Common.IPv4 + "[" + query + "]");
                this.Close();
            }

        }

        private bool LoadPartsData()
        {
            Database DB = new Database(Common.ConnectionString);
            _dtPARTS = new DataTable();
            string query = string.Empty;

            try
            {

                query = string.Format(" EXEC prFeedingGetData '{0}', '{1}', '{2}', '{3}'", Common.LineCd, Common.StationCd, Common.PC.Code, _prevPageNo);

                DB.Fill(query, ref _dtPARTS);

                if (_dtPARTS.Rows.Count == 0)
                {
                    foreach (ucPartCell part in _partCells)
                    {
                        part.Seq = "";
                        part.Part = "";
                        part.Status = CellStatus.NextWorks;
                    }
                }
                if (_dtPARTS.Rows.Count != _partCells.Count)
                {
                    FrmMessageBox msgbox = new FrmMessageBox(
                        "Call supervisor", 
                        string.Format("Not enough order data{0}to make feeding data", Environment.NewLine), 
                        FrmMessageBox.MessageStatus.NG, 
                        FrmMessageBox.AutoClose.No, 
                        MessageBoxButtons.OK, 
                        0);

                    msgbox.ShowDialog();
                }
                else
                {
             
                    for (int iCell = 0; iCell < _partCells.Count; iCell++)
                    {
                        if (iCell < _dtPARTS.Rows.Count)
                        {
                            _partCells[iCell].Seq = _dtPARTS.Rows[iCell]["SEQ_NO"].ToString();

                            _partCells[iCell].Part = _dtPARTS.Rows[iCell]["PART_NO"].ToString();
                            _partCells[iCell].Part_CN_New = (_dtPARTS.Rows[iCell]["NEW_PART_NO"] == DBNull.Value) ? "NULL" : _dtPARTS.Rows[iCell]["NEW_PART_NO"].ToString();
                            _partCells[iCell].Part_CN_Old = (_dtPARTS.Rows[iCell]["OLD_PART_NO"] == DBNull.Value) ? "NULL" : _dtPARTS.Rows[iCell]["OLD_PART_NO"].ToString();

                            if (_dtPCINFO.Rows[0]["DIRECTION"].ToString().Substring(0, 1) == "H")
                            {
                                _partCells[iCell].GroupHVIndex = _partCells[iCell].CellPoint.Y;
                            }
                            else
                            {
                                _partCells[iCell].GroupHVIndex = _partCells[iCell].CellPoint.X;
                            }

                            // old: var b
                            int b;
                            try
                            {
                                b = Convert.ToByte(_dtPARTS.Rows[iCell]["STATUS"].ToString(), 10) & (int)Math.Pow(2, Convert.ToByte(_dtPARTS.Rows[iCell]["ID"].ToString(), 10) - 1);
                            }
                            catch
                            {
                                b = 1;
                            }

                            if (b == 0)
                            {
                                _partCells[iCell].Status = CellStatus.NextWorks;
                            }
                            else 
                            {
                                _partCells[iCell].Status = CellStatus.Ok;
                            }
                        }
                        else
                        {
                            _partCells[iCell].Seq = "";
                            _partCells[iCell].Part = "";
                            _partCells[iCell].Status = CellStatus.NextWorks;
                        }

                    }

                    int tGroupIndex = 0;
                    foreach (ucPartCell c in _partCells)
                    {
                        var r = _partCells.Where(x => x.Seq.Equals(c.Seq)).OrderBy(x => x.WorkingIndex).First();

                        if (r.GroupVINIndex < 0)
                        {
                            c.GroupVINIndex = tGroupIndex;
                            tGroupIndex++;
                        }
                        else
                        {
                            c.GroupVINIndex = r.GroupVINIndex;
                        }
                    }

                    var cell = from obj in _partCells.AsEnumerable()
                               where obj.Status == CellStatus.NextWorks
                               select obj;


                    if (cell.Count() > 0)
                    {
                        ucPartCell c = cell.OrderBy(x => x.WorkingIndex).First();

                        if (c != null)
                        {
                            ucPartCell.CurrentWorkingCellIndex = c.WorkingIndex;
                            ucPartCell.CurrentWorkingHVGroupIndex = c.GroupHVIndex;
                            ucPartCell.CurrentWorkingVINGroupIndex = c.GroupVINIndex;
                            c.Status = CellStatus.CurrentWork;
                        }
                    }
                }

                ChangeCellColors();

                return true;

            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);
                tsslblMessage.Text = ex.Message;

                FrmMessageBox msgbox = new FrmMessageBox(
                        "Error",
                        ex.Message,
                        FrmMessageBox.MessageStatus.NG,
                        FrmMessageBox.AutoClose.No,
                        MessageBoxButtons.OK,
                        0);

                msgbox.ShowDialog();

                return false;
            }
            finally
            {
                this.Invalidate(true);
            }
        }

        private void ChangeCellColors()
        {
            foreach (ucPartCell part in _partCells)
            {
                //---- PART CELL COLOR
                if (part.Status == CellStatus.NextWorks)
                {
                    part.lblPart.BackColor = Properties.Settings.Default.PART_BACKCOLOR_NEXTWORKS;
                    part.lblPart.ForeColor = Properties.Settings.Default.PART_FORECOLOR_NEXTWORKS;
                }
                else if (part.Status == CellStatus.Ok)
                {
                    part.lblPart.BackColor = Properties.Settings.Default.PART_BACKCOLOR_OK;
                    part.lblPart.ForeColor = Properties.Settings.Default.PART_FORECOLOR_OK;
                }
                else if (part.Status == CellStatus.NG)
                {
                    part.lblPart.BackColor = Properties.Settings.Default.PART_BACKCOLOR_NG;
                    part.lblPart.ForeColor = Properties.Settings.Default.PART_FORECOLOR_NG;
                }
                else if (part.Status == CellStatus.CurrentWork)
                {
                    part.lblPart.BackColor = Properties.Settings.Default.PART_BACKCOLOR_CURRENTWORK;
                    part.lblPart.ForeColor = Properties.Settings.Default.PART_FORECOLOR_CURRENTWORK;
                }

                //---- SEQ CELL COLOR
                if (part.Status == CellStatus.CurrentWork)
                {
                    part.lblSeq.BackColor = Properties.Settings.Default.SEQ_BACKCOLOR_CURRENTWORK;
                    part.lblSeq.ForeColor = Properties.Settings.Default.SEQ_FORECOLOR_CURRENTWORK;
                }
                else if (part.Status == CellStatus.NextWorks)
                {
                    part.lblSeq.BackColor = Properties.Settings.Default.SEQ_BACKCOLOR_NEXTWORKS;
                    part.lblSeq.ForeColor = Properties.Settings.Default.SEQ_FORECOLOR_NEXTWORKS;
                }
                else if (part.Status == CellStatus.NG)
                {
                }
                else if (part.Status == CellStatus.Ok)
                {
                    part.lblSeq.BackColor = Properties.Settings.Default.SEQ_BACKCOLOR_OK;
                    part.lblSeq.ForeColor = Properties.Settings.Default.SEQ_FORECOLOR_OK;
                }

                //---- DIRECTION GROUP CELL COLOR
                if (ucPartCell.CurrentWorkingHVGroupIndex == part.GroupHVIndex)
                {
                    part.lblSeq.ForeColor = Properties.Settings.Default.SEQ_FORECOLOR_GROUP;
                }
            }

            //---- PART GROUP CELLS COLOR
            if (ucPartCell.CurrentWorkingCellIndex >= 0)
            {
                var vinGroup = _partCells.Where(x => x.Seq == _partCells[ucPartCell.CurrentWorkingCellIndex].Seq && x.WorkingIndex != ucPartCell.CurrentWorkingCellIndex);

                if (vinGroup != null && vinGroup.Count() > 0 && Properties.Settings.Default.PART_GROUP_DISPLAY)
                {
                    foreach (ucPartCell cell in vinGroup)
                    {
                        if (Properties.Settings.Default.PART_GROUP_CHECK )
                        {
                            if (cell.Status == CellStatus.CurrentWork || 
                                cell.Status == CellStatus.NextWorks)
                            {
                                cell.lblPart.BackColor = Properties.Settings.Default.PART_BACKCOLOR_CURRENTWORK;
                                cell.lblPart.ForeColor = Properties.Settings.Default.PART_FORECOLOR_CURRENTWORK;
                            }
                            else if (cell.Status == CellStatus.NG)
                            {
                                cell.lblPart.BackColor = Properties.Settings.Default.PART_BACKCOLOR_NG;
                                cell.lblPart.ForeColor = Properties.Settings.Default.PART_FORECOLOR_NG;
                            }
                        }
                        cell.lblSeq.BackColor = Properties.Settings.Default.SEQ_BACKCOLOR_CURRENTWORK;
                        cell.lblSeq.ForeColor = Properties.Settings.Default.SEQ_FORECOLOR_CURRENTWORK;
                    }
                }
            }

        }

        private void UpdatePartsMatchingDoneFlag()
        {
            try
            {
                string[] arrQuery = null;

                var query = from cell in _partCells.AsEnumerable()
                            where cell.Status != CellStatus.Ok
                            select cell;

                //---- 마지막 cell 처리 시
                if (query != null && query.Count() == 1)
                {
                    arrQuery = new string[_partCells.Count];

                    for (int iIndex = 0; iIndex < _partCells.Count; iIndex++)
                    {
                        arrQuery[iIndex] = string.Format(" EXEC prFeedingUpdateMatchingReuslt '{0}', '{1}','{2}','{3}','{4}'",
                                                            Common.LineCd,
                                                            Common.PC.Code,
                                                            _dtPARTS.Rows[iIndex]["VIN"].ToString(),
                                                            _dtPARTS.Rows[iIndex]["SEQ_NO"].ToString(),
                                                            "C");
                    }
                }
                else
                {
                    arrQuery = new string[] {string.Format(" EXEC prFeedingUpdateMatchingReuslt '{0}', '{1}','{2}','{3}','{4}'", 
                    Common.LineCd, 
                    Common.PC.Code,
                    _dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["VIN"].ToString(),
                    _dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["SEQ_NO"].ToString(),
                    Math.Pow(2, Convert.ToInt16(_dtPARTS.Rows[ucPartCell.CurrentWorkingCellIndex]["ID"]) - 1).ToString())};
                }

                Database dbComm = new Database(Properties.Settings.Default.CONNECTIONSTRING);

                dbComm.ExecuteNonQuery(arrQuery);
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Error, ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private void InitPartsControls()
        {
            try
            {
                if (_dtPCINFO.Rows.Count == 0) return;

                tlpParts.RowStyles.Clear();
                tlpParts.ColumnStyles.Clear();

                _partCells.Clear();

                int rows = 6;
                int cols = 4;

                rows = Convert.ToInt32(_dtPCINFO.Rows[0]["ROW_COUNT"]);
                cols = Convert.ToInt32(_dtPCINFO.Rows[0]["COLUMN_COUNT"]);

                this.tlpParts.RowCount = rows;

                for (int i = 0; i < rows; i++)
                {
                    this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / (float)rows));
                }

                this.tlpParts.ColumnCount = cols;

                for (int i = 0; i < cols; i++)
                {
                    this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / (float)cols));
                }

                //---- VS2008 버그
                for (int i = 0; i < rows; i++)
                {
                    this.tlpParts.RowStyles[i].Height = 100F / (float)rows;
                }
                for (int i = 0; i < cols; i++)
                {
                    this.tlpParts.ColumnStyles[i].Width = 100F / (float)cols;
                }

                int index = 0;

                if (_dtPCINFO.Rows[0]["DIRECTION"].ToString().Substring(0, 1) == "V")
                {
                    for (int iCol = 0; iCol < cols; iCol++)
                    {
                        for (int iRow = 0; iRow < rows; iRow++)
                        {
                            ucPartCell obj = new ucPartCell(Properties.Settings.Default.DISPLAY_SEQ_H_RATIO, Properties.Settings.Default.DISPLAY_PART_H_RATIO);
                            obj.Dock = DockStyle.Fill;
                            obj.Seq = index.ToString();
                            obj.WorkingIndex = index;
                            obj.CellPoint = new Point(iCol, iRow);
                            this.tlpParts.Controls.Add(obj, iCol, iRow);
                            _partCells.Add(obj);
                            index++;
                        }
                    }
                }
                else if (_dtPCINFO.Rows[0]["DIRECTION"].ToString().Substring(0, 1) == "H")
                {
                    for (int iRow = 0; iRow < rows; iRow++)
                    {
                        for (int iCol = 0; iCol < cols; iCol++)
                        {
                            ucPartCell obj = new ucPartCell(Properties.Settings.Default.DISPLAY_SEQ_H_RATIO, Properties.Settings.Default.DISPLAY_PART_H_RATIO);
                            obj.Dock = DockStyle.Fill;
                            obj.Seq = index.ToString();
                            obj.WorkingIndex = index;
                            this.tlpParts.Controls.Add(obj, iCol, iRow);
                            obj.CellPoint = new Point(iCol, iRow);
                            _partCells.Add(obj);
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
               
            }
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            tsslblTimer.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                if (BasicInfo.USER_ID.Length > 0)
                {
                    DataTable table = new DataTable();

                    string query = string.Format(" select top 1 log_idx from wkh_login where logout_time is null and login_time is not null and user_id = '{0}' " +
                                                 " order by login_time desc, log_idx desc ", BasicInfo.USER_ID);

                    gDb.GetData(query, ref table);

                    if (table.Rows.Count > 0)
                    {
                        int nLog_idx = Int32.Parse(table.Rows[0]["log_idx"].ToString().Trim());
                        query = string.Format(" update wkh_login set logout_time = getdate() where logout_time is null and login_time is not null and user_id = '{0}' and log_idx = '{1}' "
                                                    , BasicInfo.USER_ID, nLog_idx);
                        gDb.ExecuteNonQuery(query);
                    }
                }
            }
            catch
            {
            }
            if (_scanner != null)
            {
                _scanner.Close();
                _scanner.Dispose();
            }
            if (_logHandler != null)
            {
                _logHandler.WriteLog(KFA.MES.LOG.LogType.Information, "Program close");
                _logHandler.Dispose();
            }
            base.OnClosing(e);
        }

        private void btnDataRefresh_Click(object sender, EventArgs e)
        {
            if (LoadPartsData())
            {
                FrmMessageBox frm = new FrmMessageBox(
                                "Information",
                                "Refresh data ok",
                                FrmMessageBox.MessageStatus.OK,
                                FrmMessageBox.AutoClose.Yes,
                                MessageBoxButtons.OK,
                                0.8);
                frm.Size = new Size(600, 400);
                frm.ShowDialog();
            };
        }

        private void btnWorkInstruction_Click(object sender, EventArgs e)
        {
            if (btnWorkInstruction.BackColor == Color.LightCyan && !bScreenChange)
            {
                bScreenChange = true;
                btn_UserLogin.Enabled = false;
                btnWorkInstruction.BackColor = Color.DarkCyan;
                fWorkInst = new frmWorkInst(lbMsg, false, @"Z:\Line_PC\Work Instruction_Spanish\");
                gChild = fWorkInst;

                this.AddOwnedForm(gChild);
                gChild.Location = this.PointToScreen(new Point(0, 0));
                gChild.Show();
            }
            else if (btnWorkInstruction.BackColor == Color.DarkCyan)
            {
                bScreenChange = false;
                btn_UserLogin.Enabled = true;
                btnWorkInstruction.BackColor = Color.LightCyan;
                gChild.Close();
            }
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
#if DEBUG
            if (_dtPARTS != null && _dtPARTS.Rows.Count > 0 && ucPartCell.CurrentWorkingCellIndex < 0)
            {
                ucPartCell.CurrentWorkingCellIndex = 0;
            }
#endif

            if (ucPartCell.CurrentWorkingCellIndex >= 0)
            {
                PartDataMachingCheck(_partCells[ucPartCell.CurrentWorkingCellIndex].Part);
            }
        }

        private void btn_WorkInst_Click(object sender, EventArgs e)
        {
            if (btn_WorkInst.BackColor == Color.LightCyan && !bScreenChange)
            {
                bScreenChange = true;
                btn_UserLogin.Enabled = false;
                btn_WorkInst.BackColor = Color.DarkCyan;
                fWorkInst = new frmWorkInst(lbMsg, false, @"Z:\Line_PC\Work Instruction\");
                gChild = fWorkInst;

                this.AddOwnedForm(gChild);
                gChild.Location = this.PointToScreen(new Point(0, 0));
                gChild.Show();
            }
            else if (btn_WorkInst.BackColor == Color.DarkCyan)
            {
                bScreenChange = false;
                btn_UserLogin.Enabled = true;
                btn_WorkInst.BackColor = Color.LightCyan;
                gChild.Close();
            }
        }

        private void btn_UserLogin_Click(object sender, EventArgs e)
        {
            LogInApp();
        }

        private void btn_qualityAlert_Click(object sender, EventArgs e)
        {
            if (btn_qualityAlert.BackColor == Color.LightCyan && !bScreenChange)
            {
                bScreenChange = true;
                btn_UserLogin.Enabled = false;
                btn_qualityAlert.BackColor = Color.DarkCyan;
                fQCAlert = new frmQCAlert(lbMsg, false);
                gChild = fQCAlert;

                this.AddOwnedForm(gChild);
                gChild.Location = this.PointToScreen(new Point(0, 0));
                gChild.Show();
            }
            else if (btn_qualityAlert.BackColor == Color.DarkCyan)
            {
                bScreenChange = false;
                btn_UserLogin.Enabled = true;
                btn_qualityAlert.BackColor = Color.LightCyan;
                gChild.Close();
            }

        }

        private void btn_WorkInst_Click_1(object sender, EventArgs e)
        {
            if (btn_WorkInst.BackColor == Color.LightCyan && !bScreenChange)
            {
                bScreenChange = true;
                btn_UserLogin.Enabled = false;
                btn_WorkInst.BackColor = Color.DarkCyan;
                fWorkInst = new frmWorkInst(lbMsg, false, @"Z:\Line_PC\Work Instruction\");
                gChild = fWorkInst;

                this.AddOwnedForm(gChild);
                gChild.Location = this.PointToScreen(new Point(0, 0));
                gChild.Show();
            }
            else if (btn_WorkInst.BackColor == Color.DarkCyan)
            {
                bScreenChange = false;
                btn_UserLogin.Enabled = true;
                btn_WorkInst.BackColor = Color.LightCyan;
                gChild.Close();
            }
        }

        private void tmrAutoSkip_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrAutoSkip.Enabled = false;

                if (Properties.Settings.Default.AUTO_SKIP_ENABLE && _partCells[ucPartCell.CurrentWorkingCellIndex].Part.Equals((_autoSkipParam[1])))
                {
                    PartDataMachingCheck(_autoSkipParam[0]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                tmrAutoSkip.Enabled = true;
            }
        }

        private void lblTitle_Paint(object sender, PaintEventArgs e)
        {
            ucPartCell.DrawString((Control)sender, e, ((Control)sender).Text, 10);
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            _prevPageNo++;
            LoadPartsData();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            _prevPageNo = 0;
            LoadPartsData();
        }
    }
}
