using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpMusterGeneration
{
    static class cMusterGeneration
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                //args = new string[] { "11", "U33", "udyog65", "sa", "sa@1985", "^14215", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "7", "N011213", "udyog5\\VU10SDK", "sa", "sa1985", "^14215", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821"};
                args = new string[] { "1", "P031213", "udyog5\\USQUARE10", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };

            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmUdMuster(args));
            return 1;
        }
    }
}
