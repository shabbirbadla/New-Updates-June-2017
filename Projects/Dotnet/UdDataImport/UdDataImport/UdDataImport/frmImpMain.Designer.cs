namespace WindowsFormsApplication1
{
    partial class frmImpMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImpMain));
            this.btnPath = new System.Windows.Forms.Button();
            this.btnImportFrom = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.dtpToDt = new System.Windows.Forms.DateTimePicker();
            this.dtpFrmDt = new System.Windows.Forms.DateTimePicker();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblToDt = new System.Windows.Forms.Label();
            this.lblFromDt = new System.Windows.Forms.Label();
            this.lblImportFrom = new System.Windows.Forms.Label();
            this.txtImportFrom = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // btnPath
            // 
            this.btnPath.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPath.Location = new System.Drawing.Point(395, 66);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(28, 25);
            this.btnPath.TabIndex = 36;
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // btnImportFrom
            // 
            this.btnImportFrom.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportFrom.Location = new System.Drawing.Point(395, 12);
            this.btnImportFrom.Name = "btnImportFrom";
            this.btnImportFrom.Size = new System.Drawing.Size(28, 25);
            this.btnImportFrom.TabIndex = 35;
            this.btnImportFrom.UseVisualStyleBackColor = true;
            this.btnImportFrom.Click += new System.EventHandler(this.btnImportFrom_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(240, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 30);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(329, 106);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(83, 30);
            this.btnImport.TabIndex = 33;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(106, 69);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(285, 20);
            this.txtPath.TabIndex = 32;
            // 
            // dtpToDt
            // 
            this.dtpToDt.Checked = false;
            this.dtpToDt.CustomFormat = "dd/MM/yyyy";
            this.dtpToDt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDt.Location = new System.Drawing.Point(217, 41);
            this.dtpToDt.Name = "dtpToDt";
            this.dtpToDt.Size = new System.Drawing.Size(78, 20);
            this.dtpToDt.TabIndex = 31;
            // 
            // dtpFrmDt
            // 
            this.dtpFrmDt.Checked = false;
            this.dtpFrmDt.CustomFormat = "dd/MM/yyyy";
            this.dtpFrmDt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrmDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrmDt.Location = new System.Drawing.Point(106, 41);
            this.dtpFrmDt.Name = "dtpFrmDt";
            this.dtpFrmDt.Size = new System.Drawing.Size(78, 20);
            this.dtpFrmDt.TabIndex = 30;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPath.Location = new System.Drawing.Point(7, 69);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(85, 14);
            this.lblPath.TabIndex = 29;
            this.lblPath.Text = "Import File Name";
            // 
            // lblToDt
            // 
            this.lblToDt.AutoSize = true;
            this.lblToDt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDt.Location = new System.Drawing.Point(190, 44);
            this.lblToDt.Name = "lblToDt";
            this.lblToDt.Size = new System.Drawing.Size(18, 14);
            this.lblToDt.TabIndex = 28;
            this.lblToDt.Text = "To";
            // 
            // lblFromDt
            // 
            this.lblFromDt.AutoSize = true;
            this.lblFromDt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDt.Location = new System.Drawing.Point(7, 43);
            this.lblFromDt.Name = "lblFromDt";
            this.lblFromDt.Size = new System.Drawing.Size(54, 14);
            this.lblFromDt.TabIndex = 27;
            this.lblFromDt.Text = "Date from";
            // 
            // lblImportFrom
            // 
            this.lblImportFrom.AutoSize = true;
            this.lblImportFrom.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportFrom.Location = new System.Drawing.Point(7, 16);
            this.lblImportFrom.Name = "lblImportFrom";
            this.lblImportFrom.Size = new System.Drawing.Size(61, 14);
            this.lblImportFrom.TabIndex = 26;
            this.lblImportFrom.Text = "Import from";
            // 
            // txtImportFrom
            // 
            this.txtImportFrom.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImportFrom.Location = new System.Drawing.Point(106, 13);
            this.txtImportFrom.Name = "txtImportFrom";
            this.txtImportFrom.ReadOnly = true;
            this.txtImportFrom.Size = new System.Drawing.Size(285, 20);
            this.txtImportFrom.TabIndex = 25;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.Location = new System.Drawing.Point(4, 145);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(419, 305);
            this.listView1.TabIndex = 37;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 112;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 301;
            // 
            // frmImpMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 453);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.btnImportFrom);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.dtpToDt);
            this.Controls.Add(this.dtpFrmDt);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.lblToDt);
            this.Controls.Add(this.lblFromDt);
            this.Controls.Add(this.lblImportFrom);
            this.Controls.Add(this.txtImportFrom);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImpMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Import Tool";
            this.Load += new System.EventHandler(this.frmImpMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmImpMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.Button btnImportFrom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.DateTimePicker dtpToDt;
        private System.Windows.Forms.DateTimePicker dtpFrmDt;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Label lblToDt;
        private System.Windows.Forms.Label lblFromDt;
        private System.Windows.Forms.Label lblImportFrom;
        private System.Windows.Forms.TextBox txtImportFrom;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

