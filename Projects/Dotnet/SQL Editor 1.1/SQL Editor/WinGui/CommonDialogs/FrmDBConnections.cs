
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using SQLEditor.Database;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmDBConnections.
	/// </summary>
	public class FrmDBConnections : FrmBaseDialog
	{
		#region Private members
		private MainForm _mainForm=null;
		private DataSourceCollection _dataSourceCollection;
		XmlDocument xmlDBConnections = new XmlDocument();
        ArrayList DBConnections = new ArrayList();
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCansel;
        private Label label1;
        private ComboBox cbFilter;
        private Button btnNew;
        private Button BtnEdit;
        private ListView lstvConnections;
        private ColumnHeader columnHeader1;
        private Button btnDelete;
        private GroupBox groupBox1;
		private System.ComponentModel.IContainer components;
		#endregion
		#region Default
		public FrmDBConnections(MainForm mainForm)
		{
			_mainForm=mainForm;
			InitializeComponent();
			PopulateDBConnections();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDBConnections));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCansel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.BtnEdit = new System.Windows.Forms.Button();
            this.lstvConnections = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(196, 238);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCansel
            // 
            this.btnCansel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCansel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCansel.Location = new System.Drawing.Point(284, 238);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(80, 24);
            this.btnCansel.TabIndex = 6;
            this.btnCansel.Text = "Cancel";
            this.btnCansel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Filter:";
            this.label1.Visible = false;
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilter.Items.AddRange(new object[] {
            "Microsoft SqlClient"});
            this.cbFilter.Location = new System.Drawing.Point(48, 20);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(312, 21);
            this.cbFilter.TabIndex = 8;
            this.cbFilter.Visible = false;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnNew
            // 
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNew.Location = new System.Drawing.Point(102, 195);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(80, 24);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // BtnEdit
            // 
            this.BtnEdit.Enabled = false;
            this.BtnEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnEdit.Location = new System.Drawing.Point(190, 195);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(80, 24);
            this.BtnEdit.TabIndex = 3;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // lstvConnections
            // 
            this.lstvConnections.CheckBoxes = true;
            this.lstvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstvConnections.Location = new System.Drawing.Point(6, 12);
            this.lstvConnections.Name = "lstvConnections";
            this.lstvConnections.Size = new System.Drawing.Size(352, 177);
            this.lstvConnections.TabIndex = 0;
            this.lstvConnections.UseCompatibleStateImageBehavior = false;
            this.lstvConnections.View = System.Windows.Forms.View.Details;
            this.lstvConnections.SelectedIndexChanged += new System.EventHandler(this.lstvConnections_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Data source name";
            this.columnHeader1.Width = 347;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Location = new System.Drawing.Point(278, 195);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 24);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Remove";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.lstvConnections);
            this.groupBox1.Controls.Add(this.BtnEdit);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.cbFilter);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(6, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 227);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // FrmDBConnections
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(374, 267);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCansel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDBConnections";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data sources";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		#endregion
		private void PopulateDBConnections()
		{			
			_dataSourceCollection = DataSourceFactory.GetDataSources();
			lstvConnections.Items.Clear();

            foreach(DataSource ds in _dataSourceCollection)
			{
                if (!IsFiltered(ds.ConnectionType)) { continue; }

				ListViewItem item;
				if(ds.FriendlyName ==null || ds.FriendlyName.Length==0)
					item = new ListViewItem(ds.Name);
				else
					item = new ListViewItem(ds.Name + " [" + ds.FriendlyName + "]");
				item.Checked = ds.IsConnected;
				item.Tag = ds.ID;
				lstvConnections.Items.Add(item);
			}
		}
		private bool IsFiltered(SQLEditor.Database.DBConnectionType type)
		{
			switch (cbFilter.Text)
			{
				case "Microsoft SqlClient":
					return type==SQLEditor.Database.DBConnectionType.MicrosoftSqlClient;
				case "Microsoft OleDb":
					return type==SQLEditor.Database.DBConnectionType.MicrosoftOleDb;
				default:
					return true;
			}
		}	
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			bool connected=false;
			foreach(ListViewItem item in lstvConnections.Items)
			{
				DataSource ds = _dataSourceCollection.FindByID((Guid)item.Tag);
				if(item.Checked)
				{
					ds.IsConnected=true;
					connected =true;
				}
				else
					ds.IsConnected=false;
			}
            //////DataSourceFactory.Save(_dataSourceCollection);
            if (connected)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(Localization.GetString("FrmDBConnections.btnOk_Click"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

		}
		private void btnNew_Click(object sender, System.EventArgs e)
		{
			FrmDBConnection frm = new FrmDBConnection();
			frm.ShowDialogWindow(this);
			if(frm.DialogResult==DialogResult.OK)
			{
				_dataSourceCollection.Add(frm.dataSource);
				DataSourceFactory.Save(_dataSourceCollection);
			}
			PopulateDBConnections();
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			_dataSourceCollection.Delete(_dataSourceCollection.FindByID((Guid)lstvConnections.SelectedItems[0].Tag));
			PopulateDBConnections();
			return;
		}
		private void btnCansel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;
		}
		private void BtnEdit_Click(object sender, System.EventArgs e)
		{
			if(lstvConnections.SelectedItems.Count == 0)
			{
				MessageBox.Show(Localization.GetString("FrmDBConnections.btnDelete_Click"),this.Text,MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
				return;
			}
			string DSN = lstvConnections.SelectedItems[0].Text;
			FrmDBConnection frm = new FrmDBConnection(_dataSourceCollection.FindByID((Guid)lstvConnections.SelectedItems[0].Tag) );
			frm.ShowDialogWindow(this);
			if(frm.DialogResult==DialogResult.OK)
			{
				_dataSourceCollection.Delete(_dataSourceCollection.FindByID((Guid)lstvConnections.SelectedItems[0].Tag));
				_dataSourceCollection.Add(frm.dataSource);
				DataSourceFactory.Save(_dataSourceCollection);
			}
			PopulateDBConnections();
		}
		private void lstvConnections_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lstvConnections.SelectedItems.Count==0)
			{
				btnDelete.Enabled=false;
				BtnEdit.Enabled=false;
			}
			else
			{
				btnDelete.Enabled=true;
				BtnEdit.Enabled=true;
			}
		}
		private void cbFilter_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopulateDBConnections();
		}
	}
}
