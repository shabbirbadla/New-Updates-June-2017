using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.ComponentModel;


namespace SystemInfo
{
    public class SysInformation
    {
        public string GetSystemInformation(string passParameter)
        {
            if (passParameter.ToUpper() == "M")
            {
                return FindMACAddress();
            }
            else if (passParameter.ToUpper() == "P")
            {
                return FindMyProcessorID();
            }
            else
            {
                return "Invalid Parameter value passed";
            }
        }

        /// <summary>
        /// Returns MAC Address from first Network Card in Computer
        /// </summary>
        /// <returns>MAC Address in string format</returns>
        
        private string FindMACAddress()
        {
            //create out management class object using the
            //Win32_NetworkAdapterConfiguration class to get the attributes
            //af the network adapter
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //create our ManagementObjectCollection to get the attributes with
            ManagementObjectCollection objCol = mgmt.GetInstances();
            string address = String.Empty;
            //loop through all the objects we find
            foreach (ManagementObject obj in objCol)
            {
                if (address == String.Empty)  // only return MAC Address from first card
                {
                    //grab the value from the first network adapter we find
                    //you can change the string to an array and get all
                    //network adapters found as well
                    if ((bool)obj["IPEnabled"] == true) address = obj["MacAddress"].ToString();
                }
                //dispose of our object
                obj.Dispose();
            }
            //replace the ":" with an empty space, this could also
            //be removed if you wish
            address = address.Replace(":", "");
            //return the mac address
            return address;
        }

        //private string FindNumberOfProcessors()
        //{
        //    ManagementObjectSearcher mgmtObjects = new ManagementObjectSearcher("Select * from Win32_ComputerSystem");
        //    foreach (var item in mgmtObjects.Get())
        //    {
        //        Console.WriteLine("Number Of Processors - " + item["NumberOfProcessors"]);
        //        Console.WriteLine("Number Of Logical Processors - " + item["NumberOfLogicalProcessors"]);
        //    }
        //}

        private string FindMyProcessorID()
        {
            ManagementObjectSearcher mgmtObjSrch = new ManagementObjectSearcher("select * from win32_processor");
            foreach (var srchItem in mgmtObjSrch.Get())
            {
                return srchItem["ProcessorID"].ToString();
            }
            return "Processor ID not found";
        }
    }
}
