using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Data.SqlClient;

namespace QualityControlProcess
{
    public partial class Form1 : Form
    {
        //[DllImport("user32.dll", EntryPoint = "SetParent")]
        //internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        DataSet dsGloble = new DataSet();
        DataTable Main_vw,Item_vw;
        int CurCol, CurRow;
        Button bt;
        enum Mode
        {
            Add, Edit, View, DefValue
        }

        int CurrentProcessId=0;
        int LastProcessID;
        Mode CurrentMode;
        DataGridViewRow BlankRow = new DataGridViewRow();
        DataGridViewCell xx;
        bool isCancel;
        string _CompId;
        string _databaseName;
        string _serverName;
        string _userID;
        string _password;
        int _Range;
        string _AppUserName;
        //Icon MainIcon ;
        Icon _FrmIcon;
        string _ApplText;
        string _ApplName;
        int _ApplPID;
        string _ApplCode;//="2212";
        
        String cAppPId, cAppName;

        public Form1(string[] param)
        {
         //MessageBox.Show(Process.GetProcessById(Convert.ToInt16(_ApplCode)).MainWindowHandle.ToString());
            //SetParent(Process.GetCurrentProcess().MainWindowHandle, Process.GetProcessById(Convert.ToInt16(_ApplCode)).Handle);
          //  MessageBox.Show("Init...");
            InitializeComponent();
            isCancel = false;
            _CompId = param[0];
            _databaseName = param[1];
            _serverName = param[2];
            _userID = param[3];
            _password = param[4];
            // oDataAccess = new DataAccess_Net.clsDataAccess();
            if (param[5] != "")
            {
                _Range = Convert.ToInt32(param[5].ToString().Replace("^", ""));
            }
            _AppUserName = param[6];
            _FrmIcon = new System.Drawing.Icon(param[7].Replace("<*#*>", " "));
            _ApplText = param[8].Replace("<*#*>", " ");
            _ApplName = param[9];
            _ApplPID = Convert.ToInt32(param[10]);
            _ApplCode = param[11];
            DataLayer.CreateConnection(_serverName, _databaseName, _userID, _password);

            //Added By Amrendra On 21/01/2012 --->
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
            //<--- Added By Amrendra On 21/01/2012 
           // MessageBox.Show("Loading...");
            this.mInsertProcessIdRecord();
        }

