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
using uBaseForm;
using System.Diagnostics;
using System.Threading;            //Added by Shrikant S. on 31/03/2015 for Bug-25365

namespace TaxillaServiceTax3Template
{
    //public partial class frmST3Interface : Form               //Commented by Shrikant S. on 31/03/2015 for Bug-25365
    public partial class frmST3Interface : uBaseForm.FrmBaseForm        //Added by Shrikant S. on 31/03/2015 for Bug-25365
    {
        //Added by Shrikant S. on 31/03/2015 for Bug-25365      //Start
        DataAccess_Net.clsDataAccess oDataAccess;           
        const int Timeout = 5000;           
        private String cAppPId, cAppName;
        //Added by Shrikant S. on 31/03/2015 for Bug-25365      //End
        #region Variable Declarations
        string _Sta_dt = string.Empty,
            _End_dt = string.Empty,
            FinYear = string.Empty,
            _ConnStr = string.Empty,
            appName = string.Empty;
        string cFileName = string.Empty, chk_1 = string.Empty, chkText = string.Empty;

        cls_ST3Template m_obj_ST3Template;
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
        private string constitutionTyep;
        public string ConstitutionTyep
        {
            get { return constitutionTyep; }
            set { constitutionTyep = value; }
        }
        private string retPeriod;
        public string RetPeriod
        {
            get { return retPeriod; }
            set { retPeriod = value; }
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
        #endregion
            
        //public frmST3Interface(string connectionstring, string dbname, string iconpath, string vumess, int compid)       //Commented by Shrikant S. on 31/03/2015 for Bug-25365
        public frmST3Interface(string[] args)      //Added by Shrikant S. on 31/03/2015 for Bug-25365
        {
            try
            {
                InitializeComponent();

                //Added by Shrikant S. on 31/03/2015 for Bug-25365      //Start
                this.pFrmCaption = "Service Tax 3";
                this.Text = this.pFrmCaption;
                this.pCompId = Convert.ToInt16(args[0]);
                this.pComDbnm = args[1];
                this.pServerName = args[2];
                this.pUserId = args[3];
                this.pPassword = args[4];

                if (args[5] != "")
                {
                    this.pPApplRange = args[5].ToString().Replace("^", "");
                }

                this.pAppUerName = args[6];
                Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
                this.pFrmIcon = MainIcon;
                this.pPApplText = args[8].Replace("<*#*>", " ");
                this.pPApplName = args[9];
                this.pPApplPID = Convert.ToInt32(args[10]);

                this.pPApplCode = args[11];

                DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
                DataAccess_Net.clsDataAccess._serverName = this.pServerName;
                DataAccess_Net.clsDataAccess._userID = this.pUserId;
                DataAccess_Net.clsDataAccess._password = this.pPassword;
                oDataAccess = new DataAccess_Net.clsDataAccess();

                ConnString = "Data Source=" + this.pServerName + ";Initial Catalog=" + this.pComDbnm+";Uid="+this.pUserId+";Pwd="+this.pPassword;
                DbName = this.pComDbnm;
                Vumess = vumess;
                this.Icon = MainIcon;
                CompId = this.pCompId;
                //Added by Shrikant S. on 01/04/2015 for Bug-25365      //End

                //Commented by Shrikant S. on 01/04/2015 for Bug-25365      //Start
                //ConnString = connectionstring;
                //DbName = dbname;
                //IconPath = iconpath;
                //Vumess = vumess;
                //CompId = compid;
                //Icon ico = new Icon(IconPath);
                //this.Icon = ico;
                //Commented by Shrikant S. on 01/04/2015 for Bug-25365      //End

                

                m_obj_ST3Template = new cls_ST3Template(ConnString, Vumess);
                m_obj_ST3Template.CompId = CompId;
                m_obj_ST3Template.GetCompanyData();
                _Sta_dt = m_obj_ST3Template.FromDate;
                _End_dt = m_obj_ST3Template.ToDate;

                grpAssessee.ForeColor = Color.Purple;
                grpAssessee.Font = new Font(grpAssessee.Font.Name, 8, FontStyle.Bold);
                foreach (Control ctrl in grpAssessee.Controls)
                {
                    ctrl.ForeColor = SystemColors.ControlText;
                    ctrl.Font = new Font(ctrl.Font, FontStyle.Regular);
                }
                this.mInsertProcessIdRecord();
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }
        //Added by Shrikant S. on 31/03/2015 for Bug-25365      // Start
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "TaxillaServiceTax3Template.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "','" + this.pPApplName + "'," + this.pPApplPID.ToString() + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";

            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);
        }
        private void mDeleteProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);
        }
        //Added by Shrikant S. on 31/03/2015 for Bug-25365      // End
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Select Output path to save xml file ";
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    txtOutputPath.Text = folderBrowserDialog.SelectedPath.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("Are you sure to Close?", Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                this.mDeleteProcessIdRecord();      //Added by Shrikant S. on 01/04/2015 for Bug-25365      
                if (res == DialogResult.Yes)
                {
                    Dispose(true);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
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

                switch (chkText.ToUpper().Trim())
                {
                    case "PROPRIETORSHIP/INDIVIDUAL": //Quarterly
                    case "LIMITED LIABILITY PARTNERHIP": //Quarterly
                    case "A FIRM": //Quarterly
                        //FDate = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(d1[1]));

                        fileName = "QUARTERLY";
                        break;

                    case "REGISTERED PUBLIC LTD COMPANY": //Monthly
                    case "REGISTERED PRIVATE LTD COMPANY": //Monthly
                    case "REGISTERED TRUST": //Monthly
                    case "SOCIETY/CO-OP SOCIETY": //Monthly
                    case "HINDU UNDIVIDED FAMILY": //Monthly
                    case "GOVERNMENT": //Monthly
                    case "A LOCAL AUTHORITY": //Monthly
                    case "AN ASSOCIATION OF PERSONS OR BODY OF INDIVIDUALS, WHETHER INCORPORATED OR NOT": //Monthly
                    case "EVERY ARTIFICIAL JURIDICAL PERSON, NOT FALLING WITHIN ANY OF THE PRECEDING SUB-CLAUSES": //Monthly

                        fileName = "MONTHLY";
                        break;
                    default:
                        fileName = "MONTHLY";
                        break;
                }
                //cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "ST3_" + fileName.ToString().Trim() + ".xml");

                string dtFormat = dtpFromDate.Text.ToString().Substring(6, 4) + dtpFromDate.Text.ToString().Substring(3, 2) + dtpFromDate.Text.ToString().Substring(0, 2) + "_" + dtpToDate.Text.ToString().Substring(6, 4) + dtpToDate.Text.ToString().Substring(3, 2) + dtpToDate.Text.ToString().Substring(0, 2) + "_" + dtpReturnFilingDate.Text.ToString().Replace("/","-");

                cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "ST3_" + fileName.ToString().Trim() + "_" + dtFormat + ".xml");      // Changed by Sachin N. S. on 25/09/2014 for Bug-24064
                RetPeriod = cboHalfYearly.SelectedItem.ToString().Trim().ToUpper();

                if (backgroundWorker.IsBusy != true)
                {
                    timer.Enabled = true;
                    backgroundWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }

        #region Private Methods
        private bool Validations()
        {
            bool isValid = true;
            if (rbtnOriginal.Checked == false && rbtnRevised.Checked == false)
            {
                errorProvider1.SetError(lblReturnType, "Please select the " + lblReturnType.Text);
                isValid = false;
            }
            if (cboHalfYearly.Text.Length == 0)
            {
                errorProvider1.SetError(lblHalfYearly, "Please select the " + lblHalfYearly.Text);
                isValid = false;
            }
            if (string.IsNullOrEmpty(txtOutputPath.Text.ToString()))
            {
                errorProvider1.SetError(lblOutputPath, lblOutputPath.Text + " cannot be empty");
                isValid = false;
            }

            if (!string.IsNullOrEmpty(dtpFromDate.Text))
            {
                //Added by Shrikant S. on 31/03/2015 for Bug-25365      // Start
                //DateTime date1 = DateTime.ParseExact(dtpReturnFilingDate.Text, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
                //DateTime date2 = DateTime.ParseExact(dtpFromDate.Text, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
                //Added by Shrikant S. on 31/03/2015 for Bug-25365      // End


                //Commented by Shrikant S. on 31/03/2015 for Bug-25365      // Start
                DateTime date1 = Convert.ToDateTime(dtpReturnFilingDate.Value.ToString("dd/MM/yyyy"));
                DateTime date2 = Convert.ToDateTime(dtpFromDate.Text);
                //Commented by Shrikant S. on 31/03/2015 for Bug-25365      // End

                if (date1 < date2)
                {
                    errorProvider1.SetError(lblReturnFilingDate, lblReturnFilingDate.Text + " cannot be less than " + lblHalfYearly.Text + " period.");
                    isValid = false;
                }
            }
            return isValid;
        }
        
        private void GetST3Data()
        {
            try
            {
                //Getting Data from Database

                string[] d1 = this.dtpFromDate.Text.Trim().Split('/');
                string[] d2 = this.dtpToDate.Text.Trim().Split('/');
                FromDate = d1[1] + "/" + d1[0] + "/" + d1[2];
                ToDate = d2[1] + "/" + d2[0] + "/" + d2[2];
                m_obj_ST3Template.FromDate = FromDate;
                m_obj_ST3Template.ToDate = ToDate;
                m_obj_ST3Template.DateOfFilingRet = dtpReturnFilingDate.Value;
                m_obj_ST3Template.BgWorkProgress = backgroundWorker;

                m_obj_ST3Template.GetProcData();

                //XML Creation
                m_obj_ST3Template.ReturnType = rbtnOriginal.Checked ? rbtnOriginal.Text : rbtnRevised.Text;
                m_obj_ST3Template.ReturnPeriod = RetPeriod.Trim();
                XmlTextWriter xmlstr = m_obj_ST3Template.GenerateXML(m_obj_ST3Template.DsMain.Tables[0], cFileName.Trim());
                backgroundWorker.ReportProgress(100, "XML File Generation Completed...");
                MessageBox.Show("XML generated successfully!!\n\nPlease find the xml file at the below location:\n" + cFileName.Trim(), Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }
        #endregion

        private void cboHalfYearly_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboHalfYearly.SelectedItem.ToString().ToUpper().Trim() == "APRIL-SEPTEMBER")
                {
                    dtpFromDate.Text = "01/04/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    dtpToDate.Text = "30/09/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                }
                else if (cboHalfYearly.SelectedItem.ToString().ToUpper().Trim() == "OCTOBER-MARCH")
                {
                    dtpFromDate.Text = "01/10/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    if (_Sta_dt.Substring(_Sta_dt.Length - 4, 4) == _End_dt.Substring(_End_dt.Length - 4, 4))
                        dtpToDate.Text = "31/03/" + (Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4)) + 1).ToString();
                    else
                        dtpToDate.Text = "31/03/" + _End_dt.Substring(_End_dt.Length - 4, 4);
                }
                m_obj_ST3Template.HalfYearlyValue = cboHalfYearly.SelectedItem.ToString().Trim().ToUpper();
                m_obj_ST3Template.DtpFromDate = dtpFromDate.Text;
                m_obj_ST3Template.DtpToDate = dtpToDate.Text;
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GetST3Data();
            }
            catch (Exception ex)
            {
                CallMessageBox.Show("Message : " + ex.Message +
                                             "\nSource : " + ex.Source +
                                             "\nTargetSite : " + ex.TargetSite, Vumess);
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
                CallMessageBox.Show("Message : " + ex.Message +
                                             "\nSource : " + ex.Source +
                                             "\nTargetSite : " + ex.TargetSite, Vumess);
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
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            tsprogressbar.Value = 0;
            tsprogressbar.Visible = false;
            tsLabelStatus.Text = string.Empty;
        }

        private void frmST3Interface_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
                btnClose.PerformClick();
            if (e.Alt && e.KeyCode == Keys.G)
                btnExport.PerformClick();
        }

        private void frmST3Interface_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult res = MessageBox.Show("Are you sure to close?", Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Yes)
                    {
                        this.mDeleteProcessIdRecord();      //Added by Shrikant S. on 01/04/2015 for Bug-25365      
                        Dispose(true);
                        Application.Exit();
                    }
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }

        private void chkIndProp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    chkText = chk.Text.ToString();
                    m_obj_ST3Template.ConstitutionType = chkText.ToUpper().Trim();
                    m_obj_ST3Template.GetDate();

                    foreach (Control ctrl in grpAssessee.Controls)
                    {
                        if (ctrl is CheckBox)
                        {
                            CheckBox chk1 = (CheckBox)ctrl;
                            if (chk1.Text != chkText)
                                chk1.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //CallMessageBox.Show(ex.Message, Vumess);
                //return;
            }
        }

        private void frmST3Interface_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.DateSeparator = "/";
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
        }
        
    }
}
