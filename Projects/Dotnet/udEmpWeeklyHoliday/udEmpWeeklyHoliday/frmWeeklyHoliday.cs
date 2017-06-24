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
using ueconnect;//Added by Archana K. on 16/05/13 for Bug-7899
using GetInfo;//Added by Archana K. on 16/05/13 for Bug-7899

namespace udEmpWeeklyHoliday
{
    public partial class frmWeeklyHoliday : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;

        public string[] args1;
        DataSet dsMain;
        //DataSet tds, tdsLoc;
        DataSet dsGrd = new DataSet();
        string SqlStr = string.Empty;
        string StrCol = string.Empty;
       // string vMainField = "", 
        string vMainTblNm = "Employeemast";
        string vExpression = string.Empty;

        String cAppPId, cAppName;
        string vYear = string.Empty, vLocNm = string.Empty, vDept = string.Empty, vCate = string.Empty, vId = string.Empty;
        string weeekoff1 = "WWWWWWW", weeekoff2 = "WWWWWWW", weeekoff3 = "WWWWWWW", weeekoff4 = "WWWWWWW", weeekoff5 = "WWWWWWW", weeekoff6 = "WWWWWWW";
        //int vId;
        //bool cValid;
       // bool IsSaved = true;
       // bool vValid = true;
        string appPath;
       // int cancelVal = 0;
        string fweeekval;
        int rindex,cindex;
        //Added by Archana K. on 17/05/13 for Bug-7899 start
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        public string ServiceType = string.Empty;
        //Added by Archana K. on 17/05/13 for Bug-7899 end

        public frmWeeklyHoliday(string[] args)
        {
           // InitializeComponent();
            this.pDisableCloseBtn = true;  /* close disable */
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Weekly Holiday Master";
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
            args1 = args;

        }

        private void frmWeeklyHoliday_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");   /*Ramya*/
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.txtCategory.Enabled = false;
            this.txtDept.Enabled = false;
            this.txtDesigName.Enabled = false;
            this.txtGradeName.Enabled = false;
            this.txtLocNm.Enabled = false;

