using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udChequeMaster
{
    static class cDesigMaster
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
               // args = new string[] { "99", "A041617", "AIPLDTM010\\SQLEXPRESS", "sa", "sa1985", "^13054", "ADMIN", @"F:\Installer12.0\Bmp\Icon_VudyogSDK.ico", "VUDYOGSDK", "VUDYOGSDK.EXE", "2516", "udPID2516DTM20160721170159" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmChequeMaster(args));
            return 1;
        }
    }
}
