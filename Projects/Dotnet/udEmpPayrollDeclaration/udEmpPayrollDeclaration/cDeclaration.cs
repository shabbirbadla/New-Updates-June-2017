using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpInvestmentDeclaration
{
    static class cDeclaration
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //args = new string[] { "13", "T011213", "udyog65", "sa", "sa@1985", "^14219", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                // args = new string[] { "3", "U011213", "udyog5\\USQUARE10", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "2", "B011213", "udyog5-pc\\VUDYOGSDK", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_vudyogsdk.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDeclaration(args));
            return 1;
        }
    }
}
