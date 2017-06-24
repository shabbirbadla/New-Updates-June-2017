namespace WindowsFormsApplication5
{
    partial class frmRegister_dll
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
            this.btnregister_dll = new System.Windows.Forms.Button();
            this.txtSelPath = new System.Windows.Forms.TextBox();
            this.lblSelPath = new System.Windows.Forms.Label();
            this.btnSelPath = new System.Windows.Forms.Button();
            this.dgvFileList = new System.Windows.Forms.DataGridView();
            this.colSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSel = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnregister_dll
            // 
            this.btnregister_dll.Location = new System.Drawing.Point(84, 330);
            this.btnregister_dll.Name = "btnregister_dll";
            this.btnregister_dll.Size = new System.Drawing.Size(151, 23);
            this.btnregister_dll.TabIndex = 4;
            this.btnregister_dll.Text = "Register DLL";
            this.btnregister_dll.UseVisualStyleBackColor = true;
            this.btnregister_dll.Click += new System.EventHandler(this.btnregister_dll_Click);
            // 
            // txtSelPath
            // 
            this.txtSelPath.Enabled = false;
            this.txtSelPath.Location = new System.Drawing.Point(75, 14);
            this.txtSelPath.Name = "txtSelPath";
            this.txtSelPath.Size = new System.Drawing.Size(356, 20);
            this.txtSelPath.TabIndex = 1;
            // 
            // lblSelPath
            // 
            this.lblSelPath.Location = new System.Drawing.Point(9, 10);
            this.lblSelPath.Name = "lblSelPath";
            this.lblSelPath.Size = new System.Drawing.Size(65, 29);
            this.lblSelPath.TabIndex = 0;
            this.lblSelPath.Text = "Select DLL Path";
            this.lblSelPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSelPath
            // 
            this.btnSelPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelPath.Location = new System.Drawing.Point(434, 13);
            this.btnSelPath.Name = "btnSelPath";
            this.btnSelPath.Size = new System.Drawing.Size(27, 23);
            this.btnSelPath.TabIndex = 2;
            this.btnSelPath.Text = "...";
            this.btnSelPath.UseVisualStyleBackColor = true;
            this.btnSelPath.Click += new System.EventHandler(this.btnSelPath_Click);
            // 
            // dgvFileList
            // 
            this.dgvFileList.AllowUserToAddRows = false;
            this.dgvFileList.AllowUserToDeleteRows = false;
            this.dgvFileList.AllowUserToResizeColumns = false;
            this.dgvFileList.AllowUserToResizeRows = false;
            this.dgvFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSel,
            this.colFileName,
            this.colStatus});
            this.dgvFileList.Location = new System.Drawing.Point(11, 62);
            this.dgvFileList.Name = "dgvFileList";
            this.dgvFileList.RowHeadersVisible = false;
            this.dgvFileList.Size = new System.Drawing.Size(450, 263);
            this.dgvFileList.TabIndex = 3;
            // 
            // colSel
            // 
            this.colSel.HeaderText = "Sel";
            this.colSel.Name = "colSel";
            this.colSel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSel.Width = 25;
            // 
            // colFileName
            // 
            this.colFileName.HeaderText = "File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Width = 320;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 85;
            // 
            // chkSel
            // 
            this.chkSel.AutoSize = true;
            this.chkSel.Location = new System.Drawing.Point(13, 43);
            this.chkSel.Name = "chkSel";
            this.chkSel.Size = new System.Drawing.Size(70, 17);
            this.chkSel.TabIndex = 5;
            this.chkSel.Text = "Select All";
            this.chkSel.UseVisualStyleBackColor = true;
            this.chkSel.CheckedChanged += new System.EventHandler(this.chkSel_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(237, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmRegister_dll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 359);
            this.Controls.Add(this.chkSel);
            this.Controls.Add(this.dgvFileList);
            this.Controls.Add(this.btnSelPath);
            this.Controls.Add(this.lblSelPath);
            this.Controls.Add(this.txtSelPath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnregister_dll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRegister_dll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmRegister_dll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnregister_dll;
        private System.Windows.Forms.TextBox txtSelPath;
        private System.Windows.Forms.Label lblSelPath;
        private System.Windows.Forms.Button btnSelPath;
        private System.Windows.Forms.DataGridView dgvFileList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.CheckBox chkSel;
        private System.Windows.Forms.Button btnCancel;
    }
}

