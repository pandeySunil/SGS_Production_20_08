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
            if (intiateArdiuno) {
                serialPort = ArdiunoConnection.IntializeAudiun();
                serialPort.Open();
            }
            
        }

        public void GetInputArdiuno() {

        }
         public void Send(string s)
        {
            serialPort.Write(s);
        }
        public string Receive() {
             input = String.Empty;
            input =  serialPort.ReadLine();

            return input;
        }
        public void HardWareFunction(string s) {
            serialPort.Write(s);

        }
    }
}
