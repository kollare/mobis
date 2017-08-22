﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OMMCDP.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("40")]
        public int DISPLAY_SEQ_H_RATIO {
            get {
                return ((int)(this["DISPLAY_SEQ_H_RATIO"]));
            }
            set {
                this["DISPLAY_SEQ_H_RATIO"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public int DISPLAY_PART_H_RATIO {
            get {
                return ((int)(this["DISPLAY_PART_H_RATIO"]));
            }
            set {
                this["DISPLAY_PART_H_RATIO"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server=10.120.175.75;database=OMMCDP;uid=OMMCDP;pwd=MNA_M!2008")]
        public string CONNECTIONSTRING {
            get {
                return ((string)(this["CONNECTIONSTRING"]));
            }
            set {
                this["CONNECTIONSTRING"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int MSGBOX_HIDE_DELAY {
            get {
                return ((int)(this["MSGBOX_HIDE_DELAY"]));
            }
            set {
                this["MSGBOX_HIDE_DELAY"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AUTO_SKIP_ENABLE {
            get {
                return ((bool)(this["AUTO_SKIP_ENABLE"]));
            }
            set {
                this["AUTO_SKIP_ENABLE"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public double AUTO_SKIP_SECOND {
            get {
                return ((double)(this["AUTO_SKIP_SECOND"]));
            }
            set {
                this["AUTO_SKIP_SECOND"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("N/A|N/A")]
        public string AUTO_SKIP_BARCODE_TO_PART_NO {
            get {
                return ((string)(this["AUTO_SKIP_BARCODE_TO_PART_NO"]));
            }
            set {
                this["AUTO_SKIP_BARCODE_TO_PART_NO"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MSGBOX_ENABLE {
            get {
                return ((bool)(this["MSGBOX_ENABLE"]));
            }
            set {
                this["MSGBOX_ENABLE"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PART_GROUP_CHECK {
            get {
                return ((bool)(this["PART_GROUP_CHECK"]));
            }
            set {
                this["PART_GROUP_CHECK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PART_GROUP_DISPLAY {
            get {
                return ((bool)(this["PART_GROUP_DISPLAY"]));
            }
            set {
                this["PART_GROUP_DISPLAY"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"SELECT PC.PC_CODE,
       PC.PC_NAME,
       PC.LINE_CODE,
       PC.STATION_CODE,
       PC.PC_IDX,
       PC.MAIN_TITLE,
       PC.IP_ADDRESS,
       PC_KIND,
       PC.SCANNER_PORT,
       PC.SCANNER_BAUD,
       PC.KEEP_ALIVE,
       PC.HEARTBEAT_TIME,
       ISNULL (CM.VALUE_01, '2') AS ROW_COUNT,
       ISNULL (CM.VALUE_02, '2') AS COLUMN_COUNT,
       ISNULL (CM.VALUE_03, 'HORIZONTAL') AS DIRECTION,
       CM.VALUE_05 AS REVS_COUNT,
       CM.VALUE_06 AS PLCNM_CODES,
       CM.VALUE_07 AS PROC_DONE_FLAG,
       CM.VALUE_08 AS DISPLAY_PATTERN
  FROM MST_PCINFO PC
       LEFT OUTER JOIN MST_COMMON CM
          ON PC.PC_CODE = CM.COMMON_CODE AND CM.COMMON_GROUP = 'MAT_FEEDING'
 WHERE PC.IP_ADDRESS = '{0}'")]
        public string SQL_GET_PCINFO {
            get {
                return ((string)(this["SQL_GET_PCINFO"]));
            }
            set {
                this["SQL_GET_PCINFO"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Green")]
        public global::System.Drawing.Color PART_BACKCOLOR_OK {
            get {
                return ((global::System.Drawing.Color)(this["PART_BACKCOLOR_OK"]));
            }
            set {
                this["PART_BACKCOLOR_OK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color PART_FORECOLOR_OK {
            get {
                return ((global::System.Drawing.Color)(this["PART_FORECOLOR_OK"]));
            }
            set {
                this["PART_FORECOLOR_OK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightYellow")]
        public global::System.Drawing.Color PART_BACKCOLOR_NEXTWORKS {
            get {
                return ((global::System.Drawing.Color)(this["PART_BACKCOLOR_NEXTWORKS"]));
            }
            set {
                this["PART_BACKCOLOR_NEXTWORKS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color PART_FORECOLOR_NEXTWORKS {
            get {
                return ((global::System.Drawing.Color)(this["PART_FORECOLOR_NEXTWORKS"]));
            }
            set {
                this["PART_FORECOLOR_NEXTWORKS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red")]
        public global::System.Drawing.Color PART_BACKCOLOR_NG {
            get {
                return ((global::System.Drawing.Color)(this["PART_BACKCOLOR_NG"]));
            }
            set {
                this["PART_BACKCOLOR_NG"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color PART_FORECOLOR_NG {
            get {
                return ((global::System.Drawing.Color)(this["PART_FORECOLOR_NG"]));
            }
            set {
                this["PART_FORECOLOR_NG"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Yellow")]
        public global::System.Drawing.Color PART_BACKCOLOR_CURRENTWORK {
            get {
                return ((global::System.Drawing.Color)(this["PART_BACKCOLOR_CURRENTWORK"]));
            }
            set {
                this["PART_BACKCOLOR_CURRENTWORK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color PART_FORECOLOR_CURRENTWORK {
            get {
                return ((global::System.Drawing.Color)(this["PART_FORECOLOR_CURRENTWORK"]));
            }
            set {
                this["PART_FORECOLOR_CURRENTWORK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkBlue")]
        public global::System.Drawing.Color SEQ_BACKCOLOR_CURRENTWORK {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_BACKCOLOR_CURRENTWORK"]));
            }
            set {
                this["SEQ_BACKCOLOR_CURRENTWORK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Yellow")]
        public global::System.Drawing.Color SEQ_FORECOLOR_CURRENTWORK {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_FORECOLOR_CURRENTWORK"]));
            }
            set {
                this["SEQ_FORECOLOR_CURRENTWORK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SteelBlue")]
        public global::System.Drawing.Color SEQ_BACKCOLOR_NEXTWORKS {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_BACKCOLOR_NEXTWORKS"]));
            }
            set {
                this["SEQ_BACKCOLOR_NEXTWORKS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color SEQ_FORECOLOR_NEXTWORKS {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_FORECOLOR_NEXTWORKS"]));
            }
            set {
                this["SEQ_FORECOLOR_NEXTWORKS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SteelBlue")]
        public global::System.Drawing.Color SEQ_BACKCOLOR_OK {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_BACKCOLOR_OK"]));
            }
            set {
                this["SEQ_BACKCOLOR_OK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color SEQ_FORECOLOR_OK {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_FORECOLOR_OK"]));
            }
            set {
                this["SEQ_FORECOLOR_OK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Yellow")]
        public global::System.Drawing.Color SEQ_FORECOLOR_GROUP {
            get {
                return ((global::System.Drawing.Color)(this["SEQ_FORECOLOR_GROUP"]));
            }
            set {
                this["SEQ_FORECOLOR_GROUP"] = value;
            }
        }
    }
}
