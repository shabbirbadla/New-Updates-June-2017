using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Reflection;

namespace Udyog.Application.License
{
    public class Proxy : IDisposable
    {
        #region fields

        private EndpointAddress m_endPointAddress = null;
        private NetTcpBinding m_tcpBinding = new NetTcpBinding(SecurityMode.None, false);
        private volatile ILicenseContract m_proxy = null;

        InstanceContext m_context;

        private static object syncRoot = new object();

        // Track whether Dispose has been called.
        private bool disposed = false;

        #endregion

        #region methods

        public Proxy(InstanceContext context)
        {
            string configPath = Assembly.GetEntryAssembly().Location;
            string endpointAddress = ConfigSettings.GetEndpointAddress(configPath);

            m_context = context;
            m_endPointAddress = new EndpointAddress( endpointAddress );
        }

        public Proxy(InstanceContext context, string configPath)
        {
            string endpointAddress = ConfigSettings.GetEndpointAddress(configPath);

            m_context = context;
            m_endPointAddress = new EndpointAddress(endpointAddress);
        }

        //public ILicenseContract proxy
        //{
        //    get
        //    {
        //        if (m_proxy == null)
        //        {
        //            lock (syncRoot)
        //            {
        //                if (m_proxy == null)
        //                    //m_proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
        //                    m_proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
        //            }
        //        }

        //        return m_proxy;
        //    }
        //}

        public string ValidateLicense(string securityCode)
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            return proxy.ValidateLicense(securityCode);
        }

        public string GetConnection(string securityCode)
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            return proxy.GetConnection(securityCode);
        }

        public void CloseConnection(string securityCode)
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            proxy.CloseConnection(securityCode);
        }

        public string ExtendConnection(string securityCode)
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            return proxy.ExtendConnection(securityCode);
        }

        public int GetTotalConnectionsPermitted()
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            return proxy.GetTotalConnectionsPermitted();
        }

        public int GetNumberOfConnectedClients()
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            return proxy.GetNumberOfConnectedClients();
        }

        public void NotifyClientCountChange(int clientCount)
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            proxy.NotifyClientCountChange(clientCount);
        }

        public string CheckConnection()
        {
            //ILicenseContract proxy = new DuplexChannelFactory<ILicenseContract>(m_context, m_tcpBinding).CreateChannel(m_endPointAddress);
            ILicenseContract proxy = new ChannelFactory<ILicenseContract>(m_tcpBinding).CreateChannel(m_endPointAddress);
            return proxy.CheckConnection();
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
                    // Dispose managed resources.
                    m_proxy = null;
                    m_endPointAddress = null;
                    m_tcpBinding = null;

                    m_context = null;
                    Console.WriteLine("Proxy disposed.");
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion
    }
}
