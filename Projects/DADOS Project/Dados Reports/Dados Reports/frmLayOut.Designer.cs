namespace DadosReports
{
    partial class frmLayOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLayOut));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLayout = new System.Windows.Forms.Button();
            this.chkIsDefault = new System.Windows.Forms.CheckBox();
            this.txtLayoutName = new System.Windows.Forms.TextBox();
            this.lblIsDefault = new System.Windows.Forms.Label();
            this.lblLayoutName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnLayout);
            this.panel1.Controls.Add(this.chkIsDefault);
            this.panel1.Controls.Add(this.txtLayoutName);
            this.panel1.Controls.Add(this.lblIsDefault);
            this.panel1.Controls.Add(this.lblLayoutName);
            this.panel1.Location = new System.Drawing.Point(5, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 116);
            this.panel1.TabIndex = 0;
            // 
            // btnLayout
            // 
            this.btnLayout.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLayout.Image = global::DadosReports.Properties.Resources.search;
            this.btnLayout.Location = new System.Drawing.Point(278, 8);
            this.btnLayout.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnLayout.Name = "btnLayout";
            this.btnLayout.Size = new System.Drawing.Size(26, 25);
            this.btnLayout.TabIndex = 4;
            this.btnLayout.UseVisualStyleBackColor = true;
            this.btnLayout.Click += new System.EventHandler(this.btnLayout_Click);
            // 
            // chkIsDefault
            // 
            this.chkIsDefault.AutoSize = true;
            this.chkIsDefault.Location = new System.Drawing.Point(114, 45);
            this.chkIsDefault.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkIsDefault.Name = "chkIsDefault";
            this.chkIsDefault.Size = new System.Drawing.Size(15, 14);
            this.chkIsDefault.TabIndex = 3;
            this.chkIsDefault.UseVisualStyleBackColor = true;
            // 
            // txtLayoutName
            // 
            this.txtLayoutName.Location = new System.Drawing.Point(114, 11);
            this.txtLayoutName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtLayoutName.Name = "txtLayoutName";
            this.txtLayoutName.ReadOnly = true;
            this.txtLayoutName.Size = new System.Drawing.Size(162, 20);
            this.txtLayoutName.TabIndex = 2;
            // 
            // lblIsDefault
            // 
            this.lblIsDefault.AutoSize = true;
            this.lblIsDefault.Location = new System.Drawing.Point(8, 43);
            this.lblIsDefault.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIsDefault.Name = "lblIsDefault";
            this.lblIsDefault.Size = new System.Drawing.Size(93, 14);
            this.lblIsDefault.TabIndex = 1;
            this.lblIsDefault.Text = "Save as Default ?";
            // 
            // lblLayoutName
            // 
            this.lblLayoutName.AutoSize = true;
            this.lblLayoutName.Location = new System.Drawing.Point(8, 15);
            this.lblLayoutName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLayoutName.Name = "lblLayoutName";
            this.lblLayoutName.Size = new System.Drawing.Size(70, 14);
            this.lblLayoutName.TabIndex = 0;
            this.lblLayoutName.Text = "Layout Name";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnNew);
            this.panel2.Location = new System.Drawing.Point(5, 87);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(316, 35);
            this.panel2.TabIndex = 1;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(73, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(57, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(243, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(57, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(187, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(130, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(57, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(16, 7);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(57, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // frmLayOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 126);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLayOut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Layouts";
            this.Load += new System.EventHandler(this.frmLayOut_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLayOut_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkIsDefault;
        private System.Windows.Forms.TextBox txtLayoutName;
        private System.Windows.Forms.Label lblIsDefault;
        private System.Windows.Forms.Label lblLayoutName;
        private System.Windows.Forms.Button btnLayout;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
    }
}