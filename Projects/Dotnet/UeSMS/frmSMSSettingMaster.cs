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
using udSelectPop;

namespace UeSMS
{
    public partial class frmSMSSettingMaster : uBaseForm.FrmBaseForm
    {
        clsDataAccess _oDataAccess;
        DataTable _dtSMSSetting, _dtURLParam;
        string Mode;
        //**** Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- Start
        int ApplPId = 0;
        string ApplCode, AppCaption, cAppPId;
        string cAppName = "";
        //**** Added by Sachin N. S. on 31/03/2015 for Bug-25365 -- End

        public frmSMSSettingMaster(string[] args)
        {
            InitializeComponent();

            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "SMS Setting Master";

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

        private void frmSMSSettingMaster_Load(object sender, EventArgs e)
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

            _dtSMSSetting = new DataTable();

            _dtURLParam = new DataTable();
            _dtURLParam.Columns.Add(new DataColumn("ParamName", typeof(string)));
            _dtURLParam.Columns.Add(new DataColumn("ParamDesc", typeof(string)));
            _dtURLParam.Columns.Add(new DataColumn("ParamVal", typeof(string)));
            _dtURLParam.Columns.Add(new DataColumn("ParamEncr", typeof(string)));

            dgvURLParam.AutoGenerateColumns = false;
            dgvURLParam.DataSource = _dtURLParam;
            dgvURLParam.Columns[0].DataPropertyName = "ParamName";
            dgvURLParam.Columns[1].DataPropertyName = "ParamDesc";
            dgvURLParam.Columns[2].DataPropertyName = "";
            //dgvURLParam.Columns[3].DataPropertyName = "ParamEncr";

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
            _dtSMSSetting.Rows.Clear();
            _dtURLParam.Rows.Clear();

            DataRow _dr = _dtSMSSetting.NewRow();
            _dr[0] = 0;
            _dr[1] = 1;
            _dr[2] = "";
            _dr[3] = 0;
            _dr[4] = "";
            _dr[5] = "";
            _dr[6] = 0;
            _dr[7] = 0;
            _dr[8] = 0;
            _dr[9] = "";
            _dr[10] = "";
            _dr[11] = "";
            _dtSMSSetting.Rows.Add(_dr);

            this.rbtnSMSGateway.Checked = true;

            enableDisablecontrol(true);
            enableDisableTlBar();
            this.txtSMSGtWayNm.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_dtSMSSetting.Rows.Count <= 0)
            {
                MessageBox.Show("No records to edit.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Mode = "EDIT";
            _dtSMSSetting.Rows[0].BeginEdit();
            enableDisablecontrol(true);
            enableDisableTlBar();
            this.txtSMSURL.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_dtSMSSetting.Rows.Count <= 0)
            {
                MessageBox.Show("No records to delete.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string strrep = "";
            bool lUpdate = false;
            _oDataAccess.BeginTransaction();
            strrep = "DELETE FROM SMSSETTINGMASTER WHERE SMSSETID=" + _dtSMSSetting.Rows[0]["SMSSETID"].ToString();
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

        private void rbtnSMSGateway_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tbCntrl.TabPages["tbPgSMSGtWay"] == null)
                this.tbCntrl.TabPages.Add(tbPgSMSGtWay);
            this.tbCntrl.TabPages.Remove(tbPgMobModem);
            //_dtSMSSetting.Rows[0]["SMSVIA"] = 1;
        }

        private void rbtnMobModem_CheckedChanged(object sender, EventArgs e)
        {
            this.tbCntrl.TabPages.Remove(tbPgSMSGtWay);
            if (this.tbCntrl.TabPages["tbPgMobModem"] == null)
                this.tbCntrl.TabPages.Add(tbPgMobModem);
            //_dtSMSSetting.Rows[0]["SMSVIA"] = 2;
        }

        private void btnSMSCode_Click(object sender, EventArgs e)
        {
            string SqlStr = "";
            short vTimeOut = 50;
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select * From SMSPARAMMASTER";
            tDs = _oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select SMS Gateway Master";
            vSearchCol = "SMSGTWAYID";
            vDisplayColumnList = "SMSGTWAYID:SMS Gateway Name";
            vReturnCol = "SMSCODE,SMSGTWAYID,SMSURL,SMSPARAM";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtSMSCode.Text = oSelectPop.pReturnArray[0];
                this.txtSMSGtWayId.Text = oSelectPop.pReturnArray[1];
                this.txtSMSURL.Text = oSelectPop.pReturnArray[2];

                _dtSMSSetting.Rows[0]["SMSCODE"] = oSelectPop.pReturnArray[0];
                _dtSMSSetting.Rows[0]["SMSGTWAYID"] = oSelectPop.pReturnArray[1];
                _dtSMSSetting.Rows[0]["SMSURL"] = oSelectPop.pReturnArray[2];
                _dtSMSSetting.Rows[0]["SMSPARAM"] = oSelectPop.pReturnArray[3];
                addFillGrid();
            }
        }

