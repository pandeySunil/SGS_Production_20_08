using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        public int completed { get; set; }
        public int pending { get; set; }
        public bool useArdiuno = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("useArdiuno"));
        public double totalTacTime;
        public List<Rows> ExcelRows {get;set;}
        public bool backgroudThreadSleepFlag;
        public bool imageBlickFalg = true;
        BusinessLogic businessLogic { get; set; }
        public static SerialPort SerialPort;
        public ImageDataIntializer ImageGetter;
        //= new ImageGetter();
        Microsoft.Office.Interop.Excel.Range xlRange;
        public static int loopCount = 1;
        public static string ledOnCode;
        public static string ledOffCode;
        public static string trayCode;
        public static string sWInput;
        public static string switchCode;
        public static string preImageFile;
        public static string imageNewFile;
        public static bool imageFileChangeFlag;
        public Thread ImageLoadThread { get; set; }
        public Thread BackGroundThread;
        public string imgUrl;
        //public static int ImagIndex { get; set; }
        public static int counter { get; set; }
        public static String imageFileId;
        public Dictionary<string, Bitmap> OriginalImages = new Dictionary<string, Bitmap>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = "PRTNO";
            textTackTime.Text = "00";
            textBox1.Text = "00";
            textBox3.Text = "00";
            textBox5.Text = "00";
            try
            {
                Helper.target = Convert.ToInt32(ConfigurationManager.AppSettings.Get("target"));
                pending = Helper.target;
                completed = 0;
                textBox3.Text = Convert.ToString(completed);
                textBox5.Text = Convert.ToString(pending);
            }
            catch (Exception Ex) {
                MessageBox.Show("Please set the target");
            }

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

               // useArdiuno = Helper.ArdinoCon;
                //useArdiuno = true;
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
                    textBox2.Text = refVale;
                    
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
            if (Helper.assetFolderPath!=null&&Helper.assetFolderPath != "")
            {
                if (useArdiuno)
                {
                    ardiunoAdapter.HardWareFunction("000010000");
                }
                BackGroundThread.Start();
              
            }
            else {
                MessageBox.Show("NO asset folder is selected ");
            }
           
        }
        private void NavigationModified2()
        {
            //if (useArdiuno)
            //{
            //    ardiunoAdapter.Send(trayCode);
            //}
            var descMessage = "";
            string temp1;
            // var ImageLoadThread = new Thread(() => LoadImage(0));
            //ImageLoadThread.IsBackground = true;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            PictureBox.CheckForIllegalCrossThreadCalls = false;
            bool iterationFlag = true;
            int i = 0;
            //if (!ImageLoadThread.IsAlive)
            //{
            //  //  ImageLoadThread.Start();
            //}

            if (true)
            {
                trayCode = ExcelRows[i].TryLed;
                temp1 = ExcelRows[i].CavityLed;
                ImagIndex = Convert.ToInt32(ExcelRows[i].ImageId);
                imageFileId = ExcelRows[i].ImageFile;
                trayCode = "000" + trayCode + "00" + temp1;
                Console.WriteLine("trayCode");
                Console.WriteLine(trayCode);
                if (useArdiuno)
                {
                    ardiunoAdapter.Send(trayCode);
                }
                string temp2 = ExcelRows[i].DesMessage;
                string temp3 = ExcelRows[i].ConnectorNo;
                Console.WriteLine("Description Message");
                descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                textBoxDescription.Text = descMessage;
                Console.WriteLine(descMessage);
                ImagIndex = Convert.ToInt32(ExcelRows[i].ImageId);
                imageNewFile = ExcelRows[i].ImageFile;
                if (imageFileId!=ExcelRows[i].ImageFile|| imageFileId==null) {
                    imageFileId = ExcelRows[i].ImageFile;
                    imageFileChangeFlag = true;
                }
               
                while (iterationFlag)
                {
                    // iterationFlag = false;
                    if (i>=ExcelRows.Count())
                    {
                        //For tac time 
                        TimeSpan ts1 = stopWatch.Elapsed;
                        stopWatch.Stop();
                        ts1 = stopWatch.Elapsed;

                        textTackTime.Text = Convert.ToString(ts1.Seconds);
                        totalTacTime += Convert.ToDouble(ts1.Seconds);
                        // textToTal.Text = Convert.ToString(totalTacTime);
                        TimeSpan totalTactimespan = TimeSpan.FromSeconds(totalTacTime);

                        textToTal.Text = string.Format("{1:D2}m:{2:D2}s",
                                        totalTactimespan.Hours,
                                        totalTactimespan.Minutes,
                                        totalTactimespan.Seconds,
                                        totalTactimespan.Milliseconds);
                        return;
                    }

                    switchCode = "SW" + ExcelRows[i].SwCode;
                    Console.WriteLine("Switch-Code From sheet--- " + switchCode);
                    sWInput = SwitchPress(Convert.ToInt32(ExcelRows[i].ImageId));
                    Console.WriteLine("Switch-Code From Ardiuno--- " + sWInput);

                    var temp11 = sWInput.Contains(switchCode);
                    if (temp11)
                    {
                        i++;
                        if (true)
                        {
                            trayCode = ExcelRows[i].TryLed; ;
                            temp1 = ExcelRows[i].CavityLed; 
                            trayCode = "000" + trayCode + "00" + temp1;
                            Console.WriteLine("trayCode");
                            Console.WriteLine(trayCode);
                            ImagIndex = Convert.ToInt32(ExcelRows[i].ImageId);
                            imageFileId = ExcelRows[i].ImageFile;
                            if (useArdiuno)
                            {
                                ardiunoAdapter.Send(trayCode);
                            }
                            temp2 = ExcelRows[i].DesMessage; 
                            temp3 = ExcelRows[i].ConnectorNo; 
                            Console.WriteLine("Description Message");
                            descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                            Console.WriteLine(descMessage);
                            textBoxDescription.Text = descMessage;
                            ImagIndex = Convert.ToInt32(ExcelRows[i].ImageId);

                            iterationFlag = true;
                        }
                    }

                }
            }

            TimeSpan ts = stopWatch.Elapsed;
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
            textTackTime.Text = Convert.ToString(ts);

        }
        private void NavigationModified()
        {
           if (useArdiuno)
                {
                    ardiunoAdapter.Send(trayCode);
                }
           var descMessage = "";
            string temp1;
           // var ImageLoadThread = new Thread(() => LoadImage(0));
            //ImageLoadThread.IsBackground = true;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            PictureBox.CheckForIllegalCrossThreadCalls = false;
            bool iterationFlag = true;
            int i = 2;
            //if (!ImageLoadThread.IsAlive)
            //{
            //  //  ImageLoadThread.Start();
            //}
            
            if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 != null)
            {
                trayCode = xlRange.Cells[i, 2].Value2.ToString().Trim('/');
                temp1 = xlRange.Cells[i, 3].Value2.ToString().Trim('/');
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
                descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                textBoxDescription.Text = descMessage;
                Console.WriteLine(descMessage);
                ImagIndex = Convert.ToInt32(xlRange.Cells[i, 7].Value2.ToString().Trim('/'));
                while (iterationFlag)
                {
                    // iterationFlag = false;
                    if (xlRange.Cells[i, 4].Value2 == null) {
                        //For tac time 
                        TimeSpan ts1 = stopWatch.Elapsed;
                        stopWatch.Stop();
                        ts1 = stopWatch.Elapsed;
                         
                        textTackTime.Text = Convert.ToString(ts1.Seconds);
                        totalTacTime += Convert.ToDouble(ts1.Seconds);
                       // textToTal.Text = Convert.ToString(totalTacTime);
                        TimeSpan totalTactimespan = TimeSpan.FromSeconds(totalTacTime);

                        textToTal.Text = string.Format("{1:D2}m:{2:D2}s",
                                        totalTactimespan.Hours,
                                        totalTactimespan.Minutes,
                                        totalTactimespan.Seconds,
                                        totalTactimespan.Milliseconds);
                        return;
                    }

                    switchCode = "SW" + (xlRange.Cells[i, 4].Value2.ToString().Trim('/'));
                    Console.WriteLine("Switch-Code From sheet--- " + switchCode);
                    sWInput = SwitchPress(Convert.ToInt32(xlRange.Cells[i, 7].Value2.ToString().Trim('/')));
                    Console.WriteLine("Switch-Code From Ardiuno--- " + sWInput);

                    var temp11 = sWInput.Contains(switchCode);
                    if (temp11)
                    {
                        i++;
                        if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 != null)
                        {
                            trayCode = xlRange.Cells[i, 2].Value2.ToString().Trim('/');
                            temp1 = xlRange.Cells[i, 3].Value2.ToString().Trim('/');
                            trayCode = "000" + trayCode + "00" + temp1;
                            Console.WriteLine("trayCode");
                            Console.WriteLine(trayCode);
                            if (useArdiuno)
                            {
                                ardiunoAdapter.Send(trayCode);
                            }
                            temp2 = xlRange.Cells[i, 5].Value2.ToString().Trim('/');
                            temp3 = xlRange.Cells[i, 6].Value2.ToString().Trim('/');
                            Console.WriteLine("Description Message");
                            descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                            Console.WriteLine(descMessage);
                            textBoxDescription.Text = descMessage;
                            ImagIndex = Convert.ToInt32(xlRange.Cells[i, 7].Value2.ToString().Trim('/'));
                           
                            iterationFlag = true;
                        }
                    }

                }
            }

           TimeSpan ts = stopWatch.Elapsed;
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
            textTackTime.Text = Convert.ToString(ts);

        }
        private void Navigation()
        {
            ExcelSheetData e = new ExcelSheetData();
            ExcelRows =  e.GetRows(xlRange);
            Console.WriteLine("Excel Sheet Data Loaded");
            TextBox.CheckForIllegalCrossThreadCalls = false;
         var   ImageLoadThread = new Thread(() => LoadImage(0, ExcelRows[0].ImageFile));
            ImageLoadThread.IsBackground = true;
            ImageLoadThread.Start();
            while (true) {
                textBox1.Text = Convert.ToString(Helper.target);
                if ((Helper.target - completed) > 0) {
                    NavigationModified2();
                    completed++;
                    
                         textBox3.Text = Convert.ToString(completed);
                    textBox5.Text = Convert.ToString(Helper.target - completed);
                }
            }
            
            return;
            
            //var ImageLoadThread = new Thread(() => LoadImage(0));
            //ImageLoadThread.IsBackground = true;
           
            
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
                textBoxDescription.Text = descMessage;
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
                    string temp2 = xlRange.Cells[i, 5].Value2.ToString().Trim('/');
                    string temp3 = xlRange.Cells[i, 6].Value2.ToString().Trim('/');
                    var  descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                    textBoxDescription.Text = descMessage;
                    trayCode = xlRange.Cells[i, 2].Value2.ToString().Trim('/');

                    string temp1 = xlRange.Cells[i, 3].Value2.ToString().Trim('/');
                    trayCode = "000" + trayCode + "00" + temp1;
                    Console.WriteLine("trayCode");
                    Console.WriteLine(trayCode);
                    if (useArdiuno)
                    {
                        ardiunoAdapter.Send(trayCode);
                    }

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
                        trayCode = xlRange.Cells[i, 2].Value2.ToString().Trim('/');
                        descMessage = "Take Wire From " + temp2 + " put it into connecter NO." + temp3;
                        textBoxDescription.Text = descMessage;
                        temp1 = xlRange.Cells[i, 3].Value2.ToString().Trim('/');
                        trayCode = "000" + trayCode + "00" + temp1;
                        Console.WriteLine("trayCode");
                        Console.WriteLine(trayCode);
                        if (useArdiuno)
                        {
                            ardiunoAdapter.Send(trayCode);
                        }
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
                    textBox2.Text = fldrDlg.SelectedPath;
                    DataLoad();
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
            //var originalImage = (Image)ImageGetter.GetBitmap(0);
            //var modifiedImage = (Image)ImageGetter.GetBitmap(imageId);
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
        public void LoadImage(int imageId, string imageFile)
        {
            if (imageFileId != imageFile&& imageFileId!=null)
            {
                imageFile = imageFileId;
            }
            if (ImageGetter == null)
            {
                ImageGetter = new ImageDataIntializer();
                OriginalImages = ImageGetter.LoadOrginalImages();

            }

            var modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex, imageFile);
            var preImgIndex = ImagIndex;
            
            bool toggleFlag = false;
            //var imageId = 1;
            while (true)
            {
                try
                {
                    var originalKey = imageFile + ".json";
                    var originalImage = OriginalImages[originalKey];
                    if (imageFileId != imageFile)
                    {
                        imageFile = imageFileId;
                    }
                    if (true || preImgIndex != ImagIndex && preImageFile != imageFile || imageFileChangeFlag)
                    {
                        modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex, imageFile);
                        preImageFile = imageFile;
                        imageFileChangeFlag = false;
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
                catch (Exception Ex) {
                    MessageBox.Show(Ex.Message);
                    Application.Exit();
                }
            }
        }
        //Old Image Loader
        //public void LoadImage(int imageId) {
        //    if (ImageGetter == null) {
        //        ImageGetter = new ImageGetter();
        //    }
        //    var originalImage = (Image)ImageGetter.GetBitmap(0);
        //    var modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex);
        //    var preImgIndex = ImagIndex;
        //    bool toggleFlag = false;
        //    //var imageId = 1;
        //    while (imageBlickFalg)
        //    {
        //        if (preImgIndex != ImagIndex) {
        //            modifiedImage = (Image)ImageGetter.GetBitmap(ImagIndex);
        //        }
        //        if (toggleFlag)
        //        {

        //            pictureBox.Image = originalImage;
        //            toggleFlag = false;

        //        }
        //        else
        //        {

        //            pictureBox.Image = modifiedImage;
        //            toggleFlag = true;
        //        }
        //        Thread.Sleep(250);
        //    }
        //}

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
           // ImageGetter = new ImageGetter();



        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var printObj = new Printer();
            printObj.StartPrinting();
        }
    }
}