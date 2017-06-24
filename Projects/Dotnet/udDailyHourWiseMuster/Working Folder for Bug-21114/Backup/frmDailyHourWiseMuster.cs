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
using udReportList;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using udclsDGVNumericColumn;
using System.Runtime.InteropServices;
using ueconnect;
using GetInfo;

namespace udDailyHourWiseMuster
{
    public partial class frmDailyHourWiseMuster : uBaseForm.FrmBaseForm
    {
        String cAppPId, cAppName;
        int dgvCurRow = 0, dgvCurCol = 0;
        int controlVal = 0;
        int retVal = 0;
        string sqlQuery;
        bool b = true;
        bool vEnable = false;
        bool IsError = false;
        string ErrorStr = "";

        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
        string vMonth = string.Empty, vYear = string.Empty, vFileName = string.Empty, vTran_Cd = String.Empty;
        DataSet vDsCommon;
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
        short vTimeOut = 25;

        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;

        #region Initialization of Form with parameter
        public frmDailyHourWiseMuster(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;
            this.pPara = args;
            this.pFrmCaption = "Daily Hourwise Muster";
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
        private void frmDailyHourWiseMuster_Load(object sender, EventArgs e)
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
            }

            btnFirst.Enabled = false;
            btnBack.Enabled = false;
            btnForward.Enabled = false;
            btnLast.Enabled = false;
            btnEmail.Enabled = true;
            btnLocate.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            btnEdit.Enabled = true;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnPreview.Enabled = true;
            btnPrint.Enabled = true;
            btnExportPdf.Enabled = false;
            btnLogout.Enabled = true;
            btnHelp.Enabled = false;
            btnExit.Enabled = false;
            btnCalculator.Enabled = false;

            this.mInsertProcessIdRecord();
            this.mthDsCommon();
            loadLatestData();

            startupPath = Application.StartupPath;
            //startupPath = @"D:\Usquare10";          // To be Removed Sachin 
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
                ControlsEnable();
            this.SetFormColor();
            dgrApprov.Height = this.Height - (groupBox1.Height + 60);
            lcProcessingMonth.Focus();
        }
        #endregion


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

