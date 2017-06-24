using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class csImpMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                //args = new string[] { "36", "TestingImport", "udyog3\\vudyogsdk", "sa", "sa@1985", "^18010", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "10", "A021213", "udyog3\\Usquare10", "sa", "sa@1985", "^18010", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "2", "T031314", "desktop7", "sa", "sa@1985", "^18010", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_vudyogsdk.ico", "Usquare pack", "Vudyogsdk.EXE", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "16", "B061516", "SACHIN-PC", "sa", "sa1985", "^21259", "ADMIN", @"D:\VudyogSDK\Bmp\Icon_vudyogsdk.ico", "Usquare pack", "Vudyogsdk.EXE", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                args = new string[] { "31", "B021516", "PRO_PANKAJ\\SQLEXPRESS", "sa", "sa1985", "^13074", "ADMIN", @"F:\Installer12.0\Bmp\icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "4200", "udPID4200DTM20151026095906" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmImpMain(args));
            return 1;
        }
    }
}
