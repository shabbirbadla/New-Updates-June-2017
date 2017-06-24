using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SQLEditor
{
	public class FrmPrint : SQLEditor.FrmBaseDialog
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbSQLStatement;
		private System.Windows.Forms.RadioButton rbOutPut;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.CheckBox cbPreView;
		private FrmQuery _frmQuery;

		public FrmPrint(FrmQuery frmQuery)
		{
			_frmQuery=frmQuery;
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbOutPut = new System.Windows.Forms.RadioButton();
			this.rbSQLStatement = new System.Windows.Forms.RadioButton();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.cbPreView = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbOutPut);
			this.groupBox1.Controls.Add(this.rbSQLStatement);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(304, 80);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// rbOutPut
			// 
			this.rbOutPut.Checked = true;
			this.rbOutPut.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbOutPut.Location = new System.Drawing.Point(16, 24);
			this.rbOutPut.Name = "rbOutPut";
			this.rbOutPut.Size = new System.Drawing.Size(232, 16);
			this.rbOutPut.TabIndex = 1;
			this.rbOutPut.TabStop = true;
			this.rbOutPut.Text = "OutPut (first tab/result)";
			// 
			// rbSQLStatement
			// 
			this.rbSQLStatement.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbSQLStatement.Location = new System.Drawing.Point(16, 48);
			this.rbSQLStatement.Name = "rbSQLStatement";
			this.rbSQLStatement.Size = new System.Drawing.Size(232, 16);
			this.rbSQLStatement.TabIndex = 0;
			this.rbSQLStatement.Text = "SQL Statement";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(248, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOk.Location = new System.Drawing.Point(176, 104);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(64, 24);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// cbPreView
			// 
			this.cbPreView.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbPreView.Location = new System.Drawing.Point(16, 104);
			this.cbPreView.Name = "cbPreView";
			this.cbPreView.Size = new System.Drawing.Size(128, 16);
			this.cbPreView.TabIndex = 3;
			this.cbPreView.Text = "Preview";
			// 
			// FrmPrint
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 142);
			this.Controls.Add(this.cbPreView);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox1);
			this.Name = "FrmPrint";
			this.Text = "Print...";
			this.Load += new System.EventHandler(this.FrmPrint_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if(rbOutPut.Checked)
				_frmQuery.PrintOutPut(cbPreView.Checked);
			else
				_frmQuery.PrintStatement(cbPreView.Checked);

			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void FrmPrint_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}

