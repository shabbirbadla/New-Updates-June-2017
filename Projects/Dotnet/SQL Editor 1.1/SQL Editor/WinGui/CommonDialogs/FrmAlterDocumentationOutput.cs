using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmAlterDocumentationOutput.
	/// </summary>
	public class FrmAlterDocumentationOutput : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.CheckBox chbView;
		public System.Windows.Forms.CheckBox chbSP;
		public System.Windows.Forms.CheckBox chbFn;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox txtLike;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmAlterDocumentationOutput()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmAlterDocumentationOutput));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chbFn = new System.Windows.Forms.CheckBox();
			this.chbSP = new System.Windows.Forms.CheckBox();
			this.chbView = new System.Windows.Forms.CheckBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtLike = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chbFn);
			this.groupBox1.Controls.Add(this.chbSP);
			this.groupBox1.Controls.Add(this.chbView);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 88);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(336, 112);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Object types";
			// 
			// chbFn
			// 
			this.chbFn.Checked = true;
			this.chbFn.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbFn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbFn.Location = new System.Drawing.Point(24, 72);
			this.chbFn.Name = "chbFn";
			this.chbFn.Size = new System.Drawing.Size(168, 24);
			this.chbFn.TabIndex = 2;
			this.chbFn.Text = "User defined functions";
			// 
			// chbSP
			// 
			this.chbSP.Checked = true;
			this.chbSP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbSP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbSP.Location = new System.Drawing.Point(24, 48);
			this.chbSP.Name = "chbSP";
			this.chbSP.Size = new System.Drawing.Size(152, 24);
			this.chbSP.TabIndex = 1;
			this.chbSP.Text = "Stored procedures";
			// 
			// chbView
			// 
			this.chbView.Checked = true;
			this.chbView.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbView.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbView.Location = new System.Drawing.Point(24, 24);
			this.chbView.Name = "chbView";
			this.chbView.TabIndex = 0;
			this.chbView.Text = "Views";
			// 
			// btnOk
			// 
			this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOk.Location = new System.Drawing.Point(176, 280);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(264, 280);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.button2_Click);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.White;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(352, 80);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 40);
			this.label1.TabIndex = 4;
			this.label1.Text = "Choose which  database objects to be included in the documentation file. The outp" +
				"ut will be grouped by type and ordered by name.";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtLike);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(8, 208);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 56);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filter";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Name like:";
			// 
			// txtLike
			// 
			this.txtLike.Location = new System.Drawing.Point(80, 24);
			this.txtLike.Name = "txtLike";
			this.txtLike.Size = new System.Drawing.Size(248, 20);
			this.txtLike.TabIndex = 1;
			this.txtLike.Text = "*";
			// 
			// FrmAlterDocumentationOutput
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 310);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmAlterDocumentationOutput";
			this.Text = "Create Documentation file";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.OK;
			this.Close();
		}
	}
}
