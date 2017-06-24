using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace udAttendanceSettings
{
    static class cAttendanceSettings
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
        
            if(args.Length<3)
            {
                //args = new string[] { "19", "A021112", "udyog65", "sa", "sa@1985", "^13044", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
               // args = new string[] { "20", "N011213", @"Desktop29\SQLEXPRESS", "sa", "sa1985", "^13004", "ADMIN", @"D:\VUdyog_Installations\VUdyogSDK\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "2", "N011213", "udyog5\\USQUARE10New", "sa", "sa1985", "^21001", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821", "opening" };

            }
          
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);
           
            //MessageBox.Show("args"+args.Length);
            Application.Run(new frmAttendanceSettings(args));
            
            return 1;
            
        }
            
             
            
            

    }
}
