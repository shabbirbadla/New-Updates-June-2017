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
using DataAccess_Net;

namespace PointofSale
{
    public partial class udFrmPointofSale : Form
    {
        clsDataAccess _oDataAccess;
        DataSet _commonDs;

        #region Default Constructor and Screen Load
        public udFrmPointofSale()
        {
            InitializeComponent();
            clsScreenPropEvents.OrgHgth = this.Height;
            clsScreenPropEvents.OrgWdth = this.Width;
        }

        private void udFrmPointofSale_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.Icon = new Icon(CommonInfo.IconPath.ToString().Replace("<*#*>", " "));
            this.txtUserName.Text = CommonInfo.UserName;
            this.txtInvoiceSr.Text = CommonInfo.InvSeries;
            this.txtCategory.Text = CommonInfo.Category;
            this.txtDepartment.Text = CommonInfo.Department;

            //*** Get Data Cursor -- Start ***\\
            getDataCursor();
            //*** Get Data Cursor -- End ***\\

            //*** Binding data to controls -- Start ***\\
            BindControls();
            //*** Binding data to controls -- End ***\\

            clsInsertDefaultValue._DataSet = _commonDs;

            //*** Adding Records to Dataset -- Start ***\\
            addHdrRecords();
            //*** Adding Records to Dataset -- End ***\\

            clsScreenPropEvents.NewHgth = this.Height;
            clsScreenPropEvents.NewWdth = this.Width;
            clsScreenPropEvents.ResizeForm(this);

            dgvItemDetails.Focus();
            SendKeys.Send("{Tab}");
        }
        #endregion Default Constructor and Screen Load

