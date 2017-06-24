namespace UeSMS
{
    partial class frmSMSParamMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSMSParamMaster));
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.btnDelParam = new System.Windows.Forms.Button();
            this.btnAddParam = new System.Windows.Forms.Button();
            this.dgvURLParam = new System.Windows.Forms.DataGridView();
            this.colParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParamEnc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colParamDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSMSURL = new System.Windows.Forms.TextBox();
            this.lblSMSURL = new System.Windows.Forms.Label();
            this.txtSMSGtWayId = new System.Windows.Forms.TextBox();
            this.lblSMSGtWayId = new System.Windows.Forms.Label();
            this.txtSMSCode = new System.Windows.Forms.TextBox();
            this.lblSMSCode = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvURLParam)).BeginInit();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOuter
            // 
            this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOuter.Controls.Add(this.btnDelParam);
            this.pnlOuter.Controls.Add(this.btnAddParam);
            this.pnlOuter.Controls.Add(this.dgvURLParam);
            this.pnlOuter.Controls.Add(this.txtSMSURL);
            this.pnlOuter.Controls.Add(this.lblSMSURL);
            this.pnlOuter.Controls.Add(this.txtSMSGtWayId);
            this.pnlOuter.Controls.Add(this.lblSMSGtWayId);
            this.pnlOuter.Controls.Add(this.txtSMSCode);
            this.pnlOuter.Controls.Add(this.lblSMSCode);
            this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuter.Location = new System.Drawing.Point(0, 0);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(544, 426);
            this.pnlOuter.TabIndex = 0;
            // 
            // btnDelParam
            // 
            this.btnDelParam.Location = new System.Drawing.Point(428, 115);
            this.btnDelParam.Name = "btnDelParam";
            this.btnDelParam.Size = new System.Drawing.Size(75, 23);
            this.btnDelParam.TabIndex = 7;
            this.btnDelParam.Text = "&Delete";
            this.btnDelParam.UseVisualStyleBackColor = true;
            this.btnDelParam.Click += new System.EventHandler(this.btnDelParam_Click);
            // 
            // btnAddParam
            // 
            this.btnAddParam.Location = new System.Drawing.Point(351, 115);
            this.btnAddParam.Name = "btnAddParam";
            this.btnAddParam.Size = new System.Drawing.Size(75, 23);
            this.btnAddParam.TabIndex = 6;
            this.btnAddParam.Text = "&Add";
            this.btnAddParam.UseVisualStyleBackColor = true;
            this.btnAddParam.Click += new System.EventHandler(this.btnAddParam_Click);
            // 
            // dgvURLParam
            // 
            this.dgvURLParam.AllowUserToAddRows = false;
            this.dgvURLParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvURLParam.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colParamName,
            this.colParamEnc,
            this.colParamDesc});
            this.dgvURLParam.Location = new System.Drawing.Point(6, 144);
            this.dgvURLParam.Name = "dgvURLParam";
            this.dgvURLParam.RowHeadersVisible = false;
            this.dgvURLParam.Size = new System.Drawing.Size(530, 273);
            this.dgvURLParam.TabIndex = 8;
            this.dgvURLParam.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvURLParam_CellContentClick);
            // 
            // colParamName
            // 
            this.colParamName.HeaderText = "Parameter Name(Case Sensitive)";
            this.colParamName.Name = "colParamName";
            this.colParamName.Width = 150;
            // 
            // colParamEnc
            // 
            this.colParamEnc.HeaderText = "Encrypt";
            this.colParamEnc.Name = "colParamEnc";
            this.colParamEnc.Width = 50;
            // 
            // colParamDesc
            // 
            this.colParamDesc.HeaderText = "Parameter Description";
            this.colParamDesc.Name = "colParamDesc";
            this.colParamDesc.Width = 300;
            // 
            // txtSMSURL
            // 
            this.txtSMSURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSURL.Location = new System.Drawing.Point(104, 84);
            this.txtSMSURL.Name = "txtSMSURL";
            this.txtSMSURL.Size = new System.Drawing.Size(398, 20);
            this.txtSMSURL.TabIndex = 5;
            // 
            // lblSMSURL
            // 
            this.lblSMSURL.AutoSize = true;
            this.lblSMSURL.Location = new System.Drawing.Point(25, 86);
            this.lblSMSURL.Name = "lblSMSURL";
            this.lblSMSURL.Size = new System.Drawing.Size(29, 13);
            this.lblSMSURL.TabIndex = 4;
            this.lblSMSURL.Text = "URL";
            // 
            // txtSMSGtWayId
            // 
            this.txtSMSGtWayId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSGtWayId.Location = new System.Drawing.Point(104, 58);
            this.txtSMSGtWayId.Name = "txtSMSGtWayId";
            this.txtSMSGtWayId.Size = new System.Drawing.Size(398, 20);
            this.txtSMSGtWayId.TabIndex = 3;
            // 
            // lblSMSGtWayId
            // 
            this.lblSMSGtWayId.AutoSize = true;
            this.lblSMSGtWayId.Location = new System.Drawing.Point(25, 60);
            this.lblSMSGtWayId.Name = "lblSMSGtWayId";
            this.lblSMSGtWayId.Size = new System.Drawing.Size(78, 13);
            this.lblSMSGtWayId.TabIndex = 2;
            this.lblSMSGtWayId.Text = "SMS Gateway ";
            // 
            // txtSMSCode
            // 
            this.txtSMSCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtSMSCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMSCode.Enabled = false;
            this.txtSMSCode.Location = new System.Drawing.Point(104, 32);
            this.txtSMSCode.Name = "txtSMSCode";
            this.txtSMSCode.Size = new System.Drawing.Size(143, 20);
            this.txtSMSCode.TabIndex = 1;
            // 
            // lblSMSCode
            // 
            this.lblSMSCode.AutoSize = true;
            this.lblSMSCode.Location = new System.Drawing.Point(25, 34);
            this.lblSMSCode.Name = "lblSMSCode";
            this.lblSMSCode.Size = new System.Drawing.Size(58, 13);
            this.lblSMSCode.TabIndex = 0;
            this.lblSMSCode.Text = "SMS Code";
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
            // frmSMSParamMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 426);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.pnlOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSMSParamMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMS Gateway Master";
            this.Load += new System.EventHandler(this.frmSMSParamMaster_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSMSParamMaster_FormClosed);
            this.pnlOuter.ResumeLayout(false);
            this.pnlOuter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvURLParam)).EndInit();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.TextBox txtSMSURL;
        private System.Windows.Forms.Label lblSMSURL;
        private System.Windows.Forms.TextBox txtSMSGtWayId;
        private System.Windows.Forms.Label lblSMSGtWayId;
        private System.Windows.Forms.TextBox txtSMSCode;
        private System.Windows.Forms.Label lblSMSCode;
        private System.Windows.Forms.DataGridView dgvURLParam;
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
        private System.Windows.Forms.Button btnDelParam;
        private System.Windows.Forms.Button btnAddParam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colParamEnc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamDesc;
    }
}