using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;

namespace SGS_DEPLOYMENTPROJECT
{
   public class ArdiunoConnection
    {
        public static SerialPort mySerialPort;
   

        public static SerialPort IntializeAudiun()
        {

            try
            {
                var  PortName = ConfigurationManager.AppSettings.Get("portName");
               // PortName = "COM11";
                PortName = ConfigurationManager.AppSettings.Get("portName");
                mySerialPort = new SerialPort(PortName);
                mySerialPort.BaudRate = 115200;
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.DataBits = 8;
                mySerialPort.Handshake = Handshake.None;
                mySerialPort.RtsEnable = true;
                Helper.serialPort = mySerialPort;
                return mySerialPort;

            }
            catch (Exception Ex)
            {
                Console.WriteLine("ComPort:"+ ConfigurationManager.AppSettings.Get("portName"));
                Console.WriteLine("BaudRate:" + 115200);
            }
            return mySerialPort;
        }
    }
}
