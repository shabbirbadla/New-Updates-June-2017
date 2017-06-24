using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;
using System.IO;

namespace U2TPlus
{
    public partial class U2TPlusForm_old : Form
    {
        bool configValue = false;
        DataSet RecordsSet = null;
        public U2TPlusForm_old()
        {
            InitializeComponent();

            #region Assigning Application Values

            ApplicationValues.ApplicationPath = Application.StartupPath;
            ApplicationValues.ConfigXMLFileName = "Config.xml";
            ApplicationValues.ConfigFolderPath = ApplicationValues.ApplicationPath + @"\Config";
            ApplicationValues.ConfigFilePath = ApplicationValues.ConfigFolderPath + @"\" + ApplicationValues.ConfigXMLFileName;
            ApplicationValues.ApplicationName = Application.ProductName;
            ApplicationValues.U2TMarkupsFolderPath = Application.StartupPath + @"\U2TMarkups";
            ApplicationValues.DefaultMasterSettingsXMLName = Application.StartupPath + @"\U2TMarkups\SettingsMaster.xml";
            #endregion
        }

        private void U2TPlusForm_Load(object sender, EventArgs e)
        {
            txtXMLPath.Focus();
            if (Directory.Exists(ApplicationValues.ConfigFolderPath))
            {
                string[] files = Directory.GetFiles(ApplicationValues.ConfigFolderPath);

                if (files.Length > 0)
                {
                    foreach (string filename in files)
                    {
                        FileInfo fInfo = new FileInfo(filename);
                        if (fInfo.Name == ApplicationValues.ConfigXMLFileName)
                        {
                            configValue = true;
                            DataSet DS = new DataSet();
                            DS.ReadXml(ApplicationValues.ConfigFilePath);
                            ApplicationValues.ConfigFileValues = DS;
                            if (DS != null)
                            {
                                if (ApplicationValues.IsEnvironmentSetup)
                                    ToDisplayOptions();
                                else
                                {
                                    DisplayEnvironment(fInfo.FullName);
                                    setEnvironment();
                                    ToDisplayOptions();
                                }
                            }
                        }
                    }
                }
                if (configValue == false)
                {
                    //ConfigurationsForm CF = new ConfigurationsForm();
                    //CF.Show(this);
                    configuretion();
                    Logger.LogInfo("Config.xml file is not there");
                }
            }
            else
            {
                //ConfigurationsForm CF = new ConfigurationsForm();
                //CF.Show(this);
                configuretion();
                Logger.LogInfo("Config folder is not there");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            if (DR == DialogResult.Yes)
                Application.Exit();
        }

        

        private void configurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationsForm CF = new ConfigurationsForm();
                CF.Show(this);        
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void mastersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tranToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mastersToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm Cpwd = new ChangePasswordForm();
            Cpwd.ShowDialog(this);
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(ApplicationValues.ApplicationPath + @"\Log\U2TPlus_Logger.txt");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm hf = new HelpForm();
            hf.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog(this);
        }


