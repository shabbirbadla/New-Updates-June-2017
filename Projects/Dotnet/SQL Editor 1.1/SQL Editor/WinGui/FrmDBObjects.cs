using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WeifenLuo.WinFormsUI;
using System.Data;
using System.Data.SqlClient;
using SQLEditor.General;
using SQLEditor.Database;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmBaseContent.
	/// </summary>
	public class FrmDBObjects : FrmBaseContent
	{
		#region Members
		private Hashtable DBTreeViewTypes = new Hashtable();
		private System.Windows.Forms.TreeView TvDBObjects;
		private System.Windows.Forms.ImageList imglDataObjects;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem miAddServer;
		private System.Windows.Forms.MenuItem miUseDatabase;
		private System.Windows.Forms.MenuItem miScriptCreate;
		private System.Windows.Forms.MenuItem miScriptAlter;
		private System.Windows.Forms.MenuItem miScriptDrop;
		private System.Windows.Forms.MenuItem miScriptExec;
		private System.Windows.Forms.MenuItem miScriptSelect;
		private System.Windows.Forms.MenuItem miScriptUpdate;
		private System.Windows.Forms.MenuItem miScriptInsert;
		private System.Windows.Forms.MenuItem miScriptDelete;
		private System.Windows.Forms.MenuItem miNew;
		private System.Windows.Forms.MenuItem miDelete;
		private System.Windows.Forms.MenuItem miRemoveServer;
		private System.Windows.Forms.MenuItem miScript;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem miCreateInsertScript;
		private System.Windows.Forms.MenuItem miCreateUpdateScript;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.MenuItem miRefreshDataObjects;
		private System.Windows.Forms.MenuItem mi_CheckIn;
		private System.Windows.Forms.MenuItem mi_CheckOut;
		private System.Windows.Forms.MenuItem mi_AddToSourceControl;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mi_DetachFromSourceControl;
		private System.Windows.Forms.MenuItem miOpenScript;
		private System.Windows.Forms.MenuItem mi_VSSSettings;
		private System.Windows.Forms.MenuItem mi_UndoCheckOut;
		private System.Windows.Forms.MenuItem mi_ShowHistory;
		private System.Windows.Forms.MenuItem mi_Persist2VSS;
		private System.Windows.Forms.MenuItem mi_OpenSourceFile;
        private System.Windows.Forms.MenuItem mi_ShowConstraints;
        private System.Windows.Forms.MenuItem mi_GetProcUsers;
        private System.Windows.Forms.MenuItem mi_ServerStats;
		private System.Windows.Forms.MenuItem mi_TableSpace;
		private System.Windows.Forms.MenuItem mi_TableStats;
		private System.Windows.Forms.MenuItem mi_UniqueRowColumns;
        private System.Windows.Forms.MenuItem mi_TablePermissions;
        private System.Windows.Forms.MenuItem mi_TableInfo;
		private System.Windows.Forms.MenuItem mi_Dependencies;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem mi_ForeignKeys;
		private System.Windows.Forms.MenuItem menuItem7;
		private TreeNode CurrentTreeNode;

		#endregion
		#region Default
		public FrmDBObjects(Form mdiParentForm)
		{
			this.MdiParentForm=mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.DockAreas.DockLeft)));

			this.DefaultHelpUrl="";
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDBObjects));
            this.imglDataObjects = new System.Windows.Forms.ImageList(this.components);
            this.TvDBObjects = new System.Windows.Forms.TreeView();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.miAddServer = new System.Windows.Forms.MenuItem();
            this.miRemoveServer = new System.Windows.Forms.MenuItem();
            this.mi_GetProcUsers = new System.Windows.Forms.MenuItem();
            this.mi_ServerStats = new System.Windows.Forms.MenuItem();
            this.miUseDatabase = new System.Windows.Forms.MenuItem();
            this.miScript = new System.Windows.Forms.MenuItem();
            this.miScriptCreate = new System.Windows.Forms.MenuItem();
            this.miScriptAlter = new System.Windows.Forms.MenuItem();
            this.miScriptDrop = new System.Windows.Forms.MenuItem();
            this.miScriptExec = new System.Windows.Forms.MenuItem();
            this.miScriptSelect = new System.Windows.Forms.MenuItem();
            this.miScriptUpdate = new System.Windows.Forms.MenuItem();
            this.miScriptInsert = new System.Windows.Forms.MenuItem();
            this.miScriptDelete = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.miCreateInsertScript = new System.Windows.Forms.MenuItem();
            this.miCreateUpdateScript = new System.Windows.Forms.MenuItem();
            this.miNew = new System.Windows.Forms.MenuItem();
            this.miDelete = new System.Windows.Forms.MenuItem();
            this.miRefreshDataObjects = new System.Windows.Forms.MenuItem();
            this.miOpenScript = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mi_TableInfo = new System.Windows.Forms.MenuItem();
            this.mi_ShowConstraints = new System.Windows.Forms.MenuItem();
            this.mi_ForeignKeys = new System.Windows.Forms.MenuItem();
            this.mi_TablePermissions = new System.Windows.Forms.MenuItem();
            this.mi_TableSpace = new System.Windows.Forms.MenuItem();
            this.mi_TableStats = new System.Windows.Forms.MenuItem();
            this.mi_UniqueRowColumns = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mi_Dependencies = new System.Windows.Forms.MenuItem();
            this.mi_CheckIn = new System.Windows.Forms.MenuItem();
            this.mi_CheckOut = new System.Windows.Forms.MenuItem();
            this.mi_AddToSourceControl = new System.Windows.Forms.MenuItem();
            this.mi_DetachFromSourceControl = new System.Windows.Forms.MenuItem();
            this.mi_VSSSettings = new System.Windows.Forms.MenuItem();
            this.mi_UndoCheckOut = new System.Windows.Forms.MenuItem();
            this.mi_ShowHistory = new System.Windows.Forms.MenuItem();
            this.mi_Persist2VSS = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mi_OpenSourceFile = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // imglDataObjects
            // 
            this.imglDataObjects.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglDataObjects.ImageStream")));
            this.imglDataObjects.TransparentColor = System.Drawing.Color.Black;
            this.imglDataObjects.Images.SetKeyName(0, "");
            this.imglDataObjects.Images.SetKeyName(1, "");
            this.imglDataObjects.Images.SetKeyName(2, "");
            this.imglDataObjects.Images.SetKeyName(3, "");
            this.imglDataObjects.Images.SetKeyName(4, "");
            this.imglDataObjects.Images.SetKeyName(5, "");
            this.imglDataObjects.Images.SetKeyName(6, "");
            this.imglDataObjects.Images.SetKeyName(7, "");
            this.imglDataObjects.Images.SetKeyName(8, "");
            this.imglDataObjects.Images.SetKeyName(9, "");
            this.imglDataObjects.Images.SetKeyName(10, "");
            this.imglDataObjects.Images.SetKeyName(11, "");
            this.imglDataObjects.Images.SetKeyName(12, "");
            this.imglDataObjects.Images.SetKeyName(13, "");
            this.imglDataObjects.Images.SetKeyName(14, "");
            this.imglDataObjects.Images.SetKeyName(15, "");
            this.imglDataObjects.Images.SetKeyName(16, "");
            this.imglDataObjects.Images.SetKeyName(17, "");
            this.imglDataObjects.Images.SetKeyName(18, "");
            this.imglDataObjects.Images.SetKeyName(19, "");
            this.imglDataObjects.Images.SetKeyName(20, "");
            this.imglDataObjects.Images.SetKeyName(21, "");
            this.imglDataObjects.Images.SetKeyName(22, "");
            this.imglDataObjects.Images.SetKeyName(23, "");
            this.imglDataObjects.Images.SetKeyName(24, "");
            this.imglDataObjects.Images.SetKeyName(25, "");
            this.imglDataObjects.Images.SetKeyName(26, "");
            this.imglDataObjects.Images.SetKeyName(27, "");
            this.imglDataObjects.Images.SetKeyName(28, "");
            this.imglDataObjects.Images.SetKeyName(29, "");
            this.imglDataObjects.Images.SetKeyName(30, "");
            // 
            // TvDBObjects
            // 
            this.TvDBObjects.BackColor = System.Drawing.Color.White;
            this.TvDBObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TvDBObjects.HideSelection = false;
            this.TvDBObjects.ImageIndex = 0;
            this.TvDBObjects.ImageList = this.imglDataObjects;
            this.TvDBObjects.ItemHeight = 16;
            this.TvDBObjects.Location = new System.Drawing.Point(0, 2);
            this.TvDBObjects.Name = "TvDBObjects";
            this.TvDBObjects.SelectedImageIndex = 0;
            this.TvDBObjects.Size = new System.Drawing.Size(221, 361);
            this.TvDBObjects.TabIndex = 0;
            this.TvDBObjects.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TvDBObjects_BeforeExpand);
            this.TvDBObjects.DoubleClick += new System.EventHandler(this.TvDBObjects_DoubleClick);
            this.TvDBObjects.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TvDBObjects_MouseUp);
            this.TvDBObjects.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TvDBObjects_ItemDrag);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miAddServer,
            this.miRemoveServer,
            this.mi_GetProcUsers,
            this.mi_ServerStats,
            this.miUseDatabase,
            this.miScript,
            this.miNew,
            this.miDelete,
            this.miRefreshDataObjects,
            this.miOpenScript,
            this.menuItem1,
            this.mi_TableInfo,
            this.menuItem6,
            this.mi_Dependencies});
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // miAddServer
            // 
            this.miAddServer.Index = 0;
            this.miAddServer.Text = "&Edit server connecctions";
            this.miAddServer.Click += new System.EventHandler(this.AddConnection_Click);
            // 
            // miRemoveServer
            // 
            this.miRemoveServer.Index = 1;
            this.miRemoveServer.Text = "&Remove server connection";
            // 
            // mi_GetProcUsers
            // 
            this.mi_GetProcUsers.Index = 2;
            this.mi_GetProcUsers.Text = "&Server info";
            this.mi_GetProcUsers.Click += new System.EventHandler(this.GetProcUsers_Click);
            // 
            // mi_ServerStats
            // 
            this.mi_ServerStats.Index = 3;
            this.mi_ServerStats.Text = "Ser&ver stats";
            this.mi_ServerStats.Click += new System.EventHandler(this.ServerStats_Click);
            // 
            // miUseDatabase
            // 
            this.miUseDatabase.Index = 4;
            this.miUseDatabase.Text = "&Use database";
            this.miUseDatabase.Click += new System.EventHandler(this.UseDatabase_Click);
            // 
            // miScript
            // 
            this.miScript.Index = 5;
            this.miScript.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miScriptCreate,
            this.miScriptAlter,
            this.miScriptDrop,
            this.miScriptExec,
            this.miScriptSelect,
            this.miScriptUpdate,
            this.miScriptInsert,
            this.miScriptDelete,
            this.menuItem2,
            this.miCreateInsertScript,
            this.miCreateUpdateScript});
            this.miScript.Text = "&Script";
            // 
            // miScriptCreate
            // 
            this.miScriptCreate.Index = 0;
            this.miScriptCreate.Text = "Create";
            this.miScriptCreate.Click += new System.EventHandler(this.ScriptCreate_Click);
            // 
            // miScriptAlter
            // 
            this.miScriptAlter.Index = 1;
            this.miScriptAlter.Text = "Alter";
            this.miScriptAlter.Click += new System.EventHandler(this.ScriptAlter_Click);
            // 
            // miScriptDrop
            // 
            this.miScriptDrop.Index = 2;
            this.miScriptDrop.Text = "Drop";
            this.miScriptDrop.Click += new System.EventHandler(this.ScriptDrop_Click);
            // 
            // miScriptExec
            // 
            this.miScriptExec.Index = 3;
            this.miScriptExec.Text = "Exec";
            this.miScriptExec.Click += new System.EventHandler(this.ScriptExec_Click);
            // 
            // miScriptSelect
            // 
            this.miScriptSelect.Index = 4;
            this.miScriptSelect.Text = "Select";
            // 
            // miScriptUpdate
            // 
            this.miScriptUpdate.Index = 5;
            this.miScriptUpdate.Text = "Update";
            // 
            // miScriptInsert
            // 
            this.miScriptInsert.Index = 6;
            this.miScriptInsert.Text = "Insert";
            // 
            // miScriptDelete
            // 
            this.miScriptDelete.Index = 7;
            this.miScriptDelete.Text = "Delete";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 8;
            this.menuItem2.Text = "-";
            // 
            // miCreateInsertScript
            // 
            this.miCreateInsertScript.Index = 9;
            this.miCreateInsertScript.Text = "Create insert script";
            this.miCreateInsertScript.Click += new System.EventHandler(this.miCreateInsertScript_Click);
            // 
            // miCreateUpdateScript
            // 
            this.miCreateUpdateScript.Index = 10;
            this.miCreateUpdateScript.Text = "Create update script";
            this.miCreateUpdateScript.Click += new System.EventHandler(this.miCreateUpdateScript_Click);
            // 
            // miNew
            // 
            this.miNew.Index = 6;
            this.miNew.Text = "&New";
            // 
            // miDelete
            // 
            this.miDelete.Index = 7;
            this.miDelete.Text = "&Delete";
            // 
            // miRefreshDataObjects
            // 
            this.miRefreshDataObjects.Index = 8;
            this.miRefreshDataObjects.Text = "Re&fresh";
            this.miRefreshDataObjects.Click += new System.EventHandler(this.RefreshDataObjects_Click);
            // 
            // miOpenScript
            // 
            this.miOpenScript.Index = 9;
            this.miOpenScript.Text = "Open do&cument";
            this.miOpenScript.Click += new System.EventHandler(this.miOpenScript_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 10;
            this.menuItem1.Text = "-";
            // 
            // mi_TableInfo
            // 
            this.mi_TableInfo.Index = 11;
            this.mi_TableInfo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_ShowConstraints,
            this.mi_ForeignKeys,
            this.mi_TablePermissions,
            this.mi_TableSpace,
            this.mi_TableStats,
            this.mi_UniqueRowColumns});
            this.mi_TableInfo.Text = "Info";
            // 
            // mi_ShowConstraints
            // 
            this.mi_ShowConstraints.Index = 0;
            this.mi_ShowConstraints.Text = "Table constraints";
            this.mi_ShowConstraints.Click += new System.EventHandler(this.mi_ShowConstraints_Click);
            // 
            // mi_ForeignKeys
            // 
            this.mi_ForeignKeys.Index = 1;
            this.mi_ForeignKeys.Text = "Table Foreign &Keys";
            this.mi_ForeignKeys.Click += new System.EventHandler(this.ForeignKeys_Click);
            // 
            // mi_TablePermissions
            // 
            this.mi_TablePermissions.Index = 2;
            this.mi_TablePermissions.Text = "&Table permissions";
            this.mi_TablePermissions.Click += new System.EventHandler(this.TablePermissions_Click);
            // 
            // mi_TableSpace
            // 
            this.mi_TableSpace.Index = 3;
            this.mi_TableSpace.Text = "Ta&ble space";
            this.mi_TableSpace.Click += new System.EventHandler(this.TableSpace_Click);
            // 
            // mi_TableStats
            // 
            this.mi_TableStats.Index = 4;
            this.mi_TableStats.Text = "&Table stats";
            this.mi_TableStats.Click += new System.EventHandler(this.TableStats_Click);
            // 
            // mi_UniqueRowColumns
            // 
            this.mi_UniqueRowColumns.Index = 5;
            this.mi_UniqueRowColumns.Text = "Unique Row Columns";
            this.mi_UniqueRowColumns.Click += new System.EventHandler(this.UniqueRowColumns_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 12;
            this.menuItem6.Text = "-";
            // 
            // mi_Dependencies
            // 
            this.mi_Dependencies.Index = 13;
            this.mi_Dependencies.Text = "Dependencies";
            this.mi_Dependencies.Click += new System.EventHandler(this.Dependencies_Click);
            // 
            // mi_CheckIn
            // 
            this.mi_CheckIn.Index = -1;
            this.mi_CheckIn.Text = "";
            // 
            // mi_CheckOut
            // 
            this.mi_CheckOut.Index = -1;
            this.mi_CheckOut.Text = "";
            // 
            // mi_AddToSourceControl
            // 
            this.mi_AddToSourceControl.Index = -1;
            this.mi_AddToSourceControl.Text = "";
            // 
            // mi_DetachFromSourceControl
            // 
            this.mi_DetachFromSourceControl.Index = -1;
            this.mi_DetachFromSourceControl.Text = "";
            // 
            // mi_VSSSettings
            // 
            this.mi_VSSSettings.Index = -1;
            this.mi_VSSSettings.Text = "";
            // 
            // mi_UndoCheckOut
            // 
            this.mi_UndoCheckOut.Index = -1;
            this.mi_UndoCheckOut.Text = "";
            // 
            // mi_ShowHistory
            // 
            this.mi_ShowHistory.Index = -1;
            this.mi_ShowHistory.Text = "";
            // 
            // mi_Persist2VSS
            // 
            this.mi_Persist2VSS.Index = -1;
            this.mi_Persist2VSS.Text = "";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = -1;
            this.menuItem7.Text = "";
            // 
            // mi_OpenSourceFile
            // 
            this.mi_OpenSourceFile.Index = -1;
            this.mi_OpenSourceFile.Text = "";
            // 
            // FrmDBObjects
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(221, 365);
            this.Controls.Add(this.TvDBObjects);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDBObjects";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Text = "Company List";
            this.ResumeLayout(false);

		}
		#endregion
		#endregion
		#region Events
		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			if ((QCTreeNode) TvDBObjects.SelectedNode != null)
			{
				((QCTreeNode) TvDBObjects.SelectedNode).RefreshImage(); 
			}
		}
		private void TvDBObjects_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				CurrentTreeNode = TvDBObjects.GetNodeAt(e.X,e.Y);
				if(CurrentTreeNode != null)
				{
					if( ((QCTreeNode)CurrentTreeNode).objecttype==QCTreeNode.ObjectType.WorkSpaceItem ||
						((QCTreeNode)CurrentTreeNode).objecttype==QCTreeNode.ObjectType.WorkSpace ||
						((QCTreeNode)CurrentTreeNode).objecttype==QCTreeNode.ObjectType.WorkSpaces)
					{
						SetContextMenu((QCTreeNode)CurrentTreeNode);
						return;
					}
					SetContextMenu(CurrentTreeNode);
					this.contextMenu1.Show(TvDBObjects,new Point(e.X,e.Y));
				}
			}
			// Fix Context Menu Bug
			if (TvDBObjects.HasChildren)
			{
				TvDBObjects.ExpandAll();
			}
		}
		private void TvDBObjects_DoubleClick(object sender, System.EventArgs e)
		{
			string		csQuery;
			IDatabaseManager db = null;
			MainForm frm =  (MainForm)MdiParentForm;
			string dbName = string.Empty;

			CurrentTreeNode = ((TreeView)sender).SelectedNode;
			if(CurrentTreeNode != null)
			{
				Cursor.Current = Cursors.WaitCursor;

				switch((int)DBTreeViewTypes[CurrentTreeNode])
				{
					case (int)QCTreeNode.ObjectType.Top:
						break;
                    case (int)QCTreeNode.ObjectType.Server:
						break;
					case (int)QCTreeNode.ObjectType.Database:
						break;
					case (int)QCTreeNode.ObjectType.StoredProcedure:
					{
						// Build the query for the selected table	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
				
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName,c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);

						csQuery = db.Commands.DropProcedure(CurrentTreeNode.Text, dbName);
						csQuery += db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
							CurrentTreeNode.Text,
							frm.ActiveQueryForm.dbConnection,
							DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.PROCEDURE);
						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text;
						frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						break;
					}
					case (int)QCTreeNode.ObjectType.Trigger:
					{
						// Build the query for the selected table	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
				
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);

						csQuery = db.Commands.DropTrigger(CurrentTreeNode.Text, dbName);
						csQuery += db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
							CurrentTreeNode.Text,
							frm.ActiveQueryForm.dbConnection,
							DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.TRIGGER);

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						break;
					}
					case (int)QCTreeNode.ObjectType.Function:
					{
						// Build the query for the selected table	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
				
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
							{
                                dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);

						csQuery = db.Commands.DropFunction(CurrentTreeNode.Text, dbName);
						csQuery += db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
							CurrentTreeNode.Text,
							frm.ActiveQueryForm.dbConnection,
							DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.PROCEDURE);

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						break;
					}
					case (int)QCTreeNode.ObjectType.Table:
					{
						// Build the query for the selected table	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
				
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
							{
                                dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}

						csQuery = "SELECT ";
						if ( CurrentTreeNode.Nodes.Count == 0 ) 
						{
							csQuery += " * ";
						}
						else
						if (CurrentTreeNode.Nodes[0].Nodes[0].Text == "...")
						{
							csQuery += " * ";
						}
						else
						{
							foreach(TreeNode oNode in CurrentTreeNode.Nodes[0].Nodes)
							{
								int ciNodeIndex = oNode.Text.IndexOf("(");
								string csNodeText = oNode.Text;
								if(ciNodeIndex > 0) csNodeText = csNodeText.Remove(ciNodeIndex, csNodeText.Length - ciNodeIndex);

								csQuery += " " + csNodeText + ",";
							}
						}						
						csQuery = csQuery.Substring(0,csQuery.Length-1);
						csQuery +=" FROM " + CurrentTreeNode.Text;

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.View:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName,c.Connection);
								break;
							}
						}

						csQuery = "SELECT * ";
						csQuery = csQuery.Substring(0,csQuery.Length-1);
						csQuery +=" FROM [" + CurrentTreeNode.Text + "]";

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.ScriptFile:
						break;
					case (int)QCTreeNode.ObjectType.Filed:
					{
						// Build the query for the selected table							
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
				
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent.Parent).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Parent.Parent.Text;
								frm.ActiveQueryForm.SetDatabaseConnection(dbName,c.Connection);
								break;
							}
						}
						string cFieldName = CurrentTreeNode.Text;
						int npos = cFieldName.IndexOf("(");
						if(npos >= 0) cFieldName = cFieldName.Remove(npos,cFieldName.Length - npos);
						csQuery = "SELECT ";
						csQuery += cFieldName;
						csQuery +=" FROM " + CurrentTreeNode.Parent.Parent.Text;

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.UDT:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent.Parent).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
						csQuery = db.Commands.DropUDT(((DBObjectAttribute)CurrentTreeNode.Tag).Name, dbName);
						csQuery += db.GetCreateUDTString( (DBObjectAttribute)CurrentTreeNode.Tag, dbName, frm.ActiveQueryForm.dbConnection);

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = "UDT_" + ((DBObjectAttribute)CurrentTreeNode.Tag).Name + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						//frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.Rule:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent.Parent).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
						csQuery = db.Commands.DropRule( CurrentTreeNode.Text, dbName );
						csQuery += db.GetCreateObjectString(((QCTreeNode)CurrentTreeNode).objectName, dbName, frm.ActiveQueryForm.dbConnection);

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text ="RULE_" + CurrentTreeNode.Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						//frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.Default:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent.Parent).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
						csQuery = db.Commands.DropDefault(((QCTreeNode)CurrentTreeNode).objectName.Split('=')[0].Trim(), dbName);
						csQuery += db.GetCreateObjectString(((QCTreeNode)CurrentTreeNode).objectName.Split('=')[0].Trim(), frm.ActiveQueryForm.DatabaseName, frm.ActiveQueryForm.dbConnection);

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = "DEFAULT_" + ((QCTreeNode)CurrentTreeNode).objectName.Split('=')[0].Trim() + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						//frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.Indexes:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent).server)
							{
								dbName = CurrentTreeNode.Parent.Parent.Parent.Text;
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
						csQuery = db.Commands.GetIndexesList_RunQuery(dbName, ((QCTreeNode)CurrentTreeNode.Parent).Text);
						//csQuery = string.Format("USE [{1}]\n\nGO\n\nEXEC sp_helpindex '{0}' \n\n", ((QCTreeNode)CurrentTreeNode.Parent).Text, dbName);

						frm.ActiveQueryForm.FileName = dbName + "_" + ((QCTreeNode)CurrentTreeNode.Parent).Text + "_indexes.sql";
						frm.ActiveQueryForm.Text = "INDEXES_" + ((QCTreeNode)CurrentTreeNode.Parent).Text + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.Job:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent).server)
							{
								dbName = "msdb";
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
						
						csQuery = db.Commands.GetJobStepInfo(CurrentTreeNode.Tag.ToString());

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = "JOB_" + ((QCTreeNode)CurrentTreeNode).objectName.Split(':')[0].Trim() + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						frm.ActiveQueryForm.RunQuery();
						break;
					}
					case (int)QCTreeNode.ObjectType.JobStep:
					{
						// Build the query for the selected view	
						frm.NewQueryform();
						foreach(MainForm.DBConnection c in frm.DBConnections)
						{
							if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode.Parent.Parent).server)
							{
								dbName = "msdb";
								frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
								break;
							}
						}
						db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
						
						csQuery = CurrentTreeNode.Tag.ToString();

						frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
						frm.ActiveQueryForm.Text = "JOB_STEP" + string.Format("{0}",(CurrentTreeNode.Index + 1)) + "_" + ((QCTreeNode)CurrentTreeNode).objectName.Split(':')[0].Trim() + "[" + dbName + "]";
						frm.ActiveQueryForm.Content = csQuery;
						break;
					}
					default:
						break;					
				}
				Cursor.Current = Cursors.Default;
			}
		}
		private void TvDBObjects_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
			MainForm.DBConnection	dbConnection = null;
			ArrayList DatabaseObjects;
			CurrentTreeNode = e.Node;

			if(CurrentTreeNode != null)
			{
				Cursor.Current = Cursors.WaitCursor;
				((QCTreeNode) CurrentTreeNode).RefreshImage(); 

				switch((int)DBTreeViewTypes[CurrentTreeNode])
				{
                    case (int)QCTreeNode.ObjectType.Top:
                        {
                            break;
                        }
                    case (int)QCTreeNode.ObjectType.Server:
                            ////////Raghu
                        if (CurrentTreeNode.FirstNode.Text == "...")
                        {
                            foreach (MainForm.DBConnection c in frm.DBConnections)
                            {
                                if (((QCTreeNode)CurrentTreeNode).objectName.IndexOf(c.ConnectionName) != -1)
                                {
                                    dbConnection = c;
                                    break;
                                }
                            }
                            if (dbConnection == null) { break; }
                            CurrentTreeNode.FirstNode.Remove();

                            // Delete this node and add all databases
                            IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
                            ArrayList allDatabases = new ArrayList();

                            ArrayList dbArr = db.GetDatabasesObjects(dbConnection.ConnectionName, dbConnection.Connection);
                            // Raghu Add database details ///
                            foreach (SQLEditor.Database.DB database in dbArr)
                            {
                                allDatabases.Add(database);
                            }
                            frm.AlterDatabaseMenuItem(allDatabases);
                            ///// Company Database [Start]
                            string strDbname, strConame, lcSqlstr;
                            int nCompId;
                            if (db == null) throw new Exception("Unable to connect to the Company Master...");

                            lcSqlstr = "Select CompId,Rtrim(Co_Name)+'  ('+LTrim(Str(Year(Sta_dt)))+'-'+ltrim(Str(Year(End_dt)))+')' as Co_name,Dbname From Co_mast ";
                            lcSqlstr = lcSqlstr + "Union Select 0 As CompId,'Udyog Database' as Co_Name,'VUDYOG' as Dbname";

                            DataSet dsCo_Mast = db.ExecuteCommand_DataSet(lcSqlstr, dbConnection.Connection, "Vudyog");
                            if (dsCo_Mast != null && dsCo_Mast.Tables.Count > 0)
                            {
                                DataView dv = dsCo_Mast.Tables[0].DefaultView;
                                dv.Sort = "Co_Name";
                                foreach (DataRowView drv in dv)
                                {
                                    strDbname = drv.Row["Dbname"].ToString().Trim();
                                    strConame = drv.Row["Co_Name"].ToString().Trim();
                                    nCompId = Convert.ToInt32(drv.Row["CompId"].ToString());
                                    QCTreeNode dbDatabase = new QCTreeNode(strConame, QCTreeNode.ObjectType.Database, null, dbConnection.ConnectionName, dbConnection.Connection, strDbname, strConame,nCompId);                                    DBTreeViewTypes.Add(dbDatabase, QCTreeNode.ObjectType.Database);
                                    QCTreeNode dbNode = new QCTreeNode("...", QCTreeNode.ObjectType.Empty, null, null, null, null, strConame,nCompId);
                                    dbDatabase.Nodes.Add(dbNode);
                                    CurrentTreeNode.Nodes.Add(dbDatabase);
                                }
                            }   ///// Company Database [End]
                        }
                        break;
					case (int)QCTreeNode.ObjectType.Database:
					{
						EnumDBObjects(false);
						break;
					}
					case (int)QCTreeNode.ObjectType.Tables:
						break;
					case (int)QCTreeNode.ObjectType.StoredProcedures:
					{
						break;
					}
					case (int)QCTreeNode.ObjectType.Functions:
						break;
					case (int)QCTreeNode.ObjectType.Table:
						break;
					case (int)QCTreeNode.ObjectType.View:
						break;
					case (int)QCTreeNode.ObjectType.StoredProcedure:
					{
						EnumParameters();
						break;
					}
					case (int)QCTreeNode.ObjectType.Function:
						break;
					case (int)QCTreeNode.ObjectType.ScriptFile:
						break;
					case (int)QCTreeNode.ObjectType.Fields:
					{
						if (CurrentTreeNode.FirstNode.Text == "...")
						{							
							foreach(MainForm.DBConnection c in frm.DBConnections)
							{
								if(((QCTreeNode)CurrentTreeNode.Parent.Parent.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
								{
									dbConnection = c;
									break;
								}
							}
							if (dbConnection == null)
								break;

							CurrentTreeNode.FirstNode.Remove();
							
							QCTreeNode oNode_Fields=null;
							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
							if (db == null) throw new Exception("Unable to connect to the server.");

							DatabaseObjects = db.GetDatabasesObjectProperties(((QCTreeNode)CurrentTreeNode.Parent.Parent.Parent).objectName,((QCTreeNode)CurrentTreeNode.Parent).objectName,dbConnection.Connection);
							if(DatabaseObjects == null)
								return;

							if(DatabaseObjects.Count>0)
							{
								foreach(Database.DBObjectProperties dbObjectProperties in DatabaseObjects)
								{
									oNode_Fields = new QCTreeNode(dbObjectProperties.Name, QCTreeNode.ObjectType.Filed,null,null, null, null,"",0);
									if(dbObjectProperties.IsPrimaryKey)
										oNode_Fields.NodeFont=new Font("Microsoft Sans Serif",
											8,
											System.Drawing.FontStyle.Bold);

									DBTreeViewTypes.Add(oNode_Fields,QCTreeNode.ObjectType.Filed);											
									CurrentTreeNode.Nodes.Add(oNode_Fields);
								}														
							}
						}
						break;
					}
					case (int)QCTreeNode.ObjectType.Indexes:
					{
						if (CurrentTreeNode.FirstNode.Text == "...")
						{							
							foreach(MainForm.DBConnection c in frm.DBConnections)
							{
								if(((QCTreeNode)CurrentTreeNode.Parent.Parent.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
								{
									dbConnection = c;
									break;
								}
							}
							if (dbConnection == null)
								break;

							CurrentTreeNode.FirstNode.Remove();
							
							QCTreeNode oNode_Indexes=null;
							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
							DatabaseObjects = db.GetDatabaseIndexes(((QCTreeNode)CurrentTreeNode).database,((QCTreeNode)CurrentTreeNode.Parent).objectName, dbConnection.Connection);
							if(DatabaseObjects == null)
								return;

							if(DatabaseObjects.Count > 0)
							{
								foreach(Database.DBObject dbObject in DatabaseObjects)
								{
									if(dbObject.Type.ToLower().IndexOf("primary key") > 0)
									{
										oNode_Indexes = new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.PK, null,null,dbConnection.Connection, null,"",0);
										DBTreeViewTypes.Add(oNode_Indexes, QCTreeNode.ObjectType.PK);											
										oNode_Indexes.NodeFont=new Font("Microsoft Sans Serif",
											8,
											System.Drawing.FontStyle.Bold);
										oNode_Indexes.ImageIndex = 29;
									}
									else
									{
										oNode_Indexes = new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.Index, null,null, dbConnection.Connection, null,"",0);
										DBTreeViewTypes.Add(oNode_Indexes, QCTreeNode.ObjectType.Index);											
									}
									CurrentTreeNode.Nodes.Add(oNode_Indexes);

									QCTreeNode indexInfo = new QCTreeNode(dbObject.Type, QCTreeNode.ObjectType.Unknown, null, null,dbConnection.Connection,  null,"",0);
									DBTreeViewTypes.Add(indexInfo, QCTreeNode.ObjectType.Unknown );
									oNode_Indexes.Nodes.Add(indexInfo );
								}														
							}
						}
						break;
					}
					case (int)QCTreeNode.ObjectType.Triggers:
						break;
					case (int)QCTreeNode.ObjectType.Trigger:
					{
						if (CurrentTreeNode.FirstNode.Text == "...")
						{							
							foreach(MainForm.DBConnection c in frm.DBConnections)
							{
								if(((QCTreeNode)CurrentTreeNode.Parent.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
								{
									dbConnection = c;
									break;
								}
							}
							if (dbConnection == null)
								break;
						
							CurrentTreeNode.FirstNode.Remove();
													
							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
							DatabaseObjects = db.GetDatabasesTriggers(((QCTreeNode)CurrentTreeNode).database,((QCTreeNode)CurrentTreeNode).objectName, dbConnection.Connection);
							if(DatabaseObjects == null)
								break;
						
							if(DatabaseObjects.Count>0)
							{
								foreach(Database.DBObject dbObject in DatabaseObjects)
								{
									QCTreeNode triggerInfo = new QCTreeNode(dbObject.Type, QCTreeNode.ObjectType.Unknown, null, null, dbConnection.Connection, null,"",0);
									DBTreeViewTypes.Add(triggerInfo, QCTreeNode.ObjectType.Unknown );
									CurrentTreeNode.Nodes.Add(triggerInfo );
								}														
							}
						}
						break;
					}
					case (int)QCTreeNode.ObjectType.Filed:
						break;
					case (int)QCTreeNode.ObjectType.UDTs:
					{
						if (CurrentTreeNode.FirstNode.Text == "...")
						{							
							foreach(MainForm.DBConnection c in frm.DBConnections)
							{
								if(((QCTreeNode)CurrentTreeNode.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
								{
									dbConnection = c;
									break;
								}
							}
							if (dbConnection == null)
								break;
						
							CurrentTreeNode.FirstNode.Remove();

							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
							DatabaseObjects = db.GetDataBaseUDTs( ((QCTreeNode)CurrentTreeNode.Parent).database, dbConnection.Connection );
							if (DatabaseObjects == null)
								break;

							foreach ( DBObjectAttribute dbObjectAttrib in DatabaseObjects)
							{
								string udt_default = dbObjectAttrib.default_id != "" ? ") D:" + dbObjectAttrib.default_id : ")";
								string udt_rule = dbObjectAttrib.rule_name!= "" ? ", R:" + dbObjectAttrib.rule_name : "";
								string objname = dbObjectAttrib.Name + "(" + dbObjectAttrib.Type + udt_default + udt_rule;
								QCTreeNode UDTNode = new QCTreeNode(objname, QCTreeNode.ObjectType.UDT, 
									null, 
									((QCTreeNode) CurrentTreeNode.Parent).server,
									dbConnection.Connection,
                                    ((QCTreeNode)CurrentTreeNode.Parent).database, ((QCTreeNode)CurrentTreeNode.Parent).Co_Name, ((QCTreeNode)CurrentTreeNode.Parent).CompId);
								UDTNode.Tag = dbObjectAttrib;
								DBTreeViewTypes.Add(UDTNode, QCTreeNode.ObjectType.UDT);
								CurrentTreeNode.Nodes.Add(UDTNode);
							}
						}

						break;
					}
					case (int)QCTreeNode.ObjectType.UDT:
						break;
					case (int)QCTreeNode.ObjectType.Defaults:
					{
						if (CurrentTreeNode.FirstNode.Text == "...")
						{							
							foreach(MainForm.DBConnection c in frm.DBConnections)
							{
								if(((QCTreeNode)CurrentTreeNode.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
								{
									dbConnection = c;
									break;
								}
							}
							if (dbConnection == null)
								break;
						
							CurrentTreeNode.FirstNode.Remove();

							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
							DatabaseObjects = db.GetDataBaseDefaults( ((QCTreeNode)CurrentTreeNode.Parent).database, dbConnection.Connection );
							if (DatabaseObjects == null)
								break;

							foreach ( DBObject dbObject in DatabaseObjects )
							{
								string [] words = dbObject.Type.Split(' ');
								string val = words[words.Length - 1].Trim().IndexOf("'") == words[words.Length - 1].Trim().Length - 1 ? "' " + words[words.Length - 1].Trim() : words[words.Length - 1].Trim();
								string def_val = dbObject.Type.Trim().Length > 0 ? " = " + val : "";
								QCTreeNode defNode = new QCTreeNode(dbObject.Name + def_val, QCTreeNode.ObjectType.Default,
									null, 
									((QCTreeNode) CurrentTreeNode.Parent).server,
									dbConnection.Connection,
                                    ((QCTreeNode)CurrentTreeNode.Parent).database, ((QCTreeNode)CurrentTreeNode.Parent).Co_Name, ((QCTreeNode)CurrentTreeNode.Parent).CompId); 

								DBTreeViewTypes.Add(defNode, QCTreeNode.ObjectType.Default);
								CurrentTreeNode.Nodes.Add(defNode);
							}
						}
						break;
					}
					case (int)QCTreeNode.ObjectType.Default:
						break;
					case (int)QCTreeNode.ObjectType.Rules:
					{
						if (CurrentTreeNode.FirstNode.Text == "...")
						{							
							foreach(MainForm.DBConnection c in frm.DBConnections)
							{
								if(((QCTreeNode)CurrentTreeNode.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
								{
									dbConnection = c;
									break;
								}
							}
							if (dbConnection == null)
								break;
						
							CurrentTreeNode.FirstNode.Remove();

							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
							DatabaseObjects = db.GetDataBaseRules( ((QCTreeNode)CurrentTreeNode.Parent).database, dbConnection.Connection );
							if (DatabaseObjects == null)
								break;

							foreach ( DBObject dbObject in DatabaseObjects )
							{
								QCTreeNode ruleNode = new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.Rule, 
									null, 
									((QCTreeNode) CurrentTreeNode.Parent).server, 
									dbConnection.Connection,
                                    ((QCTreeNode)CurrentTreeNode.Parent).database, ((QCTreeNode)CurrentTreeNode.Parent).Co_Name, ((QCTreeNode)CurrentTreeNode.Parent).CompId);
								DBTreeViewTypes.Add(ruleNode, QCTreeNode.ObjectType.Rule);
								CurrentTreeNode.Nodes.Add(ruleNode);
							}
						}

						break;
					}
					case (int)QCTreeNode.ObjectType.Rule:
						break;
					default:
						break;					
				}
				Cursor.Current = Cursors.Default;
			}
		}
		private void TvDBObjects_ItemDrag(object sender, ItemDragEventArgs e)
		{
			QCTreeNode node = (QCTreeNode)e.Item;
			if(node.objecttype == QCTreeNode.ObjectType.Table
				|| node.objecttype == QCTreeNode.ObjectType.View
				|| node.objecttype == QCTreeNode.ObjectType.Filed)
			{
				DoDragDrop(node,System.Windows.Forms.DragDropEffects.Copy);
			}
		}
		#endregion
		#region Context menu
		private void AddConnection_Click(object sender, System.EventArgs e)
		{
			FrmDBConnections frm = new FrmDBConnections((MainForm)this.MdiParentForm);
			frm.ShowDialogWindow(this);
			RefreashTreeView(null);
		}
		private void UseDatabase_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
            if (frm.ActiveQueryForm == null) { return; }
            foreach (MainForm.DBConnection c in frm.DBConnections)
            {
                string connectionName = CurrentTreeNode.Parent.Text;

                if (CurrentTreeNode.Parent.Text.IndexOf(" [") > 0)
                    connectionName = CurrentTreeNode.Parent.Text.Substring(0, CurrentTreeNode.Parent.Text.IndexOf(" ["));

                if (c.ConnectionName == connectionName)
                {
                    frm.ActiveQueryForm.SetDatabaseConnection(CurrentTreeNode.Tag.ToString().ToUpper(), c.Connection);
                    break;
                }
            }			
		}

		private void ScriptCreate_Click(object sender, System.EventArgs e)
		{
            /// Create script method ///
            /// 

			Cursor.Current = Cursors.WaitCursor;

			string CreateScript = string.Empty;
			string dbName = string.Empty;
			MainForm frm =  (MainForm)MdiParentForm;
			
			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{
				
				if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
				{
					dbName = ((QCTreeNode)CurrentTreeNode).database; //CurrentTreeNode.Parent.Parent.Text;
                    if (dbName == null)
                    {
                        dbName = "Vudyog";
                    }
					frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
					break;
				}
			}
            
			///string FormName = CurrentTreeNode.Text + "[" + dbName + "]";
            string FormName = CurrentTreeNode.Text+" ["+frm.ActiveQueryForm.company.Co_Name+" ]";
			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
			if (db == null) throw new Exception("Unable to connect to the server.");

			// CurrentTreeNode.Parent.Text
			switch(CurrentTreeNode.Parent.Text.ToUpper())
			{
				case "TABLES":
				{
					CreateScript = db.GetObjectConstructorString(dbName,
						CurrentTreeNode.Text,
						frm.ActiveQueryForm.dbConnection,
						DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.TABLE);
					break;
				}
				case "TRIGGERS":
				{
					CreateScript = db.GetObjectConstructorString(dbName,
						CurrentTreeNode.Text,
						frm.ActiveQueryForm.dbConnection,
						DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.TRIGGER);
					break;
				}
				case "RULES":
				{
					CreateScript = db.GetObjectConstructorString(dbName,
						CurrentTreeNode.Text,
						frm.ActiveQueryForm.dbConnection,
						DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.RULE);
					FormName = "RULE_" + CurrentTreeNode.Text + "[" + dbName + "]";
					break;
				}
				case "DEFAULTS":
				{
					CreateScript = db.GetObjectConstructorString(dbName,
						CurrentTreeNode.Text.Split('=')[0].Trim(),
						frm.ActiveQueryForm.dbConnection,
						DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.DEFAULT);
					FormName = "DEFAULT_" + CurrentTreeNode.Text.Split('=')[0].Trim() + "[" + dbName + "]";
					break;
				}
				case "UDTS":
				{
					CreateScript = db.GetCreateUDTString( (DBObjectAttribute)CurrentTreeNode.Tag, 
						dbName, frm.ActiveQueryForm.dbConnection);
					FormName = "UDT_" + ((DBObjectAttribute)CurrentTreeNode.Tag).Name + "[" + dbName + "]";
					break;
				}
				case "JOBS":
				{
					CreateScript = db.GetCreateJobString(CurrentTreeNode.Tag.ToString(), dbName, frm.ActiveQueryForm.dbConnection);
					string Server = string.Empty;
					DataSet ds = db.ExecuteCommand_DataSet( db.Commands.GetJobInfo( CurrentTreeNode.Tag.ToString() ), 
						frm.ActiveQueryForm.dbConnection, dbName);
					if (ds != null && ds.Tables.Count > 0)
					{
						Server = ds.Tables[0].Rows[0]["originating_server"].ToString();
					}
					FormName = "JOB_" + CurrentTreeNode.Tag.ToString() + "[" + Server + "]";
					break;
				}
				default:
				{
					CreateScript = db.GetObjectConstructorString(dbName,
						CurrentTreeNode.Text,
						frm.ActiveQueryForm.dbConnection,
						DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.PROCEDURE);
					break;
				}
			}

			switch(CurrentTreeNode.Text.ToUpper())
			{
				case "UDTS":
				{
					ArrayList DatabaseObjects = db.GetDataBaseUDTs( dbName, frm.ActiveQueryForm.dbConnection );
					if (DatabaseObjects == null)
						break;

					CreateScript = string.Format("use [{0}]\n\n", dbName);

					foreach ( DBObjectAttribute dbObjectAttrib in DatabaseObjects)
					{
						CreateScript += db.Commands.DropUDT_NoUse( dbObjectAttrib.Name );
						CreateScript += db.GetCreateUDTString( dbObjectAttrib, 
							dbName, frm.ActiveQueryForm.dbConnection) + "\n\n";
					}
					break;
				}
				case "RULES":
				{
					ArrayList DatabaseObjects = db.GetDataBaseRules( dbName, frm.ActiveQueryForm.dbConnection );
					if (DatabaseObjects == null)
						break;
					CreateScript = string.Format("use [{0}]\n\n", dbName);

					foreach ( DBObject dbObject in DatabaseObjects )
					{
						CreateScript += db.Commands.DropRule_NoUse( dbObject.Name );
						CreateScript += 
							db.GetCreateObjectString(dbObject.Name, dbName, frm.ActiveQueryForm.dbConnection);
						CreateScript += "\nGO\n\n"; // added go in between each rule create statement to avoid error when running
					}
					break;
				}
				case "DEFAULTS":
				{
					ArrayList DatabaseObjects = db.GetDataBaseDefaults( dbName, frm.ActiveQueryForm.dbConnection );
					if (DatabaseObjects == null)
						break;
					CreateScript  += string.Format("use [{0}]\n\n", dbName);
					foreach ( DBObject dbObject in DatabaseObjects )
					{
						CreateScript += db.Commands.DropDefault_NoUse( dbObject.Name );
						CreateScript += 
							db.GetCreateObjectString(dbObject.Name, dbName, frm.ActiveQueryForm.dbConnection);
						CreateScript += "\nGO\n\n"; // added go in between each default create statement to avoid error when running
					}
					break;
				}
				default:
				{
					break;
				}
			}

			frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
			frm.ActiveQueryForm.Text = FormName;
			if(CreateScript.IndexOf("</member>",0)>0)
			{
				frm.ActiveQueryForm.Content = AddRevisionCommentSection(CreateScript);
			}
			else
			{
				frm.ActiveQueryForm.Content = CreateScript;
			}

			Cursor.Current = Cursors.Default;
		}

		private void ScriptAlter_Click(object sender, System.EventArgs e)
		{
			string dbName = string.Empty;
			Cursor.Current = Cursors.WaitCursor;

			string type;
			MainForm frm =  (MainForm)MdiParentForm;
			
			if( ((QCTreeNode)CurrentTreeNode).objecttype == QCTreeNode.ObjectType.Table)
			{
				ScriptCreate_Click(sender,e);
				return;
			}

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length>0)
				frm.NewQueryform();

			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
			if (db == null) throw new Exception("Unable to connect to the server.");

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{
				
				if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
				{
					dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
					frm.ActiveQueryForm.SetDatabaseConnection(dbName,c.Connection);
					break;
				}
			}

			string CreateScript = db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
				CurrentTreeNode.Text,
				frm.ActiveQueryForm.dbConnection,
				DBCommon.ScriptType.CREATE, DBCommon.ScriptObjectType.PROCEDURE);


			if(CurrentTreeNode.Parent.Text == "Stored procedures")
				type = "PROCEDURE";
			else
				type = "FUNCTION";

			frm.ActiveQueryForm.Content = Create2Alter(CreateScript,type);
			int startpos = CreateScript.IndexOf("</member>",0);
			if(startpos>0)
				frm.ActiveQueryForm.AddRevisionCommentSection();
			
			//Set database connection
			foreach(MainForm.DBConnection c in frm.DBConnections)
			{
				string databaseConnection  = CurrentTreeNode.Parent.Parent.Parent.Text;
				if(c.ConnectionName == databaseConnection)
				{
                    dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
                    frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
					break;
				}
			}
			frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
			frm.ActiveQueryForm.Text = CurrentTreeNode.Text + "[" + dbName + "]";
			
			Cursor.Current = Cursors.Default;
		}
		private void ScriptExec_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			MainForm frm =  (MainForm)MdiParentForm;

			string FormName = string.Empty;
			string exec_command = string.Empty;
			string dbName = string.Empty;

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{
				
				if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
				{
					dbName = ((QCTreeNode)CurrentTreeNode).database;
					if (dbName  == null)
						dbName = "master";
					frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
					break;
				}
			}

			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
			if (db == null) throw new Exception("Unable to connect to the server.");

			switch ( (int) ((QCTreeNode)CurrentTreeNode).objecttype )
			{
				case (int)QCTreeNode.ObjectType.Job:
				{
					FormName = "Exec_Job_" + CurrentTreeNode.Tag.ToString() + "[" + ((QCTreeNode)CurrentTreeNode).server + "]";
					exec_command = db.Commands.StartJob( CurrentTreeNode.Tag.ToString() );
					break;
				}
				case (int)QCTreeNode.ObjectType.StoredProcedure:
				{
					// TODO: add exec string for stored procs and functions
					FormName = "Exec_" + CurrentTreeNode.Text + "[" + dbName + "]";

					// get the parameters  
					EnumParameters();
					
					DataTable dtUDT = db.ExecuteCommand(db.Commands.GetUDTs(dbName), frm.ActiveQueryForm.dbConnection, dbName);

					// loop thru all the parameters
					foreach (TreeNode n in CurrentTreeNode.Nodes)
					{
						if (n.Tag != null && n.Tag.GetType() == typeof(DataRow))
						{
							DataRow dr = (DataRow)n.Tag;
							bool isUDT = false;
							if (dtUDT != null && dtUDT.Rows.Count > 0)
							{
								foreach (DataRow dr1 in dtUDT.Rows)
								{
									if (dr["TYPE_NAME"].ToString().ToLower() == dr1["name"].ToString().ToLower())
									{
										isUDT = true;
										break;
									}
								}
							}
							bool bInt = dr["TYPE_NAME"].ToString().ToLower().IndexOf("int") >= 0 ;
							bool bChar = dr["TYPE_NAME"].ToString().ToLower().IndexOf("char") >= 0 ; 
							bool bDateTime = dr["TYPE_NAME"].ToString().ToLower().IndexOf("date") >= 0 ; 
							bool bMoney = dr["TYPE_NAME"].ToString().ToLower().IndexOf("money") >= 0 ; 
							bool bScale = dr["SCALE"].ToString() == "0" ;
							string scale = bScale || bChar ? "" : "," + dr["SCALE"].ToString();
							string precision = "(" + dr["PRECISION"].ToString() + scale + ")";
							precision = bInt || bDateTime || bMoney || isUDT ? "" : precision;
							exec_command += "DECLARE " + dr["COLUMN_NAME"].ToString() + " " + dr["TYPE_NAME"].ToString() + precision + "\n";
						}
					}

					exec_command += "\nEXEC [" + dbName + "].[dbo].[" + CurrentTreeNode.Text + "]";

					foreach (TreeNode n in CurrentTreeNode.Nodes)
					{
						if (n.Tag != null && n.Tag.GetType() == typeof(DataRow))
						{
							DataRow dr = (DataRow)n.Tag;
							if ( dr["COLUMN_NAME"].ToString().IndexOf("RETURN_VALUE") < 0 )
							{
								string output = ( dr["COLUMN_TYPE"].ToString() == "4" || dr["COLUMN_TYPE"].ToString() == "2" ? " OUTPUT" : "" );
								exec_command += " " + dr["COLUMN_NAME"].ToString() + output + ",";
							}
							else
							{
								exec_command = exec_command.Insert( exec_command.IndexOf("[" + dbName), dr["COLUMN_NAME"].ToString() + " = " );
							}
						}
					}
					exec_command = exec_command.TrimEnd(',');
					break;
				}
			}

			frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
			frm.ActiveQueryForm.Text = FormName;
			if(exec_command.IndexOf("</member>",0)>0)
			{
				frm.ActiveQueryForm.Content = AddRevisionCommentSection(exec_command);
			}
			else
			{
				frm.ActiveQueryForm.Content = exec_command;
			}
			//frm.ActiveQueryForm.RunQuery();
			Cursor.Current = Cursors.Default;
		}
		private void miScriptDelete_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			MainForm frm =  (MainForm)MdiParentForm;

			string FormName = string.Empty;
			string exec_command = string.Empty;
			string dbName = string.Empty;

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{
				
				if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
				{
					dbName = ((QCTreeNode)CurrentTreeNode).database;
					if (dbName  == null)
						dbName = "master";
					frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
					break;
				}
			}

			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
			if (db == null) throw new Exception("Unable to connect to the server.");

			switch ( (int) ((QCTreeNode)CurrentTreeNode).objecttype )
			{
				case (int)QCTreeNode.ObjectType.Job:
				{
					FormName = "Delete_Job_" + CurrentTreeNode.Tag.ToString() + "[" + ((QCTreeNode)CurrentTreeNode).server + "]";
					exec_command = db.Commands.DeleteJob( CurrentTreeNode.Tag.ToString() );
					break;
				}
			}

			frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
			frm.ActiveQueryForm.Text = FormName;
			if(exec_command.IndexOf("</member>",0)>0)
			{
				frm.ActiveQueryForm.Content = AddRevisionCommentSection(exec_command);
			}
			else
			{
				frm.ActiveQueryForm.Content = exec_command;
			}
			//frm.ActiveQueryForm.RunQuery();
			Cursor.Current = Cursors.Default;
		}
		private void ScriptDrop_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			MainForm frm =  (MainForm)MdiParentForm;

			string FormName = string.Empty;
			string exec_command = string.Empty;
			string dbName = string.Empty;

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{
				
				if(c.ConnectionName == ((QCTreeNode)CurrentTreeNode).server)
				{
					dbName = ((QCTreeNode)CurrentTreeNode).database;
					if (dbName  == null)
						dbName = "master";
					frm.ActiveQueryForm.SetDatabaseConnection(dbName, c.Connection);
					break;
				}
			}

			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
			if (db == null) throw new Exception("Unable to connect to the server.");

			switch ( (int) ((QCTreeNode)CurrentTreeNode).objecttype )
			{
				case (int)QCTreeNode.ObjectType.StoredProcedure:
				{
					FormName = "Drop_" + CurrentTreeNode.Text + "[" + dbName + "]";
					exec_command = db.Commands.DropProcedure(CurrentTreeNode.Text, dbName);
					break;
				}
				case (int)QCTreeNode.ObjectType.Function:
				{
					FormName = "Drop_" + CurrentTreeNode.Text + "[" + dbName + "]";
					exec_command = db.Commands.DropFunction(CurrentTreeNode.Text, dbName);
					break;
				}
				case (int)QCTreeNode.ObjectType.Rule:
				{
					FormName = "Drop_" + CurrentTreeNode.Text + "[" + dbName + "]";
					exec_command = db.Commands.DropRule(CurrentTreeNode.Text, dbName);
					break;
				}
				case (int)QCTreeNode.ObjectType.Default:
				{
					FormName = "Drop_" + CurrentTreeNode.Text + "[" + dbName + "]";
					exec_command = db.Commands.DropDefault(((QCTreeNode)CurrentTreeNode).objectName.Split('=')[0].Trim(), dbName);
					break;
				}
				case (int)QCTreeNode.ObjectType.UDT:
				{
					FormName = "Drop_" + CurrentTreeNode.Text + "[" + dbName + "]";
					exec_command = db.Commands.DropUDT(((DBObjectAttribute)CurrentTreeNode.Tag).Name, dbName);
					break;
				}
				case (int)QCTreeNode.ObjectType.Trigger:
				{
					FormName = "Drop_" + CurrentTreeNode.Text + "[" + dbName + "]";
					exec_command = db.Commands.DropTrigger(CurrentTreeNode.Text, dbName);
					break;
				}
			}

			frm.ActiveQueryForm.FileName = dbName + "_" + CurrentTreeNode.Text + ".sql";
			frm.ActiveQueryForm.Text = FormName;
			if(exec_command.IndexOf("</member>",0)>0)
			{
				frm.ActiveQueryForm.Content = AddRevisionCommentSection(exec_command);
			}
			else
			{
				frm.ActiveQueryForm.Content = exec_command;
			}
			//frm.ActiveQueryForm.RunQuery();
			Cursor.Current = Cursors.Default;
		}
		private void miCreateInsertScript_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			string dbName = string.Empty;
			MainForm frm =  (MainForm)MdiParentForm;
			
			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{	
				if(((QCTreeNode)CurrentTreeNode.Parent.Parent).server == c.ConnectionName)
				{
					dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
					frm.ActiveQueryForm.SetDatabaseConnection(dbName,c.Connection);
					break;
				}
			}
			frm.ActiveQueryForm.CreateInsertStatement("SELECT * FROM " + CurrentTreeNode.Text);
			Cursor.Current = Cursors.Default;
		}
		private void miCreateUpdateScript_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			string dbName = string.Empty;
			MainForm frm =  (MainForm)MdiParentForm;
			
			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();
		
			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);
			if (db == null) throw new Exception("Unable to connect to the server.");

			foreach(MainForm.DBConnection c in frm.DBConnections)
			{	
				if(((QCTreeNode)CurrentTreeNode.Parent.Parent).server == c.ConnectionName)
				{
					dbName = CurrentTreeNode.Parent.Parent.Tag.ToString().ToUpper();
					frm.ActiveQueryForm.SetDatabaseConnection(dbName,c.Connection);
					break;
				}
			}

			frm.ActiveQueryForm.CreateUpdateStatement("SELECT * FROM " + CurrentTreeNode.Text);

			Cursor.Current = Cursors.Default;
		}
		private void RefreshDataObjects_Click(object sender, System.EventArgs e)
		{
			RefreashTreeView(null);
		}

		private void miOpenScript_Click(object sender, System.EventArgs e)
		{
			MainForm frm = (MainForm)MdiParentForm;
			string database = frm.ActiveQueryForm.DatabaseName;
			IDbConnection sqlConnection = frm.ActiveQueryForm.dbConnection;

			string fileName = ((QCTreeNode) CurrentTreeNode).database;
		
			StreamReader sr = new StreamReader(fileName);
			string content = "";
			content = sr.ReadToEnd();
			sr.Close();
			sr = null;

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			frm.ActiveQueryForm.SetDatabaseConnection(database,sqlConnection);
			frm.ActiveQueryForm.Content = content;
			frm.ActiveQueryForm.Text = ((QCTreeNode) CurrentTreeNode).Text;
			frm.ActiveQueryForm.FileName = fileName;
		}


		private void mi_ShowConstraints_Click(object sender, EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void UniqueRowColumns_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void TablePermissions_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void TableStats_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void TableSpace_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void RefreshView_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void Statistics_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode.Parent.Parent).server, ((QCTreeNode)CurrentTreeNode.Parent.Parent).database, CurrentTreeNode.Text);
		}

		private void Alerts_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, "msdb", CurrentTreeNode.Text);
		}

		private void ServerStats_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, "master", CurrentTreeNode.Text);
		}

		private void UpdateStats_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void Locks_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, "master", CurrentTreeNode.Text);
		}

		private void LinkedServers_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, "msdb", CurrentTreeNode.Text);
		}

		private void GetProcUsers_Click(object sender, EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, "master", CurrentTreeNode.Text);
		}

		private void DBProperties_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		private void Dependencies_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
            
		}
        
		private void ForeignKeys_Click(object sender, System.EventArgs e)
		{
			PerformMenuItemAction(sender, ((QCTreeNode)CurrentTreeNode).server, ((QCTreeNode)CurrentTreeNode).database, CurrentTreeNode.Text);
		}

		#endregion
		#region Enums
		enum MenuItemsTypes
		{
			None					=0,
			AddServerVisible		=1,
			RefreshDataObjects		=2,
			RemoveServerVisible		=4,
			UseDatabaseVisible		=8,
			NewVisible				=16,
			DeleteVisible			=32,
			ScriptAlterVisible		=64,
			ScriptCreateVisible		=128,
			ScriptDeleteVisible		=256,
			ScriptDropVisible		=512,
			ScriptInsertVisible		=1024,
			ScriptSelectVisible		=2048,
			ScriptUpdateVisible		=4096,
			CreateInsertScript		=8192,
			CreateUpdateScript		=16384,
			OpenScriptFile			=32768,
			ShowStatistics			=65536,
			ScriptExecVisible		=131072,
			GetDepends				=262144,
			TableInfo				=524288,
			ServerInfo				=1048576,
			SystemTableInfo			=2097152
		}
		
		#endregion
		#region Methods
		private void PerformMenuItemAction(object sender, string server, string dbName, string SelectedObjectName)
		{			
			Cursor.Current  = Cursors.WaitCursor;
			MainForm frm    = (MainForm)MdiParentForm;
			
			//if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length > 0)
				frm.NewQueryform();

			
			foreach ( MainForm.DBConnection c in frm.DBConnections )
			{	
				if ( server == c.ConnectionName )
				{
					frm.ActiveQueryForm.SetDatabaseConnection( dbName, c.Connection );
					break;
				}
			}

			string frmTitle = SelectedObjectName + "[" + dbName + "]";
			IDatabaseManager db = DatabaseFactory.CreateNew( frm.ActiveQueryForm.dbConnection );
			if (db == null) throw new Exception( "Unable to connect to the server." );


			//Only supports MS
			if(!(frm.ActiveQueryForm.dbConnection is SqlConnection))
			{
				MessageBox.Show("This feature is not supported for "+frm.ActiveQueryForm.dbConnection.ToString()+".");
				return;
			}

			if ( ((MenuItem)sender) == this.mi_UniqueRowColumns )
			{
				frmTitle = "UNIQUE_ROW_COLUMNS_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetUniqueRowColumns(SelectedObjectName, dbName);
			}

            ////if ( ((MenuItem)sender) == this.mi_DBProperties )
            ////{
            ////    frmTitle = "PROPERTIES_" + frmTitle;
            ////    frm.ActiveQueryForm.Content = db.Commands.GetDBProperties(dbName);
            ////}

			if ( ((MenuItem)sender) == this.mi_ShowConstraints )
			{
				frmTitle = "CONSTRAINTS_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetConstraints(SelectedObjectName, dbName);
			}

			if ( ((MenuItem)sender) == this.mi_TablePermissions )
			{
				frmTitle = "PERMISSIONS_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetTablePermissions(SelectedObjectName, dbName);
			}

			if ( ((MenuItem)sender) == this.mi_TableStats )
			{
				frmTitle = "STATISTICS_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetTableStatistics(SelectedObjectName, dbName);
			}

			if ( ((MenuItem)sender) == this.mi_TableSpace )
			{
				frmTitle = "SPACE_USED_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetTableSpace(SelectedObjectName, dbName);
			}

            ////if ( ((MenuItem)sender) == this.mi_RefreshView )
            ////{
            ////    frmTitle = "REFRESH_VIEW_" + frmTitle;
            ////    frm.ActiveQueryForm.Content = db.Commands.RefreshView(SelectedObjectName, dbName);
            ////}

            ////if ( ((MenuItem)sender) == this.mnuStatistics )
            ////{
            ////    frmTitle = "INDEX_STATISTICS_" + frmTitle;
            ////    frm.ActiveQueryForm.Content = db.Commands.GetTableIndexStatistics(CurrentTreeNode.Parent.Parent.Text, SelectedObjectName);
            ////}

            //////if ( ((MenuItem)sender) == this.mi_Alerts )
            //////{
            //////    frmTitle = "ALERTS_" + frmTitle;
            //////    frm.ActiveQueryForm.Content = db.Commands.GetAlerts();
            //////}

			if ( ((MenuItem)sender) == this.mi_ServerStats )
			{
				frmTitle = "SERVER_STATS_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetStatistics();
			}

            ////////if ( ((MenuItem)sender) == this.mi_UpdateStats )
            ////////{
            ////////    frmTitle = "UPDATE_STATS_" + frmTitle;
            ////////    frm.ActiveQueryForm.Content = db.Commands.UpdateDBStatistics(dbName);
            ////////}

            //if ( ((MenuItem)sender) == this.mi_Locks )
            //{
            //    frmTitle = "LOCKS_" + frmTitle;
            //    frm.ActiveQueryForm.Content = db.Commands.GetLocks();
            //}

            ////if ( ((MenuItem)sender) == this.mi_LinkedServers )
            ////{
            ////    frmTitle = "LINKED_SERVERS_" + frmTitle;
            ////    frm.ActiveQueryForm.Content = db.Commands.GetLinkedServers();
            ////}

			if ( ((MenuItem)sender) == this.mi_GetProcUsers )
			{
				frmTitle = "SERVER_INFO_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetWho();
			}

			if ( ((MenuItem)sender) == this.mi_Dependencies )
			{
				frmTitle = "DEPENDENCIES_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetDepends(SelectedObjectName, dbName);
			}

			if ( ((MenuItem)sender) == this.mi_ForeignKeys )
			{
				frmTitle = "FOREIGN_KEYS_" + frmTitle;
				frm.ActiveQueryForm.Content = db.Commands.GetForeignKeys(SelectedObjectName, dbName);
			}

			frm.ActiveQueryForm.Text = frmTitle;
			if (frm.ActiveQueryForm.Content.Length > 0)
			{
				frm.ActiveQueryForm.RunQuery();
			}
			Cursor.Current = Cursors.Default;
		}
		private void EnumParameters()
		{
			MainForm				frm			 =  (MainForm)MdiParentForm;
			MainForm.DBConnection	dbConnection = null;
			//ArrayList  DatabaseObjects;

			Cursor.Current = Cursors.WaitCursor;
			if (CurrentTreeNode.FirstNode.Text == "...")
			{							
				CurrentTreeNode.FirstNode.Remove();
							
				foreach(MainForm.DBConnection c in frm.DBConnections)
				{
					if(((QCTreeNode)CurrentTreeNode.Parent.Parent.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
					{
						dbConnection = c;
						break;
					}
				}
				if (dbConnection == null)
					return;

				QCTreeNode oNode_Param = null;
				string dbName = ((QCTreeNode)CurrentTreeNode.Parent.Parent).database;
				string server = ((QCTreeNode)CurrentTreeNode.Parent.Parent).server;

				IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
				if (db == null) throw new Exception("Unable to connect to the server.");

				// Uses the "EXEC sp_sproc_columns 'sproc name'"  it indicates the output params

				DataTable dtSprocParams = db.ExecuteCommand( db.Commands.GetSprocParams(((QCTreeNode)CurrentTreeNode).objectName, dbName), dbConnection.Connection, dbName );
				if(dtSprocParams != null && dtSprocParams.Rows.Count > 0)
				{
					foreach(DataRow dr in dtSprocParams.Rows)
					{
						string output = (dr["COLUMN_TYPE"].ToString() == "4" ? ", output": (dr["COLUMN_TYPE"].ToString() == "2" ? ", output":""));
						oNode_Param = new QCTreeNode( dr["COLUMN_NAME"].ToString() + " (" + dr["TYPE_NAME"].ToString() + output + ")", QCTreeNode.ObjectType.Unknown, null, server, dbConnection.Connection, dbName,"",0);
						if (oNode_Param.Text.ToLower().IndexOf("output") > 0)
						{
							oNode_Param.NodeFont=new Font("Microsoft Sans Serif",
								8,
								System.Drawing.FontStyle.Bold);
						}
						oNode_Param.Tag = dr;
						DBTreeViewTypes.Add(oNode_Param, QCTreeNode.ObjectType.Unknown);											
						CurrentTreeNode.Nodes.Add(oNode_Param);
					}														
				}
			}
			Cursor.Current = Cursors.Default;
		}

		private void EnumDBObjects(bool addtoVss)
		{
            // Add Database Details [Start]  --- Raghu [15-10-2008]
			MainForm frm =  (MainForm)MdiParentForm;
			MainForm.DBConnection dbConnection = null;
			Database.DB	serverDB = null;
            Cursor.Current = Cursors.WaitCursor;
            string cDdname;

			if (CurrentTreeNode.FirstNode.Text == "..." || (CurrentTreeNode.FirstNode.Text.ToUpper() == "TABLES" && addtoVss))
			{							
				foreach(MainForm.DBConnection c in frm.DBConnections)
				{
					if(((QCTreeNode)CurrentTreeNode.Parent).objectName.IndexOf(c.ConnectionName)!= -1)
					{
                        cDdname = CurrentTreeNode.Tag.ToString().ToUpper();
						dbConnection = c;
						break;
					}
				}
				if (dbConnection == null)
					return;

				if ( CurrentTreeNode.FirstNode.Text.ToUpper() == "TABLES"  )
				{
					CurrentTreeNode.Nodes.Clear();
				}
				else if (CurrentTreeNode.FirstNode.Text == "..." )
				{
					CurrentTreeNode.FirstNode.Remove();
				}
				else
				{
					Cursor.Current = Cursors.Default;
					return;
				}

				IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
				if (db == null) throw new Exception("Unable to connect to the server.");

				ArrayList dbArr =  db.GetDatabasesObjects(dbConnection.ConnectionName, dbConnection.Connection);

                
				foreach(SQLEditor.Database.DB database in dbArr)
				{
                    cDdname = CurrentTreeNode.Tag.ToString().ToUpper();
                    //if (database.Name.ToUpper() == CurrentTreeNode.Text.ToUpper())
                    if (database.Name.ToUpper() == cDdname)
					{
						serverDB = 	database;
						break;
					}
				}
				if (serverDB == null)
				{
					Cursor.Current = Cursors.Default;
					return;
				}

				QCTreeNode TableNode = new QCTreeNode("Tables",QCTreeNode.ObjectType.Tables,null,null,null,null,"",0);
				DBTreeViewTypes.Add(TableNode,QCTreeNode.ObjectType.Tables);

				QCTreeNode ViewNode = new QCTreeNode("Views",QCTreeNode.ObjectType.Views,null,null,null,null,"",0);
				DBTreeViewTypes.Add(ViewNode,QCTreeNode.ObjectType.Views);

				QCTreeNode SpNode = new QCTreeNode("Stored procedures",QCTreeNode.ObjectType.StoredProcedures,null,null,null,null,"",0);
				DBTreeViewTypes.Add(SpNode,QCTreeNode.ObjectType.StoredProcedures);

				QCTreeNode FnNode = new QCTreeNode("Functions",QCTreeNode.ObjectType.Functions,null,null,null,null,"",0);
				DBTreeViewTypes.Add(FnNode,QCTreeNode.ObjectType.Functions);

				QCTreeNode TrNode = new QCTreeNode("Triggers",QCTreeNode.ObjectType.Triggers,null,null,null,null,"",0);
				DBTreeViewTypes.Add(TrNode,QCTreeNode.ObjectType.Triggers);

				CurrentTreeNode.Nodes.Add(TableNode);
				CurrentTreeNode.Nodes.Add(ViewNode);
				CurrentTreeNode.Nodes.Add(SpNode);
				CurrentTreeNode.Nodes.Add(FnNode);
				CurrentTreeNode.Nodes.Add(TrNode);

				// have to implement DEFAULTS, UDTS, AND RULES seperately since they are not returned in the dbObjects array 
				QCTreeNode DefaultsNode = new QCTreeNode("Defaults", QCTreeNode.ObjectType.Defaults, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
				DBTreeViewTypes.Add(DefaultsNode, QCTreeNode.ObjectType.Defaults);
				ArrayList DefaultsArr = db.GetDataBaseDefaults( serverDB.Name, dbConnection.Connection );
				if (DefaultsArr != null && DefaultsArr.Count > 0)
				{
					QCTreeNode DefaultNode = new QCTreeNode("...", QCTreeNode.ObjectType.Default, null, null, null,null,"",0);
					DefaultsNode.Nodes.Add(DefaultNode);
					if ( addtoVss )
					{
						string CreateScript = string.Empty;
						foreach ( DBObject dbObject in DefaultsArr )
						{
							CreateScript += 
								db.GetCreateObjectString(dbObject.Name, serverDB.Name, dbConnection.Connection);
							CreateScript += "\nGO\n\n"; // added go in between each default create statement to avoid error when running
						}
                        //DefaultsNode.AddItem( CreateScript );
					}
				}
				CurrentTreeNode.Nodes.Add(DefaultsNode);

				QCTreeNode RulesNode = new QCTreeNode("Rules", QCTreeNode.ObjectType.Rules, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
				DBTreeViewTypes.Add(RulesNode, QCTreeNode.ObjectType.Rules);
				ArrayList RulesArr = db.GetDataBaseRules( serverDB.Name, dbConnection.Connection );
				if (RulesArr != null && RulesArr.Count > 0)
				{
					QCTreeNode RuleNode = new QCTreeNode("...", QCTreeNode.ObjectType.Rule, null, null, null,null,"",0);
					RulesNode.Nodes.Add(RuleNode);
					if ( addtoVss )
					{
						string CreateScript = string.Empty;
						foreach ( DBObject dbObject in RulesArr )
						{
							CreateScript += 
								db.GetCreateObjectString(dbObject.Name, serverDB.Name, dbConnection.Connection);
							CreateScript += "\nGO\n\n"; // added go in between each rule create statement to avoid error when running
						}
                        //RulesNode.AddItem( CreateScript );
					}
				}
				CurrentTreeNode.Nodes.Add(RulesNode);

				QCTreeNode UDTSNode = new QCTreeNode("UDTs", QCTreeNode.ObjectType.UDTs, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
				DBTreeViewTypes.Add(UDTSNode, QCTreeNode.ObjectType.UDTs);
				ArrayList UDTSArr = db.GetDataBaseUDTs( serverDB.Name, dbConnection.Connection );
				if (UDTSArr != null && UDTSArr.Count > 0)
				{
					QCTreeNode UDTNode = new QCTreeNode("...", QCTreeNode.ObjectType.UDT, null, null, null, null,"",0);
					UDTSNode.Nodes.Add(UDTNode);
					if ( addtoVss )
					{
						string CreateScript = string.Empty;
						CreateScript = string.Format("use [{0}]\n\n", serverDB.Name);
						foreach ( DBObjectAttribute dbObjectAttrib in UDTSArr)
						{
							CreateScript += string.Format("if exists (select * from dbo.systypes where name = N'{0}')\n" + 
							"    EXEC sp_droptype N'{0}'\n", dbObjectAttrib.Name);
							CreateScript += db.GetCreateUDTString( dbObjectAttrib, 
								serverDB.Name, dbConnection.Connection) + "\n\n";
						}
                        //UDTSNode.AddItem( CreateScript );
					}
				}
				CurrentTreeNode.Nodes.Add(UDTSNode);

				// system tables are not in the dbObject array.  
				// not planning on persisting these tables to vss
				if(dbConnection.Connection is SqlConnection)
				{
					QCTreeNode SysTablesNode = new QCTreeNode("System Tables", QCTreeNode.ObjectType.SysTables, null, null, null, null,"",0);
					DBTreeViewTypes.Add(SysTablesNode,QCTreeNode.ObjectType.SysTables);

					DataTable SysTbls = db.ExecuteCommand( db.Commands.GetSysTables( serverDB.Name ), dbConnection.Connection, serverDB.Name ) ;
					if ( SysTbls != null )
					{
						foreach( DataRow dr in SysTbls.Rows)
						{
							QCTreeNode sysTableNode = new QCTreeNode(dr["TABLE_NAME"].ToString(), QCTreeNode.ObjectType.Table, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(sysTableNode,QCTreeNode.ObjectType.Table);
							SysTablesNode.Nodes.Add(sysTableNode);
						}
					}
					CurrentTreeNode.Nodes.Add(SysTablesNode);
				}
				foreach(Database.DBObject dbObject in serverDB.dbObjects)
				{
					Cursor.Current = Cursors.WaitCursor;
					QCTreeNode oNode = null;
								
					switch(dbObject.Type.ToUpper())
					{
						case "U ": //Tables
							oNode = new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.Table, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(oNode, QCTreeNode.ObjectType.Table);
							TableNode.Nodes.Add(oNode);

							// fields
							QCTreeNode FiledNode = new QCTreeNode("Fields", QCTreeNode.ObjectType.Fields, null, null, dbConnection.Connection, null,"",0);
							DBTreeViewTypes.Add(FiledNode, QCTreeNode.ObjectType.Fields);
							oNode.Nodes.Add(FiledNode);

							//Add an empty node
							QCTreeNode dbNode = new QCTreeNode("...", QCTreeNode.ObjectType.Empty, null, null, dbConnection.Connection, null,"",0);
							FiledNode.Nodes.Add(dbNode);

							// indexes
							QCTreeNode IndexesNode = new QCTreeNode("Indexes", QCTreeNode.ObjectType.Indexes, null, null,dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(IndexesNode, QCTreeNode.ObjectType.Indexes);
							oNode.Nodes.Add(IndexesNode);

							//Add an empty node
							dbNode = new QCTreeNode("...", QCTreeNode.ObjectType.Empty, null, null,dbConnection.Connection, null,"",0);
							IndexesNode.Nodes.Add(dbNode);							
							break;
						case "V ": //Views
							oNode=new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.View, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(oNode, QCTreeNode.ObjectType.View);
							ViewNode.Nodes.Add(oNode);
							break;
						case "P ": //Stored Procedures
							oNode=new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.StoredProcedure, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(oNode, QCTreeNode.ObjectType.StoredProcedure);
							//Add an empty node

							QCTreeNode pNode = new QCTreeNode("...", QCTreeNode.ObjectType.Empty, null, null, dbConnection.Connection, null,"",0);
							oNode.Nodes.Add(pNode);
							SpNode.Nodes.Add(oNode);
							break;
						case "FN": //Functions
							oNode=new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.Function, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(oNode, QCTreeNode.ObjectType.Function);
							FnNode.Nodes.Add(oNode);
							break;
						case "TF": //Functions
							oNode = new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.Function, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(oNode, QCTreeNode.ObjectType.Function);
							FnNode.Nodes.Add(oNode);
							break;
						case "TR": //Triggers
							oNode = new QCTreeNode(dbObject.Name, QCTreeNode.ObjectType.Trigger, null, dbConnection.ConnectionName, dbConnection.Connection, serverDB.Name,"",0);
							DBTreeViewTypes.Add(oNode, QCTreeNode.ObjectType.Trigger);
							TrNode.Nodes.Add(oNode);

							//Add an empty node
							QCTreeNode propNode = new QCTreeNode("...", QCTreeNode.ObjectType.Filed, null, null, dbConnection.Connection, null,"",0);
							oNode.Nodes.Add(propNode);							
							break;
						default:										
							break;
					}
				}
				if (addtoVss)
				{
					frm.statusBar.Panels[3].Text = "";
				}
			}
			Cursor.Current = Cursors.Default;
		}
		public void RefreashTreeView(string tcDefaultDB)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				TvDBObjects.BeginUpdate();
				TvDBObjects.Nodes.Clear();
				DBTreeViewTypes.Clear();
                //QCTreeNode node = new QCTreeNode("SQL servers", QCTreeNode.ObjectType.Top, null, null, null, null, "", 0);
                //TvDBObjects.Nodes.Add(node);
                //DBTreeViewTypes.Add(node, QCTreeNode.ObjectType.Top);

                QCTreeNode node;

                MainForm frm = (MainForm)MdiParentForm;
				frm.RefreashDBConnections();
                IDbConnection sConnection = null;
				foreach(MainForm.DBConnection dbConnection in frm.DBConnections)
				{
                    //QCTreeNode serverNode;
					if(dbConnection.FrienlyName==null)
						//serverNode = new QCTreeNode(dbConnection.ConnectionName,QCTreeNode.ObjectType.Server, null, dbConnection.ConnectionName, dbConnection.Connection, null,"",0);
                        node = new QCTreeNode(dbConnection.ConnectionName,QCTreeNode.ObjectType.Server, null, dbConnection.ConnectionName, dbConnection.Connection, null,"",0);
                    else if (dbConnection.FrienlyName.Length == 0)
                        //serverNode = new QCTreeNode(dbConnection.ConnectionName, QCTreeNode.ObjectType.Server, null, dbConnection.ConnectionName, dbConnection.Connection, null, "", 0);
                        node = new QCTreeNode(dbConnection.ConnectionName, QCTreeNode.ObjectType.Server, null, dbConnection.ConnectionName, dbConnection.Connection, null, "", 0);
                    else
                    {
                        ///serverNode = new QCTreeNode(dbConnection.ConnectionName + " [" + dbConnection.FrienlyName + "]", QCTreeNode.ObjectType.Server, null, dbConnection.ConnectionName, dbConnection.Connection, null, "", 0);
                        node = new QCTreeNode(dbConnection.ConnectionName + " [" + dbConnection.FrienlyName + "]", QCTreeNode.ObjectType.Server, null, dbConnection.ConnectionName, dbConnection.Connection, null, "", 0);
                    }
                    sConnection = dbConnection.Connection;
                    //QCTreeNode node = new QCTreeNode("Company List", QCTreeNode.ObjectType.Server, null, null, null, null, "", 0);
                    TvDBObjects.Nodes.Add(node);
                    DBTreeViewTypes.Add(node, QCTreeNode.ObjectType.Server);

                    //DBTreeViewTypes.Add(serverNode, QCTreeNode.ObjectType.Server);
                    //node.Nodes.Add(serverNode);
                    //QCTreeNode dbNode ;
                    QCTreeNode dbNode = new QCTreeNode("...", QCTreeNode.ObjectType.Empty, null, null, null, null, "", 0);
                    node.Nodes.Add(dbNode);
                    //serverNode.Nodes.Add(dbNode);
					node.Expand();
                   break;
				}
				TvDBObjects.EndUpdate();
                if (tcDefaultDB != null && tcDefaultDB != "")
                {
                    frm.ActiveQueryForm.SetDatabaseConnection(tcDefaultDB.ToString().ToUpper(), sConnection);
                }
                Cursor.Current = Cursors.Default;
				bool okAll=false;
				bool noAll=false;
				ArrayList windowsToClose=new ArrayList();

                foreach(FrmQuery frmQuery in frm.QueryForms)
				{
					bool connectionStillExists=false;
					foreach(MainForm.DBConnection dbConnection in frm.DBConnections)
					{
						if(frmQuery.dbConnection.ConnectionString==dbConnection.Connection.ConnectionString)
						{
							connectionStillExists=true;
							break;
						}
					}
					if(!connectionStillExists)
					{
						FrmChangeDialogDataSource frmChangeDialogDataSource=new FrmChangeDialogDataSource(frmQuery.Text,
							((MainForm.DBConnection)frm.DBConnections[0]).ConnectionName+"["+((MainForm.DBConnection)frm.DBConnections[0]).FrienlyName+"]");
						
						if(okAll)
							frmQuery.SetDatabaseConnection(((MainForm.DBConnection)frm.DBConnections[0]).InitialCatalog, ((MainForm.DBConnection)frm.DBConnections[0]).Connection);
						else if(noAll)
							windowsToClose.Add(frmQuery);
						else if(frmChangeDialogDataSource.ShowDialogWindow(this)==DialogResult.Yes) 
						{
							frmQuery.SetDatabaseConnection(((MainForm.DBConnection)frm.DBConnections[0]).InitialCatalog, ((MainForm.DBConnection)frm.DBConnections[0]).Connection);
							okAll=frmChangeDialogDataSource.chbApplyToAll.Checked;
						}
						else
						{
							windowsToClose.Add(frmQuery);
							noAll=frmChangeDialogDataSource.chbApplyToAll.Checked;
						}
					}
				}

                foreach (FrmQuery frmQuery in windowsToClose)
                {
                    frmQuery.Close();
                }
				return;
			}
			catch(Exception ex)
			{
				TvDBObjects.EndUpdate();
				Cursor.Current = Cursors.Default;
				throw ex;
			}
		}

        ////private bool IsCheckout(SQLEditor.VSS.VSSConnection vssConnection, string type, string objectName)
        ////{
        ////    if(vssConnection == null)
        ////        return false;

        ////    switch(type.Trim())
        ////    {
        ////        case "U":
        ////            return vssConnection.GetStatus(vssConnection.GetVSSPath(VSS.VSSConnection.DBObjectTypes.Table) + "\\" + objectName)==2;

        ////        case "V":
        ////            return vssConnection.GetStatus(vssConnection.GetVSSPath(VSS.VSSConnection.DBObjectTypes.View) + "\\" + objectName)==2;

        ////        case "P":
        ////            return vssConnection.GetStatus(vssConnection.GetVSSPath(VSS.VSSConnection.DBObjectTypes.StoredProcedure) + "\\" + objectName)==2;

        ////        case "FN":
        ////            //return vssConnection.GetStatus(vssConnection.GetVSSPath(VSS.VSSConnection.DBObjectTypes.Function) + "\\" + objectName)==2;
        ////            // fall thru
        ////        case "TF":
        ////            return vssConnection.GetStatus(vssConnection.GetVSSPath(VSS.VSSConnection.DBObjectTypes.Function) + "\\" + objectName)==2;

        ////        case "TR":
        ////            return vssConnection.GetStatus(vssConnection.GetVSSPath(VSS.VSSConnection.DBObjectTypes.Trigger) + "\\" + objectName)==2;

        ////        default:
        ////            return false;
        ////    }
        ////}

        private int ParseText(WordAndPosition[] words, string s)
		{
			words.Initialize();
			int count = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				if (count >= words.Length) break;
				words[count].Word = m.Value;
				words[count].Position = m.Index;
				words[count].Length = m.Length;
				count++;
			}
			return count;
		}
		private string Create2Alter(string script, string type)
		{
			string returnString=script;
			int wordCount = script.Split(' ').Length;
			WordAndPosition[] words  = new WordAndPosition[wordCount];
			int count = ParseText(words,script);
			
			for(int i=0;i<(count - 1);i++)
			{
				if((words[i].Word.ToUpper()=="CREATE") && (
					(words[i+1].Word.ToUpper()=="PROCEDURE" || 
					words[i+1].Word.ToUpper()=="FUNCTION")|| 
					words[i+1].Word.ToUpper()=="VIEW"||
					words[i+1].Word.ToUpper()=="TABLE"||
					words[i+1].Word.ToUpper()=="TRIGGER"))
				{
					returnString = returnString.Substring(0,words[i].Position) + "ALTER" + returnString.Substring(words[i].Position + words[i].Length, returnString.Length-(words[i].Position + words[i].Length));
					break;
				}
			}
			return returnString;
			
		}
		private void SetContextMenu(TreeNode node)
		{
			//disabe all items
			foreach(MenuItem item in contextMenu1.MenuItems)
				item.Visible= false;
		
			switch((int)DBTreeViewTypes[node])
			{
				case (int)QCTreeNode.ObjectType.Top:
                    SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.AddServerVisible | MenuItemsTypes.RefreshDataObjects);
                    break;
				case (int)QCTreeNode.ObjectType.Server:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ServerInfo);
					break;
				case (int)QCTreeNode.ObjectType.Database:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.UseDatabaseVisible);
					break;
				case (int)QCTreeNode.ObjectType.Tables:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.None);
					break;
				case (int)QCTreeNode.ObjectType.StoredProcedures:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.None);
					break;
				case (int)QCTreeNode.ObjectType.Functions:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.None);
					break;
				case (int)QCTreeNode.ObjectType.Table:
					if (CurrentTreeNode.Parent.Text.ToLower() == "system tables")
						SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptCreateVisible | 
							MenuItemsTypes.CreateInsertScript | 
							MenuItemsTypes.CreateUpdateScript | 
							MenuItemsTypes.GetDepends |
							MenuItemsTypes.TableInfo |
							MenuItemsTypes.SystemTableInfo);
					
					else
						SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptCreateVisible | 
								MenuItemsTypes.CreateInsertScript | 
								MenuItemsTypes.CreateUpdateScript | 
								MenuItemsTypes.GetDepends |
								MenuItemsTypes.TableInfo);

					break;
				case (int)QCTreeNode.ObjectType.View:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptAlterVisible | MenuItemsTypes.ScriptDropVisible | MenuItemsTypes.GetDepends );
					break;
				case (int)QCTreeNode.ObjectType.StoredProcedure:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptAlterVisible | MenuItemsTypes.ScriptDropVisible | MenuItemsTypes.ScriptExecVisible | MenuItemsTypes.GetDepends );
					break;
				case (int)QCTreeNode.ObjectType.Function:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptAlterVisible | MenuItemsTypes.ScriptDropVisible | MenuItemsTypes.GetDepends );
					break;
				case (int)QCTreeNode.ObjectType.Trigger:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptAlterVisible |  MenuItemsTypes.ScriptAlterVisible | MenuItemsTypes.ScriptDropVisible | MenuItemsTypes.GetDepends );
					break;
				case (int)QCTreeNode.ObjectType.UDTs:
				case (int)QCTreeNode.ObjectType.Defaults:
				case (int)QCTreeNode.ObjectType.Rules:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptAlterVisible );
					break;
				case (int)QCTreeNode.ObjectType.UDT:
				case (int)QCTreeNode.ObjectType.Rule:
				case (int)QCTreeNode.ObjectType.Default:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptAlterVisible | MenuItemsTypes.ScriptDropVisible);
					break;
				case (int)QCTreeNode.ObjectType.ScriptFile:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptExecVisible);
					break;
				case (int)QCTreeNode.ObjectType.PK:
				case (int)QCTreeNode.ObjectType.Index:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ShowStatistics);
					break;
				case (int)QCTreeNode.ObjectType.Job:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.ScriptCreateVisible | MenuItemsTypes.ScriptDeleteVisible | MenuItemsTypes.ScriptExecVisible);
					break;
				default:
					SetContextMenuItems((QCTreeNode)node, MenuItemsTypes.None);
					break;
			}

			// Source control
			if(node is QCTreeNode)
			{
				mi_DetachFromSourceControl.Visible=false;
				if(((QCTreeNode)node).objecttype == QCTreeNode.ObjectType.Database || ((QCTreeNode)node).objecttype == QCTreeNode.ObjectType.Jobs)
				{
                    //////mi_SourceControl.Visible = true;
					mi_CheckIn.Visible=false;
					mi_CheckOut.Visible=false;
					mi_UndoCheckOut.Visible=false;
					mi_OpenSourceFile.Visible = false;
					menuItem7.Visible = false;
					mi_ShowHistory.Visible=false;
					
					if (((QCTreeNode)node).objecttype == QCTreeNode.ObjectType.Jobs)
					{
						mi_AddToSourceControl.Text = mi_AddToSourceControl.Text.Replace( "Database", "Jobs" );
					}
					else
					{
						mi_AddToSourceControl.Text = mi_AddToSourceControl.Text.Replace( "Jobs", "Database" );
					}
					if( !((QCTreeNode)node).IsUnderSourceControl)
					{
						mi_AddToSourceControl.Visible = true;
						mi_DetachFromSourceControl.Visible = false;
						mi_VSSSettings.Visible = false;
						mi_Persist2VSS.Visible = false;
					}
					else
					{
						mi_AddToSourceControl.Visible=false;
						mi_DetachFromSourceControl.Visible=true;
						mi_VSSSettings.Visible=true;
						mi_Persist2VSS.Visible = true;
					}	
					return;
				}
				else
				{
					mi_CheckIn.Visible=true;
					mi_CheckOut.Visible=true;
					mi_UndoCheckOut.Visible=true;
					mi_ShowHistory.Visible=true;
					mi_OpenSourceFile.Visible = false;
					menuItem7.Visible = false;
					mi_AddToSourceControl.Visible=false;
                    //////mi_SourceControl.Visible=false;
					mi_VSSSettings.Visible=false;
					mi_Persist2VSS.Visible = false;
				}

				if( ((QCTreeNode)node).IsUnderSourceControl )
				{
					if( ((QCTreeNode)node).objecttype == QCTreeNode.ObjectType.Rule ||
						((QCTreeNode)node).objecttype == QCTreeNode.ObjectType.UDT ||
						((QCTreeNode)node).objecttype == QCTreeNode.ObjectType.Default )
					{
						mi_CheckIn.Visible=false;
						mi_CheckOut.Visible=false;
						mi_UndoCheckOut.Visible=false;
						mi_ShowHistory.Visible=false;
					}
				}
			}
			
		}
		private void SetContextMenuItems(QCTreeNode node,MenuItemsTypes menuItemsTypes)
		{
			mi_ShowConstraints.Visible = false;

			miAddServer.Visible				= (MenuItemsTypes.AddServerVisible & menuItemsTypes)==MenuItemsTypes.AddServerVisible;
			miRefreshDataObjects.Visible	= (MenuItemsTypes.RefreshDataObjects & menuItemsTypes)==MenuItemsTypes.RefreshDataObjects;
			miRemoveServer.Visible			= (MenuItemsTypes.RemoveServerVisible & menuItemsTypes)==MenuItemsTypes.RemoveServerVisible;
			miUseDatabase.Visible			= (MenuItemsTypes.UseDatabaseVisible & menuItemsTypes)==MenuItemsTypes.UseDatabaseVisible;
			miDelete.Visible				= (MenuItemsTypes.DeleteVisible & menuItemsTypes)==MenuItemsTypes.DeleteVisible;
			miNew.Visible					= (MenuItemsTypes.NewVisible & menuItemsTypes)==MenuItemsTypes.NewVisible;
			miScriptAlter.Visible			= (MenuItemsTypes.ScriptAlterVisible & menuItemsTypes)==MenuItemsTypes.ScriptAlterVisible;
			miScriptCreate.Visible			= (MenuItemsTypes.ScriptCreateVisible & menuItemsTypes)==MenuItemsTypes.ScriptCreateVisible;
			miScriptDelete.Visible			= (MenuItemsTypes.ScriptDeleteVisible & menuItemsTypes)==MenuItemsTypes.ScriptDeleteVisible;
			miScriptExec.Visible			= (MenuItemsTypes.ScriptExecVisible & menuItemsTypes)==MenuItemsTypes.ScriptExecVisible;
			miScriptDrop.Visible			= (MenuItemsTypes.ScriptDropVisible & menuItemsTypes)==MenuItemsTypes.ScriptDropVisible;
			miScriptInsert.Visible			= (MenuItemsTypes.ScriptInsertVisible & menuItemsTypes)==MenuItemsTypes.ScriptInsertVisible;
			miScriptSelect.Visible			= (MenuItemsTypes.ScriptSelectVisible & menuItemsTypes)==MenuItemsTypes.ScriptSelectVisible;
			miScriptUpdate.Visible			= (MenuItemsTypes.ScriptUpdateVisible & menuItemsTypes)==MenuItemsTypes.ScriptUpdateVisible;
			miCreateInsertScript.Visible	= (MenuItemsTypes.CreateInsertScript & menuItemsTypes)==MenuItemsTypes.CreateInsertScript;
			miCreateUpdateScript.Visible	= (MenuItemsTypes.CreateUpdateScript & menuItemsTypes)==MenuItemsTypes.CreateUpdateScript;
			miOpenScript.Visible			= (MenuItemsTypes.OpenScriptFile & menuItemsTypes)==MenuItemsTypes.OpenScriptFile;
			
			// Sql Client
			if(node.Connection is SqlConnection)
			{
				mi_Dependencies.Visible			= (MenuItemsTypes.GetDepends & menuItemsTypes)==MenuItemsTypes.GetDepends;
				menuItem6.Visible				= (MenuItemsTypes.GetDepends & menuItemsTypes)==MenuItemsTypes.GetDepends;

                //////menuItem3.Visible				= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;
				mi_TableInfo.Visible			= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;
				mi_ShowConstraints.Visible		= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo; // only really expect tables to have contraints
				mi_TableSpace.Visible			= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;
				mi_TableStats.Visible			= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;
				mi_TablePermissions.Visible		= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;
				mi_UniqueRowColumns.Visible		= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;
				mi_ForeignKeys.Visible			= (MenuItemsTypes.TableInfo & menuItemsTypes)==MenuItemsTypes.TableInfo;

				mi_GetProcUsers.Visible			= (MenuItemsTypes.ServerInfo & menuItemsTypes)==MenuItemsTypes.ServerInfo;
                //mi_Alerts.Visible				= (MenuItemsTypes.ServerInfo & menuItemsTypes)==MenuItemsTypes.ServerInfo;
                //mi_LinkedServers.Visible		= (MenuItemsTypes.ServerInfo & menuItemsTypes)==MenuItemsTypes.ServerInfo;
                //mi_Locks.Visible				= (MenuItemsTypes.ServerInfo & menuItemsTypes)==MenuItemsTypes.ServerInfo;
				mi_ServerStats.Visible			= (MenuItemsTypes.ServerInfo & menuItemsTypes)==MenuItemsTypes.ServerInfo;
			}
			menuItem2.Visible				= miCreateInsertScript.Visible || miCreateUpdateScript.Visible;

			if(miScriptAlter.Visible || miScriptCreate.Visible|| miScriptDelete.Visible || miScriptDrop.Visible || miScriptInsert.Visible)
				miScript.Visible = true;
			else
				miScript.Visible = false;

		}
        public void CreateConstructorString(string objectName)
		{
			string dbName = string.Empty;
			MainForm frm =  (MainForm)MdiParentForm;
			frm.statusBar.Panels[3].Text = "Querying for [" + objectName + "]...";
			IDatabaseManager db = DatabaseFactory.CreateNew(frm.ActiveQueryForm.dbConnection);

			string CreateScript = db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
				objectName,
				frm.ActiveQueryForm.dbConnection,
				DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.PROCEDURE);

			if(CreateScript.Length==0)
			{
				CreateScript = db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
					objectName,
					frm.ActiveQueryForm.dbConnection,
					DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.TABLE);
			}
			if(CreateScript.Length==0)
			{
				CreateScript = db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
					objectName,
					frm.ActiveQueryForm.dbConnection,
					DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.VIEW);
			}

			if(CreateScript.Length==0)
			{
				CreateScript = db.GetObjectConstructorString(frm.ActiveQueryForm.DatabaseName,
					objectName,
					frm.ActiveQueryForm.dbConnection,
					DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.TRIGGER);
			}

			if(CreateScript.Length == 0)
			{
				string msg = String.Format(Localization.GetString("FrmDBObjects.CreateConstructorString"),objectName,frm.ActiveQueryForm.DatabaseName);
				MessageBox.Show(msg ,"Reference",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
				frm.statusBar.Panels[3].Text="";
				return;
			}

			CreateScript = Create2Alter(CreateScript,"");
			// Use database
			if(frm.ActiveQueryForm == null)
			{
				frm.statusBar.Panels[3].Text="";
				return;
			}

			IDbConnection c = frm.ActiveQueryForm.dbConnection;
			dbName = frm.ActiveQueryForm.DatabaseName;

			frm.NewQueryform();
			
			frm.ActiveQueryForm.SetDatabaseConnection(dbName, c);
			
			if(CreateScript.IndexOf("</member>",0)>0)
				frm.ActiveQueryForm.Content = AddRevisionCommentSection(CreateScript);
			else
				frm.ActiveQueryForm.Content = CreateScript;

			if (frm.ActiveQueryForm.FileName.Length == 0)
			{
				frm.ActiveQueryForm.FileName = dbName + "_" + objectName + ".sql";
			}
			frm.ActiveQueryForm.Text = objectName;
			
			frm.statusBar.Panels[3].Text="";
			
		}
		public string  AddRevisionCommentSection(string content)
		{
			int startpos = content.IndexOf("</member>",0);
			if(startpos<1)
				return content;

			startpos = content.LastIndexOf("</revision>") + 11;
			return content.Substring(0,startpos) + "\n\t<revision author='" + SystemInformation.UserName.ToString()  + "' date='" + DateTime.Now.ToString() + "'>Altered</revision>" + content.Substring(startpos);
		}
		#endregion
	}
}
