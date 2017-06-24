using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DataAccess_Net;
using uBaseForm;
using udSelectPop;
using System.IO;
using System.Diagnostics;//Rup
using System.Collections;
using System.Globalization;
using System.Threading;
using uNumericTextBox;

namespace ueItemRateUpdtae
{
    public partial class frmItemDetailsMain : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string strcol;
        String cAppPId, cAppName;
        public frmItemDetailsMain(string[] args)
        {
            InitializeComponent();

            //this.pFrmCaption = "Item Master Rate Details";
            this.pFrmCaption = "Goods Master Rate Details"; //changed by Ruchit on 01/04/2017
            this.Text = this.pFrmCaption; //Rup
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            if (args[5] != "")
            {
                this.pPApplRange = args[5].ToString().Replace("^", "");
            }
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();             
      
        }

        private void frmItemDetailsMain_Load(object sender, EventArgs e)
         {  
             //CultureInfo ci = new CultureInfo("en-US");          /*Ramya 24/02/12*/
             //ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
             //Thread.CurrentThread.CurrentCulture = ci;

             //oForm = new DynamicFormClass.clsDynamicForm(); //Rup

             string _SqlDefaultDateFormate = mGetSQLServerDefaDateFormate();
             CultureInfo ci = new CultureInfo("en-US");
             //ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
             switch (_SqlDefaultDateFormate)
             {
                 case "mdy":
                     ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
                     break;
                 case "dmy":
                     ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                     break;
                 case "ymd":
                     ci.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                     break;
             }
             Thread.CurrentThread.CurrentCulture = ci;

             this.txtSalesRate.pAllowNegative = false;  /*Ramya*/
             this.txtSalesRate.MaxLength = 9;
             this.txtSalesRate.pDecimalLength = 2;



             this.txtPurRate.pAllowNegative = false;
             this.txtPurRate.MaxLength = 9;
             this.txtPurRate.pDecimalLength = 2;



             this.txtAbtPer.pAllowNegative = false;
             this.txtAbtPer.MaxLength = 9;
             this.txtAbtPer.pDecimalLength = 2;



             this.txtMrp.pAllowNegative = false;
             this.txtMrp.MaxLength = 13;
             this.txtMrp.pDecimalLength = 2;
             /*Ramya*/

             this.mInsertProcessIdRecord(); //Rup
             this.SetFormColor();//Rup

             string appPath = Application.ExecutablePath;
             appPath = Path.GetDirectoryName(appPath);
             string fName = appPath + @"\bmp\pickup.gif"; //Rup
             if (File.Exists(fName) == true)
             {
                 this.btnSearch.Image = Image.FromFile(fName);
             }
             fName = appPath + @"\bmp\save.gif";
             if (File.Exists(fName) == true)
             {
                 this.btnUpdate.Image = Image.FromFile(fName);
                 this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
             }
             fName = appPath + @"\bmp\close2.gif";
             if (File.Exists(fName) == true)
             {
                 this.btnClose.Image = Image.FromFile(fName);
                 this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
             }

            txtPurRate.Text = Convert.ToString(0);
            txtSalesRate.Text = Convert.ToString(0);
            txtAbtPer.Text = Convert.ToString(0);
            txtMrp.Text = Convert.ToString(0);
            txtPurRate.ReadOnly = true;
            txtSalesRate.ReadOnly = true;
            txtAbtPer.ReadOnly = true;
            txtMrp.ReadOnly = true;
            //strcol = "','[Select]=cast(0 as bit), It_Name as [Item Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
            strcol = "','[Select]=cast(0 as bit), It_Name as [Goods Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''"; //changed by Ruchit
            this.mItGroupGetPop();
            this.SetMenuRights();//Rup
            //this.MinimizeBox = false;
            //this.MaximizeBox = false;
            
            // this.ShowIcon = true;

        }

