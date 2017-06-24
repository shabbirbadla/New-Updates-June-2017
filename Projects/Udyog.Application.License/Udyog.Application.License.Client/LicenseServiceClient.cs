using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Management;
using System.Timers;
using Udyog.Application.License;
using System.Reflection;
using System.Xml;
using System.ComponentModel;

namespace LicenseClient
{
    [GuidAttribute("F1761F75-4DB9-4a3d-A640-A6F2E41F375C")]
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LicenseServiceClient : IDisposable
    {
        private const string VALID_CODE = "CUSSECC";
        private const string PASSKEY = "UDYOG";
        private const int TIMER_INTERVAL = 70000;

        private InstanceContext m_context = null;
        //ILicenseContract m_proxy = null;
        private string m_serviceAddress = string.Empty;
        private Timer m_renewTimer = null;

        // Track whether Dispose has been called.
        private bool disposed = false;

        // Added by Shrikant S. on 05/07/2013 for Bug-16557             //Start        
        //To take UserId also in security code.
        // This is due to same user is logging 3 times
        private string _LoggedUser = string.Empty;
        public string LoggedUser
        {
            get { return _LoggedUser; }
            set { _LoggedUser = value; }
        }
        public void SetLoggedInUser(string username)
        {
            LoggedUser = username;
        }
        //Added by Shrikant S. on 05/07/2013 for Bug-16557             //Start
        public bool IsUserLogged()
        {
            ILicenseContract proxy = GetProxy();
            string securityCode = GenerateSecurityCode();
            return proxy.IsUserAlreadyLoggedIn(LoggedUser, securityCode);
        }
        //Added by Shrikant S. on 05/07/2013 for Bug-16557             //End

        public LicenseServiceClient()
        {
            //m_context = new InstanceContext(this);
            //m_serviceAddress = new System.Configuration.AppSettingsReader().GetValue("licenseServiceAddress", typeof(string)).ToString();
            m_serviceAddress = GetConfigValue("licenseServiceAddress");

            //m_proxy = new Proxy(m_context, Assembly.GetAssembly(this.GetType()).Location);
        }

        public string ValidateLicense()
        {
            string reachedValue = string.Empty;
            //throw new Exception("validate reached-1");
            ILicenseContract proxy = GetProxy();
            Trace.Assert(proxy != null, "The connection channel to the Service is disconnected.");
            //throw new Exception("validate reached-2");
            string securityCode = GenerateSecurityCode();
            //throw new Exception("validate reached-3");

            string encrypted = CryptoHelper.Encrypt(GetKey(), securityCode);
            //throw new Exception("validate reached-4");
            
            if (proxy.IsUserAlreadyLoggedIn(LoggedUser, securityCode))
            {
                throw new Exception("User " + LoggedUser + " already logged in.");
                //return "User " + LoggedUser + " already logged in.";
            }
            //throw new Exception("validate reached-5");
            string returnCode = proxy.ValidateLicense(encrypted);
            //throw new Exception("validate reached-6");

            return CryptoHelper.Decrypt(GetKey(), returnCode);
        }

        public bool CreateAndVerifyLicenseConnection()
        {
            ILicenseContract proxy = GetProxy();

            Trace.Assert(proxy != null, "The connection channel to the Service is disconnected.");
            Console.WriteLine("Connecting to License Service...");

            string securityCode = GenerateSecurityCode();

            // TODO: encrypt the security code
            string encrypted = CryptoHelper.Encrypt(GetKey(), securityCode);

            string returnCode = proxy.GetConnection(encrypted);
            //_IsUserAlreadyLoggedIn=proxy.IsUserAlreadyLoggedIn();     //Bug-16557
            // TODO: decrypt the return code and verify it here
            string checkCode = CryptoHelper.Decrypt(GetKey(), returnCode);

            Console.WriteLine("License Service Return Code: {0}", checkCode);

            if (checkCode.Equals(VALID_CODE))
            {
                m_renewTimer = new Timer(TIMER_INTERVAL);
                m_renewTimer.AutoReset = true;
                m_renewTimer.Elapsed += new ElapsedEventHandler(RenewTimerElapsed);
                m_renewTimer.Start();
                Console.WriteLine("Internal timer has been started.");

                return true;
            }

            return false;
        }

        public int GetMaximumClientConnectionsPermitted()
        {
            ILicenseContract proxy = GetProxy();
            return proxy.GetTotalConnectionsPermitted();
        }

        public int GetNumberOfConnectedClients()
        {
            ILicenseContract proxy = GetProxy();
            return proxy.GetNumberOfConnectedClients();
        }

        //private void OnLicenseVerifyTimerElapsed(object sender, ElapsedEventArgs e)
        //{
        //    m_proxy = new Proxy(m_context, Assembly.GetAssembly(this.GetType()).Location);

        //    RenewClientSeesionWithLicenseService();
        //    //m_licenseVerifyTimer.Stop();
        //    //m_licenseVerifyTimer.Dispose();
        //    //m_licenseVerifyTimer = null;

        //    //CreateAndVerifyLicenseConnection();
        //}

        public string RenewClientSeesionWithLicenseService()
        {
            m_renewTimer.Stop();
            string result = RenewClientSeesionWithLicenseService(0);
            m_renewTimer.Start();
            Console.WriteLine("Renew session was called by client application. The internal timer has been reset.");

            return result;
        }

        private void RenewTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Internal timmer triggered.");
            RenewClientSeesionWithLicenseService(0);
        }

