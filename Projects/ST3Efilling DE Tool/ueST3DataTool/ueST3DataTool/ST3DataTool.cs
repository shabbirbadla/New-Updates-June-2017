using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ueST3DataTool
{
    //static class ST3DataTool
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
                MessageBox.Show("Blank Parameter");
                para = new string[] { "T031011", "udyog11", "sa", "sa@1985", "","195",""};
               // para = new string[] { "", "", "", "", "",""};
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_ST3DataTool(para));
            return 1;
        }
    }
}