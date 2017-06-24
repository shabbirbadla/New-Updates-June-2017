using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udTaxillaInfo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //if (args.Length < 1)
            //{
            //   args = new string[] { @"E:\Vudyog Sdk_11\Bmp\icon_VudyogSDK.ico" };
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTaxillaInfo(args));
        }
    }
}
