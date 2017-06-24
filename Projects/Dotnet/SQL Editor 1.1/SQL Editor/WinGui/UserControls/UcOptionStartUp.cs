
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace QueryCommander.WinGui.UserControls
{
	/// <summary>
	/// Summary description for UcOptionStartUp.
	/// </summary>
	public class UcOptionStartUp : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.LinkLabel linkLabel1;
		public System.Windows.Forms.CheckBox chbShowStartPage;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.CheckBox chbShowSplash;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UcOptionStartUp()
		{
			// This call is required by the Windows.Forms Form Designer.
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
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.chbShowStartPage = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.chbShowSplash = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(8, 144);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(200, 24);
			this.linkLabel1.TabIndex = 11;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://querycommander.rockwolf.com";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// chbShowStartPage
			// 
			this.chbShowStartPage.Checked = true;
			this.chbShowStartPage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbShowStartPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbShowStartPage.Location = new System.Drawing.Point(8, 112);
			this.chbShowStartPage.Name = "chbShowStartPage";
			this.chbShowStartPage.Size = new System.Drawing.Size(264, 24);
			this.chbShowStartPage.TabIndex = 10;
			this.chbShowStartPage.Text = "Show Start page on startup";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(328, 40);
			this.label5.TabIndex = 9;
			this.label5.Text = "The application will not, under any circumstance send any information, except the" +
				" version number, to the server.";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(336, 48);
			this.label4.TabIndex = 8;
			this.label4.Text = "Enabling this option, QueryCommander will retrieve version data from rockwolf.com" +
				" and compare it to your current version. If there is a new release available you" +
				" will be given detailed information about the update.";
			// 
			// chbShowSplash
			// 
			this.chbShowSplash.Checked = true;
			this.chbShowSplash.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbShowSplash.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbShowSplash.Location = new System.Drawing.Point(8, 192);
			this.chbShowSplash.Name = "chbShowSplash";
			this.chbShowSplash.Size = new System.Drawing.Size(232, 16);
			this.chbShowSplash.TabIndex = 12;
			this.chbShowSplash.Text = "Show splash window on startup";
			// 
			// UcOptionStartUp
			// 
			this.Controls.Add(this.chbShowSplash);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.chbShowStartPage);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Name = "UcOptionStartUp";
			this.Size = new System.Drawing.Size(368, 248);
			this.Load += new System.EventHandler(this.UcOptionStartUp_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string target = "http://querycommander.rockwolf.com";
			System.Diagnostics.Process.Start(target);
		}

		private void UcOptionStartUp_Load(object sender, System.EventArgs e)
		{
			QueryCommander.Config.Settings settings = QueryCommander.Config.Settings.Load();
			if(settings.Exists())
			{
				chbShowStartPage.Checked=settings.ShowStartPage;
				chbShowSplash.Checked=settings.ShowSplash;
			}
			else
			{
				chbShowStartPage.Checked=true;
				chbShowSplash.Checked=true;
			}
		}
	}
}
