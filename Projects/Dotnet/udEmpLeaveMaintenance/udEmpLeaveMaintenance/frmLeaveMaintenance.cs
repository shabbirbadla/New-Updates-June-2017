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

namespace udEmpLeaveMaintenance
{
    public partial class frmLeaveMaintenance : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsLvMaster = new DataSet();
        string vMainTblNm = "";
        string vYear = string.Empty,vMonth=string.Empty , vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;

        DataSet dsGrd = new DataSet();
        DataSet dsLvBal = new DataSet(); /*Ramya 07/08/12*/
        string SqlStr = string.Empty;
        String cAppPId, cAppName;
        string vAction;
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

        public frmLeaveMaintenance(string[] args)
        {
            this.pDisableCloseBtn = true;  /*close disable*/
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
            this.vAction = args[12].ToLower();
            //MessageBox.Show(vAction);
            if (vAction == "opening")
            {
                this.pFrmCaption = "Leave Opening Balance";
            }
            else
            {
                if (vAction == "credit")
                {
                    this.pFrmCaption = "Leave Credit";
                    
                }
                else
                {
                    this.pFrmCaption = "Leave EnCashment";
                }                
            }
        }

        private void frmLeaveMaintenance_Load(object sender, EventArgs e)
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

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            SqlStr = "select Distinct Att_Code as LV_Code,Att_Nm as LV_Nm from Emp_Attendance_Setting where iSLeave=1 and ldeactive=0";
            dsLvMaster = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            SqlStr = "select Distinct att_Code as Lv_Codebal,SortOrd,Att_Nm as LV_Nm  From Emp_Attendance_Setting where isleave=1 and LDeactive=0 order by SortOrd";
            dsLvBal = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            int vint = 0;
            string pyear = "";
            //SqlStr = "select isnull(max(pay_year),0) as LvYear from Emp_Payroll_Year_Master";
            SqlStr = "select isnull(max(pay_year),'') as LvYear from Emp_Payroll_Year_Master";/*Ramya 21/11/12*/
            DataSet dsTemp=new DataSet();
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (dsTemp.Tables[0].Rows.Count > 0)
            { 
               
              //  vint = Convert.ToInt16(dsTemp.Tables[0].Rows[0]["LvYear"].ToString());
                pyear = dsTemp.Tables[0].Rows[0]["LvYear"].ToString();
                //if (vint > 0)
                //{
                    this.txtYear.Text = pyear; 
                //}
                this.txtYear.Text = pyear;
            }
            SqlStr = "select isnull(max(pay_month),0) as LvMonth from Emp_Leave_Maintenance where Pay_Year='" + pyear + "'";   /*Ramya*/
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
                    this.txtMonth.Text ="";
                    //this.txtMonth.Text = this.fnCMonth(DateTime.Now.Month);
                }
            }
            this.SetMenuRights();
            this.pAddButton = false;


            this.mInsertProcessIdRecord();
            this.SetFormColor();

            this.mthAddLeave();

            string appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }

            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnYear.Image = Image.FromFile(fName);
                this.btnMonth.Image = Image.FromFile(fName);
                this.btnEmpNm.Image = Image.FromFile(fName);
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnDept.Image = Image.FromFile(fName);
                this.btnCate.Image = Image.FromFile(fName);
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
            this.label3.Focus();

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
            mthEnableDisableFormControls();
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            //this.mthBindData();

        }

        private string fnCMonth(int mn)
        {
            string cmnth=string.Empty ;
            switch (mn)
            {
                case 1:
                        cmnth= "January";
                        break;
                case 2:
                        cmnth= "February";
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
            int  nmnth = 0;
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
                    nmnth =6 ;
                    break;
                case  "July":
                    nmnth =7;
                    break;
                case "August":
                    nmnth = 8;
                    break;
                case "September":
                    nmnth =9 ;
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

        private void mthBindData()
        {      
            
            this.dgvMain.DataSource = dsGrd.Tables[0];
            this.dgvMain.Columns.Clear();

            //DataGridViewColumn[100] ; 
            System.Windows.Forms.DataGridViewCheckBoxColumn ColSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //ColSel.HeaderText = "Select";
            ColSel.HeaderText = "Update";
            ColSel.Name = "colSel";
            this.dgvMain.Columns.Add(ColSel);  /*Ramya*/

            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.Name = "colId";
            this.dgvMain.Columns.Add(colId); 

            System.Windows.Forms.DataGridViewTextBoxColumn colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colYear.HeaderText = "Year";
            colYear.Name = "colYear";
            this.dgvMain.Columns.Add(colYear); 

            //System.Windows.Forms.DataGridViewTextBoxColumn colMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colMonth.HeaderText = "Month";
            //colMonth.Name = "colMonth";
            //this.dgvMain.Columns.Add(colMonth);

            System.Windows.Forms.DataGridViewTextBoxColumn colcMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colcMonth.HeaderText = "Month";
            colcMonth.Name = "colcMonth";
            this.dgvMain.Columns.Add(colcMonth);

            System.Windows.Forms.DataGridViewTextBoxColumn colEmp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmp.HeaderText = "Employee Name";
            colEmp.Name = "colEmp";
            colEmp.Width = 250;   /*Ramya*/
            this.dgvMain.Columns.Add(colEmp); 
            
            int Cnt = 0;
            string VHTxt = string.Empty;
            foreach (DataRow dr in dsLvMaster.Tables[0].Rows)
            {
                //System.Windows.Forms.DataGridViewTextBoxColumn col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                //VHTxt = dr["LV_Nm"].ToString();
                //col1.HeaderText = VHTxt;
                //VHTxt = "Col_"+dr["LV_Nm"].ToString();
                //col1.Name = "VHTxt";
                //col1.DataPropertyName = dr["LV_Code"].ToString();
                //this.dgvMain.Columns.Add(col1);

                udclsDGVNumericColumn.CNumEditDataGridViewColumn col1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                VHTxt = dr["LV_Nm"].ToString();
                col1.HeaderText = VHTxt;
                VHTxt = "Col_" + dr["LV_Code"].ToString();
                col1.Name = VHTxt;
                col1.DataPropertyName = dr["LV_Code"].ToString();
                this.dgvMain.Columns.Add(col1);
                //col1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                //col1.Name = "colSalPaidDays";
                col1.DecimalLength = 2; //3
                col1.MaxInputLength = 6;//8
                col1.AllowNegative = false;
                
                //col1.
                //col1.Width = 100;
                //this.dgvPayDet.Columns.Add(colSalPaidDays);
                //colSalPaidDays.Frozen = true;
            }

            foreach (DataRow dr in dsLvBal.Tables[0].Rows)
            {
                // System.Windows.Forms.DataGridViewTextBoxColumn col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                udclsDGVNumericColumn.CNumEditDataGridViewColumn col1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                col1.DecimalLength = 3;
                col1.MaxInputLength = 8;
                col1.AllowNegative = false;
                VHTxt = dr["LV_Nm"].ToString();
                col1.HeaderText = dr["LV_Nm"].ToString().Trim() + " Balance";
                col1.DataPropertyName = dr["Lv_Codebal"].ToString().Trim() + "_Balance";
                col1.Name = "Col_" + dr["Lv_Codebal"].ToString().Trim() + "_Balance";
                col1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                this.dgvMain.Columns.Add(col1);
                if (this.pFrmCaption == "Leave EnCashment")    /*Ramya 12/09/12*/
                {
                    col1.Visible = true;
                }
                else
                {
                    col1.Visible = false;
                }                                              /*Ramya 12/09/12*/
            }

            System.Windows.Forms.DataGridViewTextBoxColumn colEmpCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmpCode.HeaderText = "Employee Code";
            colEmpCode.Name = "colEmpCode";
            this.dgvMain.Columns.Add(colEmpCode); 

            System.Windows.Forms.DataGridViewTextBoxColumn colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            this.dgvMain.Columns.Add(colLocation); 

            System.Windows.Forms.DataGridViewTextBoxColumn colDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDept.HeaderText = "Department";
            colDept.Name = "colDept";
            this.dgvMain.Columns.Add(colDept); 

            System.Windows.Forms.DataGridViewTextBoxColumn colCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCate.HeaderText = "Category";
            colCate.Name = "colCate";
            this.dgvMain.Columns.Add(colCate); 
            

            //System.Windows.Forms.DataGridViewTextBoxColumn colEmpCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colEmpCode.HeaderText = "Employee Code";
            //colEmpCode.Name = "colEmpCode";


            //System.Windows.Forms.DataGridViewTextBoxColumn colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colCode.HeaderText = "Code";
            //colCode.Name = "colCode";

            //System.Windows.Forms.DataGridViewTextBoxColumn colAttNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colAttNm.HeaderText = "Name";
            //colAttNm.Name = "colAttNm";

            //System.Windows.Forms.DataGridViewCheckBoxColumn colIsLeave = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //colIsLeave.HeaderText = "Is Leave";
            //colIsLeave.Name = "colIsLeave";

            //System.Windows.Forms.DataGridViewTextBoxColumn colLvCFW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colLvCFW.HeaderText = "Max Carried Forward";
            //colLvCFW.Name = "colLvCFW";

            //System.Windows.Forms.DataGridViewTextBoxColumn colLvEncash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colLvEncash.HeaderText = "En Cashable";
            //colLvEncash.Name = "colLvEncash";

            //System.Windows.Forms.DataGridViewTextBoxColumn colAutoCr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colAutoCr.HeaderText = "Ato Credit";
            //colAutoCr.Name = "colAutoCr";

            //System.Windows.Forms.DataGridViewTextBoxColumn colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colLocation.HeaderText = "Location";
            //colLocation.Name = "colLocation";

            //System.Windows.Forms.DataGridViewTextBoxColumn colDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colDept.HeaderText = "Department";
            //colDept.Name = "colDept";

            //System.Windows.Forms.DataGridViewTextBoxColumn colCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colCate.HeaderText = "Category";
            //colCate.Name = "colCate";




            ////maxLvCFW,maxLvEncash,LvAutoCr

            //this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colId, colYear, colCode, colAttNm, colIsLeave, colLvCFW, colLvEncash, colAutoCr, colLocation, colDept, colCate });
            this.dgvMain.Columns["colId"].Visible = false;
            dgvMain.Columns["colSel"].DataPropertyName = "sel";
            dgvMain.Columns["colId"].DataPropertyName = "Id";
            dgvMain.Columns["colYear"].DataPropertyName = "pay_year";
            dgvMain.Columns["colcMonth"].DataPropertyName = "cMonth";
            dgvMain.Columns["colEmpCode"].DataPropertyName = "EmployeeCode";
            dgvMain.Columns["colEmp"].DataPropertyName = "EmployeeName";

            //dgvMain.Columns["colCode"].DataPropertyName = "Lv_Code";
            //dgvMain.Columns["colAttNm"].DataPropertyName = "Att_Nm";
            //dgvMain.Columns["colIsLeave"].DataPropertyName = "IsLeave";

            //dgvMain.Columns["colLvCFW"].DataPropertyName = "maxLvCFW";
            //dgvMain.Columns["colLvEncash"].DataPropertyName = "maxLvEncash";
            //dgvMain.Columns["colAutoCr"].DataPropertyName = "LvAutoCr";

            dgvMain.Columns["colLocation"].DataPropertyName = "Loc_Desc";
            dgvMain.Columns["colDept"].DataPropertyName = "Department";
            dgvMain.Columns["colCate"].DataPropertyName = "Category";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);
            this.mthChkNavigationButton();

        }

        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
             this.btnLogout.Enabled = true;  /*Ramya*///Changed by Archana K. on 180513 for Bug-7899 
            //this.btnLogout.Enabled = false;   /*Ramya*///Commented by Archana K. on 180513 for Bug-7899 
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
                         //this.btnDelete.Enabled = true;
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

        private void mthNew(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
        }

        private void mthBindClear()
        {
            string vControl = "";
            this.txtYear.Text = "";
            this.txtMonth.Text = "";
            this.txtEmpCode.Text = "";
            this.txtEmpNm.Text = "";
            //this.chkLeave.Checked = false;
            this.txtLocNm.Text = "";
            this.txtDept.Text = "";
            this.txtCategory.Text = "";
            //foreach (Control ctrl in this.gbLvDet.Controls)
            //{
            //    if (ctrl.Name.IndexOf("ltxt_") > -1)
            //    {
            //        vControl = ctrl.Name;
            //        this.gbLvDet.Controls[vControl].Text= "";
            //    }
            //}

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
                this.btnCate.Enabled = false;
                this.btnDept.Enabled = false;
                this.dgvMain.ReadOnly = false;
                this.dgvMain.Columns["colId"].ReadOnly = true;
                this.dgvMain.Columns["colYear"].ReadOnly = true;
                this.dgvMain.Columns["colcMonth"].ReadOnly = true;
                this.dgvMain.Columns["colEmp"].ReadOnly = true;
                this.dgvMain.Columns["colEmpCode"].ReadOnly = true;
                this.dgvMain.Columns["colLocation"].ReadOnly = true;
                this.dgvMain.Columns["colDept"].ReadOnly = true;
                this.dgvMain.Columns["colCate"].ReadOnly = true;
                foreach (DataRow dr in dsLvBal.Tables[0].Rows)
                {
                    this.dgvMain.Columns["Col_" + dr["Lv_Codebal"].ToString().Trim() + "_Balance"].ReadOnly = true;
                }
            }
            else
            {
                this.btnEmpNm.Enabled = true;
                this.btnYear.Enabled = true;
                this.btnMonth.Enabled = true;
                this.btnLocNm.Enabled = true;
                this.btnCate.Enabled = true;
                this.btnDept.Enabled = true;
                this.dgvMain.ReadOnly = true;   /*Ramya*/

            }

            this.txtYear.Enabled = vEnabled;
            this.txtEmpNm.Enabled = vEnabled;
            this.txtEmpCode.Enabled = vEnabled;
            this.txtCategory.Enabled = vEnabled;
            this.txtLocNm.Enabled = vEnabled;
            this.txtDept.Enabled = vEnabled;
            string vControl = string.Empty;
            //foreach (Control ctrl in this.gbLvDet.Controls)
            //{
            //    if (ctrl.Name.IndexOf("uchk_") > -1)
            //    {
            //        vControl = ctrl.Name;
            //        this.gbLvDet.Controls[vControl].Enabled = vEnabled;
            //        if (vEnabled == false)
            //        {
            //            vControl = vControl.Replace("uchk_", "ltxt_");
            //            this.gbLvDet.Controls[vControl].Enabled = vEnabled;
            //        }
            //    }
            //}
         
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

        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpLeaveMaintenance.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "set dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
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
            vSearchCol = "LocNm"; //,Loc_Code
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
                //this.mthGrdRefresh();
                this.mthView();
            }
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Dept from Department order by Dept";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Department Name";
            vSearchCol = "Dept";
            vDisplayColumnList = "Dept:Department";
            vReturnCol = "Dept";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtDept.Text = oSelectPop.pReturnArray[0];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh();
                this.mthView();
            }
        }

        private void btnCate_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Cate from Category order by Cate";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Category Name";
            vSearchCol = "Cate";
            vDisplayColumnList = "Cate:Category";
            vReturnCol = "Cate";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCategory.Text = oSelectPop.pReturnArray[0];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh();
                this.mthView();
            }

        }

        private void btnYear_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select pay_year,sDate,eDate from Emp_Payroll_Year_Master order by pay_year";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Pay Year";
            vSearchCol = "pay_year";
            vDisplayColumnList = "pay_year:Pay Year,sDate:Start Date,eDate:End Date";
            vReturnCol = "pay_year,sDate,eDate";
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
                this.txtEmpNm.Text = "";
                this.txtEmpCode.Text = "";
                this.txtLocNm.Text = "";
                this.txtDept.Text = "";
                this.txtCategory.Text = "";

                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthGrdRefresh();
                    mthView();
                }
            }

        }

        private void mthGrdRefresh()
        {
            
            SqlStr="Execute Usp_Ent_Emp_Leave_Maintenance ";//'','','','','','',''

            if (this.txtYear.Text.Trim() == "")
            {
                SqlStr = SqlStr + "'0";
            }
            else
            {
                SqlStr = SqlStr + "'" + this.txtYear.Text.Trim();
            }
            

            SqlStr = SqlStr + "'," +this.fnNMonth(this.txtMonth.Text).ToString().Trim() ;
            SqlStr = SqlStr + ",'" + this.txtLocNm.Text.Trim() + "'";
            SqlStr = SqlStr + ",'" + this.txtDept.Text.Trim() + "'";
            SqlStr = SqlStr + ",'" + this.txtCategory.Text.Trim() + "'";
            SqlStr = SqlStr + ",'" + this.txtEmpNm.Text.Trim() + "'";
            SqlStr = SqlStr + ",'" + this.vAction + "'";
            //if (string.IsNullOrEmpty(this.txtEmpNm.Text.Trim()) == false)
            
                
            
                
           
            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthBindData();
        }

        private void mthAddLeave()
        {
            string VHTxt;
            int tTop = -4;
            tTop = tTop + 4 + 20;
            //foreach (DataRow dr in dsLvMaster.Tables[0].Rows)
            //{

            //    tTop = tTop + 4 + 20;
            //    //uCheckBox.cCheckBox uChk= new uCheckBox.cCheckBox();
            //    //uChk.Left = lblYear.Left;
            //    //uChk.AutoSize = true;
                
            //    //uChk.Text = "Update All Records" ;
            //    //uChk.Name = "chk" + dr["LV_Nm"].ToString();
            //    //uChk.Top = tTop;
            //    //uChk.Visible = true;
            //    //this.gbLvDet.Controls.Add(uChk);


            //    //System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            //    //lbl.Left =uChk.Left+uChk.Size.Width+10;
            //    //lbl.Top = tTop;
            //    //lbl.Visible = true;
            //    //VHTxt = dr["LV_Nm"].ToString();
            //    //lbl.Text = VHTxt;
            //    //this.gbLvDet.Controls.Add(lbl);
            //    //System.Windows.Forms.TextBox txtlv = new TextBox();
            //    //txtlv.Left = lblMonth.Left;
            //    //txtlv.Text = "";
            //    //txtlv.Name = "txt" + dr["LV_Nm"].ToString();
            //    //txtlv.Top = tTop;
            //    //txtlv.Visible = true;
            //    //this.gbLvDet.Controls.Add(txtlv);
            //    //this.btnYear.Click += new System.EventHandler(this.btnYear_Click);

            //    uCheckBox.cCheckBox uChk = new uCheckBox.cCheckBox();
            //    uChk.Left = this.lblDept.Left ;
            //    uChk.Enabled = false;
            //    uChk.Top = tTop;
            //    uChk.Visible = true;
            //    VHTxt = dr["LV_Nm"].ToString();
            //    uChk.Text = VHTxt;
            //    uChk.Name = "uchk_" + dr["LV_Code"].ToString();
            //    uChk.CheckStateChanged += new System.EventHandler(Chk_Leave_CheckStateChanged);
            //    this.gbLvDet.Controls.Add(uChk);

            //    System.Windows.Forms.TextBox txtlv = new TextBox();
            //    txtlv.Left = this.txtDept.Left+30;
            //    txtlv.Enabled  = false;
            //    txtlv.Width = txtYear.Width;
            //    txtlv.Text = "";
            //    txtlv.Name = "ltxt_" + dr["LV_Code"].ToString();
            //    txtlv.Top = tTop;
            //    txtlv.Visible = true;
            //    this.gbLvDet.Controls.Add(txtlv);

            //    uCheckBox.cCheckBox uChkall = new uCheckBox.cCheckBox();
            //    uChkall.Left = this.btnYear.Left+30 ;
            //    uChkall.Enabled = false;
            //    uChkall.AutoSize = true;

            //    //uChkall.Text = "Update All Records";
            //    //uChkall.Name = "uChkall_" + dr["LV_Code"].ToString();
            //    //uChkall.Top = tTop;
            //    //uChkall.Visible = true;
            //    //this.gbLvDet.Controls.Add(uChkall);

            //    this.gbLvDet.Refresh();
            //    //this.dgvMain.Columns.Add(col1);
            //}
            tTop = tTop+30;
            //this.gbLvDet.Height = tTop;
            //this.chkSelectAll.Top = this.gbLvDet.Top + 10;
            //int gbListHeight=gbList.Height;
            //this.gbList.Top = gbLvDet.Top + tTop + 8;
            //this.gbList.Height = gbListHeight - tTop + 25;
            this.dgvMain.Height = this.gbList.Height - 30;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            vIsCancel = true;

            this.mthBindClear();

            if (string.IsNullOrEmpty(vYear)==false )
            {
                this.txtYear.Text = vYear;
            }
            if (string.IsNullOrEmpty(vMonth) == false)
            {
                this.txtMonth.Text = vMonth ;
            }
            //if (string.IsNullOrEmpty(vLocNm) == false)
            //{
            //    this.txtLocNm.Text = vLocNm;
            //}
            //if (string.IsNullOrEmpty(vDept) == false)
            //{
            //    this.txtDept.Text = vDept;
            //}
            //if (string.IsNullOrEmpty(vCate) == false)
            //{
            //    this.txtCategory.Text = vCate;
            //}

            this.pAddMode = false;
            this.pEditMode = false;
            this.mthGrdRefresh();
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
            if (dgvMain.Rows.Count > 0)  /*Ramya 15/06/12*/
            {
                this.mthFldRefresh(0);
            }
            this.label3.Focus();    /*Ramya 15/06/12*/
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); /*Ramya*/
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

            this.pAddMode = false;
            this.pEditMode = true;
            if (pEditMode)
            {
                this.dgvMain.Columns[0].ReadOnly = false;
                this.dgvMain.Columns[4].ReadOnly = false;
                this.dgvMain.Columns[5].ReadOnly = false;
                this.dgvMain.Columns[6].ReadOnly = false;
                this.dgvMain.Columns[7].ReadOnly = false;
            }

            vYear = this.txtYear.Text;
            vMonth = this.txtMonth.Text;
            vLocNm = this.txtLocNm.Text;
            vDept = this.txtDept.Text;
            vCate = this.txtCategory.Text;
            string vControl = "";
            //foreach (Control ctrl in this.gbLvDet.Controls)
            //{
            //    if (ctrl.Name.IndexOf("ltxt_") > -1)
            //    {
            //        vControl = ctrl.Name;
            //        this.gbLvDet.Controls[vControl].Text = "";
            //    }
            //}
            //this.mthNew(sender, e);
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            dgvMain.EndEdit();  /*Ramya */

            vIsFrmValid = true;
            this.mthValidation(ref vIsFrmValid);
            if (vIsFrmValid == false)
            {
                return;
            }
           

            ////if (this.pAddMode)
            ////{
            ////    this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
            ////    oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
            ////}

            DataTable tblMonthDays = new DataTable();
            SqlStr = "Select top 1 MonthDays From Emp_Monthly_Muster Where pay_year='" + this.txtYear.Text.Trim() + "' and pay_month=" + this.fnNMonth(this.txtMonth.Text).ToString();
            tblMonthDays = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (tblMonthDays.Rows.Count == 0)
            {
                MessageBox.Show(this.txtMonth.Text + " Process Month Creation not Created", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            vMonthDays = Convert.ToDecimal(tblMonthDays.Rows[0]["MonthDays"]);

            try // Rup 04 Jun12
            {
               
                if (this.pEditMode)
                {
                    string vstr = string.Empty;
                    if (vAction == "opening")
                    {
                        //vUpdateStr =vUpdateStr+"_OpBal";
                        vstr = "_OpBal";
                    }
                    else
                    {
                        if (vAction == "credit")
                                {
                                    //vUpdateStr = vUpdateStr + "_Credit";
                                    vstr = "_Credit";
                                }
                                else
                                {
                                    //vUpdateStr = vUpdateStr + "_EnCash";
                                    vstr = "_EnCash";
                                }
                    }

                     for (int i = 0; i < dgvMain.Rows.Count; i++)           /*Ramya 31/07/12*/
                     {
                        //MessageBox.Show(dgvMain.Columns[5].Name.ToString());
                         string vUpdateStr = string.Empty, vUpdBalStr = string.Empty;
                        
                        if (dgvMain.Rows[i].Cells["colsel"].FormattedValue.ToString() == "True")
                        {
                            //this.mSaveCommandString(ref vUpdateStr, i);
                            if (this.pFrmCaption == "Leave EnCashment")  /*Ramya 11/09/12*/
                            {
                                foreach (DataRow dtr in dsLvBal.Tables[0].Rows)
                                {
                                    if ( Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim()].FormattedValue.ToString()) > Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim() + "_Balance"].Value.ToString()))
                                    {
                                        MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + " encashment should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance");
                                        SendKeys.Send("+{TAB}");
                                        vIsFrmValid = false;

                                    }
                                    if (!vIsFrmValid)
                                    {
                                        vIsFrmValid = true;
                                        return;
                                    }
                                }
                            }                                           /*Ramya 11/09/12*/

                            oDataAccess.BeginTransaction();
                            //vUpdateStr = "L.CL" + vstr + "= isnull(" + dgvMain.Rows[i].Cells["Col_Casual Leave"].FormattedValue.ToString() + ",0)";
                            //vUpdateStr = vUpdateStr + ", L.PL" + vstr + "= isnull(" + dgvMain.Rows[i].Cells["Col_Paid Leave"].FormattedValue.ToString() + ",0)";
                            //vUpdateStr = vUpdateStr + ", L.SL" + vstr + "= isnull(" + dgvMain.Rows[i].Cells["Col_Sick Leave"].FormattedValue.ToString() + ",0)";
                            //vUpdateStr = vUpdateStr + ", L.SP" + vstr + "= isnull(" + dgvMain.Rows[i].Cells["Col_Special Leave"].FormattedValue.ToString() + ",0)";

                            foreach (DataRow dr in dsLvMaster.Tables[0].Rows)  /*Ramya 04/09/12 */
                            {
                                if(vUpdateStr.Trim()=="")
                                {
                                    vUpdateStr = "L." + dr["Lv_Code"] + vstr + "= isnull(" + dgvMain.Rows[i].Cells["Col_" + dr["LV_Code"]].FormattedValue.ToString() + ",0)";
                                }
                                else
                                {
                                    vUpdateStr = vUpdateStr+",L." + dr["Lv_Code"] + vstr + "= isnull(" + dgvMain.Rows[i].Cells["Col_" + dr["LV_Code"]].FormattedValue.ToString() + ",0)";
                                }

                                if (vUpdBalStr.Trim() == "")
                                {
                                    vUpdBalStr = "L." + dr["Lv_Code"] + "_Balance=" + "isnull(" + dr["Lv_Code"] + "_OpBal,0)+isnull(" + dr["Lv_Code"] + "_Credit,0)-isnull(" + dr["Lv_Code"] + "_Availed,0)-isnull(" + dr["Lv_Code"] + "_Encash,0)";
                                }
                                else
                                {
                                    vUpdBalStr = vUpdBalStr + ",L." + dr["Lv_Code"] + "_Balance=" + "isnull(" + dr["Lv_Code"] + "_OpBal,0)+isnull(" + dr["Lv_Code"] + "_Credit,0)-isnull(" + dr["Lv_Code"] + "_Availed,0)-isnull(" + dr["Lv_Code"] + "_Encash,0)";
                                }
                            }                                                   /*Ramya 04/09/12 */



                            vUpdateStr = "Update L set " + vUpdateStr + " From Emp_Leave_Maintenance L ";
                            vUpdateStr = vUpdateStr + " Left Join EmployeeMast E on (E.EmployeeCode=L.EmployeeCode) ";
                            vUpdateStr = vUpdateStr + " Left Join Loc_Master Lc on (Lc.Loc_Code=E.Loc_Code) ";
                            vUpdateStr = vUpdateStr + " Where L.pay_year='" + this.txtYear.Text + "' and L.pay_month=" + this.fnNMonth(txtMonth.Text).ToString().Trim();

                           // vUpdateStr = vUpdateStr + "Where L.EmployeeCode='" + dgvMain.Rows[i].Cells["colEmpCode"].Value.ToString().Trim() + "'";
                            vUpdateStr = vUpdateStr + "and L.EmployeeCode='" + dgvMain.Rows[i].Cells["colEmpCode"].Value.ToString().Trim() + "'";
                            //if (txtLocNm.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and Lc.Loc_Desc='" + this.txtLocNm.Text.Trim() + "'"; }
                            //if (txtDept.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and E.Department='" + this.txtDept.Text.Trim() + "'"; }
                            //if (txtCategory.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and E.Category='" + this.txtCategory.Text.Trim() + "'"; }
                            //if (txtEmpCode.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and E.EmployeeCode='" + this.txtEmpCode.Text.Trim() + "'"; }



                            //vUpdBalStr = "L.CL" + "_Balance=" +"isnull(CL_OpBal,0)+isnull(CL_Credit,0)-isnull(CL_Availed,0)";
                            //vUpdBalStr = vUpdBalStr+",L.PL" + "_Balance=" +"isnull(PL_OpBal,0)+isnull(PL_Credit,0)-isnull(PL_Availed,0)";
                            //vUpdBalStr = vUpdBalStr+",L.SL" + "_Balance=" +"isnull(SL_OpBal,0)+isnull(SL_Credit,0)-isnull(SL_Availed,0)";
                            //vUpdBalStr = vUpdBalStr + ",L.SP" + "_Balance=" + "isnull(SP_OpBal,0)+isnull(SP_Credit,0)-isnull(SP_Availed,0)";

                            //foreach (DataRow dr in dsLvMaster.Tables[0].Rows)  /*Ramya 04/09/12 */
                            //{
                            //    //if (vUpdBalStr.Trim() == "")
                            //    //{
                            //    //    vUpdBalStr = "L." + dr["Lv_Code"] + "_Balance=" + "isnull(" + dr["Lv_Code"] + "_OpBal,0)+isnull(" + dr["Lv_Code"] + "_Credit,0)-isnull(" + dr["Lv_Code"] + "_Availed,0)-isnull(" + dr["Lv_Code"] + "_Encash,0)";
                            //    //}
                            //    //else
                            //    //{
                            //    //    vUpdBalStr = vUpdBalStr + ",L." + dr["Lv_Code"] + "_Balance=" + "isnull(" + dr["Lv_Code"] + "_OpBal,0)+isnull(" + dr["Lv_Code"] + "_Credit,0)-isnull(" + dr["Lv_Code"] + "_Availed,0)-isnull(" + dr["Lv_Code"] + "_Encash,0)";
                            //    //}
                            //}   


                            vUpdBalStr = "Update L set " + vUpdBalStr + " From Emp_Leave_Maintenance L ";
                            vUpdBalStr = vUpdBalStr + " Where L.pay_year='" + this.txtYear.Text + "' and L.pay_month=" +this.fnNMonth(txtMonth.Text).ToString().Trim();
                            vUpdBalStr = vUpdBalStr + "and L.EmployeeCode='" + dgvMain.Rows[i].Cells["colEmpCode"].Value.ToString().Trim() + "'";

                            //vUpdBalStr = vUpdBalStr + "Where L.EmployeeCode='" + dgvMain.Rows[i].Cells["colEmpCode"].Value.ToString().Trim() + "'";

                            oDataAccess.ExecuteSQLStatement(vUpdateStr, null, vTimeOut, true);
                            oDataAccess.ExecuteSQLStatement(vUpdBalStr, null, vTimeOut, true);

                            vUpdBalStr = "Execute usp_Ent_Emp_UpdateMonthly_Muster '" + this.txtYear.Text + "'";
                            vUpdBalStr = vUpdBalStr + "," + this.fnNMonth(this.txtMonth.Text).ToString();
                            vUpdBalStr = vUpdBalStr + ",''";//Location
                            vUpdBalStr = vUpdBalStr + "," + vMonthDays.ToString();

                            oDataAccess.ExecuteSQLStatement(vUpdateStr, null, vTimeOut, true);

                            oDataAccess.CommitTransaction();

                        }
                        //if (k > 0)
                        //{
                        //}
                        //else if (k == 0)
                        //{
                        //    MessageBox.Show("Please select any Employee to update");
                        //}
                     }
                     

          
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                oDataAccess.RollbackTransaction();
            }

            //vYear = this.txtYear.Text;
            //vLocNm = this.txtLocNm.Text;
            //vDept = this.txtDept.Text;
            //vCate = this.txtCategory.Text;


            this.txtEmpNm.Text = "";      /*Ramya 07/06/12*/
            this.txtLocNm.Text = "";
            this.txtDept.Text = "";
            this.txtCategory.Text = "";   /*Ramya 07/06/12*/

            this.pAddMode = false;
            this.pEditMode = false;
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();

            timer1.Enabled = true;
            timer1.Interval = 1000;
            MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);


        }

        //private void mSaveCommandString(ref string vUpdateStr, ref string vUpdBalStr)
        //{
        //    string vInsertString = string.Empty;
        //    string vColList = "pay_year,pay_month", vColValue = this.txtYear.Text+","+this.fnNMonth(this.txtMonth.Text).ToString() ;
          
        //    string vControl = string.Empty;
     
        //    foreach (Control ctrl in gbLvDet.Controls)   /*Ramya (has to change )*/
        //    {
        //        vControl = ctrl.Name;
        //        if (vControl.IndexOf("uchk_") > -1)    /*if(dgvmain.)*/
        //        {
        //            uCheckBox.cCheckBox ctrl1 = (uCheckBox.cCheckBox)ctrl;
        //            //ctrl1 = ctrl;
        //            if (ctrl1.Checked)
        //            {
        //                if (this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text.Trim() != "")
        //                {
        //                    vUpdateStr = vUpdateStr + ",L." + vControl.Replace("uchk_", "");
        //                    vUpdBalStr = vUpdBalStr + ",L." + vControl.Replace("uchk_", "") + "_Balance=" + "isnull(" + vControl.Replace("uchk_", "") + "_OpBal,0)+isnull(" + vControl.Replace("uchk_", "") + "_Credit,0)-isnull(" + vControl.Replace("uchk_", "") + "_Availed,0)";
        //                    vColList = vColList + "," + vControl.Replace("uchk_", "");
        //                    if (vAction == "opening")
        //                    {
        //                        vUpdateStr = vUpdateStr + "_OpBal";
        //                    }
        //                    else
        //                    {
        //                        if (vAction == "credit")
        //                        {
        //                            vUpdateStr = vUpdateStr + "_Credit";
        //                        }
        //                        else
        //                        {
        //                            vUpdateStr = vUpdateStr + "_EnCash";
        //                        }
        //                    }
        //                }
        //                //vUpdateStr = vUpdateStr + "=" + this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text;
        //                //vColValue = vColValue + "," + this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text;
        //                //this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text = "";
        //                //ctrl1.Checked = false;


        //                //vUpdateStr = vUpdateStr + "=" + this.dgvMain.Rows[i].
        //                //vColValue = vColValue + "," + this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text;
        //                this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text = "";
        //                ctrl1.Checked = false;
        //            }
        //            //vSaveString=","+vControl.Replace("ltxt_"
        //        }
        //    }
        //    vUpdateStr = vUpdateStr.Trim();
        //    vUpdateStr = vUpdateStr.Substring(1, vUpdateStr.Length - 1);

        //    vUpdateStr = "Update L set " + vUpdateStr + " From Emp_Leave_Maintenance L ";
        //    vUpdateStr = vUpdateStr + " Left Join EmployeeMast E on (E.EmployeeCode=L.EmployeeCode) ";
        //    vUpdateStr = vUpdateStr + " Left Join Loc_Master Lc on (Lc.Loc_Code=E.Loc_Code) ";
        //    vUpdateStr = vUpdateStr + " Where pay_year=" + this.txtYear.Text + " and isnull(datename(month,dateadd(month, pay_month - 1, 0)),'''')='" + txtMonth.Text + "'";
            
        //    if (txtLocNm.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and Lc.Loc_Desc='" + this.txtLocNm.Text.Trim() + "'"; }
        //    if (txtDept.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and E.Department='" + this.txtDept.Text.Trim() + "'"; }
        //    if (txtCategory.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and E.Category='" + this.txtCategory.Text.Trim() + "'"; }
        //    if (txtEmpCode.Text.Trim() != "") { vUpdateStr = vUpdateStr + " and E.EmployeeCode='" + this.txtEmpCode.Text.Trim() + "'"; }

        //    vUpdBalStr = vUpdBalStr.Substring(1, vUpdBalStr.Length - 1);
        //    vUpdBalStr = "Update L set " + vUpdBalStr + " From Emp_Leave_Maintenance L ";
        //    vUpdBalStr = vUpdBalStr + " Where L.pay_year=" + this.txtYear.Text + " and L.pay_month=" +this.fnNMonth(txtMonth.Text).ToString().Trim();
            
            
            

        //    //vInsertStr="insert into Emp_Leave_Maintenance ("+vColList+)

        //    //vColList = vColList.Substring(1, vColList.Length - 1);
        //    //vColValue = vColValue.Substring(1, vColValue.Length - 1);

        //    ////vSaveString = vSaveString
        //    //string vfldList = string.Empty;
        //    //string vfldValList = string.Empty;
        //    //string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;
        //    //string vLoc_Code = string.Empty;
        //    //string vIsLeave = string.Empty;
        //    ////if (string.IsNullOrEmpty(this.txtCFW.Text.Trim())) { this.txtCFW.Text = "0"; }
        //    ////if (string.IsNullOrEmpty(this.txtEncash.Text.Trim())) { this.txtEncash.Text = "0"; }
        //    //if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
        //    //{
        //    //    SqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
        //    //    DataSet tds = new DataSet();
        //    //    tds = oDataAccess.GetDataSet(SqlStr, null, 20);
        //    //    vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
        //    //}

        //    //if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }

        //    //if (this.pAddMode == true)
        //    //{
        //    //    vSaveString = " Set DateFormat dmy insert into " + vMainTblNm;
        //    //    vfldList = "(pay_year,Att_Code,Att_Nm,isLeave,Loc_Code,Dept,Cate,maxLvCFW,maxLvEncash,LvAutoCr)";
        //    //    vfldValList = "'" + this.txtYear.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",'" + this.txtEmpCode.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",'" + this.txtEmpNm.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + "," + vIsLeave;
        //    //    vfldValList = vfldValList + ",'" + vLoc_Code + "'";
        //    //    vfldValList = vfldValList + ",'" + this.txtDept.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",'" + this.txtCategory.Text.Trim() + "'";
        //    //    //vfldValList = vfldValList + "," + this.txtCFW.Text.Trim();
        //    //    //vfldValList = vfldValList + "," + this.txtEncash.Text.Trim();
        //    //    //vfldValList = vfldValList + ",'" + this.txtAutoCr.Text.Trim() + "'";
        //    //    vSaveString = vSaveString + vfldList + " Values( " + vfldValList + ")";
        //    //}
        //    //if (this.pEditMode == true)
        //    //{
        //    //    vSaveString = " Set DateFormat dmy Update " + vMainTblNm + " Set ";
        //    //    string vWhereCondn = string.Empty;
        //    //    vfldValList = vfldValList + "[pay_year]=" + "'" + this.txtYear.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",Att_Code=" + "'" + this.txtEmpCode.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",Att_Nm=" + "'" + this.txtEmpNm.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",isLeave=" + vIsLeave;
        //    //    vfldValList = vfldValList + ",Loc_Code=" + "'" + vLoc_Code + "'";
        //    //    vfldValList = vfldValList + ",Dept=" + "'" + this.txtDept.Text.Trim() + "'";
        //    //    vfldValList = vfldValList + ",Cate=" + "'" + this.txtCategory.Text.Trim() + "'";
        //    //    //vfldValList = vfldValList + ",maxLvCFW=" + this.txtCFW.Text.Trim();
        //    //    //vfldValList = vfldValList + ",maxLvEncash=" + this.txtEncash.Text.Trim();
        //    //    //vfldValList = vfldValList + ",LvAutoCr=" + "'" + this.txtAutoCr.Text.Trim() + "'";
        //    //    vWhereCondn = " Where id=" + vId;
        //    //    vSaveString = vSaveString + vfldValList + vWhereCondn;
        //    //}


        //}

        private void mSaveCommandString(ref string vUpdateStr, ref string vUpdBalStr)
        {

           // DataTable dtLeaveDetails = (DataTable)dgvMain.Select("Select='true'");

            DataTable dtGrid = (DataTable)dgvMain.DataSource;
            DataRow[] drGrid = (DataRow[])dtGrid.Select("Sel='true'");
        }

        private void mthValidation(ref Boolean  vIsFrmValid)
        {
            string vControl = string.Empty;
            int cnt = 0;
            for (int i = 0; i < dgvMain.Rows.Count; i++)
            {
                if (dgvMain.Rows[i].Cells["colsel"].FormattedValue.ToString() == "True")
                {
                    cnt++;
                }
            }
           
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
            //                //if(Convert.ToDecimal(this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text.Trim())>0)  /*Ramya >0 */
            //                if(this.gbLvDet.Controls[vControl.Replace("uchk_", "ltxt_")].Text.Trim() != "")
            //                {
            //                    cnt = cnt + 1;
            //                }
            //            }
            //        }
            //    }
            //}
            if (cnt == 0)
            {
                MessageBox.Show("Please Select any Employee and Enter Leave to be Updated", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vIsFrmValid = false;
            }
        }

        private void mthFldRefresh(int rInd)
        {
            if (dgvMain.Rows.Count == 0)
            {
                return;
            }
            if (dgvMain.Columns[0].Name == "id")
            {
                return;
            }
            if (dgvMain.Rows[rInd].Cells["colid"].Value == null)
            {
                return;
            }
            //{ colId, colYear, colCode, colAttNm, colIsLeave, colLvCFW, colLvEncash, colAutoCr, colLocation, colDept, colCate });

            vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();
            this.txtYear.Text = dgvMain.Rows[rInd].Cells["colYear"].Value.ToString();
            this.txtMonth.Text = dgvMain.Rows[rInd].Cells["colcMonth"].Value.ToString();
            //this.txtEmpCode.Text = dgvMain.Rows[rInd].Cells["colCode"].Value.ToString();
            //this.txtEmpNm.Text = dgvMain.Rows[rInd].Cells["colAttNm"].Value.ToString();

            //if ((Boolean)dgvMain.Rows[rInd].Cells["colIsLeave"].Value == true)
            //{
            //    this.chkLeave.Checked = true;
            //}
            //else
            //{
            //    this.chkLeave.Checked = false;
            //}

            //this.txtCFW.Text = dgvMain.Rows[rInd].Cells["colLvCFW"].Value.ToString();
            //this.txtEncash.Text = dgvMain.Rows[rInd].Cells["colLvEncash"].Value.ToString();

            //this.txtCode.Text = dgvMain.Rows[rInd].Cells["colCode"].Value.ToString();
            this.txtLocNm.Text = dgvMain.Rows[rInd].Cells["colLocation"].Value.ToString();
            this.txtDept.Text = dgvMain.Rows[rInd].Cells["colDept"].Value.ToString();
            this.txtCategory.Text = dgvMain.Rows[rInd].Cells["colCate"].Value.ToString();
            this.txtEmpCode.Text = dgvMain.Rows[rInd].Cells["colEmpCode"].Value.ToString();
            this.txtEmpNm.Text = dgvMain.Rows[rInd].Cells["colEmp"].Value.ToString();
            //if (dgvMain.Rows[rInd].Cells["colFH_Day"].Value.ToString() == "True")
            //{

            //}
            //else
            //{

            //}
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow cur = new DataGridViewRow();
            cur = dgvMain.CurrentRow;
            //MessageBox.Show(vId);
            if (string.IsNullOrEmpty(vId) == false)
            {


                if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //                vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();
                    //     int vId = dgvMain.CurrentRow.Index;
                    //if (vId != null)
                    //{
                    string vDelString = "Delete from Emp_Attendance_Setting Where ID=" + vId;
                    oDataAccess.ExecuteSQLStatement(vDelString, null, vTimeOut, true);
                    //}
                    //}
                }
                this.mthBindClear();
                this.mthView();
                if (dgvMain.Rows.Count > 0)
                {
                   this.mthFldRefresh(0); 
                }
            }
            else
            {
                MessageBox.Show("Please Select the Row to be Deleted..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(btnLogout.Enabled)
            btnLogout_Click(this.btnExit, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
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
            if (txtDept.Text.Trim() != "") { SqlStr = SqlStr + " and D.Dept='" + this.txtDept.Text.Trim() + "'"; }
            if (txtCategory.Text.Trim() != "") { SqlStr = SqlStr + " and c.Cate='" + this.txtCategory.Text.Trim()  + "'"; }
            SqlStr = SqlStr + " Union Select '' as EmpNm,'' as EmpCode,'' as Department,'' as Designation,'' as Category,'' as Grade";

            SqlStr = SqlStr + " order by EmployeeName";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Employee Name";
            vSearchCol = "EmpNm"; //,EmpCode
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
                //this.mthGrdRefresh();
                this.mthView();
            }
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
            dvw=dt.DefaultView ;
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
                    //this.mthGrdRefresh();
                    txtCategory.Text = "";
                    txtEmpCode.Text = "";
                    txtEmpNm.Text = "";
                    txtLocNm.Text = "";
                    txtDept.Text = "";
                    this.mthView();
                }
            }
        }

        private void Chk_Leave_CheckStateChanged(object sender, EventArgs e)
        {
            uCheckBox.cCheckBox uchk = new uCheckBox.cCheckBox();
            uchk = (uCheckBox.cCheckBox)sender;
            string vControl = uchk.Name.Replace("uchk_", "ltxt_");
           
            if (uchk.Checked)
            {
                this.Controls["gbLvDet"].Controls[vControl].Enabled = true ;
                //vControl = vControl.Replace("ltxt_", "uChkall_");
                //this.Controls["gbLvDet"].Controls[vControl].Enabled = true;
            }
            else
            {
                this.Controls["gbLvDet"].Controls[vControl].Enabled = false ;
                //vControl = vControl.Replace("ltxt_", "uChkall_");
                //this.Controls["gbLvDet"].Controls[vControl].Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
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
                }

            }
            dgvMain.EndEdit();
            dgvMain.RefreshEdit();//Added By Pankaj B. on 20-03-2015 for Bug-25365
        }

        private void frmLeaveMaintenance_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();  /*Ramya*/
        }

        private void dgvMain_CellLeave(object sender, DataGridViewCellEventArgs e)   /*Ramya 11/09/12*/
        {
            dgvMain.EndEdit();
            if (vIsCancel == true)
            {
                vIsCancel = false;
                return;
            }
            if (pEditMode)
            {
                if (this.pFrmCaption == "Leave EnCashment")
                {
                    foreach (DataRow dtr in dsLvBal.Tables[0].Rows)
                    {
                        if (this.dgvMain.Columns[this.dgvMain.CurrentCell.ColumnIndex].Name.ToUpper() == "COL_" + dtr["Lv_Codebal"].ToString().Trim() && Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim()].FormattedValue.ToString()) > Convert.ToDecimal(this.dgvMain.CurrentRow.Cells["COL_" + dtr["Lv_Codebal"].ToString().Trim() + "_Balance"].Value.ToString()))
                        {
                            MessageBox.Show(dtr["LV_Nm"].ToString().Trim() + " encashment should be less than or equal to " + dtr["LV_Nm"].ToString().Trim() + " balance");
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

        }

        private void btnLast_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }    
        
    }
}

