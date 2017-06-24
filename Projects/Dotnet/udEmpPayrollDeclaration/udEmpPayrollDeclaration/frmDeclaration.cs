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

namespace udEmpInvestmentDeclaration
{
    public partial class frmDeclaration : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet tdsLoc;
        string SqlStr = string.Empty;
        string vMainTblNm = "Emp_Payroll_Declaration_Details i inner join EmployeeMast e on (i.EmployeeCode=e.EmployeeCode)", vMainField1 = "pMailName", vMainField2 = "Pay_Year", vMainFldVal1 = "", vMainFldVal2 = "";
        string vFormula = string.Empty;
        String cAppPId, cAppName;
        uBaseForm.FrmBaseForm vParentForm;
        bool cValid = true;
        DataTable tblOthSal,tbl80c, tblOthDed,tblGrossSal,tblLessAllow,tblDed16and17;
        DataSet ds80c;
        string fyear=string.Empty;
        short vTimeOut = 25;
        bool EditCancel = false;
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end
        public frmDeclaration(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Payroll Declaration";
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

        private void frmDeclaration_Load(object sender, EventArgs e)
        {
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;
            this.btnEmail.Enabled = false;

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
          
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.SetMenuRights();
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
            this.mInsertProcessIdRecord();
            this.SetFormColor();
            
            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            //string fName = appPath + @"\bmp\loc-on.gif";
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnEmpNm.Image = Image.FromFile(fName);
                this.btnEmpCode.Image = Image.FromFile(fName);
                this.btnYear.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
              //  this.btnYear.Image = Image.FromFile(fName);
            }
            this.mthDsCommon();


            SqlStr = " select year(sta_dt),year(end_dt) from vudyog..co_mast where CompId=" + this.pCompId;  /*Ramya 08/10/12*/
            DataTable dt1 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            fyear = dt1.Rows[0][0].ToString() + "-" + dt1.Rows[0][1].ToString();
            this.txtFyear.Text = fyear;           /*Ramya 19/02/13*/
        }

        private void mthDsCommon()
        {
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            SqlStr = "Select * From vudyog..Co_Mast where CompId=" + pPara[0];
            company = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            SqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

        }

        #region Action Button

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;

            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            this.txtYear.Focus();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            this.mcheckCallingApplication();
            cValid = true;
            this.txtYear.Focus();
            string SqlSaveStr = "";
            //this.mthValidation(ref vIsFrmValid);
            this.mthChkValidation();
            if (cValid == false)
            {
                return;
            }
          
            
            this.tbcMain.Refresh();
            this.tbcp80C.Refresh();
            this.tbcpDed16.Refresh();
            this.tbcpGrossSal.Refresh();
            this.tbcpLessAllow.Refresh();
            this.tbcpOtherIncome.Refresh();
            this.tbcpOthDed.Refresh();

            this.tbcMain.SelectedTab = this.tbcpOtherIncome;
            this.txtEmpNm.Focus();
            
