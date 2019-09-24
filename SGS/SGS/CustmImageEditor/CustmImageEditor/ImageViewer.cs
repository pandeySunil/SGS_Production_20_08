using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGS_DEPLOYMENTPROJECT;

namespace CustmImageEditor
{
    public partial class ImageViewer : Form
    {
        public Bitmap pbImageBitmap { get; set; }
        public ImageGetter ImageGetterObject { get; set; }
        public int shapePointer;
        public ImageViewer()
        {
            InitializeComponent();

            ImageGetterObject = new ImageGetter();

        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {
            shapePointer = 0;
            ImageGetterObject.GetBitmap(shapePointer);
        }

        private void nextImgButton_Click(object sender, EventArgs e)
        {
            shapePointer++;
            pictureBox1.Refresh();
           pictureBox1.Image =  ImageGetterObject.GetBitmap(shapePointer);
        }

        private void preImageButton_Click(object sender, EventArgs e)
        {
            if (shapePointer>0) {
                shapePointer--;
            }
          
            pictureBox1.Refresh();
            pictureBox1.Image = ImageGetterObject.GetBitmap(shapePointer);
        }
    }
}
