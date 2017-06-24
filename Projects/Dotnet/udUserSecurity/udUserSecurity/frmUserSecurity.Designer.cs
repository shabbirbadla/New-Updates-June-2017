namespace udUserSecurity
{
    partial class frmUserSecurity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserSecurity));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Roles", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Users", 1, 1);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newroleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteroleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyroleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.largeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prop1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newuserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteuserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyuserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.large2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.small2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.list2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.details2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.prop2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.detailsradioButton = new System.Windows.Forms.RadioButton();
            this.listradioButton = new System.Windows.Forms.RadioButton();
            this.smallradioButton = new System.Windows.Forms.RadioButton();
            this.largeradioButton = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "user3.ico");
            this.imageList1.Images.SetKeyName(1, "user6.ico");
            this.imageList1.Images.SetKeyName(2, "adminrole.ico");
            this.imageList1.Images.SetKeyName(3, "userrights.ico");
            this.imageList1.Images.SetKeyName(4, "users.ico");
            this.imageList1.Images.SetKeyName(5, "user5.ico");
            // 
            // treeView1
            // 
            this.treeView1.AccessibleDescription = "Tree";
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "Roles";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "Roles";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "Users";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Text = "Users";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(189, 436);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Location = new System.Drawing.Point(211, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(434, 464);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Details Pane";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 436);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 24);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Note : Right Click on User/ Role in Details Pane to display Context Menu.";
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(6, 14);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(422, 416);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(2, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(203, 468);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(195, 442);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tree";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "VW-LRGIC.BMP");
            this.imageList2.Images.SetKeyName(1, "VW-SMLIC.BMP");
            this.imageList2.Images.SetKeyName(2, "VW-LIST.BMP");
            this.imageList2.Images.SetKeyName(3, "VW-DTLS.BMP");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newroleMenuItem,
            this.deleteroleMenuItem,
            this.copyroleMenuItem,
            this.toolStripSeparator1,
            this.largeMenuItem,
            this.smallMenuItem,
            this.listMenuItem,
            this.detailsMenuItem,
            this.toolStripSeparator2,
            this.refreshMenuItem,
            this.prop1MenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 214);
            // 
            // newroleMenuItem
            // 
            this.newroleMenuItem.Image = global::udUserSecurity.Properties.Resources.adduser;
            this.newroleMenuItem.Name = "newroleMenuItem";
            this.newroleMenuItem.Size = new System.Drawing.Size(134, 22);
            this.newroleMenuItem.Text = "New Role";
            this.newroleMenuItem.Click += new System.EventHandler(this.newroleMenuItem_Click);
            // 
            // deleteroleMenuItem
            // 
            this.deleteroleMenuItem.Image = global::udUserSecurity.Properties.Resources.deluser;
            this.deleteroleMenuItem.Name = "deleteroleMenuItem";
            this.deleteroleMenuItem.Size = new System.Drawing.Size(134, 22);
            this.deleteroleMenuItem.Text = "Delete Role";
            this.deleteroleMenuItem.Click += new System.EventHandler(this.deleteroleMenuItem_Click);
            // 
            // copyroleMenuItem
            // 
            this.copyroleMenuItem.Image = global::udUserSecurity.Properties.Resources.copyuser;
            this.copyroleMenuItem.Name = "copyroleMenuItem";
            this.copyroleMenuItem.Size = new System.Drawing.Size(134, 22);
            this.copyroleMenuItem.Text = "Copy Role";
            this.copyroleMenuItem.Click += new System.EventHandler(this.copyroleMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // largeMenuItem
            // 
            this.largeMenuItem.Image = global::udUserSecurity.Properties.Resources.VW_LRGIC;
            this.largeMenuItem.Name = "largeMenuItem";
            this.largeMenuItem.Size = new System.Drawing.Size(134, 22);
            this.largeMenuItem.Text = "Large Icons";
            this.largeMenuItem.Click += new System.EventHandler(this.largeMenuItem_Click);
            // 
            // smallMenuItem
            // 
            this.smallMenuItem.Image = global::udUserSecurity.Properties.Resources.VW_SMLIC;
            this.smallMenuItem.Name = "smallMenuItem";
            this.smallMenuItem.Size = new System.Drawing.Size(134, 22);
            this.smallMenuItem.Text = "Small Icons";
            this.smallMenuItem.Click += new System.EventHandler(this.smallMenuItem_Click);
            // 
            // listMenuItem
            // 
            this.listMenuItem.Image = global::udUserSecurity.Properties.Resources.VW_LIST;
            this.listMenuItem.Name = "listMenuItem";
            this.listMenuItem.Size = new System.Drawing.Size(134, 22);
            this.listMenuItem.Text = "List";
            this.listMenuItem.Click += new System.EventHandler(this.listMenuItem_Click);
            // 
            // detailsMenuItem
            // 
            this.detailsMenuItem.Image = global::udUserSecurity.Properties.Resources.VW_DTLS;
            this.detailsMenuItem.Name = "detailsMenuItem";
            this.detailsMenuItem.Size = new System.Drawing.Size(134, 22);
            this.detailsMenuItem.Text = "Details";
            this.detailsMenuItem.Click += new System.EventHandler(this.detailsMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
            // 
            // refreshMenuItem
            // 
            this.refreshMenuItem.Image = global::udUserSecurity.Properties.Resources.refresh;
            this.refreshMenuItem.Name = "refreshMenuItem";
            this.refreshMenuItem.Size = new System.Drawing.Size(134, 22);
            this.refreshMenuItem.Text = "Refresh";
            this.refreshMenuItem.Click += new System.EventHandler(this.refreshMenuItem_Click);
            // 
            // prop1MenuItem
            // 
            this.prop1MenuItem.Image = global::udUserSecurity.Properties.Resources.roleproperties;
            this.prop1MenuItem.Name = "prop1MenuItem";
            this.prop1MenuItem.Size = new System.Drawing.Size(134, 22);
            this.prop1MenuItem.Text = "Properties";
            this.prop1MenuItem.Click += new System.EventHandler(this.prop1MenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newuserMenuItem,
            this.deleteuserMenuItem,
            this.copyuserMenuItem,
            this.toolStripSeparator3,
            this.large2MenuItem,
            this.small2MenuItem,
            this.list2MenuItem,
            this.details2MenuItem,
            this.toolStripSeparator4,
            this.toolStripMenuItem8,
            this.prop2MenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 236);
            // 
            // newuserMenuItem
            // 
            this.newuserMenuItem.Image = global::udUserSecurity.Properties.Resources.adduser;
            this.newuserMenuItem.Name = "newuserMenuItem";
            this.newuserMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newuserMenuItem.Text = "New User";
            this.newuserMenuItem.Click += new System.EventHandler(this.newuserMenuItem_Click);
            // 
            // deleteuserMenuItem
            // 
            this.deleteuserMenuItem.Image = global::udUserSecurity.Properties.Resources.deluser;
            this.deleteuserMenuItem.Name = "deleteuserMenuItem";
            this.deleteuserMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteuserMenuItem.Text = "Delete User";
            this.deleteuserMenuItem.Click += new System.EventHandler(this.deleteuserMenuItem_Click);
            // 
            // copyuserMenuItem
            // 
            this.copyuserMenuItem.Image = global::udUserSecurity.Properties.Resources.copyuser;
            this.copyuserMenuItem.Name = "copyuserMenuItem";
            this.copyuserMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyuserMenuItem.Text = "Copy User ";
            this.copyuserMenuItem.Click += new System.EventHandler(this.copyuserMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // large2MenuItem
            // 
            this.large2MenuItem.Image = global::udUserSecurity.Properties.Resources.VW_LRGIC;
            this.large2MenuItem.Name = "large2MenuItem";
            this.large2MenuItem.Size = new System.Drawing.Size(152, 22);
            this.large2MenuItem.Text = "Large Icons";
            this.large2MenuItem.Click += new System.EventHandler(this.largeMenuItem_Click);
            // 
            // small2MenuItem
            // 
            this.small2MenuItem.Image = global::udUserSecurity.Properties.Resources.VW_SMLIC;
            this.small2MenuItem.Name = "small2MenuItem";
            this.small2MenuItem.Size = new System.Drawing.Size(152, 22);
            this.small2MenuItem.Text = "Small Icons";
            this.small2MenuItem.Click += new System.EventHandler(this.smallMenuItem_Click);
            // 
            // list2MenuItem
            // 
            this.list2MenuItem.Image = global::udUserSecurity.Properties.Resources.VW_LIST;
            this.list2MenuItem.Name = "list2MenuItem";
            this.list2MenuItem.Size = new System.Drawing.Size(152, 22);
            this.list2MenuItem.Text = "List";
            this.list2MenuItem.Click += new System.EventHandler(this.listMenuItem_Click);
            // 
            // details2MenuItem
            // 
            this.details2MenuItem.Image = global::udUserSecurity.Properties.Resources.VW_DTLS;
            this.details2MenuItem.Name = "details2MenuItem";
            this.details2MenuItem.Size = new System.Drawing.Size(152, 22);
            this.details2MenuItem.Text = "Details";
            this.details2MenuItem.Click += new System.EventHandler(this.detailsMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Image = global::udUserSecurity.Properties.Resources.refresh;
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem8.Text = "Refresh";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // prop2MenuItem
            // 
            this.prop2MenuItem.Image = global::udUserSecurity.Properties.Resources.roleproperties;
            this.prop2MenuItem.Name = "prop2MenuItem";
            this.prop2MenuItem.Size = new System.Drawing.Size(152, 22);
            this.prop2MenuItem.Text = "Properties";
            this.prop2MenuItem.Click += new System.EventHandler(this.prop2MenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.detailsradioButton);
            this.groupBox1.Controls.Add(this.listradioButton);
            this.groupBox1.Controls.Add(this.smallradioButton);
            this.groupBox1.Controls.Add(this.largeradioButton);
            this.groupBox1.Location = new System.Drawing.Point(55, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(116, 33);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // detailsradioButton
            // 
            this.detailsradioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.detailsradioButton.AutoSize = true;
            this.detailsradioButton.Image = global::udUserSecurity.Properties.Resources.VW_DTLS;
            this.detailsradioButton.Location = new System.Drawing.Point(89, 9);
            this.detailsradioButton.Name = "detailsradioButton";
            this.detailsradioButton.Size = new System.Drawing.Size(22, 22);
            this.detailsradioButton.TabIndex = 3;
            this.detailsradioButton.TabStop = true;
            this.detailsradioButton.UseVisualStyleBackColor = true;
            this.detailsradioButton.CheckedChanged += new System.EventHandler(this.detailsradioButton_CheckedChanged);
            // 
            // listradioButton
            // 
            this.listradioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.listradioButton.AutoSize = true;
            this.listradioButton.Image = global::udUserSecurity.Properties.Resources.VW_LIST;
            this.listradioButton.Location = new System.Drawing.Point(61, 9);
            this.listradioButton.Name = "listradioButton";
            this.listradioButton.Size = new System.Drawing.Size(22, 22);
            this.listradioButton.TabIndex = 2;
            this.listradioButton.TabStop = true;
            this.listradioButton.UseVisualStyleBackColor = true;
            this.listradioButton.CheckedChanged += new System.EventHandler(this.listradioButton_CheckedChanged);
            // 
            // smallradioButton
            // 
            this.smallradioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.smallradioButton.AutoSize = true;
            this.smallradioButton.Image = global::udUserSecurity.Properties.Resources.VW_SMLIC;
            this.smallradioButton.Location = new System.Drawing.Point(33, 9);
            this.smallradioButton.Name = "smallradioButton";
            this.smallradioButton.Size = new System.Drawing.Size(22, 22);
            this.smallradioButton.TabIndex = 1;
            this.smallradioButton.TabStop = true;
            this.smallradioButton.UseVisualStyleBackColor = true;
            this.smallradioButton.CheckedChanged += new System.EventHandler(this.smallradioButton_CheckedChanged);
            // 
            // largeradioButton
            // 
            this.largeradioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.largeradioButton.AutoSize = true;
            this.largeradioButton.Image = global::udUserSecurity.Properties.Resources.VW_LRGIC;
            this.largeradioButton.Location = new System.Drawing.Point(5, 9);
            this.largeradioButton.Name = "largeradioButton";
            this.largeradioButton.Size = new System.Drawing.Size(22, 22);
            this.largeradioButton.TabIndex = 0;
            this.largeradioButton.TabStop = true;
            this.largeradioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.largeradioButton.UseVisualStyleBackColor = true;
            this.largeradioButton.CheckedChanged += new System.EventHandler(this.largeradioButton_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmUserSecurity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 478);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmUserSecurity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Security";
            this.Load += new System.EventHandler(this.frmUserSecurity_Load);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newroleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteroleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyroleMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem largeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem refreshMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prop1MenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem newuserMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteuserMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyuserMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem large2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem small2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem list2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem details2MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem prop2MenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton largeradioButton;
        private System.Windows.Forms.RadioButton detailsradioButton;
        private System.Windows.Forms.RadioButton listradioButton;
        private System.Windows.Forms.RadioButton smallradioButton;
        private System.Windows.Forms.Timer timer1;
    }
}

