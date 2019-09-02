using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class FlashScreen : Form
    {
        public Thread BackGroundThread { get; set; }

        public FlashScreen()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
           // WindowState = FormWindowState.Maximized;
           BackGroundThread = new Thread(() => BackGroundProc());
            BackGroundThread.IsBackground = true;
            BackGroundThread.SetApartmentState(ApartmentState.STA);
            if (!Directory.Exists(@"C:/" + "SGS_BASEDICRECTORY")){
                Directory.CreateDirectory(@"C:/" + "SGS_BASEDICRECTORY");
            }


        }
        private void BackGroundProc() {
            if (Helper.assetFolderPath == null || Helper.assetFolderPath == "")
            {
               // SetFolderPath();
            }
            GetIP();

           
            // Thread.Sleep();
            //this.Hide();
           
            Helper.FlashScreen.Invoke(new MethodInvoker(delegate {
                var l = new Login();
                l.Show();
                l.Focus();
                Helper.FlashScreen.Hide();

               // Helper.FlashScreen.Dispose();
            }));
        }
        [STAThread]
        private void FlashScreen_Load(object sender, EventArgs e)
        {
            BackGroundThread.Start();
           // BackGroundThread.Join();
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
                var k = Convert.ToInt32(cmd.ExecuteScalar());
                //MessageBox.Show("System Info Saved");
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Syteme Info Could Not Be saved  " +"Error Message "+Ex.Message);

            }
            finally { Con.Close(); }

           
        







    }
        
    }
}
