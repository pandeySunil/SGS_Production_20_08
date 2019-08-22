using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace ServerFileTransFerProj
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//        }
//    }
//}
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.IO;
using System.DirectoryServices;
using System.Net;
using System.Net.NetworkInformation;

namespace ServerFileTransFerProj
{
    class Program
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
         int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);
        public static double IPCount {get;set;}
       public static List<IPAddress> ipList = new List<IPAddress>();

        // Test harness.
        // If you incorporate this code into a DLL, be sure to demand FullTrust.
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static void Main(string[] args)
        {
            Console.WriteLine("REading Systems");
            var k = System.Environment.MachineName;
            string name1 = Environment.MachineName; //returns webserver
            string name2 = System.Net.Dns.GetHostName(); //returns webserver
                                                         // string name3 = System.Windows.Forms.SystemInformation.ComputerName; //returns webserver
            string name4 = System.Environment.GetEnvironmentVariable("COMPUTERNAME").ToString();//returns 
            GetSystems();

            Console.WriteLine("Reading Done");
            Console.Read();

            SafeTokenHandle safeTokenHandle;
            try
            {
                string userName, domainName;
                //domainName = Console.ReadLine();
                domainName = ".";

                Console.Write("Enter the login of a user on {0} that you wish to impersonate: ", domainName);
                //provide username of remote machine.
                userName = Console.ReadLine();
                //provide password of remote machine.
                Console.Write("Enter the password for {0}: ", userName);

                //Here's the Catch 
                //LOGON32_PROVIDER_WinNT50 = 3; and LOGON32_LOGON_NewCredentials = 9;
                const int LOGON32_PROVIDER_WinNT50 = 3;
                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_NewCredentials = 9;

                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(userName, domainName, Console.ReadLine(),
                    LOGON32_LOGON_NewCredentials, LOGON32_PROVIDER_WinNT50,
                    out safeTokenHandle);

                Console.WriteLine("LogonUser called.");

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    Console.WriteLine("LogonUser failed with error code : {0}", ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }
                using (safeTokenHandle)
                {
                    Console.WriteLine("Did LogonUser Succeed? " + (returnValue ? "Yes" : "No"));
                    Console.WriteLine("Value of Windows NT token: " + safeTokenHandle);

                    // Check the identity.
                    Console.WriteLine("Before impersonation: "
                        + WindowsIdentity.GetCurrent().Name);
                    // Use the token handle returned by LogonUser.
                    using (WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle()))
                    {
                        using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                        {

                            // Check the identity.
                            Console.WriteLine("After impersonation: "
                                + WindowsIdentity.GetCurrent().Name);
                            //File.Copy(Source File,DestinationFile);
                            File.Copy(@"C:\Users\Sunil.Pandey\Desktop\sgsproducion\SGS\SGS\SGS_TEST\Sheet.xlsx", @"\\192.168.0.7\Users\Akshay\Desktop\New folder\Sheet.xlsx", true);
                        }
                    }
                    // Releasing the context object stops the impersonation
                    // Check the identity.
                    Console.WriteLine("After closing the context: " + WindowsIdentity.GetCurrent().Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred. " + ex.Message);
            }
            Console.ReadLine();
        }
        private static void RequestHostAddress(IAsyncResult r)
        {
            Console.WriteLine(IPCount++);
            IPAddress ipAddr = (IPAddress)r.AsyncState;
            // ipAddr = (IPAddress)"192.168.0.6";
            
                if(ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                try
                {
                    Console.WriteLine(Dns.GetHostEntry(ipAddr).HostName);
                    //  Console.WriteLine(ipAddr);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                    //Console.WriteLine(ipAddr.ToString());
                }
                finally
                {
                    Dns.EndGetHostAddresses(r);
                }
            }
            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse("192.168.0.7");

            //string input = Convert.ToString(ipAddr);

            //IPAddress address;
            //if (IPAddress.TryParse(input, out address))
            //{
            //    switch (address.AddressFamily)
            //    {
            //        case System.Net.Sockets.AddressFamily.InterNetwork:
            //            Console.WriteLine(ipAddr);
            //            break;
            //        case System.Net.Sockets.AddressFamily.InterNetworkV6:
            //            // we have IPv6
            //            break;
            //        default:
            //            // umm... yeah... I'm going to need to take your red packet and...
            //            break;
            //    }
            //}
            
        }
        private static void GetSystems()
        {
           

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                IPInterfaceProperties ipProps;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (((nic.OperationalStatus == OperationalStatus.Up) && (nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)) && (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                    {
                        ipProps = nic.GetIPProperties();

                        byte[] addr;
                        string ip;

                        foreach (GatewayIPAddressInformation gip in ipProps.GatewayAddresses)
                        {
                            addr = gip.Address.GetAddressBytes();
                            
                            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
                            {
                                addr.SetValue((byte)i, 3);
                                ip = string.Format("{0}.{1}.{2}.{3}", addr[0], addr[1], addr[2], addr[3]);
                                if (ip.Length == 11&&ip.IndexOf("192.168")!=-1) {
                                    ipList.Add(IPAddress.Parse(ip));
                                    try
                                    {
                                        Console.WriteLine(Dns.GetHostEntry(IPAddress.Parse(ip)).HostName);
                                    }
                                    catch { };
                                
                                }
                               // Dns.BeginGetHostAddresses(ip, new AsyncCallback(RequestHostAddress), new IPAddress(addr));
                            }
                        }
                    }
                }
            }
            Console.WriteLine(ipList.Count());
            Console.ReadKey();
        }
    }
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
       
    }
}