using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace eFillingExtraction
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //para = new string[] { "43", "K011213", "Prod_Shrikant\\shree", "sa", "sa1985", "26Q" };
                //args = new string[] { "24Q", "01/04/2013", "31/03/2014" };
                args = new string[] { "3", "V011516", "PRO_PANKAJ\\VUDYOGSDK", "sa", "sa1985", "^13057", "ADMIN", @"F:\Installer12.0\Bmp\icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "6956", "udPID6956DTM20150702115208", "opening", "1" };

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmEfiling(args));
        }        
    }
    public enum ReturnType
    {
        eTDS, eTCS
    }
    public enum FormType
    {
        Form24Q, Form26Q, Form27Q, Form27EQ
    }
}
