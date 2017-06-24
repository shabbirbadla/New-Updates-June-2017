using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using uBaseForm;
using System.Diagnostics;
using System.Collections;
using DataAccess_Net;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Web;
using udclsDGVDateTimePicker;
using uCheckBox;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpMusterGeneration
{

    public partial class frmUdMuster : uBaseForm.FrmBaseForm
    {

        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsLvMaster = new DataSet();
        DataSet dsLvBal = new DataSet(); /*Ramya 07/08/12*/
        DataView dvwFilt = new DataView();
        
        string vYear = string.Empty, vMonth = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;

        DataSet dsGrd = new DataSet();
        string SqlStr = string.Empty;
        String cAppPId, cAppName;
        string vCurCol = string.Empty;
        Decimal vCurVal = 0;
        int vRowIndex = 0;
        Decimal vMonthDays = 0;
        Boolean vIsFrmValid = true;
        Boolean vIsCancel = false;
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmUdMuster(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPara = args;
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
            //this.pFrmCaption = "Muster Creation";
            this.pFrmCaption = "Monthly Muster";
            
        }
        private void frmUdMuster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;


            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            this.btnFirst.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnForward.Enabled = false;
            this.btnBack.Enabled = false;

            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnExportPdf.Enabled = false;

            this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;

            this.lblSVal.Visible = false;
            this.txtSVal.Visible = false;
            this.btnSVal.Visible = false;



            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            SqlStr = "select Distinct Att_Code as LV_Code,Att_Nm as LV_Nm,SortOrd,Seffect=SalPaidDayseffect from Emp_Attendance_Setting Where LDeactive=0 order by SortOrd";
            dsLvMaster = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            SqlStr = "select Distinct att_Code as Lv_Codebal,SortOrd,Att_Nm as LV_Nm  From Emp_Attendance_Setting where isleave=1 and LDeactive=0 order by SortOrd";
            dsLvBal = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            int vint = 0;
            string pyear = "";
            
            SqlStr = "select isnull(max(Pay_Year),0) as LvYear from Emp_Payroll_Year_Master";
            DataSet dsTemp = new DataSet();
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {

               // vint = Convert.ToInt16(dsTemp.Tables[0].Rows[0]["LvYear"].ToString());  /*07/08/12 need to change */

               pyear = dsTemp.Tables[0].Rows[0]["LvYear"].ToString();
                //if (vint > 0)
                //   this.txtYear.Text = pyear;
               this.txtYear.Text = pyear;
            }
            SqlStr = "select isnull(max(Pay_Month),0) as LvMonth from Emp_Leave_Maintenance where Pay_Year='" + pyear+"'";   /*Ramya*/
            dsTemp = new DataSet();
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                vint = Convert.ToInt16(dsTemp.Tables[0].Rows[0]["LvMonth"].ToString());
                if (vint > 0)
                {
                    this.txtMonth.Text = this.fnCMonth(vint);
                }
                else
                {
                    this.txtMonth.Text = this.fnCMonth(DateTime.Now.Month);
                }
            }

            this.mthdvwFilt();
            this.SetMenuRights();

            this.pAddButton = false;
            this.pDeleteButton = false;
         
            this.mInsertProcessIdRecord();
            this.SetFormColor();

           

            string appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }

            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnOthDet.Image = Image.FromFile(fName);
                this.btnYear.Image = Image.FromFile(fName);
                this.btnMonth.Image = Image.FromFile(fName);
                this.btnEmpNm.Image = Image.FromFile(fName);
                
            }
            fName = appPath + @"\bmp\loc-on.gif";

            //Added by Archana K. on 16/05/13 for Bug-7899 start
            startupPath = Application.StartupPath;
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
            //Added by Archana K. on 16/05/13 for Bug-7899 end
            this.mthView();
            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            
        }

        private void mthView()
        {
            //Added by Archana K. on 17/05/13 for Bug-7899 start
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
                this.gbEmpDet.Enabled = false;
                this.gbPeriod.Enabled = false;
                this.chkSelectAll.Enabled = false;
                this.dgvMain.ReadOnly = true;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            this.mthEnableDisableFormControls();

            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            //this.mthBindData();

        }
        private void mthGrdRefresh()
        {
            //if (this.txtYear.Text.Trim() == "")
            //{
            //    this.txtYear.Text = "0";
            //}

            SqlStr = "Execute [usp_Ent_Emp_Monthly_Muster] ";//'','','','','','',''
            SqlStr = SqlStr + "'" + this.txtYear.Text.Trim() + "'";
            SqlStr = SqlStr + "," + this.fnNMonth(this.txtMonth.Text).ToString().Trim();
            SqlStr = SqlStr + ",'" + this.txtLocNm.Text.Trim() + "'";
            SqlStr = SqlStr + ",''";// + this.txtDept.Text.Trim() + "'";
            SqlStr = SqlStr + ",'" + this.txtEmpNm.Text.Trim() + "'";
            ////if (string.IsNullOrEmpty(this.txtEmpNm.Text.Trim()) == false)


             dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthBindData();
        }
        private void mthFldRefresh(int rInd)
        {
            if (dgvMain.Rows.Count == 0 || dgvMain.Columns[0].Name == "id" || dgvMain.Rows[rInd].Cells["colid"].Value == null)
            {
                return;
            }
            //if (dgvMain.Columns[0].Name == "id")
            //{
            //    return;
            //}
            //if (dgvMain.Rows[rInd].Cells["colid"].Value == null)
            //{
            //    return;
            //}
            //{ colId, colYear, colCode, colAttNm, colIsLeave, colLvCFW, colLvEncash, colAutoCr, colLocation, colDept, colCate });

            vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();
            this.txtYear.Text = dgvMain.Rows[rInd].Cells["colYear"].Value.ToString();
            this.txtMonth.Text = dgvMain.Rows[rInd].Cells["colcMonth"].Value.ToString();
            this.txtLocNm.Text = dgvMain.Rows[rInd].Cells["colLocation"].Value.ToString();
            //this.txtOthDet.Text = dgvMain.Rows[rInd].Cells["colDept"].Value.ToString();
            this.txtEmpCode.Text = dgvMain.Rows[rInd].Cells["colEmpCode"].Value.ToString();
            this.txtEmpNm.Text = dgvMain.Rows[rInd].Cells["colEmp"].Value.ToString();
      }
        private void mthBindData()
        {

            this.dgvMain.Columns.Clear();
            this.dgvMain.DataSource = dsGrd.Tables[0];
            this.dgvMain.Columns.Clear();

            //DataGridViewColumn[100] ; 
            System.Windows.Forms.DataGridViewCheckBoxColumn ColSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColSel.HeaderText = "Select";
            ColSel.Name = "colSel";
            ColSel.ReadOnly = false;
            ColSel.Width = 75;      // Added by Sachin N. S. on 04/07/2014 for Bug-21114
            this.dgvMain.Columns.Add(ColSel);

            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Width = 75;      // Added by Sachin N. S. on 04/07/2014 for Bug-21114
            this.dgvMain.Columns.Add(colId);

            System.Windows.Forms.DataGridViewTextBoxColumn colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colYear.HeaderText = "Year";
            colYear.Name = "colYear";
            colYear.Width = 45;
            this.dgvMain.Columns.Add(colYear);
            colYear.Frozen = true;
            //System.Windows.Forms.DataGridViewTextBoxColumn colMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colMonth.HeaderText = "Month";
            //colMonth.Name = "colMonth";
            //this.dgvMain.Columns.Add(colMonth);

            System.Windows.Forms.DataGridViewTextBoxColumn colcMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colcMonth.HeaderText = "Month";
            colcMonth.Name = "colcMonth";
            colcMonth.Width = 60;
            this.dgvMain.Columns.Add(colcMonth);
            colcMonth.Frozen = true;
            colcMonth.ReadOnly = true;
            

            System.Windows.Forms.DataGridViewTextBoxColumn colEmp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmp.HeaderText = "Employee Name";
            colEmp.Name = "colEmp";
            colEmp.Width = 200;
            this.dgvMain.Columns.Add(colEmp);
            colEmp.Frozen = true;

            //***** Added by Sachin N. S. on 04/07/2014 for Bug-21114 -- Start
            System.Windows.Forms.DataGridViewTextBoxColumn colCalcPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCalcPeriod.HeaderText = "Calculation Period";
            colCalcPeriod.Name = "colCalcPeriod";
            colCalcPeriod.Width = 100;
            this.dgvMain.Columns.Add(colCalcPeriod);
            colCalcPeriod.Frozen = true;
            colCalcPeriod.ReadOnly = true;
            //***** Added by Sachin N. S. on 04/07/2014 for Bug-21114 -- End


            string VHTxt = string.Empty;
            foreach (DataRow dr in dsLvMaster.Tables[0].Rows)
            {
               // System.Windows.Forms.DataGridViewTextBoxColumn col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                udclsDGVNumericColumn.CNumEditDataGridViewColumn col1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                col1.DecimalLength = 3;
                col1.MaxInputLength = 8;
                col1.AllowNegative = false;
                VHTxt = dr["LV_Nm"].ToString();
                col1.HeaderText = VHTxt;
                col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (dr["Seffect"].ToString() != "") { col1.Tag = dr["Seffect"].ToString(); }
                VHTxt = "Col_" + dr["LV_Code"].ToString();
                col1.Name = VHTxt;
                col1.DataPropertyName = dr["LV_Code"].ToString();
                this.dgvMain.Columns.Add(col1);
                if (dr["LV_Code"].ToString().ToUpper() == "PR" || dr["LV_Code"].ToString().ToUpper() == "SalPaidDays" || dr["LV_Code"].ToString().ToUpper() == "LOP")
                {
                    col1.Frozen = true;
                }
                
            }

            foreach (DataRow dr in dsLvBal.Tables[0].Rows)
            {
               // System.Windows.Forms.DataGridViewTextBoxColumn col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                udclsDGVNumericColumn.CNumEditDataGridViewColumn col1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                col1.DecimalLength = 3;
                col1.MaxInputLength = 8;
                col1.AllowNegative = false;
                VHTxt = dr["LV_Nm"].ToString();
                col1.HeaderText =  dr["LV_Nm"].ToString().Trim() + " Balance";
                col1.DataPropertyName = dr["Lv_Codebal"].ToString().Trim() + "_Balance";
                col1.Name = "Col_" + dr["Lv_Codebal"].ToString().Trim() + "_Balance";
                col1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                this.dgvMain.Columns.Add(col1);
            }


            System.Windows.Forms.DataGridViewTextBoxColumn colEmpCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmpCode.HeaderText = "Employee Code";
            colEmpCode.Name = "colEmpCode";
            this.dgvMain.Columns.Add(colEmpCode);

            System.Windows.Forms.DataGridViewTextBoxColumn colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            this.dgvMain.Columns.Add(colLocation);

            //System.Windows.Forms.DataGridViewTextBoxColumn colDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colDept.HeaderText = "Department";
            //colDept.Name = "colDept";
            //this.dgvMain.Columns.Add(colDept);

            //System.Windows.Forms.DataGridViewTextBoxColumn colCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colCate.HeaderText = "Category";
            //colCate.Name = "colCate";
            //this.dgvMain.Columns.Add(colCate);

            this.dgvMain.Columns["colId"].Visible = false;
            this.dgvMain.Columns["colSel"].DataPropertyName = "sel";
            this.dgvMain.Columns["colId"].DataPropertyName = "Id";
            this.dgvMain.Columns["colYear"].DataPropertyName = "Pay_Year";
            this.dgvMain.Columns["colcMonth"].DataPropertyName = "cMonth";
            this.dgvMain.Columns["colEmpCode"].DataPropertyName = "EmployeeCode";
            this.dgvMain.Columns["colEmp"].DataPropertyName = "EmployeeName";
            this.dgvMain.Columns["colCalcPeriod"].DataPropertyName = "CalcPeriod";  // Added by Sachin N. S. on 04/07/2014 for Bug-21114
            //dgvMain.Columns["colEmp"].DisplayIndex = 5;


            this.dgvMain.Columns["colLocation"].DataPropertyName = "Loc_Desc";
            //dgvMain.Columns["colDept"].DataPropertyName = "Department";
            //dgvMain.Columns["colCate"].DataPropertyName = "Category";
        }

        private void mthBindClear()
        {
            
            this.txtYear.Text = "";
            this.txtMonth.Text = "";
            this.txtEmpCode.Text = "";
            this.txtEmpNm.Text = "";
            //this.chkLeave.Checked = false;
            this.txtLocNm.Text = "";
            this.txtOthDet.Text = "";
           

        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnLogout.Enabled)
                btnLogout_Click(this.btnExit, e);
        }

        private void btnYear_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Pay_Year,sDate,eDate from Emp_Payroll_Year_Master order by Pay_Year";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Payroll Year";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Payroll Year,sDate:Start Date,eDate:End Date";
            vReturnCol = "Pay_Year,sDate,eDate";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtYear.Text = oSelectPop.pReturnArray[0];
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthGrdRefresh();
                }
            }

            mthChkNavigationButton();  /*Ramya 26/09/12*/
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

            DataTable dt = new DataTable();
            DataColumn colMnth = new DataColumn();

            dt.Columns.Add(colMnth);
            for (int i = 1; i <= 12; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = this.fnCMonth(i);
                dt.Rows.Add(dr);
            }
            DataView dvw = new DataView();
            dvw = dt.DefaultView;
            VForText = "Select Month Name";
            vSearchCol = "Column1";
            vDisplayColumnList = "Column1:Month";
            vReturnCol = "Column1";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtMonth.Text = oSelectPop.pReturnArray[0];
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthGrdRefresh();
                }
            }
            mthChkNavigationButton();  /*Ramya 26/09/12*/
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnEmpNm.Enabled = false;
                this.btnYear.Enabled = false;
                this.btnMonth.Enabled = false;
                this.btnLocNm.Enabled = false;
                this.dgvMain.ReadOnly = false;
                this.dgvMain.Columns["colYear"].ReadOnly = true ;
                this.dgvMain.Columns["colcMonth"].ReadOnly = true;
                this.dgvMain.Columns["colEmp"].ReadOnly = true;
                this.dgvMain.Columns["colLocation"].ReadOnly = true;
                this.dgvMain.Columns["colEmpCode"].ReadOnly = true;
                this.dgvMain.Columns["Col_MonthDays"].ReadOnly = true;
                this.dgvMain.Columns["Col_WO"].ReadOnly = true;
                this.dgvMain.Columns["Col_SalPaidDays"].ReadOnly = true;
                this.dgvMain.Columns["Col_PR"].ReadOnly = true;
                this.dgvMain.Columns["Col_LOP"].ReadOnly = true;
                this.dgvMain.Columns["ColCalcPeriod"].ReadOnly = true;        // Added by Sachin N. S. on 09/07/2014 for Bug-21114
                foreach (DataRow dr in dsLvBal.Tables[0].Rows)
                {
                     this.dgvMain.Columns["Col_" + dr["Lv_Codebal"].ToString().Trim() + "_Balance"].ReadOnly=true;
                }
            }
            else
            {
                this.btnEmpNm.Enabled = true;
                this.btnYear.Enabled = true;
                this.btnMonth.Enabled = true;
                this.btnLocNm.Enabled = true;
                this.dgvMain.ReadOnly = true ;
            }

            this.txtYear.Enabled = vEnabled;
            this.txtEmpNm.Enabled = vEnabled;
            this.txtEmpCode.Enabled = vEnabled;
            this.txtLocNm.Enabled = vEnabled;

            string vControl = string.Empty;
           
        }
        private string fnCMonth(int mn)
        {
            string cmnth = string.Empty;
            switch (mn)
            {
                case 1:
                    cmnth = "January";
                    break;
                case 2:
                    cmnth = "February";
                    break;
                case 3:
                    cmnth = "March";
                    break;
                case 4:
                    cmnth = "April";
                    break;
                case 5:
                    cmnth = "May";
                    break;
                case 6:
                    cmnth = "June";
                    break;
                case 7:
                    cmnth = "July";
                    break;
                case 8:
                    cmnth = "August";
                    break;
                case 9:
                    cmnth = "September";
                    break;
                case 10:
                    cmnth = "October";
                    break;
                case 11:
                    cmnth = "November";
                    break;
                case 12:
                    cmnth = "December";
                    break;
            }
            return cmnth;
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
        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, vTimeOut);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    strSQL += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this.pPApplRange;
                    strSQL += "and dbo.func_decoder([user],'T') ='" + this.pAppUerName.Trim() + "'";

                }
            }
            dsRights = oDataAccess.GetDataSet(strSQL, null, vTimeOut);


            if (dsRights != null)
            {
                string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                int len = rights.Length;
                string newString = "";
                ArrayList rArray = new ArrayList();

                while (len > 2)
                {
                    newString = rights.Substring(0, 2);
                    rights = rights.Substring(2);
                    rArray.Add(newString);
                    len = rights.Length;
                }
                rArray.Add(rights);

                this.pAddButton = (rArray[0].ToString().Trim() == "IY" ? true : false);
                this.pEditButton = (rArray[1].ToString().Trim() == "CY" ? true : false);
                this.pDeleteButton = (rArray[2].ToString().Trim() == "DY" ? true : false);
                this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }
        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
             this.btnLogout.Enabled = true ;//changed by Archana K. on 20/05/13 for Bug-7899
            //this.btnLogout.Enabled = false;//Commenetd by Archana K. on 20/05/13 for Bug-7899
             if (ServiceType.ToUpper() != "VIEWER VERSION")//Added by Archana K. on 17/05/13 for Bug-7899
             {
                 if (this.pAddMode == false && this.pEditMode == false)
                 {
                     if (this.pAddButton)
                     {
                         this.btnNew.Enabled = true;
                     }
                     if (this.pEditButton)
                     {
                         this.btnEdit.Enabled = true;
                     }
                     if (this.pDeleteButton)
                     {
                         this.btnDelete.Enabled = true;
                     }
                     this.btnLogout.Enabled = true;
                     chkSelectAll.Checked = false; /*Ramya*/
                     chkSelectAll.Enabled = false;
                 }
                 else
                 {

                     this.btnSave.Enabled = true;
                     this.btnCancel.Enabled = true;
                     chkSelectAll.Enabled = true;
                 }
                 if (this.dsGrd.Tables[0].Rows.Count == 0 && this.pAddMode == false && this.pEditMode == false)
                 {
                     this.btnEdit.Enabled = false;
                     this.btnDelete.Enabled = false;
                 }
             }//Added by Archana K. on 17/05/13 for Bug-7899
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpMonthlyMuster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
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
        private void mcheckCallingApplication()/*Added Rup 07/03/2011*/
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
       

        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLocNm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEmpNm.Text.Trim()) == false)
            {
                MessageBox.Show("Employee Name is Selected ! Make it blank to Select the Location", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master union select '' as LocNm,'' as Loc_Code,'' as Add1,'' as Add2,'' as Add3,'' as [Location Name] order by Loc_Desc";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Location Name";
            vSearchCol = "LocNm";
            vDisplayColumnList = "LocNm:Location Name,Loc_Code:Location Code";
            vReturnCol = "LocNm,Loc_Code";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtLocNm.Text = oSelectPop.pReturnArray[0];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                this.mthGrdRefresh();
            }
        }

        private void btnEmpNm_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select E.EmployeeName as EmpNm,E.EmployeeCode as EmpCode,E.Department,E.Designation,E.Category,E.Grade from EmployeeMast E";
            SqlStr = SqlStr + " Left Join Loc_Master L on (L.Loc_Code=E.Loc_Code)";
            SqlStr = SqlStr + " Left Join Department D on (D.Dept=E.Department)";
            SqlStr = SqlStr + " Left Join Category C on (C.Cate=E.Category)";
            SqlStr = SqlStr + " Where 1=1";
            if (txtLocNm.Text.Trim() != "") { SqlStr = SqlStr + " and l.Loc_Desc='" + this.txtLocNm.Text.Trim() + "'"; }
            if (txtOthDet.Text.Trim() != "") { SqlStr = SqlStr + " and D.Dept='" + this.txtOthDet.Text.Trim() + "'"; }

            SqlStr = SqlStr + " Union Select '' as EmpNm,'' as EmpCode,'' as Department,'' as Designation,'' as Category,'' as Grade";

            SqlStr = SqlStr + " order by EmployeeName";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Employee Name";
            vSearchCol = "EmpNm";
            vDisplayColumnList = "EmpNm:Employee Name,EmpCode:Employee Code,Department:Department,Designation:Designation,Category:Category";
            vReturnCol = "EmpNm,EmpCode";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtEmpNm.Text = oSelectPop.pReturnArray[0];
                this.txtEmpCode.Text = oSelectPop.pReturnArray[1];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                this.mthGrdRefresh();
            }
        }
        private void mthdvwFilt()
        {
            DataTable dtFilt = new DataTable();

            DataColumn ColFldCap = new DataColumn();
            ColFldCap.ColumnName = "ColFldCap";
            dtFilt.Columns.Add(ColFldCap);

            DataColumn ColFldNm = new DataColumn();
            ColFldNm.ColumnName = "ColFldNm";
            dtFilt.Columns.Add(ColFldNm);


            string vSFlds = string.Empty, vFldNm = string.Empty,vFldCap=string.Empty ;
            DataSet tds = new DataSet();
            SqlStr = "Select SFlds=SearchFlds from MastCode where Code='EM'";
            tds = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (tds.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            int pos = 1;
            vSFlds = tds.Tables[0].Rows[0]["SFlds"].ToString();
            while (vSFlds.IndexOf("<<") > -1)
            {
                pos = vSFlds.IndexOf("<<");
                vSFlds = vSFlds.Substring(pos + 2, vSFlds.Length - pos - 2);
                pos = vSFlds.IndexOf(":");
                vFldNm = vSFlds.Substring(0, pos);

                vSFlds = vSFlds.Substring(pos + 1, vSFlds.Length - pos - 1);
                pos = vSFlds.IndexOf(">>");
                vFldCap = vSFlds.Substring(0, pos);
                if (vFldNm.IndexOf("EmployeeCode") < 0 && vFldNm.IndexOf("EmployeeName") < 0)
                {
                    DataRow drt = dtFilt.NewRow();
                    drt[0] = vFldCap;
                    drt[1] = vFldNm;
                    dtFilt.Rows.Add(drt);
                    vSFlds.Substring(pos + 1, vSFlds.Length - pos - 1);
                }
            }
            dvwFilt  = dtFilt.DefaultView;


        }
        private void btnOthDet_Click(object sender, EventArgs e)
        {
            if (dvwFilt.Table.Rows.Count <= 0)
            {
                return;
            }
            string VForText = string.Empty, vSearchCol = string.Empty,  Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            VForText = "Select Field Name";
            vSearchCol = "ColFldCap";
            vDisplayColumnList = "ColFldCap:Field Caption,ColFldNm:Field Name";
            vReturnCol = "ColFldCap,ColFldNm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvwFilt;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtOthDet.Text = oSelectPop.pReturnArray[0].Trim();
                this.lblSVal.Visible = true;
                this.txtSVal.Visible = true;
                this.btnSVal.Visible = true;
                this.txtSVal.Tag = oSelectPop.pReturnArray[1].Trim();
            }
            else 
            {
                this.lblSVal.Visible = false ;
                this.txtSVal.Visible = false ;
                this.btnSVal.Visible = false ;

            }

        }

        private void btnSVal_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEmpNm.Text.Trim()) == false)
            {
                MessageBox.Show("Employee Name is Selected ! Make it blank to Select the Location", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.txtOthDet.Text.Trim()=="")
            {
                return;
            }
            DataSet tDs=new DataSet();
            SqlStr = "Select distinct E." + txtSVal.Tag + " as FldVal From EmployeeMast E ";
            if (this.txtLocNm.Text.Trim() != "")
            {
                SqlStr = SqlStr + " Left Join Loc_Master Lc on (Lc.Loc_Code=E.Loc_Code) ";
                SqlStr = SqlStr + " where  Lc.Loc_Desc='" + this.txtLocNm.Text.Trim() + "'"; 
            }
            //SqlStr =SqlStr+" where " + txtSVal.Tag + " as FldVal From EmployeeMast order by " + txtSVal.Tag;
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (tDs.Tables[0].Rows.Count ==0)
            {
                return;
            }
            
            string VForText = string.Empty, vSearchCol = string.Empty,  Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            VForText = "Select Field Value";
            vSearchCol = "FldVal";
            vDisplayColumnList = "FldVal:Field Value";
            vReturnCol = "FldVal";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = tDs.Tables[0].DefaultView;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtSVal.Text = oSelectPop.pReturnArray[0].Trim();
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = true;
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            vIsCancel = true;
            txtEmpNm.Text = "";
            txtEmpCode.Text = "";
            txtLocNm.Text = "";
            
            this.mthGrdRefresh();   
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();

        }

        private void dgvMain_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show(((DataGridView)sende);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            this.label3.Focus();
            this.dgvMain.Refresh();

            vIsFrmValid = true;
            this.mthValidation(ref vIsFrmValid);
            if (vIsFrmValid == false)
            {
                vIsFrmValid = true;
                return;
            }
            //vMonthDays 
            // Rup 04Jun12-->
            DataTable tblMonthDays = new DataTable();
            SqlStr = "Select top 1 MonthDays From Emp_Monthly_Muster Where Pay_Year='" + this.txtYear.Text.Trim() + "' and Pay_Month=" + this.fnNMonth(this.txtMonth.Text).ToString();
            tblMonthDays = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (tblMonthDays.Rows.Count == 0)
            {
                MessageBox.Show(this.txtMonth.Text + " Process Month Creation not Created", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                vMonthDays = Convert.ToDecimal(tblMonthDays.Rows[0]["MonthDays"]);
                //<--- Rup 04Jun12
                try // Rup 04Jun12
                {
                   // oDataAccess.BeginTransaction();
                    string vUpdateStr = string.Empty, vUpdBalStr = string.Empty;
                    if (this.pEditMode)
                    {
                        foreach (DataRow dr in dsGrd.Tables[0].Rows)
                        {
                            if (dr["Sel"].ToString() == "True")
                            {
                                foreach (DataRow dtr in dsLvBal.Tables[0].Rows)
                                {
                                    if (this.dgvMain.Columns[this.dgvMain.CurrentCell.ColumnIndex].Name.ToUpper() == "COL_" + dtr["Lv_Codebal"].ToString().Trim() && Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim()].Value.ToString()) > Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim() + "_Balance"].Value.ToString()))
                                    {                                       
                                        //MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance");
                                        MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);       // Changed by Sachin N. S. on 09/07/2014 for Bug-21114
                                        SendKeys.Send("+{TAB}");
                                        vIsFrmValid = false;
                                       
                                    }
                                    if (!vIsFrmValid)
                                    {
                                        vIsFrmValid = true;
                                        return;
                                    }
                                }
                                oDataAccess.BeginTransaction();
                                this.mSaveCommandString(ref vUpdateStr, dr);
                                oDataAccess.ExecuteSQLStatement(vUpdateStr, null, vTimeOut, true);
                                oDataAccess.CommitTransaction();
                            }

                        }
                        // Rup 04Jun12
                        oDataAccess.BeginTransaction();
                        vUpdateStr = "Execute usp_Ent_Emp_UpdateMonthly_Muster '" + this.txtYear.Text + "'";
                        vUpdateStr = vUpdateStr + "," + this.fnNMonth(this.txtMonth.Text).ToString();
                        vUpdateStr = vUpdateStr + ",''";//Location
                        vUpdateStr = vUpdateStr + "," + vMonthDays.ToString();

                        oDataAccess.ExecuteSQLStatement(vUpdateStr, null, vTimeOut, true);
                        oDataAccess.CommitTransaction();
                        //
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    oDataAccess.RollbackTransaction();
                }


                vYear = this.txtYear.Text;
                vLocNm = this.txtLocNm.Text;
                //vDept = this.txtDept.Text;
                //vCate = this.txtCategory.Text;

                this.txtLocNm.Text = "";
                this.txtEmpNm.Text = "";

                this.pAddMode = false;
                this.pEditMode = false;
                this.mthGrdRefresh();
                this.mthChkNavigationButton();
                this.mthEnableDisableFormControls();
                timer1.Enabled = true;
                timer1.Interval = 1000;
                MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }

        }
        private void mSaveCommandString(ref string vUpdateStr, DataRow dr)
        {
            string vEmpCode = string.Empty;
            string  vColValue = this.txtYear.Text + "," + this.fnNMonth(this.txtMonth.Text).ToString();
            string vFld_Nm = string.Empty;
            //dsGrd.Tables[0]
            //string vControl = string.Empty;
            vUpdateStr = "";
            //foreach (DataRow dr in dsGrd.Tables[0].Rows)
            //{
            foreach (DataRow tdr in this.dsLvMaster.Tables[0].Rows)
            {
                vFld_Nm = tdr["Lv_Code"].ToString();
                vUpdateStr = vUpdateStr + "," + vFld_Nm;
                vUpdateStr = vUpdateStr + "=" + dr[vFld_Nm].ToString();
                //if (vFld_Nm.Trim().ToLower() == "employeecode") { vEmpCode = dr[vFld_Nm].ToString(); }
            }
            vUpdateStr = vUpdateStr.Trim();
            vUpdateStr = vUpdateStr.Substring(1, vUpdateStr.Length - 1);



            vUpdateStr = "Update L set " + vUpdateStr + " From Emp_Monthly_Muster L ";
            vUpdateStr = vUpdateStr + " Left Join EmployeeMast E on (E.EmployeeCode=L.EmployeeCode) ";
            vUpdateStr = vUpdateStr + " Left Join Loc_Master Lc on (Lc.Loc_Code=E.Loc_Code) ";
            vUpdateStr = vUpdateStr + " Where Pay_Year='" + this.txtYear.Text + "' and Pay_Month=" + this.fnNMonth(txtMonth.Text).ToString();
            vUpdateStr = vUpdateStr + " and E.EmployeeCode='" + dr["EmployeeCode"].ToString().Trim() + "'";
            //}
            //vInsertStr="insert into Emp_Leave_Maintenance ("+vColList+)

            //vColList = vColList.Substring(1, vColList.Length - 1);
            //vColValue = vColValue.Substring(1, vColValue.Length - 1);

            ////vSaveString = vSaveString
            //string vfldList = string.Empty;
            //string vfldValList = string.Empty;
            //string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;
            //string vLoc_Code = string.Empty;
            //string vIsLeave = string.Empty;
            ////if (string.IsNullOrEmpty(this.txtCFW.Text.Trim())) { this.txtCFW.Text = "0"; }
            ////if (string.IsNullOrEmpty(this.txtEncash.Text.Trim())) { this.txtEncash.Text = "0"; }
            //if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
            //{
            //    SqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
            //    DataSet tds = new DataSet();
            //    tds = oDataAccess.GetDataSet(SqlStr, null, 20);
            //    vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
            //}

            //if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }

            //if (this.pAddMode == true)
            //{
            //    vSaveString = " Set DateFormat dmy insert into " + vMainTblNm;
            //    vfldList = "(Pay_Year,Att_Code,Att_Nm,isLeave,Loc_Code,Dept,Cate,maxLvCFW,maxLvEncash,LvAutoCr)";
            //    vfldValList = "'" + this.txtYear.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",'" + this.txtEmpCode.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",'" + this.txtEmpNm.Text.Trim() + "'";
            //    vfldValList = vfldValList + "," + vIsLeave;
            //    vfldValList = vfldValList + ",'" + vLoc_Code + "'";
            //    vfldValList = vfldValList + ",'" + this.txtDept.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",'" + this.txtCategory.Text.Trim() + "'";
            //    //vfldValList = vfldValList + "," + this.txtCFW.Text.Trim();
            //    //vfldValList = vfldValList + "," + this.txtEncash.Text.Trim();
            //    //vfldValList = vfldValList + ",'" + this.txtAutoCr.Text.Trim() + "'";
            //    vSaveString = vSaveString + vfldList + " Values( " + vfldValList + ")";
            //}
            //if (this.pEditMode == true)
            //{
            //    vSaveString = " Set DateFormat dmy Update " + vMainTblNm + " Set ";
            //    string vWhereCondn = string.Empty;
            //    vfldValList = vfldValList + "[Pay_Year]=" + "'" + this.txtYear.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",Att_Code=" + "'" + this.txtEmpCode.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",Att_Nm=" + "'" + this.txtEmpNm.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",isLeave=" + vIsLeave;
            //    vfldValList = vfldValList + ",Loc_Code=" + "'" + vLoc_Code + "'";
            //    vfldValList = vfldValList + ",Dept=" + "'" + this.txtDept.Text.Trim() + "'";
            //    vfldValList = vfldValList + ",Cate=" + "'" + this.txtCategory.Text.Trim() + "'";
            //    //vfldValList = vfldValList + ",maxLvCFW=" + this.txtCFW.Text.Trim();
            //    //vfldValList = vfldValList + ",maxLvEncash=" + this.txtEncash.Text.Trim();
            //    //vfldValList = vfldValList + ",LvAutoCr=" + "'" + this.txtAutoCr.Text.Trim() + "'";
            //    vWhereCondn = " Where id=" + vId;
            //    vSaveString = vSaveString + vfldValList + vWhereCondn;
            //}


        }

        private void mthValidation(ref Boolean vIsFrmValid)
        {
            //dgvMain.EndEdit();
            string vControl = string.Empty;
            int cnt = 0;
            //foreach (Control ctrl in gbLvDet.Controls)
            //{
            //    vControl = ctrl.Name;
            //    if (vControl.IndexOf("uchk_") > -1)
            //    {

            //        uCheckBox.cCheckBox ctrl1 = (uCheckBox.cCheckBox)ctrl;
            //        if (ctrl1.Checked)
            //        {
            //            if (this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text.Trim() != "")
            //            {
            //                if (Convert.ToDecimal(this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text.Trim()) > 0)
            //                {
            //                    cnt = cnt + 1;
            //                }
            //            }
            //        }
            //    }
            //}
            for (int i = 0; i < dgvMain.Rows.Count; i++)  /*Ramya*/
            {
                if (dgvMain.Rows[i].Cells["colSel"].FormattedValue.ToString() == "True")
                {
                    cnt++;
                }
            }
            if (cnt == 0)
            {
                MessageBox.Show("Please Select and Enter the Leave to be Updated", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vIsFrmValid = false;          
            }
        }

        private void dgvMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (vCurCol == dgvMain.Columns[dgvMain.CurrentCell.ColumnIndex].Name.ToUpper())
            //{
            //    return;
            //}
            if (dgvMain.CurrentRow.Cells["ColCalcPeriod"].Value.ToString().Trim() != "HOURWISE")      // Added by Sachin N. S. on 09/07/2014 for Bug-21114
            {
                foreach (DataRow dtr in dsLvBal.Tables[0].Rows)
                {
                    if (this.dgvMain.Columns[this.dgvMain.CurrentCell.ColumnIndex].Name.ToUpper() == "COL_" + dtr["Lv_Codebal"].ToString().Trim() && Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim()].Value.ToString()) > Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim() + "_Balance"].Value.ToString()))
                    {
                        //MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance");
                        MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);   // Changed by Sachin N. S. on 09/07/2014 for Bug-21114
                        SendKeys.Send("+{TAB}");
                        vIsFrmValid = false;

                    }
                    if (vIsFrmValid = false)
                    {
                        vIsFrmValid = true;
                        return;
                    }
                }

                vCurCol = dgvMain.Columns[dgvMain.CurrentCell.ColumnIndex].Name.ToUpper();

                if (dgvMain.Columns[dgvMain.CurrentCell.ColumnIndex].Name.ToUpper() == "COL_MonthDays")
                {
                    return;
                }
                //int MonthDays = Convert.ToInt16(this.dsGrd.Tables[0].Rows[0]["MonthDays"]);
                int MonthDays = Convert.ToInt16(this.dsGrd.Tables[0].Rows[e.RowIndex]["MonthDays"]);    // Changed by Sachin N. S. on 09/07/2014 for Bug-21114
                DataGridViewRow cr = this.dgvMain.CurrentRow;
                if (cr != null)
                {
                    Decimal SalPaidDays = MonthDays;
                    Decimal PRDays = MonthDays;
                    string ColTag = "";
                    Decimal Lop = 0;
                    foreach (DataGridViewCell cl in cr.Cells)
                    {
                        if (dgvMain.Columns[cl.ColumnIndex].Tag != null)
                        {
                            ColTag = dgvMain.Columns[cl.ColumnIndex].Tag.ToString();
                            if (ColTag == "A") //Leave,WO,HD
                            {
                                PRDays = PRDays - Convert.ToDecimal(cl.Value);
                            }
                            else if (ColTag == "D") //AB,HA
                            {
                                SalPaidDays = SalPaidDays - Convert.ToDecimal(cl.Value);
                                PRDays = PRDays - Convert.ToDecimal(cl.Value);
                            }

                        }
                        if (vCurCol.ToUpper() == dgvMain.Columns[cl.ColumnIndex].Name.ToUpper() && vRowIndex != dgvMain.CurrentRow.Index)
                        {
                            if (vCurVal != Convert.ToDecimal(cl.Value))
                            {
                                vCurVal = Convert.ToDecimal(cl.Value);
                                vRowIndex = dgvMain.CurrentRow.Index;
                            }
                            else
                            {
                                return;
                            }

                        }
                    }

                    //SalPaidDays = MonthDays - Lop;
                    Lop = MonthDays - SalPaidDays;
                    cr.Cells["COL_PR"].Value = PRDays;
                    cr.Cells["COL_SalPaidDays"].Value = SalPaidDays;
                    cr.Cells["COL_LOP"].Value = Lop;
                }
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }

        private void btnForward_Click(object sender, EventArgs e)
        {

        }

        private void btnLast_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {

        }

        private void btnLocate_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked == true)
            {
                chkSelectAll.Text = "Select None";
                //strcol = "','[Select]=cast(1 as bit), It_Name as [Item Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
                //mthRefreshGrid(strcol);
                for (int i = 0; i < dgvMain.Rows.Count; i++)
                {
                    dgvMain.Rows[i].Cells[0].Value = true;
                    this.dgvMain.ReadOnly = false;
                    this.dgvMain.Columns["colYear"].ReadOnly = true;
                    this.dgvMain.Columns["colcMonth"].ReadOnly = true;
                    this.dgvMain.Columns["colEmp"].ReadOnly = true;
                    this.dgvMain.Columns["colLocation"].ReadOnly = true;
                    this.dgvMain.Columns["colEmpCode"].ReadOnly = true;
                    this.dgvMain.Columns["Col_MonthDays"].ReadOnly = true;
                    this.dgvMain.Columns["Col_SalPaidDays"].ReadOnly = true;
                    this.dgvMain.Columns["Col_PR"].ReadOnly = true;
                }


            }
            else if (chkSelectAll.Checked == false)
            {
                chkSelectAll.Text = "Select All";
                //strcol = "','[Select]=cast(0 as bit), It_Name as [Item Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
                //mthRefreshGrid(strcol);
                for (int i = 0; i < dgvMain.Rows.Count; i++)
                {
                    dgvMain.Rows[i].Cells[0].Value = false;
                    for (int j = 0; j < this.dgvMain.Columns.Count; j++)
                    {
                        if (j == 0)
                            this.dgvMain.Columns[j].ReadOnly = false;
                        else
                            this.dgvMain.Columns[j].ReadOnly = true;
                    }
                }

            }
            dgvMain.RefreshEdit();          // Added by Sachin N. S. on 12/04/2014 for Bug-21937
            dgvMain.EndEdit();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

        private void dgvMain_CellLeave(object sender, DataGridViewCellEventArgs e)
        {


            //foreach (DataRow dataRow in (InternalDataCollectionBase)this.dsLvBal.Tables[0].Rows)
            //{
            //    if (this.dgvMain.Columns[this.dgvMain.CurrentCell.ColumnIndex].Name.ToUpper() == "COL_" + dataRow["Lv_Codebal"].ToString().Trim() && Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dataRow["Lv_Codebal"].ToString().Trim()].Value.ToString()) > Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dataRow["Lv_Codebal"].ToString().Trim() + "_Balance"].Value.ToString()))
            //    {
            //        int num = (int)MessageBox.Show(dataRow["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dataRow["LV_Nm"].ToString().Trim() + " balance");
            //        SendKeys.Send("+{TAB}");
            //    }
            //}
            if (vIsCancel == true)
            {
                vIsCancel = false;
                return;
            }

            foreach ( DataRow dtr in dsLvBal.Tables[0].Rows)
            {
                if (this.dgvMain.CurrentCell.ColumnIndex != null)
                {
                    if (this.dgvMain.Columns[this.dgvMain.CurrentCell.ColumnIndex].Name.ToUpper() == "COL_" + dtr["Lv_Codebal"].ToString().Trim() && Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim()].Value.ToString()) > Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim() + "_Balance"].Value.ToString()))
                    {
                        //MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance");
                        MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + "  Should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);   // Changed by Sachin N. S. on 09/07/2014 for Bug-21114
                        SendKeys.Send("+{TAB}");
                        vIsFrmValid = false;

                    }
                    if (vIsFrmValid == false)
                    {
                        vIsFrmValid = true;
                        return;
                    }
                }
            }
        }

        private void frmUdMuster_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void dgvMain_Click(object sender, EventArgs e)
        {
            DataGridViewRow cur = new DataGridViewRow();
            cur = dgvMain.CurrentRow;
            if (cur != null)
            {

                int rInd = dgvMain.CurrentRow.Index;
                if (rInd != null)
                {
                    this.mthFldRefresh(rInd);
                }

            }
        }

        //****** Added by Sachin N. S. on 09/07/2014 for Bug-21114 -- Start
        private void dgvMain_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (pEditMode == true)
            {
                if (dgvMain.CurrentCell.OwningColumn.Tag != null)
                {
                    if (dgvMain.CurrentRow.Cells["colCalcPeriod"].Value.ToString().Trim() == "HOURWISE")
                        dgvMain.CurrentRow.ReadOnly = true;
                    else
                        dgvMain.CurrentRow.ReadOnly = false;
                }
            }
        }
        //****** Added by Sachin N. S. on 09/07/2014 for Bug-21114 -- End        
    }
}
