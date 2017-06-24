using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ueTblFields
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
            //args = new string[] { "3", "U011112", "UDYOG5\\usqare", "sa", "sa1985", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "^21018", "Salesman Updation In Sales Transaction" };// "Salesman Name Update Utility" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            //args = new string[] { "5", "A011213 ", "UDYOG5\\usquare10new", "sa", "sa1985", "ADMIN", @"D:\Usquare\Bmp\Icon_10usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "^21018", "Salesman Updation In Sales Transaction" };// "Salesman Name Update Utility" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            args = new string[] { "1", "P011213 ", "UDYOG5-PC\\VUDYOGSDK", "sa", "sa1985", "ADMIN", @"D:\VUdyogSDK\Bmp\Icon_VudyogSDK.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "^21001", "Employee Master Updation" };// "Employee Master Updation" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/

            //   args = new string[] { "1", "P011213", "UDYOG5-PC\\VUDYOGSDK", "sa", "sa1985", "ADMIN", @"D:\VUdyogSDK\Bmp\Icon_VudyogSDK.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "^21001", "Salesman Updation In Sales Transaction" };// "Employee Master Updation" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/

                
           //   args = new string[] { "2", "T011112", "UDYOG5\\vu10sdk", "sa", "sa1985", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "^21018", "Stock Valuation Rate Update Utility" };// "Salesman Name Update Utility" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/

            }
            //MessageBox.Show(args[0] + "," + args[1] + "," + args[2] + "," + args[3] + "," + args[4] + "," + args[5] + "," + args[6]+ "," +args[7]);
            //MessageBox.Show(args[8] + "," + args[9] + "," + args[10] + "," + args[11] + "," + args[12] + "," + args[13] );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmFields(args));
            return 1;
        }
    }
}