            SqlStr = "Select Att_Nm,Att_Code,isleave,h_Att_Code,h_Att_Nm From Emp_Attendance_Setting where LDeactive=0 and att_code not in ('MonthDays','SalPaidDays') order by SortOrd";
            tblAttSett = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            SqlStr = "Select distinct h_Att_Code,h_Att_Nm,Att_Code,Att_Nm from emp_attendance_setting where isnull(h_Att_Code,'')<>''";
            tblAdvAttSett = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataTable dtxx;
            SqlStr = "Select Max(Pay_year) From Emp_daily_Hourwise_Muster group by pay_month";
            dtxx = null;
            dtxx = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (dtxx.Rows.Count > 0)
            {
                this.txtProcYear.Text = dtxx.Rows[0][0].ToString();

                SqlStr = "Select Max(Pay_month) From Emp_daily_Hourwise_Muster where Pay_Year='" + dtxx.Rows[0][0].ToString() + "'group by pay_month";
                dtxx = null;
                dtxx = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (dtxx.Rows.Count > 0)
                    this.txtProcMonth.Text = this.fnNMonth(Convert.ToInt32(dtxx.Rows[0][0]));
            }
        }

        private int GetMonthDays(int lnMonth, string lsYear)
        {
            SqlStr = "Select top 1 MonthDays/(case when WrkHrs>0 then WrkHrs else 1 end) From Emp_Monthly_Muster where Pay_Year='" + lsYear.Trim() + "' and Pay_Month=" + lnMonth;
            DataTable dtxx = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (dtxx.Rows.Count > 0)
                return Convert.ToInt32(dtxx.Rows[0][0]);
            return 0;

        }
        #endregion

        #region Load Grid With Latest Data
        private void loadLatestData()
        {
            vMonthDays = GetMonthDays(this.fnNMonth(this.txtProcMonth.Text.Trim()), this.txtProcYear.Text.Trim());
            SqlStr = "Select EmployeeCode,upload from Emp_daily_Hourwise_Muster where Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
            SqlStr = SqlStr + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            dtUploadStatus = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            if (!string.IsNullOrEmpty(this.txtProcMonth.Text) & !string.IsNullOrEmpty(this.txtProcYear.Text))
            {
                SqlStr = "Execute Usp_Ent_Emp_Get_Daily_Hourwise " + this.txtProcYear.Text.Trim() + ", " + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim();
                dtDailyMuster = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                ColumnBindToGrid();
                if (dtDailyMuster.Rows.Count > 0)
                {
                    for (int i = 0; i < dgrApprov.Columns.Count; i++)
                        dgrApprov.Columns[i].ReadOnly = true;
                }
            }
        }
        #endregion

        #region Add Columns To the Grid
        private void ColumnBindToGrid()
        {
            dgrApprov.DataSource = dtDailyMuster;
            //return;
            dgrApprov.AllowUserToAddRows = false;
            dgrApprov.Columns.Clear();
            dgrApprov.RowTemplate.Height = 18;
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.HeaderText = "Approve";
            check.DataPropertyName = "Approve";
            check.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            check.Width = 75;
            check.ReadOnly = true;
            check.Name = "Approve";

            dgrApprov.Columns.Add(check);

            DataGridViewTextBoxColumn text = new DataGridViewTextBoxColumn();
            text.HeaderText = "Employee Code";
            text.DataPropertyName = "EmployeeCode";
            text.Name = "EmployeeCode";
            text.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            text.Width = 110;
            dgrApprov.Columns.Add(text);

            DataGridViewTextBoxColumn txtEmpName = new DataGridViewTextBoxColumn();
            txtEmpName.HeaderText = "Employee Name";
            txtEmpName.DataPropertyName = "EmployeeName";
            txtEmpName.Name = "EmployeeName";
            txtEmpName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtEmpName.Width = 110;
            dgrApprov.Columns.Add(txtEmpName);

            CNumEditDataGridViewColumn cNumCol;
            for (int i = 1; i <= vMonthDays; i++)
            {
                cNumCol = new CNumEditDataGridViewColumn();

                cNumCol.HeaderText = "Day" + i.ToString().Trim();
                cNumCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                cNumCol.DataPropertyName = "Day" + i.ToString().Trim();
                cNumCol.Name = "Day" + i.ToString().Trim();
                cNumCol.Width = 40;
                cNumCol.DecimalLength = 2;
                dgrApprov.Columns.Add(cNumCol);
            }

            cNumCol = new CNumEditDataGridViewColumn();
            cNumCol.HeaderText = "Worked Hrs";
            cNumCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            cNumCol.DataPropertyName = "WrkdHrs";
            cNumCol.Name = "WrkdHrs";
            cNumCol.Width = 75;
            cNumCol.ReadOnly = true;
            dgrApprov.Columns.Add(cNumCol);

            cNumCol = new CNumEditDataGridViewColumn();
            cNumCol.HeaderText = "Total OT Hrs";
            cNumCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            cNumCol.DataPropertyName = "WrkdOTHrs";
            cNumCol.Name = "WrkdOTHrs";
            cNumCol.Width = 75;
            cNumCol.ReadOnly = true;
            dgrApprov.Columns.Add(cNumCol);

            cNumCol = new CNumEditDataGridViewColumn();
            cNumCol.HeaderText = "Total in Month";
            cNumCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            cNumCol.DataPropertyName = "TotalHrs";
            cNumCol.Name = "TotalHrs";
            cNumCol.Width = 75;
            cNumCol.ReadOnly = true;
            dgrApprov.Columns.Add(cNumCol);

            CNumEditDataGridViewColumn txtMonthDays = new CNumEditDataGridViewColumn();
            txtMonthDays.HeaderText = "Month Hours";
            txtMonthDays.DataPropertyName = "MonthDays";
            txtMonthDays.Name = "MonthDays";
            txtMonthDays.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtMonthDays.Width = 75;
            dgrApprov.Columns.Add(txtMonthDays);

            DataGridViewTextBoxColumn txtSupervisor = new DataGridViewTextBoxColumn();
            txtSupervisor.HeaderText = "Supervisor";
            txtSupervisor.DataPropertyName = "supervisor";
            txtSupervisor.Name = "supervisor";
            txtSupervisor.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            txtSupervisor.Width = 110;
            dgrApprov.Columns.Add(txtSupervisor);
            //**********************

            /// For Header Colour change ---->
            //dgrApprov.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGray;
            dgrApprov.EnableHeadersVisualStyles = false;
            /// For Header Colour change <----
            dgrApprov.RowsDefaultCellStyle.Font = new Font("Arial", 8.0F);
        }
        #endregion

        #region Controls Enable Desable
        private void ControlsEnable()
        {
            vEnable = false;
            if (this.pAddMode | this.pEditMode)
            {
                btnEmail.Enabled = false;
                btnPreview.Enabled = false;
                btnPrint.Enabled = false;
                btnEdit.Enabled = false;

                if (dgrApprov.Rows.Count > 0)
                    dgrApprov.Columns[0].ReadOnly = false;
            }
            else
            {
                if (dtDailyMuster != null && dtDailyMuster.Rows.Count > 0)
                {
                    btnEmail.Enabled = true;
                    btnPreview.Enabled = true;
                    btnPrint.Enabled = true;
                }
                else
                {
                    btnPreview.Enabled = false;//Added By Pankaj B. for Bug-25971
                }
                btnEdit.Enabled = true;
                btnSave.Enabled = false;
                btnNew.Enabled = false;
                btnCancel.Enabled = false;
                if (dgrApprov.Rows.Count > 0)
                    for (int i = 1; i <= vMonthDays; i++)
                        dgrApprov.Columns["Day" + i.ToString().Trim()].ReadOnly = true;


            }
            foreach (object cm in dgrApprov.Columns)
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
            cAppName = "udDailyHourWiseMuster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy  insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }
        #endregion

        #region Local Fuctions
        private Decimal fnAttCodeTot(string vvAttNm, DataRow vdrAttInt)
        {

            string vvHAttNm = GetHalfDayName(vvAttNm.Trim());
            Decimal vAttCodeTot = 0;
            string vColNm = "";
            for (Decimal i = 1; i <= vMonthDays; i++)
            {
                vColNm = "Day" + i.ToString().Trim();

                if (vdrAttInt[vColNm].ToString().ToLower().Trim() == vvAttNm)
                {
                    vAttCodeTot = vAttCodeTot + 1;
                }
                if (vdrAttInt[vColNm].ToString().ToLower().Trim() == vvHAttNm)
                {
                    vAttCodeTot = vAttCodeTot + (decimal)0.5;
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
        private string GetAttCode(string AttName)
        {
            //SqlStr = "Select Att_Nm,Att_Code,isleave,h_Att_Code,h_Att_Nm From Emp_Attendance_Setting where LDeactive=0 and att_code not in ('MonthDays','SalPaidDays') order by SortOrd";
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

        private string GetAttName(string AttCode)
        {
            string Expression = string.Empty;
            Expression = "Att_Code='" + AttCode + "'";
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

            sqlQuery = "select DateName( Month , DateAdd( month , cast(Pay_Month as int), 0 )-1  ) as [Month] from emp_Processing_month where isclosed!=1 order by Pay_month";

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



        }

        #region Look and Feel
        private void frmAttendanceIntegration_Resize(object sender, EventArgs e)
        {
            dgrApprov.Height = this.Height - (groupBox1.Height + 60);
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
            //oPrint.pTran_Cd = Convert.ToInt16(this.vTran_Cd); //Commented By Amrendra

            oPrint.pSpPara = "'" + this.txtProcYear.Text.Trim() + "','" + fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "','" + this.pAppUerName + "'";
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



        private void sbProcessingYear_Click(object sender, EventArgs e)
        {
            //sqlQuery = "select  Pay_Year from emp_Processing_month where isclosed!=1"; // Commented By Amrendra on 03-07-2012
            sqlQuery = "select distinct Pay_Year from emp_Processing_month where isclosed!=1"; // Changed By Amrendra on 03-07-2012

            tDs = oDataAccess.GetDataSet(sqlQuery, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select distinct  Payroll Year";
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

        }


        #region Not Required this code
        private void mthUpdateMonthlyMuster()
        {
            IsValid = true;
            this.mthValid();
            string vSaveString = "";
            if (IsValid == false)
            {
                return;
            }
            oDataAccess.BeginTransaction();
            try
            {
                string vColNm = "";
                foreach (DataRow drEmp in dtMonthlyMuster.Rows)
                {
                    vSaveString = "";
                    foreach (DataRow drAttSet in tblAttSett.Rows)
                    {
                        vColNm = drAttSet["Att_Code"].ToString();
                        vSaveString = vSaveString + "," + vColNm + "=" + drEmp[vColNm].ToString();
                    }
                    vSaveString = vSaveString.Substring(1, vSaveString.Length - 1);
                    vSaveString = "Update Emp_Monthly_Muster Set " + vSaveString;
                    vSaveString = vSaveString + " Where EmployeeCode='" + drEmp["EmployeeCode"].ToString() + "'";
                    vSaveString = vSaveString + " and Pay_Year='" + drEmp["Pay_Year"].ToString() + "'";
                    vSaveString = vSaveString + " and Pay_Month=" + drEmp["Pay_Month"].ToString();
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);

                }
                vSaveString = "Execute usp_Enp_Emp_UpdateMonthly_Muster '" + this.txtProcYear.Text + "'";
                vSaveString = vSaveString + "," + this.fnNMonth(this.txtProcMonth.Text).ToString();
                vSaveString = vSaveString + ",''";//Location
                vSaveString = vSaveString + "," + vMonthDays.ToString();

                oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                oDataAccess.CommitTransaction();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                oDataAccess.RollbackTransaction();
            }

        }
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
                                    drDisplayNew[dc.ColumnName] = GetAttName(dr[dc].ToString());
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
            this.mcheckCallingApplication();      // To be Removed Sachin 
            dgrApprov.EndEdit();

            if (!mthValid())
                return;

            oDataAccess.BeginTransaction();
            foreach (DataRow dr in dtDailyMuster.Rows)
            {
                {
                    SqlStr = "";
                    this.mthSaveString(ref SqlStr, "Emp_Daily_Hourwise_Muster", dr);
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
            }
            SqlStr = "Execute Usp_Ent_Upd_Mnthly_Muster_As_Per_HrsWise_Muster '" + this.txtProcYear.Text.Trim() + "'," + this.fnNMonth(this.txtProcMonth.Text.Trim());
            try
            {
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                oDataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Records could not be saved...!!! " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                oDataAccess.RollbackTransaction();
                return;
            }

            this.pAddMode = false;
            this.pEditMode = false;
            loadLatestData();
            ControlsEnable();
            if (dgrApprov.Rows.Count > 0)
                dgrApprov.Columns[0].ReadOnly = true;
            MessageBox.Show("Records saved successfully...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string xx = "Select EmployeeCode from " + tblTarget + " where employeeCode='" + dr["EmployeeCode"].ToString().Trim() + "' ";
            xx = xx + " and Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
            xx = xx + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            DataTable dtX = oDataAccess.GetDataTable(xx, null, vTimeOut);
            //if ((Boolean)dr["Upload"] == true && (1 == 1) && dtX.Rows.Count==0 )
            if (dtX.Rows.Count == 0)
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
                    vfldValList = vfldValList + "," + dr[vFld_Nm].ToString() + "";
                }
                vSaveString = vSaveString + "(" + vfldList + " ) Values (" + vfldValList + ")";
            }
            else
            {
                vSaveString = "Set DateFormat dmy Update " + tblTarget + " Set";
                vSaveString = vSaveString + " SysDate='" + DateTime.Now.Date.ToString().Trim() + "'";
                vSaveString = vSaveString + ",User_Name='" + this.pAppUerName.Trim() + "'";
                if (dr["Approve"] != DBNull.Value)
                    vSaveString = vSaveString + ",Approve=" + (Convert.ToBoolean(dr["Approve"]) ? "1" : "0");
                else
                    vSaveString = vSaveString + ",Approve=0";
                for (int iCountx = 1; iCountx <= vMonthDays; iCountx++)
                {
                    vFld_Nm = "Day" + iCountx.ToString().Trim();
                    vSaveString = vSaveString + "," + vFld_Nm;
                    vSaveString = vSaveString + "=" + dr[vFld_Nm].ToString() + "";

                }
                vSaveString = vSaveString + " where employeeCode='" + dr["EmployeeCode"].ToString().Trim() + "'";
                vSaveString = vSaveString + " and Pay_Year='" + this.txtProcYear.Text.Trim() + "'";
                vSaveString = vSaveString + " And Pay_Month='" + this.fnNMonth(this.txtProcMonth.Text.Trim()).ToString().Trim() + "'";
            }

        }

        #region Validation
        public void mthValidateTextBox()
        {
            ErrorStr = "";
            if (txtProcMonth.Text == "")
            {
                IsError = true;
                ErrorStr = ErrorStr + "\nProcessing Month should not be empty.";
            }
            if (txtProcYear.Text == "")
            {
                IsError = true;
                ErrorStr = ErrorStr + "\nProcessing Year should not be empty.";
            }
            if (txtMuster.Text == "")
            {
                IsError = true;
                ErrorStr = ErrorStr + "\nMuster Type should not be empty.";
            }

        }
        private void mthValidateAttendance()
        {
            string vAttCode = "", vAttNm = "";
            ErrorStr = "";
            int RowCounter = 0;
            foreach (DataRow drMunthlyMuster in dtDailyMuster.Rows)
            {
                decimal totleave = 0, TotWeekoffs = 0, TotHolyDay = 0;
                foreach (DataRow drAttSett in tblAttSett.Select("ISLEAVE=1")) // Leave Exceeds or Not
                {
                    vAttNm = drAttSett["Att_Nm"].ToString().ToLower().Trim();
                    totleave = fnAttCodeTot(vAttNm, drMunthlyMuster);
                    if (totleave > Convert.ToDecimal(drMunthlyMuster[drAttSett["Att_Code"].ToString().ToLower().Trim() + "_Balance"]))
                    {
                        IsError = true;
                        ErrorStr = ErrorStr + drMunthlyMuster["EmployeeName"].ToString().Trim() + " : " + drAttSett["Att_nm"].ToString().Trim() + " Balance" + " Exceeds than the available.\n";
                        dgrApprov.Rows[RowCounter].Selected = true;
                    }
                }

                vAttNm = "Weekly Off".ToLower();
                TotWeekoffs = fnAttCodeTot(vAttNm, drMunthlyMuster);
                if (TotWeekoffs > Convert.ToDecimal(drMunthlyMuster["WO"]))
                {
                    IsError = true;
                    ErrorStr = ErrorStr + drMunthlyMuster["EmployeeName"].ToString().Trim() + " : Week offs Exceeds than the available.\n";
                    dgrApprov.Rows[RowCounter].Selected = true;
                }
                vAttNm = "Holiday".ToLower();
                TotWeekoffs = fnAttCodeTot(vAttNm, drMunthlyMuster);
                if (TotWeekoffs > Convert.ToDecimal(drMunthlyMuster["HD"]))
                {
                    IsError = true;
                    ErrorStr = ErrorStr + drMunthlyMuster["EmployeeName"].ToString().Trim() + " : Holidays Exceeds than the available.\n";
                    dgrApprov.Rows[RowCounter].Selected = true;
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
            else
            {
                mthValidateAttendance(); // Leave and Offs Validation
                if (IsError)
                {
                    MessageBox.Show(ErrorStr);
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
        #endregion

        #endregion


        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();      // to be removed Sachin 
            if (dgrApprov.Rows.Count <= 0)
            {
                MessageBox.Show("No records to edit.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            btnNew.Enabled = false;
            btnEdit.Enabled = false;
            btnEmail.Enabled = false;
            btnPreview.Enabled = false;
            btnPrint.Enabled = false;

            this.pAddMode = false;
            this.pEditMode = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            ControlsEnable();
            if (dgrApprov.Rows.Count > 0)
            {
                dgrApprov.Columns[0].ReadOnly = false;
                for (int i = 1; i <= vMonthDays; i++)
                    dgrApprov.Columns["Day" + i.ToString().Trim()].ReadOnly = false;
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
            if (dgrApprov.Rows.Count > 0)
                dgrApprov.Columns["Approve"].ReadOnly = true;

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            //  mDeleteProcessIdRecord(); // Added by pratap 02-11-2012
            Application.Exit();
        }

        #region Grid Check box Click
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgrApprov.CurrentCell is DataGridViewCheckBoxCell && this.pEditMode == true)
            {

                bool IsValidForApproval = true;
                foreach (DataGridViewCell cell in dgrApprov.CurrentRow.Cells)
                {
                    if ((string.IsNullOrEmpty(cell.Value.ToString()) | cell.Value.ToString().Contains("Not")) && cell.OwningColumn.DataPropertyName != "supervisor")
                    {
                        MessageBox.Show("Attendance is not entered properly.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        IsValidForApproval = false;
                        break;
                    }
                }
                if (!IsValidForApproval && (bool)dgrApprov["Approve", e.RowIndex].Value == false)
                    dgrApprov.CancelEdit();
                else
                    dgrApprov.EndEdit();
            }

        }

        private void dgrApprov_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgrApprov.CancelEdit();
        }
        #endregion

        public void btnCalculator_Click(object sender, EventArgs e)
        {

            //System.Diagnostics.Process p = System.Diagnostics.Process.Start("calc.exe");
            //p.WaitForInputIdle();
            //NativeMethods.SetParent(p.MainWindowHandle, this.Handle);
            System.Diagnostics.Process.Start("calc");
        }

        private void dgrApprov_KeyDown(object sender, KeyEventArgs e)
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
            if (e.Control && e.KeyCode == Keys.O)       // Ctrl+O Preview
            {
                btnPreview_Click(sender, e);
            }
        }

        //Added By Amrendra On 03-07-2012

        private void mDeleteProcessIdRecord()  /* Added Pratap 02/11/2012 */
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)        /*Ramya Bug-8354*/
        {
            //if (this.btnNew.Enabled)
            //    btnNew_Click(this.btnNew, e);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
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

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnPreview.Enabled)
                btnPreview_Click(this.btnPreview, e);

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnPrint.Enabled)
                btnPrint_Click(this.btnPrint, e);
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnEmail.Enabled)
                btnEmail_Click(this.btnEmail, e);
        }

        private void frmDailyHourWiseMuster_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void dgrApprov_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dgrApprov.Columns[e.ColumnIndex].DataPropertyName.Contains("Day") == true)
            {
                decimal WrkHrs = 0, WrkdHrs = 0;
                dtDailyMuster.Rows[e.RowIndex]["WrkdHrs"] = 0;
                dtDailyMuster.Rows[e.RowIndex]["WrkdOTHrs"] = 0;
                dtDailyMuster.Rows[e.RowIndex]["TotalHrs"] = 0;
                for (int i = 1; i <= 31; i++)
                {
                    WrkHrs = Convert.ToDecimal(dtDailyMuster.Rows[e.RowIndex]["WrkHrs"]);
                    WrkdHrs = Convert.ToDecimal(dtDailyMuster.Rows[e.RowIndex]["Day"+i.ToString().Trim()]);
                    if (WrkdHrs > 0)
                    {
                        dtDailyMuster.Rows[e.RowIndex]["WrkdHrs"] = Convert.ToDecimal(dtDailyMuster.Rows[e.RowIndex]["WrkdHrs"]) + (WrkHrs==0 ? WrkdHrs : WrkHrs >= WrkdHrs ? WrkdHrs : WrkHrs);
                        dtDailyMuster.Rows[e.RowIndex]["WrkdOTHrs"] = Convert.ToDecimal(dtDailyMuster.Rows[e.RowIndex]["WrkdOTHrs"]) + (WrkHrs==0 ? 0 : WrkHrs >= WrkdHrs ? 0 : WrkdHrs - WrkHrs);
                        dtDailyMuster.Rows[e.RowIndex]["TotalHrs"] = Convert.ToDecimal(dtDailyMuster.Rows[e.RowIndex]["TotalHrs"]) + WrkdHrs;
                    }
                }
            }
        }
    }
}
