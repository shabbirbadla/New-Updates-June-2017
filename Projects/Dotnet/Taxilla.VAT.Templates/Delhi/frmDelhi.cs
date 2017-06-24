using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;

namespace Delhi
{
    public partial class frmDelhi : Form
    {
        #region Variables Declarations
        cls_Delhi_DataTemplate m_obj_Delhi_DataTemplate;
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

                m_obj_Delhi_DataTemplate.FromDate = FromDate;
                m_obj_Delhi_DataTemplate.ToDate = ToDate;
                m_obj_Delhi_DataTemplate.SelectedTemplate = SelectedTemplate;
                m_obj_Delhi_DataTemplate.BgWorkProgress = backgroundWorker;
                m_obj_Delhi_DataTemplate.GetProcData();

                //XML Creation
                //XmlTextWriter xmlstr = m_obj_Daman_DiuDataTemplate.GenerateXML(m_obj_Daman_DiuDataTemplate.DsMain, cFileName.Trim());
                XmlTextWriter xmlstr = m_obj_Delhi_DataTemplate.GenerateXML(m_obj_Delhi_DataTemplate.DsMain.Tables[1], cFileName.Trim());
                backgroundWorker.ReportProgress(100, "XMxL File Generation Completed...");
                MessageBox.Show("XML generated successfully!!\n\nPlease find the xml file at the below location:\n" + cFileName.Trim(), Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public frmDelhi(string connectionstring, string dbname, string iconpath, string vumess, int compid)
        {
            InitializeComponent();

            ConnString = connectionstring;
            DbName = dbname;
            IconPath = iconpath;
            Vumess = vumess;

            Icon ico = new Icon(IconPath);
            this.Icon = ico;

            m_obj_Delhi_DataTemplate = new cls_Delhi_DataTemplate(ConnString);
            m_obj_Delhi_DataTemplate.CompId = compid;
            m_obj_Delhi_DataTemplate.GetCompanyData();
            _Sta_dt = m_obj_Delhi_DataTemplate.FromDate;
            _End_dt = m_obj_Delhi_DataTemplate.ToDate;
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
                    case "DVAT 16 2A_DVAT 30_BILL":
                        fileName = "DVAT 16 2A_DVAT 30_BILL";
                        break;
                    case "DVAT 16 2B_DVAT 31":
                        fileName = "DVAT 16 2B_DVAT 31";
                        break;
                    case "DVAT 16 2C2D":
                        fileName = "DVAT 16 2C2D";
                        break;
                    case "DVAT 17 2A":
                        fileName = "DVAT 17 2A";
                        break;
                    case "DVAT 17 2B":
                        fileName = "DVAT 17 2B";
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

        private void frmDelhi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
                btnClose.PerformClick();
            if (e.Alt && e.KeyCode == Keys.G)
                btnExport.PerformClick();
        }

        private void frmDelhi_FormClosing(object sender, FormClosingEventArgs e)
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
