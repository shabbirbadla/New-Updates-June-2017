using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using U2TPlus.BAL;

namespace U2TPlus
{
    public partial class SettingsForm : Form
    {
        #region Globle Veriable of the class

        string Title = string.Empty;
        string TableDetails = string.Empty;
        U2TPlus.BAL.CommonFunctions CFunctions = new U2TPlus.BAL.CommonFunctions();
        string getGridMode = string.Empty;
        bool ClearCellValue = false;

        #endregion

        #region Constractor

        public SettingsForm(string title, string materOrTranDetails)
        {
            Title = title;
            TableDetails = materOrTranDetails;
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            //contextMenuStrip1.Dispose();
            try
            {
                this.Text = Title + " - " + "Setting";
                string MasterID = string.Empty;
                string configuration_id = string.Empty;
                int GridIndex = 0;
                bool DataFiles = false;
                try
                {
                    if (Directory.Exists(ApplicationValues.ConfigFolderPath))
                    {
                        if (Directory.Exists(ApplicationValues.U2TMarkupsFolderPath))
                        {
                            string[] files = Directory.GetFiles(ApplicationValues.U2TMarkupsFolderPath);
                            foreach (string file in files)
                            {
                                FileInfo fInfo = new FileInfo(file);
                                if (fInfo.Name == this.Title.ToUpper().Trim() + "_MASTER.xml")
                                {
                                    DataSet DS = new DataSet();
                                    DataFiles = true;
                                    DS.ReadXml(file);
                                    foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                    {
                                        if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
                                            MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                    }

                                    if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                    {
                                        foreach (DataRow row in DS.Tables["CONFIGURATION"].Rows)
                                        {
                                            if ((row["NAME"].ToString().ToUpper().Trim() == ApplicationValues.CompanyName.ToUpper().Trim()) && (row["VALUE"].ToString().Trim() == ApplicationValues.CompanyConfigID.Trim()))
                                                configuration_id = row["CONFIGURATION_id"].ToString();
                                        }
                                        if ((configuration_id != null) && (configuration_id != string.Empty) && (configuration_id != ""))
                                        {
                                            foreach (DataRow TallyTagRow in DS.Tables["TALLYTAG"].Rows)
                                            {
                                                if (TallyTagRow["CONFIGURATION_id"].ToString() == configuration_id)
                                                {
                                                    AddGridExistingTags(GridIndex, TallyTagRow);
                                                    GridIndex = GridIndex + 1;
                                                }

                                            }//for 

                                            contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
                                            btnSave.Enabled = true;
                                        }//if
                                        else
                                        {
                                            //group is exits but company id is not there in the form title master

                                            if (File.Exists(ApplicationValues.DefaultMasterSettingsXMLName))
                                            {
                                                DS = new DataSet();
                                                DataFiles = true;
                                                DS.ReadXml(ApplicationValues.DefaultMasterSettingsXMLName);
                                                foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                                {
                                                    if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
                                                        MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                                }

                                                if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                                {
                                                    foreach (DataRow TallyTagRow in DS.Tables["TALLYTAG"].Rows)
                                                    {
                                                        if (TallyTagRow["MASTERTYPE_id"].ToString() == MasterID)
                                                        {
                                                            AddGridExistingTags(GridIndex, TallyTagRow);
                                                            GridIndex = GridIndex + 1;
                                                        }

                                                    }//for 

                                                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
                                                    btnSave.Enabled = true;
                                                }
                                                else
                                                {
                                                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                                    btnSave.Enabled = false;

                                                    MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                                    Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                                                }

                                            }
                                            else
                                            {
                                                contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                                btnSave.Enabled = false;

                                                MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                                Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                                            }
                                        }
                                    }//if
                                    break;
                                }//try to load the default xml tags based on the title of the form with company
                                //if title of the form with company is not availbale need to show the default tags in the form
                                else if (fInfo.FullName == ApplicationValues.DefaultMasterSettingsXMLName)
                                {
                                    DataSet DS = new DataSet();
                                    DataFiles = true;
                                    DS.ReadXml(file);
                                    foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                    {
                                        if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
                                            MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                    }

                                    if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                    {
                                        foreach (DataRow TallyTagRow in DS.Tables["TALLYTAG"].Rows)
                                        {
                                            if (TallyTagRow["MASTERTYPE_id"].ToString() == MasterID)
                                            {
                                                AddGridExistingTags(GridIndex, TallyTagRow);
                                                GridIndex = GridIndex + 1;
                                            }

                                        }//for 

                                        contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
                                        btnSave.Enabled = true;
                                    }
                                    else
                                    {
                                        contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                        btnSave.Enabled = false;

                                        MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                        Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                                    }
                                    break;
                                }
                            }

                            if (DataFiles == false)
                            {
                                contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                btnSave.Enabled = false;

                                MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                            }
                        }//markups folder exits is true
                        else //markups folder exits is false
                        {
                            Directory.CreateDirectory(ApplicationValues.U2TMarkupsFolderPath);
                            contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                            btnSave.Enabled = false;

                            MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please Set the Environment first", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Logger.LogInfo("Please Set the Environment first");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    Logger.LogInfo(ex);
                }

                if (SettingsdataGridView.RowCount > 0)
                {
                    //btnAdd.Text = "Edit";
                    getGridMode = "Edit";
                    SettingsdataGridView.ReadOnly = true;
                    btnSave.Enabled = false;
                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    //btnAdd.Text = "Add";
                    getGridMode = "Add";
                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
                    SettingsdataGridView.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnAddNewTag_Click(object sender, EventArgs e)
        {
            try
            {
                //if (//btnAdd.Text == "Edit")
                //{
                //    SettingsdataGridView.ReadOnly = false;
                //    SettingsdataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                //    //btnDelete.Enabled = true;
                //    btnSave.Enabled = true;
                //    //btnAdd.Text = "Add";
                //}
                //else
                //{
                if (SettingsdataGridView.Rows.Count == 0)
                {
                    SettingsdataGridView.Rows.Add();
                    SettingsdataGridView.Rows[0].Cells["Order"].Value = "1";
                    SettingsdataGridView.Rows[0].Cells["ISActive"].Value = "True";
                    SettingsdataGridView.Rows[0].Cells["IsDefault"].Value = "False";
                    btnSave.Enabled = true;
                    //btnDelete.Enabled = true;
                }
                else
                {
                    int LastIndex = SettingsdataGridView.Rows.Count - 1;
                    if ((SettingsdataGridView.Rows[LastIndex].Cells["IsActive"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["XMLOpeningTag"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["DefaultValues"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["FieldValue"].Value != null))
                    {
                        if (((SettingsdataGridView.Rows[LastIndex].Cells["IsActive"].Value.ToString()).ToLower() != "False".ToLower()) || (SettingsdataGridView.Rows[LastIndex].Cells["XMLOpeningTag"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["DefaultValues"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["FieldValue"].Value != null))
                        {
                            SettingsdataGridView.Rows.Add();
                            int intIndex = SettingsdataGridView.Rows.Count - 1;
                            SettingsdataGridView.Rows[intIndex].Cells["Order"].Value = Convert.ToInt32(SettingsdataGridView.Rows[intIndex - 1].Cells["Order"].Value) + 1;
                            SettingsdataGridView.Rows[intIndex].Cells["ISActive"].Value = "True";
                            SettingsdataGridView.Rows[intIndex].Cells["IsDefault"].Value = "False";
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show(this, "New Row Is Already Exits in Order No: " + (LastIndex + 1), "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            SettingsdataGridView.Rows[LastIndex].Cells[0].Selected = true;
                            Logger.LogInfo("New Row Is Already Exits in Order No: " + (LastIndex + 1));
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "New Row Is Already Exits, Enter the Values", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Logger.LogInfo("New Row Is Already Exits, Enter the Values");
                    }
                }
                //}//
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (SettingsdataGridView.RowCount > 0)
                {
                    if (SettingsdataGridView.SelectedRows.Count > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show(this, "Are You Sure You Need to Delete??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            foreach (DataGridViewRow row in SettingsdataGridView.SelectedRows)
                            {
                                SettingsdataGridView.Rows.Remove(row);
                            }
                            #region Deleting All the Rows In the Grid
                            if (SettingsdataGridView.RowCount == 0)
                            {
                                string MasterID = string.Empty;
                                string configuration_id = string.Empty;
                                if (Directory.Exists(ApplicationValues.U2TMarkupsFolderPath))
                                {
                                    FileInfo fInfo = new FileInfo(ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml");
                                    if (fInfo.Exists)
                                    {
                                        if (fInfo.Name == this.Title.ToUpper().Trim() + "_MASTER.xml")
                                        {
                                            DataSet DS = new DataSet();
                                            DS.ReadXml(fInfo.FullName);
                                            foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                            {
                                                if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
                                                    MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                            }

                                            if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                            {

                                                foreach (DataRow row in DS.Tables["CONFIGURATION"].Rows)
                                                {
                                                    if ((row["NAME"].ToString().ToUpper().Trim() == ApplicationValues.CompanyName.ToUpper().Trim()) && (row["VALUE"].ToString().Trim() == ApplicationValues.CompanyConfigID.Trim()))
                                                        configuration_id = row["CONFIGURATION_id"].ToString();
                                                }
                                                if ((configuration_id != null) && (configuration_id != string.Empty) && (configuration_id != ""))
                                                {
                                                    string filter = "CONFIGURATION_id=" + configuration_id;
                                                    DataRow[] rows = DS.Tables["TALLYTAG"].Select(filter);
                                                    foreach (DataRow TallyTagRow in rows)
                                                    {
                                                        DS.Tables["TALLYTAG"].Rows.Remove(TallyTagRow);
                                                        DS.AcceptChanges();
                                                    }

                                                    string CONFIGURATIONfilter = "CONFIGURATION_id=" + configuration_id;
                                                    DataRow[] CONFIGURATIONrows = DS.Tables["CONFIGURATION"].Select(CONFIGURATIONfilter);
                                                    foreach (DataRow row in CONFIGURATIONrows)
                                                    {
                                                        DS.Tables["CONFIGURATION"].Rows.Remove(row);
                                                        DS.AcceptChanges();
                                                    }

                                                    if (DS.Tables["TALLYTAG"].Rows.Count == 0 && DS.Tables["CONFIGURATION"].Rows.Count == 0)
                                                    {
                                                        string MASTERTYPEfilter = "MASTERTYPE_id=" + MasterID;
                                                        DataRow[] MASTERTYPErows = DS.Tables["MASTERTYPE"].Select(MASTERTYPEfilter);
                                                        foreach (DataRow MASTERTYPERow in MASTERTYPErows)
                                                        {
                                                            DS.Tables["MASTERTYPE"].Rows.Remove(MASTERTYPERow);
                                                            DS.AcceptChanges();
                                                        }

                                                        if (DS.Tables["MASTERTYPE"].Rows.Count == 0)
                                                        {
                                                            if (fInfo.Exists)
                                                            {
                                                                fInfo.Delete();
                                                                //btnDelete.Enabled = false;
                                                                btnSave.Enabled = false;
                                                                //btnAdd.Text = "Add";
                                                                //contextMenuStrip1.Dispose();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //XmlDocument xmlDoc = new XmlDocument();
                                                            DS.WriteXml(fInfo.FullName);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DS.WriteXml(fInfo.FullName);
                                                        //btnDelete.Enabled = false;
                                                        btnSave.Enabled = false;
                                                        //btnAdd.Text = "Add";
                                                        //contextMenuStrip1.Dispose();
                                                    }
                                                }//
                                            }
                                        }
                                    }
                                }
                            }// 
                            #endregion
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "please select at least one row", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Logger.LogInfo("please select at least one row");
                    }
                }
                else
                {
                    //btnDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ApplicationValues.MainFormIsActiveted = true;
           
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string MasterType = string.Empty;
                if (SettingsdataGridView.Rows.Count > 0)
                {
                    StringBuilder strSettingsMarkupXML = new StringBuilder();
                    StringBuilder DefaultSettingsMasterMarkupXML = new StringBuilder();
                    try
                    {
                        if (Directory.Exists(ApplicationValues.U2TMarkupsFolderPath))
                        {
                            FileInfo fInfo = new FileInfo(ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml");
                            if (fInfo.Exists)
                            {
                                if (fInfo.Name == this.Title.ToUpper().Trim() + "_MASTER.xml")
                                {
                                    #region _MASTER XML is there

                                    #region Checking and saving Tags MasterXML File Based on the Company
                                    strSettingsMarkupXML = SettingsMasterXMLBasedOnSelectedCompany();
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(strSettingsMarkupXML.ToString());
                                    XmlNode SettingsMarkupXMLNode = xmlDoc.FirstChild.FirstChild;
                                    string temp = SettingsMarkupXMLNode.OuterXml;
                                    string CONFIGURATIONNAME = xmlDoc.FirstChild.FirstChild.Attributes["NAME"].Value;
                                    string CONFIGURATIONVALUE = xmlDoc.FirstChild.FirstChild.Attributes["VALUE"].Value;
                                    bool check = false;
                                    XmlDocument Doc = new XmlDocument();
                                    string checkString = string.Empty;
                                    Doc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml");
                                    foreach (XmlNode node in Doc.SelectSingleNode("//SETTINGSU2TMARKUPXML/MASTERTYPE"))
                                    {
                                        if (node.Attributes["NAME"].Value.Equals(CONFIGURATIONNAME, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if (node.Attributes["VALUE"].Value.Equals(CONFIGURATIONVALUE, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                check = true;
                                                checkString = node.OuterXml;
                                                break;
                                            }
                                        }
                                    }
                                    string fileData = Doc.OuterXml;
                                    if (!check)
                                    {
                                        int index = fileData.LastIndexOf("</CONFIGURATION>");
                                        int len = "</CONFIGURATION>".Length;
                                        fileData = fileData.Substring(0, index + len) + temp + fileData.Substring(index + len);
                                        Doc.LoadXml(fileData);
                                    }
                                    else
                                    {
                                        int index = fileData.IndexOf(checkString);
                                        int len = checkString.Length;
                                        fileData = fileData.Substring(0, index) + temp + fileData.Substring(index + len);
                                        Doc.LoadXml(fileData);

                                    }
                                    #endregion

                                    #region Checking and saving Tags in the Defult Settings Master XML Based on the Master

                                    DefaultSettingsMasterMarkupXML = CreateDefaultSettingsMasterXML();

                                    XmlDocument DefaultXMLDoc = new XmlDocument();
                                    DefaultXMLDoc.LoadXml(DefaultSettingsMasterMarkupXML.ToString());
                                    XmlNode DefaultSettingsMarkupXMLNode = DefaultXMLDoc.FirstChild;
                                    string Defaulttemp = DefaultSettingsMarkupXMLNode.OuterXml;
                                    string masterName = DefaultXMLDoc.FirstChild.Attributes["Name"].Value;
                                    string tVersion = DefaultXMLDoc.FirstChild.Attributes["Version"].Value;
                                    bool Defaultcheck = false;
                                    XmlDocument DefaultDoc = new XmlDocument();
                                    string DefaultcheckString = string.Empty;
                                    #region Settings Master XML is Exits
                                    if (File.Exists(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml"))
                                    {
                                        DefaultDoc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                        foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//SETTINGSMASTER"))
                                        {
                                            if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                if (Defaultnode.Attributes["Version"].Value.Equals(tVersion, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    Defaultcheck = true;
                                                    DefaultcheckString = Defaultnode.OuterXml;
                                                    break;
                                                }
                                            }
                                        }
                                        string DefaultfileData = DefaultDoc.OuterXml;
                                        if (!Defaultcheck)
                                        {
                                            int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                            int len = "</SETTINGSMASTER>".Length;
                                            DefaultfileData = DefaultfileData.Substring(0, index + len) + Defaulttemp + DefaultfileData.Substring(index + len);
                                            DefaultDoc.LoadXml(DefaultfileData);
                                        }
                                        else
                                        {
                                            int index = DefaultfileData.IndexOf(DefaultcheckString);
                                            int len = DefaultcheckString.Length;
                                            DefaultfileData = DefaultfileData.Substring(0, index) + Defaulttemp + DefaultfileData.Substring(index + len);
                                            DefaultDoc.LoadXml(DefaultfileData);

                                        }
                                    }
                                    #endregion
                                    #region Settings Master Is Not Exits
                                    else
                                    {
                                        string MainTag = "<SETTINGSMASTER></SETTINGSMASTER>";
                                        XmlDocument createSettingsMaster = new XmlDocument();
                                        createSettingsMaster.LoadXml(MainTag);
                                        createSettingsMaster.Save(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                        DefaultDoc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                        foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//SETTINGSMASTER"))
                                        {
                                            if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                if (Defaultnode.Attributes["Version"].Value.Equals(tVersion, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    Defaultcheck = true;
                                                    DefaultcheckString = Defaultnode.OuterXml;
                                                    break;
                                                }
                                            }
                                        }
                                        string DefaultfileData = DefaultDoc.OuterXml;
                                        if (!Defaultcheck)
                                        {
                                            int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                            int len = "</SETTINGSMASTER>".Length;
                                            DefaultfileData = DefaultfileData.Substring(0, index + len) + Defaulttemp + DefaultfileData.Substring(index + len);
                                            DefaultDoc.LoadXml(DefaultfileData);
                                        }
                                        else
                                        {
                                            int index = DefaultfileData.IndexOf(checkString);
                                            int len = checkString.Length;
                                            DefaultfileData = DefaultfileData.Substring(0, index) + Defaulttemp + DefaultfileData.Substring(index + len);
                                            DefaultDoc.LoadXml(DefaultfileData);

                                        }
                                    }

                                    #endregion
                                    #endregion

                                    string SaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                                    Doc.Save(SaveFileName);
                                    string SaveDefaultFileName = ApplicationValues.U2TMarkupsFolderPath + @"\SettingsMaster.xml";
                                    DefaultDoc.Save(SaveDefaultFileName);
                                    if (MessageBox.Show(this, "Setting saved Sucssfully", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                    {
                                        ApplicationValues.MainFormIsActiveted = true;
                                       
                                        this.Close();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region _Master XML Is Not There
                                strSettingsMarkupXML.AppendLine("<SETTINGSU2TMARKUPXML>");
                                MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString().Trim());
                                strSettingsMarkupXML.AppendLine(SettingsMasterXMLBasedOnSelectedCompany().ToString());
                                strSettingsMarkupXML.AppendLine("</SETTINGSU2TMARKUPXML>");

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(strSettingsMarkupXML.ToString());


                                #region Checking and saving Tags in the Defult Settings Master XML Based on the Master

                                DefaultSettingsMasterMarkupXML = CreateDefaultSettingsMasterXML();

                                XmlDocument DefaultXMLDoc = new XmlDocument();
                                DefaultXMLDoc.LoadXml(DefaultSettingsMasterMarkupXML.ToString());
                                XmlNode DefaultSettingsMarkupXMLNode = DefaultXMLDoc.FirstChild;
                                string Defaulttemp = DefaultSettingsMarkupXMLNode.OuterXml;
                                string masterName = DefaultXMLDoc.FirstChild.Attributes["Name"].Value;
                                string tVersion = DefaultXMLDoc.FirstChild.Attributes["Version"].Value;
                                bool Defaultcheck = false;
                                XmlDocument DefaultDoc = new XmlDocument();
                                string DefaultcheckString = string.Empty;
                                #region Settings Master XML is Exits
                                if (File.Exists(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml"))
                                {
                                    DefaultDoc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                    foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//SETTINGSMASTER"))
                                    {
                                        if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if (Defaultnode.Attributes["Version"].Value.Equals(tVersion, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                Defaultcheck = true;
                                                DefaultcheckString = Defaultnode.OuterXml;
                                                break;
                                            }
                                        }
                                    }
                                    string DefaultfileData = DefaultDoc.OuterXml;
                                    if (!Defaultcheck)
                                    {
                                        int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                        int len = "</MASTERTYPE>".Length;
                                        DefaultfileData = DefaultfileData.Substring(0, index + len) + Defaulttemp + DefaultfileData.Substring(index + len);
                                        DefaultDoc.LoadXml(DefaultfileData);
                                    }
                                    else
                                    {
                                        int index = DefaultfileData.IndexOf(DefaultcheckString);
                                        int len = DefaultcheckString.Length;
                                        DefaultfileData = DefaultfileData.Substring(0, index) + Defaulttemp + DefaultfileData.Substring(index + len);
                                        DefaultDoc.LoadXml(DefaultfileData);

                                    }
                                }
                                #endregion
                                #region Settings Master Is Not Exits
                                else
                                {
                                    string MainTag = "<SETTINGSMASTER></SETTINGSMASTER>";
                                    XmlDocument createSettingsMaster = new XmlDocument();
                                    createSettingsMaster.LoadXml(MainTag);
                                    createSettingsMaster.Save(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                    DefaultDoc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                    foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//SETTINGSMASTER"))
                                    {
                                        if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if (Defaultnode.Attributes["Version"].Value.Equals(tVersion, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                Defaultcheck = true;
                                                DefaultcheckString = Defaultnode.OuterXml;
                                                break;
                                            }
                                        }
                                    }
                                    string DefaultfileData = DefaultDoc.OuterXml;
                                    if (!Defaultcheck)
                                    {
                                        int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                        int len = "</SETTINGSMASTER>".Length;
                                        DefaultfileData = DefaultfileData.Substring(0, index + len) + Defaulttemp + DefaultfileData.Substring(index + len);
                                        DefaultDoc.LoadXml(DefaultfileData);
                                    }
                                    else
                                    {
                                        int index = DefaultfileData.IndexOf(DefaultcheckString);
                                        int len = DefaultcheckString.Length;
                                        DefaultfileData = DefaultfileData.Substring(0, index) + Defaulttemp + DefaultfileData.Substring(index + len);
                                        DefaultDoc.LoadXml(DefaultfileData);

                                    }
                                }

                                #endregion
                                #endregion

                                string SaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                                xmlDoc.Save(SaveFileName);
                                string SaveDefaultFileName = ApplicationValues.U2TMarkupsFolderPath + @"\SettingsMaster.xml";
                                DefaultDoc.Save(SaveDefaultFileName);
                                if (MessageBox.Show(this, "Setting saved Sucssfully", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                {
                                    ApplicationValues.MainFormIsActiveted = true;
                                   
                                    this.Close();
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            #region Directory and _MASTER XML is not there
                            Directory.CreateDirectory(ApplicationValues.U2TMarkupsFolderPath);

                            strSettingsMarkupXML.AppendLine("<SETTINGSU2TMARKUPXML>");
                            MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString().Trim());
                            strSettingsMarkupXML.AppendLine(SettingsMasterXMLBasedOnSelectedCompany().ToString());
                            strSettingsMarkupXML.AppendLine("</SETTINGSU2TMARKUPXML>");

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(strSettingsMarkupXML.ToString());

                            #region Checking and saving Tags in the Defult Settings Master XML Based on the Master

                            DefaultSettingsMasterMarkupXML = CreateDefaultSettingsMasterXML();

                            XmlDocument DefaultXMLDoc = new XmlDocument();
                            DefaultXMLDoc.LoadXml(DefaultSettingsMasterMarkupXML.ToString());
                            XmlNode DefaultSettingsMarkupXMLNode = DefaultXMLDoc.FirstChild;
                            string Defaulttemp = DefaultSettingsMarkupXMLNode.OuterXml;
                            string masterName = DefaultXMLDoc.FirstChild.Attributes["Name"].Value;
                            string tVersion = DefaultXMLDoc.FirstChild.Attributes["Version"].Value;
                            bool Defaultcheck = false;
                            XmlDocument DefaultDoc = new XmlDocument();
                            string DefaultcheckString = string.Empty;
                            #region Settings Master XML is Exits
                            if (File.Exists(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml"))
                            {
                                DefaultDoc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//SETTINGSMASTER"))
                                {
                                    if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (Defaultnode.Attributes["Version"].Value.Equals(tVersion, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            Defaultcheck = true;
                                            DefaultcheckString = Defaultnode.OuterXml;
                                            break;
                                        }
                                    }
                                }
                                string DefaultfileData = DefaultDoc.OuterXml;
                                if (!Defaultcheck)
                                {
                                    int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                    int len = "</SETTINGSMASTER>".Length;
                                    DefaultfileData = DefaultfileData.Substring(0, index + len) + Defaulttemp + DefaultfileData.Substring(index + len);
                                    DefaultDoc.LoadXml(DefaultfileData);
                                }
                                else
                                {
                                    int index = DefaultfileData.IndexOf(DefaultcheckString);
                                    int len = DefaultcheckString.Length;
                                    DefaultfileData = DefaultfileData.Substring(0, index) + Defaulttemp + DefaultfileData.Substring(index + len);
                                    DefaultDoc.LoadXml(DefaultfileData);

                                }
                            }
                            #endregion
                            #region Settings Master Is Not Exits
                            else
                            {
                                string MainTag = "<SETTINGSMASTER></SETTINGSMASTER>";
                                XmlDocument createSettingsMaster = new XmlDocument();
                                createSettingsMaster.LoadXml(MainTag);
                                createSettingsMaster.Save(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                DefaultDoc.Load(ApplicationValues.U2TMarkupsFolderPath + @"\" + "SettingsMaster.xml");
                                foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//SETTINGSMASTER"))
                                {
                                    if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (Defaultnode.Attributes["Version"].Value.Equals(tVersion, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            Defaultcheck = true;
                                            DefaultcheckString = Defaultnode.OuterXml;
                                            break;
                                        }
                                    }
                                }
                                string DefaultfileData = DefaultDoc.OuterXml;
                                if (!Defaultcheck)
                                {
                                    int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                    int len = "</SETTINGSMASTER>".Length;
                                    DefaultfileData = DefaultfileData.Substring(0, index + len) + Defaulttemp + DefaultfileData.Substring(index + len);
                                    DefaultDoc.LoadXml(DefaultfileData);
                                }
                                else
                                {
                                    int index = DefaultfileData.IndexOf(DefaultcheckString);
                                    int len = DefaultcheckString.Length;
                                    DefaultfileData = DefaultfileData.Substring(0, index) + Defaulttemp + DefaultfileData.Substring(index + len);
                                    DefaultDoc.LoadXml(DefaultfileData);

                                }
                            }

                            #endregion
                            #endregion

                            string SaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                            xmlDoc.Save(SaveFileName);
                            string DefaultSaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                            DefaultDoc.Save(DefaultSaveFileName);
                            if (MessageBox.Show(this, "Setting saved Sucssfully", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            {
                                ApplicationValues.MainFormIsActiveted = true;
                               
                                this.Close();
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error: " + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        Logger.LogInfo(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void SettingsdataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ClearCellValue)
                {
                    SettingsdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void SettingsdataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SettingsdataGridView.Rows[e.RowIndex].ErrorText = String.Empty;
                if (ClearCellValue)
                {
                    SettingsdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                    ClearCellValue = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void SettingsdataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4)
                {
                    string cellFieldValue = string.Empty;

                    if (e.FormattedValue != null && !String.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        cellFieldValue = e.FormattedValue.ToString();

                        if (cellFieldValue.Contains("U2T"))
                        {
                            if (cellFieldValue.Contains("."))
                            {
                                e.Cancel = false;
                            }
                            else
                            {
                                MessageBox.Show(this, "Please Enter Field Value As U2T_(DATA BASE TABLE NAME).(COLUMN NAME)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                ClearCellValue = true;
                                SettingsdataGridView.EndEdit();
                                SettingsdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please Enter Field Value As U2T_(DATA BASE TABLE NAME).(COLUMN NAME)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            ClearCellValue = true;
                            SettingsdataGridView.EndEdit();
                            SettingsdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsdataGridView.ReadOnly = false;
                if (SettingsdataGridView.Rows.Count == 0)
                {
                    SettingsdataGridView.Rows.Add();
                    SettingsdataGridView.Rows[0].Cells["Order"].Value = "1";
                    SettingsdataGridView.Rows[0].Cells["ISActive"].Value = "True";
                    SettingsdataGridView.Rows[0].Cells["IsDefault"].Value = "False";
                    btnSave.Enabled = true;
                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    int LastIndex = SettingsdataGridView.Rows.Count - 1;
                    if ((SettingsdataGridView.Rows[LastIndex].Cells["IsActive"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["XMLOpeningTag"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["DefaultValues"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["FieldValue"].Value != null))
                    {
                        if (((SettingsdataGridView.Rows[LastIndex].Cells["IsActive"].Value.ToString()).ToLower() != "False".ToLower()) || (SettingsdataGridView.Rows[LastIndex].Cells["XMLOpeningTag"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["DefaultValues"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["FieldValue"].Value != null))
                        {
                            SettingsdataGridView.Rows.Add();
                            int intIndex = SettingsdataGridView.Rows.Count - 1;
                            SettingsdataGridView.Rows[intIndex].Cells["Order"].Value = Convert.ToInt32(SettingsdataGridView.Rows[intIndex - 1].Cells["Order"].Value) + 1;
                            SettingsdataGridView.Rows[intIndex].Cells["ISActive"].Value = "True";
                            SettingsdataGridView.Rows[intIndex].Cells["IsDefault"].Value = "False";
                        }
                        else
                        {
                            MessageBox.Show(this, "New Row Is Already Exits in Order No: " + (LastIndex + 1), "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            SettingsdataGridView.Rows[LastIndex].Cells[0].Selected = true;
                        }
                    }
                    else
                        MessageBox.Show(this, "New Row Is Already Exits, Enter the Values", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void SettingsdataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ClearCellValue)
                {
                    SettingsdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                    ClearCellValue = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsdataGridView.ReadOnly = false;
            SettingsdataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
            btnSave.Enabled = true;
            this.Text = this.Text + "    ---Edit Mode---";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SettingsdataGridView.RowCount > 0)
                {
                    if (SettingsdataGridView.SelectedRows.Count > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show(this, "Are You Sure You Need to Delete??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            foreach (DataGridViewRow row in SettingsdataGridView.SelectedRows)
                            {
                                SettingsdataGridView.Rows.Remove(row);
                            }
                            #region Deleting All the Rows In the Grid
                            if (SettingsdataGridView.RowCount == 0)
                            {
                                string MasterID = string.Empty;
                                string configuration_id = string.Empty;
                                if (Directory.Exists(ApplicationValues.U2TMarkupsFolderPath))
                                {
                                    FileInfo fInfo = new FileInfo(ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml");
                                    if (fInfo.Exists)
                                    {
                                        if (fInfo.Name == this.Title.ToUpper().Trim() + "_MASTER.xml")
                                        {
                                            DataSet DS = new DataSet();
                                            DS.ReadXml(fInfo.FullName);
                                            foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                            {
                                                if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
                                                    MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                            }

                                            if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                            {

                                                foreach (DataRow row in DS.Tables["CONFIGURATION"].Rows)
                                                {
                                                    if ((row["NAME"].ToString().ToUpper().Trim() == ApplicationValues.CompanyName.ToUpper().Trim()) && (row["VALUE"].ToString().Trim() == ApplicationValues.CompanyConfigID.Trim()))
                                                        configuration_id = row["CONFIGURATION_id"].ToString();
                                                }
                                                if ((configuration_id != null) && (configuration_id != string.Empty) && (configuration_id != ""))
                                                {
                                                    string filter = "CONFIGURATION_id=" + configuration_id;
                                                    DataRow[] rows = DS.Tables["TALLYTAG"].Select(filter);
                                                    foreach (DataRow TallyTagRow in rows)
                                                    {
                                                        DS.Tables["TALLYTAG"].Rows.Remove(TallyTagRow);
                                                        DS.AcceptChanges();
                                                    }

                                                    string CONFIGURATIONfilter = "CONFIGURATION_id=" + configuration_id;
                                                    DataRow[] CONFIGURATIONrows = DS.Tables["CONFIGURATION"].Select(CONFIGURATIONfilter);
                                                    foreach (DataRow row in CONFIGURATIONrows)
                                                    {
                                                        DS.Tables["CONFIGURATION"].Rows.Remove(row);
                                                        DS.AcceptChanges();
                                                    }

                                                    if (DS.Tables["TALLYTAG"].Rows.Count == 0 && DS.Tables["CONFIGURATION"].Rows.Count == 0)
                                                    {
                                                        string MASTERTYPEfilter = "MASTERTYPE_id=" + MasterID;
                                                        DataRow[] MASTERTYPErows = DS.Tables["MASTERTYPE"].Select(MASTERTYPEfilter);
                                                        foreach (DataRow MASTERTYPERow in MASTERTYPErows)
                                                        {
                                                            DS.Tables["MASTERTYPE"].Rows.Remove(MASTERTYPERow);
                                                            DS.AcceptChanges();
                                                        }

                                                        if (DS.Tables["MASTERTYPE"].Rows.Count == 0)
                                                        {
                                                            if (fInfo.Exists)
                                                            {
                                                                fInfo.Delete();
                                                                contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                                                btnSave.Enabled = false;
                                                                //btnAdd.Text = "Add";
                                                                //contextMenuStrip1.Dispose();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //XmlDocument xmlDoc = new XmlDocument();
                                                            DS.WriteXml(fInfo.FullName);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DS.WriteXml(fInfo.FullName);
                                                        contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                                        btnSave.Enabled = false;
                                                        //btnAdd.Text = "Add";
                                                        //contextMenuStrip1.Dispose();
                                                    }
                                                }//
                                            }
                                        }
                                    }
                                }
                            }// 
                            #endregion
                            else
                            {
                                btnSave_Click(null, null);
                            }
                        }
                    }
                    else
                    {

                        MessageBox.Show(this, "please select at least one row", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                        Logger.LogInfo("please select at least one row");
                    }
                }
                else
                {
                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //btnAdd.Text = "Add";
            getGridMode = "Add";
            contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
            SettingsdataGridView.ReadOnly = false;
            this.Text = Title + " - " + "Setting";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Markap Master Folder And XML file is There
        /// </summary>
        /// <returns>StringBuilder</returns>
        private StringBuilder SettingsMasterXMLBasedOnSelectedCompany()
        {
            string MasterType = string.Empty;
            StringBuilder strSettingsMarkupXML = new StringBuilder();
            try
            {
                MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString());
                strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "' Version='" + ApplicationValues.TallyVersion + "'>");
                MasterType = CFunctions.convertSpecialChars(ApplicationValues.CompanyName);
                strSettingsMarkupXML.AppendLine("<CONFIGURATION  NAME='" + MasterType.Trim() + "' VALUE='" + ApplicationValues.CompanyConfigID + "'>");
                foreach (DataGridViewRow row in SettingsdataGridView.Rows)
                {
                    if (row.Cells["XMLOpeningTag"].Value != null)
                        MasterType = CFunctions.convertSpecialChars(row.Cells["XMLOpeningTag"].Value.ToString().Trim());
                    else
                        MasterType = string.Empty;
                    strSettingsMarkupXML.AppendLine("<TALLYTAG Name='" + MasterType.Trim() + "'>");
                    if (row.Cells["Order"].Value != null)
                        MasterType = CFunctions.convertSpecialChars(row.Cells["Order"].Value.ToString().Trim());
                    else
                        MasterType = string.Empty;
                    strSettingsMarkupXML.AppendLine("<Order>" + MasterType.Trim() + "</Order>");
                    if (row.Cells["DefaultValues"].Value != null)
                        MasterType = CFunctions.convertSpecialChars(row.Cells["DefaultValues"].Value.ToString().Trim());
                    else
                        MasterType = string.Empty;
                    strSettingsMarkupXML.AppendLine("<DEFAULTVALUES>" + MasterType.Trim() + "</DEFAULTVALUES>");
                    if (row.Cells["FieldValue"].Value != null)
                        MasterType = CFunctions.convertSpecialChars(row.Cells["FieldValue"].Value.ToString().Trim());
                    else
                        MasterType = string.Empty;
                    strSettingsMarkupXML.AppendLine("<FIELDVALUE>" + MasterType.Trim() + "</FIELDVALUE>");
                    if (row.Cells["IsActive"].Value == null)
                        MasterType = "False";
                    else if (row.Cells["IsActive"].Value.ToString() == "True")
                        MasterType = "True";
                    else if (row.Cells["IsActive"].Value.ToString() == "False")
                        MasterType = "False";
                    strSettingsMarkupXML.AppendLine("<ISACTIVE>" + MasterType.Trim() + "</ISACTIVE>");
                    if (row.Cells["IsDefault"].Value == null)
                        MasterType = "False";
                    else if (row.Cells["IsDefault"].Value.ToString() == "True")
                        MasterType = "True";
                    else if (row.Cells["IsDefault"].Value.ToString() == "False")
                        MasterType = "False";
                    strSettingsMarkupXML.AppendLine("<ISDEFAULT>" + MasterType.Trim() + "</ISDEFAULT>");
                    strSettingsMarkupXML.AppendLine("</TALLYTAG>");
                }
                strSettingsMarkupXML.AppendLine("</CONFIGURATION>");
                strSettingsMarkupXML.AppendLine("</MASTERTYPE>");

            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
            return strSettingsMarkupXML;
        }

        /// <summary>
        /// Creating Markup Settings Master XML For Default Values
        /// </summary>
        /// <returns>StringBuilder</returns>
        private StringBuilder CreateDefaultSettingsMasterXML()
        {
            string MasterType = string.Empty;
            StringBuilder strSettingsMarkupXML = new StringBuilder();

            try
            {
                MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString().Trim());
                strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "' Version='" + ApplicationValues.TallyVersion + "'>");
                foreach (DataGridViewRow row in SettingsdataGridView.Rows)
                {
                    if (row.Cells["IsDefault"].Value.ToString() == "True" && row.Cells["IsDefault"].Value.ToString() != null)
                    {
                        if (row.Cells["XMLOpeningTag"].Value != null)
                            MasterType = CFunctions.convertSpecialChars(row.Cells["XMLOpeningTag"].Value.ToString());
                        else
                            MasterType = string.Empty;
                        strSettingsMarkupXML.AppendLine("<TALLYTAG Name='" + MasterType.Trim() + "'>");
                        if (row.Cells["Order"].Value != null)
                            MasterType = CFunctions.convertSpecialChars(row.Cells["Order"].Value.ToString());
                        else
                            MasterType = string.Empty;
                        strSettingsMarkupXML.AppendLine("<Order>" + MasterType.Trim() + "</Order>");
                        if (row.Cells["DefaultValues"].Value != null)
                            MasterType = CFunctions.convertSpecialChars(row.Cells["DefaultValues"].Value.ToString());
                        else
                            MasterType = string.Empty;
                        strSettingsMarkupXML.AppendLine("<DEFAULTVALUES>" + MasterType.Trim() + "</DEFAULTVALUES>");
                        if (row.Cells["FieldValue"].Value != null)
                            MasterType = CFunctions.convertSpecialChars(row.Cells["FieldValue"].Value.ToString());
                        else
                            MasterType = string.Empty;
                        strSettingsMarkupXML.AppendLine("<FIELDVALUE>" + MasterType.Trim() + "</FIELDVALUE>");
                        if (row.Cells["IsActive"].Value == null)
                            MasterType = "False";
                        else if (row.Cells["IsActive"].Value.ToString() == "True")
                            MasterType = "True";
                        else if (row.Cells["IsActive"].Value.ToString() == "False")
                            MasterType = "False";
                        strSettingsMarkupXML.AppendLine("<ISACTIVE>" + MasterType.Trim() + "</ISACTIVE>");
                        if (row.Cells["IsDefault"].Value == null)
                            MasterType = "False";
                        else if (row.Cells["IsDefault"].Value.ToString() == "True")
                            MasterType = "True";
                        else if (row.Cells["IsDefault"].Value.ToString() == "False")
                            MasterType = "False";
                        strSettingsMarkupXML.AppendLine("<ISDEFAULT>" + MasterType.Trim() + "</ISDEFAULT>");
                        strSettingsMarkupXML.AppendLine("</TALLYTAG>");
                    }
                }
                strSettingsMarkupXML.AppendLine("</MASTERTYPE>");
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }

            return strSettingsMarkupXML;
        }

        private void AddGridExistingTags(int GridIndex, DataRow TallyTagRow)
        {
            try
            {
                object objValue = new object();
                SettingsdataGridView.Rows.Add();
                if (TallyTagRow["IsActive"] == null)
                    objValue = "False";
                else if (TallyTagRow["IsActive"].ToString() == "")
                    objValue = "False";
                else if (TallyTagRow["IsActive"].ToString() == "True")
                    objValue = "True";
                else if (TallyTagRow["IsActive"].ToString() == "False")
                    objValue = "False";
                SettingsdataGridView.Rows[GridIndex].Cells["IsActive"].Value = objValue.ToString().Trim();
                SettingsdataGridView.Rows[GridIndex].Cells["Order"].Value = TallyTagRow["Order"].ToString().Trim();
                SettingsdataGridView.Rows[GridIndex].Cells["XMLOpeningTag"].Value = TallyTagRow["Name"].ToString().Trim();
                SettingsdataGridView.Rows[GridIndex].Cells["DefaultValues"].Value = TallyTagRow["DefaultValues"].ToString().Trim();
                SettingsdataGridView.Rows[GridIndex].Cells["FieldValue"].Value = TallyTagRow["FieldValue"].ToString().Trim();
                if (TallyTagRow["IsDefault"] == null)
                    objValue = "False";
                else if (TallyTagRow["IsDefault"].ToString() == "")
                    objValue = "False";
                else if (TallyTagRow["IsDefault"].ToString() == "True")
                    objValue = "True";
                else if (TallyTagRow["IsDefault"].ToString() == "False")
                    objValue = "False";
                SettingsdataGridView.Rows[GridIndex].Cells["IsDefault"].Value = objValue.ToString().Trim();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        #endregion

        #region Commented Code

        ///// <summary>
        ///// Creating New Markap Settings Master XML File Based on the Company.
        ///// </summary>
        ///// <returns>StringBuilder</returns>
        //private StringBuilder CreateNewSettingsMasterXMLFile()
        //{
        //    string MasterType = string.Empty;
        //    StringBuilder strSettingsMarkupXML = new StringBuilder();
        //    strSettingsMarkupXML.AppendLine("<SETTINGSU2TMARKUPXML>");
        //    MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString().Trim());
        //    strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "'>");
        //    MasterType = CFunctions.convertSpecialChars(ApplicationValues.CompanyName.Trim());
        //    strSettingsMarkupXML.AppendLine("<CONFIGURATION  NAME='" + MasterType.Trim() + "' VALUE='" + ApplicationValues.CompanyConfigID + "'>");
        //    foreach (DataGridViewRow row in SettingsdataGridView.Rows)
        //    {
        //        if (row.Cells["XMLOpeningTag"].Value != null)
        //            MasterType = CFunctions.convertSpecialChars(row.Cells["XMLOpeningTag"].Value.ToString());
        //        else
        //            MasterType = string.Empty;
        //        strSettingsMarkupXML.AppendLine("<TALLYTAG Name='" + MasterType.Trim() + "'>");
        //        if (row.Cells["Order"].Value != null)
        //            MasterType = CFunctions.convertSpecialChars(row.Cells["Order"].Value.ToString());
        //        else
        //            MasterType = string.Empty;
        //        strSettingsMarkupXML.AppendLine("<Order>" + MasterType.Trim() + "</Order>");
        //        if (row.Cells["DefaultValues"].Value != null)
        //            MasterType = CFunctions.convertSpecialChars(row.Cells["DefaultValues"].Value.ToString());
        //        else
        //            MasterType = string.Empty;
        //        strSettingsMarkupXML.AppendLine("<DEFAULTVALUES>" + MasterType.Trim() + "</DEFAULTVALUES>");
        //        if (row.Cells["FieldValue"].Value != null)
        //            MasterType = CFunctions.convertSpecialChars(row.Cells["FieldValue"].Value.ToString());
        //        else
        //            MasterType = string.Empty;
        //        strSettingsMarkupXML.AppendLine("<FIELDVALUE>" + MasterType.Trim() + "</FIELDVALUE>");
        //        if (row.Cells["IsActive"].Value == null)
        //            MasterType = "False";
        //        else if (row.Cells["IsActive"].Value.ToString() == "True")
        //            MasterType = "True";
        //        else if (row.Cells["IsActive"].Value.ToString() == "False")
        //            MasterType = "False";
        //        strSettingsMarkupXML.AppendLine("<ISACTIVE>" + MasterType.Trim() + "</ISACTIVE>");
        //        if (row.Cells["IsDefault"].Value == null)
        //            MasterType = "False";
        //        else if (row.Cells["IsDefault"].Value.ToString() == "True")
        //            MasterType = "True";
        //        else if (row.Cells["IsDefault"].Value.ToString() == "False")
        //            MasterType = "False";
        //        strSettingsMarkupXML.AppendLine("<ISDEFAULT>" + MasterType.Trim() + "</ISDEFAULT>");
        //        strSettingsMarkupXML.AppendLine("</TALLYTAG>");
        //    }
        //    strSettingsMarkupXML.AppendLine("</CONFIGURATION >");
        //    strSettingsMarkupXML.AppendLine("</MASTERTYPE>");
        //    strSettingsMarkupXML.AppendLine("</SETTINGSU2TMARKUPXML>");

        //    return strSettingsMarkupXML;
        //}

        #endregion

    }
}