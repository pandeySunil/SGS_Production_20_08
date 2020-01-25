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
        public Font DefaultFont = new Font("Arial", 10, FontStyle.Bold);

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
            //Linear barcode = new Linear();
            //barcode.Type = BarcodeType.CODE39;
            //barcode.Data = "ss1234";
            //BarCodeImage = barcode.drawBarcode();
            //Bitmap bm = new Bitmap(BarCodeImage);
            Linear code39 = new Linear();

            // Barcode data to encode
            code39.Data = _data;
            // Barcode symbology type
            code39.Type = BarcodeType.CODE39;
            // Apply checksum digit for Code-39
            code39.AddCheckSum = true;

            // The space between 2 characters in code 39; This a multiple of X; The default is 1.
            code39.I = 0.5f;
            // Wide/narrow ratio, 2.0 - 3.0 inclusive, default is 2.
            code39.N = 0.5f;
            // If true, display a * in the beginning and end of barcode text
            code39.ShowStartStopInText = false;

            /*
            * Barcode Image Related Settings
            */
            // Unit of meature for all size related setting in the library. 
            code39.UOM = UnitOfMeasure.PIXEL;
            // Bar module width (X), default is 1 pixel;
            code39.X = 0.5f;
            // Bar module height (Y), default is 60 pixel;
            code39.Y = 30;
            // Barcode image left, right, top, bottom margins. Defaults are 0.
            code39.LeftMargin = 0;
            code39.RightMargin = 0;
            code39.TopMargin = 0;
            code39.BottomMargin = 0;
            // Image resolution in dpi, default is 72 dpi.
            code39.Resolution = 72;
            // Created barcode orientation.
            //4 options are: facing left, facing right, facing bottom, and facing top
            code39.Rotate = Rotate.Rotate0;

            /*
            * Linear barcodes human readable text styles
            */
            // Display human readable text under the barcode
            code39.ShowText = true;
            // Display checksum digit at the end of barcode data.
            code39.ShowCheckSumChar = true;
            // Human readable text font size, font family and style
            code39.TextFont = new Font("Arial", 9f, FontStyle.Regular);
            // Space between barcode and text. Default is 6 pixel.
            code39.TextMargin = 6;

            // Generate Code-39 and encode barcode to gif format
            code39.Format = System.Drawing.Imaging.ImageFormat.Jpeg;
            //code39.drawBarcode("C:\\code39.gif");
            BarCodeImage = code39.drawBarcode();
            //  resizeImage(BarCodeImage, new Size(50, 50));
            //pictureBox1.Image = resizeImage(BarCodeImage, new Size(90, 40));
            //pictureBox1.Refresh();
            //pictureBox1.Show();
            Bitmap bm = new Bitmap(resizeImage(BarCodeImage, new Size(90, 40)));
            string exePath =
   System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetEntryAssembly().Location);
            bm.Save(exePath + "\\BarcodeImage.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
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
            Configuration configuration = ConfigurationManager.
       OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            GenerateBacode(configuration.AppSettings.Settings["StartingNumber"].Value);
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
