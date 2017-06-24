using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;

namespace Udyog.Application.License
{
    public partial class frmServiceManager : Form
    {
        private const string S_RESOURCEFILENAME = "LicenseServiceStrings";
        LicenseServiceHost m_serviceHost = null;
        ServiceController m_serviceController = null;

        public frmServiceManager()
        {
            InitializeComponent();

            string serviceName = ResourceHelper.GetValue(S_RESOURCEFILENAME, "S_SERVICENAME");
            m_serviceController = Program.GetExistingService(serviceName);

            this.FormClosing += new FormClosingEventHandler(OnFormClosing);

            m_serviceHost = new LicenseServiceHost();
            m_btnStartService.Enabled = !(m_serviceController.Status == ServiceControllerStatus.Running);
            m_btnStopService.Enabled = !m_btnStartService.Enabled;
        }

        void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            OnStopService_Click(sender, new EventArgs());
            //Logger.Close();
        }

        private void OnStartServiceClick(object sender, EventArgs e)
        {
            try
            {
                bool result = m_serviceHost.Start();
                m_btnStartService.Enabled = !result;
                m_btnStopService.Enabled = result;
            }
            catch (Exception ex) { }
        }

        private void OnStopService_Click(object sender, EventArgs e)
        {
            m_serviceHost.Stop();
            m_btnStartService.Enabled = true;
            m_btnStopService.Enabled = false;
        }

    }
}
