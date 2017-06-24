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
    public partial class ConfigurationsForm : Form
    {
        DataSet RecordsSet = null;
        public ConfigurationsForm()
        {
            InitializeComponent();
        }

        private void ConfigurationsForm_Load(object sender, EventArgs e)
        {
            this.Focus();
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
                MessageBox.Show(this,ex.Message,"U2T PLus - Error",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                Logger.LogInfo(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NewConfigForm ncf = new NewConfigForm("Save", "Adding New Configuration", string.Empty);
            ncf.Show(this.Owner);
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cmbTotalRecords.Items.Count != 0)
            {
                NewConfigForm ncf = new NewConfigForm("Update", "Edit Configuration", cmbTotalRecords.SelectedValue.ToString());
                ncf.Show(this.Owner);
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTotalRecords.Items.Count != 0)
            {
                NewConfigForm ncf = new NewConfigForm("Delete", "Delete Configuration", cmbTotalRecords.SelectedValue.ToString());
                ncf.Show(this.Owner);
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (ApplicationValues.ConfigFileValues != null)
            {
                DataSet xmlRead = new DataSet();
                xmlRead.ReadXml(ApplicationValues.ConfigFilePath);
                if (xmlRead.Tables[0].Rows.Count == 1)
                {
                    SetupEnvironment setenvoronment = new SetupEnvironment();
                    setenvoronment.Show(this.Owner);
                }
                else
                {
                    if (ApplicationValues.IsEnvironmentSetup)
                    {                        
                        OptionsForm of = new OptionsForm();
                        of.Show(this.Owner);
                    }
                    else
                    {
                        SetupEnvironment setenvoronment = new SetupEnvironment();
                        setenvoronment.Show(this.Owner);
                    }
                }
                this.Close();
            }
            else
            {
                OptionsForm OF = new OptionsForm();
                OF.Show(this.Owner);
                this.Close();
            }
        }

        private void cmbTotalRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTotalRecords.SelectedIndex != -1)
            {
                string filter = "ConfigNumber=" + cmbTotalRecords.SelectedValue;
                DataRow[] rows = RecordsSet.Tables[0].Select(filter);
                foreach (DataRow dRow in rows)
                {
                    txtFinancialYear.Text = dRow["FinancialYear"].ToString().Trim();
                    txtSelectCompany.Text = dRow["CompanyName"].ToString().Trim();
                    txtVersionOftheTally.Text = dRow["VersionOftheTally"].ToString().Trim();
                    txtVisualUdyogPath.Text = dRow["VisualUdyogPath"].ToString().Trim();
                    txtXMLPath.Text = dRow["XMLPath"].ToString().Trim();
                    ApplicationValues.INIFilePath = dRow["VisualUdyogPath"].ToString().Trim() + @"\Visudyog.ini";
                }
            }
        }
    }
}