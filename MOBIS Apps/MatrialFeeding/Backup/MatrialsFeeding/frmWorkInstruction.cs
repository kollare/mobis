using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using QuartzTypeLib;


namespace OMMCDP
{
    public partial class frmWorkInstruction : Form
    {
        #region 교육 동영상 관련 변수
        private const int WM_APP = 0x8000;
        private const int WM_GRAPHNOTIFY = WM_APP + 1;
        private const int EC_COMPLETE = 0x01;
        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPCHILDREN = 0x2000000;

        private FilgraphManager m_objFilterGraph = null;
        private IBasicAudio m_objBasicAudio = null;
        private IVideoWindow m_objVideoWindow = null;
        private IMediaEvent m_objMediaEvent = null;
        private IMediaEventEx m_objMediaEventEx = null;
        private IMediaPosition m_objMediaPosition = null;
        private IMediaControl m_objMediaControl = null;

        System.Windows.Forms.Panel pnlMovie = new Panel();

        //int Time_Check = 0;         // Time_Check = 동영상이 시작해서 얼마의 시간이 흘렀는가를 체크하는 변수
//        private string[] sMoviePath;    // 동영상 경로 저장
//        private int g_nMovieidx;          // 저장된 파일개수
//        private int g_nMovie = 0;       // 시작할 파일 번호
        enum MediaStatus { None, Stopped, Paused, Running };                // 상태플래그
        private MediaStatus m_CurrentStatus = MediaStatus.None;             // 상태플래그 초기화

 //       private int g_nMovieErr = 0;
        /// //////////////////////////////////////////////////
        #endregion

        private Database gdb;
        private string filename;

        public frmWorkInstruction()
        {
            InitializeComponent();
        }

        private void frmWorkInstruction_Load(object sender, EventArgs e)
        {
            gdb = new Database(BasicInfo.CONNSTRING);

            if (!CheckingFile())
            {
                MessageBox.Show(" Work Instrucktion not found ", " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            InitMovie();
        }

        private bool CheckingFile()
        {
            DataTable table = new DataTable();

            string query = string.Format(" Select File_name From mst_work_file where pc_code = '{0}' and file_type = 'V' ", BasicInfo.PCCD);

            try
            {
                gdb.GetData(query, ref table);

                if (table.Rows.Count > 0)
                {
                    filename = table.Rows[0]["file_name"].ToString().Trim();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                frmMain.LogMsg(" [frmWorkInstruction] DataBase Reading Error!! " + ex.Message);
                return false;
            }
        }

        // 동영상 파일 검색 및 시작 함수
        private void InitMovie()
        {
            Show_Movie(@"Z:\Line_PC\Video\" + filename);
        }

        public void Show_Movie(string sPath)
        {
            //tmr_Movie.Enabled = false;
            
            try
            {
                this.pnlMovie = new System.Windows.Forms.Panel();
                this.SuspendLayout();
                // 
                // pnlMovie
                // 
                this.pnlMovie.BackColor = System.Drawing.Color.Black;
                this.pnlMovie.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                this.pnlMovie.Location = new System.Drawing.Point(0, 0);
                this.pnlMovie.Name = "pnlTest";
                this.pnlMovie.Size = new System.Drawing.Size(1020, 527);
                this.pnlMovie.TabIndex = 0;

                this.Controls.Add(pnlMovie);

                m_objFilterGraph = new FilgraphManager();
                m_objFilterGraph.RenderFile(sPath);
                System.Threading.Thread.Sleep(200);
                m_objBasicAudio = m_objFilterGraph as IBasicAudio;

                m_objVideoWindow = m_objFilterGraph as IVideoWindow;

                m_objVideoWindow.Owner = (int)pnlMovie.Handle;
                m_objVideoWindow.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;
                m_objVideoWindow.SetWindowPosition(pnlMovie.ClientRectangle.Left,
                                                   pnlMovie.ClientRectangle.Top,
                                                   pnlMovie.ClientRectangle.Width,
                                                   pnlMovie.ClientRectangle.Height);


                m_objMediaEvent = m_objFilterGraph as IMediaEvent;
                m_objMediaEventEx = m_objFilterGraph as IMediaEventEx;
                m_objMediaEventEx.SetNotifyWindow((int)this.Handle, WM_GRAPHNOTIFY, 0);
                m_objMediaPosition = m_objFilterGraph as IMediaPosition;

                m_objMediaControl = m_objFilterGraph as IMediaControl;
                m_objVideoWindow = null;
                m_objMediaControl.Run();
                
                m_CurrentStatus = MediaStatus.Running;      // 플래그를 Running 으로 바꾼다.


            }
            catch (Exception ex)
            {
                frmMain.LogMsg("[frmWorkInstruction]" + ex.Message);
                m_objVideoWindow = null;
                this.Close();
            }

        }

        private void CleanUp()
        {
            if (m_objMediaControl != null)
                m_objMediaControl.Stop();

            m_CurrentStatus = MediaStatus.Stopped;

            if (m_objMediaEventEx != null)
                m_objMediaEventEx.SetNotifyWindow(0, 0, 0);

            if (m_objVideoWindow != null)
            {
                m_objVideoWindow.Visible = 0;
                m_objVideoWindow.Owner = 0;
            }

            if (m_objMediaControl != null) m_objMediaControl = null;
            if (m_objMediaPosition != null) m_objMediaPosition = null;
            if (m_objMediaEventEx != null) m_objMediaEventEx = null;
            if (m_objMediaEvent != null) m_objMediaEvent = null;
            if (m_objVideoWindow != null) m_objVideoWindow = null;
            if (m_objBasicAudio != null) m_objBasicAudio = null;
            if (m_objFilterGraph != null) m_objFilterGraph = null;
      
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                int IEventCode;
                int IParam1, IParam2;

                while (true)
                {
                    try
                    {
                        // 이벤트 큐에서 이벤트를 하나 꺼냄
                        m_objMediaEventEx.GetEvent(out IEventCode, out IParam1, out IParam2, 0);

                        // 꺼내온 이벤트의 매개변수에 할당 된 리소스를 해제
                        m_objMediaEventEx.FreeEventParams(IEventCode, IParam1, IParam2);


                        
                        if (IEventCode == EC_COMPLETE)
                        {
                            m_objMediaControl.StopWhenReady();

                            CleanUp();

                            this.pnlMovie.Dispose();
                            System.Threading.Thread.Sleep(100);

                            BasicInfo.WORKINSTRUCTION_FLAG = false;
                            this.Close();
 
                        }

                    }
                    catch (Exception) { break; }
                }
            }
            base.WndProc(ref m);
        }
    }
}