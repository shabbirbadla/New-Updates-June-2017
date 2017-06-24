using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DadosReports;

namespace DadosReports
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
            //Testing Archana 
            if (args.Length < 15)
            {
                args = new string[] { "003780|[UserName=Admin][Domain=Adaequare]|[FParty=][TParty=work order][FItem=Finished 1][TItem=Item 2]||[FrmDate=04/01/2013][Todate=03/31/2014]|Data Source=Sachin-PC;Persist Security Info=True;Password=sa1985;User ID=sa;Initial Catalog=B091314", "11", "B091314", @"Archana", "sa", "sa1985", "^19003", "ADMIN", @"E:\Vudyog Sdk_11\Bmp\Icon_VudyogSDK.ico", "Usquare10", "Usquare10.EXE", "4764", "udPID4764DTM20111213125821", "", "", "", "01-04-2012", "31-03-2013" };
                //args = new string[] { "17", "D011213", @"sachin", "sa", "sa1985", "^19003", "ADMIN", @"C:\VU10\VudyogSTD\Bmp\Icon_VudyogSTD.ico", "USquare10", "USQUARE10.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2012", "31-03-2013" };
            }
            if (args.Length != 0)
            {
                Loading.ShowSplashScreen();
                Application.Run(new ReportsMain(args));
            }
            else
            {

                MessageBox.Show("Please Supply the argument with this exe!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            
        }
    }
}