using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udUserSecurity
{
    static class cUserSecurity
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmUserSecurity());

            if (args.Length < 8)
            {
                //args = new string[] { "7", "N011213", "udyog5\\vu10sdk", "sa", "sa1985", "^14207", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                // args = new string[] { "11", "UdSep12", "udyog65", "sa", "sa@1985", "^13057", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "1", "P011213", "UDYOG5-PC\\Vudyogsdk", "sa", "sa1985", "^14207", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmUserSecurity(args));
            return 1;
        }
    }
}
