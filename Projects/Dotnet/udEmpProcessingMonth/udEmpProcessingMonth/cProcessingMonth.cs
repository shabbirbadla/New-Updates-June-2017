using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udProcessingMonth
{
    static class cHolidayMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                //args = new string[] { "1", "P011213", @"DESKTOP241\USQUARE10", "sa", "sa1982", "^14207", "ADMIN", @"D:\Program Files\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "1", "P011213 ", "UDYOG5-PC\\VUDYOGSDK", "sa", "sa1985", "^21001", "ADMIN", @"D:\VUdyogSDK\Bmp\Icon_VudyogSDK.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "Employee Master Updation" };// "Employee Master Updation" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmProcessingMonth(args));
            return 1;
        }
            
            
            
            

    }
}
