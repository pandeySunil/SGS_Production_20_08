using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class FlashScreen : Form
    {
        public FlashScreen()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void FlashScreen_Load(object sender, EventArgs e)
        {
            if (Helper.assetFolderPath == null || Helper.assetFolderPath == "")
            {
                SetFolderPath();
            }
            GetIP();

            var l = new Login();
            l.Show();
            l.Focus();
           // Thread.Sleep();
            this.Hide();
        }
        private void GetIP() {

            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            Console.WriteLine(hostName);
            // Get the IP  
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            Console.WriteLine("My IP Address is :" + myIP);
            
        }
        private void SetFolderPath()
        {

            Helper.assetFolderPath = "";
            using (var fldrDlg = new FolderBrowserDialog())
            {
                //fldrDlg.Filter = "Png Files (*.png)|*.png";
                //fldrDlg.Filter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx|CSV Files (*.csv)|*.csv"

                if (fldrDlg.ShowDialog() == DialogResult.OK)
                {
                    Helper.assetFolderPath = fldrDlg.SelectedPath;
                    //fldrDlg.SelectedPath -- your result
                }
            }
        }
    }
}
