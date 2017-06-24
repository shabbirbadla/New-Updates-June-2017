using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;
//using ueconnect;
using GetInfo;
using uNumericTextBox;
using Udyog.Library.Common;


namespace udCashIn
{
    public partial class frmuDCashIn : uBaseForm.FrmBaseForm
    {

        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        string SqlStr = string.Empty;
        string pMainField = "CNTRCODE", pMainTblNm = "CounterMast", pMainFldVal = "";
        string vMainTblNm = "POSCashIn", vMainField = "Tran_cd", vMainFldVal = ""; 
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        String cntrid=string.Empty,cntrcode = string.Empty,inv_no = string.Empty,invno = string.Empty,cashin = string.Empty;
        String userName = string.Empty, ldate = string.Empty,compID = string.Empty,sysdate = string.Empty,trnddt = string.Empty,date = string.Empty;
        Boolean cValid = true;
        Boolean Iscancel = false;
        //clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        
        public frmuDCashIn(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */
            InitializeComponent();
            timer1.Enabled = true;
            timer1.Interval = 1000;
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Counter Cash In";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            userName = this.pAppUerName;
            if (string.IsNullOrEmpty(this.lbluser.Text))
            {
                lbluser.Text = this.pAppUerName;
            }
            
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
            
        }

        private void frmuDCashIn_Load(object sender, EventArgs e)
        {
            
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;
            this.txtCash.Enabled = false; 
            this.txtCash.pAllowNegative = false;
            this.txtCash.pDecimalLength = 2;
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.SetMenuRights();
            startupPath = Application.StartupPath; //by Ruchit
            //startupPath = @"D:\VudyogSDK"; //added by Ruchit
            //oConnect = new clsConnect(); //commented on 03/03/2016
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            //oConnect.InitProc("'" + startupPath + "'", appfile); //commented on 03/03/2016
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
                //string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^'); //commented on 03/03/2016
                //ServiceType = objRegisterMe[15].ToString().Trim(); //commented on 03/03/2016
                UdyogRegister regiInfo = new UdyogRegister();
                ServiceType = regiInfo.RegistrationInfo.ServiceType;
                
            }
            
