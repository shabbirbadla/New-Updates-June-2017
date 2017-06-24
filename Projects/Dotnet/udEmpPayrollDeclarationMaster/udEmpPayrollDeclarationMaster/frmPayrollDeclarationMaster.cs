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
using uNumericTextBox;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpPayrollDeclarationMaster
{
    public partial class frmEmpInvHeadDet : uBaseForm.FrmBaseForm
    {
        //udEmpPayrollDeclarationMaster.exe
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;            
        string SqlStr = string.Empty;
        short vTimeOut;
        string vMainTblNm = "Emp_Payroll_Declaration_Master", vMainField = "Fld_Nm", vMainFldVal = "";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        bool cValid = true;
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmEmpInvHeadDet(string[] args)
        {
            this.pDisableCloseBtn = true;  /*close disable*/
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Payroll Declaration Master";
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

        private void frmEmpInvHeadDet_Load(object sender, EventArgs e)
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
            vTimeOut = 20;

            this.dtpDeact.CustomFormat = " ";
            this.dtpDeact.Format = DateTimePickerFormat.Custom;

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

            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
               this.btnSectionName.Image = Image.FromFile(fName);  

            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnSection.Image = Image.FromFile(fName);
                this.btnPayHeadFldNm.Image = Image.FromFile(fName);
            }
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
                this.pAddButton = false;
            }
        }

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            // cAppName = "udEEmpEDMaster.exe";
            cAppName = "udEmpPayrollDeclarationMaster.exe";
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

        private void mthBindClear()
        {
            this.txtSectionDet.DataBindings.Clear();
            this.txtPayHeadFldNm.DataBindings.Clear();
            this.txtmaxLimit.DataBindings.Clear();
            //this.txtHeadType.DataBindings.Clear();
            this.txtSection.Text = "";
            this.txtFldName.DataBindings.Clear();
            this.txtSortOrd.DataBindings.Clear();
            this.chkDeActivate.DataBindings.Clear();
            this.chkIntFld.DataBindings.Clear();
            this.dtpDeact.DataBindings.Clear();
        }

        private void mthBindData()
        {
            if (dsMain.Tables.Count == 0)
            {
                return;
            }
            if (dsMain.Tables[0].Rows.Count > 0)
            {

                if (dsMain.Tables[0].Rows[0]["IsDeActive"].ToString() == "True")
                {
                    chkDeActivate.Checked = true;

                }
                else
                {
                    chkDeActivate.Checked = false;
                    this.dtpDeact.Enabled = false;
                }
                if (dsMain.Tables[0].Rows[0]["IntField"].ToString() == "True") { this.chkIntFld.Checked = true; } else { this.chkIntFld.Checked = false; }

            }

            this.txtSectionDet.DataBindings.Add("Text", dsMain.Tables[0], "DeclarationDet");
            this.txtPayHeadFldNm.DataBindings.Add("Text", dsMain.Tables[0], "Monthly_Payroll_fld_Nm");
            this.txtmaxLimit.DataBindings.Add("Text", dsMain.Tables[0], "MaxLimit");
           // this.txtHeadType.DataBindings.Add("Text", dsMain.Tables[0], "HeadType");
            this.txtFldName.DataBindings.Add("Text", dsMain.Tables[0], "FLD_NM");
            this.txtSortOrd.DataBindings.Add("Text", dsMain.Tables[0], "SortOrd");
            this.dtpDeact.DataBindings.Add("Text", dsMain.Tables[0], "DeactFrom");

        }

        private void mthEnableDisableFormControls()
        {


            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnSection.Enabled = true;
                this.btnSectionName.Enabled = false;
                this.btnPayHeadFldNm.Enabled = true;
                this.txtSectionDet.Enabled = true;
                this.txtmaxLimit.Enabled = true;
                this.txtSection.Enabled = true;
                this.txtPayHeadFldNm.Enabled = true;/*Ramya*/

            }
            else
            {
                this.btnSectionName.Enabled = true;
                this.btnSection.Enabled = false;
                this.btnPayHeadFldNm.Enabled = false;
                this.txtSectionDet.Enabled = false;
                this.txtSection.Enabled = false;
                this.txtmaxLimit.Enabled = false;
                this.txtPayHeadFldNm.Enabled = false;/*Ramya*/
            }

            if (this.pAddMode)
            {
                this.txtFldName.Enabled = true;
            }
            else
            {
                this.txtFldName.Enabled = false;
            }

            this.txtSortOrd.Enabled = vEnabled;
            this.txtSectionDet.Enabled = vEnabled;
            this.chkDeActivate.Enabled = vEnabled;
            this.chkIntFld.Enabled = vEnabled;
            this.txtmaxLimit.Enabled = vEnabled;
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
                        //this.btnDocName.Enabled = false;  //Ramya 08/02/12

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
                        //this.btnDocName.Enabled = true; //Ramya 08
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
                // return; Rup
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
               // this.btnDelete.Enabled = true;   /*to disable delete button*/
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
            this.dtpDeact.Enabled = false;
            if (this.btnNew.Enabled == false && this.btnEdit.Enabled == false)
            {
                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
            }
            if (this.btnSave.Enabled == false)
            {
                this.btnLogout.Enabled = true;
                //this.btnEmail.Enabled = true;  Ramya
                //this.btnLocate.Enabled = true;
            }
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();   /*Ramya */
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select SECTION,SortOrd from Emp_Payroll_Section_Master order by SortOrd";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Section";
            vSearchCol = "SECTION";
            vDisplayColumnList = "SECTION:Section,SortOrd:Sort Order";
            vReturnCol = "SECTION";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = dsMain.Tables[0].Rows[0]["HeadType"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtSection.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtSection.Text.Trim();
                dsMain.Tables[0].Rows[0]["SECTION"] = oSelectPop.pReturnArray[0];
                //SqlStr = "Select top 1 * from "+vMainTblNm+" Where DocNm='" + vMainFldVal + "' order by SortOrd ";
                //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;

            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            this.txtSectionDet.Focus();

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
            //this.txtSortOrd.Focus();
            if (this.chkDeActivate.Checked)
            {
                this.dtpDeact.Enabled = true;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            cValid = true;  //Ramya
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
            //this.pAddMode = false;   /*ramya 26/12/2011*/
            //this.pEditMode = false;  /*ramya 26/12/2011*/

            this.mthChkNavigationButton();

            timer1.Enabled = true;
            timer1.Interval = 1000;
            MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
           
        }

        private void mthSave()
        {
            string vSaveString = string.Empty;

            if (dtpDeact.Text.Trim() != "")         /*Ramya 19/03/12*/
            {
                dsMain.Tables[0].Rows[0]["DeActFrom"] = this.dtpDeact.Text; //Rup 16/03/12
            }

            dsMain.Tables[0].Rows[0].AcceptChanges();
            dsMain.Tables[0].Rows[0].EndEdit();
            if (this.chkDeActivate.Checked) { dsMain.Tables[0].Rows[0]["IsDeactive"] = true; } else { dsMain.Tables[0].Rows[0]["IsDeactive"] = false; }
            if (this.chkIntFld.Checked) { dsMain.Tables[0].Rows[0]["IntField"] = true; } else { dsMain.Tables[0].Rows[0]["IsDeactive"] = false; }

            this.mSaveCommandString(ref vSaveString, "#ID#");

            if (this.pAddMode)    /*Ramya 10/07/12 */
            {
                SqlStr = "Execute Add_Columns 'Emp_Payroll_Declaration_Details','" + this.txtFldName.Text.Trim() + " Decimal(16,3) Default 0 with Values'";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
            }

            this.pAddMode = false; /*ramya 26/12/2011*/
            this.pEditMode = false; /*ramya 26/12/2011*/
            this.mthEnableDisableFormControls(); /*ramya 26/12/2011*/

            oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);



            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthView();

        }

        private void mthChkSaveValidation(ref Boolean vValid)
        {
            if (string.IsNullOrEmpty(this.txtSectionDet.Text.Trim()))
            {
                MessageBox.Show("Section Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
            }
            if (string.IsNullOrEmpty(this.txtFldName.Text.Trim()))
            {
                MessageBox.Show("Field Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
            }
            if (string.IsNullOrEmpty(this.txtSection.Text.Trim()))
            {
                MessageBox.Show("Section Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
            }
            //if (string.IsNullOrEmpty(this.txtLocNm.Text))
            //{
            //    MessageBox.Show("Location Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    vValid = false;
            //}

            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " select DeclarationDet from " + vMainTblNm + " where  DeclarationDet ='" + this.txtSectionDet.Text.Trim() + "'";
            if (this.pEditMode)
            {
                sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            }
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Section Name already exists", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtSectionDet.Focus();
                vValid = false;                
            }

            sqlstr = " select FLD_NM from " + vMainTblNm + " where  FLD_NM='" + this.txtFldName.Text.Trim() + "'";
            if (this.pEditMode)
            {
                sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            }
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Field Name already exists", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtFldName.Focus();
                vValid = false;
            }

            if (chkDeActivate.Checked) /*Ramya 14/3/12*/
            {
                if (dtpDeact.Value.Date < Convert.ToDateTime("01/01/2000") || dtpDeact.Value.Date >= Convert.ToDateTime("01/01/2079"))
                {
                    //MessageBox.Show("Deactive From Date Should Greater Than Or Equal To 01/01/2000");
                    MessageBox.Show("Year should be between 2000 to 2078", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    vValid = false;
                    return;
                }
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
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
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
                    //if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1) || (dtc1.ToString().Trim().ToLower() == vMainField.Trim().ToLower()))
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
                    this.txtSortOrd.Enabled = false;/*Ramya 07/02/12*/
                    this.txtSectionDet.Enabled = false;
                    this.txtSection.Enabled = false;
                    this.btnSection.Enabled = false;
                    this.chkDeActivate.Enabled = false;
                   
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
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
                this.groupBox1.Enabled = false;
                this.groupBox5.Enabled = false;
            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            this.mthEnableDisableFormControls();

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
            this.mthChkNavigationButton();

            if (dsMain.Tables[0].Rows.Count == 0)
            {
                this.btnEmail.Enabled = false;
                //this.btnLocate.Enabled = true; Ramya
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }

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

                SqlStr = " select consnm=name from sysobjects where id in (";    /*Ramya*/
                SqlStr = SqlStr + " select syscolumns.cdefault from syscolumns";
                SqlStr = SqlStr + " inner join sysobjects on (syscolumns.id=sysobjects.id)";
                SqlStr = SqlStr + " where (syscolumns.name='" + this.txtFldName.Text.Trim() + "'";
                SqlStr = SqlStr + "') and sysobjects.name='Emp_Payroll_Declaration')";
                //oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                //DataRow dr;
                //dr = oDataAccess.GetDataRow(SqlStr, null, 20);
                //string def_const = dr[0].ToString();


                DataSet ds = oDataAccess.GetDataSet(SqlStr, null, 20);

                string def_const1 = ds.Tables[0].Rows[0][0].ToString();//dr[0].ToString();
               // string def_const2 = ds.Tables[0].Rows[1][0].ToString();

                SqlStr = "alter table Emp_Payroll_Declaration_Details drop Constraint " + def_const1;
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                SqlStr = "alter table Emp_Payroll_Declaration_Details drop Column " + this.txtFldName.Text;
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);      /*Ramya*/

                string vDelString = string.Empty;
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                vDelString = "Delete from " + vMainTblNm + "  Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, vTimeOut, true);
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
                    //else   /*ramya 080212*/
                    //{
                    //    return;
                    //}

                }
                this.mthView();
                this.mthChkNavigationButton();

                timer1.Enabled = true;
                timer1.Interval = 1000;
                MessageBox.Show("Entry Deleted", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
           
            }

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mthView()
        {
            this.mthBindClear();
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if ((Boolean)dsMain.Tables[0].Rows[0]["IsDeactive"] == false) { this.chkDeActivate.Checked = false; } else { this.chkDeActivate.Checked = true; }
                    if ((Boolean)dsMain.Tables[0].Rows[0]["IntField"] == false) { this.chkIntFld.Checked = false; } else { this.chkIntFld.Checked = true; }
                    if (dsMain.Tables[0].Rows[0]["SECTION"] != DBNull.Value)
                    {
                        this.txtSection.Text = (string)dsMain.Tables[0].Rows[0]["SECTION"];
                    }
                }
            }
            this.mthBindData();
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

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
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

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from " + vMainTblNm + "  order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();
            this.mthChkNavigationButton();
        }

        private void btnSectionName_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();   /*Ramya */
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select DeclarationDet,FLD_NM,SECTION from " + vMainTblNm + " order by SortOrd";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Section Name";
            vSearchCol = "DeclarationDet";
            vDisplayColumnList = "DeclarationDet:Declaration Details,FLD_NM:Field Name,SECTION:Section";
            vReturnCol = "DeclarationDet,FLD_NM,SECTION";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = dsMain.Tables[0].Rows[0]["HeadType"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtSectionDet.Text = oSelectPop.pReturnArray[0];
                this.txtSection.Text = oSelectPop.pReturnArray[2];
                vMainFldVal = oSelectPop.pReturnArray[1];
                SqlStr = "Select top 1 * from " + vMainTblNm + " Where FLD_NM='" + vMainFldVal + "' order by SortOrd ";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            this.mthView();
            this.mthChkNavigationButton();

        }

        private void frmEmpInvHeadDet_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

        private void txtSection_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnSection_Click(sender, new EventArgs());
                }
            }
        }

        private void txtSectionDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void txtFldName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void txtSortOrd_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;
            }
        }

        private void chkDeActivate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkDeActivate.Checked)
            {
                this.dtpDeact.Enabled = true;
                // dtpDeact.Value = Convert.ToDateTime(this.dtpDeact.Text.ToString());
                dtpDeact.CustomFormat = "dd/MM/yyyy";
            }
            else
            {
                this.dtpDeact.Enabled = false;
                dtpDeact.CustomFormat = " ";
                //dtpDeact.Value = Convert.ToDateTime("01/01/1900");
            }

        }

        private void btnPayHeadFldNm_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();   /*Ramya */
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select distinct Head_Nm,Fld_Nm From Emp_pay_Head_Master union Select '','' order by Head_Nm";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            //VForText = "Select Section";
            VForText = "Select Field"; /*Ramya 14/11/12*/
            //vSearchCol = "SECTION";
            vSearchCol = "Head_Nm";
            vDisplayColumnList = "Head_Nm:Head Name,Fld_Nm:Field Name";
            vReturnCol = "Fld_Nm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = dsMain.Tables[0].Rows[0]["HeadType"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                if (oSelectPop.pReturnArray.ToString().Trim() != "")
                {
                    SqlStr = "Select DeclarationDet From Emp_Payroll_Declaration_Master Where Monthly_Payroll_fld_Nm='" + oSelectPop.pReturnArray[0].Trim() + "'";
                    DataTable dtTemp = new DataTable();
                    dtTemp = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                    if (dtTemp.Rows.Count > 0)
                    {
                        MessageBox.Show("Field is already used for " + dtTemp.Rows[0]["DeclarationDet"].ToString().Trim() + ".", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        this.btnPayHeadFldNm.Focus();
                        this.txtPayHeadFldNm.Text = oSelectPop.pReturnArray[0];
                        dsMain.Tables[0].Rows[0]["Monthly_Payroll_fld_Nm"] = oSelectPop.pReturnArray[0]; /*Ramya 20/11/12*/
                    }
                }
                //vMainFldVal = this.txtSection.Text.Trim();
                //dsMain.Tables[0].Rows[0]["Fld_Nm"] = oSelectPop.pReturnArray[0];
                //SqlStr = "Select top 1 * from "+vMainTblNm+" Where DocNm='" + vMainFldVal + "' order by SortOrd ";
                //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
