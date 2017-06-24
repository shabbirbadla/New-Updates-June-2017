using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Xml;

namespace Daman_and_Diu_30_30A_31_31A
{
    public partial class frmDamanDiu : Form
    {
        #region Variables Declarations
        cls_Daman_DiuDataTemplate m_obj_Daman_DiuDataTemplate;
        string _Sta_dt = string.Empty, _End_dt = string.Empty, cFileName = string.Empty;
        #endregion

        #region Properties
        private string connString;
        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }
        private string dbName;
        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }
        private string iconPath;
        public string IconPath
        {
            get { return iconPath; }
            set { iconPath = value; }
        }
        private string vumess;
        public string Vumess
        {
            get { return vumess; }
            set { vumess = value; }
        }
        private int compId;
        public int CompId
        {
            get { return compId; }
            set { compId = value; }
        }
        private string fromDate;
        public string FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }
        private string toDate;
        public string ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }
        private string selectedTemplate;
        public string SelectedTemplate
        {
            get { return selectedTemplate; }
            set { selectedTemplate = value; }
        }
        #endregion
        
        #region Private Methods
        private bool Validations()
        {
            try
            {
                bool isValid = true;
                if (cboSelectTemplate.Text.Length == 0)
                {
                    errorProvider1.SetError(lblSelectTemplate, lblSelectTemplate.Text + " is required.");
                    isValid = false;
                }
                if (string.IsNullOrEmpty(txtOutputPath.Text.ToString()))
                {
                    errorProvider1.SetError(lblOutputPath, lblOutputPath.Text + " is required.");
                    isValid = false;
                }
                if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
                {
                    errorProvider1.SetError(lblSelectDate, lblFrom.Text + " Date cannot be greater than " + lblTo.Text + " Date");
                    isValid = false;
                }
                return isValid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetData()
        {
            try
            {
                //Get the Data from Database
                string[] d1 = dtpFromDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim().Split('/');
                string[] d2 = dtpToDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim().Split('/');
                FromDate = d1[1] + "/" + d1[0] + "/" + d1[2];
                ToDate = d2[1] + "/" + d2[0] + "/" + d2[2];

                m_obj_Daman_DiuDataTemplate.FromDate = FromDate;
                m_obj_Daman_DiuDataTemplate.ToDate = ToDate;
                m_obj_Daman_DiuDataTemplate.SelectedTemplate = SelectedTemplate;
                m_obj_Daman_DiuDataTemplate.BgWorkProgress = backgroundWorker;
                m_obj_Daman_DiuDataTemplate.GetProcData();

                //XML Creation
                //XmlTextWriter xmlstr = m_obj_Daman_DiuDataTemplate.GenerateXML(m_obj_Daman_DiuDataTemplate.DsMain, cFileName.Trim());
                XmlTextWriter xmlstr = m_obj_Daman_DiuDataTemplate.GenerateXML(m_obj_Daman_DiuDataTemplate.DsMain.Tables[1], cFileName.Trim());
                backgroundWorker.ReportProgress(100, "XML File Generation Completed...");
                MessageBox.Show("XML generated successfully!!\n\nPlease find the xml file at the below location:\n" + cFileName.Trim(), Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public frmDamanDiu(string connectionstring, string dbname, string iconpath, string vumess, int compid)
        {
            InitializeComponent();

            ConnString = connectionstring;
            DbName = dbname;
            IconPath = iconpath;
            Vumess = vumess;

            Icon ico = new Icon(IconPath);
            this.Icon = ico;

            m_obj_Daman_DiuDataTemplate = new cls_Daman_DiuDataTemplate(ConnString);
            m_obj_Daman_DiuDataTemplate.CompId = compid;
            m_obj_Daman_DiuDataTemplate.GetCompanyData();
            _Sta_dt = m_obj_Daman_DiuDataTemplate.FromDate;
            _End_dt = m_obj_Daman_DiuDataTemplate.ToDate;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.Clear();
                if (Validations() == false)
                    return;
                errorProvider1.Clear();


                if (!string.IsNullOrEmpty(txtOutputPath.Text.Trim()))
                    if (!System.IO.Directory.Exists(txtOutputPath.Text))
                        System.IO.Directory.CreateDirectory(txtOutputPath.Text);

                string fileName = string.Empty;

                SelectedTemplate = cboSelectTemplate.SelectedItem.ToString().Trim().ToUpper();

                switch (SelectedTemplate)
                {
                    case "DAMAN VAT 30":
                        fileName = "Daman VAT30";
                        break;
                    case "DAMAN VAT 30A":
                        fileName = "Daman VAT30A";
                        break;
                    case "DAMAN VAT 31":
                        fileName = "Daman VAT 31_Template";
                        break;
                    case "DAMAN VAT 31A":
                        fileName = "Daman VAT 31A_Template";
                        break;
                }
                cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, fileName.ToString().Trim() + ".xml");
                //cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "Daman VAT" + ".xml");
                FromDate = dtpFromDate.Value.ToString();
                ToDate = dtpToDate.Value.ToString();

                if (backgroundWorker.IsBusy != true)
                {
                    timer.Enabled = true;
                    backgroundWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Select Output path to save xml file ";
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    txtOutputPath.Text = folderBrowserDialog.SelectedPath.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message : " + ex.Message +
                                             "\nSource : " + ex.Source +
                                             "\nTargetSite : " + ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                tsprogressbar.Visible = true;
                tsprogressbar.Value = e.ProgressPercentage;
                tsLabelStatus.Text = "Please wait while.. " + e.UserState.ToString().Trim();
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                MessageBox.Show("Message : " + ex.Message +
                                             "\nSource : " + ex.Source +
                                             "\nTargetSite : " + ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            tsprogressbar.Value = 0;
            tsprogressbar.Visible = false;
            tsLabelStatus.Text = string.Empty;
        }

        private void frmDamanDiu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
                btnClose.PerformClick();
            if (e.Alt && e.KeyCode == Keys.G)
                btnExport.PerformClick();
        }

        private void frmDamanDiu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult res = MessageBox.Show("Are you sure to exit?", Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Yes)
                    {
                        Dispose(true);
                        Application.Exit();
                    }
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("Are you sure to exit?", Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.Yes)
                {
                    Dispose(true);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
