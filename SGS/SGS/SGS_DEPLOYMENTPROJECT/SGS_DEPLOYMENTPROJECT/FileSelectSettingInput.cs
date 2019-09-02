using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class FileSelectSettingInput : Form
    {
        public static string txtBoxVal { get; set; }
        public FileSelectSettingInput()
        {
            InitializeComponent();
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(fileSelectFormEnterKeyPress);
        }
        private void fileSelectFormEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)

            {
               
                Helper.assetFolderPath = @"C:/ " + "SGS_BASEDICRECTORY/"+textBox1.Text;
                

            }
            this.Hide();
            this.Close();
        }
    }
}
