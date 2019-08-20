using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;
using System.Diagnostics;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            string stationName;
            string PortName;
            stationName = ConfigurationManager.AppSettings.Get("stationName");
            PortName = ConfigurationManager.AppSettings.Get("portName");

            InitializeComponent();

            textBoxPort.Text = PortName;
            textBoxStationName.Text = stationName;
            textBoxPort.ReadOnly = true;
            textBoxStationName.ReadOnly = true;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxStationName.ReadOnly)
            {
                DialogResult dialogResult = MessageBox.Show("Do You Want To Change StatioName (Enter Value In Text Box and Click)", "Stationn Name", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    textBoxStationName.ReadOnly = false;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                Configuration configuration = ConfigurationManager.
                    OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                configuration.AppSettings.Settings["stationName"].Value = textBoxStationName.Text;
                configuration.Save();

                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show(ConfigurationManager.AppSettings.Get("stationName"));
                textBoxStationName.ReadOnly = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxPort.ReadOnly)
            {
                DialogResult dialogResult = MessageBox.Show("Do You Want To Change Port Name (Enter Value In Text Box and Click)", "Port Name", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    textBoxPort.ReadOnly = false;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                Configuration configuration = ConfigurationManager.
                    OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                configuration.AppSettings.Settings["portName"].Value = textBoxPort.Text;
                configuration.Save();

                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show(ConfigurationManager.AppSettings.Get("portName"));
                textBoxPort.ReadOnly = true;
            }


        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process proc = new Process();

            try
            {
                string batDir = string.Format(@"C:\Program Files (x86)\TeamViewer\");
                proc.StartInfo.WorkingDirectory = batDir;
                proc.StartInfo.FileName = "TeamViewer.exe";
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit();
                proc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new CreateUser().Show();
        }
    }
}
