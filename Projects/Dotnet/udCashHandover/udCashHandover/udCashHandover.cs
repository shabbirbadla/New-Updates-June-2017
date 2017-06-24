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
//using ueconnect;
using GetInfo;
using udReportList;
using Udyog.Library.Common;


namespace udCashHandover
{
    public partial class udCashHandov : uBaseForm.FrmBaseForm
    {

        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet DtPOSmain = new DataSet();
        string Cash = string.Empty;
        string Cashsum = string.Empty;
        string SqlStr = string.Empty;
        string vMainField = "Tran_cd", vMainTblNm = "POSCashOut", vMainFldVal = "";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        Boolean cValid = true;
        Boolean Iscancel = false;
        //clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        string GetDt = string.Empty;
        DataSet childTbl = new DataSet();
        System.Windows.Forms.Timer Clock = null;
        DataSet vDsCommon;
        string ReportListIcon = string.Empty;

        public udCashHandov(string[] args)
        {
            StartTimer();

            this.pDisableCloseBtn = true;  /* close disable  */
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Cash Out Entry";
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

            ReportListIcon = args[7].Replace("<*#*>", " ");
        }

        private void frmGradeMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.SetMenuRights();
            startupPath = Application.StartupPath;
            //startupPath = @"D:\VUDYOGSDK\";
            //oConnect = new clsConnect();

            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            //oConnect.InitProc("'" + startupPath + "'", appfile);
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
                //string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
                //ServiceType = objRegisterMe[15].ToString().Trim();
                UdyogRegister regiInfo = new UdyogRegister();
                ServiceType = regiInfo.RegistrationInfo.ServiceType;
            }
            this.btnLast_Click(sender, e);

            this.mInsertProcessIdRecord();
            this.SetFormColor();

            string appPath = Application.ExecutablePath;
            //startupPath = @"D:\VUDYOGSDK\";

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

