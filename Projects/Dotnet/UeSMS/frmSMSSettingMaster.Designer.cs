namespace UeSMS
{
    partial class frmSMSSettingMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSMSSettingMaster));
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.tbCntrl = new System.Windows.Forms.TabControl();
            this.tbPgSMSGtWay = new System.Windows.Forms.TabPage();
            this.btnSMSCode = new System.Windows.Forms.Button();
            this.dgvURLParam = new System.Windows.Forms.DataGridView();
            this.colParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParamDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParamVal = new System.Windows.Forms.DataGridViewButtonColumn();
            this.txtSMSURL = new System.Windows.Forms.TextBox();
            this.lblSMSURL = new System.Windows.Forms.Label();
            this.txtSMSGtWayId = new System.Windows.Forms.TextBox();
            this.lblSMSGtWayId = new System.Windows.Forms.Label();
            this.txtSMSCode = new System.Windows.Forms.TextBox();
            this.lblSMSCode = new System.Windows.Forms.Label();
            this.tbPgMobModem = new System.Windows.Forms.TabPage();
            this.txtSMSSimNo = new System.Windows.Forms.TextBox();
            this.lblSMSSimNo = new System.Windows.Forms.Label();
            this.txtTimeOut = new System.Windows.Forms.TextBox();
            this.lblTimeOut = new System.Windows.Forms.Label();
            this.txtBaudRate = new System.Windows.Forms.TextBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.txtPortName = new System.Windows.Forms.TextBox();
            this.lblPortName = new System.Windows.Forms.Label();
            this.txtSMSGtWayNm = new System.Windows.Forms.TextBox();
            this.lblSMSGtWayNm = new System.Windows.Forms.Label();
            this.grpSMSVia = new System.Windows.Forms.GroupBox();
            this.rbtnMobModem = new System.Windows.Forms.RadioButton();
            this.rbtnSMSGateway = new System.Windows.Forms.RadioButton();
            this.txtSMSSetID = new System.Windows.Forms.TextBox();
            this.lblSMSSetID = new System.Windows.Forms.Label();
            this.lblSMSVia = new System.Windows.Forms.Label();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.btnForward = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEmail = new System.Windows.Forms.ToolStripButton();
            this.btnLocate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportPdf = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnCalculator = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.pnlOuter.SuspendLayout();
            this.tbCntrl.SuspendLayout();
            this.tbPgSMSGtWay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvURLParam)).BeginInit();
            this.tbPgMobModem.SuspendLayout();
            this.grpSMSVia.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOuter
            // 
            this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOuter.Controls.Add(this.tbCntrl);
            this.pnlOuter.Controls.Add(this.txtSMSGtWayNm);
            this.pnlOuter.Controls.Add(this.lblSMSGtWayNm);
            this.pnlOuter.Controls.Add(this.grpSMSVia);
            this.pnlOuter.Controls.Add(this.txtSMSSetID);
            this.pnlOuter.Controls.Add(this.lblSMSSetID);
            this.pnlOuter.Controls.Add(this.lblSMSVia);
            this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuter.Location = new System.Drawing.Point(0, 0);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(544, 484);
            this.pnlOuter.TabIndex = 0;
            // 
            // tbCntrl
            // 
            this.tbCntrl.Controls.Add(this.tbPgSMSGtWay);
            this.tbCntrl.Controls.Add(this.tbPgMobModem);
            this.tbCntrl.Location = new System.Drawing.Point(5, 127);
            this.tbCntrl.Name = "tbCntrl";
            this.tbCntrl.SelectedIndex = 0;
            this.tbCntrl.Size = new System.Drawing.Size(533, 351);
            this.tbCntrl.TabIndex = 6;
            // 
            // tbPgSMSGtWay
            // 
            this.tbPgSMSGtWay.BackColor = System.Drawing.Color.Transparent;
            this.tbPgSMSGtWay.Controls.Add(this.btnSMSCode);
            this.tbPgSMSGtWay.Controls.Add(this.dgvURLParam);
            this.tbPgSMSGtWay.Controls.Add(this.txtSMSURL);
            this.tbPgSMSGtWay.Controls.Add(this.lblSMSURL);
            this.tbPgSMSGtWay.Controls.Add(this.txtSMSGtWayId);
            this.tbPgSMSGtWay.Controls.Add(this.lblSMSGtWayId);
            this.tbPgSMSGtWay.Controls.Add(this.txtSMSCode);
            this.tbPgSMSGtWay.Controls.Add(this.lblSMSCode);
            this.tbPgSMSGtWay.Location = new System.Drawing.Point(4, 22);
            this.tbPgSMSGtWay.Name = "tbPgSMSGtWay";
            this.tbPgSMSGtWay.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgSMSGtWay.Size = new System.Drawing.Size(525, 325);
            this.tbPgSMSGtWay.TabIndex = 0;
            this.tbPgSMSGtWay.Text = "SMS Gateway Setting";
            this.tbPgSMSGtWay.UseVisualStyleBackColor = true;
            // 
            // btnSMSCode
            // 
            this.btnSMSCode.Location = new System.Drawing.Point(241, 6);
            this.btnSMSCode.Name = "btnSMSCode";
            this.btnSMSCode.Size = new System.Drawing.Size(24, 20);
            this.btnSMSCode.TabIndex = 2;
            this.btnSMSCode.Text = "...";
            this.btnSMSCode.UseVisualStyleBackColor = true;
            this.btnSMSCode.Click += new System.EventHandler(this.btnSMSCode_Click);
            // 
            // dgvURLParam
            // 
            this.dgvURLParam.AllowUserToAddRows = false;
            this.dgvURLParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvURLParam.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colParamName,
            this.colParamDesc,
            this.colParamVal});
            this.dgvURLParam.Location = new System.Drawing.Point(4, 82);
            this.dgvURLParam.Name = "dgvURLParam";
            this.dgvURLParam.RowHeadersVisible = false;
            this.dgvURLParam.Size = new System.Drawing.Size(517, 239);
            this.dgvURLParam.TabIndex = 7;
            this.dgvURLParam.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvURLParam_CellContentClick);
            // 
            // colParamName
            // 
            this.colParamName.HeaderText = "Parameter Name(Case Sensitive)";
            this.colParamName.Name = "colParamName";
            this.colParamName.ReadOnly = true;
            this.colParamName.Width = 150;
            // 
            // colParamDesc
            // 
            this.colParamDesc.HeaderText = "Parameter Description";
            this.colParamDesc.Name = "colParamDesc";
            this.colParamDesc.ReadOnly = true;
            this.colParamDesc.Width = 250;
            // 
            // colParamVal
            // 
            this.colParamVal.HeaderText = "Parameter Values";
            this.colParamVal.Name = "colParamVal";
            this.colParamVal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colParamVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colParamVal.Text = "Values";
            this.colParamVal.UseColumnTextForButtonValue = true;
            // 
            // txtSMSURL
            // 
            this.txtSMSURL.BackColor = System.Drawing.SystemColors.Control;
            this.txtSMSURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSURL.Enabled = false;
            this.txtSMSURL.Location = new System.Drawing.Point(95, 58);
            this.txtSMSURL.Name = "txtSMSURL";
            this.txtSMSURL.Size = new System.Drawing.Size(398, 20);
            this.txtSMSURL.TabIndex = 6;
            // 
            // lblSMSURL
            // 
            this.lblSMSURL.AutoSize = true;
            this.lblSMSURL.Location = new System.Drawing.Point(16, 60);
            this.lblSMSURL.Name = "lblSMSURL";
            this.lblSMSURL.Size = new System.Drawing.Size(29, 13);
            this.lblSMSURL.TabIndex = 5;
            this.lblSMSURL.Text = "URL";
            // 
            // txtSMSGtWayId
            // 
            this.txtSMSGtWayId.BackColor = System.Drawing.SystemColors.Control;
            this.txtSMSGtWayId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSGtWayId.Enabled = false;
            this.txtSMSGtWayId.Location = new System.Drawing.Point(95, 32);
            this.txtSMSGtWayId.Name = "txtSMSGtWayId";
            this.txtSMSGtWayId.Size = new System.Drawing.Size(398, 20);
            this.txtSMSGtWayId.TabIndex = 4;
            // 
            // lblSMSGtWayId
            // 
            this.lblSMSGtWayId.AutoSize = true;
            this.lblSMSGtWayId.Location = new System.Drawing.Point(16, 34);
            this.lblSMSGtWayId.Name = "lblSMSGtWayId";
            this.lblSMSGtWayId.Size = new System.Drawing.Size(78, 13);
            this.lblSMSGtWayId.TabIndex = 3;
            this.lblSMSGtWayId.Text = "SMS Gateway ";
            // 
            // txtSMSCode
            // 
            this.txtSMSCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtSMSCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSCode.Enabled = false;
            this.txtSMSCode.Location = new System.Drawing.Point(95, 6);
            this.txtSMSCode.Name = "txtSMSCode";
            this.txtSMSCode.Size = new System.Drawing.Size(143, 20);
            this.txtSMSCode.TabIndex = 1;
            // 
            // lblSMSCode
            // 
            this.lblSMSCode.AutoSize = true;
            this.lblSMSCode.Location = new System.Drawing.Point(16, 8);
            this.lblSMSCode.Name = "lblSMSCode";
            this.lblSMSCode.Size = new System.Drawing.Size(58, 13);
            this.lblSMSCode.TabIndex = 0;
            this.lblSMSCode.Text = "SMS Code";
            // 
            // tbPgMobModem
            // 
            this.tbPgMobModem.BackColor = System.Drawing.Color.Transparent;
            this.tbPgMobModem.Controls.Add(this.txtSMSSimNo);
            this.tbPgMobModem.Controls.Add(this.lblSMSSimNo);
            this.tbPgMobModem.Controls.Add(this.txtTimeOut);
            this.tbPgMobModem.Controls.Add(this.lblTimeOut);
            this.tbPgMobModem.Controls.Add(this.txtBaudRate);
            this.tbPgMobModem.Controls.Add(this.lblBaudRate);
            this.tbPgMobModem.Controls.Add(this.txtPortName);
            this.tbPgMobModem.Controls.Add(this.lblPortName);
            this.tbPgMobModem.Location = new System.Drawing.Point(4, 22);
            this.tbPgMobModem.Name = "tbPgMobModem";
            this.tbPgMobModem.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgMobModem.Size = new System.Drawing.Size(525, 325);
            this.tbPgMobModem.TabIndex = 1;
            this.tbPgMobModem.Text = "SMS setting Mobile/Modem";
            this.tbPgMobModem.UseVisualStyleBackColor = true;
            // 
            // txtSMSSimNo
            // 
            this.txtSMSSimNo.Location = new System.Drawing.Point(95, 107);
            this.txtSMSSimNo.Name = "txtSMSSimNo";
            this.txtSMSSimNo.Size = new System.Drawing.Size(112, 20);
            this.txtSMSSimNo.TabIndex = 7;
            // 
            // lblSMSSimNo
            // 
            this.lblSMSSimNo.AutoSize = true;
            this.lblSMSSimNo.Location = new System.Drawing.Point(17, 110);
            this.lblSMSSimNo.Name = "lblSMSSimNo";
            this.lblSMSSimNo.Size = new System.Drawing.Size(70, 13);
            this.lblSMSSimNo.TabIndex = 6;
            this.lblSMSSimNo.Text = "SMS Sim No.";
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.Location = new System.Drawing.Point(95, 81);
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Size = new System.Drawing.Size(112, 20);
            this.txtTimeOut.TabIndex = 5;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.AutoSize = true;
            this.lblTimeOut.Location = new System.Drawing.Point(17, 84);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(50, 13);
            this.lblTimeOut.TabIndex = 4;
            this.lblTimeOut.Text = "Time Out";
            // 
            // txtBaudRate
            // 
            this.txtBaudRate.Location = new System.Drawing.Point(95, 55);
            this.txtBaudRate.Name = "txtBaudRate";
            this.txtBaudRate.Size = new System.Drawing.Size(112, 20);
            this.txtBaudRate.TabIndex = 3;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(17, 58);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(58, 13);
            this.lblBaudRate.TabIndex = 2;
            this.lblBaudRate.Text = "Baud Rate";
            // 
            // txtPortName
            // 
            this.txtPortName.Location = new System.Drawing.Point(95, 29);
            this.txtPortName.Name = "txtPortName";
            this.txtPortName.Size = new System.Drawing.Size(112, 20);
            this.txtPortName.TabIndex = 1;
            // 
            // lblPortName
            // 
            this.lblPortName.AutoSize = true;
            this.lblPortName.Location = new System.Drawing.Point(17, 32);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(57, 13);
            this.lblPortName.TabIndex = 0;
            this.lblPortName.Text = "Port Name";
            // 
            // txtSMSGtWayNm
            // 
            this.txtSMSGtWayNm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSGtWayNm.Location = new System.Drawing.Point(104, 60);
            this.txtSMSGtWayNm.Name = "txtSMSGtWayNm";
            this.txtSMSGtWayNm.Size = new System.Drawing.Size(143, 20);
            this.txtSMSGtWayNm.TabIndex = 3;
            // 
            // lblSMSGtWayNm
            // 
            this.lblSMSGtWayNm.Location = new System.Drawing.Point(25, 57);
            this.lblSMSGtWayNm.Name = "lblSMSGtWayNm";
            this.lblSMSGtWayNm.Size = new System.Drawing.Size(73, 27);
            this.lblSMSGtWayNm.TabIndex = 2;
            this.lblSMSGtWayNm.Text = "SMS Setting Name";
            // 
            // grpSMSVia
            // 
            this.grpSMSVia.Controls.Add(this.rbtnMobModem);
            this.grpSMSVia.Controls.Add(this.rbtnSMSGateway);
            this.grpSMSVia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpSMSVia.Location = new System.Drawing.Point(104, 82);
            this.grpSMSVia.Name = "grpSMSVia";
            this.grpSMSVia.Size = new System.Drawing.Size(232, 38);
            this.grpSMSVia.TabIndex = 5;
            this.grpSMSVia.TabStop = false;
            // 
            // rbtnMobModem
            // 
            this.rbtnMobModem.AutoSize = true;
            this.rbtnMobModem.Location = new System.Drawing.Point(120, 13);
            this.rbtnMobModem.Name = "rbtnMobModem";
            this.rbtnMobModem.Size = new System.Drawing.Size(96, 17);
            this.rbtnMobModem.TabIndex = 1;
            this.rbtnMobModem.TabStop = true;
            this.rbtnMobModem.Text = "Mobile/Modem";
            this.rbtnMobModem.UseVisualStyleBackColor = true;
            this.rbtnMobModem.CheckedChanged += new System.EventHandler(this.rbtnMobModem_CheckedChanged);
            // 
            // rbtnSMSGateway
            // 
            this.rbtnSMSGateway.AutoSize = true;
            this.rbtnSMSGateway.Location = new System.Drawing.Point(13, 13);
            this.rbtnSMSGateway.Name = "rbtnSMSGateway";
            this.rbtnSMSGateway.Size = new System.Drawing.Size(93, 17);
            this.rbtnSMSGateway.TabIndex = 0;
            this.rbtnSMSGateway.TabStop = true;
            this.rbtnSMSGateway.Text = "SMS Gateway";
            this.rbtnSMSGateway.UseVisualStyleBackColor = true;
            this.rbtnSMSGateway.CheckedChanged += new System.EventHandler(this.rbtnSMSGateway_CheckedChanged);
            // 
            // txtSMSSetID
            // 
            this.txtSMSSetID.BackColor = System.Drawing.SystemColors.Control;
            this.txtSMSSetID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSSetID.Enabled = false;
            this.txtSMSSetID.Location = new System.Drawing.Point(104, 36);
            this.txtSMSSetID.Name = "txtSMSSetID";
            this.txtSMSSetID.Size = new System.Drawing.Size(143, 20);
            this.txtSMSSetID.TabIndex = 1;
            // 
            // lblSMSSetID
            // 
            this.lblSMSSetID.AutoSize = true;
            this.lblSMSSetID.Location = new System.Drawing.Point(25, 38);
            this.lblSMSSetID.Name = "lblSMSSetID";
            this.lblSMSSetID.Size = new System.Drawing.Size(78, 13);
            this.lblSMSSetID.TabIndex = 0;
            this.lblSMSSetID.Text = "SMS Setting Id";
            // 
            // lblSMSVia
            // 
            this.lblSMSVia.AutoSize = true;
            this.lblSMSVia.Location = new System.Drawing.Point(25, 98);
            this.lblSMSVia.Name = "lblSMSVia";
            this.lblSMSVia.Size = new System.Drawing.Size(48, 13);
            this.lblSMSVia.TabIndex = 4;
            this.lblSMSVia.Text = "SMS Via";
            // 
            // toolbar
            // 
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnBack,
            this.btnForward,
            this.btnLast,
            this.toolStripSeparator1,
            this.btnEmail,
            this.btnLocate,
            this.toolStripSeparator2,
            this.btnNew,
            this.btnSave,
            this.btnEdit,
            this.btnCancel,
            this.btnDelete,
            this.toolStripSeparator3,
            this.btnPreview,
            this.btnPrint,
            this.btnExportPdf,
            this.toolStripSeparator4,
            this.btnLogout,
            this.toolStripSeparator6,
            this.btnHelp,
            this.btnCalculator,
            this.toolStripSeparator7,
            this.btnExit});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(544, 25);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            // 
            // btnFirst
            // 
            this.btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(23, 22);
            this.btnFirst.Text = "toolStripButton1";
            this.btnFirst.ToolTipText = "First";
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnBack
            // 
            this.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(23, 22);
            this.btnBack.Text = "toolStripButton2";
            this.btnBack.ToolTipText = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnForward
            // 
            this.btnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(23, 22);
            this.btnForward.Text = "toolStripButton3";
            this.btnForward.ToolTipText = "Forward";
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnLast
            // 
            this.btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(23, 22);
            this.btnLast.Text = "toolStripButton4";
            this.btnLast.ToolTipText = "Last";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEmail
            // 
            this.btnEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnEmail.Image")));
            this.btnEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(23, 22);
            this.btnEmail.Text = "toolStripButton5";
            this.btnEmail.ToolTipText = "Email(Ctrl+F)";
            // 
            // btnLocate
            // 
            this.btnLocate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocate.Image = ((System.Drawing.Image)(resources.GetObject("btnLocate.Image")));
            this.btnLocate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(23, 22);
            this.btnLocate.Text = "toolStripButton6";
            this.btnLocate.ToolTipText = "Locate";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.Text = "toolStripButton7";
            this.btnNew.ToolTipText = "New(Ctrl+N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "toolStripButton1";
            this.btnSave.ToolTipText = "Save(Ctrl+S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(23, 22);
            this.btnEdit.Text = "toolStripButton8";
            this.btnEdit.ToolTipText = "Edit(Ctrl+E)";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(23, 22);
            this.btnCancel.Text = "toolStripButton9";
            this.btnCancel.ToolTipText = "Cancel(Ctrl+Z)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "toolStripButton10";
            this.btnDelete.ToolTipText = "Delete(Ctrl+D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPreview
            // 
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(23, 22);
            this.btnPreview.Text = "toolStripButton11";
            this.btnPreview.ToolTipText = "Preview(Ctrl+O)";
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 22);
            this.btnPrint.Text = "Print";
            this.btnPrint.ToolTipText = "Print(Ctrl+P)";
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExportPdf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(23, 22);
            this.btnExportPdf.Text = "toolStripButton1";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLogout
            // 
            this.btnLogout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(23, 22);
            this.btnLogout.Text = "toolStripButton14";
            this.btnLogout.ToolTipText = "Exit(Ctrl+F4)";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "toolStripButton16";
            this.btnHelp.ToolTipText = "Help";
            // 
            // btnCalculator
            // 
            this.btnCalculator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCalculator.Image = ((System.Drawing.Image)(resources.GetObject("btnCalculator.Image")));
            this.btnCalculator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalculator.Name = "btnCalculator";
            this.btnCalculator.Size = new System.Drawing.Size(23, 22);
            this.btnCalculator.Text = "toolStripButton17";
            this.btnCalculator.ToolTipText = "Calculator";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExit
            // 
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(23, 22);
            this.btnExit.Text = "toolStripButton15";
            this.btnExit.ToolTipText = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmSMSSettingMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 484);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.pnlOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSMSSettingMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMS Setting Master";
            this.Load += new System.EventHandler(this.frmSMSSettingMaster_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSMSSettingMaster_FormClosed);
            this.pnlOuter.ResumeLayout(false);
            this.pnlOuter.PerformLayout();
            this.tbCntrl.ResumeLayout(false);
            this.tbPgSMSGtWay.ResumeLayout(false);
            this.tbPgSMSGtWay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvURLParam)).EndInit();
            this.tbPgMobModem.ResumeLayout(false);
            this.tbPgMobModem.PerformLayout();
            this.grpSMSVia.ResumeLayout(false);
            this.grpSMSVia.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripButton btnFirst;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.ToolStripButton btnForward;
        private System.Windows.Forms.ToolStripButton btnLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnEmail;
        private System.Windows.Forms.ToolStripButton btnLocate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripButton btnExportPdf;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnCalculator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.TextBox txtSMSSetID;
        private System.Windows.Forms.Label lblSMSSetID;
        private System.Windows.Forms.GroupBox grpSMSVia;
        private System.Windows.Forms.RadioButton rbtnMobModem;
        private System.Windows.Forms.RadioButton rbtnSMSGateway;
        private System.Windows.Forms.Label lblSMSVia;
        private System.Windows.Forms.TextBox txtSMSGtWayNm;
        private System.Windows.Forms.Label lblSMSGtWayNm;
        private System.Windows.Forms.TabControl tbCntrl;
        private System.Windows.Forms.TabPage tbPgSMSGtWay;
        private System.Windows.Forms.TabPage tbPgMobModem;
        private System.Windows.Forms.DataGridView dgvURLParam;
        private System.Windows.Forms.TextBox txtSMSURL;
        private System.Windows.Forms.Label lblSMSURL;
        private System.Windows.Forms.TextBox txtSMSGtWayId;
        private System.Windows.Forms.Label lblSMSGtWayId;
        private System.Windows.Forms.TextBox txtSMSCode;
        private System.Windows.Forms.Label lblSMSCode;
        private System.Windows.Forms.Button btnSMSCode;
        private System.Windows.Forms.TextBox txtPortName;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.TextBox txtSMSSimNo;
        private System.Windows.Forms.Label lblSMSSimNo;
        private System.Windows.Forms.TextBox txtTimeOut;
        private System.Windows.Forms.Label lblTimeOut;
        private System.Windows.Forms.TextBox txtBaudRate;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamDesc;
        private System.Windows.Forms.DataGridViewButtonColumn colParamVal;
    }
}