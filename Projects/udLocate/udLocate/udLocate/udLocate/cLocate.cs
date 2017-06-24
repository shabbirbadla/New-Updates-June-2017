using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udLocate
{
    static class cLocate
    {
        public static string RetVal;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //args = new string[] { "68", "B041213", "udyog3\\VUDYOGSDK", "sa", "sa@1985", "^13048", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821","ADMINEMPMST0131052012" };
                args = new string[] { "68", "B041213", "udyog3\\VUDYOGSDK", "sa", "sa@1985",@"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "ADMINEMPMST0131052012" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLocate(args));
            return 0;
        }
    }
}
