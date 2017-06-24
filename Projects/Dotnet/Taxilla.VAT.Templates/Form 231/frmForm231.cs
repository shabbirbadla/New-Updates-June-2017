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

namespace Form_231
{
    public partial class frmForm231 : Form
    {
        #region Variables Declaration & Object Creation
        string _Sta_dt = string.Empty, _End_dt = string.Empty, cFileName = string.Empty;

        //Popup toolTip;
        //UCCustomToolTip customToolTip;

        cls_FORM231_DataTemplate m_obj_FORM231_DataTemplate;
        #endregion

        #region Properties
        private Button btn_Done;
        public Button Btn_Done
        {
            get { return btn_Done; }
            set { btn_Done = value; }
        }
        private ComboBox cbo_Select;
        public ComboBox Cbo_Select
        {
            get { return cbo_Select; }
            set { cbo_Select = value; }
        }
        private string periodicityOfReturn;
        public string PeriodicityOfReturn
        {
            get { return periodicityOfReturn; }
            set { periodicityOfReturn = value; }
        }
        private string monthlyPeriodicityOfReturn;
        public string MonthlyPeriodicityOfReturn
        {
            get { return monthlyPeriodicityOfReturn; }
            set { monthlyPeriodicityOfReturn = value; }
        }
        private string quarterlyPeriodicityOfReturn;
        public string QuarterlyPeriodicityOfReturn
        {
            get { return quarterlyPeriodicityOfReturn; }
            set { quarterlyPeriodicityOfReturn = value; }
        }
        private string sixMonthlyPeriodicityOfReturn;
        public string SixMonthlyPeriodicityOfReturn
        {
            get { return sixMonthlyPeriodicityOfReturn; }
            set { sixMonthlyPeriodicityOfReturn = value; }
        }
        private string retPeriod;
        public string RetPeriod
        {
            get { return retPeriod; }
            set { retPeriod = value; }
        }
        private string return_Type;
        public string Return_Type
        {
            get { return return_Type; }
            set { return_Type = value; }
        }
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
        #endregion

        public frmForm231(string connectionstring, string dbname, string iconpath, string vumess,int compid)
        {
            InitializeComponent();
            //toolTip = new Popup(customToolTip = new UCCustomToolTip());
            //toolTip.AutoClose = false;
            //toolTip.FocusOnOpen = false;
            //toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
            //Btn_Done = customToolTip.Btn_Done;
            //Btn_Done.Click += new EventHandler(Btn_Done_Click);

            ConnString = connectionstring;
            DbName = dbname;
            IconPath = iconpath;
            Vumess = vumess;
            CompId = compid;

            Icon ico = new Icon(IconPath);
            this.Icon = ico;

            m_obj_FORM231_DataTemplate = new cls_FORM231_DataTemplate(ConnString);
            m_obj_FORM231_DataTemplate.CompId = CompId;
            m_obj_FORM231_DataTemplate.GetCompanyData();
            _Sta_dt = m_obj_FORM231_DataTemplate.FromDate;
            _End_dt = m_obj_FORM231_DataTemplate.ToDate;

            //chkEligibleForYes.Checked = true;
            //chkFirstReturnNo.Checked = true;
            //chkLastReturnYes.Checked = true;
            //cboPOR.SelectedItem = "Annual";
            //cboSelect.SelectedItem = "2013-2013";
            //cboReturnType.SelectedItem = "Fresh";
            //txtOutputPath.Text = "temp";
        }

