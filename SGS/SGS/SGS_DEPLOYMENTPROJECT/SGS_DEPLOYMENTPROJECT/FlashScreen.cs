using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            SetIP(hostName, myIP);


        }
        private void SetIP(string SystemName, string IpAddress) {
            bool flag;
            SQLConnectionSetUp conObj = new SQLConnectionSetUp();
            var Con = conObj.GetConn();
            try
            {

                Con.Open();
                SqlCommand cmd = new SqlCommand("SetSystems", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SystemName", SystemName.Trim());
                cmd.Parameters.AddWithValue("@IPAddress", IpAddress.Trim());
                cmd.Parameters.AddWithValue("@UserIdLoggedIn", 1);
                MessageBox.Show("System Info Saved");

               
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Syteme Info Could Not Be saved  " +"Error Message "+Ex.Message);

            }
            finally { Con.Close(); }

           
        







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
