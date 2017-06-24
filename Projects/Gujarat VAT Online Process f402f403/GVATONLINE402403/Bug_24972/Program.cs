using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bug_24972
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
                args = new string[] { "F402403", "01/04/2014", "31/03/2015" };
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm402403(args));
        }
    }
}
