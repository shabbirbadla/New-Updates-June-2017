using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udUserRolesSelection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main()
        //static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new frmMainUserRoles());
            return 1;
        }
    }
}