        public string RenewClientSeesionWithLicenseService(int attemptId)
        {
            ILicenseContract proxy = GetProxy();

            DateTime start = DateTime.Now;
            Trace.Assert(proxy != null, "The connection channel to the Service is disconnected.");

            //Console.WriteLine("Attempt{0} Extending Client License Connection with License Service...", attemptId);       //Commented By Shrikant S. on 10/04/2013    for Bug-12044

            string securityCode = GenerateSecurityCode();
            TimeSpan timespan = DateTime.Now.Subtract(start);
            //Console.WriteLine("Attempt{0} GenerateSecurity took: {1}ms from start", attemptId, timespan.Milliseconds);        //Commented By Shrikant S. on 10/04/2013    for Bug-12044
            string encrypted = CryptoHelper.Encrypt(GetKey(), securityCode);
            timespan = DateTime.Now.Subtract(start);
            //Console.WriteLine("Attempt{0} Encryption took: {1}ms from start", attemptId, timespan.Milliseconds);          //Commented By Shrikant S. on 10/04/2013    for Bug-12044

            string returnCode = string.Empty;
            returnCode = proxy.ExtendConnection(encrypted);
            timespan = DateTime.Now.Subtract(start);
            //Console.WriteLine("Attempt{0} ExtendConnection took: {1}ms from start", attemptId, timespan.Milliseconds);        //Commented By Shrikant S. on 10/04/2013    for Bug-12044

            string checkCode = CryptoHelper.Decrypt(GetKey(), returnCode);
            timespan = DateTime.Now.Subtract(start);
            //Console.WriteLine("Attempt{0} Decrypt took: {1}ms from start", attemptId, timespan.Milliseconds);             //Commented By Shrikant S. on 10/04/2013    for Bug-12044

            //Console.WriteLine("Attempt{0} License Service Renewed with Code: {1}", attemptId, checkCode);                 //Commented By Shrikant S. on 10/04/2013    for Bug-12044
            return checkCode;
        }

        [Browsable(false)]
        public RegisterMeInfo Info
        {
            get
            {
                ILicenseContract proxy = GetProxy();
                Trace.Assert(proxy != null, "The connection channel to the Service is disconnected.");

                return proxy.GetInfo();
            }
        }

        public void CloseConnection()
        {
            ILicenseContract proxy = GetProxy();

            Trace.Assert(proxy != null, "The connection channel to the Service is disconnected.");
            string securityCode = GenerateSecurityCode();
            if (m_renewTimer != null)           //Added the conditionly for Bug-16557 by Shrikant S. on 09/07/2013
            {
                m_renewTimer.Stop();
                m_renewTimer.Dispose();
            }
            m_renewTimer = null;

            CloseConnection(securityCode);

        }

        private void CloseConnection(string securityCode)
        {
            ILicenseContract proxy = GetProxy();

            string encrypted = CryptoHelper.Encrypt(GetKey(), securityCode);
            proxy.CloseConnection(encrypted);

            m_context = null;
        }

        #region ILicenseCallback Members

        public void NotifyNewClientsConnection(int clientCount)
        {
            Console.WriteLine("Total Clients Connected {0}", clientCount);
        }

