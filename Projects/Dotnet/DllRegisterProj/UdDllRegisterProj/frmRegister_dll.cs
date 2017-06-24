using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;

namespace WindowsFormsApplication5
{
    public partial class frmRegister_dll : Form
    {
        Icon MainIcon;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
       
        public frmRegister_dll()
        {
            InitializeComponent();
            
        }
       public static void Registar_Dlls(string filePath)
            {
                try
                {
                   
                        string arg_fileinfo = filePath;
                        Process reg = new Process();
                        reg.StartInfo.FileName= "regsvr32.exe";
                        reg.StartInfo.Arguments= arg_fileinfo;
                        reg.StartInfo.UseShellExecute = false;
                        reg.StartInfo.CreateNoWindow = true;
                        reg.StartInfo.RedirectStandardOutput = true;
                        reg.Start();
                        reg.WaitForExit();
                        
                        MessageBox.Show("DLL registration done successfully.");
                        reg.Close();
                        Application.Exit();
                   
                }
               catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
            public void Checkdll(string ApplicationPath)
            {
                if (File.Exists(Path.Combine(ApplicationPath,"FATHZIP.DLL")))
                {
                   string dllnm = startupPath + "\\FATHZIP.DLL /s";
                   Registar_Dlls(dllnm);
                }
                else
                {
                    MessageBox.Show("File not found for View..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


            }
        private void btnregister_dll_Click(object sender, EventArgs e)
        {

            if (IsUserAdministrator())
            {
                Checkdll(startupPath);
            }
            else
            {
                DialogResult x = MessageBox.Show("DLL registration should be run with Administrator Rights.\n If you are using Windows Vista or Later Version,\n please exit and select Run as Administrator after Right Clicking the Exe.\n If you continue, the DLL may not be installed successfully.\nContinue ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (x == DialogResult.Yes)
                {
                    Checkdll(startupPath);
                }
                else
                {
                    Application.Exit();
                }
            }
            
        }

        private void frmRegister_dll_Load(object sender, EventArgs e)
        {
            startupPath = Application.StartupPath;
    
        }
       
        public bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

    }
}
