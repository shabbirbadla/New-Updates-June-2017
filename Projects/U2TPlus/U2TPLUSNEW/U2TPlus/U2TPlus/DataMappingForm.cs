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
    public partial class DataMappingForm : Form
    {
        #region Globel Veriables

        string Title = string.Empty;
        string TableDetails = string.Empty;
        U2TPlus.BAL.CommonFunctions CFunctions = new U2TPlus.BAL.CommonFunctions();
        string getGridMode = string.Empty;

        #endregion

        #region Form Events

        public DataMappingForm(string title, string materOrTranDetails)
        {
            Title = title;
            TableDetails = materOrTranDetails;
            InitializeComponent();
        }

        private void DataMappingForm_Load(object sender, EventArgs e)
        {
            this.Focus();

            try
            {
                this.Text = Title;
                string MasterID = string.Empty;
                string configuration_id = string.Empty;
                int GridIndex = 0;

                if (Directory.Exists(ApplicationValues.DataMappingFolderName))
                {
                    string[] files = Directory.GetFiles(ApplicationValues.DataMappingFolderName);
                    if (files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            FileInfo fInfo = new FileInfo(file);
                            if (fInfo.Name == ApplicationValues.DataMappingFileName)
                            {
                                DataSet DS = new DataSet();

                                DS.ReadXml(file);

                                foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                {
                                    if (MASTERROW["Name"].ToString().ToUpper().Trim() == this.Title.ToUpper().Trim())
                                        MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                }
                                if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                {
                                    foreach (DataRow TallyTagRow in DS.Tables["DATAMAPPING"].Rows)
                                    {
                                        if (TallyTagRow["MASTERTYPE_id"].ToString() == MasterID)
                                        {
                                            AddGridExistingTags(GridIndex, TallyTagRow);
                                            GridIndex = GridIndex + 1;
                                        }
                                    }
                                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
                                    contextMenuStrip1.Items["AddToolStripMenuItem"].Enabled = true;
                                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = true;
                                    btnSave.Enabled = true;
                                }
                                else
                                {
                                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                    contextMenuStrip1.Items["AddToolStripMenuItem"].Enabled = true;
                                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
                                    btnSave.Enabled = false;

                                    MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                                }
                            }
                            else
                            {
                                contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                contextMenuStrip1.Items["AddToolStripMenuItem"].Enabled = true;
                                contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
                                btnSave.Enabled = false;

                                MessageBox.Show(this, "No Data Files Are Available, Please Create All Data Files", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                Logger.LogInfo("No Data Files Are Available, Please Create All Data Files");
                            }
                        }
                    }
                    else
                    {
                        contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                        contextMenuStrip1.Items["AddToolStripMenuItem"].Enabled = true;
                        contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
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
                if (SettingsdataGridView.RowCount > 0)
                {
                    getGridMode = "Edit";
                    SettingsdataGridView.ReadOnly = true;
                    btnSave.Enabled = false;
                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    getGridMode = "Add";
                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
                    SettingsdataGridView.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "U2T Plus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Logger.LogInfo(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string MasterType = string.Empty;
                if (SettingsdataGridView.Rows.Count > 0)
                {
                    StringBuilder DefaultSettingsMasterMarkupXML = new StringBuilder();

                    if (Directory.Exists(ApplicationValues.DataMappingFolderName))
                    {
                        FileInfo fInfo = new FileInfo(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName);
                        if (fInfo.Exists)
                        {
                            if (fInfo.Name == ApplicationValues.DataMappingFileName)
                            {
                                DefaultSettingsMasterMarkupXML = CreateDefaultSettingsMasterXML();
                                XmlDocument DefaultXMLDoc = new XmlDocument();
                                DefaultXMLDoc.LoadXml(DefaultSettingsMasterMarkupXML.ToString());
                                XmlNode DefaultSettingsMarkupXMLNode = DefaultXMLDoc.FirstChild;
                                string Defaulttemp = DefaultSettingsMarkupXMLNode.OuterXml;
                                string masterName = DefaultXMLDoc.FirstChild.Attributes["Name"].Value;

                                bool Defaultcheck = false;
                                XmlDocument DefaultDoc = new XmlDocument();
                                string DefaultcheckString = string.Empty;

                                #region Data MApping XML is Exits
                                if (File.Exists(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName))
                                {
                                    DefaultDoc.Load(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName);
                                    foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//DATAMAPPINGSETTINGS"))
                                    {
                                        if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            Defaultcheck = true;
                                            DefaultcheckString = Defaultnode.OuterXml;
                                            break;
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

                                    string SaveDefaultFileName = ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName;
                                    DefaultDoc.Save(SaveDefaultFileName);
                                    if (MessageBox.Show(this, "Setting saved Sucssfully", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                    {
                                        ApplicationValues.MainFormIsActiveted = true;
                                       
                                        this.Close();
                                    }
                                }
                                #endregion

                            }
                        }
                        #region Data MApping XML Is Not Exists
                        else
                        {
                            DefaultSettingsMasterMarkupXML = CreateDefaultSettingsMasterXML();
                            XmlDocument DefaultXMLDoc = new XmlDocument();
                            DefaultXMLDoc.LoadXml(DefaultSettingsMasterMarkupXML.ToString());
                            XmlNode DefaultSettingsMarkupXMLNode = DefaultXMLDoc.FirstChild;
                            string Defaulttemp = DefaultSettingsMarkupXMLNode.OuterXml;
                            string masterName = DefaultXMLDoc.FirstChild.Attributes["Name"].Value;
                            bool Defaultcheck = false;
                            XmlDocument DefaultDoc = new XmlDocument();
                            string DefaultcheckString = string.Empty;

                            string MainTag = "<DATAMAPPINGSETTINGS></DATAMAPPINGSETTINGS>";
                            XmlDocument createSettingsMaster = new XmlDocument();
                            createSettingsMaster.LoadXml(MainTag);
                            createSettingsMaster.Save(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName);
                            DefaultDoc.Load(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName);

                            foreach (XmlNode Defaultnode in DefaultDoc.SelectSingleNode("//DATAMAPPINGSETTINGS"))
                            {
                                if (Defaultnode.Attributes["Name"].Value.Equals(masterName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Defaultcheck = true;
                                    DefaultcheckString = Defaultnode.OuterXml;
                                    break;
                                }
                            }

                            string DefaultfileData = DefaultDoc.OuterXml;
                            if (!Defaultcheck)
                            {
                                int index = DefaultfileData.LastIndexOf("</MASTERTYPE>");
                                int len = "</DATAMAPPINGSETTINGS>".Length;
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

                            string SaveDefaultFileName = ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName;
                            DefaultDoc.Save(SaveDefaultFileName);
                            if (MessageBox.Show(this, "Setting saved Sucssfully", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            {
                                ApplicationValues.MainFormIsActiveted = true;
                               
                                this.Close();
                            }

                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "U2T Plus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Logger.LogInfo(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (SettingsdataGridView.RowCount > 0)
            {

                getGridMode = "Edit";
                SettingsdataGridView.ReadOnly = true;
                btnSave.Enabled = false;
                contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = true;
                contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
            }
            else
            {

                getGridMode = "Add";
                contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = false;
                contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                SettingsdataGridView.ReadOnly = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ApplicationValues.MainFormIsActiveted = true;
           
            this.Close();
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsdataGridView.ReadOnly = false;
                if (SettingsdataGridView.Rows.Count == 0)
                {
                    SettingsdataGridView.Rows.Add();
                    SettingsdataGridView.Rows[0].Cells["IsActive"].Value = "True";
                    btnSave.Enabled = true;
                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = true;
                    contextMenuStrip1.Items["editToolStripMenuItem"].Enabled = true;
                    SettingsdataGridView.Update();
                }
                else
                {
                    int LastIndex = SettingsdataGridView.Rows.Count - 1;
                    if ((SettingsdataGridView.Rows[LastIndex].Cells["IsActive"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["UdyogValue"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["TallyValue"].Value != null))
                    {
                        if (((SettingsdataGridView.Rows[LastIndex].Cells["IsActive"].Value.ToString()).ToLower() != "False".ToLower()) || (SettingsdataGridView.Rows[LastIndex].Cells["UdyogValue"].Value != null) || (SettingsdataGridView.Rows[LastIndex].Cells["TallyValue"].Value != null))
                        {
                            SettingsdataGridView.Rows.Add();
                            int intIndex = SettingsdataGridView.Rows.Count - 1;
                            SettingsdataGridView.Rows[intIndex].Cells["IsActive"].Value = "True";
                            SettingsdataGridView.Update();
                            btnSave.Enabled = true;
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
                MessageBox.Show(this, ex.Message, "U2T Plus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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
                                if (Directory.Exists(ApplicationValues.DataMappingFolderName))
                                {
                                    FileInfo fInfo = new FileInfo(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName);
                                    if (fInfo.Exists)
                                    {
                                        if (fInfo.Name == ApplicationValues.DataMappingFileName)
                                        {
                                            DataSet DS = new DataSet();
                                            DS.ReadXml(fInfo.FullName);
                                            foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                                            {
                                                if (MASTERROW["Name"].ToString().ToUpper() == this.Text.ToUpper())
                                                    MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                                            }

                                            if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                                            {
                                                string filter = "MASTERTYPE_id=" + MasterID;
                                                DataRow[] rows = DS.Tables["DATAMAPPING"].Select(filter);
                                                foreach (DataRow TallyTagRow in rows)
                                                {
                                                    if (TallyTagRow["MASTERTYPE_id"].ToString() == MasterID)
                                                    {
                                                        DS.Tables["DATAMAPPING"].Rows.Remove(TallyTagRow);
                                                        DS.AcceptChanges();
                                                    }
                                                }


                                                if (DS.Tables["DATAMAPPING"].Rows.Count == 0)
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
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DS.WriteXml(fInfo.FullName);
                                                    }
                                                }
                                                else
                                                {
                                                    DS.WriteXml(fInfo.FullName);
                                                    contextMenuStrip1.Items["deleteToolStripMenuItem"].Enabled = false;
                                                    btnSave.Enabled = false;
                                                }

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

        #endregion

        #region Private Methods

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
                SettingsdataGridView.Rows[GridIndex].Cells["UdyogValue"].Value = TallyTagRow["UDYOGVALUE"].ToString().Trim();
                SettingsdataGridView.Rows[GridIndex].Cells["TallyValue"].Value = TallyTagRow["TALLYVALUE"].ToString().Trim();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
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
                strSettingsMarkupXML.AppendLine("<MASTERTYPE Name='" + MasterType.Trim() + "'>");
                foreach (DataGridViewRow row in SettingsdataGridView.Rows)
                {
                    if (row.Cells["IsActive"].Value.ToString() == "True" && row.Cells["IsActive"].Value.ToString() != null)
                    {
                        MasterType = "<DATAMAPPING>";
                        strSettingsMarkupXML.AppendLine(MasterType);
                        if (row.Cells["UdyogValue"].Value != null)
                            MasterType = CFunctions.convertSpecialChars(row.Cells["UdyogValue"].Value.ToString());
                        else
                            MasterType = string.Empty;
                        strSettingsMarkupXML.AppendLine("<UdyogValue>" + MasterType.Trim() + "</UdyogValue>");
                        if (row.Cells["TallyValue"].Value != null)
                            MasterType = CFunctions.convertSpecialChars(row.Cells["TallyValue"].Value.ToString());
                        else
                            MasterType = string.Empty;
                        strSettingsMarkupXML.AppendLine("<TallyValue>" + MasterType.Trim() + "</TallyValue>");
                        if (row.Cells["IsActive"].Value == null)
                            MasterType = "False";
                        else if (row.Cells["IsActive"].Value.ToString() == "True")
                            MasterType = "True";
                        else if (row.Cells["IsActive"].Value.ToString() == "False")
                            MasterType = "False";
                        strSettingsMarkupXML.AppendLine("<ISACTIVE>" + MasterType.Trim() + "</ISACTIVE>");
                        strSettingsMarkupXML.AppendLine("</DATAMAPPING>");
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

        #endregion
    }
}