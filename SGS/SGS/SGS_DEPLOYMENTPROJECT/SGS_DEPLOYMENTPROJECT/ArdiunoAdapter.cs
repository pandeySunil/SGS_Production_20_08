using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS_DEPLOYMENTPROJECT
{
    public class ArdiunoAdapter
    {
        public SerialPort serialPort { get; set; }
        public string input { get; set; }
        public ArdiunoAdapter(bool intiateArdiuno) {
            try {

                if (intiateArdiuno||true)
                {
                    serialPort = ArdiunoConnection.IntializeAudiun();
                    serialPort.Open();
                    Helper.ArdinoCon = true;
                }
            }
            catch (Exception e) {
                Console.WriteLine("Cannot Open Ardino Connection Using Serial window, close and open application to retry");
                Console.WriteLine("Cannot Open Ardino Connection Using Serial window, close and open application to retry");
                Helper.ArdinoCon = false;

            }
           
            
        }

        public void GetInputArdiuno() {

        }
         public void Send(string s)
        {
            try
            {
                serialPort.Write(s);
            }
            catch (Exception e)
            {
                Console.WriteLine("InnerException  " + e.InnerException);
                Console.WriteLine("Message  " + e.Message);
                Console.WriteLine("Message  " + e.StackTrace);
            }
        }
        public string Receive() {
             input = String.Empty;
            input =  serialPort.ReadLine();

            return input;
        }
        public void HardWareFunction(string s) {
            try {
                serialPort.Write(s);
            }
            catch (Exception e) {
                Console.WriteLine("InnerException  "+e.InnerException);
                Console.WriteLine("Message  " + e.Message);
                Console.WriteLine("Message  " + e.StackTrace);
            }

        }
    }
}
