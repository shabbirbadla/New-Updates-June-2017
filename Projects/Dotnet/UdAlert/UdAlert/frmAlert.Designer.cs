namespace UdAlert
{
    partial class frmAlert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlert));
            this.chkAlert1 = new System.Windows.Forms.CheckBox();
            this.lblAlName = new System.Windows.Forms.Label();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.txtLastUpdated = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAlertDesc = new System.Windows.Forms.TextBox();
            this.txtAlertName = new System.Windows.Forms.TextBox();
            this.lblAlertDescription = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAlert1
            // 
            this.chkAlert1.AutoSize = true;
            this.chkAlert1.Location = new System.Drawing.Point(378, 92);
            this.chkAlert1.Name = "chkAlert1";
            this.chkAlert1.Size = new System.Drawing.Size(114, 17);
            this.chkAlert1.TabIndex = 25;
            this.chkAlert1.Text = "Do Not Show Alert";
            this.chkAlert1.UseVisualStyleBackColor = true;
            // 
            // lblAlName
            // 
            this.lblAlName.AutoSize = true;
            this.lblAlName.BackColor = System.Drawing.Color.Transparent;
            this.lblAlName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlName.Location = new System.Drawing.Point(25, 21);
            this.lblAlName.Name = "lblAlName";
            this.lblAlName.Size = new System.Drawing.Size(62, 13);
            this.lblAlName.TabIndex = 24;
            this.lblAlName.Text = "Alert_Name";
            // 
            // btnLast
            // 
            this.btnLast.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLast.BackgroundImage")));
            this.btnLast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLast.Location = new System.Drawing.Point(524, 87);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(30, 23);
            this.btnLast.TabIndex = 23;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBack.BackgroundImage")));
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.Location = new System.Drawing.Point(524, 39);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(30, 23);
            this.btnBack.TabIndex = 22;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNext.BackgroundImage")));
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.Location = new System.Drawing.Point(524, 63);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 23);
            this.btnNext.TabIndex = 21;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.BackgroundImage")));
            this.btnFirst.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFirst.Location = new System.Drawing.Point(524, 15);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(30, 23);
            this.btnFirst.TabIndex = 20;
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // txtLastUpdated
            // 
            this.txtLastUpdated.Location = new System.Drawing.Point(131, 90);
            this.txtLastUpdated.Name = "txtLastUpdated";
            this.txtLastUpdated.Size = new System.Drawing.Size(241, 20);
            this.txtLastUpdated.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Last_Updated";
            // 
            // txtAlertDesc
            // 
            this.txtAlertDesc.Location = new System.Drawing.Point(131, 41);
            this.txtAlertDesc.Multiline = true;
            this.txtAlertDesc.Name = "txtAlertDesc";
            this.txtAlertDesc.Size = new System.Drawing.Size(361, 46);
            this.txtAlertDesc.TabIndex = 17;
            // 
            // txtAlertName
            // 
            this.txtAlertName.Location = new System.Drawing.Point(131, 18);
            this.txtAlertName.Name = "txtAlertName";
            this.txtAlertName.Size = new System.Drawing.Size(361, 20);
            this.txtAlertName.TabIndex = 16;
            // 
            // lblAlertDescription
            // 
            this.lblAlertDescription.AutoSize = true;
            this.lblAlertDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblAlertDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlertDescription.Location = new System.Drawing.Point(25, 61);
            this.lblAlertDescription.Name = "lblAlertDescription";
            this.lblAlertDescription.Size = new System.Drawing.Size(87, 13);
            this.lblAlertDescription.TabIndex = 15;
            this.lblAlertDescription.Text = "Alert_Description";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 124);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(663, 223);
            this.dataGridView1.TabIndex = 26;
            // 
            // frmAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(663, 347);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.chkAlert1);
            this.Controls.Add(this.lblAlName);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.txtLastUpdated);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAlertDesc);
            this.Controls.Add(this.txtAlertName);
            this.Controls.Add(this.lblAlertDescription);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "frmAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmAlert_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAlert_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAlert1;
        private System.Windows.Forms.Label lblAlName;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
        public System.Windows.Forms.TextBox txtLastUpdated;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtAlertDesc;
        public System.Windows.Forms.TextBox txtAlertName;
        private System.Windows.Forms.Label lblAlertDescription;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

