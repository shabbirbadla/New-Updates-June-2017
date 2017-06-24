using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Cost_cat_master
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
                //MessageBox.Show("Incorrect number of parameters passed.");
                //return 0;
                //args = new string[] { "1", "Test", "Udyog12\\SQLEXPRESS", "sa", "sa1985", "12005", "tst", "ADMIN" };
                args = new string[] { "9", "S011415", "PRO_PANKAJ\\PRO", "sa", "sa1985", "13032", "DEG", "ADMIN", @"D:\VudyogPRO\Bmp\Icon_VudyogPRO.ico", "Usquare pack", "VudyogPRO.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            clsMain oClsMain = new clsMain(args);

            //return 1;

            //            Application.Run(new Cost_Cat_Mast());
        }
    }
}
