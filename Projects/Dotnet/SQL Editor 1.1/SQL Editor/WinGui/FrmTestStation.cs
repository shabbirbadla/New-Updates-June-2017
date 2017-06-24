using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QueryCommander.General.TestBench;
//////using QueryCommander.WinGui.CommonDialogs;
using System.IO;

namespace QueryCommander
{
	/// <summary>
	/// Summary description for FrmTestStation.
	/// </summary>
	public class FrmTestStation : FrmBaseContent
	{
		#region Members
		private Hashtable treenodes = new Hashtable();
		private Action rootAction;
		private ActionStarter starter=null;
		private TestStationTreeNode _testStationTreeNode=null;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnPerfmon;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ContextMenu contextMenuActions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView tvActions;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.MenuItem menuItemAddAction;
		private System.Windows.Forms.MenuItem menuItemRemoveAction;
		private System.Windows.Forms.MenuItem menuItemAlterAction;
		private System.Windows.Forms.Button btnSuspend;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.MenuItem menuItemResultProperties;
		private System.Windows.Forms.MenuItem menuItemResultToOutPutWindow;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.ComponentModel.IContainer components;
		#endregion
		#region Default 
		public FrmTestStation(Form mdiParentForm)
		{
			this.MdiParentForm=mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.DockAreas.DockRight)));
			
			this.DefaultHelpUrl="::/Testbench.htm";
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmTestStation));
			this.tvActions = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.btnStart = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblName = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnPerfmon = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.contextMenuActions = new System.Windows.Forms.ContextMenu();
			this.menuItemAddAction = new System.Windows.Forms.MenuItem();
			this.menuItemRemoveAction = new System.Windows.Forms.MenuItem();
			this.menuItemAlterAction = new System.Windows.Forms.MenuItem();
			this.menuItemResultProperties = new System.Windows.Forms.MenuItem();
			this.btnSuspend = new System.Windows.Forms.Button();
			this.menuItemResultToOutPutWindow = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvActions
			// 
			this.tvActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tvActions.ImageList = this.imageList1;
			this.tvActions.Location = new System.Drawing.Point(0, 160);
			this.tvActions.Name = "tvActions";
			this.tvActions.Size = new System.Drawing.Size(296, 512);
			this.tvActions.TabIndex = 0;
			this.tvActions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvActions_MouseUp);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(296, 160);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// btnStart
			// 
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnStart.ImageIndex = 7;
			this.btnStart.ImageList = this.imageList1;
			this.btnStart.Location = new System.Drawing.Point(40, 112);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(64, 40);
			this.btnStart.TabIndex = 5;
			this.btnStart.Text = "Start";
			this.btnStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lblName);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnPerfmon);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(280, 96);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Senario:";
			// 
			// lblName
			// 
			this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblName.Location = new System.Drawing.Point(72, 64);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(192, 16);
			this.lblName.TabIndex = 8;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(96, 24);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 7;
			this.button2.Text = "New";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(16, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 6;
			this.button1.Text = "Open";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 5;
			this.label1.Text = "Name: ";
			// 
			// btnPerfmon
			// 
			this.btnPerfmon.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnPerfmon.Location = new System.Drawing.Point(176, 24);
			this.btnPerfmon.Name = "btnPerfmon";
			this.btnPerfmon.Size = new System.Drawing.Size(72, 23);
			this.btnPerfmon.TabIndex = 8;
			this.btnPerfmon.Text = "Perfmon";
			this.btnPerfmon.Click += new System.EventHandler(this.btnPerfmon_Click);
			// 
			// btnStop
			// 
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnStop.ImageIndex = 6;
			this.btnStop.ImageList = this.imageList1;
			this.btnStop.Location = new System.Drawing.Point(112, 112);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(64, 40);
			this.btnStop.TabIndex = 7;
			this.btnStop.Text = "Stop";
			this.btnStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// contextMenuActions
			// 
			this.contextMenuActions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.menuItemAddAction,
																							   this.menuItemRemoveAction,
																							   this.menuItemAlterAction,
																							   this.menuItem2,
																							   this.menuItemResultProperties,
																							   this.menuItemResultToOutPutWindow});
			// 
			// menuItemAddAction
			// 
			this.menuItemAddAction.Index = 0;
			this.menuItemAddAction.Text = "&Add Action";
			this.menuItemAddAction.Click += new System.EventHandler(this.menuItemAddAction_Click);
			// 
			// menuItemRemoveAction
			// 
			this.menuItemRemoveAction.Index = 1;
			this.menuItemRemoveAction.Text = "&Remove Action";
			this.menuItemRemoveAction.Click += new System.EventHandler(this.menuItemRemoveAction_Click);
			// 
			// menuItemAlterAction
			// 
			this.menuItemAlterAction.Index = 2;
			this.menuItemAlterAction.Text = "A&lter Action";
			this.menuItemAlterAction.Click += new System.EventHandler(this.menuItemAlterAction_Click);
			// 
			// menuItemResultProperties
			// 
			this.menuItemResultProperties.Index = 4;
			this.menuItemResultProperties.Text = "Result properties";
			this.menuItemResultProperties.Click += new System.EventHandler(this.menuItemResultProperties_Click);
			// 
			// btnSuspend
			// 
			this.btnSuspend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSuspend.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnSuspend.ImageIndex = 8;
			this.btnSuspend.ImageList = this.imageList1;
			this.btnSuspend.Location = new System.Drawing.Point(184, 112);
			this.btnSuspend.Name = "btnSuspend";
			this.btnSuspend.Size = new System.Drawing.Size(64, 40);
			this.btnSuspend.TabIndex = 8;
			this.btnSuspend.Text = "Suspend";
			this.btnSuspend.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnSuspend.Visible = false;
			this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
			// 
			// menuItemResultToOutPutWindow
			// 
			this.menuItemResultToOutPutWindow.Index = 5;
			this.menuItemResultToOutPutWindow.Text = "Result to output window";
			this.menuItemResultToOutPutWindow.Click += new System.EventHandler(this.menuItemResultToOutPutWindow_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.Text = "-";
			// 
			// FrmTestStation
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 670);
			this.Controls.Add(this.btnSuspend);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.tvActions);
			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.DockAreas.DockRight)));
			this.Name = "FrmTestStation";
			this.Text = "Test station";
			this.Load += new System.EventHandler(this.FrmTestStation_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion
		#region Events
		private void FrmTestStation_Load(object sender, System.EventArgs e)
		{
			btnStop.Enabled=false;
		}
		

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			((TestStationTreeNode)tvActions.Nodes[0]).ResetImage();

			Action action = ((TestStationTreeNode)tvActions.Nodes[0]).action;
			if(action.FileName== string.Empty)
				action.FileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "temp.QCTestBench");

			action.Save();

			starter = new ActionStarter();
			starter.StartEvent+=new QueryCommander.General.TestBench.ActionStarter.StartEventHandler(starter_StartEvent);
			starter.DisposeEvent+=new QueryCommander.General.TestBench.ActionStarter.DisposeEventHandler(starter_DisposeEvent);
			
			MainForm mainForm = (MainForm)this.MdiParentForm;
			starter.Start(action.FileName, mainForm.ActiveQueryForm.dbConnection, mainForm.ActiveQueryForm.DatabaseName);

			
			
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
 
			saveFileDialog1.Filter = "QCTestBench files (*.QCTestBench)|*.QCTestBench"  ;
			saveFileDialog1.FilterIndex = 2 ;
			saveFileDialog1.RestoreDirectory = true ;
 
			if(saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				int pos = saveFileDialog1.FileName.LastIndexOf("\\")+1;
				lblName.Text = saveFileDialog1.FileName.Substring(pos);
				tvActions.Nodes.Clear();
				treenodes.Clear();

				rootAction = new Action();
				rootAction.Name="Root";
				rootAction.Description="...";
				rootAction.Type=ActionType.Root;
				rootAction.FileName=saveFileDialog1.FileName;
			
				FrmTestBenchAction frmTestBenchAction=new FrmTestBenchAction(rootAction, ((MainForm)this.MdiParentForm).DBConnections);
				if(frmTestBenchAction.ShowDialogWindow(this)==DialogResult.OK)
				{
					TestStationTreeNode node = new TestStationTreeNode(frmTestBenchAction._action);
					//node.ImageIndex=1;
					tvActions.BeginUpdate();
					tvActions.Nodes.Add(node);
					tvActions.EndUpdate();
					treenodes.Add(rootAction.ID,node);
				}
			}
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			try
			{
				starter.Abort();
				((TestStationTreeNode)tvActions.Nodes[0]).ResetImage();
			}
			catch
			{
				MessageBox.Show(this, Localization.GetString("FrmTestStation.btnStop_Click"),"QueryCommander");
				return;
			}
		}

		private void btnSuspend_Click(object sender, System.EventArgs e)
		{
			if(btnSuspend.ImageIndex==7)
			{
				// Suspend
				btnSuspend.ImageIndex=8;
				starter.Suspend();
			}
			else
			{
				// Resume
				btnSuspend.ImageIndex=7;
				starter.Resume();
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = Application.ExecutablePath;
			openFileDialog1.Filter = "QCTestBench files (*.QCTestBench)|*.QCTestBench" ;
			openFileDialog1.FilterIndex = 2 ;
			openFileDialog1.RestoreDirectory = true ;

			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				int pos = openFileDialog1.FileName.LastIndexOf("\\")+1;
				lblName.Text = openFileDialog1.FileName.Substring(pos);

				rootAction = Action.Load(openFileDialog1.FileName);
				tvActions.Nodes.Clear();
				treenodes.Clear();

				tvActions.BeginUpdate();
				tvActions.Nodes.Add( AddTreeViewNode(rootAction));

				tvActions.EndUpdate();
				tvActions.ExpandAll();
				
			}
		}
