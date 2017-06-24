namespace Bug_24972
{
    partial class Frm402403
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboGVType = new System.Windows.Forms.ComboBox();
            this.lblGVType = new System.Windows.Forms.Label();
            this.cboInvNo = new System.Windows.Forms.ComboBox();
            this.lblInvNo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportData = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.cboGVType);
            this.panel1.Controls.Add(this.lblGVType);
            this.panel1.Controls.Add(this.cboInvNo);
            this.panel1.Controls.Add(this.lblInvNo);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnExportData);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 114);
            this.panel1.TabIndex = 0;
            // 
            // cboGVType
            // 
            this.cboGVType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGVType.FormattingEnabled = true;
            this.cboGVType.Items.AddRange(new object[] {
            "NonSpecified",
            "Specified"});
            this.cboGVType.Location = new System.Drawing.Point(372, 22);
            this.cboGVType.Name = "cboGVType";
            this.cboGVType.Size = new System.Drawing.Size(132, 21);
            this.cboGVType.TabIndex = 20;
            this.cboGVType.Validating += new System.ComponentModel.CancelEventHandler(this.cboGVType_Validating);
            this.cboGVType.Leave += new System.EventHandler(this.cboGVType_Leave);
            this.cboGVType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboGVType_KeyPress);
            this.cboGVType.Click += new System.EventHandler(this.cboGVType_Click);
            // 
            // lblGVType
            // 
            this.lblGVType.AutoSize = true;
            this.lblGVType.Location = new System.Drawing.Point(301, 25);
            this.lblGVType.Name = "lblGVType";
            this.lblGVType.Size = new System.Drawing.Size(65, 13);
            this.lblGVType.TabIndex = 19;
            this.lblGVType.Text = "Goods Type";
            // 
            // cboInvNo
            // 
            this.cboInvNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInvNo.FormattingEnabled = true;
            this.cboInvNo.Location = new System.Drawing.Point(87, 22);
            this.cboInvNo.Name = "cboInvNo";
            this.cboInvNo.Size = new System.Drawing.Size(121, 21);
            this.cboInvNo.TabIndex = 18;
            // 
            // lblInvNo
            // 
            this.lblInvNo.AutoSize = true;
            this.lblInvNo.Location = new System.Drawing.Point(5, 25);
            this.lblInvNo.Name = "lblInvNo";
            this.lblInvNo.Size = new System.Drawing.Size(62, 13);
            this.lblInvNo.TabIndex = 17;
            this.lblInvNo.Text = "Invoice No.";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(414, 60);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportData
            // 
            this.btnExportData.Location = new System.Drawing.Point(304, 60);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(104, 25);
            this.btnExportData.TabIndex = 12;
            this.btnExportData.Text = "Fill Data to Excel";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 88);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(514, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = " ";
            // 
            // Frm402403
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 113);
            this.Controls.Add(this.panel1);
            this.Name = "Frm402403";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form 402/403";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ComboBox cboInvNo;
        private System.Windows.Forms.Label lblInvNo;
        private System.Windows.Forms.ComboBox cboGVType;
        private System.Windows.Forms.Label lblGVType;
    }
}

