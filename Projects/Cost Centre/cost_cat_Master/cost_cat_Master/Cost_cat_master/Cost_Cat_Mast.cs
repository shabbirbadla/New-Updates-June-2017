using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;
using System.Globalization;
using System.Threading;
using uBaseForm;
using System.IO;
using System.Diagnostics;



namespace Cost_cat_master
{
    public partial class Cost_Cat_Mast : uBaseForm.FrmBaseForm
    {
        string _MasterCode;
        string _TableName;
        string _strSQL;
        DataAccess_Net.clsDataAccess oDataAccess;
        private DataSet dsFormInfo;
        private DataSet dsFieldInfo;
        private DataSet dsExport; // Added By Pankaj B. on 16-02-2015 for Bug-25197
        private string ExportTbl = ""; // Added By Pankaj B. on 16-02-2015 for Bug-25197
        private int _Range;
        private string _UserName;
        private string vMainField = string.Empty, vMainFldVal = string.Empty, VmainFieldCaption = string.Empty;
        String cAppPId, cAppName;
        Boolean vValid;
        DataView dvwMainm = new DataView();
        private DataSet dsMain = new DataSet();
        Dictionary<string, string> validations;
        private enum FormMode
        {
            New, Edit
        };

        public Cost_Cat_Mast()
        {
            InitializeComponent();
            _TableName = "Cost_Cat_Mast"; //Birendra
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Cost_Cat_Mast_Load(object sender, EventArgs e)
        {
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            validations = new Dictionary<string, string>();
            this.pAppPath = Application.StartupPath;
            this.btnCancel.Enabled = false;
            this.btnLocate.Enabled = true;
            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Visible = false;
            this.btnEmail.Enabled = false;
            this.panel1.Enabled = false;
            this.panel2.Visible = false;
            this.Height = 188 ;


            this._strSQL = "SELECT cost_cat_id,cost_cat_name,descript,activat FROM ";
            dsMain = GetDataFromSelectedTable(this._strSQL,this._TableName, oDataAccess);
            ControlBind(this._TableName );
            this.mInsertProcessIdRecord();
        }
        #region Save/Update/Cancel

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mthNew();
            HandleNavigationButtons(false, false, false, false);
            HandleButtons(false, false, true, true, false);

            EnableDiableFormControls( true);
            this.btnLocate.Enabled = false ;

        }
        private void mthNew()
        {
            this.pAddMode = true;
            this.pEditMode = false;
            object objects = this;
            DataRow drCurrent;
            drCurrent = dsMain.Tables[0].NewRow();
            dsMain.Tables[0].Rows.Add(drCurrent);
            ControlBind(this._TableName);
            this.BindingContext[dsMain.Tables[0]].Position=this.BindingContext[dsMain.Tables[0]].Count;
            this.ChkActivat.Checked = false;
            this.ChkActivat.Checked = true;
            dsMain.Tables[0].Rows[0].BeginEdit();

        }

        public void ControlBind(string tableName)
        {
            this.errorProvider1.Clear();
            DataSet dsData = new DataSet();
            string strSQL;
            int i = this.BindingContext[dsMain.Tables[0]].Position;
            this.txtCatName.DataBindings.Clear();
            this.txtDesc.DataBindings.Clear();
            this.ChkActivat.DataBindings.Clear();
            //Birendra : Bug-7896 on 24/12/2012 :Start:
            if  (dsMain.Tables[0].Rows.Count > 1 && i==0)
                HandleNavigationButtons(false , false , true , true);
            else
                if (dsMain.Tables[0].Rows.Count > 1)
                HandleNavigationButtons(true, true, true, true);
                else
                    HandleNavigationButtons(false, false, false, false);
            //Birendra : Bug-7896 on 24/12/2012 :end:


            this.txtCatName.DataBindings.Add("Text", dsMain.Tables[0], "cost_cat_name");
            this.txtDesc.DataBindings.Add("Text", dsMain.Tables[0], "descript");
            this.ChkActivat.DataBindings.Add("checked", dsMain.Tables[0], "activat", true, DataSourceUpdateMode.OnPropertyChanged);
            
            strSQL = this._strSQL  + tableName;

            dsData = oDataAccess.GetDataSet(strSQL, null, 20);
            dsData.Tables[0].TableName = "TempData";

            //this.cmbGroup.DataSource = dsData.Tables[0] ;
            //this.cmbGroup.DisplayMember = "cost_cat_name";
            //this.cmbGroup.ValueMember = "cost_cat_name";
            //this.cmbGroup.DataBindings.Add("SelectedValue",dsMain.Tables[0],"group_cost");
        }
        public DataSet GetDataFromSelectedTable(string strSQL,string tableName, DataAccess_Net.clsDataAccess oDataAccess)
        {
            DataSet dsData = new DataSet();

            try
            {
                strSQL = strSQL  + tableName;

                dsData = oDataAccess.GetDataSet(strSQL, null, 20);
                dsData.Tables[0].TableName = "FormInfo";


            }
            catch (Exception ex)
            {
//                throw ex;
                errorhandle(ex.Message);
            }
            finally
            {
                //objParam = null;
                //colParams = null;
                //oDataAccess = null;
            }
            return dsData;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }

