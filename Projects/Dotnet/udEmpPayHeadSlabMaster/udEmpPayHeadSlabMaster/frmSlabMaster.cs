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
using System.Web;
using udclsDGVDateTimePicker;
using uNumericTextBox;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpPayHeadSlabMaster
{
    public partial class frmSlabMaster : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        DataSet dsNavigate;
        DataSet dsGrd = new DataSet();
        string SqlStr = string.Empty;
        string vMainTblNm = "Emp_Pay_Head_Slab_Master", vMainField1 = "Fld_Nm", vMainFld1Val = "", vMainField2 = "State", vMainFld2Val = "";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        string vFldNm = string.Empty, vState = string.Empty, vCate = string.Empty, vId = string.Empty;
        bool cValid;
        string appPath;
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end
        public frmSlabMaster(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Pay Head Slab Master";
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

        private void frmSlabMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

       
            Thread.CurrentThread.CurrentCulture = ci;

            this.txtPercentage.pAllowNegative = false;
            this.txtPercentage.MaxLength = 7;
            this.txtPercentage.pDecimalLength = 2;

            this.txtAmount.pAllowNegative = false;
            this.txtAmount.MaxLength = 16;
            this.txtAmount.pDecimalLength = 2;

            this.txtRangeFrom.pAllowNegative = false;
            this.txtRangeFrom.MaxLength = 16;
            this.txtRangeFrom.pDecimalLength = 2;

            this.txtRangeTo.pAllowNegative = false;
            this.txtRangeTo.MaxLength = 16;
            this.txtRangeTo.pDecimalLength = 2;

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

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();


            this.SetMenuRights();



            this.mInsertProcessIdRecord();
            this.SetFormColor();


            appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
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
        }

        private void mthView()
        {
            if (dsGrd.Tables[0] != null) 
            {
                if (dsGrd.Tables[0].Rows[0]["Fld_Nm"] != DBNull.Value)
                {
                   SqlStr = "select Head_Nm from Emp_Pay_Head_Master where Fld_Nm='" + dsGrd.Tables[0].Rows[0]["Fld_Nm"].ToString() + "'";
                   
                    DataSet tdsEd = new DataSet();
                    tdsEd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (tdsEd.Tables[0].Rows.Count > 0)
                    {
                        this.txtPayHead.Text = (string)tdsEd.Tables[0].Rows[0]["Head_Nm"];
                        this.txtFldNm.Text = dsGrd.Tables[0].Rows[0]["Fld_Nm"].ToString();
                        this.txtState.Text = dsGrd.Tables[0].Rows[0]["state"].ToString(); // Added By pankaj B. on 20-03-2015 for Bug-25365
                    }
                }
            }
            //Added by Archana K. on 17/05/13 for Bug-7899 start
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
                this.gbSlabDetails.Enabled = false;

            }
            else
                //Added by Archana K. on 17/05/13 for Bug-7899 end 
            mthEnableDisableFormControls();
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            

        }
        private void mthBindData()
        {

            this.dgvMain.DataSource = dsGrd.Tables[0];
            this.dgvMain.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.Name = "colId";
           


            System.Windows.Forms.DataGridViewTextBoxColumn colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colState.HeaderText = "State";
            colState.Name = "colState";

            
            System.Windows.Forms.DataGridViewTextBoxColumn colRangeFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRangeFrom.HeaderText = "Range From";
            colRangeFrom.Name = "colRangeFrom";
            

            System.Windows.Forms.DataGridViewTextBoxColumn colRangeTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRangeTo.HeaderText = "Range To";
            colRangeTo.Name = "colRangeTo";

            System.Windows.Forms.DataGridViewTextBoxColumn colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPercentage.HeaderText = "Percentage";
            colPercentage.Name = "colPercentage";


            System.Windows.Forms.DataGridViewTextBoxColumn colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAmount.HeaderText = "Amount";
            colAmount.Name = "colAmount";
            

            udclsDGVDateTimePicker.MicrosoftDateTimePicker ColsDate = new udclsDGVDateTimePicker.MicrosoftDateTimePicker();
            ColsDate.HeaderText = "From Date";
            ColsDate.Name = "ColsDate";


            System.Windows.Forms.DataGridViewTextBoxColumn colCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCate.HeaderText = "Category";
            colCate.Name = "colCate";

            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colId,colState, colRangeFrom, colRangeTo, colPercentage, colAmount, ColsDate, colCate });
            this.dgvMain.Columns["colId"].Visible = false;

            dgvMain.Columns["colId"].DataPropertyName = "Id";

            dgvMain.Columns["colState"].DataPropertyName = "State";
            dgvMain.Columns["colRangeFrom"].DataPropertyName = "RangeFrom";
            dgvMain.Columns["colRangeTo"].DataPropertyName = "RangeTo";
            dgvMain.Columns["colPercentage"].DataPropertyName = "percentage";
            dgvMain.Columns["colAmount"].DataPropertyName = "Amount";
            dgvMain.Columns["ColsDate"].DataPropertyName = "sDate";
            dgvMain.Columns["colCate"].DataPropertyName = "Cate";
            dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }
        private void mthBindClear()
        {
            this.txtPayHead.Text = "";
            this.txtFldNm.Text = "";
            this.txtState.Text = "";
            this.txtCategory.Text = "";
            this.dtpsDate.Value = DateTime.Now.Date;
            this.txtRangeFrom.Text = "";
            this.txtRangeTo.Text = "";
            this.txtPercentage.Text ="";
            this.txtAmount.Text = "";
            dsGrd.Tables[0].Rows.Clear();
            
        }
        private void mthFldRefresh(int rInd)
        {
            if (dgvMain.Columns[0].Name == "id")
            {
                return;
            }
            if (dgvMain.Rows[rInd].Cells["colid"].Value == null)
            {
                return;
            }
            vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();

           
            this.txtCategory.Text = dgvMain.Rows[rInd].Cells["colCate"].Value.ToString();
            this.dtpsDate.Value = Convert.ToDateTime(dgvMain.Rows[rInd].Cells["ColsDate"].Value.ToString());
            this.txtRangeFrom.Text = dgvMain.Rows[rInd].Cells["ColRangeFrom"].Value.ToString();
            this.txtRangeTo.Text = dgvMain.Rows[rInd].Cells["ColRangeTo"].Value.ToString();
            this.txtPercentage.Text = dgvMain.Rows[rInd].Cells["ColPercentage"].Value.ToString();
            this.txtAmount.Text = dgvMain.Rows[rInd].Cells["ColAmount"].Value.ToString();
            this.txtState.Text = dgvMain.Rows[rInd].Cells["colState"].Value.ToString();   

            
        }
        private void mthGrdRefresh()
        {
            

            SqlStr = "Select * From  " + vMainTblNm ;
            if (string.IsNullOrEmpty(this.txtFldNm.Text.Trim()) == false)
            {
                SqlStr = SqlStr + " where fld_nm='" + this.txtFldNm.Text.Trim() + "'";
            }
            if (string.IsNullOrEmpty(this.txtState.Text.Trim()) == false)
            {
                SqlStr = SqlStr + " and State='" + this.txtState.Text.Trim() + "'";
            }
            if (string.IsNullOrEmpty(this.txtCategory.Text.Trim()) == false)
            {
                SqlStr = SqlStr + " and Cate='" + this.txtCategory.Text.Trim() + "'";
            }

            SqlStr = SqlStr + " order by " + vMainField1 + "," + vMainField2+", RangeFrom";

            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);  
            
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
                if (dsGrd.Tables[0].Rows.Count == 0)
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
                    else
                    {
                        this.btnCancel.Enabled = true;
                        this.btnSave.Enabled = true;
                        vBtnAdd = false;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                    }
                    return;
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
                    if (dsGrd.Tables[0].Rows.Count > 0)
                    {
                        vBtnDelete = true;
                        vBtnEdit = true;
                        vBtnPrint = true;

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

       
       
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            
            
            if (this.pAddMode || this.pEditMode)
            {
          
                vEnabled = true;
                string fName = appPath + @"\bmp\loc-on.gif";   
                if (File.Exists(fName) == true)
                {
                    this.btnPayHead.Image = Image.FromFile(fName);
                    this.btnState.Image = Image.FromFile(fName);
                    this.btnCate.Image = Image.FromFile(fName);
                }

            }
            else
            {
                string fName = appPath + @"\bmp\pickup.gif";
                if (File.Exists(fName) == true)
                {
                    this.btnPayHead.Image = Image.FromFile(fName);
                    this.btnState.Image = Image.FromFile(fName);
                    this.btnCate.Image = Image.FromFile(fName);
                }
            }
            if (this.pEditMode)
            {
                this.btnPayHead.Enabled = false;
            }
            else
            {
                this.btnPayHead.Enabled = true;
            }
            this.dtpsDate.Enabled = vEnabled;
            this.txtPayHead.Enabled = vEnabled;
            this.txtRangeFrom.Enabled = vEnabled;
            this.txtRangeTo.Enabled = vEnabled;
            this.txtPercentage.Enabled = vEnabled;
            this.txtAmount.Enabled = vEnabled;
            this.txtCategory.Enabled = vEnabled;
            this.txtState.Enabled = vEnabled;  
            this.txtFldNm.Enabled = vEnabled;  

            
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {

            this.mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            string SqlStr = "select top 1  " + vMainField1 + " as Col1," + vMainField2 + " as Col2 from " + vMainTblNm + " order by  " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            vMainFld1Val = "";
            vMainFld2Val = "";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["Col1"].ToString()) == false)
                {
                    vMainFld1Val = dsTemp.Tables[0].Rows[0]["Col1"].ToString().Trim();
                    vMainFld2Val = dsTemp.Tables[0].Rows[0]["Col2"].ToString().Trim();
                }
            }
            SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "+" + vMainField2 + "='" + vMainFld1Val + vMainFld2Val + "'";

            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthView();

            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            this.mthChkNavigationButton();


        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFld1Val = dsGrd.Tables[0].Rows[0][vMainField1].ToString().Trim();
            vMainFld2Val = dsGrd.Tables[0].Rows[0][vMainField2].ToString().Trim();

            SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "+" + vMainField2 + "<'" + vMainFld1Val + vMainFld2Val + "' order by " + vMainField1 + "+" + vMainField2 + " desc"+",RangeFrom";
            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            
            this.mthView();
            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            this.mthChkNavigationButton();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFld1Val = dsGrd.Tables[0].Rows[0][vMainField1].ToString().Trim();
            vMainFld2Val = dsGrd.Tables[0].Rows[0][vMainField2].ToString().Trim();
            SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "+" + vMainField2 + ">'" + vMainFld1Val + vMainFld2Val + "' order by " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = false;


            DataSet dsTemp = new DataSet();

            //string SqlStr = "select top 1  " + vMainField1 + " as Col1 from " + vMainTblNm + " order by  " + vMainField1 + " desc" + ",RangeFrom"; // Commented By pankaj B. on 20-03-2015 for Bug-25365
            string SqlStr = "select top 1  " + vMainField1 + " as Col1," + vMainField2 + " as Col2 from " + vMainTblNm + " order by  " + vMainField1 + " desc" + ",RangeFrom"; // Added By pankaj B. on 20-03-2015 for Bug-25365
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            vMainFld1Val = "";
            vMainFld2Val = "";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["Col1"].ToString()) == false)
                {
                    vMainFld1Val = dsTemp.Tables[0].Rows[0]["Col1"].ToString().Trim();
                    vMainFld2Val = dsTemp.Tables[0].Rows[0]["Col2"].ToString().Trim();
                }
            }

            if (vMainFld2Val=="")
                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "='" + vMainFld1Val+ "' order by State,RangeFrom";
            else
                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "='" + vMainFld1Val+ "' and " + vMainField2 + "='" + vMainFld2Val+ "' order by State,RangeFrom";

            
            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();
            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            
            this.mthChkNavigationButton();

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);
            this.mthChkNavigationButton();

        }
        private void mthNew(object sender, EventArgs e)
        {

            this.mthBindClear();
            this.pAddMode = true;
            this.pEditMode=false;
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
            this.txtPayHead.Focus();


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            string vSaveString = string.Empty;
            cValid = true;                              
            this.mthChkSaveValidation(ref cValid);
            if (cValid == false)
                return;                                 



            if (this.pAddMode)
            {

                this.mSaveCommandString(ref vSaveString, "#ID#", "");
                oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
            }
            if (this.pEditMode)
            {
                this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
            }
            vMainFld1Val = this.txtFldNm.Text;
            vMainFld2Val=this.txtState.Text;
            vCate = this.txtCategory.Text;

            SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "+" + vMainField2 + "='" + vMainFld1Val + vMainFld2Val + "'";
            if (txtCategory.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and  Cate='" + txtCategory.Text.Trim() + "'";
            }
            SqlStr = SqlStr + "  order by " + vMainField1 + "+" + vMainField2 + ",RangeFrom";

            dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();


        }

        private void mthChkSaveValidation(ref bool cValid)         
        {
            

            if (string.IsNullOrEmpty(this.txtPayHead.Text.Trim()))       
            {
                MessageBox.Show("Pay Head Name cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                txtPayHead.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.dtpsDate.Text.Trim()))       
            {
                MessageBox.Show("From Date cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                dtpsDate.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtRangeFrom.Text.Trim()))    
            {
                MessageBox.Show("Range From cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                txtRangeFrom.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtRangeTo.Text.Trim()))       
            {
                MessageBox.Show("Range To cannot be empty", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                txtRangeTo.Focus();
                return;
            }


            if (Convert.ToDouble(txtRangeFrom.Text.Trim()) > Convert.ToDouble(txtRangeTo.Text.Trim()))
            {
                MessageBox.Show("To Range should greater than or equalto From Range");
                cValid = false;
                txtRangeTo.Focus();
                return;
            }
            if (dtpsDate.Value.Date < Convert.ToDateTime("01/01/2000") || dtpsDate.Value.Date >= Convert.ToDateTime("01/01/2079"))
            {
                MessageBox.Show("Year should be between 2000 to 2078", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cValid = false;
                return;
            }


            if (pAddMode)
            {
                vId = Convert.ToString(0);
            }
            SqlStr = "Set DateFormat dmy execute Usp_Ent_Pay_Head_Slab_Master '" + txtFldNm.Text.Trim() + "'," + txtRangeFrom.Text.Trim() + "," + txtRangeTo.Text.Trim();           
            SqlStr = SqlStr + ",'" + txtCategory.Text.Trim() + "','" + txtState.Text.Trim() + "','"+dtpsDate.Value.Date.ToShortDateString() +"'," + vId;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (dsMain.Tables[0].Rows[0][0].ToString() == "Y")
            {
               
                MessageBox.Show("Duplicate Range value "+dsMain.Tables[0].Rows[0][1].ToString());
                txtRangeFrom.Focus();
                cValid = false;
                return;

            }

        }
        
        private void mSaveCommandString(ref string vSaveString, string vkeyField, string vExclField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            if (this.txtRangeFrom.Text.Trim() == "")
            {
                this.txtRangeFrom.Text = "0";
            }
            if (this.txtRangeTo.Text.Trim() == "")
            {
                this.txtRangeTo.Text = "0";
            }
            if (this.txtPercentage.Text.Trim() == "")
            {
                this.txtPercentage.Text = "0";
            }
            if (this.txtAmount.Text.Trim() == "")
            {
                this.txtAmount.Text = "0";
            }

            if (this.pAddMode == true)
            {
                vSaveString = "Set DateFormat dmy insert into " + vMainTblNm;
                vfldList = "(State,Fld_Nm,RangeFrom,RangeTo,Percentage,Amount,Sdate,Cate)";
                vfldValList = "'" + this.txtState.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.txtFldNm.Text.Trim() + "'";
                vfldValList = vfldValList + "," + this.txtRangeFrom.Text.Trim();
                vfldValList = vfldValList + "," + this.txtRangeTo.Text.Trim();
                vfldValList = vfldValList + "," + this.txtPercentage.Text.Trim();
                vfldValList = vfldValList + "," + this.txtAmount.Text.Trim() ;
                vfldValList = vfldValList + ",'" + this.dtpsDate.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.txtCategory.Text.Trim() + "'";
                vSaveString = vSaveString + vfldList + " Values( " + vfldValList + ")";
            }
            if (this.pEditMode == true)
            {
                vSaveString = "Set DateFormat dmy Update " + vMainTblNm + " Set ";
                string vWhereCondn = string.Empty;
                vfldValList = vfldValList + "State='" + this.txtState.Text.Trim() + "'";
                vfldValList = vfldValList + ",Fld_Nm='" + this.txtFldNm.Text.Trim() + "'";
                vfldValList = vfldValList + ",RangeFrom=" + this.txtRangeFrom.Text.Trim();
                vfldValList = vfldValList + ",RangeTo=" + this.txtRangeTo.Text.Trim();
                vfldValList = vfldValList + ",Percentage=" + this.txtPercentage.Text.Trim() ;
                vfldValList = vfldValList + ",Amount=" + this.txtAmount.Text.Trim() ;
                vfldValList = vfldValList + ",Sdate='" + this.dtpsDate.Text.Trim() + "'";
                vfldValList = vfldValList + ",Cate='" + this.txtCategory.Text.Trim() + "'";
                vWhereCondn = " Where id=" + vId;
                vSaveString = vSaveString + vfldValList + vWhereCondn;
            }


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
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

            vFldNm = this.txtFldNm.Text;
            vState = this.txtState.Text;
            vCate = this.txtCategory.Text;


            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            
            if (string.IsNullOrEmpty(vFldNm))
            {
                this.txtFldNm.Text = vFldNm;
            }
            if (string.IsNullOrEmpty(vState))
            {
                this.txtState.Text = vState;
            }
           
            if (string.IsNullOrEmpty(vCate))
            {
                this.txtCategory.Text = vCate;
            }

            this.pAddMode = false;
            this.pEditMode = false;
            if (string.IsNullOrEmpty(vFldNm))
            {
                this.btnLast_Click(sender, e);
            }
            else
            {
                vMainFld1Val = vFldNm;
                vMainFld2Val = vState;
                

                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "+" + vMainField2 + "='" + vMainFld1Val + vMainFld2Val + "'";
                SqlStr = SqlStr + "  order by " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
                dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            this.mthView();

            if (dgvMain.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            this.mthChkNavigationButton();
            

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication(); 


            if (this.dsGrd.Tables[0].Rows.Count <= 0)  
            {
                return;
            }
           
            if (string.IsNullOrEmpty(vId) == false)
            {
                if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string vDelString = "Delete from " + vMainTblNm + " Where ID=" + vId;
                    oDataAccess.ExecuteSQLStatement(vDelString, null, vTimeOut, true);
                    
                   
                    this.txtState.Text = "";
                    this.txtCategory.Text = "";
                    this.dtpsDate.Value = DateTime.Now.Date;
                    this.txtRangeFrom.Text = "";
                    this.txtRangeTo.Text = "";
                    this.txtPercentage.Text = "";
                    this.txtAmount.Text = "";
                    

                    

                    vMainFld1Val = this.txtFldNm.Text.Trim();
                    vMainFld2Val = this.txtState.Text.Trim();
                    Double vMainRangeFrom=Convert.ToDouble(this.txtRangeFrom.Text.Trim());
                    SqlStr = "Select fld_nm from " + vMainTblNm + " Where " + vMainField1 + "='" + vMainFld1Val+ "'";
                    dsNavigate = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsNavigate.Tables[0].Rows.Count > 0)
                    {
                        SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + "='" + vMainFld1Val + "'";
                        this.dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                        this.mthBindData();
                    }
                    else
                    {
                        this.btnLast_Click(sender, e);
                    }
                                  
                    this.mthGrdRefresh();
                    if (dgvMain.CurrentRow != null)
                    {
                        mthFldRefresh(dgvMain.CurrentRow.Index);
                    }

                  
                    this.mthView();
                    if (dgvMain.Rows.Count > 0)
                    {
                        this.mthFldRefresh(0);
                    }

                }

           
            }
            else
            {
                MessageBox.Show("Please Select the Row to be Deleted..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


        }
        private void mthDelete()
        {
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            cAppName = "udEmpPayHeadSlabMaster.exe";
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
        }

        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPayHead_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            if (this.pAddMode == false && this.pEditMode == false)
            {
  
                SqlStr = "Select Distinct H.Head_Nm,H.Short_Nm,S.Fld_Nm From " + vMainTblNm + " S inner join Emp_Pay_Head_Master  H on (s.Fld_Nm=H.Fld_Nm)  order by H.Head_Nm,S.Fld_NM";
                vDisplayColumnList = "Head_Nm:Head Name,Short_Nm:Short Name,Fld_Nm:Field Name";
                vReturnCol = "Head_Nm,Short_Nm,Fld_Nm";
            }
            else  
            {
                SqlStr = "Select Head_Nm,Short_Nm,Fld_Nm from Emp_Pay_Head_Master order by Head_Nm,Fld_NM";
                vDisplayColumnList = "Head_Nm:Head Name,Short_Nm:Short Name,Fld_Nm:Field Name";
                vReturnCol = "Head_Nm,Short_Nm,Fld_Nm";  
            }




            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
           

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }

            VForText = "Select Head Name";
            vSearchCol = "Head_Nm";
            
            
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
          
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
                  
            
            if (oSelectPop.pReturnArray != null)
            {
                this.txtPayHead.Text = oSelectPop.pReturnArray[0];

                this.txtFldNm.Text = oSelectPop.pReturnArray[2];
                
                
                if (this.pAddMode == false && this.pEditMode == false)
                {

                    this.txtState.Text = "";    
                    this.txtCategory.Text = "";  
                    
                    vMainFld1Val = this.txtFldNm.Text;
                    SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + " ='" + txtFldNm.Text.Trim()+"'";
                    SqlStr = SqlStr + " order by  " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
                    
                    dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthView();
                    if(dsGrd.Tables[0].Rows.Count>0)
                    {
                        this.mthFldRefresh(0);
                    }
                    this.mthChkNavigationButton();
                }
                if (this.pAddMode)
                {
                    SqlStr = "Select * from " + vMainTblNm + " Where Fld_Nm='" + this.txtFldNm.Text + "'";
                    vMainFld1Val = this.txtFldNm.Text;
                    if (this.txtState.Text.Trim() != "")
                    {
                        SqlStr = SqlStr + " and  State='" + this.txtState.Text + "'";
                    }

                    if (txtCategory.Text.Trim() != "")
                    {
                        SqlStr = SqlStr + " and  Cate='" + txtCategory.Text.Trim() + "'";
                    }

                    SqlStr = SqlStr + " order by  " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
                    dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsGrd.Tables[0].Rows.Count > 0)
                    {
                        this.mthView();
                    }
                }
            }
        }

        private void btnState_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            if (this.pAddMode == false && this.pEditMode == false)
            {
                SqlStr = "Select Distinct State from " + vMainTblNm+" where "+vMainField1+"='"+this.txtFldNm.Text.Trim();
                SqlStr=SqlStr+"' order by State";
                vReturnCol = "State";
            }
            else
            {
                SqlStr = "Select State,Code from State order by State";
                vReturnCol = "State,Code";
            }
          
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
                return;
            }

            

            VForText = "Select State Name";
            vSearchCol = "State";
            vDisplayColumnList = "State:State Name,Code:State Code";
            
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
            }
            if (this.pAddMode || (pAddMode==false &&pEditMode==false) )  
            {
                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + " ='" + vMainFld1Val + "'";
                if (this.txtState.Text.Trim() != "")
                {
                    SqlStr = SqlStr + " and  State='" + this.txtState.Text + "'";
                }


                SqlStr = SqlStr + " order by  " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
                dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                if (dsGrd.Tables[0].Rows.Count > 0)
                {
                    this.mthView();
                }
            }
        }

        private void btnCate_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            if (this.pAddMode == false && this.pEditMode == false)
            {
                SqlStr = "Select Distinct Cate from " + vMainTblNm + " where " + vMainField1 + "='" + this.txtFldNm.Text.Trim();
                if (txtState.Text.Trim() != "")
                {
                    SqlStr = SqlStr + "' and " + vMainField2 + "='" + this.txtState.Text.Trim();
                }
                SqlStr = SqlStr + "' order by Cate";
            }
            else
            {
                SqlStr = "Select Cate from Category order by Cate";
            }
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found");
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
            if (this.pAddMode == false && this.pEditMode == false)
            {
                this.mthGrdRefresh();
            }
            if (this.pAddMode || (pAddMode == false && pEditMode == false))
            {
                SqlStr = "Select * from " + vMainTblNm + " Where " + vMainField1 + " ='" + vMainFld1Val + "'";
                if (this.txtState.Text.Trim() != "")
                {
                    SqlStr = SqlStr + " and  State='" + this.txtState.Text + "'";
                }

                if (txtCategory.Text.Trim() != "")
                {
                    SqlStr = SqlStr + " and  Cate='" + txtCategory.Text.Trim() + "'";
                }

                SqlStr = SqlStr + " order by  " + vMainField1 + "+" + vMainField2 + ",RangeFrom";
                dsGrd = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                if (dsGrd.Tables[0].Rows.Count > 0)
                {
                    this.mthView();
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
            btnLogout_Click(this.btnExit, e);
        }

        private void txtState_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnState_Click(sender, new EventArgs());
            }
        }

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
               btnCate_Click(sender, new EventArgs());
            }

        }

        private void txtPayHead_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnPayHead_Click(sender, new EventArgs());
                }
            }
        }

        private void frmSlabMaster_FormClosed(object sender, FormClosedEventArgs e)    
        {
            mDeleteProcessIdRecord(); 
        }

    

      

     

    }
}
