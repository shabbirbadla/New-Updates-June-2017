namespace udSelectPop
{
    partial class SELECTPOPUP
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.dgrSelect = new System.Windows.Forms.DataGridView();
            this.btnSearchOpt = new System.Windows.Forms.Button();
            this.toolTipButton = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgrSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtSelect
            // 
            this.txtSelect.Location = new System.Drawing.Point(42, 2);
            this.txtSelect.Multiline = true;
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(361, 28);
            this.txtSelect.TabIndex = 1;
            this.txtSelect.TextChanged += new System.EventHandler(this.txtSelect_TextChanged);
            this.txtSelect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSelect_KeyDown);
            // 
            // dgrSelect
            // 
            this.dgrSelect.AllowUserToAddRows = false;
            this.dgrSelect.AllowUserToDeleteRows = false;
            this.dgrSelect.AllowUserToResizeRows = false;
            this.dgrSelect.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgrSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgrSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgrSelect.Location = new System.Drawing.Point(5, 33);
            this.dgrSelect.MultiSelect = false;
            this.dgrSelect.Name = "dgrSelect";
            this.dgrSelect.ReadOnly = true;
            this.dgrSelect.RowHeadersVisible = false;
            this.dgrSelect.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgrSelect.Size = new System.Drawing.Size(708, 226);
            this.dgrSelect.TabIndex = 2;
            this.dgrSelect.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgrSelect_ColumnHeaderMouseClick);
            this.dgrSelect.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgrSelect_DataError);
            this.dgrSelect.SelectionChanged += new System.EventHandler(this.dgrSelect_SelectionChanged);
            this.dgrSelect.DoubleClick += new System.EventHandler(this.dgrSelect_DoubleClick);
            this.dgrSelect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgrSelect_KeyDown);
            this.dgrSelect.Leave += new System.EventHandler(this.dgrSelect_Leave);
            // 
            // btnSearchOpt
            // 
            this.btnSearchOpt.Image = global::udSelectPop.Properties.Resources.Incsearch;
            this.btnSearchOpt.Location = new System.Drawing.Point(409, 1);
            this.btnSearchOpt.Name = "btnSearchOpt";
            this.btnSearchOpt.Size = new System.Drawing.Size(34, 29);
            this.btnSearchOpt.TabIndex = 4;
            this.btnSearchOpt.UseVisualStyleBackColor = true;
            this.btnSearchOpt.Click += new System.EventHandler(this.btnSearchOpt_Click);
            // 
            // SELECTPOPUP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 263);
            this.Controls.Add(this.btnSearchOpt);
            this.Controls.Add(this.dgrSelect);
            this.Controls.Add(this.txtSelect);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Location = new System.Drawing.Point(51, 1);
            this.MaximizeBox = false;
            this.Name = "SELECTPOPUP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Popup";
            this.Load += new System.EventHandler(this.SELECTPOPUP_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SELECTPOPUP_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgrSelect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.DataGridView dgrSelect;
        private System.Windows.Forms.Button btnSearchOpt;
       private System.Windows.Forms.ToolTip toolTipButton;
    }
}

