using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Deployment.Application;
using System.Diagnostics;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using System.Drawing;

namespace OMMCDP
{
    public delegate void ClickEventHandler(object sender, COMPONENT_RESULT result);
    public delegate void MessageEventHandler(MESSAGE_STATUS status, string message);
    public delegate void ToolEventHandler(TOOL_STATUS status);
    public delegate void ToolMessageEventHandler(TOOL_STATUS status, string message);
    public delegate void InvokeDelegate();
    public delegate void SupervisorEventHandler(int index, string data, int point);

    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }

    public struct PC_INFO
    {
        public string Kind;
        public string Index;
        public string Code;
        public string Name;
    }

    public class PCWORK
    {
        public PCWORK()
        {
            InitPcWork();
        }

        public void InitPcWork()
        {
            WorkCode = string.Empty;
            WorkIndex = string.Empty;
            WorkName = string.Empty;
            WorkType = WORK_TYPTE.None;
            WorkKind = string.Empty;
            ToolPlcTagCode = string.Empty;
            ToolIP = string.Empty;
            ToolPort = string.Empty;
            BctOrderColumn = string.Empty;
            ChechFlag = string.Empty;

            ScanPartNo = string.Empty;
            ScanSerial = string.Empty;
            PartNo1 = string.Empty;
            PartNo2 = string.Empty;
            BarcodeCount = 0;
            NowBarcodeCount = 0;
            SendFlag = false;
            WorkResult = WORK_RESULT.None;
            Torque = null;
            ToolPointCount = 0;
            ScopeMin = 0;
            ScopeMax = 0;

            RackCode = string.Empty;
            RackTotalCount = 0;
            RackCurrentCount = 0;

            Discription1 = string.Empty;
            DiscriptionForeColor1 = Color.Empty;
            DiscriptionBackColor1 = Color.Empty;

            Discription2 = string.Empty;
            DiscriptionForeColor2 = Color.Empty;
            DiscriptionBackColor2 = Color.Empty;
            
        }

        public string WorkCode;
        public string WorkIndex;
        public string WorkName;
        public string WorkType;
        public string WorkKind;
        public string ToolPlcTagCode;
        public string ToolIP;
        public string ToolPort;
        public string BctOrderColumn;
        public string ChechFlag;

        public string ScanPartNo;
        public string ScanSerial;
        public string PartNo1;
        public string PartNo2;
        public int BarcodeCount;
        public int NowBarcodeCount;
        public bool SendFlag;
        public WORK_RESULT WorkResult;
        public List<TORQUE> Torque;
        public int ToolPointCount;
        public double ScopeMin;
        public double ScopeMax;

        public string RackCode;
        public int RackTotalCount;
        public int RackCurrentCount;

        public string Discription1;
        public Color DiscriptionForeColor1;
        public Color DiscriptionBackColor1;

        public string Discription2;
        public Color DiscriptionForeColor2;
        public Color DiscriptionBackColor2;
    }

    public enum ANIMATE_FLAG
    {
        AW_HOR_POSITIVE = 0x00000001,
        AW_HOR_NEGATIVE = 0x00000002,
        AW_VER_POSITIVE = 0x00000004,
        AW_VER_NEGATIVE = 0x00000008,
        AW_CENTER = 0x00000010,
        AW_HIDE = 0x00010000,
        AW_ACTIVATE = 0x00020000,
        AW_SLIDE = 0x00040000,
        AW_BLEND = 0x00080000,
    }

    public class USER
    {
        public USER()
        {
            UserLevel = USER_LEVEL.None;
            UserID = "";
            SupervisorID = "";
            QualityID = "";
        }
        public USER_LEVEL UserLevel = USER_LEVEL.None;
        public string UserID = "";
        public string SupervisorID = "";
        public string QualityID = "";
    }

    public enum USER_LEVEL
    {
        None,
        GeneralUser,
        Supervisor,
    }

    public enum COMPONENT_RESULT
    {
        None,
        Close,
        Ok,
        No,
        Yes,
        Cancel,
    }

    public enum MESSAGE_STATUS
    {
        Normal,
        Success,
        Error,
    }

    public enum TOOL_STATUS
    {
        Normal,
        None,
        Error,
    }

    public enum CHECK_STORAGE
    {
        None,
        Checking,
        Done,
    }

    public enum DISPLAY_PALLET_ERROR
    {
        None,
        Done,
    }

    public enum WORK_RESULT
    {
        None,
        OK,
        NG,
        Pass,
    }

    public enum TORQUE_RESULT
    {
        None,
        OK,
        NG,
    }

    public class TORQUE
    {
        public TORQUE()
        {
            Value = "";
            Result = TORQUE_RESULT.None;
        }

        public string Value = "";
        public TORQUE_RESULT Result = TORQUE_RESULT.None; 
    }

    public struct WORK_TYPTE
    {
        public const string None = "";
        public const string Print_P = "P";
        public const string Inspection_I = "I";
        public const string Torque_T = "T";
        public const string TorqueWorkStep_U = "U";
        public const string RearCorner120Torque_L = "L";  // from PLC (LEAK TESTER) 
        public const string RecieveDataFromPLC_F = "F";   // from PLC (ETC)
        public const string Break_Main_C = "C";
        public const string Scan_S = "S";
        public const string Scan_7 = "7";
        public const string SmartGate = "R";
        public const string PickLight_N = "N";
        public const string Matching_M = "M";
        public const string CommonDiscription_Y = "Y";
        public const string PassStation_G = "G";
        public const string MainBarCode_K = "K";
        public const string Matching_9 = "9";
        public const string JSNScan_J = "J";
        public const string ScanTcodeM_B = "B";
        public const string ScanBuildInBarcode_E = "E";
        public const string LeakTester_6 = "6";
        public const string View_Load_Position_2 = "2";
    }

    //public class DISPLAY_PCWORK
    //{
    //    public DISPLAY_PCWORK(UcDisplayJob ucWorkDisplay)
    //    {
    //        WorkDisplay = ucWorkDisplay;
    //    }

    //    public UcDisplayJob WorkDisplay;
    //}
}
