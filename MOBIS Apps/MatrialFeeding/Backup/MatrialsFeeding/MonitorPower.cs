using System;
using System.Collections.Generic;
using System.Runtime.InteropServices; 

using System.Text;

namespace OMMCDP
{
    class MonitorPower
    {
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MONITORPOWER = 0xF170;

        const int MONITOR_ON = -1;
        const int MONITOR_OFF = 2;
        const int MONITOR_STANBY = 1;

        private int handle;

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);   

        public MonitorPower(int hWnd)
        {
            handle = hWnd;
        }

        //Off버튼을 누르면 LCD모니터 전원을 OFF시킴

        public void PowerOff()
        {
            SendMessage(handle, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF); 
        }

         public void PowerOn()
        {
            SendMessage(handle, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_ON); 
        }



    }

}

 


    