        public void NofityClientForSessionRenewal()
        {
            Console.WriteLine("Got Client Renew Connection Notice from License Service...");
            RenewClientSeesionWithLicenseService(0);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    //m_proxy.CloseConnection(GenerateSecurityCode());
                    //m_proxy.Dispose();
                    //m_proxy = null;

                    m_context.Close();
                    m_context = null;
                }
            }
        }

        #endregion

        private ILicenseContract GetProxy()
        {
            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None, false);
            EndpointAddress endPointAddress = new EndpointAddress(m_serviceAddress);
            ILicenseContract proxy = ChannelFactory<ILicenseContract>.CreateChannel(tcpBinding, endPointAddress);

            return proxy;
        }

        private string GetKey()
        {
            string code = string.Empty;
            return string.Format("{0}{1}", PASSKEY, code);
        }
        // Added by Shrikant S. on 15/01/2014       for Bug-21229       //Start
        public static int Asc(string strChar)
        {
            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
        // Added by Shrikant S. on 15/01/2014       for Bug-21229       //End

        private string GenerateSecurityCode()
        {
            //throw new Exception(" 2.1 reached");
            string machineName = Process.GetCurrentProcess().MachineName;
            IPAddress ipaddress = NetworkHelper.FindIPAddress(true);
            int processId = Process.GetCurrentProcess().Id;
            string processorId = string.Empty;
            //throw new Exception(" 2.2 reached");
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mgmt.GetInstances();
            //throw new Exception(" 2.3 reached");
            foreach (ManagementObject mo in moc)
            {
                if (processorId == String.Empty)
                {
                    // only return cpuInfo from first CPU
                    //processorId = mo.Properties["ProcessorId"].Value.ToString();  //Commented by Shrikant S. on 15/01/2014 for bug-21229
                    if (mo["ProcessorId"] != null)          //Added by Shrikant S. on 15/01/2014 for bug-21229
                        processorId = mo["ProcessorId"].ToString(); //Added by Shrikant S. on 15/01/2014 for bug-21229
                }
            }

            //throw new Exception(" 2.4 reached");
            // Added by Shrikant S. on 15/01/2014       for Bug-21229   //Start
            if (processorId.Trim().Length == 0)
            {
                try
                {
                    HWProfile.HW_PROFILE_INFO hw_profile_info = HWProfile.GetCurrent();
                    string guid = hw_profile_info.szHwProfileGuid;
                    guid = guid.Replace("{", "").Replace("}", "").Substring(4, 30);
                    guid = guid.Substring(0, 1) + Asc(guid.Substring(1, 1)).ToString().PadLeft(3, '0') + guid.Substring(2, 5)
                            + Asc(guid.Substring(7, 1)).ToString().PadLeft(3, '0') + Asc(guid.Substring(8, 1)).ToString().PadLeft(3, '0')
                            + guid.Substring(9, 3) + Asc(guid.Substring(12, 1)).ToString().PadLeft(3, '0') + Asc(guid.Substring(13, 1)).ToString().PadLeft(3, '0')
                            + guid.Substring(14, 9) + Asc(guid.Substring(23, 1)).ToString().PadLeft(3, '0') + guid.Substring(24);
                    processorId = guid.Replace("-", "");
                }
                catch
                {
                    //return processorId;
                    processorId = string.Empty;
                }

            }
            // Added by Shrikant S. on 15/01/2014       for Bug-21229   //End
            // TODO: encrypt the security code here
            //return string.Format("{0}|{1}|{2}|{3}", machineName, processorId, ipaddress, processId);      //Commented by Shrikant S. on 05/07/2013 for Bug-16557 
            return string.Format("{0}|{1}|{2}|{3}|{4}", machineName, processorId, ipaddress, processId, LoggedUser);        //Added by Shrikant S. on 05/07/2013 for Bug-16557 
        }

        private string GetConfigValue(string key)
        {
            XmlDocument appDoc = new XmlDocument();

            //Get app.config file path. 

            //You need to change the ‘Application.ExecutablePath’ to your exe. path.
            string appConfigFile = Assembly.GetExecutingAssembly().CodeBase + ".config";

            // Load he config file into a xml document.
            appDoc.Load(appConfigFile);

            //Construct the xpath of the connection string.

            string connectionStringXPath = "/configuration/appSettings/add[@key=\"{0}\"]";

            connectionStringXPath = string.Format(connectionStringXPath, key);

            //Get node and modify connection string attribute.
            XmlNode node = appDoc.SelectSingleNode(connectionStringXPath);
            string value = node.Attributes["value"].Value;

            return value;
        }
    }
}
