using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udLocationMaster
{
    static class cLocationMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //args = new string[] { "5", "N011112", "udyog5\\usqare", "sa", "sa1985", "^13039", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "1", "P011213", "udyog5\\usquare10", "sa", "sa1985", "^21001", "ADMIN", @"D:\U2_10\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
               // args = new string[] { "2", "N011213", "udyog5\\USQUARE10New", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };
                args = new string[] { "2", "UDYOG5-PC\\VUDYOGSDK", "P011213", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "USquare", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821", "opening" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLocationMaster(args));
          
            return 1;
        }
    }
}
