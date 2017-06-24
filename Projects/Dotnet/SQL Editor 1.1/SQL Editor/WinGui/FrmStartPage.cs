
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;
using System.IO;
using System.Xml;
using System.Text;


namespace QueryCommander
{
	/// <summary>
	/// Summary description for DummyDoc1.
	/// </summary>
	public class FrmStartPage :FrmBaseContent
	{
		#region Private members
		private string _downLoadUri="";
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.CheckBox chbShowNextTime;
		
		private System.Windows.Forms.Timer timer1;
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.MenuItem menuItem5;
		#endregion
		#region Public members
		public bool IsActive;

		#endregion
		#region Default
		public FrmStartPage(Form mdiParentForm)
		{
			this.MdiParentForm=mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
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


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmStartPage));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.chbShowNextTime = new System.Windows.Forms.CheckBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3,
																					  this.menuItem5,
																					  this.menuItem4});
			this.menuItem1.Text = "&File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "-";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "&Close";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "%Exit";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOk.Location = new System.Drawing.Point(272, 368);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Close";
			this.btnOk.Visible = false;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnUpdate.Location = new System.Drawing.Point(360, 368);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(80, 23);
			this.btnUpdate.TabIndex = 2;
			this.btnUpdate.Text = "Update now";
			this.btnUpdate.Visible = false;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// chbShowNextTime
			// 
			this.chbShowNextTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chbShowNextTime.BackColor = System.Drawing.Color.White;
			this.chbShowNextTime.Checked = true;
			this.chbShowNextTime.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbShowNextTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbShowNextTime.Location = new System.Drawing.Point(448, 368);
			this.chbShowNextTime.Name = "chbShowNextTime";
			this.chbShowNextTime.Size = new System.Drawing.Size(160, 24);
			this.chbShowNextTime.TabIndex = 3;
			this.chbShowNextTime.Text = "Show start page on startup";
			this.chbShowNextTime.Visible = false;
			this.chbShowNextTime.CheckedChanged += new System.EventHandler(this.chbShowNextTime_CheckedChanged);
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(2, 2);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(636, 402);
			this.axWebBrowser1.TabIndex = 4;
			// 
			// FrmStartPage
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 406);
			this.Controls.Add(this.chbShowNextTime);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.axWebBrowser1);
			this.DockPadding.All = 2;
			this.Name = "FrmStartPage";
			this.Text = "Start page";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FrmQuery_Closing);
			this.Load += new System.EventHandler(this.FrmQuery_Load);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#endregion
		#region Events
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		}

		protected override string GetPersistString()
		{
			return "";
		}

		private void FrmQuery_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
		private void FrmQuery_Load(object sender, System.EventArgs e)
		{
			
		}
		
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled=false;

			Object o = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			string remoteUri = "http://querycommander.sourceforge.net/startpage.xml";//"http://querycommander.rockwolf.com/startpage.xml";

			byte[] myDataBuffer = webClient.DownloadData (remoteUri);
			string xmlContent = Encoding.ASCII.GetString(myDataBuffer);

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(remoteUri);

			xmlDoc.LoadXml(xmlContent);

			XmlNodeList nList =  xmlDoc.GetElementsByTagName("version");

			string latestVersion = nList[0].Attributes.GetNamedItem("latest").Value;

			_downLoadUri =  nList[0].Attributes.GetNamedItem("url").Value;

			version thisVer = new version( System.Windows.Forms.Application.ProductVersion);
			version latestVer = new version(latestVersion);

			bool r = thisVer.isLatestVersion(latestVer);

			if(!thisVer.isLatestVersion(latestVer))
			{
				XmlNodeList statementList =  xmlDoc.GetElementsByTagName("statement");
				statementList[0].InnerText= "There is a new version. Press [Update now] to update";
				btnUpdate.Enabled = true;
			}
			else
				btnUpdate.Enabled = false;

			xmlDoc.Save(  Application.StartupPath + @"\startpage.xml");
			CopyEmbeddedResource("QueryCommander.startpage.xsl",Application.StartupPath + @"\startpage.xsl");

			axWebBrowser1.Navigate(Application.StartupPath + @"\startpage.xml", ref o, ref o, ref o, ref o);
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("iexplore.exe",_downLoadUri);
			}
			catch
			{return;}
