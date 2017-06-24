using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udAttendanceIntegration
{
    static class cEmpAttendanceIntegration
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //args = new string[] { "20", "Q011213", @"Desktop29\SQLEXPRESS", "sa", "sa1985", "^13004", "ADMIN", @"D:\VUdyog_Installations\VUdyogSDK\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "12", "E051112", "udyog5\\sqlexpress", "sa", "sa1985", "^13060", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "70", "U011213", @"UDYOG3\VUDYOGSDK", "sa", "sa@1985", "^13004", "ADMIN", @"D:\VUdyogSDK\Bmp\Icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "3", "V011516", "PRO_PANKAJ\\SQLEXPRESS", "sa", "sa1985", "^14217", "ADMIN", @"F:\Installer12.0\Bmp\icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "4228", "udPID4228DTM20150715102639", "opening" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmAttendanceIntegration(args));
            return 1;
        }
    }
}
