using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Deployment.Application;
using System.Diagnostics;

namespace OMMCDP
{
    static class Common
    {
        static public string LineCd = "";
        static public string StationCd = "";
        static public string Title = "";
        static public string ScannerPort = "";
        static public string ScannerBaud= "9600";
        static public string ConnectionString = "";
        static public string IPv4 = "";
        static public USER User = new USER();
        static public string PcKind = "";
        static public PC_INFO PC;
        

        #region Animate
        /// <summary>
        /// hwnd는 윈도우 핸들
        /// dwTime는 Animate 하는 시간
        /// dwFlags는 Animate 효과 플래그
        /// </summary>
        [DllImport("User32.dll", EntryPoint = "AnimateWindow")]
        static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        /// AnimateWindow
        public static bool SetAnimateWindow(IntPtr hwnd, int dwTime, ANIMATE_FLAG dwFlags)
        {
            return AnimateWindow(hwnd, dwTime, (int)dwFlags);
        }
        #endregion

        #region SetSystemTime
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime([In] ref SYSTEMTIME st);

        public static void SetLocalTime()
        {
            Database DB = new Database(Common.ConnectionString);
            DataTable table = new DataTable();
            string query = string.Empty;
            string sErrMsg = string.Empty;
            string sTime = string.Empty;

            try
            {
                query = "select convert(varchar, getdate(), 120) as dual";

                DB.Fill(query, ref table);

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
            catch (Exception e)
            {
                sErrMsg = "setLocalTime()" + e.Message;
                WriteLog(sErrMsg);
                return;
            }
        }
        #endregion

        #region Log File
        public static void WriteLog(string LogText)
        {
            try
            {
                string directory = string.Format("{0}{1:00}", DateTime.Now.Year, DateTime.Now.Month);
                //            string path = ".\\LOG\\" + directory + "\\";
                string path = "c:\\OMMCDP\\LinePPC_LOG\\" + directory + "\\";
                string fileName = string.Format("{0}-{1}-{2}.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string title = string.Format("{0,-23} : ", DateTime.Now);

                FileInfo file = new FileInfo(path + fileName);
                FileStream fs;


                if (!file.Exists) // 파일이 이나 디렉토리가 없으면
                {
                    System.IO.Directory.CreateDirectory(path);
                    fs = new FileStream(path + fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                }
                else
                {
                    fs = new FileStream(path + fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                }
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                if (LogText != "\r\n")
                    sw.WriteLine(title + LogText);
                else
                    sw.WriteLine(LogText);

                sw.Close();
                fs.Close();
            }
            catch
            {
            }

        }
        #endregion

        #region NetDrive
        /// 네트워크 드라이브 설정 
        public static void InitNetDrive()
        {
            try
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
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }


        }
        #endregion

        #region Shotcut
        /// 배포시 바탕화면에 shotcut 생성
        public static void CreateDesktopIcon()
        {
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                    if (ad.IsFirstRun)
                    {
                        string desktopPath = string.Empty;
                        desktopPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                            , "\\", "LinePPC", ".appref-ms");
                        string shotcutName = string.Empty;
                        shotcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs)
                            , "\\", "OMMCDP", "\\", "LinePPC", ".appref-ms");
                        File.Copy(shotcutName, desktopPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }


        }
        #endregion

        static public string AmericanDateTime(string sDate)
        {
            if (sDate.Length == 14)
            {
                string sYear = sDate.Substring(0, 4);
                string sMonth = sDate.Substring(4, 2);
                string sDay = sDate.Substring(6, 2);
                string sHour = sDate.Substring(8, 2);
                string sMin = sDate.Substring(10, 2);
                string sSec = sDate.Substring(12, 2);


                return sMonth + "/" + sDay + "/" + sYear + " " + sHour + ":" + sMin + ":" + sSec;
            }
            else
                return "";
        }

    }
}
