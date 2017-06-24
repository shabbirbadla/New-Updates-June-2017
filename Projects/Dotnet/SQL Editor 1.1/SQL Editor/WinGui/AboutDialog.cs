using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace QueryCommander
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class AboutDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label_version;
		private System.Windows.Forms.LinkLabel linkLabel4;
		private System.Windows.Forms.TabPage tabPageThanxs;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			Font = SystemInformation.MenuFont;
			label_version.Text = "Version : " + System.Windows.Forms.Application.ProductVersion;
			
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutDialog));
			this.buttonOK = new System.Windows.Forms.Button();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label_version = new System.Windows.Forms.Label();
			this.linkLabel4 = new System.Windows.Forms.LinkLabel();
			this.tabPageThanxs = new System.Windows.Forms.TabPage();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageThanxs.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(432, 288);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.linkLabel1.Location = new System.Drawing.Point(16, 288);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(112, 24);
			this.linkLabel1.TabIndex = 6;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Support";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.White;
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(522, 80);
			this.splitter1.TabIndex = 7;
			this.splitter1.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(504, 64);
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// label_version
			// 
			this.label_version.BackColor = System.Drawing.Color.White;
			this.label_version.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label_version.ForeColor = System.Drawing.Color.OliveDrab;
			this.label_version.Location = new System.Drawing.Point(288, 40);
			this.label_version.Name = "label_version";
			this.label_version.Size = new System.Drawing.Size(224, 24);
			this.label_version.TabIndex = 10;
			this.label_version.Text = "Version: ";
			// 
			// linkLabel4
			// 
			this.linkLabel4.Location = new System.Drawing.Point(176, 288);
			this.linkLabel4.Name = "linkLabel4";
			this.linkLabel4.Size = new System.Drawing.Size(176, 24);
			this.linkLabel4.TabIndex = 12;
			this.linkLabel4.TabStop = true;
			this.linkLabel4.Text = "QueryCommander Workspace";
			this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
			// 
			// tabPageThanxs
			// 
			this.tabPageThanxs.Controls.Add(this.pictureBox2);
			this.tabPageThanxs.Controls.Add(this.label3);
			this.tabPageThanxs.Controls.Add(this.linkLabel3);
			this.tabPageThanxs.Controls.Add(this.label2);
			this.tabPageThanxs.Controls.Add(this.linkLabel2);
			this.tabPageThanxs.Controls.Add(this.label1);
			this.tabPageThanxs.Location = new System.Drawing.Point(4, 22);
			this.tabPageThanxs.Name = "tabPageThanxs";
			this.tabPageThanxs.Size = new System.Drawing.Size(496, 166);
			this.tabPageThanxs.TabIndex = 1;
			this.tabPageThanxs.Text = "Credits and thankfulness";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(160, 64);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(56, 40);
			this.pictureBox2.TabIndex = 5;
			this.pictureBox2.TabStop = false;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label3.Location = new System.Drawing.Point(16, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Christian Halvarsson";
			// 
			// linkLabel3
			// 
			this.linkLabel3.Location = new System.Drawing.Point(152, 40);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size(320, 16);
			this.linkLabel3.TabIndex = 3;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "Use IRichEditOle from C#";
			this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "John Fisher";
			// 
			// linkLabel2
			// 
			this.linkLabel2.Location = new System.Drawing.Point(152, 16);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(320, 16);
			this.linkLabel2.TabIndex = 1;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "The DockManager control";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Weifen Luo";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageThanxs);
			this.tabControl1.ItemSize = new System.Drawing.Size(71, 18);
			this.tabControl1.Location = new System.Drawing.Point(8, 88);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(504, 192);
			this.tabControl1.TabIndex = 11;
			// 
			// AboutDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(522, 320);
			this.Controls.Add(this.linkLabel4);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label_version);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About QueryCommander";
			this.Load += new System.EventHandler(this.AboutDialog_Load);
			this.tabPageThanxs.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void AboutDialog_Load(object sender, System.EventArgs e)
		{
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string target = "mailto:qcsupport@rockwolf.com?subject=QueryCommander Support";//"http://workspaces.gotdotnet.com/QueryCommander";
			System.Diagnostics.Process.Start(target);
		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string target = "http://www.codeproject.com/cs/miscctrl/DockManager.asp?target=Weifen%7CLuo";
			System.Diagnostics.Process.Start(target);
		}

		private void linkLabel3_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string target = "http://www.codeproject.com/cs/miscctrl/richtextboxplus.asp";
			System.Diagnostics.Process.Start(target);
		}

		private void linkLabel4_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string target = "http://workspaces.gotdotnet.com/QueryCommander";
			System.Diagnostics.Process.Start(target);
		
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
