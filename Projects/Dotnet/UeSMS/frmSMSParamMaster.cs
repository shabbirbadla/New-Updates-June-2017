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
using System.Diagnostics;
using uBaseForm;
using DataAccess_Net;

namespace UeSMS
{
    public partial class frmSMSParamMaster : uBaseForm.FrmBaseForm
    {
        clsDataAccess _oDataAccess;
        DataTable _dtSMSGateWay, _dtURLParam;
        string Mode;
        //**** Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- Start
        int ApplPId = 0;
        string ApplCode, AppCaption, cAppPId;
        string cAppName = "";
        //**** Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- End

        public frmSMSParamMaster(string[] args)
        {

            InitializeComponent();

            this.pPApplPID = 0;
            this.pPara = args;

            this.pFrmCaption = "SMS Gateway Master";
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

        private void frmSMSParamMaster_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            Mode = "VIEW";

            enableDisablecontrol(false);
            enableDisableTlBar();

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            _oDataAccess = new DataAccess_Net.clsDataAccess();

            _dtSMSGateWay = new DataTable();

            _dtURLParam = new DataTable();
            _dtURLParam.Columns.Add(new DataColumn("ParamName", typeof(string)));
            _dtURLParam.Columns.Add(new DataColumn("ParamEncr", typeof(bool)));
            _dtURLParam.Columns.Add(new DataColumn("ParamDesc", typeof(string)));

            dgvURLParam.AutoGenerateColumns = false;
            dgvURLParam.DataSource = _dtURLParam;
            dgvURLParam.Columns[0].DataPropertyName = "ParamName";
            dgvURLParam.Columns[1].DataPropertyName = "ParamEncr";
            dgvURLParam.Columns[2].DataPropertyName = "ParamDesc";

            this.mInsertProcessIdRecord();      // Added by Sachin N. S. on 30/03/2015 for Bug-25365

            NavigateData("LAST");

        }

