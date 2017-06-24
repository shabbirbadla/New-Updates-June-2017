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
    public partial class ChangePasswordForm : Form
    {
        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text != string.Empty)
            {
                if (txtNewPassword.Text != string.Empty)
                {
                    if (txtConfirmPassword.Text != string.Empty)
                    {
                        string oldpass = ApplicationValues.Decript(AutoSettings.Default.Password);

                        if (oldpass.Trim().Equals(txtOldPassword.Text.Trim(), StringComparison.CurrentCulture))
                        {
                            if (txtNewPassword.Text.Trim().Equals(txtConfirmPassword.Text.Trim(), StringComparison.CurrentCulture))
                            {
                                string newPass = ApplicationValues.Encript(txtNewPassword.Text.Trim());
                                AutoSettings.Default.Password = newPass;
                                AutoSettings.Default.ModifyDate = DateTime.Today.Date;
                                AutoSettings.Default.Save();

                                MessageBox.Show(this, "Sucssfully Changed Password", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                Logger.LogInfo("Sucssfully Changed Password");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show(this, "NEW and Confirm Password are Not Maching", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                txtConfirmPassword.Text = "";
                                txtNewPassword.Text = "";
                                Logger.LogInfo("NEW and Confirm Password are Not Maching");
                            }

                        }
                        else
                        {
                            MessageBox.Show(this, "Please Enter Currect Old Password", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            txtOldPassword.Text = "";
                            Logger.LogInfo("Please Enter Currect Old Password");
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please Enter the Confirm Password", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Logger.LogInfo("Please Enter the Confirm Password");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please Enter the New Password", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    Logger.LogInfo("Please Enter the New Password");
                }
            }
            else
            {
                MessageBox.Show(this, "Please Enter the Old Password", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Logger.LogInfo("Please Enter the Old Password");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            lblDispalyUserID.Text = "You Are Loged in As : " + AutoSettings.Default.UserID;
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Trim().Equals(txtConfirmPassword.Text.Trim(), StringComparison.CurrentCulture))
            {

            }
            else
            {
                MessageBox.Show(this, "Please Enter Currect Password In New And Confirm", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtConfirmPassword.Text = "";
                txtNewPassword.Text = "";
                txtOldPassword.Text = "";
            }
        }
    }
}