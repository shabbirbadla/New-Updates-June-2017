using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpPayHeadSlabMaster
{
    static class cSlabMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                args = new string[] { "1", "A041415", "PRO_PANKAJ\\USQUARE", "sa", "sa1985", "^13058", "ADMIN", @"D:\USQUARE\Bmp\Icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSlabMaster(args));
            return 1;
        }
    }
}
