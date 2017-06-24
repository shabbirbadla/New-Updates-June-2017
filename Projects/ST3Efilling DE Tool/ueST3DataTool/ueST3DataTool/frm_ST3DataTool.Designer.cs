namespace ueST3DataTool
{
    partial class frm_ST3DataTool
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pgb = new System.Windows.Forms.ToolStripProgressBar();
            this.lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnFPath = new System.Windows.Forms.Button();
            this.txtFPath = new System.Windows.Forms.TextBox();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnFGen = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pgb,
            this.lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 123);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(527, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pgb
            // 
            this.pgb.Name = "pgb";
            this.pgb.Size = new System.Drawing.Size(100, 16);
            // 
            // lbl
            // 
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(118, 17);
            this.lbl.Text = "toolStripStatusLabel1";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnFPath);
            this.panel2.Controls.Add(this.txtFPath);
            this.panel2.Controls.Add(this.cmbPeriod);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmbFYear);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(516, 78);
            this.panel2.TabIndex = 15;
            // 
            // btnFPath
            // 
            this.btnFPath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFPath.Location = new System.Drawing.Point(458, 53);
            this.btnFPath.Name = "btnFPath";
            this.btnFPath.Size = new System.Drawing.Size(26, 20);
            this.btnFPath.TabIndex = 13;
            this.btnFPath.UseVisualStyleBackColor = true;
            this.btnFPath.Click += new System.EventHandler(this.btnFPath_Click);
            // 
            // txtFPath
            // 
            this.txtFPath.Location = new System.Drawing.Point(134, 53);
            this.txtFPath.Name = "txtFPath";
            this.txtFPath.Size = new System.Drawing.Size(324, 20);
            this.txtFPath.TabIndex = 12;
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Location = new System.Drawing.Point(134, 5);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(114, 21);
            this.cmbPeriod.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Financial Year";
            // 
            // cmbFYear
            // 
            this.cmbFYear.Enabled = false;
            this.cmbFYear.FormattingEnabled = true;
            this.cmbFYear.Location = new System.Drawing.Point(134, 29);
            this.cmbFYear.Name = "cmbFYear";
            this.cmbFYear.Size = new System.Drawing.Size(114, 21);
            this.cmbFYear.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Save File To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Period";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnFGen);
            this.panel3.Location = new System.Drawing.Point(6, 90);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(516, 24);
            this.panel3.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Location = new System.Drawing.Point(438, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnFGen
            // 
            this.btnFGen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFGen.Location = new System.Drawing.Point(324, 0);
            this.btnFGen.Name = "btnFGen";
            this.btnFGen.Size = new System.Drawing.Size(105, 23);
            this.btnFGen.TabIndex = 10;
            this.btnFGen.Text = "Generate ZIP";
            this.btnFGen.UseVisualStyleBackColor = true;
            this.btnFGen.Click += new System.EventHandler(this.btnFGen_Click);
            // 
            // frm_ST3DataTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 145);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_ST3DataTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ST-3 XML zip file Generation";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar pgb;
        private System.Windows.Forms.ToolStripStatusLabel lbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnFPath;
        private System.Windows.Forms.TextBox txtFPath;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbFYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnFGen;

    }
}

