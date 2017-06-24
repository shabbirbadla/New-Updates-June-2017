using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace FileExtractor
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
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CheckIntegrity ci = new CheckIntegrity();
            if (!ci.IsAssemblyRegistered())
            {
                MessageBox.Show("Interop.Udyogerp.dll Missing");
                return;
            }

            if (!ci.IsDllRegistered())
            {
                ci.Register("udyogerp.dll");
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