            this.pAddMode = false;
            this.pEditMode = true;
            ControlBind(this._TableName);
            int i = this.BindingContext[dsMain.Tables[0]].Position;
            dsMain.Tables[0].Rows[i].BeginEdit();
            HandleNavigationButtons(false, false, false, false);
            HandleButtons(false, false, true, true, false);
            EnableDiableFormControls(true);
            this.btnLocate.Enabled = false ;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Birendra : Bug-7368 on 26/11/2012 :Start:
            vValid = true;
            TextBox t1 = new TextBox();
           //Boolean RecordValidation = true;
            t1.Visible = true;
            this.Controls.Add(t1);
            object objects = this;
            t1.Focus();
            if (vValid == false)
            {
                return;
            }
            //Birendra : Bug-7368 on 26/11/2012 :End:


            if (CheckValidation())
            {
                SaveFormData(this._TableName, oDataAccess);
                EnableDiableFormControls(false);
                this.btnLocate.Enabled = true;
                HandleNavigationButtons(true, true, true, true);
                HandleButtons(true, true, false, false, true);
                // Birendra : Bug-7368 on 26/11/2012 :Start:
                this.pAddMode = false;
                this.pEditMode = false;
                CurrencyManager cm = (CurrencyManager)this.BindingContext[dsMain.Tables[0]];
                DataRow findRow = ((DataRowView)cm.Current).Row;
                int j = dsMain.Tables[0].Rows.IndexOf(findRow);
                dsMain = GetDataFromSelectedTable(this._strSQL, this._TableName, oDataAccess);
                ControlBind(this._TableName);
                this.BindingContext[dsMain.Tables[0]].Position = j;
                // Birendra : Bug-7368 on 26/11/2012 :End:

            }

        }

