using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;
using uBaseForm;
using uCheckBox;
using udclsDGVDateTimePicker;
using udSelectPop;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpHolidayMaster
{
    public partial class frmHolidayMaster : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet tds,tdsLoc;
        DataSet dsGrd = new DataSet();
        string SqlStr = string.Empty;
        string SqlStr1 = string.Empty;
        string vMainField = "", vMainTblNm = "Emp_Holiday_Master";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        string vYear = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;
        //int vId;
        bool cValid = true;
        bool vValid = true;
        string appPath;
        bool cCancel = false; /*Ramya 06/11/12*/
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmHolidayMaster(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Holiday Master";
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

        private void frmHolidayMaster_Load(object sender, EventArgs e)
        {
            
            DateTime dtime = new DateTime();
           //CultureInfo ci = new CultureInfo("en-US");
            //CultureInfo ci = new CultureInfo("en-US"); //.InvariantCulture;
           // CultureInfo ci = CultureInfo.InvariantCulture;



            ////Added By Amrendra On 27/02/2012  --->
            //string _SqlDefaultDateFormate = mGetSQLServerDefaDateFormate();
            //CultureInfo ci = new CultureInfo("en-US");
            ////ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            //switch (_SqlDefaultDateFormate)
            //{
            //    case "mdy":
            //        ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            //        break;
            //    case "dmy":
            //        ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //        break;
            //    case "ymd":
            //        ci.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            //        break;
            //}
            //Thread.CurrentThread.CurrentCulture = ci;


            //CultureInfo ci = new CultureInfo("en-US");
            //ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //Thread.CurrentThread.CurrentCulture = ci;


            CultureInfo ci = new CultureInfo("en-US");   /*Ramya*/
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            
           
           

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();


            

            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            this.btnFirst.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnForward.Enabled = false;
            this.btnBack.Enabled = false;

            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            

            this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;

      
            this.SetMenuRights();



            this.mInsertProcessIdRecord();
            this.SetFormColor();
            appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }

            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnYear.Image = Image.FromFile(fName);
                this.btnDesc.Image = Image.FromFile(fName);
               
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnDept.Image = Image.FromFile(fName);
                this.btnCate.Image = Image.FromFile(fName);
            }
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
        private string mGetSQLServerDefaDateFormate()//<--- Added By Amrendra On 21/01/2012 
        {
            return oDataAccess.GetDataTable("select [dateformat] from master..syslanguages where langid=(select [value] from sys.configurations where [name]='default language')", null, vTimeOut).Rows[0][0].ToString();
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

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpHolidayMaster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            //sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            string _SqlDefaultDateFormate = mGetSQLServerDefaDateFormate();
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            switch (_SqlDefaultDateFormate)
            {
                case "mdy":
                    ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
                    dtpeDate.CustomFormat = "MM/dd/yyyy";  /*Ramya*/
                    dtpsDate.CustomFormat = "MM/dd/yyyy";
                    break;
                case "dmy":
                    ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                    dtpeDate.CustomFormat = "dd/MM/yyyy"; /*Ramya*/
                    dtpsDate.CustomFormat = "dd/MM/yyyy";
                    break;
                case "ymd":
                    ci.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                    dtpeDate.CustomFormat = "yyyy/MM/dd"; /*Ramya*/
                    dtpsDate.CustomFormat = "yyyy/MM/dd";
                    break;
            }
            Thread.CurrentThread.CurrentCulture = ci;

            //sqlstr = "set dateformat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')"; &&Rup
            sqlstr = "insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            try
            {

                oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
               

            }
            catch (Exception)
            { }
           
             ci = new CultureInfo("en-US");   /*Ramya*/
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            dtpeDate.CustomFormat = "dd/MM/yyyy"; /*Ramya*/
            dtpsDate.CustomFormat = "dd/MM/yyyy";

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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }

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
                this.groupBox1.Enabled = false;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end
            mthEnableDisableFormControls();
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            //this.mthBindData();
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
            vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();
            this.txtYear.Text = dgvMain.Rows[rInd].Cells["colYear"].Value.ToString();
            this.txtLocNm.Text = dgvMain.Rows[rInd].Cells["colLocation"].Value.ToString();
            this.txtDept.Text = dgvMain.Rows[rInd].Cells["colDept"].Value.ToString();
            this.txtCategory.Text = dgvMain.Rows[rInd].Cells["colCate"].Value.ToString();
            this.txtDesc.Text = dgvMain.Rows[rInd].Cells["colDesc"].Value.ToString();
            this.dtpsDate.Value = Convert.ToDateTime(dgvMain.Rows[rInd].Cells["ColsDate"].Value.ToString());
            this.dtpeDate.Value = Convert.ToDateTime(dgvMain.Rows[rInd].Cells["ColeDate"].Value.ToString());
            // { colId, colYear, ColsDate, ColeDate, colDays,colDesc, colLocation, colDept, colCate});
            //MessageBox.Show(cur.Cells[colInd].Value.ToString());
        }
        
        private void mthGrdRefresh()
        {
            //SqlStr = "select H.*,L.Loc_Desc,D.Dept,C.Cate from Emp_Holiday_Master H Left Join Loc_Master L on (h.Loc_Code=L.Loc_Code) Left Join Department d on (h.Dept=d.Dept) Left Join Category C on (h.Cate=C.Cate) where 1=1";
            SqlStr = "select H.*,L.Loc_Desc from Emp_Holiday_Master H Left Join Loc_Master L on (h.Loc_Code=L.Loc_Code) Left Join Department d on (h.Dept=d.Dept) Left Join Category C on (h.Cate=C.Cate) where 1=1";

            if (string.IsNullOrEmpty(this.txtLocNm.Text.Trim()) == false)    /* by Ramya 05/03/12*/
            {
                SqlStr = SqlStr + " and L.Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
            }
            if (string.IsNullOrEmpty(this.txtYear.Text.Trim()) == false)
            {
                SqlStr = SqlStr + " and Pay_Year='" + this.txtYear.Text.Trim() + "'";
            }
            if (string.IsNullOrEmpty(this.txtDept.Text.Trim()) == false)    /* by Ramya 05/03/12*/
            {
                SqlStr = SqlStr + " and D.Dept='" + this.txtDept.Text.Trim() + "'";
            }
            if (string.IsNullOrEmpty(this.txtCategory.Text.Trim()) == false)  /* by Ramya 05/03/12*/
            {
                SqlStr = SqlStr + " and C.Cate='" + this.txtCategory.Text.Trim() + "'";
            }
            SqlStr = SqlStr + " Order by H.Pay_Year,sDate";
            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthBindData();
        }

        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;

            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                string fName = appPath + @"\bmp\loc-on.gif";   /*Ramya*/
                if (File.Exists(fName) == true)
                {
                    this.btnYear.Image = Image.FromFile(fName);
                    //this.btnDesc.Image = Image.FromFile(fName);
                }
            }
            else
            {
                string fName = appPath + @"\bmp\pickup.gif";
                if (File.Exists(fName) == true)
                {
                    this.btnYear.Image = Image.FromFile(fName);
                    //this.btnDesc.Image = Image.FromFile(fName);
                }
            }
                 
            //string fName = appPath + @"\bmp\pickup.gif";
            //if (File.Exists(fName) == true)
            //{
            //    this.btnLocNm.Image = Image.FromFile(fName);
            //    this.btnDept.Image = Image.FromFile(fName);
            //    this.btnCate.Image = Image.FromFile(fName);
            //}
            //fName = appPath + @"\bmp\loc-on.gif";
            //this.mthView();
            //if (dgvMain.Rows.Count > 0)
            //{
            //    this.mthFldRefresh(0);
            //}




            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnDesc.Enabled = false;
                
            }
            else
            {
                this.btnDesc.Enabled = true;
            }
            //this.btnLocNm.Enabled = vEnabled;
            //this.btnDept.Enabled = vEnabled;
            //this.btnCate.Enabled = vEnabled;
            //this.btnYear.Enabled = vEnabled;

            this.dtpsDate.Enabled = vEnabled; 
            this.dtpeDate.Enabled = vEnabled; 
         

            this.txtDesc.Enabled = vEnabled;
            this.txtYear.Enabled = vEnabled;
            this.txtLocNm.Enabled = vEnabled;
            this.txtCategory.Enabled = vEnabled;
            this.txtDept.Enabled = vEnabled;
            //this.dgvMain.Enabled = vEnabled;
            this.btnCate.Enabled = vEnabled;
            this.btnDept.Enabled = vEnabled;
            this.btnLocNm.Enabled = vEnabled;

        }

        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
          //this.btnLogout.Enabled = false;   /*Ramya*///Commented by Archana K. on 180513 for Bug-7899 
             this.btnLogout.Enabled = true;  /*Ramya*///Changed by Archana K. on 180513 for Bug-7899 
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
                 }
                 else
                 {
                     this.btnSave.Enabled = true;
                     this.btnCancel.Enabled = true;
                     this.btnLogout.Enabled = false; /*Ramya*/
                 }
                 if (this.dsGrd.Tables[0].Rows.Count == 0 && this.pAddMode == false && this.pEditMode == false)
                 {
                     this.btnEdit.Enabled = false;
                     this.btnDelete.Enabled = false; /*Ramya*/
                 }
             }//Added by Archana K. on 17/05/13 for Bug-7899
        }

        private void mthBindData()
        {

            this.dgvMain.DataSource = dsGrd.Tables[0];
            this.dgvMain.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.Name = "colId";
            
            //System.Windows.Forms.DataGridViewTextBoxColumn colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colSrNo.HeaderText = "Sr. No.";
            //colSrNo.Name = "colSrNo";

            System.Windows.Forms.DataGridViewTextBoxColumn colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colYear.HeaderText = "Payroll Year"; /*Ramya 09/11/12*/
            colYear.Name = "colYear";

            udclsDGVDateTimePicker.MicrosoftDateTimePicker ColsDate = new udclsDGVDateTimePicker.MicrosoftDateTimePicker();
            ColsDate.HeaderText = "From Date";
            ColsDate.Name = "ColsDate";

            udclsDGVDateTimePicker.MicrosoftDateTimePicker ColeDate = new udclsDGVDateTimePicker.MicrosoftDateTimePicker();
            ColeDate.HeaderText = "To Date";
            ColeDate.Name = "ColeDate";


            System.Windows.Forms.DataGridViewTextBoxColumn colDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDays.HeaderText = "Days";
            colDays.Name = "colDays";

            System.Windows.Forms.DataGridViewTextBoxColumn colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDesc.HeaderText = "Leave Description";
            colDesc.Width = 150;
            colDesc.Name = "colDesc";


            System.Windows.Forms.DataGridViewTextBoxColumn colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";

            System.Windows.Forms.DataGridViewTextBoxColumn colDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDept.HeaderText = "Department";
            colDept.Name = "colDept";

            System.Windows.Forms.DataGridViewTextBoxColumn colCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCate.HeaderText = "Category";
            colCate.Name = "colCate";




            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colId, colYear, ColsDate, ColeDate, colDays, colDesc, colLocation, colDept, colCate });
            this.dgvMain.Columns["colId"].Visible = false;

            dgvMain.Columns["colId"].DataPropertyName = "Id";
            //dgvMain.Columns["colSrNo"].DataPropertyName = "SrNo";
            dgvMain.Columns["colYear"].DataPropertyName = "Pay_Year";
            dgvMain.Columns["ColsDate"].DataPropertyName = "sDate";
            dgvMain.Columns["ColeDate"].DataPropertyName = "eDate";
            dgvMain.Columns["colDesc"].DataPropertyName = "Lv_Desc";
            dgvMain.Columns["colLocation"].DataPropertyName = "Loc_Desc";
            dgvMain.Columns["colDept"].DataPropertyName = "Dept";
            dgvMain.Columns["colCate"].DataPropertyName = "Cate";
            dgvMain.Columns["colDays"].DataPropertyName = "Days";
            dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);
            this.mthChkNavigationButton();
        }

        private void mthNew(object sender, EventArgs e)
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
            this.mthBindClear();
            this.mthEnableDisableFormControls();
            this.txtYear.Focus();
         
        }

        private void mthBindClear()
        {
            this.txtYear.Text = "";
            this.txtLocNm.Text = "";
            this.txtDept.Text = "";
            this.txtCategory.Text = "";
            this.txtDesc.Text = "";

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.mDeleteProcessIdRecord();
            Application.Exit();
        }

        private void btnYear_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); /*Ramya*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            if (this.pAddMode == false && this.pEditMode == false)  /*Added By Ramya*/
            {
                SqlStr = "Select distinct Pay_Year from "+vMainTblNm;
                
                //SqlStr = "Select a.Pay_Year ,b.Loc_Desc,a.Dept,a.Cate from " + vMainTblNm;
                //SqlStr = SqlStr + " a Inner Join Loc_Master b on(a.Loc_code=b.Loc_Code)";
                //SqlStr = SqlStr + " Order by Pay_Year,Loc_Desc,Dept,Cate";
                                       
            }
            else
            {
                  SqlStr = "Select Pay_Year from Emp_Payroll_Year_Master order by Pay_Year";                              
            }

            try
            {

               // dsData = oDataAccess.GetDataSet(sqlQuery, null, 20);
                tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            catch (System.Exception sqlEx)
            {
                string[] msgString = sqlEx.Message.Split('.');
                MessageBox.Show("" + msgString [0] );
                return;               
            }

           // tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    MessageBox.Show("No Record Found");
                }
                else
                {
                    MessageBox.Show("Please create Pay Year in Payroll Year Master");
                }
                return;
            }

            VForText = "Select Payroll Year";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Payroll Year";
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
                this.txtYear.Text = oSelectPop.pReturnArray[0];
                //this.dtpsDate.Text = oSelectPop.pReturnArray[1];  /*Ramya */
                //this.dtpeDate.Text = oSelectPop.pReturnArray[2]; /*Ramya */
                          
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.txtLocNm.Text = "";
                    this.txtDept.Text = "";
                    this.txtCategory.Text = "";
                    this.mthGrdRefresh();
                    this.mthFldRefresh(0);  /*Ramya 05/03/2012*/

                }
     
            }

        }

        private void btnLocNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Loc_Desc as LocNm,Loc_Code from Loc_master order by Loc_Desc";   
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
               // MessageBox.Show("No Record Found");
               //return;
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Please create Location in Location Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
                
            }

            VForText = "Select Location Name";
            vSearchCol = "LocNm";  /*Loc_Code Ramya */
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
           
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select Dept from Department order by Dept";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //MessageBox.Show("No Record Found");
                //return;

                if (this.pAddMode == false && this.pEditMode == false)
                {
                    MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Please create Department in Department Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;

                
            }

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
          
        }

        private void btnCate_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select Cate from Category order by Cate";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //MessageBox.Show("No Record Found");
                //return;
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Please create Category in Category Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;

            }

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

         }
   
         
        private void btnDesc_Click(object sender, EventArgs e)  /*Ramya 02/03/12 */
        {
            this.mcheckCallingApplication(); /*Ramya*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select a.Lv_Desc ,b.Loc_Desc,a.Dept,a.Cate from " + vMainTblNm;
            SqlStr = SqlStr + " a Left Join Loc_Master b on(a.Loc_code=b.Loc_Code)";
            SqlStr = SqlStr + " Where Pay_Year = '"+this.txtYear.Text+"'";
            SqlStr = SqlStr + " Order by Lv_Desc,Loc_Desc,Dept,Cate";
         
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            VForText = "Select Leave Description";
            vSearchCol = "Lv_Desc";
            vDisplayColumnList = "Lv_Desc:Dsescription,Loc_Desc:Location,Dept:Department,Cate:Category";
            vReturnCol = "Lv_Desc,Loc_Desc,Dept,Cate";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                this.txtDesc.Text = oSelectPop.pReturnArray[0];
                this.txtLocNm.Text = oSelectPop.pReturnArray[1];
                this.txtDept.Text = oSelectPop.pReturnArray[2];
                this.txtCategory.Text = oSelectPop.pReturnArray[3];


                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthGrdRefresh();
                }
            }
           

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cCancel = true;  /*Ramya 06/11/12*/
            if (pAddMode)
            {
                if (dgvMain.Rows.Count > 0)
                {
                    this.mthFldRefresh(0);
                }
            }
            else   /*Ramya 30/3/12*/
            {
               this.txtLocNm.Text="";
               this.txtYear.Text="";
                this.txtDept.Text="";
                this.txtCategory.Text = "";
               
            }
           // this.mthBindClear();
            if (string.IsNullOrEmpty(vYear))
            {
                this.txtYear.Text = vYear;
            }
            if (string.IsNullOrEmpty(vLocNm))
            {
                this.txtLocNm.Text = vLocNm;
            }
            if (string.IsNullOrEmpty(vDept))
            {
                this.txtDept.Text = vDept;
            }
            if (string.IsNullOrEmpty(vCate))
            {
                this.txtCategory.Text = vCate;
            }
          
            //this.txtYear.Enabled = false;
            //this.txtLocNm.Enabled = false;
            //this.txtDesc.Enabled = false;
            //this.txtDept.Enabled = false;
            //this.txtCategory.Enabled = false;
            //this.dtpsDate.Enabled = false;
            //this.dtpeDate.Enabled = false;
            //this.btnDesc.Enabled = true;

            this.pAddMode = false;
            this.pEditMode = false;
            mthEnableDisableFormControls();

            this.mthGrdRefresh();
            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            this.mthChkNavigationButton();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
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
            vDept = this.txtDept.Text;
            vCate = this.txtCategory.Text;

            //this.mthNew(sender, e);
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            string _SqlDefaultDateFormate = mGetSQLServerDefaDateFormate();
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            switch (_SqlDefaultDateFormate)
            {
                case "mdy":
                    ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
                    dtpeDate.CustomFormat = "MM/dd/yyyy";  /*Ramya*/
                    dtpsDate.CustomFormat = "MM/dd/yyyy";
                    break;
                case "dmy":
                    ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                    dtpeDate.CustomFormat = "dd/MM/yyyy"; /*Ramya*/
                    dtpsDate.CustomFormat = "dd/MM/yyyy";
                    break;
                case "ymd":
                    ci.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                    dtpeDate.CustomFormat = "yyyy/MM/dd"; /*Ramya*/
                    dtpsDate.CustomFormat = "yyyy/MM/dd";
                    break;
            }
            Thread.CurrentThread.CurrentCulture = ci;



            cValid = true;   /*Ramya*/
            this.lblMand.Focus();

            if (cValid == false)
                return;

          

            if (string.IsNullOrEmpty(this.txtYear.Text.Trim()))       /*Ramya 05/03/12*/
            {
               MessageBox.Show("Year Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               btnYear.Focus();
               return;
            }
            if (string.IsNullOrEmpty(this.dtpsDate.Text.Trim()))      /*Ramya 05/03/12*/
            {
                MessageBox.Show("From Date Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (string.IsNullOrEmpty(this.dtpeDate.Text.Trim()))       /*Ramya 05/03/12*/
            {
                MessageBox.Show("To Date Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (string.IsNullOrEmpty(this.txtDesc.Text.Trim()))      /*Ramya 05/03/12*/
            {
                MessageBox.Show("Leave Description Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        
            if (dtpsDate.Value.Date > dtpeDate.Value.Date) /*Ramya 06/11/12*/
            {
                MessageBox.Show("To Date Greater than or equal to From Date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpeDate.Focus();
                return;
            }

            vValid = true;
            this.mthChkSaveValidation(ref vValid);   /*Ramya*/
            if (vValid == false)
            {
                return;
            }
            string vSaveString = string.Empty;

            if (this.pAddMode)  
            {

                this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
            }
            if (this.pEditMode)
            {
               
                this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                try
                {
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message,this.pPApplText);
                }
            }
            
            _SqlDefaultDateFormate = mGetSQLServerDefaDateFormate();
             ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            dtpeDate.CustomFormat = "dd/MM/yyyy"; /*Ramya*/
            dtpsDate.CustomFormat = "dd/MM/yyyy";

            vYear = this.txtYear.Text;
            vLocNm = this.txtLocNm.Text;
            vDept = this.txtDept.Text;
            vCate = this.txtCategory.Text;

            this.txtLocNm.Text="";
            this.txtDept.Text="";
            this.txtCategory.Text="";

            this.pAddMode = false;
             this.pEditMode = false;
            this.mthGrdRefresh();
            //MessageBox.Show(vId);
            mthFldRefresh(dgvMain.Rows.Count-1);   /*Ramya 05/03/12*/
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
            //ci = new CultureInfo("en-US");
            //ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //Thread.CurrentThread.CurrentCulture = ci;

        }
       
        private void mthChkSaveValidation(ref Boolean vValid)
        {
              //if(!string.IsNullOrEmpty(vId.ToString()))
              //{
              //      SqlStr = "select Pay_Year+Lv_Desc from " + vMainTblNm + " where Pay_Year+Lv_Desc='" + this.txtYear.Text.Trim() + this.txtDesc.Text.Trim() + "'";  /*Ramya 05/03/12*/
              //      SqlStr = SqlStr + " and id!=" + vId;
              //      DataSet ds = oDataAccess.GetDataSet(SqlStr, null, 20);
              //     // MessageBox.Show(vId);
              //      if (ds.Tables[0].Rows.Count > 0)
              //      {
                       
              //              MessageBox.Show("Holiday for " + txtDesc.Text.Trim() + " is already there in " + txtYear.Text.Trim());
              //              vValid = false;
                        
              //      } 
              //}/*Ramya 05/03/12*/

           // MessageBox.Show(vId);
            if (pAddMode)
            {
                vId = Convert.ToString(0);
            }

          //  SqlStr = "Set DateFormat dmy execute Usp_Ent_Holiday_Master " + txtYear.Text + ",'" + dtpsDate.Value.Date + "','" + dtpeDate.Value.Date  ;           // dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            //SqlStr = "set dateformat dmy  execute Usp_Ent_Holiday_Master " + txtYear.Text + ",'" + dtpsDate.Value.Date + "','" + dtpeDate.Value.Date;           // dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            SqlStr = "execute Usp_Ent_Emp_Holiday_Master '" + txtYear.Text + "','" + dtpsDate.Value.Date + "','" + dtpeDate.Value.Date;          //Rup  /*Ramya added '" + txtYear.Text + "' on 01/10/12*/
            SqlStr = SqlStr + "','" + txtCategory.Text.Trim() + "','" + txtDept.Text.Trim() + "','" + txtLocNm.Text.Trim() + "',"+vId;

           
            try
            {
                dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,this.pPApplText);
            }

                // if (dsMain.Tables[0].Rows[0][0].ToString() == "Y")
            if (dsMain.Tables[0].Rows[0][0].ToString().Trim() != "")

            {
                DateTime dt = (DateTime)dsMain.Tables[0].Rows[0][1];
               // MessageBox.Show("There is already holiday on " + dt.Date.ToShortDateString());
                MessageBox.Show(dsMain.Tables[0].Rows[0][0].ToString());
                dtpeDate.Focus();
                vValid = false;
                return;

            }
        }

        private void mSaveCommandString(ref string vSaveString, string vkeyField, string vExclField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;
            string vLoc_Code = string.Empty;

            //int vDays = DateTime.Compare(((DateTime)this.dtpeDate.Value), ((DateTime)this.dtpsDate.Value));
            int vDays = (((DateTime)this.dtpeDate.Value).Subtract(((DateTime)this.dtpsDate.Value))).Days;
            vDays = vDays + 1;

            if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
            {
                SqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
                DataSet tds = new DataSet();
                tds = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
            }

            if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }

            if (this.pAddMode == true)
            {
                //vSaveString = " Set DateFormat dmy insert into " + vMainTblNm;
                vSaveString = "insert into " + vMainTblNm;
                vfldList = "([Pay_Year],sDate,eDate,Lv_Desc,Loc_Code,Dept,Cate,Days)";
                vfldValList = "'" + this.txtYear.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.dtpsDate.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.dtpeDate.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.txtDesc.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + vLoc_Code + "'";
                vfldValList = vfldValList + ",'" + this.txtDept.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.txtCategory.Text.Trim() + "'";
                vfldValList = vfldValList + "," + vDays.ToString();
                vSaveString = vSaveString + vfldList + " Values( " + vfldValList + ")";
            }
            if (this.pEditMode == true)
            {
               //vSaveString = " Set DateFormat dmy Update " + vMainTblNm + " Set ";
              

                vSaveString = " Update " + vMainTblNm + " Set ";
                string vWhereCondn = string.Empty;
                vfldValList = vfldValList + "[Pay_Year]=" + "'" + this.txtYear.Text.Trim() + "'";
                vfldValList = vfldValList + ",sDate=" + "'" + this.dtpsDate.Text.Trim() + "'";
                vfldValList = vfldValList + ",eDate=" + "'" + this.dtpeDate.Text.Trim() + "'";
                vfldValList = vfldValList + ",Lv_Desc=" + "'" + this.txtDesc.Text.Trim() + "'";
                vfldValList = vfldValList + ",Loc_Code=" + "'" + vLoc_Code + "'";
                vfldValList = vfldValList + ",Dept=" + "'" + this.txtDept.Text.Trim() + "'";
                vfldValList = vfldValList + ",Cate=" + "'" + this.txtCategory.Text.Trim() + "'";
                vfldValList = vfldValList + ",Days=" + vDays.ToString();
                //vfldValList = vfldValList + ")";
                vWhereCondn = " Where id=" + vId;
                vSaveString = vSaveString + vfldValList + vWhereCondn;
            }

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
            this.btnDesc.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthDelete();
        }

        private void mthDelete()
        {
            int n = 0;
            try
            {
                n = dgvMain.CurrentRow.Index - 1;
            }
            catch
            {
            }
           
            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
         MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sqlstr = "delete from " + vMainTblNm + " where id=" + vId;
                oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
                //this.mthGrdRefresh();

                this.txtLocNm.Text = "";
                this.txtCategory.Text = "";
                this.txtDept.Text = "";

                //if (dgvMain.Rows.Count > 0)
                //    this.mthFldRefresh(0);
                this.mthGrdRefresh();         /*Ramya 05/03/12*/
               
                if (dgvMain.Rows.Count > 0)   /*Ramya 05/03/12*/
                {
                    this.mthFldRefresh(n);
                }
                else 
                {                  

                    this.txtYear.Text = "";
                    this.txtLocNm.Text = "";
                    this.txtDept.Text = "";
                    this.txtCategory.Text = "";
                    this.txtDesc.Text = "";
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    this.mthGrdRefresh();   /*Ramya 30/3/12*/
                    this.mthFldRefresh(0);  /*Ramya 30/3/12*/ 

                }

            }
            
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

            

        private void frmHolidayMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        //private void dtpsDate_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtYear.Text))
        //    {
        //        MessageBox.Show("Please Select Year First");
        //        btnYear.Focus();
        //        cValid = false;
                
        //        return;
        //    }
        //    else
        //    {
        //        SqlStr = "execute Usp_Holiday_Master " + txtYear.Text + ",'" + dtpsDate.Value.Date + "'";
        //        dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
        //        if (dsMain.Tables[0].Rows[0][0].ToString() == "Y")
        //        {
        //            MessageBox.Show("There is already holiday on " + dtpsDate.Value.Date.ToShortDateString());
        //            dtpsDate.Focus();
        //            cValid = false;
        //            return;
                    
        //        }
        //    }
        //}

        private void dtpeDate_Validating(object sender, CancelEventArgs e)
        {
            if (cCancel == true)   /*Ramya 06/11/12*/
            {
                cCancel = false;
                return;
            }
            if (dtpsDate.Value.Date > dtpeDate.Value.Date)
            {
                MessageBox.Show("To Date Greater than or equal to From Date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpeDate.Focus();
                return;
            }
           
          
        }

        private void txtYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnYear_Click(sender, new EventArgs());
                }
            }
        }

        private void txtLocNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnLocNm_Click(sender, new EventArgs());
            }
        }

        private void txtDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnDept_Click(sender, new EventArgs());
            }
        }

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnCate_Click(sender, new EventArgs());
            }
        }

        private void txtDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnDesc_Click(sender, new EventArgs());
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //private void txtYear_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtYear.Text))
        //    {
        //        MessageBox.Show("Please Select Year First");
        //        btnYear.Focus();
        //        cValid = false;

        //        return;
        //    }

        //}

       

      
       
       
    }
}
