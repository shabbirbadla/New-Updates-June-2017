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
using System.Diagnostics;
using System.Reflection;    // Added by Sachin N. S. on 25/07/2013 for Bug-14538
using DataAccess_Net;



namespace PointofSale
{
    public partial class udFrmPointofSale : Form
    {
        clsDataAccess _oDataAccess;
        DataSet _commonDs;
        DataTable _dtSumDiscChrgs;      // Added By Sachin N. S. 27/05/2013 for Bug-14538
        ToolTip _toolTip;
        string pAppPath;
        string _keypress = "", _item = "";
        bool _barcodeRead = false;
        bool ldntValidate = false;
        string cAppName = "", cAppPId = "";
        string _taxExpr = "";           // Added By Sachin N. S. 17/07/2013 for Bug-14538
        string _colValue = "";          // Added By Sachin N. S. 17/07/2013 for Bug-14538
        bool _dgvDiscChrgsdataBind = false;
        decimal OrgHgth = 0M, OrgWdth = 0M;
        bool _setFocus = false;
        int _curRowNo = -1;             // Added by Sachin N. S. on 19/01/2016 for Bug-27503

        #region Default Constructor and Screen Load
        public udFrmPointofSale()
        {
            InitializeComponent();
            clsScreenPropEvents.OrgHgth = this.Height;
            clsScreenPropEvents.OrgWdth = this.Width;
            OrgHgth = this.Height;
            OrgWdth = this.Width;
        }

        private void udFrmPointofSale_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-GB");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            _toolTip = new ToolTip();


            this.Icon = new Icon(CommonInfo.IconPath.ToString().Replace("<*#*>", " "));
            // Changed by Sachin N. S. on 06/08/2013 -- Start
            //this.txtUserName.Text = CommonInfo.UserName;
            //this.txtInvoiceSr.Text = CommonInfo.InvSeries;
            //this.txtCategory.Text = CommonInfo.Category;
            //this.txtDepartment.Text = CommonInfo.Department;

            this.tsUserName.Text = "User Name : " + CommonInfo.UserName;
            this.tsInvoiceSeries.Text = "Invoice Series : " + CommonInfo.InvSeries;
            this.tsCategory.Text = "Category : " + CommonInfo.Category;
            this.tsDepartment.Text = "Department : " + CommonInfo.Department;
            // Changed by Sachin N. S. on 06/08/2013 -- End

            // Changed by Sachin N. S. on 06/08/2013 for Bug-27503 -- Start
            this.tsCounter.Text = "Counter : " + CommonInfo.CounterNm;
            // Changed by Sachin N. S. on 06/08/2013 for Bug-27503 -- End

            string appPath;
            appPath = Application.ExecutablePath;

            this.mInsertProcessIdRecord();

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }

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

            hideUnhideControls();           //*** Added by Sachin N. S. on 29/08/2013 for Bug-14538

            clsScreenPropEvents.NewHgth = this.Height;
            clsScreenPropEvents.NewWdth = this.Width;
            clsScreenPropEvents.ResizeForm(this);

            //**** Changed by Sachin N. S. on 06/08/2013 for Bug-14538 -- Start
            this.tsUserName.Height = (int)((this.Height * this.tsUserName.Height) / this.OrgHgth);
            this.tsUserName.Width = (int)((this.Width * this.tsUserName.Width) / this.OrgWdth);

            this.tsInvoiceSeries.Height = (int)((this.Height * this.tsInvoiceSeries.Height) / this.OrgHgth);
            this.tsInvoiceSeries.Width = (int)((this.Width * this.tsInvoiceSeries.Width) / this.OrgWdth);

            this.tsDepartment.Height = (int)((this.Height * this.tsDepartment.Height) / this.OrgHgth);
            this.tsDepartment.Width = (int)((this.Width * this.tsDepartment.Width) / this.OrgWdth);

            this.tsCategory.Height = (int)((this.Height * this.tsCategory.Height) / this.OrgHgth);
            this.tsCategory.Width = (int)((this.Width * this.tsCategory.Width) / this.OrgWdth);
            //**** Changed by Sachin N. S. on 06/08/2013 for Bug-14538 -- End

            //**** Changed by Sachin N. S. on 06/08/2013 for Bug-27503 -- End
            this.tsCounter.Height = (int)((this.Height * this.tsCounter.Height) / this.OrgHgth);
            this.tsCounter.Width = (int)((this.Width * this.tsCounter.Width) / this.OrgWdth);
            //**** Changed by Sachin N. S. on 06/08/2013 for Bug-27503 -- End