//		private void Test()
//		{
//			MainForm frm = (MainForm)this.MdiParentForm;
//
//			foreach(MainForm.DBConnection dbConnection in frm.DBConnections)
//			{
//				QueryCommander.Database.IDatabaseManager db = QueryCommander.Database.DatabaseFactory.CreateNew(dbConnection.Connection);
//				ArrayList dbArr =  db.GetDatabasesObjects(dbConnection.ConnectionName,dbConnection.Connection);
//			}
//		
//		}
		
		private void btnPerfmon_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("perfmon.exe");
		}

	
		private void tvActions_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				_testStationTreeNode = (TestStationTreeNode)tvActions.GetNodeAt(e.X,e.Y);
				if(_testStationTreeNode != null)
				{
					contextMenuActions.Show(tvActions,new Point(e.X,e.Y));
				}
			}
		}

		
		private void menuItemResultProperties_Click(object sender, System.EventArgs e)
		{
			FrmTestBenchActionProperties frm = new FrmTestBenchActionProperties(_testStationTreeNode.action);
			frm.ShowDialogWindow(this);
		}
		private void menuItemAlterAction_Click(object sender, System.EventArgs e)
		{
			FrmTestBenchAction frmTestBenchAction=new FrmTestBenchAction(_testStationTreeNode.action,((MainForm)this.MdiParentForm).DBConnections);
			if(frmTestBenchAction.ShowDialogWindow(this)==DialogResult.OK)
			{
				tvActions.BeginUpdate();
				_testStationTreeNode.action = frmTestBenchAction._action;
				_testStationTreeNode.Expand();
				tvActions.EndUpdate();

			}
		}

		private void menuItemRemoveAction_Click(object sender, System.EventArgs e)
		{
			TestStationTreeNode parentNode=(TestStationTreeNode)_testStationTreeNode.Parent;
			
			for(int i=0;i<parentNode.action.Actions.Count;i++)
				if(parentNode.action.Actions[i].ID==_testStationTreeNode.action.ID)
				{
					parentNode.action.Actions.RemoveAt(i);
					break;
				}
			tvActions.BeginUpdate();
			parentNode.Nodes.Remove(_testStationTreeNode);
			tvActions.EndUpdate();
		}

		private void menuItemAddAction_Click(object sender, System.EventArgs e)
		{
			Action action = new Action();
			action.Name="[Script name]";
			action.Description="...";
			action.Type=ActionType.Script;
			
			FrmTestBenchAction frmTestBenchAction=new FrmTestBenchAction(action,((MainForm)this.MdiParentForm).DBConnections);
			if(frmTestBenchAction.ShowDialogWindow(this)==DialogResult.OK)
			{
				TestStationTreeNode node = new TestStationTreeNode(frmTestBenchAction._action);
				tvActions.BeginUpdate();

				_testStationTreeNode.Nodes.Add(node);
				_testStationTreeNode.Expand();
				
				tvActions.EndUpdate();
				_testStationTreeNode.action.Actions.Add(action);
				treenodes.Add(action.ID,node);
			}
		}

			
		private void starter_StartEvent(object sender, RootActionEventArgs args)
		{
			SetActionStatus(true,args.ActionID,null);
			btnStop.Enabled=true;
		}

		private void starter_DisposeEvent(object sender, RootActionEventArgs args)
		{
			SetActionStatus(false,args.ActionID,args.actionProperties);
			btnStop.Enabled=false;;
		}
		
		private void menuItemResultToOutPutWindow_Click(object sender, System.EventArgs e)
		{
			((MainForm)this.MdiParentForm).OutputWindow.BrowseTable(_testStationTreeNode.action.GetActionProperties().DataSet,null);
			((MainForm)this.MdiParentForm).OutputWindow.Activate();
		}
	
	
		#endregion
		#region Methods
		private void SetActionStatus(bool start, Guid ActionID, ActionProperties actionProperties )
		{
			tvActions.BeginUpdate();
			TestStationTreeNode node = (TestStationTreeNode)treenodes[ActionID];
			node.action.SetActionProperties(actionProperties);
			if(!start)
			{
				node.ImageIndex=4;
				node.SelectedImageIndex=4;
			}
			else
			{
				node.ImageIndex=5;
				node.SelectedImageIndex=5;
			}
			tvActions.EndUpdate();
		}

		private TestStationTreeNode AddTreeViewNode(Action action)
		{
			
			TestStationTreeNode node = new TestStationTreeNode(action);
			treenodes.Add(action.ID,node);
			foreach(Action childAction in action.Actions)
				node.Nodes.Add( AddTreeViewNode(childAction) );

			return node;
		}

		#endregion

		
	}
}
