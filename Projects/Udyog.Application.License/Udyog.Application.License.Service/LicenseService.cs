using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Collections;
using System.Net;
using System.Diagnostics;
using System.Management;

namespace Udyog.Application.License
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class LicenseService : ILicenseContract
    {
        private const string VALID_CODE = "CUSSECC";
        private const string INVALID_CODE = "FAILED.";
        private const string PASSKEY = "UDYOG";
        private Hashtable m_clientList = new Hashtable();
        private int MAXCONNECTIONSPERMITTED = 3;
        private RegisterMeInfo m_info;
        //Added by Shrikant S. on 09/07/2013 for  Bug-16557     //Start    
        public bool IsUserAlreadyLoggedIn(string userName,string securityCode)             
        {
            bool UserLoggedIn = false;
            foreach (string myKey in m_clientList.Keys)
            {
                if (myKey.ToUpper().ToString().EndsWith("|"+userName.ToUpper().Trim()))
                {
                    //string decryptedCode = CryptoHelper.Decrypt(GetKey(), securityCode);
                    //string ipAddress = string.Empty;
                    //string processId = string.Empty;
                    //string loggedUser = string.Empty; 
                    UserLoggedIn = true;
                    //GetClientCredentials(CryptoHelper.Decrypt(GetKey(), securityCode), out ipAddress, out processId, out loggedUser);
                    //Logger.LogMessage("New Session request from IP:{0} Process:{1} Denied. Since user {2} already logged.", ipAddress, processId, loggedUser);
                }
            }
            return UserLoggedIn;
        }
        //Added by Shrikant S. on 09/07/2013 for  Bug-16557     //End

        public string ValidateLicense(string securityCode)
        {
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;       //Added by Shrikant S. on 08/07/2013 for Bug-16557
            //GetClientCredentials(CryptoHelper.Decrypt(GetKey(), securityCode), out ipAddress, out processId);     //Commented by Shrikant S. on 08/07/2013 for Bug-16557
            GetClientCredentials(CryptoHelper.Decrypt(GetKey(), securityCode), out ipAddress, out processId,out loggedUser);//Added by Shrikant S. on 08/07/2013 for Bug-16557
            Logger.LogMessage("Received request for validating license from IP:{0} Process:{1} ", ipAddress, processId);

            
            if (ValidateLicense())
            {
                Logger.LogMessage("IP:{0} Process:{1} validation of license Succeeded.", ipAddress, processId);
                return CryptoHelper.Encrypt(GetKey(), VALID_CODE);
            }
            else
            {
                Logger.LogMessage("IP:{0} Process{1} Validation Failed.", ipAddress, processId);
                return CryptoHelper.Encrypt(GetKey(), INVALID_CODE);
            }
        }

        public string GetConnection(string securityCode)
        {
            string decryptedCode = CryptoHelper.Decrypt(GetKey(), securityCode);
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;         //Added by Shrikant S. on 09/07/2013 for Bug-16557
            //GetClientCredentials(decryptedCode, out ipAddress, out processId);        //Commented by Shrikant S. on 09/07/2013 for Bug-16557   
            GetClientCredentials(decryptedCode, out ipAddress, out processId, out loggedUser);   //Added by Shrikant S. on 09/07/2013 for Bug-16557   
            Logger.LogMessage("Received request for new Session from IP:{0} Process:{1}", ipAddress, processId);

            try { CreateClientSession(decryptedCode); }
            
            catch (ClientSessionCountExceededException)
            {
                Logger.LogMessage("New Session request from IP:{0} Process:{1} Denied. Maximum client connections exceeded.", ipAddress, processId);
                return CryptoHelper.Encrypt(GetKey(), "Client Connections Exceeded. Failed.");
            }
            catch (ClinetSessionDeniedException)
            {
                Logger.LogMessage("New Session request from IP:{0} Process:{1} Denied. Client Connections Failed.", ipAddress, processId);
                return CryptoHelper.Encrypt(GetKey(), "Client Connection Deined. Failed.");
            }
            catch (Exception ex)
            {
                Logger.LogMessage("New Session request from IP:{0} Process:{1} Denied. Exception Occured: {2}.", ipAddress, processId, ex.Message);
                return CryptoHelper.Encrypt(GetKey(), "License Service Failed to provide Session. Failed.");
            }

            Logger.LogMessage("New Session request from IP:{0} Process:{1} Succeeded. New Session started.", ipAddress, processId);
            return CryptoHelper.Encrypt(GetKey(), VALID_CODE);
        }

        public void CloseConnection(string securityCode)
        {
            string decryptedCode = CryptoHelper.Decrypt(GetKey(), securityCode);
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;   //Added by Shrikant S. on 08/07/2013 for Bug-16557
            //GetClientCredentials(decryptedCode, out ipAddress, out processId);       //Commented by Shrikant S. on 08/07/2013 for Bug-16557   

            GetClientCredentials(decryptedCode, out ipAddress, out processId,out loggedUser);      //Added by Shrikant S. on 08/07/2013 for Bug-16557
            Logger.LogMessage("Received request to close session from IP:{0} Process:{1}", ipAddress, processId);

            ClientSession clientSession = m_clientList[decryptedCode] as ClientSession;
            if (clientSession != null)
            {
                OnClientSessionExpired(clientSession, new EventArgs());
                Logger.LogMessage("Close session from IP:{0} Process:{1} succeeded. Session Terminated.", ipAddress, processId);
            }
            else
            {
                Logger.LogMessage("Close session from IP:{0} Process:{1} failed. No active session found.", ipAddress, processId);
            }
        }

        public string ExtendConnection(string securityCode)
        {
            string decryptedCode = CryptoHelper.Decrypt(GetKey(), securityCode);
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;       //Added by Shrikant S. on 08/07/2013 for Bug-16557

            //GetClientCredentials(decryptedCode, out ipAddress, out processId);        //Commented by Shrikant S. on 08/07/2013 for Bug-16557
            GetClientCredentials(decryptedCode, out ipAddress, out processId,out loggedUser);          //Added by Shrikant S. on 08/07/2013 for Bug-16557
            Logger.LogMessage("Received request to extend client session from IP:{0} Process:{1}", ipAddress, processId);

            if (ExtendClientSession(decryptedCode))
            {
                Logger.LogMessage("Client session for from IP:{0} Process:{1} extended.", ipAddress, processId);
                return CryptoHelper.Encrypt(GetKey(), VALID_CODE);
            }
            else
            {
                Logger.LogMessage("Client session for from IP:{0} Process:{1} failed. The session was not extended.", ipAddress, processId);
                return CryptoHelper.Encrypt(GetKey(), INVALID_CODE);
            }
        }

        public int GetTotalConnectionsPermitted()
        {
            return MAXCONNECTIONSPERMITTED;
        }

        public int GetNumberOfConnectedClients()
        {
            return m_clientList.Count;
        }

        public void NotifyClientCountChange(int clientCount)
        {
            NotifyClients();
        }

        public string CheckConnection()
        {
            int hour = DateTime.Now.Hour;
            int min = DateTime.Now.Minute;

            return string.Format("{0}::{1}", hour, min);
        }

        public RegisterMeInfo GetInfo()
        {
            if (m_info.Equals(null))
                throw new Exception("ValidateLicense Operation not initiallized.");

            return m_info;
        }

        private void CreateClientSession(string securityCode)
        {
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;           //Added for Bug-16557
            //GetClientCredentials(securityCode, out ipAddress, out processId);
            GetClientCredentials(securityCode, out ipAddress, out processId,out loggedUser);
            ClientSession clientSession = m_clientList[securityCode] as ClientSession;

            // If the createClientSession as mistakenly called more than once; we ignore the call
            if (clientSession != null)
            {
                Logger.LogMessage("Client session from IP:{0} Process:{1} already exists. No new client session created.", ipAddress, processId);
                return;
            }

            if (m_clientList.Count == MAXCONNECTIONSPERMITTED)
            {
                Logger.LogMessage("Maximum client sessions reached. Denying client session for IP:{0} Process:{1}.", ipAddress, processId);
                throw new ClientSessionCountExceededException();
            }
           
            Logger.LogMessage("Creating new client session for IP:{0} Process:{1}.", ipAddress, processId);
            clientSession = new ClientSession(securityCode);
            //clientSession.ClientCallbackDelegate = OperationContext.Current.GetCallbackChannel<ILicenseCallback>();
            clientSession.ClientSessionExpired += new ClientSession.ClientSessionExpiredDelegate(OnClientSessionExpired);

            m_clientList.Add(securityCode, clientSession);

            Logger.LogMessage("New client session for IP:{0} Process:{1} has been created. with {2}ms timeout.", ipAddress, processId, clientSession.AutoTimeout);
            NotifyClients();
        }

        private bool ExtendClientSession(string securityCode)
        {
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;       //Added by Shrikant S. on 08/07/2013 for Bug-16557
            //GetClientCredentials(securityCode, out ipAddress, out processId);     //Commented by Shrikant S. on 08/07/2013 for Bug-16557
            GetClientCredentials(securityCode, out ipAddress, out processId,out loggedUser);       //Added by Shrikant S. on 08/07/2013 for Bug-16557
            ClientSession clientSession = m_clientList[securityCode] as ClientSession;

            if (clientSession == null)
            {
                Logger.LogMessage("No active session exists for IP:{0} Process:{1}.", ipAddress, processId);
                return false;
            }

            clientSession.ExtendSession();
            return true;
        }

        private void OnClientSessionExpired(ClientSession sender, EventArgs e)
        {
            string ipAddress = string.Empty;
            string processId = string.Empty;
            string loggedUser = string.Empty;    //Added by Shrikant S. on 08/07/2013 for Bug-16557
            //GetClientCredentials(sender.Key, out ipAddress, out processId);       //Commented by Shrikant S. on 08/07/2013 for Bug-16557
            GetClientCredentials(sender.Key, out ipAddress, out processId,out loggedUser);         //Added by Shrikant S. on 08/07/2013 for Bug-16557

            Logger.LogMessage("Client session expired for IP:{0} Process:{1}.", ipAddress, processId);
            if (m_clientList.ContainsKey(sender.Key))       //Added conditions only by Shrikant S. on 09/07/2013 for Bug-16557
                m_clientList.Remove(sender.Key);

            sender.Dispose();
            sender = null;

            NotifyClients();
        }

        private void NotifyClients()
        {
            //Logger.LogMessage("Total number of connected clients {0}", m_clientList.Count);
            foreach (ClientSession client in m_clientList.Values)
            {
                try { client.NotifyNewClientsConnections(m_clientList.Count); }
                catch { }
            }
        }

        private string GetKey()
        {
            string code = string.Empty;
            //code = CheckConnection();
            return string.Format("{0}{1}", PASSKEY, code);
        }

        private bool ValidateLicense()
        {
            bool result = false;

            UdyogRegister reg = new UdyogRegister();
            m_info = reg.RegistrationInfo;
            string[] dateVal = m_info.ExpiryDate.Trim().Split(new char[] { '-' });

            DateTime expiryDate = new DateTime(Int32.Parse(dateVal[2]), Int32.Parse(dateVal[1]), Int32.Parse(dateVal[0]));

            if (DateTime.Now.CompareTo(expiryDate) < 0)
                result = true;
            else
                result = false;

            string registeredProcessorId = m_info.MACId.Trim().Replace(UdyogRegister.Chr(1), "!");
            registeredProcessorId = registeredProcessorId.Replace("!", "");
            registeredProcessorId = Reverse(registeredProcessorId.Trim());
            string processorId = GetProcessorId();

            //Logger.LogMessage("Processor Compare: Machine {0}, File {1} ", processorId, registeredProcessorId);

            if (processorId.Equals(registeredProcessorId))
                result = true;
            else
                result = false;

            MAXCONNECTIONSPERMITTED = Int32.Parse(m_info.MaximumNumberOfUsersAllowed);
            Logger.LogMessage("Maximum Concurrent Connections Permitted : {0}", MAXCONNECTIONSPERMITTED);
            return result;
        }

        private string Reverse(string str)
        {
            int len = str.Length;
            char[] arr = new char[len];

            for (int i = 0; i < len; i++)
            {
                arr[i] = str[len - 1 - i];
            }

            return new string(arr);
        }
        // Added by Shrikant S. on 15/01/2014       for Bug-21229       //Start
        public static int Asc(string strChar)
        {
            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
        // Added by Shrikant S. on 15/01/2014       for Bug-21229       //End

        private string GetProcessorId()
        {
            string processorId = string.Empty;
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mgmt.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (processorId == String.Empty)
                {
                    //processorId = mo.Properties["ProcessorId"].Value.ToString();      //Commented by Shrikant S. on 15/01/2014 for bug-21229
                    if (mo["ProcessorId"] != null)          //Added by Shrikant S. on 15/01/2014 for bug-21229
                        processorId = mo["ProcessorId"].ToString(); //Added by Shrikant S. on 15/01/2014 for bug-21229
                }
            }
            
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
                    return processorId;
                }

            }
            // Added by Shrikant S. on 15/01/2014       for Bug-21229   //End
            return processorId;
        }

        private void GetClientCredentials(string code, out string ipAddress, out string processId)
        {
            string[] parts = code.Split(new char[] { '|' });
            ipAddress = parts[2];
            processId = parts[3];
        }
        // Added by Shrikant S. on 09/07/2013 for Bug-16557        //Start
        private void GetClientCredentials(string code, out string ipAddress, out string processId,out string loggedUser)
        {
            string[] parts = code.Split(new char[] { '|' });
            ipAddress = parts[2];
            processId = parts[3];
            loggedUser = parts[4];
        }
        // Added by Shrikant S. on 09/07/2013 for Bug-16557        //End
    }
    internal class ClientSessionCountExceededException : Exception { }
    internal class ClinetSessionDeniedException : Exception { }
}
