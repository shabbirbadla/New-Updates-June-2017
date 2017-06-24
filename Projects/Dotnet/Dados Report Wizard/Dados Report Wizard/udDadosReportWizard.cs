using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Dados_Report_Wizard
{
    static class udDadosReportWizard
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args )
        {
            //if (args.Length < 15)
            //{
            //    args = new string[] { "11", "B091314", @"sachin-pc", "sa", "sa1985", "^19003", "ADMIN", @"D:\USQUARE10\Bmp\icon_10USquare.ico", "USquare10", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2013", "31-03-2014" };
            //    //args = new string[] { "17", "D011213", @"sachin-pc", "sa", "sa1985", "^19003", "ADMIN", @"D:\USQUARE10\Bmp\icon_10USquare.ico", "USquare10", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2012", "31-03-2013" };
            //    //args = new string[] { "17", "D011213", @"sachin", "sa", "sa1985", "^19003", "ADMIN", @"C:\VU10\VudyogSTD\Bmp\Icon_VudyogSTD.ico", "USquare10", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2012", "31-03-2013" };
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDadosReport(args));
            
        }
    }
}