            cAppName = "udCashHandover.exe";
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
                MessageBox.Show("Can't proceed, Main Application " + this.pPApplText + " is closed.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        private void mthBindClear()
        {
            int Val = 0;
            this.Txt1.DataBindings.Clear();
            this.Txt2.DataBindings.Clear();
            this.Txt5.DataBindings.Clear();
            this.Txt10.DataBindings.Clear();
            this.Txt20.DataBindings.Clear();
            this.Txt50.DataBindings.Clear();
            this.Txt100.DataBindings.Clear();
            this.Txt500.DataBindings.Clear();
            this.Txt1000.DataBindings.Clear();
            this.TxtTotHndOvr.DataBindings.Clear();
            this.TxtCntrID.DataBindings.Clear();
            this.TxtDate.DataBindings.Clear();
            this.TxtTranNo.DataBindings.Clear();
            this.TxtCashin.DataBindings.Clear();
            this.TxtCashBills.DataBindings.Clear();
            this.LblDtModi.DataBindings.Clear();
            this.LblTranDt.DataBindings.Clear();
            this.txtRoundOff.DataBindings.Clear();

            this.TxtBills.Text = Convert.ToInt32(Val).ToString();
            this.TxtGrossamt.Text = Convert.ToInt32(Val).ToString();
            this.TxtDiscamt.Text = Convert.ToInt32(Val).ToString();
            this.TxtTaxamt.Text = Convert.ToInt32(Val).ToString();
            this.TxtTotHndOvr.Text = Convert.ToInt32(Val).ToString();
            this.TxtCurrency.Text = Convert.ToInt32(Val).ToString();
            //this.TxtCashBills.Text = Convert.ToInt32(Val).ToString();
            //this.TxtCashinHand.Text = Convert.ToInt32(Val).ToString();
            //this.TxtDiff.Text = Convert.ToInt32(Val).ToString();

            this.TxtCashBills.Text = Convert.ToDecimal(Val).ToString();
            this.TxtCashinHand.Text = Convert.ToDecimal(Val).ToString();
            this.TxtDiff.Text = Convert.ToDecimal(Val).ToString();
            this.txtRoundOff.Text = Convert.ToDecimal(Val).ToString();

            this.TxtUser.DataBindings.Clear();

            this.TxtGrossamt.pDecimalLength = 2;
            this.TxtDiscamt.pDecimalLength = 2;
            this.TxtTaxamt.pDecimalLength = 2;
            this.TxtTotHndOvr.pDecimalLength = 2;
            this.TxtCashBills.pDecimalLength = 2;
            this.TxtCashinHand.pDecimalLength = 2;
            this.TxtDiff.pDecimalLength = 2;
            this.txtRoundOff.pDecimalLength = 2;

            this.btnGradeName.Enabled = false;
            this.btnPreview.Enabled = false;

            if (dsMain.Tables[0].Rows.Count > 0 && this.pAddMode == false && this.pEditMode == false)
            {
                this.btnPreview.Enabled = true;
                this.btnGradeName.Enabled = true;
                this.btnGradeName.Focus();
            }
            this.DtGridvw1.DataSource = "";
        }

        private void mthBindData()
        {
            if (dsMain.Tables.Count == 0)
            {
                return;
            }

            try
            {
                this.Txt1.DataBindings.Add("Text", dsMain.Tables[0], "Cash1x");
                this.Txt2.DataBindings.Add("Text", dsMain.Tables[0], "Cash2x");
                this.Txt5.DataBindings.Add("Text", dsMain.Tables[0], "Cash5x");
                this.Txt10.DataBindings.Add("Text", dsMain.Tables[0], "Cash10x");
                this.Txt20.DataBindings.Add("Text", dsMain.Tables[0], "Cash20x");
                this.Txt50.DataBindings.Add("Text", dsMain.Tables[0], "Cash50x");
                this.Txt100.DataBindings.Add("Text", dsMain.Tables[0], "Cash100x");
                this.Txt500.DataBindings.Add("Text", dsMain.Tables[0], "Cash500x");
                this.Txt1000.DataBindings.Add("Text", dsMain.Tables[0], "Cash1000x");
                this.TxtTotHndOvr.DataBindings.Add("Text", dsMain.Tables[0], "TotHndOvr");
                this.TxtCntrID.DataBindings.Add("Text", dsMain.Tables[0], "CntrCode");
                this.TxtTranNo.DataBindings.Add("Text", dsMain.Tables[0], "Inv_no");
                this.TxtCashin.DataBindings.Add("Text", dsMain.Tables[0], "CashInAmt");
                this.TxtCashBills.DataBindings.Add("Text", dsMain.Tables[0], "TotCashAmt");
                this.LblTranDt.DataBindings.Add("Text", dsMain.Tables[0], "TrnDtTime");
                this.LblDtModi.DataBindings.Add("Text", dsMain.Tables[0], "SysDate");
                this.TxtUser.DataBindings.Add("Text", dsMain.Tables[0], "UserName");
                this.txtRoundOff.DataBindings.Add("Text", dsMain.Tables[0], "RoundOff");
            }
            catch (Exception ex)
            {
            }
            this.Txt1.MaxLength = 10;
            this.Txt2.MaxLength = 10;
            this.Txt5.MaxLength = 10;
            this.Txt10.MaxLength = 10;
            this.Txt20.MaxLength = 10;
            this.Txt50.MaxLength = 10;
            this.Txt100.MaxLength = 10;
            this.Txt500.MaxLength = 10;
            this.Txt1000.MaxLength = 10;
            this.txtRoundOff.MaxLength = 10;

            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0]["Date"].ToString()))
                {
                    this.TxtUser.Text = this.pAppUerName;
                    this.TxtDate.Text = Convert.ToDateTime(DtPOSmain.Tables["SQLDATE"].Rows[0][0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    string Date = Convert.ToDateTime(dsMain.Tables[0].Rows[0]["Date"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    this.TxtDate.Text = Date;
                    this.label28.Visible = true;
                    this.LblDtModi.Visible = true;
                    this.LblTranDt.Visible = true;
                    this.label31.Visible = true;
                }
            }
            else
            {
                this.label28.Visible = false;
                this.LblDtModi.Visible = false;
                this.LblTranDt.Visible = false;
                this.label31.Visible = false;

                this.TxtUser.Text = this.pAppUerName;
                this.TxtDate.Text = Convert.ToDateTime(DtPOSmain.Tables["SQLDATE"].Rows[0][0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            AssignSourceToGrid();
        }

        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
            }
            else
            {

            }
            act_deact();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + "  order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            GetChildRecords();
            this.mthView();
            this.mthChkNavigationButton();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();

            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            GetChildRecords();
            this.mthView();
            this.mthChkNavigationButton();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            GetChildRecords();
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
                //this.btnPreview.Enabled = false;
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

            GetChildRecords();
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
            this.btnPreview.Enabled = false;
            this.pAddMode = true;
            this.pEditMode = false;
            //GetData();

            this.mcheckCallingApplication();

            Boolean vValid = true;
            DataTable Cashin = new DataTable();
            string strSQL = "";
            strSQL = strSQL + "SET DATEFORMAT DMY SELECT TRAN_CD,USERNAME,[DATE],CASHINAMT,CNTRID,CNTRCODE FROM POSCASHIN Where USERNAME = '" + this.pAppUerName + "' AND CONVERT(VARCHAR,[DATE],103) <= ";
            strSQL = strSQL + " Getdate() AND isnull(CashOTran,0) = 0 ";
            Cashin = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];

            if (Cashin.Rows.Count <= 0)
            {
                MessageBox.Show("There is not any open cash in entry for this user.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
                this.pAddMode = false;
            }
            else
            {
                if (Cashin.Rows.Count == 1)
                {
                    if (dsMain.Tables[0].Rows.Count <= 0)
                    {
                        DataRow drCurrent;
                        drCurrent = dsMain.Tables[0].NewRow();
                        dsMain.Tables[0].Rows.Add(drCurrent);
                    }
                    dsMain.Tables[0].Rows[0]["CntrCode"] = Cashin.Rows[0]["CNTRCODE"].ToString();
                    GetData();
                }
            }
            if (vValid == false)
            {
                this.btnPreview.Enabled = true;
                return;
            }

            //this.mcheckCallingApplication();
            this.mthNew(sender, e);
            this.mthChkNavigationButton();
            AssignSourceToGrid();
            this.btnCounterCd.Focus();
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
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthEdit();
            AssignSourceToGrid();
        }

        private void mthEdit()
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }

            SqlStr = "select TOP 1 INV_NO,username FROM POSCASHOUT WHERE Inv_no='" + this.TxtTranNo.Text + "'";
            DataSet tDs = new DataSet();
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (tDs.Tables[0].Rows.Count > 0)
            {
                if (tDs.Tables[0].Rows[0]["USERNAME"].ToString().ToUpper().Trim() != this.pAppUerName.ToUpper())
                {
                    SqlStr = "You are not authorized to edit this entry";
                    MessageBox.Show(SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            this.pAddMode = false;
            this.pEditMode = true;
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.LblDtModi.Focus();
            cValid = true;
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

            Dsmain();
            dsMain.Tables[0].Rows[0].AcceptChanges();
            dsMain.Tables[0].Rows[0].EndEdit();

            this.mSaveCommandString(ref vSaveString, "#ID#");

            string Result = string.Empty;
            if (this.pAddMode == true)
                Result = "Save";
            else if (this.pEditMode == true)
                Result = "Edit";

            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();

            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);

            this.SaveToPOSREF(Result);

            if (dsMain.Tables[0].Rows[0][vMainField].ToString() == "")
            {
                SqlStr = "Select top 1 * from " + vMainTblNm + " order by tran_cd desc";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            }
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            //            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "=" + vMainFldVal;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
        }

        private void mthChkSaveValidation(ref Boolean vValid)
        {
            if (Convert.ToDecimal(this.TxtDiff.Text) != 0)
            {
                MessageBox.Show("There is amount difference, Can not save the changes !!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
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
            {
                vSaveString = "Set DateFormat dmy Update " + vMainTblNm + "  Set ";
                string vWhereCondn = string.Empty;
                foreach (DataColumn dtc1 in dsMain.Tables[0].Columns)
                {
                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        {
                            vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                            if (dtc1.DataType.Name.ToLower() == "string")
                            {
                                vfldVal = "'" + vfldVal + "'";
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
                        }
                        if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1))
                        {
                            if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                            vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                            vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                            if (dtc1.DataType.Name.ToLower() == "string")
                            {
                                vfldVal = "'" + vfldVal + "'";
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
                            vWhereCondn = vWhereCondn + vfldVal;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                            vfldList = vfldList + dtc1.ToString().Trim() + " = ";
                            vfldList = vfldList + vfldVal;
                        }
                    }
                }
                vWhereCondn = " Where Tran_cd =" + dsMain.Tables[0].Rows[0]["Tran_cd"].ToString();
                vSaveString = vSaveString + vfldList + vWhereCondn;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Iscancel = true;
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
            this.AssignSourceToGrid();
            this.act_deact();
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
            this.pAddMode = false;
            this.pEditMode = false;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthDelete();
        }

        private void mthDelete()
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            SqlStr = "select TOP 1 INV_NO,username FROM POSCASHOUT WHERE Inv_no='" + this.TxtTranNo.Text + "'";
            DataSet tDs = new DataSet();
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (tDs.Tables[0].Rows.Count > 0)
            {
                if (tDs.Tables[0].Rows[0]["USERNAME"].ToString().ToUpper().Trim() != this.pAppUerName.ToUpper())
                {
                    SqlStr = "You are not authorized to delete this entry";
                    MessageBox.Show(SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string Result = "Delete";
                this.SaveToPOSREF(Result);
                string vDelString = string.Empty;
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                vDelString = "Delete from " + vMainTblNm + "  Where Tran_cd=" + dsMain.Tables[0].Rows[0]["tran_cd"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();

                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
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

        private void mthView()
        {
            if (GetDt == "")
            {
                this.GetData();
                GetDt = "Data";
            }
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
            //this.btnPreview.Enabled = false;
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
                //this.btnDelete.Enabled = true;
                this.btnDelete.Enabled = false;
            }
            if (vBtnPrint && this.pPrintButton)
            {
                this.btnPrint.Enabled = true;
            }

            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnLogout.Enabled = false;
            this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;
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
            SqlStr = "Select convert(varchar,tran_cd) as tran_cd,Inv_No from " + vMainTblNm + " order by " + vMainField;
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Invoice No.";
            vSearchCol = "Inv_NO";
            vDisplayColumnList = "Inv_no:Invoice No.";
            vReturnCol = "Tran_cd";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            vMainFldVal = dsMain.Tables[0].Rows[0]["Tran_cd"].ToString().Trim();

            if (oSelectPop.pReturnArray != null)
            {
                vMainFldVal = oSelectPop.pReturnArray[0];
                SqlStr = "Select top 1 * from " + vMainTblNm + " Where Tran_cd=" + vMainFldVal + " order by " + vMainField;
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
                this.TxtTranNo.Text = dsMain.Tables[0].Rows[0]["Inv_no"].ToString();
            }

            GetChildRecords();
            this.mthView();
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
            if (this.btnLogout.Enabled)
                btnExit_Click(this.btnExit, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Txt2_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt5_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt10_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt20_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt50_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt100_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt500_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Txt1000_TextChanged(object sender, EventArgs e)
        {
            Calculation();
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
            sqlstr = " select " + vMainField + " from " + vMainTblNm + " where " + vMainField + "='" /*+ txtGradeName.Text.Trim() + "'"*/;
            if (this.pEditMode)
            {
                sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            }
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                ToolTip t = new ToolTip();
                cValid = false;
                e.Cancel = true;
                return;
            }

        }

        private void frmGradeMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void act_deact()
        {
            if (this.pAddMode || this.pEditMode)
            {
                this.Txt1.Enabled = true;
                this.Txt2.Enabled = true;
                this.Txt5.Enabled = true;
                this.Txt10.Enabled = true;
                this.Txt20.Enabled = true;
                this.Txt50.Enabled = true;
                this.Txt100.Enabled = true;
                this.Txt500.Enabled = true;
                this.Txt1000.Enabled = true;
                this.txtRoundOff.Enabled = true;
                this.btnCounterCd.Enabled = true;
                this.btnGradeName.Enabled = false;
            }
            else
            {
                this.Txt1.Enabled = false;
                this.Txt2.Enabled = false;
                this.Txt5.Enabled = false;
                this.Txt10.Enabled = false;
                this.Txt20.Enabled = false;
                this.Txt50.Enabled = false;
                this.Txt100.Enabled = false;
                this.Txt500.Enabled = false;
                this.Txt1000.Enabled = false;

                this.Txttot1.Enabled = false;
                this.Txttot2.Enabled = false;
                this.Txttot5.Enabled = false;
                this.Txttot10.Enabled = false;
                this.Txttot20.Enabled = false;
                this.Txttot50.Enabled = false;
                this.Txttot100.Enabled = false;
                this.Txttot500.Enabled = false;
                this.Txttot1000.Enabled = false;

                this.DtGridvw1.Enabled = false;

                this.TxtBills.Enabled = false;
                this.TxtGrossamt.Enabled = false;
                this.TxtDiscamt.Enabled = false;
                this.TxtTaxamt.Enabled = false;
                this.TxtTotHndOvr.Enabled = false;
                this.TxtCashinHand.Enabled = false;
                this.TxtCashBills.Enabled = false;
                this.TxtDiff.Enabled = false;
                this.TxtTotHndOvr.Enabled = false;
                this.TxtTotHndOvr.Enabled = false;
                this.TxtUser.Enabled = false;
                this.TxtTranNo.Enabled = false;
                this.TxtDate.Enabled = false;
                this.TxtCntrID.Enabled = false;
                this.TxtCashin.Enabled = false;
                this.TxtCurrency.Enabled = false;
                this.txtRoundOff.Enabled = false;
                this.btnCounterCd.Enabled = false;
                this.btnGradeName.Enabled = true;
            }
        }

        private void Calculation()
        {
            this.Txttot1.Text = (Convert.ToInt32(this.Txt1.Text) * 1).ToString();
            this.Txttot2.Text = (Convert.ToInt32(this.Txt2.Text) * 2).ToString();
            this.Txttot5.Text = (Convert.ToInt32(this.Txt5.Text) * 5).ToString();
            this.Txttot10.Text = (Convert.ToInt32(this.Txt10.Text) * 10).ToString();
            this.Txttot20.Text = (Convert.ToInt32(this.Txt20.Text) * 20).ToString();
            this.Txttot50.Text = (Convert.ToInt32(this.Txt50.Text) * 50).ToString();
            this.Txttot100.Text = (Convert.ToInt32(this.Txt100.Text) * 100).ToString();
            this.Txttot500.Text = (Convert.ToInt32(this.Txt500.Text) * 500).ToString();
            this.Txttot1000.Text = (Convert.ToInt32(this.Txt1000.Text) * 1000).ToString();

            this.TxtCurrency.Text = (Convert.ToInt32(this.Txttot1.Text) + Convert.ToInt32(this.Txttot2.Text) + Convert.ToInt32(this.Txttot5.Text) + Convert.ToInt32(this.Txttot10.Text) +
                      Convert.ToInt32(this.Txttot20.Text) + Convert.ToInt32(this.Txttot50.Text) + Convert.ToInt32(this.Txttot100.Text) +
                      Convert.ToInt32(this.Txttot500.Text) + Convert.ToInt32(this.Txttot1000.Text)).ToString();

            this.TxtCashinHand.Text = (Convert.ToDecimal(this.TxtCashBills.Text) + Convert.ToDecimal(this.TxtCashin.Text)).ToString();
            //this.TxtDiff.Text = (Convert.ToInt32(this.TxtCashinHand.Text) - Convert.ToInt32(this.TxtCashBills.Text)).ToString();
            //this.TxtDiff.Text = (Convert.ToInt32(this.TxtDiff.Text) - Convert.ToInt32(this.TxtCurrency.Text)).ToString();
            this.TxtDiff.Text = (Convert.ToDecimal(this.TxtCashinHand.Text) - Convert.ToInt32(this.TxtCurrency.Text) - Convert.ToDecimal(this.txtRoundOff.Text)).ToString();
        }


        private void GetData()
        {

            DataTable SysData = new DataTable();
            string strSQL = "";
            strSQL = strSQL + "Select top 1 getdate() as SQLdate,sta_dt,end_dt from Vudyog..CO_MAST where compid =" + this.pCompId;
            SysData = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];
            if (this.pAddMode == true)
            {
                int Mode = 0;
                int Tran_cd = 0;
                strSQL = "";
                strSQL = strSQL + "Execute Usp_Ent_PosCashout '" + this.pAppUerName + "','" + Convert.ToDateTime(SysData.Rows[0]["SQLdate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    + "'," + this.pCompId + "," + Tran_cd + "," + Mode + ",'" + this.TxtCntrID.Text + "'";

                DtPOSmain = oDataAccess.GetDataSet(strSQL, null, 20);

                DtPOSmain.Tables[0].TableName = "PSPAYDETAIL";
                DtPOSmain.Tables[1].TableName = "DCMAIN";
                //DtPOSmain.Tables[2].TableName = "POSREF";

                DataTable SysData1 = SysData.Copy();
                DtPOSmain.Tables.Add(SysData1);
                DtPOSmain.Tables[2].TableName = "SQLDATE";

                DataTable Cashin = new DataTable();
                strSQL = "";
                strSQL = strSQL + "SET DATEFORMAT DMY SELECT TRAN_CD,USERNAME,[DATE],CASHINAMT,CNTRID,CNTRCODE FROM POSCASHIN Where USERNAME ='" + this.pAppUerName + "' AND [DATE] <= ";
                strSQL = strSQL + " getdate() AND isnull(CashOTran,0) =0 ";
                //strSQL = strSQL + Convert.ToDateTime(DtPOSmain.Tables["SQLDATE"].Rows[0][0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "' AND isnull(CashOTran,0) =0 ";
                strSQL = strSQL + " AND CNTRCODE = '" + this.TxtCntrID.Text.Trim() + "'";
                Cashin = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];

                DataTable Cashin1 = Cashin.Copy();
                DtPOSmain.Tables.Add(Cashin1);
                DtPOSmain.Tables[3].TableName = "Cashin";

                dsMain.Tables[0].Rows[0]["Entry_Ty"] = "CO";
                dsMain.Tables[0].Rows[0]["Date"] = this.TxtDate.Text;
                dsMain.Tables[0].Rows[0]["UserName"] = this.pAppUerName;
                dsMain.Tables[0].Rows[0]["SysDate"] = DtPOSmain.Tables["SQLDATE"].Rows[0]["SQLdate"];
                dsMain.Tables[0].Rows[0]["TrnDtTime"] = DtPOSmain.Tables["SQLDATE"].Rows[0]["SQLdate"];
                dsMain.Tables[0].Rows[0]["CompID"] = this.pCompId;
                dsMain.Tables[0].Rows[0]["CashInAmt"] = DtPOSmain.Tables["CashIn"].Rows[0]["Cashinamt"].ToString();
                dsMain.Tables[0].Rows[0]["CashInTran"] = DtPOSmain.Tables["CashIn"].Rows[0]["Tran_cd"].ToString();
                dsMain.Tables[0].Rows[0]["CounterID"] = DtPOSmain.Tables["CashIn"].Rows[0]["CNTRID"].ToString();
                dsMain.Tables[0].Rows[0]["CntrCode"] = DtPOSmain.Tables["CashIn"].Rows[0]["CntrCode"].ToString();
                dsMain.Tables[0].Rows[0]["TotHndOvr"] = this.TxtTotHndOvr.Text;
                dsMain.Tables[0].Rows[0]["RoundOff"] = this.txtRoundOff.Text;
            }
            else
            {
                DataTable SysData1 = SysData.Copy();
                DtPOSmain.Tables.Add(SysData1);
                DtPOSmain.Tables[0].TableName = "SQLDATE";
            }

        }

        private void GetChildRecords()
        {
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                int Mode = 1;
                string strSQL = string.Empty;
                strSQL = "";
                //strSQL = strSQL + "SELECT PR.*,PCO.CounterID FROM POSCashOutREF PR INNER JOIN POSCashOut PCO ON PCO.Tran_cd = PR.CashOTran WHERE PCO.Tran_cd =";
                //strSQL = strSQL + dsMain.Tables[0].Rows[0]["Tran_cd"].ToString() + " and  PR.USERNAME ='" + this.pAppUerName + "' ";

                strSQL = strSQL + " Execute Usp_Ent_PosCashout '" + this.pAppUerName + "',''," + this.pCompId + "," + dsMain.Tables[0].Rows[0]["Tran_cd"].ToString() + "," + Mode + ",''";

                childTbl = oDataAccess.GetDataSet(strSQL, null, 20);

                this.DtGridvw1.RefreshEdit();
                this.DtGridvw1.Refresh();
                //childTbl.Tables[0].TableName = "POSREF1";
                childTbl.Tables[0].TableName = "PSPAYDETAIL";
                childTbl.Tables[1].TableName = "DCMAIN";
            }
        }

        private void AssignSourceToGrid()
        {
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (pAddMode == true)
                {
                    DataTable InvNo = new DataTable();
                    string SQlStr = "";
                    SQlStr = "Select TOP 1 INV_NO FROM POSCASHOUT ORDER BY INV_NO DESC ";
                    InvNo = oDataAccess.GetDataSet(SQlStr, null, 20).Tables[0];

                    if (InvNo.Rows.Count <= 0)
                        InvNo.Rows.Add();

                    if (string.IsNullOrEmpty(InvNo.Rows[0]["Inv_No"].ToString()))
                    {
                        dsMain.Tables[0].Rows[0]["Inv_No"] = "0";
                    }
                    else
                    {
                        dsMain.Tables[0].Rows[0]["Inv_No"] = InvNo.Rows[0][0].ToString();
                    }
                    string x = (Convert.ToInt32(dsMain.Tables[0].Rows[0]["Inv_No"]) + 1).ToString();
                    this.TxtTranNo.Text = x.PadLeft(6, '0');

                    DtGridvw1.AutoGenerateColumns = false;
                    DtGridvw1.DataSource = DtPOSmain.Tables["PSPAYDETAIL"];
                    DtGridvw1.Columns[0].DataPropertyName = "PayMode";
                    DtGridvw1.Columns[1].DataPropertyName = "TotalValue";
                }
                else
                {
                    GetChildRecords();
                    DtGridvw1.AutoGenerateColumns = false;
                    //DtGridvw1.DataSource = childTbl.Tables["POSREF1"];
                    DtGridvw1.DataSource = childTbl.Tables["PSPAYDETAIL"];
                    DtGridvw1.Columns[0].DataPropertyName = "PayMode";
                    DtGridvw1.Columns[1].DataPropertyName = "TotalValue";

                    if (childTbl.Tables["DCMAIN"].Rows.Count > 0)
                    {
                        this.TxtBills.Text = Convert.ToInt32(childTbl.Tables["DCMAIN"].Rows[0]["Bills"]).ToString();
                        //this.TxtGrossamt.Text = Convert.ToInt32(childTbl.Tables["DCMAIN"].Rows[0]["GrosAmount"]).ToString();
                        //this.TxtDiscamt.Text = Convert.ToInt32(childTbl.Tables["DCMAIN"].Rows[0]["DiscountAmt"]).ToString();
                        //this.TxtTaxamt.Text = Convert.ToInt32(childTbl.Tables["DCMAIN"].Rows[0]["TaxAmount"]).ToString();
                        this.TxtGrossamt.Text = childTbl.Tables["DCMAIN"].Rows[0]["GrosAmount"].ToString();
                        this.TxtDiscamt.Text = childTbl.Tables["DCMAIN"].Rows[0]["DiscountAmt"].ToString();
                        this.TxtTaxamt.Text = childTbl.Tables["DCMAIN"].Rows[0]["TaxAmount"].ToString();
                    }
                }
                this.DataBind();
                this.Calculation();
            }
        }

        private void SaveToPOSREF(string result)
        {
            string SaveString = string.Empty;
            DataTable Trancd = new DataTable();
            string strSQL = "";
            strSQL = "SELECT TOP 1 Tran_cd FROM POSCASHOUT ORDER BY TRAN_CD DESC";
            Trancd = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];

            if (result == "Save")
            {
                SaveString = "";
                SaveString = "Set DateFormat dmy UPDATE DCMAIN SET POSOUTTRAN = " + Convert.ToInt16(Trancd.Rows[0][0].ToString());
                SaveString = SaveString + " WHERE ENTRY_TY='PS' AND [USER_NAME] = '" + this.pAppUerName + "' and isnull(POSOUTTRAN,0) =0 AND CNTRCODE='" + dsMain.Tables[0].Rows[0]["CNTRCODE"].ToString() + "'";

                SaveString = SaveString + " ;Set DateFormat dmy UPDATE POSCASHIN SET CashOTran = " + Convert.ToInt16(Trancd.Rows[0][0].ToString());
                SaveString = SaveString + " Where Cntrid = " + dsMain.Tables[0].Rows[0]["CounterID"].ToString() + " And tran_cd = " + dsMain.Tables[0].Rows[0]["CashInTran"].ToString() + " and UserName ='" + this.pAppUerName + "' and isnull(CashOTran,0) =0";
                oDataAccess.ExecuteSQLStatement(SaveString, null, 20, true);
            }
            if (result == "Delete")
            {
                SaveString = "";

                SaveString = SaveString + " Set DateFormat dmy UPDATE DCMAIN SET POSOUTTRAN = 0 WHERE ENTRY_TY='PS' AND [USER_NAME] = '" + this.pAppUerName + "' and isnull(POSOUTTRAN,0) =" + Convert.ToInt16(Trancd.Rows[0][0].ToString());

                SaveString = SaveString + " ;Set DateFormat dmy UPDATE POSCASHIN SET CashOTran = 0";
                SaveString = SaveString + " Where Cntrid = " + dsMain.Tables[0].Rows[0]["CounterID"] + " And tran_cd = " + dsMain.Tables[0].Rows[0]["CashInTran"] + " and UserName ='" + this.pAppUerName + "' and CashOTran=" + dsMain.Tables[0].Rows[0]["Tran_cd"];
                oDataAccess.ExecuteSQLStatement(SaveString, null, 20, true);
            }
            if (result == "Edit")
            {
            }
        }

        private void Dsmain()
        {
            if (this.pAddMode == true)
            {
                string FinYear = DtPOSmain.Tables["SQLDATE"].Rows[0]["Sta_Dt"].ToString() + DtPOSmain.Tables["SQLDATE"].Rows[0]["End_Dt"].ToString();
                if (DtPOSmain.Tables["PSPAYDETAIL"].Rows.Count > 0)
                {
                    foreach (DataRow PAYDET in DtPOSmain.Tables["PSPAYDETAIL"].Rows)
                    {
                        if (PAYDET["PayMode"].ToString() == "CASH")
                        {
                            dsMain.Tables[0].Rows[0]["TotCashAmt"] = PAYDET["TotalValue"].ToString();
                        }

                        if (PAYDET["PayMode"].ToString() == "COUPON")
                        {
                            dsMain.Tables[0].Rows[0]["TotCopnAmt"] = PAYDET["TotalValue"].ToString();
                        }

                        if (PAYDET["PayMode"].ToString() == "CARD")
                        {
                            dsMain.Tables[0].Rows[0]["TotCardAmt"] = PAYDET["TotalValue"].ToString();
                        }

                        if (PAYDET["PayMode"].ToString() == "CHEQUE")
                        {
                            dsMain.Tables[0].Rows[0]["TotCheqAmt"] = PAYDET["TotalValue"].ToString();
                        }
                    }
                }

                dsMain.Tables[0].Rows[0]["Entry_Ty"] = "CO";
                dsMain.Tables[0].Rows[0]["Date"] = this.TxtDate.Text;
                dsMain.Tables[0].Rows[0]["UserName"] = this.pAppUerName;
                dsMain.Tables[0].Rows[0]["L_yn"] = FinYear.Substring(6, 4) + "-" + FinYear.Substring(28, 4);
                dsMain.Tables[0].Rows[0]["SysDate"] = DtPOSmain.Tables["SQLDATE"].Rows[0]["SQLdate"];
                dsMain.Tables[0].Rows[0]["TrnDtTime"] = DtPOSmain.Tables["SQLDATE"].Rows[0]["SQLdate"];
                dsMain.Tables[0].Rows[0]["CompID"] = this.pCompId;
                dsMain.Tables[0].Rows[0]["CashInAmt"] = DtPOSmain.Tables["CashIn"].Rows[0]["Cashinamt"].ToString();
                dsMain.Tables[0].Rows[0]["CashInTran"] = DtPOSmain.Tables["CashIn"].Rows[0]["Tran_cd"].ToString();
                dsMain.Tables[0].Rows[0]["CounterID"] = DtPOSmain.Tables["CashIn"].Rows[0]["CNTRID"].ToString();
                dsMain.Tables[0].Rows[0]["CntrCode"] = DtPOSmain.Tables["CashIn"].Rows[0]["CntrCode"].ToString();
                dsMain.Tables[0].Rows[0]["TotHndOvr"] = this.TxtTotHndOvr.Text;
                dsMain.Tables[0].Rows[0]["RoundOff"] = this.txtRoundOff.Text;

                DataTable InvNo = new DataTable();
                string SQlStr = "";
                SQlStr = "Select TOP 1 INV_NO FROM POSCASHOUT ORDER BY INV_NO DESC ";
                InvNo = oDataAccess.GetDataSet(SQlStr, null, 20).Tables[0];
                if (InvNo.Rows.Count > 0)
                {
                    string _InvNo = (Convert.ToInt16(InvNo.Rows[0][0]) + 1).ToString();
                    dsMain.Tables[0].Rows[0]["Inv_no"] = _InvNo.PadLeft(6, '0');
                }
                else
                    dsMain.Tables[0].Rows[0]["Inv_no"] = this.TxtTranNo.Text;
            }
            else
            {
                DataTable SysDate = new DataTable();
                string strSQL = "Select top 1 getdate() as SysDate";
                SysDate = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];
                dsMain.Tables[0].Rows[0]["SysDate"] = SysDate.Rows[0][0];
            }
        }

        private void Txt1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void Txt1_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.mthPrint(2);
        }

        private void mthPrint(Int16 vPrintOption)
        {
            mthDsCommon();

            string vRepGroup = "Cash Out";
            udReportList.cReportList oPrint = new udReportList.cReportList();
            oPrint.pDsCommon = this.vDsCommon;
            oPrint.pServerName = this.pServerName;
            oPrint.pComDbnm = this.pComDbnm;
            oPrint.pUserId = this.pUserId;
            oPrint.pPassword = this.pPassword;
            oPrint.pAppPath = this.pAppPath;
            oPrint.pPApplText = this.pPApplText;
            oPrint.pPara = this.pPara;
            oPrint.pRepGroup = vRepGroup;
            //oPrint.pTran_Cd = Convert.ToInt16(dsMain.Tables[0].Rows[0]["Tran_Cd"].ToString());
            oPrint.pSpPara = "'" + this.TxtUser.Text + "','" + this.TxtDate.Text + "'," + this.pCompId + ",'" + dsMain.Tables[0].Rows[0]["Tran_cd"].ToString() + "'";
            oPrint.pPrintOption = vPrintOption;
            oPrint.pSpIcon = ReportListIcon;
            oPrint.pSpFrmCaption = "Daily Cash Handover Report ";
            oPrint.Main();
        }

        private void mthDsCommon()
        {
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";

            SqlStr = "Select * From vudyog..Co_Mast where CompId=" + pPara[0];
            company = oDataAccess.GetDataTable(SqlStr, null, 20);

            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            SqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, 20);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.mthPrint(3);
        }

        private void DataBind()
        {
            if (this.pAddMode == true)
            {
                if (DtPOSmain.Tables["PSPAYDETAIL"] != null)
                {
                    foreach (DataRow PSPAY in DtPOSmain.Tables["PSPAYDETAIL"].Rows)
                    {
                        if (PSPAY["Paymode"].ToString() == "CASH")
                        {
                            Cash = PSPAY["Totalvalue"].ToString();
                        }
                    }
                }

                if (DtPOSmain.Tables["DCMAIN"] != null)
                {
                    if (DtPOSmain.Tables["DCMAIN"].Rows.Count > 0)
                    {
                        this.TxtBills.Text = DtPOSmain.Tables["DCMAIN"].Rows[0]["Bills"].ToString();
                        this.TxtGrossamt.Text = DtPOSmain.Tables["DCMAIN"].Rows[0]["GrosAmount"].ToString();
                        this.TxtDiscamt.Text = DtPOSmain.Tables["DCMAIN"].Rows[0]["DiscountAmt"].ToString();
                        this.TxtTaxamt.Text = DtPOSmain.Tables["DCMAIN"].Rows[0]["TaxAmount"].ToString();
                        this.TxtTotHndOvr.Text = DtPOSmain.Tables["DCMAIN"].Rows[0]["NetAmount"].ToString();
                    }
                }

                if (DtPOSmain.Tables["Cashin"] != null)
                {
                    if (DtPOSmain.Tables["Cashin"].Rows.Count > 0)
                    {
                        if (DtPOSmain.Tables["DCMAIN"].Rows.Count > 0)
                        {
                            this.TxtCashBills.Text = Cash.ToString();
                        }
                        this.TxtCntrID.Text = DtPOSmain.Tables["Cashin"].Rows[0]["CNTRCODE"].ToString();
                        this.TxtCashin.Text = DtPOSmain.Tables["CashIn"].Rows[0]["Cashinamt"].ToString();
                        this.TxtUser.Text = this.pAppUerName;
                    }
                }
            }
        }

        private void StartTimer()
        {
            Clock = new System.Windows.Forms.Timer();
            Clock.Interval = 1000;
            Clock.Tick += new EventHandler(Clock_Tick);
            Clock.Enabled = true;
        }

        void Clock_Tick(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString();
        }

        private void btnCounterCd_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            strSQL = strSQL + "SELECT distinct CNTRID,CNTRCODE FROM POSCASHIN Where USERNAME ='" + this.pAppUerName + "' AND CONVERT(VARCHAR,[DATE],103) <= ";
            strSQL = strSQL + " Getdate() AND isnull(CashOTran,0) =0 ";
            tDs = oDataAccess.GetDataSet(strSQL, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Counter";
            vSearchCol = "CNTRCODE";
            vDisplayColumnList = "CNTRCODE:Counter Code";
            vReturnCol = "CNTRID,CNTRCODE";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                if (this.TxtCntrID.Text.Trim() != oSelectPop.pReturnArray[1].ToString())
                {
                    if (this.TxtCntrID.Text.Trim() != "")
                    {
                        if (MessageBox.Show("This will change the data entered. Do you still want to Continue?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.TxtCntrID.Text = oSelectPop.pReturnArray[1].ToString();
                            dsMain.Tables[0].Rows[0]["CntrCode"] = oSelectPop.pReturnArray[1].ToString();
                            dsMain.Tables[0].Rows[0]["CounterID"] = oSelectPop.pReturnArray[0].ToString();
                            GetData();
                            this.mthBindClear();
                            this.mthBindData();
                            this.AssignSourceToGrid();
                        }
                    }
                    else
                    {
                        this.TxtCntrID.Text = oSelectPop.pReturnArray[1].ToString();
                        dsMain.Tables[0].Rows[0]["CntrCode"] = oSelectPop.pReturnArray[1].ToString();
                        dsMain.Tables[0].Rows[0]["CounterID"] = oSelectPop.pReturnArray[0].ToString();

                        GetData();
                        this.mthBindClear();
                        this.mthBindData();
                        this.AssignSourceToGrid();
                    }
                }
            }
        }

        private void txtRoundOff_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }
    }
}