        #region Private Methods
        private bool Validations()
        {
            try
            {
                bool isValid = true;
                if (chkEligibleForYes.Checked == false && chkEligibleForNo.Checked == false)
                {
                    errorProvider1.SetError(lblEligibleFor, lblEligibleFor.Text + " is required.");
                    isValid = false;
                }
                if (chkFirstReturnYes.Checked == false && chkFirstReturnNo.Checked == false)
                {
                    errorProvider1.SetError(lblFirstReturn, lblFirstReturn.Text + " is required.");
                    isValid = false;
                }
                if (chkLastReturnYes.Checked == false && chkLastReturnNo.Checked == false)
                {
                    errorProvider1.SetError(lblLastReturn, lblLastReturn.Text + " is required.");
                    isValid = false;
                }
                if (cboReturnType.Text.Length == 0)
                {
                    errorProvider1.SetError(lblReturnType, lblReturnType.Text + " is required.");
                    isValid = false;
                }
                if (cboPOR.Text.Length == 0)
                {
                    errorProvider1.SetError(lblPOR, lblPOR.Text + " is required.");
                    isValid = false;
                }
                if (string.IsNullOrEmpty(txtOutputPath.Text.ToString()))
                {
                    errorProvider1.SetError(lblOutputPath, lblOutputPath.Text + " is required.");
                    isValid = false;
                }
                if (cboSelect.Text.Length == 0)
                {
                    errorProvider1.SetError(lblPOR, lblPOR.Text + " is required.");
                    isValid = false;
                }
                return isValid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool DateValidation()
        {
            try
            {
                string d1 = string.Empty;
                string[] FDate;
                bool isValid = true;
                switch (RetPeriod)
                {
                    case "MONTHLY":
                    case "QUARTERLY":
                    case "SIX-MONTHLY":
                    case "ANNUAL":
                        FDate = FromDate.Split('/');
                        d1 = FDate[1].ToString() + "/" + FDate[0].ToString() + "/" + FDate[2].ToString();

                        //DateTime date1 = Convert.ToDateTime(dtpReturnFilingDate.Value.ToString("dd/MM/yyyy"));
                        DateTime date1 = Convert.ToDateTime(m_obj_FORM231_DataTemplate.DateOfFilingRet.ToString("dd/MM/yyyy"));
                        DateTime date2 = Convert.ToDateTime(d1);
                        if (date1 < date2)
                        {
                            MessageBox.Show("Return Filing Date cannot be less than Periodicity of Return Date.", Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isValid = false;
                        }
                        break;
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
                string monName = string.Empty, monNo = string.Empty;//, FromDate = string.Empty, ToDate = string.Empty;
                int Year;
                Year = Convert.ToInt32(_Sta_dt.Substring(_Sta_dt.Length - 4, 4));

                //Setting the Date Parameter for the Stored Procedure
                switch (RetPeriod)
                {
                    case "MONTHLY":
                        monName = MonthlyPeriodicityOfReturn.ToUpper();
                        switch (monName)
                        {
                            case "JANUARY":
                            case "MARCH":
                            case "MAY":
                            case "JULY":
                            case "AUGUST":
                            case "OCTOBER":
                            case "DECEMBER":
                                monNo = DateTime.ParseExact(monName, "MMMM", CultureInfo.CurrentCulture).Month.ToString();
                                if (monNo.Length != 2)
                                    monNo = "0" + monNo;

                                FromDate = monNo + "/01/" + Year.ToString();
                                ToDate = monNo + "/31/" + Year.ToString();
                                break;

                            case "FEBRUARY":
                                monNo = DateTime.ParseExact(monName, "MMMM", CultureInfo.CurrentCulture).Month.ToString();
                                if (monNo.Length != 2)
                                    monNo = "0" + monNo;

                                FromDate = monNo + "/01/" + Year.ToString();
                                ToDate = monNo;
                                if (DateTime.IsLeapYear(Year))
                                    ToDate = ToDate + "/29/";
                                else
                                    ToDate = ToDate + "/28/";
                                ToDate = ToDate + Year.ToString();
                                break;

                            case "APRIL":
                            case "JUNE":
                            case "SEPTEMBER":
                            case "NOVEMBER":
                                monNo = DateTime.ParseExact(monName, "MMMM", CultureInfo.CurrentCulture).Month.ToString();
                                if (monNo.Length != 2)
                                    monNo = "0" + monNo;

                                FromDate = monNo + "/01/" + Year.ToString();
                                ToDate = monNo + "/30/" + Year.ToString();
                                break;
                        }
                        break;

                    case "QUARTERLY":
                        monName = QuarterlyPeriodicityOfReturn.ToUpper();
                        switch (monName)
                        {
                            case "Q1 (APRIL-JUNE)":
                                FromDate = "04/01/" + Year.ToString();
                                ToDate = "06/30/" + Year.ToString();
                                break;

                            case "Q2 (JULY-SEPTEMBER)":
                                FromDate = "07/01/" + Year.ToString();
                                ToDate = "09/30/" + Year.ToString();
                                break;

                            case "Q3 (OCTOBER-DECEMBER)":
                                FromDate = "10/01/" + Year.ToString();
                                ToDate = "12/31/" + Year.ToString();
                                break;

                            case "Q4 (JANUARY-MARCH)":
                                if (_Sta_dt.Substring(_Sta_dt.Length - 4, 4) == _End_dt.Substring(_End_dt.Length - 4, 4))
                                    Year = Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4));
                                else
                                    Year = Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4));

                                FromDate = "01/01/" + Year.ToString();
                                ToDate = "03/31/" + Year.ToString();
                                break;
                        }
                        break;

                    case "SIX-MONTHLY":
                        monName = SixMonthlyPeriodicityOfReturn.ToUpper();
                        switch (monName)
                        {
                            case "FIRST (APRIL-SEPTEMBER)":
                                FromDate = "04/01/" + Year.ToString();
                                ToDate = "09/30/" + Year.ToString();
                                break;

                            case "SECOND (OCTOBER-MARCH)":
                                FromDate = "10/01/" + Year.ToString();
                                if (_Sta_dt.Substring(_Sta_dt.Length - 4, 4) == _End_dt.Substring(_End_dt.Length - 4, 4))
                                    Year = Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4));
                                else
                                    Year = Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4));
                                ToDate = "03/31/" + Year.ToString();
                                break;
                        }
                        break;

