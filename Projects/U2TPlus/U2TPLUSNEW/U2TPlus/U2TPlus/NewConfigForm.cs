using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;
using Udyog.lib;
using System.IO;

namespace U2TPlus
{
    public partial class NewConfigForm : Form
    {
        #region Local Veriables

        string DispalyButtonText = string.Empty;
        string Title = string.Empty;
        string ConfigNumber = string.Empty;
        CompanyInfo objCompanyInfo = new CompanyInfo();
        DataSet ds = new DataSet();

        #endregion

        #region Constractor
        public NewConfigForm(string TypeOfButton, string title, string confignumber)
        {
            DispalyButtonText = TypeOfButton;
            Title = title;
            ConfigNumber = confignumber;
            InitializeComponent();
        }
        #endregion

        #region Properties
        private string mServerName;
        public string ServerName
        {
            get { return mServerName; }
            set { mServerName = value; }
        }
        private string mUserID;
        public string UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }
        private string mPassword;
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }
        #endregion

        #region Form Event Functions

        #region Button Save Event
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text.ToLower() == "Save".ToLower())
                {
                    #region For Save CONFIGURATION  Data
                    string ConfigNumber = string.Empty;
                    bool fileEx = false;
                    if (txtXMLPath.Text != "" && txtXMLPath.Text != null)
                    {
                        if (txtVisualUdyogPath.Text != "" && txtVisualUdyogPath.Text != null)
                        {
                            if (cmbVersionOftheTally.SelectedValue != null && cmbVersionOftheTally.SelectedValue.ToString() != "Select Version")
                            {
                                if (cmbFinancialYear.SelectedValue != null)
                                {
                                    if (cmbSelectCompany.Text != null)
                                    {
                                        DataSet xmlread = new DataSet();
                                        if (File.Exists(ApplicationValues.ConfigFilePath))
                                        {
                                            xmlread.ReadXml(ApplicationValues.ConfigFilePath);

                                            if (xmlread.Tables[0].Rows.Count > 0)
                                            {
                                                ConfigNumber = xmlread.Tables[0].Rows[xmlread.Tables[0].Rows.Count - 1]["ConfigNumber"].ToString();
                                                ConfigNumber = (Convert.ToInt32(ConfigNumber) + 1).ToString();
                                                fileEx = true;
                                            }
                                        }
                                        else
                                        {
                                            ConfigNumber = "1";
                                            fileEx = false;
                                        }
                                        DataTable ConfigTable = new DataTable("NewConfig");
                                        ConfigTable.Columns.Add("ConfigNumber");
                                        ConfigTable.Columns.Add("XMLPath");
                                        ConfigTable.Columns.Add("VisualUdyogPath");
                                        ConfigTable.Columns.Add("VersionOftheTally");
                                        ConfigTable.Columns.Add("FinancialYear");
                                        ConfigTable.Columns.Add("CompanyName");
                                        ConfigTable.Columns.Add("CompanyID");
                                        ConfigTable.Columns.Add("ConnectionValues");
                                        ConfigTable.Columns.Add("CompanyValue");
                                        DataRow row = null;

                                        if (fileEx == true)
                                        {
                                            for (int i = 0; i < xmlread.Tables[0].Rows.Count; i++)
                                            {
                                                row = ConfigTable.NewRow();
                                                row["ConfigNumber"] = xmlread.Tables[0].Rows[i]["ConfigNumber"].ToString().Trim();
                                                row["XMLPath"] = xmlread.Tables[0].Rows[i]["XMLPath"].ToString().Trim();
                                                row["VisualUdyogPath"] = xmlread.Tables[0].Rows[i]["VisualUdyogPath"].ToString().Trim();
                                                row["VersionOftheTally"] = xmlread.Tables[0].Rows[i]["VersionOftheTally"].ToString().Trim();
                                                row["FinancialYear"] = xmlread.Tables[0].Rows[i]["FinancialYear"].ToString().Trim();
                                                row["CompanyName"] = xmlread.Tables[0].Rows[i]["CompanyName"].ToString().Trim();
                                                row["CompanyID"] = xmlread.Tables[0].Rows[i]["CompanyID"].ToString().Trim();
                                                row["ConnectionValues"] = xmlread.Tables[0].Rows[i]["ConnectionValues"].ToString().Trim();
                                                row["CompanyValue"] = xmlread.Tables[0].Rows[i]["CompanyValue"].ToString().Trim();
                                                ConfigTable.Rows.Add(row);
                                            }
                                            ApplicationValues.EncriptedSUPValues = ServerName + ":" + UserID + ":" + Password;
                                            row = ConfigTable.NewRow();
                                            row["ConfigNumber"] = ConfigNumber.Trim();
                                            row["XMLPath"] = txtXMLPath.Text.Trim();
                                            row["VisualUdyogPath"] = txtVisualUdyogPath.Text.Trim();
                                            row["VersionOftheTally"] = cmbVersionOftheTally.SelectedValue.ToString().Trim();
                                            row["FinancialYear"] = cmbFinancialYear.SelectedValue.ToString().Trim();
                                            row["CompanyName"] = cmbSelectCompany.Text.Trim();
                                            row["CompanyID"] = cmbSelectCompany.SelectedValue.ToString().Trim();
                                            row["ConnectionValues"] = ApplicationValues.EncriptedSUPValues.Trim();
                                            row["CompanyValue"] = ApplicationValues.Encript(txtCompanyDataBaseName.Text.Trim());
                                            ConfigTable.Rows.Add(row);

                                        }
                                        else
                                        {
                                            ApplicationValues.EncriptedSUPValues = ServerName + ":" + UserID + ":" + Password;

                                            row = ConfigTable.NewRow();
                                            row["ConfigNumber"] = ConfigNumber.Trim();
                                            row["XMLPath"] = txtXMLPath.Text.Trim();
                                            row["VisualUdyogPath"] = txtVisualUdyogPath.Text.Trim();
                                            row["VersionOftheTally"] = cmbVersionOftheTally.SelectedValue.ToString().Trim();
                                            row["FinancialYear"] = cmbFinancialYear.SelectedValue.ToString().Trim();
                                            row["CompanyName"] = cmbSelectCompany.Text.Trim();
                                            row["CompanyID"] = cmbSelectCompany.SelectedValue.ToString().Trim();
                                            row["ConnectionValues"] = ApplicationValues.EncriptedSUPValues.Trim();
                                            row["CompanyValue"] = ApplicationValues.Encript(txtCompanyDataBaseName.Text.Trim());
                                            ConfigTable.Rows.Add(row);
                                        }

                                        if (Directory.Exists(ApplicationValues.ConfigFolderPath))
                                        {
                                            ConfigTable.WriteXml(ApplicationValues.ConfigFilePath);
                                            if (DialogResult.OK == MessageBox.Show(this, "Saved Configuration!!", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                                            {                                                
                                                ApplicationValues.MainFormIsActiveted = true;
                                               
                                                this.Close();
                                            }                                            
                                        }
                                        else
                                        {
                                            Directory.CreateDirectory(ApplicationValues.ConfigFolderPath);
                                            ConfigTable.WriteXml(ApplicationValues.ConfigFilePath);
                                            if (DialogResult.OK == MessageBox.Show(this, "Saved Configuration!!", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                                            {
                                                DataSet xmlRead = new DataSet();
                                                xmlRead.ReadXml(ApplicationValues.ConfigFilePath);                                                
                                                ApplicationValues.MainFormIsActiveted = true;
                                               
                                                this.Close();
                                            }                                            
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("please select Company");
                                    }// 
                                }
                                else
                                {
                                    MessageBox.Show("please select Financial Year");
                                }//

                            }//
                            else
                            {
                                MessageBox.Show("please select Version of the Tally");
                            }
                        }
                        else
                        {
                            MessageBox.Show("please Add the Visuval Udyog Path");
                        }
                    }
                    else
                    {
                        MessageBox.Show("please Add the path of the XML");
                    }
                    #endregion
                }
                else if (btnSave.Text.ToLower() == "Update".ToLower())
                {
                    #region for Update CONFIGURATION  Data

                    for (int i = 0; i < ApplicationValues.ConfigFileValues.Tables[0].Rows.Count; i++)
                    {
                        if (ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["ConfigNumber"].ToString() == ConfigNumber)
                        {
                            ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["FinancialYear"] = cmbFinancialYear.Text.Trim();
                            ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["CompanyName"] = cmbSelectCompany.Text.Trim();
                            ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["VersionOftheTally"] = cmbVersionOftheTally.Text.Trim();
                            ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["VisualUdyogPath"] = txtVisualUdyogPath.Text.Trim();
                            ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["XMLPath"] = txtXMLPath.Text.Trim();
                            ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["CompanyValue"] = ApplicationValues.Encript(txtCompanyDataBaseName.Text);

                            ApplicationValues.ConfigFileValues.AcceptChanges();
                        }
                    }

                    if (Directory.Exists(ApplicationValues.ConfigFolderPath))
                    {
                        ApplicationValues.ConfigFileValues.WriteXml(ApplicationValues.ConfigFilePath);

                        if (DialogResult.OK == MessageBox.Show(this, "Configuration Updated!! ", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                        {                            
                            Logger.LogInfo("Configuration Updated!! ");
                            ApplicationValues.MainFormIsActiveted = true;
                           
                            this.Close();
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(ApplicationValues.ConfigFolderPath);
                        ApplicationValues.ConfigFileValues.WriteXml(ApplicationValues.ConfigFilePath);
                        if (DialogResult.OK == MessageBox.Show(this, "Configuration Updated!! ", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                        {                            
                            Logger.LogInfo("Configuration Updated!! ");
                            ApplicationValues.MainFormIsActiveted = true;
                           
                            this.Close();
                        }
                    }

                    ApplicationValues.CompanyConfigID = string.Empty;
                    ApplicationValues.CompanyDBName = string.Empty;
                    ApplicationValues.CompanyName = string.Empty;
                    ApplicationValues.GeneratCompanyXMLFilePath = string.Empty;
                    ApplicationValues.GeneratingProductPath = string.Empty;
                    ApplicationValues.TallyVersion = string.Empty;
                    ApplicationValues.IsEnvironmentSetup = false;

                    #endregion
                }
                else if (btnSave.Text.ToLower() == "Delete".ToLower())
                {
                    #region for Delete CONFIGURATION  Data
                    bool WriteXML = false;
                    if (MessageBox.Show(this, "Are You Sure You Want To Delete This Config File ??", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        string filter = "ConfigNumber=" + ConfigNumber;
                        DataRow[] rows = ApplicationValues.ConfigFileValues.Tables[0].Select(filter);

                        foreach (DataRow row in rows)
                        {
                            if (row["ConfigNumber"].ToString() == ConfigNumber)
                            {
                                ApplicationValues.ConfigFileValues.Tables[0].Rows.Remove(row);
                                ApplicationValues.ConfigFileValues.AcceptChanges();
                                if (ApplicationValues.ConfigFileValues.Tables[0].Rows.Count > 0)
                                {
                                    DataRow[] OrderConfigNumber = ApplicationValues.ConfigFileValues.Tables[0].Select("ConfigNumber >" + ConfigNumber);
                                    if ((OrderConfigNumber.Length == 0) && (ApplicationValues.ConfigFileValues.Tables[0].Rows.Count < 1))
                                    {
                                        FileInfo finfo = new FileInfo(ApplicationValues.ConfigFilePath);

                                        if (finfo.Exists)
                                        {
                                            finfo.Delete();
                                            ApplicationValues.ConfigFileValues = null;
                                            ApplicationValues.CompanyConfigID = string.Empty;
                                            ApplicationValues.CompanyDBName = string.Empty;
                                            ApplicationValues.CompanyName = string.Empty;
                                            ApplicationValues.GeneratCompanyXMLFilePath = string.Empty;
                                            ApplicationValues.GeneratingProductPath = string.Empty;
                                            ApplicationValues.TallyVersion = string.Empty;
                                            ApplicationValues.IsEnvironmentSetup = false;
                                            WriteXML = true;
                                        }
                                    }
                                    if (WriteXML == false)
                                    {
                                        for (int i = 0; i < ApplicationValues.ConfigFileValues.Tables[0].Rows.Count; i++)
                                        {
                                            if (Convert.ToInt32(ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["ConfigNumber"].ToString()) > Convert.ToInt32(ConfigNumber))
                                            {
                                                ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["ConfigNumber"] = Convert.ToInt32(ApplicationValues.ConfigFileValues.Tables[0].Rows[i]["ConfigNumber"]) - 1;
                                                ApplicationValues.ConfigFileValues.AcceptChanges();
                                            }
                                        }
                                    }
                                }
                                else if (ApplicationValues.ConfigFileValues.Tables[0].Rows.Count == 0)
                                {
                                    DataRow[] OrderConfigNumber = ApplicationValues.ConfigFileValues.Tables[0].Select("ConfigNumber >" + ConfigNumber);
                                    if (OrderConfigNumber.Length == 0)
                                    {
                                        FileInfo finfo = new FileInfo(ApplicationValues.ConfigFilePath);

                                        if (finfo.Exists)
                                        {
                                            finfo.Delete();
                                            ApplicationValues.ConfigFileValues = null;
                                            ApplicationValues.CompanyConfigID = string.Empty;
                                            ApplicationValues.CompanyDBName = string.Empty;
                                            ApplicationValues.CompanyName = string.Empty;
                                            ApplicationValues.GeneratCompanyXMLFilePath = string.Empty;
                                            ApplicationValues.GeneratingProductPath = string.Empty;
                                            ApplicationValues.TallyVersion = string.Empty;
                                            ApplicationValues.IsEnvironmentSetup = false;
                                            WriteXML = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (Directory.Exists(ApplicationValues.ConfigFolderPath) && (WriteXML == false))
                        {
                            ApplicationValues.ConfigFileValues.WriteXml(ApplicationValues.ConfigFilePath);

                            if (DialogResult.OK == MessageBox.Show(this, "Configuration Updated!! ", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                            {                                
                                Logger.LogInfo("Configuration Updated!! ");
                                ApplicationValues.MainFormIsActiveted = true;                               
                                this.Close();
                            }
                        }
                        else
                        {
                            if (WriteXML == false)
                            {
                                Directory.CreateDirectory(ApplicationValues.ConfigFolderPath);
                                ApplicationValues.ConfigFileValues.WriteXml(ApplicationValues.ConfigFilePath);
                                if (DialogResult.OK == MessageBox.Show(this, "Configuration Updated!! ", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                                {                                    
                                    Logger.LogInfo("Configuration Updated!! ");
                                    ApplicationValues.MainFormIsActiveted = true;                                   
                                    this.Close();
                                }
                            }
                            else
                            {
                                if (DialogResult.OK == MessageBox.Show(this, "Configuration Updated!! ", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                                {                                    
                                    Logger.LogInfo("Configuration Updated!! ");
                                    ApplicationValues.MainFormIsActiveted = true;                                   
                                    this.Close();
                                }
                            }
                        }
                        ApplicationValues.CompanyConfigID = string.Empty;
                        ApplicationValues.CompanyDBName = string.Empty;
                        ApplicationValues.CompanyName = string.Empty;
                        ApplicationValues.GeneratCompanyXMLFilePath = string.Empty;
                        ApplicationValues.GeneratingProductPath = string.Empty;
                        ApplicationValues.TallyVersion = string.Empty;
                        ApplicationValues.IsEnvironmentSetup = false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
        #endregion

        #region Close Button Event
        private void btnClose_Click(object sender, EventArgs e)
        {            
            ApplicationValues.MainFormIsActiveted = true;           
                this.Close();            
        }
        #endregion

        #region Button XML Path Event
        private void btnXMLPAth_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.Description = "Select Folder where save the XML file";
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                txtXMLPath.Text = folderBrowserDialog1.SelectedPath;
        }
        #endregion

        #region Button Visual Udyog -Product- Path Event
        private void btnVisualUdyogPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.Description = "Select Folder where Product install";
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                txtVisualUdyogPath.Text = folderBrowserDialog1.SelectedPath;
        }
        #endregion

        #region Thext Changed Event in the Visual Udyog Path text box
        private void txtVisualUdyogPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BindData(txtVisualUdyogPath.Text + @"\Visudyog.ini");
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
        #endregion

        #region Financial Year Dropdown Selected Index Changed Event
        private void cmbFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFinancialYear.SelectedIndex != -1)
                {
                    string filter = "FinancialYear='" + cmbFinancialYear.Text + "'";
                    ds = new DataSet();
                    ds = ApplicationValues.CompanyMaster;
                    DataView dv = new DataView();
                    dv = ds.Tables["CompanyMaster"].DefaultView;
                    dv.RowFilter = filter;
                    cmbSelectCompany.Enabled = true;
                    cmbSelectCompany.DisplayMember = "CompanyName";
                    cmbSelectCompany.ValueMember = "CompanyId";
                    cmbSelectCompany.DataSource = dv;
                    cmbSelectCompany.SelectedIndex = 0;
                }

                if (cmbSelectCompany.SelectedIndex != -1)
                {
                    string filter = "CompanyName='" + cmbSelectCompany.Text + "'";
                    ds = new DataSet();
                    ds = ApplicationValues.CompanyMaster;
                    DataRow[] rows = ds.Tables["CompanyMaster"].Select(filter);

                    foreach (DataRow row in rows)
                    {
                        txtCompanyDataBaseName.Text = row["DataBaseName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
        #endregion

        #region Select Company Dropdown Selected Index Changed Event
        private void cmbSelectCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSelectCompany.SelectedIndex != -1)
                {
                    string filter = "CompanyName='" + cmbSelectCompany.Text + "'";
                    ds = new DataSet();
                    ds = ApplicationValues.CompanyMaster;
                    DataRow[] rows = ds.Tables["CompanyMaster"].Select(filter);

                    foreach (DataRow row in rows)
                    {
                        txtCompanyDataBaseName.Text = row["DataBaseName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
        #endregion

        #region Form Load Event
        private void NewConfigForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            btnSave.Text = DispalyButtonText;

            if (btnSave.Text.ToLower() == "Save".ToLower())
            {
                try
                {
                    #region for Save Display
                    this.Text = Title;
                    bindTallyVersionDropDown();
                    #endregion
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("Error Accourd in Save Button Display " + ex.Message);
                }
            }
            else if (btnSave.Text.ToLower() == "Update".ToLower())
            {
                try
                {
                    #region for Update Display

                    this.Text = Title;
                    bindTallyVersionDropDown();
                    BindData(ApplicationValues.INIFilePath);

                    string filter = "ConfigNumber=" + ConfigNumber;
                    DataRow[] rows = ApplicationValues.ConfigFileValues.Tables[0].Select(filter);
                    foreach (DataRow dRow in rows)
                    {
                        txtVisualUdyogPath.Text = dRow["VisualUdyogPath"].ToString();
                        txtXMLPath.Text = dRow["XMLPath"].ToString();
                        cmbVersionOftheTally.Text = dRow["VersionOftheTally"].ToString();
                        cmbFinancialYear.SelectedValue = dRow["FinancialYear"].ToString();
                        cmbSelectCompany.SelectedValue = dRow["CompanyID"].ToString();
                        txtCompanyDataBaseName.Text = ApplicationValues.Decript(dRow["CompanyValue"].ToString());
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("Error Acourd in Update Button " + ex.Message);
                }
            }
            else if (btnSave.Text.ToLower() == "Delete".ToLower())
            {
                try
                {
                    #region for Delete Display

                    this.Text = Title;
                    bindTallyVersionDropDown();
                    BindData(ApplicationValues.INIFilePath);
                    string filter = "ConfigNumber=" + ConfigNumber;
                    DataRow[] rows = ApplicationValues.ConfigFileValues.Tables[0].Select(filter);
                    foreach (DataRow dRow in rows)
                    {
                        txtVisualUdyogPath.Text = dRow["VisualUdyogPath"].ToString();
                        txtXMLPath.Text = dRow["XMLPath"].ToString();
                        cmbVersionOftheTally.Text = dRow["VersionOftheTally"].ToString();
                        cmbFinancialYear.SelectedValue = dRow["FinancialYear"].ToString();
                        cmbSelectCompany.SelectedValue = dRow["CompanyID"].ToString();
                        txtCompanyDataBaseName.Text = ApplicationValues.Decript(dRow["CompanyValue"].ToString());
                    }

                    btnVisualUdyogPath.Visible = false;
                    btnXMLPAth.Visible = false;
                    txtVisualUdyogPath.Enabled = false;
                    txtXMLPath.Enabled = false;
                    cmbVersionOftheTally.Enabled = false;
                    cmbFinancialYear.Enabled = false;
                    cmbSelectCompany.Enabled = false;

                    #endregion
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("Error Accoured in the Delete Button " + ex.Message);
                }
            }
        }
        #endregion

        #endregion

        #region Private Methods
        private void bindTallyVersionDropDown()
        {
            try
            {
                DataRow r = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("Versions");
                r = dt.NewRow();
                r["Versions"] = "Select Version";
                dt.Rows.Add(r);
                r = dt.NewRow();
                r["Versions"] = "7.2";
                dt.Rows.Add(r);
                r = dt.NewRow();
                r["Versions"] = "8.1";
                dt.Rows.Add(r);
                r = dt.NewRow();
                r["Versions"] = "9.0";
                dt.Rows.Add(r);
                cmbVersionOftheTally.DataSource = dt.DefaultView;
                cmbVersionOftheTally.DisplayMember = "Versions";
                cmbVersionOftheTally.ValueMember = "Versions";
                cmbVersionOftheTally.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void BindData(string INIFilePath)
        {
            try
            {
                string INIPath = INIFilePath;
                ApplicationValues.INIFilePath = INIPath;
                Udyog.lib.INIAccess INIAccessCls = new INIAccess();
                int returnValue;
                if (File.Exists(INIPath))
                {
                    returnValue = INIAccessCls.LoadUdyogINI(ApplicationValues.INIFilePath);

                    if (returnValue > 0)
                    {

                        ServerName = INIAccessCls.Server_Name;// @"DESKTOP124\SQLEXPRESS";
                        ApplicationValues.DBServerName = ServerName;
                        UserID = INIAccessCls.Sql_User_Id;// "sa";
                        ApplicationValues.DBUserID = UserID;
                        Password = INIAccessCls.Sql_User_Pwd;// "sa1985";
                        ApplicationValues.DBPassword = Password;

                        string conn = "Data Source=" + ServerName + ";Initial Catalog=Vudyog;Persist Security Info=True;User ID=" + UserID + ";Password=" + Password;
                        ApplicationValues.vUdyogDBConnectionString = conn;

                        try
                        {
                            DataSet CompanyMast = new DataSet();
                            objCompanyInfo.CompanyMasterConnectionString = ApplicationValues.vUdyogDBConnectionString;
                            CompanyMast = objCompanyInfo.CompanyMasterData();
                            ApplicationValues.CompanyMaster = CompanyMast;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Logger.LogInfo(ex);
                        }

                        try
                        {
                            objCompanyInfo.CompanyMasterConnectionString = ApplicationValues.vUdyogDBConnectionString;
                            ds = objCompanyInfo.MasterCompanyFinancealYears();

                            if (ds.Tables.Count > 0)
                            {
                                cmbFinancialYear.Enabled = true;
                                cmbFinancialYear.DisplayMember = "FinancialYear";
                                cmbFinancialYear.ValueMember = "FinancialYear";
                                cmbFinancialYear.DataSource = ds.Tables["FinancealYears"];
                                cmbFinancialYear.SelectedIndex = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Logger.LogInfo(ex);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please Verify the Path of the Application", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    Logger.LogInfo("Please Verify the Path of the Application");
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }
        #endregion
    }
}