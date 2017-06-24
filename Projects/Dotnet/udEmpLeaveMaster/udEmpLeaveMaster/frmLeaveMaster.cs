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

namespace udEmpLeaveMaster
{
    public partial class frmLeaveMaster : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
      
        string SqlStr = string.Empty;
        string vMainTblNm = "Emp_Leave_Year_Master", vMainField = "Lv_Year", vMainFldVal = "";  
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        bool cValid;
        bool vValid;
        public frmLeaveMaster(string[] args)
        {
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Leave Year Master";
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

        private void frmLeaveMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            this.dtpsDate.CustomFormat = "dd/MM/yyyy";
            this.dtpsDate.Format = DateTimePickerFormat.Custom;
            this.dtpeDate.CustomFormat = "dd/MM/yyyy";
            this.dtpeDate.Format = DateTimePickerFormat.Custom;


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.SetMenuRights();
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
                this.btnLeaveYear.Image = Image.FromFile(fName);

            }
            DateTime sdt = new DateTime(DateTime.Now.Year, 1, 1);  
            dtpsDate.Value = sdt;
            DateTime edt = new DateTime(DateTime.Now.Year, 12, 31);
            dtpeDate.Value = edt; 
            
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


        private void mthView()
        {
           
            this.mthBindClear();
            this.mthBindData();

        }

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpLeaveMaster.exe";
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
            this.txtLeaveYear.DataBindings.Clear();
            this.dtpsDate.DataBindings.Clear();
            this.dtpeDate.DataBindings.Clear();
            this.txtRemarks.DataBindings.Clear();
         
        }
        private void mthBindData()
        {
            if (dsMain.Tables.Count == 0)
            {
                return;
            }

            this.txtLeaveYear.DataBindings.Add("Text", dsMain.Tables[0], "Lv_Year");
            this.dtpsDate.DataBindings.Add("Text", dsMain.Tables[0], "sDate");
            this.dtpeDate.DataBindings.Add("Text", dsMain.Tables[0], "eDate");
            this.txtRemarks.DataBindings.Add("Text", dsMain.Tables[0], "Remarks");


        }
        private void mthEnableDisableFormControls()
        {


            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnLeaveYear.Enabled = false;

            }
            else
            {
                this.btnLeaveYear.Enabled = true;

            }


            if (this.pAddMode)
            {
                this.txtLeaveYear.Enabled = true;
            }
            else
            {
                this.txtLeaveYear.Enabled = false;
            }
            this.dtpsDate.Enabled = vEnabled;
            this.dtpeDate.Enabled = vEnabled;
            this.txtRemarks.Enabled = vEnabled;
           

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
            if (dsMain.Tables[0].Rows.Count == 0)
            {
                if (this.pAddMode == false && this.pEditMode == false) 
                {
                    this.btnCancel.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnLeaveYear.Enabled = false; 

                    vBtnAdd = true;
                    vBtnDelete = false;
                    vBtnEdit = false;
                    vBtnPrint = false;
                    this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                }
                return;
            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                SqlStr = "select id from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "'";

                dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    this.btnForward.Enabled = true;
                    this.btnLast.Enabled = true;
                }
                SqlStr = "select id from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "'";

                dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    this.btnBack.Enabled = true;
                    this.btnFirst.Enabled = true;
                }
            }

            vBtnAdd = false;
            vBtnDelete = false;
            vBtnEdit = false;
            vBtnPrint = false;

            if (this.btnForward.Enabled == true || this.btnBack.Enabled == true || (this.pAddMode == false && this.pEditMode == false))
            {
                vBtnAdd = true;
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    vBtnDelete = true;
                    vBtnEdit = true;
                    vBtnPrint = true;
                   this.btnLeaveYear.Enabled = true; 
                }
            }
            this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);

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
            if (vBtnPrint && this.pPrintButton)
            {
                //this.btnPreview.Enabled = true;
                //this.btnPrint.Enabled = true;
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
                //this.btnEmail.Enabled = true; Ramya
                //this.btnLocate.Enabled = true;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + "  order by " + vMainField;
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
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            string SqlStr = "select top 1  " + vMainField + " as Col1 from "+vMainTblNm+" order by  " + vMainField + " desc";
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


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();

    

            
            this.pAddMode = true;
            this.pEditMode = false;

            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            this.txtLeaveYear.Focus();

            DateTime sdt = new DateTime(DateTime.Now.Year, 1, 1);  
            dtpsDate.Value = sdt;
            DateTime edt = new DateTime(DateTime.Now.Year, 12, 31);
            dtpeDate.Value = edt;  
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

        }
        private void mthEdit()
        {

            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }

            this.pAddMode = false;
            this.pEditMode = true;
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();
            this.txtRemarks.Focus();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            cValid = true;  
            this.lblMand.Focus();

            if (this.dtpsDate.Value >= this.dtpeDate.Value)    
            {
               MessageBox.Show("The To Date Must Greater Than From Date ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               dtpeDate.Focus();
               cValid = false;  
               return;

            }
            if (cValid == false)
                return;
          
            this.mcheckCallingApplication();

            vValid = true;
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

            dsMain.Tables[0].Rows[0]["sDate"] = this.dtpsDate.Text;
            dsMain.Tables[0].Rows[0]["eDate"] = this.dtpeDate.Text;
            dsMain.Tables[0].Rows[0].AcceptChanges();
                  
            dsMain.Tables[0].Rows[0].EndEdit();

            

            this.mSaveCommandString(ref vSaveString, "#ID#");

            this.pAddMode = false; 
            this.pEditMode = false;
            this.mthEnableDisableFormControls(); 

            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);


            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();

        }
        private void mthChkSaveValidation(ref Boolean vValid)
        {

            if (string.IsNullOrEmpty(this.txtLeaveYear.Text.Trim()))
            {
                MessageBox.Show("Leave Year Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
            }
         
            this.txtLeaveYear.Focus();

          
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

                vSaveString = "Set DateFormat dmy  insert into " + vMainTblNm;
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
                vSaveString = vSaveString + vfldList + vWhereCondn;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
          
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
                    this.dtpeDate.Enabled = false;
                    this.dtpsDate.Enabled = false;
                    this.txtRemarks.Enabled = false;  
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

            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string vDelString = string.Empty;
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                vDelString = "Delete from " + vMainTblNm + "  Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
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
                        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
                        dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    }

                    else
                    {
                        DateTime sdt = new DateTime(DateTime.Now.Year, 1, 1);  
                        dtpsDate.Value = sdt;
                        DateTime edt = new DateTime(DateTime.Now.Year, 12, 31);
                        dtpeDate.Value = edt;  
                    }

                }
                this.mthView();
                this.mthChkNavigationButton();
            }

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);

        }

        private void txtLeaveYear_Validating(object sender, CancelEventArgs e)
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " select " + vMainField + " from " + vMainTblNm + " where " + vMainField + "='" + txtLeaveYear.Text.Trim() + "'";
            if (this.pEditMode)
            {
                sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            }
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                ToolTip t = new ToolTip();
                string ErrMsg = t.GetToolTip(txtLeaveYear).ToString();
                errorProvider.SetError(txtLeaveYear, "Duplicate " + lblLeaveYear.Text + " Value");
                txtLeaveYear.Focus();
                cValid = false;  
                e.Cancel = true;
                return;
            }

            errorProvider.SetError(txtLeaveYear, "");
        }

        private void btnLeaveYear_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication(); 
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Lv_Year,sDate,eDate,Remarks from " + vMainTblNm + " order by " + vMainField;
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Leave Year";
            vSearchCol = "Lv_Year";
            vDisplayColumnList = "Lv_Year:Leave Year";
            vReturnCol = "Lv_Year";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            vMainFldVal = dsMain.Tables[0].Rows[0]["Lv_Year"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtLeaveYear.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtLeaveYear.Text.Trim();
                SqlStr = "Select top 1 * from " + vMainTblNm + " Where Lv_Year='" + vMainFldVal + "' order by " + vMainField;
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }

            this.mthView();

            this.mthChkNavigationButton();
        }


        private void frmLeaveMaster_FormClosed(object sender, FormClosedEventArgs e)  /*Ramya*/
        {
            mDeleteProcessIdRecord();
        }

        private void txtLeaveYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789_/-";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '_' && (sender as TextBox).Text.IndexOf('_') > -1)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '/' && (sender as TextBox).Text.IndexOf('/') > -1)
            {
                e.Handled = true;
            } 


        }

     

    

    }
}
