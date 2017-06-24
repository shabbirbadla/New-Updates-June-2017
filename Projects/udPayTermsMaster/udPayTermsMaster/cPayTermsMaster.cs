using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udPayTermsMaster
{
    static class cPayTerms
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                args = new string[] { "3", "V011516", "AIPLDTM010\\SQLEXPRESS", "sa", "sa1985", "^13074", "ADMIN", @"F:\Installer12.0\Bmp\icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "6476", "udPID6476DTM20151226112732" };
               
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPayTermsMaster(args));
            return 1;
        }
    }
}
