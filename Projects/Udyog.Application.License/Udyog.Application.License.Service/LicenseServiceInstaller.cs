using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;

namespace Udyog.Application.License
{
    [RunInstaller(true)]
    public class LicenseServiceInstaller : Installer
    {
        private const string STRING_RESOURCENAME = "LicenseServiceStrings";

        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public LicenseServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.StartType = ServiceStartMode.Automatic;
            service.ServiceName = ResourceHelper.GetValue(STRING_RESOURCENAME, "S_SERVICENAME");
            service.Description = ResourceHelper.GetValue(STRING_RESOURCENAME, "S_SERVICEDESCRIPTION");

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