        private void dgvURLParam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvURLParam.Columns["colParamVal"].Index && e.RowIndex >= 0)
            {
                frmParamValue _fv = new frmParamValue(this.pPara, _dtURLParam, e.RowIndex, Mode);
                _fv.ShowDialog();
            }
        }

        private void frmSMSSettingMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }
        //******* Tool Bar Events -- End ********//

        //******* Private Methods -- Start *******//
        private void enableDisablecontrol(bool _EnblDisable)
        {
            txtSMSSetID.Enabled = false;
            txtSMSGtWayNm.Enabled = Mode == "ADD" ? _EnblDisable : false;
            grpSMSVia.Enabled = Mode == "ADD" ? _EnblDisable : false;
            rbtnSMSGateway.Enabled = Mode == "ADD" ? _EnblDisable : false;
            rbtnMobModem.Enabled = Mode == "ADD" ? _EnblDisable : false;
            btnSMSCode.Enabled = Mode == "ADD" ? _EnblDisable : false;

            txtPortName.Enabled = _EnblDisable;
            txtBaudRate.Enabled = _EnblDisable;
            txtSMSSimNo.Enabled = _EnblDisable;
            txtTimeOut.Enabled = _EnblDisable;

            dgvURLParam.ReadOnly = !_EnblDisable;
            dgvURLParam.Columns[0].ReadOnly = true;
            dgvURLParam.Columns[1].ReadOnly = true;
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
            string _paraId = this.txtSMSSetID.Text;

            DataSet _ds = new DataSet();
            DataTable dt;
            string _NaviBtn = "0000", strrep = "";
            strrep = "EXECUTE NAVIGATE_SMS_PARA_SETTING_MASTER '" + _naviPos + "', " + (_paraId == "" ? "0" : _paraId.ToString()) + "";
            _ds = _oDataAccess.GetDataSet(strrep, null, 50);
            dt = _ds.Tables[0];
            _NaviBtn = _ds.Tables[1].Rows[0][0].ToString();

            if (_dtSMSSetting != null)
            {
                _dtSMSSetting.Rows.Clear();
            }

            _dtSMSSetting.Merge(dt);

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
            this.txtSMSSetID.DataBindings.Clear();
            this.txtSMSGtWayNm.DataBindings.Clear();

            this.txtSMSCode.DataBindings.Clear();
            this.txtSMSGtWayId.DataBindings.Clear();
            this.txtSMSURL.DataBindings.Clear();

            this.txtPortName.DataBindings.Clear();
            this.txtBaudRate.DataBindings.Clear();
            this.txtSMSSimNo.DataBindings.Clear();
            this.txtTimeOut.DataBindings.Clear();

            this.txtSMSSetID.DataBindings.Add("Text", _dtSMSSetting, "SMSSetID");
            this.txtSMSGtWayNm.DataBindings.Add("Text", _dtSMSSetting, "SMSGtWayNm");

            this.txtSMSCode.DataBindings.Add("Text", _dtSMSSetting, "SMSCode");
            this.txtSMSGtWayId.DataBindings.Add("Text", _dtSMSSetting, "SMSGtWayId");
            this.txtSMSURL.DataBindings.Add("Text", _dtSMSSetting, "SMSURL");

            this.txtPortName.DataBindings.Add("Text", _dtSMSSetting, "PortName");
            this.txtBaudRate.DataBindings.Add("Text", _dtSMSSetting, "BaudRate");
            this.txtSMSSimNo.DataBindings.Add("Text", _dtSMSSetting, "SMSSimNo");
            this.txtTimeOut.DataBindings.Add("Text", _dtSMSSetting, "TimeOut");

            if (_dtSMSSetting.Rows.Count > 0)
            {
                if (_dtSMSSetting.Rows[0]["SMSVIA"].ToString() == "1")
                    this.rbtnSMSGateway.Checked = true;
                else
                    this.rbtnSMSGateway.Checked = false;

                if (_dtSMSSetting.Rows[0]["SMSVIA"].ToString() == "2")
                    this.rbtnMobModem.Checked = true;
                else
                    this.rbtnMobModem.Checked = false;
            }
        }

        private void addFillGrid()
        {
            if (_dtURLParam != null)
                _dtURLParam.Rows.Clear();

            if (_dtSMSSetting.Rows.Count <= 0)
                return;
            string[] _UrlParam = _dtSMSSetting.Rows[0]["SMSPARAM"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            DataRow _dr;
            string _cParam = "";
            for (int i = 0; i < _UrlParam.Length; i++)
            {
                _cParam = _UrlParam[i].ToString();
                _dr = _dtURLParam.NewRow();
                _dr[0] = _cParam.Substring(0, _cParam.IndexOf(":"));
                _cParam = _cParam.Replace(_dr[0].ToString() + ":", "");
                _dr[3] = _cParam.Substring(0, _cParam.IndexOf(":"));
                _cParam = _cParam.Replace(_dr[3].ToString() + ":", "");
                _dr[1] = _cParam;
                _dtURLParam.Rows.Add(_dr);
            }
        }

        private void fillGrid()
        {
            if (_dtURLParam != null)
                _dtURLParam.Rows.Clear();

            if (_dtSMSSetting.Rows.Count <= 0)
                return;

            //string[] _UrlParam = _dtSMSSetting.Rows[0]["SMSPARAM"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] _UrlParam = _dtSMSSetting.Rows[0]["SMSPARAM"].ToString().Split(new char[] { ',' });

            string paravl = Utilities.dec(_dtSMSSetting.Rows[0]["SMSPARAVL"].ToString());
            //string[] _UrlParamVal = Utilities.dec(_dtSMSSetting.Rows[0]["SMSPARAVL"].ToString()).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] _UrlParamVal = Utilities.dec(_dtSMSSetting.Rows[0]["SMSPARAVL"].ToString()).Split(new char[] { ',' });

            if (_UrlParam.Length != _UrlParamVal.Length)
            {
                //MessageBox.Show("Parameter length and the parameter value length doesnot match.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow _dr;
            string _cParam = "", _cParamVl = "";
            for (int i = 0; i < _UrlParam.Length; i++)
            {
                _cParam = _UrlParam[i].ToString();
                _cParamVl = _UrlParamVal.Length == 0 ? "" : _UrlParamVal[i].ToString();
                _dr = _dtURLParam.NewRow();
                _dr[0] = _cParam.Substring(0, _cParam.IndexOf(":"));
                _cParam = _cParam.Replace(_dr[0].ToString() + ":", "");
                _dr[3] = _cParam.Substring(0, _cParam.IndexOf(":"));
                _cParam = _cParam.Replace(_dr[3].ToString() + ":", "");
                _dr[1] = _cParam;
                _dr[2] = _cParamVl;
                _dtURLParam.Rows.Add(_dr);
            }
        }

        private bool SaveIt()
        {
            if ((this.dgvURLParam.ContainsFocus == false && this.ActiveControl.Name == "dgvURLParam") || this.ActiveControl.GetType().Name.ToString() == "TextBox")
                this.ActiveControl.DataBindings[0].WriteValue();

            _dtSMSSetting.Rows[0]["SMSVIA"] = rbtnSMSGateway.Checked == true ? "1" : "2";

            _dtSMSSetting.AcceptChanges();
            _dtURLParam.AcceptChanges();

            if (Mode != "ADD" && Mode != "EDIT")
            {
                MessageBox.Show("Cannot save the record.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (Mode == "ADD")
            {
                if (this.txtSMSGtWayNm.Text == "")
                {
                    MessageBox.Show("SMS Setting name cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (this.rbtnSMSGateway.Checked == true)
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

                if (this.rbtnMobModem.Checked == true)
                {
                    if (this.txtPortName.Text == "")
                    {
                        MessageBox.Show("Port name cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (this.txtBaudRate.Text == "")
                    {
                        MessageBox.Show("Baud rate cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (this.txtTimeOut.Text == "")
                    {
                        MessageBox.Show("Timeout cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (this.txtSMSSimNo.Text == "")
                    {
                        MessageBox.Show("SIM number cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            if (Mode == "EDIT")
            {
                if (this.rbtnSMSGateway.Checked == true)
                {
                    if (this.txtSMSURL.Text == "")
                    {
                        MessageBox.Show("URL cannot be empty.", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            string cUrlParam = "";
            int _i1 = 1;
            foreach (DataRow _dr in _dtURLParam.Rows)
            {
                cUrlParam += (_i1 == 1 ? "" : ",") + _dr[2].ToString().Trim();
                _i1 += 1;
            }
            _dtSMSSetting.Rows[0]["SMSPARAVL"] = Utilities.enc(cUrlParam);

            int _smsCode = 0;
            bool lUpdate = false;
            _oDataAccess.BeginTransaction();
            string strrep = "";
            if (Mode == "ADD")
            {
                strrep = "INSERT INTO SMSSETTINGMASTER (SMSVIA,SMSGTWAYNM,SMSCODE,SMSPARAVL,PORTNAME,BAUDRATE,TIMEOUT,SMSSIMNO) VALUES(" + _dtSMSSetting.Rows[0]["SMSVIA"].ToString() + ",'" + _dtSMSSetting.Rows[0]["SMSGTWAYNM"].ToString() + "','" + _dtSMSSetting.Rows[0]["SMSCODE"].ToString() + "','" + _dtSMSSetting.Rows[0]["SMSPARAVL"].ToString() + "','" + _dtSMSSetting.Rows[0]["PORTNAME"].ToString() + "'," + _dtSMSSetting.Rows[0]["BAUDRATE"].ToString() + "," + _dtSMSSetting.Rows[0]["TIMEOUT"].ToString() + ",'" + _dtSMSSetting.Rows[0]["SMSSIMNO"].ToString() + "') ";
                lUpdate = false;
            }

            if (Mode == "EDIT")
            {
                strrep = "UPDATE SMSSETTINGMASTER SET SMSGTWAYNM='" + _dtSMSSetting.Rows[0]["SMSGTWAYNM"].ToString() + "', SMSPARAVL='" + _dtSMSSetting.Rows[0]["SMSPARAVL"].ToString() + "',PORTNAME='" + _dtSMSSetting.Rows[0]["PORTNAME"].ToString() + "',BAUDRATE='" + _dtSMSSetting.Rows[0]["BAUDRATE"].ToString() + "',TIMEOUT='" + _dtSMSSetting.Rows[0]["TIMEOUT"].ToString() + "',SMSSIMNO='" + _dtSMSSetting.Rows[0]["SMSSIMNO"].ToString() + "' WHERE SMSSETID=" + _dtSMSSetting.Rows[0]["SMSSETID"].ToString();
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

        private void SetGridImage()
        {
            // set images in data gridview

            //for (int r = 0; r < dgvURLParam.Rows.Count; r++)
            //{
            //    int docid = Convert.ToInt32(dgvURLParam.Rows[r].Cells[1].Value);
            //    string filename = string.Empty;
            //    if (docid != 0)
            //    {
            //        var id = m_obj_DocumentMaster.ListDoc_Mstr.Find(docmstr => docmstr.Doc_id == docid);
            //        filename = id.Doc_filename;
            //    }

            //    if (filename != string.Empty)
            //    {
            //        FileInfo file = new FileInfo(filename);
            //        DataGridViewImageCell cell = (DataGridViewImageCell)dgvURLParam.Rows[r].Cells[0];
            //        DataGridViewTextBoxCell cellid = (DataGridViewTextBoxCell)dgvURLParam.Rows[r].Cells[1];
            //        DataGridViewTextBoxCell cellname = (DataGridViewTextBoxCell)dgvURLParam.Rows[r].Cells[2];

            //        cell.ToolTipText = "Double click for view document..";
            //        cellid.ToolTipText = "Double click for modify details..";
            //        cellname.ToolTipText = "Double click for modify details..";

            //        switch (file.Extension.ToString().Trim().ToUpper())
            //        {
            //            case ".PDF":
            //                cell.Value = imagelist.Images[0];
            //                break;
            //            case ".XLS":
            //            case ".XLSX":
            //                cell.Value = imagelist.Images[1];
            //                break;
            //            case ".DOC":
            //            case ".DOCX":
            //                cell.Value = imagelist.Images[2];
            //                break;
            //            case ".JPG":
            //            case ".BMP":
            //            case ".GIF":
            //            case ".TIF":
            //            case ".PNG":
            //                cell.Value = imagelist.Images[3];
            //                break;
            //            case ".PPT":
            //            case ".PPS":
            //                cell.Value = imagelist.Images[4];
            //                break;
            //            case ".TXT":
            //                cell.Value = imagelist.Images[5];
            //                break;
            //            default:
            //                cell.Value = imagelist.Images[6];
            //                break;
            //        }
            //    }
            //}
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

        #region Generate Process Id's       
        // Added by Sachin N. S. on 30/03/2015 for Bug-25365 -- Start
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

        // Added by Sachin N. S. on 30/03/2015 for Bug-25365 -- End
        //******* Private Methods -- End *******//
    }
}
