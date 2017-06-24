using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
namespace ueTips
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] para)
        {
            if (para.Length == 0)
            {
                para = new string[] {"vudyog", @"AIPLDTM020\SQLEXPRESS", "sa", "sa1985", @"D:\UdyogGSTSDK\", @"D:\UdyogGSTSDK\Bmp\Icon_VudyogGST.ico" };
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTips(para));
            return 1;
            
            
        }
    }
}