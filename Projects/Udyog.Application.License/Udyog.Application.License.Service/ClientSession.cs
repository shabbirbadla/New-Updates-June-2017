using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Timers;
using System.ServiceModel;

namespace Udyog.Application.License
{
    public class ClientSession : IDisposable
    {
        private const int SESSION_TIMEOUT = 180000;

        private Timer m_sessionExpiryTimer = null;
        private Timer m_sessionExpiringTimer = null;

        public delegate void ClientSessionExpiredDelegate(ClientSession sender, EventArgs e);
        public event ClientSessionExpiredDelegate ClientSessionExpired;

        public ClientSession(string securityCode)
        {
            this.Key = securityCode;
            string machineName = string.Empty;
            string processorId = string.Empty;
            IPAddress ipAddress = null;
            int processId = -1;

            string loggedUser = string.Empty;       //Added by Shrikant S. on 05/07/2013 for Bug-16557
            //DecryptClientInfo(securityCode, out machineName, out processorId, out ipAddress, out processId);      //Commented by Shrikant S. on 05/07/2013 for Bug-16557
            DecryptClientInfo(securityCode, out machineName, out processorId, out ipAddress, out processId, out loggedUser);        //Added by Shrikant S. on 05/07/2013 for Bug-16557
            this.MachineName = machineName;
            this.ProcessorId = processorId;
            this.IPAddress = ipAddress;
            this.ProcessId = processId;
            this.LoggedUser = loggedUser;           //Added by Shrikant S. on 05/07/2013 for Bug-16557
            InitializeTimers();
        }

        public string Key { get; set; }
        public string MachineName { get; set; }
        public string ProcessorId { get; set; }
        public IPAddress IPAddress { get; set; }
        public int ProcessId { get; set; }

        public string LoggedUser { get; set; }      //Added by Shrikant S. on 05/07/2013 for Bug-16557
        //public ILicenseCallback ClientCallbackDelegate { get; set; }

        // Track whether Dispose has been called.
        private bool m_disposed = false;

        private void InitializeTimers()
        {
            if (m_sessionExpiryTimer != null)
            {
                m_sessionExpiryTimer.Stop();
                m_sessionExpiryTimer = null;
            }

            //if (m_sessionExpiringTimer != null)
            //{
            //    m_sessionExpiringTimer.Stop();
            //    m_sessionExpiringTimer = null;
            //}

            //m_sessionExpiringTimer = new Timer(SESSION_TIMEOUT - 30000);
            m_sessionExpiryTimer = new Timer(SESSION_TIMEOUT);

            //m_sessionExpiringTimer.AutoReset = false;
            m_sessionExpiryTimer.AutoReset = false;

            //m_sessionExpiringTimer.Elapsed += new ElapsedEventHandler(OnSessionExpiringTimerElapsed);
            m_sessionExpiryTimer.Elapsed += new ElapsedEventHandler(OnSessionExpiryTimerElapsed);

            //m_sessionExpiringTimer.Start();
            m_sessionExpiryTimer.Start();
        }

        private void DecryptClientInfo(string securityCode, out string machineName, out string processorId, out IPAddress ipAddress, out int processId)
        {
            string[] values = securityCode.Split(new char[] { '|' });
            machineName = values[0];
            processorId = values[1];
            ipAddress = IPAddress.Parse(values[2]);
            processId = int.Parse(values[3]);
        }
        //Added the below method by Shrikant S. on 05/07/2013 for Bug-16557     //Start
        private void DecryptClientInfo(string securityCode, out string machineName, out string processorId, out IPAddress ipAddress, out int processId,out string loggedUser)
        {
            string[] values = securityCode.Split(new char[] { '|' });
            machineName = values[0];
            processorId = values[1];
            ipAddress = IPAddress.Parse(values[2]);
            processId = int.Parse(values[3]);
            loggedUser = values[4];
        }
        //Added the below method by Shrikant S. on 05/07/2013 for Bug-16557     //End

        private void OnSessionExpiringTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // if the connection to the client fails; ignore the message and let the client session expire
            try 
            {
                Logger.LogMessage("Calling client for extending its session.");
                //ClientCallbackDelegate.NofityClientForSessionRenewal();
                Logger.LogMessage("NotifyClientForSessionRenewal returned.");
            }
            catch { }
        }

        private void OnSessionExpiryTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (ClientSessionExpired != null)
                ClientSessionExpired(this, new EventArgs());
        }

        public void ExtendSession()
        {
            InitializeTimers();
        }

        public void NotifyNewClientsConnections(int clientCount)
        {
            //this.ClientCallbackDelegate.NotifyNewClientsConnection(clientCount);
        }

        public int AutoTimeout
        {
            get { return SESSION_TIMEOUT; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!m_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    if (m_sessionExpiryTimer != null)
                    {
                        m_sessionExpiryTimer.Stop();
                        m_sessionExpiryTimer = null;
                    }

                    if (m_sessionExpiringTimer != null)
                    {
                        m_sessionExpiringTimer.Stop();
                        m_sessionExpiringTimer = null;
                    }

                    //ClientCallbackDelegate = null;

                    // Note disposing has been done.
                    m_disposed = true;
                }
            }
        }

        #endregion
    }
}
