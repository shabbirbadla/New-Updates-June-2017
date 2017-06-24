
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SQLEditor.Database;

namespace SQLEditor.WinGui.UserControls
{
	/// <summary>
	/// Summary description for UcDbConnectionMicrosoftOleDb.
	/// </summary>
	public class UcDbConnectionMySQLClient : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txrDS;
		private System.Windows.Forms.TextBox txtFriendlyName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox txtDefultDB;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox rbUseIntegratedSecurity;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UcDbConnectionMySQLClient(DataSource ds)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			txrDS.Enabled=false;
			txrDS.Text = ds.Name;
			txtDefultDB.Text = ds.InitialCatalog;
			if(ds.FriendlyName!=null)
				txtFriendlyName.Text = ds.FriendlyName;

			if(ds.IntegratedSecurity.Length==0)
			{
				rbUseIntegratedSecurity.Checked=false;
				txtUserName.Text = ds.UserName;
				txtPassword.Text = ds.Password;
			}
			else
			{
				rbUseIntegratedSecurity.Checked=true;
				txtUserName.Text = "";
				txtPassword.Text = "";
			}
		}
		public UcDbConnectionMySQLClient()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UcDbConnectionMySQLClient));
			this.label8 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txrDS = new System.Windows.Forms.TextBox();
			this.txtFriendlyName = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtDefultDB = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.rbUseIntegratedSecurity = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 24);
			this.label8.TabIndex = 46;
			this.label8.Text = "Friendly name:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(320, 64);
			this.label2.TabIndex = 47;
			this.label2.Text = "The MySQL Data provider is an ADO.NET driver for MySQL. No support for stored pro" +
				"cedures and user defined functions (Does not exists in MySQL 4.x).";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 44;
			this.label3.Text = "Default schema/db:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 48);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 43;
			this.label6.Text = "Server:";
			// 
			// txrDS
			// 
			this.txrDS.Location = new System.Drawing.Point(128, 48);
			this.txrDS.Name = "txrDS";
			this.txrDS.Size = new System.Drawing.Size(232, 20);
			this.txrDS.TabIndex = 1;
			this.txrDS.Text = "";
			// 
			// txtFriendlyName
			// 
			this.txtFriendlyName.Location = new System.Drawing.Point(128, 24);
			this.txtFriendlyName.Name = "txtFriendlyName";
			this.txtFriendlyName.Size = new System.Drawing.Size(232, 20);
			this.txtFriendlyName.TabIndex = 0;
			this.txtFriendlyName.Text = "MySQL";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtUserName);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(24, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(336, 88);
			this.groupBox1.TabIndex = 50;
			this.groupBox1.TabStop = false;
			// 
			// txtUserName
			// 
			this.txtUserName.Location = new System.Drawing.Point(128, 24);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(184, 20);
			this.txtUserName.TabIndex = 5;
			this.txtUserName.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 28;
			this.label5.Text = "Password:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 25;
			this.label4.Text = "User name:";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(128, 48);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(184, 20);
			this.txtPassword.TabIndex = 6;
			this.txtPassword.Text = "";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(16, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(16, 16);
			this.pictureBox1.TabIndex = 48;
			this.pictureBox1.TabStop = false;
			// 
			// txtDefultDB
			// 
			this.txtDefultDB.Location = new System.Drawing.Point(128, 72);
			this.txtDefultDB.Name = "txtDefultDB";
			this.txtDefultDB.Size = new System.Drawing.Size(232, 20);
			this.txtDefultDB.TabIndex = 2;
			this.txtDefultDB.Text = "MySQL";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.rbUseIntegratedSecurity);
			this.groupBox2.Controls.Add(this.groupBox1);
			this.groupBox2.Controls.Add(this.txtFriendlyName);
			this.groupBox2.Controls.Add(this.txrDS);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.txtDefultDB);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(8, 72);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(376, 264);
			this.groupBox2.TabIndex = 51;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Parameters:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(160, 142);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 16);
			this.label1.TabIndex = 51;
			this.label1.Text = "(Not yet supported)";
			// 
			// rbUseIntegratedSecurity
			// 
			this.rbUseIntegratedSecurity.Enabled = false;
			this.rbUseIntegratedSecurity.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbUseIntegratedSecurity.Location = new System.Drawing.Point(24, 136);
			this.rbUseIntegratedSecurity.Name = "rbUseIntegratedSecurity";
			this.rbUseIntegratedSecurity.Size = new System.Drawing.Size(136, 24);
			this.rbUseIntegratedSecurity.TabIndex = 4;
			this.rbUseIntegratedSecurity.Text = "Use Integrated Security";
			this.rbUseIntegratedSecurity.CheckedChanged += new System.EventHandler(this.rbUseIntegratedSecurity_CheckedChanged);
			// 
			// UcDbConnectionMySQLClient
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pictureBox1);
			this.Name = "UcDbConnectionMySQLClient";
			this.Size = new System.Drawing.Size(576, 760);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public DataSource GetDataSource()
		{
			DataSource dataSource = new DataSource();
			dataSource.ID = Guid.NewGuid();
			dataSource.InitialCatalog = txtDefultDB.Text;
			if(rbUseIntegratedSecurity.Checked)
				dataSource.IntegratedSecurity = "SSPI";
			else
				dataSource.IntegratedSecurity = "";
			dataSource.IsConnected = true;
			dataSource.Name = txrDS.Text;
			dataSource.PersistSecurityInfo = "TRUE";
			dataSource.UserName = txtUserName.Text;
			dataSource.Password = txtPassword.Text;
			dataSource.FriendlyName = txtFriendlyName.Text;

			dataSource.ConnectionType=DBConnectionType.MySQL;

			return dataSource;
		}

		private void rbUseIntegratedSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbUseIntegratedSecurity.Checked)
			{
				txtUserName.Enabled = false;
				txtPassword.Enabled = false;
			}
			else
			{
				txtUserName.Enabled = true;
				txtPassword.Enabled = true;
			}
		}

		private void linkDownloadDriver_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string url = "http://dev.mysql.com/downloads/connector/net/1.0.html";
			System.Diagnostics.Process.Start("IExplore.exe",url);
		}
	}
}
