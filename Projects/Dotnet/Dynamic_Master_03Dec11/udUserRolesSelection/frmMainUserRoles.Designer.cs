namespace udUserRolesSelection
{
    partial class frmMainUserRoles
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.repositoryItemTabLookUp = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemDatatypeLookup = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.grdMainvw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdMain = new DevExpress.XtraGrid.GridControl();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTabLookUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDatatypeLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMainvw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemTabLookUp
            // 
            this.repositoryItemTabLookUp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTabLookUp.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Tab Name")});
            this.repositoryItemTabLookUp.DisplayMember = "CODE";
            this.repositoryItemTabLookUp.DropDownRows = 10;
            this.repositoryItemTabLookUp.Name = "repositoryItemTabLookUp";
            this.repositoryItemTabLookUp.PopupWidth = 120;
            this.repositoryItemTabLookUp.ValueMember = "CODE";
            // 
            // repositoryItemDatatypeLookup
            // 
            this.repositoryItemDatatypeLookup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDatatypeLookup.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Datatype", "DataType")});
            this.repositoryItemDatatypeLookup.DisplayMember = "Datatype";
            this.repositoryItemDatatypeLookup.DropDownRows = 20;
            this.repositoryItemDatatypeLookup.Name = "repositoryItemDatatypeLookup";
            this.repositoryItemDatatypeLookup.PopupWidth = 220;
            this.repositoryItemDatatypeLookup.ValueMember = "Datatype";
            // 
            // grdMainvw
            // 
            this.grdMainvw.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.grdMainvw.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.grdMainvw.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.grdMainvw.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.grdMainvw.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.Empty.BackColor2 = System.Drawing.Color.White;
            this.grdMainvw.Appearance.Empty.Options.UseBackColor = true;
            this.grdMainvw.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.grdMainvw.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.EvenRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.EvenRow.Options.UseForeColor = true;
            this.grdMainvw.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.grdMainvw.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.grdMainvw.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.grdMainvw.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.FilterPanel.Options.UseBackColor = true;
            this.grdMainvw.Appearance.FilterPanel.Options.UseForeColor = true;
            this.grdMainvw.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.grdMainvw.Appearance.FixedLine.Options.UseBackColor = true;
            this.grdMainvw.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.grdMainvw.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.FocusedCell.Options.UseBackColor = true;
            this.grdMainvw.Appearance.FocusedCell.Options.UseForeColor = true;
            this.grdMainvw.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.grdMainvw.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.FocusedRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.FocusedRow.Options.UseForeColor = true;
            this.grdMainvw.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.FooterPanel.Options.UseBackColor = true;
            this.grdMainvw.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.FooterPanel.Options.UseForeColor = true;
            this.grdMainvw.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.grdMainvw.Appearance.GroupButton.Options.UseBackColor = true;
            this.grdMainvw.Appearance.GroupButton.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.GroupFooter.Options.UseBackColor = true;
            this.grdMainvw.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.GroupFooter.Options.UseForeColor = true;
            this.grdMainvw.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.grdMainvw.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.GroupPanel.Options.UseBackColor = true;
            this.grdMainvw.Appearance.GroupPanel.Options.UseForeColor = true;
            this.grdMainvw.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.GroupRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.GroupRow.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.GroupRow.Options.UseForeColor = true;
            this.grdMainvw.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.grdMainvw.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.grdMainvw.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.grdMainvw.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.grdMainvw.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.grdMainvw.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.grdMainvw.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.grdMainvw.Appearance.HorzLine.Options.UseBackColor = true;
            this.grdMainvw.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.OddRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.OddRow.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.OddRow.Options.UseForeColor = true;
            this.grdMainvw.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.grdMainvw.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.grdMainvw.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.grdMainvw.Appearance.Preview.Options.UseBackColor = true;
            this.grdMainvw.Appearance.Preview.Options.UseFont = true;
            this.grdMainvw.Appearance.Preview.Options.UseForeColor = true;
            this.grdMainvw.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.Row.Options.UseBackColor = true;
            this.grdMainvw.Appearance.Row.Options.UseBorderColor = true;
            this.grdMainvw.Appearance.Row.Options.UseForeColor = true;
            this.grdMainvw.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.grdMainvw.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
            this.grdMainvw.Appearance.RowSeparator.Options.UseBackColor = true;
            this.grdMainvw.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.grdMainvw.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.grdMainvw.Appearance.SelectedRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.SelectedRow.Options.UseForeColor = true;
            this.grdMainvw.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.grdMainvw.Appearance.TopNewRow.Options.UseBackColor = true;
            this.grdMainvw.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.grdMainvw.Appearance.VertLine.Options.UseBackColor = true;
            this.grdMainvw.GridControl = this.grdMain;
            this.grdMainvw.Name = "grdMainvw";
            this.grdMainvw.OptionsCustomization.AllowColumnMoving = false;
            this.grdMainvw.OptionsCustomization.AllowFilter = false;
            this.grdMainvw.OptionsCustomization.AllowGroup = false;
            this.grdMainvw.OptionsDetail.EnableMasterViewMode = false;
            this.grdMainvw.OptionsView.EnableAppearanceEvenRow = true;
            this.grdMainvw.OptionsView.EnableAppearanceOddRow = true;
            this.grdMainvw.OptionsView.ShowGroupPanel = false;
            // 
            // grdMain
            // 
            gridLevelNode1.RelationName = "Level1";
            this.grdMain.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.grdMain.Location = new System.Drawing.Point(7, 8);
            this.grdMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.grdMain.MainView = this.grdMainvw;
            this.grdMain.Name = "grdMain";
            this.grdMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTabLookUp,
            this.repositoryItemDatatypeLookup});
            this.grdMain.Size = new System.Drawing.Size(270, 192);
            this.grdMain.TabIndex = 1;
            this.grdMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdMainvw});
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(202, 206);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 2;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(121, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMainUserRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(286, 254);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.grdMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMainUserRoles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USer Roles Selection";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmUserRoleSelection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTabLookUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDatatypeLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMainvw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemTabLookUp;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemDatatypeLookup;
        private DevExpress.XtraGrid.Views.Grid.GridView grdMainvw;
        private DevExpress.XtraGrid.GridControl grdMain;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnCancel;


    }
}

