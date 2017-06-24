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
    public partial class SetupEnvironment : Form
    {
        DataSet RecordsSet = null;
        public SetupEnvironment()
        {
            InitializeComponent();
        }

        private void btnEnvironment_Click(object sender, EventArgs e)
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

                OptionsForm of = new OptionsForm();
                of.Show(this.Owner);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", ApplicationValues.ApplicationName + "-" + "Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            if (DR == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (DR == DialogResult.No)
            {
                if (ApplicationValues.IsEnvironmentSetup)
                {
                    OptionsForm OF = new OptionsForm();
                    OF.Show(this.Owner);
                }
                else
                {
                    ApplicationValues.IsEnvironmentSetup = false;
                    OptionsForm OF = new OptionsForm();
                    OF.Show(this.Owner);
                }
                this.Close();
            }
        }

        private void SetupEnvironment_Load(object sender, EventArgs e)
        {
            this.Focus();
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
                                    cmbTotalRecords.DisplayMember = "ConfigNumber";
                                    cmbTotalRecords.ValueMember = "ConfigNumber";
                                    cmbTotalRecords.DataSource = RecordsSet.Tables[0];

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

                            OptionsForm of = new OptionsForm();
                            of.Show(this.Owner);
                            this.Close();
                        }
                        else
                        {
                            if (RecordsSet.Tables[0].Rows.Count == 1 && ApplicationValues.IsEnvironmentSetup == true)
                            {
                                this.btnEnvironment_Click(null, null);
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
    }
}