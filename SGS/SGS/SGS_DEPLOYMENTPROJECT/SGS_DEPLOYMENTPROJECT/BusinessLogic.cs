using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Excel = Microsoft.Office.Interop.Excel;

namespace SGS_DEPLOYMENTPROJECT
{
   
    public class BusinessLogic
    {
        public static SerialPort SerialPort;
        public static Excel.Range xlRange;
        public static int loopCount = 1;
        public static string ledOnCode;
        public static string ledOffCode;
        public static string trayCode;
        public static string sWInput;
        public static string switchCode;

        public Excel.Range InitializeExcel() {

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Helper.assetFolderPath+"\\Sheet.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            return xlRange;
        }

    }
}
