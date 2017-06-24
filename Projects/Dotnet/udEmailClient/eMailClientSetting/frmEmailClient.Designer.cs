namespace eMailClient
{
    partial class frmEmailClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmailClient));
            this.tlsMain = new System.Windows.Forms.ToolStrip();
            this.tlsbtnAdd = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnEdit = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbtnSearch = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tlsbtnSend = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnPendingMails = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRepDesc = new System.Windows.Forms.TextBox();
            this.btnSelReport = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkDigSig = new System.Windows.Forms.CheckBox();
            this.lbllogemailid = new System.Windows.Forms.Label();
            this.txtlogemailid = new System.Windows.Forms.TextBox();
            this.chkemaillogfiles = new System.Windows.Forms.CheckBox();
            this.chkremovefiles = new System.Windows.Forms.CheckBox();
            this.txt_filenameprefix = new System.Windows.Forms.TextBox();
            this.btnQueryWin = new System.Windows.Forms.Button();
            this.lbl_filenameprefix = new System.Windows.Forms.Label();
            this.btnExportPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExportPath = new System.Windows.Forms.TextBox();
            this.btn_rep_nm = new System.Windows.Forms.Button();
            this.lbl_rep_nm = new System.Windows.Forms.Label();
            this.cmb_attachment_typ = new System.Windows.Forms.ComboBox();
            this.txt_rep_nm = new System.Windows.Forms.TextBox();
            this.chk_hasattachment = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_attachment_typ = new System.Windows.Forms.Label();
            this.btn_Zoom = new System.Windows.Forms.Button();
            this.chklst_tran_typ = new System.Windows.Forms.ListView();
            this.colhdr_trantyp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhdr_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Bcc = new System.Windows.Forms.Button();
            this.btn_Cc = new System.Windows.Forms.Button();
            this.btn_To = new System.Windows.Forms.Button();
            this.txt_body = new System.Windows.Forms.TextBox();
            this.txt_subject = new System.Windows.Forms.TextBox();
            this.lbl_subject = new System.Windows.Forms.Label();
            this.txt_bcc = new System.Windows.Forms.TextBox();
            this.txt_cc = new System.Windows.Forms.TextBox();
            this.txt_to = new System.Windows.Forms.TextBox();
            this.lbl_bcc = new System.Windows.Forms.Label();
            this.lbl_cc = new System.Windows.Forms.Label();
            this.lbl_to = new System.Windows.Forms.Label();
            this.lblSelReport = new System.Windows.Forms.Label();
            this.lbl_tran_typ = new System.Windows.Forms.Label();
            this.txt_desc = new System.Windows.Forms.TextBox();
            this.lbl_desc = new System.Windows.Forms.Label();
            this.txt_Id = new System.Windows.Forms.TextBox();
            this.lbl_Id = new System.Windows.Forms.Label();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fbd1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tlsMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tlsMain
            // 
            this.tlsMain.AutoSize = false;
            this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsbtnAdd,
            this.tlsbtnEdit,
            this.tlsbtnDelete,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.tlsbtnSave,
            this.tlsbtnCancel,
            this.toolStripSeparator1,
            this.tlsbtnSearch,
            this.tlsbtnExit,
            this.toolStripSeparator3,
            this.toolStripLabel2,
            this.tlsbtnSend,
            this.tlsbtnPendingMails});
            this.tlsMain.Location = new System.Drawing.Point(0, 0);
            this.tlsMain.Name = "tlsMain";
            this.tlsMain.Size = new System.Drawing.Size(980, 25);
            this.tlsMain.TabIndex = 0;
            // 
            // tlsbtnAdd
            // 
            this.tlsbtnAdd.Enabled = false;
            this.tlsbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnAdd.Image")));
            this.tlsbtnAdd.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnAdd.Name = "tlsbtnAdd";
            this.tlsbtnAdd.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnAdd.ToolTipText = "Add (Ctrl+A)";
            this.tlsbtnAdd.Click += new System.EventHandler(this.tlsbtnAdd_Click);
            // 
            // tlsbtnEdit
            // 
            this.tlsbtnEdit.Enabled = false;
            this.tlsbtnEdit.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnEdit.Image")));
            this.tlsbtnEdit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnEdit.Name = "tlsbtnEdit";
            this.tlsbtnEdit.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnEdit.ToolTipText = "Edit (Ctrl+E)";
            this.tlsbtnEdit.Click += new System.EventHandler(this.tlsbtnEdit_Click);
            // 
            // tlsbtnDelete
            // 
            this.tlsbtnDelete.Enabled = false;
            this.tlsbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnDelete.Image")));
            this.tlsbtnDelete.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnDelete.Name = "tlsbtnDelete";
            this.tlsbtnDelete.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnDelete.ToolTipText = "Delete (Ctrl+D)";
            this.tlsbtnDelete.Click += new System.EventHandler(this.tlsbtnDelete_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel1.Enabled = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(16, 22);
            this.toolStripLabel1.Text = "   ";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsbtnSave
            // 
            this.tlsbtnSave.Enabled = false;
            this.tlsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnSave.Image")));
            this.tlsbtnSave.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnSave.Name = "tlsbtnSave";
            this.tlsbtnSave.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnSave.ToolTipText = "Save (Ctrl+S)";
            this.tlsbtnSave.Click += new System.EventHandler(this.tlsbtnSave_Click);
            // 
            // tlsbtnCancel
            // 
            this.tlsbtnCancel.Enabled = false;
            this.tlsbtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnCancel.Image")));
            this.tlsbtnCancel.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnCancel.Name = "tlsbtnCancel";
            this.tlsbtnCancel.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnCancel.ToolTipText = "Cancel (Ctrl+Z)";
            this.tlsbtnCancel.Click += new System.EventHandler(this.tlsbtnCancel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsbtnSearch
            // 
            this.tlsbtnSearch.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnSearch.Image")));
            this.tlsbtnSearch.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnSearch.Name = "tlsbtnSearch";
            this.tlsbtnSearch.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnSearch.ToolTipText = "Search (F2)";
            this.tlsbtnSearch.Click += new System.EventHandler(this.tlsbtnSearch_Click);
            // 
            // tlsbtnExit
            // 
            this.tlsbtnExit.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnExit.Image")));
            this.tlsbtnExit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tlsbtnExit.Name = "tlsbtnExit";
            this.tlsbtnExit.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnExit.ToolTipText = "Exit (Alt+F4)";
            this.tlsbtnExit.Click += new System.EventHandler(this.tlsbtnExit_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(25, 22);
            // 
            // tlsbtnSend
            // 
            this.tlsbtnSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlsbtnSend.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnSend.Image")));
            this.tlsbtnSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbtnSend.Name = "tlsbtnSend";
            this.tlsbtnSend.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnSend.ToolTipText = "Send Email (Ctrl+F)";
            this.tlsbtnSend.Click += new System.EventHandler(this.tlsbtnSend_Click);
            // 
            // tlsbtnPendingMails
            // 
            this.tlsbtnPendingMails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlsbtnPendingMails.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnPendingMails.Image")));
            this.tlsbtnPendingMails.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbtnPendingMails.Name = "tlsbtnPendingMails";
            this.tlsbtnPendingMails.Size = new System.Drawing.Size(23, 22);
            this.tlsbtnPendingMails.ToolTipText = "Send Pending Email (Ctrl+P)";
            this.tlsbtnPendingMails.Click += new System.EventHandler(this.tlsbtnPendingMails_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtRepDesc);
            this.groupBox1.Controls.Add(this.btnSelReport);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btn_Zoom);
            this.groupBox1.Controls.Add(this.chklst_tran_typ);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.lblSelReport);
            this.groupBox1.Controls.Add(this.lbl_tran_typ);
            this.groupBox1.Controls.Add(this.txt_desc);
            this.groupBox1.Controls.Add(this.lbl_desc);
            this.groupBox1.Controls.Add(this.txt_Id);
            this.groupBox1.Controls.Add(this.lbl_Id);
            this.groupBox1.Location = new System.Drawing.Point(6, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(967, 550);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtRepDesc
            // 
            this.txtRepDesc.Enabled = false;
            this.txtRepDesc.Location = new System.Drawing.Point(456, 41);
            this.txtRepDesc.Name = "txtRepDesc";
            this.txtRepDesc.Size = new System.Drawing.Size(507, 20);
            this.txtRepDesc.TabIndex = 11;
            // 
            // btnSelReport
            // 
            this.btnSelReport.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelReport.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSelReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelReport.Image = ((System.Drawing.Image)(resources.GetObject("btnSelReport.Image")));
            this.btnSelReport.Location = new System.Drawing.Point(542, 17);
            this.btnSelReport.Name = "btnSelReport";
            this.btnSelReport.Size = new System.Drawing.Size(34, 23);
            this.btnSelReport.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnSelReport, "Zoom to select transaction types");
            this.btnSelReport.UseVisualStyleBackColor = false;
            this.btnSelReport.Click += new System.EventHandler(this.btnSelReport_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.chkDigSig);
            this.groupBox3.Controls.Add(this.lbllogemailid);
            this.groupBox3.Controls.Add(this.txtlogemailid);
            this.groupBox3.Controls.Add(this.chkemaillogfiles);
            this.groupBox3.Controls.Add(this.chkremovefiles);
            this.groupBox3.Controls.Add(this.txt_filenameprefix);
            this.groupBox3.Controls.Add(this.btnQueryWin);
            this.groupBox3.Controls.Add(this.lbl_filenameprefix);
            this.groupBox3.Controls.Add(this.btnExportPath);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtExportPath);
            this.groupBox3.Controls.Add(this.btn_rep_nm);
            this.groupBox3.Controls.Add(this.lbl_rep_nm);
            this.groupBox3.Controls.Add(this.cmb_attachment_typ);
            this.groupBox3.Controls.Add(this.txt_rep_nm);
            this.groupBox3.Controls.Add(this.chk_hasattachment);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.lbl_attachment_typ);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(13, 134);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(938, 107);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Attachment Settings";
            // 
            // chkDigSig
            // 
            this.chkDigSig.AutoSize = true;
            this.chkDigSig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDigSig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDigSig.Location = new System.Drawing.Point(170, 82);
            this.chkDigSig.Name = "chkDigSig";
            this.chkDigSig.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkDigSig.Size = new System.Drawing.Size(100, 17);
            this.chkDigSig.TabIndex = 18;
            this.chkDigSig.Text = "Digital Signature";
            this.chkDigSig.UseVisualStyleBackColor = true;
            // 
            // lbllogemailid
            // 
            this.lbllogemailid.AutoSize = true;
            this.lbllogemailid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllogemailid.Location = new System.Drawing.Point(439, 85);
            this.lbllogemailid.Name = "lbllogemailid";
            this.lbllogemailid.Size = new System.Drawing.Size(44, 13);
            this.lbllogemailid.TabIndex = 15;
            this.lbllogemailid.Text = "Email Id";
            // 
            // txtlogemailid
            // 
            this.txtlogemailid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtlogemailid.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlogemailid.Location = new System.Drawing.Point(558, 81);
            this.txtlogemailid.Multiline = true;
            this.txtlogemailid.Name = "txtlogemailid";
            this.txtlogemailid.Size = new System.Drawing.Size(273, 21);
            this.txtlogemailid.TabIndex = 16;
            this.toolTip1.SetToolTip(this.txtlogemailid, "Enter emailid(s)");
            // 
            // chkemaillogfiles
            // 
            this.chkemaillogfiles.AutoSize = true;
            this.chkemaillogfiles.BackColor = System.Drawing.SystemColors.Control;
            this.chkemaillogfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkemaillogfiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkemaillogfiles.Location = new System.Drawing.Point(315, 83);
            this.chkemaillogfiles.Name = "chkemaillogfiles";
            this.chkemaillogfiles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkemaillogfiles.Size = new System.Drawing.Size(86, 17);
            this.chkemaillogfiles.TabIndex = 14;
            this.chkemaillogfiles.Text = "&Email log files";
            this.toolTip1.SetToolTip(this.chkemaillogfiles, "Check if email log files");
            this.chkemaillogfiles.UseVisualStyleBackColor = false;
            this.chkemaillogfiles.CheckedChanged += new System.EventHandler(this.chkemaillogfiles_CheckedChanged);
            // 
            // chkremovefiles
            // 
            this.chkremovefiles.AutoSize = true;
            this.chkremovefiles.BackColor = System.Drawing.SystemColors.Control;
            this.chkremovefiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkremovefiles.Location = new System.Drawing.Point(130, 83);
            this.chkremovefiles.Name = "chkremovefiles";
            this.chkremovefiles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkremovefiles.Size = new System.Drawing.Size(12, 11);
            this.chkremovefiles.TabIndex = 13;
            this.toolTip1.SetToolTip(this.chkremovefiles, "Check if exported files should be removed");
            this.chkremovefiles.UseVisualStyleBackColor = false;
            // 
            // txt_filenameprefix
            // 
            this.txt_filenameprefix.BackColor = System.Drawing.Color.White;
            this.txt_filenameprefix.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_filenameprefix.Location = new System.Drawing.Point(130, 53);
            this.txt_filenameprefix.Name = "txt_filenameprefix";
            this.txt_filenameprefix.Size = new System.Drawing.Size(295, 21);
            this.txt_filenameprefix.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txt_filenameprefix, "Export file name prefix (Eg: \"Sales\",Party_nm,Inv_no,etc)");
            // 
            // btnQueryWin
            // 
            this.btnQueryWin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQueryWin.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnQueryWin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQueryWin.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryWin.Image")));
            this.btnQueryWin.Location = new System.Drawing.Point(878, 30);
            this.btnQueryWin.Name = "btnQueryWin";
            this.btnQueryWin.Size = new System.Drawing.Size(53, 37);
            this.btnQueryWin.TabIndex = 17;
            this.toolTip1.SetToolTip(this.btnQueryWin, "Click to write a Query");
            this.btnQueryWin.UseVisualStyleBackColor = false;
            this.btnQueryWin.Click += new System.EventHandler(this.btnQueryWin_Click);
            // 
            // lbl_filenameprefix
            // 
            this.lbl_filenameprefix.AutoSize = true;
            this.lbl_filenameprefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_filenameprefix.Location = new System.Drawing.Point(11, 57);
            this.lbl_filenameprefix.Name = "lbl_filenameprefix";
            this.lbl_filenameprefix.Size = new System.Drawing.Size(83, 13);
            this.lbl_filenameprefix.TabIndex = 7;
            this.lbl_filenameprefix.Text = "&File Name Prefix";
            // 
            // btnExportPath
            // 
            this.btnExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportPath.BackColor = System.Drawing.SystemColors.Control;
            this.btnExportPath.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnExportPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPath.Image = ((System.Drawing.Image)(resources.GetObject("btnExportPath.Image")));
            this.btnExportPath.Location = new System.Drawing.Point(834, 52);
            this.btnExportPath.Name = "btnExportPath";
            this.btnExportPath.Size = new System.Drawing.Size(26, 23);
            this.btnExportPath.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnExportPath, "Choose export path");
            this.btnExportPath.UseVisualStyleBackColor = false;
            this.btnExportPath.Click += new System.EventHandler(this.btnExportPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(439, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Export Path";
            // 
            // txtExportPath
            // 
            this.txtExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExportPath.BackColor = System.Drawing.Color.White;
            this.txtExportPath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExportPath.Location = new System.Drawing.Point(558, 53);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Size = new System.Drawing.Size(273, 21);
            this.txtExportPath.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtExportPath, "Enter export path");
            // 
            // btn_rep_nm
            // 
            this.btn_rep_nm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_rep_nm.BackColor = System.Drawing.SystemColors.Control;
            this.btn_rep_nm.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_rep_nm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_rep_nm.Image = ((System.Drawing.Image)(resources.GetObject("btn_rep_nm.Image")));
            this.btn_rep_nm.Location = new System.Drawing.Point(834, 24);
            this.btn_rep_nm.Name = "btn_rep_nm";
            this.btn_rep_nm.Size = new System.Drawing.Size(26, 23);
            this.btn_rep_nm.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btn_rep_nm, "Choose report");
            this.btn_rep_nm.UseVisualStyleBackColor = false;
            this.btn_rep_nm.Click += new System.EventHandler(this.btn_rep_nm_Click);
            // 
            // lbl_rep_nm
            // 
            this.lbl_rep_nm.AutoSize = true;
            this.lbl_rep_nm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_rep_nm.Location = new System.Drawing.Point(439, 29);
            this.lbl_rep_nm.Name = "lbl_rep_nm";
            this.lbl_rep_nm.Size = new System.Drawing.Size(95, 13);
            this.lbl_rep_nm.TabIndex = 4;
            this.lbl_rep_nm.Text = "&Report Path Name";
            // 
            // cmb_attachment_typ
            // 
            this.cmb_attachment_typ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_attachment_typ.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmb_attachment_typ.FormattingEnabled = true;
            this.cmb_attachment_typ.Items.AddRange(new object[] {
            "PDF",
            "Excel",
            "CSV",
            "XML"});
            this.cmb_attachment_typ.Location = new System.Drawing.Point(269, 25);
            this.cmb_attachment_typ.Name = "cmb_attachment_typ";
            this.cmb_attachment_typ.Size = new System.Drawing.Size(156, 21);
            this.cmb_attachment_typ.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cmb_attachment_typ, "Select attachment type");
            this.cmb_attachment_typ.SelectedIndexChanged += new System.EventHandler(this.cmb_attachment_typ_SelectedIndexChanged);
            // 
            // txt_rep_nm
            // 
            this.txt_rep_nm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_rep_nm.BackColor = System.Drawing.Color.White;
            this.txt_rep_nm.Enabled = false;
            this.txt_rep_nm.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_rep_nm.Location = new System.Drawing.Point(558, 25);
            this.txt_rep_nm.Name = "txt_rep_nm";
            this.txt_rep_nm.Size = new System.Drawing.Size(273, 21);
            this.txt_rep_nm.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txt_rep_nm, "Enter report path with name");
            // 
            // chk_hasattachment
            // 
            this.chk_hasattachment.AutoSize = true;
            this.chk_hasattachment.BackColor = System.Drawing.SystemColors.Control;
            this.chk_hasattachment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_hasattachment.Location = new System.Drawing.Point(130, 31);
            this.chk_hasattachment.Name = "chk_hasattachment";
            this.chk_hasattachment.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chk_hasattachment.Size = new System.Drawing.Size(12, 11);
            this.chk_hasattachment.TabIndex = 1;
            this.toolTip1.SetToolTip(this.chk_hasattachment, "Check if email has an attachment");
            this.chk_hasattachment.UseVisualStyleBackColor = false;
            this.chk_hasattachment.CheckedChanged += new System.EventHandler(this.chk_hasattachment_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "&Remove Exported Files";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Has Attachment";
            // 
            // lbl_attachment_typ
            // 
            this.lbl_attachment_typ.AutoSize = true;
            this.lbl_attachment_typ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_attachment_typ.Location = new System.Drawing.Point(164, 29);
            this.lbl_attachment_typ.Name = "lbl_attachment_typ";
            this.lbl_attachment_typ.Size = new System.Drawing.Size(91, 13);
            this.lbl_attachment_typ.TabIndex = 2;
            this.lbl_attachment_typ.Text = "&Attachment Type ";
            // 
            // btn_Zoom
            // 
            this.btn_Zoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Zoom.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Zoom.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_Zoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Zoom.Image = ((System.Drawing.Image)(resources.GetObject("btn_Zoom.Image")));
            this.btn_Zoom.Location = new System.Drawing.Point(910, 14);
            this.btn_Zoom.Name = "btn_Zoom";
            this.btn_Zoom.Size = new System.Drawing.Size(34, 23);
            this.btn_Zoom.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btn_Zoom, "Zoom to select transaction types");
            this.btn_Zoom.UseVisualStyleBackColor = false;
            this.btn_Zoom.Visible = false;
            this.btn_Zoom.Click += new System.EventHandler(this.btn_Zoom_Click);
            // 
            // chklst_tran_typ
            // 
            this.chklst_tran_typ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chklst_tran_typ.CheckBoxes = true;
            this.chklst_tran_typ.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhdr_trantyp,
            this.colhdr_name});
            this.chklst_tran_typ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chklst_tran_typ.FullRowSelect = true;
            this.chklst_tran_typ.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.chklst_tran_typ.Location = new System.Drawing.Point(456, 41);
            this.chklst_tran_typ.Name = "chklst_tran_typ";
            this.chklst_tran_typ.ShowItemToolTips = true;
            this.chklst_tran_typ.Size = new System.Drawing.Size(507, 88);
            this.chklst_tran_typ.TabIndex = 8;
            this.toolTip1.SetToolTip(this.chklst_tran_typ, "Select Transaction Type");
            this.chklst_tran_typ.UseCompatibleStateImageBehavior = false;
            this.chklst_tran_typ.View = System.Windows.Forms.View.Details;
            this.chklst_tran_typ.Visible = false;
            this.chklst_tran_typ.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.chklst_tran_typ_ItemChecked);
            // 
            // colhdr_trantyp
            // 
            this.colhdr_trantyp.Text = "Entry Type";
            this.colhdr_trantyp.Width = 100;
            // 
            // colhdr_name
            // 
            this.colhdr_name.Text = "Name";
            this.colhdr_name.Width = 300;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btn_Bcc);
            this.groupBox2.Controls.Add(this.btn_Cc);
            this.groupBox2.Controls.Add(this.btn_To);
            this.groupBox2.Controls.Add(this.txt_body);
            this.groupBox2.Controls.Add(this.txt_subject);
            this.groupBox2.Controls.Add(this.lbl_subject);
            this.groupBox2.Controls.Add(this.txt_bcc);
            this.groupBox2.Controls.Add(this.txt_cc);
            this.groupBox2.Controls.Add(this.txt_to);
            this.groupBox2.Controls.Add(this.lbl_bcc);
            this.groupBox2.Controls.Add(this.lbl_cc);
            this.groupBox2.Controls.Add(this.lbl_to);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(938, 288);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " &Mail Settings";
            // 
            // btn_Bcc
            // 
            this.btn_Bcc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Bcc.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Bcc.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_Bcc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Bcc.Image = ((System.Drawing.Image)(resources.GetObject("btn_Bcc.Image")));
            this.btn_Bcc.Location = new System.Drawing.Point(897, 77);
            this.btn_Bcc.Name = "btn_Bcc";
            this.btn_Bcc.Size = new System.Drawing.Size(34, 23);
            this.btn_Bcc.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btn_Bcc, "Click to select emailid(s)");
            this.btn_Bcc.UseVisualStyleBackColor = false;
            this.btn_Bcc.Click += new System.EventHandler(this.btn_Bcc_Click);
            // 
            // btn_Cc
            // 
            this.btn_Cc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cc.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Cc.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_Cc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cc.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cc.Image")));
            this.btn_Cc.Location = new System.Drawing.Point(897, 52);
            this.btn_Cc.Name = "btn_Cc";
            this.btn_Cc.Size = new System.Drawing.Size(34, 23);
            this.btn_Cc.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btn_Cc, "Click to select emailid(s)");
            this.btn_Cc.UseVisualStyleBackColor = false;
            this.btn_Cc.Click += new System.EventHandler(this.btn_Cc_Click);
            // 
            // btn_To
            // 
            this.btn_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_To.BackColor = System.Drawing.SystemColors.Control;
            this.btn_To.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_To.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_To.Image = ((System.Drawing.Image)(resources.GetObject("btn_To.Image")));
            this.btn_To.Location = new System.Drawing.Point(897, 26);
            this.btn_To.Name = "btn_To";
            this.btn_To.Size = new System.Drawing.Size(34, 23);
            this.btn_To.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btn_To, "Click to select emailid(s)");
            this.btn_To.UseVisualStyleBackColor = false;
            this.btn_To.Click += new System.EventHandler(this.btn_To_Click);
            // 
            // txt_body
            // 
            this.txt_body.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_body.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_body.Location = new System.Drawing.Point(15, 132);
            this.txt_body.Multiline = true;
            this.txt_body.Name = "txt_body";
            this.txt_body.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_body.Size = new System.Drawing.Size(916, 145);
            this.txt_body.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txt_body, "Enter body part of an email");
            // 
            // txt_subject
            // 
            this.txt_subject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_subject.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_subject.Location = new System.Drawing.Point(64, 105);
            this.txt_subject.Multiline = true;
            this.txt_subject.Name = "txt_subject";
            this.txt_subject.Size = new System.Drawing.Size(827, 20);
            this.txt_subject.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txt_subject, "Enter subject of an email");
            // 
            // lbl_subject
            // 
            this.lbl_subject.AutoSize = true;
            this.lbl_subject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_subject.Location = new System.Drawing.Point(12, 109);
            this.lbl_subject.Name = "lbl_subject";
            this.lbl_subject.Size = new System.Drawing.Size(43, 13);
            this.lbl_subject.TabIndex = 9;
            this.lbl_subject.Text = "&Subject";
            // 
            // txt_bcc
            // 
            this.txt_bcc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_bcc.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_bcc.Location = new System.Drawing.Point(64, 78);
            this.txt_bcc.Multiline = true;
            this.txt_bcc.Name = "txt_bcc";
            this.txt_bcc.Size = new System.Drawing.Size(827, 20);
            this.txt_bcc.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txt_bcc, "Enter emailid(s)");
            this.txt_bcc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_bcc_KeyDown);
            // 
            // txt_cc
            // 
            this.txt_cc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_cc.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_cc.Location = new System.Drawing.Point(64, 53);
            this.txt_cc.Multiline = true;
            this.txt_cc.Name = "txt_cc";
            this.txt_cc.Size = new System.Drawing.Size(827, 20);
            this.txt_cc.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txt_cc, "Enter emailid(s)");
            this.txt_cc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_cc_KeyDown);
            // 
            // txt_to
            // 
            this.txt_to.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_to.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_to.Location = new System.Drawing.Point(64, 27);
            this.txt_to.Multiline = true;
            this.txt_to.Name = "txt_to";
            this.txt_to.Size = new System.Drawing.Size(827, 20);
            this.txt_to.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txt_to, "Enter emailid(s)");
            this.txt_to.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_to_KeyDown);
            // 
            // lbl_bcc
            // 
            this.lbl_bcc.AutoSize = true;
            this.lbl_bcc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_bcc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_bcc.Location = new System.Drawing.Point(15, 81);
            this.lbl_bcc.Name = "lbl_bcc";
            this.lbl_bcc.Size = new System.Drawing.Size(37, 15);
            this.lbl_bcc.TabIndex = 6;
            this.lbl_bcc.Text = "&Bcc...";
            // 
            // lbl_cc
            // 
            this.lbl_cc.AutoSize = true;
            this.lbl_cc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_cc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cc.Location = new System.Drawing.Point(15, 56);
            this.lbl_cc.Name = "lbl_cc";
            this.lbl_cc.Size = new System.Drawing.Size(31, 15);
            this.lbl_cc.TabIndex = 3;
            this.lbl_cc.Text = "&Cc...";
            // 
            // lbl_to
            // 
            this.lbl_to.AutoSize = true;
            this.lbl_to.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_to.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_to.Location = new System.Drawing.Point(15, 30);
            this.lbl_to.Name = "lbl_to";
            this.lbl_to.Size = new System.Drawing.Size(31, 15);
            this.lbl_to.TabIndex = 0;
            this.lbl_to.Text = "&To...";
            // 
            // lblSelReport
            // 
            this.lblSelReport.AutoSize = true;
            this.lblSelReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelReport.Location = new System.Drawing.Point(456, 22);
            this.lblSelReport.Name = "lblSelReport";
            this.lblSelReport.Size = new System.Drawing.Size(75, 13);
            this.lblSelReport.TabIndex = 4;
            this.lblSelReport.Text = "Select &Report ";
            // 
            // lbl_tran_typ
            // 
            this.lbl_tran_typ.AutoSize = true;
            this.lbl_tran_typ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tran_typ.Location = new System.Drawing.Point(636, 22);
            this.lbl_tran_typ.Name = "lbl_tran_typ";
            this.lbl_tran_typ.Size = new System.Drawing.Size(90, 13);
            this.lbl_tran_typ.TabIndex = 6;
            this.lbl_tran_typ.Text = "T&ransaction Type";
            this.lbl_tran_typ.Visible = false;
            // 
            // txt_desc
            // 
            this.txt_desc.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_desc.Location = new System.Drawing.Point(100, 41);
            this.txt_desc.Multiline = true;
            this.txt_desc.Name = "txt_desc";
            this.txt_desc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_desc.Size = new System.Drawing.Size(341, 88);
            this.txt_desc.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txt_desc, "Enter email description");
            // 
            // lbl_desc
            // 
            this.lbl_desc.AutoSize = true;
            this.lbl_desc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc.Location = new System.Drawing.Point(13, 79);
            this.lbl_desc.Name = "lbl_desc";
            this.lbl_desc.Size = new System.Drawing.Size(63, 13);
            this.lbl_desc.TabIndex = 2;
            this.lbl_desc.Text = "&Description ";
            // 
            // txt_Id
            // 
            this.txt_Id.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Id.Location = new System.Drawing.Point(100, 15);
            this.txt_Id.Name = "txt_Id";
            this.txt_Id.Size = new System.Drawing.Size(134, 20);
            this.txt_Id.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txt_Id, "ID");
            // 
            // lbl_Id
            // 
            this.lbl_Id.AutoSize = true;
            this.lbl_Id.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Id.Location = new System.Drawing.Point(13, 19);
            this.lbl_Id.Name = "lbl_Id";
            this.lbl_Id.Size = new System.Drawing.Size(18, 13);
            this.lbl_Id.TabIndex = 0;
            this.lbl_Id.Text = "&ID";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.DataMember = "";
            // 
            // frmEmailClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 590);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tlsMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "frmEmailClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Email Wizard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmailClient_FormClosed);
            this.Load += new System.EventHandler(this.frmEmailClient_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmEmailClient_KeyDown);
            this.tlsMain.ResumeLayout(false);
            this.tlsMain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tlsMain;
        private System.Windows.Forms.ToolStripButton tlsbtnAdd;
        private System.Windows.Forms.ToolStripButton tlsbtnEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_subject;
        private System.Windows.Forms.Label lbl_subject;
        private System.Windows.Forms.TextBox txt_bcc;
        private System.Windows.Forms.TextBox txt_cc;
        private System.Windows.Forms.TextBox txt_to;
        private System.Windows.Forms.Label lbl_bcc;
        private System.Windows.Forms.Label lbl_cc;
        private System.Windows.Forms.Label lbl_to;
        private System.Windows.Forms.Label lbl_tran_typ;
        private System.Windows.Forms.TextBox txt_desc;
        private System.Windows.Forms.Label lbl_desc;
        private System.Windows.Forms.TextBox txt_Id;
        private System.Windows.Forms.Label lbl_Id;
        private System.Windows.Forms.TextBox txt_body;
        private System.Windows.Forms.ToolStripButton tlsbtnDelete;
        private System.Windows.Forms.ListView chklst_tran_typ;
        private System.Windows.Forms.ColumnHeader colhdr_trantyp;
        private System.Windows.Forms.ColumnHeader colhdr_name;
        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton tlsbtnSave;
        private System.Windows.Forms.ToolStripButton tlsbtnCancel;
        private System.Windows.Forms.ToolStripButton tlsbtnExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolStripButton tlsbtnSearch;
        private System.Windows.Forms.Button btn_Bcc;
        private System.Windows.Forms.Button btn_Cc;
        private System.Windows.Forms.Button btn_To;
        private System.Windows.Forms.Button btn_Zoom;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnQueryWin;
        private System.Windows.Forms.FolderBrowserDialog fbd1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnExportPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.Button btn_rep_nm;
        private System.Windows.Forms.Label lbl_rep_nm;
        private System.Windows.Forms.ComboBox cmb_attachment_typ;
        private System.Windows.Forms.TextBox txt_rep_nm;
        private System.Windows.Forms.CheckBox chk_hasattachment;
        private System.Windows.Forms.Label lbl_attachment_typ;
        private System.Windows.Forms.Label lbl_filenameprefix;
        private System.Windows.Forms.TextBox txt_filenameprefix;
        private System.Windows.Forms.CheckBox chkremovefiles;
        private System.Windows.Forms.CheckBox chkemaillogfiles;
        private System.Windows.Forms.Label lbllogemailid;
        private System.Windows.Forms.TextBox txtlogemailid;
        private System.Windows.Forms.ToolStripButton tlsbtnSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton tlsbtnPendingMails;
        private System.Windows.Forms.Button btnSelReport;
        private System.Windows.Forms.Label lblSelReport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRepDesc;
        private System.Windows.Forms.CheckBox chkDigSig;
    }
}