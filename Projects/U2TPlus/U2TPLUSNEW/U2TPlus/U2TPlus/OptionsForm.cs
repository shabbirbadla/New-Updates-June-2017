using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using U2TPlus.BAL;

namespace U2TPlus
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            DataSet CompanyMasterTableNames = new DataSet();
            DataSet CompanyTransTableNames = new DataSet();
            this.Focus();
            try
            {
                if (ApplicationValues.IsEnvironmentSetup)
                {
                    btnSetEnvironment.Text = "Set Environment";
                    contextMenuStrip1.Items.Clear();
                    contextMenuStrip2.Items.Clear();
                    if (ApplicationValues.ConfigFileValues != null)
                    {
                        BAL.CompanyInfo objCompanyInfo = new CompanyInfo();

                        objCompanyInfo.CompanyDBName = ApplicationValues.CompanyDBName;
                        objCompanyInfo.ConnectionString = ApplicationValues.CompanyDBConnectionString;

                        CompanyMasterTableNames = objCompanyInfo.GetCompanyMasterTableNames();
                        CompanyTransTableNames = objCompanyInfo.GetCompanyTransTableNames();

                        ToolStripMenuItem cat1 = new ToolStripMenuItem("Masters", null, this.MastersToolStripMenuItem_Click);
                        ToolStripMenuItem cat2 = new ToolStripMenuItem("Transactions", null, this.TransactionsToolStripMenuItem_Click);

                        ToolStripMenuItem cat3 = new ToolStripMenuItem("Masters", null, this.MastersToolStripMenuItem_Click);
                        ToolStripMenuItem cat4 = new ToolStripMenuItem("Transactions", null, this.TransactionsToolStripMenuItem_Click);

                        foreach (DataRow row in CompanyMasterTableNames.Tables[0].Rows)
                        {
                            cat1.DropDownItems.Add(row["Name"].ToString().Trim(), null, this.ContextMenuStripClickSettings_Master);
                            cat3.DropDownItems.Add(row["Name"].ToString().Trim(), null, this.ContextMenuStripClickGenerate_Master);
                            contextMenuStrip1.Items.Add(cat1);
                            contextMenuStrip2.Items.Add(cat3);
                        }
                        foreach (DataRow row in CompanyTransTableNames.Tables[0].Rows)
                        {
                            cat2.DropDownItems.Add(row["code_nm"].ToString().Trim(), null, this.ContextMenuStripClickSettings_Trans);
                            cat4.DropDownItems.Add(row["code_nm"].ToString().Trim(), null, this.ContextMenuStripClickGenerate_Trans);
                            contextMenuStrip1.Items.Add(cat2);
                            contextMenuStrip2.Items.Add(cat4);
                        }

                        //contextMenuStrip1.Items.Add(cat2);
                        //contextMenuStrip2.Items.Add(cat4);
                        lblSelectedCompany.Text = ApplicationValues.CompanyName;
                        lblUdyogProductDetails.Text = ApplicationValues.GeneratingProductPath;
                        lblXMLDetails.Text = ApplicationValues.GeneratCompanyXMLFilePath;
                    }
                    else
                    {
                        MessageBox.Show(this, "please Reset The Environment !!!", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    btnGenerate.Enabled = false;
                    btnSettings.Enabled = false;
                    btnShowLogFile.Enabled = true;
                    if (ApplicationValues.ConfigFileValues != null)
                        btnSetEnvironment.Enabled = true;
                    else
                        btnSetEnvironment.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnShowLogFile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(ApplicationValues.ApplicationPath + @"\Log\U2TPlus_Logger.txt");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int x = this.Location.X + this.groupBox3.Location.X + this.btnGenerate.Location.X + this.btnGenerate.Width;
                int y = this.Location.Y + this.groupBox3.Location.Y + this.btnGenerate.Location.Y + this.btnGenerate.Height;
                contextMenuStrip2.Show(x, y);
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                int x = this.Location.X + this.groupBox3.Location.X + this.btnSettings.Location.X + this.btnSettings.Width;
                int y = this.Location.Y + this.groupBox3.Location.Y + this.btnSettings.Location.Y + this.btnSettings.Height;

                contextMenuStrip1.Show(x, y);
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnConfigurations_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationsForm CF = new ConfigurationsForm();
                int x = this.Location.X + this.groupBox3.Location.X + this.btnConfigurations.Location.X + this.btnConfigurations.Width;
                int y = this.Location.Y + this.groupBox3.Location.Y + this.btnConfigurations.Location.Y + this.btnConfigurations.Height;
                CF.SetDesktopLocation(x, y);
                CF.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnSetEnvironment_Click(object sender, EventArgs e)
        {
            try
            {
                SetupEnvironment se = new SetupEnvironment();
                se.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            HelpForm hf = new HelpForm();
            hf.ShowDialog(this);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog(this);
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm Cpwd = new ChangePasswordForm();
            Cpwd.ShowDialog(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            if (DR == DialogResult.Yes)
                Application.Exit();
            else if (DR == DialogResult.No)
                this.Close();
        }

        private void TransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MastersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ContextMenuStripClickSettings_Master(object sender, EventArgs e)
        {
            try
            {
                string MenuItemName = string.Empty;
                string MasterOrTran = string.Empty;
                ToolStripMenuItem ctmenu = (ToolStripMenuItem)sender;
                SettingsForm sf = new SettingsForm(ctmenu.Text, ctmenu.OwnerItem.Text);
                sf.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void ContextMenuStripClickGenerate_Master(object sender, EventArgs e)
        {
            try
            {
                string MenuItemName = string.Empty;
                string MasterOrTran = string.Empty;
                ToolStripMenuItem ctmenu = (ToolStripMenuItem)sender;
                GenerateForm gf = new GenerateForm(ctmenu.Text);
                gf.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void ContextMenuStripClickSettings_Trans(object sender, EventArgs e)
        {
            try
            {
                string MenuItemName = string.Empty;
                string MasterOrTran = string.Empty;
                ToolStripMenuItem ctmenu = (ToolStripMenuItem)sender;
                TranSettingsForm sf = new TranSettingsForm(ctmenu.Text, ctmenu.OwnerItem.Text);
                sf.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void ContextMenuStripClickGenerate_Trans(object sender, EventArgs e)
        {
            try
            {
                string MenuItemName = string.Empty;
                string MasterOrTran = string.Empty;
                ToolStripMenuItem ctmenu = (ToolStripMenuItem)sender;
                TranGenerateForm gf = new TranGenerateForm(ctmenu.Text);
                gf.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
    }
}