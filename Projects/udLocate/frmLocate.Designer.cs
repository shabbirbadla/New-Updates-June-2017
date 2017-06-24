namespace udLocate
{
    partial class frmLocate
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.dvxGrdLocate = new DevExpress.XtraGrid.GridControl();
            this.gridview1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCELXLSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rTFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dvxGrdLocate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvxGrdLocate
            // 
            gridLevelNode1.RelationName = "Level1";
            this.dvxGrdLocate.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.dvxGrdLocate.Location = new System.Drawing.Point(2, 22);
            this.dvxGrdLocate.MainView = this.gridview1;
            this.dvxGrdLocate.Name = "dvxGrdLocate";
            this.dvxGrdLocate.Size = new System.Drawing.Size(805, 467);
            this.dvxGrdLocate.TabIndex = 0;
            this.dvxGrdLocate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridview1});
            // 
            // gridview1
            // 
            this.gridview1.GridControl = this.dvxGrdLocate;
            this.gridview1.Name = "gridview1";
            this.gridview1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridview1_KeyPress);
            this.gridview1.DoubleClick += new System.EventHandler(this.gridview1_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(730, -1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelToolStripMenuItem,
            this.pdfToolStripMenuItem,
            this.eXCELXLSToolStripMenuItem,
            this.rTFToolStripMenuItem,
            this.hTMLToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(142, 114);
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.excelToolStripMenuItem.Text = "EXCEL(XLS)";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // pdfToolStripMenuItem
            // 
            this.pdfToolStripMenuItem.Name = "pdfToolStripMenuItem";
            this.pdfToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.pdfToolStripMenuItem.Text = "EXCEL(XLSX)";
            this.pdfToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // eXCELXLSToolStripMenuItem
            // 
            this.eXCELXLSToolStripMenuItem.Name = "eXCELXLSToolStripMenuItem";
            this.eXCELXLSToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.eXCELXLSToolStripMenuItem.Text = "PDF";
            this.eXCELXLSToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // rTFToolStripMenuItem
            // 
            this.rTFToolStripMenuItem.Name = "rTFToolStripMenuItem";
            this.rTFToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.rTFToolStripMenuItem.Text = "RTF";
            this.rTFToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // hTMLToolStripMenuItem
            // 
            this.hTMLToolStripMenuItem.Name = "hTMLToolStripMenuItem";
            this.hTMLToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.hTMLToolStripMenuItem.Text = "HTML";
            this.hTMLToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // frmLocate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(808, 488);
            this.Controls.Add(this.dvxGrdLocate);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmLocate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmLocate_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLocate_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dvxGrdLocate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl dvxGrdLocate;
        private DevExpress.XtraGrid.Views.Grid.GridView gridview1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXCELXLSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rTFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hTMLToolStripMenuItem;

    }
}