                    case "ANNUAL":
                        if (_Sta_dt.Substring(_Sta_dt.Length - 4, 4) == _End_dt.Substring(_End_dt.Length - 4, 4))
                        {
                            Year = Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4));
                            FromDate = "01/01/" + Year.ToString();
                            ToDate = "12/31/" + Year.ToString();
                        }
                        else
                        {
                            FromDate = "04/01/" + Year.ToString();
                            Year = Convert.ToInt32(_End_dt.Substring(_End_dt.Length - 4, 4));
                            ToDate = "03/31/" + Year.ToString();
                        }
                        break;
                }
                errorProvider1.Clear();
                if (DateValidation() == false)
                    return;
                errorProvider1.Clear();

                //Get the Data from Database
                m_obj_FORM231_DataTemplate.FromDate = FromDate;
                m_obj_FORM231_DataTemplate.ToDate = ToDate;
                m_obj_FORM231_DataTemplate.BgWorkProgress = backgroundWorker;
                m_obj_FORM231_DataTemplate.GetProcData();

                //XML Creation
                m_obj_FORM231_DataTemplate.IsEligibleFor704 = chkEligibleForYes.Checked ? "Yes" : "No";
                m_obj_FORM231_DataTemplate.Return_Type = Return_Type;
                m_obj_FORM231_DataTemplate.First_Return = chkFirstReturnYes.Checked ? "Yes" : "No";
                m_obj_FORM231_DataTemplate.Last_Return = chkLastReturnYes.Checked ? "Yes" : "No";
                m_obj_FORM231_DataTemplate.PeriodicityOfReturn = RetPeriod;
                //m_obj_FORM231_DataTemplate.DateOfFilingRet = dtpReturnFilingDate.Value;
                //m_obj_FORM231_DataTemplate.Remarks = txtRemarks.Text.ToString().Trim();
                XmlTextWriter xmlstr = m_obj_FORM231_DataTemplate.GenerateXML(m_obj_FORM231_DataTemplate.DsMain.Tables[0], cFileName.Trim());
                backgroundWorker.ReportProgress(100, "XML File Generation Completed...");
                MessageBox.Show("XML generated successfully!!\n\nPlease find the xml file at the below location:\n" + cFileName.Trim(), Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Control Properties Event
        //private void Btn_Done_Click(object sender, EventArgs e)
        //{
        //    switch(cboPOR.SelectedItem.ToString().Trim().ToUpper())
        //    {
        //        case "MONTHLY":
        //            MonthlyPeriodicityOfReturn = customToolTip.Cbo_Select.SelectedItem.ToString().Trim();
        //            break;
        //        case "QUARTERLY":
        //            QuarterlyPeriodicityOfReturn = customToolTip.Cbo_Select.SelectedItem.ToString().Trim();
        //            break;
        //        case "SIX-MONTHLY":
        //            SixMonthlyPeriodicityOfReturn = customToolTip.Cbo_Select.SelectedItem.ToString().Trim();
        //            break;
        //        case "ANNUAL":
        //            break;
        //    }
        //    toolTip.Close();
        //}
        #endregion 

        private void chkEligibleForYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    string chkText = chk.Text.ToString();
                    foreach (Control ctrl in grpDealerDtls.Controls)
                    {
                        if (ctrl is CheckBox && (ctrl.Name == "chkEligibleForYes" || ctrl.Name == "chkEligibleForNo"))
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
                throw new Exception(ex.Message);
            }
        }

        private void chkFirstReturnYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    string chkText = chk.Text.ToString();
                    foreach (Control ctrl in grpDealerDtls.Controls)
                    {
                        if (ctrl is CheckBox && (ctrl.Name == "chkFirstReturnYes" || ctrl.Name == "chkFirstReturnNo"))
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
                throw new Exception(ex.Message);
            }
        }

        private void chkLastReturnYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    string chkText = chk.Text.ToString();
                    foreach (Control ctrl in grpDealerDtls.Controls)
                    {
                        if (ctrl is CheckBox && (ctrl.Name == "chkLastReturnYes" || ctrl.Name == "chkLastReturnNo"))
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
                throw new Exception(ex.Message);
            }
        }

        private void cboPOR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cboPOR.SelectedItem.ToString().ToUpper().Trim())
                {
                    case "MONTHLY":
                        if (string.IsNullOrEmpty(MonthlyPeriodicityOfReturn))
                        {
                            //customToolTip.Cbo_Select.Items.Clear();
                            cboSelect.Items.Clear();
                            string monName = string.Empty;
                            for (int i = 1; i <= 12; i++)
                            {
                                monName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                                //customToolTip.Cbo_Select.Items.Add(monName);
                                cboSelect.Items.Add(monName);
                            }
                        }
                        else
                            //customToolTip.Cbo_Select.SelectedItem = MonthlyPeriodicityOfReturn;
                            cboSelect.SelectedItem = MonthlyPeriodicityOfReturn;

                        QuarterlyPeriodicityOfReturn = string.Empty;
                        SixMonthlyPeriodicityOfReturn = string.Empty;
                        //toolTip.Show(cboPOR);
                        break;

                    case "QUARTERLY":
                        if (string.IsNullOrEmpty(QuarterlyPeriodicityOfReturn))
                        {
                            /*customToolTip.Cbo_Select.Items.Clear();
                            customToolTip.Cbo_Select.Items.Add("Q1 (April-June)");
                            customToolTip.Cbo_Select.Items.Add("Q2 (July-September)");
                            customToolTip.Cbo_Select.Items.Add("Q3 (October-December)");
                            customToolTip.Cbo_Select.Items.Add("Q4 (January-March)");*/

                            cboSelect.Items.Clear();
                            cboSelect.Items.Add("Q1 (April-June)");
                            cboSelect.Items.Add("Q2 (July-September)");
                            cboSelect.Items.Add("Q3 (October-December)");
                            cboSelect.Items.Add("Q4 (January-March)");
                        }
                        else
                            //customToolTip.Cbo_Select.SelectedItem = QuarterlyPeriodicityOfReturn;
                            cboSelect.SelectedItem = QuarterlyPeriodicityOfReturn;

                        MonthlyPeriodicityOfReturn = string.Empty;
                        SixMonthlyPeriodicityOfReturn = string.Empty;
                        //toolTip.Show(cboPOR);
                        break;

                    case "SIX-MONTHLY":
                        if (string.IsNullOrEmpty(SixMonthlyPeriodicityOfReturn))
                        {
                            /*customToolTip.Cbo_Select.Items.Clear();
                            customToolTip.Cbo_Select.Items.Add("First (April-September)");
                            customToolTip.Cbo_Select.Items.Add("Second (October-March)");*/

                            cboSelect.Items.Clear();
                            cboSelect.Items.Add("First (April-September)");
                            cboSelect.Items.Add("Second (October-March)");
                        }
                        else
                            //customToolTip.Cbo_Select.SelectedItem = SixMonthlyPeriodicityOfReturn;
                            cboSelect.SelectedItem = SixMonthlyPeriodicityOfReturn;

                        MonthlyPeriodicityOfReturn = string.Empty;
                        QuarterlyPeriodicityOfReturn = string.Empty;
                        //toolTip.Show(cboPOR);
                        break;

                    case "ANNUAL":
                        cboSelect.Items.Clear();
                        cboSelect.Items.Add(m_obj_FORM231_DataTemplate.FinYear);
                        //toolTip.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void frmForm231_Load(object sender, EventArgs e)
        {
            try
            {
                grpDealerDtls.ForeColor = Color.Purple;
                grpDealerDtls.Font = new Font(grpDealerDtls.Font.Name, 8, FontStyle.Bold);
                foreach (Control ctrl in grpDealerDtls.Controls)
                {
                    ctrl.ForeColor = SystemColors.ControlText;
                    ctrl.Font = new Font(ctrl.Font, FontStyle.Regular);
                    label1.ForeColor = Color.Red;
                    label2.ForeColor = Color.Red;
                    label3.ForeColor = Color.Red;
                    label4.ForeColor = Color.Red;
                    label5.ForeColor = Color.Red;
                }

                /*grpDeclarationDtls.ForeColor = Color.Purple;
                grpDeclarationDtls.Font = new Font(grpDeclarationDtls.Font.Name, 8, FontStyle.Bold);
                foreach (Control ctrl in grpDeclarationDtls.Controls)
                {
                    ctrl.ForeColor = SystemColors.ControlText;
                    ctrl.Font = new Font(ctrl.Font, FontStyle.Regular);
                    label6.ForeColor = Color.Red;
                }*/
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

        private void frmForm231_FormClosing(object sender, FormClosingEventArgs e)
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

                cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "MH_FORM_231" + ".xml");

                Return_Type = cboReturnType.SelectedItem.ToString().ToUpper().Trim();
                RetPeriod = cboPOR.SelectedItem.ToString().ToUpper().Trim();

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
                //throw new Exception(ex.Message);
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

        private void frmForm231_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
                btnClose.PerformClick();
            if (e.Alt && e.KeyCode == Keys.G)
                btnExport.PerformClick();
        }

        private void cboSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cboPOR.SelectedItem.ToString().Trim().ToUpper())
                {
                    case "MONTHLY":
                        MonthlyPeriodicityOfReturn = cboSelect.SelectedItem.ToString().Trim();
                        break;
                    case "QUARTERLY":
                        QuarterlyPeriodicityOfReturn = cboSelect.SelectedItem.ToString().Trim();
                        break;
                    case "SIX-MONTHLY":
                        SixMonthlyPeriodicityOfReturn = cboSelect.SelectedItem.ToString().Trim();
                        break;
                    case "ANNUAL":
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
