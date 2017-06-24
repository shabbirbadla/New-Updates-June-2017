namespace DynamicMasterDesigner
{
    partial class frmMasterFormDesigner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMasterFormDesigner));
            this.gridTabControl = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRemoveToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewTabControls = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemTabLookUp = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemDatatypeLookup = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridFields = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddField = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRemoveField = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewFields = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonPreviewForm = new System.Windows.Forms.Button();
            this.buttonLookup = new System.Windows.Forms.Button();
            this.textBoxTableName = new System.Windows.Forms.TextBox();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.textBoxCaption = new System.Windows.Forms.TextBox();
            this.labelCaption = new System.Windows.Forms.Label();
            this.labelCode = new System.Windows.Forms.Label();
            this.labelTableName = new System.Windows.Forms.Label();
            this.textBoxFormID = new System.Windows.Forms.TextBox();
            this.txtCaption = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.btnForward = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEmail = new System.Windows.Forms.ToolStripButton();
            this.btnLocate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnCalculator = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridTabControl)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTabControls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTabLookUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDatatypeLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFields)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFields)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridTabControl
            // 
            this.gridTabControl.ContextMenuStrip = this.contextMenuStrip1;
            this.gridTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.gridTabControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridTabControl.Location = new System.Drawing.Point(3, 17);
            this.gridTabControl.LookAndFeel.SkinName = "Blue";
            this.gridTabControl.MainView = this.gridViewTabControls;
            this.gridTabControl.Name = "gridTabControl";
            this.gridTabControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTabLookUp,
            this.repositoryItemDatatypeLookup});
            this.gridTabControl.Size = new System.Drawing.Size(818, 206);
            this.gridTabControl.TabIndex = 0;
            this.gridTabControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTabControls});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddToolbar,
            this.menuItemRemoveToolbar});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 48);
            // 
            // menuItemAddToolbar
            // 
            this.menuItemAddToolbar.Name = "menuItemAddToolbar";
            this.menuItemAddToolbar.Size = new System.Drawing.Size(144, 22);
            this.menuItemAddToolbar.Text = "Add Item";
            this.menuItemAddToolbar.Click += new System.EventHandler(this.menuItemAddToolbar_Click);
            // 
            // menuItemRemoveToolbar
            // 
            this.menuItemRemoveToolbar.Name = "menuItemRemoveToolbar";
            this.menuItemRemoveToolbar.Size = new System.Drawing.Size(144, 22);
            this.menuItemRemoveToolbar.Text = "Remove Item";
            this.menuItemRemoveToolbar.Click += new System.EventHandler(this.menuItemRemoveToolbar_Click);
            // 
            // gridViewTabControls
            // 
            this.gridViewTabControls.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.Empty.BackColor2 = System.Drawing.Color.White;
            this.gridViewTabControls.Appearance.Empty.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.gridViewTabControls.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.EvenRow.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gridViewTabControls.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.gridViewTabControls.Appearance.FixedLine.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.gridViewTabControls.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.gridViewTabControls.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewTabControls.Appearance.GroupButton.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gridViewTabControls.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.GroupRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.GroupRow.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.GroupRow.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewTabControls.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gridViewTabControls.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.gridViewTabControls.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.gridViewTabControls.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.OddRow.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.OddRow.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.gridViewTabControls.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gridViewTabControls.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.gridViewTabControls.Appearance.Preview.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.Preview.Options.UseFont = true;
            this.gridViewTabControls.Appearance.Preview.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.Row.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.Row.Options.UseBorderColor = true;
            this.gridViewTabControls.Appearance.Row.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewTabControls.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
            this.gridViewTabControls.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.gridViewTabControls.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewTabControls.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gridViewTabControls.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gridViewTabControls.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewTabControls.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.gridViewTabControls.Appearance.VertLine.Options.UseBackColor = true;
            this.gridViewTabControls.GridControl = this.gridTabControl;
            this.gridViewTabControls.Name = "gridViewTabControls";
            this.gridViewTabControls.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewTabControls.OptionsCustomization.AllowFilter = false;
            this.gridViewTabControls.OptionsCustomization.AllowGroup = false;
            this.gridViewTabControls.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewTabControls.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewTabControls.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewTabControls.OptionsView.ShowGroupPanel = false;
            this.gridViewTabControls.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewTabControls_FocusedRowChanged);
            this.gridViewTabControls.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridViewTabControls_ShowingEditor);
            this.gridViewTabControls.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridViewTabControls_ValidateRow);
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
            this.repositoryItemTabLookUp.EditValueChanged += new System.EventHandler(this.repositoryItemTabLookUp_EditValueChanged);
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
            this.repositoryItemDatatypeLookup.EditValueChanged += new System.EventHandler(this.repositoryItemDatatypeLookup_EditValueChanged);
            // 
            // gridFields
            // 
            this.gridFields.ContextMenuStrip = this.contextMenuStrip2;
            this.gridFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFields.Location = new System.Drawing.Point(3, 17);
            this.gridFields.LookAndFeel.SkinName = "Blue";
            this.gridFields.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.gridFields.MainView = this.gridViewFields;
            this.gridFields.Name = "gridFields";
            this.gridFields.Size = new System.Drawing.Size(818, 301);
            this.gridFields.TabIndex = 1;
            this.gridFields.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewFields});
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddField,
            this.menuItemRemoveField});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(146, 48);
            // 
            // menuItemAddField
            // 
            this.menuItemAddField.Name = "menuItemAddField";
            this.menuItemAddField.Size = new System.Drawing.Size(145, 22);
            this.menuItemAddField.Text = "Add Field";
            this.menuItemAddField.Click += new System.EventHandler(this.menuItemAddField_Click);
            // 
            // menuItemRemoveField
            // 
            this.menuItemRemoveField.Name = "menuItemRemoveField";
            this.menuItemRemoveField.Size = new System.Drawing.Size(145, 22);
            this.menuItemRemoveField.Text = "Remove Field";
            this.menuItemRemoveField.Click += new System.EventHandler(this.menuItemRemoveField_Click);
            // 
            // gridViewFields
            // 
            this.gridViewFields.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gridViewFields.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gridViewFields.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gridViewFields.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gridViewFields.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.Empty.BackColor2 = System.Drawing.Color.White;
            this.gridViewFields.Appearance.Empty.Options.UseBackColor = true;
            this.gridViewFields.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.gridViewFields.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.EvenRow.Options.UseForeColor = true;
            this.gridViewFields.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gridViewFields.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gridViewFields.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gridViewFields.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gridViewFields.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gridViewFields.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.gridViewFields.Appearance.FixedLine.Options.UseBackColor = true;
            this.gridViewFields.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.gridViewFields.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridViewFields.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridViewFields.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.gridViewFields.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridViewFields.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gridViewFields.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gridViewFields.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(174)))), ((int)(((byte)(66)))));
            this.gridViewFields.Appearance.GroupButton.Options.UseBackColor = true;
            this.gridViewFields.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gridViewFields.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gridViewFields.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gridViewFields.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridViewFields.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridViewFields.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.GroupRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.GroupRow.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.GroupRow.Options.UseForeColor = true;
            this.gridViewFields.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(95)))));
            this.gridViewFields.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridViewFields.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridViewFields.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gridViewFields.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.gridViewFields.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gridViewFields.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.gridViewFields.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridViewFields.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.OddRow.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.OddRow.Options.UseForeColor = true;
            this.gridViewFields.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.gridViewFields.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gridViewFields.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.gridViewFields.Appearance.Preview.Options.UseBackColor = true;
            this.gridViewFields.Appearance.Preview.Options.UseFont = true;
            this.gridViewFields.Appearance.Preview.Options.UseForeColor = true;
            this.gridViewFields.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.Row.Options.UseBackColor = true;
            this.gridViewFields.Appearance.Row.Options.UseBorderColor = true;
            this.gridViewFields.Appearance.Row.Options.UseForeColor = true;
            this.gridViewFields.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.gridViewFields.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
            this.gridViewFields.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridViewFields.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.gridViewFields.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewFields.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gridViewFields.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gridViewFields.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewFields.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.gridViewFields.Appearance.VertLine.Options.UseBackColor = true;
            this.gridViewFields.GridControl = this.gridFields;
            this.gridViewFields.Name = "gridViewFields";
            this.gridViewFields.NewItemRowText = "[New Row]";
            this.gridViewFields.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewFields.OptionsCustomization.AllowSort = false;
            this.gridViewFields.OptionsFilter.AllowFilterEditor = false;
            this.gridViewFields.OptionsView.ColumnAutoWidth = false;
            this.gridViewFields.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewFields.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewFields.OptionsView.ShowGroupPanel = false;
            this.gridViewFields.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewFields_FocusedRowChanged);
            this.gridViewFields.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridViewFields_FocusedColumnChanged);
            this.gridViewFields.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridViewFields_ShowingEditor);
            this.gridViewFields.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridViewFields_ValidateRow);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridTabControl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(824, 226);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tab Controls";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridFields);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 349);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(824, 321);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fields";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonPreviewForm);
            this.groupBox3.Controls.Add(this.buttonLookup);
            this.groupBox3.Controls.Add(this.textBoxTableName);
            this.groupBox3.Controls.Add(this.textBoxCode);
            this.groupBox3.Controls.Add(this.textBoxCaption);
            this.groupBox3.Controls.Add(this.labelCaption);
            this.groupBox3.Controls.Add(this.labelCode);
            this.groupBox3.Controls.Add(this.labelTableName);
            this.groupBox3.Controls.Add(this.textBoxFormID);
            this.groupBox3.Controls.Add(this.txtCaption);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(824, 98);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Form Info";
            // 
            // buttonPreviewForm
            // 
            this.buttonPreviewForm.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPreviewForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPreviewForm.Location = new System.Drawing.Point(525, 22);
            this.buttonPreviewForm.Name = "buttonPreviewForm";
            this.buttonPreviewForm.Size = new System.Drawing.Size(60, 22);
            this.buttonPreviewForm.TabIndex = 8;
            this.buttonPreviewForm.Text = "Preview";
            this.buttonPreviewForm.UseVisualStyleBackColor = false;
            this.buttonPreviewForm.Visible = false;
            this.buttonPreviewForm.Click += new System.EventHandler(this.buttonPreviewForm_Click);
            // 
            // buttonLookup
            // 
            this.buttonLookup.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLookup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLookup.Image = global::DynamicMasterDesigner.Properties.Resources.search_icon;
            this.buttonLookup.Location = new System.Drawing.Point(483, 22);
            this.buttonLookup.Name = "buttonLookup";
            this.buttonLookup.Size = new System.Drawing.Size(29, 21);
            this.buttonLookup.TabIndex = 6;
            this.buttonLookup.UseVisualStyleBackColor = false;
            this.buttonLookup.Click += new System.EventHandler(this.buttonLookup_Click);
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTableName.Location = new System.Drawing.Point(348, 57);
            this.textBoxTableName.MaxLength = 50;
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.ReadOnly = true;
            this.textBoxTableName.Size = new System.Drawing.Size(130, 21);
            this.textBoxTableName.TabIndex = 2;
            // 
            // textBoxCode
            // 
            this.textBoxCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCode.Location = new System.Drawing.Point(133, 57);
            this.textBoxCode.MaxLength = 3;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(115, 21);
            this.textBoxCode.TabIndex = 1;
            this.textBoxCode.Leave += new System.EventHandler(this.textBoxCode_Leave);
            // 
            // textBoxCaption
            // 
            this.textBoxCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCaption.Location = new System.Drawing.Point(133, 23);
            this.textBoxCaption.MaxLength = 50;
            this.textBoxCaption.Name = "textBoxCaption";
            this.textBoxCaption.Size = new System.Drawing.Size(345, 21);
            this.textBoxCaption.TabIndex = 0;
            this.textBoxCaption.Validated += new System.EventHandler(this.textBoxCaption_Validated);
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCaption.Location = new System.Drawing.Point(26, 27);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(96, 15);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "Master Caption: ";
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCode.Location = new System.Drawing.Point(39, 60);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(83, 15);
            this.labelCode.TabIndex = 1;
            this.labelCode.Text = "Master Code: ";
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTableName.Location = new System.Drawing.Point(257, 60);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(81, 15);
            this.labelTableName.TabIndex = 2;
            this.labelTableName.Text = "Table Name: ";
            // 
            // textBoxFormID
            // 
            this.textBoxFormID.Enabled = false;
            this.textBoxFormID.Location = new System.Drawing.Point(199, 57);
            this.textBoxFormID.Name = "textBoxFormID";
            this.textBoxFormID.Size = new System.Drawing.Size(49, 20);
            this.textBoxFormID.TabIndex = 9;
            // 
            // txtCaption
            // 
            this.txtCaption.Location = new System.Drawing.Point(356, 57);
            this.txtCaption.Name = "txtCaption";
            this.txtCaption.ReadOnly = true;
            this.txtCaption.Size = new System.Drawing.Size(107, 20);
            this.txtCaption.TabIndex = 10;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnBack,
            this.btnForward,
            this.btnLast,
            this.toolStripSeparator1,
            this.btnEmail,
            this.btnLocate,
            this.toolStripSeparator2,
            this.btnNew,
            this.btnSave,
            this.btnEdit,
            this.btnCancel,
            this.btnCopy,
            this.toolStripSeparator3,
            this.btnPreview,
            this.btnPrint,
            this.toolStripSeparator4,
            this.btnLogout,
            this.toolStripSeparator6,
            this.btnHelp,
            this.btnCalculator,
            this.toolStripSeparator7,
            this.btnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(824, 25);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnFirst
            // 
            this.btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(23, 22);
            this.btnFirst.Text = "toolStripButton1";
            this.btnFirst.ToolTipText = "First";
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnBack
            // 
            this.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(23, 22);
            this.btnBack.Text = "toolStripButton2";
            this.btnBack.ToolTipText = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnForward
            // 
            this.btnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(23, 22);
            this.btnForward.Text = "toolStripButton3";
            this.btnForward.ToolTipText = "Forward";
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnLast
            // 
            this.btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(23, 22);
            this.btnLast.Text = "toolStripButton4";
            this.btnLast.ToolTipText = "Last";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEmail
            // 
            this.btnEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnEmail.Image")));
            this.btnEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(23, 22);
            this.btnEmail.Text = "toolStripButton5";
            this.btnEmail.ToolTipText = "Email";
            // 
            // btnLocate
            // 
            this.btnLocate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocate.Image = ((System.Drawing.Image)(resources.GetObject("btnLocate.Image")));
            this.btnLocate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(23, 22);
            this.btnLocate.Text = "toolStripButton6";
            this.btnLocate.ToolTipText = "Locate";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.Text = "toolStripButton7";
            this.btnNew.ToolTipText = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "toolStripButton1";
            this.btnSave.ToolTipText = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(23, 22);
            this.btnEdit.Text = "toolStripButton8";
            this.btnEdit.ToolTipText = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(23, 22);
            this.btnCancel.Text = "toolStripButton9";
            this.btnCancel.ToolTipText = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(23, 22);
            this.btnCopy.Text = "toolStripButton10";
            this.btnCopy.ToolTipText = "Copy";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPreview
            // 
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(23, 22);
            this.btnPreview.Text = "toolStripButton11";
            this.btnPreview.ToolTipText = "Preview";
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 22);
            this.btnPrint.Text = "Print";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLogout
            // 
            this.btnLogout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(23, 22);
            this.btnLogout.Text = "toolStripButton14";
            this.btnLogout.ToolTipText = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "toolStripButton16";
            this.btnHelp.ToolTipText = "Help";
            // 
            // btnCalculator
            // 
            this.btnCalculator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCalculator.Image = ((System.Drawing.Image)(resources.GetObject("btnCalculator.Image")));
            this.btnCalculator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalculator.Name = "btnCalculator";
            this.btnCalculator.Size = new System.Drawing.Size(23, 22);
            this.btnCalculator.Text = "toolStripButton17";
            this.btnCalculator.ToolTipText = "Calculator";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExit
            // 
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(23, 22);
            this.btnExit.Text = "toolStripButton15";
            this.btnExit.ToolTipText = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.cancelToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(824, 24);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // frmMasterFormDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(824, 670);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.Name = "frmMasterFormDesigner";
            this.Text = "Dynamic Master Designer";
            this.Load += new System.EventHandler(this.frmMasterFormDesigner_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMasterFormDesigner_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridTabControl)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTabControls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTabLookUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDatatypeLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFields)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFields)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridTabControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTabControls;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddToolbar;
        private System.Windows.Forms.ToolStripMenuItem menuItemRemoveToolbar;
        private DevExpress.XtraGrid.GridControl gridFields;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewFields;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemTabLookUp;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemDatatypeLookup;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddField;
        private System.Windows.Forms.ToolStripMenuItem menuItemRemoveField;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonLookup;
        private System.Windows.Forms.TextBox textBoxTableName;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.TextBox textBoxCaption;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelTableName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnFirst;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.ToolStripButton btnForward;
        private System.Windows.Forms.ToolStripButton btnLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnEmail;
        private System.Windows.Forms.ToolStripButton btnLocate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnCalculator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button buttonPreviewForm;
        private System.Windows.Forms.TextBox textBoxFormID;
        private System.Windows.Forms.TextBox txtCaption;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;        

    }
}