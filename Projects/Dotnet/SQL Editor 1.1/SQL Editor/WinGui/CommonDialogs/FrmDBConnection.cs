
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using SQLEditor.Database;
using SQLEditor.WinGui.UserControls;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmDBConnection.
	/// </summary>
	public class FrmDBConnection : FrmBaseDialog
	{
        //private UcDbConnectionMicrosoftOleDbClient _ucDbConnectionMicrosoftOleDbClient=null;//new UcDbConnectionMicrosoftOleDbClient();
        //private UcDbConnectionOracleClient _ucDbConnectionOracleClient=null;// = new UcDbConnectionMicrosoftOracleClient();
		private UcDbConnectionMicrosoftSqlClient _ucDbConnectionMicrosoftSqlClient=null;// = new UcDbConnectionMicrosoftSqlClient();
        //private UcDbConnectionMySQLClient _ucDbConnectionMySQLClient=null;
        //private UcDbConnectionSybaseASEClient _ucDbConnectionSybaseASEClient=null;
        //private UcDbConnectionFirebirdClient _ucDbConnectionFirebirdClient=null;
        //private UcDbConnectionDB2Client _ucDbConnectionDB2Client=null;

		public DataSource dataSource = new DataSource();
		private XmlDocument xmlDBConnections = new XmlDocument();
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox dbConnectionType;
        private System.Windows.Forms.Panel gbParameters;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmDBConnection()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_ucDbConnectionMicrosoftSqlClient = new UcDbConnectionMicrosoftSqlClient();
			this.gbParameters.Controls.Add(_ucDbConnectionMicrosoftSqlClient);
			ActivateOptionControl(_ucDbConnectionMicrosoftSqlClient);
            dbConnectionType.Text = "Microsoft SqlClient";
		}
		public FrmDBConnection(DataSource ds)
		{
			InitializeComponent();
			//this.dataSource=dataSource;
			dbConnectionType.Enabled=false;
			switch(ds.ConnectionType)
			{
				case DBConnectionType.MicrosoftSqlClient:
					_ucDbConnectionMicrosoftSqlClient = new UcDbConnectionMicrosoftSqlClient(ds);
					this.gbParameters.Controls.Add(_ucDbConnectionMicrosoftSqlClient);
					ActivateOptionControl(_ucDbConnectionMicrosoftSqlClient);
					dbConnectionType.Text="Microsoft SqlClient";
					break;
                ////case DBConnectionType.MicrosoftOleDb:
                ////    _ucDbConnectionMicrosoftOleDbClient = new UcDbConnectionMicrosoftOleDbClient(ds);
                ////    this.gbParameters.Controls.Add(_ucDbConnectionMicrosoftOleDbClient);
                ////    ActivateOptionControl(_ucDbConnectionMicrosoftOleDbClient);
                ////    dbConnectionType.Text="Microsoft OleDb";
                ////    break;
                ////case DBConnectionType.OracleOleDb:
                ////    _ucDbConnectionOracleClient = new UcDbConnectionOracleClient(ds);
                ////    this.gbParameters.Controls.Add(_ucDbConnectionOracleClient);
                ////    ActivateOptionControl(_ucDbConnectionOracleClient);
                ////    dbConnectionType.Text="Oracle OleDb";
                ////    break;
                ////case DBConnectionType.MySQL:
                ////    _ucDbConnectionMySQLClient = new UcDbConnectionMySQLClient(ds);
                ////    this.gbParameters.Controls.Add(_ucDbConnectionMySQLClient);
                ////    ActivateOptionControl(_ucDbConnectionMySQLClient);
                ////    dbConnectionType.Text="MySQL Client";
                ////    break;
                ////case DBConnectionType.SybaseASE:
                ////    _ucDbConnectionSybaseASEClient = new UcDbConnectionSybaseASEClient(ds);
                ////    this.gbParameters.Controls.Add(_ucDbConnectionSybaseASEClient);
                ////    ActivateOptionControl(_ucDbConnectionSybaseASEClient);
                ////    dbConnectionType.Text="Sybase ASE Client";
                ////    break;
                ////case DBConnectionType.Firebird:
                ////    _ucDbConnectionFirebirdClient = new UcDbConnectionFirebirdClient(ds);
                ////    this.gbParameters.Controls.Add(_ucDbConnectionFirebirdClient);
                ////    ActivateOptionControl(_ucDbConnectionFirebirdClient);
                ////    dbConnectionType.Text="Firebird";
                ////    break;
                ////case DBConnectionType.DB2:
                ////    _ucDbConnectionDB2Client=new UcDbConnectionDB2Client(ds);
                ////    this.gbParameters.Controls.Add(_ucDbConnectionDB2Client);
                ////    ActivateOptionControl(_ucDbConnectionDB2Client);
                ////    dbConnectionType.Text="IBM DB2";
                ////    break;
			}

		
			


		}
