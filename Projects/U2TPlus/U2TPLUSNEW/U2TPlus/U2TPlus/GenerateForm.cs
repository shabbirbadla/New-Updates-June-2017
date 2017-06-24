using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using U2TPlus.BAL;

namespace U2TPlus
{
    public partial class GenerateForm : Form
    {
        #region Globle Veriables of the class

        string Title = string.Empty;
        GenerateMasterXML generateMater = new GenerateMasterXML();
        StringBuilder GenerateMasterXMLBuilder = new StringBuilder();

        #endregion

        #region Constractor
        public GenerateForm(string title)
        {
            InitializeComponent();
            Title = title;
        }
        #endregion

        #region Form Events

        private void GenerateForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            this.lblTitle.Text = Title + " - " + "XML Genareting";
            this.txtXMLFileName.Text = Title + ".xml";
            this.lblSaveLocation.Text = ApplicationValues.GeneratCompanyXMLFilePath.ToString();
            this.progressBar1.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ApplicationValues.MainFormIsActiveted = true;           
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            bool DefaultExicution = true;
            try
            {
                if (txtXMLFileName.Text != "" && txtXMLFileName.Text != null)
                {
                    this.progressBar1.Show();
                    this.progressBar1.Value = 10;
                    this.progressBar1.Update();
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
                                    DefaultExicution = false;
                                    this.progressBar1.Value = this.progressBar1.Value + 10;
                                    this.progressBar1.Update();
                                    ExicuteXMLWithMasterXML(file);
                                    break;
                                }
                                else if (fInfo.Name == "SettingsMaster.xml" && DefaultExicution == true)
                                {
                                    this.progressBar1.Value = this.progressBar1.Value + 10;
                                    this.progressBar1.Update();
                                    ExicuteXMLWithDefaultValues(file);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please Set the Environment first", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        ApplicationValues.MainFormIsActiveted = true;                       
                        this.Close();
                    }

                }
                else
                {
                    MessageBox.Show(this, "Please Give the File Name of XML", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtXMLFileName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                ApplicationValues.MainFormIsActiveted = true;                              
                Logger.LogInfo(ex);
                this.Close();
            }
        }

        #endregion

        #region Private Methods

