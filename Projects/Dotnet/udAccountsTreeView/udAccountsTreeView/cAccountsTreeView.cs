using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udAccountsTreeView
{
    static class cAccountsTreeView
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                args = new string[] { "2", "UdNov12", "RPRAJAPATI-LPTP", "sa", "sa@1985", "^14217", "ADMIN", @"D:\VudyogSdk\Bmp\Icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "2", "U011213"UdNov12, "udyog5\\USQUARE10New", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmAccountsTreeView(args));
            return 1;
        }
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new FrmAccountsTreeView());
        //}
    }
}
