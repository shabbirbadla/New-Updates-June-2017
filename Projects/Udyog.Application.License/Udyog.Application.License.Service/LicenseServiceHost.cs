using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Udyog.Application.License
{
    public class LicenseServiceHost : ServiceBase
    {
        private const string STRING_RESOURCENAME = "LicenseServiceStrings";
        private ServiceHost m_host = null;

        public LicenseServiceHost()
        {
            ServiceName = ResourceHelper.GetValue(STRING_RESOURCENAME, "S_SERVICENAME");
        }

        public bool Start()
        {
            OnStart(null);

            return (m_host.State == CommunicationState.Opened);
        }

        protected override void OnStart(string[] args)
        {
            string endPointAddress = new System.Configuration.AppSettingsReader().GetValue("licenseServiceAddress", typeof(string)).ToString();
            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None, false);
            tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            tcpBinding.ReceiveTimeout = new TimeSpan(0, 2, 0);

            m_host = new ServiceHost(typeof(LicenseService));
            m_host.AddServiceEndpoint(typeof(ILicenseContract), tcpBinding, endPointAddress);

            int ProcessorCnt = (Environment.ProcessorCount <= 0 ? 1 : Environment.ProcessorCount); 
            try
            {
                //ServiceThrottlingBehavior stb = new System.ServiceModel.Description.ServiceThrottlingBehavior();
                //stb.MaxConcurrentCalls = int.MaxValue;
                //stb.MaxConcurrentInstances = int.MaxValue;
                //stb.MaxConcurrentSessions = int.MaxValue;
                //m_host.Description.Behaviors.Add(stb);
                //Added by Shrikant S. on 04/03/2015 for Bug-25470      // Start
                m_host.Description.Behaviors.Add((IServiceBehavior)new ServiceThrottlingBehavior()
                {
                    MaxConcurrentCalls = (16 * ProcessorCnt),
                    MaxConcurrentSessions = (10 * ProcessorCnt)
                    //MaxConcurrentInstances = 50
                });
                //Added by Shrikant S. on 04/03/2015 for Bug-25470      // End
                m_host.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnStop()
        {
            if (m_host != null && m_host.State == CommunicationState.Opened)
            {
                m_host.Close();
                m_host = null;
            }
        }
    }
}
