using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UdAlert
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
          static int  Main(string[] args)
           {
            if (args.Length < 1)
            {
                args = new string[] { "6", "N011112", "udyog5\\usqare", "sa", "sa1985", "ADMIN", @"D:\VU10\VudyogSDK\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmAlert(args));
            return 1;
          }
    }
}
