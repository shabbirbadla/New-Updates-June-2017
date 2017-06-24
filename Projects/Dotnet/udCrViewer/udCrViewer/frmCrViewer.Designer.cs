namespace udCrViewer
{
    partial class frmCrViewer
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
            this.CrViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // CrViewer
            // 
            this.CrViewer.ActiveViewIndex = -1;
            this.CrViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrViewer.DisplayGroupTree = false;
            this.CrViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrViewer.Location = new System.Drawing.Point(0, 0);
            this.CrViewer.Name = "CrViewer";
            this.CrViewer.SelectionFormula = "";
            this.CrViewer.ShowGroupTreeButton = false;
            this.CrViewer.Size = new System.Drawing.Size(892, 575);
            this.CrViewer.TabIndex = 0;
            this.CrViewer.ViewTimeSelectionFormula = "";
            this.CrViewer.Load += new System.EventHandler(this.crystalReportViewer_Load);
            // 
            // frmCrViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 575);
            this.Controls.Add(this.CrViewer);
            this.Name = "frmCrViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrViewer;
    }
}

