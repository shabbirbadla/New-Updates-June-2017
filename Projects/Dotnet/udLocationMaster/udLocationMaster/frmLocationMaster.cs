using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Collections;
using DataAccess_Net;
using uBaseForm;
using System.Reflection;
using System.Text.RegularExpressions;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udLocationMaster
{
    public partial class frmLocationMaster : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        
        string SqlStr = string.Empty;
        string vMainField = "Loc_Desc",vMainTblNm = "Loc_Master", vMainFldVal = "";
        string vExpression = string.Empty; string vId = string.Empty;
        String cAppPId, cAppName;
        Boolean cValid=true;
        Boolean vEnabled; //Added by Archana K. on 05/02/13 for Bug-8633 
        DataSet dsMain;  //Added by Archana K. on 05/02/13 for Bug-8633 
        Icon MainIcon;

        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end
        public frmLocationMaster(string[] args)
        {
            this.pDisableCloseBtn = true;  /*close disable*/
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Location Master";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
        }

        

       private void frmLocationMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
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
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnLocCode.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnManager.Image = Image.FromFile(fName);
                this.btnValidity.Image = Image.FromFile(fName);
                this.btncity.Image = Image.FromFile(fName);
                this.btnstate.Image = Image.FromFile(fName);
                this.btncountry.Image = Image.FromFile(fName);
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
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
           // cAppName = "udEEmpEDMaster.exe";
            cAppName = "udLocationMaster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
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


     public  void mthBindClear()  //private
        {
            this.txtLocCode.DataBindings.Clear();
            this.txtLocNm.DataBindings.Clear();
            this.txtAdd1.DataBindings.Clear();
            this.txtAdd2.DataBindings.Clear();
            this.txtAdd3.DataBindings.Clear();
            
            this.txtArea.DataBindings.Clear();
            this.txtZone.DataBindings.Clear();
            this.txtCity.DataBindings.Clear();
            this.txtPinCode.DataBindings.Clear();

            this.txtState.DataBindings.Clear();
            this.txtCountry.DataBindings.Clear();
       
            this.txtStdCode.DataBindings.Clear();
            this.txtPhone.DataBindings.Clear();
            this.txtFax.DataBindings.Clear();
            this.txtEmail.DataBindings.Clear();

           // this.txtManager.DataBindings.Clear();
            this.txtManager.Text = ""; /*Ramya 04/07/12*/
            this.txtValidity.DataBindings.Clear();
            
        }
     private void mthBindData() 
        {
            if (dsMain.Tables.Count == 0)
            {
                return;
            }

            this.txtLocCode.DataBindings.Add("Text", dsMain.Tables[0], "Loc_Code");
            this.txtLocNm.DataBindings.Add("Text", dsMain.Tables[0], "Loc_Desc");
            this.txtAdd1.DataBindings.Add("Text", dsMain.Tables[0], "Add1");
            this.txtAdd2.DataBindings.Add("Text", dsMain.Tables[0], "Add2");
            this.txtAdd3.DataBindings.Add("Text", dsMain.Tables[0], "Add3");
            this.txtArea.DataBindings.Add("Text", dsMain.Tables[0], "Area");
            this.txtZone.DataBindings.Add("Text", dsMain.Tables[0], "Zone");
            this.txtCity.DataBindings.Add("Text", dsMain.Tables[0], "City");
            this.txtPinCode.DataBindings.Add("Text", dsMain.Tables[0], "PinCode");


            this.txtState.DataBindings.Add("Text", dsMain.Tables[0], "State");
            this.txtCountry.DataBindings.Add("Text", dsMain.Tables[0], "Country");

            this.txtStdCode.DataBindings.Add("Text", dsMain.Tables[0], "StdCd");
            this.txtPhone.DataBindings.Add("Text", dsMain.Tables[0], "phone");
            this.txtFax.DataBindings.Add("Text", dsMain.Tables[0], "Fax");
            this.txtEmail.DataBindings.Add("Text", dsMain.Tables[0], "EMail");

            //this.txtManager.DataBindings.Add("Text", dsMain.Tables[0], "mEmpCode");
            this.txtValidity.DataBindings.Add("Text", dsMain.Tables[0], "Validity");
         
                    
        }
       private void mthEnableDisableFormControls() 
        {
            vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnLocCode.Enabled = false;
                this.btnLocNm.Enabled = false;
            }
            else
            {
                this.btnLocCode.Enabled = true;
                this.btnLocNm.Enabled = true;
            }
            if (this.pAddMode)
            {
                this.txtLocCode.Enabled = true;
            }
            else
            {
                this.txtLocCode.Enabled = false;
            }
            this.btnstate.Enabled = vEnabled;
            this.btncity.Enabled = vEnabled;
            this.btncountry.Enabled = vEnabled;
            this.txtLocNm.Enabled = vEnabled;
            this.txtAdd1.Enabled = vEnabled;
            this.txtAdd1.Enabled = vEnabled;
            this.txtAdd2.Enabled = vEnabled;
            this.txtAdd3.Enabled = vEnabled;

            this.txtArea.Enabled = vEnabled;
            this.txtZone.Enabled = vEnabled;
            this.txtCity.Enabled = vEnabled;
            this.txtPinCode.Enabled = vEnabled;

            this.txtState.Enabled = vEnabled;
            this.txtCountry.Enabled = vEnabled;

            this.txtStdCode.Enabled = vEnabled;
            this.txtPhone.Enabled = vEnabled;
            this.txtFax.Enabled = vEnabled;
            this.txtEmail.Enabled = vEnabled;

            this.txtManager.Enabled = vEnabled;
            this.btnManager.Enabled = vEnabled;
            this.btnValidity.Enabled = vEnabled;
            this.txtValidity.Enabled = vEnabled;
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
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField +" desc";
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
                this.groupBox2.Enabled = false;
                this.groupBox3.Enabled = false;
            }
            else
            //Added by Archana K. on 17/05/13 for Bug-7899 end 
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
               // this.btnEmail.Enabled = false;
                //this.btnLocate.Enabled = true;
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); 
            this.pAddMode = true;
            this.pEditMode = false;

            this.mthNew(sender, e);
            
            this.mthChkNavigationButton();
            this.txtLocCode.Focus();

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

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            cValid = true;
            this.lblmand.Focus();
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

             //this.mthEnableDisableFormControls();   /*ramya 26/12/2011*/

             timer1.Enabled = true;
             timer1.Interval = 1000;
             MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
         
        }
        private void mthSave()
        {

            string vSaveString = string.Empty;

            dsMain.Tables[0].Rows[0].AcceptChanges();
            dsMain.Tables[0].Rows[0].EndEdit();
           
            /*Add City to City table*/
            string sqlcity = "Select City from city  where City='" + this.txtCity.Text.Trim() + "' order by city";
            DataSet dscity = oDataAccess.GetDataSet(sqlcity, null, 20);
            if (dscity.Tables[0].Rows.Count== 0)
            {

                string str = "insert into city(city,Apgen,Apgenby,CompId) values ('" + this.txtCity.Text.Trim() + "','','',0)";

                //"insert into city(city,apgen,apgenby,compid) values( '" + this.txtCity.Text.Trim() + "','','',0)";
                oDataAccess.ExecuteSQLStatement(str, null, 20, true);
            }
            /*Add City to City table*/
            
   
            this.mSaveCommandString(ref vSaveString, "#ID#");

            this.pAddMode = false; /*ramya 26/12/2011*/
            this.pEditMode = false; /*ramya 26/12/2011*/
           this.mthEnableDisableFormControls(); /*ramya 26/12/2011*/
           
            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);

           
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from "+vMainTblNm+" Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();

        }
        private void mthChkSaveValidation(ref Boolean vValid)
        {
           
            if (string.IsNullOrEmpty(this.txtLocCode.Text.Trim()))
            {
                MessageBox.Show("Location Code Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
            }
            if (string.IsNullOrEmpty(this.txtLocNm.Text.Trim()))
            {
                MessageBox.Show("Location Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vValid = false;
            }

            DataSet dsData = new DataSet();
            string sqlstr;
            if (pAddMode)
            {
                vId = Convert.ToString(0);
            }
            else
            {
                vId = dsMain.Tables[0].Rows[0]["id"].ToString();
            }

            sqlstr = " select Loc_Code from " + vMainTblNm + " where Loc_Code='" + txtLocCode.Text.Trim() + "' and id<>"+vId;
            //if (this.pEditMode)
            //{
            //    sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            //}
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                //ToolTip t = new ToolTip();
                //string ErrMsg = t.GetToolTip(txtLocCode).ToString();
                //errorProvider.SetError(txtLocCode, "Duplicate " + lblLoccode.Text + " value");
                MessageBox.Show(lblLoccode.Text.Trim() + " already exists! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocCode.Focus();
                vValid = false;  //Ramya
                return;
            }

            DataSet dsData1 = new DataSet();
            string sqlstr1;
            sqlstr1 = " select " + vMainField + " from " + vMainTblNm + " where " + vMainField + "='" + txtLocNm.Text.Trim() + "' and id<>"+vId;
            //if (this.pEditMode)
            //{
            //    sqlstr1 = sqlstr1 + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            //}
            dsData1 = oDataAccess.GetDataSet(sqlstr1, null, 20);
            if (dsData1.Tables[0].Rows.Count > 0)
            {
                //ToolTip t1 = new ToolTip();
                //string ErrMsg1 = t1.GetToolTip(txtLocNm).ToString();
                //errorProvider.SetError(txtLocNm, "Duplicate " + lblLocname.Text + " value");
                 MessageBox.Show(lblLocname.Text.Trim() + " already exists! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 txtLocNm.Focus();
                 vValid = false;  //Ramya
                //e.Cancel = true;
                 return;
            }

            //errorProvider.SetError(txtLocNm, "");
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

                vSaveString = " insert into " + vMainTblNm ;
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
                vSaveString = " Update " + vMainTblNm + "  Set ";
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
                    if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1) )
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
            this.pAddMode = true;
            this.pEditMode = false;
            if (this.dsMain.Tables[0].Rows.Count != 0) //Rup
            {
                dsMain.Tables[0].Rows[0].CancelEdit();
            }
            this.mthEnableDisableFormControls();
            if (this.dsMain.Tables[0].Rows.Count != 0) //Rup
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
                    //this.txtLocNm.Enabled = false;/*Ramya 17/04/12*/
                    //this.txtAdd1.Enabled = false;
                    //this.txtAdd2.Enabled = false;
                    //this.txtAdd3.Enabled = false;
                    //this.txtArea.Enabled = false;
                    //this.txtCity.Enabled = false;
                    //this.txtCountry.Enabled = false;
                    //this.txtState.Enabled = false;
                    //this.txtZone.Enabled = false;
                    //this.txtPinCode.Enabled = false;
                    //this.txtStdCode.Enabled = false;
                    //this.txtPhone.Enabled = false;
                    //this.txtFax.Enabled = false;
                    //this.txtEmail.Enabled = false;

                    vMainFldVal = dsMain.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
                    SqlStr = "Select top 1 * from " + vMainTblNm + " Where Loc_Code "  + "='" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
                    //this.mthEnableDisableFormControls();
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
            //SqlStr = "select EmployeeCode from  " + vMainTblNm + " where " + this.txtFldName.Text + "<>0";
            //DataSet tDs = new DataSet();
            //tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            //if (tDs.Tables[0].Rows.Count > 0)
            //{
            //    SqlStr = "Could Not Delete Field " + this.txtFldName.Text + " . It Contains Data in Emp_ED_Details Table";
            //    MessageBox.Show(SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            //else
            //{
            //    SqlStr = " select consnm=name from sysobjects where id in (";
            //    SqlStr = SqlStr + " select syscolumns.cdefault from syscolumns";
            //    SqlStr = SqlStr + " inner join sysobjects on (syscolumns.id=sysobjects.id)";
            //    SqlStr = SqlStr + " where syscolumns.name='" + this.txtFldName.Text + "' and sysobjects.name='Emp_ED_Details')";
            //    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

            //    SqlStr = "alter table Emp_ED_Details drop Column " + this.txtFldName.Text;
            //    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

            //}
            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string vDelString = string.Empty;
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();

                vDelString = "Delete from " + vMainTblNm + "  Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                timer1.Enabled = true;
                timer1.Interval = 1000;
                MessageBox.Show("Deleted Successfully.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();

       

                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + ">'" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "'";
                        dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    }
                    else
                    {
                        return;
                    }

                }
                this.mthView();

                this.mthChkNavigationButton();
                //timer1.Enabled = true;
                //timer1.Interval = 1000;
                //MessageBox.Show("Deleted Successfully.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
           
            }

        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.mDeleteProcessIdRecord();
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
            this.mthBindClear();
            
            this.txtManager.Text = "";

            if (this.pAddMode == false && this.pEditMode == false && dsMain.Tables[0].Rows.Count>0)
            {
         
                if (dsMain.Tables[0].Rows[0]["mEmpCode"] != DBNull.Value)
                {
                    string vmEmpCode = string.Empty;
                    vmEmpCode = (string)dsMain.Tables[0].Rows[0]["mEmpCode"];
                    if (string.IsNullOrEmpty(vmEmpCode) == false)
                    {
                        SqlStr = "Select EmployeeName as EmpNM from EmployeeMast where EmployeeCode='" + vmEmpCode + "'";
                        DataSet tdsemp = new DataSet();
                        tdsemp = oDataAccess.GetDataSet(SqlStr, null, 25);
                        this.txtManager.Text = (string)tdsemp.Tables[0].Rows[0]["EmpNm"];
                    }
                 

                }
                //if (dsMain.Tables[0].Rows[0]["city"] != DBNull.Value)
                //{
                //    string vmEmpCode = string.Empty;
                //    vmEmpCode = (string)dsMain.Tables[0].Rows[0]["city"];
                //    if (string.IsNullOrEmpty(vmEmpCode) == false)
                //    {
                //        SqlStr = "Select EmployeeName as EmpNM from EmployeeMast where EmployeeCode='" + vmEmpCode + "'";
                //        DataSet tdsemp = new DataSet();
                //        tdsemp = oDataAccess.GetDataSet(SqlStr, null, 25);
                //        this.txtSftIncharge.Text = (string)tdsemp.Tables[0].Rows[0]["EmpNm"];
                //    }

                //}
               
              
            }
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
            if (ServiceType.ToUpper() != "VIEWER VERSION")//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (dsMain.Tables[0].Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false) //Rup
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;
                        //this.btnLocCode.Enabled = false; //Ramya 08

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
            } //Added by Archana K. on 17/05/13 for Bug-7899
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
                        this.btnLocCode.Enabled = true; //Ramya 08
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
            this.btnEmail.Enabled = false;
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
               // this.btnDelete.Enabled = true; /*Ramya 06/11/12*/
            }
            if (vBtnPrint && this.pPrintButton)
            {
                //this.btnPreview.Enabled = true;
                //this.btnPrint.Enabled = true;
            }
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
           this.btnLogout.Enabled = false;
           // this.btnEmail.Enabled = false;
          
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

        private void btnValidity_Click(object sender, EventArgs e)
        {
            string appPath,appName;
            this.pDataSet = dsMain;
            appPath = this.pAppPath;
           
            appName = "udMasterTransactionList.exe";
            appPath = appPath.Trim()+"\\" + appName.Trim();
            Assembly ass = Assembly.LoadFrom(appPath);
            Form extform = new Form();
            appName = appName.Substring(0, appName.IndexOf("."));
            extform = (Form)ass.CreateInstance(appName.Trim() + ".frmMasterTransactionList", true);
            Type t = extform.GetType();
            t.GetProperty("pAddMode").SetValue(extform, this.pAddMode, null);
            t.GetProperty("pEditMode").SetValue(extform, this.pEditMode, null);
            t.GetProperty("pAppPath").SetValue(extform, this.pAppPath, null);
            t.GetProperty("pPara").SetValue(extform, this.pPara, null);
            t.GetProperty("pParentForm").SetValue(extform, this , null);
            t.GetProperty("pEntryList").SetValue(extform, this.txtValidity.Text.Trim(), null);
            extform.ShowDialog();
            //dsMain.Tables[0].Rows[0]["Validity"] = this.pDataSet.Tables[0].Rows[0]["Validity"];
            if (this.pDataSet.Tables[0].Rows[0]["Validity"] != DBNull.Value)
            {
                this.txtValidity.Text = (string)this.pDataSet.Tables[0].Rows[0]["Validity"];
            }
        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select EmployeeName as EmpNM,EmployeeCode as EmpCode from EmployeeMast where Designation like '%Manager%' order by EmployeeName";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("Please Create Employee with Designation as Manager", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Manager Name";
            vSearchCol = "EmpNM"; //,EmpCode
            vDisplayColumnList = "EmpNM:Employee Name,EmpCode:Employee Code";
            vReturnCol = "EmpNM,EmpCode";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtManager.Text = oSelectPop.pReturnArray[0].Trim();
                dsMain.Tables[0].Rows[0]["mEmpCode"] = oSelectPop.pReturnArray[1].Trim();
            }
           
        }

   
        private void txtPinCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;
            }

        }

        private void btnLocCode_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Loc_code,Loc_desc from Loc_Master order by Loc_code";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Records Found !!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }

            VForText = "Select Location Code";
            vSearchCol = "Loc_code";
            vDisplayColumnList = "Loc_code:Location Code,Loc_desc:Location Name";
            vReturnCol = "Loc_code";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
           // vMainFldVal = dsMain.Tables[0].Rows[0]["Loc_code"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtLocCode.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtLocCode.Text.Trim();
                SqlStr = "Select top 1 * from Loc_Master Where Loc_code='" + vMainFldVal + "' order by Loc_code ";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }
            
            this.mthView();
            this.mthChkNavigationButton();

        }

        private void btnLocNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Loc_code,Loc_desc from Loc_Master order by Loc_code";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Records Found !!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Location Name";
            vSearchCol = "Loc_desc";
            vDisplayColumnList = "Loc_code:Location Code,Loc_desc:Location Name";
            vReturnCol = "Loc_desc";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = dsMain.Tables[0].Rows[0]["Loc_desc"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtLocNm.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtLocNm.Text.Trim();
                SqlStr = "Select top 1 * from Loc_Master Where Loc_desc='" + vMainFldVal + "' order by Loc_code ";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }

            this.mthView();

            this.mthChkNavigationButton();

        }
        //*Ramya 26/12/2011*/

        private void txtStdCode_KeyPress(object sender, KeyPressEventArgs e)  
        {

            string num = "+0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;
            }

        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;
            }
        }

        private void txtFax_KeyPress(object sender, KeyPressEventArgs e) 
        {

            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;
            }
        }

        private void txtFax_Validating(object sender, CancelEventArgs e) 
        {

            if (txtFax.Text.Length > 10)
            {
                this.txtFax.Focus();
                MessageBox.Show("Please Provide valid fax no of 10 characters");
                cValid = true;
                return;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(this.btnExit, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
           if(!string.IsNullOrEmpty(txtEmail.Text.Trim()))
           {
            if (!Regex.Match(txtEmail.Text, @"^[a-zA-Z][a-zA-Z0-9_-]+@[a-zA-Z]+[.]{1}[a-zA-Z]+$").Success)
            {
             
                cValid = false;
                this.txtEmail.Focus();
                MessageBox.Show("Please Provide Valid Emiail Id");
                return;
            }
           }
            if (txtEmail.Text.Length > 100)
            {
                cValid = false;
                this.txtEmail.Focus();
                MessageBox.Show("Email Id Length Should Less Than 100 Characters");
                return;
            }
         
        }

        private void txtStdCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtStdCode.Text.Length > 5)
            {
                cValid = false;
                this.txtStdCode.Focus();
                MessageBox.Show("Std Code Should be Less Than Or Equal to 5 Characters");
                return;
            }
        }

        //private void txtLocCode_Validating(object sender, CancelEventArgs e)
        //{
        //    //if (dsMain.Tables[0].Rows.Count <= 0)
        //    //{
        //    //    return;
        //    //}

        //    DataSet dsData = new DataSet();
        //    string sqlstr;
        //    if (pAddMode)
        //    {
        //        vId = Convert.ToString(0);
        //    }
        //    else
        //    {
        //        vId = dsMain.Tables[0].Rows[0]["id"].ToString();
        //    }

        //    sqlstr = " select Loc_Code from " + vMainTblNm + " where Loc_Code='" + txtLocCode.Text.Trim() + "' and id<>"+vId;
        //    //if (this.pEditMode)
        //    //{
        //    //    sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
        //    //}
        //    dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
        //    if (dsData.Tables[0].Rows.Count > 0)
        //    {
        //        //ToolTip t = new ToolTip();
        //        //string ErrMsg = t.GetToolTip(txtLocCode).ToString();
        //        //errorProvider.SetError(txtLocCode, "Duplicate " + lblLoccode.Text + " value");
        //        MessageBox.Show(lblLoccode.Text.Trim() + " already exists! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        txtLocCode.Focus();
        //        cValid = false;  //Ramya
        //        e.Cancel = true;
        //        return;
        //    }

        //  //  errorProvider.SetError(txtLocCode, "");

        //    if (txtLocCode.Text.Length > 3)
        //    {
        //        cValid = false;
        //        this.txtLocCode.Focus();
        //        MessageBox.Show("Location Code  Should be Less Than Or Equal to 3 Characters", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
            
        //}

        private void btncity_Click(object sender, EventArgs e)  
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select City from city order by city";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //MessageBox.Show("No Record Found");
                //return;
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                    MessageBox.Show("No Record Found");
                //}
                //else
                //{
                //    MessageBox.Show("Please create Category in Category Master");
                //}
                return;

            }

            VForText = "Select City";
            vSearchCol = "City";
            vDisplayColumnList = "City:City";
            vReturnCol = "City";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
              this.txtCity.Text = oSelectPop.pReturnArray[0];
              dsMain.Tables[0].Rows[0]["city"] = oSelectPop.pReturnArray[0].Trim();
            }
        }


        private void btnstate_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select State from State order by State";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //MessageBox.Show("No Record Found");
                //return;
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("Please create state in", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Please create Category in Category Master");
                //}
                return;

            }

            VForText = "Select State";
            vSearchCol = "State";
            vDisplayColumnList = "State:State";
            vReturnCol = "State";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtState.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["state"] = oSelectPop.pReturnArray[0].Trim();
            }

        }

        private void btncountry_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select Country from Country order by Country";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //MessageBox.Show("No Record Found");
                //return;
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("Please create state in", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Please create Category in Category Master");
                //}
                return;

            }

            VForText = "Select Country";
            vSearchCol = "Country";
            vDisplayColumnList = "Country:Country";
            vReturnCol = "Country";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCountry.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["country"] = oSelectPop.pReturnArray[0].Trim();
            }


        }


        //private void txtLocNm_Validating(object sender, CancelEventArgs e)
        //{
        //    //if (dsMain.Tables[0].Rows.Count <= 0)
        //    //{
        //    //    return;
        //    //}

        //    DataSet dsData = new DataSet();
        //    string sqlstr;
        //    if (pAddMode)
        //    {
        //        vId = Convert.ToString(0);
        //    }
        //    else
        //    {
        //        vId = dsMain.Tables[0].Rows[0]["id"].ToString();
        //    }
        //    sqlstr = " select " + vMainField + " from " + vMainTblNm + " where " + vMainField + "='" + txtLocNm.Text.Trim() + "' and id <>"+vId;
     
        //    //if (this.pEditMode)
        //    //{
        //    //    sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
        //    //}
        //    dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
        //    if (dsData.Tables[0].Rows.Count > 0)
        //    {
                
        //        MessageBox.Show(lblLocname.Text.Trim() + " already exists! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        cValid = false;
        //        txtLocNm.Focus();
        //        return;
        //    }

           
        //}

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

        private void txtLocCode_Leave(object sender, EventArgs e)
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            if (pAddMode == false && pEditMode == false)
            {
                return;
            }

            if (pAddMode)
            {
                vId = Convert.ToString(0);
            }
            else if(pEditMode)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    vId = dsMain.Tables[0].Rows[0]["id"].ToString();
                }
                else
                {
                    return;
                }
            }

            sqlstr = " select Loc_Code from " + vMainTblNm + " where Loc_Code='" + txtLocCode.Text.Trim() + "' and id<>" + vId;
            //if (this.pEditMode)
            //{
            //    sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            //}
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                //ToolTip t = new ToolTip();
                //string ErrMsg = t.GetToolTip(txtLocCode).ToString();
                //errorProvider.SetError(txtLocCode, "Duplicate " + lblLoccode.Text + " value");
                MessageBox.Show(lblLoccode.Text.Trim() + " already exists! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLocCode.Focus();
                cValid = false;  //Ramya
                // e.Cancel = true;
                return;
            }



        }

        //private void txtLocNm_Leave(object sender, EventArgs e)
        //{

        //    DataSet dsData = new DataSet();
        //    string sqlstr;
        //    if (pAddMode)
        //    {
        //        vId = Convert.ToString(0);
        //    }
        //    else
        //    {
        //        if (dsMain.Tables[0].Rows.Count > 0)
        //        {
        //            vId = dsMain.Tables[0].Rows[0]["id"].ToString();
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    sqlstr = " select " + vMainField + " from " + vMainTblNm + " where " + vMainField + "='" + txtLocNm.Text.Trim() + "' and id <>" + vId;

        //    //if (this.pEditMode)
        //    //{
        //    //    sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
        //    //}
        //    dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
        //    if (dsData.Tables[0].Rows.Count > 0)
        //    {

        //        MessageBox.Show(lblLocname.Text.Trim() + " already exists! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        cValid = false;
        //        txtLocNm.Focus();
        //        return;
        //    }


        //}

        private void txtCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode || pEditMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btncity_Click (sender, new EventArgs());
                }
            }
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");

            }
        }

        private void txtState_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode || pEditMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnstate_Click(sender, new EventArgs());
                }
            }
        }

        private void txtCountry_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode || pEditMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btncountry_Click(sender, new EventArgs());
                }
            }
        }

        private void txtManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode || pEditMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnManager_Click(sender, new EventArgs());
                }
            }
        }

        private void txtValidity_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode || pEditMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnValidity_Click(sender, new EventArgs());
                }
            }
            
        }

        private void txtAdd1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!(char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
            //    e.Handled = true;

            //if ()
            //{
            //    e.Handled = true;
            //}
           
                //if (key.char=="'")
                //{
                //    e.Handled = true;
                //}
           
        }

        private void txtAdd1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void txtAdd2_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyValue == 222)
            {
                 SendKeys.Send("{BACKSPACE}");

           }
        }

      

        private void txtAdd3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");

            }

        }

        private void txtLocCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");

            }
        }

        private void txtLocNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");

            }
        }

        private void txtArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");

            }
        }

        private void txtZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 222)
            {
                SendKeys.Send("{BACKSPACE}");

            }
        }

        /*Ramya 26/12/2011*/
        private void btnStatuDet_Click(object sender, EventArgs e)
        {
            //Added by Archana 05/02/2013 for Bug-8633 
            FrmStatutoryDet sd = new FrmStatutoryDet(dsMain, vEnabled,MainIcon);    
            sd.ShowDialog();
        }

       
     
       
        
    }
}
