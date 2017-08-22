using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace OMMCDP
{
    public partial class frmLogin : Form
    {
        #region Animate

        //매개변수
        //hwnd는 윈도우 핸들
        //dwTime는 Animate 하는 시간
        //dwFlags는 Animate 효과 플래그

        [DllImport("User32.dll", EntryPoint = "AnimateWindow")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        //Flag 값

        //#define AW_HOR_POSITIVE             0x00000001
        //#define AW_HOR_NEGATIVE             0x00000002
        //#define AW_VER_POSITIVE             0x00000004
        //#define AW_VER_NEGATIVE             0x00000008
        //#define AW_CENTER                   0x00000010
        //#define AW_HIDE                     0x00010000
        //#define AW_ACTIVATE                 0x00020000
        //#define AW_SLIDE                    0x00040000
        //#define AW_BLEND                    0x00080000

        #endregion

        private Label lb_Msg;
        private Database gDb;

        public bool bAlertMsg = false;
        public bool bQuitFlag = false;
        public bool bSuperVisor = false;
        public bool bSuperVisor_Login = false;

        private MonitorPower csMonitor;

        public frmLogin(Label lblMsg)
        {
            lb_Msg = lblMsg;
            InitializeComponent();

#if DEBUG
            txtLoginID.Text = "101092";
            txtPassWord.Text = "8282";
#endif
        }

        // Login Form Load
        private void frmLogin_Load(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 100, 0x00000010);
            gDb = new Database(BasicInfo.CONNSTRING);
            lbErrorMsg.ForeColor = Color.Red;
            BasicInfo.LOGIN_FORM_ACTIVATE = true;

            if (bSuperVisor)
            {
                lbSuperVisor.Visible = true;
                this.BackColor = Color.Black;
            }
            else
            {
                lbSuperVisor.Visible = false;
                this.BackColor = Color.MediumBlue;
                //csMonitor = new MonitorPower(this.Handle.ToInt32());
                //tmr_Power.Start();
            }

            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        // Login 버튼 클릭
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sID = txtLoginID.Text.Trim();
            string sPass = txtPassWord.Text.Trim();

            if (sID.Length == 0 || sPass.Length == 0)
            {
                lbErrorMsg.Visible = true;
                return;
            }

            if (!CheckingUserID(sID, sPass))
            {
                lbErrorMsg.Visible = true;
                return;
            }

            if (!bSuperVisor)
            {
                // false => QC Alerts true => none
                //bAlertMsg = CheckingAlertMsg(sID);

                if (BasicInfo.USER_ID != sID)
                    bAlertMsg = false;
                else
                    bAlertMsg = true;

                BasicInfo.USER_ID = sID;
            }
            else
            {
                BasicInfo.SUPERVISOR_ID = sID;
                bSuperVisor_Login = true;
            }

            Update_Wkh_Login(sID);
            DialogResult = DialogResult.OK;

            this.Close();
        }

        // 로그인 기록 남기기
        private void Update_Wkh_Login(string sID)
        {
            string query = string.Format(" Insert Into wkh_login(user_id,login_flag, login_time, ip_address) values('{0}', 'P', getdate(), '{1}' )", sID, Common.IPv4);

            try
            {
                gDb.ExecuteNonQuery(query);

                BasicInfo.LOGIN_FLAG = true;

            }
            catch (Exception ex)
            {
                frmMain.LogMsg(" Update_Wkh_Login Database Update Error " + ex.Message);
                return;
            }

        }

        // QC Alert을 봤는지 체크
        private bool CheckingAlertMsg(string sID)
        {
            DataTable table = new DataTable();

            string query = string.Format(" Select * from wkh_work_file Where File_name = (Select file_name from mst_work_file where pc_code = '{0}' and file_type = 'Q') and User_id = '{1}' and pc_code = '{0}' "
                                        , Common.PC.Code, sID);

            try
            {
                gDb.GetData(query, ref table);

                if (table.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                frmMain.LogMsg("WKH_WORK_FILE Database Reading Error " + ex.Message);
                return false;
            }
        }

        // 로그인 User ID 체크
        private bool CheckingUserID(string sID, string sPass)
        {
            DataTable table = new DataTable();
            string sMsg = string.Empty;
            string sUserName = string.Empty;
            string sAuth_Code = string.Empty;
            string query = string.Format(" select top 1 user_name, auth_code from mst_user where user_id = '{0}' and password = '{1}'"
                                        , sID, sPass);

            try
            {
                gDb.GetData(query, ref table);

                if (table.Rows.Count > 0)
                {
                    sUserName = table.Rows[0]["user_name"].ToString().Trim();
                    sAuth_Code = table.Rows[0]["auth_code"].ToString().Trim();
                    if (bSuperVisor)
                    {
                        if (sAuth_Code.Trim().Substring(0, 1) == "S")
                        {
                            lb_Msg.ForeColor = Color.Blue;
                            lb_Msg.Text = "Supervisor Login successful";
                            BasicInfo.SUPERVISOR_FLAG = true;
                            return true;
                        }
                        else
                        {
                            sMsg = string.Format("Login Failed!! [Login ID:{0} Password:{1}] ", sID, sPass);
                            lb_Msg.ForeColor = Color.Red;
                            lb_Msg.Text = "Supervisor Login Failed!!";
                            frmMain.LogMsg(sMsg);
                            return false;

                        }
                    }
                    else
                    {
                        lb_Msg.ForeColor = Color.Blue;
                        lb_Msg.Text = "Login success!!";
                        return true;
                    }


                }
                else
                {
                    sMsg = string.Format("Login Failed!! [Login ID:{0} Password:{1}] ", sID, sPass);
                    lb_Msg.ForeColor = Color.Red;
                    lb_Msg.Text = "Login Failed!!";
                    frmMain.LogMsg(sMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                sMsg = string.Format("Login Failed!! [Login ID:{0} Password:{1}] ", sID, sPass);
                frmMain.LogMsg(sMsg + ex.Message);
                return false;
            }
        }

        // 종료
        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (bSuperVisor)
            {
                this.Close();
            }
            else
            {
                bQuitFlag = true;
                //Application.Exit();
                this.DialogResult = DialogResult.Cancel;
            }
        }


        public void _scanner_OnDataReceived(object sender, object e)
        {
            string data = string.Format("{0}", e).ToUpper();

            byte[] buffer1 = new byte[] { 2 };
            byte[] buffer2 = new byte[] { 3 };
            byte[] buffer3 = new byte[] { 13 };
            byte[] buffer4 = new byte[] { 10 };


            data = data.Replace(Encoding.Default.GetString(buffer1), "");
            data = data.Replace(Encoding.Default.GetString(buffer2), "");
            data = data.Replace(Encoding.Default.GetString(buffer3), "");
            data = data.Replace(Encoding.Default.GetString(buffer4), "");


            this.Invoke(new MethodInvoker(delegate()
            {
                frmMain.LogMsg("LogIn Scandata : [" + data + "]");

                if (!data.IndexOf("\r\n").Equals(-1) || data.Length < 7)
                    return;

                txtLoginID.Text = data.Substring(0, 6);
                txtPassWord.Text = data.Substring(6);
                btnLogin.Focus();
                btnLogin_Click(null, null);

            }));

        }

        // 스캔받은 데이터 처리부
        //private void ScanData_Recv(object send, SerialSetting.PortEventArgs args)
        //{
        //    this.Invoke(new MethodInvoker(delegate()
        //   {
        //       if (args == null)
        //           return;

        //       string data = args.GetRecvData().Trim(); ;
        //       frmMain.LogMsg("LogIn Scandata : [" + data + "]");

        //       if (!data.IndexOf("\r\n").Equals(-1) || data.Length < 7)
        //           return;

        //       txtLoginID.Text = data.Substring(0, 6);
        //       txtPassWord.Text = data.Substring(6);
        //       btnLogin.Focus();
        //       btnLogin_Click(null, null);

        //   }));

        //}

        private void frmLogin_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            BasicInfo.LOGIN_FORM_ACTIVATE = false;
        }

        private void tmr_Power_Tick(object sender, EventArgs e)
        {
            //tmr_Power.Stop();

            //csMonitor.PowerOff();
        }

        private void btn_Keyboard_Click(object sender, EventArgs e)
        {
            String sDirPath = @"C:\Programs\MYTTOUCH\";
            DirectoryInfo di = new DirectoryInfo(sDirPath);

            System.Diagnostics.ProcessStartInfo startInfo =
                new System.Diagnostics.ProcessStartInfo();

            startInfo.FileName = "MYTTOUCH.EXE"; // Execution File Name

            if (di.Exists == true)
            {
                startInfo.WorkingDirectory = @"C:\Programs\MYTTOUCH\"; // Executed path
            }
            else
            {
                startInfo.WorkingDirectory = @"C:\Program Files\MYTTOUCH\";
            }

            System.Diagnostics.Process[] proc =
                System.Diagnostics.Process.GetProcessesByName("MYTTOUCH");

            if (proc.Length > 0)
            {
                proc[0].Kill();
            }
            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }


    }
}