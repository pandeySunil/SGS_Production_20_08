using OnBarcode.Barcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SGS_DEPLOYMENTPROJECT
{

    public class Printer
    {
        public String PrintDateString { get; set; }
        public Image BarCodeImage { get; set; }
        public Graphics PreviewGraphics { get; set; }
        public String Errors { get; set; }
        public String BarcodeString {get;set;}
        public Font DefaultFont = new Font("Arial", 24, FontStyle.Bold);

        private void printPage(object sender, PrintPageEventArgs e)
        {
            var stringToPrint = PrintDateString;
            int charactersOnPage = 0;
            int linesPerPage = 0;
            Graphics graphics = e.Graphics;
            Configuration configuration = ConfigurationManager.
            OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            PrintDateString = configuration.AppSettings.Settings["PrintedCompanyName"].Value;
            BarcodeString = configuration.AppSettings.Settings["StartingNumber"].Value;

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            graphics.MeasureString(stringToPrint, DefaultFont,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            graphics.DrawString(stringToPrint, DefaultFont, Brushes.Black,
             e.MarginBounds, StringFormat.GenericTypographic);

            //graphics = Graphics.FromImage(BarCodeImage);
            graphics.DrawImage(BarCodeImage, new Rectangle(0, 50, 200, 50));
            graphics.DrawString(PrintDateString, DefaultFont, Brushes.Black, new PointF(10f, 10f));

            PreviewGraphics = graphics;
            // Remove the portion of the string that has been printed.
            Bitmap bmp = new Bitmap(200, 200, PreviewGraphics);
       
            stringToPrint = stringToPrint.Substring(charactersOnPage);
            e.HasMorePages = false;
           
        }
        private void GenerateBacode(string _data)
        {
            Linear barcode = new Linear();
            barcode.Type = BarcodeType.CODE11;
            barcode.Data = _data;
            BarCodeImage = barcode.drawBarcode();
        }

        public  void StartPrinting()
        {
            PrintDocument printDoc = new PrintDocument();
            // printDoc.PrinterSettings.PrinterName = DefaultPrinter.GetDefaultPrinterName();

            //if (Validations())
            //{
            //    MessageBox.Show("Make Sure Both The Boxes Are Filled");
            //    return;
            //}

            GenerateBacode(BarcodeString);
            PrintDateString = PrintDateString + "  " + "OK TESTED";
            PrintDateString += "\n" + Convert.ToString(DateTime.Now);
            var printServer = new LocalPrintServer();
            //  MessageBox.Show(LocalPrintServer.GetDefaultPrintQueue().FullName);
            if (LocalPrintServer.GetDefaultPrintQueue().FullName == null || LocalPrintServer.GetDefaultPrintQueue().FullName == "")
            {
                MessageBox.Show("NO PRINTER");
                return;
            }

            printDoc.PrinterSettings.PrinterName = LocalPrintServer.GetDefaultPrintQueue().FullName;
            printDoc.PrintPage += new PrintPageEventHandler(printPage);
            printDoc.Print();

        }
        //private bool Validations()
        ////{
        ////    if (textBox2.Text == null || textBox2.Text == "" || textBox1.Text == null || textBox1.Text == "")
        ////    {
        ////        return true;
        ////    }
        ////    else
        ////    {
        ////        return false;
        ////    }
        //}
    }
}
