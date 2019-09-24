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
        public Performance______Moniter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          Thread BackGroundThread = new Thread(() => Start());
            BackGroundThread.IsBackground = true;
        }
        public void Start() {
            Stopwatch stopWatch = new Stopwatch();
           
            var f = new Form1();
            while (true) {
                stopWatch.Start();
                f.SwitchPress(1);
                Console.WriteLine(stopWatch.ElapsedMilliseconds);
                stopWatch.Stop();
                stopWatch.Reset();
            }
           
        }
    }
}