        private string mGetSQLServerDefaDateFormate()//<--- Added By Amrendra On 21/01/2012 
        {
            return oDataAccess.GetDataTable("select [dateformat] from master..syslanguages where langid=(select [value] from sys.configurations where [name]='default language')", null, 20).Rows[0][0].ToString();
        }
        private void SetMenuRights() //Rup
        {
            DataSet dsMenu=new DataSet();
            DataSet dsRights = new DataSet();
            string strSQL = "select padname,barname,range from com_menu where range ="+this.pPApplRange;
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
                        strSQL += "userrights where padname ='"+padName.Trim()+"' and barname ='"+barName+"' and range = "+this.pPApplRange;
                        strSQL += "and dbo.func_decoder([user],'T') ='"+this.pAppUerName.Trim()+"'" ;

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
                btnUpdate.Enabled = (rArray[1].ToString().Trim() == "CY" ? true : false);
                //btnNew.Enabled = (myArray[0].ToString().Trim() == "IY" ? true : false);
                //btnEdit.Enabled = (myArray[1].ToString().Trim() == "CY" ? true : false);
                ////menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                //btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ueItemRateUpdate.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";   //Set DateFormat dmy
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
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

        private void mItGroupGetPop()
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet dstemp = new DataSet();
            strSQL = "select  distinct [group] as GrpName from it_mast where [group]<>''";
            dstemp = oDataAccess.GetDataSet(strSQL, null, 20);
            DataView dvw = dstemp.Tables[0].DefaultView;
            if (dvw.Count > 0)
            {
                this.txtItGroup.Text = dvw.Table.Rows[0]["GrpName"].ToString().Trim();
            }
            this.mthRefreshGrid(strcol);
        }

