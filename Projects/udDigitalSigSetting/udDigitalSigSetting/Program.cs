using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udDigitalSigSetting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 8)
            {
                //args = new string[] { "2", "A011415", @"PRO_PANKAJ\PRO", "sa", "sa1985", "^13004", "ADMIN", @"D:\VudyogPRO\Bmp\Icon_VudyogPRO.ico", "VudyogPRO", "VudyogPRO.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "12", "E051112", "udyog5\\sqlexpress", "sa", "sa1985", "^13060", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                //args = new string[] { "70", "C011415", @"PRO_PANKAJ\VUDYOGSDK", "sa", "sa1985", "^13004", "ADMIN", @"D:\VUdyogSDK\Bmp\Icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "61", "G151718", @"AIPLDTM019\SQLEXPRESS", "sa", "sa1985", "^21345", "ADMIN", @"E:\U3\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "VudyogGST", "VudyogGST.EXE", "2356", "udPID5480DTM20150910122846" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDigitalSigSetting(args));
        }
    }
}
