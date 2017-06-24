namespace udPdfSignature
{
    partial class frmPdfSign
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
            this.SuspendLayout();
            // 
            // frmPdfSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "frmPdfSign";
            this.Opacity = 0D;
            this.Text = "Pdf Signature";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPdfSign_FormClosed);
            this.Load += new System.EventHandler(this.frmPdfSign_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

