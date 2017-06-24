namespace eMailClient
{
    partial class frmExportCSV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportCSV));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.rbtnOther = new System.Windows.Forms.RadioButton();
            this.rbtnTab = new System.Windows.Forms.RadioButton();
            this.rbtnSemicolon = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnUtf32 = new System.Windows.Forms.RadioButton();
            this.rbtnUtf8 = new System.Windows.Forms.RadioButton();
            this.rbtnUtf7 = new System.Windows.Forms.RadioButton();
            this.rbtnAscii = new System.Windows.Forms.RadioButton();
            this.rbtnUnicode = new System.Windows.Forms.RadioButton();
            this.chkFirstrow = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOther);
            this.groupBox1.Controls.Add(this.rbtnOther);
            this.groupBox1.Controls.Add(this.rbtnTab);
            this.groupBox1.Controls.Add(this.rbtnSemicolon);
            this.groupBox1.Location = new System.Drawing.Point(8, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 104);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Separator";
            // 
            // txtOther
            // 
            this.txtOther.BackColor = System.Drawing.Color.White;
            this.txtOther.Location = new System.Drawing.Point(110, 72);
            this.txtOther.MaxLength = 1;
            this.txtOther.Name = "txtOther";
            this.txtOther.ReadOnly = true;
            this.txtOther.Size = new System.Drawing.Size(51, 20);
            this.txtOther.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtOther, "Enter the separator other than semicolon & tab");
            this.txtOther.TextChanged += new System.EventHandler(this.txtOther_TextChanged);
            // 
            // rbtnOther
            // 
            this.rbtnOther.AutoSize = true;
            this.rbtnOther.Location = new System.Drawing.Point(44, 74);
            this.rbtnOther.Name = "rbtnOther";
            this.rbtnOther.Size = new System.Drawing.Size(51, 17);
            this.rbtnOther.TabIndex = 2;
            this.rbtnOther.Text = "Other";
            this.rbtnOther.UseVisualStyleBackColor = true;
            this.rbtnOther.CheckedChanged += new System.EventHandler(this.rbtnOther_CheckedChanged);
            // 
            // rbtnTab
            // 
            this.rbtnTab.AutoSize = true;
            this.rbtnTab.Location = new System.Drawing.Point(44, 50);
            this.rbtnTab.Name = "rbtnTab";
            this.rbtnTab.Size = new System.Drawing.Size(44, 17);
            this.rbtnTab.TabIndex = 1;
            this.rbtnTab.Text = "Tab";
            this.rbtnTab.UseVisualStyleBackColor = true;
            // 
            // rbtnSemicolon
            // 
            this.rbtnSemicolon.AutoSize = true;
            this.rbtnSemicolon.Location = new System.Drawing.Point(44, 26);
            this.rbtnSemicolon.Name = "rbtnSemicolon";
            this.rbtnSemicolon.Size = new System.Drawing.Size(74, 17);
            this.rbtnSemicolon.TabIndex = 0;
            this.rbtnSemicolon.Text = "Semicolon";
            this.rbtnSemicolon.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnUtf32);
            this.groupBox2.Controls.Add(this.rbtnUtf8);
            this.groupBox2.Controls.Add(this.rbtnUtf7);
            this.groupBox2.Controls.Add(this.rbtnAscii);
            this.groupBox2.Controls.Add(this.rbtnUnicode);
            this.groupBox2.Location = new System.Drawing.Point(8, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 148);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encoding";
            // 
            // rbtnUtf32
            // 
            this.rbtnUtf32.AutoSize = true;
            this.rbtnUtf32.Location = new System.Drawing.Point(44, 122);
            this.rbtnUtf32.Name = "rbtnUtf32";
            this.rbtnUtf32.Size = new System.Drawing.Size(58, 17);
            this.rbtnUtf32.TabIndex = 4;
            this.rbtnUtf32.Text = "UTF32";
            this.rbtnUtf32.UseVisualStyleBackColor = true;
            // 
            // rbtnUtf8
            // 
            this.rbtnUtf8.AutoSize = true;
            this.rbtnUtf8.Location = new System.Drawing.Point(44, 98);
            this.rbtnUtf8.Name = "rbtnUtf8";
            this.rbtnUtf8.Size = new System.Drawing.Size(52, 17);
            this.rbtnUtf8.TabIndex = 3;
            this.rbtnUtf8.Text = "UTF8";
            this.rbtnUtf8.UseVisualStyleBackColor = true;
            // 
            // rbtnUtf7
            // 
            this.rbtnUtf7.AutoSize = true;
            this.rbtnUtf7.Location = new System.Drawing.Point(44, 74);
            this.rbtnUtf7.Name = "rbtnUtf7";
            this.rbtnUtf7.Size = new System.Drawing.Size(52, 17);
            this.rbtnUtf7.TabIndex = 2;
            this.rbtnUtf7.Text = "UTF7";
            this.rbtnUtf7.UseVisualStyleBackColor = true;
            // 
            // rbtnAscii
            // 
            this.rbtnAscii.AutoSize = true;
            this.rbtnAscii.Location = new System.Drawing.Point(44, 50);
            this.rbtnAscii.Name = "rbtnAscii";
            this.rbtnAscii.Size = new System.Drawing.Size(52, 17);
            this.rbtnAscii.TabIndex = 1;
            this.rbtnAscii.Text = "ASCII";
            this.rbtnAscii.UseVisualStyleBackColor = true;
            // 
            // rbtnUnicode
            // 
            this.rbtnUnicode.AutoSize = true;
            this.rbtnUnicode.Location = new System.Drawing.Point(44, 26);
            this.rbtnUnicode.Name = "rbtnUnicode";
            this.rbtnUnicode.Size = new System.Drawing.Size(65, 17);
            this.rbtnUnicode.TabIndex = 0;
            this.rbtnUnicode.Text = "Unicode";
            this.rbtnUnicode.UseVisualStyleBackColor = true;
            // 
            // chkFirstrow
            // 
            this.chkFirstrow.AutoSize = true;
            this.chkFirstrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFirstrow.Location = new System.Drawing.Point(20, 269);
            this.chkFirstrow.Name = "chkFirstrow";
            this.chkFirstrow.Size = new System.Drawing.Size(153, 17);
            this.chkFirstrow.TabIndex = 2;
            this.chkFirstrow.Text = "First row has column names";
            this.toolTip1.SetToolTip(this.chkFirstrow, "Check if CSV requires column names as the first row");
            this.chkFirstrow.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(162, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(38, 24);
            this.btnCancel.TabIndex = 4;
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
            this.btnOk.Location = new System.Drawing.Point(118, 292);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(38, 24);
            this.btnOk.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnOk, "Ok (Ctrl+O)");
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmExportCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 320);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkFirstrow);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExportCSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export CSV";
            this.Load += new System.EventHandler(this.frmExportCSV_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmExportCSV_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnOther;
        private System.Windows.Forms.RadioButton rbtnTab;
        private System.Windows.Forms.RadioButton rbtnSemicolon;
        private System.Windows.Forms.TextBox txtOther;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnUtf7;
        private System.Windows.Forms.RadioButton rbtnAscii;
        private System.Windows.Forms.RadioButton rbtnUnicode;
        private System.Windows.Forms.RadioButton rbtnUtf8;
        private System.Windows.Forms.CheckBox chkFirstrow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton rbtnUtf32;
    }
}