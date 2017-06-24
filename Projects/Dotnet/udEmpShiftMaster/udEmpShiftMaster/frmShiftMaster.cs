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
using System.Reflection;

using System.Globalization;
using System.Data.SqlClient;
using System.Threading;
using uNumericTextBox;
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpShiftMaster
{
    public partial class frmShiftMaster : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        string SqlStr = string.Empty;
        string vMainTblNm = "Emp_Shift_Master", vMainField = "Shift_Code", vMainFldVal = "";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        bool cValid = true;
        bool vCancel = false;/*Archana 19/03/13 for Bug-13508*/
        short vTimeOut = 25;  /*Ramya 21/01/13*/
        //Added by Archana K.on 19/03/13 for Bug-8634 start
            decimal vHours1 = 0, vHour2 = 0, vHour3 = 0;
            DateTime shiftfrom, shiftto, Todate, Fromdate;
            int vShiftFrom = 0, vShiftTo = 0,sValid=0;
        //Added by Archana K. on 19/03/13 for Bug-8634 end
        //uBaseForm.FrmBaseForm vParentForm;
        //Added by Archana K. on 16/05/13 for Bug-7899 start
         clsConnect oConnect;
         string startupPath = string.Empty;
         string ErrorMsg = string.Empty;
         string ServiceType = string.Empty;
        //Added by Archana K. on 16/05/13 for Bug-7899 end

        public frmShiftMaster(string[] args)
        {
            this.pDisableCloseBtn = true;  /*close disable*/
            InitializeComponent(); 
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Employee Shift Master";
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

        private void frmShiftMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.txtMinHrs.pAllowNegative = false;
            //Commented by Archana K. on 19/03/13 for Bug-8634 start
                //this.txtMinHrs.MaxLength = 1;
                //this.txtMinHrs.pDecimalLength = 0;
                //this.txthalfDayHrs.MaxLength = 1;
                //this.txthalfDayHrs.pDecimalLength = 0;
            //Commented by Archana K. on 19/03/13 for Bug-8634 end

            this.txtMinHrs.pDecimalLength = 2;//changed by Archana K. on 19/03/13 for Bug-8634
            this.txthalfDayHrs.pDecimalLength = 2;//changed by Archana K. on 19/03/13 for Bug-8634

            this.txthalfDayHrs.pAllowNegative = false;
      

            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            //this.dtpsDate.CustomFormat = "dd/MM/yyyy";
            //this.dtpsDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;

            this.dtpSFrom.CustomFormat = "HH:mm";
            this.dtpSFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSTo.CustomFormat = "HH:mm";
            this.dtpSTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;

            this.dtpLFrom.CustomFormat = "HH:mm";
            this.dtpLFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLTo.CustomFormat = "HH:mm";
            this.dtpLTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;

            //Added by Archana K.on 05/03/13 for Bug-8634 start
            this.dtpT1From.CustomFormat = "HH:mm";
            this.dtpT1From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpT1To.CustomFormat = "HH:mm";
            this.dtpT1To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpT2From.CustomFormat = "HH:mm";
            this.dtpT2From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpT2To.CustomFormat = "HH:mm";
            this.dtpT2To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            //Added by Archana K.on 05/03/13 for Bug-8634 end

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
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnShiftCode.Image = Image.FromFile(fName);
                this.btnShiftNm.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnSftIncharge.Image = Image.FromFile(fName);
                this.btnLocNm.Image = Image.FromFile(fName); 

            }
          

        }
        private void mthBindClear()
        {

            this.txtShiftCode.DataBindings.Clear();
            this.txtShiftNm.DataBindings.Clear();
            this.txtSftIncharge.DataBindings.Clear();
            this.txtRemark.DataBindings.Clear();
            this.txtMinHrs.DataBindings.Clear();
            this.txthalfDayHrs.DataBindings.Clear();

            this.txtSftIncharge.Text = "";
            this.txtLocNm.Text = "";
            //this.txtMinHrs.Text = "0"; //Commented by Archana K. on 15/03/13 for Bug-8634
            //this.txthalfDayHrs.Text = "0"; //Commented by Archana K. on 15/03/13 for Bug-8634
            //Added by Archana K. on 15/03/13 for Bug-8634 start
            this.txtMinHrs.Text = "0.00";//Changed by Archana K.for Bug-8634 
            this.txthalfDayHrs.Text = "0.00";//Changed by Archana K.for Bug-8634 
            this.dtpSTo.Text = "00:00";
            this.dtpSFrom.Text = "00:00";
            this.dtpLFrom.Text = "00:00";
            this.dtpLTo.Text = "00:00";
            this.dtpT1From.Text = "00:00";
            this.dtpT1To.Text = "00:00";
            this.dtpT2From.Text = "00:00";
            this.dtpT2To.Text = "00:00";
            //Added by Archana K. on 15/03/13 for Bug-8634 end
        }
        private void mthBindData()
        {
            if (dsMain.Tables[0].Rows.Count > 0)
            {

                if (dsMain.Tables[0].Rows[0]["Default_Shift"].ToString() == "True")
                {
                    this.chkDefaShift.Checked = true;
                }
                else
                {
                    this.chkDefaShift.Checked = false;
                }

                this.txtShiftCode.DataBindings.Add("Text", dsMain.Tables[0], "Shift_Code");
                this.txtShiftNm.DataBindings.Add("Text", dsMain.Tables[0], "Shift_Name");
                this.txtMinHrs.DataBindings.Add("Text", dsMain.Tables[0], "ShiftMinHrs");
                this.txthalfDayHrs.DataBindings.Add("Text", dsMain.Tables[0], "HalfDayHrs");
                this.txtRemark.DataBindings.Add("Text", dsMain.Tables[0], "Remark");
                //this.txtSftIncharge.DataBindings.Add("Text", dsMain.Tables[0], "Incharge_Code");
                //this.txthalfDayHrs.DataBindings.Add("Text", dsMain.Tables[0], "HalfDayHrs");
                //this.dtpSFrom.DataBindings.Add("Text", dsMain.Tables[0], "ShiftFrom");
            }
        }
        private void dtp1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpSTo_ValueChanged(object sender, EventArgs e)
        {
           //Commented by Archana K. on 13/04/13 for Bug-8634 start
            //String Str1=string.Empty ,Str2=string.Empty ;
            //this.mthConverTime(dtpSFrom, ref Str1);
            //this.mthConverTime(dtpSTo, ref Str2);
            //int t1 = Convert.ToInt16(Str1.Substring(0, 2));
            //int t2 = Convert.ToInt16(Str2.Substring(0, 2));
            //int hh = 0;
            //if (t1 > t2)
            //{

            //}
            //else
            //{
            //    hh = t2 - t1+12;
            //}
           //Commented by Archana K. on 13/04/13 for Bug-8634 end
//            if(t2>12)
//            {
//                t2 = t2-12;
//            }
//            if (t1 > 12)
//            {
//                t1 = t1 - 12;
//            }
//            int hh = t1+t2;
//            int mm = Convert.ToInt16(Str1.Substring(3, 2)) - Convert.ToInt16(Str2.Substring(3, 2));
//            //hh = Math.Abs(hh);
//            //if (hh > 12)
//            //{
//            //    int hh1 = Convert.ToInt16(Str2.Substring(0, 2));
//            //    hh = hh - 12 +hh1 ;
//            //}
//            mm = Math.Abs(mm);
//            this.txtSfDuration.Text = Convert.ToString(hh) + ":" + Convert.ToString(mm);
////            TimeSpan tt = new TimeSpan(dtt2.Ticks ,dtt1.Ticks );
//           // TimeSpan t;
           // TimeSpan t1 = new TimeSpan(dtpSFrom.Value.Ticks);
           // TimeSpan t2 = new TimeSpan(dtpSTo.Value.Ticks);
           // DateTime Dt1 =Convert.ToDateTime(dtpSFrom.Text);
           // DateTime Dt2 = Convert.ToDateTime(dtpSTo.Text);
           // DateTime Dt3 = Convert.ToDateTime(DateTime.Now.Date);
           // TimeSpan t3=  new TimeSpan (DateTime.Now.Date.Ticks);
            
           //// Dt3 = Dt2 - Dt1 - Dt3;
           // if (dtpSTo.Value > dtpSFrom.Value)
           // {
           //     t = new TimeSpan(dtpSTo.Value.Ticks - dtpSFrom.Value.Ticks);
           // }
           // else
           // {
           //     DateTime dt = Convert.ToDateTime("12:00 AM");
           //    // t = new TimeSpan(dtpSFrom.Value.Ticks-dtpSTo.Value.Ticks-dt.Ticks-t3);
           //     t = new TimeSpan(Dt1.Ticks -Dt2.Ticks-Dt3.Ticks );
           // }
           // string vDuretion; 
           // vDuretion= t.ToString();
           // vDuretion=vDuretion.Substring(0,8);
           // vDuretion=vDuretion.Substring(0,5);
           // this.txtSfDuration.Text = vDuretion;
        }
        private void dtpLTo_ValueChanged(object sender, EventArgs e)
        {
            //Commented by Archana k. on 21/03/13 for Bug-8634 start 
                //TimeSpan t = new TimeSpan(dtpLTo.Value.Ticks - dtpLFrom.Value.Ticks);
                //string vDuretion;
                //vDuretion = t.ToString();
                //vDuretion = vDuretion.Substring(0, 8);
                //vDuretion = vDuretion.Substring(0, 5);
                //this.txtMinHrs.Text = vDuretion;
            //Commented by Archana k. on 21/03/13 for Bug-8634 end
        }
        private void btnLocNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master order by Loc_Desc";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                MessageBox.Show("Please create Location in Location Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Please create Leave Year in Leave Year Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                return;
            }

            VForText = "Select Location Name";
            vSearchCol = "Loc_Code";  //LocNm,
            vDisplayColumnList = "LocNm:Location Name,Loc_Code:Location Code";
            vReturnCol = "LocNm,Loc_Code";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtLocNm.Text = oSelectPop.pReturnArray[0];
                dsMain.Tables[0].Rows[0]["Loc_Code"] = oSelectPop.pReturnArray[1];
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
            string SqlStr = "select top 1  " + vMainField + " as Col1 from "+vMainTblNm+" order by  " + vMainField + " desc";
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



            //if (dsMain.Tables[0].Rows.Count == 0)
            //{
            //    this.btnEmail.Enabled = false;
            //   // this.btnLocate.Enabled = true;
            //}
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;

            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            this.txtShiftCode.Focus();
            cValid = true;//Added by Archana K. on 22/03/13 for Bug-8634
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
            lblmand1.Focus();
            // cValid = true; //Commented by Archana K. on 21/03/13 for Bug-8634 
            //added by Archana K. on 06/05/13 for Bug-13508 start
            CancelEventArgs args = new CancelEventArgs();
            this.txtMinHrs_Validating(sender, args);
            CancelEventArgs args1 = new CancelEventArgs();
            this.txthalfDayHrs_Validating(sender, args1);
            //added by Archana K. on 06/05/13 for Bug-13508 end

            this.mthChkValidation();
            if (cValid == false)
            {
                cValid = true;//Added by Archana K. on 21/03/13 for Bug-8634 
                return;
            }

            //Added by Archana K. on 22/04/13 for Bug-5837 start
            MthChkShiftTMValid();
            if (cValid == false)
            {
                cValid = true;
                return;
            }
            MthChkLunchTMValid();
            if (cValid == false)
            {
                cValid = true;
                return;
            }
            MthChkTeabreak1TMValid();
            if (cValid == false)
            {
                cValid = true;
                return;
            }
            MthChkTeabreak2TMValid();
            if (cValid == false)
            {
                cValid = true;
                return;
            }
            //Added by Archana K. on 22/04/13 for Bug-5837 end 

           // this.txtShiftCode.Focus();
           // this.txtShiftNm.Focus();
            DateTime tms,tmt;
            //tms = Convert.ToDateTime(dtpSFrom.Text);
            //tmt = Convert.ToDateTime(dtpSTo.Text);
            //tmt = Convert.ToDateTime(dtpSTo.Text);
            //if (tms > tmt)
            //{
            //    return;
            //}
            
           

            this.Refresh();
            this.mthSave();
            this.mthChkNavigationButton();

            this.txtLocNm.Text = "";  /*Ramya*/
            timer1.Enabled = true;
            timer1.Interval = 1000;
            MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
           

        }
        private void mthSave()
        {


            string vTime=string.Empty;
            string vSaveString = string.Empty;
            string vDuretion;
           
            if (this.chkDefaShift.Checked) { dsMain.Tables[0].Rows[0]["Default_Shift"] = true; } else { dsMain.Tables[0].Rows[0]["Default_Shift"] = false; }

            vDuretion = "";
            
            this.mthConverTime(dtpSFrom,ref  vDuretion);
            dsMain.Tables[0].Rows[0]["ShiftFrom"] = vDuretion;
            this.mthConverTime(dtpSTo, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["ShiftTo"] = vDuretion;

            this.mthConverTime(this.dtpLFrom, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["LunchFrom"] = vDuretion;
            this.mthConverTime(dtpLTo, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["LunchTo"] = vDuretion;

            //dsMain.Tables[0].Rows[0]["ShiftDuration"] = this.txtSfDuration.Text ;
            //dsMain.Tables[0].Rows[0]["LunchDuration"] = this.txtMinHrs.Text;


            //if (this.chkDefaShift.Checked) { dsMain.Tables[0].Rows[0]["Default_Shift"] = true; } else { dsMain.Tables[0].Rows[0]["Default_Shift"] = false; }

            //Added by Archana K.on 05/03/13 for Bug-8634 start
            this.mthConverTime(this.dtpT1From, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["TeaFrom1"] = vDuretion;
            this.mthConverTime(this.dtpT1To, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["TeaTo1"] = vDuretion;
            this.mthConverTime(this.dtpT2From, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["TeaFrom2"] = vDuretion;
            this.mthConverTime(this.dtpT2To, ref  vDuretion);
            dsMain.Tables[0].Rows[0]["TeaTo2"] = vDuretion;
            //Added by Archana K.on 05/03/13 for Bug-8634 end

            dsMain.Tables[0].Rows[0].AcceptChanges();
            dsMain.Tables[0].Rows[0].EndEdit();

            this.mSaveCommandString(ref vSaveString, "#ID#");

            
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);

            //this.mthChkNavigationButton();

            //vMainFldVal = dsMain.Tables[0].Rows[0]["Head_Nm"].ToString().Trim() + dsMain.Tables[0].Rows[0]["EDType"].ToString().Trim();
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            SqlStr = "Select top 1 * from "+vMainTblNm +" Where " + vMainField + "='" + vMainFldVal + "'";
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

                vSaveString = " insert into "+vMainTblNm;
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
                vSaveString = " Update " + vMainTblNm + " Set ";
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
            DataSet tDs = new DataSet();
            //int vHours1 = 0,vHour2=0; //Commented by Archana K. on 16/03/13 for Bug-8634
            if (string.IsNullOrEmpty(this.txtShiftCode.Text.Trim()))
            {
                MessageBox.Show("Shift Code Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtShiftCode.Focus();
                cValid = false;
                return;

            }
            //Commented by Archana K. on 16/03/13 for Bug-8634 start
            //if (Convert.ToDateTime(dtpSTo.Text) < Convert.ToDateTime(dtpSFrom.Text))
            //{
            //    if (Convert.ToDateTime(dtpSFrom.Text).Hour > 12 && Convert.ToDateTime(dtpSTo.Text).Hour < 12)
            //    {
            //        vHour2 = (24 * 60);
            //        vHour2 = vHour2 - (dtpSFrom.Value.Hour * 60) - dtpSFrom.Value.Minute;
            //        vHour2 = vHour2 + (dtpSTo.Value.Hour * 60) + dtpSTo.Value.Minute;
            //        //vHour2 = ((24 * 60) - Convert.ToDateTime(dtpSFrom.Text).Minute) - dtpSFrom.Value ;

            //    }
            //}
            //else
            //{
            //    vHour2 = (dtpSTo.Value.Hour * 60) + dtpSTo.Value.Minute;
            //    vHour2 = vHour2 - (dtpSFrom.Value.Hour * 60) - dtpSFrom.Value.Minute;
            //}

            //vHours1 = (Convert.ToInt16(this.txtMinHrs.Text));
            //if (vHour2 < vHours1)
            //{
            //    MessageBox.Show("Minimum Hours Could not grater than Shift hours ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    this.txtMinHrs.Focus();
            //    cValid = false;
            //    return;

            //}
            //Commented by Archana K. on 16/03/13 for Bug-8634 end
            //Added by Archana K. on 05/03/13 for Bug-8634 start

            if (this.dtpSTo.Text.ToString() == "00:00" && this.dtpSFrom.Text.ToString() == "00:00")
            {
                MessageBox.Show("Shift to time should not be Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpSTo.Focus();
                cValid = false;
                return;
            }

            //Added by Archana K. on 05/03/13 for Bug-8634 end
            if (string.IsNullOrEmpty(this.txtShiftNm.Text.Trim()))
            {
                MessageBox.Show("Shift Name Could not Blank", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtShiftCode.Focus();
                cValid = false;
                return;

            }
            //Commented by Archana K. on 16/03/13 for Bug-8634 start
            //if ( Convert.ToDateTime(dtpSFrom.Text)> Convert.ToDateTime(dtpSTo.Text))
            //{
            //    if (Convert.ToDateTime(dtpSFrom.Text).Hour > 12 && Convert.ToDateTime(dtpSTo.Text).Hour < 12)
            //    {
            //        vHour2 = (24 * 60);
            //        vHour2 = vHour2 - (dtpSFrom.Value.Hour * 60) - dtpSFrom.Value.Minute;
            //        vHour2 = vHour2 + (dtpSTo.Value.Hour * 60) + dtpSTo.Value.Minute;
            //        //vHour2 = ((24 * 60) - Convert.ToDateTime(dtpSFrom.Text).Minute) - dtpSFrom.Value ;
            //        if (vHour2 < 0)
            //        {
            //            MessageBox.Show("Shift To Value could not less than Shift From Value", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            this.dtpSTo.Focus();
            //            cValid = false;
            //            return;
            //        }

            //    }

                
            //}
            //if ( Convert.ToDateTime(dtpLFrom.Text) >  Convert.ToDateTime(dtpLTo.Text))
            //{
            //    MessageBox.Show("Lunch To Value could not less than Lunch From Value", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    this.dtpSTo.Focus();
            //    cValid = false;
            //    return;
            //}
            //Commented by Archana K. on 16/03/13 for Bug-8634 end

            SqlStr = "Select Shift_Name From Emp_Shift_Master where Shift_Code='" + this.txtShiftCode.Text + "'";
            if (this.pEditMode)
            {
                SqlStr =SqlStr+ " and id<>" + dsMain.Tables[0].Rows[0]["Id"].ToString() ;
            }
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (tDs.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Shift Code already used for " + tDs.Tables[0].Rows[0]["Shift_Name"].ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtShiftCode.Focus();
                cValid = false;
                return;
            }
            
            //SqlStr = "Select Shift_Code From Emp_Shift_Master where Shift_Code='" + this.txtShiftCode.Text + "'";
               string LocCode=string.Empty;
             if(this.txtLocNm.Text.Trim()!="")
             {
             SqlStr="Select Loc_Code from Loc_Master where Loc_Desc='"+this.txtLocNm.Text.Trim()+"'";
             DataSet ds=oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
             LocCode=ds.Tables[0].Rows[0]["Loc_Code"].ToString();
             }
         

           // SqlStr = "Select Shift_Code From Emp_Shift_Master where Shift_Code='" + this.txtShiftCode.Text + "'";
             SqlStr = "select * from Emp_Shift_Master where Loc_Code+Shift_Name = '" + LocCode + this.txtShiftNm.Text.Trim() + "'";
            if (this.pEditMode)
            {
                SqlStr = SqlStr + " and id<>" + dsMain.Tables[0].Rows[0]["Id"].ToString();
            }
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (tDs.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show(this.txtShiftNm.Text.Trim()+" already created for "+this.txtLocNm.Text.Trim() , this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtShiftCode.Focus();
                cValid = false;
                return;
            }


        }
        private void mthConverTime(DateTimePicker dtp,ref string vDuretion)
        {

            TimeSpan tsp = new TimeSpan(dtp.Value.Ticks - dtp.Value.Date.Ticks);

            vDuretion = tsp.ToString();
            vDuretion = vDuretion.Substring(0, 8);
            vDuretion = vDuretion.Substring(0, 5);
            if (dtp.Value.ToString().IndexOf("AM") > -1)
            {
                vDuretion = vDuretion + " " + "AM";
            }
            else
            {
                vDuretion = vDuretion + " " + "PM";
            }
           

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthEdit();
            cValid = true;//Added by Archana K. on 22/03/13 for Bug-8634
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            vCancel = true;//Added by Archana K. on 22/03/13 for Bug-13508
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
            vCancel = false;//Added by Archana K. on 22/03/13 for Bug-13508
        }
        private void mthCancel(object sender, EventArgs e)
        {
            this.pAddMode = true;
            this.pEditMode = false;
            dsMain.Tables[0].Rows[0].CancelEdit();
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
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthView();
                }
            }

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthDelete();
            //Added by Archana K. on 21/03/13 for Bug-8634 start
            if (cValid == false)
            {
                cValid = true; 
                return;
            }
            //Added by Archana K. on 21/03/13 for Bug-8634 end
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

                if (this.btnForward.Enabled)
                {
                    SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);  /*Ramya*/
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "'";
                        dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);/*Ramya*/
                    }
                    //else
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
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {             
                vEnabled = true;
                this.btnShiftCode.Enabled = false;
                this.btnShiftNm.Enabled = false;
            }
            else
            {
                this.btnShiftCode.Enabled = true;
                this.btnShiftNm.Enabled = true;
            }

            if (this.pAddMode)
            {
                this.txtShiftCode.Enabled = true;
            }
            else
            {
                this.txtShiftCode.Enabled = false;
            }
            this.txtLocNm.Enabled = vEnabled;
            this.btnLocNm.Enabled = vEnabled;
            this.btnSftIncharge.Enabled = vEnabled;
            //this.txtShiftCode.Enabled = vEnabled;
            this.txtShiftNm.Enabled = vEnabled;
            this.txtSftIncharge.Enabled = vEnabled;

            this.dtpSFrom.Enabled = vEnabled;
            this.dtpSTo.Enabled = vEnabled;
            this.dtpLFrom.Enabled = vEnabled;
            this.dtpLTo.Enabled = vEnabled;
            this.txtMinHrs.Enabled = vEnabled;
            this.txthalfDayHrs.Enabled = vEnabled;
            this.txtRemark.Enabled = vEnabled;

            this.chkDefaShift.Enabled = vEnabled;
            //Added by Archana K. on 05/03/13 for Bug-8634 start
            this.dtpT1From.Enabled = vEnabled;
            this.dtpT1To.Enabled = vEnabled;
            this.dtpT2From.Enabled = vEnabled;
            this.dtpT2To.Enabled = vEnabled;
            //Added by Archana K. on 05/03/13 for Bug-8634 end

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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);    
        }

        private void btnShiftCode_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            //SqlStr = "Select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master order by Loc_Desc";
            SqlStr = " Select Shift_Code,Shift_Name from " + vMainTblNm;
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                    MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Please create Leave Year in Leave Year Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                return;
            }


            VForText = "Select Shift";
            vSearchCol = "Shift_Code"; //,Shift_Name
            vDisplayColumnList = "Shift_Code:Shift Code,Shift_Name:Shift Name";
            vReturnCol = "Shift_Code,Shift_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtShiftCode.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtShiftCode.Text.Trim();

                SqlStr = "Select top 1 * from " + vMainTblNm + " Where Shift_Code='" + vMainFldVal + "' order by " + vMainField;
                //this.txtLocNm.Text = oSelectPop.pReturnArray[0];
               // dsMain.Tables[0].Rows[0]["Loc_Code"] = oSelectPop.pReturnArray[1];
                dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnShiftNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            //SqlStr = "Select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master order by Loc_Desc";
            SqlStr = " Select Shift_Code,Shift_Name,Loc_Master.Loc_Desc from " + vMainTblNm + " Left Join Loc_Master on " + vMainTblNm + ".Loc_Code=Loc_Master.Loc_Code";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Please create Leave Year in Leave Year Master", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                return;
            }

            VForText = "Select Shift";
            vSearchCol = "Shift_Name"; //Shift_Code,,Loc_Desc
            vDisplayColumnList = "Shift_Code:Shift Code,Shift_Name:Shift Name,Loc_Desc:Location";
            vReturnCol = "Shift_Code,Shift_Name,Loc_Desc";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtShiftCode.Text = oSelectPop.pReturnArray[0];
                vMainFldVal = this.txtShiftCode.Text.Trim();
                this.txtShiftNm.Text = oSelectPop.pReturnArray[1];
                //this.txtLocNm.Text = oSelectPop.pReturnArray[0];
                // dsMain.Tables[0].Rows[0]["Loc_Code"] = oSelectPop.pReturnArray[1];
                  SqlStr = "Select top 1 * from " + vMainTblNm + " Where Shift_Code='" + vMainFldVal + "' order by " + vMainField;
                //this.txtLocNm.Text = oSelectPop.pReturnArray[0];
               // dsMain.Tables[0].Rows[0]["Loc_Code"] = oSelectPop.pReturnArray[1];
                  dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            this.mthView();

            this.mthChkNavigationButton();
            
        }

        private void btnSftIncharge_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select EmployeeName as EmpNM,EmployeeCode as EmpCode from EmployeeMast where Designation like '%Manager%' order by EmployeeName";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                 MessageBox.Show("Please Create Employee with Designation as Manager", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Manager Name";
            vSearchCol = "EmpCode"; //EmpNM,
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
                this.txtSftIncharge.Text = oSelectPop.pReturnArray[0].Trim();
                dsMain.Tables[0].Rows[0]["Incharge_Code"] = oSelectPop.pReturnArray[1].Trim();
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
                    if (this.pAddMode == false && this.pEditMode == false)
                    {
                        this.btnSave.Enabled = false;
                        this.btnCancel.Enabled = false;
                        vBtnAdd = true;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                        //if (this.pAddButton)
                        //{
                        //    this.btnNew.Enabled = true;
                        //}
                    }

                    return;
                }
            }//Added by Archana K. on 17/05/13 for Bug-7899
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

            if (dsMain.Tables[0].Rows.Count == 0)
            {
               // return;
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
                //this.btnEmail.Enabled = true;
                //this.btnLocate.Enabled = true;
            }
        }
        private void mthView()
        {
            this.mthBindClear();
            this.txtLocNm.Text = "";
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if ((Boolean)dsMain.Tables[0].Rows[0]["Default_Shift"] == false) { this.chkDefaShift.Checked = false; } else { this.chkDefaShift.Checked = true; }
                    if (dsMain.Tables[0].Rows[0]["Loc_Code"] != DBNull.Value)
                    {
                        SqlStr = "Select Loc_Desc from Loc_Master where Loc_Code='" + dsMain.Tables[0].Rows[0]["Loc_Code"].ToString() + "'";
                        DataSet tdsLoc = new DataSet();
                        tdsLoc = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                        if (tdsLoc.Tables[0].Rows.Count > 0)
                        {
                            this.txtLocNm.Text = (string)tdsLoc.Tables[0].Rows[0]["Loc_Desc"];
                        }
                    }
                    try//Added by Archana K.on 05/03/13 for Bug-8634
                    {
                        if (dsMain.Tables[0].Rows[0]["ShiftFrom"] != DBNull.Value) 
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["ShiftFrom"].ToString();
                                if (vDuretion != "") //Added by Archana K.on 05/03/13 for Bug-8634
                                    dtpSFrom.Value =Convert.ToDateTime(vDuretion);
                            //Added by Archana K.on 05/03/13 for Bug-8634 start
                                else
                                    dtpSFrom.Value = Convert.ToDateTime("00:00");
                            //Added by Archana K.on 05/03/13 for Bug-8634 end
                        }
                        if (dsMain.Tables[0].Rows[0]["ShiftTo"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["ShiftTo"].ToString();
                             if (vDuretion != "") //Added by Archana K.on 05/03/13 for Bug-8634
                                dtpSTo.Value = Convert.ToDateTime(vDuretion);
                             //Added by Archana K.on 05/03/13 for Bug-8634 start
                             else
                                dtpSTo.Value = Convert.ToDateTime("00:00");
                            //Added by Archana K.on 05/03/13 for Bug-8634 end
                        }
                        if (dsMain.Tables[0].Rows[0]["LunchFrom"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["LunchFrom"].ToString();
                                if (vDuretion != "") //Added by Archana K.on 05/03/13 for Bug-8634
                                    dtpLFrom.Value = Convert.ToDateTime(vDuretion);
                            //Added by Archana K.on 05/03/13 for Bug-8634 start
                                else
                                    dtpLFrom.Value = Convert.ToDateTime("00:00");
                             //Added by Archana K.on 05/03/13 for Bug-8634 end
                        }
                        if (dsMain.Tables[0].Rows[0]["LunchTo"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["LunchTo"].ToString();
                                 if (vDuretion != "") //Added by Archana K.on 05/03/13 for Bug-8634
                                    dtpLTo.Value = Convert.ToDateTime(vDuretion);
                            //Added by Archana K.on 05/03/13 for Bug-8634 start
                                else
                                    dtpLTo.Value = Convert.ToDateTime("00:00");
                            //Added by Archana K.on 05/03/13 for Bug-8634 end
                        }
                        if (dsMain.Tables[0].Rows[0]["Incharge_Code"] != DBNull.Value)
                        {
                            string vmEmpCode = string.Empty;
                            vmEmpCode = (string)dsMain.Tables[0].Rows[0]["Incharge_Code"];
                            if (string.IsNullOrEmpty(vmEmpCode) == false)
                            {
                                SqlStr = "Select EmployeeName as EmpNM from EmployeeMast where EmployeeCode='" + vmEmpCode + "'";
                                DataSet tdsemp = new DataSet();
                                tdsemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                                this.txtSftIncharge.Text = (string)tdsemp.Tables[0].Rows[0]["EmpNm"];
                            }

                        }
                        // Added by Archana K.on 05/03/13 for Bug-8634 start
                        if (dsMain.Tables[0].Rows[0]["TeaFrom1"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["TeaFrom1"].ToString();
                            if (vDuretion != "")
                                dtpT1From.Value = Convert.ToDateTime(vDuretion);
                            else
                                dtpT1From.Value = Convert.ToDateTime("00:00");
                        }

                        if (dsMain.Tables[0].Rows[0]["TeaTo1"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["TeaTo1"].ToString();
                            if (vDuretion != "")
                                dtpT1To.Value = Convert.ToDateTime(vDuretion);
                            else
                                dtpT1To.Value = Convert.ToDateTime("00:00");
                        }
                        if (dsMain.Tables[0].Rows[0]["TeaFrom2"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["TeaFrom2"].ToString();
                            if (vDuretion != "")
                                dtpT2From.Value = Convert.ToDateTime(vDuretion);
                            else
                                dtpT2From.Value = Convert.ToDateTime("00:00");
                        }
                        if (dsMain.Tables[0].Rows[0]["TeaTo2"] != DBNull.Value)
                        {
                            string vDuretion = dsMain.Tables[0].Rows[0]["TeaTo2"].ToString();
                            if (vDuretion != "")
                                dtpT2To.Value = Convert.ToDateTime(vDuretion);
                            else
                                dtpT2To.Value = Convert.ToDateTime("00:00");
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                    }

                    // Added by Archana K.on 05/03/13 for Bug-8634 end
                    
                }
            }
            
            this.mthBindData();

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
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpShiftMaster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
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

        private void txtSfDuration_TextChanged(object sender, EventArgs e)
        {
            //String Str1 = string.Empty;
            //this.mthConverTime(this.dtpSFrom, ref Str1);
            //int t1 = Convert.ToInt16(Str1.Substring(0, 2));
            //int t = Convert.ToInt16(this.txtSfDuration.Text) + t1;

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
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

        private void txtSftIncharge_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode)
            {
                if (e.KeyCode == Keys.F2)
                {
                 btnSftIncharge_Click(sender, new EventArgs());
                }
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;

        }

        private void frmShiftMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();

        }

        public void mcheckShftTimeDuration(DateTime Fdate, DateTime Tdate, string str)
        {
            //Added by Archana k.on 21/03/13 for Bug-8634

            if (((vShiftFrom >= 12 && vShiftTo <= 24) || (vShiftFrom <= 12 && vShiftTo <= 12) || (vShiftFrom <= 12 && vShiftTo >= 12)) && (vShiftTo >= 12 && vShiftTo < 24))
            {
                if ((Fdate >= shiftfrom && Fdate <= shiftto) && (Tdate >= shiftfrom && Tdate <= shiftto))
                {
                    sValid = 0;
                }
                else
                {
                    vHours1 = (Fdate.Hour) * 60 + (Fdate.Minute);
                    vHour2 = (Tdate.Hour) * 60 + (Tdate.Minute);
                    if (vHours1 > 0 || vHour2 > 0)
                    {
                        MessageBox.Show(str + " time should be between your shift time.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        //vCancel = false;
                        return;
                    }
                    vHours1 = 0;
                    vHour2 = 0;
                }


            }
            else if (vShiftFrom >= 12 && vShiftTo <= 12)
            {
                Todate = Tdate.AddDays(1);
                Fromdate = Fdate.AddDays(1);
                if ((Fromdate >= shiftfrom && Fromdate <= shiftto) && (Todate >= shiftfrom && Todate <= shiftto))
                {
                    sValid = 1;
                }
                else
                {
                    vHours1 = (Fromdate.Hour) * 60 + (Fromdate.Minute);
                    vHour2 = (Todate.Hour) * 60 + (Todate.Minute);
                    if (vHours1 > 0 || vHour2 > 0)
                    {
                        MessageBox.Show(str + " time should be between your shift time.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        //vCancel = false;
                        return;
                    }
                    vHours1 = 0;
                    vHour2 = 0;
                }
            }

        }

        public void MthChkShiftTMValid()
        {
            //Added by Archana K. on 15/03/13 for Bug-8634
            shiftfrom = Convert.ToDateTime(this.dtpSFrom.Text);
            shiftto = Convert.ToDateTime(this.dtpSTo.Text);
            if (shiftfrom.Hour >= 12 && shiftto.Hour <= 12)
            {
                if (shiftfrom.Hour <= 12)
                {
                    shiftfrom = shiftfrom.AddDays(1);
                }
                else if (shiftto.Hour <= 12)
                {
                    shiftto = shiftto.AddDays(1);
                }
                else
                {
                    shiftfrom = Convert.ToDateTime(this.dtpSFrom.Text);
                    shiftto = Convert.ToDateTime(this.dtpSTo.Text);
                }

            }
            else
            {
                shiftfrom = Convert.ToDateTime(this.dtpSFrom.Text);
                shiftto = Convert.ToDateTime(this.dtpSTo.Text);
            }

            if (shiftfrom.Hour == 0)
                vShiftFrom = 24;
            else
                vShiftFrom = shiftfrom.Hour;
            vShiftTo = shiftto.Hour;
            if (vShiftFrom >= 12 && vShiftTo <= 12)
            {
                shiftfrom = shiftfrom.AddDays(1);
                shiftto = shiftto.AddDays(1);
            }
            if (shiftfrom > shiftto)
            {
                MessageBox.Show("Shift to Value should not be less than Shift from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                this.dtpSTo.Focus();
                return;
            }
            else
            {
                if (shiftfrom.Hour > 23 && shiftfrom < shiftto)
                {
                    MessageBox.Show("Shift from Value should not be less than Shift to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpSFrom.Focus();
                    return;
                }
            }
        }
        public void MthChkLunchTMValid()
        {
            //Added by Archana K. on 15/03/13 for Bug-8634    
            mthChkFromToDate(Convert.ToDateTime(dtpLFrom.Text), Convert.ToDateTime(dtpLTo.Text));
            mcheckShftTimeDuration(Fromdate, Todate, "Lunch");
            if (sValid == 0)
            {
                if (cValid == false)
                {
                    return;
                }
                if (Fromdate > Todate)
                {
                    MessageBox.Show("Lunch to Value should not be less than lunch from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpLTo.Focus();
                    return;
                }
                else
                {
                    if (Fromdate.Hour > 23 && Fromdate < Todate)
                    {
                        MessageBox.Show("Lunch from Value should not be less than lunch to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        this.dtpLFrom.Focus();
                        return;
                    }
                }
            }
            else
            {
                if (cValid == false)
                {
                    return;
                }
                if (Fromdate > Todate)
                {
                    MessageBox.Show("Lunch to Value should not be less than lunch from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpLTo.Focus();
                    return;
                }
                else
                {
                    if (Fromdate.Hour > 23 && Fromdate < Todate)
                    {
                        MessageBox.Show("Lunch from Value should not be less than lunch to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        this.dtpLFrom.Focus();
                        return;

                    }

                }
                sValid = 0;
            }
        }
        public void MthChkTeabreak1TMValid()
        {
            //Added by Archana K. on 15/03/13 for Bug-8634
            mthChkFromToDate(Convert.ToDateTime(dtpT1From.Text), Convert.ToDateTime(dtpT1To.Text));
            mcheckShftTimeDuration(Fromdate, Todate, "Tea break1");
            if (sValid == 0)
            {
                if (cValid == false)
                {
                    return;
                }
                if (Fromdate > Todate)
                {
                    MessageBox.Show("Tea break1 to Value should not be less than Tea break1 from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpT1To.Focus();
                    return;
                }
                else
                {
                    if (Fromdate.Hour > 23 && Fromdate < Todate)
                    {
                        MessageBox.Show("Tea break1 from Value should not be less than Tea break1 to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        this.dtpT1From.Focus();
                        return;
                    }
                }
            }
            else
            {
                if (cValid == false)
                {
                    return;
                }
                if (Fromdate > Todate)
                {
                    MessageBox.Show("Tea break1 to Value should not be less than Tea break1 from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpT1To.Focus();
                    return;
                }
                else
                {
                    if (Fromdate.Hour > 23 && Fromdate < Todate)
                    {
                        MessageBox.Show("Tea break1 from Value should not be less than Tea break1 to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        this.dtpT1From.Focus();
                        return;

                    }

                }
                sValid = 0;
            }
        }
        public void MthChkTeabreak2TMValid()
        {
            //Added by Archana K. on 15/03/13 for Bug-8634
            mthChkFromToDate(Convert.ToDateTime(dtpT2From.Text), Convert.ToDateTime(dtpT2To.Text));
            mcheckShftTimeDuration(Fromdate, Todate, "Tea break2");
            if (sValid == 0)
            {
                if (cValid == false)
                {
                    return;
                }
                if (Fromdate > Todate)
                {
                    MessageBox.Show("Tea break2 to Value should not be less than Tea break2 from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpT2To.Focus();
                    return;
                }
                else
                {
                    if (Fromdate.Hour > 23 && Fromdate < Todate)
                    {
                        MessageBox.Show("Tea break2 from Value should not be less than Tea break2 to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        this.dtpT2From.Focus();
                        return;
                    }
                }
            }
            else
            {
                if (cValid == false)
                {
                    return;
                }
                if (Fromdate > Todate)
                {
                    MessageBox.Show("Tea break2 to Value should not be less than Tea break2 from Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    this.dtpT2To.Focus();
                    return;
                }
                else
                {
                    if (Fromdate.Hour > 23 && Fromdate < Todate)
                    {
                        MessageBox.Show("Tea break2 from Value should not be less than Tea break2 to Value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cValid = false;
                        this.dtpT2From.Focus();
                        return;

                    }

                }
                sValid = 0;
            }
        }

        public void mthChkFromToDate(DateTime fdate, DateTime tdate)
        {
            //Added by Archana K. on 13/04/13 for Bug-8634 
            Fromdate = fdate;
            Todate = tdate;
            if (shiftfrom.Hour >= 12 && shiftto.Hour <= 12)
            {
                if (Fromdate.Hour <= 12)
                {
                    Fromdate = Fromdate.AddDays(1);
                }
                else
                {
                    Fromdate = Convert.ToDateTime(fdate);
                }
                if (Todate.Hour <= 12)
                {
                    Todate = Todate.AddDays(1);
                }
                else
                {
                    Todate = Convert.ToDateTime(tdate);
                }

            }
            else
            {
                Fromdate = fdate;
                Todate = tdate;
            }
        }

       private void txtMinHrs_Validating(object sender, CancelEventArgs e)
        {
            //Added by Archana K. on 02/05/13 for Bug-13508
            if (vCancel == true)
                return;

            if (Convert.ToDateTime(dtpSTo.Text) <= Convert.ToDateTime(dtpSFrom.Text))
            {
                vHour2 = (24);
                vHour2 = vHour2 - (dtpSFrom.Value.Hour) - Convert.ToDecimal((Convert.ToDouble(dtpSFrom.Value.Minute) / 60.0) * 0.60);
                vHour2 = vHour2 + (dtpSTo.Value.Hour) + Convert.ToDecimal((Convert.ToDouble(dtpSTo.Value.Minute) / 60.0) * 0.60);
            }
            else
            {
                vHour2 = vHour2 - (dtpSFrom.Value.Hour) - Convert.ToDecimal((Convert.ToDouble(dtpSFrom.Value.Minute) / 60.0) * 0.60);
                vHour2 = vHour2 + (dtpSTo.Value.Hour) + Convert.ToDecimal((Convert.ToDouble(dtpSTo.Value.Minute) / 60.0) * 0.60);
            }
            vHours1 = (Convert.ToDecimal(this.txtMinHrs.Text));
            if (vHour2 < vHours1)
            {
                if (vHours1 > 0)
                {
                    MessageBox.Show("Minimum Hours should not be greater than Shift hours.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.txtMinHrs.Focus();
                    e.Cancel = true;
                    cValid = false;
                    vCancel = false;
                }

            }
            //else
            //{

            //    if (vHour2 != vHours1)
            //    {
            //        if (vHours1 > 0)
            //        {
            //            MessageBox.Show("Minimum Hours should not be less than Shift hours.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            e.Cancel = true;
            //            cValid = false;
            //            vCancel = false;
            //        }
            //    }
            //}


            vHours1 = 0;
            vHour2 = 0;
        }

        private void txthalfDayHrs_Validating(object sender, CancelEventArgs e)
        {
            //Added by Archana K. on 02/05/13 for Bug-13508
            if (vCancel == true)
                return;

            vHours1 = (Convert.ToDecimal(this.txtMinHrs.Text));
            vHour3 = (Convert.ToDecimal(this.txthalfDayHrs.Text));
            if (vHour3 > 12 && vHour3 > 0)
            {

                MessageBox.Show("Enter half day hours should be within 0 to 12.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txthalfDayHrs.Focus();
                e.Cancel = true;
                cValid = false;
                vCancel = false;
            }
            else
            {
                if (vHour3 > vHours1 && vHour3 > 0)
                {
                    MessageBox.Show("Half day hours should not be greater than minimum hours.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.txthalfDayHrs.Focus();
                    e.Cancel = true;
                    cValid = false;
                    vCancel = false;
                }

            }
            vHours1 = 0;
            vHour2 = 0;
        }

      
       
        
    }
}
