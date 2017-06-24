namespace U2TPlus
{
    partial class SettingsForm_1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm_1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SettingsdataGridView = new System.Windows.Forms.DataGridView();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XMLOpeningTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefaultValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsDefault = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddNewTag = new System.Windows.Forms.Button();
            this.btnDeleteTag = new System.Windows.Forms.Button();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsdataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SettingsdataGridView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 383);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // SettingsdataGridView
            // 
            this.SettingsdataGridView.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.SettingsdataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.SettingsdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsActive,
            this.Order,
            this.XMLOpeningTag,
            this.DefaultValues,
            this.FieldValue,
            this.IsDefault});
            this.SettingsdataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsdataGridView.Location = new System.Drawing.Point(3, 16);
            this.SettingsdataGridView.Name = "SettingsdataGridView";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.YellowGreen;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.SettingsdataGridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.SettingsdataGridView.Size = new System.Drawing.Size(885, 364);
            this.SettingsdataGridView.TabIndex = 0;
            // 
            // IsActive
            // 
            this.IsActive.HeaderText = "Is Active";
            this.IsActive.Name = "IsActive";
            this.IsActive.Width = 70;
            // 
            // Order
            // 
            this.Order.HeaderText = "Order";
            this.Order.Name = "Order";
            this.Order.ReadOnly = true;
            this.Order.Width = 50;
            // 
            // XMLOpeningTag
            // 
            this.XMLOpeningTag.HeaderText = "XML Opening Tag";
            this.XMLOpeningTag.Name = "XMLOpeningTag";
            this.XMLOpeningTag.Width = 250;
            // 
            // DefaultValues
            // 
            this.DefaultValues.HeaderText = "Default Values";
            this.DefaultValues.Name = "DefaultValues";
            this.DefaultValues.Width = 150;
            // 
            // FieldValue
            // 
            this.FieldValue.HeaderText = "Field Value";
            this.FieldValue.Name = "FieldValue";
            this.FieldValue.Width = 250;
            // 
            // IsDefault
            // 
            this.IsDefault.HeaderText = "Is Default";
            this.IsDefault.Name = "IsDefault";
            this.IsDefault.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsDefault.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsDefault.Width = 70;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnAddNewTag);
            this.groupBox2.Controls.Add(this.btnDeleteTag);
            this.groupBox2.Controls.Add(this.btnSaveAndClose);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 383);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(891, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(599, 47);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddNewTag
            // 
            this.btnAddNewTag.Location = new System.Drawing.Point(172, 47);
            this.btnAddNewTag.Name = "btnAddNewTag";
            this.btnAddNewTag.Size = new System.Drawing.Size(104, 23);
            this.btnAddNewTag.TabIndex = 0;
            this.btnAddNewTag.Text = "Add New Tag";
            this.btnAddNewTag.UseVisualStyleBackColor = true;
            this.btnAddNewTag.Click += new System.EventHandler(this.btnAddNewTag_Click);
            // 
            // btnDeleteTag
            // 
            this.btnDeleteTag.Enabled = false;
            this.btnDeleteTag.Location = new System.Drawing.Point(312, 47);
            this.btnDeleteTag.Name = "btnDeleteTag";
            this.btnDeleteTag.Size = new System.Drawing.Size(104, 23);
            this.btnDeleteTag.TabIndex = 1;
            this.btnDeleteTag.Text = "Delete Tag";
            this.btnDeleteTag.UseVisualStyleBackColor = true;
            this.btnDeleteTag.Click += new System.EventHandler(this.btnDeleteTag_Click);
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Enabled = false;
            this.btnSaveAndClose.Location = new System.Drawing.Point(454, 47);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(104, 23);
            this.btnSaveAndClose.TabIndex = 2;
            this.btnSaveAndClose.Text = "Save And Close";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnAddNewTag;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.OldLace;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(891, 483);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SettingsdataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddNewTag;
        private System.Windows.Forms.Button btnDeleteTag;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.DataGridView SettingsdataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn Order;
        private System.Windows.Forms.DataGridViewTextBoxColumn XMLOpeningTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsDefault;
        private System.Windows.Forms.Button btnClose;
    }
}