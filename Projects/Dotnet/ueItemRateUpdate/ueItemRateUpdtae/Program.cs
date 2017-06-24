using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ueItemRateUpdtae
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                //MessageBox.Show("Not enough Parameter...");
                //return 1;
               // args = new string[] { "1", "U021112", "udyog5\\usqare", "sa", "sa1985", "^21001", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/

                //args = new string[] { "4", "C011718", "AIPLDTM020\\SQLEXPRESS", "sa", "sa1985", "^21001", "ADMIN", @"D:\Installer12.0\GST\VudyogGSTSDK\Bmp\Icon_vudyoggssdk.ico", "Visual Udyog GSTSDK", "VudyogGSTSDK.exe", "5308", "udPID5308DTM20170401113131" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmItemDetailsMain(args));


            return 1;
        }
    }
}