        #region Control Events
        private void dgvItemDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvItemDetails.Columns[dgvItemDetails.CurrentCell.ColumnIndex].Name.ToString() == "colItem")
            {
                if (e.KeyCode == Keys.F2)
                {
                    popUpFromTable("ITEM");
                    _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 1;
                    SendKeys.Send("{Tab}");
                    //addChldRecords("Item");
                }
            }
            RefreshControls();
            //dgvItemDetails.Refresh();
        }

        private void dgvItemDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _commonDs.Tables["Item"].Rows[e.RowIndex].EndEdit();
            this.RefreshControls();

            if (dgvItemDetails.Columns[e.ColumnIndex].Name.ToString()=="colQuantity")
            {
                this.addChldRecords("Item");
                this.dgvItemDetails.Refresh();
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (_commonDs.Tables["Main"].Rows.Count > 0)
            {
                this.BindingContext[_commonDs, "Main"].EndCurrentEdit();
                this.DeleteChldRecords("Item");
                _commonDs.Tables["Main"].AcceptChanges();
                _commonDs.Tables["Item"].AcceptChanges();
                udFrmPaymentScreen _frmPayScrn = new udFrmPaymentScreen(_commonDs);
                _frmPayScrn.ShowDialog();
                if (_frmPayScrn._RetValue == "SAVED")
                {
                    clearTables();
                    addHdrRecords();
                    dgvItemDetails.Focus();
                    SendKeys.Send("{Tab}");
                }
                this.RefreshControls();
            }
            else
            {
                MessageBox.Show("No records found for payment.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Control Events

        #region Private Methods

        #region Build dataset and Refresh controls
        private void getDataCursor()
        {
            _oDataAccess = new clsDataAccess();
            //List<clsParam> _clparam = new List<clsParam>();
            //_clparam.Add(new clsParam { FieldName = "@Entry_ty1", Value="PS"});
            //_clparam.Add(new clsParam { FieldName = "@Tran_cd", Value = 0 });
            _commonDs = new DataSet();

            _commonDs = _oDataAccess.GetDataSet("Execute UD_GET_PS_DATASET 'PS',0", null, 25);
            _commonDs.Tables[0].TableName = "Lcode";
            _commonDs.Tables[1].TableName = "Main";
            _commonDs.Tables[2].TableName = "Item";
            _commonDs.Tables[3].TableName = "PSPayDetail";
            //_commonDs.Tables[3].TableName = "Acdet";
            //_commonDs.Tables[4].TableName = "Mall";
            //_commonDs.Tables[5].TableName = "Itref";
        }

        private void RefreshControls()
        {
            txtTotalQty.Text  = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Qty"] != null).Sum(row => Convert.ToDecimal(row["Qty"])),2).ToString();
            txtNoOfItems.Text = _commonDs.Tables["Item"].AsEnumerable().Count().ToString();
            txtGrossAmt.Text  = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["U_asseamt"] != null).Sum(row => Convert.ToDecimal(row["U_asseamt"])),2).ToString();
            txtDiscAmt.Text   = ""; //_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Qty"] != null).Sum(row => Convert.ToDecimal(row["Qty"])).ToString();
            txtNetAmt.Text    = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Gro_amt"] != null).Sum(row => Convert.ToDecimal(row["Gro_amt"])),2).ToString();
            this.Refresh();
        }
        #endregion Build dataset and Refresh controls

        #region Clear and reset Datatables
        private void clearTables()
        { 
            foreach(DataTable _dt in _commonDs.Tables)
            {
                if (_dt.TableName != "Lcode")
                {
                    _dt.Clear();
                }
            }
        }
        #endregion Clear and reset Datatables

        #region Binding Controls
        private void BindControls()
        {
            txtInvoiceNo.DataBindings.Add("text", _commonDs, "Main.Inv_no");
            txtInvoiceDt.DataBindings.Add("text", _commonDs, "Main.Date");
            txtCategory.DataBindings.Add("text", _commonDs, "Main.Cate");
            txtDepartment.DataBindings.Add("text", _commonDs, "Main.dept");
            txtInvoiceSr.DataBindings.Add("text", _commonDs, "Main.Inv_sr");
            txtUserName.DataBindings.Add("text", _commonDs, "Main.user_Name");
            txtGrossAmt.DataBindings.Add("text", _commonDs, "Main.Gro_amt");
            txtNetAmt.DataBindings.Add("text",_commonDs,"Main.Net_amt");

            gridBinding();
        }

        private void gridBinding()
        {
            // ***** Binding Item Grid -- Start ***** \\
            dgvItemDetails.AutoGenerateColumns = false;
            dgvItemDetails.ClearSelection();
            dgvItemDetails.DataSource = _commonDs.Tables["Item"];

            dgvItemDetails.Columns[0].DataPropertyName = "Item_no";
            dgvItemDetails.Columns[1].DataPropertyName = "Item";
            dgvItemDetails.Columns[2].DataPropertyName = "Qty";
            dgvItemDetails.Columns[3].DataPropertyName = "Rate";
            dgvItemDetails.Columns[4].DataPropertyName = "Item_no";
            dgvItemDetails.Columns[5].DataPropertyName = "Gro_amt";

            _commonDs.Tables["Item"].Columns["U_asseamt"].Expression = "Qty*Rate";
            _commonDs.Tables["Item"].Columns["Gro_amt"].Expression = "Qty*Rate";

            dgvItemDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            // ***** Binding Item Grid -- End ***** \\

            // ***** Binding Payment mode Grid -- Start ***** \\
            dgvPayDetails.AutoGenerateColumns = false;
            dgvPayDetails.ClearSelection();
            dgvPayDetails.DataSource = _commonDs.Tables["PSPayDetail"];

            dgvPayDetails.Columns[0].DataPropertyName = "Paymode";
            dgvPayDetails.Columns[1].DataPropertyName = "TotalValue";
            // ***** Binding Item Grid -- End ***** \\
        }
        #endregion

        #region Calling Popup Selection Screen
        private void popUpFromTable(string _popupType)
        {
            _oDataAccess = new clsDataAccess();
            string cSql = "";
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            DataTable _dt = new DataTable();
            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            switch (_popupType)
            {
                case "ITEM":
                    string cTbl = "", cCond = "", cFldList = "", xcFldList = "", xcSrchCol = "", xcDispCol = "", xcRetCol = "";
                    xcFldList = ",It_mast.Rate ";
                    xcDispCol = ",Rate:Item Rate";
                    xcRetCol = ",Rate";
                    cTbl = " It_Mast ";
                    cCond = " It_code!=0 ";

                    //******* Checking for Rate in Item Rate master -- Start *******\\
                    if ((bool)_commonDs.Tables["Lcode"].Rows[0]["It_Rate"] == true)
                    {
                        string cSql1 = "Select top 1 Rate_Level From Ac_mast where Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString();
                        _dt = _oDataAccess.GetDataTable(cSql1, null, 50);
                        if ((int)_dt.Rows[0]["Rate_Level"] != 0)
                        {
                            cCond += " And (It_mast.It_code = It_rate.It_code And (It_rate.Rlevel = " + _dt.Rows[0]["Rate_Level"].ToString() + " Or It_rate.Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString() + ")) ";
                        }
                        else
                        {
                            cCond += " And (It_mast.It_code = It_rate.It_code And It_rate.Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString() + ") ";
                        }
                        cTbl += ",It_Rate ";
                        xcFldList += ",It_Rate.It_Rate ";
                        xcDispCol += ",It_Rate:Party Rate";
                        xcRetCol += ",It_Rate";
                    }
                    //******* Checking for Rate in Item Rate master -- End *******\\

                    cFldList = _commonDs.Tables["Lcode"].Rows[0]["It_fields"].ToString();
                    string[][] cFldLst = cFldList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(chunk => chunk.Split(':')).ToArray();
                    var cItCode =
                        from _cFldLst in cFldLst
                        where _cFldLst[0].ToUpper().Contains("IT_CODE")
                        select _cFldLst[0];
                    if (cItCode.Count<string>() == 0)
                    {
                        xcFldList += ",It_mast.It_Code ";
                    }

                    cCond += " And (It_mast.ldeactive = 0 Or (It_mast.ldeactive = 1 And It_mast.deactfrom > '" + txtInvoiceDt.Text.ToString() + "' )) ";
                    cSql = "set dateformat dmy select " + cFldLst[0][0] + xcFldList + " from " + cTbl + " where " + cCond + " order by It_Name ";
                    cFrmCap = "Select Item";
                    cSrchCol = "It_Name" + xcSrchCol;
                    cDispCol = "It_Name:Item" + xcDispCol;
                    cRetCol = "It_Code,It_Name" + xcRetCol;
                    break;
            }

            _dt = _oDataAccess.GetDataTable(cSql, null, 50);
            DataView _dvw = _dt.DefaultView;

            _oSelPop.pdataview = _dvw;
            _oSelPop.pformtext = cFrmCap;
            _oSelPop.psearchcol = cSrchCol;
            _oSelPop.pDisplayColumnList = cDispCol;
            _oSelPop.pRetcolList = cRetCol;
            _oSelPop.Icon = new Icon(CommonInfo.IconPath.ToString().Replace("<*#*>", " "));
            _oSelPop.ShowDialog();

            if (_oSelPop.pReturnArray != null)
            {
                switch (_popupType)
                {
                    case "ITEM":
                        //dgvItemDetails.CurrentRow.Cells["colItem"].Value = _oSelPop.pReturnArray[1].ToString();
                        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["It_Code"] = _oSelPop.pReturnArray[0].ToString();
                        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = _oSelPop.pReturnArray[1].ToString();
                        string[] _arr = cRetCol.Split(',');
                        int _iRate = Array.IndexOf(_arr, "Rate");
                        int _iItRate = Array.IndexOf(_arr, "It_Rate");
                        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"] = (_iItRate > 0 ? (Convert.ToInt16(_oSelPop.pReturnArray[_iItRate]) > 0 ? _oSelPop.pReturnArray[_iItRate] : _oSelPop.pReturnArray[_iRate]) : _oSelPop.pReturnArray[_iRate]);
                        break;
                }
            }
        }
        #endregion

        #region Add - Edit - Delete Methods
        private void addHdrRecords()
        {
            DataRow _dr;
            _dr = clsInsertDefaultValue.AddNewRow(_commonDs.Tables["Main"]);
            _commonDs.Tables["Main"].Rows.Add(_dr);
            clsInsertDefaultValue.InsertDefVal_Main(0);
            addChldRecords("Item");
        }

        private void addChldRecords(string cTblName)
        {
            DataRow _dr;
            _dr = clsInsertDefaultValue.AddNewRow(_commonDs.Tables[cTblName]);
            _commonDs.Tables[cTblName].Rows.Add(_dr);
            this.BindingContext[_commonDs, cTblName].Position = _commonDs.Tables["Item"].Rows.Count;
            clsInsertDefaultValue.InsertDefVal_Item(this.BindingContext[_commonDs, cTblName].Position);
            if (dgvItemDetails.CurrentRow.Index < this.dgvItemDetails.Rows.Count && dgvItemDetails.Rows.Count>1) 
            {
                //this.BindingContext[_commonDs,cTblName].
                //dgvItemDetails.CurrentRow.Index += 1; 
                int i= dgvItemDetails.CurrentRow.Index+1;
                dgvItemDetails.CurrentCell = dgvItemDetails.Rows[i].Cells[1];
            }
        }

        private void DeleteChldRecords(string cTblName)
        {
            _commonDs.Tables["Item"].Rows.Cast<DataRow>().Where(r => r["Item"].ToString() == "" && Convert.ToDecimal(r["Qty"]) == 0).ToList().ForEach(r => r.Delete());
        }
        #endregion Add - Edit - Delete Methods

        #endregion Private Methods
    }
}
