using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UDDataExport
{
    static class csExpMain
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
                //args = new string[] { "19", "Export", "Udyog65", "sa", "sa@1985", "^18010", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "Usquare pack", "VudyogSDK.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "25", "Export", "udyog3\\vudyogsdk", "sa", "sa@1985", "^18010", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "Usquare pack", "VudyogSDK.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*///tESTING
                //args = new string[] { "1", "D011314", "desktop7\\desktop7", "sa", "sa@1985", "^18010", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_VudyogSDK.ico", "Usquare pack", "VudyogSDK.EXE", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                args = new string[] { "2", "X011516", "PRO_PANKAJ\\SQLEXPRESS", "sa", "sa1985", "^13074", "ADMIN", @"F:\Installer12.0\Bmp\icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "424", "udPID424DTM20151027113508" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmExpMain(args));
            return 1;
        }
    }
}
