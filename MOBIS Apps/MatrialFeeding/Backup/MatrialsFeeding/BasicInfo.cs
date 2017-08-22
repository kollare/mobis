using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace OMMCDP
{
    public class BasicInfo
    {
        public static string PCCD;                      // Work PC Code
        public static string CONNSTRING;                // Database Connect String
        public static string USER_ID;                   // Login User ID
        public static bool LOGIN_FLAG;                  // 로그인 유무 true/false
        public static string SUPERVISOR_ID;             // SuperVisor ID
        public static string QUALITY_ID;             // QUALITY ID
        public static bool MATCHING_FLAG;               // 매칭 작업 유무 true/false
        public static bool SUPERVISOR_FLAG;             // SuperVisor Call true/false
        public static bool WORKINSTRUCTION_FLAG;        // 동영상 실행 유무 
        public static int SUPERVISOR_BUTTON_INDEX;      // 슈퍼바이저가 버튼으로 강제 완료시 어떤 랙을 클릭하였는지 알기위한 인덱스
        public static string BARCODE_ERROR_MESSGE;
        public static bool BARCODEFLAG;
        public static string PRINTMSG;
        public static bool SHIPPING;                    // 강제출하 유무
        public static int BARCODEINDEX;                        // 1: 프린트 완료 0 : 프린트 
        public static bool REPRINT;
        public static bool LOGIN_FORM_ACTIVATE;         // 로그인 폼 활성화 유무
        public static bool LOAD_POSITION_FLAG;          // (loadposition 바코드 read 유/무)
        public static bool RHLH_FLAG;
        public static bool PARTCHECK_FLAG;
        public static int LOAD_POSITION;                // frmBase2x3에서만 사용 (read한 loadposition의 값)
        public static string LOAD_LHRH;                 


        public string LINE_CODE;                        //라인코드
        public string STATION_CODE;                     //스테이션 코드
        public string ORDER_WORKTIME;                   //서브장 매칭 타임 컬럼명
        public int PC_IDX;
        public string PC_NAME;
        public string PC_KIND;
        public string IP_ADDRESS;
        public string MAIN_TITLE;
        public int SCANNER_PORT;
        public string BARCODE_IP;
        public int BARCODE_PORT;
        public string LHRH;

        public struct _ORDER
        {
            public string SEQ_IDX;          //Seq_Idx
            public string SEQ_NO;
            public string VIN;
            public string PART_NO;
            public string RACK_ID;
            public string LOCAL_INDEX;
            public string RECEIVED_TIME;

            public string LH_PART;
            public string RH_PART;

            public string AIR;

            public bool WORK_RESULT;        //작업 결과     (true/false)
        }

        public struct _RACK
        {
            public string RACK_ID;
            public string RECEIVED_TIME;
            public string LOWEST_SEQ;
            public string HIGHEST_SEQ;
            public string LOWEST_INDEX;
            public string HIGHEST_INDEX;
            public string LOCAL_INDEX;
            public int TOTAL_COUNT;
            public int CURRENT_COUNT;
            public bool SHIPPED_RESULT;

            public string LOCAL_INDEX_LH;
            public string LOCAL_INDEX_RH;
            public int CURRENT_LH_COUNT;
            public int CURRENT_RH_COUNT;
            public bool SHIPPED_LH_RESULT;
            public bool SHIPPED_RH_RESULT;


        }

        public struct _WORK
        {
            public string LINE_CODE;
            public string WORK_CODE;
            public string WORK_TYPE;
        }


        public BasicInfo(DataTable table)
        {
            CONNSTRING = table.Rows[0]["CONNECTIONSTRING"].ToString();
            PCCD = string.Empty;
            USER_ID = string.Empty;
            LOGIN_FLAG = false;
            MATCHING_FLAG = false;
            SUPERVISOR_FLAG = false;
            SUPERVISOR_ID = string.Empty;
            BARCODEFLAG = false;
            PRINTMSG = string.Empty;
            BARCODE_ERROR_MESSGE = "";
            SHIPPING = false;
            BARCODEINDEX = 0;
            REPRINT = false;
            SUPERVISOR_BUTTON_INDEX = 0;
            WORKINSTRUCTION_FLAG = false;
            LOGIN_FORM_ACTIVATE = false;
            LOAD_POSITION = 0;
            LOAD_POSITION_FLAG = false;
            LOAD_LHRH = "R";
            RHLH_FLAG = false;

            this.LINE_CODE = string.Empty;
            this.STATION_CODE = string.Empty;
            this.ORDER_WORKTIME = string.Empty;
            this.PC_IDX = 0;
            this.PC_NAME = string.Empty;
            this.PC_KIND = string.Empty;
            this.IP_ADDRESS = string.Empty;
            this.MAIN_TITLE = string.Empty;
            this.LHRH = string.Empty;
            this.SCANNER_PORT = 0;
        }

        public bool CheckChartoInt(string sValue)
        {
            bool bResult = true;
            char[] chValue = sValue.ToCharArray();

            for (int i = 0; i < chValue.Length; i++)
            {
                if (!Char.IsDigit(chValue[i]))
                    bResult = false;                
            }

            return bResult;
        }


    }

}
