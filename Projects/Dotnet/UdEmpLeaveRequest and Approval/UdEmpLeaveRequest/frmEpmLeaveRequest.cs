using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ueconnect;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using GetInfo;


namespace UdEmpLeaveRequest
{
    public partial class frmEpmLeaveRequest : uBaseForm.FrmBaseForm
    {
        String cAppPId, cAppName;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string EmpCode = string.Empty;
        string LeaveBalance = string.Empty,LeaveOpening=string.Empty,LeaveAvail=string.Empty,LeaveNm=string.Empty,LeaveCredit=string.Empty,LeaveEncash=string.Empty;
        int rowindex,AppID,grdcnt;
        clsConnect oConnect;
        string ServiceType = string.Empty;
        bool vEnable = false;
        bool IsError = false;
        string ErrorStr = "";
        string sqlQuery;
        DataSet tDs = new DataSet();
        DataSet DS1 = new DataSet();
        DataSet dstot = new DataSet();
        DataSet vDsCommon;
        DataRow dr;
        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
        DataAccess_Net.clsDataAccess oDataAccess = new DataAccess_Net.clsDataAccess();
        DataGridViewComboBoxCell cmb = new DataGridViewComboBoxCell();
        DataGridViewComboBoxColumn cmb2 = new DataGridViewComboBoxColumn();

        bool IsComboBox = false;

        #region Initialization of Form with parameter
        public frmEpmLeaveRequest(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;
            this.pPara = args;
            this.pFrmCaption = "Employee Leave Request";
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
            this.set_grid_structure();// Set Structure of Grid
        }
        #endregion

