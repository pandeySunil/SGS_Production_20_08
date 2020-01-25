using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Login());
            Helper.form1 = new Form1();
            Helper.FlashScreen = new FlashScreen();
            //Helper.iOCheck = new IOCheck();

            // Application.Run(new NewImageGetterTester());
            if (VaLidateApplication())
            {
                Application.Run(Helper.FlashScreen);
                
            }
            else {
                Application.Exit();
            }
            //Application.Run(new PrinterSettingForm());

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            Console.WriteLine(e.Exception.Message);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            Console.WriteLine((e.ExceptionObject as Exception).Message);
        }
        private  static bool VaLidateApplication()
        {
            var Valid = true;

            Configuration configuration = ConfigurationManager.
                      OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            List<string> allConfigKey = new List<string>();
            allConfigKey.Add("stationName");
            allConfigKey.Add("portName");
            allConfigKey.Add("target");
            allConfigKey.Add("useArdiuno");
            allConfigKey.Add("stationName");
            allConfigKey.Add("autoPrint");
            allConfigKey.Add("StartingNumber");
            allConfigKey.Add("PrintedCompanyName");




            var absentKeys = allConfigKey.Where(w => !(configuration.AppSettings.Settings.AllKeys.ToList().Contains(w))).ToList();
            if (absentKeys != null && absentKeys.Any())
            {
                var abKeys = "";
                foreach (var a in absentKeys)
                {
                    abKeys += "\n" + a;

                }
                MessageBox.Show("Please See The Aplication Config File The Given Keys Are Absent" + abKeys);
                return false;
            }





            return Valid;


        }

    }
    }
