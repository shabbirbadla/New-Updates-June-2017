namespace UdEmpLeaveApproval
{
    partial class frmEmpLeaveApproval
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
            this.dgvApproval = new System.Windows.Forms.DataGridView();
            this.btnShowRec = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEmpNm = new System.Windows.Forms.Button();
            this.txtEmpNm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAppBy = new System.Windows.Forms.Button();
            this.txtApprNm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnStatusUp = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApproval)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvApproval
            // 
            this.dgvApproval.AllowUserToAddRows = false;
            this.dgvApproval.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvApproval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApproval.Location = new System.Drawing.Point(12, 139);
            this.dgvApproval.Name = "dgvApproval";
            this.dgvApproval.RowHeadersVisible = false;
            this.dgvApproval.Size = new System.Drawing.Size(1173, 304);
            this.dgvApproval.TabIndex = 3;
            // 
            // btnShowRec
            // 
            this.btnShowRec.Location = new System.Drawing.Point(100, 15);
            this.btnShowRec.Name = "btnShowRec";
            this.btnShowRec.Size = new System.Drawing.Size(89, 23);
            this.btnShowRec.TabIndex = 7;
            this.btnShowRec.Text = "Show Record";
            this.btnShowRec.UseVisualStyleBackColor = true;
            this.btnShowRec.Click += new System.EventHandler(this.btnShowRec_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnShowRec);
            this.groupBox1.Controls.Add(this.chkSelectAll);
            this.groupBox1.Location = new System.Drawing.Point(13, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 47);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(210, 15);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Save";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Enabled = false;
            this.chkSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(6, 19);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 0;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.Click += new System.EventHandler(this.chkSelectAll_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnEmpNm);
            this.groupBox2.Controls.Add(this.txtEmpNm);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnAppBy);
            this.groupBox2.Controls.Add(this.txtApprNm);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 77);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            // 
            // btnEmpNm
            // 
            this.btnEmpNm.Location = new System.Drawing.Point(423, 41);
            this.btnEmpNm.Name = "btnEmpNm";
            this.btnEmpNm.Size = new System.Drawing.Size(28, 23);
            this.btnEmpNm.TabIndex = 12;
            this.btnEmpNm.UseVisualStyleBackColor = true;
            this.btnEmpNm.Click += new System.EventHandler(this.btnEmpNm_Click_1);
            // 
            // txtEmpNm
            // 
            this.txtEmpNm.Location = new System.Drawing.Point(99, 42);
            this.txtEmpNm.Name = "txtEmpNm";
            this.txtEmpNm.ReadOnly = true;
            this.txtEmpNm.Size = new System.Drawing.Size(320, 20);
            this.txtEmpNm.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Employee Name";
            // 
            // btnAppBy
            // 
            this.btnAppBy.Location = new System.Drawing.Point(422, 13);
            this.btnAppBy.Name = "btnAppBy";
            this.btnAppBy.Size = new System.Drawing.Size(28, 23);
            this.btnAppBy.TabIndex = 9;
            this.btnAppBy.UseVisualStyleBackColor = true;
            this.btnAppBy.Click += new System.EventHandler(this.btnAppBy_Click_1);
            // 
            // txtApprNm
            // 
            this.txtApprNm.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtApprNm.Location = new System.Drawing.Point(99, 14);
            this.txtApprNm.Name = "txtApprNm";
            this.txtApprNm.ReadOnly = true;
            this.txtApprNm.Size = new System.Drawing.Size(320, 20);
            this.txtApprNm.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Approved By";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnStatusUp);
            this.groupBox3.Controls.Add(this.cmbStatus);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(323, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 45);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bulk Update";
            // 
            // btnStatusUp
            // 
            this.btnStatusUp.Enabled = false;
            this.btnStatusUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatusUp.Location = new System.Drawing.Point(203, 14);
            this.btnStatusUp.Name = "btnStatusUp";
            this.btnStatusUp.Size = new System.Drawing.Size(75, 23);
            this.btnStatusUp.TabIndex = 3;
            this.btnStatusUp.Text = "Update";
            this.btnStatusUp.UseVisualStyleBackColor = true;
            this.btnStatusUp.Click += new System.EventHandler(this.btnStatusUp_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.Enabled = false;
            this.cmbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(83, 16);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(109, 21);
            this.cmbStatus.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Select Status";
            // 
            // frmEmpLeaveApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 444);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvApproval);
            this.MaximizeBox = false;
            this.Name = "frmEmpLeaveApproval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmEmpLeaveApproval_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmpLeaveApproval_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApproval)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvApproval;
        private System.Windows.Forms.Button btnShowRec;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnEmpNm;
        private System.Windows.Forms.TextBox txtEmpNm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAppBy;
        private System.Windows.Forms.TextBox txtApprNm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStatusUp;
    }
}