            this.groupBox1.Enabled = false;
            //Added by Archana K. on 16/05/13 for Bug-7899 start
            //startupPath = "E:\\Vudyog Usquare";
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
                //this.btnYear.Image = Image.FromFile(fName);
                //this.btnDesc.Image = Image.FromFile(fName);
               
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnCate.Image = Image.FromFile(fName);
                this.btnDept.Image = Image.FromFile(fName);
                this.btndesig.Image = Image.FromFile(fName);
                this.btnGrade.Image = Image.FromFile(fName);

            }


            StrCol = "Select [Select]=cast(0 as bit),EmployeeName,FirstWeekWO,SecondWeekWO,ThirdWeekWO,FourthWeekWO,FifthWeekWO,SixthWeekWO,EmployeeCode,Department,Designation,Category,Grade,Loc_Master.Loc_Desc as Location from employeemast Left Join Loc_Master on(employeemast.Loc_code=Loc_Master.Loc_Code)";
            //dsMain = oDataAccess.GetDataSet(StrCol, null, 20);
            Binddata(StrCol);
             if (pEditButton == true)
            {
                this.btnUpdate.Enabled = true;
            }
            else
            {
                this.btnUpdate.Enabled = false;
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
                //Added by Archana K. on 17/05/13 for Bug-7899 start
                if (ServiceType.ToUpper() == "VIEWER VERSION")
                {
                    this.pAddButton = false;
                    this.pEditButton = false;
                    this.pDeleteButton = false;
                    this.pPrintButton = false;
                    this.btnBulkupdate.Enabled = false;
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Columns["btnUpdate"].Visible = false;
                    }
                }
                else
                {
                    //Added by Archana K. on 17/05/13 for Bug-7899 end
                    this.pAddButton = (rArray[0].ToString().Trim() == "IY" ? true : false);
                    this.pEditButton = (rArray[1].ToString().Trim() == "CY" ? true : false);
                    this.pDeleteButton = (rArray[2].ToString().Trim() == "DY" ? true : false);
                    this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);
                }
            }//Added by Archana K. on 17/05/13 for Bug-7899
        }

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udEmpWeeklyHoliday.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set Dateformat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            try
            {
                oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
            }
            catch (Exception)
            { }



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

        private void Binddata(string  str)
        {
            dsMain = oDataAccess.GetDataSet(str, null, 20);
            dataGridView1.DataSource = dsMain.Tables[0];
            //DataGridViewCheckBoxColumn chSelect = new DataGridViewCheckBoxColumn();
            // dataGridView1.Columns.Add(chSelect);
            if (dsMain.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("No Employee Details Found",this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                return;
            }
                
           
            DataGridViewButtonColumn bt1 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(bt1);
            bt1.HeaderText = "First Week";
            bt1.Name = "FirstWeek";
            bt1.Width = 100;
            bt1.Text = "First Week";
            bt1.UseColumnTextForButtonValue = true;
            // bt1.CEL                  

            DataGridViewButtonColumn bt2 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(bt2);
            bt2.HeaderText = "Second Week";
            bt2.Name = "SecondWeek";
            bt2.Width = 100;
            bt2.Text = "Second Week";
            bt2.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn bt3 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(bt3);
            bt3.HeaderText = "Third Week";
            bt3.Name = "ThirdWeek";
            bt3.Width = 100;
            bt3.Text = "Third Week";
            bt3.UseColumnTextForButtonValue = true;


            DataGridViewButtonColumn bt4 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(bt4);
            bt4.HeaderText = "Fourth Week";
            bt4.Name = "FourthWeek";
            bt4.Width = 100;
            bt4.Text = "Fourth Week";
            bt4.UseColumnTextForButtonValue = true;


            DataGridViewButtonColumn bt5 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(bt5);
            bt5.HeaderText = "Fifth Week";
            bt5.Name = "FifthWeek";
            bt5.Width = 100;
            bt5.Text = "Fifth Week";
            bt5.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn bt6 = new DataGridViewButtonColumn();
            bt6.HeaderText = "Sixth Week";
            bt6.Name = "SixthWeek";
            bt6.Width = 100;
            bt6.Text = "Sixth Week";
            bt6.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(bt6);

            DataGridViewButtonColumn bt7 = new DataGridViewButtonColumn();
            bt7.HeaderText = "";
            bt7.DefaultCellStyle.ForeColor = Color.Blue;
            bt7.Name = "btnUpdate";
            bt7.Width = 200;
            bt7.Text = "        Update        ";
            bt7.UseColumnTextForButtonValue = true;
            
            
            dataGridView1.Columns.Add(bt7);


            dataGridView1.Columns["FirstWeekWO"].Visible = false;
            dataGridView1.Columns["SecondWeekWO"].Visible = false;
            dataGridView1.Columns["ThirdWeekWO"].Visible = false;
            dataGridView1.Columns["FourthWeekWO"].Visible = false;
            dataGridView1.Columns["FifthWeekWO"].Visible = false;
            dataGridView1.Columns["SixthWeekWO"].Visible = false;
            //dataGridView1.Columns["btnUpdate"].Visible = false;
            //Added by Archana K. on 17/05/13 for Bug-7899 start
            if (ServiceType.ToUpper() == "VIEWER VERSION")
                dataGridView1.Columns["btnUpdate"].Visible = false;
            else
                dataGridView1.Columns["btnUpdate"].Visible = true;

            //Added by Archana K. on 17/05/13 for Bug-7899 end   
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
          //  dataGridView1.Sort(sortedColumn, System.ComponentModel.ListSortDirection.Ascending);
            if (dataGridView1.Rows.Count > 0)
            {
                this.mthFldRefresh(0);
            }
            
            //dataGridView1.Columns["First
        }

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    MessageBox.Show("In Cell Click");

        //}

        //private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    MessageBox.Show(dataGridView1.CurrentCell.Value.ToString());
        //}

     

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CausesValidation = false;
            rindex = e.RowIndex;
            cindex = e.ColumnIndex;
            string vEmployeeCode = "";
            if (rindex < 0)
            {
                return;
            }
            //int cidx = dataGridView1.CurrentCell.ColumnIndex;
            if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                if ((Boolean)dataGridView1.CurrentCell.Value==false)
                {
                    //dataGridView1.CurrentCell.Value.ToString() == "True";
                    dataGridView1.CurrentCell.Value = 1;

                }
                else
                {
                    dataGridView1.CurrentCell.Value = 0;

                }
            }
            int j = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            { 
                if(dataGridView1.Rows[i].Cells["Select"].Value.ToString()=="True")
                {
                    j++;
                }
                if(j>1)
                {
                    groupBox1.Enabled=true;
                }
                else
                {
                    groupBox1.Enabled=false;
                }


            }
           // MessageBox.Show(dataGridView1.CurrentCell.Value.ToString());
           // MessageBox.Show(e.ColumnIndex.ToString());
            if (e.ColumnIndex >= 13)
            {
                string colname=dataGridView1.Columns[cindex].Name.ToString();
                if (colname == "btnUpdate")   
                {
                    if (dataGridView1.Rows[rindex].Cells["Select"].Value.ToString() == "True")
                    {
                        //MessageBox.Show(colname);
                        if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Employeecode"].Value != null)
                        {
                            vEmployeeCode = dataGridView1.Rows[rindex].Cells["Employeecode"].Value.ToString().Trim();
                        }
                        if (string.IsNullOrEmpty(vEmployeeCode) == false)
                        {
                            SqlStr = " set dateformat dmy Update " + vMainTblNm + " set FirstWeekWO= '" + dataGridView1.Rows[rindex].Cells["FirstWeekWO"].Value.ToString() + "'";
                            SqlStr = SqlStr + ",SecondWeekWO='" + dataGridView1.Rows[rindex].Cells["SecondWeekWO"].Value.ToString() + "'";
                            SqlStr = SqlStr + ",ThirdWeekWO='" + dataGridView1.Rows[rindex].Cells["ThirdWeekWO"].Value.ToString() + "'";
                            SqlStr = SqlStr + ",FourthWeekWO='" + dataGridView1.Rows[rindex].Cells["FourthWeekWO"].Value.ToString() + "'";
                            SqlStr = SqlStr + ",FifthWeekWO='" + dataGridView1.Rows[rindex].Cells["FifthWeekWO"].Value.ToString() + "'";
                            SqlStr = SqlStr + ",SixthWeekWO='" + dataGridView1.Rows[rindex].Cells["SixthWeekWO"].Value.ToString() + "'";
                            SqlStr = SqlStr + " where Employeecode='" + dataGridView1.Rows[rindex].Cells["Employeecode"].Value.ToString().Trim() + "'";
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                            MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.GridRefresh();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select the Employee First", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        
                    }
                    return;
                }      
                
                
                

               
                
              
            //{

                if (dataGridView1.Rows[rindex].Cells["Select"].Value.ToString() == "True")
                {

                   // string weekVal = gridView1.GetRowCellValue(rowindex, colname.Replace(" ", "") + "Val").ToString();
                    string weekval = dataGridView1.Rows[rindex].Cells[colname.Replace(" ", "") + "WO"].Value.ToString();
                    showform(ref weekval);
                    dataGridView1.Rows[rindex].Cells[colname.Replace(" ", "") + "WO"].Value= fweeekval;


                }
                else
                {
                    MessageBox.Show("Please Select the Employee First", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void showform(ref string val)
        {
            //frmWeekoff frmWoff = new frmWeekoff(args1);//Commented by Archana K. on 21/05/13 for Bug-7899
            frmWeekoff frmWoff = new frmWeekoff(args1, ServiceType);//Changed by Archana K. on 21/05/13 for Bug-7899
            frmWoff.CweekVal = val;
            frmWoff.ShowDialog();
            if (frmWoff.Cweekoff != null)
            {
                fweeekval = frmWoff.Cweekoff;
                val = fweeekval;
            }
            //if(frmWoff.

        }


        private void groupBox1_EnabledChanged(object sender, EventArgs e)
        {
            if (groupBox1.Enabled)
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                if (pEditButton == true)
                {
                    btnUpdate.Enabled = true;
                }
                else
                {
                    btnUpdate.Enabled = false;
                }
            }
        }

        private void btn1week_Click(object sender, EventArgs e)
        {

            showform(ref weeekoff1);
            //showform(weekval);
           
           // dataGridView1.Rows[rindex].Cells[colname.Replace(" ", "") + "Val"].Value = fweeekval;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["FirstWeekWO"].Value = fweeekval;

                }
            }
        }

        private void showform()
        {
           // throw new NotImplementedException();
        }

        private void btn2week_Click(object sender, EventArgs e)
        {
            showform(ref weeekoff2 );
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["SecondWeekWO"].Value = fweeekval;

                }
            }

        }

        private void btn3week_Click(object sender, EventArgs e)
        {
            showform(ref weeekoff3 );
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["ThirdWeekWO"].Value = fweeekval;

                }
            }
        }

        private void btn4week_Click(object sender, EventArgs e)
        {
            showform(ref weeekoff4 );
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["FourthWeekWO"].Value = fweeekval;

                }
            }
        }

        private void btn5week_Click(object sender, EventArgs e)
        {

            showform(ref weeekoff5);
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["FifthWeekWO"].Value = fweeekval;

                }
            }
        }

        private void btn6Week_Click(object sender, EventArgs e)
        {
            showform(ref weeekoff6);
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    dataGridView1.Rows[i].Cells["SixthWeekWO"].Value = fweeekval;

                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); 

            int k = 0;
          
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells["Select"].Value.ToString() == "True")
                {
                    k++;
                    SqlStr = " set dateformat dmy Update " + vMainTblNm + " set FirstWeekWO= '" + dataGridView1.Rows[i].Cells["FirstWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",SecondWeekWO='" + dataGridView1.Rows[i].Cells["SecondWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",ThirdWeekWO='" + dataGridView1.Rows[i].Cells["ThirdWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",FourthWeekWO='" + dataGridView1.Rows[i].Cells["FourthWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",FifthWeekWO='" + dataGridView1.Rows[i].Cells["FifthWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",SixthWeekWO='" + dataGridView1.Rows[i].Cells["SixthWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + " where Employeecode='" + dataGridView1.Rows[i].Cells["Employeecode"].Value.ToString().Trim() + "'";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                }
                //if (k >= 1)           
                //{
                //    //SqlStr = "Update " + vMainTblNm + " set FirstWeekVal= '" + dataGridView1.Rows[i].Cells["FirstWeekVal"].Value.ToString() + "'";
                //    //SqlStr = SqlStr + ",SecondWeekVal='" + daaGridView1.Rows[i].Cells["SecondWeekVal"].Value.ToString() + "'";
                //    //SqlStr = SqlStr + ",ThirdWeekVal='" + dataGridView1.Rows[i].Cells["ThirdWeekVal"].Value.ToString() + "'";
                //    //SqlStr = SqlStr + ",FourthWeekVal='" + dataGridView1.Rows[i].Cells["FourthWeekVal"].Value.ToString() + "'";
                //    //SqlStr = SqlStr + ",FifthWeekVal='" + dataGridView1.Rows[i].Cells["FifthWeekVal"].Value.ToString() + "'";

                //}
                //else
                //{
                //    MessageBox.Show("Please select atleast one employee to update");
                //    return;
                //}

            }
            if(k>=1)
            {
                weeekoff1 = "WWWWWWW"; weeekoff2 = "WWWWWWW"; weeekoff3 = "WWWWWWW"; weeekoff4 = "WWWWWWW"; weeekoff5 = "WWWWWWW";
                MessageBox.Show("Entry Saved", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                GridRefresh();
            }
            else
            {
                MessageBox.Show("Please select atleast one employee to update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
        }

        private void btnBulkupdate_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();

            if (MessageBox.Show("Do you want to change the setting ", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    SqlStr = "set dateformat dmy Update " + vMainTblNm + " set FirstWeekWO= '" + dataGridView1.Rows[i].Cells["FirstWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",SecondWeekWO='" + dataGridView1.Rows[i].Cells["SecondWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",ThirdWeekWO='" + dataGridView1.Rows[i].Cells["ThirdWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",FourthWeekWO='" + dataGridView1.Rows[i].Cells["FourthWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",FifthWeekWO='" + dataGridView1.Rows[i].Cells["FifthWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + ",SixthWeekWO='" + dataGridView1.Rows[i].Cells["SixthWeekWO"].Value.ToString() + "'";
                    SqlStr = SqlStr + " where Employeecode='" + dataGridView1.Rows[i].Cells["Employeecode"].Value.ToString() + "'";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);

                }
                weeekoff1 = "WWWWWWW"; weeekoff2 = "WWWWWWW"; weeekoff3 = "WWWWWWW"; weeekoff4 = "WWWWWWW"; weeekoff5 = "WWWWWWW";
                MessageBox.Show("Entry Saved");
                GridRefresh();
            }
        }
        private void GridRefresh()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells["Select"].Value = 0;

            }
            this.chkSelect.Checked = false;
            this.dataGridView1.RefreshEdit();       //Added by Shrikant S. on 11/09/2014 for auto updater 11.0.8
        }

        private void mthFldRefresh(int rInd)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            //if (dataGridView1.Rows[rInd].Cells["Employeecode"].Value==null)
            //{
            //    return;
            //}

            this.txtLocNm.Text = dataGridView1.Rows[rInd].Cells["Location"].Value.ToString();
            this.txtDept.Text = dataGridView1.Rows[rInd].Cells["Department"].Value.ToString();
            this.txtCategory.Text = dataGridView1.Rows[rInd].Cells["Category"].Value.ToString();
            this.txtGradeName.Text = dataGridView1.Rows[rInd].Cells["Grade"].Value.ToString();
            this.txtDesigName.Text = dataGridView1.Rows[rInd].Cells["Designation"].Value.ToString();

                 // { colId, colYear, ColsDate, ColeDate, colDays,colDesc, colLocation, colDept, colCate});
            //MessageBox.Show(cur.Cells[colInd].Value.ToString());
        }
        

        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelect.Checked)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Cells["Select"].Value = 1; 
                }
                this.groupBox1.Enabled = true;
            }
            else
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Cells["Select"].Value = 0; 
                }
                this.groupBox1.Enabled = false;
            }
            dataGridView1.RefreshEdit();// Added By Pankaj B. on 20-03-2015 for Bug-25365
        }

        private void btnLocNm_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select distinct  Loc_Master.Loc_Desc as Location from " + vMainTblNm + " Left Join Loc_Master on(employeemast.Loc_code=Loc_Master.Loc_Code) order by Loc_Master.Loc_Desc";
           
            try
            {
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            }
            catch (System.Exception sqlEx)
            {
                string[] msgString = sqlEx.Message.Split('.');
                MessageBox.Show("" + msgString[0]);
                return;
            }
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

             VForText = "Select Location";
             vSearchCol = "Location";
             vDisplayColumnList = "Location:Location";
             vReturnCol = "Location";
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
                string str = StrCol + " where  Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
                dataGridView1.Columns.Clear();
                this.Binddata(str);
                this.mthFldRefresh(0);
                this.chkSelect.Checked = false;


            }

        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); /*Ramya*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            
            SqlStr = "Select distinct department from " + vMainTblNm + " Left Join Loc_Master on(employeemast.Loc_code=Loc_Master.Loc_Code) where  Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "' order by department";
            try
            {

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            }
            catch (System.Exception sqlEx)
            {
                string[] msgString = sqlEx.Message.Split('.');
                MessageBox.Show("" + msgString[0]);
                return;
            }

            // tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Department";
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
                this.txtDept.Text = oSelectPop.pReturnArray[0];
                string str = StrCol + "where  Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
                if (this.txtDept.Text.Trim() != "")
                {
                    str = str + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
                }
                dataGridView1.Columns.Clear();
                this.Binddata(str);
                this.mthFldRefresh(0);
                this.chkSelect.Checked = false;

                //}

            }

        }


        private void mthGridRefresh(string sql)
        {


        }

        private void btnCate_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); /*Ramya*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select distinct Category from " + vMainTblNm + " Left Join Loc_Master on(employeemast.Loc_code=Loc_Master.Loc_Code) where  Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
            if (this.txtDept.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
            }
            SqlStr = SqlStr + " order by Category";
            try
            {

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            }
            catch (System.Exception sqlEx)
            {
                string[] msgString = sqlEx.Message.Split('.');
                MessageBox.Show("" + msgString[0]);
                return;
            }

            // tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Category";
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
                this.txtCategory.Text = oSelectPop.pReturnArray[0];
               
                    string str = StrCol + " where Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
                    if (this.txtDept.Text.Trim() != "")
                    {
                        str = str + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
                    }
                    if (this.txtCategory.Text.Trim() != "")
                    {
                        str = str + " and Category='" + this.txtCategory.Text.ToString().Trim() + "'";
                    }
                    dataGridView1.Columns.Clear();
                    this.Binddata(str);
                    this.mthFldRefresh(0);
                    this.chkSelect.Checked = false;

                //}

            }


        }

        private void btndesig_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); /*Ramya*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select distinct Designation from " + vMainTblNm + " Left Join Loc_Master on(employeemast.Loc_code=Loc_Master.Loc_Code) where  Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
            if (this.txtDept.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
            }
            if (this.txtCategory.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and Category='" + this.txtCategory.Text.ToString().Trim() + "'";
            }
            
            SqlStr=SqlStr+" order by Designation";
            try
            {
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            }
            catch (System.Exception sqlEx)
            {
                string[] msgString = sqlEx.Message.Split('.');
                MessageBox.Show("" + msgString[0]);
                return;
            }

            // tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Designation";
            vSearchCol = "Designation";
            vDisplayColumnList = "Designation:Designation";
            vReturnCol = "Designation";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtDesigName.Text = oSelectPop.pReturnArray[0];

                string str = StrCol + " where Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
                if (this.txtDept.Text.Trim() != "")
                {
                    str = str + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
                }
                if (this.txtCategory.Text.Trim() != "")
                {
                    str = str + " and Category='" + this.txtCategory.Text.ToString().Trim() + "'";
                }
                if (this.txtDesigName.Text.Trim() != "")
                {
                    str = str + " and Designation='" + this.txtDesigName.Text.ToString().Trim() + "'";
                }
                dataGridView1.Columns.Clear();
                this.Binddata(str);
                this.mthFldRefresh(0);
                this.chkSelect.Checked = false;


            }
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication(); /*Ramya*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();

            SqlStr = "Select distinct Grade from " + vMainTblNm + " Left Join Loc_Master on(employeemast.Loc_code=Loc_Master.Loc_Code) where  Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
            if (this.txtDept.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
            }
            if (this.txtCategory.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and Category='" + this.txtCategory.Text.ToString().Trim() + "'";
            }
            if (this.txtDesigName.Text.Trim() != "")
            {
                SqlStr = SqlStr + " and Designation='" + this.txtDesigName.Text.ToString().Trim() + "'";
            }
            SqlStr = SqlStr+" order by Grade";
            try
            {

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            }
            catch (System.Exception sqlEx)
            {
                string[] msgString = sqlEx.Message.Split('.');
                MessageBox.Show("" + msgString[0]);
                return;
            }

            // tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Record Found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            VForText = "Select Grade";
            vSearchCol = "Grade";
            vDisplayColumnList = "Grade:Grade";
            vReturnCol = "Grade";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGradeName.Text = oSelectPop.pReturnArray[0];

                string str = StrCol + " where Loc_Master.Loc_Desc='" + this.txtLocNm.Text.ToString().Trim() + "'";
                if (this.txtDept.Text.Trim() != "")
                {
                    str = str + " and  department='" + this.txtDept.Text.ToString().Trim() + "'";
                }
                if (this.txtCategory.Text.Trim() != "")
                {
                    str = str + " and Category='" + this.txtCategory.Text.ToString().Trim() + "'";
                }
                if (this.txtDesigName.Text.Trim() != "")
                {
                    str = str + " and Designation='" + this.txtDesigName.Text.ToString().Trim() + "'";
                }
                if (this.txtGradeName.Text.Trim() != "")
                {
                    str = str + " and Grade='" + this.txtGradeName.Text.ToString().Trim() + "'";
                }
                dataGridView1.Columns.Clear();
                this.Binddata(str);
                this.mthFldRefresh(0);
                this.chkSelect.Checked = false;

                //}

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            DataGridViewRow cur = new DataGridViewRow();
            cur = dataGridView1.CurrentRow;
            if (cur != null)
            {

                int rInd = dataGridView1.CurrentRow.Index;
                if (rInd != null)
                {
                    this.mthFldRefresh(rInd);
                }

            }
        }

        private void frmWeeklyHoliday_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }

     
            



      
    }
}
