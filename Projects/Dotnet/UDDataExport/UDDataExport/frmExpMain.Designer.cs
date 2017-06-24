namespace UDDataExport
{
    partial class frmExpMain
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            this.txtSendTo = new System.Windows.Forms.TextBox();
            this.lblSendTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblFromDt = new System.Windows.Forms.Label();
            this.lblToDt = new System.Windows.Forms.Label();
            this.lblPath = new System.Windows.Forms.Label();
            this.dtpFrmDt = new System.Windows.Forms.DateTimePicker();
            this.dtpToDt = new System.Windows.Forms.DateTimePicker();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSendTo = new System.Windows.Forms.Button();
            this.btnPath = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // txtSendTo
            // 
            this.txtSendTo.Location = new System.Drawing.Point(124, 30);
            this.txtSendTo.Name = "txtSendTo";
            this.txtSendTo.ReadOnly = true;
            this.txtSendTo.Size = new System.Drawing.Size(285, 20);
            this.txtSendTo.TabIndex = 0;
            // 
            // lblSendTo
            // 
            this.lblSendTo.AutoSize = true;
            this.lblSendTo.Location = new System.Drawing.Point(25, 33);
            this.lblSendTo.Name = "lblSendTo";
            this.lblSendTo.Size = new System.Drawing.Size(79, 13);
            this.lblSendTo.TabIndex = 1;
            this.lblSendTo.Text = "Export Data To";
            // 
            // lblFrom
            // 
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.3F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(25, 6);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(416, 21);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "Data From";
            // 
            // lblFromDt
            // 
            this.lblFromDt.AutoSize = true;
            this.lblFromDt.Location = new System.Drawing.Point(25, 58);
            this.lblFromDt.Name = "lblFromDt";
            this.lblFromDt.Size = new System.Drawing.Size(53, 13);
            this.lblFromDt.TabIndex = 4;
            this.lblFromDt.Text = "Date from";
            // 
            // lblToDt
            // 
            this.lblToDt.AutoSize = true;
            this.lblToDt.Location = new System.Drawing.Point(208, 59);
            this.lblToDt.Name = "lblToDt";
            this.lblToDt.Size = new System.Drawing.Size(20, 13);
            this.lblToDt.TabIndex = 5;
            this.lblToDt.Text = "To";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(25, 84);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(87, 13);
            this.lblPath.TabIndex = 6;
            this.lblPath.Text = "Export File Name";
            // 
            // dtpFrmDt
            // 
            this.dtpFrmDt.Checked = false;
            this.dtpFrmDt.CustomFormat = "dd/MM/yyyy";
            this.dtpFrmDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrmDt.Location = new System.Drawing.Point(124, 56);
            this.dtpFrmDt.Name = "dtpFrmDt";
            this.dtpFrmDt.Size = new System.Drawing.Size(78, 20);
            this.dtpFrmDt.TabIndex = 2;
            // 
            // dtpToDt
            // 
            this.dtpToDt.Checked = false;
            this.dtpToDt.CustomFormat = "dd/MM/yyyy";
            this.dtpToDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDt.Location = new System.Drawing.Point(235, 56);
            this.dtpToDt.Name = "dtpToDt";
            this.dtpToDt.Size = new System.Drawing.Size(78, 20);
            this.dtpToDt.TabIndex = 3;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(124, 82);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(285, 20);
            this.txtPath.TabIndex = 4;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(346, 108);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(83, 28);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(257, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSendTo
            // 
            this.btnSendTo.Location = new System.Drawing.Point(413, 29);
            this.btnSendTo.Name = "btnSendTo";
            this.btnSendTo.Size = new System.Drawing.Size(28, 23);
            this.btnSendTo.TabIndex = 1;
            this.btnSendTo.UseVisualStyleBackColor = true;
            this.btnSendTo.Click += new System.EventHandler(this.btnSendTo_Click);
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(413, 79);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(28, 23);
            this.btnPath.TabIndex = 5;
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(6, 142);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(438, 308);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 340;
            // 
            // frmExpMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 453);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.btnSendTo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.dtpToDt);
            this.Controls.Add(this.dtpFrmDt);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.lblToDt);
            this.Controls.Add(this.lblFromDt);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.lblSendTo);
            this.Controls.Add(this.txtSendTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExpMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Export Tool";
            this.Load += new System.EventHandler(this.frmExpMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmExpMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSendTo;
        private System.Windows.Forms.Label lblSendTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblFromDt;
        private System.Windows.Forms.Label lblToDt;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.DateTimePicker dtpFrmDt;
        private System.Windows.Forms.DateTimePicker dtpToDt;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSendTo;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

