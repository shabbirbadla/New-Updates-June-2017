namespace U2TPlus
{
    partial class ConfigurationsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSelectCompany = new System.Windows.Forms.TextBox();
            this.txtFinancialYear = new System.Windows.Forms.TextBox();
            this.txtVersionOftheTally = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbTotalRecords = new System.Windows.Forms.ComboBox();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.txtVisualUdyogPath = new System.Windows.Forms.TextBox();
            this.txtXMLPath = new System.Windows.Forms.TextBox();
            this.lblSelectCompany = new System.Windows.Forms.Label();
            this.lblFinancialYear = new System.Windows.Forms.Label();
            this.lblVersionOftheTally = new System.Windows.Forms.Label();
            this.lblVisualUdyogPath = new System.Windows.Forms.Label();
            this.lblXMLPath = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(151, 19);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(241, 19);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(322, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(403, 19);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Close";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "System Config";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 264);
            this.panel1.TabIndex = 1;
            // 
            // groupBox2
            // 
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
            this.groupBox2.Size = new System.Drawing.Size(496, 264);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // txtSelectCompany
            // 
            this.txtSelectCompany.Location = new System.Drawing.Point(151, 200);
            this.txtSelectCompany.Name = "txtSelectCompany";
            this.txtSelectCompany.Size = new System.Drawing.Size(314, 20);
            this.txtSelectCompany.TabIndex = 19;
            // 
            // txtFinancialYear
            // 
            this.txtFinancialYear.Location = new System.Drawing.Point(151, 172);
            this.txtFinancialYear.Name = "txtFinancialYear";
            this.txtFinancialYear.Size = new System.Drawing.Size(314, 20);
            this.txtFinancialYear.TabIndex = 18;
            // 
            // txtVersionOftheTally
            // 
            this.txtVersionOftheTally.Location = new System.Drawing.Point(151, 145);
            this.txtVersionOftheTally.Name = "txtVersionOftheTally";
            this.txtVersionOftheTally.Size = new System.Drawing.Size(314, 20);
            this.txtVersionOftheTally.TabIndex = 17;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbTotalRecords);
            this.groupBox3.Controls.Add(this.lblTotalRecords);
            this.groupBox3.Location = new System.Drawing.Point(12, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(466, 50);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Total Records";
            // 
            // cmbTotalRecords
            // 
            this.cmbTotalRecords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTotalRecords.FormattingEnabled = true;
            this.cmbTotalRecords.Location = new System.Drawing.Point(139, 18);
            this.cmbTotalRecords.Name = "cmbTotalRecords";
            this.cmbTotalRecords.Size = new System.Drawing.Size(314, 21);
            this.cmbTotalRecords.TabIndex = 1;
            this.cmbTotalRecords.SelectedIndexChanged += new System.EventHandler(this.cmbTotalRecords_SelectedIndexChanged);
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.Location = new System.Drawing.Point(8, 22);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(81, 17);
            this.lblTotalRecords.TabIndex = 0;
            this.lblTotalRecords.Text = "Select Record";
            // 
            // txtVisualUdyogPath
            // 
            this.txtVisualUdyogPath.Location = new System.Drawing.Point(151, 119);
            this.txtVisualUdyogPath.Name = "txtVisualUdyogPath";
            this.txtVisualUdyogPath.Size = new System.Drawing.Size(314, 20);
            this.txtVisualUdyogPath.TabIndex = 10;
            // 
            // txtXMLPath
            // 
            this.txtXMLPath.Location = new System.Drawing.Point(151, 93);
            this.txtXMLPath.Name = "txtXMLPath";
            this.txtXMLPath.Size = new System.Drawing.Size(314, 20);
            this.txtXMLPath.TabIndex = 7;
            // 
            // lblSelectCompany
            // 
            this.lblSelectCompany.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectCompany.Location = new System.Drawing.Point(12, 199);
            this.lblSelectCompany.Name = "lblSelectCompany";
            this.lblSelectCompany.Size = new System.Drawing.Size(116, 23);
            this.lblSelectCompany.TabIndex = 6;
            this.lblSelectCompany.Text = "Select Company";
            this.lblSelectCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFinancialYear
            // 
            this.lblFinancialYear.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinancialYear.Location = new System.Drawing.Point(12, 171);
            this.lblFinancialYear.Name = "lblFinancialYear";
            this.lblFinancialYear.Size = new System.Drawing.Size(100, 23);
            this.lblFinancialYear.TabIndex = 5;
            this.lblFinancialYear.Text = "Financial Year";
            this.lblFinancialYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersionOftheTally
            // 
            this.lblVersionOftheTally.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionOftheTally.Location = new System.Drawing.Point(12, 144);
            this.lblVersionOftheTally.Name = "lblVersionOftheTally";
            this.lblVersionOftheTally.Size = new System.Drawing.Size(130, 23);
            this.lblVersionOftheTally.TabIndex = 4;
            this.lblVersionOftheTally.Text = "Tally Version";
            this.lblVersionOftheTally.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVisualUdyogPath
            // 
            this.lblVisualUdyogPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisualUdyogPath.Location = new System.Drawing.Point(12, 118);
            this.lblVisualUdyogPath.Name = "lblVisualUdyogPath";
            this.lblVisualUdyogPath.Size = new System.Drawing.Size(130, 23);
            this.lblVisualUdyogPath.TabIndex = 3;
            this.lblVisualUdyogPath.Text = "Udyog Product Path ";
            this.lblVisualUdyogPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblXMLPath
            // 
            this.lblXMLPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXMLPath.Location = new System.Drawing.Point(12, 92);
            this.lblXMLPath.Name = "lblXMLPath";
            this.lblXMLPath.Size = new System.Drawing.Size(100, 23);
            this.lblXMLPath.TabIndex = 0;
            this.lblXMLPath.Text = "XML Path";
            this.lblXMLPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigurationsForm
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(496, 314);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Configurations";
            this.Load += new System.EventHandler(this.ConfigurationsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSelectCompany;
        private System.Windows.Forms.Label lblFinancialYear;
        private System.Windows.Forms.Label lblVersionOftheTally;
        private System.Windows.Forms.Label lblVisualUdyogPath;
        private System.Windows.Forms.Label lblXMLPath;
        private System.Windows.Forms.TextBox txtVisualUdyogPath;
        private System.Windows.Forms.TextBox txtXMLPath;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbTotalRecords;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.TextBox txtSelectCompany;
        private System.Windows.Forms.TextBox txtFinancialYear;
        private System.Windows.Forms.TextBox txtVersionOftheTally;
    }
}