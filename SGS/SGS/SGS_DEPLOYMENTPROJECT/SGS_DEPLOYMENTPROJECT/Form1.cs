using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGS_DEPLOYMENTPROJECT;

namespace SGS_DEPLOYMENTPROJECT
{
    public partial class Form1 : Form
    {
        public ArdiunoAdapter ardiunoAdapter { get; set; }
        public int ImagIndex { get; set; }
        public bool useArdiuno = true;

        public bool backgroudThreadSleepFlag;
        public bool imageBlickFalg = true;
        BusinessLogic businessLogic { get; set; }
        public static SerialPort SerialPort;
        public ImageGetter ImageGetter;
        //= new ImageGetter();
        Microsoft.Office.Interop.Excel.Range xlRange;
        public static int loopCount = 1;
        public static string ledOnCode;
        public static string ledOffCode;
        public static string trayCode;
        public static string sWInput;
        public static string switchCode;
        public Thread BackGroundThread;
        public string imgUrl;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = "PRTNO";
            textBox1.Text = "00";
            textBox3.Text = "00";
            textBox5.Text = "00";
            textBox2.ForeColor = System.Drawing.Color.GreenYellow;
            textBox1.ForeColor = System.Drawing.Color.GreenYellow;
            textBox3.ForeColor = System.Drawing.Color.GreenYellow;
            textBox5.ForeColor = System.Drawing.Color.GreenYellow;
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");
            labelDate.Text = DateTime.Now.ToShortDateString();
            btnOn.BackColor = System.Drawing.Color.Red;
            btnOff.BackColor = System.Drawing.Color.Red;
            pictureBox.BackColor = System.Drawing.Color.AliceBlue;
            textToTal.BackColor = System.Drawing.Color.Yellow;
            textTackTime.BackColor = System.Drawing.Color.Yellow;

            btnStart.BackColor = System.Drawing.Color.Green;
            btnBck.BackColor = System.Drawing.Color.Yellow;
            btnStop.BackColor = System.Drawing.Color.Red;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            lableLoggedInUser.Text = "Hello, "+Helper.LoggedInUserName;
            
            if (Helper.ExcelSheetName == "")
            {

                MessageBox.Show("Please Select  Sheet To Be Loaded");
            }
            //SelectExcelFile();
           // SetFolderPath();
           
          
            BackGroundThread = new Thread(() => Navigation());
            BackGroundThread.IsBackground = true;
            //SerialPort = ArdiunoConnection.IntializeAudiun();
            //textBoxDevelopersArea.Text += "Opening Serial Port\\n";
            //SerialPort.Open();
            businessLogic = new BusinessLogic();
            //Helper.ExcelSheetName = "";


            // textBoxDevelopersArea.Text += "Sheet Intiallized\\n";
            // pictureBox.Dock = DockStyle.Fill;
            if (Helper.assetFolderPath == null || Helper.assetFolderPath == "")
            {
               // ImageLoadThread.Start();
            }
            Console.WriteLine("Sytem Running in Dev mode----");
            if (useArdiuno) {
                ardiunoAdapter = new ArdiunoAdapter(useArdiuno);

                useArdiuno = Helper.ArdinoCon;
            }
           // DataLoad();
          //  ImageGetter = new ImageGetter();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // new FileSelect().Show();
            //MessageBox.Show(Properties.Settings.Default.FileSelectMode.ToString());

            // SelectExcelFile();
            switch (Properties.Settings.Default.FileSelectMode) { 
                case 1:
                    SetFolderPath();
                break;
            case 2:
                    BarCodeFileSelect();
                    
                    
                break;
            case 3:
                    MessageBox.Show("E-Planning Feature In Progress");
                    break;


            }
        }
        private void BarCodeFileSelect() {
            var fileSelectForm = new FileSelectSettingInput();
            // fileSelectForm.Show();
            //fileSelectForm.Focus();
            // fileSelectForm.
            var dilogInputbox = new InputDilogBox();
             var  refVale = "";
            ;
            if ((dilogInputbox.InputBox("Use Barcode Reader ", "", ref refVale)) == DialogResult.OK)
            {
                Helper.assetFolderPath = @"C:\" + @"SGS_BASEDICRECTORY\"+refVale;
                if (!Directory.Exists(Helper.assetFolderPath))
                {
                    MessageBox.Show(refVale + "Folder Doesn't Exist");
                }
                else {
                    DataLoad();
                    
                }
            }
           

        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
           // DialogResult dialogResult = MessageBox.Show("LogOut will Close the Application Do you Wish To Continue ", "LogOut", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    BackGroundThread.Abort();
            //    Application.Exit();
            //    LogOut();
            //}
            //else if (dialogResult == DialogResult.No)
            //{
            //    return;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BackGroundThread.Abort();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (useArdiuno) {
                ardiunoAdapter.HardWareFunction("000010000");
            }
            BackGroundThread.Start();
            // textBoxDevelopersArea.Text += "Navigation Thread is Stated\\n";
            //if (Helper.ExcelSheetName != ""||true)
            //{
            //    businessLogic.InitializeExcel();
            //    xlRange = businessLogic.InitializeExcel();
            //    BackGroundThread.Start();
            //}

