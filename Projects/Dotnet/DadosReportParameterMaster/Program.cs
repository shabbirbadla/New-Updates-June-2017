using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DadosReportParameterMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ParameterMaster(args));

            //if (args.Length < 15)
            //{
            //    args = new string[] { "11", "B091314", @"sachin-pc", "sa", "sa1985", "^19003", "ADMIN", @"D:\USQUARE10\Bmp\icon_10USquare.ico", "USquare10", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2013", "31-03-2014" };
            //}

            if (args.Length != 0)
            {
                Application.Run(new ParameterMaster(args));
            }
            else
            {
                MessageBox.Show("Please supply the argument with this exe!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            
        }
    }
}
