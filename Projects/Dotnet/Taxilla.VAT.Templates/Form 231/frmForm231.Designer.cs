namespace Form_231
{
    partial class frmForm231
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmForm231));
            this.grpDealerDtls = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSelect = new System.Windows.Forms.ComboBox();
            this.cboPOR = new System.Windows.Forms.ComboBox();
            this.lblLastReturn = new System.Windows.Forms.Label();
            this.lblPOR = new System.Windows.Forms.Label();
            this.chkLastReturnNo = new System.Windows.Forms.CheckBox();
            this.chkLastReturnYes = new System.Windows.Forms.CheckBox();
            this.lblFirstReturn = new System.Windows.Forms.Label();
            this.cboReturnType = new System.Windows.Forms.ComboBox();
            this.chkFirstReturnNo = new System.Windows.Forms.CheckBox();
            this.chkFirstReturnYes = new System.Windows.Forms.CheckBox();
            this.lblReturnType = new System.Windows.Forms.Label();
            this.chkEligibleForNo = new System.Windows.Forms.CheckBox();
            this.chkEligibleForYes = new System.Windows.Forms.CheckBox();
            this.lblEligibleFor = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsprogressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.grpDealerDtls.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpDealerDtls
            // 
            this.grpDealerDtls.Controls.Add(this.label5);
            this.grpDealerDtls.Controls.Add(this.label4);
            this.grpDealerDtls.Controls.Add(this.label3);
            this.grpDealerDtls.Controls.Add(this.label2);
            this.grpDealerDtls.Controls.Add(this.label1);
            this.grpDealerDtls.Controls.Add(this.cboSelect);
            this.grpDealerDtls.Controls.Add(this.cboPOR);
            this.grpDealerDtls.Controls.Add(this.lblLastReturn);
            this.grpDealerDtls.Controls.Add(this.lblPOR);
            this.grpDealerDtls.Controls.Add(this.chkLastReturnNo);
            this.grpDealerDtls.Controls.Add(this.chkLastReturnYes);
            this.grpDealerDtls.Controls.Add(this.lblFirstReturn);
            this.grpDealerDtls.Controls.Add(this.cboReturnType);
            this.grpDealerDtls.Controls.Add(this.chkFirstReturnNo);
            this.grpDealerDtls.Controls.Add(this.chkFirstReturnYes);
            this.grpDealerDtls.Controls.Add(this.lblReturnType);
            this.grpDealerDtls.Controls.Add(this.chkEligibleForNo);
            this.grpDealerDtls.Controls.Add(this.chkEligibleForYes);
            this.grpDealerDtls.Controls.Add(this.lblEligibleFor);
            this.grpDealerDtls.Location = new System.Drawing.Point(5, 2);
            this.grpDealerDtls.Name = "grpDealerDtls";
            this.grpDealerDtls.Size = new System.Drawing.Size(399, 187);
            this.grpDealerDtls.TabIndex = 0;
            this.grpDealerDtls.TabStop = false;
            this.grpDealerDtls.Text = "&Dealer Details";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(81, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 15);
            this.label5.TabIndex = 18;
            this.label5.Text = "*";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(114, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "*";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(79, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "*";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(89, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(106, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboSelect
            // 
            this.cboSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboSelect.FormattingEnabled = true;
            this.cboSelect.Location = new System.Drawing.Point(142, 134);
            this.cboSelect.Name = "cboSelect";
            this.cboSelect.Size = new System.Drawing.Size(244, 21);
            this.cboSelect.TabIndex = 13;
            this.cboSelect.SelectedIndexChanged += new System.EventHandler(this.cboSelect_SelectedIndexChanged);
            // 
            // cboPOR
            // 
            this.cboPOR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPOR.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboPOR.FormattingEnabled = true;
            this.cboPOR.Items.AddRange(new object[] {
            "Monthly",
            "Quarterly",
            "Six-monthly",
            "Annual"});
            this.cboPOR.Location = new System.Drawing.Point(142, 105);
            this.cboPOR.Name = "cboPOR";
            this.cboPOR.Size = new System.Drawing.Size(244, 21);
            this.cboPOR.TabIndex = 12;
            this.cboPOR.SelectedIndexChanged += new System.EventHandler(this.cboPOR_SelectedIndexChanged);
            // 
            // lblLastReturn
            // 
            this.lblLastReturn.AutoSize = true;
            this.lblLastReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastReturn.Location = new System.Drawing.Point(15, 165);
            this.lblLastReturn.Name = "lblLastReturn";
            this.lblLastReturn.Size = new System.Drawing.Size(67, 13);
            this.lblLastReturn.TabIndex = 11;
            this.lblLastReturn.Text = "&Last Return?";
            // 
            // lblPOR
            // 
            this.lblPOR.AutoSize = true;
            this.lblPOR.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPOR.Location = new System.Drawing.Point(15, 109);
            this.lblPOR.Name = "lblPOR";
            this.lblPOR.Size = new System.Drawing.Size(100, 13);
            this.lblPOR.TabIndex = 10;
            this.lblPOR.Text = "&Periodicity of Return";
            // 
            // chkLastReturnNo
            // 
            this.chkLastReturnNo.AutoSize = true;
            this.chkLastReturnNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkLastReturnNo.Location = new System.Drawing.Point(220, 163);
            this.chkLastReturnNo.Name = "chkLastReturnNo";
            this.chkLastReturnNo.Size = new System.Drawing.Size(38, 17);
            this.chkLastReturnNo.TabIndex = 9;
            this.chkLastReturnNo.Tag = "IndProp";
            this.chkLastReturnNo.Text = "&No";
            this.chkLastReturnNo.UseVisualStyleBackColor = true;
            this.chkLastReturnNo.CheckedChanged += new System.EventHandler(this.chkLastReturnYes_CheckedChanged);
            // 
            // chkLastReturnYes
            // 
            this.chkLastReturnYes.AutoSize = true;
            this.chkLastReturnYes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkLastReturnYes.Location = new System.Drawing.Point(142, 163);
            this.chkLastReturnYes.Name = "chkLastReturnYes";
            this.chkLastReturnYes.Size = new System.Drawing.Size(42, 17);
            this.chkLastReturnYes.TabIndex = 8;
            this.chkLastReturnYes.Tag = "IndProp";
            this.chkLastReturnYes.Text = "&Yes";
            this.chkLastReturnYes.UseVisualStyleBackColor = true;
            this.chkLastReturnYes.CheckedChanged += new System.EventHandler(this.chkLastReturnYes_CheckedChanged);
            // 
            // lblFirstReturn
            // 
            this.lblFirstReturn.AutoSize = true;
            this.lblFirstReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstReturn.Location = new System.Drawing.Point(15, 82);
            this.lblFirstReturn.Name = "lblFirstReturn";
            this.lblFirstReturn.Size = new System.Drawing.Size(66, 13);
            this.lblFirstReturn.TabIndex = 7;
            this.lblFirstReturn.Text = "&First Return?";
            // 
            // cboReturnType
            // 
            this.cboReturnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReturnType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboReturnType.FormattingEnabled = true;
            this.cboReturnType.Items.AddRange(new object[] {
            "Original",
            "Fresh",
            "Revised_under_s_20(4)(a)",
            "Revised_under_s_20(4)(b)",
            "Revised_under_s_20(4)(c)"});
            this.cboReturnType.Location = new System.Drawing.Point(142, 51);
            this.cboReturnType.Name = "cboReturnType";
            this.cboReturnType.Size = new System.Drawing.Size(244, 21);
            this.cboReturnType.TabIndex = 6;
            // 
            // chkFirstReturnNo
            // 
            this.chkFirstReturnNo.AutoSize = true;
            this.chkFirstReturnNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkFirstReturnNo.Location = new System.Drawing.Point(220, 80);
            this.chkFirstReturnNo.Name = "chkFirstReturnNo";
            this.chkFirstReturnNo.Size = new System.Drawing.Size(38, 17);
            this.chkFirstReturnNo.TabIndex = 5;
            this.chkFirstReturnNo.Tag = "IndProp";
            this.chkFirstReturnNo.Text = "&No";
            this.chkFirstReturnNo.UseVisualStyleBackColor = true;
            this.chkFirstReturnNo.CheckedChanged += new System.EventHandler(this.chkFirstReturnYes_CheckedChanged);
            // 
            // chkFirstReturnYes
            // 
            this.chkFirstReturnYes.AutoSize = true;
            this.chkFirstReturnYes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkFirstReturnYes.Location = new System.Drawing.Point(142, 80);
            this.chkFirstReturnYes.Name = "chkFirstReturnYes";
            this.chkFirstReturnYes.Size = new System.Drawing.Size(42, 17);
            this.chkFirstReturnYes.TabIndex = 4;
            this.chkFirstReturnYes.Tag = "IndProp";
            this.chkFirstReturnYes.Text = "&Yes";
            this.chkFirstReturnYes.UseVisualStyleBackColor = true;
            this.chkFirstReturnYes.CheckedChanged += new System.EventHandler(this.chkFirstReturnYes_CheckedChanged);
            // 
            // lblReturnType
            // 
            this.lblReturnType.AutoSize = true;
            this.lblReturnType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReturnType.Location = new System.Drawing.Point(15, 55);
            this.lblReturnType.Name = "lblReturnType";
            this.lblReturnType.Size = new System.Drawing.Size(75, 13);
            this.lblReturnType.TabIndex = 3;
            this.lblReturnType.Text = "&Type of Return";
            // 
            // chkEligibleForNo
            // 
            this.chkEligibleForNo.AutoSize = true;
            this.chkEligibleForNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkEligibleForNo.Location = new System.Drawing.Point(220, 26);
            this.chkEligibleForNo.Name = "chkEligibleForNo";
            this.chkEligibleForNo.Size = new System.Drawing.Size(38, 17);
            this.chkEligibleForNo.TabIndex = 2;
            this.chkEligibleForNo.Tag = "IndProp";
            this.chkEligibleForNo.Text = "&No";
            this.chkEligibleForNo.UseVisualStyleBackColor = true;
            this.chkEligibleForNo.CheckedChanged += new System.EventHandler(this.chkEligibleForYes_CheckedChanged);
            // 
            // chkEligibleForYes
            // 
            this.chkEligibleForYes.AutoSize = true;
            this.chkEligibleForYes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkEligibleForYes.Location = new System.Drawing.Point(142, 26);
            this.chkEligibleForYes.Name = "chkEligibleForYes";
            this.chkEligibleForYes.Size = new System.Drawing.Size(42, 17);
            this.chkEligibleForYes.TabIndex = 1;
            this.chkEligibleForYes.Tag = "IndProp";
            this.chkEligibleForYes.Text = "&Yes";
            this.chkEligibleForYes.UseVisualStyleBackColor = true;
            this.chkEligibleForYes.CheckedChanged += new System.EventHandler(this.chkEligibleForYes_CheckedChanged);
            // 
            // lblEligibleFor
            // 
            this.lblEligibleFor.AutoSize = true;
            this.lblEligibleFor.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEligibleFor.Location = new System.Drawing.Point(15, 28);
            this.lblEligibleFor.Name = "lblEligibleFor";
            this.lblEligibleFor.Size = new System.Drawing.Size(93, 13);
            this.lblEligibleFor.TabIndex = 0;
            this.lblEligibleFor.Text = "Is &Eligible for 704?";
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(323, 223);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 29);
            this.btnClose.TabIndex = 16;
            this.toolTip1.SetToolTip(this.btnClose, "Click to Close (Alt+C)");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(230, 223);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(81, 29);
            this.btnExport.TabIndex = 15;
            this.btnExport.Text = "&Generate";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolTip1.SetToolTip(this.btnExport, "Click to Generate XML (Alt+G)");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLabelStatus,
            this.tsprogressbar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 262);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(410, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLabelStatus
            // 
            this.tsLabelStatus.Name = "tsLabelStatus";
            this.tsLabelStatus.Size = new System.Drawing.Size(395, 17);
            this.tsLabelStatus.Spring = true;
            this.tsLabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsprogressbar
            // 
            this.tsprogressbar.Name = "tsprogressbar";
            this.tsprogressbar.Size = new System.Drawing.Size(100, 16);
            this.tsprogressbar.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtOutputPath);
            this.panel1.Controls.Add(this.lblOutputPath);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.grpDealerDtls);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 284);
            this.panel1.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(86, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 15);
            this.label7.TabIndex = 20;
            this.label7.Text = "*";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(366, 193);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(28, 24);
            this.btnBrowse.TabIndex = 19;
            this.toolTip1.SetToolTip(this.btnBrowse, "Click to browse");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.BackColor = System.Drawing.Color.White;
            this.txtOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputPath.Location = new System.Drawing.Point(142, 195);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(220, 20);
            this.txtOutputPath.TabIndex = 18;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputPath.Location = new System.Drawing.Point(15, 199);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(71, 13);
            this.lblOutputPath.TabIndex = 17;
            this.lblOutputPath.Text = "Output &Folder";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            // 
            // timer
            // 
            this.timer.Interval = 7000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmForm231
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 284);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmForm231";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Maharashtra VAT-FORM 231";
            this.Load += new System.EventHandler(this.frmForm231_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmForm231_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmForm231_KeyDown);
            this.grpDealerDtls.ResumeLayout(false);
            this.grpDealerDtls.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDealerDtls;
        private System.Windows.Forms.ComboBox cboReturnType;
        private System.Windows.Forms.CheckBox chkFirstReturnNo;
        private System.Windows.Forms.CheckBox chkFirstReturnYes;
        private System.Windows.Forms.Label lblReturnType;
        private System.Windows.Forms.Label lblPOR;
        private System.Windows.Forms.CheckBox chkLastReturnNo;
        private System.Windows.Forms.CheckBox chkLastReturnYes;
        private System.Windows.Forms.Label lblFirstReturn;
        private System.Windows.Forms.Label lblLastReturn;
        private System.Windows.Forms.ComboBox cboPOR;
        private System.Windows.Forms.CheckBox chkEligibleForNo;
        private System.Windows.Forms.CheckBox chkEligibleForYes;
        private System.Windows.Forms.Label lblEligibleFor;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsLabelStatus;
        private System.Windows.Forms.ToolStripProgressBar tsprogressbar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ComboBox cboSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
    }
}

