namespace U2TPlus
{
    partial class NewConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewConfigForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnVisualUdyogPath = new System.Windows.Forms.Button();
            this.btnXMLPAth = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbSelectCompany = new System.Windows.Forms.ComboBox();
            this.cmbFinancialYear = new System.Windows.Forms.ComboBox();
            this.cmbVersionOftheTally = new System.Windows.Forms.ComboBox();
            this.txtVisualUdyogPath = new System.Windows.Forms.TextBox();
            this.txtXMLPath = new System.Windows.Forms.TextBox();
            this.lblSelectCompany = new System.Windows.Forms.Label();
            this.lblFinancialYear = new System.Windows.Forms.Label();
            this.lblVersionOftheTally = new System.Windows.Forms.Label();
            this.lblVisualUdyogPath = new System.Windows.Forms.Label();
            this.lblXMLPath = new System.Windows.Forms.Label();
            this.txtCompanyDataBaseName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDisplayType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(319, 160);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnVisualUdyogPath
            // 
            this.btnVisualUdyogPath.Location = new System.Drawing.Point(489, 46);
            this.btnVisualUdyogPath.Name = "btnVisualUdyogPath";
            this.btnVisualUdyogPath.Size = new System.Drawing.Size(75, 23);
            this.btnVisualUdyogPath.TabIndex = 4;
            this.btnVisualUdyogPath.Text = "...";
            this.btnVisualUdyogPath.UseVisualStyleBackColor = true;
            this.btnVisualUdyogPath.Click += new System.EventHandler(this.btnVisualUdyogPath_Click);
            // 
            // btnXMLPAth
            // 
            this.btnXMLPAth.Location = new System.Drawing.Point(489, 18);
            this.btnXMLPAth.Name = "btnXMLPAth";
            this.btnXMLPAth.Size = new System.Drawing.Size(75, 23);
            this.btnXMLPAth.TabIndex = 2;
            this.btnXMLPAth.Text = "...";
            this.btnXMLPAth.UseVisualStyleBackColor = true;
            this.btnXMLPAth.Click += new System.EventHandler(this.btnXMLPAth_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(578, 199);
            this.panel1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnVisualUdyogPath);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnXMLPAth);
            this.groupBox2.Controls.Add(this.cmbSelectCompany);
            this.groupBox2.Controls.Add(this.cmbFinancialYear);
            this.groupBox2.Controls.Add(this.cmbVersionOftheTally);
            this.groupBox2.Controls.Add(this.txtVisualUdyogPath);
            this.groupBox2.Controls.Add(this.txtXMLPath);
            this.groupBox2.Controls.Add(this.lblSelectCompany);
            this.groupBox2.Controls.Add(this.lblFinancialYear);
            this.groupBox2.Controls.Add(this.lblVersionOftheTally);
            this.groupBox2.Controls.Add(this.lblVisualUdyogPath);
            this.groupBox2.Controls.Add(this.lblXMLPath);
            this.groupBox2.Controls.Add(this.txtCompanyDataBaseName);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(578, 199);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(408, 160);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbSelectCompany
            // 
            this.cmbSelectCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectCompany.Enabled = false;
            this.cmbSelectCompany.FormattingEnabled = true;
            this.cmbSelectCompany.Location = new System.Drawing.Point(169, 133);
            this.cmbSelectCompany.Name = "cmbSelectCompany";
            this.cmbSelectCompany.Size = new System.Drawing.Size(314, 21);
            this.cmbSelectCompany.TabIndex = 7;
            this.cmbSelectCompany.SelectedIndexChanged += new System.EventHandler(this.cmbSelectCompany_SelectedIndexChanged);
            // 
            // cmbFinancialYear
            // 
            this.cmbFinancialYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFinancialYear.Enabled = false;
            this.cmbFinancialYear.FormattingEnabled = true;
            this.cmbFinancialYear.Location = new System.Drawing.Point(169, 104);
            this.cmbFinancialYear.Name = "cmbFinancialYear";
            this.cmbFinancialYear.Size = new System.Drawing.Size(121, 21);
            this.cmbFinancialYear.TabIndex = 6;
            this.cmbFinancialYear.SelectedIndexChanged += new System.EventHandler(this.cmbFinancialYear_SelectedIndexChanged);
            // 
            // cmbVersionOftheTally
            // 
            this.cmbVersionOftheTally.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersionOftheTally.FormattingEnabled = true;
            this.cmbVersionOftheTally.Items.AddRange(new object[] {
            "Select Version",
            "7.2",
            "8.1",
            "9.0"});
            this.cmbVersionOftheTally.Location = new System.Drawing.Point(169, 76);
            this.cmbVersionOftheTally.Name = "cmbVersionOftheTally";
            this.cmbVersionOftheTally.Size = new System.Drawing.Size(121, 21);
            this.cmbVersionOftheTally.TabIndex = 5;
            // 
            // txtVisualUdyogPath
            // 
            this.txtVisualUdyogPath.Location = new System.Drawing.Point(169, 47);
            this.txtVisualUdyogPath.Name = "txtVisualUdyogPath";
            this.txtVisualUdyogPath.ReadOnly = true;
            this.txtVisualUdyogPath.Size = new System.Drawing.Size(314, 20);
            this.txtVisualUdyogPath.TabIndex = 3;
            this.txtVisualUdyogPath.TextChanged += new System.EventHandler(this.txtVisualUdyogPath_TextChanged);
            // 
            // txtXMLPath
            // 
            this.txtXMLPath.Location = new System.Drawing.Point(169, 19);
            this.txtXMLPath.Name = "txtXMLPath";
            this.txtXMLPath.ReadOnly = true;
            this.txtXMLPath.Size = new System.Drawing.Size(314, 20);
            this.txtXMLPath.TabIndex = 1;
            // 
            // lblSelectCompany
            // 
            this.lblSelectCompany.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectCompany.Location = new System.Drawing.Point(11, 132);
            this.lblSelectCompany.Name = "lblSelectCompany";
            this.lblSelectCompany.Size = new System.Drawing.Size(116, 23);
            this.lblSelectCompany.TabIndex = 0;
            this.lblSelectCompany.Text = "Select Company";
            this.lblSelectCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFinancialYear
            // 
            this.lblFinancialYear.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinancialYear.Location = new System.Drawing.Point(11, 103);
            this.lblFinancialYear.Name = "lblFinancialYear";
            this.lblFinancialYear.Size = new System.Drawing.Size(100, 23);
            this.lblFinancialYear.TabIndex = 0;
            this.lblFinancialYear.Text = "Financial Year";
            this.lblFinancialYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersionOftheTally
            // 
            this.lblVersionOftheTally.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionOftheTally.Location = new System.Drawing.Point(11, 75);
            this.lblVersionOftheTally.Name = "lblVersionOftheTally";
            this.lblVersionOftheTally.Size = new System.Drawing.Size(130, 23);
            this.lblVersionOftheTally.TabIndex = 0;
            this.lblVersionOftheTally.Text = "Tally Version";
            this.lblVersionOftheTally.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVisualUdyogPath
            // 
            this.lblVisualUdyogPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisualUdyogPath.Location = new System.Drawing.Point(11, 46);
            this.lblVisualUdyogPath.Name = "lblVisualUdyogPath";
            this.lblVisualUdyogPath.Size = new System.Drawing.Size(130, 23);
            this.lblVisualUdyogPath.TabIndex = 0;
            this.lblVisualUdyogPath.Text = "Udyog Product Path ";
            this.lblVisualUdyogPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblXMLPath
            // 
            this.lblXMLPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXMLPath.Location = new System.Drawing.Point(11, 18);
            this.lblXMLPath.Name = "lblXMLPath";
            this.lblXMLPath.Size = new System.Drawing.Size(100, 23);
            this.lblXMLPath.TabIndex = 0;
            this.lblXMLPath.Text = "XML Path";
            this.lblXMLPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCompanyDataBaseName
            // 
            this.txtCompanyDataBaseName.Location = new System.Drawing.Point(169, 163);
            this.txtCompanyDataBaseName.Name = "txtCompanyDataBaseName";
            this.txtCompanyDataBaseName.Size = new System.Drawing.Size(314, 20);
            this.txtCompanyDataBaseName.TabIndex = 0;
            this.txtCompanyDataBaseName.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDisplayType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(578, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lblDisplayType
            // 
            this.lblDisplayType.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayType.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.lblDisplayType.Location = new System.Drawing.Point(233, 18);
            this.lblDisplayType.Name = "lblDisplayType";
            this.lblDisplayType.Size = new System.Drawing.Size(331, 23);
            this.lblDisplayType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "System Config";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NewConfigForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(578, 249);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewConfigForm";
            this.Load += new System.EventHandler(this.NewConfigForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnVisualUdyogPath;
        private System.Windows.Forms.Button btnXMLPAth;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbSelectCompany;
        private System.Windows.Forms.ComboBox cmbFinancialYear;
        private System.Windows.Forms.ComboBox cmbVersionOftheTally;
        private System.Windows.Forms.TextBox txtVisualUdyogPath;
        private System.Windows.Forms.TextBox txtXMLPath;
        private System.Windows.Forms.Label lblSelectCompany;
        private System.Windows.Forms.Label lblFinancialYear;
        private System.Windows.Forms.Label lblVersionOftheTally;
        private System.Windows.Forms.Label lblVisualUdyogPath;
        private System.Windows.Forms.Label lblXMLPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDisplayType;
        private System.Windows.Forms.TextBox txtCompanyDataBaseName;
    }
}