using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpPayrollDeclarationMaster
{
    static class cEmpInvHeadDet
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
               // args = new string[] { "13", "T011213", "udyog65", "sa", "sa@1985", "^13070", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" }; 
                //args = new string[] { "5", "N011112", "udyog5\\usqare", "sa", "sa1985", "^13039", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
               args = new string[] { "2", "P021213", "udyog5\\USQUARE10New", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmEmpInvHeadDet(args));
            return 1;
        }
    }
}