            //***** Changed By Sachin N. S. on 23/05/2013 for Bug-14538 -- Start *****//
            _dgvDiscChrgsdataBind = true;
            this.Refresh();
            //txtParty.Focus(); 
            dgvItemDetails.Focus();
            SendKeys.Send("{Tab}");
            //***** Changed By Sachin N. S. on 23/05/2013 for Bug-14538 -- Start *****//
        }
        #endregion Default Constructor and Screen Load

        #region Control Events

        //******* Methods to Call Shortcut Keys -- Start *******\\
        private void udFrmPointofSale_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    this.addChldRecords("Item");
                    break;
                case Keys.F4:
                    this.btnPayment.PerformClick();
                    break;
                case Keys.F5:
                    this.btnPrintInv.PerformClick();
                    break;
                case Keys.F12:
                    this.btnExit.PerformClick();
                    break;
                case Keys.F6:                           // Added by Sachin N. S. on 20/01/2016 for Bug-27503
                    this.CancelEntry();
                    break;
            }
        }

        private void udFrmPointofSale_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (keyData == Keys.F1)
            //{
            //    MessageBox.Show("You pressed the F1 key");
            //    return true;    // indicate that you handled this keystroke
            //}
            //// Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        //******* Methods to Call Shortcut Keys -- End *******\\

        //****** Added by Sachin N. S. on 22/05/2013 for Bug-14538 -- Start ******//
        private void txtParty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                this.cmdParty.PerformClick();
            }
        }

        private void cmdParty_Click(object sender, EventArgs e)
        {
            if (_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Item"] != null).Count(row => row["Item"].ToString().Trim() != string.Empty) <= 0)
            {
                this.popUpFromTable("PARTY");
            }
            else
            {
                MessageBox.Show("Party cannot be changed as Item have been selected.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtWareHouse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                this.cmdWarehouse.PerformClick();
            }
            RefreshControls();
            dgvItemDetails.Focus();
            SendKeys.Send("{Tab}");
        }

        private void cmdWarehouse_Click(object sender, EventArgs e)
        {
            if (_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Item"] != null).Count(row => row["Item"].ToString().Trim() != string.Empty) <= 0)
            {
                this.popUpFromTable("WAREHOUSE");
            }
            else
            {
                MessageBox.Show("Warehouse cannot be changed as Item have been selected.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //****** Added by Sachin N. S. on 22/05/2013 for Bug-14538 -- End ******//

        private void dgvItemDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvItemDetails.RowCount > 0)
            {
                if (dgvItemDetails.Columns[dgvItemDetails.CurrentCell.ColumnIndex].Name.ToString() == "colItem")
                {
                    if (e.KeyCode == Keys.F2 && dgvItemDetails.CurrentRow.Cells[dgvItemDetails.CurrentCell.ColumnIndex].Value.ToString() == "")
                    {
                        popUpFromTable("ITEM");
                        if (this.dgvItemDetails.CurrentRow.Cells[1].Value.ToString() != "")     // ****** Added by Sachin N. S. on 20/08/2015 for Bug-26654
                        {
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 1;
                            _keypress = "F2";

                            /// Changed by Sachin N. S. on 06/02/2014 for Rupesh Panhale's Query -- Start
                            //SendKeys.Send("{Tab}");       // Commented by Sachin N. S. on 06/02/2014 for Rupesh Requirement
                            this.addChldRecords("Item");
                            this.dgvItemDetails.Refresh();
                        }
                        _item = "";
                        /// Changed by Sachin N. S. on 06/02/2014 for Rupesh Panhale's Query -- End

                        //addChldRecords("Item");

                        // ******* Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- Start
                        int _iRwPos = _curRowNo;        // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                        if (_iRwPos >= 0 && Convert.ToInt16(_commonDs.Tables["Item"].Rows[_iRwPos]["It_Code"])!=0)
                        {
                            string cSql = "Select AskQty,AskRate from It_Mast where It_Code=" + _commonDs.Tables["Item"].Rows[_iRwPos]["It_Code"].ToString();       // Added by Sachin N. S. on 18/01/2016 for Bug-27503

                            DataTable _dt;
                            _dt = _oDataAccess.GetDataTable(cSql, null, 50);
                            if (Convert.ToBoolean(_dt.Rows[0]["AskQty"]) == true)
                            {
                                this.BindingContext[_commonDs, "Item"].Position = _iRwPos;
                                dgvItemDetails.CurrentCell = dgvItemDetails.Rows[_iRwPos].Cells[2];
                            }
                            else
                            {
                                if (Convert.ToBoolean(_dt.Rows[0]["AskRate"]) == true)
                                {
                                    this.BindingContext[_commonDs, "Item"].Position = _iRwPos;
                                    dgvItemDetails.CurrentCell = dgvItemDetails.Rows[_iRwPos].Cells[3];
                                }
                            }
                        }
                        // ******* Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- End
                    }

                    if (e.KeyCode == Keys.Enter || (e.Control == true && e.KeyCode == Keys.V))
                    {
                        //MessageBox.Show("ENTER");
                        _keypress = "ENTER";
                        //if (dgvItemDetails.CurrentRow.Cells[dgvItemDetails.CurrentCell.ColumnIndex].Value.ToString() != "")
                        //{
                        //    bool llAdd = false;
                        //    llAdd = ReadBarcode(dgvItemDetails.CurrentRow.Cells[dgvItemDetails.CurrentCell.ColumnIndex].Value.ToString());
                        //    if (llAdd == true)
                        //    {
                        //        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 1;
                        //        SendKeys.Send("{Tab}");
                        //    }
                        //    else
                        //    {
                        //        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = "";
                        //    }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Barcode cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                    }
                }
                // ****** Added by Sachin N. S. on 17/07/2013 for Bug-14538 -- Start ******//
                if (dgvItemDetails.Columns[dgvItemDetails.CurrentCell.ColumnIndex].Name.ToString() == "colTax_Name")
                {
                    if (e.KeyCode == Keys.F2)
                    {
                        popUpFromTable("SALESTAX");
                    }
                }
                // ****** Added by Sachin N. S. on 17/07/2013 for Bug-14538 -- End ******//

                // ****** Added by Sachin N. S. on 21/08/2015 for Bug-26654 -- Start ******//
                if (dgvItemDetails.Columns[dgvItemDetails.CurrentCell.ColumnIndex].Name.ToString() == "colDelete")
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        int _rowIndex = dgvItemDetails.CurrentCell.RowIndex;
                        int _srNo = Convert.ToInt16(_commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentCell.RowIndex]["Item_No"]);
                        _commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentCell.RowIndex].Delete();
                        _commonDs.Tables["Item"].AcceptChanges();
                        if (dgvItemDetails.RowCount > 0 && _rowIndex <= dgvItemDetails.CurrentCell.RowIndex)
                        {
                            for (int i = dgvItemDetails.CurrentCell.RowIndex; i < _commonDs.Tables["Item"].Rows.Count; i++)
                            {
                                _commonDs.Tables["Item"].Rows[i]["Item_No"] = _srNo.ToString();
                                _srNo += 1;
                            }
                        }
                    }
                }
                // ****** Added by Sachin N. S. on 21/08/2015 for Bug-26654 -- End ******//
            }
            RefreshControls();
            _keypress = "";       // Added by Sachin N. S. on 26/08/2015 for Bug-26654
            //dgvItemDetails.Refresh();
        }

        private void dgvItemDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItemDetails.Columns[e.ColumnIndex].Name == "colQuantity" || (dgvItemDetails.Columns[e.ColumnIndex].Name == "colItem" && _keypress != "F2") || dgvItemDetails.Columns[e.ColumnIndex].Name == "colRate")   // Changed by Sachin N. S. on 19/01/2016 for Bug-27503
            {
                //MessageBox.Show("Test -1");
                _commonDs.Tables["Item"].Rows[e.RowIndex].EndEdit();
                this.RefreshControls();

                if (dgvItemDetails.Columns[e.ColumnIndex].Name.ToString() == "colQuantity" || (dgvItemDetails.Columns[e.ColumnIndex].Name == "colItem" && _keypress != "F2") || dgvItemDetails.Columns[e.ColumnIndex].Name == "colRate")   // Changed by Sachin N. S. on 19/01/2016 for Bug-27503
                {
                    //MessageBox.Show("Test -2");
                    string cSql = "";
                    int _iRwPos = _curRowNo;      // Added by Sachin N. S. on 18/01/2016 for Bug-27503
                    if (_iRwPos >= 0)
                    {
                        cSql = "Select AskQty,AskRate from It_Mast where It_Code=" + _commonDs.Tables["Item"].Rows[_iRwPos]["It_Code"].ToString();       // Added by Sachin N. S. on 18/01/2016 for Bug-27503
                    }

                    // ******* Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- Start
                    if (_iRwPos >= 0 && Convert.ToInt16(_commonDs.Tables["Item"].Rows[_iRwPos]["It_Code"])!=0)
                    {
                        DataTable _dt;
                        _dt = _oDataAccess.GetDataTable(cSql, null, 50);
                        if (Convert.ToBoolean(_dt.Rows[0]["AskQty"]) == true && (dgvItemDetails.Columns[e.ColumnIndex].Name == "colItem" && _keypress != "F2"))
                        {
                            this.BindingContext[_commonDs, "Item"].Position = _iRwPos;
                            dgvItemDetails.CurrentCell = dgvItemDetails.Rows[_iRwPos].Cells[2];
                            //this.BindingContext[_commonDs, "Item"].Position = _iRwPos;
                        }
                        else
                        {
                            if (Convert.ToBoolean(_dt.Rows[0]["AskRate"]) == true && (dgvItemDetails.Columns[e.ColumnIndex].Name == "colQuantity" || (dgvItemDetails.Columns[e.ColumnIndex].Name == "colItem" && _keypress != "F2")))
                            {
                                this.BindingContext[_commonDs, "Item"].Position = _iRwPos;
                                dgvItemDetails.CurrentCell = dgvItemDetails.Rows[_iRwPos].Cells[3];
                                //this.BindingContext[_commonDs, "Item"].Position = _iRwPos;
                            }
                            else
                            {
                                this.addChldRecords("Item");
                            }
                        }
                    }
                    // ******* Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- End

                    this.dgvItemDetails.Refresh();
                    _item = "";
                    _barcodeRead = false;
                    _setFocus = true;
                }
            }
            else        //********* Added by Sachin N. S. on 26/06/2013 for Bug-14538 -- Start *********//
            {
                _commonDs.Tables["Item"].Rows[e.RowIndex].EndEdit();
                this.dgvItemDetails.Refresh();
                this.RefreshControls();
            }           //********* Added by Sachin N. S. on 26/06/2013 for Bug-14538 -- End *********//
        }

        private void dgvItemDetails_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (ldntValidate == true) { return; }
            if (dgvItemDetails.Rows[e.RowIndex].IsNewRow == true) { return; }
            if (dgvItemDetails.Columns[e.ColumnIndex].Name == "colQuantity")
            {
                if (Convert.ToInt16(_commonDs.Tables["Item"].Rows[e.RowIndex]["It_code"]) == 0)
                { return; }
                // **** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start **** //
                //if (Convert.ToDecimal(_commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"]) == 0)
                if (Convert.ToDecimal(e.FormattedValue) == 0)
                { return; }
                // **** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- End **** //

                if (Stock_Checking(Convert.ToInt16(_commonDs.Tables["Item"].Rows[e.RowIndex]["It_code"]), Convert.ToDateTime(this.txtInvoiceDt.Value), Convert.ToDecimal(e.FormattedValue), e.RowIndex) <= 0)
                {
                    _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 0;
                    dgvItemDetails.CancelEdit();
                    MessageBox.Show("Item out of Stock.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }

            if (dgvItemDetails.Columns[e.ColumnIndex].Name == "colItem" && _keypress != "F2")
            {
                _keypress = "";
                if (_barcodeRead == true)
                    return;

                if (e.FormattedValue.ToString() != "" && _item != e.FormattedValue.ToString())
                {
                    string clAdd = "ADD";
                    //llAdd = ReadBarcode(dgvItemDetails.CurrentRow.Cells[dgvItemDetails.CurrentCell.ColumnIndex].Value.ToString());
                    clAdd = ReadBarcode(e.FormattedValue.ToString());
                    if (clAdd == "ADD" && e.RowIndex == this.BindingContext[_commonDs, "Item"].Position)
                    {
                        //MessageBox.Show("Validating : "+this.BindingContext[_commonDs, "Item"].Position.ToString());
                        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 1;
                        //SendKeys.Send("{Tab}");
                        _barcodeRead = true;
                    }
                    else
                    {
                        //MessageBox.Show("Test -5");
                        if (clAdd == "EXIST")
                        {
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = "";
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 0;
                            dgvItemDetails.CancelEdit();
                        }
                        else
                        {
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = "";
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 0;
                            //int i = dgvItemDetails.CurrentRow.Index;
                            //dgvItemDetails.CurrentCell = dgvItemDetails.Rows[i].Cells[1];
                            dgvItemDetails.CancelEdit();
                            SendKeys.Send("{Esc}");
                            e.Cancel = true;
                        }
                    }
                }
            }
            _keypress = "";
        }

        private void dgvItemDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItemDetails.Columns[e.ColumnIndex].Name == "colItem")
            {
                _item = dgvItemDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            //            dgvItemDetails.Columns[e.ColumnIndex].Tag = dgvItemDetails.Columns[e.ColumnIndex].Tag == null || Convert.ToString(dgvItemDetails.Columns[e.ColumnIndex].Tag) == string.Empty ? dgvItemDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value : Convert.ToString(dgvItemDetails.Columns[e.ColumnIndex].Tag).Substring(0, 2) + dgvItemDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();     // Added by Sachin N. S. on 24/07/2013 for Bug-14538
            _colValue = dgvItemDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();     // Added by Sachin N. S. on 24/07/2013 for Bug-14538
        }

        private void dgvItemDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.ldntValidate = true;
            if (e.ColumnIndex == dgvItemDetails.Columns["colDelete"].Index && e.RowIndex >= 0)
            {
                int _srNo = Convert.ToInt16(_commonDs.Tables["Item"].Rows[e.RowIndex]["Item_No"]);
                _commonDs.Tables["Item"].Rows[e.RowIndex].Delete();
                _commonDs.Tables["Item"].AcceptChanges();
                for (int i = e.RowIndex; i < _commonDs.Tables["Item"].Rows.Count; i++)
                {
                    _commonDs.Tables["Item"].Rows[i]["Item_No"] = _srNo.ToString();
                    _srNo += 1;
                }
            }
            this.RefreshControls();
            this.ldntValidate = false;
        }

        private void dgvItemDetails_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == dgvItemDetails.Columns["colDelete"].Index && e.RowIndex >= 0)
            //{
            //    Control control = dgvItemDetails;

            //    while (control != null)
            //    {
            //        control.CausesValidation = false;
            //        control = control.Parent;
            //    }
            //}
        }

        //****** Added by Sachin N. S. on 24/07/2013 for Bug-14538 -- Start ******//
        private void dgvItemDetails_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (ldntValidate == true) return;

            if (dgvItemDetails.Columns[e.ColumnIndex].DataPropertyName == "Qty" || dgvItemDetails.Columns[e.ColumnIndex].DataPropertyName == "Rate" || dgvItemDetails.Columns[e.ColumnIndex].DataPropertyName == "Item" || Convert.ToString(dgvItemDetails.Columns[e.ColumnIndex].Tag) == "V#" || Convert.ToString(dgvItemDetails.Columns[e.ColumnIndex].Tag) == "%#" || Convert.ToString(dgvItemDetails.Columns[e.ColumnIndex].Tag) == "S#")
            {
                if (_colValue != dgvItemDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    itemwiseTotal(e.RowIndex, e.ColumnIndex);        // Changed by Sachin N. S. on 15/01/2016 for Bug-27503
                }
            }
        }

        private void dgvItemDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (Convert.ToString(dgvItemDetails.CurrentCell.OwningColumn.Tag) == "S#")
            {
                ComboBox _cb = e.Control as ComboBox;
                if (_cb != null)
                {
                    _cb.SelectionChangeCommitted -= new
                     EventHandler(cbSalesTax_SelectionChangeCommitted);

                    _cb.SelectionChangeCommitted += new
                      EventHandler(cbSalesTax_SelectionChangeCommitted);
                }
            }
        }

        private void cbSalesTax_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvItemDetails.EndEdit();
            itemwiseTotal(dgvItemDetails.CurrentCell.RowIndex, dgvItemDetails.CurrentCell.ColumnIndex);      // Changed by Sachin N. S. on 15/01/2016 for Bug-27503
        }

        //private void cbSalesTax_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dgvItemDetails.EndEdit();
        //    itemwiseTotal(dgvItemDetails.CurrentCell.ColumnIndex);
        //}
        //****** Added by Sachin N. S. on 24/07/2013 for Bug-14538 -- End ******//

        //private void dgvItemDetails_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.ColumnIndex == dgvItemDetails.Columns["colDelete"].Index && e.RowIndex >= 0)
        //    {
        //        Control control = dgvItemDetails;

        //        while (control != null)
        //        {
        //            control.CausesValidation = false;
        //            control = control.Parent;
        //        }
        //        this.AutoValidate = AutoValidate.Disable;
        //    }
        //}

        private void dgvDiscChrgs_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ldntValidate == true) return;

            if (_dgvDiscChrgsdataBind == true)      // Added by Sachin N. S. on 18/07/2013 for Bug-14538
            {
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    if (_commonDs.Tables["HdDiscChrgs"].Rows[e.RowIndex]["Code"].ToString() == "S")
                    {
                        dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                    }
                    else
                    {
                        dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                    }
                }

                if (e.ColumnIndex != 0 && e.RowIndex != -1)
                {
                    if (_commonDs.Tables["HdDiscChrgs"].Rows[e.RowIndex]["Code"].ToString() == "S")
                    {
                        if (_commonDs.Tables["HdDiscChrgs"].Rows[e.RowIndex]["Head_Nm"].ToString() == "NO-TAX")
                        {
                            dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                        }
                        else
                        {
                            dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                        }
                    }
                    else
                    {
                        dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                    }
                }
                _colValue = dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();     // Added by Sachin N. S. on 14/01/2016 for Bug-27503
            }
        }

        private void dgvDiscChrgs_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex !=0)
            // Added by Sachin N. S. on 14/01/2016 for Bug-27503 -- Start
            if (_colValue != dgvDiscChrgs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
            {
                if (e.ColumnIndex == 2)
                {
                    _commonDs.Tables["HdDiscChrgs"].Rows[e.RowIndex]["Def_Pert"] = 0;
                }
                // Added by Sachin N. S. on 14/01/2016 for Bug-27503 -- End
                this.CalculateHeaderTax(_commonDs.Tables["HdDiscChrgs"].Rows[e.RowIndex]["Fld_Nm"].ToString());
                this.RefreshControls();
            }
        }

        //******* Added by Sachin N. S. on 20/06/2013 for Bug-14538 ******* Start
        private void dgvDiscChrgs_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvDiscChrgs.RowCount > 0)
            {
                if (dgvDiscChrgs.Columns[dgvDiscChrgs.CurrentCell.ColumnIndex].Name.ToString() == "colHeading")
                {
                    if (e.KeyCode == Keys.F2 && _commonDs.Tables["HdDiscChrgs"].Rows[dgvDiscChrgs.CurrentCell.RowIndex]["Code"].ToString() == "S")
                    {
                        popUpFromTable("SALESTAX");
                    }
                }
            }
        }

        private void btnAddInfo_Click(object sender, EventArgs e)
        {
            _commonDs.Tables["MAIN"].AcceptChanges();
            udAdditionalInfo.udAdditionalInfo _udAdditionalInfo = new udAdditionalInfo.udAdditionalInfo();
            _udAdditionalInfo._commonDs = _commonDs;
            _udAdditionalInfo._EntryTy = "PS";
            _udAdditionalInfo._HdrDtl = "H";
            _udAdditionalInfo._TblName = "MAIN";
            _udAdditionalInfo.callAdditionalInfo();
        }
        //******* Added by Sachin N. S. on 20/06/2013 for Bug-14538 ******* End

        private void btnPrintInv_Click(object sender, EventArgs e)
        {
            popUpFromTable("PRINTBILL");
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (_commonDs.Tables["Main"].Rows.Count > 0)
            {
                this.BindingContext[_commonDs, "Main"].EndCurrentEdit();

                //**** Added by Sachin N. S. on 14/06/2014 for Bug-14538 -- Start
                if (Convert.ToDecimal(this.txtNetAmt.Text) <= 0)
                {
                    MessageBox.Show("No records found for payment.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //**** Added by Sachin N. S. on 14/06/2014 for Bug-14538 -- End

                this.DeleteChldRecords("Item");
                _commonDs.Tables["Main"].AcceptChanges();
                _commonDs.Tables["Item"].AcceptChanges();

                if (_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Item"] != null).Count(row => row["Item"].ToString().Trim() == string.Empty) > 0)
                {
                    MessageBox.Show("Item name cannot be blank.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SendKeys.Send("{Esc}");
                    return;
                }

                if (Convert.ToDecimal(this.txtNetAmt.Text) > 0)
                {
                    udFrmPaymentScreen _frmPayScrn = new udFrmPaymentScreen(_commonDs);
                    _frmPayScrn.ShowDialog();
                    if (_frmPayScrn._RetValue == "SAVED")
                    {
                        PrintBill(Convert.ToInt16(_commonDs.Tables["Main"].Rows[0]["Tran_cd"]));
                        clearTables();
                        addHdrRecords();
                        dgvItemDetails.Focus();
                        SendKeys.Send("{Tab}");
                    }
                }
                else
                {
                    MessageBox.Show("No records found for payment.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.RefreshControls();
            }
            else
            {
                MessageBox.Show("No records found for payment.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.ldntValidate = true;
            if (_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Item"] != null).Count(row => row["Item"].ToString().Trim() != string.Empty) > 0)
            {
                if (MessageBox.Show("The data will not be saved if clicked on Exit.\nDo you want still want to Exit?", CommonInfo.AppCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    this.ldntValidate = false;
                    return;
                }
            }
            this.Close();
        }

        private void btnPrintInv_MouseHover(object sender, EventArgs e)
        {
            _toolTip.Show("Reprint Bill", btnPrintInv);
        }

        private void btnPayment_MouseHover(object sender, EventArgs e)
        {
            _toolTip.Show("Pay Bill", btnPayment);
        }

        private void btnExit_MouseHover(object sender, EventArgs e)
        {
            _toolTip.Show("Exit POS", btnExit);
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

            _commonDs = _oDataAccess.GetDataSet("Execute USP_GET_PS_DATASET 'PS',0", null, 25);
            _commonDs.Tables[0].TableName = "Lcode";
            _commonDs.Tables[1].TableName = "Main";
            _commonDs.Tables[2].TableName = "Item";
            _commonDs.Tables[3].TableName = "PSPayDetail";
            _commonDs.Tables[4].TableName = "DcMast";
            _commonDs.Tables[5].TableName = "DiscChrgsFldLst";   // DCMast Fields linked in Lother of Price List Master, Account Master and Item Master. // Added by Sachin N. S. on 22/06/2013 for Bug-14538
            _commonDs.Tables[6].TableName = "Stax_Mas";          // Sales Tax Master. // Added by Sachin N. S. on 22/06/2013 for Bug-14538
            //_commonDs.Tables[3].TableName = "Acdet";
            //_commonDs.Tables[4].TableName = "Mall";
            //_commonDs.Tables[5].TableName = "Itref";

            //****** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- Start ******//
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Extra"]) == true)
            {
                DataTable _dt = new DataTable();
                _dt = _oDataAccess.GetDataTable("Select * from Lother where E_Code='PS' Order by Att_file, Serial, SubSerial", null, 25);
                _dt.TableName = "Lother";
                _commonDs.Tables.Add(_dt);
            }

            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["I_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
            {
                DataRow[] _dr1 = _commonDs.Tables["DiscChrgsFldLst"].Select("Att_File=true");
                DataTable _dt;
                if (_dr1.Count() > 0)
                {
                    _dt = _dr1.CopyToDataTable();
                    string _hdrDiscFlds = string.Join(",", _dt.Rows.OfType<DataRow>().Select(row => row["LothrFldNm"].ToString()).ToArray());
                    CommonInfo.HdrDiscFlds = _hdrDiscFlds;
                }
                _dr1 = _commonDs.Tables["DiscChrgsFldLst"].Select("Att_File=false and E_code='PM'");
                if (_dr1.Count() > 0)
                {
                    _dt = _dr1.CopyToDataTable();
                    string _dtlDiscFlds = string.Join(",", _dt.Rows.OfType<DataRow>().Select(row => row["LothrFldNm"].ToString()).ToArray());
                    CommonInfo.DtlDiscFlds = _dtlDiscFlds;
                }
                _dr1 = _commonDs.Tables["DiscChrgsFldLst"].Select("Att_File=false and E_code='IM'");
                if (_dr1.Count() > 0)
                {
                    _dt = _dr1.CopyToDataTable();
                    string _dtlDiscFlds = string.Join(",", _dt.Rows.OfType<DataRow>().Select(row => row["LothrFldNm"].ToString()).ToArray());
                    CommonInfo.DtlDiscFldsIM = _dtlDiscFlds;
                }
                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
                {
                    DataRow _dr;
                    _dr = _commonDs.Tables["Stax_Mas"].NewRow();
                    _dr["Tax_Name"] = "NO-TAX";
                    _dr["Level1"] = 0;
                    _commonDs.Tables["Stax_Mas"].Rows.Add(_dr);
                }
            }
            //****** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- End ******//
        }

        private void RefreshControls()
        {
            txtTotalQty.Text = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Qty"] != null).Sum(row => Convert.ToDecimal(row["Qty"])), 2).ToString();
            txtNoOfItems.Text = _commonDs.Tables["Item"].AsEnumerable().Where(row => row["Item"] != null).Count().ToString();
            //txtGrossAmt.Text = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["U_asseamt"] != null).Sum(row => Convert.ToDecimal(row["U_asseamt"])), 2).ToString();
            txtGrossAmt.Text = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Gro_amt"] != null).Sum(row => Convert.ToDecimal(row["Gro_amt"])), 2).ToString();    // Changed by Sachin N. S. on 25/05/2013 for Bug-14538
            //txtTaxAmt.Text = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Qty"] != null).Sum(row => Convert.ToDecimal(row["TaxAmt"])), 2).ToString();
            //txtTaxAmt.Text = _commonDs.Tables["Item"].AsEnumerable().Where(row => row["Qty"] != null).Sum(row => Math.Round(Convert.ToDecimal(row["TaxAmt"]),2)).ToString();
            int _roundoff = Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Net_Op"]) ? 0 : 2;
            //txtNetAmt.Text = Math.Round(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Gro_amt"] != null).Sum(row => Convert.ToDecimal(row["Gro_amt"])), _roundoff).ToString();

            //***** Added by Sachin N. S. on 22/05/2013 for Bug-14538 -- Start *****//
            _commonDs.Tables["Main"].Rows[0]["Gro_Amt"] = Convert.ToDecimal(txtGrossAmt.Text);
            this.CalculateHeaderTax("");
            this.RefreshSummaryGrid();
            //***** Added by Sachin N. S. on 22/05/2013 for Bug-14538 -- End *****//
            this.Refresh();
        }

        // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ****** //
        private void RefreshSummaryGrid()
        {
            Decimal _Amount = 0;
            var _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Gross Amount"
                               select currentRw).FirstOrDefault();
            _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Gro_amt"];

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Less : Dedn. Before Tax"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_deduc"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Taxable Charges"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_tax"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Excise Duty"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_examt"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Add/Less Charges"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_add"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Sales Tax"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Taxamt"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Non Taxable Charges"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_nontax"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Less : Final Discount"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_fdisc"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Net Amount"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                _Amount = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_examt"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_add"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["taxamt"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_nontax"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_fdisc"]);
                int _roundoff = Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Net_Op"]) ? 0 : 2;
                _currentRow["Amount"] = Math.Round(_Amount, _roundoff).ToString();
                _commonDs.Tables["Main"].Rows[0]["Net_Amt"] = _currentRow["Amount"];
            }

            _currentRow = (from currentRw in _dtSumDiscChrgs.AsEnumerable()
                           where currentRw.Field<string>("HeadingNm") == "Round Off"
                           select currentRw).FirstOrDefault();
            if (_currentRow != null)
            {
                int _roundoff = Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Net_Op"]) ? 0 : 2;
                _Amount = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_Amt"]) - _Amount;
                _commonDs.Tables["Main"].Rows[0]["roundoff"] = _roundoff == 0 ? _Amount : Math.Round(_Amount, 2);
                _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["roundoff"];
            }

        }

        private void itemwiseTotal(int _grdRowPos, int _grdColPos)
        {
            _grdColPos = dgvItemDetails.Columns[_grdColPos].DisplayIndex;
            string _fldNm, _a_s;
            DataRow _dr, _dr1, _dr2;
            //_dr1 = _commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentRow.Index];
            _dr1 = _commonDs.Tables["Item"].Rows[_grdRowPos];       // Changed by Scahin N. S. on 15/01/2016 for Bug-27503
            _dr1["Tot_deduc"] = 0;
            _dr1["Tot_tax"] = 0;
            _dr1["Tot_examt"] = 0;
            _dr1["Tot_add"] = 0;
            _dr1["Tot_nontax"] = 0;
            _dr1["Tot_fdisc"] = 0;

            decimal _maValue = 0M;
            _maValue = Convert.ToDecimal(_dr1["Qty"]) * Convert.ToDecimal(_dr1["Rate"]);
            string _cpertNm = "";
            decimal _npertnm = 0M, _mAmt = 0M;
            #region Evaluating Column values before the column edited
            DataGridViewColumn _dgvCol, _dgvCol1;
            for (int i = 0; i < _grdColPos; i++)
            {
                _dgvCol = (from _grdVwCol in dgvItemDetails.Columns.Cast<DataGridViewColumn>()
                           where _grdVwCol.DisplayIndex == i
                           select _grdVwCol).SingleOrDefault();
                if (Convert.ToString(_dgvCol.Tag) == "V#")
                {
                    if (_dgvCol.DisplayIndex < _grdColPos)
                    {
                        _fldNm = _dgvCol.DataPropertyName;
                        _dr = _commonDs.Tables["DtDiscChrgs"].Select("Fld_Nm='" + _fldNm.ToString().Trim() + "'").FirstOrDefault();
                        switch (_dr["A_S"].ToString())
                        {
                            case "+":
                                if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                                { _a_s = "-"; }
                                else
                                { _a_s = _dr["a_s"].ToString(); }
                                break;

                            case "-":
                                if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                                { _a_s = "-"; }
                                else
                                { _a_s = _dr["a_s"].ToString(); }
                                break;

                            default:
                                if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                                { _a_s = "-"; }
                                else
                                { _a_s = "+"; }
                                break;
                        }
                        _maValue = _maValue + (_a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion Evaluating Column values before the column edited

            #region Evaluating Column values after the column edited
            for (int i = _grdColPos; i < dgvItemDetails.Columns.Count; i++)
            {
                _dgvCol = (from _grdVwCol in dgvItemDetails.Columns.Cast<DataGridViewColumn>()
                           where _grdVwCol.DisplayIndex == i
                           select _grdVwCol).FirstOrDefault();

                if (Convert.ToString(_dgvCol.Tag) == "V#" || Convert.ToString(_dgvCol.Tag) == "S#" || Convert.ToString(_dgvCol.Tag) == "%#")
                {
                    if (Convert.ToString(_dgvCol.Tag) == "V#")
                    {
                        _fldNm = _dgvCol.DataPropertyName;
                    }
                    else
                    {
                        _dgvCol1 = (from _grdVwCol in dgvItemDetails.Columns.Cast<DataGridViewColumn>()
                                    where _grdVwCol.DisplayIndex == i + 1
                                    select _grdVwCol).FirstOrDefault();
                        _fldNm = _dgvCol1.DataPropertyName;
                    }
                    _dr = (_commonDs.Tables["DtDiscChrgs"].Select("Fld_Nm='" + _fldNm + "'").FirstOrDefault());
                    switch (_dr["A_S"].ToString())
                    {
                        case "+":
                            if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                            { _a_s = "-"; }
                            else
                            { _a_s = _dr["a_s"].ToString(); }
                            break;

                        case "-":
                            if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                            { _a_s = "-"; }
                            else
                            { _a_s = _dr["a_s"].ToString(); }
                            break;

                        default:
                            if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                            { _a_s = "-"; }
                            else
                            { _a_s = "+"; }
                            break;
                    }

                    _dgvCol1 = (from _grdVwCol in dgvItemDetails.Columns.Cast<DataGridViewColumn>()
                                where _grdVwCol.DisplayIndex == i - 1
                                select _grdVwCol).FirstOrDefault();
                    if (Convert.ToString(_dgvCol.Tag) == "%#" || Convert.ToString(_dgvCol1.Tag) == "%#")
                    {
                        _mAmt = 0M;
                        _cpertNm = _dr["Pert_Name"].ToString().Trim();
                        //_npertnm = Convert.ToDecimal(_commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentRow.Index][_cpertNm]);
                        _npertnm = Convert.ToDecimal(_commonDs.Tables["Item"].Rows[_grdRowPos][_cpertNm]);         // Changed by Sachin N. S. on 15/01/2016 for Bug-27503
                        if (_npertnm > 0)
                        {
                            if (_dr["Disp_Sign"].ToString().Trim() == "%")
                            {
                                _mAmt = _maValue * (_npertnm / 100);
                            }
                            if (Convert.ToString(_dgvCol1.Tag) == "%#")
                            {
                                if (Convert.ToDecimal(_dr1[_fldNm]) != _mAmt)
                                {
                                    _dr1[_cpertNm] = 0;
                                }
                            }
                        }
                        else
                        {
                            _dr1[_dr["Pert_Name"].ToString().Trim()] = 0;
                            if (_grdColPos != i)
                            {
                                //_mAmt = Convert.ToDecimal(_commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentRow.Index][_fldNm]);
                                _mAmt = Convert.ToDecimal(_commonDs.Tables["Item"].Rows[_grdRowPos][_fldNm]);      // Changed by Sachin N. S. on 15/01/2016 for Bug-27503
                            }
                        }

                        if (Convert.ToString(_dgvCol.Tag) == "%#")
                        {
                            _dr1[_fldNm] = _mAmt;
                        }
                    }

                    if (Convert.ToString(_dgvCol.Tag) == "S#" || Convert.ToString(_dgvCol1.Tag) == "S#")
                    {
                        _mAmt = 0.00M;
                        _cpertNm = _dr1["Tax_Name"].ToString().Trim();
                        if (_cpertNm != string.Empty)
                        {
                            _dr2 = _commonDs.Tables["Stax_Mas"].Select("Tax_Name='" + _cpertNm + "'").FirstOrDefault();
                            _npertnm = Convert.ToDecimal(_dr2["Level1"]);
                            if (_npertnm > 0)
                            {
                                _mAmt = _maValue * (_npertnm / 100);
                            }

                            if (Convert.ToString(_dgvCol.Tag) == "S#")
                            {
                                _dr1[_fldNm] = _mAmt;
                            }
                        }
                    }

                    if (Convert.ToString(_dgvCol.Tag) != "S#" && Convert.ToString(_dgvCol.Tag) != "%#")
                    {
                        if (_dr["Excl_Gross"].ToString() != "C" && _dr["Excl_Gross"].ToString() != "A" && _dr["Excl_Gross"].ToString() != "E" && _dr["Excl_Gross"].ToString() != "P")
                        {
                            _maValue += _a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]);

                            switch (_dr["Code"].ToString())
                            {
                                case "D":
                                    _dr1["Tot_deduc"] = Convert.ToDecimal(_dr1["Tot_deduc"]) + (_a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]));
                                    break;
                                case "T":
                                    _dr1["Tot_tax"] = Convert.ToDecimal(_dr1["Tot_tax"]) + _a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]);
                                    break;
                                case "E":
                                    _dr1["Tot_examt"] = Convert.ToDecimal(_dr1["Tot_examt"]) + _a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]);
                                    break;
                                case "A":
                                    _dr1["Tot_add"] = Convert.ToDecimal(_dr1["Tot_add"]) + _a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]);
                                    break;
                                case "N":
                                    _dr1["Tot_nontax"] = Convert.ToDecimal(_dr1["Tot_nontax"]) + _a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]);
                                    break;
                                case "F":
                                    _dr1["Tot_fdisc"] = Convert.ToDecimal(_dr1["Tot_fdisc"]) + _a_s == "+" ? +Convert.ToDecimal(_dr1[_fldNm]) : -Convert.ToDecimal(_dr1[_fldNm]);
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion Evaluating Column values after the column edited
        }
        // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ****** //
        #endregion Build dataset and Refresh controls

        #region Clear and reset Datatables
        private void clearTables()
        {
            foreach (DataTable _dt in _commonDs.Tables)
            {
                if (_dt.TableName != "Lcode" && _dt.TableName != "DCMast" && _dt.TableName != "DiscChrgsFldLst" && _dt.TableName != "HdDiscChrgs" && _dt.TableName != "DtDiscChrgs" && _dt.TableName != "Stax_Mas")   // ****** Changed by Sachin N. S. on 28/05/2013 for Bug-14538
                {
                    _dt.Clear();
                }
                if (_dt.TableName == "HdDiscChrgs")
                {
                    _dt.Select().ToList<DataRow>().ForEach(r => { r["Def_Pert"] = 0; r["Def_Amt"] = 0; });
                }
            }
        }
        #endregion Clear and reset Datatables

        #region Binding Controls
        private void BindControls()
        {
            txtInvoiceNo.DataBindings.Add("text", _commonDs, "Main.Inv_no");
            txtInvoiceDt.DataBindings.Add("text", _commonDs, "Main.Date");
            // Commented by Sachin N. S. on 06/08/2013 for Bug-14538 -- Start
            //txtCategory.DataBindings.Add("text", _commonDs, "Main.Cate");
            //txtDepartment.DataBindings.Add("text", _commonDs, "Main.dept");
            //txtInvoiceSr.DataBindings.Add("text", _commonDs, "Main.Inv_sr");
            //txtUserName.DataBindings.Add("text", _commonDs, "Main.user_Name");
            // Commented by Sachin N. S. on 06/08/2013 for Bug-14538 -- End
            txtGrossAmt.DataBindings.Add("text", _commonDs, "Main.Gro_amt");
            txtNetAmt.DataBindings.Add("text", _commonDs, "Main.Net_amt");
            txtTaxAmt.DataBindings.Add("text", _commonDs, "Main.TaxAmt");
            txtParty.DataBindings.Add("text", _commonDs, "Main.Party_nm");

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

            //****** Added by Sachin N. S. 25/06/2013 for Bug-14538 -- Start ******//
            //dgvItemDetails.Columns[4].DataPropertyName = "TaxAmt";    // Commented by Sachin N. S. on 25/06/2013 for Bug-14538
            int _colId = 4;
            DataRow _dr;
            DataRow[] _drdcmast;
            DataTable _dtdcmast;
            string _expStr = "U_asseamt";
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["I_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
            {
                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["I_Disc"]) == true)
                {
                    _drdcmast = _commonDs.Tables["DCMast"].Select("Att_file=false");
                    if (_drdcmast.Count() > 0)
                    {
                        _dtdcmast = _drdcmast.CopyToDataTable();
                    }
                    else
                    {
                        _dtdcmast = _commonDs.Tables["DCMast"].Clone();
                    }
                }
                else
                {
                    _dtdcmast = _commonDs.Tables["DCMast"].Clone();
                }
                _dtdcmast.TableName = "DtDiscChrgs";
                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
                {
                    _dr = _dtdcmast.NewRow();
                    _dr["Entry_ty"] = _commonDs.Tables["Lcode"].Rows[0]["Entry_ty"];
                    _dr["Code"] = "S";
                    _dr["fld_nm"] = "TAXAMT";
                    _dr["Round_off"] = false;
                    _dr["Att_file"] = false;
                    _dr["Disp_sign"] = "%";
                    _dr["Head_Nm"] = "NO-TAX";
                    _dr["Def_Pert"] = 0;
                    _dr["SortOrder"] = "EA1";
                    _dtdcmast.Rows.Add(_dr);
                }
                _dtdcmast.DefaultView.Sort = "SortOrder ASC";
                _dtdcmast = _dtdcmast.DefaultView.ToTable();
                _commonDs.Tables.Add(_dtdcmast);

                this.CreateItChrgsColDynamically(ref _expStr, ref _colId);
            }
            dgvItemDetails.Columns[4].DataPropertyName = "Gro_amt";
            dgvItemDetails.Columns[4].DisplayIndex = _colId;
            _colId += 1;
            dgvItemDetails.Columns[5].DataPropertyName = "";
            dgvItemDetails.Columns[5].DisplayIndex = _colId;

            //****** Added by Sachin N. S. 25/06/2013 for Bug-14538 -- End ******//

            //dgvItemDetails.Columns[5].DataPropertyName = "Gro_amt";   // Commented by Sachin N. S. 25/06/2013 for Bug-14538
            //dgvItemDetails.Columns[6].DataPropertyName = "";          // Commented by Sachin N. S. 25/06/2013 for Bug-14538

            _commonDs.Tables["Item"].Columns["U_asseamt"].Expression = "Qty*Rate";
            _commonDs.Tables["Item"].Columns["Gro_amt"].Expression = _expStr;       // Added by Sachin N. S. 25/06/2013 for Bug-14538
            //_commonDs.Tables["Item"].Columns["TaxAmt"].Expression = "(U_asseamt*TaxPercent)/100";     // Commented by Sachin N. S. 25/06/2013 for Bug-14538
            //_commonDs.Tables["Item"].Columns["Gro_amt"].Expression = "(Qty*Rate)+TaxAmt";             // Commented by Sachin N. S. 25/06/2013 for Bug-14538

            dgvItemDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            // ***** Binding Item Grid -- End ***** \\

            // ***** Binding Header-wise Discount and Charges Grid for Bug-14538 -- Start ***** \\
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true)
            {
                dgvDiscChrgs.AutoGenerateColumns = false;
                dgvDiscChrgs.ClearSelection();
                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
                {
                    _drdcmast = _commonDs.Tables["DCMast"].Select("Att_file=true");
                    if (_drdcmast.Count() > 0)
                    {
                        _dtdcmast = _drdcmast.CopyToDataTable();
                    }
                    else
                    {
                        _dtdcmast = _commonDs.Tables["DCMast"].Clone();
                    }
                }
                else
                {
                    _dtdcmast = _commonDs.Tables["DCMast"].Clone();
                }
                _dtdcmast.TableName = "HdDiscChrgs";
                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true)
                {
                    _dr = _dtdcmast.NewRow();
                    _dr["Entry_ty"] = _commonDs.Tables["Lcode"].Rows[0]["Entry_ty"];
                    _dr["Code"] = "S";
                    _dr["fld_nm"] = "TAXAMT";
                    _dr["Round_off"] = false;
                    _dr["Att_file"] = true;
                    _dr["Disp_sign"] = "%";
                    _dr["Head_Nm"] = "NO-TAX";
                    _dr["Def_Pert"] = 0;
                    _dr["SortOrder"] = "EA1";
                    _dtdcmast.Rows.Add(_dr);
                }
                _dtdcmast.DefaultView.Sort = "SortOrder ASC";
                _dtdcmast = _dtdcmast.DefaultView.ToTable();

                _commonDs.Tables.Add(_dtdcmast);
                dgvDiscChrgs.DataSource = _commonDs.Tables["HdDiscChrgs"];

                dgvDiscChrgs.Columns[0].DataPropertyName = "Head_Nm";
                dgvDiscChrgs.Columns[1].DataPropertyName = "Def_Pert";
                dgvDiscChrgs.Columns[2].DataPropertyName = "Def_Amt";
            }
            // ***** Binding Header-wise Discount and Charges Grid for Bug-14538 -- End ***** \\

            // ***** Added by Sachin N. S. on 27/05/2013 for Bug-14538 -- Start ***** //
            // ***** Binding the Summary of the Gross Amount, Tax structure and Net Amount -- Start ***** \\
            dgvSummary.AutoGenerateColumns = false;
            dgvSummary.ClearSelection();

            _dtSumDiscChrgs = new DataTable();
            _dtSumDiscChrgs.TableName = "SumTaxChrgs";
            _dtSumDiscChrgs.Columns.Add(new DataColumn("HeadingNm", typeof(string)));
            _dtSumDiscChrgs.Columns.Add(new DataColumn("Amount", typeof(decimal)));

            //DataRow _dr;
            _dr = _dtSumDiscChrgs.NewRow();
            _dr["HeadingNm"] = "Gross Amount";
            _dr["Amount"] = 0.00;
            _dtSumDiscChrgs.Rows.Add(_dr);

            if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "D").Count() > 0 && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Less : Dedn. Before Tax";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "T").Count() > 0 && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Taxable Charges";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "E").Count() > 0 && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Excise Duty";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "A").Count() > 0 && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Add/Less Charges";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["stax_tran"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Sales Tax";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "N").Count() > 0 && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Non Taxable Charges";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "F").Count() > 0 && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Less : Final Discount";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Net_op"]) == true)
            {
                _dr = _dtSumDiscChrgs.NewRow();
                _dr["HeadingNm"] = "Round Off";
                _dr["Amount"] = 0.00;
                _dtSumDiscChrgs.Rows.Add(_dr);
            }

            _dr = _dtSumDiscChrgs.NewRow();
            _dr["HeadingNm"] = "Net Amount";
            _dr["Amount"] = 0.00;
            _dtSumDiscChrgs.Rows.Add(_dr);

            dgvSummary.DataSource = _dtSumDiscChrgs;

            dgvSummary.Columns[0].DataPropertyName = "HeadingNm";
            dgvSummary.Columns[1].DataPropertyName = "Amount";

            // ***** Binding the Summary of the Gross Amount, Tax structure and Net Amount -- End ***** \\
            // ***** Added by Sachin N. S. on 27/05/2013 for Bug-14538 -- End ***** //

            //// ***** Binding Payment mode Grid -- Start ***** \\
            //dgvPayDetails.AutoGenerateColumns = false;
            //dgvPayDetails.ClearSelection();
            //dgvPayDetails.DataSource = _commonDs.Tables["PSPayDetail"];

            //dgvPayDetails.Columns[0].DataPropertyName = "Paymode";
            //dgvPayDetails.Columns[1].DataPropertyName = "TotalValue";
            //// ***** Binding Item Grid -- End ***** \\
        }

        //***** Added by Sachin N. S. on 25/06/2013 for Bug-14538 -- Start *****//
        private void CreateItChrgsColDynamically(ref string _expStr, ref int _colId)
        {
            udclsDGVNumericColumn.CNumEditDataGridViewColumn _dc;
            _expStr = (_expStr == "" ? "U_asseamt" : _expStr);
            string _a_s = "";
            foreach (DataRow _dr in _commonDs.Tables["DtDiscChrgs"].Rows)
            {
                if (_dr["a_s"].ToString().Trim() != string.Empty)
                {
                    _a_s = _dr["a_s"].ToString().Trim();
                }
                else if (_dr["Code"].ToString().Trim() == "D" || _dr["Code"].ToString().Trim() == "F")
                {
                    _a_s = "-";
                }
                else
                {
                    _a_s = "+";
                }

                if (_dr["Code"].ToString().Trim() != "S")
                {
                    if (_dr["Pert_Name"].ToString().Trim() != string.Empty)
                    {
                        _dc = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                        _dc.Name = "col" + _dr["Pert_Name"].ToString().Trim();
                        _dc.DataPropertyName = _dr["Pert_Name"].ToString().Trim();
                        _dc.HeaderText = _dr["Head_Nm"].ToString().Trim() + " %";
                        //_dc.HeaderText = " % ";
                        _dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        _dc.DecimalLength = 2;
                        _dc.DisplayIndex = _colId;
                        _dc.Tag = "%#";
                        dgvItemDetails.Columns.Add(_dc);
                        _colId += 1;
                    }

                    _dc = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                    _dc.Name = "col" + _dr["Fld_Nm"].ToString().Trim();
                    _dc.DataPropertyName = _dr["Fld_Nm"].ToString().Trim();
                    _dc.HeaderText = _dr["Head_Nm"].ToString().Trim();
                    _dc.DecimalLength = 2;
                    _dc.DisplayIndex = _colId;
                    //_dc.ReadOnly = true;
                    _dc.Tag = "V#";
                    dgvItemDetails.Columns.Add(_dc);

                    //                    _commonDs.Tables["Item"].Columns[_dr["Fld_Nm"].ToString().Trim()].Expression = "((" + _expStr + ")*" + _dr["Pert_Name"].ToString().Trim() + ")/100";
                    _colId += 1;
                    _expStr += _a_s + _dr["Fld_Nm"].ToString().Trim();
                }
                else
                {
                    //DataGridViewTextBoxColumn _tb;
                    DataGridViewComboBoxColumn _tb;
                    _tb = new DataGridViewComboBoxColumn();
                    _tb.Name = "colTax_Name";
                    _tb.DataPropertyName = "Tax_Name";
                    _tb.HeaderText = "Tax Name";
                    _tb.Items.AddRange(_commonDs.Tables["Stax_Mas"].AsEnumerable().Select(row => row["Tax_Name"].ToString()).ToArray());
                    _tb.Tag = "S#";
                    _tb.DisplayIndex = _colId;
                    _tb.Width = 150;
                    dgvItemDetails.Columns.Add(_tb);
                    _colId += 1;

                    _dc = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                    _dc.Name = "col" + _dr["Fld_Nm"].ToString().Trim();
                    _dc.DataPropertyName = _dr["Fld_Nm"].ToString().Trim();
                    _dc.HeaderText = "Tax Amount";
                    _dc.DecimalLength = 2;
                    _dc.DisplayIndex = _colId;
                    _dc.ReadOnly = true;
                    _dc.Tag = "V#";
                    dgvItemDetails.Columns.Add(_dc);

                    //_commonDs.Tables["Item"].Columns[_dr["Fld_Nm"].ToString().Trim()].Expression = _commonDs.Tables["Item"].Columns[_dr["Fld_Nm"].ToString().Trim()].Expression = "((" + _expStr + ")*TaxPer)/100";
                    _taxExpr = _expStr;
                    _colId += 1;
                    _expStr += _a_s + _dr["Fld_Nm"].ToString().Trim();
                }
            }

            _commonDs.Tables["Item"].Columns["Gro_amt"].Expression = _expStr;
        }
        //***** Added by Sachin N. S. on 25/06/2013 for Bug-14538 -- End *****//
        #endregion

        #region Calling Popup Selection Screen
        private void popUpFromTable(string _popupType)
        {
            _oDataAccess = new clsDataAccess();
            string cSql = "";
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            string cTbl = "", cCond = "", cFldList = "", xcFldList = "", xcSrchCol = "", xcDispCol = "", xcRetCol = "";
            string[][] cFldLst;     //***** Added by Sachin N. S. on 23/05/2013 for Bug-14538
            int _lRateLevel = -1;    //***** Added by Sachin N. S. on 23/05/2013 for Bug-14538
            DataTable _dt = new DataTable();
            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            switch (_popupType)
            {
                //***** Added by Sachin N. S. on 23/05/2013 for Bug-14538 -- Start *****//
                case "PARTY":
                    xcFldList = "";
                    xcDispCol = "";
                    xcRetCol = "";
                    cTbl = " Ac_Mast ";
                    cCond = " Ac_Id!=0 ";

                    cFldList = _commonDs.Tables["Lcode"].Rows[0]["Ac_fields"].ToString();
                    cFldLst = cFldList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Split(':')).ToArray();
                    var cAcId =
                        from _cFldLst in cFldLst
                        where _cFldLst[0].ToUpper().Contains("AC_ID")
                        select _cFldLst[0];
                    if (cAcId.Count<string>() == 0)
                    {
                        xcFldList += ",Ac_mast.Ac_Id ";
                    }
                    //xcFldList += ",Ac_mast.DISCPERBFT,Ac_mast.DISCPERAFT ";   // Commented by Sachin N. S. on 18/07/2013 for Bug-14538
                    //xcRetCol += ",DISCPERBFT,DISCPERAFT";                     // Commented by Sachin N. S. on 18/07/2013 for Bug-14538

                    cCond += " And (Ac_mast.ldeactive = 0 Or (Ac_mast.ldeactive = 1 And Ac_mast.deactfrom > '" + txtInvoiceDt.Text.ToString() + "' )) ";
                    cSql = "set dateformat dmy select " + cFldLst[0][0] + xcFldList + " from " + cTbl + " where " + cCond + " order by Ac_Name ";
                    cFrmCap = "Select Party";
                    cSrchCol = "Ac_Name" + xcSrchCol;
                    cDispCol = "Ac_Name:Party" + xcDispCol;
                    cRetCol = "Ac_Id,Ac_Name" + xcRetCol;
                    break;

                case "WAREHOUSE":
                    xcFldList = "";
                    xcDispCol = "";
                    xcRetCol = "";
                    cTbl = " Warehouse ";
                    cCond = " Ware_nm!='' ";

                    cFldList = "Ware_nm";

                    cCond += " And Validity like '%" + Convert.ToString(_commonDs.Tables["Lcode"].Rows[0]["Entry_Ty"]) + "%' ";
                    cSql = "set dateformat dmy select Ware_nm from Warehouse where " + cCond + " order by Ware_nm ";
                    cFrmCap = "Select Warehouse";
                    cSrchCol = "Ware_nm";
                    cDispCol = "Ware_nm:Warehouse" + xcDispCol;
                    cRetCol = "Ware_nm" + xcRetCol;
                    break;

                //***** Added by Sachin N. S. on 23/05/2013 for Bug-14538 -- End *****//

                case "ITEM":
                    xcFldList = ",It_mast.Rate,It_mast.Tax_Name,It_Mast.InclSTTax,It_Mast.AskQty,It_Mast.AskRate ";                // Changed by Sachin N. S. on 18/01/2016 for Bug-27503
                    xcDispCol = ",Rate:Item Rate,Tax_Name:Tax Name,InclSTTax:Incl of Sales Tax,AskQty: Ask Quantity,AskRate:Ask Rate";    // Changed by Sachin N. S. on 18/01/2016 for Bug-27503
                    xcRetCol = ",Rate,Tax_Name,InclSTTax,AskQty,AskRate";                                          // Changed by Sachin N. S. on 18/01/2016 for Bug-27503
                    cTbl = " It_Mast ";
                    cCond = " It_Mast.It_code!=0 ";

                    //******* Checking for Rate in Item Rate master -- Start *******\\
                    if ((bool)_commonDs.Tables["Lcode"].Rows[0]["It_Rate"] == true)
                    {
                        string cSql1 = "Select top 1 Rate_Level From Ac_mast where Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString();
                        _dt = _oDataAccess.GetDataTable(cSql1, null, 50);
                        _lRateLevel = Convert.ToInt16(_dt.Rows[0]["Rate_Level"]);           // Added by Sachin N. S. on 29/06/2013 for Bug-14538
                        if (Convert.ToInt16(_dt.Rows[0]["Rate_Level"]) != 0)
                        {
                            cCond += " And (It_mast.It_code = It_rate.It_code And (It_rate.Rlevel = " + _lRateLevel.ToString() + ")) ";
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
                    cFldLst = cFldList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Split(':')).ToArray();
                    var cItCode =
                        from _cFldLst in cFldLst
                        where _cFldLst[0].ToUpper().Contains("IT_CODE")
                        select _cFldLst[0];
                    if (cItCode.Count<string>() == 0)
                    {
                        xcFldList += ",It_mast.It_Code ";
                    }

                    string cFldStr = splitText(cFldLst, ref xcSrchCol, ref xcDispCol);  //***** Added by Sachin N. S. on 13/08/2015 for Bug-26654                    

                    cCond += " And (It_mast.ldeactive = 0 Or (It_mast.ldeactive = 1 And It_mast.deactfrom > '" + txtInvoiceDt.Text.ToString() + "' )) ";
                    cSql = "set dateformat dmy select " + cFldStr + xcFldList + " from " + cTbl + " where " + cCond + " order by It_Name ";
                    cFrmCap = "Select Item";
                    cSrchCol = xcSrchCol;
                    cDispCol = xcDispCol;
                    cRetCol = "It_Code,It_Name" + xcRetCol;
                    break;

                //********* Added by Sachin N. S. on 20/06/2013 for Bug-14538 -- Start ********//
                case "SALESTAX":
                    string _stType = "";
                    cSql = "Select top 1 St_Type from Ac_Mast where ac_id=" + _commonDs.Tables["Main"].Rows[0]["Ac_id"].ToString();
                    _dt = _oDataAccess.GetDataTable(cSql, null, 50);
                    if (_dt.Rows.Count > 0)
                    {
                        _stType = _dt.Rows[0][0].ToString().Trim();
                    }

                    xcFldList = "";
                    xcDispCol = "";
                    xcRetCol = "";
                    cTbl = " STax_Mas ";
                    cCond = " Entry_ty = '" + _commonDs.Tables["Main"].Rows[0]["Entry_ty"].ToString() + "' And (Stax_Mas.ldeactive = 0 Or (Stax_Mas.ldeactive = 1 And Stax_Mas.deactfrom > '" + _commonDs.Tables["Main"].Rows[0]["Date"].ToString() + "'))";
                    cCond += _stType == string.Empty ? "" : " and sTax_Mas.St_Type='" + _stType.ToString() + "'";

                    cSql = "Set dateformat dmy Select Stax_Mas.Tax_Name,Stax_Mas.Level1 From " + cTbl + " where " + cCond + " order by Stax_Mas.Tax_Name ";
                    cFrmCap = "Select Sales Tax";
                    cSrchCol = "Tax_Name";
                    cDispCol = "Tax_Name:Tax_Name";
                    cRetCol = "Tax_Name,Level1";
                    break;
                //********* Added by Sachin N. S. on 20/06/2013 for Bug-14538 -- End ********//

                case "PRINTBILL":
                    xcFldList = ",It_mast.Rate,It_mast.Tax_Name ";
                    xcDispCol = ",Rate:Item Rate,Tax_Name:Tax Name";
                    xcRetCol = ",Rate,Tax_Name";
                    cTbl = " DCMAIN ";
                    cCond = " Entry_ty='PS' and DATE='" + DateTime.Now.Date + "' ";

                    cSql = "set dateformat dmy select Inv_No,Tran_cd from " + cTbl + " where " + cCond + " order by Inv_No ";
                    cFrmCap = "Select Bill No.";
                    cSrchCol = "Inv_No";
                    cDispCol = "Inv_No:Bill No.";
                    cRetCol = "Tran_cd,Inv_No";
                    break;

                // ******* Added by Sachin N. S. on 21/01/2016 for Bug-27503 -- Start
                case "CANCELBILL":
                    xcFldList = "";
                    xcDispCol = "";
                    xcRetCol = "";
                    cTbl = " DCMAIN ";
                    cCond = " Entry_Ty='PS' and Party_nm!='CANCELLED.' ";

                    cFldList = "Inv_No,Date";

                    cCond += " ";
                    cSql = "set dateformat dmy select Tran_cd,Inv_No,Date from DCMAIN where " + cCond + " order by Date ";
                    cFrmCap = "Select Bill to Cancel";
                    cSrchCol = "Inv_No";
                    cDispCol = "Inv_No:Bill No.,Date:Bill Date" + xcDispCol;
                    cRetCol = "Tran_cd" + xcRetCol;
                    break;
                // ******* Added by Sachin N. S. on 21/01/2016 for Bug-27503 -- End
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
                    // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ****** //
                    case "PARTY":
                        _commonDs.Tables["Main"].Rows[this.BindingContext[_commonDs, "Main"].Position]["Ac_Id"] = _oSelPop.pReturnArray[0].ToString();
                        _commonDs.Tables["Main"].Rows[this.BindingContext[_commonDs, "Main"].Position]["Party_Nm"] = _oSelPop.pReturnArray[1].ToString();

                        //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- Start *****//
                        clsInsertDefaultValue.replNewvalueinChldTable("Item", "Ac_Id", _oSelPop.pReturnArray[0].ToString());
                        clsInsertDefaultValue.replNewvalueinChldTable("Item", "Party_Nm", _oSelPop.pReturnArray[1].ToString());

                        if (Convert.ToInt16(CommonInfo.PartyId) != Convert.ToInt16(_oSelPop.pReturnArray[0]) && (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true || CommonInfo.HdrDiscFlds != ""))
                        {
                            clsInsertDefaultValue.GetPartyDefaDiscChrgsValue(Convert.ToInt16(_oSelPop.pReturnArray[0]));
                            replDefaDiscChrgsValues("PARTY", 0);
                        }
                        //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- End *****//
                        break;

                    case "WAREHOUSE":
                        this.txtWareHouse.Text = _oSelPop.pReturnArray[0].ToString();
                        clsInsertDefaultValue.replNewvalueinChldTable("Item", "Ware_Nm", _oSelPop.pReturnArray[0].ToString());
                        CommonInfo.ChngdWareHouse = _oSelPop.pReturnArray[0].ToString();
                        break;
                    // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ****** //

                    case "ITEM":
                        // ****** Added by Sachin N. S. on 20/08/2015 for Bug-26654 -- Start 
                        int _nRowIndex = this.BindingContext[_commonDs, "Item"].Position;       // Added by Sachin N. S. on 15/01/2016 for Bug-27503
                        _curRowNo = _nRowIndex;     // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                        if (Stock_Checking(Convert.ToInt16(_oSelPop.pReturnArray[0].ToString()), Convert.ToDateTime(this.txtInvoiceDt.Value), 1, this.dgvItemDetails.CurrentRow.Index) <= 0)
                        {
                            MessageBox.Show("Item out of Stock.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // ****** Added by Sachin N. S. on 20/08/2015 for Bug-26654 -- End

                            string clAdd = "Add";
                            if (_commonDs.Tables["Item"].Rows.Count > 0)
                            {
                                var _currentRow = (from currentRw in _commonDs.Tables["Item"].AsEnumerable()
                                                   where currentRw.Field<decimal>("It_code") == Convert.ToDecimal(_oSelPop.pReturnArray[0].ToString())
                                                   select currentRw).FirstOrDefault();
                                if (_currentRow != null && _currentRow.Table.Rows.Count > 0)
                                {
                                    _currentRow["Qty"] = Convert.ToDecimal(_currentRow["Qty"]) + 1;
                                    clAdd = "EXIST";
                                    _nRowIndex = _commonDs.Tables["Item"].Rows.IndexOf(_commonDs.Tables["Item"].Select("It_Code='" + Convert.ToDecimal(_oSelPop.pReturnArray[0].ToString()) + "'")[0]);     // Added by Sachin N. S. on 15/01/2016 for Bug-27503
                                    _curRowNo = _nRowIndex;     // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                                }
                            }

                            if (clAdd == "Add")
                            {
                                this.BindingContext[_commonDs, "Item"].Position = dgvItemDetails.CurrentCell.RowIndex;      // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                                _curRowNo = this.BindingContext[_commonDs, "Item"].Position;                                // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                                _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["It_Code"] = _oSelPop.pReturnArray[0].ToString();
                                _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = _oSelPop.pReturnArray[1].ToString();
                                _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Qty"] = 1;      // Added by Sachin N. S. on 15/01/2016 for Bug-27503
                                string[] _arr = cRetCol.Split(',');
                                int _iRate = Array.IndexOf(_arr, "Rate");
                                int _iItRate = Array.IndexOf(_arr, "It_Rate");
                                int _iInclTx = Array.IndexOf(_arr, "InclSTTax");        // Added by Sachin N. S. on 18/01/2016 for Bug-27503

                                //***** Commented by Sachin N. S. on 29/06/2013 for Bug-14538 -- Start *****//
                                //int _TaxName = Array.IndexOf(_arr, "Tax_Name");
                                //if (_oSelPop.pReturnArray[_TaxName].ToString() != "")
                                //{
                                //    cSql = "SET DATEFORMAT DMY; SELECT LEVEL1 FROM STAX_MAS WHERE ENTRY_TY='PS' AND TAX_NAME = '" + _oSelPop.pReturnArray[_TaxName].ToString() + "' and '" + this.txtInvoiceDt.Value.ToString() + "' between Wefstkfrom and Wefstkto ";
                                //    _dt = _oDataAccess.GetDataTable(cSql, null, 50);
                                //    if (_dt.Rows.Count > 0)
                                //    {
                                //        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["TAXPERCENT"] = _dt.Rows[0]["Level1"];
                                //    }
                                //}
                                //***** Commented by Sachin N. S. on 29/06/2013 for Bug-14538 -- End *****//

                                _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"] = (_iItRate > 0 ? (Convert.ToDecimal(_oSelPop.pReturnArray[_iItRate]) > 0 ? _oSelPop.pReturnArray[_iItRate] : _oSelPop.pReturnArray[_iRate]) : _oSelPop.pReturnArray[_iRate]);
                                //_commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Tax_Name"] = _oSelPop.pReturnArray[_TaxName];     //***** Commented by Sachin N. S. on 29/06/2013 for Bug-14538 

                                //***** Added by Sachin N. S. on 29/06/2013 for Bug-14538 -- Start *****//
                                if (_lRateLevel == -1)
                                {
                                    string cSql1 = "Select top 1 Rate_Level From Ac_Mast where Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString();
                                    _dt = _oDataAccess.GetDataTable(cSql1, null, 50);
                                    _lRateLevel = Convert.ToInt16(_dt.Rows[0]["Rate_Level"]);
                                    cTbl += ",It_Rate ";
                                }
                                _lRateLevel = _lRateLevel == -1 ? 0 : _lRateLevel;
                                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["I_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
                                {
                                    replDefaDiscChrgsValues("ITEM", _lRateLevel);
                                }
                                //***** Added by Sachin N. S. on 29/06/2013 for Bug-14538 -- End *****//

                                //***** Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- Start *****//
                                if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true && _iInclTx > 0)
                                {
                                    if (Convert.ToBoolean(_oSelPop.pReturnArray[_iInclTx]) == true)
                                    {
                                        decimal _npertnm = 0.00M;
                                        string _cpertNm = _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Tax_Name"].ToString().Trim();
                                        if (_cpertNm != string.Empty)
                                        {
                                            DataRow _dr2;
                                            _dr2 = _commonDs.Tables["Stax_Mas"].Select("Tax_Name='" + _cpertNm + "'").FirstOrDefault();
                                            _npertnm = Convert.ToDecimal(_dr2["Level1"]);
                                        }
                                        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"] = (Convert.ToDecimal(_commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"]) * 100) / (100 + _npertnm);
                                    }
                                }
                                //***** Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- End *****//
                            }
                        }   // ****** Added by Sachin N. S. on 20/08/2015 for Bug-26654
                        itemwiseTotal(_nRowIndex, 0);      // Added by Sachin N. S. on 14/01/2016 for Bug-27503
                        break;

                    //***** Added by Sachin N. S. on 20/06/2013 for Bug-14538 -- Start *****//
                    case "SALESTAX":
                        if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true)
                        {
                            _commonDs.Tables["Main"].Rows[0]["Tax_Name"] = _oSelPop.pReturnArray[0].ToString();
                            //_commonDs.Tables["Main"].Rows[0]["TaxPercent"] = _oSelPop.pReturnArray[1].ToString();     // Commented by Sachin N. S. on 25/07/2013 for Bug-14538

                            if (Convert.ToDecimal(_oSelPop.pReturnArray[1]) > 0)
                            {
                                var _currentRow = (from currentRw in _commonDs.Tables["HdDiscChrgs"].AsEnumerable()
                                                   where currentRw.Field<string>("Code") == "S"
                                                   select currentRw).FirstOrDefault();
                                _currentRow["Head_nm"] = _oSelPop.pReturnArray[0].ToString();
                                _currentRow["Def_Pert"] = Convert.ToDecimal(_oSelPop.pReturnArray[1]);
                                this.CalculateHeaderTax(_currentRow["Fld_nm"].ToString());
                                this.RefreshControls();
                            }
                        }
                        if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
                        {
                            _commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentRow.Index]["Tax_Name"] = _oSelPop.pReturnArray[0].ToString();
                            _commonDs.Tables["Item"].Rows[dgvItemDetails.CurrentRow.Index]["TaxPer"] = Convert.ToDecimal(_oSelPop.pReturnArray[1]);
                        }
                        itemwiseTotal(dgvItemDetails.CurrentCell.RowIndex, dgvItemDetails.CurrentCell.ColumnIndex);      // Changed by Sachin N. S. on 15/01/2016 for Bug-27503
                        break;
                    //***** Added by Sachin N. S. on 20/06/2013 for Bug-14538 -- End *****//

                    case "PRINTBILL":
                        if (_oSelPop.pReturnArray[0].ToString() != "")
                        {
                            PrintBill(Convert.ToInt16(_oSelPop.pReturnArray[0].ToString()));
                            MessageBox.Show("Bill No. " + _oSelPop.pReturnArray[1].ToString() + " printed successfully.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No bills selected to print.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;

                    // ***** Added by Sachin N. S. on 21/01/2016 for Bug-27503 -- Start
                    case "CANCELBILL":
                        this.pnlPrintBill.Visible = true;
                        this.lblPrintBill.Visible = true;
                        this.pgrsBar.Visible = true;
                        this.lblPrintBill.Text = "Cancelling Bill......";

                        if (Convert.ToInt16(_oSelPop.pReturnArray[0]) == 0)
                        {
                            MessageBox.Show("No bill selected to Cancel.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (clsInsUpdDelPrint.CancelRecords(Convert.ToInt16(_oSelPop.pReturnArray[0])) == false)
                            {
                                MessageBox.Show("Could not cancel the Bill. Try again...", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Bill Cancelled Successfully", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        this.pnlPrintBill.Visible = false;
                        this.lblPrintBill.Visible = false;
                        this.pgrsBar.Visible = false;
                        break;
                    // ***** Added by Sachin N. S. on 21/01/2016 for Bug-27503 -- End
                }
            }
        }

        #region Read Barcode
        private string ReadBarcode(string _barCode)
        {
            _oDataAccess = new clsDataAccess();
            string cSql = "";
            string cSrchCol = "", cDispCol = "", cRetCol = "";
            string cTbl = "", cCond = "", cFldList = "", xcFldList = "", xcSrchCol = "", xcDispCol = "", xcRetCol = "";
            int _lRateLevel = -1;    //***** Added by Sachin N. S. on 18/01/2016 for Bug-27503

            DataTable _dt1 = new DataTable();
            DataTable _dt = new DataTable();
            xcFldList = ",It_mast.Rate,It_mast.Tax_Name,It_Mast.InclSTTax ";                    // Changed by Sachin N. S. on 18/01/2016 for Bug-27503
            xcDispCol = ",Rate:Item Rate,Tax_Name:Tax Name,InclSTTax:Incl of Sales Tax";        // Changed by Sachin N. S. on 18/01/2016 for Bug-27503
            xcRetCol = ",Rate,Tax_Name,InclSTTax";                                              // Changed by Sachin N. S. on 18/01/2016 for Bug-27503
            cTbl = " It_Mast ";
            cCond = " It_Mast.It_code!=0 ";         //***** Changed by Sachin N. S. on 26/08/2015 for Bug-26654

            //******* Checking for Rate in Item Rate master -- Start *******\\
            if ((bool)_commonDs.Tables["Lcode"].Rows[0]["It_Rate"] == true)
            {
                string cSql1 = "Select top 1 Rate_Level From Ac_mast where Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString();
                _dt = _oDataAccess.GetDataTable(cSql1, null, 50);
                _lRateLevel = Convert.ToInt16(_dt.Rows[0]["Rate_Level"]);           // Added by Sachin N. S. on 18/01/2016 for Bug-27503
                if (Convert.ToInt16(_dt.Rows[0]["Rate_Level"]) != 0)    //***** Changed by Sachin N. S. on 26/08/2015 for Bug-26654
                {
                    cCond += " And (It_mast.It_code = It_rate.It_code And (It_rate.Rlevel = " + _dt.Rows[0]["Rate_Level"].ToString() + ")) ";
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

            //******* Searching for Barcode value in BarcodeTran Table -- Start *******\\
            if (_commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString().Trim() != "")
            {
                cCond += " And ((BarCodeTran.Entry_ty='IM' and BarCodeTran." + _commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString().Trim() + " = '" + _barCode.ToString() + "' and BarCodeTran.Tran_cd = It_Mast.It_Code ) )";
                cTbl += ",BarcodeTran ";
                xcFldList += ",BarCodeTran." + _commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString().Trim();
                xcDispCol += "," + _commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString().Trim() + ":Bar Code";
                xcRetCol += "," + _commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString().Trim();
            }
            else
            {
                if (_barCode.ToString() != string.Empty)
                {
                    cCond += " And It_Mast.It_Name = '" + _barCode.ToString() + "' ";
                }
            }
            //******* Searching for Barcode value in BarcodeTran Table -- End *******\\

            cFldList = _commonDs.Tables["Lcode"].Rows[0]["It_fields"].ToString();
            string[][] cFldLst = cFldList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Split(':')).ToArray();
            var cItCode =
                from _cFldLst in cFldLst
                where _cFldLst[0].ToUpper().Contains("IT_CODE")
                select _cFldLst[0];
            if (cItCode.Count<string>() == 0)
            {
                xcFldList += ",It_mast.It_Code ";
            }

            ////Added for New field reference in Barcode -- 04/04/2013 -- Start
            //if (_commonDs.Tables["Lcode"].Columns["POSBarcode"]!=null)
            //{
            //    if (_commonDs.Tables["Lcode"].Rows[0]["POSBarcode"] != string.Empty)
            //    {
            //        cFldList = ">>"+_commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString()+"<<";
            //        string[][] cFldLst1 = cFldList.Split(new[] { ">><<" }, StringSplitOptions.RemoveEmptyEntries).Select(row => row.Split(':')).ToArray();
            //    }
            //}
            ////Added for New field reference in Barcode -- 04/04/2013 -- End

            cCond += " And (It_mast.ldeactive = 0 Or (It_mast.ldeactive = 1 And It_mast.deactfrom > '" + txtInvoiceDt.Text.ToString() + "' )) ";
            cSql = "set dateformat dmy; select " + cFldLst[0][0] + xcFldList + " from " + cTbl + " where " + cCond + " order by It_Name ";
            //cFrmCap = "Select Item";
            cSrchCol = "It_Name" + xcSrchCol;
            cDispCol = "It_Name:Item" + xcDispCol;
            cRetCol = "It_Code,It_Name" + xcRetCol;

            //MessageBox.Show(cSql);

            _dt = _oDataAccess.GetDataTable(cSql, null, 50);
            string clAdd = "ADD";
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    int _nRowIndex = this.BindingContext[_commonDs, "Item"].Position;       // Added by Sachin N. S. on 15/01/2016 for Bug-27503
                    _curRowNo = _nRowIndex;     // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                    if (_commonDs.Tables["Item"].Rows.Count > 0)
                    {
                        var _currentRow = (from currentRw in _commonDs.Tables["Item"].AsEnumerable()
                                           where currentRw.Field<decimal>("It_code") == Convert.ToDecimal(_dt.Rows[0]["It_Code"])
                                           select currentRw).FirstOrDefault();
                        if (_currentRow != null && _currentRow.Table.Rows.Count > 0)
                        {
                            //********* Added by Sachin N. S. on 19/02/2016 for Bug-27503 -- Start
                            _nRowIndex = _commonDs.Tables["Item"].Rows.IndexOf(_commonDs.Tables["Item"].Select("It_Code='" + Convert.ToDecimal(_dt.Rows[0]["It_Code"]) + "'")[0]);
                            if (Stock_Checking(Convert.ToInt16(_dt.Rows[0]["It_Code"].ToString()), Convert.ToDateTime(this.txtInvoiceDt.Value), 1, _nRowIndex) <= 0)
                            {
                                MessageBox.Show("Item out of Stock.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                clAdd = "NOTFOUND";
                            }
                            else
                            {
                                _currentRow["Qty"] = Convert.ToDecimal(_currentRow["Qty"]) + 1;
                                clAdd = "EXIST";
                            }
                            //********* Added by Sachin N. S. on 19/02/2016 for Bug-27503 -- End
                            _curRowNo = _nRowIndex;     // Added by Sachin N. S. on 19/01/2016 for Bug-27503
                        }
                    }

                    if (clAdd == "ADD")
                    {
                        if (Stock_Checking(Convert.ToInt16(_dt.Rows[0]["It_Code"].ToString()), Convert.ToDateTime(this.txtInvoiceDt.Value), 1, _nRowIndex) <= 0)
                        {
                            MessageBox.Show("Item out of Stock.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clAdd = "NOTFOUND";
                        }
                        else
                        {
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["It_Code"] = _dt.Rows[0]["It_Code"].ToString();
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = _dt.Rows[0]["It_name"].ToString();
                            string[] _arr = cRetCol.Split(',');
                            int _iRate = Array.IndexOf(_arr, "Rate");
                            int _iItRate = Array.IndexOf(_arr, "It_Rate");
                            int _iInclTx = Array.IndexOf(_arr, "InclSTTax");        // Added by Sachin N. S. on 18/01/2016 for Bug-27503
                            //int _TaxName = Array.IndexOf(_arr, "Tax_Name");           // Commented by Sachin N. S. on 18/01/2016 for Bug-27503

                            // ******* Commented by Sachin N. S. on 25/07/2013 for Bug-14538 -- Start ******* //
                            //if (_dt.Rows[0]["Tax_Name"].ToString() != "")
                            //{
                            //    cSql = "SET DATEFORMAT DMY; SELECT LEVEL1 FROM STAX_MAS WHERE ENTRY_TY='PS' AND TAX_NAME = '" + _dt.Rows[0]["Tax_Name"].ToString() + "' and '" + this.txtInvoiceDt.Value.ToString() + "' between Wefstkfrom and Wefstkto ";
                            //    _dt1 = _oDataAccess.GetDataTable(cSql, null, 50);
                            //    if (_dt1.Rows.Count > 0)
                            //    {
                            //        _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["TAXPERCENT"] = _dt1.Rows[0]["Level1"];
                            //    }
                            //}
                            // ******* Commented by Sachin N. S. on 25/07/2013 for Bug-14538 -- End ******* //
                            _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"] = (_iItRate > 0 ? (Convert.ToInt16(_dt.Rows[0]["It_Rate"]) > 0 ? _dt.Rows[0]["It_Rate"] : _dt.Rows[0]["Rate"]) : _dt.Rows[0]["Rate"]);
                            bool _lInclStTax = (_iInclTx > 0 ? Convert.ToBoolean(_dt.Rows[0]["InclSTTax"]) : false);
                            //_commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Tax_Name"] = _dt.Rows[0]["Tax_Name"].ToString().Trim() == "" ? "NO-TAX" : _dt.Rows[0]["Tax_Name"].ToString().Trim();          // Commented by Sachin N. S. on 18/01/2016 for Bug-27503

                            //***** Added by Sachin N. S. on 29/06/2013 for Bug-14538 -- Start *****//
                            if (_lRateLevel == -1)
                            {
                                string cSql1 = "Select top 1 Rate_Level From Ac_Mast where Ac_id = " + _commonDs.Tables["Main"].Rows[0]["Ac_Id"].ToString();
                                _dt = _oDataAccess.GetDataTable(cSql1, null, 50);
                                _lRateLevel = Convert.ToInt16(_dt.Rows[0]["Rate_Level"]);
                                cTbl += ",It_Rate ";
                            }
                            _lRateLevel = _lRateLevel == -1 ? 0 : _lRateLevel;
                            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["I_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
                            {
                                replDefaDiscChrgsValues("ITEM", _lRateLevel);
                            }
                            //***** Added by Sachin N. S. on 29/06/2013 for Bug-14538 -- End *****//

                            //***** Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- Start *****//
                            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true && _iInclTx > 0)
                            {
                                if (_lInclStTax == true)
                                {
                                    decimal _npertnm = 0.00M;
                                    string _cpertNm = _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Tax_Name"].ToString().Trim();
                                    if (_cpertNm != string.Empty)
                                    {
                                        DataRow _dr2;
                                        _dr2 = _commonDs.Tables["Stax_Mas"].Select("Tax_Name='" + _cpertNm + "'").FirstOrDefault();
                                        _npertnm = Convert.ToDecimal(_dr2["Level1"]);
                                    }
                                    _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"] = (Convert.ToDecimal(_commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Rate"]) * 100) / (100 + _npertnm);
                                }
                            }
                            //***** Added by Sachin N. S. on 18/01/2016 for Bug-27503 -- End *****//
                        }
                    }
                    itemwiseTotal(_nRowIndex, 0);       // Added by Sachin N. S. on 15/01/2016 for Bug-27503
                }
                else
                {
                    MessageBox.Show("Validation failed for either of following reasons: \n" +
                        "1. Item not found in the Item Master. \n" +
                        (((bool)_commonDs.Tables["Lcode"].Rows[0]["It_Rate"] == true) ? "2. Rate not defined in the Price list master. \n" : "") +
                        ((_commonDs.Tables["Lcode"].Rows[0]["POSBarcode"].ToString().Trim() != "") ? (((bool)_commonDs.Tables["Lcode"].Rows[0]["It_Rate"] == true) ? "3." : "2.") + " Barcode value not matching with the value defined in Item Master." : "") , CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clAdd = "NOTFOUND";
                }
            }
            //else
            //{
            //    MessageBox.Show("Item not found in the Item Master.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["It_Code"] = 0;
            //    _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["Item"] = "";
            //    clAdd = "NOTFOUND";
            //}

            return clAdd;
        }
        #endregion Read Barcode

        #endregion Calling Popup Selection Screen

        #region Stock Checking
        private decimal Stock_Checking(int _It_Code, DateTime _InvDt, decimal _CurstkVal, int _rowIndex)
        {
            decimal StkQty;

            if (CommonInfo.Neg_itBal == 0)
            {
                string cSql;
                cSql = "Set dateformat DMY;";
                cSql += " Select SUM(case when b.inv_stk='+' then a.Qty else case when b.Inv_stk='-' then -Qty else 0 end end ) as Qty From It_balw A,Lcode B where ";
                cSql += " a.It_code = " + _It_Code.ToString();
                cSql += " and a.Date <= '" + _InvDt.ToString() + "'";
                cSql += " and a.Entry_ty = b.Entry_ty and b.Inv_stk!='' ";
                DataTable _dt;
                _dt = _oDataAccess.GetDataTable(cSql, null, 50);
                decimal.TryParse(_dt.Rows[0]["Qty"].ToString(), out StkQty);
                StkQty = StkQty - Convert.ToDecimal(_commonDs.Tables["Item"].AsEnumerable().Where((row, index) => Convert.ToInt16(row["It_Code"]) == _It_Code && row["Qty"] != null && index != _rowIndex).Sum(row => Convert.ToDecimal(row["Qty"]))) - _CurstkVal;
            }
            else
            {
                StkQty = 1;     // Added by Sachin N. S. on 14/01/2016 for Bug-27503 for bypassing Stock checking
            }
            return StkQty;
        }
        #endregion Stock Checking

        // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ****** //
        #region Calculate Headerwise Taxes
        private void CalculateHeaderTax(string _HeadFld)
        {
            bool _calcTax = false;
            decimal mcalamt = 0, mpert = 0;
            string mfld_nm = "", mper_nm = "";

            //****** Added by Sachin N. S. on 25/05/2013 for Bug-14538 -- Start ******//
            _commonDs.Tables["Main"].Rows[0]["Tot_Deduc"] = 0;
            _commonDs.Tables["Main"].Rows[0]["tot_tax"] = 0;
            _commonDs.Tables["Main"].Rows[0]["tot_examt"] = 0;
            _commonDs.Tables["Main"].Rows[0]["tot_add"] = 0;
            _commonDs.Tables["Main"].Rows[0]["tot_nontax"] = 0;
            _commonDs.Tables["Main"].Rows[0]["tot_fdisc"] = 0;
            //****** Added by Sachin N. S. on 25/05/2013 for Bug-14538 -- End ******//

            if (Convert.ToBoolean(_commonDs.Tables["LCode"].Rows[0]["V_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["LCode"].Rows[0]["Stax_Tran"]) == true)
            {
                foreach (DataRow _dr in _commonDs.Tables["HdDiscChrgs"].Rows)
                {
                    if (Convert.ToBoolean(_dr["Att_File"]) == false)
                    {
                        continue;
                    }

                    if (_dr["Fld_nm"].ToString() == _HeadFld || _HeadFld == string.Empty)
                    {
                        _calcTax = true;
                    }

                    mfld_nm = _dr["fld_nm"].ToString().Trim();      // Changed by Sachin N. S. on 25/05/2013 for Bug-14538
                    if (_calcTax)
                    {
                        mper_nm = _dr["pert_name"].ToString().Trim();
                        mpert = Convert.ToDecimal(_dr["Def_Pert"]);
                        if (mper_nm != "" && _commonDs.Tables["Main"].Rows[0][mper_nm] != null && Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0][mper_nm]) != 0)
                        {
                            mpert = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0][mper_nm]);
                        }

                        decimal mamt = 0;       // Added by Sachin N. S. on 14/01/2016 for Bug-27503
                        mamt = Convert.ToDecimal(_dr["Def_Amt"]);   // Added by Sachin N. S. on 14/01/2016 for Bug-27503
                        if (mpert > 0)
                        {
                            //if (_dr["AmtExpr"].ToString() != string.Empty)
                            //{
                            //    string _amtExpr = _dr["AmtExpr"].ToString().Replace("Main_vw.", "");
                            //    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]);
                            //}
                            //else
                            //{
                            switch (_dr["Code"].ToString())
                            {
                                case "D":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]);
                                    break;
                                case "T":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]);
                                    break;
                                case "E":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]);
                                    break;
                                case "A":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Tot_Examt"]);
                                    break;
                                case "S":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Tot_Examt"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_add"]);
                                    break;
                                case "N":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Tot_Examt"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_add"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["taxamt"]);
                                    break;
                                case "F":
                                    mcalamt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["gro_amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_deduc"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Tot_Examt"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_add"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["taxamt"]) + Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_nontax"]);
                                    break;
                            }
                            //}

                            string mdef_qty = "";
                            decimal mdef_qtyv = 0;
                            switch (_dr["disp_sign"].ToString())
                            {
                                case "@":
                                    mdef_qty = "Item_vw.QTY";
                                    mdef_qtyv = 0;
                                    mdef_qtyv = Convert.ToDecimal(_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Qty"] != null).Sum(row => Convert.ToDecimal(row["Qty"])));
                                    mamt = mdef_qtyv * mpert;
                                    break;
                                case "%":
                                    mamt = mcalamt * (mpert / 100);
                                    break;
                            }

                            mamt = Math.Round(mamt, 2);
                            _dr["Def_amt"] = Convert.ToDecimal(mamt);
                            //_commonDs.Tables["Main"].Rows[0][_dr["Fld_Nm"].ToString().Trim()] = Convert.ToDecimal(mamt);      // Commented by Sachin N. S. on 14/01/2016 for Bug-27503
                        }
                        _commonDs.Tables["Main"].Rows[0][mfld_nm] = Convert.ToDecimal(mamt);        // Added by Sachin N. S. on 14/01/2016 for Bug-27503
                    }

                    if (_dr["Excl_Net"].ToString() != "D" || _dr["Excl_Net"].ToString() != "G" || _dr["Excl_Net"].ToString() != "A")
                    {
                        string ma_s = "";
                        switch (_dr["A_S"].ToString())
                        {
                            case "+":
                                if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                                {
                                    ma_s = "-";
                                }
                                else
                                {
                                    ma_s = _dr["a_s"].ToString();
                                }
                                break;

                            case "-":
                                if (_dr["CODE"].ToString() == "D" || _dr["CODE"].ToString() == "F")
                                {
                                    ma_s = "+";
                                }
                                else
                                {
                                    ma_s = _dr["a_s"].ToString();
                                }
                                break;

                            default:
                                ma_s = "+";
                                break;
                        }

                        decimal _fldVal = (ma_s == "+" ? +Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0][mfld_nm]) : -Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0][mfld_nm]));
                        switch (_dr["Code"].ToString())
                        {
                            case "D":
                                _commonDs.Tables["Main"].Rows[0]["Tot_Deduc"] = Math.Round(Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Tot_Deduc"]) + _fldVal, 2);
                                break;
                            case "T":
                                _commonDs.Tables["Main"].Rows[0]["tot_tax"] = Math.Round(Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_tax"]) + _fldVal, 2);
                                break;
                            case "E":
                                _commonDs.Tables["Main"].Rows[0]["tot_examt"] = Math.Round(Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_examt"]) + _fldVal, 2);
                                break;
                            case "A":
                                _commonDs.Tables["Main"].Rows[0]["tot_add"] = Math.Round(Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_add"]) + _fldVal, 2);
                                break;
                            case "N":
                                _commonDs.Tables["Main"].Rows[0]["tot_nontax"] = Math.Round(Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_nontax"]) + _fldVal, 2);
                                break;
                            case "F":
                                _commonDs.Tables["Main"].Rows[0]["tot_fdisc"] = Math.Round(Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["tot_fdisc"]) + _fldVal, 2);
                                break;
                        }
                    }
                }
            }
        }
        #endregion Calculate Headerwise Taxes
        // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ****** //

        #region Add - Edit - Delete Methods
        private void addHdrRecords()
        {
            DataRow _dr;
            this.txtWareHouse.Text = CommonInfo.WareHouse;
            _dr = clsInsertDefaultValue.AddNewRow(_commonDs.Tables["Main"]);
            _commonDs.Tables["Main"].Rows.Add(_dr);
            clsInsertDefaultValue.InsertDefVal_Main(0);
            // ******* Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- Start *******//
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true || Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true)
            {
                replDefaDiscChrgsValues("PARTY", 0);
            }
            // ******* Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- End *******//
            addChldRecords("Item");
        }

        private void addChldRecords(string cTblName)
        {
            bool _add = false;
            if (dgvItemDetails.Rows.Count > 0)
            {
                if (_commonDs.Tables["Item"].AsEnumerable().Where(row => row["Item"].ToString() == "").Count() == 0)
                {
                    _add = true;
                }
            }
            else
            {
                _add = true;
            }

            if (_add == true)
            {
                DataRow _dr;
                _dr = clsInsertDefaultValue.AddNewRow(_commonDs.Tables[cTblName]);
                _commonDs.Tables[cTblName].Rows.Add(_dr);
                this.BindingContext[_commonDs, cTblName].Position = _commonDs.Tables["Item"].Rows.Count;
                clsInsertDefaultValue.InsertDefVal_Item(this.BindingContext[_commonDs, cTblName].Position);
                if (dgvItemDetails.CurrentRow.Index < this.dgvItemDetails.Rows.Count && dgvItemDetails.Rows.Count > 1)
                {
                    ldntValidate = true;
                    //this.BindingContext[_commonDs,cTblName].
                    //dgvItemDetails.CurrentRow.Index += 1; 
                    int i = dgvItemDetails.CurrentRow.Index + 1;
                    dgvItemDetails.CurrentCell = dgvItemDetails.Rows[i].Cells[1];
                    ldntValidate = false;
                }
            }
        }

        private void DeleteChldRecords(string cTblName)
        {
            _commonDs.Tables["Item"].Rows.Cast<DataRow>().Where(r => r["Item"].ToString() == "" && Convert.ToDecimal(r["Qty"]) == 0).ToList().ForEach(r => r.Delete());
        }

        //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- Start *****//
        #region Replace Default Discount & Charges or Sales Tax Value
        private void replDefaDiscChrgsValues(string _type, int _RateLevel)
        {
            DataRow _dr1;
            string cSql = "";
            if (_type == "PARTY")
            {
                foreach (DataRow _dr in _commonDs.Tables["HdDiscChrgs"].Rows)
                {
                    if (_commonDs.Tables["PartyDiscChrgs"] != null)
                    {
                        if (_dr["Code"].ToString().Trim() == "S")
                        {
                            if (_commonDs.Tables["PartyDiscChrgs"].Columns.Contains("Tax_Name"))
                            {
                                cSql = "Set DateFormat DMY Select top 1 Level1 from Stax_mas where Tax_Name = '" + _commonDs.Tables["PartyDiscChrgs"].Rows[0]["Tax_Name"].ToString().Trim() + "' and lDeactive=0 and Deactfrom<='" + this.txtInvoiceDt.Text.ToString() + "'";
                                _dr1 = _oDataAccess.GetDataRow(cSql, null, 25);
                                if (_dr1 != null)
                                {
                                    _dr["Head_Nm"] = _commonDs.Tables["PartyDiscChrgs"].Rows[0]["Tax_Name"].ToString();
                                    _dr["Def_Pert"] = Convert.ToDecimal(_dr1["Level1"]);
                                }
                            }
                        }
                        else
                        {
                            if (_commonDs.Tables["PartyDiscChrgs"].Columns.Contains(_dr["Pert_Name"].ToString().Trim()))
                            {
                                _dr["Def_Pert"] = Convert.ToDecimal(_commonDs.Tables["PartyDiscChrgs"].Rows[0][_dr["Pert_Name"].ToString()]);
                            }
                            if (_commonDs.Tables["PartyDiscChrgs"].Columns.Contains(_dr["Fld_Nm"].ToString().Trim()) && Convert.ToDecimal(_dr["Def_Pert"]) == 0)
                            {
                                _dr["Def_Amt"] = Convert.ToDecimal(_commonDs.Tables["PartyDiscChrgs"].Rows[0][_dr["Fld_Nm"].ToString()]);
                            }
                        }
                    }
                }
            }

            if (_type == "ITEM")
            {
                DataTable _dt;
                _dt = clsInsertDefaultValue.GetItemDefaDiscChrgsValue(Convert.ToInt16(_commonDs.Tables["Main"].Rows[0]["Ac_id"]), Convert.ToInt16(_commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position]["It_Code"]), Convert.ToDecimal(_RateLevel));

                DataRow _dr;
                string _cfldnm = "";
                if (_dt.Rows.Count > 0)
                {
                    foreach (DataColumn _dc2 in _dt.Columns)
                    {
                        _cfldnm = _dc2.ColumnName.Trim();
                        _dr = _commonDs.Tables["DiscChrgsFldLst"].Select("Att_File=False and LothrFldNm='" + _cfldnm + "'").FirstOrDefault();
                        if ((_dr["Code"].ToString() == "S" && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true) || (_dr["Code"].ToString() != "S" && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["I_Disc"]) == true))
                        {
                            if (_dr["Code"].ToString() == "S" && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
                            {
                                if (_commonDs.Tables["Stax_Mas"].AsEnumerable().Where(row => row["Tax_Name"] != null).Count(row => row["Tax_Name"].ToString().Trim() == _dt.Rows[0][_cfldnm].ToString().Trim()) > 0)
                                {
                                    _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position][_cfldnm] = _dt.Rows[0][_cfldnm];
                                }
                                else
                                {
                                    if (_dt.Rows[0][_cfldnm].ToString().Trim() != "")
                                    {
                                        MessageBox.Show("The Sales tax `" + _dt.Rows[0][_cfldnm].ToString().Trim() + "` selected for the Item is not activated in Sales Tax master for the transaction.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            else
                            {
                                _commonDs.Tables["Item"].Rows[this.BindingContext[_commonDs, "Item"].Position][_cfldnm] = _dr["Code"].ToString() == "S" ? _dt.Rows[0][_cfldnm].ToString().Trim() == "" ? "NO-TAX" : _dt.Rows[0][_cfldnm] : _dt.Rows[0][_cfldnm];
                            }
                        }
                    }
                }
            }
        }
        #endregion Replace Default Discount & Charges or Sales Tax Value
        //***** Added by Sachin N. S. on 25/06/2013 for Bug-14538 -- End *****//

        #endregion Add - Edit - Delete Methods

        #region Generate Process Id's
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            _oDataAccess = new clsDataAccess();
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udPointofSale.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + CommonInfo.ApplCode + "','" + DateTime.Now.Date.ToString() + "','" + CommonInfo.ApplName + "'," + CommonInfo.ApplPId + ",'" + CommonInfo.AppCaption + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            _oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            _oDataAccess = new clsDataAccess();
            if (string.IsNullOrEmpty(CommonInfo.ApplName) || CommonInfo.ApplPId == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + CommonInfo.ApplName + "' and pApplId=" + CommonInfo.ApplPId + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            _oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        #endregion Generate Process Id's

        #region Printing Bill
        private void PrintBill(int _tran_cd)
        {
            this.pnlPrintBill.Visible = true;    // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.lblPrintBill.Visible = true;   // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.pgrsBar.Visible = true;       // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.lblPrintBill.Text = "Printing Bill......";      // Added by Sachin N. S. on 03/02/2016 for Bug-27503
            this.Refresh();

            string csql = "";
            DataSet vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            csql = "Select * From vudyog..Co_Mast where CompId=" + CommonInfo.CompId.ToString();
            company = _oDataAccess.GetDataTable(csql, null, 25);
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            csql = "Select Top 1 * From Manufact";
            tblCoAdditional = _oDataAccess.GetDataTable(csql, null, 25);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

            string vRepGroup = "POINT OF SALES";
            string[] cpara;
            cpara = new string[1];

            udReportList.cReportList oPrint = new udReportList.cReportList();
            oPrint.pDsCommon = vDsCommon;
            oPrint.pServerName = CommonInfo.Server;
            oPrint.pComDbnm = CommonInfo.DbName;
            oPrint.pUserId = CommonInfo.Uid;
            oPrint.pPassword = CommonInfo.Pwd;
            oPrint.pAppPath = this.pAppPath;
            oPrint.pPApplText = "";
            oPrint.pPara = cpara;
            oPrint.pRepGroup = vRepGroup;
            oPrint.pSpPara = _tran_cd.ToString(); // vPrintPara + ",'" + this.pAppUerName.Trim() + "'"; /*Ramya 01/11/12*/
            oPrint.pPrintOption = 3;
            oPrint.Main();

            this.pnlPrintBill.Visible = false;   // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.lblPrintBill.Visible = false;   // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.pgrsBar.Visible = false;        // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.Refresh();
        }
        #endregion

        //***** Added by Sachin N. S. on 29/08/2013 for Bug-14538 -- Start *****//
        #region Method to Hide/Unhide the Controls
        private void hideUnhideControls()
        {
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Extra"]) == false)
            {
                this.btnAddInfo.Visible = false;
            }
            this.pnlPrintBill.Visible = false;   // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.lblPrintBill.Visible = false;   // Added by Sachin N. S. on 21/08/2015 for Bug-26654
            this.pgrsBar.Visible = false;        // Added by Sachin N. S. on 21/08/2015 for Bug-26654
        }
        #endregion Method to Hide/Unhide the Controls
        //***** Added by Sachin N. S. on 29/08/2013 for Bug-14538 -- End *****//

        //***** Added by Sachin N. S. on 13/08/2015 for Bug-26654 -- Start *****//
        private string splitText(string[][] cFldLst, ref string xcSrchCol, ref string xcDispCol)
        {
            string cFldStr = "";
            for (int i = 0; i < cFldLst.GetLength(0); i++)
            {
                cFldStr += (cFldStr == "" ? "" : ",") + cFldLst[i][0].ToString();
                xcSrchCol += (xcSrchCol == "" ? "" : "+") + cFldLst[i][0].ToString().Split('.')[1].ToString();
                xcDispCol += (xcDispCol == "" ? "" : ",") + cFldLst[i][0].ToString().Split('.')[1].ToString() + ":" + cFldLst[i][1].ToString();
            }
            return cFldStr;
        }
        //***** Added by Sachin N. S. on 13/08/2015 for Bug-26654 -- End *****//

        //***** Added by Sachin N. S. on 20/01/2016 for Bug-27503 -- Start *****//
        private void CancelEntry()
        {
            popUpFromTable("CANCELBILL");
        }

        private void dgvItemDetails_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != dgvItemDetails.BindingContext[_commonDs, "Item"].Position)
                dgvItemDetails.BindingContext[_commonDs, "Item"].Position = e.RowIndex;
        }
        //***** Added by Sachin N. S. on 20/01/2016 for Bug-27503 -- End *****//
        #endregion Private Methods

    }
}
