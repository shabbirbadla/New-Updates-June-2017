namespace Colonne
{
    partial class DadosColumns
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DadosColumns));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbQuarry = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGetColumns = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ColumnsdataGridView = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Precision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsGrouped = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsFreezing = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSummury = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsDisplayed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColumnsdataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbQuarry);
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(865, 183);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lbQuarry
            // 
            this.lbQuarry.FormattingEnabled = true;
            this.lbQuarry.Location = new System.Drawing.Point(7, 12);
            this.lbQuarry.Name = "lbQuarry";
            this.lbQuarry.Size = new System.Drawing.Size(852, 160);
            this.lbQuarry.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(116, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGetColumns
            // 
            this.btnGetColumns.Location = new System.Drawing.Point(10, 40);
            this.btnGetColumns.Name = "btnGetColumns";
            this.btnGetColumns.Size = new System.Drawing.Size(75, 23);
            this.btnGetColumns.TabIndex = 2;
            this.btnGetColumns.Text = "Get Columns";
            this.btnGetColumns.UseVisualStyleBackColor = true;
            this.btnGetColumns.Click += new System.EventHandler(this.btnGetColumns_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ColumnsdataGridView);
            this.groupBox2.Location = new System.Drawing.Point(5, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(648, 310);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // ColumnsdataGridView
            // 
            this.ColumnsdataGridView.AllowUserToAddRows = false;
            this.ColumnsdataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnsdataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnsdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColumnsdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnCaption,
            this.ColumnDataType,
            this.ColumnOrder,
            this.Precision,
            this.IsGrouped,
            this.IsFreezing,
            this.ColWidth,
            this.IsSummury,
            this.IsDisplayed});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnsdataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnsdataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnsdataGridView.Location = new System.Drawing.Point(3, 16);
            this.ColumnsdataGridView.MultiSelect = false;
            this.ColumnsdataGridView.Name = "ColumnsdataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnsdataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnsdataGridView.Size = new System.Drawing.Size(642, 291);
            this.ColumnsdataGridView.TabIndex = 4;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "ColumnName";
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
            this.ColumnDataType.HeaderText = "Column Data Type";
            this.ColumnDataType.Name = "ColumnDataType";
            this.ColumnDataType.ReadOnly = true;
            this.ColumnDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDataType.Width = 150;
            // 
            // ColumnOrder
            // 
            this.ColumnOrder.HeaderText = "Column Order";
            this.ColumnOrder.Name = "ColumnOrder";
            this.ColumnOrder.ReadOnly = true;
            this.ColumnOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Precision
            // 
            this.Precision.HeaderText = "Precision";
            this.Precision.Name = "Precision";
            this.Precision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IsGrouped
            // 
            this.IsGrouped.HeaderText = "Is Grouped";
            this.IsGrouped.Name = "IsGrouped";
            this.IsGrouped.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsGrouped.Width = 50;
            // 
            // IsFreezing
            // 
            this.IsFreezing.HeaderText = "Is Freezing";
            this.IsFreezing.Name = "IsFreezing";
            this.IsFreezing.Width = 50;
            // 
            // ColWidth
            // 
            this.ColWidth.HeaderText = "Column Width";
            this.ColWidth.Name = "ColWidth";
            this.ColWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IsSummury
            // 
            this.IsSummury.HeaderText = "Is Summury";
            this.IsSummury.Name = "IsSummury";
            this.IsSummury.Width = 50;
            // 
            // IsDisplayed
            // 
            this.IsDisplayed.HeaderText = "Is Displayed";
            this.IsDisplayed.Name = "IsDisplayed";
            this.IsDisplayed.Width = 50;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.ShowAlways = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.linkLabel1);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnGetColumns);
            this.groupBox3.Location = new System.Drawing.Point(665, 189);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 310);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(70, 236);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(48, 143);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(100, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Change Data Base ";
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // DadosColumns
            // 
            this.AcceptButton = this.btnGetColumns;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(877, 504);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DadosColumns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Colonne";
            this.Load += new System.EventHandler(this.DadosColumns_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ColumnsdataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbQuarry;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGetColumns;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView ColumnsdataGridView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Precision;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsGrouped;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsFreezing;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWidth;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSummury;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsDisplayed;
    }
}