        private void ExicuteXMLWithDefaultValues(string file)
        {
            try
            {
                string MasterID = string.Empty;
                DataSet DS = new DataSet();
                generateMater.isGeneratesDefaultSettings = true;
                DS.ReadXml(file);
                foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                {
                    if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
                        MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                    this.progressBar1.Value = this.progressBar1.Value + 10;
                    this.progressBar1.Update();
                }
                if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                {
                    generateMater.TallyTagsTable = DS.Tables["TALLYTAG"];//.Select("CONFIGURATION _id=" + CONFIGURATION_ID);
                    generateMater.Configuration_ID = MasterID;
                    generateMater.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                    if (rbtNotGenerated.Checked)
                    {
                        generateMater.GeneratingOptions = rbtNotGenerated.Text;
                    }
                    else if (rbtGenerated.Checked)
                    {
                        generateMater.GeneratingOptions = rbtGenerated.Text;
                    }
                    else
                    {
                        generateMater.GeneratingOptions = rbtAll.Text;
                    }
                    #region Calling BAL Object To Build The XML

                    ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
                    GenerateMasterXMLBuilder = generateMater.GenareteMaster();
                    this.progressBar1.Value = this.progressBar1.Value + 200;
                    this.progressBar1.Update();
                    if (((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != "")) && (generateMater.dbError == string.Empty))
                    {
                        StringBuilder NewString = new StringBuilder();
                        NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
                        NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
                        NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

                        try
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(NewString.ToString());
                            int rValue = 0;
                            if (generateMater.isNeedToUpdate)
                            {
                                rValue = generateMater.UpdateGenerateMaster();
                                this.progressBar1.Value = this.progressBar1.Value + 100;
                                this.progressBar1.Update();
                            }
                            if (generateMater.dbError == "")
                            {
                                if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
                                {
                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                    this.progressBar1.Value = 1000;
                                    this.progressBar1.Update();
                                    if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                    {
                                        ApplicationValues.MainFormIsActiveted = true;
                                                                             
                                        Logger.LogInfo("Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                        this.Close();
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
                                        xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                        this.progressBar1.Value = 1000;
                                        this.progressBar1.Update();
                                        if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                        {
                                            ApplicationValues.MainFormIsActiveted = true;
                                                                                       
                                            Logger.LogInfo("Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                            this.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                        progressBar1.Hide();
                                        Logger.LogInfo(ex);
                                    }

                                }
                            }
                            else
                            {
                                if (generateMater.dbError == "" || generateMater.dbError == string.Empty)
                                {
                                    MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    progressBar1.Hide();
                                    Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                                }
                                else
                                {
                                    MessageBox.Show(this, generateMater.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    progressBar1.Hide();
                                    Logger.LogInfo(generateMater.dbError);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            progressBar1.Hide();
                            Logger.LogInfo("Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message);
                        }
                    }
                    else
                    {
                        if (generateMater.dbError == "" || generateMater.dbError == string.Empty)
                        {
                            MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            progressBar1.Hide();
                            Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                        }
                        else
                        {
                            MessageBox.Show(this, generateMater.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            progressBar1.Hide();
                            Logger.LogInfo(generateMater.dbError);
                        }
                    }

                    #endregion
                }
                else
                {
                    MessageBox.Show(this, "Please Set the Settings first", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ApplicationValues.MainFormIsActiveted = true;
                           
                    Logger.LogInfo("Please Set the Settings first");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }

        }

        private void ExicuteXMLWithMasterXML(string file)
        {

            try
            {
                string MasterID = string.Empty;
                string Configuration_ID = string.Empty;
                generateMater.isGeneratesDefaultSettings = false;
                DataSet DS = new DataSet();
                DS.ReadXml(file);

                foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                {
                    if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper() && MASTERROW["Version"].ToString().ToUpper() == ApplicationValues.TallyVersion.ToUpper())
                        MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                    this.progressBar1.Value = this.progressBar1.Value + 10;
                    this.progressBar1.Update();
                }

                if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                {
                    foreach (DataRow row in DS.Tables["CONFIGURATION"].Rows)
                    {
                        if ((row["NAME"].ToString().ToUpper().Trim() == ApplicationValues.CompanyName.ToUpper().Trim()) && (row["VALUE"].ToString().Trim() == ApplicationValues.CompanyConfigID.Trim()))
                        {
                            Configuration_ID = row["CONFIGURATION_id"].ToString();
                            this.progressBar1.Value = this.progressBar1.Value + 10;
                            this.progressBar1.Update();
                        }
                    }
                    if ((Configuration_ID != null) && (Configuration_ID != string.Empty) && (Configuration_ID != ""))
                    {
                        generateMater.TallyTagsTable = DS.Tables["TALLYTAG"];//.Select("CONFIGURATION _id=" + CONFIGURATION_ID);
                        generateMater.Configuration_ID = Configuration_ID;
                        generateMater.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                        if (rbtNotGenerated.Checked)
                        {
                            generateMater.GeneratingOptions = rbtNotGenerated.Text;
                        }
                        else if (rbtGenerated.Checked)
                        {
                            generateMater.GeneratingOptions = rbtGenerated.Text;
                        }
                        else
                        {
                            generateMater.GeneratingOptions = rbtAll.Text;
                        }

                        #region Calling BAL Object To Build the XML

                        ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
                        GenerateMasterXMLBuilder = generateMater.GenareteMaster();
                        this.progressBar1.Value = this.progressBar1.Value + 40;
                        this.progressBar1.Update();
                        if (((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != "")) && (generateMater.dbError == ""))
                        {
                            StringBuilder NewString = new StringBuilder();
                            NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
                            NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
                            NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

                            try
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(NewString.ToString());
                                int rValue = 0;
                                if (generateMater.isNeedToUpdate)
                                {
                                    rValue = generateMater.UpdateGenerateMaster();
                                    this.progressBar1.Value = this.progressBar1.Value + 10;
                                    this.progressBar1.Update();
                                }
                                if (generateMater.dbError == "")
                                {
                                    if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
                                    {
                                        xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                        this.progressBar1.Value = 1000;
                                        this.progressBar1.Update();
                                        if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                        {
                                            ApplicationValues.MainFormIsActiveted = true;
                                                                                    
                                            Logger.LogInfo("Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                            this.Close();
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
                                            xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                            this.progressBar1.Value = 1000;
                                            this.progressBar1.Update();
                                            if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                            {
                                                ApplicationValues.MainFormIsActiveted = true;
                                               
                                                Logger.LogInfo("Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + txtXMLFileName.Text);
                                                this.Close();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                            progressBar1.Hide();
                                            Logger.LogInfo(ex);
                                        }
                                    }
                                }
                                else
                                {
                                    if (generateMater.dbError == "" || generateMater.dbError == string.Empty)
                                    {
                                        MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                        progressBar1.Hide();
                                        Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, generateMater.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                        progressBar1.Hide();
                                        Logger.LogInfo(generateMater.dbError);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                progressBar1.Hide();
                                Logger.LogInfo("Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message);
                            }
                        }
                        else
                        {
                            if (generateMater.dbError == "" || generateMater.dbError == string.Empty)
                            {
                                MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                progressBar1.Hide();
                                Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                            }
                            else
                            {
                                MessageBox.Show(this, generateMater.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                progressBar1.Hide();
                                Logger.LogInfo(generateMater.dbError);
                            }
                        }

                        #endregion

                    }
                    else
                    {
                        FileInfo fInfo = new FileInfo(ApplicationValues.U2TMarkupsFolderPath + "/SettingsMaster.xml");
                        if (fInfo.Exists)
                        {
                            this.progressBar1.Value = this.progressBar1.Value + 100;
                            this.progressBar1.Update();
                            ExicuteXMLWithDefaultValues(fInfo.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }

        }

        #endregion

        #region Commented Code
        //GenerateMasterXML_OLD generateMater_OLD = new GenerateMasterXML_OLD();
        #region genarete XML With Master XML
        //private void ExicuteXMLWithMasterXML_OLD(string file)
        //{
        //    string MasterID = string.Empty;
        //    string Configuration_ID = string.Empty;
        //    generateMater_OLD.isGeneratesDefaultSettings = false;
        //    DataSet DS = new DataSet();
        //    DS.ReadXml(file);

        //    foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
        //    {
        //        if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
        //            MasterID = MASTERROW["MASTERTYPE_id"].ToString();
        //    }

        //    if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
        //    {
        //        foreach (DataRow row in DS.Tables["CONFIGURATION"].Rows)
        //        {
        //            if ((row["NAME"].ToString().ToUpper().Trim() == ApplicationValues.CompanyName.ToUpper().Trim()) && (row["VALUE"].ToString().Trim() == ApplicationValues.CompanyConfigID.Trim()))
        //            {
        //                Configuration_ID = row["CONFIGURATION_id"].ToString();
        //            }
        //        }
        //        if ((Configuration_ID != null) && (Configuration_ID != string.Empty) && (Configuration_ID != ""))
        //        {
        //            #region Based on the CONFIGURATION  ID get the TALLY TAGS
        //            generateMater_OLD.TallyTagsTable = DS.Tables["TALLYTAG"];//.Select("CONFIGURATION _id=" + CONFIGURATION_ID);
        //            generateMater_OLD.Configuration_ID = Configuration_ID;
        //            generateMater_OLD.ConnectionString = ApplicationValues.CompanyDBConnectionString;
        //            if (rbtNotGenerated.Checked)
        //            {
        //                generateMater_OLD.GeneratingOptions = rbtNotGenerated.Text;
        //            }
        //            else if (rbtGenerated.Checked)
        //            {
        //                generateMater_OLD.GeneratingOptions = rbtGenerated.Text;
        //            }
        //            else
        //            {
        //                generateMater_OLD.GeneratingOptions = rbtAll.Text;
        //            }
        //            #region For Account Group Master
        //            if (this.Title.ToUpper() == "Account Group Master".ToUpper())
        //            {
        //                #region for Tally Version 9.0
        //                if (ApplicationValues.TallyVersion == "9.0")
        //                {
        //                    ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //                    GenerateMasterXMLBuilder = generateMater_OLD.GenerateAccountGroupMasterTV90();

        //                    if (((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != "")) && (generateMater_OLD.dbError == ""))
        //                    {
        //                        StringBuilder NewString = new StringBuilder();
        //                        NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
        //                        NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
        //                        NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

        //                        try
        //                        {
        //                            XmlDocument xmlDoc = new XmlDocument();
        //                            xmlDoc.LoadXml(NewString.ToString());
        //                            int rValue = 0;
        //                            if (generateMater_OLD.isNeedToUpdate)
        //                            {
        //                                rValue = generateMater_OLD.UpdateGenerateMasterTV90();
        //                            }
        //                            if (generateMater_OLD.dbError == "")
        //                            {
        //                                if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
        //                                {
        //                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                    if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                    {
        //                                        OptionsForm of = new OptionsForm();
        //                                        of.Show(this.Owner);
        //                                        this.Close();
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    try
        //                                    {
        //                                        Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
        //                                        xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                        if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                        {
        //                                            OptionsForm of = new OptionsForm();
        //                                            of.Show(this.Owner);
        //                                            this.Close();
        //                                        }
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                                {
        //                                    MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                        {
        //                            MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                    }

        //                }
        //                #endregion
        //                #region for Tally Version 8.1
        //                else if (ApplicationValues.TallyVersion == "8.1")
        //                {
        //                    MessageBox.Show(this, "Not Implemented Tally Version 8.1 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //                }
        //                #endregion
        //                #region for Tally Version 7.2
        //                else if (ApplicationValues.TallyVersion == "7.2")
        //                {
        //                    MessageBox.Show(this, "Not Implemented Tally Version 7.2 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //                }
        //                #endregion
        //            }
        //            #endregion
        //            #region for Account Master
        //            else if (this.Title.ToUpper() == "Account Master".ToUpper())
        //            {
        //                #region for Tally Version 9.0
        //                if (ApplicationValues.TallyVersion == "9.0")
        //                {
        //                    ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //                    GenerateMasterXMLBuilder = generateMater_OLD.GenerateAccountMasterTV90();

        //                    if ((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != ""))
        //                    {
        //                        StringBuilder NewString = new StringBuilder();
        //                        NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
        //                        NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
        //                        NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

        //                        try
        //                        {
        //                            XmlDocument xmlDoc = new XmlDocument();
        //                            xmlDoc.LoadXml(NewString.ToString());
        //                            int rValue = 0;
        //                            if (generateMater_OLD.isNeedToUpdate)
        //                            {
        //                                rValue = generateMater_OLD.UpdateGenerateMasterTV90();
        //                            }
        //                            if ((generateMater_OLD.dbError == "") && (rValue > 0))
        //                            {
        //                                if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
        //                                {
        //                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                    if (MessageBox.Show(this, "Saved The XML File In Thc Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                    {
        //                                        OptionsForm of = new OptionsForm();
        //                                        of.Show(this.Owner);
        //                                        this.Close();
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    try
        //                                    {
        //                                        Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
        //                                        xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                        if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                        {
        //                                            OptionsForm of = new OptionsForm();
        //                                            of.Show(this.Owner);
        //                                            this.Close();
        //                                        }
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                    }
        //                                }

        //                            }
        //                            else
        //                            {
        //                                if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                                {
        //                                    MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                        {
        //                            MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                    }
        //                }
        //                #endregion
        //                #region for Tally Version 8.1
        //                else if (ApplicationValues.TallyVersion == "8.1")
        //                {
        //                    MessageBox.Show(this, "Not Implemented Tally Version 8.1 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //                }
        //                #endregion
        //                #region for Tally Version 7.2
        //                else if (ApplicationValues.TallyVersion == "7.2")
        //                {
        //                    MessageBox.Show(this, "Not Implemented Tally Version 7.2 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //                }
        //                #endregion
        //            }
        //            #endregion
        //            #region For Item Group Master
        //            else if (this.Title.ToUpper() == "Item Group Master".ToUpper())
        //            {
        //                ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //            }
        //            #endregion
        //            #region For Item Master
        //            else if (this.Title.ToUpper() == "Item Master".ToUpper())
        //            {
        //                ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //            }
        //            #endregion
        //            #endregion
        //        }
        //        else
        //        {
        //            FileInfo fInfo = new FileInfo(ApplicationValues.U2TMarkupsFolderPath + "/SettingsMaster.xml");
        //            if (fInfo.Exists)
        //            {
        //                ExicuteXMLWithDefaultValues(fInfo.FullName);
        //            }
        //        }
        //    }//if
        //}
        #endregion

        #region genarete XML With Default XML
        //private void ExicuteXMLWithDefaultValues_OLD(string file)
        //{
        //    string MasterID = string.Empty;
        //    DataSet DS = new DataSet();
        //    generateMater_OLD.isGeneratesDefaultSettings = true;
        //    DS.ReadXml(file);

        //    foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
        //    {
        //        if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper())
        //            MasterID = MASTERROW["MASTERTYPE_id"].ToString();
        //    }

        //    if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
        //    {
        //        #region Based on the CONFIGURATION  ID get the TALLY TAGS
        //        generateMater_OLD.TallyTagsTable = DS.Tables["TALLYTAG"];//.Select("CONFIGURATION _id=" + CONFIGURATION_ID);
        //        generateMater_OLD.Configuration_ID = MasterID;
        //        generateMater_OLD.ConnectionString = ApplicationValues.CompanyDBConnectionString;
        //        if (rbtNotGenerated.Checked)
        //        {
        //            generateMater_OLD.GeneratingOptions = rbtNotGenerated.Text;
        //        }
        //        else if (rbtGenerated.Checked)
        //        {
        //            generateMater_OLD.GeneratingOptions = rbtGenerated.Text;
        //        }
        //        else
        //        {
        //            generateMater_OLD.GeneratingOptions = rbtAll.Text;
        //        }
        //        #region For Account Group Master
        //        if (this.Title.ToUpper() == "Account Group Master".ToUpper())
        //        {
        //            #region for Tally Version 9.0
        //            if (ApplicationValues.TallyVersion == "9.0")
        //            {
        //                ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //                GenerateMasterXMLBuilder = generateMater_OLD.GenerateAccountGroupMasterTV90();

        //                if (((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != "")) && (generateMater_OLD.dbError == string.Empty))
        //                {
        //                    StringBuilder NewString = new StringBuilder();
        //                    NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
        //                    NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
        //                    NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

        //                    try
        //                    {
        //                        XmlDocument xmlDoc = new XmlDocument();
        //                        xmlDoc.LoadXml(NewString.ToString());
        //                        int rValue = 0;
        //                        if (generateMater_OLD.isNeedToUpdate)
        //                        {
        //                            rValue = generateMater_OLD.UpdateGenerateMasterTV90();
        //                        }
        //                        if (generateMater_OLD.dbError == "")
        //                        {
        //                            if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
        //                            {
        //                                xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                {
        //                                    OptionsForm of = new OptionsForm();
        //                                    of.Show(this.Owner);
        //                                    this.Close();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                try
        //                                {
        //                                    Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
        //                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                    if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                    {
        //                                        OptionsForm of = new OptionsForm();
        //                                        of.Show(this.Owner);
        //                                        this.Close();
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }

        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }

        //            }//
        //            #endregion
        //            #region for Tally Version 8.1
        //            else if (ApplicationValues.TallyVersion == "8.1")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 8.1 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            #endregion
        //            #region for Tally Version 7.2
        //            else if (ApplicationValues.TallyVersion == "7.2")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 7.2 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            #endregion
        //        }
        //        #endregion
        //        #region for Account Master
        //        else if (this.Title.ToUpper() == "Account Master".ToUpper())
        //        {
        //            #region for Tally Version 9.0
        //            if (ApplicationValues.TallyVersion == "9.0")
        //            {
        //                ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //                GenerateMasterXMLBuilder = generateMater_OLD.GenerateAccountMasterTV90();

        //                if ((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != ""))
        //                {
        //                    StringBuilder NewString = new StringBuilder();
        //                    NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
        //                    NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
        //                    NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

        //                    try
        //                    {
        //                        XmlDocument xmlDoc = new XmlDocument();
        //                        xmlDoc.LoadXml(NewString.ToString());
        //                        int rValue = 0;
        //                        if (generateMater_OLD.isNeedToUpdate)
        //                        {
        //                            rValue = generateMater_OLD.UpdateGenerateMasterTV90();
        //                        }
        //                        if ((generateMater_OLD.dbError == "") && (rValue > 0))
        //                        {
        //                            if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
        //                            {
        //                                xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                if (MessageBox.Show(this, "Saved The XML File In Thc Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                {
        //                                    OptionsForm of = new OptionsForm();
        //                                    of.Show(this.Owner);
        //                                    this.Close();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                try
        //                                {
        //                                    Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
        //                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                    if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                    {
        //                                        OptionsForm of = new OptionsForm();
        //                                        of.Show(this.Owner);
        //                                        this.Close();
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //            }
        //            #endregion
        //            #region for Tally Version 8.1
        //            else if (ApplicationValues.TallyVersion == "8.1")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 8.1 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            #endregion
        //            #region for Tally Version 7.2
        //            else if (ApplicationValues.TallyVersion == "7.2")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 7.2 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            #endregion
        //        }
        //        #endregion
        //        #region For Item Group Master
        //        else if (this.Title.ToUpper() == "Item Group Master".ToUpper())
        //        {
        //            if (ApplicationValues.TallyVersion == "9.0")
        //            {
        //                ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //                GenerateMasterXMLBuilder = generateMater_OLD.GenerateItemGroupMasterTV90();

        //                if (((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != "")) && (generateMater_OLD.dbError == ""))
        //                {
        //                    StringBuilder NewString = new StringBuilder();
        //                    NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
        //                    NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
        //                    NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

        //                    try
        //                    {
        //                        XmlDocument xmlDoc = new XmlDocument();
        //                        xmlDoc.LoadXml(NewString.ToString());
        //                        int rValue = 0;
        //                        if (generateMater_OLD.isNeedToUpdate)
        //                        {
        //                            rValue = generateMater_OLD.UpdateGenerateMasterTV90();
        //                        }
        //                        if (generateMater_OLD.dbError == "")
        //                        {
        //                            if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
        //                            {
        //                                xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                {
        //                                    OptionsForm of = new OptionsForm();
        //                                    of.Show(this.Owner);
        //                                    this.Close();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                try
        //                                {
        //                                    Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
        //                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                    if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                    {
        //                                        OptionsForm of = new OptionsForm();
        //                                        of.Show(this.Owner);
        //                                        this.Close();
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }

        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //            }
        //            else if (ApplicationValues.TallyVersion == "8.1")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 8.1 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            else if (ApplicationValues.TallyVersion == "7.2")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 7.2 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //        }
        //        #endregion
        //        #region For Item Master
        //        else if (this.Title.ToUpper() == "Item Master".ToUpper())
        //        {
        //            #region For Tally Version 9.0
        //            if (ApplicationValues.TallyVersion == "9.0")
        //            {
        //                ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
        //                GenerateMasterXMLBuilder = generateMater_OLD.GenerateAccountMasterTV90();

        //                if ((GenerateMasterXMLBuilder.ToString() != null) && (GenerateMasterXMLBuilder.ToString() != ""))
        //                {
        //                    StringBuilder NewString = new StringBuilder();
        //                    NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
        //                    NewString.AppendLine(GenerateMasterXMLBuilder.ToString());
        //                    NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

        //                    try
        //                    {
        //                        XmlDocument xmlDoc = new XmlDocument();
        //                        xmlDoc.LoadXml(NewString.ToString());
        //                        int rValue = 0;
        //                        if (generateMater_OLD.isNeedToUpdate)
        //                        {
        //                            rValue = generateMater_OLD.UpdateGenerateMasterTV90();
        //                        }
        //                        if ((generateMater_OLD.dbError == "") && (rValue > 0))
        //                        {
        //                            if (Directory.Exists(ApplicationValues.GeneratCompanyXMLFilePath))
        //                            {
        //                                xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                if (MessageBox.Show(this, "Saved The XML File In Thc Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                {
        //                                    OptionsForm of = new OptionsForm();
        //                                    of.Show(this.Owner);
        //                                    this.Close();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                try
        //                                {
        //                                    Directory.CreateDirectory(ApplicationValues.GeneratCompanyXMLFilePath);
        //                                    xmlDoc.Save(ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text);
        //                                    if (MessageBox.Show(this, "Saved The XML File In The Location : " + ApplicationValues.GeneratCompanyXMLFilePath + @"\" + textBox1.Text, "Saved Sucsessfully", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
        //                                    {
        //                                        OptionsForm of = new OptionsForm();
        //                                        of.Show(this.Owner);
        //                                        this.Close();
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    MessageBox.Show(this, "Please Select proper location to save the XML file Error:  " + ex.Message, "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (generateMater_OLD.dbError == "" || generateMater_OLD.dbError == string.Empty)
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(this, "Not Generated XML File Because of Internal Error Or Try To Chose the Different Option Like ALL or Generated : May be Error Is :--" + generateMater_OLD.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                    }
        //                }
        //            }
        //            #endregion
        //            #region For Tally version 8.1
        //            else if (ApplicationValues.TallyVersion == "8.1")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 8.1 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            #endregion
        //            #region For Tally Verion 7.2
        //            else if (ApplicationValues.TallyVersion == "7.2")
        //            {
        //                MessageBox.Show(this, "Not Implemented Tally Version 7.2 xml in this Version, Please Download letest Version", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        //            }
        //            #endregion
        //        }
        //        #endregion
        //        #endregion
        //    }//if
        //    else
        //    {
        //        MessageBox.Show(this, "Please Set the Settings first", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //    }
        //}
        #endregion
        #endregion
    }
}