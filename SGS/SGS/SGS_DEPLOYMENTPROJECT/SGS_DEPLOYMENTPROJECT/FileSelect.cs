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
    public partial class FileSelect : Form
    {
        public FileSelect()
        {
            InitializeComponent();
            DirectoryInfo dinfo = new DirectoryInfo("C:\\Users\\Sunil.Pandey\\Desktop\\SGS\\SGS_Sheets");
            FileInfo[] Files = dinfo.GetFiles("*.xlsx");
            foreach (FileInfo file in Files)
            {
                listBox1.Items.Add(file.Name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (ListBox)sender;
            Helper.ExcelSheetName =  s.SelectedItem.ToString();
            MessageBox.Show("Slecting File:" + Helper.ExcelSheetName);
            this.Hide();
            
        }
    }
}