            //else {
            //    MessageBox.Show("First Choose The Excel File");
            //}
            //new Thread(() => Navigation()) { IsBackground = true }.Start();
        }
        private void Navigation()
        {
            var ImageLoadThread = new Thread(() => LoadImage(0));
            ImageLoadThread.IsBackground = true;
           
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            
            
            TextBox.CheckForIllegalCrossThreadCalls = false;
            PictureBox.CheckForIllegalCrossThreadCalls = false;
           // SerialPort = ArdiunoConnection.IntializeAudiun();
           // textBoxDevelopersArea.Text += "Opening Serial Port\\n";
            //SerialPort.Open();
            //businessLogic = new BusinessLogic();
            //if (Helper.assetFolderPath != "" || Helper.assetFolderPath != null)
            //{
            //    businessLogic.InitializeExcel();
            //    xlRange = businessLogic.InitializeExcel();
            //}
           // textBoxDevelopersArea.Text += "Sheet Intiallized\\n";

            int Count = 1;
            ImageLoadThread.Start();
            int i = 2;
            bool sheetDataFlag = true;
            bool indexIncrementFlag = false;
            if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 != null)
            {


                trayCode = xlRange.Cells[i, 2].Value2.ToString().Trim('/');

                string temp1 = xlRange.Cells[i, 3].Value2.ToString().Trim('/');
                trayCode = "000" + trayCode + "00" + temp1;
                Console.WriteLine("trayCode");
                Console.WriteLine(trayCode);
                if (useArdiuno)
                {
                    ardiunoAdapter.Send(trayCode);
                }
                string temp2 = xlRange.Cells[i, 5].Value2.ToString().Trim('/');
                string temp3 = xlRange.Cells[i, 6].Value2.ToString().Trim('/');
                Console.WriteLine("Description Message");
                var descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                Console.WriteLine(descMessage);
                ImagIndex = Convert.ToInt32(xlRange.Cells[i, 7].Value2.ToString().Trim('/'));
                ledOnCode = trayCode + temp1;
                ledOffCode = trayCode + temp2;
                indexIncrementFlag = true;
            }
            while (sheetDataFlag)
            {
              
                Console.WriteLine(Count);
                Console.WriteLine("Started----");
                if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 != null)
                {

                    
                    switchCode = "SW" + (xlRange.Cells[i, 4].Value2.ToString().Trim('/'));

                    Console.WriteLine("Press: " + switchCode);
                   
                    sWInput = SwitchPress(Convert.ToInt32(xlRange.Cells[i, 7].Value2.ToString().Trim('/')));
                    if (indexIncrementFlag)
                    {
                        i++;
                        indexIncrementFlag = false;
                    }
                    // textBoxDescription.Text = descMessage;

                    var temp11 = sWInput.Contains(switchCode);
                  
                    var ardinoInputCode = switchCode + "\r";
                    var consoleInputCode = switchCode;

                    var ardinoInputCodefalg = string.Equals(sWInput, ardinoInputCode, StringComparison.OrdinalIgnoreCase);
                    var consoleInputCodefalg = string.Equals(sWInput, consoleInputCode, StringComparison.OrdinalIgnoreCase);
                    if (ardinoInputCodefalg || consoleInputCodefalg || temp11)
                    {
                        switchCode = xlRange.Cells[i, 6].Value2.ToString().Trim('/');
                        temp11 = sWInput.Contains(switchCode);
                        ardinoInputCode = switchCode + "\r";
                        consoleInputCode = switchCode;
                        ardinoInputCodefalg = string.Equals(sWInput, ardinoInputCode, StringComparison.OrdinalIgnoreCase);
                        consoleInputCodefalg = string.Equals(sWInput, consoleInputCode, StringComparison.OrdinalIgnoreCase);
                        ImagIndex = Convert.ToInt32(xlRange.Cells[i, 7].Value2.ToString().Trim('/'));
                        indexIncrementFlag = true;
                        Count++;

                    }

                   
                    if (backgroudThreadSleepFlag) {
                        while (true) {

                        }
                    }
                }
            }
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
            textTackTime.Text = elapsedTime;

        }
        private void SetFolderPath()
        {

           

            Helper.assetFolderPath = "";
           // @"C:/" + "SGS_BASEDICRECTORY"

            using (var fldrDlg = new FolderBrowserDialog())
            {
                fldrDlg.SelectedPath  = @"C:/ " + "SGS_BASEDICRECTORY";
               
                if (fldrDlg.ShowDialog() == DialogResult.OK)
                {
                    Helper.assetFolderPath = fldrDlg.SelectedPath;
                    //fldrDlg.SelectedPath -- your result
                }
            }
        }
        private void btnOn_Click(object sender, EventArgs e)
        {



        }
        public  string SwitchPress(int imageId)
        {
            var readText = "";
            bool Flag = true;
          bool   toggleFlag = true;
            var originalImage = (Image)ImageGetter.GetBitmap(0);
            var modifiedImage = (Image)ImageGetter.GetBitmap(imageId);
            while (Flag)
            {
                //if (toggleFlag)
                //{

                //    pictureBox.Image = originalImage;
                //    toggleFlag = false;

                //}
                //else
                //{

                //    pictureBox.Image = modifiedImage;
                //    toggleFlag = true;
                //}
                //Thread.Sleep(250);
                if (useArdiuno)
                {
                    readText = ardiunoAdapter.Receive();
                    Console.WriteLine("Code From Ardiuno  " + readText);

                }
                else
                {
                    readText = Console.ReadLine();
                }
                //var readText = SerialPort.ReadLine();


                if (readText != "")
                {
                   return readText;

                }

            }
            return string.Empty;
        }
        private void SelectExcelFile()
        {
            var filePath = "";
            // Create and open a file selector
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.InitialDirectory = @"C:/ " + "SGS_BASEDICRECTORY";

            // select filter type
            //opnDlg.Filter = "Parm Files (*.parm)|*.parmAll Files (*.*)|*.*";

            if (opnDlg.ShowDialog(this) == DialogResult.OK)
            {
                
                FileInfo f = new FileInfo(opnDlg.FileName);
                //f.
                
                if (f.DirectoryName == Application.StartupPath)
                    filePath = f.Name;  // only file name
                else if (f.DirectoryName.StartsWith(Application.StartupPath))
                    // relative path
                    filePath = f.FullName.Replace(Application.StartupPath, ".");
                else
                    filePath = f.FullName;  // Full path
            }
            Helper.ExcelSheetName = filePath;
            if (Application.OpenForms.OfType<Form1>().Count() > 0)
                Application.OpenForms.OfType<Form1>().First().Close();

            Form1 frm = new Form1();
            frm.Show();

        }
        private void  LogOut() {

            SQLConnectionSetUp conObj = new SQLConnectionSetUp();
            var Con = conObj.GetConn();
            try
            {

                Con.Open();
                SqlCommand cmd = new SqlCommand("LogOut", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", Helper.LoggedInUserName);

                cmd.ExecuteNonQuery();

                }

            catch (Exception Ex)
            {

            }
            finally { Con.Close(); }

            

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            
           

            try
            {
                MessageBox.Show("Closeing Application ");
                LogOut();


            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroudThreadSleepFlag = true;
            DialogResult dialogResult = MessageBox.Show("Setting Will Terminate The Current Cycle", "Setting", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                BackGroundThread.Abort();
                var settingForm = new SettingForm();
                if (settingForm != null) {
                    settingForm.Show();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                backgroudThreadSleepFlag = false;
                return;
            }
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");

        }
       
        public void LoadImage(int imageId) {
            if (ImageGetter == null) {
                ImageGetter = new ImageGetter();
            }
            var originalImage = (Image)ImageGetter.GetBitmap(0);
            var modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex);
            var preImgIndex = ImagIndex;
            bool toggleFlag = false;
            //var imageId = 1;
            while (imageBlickFalg)
            {
                if (preImgIndex != ImagIndex) {
                    modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex);
                }
                if (toggleFlag)
                {

                    pictureBox.Image = originalImage;
                    toggleFlag = false;

                }
                else
                {

                    pictureBox.Image = modifiedImage;
                    toggleFlag = true;
                }
                Thread.Sleep(250);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (useArdiuno) {
                ardiunoAdapter.HardWareFunction("000020000");
            }
        }

        private void btnBck_Click(object sender, EventArgs e)
        {
            if (useArdiuno)
            {
                ardiunoAdapter.HardWareFunction("000030000");
            }
        }
        private void DataLoad()
        {
              // businessLogic.InitializeExcel();
                xlRange = businessLogic.InitializeExcel();
            ImageGetter = new ImageGetter();



        }
    }
}