using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DynamicMasterDesigner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 6)
            {
                //MessageBox.Show("Incorrect number of parameters passed.");
                //return 0;
                args = new string[] { "1", "Test", "Udyog12\\SQLEXPRESS", "sa", "sa1985", "^12005", "ADMIN" };                
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMasterFormDesigner(args));
            return 1;
        }
    }
}
