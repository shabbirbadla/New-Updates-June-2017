using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udCashHandover
{
    static class cCashHandover
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //args = new string[] { "5", "N011112", "udyog5\\usqare", "sa", "sa1985", "^13048", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
               //args = new string[] { "2", "X011516", "AIPLDTM010\\SQLEXPRESS", "sa", "sa1985", "^14207", "ADMIN", @"F:\Installer12.0\Bmp\Icon_VudyogSDK.ico", "VUDYOGSDK", "VUDYOGSDK.EXE", "3624", "udPID3624DTM20160302111523" };
                //args = new string[] { "18", "B061516", "AIPLLTM001", "sa", "sa1985", "^21379", "ADMIN", @"D:\VudyogSdk\Bmp\Icon_VudyogSDK.ico", "VUDYOGSDK", "VUDYOGSDK.EXE", "3624", "udPID3624DTM20160302111523" };
                args = new string[] { "2", "B011617", "AIPLLTM001\\AIPLLTM001", "sa", "sa1985", "^21288", "ADMIN", @"D:\VudyogSdk\Bmp\Icon_VudyogSDK.ico", "VUDYOGSDK", "VUDYOGSDK.EXE", "3624", "udPID3624DTM20160302111523" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new udCashHandov(args));
            return 1;
        }
    }
}
