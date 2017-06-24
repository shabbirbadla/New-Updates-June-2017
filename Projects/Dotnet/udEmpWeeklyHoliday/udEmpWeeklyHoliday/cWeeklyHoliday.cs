using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udEmpWeeklyHoliday
{
    static class cWeeklyHoliday
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new frmWeeklyHoliday());
//        }
//    }
//}
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
               //args = new string[] { "16", "P071213", "udyog5\\sqlexpress", "sa", "sa1985", "^21001", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
               args = new string[] { "1", "P011213", "udyog65", "sa", "sa@1985", "^14207", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmWeeklyHoliday(args));

            //Application.Run(new frmWeekoff(args));
            return 1;
        }
    }
}
