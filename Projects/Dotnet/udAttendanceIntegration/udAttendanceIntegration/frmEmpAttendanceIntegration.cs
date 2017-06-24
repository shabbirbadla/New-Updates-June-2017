using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;
using System.Data.OleDb;
using udReportList;       // Added by amrendra on 03-07-2012
using System.Diagnostics; // Added by amrendra on 03-07-2012
using System.IO;
using System.Globalization;
using System.Threading;
using udclsDGVNumericColumn;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udAttendanceIntegration
{
    public partial class frmAttendanceIntegration : uBaseForm .FrmBaseForm
    {
        String cAppPId, cAppName;
        int dgvCurRow = 0, dgvCurCol = 0;
        int xNmCount = 0; //Added By Kishor A. for Bug-26387 on 13/07/2015

        short vTimeOut=25;
        int controlVal = 0;
        int retVal = 0;
        string sqlQuery;
        //bool IsFetched = false;
        bool b = true;
        bool vEnable = false;
        bool IsError = false;
        string ErrorStr = "";
        //string vMainTblNm = "MpMain", vMainField = "Inv_No", vOrdFld = "Inv_No", vMainFldVal = ""; //Added By Amrendra On 03-07-2012
        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
        string vMonth = string.Empty, vYear = string.Empty, vFileName = string.Empty, vTran_Cd=String.Empty ;
        DataSet vDsCommon; //Added by Amrendra 
        DataSet dsData = new DataSet();
        DataSet tDs = new DataSet();
        DataAccess_Net.clsDataAccess oDataAccess = new clsDataAccess();
        string empCode = string.Empty;
        string sickLeaves = string.Empty;
        string casualLeaves = string.Empty;
        string presentDays = string.Empty;
        string month = string.Empty;
        string absentDays = string.Empty;
        string holidays = string.Empty;
        string weeklyOffs = string.Empty;
        decimal totPaidDays;

        Decimal vMonthDays = 0;
        string SqlStr = "";
        DataTable tblAttSett, dtMonthlyMuster, dtDailyMuster, dtUploadStatus, dtDisplay, tblAdvAttSett;
        Boolean IsValid;
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end


        #region Initialization of Form with parameter        
        public frmAttendanceIntegration(string[] args)
        {
            
            InitializeComponent();
            this.pDisableCloseBtn = true;
            this.pPara = args;
            this.pFrmCaption = "Attendance Integration";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
        }
        #endregion

        #region Form Load Event
        private void frmAttendanceIntegration_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            txtMuster.Text = "Monthly";
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnMonth.Image = Image.FromFile(fName);
                this.btnProcYear.Image = Image.FromFile(fName);
                this.sbExcelFile.Image = Image.FromFile(fName);
            }

            btnFirst.Enabled = false;
            btnBack.Enabled = false;
            btnForward.Enabled = false;
            btnLast.Enabled = false;
            //btnEmail.Enabled = true; Commented By Kishor A. for Bug-25818 
            btnEmail.Enabled = false; // Added  for Bug-25818 By Kishor A.
            btnLocate.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            //btnEdit.Enabled = true; Commented for Bug-25818 By Kishor A.
            btnEdit.Enabled = false; // Added for Bug-25818 By Kishor A.
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnPreview.Enabled = false;
            //btnPrint.Enabled = true; Commented for Bug-25818 By Kishor A.
            btnPrint.Enabled = false; // Added for Bug-25818 By Kishor A.
            btnExportPdf.Enabled = false;
            //btnGetExcel.Enabled = false; Commented for Bug-25818 By Kishor A.
            btnGetExcel.Enabled = true; // Added for Bug-25818 By Kishor A.
            btnLogout.Enabled = true; 
            btnHelp.Enabled = false;
            btnCalculator.Enabled = false;
            btnExit.Enabled = false;
            sbExcelFile.Enabled = true;// Added for Bug-25818 By Kishor A.
            this.mInsertProcessIdRecord();
            this.mthDsCommon();
            loadLatestData();
            //Added by Archana K. on 16/05/13 for Bug-7899 start
            startupPath = Application.StartupPath;
            //startupPath = "F:\\Installer12.0";
            oConnect = new clsConnect();
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            oConnect.InitProc("'" + startupPath + "'", appfile);
            DirectoryInfo dir = new DirectoryInfo(startupPath);
            Array totalFile = dir.GetFiles();
            string registerMePath = string.Empty;
            for (int i = 0; i < totalFile.Length; i++)
            {
                string fname = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                {
                    registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    break;
                }
                
            }
            if (registerMePath == string.Empty)
            {
                ServiceType = "";
            }
            else
            {
                string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
                ServiceType = objRegisterMe[15].ToString().Trim();
            }
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
                this.groupBox1.Enabled = false;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            ControlsEnable();
            this.SetFormColor();
            //tabControl1.Height = this.Height - (groupBox1.Height + 60);
            
            //dgvExcel.Height = tabPage1.Height-10;
            dgvExcel.Height = this.Height - (groupBox1.Height + 60);
            lcProcessingMonth.Focus();
        }
        #endregion

        //Added By Amrendra On 03-07-2012

        #region Common Datatables
        private void mthDsCommon()
        {
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            SqlStr = "Select * From vudyog..Co_Mast where CompId=" + pPara[0];
            company = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            SqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

            //Added By Amrendra On 25/07/2012 
            SqlStr = "Select Att_Nm,Att_Code,isleave,h_Att_Code,h_Att_Nm From Emp_Attendance_Setting where LDeactive=0 and att_code not in ('MonthDays','SalPaidDays') order by SortOrd";
            tblAttSett = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            SqlStr = "Select distinct h_Att_Code,h_Att_Nm,Att_Code,Att_Nm from emp_attendance_setting where isnull(h_Att_Code,'')<>''";
            tblAdvAttSett = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);


            SqlStr = "Select Max(Pay_month) From Emp_daily_Muster group by pay_month";
            DataTable dtxx = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (dtxx.Rows.Count > 0)
                this.txtProcMonth.Text = this.fnNMonth(Convert.ToInt32(dtxx.Rows[0][0]));
            SqlStr = "Select Max(Pay_year) From Emp_daily_Muster group by pay_month";
            dtxx = null;
            dtxx = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (dtxx.Rows.Count > 0)
                this.txtProcYear.Text = dtxx.Rows[0][0].ToString();

            //Added By Amrendra On 25/07/2012 
            

        }
        private int GetMonthDays(int lnMonth, string lsYear)
        {
            SqlStr = "Select top 1 MonthDays From Emp_Monthly_Muster where Pay_Year='"+lsYear.Trim()+"' and Pay_Month="+lnMonth;
            DataTable dtxx = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (dtxx.Rows.Count > 0)
                return Convert.ToInt32(dtxx.Rows[0][0]);
            return 0;

        }
        #endregion

        #region Load Grid With Latest Data
        private void loadLatestData()
        {
            vMonthDays=GetMonthDays(this.fnNMonth(this.txtProcMonth.Text.Trim()), this.txtProcYear.Text.Trim());
            SqlStr = "Select EmployeeCode,upload from Emp_daily_Muster where Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
            SqlStr = SqlStr + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            dtUploadStatus = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            if (!string.IsNullOrEmpty(this.txtProcMonth.Text) & !string.IsNullOrEmpty(this.txtProcYear.Text))
            {
                SqlStr = "Execute Usp_Ent_Emp_Get_Attendance_Approval '" + this.txtProcYear.Text.Trim() + "', " + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim();
                dtDailyMuster = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                ColumnBindToGrid();
                if (dtDailyMuster.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvExcel.Columns.Count; i++)
                        dgvExcel.Columns[i].ReadOnly = true;
                }
            }
        }
        #endregion

        #region Add Columns To the Grid
        private void ColumnBindToGrid()
        {
            dgvExcel.RowTemplate.Height = 18;
            dgvExcel.DataSource = dtDailyMuster;
            dgvExcel.AutoResizeColumns ();
            dgvExcel.Refresh();
            //dgvExcel.DataSource = dtDisplay;
            //return;
            dgvExcel.AllowUserToAddRows = false;
            dgvExcel.Columns.Clear();
            
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.HeaderText = "Upload";
            check.Name = "Upload";
            check.DataPropertyName = "Upload";
            check.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            check.ReadOnly = true;
            dgvExcel.Columns.Add(check);

            DataGridViewTextBoxColumn text = new DataGridViewTextBoxColumn();
            text.HeaderText = "Employee Code";
            text.Name = "EmployeeCode";
            text.DataPropertyName = "EmployeeCode";
            text.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dgvExcel.Columns.Add(text);

            DataGridViewTextBoxColumn txtEmpName = new DataGridViewTextBoxColumn();
            txtEmpName.HeaderText = "Employee Name";
            txtEmpName.DataPropertyName = "EmployeeName";
            txtEmpName.Name = "EmployeeName";
            txtEmpName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtEmpName.Width = 110;
            dgvExcel.Columns.Add(txtEmpName);

            //Added By Kishor A. for Bug-26387 on 13/07/2015 Start
            DataGridViewTextBoxColumn OTH = new DataGridViewTextBoxColumn();
            OTH.HeaderText = "OT HOURS";
            OTH.Name = "OTH";
            OTH.DataPropertyName = "OTH";
            OTH.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dgvExcel.Columns.Add(OTH);

            DataGridViewTextBoxColumn HOTH = new DataGridViewTextBoxColumn();
            HOTH.HeaderText = "HOT HOURS";
            HOTH.DataPropertyName = "HOTH";
            HOTH.Name = "HOTH";
            HOTH.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            HOTH.Width = 110;
            dgvExcel.Columns.Add(HOTH);
            //Added By Kishor A. for Bug-26387 on 13/07/2015 End

            //for (int i = 1; i <= 31; i++)
            //{
            //    text = new DataGridViewTextBoxColumn();
            //    text.HeaderText = "Day" + i.ToString().Trim();
            //    text.DataPropertyName = "Day" + i.ToString().Trim();
            //    text.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //    dataGridView1.Columns.Add(text);
            //}

            DataGridViewComboBoxColumn cmbBox;
            for (int i = 1; i <= vMonthDays; i++)
               {
                cmbBox = new DataGridViewComboBoxColumn();
                foreach (DataRow drt in tblAttSett.Rows)
                {
                    cmbBox.Items.Add(drt["Att_Nm"].ToString());
                    if (!string.IsNullOrEmpty(drt["H_Att_Nm"].ToString()))
                    {
                        cmbBox.Items.Add(drt["H_Att_Nm"].ToString());
                    }
                }


                cmbBox.HeaderText = "Day" + i.ToString().Trim();
                cmbBox.Name = "Day" + i.ToString().Trim();
                cmbBox.DataPropertyName = "Day" + i.ToString().Trim();
                cmbBox.Width = 100;
                dgvExcel.Columns.Add(cmbBox);
            }
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            /* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            dgvExcel.DataSource = dtDailyMuster;
            //return;
            dgvExcel.AllowUserToAddRows = false;
            dgvExcel.Columns.Clear();
            dgvExcel.RowTemplate.Height = 18;
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.HeaderText = "Approve";
            check.DataPropertyName = "Approve";
            check.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            check.ReadOnly = true;
            check.Name = "Approve";

            dgvExcel.Columns.Add(check);

            DataGridViewTextBoxColumn text = new DataGridViewTextBoxColumn();
            text.HeaderText = "Employee Code";
            text.DataPropertyName = "EmployeeCode";
            text.Name = "EmployeeCode";
            text.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            text.Width = 110;
            dgvExcel.Columns.Add(text);

            DataGridViewTextBoxColumn txtEmpName = new DataGridViewTextBoxColumn();
            txtEmpName.HeaderText = "Employee Name";
            txtEmpName.DataPropertyName = "EmployeeName";
            txtEmpName.Name = "EmployeeName";
            txtEmpName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtEmpName.Width = 110;
            dgvExcel.Columns.Add(txtEmpName);
            //for (int i = 1; i <= 31; i++)
            //{
            //    text = new DataGridViewTextBoxColumn();
            //    text.HeaderText = "Day" + i.ToString().Trim();
            //    text.DataPropertyName = "Day" + i.ToString().Trim();
            //    text.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //    dataGridView1.Columns.Add(text);
            //}
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            DataGridViewComboBoxColumn cmbBox;
            for (int i = 1; i <= vMonthDays; i++)
            {
                cmbBox = new DataGridViewComboBoxColumn();
                foreach (DataRow drt in tblAttSett.Rows)
                    cmbBox.Items.Add(drt["Att_Nm"].ToString());
                cmbBox.HeaderText = "Day" + i.ToString().Trim();
                cmbBox.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                cmbBox.DataPropertyName = "Day" + i.ToString().Trim();
                cmbBox.Name = "Day" + i.ToString().Trim();
                cmbBox.Width = 100;
                //cmbBox.
                dgvExcel.Columns.Add(cmbBox);
            }

            //DataGridViewTextBoxColumn txtEmpName = new DataGridViewTextBoxColumn();
            //txtEmpName.HeaderText = "Employee Name";
            //txtEmpName.DataPropertyName = "EmployeeName";
            //txtEmpName.Name = "EmployeeName";
            //txtEmpName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //txtEmpName.Width = 110;
            //dgrApprov.Columns.Add(txtEmpName);

            DataGridViewTextBoxColumn txtSupervisor = new DataGridViewTextBoxColumn();
            txtSupervisor.HeaderText = "Supervisor";
            txtSupervisor.DataPropertyName = "supervisor";
            txtSupervisor.Name = "supervisor";
            txtSupervisor.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtSupervisor.Width = 110;
            dgvExcel.Columns.Add(txtSupervisor);
            //**********************
            DataGridViewTextBoxColumn txtMonthDays = new DataGridViewTextBoxColumn();
            txtMonthDays.HeaderText = "MonthDays";
            txtMonthDays.DataPropertyName = "MonthDays";
            txtMonthDays.Name = "MonthDays";
            txtMonthDays.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtMonthDays.Width = 110;
            dgvExcel.Columns.Add(txtMonthDays);

            DataGridViewTextBoxColumn txtWeekOff = new DataGridViewTextBoxColumn();
            txtWeekOff.HeaderText = "Tot. Week Offs";
            txtWeekOff.DataPropertyName = "WO";
            txtWeekOff.Name = "WO";
            txtWeekOff.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtWeekOff.Width = 110;
            dgvExcel.Columns.Add(txtWeekOff);

            DataGridViewTextBoxColumn txtHoliDay = new DataGridViewTextBoxColumn();
            txtHoliDay.HeaderText = "Holidays";
            txtHoliDay.DataPropertyName = "HD";
            txtHoliDay.Name = "HD";
            txtHoliDay.ValueType = typeof(decimal);
            txtHoliDay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtHoliDay.Width = 110;
            dgvExcel.Columns.Add(txtHoliDay);

            
            DataRow[] tempx = tblAttSett.Select("isleave=1");
            if (tempx.Length > 0)
            {
                DataGridViewTextBoxColumn txtLeave;
                foreach (DataRow drAttSett in tempx)
                {
                    txtLeave = new DataGridViewTextBoxColumn();
                    txtLeave.HeaderText = drAttSett["Att_Code"].ToString().Trim() + " Balance"; //"Supervisor";
                    txtLeave.DataPropertyName = drAttSett["Att_Code"].ToString().Trim() + "_Balance";
                    txtLeave.Name = drAttSett["Att_Code"].ToString().Trim() + "_Balance";
                    txtLeave.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                    txtLeave.Width = 110;
                    dgvExcel.Columns.Add(txtLeave);
                }
            }

            //**********************
            XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX*/
            /// For Header Colour change ---->
            //dgrApprov.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGray;
            dgvExcel.EnableHeadersVisualStyles = false;
            /// For Header Colour change <----
            dgvExcel.RowsDefaultCellStyle.Font = new Font("Arial", 8.0F);
        }
        #endregion

        

        #region Controls Enable Desable
        private void ControlsEnable()
        {
            vEnable = false;
            if (this.pAddMode | this.pEditMode)
            {
                //btnEmail.Enabled = false;
                //btnPreview.Enabled = false;
                //btnPrint.Enabled = false;
                btnEdit.Enabled = false;
                sbExcelFile.Enabled = true;
                chkSelectAll.Enabled = true;
                if (dgvExcel.Rows.Count > 0)
                    dgvExcel.Columns[0].ReadOnly = false;
            }
            else
            {
                //if (dtDailyMuster!=null && dtDailyMuster.Rows.Count > 0)
                //{
                //    btnEmail.Enabled = true;
                //    btnPreview.Enabled = true;
                //    btnPrint.Enabled = true;
                //}

                
                if (dtDailyMuster != null && dtDailyMuster.Rows.Count > 0) // Added for Bug-25818 By Kishor A.
                
                {
                    btnEmail.Enabled = true; // Added for Bug-25818 By Kishor A.
                    btnPreview.Enabled = true; // Added for Bug-25818 By Kishor A.
                    btnPrint.Enabled = true; // Added for Bug-25818 By Kishor A.
                    btnEdit.Enabled = true;
                }
                else
                {
                    btnPreview.Enabled = false; // Added for Bug-25818 By Kishor A.
                }

                btnSave.Enabled = false;
                btnNew.Enabled = false;
                btnCancel.Enabled = false;
                sbExcelFile.Enabled = false;
                chkSelectAll.Enabled = false;
                if (dgvExcel.Rows.Count>0)


                    for (int i = 1; i <= vMonthDays; i++)
                        dgvExcel.Columns["Day" + i.ToString().Trim()].ReadOnly = true;

                
            }
            foreach (object cm in dgvExcel.Columns)
                if (cm.GetType().Name == "DataGridViewComboBoxColumn")
                {
                    ((DataGridViewComboBoxColumn)cm).DisplayStyle = (((DataGridViewComboBoxColumn)cm).ReadOnly == true ? DataGridViewComboBoxDisplayStyle.Nothing : DataGridViewComboBoxDisplayStyle.ComboBox);
                }

        }
        #endregion

        #region Main Application running check
        private void mcheckCallingApplication()
        {
            Process pProc;
            Boolean procExists = true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this.pPApplPID));
                String pName = pProc.ProcessName;
                string pName1 = this.pPApplName.Substring(0, this.pPApplName.IndexOf("."));
                if (pName.ToUpper() != pName1.ToUpper())
                {
                    procExists = false;
                }
            }
            catch (Exception)
            {
                procExists = false;
            }
            if (procExists == false)
            {
                MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }
        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udAttendanceIntegration.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "set Dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }
        #endregion

        #region Local Fuctions
        private Decimal fnAttCodeTot(string vvAttNm, DataRow vdrAttInt)
        {
            Decimal vAttCodeTot = 0;
            string vColNm = "";
            for (Decimal i = 1; i <= vMonthDays; i++)
            {
                vColNm = "Day"+i.ToString().Trim();
                if (vdrAttInt[vColNm].ToString().ToLower().Trim() == vvAttNm)
                {
                    vAttCodeTot = vAttCodeTot + 1;
                }
            }
            return vAttCodeTot;
        }
        private int fnNMonth(string mn)
        {
            int nmnth = 0;
            switch (mn)
            {
                case "January":
                    nmnth = 1;
                    break;
                case "February":
                    nmnth = 2;
                    break;
                case "March":
                    nmnth = 3;
                    break;
                case "April":
                    nmnth = 4;
                    break;
                case "May":
                    nmnth = 5;
                    break;
                case "June":
                    nmnth = 6;
                    break;
                case "July":
                    nmnth = 7;
                    break;
                case "August":
                    nmnth = 8;
                    break;
                case "September":
                    nmnth = 9;
                    break;
                case "October":
                    nmnth = 10;
                    break;
                case "November":
                    nmnth = 11;
                    break;
                case "December":
                    nmnth = 12;
                    break;
            }
            return nmnth;
        }
        private string fnNMonth(int mn)
        {
            string nmnth = string.Empty;
            switch (mn)
            {
                case 1:
                    nmnth = "January";
                    break;
                case 2:
                    nmnth = "February";
                    break;
                case 3:
                    nmnth = "March";
                    break;
                case 4:
                    nmnth = "April";
                    break;
                case 5:
                    nmnth = "May";
                    break;
                case 6:
                    nmnth = "June";
                    break;
                case 7:
                    nmnth = "July";
                    break;
                case 8:
                    nmnth = "August";
                    break;
                case 9:
                    nmnth = "September";
                    break;
                case 10:
                    nmnth = "October";
                    break;
                case 11:
                    nmnth = "November";
                    break;
                case 12:
                    nmnth = "December";
                    break;
            }
            return nmnth;
        }
        private string GetAttCode(string AttCode, bool IsCode)
        {
            //SqlStr = "Select Att_Nm,Att_Code,isleave,h_Att_Code,h_Att_Nm From Emp_Attendance_Setting where LDeactive=0 and att_code not in ('MonthDays','SalPaidDays') order by SortOrd";
            if (String.IsNullOrEmpty(AttCode))
                return "";
            string Expression = string.Empty;
            if (IsCode)
                Expression = "Att_Code='" + AttCode + "'";
            else
                Expression = "Att_nm='" + AttCode + "'";

            DataRow[] tempx = tblAttSett.Select(Expression);
            if (IsCode)
                return tempx.Length > 0 ? tempx[0][0].ToString() : "";
            else
                return tempx.Length > 0 ? tempx[0][1].ToString() : "";
        }

        private string GetAttName(string AttCode)
        {
            string Expression = string.Empty;
            Expression = "Att_Code='" + AttCode + "'";
            DataRow[] tempx = tblAttSett.Select(Expression);
            return tempx.Length > 0 ? tempx[0][1].ToString() : "";
        }

        private string GetAttCode(string AttName)
        {
            string xAttNm;
            xAttNm = GetHalfDayCode(AttName);
            // MessageBox.Show(xAttNm);
            if (!String.IsNullOrEmpty(xAttNm))
                return xAttNm;
            string Expression = string.Empty;
            Expression = "Att_nm='" + AttName + "'";
            DataRow[] tempx = tblAttSett.Select(Expression);
            return tempx.Length > 0 ? tempx[0][1].ToString() : "";
        }
        private string GetHalfDayName(string AttCode)
        {
            string Expression = string.Empty;
            if (AttCode.Trim().Length > 3)
                Expression = "Att_Nm='" + AttCode + "'";
            else
                Expression = "Att_Code='" + AttCode + "'";
            DataRow[] tempx = tblAdvAttSett.Select(Expression);
            return tempx.Length > 0 ? tempx[0][1].ToString() : "";
        }
        private string GetHalfDayCode(string AttName)
        {
            string Expression = string.Empty;
            if (AttName.Trim().Length > 3)
                Expression = "H_Att_Nm='" + AttName + "'";
            else
                Expression = "H_Att_Code='" + AttName + "'";
            DataRow[] tempx = tblAdvAttSett.Select(Expression);
            Expression = string.Empty;
            if (tempx.Length > 0)
                Expression = tempx[0][0].ToString();
            //return tempx.Length > 0 ? tempx[0][0].ToString() : "";
            return tempx.Length > 0 ? Expression : "";
        }
        
        #endregion

        private void btnMonth_Click(object sender, EventArgs e)
        {
            //controlVal = 1; 
           //retVal= mthValidateTextBox(controlVal);
           //if (retVal == 1)
           //{
           //    return;
           //}
            sqlQuery = "select DateName( Month , DateAdd( month , cast(Pay_Month as int), 0 )-1  ) as [Month] from emp_Processing_month where isclosed!=1";

            tDs = oDataAccess.GetDataSet(sqlQuery, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select distinct  Month";
            vSearchCol = "Month";
            vDisplayColumnList = "Month:Month";
            vReturnCol = "Month";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                //    mthBindClear();
                //}
                this.txtProcMonth.Text = oSelectPop.pReturnArray[0];
            }
            // <--------Added by Pratap on  :13-09-2012----------->
            mthColumnsSize();


        }
        
        #region Look and Feel
        private void frmAttendanceIntegration_Resize(object sender, EventArgs e)
        {
            //tabControl1.Height = this.Height - (groupBox1.Height + 60);
            dgvExcel.Height = this.Height - (groupBox1.Height + 60);
            //dgvExcel.Height = tabPage1.Height - 10;
        }
        private void SetFormColor()
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(strSQL, null, vTimeOut);
            if (dsColor != null)
            {
                if (dsColor.Tables.Count > 0)
                {
                    dsColor.Tables[0].TableName = "ColorInfo";
                    colorCode = dsColor.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);

        }
        #endregion
        
        #region Print And Email 
        private void mthPrint(Int16 vPrintOption)
        {
            string vRepGroup = "Monhly Attendance Reader";
            udReportList.cReportList oPrint = new udReportList.cReportList();
            oPrint.pDsCommon = this.vDsCommon; //Commented By Amrendra
            oPrint.pServerName = this.pServerName;
            oPrint.pComDbnm = this.pComDbnm;
            oPrint.pUserId = this.pUserId;
            oPrint.pPassword = this.pPassword;
            oPrint.pAppPath = this.pAppPath;
            oPrint.pPApplText = this.pPApplText;
            oPrint.pPara = this.pPara;
            oPrint.pRepGroup = vRepGroup;
            //oPrint.pTran_Cd = Convert.ToInt16(this.vTran_Cd);//Commented By Amrendra
            oPrint.pSpPara = "'" + this.txtProcYear.Text.Trim() + "','" + fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "','" + this.pAppUerName+"'";
            oPrint.pPrintOption = vPrintOption;
            oPrint.Main();
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {
            
            this.mthPrint(7);
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.mthPrint(2);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.mthPrint(3);
        }
        #endregion

        //Added By Amrendra On 03-07-2012

        private void sbProcessingYear_Click(object sender, EventArgs e)
        {
            //sqlQuery = "select  Pay_Year from emp_Processing_month where isclosed!=1"; // Commented By Amrendra on 03-07-2012
            sqlQuery = "select distinct Pay_Year from emp_Processing_month where isclosed!=1"; // Changed By Amrendra on 03-07-2012

            tDs = oDataAccess.GetDataSet(sqlQuery, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select distinct  pYear";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Year";
            vReturnCol = "Pay_Year";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtProcYear.Text = oSelectPop.pReturnArray[0];
            }
            if (!string.IsNullOrEmpty(this.txtProcMonth.Text) & !string.IsNullOrEmpty(this.txtProcYear.Text))
            {
                loadLatestData();
                ControlsEnable();
                this.SetFormColor();
            }
            // <--------Added by Pratap on  :13-09-2012----------->
            mthColumnsSize();
        }        
        
        #region Not Required this code
        private void mthSavePayDet1(ref string vSaveString, string tblTarget, DataRow dr)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + tblTarget + "' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }
            /*<---Identity Columns*/

            if ((Boolean)dr["Upload"] == false && this.pEditMode)
            {
                SqlStr = "Delete From " + tblTarget + " where Pay_year='" + this.txtProcYear.Text.Trim() + "' and Pay_Month=" + this.fnNMonth(this.txtProcMonth.Text) + " and EmployeeCode='" + dr["EmployeeCode"].ToString() + "'";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }

            sqlstr = "Select * From " + tblTarget + " where 1=2";
            DataSet tDsPay = new DataSet();
            tDsPay = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);

            string vEmpCode = string.Empty;
            string vColValue = this.txtProcYear.Text + "," + this.fnNMonth(this.txtProcMonth.Text).ToString();
            string vFld_Nm = string.Empty;
            vSaveString = "";
            string vFldNM = "";
            if (Convert.ToDecimal(dr["Tran_Cd"]) == 0) //New Record
            {
                dr["Tran_Cd"] = vTran_Cd;
                vSaveString = "Set DateFormat dmy insert into " + tblTarget;
                //dsSave.Tables[0].AcceptChanges();



                foreach (DataColumn dtc1 in tDsPay.Tables[0].Columns)
                {
                    vFldNM = dtc1.ColumnName.Trim();
                    if (vIdentityFields.IndexOf("#" + vFldNM + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + "[" + vFldNM + "]";
                        vfldVal = dr[vFldNM].ToString().Trim();

                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if ((dtc1.DataType.Name.ToLower() == "decimal" || dtc1.DataType.Name.ToLower() == "int16" || dtc1.DataType.Name.ToLower() == "int32") && string.IsNullOrEmpty(vfldVal))
                        {
                            vfldVal = "0";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime") //Alert Master
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean") //Alert Master
                        {
                            if (vfldVal.ToLower() == "true")
                            {
                                vfldVal = "1";
                            }
                            else
                            {
                                vfldVal = "0";
                            }

                        }

                        vfldValList = vfldValList + vfldVal;
                    }
                }

                vSaveString = vSaveString + " (" + vfldList + ") Values (" + vfldValList + ")";

            }
            else
            //if (Convert.ToDecimal(dr["Tran_Cd"]) != 0) //Edit Record
            {
                //dsGrd.Tables[0]
                //string vControl = string.Empty;

                //foreach (DataRow dr in dsGrd.Tables[0].Rows)
                //{
                vSaveString = " PayGenerated=" + (dr["PayGenerated"].ToString().Trim() == "True" ? "1" : "0");
                vSaveString = vSaveString + "," + " NetPayment=" + dr["NetPayment"].ToString().Trim();
                vSaveString = vSaveString + "," + "Tran_Cd=" + vTran_Cd.ToString().Trim();
                vSaveString = vSaveString + "," + " Narr='" + dr["Narr"].ToString().Trim() + "'";
                //foreach (DataRow tdr in this.dsEDMaster.Tables[0].Rows)
                for (int iCountx = 1; iCountx <= vMonthDays; iCountx++)
                {
                    vFld_Nm = "Day" + iCountx.ToString().Trim();
                    vSaveString = vSaveString + "," + vFld_Nm;
                    vSaveString = vSaveString + "=" + dr[iCountx.ToString().Trim()].ToString();
                }
                vSaveString = vSaveString.Trim();
                vSaveString = "Update P set " + vSaveString + " From Emp_Monthly_Payroll P ";
                vSaveString = vSaveString + " Left Join EmployeeMast E on (E.EmployeeCode=P.EmployeeCode) ";
                vSaveString = vSaveString + " Left Join Loc_Master Lc on (Lc.Loc_Code=E.Loc_Code) ";
                vSaveString = vSaveString + " Where P.Pay_Year=" + this.txtProcYear.Text + " and P.Pay_Month=" + this.fnNMonth(txtProcMonth.Text).ToString();
                vSaveString = vSaveString + " and E.EmployeeCode='" + dr["EmployeeCode"].ToString().Trim() + "'";
            }

        }
        private bool validation()
        {
            bool ValidationResult = true;
            var groupdata = from x in dsData.Tables["AttendanceIntegration"].AsEnumerable()
                            group x by new
                            {
                                EmployeeCode = x.Field<string>(dsData.Tables["AttendanceIntegration"].Columns["Employee Code"]),
                                EmployeeName = x.Field<string>(dsData.Tables["AttendanceIntegration"].Columns["Employee Name"])
                            } into g
                            where g.Count() > 1
                            select new
                            {
                                EmployeeName = g.Key.EmployeeName,
                                EmployeeCode = g.Key.EmployeeCode,
                                Count = g.Count()
                            };
            string msgstr = String.Empty;
            foreach (var i in groupdata)
            {
                msgstr = msgstr + i.EmployeeName + "(" + i.EmployeeCode + ") " + i.Count.ToString() + " Entries found." + "\n";
                ValidationResult = false;
            }
            if (msgstr != String.Empty)
                MessageBox.Show("Following Employees has more than one attendance record:\n\n" + msgstr);
            return ValidationResult;
        }
        private void RenderDisplayTable()
        {
            dtDisplay = null;
            dtDisplay = new DataTable();
            foreach (DataColumn dc in dtDailyMuster.Columns)
            {
                DataColumn newDc = new DataColumn();
                newDc.DataType = dc.DataType;
                newDc.ColumnName = dc.ColumnName;
                dtDisplay.Columns.Add(newDc);

            }
            foreach (DataRow dr in dtDailyMuster.Rows)
            {
                DataRow drDisplayNew = dtDisplay.NewRow();
                int DayCaount = 1;
                foreach (DataColumn dc in dtDailyMuster.Columns)
                {
                    foreach (DataRow drt in tblAttSett.Rows)
                    {
                        if (dc.ColumnName.ToLower() == (drt["Att_code"].ToString().Trim() + "_Balnace").ToLower())
                        {
                            drDisplayNew[dc] = dr[dc];
                        }
                    }

                    switch (dc.ColumnName)
                    {
                        case "ID":
                            drDisplayNew["ID"] = dr[dc];
                            break;
                        case "EmployeeCode":
                            drDisplayNew["EmployeeCode"] = dr[dc];
                            break;
                        case "EmployeeName":
                            drDisplayNew["EmployeeName"] = dr[dc];
                            break;
                        case "supervisor":
                            drDisplayNew["supervisor"] = dr[dc];
                            break;
                        case "Pay_Year":
                            drDisplayNew["Pay_Year"] = this.txtProcYear.Text.Trim();
                            break;
                        case "Pay_Month":
                            drDisplayNew["Pay_Month"] = this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString();
                            break;
                        case "Upload":
                            drDisplayNew["Upload"] = dr["Upload"];
                            break;
                        case "Approve":
                            drDisplayNew["Approve"] = dr["Approve"];
                            break;
                        default:
                            if (dc.ColumnName.Length > 2)
                                if (dc.ColumnName.Substring(0, 3).ToUpper() == "DAY")
                                {
                                    //DataRow[] tempx = tblAttSett.Select("Att_nm='" + dr[DayCaount.ToString()] + "'");
                                    DayCaount++;
                                    //drDisplayNew[dc] = tempx[0][1];
                                    drDisplayNew[dc.ColumnName] = GetAttName (dr[dc].ToString());
                                }
                            break;
                    }

                }
                dtDisplay.Rows.Add(drDisplayNew);

            }
            ColumnBindToGrid();
            //dataGridView1.DataSource = dtDisplay;
            //dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void teProcessingMonth_TabIndexChanged(object sender, EventArgs e)
        {
            if (sender == txtProcMonth)
            {
                controlVal = 1;
                //retVal = mthValidateTextBox(controlVal);
                if (retVal == 1)
                {
                    return;
                }
            }
        }
        
        #endregion

        #region Save with Validation
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            dgvExcel.EndEdit();

            if (!mthValid())
                return;

            foreach (DataRow dr in dtDailyMuster.Rows)
            {
//                if (this.pEditMode || (Boolean)dr["Upload"] == true) 
                {
                    SqlStr = "";
                    this.mthSaveString(ref SqlStr, "Emp_Daily_Muster", dr);
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
            }
            ///Not Requied in AttendaceIntegration but required in Approval----->
            //SqlStr = "Execute Usp_Ent_Update_Monthly_Muster_As_Per_Daily_Muster '" + this.txtProcYear.Text.Trim()+ "',"+ this.fnNMonth(this.txtProcMonth.Text.Trim());
            //oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            ///Not Requied in AttendaceIntegration but required in Approval<-----
            #region 
            //foreach (DataRow dr in dtDailyMuster.Rows)
            //{
            //    if (this.pEditMode || (Boolean)dr["Upload"] == true)
            //    {
            //        SqlStr = "";
            //        this.mthSaveString(ref SqlStr, "Emp_Daily_Muster", dr);
            //        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            //    }
            //}

     
            //btnEdit.Enabled = true;
            //btnSave.Enabled = false;
            //btnEmail.Enabled = true;
            //btnPreview.Enabled = true;
            //btnPrint.Enabled = true;
                       
            #endregion
            this.pAddMode = false;
            this.pEditMode = false;
            loadLatestData();
            ControlsEnable();
            if (dgvExcel.Rows.Count > 0)
                dgvExcel.Columns[0].ReadOnly = true;
            // <--------Added by Pratap on  :13-09-2012----------->
            mthColumnsSize();
            
        }

        private void mthSaveString(ref string vSaveString, string tblTarget, DataRow dr)
        {
            string vfldList = string.Empty, vFld_Nm = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;
            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + tblTarget + "' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }
            /*<---Identity Columns*/
            this.pEditMode = true;
            //if ((Boolean)dr["Upload"] == false && this.pEditMode)
            //{
            //    SqlStr = "Delete From " + tblTarget + " where Pay_year='" + this.txtProcYear.Text.Trim() + "' and Pay_Month=" + this.fnNMonth(this.txtProcMonth.Text) + " and EmployeeCode='" + dr["EmployeeCode"].ToString() + "'";
            //    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            //}
            string xx = "Select EmployeeCode from " + tblTarget + " where employeeCode='" + dr["EmployeeCode"].ToString().Trim() + "' ";
            xx = xx + " and Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
            xx = xx + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            DataTable dtX = oDataAccess.GetDataTable(xx, null, vTimeOut);

            if ((Boolean)dr["Upload"] == true && dtX.Rows.Count == 0)
            {
                vSaveString = "Set DateFormat dmy insert into " + tblTarget;
                vfldList = "EmployeeCode,Pay_Year,Pay_Month,SysDate,User_Name,Upload";
                vfldValList = "'" + dr["EmployeeCode"].ToString().Trim() + "'";
                vfldValList = vfldValList + ",'" + this.txtProcYear.Text.Trim() + "'";
                vfldValList = vfldValList + "," + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim();
                vfldValList = vfldValList + ",'" + DateTime.Now.Date.ToString().Trim() + "'";
                vfldValList = vfldValList + ",'" + this.pAppUerName.Trim() + "'";
                vfldValList = vfldValList + ",1";

                for (int iCountx = 1; iCountx <= vMonthDays; iCountx++)
                {
                    vFld_Nm = "Day" + iCountx.ToString().Trim();
                    vfldList = vfldList + "," + vFld_Nm;
                    vfldValList = vfldValList + ",'" + GetAttCode(dr[vFld_Nm].ToString()) + "'";
                }
                vSaveString = vSaveString + "(" + vfldList + " ) Values (" + vfldValList + ")";
            }
            else
            {
               
                vSaveString = "Set DateFormat dmy Update " + tblTarget + " Set";
                vSaveString = vSaveString + " SysDate='" + DateTime.Now.Date.ToString().Trim() + "'";
                vSaveString = vSaveString + ",User_Name='" + this.pAppUerName.Trim() + "'";
                vSaveString = vSaveString + ",Upload=" + ((Boolean)dr["Upload"] == false ? "0" : "1");
                for (int iCountx = 1; iCountx <= vMonthDays; iCountx++)
                {
                    vFld_Nm = "Day" + iCountx.ToString().Trim();
                    vSaveString = vSaveString + "," + vFld_Nm;
                    vSaveString = vSaveString + "='" + ((Boolean)dr["Upload"] == false ? "" : GetAttCode(dr[vFld_Nm].ToString())) + "'";
                }

                //Added By Kishor A. for Bug-26387 on 13/07/2015 Start...

                if (xNmCount == dr.Table.Rows.Count)
                {
                    xNmCount = 0;
                }
                if (xNmCount < dr.Table.Rows.Count)
  
                {
                    if ((Boolean)dr["Upload"] == true)
                    {
                        vSaveString = vSaveString + "," + "OTH" + "=" + dr.Table.Rows[xNmCount]["OTH"];
                        vSaveString = vSaveString + "," + "HOTH" + "=" + dr.Table.Rows[xNmCount]["HOTH"];
                    }
                    else
                    {
                        vSaveString = vSaveString + "," + "OTH" + "=" +0 ;
                        vSaveString = vSaveString + "," + "HOTH" + "=" +0;
                    }
                }
                //Added By Kishor A. for Bug-26387 on 13/07/2015 End...

                vSaveString = vSaveString + " where employeeCode='" + dr["EmployeeCode"].ToString().Trim() + "'";
                vSaveString = vSaveString + " and Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
                vSaveString = vSaveString + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            }
            xNmCount++; //Added By Kishor A. for Bug-26387 on 13/07/2015 
        }

        #region Validation
        public void mthValidateTextBox()
        {
            ErrorStr = "";
            if (txtProcMonth.Text == "" )
            {
                IsError = true;
                ErrorStr = ErrorStr+"\nProcessing Month should not be empty.";
            }
            if (txtProcYear.Text == "" )
            {
                IsError = true;
                ErrorStr = ErrorStr + "\nProcessing Year should not be empty.";
            }
            if (txtMuster.Text == "" )
            {
                IsError = true;
                ErrorStr = ErrorStr + "\nMuster Type should not be empty.";                
            }
        }
        private void mthValidateAttendance()
        {
            string vAttCode = "", vAttNm = "";
            ErrorStr="";
            int RowCounter = 0;
            foreach (DataRow drMunthlyMuster in dtDailyMuster.Rows)
            {
                decimal  totleave = 0,TotWeekoffs=0,TotHolyDay=0;
                foreach (DataRow drAttSett in tblAttSett.Select("ISLEAVE=1")) // Leave Exceeds or Not
                {
                    vAttNm = drAttSett["Att_Nm"].ToString().ToLower().Trim();
                    totleave = fnAttCodeTot(vAttNm, drMunthlyMuster);
                    if (totleave > Convert.ToDecimal(drMunthlyMuster[drAttSett["Att_Code"].ToString().ToLower().Trim() + "_Balance"]))
                    {
                        IsError = true;
                        ErrorStr = ErrorStr + drMunthlyMuster["EmployeeName"].ToString().Trim() + " : "+drAttSett["Att_nm"].ToString().Trim() + " Balance"+" Exceeds than the available.\n";
                        dgvExcel.Rows[RowCounter].Selected = true;
                    }
                }

                vAttNm = "Weekly Off".ToLower();
                TotWeekoffs = fnAttCodeTot(vAttNm, drMunthlyMuster);
                if (TotWeekoffs > Convert.ToDecimal(drMunthlyMuster["WO"]))
                {
                    IsError = true;
                    ErrorStr = ErrorStr + drMunthlyMuster["EmployeeName"].ToString().Trim() + " : Week offs Exceeds than the available.\n";
                    dgvExcel.Rows[RowCounter].Selected = true;
                }
                vAttNm = "Holiday".ToLower();
                TotWeekoffs = fnAttCodeTot(vAttNm, drMunthlyMuster);
                if (TotWeekoffs > Convert.ToDecimal(drMunthlyMuster["HD"]))
                {
                    IsError = true;
                    ErrorStr = ErrorStr + drMunthlyMuster["EmployeeName"].ToString().Trim() + " : Holidays Exceeds than the available.\n";
                    dgvExcel.Rows[RowCounter].Selected = true;
                }
                if (IsError)
                    break;
                RowCounter++;
            }
        }
        private bool mthValid()
        {
            IsError = false;
            mthValidateTextBox(); // Test Box Validations
            if (IsError)
            {
                MessageBox.Show(ErrorStr);
                return false;
            }
            //else
            //{
            //    mthValidateAttendance(); // Leave and Offs Validation
            //    if (IsError)
            //    {
            //        MessageBox.Show(ErrorStr);
            //        return false;
            //    }
            //    else
            //    {
                    return true;
            //    }
            //}
        
        }
        #endregion

        #endregion


        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            btnNew.Enabled = false;
            btnEdit.Enabled = false;
            //btnEmail.Enabled = false;
            //btnPreview.Enabled = false;
            //btnPrint.Enabled = false;

            this.pAddMode = false ;
            this.pEditMode = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true ;
            ControlsEnable();
            if (dgvExcel.Rows.Count > 0)
            {
                dgvExcel.Columns[0].ReadOnly = false;
                for (int i = 1; i <= vMonthDays; i++)
                    dgvExcel.Columns["Day" + i.ToString().Trim()].ReadOnly = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //btnEdit.Enabled = true;
            //btnSave.Enabled = false;
            //btnCancel.Enabled = false;
            //btnEmail.Enabled = true;
            //btnPreview.Enabled = true;
            //btnPrint.Enabled = true;
            //loadLatestData();
            dtDailyMuster.RejectChanges();
            this.pAddMode = false;
            this.pEditMode = false;
            ControlsEnable();
            if (dgvExcel.Rows.Count > 0)
                dgvExcel.Columns["Upload"].ReadOnly = true ;
            lcProcessingMonth.Focus(); 

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
       
            Application.Exit();
        }

        #region Grid Check box Click

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //MessageBox.Show(e.RowIndex.ToString());
            //MessageBox.Show(dtDailyMuster.Rows[e.RowIndex]["Approve"].ToString());
            if(e.RowIndex>=0 && dgvExcel.Rows.Count>0 && this.pEditMode == true)
            if ((bool)dtDailyMuster.Rows[e.RowIndex]["Approve"] && this.pEditMode == true)
            {
                dgvExcel.CurrentRow.ReadOnly = true;
                dgvExcel.CancelEdit();
            }

            //if (dgvExcel.CurrentCell is DataGridViewCheckBoxCell && this.pEditMode == true)
            //{

            //    bool IsValidForApproval = true;
            //    foreach (DataGridViewCell cell in dgvExcel.CurrentRow.Cells)
            //    {
                    
            //        if (string.IsNullOrEmpty(cell.Value.ToString()) | cell.Value.ToString().Contains("Not"))
            //        {
            //            MessageBox.Show("Attendance Missing...");
            //            IsValidForApproval = false;
            //            break;
            //        }
            //    }

            //    if (!IsValidForApproval && (bool)dgvExcel["Approve", e.RowIndex].Value == false)
            //        dgvExcel.CancelEdit();
            //    else
            //        dgvExcel.EndEdit();
            //}
            
        }
        private void dgrApprov_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //dgvExcel.CancelEdit();
        }
        #endregion 

        private void sbExcelFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "Excel files (*.xls)|*.xls|Excel files(2003 or later) (*.xlsx)|*.xlsx";
            ofDialog.ShowDialog();
            this.teExcelFile.Text = ofDialog.FileName;
            if (!string.IsNullOrEmpty(teExcelFile.Text))
                btnGetExcel.Enabled = true;
            else
                btnGetExcel.Enabled = false;

        }

        private void btnGetExcel_Click(object sender, EventArgs e)
        {
            if (teExcelFile.Text == "")
            {
                MessageBox.Show("\nPlease Specify an Excel File First.");
                return;
            }

            //if (IsFetched)
            //    dgvExcel.Rows.Clear();

            dtMonthlyMuster = new DataTable();
            vFileName = this.teExcelFile.Text;
            vYear = this.txtProcYear.Text;
            vMonth = this.txtProcMonth.Text;

            SqlStr = "Select EmployeeCode,upload from Emp_daily_Muster where Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
            SqlStr = SqlStr + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            dtUploadStatus = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            //OleDbConnection conExcel = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + vFileName + ";Extended Properties=Excel 12.0"); //Commented By Amrendra On 05/12/2012
            OleDbConnection conExcel=null;
            if (Path.GetExtension(vFileName).ToUpper()==".XLS")
                conExcel = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + vFileName + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"");//Added By Amrendra On 05/12/2012
            else if (Path.GetExtension(vFileName).ToUpper()==".XLSX")
                conExcel = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + vFileName + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"");//Added By Amrendra On 05/12/2012
            if (conExcel == null)
            {
                MessageBox.Show("\nUnable to connect to excel.");
                return;
            }
            OleDbDataAdapter daExcel = new OleDbDataAdapter("Select * from [" + "Sheet1" + "$] where [Enter Year]=" + this.txtProcYear.Text.Trim() + " and [Enter Month]='" + this.txtProcMonth.Text.Trim() + "' order by [Enter employee ID],[timestamp] desc", conExcel); // Excel Column Name
            daExcel.Fill(dsData, "Sheet1");
            OleDbDataAdapter daTExcel = new OleDbDataAdapter("Select min([Timestamp]) from [" + "Sheet1" + "$]", conExcel);

            daTExcel.Fill(dsData, "Temp");
            conExcel.Close();

            if (dtMonthlyMuster.Rows.Count == 0)
            {
                //MessageBox.Show(this.txtProcMonth.Text + " Process Month Creation not Created", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return;
            }
            MergeExcel(dsData.Tables["Sheet1"]); //Merging Excel Table

            int counter = 1;
            int DayCaount = 0;
            string CurrEmpCode = String.Empty;
            RenderDisplayTable();
            if (dgvExcel.Rows.Count > 0)
            {
                dgvExcel.Columns["Upload"].ReadOnly = false;
                dgvExcel.Columns["EmployeeCode"].ReadOnly = true ;
                dgvExcel.Columns["EmployeeName"].ReadOnly = true;

                for (int i = 1; i <= vMonthDays; i++)
                    dgvExcel.Columns["Day" + i.ToString().Trim()].ReadOnly = false;
            }
            //dgvExcel.DataSource = dtDisplay;// dtDailyMuster;
            dgvExcel.AllowUserToAddRows = false;

            // <--------Added by Pratap on  :13-09-2012----------->
            mthColumnsSize();
            //dataGridView1_CellContentClick(sender ,  e);
            ControlsEnable();
            
        }
        private void MergeExcel(DataTable ExcelTable)
        {
            DataRow rwDailyMuster;
            foreach (DataRow dr in ExcelTable.Rows)
            {
                rwDailyMuster = GetUploadRow(dr["Enter Employee ID"].ToString().Trim()); // Excel Column Name 
                if (rwDailyMuster != null)
                {
                    if (rwDailyMuster["Approve"].ToString().ToLower() != "true")
                    {
                        UpdateDayCol(rwDailyMuster, dr);
                    }
                }
            }
        }
        private void AddExcelRow(DataRow drNewDaily, DataRow drExcel)
        {
            int DayCaount = 1;
            DataRow drEmp = dtDailyMuster.NewRow();

            foreach (DataColumn dc in dtDailyMuster.Columns)
            {

                switch (dc.ColumnName)
                {
                    case "EmployeeCode":
                        drNewDaily[dc] = drExcel["Enter Employee ID"]; // Excel Column Name 
                        break;
                    case "Pay_Year":
                        drNewDaily[dc] = this.txtProcYear.Text.Trim();
                        break;
                    case "Pay_Month":
                        drNewDaily[dc] = this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString();
                        break;                    
                    case "Upload":
                        drNewDaily[dc] = false;
                        break;
                    default:
                        if (dc.ColumnName.Length > 2)
                            if (dc.ColumnName.Substring(0, 3).ToUpper() == "DAY")
                            {
                                DataRow[] tempx = tblAttSett.Select("Att_nm='" + drExcel[DayCaount.ToString()] + "'");
                                DayCaount++;
                                drNewDaily[dc] = tempx.Length > 0 ? tempx[0][1] : "NA";
                            }
                        break;
                }
                //    MessageBox.Show(dc.ColumnName);
            }


            if (drNewDaily["EmployeeCode"] != DBNull.Value)
            {
                //drEmp["ID"] = counter++;
                dtDailyMuster.Rows.Add(drNewDaily);
            }
        }
        private void UpdateDayCol(DataRow drDaily, DataRow drExcel)
        {

            string xNm = string.Empty;
            int DayCaount = 0;
            int xNmCount = 0;
            foreach (DataColumn dc in dtDailyMuster.Columns)
            {
                           
                DayCaount++;

                //Added By Kishor A. for Bug-26387 on 13/07/2015 Start....
                if (xNmCount < dc.Table.Rows.Count)
                {
                    xNm = dc.Table.Rows[xNmCount]["EmployeeCode"].ToString();
                    if (xNm == drExcel.Table.Rows[xNmCount]["Enter Employee ID"].ToString())
                    {
                        dc.Table.Rows[xNmCount]["OTH"] = drExcel.Table.Rows[xNmCount]["OT Hours"];
                        dc.Table.Rows[xNmCount]["HOTH"] = drExcel.Table.Rows[xNmCount]["HOT Hours"];
                    }
                    xNmCount++;
                }
                //Added By Kishor A. for Bug-26387 on 13/07/2015 End....

                if (dc.ColumnName.Length > 2)
                    if (dc.ColumnName.Substring(0, 3).ToUpper() == "DAY")
                    {
                        //DataRow[] tempx = tblAttSett.Select("Att_nm='" + drExcel[DayCaount.ToString()] + "'"); Commnted By Kishor A. for Bug-26387 on 13/07/2015

                        DataRow[] tempx = tblAttSett.Select("Att_nm='" + drExcel[DayCaount] + "'"); //Added By Kishor A. for Bug-26387 on 13/07/2015

                        //DayCaount++; Commnted By Kishor A. for Bug-26387 on 13/07/2015
                        //drDaily[dc] = tempx.Length > 0 ? tempx[0][1] : "NA";
                        drDaily[dc] = tempx.Length > 0 ? tempx[0][0] : "Not Available";

                        //    MessageBox.Show(dc.ColumnName);
                    }
            }
        }
        private DataRow GetUploadRow(string EmployeeCode)
        {
            if (dtDailyMuster != null && dtDailyMuster.Rows.Count > 0)
                foreach (DataRow dr in dtDailyMuster.Rows)
                    if (dr["Employeecode"].ToString().Trim() == EmployeeCode)
                        return dr;
            return null;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pEditMode == true)
            {
                if (chkSelectAll.Checked == true)
                {
                    chkSelectAll.Text = "Select None";
                    for (int i = 0; i < dgvExcel.Rows.Count; i++)
                    {
                        dgvExcel.Rows[i].Cells["Upload"].Value = true;
                    }
                }
                else if (chkSelectAll.Checked == false)
                {
                    chkSelectAll.Text = "Select All";
                    for (int i = 0; i < dgvExcel.Rows.Count; i++)
                    {
                        if ((bool)((DataTable)dgvExcel.DataSource).Rows[i]["Approve"] != true)
                            dgvExcel.Rows[i].Cells[0].Value = false;
                    }
                }
                dgvExcel.EndEdit();
            }
        }
        //  Added by Pratap. on 11-09-2012
        #region Short cut Keys for Editing,Saving and Undo 
        
        private void frmAttendanceIntegration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl+S Save
            {
                btnSave_Click(sender, e);
            }
            if (e.Control && e.KeyCode == Keys.Z)       //Ctrl+Z  Undo
            {
                btnCancel_Click(sender, e);
            }
            if (e.Control && e.KeyCode == Keys.E)       // Ctrl+E  Edit
            {
                btnEdit_Click(sender, e);
            }


        }
        
        #endregion
        // <---------Added by Pratap On:13-09-2012----------->
        #region Data Gridview columns size
        public void mthColumnsSize()
        {
            try
            {

                for (int i = 0; i <= vMonthDays + 2; i++)
                {
                    DataGridViewColumn columnSize = dgvExcel.Columns[i];
                    columnSize.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
            }

        } 
        #endregion


        private void mDeleteProcessIdRecord()/* Added Pratap 27/10/2012 */
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.btnNew.Enabled)
            //    btnNew_Click(this.btnNew, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.btnDelete.Enabled)
            //    btnDelete_Click(this.btnDelete, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }

        private void frmAttendanceIntegration_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

    }
}
