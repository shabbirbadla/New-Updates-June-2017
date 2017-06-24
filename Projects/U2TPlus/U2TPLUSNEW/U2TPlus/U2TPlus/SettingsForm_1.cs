using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;
using System.IO;
using System.Xml;

namespace U2TPlus
{
    public partial class SettingsForm_1 : Form
    {
        string Title = string.Empty;
        string TableDetails = string.Empty;
        U2TPlus.BAL.CommonFunctions CFunctions = new U2TPlus.BAL.CommonFunctions();
        public SettingsForm_1(string title, string materOrTranDetails)
        {
            Title = title;
            TableDetails = materOrTranDetails;
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
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
                                                GridIndex = GridIndex + 1;
                                            }

                                        }//for 

                                        btnDeleteTag.Enabled = true;
                                        btnSaveAndClose.Enabled = true;                                        
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
                                                        GridIndex = GridIndex + 1;
                                                    }

                                                }//for 

                                                btnDeleteTag.Enabled = true;
                                                btnSaveAndClose.Enabled = true;                                                
                                            }
                                            else
                                            {
                                                btnDeleteTag.Enabled = false;
                                                btnSaveAndClose.Enabled = false;                                               

                                                MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                            }

                                        }
                                        else
                                        {
                                            btnDeleteTag.Enabled = false;
                                            btnSaveAndClose.Enabled = false;                                           

                                            MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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
                                            GridIndex = GridIndex + 1;
                                        }

                                    }//for 

                                    btnDeleteTag.Enabled = true;
                                    btnSaveAndClose.Enabled = true;                                    
                                }
                                else
                                {
                                    btnDeleteTag.Enabled = false;
                                    btnSaveAndClose.Enabled = false;                                   

                                    MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                }
                                break;
                            }
                        }

                        if (DataFiles == false)
                        {
                            btnDeleteTag.Enabled = false;
                            btnSaveAndClose.Enabled = false;
                            
                            MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }//markups folder exits is true
                    else //markups folder exits is false
                    {
                        Directory.CreateDirectory(ApplicationValues.U2TMarkupsFolderPath);
                        btnDeleteTag.Enabled = false;
                        btnSaveAndClose.Enabled = false;                        

                        MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                    MessageBox.Show(this, "Please Set the Environment first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAddNewTag_Click(object sender, EventArgs e)
        {
            if (SettingsdataGridView.Rows.Count == 0)
            {
                SettingsdataGridView.Rows.Add();
                SettingsdataGridView.Rows[0].Cells["Order"].Value = "1";
                SettingsdataGridView.Rows[0].Cells["ISActive"].Value = "True";
                SettingsdataGridView.Rows[0].Cells["IsDefault"].Value = "False";
                btnSaveAndClose.Enabled = true;
                btnDeleteTag.Enabled = true;                
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
                        MessageBox.Show(this, "New Row Is Already Exits in Order No: " + (LastIndex + 1), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        SettingsdataGridView.Rows[LastIndex].Cells[0].Selected = true;
                    }
                }
                else
                    MessageBox.Show(this, "New Row Is Already Exits, Enter the Values", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            if (SettingsdataGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in SettingsdataGridView.SelectedRows)
                {
                    SettingsdataGridView.Rows.Remove(row);
                }
            }
            else
                MessageBox.Show(this, "please select at least one row", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            string MasterType = string.Empty;
            if (SettingsdataGridView.Rows.Count > 0)
            {
                StringBuilder strSettingsMarkupXML = new StringBuilder();
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
                                MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString());
                                strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "'>");
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
                                string SaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                                Doc.Save(SaveFileName);
                                if (MessageBox.Show(this, "Setting saved Sucssfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                {
                                    OptionsForm of = new OptionsForm();
                                    of.Show(this.Owner);
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
                            strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "'>");
                            MasterType = CFunctions.convertSpecialChars(ApplicationValues.CompanyName.Trim());
                            strSettingsMarkupXML.AppendLine("<CONFIGURATION  NAME='" + MasterType.Trim() + "' VALUE='" + ApplicationValues.CompanyConfigID + "'>");
                            foreach (DataGridViewRow row in SettingsdataGridView.Rows)
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
                                strSettingsMarkupXML.AppendLine("<ISACTIVE>" + row.Cells["IsActive"].Value + "</ISACTIVE>");
                                strSettingsMarkupXML.AppendLine("<ISDEFAULT>" + row.Cells["IsDefault"].Value + "</ISDEFAULT>");
                                strSettingsMarkupXML.AppendLine("</TALLYTAG>");
                            }
                            strSettingsMarkupXML.AppendLine("</CONFIGURATION >");
                            strSettingsMarkupXML.AppendLine("</MASTERTYPE>");
                            strSettingsMarkupXML.AppendLine("</SETTINGSU2TMARKUPXML>");
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(strSettingsMarkupXML.ToString());
                            string SaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                            xmlDoc.Save(SaveFileName);
                            if (MessageBox.Show(this, "Setting saved Sucssfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            {
                                OptionsForm of = new OptionsForm();
                                of.Show(this.Owner);
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
                        MasterType = CFunctions.convertSpecialChars(this.Title.ToUpper().ToString());
                        strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "'>");
                        MasterType = CFunctions.convertSpecialChars(ApplicationValues.CompanyName);
                        strSettingsMarkupXML.AppendLine("<CONFIGURATION  NAME='" + MasterType.Trim() + "' VALUE='" + ApplicationValues.CompanyConfigID + "'>");
                        foreach (DataGridViewRow row in SettingsdataGridView.Rows)
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
                            strSettingsMarkupXML.AppendLine("<ISACTIVE>" + row.Cells["IsActive"].Value.ToString().Trim() + "</ISACTIVE>");
                            strSettingsMarkupXML.AppendLine("<ISDEFAULT>" + row.Cells["IsDefault"].Value.ToString().Trim() + "</ISDEFAULT>");
                            strSettingsMarkupXML.AppendLine("</TALLYTAG>");
                        }
                        strSettingsMarkupXML.AppendLine("</CONFIGURATION >");
                        strSettingsMarkupXML.AppendLine("</MASTERTYPE>");
                        strSettingsMarkupXML.AppendLine("</SETTINGSU2TMARKUPXML>");
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(strSettingsMarkupXML.ToString());
                        string SaveFileName = ApplicationValues.U2TMarkupsFolderPath + @"\" + this.Title.ToUpper().Trim() + "_MASTER.xml";
                        xmlDoc.Save(SaveFileName);
                        if (MessageBox.Show(this, "Setting saved Sucssfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            OptionsForm of = new OptionsForm();
                            of.Show(this.Owner);
                            this.Close();
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }        

        private void btnClose_Click(object sender, EventArgs e)
        {
            OptionsForm of = new OptionsForm();
            of.Show(this.Owner);
            this.Close();
        }
    }
}