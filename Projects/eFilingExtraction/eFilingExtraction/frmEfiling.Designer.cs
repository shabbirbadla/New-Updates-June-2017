namespace eFillingExtraction
{
    partial class frmEfiling
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
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkRAddChange = new System.Windows.Forms.CheckBox();
            this.chkAddChange = new System.Windows.Forms.CheckBox();
            this.cboFormNo = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGenerateFile = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatuslbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtFrmDate = new System.Windows.Forms.TextBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.cboQuarter = new System.Windows.Forms.ComboBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.lblQuarter = new System.Windows.Forms.Label();
            this.lblFormNo = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(422, 62);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(11, 14);
            this.lbl3.TabIndex = 7;
            this.lbl3.Text = ")";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(331, 62);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(18, 14);
            this.lbl2.TabIndex = 6;
            this.lbl2.Text = "To";
            // 
            // txtToDate
            // 
            this.txtToDate.BackColor = System.Drawing.Color.White;
            this.txtToDate.Enabled = false;
            this.txtToDate.Location = new System.Drawing.Point(352, 59);
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(69, 20);
            this.txtToDate.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chkRAddChange);
            this.panel1.Controls.Add(this.chkAddChange);
            this.panel1.Controls.Add(this.cboFormNo);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnGenerateFile);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtOutputPath);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.lbl3);
            this.panel1.Controls.Add(this.lbl2);
            this.panel1.Controls.Add(this.txtToDate);
            this.panel1.Controls.Add(this.txtFrmDate);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Controls.Add(this.cboQuarter);
            this.panel1.Controls.Add(this.lblOutputPath);
            this.panel1.Controls.Add(this.lblQuarter);
            this.panel1.Controls.Add(this.lblFormNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 239);
            this.panel1.TabIndex = 1;
            // 
            // chkRAddChange
            // 
            this.chkRAddChange.AutoSize = true;
            this.chkRAddChange.Location = new System.Drawing.Point(92, 150);
            this.chkRAddChange.Name = "chkRAddChange";
            this.chkRAddChange.Size = new System.Drawing.Size(232, 18);
            this.chkRAddChange.TabIndex = 12;
            this.chkRAddChange.Text = "Change of Address of Responsible Person";
            this.chkRAddChange.UseVisualStyleBackColor = true;
            // 
            // chkAddChange
            // 
            this.chkAddChange.AutoSize = true;
            this.chkAddChange.Location = new System.Drawing.Point(92, 123);
            this.chkAddChange.Name = "chkAddChange";
            this.chkAddChange.Size = new System.Drawing.Size(180, 18);
            this.chkAddChange.TabIndex = 11;
            this.chkAddChange.Text = "Change of Address of Deductor";
            this.chkAddChange.UseVisualStyleBackColor = true;
            // 
            // cboFormNo
            // 
            this.cboFormNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormNo.FormattingEnabled = true;
            this.cboFormNo.Location = new System.Drawing.Point(92, 23);
            this.cboFormNo.Name = "cboFormNo";
            this.cboFormNo.Size = new System.Drawing.Size(121, 22);
            this.cboFormNo.TabIndex = 13;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(336, 180);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 27);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerateFile
            // 
            this.btnGenerateFile.Location = new System.Drawing.Point(242, 180);
            this.btnGenerateFile.Name = "btnGenerateFile";
            this.btnGenerateFile.Size = new System.Drawing.Size(90, 27);
            this.btnGenerateFile.TabIndex = 21;
            this.btnGenerateFile.Text = "Generate File";
            this.btnGenerateFile.UseVisualStyleBackColor = true;
            this.btnGenerateFile.Click += new System.EventHandler(this.btnGenerateExcel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(365, 89);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(57, 27);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(92, 92);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(267, 20);
            this.txtOutputPath.TabIndex = 9;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatuslbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 213);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(447, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatuslbl
            // 
            this.toolStripStatuslbl.Name = "toolStripStatuslbl";
            this.toolStripStatuslbl.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatuslbl.Text = "Ready";
            // 
            // txtFrmDate
            // 
            this.txtFrmDate.BackColor = System.Drawing.Color.White;
            this.txtFrmDate.Enabled = false;
            this.txtFrmDate.Location = new System.Drawing.Point(262, 59);
            this.txtFrmDate.Name = "txtFrmDate";
            this.txtFrmDate.Size = new System.Drawing.Size(69, 20);
            this.txtFrmDate.TabIndex = 5;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(224, 61);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(38, 14);
            this.lbl1.TabIndex = 4;
            this.lbl1.Text = "( From";
            // 
            // cboQuarter
            // 
            this.cboQuarter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuarter.FormattingEnabled = true;
            this.cboQuarter.Items.AddRange(new object[] {
            "Q1",
            "Q2",
            "Q3",
            "Q4"});
            this.cboQuarter.Location = new System.Drawing.Point(92, 57);
            this.cboQuarter.Name = "cboQuarter";
            this.cboQuarter.Size = new System.Drawing.Size(121, 22);
            this.cboQuarter.TabIndex = 3;
            this.cboQuarter.SelectedIndexChanged += new System.EventHandler(this.cboQuarter_SelectedIndexChanged);
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(10, 96);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(72, 14);
            this.lblOutputPath.TabIndex = 2;
            this.lblOutputPath.Text = "Output Folder";
            // 
            // lblQuarter
            // 
            this.lblQuarter.AutoSize = true;
            this.lblQuarter.Location = new System.Drawing.Point(10, 60);
            this.lblQuarter.Name = "lblQuarter";
            this.lblQuarter.Size = new System.Drawing.Size(44, 14);
            this.lblQuarter.TabIndex = 2;
            this.lblQuarter.Text = "Quarter";
            // 
            // lblFormNo
            // 
            this.lblFormNo.AutoSize = true;
            this.lblFormNo.Location = new System.Drawing.Point(10, 26);
            this.lblFormNo.Name = "lblFormNo";
            this.lblFormNo.Size = new System.Drawing.Size(50, 14);
            this.lblFormNo.TabIndex = 0;
            this.lblFormNo.Text = "Form No.";
            // 
            // frmEfiling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 239);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmEfiling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmEfiling_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.TextBox txtToDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkRAddChange;
        private System.Windows.Forms.CheckBox chkAddChange;
        private System.Windows.Forms.ComboBox cboFormNo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGenerateFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatuslbl;
        private System.Windows.Forms.TextBox txtFrmDate;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.ComboBox cboQuarter;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.Label lblQuarter;
        private System.Windows.Forms.Label lblFormNo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

