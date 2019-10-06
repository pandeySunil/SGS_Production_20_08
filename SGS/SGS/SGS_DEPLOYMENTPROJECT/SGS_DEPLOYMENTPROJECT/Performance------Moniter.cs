using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class Performance______Moniter : Form
    {
      public ArdiunoAdapter ardiunoAdapter = new ArdiunoAdapter(true);
        public Performance______Moniter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          Thread BackGroundThread = new Thread(() => Start());
            BackGroundThread.IsBackground = true;
        }
        public string SwitchPress(int imageId)
        {
            //var readText = "";
            //bool Flag = true;
            //bool toggleFlag = true;
            //var originalImage = (Image)ImageGetter.GetBitmap(0);
            //var modifiedImage = (Image)ImageGetter.GetBitmap(imageId);
            while (Flag)
            {
                //if (toggleFlag)
                //{

                //    pictureBox.Image = originalImage;
                //    toggleFlag = false;

                //}
                //else
                //{

                //    pictureBox.Image = modifiedImage;
                //    toggleFlag = true;
                //}
                //Thread.Sleep(250);
                if (useArdiuno)
                {
                    readText = ardiunoAdapter.Receive();
                    Console.WriteLine("Code From Ardiuno  " + readText);

                }
                else
                {
                    readText = Console.ReadLine();
                }
                //var readText = SerialPort.ReadLine();


                if (readText != "")
                {
                    return readText;

                }

            }
            return string.Empty;
        }
        public void Start() {
            Stopwatch stopWatch = new Stopwatch();
           
            var f = new Form1();
            while (true) {
                stopWatch.Start();
                //f.SwitchPress(1);
                Console.WriteLine(stopWatch.ElapsedMilliseconds);
                stopWatch.Stop();
                stopWatch.Reset();
            }
           
        }
    }
}
