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
using udclsUDF;
using System.Text.RegularExpressions;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpPayHeadMaster
{
    public partial class frmPayHeadMaster : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
   
        string SqlStr = string.Empty;
        string vMainTblNm = "Emp_Pay_Head_Master", vMainField = "Fld_Nm", vMainFldVal = "";
        string vFormula = string.Empty;
        String cAppPId, cAppName;
        char dc_code;  /*Ramya*/
        string dc_ShortNm, dc_FldNm, dc_dAcNm, dc_cAcNm,dc_corder;/*Ramya*/
        uBaseForm.FrmBaseForm vParentForm;
        bool cValid = true;
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmPayHeadMaster(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */
            InitializeComponent();
            
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Pay Head Master";
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

        private void frmPayHeadMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

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


            this.txtDefaRate.pAllowNegative = false;
            this.txtDefaRate.MaxLength = 10;
            this.txtDefaRate.pDecimalLength = 3;

            this.txtDefaAmt.pAllowNegative = false;
            this.txtDefaAmt.MaxLength = 16;
            this.txtDefaAmt.pDecimalLength = 3;
            
            
            //textBox1.DecimalLength = 3;
            //textBox1.MaxLength = 10;
            //txt2.pDecimalLength = 3;
            //txt2.pAllowNegative = false
            
            
            //udclsDGVNumericColumn.CNumEditBox mum1=new 
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;


            //this.dtpDeact.CustomFormat = "dd/MM/yyyy";
            this.dtpDeact.CustomFormat = " ";
            this.dtpDeact.Format = DateTimePickerFormat.Custom;

           
            this.SetMenuRights();
            //this.pDeleteButton = false;

            //Added by Archana K. on 16/05/13 for Bug-7899 start
            startupPath = Application.StartupPath;
            //startupPath = @"D:\Usquare";
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
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnHeadNm.Image = Image.FromFile(fName);
                this.btnFldNm.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnHeadType.Image = Image.FromFile(fName);
                this.btnStPayType.Image = Image.FromFile(fName);
                this.btnCalcPeriod.Image = Image.FromFile(fName);
                this.btnCalcType.Image = Image.FromFile(fName);
                this.btnFormula.Image = Image.FromFile(fName);
                this.btndAc_Name.Image = Image.FromFile(fName);
                this.btncAc_Name.Image = Image.FromFile(fName);
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
                        this.btnHeadNm.Enabled = false;  //Ramya 08/02/12
                        this.btnFldNm.Enabled = false;  //Ramya 08/02/12

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
                        this.btnHeadNm.Enabled = true;  //Ramya 08/02/12
                        this.btnFldNm.Enabled = true;  //Ramya 08/02/12
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
               // this.btnDelete.Enabled = true;
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
            //if(chkDeActivate.Checked)
            this.dtpDeact.Enabled = false;
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
            
            if (this.dsMain.Tables[0].Rows.Count > 0)
            {
                if (dsMain.Tables[0].Rows[0]["HeadTypeCode"].ToString() != "")
                {
                    SqlStr = "Select HeadType From Emp_Pay_Head Where HeadTypeCode='" + dsMain.Tables[0].Rows[0]["HeadTypeCode"].ToString() + "'";
                    DataSet tdsHeadType = new DataSet();
                    tdsHeadType = oDataAccess.GetDataSet(SqlStr, null, 20);
                    this.txtHeadType.Text = tdsHeadType.Tables[0].Rows[0]["HeadType"].ToString();
                }
                else
                {
                    this.txtHeadType.Text = "";
                }
            }
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if ((Boolean)dsMain.Tables[0].Rows[0]["Round_Off"] == false) { this.chkRoundOff.Checked = false; } else { this.chkRoundOff.Checked = true; }
                //if ((Boolean)dsMain.Tables[0].Rows[0]["PayEditable"] == false) { this.chkSlabMaster.Checked = false; } else { this.chkSlabMaster.Checked = true; }
                if ((Boolean)dsMain.Tables[0].Rows[0]["Slab_Appl"] == false) { this.chkSlabMaster.Checked = false; } else { this.chkSlabMaster.Checked = true; } /*Ramya 18/06/13*/
                if ((Boolean)dsMain.Tables[0].Rows[0]["PrInPaySlip"] == false) { this.ChkPaySlip.Checked = false; } else { this.ChkPaySlip.Checked = true; }
                if ((Boolean)dsMain.Tables[0].Rows[0]["PayEditable"] == false) { this.ChkMonthlyEditable.Checked = false; } else { this.ChkMonthlyEditable.Checked = true; }
                if ((Boolean)dsMain.Tables[0].Rows[0]["IsDeactive"] == false) { this.chkDeActivate.Checked = false; } else { this.chkDeActivate.Checked = true; }
               
            }
           
                               
            this.mthBindData();

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
                    this.groupBox4.Enabled = false;
                    this.groupBox5.Enabled = false;
                    this.groupFld.Enabled = false;
                    this.txtCalcType.Enabled = false;
                }
                else
              //Added by Archana K. on 17/05/13 for Bug-7899 end 
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            string SqlStr = "select top 1  " + vMainField + " as Col1 from Emp_Pay_Head_Master order by  " + vMainField + " desc";
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
                //this.btnLocate.Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);

            this.mthChkNavigationButton();
           // this.btnHeadNm.Focus();
            txtHead_Nm.Focus();
        }
        private void mthNew(object sender, EventArgs e)
        {
           
            this.mthBindClear();
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                dsMain.Tables[0].Rows.RemoveAt(0);
                dsMain.Tables[0].AcceptChanges();
            }
            /*Ramya*/

            //if (tdsLoc.Tables.Count > 0)
            //{
            //    if (tdsLoc.Tables[0].Rows.Count > 0)
            //    {
            //        tdsLoc.Tables[0].Rows.RemoveAt(0);
            //        tdsLoc.Tables[0].AcceptChanges();
            //    }
            //}

            this.txtSortOrd.Value = 0;

            DataRow drCurrent;
            drCurrent = dsMain.Tables[0].NewRow();
            dsMain.Tables[0].Rows.Add(drCurrent);


            this.mthBindData();
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            

            //this.rbtnEarnings.Checked = true;
            //DataSet tDs = new DataSet();
            //SqlStr = "select isnull(max(SortOrd),0)+1 as SortOrd from Emp_Pay_Head_Master where edType='E'";
            //tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            //dsMain.Tables[0].Rows[0]["SortOrd"] = tDs.Tables[0].Rows[0]["SortOrd"];
            //this.txtSortOrd.Value = (Int16)dsMain.Tables[0].Rows[0]["SortOrd"];

        }
        //private string mGetSQLServerDefaDateFormate()//<--- Added By Amrendra On 21/01/2012 
        //{
        //    return oDataAccess.GetDataTable("select [dateformat] from master..syslanguages where langid=(select [value] from sys.configurations where [name]='default language')", null, 20).Rows[0][0].ToString();
        //}
        private void btnSave_Click(object sender, EventArgs e)
        {
           

            this.mcheckCallingApplication();
            cValid = true;
            lblMand1.Focus();
            if (cValid == false)
                return;

            cValid = true;
            this.mthChkValidation();
            if (cValid == false)
            {
                return;
            }

            //this.lblMand1.Focus();
                     
            
            this.txtHead_Nm.Focus();
            this.txtFldName.Focus();

            this.Refresh();
            this.mthSave();
   
            this.mthChkNavigationButton();

            this.btnHeadNm.Focus();
        }
        private void mthSave()
        {

            string vSaveString = string.Empty;

            SqlStr = "Select HeadTypeCode From Emp_Pay_Head Where HeadType='" + this.txtHeadType.Text + "'";
            DataSet tdsHeadType = new DataSet();
            tdsHeadType = oDataAccess.GetDataSet(SqlStr, null, 20);
            dsMain.Tables[0].Rows[0]["HeadTypeCode"] = tdsHeadType.Tables[0].Rows[0]["HeadTypeCode"];
            dsMain.Tables[0].Rows[0]["CalcPeriod"] = this.txtCalcPeriod.Text;
            dsMain.Tables[0].Rows[0]["CalcType"] = this.txtCalcType.Text; /*Rup 07/11/2012*/
            dsMain.Tables[0].Rows[0]["StPayType"] = this.txtStPayType.Text;
            if(dtpDeact.Text.Trim()!="")         /*Ramya 19/03/12*/
            {
             dsMain.Tables[0].Rows[0]["DeActFrom"] = this.dtpDeact.Text; //Rup 16/03/12
            }
            //else
            //{

            //}
            dsMain.Tables[0].Rows[0].AcceptChanges();
            
            dsMain.Tables[0].Rows[0].EndEdit();


            if (this.chkRoundOff.Checked) { dsMain.Tables[0].Rows[0]["Round_Off"] = true; } else { dsMain.Tables[0].Rows[0]["Round_Off"] = false; }
            if (this.chkSlabMaster.Checked) { dsMain.Tables[0].Rows[0]["Slab_Appl"] = true; } else { dsMain.Tables[0].Rows[0]["Slab_Appl"] = false; }
            if (this.ChkPaySlip.Checked) { dsMain.Tables[0].Rows[0]["PrInPaySlip"] = true; } else { dsMain.Tables[0].Rows[0]["PrInPaySlip"] = false; }
            if (this.ChkMonthlyEditable.Checked) { dsMain.Tables[0].Rows[0]["PayEditable"] = true; } else { dsMain.Tables[0].Rows[0]["PayEditable"] = false; }
            if (this.chkDeActivate.Checked) { dsMain.Tables[0].Rows[0]["IsDeactive"] = true; } else { dsMain.Tables[0].Rows[0]["IsDeactive"] = false; }

            this.mSaveCommandString(ref vSaveString, "#ID#");
            //if (!IsExit)
            //{
            //    return;
            //}
              dc_ShortNm = dsMain.Tables[0].Rows[0]["Short_Nm"].ToString().Trim();
                dc_FldNm = dsMain.Tables[0].Rows[0]["Fld_Nm"].ToString().Trim();
                dc_dAcNm = dsMain.Tables[0].Rows[0]["dAc_Name"].ToString().Trim();
                dc_cAcNm = dsMain.Tables[0].Rows[0]["cAc_Name"].ToString().Trim();

            if (this.pAddMode)
            {
                SqlStr = "Execute Add_Columns 'Emp_Pay_Head_Details','" + this.txtFldName.Text.Trim() + " Decimal(17,2) Default 0 with Values'";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                SqlStr = "Execute Add_Columns 'Emp_Pay_Head_Details','" + this.txtFldName.Text.Trim() + "YN" + " Bit Default 0 with Values'";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                SqlStr = "Execute Add_Columns 'Emp_Monthly_Payroll','" + this.txtFldName.Text.Trim() + " Decimal(17,2) Default 0 with Values'";  /*Ramya 15/06/12*/
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);  /*Ramya 15/06/12*/



                /*Ramya 30/06/12*/

                DataSet dscorder;
                SqlStr = "Select (max(corder)+1) as corder from dcmast where entry_ty='PP' ";
                dscorder = oDataAccess.GetDataSet(SqlStr, null, 20);
                dc_corder = dscorder.Tables[0].Rows[0]["corder"].ToString();


                if (dsMain.Tables[0].Rows[0]["HeadTypeCode"].ToString().Trim() == "E")
                {
                    dc_code = 'A';
                }
                else
                {
                    dc_code = 'F';
                }
          

                SqlStr = " set dateformat DMY ";
                SqlStr=SqlStr+"If not exists(Select fld_nm from  dcmast where entry_ty='PP' and Fld_Nm='"+dc_FldNm+"') begin";
                SqlStr =SqlStr+ " insert into dcmast([ENTRY_TY],[CODE],[CORDER],[HEAD_NM],[FLD_NM],[DAC_NAME],[DEF_PERT],[DEF_AMT],[ROUND_OFF],[ATT_FILE],[AMTEXPR],[DISP_PERT],[PERT_NAME],[DEFA_FLD],[DISP_SIGN],[A_S],[BEF_AFT],[USER_NAME],[SYSDATE],[EXCL_GROSS],[EXCL_NET],[APGEN],[APGENBY],[STKVAL],[WEFSTKFROM],[WEFSTKTO],[REMARKS],[COMPID],[LDEACTIVE],[DEACTFROM],[LHDRROUND],[VALIDITY],[CAC_NAME],[FLD_DESC],[FCFLD_NM],[CRAC_NAME],[CRAMTEXPR],[CFIELDNAME],[FCDEF_AMT],[FCROUND_OFF],[E_],[N_],[R_],[T_],[I_],[O_],[B_],[X_],[XTVS_],[DSNI_],[MCUR_],[TDS_])";
                SqlStr = SqlStr + " values ('PP','" + dc_code + "','" + dc_corder + "','" + dc_ShortNm + "','" + dc_FldNm + "','";
                if (dc_dAcNm == "")
                {
                    SqlStr = SqlStr + dc_dAcNm ; 
                }
                else
                {
                    SqlStr = SqlStr + '"' + dc_dAcNm + '"';
                }
                SqlStr = SqlStr +"',0,0,0,1,'',0,'',0,'','',2,'ADMIN','" + DateTime.Now.ToShortDateString() + "','','','','',0,'','','',0,0,'',0,'','";
                if (dc_cAcNm == "")
                {
                    SqlStr = SqlStr  + dc_cAcNm ;
                }
                else
                {
                    SqlStr = SqlStr + '"' + dc_cAcNm + '"';
                }
                SqlStr=SqlStr+ "','','','','','',0,0,0,0,0,0,0,0,0,0,0,0,0,0) end";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                SqlStr = "Execute Add_Columns 'EpMain','" + this.txtFldName.Text.Trim() + " Decimal(17,2) Default 0 with Values'";  /*Ramya for Bug-13042*/
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                /*Ramya 30/06/12*/


            }
            else if(this.pEditMode)   /*Ramya */
            {
                DataSet dsDcMast;
                SqlStr = "Select DAC_NAME,CAC_NAME from dcmast where FLD_NM='" + dc_FldNm + "'";
                dsDcMast = oDataAccess.GetDataSet(SqlStr, null, 20);

                if (dsDcMast.Tables[0].Rows[0]["DAC_NAME"].ToString().Trim() == "" && dsDcMast.Tables[0].Rows[0]["CAC_NAME"].ToString().Trim() == "")
                {
                    SqlStr = " Update dcmast set DAC_NAME='" + dc_dAcNm + "' where fld_nm='" + dc_FldNm + "'";
                    SqlStr = " Update dcmast set CAC_NAME='" + dc_cAcNm + "' where fld_nm='" + dc_FldNm + "'";
                }
                 else
                {
                    MessageBox.Show("Please configure Discount & Charges Master Manually");
                }

                //if (dsDcMast.Tables[0].Rows[0]["CAC_NAME"].ToString().Trim() == "")
                //{
                //    SqlStr = " Update dcmast set CAC_NAME='" + dc_cAcNm + "' where fld_nm='" + dc_FldNm + "'";
                //}


            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);

            //this.mthChkNavigationButton();

            //vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim() + dsMain.Tables[0].Rows[0]["EDType"].ToString().Trim();
            vMainFldVal = dsMain.Tables[0].Rows[0]["Fld_Nm"].ToString().Trim();
            SqlStr = "Select top 1 * from Emp_Pay_Head_Master Where " + vMainField + "='" + vMainFldVal + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

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
            sqlstr = sqlstr + " and o.name='Emp_Pay_Head_Master' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                //DataSet dsFlds = oDataAccess.GetDataSet("Select Fld_Nm from Emp_Pay_Head_Master", null, 20);
                vSaveString = "Set DateFormat dmy insert into Emp_Pay_Head_Master ";
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
                vSaveString = "Set DateFormat dmy Update Emp_Pay_Head_Master Set ";
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
            DataSet tDs=new DataSet() ;

            if (string.IsNullOrEmpty(this.txtHead_Nm.Text.Trim()))     /*Ramya 13/3/12*/
            {
                MessageBox.Show("Head Name cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHead_Nm.Focus();
                cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.txtHeadType.Text.Trim()))
            {
                MessageBox.Show("Head Type cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHeadType.Focus();
                 cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.txtShortNm.Text.Trim()))
            {
                MessageBox.Show("Short Name cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtShortNm.Focus();
                cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.txtCalcPeriod.Text.Trim()))
            {
                MessageBox.Show("Calculation Period cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCalcPeriod.Focus();
                cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.txtCalcType.Text.Trim())) /*Rup 07/11/2012*/
            {
                MessageBox.Show("Calculation Type cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCalcPeriod.Focus();
                cValid = false;
                return;
            }
            if (string.IsNullOrEmpty(this.txtFldName.Text.Trim()))
            {
                MessageBox.Show("Field Name cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFldName.Focus();
                cValid = false;
                return;
            }
          
            if (chkDeActivate.Checked) /*Ramya 14/3/12*/
            {
                if (dtpDeact.Value.Date < Convert.ToDateTime("01/01/2000") || dtpDeact.Value.Date>=Convert.ToDateTime("01/01/2079"))
                {
                    //MessageBox.Show("Deactive From Date Should Greater Than Or Equal To 01/01/2000");
                    MessageBox.Show("Year should be between 2000 to 2078", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cValid = false;
                    return;
                }
            }
                     
            if (this.pAddMode)
            {
                SqlStr = "Select Head_NM,Fld_Nm From Emp_Pay_Head_Master where fld_nm='"+this.txtFldName.Text+"'";
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                if (tDs.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Filed Name already used for " + tDs.Tables[0].Rows[0]["Head_Nm"].ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtFldName.Focus();
                    cValid = false;
                    return;

                }
            }

            SqlStr = "Select Head_NM From Emp_Pay_Head_Master where fld_nm='" + this.txtHead_Nm.Text + "'";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            //if (tDs.Tables[0].Rows.Count > 0)//Commented by Pankaj B. on 23-03-2015 for Bug-23365
            if (tDs.Tables[0].Rows.Count > 1)//Added by Pankaj B. on 23-03-2015 for Bug-23365
            {
                MessageBox.Show("Duplicate Head Name " + this.txtHead_Nm.Text, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtHead_Nm.Focus();
                cValid = false;
                return;

            }
            if (this.txtSortOrd.Value > 99)
            {
                MessageBox.Show("Order No. Could not grater than 99 ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtSortOrd.Focus();
                cValid = false;
                return;

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

            this.pAddMode = false;
            this.pEditMode = true;
            //this.txtFldName.ReadOnly = true;
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();
            if (this.chkDeActivate.Checked)
                this.dtpDeact.Enabled = true;

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
            this.btnHeadNm.Focus();
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
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
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
            SqlStr = "select EmployeeCode from Emp_Pay_Head_Details where " + this.txtFldName.Text + "<>0";
            DataSet tDs = new DataSet();
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (tDs.Tables[0].Rows.Count > 0)
            {
                SqlStr = "Could Not Delete Field " + this.txtFldName.Text + " . It Contains Data in Emp_Pay_Head_Details Table";
                MessageBox.Show(SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            SqlStr = "select distinct fld_nm from Emp_Pay_Head_Slab_Master ";   /*Ramya*/
            DataSet stDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (stDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in stDs.Tables[0].Rows)
                {
                    if (dr["fld_Nm"].ToString() == dsMain.Tables[0].Rows[0]["Fld_Nm"].ToString())
                    {
                        SqlStr = "Could Not Delete Field " + this.txtFldName.Text + " . It Contains Data in Pay Head Slab Master";
                        MessageBox.Show(SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                }
            }                  /*Ramya*/

      
            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                SqlStr = " select consnm=name from sysobjects where id in (";    /*Ramya*/
                SqlStr = SqlStr + " select syscolumns.cdefault from syscolumns";
                SqlStr = SqlStr + " inner join sysobjects on (syscolumns.id=sysobjects.id)";
                SqlStr = SqlStr + " where (syscolumns.name='" + this.txtFldName.Text.Trim() + "' or syscolumns.name='" + this.txtFldName.Text.Trim() + "YN"; 
                SqlStr = SqlStr + "') and sysobjects.name='Emp_Pay_Head_Details')";
                //oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                //DataRow dr;
                //dr = oDataAccess.GetDataRow(SqlStr, null, 20);
                //string def_const = dr[0].ToString();


                DataSet ds= oDataAccess.GetDataSet(SqlStr, null, 20);

                string def_const1 = ds.Tables[0].Rows[0][0].ToString();//dr[0].ToString();
                string def_const2 = ds.Tables[0].Rows[1][0].ToString();

                SqlStr = "alter table Emp_Pay_Head_Details drop Constraint " + def_const1;
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                SqlStr = "alter table Emp_Pay_Head_Details drop Constraint " + def_const2;
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                SqlStr = "alter table Emp_Pay_Head_Details drop Column " + this.txtFldName.Text;
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);      /*Ramya*/
                
                SqlStr = "alter table Emp_Pay_Head_Details drop Column " + this.txtFldName.Text+"YN";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);      /*Ramya*/


                string vDelString = string.Empty;
                //vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim() + dsMain.Tables[0].Rows[0]["EDType"].ToString().Trim();
                vMainFldVal = dsMain.Tables[0].Rows[0]["Fld_Nm"].ToString().Trim();

                vDelString = "Delete from Emp_Pay_Head_Master Where ID=" + dsMain.Tables[0].Rows[0]["id"].ToString();
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();
                //this.tdsLoc.Tables[0].Rows[0].Delete();  //Ramya
                

                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from Emp_Pay_Head_Master Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        SqlStr = "Select top 1 * from Emp_Pay_Head_Master Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc"; //ramya 13/02/12
                        dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    }
                    
                    //else    /*commented by ramya*/
                    //{
                    //    return; 
                    //}

                }
                this.mthView();
                this.mthChkNavigationButton();
            }
            //else if(MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void mthBindClear()
        {
            
            this.txtHead_Nm.DataBindings.Clear();
            this.txtHeadType.DataBindings.Clear();
            this.txtHeadType.Text = "";
            this.txtStPayType.DataBindings.Clear();
            this.txtShortNm.DataBindings.Clear();
            this.txtSortOrd.DataBindings.Clear();

            this.txtCalcPeriod.DataBindings.Clear();
            this.txtCalcType.DataBindings.Clear();
            this.txtFldName.DataBindings.Clear();
            this.txtExpr.DataBindings.Clear();

            this.txtDefaRate.DataBindings.Clear();
            this.txtDefaAmt.DataBindings.Clear();
            this.txtFormula.DataBindings.Clear();

            this.txtdAc_Name.DataBindings.Clear();
            this.txtcAc_Name.DataBindings.Clear();
            this.txtRemark.DataBindings.Clear();
            this.dtpDeact.DataBindings.Clear();

            this.chkRoundOff.DataBindings.Clear();    
            this.ChkPaySlip.DataBindings.Clear();
            this.ChkMonthlyEditable.DataBindings.Clear();
            this.chkSlabMaster.DataBindings.Clear(); 
            this.chkDeActivate.DataBindings.Clear();

        }
        private void mthBindData()
        {
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
            }
            else
            {
                
            }

            this.txtHead_Nm.DataBindings.Add("Text", dsMain.Tables[0], "Head_Nm");
            this.txtStPayType.DataBindings.Add("Text", dsMain.Tables[0], "StPayType");
            this.txtShortNm.DataBindings.Add("Text", dsMain.Tables[0], "Short_Nm");
            this.txtSortOrd.DataBindings.Add("Text", dsMain.Tables[0], "SortOrd");

            this.txtCalcPeriod.DataBindings.Add("Text", dsMain.Tables[0], "CalcPeriod");
            this.txtCalcType.DataBindings.Add("Text", dsMain.Tables[0], "CalcType");
            this.txtFldName.DataBindings.Add("Text", dsMain.Tables[0], "Fld_Nm");

            this.txtDefaRate.DataBindings.Add("Text", dsMain.Tables[0], "Def_Rate");
            this.txtDefaAmt.DataBindings.Add("Text", dsMain.Tables[0], "Def_Amt");
            this.txtFormula.DataBindings.Add("Text", dsMain.Tables[0], "Formula");
            this.txtExpr.DataBindings.Add("Text", dsMain.Tables[0], "dNetExpr");/*Ramya 17/06/13*/


            this.txtdAc_Name.DataBindings.Add("Text", dsMain.Tables[0], "dAc_Name");
            this.txtcAc_Name.DataBindings.Add("Text", dsMain.Tables[0], "cAc_Name");
            this.dtpDeact.DataBindings.Add("Text", dsMain.Tables[0], "DeactFrom");
            this.txtRemark.DataBindings.Add("Text", dsMain.Tables[0], "Remark");
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnHeadNm.Enabled = false;
                this.btnFldNm.Enabled = false;

            }
            else
            {
                this.btnHeadNm.Enabled = true;
                this.btnFldNm.Enabled = true;
            }
            
            this.txtHead_Nm.Enabled = vEnabled;
            this.txtHeadType.Enabled = vEnabled;
            this.txtStPayType.Enabled = vEnabled;
            this.txtShortNm.Enabled = vEnabled;
            this.txtSortOrd.Enabled = vEnabled;
            this.txtCalcPeriod.Enabled = vEnabled;
            this.txtCalcType.Enabled = vEnabled;
            
            this.txtDefaRate.Enabled = vEnabled;
            this.txtDefaAmt.Enabled = vEnabled;
            this.txtFormula.Enabled = vEnabled;
            this.txtdAc_Name.ReadOnly = true;
            this.txtcAc_Name.ReadOnly = true;
            this.txtRemark.Enabled = vEnabled;

            this.chkRoundOff.Enabled = vEnabled;
            this.chkSlabMaster.Enabled = vEnabled;
            this.ChkPaySlip.Enabled = vEnabled;
            this.ChkMonthlyEditable.Enabled = vEnabled;
            this.chkDeActivate.Enabled = vEnabled;

           
            this.btnStPayType.Enabled = vEnabled;
            this.btnFormula.Enabled = vEnabled;
            this.btndAc_Name.Enabled = vEnabled;
            this.btncAc_Name.Enabled = vEnabled;
            this.btnCalcPeriod.Enabled = vEnabled;
            this.btnCalcType.Enabled = vEnabled;
            this.txtFldName.Enabled = false;
            this.btnHeadType.Enabled = false;
            this.txtExpr.Enabled = vEnabled; /*Ramya 17/06/13*/
            if (this.pAddMode)
            {
                this.txtFldName.Enabled = true;
                this.txtFldName.Enabled = true;
                this.btnHeadType.Enabled = true;
               
            }

        }
        private void SetMenuRights() //Rup
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
        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpPayHeadMaster.exe";
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

        private void btnHeadNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Head_Nm,Short_Nm,Fld_Nm from Emp_Pay_Head_Master order by Head_Nm,Fld_NM";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }

            VForText = "Select Head Name";
            vSearchCol = "Head_Nm";
            vDisplayColumnList = "Head_Nm:Head Name,Short_Nm:Short Name,Fld_Nm:Field Name";
            vReturnCol = "Head_Nm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtdAc_Name.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtdAc_Name.Text.Trim();
                SqlStr = "Select top 1 * from Emp_Pay_Head_Master Where Head_Nm='" + vMainFldVal + "' order by Head_Nm ";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }



            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnFldNm_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Fld_Nm,Head_Nm,Short_Nm from Emp_Pay_Head_Master order by Head_Nm,Fld_NM";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }


            VForText = "Select Field Name";
            vSearchCol = "Fld_Nm";
            vDisplayColumnList = "Fld_Nm:Field Name,Head_Nm:Head Name,Short_Nm:Short Name";
            vReturnCol = "Fld_Nm,Head_Nm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtdAc_Name.Text = oSelectPop.pReturnArray[1];
                vMainFldVal = this.txtdAc_Name.Text.Trim();
                SqlStr = "Select top 1 * from Emp_Pay_Head_Master Where Head_Nm='" + vMainFldVal + "' order by Head_Nm ";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            }
            //


            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnFormula_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFldName.Text))
            {
                MessageBox.Show("Field Name cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            
            uBaseForm.FrmBaseForm oForm = new frmFormula();
            Type tfrm = oForm.GetType();
            this.pDataSet = dsMain;
            //this.txtAmountExpr. = gridTable.CurrentRow.Cells["String Field"].Value.ToString();
            tfrm.GetProperty("pFormula").SetValue(oForm, this.txtFormula.Text, null);
            tfrm.GetProperty("pPara").SetValue(oForm, this.pPara, null);
            tfrm.GetProperty("pDataSet").SetValue(oForm, dsMain, null);
            tfrm.GetProperty("pParentForm").SetValue(oForm, this, null);
            tfrm.GetProperty("pAddMode").SetValue(oForm, this.pAddMode, null);
            tfrm.GetProperty("pEditMode").SetValue(oForm, this.pEditMode, null);
            oForm.ShowDialog();
            this.txtFormula.Text = dsMain.Tables[0].Rows[0]["Formula"].ToString();
        }

        private void btndAc_Name_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "SELECT Ac_Name,[Group] FROM Ac_Mast order by ac_name";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;

            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }

            VForText = "Select Account Name";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Account Name,[Group]:Group";
            vReturnCol = "Ac_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtdAc_Name.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["dAc_Name"] = this.txtdAc_Name.Text;
            }
        }

        private void btncAc_Name_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "SELECT Ac_Name,[Group] FROM Ac_Mast order by ac_name";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
              if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }

            VForText = "Select Account Name";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Account Name,[Group]:Group";
            vReturnCol = "Ac_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtcAc_Name.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["cAc_Name"] = this.txtcAc_Name.Text;
            }
        }

        private void btnHeadType_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select HeadType,HeadTypeCode,PayEffect From Emp_Pay_Head Order by HeadType";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }

            VForText = "Select Pay Head Type";
            vSearchCol = "HeadType";
            vDisplayColumnList = "HeadType:Head Type,PayEffect:Payroll Effect ";
            vReturnCol = "HeadType";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtHeadType.Text = oSelectPop.pReturnArray[0];
            }
            
        }

        private void btnCalcPeriod_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn();
            col1.ColumnName = "Period";
            col1.DataType = typeof(string);
            dt.Columns.Add(col1);
            DataRow tdr = dt.NewRow();
            tdr["Period"] = "Monthly";
            dt.Rows.Add(tdr);
            DataView dvw = new DataView();
            dvw = dt.DefaultView;

            VForText = "Select Calculation Period";
            vSearchCol = "Period";
            vDisplayColumnList = "Period:Calculation Period";
            vReturnCol = "Period";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCalcPeriod.Text = oSelectPop.pReturnArray[0];
            }

        }

        private void btnCalcType_Click_1t(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn();
            col1.ColumnName = "CalcType";
            col1.DataType = typeof(string);
            dt.Columns.Add(col1);
            for (int i = 1; i<=3; i++)
            {
                DataRow tdr = dt.NewRow();
                switch (i)
                {
                    case 1:
                        tdr["Period"] = "Muster";
                        break;
                    case 2:
                        tdr["Period"] = "Muster";
                        break;
                    default:
                        tdr["Period"] = "Muster";
                        break;
                }
                
                dt.Rows.Add(tdr);
            }
            
            DataView dvw = new DataView();
            dvw = dt.DefaultView;

            VForText = "Select Calculation Period";
            vSearchCol = "Period";
            vDisplayColumnList = "Period:Calculation Period";
            vReturnCol = "Period";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCalcPeriod.Text = oSelectPop.pReturnArray[0];
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLogout_Click(this.btnExit, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);

        }

        private void btnStPayType_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn();
            col1.ColumnName = "PayType";
            col1.DataType = typeof(string);
            dt.Columns.Add(col1);
            for (int i = 1; i <= 11; i++)
            {
                DataRow tdr = dt.NewRow();
                switch (i)
                {
                    case 1:
                        tdr["PayType"] = " ";
                        break;

                    case 2:
                        tdr["PayType"] = "EDLI AcNo-21";
                        break;
                    case 3:
                        tdr["PayType"] = "EDLI AcNo-22";
                        break;
                    case 4:
                        tdr["PayType"] = "ESIC";
                        break;
                    case 5:
                        tdr["PayType"] = "EPS A/C No.10";   
                        break;
                    case 6:
                        tdr["PayType"] = "Gratuity";
                        break;
                    case 7:
                        tdr["PayType"] = "PF A/C No.1";
                        break;
                    case 8:
                        tdr["PayType"] = "PF A/C No.2";
                        break;
                    case 9:
                        tdr["PayType"] = "Professional Tax";
                        break;
                    case 10:
                        tdr["PayType"] = "Voluntary PF A/C No.1";                        
                        break;
                    default:
                        tdr["PayType"] = "TDS";
                        break;
                }

                dt.Rows.Add(tdr);
            }

            DataView dvw = new DataView();
            dvw = dt.DefaultView;

            VForText = "Select Statutory Pay Type";
            vSearchCol = "PayType";
            vDisplayColumnList = "PayType:Pay Type";
            vReturnCol = "PayType";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtStPayType.Text = oSelectPop.pReturnArray[0];
            }
        }

        private void txtSortOrd_Enter(object sender, EventArgs e)
        {
            DataSet tDs = new DataSet();

            int vVal=0;
            if (this.txtSortOrd.Value == 0 && this.pAddMode && this.txtHeadType.Text.Trim()!="")
            {
                SqlStr = "Select isnull(max(a.SortOrd),0) as maxval From Emp_Pay_Head_Master a inner join Emp_Pay_Head b on (a.HeadTypeCode=b.HeadTypeCode) where b.HeadType='" + this.txtHeadType.Text + "'";
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                vVal =Convert.ToInt16(tDs.Tables[0].Rows[0]["maxval"]);
                vVal = vVal + 1;
                this.txtSortOrd.Value = vVal;
            }
            
        }

        private void txtSortOrd_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtSortOrd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 45)
            {
                e.Handled = true;
            }

        }

        private void txtFldName_Validating(object sender, CancelEventArgs e)
        {
            udclsUDF.udclsUDF oudclsUDF = new udclsUDF.udclsUDF();
                                 
            if (this.txtFldName.Text.Trim() != "")
            {
                int fVal = 0;
                fVal = oudclsUDF.fascii(txtFldName.Text.Substring(0, 1));
                if (!(fVal > 96 && fVal < 123) && !(fVal > 64 && fVal < 91))
                {
                    MessageBox.Show("Field Name Starting Character Should Be Alphabet");
                    cValid = false;
                    txtFldName.Focus();
                    return;
                }

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

        private void txtFldName_KeyPress(object sender, KeyPressEventArgs e)
        {
             //Regex regex = new Regex(@"[\d]");
           //Regex regex = new Regex( "^[a-zA-Z0-9]+$");
           // if (regex.IsMatch(txtFldName.Text.Trim()))    
           // {
           //     e.Handled = true;    
           // } 
            if (!(char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;

        }

        private void frmPayHeadMaster_FormClosed(object sender, FormClosedEventArgs e)  /*Ramya*/
        {
            mDeleteProcessIdRecord();
        }

        private void txtHeadType_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnHeadType_Click(sender, new EventArgs());
                }
            }
        }

        private void txtStPayType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnStPayType_Click(sender, new EventArgs());
            }
        }

        private void txtCalcPeriod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnCalcPeriod_Click(sender, new EventArgs());
            }
        }

        private void txtFormula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnFormula_Click(sender, new EventArgs());
            }
        }

        private void txtdAc_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btndAc_Name_Click(sender, new EventArgs());
            }
        }

        private void txtcAc_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btncAc_Name_Click(sender, new EventArgs());
            }
        }

        private void btnCalcType_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn();
            col1.ColumnName = "CalcType";
            col1.DataType = typeof(string);
            dt.Columns.Add(col1);
            for (int i = 1; i <= 3; i++)
            {
                DataRow tdr = dt.NewRow();
                switch (i)
                {
                    case 1:
                        tdr["CalcType"] = "On Attendance";
                        break;
                    case 2:
                        tdr["CalcType"] = "On Production";
                        break;
                    default:
                        tdr["CalcType"] = "Common";
                        break;
                }

                dt.Rows.Add(tdr);
            }

            DataView dvw = new DataView();
            dvw = dt.DefaultView;

            VForText = "Select Calculation Type";
            vSearchCol = "CalcType";
            vDisplayColumnList = "CalcType:Calculation Type";
            vReturnCol = "CalcType";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCalcType.Text = oSelectPop.pReturnArray[0];
            }
        }

        private void txtExpr_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!(char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
            if (!(char.IsLetter(e.KeyChar) ))
                e.Handled = true;
        }

        

        

        //private void txtHead_Nm_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (pAddMode == false && pEditMode == false)
        //    {
        //        if (e.KeyCode == Keys.F2)
        //        {
        //            btnHeadNm_Click(sender, new EventArgs());
        //        }
        //    }
        //}

      

        


             
    }
}