        #region Load Form
        private void frmEpmLeaveRequest_Load(object sender, EventArgs e)
        {

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.DateSeparator = "/";
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            string appPath =Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnApplNo.Image = Image.FromFile(fName);
                this.btnEmp.Image = Image.FromFile(fName);
            }
            this.mInsertProcessIdRecord();
            startupPath = Application.StartupPath;
            //startupPath = @"D:\VudyogPRO";          // To be Removed Sachin 
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
            }
            else
                ControlsEnable();
            this.pAddMode = false;
            this.pEditMode = false;
            this.btnLast_Click(sender,e);
            txtEmp.Focus();
            
        }
        #endregion

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "UdEmpLeaveRequest.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy  insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 25, true);
        }

        #region Controls Enable Desable
        private void ControlsEnable()
        {
            vEnable = false;
           if (this.pEditMode)
            {
                this.ActionButtonState(false,false,true,true,false,false,false,false);
                this.MOVEBTNSTATE(false, false, false, false);
                
                txtRemarks.Enabled = true;
                groupBox2.Visible = false;
                dgvLeaveReq.Enabled = true;

            }
            else if (this.pAddMode)
            {
                this.ActionButtonState(false, false, true, true, false,false,false,false);
                this.MOVEBTNSTATE(false,false,false,false);

                btnApplNo.Enabled = false;
                btnEmp.Enabled = true;


                foreach (Control c in Controls)
                {
                    if (c is TextBox)
                    {
                        c.Text = String.Empty;
                        c.Enabled = true;
                    }
                    if (c is DateTimePicker)
                    {
                        c.Text = String.Empty;
                        c.Enabled = true;
                    }
                }
                txtApplNo.Enabled = false;
                txtDate.Enabled = false;
                dgvLeaveReq.ReadOnly = false;
                tDs.Tables[0].Rows.Clear();
            }
            else
            {
                this.ActionButtonState(true,false,false,false,true,true,true,true);
                btnExit.Enabled = true;
                btnApplNo.Enabled = true;
                btnEmp.Enabled = false;

                txtApplNo.Enabled = false;
                txtEmp.Enabled = false;
                txtRemarks.Enabled = false;
                groupBox2.Visible = false;
                dgvLeaveReq.Enabled = true;
                dgvLeaveReq.ReadOnly = true;

                dgvLeaveReq.Columns["LEAVETYPE"].Frozen = true;
                dgvLeaveReq.Columns["LEAVENM"].Frozen = true;
                dgvLeaveReq.Columns["SDATE"].Frozen = true;
                dgvLeaveReq.Columns["EDATE"].Frozen = true;
                dgvLeaveReq.Columns["TOT_LEAVE"].Frozen = true;
                dgvLeaveReq.Columns["USED_LEAVE"].Frozen = true;
                dgvLeaveReq.Columns["BAL_LEAVE"].Frozen = true;
                dgvLeaveReq.Columns["REQ_LEAVE"].Frozen = true;

                dgvLeaveReq.Columns["CONSTYPE"].Width = 80;
                dgvLeaveReq.Columns["SDATE"].Width = 80;

            }
        }
        #endregion

        #region show total in total Grid
        private void cal_total()
        {
            decimal REQ_LEAVE = 0, TOT_LEAVE = 0, USED_LEAVE = 0, BAL_LEAVE = 0;
            foreach (DataRow dr in tDs.Tables[0].Rows)
            {
                TOT_LEAVE += Convert.ToDecimal(dr["TOT_LEAVE"]);
                USED_LEAVE += Convert.ToDecimal(dr["USED_LEAVE"]);
                BAL_LEAVE += Convert.ToDecimal(dr["BAL_LEAVE"]);
                REQ_LEAVE += Convert.ToDecimal(dr["REQ_LEAVE"]);
            }
            dgvTotal.Rows[0].Cells["TOT_LEAVE"].Value = TOT_LEAVE;
            dgvTotal.Rows[0].Cells["USED_LEAVE"].Value = USED_LEAVE;
            dgvTotal.Rows[0].Cells["BAL_LEAVE"].Value = BAL_LEAVE;
            dgvTotal.Rows[0].Cells["REQ_LEAVE"].Value = REQ_LEAVE;
        }
        #endregion

        #region Select Employee Popup
        private void btnEmp_Click(object sender, EventArgs e)
        {
            EmpCode = "";
            sqlQuery = "select EmployeeCode,EmployeeName from EmployeeMast";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = DS1.Tables[0].DefaultView;

            VForText = "Select Employee";
            vSearchCol = "EmployeeName";
            vDisplayColumnList = "EmployeeCode:Employee Code,EmployeeName:Employee Name";
            vReturnCol = "EmployeeName,EmployeeCode";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtEmp.Text = oSelectPop.pReturnArray[0];
                EmpCode=oSelectPop.pReturnArray[1];
            }
            sqlQuery = "select B.LEAVETYPE AS [Leave Type],B.LEAVENM as [Leave Name],B.SDATE [Start Date],B.EDATE [End Date],B.CONSTYPE as [Consumption Type],B.REQ_LEAVE [Req. Leave],B.STATUS as Status";
            sqlQuery += " FROM Emp_Leave_Request_Item b";
            sqlQuery += " where B.EMPCODE='" + EmpCode + "' AND convert(datetime,SDATE,103)>Convert(datetime, DateAdd(month, -3, getdate()),103) ORDER BY B.SDATE";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            if (DS1.Tables[0].Rows.Count!=0)
            {
            dgvOverview.DataSource = DS1.Tables[0].DefaultView;
            dgvOverview.Columns[0].Width = 50;
            dgvOverview.Columns[1].Width = 100;
            dgvOverview.Columns[2].Width = 70;
            dgvOverview.Columns[3].Width = 70;
            dgvOverview.Columns[4].Width = 70;
            dgvOverview.Columns[5].Width = 50;
            dgvOverview.Columns[6].Width = 80;
            groupBox2.Visible = true;
            }

            errorProvider1.SetError(txtEmp, "");
        }
        #endregion



        #region Exit Form
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Set datagrid structure
        public void set_grid_structure()
        {
            dgvLeaveReq.RowHeadersVisible = false;
            dgvLeaveReq.AutoGenerateColumns = false;
            if (this.pAddMode)
            {
                //dgvLeaveReq.Columns["LEAVETYPE"].Frozen = true;
                //dgvLeaveReq.Columns["LEAVENM"].Frozen = true;
                //dgvLeaveReq.Columns["SDATE"].Frozen = true;
                //dgvLeaveReq.Columns["EDATE"].Frozen = true;
                //dgvLeaveReq.Columns["TOT_LEAVE"].Frozen = true;
                //dgvLeaveReq.Columns["USED_LEAVE"].Frozen = true;
                //dgvLeaveReq.Columns["BAL_LEAVE"].Frozen = true;
                //dgvLeaveReq.Columns["REQ_LEAVE"].Frozen = true;
                dgvLeaveReq.Columns[0].Frozen = false;
                dgvLeaveReq.Columns[1].Frozen = false;
                dgvLeaveReq.Columns[2].Frozen = false;
                dgvLeaveReq.Columns[3].Frozen = false;
                dgvLeaveReq.Columns[4].Frozen = false;
                dgvLeaveReq.Columns[5].Frozen = false;
                dgvLeaveReq.Columns[6].Frozen = false;
                dgvLeaveReq.Columns[7].Frozen = false;
                //dgvLeaveReq.ColumnCount = 13;
                //DataSet ds = new DataSet();
                
                DataGridViewComboBoxColumn cmb1 = new DataGridViewComboBoxColumn();
                sqlQuery = "select att_code as leave_type,att_nm as leave_nm from Emp_Attendance_Setting where isleave=1 and ldeactive=0";
                DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                cmb1.Items.Add("");
                for (int i = 0; i < DS1.Tables[0].Rows.Count; i++)
                {
                    cmb1.Items.Add(DS1.Tables[0].Rows[i]["leave_type"].ToString());
                }
                cmb1.HeaderText = "Leave Type";
                cmb1.DataPropertyName = "LEAVETYPE";
                cmb1.Name = "LEAVETYPE";
               
                dgvLeaveReq.Columns.RemoveAt(0);
                dgvLeaveReq.Columns.Insert(0, cmb1);
                dgvLeaveReq.Columns[0].ReadOnly = false;
                //dgvLeaveReq.Rows[0].Cells[0] = cmb;

                
                //dgvLeaveReq.Columns[0].HeaderText = "Leave Desc";
                //dgvLeaveReq.Columns[0].DataPropertyName = "LEAVENM";
                //dgvLeaveReq.Columns[0].Name = "LEAVENM";
                


                //dgvLeaveReq.Columns[1].HeaderText = "Leave Desc";
                //dgvLeaveReq.Columns[1].DataPropertyName = "LEAVENM";
                //dgvLeaveReq.Columns[1].Name = "LEAVENM";

     

                dgvLeaveReq.Columns.RemoveAt(2);
                cmb2.Items.Clear();
                cmb2.Items.Add("");
                cmb2.Items.Add("Full Day");
                cmb2.Items.Add("Half Day");
                dgvLeaveReq.Columns.Insert(2, cmb2);
                dgvLeaveReq.Columns[2].HeaderText = "Consumption Type";
                dgvLeaveReq.Columns[2].DataPropertyName = "CONSTYPE";
                dgvLeaveReq.Columns[2].Name = "CONSTYPE";
                dgvLeaveReq.Columns[2].ReadOnly = false;




                CalendarColumn cal1 = new CalendarColumn();
                dgvLeaveReq.Columns.RemoveAt(3);
                dgvLeaveReq.Columns.Insert(3, cal1);
                dgvLeaveReq.Columns[3].HeaderText = "Start DT";
                dgvLeaveReq.Columns[3].Name = "SDATE";
                dgvLeaveReq.Columns[3].DataPropertyName = "SDATE";
                dgvLeaveReq.Columns[3].ReadOnly = false;

                CalendarColumn cal2 = new CalendarColumn();
                dgvLeaveReq.Columns.RemoveAt(4);
                dgvLeaveReq.Columns.Insert(4, cal2);
                dgvLeaveReq.Columns[4].HeaderText = "End DT";
                dgvLeaveReq.Columns[4].Name = "EDATE";
                dgvLeaveReq.Columns[4].DataPropertyName = "EDATE";
                dgvLeaveReq.Columns[4].ReadOnly = false;

                //DataGridViewComboBoxColumn cmb2 = new DataGridViewComboBoxColumn();
                //CalendarColumn cal1 = new CalendarColumn();
                //cal1.HeaderText = "Start DT";
                //cal1.Name = "SDATE";
                //dgvLeaveReq.Columns.Add(cal1);
                //dgvLeaveReq.Columns[2].HeaderText = "Start DT";
                //dgvLeaveReq.Columns[2].DataPropertyName = "SDATE";
                //dgvLeaveReq.Columns[2].Name = "SDATE";
 

            }
            else
            {

                dgvLeaveReq.ColumnCount = 13;

                dgvLeaveReq.Columns[0].HeaderText = "Leave Type";
                dgvLeaveReq.Columns[0].DataPropertyName = "LEAVETYPE";
                dgvLeaveReq.Columns[0].Name = "LEAVETYPE";
                dgvLeaveReq.Columns[1].HeaderText = "Leave Desc";
                dgvLeaveReq.Columns[1].DataPropertyName = "LEAVENM";
                dgvLeaveReq.Columns[1].Name = "LEAVENM";
                dgvLeaveReq.Columns[2].HeaderText = "Consumption Type";
                dgvLeaveReq.Columns[2].DataPropertyName = "CONSTYPE";
                dgvLeaveReq.Columns[2].Name = "CONSTYPE";
                dgvLeaveReq.Columns[3].HeaderText = "Start DT";
                dgvLeaveReq.Columns[3].DataPropertyName = "SDATE";
                dgvLeaveReq.Columns[3].Name = "SDATE";
                dgvLeaveReq.Columns[4].HeaderText = "End DT";
                dgvLeaveReq.Columns[4].DataPropertyName = "EDATE";
                dgvLeaveReq.Columns[4].Name = "EDATE";


  
            }
            //dgvLeaveReq.Columns[0].HeaderText = "Leave Type";
            //dgvLeaveReq.Columns[0].DataPropertyName = "LEAVETYPE";
            //dgvLeaveReq.Columns[0].Name = "LEAVETYPE";
            //dgvLeaveReq.Columns[1].HeaderText = "Leave Desc";
            //dgvLeaveReq.Columns[1].DataPropertyName = "LEAVENM";
            //dgvLeaveReq.Columns[1].Name = "LEAVENM";
            //dgvLeaveReq.Columns[2].HeaderText = "Start DT";
            //dgvLeaveReq.Columns[2].DataPropertyName = "SDATE";
            //dgvLeaveReq.Columns[2].Name = "SDATE";
            //dgvLeaveReq.Columns[3].HeaderText = "End DT";
            //dgvLeaveReq.Columns[3].DataPropertyName = "EDATE";
            //dgvLeaveReq.Columns[3].Name = "EDATE";
            //dgvLeaveReq.Columns[4].HeaderText = "Consumption Type";
            //dgvLeaveReq.Columns[4].DataPropertyName = "CONSTYPE";
            //dgvLeaveReq.Columns[4].Name = "CONSTYPE";
            dgvLeaveReq.Columns[5].HeaderText = "Total Leaves";
            dgvLeaveReq.Columns[5].DataPropertyName = "TOT_LEAVE";
            dgvLeaveReq.Columns[5].Name = "TOT_LEAVE";
            dgvLeaveReq.Columns[6].HeaderText = "Used";
            dgvLeaveReq.Columns[6].DataPropertyName = "USED_LEAVE";
            dgvLeaveReq.Columns[6].Name = "USED_LEAVE";
            dgvLeaveReq.Columns[7].HeaderText = "Balance";
            dgvLeaveReq.Columns[7].DataPropertyName = "BAL_LEAVE";
            dgvLeaveReq.Columns[7].Name = "BAL_LEAVE";
            dgvLeaveReq.Columns[8].HeaderText = "Requested Leaves";
            dgvLeaveReq.Columns[8].DataPropertyName = "REQ_LEAVE";
            dgvLeaveReq.Columns[8].Name = "REQ_LEAVE";
            dgvLeaveReq.Columns[9].HeaderText = "Status";
            dgvLeaveReq.Columns[9].DataPropertyName = "STATUS";
            dgvLeaveReq.Columns[9].Name = "STATUS";
            dgvLeaveReq.Columns[10].HeaderText = "Approved By";
            dgvLeaveReq.Columns[10].DataPropertyName = "APPR_BY";
            dgvLeaveReq.Columns[10].Name = "APPR_BY";
            dgvLeaveReq.Columns[11].HeaderText = "Approved DT";
            dgvLeaveReq.Columns[11].DataPropertyName = "APPR_DT";
            dgvLeaveReq.Columns[11].Name = "APPR_DT";
            dgvLeaveReq.Columns[12].HeaderText = "Approval Remarks";
            dgvLeaveReq.Columns[12].DataPropertyName = "APPR_REMARKS";
            dgvLeaveReq.Columns[12].Name = "APPR_REMARKS";

            //dgvLeaveReq.Columns[0].Width = 60;
            dgvLeaveReq.Columns[2].Width = 80;
            dgvLeaveReq.Columns[3].Width = 80;
            dgvLeaveReq.Columns[5].Width = 80;
            dgvLeaveReq.Columns[6].Width = 80;
            dgvLeaveReq.Columns[9].Width = 70;
            dgvLeaveReq.Columns[10].Width = 200;
            dgvLeaveReq.Columns[11].Width = 70;
            dgvLeaveReq.Columns[12].Width = 180;

            dgvLeaveReq.Columns["LEAVETYPE"].Frozen = true;
            dgvLeaveReq.Columns["LEAVENM"].Frozen = true;
            dgvLeaveReq.Columns["SDATE"].Frozen = true;
            dgvLeaveReq.Columns["EDATE"].Frozen = true;
            dgvLeaveReq.Columns["TOT_LEAVE"].Frozen = true;
            dgvLeaveReq.Columns["TOT_LEAVE"].ReadOnly = true;
            dgvLeaveReq.Columns["USED_LEAVE"].Frozen = true;
            dgvLeaveReq.Columns["USED_LEAVE"].ReadOnly = true;
            dgvLeaveReq.Columns["BAL_LEAVE"].Frozen = true;
            dgvLeaveReq.Columns["BAL_LEAVE"].ReadOnly = true;
            dgvLeaveReq.Columns["REQ_LEAVE"].Frozen = true;
            dgvLeaveReq.Columns["REQ_LEAVE"].ReadOnly = true;

            dgvLeaveReq.Columns["STATUS"].ReadOnly = true;
            dgvLeaveReq.Columns["APPR_BY"].ReadOnly= true;
            dgvLeaveReq.Columns["APPR_DT"].ReadOnly = true;
            dgvLeaveReq.Columns["APPR_REMARKS"].ReadOnly = true;



           // dgvLeaveReq.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void set_total_grid_structure()
        {

            dgvTotal.RowHeadersVisible = false;
            dgvTotal.AutoGenerateColumns = false;
            //if (this.pAddMode == false)
            //{
                dgvTotal.ColumnCount = 13;
            dgvTotal.Columns[0].HeaderText = "Leave Type";
            dgvTotal.Columns[0].DataPropertyName = "LEAVETYPE";
            dgvTotal.Columns[0].Name = "LEAVETYPE";
            dgvTotal.Columns[1].HeaderText = "Leave Desc";
            dgvTotal.Columns[1].DataPropertyName = "LEAVENM";
            dgvTotal.Columns[1].Name = "LEAVENM";
            dgvTotal.Columns[2].HeaderText = "Start DT";
            dgvTotal.Columns[2].DataPropertyName = "SDATE";
            dgvTotal.Columns[2].Name = "SDATE";
            dgvTotal.Columns[3].HeaderText = "End DT";
            dgvTotal.Columns[3].DataPropertyName = "EDATE";
            dgvTotal.Columns[3].Name = "EDATE";
            dgvTotal.Columns[4].HeaderText = "Consumption Type";
            dgvTotal.Columns[4].DataPropertyName = "CONSTYPE";
            dgvTotal.Columns[4].Name = "CONSTYPE";
            dgvTotal.Columns[5].HeaderText = "Total Leaves";
            dgvTotal.Columns[5].DataPropertyName = "TOT_LEAVE";
            dgvTotal.Columns[5].Name = "TOT_LEAVE";
            dgvTotal.Columns[6].HeaderText = "Used";
            dgvTotal.Columns[6].DataPropertyName = "USED_LEAVE";
            dgvTotal.Columns[6].Name = "USED_LEAVE";
            dgvTotal.Columns[7].HeaderText = "Balance";
            dgvTotal.Columns[7].DataPropertyName = "BAL_LEAVE";
            dgvTotal.Columns[7].Name = "BAL_LEAVE";
            dgvTotal.Columns[8].HeaderText = "Requested Leaves";
            dgvTotal.Columns[8].DataPropertyName = "REQ_LEAVE";
            dgvTotal.Columns[8].Name = "REQ_LEAVE";
            dgvTotal.Columns[9].HeaderText = "Status";
            dgvTotal.Columns[9].DataPropertyName = "STATUS";
            dgvTotal.Columns[9].Name = "STATUS";
            dgvTotal.Columns[10].HeaderText = "Approved By";
            dgvTotal.Columns[10].DataPropertyName = "APPR_BY";
            dgvTotal.Columns[10].Name = "APPR_BY";
            dgvTotal.Columns[11].HeaderText = "Approved DT";
            dgvTotal.Columns[11].DataPropertyName = "APPR_DT";
            dgvTotal.Columns[11].Name = "APPR_DT";
            dgvTotal.Columns[12].HeaderText = "Approval Remarks";
            dgvTotal.Columns[12].DataPropertyName = "APPR_REMARKS";
            dgvTotal.Columns[12].Name = "APPR_REMARKS";

            //dgvTotal.Columns[0].Width = 60;
            dgvTotal.Columns[2].Width = 80;
            dgvTotal.Columns[3].Width = 80;
            dgvTotal.Columns[5].Width = 80;
            dgvTotal.Columns[6].Width = 80;
            dgvTotal.Columns[9].Width = 70;
            dgvTotal.Columns[10].Width = 200;
            dgvTotal.Columns[11].Width = 70;
            dgvTotal.Columns[12].Width = 180;

        }

        private void set_add_grid_structure()
        {
            dgvLeaveReq.RowHeadersVisible = false;
            dgvLeaveReq.AutoGenerateColumns = false;
            dgvLeaveReq.ColumnCount = 13;
            dgvLeaveReq.Columns[0].HeaderText = "Leave Type";
            dgvLeaveReq.Columns[0].DataPropertyName = "LEAVETYPE";
            dgvLeaveReq.Columns[0].Name = "LEAVETYPE";
            dgvLeaveReq.Columns[1].HeaderText = "Leave Desc";
            dgvLeaveReq.Columns[1].DataPropertyName = "LEAVENM";
            dgvLeaveReq.Columns[1].Name = "LEAVENM";
            dgvLeaveReq.Columns[2].HeaderText = "Start DT";
            dgvLeaveReq.Columns[2].DataPropertyName = "SDATE";
            dgvLeaveReq.Columns[2].Name = "SDATE";
            dgvLeaveReq.Columns[3].HeaderText = "End DT";
            dgvLeaveReq.Columns[3].DataPropertyName = "EDATE";
            dgvLeaveReq.Columns[3].Name = "EDATE";
            dgvLeaveReq.Columns[4].HeaderText = "Consumption Type";
            dgvLeaveReq.Columns[4].DataPropertyName = "CONSTYPE";
            dgvLeaveReq.Columns[4].Name = "CONSTYPE";
            dgvLeaveReq.Columns[5].HeaderText = "Total Leaves";
            dgvLeaveReq.Columns[5].DataPropertyName = "TOT_LEAVE";
            dgvLeaveReq.Columns[5].Name = "TOT_LEAVE";
            dgvLeaveReq.Columns[6].HeaderText = "Used";
            dgvLeaveReq.Columns[6].DataPropertyName = "USED_LEAVE";
            dgvLeaveReq.Columns[6].Name = "USED_LEAVE";
            dgvLeaveReq.Columns[7].HeaderText = "Balance";
            dgvLeaveReq.Columns[7].DataPropertyName = "BAL_LEAVE";
            dgvLeaveReq.Columns[7].Name = "BAL_LEAVE";
            dgvLeaveReq.Columns[8].HeaderText = "Requested Leaves";
            dgvLeaveReq.Columns[8].DataPropertyName = "REQ_LEAVE";
            dgvLeaveReq.Columns[8].Name = "REQ_LEAVE";
            dgvLeaveReq.Columns[9].HeaderText = "Status";
            dgvLeaveReq.Columns[9].DataPropertyName = "STATUS";
            dgvLeaveReq.Columns[9].Name = "STATUS";
            dgvLeaveReq.Columns[10].HeaderText = "Approved By";
            dgvLeaveReq.Columns[10].DataPropertyName = "APPR_BY";
            dgvLeaveReq.Columns[10].Name = "APPR_BY";
            dgvLeaveReq.Columns[11].HeaderText = "Approved DT";
            dgvLeaveReq.Columns[11].DataPropertyName = "APPR_DT";
            dgvLeaveReq.Columns[11].Name = "APPR_DT";
            dgvLeaveReq.Columns[12].HeaderText = "Approval Remarks";
            dgvLeaveReq.Columns[12].DataPropertyName = "APPR_REMARKS";
            dgvLeaveReq.Columns[12].Name = "APPR_REMARKS";

            //dgvLeaveReq.Columns[0].Width = 60;
            dgvLeaveReq.Columns[2].Width = 80;
            dgvLeaveReq.Columns[3].Width = 80;
            dgvLeaveReq.Columns[5].Width = 80;
            dgvLeaveReq.Columns[6].Width = 80;
            dgvLeaveReq.Columns[9].Width = 70;
            dgvLeaveReq.Columns[10].Width = 200;
            dgvLeaveReq.Columns[11].Width = 70;
            dgvLeaveReq.Columns[12].Width = 180;

        }
        //public void get_cell_centents()
        //{
        //        sqlQuery = "select att_code as leave_type,att_nm as leave_nm from Emp_Attendance_Setting where isleave=1 and ldeactive=0";
        //        tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
        //        cmb.Items.Add("");
        //        for (int i = 0; i < tDs.Tables[0].Rows.Count; i++)
        //        {
        //            cmb.Items.Add(tDs.Tables[0].Rows[i]["leave_type"].ToString());
        //        }
        //}

        #endregion

   
        #region Add new record
        private void btnNew_Click(object sender, EventArgs e)
        {
            AppID = 0;
            grdcnt = 1;
            this.pAddMode = true;

            
            tDs.Tables[0].Rows.Clear();
            this.ControlsEnable();
            this.set_grid_structure();
            dgvLeaveReq.DataSource = tDs.Tables[0];
            dstot.Tables[0].Rows.Clear();
            dgvTotal.DataSource = null;
            this.set_total_grid_structure();
            dgvTotal.DataSource = dstot.Tables[0];

            sqlQuery = "select ident_current('Emp_Leave_Request_Head') AS APPID,convert(varchar(10),getdate(),103) AS CURRDATE";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw =  DS1.Tables[0].DefaultView;
            txtDate.Text = DS1.Tables[0].Rows[0]["CURRDATE"].ToString();

            if (Convert.ToInt32(DS1.Tables[0].Rows[0]["APPID"].ToString()) == 0)
            {
                sqlQuery = "dbcc checkident('Emp_Leave_Request_Head',reseed,1)";
                oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);
                
                AppID = 1;
                txtApplNo.Text = "1";
            }
            else
            {
                AppID = Convert.ToInt32(DS1.Tables[0].Rows[0]["APPID"].ToString())+1;
                txtApplNo.Text = Convert.ToString(AppID);
            }
            
            dgvLeaveReq.Enabled = true;

            // Add blank row in total Grid
            DataRow dr1;
            dr1 = dstot.Tables[0].NewRow();
           
            foreach (DataColumn dc in dstot.Tables[0].Columns)
            {
                switch (dc.DataType.ToString())
                {
                    case "System.String":
                        dr1[dc] = "";
                        break;
                    case "System.DateTime":
                        dr1[dc] = DateTime.Now.Date;
                        break;
                    case "System.Int32":
                        dr1[dc] = 0;
                        break;
                    case "System.Decimal":
                        dr1[dc] = 0.00;
                        break;
                    default:
                        break;
                }

            }
            dstot.Tables[0].Rows.Add(dr1);
            dstot.Tables[0].Rows[0][0] = "Total";
        }
        #endregion

        private int Validate_leave(string leavetype)
        {
            foreach (DataGridViewRow row in dgvLeaveReq.Rows)
            {
                if (leavetype == row.Cells[0].Value.ToString().Trim())
                {
                    return 1;
                }
            }
            return 0;
        }

        private bool validation()
        {
            bool error = true;
            if (this.pAddMode || this.pEditMode)
            {
                if (txtEmp.Text.Trim().Length == 0)
                {
                    errorProvider1.SetError(txtEmp, "Please select Employee Name");
                    txtEmp.Focus();
                    error = false;
                    
                }

                foreach (DataGridViewRow dr in dgvLeaveReq.Rows)
                {
                    if (dr.Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() == "")
                    {
                        MessageBox.Show("Please select Leave Type", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        error = false;
                        break;
                    }
                    if (dr.Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() == "")
                    {
                        MessageBox.Show("Please select Leave Consumption type", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        error = false;
                        break;
                    }
                    if (dr.Cells["REQ_LEAVE"].EditedFormattedValue.ToString().Trim() == "0")
                    {
                        MessageBox.Show("Requested leave days not entered properly", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        error = false;
                        break;
                    }
                    if (dr.Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() == "Half Day" && ((Convert.ToDateTime(dr.Cells["EDATE"].EditedFormattedValue) - Convert.ToDateTime(dr.Cells["SDATE"].EditedFormattedValue)).TotalDays > 0))
                    {
                        MessageBox.Show("Please select valid date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvLeaveReq.ClearSelection();
                        dr.Cells["EDATE"].Selected=true;
                        error = false;
                        break;
                    }
                    if (dgvLeaveReq.Columns[3].Name == "SDATE" && this.pAddMode && dgvLeaveReq.Rows.Count != 0)
                    {
                        sqlQuery = "select SDATE from Emp_Leave_Request_Item where empcode='" + EmpCode + "' and (convert(varchar(10),SDATE,103)='" + dgvLeaveReq.Rows[dgvLeaveReq.Rows.Count - 1].Cells["SDATE"].EditedFormattedValue.ToString().Substring(0, 10) + "' OR convert(varchar(10),EDATE,103)='" + dgvLeaveReq.Rows[dgvLeaveReq.Rows.Count - 1].Cells["SDATE"].EditedFormattedValue.ToString().Substring(0, 10) + "')";
                        DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                        if (DS1.Tables[0].Rows.Count != 0)
                        {
                            MessageBox.Show("Leave already applied", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            error = false;
                            break;
                        }
                    }
                }

                for (int i = 0; i < dgvLeaveReq.RowCount-1; i++)
                {
                    for (int j = 1; j <= dgvLeaveReq.RowCount-1; j++)
                    {
                        if (dgvLeaveReq.Rows[i].Cells["SDATE"].EditedFormattedValue.ToString().Trim() == dgvLeaveReq.Rows[j].Cells["SDATE"].EditedFormattedValue.ToString().Trim())
                        {
                            MessageBox.Show("Leave already applied", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvLeaveReq.ClearSelection();
                            dgvLeaveReq.Rows[j].Cells["SDATE"].Selected=true;
                            error = false;
                            break;
                        }

                        if (dgvLeaveReq.Rows[i].Cells["EDATE"].EditedFormattedValue.ToString().Trim() == dgvLeaveReq.Rows[j].Cells["EDATE"].EditedFormattedValue.ToString().Trim())
                        {
                            MessageBox.Show("Leave already applied", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvLeaveReq.ClearSelection();
                            dgvLeaveReq.Rows[j].Cells["EDATE"].Selected = true;
                            error = false;
                            break;
                        }
                    }
                    if (error==false)
                        break;
                }

                      

            }
            else
            {
                foreach (DataGridViewRow dgrow in dgvLeaveReq.Rows)
                {
                    if (dgrow.Cells["STATUS"].Value.ToString().Trim() == "Approved" || dgrow.Cells["STATUS"].Value.ToString().Trim() == "Rejected")
                    {
                        error = false;
                    }
                }
            }

            return error;
        }

        private bool save_validation()
        {
             bool error = true;
              if (txtEmp.Text.Trim().Length == 0)
                {
                    errorProvider1.SetError(txtEmp, "Please select Employee Name");
                    txtEmp.Focus();
                    error = false;
                    return error;
                }
            if (dgvLeaveReq.Rows.Count==0 )
            {
                MessageBox.Show("Please add Leave to request", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                error = false;
            }
              foreach (DataGridViewRow dr in dgvLeaveReq.Rows)
              {
                  if (dr.Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() == "")
                  {
                      MessageBox.Show("Please select Leave Type", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                      error = false;
                  }
                  if (dr.Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() == "")
                  {
                      MessageBox.Show("Please select Leave Consumption type", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                      error = false;
                  }
                  if (dr.Cells["REQ_LEAVE"].EditedFormattedValue.ToString().Trim() == "0")
                  {
                      MessageBox.Show("Requested leave days not entered properly", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                      error = false;
                  }

              }

            return error;
        }
        

        #region Select record in gridview and delete
        private void dgvLeaveReq_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex>=0)
            {
                this.dgvLeaveReq.Rows[e.RowIndex].Selected = true;
                this.rowindex = e.RowIndex;
                this.contextMenuStrip1.Show(this.dgvLeaveReq, Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Save Record
        private void btnSave_Click(object sender, EventArgs e)
        {
            int CNT=0;
            if (this.save_validation() == false)
            {
                return;
            }

            if (this.validation() == false)
            {
                return;
            }
            for (int i=0;i<dgvLeaveReq.RowCount;i++)
            {
                if (this.pAddMode && dgvLeaveReq.Rows[i].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() != "" && dgvLeaveReq.Rows[i].Cells["LEAVENM"].EditedFormattedValue.ToString().Trim() == "")
                {
                    sqlQuery = "select att_nm as leave_nm from Emp_Attendance_Setting where att_code='" + dgvLeaveReq.Rows[i].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "'";
                    DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                    DataView dvw = DS1.Tables[0].DefaultView;
                    dgvLeaveReq.Rows[i].Cells["LEAVENM"].Value = DS1.Tables[0].Rows[0]["leave_nm"].ToString();
                }
                calculate_leave(i);
                dgvLeaveReq.EndEdit();
            }

            try
            {
                oDataAccess.BeginTransaction();
                //insert into Emp_Leave_Request_Head table Strat
                if (this.pAddMode)
                {
                    sqlQuery = "insert into Emp_Leave_Request_Head([DATE],[EMPNM],[EMPCODE],[REMARKS])";
                    sqlQuery += "values('" + txtDate.Text + "','" + txtEmp.Text + "','" + EmpCode + "','" + txtRemarks.Text + "')";
                    oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);

                    //insert into Emp_Leave_Request_Head table End
                    foreach (DataGridViewRow row in dgvLeaveReq.Rows)
                    {
                        CNT = CNT + 1;
                        sqlQuery = "";
                        //insert into Emp_Leave_Request_ITEM table Strat
                        sqlQuery = "Set DateFormat dmy insert into Emp_Leave_Request_ITEM";
                        sqlQuery += "([APPID],[DATE],[EMPCODE],[LEAVETYPE],[LEAVENM],[SDATE],[EDATE],[CONSTYPE],[TOT_LEAVE],[USED_LEAVE]";
                        sqlQuery += ",[BAL_LEAVE],[REQ_LEAVE],[STATUS],[APPR_BY],[APPR_DT],[APPR_REMARKS],[LEAVENO]) VALUES(";
                        sqlQuery += "" + AppID + ",'" + txtDate.Text + "','" + EmpCode + "',";
                        sqlQuery += "'" + row.Cells["LEAVETYPE"].Value + "',";
                        sqlQuery += "'" + row.Cells["LEAVENM"].Value + "',";
                        sqlQuery += "'" + row.Cells["SDATE"].Value + "',";
                        sqlQuery += "'" + row.Cells["EDATE"].Value + "',";
                        sqlQuery += "'" + row.Cells["CONSTYPE"].Value + "',";
                        sqlQuery += "" + row.Cells["TOT_LEAVE"].Value + ",";
                        sqlQuery += "" + row.Cells["USED_LEAVE"].Value + ",";
                        sqlQuery += "" + row.Cells["BAL_LEAVE"].Value + ",";
                        sqlQuery += "" + row.Cells["REQ_LEAVE"].Value + ",";
                        sqlQuery += "'" + row.Cells["STATUS"].Value + "','','','',"+(CNT)+")";
                        oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);
                        //insert into Emp_Leave_Request_ITEM table End
                    }
                }

                this.pAddMode = false;
                this.pEditMode = false;
                dgvLeaveReq.Columns[0].Frozen = false;
                dgvLeaveReq.Columns[1].Frozen = false;
                dgvLeaveReq.Columns[2].Frozen = false;
                dgvLeaveReq.Columns[3].Frozen = false;
                dgvLeaveReq.Columns[4].Frozen = false;
                dgvLeaveReq.Columns[5].Frozen = false;
                dgvLeaveReq.Columns[6].Frozen = false;
                dgvLeaveReq.Columns[7].Frozen = false;
                DataGridViewTextBoxColumn txt1 = new DataGridViewTextBoxColumn();
                txt1.HeaderText = "Leave Type";
                txt1.DataPropertyName = "LEAVETYPE";
                txt1.Name = "LEAVETYPE";
                dgvLeaveReq.Columns.RemoveAt(0);
                dgvLeaveReq.Columns.Insert(0, txt1);

                DataGridViewTextBoxColumn cal1 = new DataGridViewTextBoxColumn();
                dgvLeaveReq.Columns.RemoveAt(3);
                dgvLeaveReq.Columns.Insert(3, cal1);
                dgvLeaveReq.Columns[3].HeaderText = "Start DT";
                dgvLeaveReq.Columns[3].Name = "SDATE";
                dgvLeaveReq.Columns[3].DataPropertyName = "SDATE";

                DataGridViewTextBoxColumn cal2 = new DataGridViewTextBoxColumn();
                dgvLeaveReq.Columns.RemoveAt(4);
                dgvLeaveReq.Columns.Insert(4, cal2);
                dgvLeaveReq.Columns[4].HeaderText = "End DT";
                dgvLeaveReq.Columns[4].Name = "EDATE";
                dgvLeaveReq.Columns[4].DataPropertyName = "EDATE";

                DataGridViewTextBoxColumn txt2 = new DataGridViewTextBoxColumn();
                dgvLeaveReq.Columns.RemoveAt(2);
                dgvLeaveReq.Columns.Insert(2, txt2);
                dgvLeaveReq.Columns[2].HeaderText = "Consumption Type";
                dgvLeaveReq.Columns[2].DataPropertyName = "CONSTYPE";
                dgvLeaveReq.Columns[2].Name = "CONSTYPE";

                this.ControlsEnable();
                MessageBox.Show("Records saved successfully...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.MOVEBTNSTATE(true, true, false, false);
                this.ActionButtonState(true, false, false, false, true,true,true,true);

                oDataAccess.CommitTransaction();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Records could not be saved...!!! " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                oDataAccess.RollbackTransaction();
                return;
            }

        }
        #endregion

        #region Display record in view mode
        private void showrecord()
        {
            if (tDs.Tables[0].Rows.Count > 0)
            {
                txtApplNo.Text = tDs.Tables[0].Rows[0]["APPID"].ToString();
                txtDate.Text = tDs.Tables[0].Rows[0]["DATE"].ToString().Substring(0, 10);
                txtEmp.Text = tDs.Tables[0].Rows[0]["EMPNM"].ToString();
                txtRemarks.Text = tDs.Tables[0].Rows[0]["REMARKS"].ToString();
                dgvLeaveReq.DataSource = tDs.Tables[0];

                // this.set_total_grid_structure();
                sqlQuery = "select [LEAVETYPE]='Total',[LEAVENM]='',[SDATE]='',[EDATE]='',[CONSTYPE]='',sum([TOT_LEAVE]) as [TOT_LEAVE],sum([USED_LEAVE]) as [USED_LEAVE],sum([BAL_LEAVE]) as [BAL_LEAVE],sum([REQ_LEAVE]) as [REQ_LEAVE],";
                sqlQuery += "[STATUS]='',[APPR_BY]='',[APPR_DT]='',[APPR_REMARKS]='' ";
                sqlQuery += " from Emp_Leave_Request_ITEM WHERE APPID=" + txtApplNo.Text + " group by appid";
                // tDs.Tables[1].Select;
                dstot = oDataAccess.GetDataSet(sqlQuery, null, 25);

                dgvTotal.DataSource = dstot.Tables[0];
                dgvTotal.Columns[0].Width = 100;
                dgvTotal.Columns[2].Width = 80;
                dgvTotal.Columns[3].Width = 80;
                dgvTotal.Columns[5].Width = 80;
                dgvTotal.Columns[6].Width = 80;
                dgvTotal.Columns[9].Width = 70;
                dgvTotal.Columns[10].Width = 200;
                dgvTotal.Columns[11].Width = 70;
                dgvTotal.Columns[12].Width = 180;

            }
            if (tDs.Tables[0].Rows.Count == 0)
            {
                tDs.Tables[0].Rows.Clear();
                dgvLeaveReq.DataSource = tDs.Tables[0];
                dgvLeaveReq.Refresh();
                txtApplNo.Text = "";
                txtDate.Text = "";
                txtEmp.Text = "";
                txtRemarks.Text = "";

                sqlQuery = "select [LEAVETYPE]='Total',[LEAVENM]='',[SDATE]='',[EDATE]='',[CONSTYPE]='',sum([TOT_LEAVE]) as [TOT_LEAVE],sum([USED_LEAVE]) as [USED_LEAVE],sum([BAL_LEAVE]) as [BAL_LEAVE],sum([REQ_LEAVE]) as [REQ_LEAVE],";
                sqlQuery += "[STATUS]='',[APPR_BY]='',[APPR_DT]='',[APPR_REMARKS]='' ";
                sqlQuery += " from Emp_Leave_Request_ITEM WHERE 1=2";
                // tDs.Tables[1].Select;
                dstot = oDataAccess.GetDataSet(sqlQuery, null, 25);
                dgvTotal.DataSource = dstot;
            }
        }
        #endregion

        #region VIEW FIRST,NEXT,PREVIOUS AND LAST RECORD

        #region Move to Last record
        private void btnLast_Click(object sender, EventArgs e)
        {
            sqlQuery = "select A.APPID,A.DATE,A.EMPNM,A.EMPCODE,A.REMARKS,B.LEAVETYPE,B.LEAVENM,B.CONSTYPE,B.SDATE,B.EDATE,B.TOT_LEAVE,B.USED_LEAVE,B.BAL_LEAVE,B.REQ_LEAVE,B.STATUS,B.APPR_BY,B.APPR_DT,B.APPR_REMARKS";
            sqlQuery += " FROM Emp_Leave_Request_Head A";
            sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
            sqlQuery += " WHERE A.APPID=(SELECT TOP 1 C.APPID FROM Emp_Leave_Request_Head C ORDER BY C.APPID DESC)";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            if (tDs.Tables[0].Rows.Count == 0)
            {

                this.MOVEBTNSTATE(false, false, false, false);
                this.ActionButtonState(true, false, false, false, false,false,false,false);
            }
            else
            {
                this.MOVEBTNSTATE(true, true, false, false);
            }
            showrecord();
        }
        #endregion

        #region Move to first record
        private void btnFirst_Click(object sender, EventArgs e)
        {
            sqlQuery = "select A.APPID,A.DATE,A.EMPNM,A.EMPCODE,A.REMARKS,B.LEAVETYPE,B.LEAVENM,B.SDATE,B.EDATE,B.CONSTYPE,B.TOT_LEAVE,B.USED_LEAVE,B.BAL_LEAVE,B.REQ_LEAVE,B.STATUS,B.APPR_BY,B.APPR_DT,B.APPR_REMARKS";
            sqlQuery += " FROM Emp_Leave_Request_Head A";
            sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
            sqlQuery += " WHERE A.APPID=(SELECT TOP 1 C.APPID FROM Emp_Leave_Request_Head C)";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            this.MOVEBTNSTATE(false, false, true, true);
            showrecord();
        }
        #endregion

        #region Move to next record
        private void btnForward_Click(object sender, EventArgs e)
        {
            sqlQuery = "select A.APPID,A.DATE,A.EMPNM,A.EMPCODE,A.REMARKS,B.LEAVETYPE,B.LEAVENM,B.SDATE,B.EDATE,B.CONSTYPE,B.TOT_LEAVE,B.USED_LEAVE,B.BAL_LEAVE,B.REQ_LEAVE,B.STATUS,B.APPR_BY,B.APPR_DT,B.APPR_REMARKS";
            sqlQuery += " FROM Emp_Leave_Request_Head A";
            sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
            sqlQuery += " WHERE A.APPID=(SELECT TOP 1 C.APPID FROM Emp_Leave_Request_Head C WHERE C.APPID>" + Convert.ToInt32(txtApplNo.Text) + ")";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);

            if (tDs.Tables[0].Rows.Count != 0)
            {
                this.MOVEBTNSTATE(true, true, true, true);
                showrecord();
            }
            else
            {
                this.MOVEBTNSTATE(true, true, false, false);
            }

        }
#endregion

        #region Move To previous record
        private void btnBack_Click(object sender, EventArgs e)
        {
            sqlQuery = "select A.APPID,A.DATE,A.EMPNM,A.EMPCODE,A.REMARKS,B.LEAVETYPE,B.LEAVENM,B.SDATE,B.EDATE,B.CONSTYPE,B.TOT_LEAVE,B.USED_LEAVE,B.BAL_LEAVE,B.REQ_LEAVE,B.STATUS,B.APPR_BY,B.APPR_DT,B.APPR_REMARKS";
            sqlQuery += " FROM Emp_Leave_Request_Head A";
            sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
            sqlQuery += " WHERE A.APPID=(SELECT TOP 1 C.APPID FROM Emp_Leave_Request_Head C WHERE C.APPID<" + Convert.ToInt32(txtApplNo.Text) + " ORDER BY C.APPID DESC)";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);

            if (tDs.Tables[0].Rows.Count != 0)
            {
                this.MOVEBTNSTATE(true, true, true, true);
                showrecord();
            }
            else
            {
                this.MOVEBTNSTATE(false,false,true,true);
            }
        }
        #endregion

        #endregion

        #region Enable disable [Action and move] menu
        private void MOVEBTNSTATE(bool first,bool back,bool next,bool last)
        {
            btnFirst.Enabled = first;
            btnBack.Enabled = back;
            btnForward.Enabled = next;
            btnLast.Enabled = last;
        }

        private void ActionButtonState(bool New, bool Edit, bool Save, bool Cancel, bool Delete,bool Preview,bool Print,bool Export)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
            btnDelete.Enabled = Delete;
            btnPreview.Enabled = Preview;
            btnPrint.Enabled = Print;
            btnExportPdf.Enabled = Export;
        }
        #endregion

        #region Cancel Record
        private void btnCancel_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (this.pAddMode)
            {
                this.pAddMode = false;
                this.pEditMode = false;

                dgvLeaveReq.Columns[0].Frozen = false;
                dgvLeaveReq.Columns[1].Frozen = false;
                dgvLeaveReq.Columns[2].Frozen = false;
                dgvLeaveReq.Columns[3].Frozen = false;
                dgvLeaveReq.Columns[4].Frozen = false;
                dgvLeaveReq.Columns[5].Frozen = false;
                dgvLeaveReq.Columns[6].Frozen = false;
                dgvLeaveReq.Columns[7].Frozen = false;
                DataGridViewTextBoxColumn cmb1 = new DataGridViewTextBoxColumn();
                cmb1.HeaderText = "Leave Type";
                cmb1.DataPropertyName = "LEAVETYPE";
                cmb1.Name = "LEAVETYPE";
                dgvLeaveReq.Columns.RemoveAt(0);
                dgvLeaveReq.Columns.Insert(0, cmb1);

                DataGridViewTextBoxColumn cal1 = new DataGridViewTextBoxColumn();
                dgvLeaveReq.Columns.RemoveAt(3);
                dgvLeaveReq.Columns.Insert(3, cal1);
                dgvLeaveReq.Columns[3].HeaderText = "Start DT";
                dgvLeaveReq.Columns[3].Name = "SDATE";
                dgvLeaveReq.Columns[3].DataPropertyName = "SDATE";

                DataGridViewTextBoxColumn cal2 = new DataGridViewTextBoxColumn();
                dgvLeaveReq.Columns.RemoveAt(4);
                dgvLeaveReq.Columns.Insert(4, cal2);
                dgvLeaveReq.Columns[4].HeaderText = "End DT";
                dgvLeaveReq.Columns[4].Name = "EDATE";
                dgvLeaveReq.Columns[4].DataPropertyName = "EDATE";

                DataGridViewTextBoxColumn cmb2 = new DataGridViewTextBoxColumn();
                dgvLeaveReq.Columns.RemoveAt(2);
                dgvLeaveReq.Columns.Insert(2, cmb2);
                dgvLeaveReq.Columns[2].HeaderText = "Consumption Type";
                dgvLeaveReq.Columns[2].DataPropertyName = "CONSTYPE";
                dgvLeaveReq.Columns[2].Name = "CONSTYPE";

                this.ControlsEnable();
                this.btnLast_Click(sender, e);
                
            }
            else
            {
                sqlQuery = "select A.APPID,A.DATE,A.EMPNM,A.EMPCODE,A.REMARKS,B.LEAVETYPE,B.LEAVENM,B.SDATE,B.EDATE,B.CONSTYPE,B.TOT_LEAVE,B.USED_LEAVE,B.BAL_LEAVE,B.REQ_LEAVE,B.STATUS,B.APPR_BY,B.APPR_DT,B.APPR_REMARKS";
                sqlQuery += " FROM Emp_Leave_Request_Head A";
                sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
                sqlQuery += " WHERE A.APPID=" + Convert.ToInt32(txtApplNo.Text) + "";
                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                this.pAddMode = false;
                this.pEditMode = false;
                this.ControlsEnable();
                showrecord();
            }
        }
        #endregion

        #region Delete Record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            if (this.validation() == false)
            {
                MessageBox.Show("You can not delete this Leave Request", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                DialogResult dres;
                dres = MessageBox.Show("Do you want to delete this record...!!! ", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dres.Equals(DialogResult.Yes))
                {
                    oDataAccess.BeginTransaction();
                    //insert into Emp_Leave_Request_Head table Strat
                    sqlQuery = "Delete from Emp_Leave_Request_Item where appid=" + Convert.ToInt32(txtApplNo.Text) + "";
                    oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);
                    sqlQuery = "Delete from Emp_Leave_Request_Head where appid="+Convert.ToInt32(txtApplNo.Text)+"";
                    oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);
                    MessageBox.Show("Records Deleted Successfuly...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    oDataAccess.CommitTransaction();
                    this.ControlsEnable();
                    this.btnLast_Click(sender, e);

 
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Records could not be Deleted...!!! " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                oDataAccess.RollbackTransaction();
                return;
            }
        }
        #endregion

        #region Select Application number Popup
        private void btnApplNo_Click(object sender, EventArgs e)
        {

            sqlQuery = "select cast(APPID as varchar) as APPID,DATE,EMPNM,EMPCODE from Emp_Leave_Request_Head";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Leave Request Application";
            vSearchCol = "APPID";
            vDisplayColumnList = "APPID:Application ID,DATE:Application Date,EMPNM:Employee Name,EMPCODE:Employee Code";
            vReturnCol = "APPID";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                sqlQuery = "select A.APPID,A.DATE,A.EMPNM,A.EMPCODE,A.REMARKS,B.LEAVETYPE,B.LEAVENM,B.SDATE,B.EDATE,B.CONSTYPE,B.TOT_LEAVE,B.USED_LEAVE,B.BAL_LEAVE,B.REQ_LEAVE,B.STATUS,B.APPR_BY,B.APPR_DT,B.APPR_REMARKS";
                sqlQuery += " FROM Emp_Leave_Request_Head A";
                sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
                sqlQuery += " WHERE A.APPID=" + Convert.ToInt32(oSelectPop.pReturnArray[0]) + "";
                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                showrecord();

            }
        }
        #endregion

        #region Give colour to status column of grid
        private void dgvLeaveReq_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "STATUS" && this.pAddMode == false)
            {
                if (e.Value.ToString().Trim() == "Rejected")
                {
                    e.CellStyle.BackColor = Color.Tomato;
                }
                else if (e.Value.ToString().Trim() == "Approved")
                {
                    e.CellStyle.BackColor = Color.Chartreuse;
                }
                else
                {
                    e.CellStyle.BackColor = Color.PeachPuff;
                }
            }

        }

        private void dgvOverview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (dgvOverview.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value.ToString().Trim() == "Rejected")
                {
                    e.CellStyle.BackColor = Color.Tomato;
                }
                else if (e.Value.ToString().Trim() == "Approved")
                {
                    e.CellStyle.BackColor = Color.Chartreuse;
                }
                else
                {
                    e.CellStyle.BackColor = Color.PeachPuff;
                }
            }

        }
        #endregion


        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.pEditMode = true;
            this.pAddMode = false;
            this.ControlsEnable();
            
        }



        private void mDeleteProcessIdRecord()  
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 25, true);
        }

        private void frmEpmLeaveRequest_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void dgvLeaveReq_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLeaveReq.Rows.Count != 0 && e.RowIndex >= 0 && this.pAddMode)
            {
                if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "LEAVETYPE" && this.pAddMode && dgvLeaveReq.Rows[e.RowIndex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() != "")
                {
                    sqlQuery = "select att_nm as leave_nm from Emp_Attendance_Setting where att_code='" + dgvLeaveReq.Rows[e.RowIndex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "'";
                    DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                    DataView dvw = DS1.Tables[0].DefaultView;
                    dgvLeaveReq.Rows[e.RowIndex].Cells["LEAVENM"].Value = DS1.Tables[0].Rows[0]["leave_nm"].ToString();
                    if (dgvLeaveReq.Rows[e.RowIndex].Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() != "")
                    {
                        this.calculate_leave(e.RowIndex);
                    }
                }

                    if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "EDATE" && dgvLeaveReq.Rows[e.RowIndex].Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() != "")
                    {
                        this.calculate_leave(e.RowIndex);
                    }
                    if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "CONSTYPE" && dgvLeaveReq.Rows[e.RowIndex].Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() != "")
                    {
                        this.calculate_leave(e.RowIndex);
                    }
            }

        }

        private void calculate_leave(int rindex)
        {
            LeaveOpening = dgvLeaveReq.Rows[rindex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "_OpBal";
            LeaveCredit = dgvLeaveReq.Rows[rindex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "_Credit";
            LeaveAvail = dgvLeaveReq.Rows[rindex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "_Availed";
            LeaveEncash = dgvLeaveReq.Rows[rindex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "_EnCash";
            LeaveBalance = dgvLeaveReq.Rows[rindex].Cells["LEAVETYPE"].EditedFormattedValue.ToString().Trim() + "_Balance";
            sqlQuery = "execute('select '+'" + LeaveOpening + "'+' as Opening,'+'" + LeaveCredit + "'+' as Credit,'+'" + LeaveAvail + "'+' as Availed,'+'" + LeaveEncash + "'+' as Encash,'+'" + LeaveBalance + "'+' as Balnace from Emp_Leave_Maintenance where employeecode=''" + EmpCode + "'' and pay_month=" + Convert.ToDateTime(dgvLeaveReq.Rows[rindex].Cells["SDATE"].EditedFormattedValue).Month + " and pay_year=" + Convert.ToDateTime(dgvLeaveReq.Rows[rindex].Cells["SDATE"].EditedFormattedValue).Year + "')";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = DS1.Tables[0].DefaultView;

            if (DS1.Tables[0].Rows.Count != 0)
            {
                tDs.Tables[0].Rows[rindex]["TOT_LEAVE"] = Convert.ToString(Convert.ToDecimal(DS1.Tables[0].Rows[0]["Opening"]) + Convert.ToDecimal(DS1.Tables[0].Rows[0]["Credit"])); ;
                tDs.Tables[0].Rows[rindex]["USED_LEAVE"] = Convert.ToString(Convert.ToDecimal(DS1.Tables[0].Rows[0]["Availed"]) + Convert.ToDecimal(DS1.Tables[0].Rows[0]["Encash"]));
                tDs.Tables[0].Rows[rindex]["BAL_LEAVE"] = DS1.Tables[0].Rows[0]["Balnace"].ToString();
                if (dgvLeaveReq.Rows[rindex].Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() == "Half Day")
                {
                    tDs.Tables[0].Rows[rindex]["REQ_LEAVE"] = "0.5";
                }
                else
                {
                    tDs.Tables[0].Rows[rindex]["REQ_LEAVE"] = Convert.ToString((Convert.ToDateTime(dgvLeaveReq.Rows[rindex].Cells["EDATE"].EditedFormattedValue) - Convert.ToDateTime(dgvLeaveReq.Rows[rindex].Cells["SDATE"].EditedFormattedValue)).TotalDays + 1);
                }

            }
            else
            {
                tDs.Tables[0].Rows[rindex]["TOT_LEAVE"] = "0";
                tDs.Tables[0].Rows[rindex]["USED_LEAVE"] = "0";
                tDs.Tables[0].Rows[rindex]["BAL_LEAVE"] = "0";
                if (dgvLeaveReq.Rows[rindex].Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() == "Half Day")
                {
                    tDs.Tables[0].Rows[rindex]["REQ_LEAVE"] = "0.5";
                }
                else
                {
                    tDs.Tables[0].Rows[rindex]["REQ_LEAVE"] = Convert.ToString((Convert.ToDateTime(dgvLeaveReq.Rows[rindex].Cells["EDATE"].EditedFormattedValue) - Convert.ToDateTime(dgvLeaveReq.Rows[rindex].Cells["SDATE"].EditedFormattedValue)).TotalDays + 1);
                }

            }
            
            this.cal_total();
            dgvLeaveReq.Refresh();
        }


        private void dgvLeaveReq_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.pAddMode )
            {
                if (dgvLeaveReq.Rows.Count == 0)
                {
                    deleteRowToolStripMenuItem1.Visible = false;
                }
                else
                {
                    deleteRowToolStripMenuItem1.Visible = true;
                }
                this.contextMenuStrip1.Show(this.dgvLeaveReq, Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void AddRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.validation() == false)
            {
                return;
            }

            this.pAddMode = true;

            dr=tDs.Tables[0].NewRow();

            foreach (DataColumn dc in tDs.Tables[0].Columns)
            {
                switch (dc.DataType.ToString())
                {
                    case "System.String":
                        dr[dc] = "";
                        break;
                    case "System.DateTime":
                        dr[dc] = DateTime.Now.Date;
                        break;
                    case "System.Int32":
                        dr[dc] = 0;
                        break;
                    case "System.Decimal":
                        dr[dc] = 0.00;
                        break;
                    default:
                        break;
                }

            }
            dr["STATUS"] = "Pending";
            dr["APPR_DT"]=@"01/01/1900";
            tDs.Tables[0].Rows.Add(dr);
            dgvLeaveReq.Focus();
            dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[tDs.Tables[0].Rows.Count-1].Cells["LEAVETYPE"];
     
        }

        private void deleteRowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
                if (dgvLeaveReq.SelectedRows.Count > 0)
                {
                if (dgvLeaveReq.Rows[this.rowindex].Cells["STATUS"].Value.ToString().Trim() == "Approved" || dgvLeaveReq.Rows[this.rowindex].Cells["STATUS"].Value.ToString().Trim() == "Rejected")
                {
                    MessageBox.Show("You can not delete this record", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                    dgvTotal.Rows[0].Cells[5].Value = Convert.ToString(Convert.ToDecimal(dgvTotal.Rows[0].Cells[5].Value) - Convert.ToDecimal(dgvLeaveReq.Rows[this.rowindex].Cells["TOT_LEAVE"].Value));
                    dgvTotal.Rows[0].Cells[6].Value = Convert.ToString(Convert.ToDecimal(dgvTotal.Rows[0].Cells[6].Value) - Convert.ToDecimal(dgvLeaveReq.Rows[this.rowindex].Cells["USED_LEAVE"].Value));
                    dgvTotal.Rows[0].Cells[7].Value = Convert.ToString(Convert.ToDecimal(dgvTotal.Rows[0].Cells[7].Value) - Convert.ToDecimal(dgvLeaveReq.Rows[this.rowindex].Cells["BAL_LEAVE"].Value));
                    dgvTotal.Rows[0].Cells[8].Value = Convert.ToString(Convert.ToDecimal(dgvTotal.Rows[0].Cells[8].Value) - Convert.ToDecimal(dgvLeaveReq.Rows[this.rowindex].Cells["REQ_LEAVE"].Value));
                    this.dgvLeaveReq.Rows.RemoveAt(this.rowindex);
                }
            
        }

        private void dgvLeaveReq_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvLeaveReq_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (this.pAddMode && dgvLeaveReq.Rows.Count != 0)
            {
                if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "LEAVETYPE" && dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim() == "")
                {

                    MessageBox.Show("Please select Leave type");
                    dgvLeaveReq.ClearSelection();
                    dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    e.Cancel = true;

                }

                if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "CONSTYPE" && dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim() == "")
                {


                    dgvLeaveReq.ClearSelection();
                    dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    e.Cancel = true;
                    dgvLeaveReq.Columns[e.ColumnIndex].Selected = true;

                }
                    if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "SDATE" && this.pAddMode)
                {
                    sqlQuery = "select SDATE from Emp_Leave_Request_Item where empcode='" + EmpCode + "' and (convert(varchar(10),SDATE,103)='" + dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Substring(0, 10) + "' OR convert(varchar(10),EDATE,103)='" + dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Substring(0, 10) + "')";
                    DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                    if (DS1.Tables[0].Rows.Count != 0)
                    {
                        MessageBox.Show("Leave already applied", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvLeaveReq.ClearSelection();
                        dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex];

                        e.Cancel = true;

                    }
                    if (e.RowIndex>0)
                    {
                    foreach (DataGridViewRow dr in dgvLeaveReq.Rows)
                    {
                        if (dr.Index != e.RowIndex)
                        {
                            if (dgvLeaveReq.Rows[e.RowIndex].Cells["SDATE"].EditedFormattedValue.ToString().Trim() == dr.Cells["SDATE"].Value.ToString().Substring(0, 10) || dgvLeaveReq.Rows[e.RowIndex].Cells["SDATE"].EditedFormattedValue.ToString().Trim() == dr.Cells["EDATE"].Value.ToString().Substring(0, 10))
                            {


                                MessageBox.Show("Leave already applied", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgvLeaveReq.ClearSelection();
                                dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex];
                                e.Cancel = true;
                                break;
                            }
                        }
                    }
                    
                        }
                    if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "EDATE" && this.pAddMode)
                    {
                        sqlQuery = "select EDATE from Emp_Leave_Request_Item where empcode='" + EmpCode + "' and convert(varchar(10),EDATE,103)='" + dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Substring(0, 10) + "' OR convert(varchar(10),SDATE,103)='" + dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Substring(0, 10) + "'";
                        DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                        if (DS1.Tables[0].Rows.Count != 0)
                        {
                            MessageBox.Show("Leave already applied", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvLeaveReq.ClearSelection();
                            dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex];

                            e.Cancel = true;

                        }
                    }
                }

                if (dgvLeaveReq.Columns[e.ColumnIndex].Name == "EDATE" && this.pAddMode)
                {
                    if ((Convert.ToDateTime(dgvLeaveReq.Rows[e.RowIndex].Cells["EDATE"].EditedFormattedValue) - Convert.ToDateTime(dgvLeaveReq.Rows[e.RowIndex].Cells["SDATE"].EditedFormattedValue)).TotalDays < 0)
                    {
                        MessageBox.Show("End date can not be less than start date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvLeaveReq.ClearSelection();
                        dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        e.Cancel = true;
                    }

                    if (dgvLeaveReq.Rows[e.RowIndex].Cells["CONSTYPE"].EditedFormattedValue.ToString().Trim() == "Half Day" && ((Convert.ToDateTime(dgvLeaveReq.Rows[e.RowIndex].Cells["EDATE"].EditedFormattedValue) - Convert.ToDateTime(dgvLeaveReq.Rows[e.RowIndex].Cells["SDATE"].EditedFormattedValue)).TotalDays > 0))
                    {
                        MessageBox.Show("Please select valid date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvLeaveReq.ClearSelection();
                        dgvLeaveReq.CurrentCell = dgvLeaveReq.Rows[e.RowIndex].Cells["EDATE"];
                        e.Cancel = true;
                    }
                }
            }
        }

        private void dgvLeaveReq_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmEpmLeaveRequest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        this.btnNew_Click(sender, e);
                        break;
                    case Keys.Z:
                        this.btnCancel_Click(sender, e);
                        break;
                    case Keys.S:
                        this.btnSave_Click(sender, e);
                        break;
                    case Keys.D:
                        this.btnDelete_Click(sender, e);
                        break;
                    default:
                        break;

                }

            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.mthPrint(2);
        }


        #region Print And Email
        private void mthPrint(Int16 vPrintOption)
        {
            string vRepGroup = "Employee Leave Request";
            this.mthDsCommon();
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
            
            oPrint.pSpPara = "" + this.txtApplNo.Text.Trim() + ",'" + this.txtEmp.Text.Trim() + "'";
            oPrint.pPrintOption = vPrintOption;
            oPrint.Main();
            if (vPrintOption == 4)
            {
                MessageBox.Show("PDF Generated at "+this.pAppPath+@"\PDF_Files", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }
        }
        #endregion

        #region Common Datatables
        private void mthDsCommon()
        {
            
            string SqlStr;
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";

            SqlStr = "Select * From vudyog..Co_Mast where CompId=" + pPara[0];
            company = oDataAccess.GetDataTable(SqlStr, null, 25);

            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            SqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, 25);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

        }
        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.mthPrint(3);
        }


        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            this.mthPrint(4);
        }


    }
}