//			return;
//
//
//			string url="https://sourceforge.net/project/showfiles.php?group_id=123942&package_id=135409";
//			
//
//			System.Net.WebClient webClient = new System.Net.WebClient();
//			
//			string path = _downLoadUri.Substring(0,_downLoadUri.LastIndexOf("/"));
//			string file = _downLoadUri.Substring(_downLoadUri.LastIndexOf("/")+1, _downLoadUri.Length - _downLoadUri.LastIndexOf("/") - 1);
//			
//			
//
//			FolderBrowserDialog fbd = new FolderBrowserDialog();
//			fbd.Description = "Save " + file;
//			if (fbd.ShowDialog() == DialogResult.OK)
//			{
//				MainForm frm = (MainForm)MdiParentForm;
//				frm.statusBar.Panels[3].Text = "Downloading " +_downLoadUri + "...";
//				Application.DoEvents();
//			
//				string localPath = fbd.SelectedPath + @"\" + file;
//			
//				webClient.DownloadFile(_downLoadUri, localPath);
//			
//				System.Diagnostics.Process.Start(localPath);
//
//				this.Close();
//				Application.Exit();
//			}
		}

		private void chbShowNextTime_CheckedChanged(object sender, System.EventArgs e)
		{

			QueryCommander.Config.Settings settings = QueryCommander.Config.Settings.Load();
			settings.ShowStartPage=false;
			settings.Save();
		}

		#endregion
		#region Private methods
		private void CopyEmbeddedResource(string resource, string filepath)
		{
			System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
			System.IO.Stream str = a.GetManifestResourceStream(resource);
			System.IO.StreamReader reader = new StreamReader(str);
			System.IO.FileStream fileStream = new FileStream(filepath,System.IO.FileMode.Create);
			System.IO.StreamWriter writer = new StreamWriter(fileStream);
			writer.WriteLine(reader.ReadToEnd());
			writer.Close();
		}
		#endregion
		#region Public methods
		public void ShowContent(string uri)
		{
			Object o = null;
			
			axWebBrowser1.Navigate(uri, ref o, ref o, ref o, ref o);
		}
		public void ShowContent(string DownloadUri, string FilePath, bool ShowUpdate)
		{
			Object o = null;
			_downLoadUri = DownloadUri;
			System.Net.WebClient webClient = new System.Net.WebClient();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(FilePath);

			btnUpdate.Enabled = ShowUpdate;
			
			axWebBrowser1.Navigate(FilePath, ref o, ref o, ref o, ref o);
		}

		#endregion	
		#region classes
		private class version
		{
			public int Major;
			public int Minor;
			public int Build;
			public int Revision;
			public version(string versionString)
			{
				string delimStr = ".";
				char [] delimiter = delimStr.ToCharArray();
				string [] split = null;
				split = versionString.Split(delimiter);
				int i = split.Length;
				if(split.Length>0)
					Major = Convert.ToInt16(split[0]);
				else
					Major = 0;
				if(split.Length>1)
					Minor = Convert.ToInt16(split[1]);
				else
					Minor = 0;
				if(split.Length>2)
					Build = Convert.ToInt16(split[2]);
				else
					Build = 0;
				if(split.Length>3)
					Revision = Convert.ToInt16(split[3]);
				else
					Revision = 0;

			}
		
			public bool isLatestVersion(version latestVersion)
			{
				if(this.Major<latestVersion.Major) return false;
				if(this.Minor<latestVersion.Minor) return false;
				if(this.Build<latestVersion.Build) return false;
				if(this.Revision<latestVersion.Revision) return false;

				return true;

			}
		}
		#endregion
		

	}
}
