namespace Colonne
{
    partial class Columns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Columns));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.linkLabelChangeDatabase = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxQuarry = new System.Windows.Forms.TextBox();
            this.btnGetColumns = new System.Windows.Forms.Button();
            this.linkLabelEditQuarry = new System.Windows.Forms.LinkLabel();
            this.comboBoxQuarryID = new System.Windows.Forms.ComboBox();
            this.btnTestQuarry = new System.Windows.Forms.Button();
            this.btnUpdateQuarry = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ColumnsdataGridView = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Precision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsGrouped = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsFreezing = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsSummury = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsDisplayed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColumnsdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.linkLabelChangeDatabase);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxQuarry);
            this.groupBox1.Controls.Add(this.btnGetColumns);
            this.groupBox1.Controls.Add(this.linkLabelEditQuarry);
            this.groupBox1.Controls.Add(this.comboBoxQuarryID);
            this.groupBox1.Controls.Add(this.btnTestQuarry);
            this.groupBox1.Controls.Add(this.btnUpdateQuarry);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox1.Size = new System.Drawing.Size(1028, 213);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(840, 12);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(141, 25);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // linkLabelChangeDatabase
            // 
            this.linkLabelChangeDatabase.AutoSize = true;
            this.linkLabelChangeDatabase.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelChangeDatabase.Location = new System.Drawing.Point(312, 18);
            this.linkLabelChangeDatabase.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.linkLabelChangeDatabase.Name = "linkLabelChangeDatabase";
            this.linkLabelChangeDatabase.Size = new System.Drawing.Size(122, 14);
            this.linkLabelChangeDatabase.TabIndex = 2;
            this.linkLabelChangeDatabase.TabStop = true;
            this.linkLabelChangeDatabase.Text = "Change Database";
            this.linkLabelChangeDatabase.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelChangeDatabase_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(691, 13);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(141, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Quarry ID";
            // 
            // textBoxQuarry
            // 
            this.textBoxQuarry.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxQuarry.Enabled = false;
            this.textBoxQuarry.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQuarry.Location = new System.Drawing.Point(5, 43);
            this.textBoxQuarry.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBoxQuarry.Multiline = true;
            this.textBoxQuarry.Name = "textBoxQuarry";
            this.textBoxQuarry.Size = new System.Drawing.Size(1018, 167);
            this.textBoxQuarry.TabIndex = 7;
            // 
            // btnGetColumns
            // 
            this.btnGetColumns.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetColumns.Location = new System.Drawing.Point(542, 13);
            this.btnGetColumns.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnGetColumns.Name = "btnGetColumns";
            this.btnGetColumns.Size = new System.Drawing.Size(141, 25);
            this.btnGetColumns.TabIndex = 4;
            this.btnGetColumns.Text = "Get Columns";
            this.btnGetColumns.UseVisualStyleBackColor = true;
            this.btnGetColumns.Click += new System.EventHandler(this.btnGetColumns_Click);
            // 
            // linkLabelEditQuarry
            // 
            this.linkLabelEditQuarry.AutoSize = true;
            this.linkLabelEditQuarry.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelEditQuarry.Location = new System.Drawing.Point(451, 20);
            this.linkLabelEditQuarry.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.linkLabelEditQuarry.Name = "linkLabelEditQuarry";
            this.linkLabelEditQuarry.Size = new System.Drawing.Size(83, 14);
            this.linkLabelEditQuarry.TabIndex = 3;
            this.linkLabelEditQuarry.TabStop = true;
            this.linkLabelEditQuarry.Text = "Edit Quarry";
            this.linkLabelEditQuarry.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEditQuarry_LinkClicked);
            // 
            // comboBoxQuarryID
            // 
            this.comboBoxQuarryID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQuarryID.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxQuarryID.FormattingEnabled = true;
            this.comboBoxQuarryID.Location = new System.Drawing.Point(138, 12);
            this.comboBoxQuarryID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.comboBoxQuarryID.Name = "comboBoxQuarryID";
            this.comboBoxQuarryID.Size = new System.Drawing.Size(157, 22);
            this.comboBoxQuarryID.TabIndex = 1;
            this.comboBoxQuarryID.SelectedIndexChanged += new System.EventHandler(this.comboBoxQuarryID_SelectedIndexChanged);
            // 
            // btnTestQuarry
            // 
            this.btnTestQuarry.Location = new System.Drawing.Point(840, 66);
            this.btnTestQuarry.Name = "btnTestQuarry";
            this.btnTestQuarry.Size = new System.Drawing.Size(141, 25);
            this.btnTestQuarry.TabIndex = 8;
            this.btnTestQuarry.Text = "Test Quarry";
            this.btnTestQuarry.UseVisualStyleBackColor = true;
            this.btnTestQuarry.Click += new System.EventHandler(this.btnTestQuarry_Click);
            // 
            // btnUpdateQuarry
            // 
            this.btnUpdateQuarry.Location = new System.Drawing.Point(840, 184);
            this.btnUpdateQuarry.Name = "btnUpdateQuarry";
            this.btnUpdateQuarry.Size = new System.Drawing.Size(141, 25);
            this.btnUpdateQuarry.TabIndex = 9;
            this.btnUpdateQuarry.Text = "Update Quarry";
            this.btnUpdateQuarry.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ColumnsdataGridView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 213);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox2.Size = new System.Drawing.Size(1028, 281);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // ColumnsdataGridView
            // 
            this.ColumnsdataGridView.AllowUserToAddRows = false;
            this.ColumnsdataGridView.AllowUserToDeleteRows = false;
            this.ColumnsdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColumnsdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnCaption,
            this.ColumnDataType,
            this.ColumnOrder,
            this.Precision,
            this.ColWidth,
            this.IsGrouped,
            this.IsFreezing,
            this.IsSummury,
            this.IsDisplayed});
            this.ColumnsdataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnsdataGridView.Enabled = false;
            this.ColumnsdataGridView.Location = new System.Drawing.Point(5, 18);
            this.ColumnsdataGridView.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ColumnsdataGridView.Name = "ColumnsdataGridView";
            this.ColumnsdataGridView.Size = new System.Drawing.Size(1018, 260);
            this.ColumnsdataGridView.TabIndex = 8;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Column Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnName.Width = 150;
            // 
            // ColumnCaption
            // 
            this.ColumnCaption.HeaderText = "Column Caption";
            this.ColumnCaption.Name = "ColumnCaption";
            this.ColumnCaption.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnCaption.Width = 150;
            // 
            // ColumnDataType
            // 
            this.ColumnDataType.HeaderText = "Column Data Types";
            this.ColumnDataType.Name = "ColumnDataType";
            this.ColumnDataType.ReadOnly = true;
            this.ColumnDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDataType.Width = 105;
            // 
            // ColumnOrder
            // 
            this.ColumnOrder.HeaderText = "Column Order";
            this.ColumnOrder.Name = "ColumnOrder";
            this.ColumnOrder.ReadOnly = true;
            this.ColumnOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnOrder.Width = 75;
            // 
            // Precision
            // 
            this.Precision.HeaderText = "Precision";
            this.Precision.Name = "Precision";
            this.Precision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Precision.Width = 75;
            // 
            // ColWidth
            // 
            this.ColWidth.HeaderText = "Columns Width";
            this.ColWidth.Name = "ColWidth";
            this.ColWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColWidth.Width = 75;
            // 
            // IsGrouped
            // 
            this.IsGrouped.HeaderText = "IsGrouped";
            this.IsGrouped.Name = "IsGrouped";
            this.IsGrouped.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsGrouped.Width = 85;
            // 
            // IsFreezing
            // 
            this.IsFreezing.HeaderText = "IsFreezing";
            this.IsFreezing.Name = "IsFreezing";
            this.IsFreezing.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsFreezing.Width = 85;
            // 
            // IsSummury
            // 
            this.IsSummury.HeaderText = "IsSummury";
            this.IsSummury.Name = "IsSummury";
            this.IsSummury.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsSummury.Width = 85;
            // 
            // IsDisplayed
            // 
            this.IsDisplayed.HeaderText = "IsDisplayed";
            this.IsDisplayed.Name = "IsDisplayed";
            this.IsDisplayed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsDisplayed.Width = 90;
            // 
            // Columns
            // 
            this.AcceptButton = this.btnGetColumns;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1028, 494);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.7F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "Columns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Columns";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Columns_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ColumnsdataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxQuarryID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGetColumns;
        private System.Windows.Forms.LinkLabel linkLabelChangeDatabase;
        private System.Windows.Forms.TextBox textBoxQuarry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ColumnsdataGridView;
        private System.Windows.Forms.LinkLabel linkLabelEditQuarry;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Precision;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWidth;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsGrouped;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsFreezing;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSummury;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsDisplayed;
        private System.Windows.Forms.Button btnUpdateQuarry;
        private System.Windows.Forms.Button btnTestQuarry;
    }
}