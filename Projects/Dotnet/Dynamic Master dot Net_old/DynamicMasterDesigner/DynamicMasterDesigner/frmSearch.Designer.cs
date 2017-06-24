namespace DynamicMasterDesigner
{
    partial class frmSearch
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxDefaultSearch = new System.Windows.Forms.CheckBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.gridFormList = new DevExpress.XtraGrid.GridControl();
            this.gridViewFormList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabelFilterColumn = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFormList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFormList)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxDefaultSearch);
            this.panel1.Controls.Add(this.textBoxSearch);
            this.panel1.Controls.Add(this.lblSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(755, 60);
            this.panel1.TabIndex = 0;
            // 
            // checkBoxDefaultSearch
            // 
            this.checkBoxDefaultSearch.AutoSize = true;
            this.checkBoxDefaultSearch.Enabled = false;
            this.checkBoxDefaultSearch.Location = new System.Drawing.Point(59, 36);
            this.checkBoxDefaultSearch.Name = "checkBoxDefaultSearch";
            this.checkBoxDefaultSearch.Size = new System.Drawing.Size(97, 17);
            this.checkBoxDefaultSearch.TabIndex = 2;
            this.checkBoxDefaultSearch.Text = "Default Search";
            this.checkBoxDefaultSearch.UseVisualStyleBackColor = true;
            this.checkBoxDefaultSearch.CheckedChanged += new System.EventHandler(this.checkBoxDefaultSearch_CheckedChanged);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(58, 9);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(685, 20);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            this.textBoxSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSearch_KeyPress);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(11, 12);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(47, 13);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search: ";
            // 
            // gridFormList
            // 
            this.gridFormList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFormList.Location = new System.Drawing.Point(0, 60);
            this.gridFormList.MainView = this.gridViewFormList;
            this.gridFormList.Name = "gridFormList";
            this.gridFormList.Size = new System.Drawing.Size(755, 278);
            this.gridFormList.TabIndex = 1;
            this.gridFormList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewFormList});
            this.gridFormList.Click += new System.EventHandler(this.gridFormList_Click);
            // 
            // gridViewFormList
            // 
            this.gridViewFormList.GridControl = this.gridFormList;
            this.gridViewFormList.Name = "gridViewFormList";
            this.gridViewFormList.OptionsBehavior.Editable = false;
            this.gridViewFormList.OptionsCustomization.AllowFilter = false;
            this.gridViewFormList.OptionsCustomization.AllowGroup = false;
            this.gridViewFormList.OptionsFilter.AllowFilterEditor = false;
            this.gridViewFormList.DoubleClick += new System.EventHandler(this.gridViewFormList_DoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelFilterColumn});
            this.statusStrip1.Location = new System.Drawing.Point(0, 316);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(755, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabelFilterColumn
            // 
            this.statusLabelFilterColumn.Name = "statusLabelFilterColumn";
            this.statusLabelFilterColumn.Size = new System.Drawing.Size(78, 17);
            this.statusLabelFilterColumn.Text = "Filter Columns:";
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 338);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gridFormList);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSearch";
            this.ShowInTaskbar = false;
            this.Text = "Search";
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFormList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFormList)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridFormList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewFormList;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox checkBoxDefaultSearch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelFilterColumn;
    }
}