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
using System.Data.OleDb;
using udReportList;
using System.Globalization;
using System.Threading;
using udclsDGVNumericColumn;
using udSelectPop;
using System.Reflection;
using System.Web;


namespace udTdsProjection
{
    public partial class frmTdsProjection : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet dsGrd = new DataSet();
        string SqlStr = string.Empty;
        string vMainField = "", vMainTblNm = "Emp_TDS_Projection";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        string vYear = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;
        DataSet vDsCommon;
        short  vTimeOut = 20;
        DataTable dtMain;
        //int vId;
        #region Initialization of Form with parameter
        public frmTdsProjection(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;
            this.pPara = args;
            this.pFrmCaption = "TDS Projection";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            //Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            //this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];

        }

        private void frmTdsProjection_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

         
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
                this.btnYear.Image = Image.FromFile(fName);
            }

          

            
            this.mInsertProcessIdRecord();
            this.mthDsCommon();

            this.StbLblMsg.Text = "";

            this.txtYear.Text = ((DateTime)vDsCommon.Tables["company"].Rows[0]["Sta_Dt"]).Year.ToString().Trim();
            this.txtYear.Text =this.txtYear.Text+ "-";
            this.txtYear.Text = this.txtYear.Text+((DateTime)vDsCommon.Tables["company"].Rows[0]["end_Dt"]).Year.ToString().Trim();

            this.SetFormColor();

            this.SetMenuRights();
            
            this.DataBind();

            mthEnableDisableFormControls();

            this.pAddMode = false;
            this.pEditMode = false;

            this.mthChkNavigationButton();
            //dgrMain.Height = this.Height - (groupBox1.Height + 60);
        }
        #endregion

        #region Common Database Methods
        private void mthDsCommon()
        {
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
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udTdsProjection.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
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
            dsColor = oDataAccess.GetDataSet(strSQL, null, 20);
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
        #region Navigation Button
        private void mthChkNavigationButton()
        {
            btnFirst.Enabled = false;
            btnBack.Enabled = false;
            btnForward.Enabled = false;
            btnLast.Enabled = false;
            //btnEmail.Enabled = false;
            btnLocate.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            this.btnCalculator.Enabled = false;
            //btnPreview.Enabled = false;
            //btnPrint.Enabled = false;
            //btnExportPdf.Enabled = false;
            //btnLogout.Enabled = false;
            btnHelp.Enabled = false;
            btnExit.Enabled = false;
            
            //if (this.pEditMode)
            //{
            //    this.btnSave.Enabled = true;
            //    this.btnCancel.Enabled = true;
            //    this.btnEdit.Enabled = false;
            //}
            //else
            //{
            //    this.btnSave.Enabled = false;
            //    this.btnCancel.Enabled = false;
            //    if (this.pEditButton)
            //    {
            //        this.btnEdit.Enabled = true;
            //    }
            //    else
            //    {
            //        this.btnEdit.Enabled = false;
            //    }
            //}

            //if (this.pPrintButton && this.btnEdit.Enabled == false)
            //{
            //    this.btnPrint.Enabled = false;
            //    this.btnPreview.Enabled = false;
            //    this.btnEmail.Enabled = false;
            //}
            //else
            //{
            //    this.btnPrint.Enabled = false;
            //    this.btnPreview.Enabled = false;
            //    this.btnEmail.Enabled = false;
            //}

        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pEditButton)
            {
               this.btnTDSProjection.Enabled = true;
               //foreach (DataRow dr in dtMain.Rows)
               //{
               //     dr["ColPrint"] = false;
               //}
            }
            else
            {
                this.btnTDSProjection.Enabled = true;
                //foreach (DataRow dr in dtMain.Rows)
                //{
                //    dr["ColProcess"] = false;
                //}
                this.btnTDSProjection.Enabled = false;
            }
            if (this.pPrintButton)
            {
                this.btnPreview.Enabled = true ;
                this.btnPrint.Enabled = true;
                this.btnExportPdf.Enabled = true;
                this.btnEmail.Enabled = true;
            }
            else
            {
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnExportPdf.Enabled = false;
                this.btnEmail.Enabled = false;
            }

            //if (this.pAddMode || this.pEditMode)
            //{
            //    vEnabled = true;
            //    this.dgrMain.Columns["ColPrint"].ReadOnly = true;
            //    foreach (DataRow dr in dtMain.Rows)
            //    {
            //        dr["ColPrint"] = false;
            //    }
            //    this.dgrMain.Columns["ColProcess"].ReadOnly = false;
            //}
            //else
            //{
            //    this.dgrMain.Columns["ColPrint"].ReadOnly = false ;
            //    this.dgrMain.Columns["ColProcess"].ReadOnly = true;
            //    foreach (DataRow dr in dtMain.Rows)
            //    {
            //        dr["ColProcess"] = false;
            //    }
            //    //this.btnDesc.Enabled = true;
            //}
            //this.btnLocNm.Enabled = vEnabled;
            //this.btnDept.Enabled = vEnabled;
            //this.btnCate.Enabled = vEnabled;
            //this.btnYear.Enabled = vEnabled;
            //this.dtpsDate.Enabled = vEnabled;
            //this.dtpeDate.Enabled = vEnabled;
            //this.txtDesc.Enabled = vEnabled;
            //this.txtYear.Enabled = vEnabled;
            //this.txtLocNm.Enabled = vEnabled;
            //this.dgvMain.Enabled = vEnabled;

        }
        #endregion
        #region Action Button
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnYear_Click(object sender, EventArgs e)
        {
           

            //string VForText, vSearchCol, vDisplayColumnList, vReturnCol;
            //SqlStr = "select distinct Pay_Year from emp_Processing_month where isclosed!=1"; // Changed By Amrendra on 03-07-2012

            //DataSet tDs = oDataAccess.GetDataSet(SqlStr , null, vTimeOut);

            //DataView dvw = tDs.Tables[0].DefaultView;

            //VForText = "Select distinct  pYear";
            //vSearchCol = "Pay_Year";
            //vDisplayColumnList = "Pay_Year:Year";
            //vReturnCol = "Pay_Year";
            //udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            //oSelectPop.pdataview = dvw;
            //oSelectPop.pformtext = VForText;
            //oSelectPop.psearchcol = vSearchCol;

            //oSelectPop.pDisplayColumnList = vDisplayColumnList;
            //oSelectPop.pRetcolList = vReturnCol;
            //oSelectPop.ShowDialog();
            //if (oSelectPop.pReturnArray != null)
            //{
            //    this.txtYear.Text = oSelectPop.pReturnArray[0];
            //}
        }
        private static string GetMonthName(int month, bool abbrev)  
        {
           
            DateTime date = new DateTime(1900, month, 1);
            if (abbrev)
            {
                return date.ToString("MMM");
            }
            else
            {
                return date.ToString("MMMM");
            }
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
        private void btnMonth_Click(object sender, EventArgs e)
        {
            string VForText, vSearchCol, vDisplayColumnList, vReturnCol;
            DataTable dt = new DataTable();
            DataColumn dtColMonth = new DataColumn();
            dtColMonth.ColumnName = "dtColMonth";
            dtColMonth.DataType = typeof(string);
            dt.Columns.Add(dtColMonth);
            for (int month = 1; month < 13; month++)
            {
                DataRow tDr = dt.NewRow();
                tDr[0] = GetMonthName(month, false);
                dt.Rows.Add(tDr);
            }
            DataView dvw = dt.DefaultView;

            VForText = "Select Month ";
            vSearchCol = "dtColMonth";
            vDisplayColumnList = "dtColMonth:Month";
            vReturnCol = "dtColMonth";
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
            }
        }

        #endregion
        #region Data Bind
        private void DataBind()
        {
            SqlStr = "Select ColPrint=Cast(0 as Bit),ColProcess=Cast(0 as Bit),EmployeeCode,EmployeeName,Department,Category From EmployeeMast where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' and PackageSal<>0 order by EmployeeCode,EmployeeName";
            dtMain = new DataTable();
            dtMain = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            this.dgrMain.DataSource = dtMain;

            this.dgrMain.Columns.Clear();
            DataGridViewCheckBoxColumn ColPrint = new DataGridViewCheckBoxColumn();
            ColPrint.HeaderText = "Print";
            ColPrint.Name = "ColPrint";
            ColPrint.DataPropertyName = "ColPrint";
            ColPrint.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //ColPrint.ReadOnly = true;
            ColPrint.Width = 45;
            dgrMain.Columns.Add(ColPrint);

            DataGridViewCheckBoxColumn ColProcess = new DataGridViewCheckBoxColumn();
            ColProcess.HeaderText = "ColProcess";
            ColProcess.Name = "ColProcess";
            ColProcess.DataPropertyName = "ColProcess";
            ColProcess.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //ColProcess.ReadOnly = true;
            ColProcess.Width = 70;
            dgrMain.Columns.Add(ColProcess);

            DataGridViewTextBoxColumn ColEmpCode = new DataGridViewTextBoxColumn();
            ColEmpCode.HeaderText = "Employee Code";
            ColEmpCode.Name = "ColEmpCode";
            ColEmpCode.DataPropertyName = "EmployeeCode";
            ColEmpCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColEmpCode.ReadOnly = true;
            dgrMain.Columns.Add(ColEmpCode);

            DataGridViewTextBoxColumn ColEmpName = new DataGridViewTextBoxColumn();
            ColEmpName.HeaderText = "Employee Name";
            ColEmpName.Name = "ColEmpName";
            ColEmpName.DataPropertyName = "EmployeeName";
            ColEmpName.Width = 340;
            ColEmpName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColEmpName.ReadOnly = true;
            dgrMain.Columns.Add(ColEmpName);

            DataGridViewTextBoxColumn ColDepartment = new DataGridViewTextBoxColumn();
            ColDepartment.HeaderText = "Department";
            ColDepartment.Name = "ColEmpName";
            ColDepartment.DataPropertyName = "Department";
            ColDepartment.Width = 150;
            ColDepartment.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColDepartment.ReadOnly = true;
            dgrMain.Columns.Add(ColDepartment);

            DataGridViewTextBoxColumn ColCategory = new DataGridViewTextBoxColumn();
            ColCategory.HeaderText = "Category";
            ColCategory.Name = "ColEmpName";
            ColCategory.DataPropertyName = "Category";
            ColCategory.Width = 150;
            ColCategory.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColCategory.ReadOnly = true;
            dgrMain.Columns.Add(ColCategory);

        }
        #endregion
        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, 20);
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
            dsRights = oDataAccess.GetDataSet(strSQL, null, 20);


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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.pEditMode = true;
            this.mthEnableDisableFormControls(); 
            this.mthChkNavigationButton();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
        }
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
        }

        private void btnTDSProjection_Click(object sender, EventArgs e)
        {
            Boolean IsValid=true;
            DateTime vStaDt;
            vStaDt = ((DateTime)this.vDsCommon.Tables["company"].Rows[0]["Sta_Dt"]);
            vStaDt =Convert.ToDateTime("01/"+this.fnNMonth(this.txtMonth.Text).ToString().Trim()+"/" + vStaDt.Year.ToString().Trim());
            if (this.txtYear.Text.Trim() == "")
            {
                IsValid = false;
                MessageBox.Show("Please Select Financial Year", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnYear.Focus();
            }
            if (this.txtMonth.Text.Trim() == "" && IsValid)
            {
                IsValid = false;
                MessageBox.Show("Please Select From Month", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnMonth.Focus();
            }
            if (IsValid)
            {
                this.StbLblMsg.Text = "Running TDS Projection Process....";
                this.stbMain.Refresh();
                foreach (DataRow drt in dtMain.Rows)
                {
                    if ((Boolean)drt["ColProcess"])
                    {
                        SqlStr = "Set DateFormat dmy Execute Usp_Ent_Emp_TDS_Projection '" + drt["EmployeeCode"].ToString().Trim() + "','" + this.txtYear.Text.Trim() + "','"+vStaDt+"','" + this.vDsCommon.Tables["company"].Rows[0]["End_Dt"].ToString() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, false);
                    }

                }
                this.StbLblMsg.Text = "TDS Projection Process is Completed";
                this.stbMain.Refresh();
                this.StbLblMsg.Text = "";
            }
           
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {

        }

        private void frmTdsProjection_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();/*Ramya 27/10/12*/
        }

        



    }
}