        //******* Tool Bar Events -- Start *******//
        private void btnFirst_Click(object sender, EventArgs e)
        {
            NavigateData("FIRST");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            NavigateData("PREVIOUS");
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            NavigateData("NEXT");
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            NavigateData("LAST");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Mode = "ADD";
            _dtSMSGateWay.Rows.Clear();
            _dtURLParam.Rows.Clear();

            DataRow _dr = _dtSMSGateWay.NewRow();
            _dr[0] = 0;
            _dr[1] = "";
            _dr[2] = "";
            _dr[3] = "";
            _dtSMSGateWay.Rows.Add(_dr);

            enableDisablecontrol(true);
            enableDisableTlBar();
            this.txtSMSGtWayId.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_dtSMSGateWay.Rows.Count <= 0)
            {
                MessageBox.Show("No records to edit.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Mode = "EDIT";
            _dtSMSGateWay.Rows[0].BeginEdit();
            enableDisablecontrol(true);
            enableDisableTlBar();
            this.txtSMSURL.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_dtSMSGateWay.Rows.Count <= 0)
            {
                MessageBox.Show("No records to delete.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string strrep = "";
            bool lUpdate = false;
            _oDataAccess.BeginTransaction();
            strrep = "DELETE FROM SMSPARAMMASTER WHERE SMSCODE=" + _dtSMSGateWay.Rows[0]["SMSCODE"].ToString();
            lUpdate = true;

            try
            {
                _oDataAccess.ExecuteSQLStatement(strrep, null, 50, lUpdate);
                _oDataAccess.CommitTransaction();
                MessageBox.Show("Record deleted successfully...!!!", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _oDataAccess.RollbackTransaction();
                MessageBox.Show("Could not delete the record...!!!\n" + ex.Message, this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            NavigateData("LAST");
            enableDisablecontrol(false);
            enableDisableTlBar();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveIt() == false)
                return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Mode = "VIEW";
            NavigateData("LAST");
            enableDisablecontrol(false);
            enableDisableTlBar();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddParam_Click(object sender, EventArgs e)
        {
            DataRow _dr = _dtURLParam.NewRow();
            _dr[0] = "";
            _dr[1] = false;
            _dr[2] = "";
            _dtURLParam.Rows.Add(_dr);
            this.BindingContext[_dtURLParam].Position = _dtURLParam.Rows.Count;
            int i = this.BindingContext[_dtURLParam].Position;
            dgvURLParam.CurrentCell = dgvURLParam.Rows[i].Cells[0];
            dgvURLParam.Focus();
        }

        private void btnDelParam_Click(object sender, EventArgs e)
        {
            if (_dtURLParam.Rows.Count > 0)
                _dtURLParam.Rows[this.BindingContext[_dtURLParam].Position].Delete();
        }
        //******* Tool Bar Events -- End ********//

        //******* Private Methods -- Start *******//
        private void enableDisablecontrol(bool _EnblDisable)
        {
            txtSMSCode.Enabled = false;
            txtSMSGtWayId.Enabled = Mode == "ADD" ? _EnblDisable : false;
            txtSMSURL.Enabled = _EnblDisable;

            btnAddParam.Enabled = _EnblDisable;
            btnDelParam.Enabled = _EnblDisable;
            dgvURLParam.ReadOnly = !_EnblDisable;
        }

        private void enableDisableTlBar()
        {
            if (Mode == "ADD" || Mode == "EDIT")
                NavButtonState(false, false, false, false);

            btnNew.Enabled = Mode == "ADD" || Mode == "EDIT" ? false : true;
            btnEdit.Enabled = Mode == "ADD" || Mode == "EDIT" ? false : true;
            btnDelete.Enabled = Mode == "VIEW" ? true : false;

            btnSave.Enabled = Mode == "ADD" || Mode == "EDIT" ? true : false;
            btnCancel.Enabled = Mode == "ADD" || Mode == "EDIT" ? true : false;

            btnCalculator.Enabled = false;
            btnEmail.Enabled = false;
            btnExportPdf.Enabled = false;
            btnHelp.Enabled = false;
            btnLocate.Enabled = false;
            btnLogout.Enabled = false;
            btnPreview.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void NavigateData(string _naviPos)
        {
            string _paraId = this.txtSMSCode.Text;

            DataSet _ds = new DataSet();
            DataTable dt;
            string _NaviBtn = "0000", strrep = "";
            strrep = "EXECUTE NAVIGATE_SMS_PARA_MASTER '" + _naviPos + "', " + (_paraId == "" ? "0" : _paraId.ToString()) + "";
            _ds = _oDataAccess.GetDataSet(strrep, null, 50);
            dt = _ds.Tables[0];
            _NaviBtn = _ds.Tables[1].Rows[0][0].ToString();

            if (_dtSMSGateWay != null)
            {
                _dtSMSGateWay.Rows.Clear();
            }

            _dtSMSGateWay.Merge(dt);

            setDataBinding();

            fillGrid();

            NavButtonState(_NaviBtn.Substring(0, 1) == "1" ? true : false, _NaviBtn.Substring(1, 1) == "1" ? true : false, _NaviBtn.Substring(2, 1) == "1" ? true : false, _NaviBtn.Substring(3, 1) == "1" ? true : false);
        }

        private void NavButtonState(bool First, bool Prev, bool Next, bool Last)
        {
            btnFirst.Enabled = First;
            btnBack.Enabled = Prev;
            btnForward.Enabled = Next;
            btnLast.Enabled = Last;
        }

        private void setDataBinding()
        {
            this.txtSMSCode.DataBindings.Clear();
            this.txtSMSGtWayId.DataBindings.Clear();
            this.txtSMSURL.DataBindings.Clear();

            this.txtSMSCode.DataBindings.Add("Text", _dtSMSGateWay, "SMSCode");
            this.txtSMSGtWayId.DataBindings.Add("Text", _dtSMSGateWay, "SMSGTWAYID");
            this.txtSMSURL.DataBindings.Add("Text", _dtSMSGateWay, "SMSURL");
        }

        private void fillGrid()
        {
            if (_dtURLParam != null)
                _dtURLParam.Rows.Clear();

            if (_dtSMSGateWay.Rows.Count <= 0)
                return;
            string[] _UrlParam = _dtSMSGateWay.Rows[0]["SMSPARAM"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            DataRow _dr;
            string _cParam1 = "";
            foreach (string _cParam in _UrlParam)
            {
                _cParam1 = _cParam;
                _dr = _dtURLParam.NewRow();
                _dr[0] = _cParam1.Substring(0, _cParam1.IndexOf(":"));
                _cParam1 = _cParam1.Replace(_dr[0].ToString() + ":", "");
                _dr[1] = _cParam1.Substring(0, _cParam1.IndexOf(":"));
                _cParam1 = _cParam1.Replace(_dr[1].ToString() + ":", "");
                _dr[2] = _cParam1;
                _dtURLParam.Rows.Add(_dr);
            }
        }

        private bool SaveIt()
        {
            dgvURLParam.EndEdit();
            if (this.dgvURLParam.ContainsFocus == false)
                this.ActiveControl.DataBindings[0].WriteValue();
            _dtSMSGateWay.AcceptChanges();
            _dtURLParam.AcceptChanges();

            if (Mode != "ADD" && Mode != "EDIT")
            {
                MessageBox.Show("Cannot save the record.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (Mode == "ADD")
            {
                if (this.txtSMSGtWayId.Text == "")
                {
                    MessageBox.Show("SMS Gateway cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (this.txtSMSURL.Text == "")
                {
                    MessageBox.Show("URL cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (Mode == "EDIT")
            {
                if (this.txtSMSURL.Text == "")
                {
                    MessageBox.Show("URL cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            string cUrlParam = "";
            foreach (DataRow _dr in _dtURLParam.Rows)
            {
                cUrlParam += (cUrlParam == "" ? "" : ",") + _dr[0].ToString().Trim() + ":" + _dr[1].ToString().Trim() + ":" + _dr[2].ToString().Trim();
            }
            _dtSMSGateWay.Rows[0]["SMSPARAM"] = cUrlParam;

            int _smsCode = 0;
            bool lUpdate = false;
            _oDataAccess.BeginTransaction();
            string strrep = "";
            if (Mode == "ADD")
            {
                strrep = "INSERT INTO SMSPARAMMASTER (SMSGTWAYID,SMSURL,SMSPARAM) VALUES('" + _dtSMSGateWay.Rows[0]["SMSGTWAYID"].ToString() + "','" + _dtSMSGateWay.Rows[0]["SMSURL"].ToString() + "','" + _dtSMSGateWay.Rows[0]["SMSPARAM"].ToString() + "') ";
                lUpdate = false;
            }

            if (Mode == "EDIT")
            {
                strrep = "UPDATE SMSPARAMMASTER SET SMSURL='" + _dtSMSGateWay.Rows[0]["SMSURL"].ToString() + "', SMSPARAM='" + _dtSMSGateWay.Rows[0]["SMSPARAM"].ToString() + "' WHERE SMSCODE=" + _dtSMSGateWay.Rows[0]["SMSCODE"].ToString();
                lUpdate = true;
            }

            try
            {
                _smsCode = _oDataAccess.ExecuteSQLStatement(strrep, null, 50, lUpdate);
                if (_smsCode >= 0)
                {
                    _oDataAccess.CommitTransaction();
                    MessageBox.Show("Record saved successfully...!!!", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _oDataAccess.RollbackTransaction();
                    MessageBox.Show("Record cannot be saved...!!!", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _oDataAccess.RollbackTransaction();
                MessageBox.Show("Could not save the record...!!!\n" + ex.Message, this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            Mode = "VIEW";
            enableDisablecontrol(false);
            enableDisableTlBar();
            NavigateData("LAST");
            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.N):
                    this.btnNew.PerformClick();
                    break;
                case (Keys.Control | Keys.E):
                    this.btnEdit.PerformClick();
                    break;
                case (Keys.Control | Keys.D):
                    this.btnDelete.PerformClick();
                    break;
                case (Keys.Control | Keys.S):
                    this.btnSave.PerformClick();
                    break;
                case (Keys.Control | Keys.Z):
                    this.btnCancel.PerformClick();
                    break;
                case (Keys.Control | Keys.F4):
                    this.btnExit.PerformClick();
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private void dgvURLParam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvURLParam.EndEdit();
        }

        // Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- Start
        private void frmSMSParamMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }
        // Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- End

        #region Generate Process Id's       
        // Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- Start
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            _oDataAccess = new clsDataAccess();
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ueSMS.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + AppCaption + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            _oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()
        {
            _oDataAccess = new clsDataAccess();
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            _oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        #endregion Generate Process Id's    

        // Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- End

        //******* Private Methods -- End *******//
    }
}
