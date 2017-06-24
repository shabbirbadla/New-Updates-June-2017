namespace TaxillaServiceTax3Template
{
    partial class frmST3Interface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmST3Interface));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpReturnFilingDate = new System.Windows.Forms.DateTimePicker();
            this.lblReturnFilingDate = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpAssessee = new System.Windows.Forms.GroupBox();
            this.chkArtiJuriPer = new System.Windows.Forms.CheckBox();
            this.chkLocAuth = new System.Windows.Forms.CheckBox();
            this.chkAssofPer = new System.Windows.Forms.CheckBox();
            this.chkGov = new System.Windows.Forms.CheckBox();
            this.chkHinUnFam = new System.Windows.Forms.CheckBox();
            this.chkFirm = new System.Windows.Forms.CheckBox();
            this.chkSocCoSoc = new System.Windows.Forms.CheckBox();
            this.chkRegTru = new System.Windows.Forms.CheckBox();
            this.chkRPrivLC = new System.Windows.Forms.CheckBox();
            this.chkRPubLC = new System.Windows.Forms.CheckBox();
            this.chkPartnership = new System.Windows.Forms.CheckBox();
            this.chkIndProp = new System.Windows.Forms.CheckBox();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.TextBox();
            this.dtpFromDate = new System.Windows.Forms.TextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cboHalfYearly = new System.Windows.Forms.ComboBox();
            this.lblHalfYearly = new System.Windows.Forms.Label();
            this.rbtnRevised = new System.Windows.Forms.RadioButton();
            this.rbtnOriginal = new System.Windows.Forms.RadioButton();
            this.lblReturnType = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsprogressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.grpAssessee.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dtpReturnFilingDate);
            this.panel1.Controls.Add(this.lblReturnFilingDate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.grpAssessee);
            this.panel1.Controls.Add(this.lbl3);
            this.panel1.Controls.Add(this.lblTo);
            this.panel1.Controls.Add(this.dtpToDate);
            this.panel1.Controls.Add(this.dtpFromDate);
            this.panel1.Controls.Add(this.lblFrom);
            this.panel1.Controls.Add(this.cboHalfYearly);
            this.panel1.Controls.Add(this.lblHalfYearly);
            this.panel1.Controls.Add(this.rbtnRevised);
            this.panel1.Controls.Add(this.rbtnOriginal);
            this.panel1.Controls.Add(this.lblReturnType);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtOutputPath);
            this.panel1.Controls.Add(this.lblOutputPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 430);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(76, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "*";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpReturnFilingDate
            // 
            this.dtpReturnFilingDate.CustomFormat = "dd/MM/yyyy";
            this.dtpReturnFilingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReturnFilingDate.Location = new System.Drawing.Point(107, 87);
            this.dtpReturnFilingDate.Name = "dtpReturnFilingDate";
            this.dtpReturnFilingDate.Size = new System.Drawing.Size(129, 20);
            this.dtpReturnFilingDate.TabIndex = 14;
            // 
            // lblReturnFilingDate
            // 
            this.lblReturnFilingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReturnFilingDate.Location = new System.Drawing.Point(9, 84);
            this.lblReturnFilingDate.Name = "lblReturnFilingDate";
            this.lblReturnFilingDate.Size = new System.Drawing.Size(66, 27);
            this.lblReturnFilingDate.TabIndex = 12;
            this.lblReturnFilingDate.Text = "Return &Filing Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(79, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "*";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(65, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(83, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpAssessee
            // 
            this.grpAssessee.Controls.Add(this.chkArtiJuriPer);
            this.grpAssessee.Controls.Add(this.chkLocAuth);
            this.grpAssessee.Controls.Add(this.chkAssofPer);
            this.grpAssessee.Controls.Add(this.chkGov);
            this.grpAssessee.Controls.Add(this.chkHinUnFam);
            this.grpAssessee.Controls.Add(this.chkFirm);
            this.grpAssessee.Controls.Add(this.chkSocCoSoc);
            this.grpAssessee.Controls.Add(this.chkRegTru);
            this.grpAssessee.Controls.Add(this.chkRPrivLC);
            this.grpAssessee.Controls.Add(this.chkRPubLC);
            this.grpAssessee.Controls.Add(this.chkPartnership);
            this.grpAssessee.Controls.Add(this.chkIndProp);
            this.grpAssessee.Location = new System.Drawing.Point(6, 121);
            this.grpAssessee.Name = "grpAssessee";
            this.grpAssessee.Size = new System.Drawing.Size(446, 212);
            this.grpAssessee.TabIndex = 15;
            this.grpAssessee.TabStop = false;
            this.grpAssessee.Text = "&Constitution of Assessee (Please tick the appropriate category)";
            // 
            // chkArtiJuriPer
            // 
            this.chkArtiJuriPer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkArtiJuriPer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkArtiJuriPer.Location = new System.Drawing.Point(255, 160);
            this.chkArtiJuriPer.Name = "chkArtiJuriPer";
            this.chkArtiJuriPer.Size = new System.Drawing.Size(172, 47);
            this.chkArtiJuriPer.TabIndex = 11;
            this.chkArtiJuriPer.Tag = "Others";
            this.chkArtiJuriPer.Text = "Every Artificial Juridical Person, not falling within any of the preceding sub-cl" +
                "auses";
            this.chkArtiJuriPer.UseVisualStyleBackColor = true;
            this.chkArtiJuriPer.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkLocAuth
            // 
            this.chkLocAuth.AutoSize = true;
            this.chkLocAuth.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkLocAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLocAuth.Location = new System.Drawing.Point(255, 135);
            this.chkLocAuth.Name = "chkLocAuth";
            this.chkLocAuth.Size = new System.Drawing.Size(103, 17);
            this.chkLocAuth.TabIndex = 9;
            this.chkLocAuth.Tag = "Others";
            this.chkLocAuth.Text = "A Local Authority";
            this.chkLocAuth.UseVisualStyleBackColor = true;
            this.chkLocAuth.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkAssofPer
            // 
            this.chkAssofPer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkAssofPer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAssofPer.Location = new System.Drawing.Point(12, 162);
            this.chkAssofPer.Name = "chkAssofPer";
            this.chkAssofPer.Size = new System.Drawing.Size(172, 43);
            this.chkAssofPer.TabIndex = 10;
            this.chkAssofPer.Tag = "Others";
            this.chkAssofPer.Text = "An Association of Persons or Body of Individuals, whether incorporated or not";
            this.chkAssofPer.UseVisualStyleBackColor = true;
            this.chkAssofPer.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkGov
            // 
            this.chkGov.AutoSize = true;
            this.chkGov.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGov.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGov.Location = new System.Drawing.Point(12, 135);
            this.chkGov.Name = "chkGov";
            this.chkGov.Size = new System.Drawing.Size(82, 17);
            this.chkGov.TabIndex = 8;
            this.chkGov.Tag = "Others";
            this.chkGov.Text = "Government";
            this.chkGov.UseVisualStyleBackColor = true;
            this.chkGov.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkHinUnFam
            // 
            this.chkHinUnFam.AutoSize = true;
            this.chkHinUnFam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkHinUnFam.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHinUnFam.Location = new System.Drawing.Point(255, 108);
            this.chkHinUnFam.Name = "chkHinUnFam";
            this.chkHinUnFam.Size = new System.Drawing.Size(133, 17);
            this.chkHinUnFam.TabIndex = 7;
            this.chkHinUnFam.Tag = "Others";
            this.chkHinUnFam.Text = "Hindu Undivided Family";
            this.chkHinUnFam.UseVisualStyleBackColor = true;
            this.chkHinUnFam.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkFirm
            // 
            this.chkFirm.AutoSize = true;
            this.chkFirm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkFirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFirm.Location = new System.Drawing.Point(12, 54);
            this.chkFirm.Name = "chkFirm";
            this.chkFirm.Size = new System.Drawing.Size(53, 17);
            this.chkFirm.TabIndex = 2;
            this.chkFirm.Tag = "Others";
            this.chkFirm.Text = "A Firm";
            this.chkFirm.UseVisualStyleBackColor = true;
            this.chkFirm.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkSocCoSoc
            // 
            this.chkSocCoSoc.AutoSize = true;
            this.chkSocCoSoc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkSocCoSoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSocCoSoc.Location = new System.Drawing.Point(12, 108);
            this.chkSocCoSoc.Name = "chkSocCoSoc";
            this.chkSocCoSoc.Size = new System.Drawing.Size(126, 17);
            this.chkSocCoSoc.TabIndex = 6;
            this.chkSocCoSoc.Tag = "SocCoSoc";
            this.chkSocCoSoc.Text = "Society/Co-op Society";
            this.chkSocCoSoc.UseVisualStyleBackColor = true;
            this.chkSocCoSoc.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkRegTru
            // 
            this.chkRegTru.AutoSize = true;
            this.chkRegTru.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkRegTru.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRegTru.Location = new System.Drawing.Point(255, 81);
            this.chkRegTru.Name = "chkRegTru";
            this.chkRegTru.Size = new System.Drawing.Size(99, 17);
            this.chkRegTru.TabIndex = 5;
            this.chkRegTru.Tag = "RegTru";
            this.chkRegTru.Text = "Registered Trust";
            this.chkRegTru.UseVisualStyleBackColor = true;
            this.chkRegTru.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkRPrivLC
            // 
            this.chkRPrivLC.AutoSize = true;
            this.chkRPrivLC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkRPrivLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRPrivLC.Location = new System.Drawing.Point(12, 81);
            this.chkRPrivLC.Name = "chkRPrivLC";
            this.chkRPrivLC.Size = new System.Drawing.Size(175, 17);
            this.chkRPrivLC.TabIndex = 4;
            this.chkRPrivLC.Tag = "RPrivLC";
            this.chkRPrivLC.Text = "Registered Private Ltd Company";
            this.chkRPrivLC.UseVisualStyleBackColor = true;
            this.chkRPrivLC.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkRPubLC
            // 
            this.chkRPubLC.AutoSize = true;
            this.chkRPubLC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkRPubLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRPubLC.Location = new System.Drawing.Point(255, 54);
            this.chkRPubLC.Name = "chkRPubLC";
            this.chkRPubLC.Size = new System.Drawing.Size(170, 17);
            this.chkRPubLC.TabIndex = 3;
            this.chkRPubLC.Tag = "RPubLC";
            this.chkRPubLC.Text = "Registered Public Ltd Company";
            this.chkRPubLC.UseVisualStyleBackColor = true;
            this.chkRPubLC.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkPartnership
            // 
            this.chkPartnership.AutoSize = true;
            this.chkPartnership.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkPartnership.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPartnership.Location = new System.Drawing.Point(255, 27);
            this.chkPartnership.Name = "chkPartnership";
            this.chkPartnership.Size = new System.Drawing.Size(145, 17);
            this.chkPartnership.TabIndex = 1;
            this.chkPartnership.Tag = "Partnership";
            this.chkPartnership.Text = "Limited Liability Partnerhip";
            this.chkPartnership.UseVisualStyleBackColor = true;
            this.chkPartnership.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // chkIndProp
            // 
            this.chkIndProp.AutoSize = true;
            this.chkIndProp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkIndProp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIndProp.Location = new System.Drawing.Point(12, 27);
            this.chkIndProp.Name = "chkIndProp";
            this.chkIndProp.Size = new System.Drawing.Size(136, 17);
            this.chkIndProp.TabIndex = 0;
            this.chkIndProp.Tag = "IndProp";
            this.chkIndProp.Text = "Proprietorship/Individual";
            this.chkIndProp.UseVisualStyleBackColor = true;
            this.chkIndProp.CheckedChanged += new System.EventHandler(this.chkIndProp_CheckedChanged);
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(439, 49);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(10, 13);
            this.lbl3.TabIndex = 11;
            this.lbl3.Text = ")";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(348, 49);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 9;
            this.lblTo.Text = "To";
            // 
            // dtpToDate
            // 
            this.dtpToDate.BackColor = System.Drawing.Color.White;
            this.dtpToDate.Location = new System.Drawing.Point(369, 45);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.ReadOnly = true;
            this.dtpToDate.Size = new System.Drawing.Size(69, 20);
            this.dtpToDate.TabIndex = 10;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.BackColor = System.Drawing.Color.White;
            this.dtpFromDate.Location = new System.Drawing.Point(279, 45);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ReadOnly = true;
            this.dtpFromDate.Size = new System.Drawing.Size(69, 20);
            this.dtpFromDate.TabIndex = 8;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(241, 49);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(36, 13);
            this.lblFrom.TabIndex = 7;
            this.lblFrom.Text = "( From";
            // 
            // cboHalfYearly
            // 
            this.cboHalfYearly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHalfYearly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboHalfYearly.FormattingEnabled = true;
            this.cboHalfYearly.Items.AddRange(new object[] {
            "April-September",
            "October-March"});
            this.cboHalfYearly.Location = new System.Drawing.Point(107, 45);
            this.cboHalfYearly.Name = "cboHalfYearly";
            this.cboHalfYearly.Size = new System.Drawing.Size(129, 21);
            this.cboHalfYearly.TabIndex = 6;
            this.cboHalfYearly.SelectedIndexChanged += new System.EventHandler(this.cboHalfYearly_SelectedIndexChanged);
            // 
            // lblHalfYearly
            // 
            this.lblHalfYearly.AutoSize = true;
            this.lblHalfYearly.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHalfYearly.Location = new System.Drawing.Point(9, 49);
            this.lblHalfYearly.Name = "lblHalfYearly";
            this.lblHalfYearly.Size = new System.Drawing.Size(57, 13);
            this.lblHalfYearly.TabIndex = 4;
            this.lblHalfYearly.Text = "&Half Yearly";
            // 
            // rbtnRevised
            // 
            this.rbtnRevised.AutoSize = true;
            this.rbtnRevised.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnRevised.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnRevised.Location = new System.Drawing.Point(241, 10);
            this.rbtnRevised.Name = "rbtnRevised";
            this.rbtnRevised.Size = new System.Drawing.Size(63, 17);
            this.rbtnRevised.TabIndex = 3;
            this.rbtnRevised.Text = "Revised";
            this.rbtnRevised.UseVisualStyleBackColor = false;
            // 
            // rbtnOriginal
            // 
            this.rbtnOriginal.AutoSize = true;
            this.rbtnOriginal.Checked = true;
            this.rbtnOriginal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnOriginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnOriginal.Location = new System.Drawing.Point(107, 10);
            this.rbtnOriginal.Name = "rbtnOriginal";
            this.rbtnOriginal.Size = new System.Drawing.Size(59, 17);
            this.rbtnOriginal.TabIndex = 2;
            this.rbtnOriginal.TabStop = true;
            this.rbtnOriginal.Text = "Original";
            this.rbtnOriginal.UseVisualStyleBackColor = true;
            // 
            // lblReturnType
            // 
            this.lblReturnType.AutoSize = true;
            this.lblReturnType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReturnType.Location = new System.Drawing.Point(9, 12);
            this.lblReturnType.Name = "lblReturnType";
            this.lblReturnType.Size = new System.Drawing.Size(75, 13);
            this.lblReturnType.TabIndex = 0;
            this.lblReturnType.Text = "&Type of Return";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLabelStatus,
            this.tsprogressbar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 404);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(456, 22);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLabelStatus
            // 
            this.tsLabelStatus.Name = "tsLabelStatus";
            this.tsLabelStatus.Size = new System.Drawing.Size(441, 17);
            this.tsLabelStatus.Spring = true;
            this.tsLabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsprogressbar
            // 
            this.tsprogressbar.Name = "tsprogressbar";
            this.tsprogressbar.Size = new System.Drawing.Size(100, 16);
            this.tsprogressbar.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(368, 367);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 29);
            this.btnClose.TabIndex = 21;
            this.toolTip1.SetToolTip(this.btnClose, "Click to Close (Alt+C)");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(281, 367);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(81, 29);
            this.btnExport.TabIndex = 20;
            this.btnExport.Text = "&Generate";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolTip1.SetToolTip(this.btnExport, "Click to Generate XML (Alt+G)");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(421, 339);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(28, 24);
            this.btnBrowse.TabIndex = 19;
            this.toolTip1.SetToolTip(this.btnBrowse, "Click to browse");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.BackColor = System.Drawing.Color.White;
            this.txtOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputPath.Location = new System.Drawing.Point(107, 341);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(307, 20);
            this.txtOutputPath.TabIndex = 18;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputPath.Location = new System.Drawing.Point(9, 345);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(71, 13);
            this.lblOutputPath.TabIndex = 16;
            this.lblOutputPath.Text = "Output &Folder";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            // 
            // timer
            // 
            this.timer.Interval = 7000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmST3Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 430);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmST3Interface";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Tax 3";
            this.Load += new System.EventHandler(this.frmST3Interface_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmST3Interface_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmST3Interface_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpAssessee.ResumeLayout(false);
            this.grpAssessee.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsLabelStatus;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblReturnType;
        private System.Windows.Forms.RadioButton rbtnRevised;
        private System.Windows.Forms.RadioButton rbtnOriginal;
        private System.Windows.Forms.ComboBox cboHalfYearly;
        private System.Windows.Forms.Label lblHalfYearly;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.TextBox dtpToDate;
        private System.Windows.Forms.TextBox dtpFromDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ToolStripProgressBar tsprogressbar;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox grpAssessee;
        private System.Windows.Forms.CheckBox chkPartnership;
        private System.Windows.Forms.CheckBox chkIndProp;
        private System.Windows.Forms.CheckBox chkSocCoSoc;
        private System.Windows.Forms.CheckBox chkRegTru;
        private System.Windows.Forms.CheckBox chkRPrivLC;
        private System.Windows.Forms.CheckBox chkRPubLC;
        private System.Windows.Forms.CheckBox chkAssofPer;
        private System.Windows.Forms.CheckBox chkGov;
        private System.Windows.Forms.CheckBox chkHinUnFam;
        private System.Windows.Forms.CheckBox chkFirm;
        private System.Windows.Forms.CheckBox chkArtiJuriPer;
        private System.Windows.Forms.CheckBox chkLocAuth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpReturnFilingDate;
        private System.Windows.Forms.Label lblReturnFilingDate;

    }
}

