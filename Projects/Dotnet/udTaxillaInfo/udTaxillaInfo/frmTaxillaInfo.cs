using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using ueconnect;
using GetInfo;
using System.IO;
namespace udTaxillaInfo
{
    public partial class frmTaxillaInfo : Form
    {
        Icon MainIcon;
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        public frmTaxillaInfo(string[] args)
        {
            InitializeComponent();
            MainIcon = new System.Drawing.Icon(args[0].ToString());
            this.Icon = MainIcon;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            CheckPDF(startupPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.udyogtax.com/taxilla/");
        }
        public void CheckPDF(string ApplicationPath)
        {
               if(File.Exists(Path.Combine(ApplicationPath,"Taxilla Welcome.pdf")))
                {
                    System.Diagnostics.Process.Start(Path.Combine(ApplicationPath,"Taxilla Welcome.pdf"));
                }
                else
                {
                    MessageBox.Show("File not found for View..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
   
                        
        }

        private void frmTaxillaInfo_Load(object sender, EventArgs e)
        {
            startupPath = Application.StartupPath;
            oConnect = new clsConnect();
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            oConnect.InitProc("'" + startupPath + "'", appfile);
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
    }

  
}
