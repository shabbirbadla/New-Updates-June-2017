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
using uCheckBox;
using udclsDGVNumericColumn;
using uNumericTextBox;
using udDataTableQuery;
using udReportList;
using udTextEdit;
using udEmpColValid; //Rup 14/06/2013
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpMonthlyPayroll
{
    public partial class frmMonthlyPayroll : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataSet dsMain = new DataSet(), dsPayDet = new DataSet();
        string vMainTblNm = "MpMain", vMainField = "Inv_No", vOrdFld = "Inv_No", vMainFldVal = "";
        string vInv_No = "", vDocNo = "", vLoc_Code = "";
        
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsEDMaster = new DataSet();
        DataView dvwFilt = new DataView();
        DataSet dsSummary,dsAcDet;
        DataSet dsGenInv, dsGenMiss, dsGenDocNo;
        DataRow drLcode;
        Boolean cValid = true,cRecord=false;
        string vL_Yn = string.Empty, vGen_Inv,vDefa_Db,vDefa_Cr;
        int vgridcount;
        int iInv_No = 0, iDoc_No = 0, vTran_Cd = 0, vInvNoLen = 0;
        string vYear = string.Empty, vMonth = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;
        string appPath;
        string SqlStr = string.Empty;
        String cAppPId, cAppName;
        string vCurCol = string.Empty;
        String FinYear = string.Empty;
        short vTimeOut = 50;  /*Ramya 21/01/13*/
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end
        DataTable temtbl;//Added by Archana K. on 20/09/13 for Bug-18246 
        public frmMonthlyPayroll(string[] args)
        {
            this.pDisableCloseBtn = true;  /* close disable  */  /*Ramya Bug-8354*/
            InitializeComponent();
            this.pPara = args;
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
            this.pFrmCaption = "Monthly Payroll Generation";
        }
        private void frmMonthlyPayroll_Load(object sender, EventArgs e)
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

            this.btnEmail.Enabled = false;
            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnExportPdf.Enabled = false;  

            this.btnLocate.Enabled = false;


            this.txtEarnings.pAllowNegative = false;
            this.txtEarnings.MaxLength = 16;
            this.txtEarnings.pDecimalLength = 3;

            this.txtBonus.pAllowNegative = false;
            this.txtBonus.MaxLength = 16;
            this.txtBonus.pDecimalLength = 3;

            this.txtStatDed.pAllowNegative = false;
            this.txtStatDed.MaxLength = 16;
            this.txtStatDed.pDecimalLength = 3;

            this.txtDeductions.pAllowNegative = false;
            this.txtDeductions.MaxLength = 16;
            this.txtDeductions.pDecimalLength = 3;

            this.txtStatContr.pAllowNegative = false;
            this.txtStatContr.MaxLength = 16;
            this.txtStatContr.pDecimalLength = 3;

            this.txtGratuity.pAllowNegative = false;
            this.txtGratuity.MaxLength = 16;
            this.txtGratuity.pDecimalLength = 3;

            this.txtOthChg.pAllowNegative = false;
            this.txtOthChg.MaxLength = 16;
            this.txtOthChg.pDecimalLength = 3;

            this.txtNetSalary.pAllowNegative = false;
            this.txtNetSalary.MaxLength = 16;
            this.txtNetSalary.pDecimalLength = 3;

            this.txtGrossSalary.pAllowNegative = false;
            this.txtGrossSalary.MaxLength = 16;
            this.txtGrossSalary.pDecimalLength = 3;

            this.txtTotal.pAllowNegative = false;
            this.txtTotal.MaxLength = 16;
            this.txtTotal.pDecimalLength = 3;

            this.StbLbl.Text = "";

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.mthDsCommon();

            int vint = 0;

            SqlStr = "Select * From LCode where Entry_Ty='MP'";
            drLcode = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
            vInvNoLen = Convert.ToInt16(drLcode["Invno_Size"]);
            vGen_Inv = drLcode["Gen_Inv"].ToString().Trim();
            vDefa_Db = drLcode["Defa_Db"].ToString().Trim().Replace(@"""", "");
            vDefa_Cr = drLcode["Defa_Cr"].ToString().Trim();
            if (string.IsNullOrEmpty(vGen_Inv))
            {
                vGen_Inv = "MP";
            }

            //SqlStr = "select isnull(max(Pay_Year),0) as LvYear from Emp_Monthly_Payroll";
            //DataSet dsTemp = new DataSet();
            //dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{

            //    vint = Convert.ToInt16(dsTemp.Tables[0].Rows[0]["LvYear"].ToString());
            //    if (vint > 0)
            //    {
            //        this.txtYear.Text = vint.ToString();
            //    }
            //}
            //SqlStr = "select isnull(max(Pay_Month),0) as LvMonth from Emp_Monthly_Payroll ";
            //dsTemp = new DataSet();
            //dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    vint = Convert.ToInt16(dsTemp.Tables[0].Rows[0]["LvMonth"].ToString());
            //    if (vint > 0)
            //    {
            //        this.txtMonth.Text = this.fnCMonth(vint);
            //    }
            //    else
            //    {
            //        this.txtMonth.Text = this.fnCMonth(DateTime.Now.Month);
            //    }
            //}

           // SqlStr = "Set DateFormat dmy Select p.HeadType,p.PayEffect,e.Short_Nm,e.Fld_Nm,PayEditable from Emp_Pay_Head_Master e inner join Emp_Pay_Head p on (e.HeadTypeCode=p.HeadTypeCode) Where (isDeActive=0 or (isDeActive=1 and DeactFrom>'" + this.dtpProcsessDate.Value + "' ) )";
            SqlStr = "Set DateFormat dmy Select p.HeadType,p.PayEffect,e.Short_Nm,e.Fld_Nm,PayEditable,e.dNetExpr from Emp_Pay_Head_Master e inner join Emp_Pay_Head p on (e.HeadTypeCode=p.HeadTypeCode) "; //Rup 14/06/2013
            SqlStr = SqlStr + " Order By p.SortOrd,e.SortOrd";
            dsEDMaster = new DataSet();
            dsEDMaster = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthdvwFilt();
            this.SetMenuRights();

            this.pDeleteButton = false;

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
               
                this.btnOthDet.Image = Image.FromFile(fName);
                this.btnYear.Image = Image.FromFile(fName);
                this.btnMonth.Image = Image.FromFile(fName);
            }
            fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnDept.Image = Image.FromFile(fName);
                this.btnCate.Image = Image.FromFile(fName);
                this.btnEmpNm.Image = Image.FromFile(fName);
                this.btnCalcPeriod.Image = Image.FromFile(fName);       // Added by Sachin N. S. on 27/06/2014 for Bug-21114
            }
            //Added by Archana K. on 16/05/13 for Bug-7899 start
            startupPath = Application.StartupPath;
           // startupPath = "E:\\Vudyog Sdk_11";
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
            //Added by Archana K. on 20/09/13 for Bug-18246 start
            SqlStr = "Select employeecode=cast('' as varchar),loanamt=cast(0 as float),advamt=cast(0 as float),Pay_month=cast(0 as int) Where 1=2";
            temtbl = new DataTable();
            temtbl = oDataAccess.GetDataTable(SqlStr, null, 30);
            //Added by Archana K. on 20/09/13 for Bug-18246 end
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
     
        private void dgvPayDet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                return;
            }
            if ((dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex].GetType().Name) == "CNumEditDataGridViewColumn")  //Rup 14/06/2013
            {

                if (string.IsNullOrEmpty( ((udclsDGVNumericColumn.CNumEditDataGridViewColumn)dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex]).pValidFunNm) == false) //Rup 14/06/2013
                {
                    udEmpColValid.cEmpColValid  udcolvalid = new udEmpColValid.cEmpColValid();
                    udcolvalid.pValidFunNm = ((udclsDGVNumericColumn.CNumEditDataGridViewColumn)dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex]).pValidFunNm.ToUpper().Trim();
                    udcolvalid.mthNumericColumnCalculate(ref this.dgvPayDet, this.dgvPayDet.CurrentRow.Index,oDataAccess);
                }

            }
            Decimal vTDSPer = 0, vProjGross = 0, vMonthPen=0,vOrgTDS=0,vGrossDiff=0,vNewTdS=0;
            /*Commented by Ramya for Bug-14226*/

            //if (vCurCol == dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex].Name.ToUpper())
            //{
            //    vCurCol = "";
            //    return;
            //}

            /*Commented by Ramya for Bug-14226*/
            vCurCol = dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex].Name.ToUpper();

            if (dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex].Name.ToUpper() == "Col_NetPayment")
            {
                return;
            }
            if (dgvPayDet.Columns[dgvPayDet.CurrentCell.ColumnIndex].Name.ToUpper() == "Col_GrossPayment")
            {
                return;
            }
            
            DataGridViewRow cr = this.dgvPayDet.CurrentRow;
            if (cr != null)
           {
                decimal  vNetPayment = 0,VGrossPayment=0;
                int MonthDays = 0;
                string ColTag = "";
                int Lop = 0;

                //vTDSPer = cr.Cells[" vProjGross = 0, vMonthPen=0,vOrgTDS=0;
                //cr.Cells["colTDSPer"]
                vTDSPer = 0; vProjGross = 0; vMonthPen = 0; vOrgTDS=0;
                foreach (DataGridViewCell cl in cr.Cells)
                {
                    if (dgvPayDet.Columns[cl.ColumnIndex].Name == "colTDSPer")
                    {
                        vTDSPer = Convert.ToDecimal(cl.Value);
                    }
                    if (dgvPayDet.Columns[cl.ColumnIndex].Name == "colProjGross")
                    {
                        vProjGross = Convert.ToDecimal(cl.Value);
                    }
                    if (dgvPayDet.Columns[cl.ColumnIndex].Name == "colMonthPen")
                    {
                        vMonthPen = Convert.ToDecimal(cl.Value);
                    }
                    if (dgvPayDet.Columns[cl.ColumnIndex].Name == "colOrgTDS")
                    {
                        vOrgTDS = Convert.ToDecimal(cl.Value);
                    }
                    if (dgvPayDet.Columns[cl.ColumnIndex].Tag != null)
                    {
                        
                        ColTag = dgvPayDet.Columns[cl.ColumnIndex].Tag.ToString();
                        if (ColTag == "+")
                        {
                            vNetPayment = vNetPayment + Convert.ToDecimal(cl.Value);
                            VGrossPayment = VGrossPayment + Convert.ToDecimal(cl.Value);
                        }
                        if (ColTag == "-")
                        {
                            vNetPayment = vNetPayment - Convert.ToDecimal(cl.Value);
                        }
                    }
                    //if (dgvPayDet.Columns[cl.ColumnIndex].Name == "colTDSPer")
                    //{ 
                    //}
                    //if (dgvMain.Columns[cl.ColumnIndex].Name.ToUpper() == "COL_MND")
                    //{
                    //    MonthDays = Convert.ToInt16(cl.Value);
                    //}
                }
                //Lop = MonthDays - SalPaidDays;
                //SalPaidDays = 30 - Lop;

               
                if (vProjGross != 0)    /*Ramya on 29/05/13 for manual entry of tds at the time of payroll generation*/
                {
                    if (VGrossPayment != vProjGross)
                    {
                        if (this.pAddMode)
                        {
                            //****** Commented by Sachin N. S. on 14/02/2014 for Bug-21542 -- Start 
                            //vGrossDiff = VGrossPayment - vProjGross;
                            //vNewTdS = vGrossDiff * vTDSPer / 100;
                            //if (vMonthPen == 0)   /*Ramya 30/10/12*/
                            //{
                            //    vNewTdS = 0;
                            //}
                            //else
                            //{
                            //    vNewTdS = vNewTdS / vMonthPen;
                            //}
                            //cr.Cells["col_TDSAmt"].Value = Convert.ToDecimal(cr.Cells["colOrgTDS"].Value) + vNewTdS;
                            //****** Commented by Sachin N. S. on 14/02/2014 for Bug-21542 -- End
                        }
                        if (this.pEditMode)
                        {
                            //****** Commented by Sachin N. S. on 14/02/2014 for Bug-21542 -- Start 
                            //vNewTdS = VGrossPayment * vOrgTDS / vProjGross;
                            ///*Avg TDS calculation currently ignored*/
                            //cr.Cells["col_TDSAmt"].Value = vNewTdS;
                            //****** Commented by Sachin N. S. on 14/02/2014 for Bug-21542 -- End
                        }

                    }
                }
               
        
                cr.Cells["colNetPayment"].Value = vNetPayment;

               // cr.Cells["colGrossPayment"].Value = vNetPayment;
                cr.Cells["colGrossPayment"].Value = VGrossPayment;  /*Ramya 17/05/13*/
                DataTable dt1 = new DataTable();
                dt1 = dsPayDet.Tables[0];

                
           }
        }
        private void mthCalSummary()
        {
            
        }
        private void mthDgvSumColumns()
        {
            dsSummary = new DataSet();

            SqlStr = "Select h.HeadType,e.HeadTypeCode,e.Short_Nm,e.Fld_Nm,Amount=cast(0 as Decimal(17,3)),e.dAc_Name,e.cAc_Name from Emp_Pay_Head_Master e";
            SqlStr = SqlStr + " inner join Emp_Pay_Head h on (e.HeadTypeCode=h.HeadTypeCode)";
            SqlStr = SqlStr + " Where (isDeActive=0 or (isDeActive=1 and DeactFrom<GetDate() ) )";
            SqlStr = SqlStr + " order by h.SortOrd,e.SortOrd,h.PayEffect";
            dsSummary = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            //this.dgvSum.Columns.Clear();
            this.dgvSum.DataSource = dsSummary.Tables[0];
            this.dgvSum.Columns.Clear();

            System.Windows.Forms.DataGridViewTextBoxColumn colHeadTye = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colHeadTye.HeaderText = "HeadType";
            colHeadTye.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colHeadTye.Name = "colHeadTye";
            colHeadTye.Width = 200;
            this.dgvSum.Columns.Add(colHeadTye);

            System.Windows.Forms.DataGridViewTextBoxColumn colShortNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colShortNm.HeaderText = "Short Name";
            colShortNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colShortNm.Name = "colShortNm";
            colShortNm.Width = 200;
            this.dgvSum.Columns.Add(colShortNm);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmt = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colAmt.HeaderText = "Amount";
            colAmt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAmt.Name = "colAmt";
            colAmt.Width = 200;
            this.dgvSum.Columns.Add(colAmt);

            dgvSum.Columns["colHeadTye"].DataPropertyName = "HeadType";
            dgvSum.Columns["colShortNm"].DataPropertyName = "Short_Nm";
            dgvSum.Columns["colAmt"].DataPropertyName = "Amount";
            


        }
        private void mthCalsum()
        {
            udDataTableQuery.cCalc oCalc = new udDataTableQuery.cCalc();
            

            Decimal vNetSal=0,vAmt=0,vEarning=0, vBonus=0, vStatDed=0, vEmployeeDed=0, vStatContr=0, vGratuity=0, vEmplOthChg = 0,vLoan=0,vAdvance=0;
            string vFldNm="",vEarningFld = "", vBonusFld = "", vStatDedFld = "", vEmployeeDedFld = "", vStatContrFld = "", vGratuityFld = "", vEmplOthChgFld = "",vLoanFld="",vAdvFld="";
            for (int i = 0; i <=dsSummary.Tables[0].Rows.Count-1; i++)
            {
                vFldNm = dsSummary.Tables[0].Rows[i]["Fld_Nm"].ToString().ToUpper().Trim();
                switch  (dsSummary.Tables[0].Rows[i]["HeadTypeCode"].ToString())
                {
                    case "E":
                        vEarningFld = vEarningFld+"[" + vFldNm+"]";
                        break;
                    case "BNS":
                        vBonusFld = vBonusFld + "[" + vFldNm + "]";
                        break;
                    case "ESD":
                        vStatDedFld = vStatDedFld + "[" + vFldNm + "]";
                        break;
                    case "D":
                        vEmployeeDedFld = vEmployeeDedFld + "[" + vFldNm + "]";
                        break;
                    case "RSC":
                        vStatContrFld = vStatContrFld + "[" + vFldNm + "]";
                        break;
                    case "GRT":
                        vGratuityFld = vGratuityFld + "[" + vFldNm + "]";
                        break;
                    case "ROC":
                        vEmplOthChgFld = vEmplOthChgFld + "[" + vFldNm + "]";
                        break;
                    case "LON":                                         /*Ramya 05/02/13*/
                        vLoanFld = vLoanFld + "[" + vFldNm + "]";
                        break;
                    case "ADV":                                          /*Ramya 05/02/13*/
                        vAdvFld = vAdvFld + "[" + vFldNm + "]";
                        break;
                        

                }
              
            }
            if (this.dsPayDet.Tables.Count == 0)
            {
                return;
            }
            //foreach (DataRow tDr in this.dsPayDet.Tables[0].Rows)   /*Commented by Ramya for Summary values are coming wrong*/
            //{
                foreach (DataColumn col in this.dsPayDet.Tables[0].Columns)
                {

                    vFldNm =col.ColumnName ;
                    vFldNm = vFldNm.ToUpper().Trim();

                    vAmt = 0;
                    if (vEarningFld.IndexOf("["+vFldNm+"]") >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vEarning = vEarning + vAmt;
                    }
                    else if (vBonusFld.IndexOf(vFldNm) >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vBonus = vBonus + +vAmt;
                    }
                    else if (vStatDedFld.IndexOf(vFldNm) >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vStatDed = vStatDed + +vAmt;
                    }
                    else if (vEmployeeDedFld.IndexOf(vFldNm) >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vEmployeeDed = vEmployeeDed + +vAmt;
                       
                    }
                    else if (vStatContrFld.IndexOf(vFldNm) >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vStatContr = vStatContr + vAmt;
                       
                    }
                    else if (vGratuityFld.IndexOf(vFldNm) >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vGratuity = vGratuity +vAmt;
                    }
                    else if (vEmplOthChgFld.IndexOf(vFldNm) >= 0)
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vEmplOthChg = vEmplOthChg + vAmt;
                    }
                    else if (vLoanFld.IndexOf(vFldNm) >= 0)  /*Ramya 05/02/13*/
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vLoan = vLoan + vAmt;
                     
                    }
                    else if (vAdvFld.IndexOf(vFldNm) >= 0)  /*Ramya 05/02/13*/
                    {
                        vAmt = oCalc.funcSum(this.dsPayDet.Tables[0], vFldNm, "");
                        vAdvance = vAdvance + vAmt;
                    }
                    
                    if (vAmt != 0)
                    {
                        foreach (DataRow tdr1 in dsSummary.Tables[0].Rows)
                        {
                            if (tdr1["Fld_Nm"].ToString().ToUpper().Trim()== vFldNm &&vAmt!=0)
                            {
                                tdr1["Amount"] = vAmt;
                                break;
                            }
                        }
                    }

                }
                
            //}

            this.txtEarnings.Text = vEarning.ToString();
            this.txtBonus.Text = vBonus.ToString();
            this.txtStatDed.Text = vStatDed.ToString();
            this.txtDeductions.Text = vEmployeeDed.ToString();        
            this.txtStatContr.Text = vStatContr.ToString();
            this.txtGratuity.Text = vGratuity.ToString();
            this.txtOthChg.Text = vEmplOthChg.ToString();
            this.txtLoan.Text = vLoan.ToString(); /*Ramya 05/02/13*/
            this.txtAdvance.Text = vAdvance.ToString();/*Ramya 05/02/13*/

            this.txtNetSalary.Text = oCalc.funcSum(this.dsPayDet.Tables[0], "NetPayment", "").ToString();
            vAmt = Convert.ToDecimal(this.txtEarnings.Text) + Convert.ToDecimal(this.txtBonus.Text) + Convert.ToDecimal(this.txtStatDed.Text) + Convert.ToDecimal(this.txtDeductions.Text) + Convert.ToDecimal(this.txtStatContr.Text) + Convert.ToDecimal(this.txtGratuity.Text) + Convert.ToDecimal(this.txtOthChg.Text) ;
            //this.txtGrossSalary.Text = vAmt.ToString(); /*Ramya 24/01/13*/
            this.txtGrossSalary.Text = oCalc.funcSum(this.dsPayDet.Tables[0], "GrossPayment", "").ToString();

        }
       
        private Decimal f1unnSum(DataTable SourceTable, string vFldNm, string filtcond)
        {
            string[] FieldNames = { vFldNm };
            
            DataRow[] orderedRows = SourceTable.Select(filtcond, string.Join(", ", FieldNames));

            Decimal vTotFld = 0;
            foreach (DataRow drfunSum in orderedRows)
            {
                vTotFld = vTotFld +Convert.ToDecimal(drfunSum[vFldNm]);
            }
            return vTotFld;
        }
        private void mthCalcAcDetAmt()
        {
            udDataTableQuery.cCalc oCalc = new udDataTableQuery.cCalc();
            string filtcond = "Amount<>0 ";
            //filtcond = "";
            dsAcDet.Tables[0].Rows.Clear();
            DataTable tblDistinctAc = new DataTable();
            string[] FieldNames = { "dAc_Name" };
            udDataTableQuery.cSelectDistinct qdAc_Name = new udDataTableQuery.cSelectDistinct();
            tblDistinctAc = qdAc_Name.SelectDistinct(dsSummary.Tables[0], filtcond, FieldNames);
            
            filtcond = "";
            foreach (DataRow tdr in tblDistinctAc.Rows)
            {
                filtcond = "dAc_Name='" + tdr["dAc_Name"].ToString().Trim() + "'";
                Decimal vAcAmt = oCalc.funcSum(dsSummary.Tables[0], "Amount", filtcond);
                if (vAcAmt != 0)
                {
                    DataRow tdr1 = dsAcDet.Tables[0].NewRow();
                    tdr1["Ac_Name"] = tdr["dAc_Name"].ToString().Trim();
                    tdr1["Amount"] = vAcAmt;
                    tdr1["Amt_Ty"] = "Dr";
                    dsAcDet.Tables[0].Rows.Add(tdr1);
                }
  
            }

            tblDistinctAc = new DataTable();
            FieldNames[0] = "cAc_Name";
            
            qdAc_Name = new udDataTableQuery.cSelectDistinct();
            filtcond = "Amount<>0 ";
            tblDistinctAc = qdAc_Name.SelectDistinct(dsSummary.Tables[0], filtcond, FieldNames);

            filtcond = "";
            foreach (DataRow tdr in tblDistinctAc.Rows)
            {
                filtcond = "cAc_Name='" + tdr["cAc_Name"].ToString().Trim() + "'";
                Decimal vAcAmt = oCalc.funcSum(dsSummary.Tables[0], "Amount", filtcond);
                if (vAcAmt != 0)
                {
                    DataRow tdr1 = dsAcDet.Tables[0].NewRow();
                    tdr1["Ac_Name"] = tdr["cAc_Name"].ToString().Trim();
                    tdr1["Amount"] = vAcAmt;
                    tdr1["Amt_Ty"] = "Cr";
                    dsAcDet.Tables[0].Rows.Add(tdr1);
                }

            }
           
        }
        private void tbEmpDet_Click(object sender, EventArgs e)
        {
            //if (tbEmpDet.SelectedTab.Name == "tbpSum" && (this.pAddMode||this.pEditMode ) )
            if (tbEmpDet.SelectedTab.Name == "tbpSum" )
            {
                this.mthDgvSumColumns();
                this.mthCalsum();
            }
            if (tbEmpDet.SelectedTab.Name == "tbpAcDet" && (this.pAddMode || this.pEditMode))
            {
                dsAcDet = new DataSet();

                SqlStr = "Select * From ArAcDet where 1=2";
                //SqlStr = SqlStr + " Where (isDeActive=0 or (isDeActive=1 and DeactFrom<GetDate() ) )";

                dsAcDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                this.mthCalcAcDetAmt();
                //this.mthDgvAcDetColumns(); ?
                
            }
        }

      

#region Action Buttons
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
            vTran_Cd = 0;
            if (dsPayDet.Tables.Count > 0)
            {
                dsPayDet.Tables[0].Rows.Clear();
            }
         
            this.mthBindData();
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = true;
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
                    //SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                    SqlStr = "Select top 1 * from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "' and " + vMainField + "='" + vMainFldVal + "'";
                    dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    this.mthView();
                }
            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.btnYear.Focus();
           // this.mthGrdRefresh();
            this.mthEnableDisableFormControls();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            cValid = true;
            this.txtYear.Focus();
            //this.mthValidation(ref vIsFrmValid);
            this.mthValidation();//Added by Archana K. on 10/09/13 for Bug-18617
            if (cValid == false)
            {
                cValid = true;//Added by Archana K. on 10/09/13 for Bug-18617
                return;//Uncommented by Archana
            }
            int cnt = 0;
            //if (dsPayDet.Tables[0].Rows.Count > 0)//Commented by Archana K. on 11/09/13 for Bug-18617
            if (this.dsPayDet.Tables.Count > 0)//Changed by Archana K. on 11/09/13 for Bug-18617
            {
                if (dsPayDet.Tables[0].Rows.Count > 0)//Added by Archana K. on 11/09/13 for Bug-18617
                {
                    foreach (DataRow dr in dsPayDet.Tables[0].Rows)
                    {
                        if ((Boolean)dr["PayGenerated"] == true)
                        {
                            cnt = cnt + 1;
                        }
                    }
                    //Added by Archana K. on 21/09/13 for Bug-18246 start
                    //if (pEditButton)    
                    if (this.pEditMode)
                    {
                        for (int i = 0; i < dsPayDet.Tables[0].Rows.Count; i++)
                        {
                            foreach (DataRow tDr in temtbl.Rows)
                            {
                                if (dsPayDet.Tables[0].Rows[i]["Employeecode"].ToString() == tDr["Employeecode"].ToString())
                                {
                                    if (Convert.ToDecimal(tDr["Loanamt"]) < Convert.ToDecimal(dsPayDet.Tables[0].Rows[i]["Loanamt"]))
                                    {
                                        MessageBox.Show("Loan amount should not be greater than Installment amount", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        cValid = false;
                                        dsPayDet.Tables[0].Rows[i]["Loanamt"] = tDr["Loanamt"];
                                        return;
                                    }
                                }

                            }
                        }
                    }
              }
              else
              {
                    MessageBox.Show("No employee exist for this processing month", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cValid = false;
                    return;

              }
                //Added by Archana K. on 11/09/13 for Bug-18617 end  
            }
            else
            {
                MessageBox.Show("No employee exist for this processing month", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                return;

            }

            if (cValid == false)
            {
                cValid = true;
                return;
            }
            if (cnt == 0 && pAddMode)
            {
                MessageBox.Show("Please select any employee to process payroll", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                return;
            }
            
            oDataAccess.BeginTransaction();
            try
            {
                //this.mthGenerateNewInvNo();//Commented by Archana Khade on 12/09/13 for Bug-18617 
                //this.mthGenDocNo();
                this.mthUpd_Main_AcDet();
                if (this.pEditMode == true)
                {
                    vTran_Cd =Convert.ToInt16(dsMain.Tables[0].Rows[0]["tran_cd"]);
                }
                if (this.pAddMode)
                {
                    SqlStr = "select ident_current('MpMain') as Tran_cd";
                    DataRow tDr=oDataAccess.GetDataRow(SqlStr,null,vTimeOut);
                    vTran_Cd = Convert.ToInt16(tDr["Tran_cd"]);
                }
                //if (this.pEditMode OrderedEnumerableRowCollection )
                //{
                foreach (DataRow dr in dsPayDet.Tables[0].Rows)
                {
                    //Added by Archana K. on 12/09/13 for bug-18617 start
                    if (this.pEditMode && (Boolean)dr["PayGenerated"] == false && cRecord == false)
                        cRecord = true;
                    //Added by Archana K. on 12/09/13 for bug-18617 end
                    if (this.pEditMode || (Boolean)dr["PayGenerated"] == true)
                    {
                            SqlStr = "";
                            this.mthSavePayDet(ref SqlStr, "Emp_monthly_Payroll", dr);
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                    }
                    
                }
                //Added by Archana K. on 13/07/13 for Bug-18617 start
                DataRow[] drow = dsPayDet.Tables[0].Select("Paygenerated=true");
                if (cRecord == true && drow.Length == 0)
                {
                    DialogResult x = MessageBox.Show("Do you really want to delete the record?", "Record Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (x == DialogResult.Yes)
                    {
                        vMainFldVal = this.dsMain.Tables[0].Rows[0][vMainField].ToString();   
                        SqlStr = "Delete From MpMain where inv_no='" + this.txtInvNo.Text.Trim() + "' and Entry_ty='MP'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                        vMainFldVal = "";
                        drow = dsPayDet.Tables[0].Select("Paygenerated=false");
                        foreach (DataRow r in drow)
                            dsPayDet.Tables[0].Rows.Remove(r);
                        dsPayDet.AcceptChanges();
                        
                    }
                    else
                    {
                        dsPayDet.RejectChanges();
                        oDataAccess.RollbackTransaction();
                        return;
                    }
                }
                this.mthGenerateNewInvNo();
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();
                //Added by Archana K. on 12/09/13 for bug-18617 end
                //}
                    oDataAccess.CommitTransaction();

                 //SqlStr = "Execute Usp_Ent_Emp_Update_LastDate";   /*Ramya 19/10/12*/ //Commented by Archana K. on 26/09/13 for Bug-18246 
                //oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut,false);             /*Ramya 19/10/12*/ //Commented by Archana K. on 26/09/13 for Bug-18246 
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,this.pPApplName);
                oDataAccess.RollbackTransaction();
            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
            //vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();//Commented by Archana K. on 12/09/13 for bug-18617 
            //SqlStr = "Select top 1 * from "+vMainTblNm+" Where " + vMainField + "='" + vMainFldVal + "'";//Commented by Archana K. on 12/09/13 for bug-18617 
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "'" + (vMainFldVal == "" ? "" : " and " + vMainField + "='" + vMainFldVal + "'") + " Order by " + vOrdFld + " desc";//Changed by Archana K. on 12/09/13 for bug-18617 
            dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthBindClear();
            this.mthBindData();
            this.mthView();

        }
        private void mSaveCommandString(ref string vSaveString, string vkeyField, string tblTarget, ref DataSet dsSave)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + tblTarget + "' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                vSaveString = "Set DateFormat dmy insert into " + tblTarget;
                dsSave.Tables[0].AcceptChanges();

                foreach (DataColumn dtc1 in dsSave.Tables[0].Columns)
                {

                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + "[" + dtc1.ToString().Trim() + "]";
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
                vSaveString = "Set DateFormat dmy Update "+ tblTarget+" Set ";
                string vWhereCondn = string.Empty;
                foreach (DataColumn dtc1 in dsMain.Tables[0].Columns)
                {
                    //if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        else if ((dtc1.DataType.Name.ToLower() == "decimal" || dtc1.DataType.Name.ToLower() == "int16" || dtc1.DataType.Name.ToLower() == "int32") && string.IsNullOrEmpty(vfldVal))
                        {
                            vfldVal = "0";
                        }
                        else if (dtc1.DataType.Name.ToLower() == "datetime") //Alert Master
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        else if (dtc1.DataType.Name.ToLower() == "boolean") //Alert Master
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
                    if (vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1 || vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") > -1) 
                    {
                        if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                        vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        else if (dtc1.DataType.Name.ToLower() == "datetime") //Alert Master
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        else if (dtc1.DataType.Name.ToLower() == "boolean") //Alert Master
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
                        vfldList = vfldList +"["+dtc1.ToString().Trim() + "] = "; //Alert Master
                        vfldList = vfldList + vfldVal;
                    }
                }
                vSaveString = vSaveString + vfldList + vWhereCondn;
            }
        }
        private void mthSavePayDet(ref string vSaveString,string tblTarget, DataRow dr)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + tblTarget + "' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }
            /*<---Identity Columns*/  
            if((Boolean)dr["PayGenerated"]==false && this.pEditMode )
            {
                SqlStr = "Delete From Emp_Monthly_Payroll where Pay_Year='" + this.txtYear.Text.Trim() + "' and Pay_Month=" + this.fnNMonth(this.txtMonth.Text) + " and EmployeeCode='" + dr["EmployeeCode"].ToString() + "'";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
          
            sqlstr = "Select * From Emp_Monthly_Payroll where 1=2";
            DataSet tDsPay = new DataSet();
            tDsPay = oDataAccess.GetDataSet(sqlstr, null, vTimeOut);

            string vEmpCode = string.Empty;
            string vColValue = this.txtYear.Text + "," + this.fnNMonth(this.txtMonth.Text).ToString();
            string vFld_Nm = string.Empty;
            DateTime lastDate = new DateTime(int.Parse(this.txtYear.Text.Trim()),this.fnNMonth(this.txtMonth.Text), 1).AddMonths(1).AddDays(-1);//Added by Archana K. on27/09/13 for Bug-18246 
            vSaveString = "";
            string vFldNM = "";
            if (Convert.ToDecimal(dr["Tran_Cd"]) == 0) //New Record
            {
                dr["Tran_Cd"] = vTran_Cd;
                vSaveString = "Set DateFormat dmy insert into " + tblTarget;
                //dsSave.Tables[0].AcceptChanges();
                foreach (DataColumn dtc1 in tDsPay.Tables[0].Columns)
                {
                    vFldNM = dtc1.ColumnName.Trim();
                    if (vIdentityFields.IndexOf("#" + vFldNM + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + "[" + vFldNM + "]";
                        vfldVal =dr[vFldNM].ToString().Trim();

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
                            //Added by Archana K. on 27/09/13 for Bug-18246 start
                            if(vFldNM=="MnthLastDt")
                                vfldVal = "'" + lastDate + "'";
                            else
                            //Added by Archana K. on 27/09/13 for Bug-18246 end
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
            else 
            //if (Convert.ToDecimal(dr["Tran_Cd"]) != 0) //Edit Record
            {
                
                vSaveString = " PayGenerated=" + (dr["PayGenerated"].ToString().Trim() == "True" ? "1" : "0");
                vSaveString = vSaveString + "," + " NetPayment=" + dr["NetPayment"].ToString().Trim();
                vSaveString = vSaveString + "," + "Tran_Cd=" + vTran_Cd.ToString().Trim();
                vSaveString = vSaveString + "," +" Narr='" + dr["Narr"].ToString().Trim()+"'";
                foreach (DataRow tdr in this.dsEDMaster.Tables[0].Rows)
                {
                    vFld_Nm = tdr["fld_nm"].ToString();
                    vSaveString = vSaveString + "," + vFld_Nm;
                    vSaveString = vSaveString + "=" + dr[vFld_Nm].ToString();
                }
                vSaveString = vSaveString + "," + "MnthLastDt='" + lastDate + "'";//Added by Archana K. on 27/09/13 for Bug-18246
                vSaveString = vSaveString.Trim();
                vSaveString = "Update P set " + vSaveString + " From Emp_Monthly_Payroll P ";
                vSaveString = vSaveString + " Left Join EmployeeMast E on (E.EmployeeCode=P.EmployeeCode) ";
                vSaveString = vSaveString + " Left Join Loc_Master Lc on (Lc.Loc_Code=E.Loc_Code) ";
                vSaveString = vSaveString + " Where P.Pay_Year=" + this.txtYear.Text + " and P.Pay_Month=" + this.fnNMonth(txtMonth.Text).ToString();
                vSaveString = vSaveString + " and E.EmployeeCode='" + dr["EmployeeCode"].ToString().Trim() + "'";
                
            }
        }
        private void mthUpd_Main_AcDet()
        {

            //--->Main
            dsMain.Tables[0].Rows[0]["Entry_Ty"] = "MP";
            dsMain.Tables[0].Rows[0]["Date"] = this.dtpProcsessDate.Value.ToString();
            dsMain.Tables[0].Rows[0]["Doc_No"] = this.vDocNo.Trim();
            dsMain.Tables[0].Rows[0]["Rule"] = "";
            dsMain.Tables[0].Rows[0]["Party_Nm"] = vDefa_Db;
            dsMain.Tables[0].Rows[0]["Inv_No"] = this.txtInvNo.Text.Trim();
            dsMain.Tables[0].Rows[0]["L_Yn"] = vL_Yn.Trim();
            dsMain.Tables[0].Rows[0]["Gro_Amt"] = this.txtGrossSalary.Text;
            dsMain.Tables[0].Rows[0]["Net_Amt"] = this.txtNetSalary.Text; /*Ramya changed txtGrossSalary.text 25/01/13*/
            dsMain.Tables[0].Rows[0]["User_Name"] = this.pAppUerName;
            dsMain.Tables[0].Rows[0]["apGen"] = (((Boolean)drLcode["apgenps"] == false) ? "YES" : "PENDING");
            dsMain.Tables[0].Rows[0]["apGenTime"] = (((Boolean)drLcode["apgenps"] == false) ? DateTime.Now.ToString().Replace("AM", "").Replace("PM", "") : "");
            dsMain.Tables[0].Rows[0]["apGenby"] = (((Boolean)drLcode["apgenps"] == false) ? this.pAppUerName : "");
            dsMain.Tables[0].Rows[0]["SysDate"] = DateTime.Now.ToString().Replace("AM", "").Replace("PM", "");
            dsMain.Tables[0].Rows[0]["Ac_Id"] = 3;
            dsMain.Tables[0].Rows[0]["CompId"] = this.pCompId;
            this.mSaveCommandString(ref SqlStr, "", vMainTblNm , ref dsMain);
            oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);
            //<---Main
            //--->AcDet

            //this.mthUpd_AcDet();
            //<---AcDet
        }
     
        private void btnPayroll_Click1(object sender, EventArgs e)
        {
            if (this.pAddMode && this.txtInvNo.Text.Trim() == "")
            {
                this.txtInvNo.Text = funcGenInvNo();
            }
            SqlStr = "Execute [usp_Ent_Emp_Monthly_Payroll] ";//'','','','','','',''
            SqlStr = SqlStr + vTran_Cd.ToString().Trim();
            if (this.txtYear.Text.Trim() == "")
            {
                SqlStr = SqlStr + ",0";
            }
            else
            {
                SqlStr = SqlStr +","+ this.txtYear.Text.Trim();
            }
            if (this.txtMonth.Text.Trim() == "")
            {
                SqlStr = SqlStr + ",0";
            }
            else
            {
                SqlStr = SqlStr + "," + this.fnNMonth(this.txtMonth.Text).ToString().Trim();
            }
            SqlStr = SqlStr + ",'" + this.txtLocNm.Text.Trim() + "'";
            String FiltCon = " and 1=1";
            if (this.txtEmpCode.Text.Trim() != "")
            {
                FiltCon = " and e.EmployeeCode=''" + this.txtEmpCode.Text.Trim() + "''";
            }
           
            SqlStr = SqlStr + ",'" + FiltCon + "'";
            SqlStr = SqlStr + ",'"+vL_Yn+"'";
            SqlStr = SqlStr + ",''";
            this.dsPayDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthdgvPayRefresh();

        }
        private void btnPayroll_Click(object sender, EventArgs e)
        {
            //Commented by Archana K. on 11/09/13 for Bug-18617 start
            //if (this.txtYear.Text.Trim() == "") /*Ramya 18/06/13*/
            //{
            //    MessageBox.Show("Year Cannot be Blank...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    cValid = false;
            //    return;

            //}
            //if (this.txtMonth.Text.Trim() == "") /*Ramya 18/06/13*/
            //{
            //    MessageBox.Show("Month cannot Be Blank...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    cValid = false;
            //    return;

            //}
            //Commented by Archana K. on 11/09/13 for Bug-18617 end
            cValid = true;
            this.mthValidation();//Added by Archana K. on 10/09/13 for Bug-18617
            if (cValid == false)
            {
                cValid = true;
                return;
            }
            if (this.pAddMode && this.txtInvNo.Text.Trim() == "")
            {
                this.txtInvNo.Text = funcGenInvNo();
            }

            this.StbLbl.Text = "Generating Payroll....";
            this.stbMain.Refresh();

            SqlStr = "set dateformat dmy Execute [usp_Ent_Emp_Monthly_Payroll] ";//'','','','','','',''
            SqlStr = SqlStr + vTran_Cd.ToString().Trim();
            if (this.txtYear.Text.Trim() == "")
            {
                SqlStr = SqlStr + ",0";
            }
            else
            {
                SqlStr = SqlStr + ",'" + this.txtYear.Text.Trim()+"'";
            }
            if (this.txtMonth.Text.Trim() == "")
            {
                SqlStr = SqlStr + ",0";
            }
            else
            {
                SqlStr = SqlStr + "," + this.fnNMonth(this.txtMonth.Text).ToString().Trim();
            }
            SqlStr = SqlStr + ",'" + this.txtLocNm.Text.Trim() + "'";
            String FiltCon = " and 1=1";
            if (vLoc_Code != "")
            {
                FiltCon = " and e.Loc_Code=''" + vLoc_Code + "''";
            }
            if (this.txtEmpCode.Text.Trim() != "")
            {
                FiltCon = FiltCon + " and e.EmployeeCode=''" + this.txtEmpCode.Text.Trim() + "''";
            }
            if (this.txtDept.Text.Trim() != "")
            {
                FiltCon = FiltCon + " and e.Department=''" + this.txtDept.Text.Trim() + "''";
            }
            if (this.txtCategory.Text.Trim() != "")
            {
                FiltCon = FiltCon + " and e.Category=''" + this.txtCategory.Text.Trim() + "''";
            }
            //*** Added by Sachin N. S. on 24/06/2014 for Bug-21114 -- Start
            if (this.txtCalcPeriod.Text.Trim() != "")
            {
                FiltCon = FiltCon + " and e.CalcPeriod=''" + this.txtCalcPeriod.Text.Trim() + "''";
            }
            //*** Added by Sachin N. S. on 24/06/2014 for Bug-21114 -- End

            SqlStr = SqlStr + ",'" + FiltCon + "'";
            SqlStr = SqlStr + ",'" + vL_Yn + "'";
            SqlStr = SqlStr + ",''";
                      
            this.dsPayDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthdgvPayRefresh();
            if (this.dgvPayDet.Columns.Count != 0)   /*Ramya 06/02/13 */
            {
                this.dgvPayDet.Columns["colYear"].ReadOnly = true;
                this.dgvPayDet.Columns["colcMonth"].ReadOnly = true;
                this.dgvPayDet.Columns["colEmp"].ReadOnly = true;
                this.dgvPayDet.Columns["colEmpCode"].ReadOnly = true;
                this.dgvPayDet.Columns["colLocation"].ReadOnly = true;
                this.dgvPayDet.Columns["colDept"].ReadOnly = true;
                this.dgvPayDet.Columns["colCate"].ReadOnly = true;
                this.dgvPayDet.Columns["ColNetPayment"].ReadOnly = true;
                this.dgvPayDet.Columns["ColGrossPayment"].ReadOnly = true;
                this.dgvPayDet.Columns["colSalPaidDays"].ReadOnly = true;
                this.dgvPayDet.Columns["colPrint"].ReadOnly = true;
                this.dgvPayDet.Columns["colSel"].ReadOnly = false;
                this.dgvPayDet.Columns["colCalcPeriod"].ReadOnly = false;       // Added by Sachin N. S. on 09/07/2014 for Bug-21114
            }
            if (txtYear.Text.Trim() != "" && txtMonth.Text.Trim() != "")     /*Ramya 08/03/2013*/
            {
                SqlStr = "Set DateFormat dmy Select p.HeadType,p.PayEffect,e.Short_Nm,e.Fld_Nm,PayEditable from Emp_Pay_Head_Master e inner join Emp_Pay_Head p on (e.HeadTypeCode=p.HeadTypeCode) Where isDeActive=1 and DeactFrom<'" + "1/" + fnNMonth(txtMonth.Text.Trim())+"/" + txtYear.Text.Trim() + "' ";
                SqlStr = SqlStr + " Order By p.SortOrd,e.SortOrd";
                DataSet dse = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            
                if (dse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dse.Tables[0].Rows)
                    {

                        dgvPayDet.Columns["Col_" + dr["Fld_Nm"].ToString()].ReadOnly = true;
                        //dgvPayDet.Columns["Col_" + dr["Fld_Nm"].ToString()].Visible = false;
                        
                    }
                }
            }
            mthchkdata();//Added by Archana K. on 21/09/13 for Bug-18246
            this.StbLbl.Text = "Payroll Generation Completed";
            this.stbMain.Refresh();
            this.StbLbl.Text = "";
            this.stbMain.Refresh();
        }
        #endregion

#region Navigation Button
        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = this.dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
           // SqlStr = "Select top 1 * from " + vMainTblNm + "  order by " + vMainField;
            SqlStr = "Select top 1 * from " + vMainTblNm + " where L_yn='" + vL_Yn + "'  order by " + vMainField;
            this.dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();
            this.mthChkNavigationButton();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = this.dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
           // SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "' and " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
            this.dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = this.dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
           // SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "' and " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
            this.dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            this.mthView();

            this.mthChkNavigationButton();
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = false;
            this.pEditMode = false;
            //this.mthEnableDisableFormControls();
            DataSet dsTemp = new DataSet();
            //string SqlStr = "select top 1  " + vMainField + " as Col1 from " + vMainTblNm + " order by  " + vOrdFld + " desc";
            string SqlStr = "select top 1  " + vMainField + " as Col1 from " + vMainTblNm + " where L_yn='" + vL_Yn + "' order by  " + vOrdFld + " desc";
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            vMainFldVal = "";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["Col1"].ToString()) == false)
                {
                    vMainFldVal = dsTemp.Tables[0].Rows[0]["Col1"].ToString().Trim();
                }
            }
           // SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            SqlStr = "Select top 1 * from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "' and " + vMainField + "='" + vMainFldVal + "'";

            this.dsMain = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            this.mthView();
            this.mthChkNavigationButton();

        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                this.btnDelete.Enabled = true;
            }
            if (vBtnPrint && this.pPrintButton)
            {
                this.btnEmail.Enabled = true ;
                this.btnPrint.Enabled = true;
                this.btnExportPdf.Enabled = true;
            }
            if (this.btnNew.Enabled || this.btnEdit.Enabled)
            {
                this.btnPreview.Enabled = true;
                try            /*Ramya 21/11/12*/
                {
                    if (dgvPayDet.Rows.Count == 0)
                    {
                        this.btnPreview.Enabled = false;
                    }
                }
                catch
                {

                }
            }
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnLogout.Enabled = false;
           // this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;

            //this.dtpDeact.Enabled = false;
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
                if (this.dsMain.Tables[0].Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false) //Rup
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;

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

                    //SqlStr = "select Tran_Cd from " + vMainTblNm + " Where " + vMainField + ">'" + vMainFldVal + "'";

                    SqlStr = "select Tran_Cd from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "' and " + vMainField + ">'" + vMainFldVal + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    //SqlStr = "select Tran_Cd from " + vMainTblNm + " Where " + vMainField + "<'" + vMainFldVal + "'";

                    SqlStr = "select Tran_Cd from " + vMainTblNm + " Where  L_yn='" + vL_Yn + "' and " + vMainField + "<'" + vMainFldVal + "'";

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
                        //this.btnHeadNm.Enabled = true;  //Ramya 08/02/12
                        //this.btnFldNm.Enabled = true;  //Ramya 08/02/12
                    }
                }
            }//Added by Archana K. on 17/05/13 for Bug-7899
            this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
           
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
                string fName = appPath + @"\bmp\loc-on.gif";   /*Ramya*/
                if (File.Exists(fName) == true)
                {
                    this.btnYear.Image = Image.FromFile(fName);
                    this.btnMonth.Image = Image.FromFile(fName);
                    //this.btnDesc.Image = Image.FromFile(fName);
                }
                this.btnEmpNm.Enabled = false;
                this.btnLocNm.Enabled = false;
               

                this.dgvPayDet.ReadOnly = false;
                if (this.dgvPayDet.Columns.Count != 0)
                {
                    this.dgvPayDet.Columns["colYear"].ReadOnly = true;
                    this.dgvPayDet.Columns["colcMonth"].ReadOnly = true;
                    this.dgvPayDet.Columns["colEmp"].ReadOnly = true;
                    this.dgvPayDet.Columns["colEmpCode"].ReadOnly = true;
                    this.dgvPayDet.Columns["colLocation"].ReadOnly = true;
                    this.dgvPayDet.Columns["colDept"].ReadOnly = true;
                    this.dgvPayDet.Columns["colCate"].ReadOnly = true;
                    this.dgvPayDet.Columns["ColNetPayment"].ReadOnly = true;
                    this.dgvPayDet.Columns["ColGrossPayment"].ReadOnly = true;
                    this.dgvPayDet.Columns["colSalPaidDays"].ReadOnly = true;
                    this.dgvPayDet.Columns["colPrint"].ReadOnly = true;
                    this.dgvPayDet.Columns["colSel"].ReadOnly = false ;
                
                    string ColNm = string.Empty;
                    foreach (DataRow dr in dsEDMaster.Tables[0].Rows)
                    {
                        ColNm = "Col_" + dr["fld_nm"].ToString();
                        if (dr["payEditable"].ToString() == "False")
                        {
                            this.dgvPayDet.Columns[ColNm].ReadOnly = true;
                        }
                        else
                        {
                            this.dgvPayDet.Columns[ColNm].ReadOnly = false;
                        }

                    }
                }
            }
            else
            {
                string fName = appPath + @"\bmp\pickup.gif";   /*Ramya*/
                if (File.Exists(fName) == true)
                {
                    this.btnYear.Image = Image.FromFile(fName);
                    this.btnMonth.Image = Image.FromFile(fName);
                    //this.btnDesc.Image = Image.FromFile(fName);
                }
  
                this.btnEmpNm.Enabled = true;
                this.dgvPayDet.ReadOnly = false;
                //this.dgvPayDet.Columns["colPrint"].ReadOnly = false;
                if (this.dgvPayDet.Columns.Count != 0)
                {
                    this.dgvPayDet.Columns["colPrint"].ReadOnly = false;
                    this.dgvPayDet.Columns["colSalPaidDays"].ReadOnly = true;
                    for(int i=0;i<= this.dgvPayDet.Columns.Count-1;i++)
                    {

                        if (this.dgvPayDet.Columns[i].Name.ToLower() != "colprint")
                        {
                            this.dgvPayDet.Columns[i].ReadOnly = true;
                        }
                    }

                    this.dgvPayDet.Refresh();
                }

                this.btnLocNm.Enabled = true;
            }
 
            this.vgridcount = this.dgvPayDet.Rows.Count;//--commented by satish pal for bug-18215 dated 13/08/2013
            if (this.pAddMode)
            {
                this.btnLocNm.Enabled = true;
                this.btnDept.Enabled = true;
                this.btnCate.Enabled = true;
                this.btnEmpNm.Enabled = true;
                this.btnYear.Enabled = true;  /*Ramya 30/10/12*/
                this.btnMonth.Enabled = true; /*Ramya 30/10/12*/
                this.btnCalcPeriod.Enabled = true;  // Added by Sachin N. S. on 27/06/2014 for Bug-21114
            }
            else
            {
                this.btnLocNm.Enabled = false;
                this.btnDept.Enabled = false;
                this.btnCate.Enabled = false;
                this.btnEmpNm.Enabled = false;
                this.btnYear.Enabled = false;  /*Ramya 30/10/12*/
                this.btnMonth.Enabled = false; /*Ramya 30/10/12*/
                this.btnCalcPeriod.Enabled = false;  // Added by Sachin N. S. on 27/06/2014 for Bug-21114
                //--Addedd by satish pal for -18215 dated 13/08/2013-start
                if (this.vgridcount == 0)
                {
                    this.btnLocNm.Enabled = vEnabled;
                    this.btnDept.Enabled = vEnabled;
                    this.btnCate.Enabled = vEnabled;
                    this.btnEmpNm.Enabled = vEnabled;
                    this.btnYear.Enabled = vEnabled;
                    this.btnMonth.Enabled = vEnabled;
                    this.btnCalcPeriod.Enabled = vEnabled;  // Added by Sachin N. S. on 27/06/2014 for Bug-21114
                }
                //--Addedd by satish pal for-18215  dated 13/08/2013-End
            }
                //satish-end
            this.btnPayroll.Enabled = vEnabled;
          
            this.txtYear.Enabled = vEnabled;
            this.txtMonth.Enabled = vEnabled;
            this.dtpProcsessDate.Enabled = vEnabled;

            this.txtEmpNm.Enabled = vEnabled;
            this.txtEmpCode.Enabled = vEnabled;
            this.txtDept.Enabled = vEnabled;
            this.txtLocNm.Enabled = vEnabled;
            this.txtCategory.Enabled = vEnabled;
            string vControl = string.Empty;
            this.chkSelectAll.Enabled = vEnabled;//Added by Archana K. on 10/09/13 for Bug-18617 
        }
        #endregion

#region Function and Method For Action And Navigation Buttons
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
                voldInvoiceNo = Convert.ToInt16(this.txtInvNo.Text);
            }
            int _vInvoiceEn = voldInvoiceNo;
            if (this.txtInvNo.ToString().Trim() == "")
            {
                i_vInvoiceNo = 1;
            }
            else
            {
                i_vInvoiceNo = Convert.ToInt16(this.txtInvNo.Text);
            }

            SqlStr = "Select * from Gen_inv with (NOLOCK) where 1=0";
            DataTable Gen_inv_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "Select * from Gen_miss with (NOLOCK) where 1=0";
            DataTable Gen_miss_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataRow drGen_inv_vw = Gen_inv_vw.NewRow();
            drGen_inv_vw["Entry_ty"] = "MP";
            drGen_inv_vw["Inv_dt"] = this.dtpProcsessDate.Value;
            drGen_inv_vw["Inv_sr"] = "";
            drGen_inv_vw["Inv_No"] = i_vInvoiceNo;
            drGen_inv_vw["l_Yn"] = vL_Yn;
            Gen_inv_vw.Rows.Add(drGen_inv_vw);

            DataRow drGen_miss_vw = Gen_miss_vw.NewRow();
            drGen_miss_vw["Entry_ty"] = "MP";
            drGen_miss_vw["Inv_dt"] = this.dtpProcsessDate.Value;
            drGen_miss_vw["Inv_sr"] = "";
            drGen_miss_vw["Inv_No"] = i_vInvoiceNo;
            drGen_miss_vw["l_Yn"] = vL_Yn;
            drGen_miss_vw["Flag"] = "Y";
            Gen_miss_vw.Rows.Add(drGen_miss_vw);

            Boolean mrollback = true;

            SqlStr = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX)";
            SqlStr = SqlStr + " where Entry_ty = 'MP' And Inv_sr ='' And L_yn ='" + vL_Yn + "'";
            DataTable tmptbl_vw = new DataTable();
            tmptbl_vw = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "";
            if (tmptbl_vw.Rows.Count <= 0)
            {
                SqlStr = "Insert into Gen_inv (Entry_Ty,Inv_Sr,Inv_No,L_Yn,Inv_Dt,CompId) Values ('MP',''," + Gen_inv_vw.Rows[0]["Inv_No"].ToString().Trim() + ",'" + vL_Yn + "','" + this.dtpProcsessDate.Text + "',0)";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
            else
            {
                //if (Convert.ToInt16(tmptbl_vw.Rows[0]["Inv_no"]) < Convert.ToInt16(drGen_inv_vw["Inv_No"])//Commented by Archana K. on 12/09/13 for Bug-18617 
                if (Convert.ToInt16(tmptbl_vw.Rows[0]["Inv_no"]) < Convert.ToInt16(drGen_inv_vw["Inv_No"]) && dsPayDet.Tables[0].Rows.Count>0)//Changed by Archana K. on 12/09/13 for Bug-18617 
                {

                    mCond = "Entry_ty = 'MP' And Inv_sr='' And L_yn ='" + vL_Yn + "'";
                    SqlStr = "Set DateFormat DMY  Update Gen_inv Set Inv_No=" + i_vInvoiceNo.ToString().Trim() + ",Inv_Dt='" + this.dtpProcsessDate.Text + "' where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
                //Added by Archana K. on 12/09/13 for Bug-18617 start
                else if(dsPayDet.Tables[0].Rows.Count==0)
                {
                    mCond = "Entry_ty = 'MP' And Inv_no=" + i_vInvoiceNo.ToString().Trim();
                    SqlStr = "Set DateFormat DMY  Update Gen_inv Set inv_no=" + Gen_inv_vw.Rows[0]["Inv_no"].ToString() + "-1 where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                    mCond = "Entry_ty = 'MP' And Inv_no=" + i_vInvoiceNo.ToString().Trim();
                    SqlStr = "Set DateFormat DMY  Update Gen_miss Set Flag='N' where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
                //Added by Archana K. on 12/09/13 for Bug-18617 end

            }
            SqlStr = "Select Top 1 Flag from Gen_miss where Entry_ty = 'MP' And Inv_sr = '' And Inv_no =" + Gen_inv_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
            string  vFoundInMiss = "Y";
            DataTable tmptbl_vw1 = new DataTable();
            tmptbl_vw1 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            SqlStr = "";
            if (tmptbl_vw1.Rows.Count <= 0)
            {
                vFoundInMiss = "N";
                SqlStr = "Set DateFormat DMY INSERT INTO GEN_MISS ([entry_ty],[inv_sr],[inv_no],[flag],[l_yn],[inv_dt],[user_name],[CompId]) VALUES";
                SqlStr = SqlStr + "('MP',''," + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",'" + Gen_miss_vw.Rows[0]["Flag"].ToString() + "','" + vL_Yn + "','" + this.dtpProcsessDate.Text + "','',0)";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            }
            else
            {
                vFoundInMiss = tmptbl_vw1.Rows[0]["Flag"].ToString();
                //if (vFoundInMiss == "N")//Commented by Archana K. on 12/09/13 for Bug-18617 
                if (vFoundInMiss == "N" && dsPayDet.Tables[0].Rows.Count > 0)//Changed by Archana K. on 12/09/13 for Bug-18617
                {
                    mCond = "Entry_ty = 'MP' And Inv_sr='' and Inv_no=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
                    SqlStr = "Set DateFormat DMY Update GEN_MISS Set Inv_No=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",Inv_Dt='" + this.dtpProcsessDate.Text + "',Flag='" + Gen_miss_vw.Rows[0]["Flag"].ToString() + "'  where " + mCond;
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                }
            }


            if (vFoundInMiss == "N")
            {
                vInv_No = Convert.ToString(iInv_No).Trim();
                vInv_No = vInv_No.PadLeft(vInvNoLen, Convert.ToChar("0"));
                SqlStr = "Select Top 1 Entry_ty from " + vMainTblNm + " where Entry_ty = '" + vGen_Inv + "' And Inv_no = '" + this.txtInvNo.Text.Trim() + "' And L_yn = '" + vL_Yn + "'";
                DataTable tmptbl_vw2 = new DataTable();
                tmptbl_vw2 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                if (tmptbl_vw2.Rows.Count <= 0)
                {
                    mrollback = false;
                    return ; //End Do
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
                mCond = "Entry_ty = 'MP' And Inv_sr='' and Inv_no=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + " And L_yn ='" + vL_Yn + "'";
                try
                {
                    SqlStr = "Set DateFormat DMY Update GEN_MISS Set Inv_No=" + Gen_miss_vw.Rows[0]["Inv_no"].ToString() + ",Inv_Dt='" + this.dtpProcsessDate.Text + "' where " + mCond;
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

        private string funcGenInvNo()
        {
            if (vL_Yn == "")
            {
                SqlStr = "Select Sta_Dt,End_Dt From Vudyog..Co_Mast Where CompId=" + this.pCompId.ToString();
                DataRow drYear = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
                vL_Yn = Convert.ToDateTime(drYear["Sta_Dt"]).Year.ToString().Trim() + "-" + Convert.ToDateTime(drYear["End_Dt"]).Year.ToString().Trim();

            }
            SqlStr = "Select Inv_No=isnull(max(Inv_No),0)+1 From Gen_Inv where Entry_ty='" + vGen_Inv + "' and L_Yn='" + vL_Yn + "'";
            DataRow drInvNo;
            drInvNo = oDataAccess.GetDataRow(SqlStr, null, vTimeOut);
            iInv_No = Convert.ToInt16(drInvNo["Inv_No"]);
            vInv_No = Convert.ToString(iInv_No).Trim();
            vInv_No = vInv_No.PadLeft(vInvNoLen, Convert.ToChar("0"));
            return vInv_No;


        }

        //private void mthValidation(ref Boolean vIsFrmValid)//Commented by Archana K. on 10/09/13 for Bug-18617
        private void mthValidation()//Changed by Archana K. on 10/09/13 for Bug-18617
        {
            //Commented by Archana K. on 10/09/13 start
            //string vControl = string.Empty;
            //int cnt = 0;
            //if (cnt == 0)
            //{
            //    MessageBox.Show("Please Select and Enter Leave to be Updated", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    // vIsFrmValid = false;
            //}
            //Commented by Archana K. on 10/09/13 end
            //Added by Archana K. on 06/09/13 start
            if (this.txtYear.Text.Trim() == "")
            {
                MessageBox.Show("Year Cannot be Blank...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                return;

            }
            if (this.txtMonth.Text.Trim() == "")
            {
                MessageBox.Show("Month cannot Be Blank...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cValid = false;
                return;

            }
            
            //Added by Archana K. on 06/09/13 end
        }
#endregion
#region Security Checking
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
            foreach(Control ctrl in this.Controls)
            {
                ctrl.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            }
            this.tbEmpDet.TabPages[0].BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            this.tbEmpDet.TabPages[1].BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            
        }

        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpMonthlyPayroll.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
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
#endregion
#region Data View
        private void mthView()
        {
            
            
            this.mthBindClear();
            if (this.pAddMode == false && dsMain.Tables[0].Rows.Count>0)
            {
                vTran_Cd = Convert.ToInt16(dsMain.Tables[0].Rows[0]["Tran_Cd"]);
            }
            else
            {
                vTran_Cd = 0;
            }
            if (this.pAddMode == false && this.pEditMode == false && vTran_Cd>0)
            {
                SqlStr = "Select colPrint=cast(0 as bit),e.EmployeeName,e.Department,e.Category,e.Designation,ProjGross=GrossPayment,OrgTDS=TDSAmt,e.CalcPeriod,p.* From Emp_Monthly_Payroll p inner join EmployeeMast e on (e.EmployeeCode=p.EmployeeCode) where p.Tran_Cd=" + vTran_Cd.ToString() + " order by EmployeeName";      // Changed by Sachin N. S. on 24/06/2014 for Bug-21114
                //SqlStr = "Select colPrint=cast(0 as bit),e.EmployeeName,e.Department,e.Category,e.Designation,ProjGross=GrossPayment,OrgTDS=TDSAmt,p.* From Emp_Monthly_Payroll p inner join EmployeeMast e on (e.EmployeeCode=p.EmployeeCode) where p.Tran_Cd=" + vTran_Cd.ToString() + " order by EmployeeName";
                //SqlStr = "Select colPrint=cast(0 as bit),e.EmployeeName,e.Department,e.Category,e.Designation,ProjGross=GrossPayment,OrgTDS=TDSAmt,p.*,iif(e.loanamt, From Emp_Monthly_Payroll p inner join EmployeeMast e on (e.EmployeeCode=p.EmployeeCode) where p.Tran_Cd=" + vTran_Cd.ToString() + " order by EmployeeName";
                dsPayDet = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            }
            //mthEnableDisableFormControls(); //?
            this.mthChkNavigationButton(); //?
            this.mthBindData();
            mthEnableDisableFormControls(); //?
            if (this.dsPayDet.Tables.Count == 0)
            {
                return;
            }
            if (this.dsPayDet.Tables[0].Rows.Count > 0)
            {
                this.txtYear.Text = dsPayDet.Tables[0].Rows[0]["Pay_Year"].ToString();
                this.txtMonth.Text = this.fnCMonth(Convert.ToInt16(dsPayDet.Tables[0].Rows[0]["Pay_Month"]));
                
                DataTable tblDistinctDept= new DataTable();
                string[] FieldNamesE = { "EmployeeCode", "EmployeeName" };
                udDataTableQuery.cSelectDistinct qDeptE = new udDataTableQuery.cSelectDistinct();
                tblDistinctDept = qDeptE.SelectDistinct(dsPayDet.Tables[0], "", FieldNamesE);
                if (tblDistinctDept.Rows.Count == 1)
                {
                    this.txtEmpNm.Text = tblDistinctDept.Rows[0]["EmployeeName"].ToString();
                    this.txtEmpCode.Text = tblDistinctDept.Rows[0]["EmployeeCode"].ToString();
                }

                
                string[] FieldNamesD = {"Department"};
                tblDistinctDept = new DataTable();
                udDataTableQuery.cSelectDistinct qDeptD = new udDataTableQuery.cSelectDistinct();
                tblDistinctDept = qDeptD.SelectDistinct(dsPayDet.Tables[0], "", FieldNamesD);
                if (tblDistinctDept.Rows.Count == 1)
                {
                    this.txtDept.Text = tblDistinctDept.Rows[0]["Department"].ToString();
                }
                tblDistinctDept = new DataTable();
               
                string[] FieldNamesC = { "Category" };
                udDataTableQuery.cSelectDistinct qDeptC = new udDataTableQuery.cSelectDistinct();
                tblDistinctDept = qDeptC.SelectDistinct(dsPayDet.Tables[0], "", FieldNamesC);
                if (tblDistinctDept.Rows.Count == 1)
                {
                    this.txtCategory.Text = tblDistinctDept.Rows[0]["Category"].ToString();
                }
                this.txtCalcPeriod.Text = dsPayDet.Tables[0].Rows[0]["CalcPeriod"].ToString();      // Added by Sachin N. S. on 24/06/2014 for Bug-21114                
                
            }
            
            if (tbEmpDet.SelectedTab.Name == "tbpSum" && (this.pAddMode==false  || this.pEditMode==false ))
            {
                this.mthDgvSumColumns();
                this.mthCalsum();
            }

        }
        private void mthBindData()
        {
            this.txtInvNo.DataBindings.Add("Text", dsMain.Tables[0], "Inv_No");
            this.dtpProcsessDate.DataBindings.Add("Text", dsMain.Tables[0], "Date");
            if (dsPayDet.Tables.Count == 0)
            {
                return;
            }

            this.mthdgvPayRefresh();
            
           
        }
        private void mthBindClear()
        {
            this.txtInvNo.DataBindings.Clear();
            this.dtpProcsessDate.DataBindings.Clear();
            this.txtYear.Text = "";
            this.txtMonth.Text = "";
            this.txtInvNo.Text = "";
            this.txtEmpCode.Text = "";
            this.txtEmpNm.Text = "";
            this.txtDept.Text = "";
            this.txtCategory.Text = "";
            this.txtLocNm.Text = "";
            vLoc_Code = "";
            vLocNm = "";
            this.dsPayDet.Clear();//Added by Archana K. on 11/09/13 for Bug-18617

        }
        private void mthdgvPayRefresh()
        {

            this.dgvPayDet.Columns.Clear();
            this.dgvPayDet.DataSource = this.dsPayDet.Tables[0];
            this.dgvPayDet.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn ColSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColSel.HeaderText = "Processed";  /*Ramya chnaged Select to Processed 04/01/13*/
            ColSel.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //ColSel.Width = 45;
            ColSel.Width = 65;
            ColSel.Name = "ColSel";
            this.dgvPayDet.Columns.Add(ColSel);

            System.Windows.Forms.DataGridViewCheckBoxColumn ColPrint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColPrint.HeaderText = "Print";
            ColPrint.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColPrint.Width = 40;
            ColPrint.Name = "ColPrint";
            this.dgvPayDet.Columns.Add(ColPrint);


            System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colId.HeaderText = "Id";
            colId.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colId.Name = "colId";
            this.dgvPayDet.Columns.Add(colId);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colYear = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colYear.HeaderText = "Year";
            colYear.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colYear.Name = "colYear";
            colYear.DefaultCellStyle.BackColor = Color.LightGray;
            colYear.Width = 45;
            this.dgvPayDet.Columns.Add(colYear);
            colYear.Frozen = true;


            System.Windows.Forms.DataGridViewTextBoxColumn colcMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colcMonth.HeaderText = "Month";
            colcMonth.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colcMonth.Name = "colcMonth";
            colcMonth.DefaultCellStyle.BackColor = Color.LightGray;
            colcMonth.Width = 60;
            this.dgvPayDet.Columns.Add(colcMonth);
            colcMonth.Frozen = true;
            colcMonth.ReadOnly = true;


            System.Windows.Forms.DataGridViewTextBoxColumn colEmp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmp.HeaderText = "Employee Name";
            colEmp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEmp.Name = "colEmp";
            colEmp.DefaultCellStyle.BackColor = Color.LightGray;
            colEmp.Width = 200;
            this.dgvPayDet.Columns.Add(colEmp);
            colEmp.Frozen = true;

            System.Windows.Forms.DataGridViewTextBoxColumn colEmpCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmpCode.HeaderText = "Employee Code";
            colEmpCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEmpCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEmpCode.Width = 60;
            colEmpCode.Name = "colEmpCode";
            colEmpCode.DefaultCellStyle.BackColor = Color.LightGray; 
            this.dgvPayDet.Columns.Add(colEmpCode);

            //****** Added by Sachin N. S. on 09/07/2014 for Bug-21114 -- Start
            System.Windows.Forms.DataGridViewTextBoxColumn colCalcPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCalcPeriod.HeaderText = "Calculation Period";
            colCalcPeriod.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colCalcPeriod.Width = 75;
            colCalcPeriod.Name = "colCalcPeriod";
            colCalcPeriod.DefaultCellStyle.BackColor = Color.LightGray;
            colCalcPeriod.DataPropertyName = "CalcPeriod";
            this.dgvPayDet.Columns.Add(colCalcPeriod);
            //****** Added by Sachin N. S. on 09/07/2014 for Bug-21114 -- End

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colSalPaidDays = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colSalPaidDays.HeaderText = "Salary Paid Days";
            colSalPaidDays.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSalPaidDays.Name = "colSalPaidDays";
            colSalPaidDays.DefaultCellStyle.BackColor = Color.LightGray;
            colSalPaidDays.DecimalLength = 3;
            colSalPaidDays.Width = 80;
            this.dgvPayDet.Columns.Add(colSalPaidDays);
            colSalPaidDays.Frozen = true;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colGrossPayment = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colGrossPayment.HeaderText = "Gross Payment";
            colGrossPayment.Name = "colGrossPayment";
            colGrossPayment.DefaultCellStyle.BackColor = Color.LightGray;
            colGrossPayment.Width = 80;
            colGrossPayment.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colGrossPayment.DecimalLength = 2;
            this.dgvPayDet.Columns.Add(colGrossPayment);
            colGrossPayment.Frozen = true;


            udclsDGVNumericColumn.CNumEditDataGridViewColumn colNetPayment = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colNetPayment.HeaderText = "Net Payment";
            colNetPayment.Name = "colNetPayment";
            colNetPayment.DefaultCellStyle.BackColor = Color.LightGray;
            colNetPayment.Width = 80;
            colNetPayment.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colNetPayment.DecimalLength = 2;
            this.dgvPayDet.Columns.Add(colNetPayment);
            colNetPayment.Frozen = true;


            string VHTxt = string.Empty;
            foreach (DataRow dr in this.dsEDMaster.Tables[0].Rows)
            {
                udclsDGVNumericColumn.CNumEditDataGridViewColumn  col1 = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                
                VHTxt = dr["Short_Nm"].ToString().Trim();
                col1.HeaderText = VHTxt;
                col1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col1.DecimalLength = 2;
                if (dr["PayEffect"].ToString() != "") { col1.Tag = dr["PayEffect"].ToString(); }
               // if (((Boolean)dr["PayEditable"]) == false) { col1.DefaultCellStyle.BackColor = Color.LightGray; }
                if (((Boolean)dr["PayEditable"]) == false) { col1.DefaultCellStyle.BackColor = Color.LightGray; col1.ReadOnly = true; } /*Ramya 15/06/13*/  
                VHTxt = "Col_" + dr["Fld_Nm"].ToString();
                col1.Name = VHTxt;
                col1.DataPropertyName = dr["Fld_Nm"].ToString();
                col1.pValidFunNm = dr["dNetExpr"].ToString(); //Rup 14/06/2013
                this.dgvPayDet.Columns.Add(col1);
                //Col1.DefaultCellStyle.BackColor = Color.LightGray;
            }

            System.Windows.Forms.DataGridViewTextBoxColumn colNarr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNarr.HeaderText = "Narr";
            colNarr.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colNarr.Name = "colNarr";
            this.dgvPayDet.Columns.Add(colNarr);


            System.Windows.Forms.DataGridViewTextBoxColumn colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLocation.HeaderText = "Location";
            colLocation.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colLocation.Name = "colLocation";
            colLocation.DefaultCellStyle.BackColor = Color.LightGray; 
            this.dgvPayDet.Columns.Add(colLocation);

            System.Windows.Forms.DataGridViewTextBoxColumn colDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDept.HeaderText = "Department";
            colDept.DataPropertyName = "Department";
            colDept.DefaultCellStyle.BackColor = Color.LightGray; 
            colDept.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDept.Name = "colDept";
            this.dgvPayDet.Columns.Add(colDept);

            System.Windows.Forms.DataGridViewTextBoxColumn colCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCate.HeaderText = "Category";
            colCate.DataPropertyName = "Category";
            colCate.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colCate.DefaultCellStyle.BackColor = Color.LightGray; 
            colCate.Name = "colCate";
            colCate.DefaultCellStyle.BackColor = Color.LightGray; 
            this.dgvPayDet.Columns.Add(colCate);


            System.Windows.Forms.DataGridViewImageColumn coliNarr = new System.Windows.Forms.DataGridViewImageColumn();
            coliNarr.HeaderText = "Narration";
            coliNarr.Name = "coliNarr";
            //coliNarr.HeaderCell.Style.ForeColor = Color.Blue;
            string fName = appPath + @"\bmp\narration.gif";
            //string vFileName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName))
            {
                coliNarr.Image = Image.FromFile(fName);
            }
            this.dgvPayDet.Columns.Add(coliNarr);



            //System.Windows.Forms.DataGridViewTextBoxColumn coliNarr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //coliNarr.HeaderText = "Narration";
            //coliNarr.Name = "coliNarr";
            ////coliNarr.HeaderCell.Style.ForeColor = Color.Blue;
            //string fName = appPath + @"\bmp\narration.gif";
            ////string vFileName = this.pAppPath + @"\bmp\pickup.gif";
            //if (File.Exists(fName))
            //{
            //    coliNarr.Image = Image.FromFile(fName);
            //}
            //this.dgvPayDet.Columns.Add(coliNarr);




            //vTDSPer = 0, vProjGross = 0, vMonthPen=0,vOrgTDS
            /*TDS Calculation--->*/
            udclsDGVNumericColumn.CNumEditDataGridViewColumn colTDSPer = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colTDSPer.HeaderText = "TDSPer";
            colTDSPer.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTDSPer.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colTDSPer.DecimalLength = 2;
            colTDSPer.Name = "colTDSPer";
            colTDSPer.DataPropertyName = "TDSPer";
            this.dgvPayDet.Columns.Add(colTDSPer);
            this.dgvPayDet.Columns["colTDSPer"].Visible = false; 

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colProjGross = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colProjGross.HeaderText = "ProjGross";
            colProjGross.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colProjGross.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colProjGross.DecimalLength = 2;
            colProjGross.Name = "colProjGross";
            colProjGross.DataPropertyName = "ProjGross";
            this.dgvPayDet.Columns.Add(colProjGross);
            this.dgvPayDet.Columns["colProjGross"].Visible = false;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colMonthPen = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colMonthPen.HeaderText = "MonthPen";
            colMonthPen.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colMonthPen.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colMonthPen.DecimalLength = 2;
            colMonthPen.Name = "colMonthPen";
            colMonthPen.DataPropertyName = "MonthPen";
            this.dgvPayDet.Columns.Add(colMonthPen);
            this.dgvPayDet.Columns["colMonthPen"].Visible = false;

            udclsDGVNumericColumn.CNumEditDataGridViewColumn colOrgTDS = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            colOrgTDS.HeaderText = "OrgTDS";
            colOrgTDS.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colOrgTDS.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colOrgTDS.DecimalLength = 2;
            colOrgTDS.Name = "colOrgTDS";
            colOrgTDS.DataPropertyName = "OrgTDS";
            this.dgvPayDet.Columns.Add(colOrgTDS);
            this.dgvPayDet.Columns["colOrgTDS"].Visible = false; 
    
            
            /*<--- TDS Calciulation*/

            this.dgvPayDet.Columns["colId"].Visible = false;
            dgvPayDet.Columns["colSel"].DataPropertyName = "PayGenerated";
            dgvPayDet.Columns["colPrint"].DataPropertyName = "colPrint";
            dgvPayDet.Columns["colId"].DataPropertyName = "Id";
            dgvPayDet.Columns["colYear"].DataPropertyName = "Pay_Year";
            dgvPayDet.Columns["colcMonth"].DataPropertyName = "cMonth";
            dgvPayDet.Columns["colEmpCode"].DataPropertyName = "EmployeeCode";
            dgvPayDet.Columns["colEmp"].DataPropertyName = "EmployeeName";
            dgvPayDet.Columns["colSalPaidDays"].DataPropertyName = "SalPaidDays";
            dgvPayDet.Columns["colNetPayment"].DataPropertyName = "NetPayment";
            dgvPayDet.Columns["colGrossPayment"].DataPropertyName = "GrossPayment";

            dgvPayDet.Columns["colLocation"].DataPropertyName = "Loc_Desc";
            dgvPayDet.Columns["colNarr"].DataPropertyName = "Narr";

            this.dgvPayDet.Columns["colYear"].Visible = false;
            this.dgvPayDet.Columns["colcMonth"].Visible = false;
            this.dgvPayDet.Columns["colNarr"].Visible = false;
        }
       
       
        private void mthGrdRefresh()
        {
            
            
            this.mthBindData();

        }
        private void mthFldRefresh(int rInd)
        {
            if (dgvPayDet.Rows.Count == 0)
            {
                return;
            }
            if (dgvPayDet.Columns[0].Name == "id")
            {
                return;
            }
            if (dgvPayDet.Rows[rInd].Cells["colid"].Value == null)
            {
                return;
            }


            vId = dgvPayDet.Rows[rInd].Cells["colid"].Value.ToString();
            this.txtYear.Text = dgvPayDet.Rows[rInd].Cells["colYear"].Value.ToString();
            this.txtMonth.Text = dgvPayDet.Rows[rInd].Cells["colcMonth"].Value.ToString();
            this.txtLocNm.Text = dgvPayDet.Rows[rInd].Cells["colLocation"].Value.ToString();
            this.txtEmpCode.Text = dgvPayDet.Rows[rInd].Cells["colEmpCode"].Value.ToString();
            this.txtEmpNm.Text = dgvPayDet.Rows[rInd].Cells["colEmp"].Value.ToString();
        }
        
      
#endregion 
#region Data Filteration
        private void btnYear_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select distinct m.Pay_Year,y.sDate,y.eDate From Emp_Monthly_Muster m inner join Emp_Payroll_Year_Master y on (m.Pay_Year=y.Pay_Year) order by y.sDate desc";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Payroll Year";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Payroll Year,sDate:Start Date,eDate:End Date";
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

        }
        private void btnMonth_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            this.vgridcount = this.dgvPayDet.Rows.Count;//--commented by satish pal for-18215  dated 13/08/2013
             //if (this.pAddMode--commented by satish pal for  dated 13/08/2013
            if (this.pAddMode || this.vgridcount==0)//--Addedd by satish pal for bug-18215  dated 13/08/2013
            {
                SqlStr = "Select Distinct DateName( month , DateAdd( month , Pay_Month , 0 ) - 1 ) as colMnth,Pay_Month From Emp_Monthly_Muster Order by Pay_Month--Where  Pay_Year+cast(Pay_Month as varchar) not in (Select distinct Pay_Year+cast(Pay_Month as varchar) From Emp_Monthly_Payroll) ";
            }
            else
            {
                SqlStr = "Select Distinct DateName( month , DateAdd( month , Pay_Month , 0 ) - 1 ) as colMnth,Pay_Month From Emp_Monthly_Muster Where  Pay_Year+cast(Pay_Month as varchar) in (Select distinct Pay_Year+cast(Pay_Month as varchar) From Emp_Monthly_Payroll) Order by Pay_Month";
            }
            DataSet tDs = new DataSet();
            tDs=oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            //DataTable dt = new DataTable();
            //DataColumn colMnth = new DataColumn();

            //dt.Columns.Add(colMnth);
            //for (int i = 1; i <= 12; i++)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[0] = this.fnCMonth(i);
            //    dt.Rows.Add(dr);
            //}
            DataView dvw = new DataView();
            dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Month Name";
            vSearchCol = "colMnth";
            vDisplayColumnList = "colMnth:Month";
            vReturnCol = "colMnth";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtMonth.Text = oSelectPop.pReturnArray[0];
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    //this.mthGrdRefresh();
                }
            }
        }
        private void btnEmpNm_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select E.EmployeeName as EmpNm,E.EmployeeCode as EmpCode,E.Department,E.Designation,E.Category,E.Grade from EmployeeMast E";
            SqlStr = SqlStr + " Left Join Loc_Master L on (L.Loc_Code=E.Loc_Code)";
            SqlStr = SqlStr + " Left Join Department D on (D.Dept=E.Department)";
            SqlStr = SqlStr + " Left Join Category C on (C.Cate=E.Category)";
            SqlStr = SqlStr + " Where 1=1";
           

            SqlStr = SqlStr + " Union Select '' as EmpNm,'' as EmpCode,'' as Department,'' as Designation,'' as Category,'' as Grade";

            SqlStr = SqlStr + " order by EmployeeName";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Employee Name";
            vSearchCol = "EmpNm";
            vDisplayColumnList = "EmpNm:Employee Name,EmpCode:Employee Code,Department:Department,Designation:Designation,Category:Category";
            vReturnCol = "EmpNm,EmpCode";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtEmpNm.Text = oSelectPop.pReturnArray[0];
                this.txtEmpCode.Text = oSelectPop.pReturnArray[1];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                this.mthGrdRefresh();
            }
        }
      
      
        private void btnLocNm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEmpNm.Text.Trim()) == false)
            {
                MessageBox.Show("Employee Name is Selected ! Make it blank to Select the Location", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Loc_Desc as LocNm,Loc_Code,Add1,Add2,Add3,City as [Location Name] from Loc_master union select '' as LocNm,'' as Loc_Code,'' as Add1,'' as Add2,'' as Add3,'' as [Location Name] order by Loc_Desc";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Location Name";
            vSearchCol = "LocNm";
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
                vLoc_Code = oSelectPop.pReturnArray[1];

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh();
            }
        }
        private void btnDept_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select distinct E.Department from Loc_master L inner join EmployeeMast E on (E.Loc_Code=L.Loc_Code";
            if (this.txtLocNm.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and L.Loc_Code=" + "'" + vLoc_Code + "'";
            }
            SqlStr = SqlStr + ") union select '' as Department order by Department";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Department Name";
            vSearchCol = "Department";
            vDisplayColumnList = "Department:Department";
            vReturnCol = "Department";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtDept.Text = oSelectPop.pReturnArray[0].Trim();

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh();
            }

        }

        private void btnCate_Click(object sender, EventArgs e)
        {

            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select distinct E.Category from Loc_master L inner join EmployeeMast E on (E.Loc_Code=L.Loc_Code";
            if (this.txtLocNm.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and L.Loc_Code=" + "'" + vLoc_Code + "'";
            }
            SqlStr = SqlStr + ") union select '' as Category order by Category";
            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);


            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Category Name";
            vSearchCol = "Category";
            vDisplayColumnList = "Category:Category";
            vReturnCol = "Category";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCategory.Text = oSelectPop.pReturnArray[0].Trim();

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh();
            }
        }
        private void mthdvwFilt()
        {
            DataTable dtFilt = new DataTable();

            DataColumn ColFldCap = new DataColumn();
            ColFldCap.ColumnName = "ColFldCap";
            dtFilt.Columns.Add(ColFldCap);

            DataColumn ColFldNm = new DataColumn();
            ColFldNm.ColumnName = "ColFldNm";
            dtFilt.Columns.Add(ColFldNm);


            string vSFlds = string.Empty, vFldNm = string.Empty, vFldCap = string.Empty;
            DataSet tds = new DataSet();
            SqlStr = "Select SFlds=SearchFlds from MastCode where Code='EM'";
            tds = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            if (tds.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            int pos = 1;
            vSFlds = tds.Tables[0].Rows[0]["SFlds"].ToString();
            while (vSFlds.IndexOf("<<") > -1)
            {
                pos = vSFlds.IndexOf("<<");
                vSFlds = vSFlds.Substring(pos + 2, vSFlds.Length - pos - 2);
                pos = vSFlds.IndexOf(":");
                vFldNm = vSFlds.Substring(0, pos);

                vSFlds = vSFlds.Substring(pos + 1, vSFlds.Length - pos - 1);
                pos = vSFlds.IndexOf(">>");
                vFldCap = vSFlds.Substring(0, pos);
                if (vFldNm.IndexOf("EmployeeCode") < 0 && vFldNm.IndexOf("EmployeeName") < 0)
                {
                    DataRow drt = dtFilt.NewRow();
                    drt[0] = vFldCap;
                    drt[1] = vFldNm;
                    dtFilt.Rows.Add(drt);
                    vSFlds.Substring(pos + 1, vSFlds.Length - pos - 1);
                }
            }
            dvwFilt = dtFilt.DefaultView;


        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSelectAll.Checked == false)
            {
                if (dsPayDet.Tables.Count > 0)
                {
                    foreach (DataRow tdr in dsPayDet.Tables[0].Rows)
                    {
                        tdr["PayGenerated"] = false;
                    }
                }
                this.chkSelectAll.Text = "Select All";
            }
            else
            {
                this.chkSelectAll.Text = "De Select All";
                if (dsPayDet.Tables.Count > 0)
                {
                    foreach (DataRow tdr in dsPayDet.Tables[0].Rows)
                    {
                        tdr["PayGenerated"] = true;
                    }
                }
            }

        }
#endregion

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
        private void mthPrint(Int16  vPrintOption)
        {
        
            //this.dgvPayDet.CurrentRow.Cells[0].
            if (dgvPayDet.Rows.Count != 0)      /*Ramya 30/10/12*/
            {
                dgvPayDet.CurrentCell = dgvPayDet.Rows[0].Cells[0];
            }

            this.stbMain.Text = "";
            this.StbLbl.Text = "Generating Salary Slip....";
            this.stbMain.Refresh();
            this.StbLbl.Visible = true;
            this.stbMain.Refresh();
            
            string vPrintPara="";
            this.dgvPayDet.Refresh();
            this.btnYear.Focus();

            // Added for Bug-26255 on 03/06/2015 ("to check office email id ) start
            if (vPrintOption == 7)
            {
                string empname = string.Empty;
                for (int x = 0; x < dsPayDet.Tables[0].Rows.Count; x++)
                {
                    if (dsPayDet.Tables[0].Rows[x]["colPrint"].ToString().Trim() == "True" && dsPayDet.Tables[0].Rows[x]["emailoff"].ToString().Trim() == string.Empty)
                    {
                        empname = empname + "," + dsPayDet.Tables[0].Rows[x]["EmployeeName"].ToString().Trim();
                        this.dgvPayDet.Rows[x].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    }
                    else
                    {
                        this.dgvPayDet.Rows[x].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
                if (empname.Length > 2)
                {
                    DialogResult YesNo = MessageBox.Show("Highlited employees doesn't have the office email id, do you want to continue? ", "Empty Email ID", MessageBoxButtons.YesNo);
                    if (YesNo == DialogResult.Yes)
                    {
                        for (int y = 0; y < dsPayDet.Tables[0].Rows.Count; y++)
                        {

                            if (dsPayDet.Tables[0].Rows[y]["colPrint"].ToString().Trim() == "True" && dsPayDet.Tables[0].Rows[y]["emailoff"].ToString().Trim() == string.Empty)
                            {
                                dsPayDet.Tables[0].Rows[y]["colPrint"] = false;
                                this.dgvPayDet.Rows[y].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                    }

                    else
                    {
                        return;
                    }
                }
            }

            // Added for Bug-26255 on 03/06/2015 ("to check office email id ) end

            this.mthPrintPara(ref vPrintPara);
            string vRepGroup = "Monthly Payment";
            //MessageBox.Show(vPrintPara);
            if (vPrintPara == "")  /*Ramya 09/10/12*/
            {
                MessageBox.Show("Please select atleast one employee to print", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }                       /*Ramya 09/10/12*/
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
            //oPrint.pTran_Cd = Convert.ToInt16(this.vTran_Cd);
           // oPrint.pSpPara = vPrintPara;
            oPrint.pSpPara = vPrintPara + ",'" + this.pAppUerName.Trim() + "'"; /*Ramya 01/11/12*/
            oPrint.pPrintOption = vPrintOption;
            oPrint.Main();

            this.StbLbl.Text = "Salary Slip Generated";

            if (vPrintOption != 2)
            {
                timer1.Enabled = true; /*Ramya 01/11/12*/
                timer1.Interval = 1000;
                MessageBox.Show("Salary Slip Generated", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }
            this.stbMain.Refresh();
            this.StbLbl.Text = "";
            this.stbMain.Refresh();
        }
        private void mthPrintPara(ref string vPrintPara)
        {
           
            vPrintPara = "";
           // dsPayDet.
            foreach (DataRow drt in dsPayDet.Tables[0].Rows)
            {
                if ((Boolean)drt["ColPrint"])
                {
                    vPrintPara = vPrintPara + ",''" + drt["EmployeeCode"].ToString().Trim() + "''";
                }

            }
            if (vPrintPara.Length > 1)
            {
                vPrintPara = vPrintPara.Substring(1, vPrintPara.Length - 1);
                vPrintPara = " 'EmployeeCode in (" + vPrintPara + ")'";
                vPrintPara = dsMain.Tables[0].Rows[0]["Tran_Cd"].ToString() + "," + vPrintPara;
                //vPrintPara = " '" + this.txtYear.Text.Trim() + "'," + vPrintPara;
            }

        }
        private void dgvPayDet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int cInd = e.ColumnIndex;
            string vColName = this.dgvPayDet.Columns[e.ColumnIndex].Name;
            string vColVal = "";
            if (vColName == "coliNarr" && dgvPayDet.CurrentRow!=null )
            {
                if (dgvPayDet.CurrentRow.Cells["colNarr"].Value != null)
                {
                    vColVal = dgvPayDet.CurrentRow.Cells["colNarr"].Value.ToString();
                }

                //if (dgvPayDet.CurrentRow.Cells["coliNarr"].Value != null)
                //{
                //    vColVal = dgvPayDet.CurrentRow.Cells["coliNarr"].Value.ToString();
                //}
                //MessageBox.Show(vColVal);
                udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
                oudTextEdit.pAddMode = this.pAddMode;
                oudTextEdit.pEditMode = this.pEditMode;
                oudTextEdit.pICon = this.Icon;
                oudTextEdit.pTextVal = vColVal;
                oudTextEdit.pFrmCaption = "Narration Details ";
                oudTextEdit.mthCallTextEdit();
                vColVal = oudTextEdit.pTextVal;
                dgvPayDet.CurrentRow.Cells["colNarr"].Value = vColVal;

             // dgvPayDet.CurrentRow.Cells["coliNarr"].Value = vColVal;
                
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
            if (btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnLogout.Enabled)
                btnLogout_Click(this.btnExit, e);

        }

        private void frmMonthlyPayroll_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord(); /*Ramya 27/10/12*/
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

        private void txtMonth_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnMonth_Click(sender, new EventArgs());
                }
            }
        }

        private void txtLocNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnLocNm_Click(sender, new EventArgs());
                }
            }
        }

        private void txtDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnDept_Click(sender, new EventArgs());
                }
            }
        }

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnCate_Click(sender, new EventArgs());
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
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

        private void mthchkdata()
        {
            //Added by Archana K. on 21/09/13 for Bug-18246 
               if (dsPayDet.Tables.Count > 0)
                {
                    for (int i = 0; i < dsPayDet.Tables[0].Rows.Count; i++)
                    {
                        if(pEditButton)
                        {
                            DataTable temp = new DataTable();
                            temp.TableName = "temp";
                            SqlStr = "Select a.tot_amt,b.fld_NM from emp_loan_advance_details a inner join Emp_Loan_Advance b on (a.Tran_cd=b.Tran_cd)  where a.employeecode='" + dsPayDet.Tables[0].Rows[i]["Employeecode"] + "' and a.pay_month=" + dsPayDet.Tables[0].Rows[i]["Pay_month"];
                            temp = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                            foreach (DataRow row in temp.Rows)
                            {
                                if (Convert.ToInt32(dsPayDet.Tables[0].Rows[i]["Loanamt"]) == 0 && row["fld_NM"].ToString().ToUpper() == "LOANAMT")
                                {
                                   dsPayDet.Tables[0].Rows[i]["Loanamt"] = row["tot_amt"];
                                }
                                else if (Convert.ToInt32(dsPayDet.Tables[0].Rows[i]["Advamt"]) == 0 && row["fld_NM"].ToString().ToUpper() == "ADVAMT")
                                {
                                    dsPayDet.Tables[0].Rows[i]["Advamt"] = row["tot_amt"];
                                }
                            }
                        }
                        DataRow rtemp = temtbl.NewRow();
                        rtemp["Employeecode"] = dsPayDet.Tables[0].Rows[i]["Employeecode"];
                        rtemp["Pay_month"] = dsPayDet.Tables[0].Rows[i]["Pay_month"];
                        rtemp["Loanamt"] = dsPayDet.Tables[0].Rows[i]["Loanamt"];
                        rtemp["Advamt"] = dsPayDet.Tables[0].Rows[i]["Advamt"];
                        temtbl.Rows.Add(rtemp);
                    }
                }
        }

        //******** Added by Sachin N. S. on 24/06/2014 for Bug-21114 -- Start
        private void btnCalcPeriod_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable _dt = new DataTable();
            _dt.Columns.Add(new DataColumn("CalcPeriod", typeof(string)));

            DataRow _dr;
            _dr = _dt.NewRow();
            _dr[0] = "";
            _dt.Rows.Add(_dr);

            _dr = _dt.NewRow();
            _dr[0] = "MONTHWISE";
            _dt.Rows.Add(_dr);

            _dr = _dt.NewRow();
            _dr[0] = "DAYWISE";
            _dt.Rows.Add(_dr);

            _dr = _dt.NewRow();
            _dr[0] = "HOURWISE";
            _dt.Rows.Add(_dr);

            DataView dvw = _dt.DefaultView;

            VForText = "Select Calculation Period";
            vSearchCol = "CalcPeriod";
            vDisplayColumnList = "CalcPeriod:Calculation Period";
            vReturnCol = "CalcPeriod";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtCalcPeriod.Text = oSelectPop.pReturnArray[0].Trim();

            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.mthGrdRefresh();
            }
        }

        private void txtCalcPeriod_KeyDown(object sender, KeyEventArgs e)
        {
            if (pAddMode == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnCalcPeriod_Click(sender, new EventArgs());
                }
            }
        }
        //******** Added by Sachin N. S. on 24/06/2014 for Bug-21114 -- End

    }
}
