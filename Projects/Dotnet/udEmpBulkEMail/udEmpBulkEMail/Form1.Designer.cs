namespace udEmpBulkEMail
{
    partial class Form1
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
            this.btnExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.rtxtEmailBody = new System.Windows.Forms.RichTextBox();
            this.btnEMail = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtxtSubject = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(519, 7);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(28, 23);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select File";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(74, 9);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(439, 20);
            this.txtFileName.TabIndex = 2;
            // 
            // rtxtEmailBody
            // 
            this.rtxtEmailBody.Location = new System.Drawing.Point(74, 83);
            this.rtxtEmailBody.Name = "rtxtEmailBody";
            this.rtxtEmailBody.Size = new System.Drawing.Size(617, 122);
            this.rtxtEmailBody.TabIndex = 3;
            this.rtxtEmailBody.Text = "";
            this.rtxtEmailBody.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // btnEMail
            // 
            this.btnEMail.Location = new System.Drawing.Point(616, 7);
            this.btnEMail.Name = "btnEMail";
            this.btnEMail.Size = new System.Drawing.Size(75, 23);
            this.btnEMail.TabIndex = 4;
            this.btnEMail.Text = "Send E-Mail";
            this.btnEMail.UseVisualStyleBackColor = true;
            this.btnEMail.Click += new System.EventHandler(this.btnEMail_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Email Body";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Subject";
            // 
            // rtxtSubject
            // 
            this.rtxtSubject.Location = new System.Drawing.Point(74, 35);
            this.rtxtSubject.Name = "rtxtSubject";
            this.rtxtSubject.Size = new System.Drawing.Size(617, 42);
            this.rtxtSubject.TabIndex = 7;
            this.rtxtSubject.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 207);
            this.Controls.Add(this.rtxtSubject);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEMail);
            this.Controls.Add(this.rtxtEmailBody);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExcel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.RichTextBox rtxtEmailBody;
        private System.Windows.Forms.Button btnEMail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtxtSubject;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

