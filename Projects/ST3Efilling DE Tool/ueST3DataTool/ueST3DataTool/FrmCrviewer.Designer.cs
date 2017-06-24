namespace ueST3DataTool
{
    partial class FrmCrviewer
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
            this.crviewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crviewer
            // 
            this.crviewer.ActiveViewIndex = -1;
            this.crviewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crviewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crviewer.Location = new System.Drawing.Point(0, 0);
            this.crviewer.Name = "crviewer";
            this.crviewer.SelectionFormula = "";
            this.crviewer.Size = new System.Drawing.Size(284, 264);
            this.crviewer.TabIndex = 1;
            this.crviewer.ViewTimeSelectionFormula = "";
            // 
            // FrmCrviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.crviewer);
            this.Name = "FrmCrviewer";
            this.Text = "FrmCrviewer";
            this.Load += new System.EventHandler(this.FrmCrviewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crviewer;
    }
}