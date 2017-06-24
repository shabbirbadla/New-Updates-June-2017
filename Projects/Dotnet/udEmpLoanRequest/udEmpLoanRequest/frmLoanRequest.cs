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
using uNumericTextBox;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpLoanRequest
{
    public partial class frmLoanRequest : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet vDsCommon;
        String SqlStr;
        String cAppPId, cAppName;
        string vMainTblNm = "Emp_Loan_Advance_Request", vMainField = "Inv_No", vMainFldVal = "";
        Boolean cValid;
        string vInv_No = "", vDocNo = "";
        string vL_Yn = string.Empty, vGen_Inv="LA";
        int iInv_No = 0, iDoc_No = 0, vTran_Cd = 0, vInvNoLen = 0;
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmLoanRequest(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Loan and Advance Request";
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

       

        private void frmLoanApplication_Load(object sender, EventArgs e)
        {
            string fName;
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.txtAmount.pAllowNegative = false;
            this.txtAmount.MaxLength = 12;
            this.txtAmount.pDecimalLength = 2;

            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            this.DtpAppDt.CustomFormat = "dd/MM/yyyy";
            this.DtpAppDt.Format = DateTimePickerFormat.Custom;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.SetMenuRights();
            this.mInsertProcessIdRecord();
            this.SetFormColor();
            this.mthDsCommon();

            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
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
            this.btnLast_Click(sender, e);

             fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnAppNo.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnEmpNm.Image = Image.FromFile(fName);
                this.btnPayHead.Image = Image.FromFile(fName);
            }

            if (vL_Yn == "")
            {
                SqlStr = "Select Sta_Dt,End_Dt From Vudyog..Co_Mast Where CompId=" + this.pCompId.ToString();
                DataRow drYear = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
                vL_Yn = Convert.ToDateTime(drYear["Sta_Dt"]).Year.ToString().Trim() + "-" + Convert.ToDateTime(drYear["End_Dt"]).Year.ToString().Trim();
            }

        }
# region Navigation Buttons
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
            if (ServiceType.ToUpper() != "VIEWER VERSION")//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (dsMain.Tables[0].Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false)
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;
                        //this.btnHeadNm.Enabled = false; 
                        //this.btnFldNm.Enabled = false;  

                        vBtnAdd = true;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                    }
                    return;
                }
            }//Added by Archana K. on 17/05/13 for Bug-7899
            if (dsMain.Tables[0].Rows.Count > 0)//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                    SqlStr = "select id from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = "select id from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnBack.Enabled = true;
                        this.btnFirst.Enabled = true;


                    }
                }
            }//Added by Archana K. on 17/05/13 for Bug-7899

            vBtnAdd = false;
            vBtnDelete = false;
            vBtnEdit = false;
            vBtnPrint = false;
            if (ServiceType.ToUpper() != "VIEWER VERSION")//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (this.btnForward.Enabled == true || this.btnBack.Enabled == true || (this.pAddMode == false && this.pEditMode == false))
                {
                    vBtnAdd = true;
                    if (dsMain.Tables[0].Rows.Count > 0)
                    {
                        vBtnDelete = true;
                        vBtnEdit = true;
                        vBtnPrint = true;
                        //this.btnHeadNm.Enabled = true; 
                        //this.btnFldNm.Enabled = true; 
                    }
                }
                this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
            }//Added by Archana K. on 17/05/13 for Bug-7899
        }

        private void mthChkAEDPButton(Boolean vBtnAdd, Boolean vBtnEdit, Boolean vBtnDelete, Boolean vBtnPrint)
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnLocate.Enabled = false;


            if (dsMain.Tables[0].Rows.Count == 0)
            {
                //return;
            }

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
                this.btnPreview.Enabled = true;
                this.btnPrint.Enabled = true;
            }
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnLogout.Enabled = false;
            this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;
            //if(chkDeActivate.Checked)
            //this.DtpAppDt.Enabled = false;
            if (this.btnNew.Enabled == false && this.btnEdit.Enabled == false)
            {
                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
            }
            if (this.btnSave.Enabled == false)
            {
                this.btnLogout.Enabled = true;
                // this.btnEmail.Enabled = true;
                // this.btnLocate.Enabled = true;
            }
        }
        private void mthView()
        {
            this.mthBindClear();
            this.mthBindData();
            DataTable tblEmp = new DataTable();
            DataTable tblPayHead = new DataTable();
            if (this.dsMain.Tables[0].Rows.Count > 0)
            {
                SqlStr = "Select EmployeeName=(case when isnull(pMailName,'')='' then EmployeeName Else pMailName end) From EmployeeMast where EmployeeCode='" + this.dsMain.Tables[0].Rows[0]["EmployeeCode"].ToString() + "'";
                tblEmp = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (tblEmp.Rows.Count > 0)
                {
                    this.txtEmpNm.Text = tblEmp.Rows[0]["EmployeeName"].ToString();
                }
                SqlStr = "Select Head_Nm,Short_Nm From Emp_Pay_Head_Master where Fld_Nm='" + this.dsMain.Tables[0].Rows[0]["Fld_nm"].ToString() + "'";
                tblPayHead = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (tblPayHead.Rows.Count > 0)
                {
                    this.txtPayHead.Text = tblPayHead.Rows[0]["Head_Nm"].ToString();
                    this.txtShortNm.Text = tblPayHead.Rows[0]["Short_Nm"].ToString();
                }
            //    if (dsMain.Tables[0].Rows[0]["HeadTypeCode"].ToString() != "")
            //    {
            //        SqlStr = "Select HeadType From Emp_Pay_Head Where HeadTypeCode='" + dsMain.Tables[0].Rows[0]["HeadTypeCode"].ToString() + "'";
            //        DataSet tdsHeadType = new DataSet();
            //        tdsHeadType = oDataAccess.GetDataSet(SqlStr, null, 20);
            //        this.txtHeadType.Text = tdsHeadType.Tables[0].Rows[0]["HeadType"].ToString();
            //    }
            //    else
            //    {
            //        this.txtHeadType.Text = "";
            //    }
            //}
            //if (dsMain.Tables[0].Rows.Count > 0)
            //{
            //    if ((Boolean)dsMain.Tables[0].Rows[0]["Round_Off"] == false) { this.chkRoundOff.Checked = false; } else { this.chkRoundOff.Checked = true; }
            //    if ((Boolean)dsMain.Tables[0].Rows[0]["PayEditable"] == false) { this.chkSlabMaster.Checked = false; } else { this.chkSlabMaster.Checked = true; }
            //    if ((Boolean)dsMain.Tables[0].Rows[0]["PrInPaySlip"] == false) { this.ChkPaySlip.Checked = false; } else { this.ChkPaySlip.Checked = true; }
            //    if ((Boolean)dsMain.Tables[0].Rows[0]["PayEditable"] == false) { this.ChkMonthlyEditable.Checked = false; } else { this.ChkMonthlyEditable.Checked = true; }
            //    if ((Boolean)dsMain.Tables[0].Rows[0]["IsDeactive"] == false) { this.chkDeActivate.Checked = false; } else { this.chkDeActivate.Checked = true; }

            }

           

        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + "  order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();
            this.mthChkNavigationButton();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {

            this.mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = false;
            //Added by Archana K. on 17/05/13 for Bug-7899 start
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPreview.Enabled = true;
                this.btnPrint.Enabled = true;
                this.gbxDetails.Enabled = false;
                this.gbxRemark.Enabled = false;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            string SqlStr = "select top 1  " + vMainField + " as Col1 from "+vMainTblNm+ " order by  " + vMainField + " desc";
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            vMainFldVal = "";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["Col1"].ToString()) == false)
                {
                    vMainFldVal = dsTemp.Tables[0].Rows[0]["Col1"].ToString().Trim();
                }
            }
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";

            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthView();
            this.mthChkNavigationButton();



            if (dsMain.Tables[0].Rows.Count == 0)
            {
                this.btnEmail.Enabled = false;
                //this.btnLocate.Enabled = true;
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLogout_Click(this.btnExit, e);
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnAppNo.Enabled = false;
            }
            else
            {
                this.btnAppNo.Enabled = true ;
            }
            this.btnEmpNm.Enabled = vEnabled;
            this.btnPayHead.Enabled = vEnabled;
            this.txtAppNo.Enabled = vEnabled;
            this.txtEmpCode.Enabled = vEnabled;
            this.txtFld_Nm.Enabled = vEnabled;
            this.txtAmount.Enabled = vEnabled;
            this.txtEmpNm.Enabled = vEnabled; /*Ramya*/
            this.txtPayHead.Enabled = vEnabled; /*Ramya*/
            this.txtShortNm.Enabled = vEnabled; /*Ramya*/
            this.DtpAppDt.Enabled = vEnabled;
            this.txtRemark.Enabled = vEnabled;
            //this.DtpAppDt.Enabled = vEnabled;  /*Ramya*/
            if (this.pAddMode)
            {
                //this.txtFldName.Enabled = true;
            }

        }
        private void mthBindClear()
        {

            this.txtAppNo.DataBindings.Clear();
            this.txtEmpCode.DataBindings.Clear();
            this.txtFld_Nm.DataBindings.Clear();
            this.txtAmount.DataBindings.Clear();
            this.DtpAppDt.DataBindings.Clear();

            this.txtRemark.DataBindings.Clear();
            
            this.txtEmpNm.Text = "";
            this.txtPayHead.Text = "";
            this.txtShortNm.Text = "";
        }
        private void mthBindData()
        {
            this.txtAppNo.DataBindings.Add("Text", dsMain.Tables[0], "Inv_No");
            this.txtEmpCode.DataBindings.Add("Text", dsMain.Tables[0], "EmployeeCode");
            this.txtFld_Nm.DataBindings.Add("Text", dsMain.Tables[0], "Fld_Nm");
            this.txtAmount.DataBindings.Add("Text", dsMain.Tables[0], "Amount");
            this.DtpAppDt.DataBindings.Add("Text", dsMain.Tables[0], "Date"); 
            this.txtRemark.DataBindings.Add("Text", dsMain.Tables[0], "Remark");
        }
