using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using udyogerp;

namespace UdAlertExecution
{


    public class CheckIntegrity
    {
        [DllImport("kernel32")]
        public extern static int LoadLibrary(string lpLibFileName);

        [DllImport("kernel32")]
        public extern static bool FreeLibrary(int hLibModule);

        public bool IsDllRegistered()
        {
            int libId = LoadLibrary("udyogerp.dll");
            if (libId > 0) FreeLibrary(libId);
            return (libId > 0);
        }
        public bool IsAssemblyRegistered()
        {
            try
            {
                Assembly asbl = Assembly.Load("Interop.udyogerp");
                if (asbl.GetName().GetPublicKeyToken() == null)
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        internal void Register(String assemblyName)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("regsvr32.exe", string.Format("/s {0}", assemblyName));
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
        }
        internal void DeRegister(String assemblyName)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("regsvr32.exe", string.Format("/u/s {0}", assemblyName));
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
        }

    }
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length <= 0)
            {
                //args = new string[] { "5", "N011112", "UDYOG5\\Usqare", "sa", "sa1985", "13036", "ALM", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" }; /*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //CheckIntegrity ci = new CheckIntegrity();
                //if (!ci.IsAssemblyRegistered())
                //{
                //     //MessageBox.Show("Interop.Udyogerp.dll Missing");
                //      return 1;
                //}

                //if (!ci.IsDllRegistered())
                //{
                //    ci.Register("udyogerp.dll");
                //}

                
                string CurrPath;
                CurrPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                args = new string[4];
                //args[0] = "udyog3\\vudyogsdk";
                //args[1] = "sa";
                //args[2] = "sa@1985";
                //args[3] = "Vudyog";
                GetInfo.iniFile ini = new GetInfo.iniFile(CurrPath + "\\" + "Visudyog.ini");
                GetInfo.EncDec eObject = new GetInfo.EncDec();
                args[0] = ini.IniReadValue("DataServer", "Name");
                args[1] = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
                args[2] = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));
                args[3] = "vudyog";

                args[1] = eObject.Dec("myName", eObject.OnDecrypt("myName", args[1]));
                args[2] = eObject.Dec("myName", eObject.OnDecrypt("myName", args[2]));
            }
                  
            ClsMain c = new ClsMain(args);
            c.GetData();
            return 1;
        }

    }
}
