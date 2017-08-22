using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InfoBoard
{
    public partial class frmMain : Form
    {
        int iYellow;
        int iRed;

        int iYellow2;
        int iRed2;

        bool bAlarm = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SetSheet();
            GetSetting();
            
            tmrGetData.Enabled = true;
            tmrGetData.Interval = 15000;
            tmrGetData.Start();

            tmrUpdate.Enabled = true;
            tmrUpdate.Interval = 15000;
            tmrUpdate.Start();

            GetData();
            UpdateBuildData();

            tmrSetTime.Enabled = true;
            tmrSetTime.Interval = 1000;
            tmrSetTime.Start();
        }

        private void SetSheet()
        {
            fpSpread1.Sheets[0].Cells[0, 0].Text = "FCA Cnt/Shift";

            fpSpread1.Sheets[0].Cells[0, 1].ColumnSpan = 7;
            fpSpread1.Sheets[0].Cells[0, 1].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[0, 1].Text = "Production Information\n ";

            fpSpread1.Sheets[0].Cells[0, 8].ColumnSpan = 2;
            //fpSpread1.Sheets[0].Cells[0, 8].Text = "MOBIS Cnt/Time of Day";
            fpSpread1.Sheets[0].Cells[0, 8].Text = "FCA JPH";

            fpSpread1.Sheets[0].Cells[1, 8].ColumnSpan = 2;

            fpSpread1.Sheets[0].Cells[2, 0].RowSpan = 2;

            fpSpread1.Sheets[0].Cells[2, 1].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 1].Text = "Front";

            fpSpread1.Sheets[0].Cells[2, 2].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 2].Text = "Rear";

            fpSpread1.Sheets[0].Cells[2, 3].ColumnSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 3].Text = "FCOS";
            fpSpread1.Sheets[0].Cells[3, 3].Text = "LH";
            fpSpread1.Sheets[0].Cells[3, 4].Text = "RH";

            fpSpread1.Sheets[0].Cells[2, 5].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 5].Text = "Rear\nShock";

            fpSpread1.Sheets[0].Cells[2, 6].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 6].Text = "Rear\nSpring";

            fpSpread1.Sheets[0].Cells[2, 7].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 7].Text = "Front\nCradle";

            fpSpread1.Sheets[0].Cells[2, 8].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 8].Text = "Rear\nCradle";

            fpSpread1.Sheets[0].Cells[2, 9].RowSpan = 2;
            fpSpread1.Sheets[0].Cells[2, 9].Text = "Rear\nKnuckle";

            fpSpread1.Sheets[0].Cells[4, 0].Text = "Broadcast";
            fpSpread1.Sheets[0].Cells[5, 0].Text = "Cnt/Shift";
            fpSpread1.Sheets[0].Cells[6, 0].Text = "GAP";
            fpSpread1.Sheets[0].Cells[7, 0].Text = "Cnt/Hr";
            fpSpread1.Sheets[0].Cells[8, 0].Text = "JPH";
            fpSpread1.Sheets[0].Cells[9, 0].Text = "Internal";
            fpSpread1.Sheets[0].Cells[10, 0].Text = "In Transit";
            fpSpread1.Sheets[0].Cells[11, 0].Text = "External";
        }

        private void GetSetting()
        {
            string sQuery = "SELECT VALUE_01, VALUE_02, VALUE_03, VALUE_04 FROM MST_COMMON WITH (NOLOCK) WHERE COMMON_GROUP = 'BC_ALERT' ORDER BY COMMON_CODE";
            DataTable dt = Program.QueryExecute(sQuery, null);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iRed = Int32.Parse(dt.Rows[i][0].ToString());
                    iYellow = Int32.Parse(dt.Rows[i][1].ToString());
                    iRed2 = Int32.Parse(dt.Rows[i][2].ToString());
                    iYellow2 = Int32.Parse(dt.Rows[i][3].ToString());
                }
            }
        }

        private void tmrGetData_Tick(object sender, EventArgs e)
        {
            tmrGetData.Stop();
            GetData();
            tmrGetData.Start();
        }

        private void UpdateBuildData()
        {
            try
            {
                string sTime = "";
                string sCurTime = "";
                string sPltDate = "";
                string sFirst = "";
                string sSecond = "";

                string sCountShiftUpdate = "";
                string sCountShiftQuery = "";
                string sChryslerQuery = "";
                string sChryslerUpdate = "";
                string sBroadcastQuery = "";
                string sBroadcastUpdate = "";
                string sGapUpdate = "";
                string sInternalInv = "";
                string sInternalUpdate = "";
                string sExternalInv = "";
                string sExternalUpdate = "";
                string sInTransit = "";
                string sInTransitlUpdate = "";
                string sGetJPH = "";
                string sGetJPHUpdate = "";
                string sCountDayUpdate = "";

                string sQuery = "";

                // Get GAP Order
                int[] aryGapOrder = { 0, 0, 0, 0, 0 };
                string sGapOrder = "SELECT VALUE_01 FROM MST_COMMON WITH (NOLOCK) WHERE COMMON_GROUP = 'BC_GAP' ORDER BY COMMON_CODE";

                DataTable dtGapOrder = Program.QueryExecute(sGapOrder, null);
                if (dtGapOrder.Rows.Count > 0)
                {
                    for (int i = 0; i < dtGapOrder.Rows.Count; i++)
                        aryGapOrder[i] = Int32.Parse(dtGapOrder.Rows[i][0].ToString());
                }

                // Get Current Time
                string sGetTime = "SELECT CONVERT(VARCHAR(20),GETDATE(),20) AS CURDATE";

                DataTable dtTime = Program.QueryExecute(sGetTime, null);
                if (string.IsNullOrEmpty(dtTime.Rows[0][0].ToString()))
                    sCurTime = DateTime.Now.ToString("yyyymmddhhMMss");
                else
                    sCurTime = dtTime.Rows[0][0].ToString().Substring(0, 4) + // Year
                        dtTime.Rows[0][0].ToString().Substring(5, 2) + // Month
                        dtTime.Rows[0][0].ToString().Substring(8, 2) + // Day
                        dtTime.Rows[0][0].ToString().Substring(11, 2) + // Hour
                        dtTime.Rows[0][0].ToString().Substring(14, 2) + // Minute
                        dtTime.Rows[0][0].ToString().Substring(17, 2); // Second

                // Get Fist, Second Time
                string sGetWorkPattern = "Select SHIFT_01_START, SHIFT_02_START, dbo.uf_PlantDate() AS PLTDATE From MST_PATTERN WITH (NOLOCK) Where PATTERN_CODE = (Select PATTERN_CODE From MST_DAY_PATTERN Where PROD_DATE = (Select dbo.uf_PlantDate()))";

                DataTable dtWorkPattern = Program.QueryExecute(sGetWorkPattern, null);
                if (dtWorkPattern.Rows.Count > 0)
                {
                    sFirst = dtWorkPattern.Rows[0][0].ToString();
                    sSecond = dtWorkPattern.Rows[0][1].ToString();
                    sPltDate = dtWorkPattern.Rows[0][2].ToString();
                }
                else
                {
                    sFirst = "0600";
                    sSecond = "1800";
                    sPltDate = DateTime.Now.ToString("yyyymmdd");
                }

                if (Int32.Parse(sCurTime.Substring(8, 4).ToString()) >= Int32.Parse(sFirst) && Int32.Parse(sCurTime.Substring(8, 4).ToString()) < Int32.Parse(sSecond))
                    sTime = sFirst;
                else
                    sTime = sSecond;

                int iStartPoint = ((Int32.Parse(sTime.Substring(0, 2))) - 5) * 12 + Int32.Parse(sTime.Substring(2, 2)) / 5 + 1;

                // Chrysler
                sChryslerQuery = "Select Count(*) As CNT From MQT_GSP_X84 WITH (NOLOCK) Where RECEIVE_TIME >= (Select dbo.uf_PlantDate()+'" + sTime + "00')";

                DataTable dtChrysler = Program.QueryExecute(sChryslerQuery, null);

                sChryslerUpdate = "Update WKH_BOARD " +
                    "Set FRONT = '" + dtChrysler.Rows[0][0].ToString() + "', " +
                    "REAR = '" + dtChrysler.Rows[0][0].ToString() + "', " +
                    "FCOSL = '" + dtChrysler.Rows[0][0].ToString() + "', " +
                    "FCOSR = '" + dtChrysler.Rows[0][0].ToString() + "', " +
                    "RSHOCK = '" + dtChrysler.Rows[0][0].ToString() + "', " +
                    "RSPRING = '" + dtChrysler.Rows[0][0].ToString() + "' " +
                    "Where CLASS = 'CHRYSLER'";
                Program.QueryExecute(sChryslerUpdate, null);

                Program.QueryExecute2nd(sChryslerUpdate, null);


                // Broadcast
                sBroadcastQuery = "Select * From (Select '1' as IDX, Count(*)+(Select Count(*) From BCT_CORNER_X84 WITH (NOLOCK) Where CREATE_FLAG = '0')+(Select Count(*) From BCT_ORDER_X84 WITH (NOLOCK) Where FLAG03 = '0') As CNT " +
                    "From MQT_SP_X84 WITH (NOLOCK) Where FLAG01 = '1' And FLAG02 = '0' And CANCEL_FLAG = '0') a " +
                    "UNION " +
                    "Select * From (Select '2' as IDX, Count(*)+(Select Count(*) From BCT_CORNER_X85 WITH (NOLOCK) Where CREATE_FLAG = '0')+(Select Count(*) From BCT_ORDER_X85 WITH (NOLOCK) Where FLAG03 = '0') As CNT " +
                    "From MQT_SP_X85 WITH (NOLOCK) Where FLAG01 = '1' And FLAG02 = '0' And CANCEL_FLAG = '0') b " +
                    "UNION " +
                    "Select * From (Select '3' as IDX, Count(*)+(Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where FLAG03 Is NULL) As CNT " +
                    "From MQT_SP_X89 WITH (NOLOCK) Where FLAG01 = '1' And FLAG02 = '0' And CANCEL_FLAG = '0'  ) c  " +
                    "UNION " +
                    "Select * From (Select '4' as IDX, Count(*)+(Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where FLAG04 Is NULL) As CNT " +
                    "From MQT_SP_X89 WITH (NOLOCK) Where FLAG01 = '1' And FLAG02 = '0' And CANCEL_FLAG = '0'  ) d " +
                    "UNION " +
                    "Select * From (Select '5' as IDX, Count(*)+(Select Count(*) From BCT_ORDER_X91A WITH (NOLOCK) Where FLAG03 Is NULL) As CNT " +
                    "From MQT_SP_X91 WITH (NOLOCK) Where FLAG01 = '1' And FLAG02 = '0' And CANCEL_FLAG = '0'  ) e " +
                    "UNION " +
                    "Select * From (Select '6' as IDX, Count(*)+(Select Count(*) From BCT_ORDER_X91B WITH (NOLOCK) Where FLAG03 Is NULL) As CNT " +
                    "From MQT_SP_X91 WITH (NOLOCK) Where FLAG01 = '1' And FLAG03 = '0' And CANCEL_FLAG = '0'  ) f   " +
                    "Order by IDX ASC";

                DataTable dtBroadcast = Program.QueryExecute(sBroadcastQuery, null);
                if (dtBroadcast.Rows.Count > 0)
                {
                    sBroadcastUpdate = "Update WKH_BOARD " +
                        "Set FRONT = '" + dtBroadcast.Rows[0][1].ToString() + "', " +
                        "REAR = '" + dtBroadcast.Rows[1][1].ToString() + "', " +
                        "FCOSL = '" + dtBroadcast.Rows[2][1].ToString() + "', " +
                        "FCOSR = '" + dtBroadcast.Rows[3][1].ToString() + "', " +
                        "RSHOCK = '" + dtBroadcast.Rows[4][1].ToString() + "', " +
                        "RSPRING = '" + dtBroadcast.Rows[5][1].ToString() + "' " +
                        "Where CLASS = 'BBO'";
                    Program.QueryExecute(sBroadcastUpdate, null);

                    Program.QueryExecute2nd(sBroadcastUpdate, null);
                }

                // Count/Shift
                sCountShiftQuery = "Select ( Select Count(*) From BCT_ORDER_X84 WITH (NOLOCK) Where BUILD_OUT_TIME >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As FRONT, " +
                    "( Select Count(*) From BCT_ORDER_X85 WITH (NOLOCK) Where BUILD_OUT_TIME >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As REAR, " +
                    "( Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where WORK_TIME06 >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As FRTL, " +
                    "( Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where WORK_TIME05 >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As FRTR, " +
                    "( Select Count(*) From BCT_ORDER_X91A WITH (NOLOCK) Where BUILD_OUT_TIME >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As RRSHOCK, " +
                    "( Select Count(*) From BCT_ORDER_X91B WITH (NOLOCK) Where BUILD_OUT_TIME >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As RRSPRING, " +
                    "( Select Count(*) From PDT_RESULT_ETC Where LINE_CODE = 'X84' And STATION_CODE = 'M038' And RESULT_TIME01 >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As FCRADLE, " +
                    "( Select Count(*) From PDT_RESULT_ETC Where LINE_CODE = 'X85' And STATION_CODE = 'P010' And RESULT_TIME01 >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As RCRADLE, " +
                    "( Select Count(*) From PDT_RESULT_ETC Where LINE_CODE = 'X85' And STATION_CODE = 'B035' And RESULT_TIME01 >= (Select dbo.uf_PlantDate()+'" + sTime + "00') ) As RKNUCKLE";

                DataTable dtCountShift = Program.QueryExecute(sCountShiftQuery, null);
                if (dtCountShift.Rows.Count > 0)
                {
                    sCountShiftUpdate = "Update WKH_BOARD " +
                        "Set FRONT = '" + dtCountShift.Rows[0][0].ToString() + "', " +
                        "REAR = '" + dtCountShift.Rows[0][1].ToString() + "', " +
                        "FCOSL = '" + dtCountShift.Rows[0][2].ToString() + "', " +
                        "FCOSR = '" + dtCountShift.Rows[0][3].ToString() + "', " +
                        "RSHOCK = '" + dtCountShift.Rows[0][4].ToString() + "', " +
                        "RSPRING = '" + dtCountShift.Rows[0][5].ToString() + "', " +
                        "F_CRADLE = '" + dtCountShift.Rows[0][6].ToString() + "', " +
                        "R_CRADLE = '" + dtCountShift.Rows[0][7].ToString() + "', " +
                        "R_KNUCKLE = '" + dtCountShift.Rows[0][8].ToString() + "' " +
                        "Where CLASS = 'COUNTSHIFT'";

                    Program.QueryExecute(sCountShiftUpdate, null);

                    Program.QueryExecute2nd(sCountShiftUpdate, null);
                }

                // GAP
                sGapUpdate = "Update WKH_BOARD Set " +
                    "FRONT = '" + (Int32.Parse(dtCountShift.Rows[0][0].ToString()) - Int32.Parse(dtChrysler.Rows[0][0].ToString())).ToString() + "', " +
                    "REAR = '" + (Int32.Parse(dtCountShift.Rows[0][1].ToString()) - Int32.Parse(dtChrysler.Rows[0][0].ToString())).ToString() + "', " +
                    "FCOSL = '" + (Int32.Parse(dtCountShift.Rows[0][2].ToString()) - Int32.Parse(dtChrysler.Rows[0][0].ToString())).ToString() + "', " +
                    "FCOSR = '" + (Int32.Parse(dtCountShift.Rows[0][3].ToString()) - Int32.Parse(dtChrysler.Rows[0][0].ToString())).ToString() + "', " +
                    "RSHOCK = '" + (Int32.Parse(dtCountShift.Rows[0][4].ToString()) - Int32.Parse(dtChrysler.Rows[0][0].ToString())).ToString() + "', " +
                    "RSPRING = '" + (Int32.Parse(dtCountShift.Rows[0][5].ToString()) - Int32.Parse(dtChrysler.Rows[0][0].ToString())).ToString() + "', " +
                    "F_CRADLE = '" + (Int32.Parse(dtCountShift.Rows[0][6].ToString()) - Int32.Parse(dtCountShift.Rows[0][0].ToString())).ToString() + "', " +
                    "R_CRADLE = '" + (Int32.Parse(dtCountShift.Rows[0][7].ToString()) - Int32.Parse(dtCountShift.Rows[0][1].ToString())).ToString() + "', " +
                    "R_KNUCKLE = '" + (Int32.Parse(dtCountShift.Rows[0][8].ToString()) - Int32.Parse(dtCountShift.Rows[0][1].ToString())).ToString() + "' " +
                    "Where CLASS = 'GAP'";

                Program.QueryExecute(sGapUpdate, null);

                Program.QueryExecute2nd(sGapUpdate, null);

                // Count/Hour
                string sCntTime = "";
                string sCntTimeQuery = "SELECT CONVERT(VARCHAR(20),DATEADD(HOUR,-1,GETDATE()),20) AS CURDATE";
                DataTable dtCntTime = Program.QueryExecute(sCntTimeQuery, null);
                if (dtCntTime.Rows.Count > 0)
                    sCntTime = dtCntTime.Rows[0][0].ToString().Substring(0, 4) + // Year
                        dtCntTime.Rows[0][0].ToString().Substring(5, 2) + // Month
                        dtCntTime.Rows[0][0].ToString().Substring(8, 2) + // Day
                        dtCntTime.Rows[0][0].ToString().Substring(11, 2) + // Hour
                        dtCntTime.Rows[0][0].ToString().Substring(14, 2) + // Minute
                        dtCntTime.Rows[0][0].ToString().Substring(17, 2); // Second

                string sCountHour = "Select " +
                    "( Select Count(*) From BCT_ORDER_X84 WITH (NOLOCK) Where BUILD_OUT_TIME >= '" + sCntTime + "' ) As FRONT, " +
                    "( Select Count(*) From BCT_ORDER_X85 WITH (NOLOCK) Where BUILD_OUT_TIME >= '" + sCntTime + "' ) As REAR, " +
                    "( Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where WORK_TIME06 >= '" + sCntTime + "' ) As FRTL, " +
                    "( Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where WORK_TIME05 >= '" + sCntTime + "' ) As FRTR, " +
                    "( Select Count(*) From BCT_ORDER_X91A WITH (NOLOCK) Where BUILD_OUT_TIME >= '" + sCntTime + "' ) As RRSHOCK, " +
                    "( Select Count(*) From BCT_ORDER_X91B WITH (NOLOCK) Where BUILD_OUT_TIME >= '" + sCntTime + "' ) As RRSPRING, " +
                    "( Select Count(*) From PDT_RESULT_ETC Where LINE_CODE = 'X84' And STATION_CODE = 'M038' And RESULT_TIME01 >= '" + sCntTime + "' ) As FCRADLE, " +
                    "( Select Count(*) From PDT_RESULT_ETC Where LINE_CODE = 'X85' And STATION_CODE = 'P010' And RESULT_TIME01 >= '" + sCntTime + "' ) As RCRADLE, " +
                    "( Select Count(*) From PDT_RESULT_ETC Where LINE_CODE = 'X85' And STATION_CODE = 'B035' And RESULT_TIME01 >= '" + sCntTime + "' ) As RKNUCLE ";

                DataTable dtCountHour = Program.QueryExecute(sCountHour, null);
                if (dtCountHour.Rows.Count > 0)
                {
                    string sCountHourUpdate = "Update WKH_BOARD Set " +
                        "FRONT = '" + dtCountHour.Rows[0][0].ToString() + "',  " +
                        "REAR = '" + dtCountHour.Rows[0][1].ToString() + "',  " +
                        "FCOSL = '" + dtCountHour.Rows[0][2].ToString() + "',  " +
                        "FCOSR = '" + dtCountHour.Rows[0][3].ToString() + "',  " +
                        "RSHOCK = '" + dtCountHour.Rows[0][4].ToString() + "',  " +
                        "RSPRING = '" + dtCountHour.Rows[0][5].ToString() + "', " +
                        "F_CRADLE = '" + dtCountHour.Rows[0][6].ToString() + "',  " +
                        "R_CRADLE = '" + dtCountHour.Rows[0][7].ToString() + "', " +
                        "R_KNUCKLE = '" + dtCountHour.Rows[0][8].ToString() + "' " +
                        "Where CLASS = 'COUNTHOUR'";
                    Program.QueryExecute(sCountHourUpdate, null);

                    Program.QueryExecute2nd(sCountHourUpdate, null);
                }

                // Get Break Time
                //Front Module 
                int nBFrt = 0;
                sQuery = string.Format("EXEC dbo.pr_GetBreakTimeResult '{0}', 'BUILD_OUT_TIME', 'BCT_ORDER_X84' ", sTime);
                DataTable dtBreakFrt = Program.QueryExecute(sQuery, null);
                if (dtBreakFrt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtBreakFrt.Rows.Count; i++)
                        if (!dtBreakFrt.Rows[i][0].ToString().Equals("11"))
                        {
                            nBFrt = nBFrt + Int32.Parse(dtBreakFrt.Rows[i][1].ToString());
                        }                    
                }

                
                int nBRear = 0;
                sQuery = string.Format("EXEC dbo.pr_GetBreakTimeResult '{0}', 'BUILD_OUT_TIME', 'BCT_ORDER_X85' ", sTime);
                DataTable dtBreakRear = Program.QueryExecute(sQuery, null);
                if (dtBreakRear.Rows.Count > 0)
                {
                    for (int i = 0; i < dtBreakRear.Rows.Count; i++)
                        if (!dtBreakRear.Rows[i][0].ToString().Equals("11"))
                        {
                            nBRear = nBRear + Int32.Parse(dtBreakRear.Rows[i][1].ToString());
                        }
                }


                // JPH
                string sPltStart = sPltDate.Substring(0, 4) + "-" + sPltDate.Substring(4, 2) + "-" + sPltDate.Substring(6, 2) + " " + sTime.Substring(0, 2) + ":" + sTime.Substring(2, 2) + ":00";
                sGetJPH = "SELECT (LEN(REPLACE(SUBSTRING(PATTERN_DATA + PATTERN_DATA, " + iStartPoint.ToString() + ", DateDiff(s,convert(datetime, '" + sPltStart + "'),getdate()) / 60/ 5 ),'0',''))*60*5 +  " +
                    "CASE WHEN SUBSTRING(PATTERN_DATA + PATTERN_DATA, DateDiff(s,convert(datetime, '" + sPltStart + "'),getdate()) / 60/ 5 + " + iStartPoint.ToString() + ", 1) = '1' " +
                    "THEN DateDiff(s,convert(datetime, '" + sPltStart + "'),getdate()) - DateDiff(s,convert(datetime, '" + sPltStart + "'),getdate()) / 60/ 5 * 60 *5  " +
                    "ELSE 0 END) As SEC " +
                    "FROM MST_DAY_PATTERN A WITH (NOLOCK), MST_PATTERN B WITH (NOLOCK) " +
                    "WHERE A.PATTERN_CODE = B.PATTERN_CODE AND PROD_DATE = '" + sPltDate + "' ";

                DataTable dtGetJPH = Program.QueryExecute(sGetJPH, null);
                if (Int32.Parse(dtGetJPH.Rows[0][0].ToString()) > 0)
                {
                    string sFront = "";
                    string sRear = "";
                    string sFCOSL = "";
                    string sFCOSR = "";
                    string sRSHOCK = "";
                    string sRSPRING = "";
                    string sFCRADLE = "";
                    string sRCRADLE = "";
                    string sRKNUCKLE = "";

                    //Double d = Double.Parse(dtCountShift.Rows[0][0].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    Double d = (Double.Parse(dtCountShift.Rows[0][0].ToString()) - nBFrt)/ (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sFront = "0.0";
                    else
                        sFront = d.ToString("##.#");

                    //d = Double.Parse(dtCountShift.Rows[0][1].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    d = (Double.Parse(dtCountShift.Rows[0][1].ToString()) - nBRear) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sRear = "0.0";
                    else
                        sRear = d.ToString("##.#");

                    d = Double.Parse(dtCountShift.Rows[0][2].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sFCOSL = "0.0";
                    else
                        sFCOSL = d.ToString("##.#");

                    d = Double.Parse(dtCountShift.Rows[0][3].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sFCOSR = "0.0";
                    else
                        sFCOSR = d.ToString("##.#");

                    d = Double.Parse(dtCountShift.Rows[0][4].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sRSHOCK = "0.0";
                    else
                        sRSHOCK = d.ToString("##.#");

                    d = Double.Parse(dtCountShift.Rows[0][5].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sRSPRING = "0.0";
                    else
                        sRSPRING = d.ToString("##.#");

                    // Front Cradle
                    d = Double.Parse(dtCountShift.Rows[0][6].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sFCRADLE = "0.0";
                    else
                        sFCRADLE = d.ToString("##.#");

                    // Rear Cradle
                    d = Double.Parse(dtCountShift.Rows[0][7].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sRCRADLE = "0.0";
                    else
                        sRCRADLE = d.ToString("##.#");

                    // Rear Knuckle
                    d = Double.Parse(dtCountShift.Rows[0][8].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                    if (string.IsNullOrEmpty(d.ToString("##.#")))
                        sRKNUCKLE = "0.0";
                    else
                        sRKNUCKLE = d.ToString("##.#");

                    sGetJPHUpdate = "Update WKH_BOARD Set " +
                        "FRONT = '" + sFront + "',  " +
                        "REAR = '" + sRear + "',  " +
                        "FCOSL = '" + sFCOSL + "',  " +
                        "FCOSR = '" + sFCOSR + "',  " +
                        "RSHOCK = '" + sRSHOCK + "',  " +
                        "RSPRING = '" + sRSPRING + "',  " +
                        "F_CRADLE = '" + sFCRADLE + "',  " +
                        "R_CRADLE = '" + sRCRADLE + "',  " +
                        "R_KNUCKLE = '" + sRKNUCKLE + "'  " +
                        "Where CLASS = 'JPH'";
                }
                else
                {
                    sGetJPHUpdate = "Update WKH_BOARD Set " +
                        "FRONT = '0.0',  " +
                        "REAR = '0.0',  " +
                        "FCOSL = '0.0',  " +
                        "FCOSR = '0.0',  " +
                        "RSHOCK = '0.0',  " +
                        "RSPRING = '0.0',  " +
                        "F_CRADLE = '0.0',  " +
                        "R_CRADLE = '0.0',  " +
                        "R_KNUCKLE = '0.0' " +
                        "Where CLASS = 'JPH'";
                }

                Program.QueryExecute(sGetJPHUpdate, null);

                Program.QueryExecute2nd(sGetJPHUpdate, null);

                // Count/Time of Day --> FCA JPH

                sCountDayUpdate = "Update WKH_BOARD Set " +
                    //"FRONT = '" + (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 54.5).ToString("##") + "',  " +
                    //"REAR = '" + (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 54.5).ToString("##") + "',  " +
                    //"FCOSL = '" + (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 54.5).ToString("##") + "',  " +
                    //"FCOSR = '" + (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 54.5).ToString("##") + "',  " +
                    //"RSHOCK = '" + (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 54.5).ToString("##") + "',  " +
                    "RSPRING = '" + (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 54.5).ToString("##") + "'  " +
                    "Where CLASS = 'MOBIS'";
                Program.QueryExecute(sCountDayUpdate, null);

                string sFCAJPH = "0.0";
                Double dFCA = Double.Parse(dtChrysler.Rows[0][0].ToString()) / (Double.Parse(dtGetJPH.Rows[0][0].ToString()) / 3600);
                if (string.IsNullOrEmpty(dFCA.ToString("##.#")))
                    sFCAJPH = "0.0";
                else
                    sFCAJPH = dFCA.ToString("##.#");

                sCountDayUpdate = "Update WKH_BOARD Set " +
                    "FRONT = '" + sFCAJPH + "',  " +
                    "REAR = '" + sFCAJPH + "',  " +
                    "FCOSL = '" + sFCAJPH + "',  " +
                    "FCOSR = '" + sFCAJPH + "',  " +
                    "RSHOCK = '" + sFCAJPH + "'  " +
                    //"RSPRING = '" + sFCAJPH + "'  " +
                    "Where CLASS = 'MOBIS'";
                Program.QueryExecute(sCountDayUpdate, null);

                Program.QueryExecute2nd(sCountDayUpdate, null);

                // Internal
                sInternalInv = "Select " +
                    "( Select Count(*) From BCT_ORDER_X84 WITH (NOLOCK) Where BUILD_OUT_TIME Is Not NULL And TRAILER_TIME Is NULL ) As FRONT, " +
                    "( Select Count(*) From BCT_ORDER_X85 WITH (NOLOCK) Where BUILD_OUT_TIME Is Not NULL And TRAILER_TIME Is NULL ) As REAR, " +
                    "( Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where WORK_TIME06 Is Not NULL And TRAILER_TIME Is NULL ) As FRTL, " +
                    "( Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where WORK_TIME05 Is Not NULL And TRAILER_TIME Is NULL ) As FRTR, " +
                    "( Select Count(*) From BCT_ORDER_X91A WITH (NOLOCK) Where WORK_TIME04 Is Not NULL And TRAILER_TIME Is NULL ) As RRSHOCK, " +
                    "( Select Count(*) From BCT_ORDER_X91B WITH (NOLOCK) Where BUILD_OUT_TIME Is Not NULL And TRAILER_TIME Is NULL  ) As RRSPRING ";

                DataTable dtInternal = Program.QueryExecute(sInternalInv, null);
                if (dtInternal.Rows.Count > 0)
                {
                    sInternalUpdate = "Update WKH_BOARD Set " +
                        "FRONT = '" + dtInternal.Rows[0][0].ToString() + "',  " +
                        "REAR = '" + dtInternal.Rows[0][1].ToString() + "',  " +
                        "FCOSL = '" + dtInternal.Rows[0][2].ToString() + "',  " +
                        "FCOSR = '" + dtInternal.Rows[0][3].ToString() + "',  " +
                        "RSHOCK = '" + dtInternal.Rows[0][4].ToString() + "',  " +
                        "RSPRING = '" + dtInternal.Rows[0][5].ToString() + "'  " +
                        "Where CLASS = 'INTERNAL'";
                    Program.QueryExecute(sInternalUpdate, null);

                    Program.QueryExecute2nd(sInternalUpdate, null);
                }


                // In Transit
                sInTransit = "Select " +
                    "(select isnull(sum(shipping_qty),0) from pdh_intransit where Line_code='X84' and FLAG01 = '0') FRONT, " +
                    "(select isnull(sum(shipping_qty),0) from pdh_intransit where Line_code='X85' and FLAG01 = '0') REAR, " +
                    "(select isnull(sum(shipping_qty),0) from pdh_intransit where Line_code='X89' and FLAG01 = '0') FCOS, " +
                    "(select isnull(sum(shipping_qty),0) from pdh_intransit where Line_code='X91A' and FLAG01 = '0') RSHOCK, " +
                    "(select isnull(sum(shipping_qty),0) from pdh_intransit where Line_code='X91B' and FLAG01 = '0') RSPRING" +
                    " ";


                DataTable dtInTransit = Program.QueryExecute(sInTransit, null);
                if (dtInTransit.Rows.Count > 0)
                {
                    sInTransitlUpdate = "Update WKH_BOARD Set " +
                        "FRONT = '" + dtInTransit.Rows[0][0].ToString() + "',  " +
                        "REAR = '" + dtInTransit.Rows[0][1].ToString() + "',  " +
                        "FCOSL = '" + dtInTransit.Rows[0][2].ToString() + "',  " +
                        "FCOSR = '" + dtInTransit.Rows[0][2].ToString() + "',  " +
                        "RSHOCK = '" + dtInTransit.Rows[0][3].ToString() + "',  " +
                        "RSPRING = '" + dtInTransit.Rows[0][4].ToString() + "'  " +
                        "Where CLASS = 'INTRANSIT' ";
                    Program.QueryExecute(sInTransitlUpdate, null);
 
                    Program.QueryExecute2nd(sInTransitlUpdate, null);
                }


                // External
                sExternalInv = "Select " +
                    /*
                    "(Select Count(*) From BCT_ORDER_X84 WITH (NOLOCK) Where TRAILER_TIME Is Not NULL and seq_IDX > '201409020001' And Right(SEQ_NO,6) >= RIGHT((Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc), 6)) As Front, " +
                    "(Select Count(*) From BCT_ORDER_X85 WITH (NOLOCK) Where TRAILER_TIME Is Not NULL and seq_IDX > '201409020001' And Right(SEQ_NO,6) >= RIGHT((Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc), 6)) As Rear, " +
                    "(Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where TRAILER_TIME Is Not NULL and seq_IDX > '201409020001' And Right(SEQ_NO,6) >= RIGHT((Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc), 6)) As FCOS, " +
                    "(Select Count(*) From BCT_ORDER_X91A WITH (NOLOCK) Where TRAILER_TIME Is Not NULL and seq_IDX > '201409020001' And Right(SEQ_NO,6) >= RIGHT((Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc), 6)) As RrShock, " +
                    "(Select Count(*) From BCT_ORDER_X91B WITH (NOLOCK) Where TRAILER_TIME Is Not NULL and seq_IDX > '201409020001' And Right(SEQ_NO,6) >= RIGHT((Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc), 6)) As RrSpring " +
                    " ";
                     */

                    "(Select Count(*) From BCT_ORDER_X84 WITH (NOLOCK) Where TRAILER_TIME Is Not NULL  And SEQ_NO >= (Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc)) As Front, " +
                    "(Select Count(*) From BCT_ORDER_X85 WITH (NOLOCK) Where TRAILER_TIME Is Not NULL  And SEQ_NO >= (Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc)) As Rear, " +
                    "(Select Count(*) From BCT_ORDER_X89 WITH (NOLOCK) Where TRAILER_TIME Is Not NULL  And SEQ_NO >= (Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc)) As FCOS, " +
                    "(Select Count(*) From BCT_ORDER_X91A WITH (NOLOCK) Where TRAILER_TIME Is Not NULL And SEQ_NO >= (Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc)) As RrShock, " +
                    "(Select Count(*) From BCT_ORDER_X91B WITH (NOLOCK) Where TRAILER_TIME Is Not NULL And SEQ_NO >= (Select Top 1 SEQ_NO From MQT_GSP_X84 Order By RECEIVE_TIME Desc)) As RrSpring " +
                " ";


                DataTable dtExternal = Program.QueryExecute(sExternalInv, null);
                if (dtExternal.Rows.Count > 0)
                {
                    sExternalUpdate = "Update WKH_BOARD Set " +
                        "FRONT = '" + (Int32.Parse(dtExternal.Rows[0][0].ToString()) - aryGapOrder[0]).ToString() + "',  " +
                        "REAR = '" + (Int32.Parse(dtExternal.Rows[0][1].ToString()) - aryGapOrder[1]).ToString() + "',  " +
                        "FCOSL = '" + (Int32.Parse(dtExternal.Rows[0][2].ToString()) - aryGapOrder[2]).ToString() + "',  " +
                        "FCOSR = '" + (Int32.Parse(dtExternal.Rows[0][2].ToString()) - aryGapOrder[2]).ToString() + "',  " +
                        "RSHOCK = '" + (Int32.Parse(dtExternal.Rows[0][3].ToString()) - aryGapOrder[3]).ToString() + "',  " +
                        "RSPRING = '" + (Int32.Parse(dtExternal.Rows[0][4].ToString()) - aryGapOrder[4]).ToString() + "'  " +
                        "Where CLASS = 'EXTERNAL' ";
                    Program.QueryExecute(sExternalUpdate, null);

                    Program.QueryExecute2nd(sExternalUpdate, null);
                }

 
                // Aligner
                string sAlignerQuery = "Select " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X84 WITH (NOLOCK) Where Machine = 'FM262' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X84 WITH (NOLOCK) Where Machine = 'FM272' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X84 WITH (NOLOCK) Where Machine = 'FM232' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X84 WITH (NOLOCK) Where Machine = 'FM208' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X84 WITH (NOLOCK) Where Machine = 'FM202' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X85 WITH (NOLOCK) Where Machine = 'RM192' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X85 WITH (NOLOCK) Where Machine = 'RM202' Order by CREATE_TIME Desc) As CT), " +
                    "(Select Avg(Convert(float, CycleTime)) As CYCTIME  From ( Select top 50 CycleTime From WKH_ALIGN_ROBOT_X85 WITH (NOLOCK) Where Machine = 'RM198' Order by CREATE_TIME Desc) As CT) " +
                    " ";
                DataTable dtAligner = Program.QueryExecute(sAlignerQuery, null);
                if (dtAligner.Rows.Count > 0)
                {
                    string sFrontAlignerUpdate = "Update WKH_BOARD Set " +
                        "ALIGNER_A = '" + dtAligner.Rows[0][0].ToString() + "',  " +
                        "ALIGNER_B = '" + dtAligner.Rows[0][1].ToString() + "',  " +
                        "ALIGNER_C = '" + dtAligner.Rows[0][2].ToString() + "',  " +
                        "ALIGNER_D = '" + dtAligner.Rows[0][3].ToString() + "',  " +
                        "ALIGNER_E = '" + dtAligner.Rows[0][4].ToString() + "' Where CLASS = 'FRONT' ";
                    Program.QueryExecute(sFrontAlignerUpdate, null);

                    Program.QueryExecute2nd(sFrontAlignerUpdate, null);

                    string sRearAlignerUpdate = "Update WKH_BOARD Set " +
                        "ALIGNER_A = '" + dtAligner.Rows[0][5].ToString() + "',  " +
                        "ALIGNER_B = '" + dtAligner.Rows[0][6].ToString() + "',  " +
                        "ALIGNER_C = '" + dtAligner.Rows[0][7].ToString() + "' Where CLASS = 'REAR' ";
                    Program.QueryExecute(sRearAlignerUpdate, null);

                    Program.QueryExecute2nd(sRearAlignerUpdate, null);
                }
                else
                {

                }


                // Last Broadcast
                string sSeq = "00000000000";
                string sVin = "00000000000000000" ;
                string sRecvTime = "00000000000000";
                string sSec = "0";

                sQuery = "Select Top 1 SEQ_NO, VIN, RECEIVE_TIME From MQT_SP_X84 Order By RECEIVE_TIME Desc";
                DataTable dtLastSeq = Program.QueryExecute(sQuery, null);
                if (dtLastSeq.Rows.Count > 0)
                {
                    sSeq = dtLastSeq.Rows[0][0].ToString();
                    sVin = dtLastSeq.Rows[0][1].ToString();
                    sRecvTime = dtLastSeq.Rows[0][2].ToString();
                }

                lblLastSeq.Text = "[ Last Broadcast ]  " + sSeq + "  " + sVin + "  -  " + sRecvTime.Substring(4, 2) + "/" + sRecvTime.Substring(6, 2) + " " + sRecvTime.Substring(8, 2) + ":" + sRecvTime.Substring(10, 2) + ":" + sRecvTime.Substring(12, 2);

                sQuery = "EXEC dbo.pr_GetElapseTime '" + sRecvTime + "'";
                DataTable dtElapse = Program.QueryExecute(sQuery, null);
                if (dtElapse.Rows.Count > 0)
                {
                    sSec = dtElapse.Rows[0][0].ToString();
                }

                if (Int32.Parse(sSec.ToString()) > 300)
                {
                    bAlarm = true;
                }
                else 
                { 
                    bAlarm = false;
                    lblLastSeq.BackColor = System.Drawing.Color.White;
                    lblLastSeq.ForeColor = System.Drawing.Color.Black;
                }



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GetData()
        {
            try
            {
                int nExternal;


                string sQuery = "SELECT * FROM WKH_BOARD WITH (NOLOCK) ORDER BY COL_IDX";

                DataTable dt = Program.QueryExecute(sQuery, null);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // MOBIS Count
                        if (i == 0)
                        {
                            fpSpread1.Sheets[0].Cells[1, 8].Text = dt.Rows[i][2].ToString();
                        }
                        // BBO
                        else if (i == 1)
                        {
                            fpSpread1.Sheets[0].Cells[4, 1].Text = dt.Rows[i][2].ToString();
                            fpSpread1.Sheets[0].Cells[4, 2].Text = dt.Rows[i][3].ToString();
                            fpSpread1.Sheets[0].Cells[4, 3].Text = dt.Rows[i][4].ToString();
                            fpSpread1.Sheets[0].Cells[4, 4].Text = dt.Rows[i][5].ToString();
                            fpSpread1.Sheets[0].Cells[4, 5].Text = dt.Rows[i][6].ToString();
                            fpSpread1.Sheets[0].Cells[4, 6].Text = dt.Rows[i][7].ToString();
                            fpSpread1.Sheets[0].Cells[4, 7].Text = dt.Rows[i][11].ToString();
                            fpSpread1.Sheets[0].Cells[4, 8].Text = dt.Rows[i][12].ToString();
                            fpSpread1.Sheets[0].Cells[4, 9].Text = dt.Rows[i][13].ToString();
                        }
                        //// Chrysler
                        else if (i == 2)
                        {
                            fpSpread1.Sheets[0].Cells[1, 0].Text = dt.Rows[i][2].ToString();
                        }
                        //// Count
                        else if (i == 3)
                        {
                            fpSpread1.Sheets[0].Cells[5, 1].Text = dt.Rows[i][2].ToString();
                            fpSpread1.Sheets[0].Cells[5, 2].Text = dt.Rows[i][3].ToString();
                            fpSpread1.Sheets[0].Cells[5, 3].Text = dt.Rows[i][4].ToString();
                            fpSpread1.Sheets[0].Cells[5, 4].Text = dt.Rows[i][5].ToString();
                            fpSpread1.Sheets[0].Cells[5, 5].Text = dt.Rows[i][6].ToString();
                            fpSpread1.Sheets[0].Cells[5, 6].Text = dt.Rows[i][7].ToString();
                            fpSpread1.Sheets[0].Cells[5, 7].Text = dt.Rows[i][11].ToString();
                            fpSpread1.Sheets[0].Cells[5, 8].Text = dt.Rows[i][12].ToString();
                            fpSpread1.Sheets[0].Cells[5, 9].Text = dt.Rows[i][13].ToString();
                        }
                        //// GAP
                        else if (i == 4)
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i][2].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][2].ToString()) >= 0) // Front
                                    fpSpread1.Sheets[0].Cells[6, 1].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 1].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 1].Text = dt.Rows[i][2].ToString();

                            if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][3].ToString()) >= 0) // Rear
                                    fpSpread1.Sheets[0].Cells[6, 2].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 2].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 2].Text = dt.Rows[i][3].ToString();

                            if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][4].ToString()) >= 0) // FCOS LH
                                    fpSpread1.Sheets[0].Cells[6, 3].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 3].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 3].Text = dt.Rows[i][4].ToString();

                            if (!string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][5].ToString()) >= 0) // FCOS RH
                                    fpSpread1.Sheets[0].Cells[6, 4].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 4].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 4].Text = dt.Rows[i][5].ToString();

                            if (!string.IsNullOrEmpty(dt.Rows[i][6].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][6].ToString()) >= 0) // Rear Shock
                                    fpSpread1.Sheets[0].Cells[6, 5].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 5].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 5].Text = dt.Rows[i][6].ToString();

                            if (!string.IsNullOrEmpty(dt.Rows[i][7].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][7].ToString()) >= 0) // Rear Spring
                                    fpSpread1.Sheets[0].Cells[6, 6].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 6].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 6].Text = dt.Rows[i][7].ToString();

                            if (!string.IsNullOrEmpty(dt.Rows[i][11].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][11].ToString()) >= 0) // Front Cradle
                                    fpSpread1.Sheets[0].Cells[6, 7].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 7].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 7].Text = dt.Rows[i][11].ToString();
                            
                            if (!string.IsNullOrEmpty(dt.Rows[i][12].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][12].ToString()) >= 0) // Rear Cradle
                                    fpSpread1.Sheets[0].Cells[6, 8].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 8].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 8].Text = dt.Rows[i][12].ToString();
                            
                            if (!string.IsNullOrEmpty(dt.Rows[i][13].ToString()))
                            {
                                if (Int32.Parse(dt.Rows[i][13].ToString()) >= 0) // Rear Knuckle
                                    fpSpread1.Sheets[0].Cells[6, 9].ForeColor = Color.White;
                                else
                                    fpSpread1.Sheets[0].Cells[6, 9].ForeColor = Color.Red;
                            }

                            fpSpread1.Sheets[0].Cells[6, 9].Text = dt.Rows[i][13].ToString();
                        }
                        //// JPH
                        else if (i == 5)
                        {
                            fpSpread1.Sheets[0].Cells[8, 1].Text = dt.Rows[i][2].ToString();
                            fpSpread1.Sheets[0].Cells[8, 2].Text = dt.Rows[i][3].ToString();
                            fpSpread1.Sheets[0].Cells[8, 3].Text = dt.Rows[i][4].ToString();
                            fpSpread1.Sheets[0].Cells[8, 4].Text = dt.Rows[i][5].ToString();
                            fpSpread1.Sheets[0].Cells[8, 5].Text = dt.Rows[i][6].ToString();
                            fpSpread1.Sheets[0].Cells[8, 6].Text = dt.Rows[i][7].ToString();
                            fpSpread1.Sheets[0].Cells[8, 7].Text = dt.Rows[i][11].ToString();
                            fpSpread1.Sheets[0].Cells[8, 8].Text = dt.Rows[i][12].ToString();
                            fpSpread1.Sheets[0].Cells[8, 9].Text = dt.Rows[i][13].ToString();
                        }
                        //// Count / Hour
                        else if (i == 6)
                        {
                            fpSpread1.Sheets[0].Cells[7, 1].Text = dt.Rows[i][2].ToString();
                            fpSpread1.Sheets[0].Cells[7, 2].Text = dt.Rows[i][3].ToString();
                            fpSpread1.Sheets[0].Cells[7, 3].Text = dt.Rows[i][4].ToString();
                            fpSpread1.Sheets[0].Cells[7, 4].Text = dt.Rows[i][5].ToString();
                            fpSpread1.Sheets[0].Cells[7, 5].Text = dt.Rows[i][6].ToString();
                            fpSpread1.Sheets[0].Cells[7, 6].Text = dt.Rows[i][7].ToString();
                            fpSpread1.Sheets[0].Cells[7, 7].Text = dt.Rows[i][11].ToString();
                            fpSpread1.Sheets[0].Cells[7, 8].Text = dt.Rows[i][12].ToString();
                            fpSpread1.Sheets[0].Cells[7, 9].Text = dt.Rows[i][13].ToString();
                        }
                        //// INT.
                        else if (i == 7)
                        {
                            fpSpread1.Sheets[0].Cells[9, 1].Text = dt.Rows[i][2].ToString();
                            fpSpread1.Sheets[0].Cells[9, 2].Text = dt.Rows[i][3].ToString();
                            fpSpread1.Sheets[0].Cells[9, 3].Text = dt.Rows[i][4].ToString();
                            fpSpread1.Sheets[0].Cells[9, 4].Text = dt.Rows[i][5].ToString();
                            fpSpread1.Sheets[0].Cells[9, 5].Text = dt.Rows[i][6].ToString();
                            fpSpread1.Sheets[0].Cells[9, 6].Text = dt.Rows[i][7].ToString();
                            fpSpread1.Sheets[0].Cells[9, 7].Text = "-";
                            fpSpread1.Sheets[0].Cells[9, 8].Text = "-";
                            fpSpread1.Sheets[0].Cells[9, 9].Text = "-";
                        }
                        //// InTransit
                        else if (i == 11)
                        {
                            if (Int32.Parse(dt.Rows[8][2].ToString()) > iYellow) // Front
                                fpSpread1.Sheets[0].Cells[10, 1].ForeColor = Color.White;
                            else if (Int32.Parse(dt.Rows[8][2].ToString()) <= iYellow && Int32.Parse(dt.Rows[8][2].ToString()) > iRed)
                                fpSpread1.Sheets[0].Cells[10, 1].ForeColor = Color.Yellow;
                            else if (Int32.Parse(dt.Rows[8][2].ToString()) <= iRed)
                                fpSpread1.Sheets[0].Cells[10, 1].ForeColor = Color.Red;

                            if (Int32.Parse(dt.Rows[8][3].ToString()) > iYellow) // Rear
                                fpSpread1.Sheets[0].Cells[10, 2].ForeColor = Color.White;
                            else if (Int32.Parse(dt.Rows[8][3].ToString()) <= iYellow && Int32.Parse(dt.Rows[8][3].ToString()) > iRed)
                                fpSpread1.Sheets[0].Cells[10, 2].ForeColor = Color.Yellow;
                            else if (Int32.Parse(dt.Rows[8][3].ToString()) <= iRed)
                                fpSpread1.Sheets[0].Cells[10, 2].ForeColor = Color.Red;

                            if (Int32.Parse(dt.Rows[8][4].ToString()) > iYellow) // FCOS LH
                                fpSpread1.Sheets[0].Cells[10, 3].ForeColor = Color.White;
                            else if (Int32.Parse(dt.Rows[8][4].ToString()) <= iYellow && Int32.Parse(dt.Rows[8][4].ToString()) > iRed)
                                fpSpread1.Sheets[0].Cells[10, 3].ForeColor = Color.Yellow;
                            else if (Int32.Parse(dt.Rows[8][4].ToString()) <= iRed)
                                fpSpread1.Sheets[0].Cells[10, 3].ForeColor = Color.Red;

                            if (Int32.Parse(dt.Rows[8][5].ToString()) > iYellow) // FCOS RH
                                fpSpread1.Sheets[0].Cells[10, 4].ForeColor = Color.White;
                            else if (Int32.Parse(dt.Rows[8][5].ToString()) <= iYellow && Int32.Parse(dt.Rows[8][5].ToString()) > iRed)
                                fpSpread1.Sheets[0].Cells[10, 4].ForeColor = Color.Yellow;
                            else if (Int32.Parse(dt.Rows[8][5].ToString()) <= iRed)
                                fpSpread1.Sheets[0].Cells[10, 4].ForeColor = Color.Red;

                            if (Int32.Parse(dt.Rows[8][6].ToString()) > iYellow) // Rear Shock
                                fpSpread1.Sheets[0].Cells[10, 5].ForeColor = Color.White;
                            else if (Int32.Parse(dt.Rows[8][6].ToString()) <= iYellow && Int32.Parse(dt.Rows[8][6].ToString()) > iRed)
                                fpSpread1.Sheets[0].Cells[10, 5].ForeColor = Color.Yellow;
                            else if (Int32.Parse(dt.Rows[8][6].ToString()) <= iRed)
                                fpSpread1.Sheets[0].Cells[10, 5].ForeColor = Color.Red;

                            if (Int32.Parse(dt.Rows[8][7].ToString()) > iYellow) // Rear Spring
                                fpSpread1.Sheets[0].Cells[10, 6].ForeColor = Color.White;
                            else if (Int32.Parse(dt.Rows[8][7].ToString()) <= iYellow && Int32.Parse(dt.Rows[8][7].ToString()) > iRed)
                                fpSpread1.Sheets[0].Cells[10, 6].ForeColor = Color.Yellow;
                            else if (Int32.Parse(dt.Rows[8][7].ToString()) <= iRed)
                                fpSpread1.Sheets[0].Cells[10, 6].ForeColor = Color.Red;


                            
                            fpSpread1.Sheets[0].Cells[10, 1].Text = dt.Rows[i][2].ToString();
                            fpSpread1.Sheets[0].Cells[10, 2].Text = dt.Rows[i][3].ToString();
                            fpSpread1.Sheets[0].Cells[10, 3].Text = dt.Rows[i][4].ToString();
                            fpSpread1.Sheets[0].Cells[10, 4].Text = dt.Rows[i][5].ToString();
                            fpSpread1.Sheets[0].Cells[10, 5].Text = dt.Rows[i][6].ToString();
                            fpSpread1.Sheets[0].Cells[10, 6].Text = dt.Rows[i][7].ToString();
                            fpSpread1.Sheets[0].Cells[10, 7].Text = "-";
                            fpSpread1.Sheets[0].Cells[10, 8].Text = "-";
                            fpSpread1.Sheets[0].Cells[10, 9].Text = "-";
                        }
                        //// EXT.
                        else if (i == 8)
                        {
                            //Front
                            nExternal = Int32.Parse(dt.Rows[i][2].ToString()) - Int32.Parse(dt.Rows[11][2].ToString());
                            if (nExternal > iYellow2)
                                fpSpread1.Sheets[0].Cells[11, 1].ForeColor = Color.White;
                            else if (nExternal <= iYellow2 && nExternal > iRed2)
                                fpSpread1.Sheets[0].Cells[11, 1].ForeColor = Color.Yellow;
                            else if (nExternal <= iRed2)
                                fpSpread1.Sheets[0].Cells[11, 1].ForeColor = Color.Red;
                            
                            fpSpread1.Sheets[0].Cells[11, 1].Text = nExternal.ToString();
                            //fpSpread1.Sheets[0].Cells[10, 1].Text = dt.Rows[i][2].ToString();

                            //rear
                            nExternal = Int32.Parse(dt.Rows[i][3].ToString()) - Int32.Parse(dt.Rows[11][3].ToString());
                            if (nExternal > iYellow2)
                                fpSpread1.Sheets[0].Cells[11, 2].ForeColor = Color.White;
                            else if (nExternal <= iYellow2 && nExternal > iRed2)
                                fpSpread1.Sheets[0].Cells[11, 2].ForeColor = Color.Yellow;
                            else if (nExternal <= iRed2)
                                fpSpread1.Sheets[0].Cells[11, 2].ForeColor = Color.Red;
                            fpSpread1.Sheets[0].Cells[11, 2].Text = nExternal.ToString();
                            //fpSpread1.Sheets[0].Cells[10, 2].Text = dt.Rows[i][3].ToString();

                            //FCOS LH
                            nExternal = Int32.Parse(dt.Rows[i][4].ToString()) - Int32.Parse(dt.Rows[11][4].ToString());
                            if (nExternal > iYellow2)
                                fpSpread1.Sheets[0].Cells[11, 3].ForeColor = Color.White;
                            else if (nExternal <= iYellow2 && nExternal > iRed2)
                                fpSpread1.Sheets[0].Cells[11, 3].ForeColor = Color.Yellow;
                            else if (nExternal <= iRed2)
                                fpSpread1.Sheets[0].Cells[11, 3].ForeColor = Color.Red;
                            fpSpread1.Sheets[0].Cells[11, 3].Text = nExternal.ToString();
                            //fpSpread1.Sheets[0].Cells[10, 3].Text = dt.Rows[i][4].ToString();

                            // FCOS RH
                            nExternal = Int32.Parse(dt.Rows[i][5].ToString()) - Int32.Parse(dt.Rows[11][5].ToString());
                            if (nExternal > iYellow2)
                                fpSpread1.Sheets[0].Cells[11, 4].ForeColor = Color.White;
                            else if (nExternal <= iYellow2 && nExternal > iRed2)
                                fpSpread1.Sheets[0].Cells[11, 4].ForeColor = Color.Yellow;
                            else if (nExternal <= iRed2)
                                fpSpread1.Sheets[0].Cells[11, 4].ForeColor = Color.Red;
                            fpSpread1.Sheets[0].Cells[11, 4].Text = nExternal.ToString();
                            //fpSpread1.Sheets[0].Cells[10, 4].Text = dt.Rows[i][5].ToString();

                            // Rear Shock
                            nExternal = Int32.Parse(dt.Rows[i][6].ToString()) - Int32.Parse(dt.Rows[11][6].ToString());
                            if (nExternal > iYellow2)
                                fpSpread1.Sheets[0].Cells[11, 5].ForeColor = Color.White;
                            else if (nExternal <= iYellow2 && nExternal > iRed2)
                                fpSpread1.Sheets[0].Cells[11, 5].ForeColor = Color.Yellow;
                            else if (nExternal <= iRed2)
                                fpSpread1.Sheets[0].Cells[11, 5].ForeColor = Color.Red;
                            fpSpread1.Sheets[0].Cells[11, 5].Text = nExternal.ToString();
                            //fpSpread1.Sheets[0].Cells[10, 5].Text = dt.Rows[i][6].ToString();

                            // Rear Spring
                            nExternal = Int32.Parse(dt.Rows[i][7].ToString()) - Int32.Parse(dt.Rows[11][7].ToString());
                            if (nExternal > iYellow2)
                                fpSpread1.Sheets[0].Cells[11, 6].ForeColor = Color.White;
                            else if (nExternal <= iYellow2 && nExternal > iRed2)
                                fpSpread1.Sheets[0].Cells[11, 6].ForeColor = Color.Yellow;
                            else if (nExternal <= iRed2)
                                fpSpread1.Sheets[0].Cells[11, 6].ForeColor = Color.Red;
                            fpSpread1.Sheets[0].Cells[11, 6].Text = nExternal.ToString();
                            //fpSpread1.Sheets[0].Cells[10, 6].Text = dt.Rows[i][7].ToString();

                            fpSpread1.Sheets[0].Cells[11, 7].Text = "-";
                            fpSpread1.Sheets[0].Cells[11, 8].Text = "-";
                            fpSpread1.Sheets[0].Cells[11, 9].Text = "-";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            UpdateBuildData();
            tmrUpdate.Start();
        }

        private void tmrSetTime_Tick(object sender, EventArgs e)
        {
            lblClock.Text = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss");

            if (bAlarm)
            {
                if (lblLastSeq.BackColor == System.Drawing.Color.Yellow)
                {
                    lblLastSeq.BackColor = System.Drawing.Color.Red;//  .FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    lblLastSeq.ForeColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    lblLastSeq.BackColor = System.Drawing.Color.Yellow;//  .FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    lblLastSeq.ForeColor = System.Drawing.Color.Red;
                }


            }
        }

        private void lblClock_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
