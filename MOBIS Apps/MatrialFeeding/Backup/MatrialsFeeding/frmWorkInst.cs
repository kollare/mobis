using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace OMMCDP
{
    public partial class frmWorkInst : Form
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
        private bool bBtnShow;
        private int gFileViewCount = 0;
        private int gFileCount = 0;
        private string gFolder = string.Empty;
        private string m_sPath = string.Empty;

        public frmWorkInst(Label lbMsg, bool bFlag, string path)
        {
            bBtnShow = bFlag;
            lb_Msg = lbMsg;
            m_sPath = path;
            InitializeComponent();
        }


        public void Show_powerpoint()
        {

            try
            {
                string sPath = m_sPath + gFolder;
                DirectoryInfo dinfo = new DirectoryInfo(sPath);
                if (!dinfo.Exists)
                {
                    MessageBox.Show(" directory not found ");
                    this.Close();
                }

                FileInfo[] finfo = dinfo.GetFiles();

                if (finfo.Length.Equals(0))
                    this.Close();

                string[] sQCfilePath = new string[finfo.Length];
                int nidx = 0;

                for (int i = 0; i < finfo.Length; i++)
                {
                    if (finfo[i].FullName.ToString().ToUpper().Trim().Substring(finfo[i].FullName.Length - 3, 3) == "GIF" || finfo[i].FullName.ToString().ToUpper().Trim().Substring(finfo[i].FullName.Length - 3, 3) == "PNG" || finfo[i].FullName.ToString().ToUpper().Trim().Substring(finfo[i].FullName.Length - 3, 3) == "JPG" || finfo[i].FullName.ToString().ToUpper().Trim().Substring(finfo[i].FullName.Length - 3, 3) == "BMP")
                    {
                        sQCfilePath[nidx] = finfo[i].FullName.ToString();
                        nidx++;
                    }
                }

                if (nidx != 0)
                {
                    gFileCount = nidx;   //전체 파일개수 저장
                    picQC.Image = Image.FromFile(sQCfilePath[gFileViewCount]);
                    if (gFileCount != 0)
                    {
                        if (gFileViewCount + 1 == gFileCount)
                            gFileViewCount = 0;
                        else
                            gFileViewCount++;
                    }
                    else
                    {
                        MessageBox.Show(" image file not found ");
                        this.Close();

                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmWorkInst_Load(object sender, EventArgs e)
        {
            gDb = new Database(BasicInfo.CONNSTRING);

            btn_Close.Visible = bBtnShow;

            gFolder = InitFrmWorkInstFile();

            Show_powerpoint();
        }

        private string InitFrmWorkInstFile()
        {
            DataTable table = new DataTable();
            string sTemp = string.Empty;
            string query = string.Empty;

            if( m_sPath.Trim().Contains("Spanish"))
            {
                query = string.Format(" Select file_name from mst_work_file where pc_code = '{0}' and file_type = 'E' "
                                              , BasicInfo.PCCD);
            }
            else{
                query = string.Format(" Select file_name from mst_work_file where pc_code = '{0}' and file_type = 'W' "
                                              , BasicInfo.PCCD);
            }

            try
            {
                gDb.GetData(query, ref table);

                if (table.Rows.Count > 0)
                {
                    sTemp = table.Rows[0]["file_name"].ToString().Trim();
                }
                else
                {
                    MessageBox.Show(" Work instruction file not found ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frmMain.LogMsg(" Work instruction file not found  ");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" FrmWorkInst Database Select Error !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                frmMain.LogMsg(" FrmWorkInst Database Select Error !! " + ex.Message);
                this.Close();
            }

            return sTemp;
        }

        private void picQC_Click(object sender, EventArgs e)
        {
            Show_powerpoint();
        }

        private void frmWorkInst_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picQC.Image != null)
                picQC.Image.Dispose();
        }
    }
}