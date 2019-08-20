using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Are you sure about your credentilas");
            
            if (true||ValidateUser()) {
                //if (Application.OpenForms.OfType<Login>().Count() > 0)
                //{
                //    Application.OpenForms.OfType<Login>().First().Close();
                //}
                lblError.Text = "SuccessFully Logged In ";
                new Form1().Show();
                
            }

            else {

                lblError.Text = "Wrong Credentials Please Enter New One";


            }
            
            
        }

        private bool ValidateUser() {
            bool flag = false;
            SQLConnectionSetUp conObj = new SQLConnectionSetUp();
            var Con = conObj.GetConn();
            try
            {

                Con.Open();
                SqlCommand cmd = new SqlCommand("ValidateUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", textBoxUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", textBoxPassword.Text.Trim());


                if (Convert.ToInt32(cmd.ExecuteScalar()) == 1)
                {
                    Helper.LoggerInUserIsAdmin = true;
                    Helper.LoggedInUserName = textBoxUsername.Text;
                    flag = true;

                }
                else if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    Helper.LoggerInUserIsAdmin = false;
                    Helper.LoggedInUserName = textBoxUsername.Text;
                    flag = true;
                }
                else {
                    flag = false;
                }
            }
            catch (Exception Ex)
            {
                flag = false;
            }
            finally { Con.Close(); }

            return flag;
        }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