        public int SaveFormData(string tableName,DataAccess_Net.clsDataAccess oDataAccess)
        {
            int id = 0;
            DataSet dsData = new DataSet();
            StringBuilder strFieldNames = new StringBuilder();
            StringBuilder strFieldValues = new StringBuilder();
            string strSQL="";
            try
            {

                //strSQL = "SELECT * FROM " + tableName +" Where cost_cat_name = "+"'"+this.cmbGroup.Text +"'";
                //dsData = oDataAccess.GetDataSet(strSQL, null, 20);
                //dsData.Tables[0].TableName = "TmpData";
//                int j = this.BindingContext[dsMain.Tables[0]].Position;
                CurrencyManager cm = (CurrencyManager)this.BindingContext[dsMain.Tables[0]];
                DataRow findRow = ((DataRowView)cm.Current).Row;
                int j = dsMain.Tables[0].Rows.IndexOf(findRow);

                //if (dsData.Tables[0].Rows.Count == 0)
                //{
                //    dsMain.Tables[0].Rows[0]["gr_cost_id"] = 9999;
                //}
                //else
                //{
                //    dsMain.Tables[0].Rows[j]["gr_cost_id"] = dsData.Tables[0].Rows[0]["cost_cat_id"];
                //}

                //dsMain.Tables[0].AcceptChanges();
                if (this.pAddMode)
                {
                    oDataAccess.BeginTransaction();
                    strSQL = InsertStatment(tableName, null);
                    oDataAccess.ExecuteSQLStatement(strSQL, null, 20, true);

                    // Added By Pankaj B. on 16-02-2015 for Bug-25197 Start
                    strSQL = "select cmastcode,dbexport from Tbl_DataExport_Mast where ctype='M' and cmastcode='cost_Cat_mast'";
                    dsExport= oDataAccess.GetDataSet(strSQL,null,25);
                    if (dsExport.Tables[0].Rows.Count!=0)
                    {
                        ExportTbl = dsExport.Tables[0].Rows[0]["dbexport"].ToString().Trim() + ".dbo." + tableName;
                        strSQL = InsertStatment(ExportTbl, null);
                        oDataAccess.ExecuteSQLStatement(strSQL, null, 20, true);
                    }
                    // Added By Pankaj B. on 16-02-2015 for Bug-25197 End

                    oDataAccess.CommitTransaction();
                }
                if (this.pEditMode)
                {
                    oDataAccess.BeginTransaction();
                    strSQL = UpdateStatment(tableName, "cost_cat_id") + " where cost_cat_id=" + dsMain.Tables[0].Rows[j]["cost_cat_id"].ToString();
                    oDataAccess.ExecuteSQLStatement(strSQL, null, 20, true);

                    // Added By Pankaj B. on 16-02-2015 for Bug-25197 Start
                    strSQL = "select cmastcode,dbexport from Tbl_DataExport_Mast where ctype='M' and cmastcode='cost_Cat_mast'";
                    dsExport = oDataAccess.GetDataSet(strSQL, null, 25);
                    if (dsExport.Tables[0].Rows.Count != 0)
                    {
                        ExportTbl = dsExport.Tables[0].Rows[0]["dbexport"].ToString().Trim() + ".dbo." + tableName;
                        strSQL = UpdateStatment(ExportTbl, "cost_cat_id") + " where cost_cat_id=(select cost_cat_id from "+ExportTbl+" where cost_cat_name= '"+dsMain.Tables[0].Rows[j]["cost_cat_name"].ToString()+"')";
                        oDataAccess.ExecuteSQLStatement(strSQL, null, 20, true);
                    }
                    // Added By Pankaj B. on 16-02-2015 for Bug-25197 End

                    oDataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
//                throw ex;
                errorhandle(ex.Message);

            }
            finally
            {
                //objParam = null;
                //colParams = null;
                //oDataAccess = null;
            }

            return id;
        }
        public string InsertStatment(string tablename, string exclfld)
        {
            string strfld = "", strval = "", strSQL = "", paramType = "";
            bool convertstr = false;
            strSQL = "INSERT INTO " + tablename + "(";
            int j = this.BindingContext[dsMain.Tables[0]].Position;

            try
            {

                for (int i = 0; i < dsMain.Tables[0].Columns.Count; i++)
                {
                    paramType = "";
                    convertstr = false;

                    switch (dsMain.Tables[0].Columns[i].DataType.ToString())
                    {
                        case "System.String":
                            paramType = "'";
                            break;
                        case "System.Decimal":
                            paramType = "";
                            break;
                        case "System.Boolean":
                            paramType = "";
                            convertstr = true;
                            break;
                        case "System.DateTime":
                            paramType = "";
                            break;
                        case "System.Int32":
                            paramType = "";
                            break;
                        case "System.Byte[]":
                            paramType = "";
                            break;
                    }

                    if (dsMain.Tables[0].Rows[j][i].ToString() != "" || convertstr)
                    {
                        if (strfld == "")
                        {
                            strfld = dsMain.Tables[0].Columns[i].ToString();
                            if (!convertstr)
                                strval = paramType + dsMain.Tables[0].Rows[j][i].ToString() + paramType;
                            else
                                strval = paramType + Convert.ToByte(dsMain.Tables[0].Rows[j][i]) + paramType;
                        }
                        else
                        {
                            strfld = strfld + "," + dsMain.Tables[0].Columns[i].ToString();
                            if (!convertstr)
                                strval = strval + "," + paramType + dsMain.Tables[0].Rows[j][i].ToString() + paramType;
                            else
                                strval = strval + "," + paramType + Convert.ToByte(dsMain.Tables[0].Rows[j][i]) + paramType;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
//                throw ex;
                errorhandle(ex.Message);
            }
            finally
            {
            }
            strSQL = strSQL + strfld + ") Values (" + strval + ")";
            return strSQL;
        }

        public string UpdateStatment(string tablename, string exclfld)
        {
            string strfld = "", strval = "", strSQL = "", paramType = "";
            bool convertstr = false;
            strSQL = "Update " + tablename + " set ";
            int j = this.BindingContext[dsMain.Tables[0]].Position;

            try
            {
                for (int i = 0; i < dsMain.Tables[0].Columns.Count; i++)
                {
                    paramType = "";
                    convertstr = false;
                    switch (dsMain.Tables[0].Columns[i].DataType.ToString())
                    {
                        case "System.String":
                            paramType = "'";
                            break;
                        case "System.Decimal":
                            paramType = "";
                            break;
                        case "System.Boolean":
                            paramType = "";
                            convertstr = true;
                            break;
                        case "System.DateTime":
                            paramType = "";
                            break;
                        case "System.Int32":
                            paramType = "";
                            break;
                        case "System.Byte[]":
                            paramType = "";
                            break;
                    }
//                    if ((dsMain.Tables[0].Rows[j][i].ToString() != "" || convertstr) && !exclfld.Contains(dsMain.Tables[0].Columns[i].ColumnName.ToString()))
                    //Birendra - Bug-12454 on 15/04/2013 :modified above condition as per below one:
                    if ((dsMain.Tables[0].Columns[i].ToString() != "" || convertstr) && !exclfld.Contains(dsMain.Tables[0].Columns[i].ColumnName.ToString()))
                    {
                        strfld = dsMain.Tables[0].Columns[i].ToString();
                        if (strval == "")
                        {
                            if (convertstr)
                                strval = strfld + " = " + paramType + Convert.ToByte(dsMain.Tables[0].Rows[j][i]) + paramType;
                            else
                                strval = strfld + " = " + paramType + dsMain.Tables[0].Rows[j][i].ToString() + paramType;

                        }
                        else
                        {
                            if (convertstr)
                                strval = strval + "," + strfld + " = " + paramType + Convert.ToByte(dsMain.Tables[0].Rows[j][i]) + paramType;
                            else
                                strval = strval + "," + strfld + " = " + paramType + dsMain.Tables[0].Rows[j][i].ToString() + paramType;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
//                throw ex;
                errorhandle(ex.Message);
            }
            finally
            {
            }
            strSQL = strSQL+strval;
            return strSQL;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableDiableFormControls( false);
            this.pAddMode = false;
            this.pEditMode = false;
            HandleButtons(true, true, false, false, true);
            this.btnLocate.Enabled = true;
            dsMain = GetDataFromSelectedTable(this._strSQL ,this._TableName, oDataAccess);
            ControlBind(this._TableName);


        }

        private void EnableDiableFormControls( bool enabled)
        {
            this.panel1.Enabled = enabled;
            if (enabled)
            {
                if (this.pAddMode == true)
                {
                    this.txtCatName.Enabled = true;
                    this.txtCatName.Focus();
                }
                else
                {
                    this.txtCatName.Enabled = false;
                    this.txtDesc.Focus();
                }
            }
        }

        private void SetFocusToControl(Control ctrl, string controlName)
        {
        }

        #endregion

        #region Navigation

        private void btnFirst_Click(object sender, EventArgs e)
        {
            int i = this.BindingContext[dsMain.Tables[0]].Position;
            this.BindingContext[dsMain.Tables[0]].Position = 0;
            //Birendra : Bug-7896 on 20/12/2012 :Start:
            this.btnLast.Enabled = true ;
            this.btnForward.Enabled = true;
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            //Birendra : Bug-7896 on 20/12/2012 :End:


        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            int i = this.BindingContext[dsMain.Tables[0]].Position;
            this.BindingContext[dsMain.Tables[0]].Position -= 1;
            //Birendra : Bug-7896 on 20/12/2012 :Start:
            if (this.BindingContext[dsMain.Tables[0]].Position == 0)
            {
                this.btnLast.Enabled = true;
                this.btnForward.Enabled = true;
                this.btnFirst.Enabled = false;
                this.btnBack.Enabled = false;
            }
            else
            {
                this.btnLast.Enabled = true;
                this.btnForward.Enabled = true;
                this.btnFirst.Enabled = true;
                this.btnBack.Enabled = true;
            }
            //Birendra : Bug-7896 on 20/12/2012 :End:

        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            int i=this.BindingContext[dsMain.Tables[0]].Position;
            this.BindingContext[dsMain.Tables[0]].Position   += 1;
            if (this.BindingContext[dsMain.Tables[0]].Position>i)
            {
                this.btnFirst.Enabled=true;
                this.btnBack.Enabled=true;
            }
            //Birendra : Bug-7896 on 20/12/2012 :Start:
            if (this.BindingContext[dsMain.Tables[0]].Position == this.BindingContext[dsMain.Tables[0]].Count-1)
            {
                this.btnLast.Enabled = false;
                this.btnForward.Enabled = false;
                this.btnFirst.Enabled = true;
                this.btnBack.Enabled = true;
            }
            //Birendra : Bug-7896 on 20/12/2012 :End:

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int i = this.BindingContext[dsMain.Tables[0]].Position;
            this.BindingContext[dsMain.Tables[0]].Position = this.BindingContext[dsMain.Tables[0]].Count;
            if (this.BindingContext[dsMain.Tables[0]].Position > i)
            {
                this.btnFirst.Enabled = true;
                this.btnBack.Enabled = true;
                //Birendra : Bug-7896 on 20/12/2012 :Start:
                this.btnLast.Enabled = false;
                this.btnForward.Enabled = false;
                //Birendra : Bug-7896 on 20/12/2012 :End:
            }

        }

        private void SetBindingContextPosition(string bindingMode, int newPosition)
        {
            int recCount = this.BindingContext[dsFormInfo, "FormInfo"].Count;
            switch (bindingMode)
            {
                case "First":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = 0;
                    HandleNavigationButtons(false, false, true, true);
                    break;
                case "Previous":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = this.BindingContext[dsFormInfo, "FormInfo"].Position - 1;
                    if (this.BindingContext[dsFormInfo, "FormInfo"].Position == 0)
                        HandleNavigationButtons(false, false, true, true);
                    else
                        HandleNavigationButtons(true, true, true, true);
                    break;
                case "Next":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = this.BindingContext[dsFormInfo, "FormInfo"].Position + 1;
                    if (this.BindingContext[dsFormInfo, "FormInfo"].Position == recCount - 1)
                        HandleNavigationButtons(true, true, false, false);
                    else
                        HandleNavigationButtons(true, true, true, true);
                    break;
                case "Last":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = recCount;
                    if (recCount == 0 || recCount == 1)
                        HandleNavigationButtons(false, false, false, false);
                    else
                        HandleNavigationButtons(true, true, false, false);
                    break;
                case "SelectedRecord":
                    //if (this.BindingContext[dsFormInfo, "FormInfo"].Position != newRowIndex)
                    //this.BindingContext[dsFormInfo, "FormInfo"].Position = newRowIndex;
                    if (newPosition == 0)
                        SetBindingContextPosition("First", 0);
                    else if (newPosition + 1 == recCount)
                        SetBindingContextPosition("Last", 0);
                    else
                    {
                        this.BindingContext[dsFormInfo, "FormInfo"].Position = newPosition;
                        HandleNavigationButtons(true, true, true, true);
                    }
                    break;
            }
        }

        private void HandleNavigationButtons(bool first, bool prev, bool next, bool last)
        {
            btnFirst.Enabled = first;
            btnBack.Enabled = prev;
            btnForward.Enabled = next;
            btnLast.Enabled = last;
        }

        private void HandleButtons(bool newButton, bool editButton, bool cancelButton, bool saveButton, bool deleteButton)
        {
            btnNew.Visible = newButton;
            btnNew.Enabled = newButton;
            btnEdit.Visible = editButton;
            btnEdit.Enabled = editButton;
            btnCancel.Visible = cancelButton;
            btnCancel.Enabled = cancelButton;
            btnSave.Visible = saveButton;
            btnSave.Enabled = saveButton;
            btnDelete.Enabled = deleteButton;
        }

        #endregion

        #region Select Popup


        #endregion

        #region Shortcut Keys

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(this.btnExit, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        #endregion

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            // Birendra : Bug-7710 on 15/12/2012 :Start:
            if (this.txtCatName.Text == "Primary")
            {
                MessageBox.Show("Default category won't be deleted.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // Birendra : Bug-7710 on 15/12/2012 :End:
            int j = this.BindingContext[dsMain.Tables[0]].Position;
            if (MessageBox.Show("Are you sure to delete this Entry ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.pAddMode = false;
                this.pEditMode = false;
                string strSQL = "";
                if (dsMain.Tables[0].Rows[j]["cost_cat_id"].ToString()!="" )
                {
                    DataSet dsData = new DataSet();
                    strSQL = "SELECT top 1 cost_cat_id FROM cost_cen_mast" + " Where cost_cat_id=" + dsMain.Tables[0].Rows[j]["cost_cat_id"].ToString();
                    dsData = oDataAccess.GetDataSet(strSQL, null, 20);

                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("There is one or more dependent entry !!!", this.pPApplText,
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                string vDelString = string.Empty;
                vDelString = "Delete from " + _TableName + " Where Cost_cat_name='" + this.txtCatName.Text+"'";
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);

                // Added By Pankaj B. on 16-02-2015 for Bug-25197 Start
                strSQL = "select cmastcode,dbexport from Tbl_DataExport_Mast where ctype='M' and cmastcode='cost_Cat_mast'";
                dsExport = oDataAccess.GetDataSet(strSQL, null, 25);
                if (dsExport.Tables[0].Rows.Count != 0)
                {
                    ExportTbl = dsExport.Tables[0].Rows[0]["dbexport"].ToString().Trim() + ".dbo." + _TableName;
                    vDelString = "Delete from " + ExportTbl + " Where Cost_cat_name='" + this.txtCatName.Text+"'";
                    oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                }
                // Added By Pankaj B. on 16-02-2015 for Bug-25197 End

                this.dsMain.Tables[0].Rows[j].Delete();
                this.dsMain.Tables[0].AcceptChanges();
                ControlBind(this._TableName); ///added by satish pal for bug- dated 14/06/2013
            }

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        } 
    
        private void btnLocate_Click(object sender, EventArgs e)
        {
            DataSet dsData = new DataSet();
            this.panel2.Top = this.label1.Top + 15; 
            this.panel2.Visible = true;
            this.cmbval.ValueMember = "";
            this.cmbfield.DataBindings.Clear();
            string strSQL = "select Column_name  from INFORMATION_SCHEMA.COLUMNS  where TABLE_NAME = '" + this._TableName + "'";
            dsData = oDataAccess.GetDataSet(strSQL, null, 20);
            dsData.Tables[0].TableName = "TempData";

            this.cmbfield.DataSource = dsData.Tables[0];
            this.cmbfield.DisplayMember = "column_name";
            this.cmbfield.ValueMember = "column_name";
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
//            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
//            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
        }
        public string PMasterCode/*Added Rup 07/03/2011*/
        {
            set
            {
                _MasterCode = value;
            }
            get
            {
                return _MasterCode;
            }
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.pAddMode || this.pEditMode)
            {
                if (MessageBox.Show("Do you want to save your changes first ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.btnSave_Click(sender, e);
                }
                else
                {
                    this.pAddMode = false;
                    this.pEditMode = false;
                    Application.Exit();
                }
            }
        }
        private void utextBox_KeyDown(object sender, KeyEventArgs e)
        {
//            string vHelpQuery = ((uTextBox.cTextBox)sender).pHelpQuery;
//            if ((this.pAddMode || this.pEditMode) && e.KeyCode.ToString() == "F2" && string.IsNullOrEmpty(vHelpQuery) == false)
//            {
//                EventArgs ex = new EventArgs();
////                this.HelpBtn_Click(sender, e);
//            }
        }

        private void MasterForm_KeyUp(object sender, KeyEventArgs e)
        {
            //e.KeyCode.ToString();
        }

        private void MasterForm_KeyDown(object sender, KeyEventArgs e)
        {
            this.errorProvider1.Clear();
            if (e.Control == true)
            {
                EventArgs exf = new EventArgs();
                if (e.KeyCode == Keys.D)
                {
                    if (this.btnDelete.Enabled)
                    this.btnDelete_Click(sender, exf);
                }
                if (e.KeyCode == Keys.E)
                {
                    if (this.btnEdit.Enabled)
                    this.btnEdit_Click(sender, exf);
                }
                if (e.KeyCode == Keys.N)
                {
                    if (this.btnNew.Enabled)
                    this.btnNew_Click(sender, exf);
                }
                if (e.KeyCode == Keys.Z)
                {
                    if (this.btnCancel.Enabled)
                    this.btnCancel_Click(sender, exf);
                }
                if (e.KeyCode == Keys.S)
                {
                    if (this.btnSave.Enabled)
                        this.btnSave_Click(sender, exf);
                }
                if (e.KeyCode == Keys.L)
                {
                    if (this.btnLocate.Enabled)
                    this.btnLocate_Click(sender, exf);
                }
                if (e.KeyCode == Keys.Left)
                {
                    if(this.btnBack.Enabled)
                    this.btnBack_Click(sender, exf);
                }
                if (e.KeyCode == Keys.Right)
                {
                    if (this.btnForward.Enabled)
                    this.btnForward_Click(sender, exf);
                }

                if (e.KeyCode == Keys.F4)
                {
                    if (this.btnExit.Enabled)
                    this.btnExit_Click(sender, exf);
                }

            }
            //MessageBox.Show(e.KeyCode.ToString());
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            if (this.cmbfield.Text != "")
            {
                dsMain.Tables[0].DefaultView.Sort = this.cmbfield.Text;
                if (this.cmbval.Text != "")
                {
                    int i = dsMain.Tables[0].DefaultView.Find(this.cmbval.Text);
                    this.BindingContext[dsMain.Tables[0]].Position = i;
                }
                dsMain.Tables[0].DefaultView.Sort = "";

            }
        }

        private void cmbfield_Leave(object sender, EventArgs e)
        {
            DataSet dsData1 = new DataSet();
            this.cmbval.DataBindings.Clear();
            string strSQL = "select "+this.cmbfield.Text + " from  " + this._TableName ;
            dsData1 = oDataAccess.GetDataSet(strSQL, null, 20);
            dsData1.Tables[0].TableName = "TempData";

            this.cmbval.DataSource = dsData1.Tables[0];
            this.cmbval.DisplayMember = this.cmbfield.Text ;
            this.cmbval.ValueMember = this.cmbfield.Text ;
        }

        private void ChkActivat_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Cost_Cat_Mast_KeyDown(object sender, KeyEventArgs e)
        {
            MasterForm_KeyDown(sender, e);
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
           //added and commented by satish pal for bug-14884 dated -14/12/2013-start
            // this.Dispose();

            if (this.pAddMode || this.pEditMode)
            {
                if (MessageBox.Show("Do you want to save your changes first ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.btnSave_Click(sender, e);
                }
                else
                {
                    this.Dispose();
                }
            }
            else
            {
                this.Dispose();
            }

            //added and commented by satish pal for bug-14884 dated -14/12/2013-start
        }

        private void txtCatName_Validating(object sender, CancelEventArgs e)
        {
            txtCategory_valid();
        }
        private Boolean  txtCategory_valid()
        {
            Boolean zret = false;
            DataSet dsData1 = new DataSet();
            string strSQL = "select cost_Cat_name  from  " + this._TableName + " Where Cost_cat_name= '" + this.txtCatName.Text.ToString() + "'";
            dsData1 = oDataAccess.GetDataSet(strSQL, null, 20);
            if (this.pAddMode == true)
            {
                if (dsData1.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Cost Category Allready Exist!!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.txtCatName.Text = "";
                    this.txtCatName.Focus();
                    return zret;
                }
                //if (this.txtCatName.Text.Length == 0)
                if (this.txtCatName.Text.Trim().Length == 0) //Birendra : Bug-8357 on 17/01/2013 
                {
                    this.errorProvider1.SetError(this.txtCatName, "Category Name can not empty!!!");
                    this.txtCatName.Focus();
                    return zret;
                }
            }
            return true;
        }
        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "UeCost_cat_master.exe";
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

        #region Validation handle
        private Boolean  CheckValidation()
        {
            Boolean zret = false;
            zret = txtCategory_valid();

            return zret;
        }
        #endregion

        #region Error handle
        private void errorhandle(string errmsg)
        {
            MessageBox.Show(errmsg, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            this.Dispose(); 
        }
        #endregion

        private void Cost_Cat_Mast_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (this.pAddMode || this.pEditMode)
            {
                if (MessageBox.Show("Do you want to save your changes first ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.btnSave_Click(sender, e);
                }
                else
                {
                    this.pAddMode = false;
                    this.pEditMode = false;
                    Application.Exit();
                }
            }

        }

        private void txtCatName_Leave(object sender, EventArgs e)
        {
        }
      
    }
}
