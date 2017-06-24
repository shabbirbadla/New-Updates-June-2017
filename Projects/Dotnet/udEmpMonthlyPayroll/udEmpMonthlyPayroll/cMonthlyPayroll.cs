using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpMonthlyPayroll
{
    static class cMonthlyPayroll
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                //args = new string[] { "11", "UdSep12", "udyog65", "sa", "sa@1985", "^14217", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
               //args = new string[] { "8", "Bug-14222", "udyog5-pc\\VUdyogSDK", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };
           // args = new string[] { "1", "N031314", "PROD_SATISH\\SATISH", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };
                
               // args = new string[] { "8", "Testing", "udyog5-pc\\VUdyogSDK", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };
                ///args = new string[] { "1", "P011213", "udyog5-pc\\VUdyogSDK", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };

               // args = new string[] { "1", "E011213", "udyog5-pc\\VUdyogEnt", "sa", "sa1985", "^21001", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };
                args = new string[] { "21", "A011314", "Archana", "sa", "sa1985", "^14217", "ADMIN", @"D:\VudyogSDK\Bmp\icon_VudyogSDK.ico", "VudyogENT", "VudyogENT.exe", "4764", "udPID4764DTM20111213125821", "opening" };


            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMonthlyPayroll(args));
            return 1;
        }
    }
}
