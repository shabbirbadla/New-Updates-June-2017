namespace U2TPlus
{
    partial class U2TPlusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(U2TPlusForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnLastRecord = new System.Windows.Forms.Button();
            this.btnFirstRecord = new System.Windows.Forms.Button();
            this.cmbTotalRecords = new System.Windows.Forms.ComboBox();
            this.txtCompanyConfigID = new System.Windows.Forms.TextBox();
            this.txtCompanyDataBaseName = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.txtSelectCompany = new System.Windows.Forms.TextBox();
            this.txtFinancialYear = new System.Windows.Forms.TextBox();
            this.txtVersionOftheTally = new System.Windows.Forms.TextBox();
            this.txtVisualUdyogPath = new System.Windows.Forms.TextBox();
            this.txtXMLPath = new System.Windows.Forms.TextBox();
            this.lblSelectCompany = new System.Windows.Forms.Label();
            this.lblFinancialYear = new System.Windows.Forms.Label();
            this.lblVersionOftheTally = new System.Windows.Forms.Label();
            this.lblVisualUdyogPath = new System.Windows.Forms.Label();
            this.lblXMLPath = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.generateToolStripMenuItem,
            this.dataMappingToolStripMenuItem,
            this.changePasswordToolStripMenuItem,
            this.logToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(928, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.settingsToolStripMenuItem.Text = "S&ettings";
            // 
            // generateToolStripMenuItem
            // 
            this.generateToolStripMenuItem.Name = "generateToolStripMenuItem";
            this.generateToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.generateToolStripMenuItem.Text = "&Generate";
            // 
            // dataMappingToolStripMenuItem
            // 
            this.dataMappingToolStripMenuItem.Name = "dataMappingToolStripMenuItem";
            this.dataMappingToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.dataMappingToolStripMenuItem.Text = "&Data Mapping";
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
            this.changePasswordToolStripMenuItem.Text = "Change &Password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.logToolStripMenuItem.Text = "&Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnLastRecord);
            this.groupBox1.Controls.Add(this.btnFirstRecord);
            this.groupBox1.Controls.Add(this.cmbTotalRecords);
            this.groupBox1.Controls.Add(this.txtCompanyConfigID);
            this.groupBox1.Controls.Add(this.txtCompanyDataBaseName);
            this.groupBox1.Controls.Add(this.btnNext);
            this.groupBox1.Controls.Add(this.btnPrevious);
            this.groupBox1.Controls.Add(this.txtSelectCompany);
            this.groupBox1.Controls.Add(this.txtFinancialYear);
            this.groupBox1.Controls.Add(this.txtVersionOftheTally);
            this.groupBox1.Controls.Add(this.txtVisualUdyogPath);
            this.groupBox1.Controls.Add(this.txtXMLPath);
            this.groupBox1.Controls.Add(this.lblSelectCompany);
            this.groupBox1.Controls.Add(this.lblFinancialYear);
            this.groupBox1.Controls.Add(this.lblVersionOftheTally);
            this.groupBox1.Controls.Add(this.lblVisualUdyogPath);
            this.groupBox1.Controls.Add(this.lblXMLPath);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(630, 277);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Environment";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Location = new System.Drawing.Point(115, 207);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Location = new System.Drawing.Point(197, 207);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 12;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Location = new System.Drawing.Point(278, 207);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnLastRecord
            // 
            this.btnLastRecord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLastRecord.Location = new System.Drawing.Point(578, 28);
            this.btnLastRecord.Name = "btnLastRecord";
            this.btnLastRecord.Size = new System.Drawing.Size(36, 23);
            this.btnLastRecord.TabIndex = 5;
            this.btnLastRecord.Text = ">>";
            this.btnLastRecord.UseVisualStyleBackColor = true;
            this.btnLastRecord.Click += new System.EventHandler(this.btnLastRecord_Click);
            // 
            // btnFirstRecord
            // 
            this.btnFirstRecord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFirstRecord.Location = new System.Drawing.Point(112, 28);
            this.btnFirstRecord.Name = "btnFirstRecord";
            this.btnFirstRecord.Size = new System.Drawing.Size(36, 23);
            this.btnFirstRecord.TabIndex = 1;
            this.btnFirstRecord.Text = "<<";
            this.btnFirstRecord.UseVisualStyleBackColor = true;
            this.btnFirstRecord.Click += new System.EventHandler(this.btnFirstRecord_Click);
            // 
            // cmbTotalRecords
            // 
            this.cmbTotalRecords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTotalRecords.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbTotalRecords.FormattingEnabled = true;
            this.cmbTotalRecords.Location = new System.Drawing.Point(184, 29);
            this.cmbTotalRecords.Name = "cmbTotalRecords";
            this.cmbTotalRecords.Size = new System.Drawing.Size(358, 21);
            this.cmbTotalRecords.TabIndex = 3;
            this.cmbTotalRecords.SelectedIndexChanged += new System.EventHandler(this.cmbTotalRecords_SelectedIndexChanged);
            // 
            // txtCompanyConfigID
            // 
            this.txtCompanyConfigID.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtCompanyConfigID.Location = new System.Drawing.Point(314, 208);
            this.txtCompanyConfigID.Name = "txtCompanyConfigID";
            this.txtCompanyConfigID.ReadOnly = true;
            this.txtCompanyConfigID.Size = new System.Drawing.Size(154, 21);
            this.txtCompanyConfigID.TabIndex = 0;
            this.txtCompanyConfigID.Visible = false;
            // 
            // txtCompanyDataBaseName
            // 
            this.txtCompanyDataBaseName.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtCompanyDataBaseName.Location = new System.Drawing.Point(131, 208);
            this.txtCompanyDataBaseName.Name = "txtCompanyDataBaseName";
            this.txtCompanyDataBaseName.ReadOnly = true;
            this.txtCompanyDataBaseName.Size = new System.Drawing.Size(154, 21);
            this.txtCompanyDataBaseName.TabIndex = 0;
            this.txtCompanyDataBaseName.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNext.Location = new System.Drawing.Point(543, 28);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(36, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrevious.Location = new System.Drawing.Point(147, 28);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(36, 23);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // txtSelectCompany
            // 
            this.txtSelectCompany.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtSelectCompany.Location = new System.Drawing.Point(115, 177);
            this.txtSelectCompany.Name = "txtSelectCompany";
            this.txtSelectCompany.ReadOnly = true;
            this.txtSelectCompany.Size = new System.Drawing.Size(497, 21);
            this.txtSelectCompany.TabIndex = 10;
            // 
            // txtFinancialYear
            // 
            this.txtFinancialYear.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtFinancialYear.Location = new System.Drawing.Point(115, 148);
            this.txtFinancialYear.Name = "txtFinancialYear";
            this.txtFinancialYear.ReadOnly = true;
            this.txtFinancialYear.Size = new System.Drawing.Size(497, 21);
            this.txtFinancialYear.TabIndex = 9;
            // 
            // txtVersionOftheTally
            // 
            this.txtVersionOftheTally.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtVersionOftheTally.Location = new System.Drawing.Point(115, 119);
            this.txtVersionOftheTally.Name = "txtVersionOftheTally";
            this.txtVersionOftheTally.ReadOnly = true;
            this.txtVersionOftheTally.Size = new System.Drawing.Size(497, 21);
            this.txtVersionOftheTally.TabIndex = 8;
            // 
            // txtVisualUdyogPath
            // 
            this.txtVisualUdyogPath.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtVisualUdyogPath.Location = new System.Drawing.Point(115, 90);
            this.txtVisualUdyogPath.Name = "txtVisualUdyogPath";
            this.txtVisualUdyogPath.ReadOnly = true;
            this.txtVisualUdyogPath.Size = new System.Drawing.Size(497, 21);
            this.txtVisualUdyogPath.TabIndex = 7;
            // 
            // txtXMLPath
            // 
            this.txtXMLPath.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtXMLPath.Location = new System.Drawing.Point(115, 62);
            this.txtXMLPath.Name = "txtXMLPath";
            this.txtXMLPath.ReadOnly = true;
            this.txtXMLPath.Size = new System.Drawing.Size(497, 21);
            this.txtXMLPath.TabIndex = 6;
            // 
            // lblSelectCompany
            // 
            this.lblSelectCompany.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblSelectCompany.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectCompany.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblSelectCompany.Location = new System.Drawing.Point(9, 176);
            this.lblSelectCompany.Name = "lblSelectCompany";
            this.lblSelectCompany.Size = new System.Drawing.Size(116, 23);
            this.lblSelectCompany.TabIndex = 0;
            this.lblSelectCompany.Text = "Company";
            this.lblSelectCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFinancialYear
            // 
            this.lblFinancialYear.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblFinancialYear.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinancialYear.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblFinancialYear.Location = new System.Drawing.Point(9, 147);
            this.lblFinancialYear.Name = "lblFinancialYear";
            this.lblFinancialYear.Size = new System.Drawing.Size(100, 23);
            this.lblFinancialYear.TabIndex = 0;
            this.lblFinancialYear.Text = "Financial Year";
            this.lblFinancialYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersionOftheTally
            // 
            this.lblVersionOftheTally.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblVersionOftheTally.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionOftheTally.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblVersionOftheTally.Location = new System.Drawing.Point(9, 118);
            this.lblVersionOftheTally.Name = "lblVersionOftheTally";
            this.lblVersionOftheTally.Size = new System.Drawing.Size(149, 23);
            this.lblVersionOftheTally.TabIndex = 0;
            this.lblVersionOftheTally.Text = "Tally Version";
            this.lblVersionOftheTally.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVisualUdyogPath
            // 
            this.lblVisualUdyogPath.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblVisualUdyogPath.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisualUdyogPath.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblVisualUdyogPath.Location = new System.Drawing.Point(9, 89);
            this.lblVisualUdyogPath.Name = "lblVisualUdyogPath";
            this.lblVisualUdyogPath.Size = new System.Drawing.Size(116, 23);
            this.lblVisualUdyogPath.TabIndex = 0;
            this.lblVisualUdyogPath.Text = "Udyog Path";
            this.lblVisualUdyogPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblXMLPath
            // 
            this.lblXMLPath.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblXMLPath.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXMLPath.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblXMLPath.Location = new System.Drawing.Point(9, 61);
            this.lblXMLPath.Name = "lblXMLPath";
            this.lblXMLPath.Size = new System.Drawing.Size(100, 23);
            this.lblXMLPath.TabIndex = 0;
            this.lblXMLPath.Text = "XML Path";
            this.lblXMLPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(629, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 478);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "About U2T Plus";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.groupBox3.Controls.Add(this.pictureBox1);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(2, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(627, 203);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(621, 182);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // U2TPlusForm
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(928, 502);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "U2TPlusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "U2T Plus";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.U2TPlusForm_Load);
            this.Activated += new System.EventHandler(this.U2TPlusForm_Activated);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtCompanyConfigID;
        private System.Windows.Forms.TextBox txtCompanyDataBaseName;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.TextBox txtSelectCompany;
        private System.Windows.Forms.TextBox txtFinancialYear;
        private System.Windows.Forms.TextBox txtVersionOftheTally;
        private System.Windows.Forms.TextBox txtVisualUdyogPath;
        private System.Windows.Forms.TextBox txtXMLPath;
        private System.Windows.Forms.Label lblSelectCompany;
        private System.Windows.Forms.Label lblFinancialYear;
        private System.Windows.Forms.Label lblVersionOftheTally;
        private System.Windows.Forms.Label lblVisualUdyogPath;
        private System.Windows.Forms.Label lblXMLPath;
        private System.Windows.Forms.ComboBox cmbTotalRecords;
        private System.Windows.Forms.Button btnLastRecord;
        private System.Windows.Forms.Button btnFirstRecord;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ToolStripMenuItem dataMappingToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}