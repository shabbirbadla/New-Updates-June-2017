
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
	public class UcDbConnectionMicrosoftSqlClient : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txrDS;
        private System.Windows.Forms.TextBox txtFriendlyName;
		private System.Windows.Forms.TextBox txtTimeOut;
        private System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox txtDefultDB;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox rbUseIntegratedSecurity;
        private GroupBox groupBox1;
        private TextBox txtUserName;
        private Label label5;
        private Label label4;
        private TextBox txtPassword;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UcDbConnectionMicrosoftSqlClient(DataSource ds)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			txrDS.Enabled=false;
			txrDS.Text = ds.Name;
			txtDefultDB.Text = ds.InitialCatalog;
			txtTimeOut.Text = ds.TimeOut;
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
		public UcDbConnectionMicrosoftSqlClient()
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
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txrDS = new System.Windows.Forms.TextBox();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.txtTimeOut = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDefultDB = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbUseIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 46;
            this.label8.Text = "Friendly name:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 44;
            this.label3.Text = "Default database:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 20);
            this.label6.TabIndex = 43;
            this.label6.Text = "Server:";
            // 
            // txrDS
            // 
            this.txrDS.Location = new System.Drawing.Point(120, 48);
            this.txrDS.Name = "txrDS";
            this.txrDS.Size = new System.Drawing.Size(232, 20);
            this.txrDS.TabIndex = 1;
            this.txrDS.TextChanged += new System.EventHandler(this.txrDS_TextChanged);
            // 
            // txtFriendlyName
            // 
            this.txtFriendlyName.Location = new System.Drawing.Point(120, 24);
            this.txtFriendlyName.Name = "txtFriendlyName";
            this.txtFriendlyName.Size = new System.Drawing.Size(232, 20);
            this.txtFriendlyName.TabIndex = 0;
            this.txtFriendlyName.Text = "Company";
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.Location = new System.Drawing.Point(128, 96);
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Size = new System.Drawing.Size(232, 20);
            this.txtTimeOut.TabIndex = 3;
            this.txtTimeOut.Text = "15";
            this.txtTimeOut.Visible = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 24);
            this.label7.TabIndex = 45;
            this.label7.Text = "Time out (default=15):";
            this.label7.Visible = false;
            // 
            // txtDefultDB
            // 
            this.txtDefultDB.Location = new System.Drawing.Point(120, 72);
            this.txtDefultDB.Name = "txtDefultDB";
            this.txtDefultDB.Size = new System.Drawing.Size(232, 20);
            this.txtDefultDB.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbUseIntegratedSecurity);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.txtFriendlyName);
            this.groupBox2.Controls.Add(this.txrDS);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtTimeOut);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtDefultDB);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(376, 183);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters:";
            // 
            // rbUseIntegratedSecurity
            // 
            this.rbUseIntegratedSecurity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbUseIntegratedSecurity.Location = new System.Drawing.Point(19, 110);
            this.rbUseIntegratedSecurity.Name = "rbUseIntegratedSecurity";
            this.rbUseIntegratedSecurity.Size = new System.Drawing.Size(13, 10);
            this.rbUseIntegratedSecurity.TabIndex = 4;
            this.rbUseIntegratedSecurity.Text = "Use Integrated Security";
            this.rbUseIntegratedSecurity.Visible = false;
            this.rbUseIntegratedSecurity.CheckedChanged += new System.EventHandler(this.rbUseIntegratedSecurity_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(114, 47);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(139, 20);
            this.txtPassword.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "User name:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(114, 21);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(139, 20);
            this.txtUserName.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(6, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 79);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            // 
            // UcDbConnectionMicrosoftSqlClient
            // 
            this.Controls.Add(this.groupBox2);
            this.Name = "UcDbConnectionMicrosoftSqlClient";
            this.Size = new System.Drawing.Size(383, 189);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public DataSource GetDataSource()
		{
			DataSource dataSource = new DataSource();
			dataSource.ID = Guid.NewGuid();
			dataSource.InitialCatalog = txtDefultDB.Text;
            if (rbUseIntegratedSecurity.Checked) { dataSource.IntegratedSecurity = "SSPI"; }
            else { dataSource.IntegratedSecurity = ""; }
			dataSource.IsConnected = true;
			dataSource.Name = txrDS.Text;
			dataSource.PersistSecurityInfo = "TRUE";
			dataSource.TimeOut = txtTimeOut.Text;
			dataSource.UserName = txtUserName.Text;
			dataSource.Password = txtPassword.Text;
			dataSource.FriendlyName = txtFriendlyName.Text;
			dataSource.ConnectionType=DBConnectionType.MicrosoftSqlClient;
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

        private void txrDS_TextChanged(object sender, EventArgs e)
        {

        }
	}
}
