namespace eMailClient
{
    partial class frmSendEmail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSendEmail));
            this.chklst_email = new System.Windows.Forms.ListView();
            this.colhdr_trantyp = new System.Windows.Forms.ColumnHeader();
            this.colhdr_name = new System.Windows.Forms.ColumnHeader();
            this.chk_selectall = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // chklst_email
            // 
            this.chklst_email.CheckBoxes = true;
            this.chklst_email.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhdr_trantyp,
            this.colhdr_name});
            this.chklst_email.FullRowSelect = true;
            this.chklst_email.GridLines = true;
            this.chklst_email.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.chklst_email.Location = new System.Drawing.Point(2, 2);
            this.chklst_email.Name = "chklst_email";
            this.chklst_email.ShowItemToolTips = true;
            this.chklst_email.Size = new System.Drawing.Size(777, 231);
            this.chklst_email.TabIndex = 4;
            this.toolTip1.SetToolTip(this.chklst_email, "Select Email(s) to send");
            this.chklst_email.UseCompatibleStateImageBehavior = false;
            this.chklst_email.View = System.Windows.Forms.View.Details;
            // 
            // colhdr_trantyp
            // 
            this.colhdr_trantyp.Text = "Id";
            this.colhdr_trantyp.Width = 61;
            // 
            // colhdr_name
            // 
            this.colhdr_name.Text = "Description";
            this.colhdr_name.Width = 535;
            // 
            // chk_selectall
            // 
            this.chk_selectall.AutoSize = true;
            this.chk_selectall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_selectall.Location = new System.Drawing.Point(2, 243);
            this.chk_selectall.Name = "chk_selectall";
            this.chk_selectall.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chk_selectall.Size = new System.Drawing.Size(67, 17);
            this.chk_selectall.TabIndex = 28;
            this.chk_selectall.Text = "Select &All";
            this.toolTip1.SetToolTip(this.chk_selectall, "Check for Select All");
            this.chk_selectall.UseVisualStyleBackColor = true;
            this.chk_selectall.CheckedChanged += new System.EventHandler(this.chk_selectall_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(741, 239);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(38, 24);
            this.btnCancel.TabIndex = 30;
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel (Ctrl+Z)");
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(699, 239);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(38, 24);
            this.btnOk.TabIndex = 29;
            this.toolTip1.SetToolTip(this.btnOk, "Ok (Ctrl+O)");
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // frmSendEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 266);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chk_selectall);
            this.Controls.Add(this.chklst_email);
            this.Name = "frmSendEmail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send Email";
            this.Load += new System.EventHandler(this.frmSendEmail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSendEmail_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView chklst_email;
        private System.Windows.Forms.ColumnHeader colhdr_trantyp;
        private System.Windows.Forms.ColumnHeader colhdr_name;
        private System.Windows.Forms.CheckBox chk_selectall;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}