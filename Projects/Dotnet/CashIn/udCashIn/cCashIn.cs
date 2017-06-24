using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udCashIn
{
    static class cGradeMaster
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
                //args = new string[] { "1", "A011213", "UDYOG5\\USQUARE10New", "sa", "sa1985", "^14207", "ADMIN", @"D:\U2_10\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" }; //commented by Ruchit
                //args = new string[] { "1", "P011516", "ARCHANA\\USQUARE", "sa", "sa1985", "^13055", "ADMIN", @"D:\Installer12.0\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "7064", "udPID7064DTM20160302162850" }; //Added b Ruchit
                //args = new string[] { "2", "B011617", "AIPLLTM001\\AIPLLTM001", "sa", "sa1985", "^21245", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "7064", "udPID7064DTM20160302162850" }; //Added b Ruchit
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmuDCashIn(args));
            return 1;
        }
    }
}
