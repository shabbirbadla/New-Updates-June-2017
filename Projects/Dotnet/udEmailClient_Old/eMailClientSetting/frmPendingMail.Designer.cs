namespace eMailClient
{
    partial class frmPendingMail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPendingMail));
            this.dgvPendingMail = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chk_selectall = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingMail)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPendingMail
            // 
            this.dgvPendingMail.AllowUserToAddRows = false;
            this.dgvPendingMail.AllowUserToDeleteRows = false;
            this.dgvPendingMail.AllowUserToOrderColumns = true;
            this.dgvPendingMail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPendingMail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPendingMail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPendingMail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPendingMail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingMail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colFilename});
            this.dgvPendingMail.Location = new System.Drawing.Point(0, 0);
            this.dgvPendingMail.Name = "dgvPendingMail";
            this.dgvPendingMail.Size = new System.Drawing.Size(920, 424);
            this.dgvPendingMail.TabIndex = 0;
            this.dgvPendingMail.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPendingMail_CellClick);
            // 
            // colSelect
            // 
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colSelect.HeaderText = "SELECT";
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 54;
            // 
            // colFilename
            // 
            this.colFilename.HeaderText = "ATTACHMENT";
            this.colFilename.Image = global::eMailClient.Properties.Resources.doc_file;
            this.colFilename.Name = "colFilename";
            this.colFilename.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilename.Width = 87;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(882, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(38, 24);
            this.btnCancel.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel (Ctrl+Z)");
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(838, 427);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(38, 24);
            this.btnOk.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnOk, "Ok (Ctrl+O)");
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chk_selectall
            // 
            this.chk_selectall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chk_selectall.AutoSize = true;
            this.chk_selectall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_selectall.Location = new System.Drawing.Point(0, 431);
            this.chk_selectall.Name = "chk_selectall";
            this.chk_selectall.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chk_selectall.Size = new System.Drawing.Size(67, 17);
            this.chk_selectall.TabIndex = 27;
            this.chk_selectall.Text = "Select &All";
            this.toolTip1.SetToolTip(this.chk_selectall, "Check for Select All");
            this.chk_selectall.UseVisualStyleBackColor = true;
            this.chk_selectall.CheckedChanged += new System.EventHandler(this.chk_selectall_CheckedChanged);
            // 
            // frmPendingMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 454);
            this.Controls.Add(this.chk_selectall);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dgvPendingMail);
            this.Name = "frmPendingMail";
            this.Text = "Pending Emails";
            this.Load += new System.EventHandler(this.frmPendingMail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPendingMail_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingMail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPendingMail;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chk_selectall;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewImageColumn colFilename;
    }
}