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
using udclsDGVNumericColumn;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpLoanDetails
{
    public partial class frmLoanDetails : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain, dsLoanDet;
        DataSet vDsCommon;
        String SqlStr;
        String cAppPId, cAppName;
        string vMainTblNm = "Emp_Loan_Advance", vMainField = "Inv_No", vMainFldVal = "";
        Boolean cValid;
        string vInv_No = "", vDocNo = "";
        string vL_Yn = string.Empty, vGen_Inv = "LD";
        int iInv_No = 0, iDoc_No = 0, vTran_Cd = 0, vInvNoLen = 0;
        int vReq_Id;
        DataTable tblDelDet;
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmLoanDetails(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Loan and Advance Details";
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
        private void frmLoanDetails_Load(object sender, EventArgs e)
        {
            string fName;
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.ntxtDue_Amt.pAllowNegative = false;
            this.ntxtDue_Amt.MaxLength = 17;
            this.ntxtDue_Amt.pDecimalLength = 2;

            this.ntxtAmount.pAllowNegative = false;
            this.ntxtAmount.MaxLength = 17;
            this.ntxtAmount.pDecimalLength = 2;

            this.ntxtLoanAmt.pAllowNegative = false;
            this.ntxtLoanAmt.MaxLength = 17;
            this.ntxtLoanAmt.pDecimalLength = 2;

            this.ntxtPer.pAllowNegative = false;
            this.ntxtPer.MaxLength = 7;
            this.ntxtPer.pDecimalLength = 3;

            this.ntxtInst.pAllowNegative = false;
            this.ntxtInst.MaxLength = 3;        // Changed by Sachin N. S. on 28/03/2014 for Bug-21908
            this.ntxtInst.pDecimalLength = 0;

            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            this.DtpReqDt.CustomFormat = "dd/MM/yyyy";
            this.DtpReqDt.Format = DateTimePickerFormat.Custom;

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

            fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnRefNo.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnAppNo.Image = Image.FromFile(fName);
                this.btnApprovedBy.Image = Image.FromFile(fName);
                this.btnBankNm.Image = Image.FromFile(fName);
            }

            if (vL_Yn == "")
            {
                SqlStr = "Select Sta_Dt,End_Dt From Vudyog..Co_Mast Where CompId=" + this.pCompId.ToString();
                DataRow drYear = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
                vL_Yn = Convert.ToDateTime(drYear["Sta_Dt"]).Year.ToString().Trim() + "-" + Convert.ToDateTime(drYear["End_Dt"]).Year.ToString().Trim();
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
            this.btnLast_Click(sender, e);
            SqlStr = "Select DelID=cast(0 as Int) Where 1=2";
            tblDelDet = new DataTable();
            tblDelDet = oDataAccess.GetDataTable(SqlStr, null, 30);
        }

        # region Add Edit Delete Save Buttons
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            //this.btnAppNo.Focus();
            //this.txtRefNo.Focus();/*Ramya 16/11/12*/
            this.label7.Focus();
            SendKeys.Send("{TAB}");

        }
        private void mthNew(object sender, EventArgs e)
        {

            this.mthBindClear();
            vTran_Cd = 0;
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
            if (dsLoanDet != null)
            {
                if (dsLoanDet.Tables.Count > 0)
                {
                    dsLoanDet.Tables[0].Rows.Clear();
                }
            }
            dsMain.Tables[0].Rows[0].BeginEdit();
            if (this.pAddMode && this.txtAppNo.Text.Trim() == "")
            {
                this.txtRefNo.Text = funcGenInvNo();
                dsMain.Tables[0].Rows[0]["Inv_No"] = this.txtRefNo.Text;
            }
            dsMain.Tables[0].Rows[0]["Date"] = DateTime.Now.Date;
            this.DtpReqDt.Value = DateTime.Now.Date;
            dsLoanDet = new DataSet();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            cValid = true;
            this.mthChkValidation();
            if (cValid == false)
            {
                return;
            }
            //Added by Archana K. on 16/08/13 for bug-18246 start
            if (this.dsLoanDet.Tables.Count > 0)
            {
                if (dsLoanDet.Tables[0].Rows.Count > 0)
                {
                    if (ntxtInst.Text != dgvLoanDet.Rows.Count.ToString())
                    {
                        MessageBox.Show("No of Installent and schedule records mismatch", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop); /*Ramya 01/03/2013*/
                        btnSchedule.Focus();
                        cValid = false;
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("No of Installent and schedule records mismatch", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop); /*Ramya 01/03/2013*/
                btnSchedule.Focus();
                cValid = false;
                return;

            }
            if (cValid == false)
            {
                return;
            }
            //Added by Archana K. on 16/08/13 for bug-18246 end
            label1.Focus();

            this.mcheckCallingApplication();


            //this.txtHead_Nm.Focus();
            this.txtAppNo.Focus();

            this.Refresh();
            cValid = true; /*Ramya*/
            this.mthSave();
            if (cValid == false)
            {
                return;
            }


            this.mthChkNavigationButton();

        }
        private void mthSave()
        {


            string vSaveString = string.Empty;

            if (this.rbtnApproved.Checked)
            {
                dsMain.Tables[0].Rows[0]["Req_Stat"] = "A";
            }
            else if (this.rbtnRejected.Checked)
            {
                dsMain.Tables[0].Rows[0]["Req_Stat"] = "R";
            }
            else
            {
                dsMain.Tables[0].Rows[0]["Req_Stat"] = "H";

            }

            if (this.rbtnFlat.Checked)
            {
                dsMain.Tables[0].Rows[0]["Loan_Type"] = "F";
            }
            else if (this.rbtnDemnising.Checked)
            {

                dsMain.Tables[0].Rows[0]["Loan_Type"] = "D";
            }
            else
            {
                dsMain.Tables[0].Rows[0]["Loan_Type"] = "N";

            }
            dsMain.Tables[0].Rows[0]["Entry_ty"] = vGen_Inv;
            dsMain.Tables[0].Rows[0]["FinYear"] = vL_Yn;
            if (dtpRefDt.Text.Trim() != "")         /*Ramya 22/11/12*/
            {
                dsMain.Tables[0].Rows[0]["Date"] = this.dtpRefDt.Value;
            }
            dsMain.Tables[0].Rows[0]["AppDate"] = this.dtpApproveDt.Value;
            dsMain.Tables[0].Rows[0]["Ded_Date"] = this.DtpDedDate.Value;
            dsMain.Tables[0].Rows[0]["Cheque_Dt"] = this.DtpChequeDt.Value;
            dsMain.Tables[0].Rows[0]["Req_Date"] = this.DtpReqDt.Value;  /*Ramya 17/11/12*/
            dsMain.Tables[0].Rows[0].AcceptChanges();
            dsMain.Tables[0].Rows[0].EndEdit();

            vSaveString = "";
            this.mSaveCommandString(ref vSaveString, "#Tran_Cd#", "#cMonth#", dsMain.Tables[0].Rows[0], vMainTblNm);
            oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
            //oDataAccess.BeginTransaction();
            try
            {
                if (dsLoanDet.Tables.Count != 0)
                {
                    if (dsLoanDet.Tables[0].Rows.Count == 0 && rbtnApproved.Checked)  /*Ramya*/
                    {
                        MessageBox.Show("Please Shedule the Loan First");
                        cValid = false;
                        return;

                    }
                }

                oDataAccess.BeginTransaction();  /*Ramya 16/04/2013*/
                if (this.pEditMode == true)
                {
                    vTran_Cd = Convert.ToInt16(dsMain.Tables[0].Rows[0]["tran_cd"]);
                    foreach (DataRow tDr in tblDelDet.Rows)
                    {
                        SqlStr = "Delete From Emp_Loan_Advance_Details Where ID=" + tDr["DelId"].ToString();
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                    }
                }
                // oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
                if (this.pAddMode)
                {
                    SqlStr = "select ident_current('" + vMainTblNm + "') as Tran_cd";
                    DataRow tDr = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
                    vTran_Cd = Convert.ToInt16(tDr["Tran_cd"]);
                }


                //if (this.pAddMode)
                //{
                if (rbtnApproved.Checked)  /*Ramya 01/11/12*/
                {
                    if (dsLoanDet.Tables.Count != 0)//Added by Archana K. on 23/05/13 for Bug-14369
                    {
                        foreach (DataRow tDr in dsLoanDet.Tables[0].Rows)
                        {
                            vSaveString = "";
                            if (tDr["Id"] == DBNull.Value)
                            {
                                tDr["Entry_ty"] = vGen_Inv;
                                tDr["Tran_cd"] = vTran_Cd;
                                //this.mSaveCommandString(ref vSaveString, "#Tran_Cd#", "#cMonth#", tDr, "Emp_Loan_Advance_Details");
                                this.mth_InsertString(ref vSaveString, "", "#cMonth#", tDr, "Emp_Loan_Advance_Details");

                            }
                            else
                            {
                                this.mth_UpdateString(ref vSaveString, "#ID#", "#cMonth#Tran_Cd#", tDr, "Emp_Loan_Advance_Details");
                            }
                            oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                        }
                    }//Added by Archana K. on 23/05/13 for Bug-14369
                }
                //this.mSaveCommandString(ref vSaveString, "#ID#", "#cMonth#TRAN_CD#", dsLoanDet.Tables[0], "Emp_Loan_Advance_Details");

                //}
                oDataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                oDataAccess.RollbackTransaction();
            }

            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();

            this.mthGenerateNewInvNo();

            //this.mthChkNavigationButton();

            //vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim() + dsMain.Tables[0].Rows[0]["EDType"].ToString().Trim();
            vMainFldVal = dsMain.Tables[0].Rows[0]["Inv_No"].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();




        }
        private void mth_InsertString(ref string vSaveString, string vkeyField, string vFieldExclude, DataRow vSourceRow, string vTargetTable)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + vTargetTable + "' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            vSaveString = "Set DateFormat dmy insert into " + vTargetTable;
            foreach (DataColumn dtc1 in vSourceRow.Table.Columns)
            {

                if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                {
                    if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                    if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                    vfldList = vfldList + dtc1.ToString().Trim();
                    vfldVal = vSourceRow[dtc1.ToString().Trim()].ToString().Trim();

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
        private void mth_UpdateString(ref string vSaveString, string vkeyField, string vFieldExclude, DataRow vSourceRow, string vTargetTable)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            //DataSet dsData = new DataSet();
            //string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            //sqlstr = sqlstr + " and o.name='" + vTargetTable + "' ";
            //dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            //foreach (DataRow dr1 in dsData.Tables[0].Rows)
            //{
            //    if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
            //    vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            //}

            /*<---Identity Columns--->*/

            vSaveString = "Set DateFormat dmy Update " + vTargetTable + " Set ";
            string vWhereCondn = string.Empty;
            foreach (DataColumn dtc1 in vSourceRow.Table.Columns)
            {
                //if (vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 )
                if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                {
                    // if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; } Alert Master
                    //vfldList = vfldList+ dtc1.ToString().Trim()+" = "; //Alert Master
                    vfldVal = vSourceRow[dtc1.ToString().Trim()].ToString().Trim();
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
                if (vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1)
                //if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1) || (dtc1.ToString().Trim().ToLower() == vMainField.Trim().ToLower()))
                {
                    if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                    vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                    vfldVal = vSourceRow[dtc1.ToString().Trim()].ToString().Trim();
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


                else if (vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                //else //if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                {
                    if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; } //Alert Master
                    vfldList = vfldList + dtc1.ToString().Trim() + " = "; //Alert Master
                    vfldList = vfldList + vfldVal;
                }
            }
            vSaveString = vSaveString + vfldList + vWhereCondn;

        }

        private void mSaveCommandString(ref string vSaveString, string vkeyField, string vFieldExclude, DataRow vSourceRow, string vTargetTable)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + vTargetTable + "' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                vSaveString = "Set DateFormat dmy insert into " + vTargetTable;


                foreach (DataColumn dtc1 in vSourceRow.Table.Columns)
                {

                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + dtc1.ToString().Trim();
                        vfldVal = vSourceRow[dtc1.ToString().Trim()].ToString().Trim();

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
                vSaveString = "Set DateFormat dmy Update " + vTargetTable + " Set ";
                string vWhereCondn = string.Empty;
                foreach (DataColumn dtc1 in vSourceRow.Table.Columns)
                {
                    //if (vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 )
                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        // if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; } Alert Master
                        //vfldList = vfldList+ dtc1.ToString().Trim()+" = "; //Alert Master
                        vfldVal = vSourceRow[dtc1.ToString().Trim()].ToString().Trim();
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
                        vfldVal = vSourceRow[dtc1.ToString().Trim()].ToString().Trim();
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
                    else if (vFieldExclude.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
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
            if (string.IsNullOrEmpty(this.txtAppNo.Text))   /*Ramya 01/11/12*/
            {
                MessageBox.Show("Request No Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtAppNo.Focus();
                cValid = false;
                return;
            }
            if (!(rbtnApproved.Checked || rbtnHold.Checked || rbtnRejected.Checked)) /*Ramya 22/11/12*/
            {
                MessageBox.Show("Status Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                rbtnHold.Focus();
                cValid = false;
                return;

            }
            if (rbtnApproved.Checked || rbtnRejected.Checked)  /*Ramya 22/11/12*/
            {
                if (string.IsNullOrEmpty(this.txtApprovedBy.Text)) /*Ramya 01/11/12*/
                {
                    MessageBox.Show("Approved/Rejected By Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtApprovedBy.Focus();
                    cValid = false;
                    return;
                }
            }
            if (rbtnApproved.Checked) /*Ramya 22/11/12*/
            {
                if (Convert.ToDecimal((this.ntxtLoanAmt.Text)) == 0)
                {
                    MessageBox.Show("Approved Amount Should not be zero", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.ntxtLoanAmt.Focus();
                    cValid = false;
                    return;

                }
            }
            if (Convert.ToDecimal((this.ntxtLoanAmt.Text)) > Convert.ToDecimal((this.ntxtAmount.Text)))
            {
                MessageBox.Show("Loan Amount Should not grater then Application Amount", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.ntxtLoanAmt.Focus();
                cValid = false;
                return;
            }
            if (rbtnApproved.Checked)  /*Ramya 22/11/12*/
            {
                if (Convert.ToDecimal((this.ntxtInst.Text)) == 0)
                {
                    MessageBox.Show("No. of Installment Should not be zero", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.ntxtInst.Focus();
                    cValid = false;
                    return;
                }
            }
            if (this.dtpRefDt.Value < this.DtpReqDt.Value)
            {
                MessageBox.Show("Reference Date Should be Less then Request Date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpRefDt.Focus();
                cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.dtpApproveDt.Value.ToString()))   /*Ramya 01/11/12*/
            {
                MessageBox.Show("Approved Date Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpApproveDt.Focus();
                cValid = false;
                return;

            }
            if (rbtnApproved.Checked)
            {
                if (Convert.ToDateTime(this.dtpApproveDt.Value).Date < Convert.ToDateTime(this.dtpRefDt.Value).Date)  /*Ramya*/
                {

                    //MessageBox.Show("Approval Date Should not be Less then Application Date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("Approval Date Should not be Less then Reference Date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop); /*Ramya 01/03/2013*/
                    this.dtpApproveDt.Focus();
                    cValid = false;
                    return;
                }
            }
            if (rbtnApproved.Checked) /*Rupesh 30/11/12*/
            {

                if (Convert.ToDateTime(this.DtpDedDate.Value).Date < Convert.ToDateTime(this.dtpApproveDt.Value).Date)
                {

                    MessageBox.Show("Deduction Date Should not be Less then Approval Date", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.DtpDedDate.Focus();
                    cValid = false;
                    return;
                }
            }
            if (Convert.ToDecimal((this.ntxtPer.Text)) == 0 && (this.rbtnFlat.Checked || this.rbtnDemnising.Checked))
            {
                MessageBox.Show("Interest Should not be zero", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.ntxtInst.Focus();
                cValid = false;
                return;
            }

            //if (!string.IsNullOrEmpty(this.DtpDedDate.Value.ToString()) && this.pEditMode)   /*Ramya 01/11/12*/
            //{
            //    DateTime vDate = this.DtpDedDate.Value;
            //    SqlStr = "Select Pay_Year,Pay_Month=Min(Pay_Month) from Emp_Loan_Advance_Details Where Tran_Cd=" + vTran_Cd.ToString() + " and RePay_Amt<>0 and Pay_Year in (Select Min(Pay_Year) from Emp_Loan_Advance_Details Where Tran_Cd=" + vTran_Cd.ToString()  +")  and (Pay_Year=" + Convert.ToDateTime(DtpDedDate.Value).Year.ToString() + " and Pay_Month<>" + Convert.ToDateTime(DtpDedDate.Value).Month.ToString() +") Group By Pay_Year";
            //    SqlStr = "Select Min(Cast(Cast(Pay_Year as Varchar)+'/'+Cast(Pay_Month  as Varchar)+'/01' as smalldatetime)) from Emp_Loan_Advance_Details Where Tran_Cd=10 and RePay_Amt<>0";
            //    DataSet tds = new DataSet();
            //    tds = oDataAccess.GetDataSet(SqlStr, null, 30);
            //    if (tds.Tables[0].Rows.Count > 0)
            //    {
            //        if((this.DtpDedDate.Value).Date< (Convert.ToDateTime((tds.Tables[0].Rows[0]["minDate"])).Date) )
            //        {
            //            MessageBox.Show("Deduction Should be Statrted with " + fnCMonth(12) + " Month Only", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            this.DtpDedDate.Focus();
            //        }
            //    }

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
            if (this.pEditMode)
            {
                SqlStr = "Select EmployeeCode  From Emp_Loan_Advance_Details Where Tran_Cd=" + vTran_Cd.ToString().Trim() + " And RePay_Amt<>0";
                DataTable tTblEdit = new DataTable();
                tTblEdit = oDataAccess.GetDataTable(SqlStr, null, 30);

                if (tTblEdit.Rows.Count > 0)
                {
                    this.DtpDedDate.Enabled = false;
                }
            }
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

                vDelString = "Delete from " + vMainTblNm + " Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, vTimeOut, true);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();
                //this.tdsLoc.Tables[0].Rows[0].Delete();  //Ramya


                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
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
                btnNew_Click(this.btnNew, e);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);

        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
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
                btnDelete_Click(this.btnCancel, e);

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
                //i_vInvoiceNo = Convert.ToInt16(this.txtAppNo.Text);//Commented by Archana K. on 25/05/13 for Bug-14369
                i_vInvoiceNo = Convert.ToInt16(this.txtRefNo.Text);//Changed by Archana K. on 25/05/13 for Bug-14369
            }

            SqlStr = "Select * from Gen_inv with (NOLOCK) where 1=0";
            DataTable Gen_inv_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "Select * from Gen_miss with (NOLOCK) where 1=0";
            DataTable Gen_miss_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataRow drGen_inv_vw = Gen_inv_vw.NewRow();
            //drGen_inv_vw["Entry_ty"] = "LA";//Commented by Archana K. on 23/05/13 for Bug-14369
            drGen_inv_vw["Entry_ty"] = "LD";//Changed by Archana K. on 23/05/13 for Bug-14369
            drGen_inv_vw["Inv_dt"] = this.DtpReqDt.Value;
            drGen_inv_vw["Inv_sr"] = "";
            drGen_inv_vw["Inv_No"] = i_vInvoiceNo;
            drGen_inv_vw["l_Yn"] = vL_Yn;
            Gen_inv_vw.Rows.Add(drGen_inv_vw);

            DataRow drGen_miss_vw = Gen_miss_vw.NewRow();
            //drGen_miss_vw["Entry_ty"] = "LA";//Commented by Archana K. on 23/05/13 for Bug-14369
            drGen_miss_vw["Entry_ty"] = "LD";//Changed by Archana K. on 23/05/13 for Bug-14369
            drGen_miss_vw["Inv_dt"] = this.DtpReqDt.Value;
            drGen_miss_vw["Inv_sr"] = "";
            drGen_miss_vw["Inv_No"] = i_vInvoiceNo;
            drGen_miss_vw["l_Yn"] = vL_Yn;
            drGen_miss_vw["Flag"] = "Y";
            Gen_miss_vw.Rows.Add(drGen_miss_vw);

            Boolean mrollback = true;

            SqlStr = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX)";
            SqlStr = SqlStr + " where Entry_ty = '" + vGen_Inv + "' And Inv_sr ='' And L_yn ='" + vL_Yn + "'";
            DataTable tmptbl_vw = new DataTable();
            tmptbl_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "";
            if (tmptbl_vw.Rows.Count <= 0)
            {
                SqlStr = "Insert into Gen_inv (Entry_Ty,Inv_Sr,Inv_No,L_Yn,Inv_Dt,CompId) Values ('" + vGen_Inv + "',''," + Gen_inv_vw.Rows[0]["Inv_No"].ToString().Trim() + ",'" + vL_Yn + "','" + this.DtpReqDt.Text + "',0)";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
            else
            {
                if (Convert.ToInt16(tmptbl_vw.Rows[0]["Inv_no"]) < Convert.ToInt16(drGen_inv_vw["Inv_No"]))
                {

                    mCond = "Entry_ty = '" + vGen_Inv + "' And Inv_sr='' And L_yn ='" + vL_Yn + "'";
                    SqlStr = "Update Gen_inv Set Inv_No=" + i_vInvoiceNo.ToString().Trim() + ",Inv_Dt='" + this.DtpReqDt.Text + "' where " + mCond;
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
                SqlStr = SqlStr + "('" + vGen_Inv + "',''," + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",'" + Gen_miss_vw.Rows[0]["Flag"].ToString() + "','" + vL_Yn + "','" + this.DtpReqDt.Text + "','',0)";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
            else
            {
                vFoundInMiss = tmptbl_vw1.Rows[0]["Flag"].ToString();
                if (vFoundInMiss == "N")
                {
                    mCond = "Entry_ty = '" + vGen_Inv + "' And Inv_sr='' and Inv_no=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
                    SqlStr = "Update GEN_MISS Set Inv_No=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",Inv_Dt='" + this.DtpReqDt.Text + "',Flag='" + Gen_miss_vw.Rows[0]["Flag"].ToString() + "'  where " + mCond;
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
                    SqlStr = "Update GEN_MISS Set Inv_No=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",Inv_Dt='" + this.DtpReqDt.Text + "' where " + mCond;
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
            cAppName = "udEmpLoanDetails.exe";
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
        #endregion

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

                    SqlStr = "select " + vMainField + " from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = "select " + vMainField + " from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnBack.Enabled = true;
                        this.btnFirst.Enabled = true;


                    }
                }//Added by Archana K. on 17/05/13 for Bug-7899
            }

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
            this.DtpReqDt.Enabled = false;
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
            if (this.pAddMode == false && dsMain.Tables[0].Rows.Count > 0)
            {
                vTran_Cd = Convert.ToInt16(dsMain.Tables[0].Rows[0]["Tran_Cd"]);
            }
            else
            {
                vTran_Cd = 0;
            }
            if (this.dsMain.Tables[0].Rows.Count > 0)
            {
                this.dtpRefDt.Value = Convert.ToDateTime(this.dsMain.Tables[0].Rows[0]["Date"]).Date;  /*Rupesh 22/11/12*/
                this.DtpDedDate.Value = Convert.ToDateTime(this.dsMain.Tables[0].Rows[0]["Ded_Date"]).Date;
                this.DtpChequeDt.Value = Convert.ToDateTime(this.dsMain.Tables[0].Rows[0]["Cheque_Dt"]).Date;
                this.dtpApproveDt.Value = Convert.ToDateTime(this.dsMain.Tables[0].Rows[0]["AppDate"]).Date; /*Ramya 04/03/13*/
                //this.DtpDedDate.DataBindings.Add("Text", dsMain.Tables[0], "Ded_Date");
                // this.DtpChequeDt.DataBindings.Add("Text", dsMain.Tables[0], "Cheque_Dt");

                SqlStr = "Select EmployeeName=(case when isnull(b.pMailName,'')='' then b.EmployeeName Else b.pMailName end),a.EmployeeCode";
                SqlStr = SqlStr + " ,a.Inv_No,a.Date,Amount";
                SqlStr = SqlStr + " ,C.Head_Nm,c.Short_Nm";
                //SqlStr =SqlStr +" ,a.Fld_Nm";
                SqlStr = SqlStr + " From Emp_Loan_Advance_Request a";
                SqlStr = SqlStr + " inner join EmployeeMast b on (a.EmployeeCode=b.EmployeeCode)";
                SqlStr = SqlStr + " inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)";
                SqlStr = SqlStr + " Where a.Inv_No='" + this.txtAppNo.Text + "' and a.EmployeeCode='" + this.txtEmpCode.Text + "'";
                tblEmp = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (tblEmp.Rows.Count > 0)
                {
                    this.txtEmpNm.Text = tblEmp.Rows[0]["EmployeeName"].ToString();
                    this.DtpReqDt.Text = tblEmp.Rows[0]["Date"].ToString();
                    this.ntxtAmount.Text = tblEmp.Rows[0]["Amount"].ToString();
                    this.txtPayHead.Text = tblEmp.Rows[0]["Head_Nm"].ToString();
                    this.txtShortNm.Text = tblEmp.Rows[0]["Short_Nm"].ToString();
                }
                SqlStr = "Select Ac_Name From Ac_Mast Master Where Ac_id=" + dsMain.Tables[0].Rows[0]["Bank_Id"].ToString();
                tblPayHead = new DataTable();
                tblEmp = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (tblEmp.Rows.Count > 0)
                {
                    this.txtBankNm.Text = tblEmp.Rows[0]["Ac_Name"].ToString();
                }
                SqlStr = "Select EmployeeName=(case when isnull(pMailName,'')='' then EmployeeName Else pMailName end) From EmployeeMast Where EmployeeCode='" + dsMain.Tables[0].Rows[0]["AppByCode"].ToString() + "'";
                tblPayHead = new DataTable();
                tblEmp = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (tblEmp.Rows.Count > 0)
                {
                    this.txtApprovedBy.Text = tblEmp.Rows[0]["EmployeeName"].ToString();
                }

                if (dsMain.Tables[0].Rows[0]["Req_Stat"].ToString() == "A")
                {
                    this.rbtnApproved.Checked = true;
                }
                else if (dsMain.Tables[0].Rows[0]["Req_Stat"].ToString() == "R")
                {
                    this.rbtnRejected.Checked = true;
                }
                else
                {
                    this.rbtnHold.Checked = true;
                }

                if (dsMain.Tables[0].Rows[0]["Loan_Type"].ToString() == "F")
                {
                    this.rbtnFlat.Checked = true;
                }
                else if (dsMain.Tables[0].Rows[0]["Loan_Type"].ToString() == "D")
                {
                    this.rbtnDemnising.Checked = true;
                }
                else
                {
                    this.rbtnNone.Checked = true;
                }
                if (this.pAddMode == false && this.pEditMode == false && vTran_Cd > 0)
                {
                    //SqlStr = "Select *,cMonth=isnull(datename(month,dateadd(month, Pay_Month - 1, 0)),'''') From Emp_Loan_Advance_Details where EmployeeCode='" + this.txtEmpCode.Text + "' and Inv_No='" + this.txtRefNo.Text + "' order by Pay_Year,Pay_Month";
                    SqlStr = "Select *,cMonth=isnull(datename(month,dateadd(month, Pay_Month - 1, 0)),'''') From Emp_Loan_Advance_Details where Tran_cd=" + vTran_Cd + " order by Pay_Year,Pay_Month";
                    dsLoanDet = new DataSet();
                    dsLoanDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthGridRefresh();

                }

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
            DataSet dsTemp = new DataSet();
            string SqlStr = "select top 1  " + vMainField + " as Col1 from " + vMainTblNm + " order by  " + vMainField + " desc";
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
            //Added by Archana K. on 17/05/13 for Bug-7899 start
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPreview.Enabled = true;
                this.btnPrint.Enabled = true;
                this.groupBox1.Enabled = false;
                this.groupBox2.Enabled = false;
                this.groupBox3.Enabled = false;
                this.gbxDetails.Enabled = false;
                this.gbxType.Enabled = false;
                this.gbxChequeDet.Enabled = false;

            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
                this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();



            if (dsMain.Tables[0].Rows.Count == 0)
            {
                this.btnEmail.Enabled = false;
                //this.btnLocate.Enabled = true;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                //this.btnPayHead.Enabled = false;
                //this.btnFldNm.Enabled = false;

            }
            else
            {
                //this.btnHeadNm.Enabled = true;
                //this.btnFldNm.Enabled = true;
            }

            this.dtpApproveDt.Enabled = vEnabled;
            this.DtpChequeDt.Enabled = vEnabled;

            rbtnHold.Enabled = vEnabled;
            rbtnApproved.Enabled = vEnabled;
            rbtnRejected.Enabled = vEnabled;
            dtpRefDt.Enabled = vEnabled;

            btnApprovedBy.Enabled = vEnabled;
            DtpReqDt.Enabled = vEnabled;
            txtRemark.Enabled = vEnabled;

            rbtnNone.Enabled = vEnabled;
            rbtnFlat.Enabled = vEnabled;
            rbtnDemnising.Enabled = vEnabled;

            ntxtLoanAmt.Enabled = vEnabled;
            ntxtPer.Enabled = vEnabled;
            ntxtInst.Enabled = vEnabled;


            txtChequeNo.Enabled = vEnabled;
            DtpDedDate.Enabled = vEnabled;
            txtApprovedBy.Enabled = vEnabled; /*Ramya 27/10/12*/
            txtEmpNm.Enabled = vEnabled;/*Ramya 27/10/12*/
            txtBankNm.Enabled = vEnabled;/*Ramya 01/11/12*/
            dgvLoanDet.ReadOnly = !vEnabled;//Commented by Archana K. on 05/09/13 for Bug-18246   // Un-Commented by Sachin N. S. on 15/10/2013 for Bug-18246
            //dgvLoanDet.ReadOnly = true;//Added by Archana K. on 05/09/13 for Bug-18246    // Commented by Sachin N. S. on 15/10/2013 for Bug-18246
            this.btnRefNo.Enabled = !vEnabled;
            this.btnAppNo.Enabled = vEnabled;
            this.btnApprovedBy.Enabled = vEnabled;
            this.btnBankNm.Enabled = vEnabled;
            this.btnSchedule.Enabled = vEnabled;
            this.txtPayHead.Enabled = vEnabled;  /*Ramya 19/10/12*/
            this.txtAppNo.Enabled = vEnabled;   /*Ramya 19/10/12*/
            this.txtEmpNm.Enabled = vEnabled;   /*Ramya 19/10/12*/
            this.txtEmpCode.Enabled = vEnabled;  /*Ramya 19/10/12*/

            //this.txtAmount.Enabled = vEnabled;
            //this.DtpAppDt.Enabled = vEnabled;
            //this.txtRemark.Enabled = vEnabled;
            if (this.pAddMode == false)
            {
                this.btnAppNo.Enabled = false;
                //this.btnSchedule.Enabled = false;
            }

        }
        private void mthBindClear()
        {
            this.txtAppNo.DataBindings.Clear();

            rbtnNone.Checked = false;
            rbtnFlat.Checked = false;
            rbtnDemnising.Checked = false;

            rbtnHold.Checked = false;
            rbtnApproved.Checked = false;
            rbtnRejected.Checked = false;
            this.ntxtDue_Amt.DataBindings.Clear();
            this.txtRefNo.DataBindings.Clear();
            this.dtpRefDt.DataBindings.Clear();
            this.txtEmpCode.DataBindings.Clear();

            this.DtpReqDt.DataBindings.Clear();
            this.txtRemark.DataBindings.Clear();



            this.ntxtLoanAmt.DataBindings.Clear();
            this.ntxtPer.DataBindings.Clear();
            this.ntxtInst.DataBindings.Clear();
            this.DtpDedDate.DataBindings.Clear();

            this.txtChequeNo.DataBindings.Clear();
            this.DtpChequeDt.DataBindings.Clear();

            vReq_Id = 0;
            //this.DtpAppDt.Value =null;
            this.txtEmpNm.Text = "";
            this.txtPayHead.Text = "";
            this.txtShortNm.Text = "";
            this.ntxtAmount.Text = "0";
            this.txtApprovedBy.Text = "";
            this.txtBankNm.Text = "";


        }
        private void mthBindData()
        {
            //Req_Stat
            //Loan_Type
            //Req_Amt
            //AppByCode
            //Req_Date
            //Bank_id

            this.txtAppNo.DataBindings.Add("Text", dsMain.Tables[0], "Req_No");
            this.txtEmpCode.DataBindings.Add("Text", dsMain.Tables[0], "EmployeeCode");
            if (dsMain.Tables[0].Rows.Count > 0 && this.pAddMode == false)
            {
                vReq_Id = Convert.ToInt16(dsMain.Tables[0].Rows[0]["Req_No"]);
            }

            this.txtRefNo.DataBindings.Add("Text", dsMain.Tables[0], "Inv_No");
            //if (dsMain.Tables[0] .Columns["Date"].ToString()!= null)
            //{
            //this.dtpRefDt.DataBindings.Add("Text", dsMain.Tables[0], "Date");  /*Ramya*/
            //}
            this.ntxtDue_Amt.DataBindings.Add("Text", dsMain.Tables[0], "Due_Amt");

            this.DtpReqDt.DataBindings.Add("Text", dsMain.Tables[0], "Req_Date");
            this.txtRemark.DataBindings.Add("Text", dsMain.Tables[0], "Remark");



            this.ntxtLoanAmt.DataBindings.Add("Text", dsMain.Tables[0], "Loan_Amt");
            this.ntxtPer.DataBindings.Add("Text", dsMain.Tables[0], "Int_Per");
            this.ntxtInst.DataBindings.Add("Text", dsMain.Tables[0], "InstNo");
            //this.DtpDedDate.DataBindings.Add("Text", dsMain.Tables[0], "Ded_Date");
            // this.DtpChequeDt.DataBindings.Add("Text", dsMain.Tables[0], "Cheque_Dt");
            this.txtChequeNo.DataBindings.Add("Text", dsMain.Tables[0], "Cheque_No");



        }
        #endregion

        #region Search and Help Buttons
        private void btnApprovedBy_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select Distinct EmployeeName=(case when isnull(pMailName,'')='' then EmployeeName Else pMailName end), EmployeeCode from EmployeeMast Where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' and EmployeeCode<>'" + this.txtEmpCode.Text + "' order by EmployeeCode ,(case when isnull(pMailName,'')='' then EmployeeName Else pMailName end) ";
            VForText = "Select Employee Name";
            vSearchCol = "EmployeeName";
            vDisplayColumnList = "EmployeeName:Employee Name,Short_Nm:Short Name,EmployeeCode:Employee Code";
            vReturnCol = "EmployeeName,EmployeeCode";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            DataView dvw = tDs.Tables[0].DefaultView;

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)     /*Ramya 27/10/12*/
            {
                this.txtApprovedBy.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["AppByCode"] = oSelectPop.pReturnArray[1];
            }
        }

        private void btnBankNm_Click(object sender, EventArgs e)
        {
            //Select Ac_Name,Ac_Id From Ac_Mast where typ='BANK' order by Ac_name
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select Ac_Name,MailName,Ac_Id From Ac_Mast where typ='BANK'  order by Ac_name";
            VForText = "Select Bank Name";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Bank Name,MailName:Mail Name";
            vReturnCol = "Ac_Name,Ac_Id";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            DataView dvw = tDs.Tables[0].DefaultView;

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)     /*Ramya 27/10/12*/
            {
                this.txtBankNm.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["Bank_Id"] = oSelectPop.pReturnArray[1];
            }
        }
        private void btnApplNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            /*Ramya changed the order of columns 27/02/13*/
            SqlStr = "Select a.Inv_No,a.Date,EmployeeName=(case when isnull(b.pMailName,'')='' then b.EmployeeName Else b.pMailName end),a.EmployeeCode";
            SqlStr = SqlStr + " ,Amount";
            SqlStr = SqlStr + " ,C.Head_Nm,c.Short_Nm,Req_Id=a.ID,a.Fld_Nm";
            //SqlStr =SqlStr +" ,a.Fld_Nm";
            SqlStr = SqlStr + " From Emp_Loan_Advance_Request a";
            SqlStr = SqlStr + " inner join EmployeeMast b on (a.EmployeeCode=b.EmployeeCode)";
            SqlStr = SqlStr + " inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)";
            SqlStr = SqlStr + " Where a.inv_no not in (Select distinct Req_No from Emp_Loan_Advance)";
            SqlStr = SqlStr + " order by A.EmployeeCode,a.Inv_No,c.Head_Nm";

            VForText = "Select  Request No.";
            //vSearchCol = "Inv_No,EmployeeCode,EmployeeName,Date,Amount,Head_Nm,Short_Nm,Req_Id";
            vSearchCol = "Inv_No";
            vDisplayColumnList = "Inv_No:Request No,EmployeeCode:Employee Code,EmployeeName:Employee Name,Date:Request Date,Amount:Amount,Head_Nm:Pay Head Name";
            vReturnCol = "Inv_No,EmployeeName,EmployeeCode,Date,Amount,Head_Nm,Short_Nm,Req_Id,Fld_Nm";

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
                this.txtAppNo.Text = oSelectPop.pReturnArray[0];
                this.txtEmpNm.Text = oSelectPop.pReturnArray[1];
                this.txtEmpCode.Text = oSelectPop.pReturnArray[2];
                this.DtpReqDt.Text = oSelectPop.pReturnArray[3];
                this.ntxtAmount.Text = oSelectPop.pReturnArray[4];
                this.txtPayHead.Text = oSelectPop.pReturnArray[5];
                this.txtShortNm.Text = oSelectPop.pReturnArray[6];
                //vReq_Id = oSelectPop.pReturnArray[7];

                dsMain.Tables[0].Rows[0]["Req_No"] = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["EmployeeCode"] = oSelectPop.pReturnArray[2];
                dsMain.Tables[0].Rows[0]["Req_ID"] = oSelectPop.pReturnArray[7];
                dsMain.Tables[0].Rows[0]["Fld_Nm"] = oSelectPop.pReturnArray[8];
            }
        }

        #endregion

        #region Scedule
        private void btnSchedule_Click(object sender, EventArgs e)
        {
            int cnt = Convert.ToInt16(this.ntxtInst.Text);
            Decimal vActualBalance = Convert.ToDecimal(this.ntxtLoanAmt.Text), vDueAmt = 0;
            int vNoInst = Convert.ToInt16(this.ntxtInst.Text);
            DateTime tDate = this.DtpDedDate.Value;
            Decimal vOpBal = 0, vInstallment = 0, vInterest = 0, vTotAmount = 0, vProj_Repay = 0, vRepay_Amt = 0, vCl_Bal = 0;
            int PayGenMonth = 0;
            string PayGenYear = "";
            cValid = true;
            if (rbtnApproved.Checked == false)     /*changed from mchkvalidation*/
            {
                MessageBox.Show("Please Approve the Application", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.rbtnApproved.Focus();
                cValid = false;
                return;
            }

            this.mthChkValidation();
            if (cValid == false)
            {
                return;
            }
            //if (this.pAddMode && dsLoanDet.Tables.Count > 0)
            //{
            //    dsLoanDet.Tables[0].Rows.Clear();
            //}
            //if (this.pEditMode && dsLoanDet.Tables.Count > 0 && this.DtpDedDate.Enabled == true)
            //{
            //    dsLoanDet.Tables[0].Rows.Clear();
            //}


            if (dsLoanDet.Tables.Count > 0)
            {
                //vActualBalance = Convert.ToDecimal(this.ntxtDue_Amt.Text);
                vDueAmt = Convert.ToDecimal(this.ntxtDue_Amt.Text);
                vDueAmt = Convert.ToDecimal(this.ntxtLoanAmt.Text);
                //vDueAmt = 5020;
                vActualBalance = vDueAmt;
                for (int i = 0; i <= dsLoanDet.Tables[0].Rows.Count - 1; i++)
                {
                    //if (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Repay_Amt"]) > 0 || Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Proj_RePay"]) == 0)
                    if (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Repay_Amt"]) > 0 && Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Proj_RePay"]) == 0)   // Changed by Sachin N. S. on 28/10/2013 for Bug-18246
                    {
                        //vActualBalance = dsLoanDet.Tables[0].Rows[i]["Cl_Bal"].ToString();
                        //vActualBalance = vActualBalance - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Repay_Amt"]) ;
                        vNoInst = vNoInst - 1;
                        PayGenYear = dsLoanDet.Tables[0].Rows[i]["Pay_Year"].ToString();
                        PayGenMonth = Convert.ToInt16(dsLoanDet.Tables[0].Rows[i]["Pay_Month"]);
                        tDate = tDate.AddMonths(1);
                        vDueAmt = vDueAmt + Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Interest"]) - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Repay_Amt"]);
                    }
                    else
                    {
                        if (dsLoanDet.Tables[0].Rows[i]["ID"].ToString() != "")//Added by Archana K. on 17/09/13 for Bug-18246
                        {
                            DataRow rTblDel = tblDelDet.NewRow();
                            rTblDel["DelId"] = dsLoanDet.Tables[0].Rows[i]["ID"];
                            tblDelDet.Rows.Add(rTblDel);
                        }
                        //Added by Sachin N. S. on 17/09/13 for Bug-18246 -- Start
                        if (dsLoanDet.Tables[0].Rows[i]["ID"].ToString() == "") 
                        {
                            dsLoanDet.Tables[0].Rows.RemoveAt(i);
                            i = i - 1;
                        }
                        //Added by Sachin N. S. on 17/09/13 for Bug-18246 -- End

                        //dsLoanDet.Tables[0].Rows.RemoveAt(i);     // Commented by Sachin N. S. on 17/09/13 for Bug-18246
                        //vDueAmt = vDueAmt - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Interest"]) ;
                        //i = i - 1;                                  // Commented by Sachin N. S. on 17/09/13 for Bug-18246
                        //vDueAmt=vDueAmt+ Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Proj_RePay"]);
                        //vActualBalance = vActualBalance - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Proj_RePay"]) - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["interest"]);

                    }
                }
                //vDueAmt = vActualBalance;
                vActualBalance = vDueAmt;
            }
            else
            {
                vActualBalance = Convert.ToDecimal(this.ntxtLoanAmt.Text);
            }
            if (vNoInst <= 0)
            {
                if (vActualBalance > 0)     // Added by Sachin N. S. on 10/12/2013 for Bug-18246 -- Start
                {
                    MessageBox.Show("Balance amount still pending. Cannot continue...!!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    if (dsLoanDet.Tables[0].Rows.Cast<DataRow>().Where(r => Convert.ToInt16(r["Id"]) != 0 && Convert.ToDecimal(r["Repay_Amt"]) == 0).Count() > 0)
                    {
                        this.ntxtInst.Text = (Convert.ToInt16(this.ntxtInst.Text) + 1).ToString();
                        return;
                    }
                    else
                    {
                        vNoInst += 1;
                        this.ntxtInst.Text = (Convert.ToInt16(this.ntxtInst.Text) + 1).ToString();
                    }
                }                           // Added by Sachin N. S. on 10/12/2013 for Bug-18246 -- End
                else
                {
                    //**** Added by Sachin N. S. on 21/10/2013 for Bug-18246 -- Start ****//
                    if (tblDelDet.Rows.Count > 0)
                    {
                        int _cId = 0;
                        foreach (DataRow _dr in tblDelDet.Rows)
                        {
                            _cId = Convert.ToInt16(_dr[0]);
                            dsLoanDet.Tables[0].Rows.Cast<DataRow>().Where(r => Convert.ToInt16(r["Id"]) == _cId).ToList().ForEach(r => r.Delete());
                            dsLoanDet.Tables[0].AcceptChanges();
                        }
                    }
                    //**** Added by Sachin N. S. on 21/10/2013 for Bug-18246 -- End ****//
                    return;
                }
            }
            else
            {
                //**** Added by Sachin N. S. on 21/10/2013 for Bug-18246 -- Start ****//
                if (vActualBalance <= 0)
                {
                    MessageBox.Show("No loan balance is pending. Cannot continue...!!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.ntxtInst.Text = (dsLoanDet.Tables[0].Rows.Count).ToString();
                    return;
                }
                else
                {
                    if (tblDelDet.Rows.Count > 0)
                    {
                        int _cId = 0;
                        foreach (DataRow _dr in tblDelDet.Rows)
                        {
                            _cId = Convert.ToInt16(_dr[0]);
                            dsLoanDet.Tables[0].Rows.Cast<DataRow>().Where(r => Convert.ToInt16(r["Id"]) == _cId).ToList().ForEach(r => r.Delete());
                            dsLoanDet.Tables[0].AcceptChanges();
                        }
                    }
                }
                //**** Added by Sachin N. S. on 21/10/2013 for Bug-18246 -- End ****//
            }

            //vInstallment = vActualBalance / Convert.ToDecimal(vNoInst);       //**** Commented by Sachin N. S. on 21/10/2013 for Bug-18246
            vInstallment = Math.Round(vActualBalance / Convert.ToDecimal(vNoInst), 2);     //**** Changed by Sachin N. S. on 21/10/2013 for Bug-18246


            vOpBal = vActualBalance;
            //this.mthGridRefresh();
            if (dsLoanDet.Tables.Count == 0)
            {
                dsLoanDet = new DataSet();
                SqlStr = "Select *,cMonth=isnull(datename(month,dateadd(month, Pay_Month - 1, 0)),'''') From Emp_Loan_Advance_Details where EmployeeCode='" + this.txtEmpCode.Text + "' and Inv_No='" + this.txtRefNo.Text + "' order by Pay_Year,Pay_Month";
                dsLoanDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            for (int i = 1; i <= vNoInst; i++)
            {
                vInterest = 0;

                DataRow tDr = dsLoanDet.Tables[0].NewRow();
                tDr["EmployeeCode"] = this.txtEmpCode.Text;
                tDr["Inv_No"] = this.txtRefNo.Text;
                tDr["Pay_Year"] = tDate.Year.ToString();
                tDr["Pay_Month"] = tDate.Month.ToString();
                tDr["cMonth"] = fnCMonth(tDate.Month);
                tDr["Op_Bal"] = vOpBal;
                //Commented by Archana K. on 10/09/13 for Bug-18246 start
                //if (i == vNoInst && vCl_Bal != 0)
                //{
                //    vInstallment = vCl_Bal;
                //}
                //Commented by Archana K. on 10/09/13 for Bug-18246 end

                vInstallment = (i == vNoInst ? vActualBalance - (vInstallment * (i - 1)) : vInstallment);   //*** Added by Sachin N. S. on 21/10/2013 for Bug-18246

                tDr["Inst_Amt"] = vInstallment;
                if (this.rbtnNone.Checked)
                {
                    vInterest = 0;
                }
                else if (this.rbtnFlat.Checked)
                {
                    vInterest = Decimal.Round((vInstallment * (Decimal.Round(Convert.ToDecimal(this.ntxtPer.Text) / 12, 2))) / 100, 2);
                }
                else if (this.rbtnDemnising.Checked)
                {
                    vInterest = Decimal.Round((vOpBal * (Decimal.Round(Convert.ToDecimal(this.ntxtPer.Text) / 12, 2))) / 100, 2);
                }
                tDr["Interest"] = vInterest;
                tDr["Tot_Amt"] = vInstallment + vInterest;
                tDr["Repay_Amt"] = vRepay_Amt;

                if (vRepay_Amt == 0)
                {
                    vProj_Repay = vInstallment + vInterest;
                }

                tDr["Proj_Repay"] = vProj_Repay;
                //vCl_Bal = vOpBal + vInterest - vProj_Repay;//Commented by Archana K. on 06/05/13 for Bug-18246
                vCl_Bal = vOpBal + vInterest - vRepay_Amt;//Changed by Archana K. on 06/05/13 for Bug-18246
                tDr["Cl_Bal"] = vCl_Bal;
                this.dsLoanDet.Tables[0].Rows.Add(tDr);
                vOpBal = vCl_Bal;
                tDate = tDate.AddMonths(1);
                //vDueAmt = vDueAmt + vInstallment + vInterest-vProj_Repay;
                vDueAmt = vDueAmt + vInterest;
            }

            this.dsMain.Tables[0].Rows[0]["Due_Amt"] = vDueAmt;
            this.ntxtDue_Amt.Text = vDueAmt.ToString();
            //this.ntxtDue_Amt.Text = vDueAmt.ToString();
            this.mthGridRefresh();
            //}
        }
        private void btnSchedule_Click_Old(object sender, EventArgs e)
        {
            int cnt = Convert.ToInt16(this.ntxtInst.Text);
            Decimal vActualBalance = Convert.ToDecimal(this.ntxtLoanAmt.Text);
            DateTime tDate = this.DtpDedDate.Value;
            Decimal vOpBal = 0, vInstallment = 0, vInterest = 0, vTotAmount = 0, vProj_Repay = 0, vRepay_Amt = 0, vCl_Bal = 0;
            cValid = true;
            if (rbtnApproved.Checked == false)     /*changed from mchkvalidation*/
            {
                MessageBox.Show("Please Approve the Application", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.rbtnApproved.Focus();
                cValid = false;
                return;
            }

            this.mthChkValidation();
            if (cValid == false)
            {
                return;
            }
            if (this.pAddMode && dsLoanDet.Tables.Count > 0)
            {
                dsLoanDet.Tables[0].Rows.Clear();
            }
            if (this.pEditMode && dsLoanDet.Tables.Count > 0 && this.DtpDedDate.Enabled == true)
            {
                dsLoanDet.Tables[0].Rows.Clear();
            }

            //if (dsLoanDet.Tables.Count > 0)
            //{
            //    if (this.pEditMode)
            //    {
            //        for (int i = 0; i <= dsLoanDet.Tables[0].Rows.Count - 1; i++)
            //        {
            //            if (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Repay_Amt"]) == 0)
            //            {
            //                dsLoanDet.Tables[0].Rows.RemoveAt(i);
            //                i = i - 1;
            //            }
            //            else
            //            {
            //                vActualBalance = vActualBalance - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["Repay_Amt"]) - Convert.ToDecimal(dsLoanDet.Tables[0].Rows[i]["interest"]);
            //            }
            //        }
            //    }
            //    cnt = cnt - dsLoanDet.Tables[0].Rows.Count;
            //}
            this.mthGridRefresh();

            //if (dsLoanDet.Tables[0].Rows.Count == 0)
            //{
            //vInstallment = Convert.ToDecimal(this.ntxtLoanAmt.Text) / Convert.ToDecimal(this.ntxtInst.Text);
            Decimal vDueAmt = 0;
            vInstallment = vActualBalance / Convert.ToDecimal(cnt);
            vOpBal = Convert.ToDecimal(this.ntxtLoanAmt.Text);
            for (int i = 1; i <= cnt; i++)
            {
                vInterest = 0;

                DataRow tDr = dsLoanDet.Tables[0].NewRow();
                tDr["EmployeeCode"] = this.txtEmpCode.Text;
                tDr["Inv_No"] = this.txtRefNo.Text;
                tDr["Pay_Year"] = tDate.Year.ToString();
                tDr["Pay_Month"] = tDate.Month.ToString();
                tDr["cMonth"] = fnCMonth(tDate.Month);
                tDr["Op_Bal"] = vOpBal;
                if (i == cnt)
                {
                    vInstallment = vCl_Bal;
                }
                tDr["Inst_Amt"] = vInstallment;
                if (this.rbtnNone.Checked)
                {
                    vInterest = 0;
                }
                else if (this.rbtnFlat.Checked)
                {
                    vInterest = Decimal.Round((vInstallment * (Decimal.Round(Convert.ToDecimal(this.ntxtPer.Text) / 12, 2))) / 100, 2);
                }
                else if (this.rbtnDemnising.Checked)
                {
                    vInterest = Decimal.Round((vOpBal * (Decimal.Round(Convert.ToDecimal(this.ntxtPer.Text) / 12, 2))) / 100, 2);
                }
                tDr["Interest"] = vInterest;
                tDr["Tot_Amt"] = vInstallment + vInterest;
                tDr["Repay_Amt"] = vRepay_Amt;
                if (vRepay_Amt == 0)
                {
                    vProj_Repay = vInstallment + vInterest;
                }
                tDr["Proj_Repay"] = vProj_Repay;
                vCl_Bal = vOpBal + vInterest - vProj_Repay;
                tDr["Cl_Bal"] = vCl_Bal;
                this.dsLoanDet.Tables[0].Rows.Add(tDr);
                vOpBal = vCl_Bal;
                tDate = tDate.AddMonths(1);
                vDueAmt = vDueAmt + vInstallment + vInterest;
            }

            this.dsMain.Tables[0].Rows[0]["Due_Amt"] = vDueAmt;
            this.ntxtDue_Amt.Text = vDueAmt.ToString();
            //this.ntxtDue_Amt.Text = vDueAmt.ToString();
            this.mthGridRefresh();
            //}
        }
        private void mthGridRefresh()
        {

            //if (dsLoanDet == null)
            if (dsLoanDet.Tables.Count == 0)
            {
                dsLoanDet = new DataSet();
                SqlStr = "Select *,cMonth=isnull(datename(month,dateadd(month, Pay_Month - 1, 0)),'''') From Emp_Loan_Advance_Details where EmployeeCode='" + this.txtEmpCode.Text + "' and Inv_No='" + this.txtRefNo.Text + "' order by Pay_Year,Pay_Month";
                dsLoanDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            this.dgvLoanDet.Columns.Clear();
            this.dgvLoanDet.DataSource = this.dsLoanDet.Tables[0];
            this.dgvLoanDet.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colId.Name = "colId";
            colId.DataPropertyName = "Id";
            this.dgvLoanDet.Columns.Add(colId);
            colId.Visible = false;

            System.Windows.Forms.DataGridViewTextBoxColumn colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colYear.HeaderText = "Year";
            colYear.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colYear.Name = "colYear";
            colYear.DataPropertyName = "Pay_Year";
            //colYear.DefaultCellStyle.BackColor = Color.LightGray;
            colYear.Width = 45;
            this.dgvLoanDet.Columns.Add(colYear);

            System.Windows.Forms.DataGridViewTextBoxColumn colcMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colcMonth.HeaderText = "Month";
            colcMonth.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colcMonth.Name = "colcMonth";
            colcMonth.DataPropertyName = "cMonth";
            //colcMonth.DefaultCellStyle.BackColor = Color.LightGray;
            colcMonth.Width = 80;
            this.dgvLoanDet.Columns.Add(colcMonth);
            colcMonth.Frozen = true;
            colcMonth.ReadOnly = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colMonth.HeaderText = "Month";
            colMonth.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMonth.Name = "colMonth";
            colMonth.DataPropertyName = "Pay_Month";
            colMonth.Visible = false;
            this.dgvLoanDet.Columns.Add(colMonth);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colOpBal = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colOpBal.HeaderText = "Opening";
            colOpBal.Name = "colOpBal";
            colOpBal.DataPropertyName = "Op_Bal";
            //colOpBal.DefaultCellStyle.BackColor = Color.LightGray;
            colOpBal.Width = 80;
            colOpBal.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colOpBal.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colOpBal);


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colInstallment = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colInstallment.HeaderText = "Installment";
            colInstallment.Name = "colInstallment";
            colInstallment.DataPropertyName = "Inst_Amt";
            //colInstallment.DefaultCellStyle.BackColor = Color.LightGray;
            colInstallment.Width = 90;
            colInstallment.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colInstallment.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colInstallment);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colInterest = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colInterest.HeaderText = "Interest";
            colInterest.Name = "colInterest";
            colInterest.DataPropertyName = "Interest";
            //colInterest.DefaultCellStyle.BackColor = Color.LightGray;
            colInterest.Width = 90;
            colInterest.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colInterest.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colInterest);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colTotAmt = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colTotAmt.HeaderText = "Total Amount";
            colTotAmt.Name = "colTotAmt";
            colTotAmt.DataPropertyName = "Tot_Amt";
            //colTotAmt.DefaultCellStyle.BackColor = Color.LightGray;
            colTotAmt.Width = 100;
            colTotAmt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTotAmt.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colTotAmt);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colProjRepay_Amt = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colProjRepay_Amt.HeaderText = "Repayment(Proj.)";
            colProjRepay_Amt.Name = "colProjRepay_Amt";
            colProjRepay_Amt.DataPropertyName = "Proj_Repay";
            //colRepay_Amt.DefaultCellStyle.BackColor = Color.LightGray;
            colProjRepay_Amt.Width = 100;
            colProjRepay_Amt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colProjRepay_Amt.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colProjRepay_Amt);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colRepay_Amt = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colRepay_Amt.HeaderText = "Repayment";
            colRepay_Amt.Name = "colAmt_Repay";
            colRepay_Amt.DataPropertyName = "Repay_Amt";
            //colRepay_Amt.DefaultCellStyle.BackColor = Color.LightGray;
            colRepay_Amt.Width = 100;
            colRepay_Amt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colRepay_Amt.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colRepay_Amt);


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colClBal = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colClBal.HeaderText = "Balance";
            colClBal.Name = "colClBal";
            colClBal.DataPropertyName = "Cl_Bal";
            //colClBal.DefaultCellStyle.BackColor = Color.LightGray;
            colClBal.Width = 100;
            colClBal.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colClBal.DecimalLength = 2;
            this.dgvLoanDet.Columns.Add(colClBal);

            //colGrossPayment.Frozen = true;

            //colYear.Frozen = true;
        }
        #endregion

        private void btnAppNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select a.Inv_No,EmployeeName=(case when isnull(b.pMailName,'')='' then b.EmployeeName Else b.pMailName end),a.EmployeeCode";
            SqlStr = SqlStr + " ,a.Date";
            //SqlStr = SqlStr + " From Emp_Loan_Advance_Request a";
            SqlStr = SqlStr + " From Emp_Loan_Advance a";  /*Ramya 28/5/13*/
            SqlStr = SqlStr + " inner join EmployeeMast b on (a.EmployeeCode=b.EmployeeCode)";

            VForText = "Select Application No. and Employee Name";
            vSearchCol = "Inv_No";
            vDisplayColumnList = "Inv_No:Application No,EmployeeCode:Employee Code,EmployeeName:Employee Name";
            //vReturnCol = "EmployeeName,Inv_No";  
            vReturnCol = "EmployeeName,Inv_No,EmployeeCode";/*Ramya 28/5/13*/



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

                //SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "' and EmployeeCode='" + oSelectPop.pReturnArray[2] + "'";  /*Ramya 28/5/13*/
                dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                this.mthView();
                this.mthChkNavigationButton();
                this.mthEnableDisableFormControls();
            }

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


        private void txtAppNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnApplNo_Click(sender, new EventArgs());
                }
            }
        }

        private void frmLoanDetails_FormClosed(object sender, FormClosedEventArgs e)
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

            string vRepGroup = "Loan and Advance Details";
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
            oPrint.pSpPara = dsMain.Tables[0].Rows[0]["Tran_Cd"].ToString();
            oPrint.pPrintOption = vPrintOption;
            oPrint.Main();
            if (vPrintOption != 2) /*Ramya 14/11/12*/
            {
                timer1.Enabled = true; /*Ramya 01/11/12*/
                timer1.Interval = 1000;
                MessageBox.Show("Loan Details Generated", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

        private void DtpDedDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DtpDedDate_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.DtpDedDate.Value.ToString()) && this.pEditMode)   /*Ramya 01/11/12*/
            {
                DateTime vDate = this.DtpDedDate.Value;
                SqlStr = "Set DateFormat dmy Select minDate=Min(Cast('01/'+Cast(Pay_Month  as Varchar)+'/'+Cast(Pay_Year as Varchar) as smalldatetime)) from Emp_Loan_Advance_Details Where Tran_Cd=" + vTran_Cd.ToString() + " and RePay_Amt<>0";
                DataSet tds = new DataSet();
                tds = oDataAccess.GetDataSet(SqlStr, null, 30);

                if (tds.Tables[0].Rows.Count > 0)
                {
                    //if (tds.Tables[0].Rows[0]["MinDate"] != null)
                    if (tds.Tables[0].Rows[0]["MinDate"] != DBNull.Value) //Rup 30/11/2012
                    {
                        DateTime tdt = Convert.ToDateTime(tds.Tables[0].Rows[0]["MinDate"]);
                        if (tdt.Month != this.DtpDedDate.Value.Month || tdt.Year != this.DtpDedDate.Value.Year)
                        {
                            MessageBox.Show("Deduction Should be Statrted with Year = " + tdt.Year.ToString().Trim() + " and  Month =" + this.fnCMonth(tdt.Month).Trim() + " Only", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            this.DtpDedDate.Focus();
                            return;
                        }
                    }

                    //if ((Convert.ToDateTime((tds.Tables[0].Rows[0]["minDate"])).Date) < (this.DtpDedDate.Value).Date)
                    //{


                    //}
                }

            }
        }

        private void rbtnHold_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnHold.Checked)
            {
                //this.dtpApproveDt.CustomFormat=" ";
                this.dtpApproveDt.Value = Convert.ToDateTime("01/01/1900").Date;
            }
            else
            {
                // this.dtpApproveDt.CustomFormat = "dd\\MM\\yyyy";
                this.dtpApproveDt.Value = DateTime.Now.Date;
            }
        }

        private void rbtnApproved_CheckedChanged(object sender, EventArgs e)  /*Ramya */
        {
            if (rbtnApproved.Checked)
            {
                lblAppMand.Visible = true;
                lblAppAmtMand.Visible = true;
                lblNoMand.Visible = true;
                lblAppDtMand.Visible = true;
                lblDedDtMand.Visible = true;

            }
            else
            {
                lblAppMand.Visible = false;
                lblAppAmtMand.Visible = false;
                lblNoMand.Visible = false;
                lblAppDtMand.Visible = false;
                lblDedDtMand.Visible = false;
            }

        }

        private void rbtnRejected_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnRejected.Checked)
            {
                lblAppMand.Visible = true;
                this.dtpApproveDt.Value = Convert.ToDateTime("01/01/1900").Date;
                // this.dtpApproveDt.CustomFormat = "dd\\MM\\yyyy";

            }
            else
            {
                lblAppMand.Visible = false;
                // this.dtpApproveDt.CustomFormat = "dd\\MM\\yyyy";
                this.dtpApproveDt.Value = DateTime.Now.Date;
            }


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
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }

        private void txtApprovedBy_KeyDown(object sender, KeyEventArgs e) /*Ramya 27/02/13*/
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnApprovedBy_Click(sender, new EventArgs());
                }
            }
        }

        private void txtBankNm_KeyDown(object sender, KeyEventArgs e)  /*Ramya 27/02/13*/
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnBankNm_Click(sender, new EventArgs());
                }
            }
        }

        private void rbtnFlat_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnFlat.Checked)
            {
                lblIntMand.Visible = true;
            }
            else
            {
                lblIntMand.Visible = false;
            }

        }

        private void rbtnDemnising_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDemnising.Checked)
            {
                lblIntMand.Visible = true;
            }
            else
            {
                lblIntMand.Visible = false;
            }
        }

        //**** Added by Sachin N. S. on 21/10/2013 for Bug-18246 -- Start ****//
        private void dgvLoanDet_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                if (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[e.RowIndex]["Repay_Amt"]) == 0)
                {
                    if (e.RowIndex == 0)
                    {
                        ResetSchedule(e.RowIndex, Convert.ToDecimal(e.FormattedValue));
                    }
                    else
                    {
                        if (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[e.RowIndex]["Proj_Repay"]) != Convert.ToDecimal(e.FormattedValue))
                        {
                            if (e.RowIndex == dsLoanDet.Tables[0].Rows.Count - 1)
                            {
                                MessageBox.Show("The installment number has reached the maximum.\n Increase the No. of Installments to change the Repayment(Proj.).", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dsLoanDet.Tables[0].Rows[e.RowIndex]["Proj_Repay"] = Convert.ToDecimal(dsLoanDet.Tables[0].Rows[e.RowIndex]["Tot_Amt"]);
                            }
                            else if (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[e.RowIndex - 1]["Proj_Repay"]) > 0)
                            {
                                MessageBox.Show("The payroll for the previous month is not generated. Cannot schedule...!!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dsLoanDet.Tables[0].Rows[e.RowIndex]["Proj_Repay"] = Convert.ToDecimal(dsLoanDet.Tables[0].Rows[e.RowIndex]["Tot_Amt"]);
                            }
                            else
                            {
                                ResetSchedule(e.RowIndex, Convert.ToDecimal(e.FormattedValue));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The payroll for this month is already generated. Cannot schedule...!!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dsLoanDet.Tables[0].Rows[e.RowIndex]["Proj_Repay"] = 0;
                }
            }
        }

        private void ResetSchedule(int _nRowNo, decimal _nProjAmt)
        {
            int _nIntlmnt = dsLoanDet.Tables[0].Rows.Count - (_nRowNo + 1);
            decimal _nProjAmtMnthly = (Convert.ToDecimal(dsLoanDet.Tables[0].Rows[_nRowNo + 1]["Op_Bal"]) - _nProjAmt) / _nIntlmnt;
            for (int i = _nRowNo + 1; i < dsLoanDet.Tables[0].Rows.Count; i++)
            {
                dsLoanDet.Tables[0].Rows[i]["Inst_amt"] = _nProjAmtMnthly;
                dsLoanDet.Tables[0].Rows[i]["Tot_amt"] = _nProjAmtMnthly;
                dsLoanDet.Tables[0].Rows[i]["Proj_Repay"] = _nProjAmtMnthly;
            }
        }
        //**** Added by Sachin N. S. on 21/10/2013 for Bug-18246 -- End ****//

    }
}
