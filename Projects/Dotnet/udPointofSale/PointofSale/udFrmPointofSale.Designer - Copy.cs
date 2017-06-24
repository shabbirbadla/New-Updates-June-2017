namespace PointofSale
{
    partial class udFrmPointofSale
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.pnlCenterBox = new System.Windows.Forms.Panel();
            this.txtInvoiceDt = new System.Windows.Forms.DateTimePicker();
            this.dgvItemDetails = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colRate = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colDiscount = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colAmount = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.dgvPayDetails = new System.Windows.Forms.DataGridView();
            this.colPaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentAmt = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.lblPayDetails = new System.Windows.Forms.Label();
            this.pnlHoriz4 = new System.Windows.Forms.Panel();
            this.txtNetAmt = new System.Windows.Forms.TextBox();
            this.lblNetAmt = new System.Windows.Forms.Label();
            this.txtDiscAmt = new System.Windows.Forms.TextBox();
            this.lblDiscAmt = new System.Windows.Forms.Label();
            this.txtGrossAmt = new System.Windows.Forms.TextBox();
            this.lblGrossAmt = new System.Windows.Forms.Label();
            this.txtTotalQty = new System.Windows.Forms.TextBox();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.txtNoOfItems = new System.Windows.Forms.TextBox();
            this.lblNoOfItems = new System.Windows.Forms.Label();
            this.pnlHoriz3 = new System.Windows.Forms.Panel();
            this.lblInvoiceDt = new System.Windows.Forms.Label();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.pnlHoriz2 = new System.Windows.Forms.Panel();
            this.pnlHoriz1 = new System.Windows.Forms.Panel();
            this.pnlRightBox = new System.Windows.Forms.Panel();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrintInv = new System.Windows.Forms.Button();
            this.btnSearchInv = new System.Windows.Forms.Button();
            this.lblActionBtn = new System.Windows.Forms.Label();
            this.pnlLeftBox = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAddItemShrtKey = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.txtInvoiceSr = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblLoginDetails = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlOuter.SuspendLayout();
            this.pnlCenterBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayDetails)).BeginInit();
            this.pnlRightBox.SuspendLayout();
            this.pnlLeftBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOuter
            // 
            this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOuter.Controls.Add(this.pnlCenterBox);
            this.pnlOuter.Controls.Add(this.pnlRightBox);
            this.pnlOuter.Controls.Add(this.pnlLeftBox);
            this.pnlOuter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlOuter.Location = new System.Drawing.Point(4, 3);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(768, 524);
            this.pnlOuter.TabIndex = 0;
            // 
            // pnlCenterBox
            // 
            this.pnlCenterBox.BackColor = System.Drawing.Color.Silver;
            this.pnlCenterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCenterBox.Controls.Add(this.txtInvoiceDt);
            this.pnlCenterBox.Controls.Add(this.dgvItemDetails);
            this.pnlCenterBox.Controls.Add(this.dgvPayDetails);
            this.pnlCenterBox.Controls.Add(this.lblPayDetails);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz4);
            this.pnlCenterBox.Controls.Add(this.txtNetAmt);
            this.pnlCenterBox.Controls.Add(this.lblNetAmt);
            this.pnlCenterBox.Controls.Add(this.txtDiscAmt);
            this.pnlCenterBox.Controls.Add(this.lblDiscAmt);
            this.pnlCenterBox.Controls.Add(this.txtGrossAmt);
            this.pnlCenterBox.Controls.Add(this.lblGrossAmt);
            this.pnlCenterBox.Controls.Add(this.txtTotalQty);
            this.pnlCenterBox.Controls.Add(this.lblTotalQty);
            this.pnlCenterBox.Controls.Add(this.txtNoOfItems);
            this.pnlCenterBox.Controls.Add(this.lblNoOfItems);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz3);
            this.pnlCenterBox.Controls.Add(this.lblInvoiceDt);
            this.pnlCenterBox.Controls.Add(this.txtInvoiceNo);
            this.pnlCenterBox.Controls.Add(this.lblInvoiceNo);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz2);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz1);
            this.pnlCenterBox.Location = new System.Drawing.Point(148, 3);
            this.pnlCenterBox.Name = "pnlCenterBox";
            this.pnlCenterBox.Size = new System.Drawing.Size(470, 516);
            this.pnlCenterBox.TabIndex = 2;
            // 
            // txtInvoiceDt
            // 
            this.txtInvoiceDt.CalendarFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceDt.CalendarForeColor = System.Drawing.SystemColors.WindowText;
            this.txtInvoiceDt.CalendarMonthBackground = System.Drawing.SystemColors.ControlDark;
            this.txtInvoiceDt.Enabled = false;
            this.txtInvoiceDt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceDt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInvoiceDt.Location = new System.Drawing.Point(334, 2);
            this.txtInvoiceDt.Name = "txtInvoiceDt";
            this.txtInvoiceDt.Size = new System.Drawing.Size(100, 26);
            this.txtInvoiceDt.TabIndex = 21;
            this.txtInvoiceDt.TabStop = false;
            // 
            // dgvItemDetails
            // 
            this.dgvItemDetails.AllowUserToAddRows = false;
            this.dgvItemDetails.AllowUserToDeleteRows = false;
            this.dgvItemDetails.AllowUserToResizeColumns = false;
            this.dgvItemDetails.AllowUserToResizeRows = false;
            this.dgvItemDetails.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvItemDetails.ColumnHeadersHeight = 28;
            this.dgvItemDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItemDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colItem,
            this.colQuantity,
            this.colRate,
            this.colDiscount,
            this.colAmount});
            this.dgvItemDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvItemDetails.Location = new System.Drawing.Point(2, 36);
            this.dgvItemDetails.MultiSelect = false;
            this.dgvItemDetails.Name = "dgvItemDetails";
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvItemDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvItemDetails.RowHeadersVisible = false;
            this.dgvItemDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvItemDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvItemDetails.Size = new System.Drawing.Size(464, 271);
            this.dgvItemDetails.TabIndex = 20;
            this.dgvItemDetails.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_CellEndEdit);
            this.dgvItemDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvItemDetails_KeyDown);
            // 
            // colSrNo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = null;
            this.colSrNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSrNo.HeaderText = "Sr. No";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            this.colSrNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSrNo.Width = 80;
            // 
            // colItem
            // 
            this.colItem.HeaderText = "Item";
            this.colItem.Name = "colItem";
            this.colItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colItem.Width = 200;
            // 
            // colQuantity
            // 
            this.colQuantity.AllowNegative = false;
            this.colQuantity.DecimalLength = 3;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "F3";
            this.colQuantity.DefaultCellStyle = dataGridViewCellStyle2;
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colRate
            // 
            this.colRate.DecimalLength = 2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "F2";
            dataGridViewCellStyle3.NullValue = null;
            this.colRate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colRate.HeaderText = "Rate";
            this.colRate.Name = "colRate";
            this.colRate.ReadOnly = true;
            this.colRate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colDiscount
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDiscount.DefaultCellStyle = dataGridViewCellStyle4;
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.ReadOnly = true;
            this.colDiscount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colAmount
            // 
            this.colAmount.DecimalLength = 2;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "F2";
            this.colAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            this.colAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dgvPayDetails
            // 
            this.dgvPayDetails.AllowUserToAddRows = false;
            this.dgvPayDetails.AllowUserToDeleteRows = false;
            this.dgvPayDetails.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvPayDetails.ColumnHeadersHeight = 28;
            this.dgvPayDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPayDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPaymentType,
            this.colPaymentAmt});
            this.dgvPayDetails.Location = new System.Drawing.Point(2, 436);
            this.dgvPayDetails.Name = "dgvPayDetails";
            this.dgvPayDetails.RowHeadersVisible = false;
            this.dgvPayDetails.Size = new System.Drawing.Size(464, 75);
            this.dgvPayDetails.TabIndex = 19;
            // 
            // colPaymentType
            // 
            this.colPaymentType.HeaderText = "Payment Mode";
            this.colPaymentType.Name = "colPaymentType";
            this.colPaymentType.ReadOnly = true;
            this.colPaymentType.Width = 300;
            // 
            // colPaymentAmt
            // 
            this.colPaymentAmt.DecimalLength = 2;
            dataGridViewCellStyle7.Format = "F2";
            this.colPaymentAmt.DefaultCellStyle = dataGridViewCellStyle7;
            this.colPaymentAmt.HeaderText = "Amount";
            this.colPaymentAmt.Name = "colPaymentAmt";
            this.colPaymentAmt.ReadOnly = true;
            this.colPaymentAmt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPaymentAmt.Width = 150;
            // 
            // lblPayDetails
            // 
            this.lblPayDetails.AutoSize = true;
            this.lblPayDetails.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayDetails.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPayDetails.Location = new System.Drawing.Point(3, 414);
            this.lblPayDetails.Name = "lblPayDetails";
            this.lblPayDetails.Size = new System.Drawing.Size(132, 19);
            this.lblPayDetails.TabIndex = 18;
            this.lblPayDetails.Text = "Payment Details";
            // 
            // pnlHoriz4
            // 
            this.pnlHoriz4.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHoriz4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHoriz4.Location = new System.Drawing.Point(-1, 406);
            this.pnlHoriz4.Name = "pnlHoriz4";
            this.pnlHoriz4.Size = new System.Drawing.Size(471, 5);
            this.pnlHoriz4.TabIndex = 17;
            // 
            // txtNetAmt
            // 
            this.txtNetAmt.BackColor = System.Drawing.Color.LightGray;
            this.txtNetAmt.Enabled = false;
            this.txtNetAmt.ForeColor = System.Drawing.Color.Aqua;
            this.txtNetAmt.Location = new System.Drawing.Point(314, 377);
            this.txtNetAmt.Name = "txtNetAmt";
            this.txtNetAmt.Size = new System.Drawing.Size(153, 26);
            this.txtNetAmt.TabIndex = 16;
            this.txtNetAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNetAmt
            // 
            this.lblNetAmt.AutoSize = true;
            this.lblNetAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblNetAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetAmt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNetAmt.Location = new System.Drawing.Point(338, 354);
            this.lblNetAmt.Name = "lblNetAmt";
            this.lblNetAmt.Size = new System.Drawing.Size(104, 20);
            this.lblNetAmt.TabIndex = 15;
            this.lblNetAmt.Text = "Net Amount";
            // 
            // txtDiscAmt
            // 
            this.txtDiscAmt.BackColor = System.Drawing.Color.LightGray;
            this.txtDiscAmt.Enabled = false;
            this.txtDiscAmt.ForeColor = System.Drawing.Color.Aqua;
            this.txtDiscAmt.Location = new System.Drawing.Point(158, 377);
            this.txtDiscAmt.Name = "txtDiscAmt";
            this.txtDiscAmt.Size = new System.Drawing.Size(153, 26);
            this.txtDiscAmt.TabIndex = 14;
            this.txtDiscAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDiscAmt
            // 
            this.lblDiscAmt.AutoSize = true;
            this.lblDiscAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscAmt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblDiscAmt.Location = new System.Drawing.Point(161, 354);
            this.lblDiscAmt.Name = "lblDiscAmt";
            this.lblDiscAmt.Size = new System.Drawing.Size(147, 20);
            this.lblDiscAmt.TabIndex = 13;
            this.lblDiscAmt.Text = "Discount Amount";
            // 
            // txtGrossAmt
            // 
            this.txtGrossAmt.BackColor = System.Drawing.Color.LightGray;
            this.txtGrossAmt.Enabled = false;
            this.txtGrossAmt.ForeColor = System.Drawing.Color.Aqua;
            this.txtGrossAmt.Location = new System.Drawing.Point(2, 377);
            this.txtGrossAmt.Name = "txtGrossAmt";
            this.txtGrossAmt.Size = new System.Drawing.Size(153, 26);
            this.txtGrossAmt.TabIndex = 12;
            this.txtGrossAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblGrossAmt
            // 
            this.lblGrossAmt.AutoSize = true;
            this.lblGrossAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblGrossAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossAmt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGrossAmt.Location = new System.Drawing.Point(16, 354);
            this.lblGrossAmt.Name = "lblGrossAmt";
            this.lblGrossAmt.Size = new System.Drawing.Size(124, 20);
            this.lblGrossAmt.TabIndex = 11;
            this.lblGrossAmt.Text = "Gross Amount";
            // 
            // txtTotalQty
            // 
            this.txtTotalQty.Enabled = false;
            this.txtTotalQty.Location = new System.Drawing.Point(353, 315);
            this.txtTotalQty.Name = "txtTotalQty";
            this.txtTotalQty.Size = new System.Drawing.Size(100, 26);
            this.txtTotalQty.TabIndex = 10;
            this.txtTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQty.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTotalQty.Location = new System.Drawing.Point(229, 318);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(121, 20);
            this.lblTotalQty.TabIndex = 9;
            this.lblTotalQty.Text = "Total Quantity";
            // 
            // txtNoOfItems
            // 
            this.txtNoOfItems.Enabled = false;
            this.txtNoOfItems.Location = new System.Drawing.Point(124, 315);
            this.txtNoOfItems.Name = "txtNoOfItems";
            this.txtNoOfItems.Size = new System.Drawing.Size(100, 26);
            this.txtNoOfItems.TabIndex = 8;
            this.txtNoOfItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNoOfItems
            // 
            this.lblNoOfItems.AutoSize = true;
            this.lblNoOfItems.BackColor = System.Drawing.Color.Transparent;
            this.lblNoOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoOfItems.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblNoOfItems.Location = new System.Drawing.Point(11, 318);
            this.lblNoOfItems.Name = "lblNoOfItems";
            this.lblNoOfItems.Size = new System.Drawing.Size(107, 20);
            this.lblNoOfItems.TabIndex = 7;
            this.lblNoOfItems.Text = "No. of Items";
            // 
            // pnlHoriz3
            // 
            this.pnlHoriz3.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHoriz3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHoriz3.Location = new System.Drawing.Point(-1, 343);
            this.pnlHoriz3.Name = "pnlHoriz3";
            this.pnlHoriz3.Size = new System.Drawing.Size(471, 5);
            this.pnlHoriz3.TabIndex = 6;
            // 
            // lblInvoiceDt
            // 
            this.lblInvoiceDt.AutoSize = true;
            this.lblInvoiceDt.BackColor = System.Drawing.Color.Transparent;
            this.lblInvoiceDt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceDt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblInvoiceDt.Location = new System.Drawing.Point(224, 5);
            this.lblInvoiceDt.Name = "lblInvoiceDt";
            this.lblInvoiceDt.Size = new System.Drawing.Size(103, 19);
            this.lblInvoiceDt.TabIndex = 4;
            this.lblInvoiceDt.Text = "Invoice Date";
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.LightGray;
            this.txtInvoiceNo.Enabled = false;
            this.txtInvoiceNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceNo.Location = new System.Drawing.Point(117, 2);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(100, 26);
            this.txtInvoiceNo.TabIndex = 3;
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.BackColor = System.Drawing.Color.Transparent;
            this.lblInvoiceNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceNo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblInvoiceNo.Location = new System.Drawing.Point(20, 5);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Size = new System.Drawing.Size(94, 19);
            this.lblInvoiceNo.TabIndex = 2;
            this.lblInvoiceNo.Text = "Invoice No.";
            // 
            // pnlHoriz2
            // 
            this.pnlHoriz2.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHoriz2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHoriz2.Location = new System.Drawing.Point(-1, 309);
            this.pnlHoriz2.Name = "pnlHoriz2";
            this.pnlHoriz2.Size = new System.Drawing.Size(471, 5);
            this.pnlHoriz2.TabIndex = 1;
            // 
            // pnlHoriz1
            // 
            this.pnlHoriz1.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHoriz1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHoriz1.Location = new System.Drawing.Point(-2, 29);
            this.pnlHoriz1.Name = "pnlHoriz1";
            this.pnlHoriz1.Size = new System.Drawing.Size(471, 5);
            this.pnlHoriz1.TabIndex = 0;
            // 
            // pnlRightBox
            // 
            this.pnlRightBox.BackColor = System.Drawing.Color.Silver;
            this.pnlRightBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRightBox.Controls.Add(this.btnPayment);
            this.pnlRightBox.Controls.Add(this.btnExit);
            this.pnlRightBox.Controls.Add(this.btnPrintInv);
            this.pnlRightBox.Controls.Add(this.btnSearchInv);
            this.pnlRightBox.Controls.Add(this.lblActionBtn);
            this.pnlRightBox.Location = new System.Drawing.Point(621, 3);
            this.pnlRightBox.Name = "pnlRightBox";
            this.pnlRightBox.Size = new System.Drawing.Size(142, 516);
            this.pnlRightBox.TabIndex = 1;
            // 
            // btnPayment
            // 
            this.btnPayment.Location = new System.Drawing.Point(28, 128);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(84, 46);
            this.btnPayment.TabIndex = 9;
            this.btnPayment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPayment.UseVisualStyleBackColor = true;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(28, 174);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(84, 46);
            this.btnExit.TabIndex = 8;
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrintInv
            // 
            this.btnPrintInv.Location = new System.Drawing.Point(28, 82);
            this.btnPrintInv.Name = "btnPrintInv";
            this.btnPrintInv.Size = new System.Drawing.Size(84, 46);
            this.btnPrintInv.TabIndex = 7;
            this.btnPrintInv.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintInv.UseVisualStyleBackColor = true;
            // 
            // btnSearchInv
            // 
            this.btnSearchInv.Image = global::PointofSale.Properties.Resources.loc_on;
            this.btnSearchInv.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchInv.Location = new System.Drawing.Point(28, 36);
            this.btnSearchInv.Name = "btnSearchInv";
            this.btnSearchInv.Size = new System.Drawing.Size(84, 46);
            this.btnSearchInv.TabIndex = 6;
            this.btnSearchInv.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchInv.UseVisualStyleBackColor = true;
            // 
            // lblActionBtn
            // 
            this.lblActionBtn.BackColor = System.Drawing.Color.Transparent;
            this.lblActionBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionBtn.ForeColor = System.Drawing.Color.Blue;
            this.lblActionBtn.Location = new System.Drawing.Point(9, 9);
            this.lblActionBtn.Name = "lblActionBtn";
            this.lblActionBtn.Size = new System.Drawing.Size(123, 19);
            this.lblActionBtn.TabIndex = 5;
            this.lblActionBtn.Text = "Action Buttons";
            // 
            // pnlLeftBox
            // 
            this.pnlLeftBox.BackColor = System.Drawing.Color.Silver;
            this.pnlLeftBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLeftBox.Controls.Add(this.label7);
            this.pnlLeftBox.Controls.Add(this.label6);
            this.pnlLeftBox.Controls.Add(this.label5);
            this.pnlLeftBox.Controls.Add(this.label4);
            this.pnlLeftBox.Controls.Add(this.label3);
            this.pnlLeftBox.Controls.Add(this.label2);
            this.pnlLeftBox.Controls.Add(this.label1);
            this.pnlLeftBox.Controls.Add(this.lblAddItemShrtKey);
            this.pnlLeftBox.Controls.Add(this.txtCategory);
            this.pnlLeftBox.Controls.Add(this.txtDepartment);
            this.pnlLeftBox.Controls.Add(this.txtInvoiceSr);
            this.pnlLeftBox.Controls.Add(this.txtUserName);
            this.pnlLeftBox.Controls.Add(this.lblLoginDetails);
            this.pnlLeftBox.Location = new System.Drawing.Point(3, 3);
            this.pnlLeftBox.Name = "pnlLeftBox";
            this.pnlLeftBox.Size = new System.Drawing.Size(142, 516);
            this.pnlLeftBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "Shortcut Keys";
            // 
            // lblAddItemShrtKey
            // 
            this.lblAddItemShrtKey.AutoSize = true;
            this.lblAddItemShrtKey.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddItemShrtKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblAddItemShrtKey.Location = new System.Drawing.Point(-1, 193);
            this.lblAddItemShrtKey.Name = "lblAddItemShrtKey";
            this.lblAddItemShrtKey.Size = new System.Drawing.Size(186, 22);
            this.lblAddItemShrtKey.TabIndex = 5;
            this.lblAddItemShrtKey.Text = "[F1] - Add New Line";
            // 
            // txtCategory
            // 
            this.txtCategory.BackColor = System.Drawing.Color.White;
            this.txtCategory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCategory.Enabled = false;
            this.txtCategory.ForeColor = System.Drawing.Color.Teal;
            this.txtCategory.Location = new System.Drawing.Point(2, 121);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(136, 26);
            this.txtCategory.TabIndex = 4;
            // 
            // txtDepartment
            // 
            this.txtDepartment.BackColor = System.Drawing.Color.White;
            this.txtDepartment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDepartment.Enabled = false;
            this.txtDepartment.ForeColor = System.Drawing.Color.Teal;
            this.txtDepartment.Location = new System.Drawing.Point(2, 92);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(136, 26);
            this.txtDepartment.TabIndex = 3;
            // 
            // txtInvoiceSr
            // 
            this.txtInvoiceSr.BackColor = System.Drawing.Color.White;
            this.txtInvoiceSr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvoiceSr.Enabled = false;
            this.txtInvoiceSr.ForeColor = System.Drawing.Color.Teal;
            this.txtInvoiceSr.Location = new System.Drawing.Point(2, 63);
            this.txtInvoiceSr.Name = "txtInvoiceSr";
            this.txtInvoiceSr.Size = new System.Drawing.Size(136, 26);
            this.txtInvoiceSr.TabIndex = 2;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.White;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Enabled = false;
            this.txtUserName.ForeColor = System.Drawing.Color.Teal;
            this.txtUserName.Location = new System.Drawing.Point(2, 34);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(136, 26);
            this.txtUserName.TabIndex = 1;
            // 
            // lblLoginDetails
            // 
            this.lblLoginDetails.AutoSize = true;
            this.lblLoginDetails.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginDetails.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginDetails.ForeColor = System.Drawing.Color.Blue;
            this.lblLoginDetails.Location = new System.Drawing.Point(12, 9);
            this.lblLoginDetails.Name = "lblLoginDetails";
            this.lblLoginDetails.Size = new System.Drawing.Size(109, 19);
            this.lblLoginDetails.TabIndex = 0;
            this.lblLoginDetails.Text = "Login Details";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(-1, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 22);
            this.label2.TabIndex = 7;
            this.label2.Text = "[F2] - List Item";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Location = new System.Drawing.Point(-1, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "[F3] - Search Bill";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label4.Location = new System.Drawing.Point(-1, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 22);
            this.label4.TabIndex = 9;
            this.label4.Text = "[F4] - Pay Bill";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label5.Location = new System.Drawing.Point(-1, 313);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(163, 22);
            this.label5.TabIndex = 10;
            this.label5.Text = "[F5] - RePrint Bill";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label6.Location = new System.Drawing.Point(-1, 343);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(208, 22);
            this.label6.TabIndex = 10;
            this.label6.Text = "[F6] - Delete Line Item";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label7.Location = new System.Drawing.Point(-2, 374);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(153, 22);
            this.label7.TabIndex = 10;
            this.label7.Text = "[F12] - Exit POS";
            // 
            // udFrmPointofSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(775, 529);
            this.Controls.Add(this.pnlOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "udFrmPointofSale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Point of Sale";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.udFrmPointofSale_Load);
            this.pnlOuter.ResumeLayout(false);
            this.pnlCenterBox.ResumeLayout(false);
            this.pnlCenterBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayDetails)).EndInit();
            this.pnlRightBox.ResumeLayout(false);
            this.pnlLeftBox.ResumeLayout(false);
            this.pnlLeftBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Panel pnlRightBox;
        private System.Windows.Forms.Panel pnlLeftBox;
        private System.Windows.Forms.Panel pnlCenterBox;
        private System.Windows.Forms.Panel pnlHoriz1;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.Panel pnlHoriz2;
        private System.Windows.Forms.Label lblInvoiceDt;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.Label lblLoginDetails;
        private System.Windows.Forms.TextBox txtTotalQty;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.TextBox txtNoOfItems;
        private System.Windows.Forms.Label lblNoOfItems;
        private System.Windows.Forms.Panel pnlHoriz3;
        private System.Windows.Forms.TextBox txtNetAmt;
        private System.Windows.Forms.Label lblNetAmt;
        private System.Windows.Forms.TextBox txtDiscAmt;
        private System.Windows.Forms.Label lblDiscAmt;
        private System.Windows.Forms.TextBox txtGrossAmt;
        private System.Windows.Forms.Label lblGrossAmt;
        private System.Windows.Forms.Panel pnlHoriz4;
        private System.Windows.Forms.DataGridView dgvPayDetails;
        private System.Windows.Forms.Label lblPayDetails;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtInvoiceSr;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.DataGridView dgvItemDetails;
        private System.Windows.Forms.Label lblActionBtn;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrintInv;
        private System.Windows.Forms.Button btnSearchInv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentType;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colPaymentAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colQuantity;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colRate;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colDiscount;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount;
        private System.Windows.Forms.DateTimePicker txtInvoiceDt;
        private System.Windows.Forms.Label lblAddItemShrtKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

