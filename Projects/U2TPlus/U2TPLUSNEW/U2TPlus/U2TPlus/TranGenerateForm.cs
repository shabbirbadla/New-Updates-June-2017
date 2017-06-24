using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;
using System.Xml;
using System.IO;

namespace U2TPlus
{
    public partial class TranGenerateForm : Form
    {
        #region Globle Veriables of the class

        string Title = string.Empty;
        //GenerateMasterXML generateMater = new GenerateMasterXML();
        StringBuilder GenerateTransXMLBuilder = new StringBuilder();
        GenerateTransXML generateTrans = new GenerateTransXML();


        #endregion

        public TranGenerateForm(string title)
        {
            InitializeComponent();
            Title = title;
        }

        private void TranGenerateForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
                this.lblTitle.Text = Title + " - " + "XML Genareting";
                this.txtXMLFileName.Text = Title + ".xml";
                this.lblSaveLocation.Text = ApplicationValues.GeneratCompanyXMLFilePath.ToString();
                 string previousYear;
                if(DateTime.Now.Month <= 3)
                     previousYear = (DateTime.Now.Year - 2).ToString();
                else
                     previousYear = (DateTime.Now.Year - 1).ToString();
                 string finalDate = "04/01/" + previousYear;
                 DateTime lastFinancealYear;  
                 DateTime.TryParse(finalDate, out lastFinancealYear);
                 this.dateTimePickerFromDate.Value = lastFinancealYear;
                 this.dateTimePickerToDate.Value = DateTime.Today;
                this.progressBar1.Hide();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message,"U2T PLus - Error",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                Logger.LogInfo(ex);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            bool DefaultExicution = true;
            try
            {
                if (CheckingMonditoryFields())
                {
                    this.progressBar1.Show();
                    this.progressBar1.Value = 100;
                    this.progressBar1.Update();

                    if (Directory.Exists(ApplicationValues.ConfigFolderPath))
                    {
                        if (Directory.Exists(ApplicationValues.U2TMarkupsFolderPath))
                        {
                            string[] files = Directory.GetFiles(ApplicationValues.U2TMarkupsFolderPath);
                            foreach (string file in files)
                            {
                                FileInfo fInfo = new FileInfo(file);
                                if (fInfo.Name == this.Title.ToUpper().Trim() + "_TRANSACTIONS.xml")
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error :" + ex.Message, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                ApplicationValues.MainFormIsActiveted = true;
               
                Logger.LogInfo(ex);
                this.Close();
            }
        }

        private void ExicuteXMLWithDefaultValues(string file)
        {
            try
            {
                string MasterID = string.Empty;
                DataSet DS = new DataSet();
                generateTrans.isGeneratesDefaultSettings = true;
                generateTrans.GenerateFromDate = dateTimePickerFromDate.Text;
                generateTrans.GenerateToDate = dateTimePickerToDate.Text;
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
                    generateTrans.TallyTagsTable = DS.Tables["TALLYTAG"];//.Select("CONFIGURATION _id=" + CONFIGURATION_ID);
                    generateTrans.Configuration_ID = MasterID;
                    generateTrans.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                    if (rbtNotGenerated.Checked)
                    {
                        generateTrans.GeneratingOptions = rbtNotGenerated.Text;
                    }
                    else if (rbtGenerated.Checked)
                    {
                        generateTrans.GeneratingOptions = rbtGenerated.Text;
                    }
                    else
                    {
                        generateTrans.GeneratingOptions = rbtAll.Text;
                    }
                    #region Calling BAL Object To Build The XML

                    ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
                    generateTrans.GenaretingTransName = ApplicationValues.GenaretingMasterName;
                    ApplicationValues.TransEntryType = generateTrans.GetEntryType();
                    GenerateTransXMLBuilder = generateTrans.GenerateTransactionXML();
                    this.progressBar1.Value = this.progressBar1.Value + 200;
                    this.progressBar1.Update();
                    if (((GenerateTransXMLBuilder.ToString() != null) && (GenerateTransXMLBuilder.ToString() != "")) && (generateTrans.dbError == string.Empty))
                    {
                        StringBuilder NewString = new StringBuilder();
                        NewString.AppendLine(ApplicationValues.TranXMLHeader.ToString());
                        NewString.AppendLine(GenerateTransXMLBuilder.ToString());
                        NewString.AppendLine(ApplicationValues.TranXMLFooter.ToString());

                        try
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(NewString.ToString());
                            int rValue = 0;
                            if (generateTrans.isNeedToUpdate)
                            {
                                rValue = generateTrans.UpdateGenerateTrans();
                                this.progressBar1.Value = this.progressBar1.Value + 100;
                                this.progressBar1.Update();
                            }
                            if (generateTrans.dbError == "")
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
                                if (generateTrans.dbError == "" || generateTrans.dbError == string.Empty)
                                {
                                    MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    progressBar1.Hide();
                                    Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                                }
                                else
                                {
                                    MessageBox.Show(this, generateTrans.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    progressBar1.Hide();
                                    Logger.LogInfo(generateTrans.dbError);
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
                        if (generateTrans.dbError == "" || generateTrans.dbError == string.Empty)
                        {
                            MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            progressBar1.Hide();
                            Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                        }
                        else
                        {
                            MessageBox.Show(this, generateTrans.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            progressBar1.Hide();
                            Logger.LogInfo(generateTrans.dbError);
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
                generateTrans.isGeneratesDefaultSettings = false;
                generateTrans.GenerateFromDate = dateTimePickerFromDate.Text;
                generateTrans.GenerateToDate = dateTimePickerToDate.Text;
                DataSet DS = new DataSet();
                DS.ReadXml(file);

                foreach (DataRow MASTERROW in DS.Tables["MASTERTYPE"].Rows)
                {
                    if (MASTERROW["Name"].ToString().ToUpper() == this.Title.ToUpper() && MASTERROW["Version"].ToString().ToUpper() == ApplicationValues.TallyVersion.ToUpper())
                        MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                    this.progressBar1.Value = this.progressBar1.Value + 100;
                    this.progressBar1.Update();
                }

                if ((MasterID != null) && (MasterID != string.Empty) && (MasterID != ""))
                {
                    foreach (DataRow row in DS.Tables["CONFIGURATION"].Rows)
                    {
                        if ((row["NAME"].ToString().ToUpper().Trim() == ApplicationValues.CompanyName.ToUpper().Trim()) && (row["VALUE"].ToString().Trim() == ApplicationValues.CompanyConfigID.Trim()))
                        {
                            Configuration_ID = row["CONFIGURATION_id"].ToString();
                            this.progressBar1.Value = this.progressBar1.Value + 100;
                            this.progressBar1.Update();
                        }
                    }
                    if ((Configuration_ID != null) && (Configuration_ID != string.Empty) && (Configuration_ID != ""))
                    {
                        generateTrans.TallyTagsTable = DS.Tables["TALLYTAG"];//.Select("CONFIGURATION _id=" + CONFIGURATION_ID);
                        generateTrans.Configuration_ID = Configuration_ID;
                        generateTrans.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                        if (rbtNotGenerated.Checked)
                        {
                            generateTrans.GeneratingOptions = rbtNotGenerated.Text;
                        }
                        else if (rbtGenerated.Checked)
                        {
                            generateTrans.GeneratingOptions = rbtGenerated.Text;
                        }
                        else
                        {
                            generateTrans.GeneratingOptions = rbtAll.Text;
                        }

                        #region Calling BAL Object To Build the XML

                        ApplicationValues.GenaretingMasterName = this.Title.ToUpper();
                        generateTrans.GenaretingTransName = ApplicationValues.GenaretingMasterName;
                        ApplicationValues.TransEntryType = generateTrans.GetEntryType();
                        GenerateTransXMLBuilder = generateTrans.GenerateTransactionXML();
                        this.progressBar1.Value = this.progressBar1.Value + 200;
                        this.progressBar1.Update();
                        if (((GenerateTransXMLBuilder.ToString() != null) && (GenerateTransXMLBuilder.ToString() != "")) && (generateTrans.dbError == ""))
                        {
                            StringBuilder NewString = new StringBuilder();
                            NewString.AppendLine(ApplicationValues.GenaretingXMLHeader.ToString());
                            NewString.AppendLine(GenerateTransXMLBuilder.ToString());
                            NewString.AppendLine(ApplicationValues.GeneratingXMLFooter.ToString());

                            try
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(NewString.ToString());
                                int rValue = 0;
                                if (generateTrans.isNeedToUpdate)
                                {
                                    rValue = generateTrans.UpdateGenerateTrans();
                                    this.progressBar1.Value = this.progressBar1.Value + 100;
                                    this.progressBar1.Update();
                                }
                                if (generateTrans.dbError == "")
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
                                    if (generateTrans.dbError == "" || generateTrans.dbError == string.Empty)
                                    {
                                        MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                        progressBar1.Hide();
                                        Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, generateTrans.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                        progressBar1.Hide();
                                        Logger.LogInfo(generateTrans.dbError);
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
                            if (generateTrans.dbError == "" || generateTrans.dbError == string.Empty)
                            {
                                MessageBox.Show(this, "Not Generated XML File; please Chose the Different Option Like ALL or Generated", "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                progressBar1.Hide();
                                Logger.LogInfo("Not Generated XML File; please Chose the Different Option Like ALL or Generated");
                            }
                            else
                            {
                                MessageBox.Show(this, generateTrans.dbError, "U2T PLus - Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                progressBar1.Hide();
                                Logger.LogInfo(generateTrans.dbError);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            ApplicationValues.MainFormIsActiveted = true;
           
            this.Close();
        }

        private bool CheckingMonditoryFields()
        {
            bool isModitoryFilds = true;
            if (String.IsNullOrEmpty(txtXMLFileName.Text))
            {
                isModitoryFilds = false;
                MessageBox.Show(this, "Please Enter XML File Name", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtXMLFileName.Focus();
            }
            else if (String.IsNullOrEmpty(dateTimePickerFromDate.Text))
            {
                isModitoryFilds = false;
                MessageBox.Show(this, "Please Select From Date", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (String.IsNullOrEmpty(dateTimePickerToDate.Text))
            {
                isModitoryFilds = false;
                MessageBox.Show(this, "Please Select To Date", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (!String.IsNullOrEmpty(dateTimePickerFromDate.Text) && !String.IsNullOrEmpty(dateTimePickerToDate.Text))
            {
                if (dateTimePickerFromDate.Value >= dateTimePickerToDate.Value)
                {
                    MessageBox.Show(this, "From date should be Lesser then the To date", "U2T PLus - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    isModitoryFilds = false;
                }
            }
            return isModitoryFilds;
        }

    }
}