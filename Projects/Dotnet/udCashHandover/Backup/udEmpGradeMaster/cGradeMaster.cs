using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpGradeMaster
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
                args = new string[] { "1", "A011213", "UDYOG5\\USQUARE10New", "sa", "sa1985", "^14207", "ADMIN", @"D:\U2_10\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmGradeMaster(args));
            return 1;
        }
    }
}