            if (this.pAddMode || this.pEditMode)
            {
                SqlSaveStr = "";
                this.mthSaveInsertString(ref SqlSaveStr);
                oDataAccess.BeginTransaction();
                oDataAccess.ExecuteSQLStatement(SqlSaveStr, null, vTimeOut, true);
                oDataAccess.CommitTransaction();
                
            }
            if (this.pAddMode)
            {
                this.dsMain.Tables[0].Rows[0]["EmployeeCode"] = this.txtEmpCode.Text.Trim();
                dsMain.Tables[0].Rows[0]["pMailName"] = this.txtEmpNm.Text.Trim();
            }
            SqlSaveStr = "";
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
          
           
            //vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            //SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthBindClear();
            this.mthBindData();
            this.mthView();
        }

        private void mthChkValidation()
        {
            DataSet tDs = new DataSet();
            SqlStr = "Select EmployeeCode From Emp_Payroll_Declaration_Details where EmployeeCode='" + this.txtEmpCode.Text + "'";
            if (this.pEditMode)
            {
                SqlStr = SqlStr + " and id <>" + dsMain.Tables[0].Rows[0]["Id"].ToString();
            }
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (tDs.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Employee Payroll Details aleady exists" , this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnEmpNm.Focus();
                cValid = false;
                return;
            }

    


        }

        private void mthSaveInsertString(ref string SqlSaveStr)
        {
            String FldList = "", FldVal = "", WhCon = "";
            if (this.pAddMode)
            {
                /*Ramya*/
                foreach (DataRow drt in this.tblGrossSal.Rows)   
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString();
                    FldVal = FldVal + "," + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tblLessAllow.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString();
                    FldVal = FldVal + "," + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tblDed16and17.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString();
                    FldVal = FldVal + "," + drt["AMount"].ToString().Trim();
                } 
                /*Ramya*/

                foreach (DataRow drt in this.tblOthSal.Rows)
                {
                    FldList =FldList+ "," + drt["Fld_Nm"].ToString();
                    FldVal = FldVal+"," + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tbl80c.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString();
                    FldVal = FldVal + "," + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tblOthDed.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString();
                    FldVal = FldVal + "," + drt["AMount"].ToString().Trim();
                }
                SqlSaveStr = "Insert into Emp_Payroll_Declaration_Details (EmployeeCode,Pay_Year" + FldList + ",FinYear) Values('"+this.txtEmpCode.Text.Trim()+"','"+this.txtYear.Text+"'" + FldVal +",'"+this.txtFyear.Text+"')";
            }

            if (this.pEditMode)
            {
                WhCon=" Where EmployeeCode='"+this.txtEmpCode.Text.Trim()+"' and Pay_Year='"+this.txtYear.Text.Trim()+"'";
                /*Ramya*/
                foreach (DataRow drt in this.tblGrossSal.Rows)       
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString() + "=" + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tblLessAllow.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString() + "=" + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tblDed16and17.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString() + "=" + drt["AMount"].ToString().Trim();
                }
                /*Ramya*/
                foreach (DataRow drt in this.tblOthSal.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString() + "=" + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tbl80c.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString() + "=" + drt["AMount"].ToString().Trim();
                }
                foreach (DataRow drt in this.tblOthDed.Rows)
                {
                    FldList = FldList + "," + drt["Fld_Nm"].ToString() + "=" + drt["AMount"].ToString().Trim();
                }
                FldList = FldList.Substring(1, FldList.Length - 1);
                SqlSaveStr = "Update Emp_Payroll_Declaration_Details Set " + FldList + WhCon;
                //(EmployeeCoce,Pay_Year
            }
            

        }
        
        private void mthSaveUpdateString()
        {

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
            //dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();

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
                    EditCancel = true;
                    //this.pAddMode = false;
                    //this.pEditMode = false;
                    //this.mthEnableDisableFormControls();
                    //vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    //SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                    //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
                    //this.mthView();
                    
                }
            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            mthBindClear(); //?

            //this.btnYear.Focus(); Rup
            //this.mthGrdRefresh(); Rup
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthDelete();

        }

        private void mthDelete()
        {
            //if (this.dsMain.Tables[0].Rows.Count <= 0)
            //{
            //    return;
            //}
            //SqlStr = "select distinct Grade from employeemast ";
            //DataSet tDs = new DataSet();
            //tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            //if (tDs.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in tDs.Tables[0].Rows)
            //    {
            //        if (this.txtGradeName.Text.ToUpper().Trim() == dr["Grade"].ToString().ToUpper().Trim())
            //        {

            //            SqlStr = "Could Not Delete Grade " + this.txtGradeName.Text + " . It Is Used In Employee Master";
            //            MessageBox.Show(SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            return;
            //        }
            //    }
            //}

            //if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    string vDelString = string.Empty;
            //    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

            //    vDelString = "Delete from " + vMainTblNm + "  Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
            //    oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
            //    this.dsMain.Tables[0].Rows[0].Delete();
            //    this.dsMain.Tables[0].AcceptChanges();

            //    if (this.btnForward.Enabled)
            //    {
            //        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
            //        dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
            //        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            //    }
            //    else
            //    {
            //        if (this.btnBack.Enabled)
            //        {
            //            SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            //            dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
            //            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            //        }

            //    }
            //    this.mthView();
            //    this.mthChkNavigationButton();
            //}

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                btnDelete_Click(this.btnDelete, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);   
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

        #endregion

        #region PrintButton

        private void mthPrint(Int16 vPrintOption)
        {
            string vRepGroup = "Employee Payroll Declaration";
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
            //oPrint.pTran_Cd = 0;
           // oPrint.pSpPara="'"+this.txtYear.Text.Trim()+"','"+this.txtEmpCode.Text.Trim()+"','"+this.pAppUerName.Trim()+"'";  /*Ramya*/
            //oPrint.pSpPara = "[Pay_Year=" + this.txtYear.Text.Trim() + "],[EmpNm=" + this.txtEmpNm.Text.Trim() + "]";
            oPrint.pSpPara = "'" + this.txtYear.Text.Trim() + "','" + this.txtEmpCode.Text.Trim() + "','" + this.pAppUerName.Trim() + "'";
            oPrint.pPrintOption = vPrintOption;
            oPrint.Main();
           
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            lblstatus.Text = "Payroll Declaration Preview...";
            this.mthPrint(2);
            lblstatus.Text = "";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.mthPrint(3);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(Path.GetDirectoryName(Application.ExecutablePath));
           
            lblstatus.Text = "Generating Pdf Files...";
            this.mthPrint(4);
            lblstatus.Text = "File Generated";
            lblstatus.Text = "";
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            lblstatus.Text = "Sending Email...";
            this.mthPrint(7);
            lblstatus.Text = "";
        }

        #endregion

        #region Navigation Button

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            //string SqlStr = "select top 1  " + vMainField + " as Col1 from "+vMainTblNm+" order by  " + vMainField + " desc";
            string SqlStr = "Select Top 1 e." + vMainField1 + ",i." + vMainField2 + ",i.EmployeeCode,id  from " + vMainTblNm + " where e.ActiveStatus=1 order by " + vMainField1 + "+" + vMainField2 ;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            vMainFldVal1 = "";
            vMainFldVal2 = "";
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0][vMainField1].ToString()) == false)
                {
                    vMainFldVal1 = dsMain.Tables[0].Rows[0][vMainField1].ToString().Trim();
                    vMainFldVal2 = dsMain.Tables[0].Rows[0][vMainField2].ToString().Trim();
                }
            }
            //SqlStr = "Select  pMailName,Pay_Year from " + vMainTblNm + " Where " + vMainField1 + "='" + vMainFldVal1 + "' and " + vMainField2 + "='" + vMainFldVal2; //Rup

            //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20); //Rup
            this.mthView();
            this.mthChkNavigationButton();



            if (dsMain.Tables[0].Rows.Count == 0)
            {
                //this.btnEmail.Enabled = false;
                //this.btnLocate.Enabled = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal1 = dsMain.Tables[0].Rows[0][vMainField1].ToString().Trim();
            vMainFldVal2 = dsMain.Tables[0].Rows[0][vMainField2].ToString().Trim();
            //SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            SqlStr = "select Top 1 e." + vMainField1 + ",i." + vMainField2 + ",i.EmployeeCode,id from " + vMainTblNm;
            SqlStr = SqlStr + " Where ActiveStatus=1 and rtrim(" + vMainField1 + ")+rtrim(" + vMainField2 + ")<'" + vMainFldVal1 + "'+'" + vMainFldVal2 + "'";
            SqlStr = SqlStr + " order by " + vMainField1 + "+" + vMainField2+" Desc";

            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal1 = dsMain.Tables[0].Rows[0][vMainField1].ToString().Trim();
            vMainFldVal2 = dsMain.Tables[0].Rows[0][vMainField2].ToString().Trim();
            //SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            SqlStr = "select Top 1 e." + vMainField1 + ",i." + vMainField2 + ",i.EmployeeCode,id from " + vMainTblNm;
            SqlStr = SqlStr + " Where ActiveStatus=1 and rtrim(" + vMainField1 + ")+rtrim(" + vMainField2 + ")>'" + vMainFldVal1 + "'+'" + vMainFldVal2 + "'";
            SqlStr = SqlStr + " order by " + vMainField1 + "+" + vMainField2 ;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            //this.mcheckCallingApplication();
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
                this.btnExportPdf.Enabled = true;
                this.gbPeriod.Enabled = false;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            //string SqlStr = "select top 1  " + vMainField + " as Col1 from "+vMainTblNm+" order by  " + vMainField + " desc";
            string SqlStr = "Select Top 1 e."+vMainField1+",i."+vMainField2+",i.EmployeeCode,id  from " + vMainTblNm + " where e.ActiveStatus=1 order by e." + vMainField1 + "+i." + vMainField2 + " desc";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            vMainFldVal1 = "";
            vMainFldVal2= "";
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0][vMainField1].ToString()) == false)
                {
                    vMainFldVal1 = dsMain.Tables[0].Rows[0][vMainField1].ToString().Trim();
                    vMainFldVal2 = dsMain.Tables[0].Rows[0][vMainField2].ToString().Trim();
                }
            }
            //SqlStr = "Select  pMailName,Pay_Year from " + vMainTblNm + " Where " + vMainField1 + "='" + vMainFldVal1 + "' and " + vMainField2 + "='" + vMainFldVal2; //Rup

            //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20); //Rup
            this.mthView();
            this.mthChkNavigationButton();



            if (dsMain.Tables[0].Rows.Count == 0)
            {
                //this.btnEmail.Enabled = false;
                //this.btnLocate.Enabled = true;
            }
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
            if (ServiceType.ToUpper() != "VIEWER VERSION")//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (dsMain.Tables[0].Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false) //Rup
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;
                        //this.btnEmpNm.Enabled = false;  //Ramya 08/02/12
                        // this.btnEmpCode.Enabled = false;  //Ramya 08/02/12

                        vBtnAdd = true;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                    }
                    return;
                }
            } //Added by Archana K. on 17/05/13 for Bug-7899
            if (dsMain.Tables[0].Rows.Count > 0)//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    vMainFldVal1 = dsMain.Tables[0].Rows[0][vMainField1].ToString().Trim();
                    vMainFldVal2 = dsMain.Tables[0].Rows[0][vMainField2].ToString().Trim();

                    SqlStr = "select id from " + vMainTblNm + " Where ActiveStatus=1 and rtrim(" + vMainField1 + ")+rtrim(" + vMainField2 + ")>'" + vMainFldVal1 + "'+'" + vMainFldVal2 + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = "select id from " + vMainTblNm + " Where ActiveStatus=1 and rtrim(" + vMainField1 + ")+rtrim(" + vMainField2 + ")<'" + vMainFldVal1 + "'+'" + vMainFldVal2 + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnBack.Enabled = true;
                        this.btnFirst.Enabled = true;


                    }
                } //Added by Archana K. on 17/05/13 for Bug-7899
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
                        //this.btnEmpNm.Enabled = true;  //Ramya 08/02/12
                        //this.btnEmpCode.Enabled = true;  //Ramya 08/02/12
                    }
                }
                this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
            } //Added by Archana K. on 17/05/13 for Bug-7899
        }

        private void mthChkAEDPButton(Boolean vBtnAdd, Boolean vBtnEdit, Boolean vBtnDelete, Boolean vBtnPrint)
        {
            
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;

            this.btnEmail.Enabled = false;
            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnExportPdf.Enabled = false;
            this.btnLocate.Enabled = false;


            //if (dsMain.Tables[0].Rows.Count == 0)
            //{
            //    //return;
            //}
           
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
               // this.btnDelete.Enabled = true;
            }
            if (vBtnPrint && this.pPrintButton)
            {
                this.btnEmail.Enabled = true;
                this.btnPreview.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnExportPdf.Enabled = true;
            }
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnLogout.Enabled = false;
            //this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;
            //if(chkDeActivate.Checked)
            
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

        private void mthBindClear()
        {
            this.txtYear.DataBindings.Clear();
            this.txtEmpNm.DataBindings.Clear();
            this.txtEmpCode.DataBindings.Clear();
            //this.txtFyear.DataBindings.Clear(); /*Ramya 19/02/13*/
            if (EditCancel == false)
            {
                this.txtFyear.Text = ""; /*Ramya 19/02/13*/
            }
            //else
            //{
            //    this.EditCancel = false;  
            //}
        }

        private void mthBindData()
        {
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                this.txtYear.DataBindings.Add("Text", dsMain.Tables[0], "Pay_Year");
                this.txtEmpNm.DataBindings.Add("Text", dsMain.Tables[0], "pMailName");
                this.txtEmpCode.DataBindings.Add("Text", dsMain.Tables[0], "EmployeeCode");
                //this.txtFyear.DataBindings.Add("Text", fyear);  /*Ramya 19/02/13*/
                txtFyear.Text = fyear; /*Ramya 19/02/13*/

            }
            this.mthGrdRefreshN();
        }
       
        private void mthView()
        {
            this.mthBindClear();
            this.mthBindData();
            //this.mthGrdRefresh();

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

              
                ////menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                //btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }           
              
        #endregion

        #region FormCommonCode()

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

        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpPayrollDeclaration.exe";
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

        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
        

            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                //this.btnEmpNm.Enabled = false;
                //this.btnEmpCode.Enabled = false;

            }
            else
            {
                //this.btnEmpNm.Enabled = true;
                //this.btnEmpCode.Enabled = true;
            }

            this.txtFyear.Enabled = vEnabled; /*Ramya 19/02/13*/
            
            this.btnYear.Enabled = vEnabled;
            this.btnEmpNm.Enabled = true;
            this.txtYear.Enabled = vEnabled;
            this.txtEmpNm.Enabled = vEnabled;
            this.txtEmpCode.Enabled = vEnabled;
            if (pEditMode)      /*Ramya 30/10/12*/
            {
                this.btnEmpNm.Enabled = false;
                this.btnYear.Enabled = false;
            }

           
           
        }

        #endregion

        private void btnYear_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select distinct Pay_Year,sDate,eDate From Emp_Payroll_Year_Master y order by y.sDate desc";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Leave Year";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Leave Year,sDate:Start Date,eDate:End Date";
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
                    //this.mthGrdRefresh();
                }
            }

            //Select distinct Pay_Year  as Pay_Year From Emp_Payroll_Year_Master order by Pay_Year
        }

        private void btnEmpNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            if (pAddMode)
            {
                SqlStr = "Select E.pMailName as EmpNm,E.EmployeeCode as EmpCode,E.Department,E.Designation,E.Category,E.Grade from EmployeeMast E";
                SqlStr = SqlStr + " Left Join Loc_Master L on (L.Loc_Code=E.Loc_Code)";
                SqlStr = SqlStr + " Left Join Department D on (D.Dept=E.Department)";
                SqlStr = SqlStr + " Left Join Category C on (C.Cate=E.Category)";
                SqlStr = SqlStr + " Where 1=1 and ActiveStatus=1";


                SqlStr = SqlStr + " Union Select '' as EmpNm,'' as EmpCode,'' as Department,'' as Designation,'' as Category,'' as Grade";

                SqlStr = SqlStr + " order by pMailName";
            }
            else 
            {
                SqlStr = "select E.EmployeeName as EmployeeName ,p.pay_year as pay_year,p.EmployeeCode as EmployeeCode from Emp_Payroll_Declaration_Details p  Left Join  EmployeeMast E  on (E.EmployeeCode=P.EmployeeCode)";
            }
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                 return;
            }


            VForText = "Select Employee Name";
            if (pAddMode)
            {
                vSearchCol = "EmpNm";
                vDisplayColumnList = "EmpNm:Employee Name,EmpCode:Employee Code,Department:Department,Designation:Designation,Category:Category";
                vReturnCol = "EmpNm,EmpCode";

            }
            else             
            {
                vSearchCol = "EmployeeName";
                vDisplayColumnList = "EmployeeName:Employee Name,pay_year:Year";
                vReturnCol = "EmployeeName,pay_year,EmployeeCode";
            }
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                if (pAddMode)
                {
                    this.txtEmpNm.Text = oSelectPop.pReturnArray[0];
                    this.txtEmpCode.Text = oSelectPop.pReturnArray[1];
                }
                else
                {
                    this.txtEmpNm.Text = oSelectPop.pReturnArray[0];
                    this.txtYear.Text = oSelectPop.pReturnArray[1];
                    //string 
                        SqlStr = "Select Top 1 " + vMainField1 + "," + vMainField2 + ",i.EmployeeCode,id  from " + vMainTblNm + " where  e.EmployeeCode='" +oSelectPop.pReturnArray[2]+"'  order by " + vMainField1 + "+" + vMainField2;

                    //SqlStr = "Select i.* from " + vMainTblNm + " Where e.EmployeeName='" +oSelectPop.pReturnArray[2]+"'";// +"' order by " + vMainField;
                     dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthView();
                    this.mthChkNavigationButton();
                }

            }
            //this.mthGrdRefresh();
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh(); Rup
            }
            //this.mthGrdRefresh();
        }

        //private void btnEmpCode_Click(object sender, EventArgs e)
        //{

        //}

        private void mthGrdRefreshN()
        {
            tblOthSal=new DataTable();
            tbl80c=new DataTable();
            tblOthDed=new DataTable();
              
            tblGrossSal=new DataTable();    /*Ramya*/
            tblLessAllow=new DataTable();
            tblDed16and17 = new DataTable();

            ds80c = new DataSet();
            //1
            SqlStr = "Execute Usp_Ent_Emp_Payroll_Declaration '" + this.txtEmpCode.Text + "','" + this.txtYear.Text + "','''Section17 (1)'',''Section17 (2)'',''Section17 (3)'''";
            tblGrossSal = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            //2
            SqlStr = "Execute Usp_Ent_Emp_Payroll_Declaration '" + this.txtEmpCode.Text + "','" + this.txtYear.Text + "','''Section 10'''";
            tblLessAllow = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            //3
            SqlStr = "Execute Usp_Ent_Emp_Payroll_Declaration '" + this.txtEmpCode.Text + "','" + this.txtYear.Text + "','''Section 16'''";
            tblDed16and17 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);  /*Ramya*/
            //4
            SqlStr = "Execute Usp_Ent_Emp_Payroll_Declaration '" + this.txtEmpCode.Text + "','" + this.txtYear.Text + "','''Income other than salary'''";
            tblOthSal = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            //5
            SqlStr = "Execute Usp_Ent_Emp_Payroll_Declaration '" + this.txtEmpCode.Text + "','" + this.txtYear.Text + "','''Section 80C'',''Section 80CCC'',''Section 80CCD'''";
            tbl80c = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            ds80c = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            //6
            SqlStr = "Execute Usp_Ent_Emp_Payroll_Declaration '" + this.txtEmpCode.Text + "','" + this.txtYear.Text + "','''Section 80CCF'',''Section 80E'',''Section 80G'',''Section 80GG'',''Section 80GGA'',''Section 80GGC'',''Section 80U'',''Section 80DD'',''Section 80D'',''Section 80DDB'',''Section 80G (100%)'',''Section 80G (50%)'',''Others Deduction Under Section VI-A'''";
            tblOthDed = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            //1
            this.dgvGrossSal.Columns.Clear();  /*Ramya*/
            this.dgvGrossSal.DataSource = this.tblGrossSal;
            this.dgvGrossSal.Columns.Clear();


            System.Windows.Forms.DataGridViewTextBoxColumn colSectionDet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSectionDet.HeaderText = "Particulars";
            colSectionDet.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSectionDet.Name = "colSectionDet";
            colSectionDet.Width = 340;
            this.dgvGrossSal.Columns.Add(colSectionDet);
            colSectionDet.ReadOnly = true;
            
            System.Windows.Forms.DataGridViewTextBoxColumn colSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSection.HeaderText = "Section";
            colSection.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSection.Name = "colSection";
            colSection.DataPropertyName = "Section";
            colSection.Width = 160;
            this.dgvGrossSal.Columns.Add(colSection);
            colSection.ReadOnly = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMaxLimit = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMaxLimit.HeaderText = "Maximum Amount Allowed";
            colMaxLimit.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMaxLimit.Name = "colMaxLimit";
            colMaxLimit.DataPropertyName = "MaxLimit";
            colMaxLimit.DecimalLength = 3;
            colMaxLimit.Width = 120;// 110;
            this.dgvGrossSal.Columns.Add(colMaxLimit);
            colMaxLimit.ReadOnly = true;
            this.dgvGrossSal.Refresh();


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmount.HeaderText = "Amount";
            colAmount.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmount.Name = "colAmount";
            colAmount.DecimalLength = 3;
            colAmount.Width = 135;// 110;
            this.dgvGrossSal.Columns.Add(colAmount);
            this.dgvGrossSal.Refresh();
            
            
            dgvGrossSal.Columns["colSectionDet"].DataPropertyName = "DeclarationDet";
            dgvGrossSal.Columns["colAmount"].DataPropertyName = "Amount";
            this.tbcMain.SelectedTab = tbcpLessAllow;
            //2
            this.dgvLessAllow.Columns.Clear();
            this.dgvLessAllow.DataSource = this.tblLessAllow;
            this.dgvLessAllow.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colSectionDet1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSectionDet1.HeaderText = "Particulars";
            colSectionDet1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSectionDet1.Name = "colSectionDet";
            colSectionDet1.DataPropertyName = "DeclarationDet";
            colSectionDet1.Width = 340;
            this.dgvLessAllow.Columns.Add(colSectionDet1);
            colSectionDet1.ReadOnly = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colSection1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSection1.HeaderText = "Section";
            colSection1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSection1.Name = "colSection1";
            colSection1.DataPropertyName = "Section";
            colSection1.Width = 160;
            this.dgvLessAllow.Columns.Add(colSection1);
            colSection1.ReadOnly = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMaxLimit1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMaxLimit1.HeaderText = "Maximum Amount Allowed";
            colMaxLimit1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMaxLimit1.Name = "colMaxLimit1";
            colMaxLimit1.DataPropertyName = "MaxLimit";
            colMaxLimit1.DecimalLength = 3;
            colMaxLimit1.Width = 120;// 110;
            this.dgvLessAllow.Columns.Add(colMaxLimit1);
            colMaxLimit1.ReadOnly = true;
            this.dgvLessAllow.Refresh();


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmount1.HeaderText = "Amount";
            colAmount1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmount1.Name = "colAmount";
            colAmount1.DataPropertyName = "Amount";
            colAmount1.DecimalLength = 3;
            colAmount1.Width = 135;
            this.dgvLessAllow.Columns.Add(colAmount1);
            this.dgvLessAllow.Refresh();


            this.tbcMain.SelectedTab = tbcpDed16;
            //3
            this.dgvDed16and17.Columns.Clear();
            this.dgvDed16and17.DataSource = this.tblDed16and17;
            this.dgvDed16and17.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colSectionDet2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSectionDet2.HeaderText = "Particulars";
            colSectionDet2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSectionDet2.Name = "colSectionDet";
            colSectionDet2.DataPropertyName = "DeclarationDet";
            colSectionDet2.Width = 340;
            this.dgvDed16and17.Columns.Add(colSectionDet2);
            colSectionDet2.ReadOnly = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colSection2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSection2.HeaderText = "Section";
            colSection2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSection2.Name = "colSection2";
            colSection2.DataPropertyName = "Section";
            colSection2.Width = 160;
            this.dgvDed16and17.Columns.Add(colSection2);
            colSection2.ReadOnly = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMaxLimit2 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMaxLimit2.HeaderText = "Maximum Amount Allowed";
            colMaxLimit2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMaxLimit2.Name = "colMaxLimit2";
            colMaxLimit2.DataPropertyName = "MaxLimit";
            colMaxLimit2.DecimalLength = 3;
            colMaxLimit2.Width = 120;// 110;
            this.dgvDed16and17.Columns.Add(colMaxLimit2);
            colMaxLimit2.ReadOnly = true;
            this.dgvDed16and17.Refresh();


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount2 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmount2.HeaderText = "Amount";
            colAmount2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmount2.Name = "colAmount";
            colAmount2.DataPropertyName = "Amount";
            colAmount2.DecimalLength = 3;
            colAmount2.Width = 135;
            this.dgvDed16and17.Columns.Add(colAmount2);

            

            this.tbcMain.SelectedTab = tbcpOtherIncome;
            //4
            this.dgvOthSal.Columns.Clear();
            this.dgvOthSal.DataSource = this.tblOthSal;
            this.dgvOthSal.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colSectionDet3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSectionDet3.HeaderText = "Particulars";
            colSectionDet3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSectionDet3.Name = "colSectionDet";
            colSectionDet3.Width = 340;
            this.dgvOthSal.Columns.Add(colSectionDet3);
            colSectionDet3.ReadOnly = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colSection3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSection3.HeaderText = "Section";
            colSection3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSection3.Name = "colSection1";
            colSection3.DataPropertyName = "Section";
            colSection3.Width = 160;
            this.dgvOthSal.Columns.Add(colSection3);
            colSection3.ReadOnly = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMaxLimit3 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMaxLimit3.HeaderText = "Maximum Amount Allowed";
            colMaxLimit3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMaxLimit3.Name = "colMaxLimit3";
            colMaxLimit3.DataPropertyName = "MaxLimit";
            colMaxLimit3.DecimalLength = 3;
            colMaxLimit3.Width = 120;// 110;
            this.dgvOthSal.Columns.Add(colMaxLimit3);
            colMaxLimit3.ReadOnly = true;
            this.dgvOthSal.Refresh();


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount3 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmount3.HeaderText = "Amount";
            colAmount3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmount3.Name = "colAmount";
            colAmount3.DecimalLength = 3;
            colAmount3.Width = 135;
            this.dgvOthSal.Columns.Add(colAmount3);
            this.dgvOthSal.Refresh();

            dgvOthSal.Columns["colSectionDet"].DataPropertyName = "DeclarationDet";
            dgvOthSal.Columns["colAmount"].DataPropertyName = "Amount";

            this.tbcMain.SelectedTab = tbcp80C;
            //5
            this.dgvSum.Columns.Clear();
            this.dgvSum.DataSource = tbl80c;
            this.dgvSum.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colSectionDet4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSectionDet4.HeaderText = "Particulars";
            colSectionDet4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSectionDet4.Name = "colSectionDet";
            colSectionDet4.DataPropertyName = "DeclarationDet";
            colSectionDet4.Width = 340;
            this.dgvSum.Columns.Add(colSectionDet4);
            colSectionDet4.ReadOnly = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colSection4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSection4.HeaderText = "Section";
            colSection4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSection4.Name = "colSection4";
            colSection4.DataPropertyName = "Section";
            colSection4.Width = 160;
            this.dgvSum.Columns.Add(colSection4);
            colSection4.ReadOnly = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMaxLimit4 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMaxLimit4.HeaderText = "Maximum Amount Allowed";
            colMaxLimit4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMaxLimit4.Name = "colMaxLimit4";
            colMaxLimit4.DataPropertyName = "MaxLimit";
            colMaxLimit4.DecimalLength = 3;
            colMaxLimit4.Width = 120;// 110;
            this.dgvSum.Columns.Add(colMaxLimit4);
            colMaxLimit4.ReadOnly = true;
            this.dgvSum.Refresh();

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount4 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmount4.HeaderText = "Amount";
            colAmount4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmount4.Name = "colAmount";
            colAmount4.DataPropertyName = "Amount";
            colAmount4.DecimalLength = 3;
            colAmount4.Width = 135;
            this.dgvSum.Columns.Add(colAmount4);
            this.dgvSum.Refresh();

            //MessageBox.Show(this.dgv80c.Columns.Count.ToString());
            //this.dgv80c.Columns["colSectionDet"].DataPropertyName = "DeclarationDet";
            //this.dgv80c.Columns["colAmount"].DataPropertyName = "Amount";

            this.tbcMain.SelectedTab = tbcpOthDed ;
            //6
            this.dgvOthDed.Columns.Clear();
            this.dgvOthDed.DataSource = this.tblOthDed;
            this.dgvOthDed.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colSectionDet5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSectionDet5.HeaderText = "Particulars";
            colSectionDet5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSectionDet5.Name = "colSectionDet";
            colSectionDet5.Width = 340;
            this.dgvOthDed.Columns.Add(colSectionDet5);
            colSectionDet5.ReadOnly = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colSection5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSection5.HeaderText = "Section";
            colSection5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSection5.Name = "colSection1";
            colSection5.DataPropertyName = "Section";
            colSection5.Width = 160;
            this.dgvOthDed.Columns.Add(colSection5);
            colSection5.ReadOnly = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMaxLimit5 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMaxLimit5.HeaderText = "Maximum Amount Allowed";
            colMaxLimit5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMaxLimit5.Name = "colMaxLimit5";
            colMaxLimit5.DataPropertyName = "MaxLimit";
            colMaxLimit5.DecimalLength = 3;
            colMaxLimit5.Width = 120;// 110;
            this.dgvOthDed.Columns.Add(colMaxLimit5);
            colMaxLimit5.ReadOnly = true;
            this.dgvOthDed.Refresh();


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount5 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmount5.HeaderText = "Amount";
            colAmount5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmount5.Name = "colAmount";
            colAmount5.DecimalLength = 3;
            colAmount5.Width = 135;
            this.dgvOthDed.Columns.Add(colAmount5);

            
            dgvOthDed.Columns["colSectionDet"].DataPropertyName = "DeclarationDet";
            dgvOthDed.Columns["colAmount"].DataPropertyName = "Amount";
            this.tbcMain.SelectedTab =tbcpGrossSal;
            //this.tbcpDed16and17.v
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

        private void txtYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnYear_Click(sender, new EventArgs());
                }
            }
        }

        private void txtEmpNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnEmpNm_Click(sender, new EventArgs());
                }
            }
        }

        private void frmDeclaration_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

       

      

       

       
      

     
    }
}
