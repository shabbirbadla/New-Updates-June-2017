namespace udAccountsTreeView
{
    partial class FrmAccountsTreeView
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
            this.gbx2 = new System.Windows.Forms.GroupBox();
            this.Tvw = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAcNm = new System.Windows.Forms.Button();
            this.txtAcNm = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.gbxGroup = new System.Windows.Forms.GroupBox();
            this.btnGroup = new System.Windows.Forms.Button();
            this.chkLedger = new System.Windows.Forms.CheckBox();
            this.chlDeAct = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtGrp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.stbMain = new System.Windows.Forms.StatusStrip();
            this.sblChild = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.gbx2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbxGroup.SuspendLayout();
            this.stbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbx2
            // 
            this.gbx2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx2.Controls.Add(this.Tvw);
            this.gbx2.Location = new System.Drawing.Point(12, 139);
            this.gbx2.Name = "gbx2";
            this.gbx2.Size = new System.Drawing.Size(1007, 401);
            this.gbx2.TabIndex = 2;
            this.gbx2.TabStop = false;
            this.gbx2.Text = "Chart of Accounts";
            // 
            // Tvw
            // 
            this.Tvw.AllowDrop = true;
            this.Tvw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tvw.Location = new System.Drawing.Point(10, 19);
            this.Tvw.Name = "Tvw";
            this.Tvw.Size = new System.Drawing.Size(987, 374);
            this.Tvw.TabIndex = 2;
            this.Tvw.DoubleClick += new System.EventHandler(this.Tvw_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAcNm);
            this.groupBox1.Controls.Add(this.txtAcNm);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Location = new System.Drawing.Point(456, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 62);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // btnAcNm
            // 
            this.btnAcNm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAcNm.Location = new System.Drawing.Point(401, 17);
            this.btnAcNm.Name = "btnAcNm";
            this.btnAcNm.Size = new System.Drawing.Size(24, 20);
            this.btnAcNm.TabIndex = 86;
            this.btnAcNm.UseVisualStyleBackColor = true;
            this.btnAcNm.Click += new System.EventHandler(this.btnAcNm_Click);
            // 
            // txtAcNm
            // 
            this.txtAcNm.Location = new System.Drawing.Point(48, 18);
            this.txtAcNm.Name = "txtAcNm";
            this.txtAcNm.ReadOnly = true;
            this.txtAcNm.Size = new System.Drawing.Size(347, 20);
            this.txtAcNm.TabIndex = 85;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 21);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 13);
            this.label14.TabIndex = 84;
            this.label14.Text = "Name";
            // 
            // gbxGroup
            // 
            this.gbxGroup.Controls.Add(this.btnGroup);
            this.gbxGroup.Controls.Add(this.chkLedger);
            this.gbxGroup.Controls.Add(this.chlDeAct);
            this.gbxGroup.Controls.Add(this.btnRefresh);
            this.gbxGroup.Controls.Add(this.txtGrp);
            this.gbxGroup.Controls.Add(this.label3);
            this.gbxGroup.Location = new System.Drawing.Point(12, 29);
            this.gbxGroup.Name = "gbxGroup";
            this.gbxGroup.Size = new System.Drawing.Size(438, 100);
            this.gbxGroup.TabIndex = 87;
            this.gbxGroup.TabStop = false;
            this.gbxGroup.Text = "Options";
            // 
            // btnGroup
            // 
            this.btnGroup.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnGroup.Location = new System.Drawing.Point(401, 17);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(24, 20);
            this.btnGroup.TabIndex = 87;
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // chkLedger
            // 
            this.chkLedger.AutoSize = true;
            this.chkLedger.Location = new System.Drawing.Point(10, 68);
            this.chkLedger.Name = "chkLedger";
            this.chkLedger.Size = new System.Drawing.Size(152, 17);
            this.chkLedger.TabIndex = 88;
            this.chkLedger.Text = "Include Accounts/Ledgers";
            this.chkLedger.UseVisualStyleBackColor = true;
            // 
            // chlDeAct
            // 
            this.chlDeAct.AutoSize = true;
            this.chlDeAct.Location = new System.Drawing.Point(10, 45);
            this.chlDeAct.Name = "chlDeAct";
            this.chlDeAct.Size = new System.Drawing.Size(252, 17);
            this.chlDeAct.TabIndex = 87;
            this.chlDeAct.Text = "Include Deactivated Accounts/Groups/Ledgers";
            this.chlDeAct.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(336, 54);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(82, 31);
            this.btnRefresh.TabIndex = 86;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtGrp
            // 
            this.txtGrp.Location = new System.Drawing.Point(48, 18);
            this.txtGrp.Name = "txtGrp";
            this.txtGrp.ReadOnly = true;
            this.txtGrp.Size = new System.Drawing.Size(347, 20);
            this.txtGrp.TabIndex = 85;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 84;
            this.label3.Text = "Group";
            // 
            // stbMain
            // 
            this.stbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblChild});
            this.stbMain.Location = new System.Drawing.Point(0, 555);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(1028, 22);
            this.stbMain.TabIndex = 91;
            this.stbMain.Text = "statusStrip1";
            // 
            // sblChild
            // 
            this.sblChild.Name = "sblChild";
            this.sblChild.Size = new System.Drawing.Size(118, 17);
            this.sblChild.Text = "toolStripStatusLabel1";
            this.sblChild.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1028, 25);
            this.toolStrip1.TabIndex = 93;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FrmAccountsTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 577);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.stbMain);
            this.Controls.Add(this.gbxGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbx2);
            this.Name = "FrmAccountsTreeView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.FrmAccountsTreeView_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAccountsTreeView_FormClosing);
            this.gbx2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxGroup.ResumeLayout(false);
            this.gbxGroup.PerformLayout();
            this.stbMain.ResumeLayout(false);
            this.stbMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbx2;
        private System.Windows.Forms.TreeView Tvw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAcNm;
        private System.Windows.Forms.TextBox txtAcNm;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox gbxGroup;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtGrp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chlDeAct;
        private System.Windows.Forms.StatusStrip stbMain;
        private System.Windows.Forms.ToolStripStatusLabel sblChild;
        private System.Windows.Forms.CheckBox chkLedger;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;

    }
}

