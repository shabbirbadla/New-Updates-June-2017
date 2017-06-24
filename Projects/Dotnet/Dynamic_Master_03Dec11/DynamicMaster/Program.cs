using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DynamicMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 8)
            {
                //MessageBox.Show("Incorrect number of parameters passed.");
                //return 0;
                args = new string[] { "5", "N031213", "udyog3\\usquare10", "sa", "sa@1985", "13032", "QPR", "ADMIN", @"D:\USquare10\Bmp\Icon_10USquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
              }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MasterForm(args));
            clsMain oClsMain = new clsMain(args);
            return 1;

        }
    }
}
