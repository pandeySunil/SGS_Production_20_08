using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class PrinterSettingForm : Form
    {
        public PrinterSettingForm()
        {
            InitializeComponent();
        }

        private void PrinterSettingForm_Load(object sender, EventArgs e)
        {
            try {
                Configuration configuration = ConfigurationManager.
                  OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

                textBox4.Text = configuration.AppSettings.Settings["PrintedCompanyName"].Value;
                textBox6.Text = configuration.AppSettings.Settings["StartingNumber"].Value;
                textBox4.ReadOnly = true;
                textBox6.ReadOnly = true;

            }
            catch (Exception Ex) {
                MessageBox.Show("Something Went Wrong While Loading Current Printer Preferences Please Look Into Config File");
            }
            
        }

        private void SetConfigValues(string key,string value) {

            Configuration configuration = ConfigurationManager.
                    OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            configuration.AppSettings.Settings[key].Value = Convert.ToString(value);
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
          //  MessageBox.Show(ConfigurationManager.AppSettings.Get("stationName"));
            
        }

        private bool ValidateValues() {
            var formvalid = true;

            if (textBox1.Text == null || textBox1.Text == "") {
                formvalid = false;
                MessageBox.Show("The Company Name Can't Be Kept Empty");
            }

            try {
                Convert.ToInt32(textBox2.Text);
            }

            catch(Exception Ex) {
                formvalid = false;
                MessageBox.Show("The Start Number Can only be number");
            }
            return formvalid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateValues())
            {


            }
            else {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ValidateValues())
            {
                SetConfigValues("PrintedCompanyName", textBox1.Text);
                SetConfigValues("StartingNumber", textBox3.Text + textBox2.Text);
            }
        }
    }
}