            this.btnLast_Click(sender, e);


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
                this.btnGradeName.Image = Image.FromFile(fName);

            }
            string logonname = appPath + @"\bmp\loc-on.gif";
            string logoffname = appPath + @"\bmp\loc-off.gif";
            if(File.Exists(logonname) == true)
            {
                if (this.pAddMode == false || this.pEditMode == false)
                {
                    this.btnCashIn.Image = Image.FromFile(logonname);
                }
                else if(File.Exists(logoffname) == true)
                {
                    this.btnCashIn.Image = Image.FromFile(logoffname);
                }
            }
            this.GetInvData();
        }
        
        private void GetInvData()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsVal = new DataSet();
            String a = "select user_name,sta_dt,end_dt,compid from vudyog..co_mast";
            a = a+"; select getdate() as sysdate";
            if (String.IsNullOrEmpty(this.txtInvNo.Text) == false)
            {
                a = a + "; select trndttime from poscashin where inv_no=" + txtInvNo.Text;
            }
            dsMenu = oDataAccess.GetDataSet(a, null, 20);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    ldate = dsMenu.Tables[0].Rows[0]["sta_dt"].ToString() + dsMenu.Tables[0].Rows[0]["end_dt"].ToString();
                    ldate = ldate.Substring(6,4)+"-"+ldate.Substring(28,4);
                    compID = dsMenu.Tables[0].Rows[0]["compid"].ToString();
                    sysdate = dsMenu.Tables[1].Rows[0]["sysdate"].ToString();
                    date = sysdate.Substring(0,10);
                }
                //ruchit start 01/03/2016
                if (String.IsNullOrEmpty(this.txtInvNo.Text) == false)
                {
                    if (dsMenu.Tables[2].Rows.Count > 0)
                    {
                        date = dsMenu.Tables[2].Rows[0]["trndttime"].ToString().Substring(0, 10);
                    }
                    else
                    {
                        date = sysdate.Substring(0, 10);
                    }
                }
                //ruchit start 01/03/2016
            }
            dsVal = oDataAccess.GetDataSet(a, null, 20);
            txtdt.Text = date;
            date = txtdt.Text;
        }

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

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            
            cAppName = "udCashIn.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
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

        private void mthBindClear()
        {
            this.txtGradeName.DataBindings.Clear();
            this.txtInvNo.DataBindings.Clear();
            this.txtCash.DataBindings.Clear();
            this.lblmod.DataBindings.Clear();
            this.lbltran.DataBindings.Clear();
            this.lbluser.DataBindings.Clear();
            this.txtdt.DataBindings.Clear(); //added by Ruchit on 01/03/2016
        }

        private void mthBindData()
        {
            if (dsMain.Tables.Count == 0)
            {
                return;
            }
            else
            {
            this.lbluser.DataBindings.Add("Text", dsMain.Tables[0], "USERNAME");
            this.txtInvNo.DataBindings.Add("Text", dsMain.Tables[0], "INV_NO");
            this.txtCash.DataBindings.Add("Text", dsMain.Tables[0], "CashInAmt");
            this.txtGradeName.DataBindings.Add("Text", dsMain.Tables[0], "CNTRCODE");
            this.lblmod.DataBindings.Add("Text", dsMain.Tables[0], "SYSDATE");
            this.lbltran.DataBindings.Add("Text", dsMain.Tables[0], "TRNDTTIME");
            this.txtdt.DataBindings.Add("Text", dsMain.Tables[0], "SYSDATE").ToString().Substring(0, 10); //added by Ruchit on 01/03/2016
            }

        }

        private void mthEnableDisableFormControls()
        {
            if (this.pAddMode || this.pEditMode)
            {
                this.btnGradeName.Enabled = true;
                this.btnCashIn.Enabled = false;
            }
            else
            {
                this.btnGradeName.Enabled = false;
                this.txtGradeName.Enabled = false;
                this.txtCash.Enabled = false;
                this.btnCashIn.Enabled = true;
            }


            if (this.pAddMode)
            {
                this.txtGradeName.Enabled = false;
            }
            else if (this.pEditMode)
            {
                this.txtGradeName.Enabled = false;
                this.btnGradeName.Enabled = false;
                this.txtCash.Enabled = true;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();

        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = false;
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.groupBox1.Enabled = false;
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
            }
            else
            this.mthEnableDisableFormControls();
            DataSet dsTemp = new DataSet();
            string SqlStr = "select top 1  " + vMainField + " as Col1 from " + vMainTblNm + " order by  " + vMainField + " desc";
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
            vMainFldVal = "";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["Col1"].ToString()) == false)
                {
                    vMainFldVal = dsTemp.Tables[0].Rows[0]["Col1"].ToString().Trim();
                }
            }
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();


            if (dsMain.Tables[0].Rows.Count == 0)
            {
                this.btnEmail.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            DataSet dsNew = new DataSet();
            string sqlnew = "select top 1 username,tran_cd,cashotran from POSCashIn where username='"+userName+"' order by Tran_cd desc" ;
            dsNew = oDataAccess.GetDataSet(sqlnew, null, 20);
            if (dsNew.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsNew.Tables[0].Rows[0]["cashotran"]) == 0)
                {
                    MessageBox.Show("Please do a cashout entry first for the user", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.txtCash.Enabled = false;
                }
                else
                {
                    this.pAddMode = true;
                    this.pEditMode = false;
                    this.mthNew(sender, e);
                    this.mthChkNavigationButton();
                    this.lbluser.Text = dsMain.Tables[0].Rows[0]["username"].ToString().ToUpper();
                    this.txtCash.Enabled = true;
                    this.txtInvNo.Focus();
                    txtdt.Text = sysdate.Substring(0,10); //ruchit
                }
            }
            else
            {
                this.pAddMode = true;
                this.pEditMode = false;
                this.mthNew(sender, e);
                this.mthChkNavigationButton();
                this.lbluser.Text = dsMain.Tables[0].Rows[0]["username"].ToString().ToUpper();
                this.txtCash.Enabled = true;
                this.txtInvNo.Focus();
                txtdt.Text = sysdate.Substring(0, 10); //ruchit
            }
        }

        private void mthNew(object sender, EventArgs e)
        {
            this.mthBindClear();
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                dsMain.Tables[0].Rows.RemoveAt(0);
                dsMain.Tables[0].AcceptChanges();
            }
            DataRow drCurrent;
            drCurrent = dsMain.Tables[0].NewRow();
            dsMain.Tables[0].Rows.Add(drCurrent);
            this.mthBindData();
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();
            dsMain.Tables[0].Rows[0]["username"] = this.pAppUerName;
            DataSet dsNew = new DataSet();
            DataTable inv = new DataTable();
            string sqlnew = "select top 1 inv_no from POSCashIn order by inv_no desc";
            inv = oDataAccess.GetDataSet(sqlnew, null, 20).Tables[0];
            if (inv.Rows.Count <= 0)
                inv.Rows.Add();
            { 
            if (string.IsNullOrEmpty(inv.Rows[0]["Inv_no"].ToString()))
            {
                dsMain.Tables[0].Rows[0]["inv_no"] = "0";
            }
            else
            {
                dsMain.Tables[0].Rows[0]["inv_no"] = inv.Rows[0][0].ToString();
            }
                string x = (Convert.ToInt16(dsMain.Tables[0].Rows[0]["Inv_No"]) + 1).ToString();
                inv_no = x.PadLeft(6, '0');
                this.txtInvNo.Text = inv_no;
                invno = this.txtInvNo.Text;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthEdit();
        }

        private void mthEdit()
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            else if ((Convert.ToInt32(this.dsMain.Tables[0].Rows[0]["CashOTran"]) == 0) && userName == this.dsMain.Tables[0].Rows[0]["USERNAME"].ToString())
            {
                this.pAddMode = false;
                this.pEditMode = true;
                this.mthEnableDisableFormControls();
                dsMain.Tables[0].Rows[0].BeginEdit();
                this.mthChkNavigationButton();
            }
            else if ((Convert.ToInt32(this.dsMain.Tables[0].Rows[0]["CashOTran"]) > 0) && userName == this.dsMain.Tables[0].Rows[0]["USERNAME"].ToString())
            {
                MessageBox.Show("CashIn Entry can't be edited as CashOut Transaction is already done.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else if (userName != this.dsMain.Tables[0].Rows[0]["CashOTran"].ToString())
            {
                MessageBox.Show("You're not authorized to edit this transaction.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            cValid = true;  
            this.lblMand.Focus();
            if (cValid == false)
                return;

            this.mcheckCallingApplication();

            Boolean vValid = true;
            this.mthChkSaveValidation(ref vValid);
            if (vValid == false)
            {
                return;
            }


            this.Refresh();
            this.mthSave();

            this.mthChkNavigationButton();
        }

        private void mthSave()
        {
            string vSaveString = string.Empty;
            dsMain.Tables[0].Rows[0].AcceptChanges();
            dsMain.Tables[0].Rows[0].EndEdit();
            this.mSaveCommandString(ref vSaveString, "#ID#");
            this.pAddMode = false; 
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
            SqlStr = "Select top 1 * from " + vMainTblNm + " order by tran_cd desc";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();

        }

        private void mthChkSaveValidation(ref Boolean vValid)
        {
            DataSet ds = new DataSet();
            DataSet dsVal = new DataSet();
            String a = "select getdate() as sysdate";
            ds = oDataAccess.GetDataSet(a, null, 20);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    sysdate = ds.Tables[0].Rows[0]["sysdate"].ToString();
                }
            }
            dsVal = oDataAccess.GetDataSet(a, null, 20);
             if (string.IsNullOrEmpty(this.txtGradeName.Text.Trim()))
            {
                MessageBox.Show("Counter Code cannot be Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
                this.txtGradeName.Focus();
            }
            

        }

        private void mSaveCommandString(ref string vSaveString, string vkeyField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;
            
            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + vMainTblNm + " ' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                dsMain.Tables[0].Rows[0]["ENTRY_TY"] = "CI";
                dsMain.Tables[0].Rows[0]["CNTRID"] = cntrid;
                dsMain.Tables[0].Rows[0]["CNTRCODE"] = cntrcode;
                dsMain.Tables[0].Rows[0]["l_yn"] = ldate;
                dsMain.Tables[0].Rows[0]["username"] = userName;
                dsMain.Tables[0].Rows[0]["compid"] = compID;
                dsMain.Tables[0].Rows[0]["trndttime"] = sysdate;
                dsMain.Tables[0].Rows[0]["sysdate"] = sysdate;
                //dsMain.Tables[0].Rows[0]["date"] = date; //commented by ruchit on 01/03/2016
                dsMain.Tables[0].Rows[0]["date"] = sysdate.Substring(0,10); //added by Ruchit on 01/03/2016
                DataTable InvNo = new DataTable();
                string SQlStr = "";
                SQlStr = "Select TOP 1 INV_NO FROM POSCASHIN ORDER BY INV_NO DESC ";
                InvNo = oDataAccess.GetDataSet(SQlStr, null, 20).Tables[0];
                if (InvNo.Rows.Count > 0)
                {
                    string _InvNo = (Convert.ToInt16(InvNo.Rows[0][0]) + 1).ToString();
                    dsMain.Tables[0].Rows[0]["Inv_no"] = _InvNo.PadLeft(6, '0');
                    invno = this.dsMain.Tables[0].Rows[0]["Inv_no"].ToString();
                    this.txtInvNo.Text = invno;
                }
                else
                {
                    dsMain.Tables[0].Rows[0]["Inv_no"] = this.txtInvNo.Text;
                }
                dsMain.Tables[0].Rows[0]["INV_NO"] = invno;
                vSaveString = "Set DateFormat dmy insert into " + vMainTblNm;
                dsMain.Tables[0].AcceptChanges();
                foreach (DataColumn dtc1 in dsMain.Tables[0].Columns)
                {

                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + dtc1.ToString().Trim();
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();

                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if ((dtc1.DataType.Name.ToLower() == "decimal" || dtc1.DataType.Name.ToLower() == "int16" || dtc1.DataType.Name.ToLower() == "int32") && string.IsNullOrEmpty(vfldVal))
                        {
                            vfldVal = "0";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean")
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
            
            if (this.pEditMode == true)
                
                foreach (DataRow Posref in dsMain.Tables[0].Rows)
                {
                    vSaveString = "";
                    vSaveString = "Set DateFormat dmy update PosCashIn  set SYSDATE= '"+sysdate+"' ,CashInAmt="+txtCash.Text+"where Tran_cd="+Posref["Tran_cd"];
                }
            this.mthChkNavigationButton();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Iscancel = true;
            this.txtCash.Clear();
            this.txtCash.Enabled = false;
            this.txtInvNo.Clear();
            this.txtInvNo.Enabled = false;
            this.txtGradeName.Clear();
            this.txtCash.Clear();
            this.txtCash.Enabled = false;
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
            if (string.IsNullOrEmpty(this.lbluser.Text))
            {
                lbluser.Text = this.pAppUerName;
            }
            if (string.IsNullOrEmpty(this.txtdt.Text))
            {
                txtdt.Text = sysdate.Substring(0, 10);
            }

        }

        private void mthCancel(object sender, EventArgs e)
        {
            
            if (this.dsMain.Tables[0].Rows.Count != 0) 
            {
                dsMain.Tables[0].Rows[0].CancelEdit();
            }

            this.mthEnableDisableFormControls();
            if (this.dsMain.Tables[0].Rows.Count == 1)
            {
                dsMain.Tables[0].Rows[0].CancelEdit();
                if (this.pAddMode)
                {
                    dsMain.Tables[0].Rows[0].Delete();
                    this.btnLast_Click(sender, e);
                }
                if (this.pEditMode)
                {
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
                    this.mthView();
                }
            }
            txtCash.Focus();
            this.pAddMode = false;
            this.pEditMode = false;
            this.txtCash.Enabled = false;
            this.btnCashIn.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthDelete();
            // ruchit added for bug 27503 on 01/03/2016 
            if (string.IsNullOrEmpty(this.txtdt.Text))
            {
                txtdt.Text = sysdate.Substring(0, 10);
            }
            // ruchit added for bug 27503 on 01/03/2016
        }

        private void mthDelete()
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            if ((Convert.ToInt32(this.dsMain.Tables[0].Rows[0]["CashOTran"]) == 0) && userName == this.dsMain.Tables[0].Rows[0]["USERNAME"].ToString())
            {
                //do nothing;
            }
            else if (userName != this.dsMain.Tables[0].Rows[0]["username"].ToString())
            {
                MessageBox.Show("You're not authorized to delete this entry", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                MessageBox.Show("Could not delete this entry " + Environment.NewLine + " It has been used in cashout Transaction", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string vDelString = string.Empty;
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                vDelString = "Delete from " + vMainTblNm + "  Where tran_cd=" + dsMain.Tables[0].Rows[0]["tran_cd"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();

                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + ">'" + vMainFldVal + "' order by "+vMainField;
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by "+vMainField+" desc";
                        dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    }

                }
                this.mthView();
                this.mthChkNavigationButton();
            }

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //ruchit for testing start
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
        //ruchit for testing end
        private void mthView()
        {
            this.mthBindClear();
            this.mthBindData();
           
        }

        private void mthChkNavigationButton()
        {
            DataSet dsTemp = new DataSet();
            this.btnForward.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            this.btnLocate.Enabled = false;
            btnEdit.Enabled = false;
            Boolean vBtnAdd, vBtnEdit, vBtnDelete, vBtnPrint;
            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (dsMain.Tables[0].Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false)
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;
                        this.btnGradeName.Enabled = false;

                        vBtnAdd = true;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                    }
                    return;
                }
            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    if (string.IsNullOrEmpty(this.lbluser.Text) == true)
                    {
                    //lbluser.Text = dsMain.Tables[0].Rows[0]["username"].ToString(); //02/02/2016
                        lbluser.Text = this.pAppUerName;
                    }
                    SqlStr = "select tran_cd from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "'";
                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = "select tran_cd from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnBack.Enabled = true;
                        this.btnFirst.Enabled = true;
                    }
                }

            }
           
            vBtnAdd = false;
            vBtnDelete = false;
            vBtnEdit = false;
            vBtnPrint = false;
            //this.pAddMode = false; //ruchit for testing
            //this.pEditMode = false; //ruchit for testing
            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (this.btnForward.Enabled == true || this.btnBack.Enabled == true || (this.pAddMode == false && this.pEditMode == false))
                {
                    vBtnAdd = true;
                    if (dsMain.Tables[0].Rows.Count > 0)
                    {
                        vBtnDelete = true;
                        vBtnEdit = true;
                        vBtnPrint = true;
                        this.btnGradeName.Enabled = false;
                    }
                }
                this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
            }
        }

        private void mthChkAEDPButton(Boolean vBtnAdd, Boolean vBtnEdit, Boolean vBtnDelete, Boolean vBtnPrint)
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnLocate.Enabled = false;
            if (vBtnAdd && this.pAddButton)
            {
                this.btnNew.Enabled = true;
            }
            if (vBtnEdit && this.pEditButton)
            {
                this.btnEdit.Enabled = true;
            }
            if (vBtnDelete && this.pDeleteButton)
            {
                this.btnDelete.Enabled = true;
            }
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnLogout.Enabled = false;
            this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;
            //this.btnNew.Enabled = true; //ruchit for testing
            if (this.btnNew.Enabled == false && this.btnEdit.Enabled == false)
            {
                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
            }
            if (this.btnSave.Enabled == false)
            {
                this.btnLogout.Enabled = true;
            }
        }

        private void btnGradeName_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication(); 
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select * from " + pMainTblNm + " order by " + "cntrid";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Counter Code";
            vSearchCol = "CNTRCODE";
            vDisplayColumnList = "CNTRCODE:Counter Code";
            vReturnCol = "CNTRCODE";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGradeName.Text = oSelectPop.pReturnArray[0];
                pMainFldVal = this.txtGradeName.Text.Trim();
                SqlStr = "Select top 1 * from " + pMainTblNm + " Where CNTRCODE='" + pMainFldVal + "' order by "+pMainField;
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                cntrid = tDs.Tables[0].Rows[0]["CNTRID"].ToString().Trim();
                cntrcode = tDs.Tables[0].Rows[0]["CNTRCODE"].ToString().Trim();
            }
            this.mthView();
            this.txtGradeName.Text = cntrcode;
            this.txtInvNo.Text = invno;
            this.txtdt.Text = sysdate.Substring(0,10); 
            this.mthChkNavigationButton();
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.btnLogout.Enabled)
            btnExit_Click(this.btnExit, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
        private void txtGradeName_Validating(object sender, CancelEventArgs e)
        {  
            if (Iscancel == true) 
            {
                Iscancel = false;
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " select " + vMainField + " from " + vMainTblNm + " where " + vMainField + "='" + txtGradeName.Text.Trim() + "'";
            if (this.pEditMode)
            {
                sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            }
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                ToolTip t = new ToolTip();
                string ErrMsg = t.GetToolTip(txtGradeName).ToString();
                errorProvider.SetError(txtGradeName, "Duplicate " + lblGrade.Text + " value");
                txtGradeName.Focus();
                cValid = false;  
                e.Cancel = true;
                return;
            }

            errorProvider.SetError(txtGradeName, "");
        }

        private void frmuDCashIn_FormClosed(object sender, FormClosedEventArgs e) 
        {
            mDeleteProcessIdRecord();
        }

        private void btnCashIn_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select * from " + vMainTblNm + " order by " + "inv_no";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            
            VForText = "Select Cash In Number";
            vSearchCol = "INV_NO";
            vDisplayColumnList = "INV_NO:CashIn Number";
            vReturnCol = "INV_NO";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            
            if (oSelectPop.pReturnArray != null)
            {
                this.txtInvNo.Text = oSelectPop.pReturnArray[0];
                invno = this.txtInvNo.Text.Trim();
                SqlStr = "Select top 1 * from " + vMainTblNm + " Where Inv_no='" + invno + "' order by " + invno;
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                cntrcode = tDs.Tables[0].Rows[0]["CNTRCODE"].ToString().Trim();
                invno = tDs.Tables[0].Rows[0]["INV_NO"].ToString().Trim();
                cashin = tDs.Tables[0].Rows[0]["CashinAmt"].ToString().Trim();
                sysdate = tDs.Tables[0].Rows[0]["SYSDATE"].ToString().Trim();
                trnddt = tDs.Tables[0].Rows[0]["TRNDTTIME"].ToString().Trim();
            }
            //added by ruchit for 27503 on 01/03/2016
            else if (oSelectPop.pReturnArray == null)
            {
                //this.txtInvNo.Text = oSelectPop.pReturnArray[0];
                invno = this.txtInvNo.Text.Trim();
                //SqlStr = "Select top 1 * from " + vMainTblNm + " order by inv_no desc" ;
                
                if(String.IsNullOrEmpty(invno))
                {
                    return;
                }
                
                //else
                //{
                //    SqlStr = "Select top 1 * from " + vMainTblNm + " Where Inv_no=" + invno;
                    
                //}
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                cntrcode = tDs.Tables[0].Rows[0]["CNTRCODE"].ToString().Trim();
                invno = tDs.Tables[0].Rows[0]["INV_NO"].ToString().Trim();
                cashin = tDs.Tables[0].Rows[0]["CashinAmt"].ToString().Trim();
                sysdate = tDs.Tables[0].Rows[0]["SYSDATE"].ToString().Trim();
                trnddt = tDs.Tables[0].Rows[0]["TRNDTTIME"].ToString().Trim();
            }
            //added by ruchit for 27503 on 01/03/2016
            this.mthView();
            this.txtGradeName.Text = cntrcode;
            this.txtInvNo.Text = invno;
            this.txtCash.Text = cashin;
            this.lblmod.Text = sysdate;
            this.lbltran.Text = trnddt;
            this.mthChkNavigationButton();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblcurrent.Text = DateTime.Now.ToShortDateString()+ " " + DateTime.Now.ToLongTimeString();
        }       
         
    }
}
