namespace UeSMS
{
    partial class frmParamValue
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
            this.lblParamNm = new System.Windows.Forms.Label();
            this.txtParamNm = new System.Windows.Forms.TextBox();
            this.lblParamDesc = new System.Windows.Forms.Label();
            this.txtParamDesc = new System.Windows.Forms.TextBox();
            this.lblParamVal = new System.Windows.Forms.Label();
            this.txtParamVal = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblParamNm
            // 
            this.lblParamNm.AutoSize = true;
            this.lblParamNm.Location = new System.Drawing.Point(20, 19);
            this.lblParamNm.Name = "lblParamNm";
            this.lblParamNm.Size = new System.Drawing.Size(86, 13);
            this.lblParamNm.TabIndex = 0;
            this.lblParamNm.Text = "Parameter Name";
            // 
            // txtParamNm
            // 
            this.txtParamNm.BackColor = System.Drawing.SystemColors.Control;
            this.txtParamNm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParamNm.Enabled = false;
            this.txtParamNm.Location = new System.Drawing.Point(134, 16);
            this.txtParamNm.Name = "txtParamNm";
            this.txtParamNm.Size = new System.Drawing.Size(271, 20);
            this.txtParamNm.TabIndex = 1;
            // 
            // lblParamDesc
            // 
            this.lblParamDesc.AutoSize = true;
            this.lblParamDesc.Location = new System.Drawing.Point(20, 45);
            this.lblParamDesc.Name = "lblParamDesc";
            this.lblParamDesc.Size = new System.Drawing.Size(111, 13);
            this.lblParamDesc.TabIndex = 0;
            this.lblParamDesc.Text = "Parameter Description";
            // 
            // txtParamDesc
            // 
            this.txtParamDesc.BackColor = System.Drawing.SystemColors.Control;
            this.txtParamDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParamDesc.Enabled = false;
            this.txtParamDesc.Location = new System.Drawing.Point(134, 42);
            this.txtParamDesc.Name = "txtParamDesc";
            this.txtParamDesc.Size = new System.Drawing.Size(271, 20);
            this.txtParamDesc.TabIndex = 1;
            // 
            // lblParamVal
            // 
            this.lblParamVal.AutoSize = true;
            this.lblParamVal.Location = new System.Drawing.Point(20, 71);
            this.lblParamVal.Name = "lblParamVal";
            this.lblParamVal.Size = new System.Drawing.Size(85, 13);
            this.lblParamVal.TabIndex = 0;
            this.lblParamVal.Text = "Parameter Value";
            // 
            // txtParamVal
            // 
            this.txtParamVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParamVal.Location = new System.Drawing.Point(134, 68);
            this.txtParamVal.Name = "txtParamVal";
            this.txtParamVal.Size = new System.Drawing.Size(271, 20);
            this.txtParamVal.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(329, 111);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlOuter
            // 
            this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOuter.Controls.Add(this.panel1);
            this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuter.Location = new System.Drawing.Point(0, 0);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(425, 141);
            this.pnlOuter.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(-1, 103);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(424, 1);
            this.panel1.TabIndex = 0;
            // 
            // frmParamValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(425, 141);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtParamVal);
            this.Controls.Add(this.lblParamVal);
            this.Controls.Add(this.txtParamDesc);
            this.Controls.Add(this.lblParamDesc);
            this.Controls.Add(this.txtParamNm);
            this.Controls.Add(this.lblParamNm);
            this.Controls.Add(this.pnlOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmParamValue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmParamValue_Load);
            this.pnlOuter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblParamNm;
        private System.Windows.Forms.TextBox txtParamNm;
        private System.Windows.Forms.Label lblParamDesc;
        private System.Windows.Forms.TextBox txtParamDesc;
        private System.Windows.Forms.Label lblParamVal;
        private System.Windows.Forms.TextBox txtParamVal;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Panel panel1;
    }
}
