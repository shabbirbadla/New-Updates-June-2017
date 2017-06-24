using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using QueryCommander.General.TestBench;

namespace QueryCommander.WinGui.CommonDialogs
{
	/// <summary>
	/// Summary description for FrmTestBenchAction.
	/// </summary>
	public class FrmTestBenchAction : FrmBaseDialog
	{
		public ArrayList DBConnections = null;

		public Action _action=null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtDesc;
		private System.Windows.Forms.RadioButton rbScript;
		private System.Windows.Forms.RadioButton rbWait;
		private System.Windows.Forms.TextBox txtScriptFile;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtWaitTime;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.RadioButton rbNone;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtThreads;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cbServers;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbDatabases;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmTestBenchAction(Action action, ArrayList dbConnections)
		{
			
			_action=action;
			InitializeComponent();
			SetServers(dbConnections);
			txtDesc.Text=_action.Description;
			txtName.Text=_action.Name;
			txtThreads.Text=_action.Multiplier.ToString();
			if(_action.Type==ActionType.Script)
			{
				rbScript.Checked=true;
				txtScriptFile.Text=_action.ScriptFile;
				txtScriptFile.Enabled=true;
				rbWait.Checked=false;
				txtWaitTime.Enabled=false;
				rbNone.Checked=false;
				txtThreads.Enabled=true;
			}
			else if(_action.Type==ActionType.Wait)
			{
				rbScript.Checked=false;
				txtScriptFile.Enabled=false;
				rbWait.Checked=true;
				txtWaitTime.Enabled=true;
				txtWaitTime.Text=_action.WaitMilliSeconds.ToString();
				rbNone.Checked=false;
				txtThreads.Enabled=false;
				
			}
			else if(_action.Type==ActionType.None)
			{
				rbScript.Checked=false;
				txtScriptFile.Enabled=false;
				rbWait.Checked=false;
				txtWaitTime.Enabled=false;
				rbNone.Checked=true;
				txtThreads.Enabled=false;
			}
			else
			{
				rbScript.Checked=false;
				txtScriptFile.Enabled=false;
				rbWait.Checked=false;
				txtWaitTime.Enabled=false;
				rbNone.Checked=false;
				txtThreads.Enabled=false;
			}
		}
		public FrmTestBenchAction(ArrayList dbConnections)
		{
			
			InitializeComponent();
			SetServers(dbConnections);
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rbScript = new System.Windows.Forms.RadioButton();
			this.rbWait = new System.Windows.Forms.RadioButton();
			this.txtScriptFile = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtWaitTime = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cbDatabases = new System.Windows.Forms.ComboBox();
			this.txtThreads = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cbServers = new System.Windows.Forms.ComboBox();
			this.rbNone = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name;";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(88, 32);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(336, 20);
			this.txtName.TabIndex = 1;
			this.txtName.Text = "";
			// 
			// txtDesc
			// 
			this.txtDesc.Location = new System.Drawing.Point(88, 56);
			this.txtDesc.Multiline = true;
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.Size = new System.Drawing.Size(336, 64);
			this.txtDesc.TabIndex = 3;
			this.txtDesc.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Description:";
			// 
			// rbScript
			// 
			this.rbScript.Checked = true;
			this.rbScript.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbScript.Location = new System.Drawing.Point(24, 24);
			this.rbScript.Name = "rbScript";
			this.rbScript.TabIndex = 4;
			this.rbScript.TabStop = true;
			this.rbScript.Text = "Script";
			this.rbScript.CheckedChanged += new System.EventHandler(this.rbScript_CheckedChanged);
			// 
			// rbWait
			// 
			this.rbWait.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbWait.Location = new System.Drawing.Point(24, 160);
			this.rbWait.Name = "rbWait";
			this.rbWait.TabIndex = 5;
			this.rbWait.Text = "Wait";
			this.rbWait.CheckedChanged += new System.EventHandler(this.rbWait_CheckedChanged);
			// 
			// txtScriptFile
			// 
			this.txtScriptFile.Location = new System.Drawing.Point(160, 48);
			this.txtScriptFile.Name = "txtScriptFile";
			this.txtScriptFile.Size = new System.Drawing.Size(208, 20);
			this.txtScriptFile.TabIndex = 6;
			this.txtScriptFile.Text = "";
			// 
			// btnBrowse
			// 
			this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnBrowse.Location = new System.Drawing.Point(376, 48);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(32, 24);
			this.btnBrowse.TabIndex = 7;
			this.btnBrowse.Text = "...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "File:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(56, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Seconds:";
			// 
			// txtWaitTime
			// 
			this.txtWaitTime.Enabled = false;
			this.txtWaitTime.Location = new System.Drawing.Point(160, 184);
			this.txtWaitTime.Name = "txtWaitTime";
			this.txtWaitTime.Size = new System.Drawing.Size(24, 20);
			this.txtWaitTime.TabIndex = 9;
			this.txtWaitTime.Text = "0";
			this.txtWaitTime.TextChanged += new System.EventHandler(this.txtWaitTime_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.cbDatabases);
			this.groupBox1.Controls.Add(this.txtThreads);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.cbServers);
			this.groupBox1.Controls.Add(this.rbNone);
			this.groupBox1.Controls.Add(this.rbWait);
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.txtWaitTime);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtScriptFile);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.rbScript);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 136);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(416, 248);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Action type";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(56, 96);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 16);
			this.label7.TabIndex = 17;
			this.label7.Text = "Database:";
			// 
			// cbDatabases
			// 
			this.cbDatabases.Location = new System.Drawing.Point(160, 96);
			this.cbDatabases.Name = "cbDatabases";
			this.cbDatabases.Size = new System.Drawing.Size(208, 21);
			this.cbDatabases.TabIndex = 16;
			this.cbDatabases.Text = "comboBox1";
			// 
			// txtThreads
			// 
			this.txtThreads.Enabled = false;
			this.txtThreads.Location = new System.Drawing.Point(160, 136);
			this.txtThreads.Name = "txtThreads";
			this.txtThreads.Size = new System.Drawing.Size(24, 20);
			this.txtThreads.TabIndex = 14;
			this.txtThreads.Text = "1";
			this.txtThreads.TextChanged += new System.EventHandler(this.txtThreads_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(56, 136);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(104, 16);
			this.label6.TabIndex = 15;
			this.label6.Text = "Number of threads:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(56, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 13;
			this.label5.Text = "Data source:";
			// 
			// cbServers
			// 
			this.cbServers.Location = new System.Drawing.Point(160, 72);
			this.cbServers.Name = "cbServers";
			this.cbServers.Size = new System.Drawing.Size(208, 21);
			this.cbServers.TabIndex = 12;
			this.cbServers.Text = "comboBox1";
			this.cbServers.SelectedIndexChanged += new System.EventHandler(this.cbServers_SelectedIndexChanged);
			// 
			// rbNone
			// 
			this.rbNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbNone.Location = new System.Drawing.Point(24, 208);
			this.rbNone.Name = "rbNone";
			this.rbNone.TabIndex = 11;
			this.rbNone.Text = "None ";
			this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
			// 
			// btnOk
			// 
			this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOk.Location = new System.Drawing.Point(272, 392);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 12;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(352, 392);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// FrmTestBenchAction
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(448, 438);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtDesc);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "FrmTestBenchAction";
			this.Text = "TestBench Action";
			this.Load += new System.EventHandler(this.FrmTestBenchAction_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = Application.ExecutablePath + @"\scriptfiles";
			openFileDialog1.Filter = "SQL files (*.SQL)|*.SQL|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex = 2 ;
			openFileDialog1.RestoreDirectory = true ;

			if(openFileDialog1.ShowDialog() == DialogResult.OK)
				txtScriptFile.Text= openFileDialog1.FileName;
 
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			_action.Name=txtName.Text;
			_action.Description=txtDesc.Text;
			if(rbScript.Checked)
			{
				_action.Type=ActionType.Script;
				_action.ScriptFile=txtScriptFile.Text;
				_action.Multiplier=Convert.ToInt16(txtThreads.Text);
				_action.ConnectionString = ((Server)cbServers.SelectedItem).Connection.ConnectionString;
				_action.ConnectionType = ((Server)cbServers.SelectedItem).Connection.ToString();
				//_action.dataConnection = ((Server)cbServers.SelectedItem).Connection;
				_action.DatabaseName = cbDatabases.SelectedItem.ToString();
			}
			else if(rbWait.Checked)
			{
				_action.Type=ActionType.Wait;
				_action.WaitMilliSeconds=Convert.ToInt32(txtWaitTime.Text);
			}
			else
			{
				_action.Type=ActionType.None;
			}
			this.DialogResult=DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		private void FrmTestBenchAction_Load(object sender, System.EventArgs e)
		{
		
		}

		private void rbScript_CheckedChanged(object sender, System.EventArgs e)
		{
			txtScriptFile.Enabled=true;
			txtWaitTime.Enabled=false;
			txtThreads.Enabled=true;
		}
		private void rbWait_CheckedChanged(object sender, System.EventArgs e)
		{
			txtScriptFile.Enabled=false;
			txtWaitTime.Enabled=true;
			txtThreads.Enabled=false;
		}

		private void rbNone_CheckedChanged(object sender, System.EventArgs e)
		{
			txtScriptFile.Enabled=false;
			txtWaitTime.Enabled=false;
			txtThreads.Enabled=false;
		}

		private void txtThreads_TextChanged(object sender, System.EventArgs e)
		{
			int cursorPos = txtThreads.SelectionStart;

			string notValid = "[^0-9]";
			Regex regex = new Regex(notValid);
			
			string oldText = txtThreads.Text;
			string newText = regex.Replace(oldText, "");

			if (newText != oldText) 
			{
				txtThreads.Text = newText;
				int i;
				for (i = 0; i < newText.Length; i++) if (newText[i] != oldText[i]) break;
				txtThreads.SelectionStart = i;
			}
		}
		private void SetServers(ArrayList dbConnections)
		{
			foreach(MainForm.DBConnection dbConnection in dbConnections)
			{
				QueryCommander.Database.IDatabaseManager db = QueryCommander.Database.DatabaseFactory.CreateNew(dbConnection.Connection);
				ArrayList dbArr =  db.GetDatabasesObjects(dbConnection.ConnectionName,dbConnection.Connection);
				cbServers.Items.Add(new Server(db,dbArr, dbConnection.ConnectionName,dbConnection.Connection));
				
			}
			if(cbServers.Items.Count>0)
			{
				cbServers.SelectedIndex=0;
				//cbServers_SelectedIndexChanged(cbServers,null);
			}
		
		}

		private void cbServers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			cbDatabases.Items.Clear();

			foreach(QueryCommander.Database.DB db in ((Server)cbServers.SelectedItem).Databases )
				cbDatabases.Items.Add(db.Name);

			if(cbDatabases.Items.Count>0)
			{
				cbDatabases.SelectedIndex=0;
			}
		}

		private void txtWaitTime_TextChanged(object sender, System.EventArgs e)
		{
			int cursorPos = txtWaitTime.SelectionStart;

			string notValid = "[^0-9]";
			Regex regex = new Regex(notValid);
			
			string oldText = txtWaitTime.Text;
			string newText = regex.Replace(oldText, "");

			if (newText != oldText) 
			{
				txtWaitTime.Text = newText;
				int i;
				for (i = 0; i < newText.Length; i++) if (newText[i] != oldText[i]) break;
				txtWaitTime.SelectionStart = i;
			}
		}
	
		private class Server
		{
			public Server(QueryCommander.Database.IDatabaseManager databaseManager, ArrayList databases, string connectionName, System.Data.IDbConnection connection)
			{
				this.DatabaseManager=databaseManager;
				this.Databases=databases;
				this.ConnectionName=connectionName;
				this.Connection = connection;
			}
			public string ConnectionName;
			public QueryCommander.Database.IDatabaseManager DatabaseManager;
			public ArrayList Databases;
			public System.Data.IDbConnection Connection;
			public override string ToString()
			{
				return this.ConnectionName;
			}
		}
	}

		
}
