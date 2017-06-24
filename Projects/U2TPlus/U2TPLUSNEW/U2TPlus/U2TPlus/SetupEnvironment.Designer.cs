namespace U2TPlus
{
    partial class SetupEnvironment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupEnvironment));
            this.txtFinancialYear = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVersionOftheTally = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbTotalRecords = new System.Windows.Forms.ComboBox();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.txtSelectCompany = new System.Windows.Forms.TextBox();
            this.txtVisualUdyogPath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCompanyConfigID = new System.Windows.Forms.TextBox();
            this.txtCompanyDataBaseName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEnvironment = new System.Windows.Forms.Button();
            this.txtXMLPath = new System.Windows.Forms.TextBox();
            this.lblSelectCompany = new System.Windows.Forms.Label();
            this.lblFinancialYear = new System.Windows.Forms.Label();
            this.lblVersionOftheTally = new System.Windows.Forms.Label();
            this.lblVisualUdyogPath = new System.Windows.Forms.Label();
            this.lblXMLPath = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFinancialYear
            // 
            this.txtFinancialYear.Location = new System.Drawing.Point(194, 190);
            this.txtFinancialYear.Name = "txtFinancialYear";
            this.txtFinancialYear.ReadOnly = true;
            this.txtFinancialYear.Size = new System.Drawing.Size(314, 20);
            this.txtFinancialYear.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(592, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Set Environment";
            // 
            // txtVersionOftheTally
            // 
            this.txtVersionOftheTally.Location = new System.Drawing.Point(194, 164);
            this.txtVersionOftheTally.Name = "txtVersionOftheTally";
            this.txtVersionOftheTally.ReadOnly = true;
            this.txtVersionOftheTally.Size = new System.Drawing.Size(314, 20);
            this.txtVersionOftheTally.TabIndex = 17;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbTotalRecords);
            this.groupBox3.Controls.Add(this.lblTotalRecords);
            this.groupBox3.Location = new System.Drawing.Point(32, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(536, 50);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Total Records";
            // 
            // cmbTotalRecords
            // 
            this.cmbTotalRecords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTotalRecords.FormattingEnabled = true;
            this.cmbTotalRecords.Location = new System.Drawing.Point(162, 16);
            this.cmbTotalRecords.Name = "cmbTotalRecords";
            this.cmbTotalRecords.Size = new System.Drawing.Size(314, 21);
            this.cmbTotalRecords.TabIndex = 1;
            this.cmbTotalRecords.SelectedIndexChanged += new System.EventHandler(this.cmbTotalRecords_SelectedIndexChanged);
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.Location = new System.Drawing.Point(75, 20);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(81, 17);
            this.lblTotalRecords.TabIndex = 0;
            this.lblTotalRecords.Text = "Select Record";
            // 
            // txtSelectCompany
            // 
            this.txtSelectCompany.Location = new System.Drawing.Point(194, 217);
            this.txtSelectCompany.Name = "txtSelectCompany";
            this.txtSelectCompany.ReadOnly = true;
            this.txtSelectCompany.Size = new System.Drawing.Size(314, 20);
            this.txtSelectCompany.TabIndex = 19;
            // 
            // txtVisualUdyogPath
            // 
            this.txtVisualUdyogPath.Location = new System.Drawing.Point(194, 139);
            this.txtVisualUdyogPath.Name = "txtVisualUdyogPath";
            this.txtVisualUdyogPath.ReadOnly = true;
            this.txtVisualUdyogPath.Size = new System.Drawing.Size(314, 20);
            this.txtVisualUdyogPath.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCompanyConfigID);
            this.groupBox2.Controls.Add(this.txtCompanyDataBaseName);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnEnvironment);
            this.groupBox2.Controls.Add(this.txtSelectCompany);
            this.groupBox2.Controls.Add(this.txtFinancialYear);
            this.groupBox2.Controls.Add(this.txtVersionOftheTally);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.txtVisualUdyogPath);
            this.groupBox2.Controls.Add(this.txtXMLPath);
            this.groupBox2.Controls.Add(this.lblSelectCompany);
            this.groupBox2.Controls.Add(this.lblFinancialYear);
            this.groupBox2.Controls.Add(this.lblVersionOftheTally);
            this.groupBox2.Controls.Add(this.lblVisualUdyogPath);
            this.groupBox2.Controls.Add(this.lblXMLPath);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(592, 296);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // txtCompanyConfigID
            // 
            this.txtCompanyConfigID.Location = new System.Drawing.Point(354, 243);
            this.txtCompanyConfigID.Name = "txtCompanyConfigID";
            this.txtCompanyConfigID.ReadOnly = true;
            this.txtCompanyConfigID.Size = new System.Drawing.Size(154, 20);
            this.txtCompanyConfigID.TabIndex = 23;
            this.txtCompanyConfigID.Visible = false;
            // 
            // txtCompanyDataBaseName
            // 
            this.txtCompanyDataBaseName.Location = new System.Drawing.Point(194, 242);
            this.txtCompanyDataBaseName.Name = "txtCompanyDataBaseName";
            this.txtCompanyDataBaseName.ReadOnly = true;
            this.txtCompanyDataBaseName.Size = new System.Drawing.Size(154, 20);
            this.txtCompanyDataBaseName.TabIndex = 22;
            this.txtCompanyDataBaseName.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(389, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(119, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEnvironment
            // 
            this.btnEnvironment.Location = new System.Drawing.Point(194, 263);
            this.btnEnvironment.Name = "btnEnvironment";
            this.btnEnvironment.Size = new System.Drawing.Size(119, 23);
            this.btnEnvironment.TabIndex = 20;
            this.btnEnvironment.Text = "Set Environment";
            this.btnEnvironment.UseVisualStyleBackColor = true;
            this.btnEnvironment.Click += new System.EventHandler(this.btnEnvironment_Click);
            // 
            // txtXMLPath
            // 
            this.txtXMLPath.Location = new System.Drawing.Point(194, 115);
            this.txtXMLPath.Name = "txtXMLPath";
            this.txtXMLPath.ReadOnly = true;
            this.txtXMLPath.Size = new System.Drawing.Size(314, 20);
            this.txtXMLPath.TabIndex = 7;
            // 
            // lblSelectCompany
            // 
            this.lblSelectCompany.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectCompany.Location = new System.Drawing.Point(58, 217);
            this.lblSelectCompany.Name = "lblSelectCompany";
            this.lblSelectCompany.Size = new System.Drawing.Size(116, 23);
            this.lblSelectCompany.TabIndex = 6;
            this.lblSelectCompany.Text = "Company";
            // 
            // lblFinancialYear
            // 
            this.lblFinancialYear.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinancialYear.Location = new System.Drawing.Point(58, 191);
            this.lblFinancialYear.Name = "lblFinancialYear";
            this.lblFinancialYear.Size = new System.Drawing.Size(100, 23);
            this.lblFinancialYear.TabIndex = 5;
            this.lblFinancialYear.Text = "Financial Year";
            // 
            // lblVersionOftheTally
            // 
            this.lblVersionOftheTally.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionOftheTally.Location = new System.Drawing.Point(58, 165);
            this.lblVersionOftheTally.Name = "lblVersionOftheTally";
            this.lblVersionOftheTally.Size = new System.Drawing.Size(130, 23);
            this.lblVersionOftheTally.TabIndex = 4;
            this.lblVersionOftheTally.Text = "Version Of the Tally";
            // 
            // lblVisualUdyogPath
            // 
            this.lblVisualUdyogPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisualUdyogPath.Location = new System.Drawing.Point(58, 140);
            this.lblVisualUdyogPath.Name = "lblVisualUdyogPath";
            this.lblVisualUdyogPath.Size = new System.Drawing.Size(116, 23);
            this.lblVisualUdyogPath.TabIndex = 3;
            this.lblVisualUdyogPath.Text = "Visual Udyog Path ";
            // 
            // lblXMLPath
            // 
            this.lblXMLPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXMLPath.Location = new System.Drawing.Point(58, 117);
            this.lblXMLPath.Name = "lblXMLPath";
            this.lblXMLPath.Size = new System.Drawing.Size(100, 23);
            this.lblXMLPath.TabIndex = 0;
            this.lblXMLPath.Text = "XML Path";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 296);
            this.panel1.TabIndex = 3;
            // 
            // SetupEnvironment
            // 
            this.AcceptButton = this.btnEnvironment;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(592, 296);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupEnvironment";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Environment";
            this.Load += new System.EventHandler(this.SetupEnvironment_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtFinancialYear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVersionOftheTally;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbTotalRecords;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.TextBox txtSelectCompany;
        private System.Windows.Forms.TextBox txtVisualUdyogPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtXMLPath;
        private System.Windows.Forms.Label lblSelectCompany;
        private System.Windows.Forms.Label lblFinancialYear;
        private System.Windows.Forms.Label lblVersionOftheTally;
        private System.Windows.Forms.Label lblVisualUdyogPath;
        private System.Windows.Forms.Label lblXMLPath;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEnvironment;
        private System.Windows.Forms.TextBox txtCompanyDataBaseName;
        private System.Windows.Forms.TextBox txtCompanyConfigID;
    }
}