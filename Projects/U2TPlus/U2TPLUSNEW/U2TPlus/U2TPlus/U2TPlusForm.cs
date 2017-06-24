using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;
using System.IO;
using System.Configuration;

namespace U2TPlus
{
    public partial class U2TPlusForm : Form
    {
        DataSet RecordsSet = new DataSet();
        private string mRecordSelectedValue = string.Empty;
        public string RecordSelectedValue
        {
            get { return mRecordSelectedValue; }
            set { mRecordSelectedValue = value; }
        }

        public U2TPlusForm()
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
            ApplicationValues.DataMappingFolderName = ApplicationValues.ApplicationPath + @"\DataMapping";
            ApplicationValues.DataMappingFileName = "DataMapping.xml";

            string AccountSwapping = ConfigurationManager.AppSettings["AccountSwapping"].ToString();
            ApplicationValues.AccountSwappingDataTables = AccountSwapping.Split(new Char[] { ',' });
            string AccountGroupSwapping = ConfigurationManager.AppSettings["AccountGroupSwapping"].ToString();
            ApplicationValues.AccountGroupSwappingDataTables = AccountGroupSwapping.Split(new Char[] { ',' });

            #endregion
        }

        #region Page Events

        private void U2TPlusForm_Load(object sender, EventArgs e)
        {
            btnAdd.Focus();

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
                                if (dataMappingToolStripMenuItem.DropDown.Items.Count == 0)
                                {
                                    dataMappingToolStripMenuItem.DropDown.Items.Add("Accounts", null, this.ContextMenuStripClickSettings_DataMapping);
                                    dataMappingToolStripMenuItem.DropDown.Items.Add("Accounts Group", null, this.ContextMenuStripClickSettings_DataMapping);
                                }
                                if (RecordSelectedValue == string.Empty)
                                {
                                    cmbTotalRecords.SelectedIndex = 0;
                                    if (cmbTotalRecords.Items.Count == 1)
                                    {
                                        btnNext.Enabled = false;
                                        btnPrevious.Enabled = false;
                                        btnFirstRecord.Enabled = false;
                                        btnLastRecord.Enabled = false;
                                    }
                                    else
                                    {
                                        btnNext.Enabled = true;
                                        btnPrevious.Enabled = false;
                                        btnFirstRecord.Enabled = false;
                                        btnLastRecord.Enabled = true;
                                    }
                                }
                                else
                                {

                                    if (RecordSelectedValue == "1")
                                    {
                                        cmbTotalRecords.SelectedValue = RecordSelectedValue;
                                        if (cmbTotalRecords.Items.Count == 1)
                                        {
                                            btnNext.Enabled = false;
                                            btnPrevious.Enabled = false;
                                            btnFirstRecord.Enabled = false;
                                            btnLastRecord.Enabled = false;
                                        }
                                        else
                                        {
                                            btnNext.Enabled = true;
                                            btnPrevious.Enabled = false;
                                            btnFirstRecord.Enabled = false;
                                            btnLastRecord.Enabled = true;
                                        }
                                    }
                                    else if (RecordSelectedValue == Convert.ToString(cmbTotalRecords.Items.Count))
                                    {
                                        btnNext.Enabled = false;
                                        btnPrevious.Enabled = true;
                                        btnFirstRecord.Enabled = true;
                                        btnLastRecord.Enabled = false;
                                        cmbTotalRecords.SelectedValue = RecordSelectedValue;
                                    }
                                    else
                                    {
                                        btnNext.Enabled = true;
                                        btnPrevious.Enabled = true;
                                        btnFirstRecord.Enabled = true;
                                        btnLastRecord.Enabled = true;
                                        cmbTotalRecords.SelectedValue = RecordSelectedValue;
                                    }
                                }

                                if (ApplicationValues.MainFormIsActiveted)
                                {
                                    cmbTotalRecords.Enabled = true;

                                    if (settingsToolStripMenuItem.DropDown.Items.Count > 0 && generateToolStripMenuItem.DropDown.Items.Count > 0)
                                    {
                                        setEnvironment();
                                    }
                                    else
                                    {
                                        setEnvironment();
                                        FillSettingsAndGenerateMenuItems();
                                    }
                                }
                                else
                                {
                                    setEnvironment();
                                    FillSettingsAndGenerateMenuItems();

                                }
                                txtXMLPath.ReadOnly = true;
                                txtVisualUdyogPath.ReadOnly = true;
                                txtVersionOftheTally.ReadOnly = true;
                                txtSelectCompany.ReadOnly = true;
                                txtFinancialYear.ReadOnly = true;
                                btnDelete.Enabled = true;
                                btnEdit.Enabled = true;
                                setEnvironment();
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

                }
                else
                {

                    if (ApplicationValues.ConfigFileValues != null)
                    {
                        DataTable dtRecords = new DataTable();
                        dtRecords.Columns.Add("ConfigText");
                        dtRecords.Columns.Add("ConfigNumber");
                        DataRow RecordsRow = null;

                        foreach (DataRow row in ApplicationValues.ConfigFileValues.Tables[0].Rows)
                        {
                            RecordsRow = dtRecords.NewRow();
                            RecordsRow["ConfigText"] = "( " + row["CompanyName"].ToString() + " : " + row["FinancialYear"].ToString() + " ) ( Tally :" + row["VersionOftheTally"].ToString() + " )";
                            RecordsRow["ConfigNumber"] = row["ConfigNumber"].ToString();
                            dtRecords.Rows.Add(RecordsRow);
                        }
                        cmbTotalRecords.DisplayMember = "ConfigText";
                        cmbTotalRecords.ValueMember = "ConfigNumber";
                        cmbTotalRecords.DataSource = RecordsRow.Table.DefaultView;
                    }
                    else
                    {
                        cmbTotalRecords.DataSource = null;
                    }

                    cmbTotalRecords.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnFirstRecord.Enabled = false;
                    btnLastRecord.Enabled = false;
                    txtXMLPath.ReadOnly = true;
                    txtXMLPath.Text = "";
                    txtVisualUdyogPath.ReadOnly = true;
                    txtVisualUdyogPath.Text = "";
                    txtVersionOftheTally.ReadOnly = true;
                    txtVersionOftheTally.Text = "";
                    txtSelectCompany.ReadOnly = true;
                    txtSelectCompany.Text = "";
                    txtFinancialYear.ReadOnly = true;
                    txtFinancialYear.Text = "";
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    settingsToolStripMenuItem.Enabled = false;
                    generateToolStripMenuItem.Enabled = false;
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
                settingsToolStripMenuItem.Enabled = false;
                generateToolStripMenuItem.Enabled = false;
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm Cpwd = new ChangePasswordForm();
            Cpwd.ShowDialog(this);
            ApplicationValues.MainFormIsActiveted = false;
            if (cmbTotalRecords.Items.Count > 0)
            {
                RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
            }
            else
            {
                RecordSelectedValue = string.Empty;
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(ApplicationValues.ApplicationPath + @"\Log\U2TPlus_Logger.txt");
            ApplicationValues.MainFormIsActiveted = true;
            if (cmbTotalRecords.Items.Count > 0)
            {
                RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
            }
            else
            {
                RecordSelectedValue = string.Empty;
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm hf = new HelpForm();
            hf.ShowDialog(this);
            ApplicationValues.MainFormIsActiveted = false;
            if (cmbTotalRecords.Items.Count > 0)
            {
                RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
            }
            else
            {
                RecordSelectedValue = string.Empty;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog(this);
            ApplicationValues.MainFormIsActiveted = false;
            if (cmbTotalRecords.Items.Count > 0)
            {
                RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
            }
            else
            {
                RecordSelectedValue = string.Empty;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            if (DR == DialogResult.Yes)
                Application.Exit();
        }

        private void btnFirstRecord_Click(object sender, EventArgs e)
        {
            cmbTotalRecords.SelectedIndex = 0;
            btnNext.Enabled = true;
            btnPrevious.Enabled = false;
            btnFirstRecord.Enabled = false;
            btnLastRecord.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int totalRecords = cmbTotalRecords.Items.Count;
            int selectdRecordIndex = cmbTotalRecords.SelectedIndex;
            if (selectdRecordIndex == -1)
            {
                btnNext.Enabled = true;
                btnPrevious.Enabled = false;
                btnFirstRecord.Enabled = false;
                btnLastRecord.Enabled = true;
                cmbTotalRecords.SelectedIndex = selectdRecordIndex + 1;
                selectdRecordIndex = cmbTotalRecords.SelectedIndex;
            }
            else if (selectdRecordIndex == 0)
            {
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                btnFirstRecord.Enabled = true;
                btnLastRecord.Enabled = true;
                cmbTotalRecords.SelectedIndex = selectdRecordIndex + 1;
                selectdRecordIndex = cmbTotalRecords.SelectedIndex;
            }
            else if (totalRecords > selectdRecordIndex)
            {
                if (totalRecords == selectdRecordIndex + 1)
                {
                    cmbTotalRecords.SelectedIndex = selectdRecordIndex;
                    selectdRecordIndex = cmbTotalRecords.SelectedIndex;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = true;
                    btnFirstRecord.Enabled = true;
                    btnLastRecord.Enabled = false;
                }
                else
                {
                    cmbTotalRecords.SelectedIndex = selectdRecordIndex + 1;
                    selectdRecordIndex = cmbTotalRecords.SelectedIndex;
                    if (totalRecords == selectdRecordIndex + 1)
                    {
                        btnNext.Enabled = false;
                        btnPrevious.Enabled = true;
                        btnFirstRecord.Enabled = true;
                        btnLastRecord.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                        btnPrevious.Enabled = true;
                        btnFirstRecord.Enabled = true;
                        btnLastRecord.Enabled = true;
                    }
                }
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int totalRecords = cmbTotalRecords.Items.Count;
            int selectdRecordIndex = cmbTotalRecords.SelectedIndex;

            if (selectdRecordIndex == -1)
            {
                btnPrevious.Enabled = false;
                btnFirstRecord.Enabled = false;
                btnNext.Enabled = true;
                btnLastRecord.Enabled = true;
            }
            else if (selectdRecordIndex == 0)
            {
                cmbTotalRecords.SelectedIndex = selectdRecordIndex - 1;
                selectdRecordIndex = cmbTotalRecords.SelectedIndex;
                if (selectdRecordIndex + totalRecords == totalRecords)
                {
                    btnPrevious.Enabled = false;
                    btnFirstRecord.Enabled = false;
                    btnNext.Enabled = true;
                    btnLastRecord.Enabled = true;
                }
            }
            else if (selectdRecordIndex < totalRecords)
            {
                cmbTotalRecords.SelectedIndex = selectdRecordIndex - 1;
                selectdRecordIndex = cmbTotalRecords.SelectedIndex;
                if (selectdRecordIndex + totalRecords == totalRecords)
                {
                    btnPrevious.Enabled = false;
                    btnFirstRecord.Enabled = false;
                    btnNext.Enabled = true;
                    btnLastRecord.Enabled = true;
                }
                else
                {
                    btnPrevious.Enabled = true;
                    btnFirstRecord.Enabled = true;
                    btnNext.Enabled = true;
                    btnLastRecord.Enabled = true;
                }
            }
        }

        private void btnLastRecord_Click(object sender, EventArgs e)
        {
            int totalRecords = cmbTotalRecords.Items.Count;
            cmbTotalRecords.SelectedIndex = totalRecords - 1;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirstRecord.Enabled = true;
            btnLastRecord.Enabled = false;
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
                        setEnvironment();
                        settingsToolStripMenuItem.Enabled = true;
                        generateToolStripMenuItem.Enabled = true;
                        cmbTotalRecords.Enabled = true;

                        if (cmbTotalRecords.Items.Count == 0)
                        {
                            cmbTotalRecords.SelectedIndex = 0;
                            if (cmbTotalRecords.Items.Count == 1)
                            {
                                btnNext.Enabled = false;
                                btnPrevious.Enabled = false;
                                btnFirstRecord.Enabled = false;
                                btnLastRecord.Enabled = false;
                            }
                            else
                            {
                                btnNext.Enabled = true;
                                btnPrevious.Enabled = false;
                                btnFirstRecord.Enabled = false;
                                btnLastRecord.Enabled = true;
                            }
                        }
                        else
                        {
                            if (cmbTotalRecords.SelectedValue.ToString() == "1")
                            {
                                btnNext.Enabled = true;
                                btnPrevious.Enabled = false;
                                btnFirstRecord.Enabled = false;
                                btnLastRecord.Enabled = true;
                            }
                            else if (cmbTotalRecords.SelectedValue.ToString() == Convert.ToString(cmbTotalRecords.Items.Count))
                            {
                                btnNext.Enabled = false;
                                btnPrevious.Enabled = true;
                                btnFirstRecord.Enabled = true;
                                btnLastRecord.Enabled = false;
                            }
                            else
                            {
                                btnNext.Enabled = true;
                                btnPrevious.Enabled = true;
                                btnFirstRecord.Enabled = true;
                                btnLastRecord.Enabled = true;
                            }
                        }
                    }

                }
                else if (cmbTotalRecords.SelectedIndex == -1)
                {
                    txtFinancialYear.Text = "";
                    txtSelectCompany.Text = "";
                    txtVersionOftheTally.Text = "";
                    txtVisualUdyogPath.Text = "";
                    txtXMLPath.Text = "";
                    txtCompanyDataBaseName.Text = "";
                    txtCompanyConfigID.Text = "";

                    settingsToolStripMenuItem.Enabled = false;
                    generateToolStripMenuItem.Enabled = false;
                    cmbTotalRecords.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NewConfigForm ncf = new NewConfigForm("Save", "Adding New Configuration", string.Empty);
            ncf.Show(this);
            ApplicationValues.MainFormIsActiveted = false;
            if (cmbTotalRecords.Items.Count > 0)
            {
                RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
            }
            else
            {
                RecordSelectedValue = string.Empty;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cmbTotalRecords.Items.Count != 0)
            {
                NewConfigForm ncf = new NewConfigForm("Update", "Edit Configuration", cmbTotalRecords.SelectedValue.ToString());
                ncf.Show(this);
            }
            ApplicationValues.MainFormIsActiveted = false;
            if (cmbTotalRecords.Items.Count > 0)
            {
                RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
            }
            else
            {
                RecordSelectedValue = string.Empty;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTotalRecords.Items.Count != 0)
            {
                if (cmbTotalRecords.Items.Count > 1)
                {
                    NewConfigForm ncf = new NewConfigForm("Delete", "Delete Configuration", cmbTotalRecords.SelectedValue.ToString());
                    ncf.Show(this);
                }
                else
                {
                    if (DialogResult.OK == MessageBox.Show(this, "There is Only One Record will be available, are you sure you want to delete the record???", "U2T Plus - Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        NewConfigForm ncf = new NewConfigForm("Delete", "Delete Configuration", cmbTotalRecords.SelectedValue.ToString());
                        ncf.Show(this);
                    }
                }

            }
            ApplicationValues.MainFormIsActiveted = false;
            if (cmbTotalRecords.Items.Count > 0)
            {
                if (cmbTotalRecords.Items.Count.ToString() != cmbTotalRecords.SelectedValue.ToString())
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
            }
            else
            {
                RecordSelectedValue = string.Empty;
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
                ApplicationValues.MainFormIsActiveted = false;
                if (cmbTotalRecords.Items.Count > 0)
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
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
                ApplicationValues.MainFormIsActiveted = false;
                if (cmbTotalRecords.Items.Count > 0)
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
        //                     ContextMenuStripClickGenerate_DataMapping
        private void ContextMenuStripClickSettings_DataMapping(object sender, EventArgs e)
        {
            try
            {
                string MenuItemName = string.Empty;
                string MasterOrTran = string.Empty;
                ToolStripMenuItem ctmenu = (ToolStripMenuItem)sender;
                DataMappingForm DMF = new DataMappingForm(ctmenu.Text, ctmenu.OwnerItem.Text);
                DMF.Show(this);
                ApplicationValues.MainFormIsActiveted = false;
                if (cmbTotalRecords.Items.Count > 0)
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void ContextMenuStripClickGenerate_DataMapping(object sender, EventArgs e)
        {
            try
            {
                string MenuItemName = string.Empty;
                string MasterOrTran = string.Empty;
                ToolStripMenuItem ctmenu = (ToolStripMenuItem)sender;
                DataMappingForm DMF = new DataMappingForm(ctmenu.Text, ctmenu.OwnerItem.Text);
                DMF.Show(this);
                ApplicationValues.MainFormIsActiveted = false;
                if (cmbTotalRecords.Items.Count > 0)
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
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
                ApplicationValues.MainFormIsActiveted = false;
                if (cmbTotalRecords.Items.Count > 0)
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
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
                ApplicationValues.MainFormIsActiveted = false;
                if (cmbTotalRecords.Items.Count > 0)
                {
                    RecordSelectedValue = cmbTotalRecords.SelectedValue.ToString();
                }
                else
                {
                    RecordSelectedValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void U2TPlusForm_Activated(object sender, EventArgs e)
        {
            if (ApplicationValues.MainFormIsActiveted)
            {
                btnAdd.Focus();
                U2TPlusForm_Load(null, null);
            }
        }

        #endregion

        #region PRivate Mothods

        //to fill Settings and generate menu items

        public void FillSettingsAndGenerateMenuItems()
        {
            DataSet CompanyMasterTableNames = new DataSet();
            DataSet CompanyTransTableNames = new DataSet();
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
                ApplicationValues.INIFilePath = txtVisualUdyogPath.Text + @"\Visudyog.ini";
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

        #endregion




    }
}