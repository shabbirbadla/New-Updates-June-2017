namespace eMailClient
{
    partial class frmQueryWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQueryWindow));
            this.txt_query = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Undo1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Cut1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Copy1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Paste1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAll1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Execute1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnMoveFldAll = new System.Windows.Forms.Button();
            this.btnMoveFldOne = new System.Windows.Forms.Button();
            this.txt_reportquery = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.Execute = new System.Windows.Forms.ToolStripMenuItem();
            this.rbtnSP = new System.Windows.Forms.RadioButton();
            this.rbtnQuery = new System.Windows.Forms.RadioButton();
            this.lstvwParameters = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.lstvwFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblQryP2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Id = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblQryP1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_query
            // 
            this.txt_query.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_query.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_query.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_query.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_query.Location = new System.Drawing.Point(15, 35);
            this.txt_query.Multiline = true;
            this.txt_query.Name = "txt_query";
            this.txt_query.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_query.Size = new System.Drawing.Size(449, 152);
            this.txt_query.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txt_query, "Enter a process query");
            this.txt_query.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_query_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Undo1,
            this.toolStripSeparator3,
            this.Cut1,
            this.Copy1,
            this.Paste1,
            this.Delete1,
            this.toolStripSeparator4,
            this.SelectAll1,
            this.Execute1});
            this.contextMenuStrip1.Name = "contextMenuStrip2";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 170);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // Undo1
            // 
            this.Undo1.Name = "Undo1";
            this.Undo1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.Z)));
            this.Undo1.Size = new System.Drawing.Size(167, 22);
            this.Undo1.Text = "&Undo";
            this.Undo1.Click += new System.EventHandler(this.Undo1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(164, 6);
            // 
            // Cut1
            // 
            this.Cut1.Name = "Cut1";
            this.Cut1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.Cut1.Size = new System.Drawing.Size(167, 22);
            this.Cut1.Text = "Cu&t";
            this.Cut1.Click += new System.EventHandler(this.Cut1_Click);
            // 
            // Copy1
            // 
            this.Copy1.Name = "Copy1";
            this.Copy1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Copy1.Size = new System.Drawing.Size(167, 22);
            this.Copy1.Text = "&Copy";
            this.Copy1.Click += new System.EventHandler(this.Copy1_Click);
            // 
            // Paste1
            // 
            this.Paste1.Name = "Paste1";
            this.Paste1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Paste1.Size = new System.Drawing.Size(167, 22);
            this.Paste1.Text = "&Paste";
            this.Paste1.Click += new System.EventHandler(this.Paste1_Click);
            // 
            // Delete1
            // 
            this.Delete1.Name = "Delete1";
            this.Delete1.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.Delete1.Size = new System.Drawing.Size(167, 22);
            this.Delete1.Text = "&Delete";
            this.Delete1.Click += new System.EventHandler(this.Delete1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(164, 6);
            // 
            // SelectAll1
            // 
            this.SelectAll1.Name = "SelectAll1";
            this.SelectAll1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SelectAll1.Size = new System.Drawing.Size(167, 22);
            this.SelectAll1.Text = "Select &All";
            this.SelectAll1.Click += new System.EventHandler(this.SelectAll1_Click);
            // 
            // Execute1
            // 
            this.Execute1.Name = "Execute1";
            this.Execute1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.Execute1.Size = new System.Drawing.Size(167, 22);
            this.Execute1.Text = "&Execute";
            this.Execute1.Click += new System.EventHandler(this.Execute1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(727, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(38, 24);
            this.btnCancel.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel (Ctrl+Z)");
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(685, 445);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(38, 24);
            this.btnOk.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnOk, "Ok (Ctrl+O)");
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Image = ((System.Drawing.Image)(resources.GetObject("btnExecute.Image")));
            this.btnExecute.Location = new System.Drawing.Point(114, 13);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(28, 20);
            this.btnExecute.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnExecute, "Execute (F5)");
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnMoveFldAll
            // 
            this.btnMoveFldAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveFldAll.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnMoveFldAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveFldAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveFldAll.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveFldAll.Image")));
            this.btnMoveFldAll.Location = new System.Drawing.Point(565, 221);
            this.btnMoveFldAll.Name = "btnMoveFldAll";
            this.btnMoveFldAll.Size = new System.Drawing.Size(25, 20);
            this.btnMoveFldAll.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnMoveFldAll, "Click to shift all the fields to parameters list");
            this.btnMoveFldAll.UseVisualStyleBackColor = true;
            this.btnMoveFldAll.Click += new System.EventHandler(this.btnMoveFldAll_Click);
            // 
            // btnMoveFldOne
            // 
            this.btnMoveFldOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveFldOne.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnMoveFldOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveFldOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveFldOne.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveFldOne.Image")));
            this.btnMoveFldOne.Location = new System.Drawing.Point(565, 190);
            this.btnMoveFldOne.Name = "btnMoveFldOne";
            this.btnMoveFldOne.Size = new System.Drawing.Size(25, 20);
            this.btnMoveFldOne.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnMoveFldOne, "Click to shift single field to parameters list");
            this.btnMoveFldOne.UseVisualStyleBackColor = false;
            this.btnMoveFldOne.Click += new System.EventHandler(this.btnMoveFldOne_Click);
            // 
            // txt_reportquery
            // 
            this.txt_reportquery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_reportquery.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_reportquery.ContextMenuStrip = this.contextMenuStrip2;
            this.txt_reportquery.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_reportquery.Location = new System.Drawing.Point(15, 247);
            this.txt_reportquery.Multiline = true;
            this.txt_reportquery.Name = "txt_reportquery";
            this.txt_reportquery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_reportquery.Size = new System.Drawing.Size(449, 152);
            this.txt_reportquery.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txt_reportquery, "Enter a report query");
            this.txt_reportquery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_reportquery_KeyDown);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Undo,
            this.toolStripSeparator1,
            this.Cut,
            this.Copy,
            this.Paste,
            this.Delete,
            this.toolStripSeparator2,
            this.SelectAll,
            this.Execute});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(168, 170);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // Undo
            // 
            this.Undo.Name = "Undo";
            this.Undo.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.Z)));
            this.Undo.Size = new System.Drawing.Size(167, 22);
            this.Undo.Text = "&Undo";
            this.Undo.Click += new System.EventHandler(this.Undo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // Cut
            // 
            this.Cut.Name = "Cut";
            this.Cut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.Cut.Size = new System.Drawing.Size(167, 22);
            this.Cut.Text = "Cu&t";
            this.Cut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // Copy
            // 
            this.Copy.Name = "Copy";
            this.Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Copy.Size = new System.Drawing.Size(167, 22);
            this.Copy.Text = "&Copy";
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // Paste
            // 
            this.Paste.Name = "Paste";
            this.Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Paste.Size = new System.Drawing.Size(167, 22);
            this.Paste.Text = "&Paste";
            this.Paste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.Delete.Size = new System.Drawing.Size(167, 22);
            this.Delete.Text = "&Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(164, 6);
            // 
            // SelectAll
            // 
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SelectAll.Size = new System.Drawing.Size(167, 22);
            this.SelectAll.Text = "Select &All";
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // Execute
            // 
            this.Execute.Name = "Execute";
            this.Execute.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.Execute.Size = new System.Drawing.Size(167, 22);
            this.Execute.Text = "&Execute";
            // 
            // rbtnSP
            // 
            this.rbtnSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbtnSP.AutoSize = true;
            this.rbtnSP.Location = new System.Drawing.Point(270, 227);
            this.rbtnSP.Name = "rbtnSP";
            this.rbtnSP.Size = new System.Drawing.Size(108, 17);
            this.rbtnSP.TabIndex = 32;
            this.rbtnSP.Text = "Stored Procedure";
            this.toolTip1.SetToolTip(this.rbtnSP, "Click if query type is stored procedure");
            this.rbtnSP.UseVisualStyleBackColor = true;
            // 
            // rbtnQuery
            // 
            this.rbtnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbtnQuery.AutoSize = true;
            this.rbtnQuery.Location = new System.Drawing.Point(174, 227);
            this.rbtnQuery.Name = "rbtnQuery";
            this.rbtnQuery.Size = new System.Drawing.Size(53, 17);
            this.rbtnQuery.TabIndex = 31;
            this.rbtnQuery.Text = "Query";
            this.toolTip1.SetToolTip(this.rbtnQuery, "Click if query type is query");
            this.rbtnQuery.UseVisualStyleBackColor = true;
            // 
            // lstvwParameters
            // 
            this.lstvwParameters.AllowDrop = true;
            this.lstvwParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvwParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lstvwParameters.FullRowSelect = true;
            this.lstvwParameters.GridLines = true;
            this.lstvwParameters.Location = new System.Drawing.Point(470, 245);
            this.lstvwParameters.Name = "lstvwParameters";
            this.lstvwParameters.ShowItemToolTips = true;
            this.lstvwParameters.Size = new System.Drawing.Size(230, 152);
            this.lstvwParameters.TabIndex = 8;
            this.toolTip1.SetToolTip(this.lstvwParameters, "Parameters List");
            this.lstvwParameters.UseCompatibleStateImageBehavior = false;
            this.lstvwParameters.View = System.Windows.Forms.View.Details;
            this.lstvwParameters.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstvwParameters_DragDrop);
            this.lstvwParameters.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstvwParameters_MouseDown);
            this.lstvwParameters.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstvwParameters_DragEnter);
            this.lstvwParameters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstvwParameters_KeyDown);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Parameters";
            this.columnHeader2.Width = 221;
            // 
            // lstvwFields
            // 
            this.lstvwFields.AllowDrop = true;
            this.lstvwFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvwFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstvwFields.FullRowSelect = true;
            this.lstvwFields.GridLines = true;
            this.lstvwFields.Location = new System.Drawing.Point(470, 35);
            this.lstvwFields.Name = "lstvwFields";
            this.lstvwFields.ShowItemToolTips = true;
            this.lstvwFields.Size = new System.Drawing.Size(230, 152);
            this.lstvwFields.TabIndex = 3;
            this.toolTip1.SetToolTip(this.lstvwFields, "Database Fields List");
            this.lstvwFields.UseCompatibleStateImageBehavior = false;
            this.lstvwFields.View = System.Windows.Forms.View.Details;
            this.lstvwFields.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstvwFields_MouseMove);
            this.lstvwFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstvwFields_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Fields";
            this.columnHeader1.Width = 221;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbtnSP);
            this.groupBox1.Controls.Add(this.rbtnQuery);
            this.groupBox1.Controls.Add(this.btnMoveFldAll);
            this.groupBox1.Controls.Add(this.btnMoveFldOne);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.txt_query);
            this.groupBox1.Controls.Add(this.lstvwParameters);
            this.groupBox1.Controls.Add(this.lstvwFields);
            this.groupBox1.Controls.Add(this.btnExecute);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl_Id);
            this.groupBox1.Controls.Add(this.txt_reportquery);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(9, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 440);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lblQryP2);
            this.panel2.Location = new System.Drawing.Point(14, 399);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(449, 31);
            this.panel2.TabIndex = 29;
            // 
            // lblQryP2
            // 
            this.lblQryP2.AutoSize = true;
            this.lblQryP2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQryP2.Location = new System.Drawing.Point(2, 1);
            this.lblQryP2.Name = "lblQryP2";
            this.lblQryP2.Size = new System.Drawing.Size(16, 13);
            this.lblQryP2.TabIndex = 0;
            this.lblQryP2.Text = "M";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Report Query";
            // 
            // lbl_Id
            // 
            this.lbl_Id.AutoSize = true;
            this.lbl_Id.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Id.Location = new System.Drawing.Point(12, 17);
            this.lbl_Id.Name = "lbl_Id";
            this.lbl_Id.Size = new System.Drawing.Size(90, 13);
            this.lbl_Id.TabIndex = 0;
            this.lbl_Id.Text = "Process Query";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblQryP1);
            this.panel1.Location = new System.Drawing.Point(15, 187);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 31);
            this.panel1.TabIndex = 24;
            // 
            // lblQryP1
            // 
            this.lblQryP1.AutoSize = true;
            this.lblQryP1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQryP1.Location = new System.Drawing.Point(2, 2);
            this.lblQryP1.Name = "lblQryP1";
            this.lblQryP1.Size = new System.Drawing.Size(16, 13);
            this.lblQryP1.TabIndex = 0;
            this.lblQryP1.Text = "M";
            // 
            // frmQueryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(773, 472);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmQueryWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Window";
            this.Load += new System.EventHandler(this.frmQueryWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmQueryWindow_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_query;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txt_reportquery;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_Id;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ListView lstvwParameters;
        private System.Windows.Forms.ListView lstvwFields;
        private System.Windows.Forms.Label lblQryP1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblQryP2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnMoveFldAll;
        private System.Windows.Forms.Button btnMoveFldOne;
        private System.Windows.Forms.RadioButton rbtnSP;
        private System.Windows.Forms.RadioButton rbtnQuery;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem Undo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Cut;
        private System.Windows.Forms.ToolStripMenuItem Copy;
        private System.Windows.Forms.ToolStripMenuItem Paste;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem SelectAll;
        private System.Windows.Forms.ToolStripMenuItem Execute;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Undo1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem Cut1;
        private System.Windows.Forms.ToolStripMenuItem Copy1;
        private System.Windows.Forms.ToolStripMenuItem Paste1;
        private System.Windows.Forms.ToolStripMenuItem Delete1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem SelectAll1;
        private System.Windows.Forms.ToolStripMenuItem Execute1;
    }
}