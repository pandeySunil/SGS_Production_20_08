using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class CreateUser : Form
    {
        public CreateUser()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string error ="";
            if (textBoxPassword.Text == "" && textBoxUserName.Text == "") {
                error = "Either UserName OR Password is Blank";
                labeMessage.Text = error;
            }
            if (error == "") {
                SQLConnectionSetUp conObj = new SQLConnectionSetUp();
                var Con = conObj.GetConn();
                try
                {
                    
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert_User", Con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", textBoxUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", textBoxPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@isInUse", 0);
                    cmd.Parameters.AddWithValue("@IsAdmin", checkBox1.Checked);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) == 1)
                    {

                        labeMessage.Text = "User Name Already Exist";
                    }
                    else
                    {
                        labeMessage.Text = "User Created Sucessufully";
                    }
                }
                catch (Exception Ex)
                {

                }
                finally { Con.Close(); }
                
            }
            
        }
        private void CreateUser_Load(object sender, EventArgs e) {


        }

    }
}