#endregion
# region Add Edit Delete Save Buttons
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            //this.btnEmpNm.Focus();
            this.label5.Focus();
            SendKeys.Send("{TAB}");
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
            if (this.pAddMode && this.txtAppNo.Text.Trim() == "")
            {
                this.txtAppNo.Text = funcGenInvNo();
                dsMain.Tables[0].Rows[0]["Inv_No"] = this.txtAppNo.Text;
            }
            dsMain.Tables[0].Rows[0]["Date"] = DateTime.Now.Date;
            this.DtpAppDt.Value = DateTime.Now.Date;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            cValid = true;
            this.mthChkValidation();
            if (cValid == false)
            {
                return;
            }
            label1.Focus();

            this.mcheckCallingApplication();


            //this.txtHead_Nm.Focus();
            this.txtAppNo.Focus();

            this.Refresh();
            this.mthSave();

            this.mthChkNavigationButton();

            timer1.Enabled = true;   /*Ramya 20/10/12*/
            timer1.Interval = 1000;
            MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

        }
        private void mthSave()
        {


            string vSaveString = string.Empty;

            this.mthGenerateNewInvNo();
            
            dsMain.Tables[0].Rows[0].AcceptChanges();

            dsMain.Tables[0].Rows[0].EndEdit();
           
            

            this.mSaveCommandString(ref vSaveString, "#ID#");
         
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);

   
            vMainFldVal = dsMain.Tables[0].Rows[0]["Inv_No"].ToString().Trim();
            SqlStr = "Select top 1 * from "+vMainTblNm+" Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();




        }
        private void mSaveCommandString(ref string vSaveString, string vkeyField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='"+vMainTblNm+"' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                //DataSet dsFlds = oDataAccess.GetDataSet("Select Fld_Nm from Emp_Pay_Head_Master", null, 20);
                vSaveString = "Set DateFormat dmy insert into Emp_Loan_Advance_Request ";
                dsMain.Tables[0].AcceptChanges();
                // string fldnm= dsMain.Tables[0].Rows[0]["Fld_Nm"].ToString();

                //foreach (DataRow dr in dsFlds.Tables[0].Rows)
                //{
                //    if (fldnm == dr["Fld_Nm"].ToString())
                //    {

                //       MessageBox.Show("Duplicate Field Name Not Allowed");
                //       IsExit = false;
                //       //return;
                //    }
                //}

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
            if (this.pEditMode == true)
            {
                vSaveString = "Set DateFormat dmy Update Emp_Loan_Advance_Request Set ";
                string vWhereCondn = string.Empty;
                foreach (DataColumn dtc1 in dsMain.Tables[0].Columns)
                {

                    {
                        // if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; } Alert Master
                        //vfldList = vfldList+ dtc1.ToString().Trim()+" = "; //Alert Master
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
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
                        // vfldList = vfldList + vfldVal; Alert Master
                        //if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                    }
                    //if (vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1) Alert Master
                    if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1) || (dtc1.ToString().Trim().ToLower() == vMainField.Trim().ToLower()))
                    {
                        if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                        vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
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
                        vWhereCondn = vWhereCondn + vfldVal;
                    }
                    else //Alert Master
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; } //Alert Master
                        vfldList = vfldList + dtc1.ToString().Trim() + " = "; //Alert Master
                        vfldList = vfldList + vfldVal;
                    }
                }
                vSaveString = vSaveString + vfldList + vWhereCondn;
            }
        }
        private void mthChkValidation()
        {
            //DataSet tDs = new DataSet();
            if (string.IsNullOrEmpty(this.txtEmpNm.Text))
            {
                MessageBox.Show("Employee Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtEmpNm.Focus();
                cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.txtPayHead.Text))
            {
                MessageBox.Show("Pay Head Type Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtPayHead.Focus();
                cValid = false;
                return;
            }
            //if (string.IsNullOrEmpty(this.txtShortNm.Text))
            //{
            //    MessageBox.Show("Short Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtShortNm.Focus();
            //    cValid = false;
            //    return;
            //}
            //if (string.IsNullOrEmpty(this.txtCalcPeriod.Text))
            //{
            //    MessageBox.Show("Calculation Period Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtCalcPeriod.Focus();
            //    cValid = false;
            //    return;
            //}
            //if (string.IsNullOrEmpty(this.txtFldName.Text))
            //{
            //    MessageBox.Show("Filed Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtFldName.Focus();
            //    cValid = false;
            //    return;
            //}
            //if (this.pAddMode)
            //{
            //    SqlStr = "Select Head_NM,Fld_Nm From Emp_Pay_Head_Master where fld_nm='" + this.txtFldName.Text + "'";
            //    tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            //    if (tDs.Tables[0].Rows.Count > 0)
            //    {
            //        MessageBox.Show("Filed Name already used for " + tDs.Tables[0].Rows[0]["Head_Nm"].ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        txtFldName.Focus();
            //        cValid = false;
            //        return;

            //    }
            //}

            //SqlStr = "Select Head_NM From Emp_Pay_Head_Master where fld_nm='" + this.txtHead_Nm.Text + "'";
            //tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            //if (tDs.Tables[0].Rows.Count > 0)
            //{
            //    MessageBox.Show("Duplicate Head Name " + this.txtHead_Nm.Text, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtHead_Nm.Focus();
            //    cValid = false;
            //    return;

            //}
            //if (this.txtSortOrd.Value > 99)
            //{
            //    MessageBox.Show("Order No. Could not grater than 99 ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtSortOrd.Focus();
            //    cValid = false;
            //    return;

            //}

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
            //this.txtFldName.ReadOnly = true;
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();
           

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
            this.txtAppNo.Focus();
        }
        private void mthCancel(object sender, EventArgs e)
        {

            if (this.dsMain.Tables[0].Rows.Count != 0)
            {
                dsMain.Tables[0].Rows[0].CancelEdit();
            }


            if (this.dsMain.Tables[0].Rows.Count == 1)
            {
                dsMain.Tables[0].Rows[0].CancelEdit();
                if (this.pAddMode)
                {
                    dsMain.Tables[0].Rows[0].Delete();
                    this.pAddMode = false;
                    this.pEditMode = false;
                    this.btnLast_Click(sender, e);
                }
                if (this.pEditMode)
                {

                    this.pAddMode = false;
                    this.pEditMode = false;
                    this.mthEnableDisableFormControls();
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthView();
                }
            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
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
                //vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim() + dsMain.Tables[0].Rows[0]["EDType"].ToString().Trim();
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                vDelString = "Delete from "+vMainTblNm+" Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, vTimeOut, true);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();
                //this.tdsLoc.Tables[0].Rows[0].Delete();  //Ramya


                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from "+vMainTblNm+" Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc"; //ramya 13/02/12
                        dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    }
                }
                this.mthView();
                this.mthChkNavigationButton();
            }

        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
            {
                btnNew_Click(this.btnNew, e);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
            {
                btnEdit_Click(this.btnEdit, e);
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
            {
                btnSave_Click(this.btnSave, e);
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
            {
                btnCancel_Click(this.btnCancel, e);
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
            {
                btnDelete_Click(this.btnCancel, e);
            }
        }
#endregion
#region Common Methods
        private string funcGenInvNo()
        {
            vInvNoLen = 6;
            SqlStr = "Select Inv_No=isnull(max(Inv_No),0)+1 From Gen_Inv where Entry_ty='" + vGen_Inv + "' and L_Yn='" + vL_Yn + "'";
            DataRow drInvNo;
            drInvNo = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
            iInv_No = Convert.ToInt16(drInvNo["Inv_No"]);
            vInv_No = Convert.ToString(iInv_No).Trim();
            vInv_No = vInv_No.PadLeft(vInvNoLen, Convert.ToChar("0"));
            return vInv_No;


        }
        private void mthGenerateNewInvNo()
        {
            if (vL_Yn == "")
            {
                SqlStr = "Select Sta_Dt,End_Dt From Vudyog..Co_Mast Where CompId=" + this.pCompId.ToString();
                DataRow drYear = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
                vL_Yn = Convert.ToDateTime(drYear["Sta_Dt"]).Year.ToString().Trim() + "-" + Convert.ToDateTime(drYear["End_Dt"]).Year.ToString().Trim();

            }
            int i_vInvoiceNo = 0;
            string mCond = "";
            int voldInvoiceNo = 0;
            if (this.pEditMode)
            {
                voldInvoiceNo = Convert.ToInt16(this.txtAppNo.Text);
            }
            int _vInvoiceEn = voldInvoiceNo;
            if (this.txtAppNo.Text.ToString().Trim() == "")
            {
                i_vInvoiceNo = 1;
            }
            else
            {
                i_vInvoiceNo = Convert.ToInt16(this.txtAppNo.Text);
            }

            SqlStr = "Select * from Gen_inv with (NOLOCK) where 1=0";
            DataTable Gen_inv_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "Select * from Gen_miss with (NOLOCK) where 1=0";
            DataTable Gen_miss_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataRow drGen_inv_vw = Gen_inv_vw.NewRow();
            drGen_inv_vw["Entry_ty"] = "LA";
            drGen_inv_vw["Inv_dt"] = this.DtpAppDt.Value;
            drGen_inv_vw["Inv_sr"] = "";
            drGen_inv_vw["Inv_No"] = i_vInvoiceNo;
            drGen_inv_vw["l_Yn"] = vL_Yn;
            Gen_inv_vw.Rows.Add(drGen_inv_vw);

            DataRow drGen_miss_vw = Gen_miss_vw.NewRow();
            drGen_miss_vw["Entry_ty"] = "LA";
            drGen_miss_vw["Inv_dt"] = this.DtpAppDt.Value;
            drGen_miss_vw["Inv_sr"] = "";
            drGen_miss_vw["Inv_No"] = i_vInvoiceNo;
            drGen_miss_vw["l_Yn"] = vL_Yn;
            drGen_miss_vw["Flag"] = "Y";
            Gen_miss_vw.Rows.Add(drGen_miss_vw);

            Boolean mrollback = true;

            SqlStr = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX)";
            SqlStr = SqlStr + " where Entry_ty = '"+vGen_Inv+"' And Inv_sr ='' And L_yn ='" + vL_Yn + "'";
            DataTable tmptbl_vw = new DataTable();
            tmptbl_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "";
            if (tmptbl_vw.Rows.Count <= 0)
            {
                SqlStr = "Insert into Gen_inv (Entry_Ty,Inv_Sr,Inv_No,L_Yn,Inv_Dt,CompId) Values ('" + vGen_Inv + "',''," + Gen_inv_vw.Rows[0]["Inv_No"].ToString().Trim() + ",'" + vL_Yn + "','" + this.DtpAppDt.Text + "',0)";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
            else
            {
                if (Convert.ToInt16(tmptbl_vw.Rows[0]["Inv_no"]) < Convert.ToInt16(drGen_inv_vw["Inv_No"]))
                {

                    mCond = "Entry_ty = '" + vGen_Inv + "' And Inv_sr='' And L_yn ='" + vL_Yn + "'";
                    SqlStr = "Update Gen_inv Set Inv_No=" + i_vInvoiceNo.ToString().Trim() + ",Inv_Dt='" + this.DtpAppDt.Text + "' where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }


            }
            SqlStr = "Select Top 1 Flag from Gen_miss where Entry_ty = '" + vGen_Inv + "' And Inv_sr = '' And Inv_no =" + Gen_inv_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
            string vFoundInMiss = "Y";
            DataTable tmptbl_vw1 = new DataTable();
            tmptbl_vw1 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "";
            if (tmptbl_vw1.Rows.Count <= 0)
            {
                vFoundInMiss = "N";
                SqlStr = "INSERT INTO GEN_MISS ([entry_ty],[inv_sr],[inv_no],[flag],[l_yn],[inv_dt],[user_name],[CompId]) VALUES";
                SqlStr = SqlStr + "('"+vGen_Inv+"',''," + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",'" + Gen_miss_vw.Rows[0]["Flag"].ToString() + "','" + vL_Yn + "','" + this.DtpAppDt.Text + "','',0)";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
            else
            {
                vFoundInMiss = tmptbl_vw1.Rows[0]["Flag"].ToString();
                if (vFoundInMiss == "N")
                {
                    mCond = "Entry_ty = '" + vGen_Inv + "' And Inv_sr='' and Inv_no=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
                    SqlStr = "Update GEN_MISS Set Inv_No=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",Inv_Dt='" + this.DtpAppDt.Text + "',Flag='" + Gen_miss_vw.Rows[0]["Flag"].ToString() + "'  where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
            }


            if (vFoundInMiss == "N")
            {
                vInv_No = Convert.ToString(iInv_No).Trim();
                vInv_No = vInv_No.PadLeft(vInvNoLen, Convert.ToChar("0"));
              
                SqlStr = "Select Top 1 Entry_ty from " + vMainTblNm + " where Entry_ty = '" + vGen_Inv + "' And Inv_no = '" + this.txtAppNo.Text.Trim() + "' And FinYear = '" + vL_Yn + "'";
                DataTable tmptbl_vw2 = new DataTable();
                tmptbl_vw2 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                if (tmptbl_vw2.Rows.Count <= 0)
                {
                    mrollback = false;
                    return; //End Do
                }
                else
                {
                    vFoundInMiss = "Y";
                }

            }
            if (vFoundInMiss == "Y")
            {
                Gen_inv_vw.Rows[0]["Inv_No"] = Convert.ToInt16(Gen_inv_vw.Rows[0]["Inv_No"]) + 1;
                Gen_miss_vw.Rows[0]["Inv_No"] = Convert.ToInt16(Gen_inv_vw.Rows[0]["Inv_No"]);
            }
            SqlStr = "";
            if (mrollback == false)
            {
                Gen_inv_vw.Rows[0]["Inv_No"] = _vInvoiceEn;
                Gen_miss_vw.Rows[0]["Inv_No"] = _vInvoiceEn;
                mCond = "Entry_ty = '" + vGen_Inv + "' And Inv_sr='' and Inv_no=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
                try
                {
                    SqlStr = "Update GEN_MISS Set Inv_No=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",Inv_Dt='" + this.DtpAppDt.Text + "' where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.pPApplName);
                    mrollback = true;
                    oDataAccess.RollbackTransaction();
                }
            }

        }

        private void SetMenuRights() //Rup
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
                //this.pDeleteButton = (rArray[2].ToString().Trim() == "DY" ? true : false);
                this.pDeleteButton = false;
                this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);
                ////menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                //btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }
        private void SetFormColor() //Rup
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
        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpLoanRequest.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);

            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
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

#endregion

        private void btnPayHead_Click(object sender, EventArgs e)
        {
            if (this.pEditMode)
            {
                SqlStr = "Select Req_Id From Emp_Loan_Advance Where Req_Id=" + dsMain.Tables[0].Rows[0]["Id"].ToString();
                DataTable tdt = new DataTable();
                tdt = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (tdt.Rows.Count > 0)
                {
                    MessageBox.Show("Can't change the Value becuase Loan and Advance Details Already Generated for this Request", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);
                    return;
                }
            }
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Head_Nm,Short_Nm,Fld_Nm from Emp_Pay_Head_Master where HeadTypeCode in ('LON','ADV') order by Head_Nm,Fld_NM";
            VForText = "Select Head Name";
            vSearchCol = "Head_Nm";
            vDisplayColumnList = "Head_Nm:Head Name,Short_Nm:Short Name,Fld_Nm:Field Name";
            vReturnCol = "Head_Nm,Short_Nm,Fld_Nm";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtPayHead.Text = oSelectPop.pReturnArray[0];
                this.txtShortNm.Text = oSelectPop.pReturnArray[1];
                this.txtFld_Nm.Text = oSelectPop.pReturnArray[2];
                dsMain.Tables[0].Rows[0]["Fld_Nm"] = oSelectPop.pReturnArray[2];
                //vMainFldVal = this.txtdAc_Name.Text.Trim();
                //SqlStr = "Select top 1 * from Emp_Pay_Head_Master Where Head_Nm='" + vMainFldVal + "' order by Head_Nm ";
                //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }
        }

       

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEmpNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            if (this.pAddMode)
            {

                SqlStr = "Select Distinct EmployeeName=(case when isnull(pMailName,'')='' then EmployeeName Else pMailName end), EmployeeCode from EmployeeMast Where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' order by EmployeeCode ,(case when isnull(pMailName,'')='' then EmployeeName Else pMailName end) ";
                VForText = "Select Employee Name";
                vSearchCol = "EmployeeName";
                vDisplayColumnList = "EmployeeName:Employee Name,Short_Nm:Short Name,EmployeeCode:Employee Code";
                vReturnCol = "EmployeeName,EmployeeCode";
            }
            else
            {
                SqlStr = "Select EmployeeName=(case when isnull(b.pMailName,'')='' then b.EmployeeName Else b.pMailName end),a.EmployeeCode";
                SqlStr = SqlStr + " ,a.Inv_No,a.Date";
                SqlStr = SqlStr + " ,C.Head_Nm,c.Short_Nm";
                //SqlStr =SqlStr +" ,a.Fld_Nm";
                SqlStr = SqlStr + " From Emp_Loan_Advance_Request a";
                SqlStr = SqlStr + " inner join EmployeeMast b on (a.EmployeeCode=b.EmployeeCode)";
                SqlStr = SqlStr + " inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm) order by A.EmployeeCode,a.Inv_No,c.Head_Nm";

                VForText = "Select Employee Name and Application No.";
                vSearchCol = "EmployeeName";
                vDisplayColumnList = "Inv_No:Application No,EmployeeCode:Employee Code,EmployeeName:Employee Name,Head_Nm:Pay Head Name";
                vReturnCol = "EmployeeName,Inv_No";
                
            }

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            DataView dvw = tDs.Tables[0].DefaultView;

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = dsMain.Tables[0].Rows[0]["EmployeeCode"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                if (this.pAddMode)
                {
                    this.txtEmpNm.Text = oSelectPop.pReturnArray[0];
                    this.txtEmpCode.Text = oSelectPop.pReturnArray[1];
                    dsMain.Tables[0].Rows[0]["EmployeeCode"] = oSelectPop.pReturnArray[1];
                }
                else
                {
                    this.txtAppNo.Text = oSelectPop.pReturnArray[1];
                    vMainFldVal = this.txtShortNm.Text = oSelectPop.pReturnArray[1];

                    SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthView();
                    this.mthChkNavigationButton();
                    this.mthEnableDisableFormControls();

                }

            }


        }

        private void btnAppNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select a.Inv_No,EmployeeName=(case when isnull(b.pMailName,'')='' then b.EmployeeName Else b.pMailName end),a.EmployeeCode";
            SqlStr = SqlStr + " ,a.Date";
            SqlStr = SqlStr + " ,C.Head_Nm,c.Short_Nm";
            //SqlStr =SqlStr +" ,a.Fld_Nm";
            SqlStr = SqlStr + " From Emp_Loan_Advance_Request a";
            SqlStr = SqlStr + " inner join EmployeeMast b on (a.EmployeeCode=b.EmployeeCode)";
            SqlStr = SqlStr + " inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm) order by A.EmployeeCode,a.Inv_No,c.Head_Nm";

            VForText = "Select Request No. and Employee Name";
            vSearchCol = "Inv_No";
            vDisplayColumnList = "Inv_No:Request No,EmployeeCode:Employee Code,EmployeeName:Employee Name,Head_Nm:Pay Head Name";
            vReturnCol = "EmployeeName,Inv_No";



            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            DataView dvw = tDs.Tables[0].DefaultView;

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                this.txtAppNo.Text = oSelectPop.pReturnArray[1];
                vMainFldVal = this.txtShortNm.Text = oSelectPop.pReturnArray[1];

                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                this.mthView();
                this.mthChkNavigationButton();
                this.mthEnableDisableFormControls();
            }

        }

        private void txtEmpNm_KeyDown(object sender, KeyEventArgs e)      /*Ramya 20/10/12*/
        {
            if (pAddMode == true)    
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnEmpNm_Click(sender, new EventArgs());
                }
            }
        }

        private void txtPayHead_KeyDown(object sender, KeyEventArgs e)   /*Ramya 20/10/12*/
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnPayHead_Click(sender, new EventArgs());
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)    /*Ramya 20/10/12*/
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

        private void frmLoanRequest_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();/*Ramya 27/10/12*/
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.mthPrint(2);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.mthPrint(3);

        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            this.mthPrint(4);
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            this.mthPrint(7);
        }
        private void mthPrint(Int16 vPrintOption)
        {

            string vRepGroup = "Loan and Advance Request";
            //MessageBox.Show(vPrintPara);
            udReportList.cReportList oPrint = new udReportList.cReportList();
            oPrint.pDsCommon = this.vDsCommon;
            oPrint.pServerName = this.pServerName;
            oPrint.pComDbnm = this.pComDbnm;
            oPrint.pUserId = this.pUserId;
            oPrint.pPassword = this.pPassword;
            oPrint.pAppPath = this.pAppPath;
            oPrint.pPApplText = this.pPApplText;
            //oPrint.pPara = this.pPara;
            oPrint.pRepGroup = vRepGroup;
            //oPrint.pTran_Cd = Convert.ToInt16(this.vTran_Cd);
            oPrint.pSpPara = dsMain.Tables[0].Rows[0]["Id"].ToString();
            oPrint.pPrintOption = vPrintOption;
            oPrint.Main();

            timer1.Enabled = true; /*Ramya 01/11/12*/
            timer1.Interval = 1000;
            MessageBox.Show("Loan and Advance Request Generated", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);



        }

        private void mthDsCommon()
        {
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            SqlStr = "Select * From vudyog..Co_Mast where CompId=" + pPara[0];
            company = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (vL_Yn == "")
            {
                vL_Yn = ((DateTime)company.Rows[0]["Sta_Dt"]).Year.ToString().Trim() + "-" + ((DateTime)company.Rows[0]["End_Dt"]).Year.ToString().Trim();
            }
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            SqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
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
            if (this.btnEmail.Enabled)
                btnEmail_Click(this.btnEmail, e);
        }

        private void exportPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnExportPdf.Enabled)
                btnExportPdf_Click(this.btnExportPdf, e);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);  
        }

        


        

    }
}
