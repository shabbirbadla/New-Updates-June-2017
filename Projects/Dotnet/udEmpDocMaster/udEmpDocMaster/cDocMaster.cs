using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpDocMaster
{
    static class cDocMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                args = new string[] { "5", "N011112", "udyog5\\usqare", "sa", "sa1985", "^13048", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDocMaster(args));
            return 1;
        }
    }
}
