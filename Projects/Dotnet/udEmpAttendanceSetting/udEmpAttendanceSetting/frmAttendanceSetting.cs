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
using DevExpress.XtraEditors.Repository;
using System.Text.RegularExpressions;
using uNumericTextBox;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udAttendanceSettings
{
    public partial class frmAttendanceSettings : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsGrd=new DataSet();
        string SqlStr = string.Empty;
        string vMainTblNm = "Emp_Attendance_Setting";
        string sqlCommand = string.Empty;
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
        DataSet tDs = new DataSet();
        string vYear = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId=string.Empty,vDeact=string.Empty ,vDeactDt=string.Empty ;
        string  appPath;
        int txtID = 0;
        int contVal = 0;
        int valReturn = 0;
        int flagFiltration1 = 0, flagFiltration2 = 0, flagGridEmpty = 0, flagFiltration4 = 0, flagFiltration5 = 0,flagFiltration6 = 0;
         string strVal=string.Empty ;
         // Added by Archana K. on 17/05/13 for Bug-7899 start
         clsConnect oConnect;
         string startupPath = string.Empty;
         string ErrorMsg = string.Empty;
         string ServiceType = string.Empty;
         // Added by Archana K. on 17/05/13 for Bug-7899 end
        public frmAttendanceSettings(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */ /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPara = args;
            this.pFrmCaption = "Attendance Setting Master";
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

        private void frmAttendanceSettings_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            
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

             // Added by Pratap 17-04-2012
            this.btnEdit.Enabled = false;
            this.dtpDeact.Enabled = false;
            this.btnDelete.Enabled = false;
            // pratap  25-04-2012
            this.txtCFW.pAllowNegative = false;
            this.txtCFW.MaxLength = 6;
            this.txtCFW.pDecimalLength = 3;
            // pratap  25-04-2012
            this.txtAutoCr.pAllowNegative = false;
            this.txtAutoCr.MaxLength = 6;
            this.txtAutoCr.pDecimalLength = 3;
            // pratap  25-04-2012
            this.txtEncash.pAllowNegative = false;
            this.txtEncash.MaxLength = 6;
            this.txtEncash.pDecimalLength = 3;
            
         

            if (pAddMode == false)
            {
                this.chkDeActivate.Checked = false;
                this.dtpDeact.Text = "";
            }
            
        
            

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

            string fName = appPath + @"\bmp\pickup.gif";
            
            if (File.Exists(fName) == true)
            {
         
                this.btnNm.Image = Image.FromFile(fName);
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnDept.Image = Image.FromFile(fName);
                this.btnCate.Image = Image.FromFile(fName);
                this.btnCode.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";

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
            this.mthView();

           
            this.mthFldRefresh(0);
      
          
        }
        private void mthView()
        {
            mthEnableDisableFormControls();
            this.mthGrdRefresh();
            this.mthChkNavigationButton();
            

        }
        private void mthBindData()   /* Pratap 03-04-2012 */
        {
            if (pAddMode==true || pEditMode==true)
                return;            

                        
           
            this.devDgvMain.DataSource = dsGrd.Tables[0];
            
            
            this.devDgvMain.ForceInitialize();

            this.gridView1.RefreshData();
            this.devDgvMain.RefreshDataSource();

          

            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["Id"].Caption="ID";

            gridView1.Columns["Att_Code"].Caption = "Code";
            gridView1.Columns["Att_Nm"].Caption = "Name";
            gridView1.Columns["Lv_Year"].Caption = "Pay Year";/*Ramya Bug-11995*/
            gridView1.Columns["isLeave"].Caption = "Is Leave";
            gridView1.Columns["Dept"].Caption = "Department";
            gridView1.Columns["Cate"].Caption = "Category";
            gridView1.Columns["maxLvCFW"].Caption = "Max Carry Forwarded";
            gridView1.Columns["maxLvEncash"].Caption = "EnCashable";
            gridView1.Columns["LvAutoCr"].Caption = "Auto Credit";
            gridView1.Columns["Loc_desc"].Caption = "Location";
            gridView1.Columns["ldeactive"].Caption = "DeActive Status";
            gridView1.Columns["deactfrom"].Caption = "DeActive Date";
       
            gridView1.Columns["Id"].Visible = false;
            gridView1.Columns["SalPaidDayseffect"].Visible = false;
            gridView1.Columns["SortOrd"].Visible = false;
        
            gridView1.Columns["Loc_Code"].Visible = false;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();
       
            
           
            devDgvMain.ForceInitialize();

        }
     
        private void btnNew_Click(object sender, EventArgs e)
        {
           
     
        mcheckCallingApplication();  //Added by pratap 30-04-2012 Tkt2128
          
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);
            this.mthChkNavigationButton();
            this.txtName.Focus();
            this.ActiveControl = txtName;
            this.chkDeActivate.Checked = false;
            
        }
        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (this.pAddButton)
                {
                    this.btnNew.Enabled = true;
                }
                if (this.pEditButton)
                {
                    this.btnEdit.Enabled = true;
                }
                if (this.pDeleteButton)
                {
                    this.btnDelete.Enabled = false; //pratap 30-04-2012
                }
                this.btnLogout.Enabled = true;
              


            }
            else
            {
                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
                this.btnLogout.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            if (this.dsGrd.Tables[0].Rows.Count == 0 && this.pAddMode == false && this.pEditMode == false)
            {
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
            }
        }
        private void mthNew(object sender, EventArgs e)
        {

            this.mthBindClear();
            
            this.mthEnableDisableFormControls();
           
            

        }
        private void mthBindClear()
        {
            
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.chkLeave.Checked = false;  // pratap  25-04-2012
            this.txtCFW.Text = "";
            this.txtEncash.Text = "";
            this.txtAutoCr.Text = "";
            this.txtLocNm.Text = "";
            this.txtDept.Text = "";
            this.txtCategory.Text = "";
            
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                this.btnNm.Enabled = false;
                this.btnCode.Enabled = false;
                this.dtpDeact.Enabled = false;
            }
            else
            {
                
                this.btnNm.Enabled = true ;
                this.btnCode.Enabled = true;
                this.txtName.Enabled = false;
             
            }
            
            this.txtDept.Enabled = vEnabled;
            this.txtCategory.Enabled = vEnabled;
            this.txtLocNm.Enabled = vEnabled;
            this.btnDelete.Enabled = false;
            this.txtCode.Enabled = vEnabled;
            if(pEditMode==true)
            {
                this.txtCode.Enabled = false;
                this.btnCode.Enabled = false;
            }
            
           
            
            this.txtName.Enabled = vEnabled;
            
            this.chkLeave.Enabled = vEnabled;
            this.txtCFW.Enabled = vEnabled;
            this.txtEncash.Enabled = vEnabled;
            this.txtAutoCr.Enabled = vEnabled;
            this.chkDeActivate.Enabled = vEnabled;
            this.dtpDeact .Enabled =false ;

            if (this.chkLeave.Checked && (this.pAddMode || this.pEditMode))
            {
                this.txtCFW.Enabled = true ;
                this.txtEncash.Enabled = true;
                this.txtAutoCr.Enabled = true;
            }
            else
            {
                this.txtCFW.Enabled = false;
                this.txtEncash.Enabled = false;
                this.txtAutoCr.Enabled = false;
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

                
                this.pEditButton = (rArray[1].ToString().Trim() == "CY" ? true : false);
               
                this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);

            }
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpAttendanceSetting.exe"; // pratap  25-04-2012
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
           
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void btnLocNm_Click(object sender, EventArgs e) // pratap  25-04-2012
        {
            txtID = 3;
            contVal = 1;
            valReturn  = validateTextBox(contVal);
            if (valReturn == 1)
            {
                return;
            }
                   
                
            SqlStr = "select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master union select '' as LocNm,'' as Loc_Code,'' as Add1,'' as Add2,'' as Add3,'' as [Location Name] order by Loc_Desc";
           
           
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Location Name";
            vSearchCol = "LocNm";
            vDisplayColumnList = "LocNm:Location Name,Loc_Code:Location Code";
            vReturnCol = "LocNm,Loc_Code";
            
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            oSelectPop.pSearchImg = appPath;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                
                this.txtLocNm.Text = oSelectPop.pReturnArray[0];
                flagFiltration4 = 4;
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthGrdRefresh();
                    this.mthFldRefresh(0);
                    
                }
                
                
            }
            
            
        }

        

        private void btnDept_Click(object sender, EventArgs e) // pratap  25-04-2012
        {
            txtID = 4;
            contVal = 2;
            valReturn = validateTextBox(contVal);
            if (valReturn == 1)
            {
                return;
            }

            
            SqlStr = "Select Dept from Department order by Dept";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Department Name";
            vSearchCol = "Dept";
            vDisplayColumnList = "Dept:Department";
            vReturnCol = "Dept";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            oSelectPop.pSearchImg = appPath;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                
                this.txtDept.Text = oSelectPop.pReturnArray[0];
                flagFiltration5 = 5;
                if (this.pAddMode == false && this.pEditMode == false)
                {
                   
                    this.mthGrdRefresh();
                    this.mthFldRefresh(0);
                   
                }
                
               
            }
            
            
           
        }

        private void btnCate_Click(object sender, EventArgs e)  // pratap  25-04-2012
        {
            txtID = 5;
            
            SqlStr = "select distinct Cate from category";
          
            
        
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Category Name";
            vSearchCol = "Cate";
            vDisplayColumnList = "Cate:Category";
            vReturnCol = "Cate";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            oSelectPop.pSearchImg = appPath;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
               
                this.txtCategory.Text = oSelectPop.pReturnArray[0];
                flagFiltration6 = 6;
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    
                    this.mthGrdRefresh();
                    this.mthFldRefresh(0);
                    
                }
             
            }
            
         
           
        }

      
        private void mthGrdRefresh()    // pratap  25-04-2012
        {
            if (pAddMode == false)
            {
                SqlStr = "EXECUTE USP_ENT_EMP_ATTENDANCE_SETTINGS '" + txtCode.Text.Trim() + "','" + txtName.Text.Trim() + "'";  // pratap ,'" + txtCode.Text + "'
                SqlStr = SqlStr + ",'" + txtLocNm.Text.Trim() + "','" + txtDept.Text.Trim() + "','" +txtCategory.Text.Trim()+ "'";
                MatchCollection col = Regex.Matches(SqlStr, @"'(.*?)'");

                if (pAddMode == false && pEditMode == false && flagFiltration2 != 2) 
                {
                    if (col[0].Groups[1].ToString() != string.Empty)
                    {
                        
                        SqlStr = SqlStr.Replace("'"+col[0].Groups[1].ToString()+"'", "''");
                    }
                    

                }

                if (pAddMode == false && pEditMode == false && flagFiltration1 != 1) 
                {
                    if (col[1].Groups[1].ToString() != string.Empty)
                    {
                        SqlStr = SqlStr.Replace("'"+col[1].Groups[1].ToString()+"'", "''");
                    }
                    
                }

                if (pAddMode == false && pEditMode == false && flagFiltration4 != 4) 
                {
                    if (col[2].Groups[1].ToString() != string.Empty)
                    {
                        SqlStr = SqlStr.Replace("'"+col[2].Groups[1].ToString()+"'", "''");
                    }
                    
                }

                

                if (pAddMode == false && pEditMode == false && flagFiltration5 != 5)
                {
                    
                    if (col[3].Groups[1].ToString() != string.Empty)
                    {
                        SqlStr = SqlStr.Replace("'"+col[3].Groups[1].ToString()+"'", "''");
                    }
                    
                }


                if (pAddMode == false && pEditMode == false && flagFiltration6 != 6) 
                {
                    if (col[4].Groups[1].ToString() != string.Empty)
                    {
                        SqlStr = SqlStr.Replace("'"+col[4].Groups[1].ToString()+"'", "''");
                    }
                    
                }




               
            }
            if (pAddMode == true || pEditMode == true )
            {
                SqlStr = "EXECUTE USP_ENT_EMP_ATTENDANCE_SETTINGS  '',''";  // pratap ,'" + txtCode.Text + "'
                SqlStr = SqlStr + ",'','',''";
            }

            dsGrd = oDataAccess.GetDataSet(SqlStr, null, 25);
            if (dsGrd.Tables[0].Rows.Count == 0)
            {
                flagFiltration1 = 0;
                flagFiltration2 = 0;
               
                flagFiltration4 = 0;
                flagFiltration5 = 0;
                flagFiltration6 = 0;
            }
            if (dsGrd.Tables[0].Rows.Count <= 0 )
            {
                
                MessageBox.Show("No Records Found",this.pPApplText,MessageBoxButtons.OK, MessageBoxIcon.Information );
               
               
                 
                   SqlStr = "EXECUTE USP_ENT_EMP_ATTENDANCE_SETTINGS  '',''";  // pratap ,'" + txtCode.Text + "'
                   SqlStr = SqlStr + ",'','',''";        

                   dsGrd = oDataAccess.GetDataSet(SqlStr, null, 25);
                //}
                this.mthBindData();
                this.mthFldRefresh(0);
                
                return;
            }
            this.mthBindData();
            flagFiltration1 = 0;
            flagFiltration2 = 0;
   
            flagFiltration4 = 0;
            flagFiltration5 = 0;
            flagFiltration6 = 0;
           
        }

       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.pCancelMode = true;
            this.pAddMode = true;
           
            this.mthBindClear();
          
            if (string.IsNullOrEmpty(vLocNm))
            {
                this.txtLocNm.Text = vLocNm;
            }
            if (string.IsNullOrEmpty(vDept))
            {
                this.txtDept.Text = vDept;
            }
            if (string.IsNullOrEmpty(vCate))
            {
                this.txtCategory.Text = vCate;
            }

            this.pAddMode = false ;
            this.pEditMode = false;
            this.mthGrdRefresh();
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
            this.mthFldRefresh(0);
            this.pCancelMode = false;
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
             mcheckCallingApplication(); //Added by pratap 30-04-2012 Tkt2128
            

            this.pAddMode = false ;
            this.pEditMode = true ;
            
            
            vLocNm = this.txtLocNm.Text;
            vDept = this.txtDept.Text;
            vCate = this.txtCategory.Text;

          
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
            
            if (this.chkDeActivate.Checked == true)
            {
                this.dtpDeact.Enabled = true;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
             int ret=0;
            contVal = 2;
            valReturn = validateTextBox(contVal);
            if (valReturn == 1)
            {
                return;
            }
            contVal = 3;
            valReturn = validateTextBox(contVal);
            if (valReturn == 1)
            {
                return;
            }
            string vSaveString = string.Empty;

            if (this.pAddMode)
            {
                
                this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
            }
            SqlStr = "select count(Att_Nm) from emp_attendance_setting where Att_Nm='" + this.txtName.Text + "' or Att_Code='"+this.txtCode .Text+"'";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (tDs.Tables[0].Rows[0][0].ToString()!="1")
            {
                MessageBox.Show("Duplicate Attendance Name " + this.txtName.Text, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtName.Focus();
                
                return;

            }
            if (this.pEditMode) /*Added by pratap */
            {
                this.mSaveCommandString(ref vSaveString, "#ID#", ",Loc_Desc");
                try
                {
                    ret= oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
                    string sqlstring1="Execute Add_Columns 'Emp_Leave_Maintenance','"+txtCode.Text+"_Opbal decimal (8,3) Default 0 with VaLUES'";
                    oDataAccess.ExecuteSQLStatement(sqlstring1, null, 20, true);
                    sqlstring1 = "Execute Add_Columns 'Emp_Leave_Maintenance','" + txtCode.Text + "_Credit decimal (8,3) Default 0 with VaLUES'";
                    oDataAccess.ExecuteSQLStatement(sqlstring1, null, 20, true);
                    sqlstring1 = "Execute Add_Columns 'Emp_Leave_Maintenance','" + txtCode.Text + "_Availed decimal (8,3) Default 0 with VaLUES'";
                    oDataAccess.ExecuteSQLStatement(sqlstring1, null, 20, true);
                    sqlstring1 = "Execute Add_Columns 'Emp_Leave_Maintenance','" + txtCode.Text + "_EnCash decimal (8,3) Default 0 with VaLUES'";
                    oDataAccess.ExecuteSQLStatement(sqlstring1, null, 20, true);

                  

                }
                catch (Exception )
                {
                }
            }
           
            vLocNm = this.txtLocNm.Text;
            vDept = this.txtDept.Text;
            vCate = this.txtCategory.Text;
            vDeact = this.chkDeActivate.Checked.ToString ();
            vDeactDt = this.dtpDeact.Text;

            this.pAddMode = false;
           
            this.mthGrdRefresh();
            this.pEditMode = false;
            this.mthChkNavigationButton();
           
            this.mthEnableDisableFormControls();
            this.mthBindData();
            this.mthFldRefresh(0);
           

            timer1.Enabled = true;
            timer1.Interval = 1000;
            MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                     

        }
        private void mSaveCommandString(ref string vSaveString, string vkeyField,string vExclField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;
            string vLoc_Code = string.Empty;
            string vIsLeave = string.Empty;
            if (string.IsNullOrEmpty(this.txtCFW.Text.Trim())) { this.txtCFW.Text = "0"; }
            if (string.IsNullOrEmpty(this.txtEncash.Text.Trim())) { this.txtEncash.Text = "0"; }

            if (this.chkLeave.Checked == true) { vIsLeave = "1"; } else { vIsLeave = "0"; }
            
            if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
            {
                SqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
                DataSet tds = new DataSet();
                tds = oDataAccess.GetDataSet(SqlStr, null, 20);
                try
                {
                    vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
                }
                catch(Exception )
                {
                }
                
            }
            
            if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }

            if (this.pAddMode == true)
            {
                vSaveString = " Set DateFormat dmy insert into "+vMainTblNm;
                vfldList = "(Att_Code,Att_Nm,isLeave,Loc_Code,Dept,Cate,maxLvCFW,maxLvEncash,LvAutoCr,ldeactive,deactfrom)";
            
                vfldValList = vfldValList + "'" + this.txtCode.Text.Trim() + "'";
                vfldValList = vfldValList + ",'" + this.txtName.Text.Trim() + "'";
                vfldValList = vfldValList + "," + vIsLeave;
                vfldValList = vfldValList + ",'" + vLoc_Code + "'";
                vfldValList = vfldValList +",'"+this.txtDept.Text.Trim()+"'";
                vfldValList = vfldValList +",'"+this.txtCategory.Text.Trim()+"'";
                vfldValList = vfldValList + "," + this.txtCFW.Text.Trim();
                vfldValList = vfldValList + "," + this.txtEncash.Text.Trim();
                vfldValList = vfldValList + ",'" + this.txtAutoCr.Text.Trim() + "'";
                if (chkDeActivate.Checked == true)
                {
                    vfldValList = vfldValList + ",1";
                    vfldValList = vfldValList + ",'" + this.dtpDeact.Text.Trim() + "'";

                }
                else
                {
                    vfldValList = vfldValList + ",0";   // pratap 30-04-2012
                    vfldValList = vfldValList + ",''";  
                }
                vSaveString = vSaveString + vfldList +" Values( "+ vfldValList +")";
            }
            if (this.pEditMode == true)
            {
                vSaveString = " Set DateFormat dmy Update " + vMainTblNm+" Set " ;
                string vWhereCondn = string.Empty;
                
                vfldValList = vfldValList + " Att_Code=" + "'" + this.txtCode.Text.Trim() + "'";
                vfldValList = vfldValList + ",Att_Nm=" + "'" + this.txtName.Text.Trim() + "'";
                vfldValList = vfldValList + ",isLeave=" + vIsLeave;
                vfldValList = vfldValList + ",Loc_Code=" + "'" + vLoc_Code + "'";
                vfldValList = vfldValList + ",Dept=" + "'" + this.txtDept.Text.Trim() + "'";
                vfldValList = vfldValList + ",Cate=" + "'" + this.txtCategory.Text.Trim() + "'";
                vfldValList = vfldValList + ",maxLvCFW=" + this.txtCFW.Text.Trim()  ;
                vfldValList = vfldValList + ",maxLvEncash=" +this.txtEncash.Text.Trim();
                vfldValList = vfldValList + ",LvAutoCr=" + this.txtAutoCr.Text.Trim() ;

                vfldValList = vfldValList + ",ldeactive=" + "'" + this.chkDeActivate.Checked + "'";
                if (this.chkDeActivate.Checked == true )
                {
                    vfldValList = vfldValList + ",deactfrom=" + "'" + this.dtpDeact.Value +"'";
                }
                else
                {
                    vfldValList = vfldValList + ",deactfrom=" + "''";
                }
                vWhereCondn = " Where id=" + vId; 
                
                vSaveString = vSaveString + vfldValList + vWhereCondn;
            }

            
        }
        private void mthFldRefresh(int rInd)
        {
            if (pAddMode)
            {
                return;
            }
            if (pEditMode)
            {
                return;
            }


            if (gridView1.RowCount == 0)
            {
                return;
            }
            if (gridView1.Columns[0].Name.ToString() == "Id")
            {
                return;
            }
            try
            {

                if (gridView1.GetDataRow(rInd).ItemArray.GetLength(0).ToString() == "")
                {
                    return;
                }
            }
            catch(Exception )
            {
            }
            DataRow drData = gridView1.GetDataRow(rInd);
            
            try
            {
                vId = drData.ItemArray[0].ToString();
                
                this.txtCode.Text = drData["Att_Code"].ToString();
               
                this.txtName.Text = drData["Att_Nm"].ToString();
                if ((Boolean)drData["isLeave"] == true)
                {
                    this.chkLeave.Checked = true;
                }
                else
                {
                    this.chkLeave.Checked = false;
                }
                this.txtCFW.Text = drData["maxlvCFW"].ToString();
                this.txtEncash.Text = drData["maxlvEncash"].ToString();
                this.txtAutoCr.Text = drData["LvAutoCr"].ToString();
                this.txtDept.Text = drData["Dept"].ToString();
                this.txtLocNm.Text = drData["Loc_Desc"].ToString();
                this.txtCategory.Text = drData["Cate"].ToString();
                this.chkDeActivate.Checked = (bool)drData["ldeactive"];
           
        this.dtpDeact.Text = drData["deactfrom"].ToString();
             
                
                

            }
            catch (Exception )
            {
            }         

            

        }

       
        //private void mthFldRefresh(int rInd)
        //{
        //    if (dgvMain.Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    if (dgvMain.Columns[0].Name == "id")
        //    {
        //        return;
        //    }
        //    if (dgvMain.Rows[rInd].Cells["colid"].Value == null)
        //    {
        //        return;
        //    }
        //    //{ colId, colYear, colCode, colAttNm, colIsLeave, colLvCFW, colLvEncash, colAutoCr, colLocation, colDept, colCate });

        //    vId = dgvMain.Rows[rInd].Cells["colid"].Value.ToString();
        //    this.txtYear.Text = dgvMain.Rows[rInd].Cells["colYear"].Value.ToString();
        //    this.txtCode.Text = dgvMain.Rows[rInd].Cells["colCode"].Value.ToString();
        //    this.txtName.Text = dgvMain.Rows[rInd].Cells["colAttNm"].Value.ToString();

        //    if ((Boolean) dgvMain.Rows[rInd].Cells["colIsLeave"].Value == true)
        //    {
        //        this.chkLeave.Checked = true;
        //    }
        //    else
        //    {
        //        this.chkLeave.Checked = false;
        //    }

        //    this.txtCFW.Text = dgvMain.Rows[rInd].Cells["colLvCFW"].Value.ToString();
        //    this.txtEncash.Text = dgvMain.Rows[rInd].Cells["colLvEncash"].Value.ToString();

        //    //this.txtCode.Text = dgvMain.Rows[rInd].Cells["colCode"].Value.ToString();
        //    this.txtLocNm.Text = dgvMain.Rows[rInd].Cells["colLocation"].Value.ToString();
        //    this.txtDept.Text = dgvMain.Rows[rInd].Cells["colDept"].Value.ToString();
        //    this.txtCategory.Text = dgvMain.Rows[rInd].Cells["colCate"].Value.ToString();
            
        //    //if (dgvMain.Rows[rInd].Cells["colFH_Day"].Value.ToString() == "True")
        //    //{

        //    //}
        //    //else
        //    //{

        //    //}
        //}

       

        //private void dgvMain_Click(object sender, EventArgs e)
        //{
        //    //DataGridViewRow cur = new DataGridViewRow();
        //    //cur = dgvMain.CurrentRow;
        //    //if (cur != null)
        //    //{

        //    //    int rInd = dgvMain.CurrentRow.Index;
        //    //    if (rInd != null)
        //    //    {
        //    //        this.mthFldRefresh(rInd);
        //    //    }


        //    //}
        //}

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(vId) == false)
            {
                

                if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    
                    string vDelString = "Delete from Emp_Attendance_Setting Where ID=" + vId;
                        oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                   
                }
                this.mthBindClear();
                this.mthView();
                
            }
            else
            {
                MessageBox.Show("Please Select the Row to be Deleted..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               
            }

        }

      

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);

            contVal = 1;
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pCancelMode = true;
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);

            this.mthFldRefresh(0);

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLogout_Click(this.btnExit, e);
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);

        }

     

        private void chkLeave_CheckedChanged(object sender, EventArgs e) 
        {
            
            if (this.pAddMode == false && this.pEditMode == false)
            {
                return;
            }
            if (this.chkLeave.Checked)
            {
                this.txtCFW.Enabled = true;
                this.txtEncash.Enabled = true;
                this.txtAutoCr.Enabled = true;
           
            }
            else
            {
                this.txtCFW.Enabled=false  ;
                this.txtEncash.Enabled = false;
                this.txtAutoCr.Enabled = false;

                this.txtCFW.Text="";
                this.txtEncash.Text = "";
                this.txtAutoCr.Text = "";
            }

        }



        private void devDgvMain_Click(object sender, EventArgs e) // Added by pratap  09-04-2012
        {
           

             int rInd = gridView1.FocusedRowHandle;
           
            if (rInd >= 0)
            {
                this.mthFldRefresh(rInd);
            }

            
        }

        
        private int validateTextBox(int valFromControl)   /* Added By Pratap on 09-04-2012*/
        {
            switch (valFromControl)
            {
                
                case 2:
                    {
                        if (txtName.Text.Trim() == string.Empty && pCancelMode ==false)
                        {
                            MessageBox.Show("Name should not be blank",this.pPApplText,MessageBoxButtons.OK, MessageBoxIcon.Information );
                            this.txtName.Focus();
                            return 1;
                        }
                        
                        break;

                    }
                case 3:
                    {

                        if (txtCode.Text.Trim() == string.Empty && pCancelMode ==false)
                        {
                            MessageBox.Show("Code should not be blank",this.pPApplText,MessageBoxButtons.OK, MessageBoxIcon.Information );
                            this.txtCode.Focus();
                            return 1;
                        }
                        break;

                    }
                case 4:
                    {
                        Regex regex = new Regex("^[0-9]*$");

                        if (regex.IsMatch(this.txtCFW.Text) || regex.IsMatch (this.txtAutoCr .Text) || regex.IsMatch (this.txtEncash .Text))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Enter numerics only",this.pPApplText,
                        MessageBoxButtons.OK, MessageBoxIcon.Information );
                            return 1;
                        }
                        break;
                    }
                

                   
            }
            return 0;
        }


        private void btnNm_Click(object sender, EventArgs e) // Added By Pratap 19-04-2012
        {
            

            txtID = 1;

            
            SqlStr = "select distinct Att_Nm as [Name],Att_Code as [Code] from emp_attendance_setting  union select '' as [Name],'' as [Code]";
            SqlStr=SqlStr+ " order by Att_Nm ";          


            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select distinct  Name,Code";
            vSearchCol = "Name";
            vDisplayColumnList = "Name:Name,Code:Code";
            vReturnCol = "Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            //udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pSearchImg = appPath;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                
                this.txtName .Text = oSelectPop.pReturnArray[0];
                flagFiltration1= 1; //19
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthGrdRefresh();
                    
                  
                    this.mthFldRefresh(0);

                }


            }
          
            
        }

        
        

        
        private void txtName_MouseClick(object sender, MouseEventArgs e) // pratap  25-04-2012
        {
            contVal = 1;
            valReturn = validateTextBox(contVal);

        }

        private void txtCode_MouseClick(object sender, MouseEventArgs e) // pratap  25-04-2012
        {
            contVal = 1;
            valReturn = validateTextBox(contVal);
            if (valReturn == 1) return;


            contVal = 2;
            valReturn = validateTextBox(contVal);
            if (valReturn == 1) return;

        }

        private void btnCode_Click(object sender, EventArgs e) // pratap  25-04-2012
        {
            txtID = 2;
            contVal = 1;
            valReturn = validateTextBox(contVal);
            if (valReturn == 1)
            {
                return;
            }
            
            SqlStr = "select distinct Att_Code as Code from emp_attendance_setting union select '' as Code order by Att_Code";

       

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Attendance Code";
            vSearchCol = "Code";
            vDisplayColumnList = "Code:Code";
            vReturnCol = "Code";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            oSelectPop.pSearchImg = appPath;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
           
            if (oSelectPop.pReturnArray != null)
            {
               
                this.txtCode .Text = oSelectPop.pReturnArray[0];
                
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    flagFiltration2 = 2;
                    this.mthGrdRefresh();
                    this.mthFldRefresh(0);
                    
                }


            }

        }

        private void txtName_Validating(object sender, CancelEventArgs e) // pratap  25-04-2012
        {
            this.chkLeave.CausesValidation  = false;
            chkDeActivate.CausesValidation = false;
            
            if (pAddMode != false && pEditMode != false)
            {
                return;
            }
            

                if (sender == txtName)
                {
                    contVal = 2;
                    valReturn = validateTextBox(contVal);
                    if (valReturn == 1)
                    {
                        return;
                    }
                }
                if (sender == txtCode)
                {
                    contVal = 3;
                    valReturn = validateTextBox(contVal);
                    if (valReturn == 1)
                    {
                        return;
                    }
                }
                if (sender == txtAutoCr || sender == txtCFW || sender == txtEncash)
                {
                    contVal = 4;
                    valReturn = validateTextBox(contVal);
                    if (valReturn == 1)
                    {
                        return;
                    }
                }
            
        }

        private void chkDeActivate_CheckedChanged(object sender, EventArgs e) // pratap  25-04-2012
        {
        
            if (this.chkDeActivate.Checked)
            {
                if (pEditMode != false )
                {
                    
                    this.dtpDeact.Enabled = true;
                    
                }
               
                
            }
            else
            {
                this.dtpDeact.Enabled = false;
                
            }

         
       
        }

              

        

       

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnNm_Click(sender, new EventArgs());
                }

            }

        }

        private void txtLocNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnLocNm_Click(sender, new EventArgs());
                }

            }
        }

        private void txtDept_KeyDown( object sender , KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                   btnDept_Click( sender , new EventArgs());
                }

            }

        }

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnCate_Click(sender, new EventArgs());
                }

            }

        }

        private void mthBtnRefreshTextBox()
        {
            
            
        }   
        
      
  private void timer1_Tick(object sender, EventArgs e)
        {
           SendKeys.Send("{ENTER}");
           timer1.Enabled = false;
       }

  private void frmAttendanceSettings_FormClosed(object sender, FormClosedEventArgs e)
  {
      mDeleteProcessIdRecord();
  }

        


       
        


       
    }
}
