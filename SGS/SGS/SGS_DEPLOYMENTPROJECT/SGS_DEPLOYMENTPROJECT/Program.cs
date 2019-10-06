using System;
using System.Collections.Generic;
using System.Linq;
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
            Application.Run(Helper.FlashScreen);
            
        }

    }
}
 