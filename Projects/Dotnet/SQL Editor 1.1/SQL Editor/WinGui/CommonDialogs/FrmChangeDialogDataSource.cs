using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmChangeDialogDataSource.
	/// </summary>
	public class FrmChangeDialogDataSource :FrmBaseDialog
    {
		private System.Windows.Forms.Label lbl2;
		private System.Windows.Forms.Label lbl1;
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Button btnNo;
		public System.Windows.Forms.CheckBox chbApplyToAll;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmChangeDialogDataSource(string dialogName, string newConnectionName)
		{
			
			InitializeComponent();

			string pattern1="The {0} Query dialog where using the {1} connection which has now been removed.";
			string pattern2="Would you like to change the database connection for {0} to the default database?";
			
			lbl1.Text=String.Format(pattern1,dialogName,newConnectionName);
			lbl2.Text=String.Format(pattern2,dialogName);
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
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.chbApplyToAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbl2
            // 
            this.lbl2.Location = new System.Drawing.Point(12, 57);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(368, 32);
            this.lbl2.TabIndex = 0;
            this.lbl2.Text = "Would you like to change the database connection for XXXXX to the default databas" +
                "e?";
            // 
            // lbl1
            // 
            this.lbl1.Location = new System.Drawing.Point(12, 9);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(384, 32);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "The XXXXX Query dialog where using the XXXX connection which has now been removed" +
                ".";
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(108, 113);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "Yes";
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(188, 113);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "No, close it";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // chbApplyToAll
            // 
            this.chbApplyToAll.Location = new System.Drawing.Point(4, 145);
            this.chbApplyToAll.Name = "chbApplyToAll";
            this.chbApplyToAll.Size = new System.Drawing.Size(256, 24);
            this.chbApplyToAll.TabIndex = 4;
            this.chbApplyToAll.Text = "Apply this settings to all Query dialogs.";
            // 
            // FrmChangeDialogDataSource
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 178);
            this.Controls.Add(this.chbApplyToAll);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lbl2);
            this.Name = "FrmChangeDialogDataSource";
            this.Text = "Change database connection";
            this.ResumeLayout(false);

		}
		#endregion

		private void btnYes_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Yes;
			this.Close();
		}

		private void btnNo_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.No;
			this.Close();
		}
	}
}
