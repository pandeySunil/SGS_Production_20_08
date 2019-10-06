using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class NewImageGetterTester : Form
    {
        public static Microsoft.Office.Interop.Excel.Range xlRange;
        public static int ImagIndex { get; set; }
        public static int counter { get; set; }
        public static String imageFileId;
        public Dictionary<string, Bitmap> OriginalImages = new Dictionary<string, Bitmap>();
        public NewImageGetterTester()
        {
            
            InitializeComponent();
            Helper.assetFolderPath = @"C:\Users\Sunil.Pandey\Desktop\connectors3\connectors";
            this.Show();
            this.Focus();
            counter = 2;
        }
        ImageDataIntializer ImageGetter { get; set; }
        private void NewImageGetterTester_Load(object sender, EventArgs e)
        {
            var sheetObject = new BusinessLogic();
            xlRange = sheetObject.InitializeExcel();
            var ImageLoadThread = new Thread(() => LoadImage(0,xlRange.Cells[2, 8].Value2));
            
            if (xlRange.Cells[2, 8] != null && xlRange.Cells[2, 8].Value2 != null) {
                ImageLoadThread.Start();
            }
            ImageLoadThread.IsBackground = true;

        }
       
        public void LoadImage(int imageId,string imageFile)
        {
            if (imageFileId !=null) {
            imageFile = imageFileId;
            }
            if (ImageGetter == null)
            {
                ImageGetter = new ImageDataIntializer();
                OriginalImages = ImageGetter.LoadOrginalImages();

            }
            
            var modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex, imageFile);
            var preImgIndex = ImagIndex;
            bool toggleFlag = false;
            //var imageId = 1;
            while (true)
            {
                var originalKey = imageFile + ".json";
                var originalImage = OriginalImages[originalKey];
                if (imageFileId!=null)
                {
                    imageFile = imageFileId;
                }
                if (preImgIndex != ImagIndex)
                {
                    modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex, imageFile);
                }
                if (toggleFlag)
                {

                    pictureBox1.Image = originalImage;
                    toggleFlag = false;

                }
                else
                {

                    pictureBox1.Image = modifiedImage;
                    toggleFlag = true;
                }
                Thread.Sleep(250);
            }
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            if (ImagIndex != 1) {
                counter--;
                if (xlRange.Cells[ImagIndex, 7] != null && xlRange.Cells[ImagIndex, 8].Value2 != null && xlRange.Cells[ImagIndex, 8] != null && xlRange.Cells[ImagIndex, 7].Value2 != null)
                {
                    ImagIndex = Convert.ToInt32(xlRange.Cells[ImagIndex, 7].Value2);
                    imageFileId = xlRange.Cells[ImagIndex, 8].Value2;

                }
            }

        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (ImagIndex != 10)
            {
                counter++;
                if (xlRange.Cells[counter, 7] != null && xlRange.Cells[counter, 8].Value2 != null && xlRange.Cells[counter, 8] != null && xlRange.Cells[counter, 7].Value2 != null)
                {
                    ImagIndex = Convert.ToInt32(xlRange.Cells[counter, 7].Value2);
                    imageFileId =  xlRange.Cells[counter, 8].Value2;

                }

                
            }
        }
    }
}