        private void DisplayEnvironment(string filepath)
        {
            try
            {
                cmbTotalRecords.Focus();

                if (Directory.Exists(ApplicationValues.ConfigFolderPath))
                {
                    string[] files = Directory.GetFiles(ApplicationValues.ConfigFolderPath);

                    if (files.Length > 0)
                    {
                        foreach (string filename in files)
                        {
                            FileInfo fInfo = new FileInfo(filename);
                            if (fInfo.Name == ApplicationValues.ConfigXMLFileName)
                            {
                                RecordsSet = new DataSet();
                                RecordsSet.ReadXml(filename);

                                ApplicationValues.ConfigFileValues = RecordsSet;

                                if (RecordsSet != null)
                                {
                                    DataTable dtRecords = new DataTable();
                                    dtRecords.Columns.Add("ConfigText");
                                    dtRecords.Columns.Add("ConfigNumber");
                                    DataRow RecordsRow = null;

                                    foreach (DataRow row in RecordsSet.Tables[0].Rows)
                                    {
                                        RecordsRow = dtRecords.NewRow();
                                        RecordsRow["ConfigText"] = "( " + row["CompanyName"].ToString() + " : " + row["FinancialYear"].ToString() + " ) ( Tally :" + row["VersionOftheTally"].ToString() + " )";
                                        RecordsRow["ConfigNumber"] = row["ConfigNumber"].ToString();
                                        dtRecords.Rows.Add(RecordsRow);
                                    }
                                    cmbTotalRecords.DisplayMember = "ConfigText";
                                    cmbTotalRecords.ValueMember = "ConfigNumber";
                                    cmbTotalRecords.DataSource = RecordsRow.Table.DefaultView;

                                    txtXMLPath.ReadOnly = true;
                                    txtVisualUdyogPath.ReadOnly = true;
                                    txtVersionOftheTally.ReadOnly = true;
                                    txtSelectCompany.ReadOnly = true;
                                    txtFinancialYear.ReadOnly = true;
                                }
                                else
                                {
                                    cmbTotalRecords.Enabled = false;
                                    txtXMLPath.ReadOnly = true;
                                    txtVisualUdyogPath.ReadOnly = true;
                                    txtVersionOftheTally.ReadOnly = true;
                                    txtSelectCompany.ReadOnly = true;
                                    txtFinancialYear.ReadOnly = true;
                                }

                            }
                            else
                            {
                                cmbTotalRecords.Enabled = false;
                                txtXMLPath.ReadOnly = true;
                                txtVisualUdyogPath.ReadOnly = true;
                                txtVersionOftheTally.ReadOnly = true;
                                txtSelectCompany.ReadOnly = true;
                                txtFinancialYear.ReadOnly = true;
                            }
                        }//
                    }
                    else
                    {
                        cmbTotalRecords.Enabled = false;
                        txtXMLPath.ReadOnly = true;
                        txtVisualUdyogPath.ReadOnly = true;
                        txtVersionOftheTally.ReadOnly = true;
                        txtSelectCompany.ReadOnly = true;
                        txtFinancialYear.ReadOnly = true;
                    }

                    if (RecordsSet != null)
                    {
                        if (RecordsSet.Tables[0].Rows.Count == 1 && ApplicationValues.IsEnvironmentSetup == false)
                        {
                            ApplicationValues.ApplicationName = Application.ProductName;

                            if (cmbTotalRecords.SelectedIndex != -1)
                            {
                                string filter = "ConfigNumber=" + cmbTotalRecords.SelectedValue;
                                DataRow[] rows = RecordsSet.Tables[0].Select(filter);
                                foreach (DataRow dRow in rows)
                                {
                                    ApplicationValues.DecriptSUPValues = dRow["ConnectionValues"].ToString();
                                }
                            }
                            else
                                MessageBox.Show(this, "Please Select The Record", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


                            string[] DBValues = ApplicationValues.DecriptSUPValues.Split(new char[] { ':' });

                            ApplicationValues.DBServerName = DBValues[0].ToString();
                            ApplicationValues.DBUserID = DBValues[1].ToString();
                            ApplicationValues.DBPassword = DBValues[2].ToString();
                            ApplicationValues.CompanyDBName = ApplicationValues.Decript(txtCompanyDataBaseName.Text);
                            string conn = "Data Source=" + ApplicationValues.DBServerName.Trim() + ";Initial Catalog=" + ApplicationValues.CompanyDBName.Trim() + ";Persist Security Info=True;User ID=" + ApplicationValues.DBUserID.Trim() + ";Password=" + ApplicationValues.DBPassword.Trim();
                            ApplicationValues.CompanyDBConnectionString = conn;
                            ApplicationValues.CompanyName = txtSelectCompany.Text;
                            ApplicationValues.CompanyConfigID = txtCompanyConfigID.Text;
                            ApplicationValues.GeneratCompanyXMLFilePath = txtXMLPath.Text;
                            ApplicationValues.INIFilePath = txtXMLPath.Text + @"\Visudyog.ini";
                            ApplicationValues.GeneratingProductPath = txtVisualUdyogPath.Text;
                            ApplicationValues.TallyVersion = txtVersionOftheTally.Text;
                            ApplicationValues.ComanyFinancealYear = txtFinancialYear.Text;
                            StringBuilder strMasterXMLHeader = new StringBuilder();
                            StringBuilder strMasterXMLFooter = new StringBuilder();

                            strMasterXMLHeader.AppendLine();
                            strMasterXMLHeader.AppendLine("<ENVELOPE>");
                            strMasterXMLHeader.AppendLine("<HEADER>");
                            strMasterXMLHeader.AppendLine("<TALLYREQUEST>");
                            strMasterXMLHeader.Append("Import Data");
                            strMasterXMLHeader.AppendLine("</TALLYREQUEST>");
                            strMasterXMLHeader.AppendLine("</HEADER>");
                            strMasterXMLHeader.AppendLine("<BODY>");
                            strMasterXMLHeader.AppendLine("<IMPORTDATA>");
                            strMasterXMLHeader.AppendLine("<REQUESTDESC>");
                            strMasterXMLHeader.AppendLine("<REPORTNAME>");
                            strMasterXMLHeader.Append("All Masters");
                            strMasterXMLHeader.AppendLine("</REPORTNAME>");
                            strMasterXMLHeader.AppendLine("</REQUESTDESC>");
                            strMasterXMLHeader.AppendLine("<REQUESTDATA>");


                            strMasterXMLFooter.AppendLine("</REQUESTDATA>");
                            strMasterXMLFooter.AppendLine("</IMPORTDATA>");
                            strMasterXMLFooter.AppendLine("</BODY>");
                            strMasterXMLFooter.AppendLine("</ENVELOPE>");


                            ApplicationValues.GenaretingXMLHeader = strMasterXMLHeader;
                            ApplicationValues.GeneratingXMLFooter = strMasterXMLFooter;

                            StringBuilder strTranXMLHeader = new StringBuilder();
                            StringBuilder strTranXMLFooter = new StringBuilder();

                            strTranXMLHeader.AppendLine();
                            strTranXMLHeader.AppendLine("<ENVELOPE>");
                            strTranXMLHeader.AppendLine("<HEADER>");
                            strTranXMLHeader.AppendLine("<TALLYREQUEST>");
                            strTranXMLHeader.Append("Import Data");
                            strTranXMLHeader.AppendLine("</TALLYREQUEST>");
                            strTranXMLHeader.AppendLine("</HEADER>");
                            strTranXMLHeader.AppendLine("<BODY>");
                            strTranXMLHeader.AppendLine("<IMPORTDATA>");
                            strTranXMLHeader.AppendLine("<REQUESTDESC>");
                            strTranXMLHeader.AppendLine("<REPORTNAME>");
                            strTranXMLHeader.Append("Vouchers");
                            strTranXMLHeader.AppendLine("</REPORTNAME>");
                            strTranXMLHeader.AppendLine("<STATICVARIABLES>");
                            strTranXMLHeader.AppendLine("<SVCURRENTCOMPANY>");
                            strTranXMLHeader.Append(ApplicationValues.CompanyName);
                            strTranXMLHeader.AppendLine("</SVCURRENTCOMPANY>");
                            strTranXMLHeader.AppendLine("</STATICVARIABLES>");
                            strTranXMLHeader.AppendLine("</REQUESTDESC>");
                            strTranXMLHeader.AppendLine("<REQUESTDATA>");

                            strTranXMLFooter.AppendLine("</REQUESTDATA>");
                            strTranXMLFooter.AppendLine("</IMPORTDATA>");
                            strTranXMLFooter.AppendLine("</BODY>");
                            strTranXMLFooter.AppendLine("</ENVELOPE>");


                            ApplicationValues.TranXMLHeader = strTranXMLHeader;
                            ApplicationValues.TranXMLFooter = strTranXMLFooter;

                            ApplicationValues.IsEnvironmentSetup = true;

                            //OptionsForm of = new OptionsForm();
                            //of.Show(this.Owner);
                            //this.Close();
                        }
                        else
                        {
                            if (RecordsSet.Tables[0].Rows.Count == 1 && ApplicationValues.IsEnvironmentSetup == true)
                            {
                                setEnvironment();
                            }
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(ApplicationValues.ConfigFolderPath);
                    cmbTotalRecords.Enabled = false;
                    txtXMLPath.ReadOnly = true;
                    txtVisualUdyogPath.ReadOnly = true;
                    txtVersionOftheTally.ReadOnly = true;
                    txtSelectCompany.ReadOnly = true;
                    txtFinancialYear.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }


        private void cmbTotalRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTotalRecords.SelectedIndex != -1)
                {
                    string filter = "ConfigNumber=" + cmbTotalRecords.SelectedValue;
                    DataRow[] rows = RecordsSet.Tables[0].Select(filter);
                    foreach (DataRow dRow in rows)
                    {
                        txtFinancialYear.Text = dRow["FinancialYear"].ToString();
                        txtSelectCompany.Text = dRow["CompanyName"].ToString();
                        txtVersionOftheTally.Text = dRow["VersionOftheTally"].ToString();
                        txtVisualUdyogPath.Text = dRow["VisualUdyogPath"].ToString();
                        txtXMLPath.Text = dRow["XMLPath"].ToString();
                        txtCompanyDataBaseName.Text = dRow["CompanyValue"].ToString();
                        txtCompanyConfigID.Text = dRow["ConfigNumber"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }


        private void ToDisplayOptions()
        {
            DataSet CompanyMasterTableNames = new DataSet();
            DataSet CompanyTransTableNames = new DataSet();
            this.Focus();
            try
            {
                if (ApplicationValues.IsEnvironmentSetup)
                {
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
                            settingsToolStripMenuItem.DropDown.Items.Add(cat1);
                            generateToolStripMenuItem.DropDown.Items.Add(cat3);
                        }
                        foreach (DataRow row in CompanyTransTableNames.Tables[0].Rows)
                        {
                            cat2.DropDownItems.Add(row["code_nm"].ToString().Trim(), null, this.ContextMenuStripClickSettings_Trans);
                            cat4.DropDownItems.Add(row["code_nm"].ToString().Trim(), null, this.ContextMenuStripClickGenerate_Trans);
                            settingsToolStripMenuItem.DropDown.Items.Add(cat2);
                            generateToolStripMenuItem.DropDown.Items.Add(cat4);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "please Reset The Environment !!!", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    generateToolStripMenuItem.Enabled = false;
                    settingsToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
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
                sf.Show(this);
                //this.Close();
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
                gf.Show(this);
                //this.Close();
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
                sf.Show(this);
                //this.Close();
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
                gf.Show(this);
                //this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void setEnvironment()
        {
            try
            {
                ApplicationValues.ApplicationName = Application.ProductName;

                if (cmbTotalRecords.SelectedIndex != -1)
                {
                    string filter = "ConfigNumber=" + cmbTotalRecords.SelectedValue;
                    DataRow[] rows = RecordsSet.Tables[0].Select(filter);
                    foreach (DataRow dRow in rows)
                    {
                        ApplicationValues.DecriptSUPValues = dRow["ConnectionValues"].ToString();
                    }
                }
                else
                    MessageBox.Show(this, "Please Select The Record", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


                string[] DBValues = ApplicationValues.DecriptSUPValues.Split(new char[] { ':' });

                ApplicationValues.DBServerName = DBValues[0].ToString();
                ApplicationValues.DBUserID = DBValues[1].ToString();
                ApplicationValues.DBPassword = DBValues[2].ToString();
                ApplicationValues.CompanyDBName = ApplicationValues.Decript(txtCompanyDataBaseName.Text);
                string conn = "Data Source=" + ApplicationValues.DBServerName.Trim() + ";Initial Catalog=" + ApplicationValues.CompanyDBName.Trim() + ";Persist Security Info=True;User ID=" + ApplicationValues.DBUserID.Trim() + ";Password=" + ApplicationValues.DBPassword.Trim();
                ApplicationValues.CompanyDBConnectionString = conn;
                ApplicationValues.CompanyName = txtSelectCompany.Text;
                ApplicationValues.CompanyConfigID = txtCompanyConfigID.Text;
                ApplicationValues.GeneratCompanyXMLFilePath = txtXMLPath.Text;
                ApplicationValues.INIFilePath = txtXMLPath.Text + @"\Visudyog.ini";
                ApplicationValues.GeneratingProductPath = txtVisualUdyogPath.Text;
                ApplicationValues.TallyVersion = txtVersionOftheTally.Text;
                ApplicationValues.ComanyFinancealYear = txtFinancialYear.Text;
                StringBuilder strMasterXMLHeader = new StringBuilder();
                StringBuilder strMasterXMLFooter = new StringBuilder();

                strMasterXMLHeader.AppendLine();
                strMasterXMLHeader.AppendLine("<ENVELOPE>");
                strMasterXMLHeader.AppendLine("<HEADER>");
                strMasterXMLHeader.AppendLine("<TALLYREQUEST>");
                strMasterXMLHeader.Append("Import Data");
                strMasterXMLHeader.AppendLine("</TALLYREQUEST>");
                strMasterXMLHeader.AppendLine("</HEADER>");
                strMasterXMLHeader.AppendLine("<BODY>");
                strMasterXMLHeader.AppendLine("<IMPORTDATA>");
                strMasterXMLHeader.AppendLine("<REQUESTDESC>");
                strMasterXMLHeader.AppendLine("<REPORTNAME>");
                strMasterXMLHeader.Append("All Masters");
                strMasterXMLHeader.AppendLine("</REPORTNAME>");
                strMasterXMLHeader.AppendLine("</REQUESTDESC>");
                strMasterXMLHeader.AppendLine("<REQUESTDATA>");

                strMasterXMLFooter.AppendLine("</REQUESTDATA>");
                strMasterXMLFooter.AppendLine("</IMPORTDATA>");
                strMasterXMLFooter.AppendLine("</BODY>");
                strMasterXMLFooter.AppendLine("</ENVELOPE>");


                ApplicationValues.GenaretingXMLHeader = strMasterXMLHeader;
                ApplicationValues.GeneratingXMLFooter = strMasterXMLFooter;

                StringBuilder strTranXMLHeader = new StringBuilder();
                StringBuilder strTranXMLFooter = new StringBuilder();

                strTranXMLHeader.AppendLine();
                strTranXMLHeader.AppendLine("<ENVELOPE>");
                strTranXMLHeader.AppendLine("<HEADER>");
                strTranXMLHeader.AppendLine("<TALLYREQUEST>");
                strTranXMLHeader.Append("Import Data");
                strTranXMLHeader.AppendLine("</TALLYREQUEST>");
                strTranXMLHeader.AppendLine("</HEADER>");
                strTranXMLHeader.AppendLine("<BODY>");
                strTranXMLHeader.AppendLine("<IMPORTDATA>");
                strTranXMLHeader.AppendLine("<REQUESTDESC>");
                strTranXMLHeader.AppendLine("<REPORTNAME>");
                strTranXMLHeader.Append("Vouchers");
                strTranXMLHeader.AppendLine("</REPORTNAME>");
                strTranXMLHeader.AppendLine("<STATICVARIABLES>");
                strTranXMLHeader.AppendLine("<SVCURRENTCOMPANY>");
                strTranXMLHeader.Append(ApplicationValues.CompanyName);
                strTranXMLHeader.AppendLine("</SVCURRENTCOMPANY>");
                strTranXMLHeader.AppendLine("</STATICVARIABLES>");
                strTranXMLHeader.AppendLine("</REQUESTDESC>");
                strTranXMLHeader.AppendLine("<REQUESTDATA>");

                strTranXMLFooter.AppendLine("</REQUESTDATA>");
                strTranXMLFooter.AppendLine("</IMPORTDATA>");
                strTranXMLFooter.AppendLine("</BODY>");
                strTranXMLFooter.AppendLine("</ENVELOPE>");


                ApplicationValues.TranXMLHeader = strTranXMLHeader;
                ApplicationValues.TranXMLFooter = strTranXMLFooter;

                ApplicationValues.IsEnvironmentSetup = true;

                //OptionsForm of = new OptionsForm();
                //of.Show(this.Owner);
                //this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NewConfigForm ncf = new NewConfigForm("Save", "Adding New Configuration", string.Empty);
            ncf.Show(this.Owner);           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cmbTotalRecords.Items.Count != 0)
            {
                NewConfigForm ncf = new NewConfigForm("Update", "Edit Configuration", cmbTotalRecords.SelectedValue.ToString());
                ncf.Show(this.Owner);                
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTotalRecords.Items.Count != 0)
            {
                NewConfigForm ncf = new NewConfigForm("Delete", "Delete Configuration", cmbTotalRecords.SelectedValue.ToString());
                ncf.Show(this.Owner);                
            }
        }

        private void configuretion()
        {
            try
            {
                if (Directory.Exists(ApplicationValues.ConfigFolderPath))
                {
                    string[] files = Directory.GetFiles(ApplicationValues.ConfigFolderPath);

                    if (files.Length > 0)
                    {
                        foreach (string filename in files)
                        {
                            FileInfo fInfo = new FileInfo(filename);
                            if (fInfo.Name == ApplicationValues.ConfigXMLFileName)
                            {
                                RecordsSet = new DataSet();
                                RecordsSet.ReadXml(filename);

                                ApplicationValues.ConfigFileValues = RecordsSet;

                                if (RecordsSet != null)
                                {
                                    DataTable dtRecords = new DataTable();
                                    dtRecords.Columns.Add("ConfigText");
                                    dtRecords.Columns.Add("ConfigNumber");
                                    DataRow RecordsRow = null;

                                    foreach (DataRow row in RecordsSet.Tables[0].Rows)
                                    {
                                        RecordsRow = dtRecords.NewRow();
                                        RecordsRow["ConfigText"] = "( " + row["CompanyName"].ToString() + " : " + row["FinancialYear"].ToString() + " ) ( Tally :" + row["VersionOftheTally"].ToString() + " )";
                                        RecordsRow["ConfigNumber"] = row["ConfigNumber"].ToString();
                                        dtRecords.Rows.Add(RecordsRow);
                                    }
                                    cmbTotalRecords.DisplayMember = "ConfigText";
                                    cmbTotalRecords.ValueMember = "ConfigNumber";
                                    cmbTotalRecords.DataSource = RecordsRow.Table.DefaultView;

                                    txtXMLPath.ReadOnly = true;
                                    btnNext.Enabled = true;
                                    btnPrevious.Enabled = true;
                                    btnFirstRecord.Enabled = true;
                                    btnLastRecord.Enabled = true;
                                    txtVisualUdyogPath.ReadOnly = true;
                                    txtVersionOftheTally.ReadOnly = true;
                                    txtSelectCompany.ReadOnly = true;
                                    txtFinancialYear.ReadOnly = true;
                                    btnDelete.Enabled = true;
                                    btnEdit.Enabled = true;
                                }
                                else
                                {
                                    cmbTotalRecords.Enabled = false;
                                    btnNext.Enabled = false;
                                    btnPrevious.Enabled = false;
                                    btnFirstRecord.Enabled = false;
                                    btnLastRecord.Enabled = false;
                                    txtXMLPath.ReadOnly = true;
                                    txtVisualUdyogPath.ReadOnly = true;
                                    txtVersionOftheTally.ReadOnly = true;
                                    txtSelectCompany.ReadOnly = true;
                                    txtFinancialYear.ReadOnly = true;
                                    btnDelete.Enabled = false;
                                    btnEdit.Enabled = false;
                                }

                            }
                            else
                            {
                                cmbTotalRecords.Enabled = false;
                                btnNext.Enabled = false;
                                btnPrevious.Enabled = false;
                                btnFirstRecord.Enabled = false;
                                btnLastRecord.Enabled = false;
                                txtXMLPath.ReadOnly = true;
                                txtVisualUdyogPath.ReadOnly = true;
                                txtVersionOftheTally.ReadOnly = true;
                                txtSelectCompany.ReadOnly = true;
                                txtFinancialYear.ReadOnly = true;
                                btnDelete.Enabled = false;
                                btnEdit.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        cmbTotalRecords.Enabled = false;
                        btnNext.Enabled = false;
                        btnPrevious.Enabled = false;
                        btnFirstRecord.Enabled = false;
                        btnLastRecord.Enabled = false;
                        txtXMLPath.ReadOnly = true;
                        txtVisualUdyogPath.ReadOnly = true;
                        txtVersionOftheTally.ReadOnly = true;
                        txtSelectCompany.ReadOnly = true;
                        txtFinancialYear.ReadOnly = true;
                        btnDelete.Enabled = false;
                        btnEdit.Enabled = false;
                    }
                }
                else
                {
                    Directory.CreateDirectory(ApplicationValues.ConfigFolderPath);
                    cmbTotalRecords.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnFirstRecord.Enabled = false;
                    btnLastRecord.Enabled = false;
                    txtXMLPath.ReadOnly = true;
                    txtVisualUdyogPath.ReadOnly = true;
                    txtVersionOftheTally.ReadOnly = true;
                    txtSelectCompany.ReadOnly = true;
                    txtFinancialYear.ReadOnly = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Logger.LogInfo(ex);
            }
        }
    }
}