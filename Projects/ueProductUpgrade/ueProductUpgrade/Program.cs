using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ueProductUpgrade
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new frmMain());
        //}
        //  added by sandeep for bug-18141 on 14-sep-13 -->start
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                args = new string[] { "258", "M061415", "prod_shrikant\\shree", "sa", "sa1985", "^18010", "ADMIN", @"e:\u3\vudyogsdk\Bmp\Icon_Vudyogsdk.ico", "Vudyog SDK", "VudyogSDK.exe", "1", "udpid6096DTM20110307112715" };/*added by sandeep for bug-18141 on 14-sep-13 Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //          Application.Run(new frmMain()); // Commented added by sandeep for bug-18141 on 14-sep-13
            Application.Run(new frmMain(args)); //added by sandeep for bug-18141 on 14-sep-13
            return 1;
            //  added by sandeep for bug-18141 on 14-sep-13 -->End
        }
    }
}
