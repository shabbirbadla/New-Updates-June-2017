// *******************************************************************************
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *******************************************************************************
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace QueryCommander
{
	/// <summary>
	/// Summary description for FrmSettings.
	/// </summary>
	public class FrmSettings : FrmBaseDialog
	{
		private Font font;
		private MainForm _mainform;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnColor;
		private QCSimpleRichEditor lblDefault;
		private QCSimpleRichEditor lblSyntax;
		private System.Windows.Forms.Button btnSyntax;
		private QCSimpleRichEditor lblComment;
		private System.Windows.Forms.Button btnComment;
		private QCSimpleRichEditor lblQuote;
		private System.Windows.Forms.Button btnQuote;
		private QCSimpleRichEditor lblOperand;
		private System.Windows.Forms.Button btnOperand;
		private QCSimpleRichEditor lblFunction;
		private System.Windows.Forms.Button btnFunction;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox chbRunWithIOStat;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDiffPercent;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chbShowStartPage;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.CheckBox chbShowCommentHeader;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmSettings(MainForm mainForm)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_mainform = mainForm;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmSettings));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.chbShowStartPage = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtDiffPercent = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.chbRunWithIOStat = new System.Windows.Forms.CheckBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.lblFunction = new QueryCommander.QCSimpleRichEditor();
			this.btnFunction = new System.Windows.Forms.Button();
			this.lblOperand = new QueryCommander.QCSimpleRichEditor();
			this.btnOperand = new System.Windows.Forms.Button();
			this.lblQuote = new QueryCommander.QCSimpleRichEditor();
			this.btnQuote = new System.Windows.Forms.Button();
			this.lblComment = new QueryCommander.QCSimpleRichEditor();
			this.btnComment = new System.Windows.Forms.Button();
			this.lblSyntax = new QueryCommander.QCSimpleRichEditor();
			this.btnSyntax = new System.Windows.Forms.Button();
			this.lblDefault = new QueryCommander.QCSimpleRichEditor();
			this.btnColor = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.chbShowCommentHeader = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(8, 136);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(584, 296);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.linkLabel1);
			this.tabPage3.Controls.Add(this.chbShowStartPage);
			this.tabPage3.Controls.Add(this.label5);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(576, 270);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Start page";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(16, 152);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(200, 24);
			this.linkLabel1.TabIndex = 3;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://querycommander.rockwolf.com";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// chbShowStartPage
			// 
			this.chbShowStartPage.Checked = true;
			this.chbShowStartPage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbShowStartPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbShowStartPage.Location = new System.Drawing.Point(16, 120);
			this.chbShowStartPage.Name = "chbShowStartPage";
			this.chbShowStartPage.Size = new System.Drawing.Size(264, 24);
			this.chbShowStartPage.TabIndex = 2;
			this.chbShowStartPage.Text = "Show Start page on startup";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(512, 40);
			this.label5.TabIndex = 1;
			this.label5.Text = "The application will not, under any circumstance send any information, exept the " +
				"version number, to the server.";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(536, 48);
			this.label4.TabIndex = 0;
			this.label4.Text = "Enabling this option, QueryCommander will retrieve version data from rockwolf.com" +
				" and compare it to your current version. If there is a new release available you" +
				" will be given detailed information about the update.";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.chbShowCommentHeader);
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Controls.Add(this.checkBox1);
			this.tabPage2.Controls.Add(this.chbRunWithIOStat);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(576, 270);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Query settings";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtDiffPercent);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 104);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(560, 152);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Information message";
			// 
			// txtDiffPercent
			// 
			this.txtDiffPercent.Location = new System.Drawing.Point(144, 56);
			this.txtDiffPercent.Name = "txtDiffPercent";
			this.txtDiffPercent.Size = new System.Drawing.Size(32, 20);
			this.txtDiffPercent.TabIndex = 5;
			this.txtDiffPercent.Text = "101";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Differencial percentage:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(480, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "Enabeling this option will inform the user about ineffective query plans. ";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(520, 40);
			this.label2.TabIndex = 3;
			this.label2.Text = "Setting the differencial percentage will affect when the alarm is given. 101% (de" +
				"fault) means the alarm will be set of when the table scan exceed the rowcount.  " +
				"";
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Enabled = false;
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox1.Location = new System.Drawing.Point(16, 24);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(264, 16);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Use upper case on reserved words recognizion";
			// 
			// chbRunWithIOStat
			// 
			this.chbRunWithIOStat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbRunWithIOStat.Location = new System.Drawing.Point(16, 48);
			this.chbRunWithIOStat.Name = "chbRunWithIOStat";
			this.chbRunWithIOStat.Size = new System.Drawing.Size(176, 16);
			this.chbRunWithIOStat.TabIndex = 1;
			this.chbRunWithIOStat.Text = "Run query with IO statistics";
			this.chbRunWithIOStat.CheckedChanged += new System.EventHandler(this.chbRunWithIOStat_CheckedChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.lblFunction);
			this.tabPage1.Controls.Add(this.btnFunction);
			this.tabPage1.Controls.Add(this.lblOperand);
			this.tabPage1.Controls.Add(this.btnOperand);
			this.tabPage1.Controls.Add(this.lblQuote);
			this.tabPage1.Controls.Add(this.btnQuote);
			this.tabPage1.Controls.Add(this.lblComment);
			this.tabPage1.Controls.Add(this.btnComment);
			this.tabPage1.Controls.Add(this.lblSyntax);
			this.tabPage1.Controls.Add(this.btnSyntax);
			this.tabPage1.Controls.Add(this.lblDefault);
			this.tabPage1.Controls.Add(this.btnColor);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(576, 270);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Editor";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(16, 224);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 24);
			this.button1.TabIndex = 12;
			this.button1.Text = "Font settings";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lblFunction
			// 
			this.lblFunction.BackColor = System.Drawing.Color.White;
			this.lblFunction.ForeColor = System.Drawing.Color.PaleVioletRed;
			this.lblFunction.Location = new System.Drawing.Point(16, 176);
			this.lblFunction.Multiline = false;
			this.lblFunction.Name = "lblFunction";
			this.lblFunction.Size = new System.Drawing.Size(200, 22);
			this.lblFunction.TabIndex = 11;
			this.lblFunction.Text = "Function color";
			// 
			// btnFunction
			// 
			this.btnFunction.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnFunction.Location = new System.Drawing.Point(224, 176);
			this.btnFunction.Name = "btnFunction";
			this.btnFunction.Size = new System.Drawing.Size(24, 16);
			this.btnFunction.TabIndex = 10;
			this.btnFunction.Text = "...";
			this.btnFunction.Click += new System.EventHandler(this.btnFunction_Click);
			// 
			// lblOperand
			// 
			this.lblOperand.BackColor = System.Drawing.Color.White;
			this.lblOperand.ForeColor = System.Drawing.Color.Gray;
			this.lblOperand.Location = new System.Drawing.Point(16, 144);
			this.lblOperand.Multiline = false;
			this.lblOperand.Name = "lblOperand";
			this.lblOperand.Size = new System.Drawing.Size(200, 22);
			this.lblOperand.TabIndex = 9;
			this.lblOperand.Text = "Operand color";
			// 
			// btnOperand
			// 
			this.btnOperand.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOperand.Location = new System.Drawing.Point(224, 144);
			this.btnOperand.Name = "btnOperand";
			this.btnOperand.Size = new System.Drawing.Size(24, 16);
			this.btnOperand.TabIndex = 8;
			this.btnOperand.Text = "...";
			this.btnOperand.Click += new System.EventHandler(this.btnOperand_Click);
			// 
			// lblQuote
			// 
			this.lblQuote.BackColor = System.Drawing.Color.White;
			this.lblQuote.ForeColor = System.Drawing.Color.Red;
			this.lblQuote.Location = new System.Drawing.Point(16, 112);
			this.lblQuote.Multiline = false;
			this.lblQuote.Name = "lblQuote";
			this.lblQuote.Size = new System.Drawing.Size(200, 22);
			this.lblQuote.TabIndex = 7;
			this.lblQuote.Text = "Quote color";
			// 
			// btnQuote
			// 
			this.btnQuote.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnQuote.Location = new System.Drawing.Point(224, 112);
			this.btnQuote.Name = "btnQuote";
			this.btnQuote.Size = new System.Drawing.Size(24, 16);
			this.btnQuote.TabIndex = 6;
			this.btnQuote.Text = "...";
			this.btnQuote.Click += new System.EventHandler(this.btnQuote_Click);
			// 
			// lblComment
			// 
			this.lblComment.BackColor = System.Drawing.Color.White;
			this.lblComment.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.lblComment.Location = new System.Drawing.Point(16, 80);
			this.lblComment.Multiline = false;
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(200, 22);
			this.lblComment.TabIndex = 5;
			this.lblComment.Text = "Comment color";
			// 
			// btnComment
			// 
			this.btnComment.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnComment.Location = new System.Drawing.Point(224, 80);
			this.btnComment.Name = "btnComment";
			this.btnComment.Size = new System.Drawing.Size(24, 16);
			this.btnComment.TabIndex = 4;
			this.btnComment.Text = "...";
			this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
			// 
			// lblSyntax
			// 
			this.lblSyntax.BackColor = System.Drawing.Color.White;
			this.lblSyntax.ForeColor = System.Drawing.Color.Blue;
			this.lblSyntax.Location = new System.Drawing.Point(16, 48);
			this.lblSyntax.Multiline = false;
			this.lblSyntax.Name = "lblSyntax";
			this.lblSyntax.Size = new System.Drawing.Size(200, 22);
			this.lblSyntax.TabIndex = 3;
			this.lblSyntax.Text = "Syntax color";
			// 
			// btnSyntax
			// 
			this.btnSyntax.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSyntax.Location = new System.Drawing.Point(224, 48);
			this.btnSyntax.Name = "btnSyntax";
			this.btnSyntax.Size = new System.Drawing.Size(24, 16);
			this.btnSyntax.TabIndex = 2;
			this.btnSyntax.Text = "...";
			this.btnSyntax.Click += new System.EventHandler(this.btnSyntax_Click);
			// 
			// lblDefault
			// 
			this.lblDefault.BackColor = System.Drawing.Color.White;
			this.lblDefault.ForeColor = System.Drawing.Color.Black;
			this.lblDefault.Location = new System.Drawing.Point(16, 16);
			this.lblDefault.Multiline = false;
			this.lblDefault.Name = "lblDefault";
			this.lblDefault.Size = new System.Drawing.Size(200, 22);
			this.lblDefault.TabIndex = 1;
			this.lblDefault.Text = "Default color";
			// 
			// btnColor
			// 
			this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnColor.Location = new System.Drawing.Point(224, 16);
			this.btnColor.Name = "btnColor";
			this.btnColor.Size = new System.Drawing.Size(24, 16);
			this.btnColor.TabIndex = 0;
			this.btnColor.Text = "...";
			this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
			// 
			// btnOk
			// 
			this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOk.Location = new System.Drawing.Point(416, 440);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 24);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(504, 440);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 24);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(600, 128);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// chbShowCommentHeader
			// 
			this.chbShowCommentHeader.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbShowCommentHeader.Location = new System.Drawing.Point(16, 72);
			this.chbShowCommentHeader.Name = "chbShowCommentHeader";
			this.chbShowCommentHeader.Size = new System.Drawing.Size(280, 16);
			this.chbShowCommentHeader.TabIndex = 3;
			this.chbShowCommentHeader.Text = "Show document header window";
			// 
			// FrmSettings
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(600, 478);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmSettings";
			this.Text = "Settings";
			this.Load += new System.EventHandler(this.FrmSettings_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.AllowFullOpen = true;
			colorDialog.ShowHelp = true;
			colorDialog.Color = lblDefault.ForeColor;
			if(colorDialog.ShowDialog()==DialogResult.OK)
				lblDefault.ForeColor = colorDialog.Color;

		}

		private void btnSyntax_Click(object sender, System.EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.AllowFullOpen = true;
			colorDialog.ShowHelp = true;
			colorDialog.Color = lblSyntax.ForeColor;
			if(colorDialog.ShowDialog()==DialogResult.OK)
				lblSyntax.ForeColor = colorDialog.Color;

		}

		private void btnComment_Click(object sender, System.EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.AllowFullOpen = true;
			colorDialog.ShowHelp = true;
			colorDialog.Color = lblComment.ForeColor;
			if(colorDialog.ShowDialog()==DialogResult.OK)
				lblComment.ForeColor = colorDialog.Color;

		}

		private void btnQuote_Click(object sender, System.EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.AllowFullOpen = true;
			colorDialog.ShowHelp = true;
			colorDialog.Color = lblQuote.ForeColor;
			if(colorDialog.ShowDialog()==DialogResult.OK)
				lblQuote.ForeColor = colorDialog.Color;

		}

		private void btnOperand_Click(object sender, System.EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.AllowFullOpen = true;
			colorDialog.ShowHelp = true;
			colorDialog.Color = lblOperand.ForeColor;
			if(colorDialog.ShowDialog()==DialogResult.OK)
				lblOperand.ForeColor = colorDialog.Color;

		}

		private void btnFunction_Click(object sender, System.EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.AllowFullOpen = true;
			colorDialog.ShowHelp = true;
			colorDialog.Color = lblOperand.ForeColor;
			if(colorDialog.ShowDialog()==DialogResult.OK)
				lblOperand.ForeColor = colorDialog.Color;

		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			FontDialog fontDialog = new FontDialog();
			fontDialog.Font = font;
			fontDialog.ShowColor = false;
			if(fontDialog.ShowDialog()==DialogResult.OK)
			{
				font = fontDialog.Font;
				lblComment.Font = font;
				lblDefault.Font = font;
				lblFunction.Font = font;
				lblOperand.Font = font;
				lblQuote.Font = font;
				lblSyntax.Font = font;
			}

		}

		private void FrmSettings_Load(object sender, System.EventArgs e)
		{
			lblDefault.ForeColor		= _mainform.ActiveQueryForm.syntaxReader.DefaultColor;
			lblComment.ForeColor		= _mainform.ActiveQueryForm.syntaxReader.CommentColor;
			lblSyntax.ForeColor			= _mainform.ActiveQueryForm.syntaxReader.KeyWordColor;
			lblQuote.ForeColor			= _mainform.ActiveQueryForm.syntaxReader.StringColor;
			lblOperand.ForeColor		= _mainform.ActiveQueryForm.syntaxReader.OperatorColor;
			lblFunction.ForeColor		= _mainform.ActiveQueryForm.syntaxReader.CompareColor;
			lblDefault.Font				= _mainform.ActiveQueryForm.EditorFont;
			chbRunWithIOStat.Checked	= _mainform.ActiveQueryForm.syntaxReader.RunWithIOStatistics;
			txtDiffPercent.Text			= _mainform.ActiveQueryForm.syntaxReader.DifferencialPercentage.ToString();
			chbShowCommentHeader.Checked =  _mainform.ActiveQueryForm.syntaxReader.ShowFrmDocumentHeader;
			if(_mainform.ActiveQueryForm.syntaxReader.HideStartPage)
				chbShowStartPage.Checked=false;
			else
				chbShowStartPage.Checked=true;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			try
			{
				int differencialPercentage = Convert.ToInt16( txtDiffPercent.Text);

				foreach(FrmQuery frm in _mainform.QueryForms)
				{
					frm.syntaxReader.DefaultColor = lblDefault.ForeColor;
					frm.syntaxReader.CommentColor = lblComment.ForeColor;
					frm.syntaxReader.KeyWordColor = lblSyntax.ForeColor;
					frm.syntaxReader.StringColor = lblQuote.ForeColor;
					frm.syntaxReader.OperatorColor = lblOperand.ForeColor;
					frm.syntaxReader.CompareColor = lblFunction.ForeColor;
					
					frm.syntaxReader.color_default	= lblDefault.GetOleObjectForeColor();
					frm.syntaxReader.color_comment	= lblComment.GetOleObjectForeColor();
					frm.syntaxReader.color_keyword	= lblSyntax.GetOleObjectForeColor();
					frm.syntaxReader.color_string	= lblQuote.GetOleObjectForeColor();
					frm.syntaxReader.color_operator	= lblOperand.GetOleObjectForeColor();
					frm.syntaxReader.color_compare	= lblFunction.GetOleObjectForeColor();

					if(chbShowStartPage.Checked)
						frm.syntaxReader.HideStartPage=false;
					else
						frm.syntaxReader.HideStartPage=true;

					frm.EditorFont = lblDefault.Font;
					frm.syntaxReader.RunWithIOStatistics=chbRunWithIOStat.Checked;
					frm.syntaxReader.DifferencialPercentage=differencialPercentage;
					frm.syntaxReader.ShowFrmDocumentHeader = chbShowCommentHeader.Checked;
					frm.syntaxReader.Save(lblDefault.Font);
					
					this.Close();
				}
				MessageBox.Show("You must close current query window tp apply font settings.","Font settings",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Could not save settings.\n" + ex.Message,"Saveing settings",MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
		}

		private void chbRunWithIOStat_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chbRunWithIOStat.Checked)
				groupBox1.Enabled=true;
			else
				groupBox1.Enabled=false;
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string target = "http://querycommander.rockwolf.com";
			System.Diagnostics.Process.Start(target);
		}
	}
}
