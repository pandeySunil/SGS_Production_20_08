using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS_DEPLOYMENTPROJECT
{
    public static class Helper
    {
        public static string ExcelSheetName { get; set; }
        public static string SerialPortName { get; set; }
        public static string LoggedInUserName { get; set; }
        public static bool LoggerInUserIsAdmin { get; set; }
        public static SettingForm settingForm { get; set; }
        public static CreateUser CreateUser { get; set; }
        public static IOCheck iOCheck { get; set; }
        public static Form1 form1 { get; set; }
        public static string  assetFolderPath { get; set; }

    }

}
