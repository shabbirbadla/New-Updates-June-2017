namespace ueItemRateUpdtae
{
    partial class frmItemDetailsMain
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblGroup = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtItGroup = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblSelect = new System.Windows.Forms.Label();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.chkMrp = new System.Windows.Forms.CheckBox();
            this.chkAbt = new System.Windows.Forms.CheckBox();
            this.txtMrp = new uNumericTextBox.cNumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAbtPer = new uNumericTextBox.cNumericTextBox();
            this.chkSalesRate = new System.Windows.Forms.CheckBox();
            this.chkPurRate = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSales = new System.Windows.Forms.Label();
            this.lblPurchase = new System.Windows.Forms.Label();
            this.txtSalesRate = new uNumericTextBox.cNumericTextBox();
            this.txtPurRate = new uNumericTextBox.cNumericTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 136);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(754, 265);
            this.dataGridView1.TabIndex = 0;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroup.Location = new System.Drawing.Point(20, 19);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(41, 14);
            this.lblGroup.TabIndex = 6;
            this.lblGroup.Text = "Group";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(655, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(28, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtItGroup
            // 
            this.txtItGroup.Location = new System.Drawing.Point(73, 19);
            this.txtItGroup.Name = "txtItGroup";
            this.txtItGroup.Size = new System.Drawing.Size(548, 20);
            this.txtItGroup.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(653, 96);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdate.Location = new System.Drawing.Point(653, 71);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelect.Location = new System.Drawing.Point(41, 105);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(58, 14);
            this.lblSelect.TabIndex = 38;
            this.lblSelect.Text = "Select All";
            // 
            // chkSelect
            // 
            this.chkSelect.AutoSize = true;
            this.chkSelect.Location = new System.Drawing.Point(23, 105);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(15, 14);
            this.chkSelect.TabIndex = 37;
            this.chkSelect.UseVisualStyleBackColor = true;
            this.chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
            // 
            // chkMrp
            // 
            this.chkMrp.AutoSize = true;
            this.chkMrp.Location = new System.Drawing.Point(519, 77);
            this.chkMrp.Name = "chkMrp";
            this.chkMrp.Size = new System.Drawing.Size(15, 14);
            this.chkMrp.TabIndex = 8;
            this.chkMrp.UseVisualStyleBackColor = true;
            this.chkMrp.CheckedChanged += new System.EventHandler(this.chkMrp_CheckedChanged);
            // 
            // chkAbt
            // 
            this.chkAbt.AutoSize = true;
            this.chkAbt.Location = new System.Drawing.Point(367, 77);
            this.chkAbt.Name = "chkAbt";
            this.chkAbt.Size = new System.Drawing.Size(15, 14);
            this.chkAbt.TabIndex = 6;
            this.chkAbt.UseVisualStyleBackColor = true;
            this.chkAbt.CheckedChanged += new System.EventHandler(this.chkAbt_CheckedChanged);
            // 
            // txtMrp
            // 
            this.txtMrp.Location = new System.Drawing.Point(540, 74);
            this.txtMrp.MaxLength = 11;
            this.txtMrp.Name = "txtMrp";
            this.txtMrp.Size = new System.Drawing.Size(62, 20);
            this.txtMrp.TabIndex = 9;
            this.txtMrp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMrp_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(542, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 14);
            this.label2.TabIndex = 33;
            this.label2.Text = "MRP Rate\r\n";
            // 
            // txtAbtPer
            // 
            this.txtAbtPer.Location = new System.Drawing.Point(388, 74);
            this.txtAbtPer.MaxLength = 7;
            this.txtAbtPer.Name = "txtAbtPer";
            this.txtAbtPer.Size = new System.Drawing.Size(62, 20);
            this.txtAbtPer.TabIndex = 7;
            this.txtAbtPer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAbtPer_KeyPress);
            // 
            // chkSalesRate
            // 
            this.chkSalesRate.AutoSize = true;
            this.chkSalesRate.Location = new System.Drawing.Point(63, 77);
            this.chkSalesRate.Name = "chkSalesRate";
            this.chkSalesRate.Size = new System.Drawing.Size(15, 14);
            this.chkSalesRate.TabIndex = 2;
            this.chkSalesRate.UseVisualStyleBackColor = true;
            this.chkSalesRate.CheckedChanged += new System.EventHandler(this.chkSalesRate_CheckedChanged);
            // 
            // chkPurRate
            // 
            this.chkPurRate.AutoSize = true;
            this.chkPurRate.Location = new System.Drawing.Point(215, 77);
            this.chkPurRate.Name = "chkPurRate";
            this.chkPurRate.Size = new System.Drawing.Size(15, 14);
            this.chkPurRate.TabIndex = 4;
            this.chkPurRate.UseVisualStyleBackColor = true;
            this.chkPurRate.CheckedChanged += new System.EventHandler(this.chkPurRate_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(400, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 14);
            this.label1.TabIndex = 29;
            this.label1.Text = "Abt %";
            // 
            // lblSales
            // 
            this.lblSales.AutoSize = true;
            this.lblSales.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSales.Location = new System.Drawing.Point(83, 57);
            this.lblSales.Name = "lblSales";
            this.lblSales.Size = new System.Drawing.Size(64, 14);
            this.lblSales.TabIndex = 28;
            this.lblSales.Text = "Sales Rate\r\n";
            // 
            // lblPurchase
            // 
            this.lblPurchase.AutoSize = true;
            this.lblPurchase.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPurchase.Location = new System.Drawing.Point(224, 57);
            this.lblPurchase.Name = "lblPurchase";
            this.lblPurchase.Size = new System.Drawing.Size(86, 14);
            this.lblPurchase.TabIndex = 27;
            this.lblPurchase.Text = "Purchase Rate";
            // 
            // txtSalesRate
            // 
            this.txtSalesRate.Location = new System.Drawing.Point(84, 74);
            this.txtSalesRate.MaxLength = 7;
            this.txtSalesRate.Name = "txtSalesRate";
            this.txtSalesRate.Size = new System.Drawing.Size(62, 20);
            this.txtSalesRate.TabIndex = 3;
            this.txtSalesRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesRate_KeyPress);
            // 
            // txtPurRate
            // 
            this.txtPurRate.Location = new System.Drawing.Point(236, 74);
            this.txtPurRate.MaxLength = 7;
            this.txtPurRate.Name = "txtPurRate";
            this.txtPurRate.Size = new System.Drawing.Size(62, 20);
            this.txtPurRate.TabIndex = 5;
            this.txtPurRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPurRate_KeyPress);
            // 
            // frmItemDetailsMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 401);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.chkMrp);
            this.Controls.Add(this.chkAbt);
            this.Controls.Add(this.txtMrp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAbtPer);
            this.Controls.Add(this.chkSalesRate);
            this.Controls.Add(this.chkPurRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSales);
            this.Controls.Add(this.lblPurchase);
            this.Controls.Add(this.txtSalesRate);
            this.Controls.Add(this.txtPurRate);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtItGroup);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmItemDetailsMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmItemDetailsMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmItemDetailsMain_FormClosed_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtItGroup;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.CheckBox chkMrp;
        private System.Windows.Forms.CheckBox chkAbt;
        private uNumericTextBox.cNumericTextBox txtMrp;
        private System.Windows.Forms.Label label2;
        private uNumericTextBox.cNumericTextBox txtAbtPer;
        private System.Windows.Forms.CheckBox chkSalesRate;
        private System.Windows.Forms.CheckBox chkPurRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSales;
        private System.Windows.Forms.Label lblPurchase;
        private uNumericTextBox.cNumericTextBox txtSalesRate;
        private uNumericTextBox.cNumericTextBox txtPurRate;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

