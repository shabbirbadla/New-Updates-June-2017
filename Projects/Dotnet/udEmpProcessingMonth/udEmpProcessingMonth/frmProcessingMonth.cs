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
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udProcessingMonth
{
    public partial class frmProcessingMonth : uBaseForm.FrmBaseForm
    {
        #region Variable Declaration
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet dsGrd = new DataSet();
        string SqlStr = string.Empty;
        string vMainField = "", vMainTblNm = "Emp_Processing_Month";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        Boolean cValid = true;
        string vYear = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;
        string vLoc_Code = string.Empty;
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end
        #endregion
        
        #region Args passing to the Project
        public frmProcessingMonth(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Processing Month Creation";
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

        #region Page Load event
        private void frmProcessingMonth_Load(object sender, EventArgs e)
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
            this.SetMenuRights();
            this.dgvMain.ReadOnly = true;


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
                this.btnYear.Image = Image.FromFile(fName);
                this.btnMonth.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";

            //Added by Archana K. on 16/05/13 for Bug-7899 start
            //startupPath = "E:\\Vudyog ENT";
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
        #endregion

        #region Functionality for view
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
                this.gbMonthDet.Enabled = false;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            mthEnableDisableFormControls();
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            //this.mthBindData();

        }

        #endregion

        #region Functionality to bind Data
        private void mthBindData()
        {

            this.dgvMain.DataSource = dsGrd.Tables[0];
            this.dgvMain.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.Name = "colId";

            System.Windows.Forms.DataGridViewTextBoxColumn colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colYear.HeaderText = "Year";
            colYear.Name = "colYear";

            System.Windows.Forms.DataGridViewTextBoxColumn colcMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colcMonth.HeaderText = "Month";
            colcMonth.Name = "colcMonth";

            System.Windows.Forms.DataGridViewTextBoxColumn colPay_Month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPay_Month.HeaderText = "nMonth";
            colPay_Month.Name = "colPay_Month";

            System.Windows.Forms.DataGridViewTextBoxColumn colLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLoc.HeaderText = "Location";
            colLoc.Name = "colLoc";

            System.Windows.Forms.DataGridViewCheckBoxColumn colisClosed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colisClosed.HeaderText = "Is Closed";
            colisClosed.Name = "colisClosed";
            //this.dgvMain.Columns.Add(colMonth); 




            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colId, colYear, colcMonth, colPay_Month, colLoc, colisClosed });
            this.dgvMain.Columns["colId"].Visible = false;
            this.dgvMain.Columns["colPay_Month"].Visible = false;

            dgvMain.Columns["colId"].DataPropertyName = "Id";
            //dgvMain.Columns["colSrNo"].DataPropertyName = "SrNo";
            dgvMain.Columns["colYear"].DataPropertyName = "Pay_Year";
            dgvMain.Columns["colcMonth"].DataPropertyName = "cMonth";
            dgvMain.Columns["ColPay_Month"].DataPropertyName = "Pay_Month";
            dgvMain.Columns["colLoc"].DataPropertyName = "Loc_Desc";
            dgvMain.Columns["colisClosed"].DataPropertyName = "isClosed";
        } 
        #endregion

        #region Button click event for New Record
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);

            btnLogout.Enabled = false;
            this.mthChkNavigationButton();

        } 
        #endregion

        #region Functionality to Check Navigation Button
        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
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
                    btnLogout.Enabled = true;

                }
                else
                {
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    btnLogout.Enabled = false;
                }
                if (this.dsGrd.Tables[0].Rows.Count == 0 && this.pAddMode == false && this.pEditMode == false)
                {
                    this.btnEdit.Enabled = false;
                    this.btnDelete.Enabled = false;
                }
            }//Added by Archana K. on 17/05/13 for Bug-7899
        } 
        #endregion

        #region FunctionalityFor new Record
        private void mthNew(object sender, EventArgs e)
        {
            this.mthBindClear();
            this.mthEnableDisableFormControls();
        } 
        #endregion

        #region Functionality to clear Textboxes
        private void mthBindClear()
        {
            this.txtYear.Text = ""; /*Ramya 29/10/2012*/
            this.txtLocNm.Text = "";
            this.txtMonth.Text = "";


        } 
        #endregion

        #region Functionality Enable and Disable Controls
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            this.btnYear.Enabled = false;
            this.btnMonth.Enabled = false;
            this.btnLocNm.Enabled = false;
            this.chkIsClosed.Enabled = vEnabled;
            this.txtYear.Enabled = false;
            this.txtMonth.Enabled = false;
            this.txtLocNm.Enabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
            }

            if (this.pAddMode)
            {
                this.btnYear.Enabled = true;
                this.btnMonth.Enabled = true;
                this.btnLocNm.Enabled = true;
            }
            
            this.chkIsClosed.Enabled = vEnabled;
            //this.txtYear.Enabled = vEnabled;
            //this.txtMonth.Enabled = vEnabled;
            //this.txtLocNm.Enabled = vEnabled;
            //this.dgvMain.Enabled = vEnabled;

        }
        #endregion

        #region Functionality to set Menu Rights
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
        #endregion

        #region Functionality to Insert Record
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpProcessingMonth.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true); /*Ramya 10/12/12*/
        } 
        #endregion

        #region Functionality to Delete a Record
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
        #endregion

        #region Functionality to check and call application
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
        #endregion

        #region Functionality to set Form color
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

        #region Functionality for Logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        } 
        #endregion

        #region Functionality to select location name through pop up
        private void btnLocNm_Click(object sender, EventArgs e)
        {
            if (txtMonth.Text == string.Empty)
            {
                MessageBox.Show("Please Select Month!", this.pPApplText);

            }
            else
            {
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataSet tDs = new DataSet();
                SqlStr = "select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master union select '' as LocNm,'' as Loc_Code,'' as Add1,'' as Add2,'' as Add3,'' as [Location Name] order by Loc_Desc";
                tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


                DataView dvw = tDs.Tables[0].DefaultView;

                VForText = "Select Location Name";
                vSearchCol = "LocNm";//,Loc_Code
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
        } 
        #endregion

        #region Functionality to Select Departmentthrough popup
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
                //this.txtDept.Text = oSelectPop.pReturnArray[0];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.dtpsDate.Text = oSelectPop.pReturnArray[1];
                //this.dtpeDate.Text = oSelectPop.pReturnArray[2];
                this.mthGrdRefresh();
            }
        } 
        #endregion

        #region Functionality to Select Year through popup
        private void btnYear_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Pay_Year,sDate,eDate from Emp_Payroll_Year_Master order by Pay_Year";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Pay Year";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Pay Year,sDate:Start Date,eDate:End Date";
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
                    if (this.pAddMode == false && this.pEditMode == false)
                    {
                        this.mthGrdRefresh();
                    }
                }
            }

        } 
        #endregion

        #region Functionality to get default value( in grid) after refresh
        private void mthGrdRefresh()
        {
            //SqlStr = "select H.*,L.Loc_Desc,D.Dept,C.Cate from Emp_Holiday_Master H Left Join Loc_Master L on (h.Loc_Code=L.Loc_Code) Left Join Department d on (h.Dept=d.Dept) Left Join Category C on (h.Cate=C.Cate) where 1=1";
            SqlStr = "select M.*,datename(month,dateadd(month, Pay_Month - 1, 0)) as cMonth,L.Loc_Desc from Emp_Processing_Month M Left Join Loc_Master L on (M.Loc_Code=L.Loc_Code) where 1=1";

            if (string.IsNullOrEmpty(this.txtLocNm.Text.Trim()) == false)
            {
                SqlStr = SqlStr + " and L.Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
            }

            SqlStr = SqlStr + " Order by m.Pay_Year,Pay_Month";
            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthBindData();
        } 
        #endregion

        #region Functionality to Cancel Event and show default grid
        private void btnCancel_Click(object sender, EventArgs e)
        {


            this.mthBindClear();
            if (string.IsNullOrEmpty(vYear))
            {
                this.txtYear.Text = vYear;
            }
            if (string.IsNullOrEmpty(vLocNm))
            {
                this.txtLocNm.Text = vLocNm;
            }
            //if (string.IsNullOrEmpty(vDept))
            //{
            //    this.txtDept.Text = vDept;
            //}
            //if (string.IsNullOrEmpty(vCate))
            //{
            //    this.txtCategory.Text = vCate;
            //}

            this.pAddMode = false;
            this.pEditMode = false;
            this.chkIsClosed.Enabled = false;
            this.mthGrdRefresh();
            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();//Archana K. on 25/06/13 for Bug-16410
        } 
        #endregion

        #region Functionality to Edit Records
        private void btnEdit_Click(object sender, EventArgs e)
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

            this.pAddMode = false;
            this.pEditMode = true;

            vYear = this.txtYear.Text;
            vLocNm = this.txtLocNm.Text;
            //vDept = this.txtDept.Text;
            //vCate = this.txtCategory.Text;

            //this.mthNew(sender, e);
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();

        } 
        #endregion

        #region Functionality to get month number from month name 
        private int getmonth(string monthName)
        {
            int monthNumber = 0;
            switch (monthName.ToString().ToUpper().Trim())
            {
                case "JANUARY":
                    {
                        monthNumber = 1;
                        break;
                    }
                case "FEBRUARY":
                    {
                        monthNumber = 2;
                        break;
                    }
                case "MARCH":
                    {
                        monthNumber = 3;
                        break;
                    }
                case "APRIL":
                    {
                        monthNumber = 4;
                        break;
                    }
                case "MAY":
                    {
                        monthNumber = 5;
                        break;
                    }
                case "JUNE":
                    {
                        monthNumber = 6;
                        break;
                    }
                case "JULY":
                    {
                        monthNumber = 7;
                        break;
                    }
                case "AUGUST":
                    {
                        monthNumber = 8;
                        break;
                    }
                case "SEPTEMBER":
                    {
                        monthNumber = 9;
                        break;
                    }
                case "OCTOBER":
                    {
                        monthNumber = 10;
                        break;
                    }
                case "NOVEMBER":
                    {
                        monthNumber = 11;
                        break;
                    }
                case "DECEMBER":
                    {
                        monthNumber = 12;
                        break;
                    }
                default:
                    break;
            }
            return monthNumber;


        } 
        #endregion

        #region Functionality to check data already exists or not
        private bool monthExist()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            //sqlstr = " select Pay_Month  from " + vMainTblNm + " where Pay_Month= " + getmonth(this.txtMonth.Text.Trim()) + " and Pay_Year = '" + this.txtYear.Text.Trim() + "'";  
            //if (this.txtLocNm.Text.Trim() == string.Empty)
            //{
            //    sqlstr = " select Pay_Month,Loc_Code  from " + vMainTblNm + " where Pay_Month= " + getmonth(this.txtMonth.Text.Trim()) + " and Pay_Year = '" + this.txtYear.Text.Trim() + "'";    /*Ramya 22/02/13*/

            //}
            //else
            //{
            //    DataSet dsloc;
            //    string sqlloc = "select Loc_Code from loc_master where Loc_desc='" + this.txtLocNm.Text.Trim() + "'";
            //    dsloc = oDataAccess.GetDataSet(sqlloc, null, vTimeOut);
            //    sqlstr = " select Pay_Month,Loc_Code  from " + vMainTblNm + " where Pay_Month= " + getmonth(this.txtMonth.Text.Trim()) + " and Pay_Year = '" + this.txtYear.Text.Trim() + "' and Loc_Code = '" + dsloc.Tables[0].Columns["Loc_Code"].ToString().Trim() + "'";    /*Ramya 22/02/13*/
            //}


            sqlstr = " select Pay_Month,Loc_Code  from " + vMainTblNm + " where Pay_Month= " + getmonth(this.txtMonth.Text.Trim()) + " and Pay_Year = '" + this.txtYear.Text.Trim() + "'";    /*Ramya 22/02/13*/
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);

            

            if (dsData.Tables[0].Rows.Count > 0 && this.pAddMode)
            {

                if (dsData.Tables[0].Rows[0]["Loc_Code"].ToString().Trim() == string.Empty)    /*Ramya 23/02/13*/
                {
                    MessageBox.Show(this.txtMonth.Text.Trim() + " Month Already Exists For All Locations", this.pPApplText);
                    cValid = false;
                    return cValid;

                }
                DataSet dsloc;  /*Ramya 23/02/13*/
                string sqlloc = "select Loc_Desc from loc_master where Loc_Code='" + dsData.Tables[0].Rows[0]["Loc_Code"].ToString().Trim() + "'";
                dsloc = oDataAccess.GetDataSet(sqlloc, null, vTimeOut);
                if (this.txtLocNm.Text.Trim() == string.Empty)    /*Ramya 23/02/13*/
                {
                    MessageBox.Show(this.txtMonth.Text.Trim() + " Month Already Exists For " + dsloc.Tables[0].Rows[0]["Loc_Desc"].ToString().Trim() + " Location", this.pPApplText);
                    cValid = false;
                    return cValid;
                }
                else
                {

                    if (dsloc.Tables[0].Rows[0]["Loc_Desc"].ToString().Trim() == this.txtLocNm.Text.Trim())  /*Ramya 23/02/13*/
                    {

                        MessageBox.Show(this.txtMonth.Text.Trim() + " Month Already Exists For " + this.txtLocNm.Text.Trim() + " Location", this.pPApplText);
                        cValid = false;
                        return cValid;
                    }
                    else
                    {
                        return true;
                    }

                    
                }

                //MessageBox.Show(this.txtMonth.Text.Trim() + " Already Exists!", this.pPApplText);
                
              
                //if (dsData.Tables[0].Columns["Loc_Code"].ToString() == string.Empty)
                //{
                //    MessageBox.Show(this.txtMonth.Text.Trim() + " Already Exists!", this.pPApplText);
                //    cValid = false;
                //    return cValid;
                //}
                
            }
            else
            {
                return true;
            }

        } 
        #endregion

        #region Functionality to save Records ( Save button Click)
        private void btnSave_Click(object sender, EventArgs e)
        {
            string vSaveString = string.Empty;
            if (txtYear.Text == string.Empty)
            {
                MessageBox.Show("Please Select Year!", this.pPApplText);
            }
            else if (txtMonth.Text == string.Empty)
            {
                MessageBox.Show("Please Select Month!", this.pPApplText);
            }


            else
            {

                bool check = monthExist();
                if (check == true)
                {

                    if (this.pAddMode)
                    {

                        this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                        oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                    }
                    if (this.pEditMode)
                    {
                        this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                        oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);

                    }
                    vYear = this.txtYear.Text;
                    vLocNm = this.txtLocNm.Text;
                    SqlStr = "Set DateFormat dmy Execute Usp_Ent_Emp_Processing_Month_Creation '" + this.txtYear.Text + "'," + this.fnNMonth(this.txtMonth.Text).ToString();
                    SqlStr = SqlStr + ",'" + vLoc_Code + "'";


                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                    timer1.Enabled = true;  /*Ramya 10/12/12*/
                    timer1.Interval = 1000;
                    MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    btnCancel_Click(this.btnCancel, e);
                }
            }

        } 
        #endregion

        #region Functionality to create save Command String
        private void mSaveCommandString(ref string vSaveString, string vkeyField, string vExclField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;

            if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
            {
                SqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
                DataSet tds = new DataSet();
                tds = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
            }
            else
            {
                vLoc_Code = string.Empty;
            }

            if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }

            if (this.pAddMode == true)
            {
                vSaveString = " Set DateFormat dmy insert into " + vMainTblNm;
                vfldList = "([Pay_Year],Pay_Month,Loc_Code,isClosed)";
                vfldValList = "'" + this.txtYear.Text.Trim() + "'";
                vfldValList = vfldValList + "," + this.fnNMonth(this.txtMonth.Text).ToString();
                vfldValList = vfldValList + ",'" + vLoc_Code + "'";
                vfldValList = vfldValList + "," + (this.chkIsClosed.Checked ? "1" : "0");
                vSaveString = vSaveString + vfldList + " Values( " + vfldValList + ")";
            }
            if (this.pEditMode == true)
            {
                vSaveString = " Set DateFormat dmy Update " + vMainTblNm + " Set Loc_Code='" + vLoc_Code + "', IsClosed=" + (this.chkIsClosed.Checked ? "1" : "0");
                vSaveString = vSaveString + " " + " Where id=" + vId;
            }


        } 
        #endregion

       
        #region Functionality to refresh a field
        private void mthFldRefresh(int rInd)
        {
            if (dgvMain.Columns[0].Name == "id")
            {
                return;
            }
            if (dgvMain.Rows[rInd].Cells["colid"].Value == null)
            {
                return;
            }
            vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();
            this.txtYear.Text = dgvMain.Rows[rInd].Cells["colYear"].Value.ToString();
            this.txtLocNm.Text = dgvMain.Rows[rInd].Cells["colLoc"].Value.ToString();
            this.txtMonth.Text = dgvMain.Rows[rInd].Cells["colcMonth"].Value.ToString();

            if (dgvMain.Rows[rInd].Cells["colIsClosed"].Value.ToString() == "True")
            {
                this.chkIsClosed.Checked = true;
            }
            else
            {
                this.chkIsClosed.Checked = false;
            }
        } 
        #endregion
      

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

        #region Functionality to select month through popup
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
            }
        } 
        #endregion

        #region Functionality to get month name from month number
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
        #endregion

        #region Functionality to get month number from month name
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
        #endregion

        #region Functionality to Delete a record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow cur = new DataGridViewRow();
            cur = dgvMain.CurrentRow;
            if (string.IsNullOrEmpty(vId) == false)
            {

                if (string.IsNullOrEmpty(vId) == false)
                {
                    if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlStr = "Delete from Emp_Processing_Month where  id=" + vId;
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                    }
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
        #endregion

        #region ToolStrip Menu item click Related

        #region ToolStrip Menu item click for new button
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }
        #endregion

        #region ToolStrip Menu item click for Edit button
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }
        #endregion

        #region ToolStrip Menu item click for Save button
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }
        #endregion

        #region ToolStrip Menu item click for Delete button
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }
        #endregion

        #region ToolStrip Menu item click for Cancel button
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        #endregion

        #region ToolStrip Menu item click for close button
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }
        #endregion 

        private void frmProcessingMonth_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord(); /*Ramya 29/10/2012*/
        }

      
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

       
    }
}
