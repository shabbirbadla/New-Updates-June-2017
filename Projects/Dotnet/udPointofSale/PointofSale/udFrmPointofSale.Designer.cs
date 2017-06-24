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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.stStripMenu = new System.Windows.Forms.StatusStrip();
            this.tsUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsInvoiceSeries = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsDepartment = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsCategory = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlCenterBox = new System.Windows.Forms.Panel();
            this.pnlPrintBill = new System.Windows.Forms.Panel();
            this.lblPrintBill = new System.Windows.Forms.Label();
            this.pgrsBar = new System.Windows.Forms.ProgressBar();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.colSumHeadNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumAmount = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.dgvDiscChrgs = new System.Windows.Forms.DataGridView();
            this.colHeading = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercent = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colTaxAmount = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.btnAddInfo = new System.Windows.Forms.Button();
            this.cmdWarehouse = new System.Windows.Forms.Button();
            this.cmdParty = new System.Windows.Forms.Button();
            this.txtInvoiceDt = new System.Windows.Forms.DateTimePicker();
            this.dgvItemDetails = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colRate = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colAmount = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.txtTotalQty = new System.Windows.Forms.TextBox();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.txtNoOfItems = new System.Windows.Forms.TextBox();
            this.lblNoOfItems = new System.Windows.Forms.Label();
            this.pnlHoriz3 = new System.Windows.Forms.Panel();
            this.lblInvoiceDt = new System.Windows.Forms.Label();
            this.txtWareHouse = new System.Windows.Forms.TextBox();
            this.txtParty = new System.Windows.Forms.TextBox();
            this.lblWarehouse = new System.Windows.Forms.Label();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.lblParty = new System.Windows.Forms.Label();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.pnlHoriz2 = new System.Windows.Forms.Panel();
            this.pnlHoriz1 = new System.Windows.Forms.Panel();
            this.pnlRightBox = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblExitPOS = new System.Windows.Forms.Label();
            this.btnPayment = new System.Windows.Forms.Button();
            this.lblReprintBill = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblPayBill = new System.Windows.Forms.Label();
            this.btnPrintInv = new System.Windows.Forms.Button();
            this.lblListItem = new System.Windows.Forms.Label();
            this.lblActionBtn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAddItemShrtKey = new System.Windows.Forms.Label();
            this.lblGrossAmt = new System.Windows.Forms.Label();
            this.txtGrossAmt = new System.Windows.Forms.TextBox();
            this.txtNetAmt = new System.Windows.Forms.TextBox();
            this.lblTaxAmt = new System.Windows.Forms.Label();
            this.lblNetAmt = new System.Windows.Forms.Label();
            this.txtTaxAmt = new System.Windows.Forms.TextBox();
            this.pnlOuter.SuspendLayout();
            this.stStripMenu.SuspendLayout();
            this.pnlCenterBox.SuspendLayout();
            this.pnlPrintBill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscChrgs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).BeginInit();
            this.pnlRightBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOuter
            // 
            this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOuter.Controls.Add(this.stStripMenu);
            this.pnlOuter.Controls.Add(this.pnlCenterBox);
            this.pnlOuter.Controls.Add(this.pnlRightBox);
            this.pnlOuter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlOuter.Location = new System.Drawing.Point(4, 3);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(768, 523);
            this.pnlOuter.TabIndex = 0;
            // 
            // stStripMenu
            // 
            this.stStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUserName,
            this.tsCounter,
            this.tsInvoiceSeries,
            this.tsDepartment,
            this.tsCategory});
            this.stStripMenu.Location = new System.Drawing.Point(0, 494);
            this.stStripMenu.Name = "stStripMenu";
            this.stStripMenu.Size = new System.Drawing.Size(766, 27);
            this.stStripMenu.TabIndex = 3;
            this.stStripMenu.Text = "statusStrip1";
            // 
            // tsUserName
            // 
            this.tsUserName.AutoSize = false;
            this.tsUserName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsUserName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsUserName.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsUserName.ForeColor = System.Drawing.Color.Blue;
            this.tsUserName.Name = "tsUserName";
            this.tsUserName.Size = new System.Drawing.Size(145, 22);
            this.tsUserName.Text = "User Name";
            this.tsUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsCounter
            // 
            this.tsCounter.AutoSize = false;
            this.tsCounter.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsCounter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsCounter.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsCounter.ForeColor = System.Drawing.Color.Blue;
            this.tsCounter.Name = "tsCounter";
            this.tsCounter.Size = new System.Drawing.Size(145, 22);
            this.tsCounter.Text = "Counter";
            this.tsCounter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsInvoiceSeries
            // 
            this.tsInvoiceSeries.AutoSize = false;
            this.tsInvoiceSeries.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsInvoiceSeries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsInvoiceSeries.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsInvoiceSeries.ForeColor = System.Drawing.Color.Blue;
            this.tsInvoiceSeries.Name = "tsInvoiceSeries";
            this.tsInvoiceSeries.Size = new System.Drawing.Size(145, 22);
            this.tsInvoiceSeries.Text = "Invoice Series";
            this.tsInvoiceSeries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsDepartment
            // 
            this.tsDepartment.AutoSize = false;
            this.tsDepartment.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsDepartment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsDepartment.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsDepartment.ForeColor = System.Drawing.Color.Blue;
            this.tsDepartment.Name = "tsDepartment";
            this.tsDepartment.Size = new System.Drawing.Size(145, 22);
            this.tsDepartment.Text = "Department";
            this.tsDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsCategory
            // 
            this.tsCategory.AutoSize = false;
            this.tsCategory.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsCategory.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsCategory.ForeColor = System.Drawing.Color.Blue;
            this.tsCategory.Name = "tsCategory";
            this.tsCategory.Size = new System.Drawing.Size(170, 22);
            this.tsCategory.Text = "Category";
            this.tsCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlCenterBox
            // 
            this.pnlCenterBox.BackColor = System.Drawing.Color.Silver;
            this.pnlCenterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCenterBox.Controls.Add(this.pnlPrintBill);
            this.pnlCenterBox.Controls.Add(this.dgvSummary);
            this.pnlCenterBox.Controls.Add(this.dgvDiscChrgs);
            this.pnlCenterBox.Controls.Add(this.btnAddInfo);
            this.pnlCenterBox.Controls.Add(this.cmdWarehouse);
            this.pnlCenterBox.Controls.Add(this.cmdParty);
            this.pnlCenterBox.Controls.Add(this.txtInvoiceDt);
            this.pnlCenterBox.Controls.Add(this.dgvItemDetails);
            this.pnlCenterBox.Controls.Add(this.txtTotalQty);
            this.pnlCenterBox.Controls.Add(this.lblTotalQty);
            this.pnlCenterBox.Controls.Add(this.txtNoOfItems);
            this.pnlCenterBox.Controls.Add(this.lblNoOfItems);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz3);
            this.pnlCenterBox.Controls.Add(this.lblInvoiceDt);
            this.pnlCenterBox.Controls.Add(this.txtWareHouse);
            this.pnlCenterBox.Controls.Add(this.txtParty);
            this.pnlCenterBox.Controls.Add(this.lblWarehouse);
            this.pnlCenterBox.Controls.Add(this.txtInvoiceNo);
            this.pnlCenterBox.Controls.Add(this.lblParty);
            this.pnlCenterBox.Controls.Add(this.lblInvoiceNo);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz2);
            this.pnlCenterBox.Controls.Add(this.pnlHoriz1);
            this.pnlCenterBox.Location = new System.Drawing.Point(3, 3);
            this.pnlCenterBox.Name = "pnlCenterBox";
            this.pnlCenterBox.Size = new System.Drawing.Size(615, 516);
            this.pnlCenterBox.TabIndex = 0;
            // 
            // pnlPrintBill
            // 
            this.pnlPrintBill.Controls.Add(this.lblPrintBill);
            this.pnlPrintBill.Controls.Add(this.pgrsBar);
            this.pnlPrintBill.Location = new System.Drawing.Point(185, 131);
            this.pnlPrintBill.Name = "pnlPrintBill";
            this.pnlPrintBill.Size = new System.Drawing.Size(318, 64);
            this.pnlPrintBill.TabIndex = 22;
            // 
            // lblPrintBill
            // 
            this.lblPrintBill.AutoSize = true;
            this.lblPrintBill.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintBill.Location = new System.Drawing.Point(9, 8);
            this.lblPrintBill.Name = "lblPrintBill";
            this.lblPrintBill.Size = new System.Drawing.Size(104, 20);
            this.lblPrintBill.TabIndex = 23;
            this.lblPrintBill.Text = "Printing Bill.....";
            // 
            // pgrsBar
            // 
            this.pgrsBar.BackColor = System.Drawing.Color.Silver;
            this.pgrsBar.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.pgrsBar.Location = new System.Drawing.Point(9, 31);
            this.pgrsBar.Name = "pgrsBar";
            this.pgrsBar.Size = new System.Drawing.Size(300, 23);
            this.pgrsBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgrsBar.TabIndex = 22;
            // 
            // dgvSummary
            // 
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.AllowUserToResizeColumns = false;
            this.dgvSummary.AllowUserToResizeRows = false;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSumHeadNm,
            this.colSumAmount});
            this.dgvSummary.Location = new System.Drawing.Point(308, 350);
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.RowHeadersVisible = false;
            this.dgvSummary.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSummary.Size = new System.Drawing.Size(303, 144);
            this.dgvSummary.TabIndex = 0;
            // 
            // colSumHeadNm
            // 
            this.colSumHeadNm.HeaderText = "Heading Name";
            this.colSumHeadNm.Name = "colSumHeadNm";
            this.colSumHeadNm.ReadOnly = true;
            this.colSumHeadNm.Width = 320;
            // 
            // colSumAmount
            // 
            this.colSumAmount.DecimalLength = 2;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "F2";
            this.colSumAmount.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSumAmount.HeaderText = "Amount";
            this.colSumAmount.Name = "colSumAmount";
            this.colSumAmount.pValidFunNm = null;
            this.colSumAmount.ReadOnly = true;
            this.colSumAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSumAmount.Width = 150;
            // 
            // dgvDiscChrgs
            // 
            this.dgvDiscChrgs.AllowUserToAddRows = false;
            this.dgvDiscChrgs.AllowUserToDeleteRows = false;
            this.dgvDiscChrgs.AllowUserToResizeColumns = false;
            this.dgvDiscChrgs.AllowUserToResizeRows = false;
            this.dgvDiscChrgs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscChrgs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHeading,
            this.colPercent,
            this.colTaxAmount});
            this.dgvDiscChrgs.Location = new System.Drawing.Point(3, 350);
            this.dgvDiscChrgs.Name = "dgvDiscChrgs";
            this.dgvDiscChrgs.RowHeadersVisible = false;
            this.dgvDiscChrgs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDiscChrgs.Size = new System.Drawing.Size(303, 144);
            this.dgvDiscChrgs.TabIndex = 20;
            this.dgvDiscChrgs.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscChrgs_CellEnter);
            this.dgvDiscChrgs.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscChrgs_CellValidated);
            this.dgvDiscChrgs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDiscChrgs_KeyDown);
            // 
            // colHeading
            // 
            this.colHeading.HeaderText = "Heading Name";
            this.colHeading.Name = "colHeading";
            this.colHeading.Width = 270;
            // 
            // colPercent
            // 
            this.colPercent.DecimalLength = 2;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "F2";
            this.colPercent.DefaultCellStyle = dataGridViewCellStyle2;
            this.colPercent.HeaderText = "%age";
            this.colPercent.Name = "colPercent";
            this.colPercent.pValidFunNm = null;
            this.colPercent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPercent.Width = 80;
            // 
            // colTaxAmount
            // 
            this.colTaxAmount.DecimalLength = 2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "F2";
            this.colTaxAmount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTaxAmount.HeaderText = "Amount";
            this.colTaxAmount.Name = "colTaxAmount";
            this.colTaxAmount.pValidFunNm = null;
            this.colTaxAmount.ReadOnly = true;
            this.colTaxAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTaxAmount.Width = 140;
            // 
            // btnAddInfo
            // 
            this.btnAddInfo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddInfo.Location = new System.Drawing.Point(480, 60);
            this.btnAddInfo.Name = "btnAddInfo";
            this.btnAddInfo.Size = new System.Drawing.Size(130, 26);
            this.btnAddInfo.TabIndex = 11;
            this.btnAddInfo.Text = "Add. Info.";
            this.btnAddInfo.UseVisualStyleBackColor = true;
            this.btnAddInfo.Click += new System.EventHandler(this.btnAddInfo_Click);
            // 
            // cmdWarehouse
            // 
            this.cmdWarehouse.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdWarehouse.Location = new System.Drawing.Point(437, 60);
            this.cmdWarehouse.Name = "cmdWarehouse";
            this.cmdWarehouse.Size = new System.Drawing.Size(37, 26);
            this.cmdWarehouse.TabIndex = 11;
            this.cmdWarehouse.Text = "...";
            this.cmdWarehouse.UseVisualStyleBackColor = true;
            this.cmdWarehouse.Click += new System.EventHandler(this.cmdWarehouse_Click);
            // 
            // cmdParty
            // 
            this.cmdParty.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdParty.Location = new System.Drawing.Point(574, 31);
            this.cmdParty.Name = "cmdParty";
            this.cmdParty.Size = new System.Drawing.Size(37, 26);
            this.cmdParty.TabIndex = 8;
            this.cmdParty.Text = "...";
            this.cmdParty.UseVisualStyleBackColor = true;
            this.cmdParty.Click += new System.EventHandler(this.cmdParty_Click);
            // 
            // txtInvoiceDt
            // 
            this.txtInvoiceDt.CalendarFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceDt.CalendarForeColor = System.Drawing.SystemColors.WindowText;
            this.txtInvoiceDt.CalendarMonthBackground = System.Drawing.SystemColors.ControlDark;
            this.txtInvoiceDt.CustomFormat = "dd/MM/yyyy";
            this.txtInvoiceDt.Enabled = false;
            this.txtInvoiceDt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtInvoiceDt.Location = new System.Drawing.Point(508, 2);
            this.txtInvoiceDt.Name = "txtInvoiceDt";
            this.txtInvoiceDt.Size = new System.Drawing.Size(100, 26);
            this.txtInvoiceDt.TabIndex = 5;
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
            this.colAmount,
            this.colDelete});
            this.dgvItemDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvItemDetails.Location = new System.Drawing.Point(2, 95);
            this.dgvItemDetails.MultiSelect = false;
            this.dgvItemDetails.Name = "dgvItemDetails";
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvItemDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvItemDetails.RowHeadersVisible = false;
            this.dgvItemDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvItemDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvItemDetails.Size = new System.Drawing.Size(609, 212);
            this.dgvItemDetails.TabIndex = 13;
            this.dgvItemDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_CellContentClick);
            this.dgvItemDetails.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_CellEndEdit);
            this.dgvItemDetails.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_CellEnter);
            this.dgvItemDetails.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_CellMouseEnter);
            this.dgvItemDetails.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_CellValidated);
            this.dgvItemDetails.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvItemDetails_CellValidating);
            this.dgvItemDetails.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvItemDetails_EditingControlShowing);
            this.dgvItemDetails.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemDetails_RowEnter);
            this.dgvItemDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvItemDetails_KeyDown);
            // 
            // colSrNo
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.NullValue = null;
            this.colSrNo.DefaultCellStyle = dataGridViewCellStyle4;
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "F3";
            this.colQuantity.DefaultCellStyle = dataGridViewCellStyle5;
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.pValidFunNm = null;
            this.colQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colRate
            // 
            this.colRate.DecimalLength = 2;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "F2";
            dataGridViewCellStyle6.NullValue = null;
            this.colRate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colRate.HeaderText = "Rate";
            this.colRate.Name = "colRate";
            this.colRate.pValidFunNm = null;
            this.colRate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colAmount
            // 
            this.colAmount.DecimalLength = 2;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "F2";
            this.colAmount.DefaultCellStyle = dataGridViewCellStyle7;
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.pValidFunNm = null;
            this.colAmount.ReadOnly = true;
            this.colAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colDelete
            // 
            this.colDelete.HeaderText = "Delete";
            this.colDelete.Name = "colDelete";
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 75;
            // 
            // txtTotalQty
            // 
            this.txtTotalQty.Enabled = false;
            this.txtTotalQty.Location = new System.Drawing.Point(448, 315);
            this.txtTotalQty.Name = "txtTotalQty";
            this.txtTotalQty.Size = new System.Drawing.Size(160, 26);
            this.txtTotalQty.TabIndex = 18;
            this.txtTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQty.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTotalQty.Location = new System.Drawing.Point(324, 318);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(121, 20);
            this.lblTotalQty.TabIndex = 17;
            this.lblTotalQty.Text = "Total Quantity";
            // 
            // txtNoOfItems
            // 
            this.txtNoOfItems.Enabled = false;
            this.txtNoOfItems.Location = new System.Drawing.Point(124, 315);
            this.txtNoOfItems.Name = "txtNoOfItems";
            this.txtNoOfItems.Size = new System.Drawing.Size(160, 26);
            this.txtNoOfItems.TabIndex = 16;
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
            this.lblNoOfItems.TabIndex = 15;
            this.lblNoOfItems.Text = "No. of Items";
            // 
            // pnlHoriz3
            // 
            this.pnlHoriz3.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHoriz3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHoriz3.Location = new System.Drawing.Point(-1, 343);
            this.pnlHoriz3.Name = "pnlHoriz3";
            this.pnlHoriz3.Size = new System.Drawing.Size(615, 5);
            this.pnlHoriz3.TabIndex = 19;
            // 
            // lblInvoiceDt
            // 
            this.lblInvoiceDt.AutoSize = true;
            this.lblInvoiceDt.BackColor = System.Drawing.Color.Transparent;
            this.lblInvoiceDt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceDt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblInvoiceDt.Location = new System.Drawing.Point(398, 5);
            this.lblInvoiceDt.Name = "lblInvoiceDt";
            this.lblInvoiceDt.Size = new System.Drawing.Size(103, 19);
            this.lblInvoiceDt.TabIndex = 4;
            this.lblInvoiceDt.Text = "Invoice Date";
            // 
            // txtWareHouse
            // 
            this.txtWareHouse.BackColor = System.Drawing.Color.White;
            this.txtWareHouse.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWareHouse.Location = new System.Drawing.Point(117, 60);
            this.txtWareHouse.Name = "txtWareHouse";
            this.txtWareHouse.ReadOnly = true;
            this.txtWareHouse.Size = new System.Drawing.Size(317, 26);
            this.txtWareHouse.TabIndex = 10;
            this.txtWareHouse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWareHouse_KeyDown);
            // 
            // txtParty
            // 
            this.txtParty.BackColor = System.Drawing.Color.White;
            this.txtParty.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParty.Location = new System.Drawing.Point(117, 31);
            this.txtParty.Name = "txtParty";
            this.txtParty.ReadOnly = true;
            this.txtParty.Size = new System.Drawing.Size(455, 26);
            this.txtParty.TabIndex = 7;
            this.txtParty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtParty_KeyDown);
            // 
            // lblWarehouse
            // 
            this.lblWarehouse.AutoSize = true;
            this.lblWarehouse.BackColor = System.Drawing.Color.Transparent;
            this.lblWarehouse.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouse.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblWarehouse.Location = new System.Drawing.Point(20, 63);
            this.lblWarehouse.Name = "lblWarehouse";
            this.lblWarehouse.Size = new System.Drawing.Size(95, 19);
            this.lblWarehouse.TabIndex = 9;
            this.lblWarehouse.Text = "Warehouse";
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
            // lblParty
            // 
            this.lblParty.AutoSize = true;
            this.lblParty.BackColor = System.Drawing.Color.Transparent;
            this.lblParty.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParty.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblParty.Location = new System.Drawing.Point(20, 34);
            this.lblParty.Name = "lblParty";
            this.lblParty.Size = new System.Drawing.Size(49, 19);
            this.lblParty.TabIndex = 6;
            this.lblParty.Text = "Party";
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
            this.pnlHoriz2.Size = new System.Drawing.Size(615, 5);
            this.pnlHoriz2.TabIndex = 14;
            // 
            // pnlHoriz1
            // 
            this.pnlHoriz1.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHoriz1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHoriz1.Location = new System.Drawing.Point(-2, 88);
            this.pnlHoriz1.Name = "pnlHoriz1";
            this.pnlHoriz1.Size = new System.Drawing.Size(615, 5);
            this.pnlHoriz1.TabIndex = 12;
            // 
            // pnlRightBox
            // 
            this.pnlRightBox.BackColor = System.Drawing.Color.Silver;
            this.pnlRightBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRightBox.Controls.Add(this.label2);
            this.pnlRightBox.Controls.Add(this.lblExitPOS);
            this.pnlRightBox.Controls.Add(this.btnPayment);
            this.pnlRightBox.Controls.Add(this.lblReprintBill);
            this.pnlRightBox.Controls.Add(this.btnExit);
            this.pnlRightBox.Controls.Add(this.lblPayBill);
            this.pnlRightBox.Controls.Add(this.btnPrintInv);
            this.pnlRightBox.Controls.Add(this.lblListItem);
            this.pnlRightBox.Controls.Add(this.lblActionBtn);
            this.pnlRightBox.Controls.Add(this.label1);
            this.pnlRightBox.Controls.Add(this.lblAddItemShrtKey);
            this.pnlRightBox.Controls.Add(this.lblGrossAmt);
            this.pnlRightBox.Controls.Add(this.txtGrossAmt);
            this.pnlRightBox.Controls.Add(this.txtNetAmt);
            this.pnlRightBox.Controls.Add(this.lblTaxAmt);
            this.pnlRightBox.Controls.Add(this.lblNetAmt);
            this.pnlRightBox.Controls.Add(this.txtTaxAmt);
            this.pnlRightBox.Location = new System.Drawing.Point(621, 3);
            this.pnlRightBox.Name = "pnlRightBox";
            this.pnlRightBox.Size = new System.Drawing.Size(142, 494);
            this.pnlRightBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(1, 443);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(124, 18);
            this.label2.TabIndex = 11;
            this.label2.Text = "[F6] - Cancel Bill";
            // 
            // lblExitPOS
            // 
            this.lblExitPOS.AutoSize = true;
            this.lblExitPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExitPOS.ForeColor = System.Drawing.Color.Green;
            this.lblExitPOS.Location = new System.Drawing.Point(1, 468);
            this.lblExitPOS.Name = "lblExitPOS";
            this.lblExitPOS.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblExitPOS.Size = new System.Drawing.Size(119, 18);
            this.lblExitPOS.TabIndex = 10;
            this.lblExitPOS.Text = "[F12] - Exit POS";
            // 
            // btnPayment
            // 
            this.btnPayment.Location = new System.Drawing.Point(12, 80);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(116, 46);
            this.btnPayment.TabIndex = 2;
            this.btnPayment.Text = "Pay Bill";
            this.btnPayment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPayment.UseVisualStyleBackColor = true;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            this.btnPayment.MouseHover += new System.EventHandler(this.btnPayment_MouseHover);
            // 
            // lblReprintBill
            // 
            this.lblReprintBill.AutoSize = true;
            this.lblReprintBill.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReprintBill.ForeColor = System.Drawing.Color.Green;
            this.lblReprintBill.Location = new System.Drawing.Point(1, 418);
            this.lblReprintBill.Name = "lblReprintBill";
            this.lblReprintBill.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblReprintBill.Size = new System.Drawing.Size(128, 18);
            this.lblReprintBill.TabIndex = 9;
            this.lblReprintBill.Text = "[F5] - Reprint Bill";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 126);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(116, 46);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.MouseHover += new System.EventHandler(this.btnExit_MouseHover);
            // 
            // lblPayBill
            // 
            this.lblPayBill.AutoSize = true;
            this.lblPayBill.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayBill.ForeColor = System.Drawing.Color.Green;
            this.lblPayBill.Location = new System.Drawing.Point(1, 393);
            this.lblPayBill.Name = "lblPayBill";
            this.lblPayBill.Size = new System.Drawing.Size(100, 18);
            this.lblPayBill.TabIndex = 8;
            this.lblPayBill.Text = "[F4] - Pay Bill";
            // 
            // btnPrintInv
            // 
            this.btnPrintInv.Location = new System.Drawing.Point(12, 34);
            this.btnPrintInv.Name = "btnPrintInv";
            this.btnPrintInv.Size = new System.Drawing.Size(116, 46);
            this.btnPrintInv.TabIndex = 1;
            this.btnPrintInv.Text = "Reprint Bill";
            this.btnPrintInv.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintInv.UseVisualStyleBackColor = true;
            this.btnPrintInv.Click += new System.EventHandler(this.btnPrintInv_Click);
            this.btnPrintInv.MouseHover += new System.EventHandler(this.btnPrintInv_MouseHover);
            // 
            // lblListItem
            // 
            this.lblListItem.AutoSize = true;
            this.lblListItem.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListItem.ForeColor = System.Drawing.Color.Green;
            this.lblListItem.Location = new System.Drawing.Point(1, 368);
            this.lblListItem.Name = "lblListItem";
            this.lblListItem.Size = new System.Drawing.Size(142, 18);
            this.lblListItem.TabIndex = 7;
            this.lblListItem.Text = "[F3] - Add new Line";
            // 
            // lblActionBtn
            // 
            this.lblActionBtn.BackColor = System.Drawing.Color.Transparent;
            this.lblActionBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionBtn.ForeColor = System.Drawing.Color.Blue;
            this.lblActionBtn.Location = new System.Drawing.Point(9, 9);
            this.lblActionBtn.Name = "lblActionBtn";
            this.lblActionBtn.Size = new System.Drawing.Size(123, 19);
            this.lblActionBtn.TabIndex = 0;
            this.lblActionBtn.Text = "Action Buttons";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(14, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Shortcut Keys";
            // 
            // lblAddItemShrtKey
            // 
            this.lblAddItemShrtKey.AutoSize = true;
            this.lblAddItemShrtKey.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddItemShrtKey.ForeColor = System.Drawing.Color.Green;
            this.lblAddItemShrtKey.Location = new System.Drawing.Point(1, 343);
            this.lblAddItemShrtKey.Name = "lblAddItemShrtKey";
            this.lblAddItemShrtKey.Size = new System.Drawing.Size(143, 18);
            this.lblAddItemShrtKey.TabIndex = 6;
            this.lblAddItemShrtKey.Text = "[F2] - List selection";
            // 
            // lblGrossAmt
            // 
            this.lblGrossAmt.AutoSize = true;
            this.lblGrossAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblGrossAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossAmt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGrossAmt.Location = new System.Drawing.Point(8, 191);
            this.lblGrossAmt.Name = "lblGrossAmt";
            this.lblGrossAmt.Size = new System.Drawing.Size(124, 20);
            this.lblGrossAmt.TabIndex = 4;
            this.lblGrossAmt.Text = "Gross Amount";
            // 
            // txtGrossAmt
            // 
            this.txtGrossAmt.BackColor = System.Drawing.Color.LightGray;
            this.txtGrossAmt.Enabled = false;
            this.txtGrossAmt.ForeColor = System.Drawing.Color.Aqua;
            this.txtGrossAmt.Location = new System.Drawing.Point(3, 214);
            this.txtGrossAmt.Name = "txtGrossAmt";
            this.txtGrossAmt.Size = new System.Drawing.Size(135, 26);
            this.txtGrossAmt.TabIndex = 5;
            this.txtGrossAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtNetAmt
            // 
            this.txtNetAmt.BackColor = System.Drawing.Color.LightGray;
            this.txtNetAmt.Enabled = false;
            this.txtNetAmt.ForeColor = System.Drawing.Color.Aqua;
            this.txtNetAmt.Location = new System.Drawing.Point(3, 269);
            this.txtNetAmt.Name = "txtNetAmt";
            this.txtNetAmt.Size = new System.Drawing.Size(135, 26);
            this.txtNetAmt.TabIndex = 7;
            this.txtNetAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTaxAmt
            // 
            this.lblTaxAmt.AutoSize = true;
            this.lblTaxAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblTaxAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAmt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTaxAmt.Location = new System.Drawing.Point(18, 301);
            this.lblTaxAmt.Name = "lblTaxAmt";
            this.lblTaxAmt.Size = new System.Drawing.Size(104, 20);
            this.lblTaxAmt.TabIndex = 8;
            this.lblTaxAmt.Text = "Tax Amount";
            this.lblTaxAmt.Visible = false;
            // 
            // lblNetAmt
            // 
            this.lblNetAmt.AutoSize = true;
            this.lblNetAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblNetAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetAmt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNetAmt.Location = new System.Drawing.Point(18, 246);
            this.lblNetAmt.Name = "lblNetAmt";
            this.lblNetAmt.Size = new System.Drawing.Size(104, 20);
            this.lblNetAmt.TabIndex = 6;
            this.lblNetAmt.Text = "Net Amount";
            // 
            // txtTaxAmt
            // 
            this.txtTaxAmt.BackColor = System.Drawing.Color.LightGray;
            this.txtTaxAmt.Enabled = false;
            this.txtTaxAmt.ForeColor = System.Drawing.Color.Aqua;
            this.txtTaxAmt.Location = new System.Drawing.Point(3, 324);
            this.txtTaxAmt.Name = "txtTaxAmt";
            this.txtTaxAmt.Size = new System.Drawing.Size(135, 26);
            this.txtTaxAmt.TabIndex = 9;
            this.txtTaxAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTaxAmt.Visible = false;
            // 
            // udFrmPointofSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 529);
            this.Controls.Add(this.pnlOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "udFrmPointofSale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Point of Sale";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.udFrmPointofSale_FormClosed);
            this.Load += new System.EventHandler(this.udFrmPointofSale_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.udFrmPointofSale_KeyDown);
            this.pnlOuter.ResumeLayout(false);
            this.pnlOuter.PerformLayout();
            this.stStripMenu.ResumeLayout(false);
            this.stStripMenu.PerformLayout();
            this.pnlCenterBox.ResumeLayout(false);
            this.pnlCenterBox.PerformLayout();
            this.pnlPrintBill.ResumeLayout(false);
            this.pnlPrintBill.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscChrgs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).EndInit();
            this.pnlRightBox.ResumeLayout(false);
            this.pnlRightBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Panel pnlRightBox;
        private System.Windows.Forms.Panel pnlCenterBox;
        private System.Windows.Forms.Panel pnlHoriz1;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.Panel pnlHoriz2;
        private System.Windows.Forms.Label lblInvoiceDt;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.TextBox txtTotalQty;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.TextBox txtNoOfItems;
        private System.Windows.Forms.Label lblNoOfItems;
        private System.Windows.Forms.Panel pnlHoriz3;
        private System.Windows.Forms.TextBox txtNetAmt;
        private System.Windows.Forms.Label lblNetAmt;
        private System.Windows.Forms.TextBox txtTaxAmt;
        private System.Windows.Forms.Label lblTaxAmt;
        private System.Windows.Forms.TextBox txtGrossAmt;
        private System.Windows.Forms.Label lblGrossAmt;
        private System.Windows.Forms.DataGridView dgvItemDetails;
        private System.Windows.Forms.Label lblActionBtn;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrintInv;
        private System.Windows.Forms.DateTimePicker txtInvoiceDt;
        private System.Windows.Forms.Label lblAddItemShrtKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPayBill;
        private System.Windows.Forms.Label lblListItem;
        private System.Windows.Forms.Label lblReprintBill;
        private System.Windows.Forms.Label lblExitPOS;
        private System.Windows.Forms.Button cmdParty;
        private System.Windows.Forms.TextBox txtParty;
        private System.Windows.Forms.Label lblParty;
        private System.Windows.Forms.DataGridView dgvDiscChrgs;
        private System.Windows.Forms.DataGridView dgvSummary;
        private System.Windows.Forms.Button cmdWarehouse;
        private System.Windows.Forms.TextBox txtWareHouse;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.StatusStrip stStripMenu;
        private System.Windows.Forms.ToolStripStatusLabel tsUserName;
        private System.Windows.Forms.ToolStripStatusLabel tsInvoiceSeries;
        private System.Windows.Forms.ToolStripStatusLabel tsDepartment;
        private System.Windows.Forms.ToolStripStatusLabel tsCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeading;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colPercent;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colTaxAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumHeadNm;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colSumAmount;
        private System.Windows.Forms.Button btnAddInfo;
        private System.Windows.Forms.Panel pnlPrintBill;
        private System.Windows.Forms.Label lblPrintBill;
        private System.Windows.Forms.ProgressBar pgrsBar;
        private System.Windows.Forms.ToolStripStatusLabel tsCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colQuantity;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colRate;
        private udclsDGVNumericColumn.CNumEditDataGridViewColumn colAmount;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.Label label2;
    }
}

