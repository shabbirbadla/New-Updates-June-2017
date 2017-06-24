using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Configuration.Install;
using System.Reflection;

namespace Udyog.Application.License
{
    static class Program
    {
        private const string STRING_RESOURCENAME = "LicenseServiceStrings";
        private static NotifyIcon appIcon;
        private static frmServiceManager m_monitorForm = null;

        static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {                
                ServiceController existingService = null;
                string serviceName = ResourceHelper.GetValue(STRING_RESOURCENAME, "S_SERVICENAME");
                string title = ResourceHelper.GetValue(STRING_RESOURCENAME, "S_TITLE");

                existingService = GetExistingService(serviceName);

                if (args[0].Equals(ResourceHelper.GetValue(STRING_RESOURCENAME, "S_INSTALL_PARAMETER")) && existingService == null)
                {
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show(ResourceHelper.GetFormatedValue(STRING_RESOURCENAME, "S_SERVICE_INSTALLMESSAGE", serviceName), 
                        title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            SelfInstaller.InstallMe();

                            existingService = GetExistingService(serviceName);
                            existingService.Start();
                            existingService.WaitForStatus(ServiceControllerStatus.Running);

                            MessageBox.Show(ResourceHelper.GetFormatedValue(STRING_RESOURCENAME, "S_SERVICE_INSTALLSUCCESS", serviceName)
                                , title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if (args[0].Equals(ResourceHelper.GetValue(STRING_RESOURCENAME, "S_UNINSTALL_PARAMETER")) && existingService != null)
                {
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show(ResourceHelper.GetFormatedValue(STRING_RESOURCENAME, "S_SERVICE_UNINSTALLMESSAGE", serviceName)
                        , title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            if (existingService.Status != ServiceControllerStatus.Stopped)
                            {
                                existingService.Stop();
                                existingService.WaitForStatus(ServiceControllerStatus.Stopped);
                            }

                            SelfInstaller.UninstallMe();
                            MessageBox.Show(ResourceHelper.GetFormatedValue(STRING_RESOURCENAME, "S_SERVICE_UNINSTALLSUCCESS", serviceName)
                                , title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }
                }
                else if (args[0].Equals(ResourceHelper.GetValue(STRING_RESOURCENAME, "S_SHOWUI")) && existingService != null)
                {
                    frmServiceManager frm = new frmServiceManager();
                    System.Windows.Forms.Application.Run(frm);
                }

                return;
            }

            // Started from the SCM
            System.ServiceProcess.ServiceBase[] servicestorun;
            servicestorun = new System.ServiceProcess.ServiceBase[] { new LicenseServiceHost() };
            ServiceBase.Run(servicestorun);
        }

        public static ServiceController GetExistingService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(serviceName))
                {
                    return service;
                }
            }

            return null;
        }
    }

    public static class SelfInstaller
    {
        private static readonly string m_exePath = Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { m_exePath });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", m_exePath });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