        #region Main Application Integration Check
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ueQualityControlProcess.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this._ApplCode + "','" + DateTime.Now.Date.ToShortDateString().ToString() + "','" + this._ApplName + "'," + this._ApplPID + ",'" + this._ApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            DataLayer.ExecuteSQLnonQuery(sqlstr);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this._ApplName + "' and pApplId=" + this._ApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            DataLayer.ExecuteSQLnonQuery(sqlstr);
        }
        private void mcheckCallingApplication()/*Added Rup 07/03/2011*/
        {
            Process pProc;
            Boolean procExists = true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this._ApplPID));
                String pName = pProc.ProcessName;
                string pName1 = this._ApplName.Substring(0, this._ApplName.IndexOf("."));
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
                //MessageBox.Show("Can't proceed,Main Application " + this._ApplText + " is closed", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //Application.Exit();
            }
        }
        #endregion
        private void loadData()
        {
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master", "");  
            //DataLayer.GetMasterDataSet(dsGloble,"QC_Process_Master","");
            DataLayer.GetParameters(dsGloble);
        }
        //<--- Commented By Pankaj On 24/06/2   014 for Bug-22827 Start
        //private void GetProcessData()
        //{           
        //    string sqlcmd = string.Empty;
        //    if (Main_vw == null)
        //        sqlcmd = "select top 1 * from Qc_process_master order by qc_process_id desc";
        //    else
        //        sqlcmd = "select top 1 * from Qc_process_master where qc_process_id ="+CurrentProcessId.ToString();
        //    Main_vw=DataLayer.GetDataTable(sqlcmd);
        //    CurrentProcessId = (Main_vw.Rows.Count > 0 ? Convert.ToInt32(Main_vw.Rows[0]["QC_PROCESS_ID"]) : 0);
        //    sqlcmd = "select ITEM_ID,qc_para,[Desc],std_value,Low_Tol,Up_Tol,is_tol_in_percent from qc_process_item where qc_process_id =" + CurrentProcessId.ToString();
        //    Item_vw = DataLayer.GetDataTable(sqlcmd);       
        //}
        //<--- Commented By Pankaj On 24/06/2014 for Bug-22827 End

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = this._FrmIcon;
           // MessageBox.Show("Loaded...");
            dgvParam.RowHeadersVisible = false;
            BlankRow = dgvParam.Rows[0];
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master","");
            DataGridViewComboBoxColumn lst = (DataGridViewComboBoxColumn)dgvParam.Columns[0];
            lst.DataSource = DataLayer.GetParameters(dsGloble).Tables["QC_Parameter_Master"];
            dsGloble.Tables["QC_Parameter_Master"].PrimaryKey = new DataColumn[] { dsGloble.Tables["QC_Parameter_Master"].Columns["QC_Para"] };
            lst.DisplayMember = "QC_Para";
            lst.ValueMember = "QC_Para";
            lst.Tag = "QC_PARA";
            btnLast_Click(sender, e);
            //CurrentProcessId = 0; //<--- Commented By Pankaj On 23/06/2014 for Bug-22827
            //SetLastProcessID();

            //Databind(CurrentProcessId);//<--- Commented By Pankaj On 23/06/2014 for Bug-22827
            //Databind(LastProcessID);//<--- Added By Pankaj On 23/06/2014 for Bug-22827
            CurrentProcessId = LastProcessID;//<--- Added By Pankaj On 23/06/2014 for Bug-22827
            CurrentMode = Mode.View;
            ControlState(CurrentMode);
            //<--- Commented By Pankaj On 28/04/2014 for Bug-22827 start
            //if (IsNoRec())
              //  ButtonState(true, false, false, false,false,false);
            //else
            //<--- Commented By Pankaj On 28/04/2014 for Bug-22827 start
            ButtonState(true, true, false, false,true ,true   );
            NavButtonState(true, true, false, false);//<--- Added By Pankaj On 28/04/2014 for Bug-22827
            btnEmail.Enabled = false;
            btnLocate.Enabled = false;
            btnPreview.Enabled = false;
            btnPrint.Enabled = false;
            btnHelp.Enabled = false;
            btnExit.Enabled = false;
            //btnCopy.Enabled = true;  
            bt = new Button();
            bt.Visible = false;

            //this.SetMenuRights(); 

        }

        //<--- Commented By Pankaj On 24/06/2014 for Bug-22827 Start
        //private void ColumnBind()
        //{
        //    dgvParam.Columns.Clear();
        //    dgvParam.DataSource = Item_vw;
        //    DataGridViewComboBoxColumn lst = new DataGridViewComboBoxColumn();
        //    lst.HeaderText = "Parameter";
        //    lst.Name = "QC_Para";
        //    lst.DataSource = dsGloble.Tables["QC_Parameter_Master"];
        //    lst.DataPropertyName = "QC_Para";
        //    lst.DisplayMember = "QC_Para";
        //    lst.ValueMember = "QC_Para";

        //    DataGridViewTextBoxColumn colDisc = new DataGridViewTextBoxColumn();
        //    colDisc.Name = "Description";
        //    colDisc.DataPropertyName = "Desc";

        //    DataGridViewTextBoxColumn colStdVal = new DataGridViewTextBoxColumn();
        //    colStdVal.Name = "Standerd Value";
        //    colStdVal.DataPropertyName = "std_value";

        //    DataGridViewTextBoxColumn colTolLow = new DataGridViewTextBoxColumn();
        //    colTolLow.Name = "Tol.(-)";
        //    colTolLow.DataPropertyName = "Low_Tol";

        //    DataGridViewTextBoxColumn colTolHigh = new DataGridViewTextBoxColumn();
        //    colTolHigh.Name = "Tol.(+)";
        //    colTolHigh.DataPropertyName = "Up_Tol";

        //    DataGridViewCheckBoxColumn ColIsPers = new DataGridViewCheckBoxColumn();
        //    ColIsPers.Name = "Tol.(%)";
        //    ColIsPers.DataPropertyName = "is_tol_in_percent";

        //    DataGridViewTextBoxColumn colDefaVal = new DataGridViewTextBoxColumn();
        //    colDefaVal.Name = "Default Values";
        //    colDefaVal.DataPropertyName = "DEF_Val";

        //    dgvParam.Columns.AddRange(new DataGridViewColumn[] { lst, colDisc, colStdVal, colTolLow, colTolHigh, ColIsPers, colDefaVal });
        //    dgvParam.Refresh();

        //}
        //<--- Commented By Pankaj On 24/06/2014 for Bug-22827 End
        private string mGetSQLServerDefaDateFormate()//<--- Added By Amrendra On 21/01/2012 
        {
            return DataLayer.GetDataTable("select [dateformat] from master..syslanguages where langid=(select [value] from sys.configurations where [name]='default language')").Rows[0][0].ToString();
        }

        private void SetMenuRights() 
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            string strSQL = "select padname,barname,range from com_menu where range =" + this._Range;
            dsMenu = DataLayer.GetDataSet(strSQL);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    strSQL += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this._Range;
                    strSQL += "and dbo.func_decoder([user],'T') ='" + this._AppUserName.Trim() + "'";

                }
            }
            dsRights = DataLayer.GetDataSet(strSQL);


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
                //btnSave.Enabled = (rArray[1].ToString().Trim() == "CY" ? true : false);
                //btnNew.Enabled = (myArray[0].ToString().Trim() == "IY" ? true : false);
                //btnEdit.Enabled = (myArray[1].ToString().Trim() == "CY" ? true : false);
                ////menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                //btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }
        private void Databind(int RowId)
        {       
            //if (dsGloble.Tables["QC_Process_Master"].Rows.Count > 0 )
            //{
                txtProcessName.Text = dsGloble.Tables["QC_Process_Master"].Rows[RowId]["QC_PROCESS"].ToString();
                txtSampleQty.Text = dsGloble.Tables["QC_Process_Master"].Rows[RowId]["QC_SAMPLE_QTY"].ToString();
                txtFailIf.Text = dsGloble.Tables["QC_Process_Master"].Rows[RowId]["FAIL_IF_QTY"].ToString();
                DataLayer.GetItemDataSet(dsGloble, "QC_Process_Item", "QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[RowId]["QC_PROCESS_ID"].ToString());
                dgvParam.DataSource = dsGloble.Tables["QC_Process_Item"];
                
            //}
            //else
            //{
            //    DataLayer.GetItemDataSet(dsGloble, "QC_Process_Item", "QC_PROCESS_ID=0");
            //    dgvParam.DataSource = dsGloble.Tables["QC_Process_Item"];
            //}
            //SetNav();
            //if (IsSingleRec())
            //{
            //    NavButtonState(false, false, false, false);

            //}
            
            DataGridRef();

        }

        private void ControlState(Mode md)
        {
            if (md == Mode.View)
            {
                //txtProcessName.ReadOnly = true;
                //txtSampleQty.ReadOnly = true;
                //txtFailIf.ReadOnly = true;
                //dgvParam.ReadOnly = true;
                txtProcessName.Enabled = false ;
                txtSampleQty.Enabled = false;
                txtFailIf.Enabled = false;
                dgvParam.ReadOnly = true;
                dgvParam.AllowUserToAddRows = false;  //<--- Added By Pankaj On 24/06/2014 for Bug-22827 
                NavButtonState(true , true , true ,true );
                
            }
            else if (md == Mode.Edit)
            {
                txtProcessName.ReadOnly = true;
                //txtSampleQty.ReadOnly = false;
                //txtFailIf.ReadOnly = false;
                txtSampleQty.Enabled = true;
                txtFailIf.Enabled = true;
                dgvParam.ReadOnly = false;
                DataGridRef();
                dgvParam.AllowUserToAddRows = true; //<--- Added By Pankaj On 24/06/2014 for Bug-22827 
                NavButtonState(false, false, false, false);
            }

            else
            {
                txtProcessName.ReadOnly = false;//<--- Added By Pankaj On 28/04/2014 for Bug-22827
                //txtSampleQty.ReadOnly = false;
                //txtFailIf.ReadOnly = false;
                txtProcessName.Enabled = true;
                txtSampleQty.Enabled = true;
                txtFailIf.Enabled = true;
                dgvParam.ReadOnly = false;
                dgvParam.AllowUserToAddRows = true; //<--- Added By Pankaj On 24/06/2014 for Bug-22827 
                NavButtonState(false, false, false, false);
            }
            object cm = dgvParam.Columns[0];
                if (cm.GetType().Name == "DataGridViewComboBoxColumn")
                {
                    ((DataGridViewComboBoxColumn)cm).DisplayStyle = (md != Mode.Edit && md != Mode.Add ? DataGridViewComboBoxDisplayStyle.Nothing : DataGridViewComboBoxDisplayStyle.ComboBox);
                }
        }
        private void GetTamplateQCItemTable()
        {
            DataLayer.GetItemDataSet(dsGloble, "QC_Process_Item", "QC_PROCESS_ID=0");
            dgvParam.DataSource = dsGloble.Tables["QC_Process_Item"];
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Cancel, bool Delete, bool Copy)//<--- Added By Pankaj On 26/04/2014 Delete and Copy parameter added for Bug-22827
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
            //<--- Added By Pankaj On 26/04/2014 for Bug-22827 Start
            btnDelete.Enabled = Delete;
            btnCopy.Enabled = Copy;
            //<--- Added By Pankaj On 26/04/2014 for Bug-22827 End
            if (IsNoRec() && CurrentMode !=Mode.Add)
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnCopy.Enabled = false;//<--- Added By Pankaj On 26/04/2014 for Bug-22827
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            dgvParam.EndEdit();
            
            //MessageBox.Show(dgvParam.Rows[0].Cells[0].Value.ToString()     );
            if (dgvParam.Rows.Count ==1  )
            {
                MessageBox.Show("Please Enter Item details", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvParam.Focus();   
                return;
            }
            //txtProcessName_Leave(sender, e);
            int id;
            if (CurrentMode == Mode.Add)
            {
                if (String.IsNullOrEmpty(txtProcessName.Text))
                {
                    MessageBox.Show("Please Enter a Process Name...", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error );
                    txtProcessName.Focus();
                    return;
                }
                //<--- Added By Pankaj On 28/04/2014 for Bug-22827 start
                string sqlcmd;
                DataTable ProcNM_vw;
                //txtSampleQty.Focus();   
                sqlcmd = "select qc_process from qc_process_master where qc_process ='" + txtProcessName.Text + "'";
                ProcNM_vw = DataLayer.GetDataTable(sqlcmd);
                if (ProcNM_vw.Rows.Count > 0)
                {
                    MessageBox.Show("Duplicate Process Name Not Allowed", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error );
                    txtProcessName.Text = "";
                    txtProcessName.Focus();
                    return;
                }
                
                //<--- Added By Pankaj On 28/04/2014 for Bug-22827 end
                int lValid=0 ;

                foreach (DataGridViewRow dgvr in dgvParam.Rows)
                {
                    if(dgvr.IsNewRow==false)
                    if (string.IsNullOrEmpty(dgvr.Cells[2].Value.ToString()))
                    {
                        lValid += 1;
                        MessageBox.Show("Please Add Parameters with Standard Value...", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvr.Cells[2].Selected = true;
                        return;
                    }
                }
                //DataRow[] foundrows = ((DataTable)dgvParam.DataSource).Select("STD_VALUE is null");
                //if (foundrows.Length == ((DataTable)dgvParam.DataSource).Rows.Count)
                //if (lValid == dgvParam.Rows.Count)
                //{
                //    MessageBox.Show("Please Add Parameters with Standard Value...", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Information );
                //    return;
                //}
                string ProcessMaster = "insert into QC_PROCESS_MASTER (QC_PROCESS,QC_SAMPLE_QTY,FAIL_IF_QTY) values ('" + txtProcessName.Text + "','" + txtSampleQty.Text + "','" + txtFailIf.Text + "')";
                id = DataLayer.InsertInMaster(ProcessMaster);
                DataLayer.Save((DataTable)dgvParam.DataSource, id);
                CurrentMode = Mode.View;
                DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master","");
                ControlState(CurrentMode);
                ButtonState(true, true, false, false,true ,true   );
                NavButtonState(true, true, false, false);
                //SetLastProcessID();
                MessageBox.Show("Saved Successfully", this._ApplText, MessageBoxButtons.OK,MessageBoxIcon.Information );
                btnLast_Click(sender, e); //<--- Commented By Pankaj On 24/06/2014  for Bug-22827
            }
            else if (CurrentMode == Mode.Edit)
            {
                string sqlstr = "update QC_PROCESS_Master set qc_sample_qty=" + txtSampleQty.Text.Trim() + ", fail_if_qty=" + txtFailIf.Text.Trim() + "where QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"];
                DataLayer.ExecuteSQLnonQuery(sqlstr);
                id = Convert.ToInt32(dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"]);
                DataLayer.Update((DataTable)dgvParam.DataSource, id);
                ((DataTable)dgvParam.DataSource).Columns.Remove("DataType");
                CurrentMode = Mode.View;
                ControlState(CurrentMode);
                ButtonState(true, true, false, false,true ,true   );
                MessageBox.Show("Saved Successfully", this._ApplText, MessageBoxButtons.OK,MessageBoxIcon.Information  );
            }
            dgvParam.Refresh();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            CurrentMode = Mode.Add;
            GetTamplateQCItemTable();
            ControlState(CurrentMode);
            txtProcessName.Text = "";
            txtSampleQty.Text = "";
            txtFailIf.Text = "";
            btnSave1.Text = "Save";
            ButtonState(false, false, true, true,false,false  );
            txtProcessName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            CurrentMode = Mode.Edit;
            btnSave1.Text = "Update";
            ButtonState(false, false, true, true,false,false  );
            ControlState(CurrentMode);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Discard Changes ?", this._ApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isCancel = true;
                dgvParam.EndEdit();
                this.mcheckCallingApplication();  // To check is parent application is running or not.

                CurrentMode = Mode.View;
                ControlState(CurrentMode);
                //ButtonState(true, true, false, false); 
                Databind(0);
                if (IsNoRec())
                    ButtonState(true, false, false, false, false, false);
                else
                    ButtonState(true, true, false, false, true, true);
                isCancel = false;
            }
        }


             

        private void btnPrev_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }
        private void SetNav()

        {
            if (IsNoRec())
                NavButtonState(false, false, false, false);
            else if (CurrentProcessId == 0)
            {
                NavButtonState(false, false, true, true);
            }
            else if (CurrentProcessId == dsGloble.Tables["QC_Process_Master"].Rows.Count - 1)
            {
                NavButtonState(true, true, false, false);
            }
            else
                NavButtonState(true, true, true, true);

        }
        //private void btnLast_Click(object sender, EventArgs e)
        //{
        //    CurrentProcessId = LastProcessID;
        //    Databind(CurrentProcessId);
        //}

        private void NavButtonState(bool First, bool Prev, bool Next, bool Last)
        {
            btnFirst.Enabled = First;
            btnBack.Enabled = Prev;
            btnForward.Enabled = Next;
            btnLast.Enabled = Last;
        }

        private void SetLastProcessID()
        {
            //dsGloble.Reset();  
            dsGloble.Tables.Remove("QC_Process_Master");//<--- Added By Pankaj On 24/06/2014  for Bug-22827
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master","");
            LastProcessID = dsGloble.Tables["QC_Process_Master"].Rows.Count  - 1;
            CurrentProcessId = LastProcessID;
        }
        private bool IsSingleRec()
        {
            if (dsGloble.Tables["QC_Process_Master"].Rows.Count == 1)
                return true;
            return false;
        }
        private bool IsNoRec()
        {
            if (dsGloble.Tables["QC_Process_Master"].Rows.Count <= 0)
                return true;
            return false;
        }
        private void DataGridRef()
        {
 
            foreach (DataGridViewRow rw in dgvParam.Rows)
            {
                if (rw.Cells[0].Value == null)
                    return;
                RefRow(rw);
            }
            dgvParam.Refresh();
               

        }
        private void RefRow(DataGridViewRow rw)
        {
            if (rw.Cells[0].Value == null ||  isCancel == true )
            {
                return;
            }
            DataRow dr = dsGloble.Tables["QC_Parameter_Master"].Rows.Find(rw.Cells[0].Value.ToString());
                
            if (dr != null)
            {
                switch (dr["DataType"].ToString().ToUpper())
                {
                    case "NUMERIC":
                        rw.Cells[1].ReadOnly = false;
                        rw.Cells[1].Style.ForeColor = Color.Black;
                        rw.Cells[1].Value = dr.ItemArray[1].ToString();//<--- Added By Pankaj On 26/04/2014 for Bug-22827
                        rw.Cells[2].ReadOnly = false;
                        rw.Cells[2].Style.ForeColor = Color.Black;

                        rw.Cells[3].ReadOnly = false;
                        rw.Cells[3].Style.BackColor = Color.White;
                        rw.Cells[4].ReadOnly = false;
                        rw.Cells[4].Style.BackColor = Color.White;
                        rw.Cells[5].ReadOnly = false;
                        rw.Cells[5].Style.BackColor = Color.White;
                        rw.Cells[6].ReadOnly = true;
                        //rw.Cells[6].Style.BackColor = Color.LightGray;
                        rw.Cells[6].Style.ForeColor = Color.LightGray;
                        break;
                    case "VARCHAR":
                        rw.Cells[1].ReadOnly = false;
                        rw.Cells[1].Style.ForeColor = Color.Black;
                        rw.Cells[1].Value = dr.ItemArray[1].ToString();//<--- Added By Pankaj On 26/04/2014 for Bug-22827
                        rw.Cells[2].ReadOnly = false;
                        rw.Cells[2].Style.ForeColor = Color.Black;
                        rw.Cells[3].ReadOnly = true;
                        //rw.Cells[3].Style.BackColor = Color.LightGray;
                        rw.Cells[3].Style.ForeColor = Color.LightGray;
                        rw.Cells[4].ReadOnly = true;
                        //rw.Cells[4].Style.BackColor = Color.LightGray;
                        rw.Cells[4].Style.ForeColor = Color.LightGray;
                        rw.Cells[5].ReadOnly = true;
                        //rw.Cells[5].Style.BackColor = Color.LightGray;
                        rw.Cells[5].Style.ForeColor = Color.LightGray;
                        rw.Cells[6].ReadOnly = false;
                        rw.Cells[6].Style.BackColor = Color.White;
                        break;
                    default:
                        MessageBox.Show(dr["DataType"].ToString() + " Not a Valid Data Type", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error );
                        rw.Cells[1].ReadOnly = true;
                        rw.Cells[1].Style.ForeColor = Color.LightGray;
                        rw.Cells[2].ReadOnly = true;
                        rw.Cells[2].Value ="";
                        rw.Cells[2].Style.ForeColor = Color.LightGray;
                        rw.Cells[3].ReadOnly = true;
                        rw.Cells[3].Value = "";
                        rw.Cells[3].Style.ForeColor = Color.LightGray;
                        rw.Cells[4].ReadOnly = true;
                        rw.Cells[4].Value = "";
                        rw.Cells[4].Style.ForeColor = Color.LightGray;
                        rw.Cells[5].ReadOnly = true;
                        rw.Cells[5].Value = false;
                        rw.Cells[5].Style.ForeColor = Color.LightGray;
                        rw.Cells[6].ReadOnly = true;
                        rw.Cells[6].Value = "";
                        rw.Cells[6].Style.BackColor = Color.White;
                        break;
                }
                //dataGridView1.Refresh();
            }
        }
 

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(this.ActiveControl.Name.ToString());
           
            //if (!string.IsNullOrEmpty(dgvParam.Rows[e.RowIndex].Cells[0].Value.ToString().Trim()) && e.ColumnIndex==1)
            //if (isCancel ==false)
            RefRow(dgvParam.Rows[e.RowIndex]);


        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
             DataRow dr = dsGloble.Tables["QC_Parameter_Master"].Rows.Find(dgvParam[0,e.RowIndex].Value.ToString());
            // MessageBox.Show(dgvParam.Columns[e.ColumnIndex].HeaderText);
            // MessageBox.Show(dr.RowState.ToString() );  


             if (dgvParam.Columns[e.ColumnIndex].HeaderText == "Default Value" && dr != null && dr["DataType"].ToString().ToUpper() == "VARCHAR")
             {
                 CurCol = e.ColumnIndex;
                 CurRow = e.RowIndex;

                 xx = dgvParam.Rows[CurRow].Cells[CurCol];
                 //if (!xx.ReadOnly)
                 //{
                 System.Drawing.Rectangle rct = dgvParam.GetCellDisplayRectangle(xx.ColumnIndex, xx.RowIndex, true);

                 bt.Size = new Size(25, rct.Height - 1);
                 bt.Top = dgvParam.Top + dgvParam.GetCellDisplayRectangle(xx.ColumnIndex, xx.RowIndex, true).Top + dgvParam.GetCellDisplayRectangle(xx.ColumnIndex, xx.RowIndex, true).Height + 2;
                 bt.Left = dgvParam.Left + dgvParam.GetCellDisplayRectangle(xx.ColumnIndex, xx.RowIndex, true).Left + rct.Width - bt.Width;
                 bt.Name = "Button1";
                 bt.BackColor = xx.Style.SelectionBackColor;
                 bt.Visible = true;
                 bt.TabStop = false;
                 bt.FlatStyle = FlatStyle.Standard;
                 bt.Click -= new EventHandler(bt_Click);
                 bt.Click += new EventHandler(bt_Click);
                 this.Controls.Add(bt);
                 bt.BringToFront();
                 //}
                 //else
                 //    if (bt != null)
                 //        bt.Visible = false;
             }
             else if (bt != null)
                 bt.Visible = false;
        }

        void bt_Click(object sender, EventArgs e)
        {
            bt.Visible = false;
            DefaValueMode(true);
            txtDefValue.Text = dgvParam.Rows[CurRow].Cells[CurCol].Value.ToString();
            if ((CurrentMode == Mode.Add | CurrentMode == Mode.Edit) && !dgvParam.Rows[CurRow].Cells[CurCol].ReadOnly)
                txtDefValue.ReadOnly = false;
            else
                txtDefValue.ReadOnly = true;
        }
        void DefaValueMode(bool Edit)
        {

            pnlDefValue.Visible = Edit;
            pnlCmdButtons.Enabled = !Edit;
            pnlMain.Enabled = !Edit;
        }

        private void btnDefValueDone_Click(object sender, EventArgs e)
        {

            dgvParam.Rows[CurRow].Cells[CurCol].Value = txtDefValue.Text;
            DefaValueMode(false);
        }

        private void btnDefValueCancel_Click(object sender, EventArgs e)
        {
            DefaValueMode(false);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            this.mDeleteProcessIdRecord();
            
        }

        

        private void txtSampleQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void txtFailIf_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Process ?", this._ApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                //string sqlstr = "select * from ITEM_QC_PROCESS_MASTER where it_qcproc=" + dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"];
                string sqlstr = "select * from ITEM_QC_PROCESS_MASTER where it_qcproc=" + dsGloble.Tables["QC_Process_Master"].Rows[0]["QC_PROCESS_ID"];
                DataTable dt1x = DataLayer.GetDataTable(sqlstr);
                //sqlstr = "select * from QC_INSPECTION_ITEM where qc_process_id=" + dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"];
                sqlstr = "select * from QC_INSPECTION_ITEM where qc_process_id=" + dsGloble.Tables["QC_Process_Master"].Rows[0]["QC_PROCESS_ID"];
                DataTable dt2x = DataLayer.GetDataTable(sqlstr);
                if (dt1x.Rows.Count > 0 | dt2x.Rows.Count > 0)
                    MessageBox.Show("Can't Delete, process is in use!", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    //DataLayer.RemoveItem("delete from QC_PROCESS_ITEM where QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"]);//Commented By Pankaj On 23/06/2014 for Bug-22827
                    //DataLayer.RemoveItem("delete from QC_PROCESS_MASTER where QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"]);//Commented By Pankaj On 23/06/2014 for Bug-22827
                    DataLayer.RemoveItem("delete from QC_PROCESS_ITEM where QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[0]["QC_PROCESS_ID"]);
                    DataLayer.RemoveItem("delete from QC_PROCESS_MASTER where QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[0]["QC_PROCESS_ID"]);

                    //sqlstr = "delete from QC_PROCESS_MASTER where QC_PROCESS_ID=" + dsGloble.Tables["QC_Process_Master"].Rows[CurrentProcessId]["QC_PROCESS_ID"];
                    //DataLayer.ExecuteSQLnonQuery(sqlstr);
                    MessageBox.Show("Process Deleted...", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.None);
                    dsGloble.Tables.Remove("QC_Process_Master");
                    DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master", "");

                    if (IsNoRec())
                        ButtonState(true, false, false, false, false, false);
                    else
                        ButtonState(true, true, false, false, true, true);
                    txtProcessName.Text = "";
                    txtSampleQty.Text = "";
                    txtFailIf.Text = "";
                    //SetLastProcessID();Commented By Pankaj On 23/06/2014 for Bug-22827
                    //Databind(LastProcessID);////Commented By Pankaj On 23/06/2014 for Bug-22827
                    //Databind(CurrentProcessId);//<--- Commented By Pankaj On 28/04/2014 for Bug-22827
                    btnLast_Click(sender, e);
                }
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            //<--- Commented By Pankaj On 24/06/2014  for Bug-22827 Start
            //string sqlcmd = string.Empty;
            //sqlcmd = "select top 1 * from Qc_process_master";

            //sqlcmd = "select top 1 ITEM_ID,qc_para,[Desc],std_value,Low_Tol,Up_Tol,is_tol_in_percent from qc_process_item";
            //CurrentProcessId = Convert.ToInt32(dsGloble.Tables["QC_Process_Master"].Rows[0]["qc_process_id"].ToString()) ;
            //MessageBox.Show(CurrentProcessId.ToString() );   
            //CurrentProcessId = 0;
            //SetNav();
            //Databind(CurrentProcessId);
            //<--- Commented By Pankaj On 24/06/2014  for Bug-22827 End

            //<--- Added By Pankaj On 24/06/2014  for Bug-22827
           dsGloble.Tables.Remove("QC_Process_Master");
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master", " order by qc_process_id asc");
         if (dsGloble.Tables["QC_Process_Master"].Rows.Count != 0)
            {
                NavButtonState(false, false, true, true);
                Databind(0);

            }
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 End

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 Start
            CurrentProcessId = Convert.ToInt32(dsGloble.Tables["QC_Process_Master"].Rows[0]["QC_PROCESS_ID"].ToString());
            dsGloble.Tables.Remove("QC_Process_Master");//<--- Added By Pankaj On 24/06/2014  for Bug-22827
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master", " where qc_process_id < '"+CurrentProcessId +"' order by qc_process_id desc");
            if (dsGloble.Tables["QC_Process_Master"].Rows.Count != 0)
            {
                NavButtonState(true, true, true, true);
                Databind(0);

            }
            else
            {
                btnFirst_Click(sender, e);
            }
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 end
            //<--- Commented By Pankaj On 24/06/2014  for Bug-22827 Start
            //if (CurrentProcessId > 0)
            //    CurrentProcessId -= 1;
            //<--- Commented By Pankaj On 24/06/2014  for Bug-22827 End
            
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            //<--- Commented By Pankaj On 23/06/2014  for Bug-22827 start
            //this.mcheckCallingApplication();  // To check is parent application is running or not.
            //if (CurrentProcessId < dsGloble.Tables["QC_Process_Master"].Rows.Count - 1)
            //    CurrentProcessId += 1;
            //Databind(CurrentProcessId);
            //<--- Commented By Pankaj On 23/06/2014  for Bug-22827 End
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 Start
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            CurrentProcessId = Convert.ToInt32(dsGloble.Tables["QC_Process_Master"].Rows[0]["QC_PROCESS_ID"].ToString());
            dsGloble.Tables.Remove("QC_Process_Master");//<--- Added By Pankaj On 24/06/2014  for Bug-22827
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master", " where qc_process_id > '" + CurrentProcessId + "' order by qc_process_id asc");
            if (dsGloble.Tables["QC_Process_Master"].Rows.Count != 0)
            {
                Databind(0);

            }
            else
            {
                btnLast_Click(sender, e);
            }
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 End
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();  // To check is parent application is running or not.
            
            string sqlcmd = string.Empty;
            //<--- Commented By Pankaj On 23/06/2014  for Bug-22827 start
            //sqlcmd = "select top 1 * from Qc_process_master";
            //sqlcmd = "select top 1 * from Qc_process_master order by qc_process_id desc";
            //Main_vw = dsGloble.Tables.Add("QC_Process_Master"); 

            //sqlcmd = "select top 1 ITEM_ID,qc_para,[Desc],std_value,Low_Tol,Up_Tol,is_tol_in_percent from qc_process_item order by  qc_process_id desc";
            //dsGloble.Tables.Remove("QC_Process_Item");//<--- Added By Pankaj On 24/06/2014  for Bug-22827
            //Item_vw = dsGloble.Tables.Add("QC_Process_Item");
            //Item_vw = DataLayer.GetDataTable(sqlcmd);
            //<--- Commented By Pankaj On 23/06/2014  for Bug-22827 end
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 Start
            dsGloble.Tables.Remove("QC_Process_Master");
            DataLayer.GetMasterDataSet(dsGloble, "QC_Process_Master", " order by qc_process_id desc");

            if (dsGloble.Tables["QC_Process_Master"].Rows.Count != 0)
            {
                NavButtonState(true, true, false, false);
                Databind(0);

            }
            //<--- Added By Pankaj On 24/06/2014  for Bug-22827 End

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void dgvParam_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                (e.Control as DataGridViewTextBoxEditingControl).KeyPress -= new KeyPressEventHandler(MinedgvParam_KeyPress);
                (e.Control as DataGridViewTextBoxEditingControl).KeyPress += new KeyPressEventHandler(MinedgvParam_KeyPress);                
            }
        }

        private void MinedgvParam_KeyPress(object sender, KeyPressEventArgs e)
        {
             DataRow dr = dsGloble.Tables["QC_Parameter_Master"].Rows.Find(dgvParam[0,dgvParam.CurrentCell.RowIndex].Value.ToString());

             if (CurrentMode == Mode.Add | CurrentMode == Mode.Edit)
             {
                 if (Convert.ToInt32(e.KeyChar) == 39)
                 {
                     e.Handled = true;
                     return;
                 }

                 if (dgvParam.CurrentCell.OwningColumn.HeaderText == "Standard Value")
                 {
                     if (dr != null && dr["DataType"].ToString().ToUpper() == "NUMERIC")
                     {
                         if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8 || e.KeyChar == 45)
                             e.Handled = false;
                         else
                             e.Handled = true;
                         return;
                     }
                 }

                 if (dgvParam.CurrentCell.OwningColumn.HeaderText == "(-)Tol." || dgvParam.CurrentCell.OwningColumn.HeaderText == "(+)Tol.")
                 {
                     if ((!String.IsNullOrEmpty(dgvParam["StandardValue", dgvParam.CurrentCell.RowIndex].Value.ToString()) && (e.KeyChar >= 48 && e.KeyChar <= 57)) || e.KeyChar == 8)
                         e.Handled = false;
                     else
                         e.Handled = true;
                     return;
                 }
//                 if (dgvParam.CurrentCell.OwningColumn.HeaderText == "Default Value")
  //               {
//                     dgvParam.CurrentCell.OwningRow.IsNewRow 
                     if (String.IsNullOrEmpty(dgvParam["StandardValue", dgvParam.CurrentCell.RowIndex].Value.ToString()) &&
                         String.IsNullOrEmpty(dgvParam["Parameter", dgvParam.CurrentCell.RowIndex].Value.ToString()))
                     {
                         e.Handled = true;
                         return;
    //                 }
                 }
             }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnNew.Enabled)
                btnNew_Click(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnEdit.Enabled)
                btnEdit_Click(sender, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (btnSave.Enabled)
                btnSave_Click(sender, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnCancel.Enabled)
                btnCancel_Click(sender, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(this.btnExit, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvParam_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

            //if (isCancel == false)
            //    RefRow(dgvParam.Rows[e.RowIndex]);
        }

        private void dgvParam_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //<--- Added By Pankaj On 24/06/2014 for Bug-22827 Start
            dgvParam.EndEdit();
            if (dgvParam.Columns[e.ColumnIndex].HeaderText == "Standard Value" && dgvParam.Rows[e.RowIndex ].Cells[e.ColumnIndex ].Value.ToString()==""   )
            {

                MessageBox.Show("Please Add Parameters with Standard Value...", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true; 
                
            }
            //<--- Added By Pankaj On 24/06/2014 for Bug-22827 End
            ////dgvParam.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //dgvParam.EndEdit();
            //if (dgvParam.CurrentCell is DataGridViewComboBoxCell)
            //{
            //    if (string.IsNullOrEmpty(dgvParam.CurrentCell.Value.ToString().Trim()) && isCancel==false)
            //        e.Cancel = true;
            //}
        }

        private void btnCopy_Click(object sender, EventArgs e) //<--- Added By Pankaj On 26/04/2014 for Bug-22827
        {
            
            CurrentMode = Mode.Add  ;
            ControlState(CurrentMode);
            
            if (IsNoRec())
                ButtonState(true, false, false, false, false, false);
            else
                txtProcessName.Text = "";
            //txtSampleQty.Text = "";
            //txtFailIf.Text = "";
            btnSave1.Text = "Save";
            ButtonState(false, false, true, true, false, false);
            txtProcessName.Focus();
            
        }

        private void txtProcessName_Leave(object sender, EventArgs e)
        {
            string sqlcmd;
            DataTable ProcNM_vw;
            sqlcmd = "select qc_process from qc_process_master where qc_process ='" + txtProcessName.Text   +"'" ;
            ProcNM_vw = DataLayer.GetDataTable(sqlcmd);   
            if (ProcNM_vw.Rows.Count>0)
            {
                MessageBox.Show("Duplicate Process Name Not Allowed", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error );
                txtProcessName.Text = "";
                txtProcessName.Focus();
                return;
            }
        }

        //<--- Added By Pankaj On 24/06/2014 for Bug-22827 Start

        private void txtFailIf_Leave(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtSampleQty.Text) && !string.IsNullOrEmpty(txtFailIf.Text))
            {
            if (Convert.ToInt32(txtFailIf.Text)   > Convert.ToInt32(txtSampleQty.Text  ) )
            {
                MessageBox.Show("Fails iF Qty greater than Sample Qty Not Allowed", this._ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFailIf.Text  = "";
                txtFailIf.Focus();  
                return;
            }
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string msg;

            if (CurrentMode == Mode.Add || CurrentMode == Mode.Edit )
                msg = "Changes found , Discard changes ?";
            else
                msg = "want to exit ?";

            if (MessageBox.Show(msg, this._ApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                if (CurrentMode == Mode.Add || CurrentMode == Mode.Edit)
                    btnSave.PerformClick();
                else
                    e.Cancel = false;
            else
                e.Cancel = true;
        }


        //<--- Added By Pankaj On 24/06/2014 for Bug-22827 End
        


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    foreach (DataRow r in dsGloble.Tables["QC_Process_ITEM"].Rows)
        //    {
        //        foreach (DataColumn c in r.Table.Columns)
        //            MessageBox.Show(r[c].ToString());

        //    }
        //}
    }
}
