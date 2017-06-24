using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpShiftMaster
{
    static class cShiftMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
               //args = new string[] { "1", "H011112", "udyog65", "sa", "sa@1985", "^13044", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" }; 
               //args = new string[] { "5", "N011112", "udyog5\\usqare", "sa", "sa1985", "^13039", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "5", "P071213", "udyog5\\sqlexpress", "sa", "sa1985", "^13060", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmShiftMaster(args));
            return 1;
        }
    }
}