        private void mthRefreshGrid(string str)
        {
            //string sqlstr = "execute Usp_Uerport_ItemFoundFromGroup '" + txtItGroup.Text.Trim() + str;
            string sqlstr = "execute Usp_Uerport_ItemFoundFromGroup '" + txtItGroup.Text.Trim() + str + ",'isService = 0'"; //added by Ruchit on 05/04/2017
            DataSet dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
            dsMain.Tables[0].Columns[1].ReadOnly = true;
            dsMain.Tables[0].Columns[2].ReadOnly = true;
            dsMain.Tables[0].Columns[3].ReadOnly = true;
            dsMain.Tables[0].Columns[4].ReadOnly = true;
            dsMain.Tables[0].Columns[5].ReadOnly = true;
            dataGridView1.DataSource = dsMain.Tables[0];
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 280;
            dataGridView1.Columns[3].Width = 120;
          
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();//Rup
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet dstemp = new DataSet();
            strSQL = "select  distinct [group] as GrpName from it_mast ";
            dstemp = oDataAccess.GetDataSet(strSQL, null, 20);
            DataView dvw = dstemp.Tables[0].DefaultView;

            //VForText = "Select Item Group Name";
            VForText = "Select Goods Group Name"; //Changed by Ruchit
            vSearchCol = "GrpName";
            vDisplayColumnList = "GrpName:Group Name";
            vReturnCol = "GrpName";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtItGroup.Text = oSelectPop.pReturnArray[0];
            }
            this.mthRefreshGrid(strcol);
        }

        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelect.Checked == true)
            {
                lblSelect.Text = "Select None";
                //strcol = "','[Select]=cast(1 as bit), It_Name as [Item Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
                strcol = "','[Select]=cast(1 as bit), It_Name as [Goods Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
                mthRefreshGrid(strcol);

            }
            else if (chkSelect.Checked == false)
            {
                lblSelect.Text = "Select All";
                //strcol = "','[Select]=cast(0 as bit), It_Name as [Item Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
                strcol = "','[Select]=cast(0 as bit), It_Name as [Goods Name],Rate as [Sales Rate],curr_cost as [Purchase Rate],abtper as [Abatement%],mrprate as [MRP Rate]',''";
                mthRefreshGrid(strcol);
            }

        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); //Rup
            string str1;
            int j = 0;
            DataTable dttab = (DataTable)dataGridView1.DataSource;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    
                    string selval = dttab.Rows[i][0].ToString();
                    if (selval == "True")
                        j++;

                }
                if (j <= 0)
                {
                    //MessageBox.Show("Please Select Any Item To Update...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Please Select Any Goods To Update...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
          
                    string selval = dttab.Rows[i][0].ToString();
                    string itname = dttab.Rows[i][1].ToString();
                    //int purrate = Convert.ToInt32(txtPurRate.Text.Trim());
                        
                    if (selval == "True")
                    {
                        str1 = "";
                        if (chkPurRate.Checked == true)
                        {
                            str1 = " curr_cost=" + txtPurRate.Text.Trim();
                        
                        }
                        if (chkSalesRate.Checked == true)
                        {
                            if (string.IsNullOrEmpty(str1) == false)
                            {
                                str1 = str1 + ",";
                            }
                            str1 = str1 + " rate=" + txtSalesRate.Text.Trim();
                            
                           
                        }
                        if (chkAbt.Checked == true)
                        {
                            if (string.IsNullOrEmpty(str1) == false)
                            {
                                str1 = str1 + ",";
                            }
                            str1 = str1 + " abtper=" + txtAbtPer.Text.Trim();
                        }
                        if (chkMrp.Checked == true)
                        {
                            if (string.IsNullOrEmpty(str1) == false)
                            {
                                str1 = str1 + ",";
                            }
                            str1 = str1 + " mrprate=" + txtMrp.Text.Trim();
                        }
                        if ((chkPurRate.Checked == false) && (chkSalesRate.Checked == false) && (chkAbt.Checked == false) && (chkMrp.Checked == false))
                        {
                            MessageBox.Show("Please Select Any Rate To Update" ,this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if ((txtSalesRate.Text == "") || (txtPurRate.Text == "") || (txtAbtPer.Text == "") || (txtMrp.Text == ""))
                        {
                            MessageBox.Show("Please Enter Some Value For Selected Rate", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        
                        string str2 = ", [user_name]= '" + pAppUerName + "' , [sysdate]= '" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "'";
                        str1 = " Update It_Mast set " + str1 + str2;
                        str1 = str1 + " where   [It_name]='" + itname + "'";
                        oDataAccess.ExecuteSQLStatement(str1, null, 20, true);
                       
                        
                        
                    }
                    
                }
                mthRefreshGrid(strcol);
                chkSalesRate.Checked = false;
                chkPurRate.Checked = false;
                chkAbt.Checked = false;
                chkMrp.Checked = false;
                chkSelect.Checked = false;
                MessageBox.Show("Updated Successfully", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);

                
            }
          
       

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Rup
        }

        private void chkSalesRate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesRate.Checked == true)
            {
                txtSalesRate.ReadOnly = false;
                txtSalesRate.Text = "";
                txtSalesRate.Focus();
            
            }
            else
            {
                txtSalesRate.Text = Convert.ToString(0);
                txtSalesRate.ReadOnly = true;
            }
        }

        private void chkPurRate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPurRate.Checked == true)
            {
                txtPurRate.ReadOnly = false;
                txtPurRate.Text = "";
                txtPurRate.Focus();

            }
            else
            {
                txtPurRate.Text = Convert.ToString(0);
                txtPurRate.ReadOnly = true;
            }

        }

        private void chkAbt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAbt.Checked == true)
            {
                txtAbtPer.ReadOnly = false;
                txtAbtPer.Text = "";
                txtAbtPer.Focus();
            }
            else
            {
                txtAbtPer.Text = Convert.ToString(0);
                txtAbtPer.ReadOnly = true;
            }

        }

        private void chkMrp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMrp.Checked == true)
            {
                txtMrp.ReadOnly = false;
                txtMrp.Text = "";
                txtMrp.Focus();
                 
            }
            else
            {
                txtMrp.Text = Convert.ToString(0);
                txtMrp.ReadOnly = true;
            }

        }

        private void txtSalesRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;

            }
                    
        }

        private void txtPurRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;

            }
        }

        private void txtAbtPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;

            }

        }

        private void txtMrp_KeyPress(object sender, KeyPressEventArgs e)
        {
            string num = "0123456789.";
            if (num.IndexOf(e.KeyChar) == -1)
            {
                e.Handled = true;

            }
        }

        private void frmItemDetailsMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();//Rup
        }

        private void SetFormColor() //Rup
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid ="+this.pCompId;
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

        private void frmItemDetailsMain_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord(); /*Ramya 24/02/2012*/
        }

      

     

        

    }
}

