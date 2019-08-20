using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class IOCheck : Form
    {
        public SerialPort serialPort;
        public string newValue;
        public string uniqueId = "000000";
        public int fixPreveNum = 0;
        public int preveNum = 0;
        public int caviPreveNum = 0;
        public string fixtureDisplayCode { get; set; }
        public string cavityDisplayCode { get; set; }
        public string trayCode { get; set; }
        public string fixtureCode { get; set; }
        public string cavityCode { get; set; }
        public IOCheck()
        {
            
            

            InitializeComponent();
            textBox1.Text = "0000";
            textBox4.Text = "0";
            textBox6.Text = "00";
            textBox2.Text = "10";
            textBox3.Text = "10";
            textBox5.Text = "10";
            textBox7.Text = "000000";
            label10.Text = "";
            label12.Text = "";
            label14.Text = "";
            serialPort = ArdiunoConnection.IntializeAudiun();
         serialPort.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //newValue = IncrementValue(textBox1.Text, Convert.ToInt32(textBox2.Text));
            //textBox1.Text = "00"+newValue;
            if (Convert.ToUInt32(textBox2.Text) > preveNum)
            {
                preveNum++;
                uniqueId = "0000";
                int newNumber = Convert.ToInt32(uniqueId) + preveNum;
                uniqueId = newNumber.ToString("0000");
                // MessageBox.Show(uniqueId);
                textBox1.Text = uniqueId;
                label12.Text = uniqueId;
                trayCode = uniqueId + "00000";
            }
            else {

                MessageBox.Show("Please increase the limit");

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //newValue =  DecrementValue(textBox1.Text);
            //textBox1.Text = "00"+newValue;
            if (preveNum > 1)
            {
                preveNum--;
                uniqueId = "0000";
                int newNumber = Convert.ToInt32(uniqueId) + preveNum;
                uniqueId = newNumber.ToString("0000");
                // MessageBox.Show(uniqueId);
                textBox1.Text = uniqueId;
                label12.Text = uniqueId;
                trayCode = uniqueId + "00000";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Fixture increment Button
            //newValue = IncrementValue(textBox4.Text, Convert.ToInt32(textBox3.Text));
            //textBox4.Text = newValue;
            if (Convert.ToInt32(textBox3.Text) > fixPreveNum)
            {
                fixPreveNum++;
                uniqueId = "00";
                int newNumber = Convert.ToInt32(uniqueId) + fixPreveNum;
                uniqueId = newNumber.ToString("00");
                // MessageBox.Show(uniqueId);
                textBox1.Text = uniqueId;
                fixtureCode = "00000" + uniqueId;
                textBox4.Text = uniqueId;

                if (Convert.ToInt32(uniqueId[0].ToString()) > 0)
                {

                    fixtureDisplayCode = uniqueId;
                    label10.Text = fixtureDisplayCode + cavityDisplayCode;
                }
                else
                {

                    fixtureDisplayCode = Convert.ToString(uniqueId[1]);
                    label10.Text = fixtureDisplayCode + cavityDisplayCode;
                }
            }
            else {
                MessageBox.Show("Please increase the limit");

            }
            // MessageBox.Show(fixtureCode);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //newValue = DecrementValue(textBox4.Text);
            //textBox4.Text = newValue;
            if (fixPreveNum > 1)
            {
                fixPreveNum--;
                uniqueId = "00";
                int newNumber = Convert.ToInt32(uniqueId) + fixPreveNum;
                uniqueId = newNumber.ToString("00");

                textBox1.Text = uniqueId;
                fixtureCode = "00000" + uniqueId;
                textBox4.Text = uniqueId;


                if (Convert.ToInt32(Convert.ToString(uniqueId[0])) > 0)
                {

                    fixtureDisplayCode = uniqueId;
                    label10.Text = fixtureDisplayCode + cavityDisplayCode;
                }
                else
                {

                    fixtureDisplayCode = Convert.ToString(uniqueId[1]);
                    label10.Text = fixtureDisplayCode + cavityDisplayCode;
                }
            }
            // MessageBox.Show(fixtureCode);
        }

        private void button11_Click(object sender, EventArgs e)
        {

            //    newValue = IncrementValue(textBox6.Text,Convert.ToInt32(textBox5.Text));
            //textBox6.Text = "0"+newValue;
            caviPreveNum++;
            uniqueId = "00";
            int newNumber = Convert.ToInt32(uniqueId) + caviPreveNum;
            uniqueId = newNumber.ToString("00");
            // MessageBox.Show(uniqueId);
            textBox6.Text = uniqueId;
            cavityCode = uniqueId;
            cavityDisplayCode = uniqueId;
            label10.Text = fixtureDisplayCode + uniqueId;
           // MessageBox.Show(cavityCode);

        }
       

        private void button9_Click(object sender, EventArgs e)
        {
            //newValue = DecrementValue(textBox6.Text);
            //textBox6.Text = "0"+newValue;
            if (caviPreveNum > 0)
            {
                caviPreveNum--;
                uniqueId = "00";
                int newNumber = Convert.ToInt32(uniqueId) + caviPreveNum;
                uniqueId = newNumber.ToString("00");
                // MessageBox.Show(uniqueId);
                textBox6.Text = uniqueId;
                cavityCode = uniqueId;
                cavityDisplayCode = uniqueId;
                label10.Text = fixtureDisplayCode + uniqueId;

                // MessageBox.Show(cavityCode);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(trayCode) == 0 || Convert.ToInt32(trayCode) < 0)
            {
                MessageBox.Show("Can't Send 0,please First Increment");
            }
            else {
                SendToAurdino(trayCode);
            }

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendToAurdino(newValue);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            SendToAurdino(newValue);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendToAurdino(cavityCode);
        }

        private string IncrementValue(string s, int i) {

            if ( Convert.ToInt32(s) <=i) {
                var temp = Convert.ToInt32(s);
                temp++;
                return Convert.ToString(temp);
            }
            return "";
        }
        private string DecrementValue(string s)
        {
            if (Convert.ToInt32(s)>0) { 
            var temp = Convert.ToInt32(s) - 1;
            return Convert.ToString(temp);
        }
        return "";
        }

        private void SendToAurdino(string s) {

           serialPort.Write(s);

        }

        private void ReadAurdino() {
            var flag = true;
            while (flag) {
                if (serialPort.ReadLine() != "") {
                    textBox8.Text = serialPort.ReadLine();
                    flag = false;
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReadAurdino();
           
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
           // MessageBox.Show(fixtureCode + cavityCode);
            SendToAurdino(fixtureCode+cavityCode);
        }

        private void IOCheck_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void PullCheckChange(object sender, EventArgs e)
        {
            label14.Text = textBox8.Text;
        }
    }
}
