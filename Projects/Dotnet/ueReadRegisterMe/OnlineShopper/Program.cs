using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ueReadRegisterMe;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
         {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmOnlineShop());

            //ueReadRegisterMe.ueReadRegisterMe RegMe = new ueReadRegisterMe.ueReadRegisterMe();
            //RegMe.getUnqValue("58557457");
            //RegMe._ueReadRegisterMe(@"D:\Usquare");
            //MessageBox.Show(RegMe.r_add);
            //RegMe.ReadRegisterMe(@"D:\USquare");
        }
    }
}