//		public FrmDBConnection(string dsn)
//		{
//			//
//			// Required for Windows Form Designer support
//			//
//			InitializeComponent();
//
//			
//
//			xmlDBConnections.Load(Application.StartupPath +  @"\DBConnections.xml");
//			XmlNodeList xmlNodeList = xmlDBConnections.GetElementsByTagName("CONNECTIONS");
//			foreach(XmlNode node in xmlNodeList[0].ChildNodes)
//			{
//				if(node.Attributes.GetNamedItem("name").Value == dsn)
//				{
//					txrDS.Enabled = false;
//					txrDS.Text = node.Attributes.GetNamedItem("name").Value;
//					string s = node.Attributes.GetNamedItem("string").Value;
//					int start,length;
//
//					start = s.IndexOf("Provider=")+9;
//					length = s.IndexOf(";") - start;
//					txtProvider.Text = s.Substring(start,length);
//					
//					start = s.IndexOf("User ID=")+8;
//					if(start>8)
//					{
//						length = s.IndexOf(";",start) - start;
//						if(length>0)
//							txtUserName.Text = s.Substring(start,length);
//					}
//
//					start = s.IndexOf("Password=")+9;
//					if(start>9)
//					{
//						length = s.IndexOf(";",start) - start;
//						if(length>0)
//							txtPassword.Text = s.Substring(start,length);
//					}
//
//					start = s.IndexOf("Catalog=")+8;
//					if(start>8)
//					{
//						length = s.IndexOf(";",start) - start;
//						if(length>0)
//							txtDefultDB.Text = s.Substring(start,length);
//					}
//
//					if(txtUserName.Text.Length==0)
//						chbUIS.Checked = true;
//					else
//						chbUIS.Checked = false;
//
//					continue;
//				}
//			}
//			txrDS.Focus();
//		}

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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dbConnectionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbParameters = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(222, 203);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "Ok";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(308, 203);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 24);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            // 
            // dbConnectionType
            // 
            this.dbConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dbConnectionType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dbConnectionType.Items.AddRange(new object[] {
            "Microsoft SqlClient"});
            this.dbConnectionType.Location = new System.Drawing.Point(96, 12);
            this.dbConnectionType.Name = "dbConnectionType";
            this.dbConnectionType.Size = new System.Drawing.Size(300, 21);
            this.dbConnectionType.TabIndex = 21;
            this.dbConnectionType.Visible = false;
            this.dbConnectionType.SelectedIndexChanged += new System.EventHandler(this.dbConnectionType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Connection type:";
            this.label1.Visible = false;
            // 
            // gbParameters
            // 
            this.gbParameters.Location = new System.Drawing.Point(4, 3);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(384, 194);
            this.gbParameters.TabIndex = 22;
            // 
            // FrmDBConnection
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(390, 231);
            this.ControlBox = false;
            this.Controls.Add(this.gbParameters);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dbConnectionType);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FrmDBConnection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database connection";
            this.Load += new System.EventHandler(this.FrmDBConnection_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			switch(dbConnectionType.Text)
			{
				case "Microsoft SqlClient":
					if( _ucDbConnectionMicrosoftSqlClient.txtDefultDB.Text.Length==0)
					{
						MessageBox.Show( SQLEditor.Localization.GetString("FrmConnection.DatabaseIsNull"),
							"Microsoft SqlClient",MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}
                    dataSource=_ucDbConnectionMicrosoftSqlClient.GetDataSource();
					break;
                ////case "Microsoft OleDb":
                ////    if( _ucDbConnectionMicrosoftOleDbClient.txtDefultDB.Text.Length==0)
                ////    {
                ////        MessageBox.Show( SQLEditor.Localization.GetString("FrmConnection.DatabaseIsNull"),
                ////            "Microsoft SqlClient",MessageBoxButtons.OK, MessageBoxIcon.Information);
                ////        return;
                ////    }
                ////    dataSource=_ucDbConnectionMicrosoftOleDbClient.GetDataSource();
                ////    break;
                ////case "Oracle OleDb":
                ////    dataSource=_ucDbConnectionOracleClient.GetDataSource();
                ////    break;
                ////case "MySQL Client":
                ////    dataSource=_ucDbConnectionMySQLClient.GetDataSource();
                ////    break;
                ////case "Sybase ASE Client":
                ////    dataSource=_ucDbConnectionSybaseASEClient.GetDataSource();
                ////    break;
                ////case "Firebird":
                ////    dataSource=_ucDbConnectionFirebirdClient.GetDataSource();
                ////    break;
                ////case "IBM DB2":
                ////    dataSource=_ucDbConnectionDB2Client.GetDataSource();
                ////    break;
				default:
					MessageBox.Show(Localization.GetString("FrmDBConnection.button1_Click"),this.Text,MessageBoxButtons.OK,MessageBoxIcon.Hand);
					return;
			}


			this.DialogResult = DialogResult.OK;
			this.Close();
			return;
		}

//		private void chbUIS_CheckedChanged(object sender, System.EventArgs e)
//		{
//			if(chbUIS.Checked)
//			{
//				txtUserName.Enabled = false;
//				txtPassword.Enabled = false;
//			}
//			else
//			{
//				txtUserName.Enabled = true;
//				txtPassword.Enabled = true;
//			}
//		}

		private void txrDS_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		
		}

		

		private void txtTimeOut_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			string val = e.KeyChar.ToString();
			if(val=="\b")
			{
				e.Handled=false;
				return;
			}
			char c = val.ToCharArray()[0];

			if(!char.IsNumber(c))
				e.Handled=true;
		}

		private void FrmDBConnection_Load(object sender, System.EventArgs e)
		{
			
		}


		private void ActivateOptionControl(System.Windows.Forms.UserControl optionControl)
		{
			foreach(UserControl uc in this.gbParameters.Controls)
			{
				if(uc != optionControl)
					uc.Hide();
			}
			optionControl.Show();

		}

		private void dbConnectionType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(dbConnectionType.Text)
			{
				case "Microsoft SqlClient":
					if(_ucDbConnectionMicrosoftSqlClient==null)
						_ucDbConnectionMicrosoftSqlClient = new UcDbConnectionMicrosoftSqlClient();
					this.gbParameters.Controls.Add(_ucDbConnectionMicrosoftSqlClient);
					ActivateOptionControl(_ucDbConnectionMicrosoftSqlClient);
					break;
                ////case "Microsoft OleDb":
                ////    if(_ucDbConnectionMicrosoftOleDbClient==null)
                ////        _ucDbConnectionMicrosoftOleDbClient = new UcDbConnectionMicrosoftOleDbClient();
                ////    this.gbParameters.Controls.Add(_ucDbConnectionMicrosoftOleDbClient);
                ////    ActivateOptionControl(_ucDbConnectionMicrosoftOleDbClient);
                ////    break;
                ////////case "Oracle OleDb":
                ////    if(_ucDbConnectionOracleClient==null)
                ////        _ucDbConnectionOracleClient = new UcDbConnectionOracleClient();
                ////    this.gbParameters.Controls.Add(_ucDbConnectionOracleClient);
                ////    ActivateOptionControl(_ucDbConnectionOracleClient);
                ////    break;
                ////case "MySQL Client":
                ////    if(_ucDbConnectionMySQLClient==null)
                ////        _ucDbConnectionMySQLClient = new UcDbConnectionMySQLClient();
                ////    this.gbParameters.Controls.Add(_ucDbConnectionMySQLClient);
                ////    ActivateOptionControl(_ucDbConnectionMySQLClient);
                ////    break;
                ////case "Sybase ASE Client":
                ////    if(_ucDbConnectionSybaseASEClient==null)
                ////        _ucDbConnectionSybaseASEClient = new UcDbConnectionSybaseASEClient();
                ////    this.gbParameters.Controls.Add(_ucDbConnectionSybaseASEClient);
                ////    ActivateOptionControl(_ucDbConnectionSybaseASEClient);
                ////    break;
                ////case "Firebird":
                ////    if(_ucDbConnectionFirebirdClient==null)
                ////        _ucDbConnectionFirebirdClient = new UcDbConnectionFirebirdClient();
                ////    this.gbParameters.Controls.Add(_ucDbConnectionFirebirdClient);
                ////    ActivateOptionControl(_ucDbConnectionFirebirdClient);
                ////    break;
                ////case "IBM DB2":
                ////    if(_ucDbConnectionDB2Client==null)
                ////        _ucDbConnectionDB2Client = new UcDbConnectionDB2Client();
                ////    this.gbParameters.Controls.Add(_ucDbConnectionDB2Client);
                ////    ActivateOptionControl(_ucDbConnectionDB2Client);
                ////    break;
				default:
					MessageBox.Show(Localization.GetString("FrmDBConnection.button1_Click"),this.Text,MessageBoxButtons.OK,MessageBoxIcon.Hand);
					return;
			}
		}

		private void btnHelp_Click(object sender, System.EventArgs e)
		{
		
		}

	}
}
