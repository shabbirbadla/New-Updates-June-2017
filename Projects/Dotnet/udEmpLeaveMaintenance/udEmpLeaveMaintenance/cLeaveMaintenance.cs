using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpLeaveMaintenance
{
    static class cLeaveMaintenance
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {

                args = new string[] { "1", "A041415", "PRO_PANKAJ\\USQUARE", "sa", "sa1985", "^14209", "ADMIN", @"D:\vudyogsdk\Bmp\Icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };
                //args = new string[] { "3", "U011213", "udyog5\\USQUARE10", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "encash" };
                //args = new string[] { "2", "P021213", "udyog5\\usquare10New", "sa", "sa1985", "^21001", "ADMIN", @"D:\vudyogsdk\Bmp\Icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };



            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLeaveMaintenance(args));
            return 1;
        }
    }
}
