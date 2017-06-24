using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpLeaveMaster
{
    static class cLeaveMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //MessageBox.Show("Incorrect number of parameters passed.");
                //return 0;
                //args = new string[] { "19", "A021112", "udyog65", "sa", "sa@1985", "13032", "ALM", "ADMIN", @"D:\USquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                args = new string[] { "5", "N011112", "udyog5\\usqare", "sa", "sa1985", "^13044", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLeaveMaster(args));
            return 1;
        }
    }
}
