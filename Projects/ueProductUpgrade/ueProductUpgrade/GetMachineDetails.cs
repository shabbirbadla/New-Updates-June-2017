using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace ueProductUpgrade
{
    class GetMachineDetails
    {
        public static string MachineName()
        {
            return System.Net.Dns.GetHostName().ToUpper();
        }
        public static string IpAddress()
        {
            string retVal = string.Empty;
            ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            ManagementObjectCollection objCollection = objSearcher.Get();
            foreach (ManagementObject obj in objCollection)
            {
                string[] AddressList = (string[])obj["IPAddress"];
                retVal = AddressList[0].ToString();
                break;
            }
            if (retVal.Length == 0)
                retVal = "127.0.0.1";
            return retVal;
        }
        public static string ProcessorId()
        {
            string processorID = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * FROM WIN32_Processor");
            ManagementObjectCollection mObject = searcher.Get();

            foreach (ManagementObject obj in mObject)
            {
                processorID = obj["ProcessorId"].ToString();
                break;
            }
            return processorID; 
        }
    }
}
