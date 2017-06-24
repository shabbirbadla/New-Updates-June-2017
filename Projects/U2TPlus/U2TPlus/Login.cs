using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;

namespace U2TPlus
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userid = AutoSettings.Default.UserID;
            string pws = ApplicationValues.Decript(AutoSettings.Default.Password);
            string ModifyDate = AutoSettings.Default.ModifyDate.Date.ToString();
            int fCount = AutoSettings.Default.FilureCount;
            if (fCount < 3)
            {
                if (txtUserName.Text == userid)
                {
                    if (txtPassword.Text == pws)
                    {
                        AutoSettings.Default.FilureCount = 0;
                        AutoSettings.Default.Save();

                        this.Hide();                        
                        U2TPlusForm mf = new U2TPlusForm();
                        ApplicationValues.UserName = this.txtUserName.Text;
                        mf.Show();

                        Logger.LogInfo("Login is sucsess");
                    }
                    else
                    {
                        MessageBox.Show(this, "May be UserID or Password are Wrong", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        fCount = fCount + 1;
                        AutoSettings.Default.FilureCount = fCount;
                        AutoSettings.Default.Save();
                        Logger.LogInfo("May be UserID or Password are Wrong");
                    }
                }
                else
                {
                    MessageBox.Show(this, "May be UserID or Password are Wrong", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    fCount = fCount + 1;
                    AutoSettings.Default.FilureCount = fCount;
                    AutoSettings.Default.Save();
                    Logger.LogInfo("May be UserID or Password are Wrong");
                }
            }
            else
            {
                MessageBox.Show(this, "Please Try Later With Known UserID and Password.", "Sorry You Can't Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                AutoSettings.Default.FilureCount = 0;
                AutoSettings.Default.Save();
                Logger.LogInfo("'Please Try Later With Known UserID and Password.', 'Sorry You Can't Login'");
                Application.Exit();

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.Focus();
            string pws = ApplicationValues.Decript(AutoSettings.Default.Password);
            if (pws == "admin")
            {
                toolTip2.Active = true;
            }
            else
            {
                toolTip2.Hide(this);
                toolTip2.Active = false;
            }
        }
    }
}