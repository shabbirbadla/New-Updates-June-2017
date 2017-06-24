using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;

namespace PointofSale
{
    public partial class udFrmPaymentScreen : Form
    {
        clsDataAccess _oDataAccess;
        DataSet _commonDs;
        DataTable _dtTaxChrgs;
        public string _RetValue = "", _PaymentType = "";

        #region Default Constructor and Screen Load
        public udFrmPaymentScreen()
        {
            InitializeComponent();
        }

        public udFrmPaymentScreen(DataSet _Ds)
        {
            _commonDs = _Ds;
            InitializeComponent();
            this.txtCashAmt.pDecimalLength = 2;
            this.txtAmtCard.pDecimalLength = 2;
            this.txtAmtCheque.pDecimalLength = 2;
        }

        private void udFrmPaymentScreen_Load(object sender, EventArgs e)
        {
            this.Icon = new Icon(CommonInfo.IconPath.ToString().Replace("<*#*>", " "));
            this.BindingControls();
            //this.addNewPayDetailsRcrd("CASH");
            //this.Add_PaymentModeDetails("CASH");
        }
        #endregion  Default Constructor and Screen Load

        #region Control Events

        private void udFrmPaymentScreen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    this.btnCoupon.PerformClick();
                    break;
                case Keys.F3:
                    this.btnCard.PerformClick();
                    break;
                case Keys.F4:
                    this.btnCheque.PerformClick();
                    break;
                case Keys.F5:
                    this.txtCashAmt.Focus();
                    break;
                case Keys.F6:
                    this.addPaymentBtnClick();
                    break;
                case Keys.F7:
                    this.btnDelPaymnt.PerformClick();
                    break;
                case Keys.F10:
                    this.btnSave.PerformClick();
                    break;
                case Keys.F12:
                    this.btnCancel.PerformClick();
                    break;
            }
        }

        private void dgvPayModeAmt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPayModeAmt.Columns["colDelPayment"].Index && e.RowIndex >= 0)
            {
                if (_commonDs.Tables["PSPayDetail"].Rows[e.RowIndex]["PayMode"].ToString() == "CASH")
                {
                    this.txtCashAmt.Text = "";
                }
                _commonDs.Tables["PSPayDetail"].Rows[e.RowIndex].Delete();
                this.RefreshControls();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // ***** Commented by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ***** //
            //if (_commonDs.Tables["Main"].Rows[0]["RoundOff"] != null && Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Net_Op"]))
            //{
            //    decimal _netamt = _commonDs.Tables["Item"].AsEnumerable().Where(row => row["Gro_amt"] != null).Sum(row => Convert.ToDecimal(row["Gro_amt"]));
            //    _commonDs.Tables["Main"].Rows[0]["RoundOff"] = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_Amt"]) - _netamt;
            //}
            // ***** Commented by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ***** //

            // ***** Added by Sachin N. S. on 07/08/2013 for Bug-14538 -- Start ***** //
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["ApGenPS"]) == false)
            {
                _commonDs.Tables["Main"].Rows[0]["ApGen"] = "YES";
                _commonDs.Tables["Main"].Rows[0]["ApGenBy"] = CommonInfo.UserName;
                _commonDs.Tables["Main"].Rows[0]["ApGenTime"] = _commonDs.Tables["Main"].Rows[0]["Sysdate"];
            }
            else
            {
                _commonDs.Tables["Main"].Rows[0]["ApGen"] = "PENDING";
                _commonDs.Tables["Main"].Rows[0]["ApGenBy"] = "";
                _commonDs.Tables["Main"].Rows[0]["ApGenTime"] = "";
            }
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["ApLedPs"]) == false)
            {
                _commonDs.Tables["Main"].Rows[0]["Apled"] = "YES";
                _commonDs.Tables["Main"].Rows[0]["ApLedBy"] = CommonInfo.UserName;
                _commonDs.Tables["Main"].Rows[0]["ApLedTime"] = _commonDs.Tables["Main"].Rows[0]["Sysdate"];
            }
            else
            {
                _commonDs.Tables["Main"].Rows[0]["Apled"] = "PENDING";
                _commonDs.Tables["Main"].Rows[0]["ApLedBy"] = "";
                _commonDs.Tables["Main"].Rows[0]["ApLedTime"] = "";
            }
            // ***** Added by Sachin N. S. on 07/08/2013 for Bug-14538 -- End ***** //

            Decimal _totalPaid = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
            _commonDs.Tables["Main"].Rows[0]["TotalPaid"] = _totalPaid;
            _commonDs.Tables["Main"].Rows[0]["BalAmt"] = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]) - _totalPaid;
            _commonDs.Tables["Main"].AcceptChanges();
            _commonDs.Tables["Item"].AcceptChanges();
            _commonDs.Tables["PSPayDetail"].AcceptChanges();

            if (this.ValidateRecords())
            {
                clsInsUpdDelPrint._commonDs = _commonDs;
                if (clsInsUpdDelPrint.InsertRecords())
                {
                    this.Close();
                    MessageBox.Show(clsInsUpdDelPrint._retMsg, CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RetValue = "SAVED";
                }
                else
                {
                    MessageBox.Show(clsInsUpdDelPrint._retMsg, CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _commonDs.Tables["Main"].RejectChanges();
            _commonDs.Tables["Item"].RejectChanges();
            _commonDs.Tables["PSPayDetail"].RejectChanges();
            this.Close();
            _RetValue = "CANCELLED";
        }

        private void btnCoupon_Click(object sender, EventArgs e)
        {
            this.dgvPayModeAmt.Visible = false;
            this.pnlCouponDetails.Visible = true;
            this.pnlCardDetails.Visible = false;
            this.pnlChequeDetails.Visible = false;
            this.pnlCouponDetails.Top = this.dgvPayModeAmt.Top;
            this.pnlCouponDetails.Left = this.dgvPayModeAmt.Left;
            this.btnDelPaymnt.Visible = true;
            this.btnDelPaymnt.Text = "Show Payment Mode";
            _PaymentType = "COUPON";
            this.txtNmCoupon.Focus();
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            this.dgvPayModeAmt.Visible = false;
            this.pnlCouponDetails.Visible = false;
            this.pnlCardDetails.Visible = true;
            this.pnlChequeDetails.Visible = false;
            this.pnlCardDetails.Top = this.dgvPayModeAmt.Top;
            this.pnlCardDetails.Left = this.dgvPayModeAmt.Left;
            this.btnDelPaymnt.Visible = true;
            this.btnDelPaymnt.Text = "Show Payment Mode";
            _PaymentType = "CARD";
            this.txtNmCard.Focus();
        }

        private void btnCheque_Click(object sender, EventArgs e)
        {
            this.dgvPayModeAmt.Visible = false;
            this.pnlCouponDetails.Visible = false;
            this.pnlCardDetails.Visible = false;
            this.pnlChequeDetails.Visible = true;
            this.pnlChequeDetails.Top = this.dgvPayModeAmt.Top;
            this.pnlChequeDetails.Left = this.dgvPayModeAmt.Left;
            this.btnDelPaymnt.Visible = true;
            this.btnDelPaymnt.Text = "Show Payment Mode";
            this.txtDtCheque.Value = DateTime.Now;
            _PaymentType = "CHEQUE";
            this.txtAmtCheque.Focus();
        }

        private void btnAddCoupon_Click(object sender, EventArgs e)
        {
            this.Add_PaymentModeDetails("COUPON");
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            this.Add_PaymentModeDetails("CARD");
        }

        private void btnAddCheque_Click(object sender, EventArgs e)
        {
            this.Add_PaymentModeDetails("CHEQUE");
        }

        private void btnBankCheque_Click(object sender, EventArgs e)
        {
            this.popUpFromTable("BANKNAME");
        }

        private void btnTypCard_Click(object sender, EventArgs e)
        {
            this.popUpFromTable("CARDTYPE");
        }

        private void btnDelPaymnt_Click(object sender, EventArgs e)
        {
            if (btnDelPaymnt.Text == "Delete Payment")
            {

            }
            else
            {
                _PaymentType = "";
                btnDelPaymnt.Text = "Delete Payment";
                this.dgvPayModeAmt.Visible = true;
                this.pnlCouponDetails.Visible = false;
                this.pnlCardDetails.Visible = false;
                this.pnlChequeDetails.Visible = false;
                this.btnDelPaymnt.Visible = false;
            }
        }

        private void txtCashAmt_TextChanged(object sender, EventArgs e)
        {
            this.Add_PaymentModeDetails("CASH");
            this.RefreshControls();
        }

        private void txtValCoupon_TextChanged(object sender, EventArgs e)
        {
            this.txtTotCoupon.Text = (Convert.ToDecimal(this.txtValCoupon.Text) * Convert.ToDecimal(this.txtQtyCoupon.Text)).ToString();
        }

        private void txtQtyCoupon_TextChanged(object sender, EventArgs e)
        {
            this.txtTotCoupon.Text = (Convert.ToDecimal(this.txtValCoupon.Text) * Convert.ToDecimal(this.txtQtyCoupon.Text)).ToString();
        }

        private void chkDebitCard_Leave(object sender, EventArgs e)
        {
            this.lblDebitCard.Font = new Font(this.lblDebitCard.Font, FontStyle.Regular);
            this.lblDebitCard.Refresh();
        }

        private void chkDebitCard_GotFocus(object sender, EventArgs e)
        {
            this.lblDebitCard.Font = new Font(this.lblDebitCard.Font, FontStyle.Bold);
            this.lblDebitCard.Refresh();
        }

        #endregion Control Events

        #region Private Methods

        #region Binding Controls and Grids
        private void BindingControls()
        {
            this.txtTotBillAmt.Text = "Bill Amt. :" + _commonDs.Tables["Main"].Rows[0]["Net_amt"].ToString();
            this.txtCashAmt.Text = _commonDs.Tables["Main"].Rows[0]["Net_amt"].ToString();

            dgvPayModeAmt.AutoGenerateColumns = false;
            dgvPayModeAmt.ClearSelection();
            dgvPayModeAmt.DataSource = _commonDs.Tables["PSPayDetail"];
            dgvPayModeAmt.Columns[0].DataPropertyName = "PayMode";
            dgvPayModeAmt.Columns[1].DataPropertyName = "TotalValue";

            this.TaxAndCharges();

            dgvTaxAndChrgs.AutoGenerateColumns = false;
            dgvTaxAndChrgs.ClearSelection();
            dgvTaxAndChrgs.DataSource = _dtTaxChrgs;
            dgvTaxAndChrgs.Columns[0].DataPropertyName = "HeadingNm";
            dgvTaxAndChrgs.Columns[1].DataPropertyName = "Amount";

            foreach (DataGridViewRow dgvrow in dgvTaxAndChrgs.Rows)
            {
                if (dgvrow.Cells[0].Value.ToString() == "Gross Amount" || dgvrow.Cells[0].Value.ToString() == "Net Amount" || dgvrow.Cells[0].Value.ToString() == "Balance Amount")
                {
                    dgvrow.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                }
            }
            this.RefreshControls();
        }

        private void TaxAndCharges()
        {
            _dtTaxChrgs = new DataTable();
            _dtTaxChrgs.TableName = "TaxChrgs";
            _dtTaxChrgs.Columns.Add(new DataColumn("HeadingNm", typeof(string)));
            _dtTaxChrgs.Columns.Add(new DataColumn("Amount", typeof(decimal)));

            DataRow _dr;

            // ******* Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ******* //
            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Gross Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);

            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "D").Count() > 0)
                {
                    _dr = _dtTaxChrgs.NewRow();
                    _dr["HeadingNm"] = "Less : Dedn. Before Tax";
                    _dr["Amount"] = 0.00;
                    _dtTaxChrgs.Rows.Add(_dr);
                }

                if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "T").Count() > 0)
                {
                    _dr = _dtTaxChrgs.NewRow();
                    _dr["HeadingNm"] = "Taxable Charges";
                    _dr["Amount"] = 0.00;
                    _dtTaxChrgs.Rows.Add(_dr);
                }

                if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "E").Count() > 0)
                {
                    _dr = _dtTaxChrgs.NewRow();
                    _dr["HeadingNm"] = "Excise Duty";
                    _dr["Amount"] = 0.00;
                    _dtTaxChrgs.Rows.Add(_dr);
                }

                if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "A").Count() > 0)
                {
                    _dr = _dtTaxChrgs.NewRow();
                    _dr["HeadingNm"] = "Add/Less Charges";
                    _dr["Amount"] = 0.00;
                    _dtTaxChrgs.Rows.Add(_dr);
                }
            }
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["stax_tran"]) == true)
            {
                _dr = _dtTaxChrgs.NewRow();
                _dr["HeadingNm"] = "Sales Tax";
                _dr["Amount"] = 0.00;
                _dtTaxChrgs.Rows.Add(_dr);
            }

            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["V_Disc"]) == true)
            {
                if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "N").Count() > 0)
                {
                    _dr = _dtTaxChrgs.NewRow();
                    _dr["HeadingNm"] = "Non Taxable Charges";
                    _dr["Amount"] = 0.00;
                    _dtTaxChrgs.Rows.Add(_dr);
                }

                if (_commonDs.Tables["DCMast"].AsEnumerable().Where(row => Convert.ToString(row["Code"]) == "F").Count() > 0)
                {
                    _dr = _dtTaxChrgs.NewRow();
                    _dr["HeadingNm"] = "Less : Final Discount";
                    _dr["Amount"] = 0.00;
                    _dtTaxChrgs.Rows.Add(_dr);
                }
            }
            if (Convert.ToBoolean(_commonDs.Tables["Lcode"].Rows[0]["Net_op"]) == true)
            {
                _dr = _dtTaxChrgs.NewRow();
                _dr["HeadingNm"] = "Round Off";
                _dr["Amount"] = 0.00;
                _dtTaxChrgs.Rows.Add(_dr);
            }

            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Net Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);
            // ******* Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ******* //

            // ******* Commented by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ******* //
            //_dr = _dtTaxChrgs.NewRow();
            //_dr["HeadingNm"] = "Gross Amount";
            //_dr["Amount"] = 0.00;
            //_dtTaxChrgs.Rows.Add(_dr);
            ////dgvTaxAndChrgs.Rows[dgvTaxAndChrgs.Rows.Count - 1].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            //_dr = _dtTaxChrgs.NewRow();
            //_dr["HeadingNm"] = "Tax Amount";
            //_dr["Amount"] = 0.00;
            //_dtTaxChrgs.Rows.Add(_dr);

            //_dr = _dtTaxChrgs.NewRow();
            //_dr["HeadingNm"] = "Round off Amount";
            //_dr["Amount"] = 0.00;
            //_dtTaxChrgs.Rows.Add(_dr);

            //_dr = _dtTaxChrgs.NewRow();
            //_dr["HeadingNm"] = "Net Amount";
            //_dr["Amount"] = 0.00;
            //_dtTaxChrgs.Rows.Add(_dr);
            ////dgvTaxAndChrgs.Rows[dgvTaxAndChrgs.Rows.Count - 1].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            // ******* Commented by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ******* //

            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Cash Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);

            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Coupon Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);

            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Card Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);

            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Cheque Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);

            _dr = _dtTaxChrgs.NewRow();
            _dr["HeadingNm"] = "Balance Amount";
            _dr["Amount"] = 0.00;
            _dtTaxChrgs.Rows.Add(_dr);
            //dgvTaxAndChrgs.Rows[dgvTaxAndChrgs.Rows.Count - 1].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
        }

        #endregion Binding Controls and Grids

        #region RefreshControls
        private void RefreshControls()
        {
            Decimal _totalPaid = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
            if (Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]) - _totalPaid < 0)
            {
                this.txtTotalPaid.BackColor = Color.GreenYellow;
            }
            if (Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]) - _totalPaid > 0)
            {
                this.txtTotalPaid.BackColor = Color.Red;
            }
            if (Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]) - _totalPaid == 0)
            {
                this.txtTotalPaid.BackColor = Color.White;
            }

            this.txtTotalPaid.Text = "Total Paid : " + _totalPaid.ToString();
            this.txtTenderChng.Text = "Change : " + (Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]) - _totalPaid).ToString();

            if (_dtTaxChrgs != null)
            {
                var _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                                   where currentRw.Field<string>("HeadingNm") == "Gross Amount"
                                   select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Gro_amt"];
                }

                // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ****** //
                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Less : Dedn. Before Tax"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["tot_deduc"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Taxable Charges"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["tot_tax"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Excise Duty"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_examt"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Add/Less Charges"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_add"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Sales Tax"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Taxamt"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Non Taxable Charges"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_nontax"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Less : Final Discount"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Tot_fdisc"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Round Off"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["roundoff"];
                }

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Net Amount"
                               select currentRw).FirstOrDefault();
                if (_currentRow != null)
                {
                    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Net_Amt"];
                }
                // ****** Added by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ****** //

                // ****** Commented by Sachin N. S. on 28/05/2013 for Bug-14538 -- Start ****** //
                //_currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                //                   where currentRw.Field<string>("HeadingNm") == "Tax Amount"
                //                   select currentRw).FirstOrDefault();
                //if (_currentRow != null)
                //{
                //    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["TaxAmt"];
                //}

                //_currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                //               where currentRw.Field<string>("HeadingNm") == "Round off Amount"
                //               select currentRw).FirstOrDefault();
                //if (_currentRow != null)
                //{
                //    _currentRow["Amount"] = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_Amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Gro_Amt"]) - Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["TaxAmt"]);
                //}

                //_currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                //                   where currentRw.Field<string>("HeadingNm") == "Net Amount"
                //                   select currentRw).FirstOrDefault();
                //if (_currentRow != null)
                //{
                //    _currentRow["Amount"] = _commonDs.Tables["Main"].Rows[0]["Net_amt"];
                //}
                // ****** Commented by Sachin N. S. on 28/05/2013 for Bug-14538 -- End ****** //

                Decimal _PayModeAmt, _BalanceAmt = 0.00M;
                _BalanceAmt = Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]);
                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Cash Amount"
                               select currentRw).FirstOrDefault();
                _PayModeAmt = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["PayMode"].ToString() == "CASH" && row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
                _currentRow["Amount"] = _PayModeAmt;
                _BalanceAmt -= _PayModeAmt;

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Coupon Amount"
                               select currentRw).FirstOrDefault();
                _PayModeAmt = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["PayMode"].ToString() == "COUPON" && row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
                _currentRow["Amount"] = _PayModeAmt;
                _BalanceAmt -= _PayModeAmt;

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Card Amount"
                               select currentRw).FirstOrDefault();
                _PayModeAmt = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["PayMode"].ToString() == "CARD" && row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
                _currentRow["Amount"] = _PayModeAmt;
                _BalanceAmt -= _PayModeAmt;

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Cheque Amount"
                               select currentRw).FirstOrDefault();
                _PayModeAmt = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["PayMode"].ToString() == "CHEQUE" && row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
                _currentRow["Amount"] = _PayModeAmt;
                _BalanceAmt -= _PayModeAmt;

                _currentRow = (from currentRw in _dtTaxChrgs.AsEnumerable()
                               where currentRw.Field<string>("HeadingNm") == "Balance Amount"
                               select currentRw).FirstOrDefault();
                //_PayModeAmt = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["PayMode"].ToString() == "CHEQUE" && row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
                _currentRow["Amount"] = _BalanceAmt;

            }
            //_dr["HeadingNm"] = "Balance Amount";

            this.Refresh();
        }
        #endregion

        #region Adding New Records of Payment modes in datatable
        private void addNewPayDetailsRcrd(string _paymode)
        {
            DataRow _dr;
            _dr = clsInsertDefaultValue.AddNewRow(_commonDs.Tables["PSPayDetail"]);
            _commonDs.Tables["PSPayDetail"].Rows.Add(_dr);
            this.BindingContext[_commonDs, "PSPayDetail"].Position = _commonDs.Tables["PSPayDetail"].AsEnumerable().Count() - 1;
            clsInsertDefaultValue.InsertDefVal_PayDetail(this.BindingContext[_commonDs, "PSPayDetail"].Position, _paymode);
        }

        private bool Add_PaymentModeDetails(string _paymode)
        {
            int _iPosition;
            if (_paymode.ToString() == "CASH")
            {
                //DataRow _dr = (DataRow)_commonDs.Tables["PSPayDetail"].AsEnumerable().Select(row => row["PayMode"]="CASH");
                //int _icount = _commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["PayMode"].ToString() == "CASH").Count();

                _iPosition = -1;
                var _dr = (from myRow in _commonDs.Tables["PSPayDetail"].AsEnumerable()
                           where myRow.Field<string>("PayMode") == "CASH"
                           select myRow).FirstOrDefault();
                if (_dr == null)
                {
                    this.addNewPayDetailsRcrd("CASH");
                }
                _dr = (from myRow in _commonDs.Tables["PSPayDetail"].AsEnumerable()
                       where myRow.Field<string>("PayMode") == "CASH"
                       select myRow).FirstOrDefault();
                _dr["TotalValue"] = Math.Round(Convert.ToDecimal(this.txtCashAmt.Text), 2);

                //foreach (DataRow _dr1 in _dr)
                //{
                //    _iPosition = _commonDs.Tables["PSPayDetail"].Rows.IndexOf(_dr1);
                //}
                //if (_iPosition == -1)
                //{
                //    this.addNewPayDetailsRcrd("CASH");
                //}
                //_commonDs.Tables["PSPayDetail"].Rows[this.BindingContext[_commonDs, "PSPayDetail"].Position]["TotalValue"] = Math.Round(Convert.ToDecimal(this.txtCashAmt.Text),2);
            }

            if (_paymode.ToString() == "COUPON")
            {
                // ******* Validation for Coupon -- Start ******* \\
                if (this.txtNmCoupon.Text == "")
                {
                    MessageBox.Show("Coupon Name cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtNmCoupon.Focus();
                    return false;
                }

                if (this.txtNoCoupon.Text == "")
                {
                    MessageBox.Show("Coupon number cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtNoCoupon.Focus();
                    return false;
                }

                if (Convert.ToDecimal(this.txtValCoupon.Text) == 0)
                {
                    MessageBox.Show("Coupon value cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtValCoupon.Focus();
                    return false;
                }

                if (Convert.ToDecimal(this.txtQtyCoupon.Text) == 0)
                {
                    MessageBox.Show("Coupon quantity cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtQtyCoupon.Focus();
                    return false;
                }
                // ******* Validation for Coupon -- End ******* \\

                this.addNewPayDetailsRcrd("COUPON");
                _iPosition = this.BindingContext[_commonDs, "PSPayDetail"].Position;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CouponNm"] = this.txtNmCoupon.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CouponNo"] = this.txtNoCoupon.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CouponVal"] = Convert.ToDecimal(this.txtValCoupon.Text);
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CouponQty"] = Convert.ToDecimal(this.txtQtyCoupon.Text);
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["TotalValue"] = (Convert.ToDecimal(this.txtQtyCoupon.Text) * Convert.ToDecimal(this.txtValCoupon.Text));

                this.txtNmCoupon.Text = "";
                this.txtNoCoupon.Text = "";
                this.txtValCoupon.Text = "";
                this.txtQtyCoupon.Text = "";
                this.txtTotCoupon.Text = "";
            }

            if (_paymode.ToString() == "CARD")
            {
                // ******* Validation for Card -- Start ******* \\
                if (this.txtNmCard.Text == "")
                {
                    MessageBox.Show("Card name cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtNmCard.Focus();
                    return false;
                }

                if (this.txtNoCard.Text == "")
                {
                    MessageBox.Show("Card number cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtNoCard.Focus();
                    return false;
                }

                if (this.txtTypCard.Text == "")
                {
                    MessageBox.Show("Card type cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtTypCard.Focus();
                    return false;
                }

                if (Convert.ToDecimal(this.txtAmtCard.Text) == 0)
                {
                    MessageBox.Show("Card amount cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtAmtCard.Focus();
                    return false;
                }
                // ******* Validation for Card -- End ******* \\

                this.addNewPayDetailsRcrd("CARD");
                _iPosition = this.BindingContext[_commonDs, "PSPayDetail"].Position;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CardNm"] = this.txtNmCard.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CardNo"] = this.txtNoCard.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CardType"] = this.txtTypCard.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["IsDebitCard"] = Convert.ToBoolean(this.chkDebitCard.Checked);
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CardAmt"] = Convert.ToDecimal(this.txtAmtCard.Text);
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["TotalValue"] = Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].Rows[_iPosition]["CardAmt"]);

                this.txtNmCard.Text = "";
                this.txtNoCard.Text = "";
                this.txtTypCard.Text = "";
                this.chkDebitCard.Checked = false;
                this.txtAmtCard.Text = "";
            }

            if (_paymode.ToString() == "CHEQUE")
            {
                // ******* Validation for Card -- Start ******* \\
                if (Convert.ToDecimal(this.txtAmtCheque.Text) == 0)
                {
                    MessageBox.Show("Cheque amount cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtAmtCheque.Focus();
                    return false;
                }

                if (this.txtNoCheque.Text == "")
                {
                    MessageBox.Show("Cheque number cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtNoCheque.Focus();
                    return false;
                }

                if (this.txtBankCheque.Text == "")
                {
                    MessageBox.Show("Bank Name cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtBankCheque.Focus();
                    return false;
                }

                if (this.txtDtCheque.Text == "")
                {
                    MessageBox.Show("Cheque date cannot be empty.", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtDtCheque.Focus();
                    return false;
                }
                // ******* Validation for Card -- End ******* \\

                this.addNewPayDetailsRcrd("CHEQUE");
                _iPosition = this.BindingContext[_commonDs, "PSPayDetail"].Position;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["ChequeAmt"] = Convert.ToDecimal(this.txtAmtCheque.Text);
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["ChequeNo"] = this.txtNoCheque.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["BankNm"] = this.txtBankCheque.Text;
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["Chequedt"] = Convert.ToDateTime(this.txtDtCheque.Text);
                _commonDs.Tables["PSPayDetail"].Rows[_iPosition]["TotalValue"] = Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].Rows[_iPosition]["ChequeAmt"]);

                this.txtAmtCheque.Text = "";
                this.txtNoCheque.Text = "";
                this.txtBankCheque.Text = "";
                this.txtDtCheque.Value = DateTime.Now;
            }

            this.RefreshControls();
            return true;
        }

        private void addPaymentBtnClick()
        {
            switch (_PaymentType)
            {
                case "COUPON":
                    this.btnAddCoupon.PerformClick();
                    break;
                case "CARD":
                    this.btnAddCard.PerformClick();
                    break;
                case "CHEQUE":
                    this.btnAddCheque.PerformClick();
                    break;
            }
        }
        #endregion Adding New Records of Payment modes in datatable

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
                case "CARDTYPE":
                    cSql = "set dateformat dmy select CardType from MSTCARDTYPE order by CardType ";
                    cFrmCap = "Select Card Type";
                    cSrchCol = "CardType";
                    cDispCol = "CardType:Card Type";
                    cRetCol = "CardType";
                    break;

                case "BANKNAME":
                    cSql = "set dateformat dmy select distinct BankNm from PSPAYDETAIL order by BankNm ";
                    cFrmCap = "Select Bank Name";
                    cSrchCol = "BankNm";
                    cDispCol = "BankNm:Bank Name";
                    cRetCol = "Banknm";
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
                    case "CARDTYPE":
                        this.txtTypCard.Text = _oSelPop.pReturnArray[0].ToString();
                        break;
                    case "BANKNAME":
                        this.txtBankCheque.Text = _oSelPop.pReturnArray[0].ToString();
                        break;
                }
            }
        }

        #endregion Calling Popup Selection Screen

        #region Validating Records
        private bool ValidateRecords()
        {
            Decimal _totalPaid = Math.Round(Convert.ToDecimal(_commonDs.Tables["PSPayDetail"].AsEnumerable().Where(row => row["TotalValue"] != null).Sum(row => Convert.ToDecimal(row["TotalValue"]))), 2);
            if (Convert.ToDecimal(_commonDs.Tables["Main"].Rows[0]["Net_amt"]) - _totalPaid > 0)
            {
                MessageBox.Show("Amount paid is less than the amount to be paid. Cannot continue...!!!", CommonInfo.AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        #endregion Validating Records

        #endregion Private Methods

    }
}
