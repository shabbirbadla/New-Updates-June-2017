using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpLeaveMaster
{
    static class cLeaveMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //MessageBox.Show("Incorrect number of parameters passed.");
                //return 0;
                //args = new string[] { "13", "Bug123", "udyog65", "sa", "sa@1985", "^13059", "ADMIN", @"D:\vudyogsdk\Bmp\Icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "1", "P011213", "udyog5\\USQUARE10NEW", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLeaveMaster(args));
            return 1;
        }
    }
}
