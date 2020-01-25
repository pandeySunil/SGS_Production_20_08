using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        public static Login Login { get; set; }
        public static FlashScreen FlashScreen { get; set; }
        public static bool ArdinoCon = true;
        public static SerialPort serialPort { get; set; }
        public static string assetFolderPath { get; set; }
        public static int target { get; set; }
        public static string GetKeyValue(string s)
        {
            var value = "";

            Configuration configuration = ConfigurationManager.
                       OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            value = ConfigurationManager.AppSettings.Get(s);

            return value;
        }
        public static void  SetKeyValue(string s,string v)
        {
            Configuration configuration = ConfigurationManager.
                       OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            configuration.AppSettings.Settings[s].Value = v;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string GetBarcode() {
          
            var barcode = GetKeyValue("StartingNumber");
            int o = 0;
            var bNumber = int.TryParse(Regex.Match(barcode, @"\d+").Value, out o);
            if (bNumber) {
                o++;
            }
            string bString = new String(barcode.Where(c => Char.IsLetter(c)).ToArray());

            barcode = o > 0 ? bString + Convert.ToString(o) : bString;
            SetKeyValue("StartingNumber", barcode);
            return barcode;

        }

    }


}
