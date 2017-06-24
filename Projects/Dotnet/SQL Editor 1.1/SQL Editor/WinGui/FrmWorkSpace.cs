// *******************************************************************************
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *******************************************************************************
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WeifenLuo.WinFormsUI;
using System.Data;
using System.Data.SqlClient;
using QueryCommander.General;
using QueryCommander.Database;
using QueryCommander.General.WorkSpace;

namespace QueryCommander
{
	/// <summary>
	/// Summary description for FrmWorkSpace.
	/// </summary>
	public class FrmWorkSpace : FrmBaseContent
	{
		#region Members
		private System.Windows.Forms.TreeView TvDBObjects;
		private System.Windows.Forms.ImageList imglDataObjects;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ContextMenu contextMenuWorkSpace;
		private System.Windows.Forms.MenuItem menuItemAddWorkspace;
		private System.Windows.Forms.MenuItem menuItemDeleteWorkspace;
		private System.Windows.Forms.MenuItem menuItemAddActiveDocument;
		private System.Windows.Forms.MenuItem menuItemAddAllDocuments;
		private System.Windows.Forms.MenuItem menuItemDeleteItem;
		private System.Windows.Forms.MenuItem menuItemOpenDocument;
		private System.Windows.Forms.MenuItem menuItemAddScriptFile;
		private System.Windows.Forms.MenuItem menuItemOpenWorkSpace;
		private System.Windows.Forms.MenuItem menuItemSep1;
		private System.Windows.Forms.MenuItem menuItemRenameWorkspace;
		private QCTreeNode CurrentTreeNode;
		#endregion
		#region Default
		public FrmWorkSpace(Form mdiParentForm)
		{
			this.MdiParentForm = mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float 
				| WeifenLuo.WinFormsUI.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.DockAreas.DockRight)));
            
			this.DefaultHelpUrl="::/Workspace%20Explorer.htm";
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmWorkSpace));
			this.imglDataObjects = new System.Windows.Forms.ImageList(this.components);
			this.TvDBObjects = new System.Windows.Forms.TreeView();
			this.contextMenuWorkSpace = new System.Windows.Forms.ContextMenu();
			this.menuItemAddWorkspace = new System.Windows.Forms.MenuItem();
			this.menuItemOpenWorkSpace = new System.Windows.Forms.MenuItem();
			this.menuItemDeleteWorkspace = new System.Windows.Forms.MenuItem();
			this.menuItemRenameWorkspace = new System.Windows.Forms.MenuItem();
			this.menuItemSep1 = new System.Windows.Forms.MenuItem();
			this.menuItemAddActiveDocument = new System.Windows.Forms.MenuItem();
			this.menuItemAddAllDocuments = new System.Windows.Forms.MenuItem();
			this.menuItemAddScriptFile = new System.Windows.Forms.MenuItem();
			this.menuItemOpenDocument = new System.Windows.Forms.MenuItem();
			this.menuItemDeleteItem = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// imglDataObjects
			// 
			this.imglDataObjects.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imglDataObjects.ImageSize = new System.Drawing.Size(16, 16);
			this.imglDataObjects.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglDataObjects.ImageStream")));
			this.imglDataObjects.TransparentColor = System.Drawing.Color.Black;
			// 
			// TvDBObjects
			// 
			this.TvDBObjects.BackColor = System.Drawing.Color.White;
			this.TvDBObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TvDBObjects.ImageList = this.imglDataObjects;
			this.TvDBObjects.ItemHeight = 16;
			this.TvDBObjects.Location = new System.Drawing.Point(0, 2);
			this.TvDBObjects.Name = "TvDBObjects";
			this.TvDBObjects.Size = new System.Drawing.Size(221, 361);
			this.TvDBObjects.TabIndex = 0;
			this.TvDBObjects.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TvDBObjects_MouseUp);
			this.TvDBObjects.DoubleClick += new System.EventHandler(this.TvDBObjects_DoubleClick);
			// 
			// contextMenuWorkSpace
			// 
			this.contextMenuWorkSpace.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuItemAddWorkspace,
																								 this.menuItemOpenWorkSpace,
																								 this.menuItemDeleteWorkspace,
																								 this.menuItemRenameWorkspace,
																								 this.menuItemSep1,
																								 this.menuItemAddActiveDocument,
																								 this.menuItemAddAllDocuments,
																								 this.menuItemAddScriptFile,
																								 this.menuItemOpenDocument,
																								 this.menuItemDeleteItem});
			// 
			// menuItemAddWorkspace
			// 
			this.menuItemAddWorkspace.Index = 0;
			this.menuItemAddWorkspace.Text = "Add WorkSpace";
			this.menuItemAddWorkspace.Click += new System.EventHandler(this.menuItemAddWorkspace_Click);
			// 
			// menuItemOpenWorkSpace
			// 
			this.menuItemOpenWorkSpace.Index = 1;
			this.menuItemOpenWorkSpace.Text = "Open Workspace";
			this.menuItemOpenWorkSpace.Click += new System.EventHandler(this.menuItemOpenWorkSpace_Click);
			// 
			// menuItemDeleteWorkspace
			// 
			this.menuItemDeleteWorkspace.Index = 2;
			this.menuItemDeleteWorkspace.Text = "Delete WorkSpace";
			this.menuItemDeleteWorkspace.Click += new System.EventHandler(this.menuItemDeleteWorkspace_Click);
			// 
			// menuItemRenameWorkspace
			// 
			this.menuItemRenameWorkspace.Index = 3;
			this.menuItemRenameWorkspace.Text = "Rename Workspace";
			this.menuItemRenameWorkspace.Click += new System.EventHandler(this.menuItemRenameWorkspace_Click);
			// 
			// menuItemSep1
			// 
			this.menuItemSep1.Index = 4;
			this.menuItemSep1.Text = "-";
			// 
			// menuItemAddActiveDocument
			// 
			this.menuItemAddActiveDocument.Index = 5;
			this.menuItemAddActiveDocument.Text = "Add active document ";
			this.menuItemAddActiveDocument.Click += new System.EventHandler(this.menuItemAddActiveDocument_Click);
			// 
			// menuItemAddAllDocuments
			// 
			this.menuItemAddAllDocuments.Index = 6;
			this.menuItemAddAllDocuments.Text = "Add all documents";
			this.menuItemAddAllDocuments.Click += new System.EventHandler(this.menuItemAddAllDocuments_Click);
			// 
			// menuItemAddScriptFile
			// 
			this.menuItemAddScriptFile.Index = 7;
			this.menuItemAddScriptFile.Text = "Add script file";
			this.menuItemAddScriptFile.Click += new System.EventHandler(this.menuItemAddScriptFile_Click);
			// 
			// menuItemOpenDocument
			// 
			this.menuItemOpenDocument.Index = 8;
			this.menuItemOpenDocument.Text = "Open document";
			this.menuItemOpenDocument.Click += new System.EventHandler(this.menuItemOpenDocument_Click);
			// 
			// menuItemDeleteItem
			// 
			this.menuItemDeleteItem.Index = 9;
			this.menuItemDeleteItem.Text = "Delete item";
			this.menuItemDeleteItem.Click += new System.EventHandler(this.menuItemDeleteItem_Click);
			// 
			// FrmWorkSpace
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(221, 365);
			this.Controls.Add(this.TvDBObjects);
			this.DockPadding.Bottom = 2;
			this.DockPadding.Top = 2;
			this.HideOnClose = true;
			this.Name = "FrmWorkSpace";
			this.Text = "Workspace Explorer";
			this.Click += new System.EventHandler(this.FrmDBObjects_Click);
			this.Load += new System.EventHandler(this.FrmDBObjects_Load);
			this.ResumeLayout(false);

		}
		#endregion
		#endregion
		#region Events
		private void FrmDBObjects_Load(object sender, System.EventArgs e)
		{
			RefreashTreeView();
		}

		private void FrmDBObjects_Click(object sender, System.EventArgs e)
		{
			
		}

		private void TvDBObjects_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				CurrentTreeNode = (QCTreeNode)TvDBObjects.GetNodeAt(e.X,e.Y);
				if(CurrentTreeNode != null)
				{
					
					SetContextMenu((QCTreeNode)CurrentTreeNode);
					contextMenuWorkSpace.Show(TvDBObjects,new Point(e.X,e.Y));
				}
			}
		}
		
		private void TvDBObjects_DoubleClick(object sender, System.EventArgs e)
		{
			CurrentTreeNode = ((QCTreeNode) TvDBObjects.SelectedNode);
			if(CurrentTreeNode.objecttype!=QCTreeNode.ObjectType.WorkSpaceItem)
				return;

			MainForm frm =  (MainForm)MdiParentForm;
			string database = frm.ActiveQueryForm.DatabaseName;
			IDbConnection sqlConnection = frm.ActiveQueryForm.dbConnection;

			string fileName = CurrentTreeNode.database;
		
			StreamReader sr = new StreamReader(fileName);
			string content = "";
			string line;
			while ((line = sr.ReadLine()) != null) 
			{
				content += "\n" + line;
			}
			sr.Close();

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length>0)
				frm.NewQueryform();

			frm.ActiveQueryForm.SetDatabaseConnection(database,sqlConnection);
			frm.ActiveQueryForm.Content=content;
			frm.ActiveQueryForm.Text = ((QCTreeNode) CurrentTreeNode).Text;
			frm.ActiveQueryForm.FileName=fileName;
		}
		#endregion
		#region Methods
		public void RefreashTreeView()
		{
			try
			{
				TvDBObjects.BeginUpdate();
				TvDBObjects.Nodes.Clear();

				QCTreeNode WorkSpaceCollectionNode=new QCTreeNode("Workspaces",QCTreeNode.ObjectType.WorkSpaces,null,null,null,null);
				TvDBObjects.Nodes.Add(WorkSpaceCollectionNode);
				

				MainForm frm = (MainForm)MdiParentForm;

				foreach(WorkSpace ws in frm.workSpaceCollection)
				{
					QCTreeNode WorkSpaceNode = new QCTreeNode(ws.Name,QCTreeNode.ObjectType.WorkSpace,null,null,null,ws.Name);
					WorkSpaceCollectionNode.Nodes.Add(WorkSpaceNode);

					foreach(WorkSpaceItem file in ws.WorkSpaceItems)
					{
						QCTreeNode nn = new QCTreeNode(file.FileName,QCTreeNode.ObjectType.WorkSpaceItem,null,ws.Name,null,file.FilePath);
						WorkSpaceNode.Nodes.Add(nn);
					}
				}
				WorkSpaceCollectionNode.Expand();
				TvDBObjects.EndUpdate();
			}
			catch(Exception ex)
			{
				TvDBObjects.EndUpdate();
				throw ex;

			}
		}
		
		private void SetContextMenu(QCTreeNode node)
		{
			//disabe all items
			foreach(MenuItem item in contextMenuWorkSpace.MenuItems)
				item.Visible= false;
			switch(node.objecttype)
			{
				case QCTreeNode.ObjectType.WorkSpaces:
					menuItemAddWorkspace.Visible=true;
					break;
				case QCTreeNode.ObjectType.WorkSpace:
					menuItemAddActiveDocument.Visible=true;
					menuItemAddAllDocuments.Visible=true;
					menuItemAddScriptFile.Visible=true;
					menuItemDeleteWorkspace.Visible=true;
					menuItemOpenWorkSpace.Visible=true;
					menuItemSep1.Visible=true;
					menuItemRenameWorkspace.Visible=true;
					break;
				case QCTreeNode.ObjectType.WorkSpaceItem:
					menuItemDeleteItem.Visible=true;
					menuItemOpenDocument.Visible=true;
					break;
			}
		}
		
		#endregion	
		#region ContextMenu Events
		private void menuItemAddWorkspace_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
			WorkSpace ws = frm.AddWorkSpace();
			if(ws!=null)
			{
				RefreashTreeView();
			}
		}

		private void menuItemDeleteWorkspace_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
		
			foreach(WorkSpace ws in frm.workSpaceCollection)
			{
				if(ws.Name==CurrentTreeNode.Text)
				{
					frm.DeleteWorkSpace(ws);
					CurrentTreeNode.Remove();
					WorkSpaceFactory.Save(Application.StartupPath + "\\WorkSpace.config",frm.workSpaceCollection);
					return;
				}
			}
			
		}

		private void menuItemAddActiveDocument_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
			foreach(WorkSpace ws in frm.workSpaceCollection)
			{
				if(ws.Name==CurrentTreeNode.Text)
				{
					WorkSpaceItem wsItem =  frm.AddActiveDocumentToWorkspace(ws);
					if(wsItem!=null)
					{
						QCTreeNode node = new QCTreeNode(wsItem.FileName,QCTreeNode.ObjectType.WorkSpaceItem,null,wsItem.FilePath,null,wsItem.FileName);
						CurrentTreeNode.Nodes.Add(node);
						WorkSpaceFactory.Save(Application.StartupPath + "\\WorkSpace.config",frm.workSpaceCollection);
					}
				}
			}
		}

		private void menuItemAddAllDocuments_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;

			foreach(WorkSpace ws in frm.workSpaceCollection)
			{
				if(ws.Name==CurrentTreeNode.Text)
				{
					WorkSpaceItemCollection wsic = frm.AddAllDocumentToWorkspace(ws);
					if(wsic.Count>0)
					{
						foreach(WorkSpaceItem wsItem in wsic)
						{
		
							QCTreeNode node = new QCTreeNode(wsItem.FileName,QCTreeNode.ObjectType.WorkSpaceItem,null,wsItem.FilePath,null,wsItem.FileName);
							CurrentTreeNode.Nodes.Add(node);
						}
						WorkSpaceFactory.Save(Application.StartupPath + "\\WorkSpace.config",frm.workSpaceCollection);
					}
					return;
				}
			}
			
		}

		private void menuItemAddScriptFile_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
			foreach(WorkSpace ws in frm.workSpaceCollection)
			{
				if(ws.Name==CurrentTreeNode.Text)
				{
					WorkSpaceItem wsItem =  frm.AddAnyDocumentToWorkSpace(ws);
					if(wsItem!=null)
					{
						QCTreeNode node = new QCTreeNode(wsItem.FileName,QCTreeNode.ObjectType.WorkSpaceItem,null,wsItem.FilePath,null,wsItem.FileName);
						CurrentTreeNode.Nodes.Add(node);
						WorkSpaceFactory.Save(Application.StartupPath + "\\WorkSpace.config",frm.workSpaceCollection);
					}
				}
			}
		}
		private void menuItemDeleteItem_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
		
			frm.DeleteWorkspaceItem(CurrentTreeNode.server, CurrentTreeNode.database);
			CurrentTreeNode.Remove();
			WorkSpaceFactory.Save(Application.StartupPath + "\\WorkSpace.config",frm.workSpaceCollection);
			
			
		}

		private void menuItemOpenDocument_Click(object sender, System.EventArgs e)
		{
			MainForm frm =  (MainForm)MdiParentForm;
			string database = frm.ActiveQueryForm.DatabaseName;
			IDbConnection sqlConnection = frm.ActiveQueryForm.dbConnection;

			string fileName = ((QCTreeNode) CurrentTreeNode).database;
		
			StreamReader sr = new StreamReader(fileName);
			string content = "";
			string line;
			while ((line = sr.ReadLine()) != null) 
			{
				content += "\n" + line;
			}
			sr.Close();

			if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length>0)
				frm.NewQueryform();

			frm.ActiveQueryForm.SetDatabaseConnection(database,sqlConnection);
			frm.ActiveQueryForm.Content=content;
			frm.ActiveQueryForm.Text = ((QCTreeNode) CurrentTreeNode).Text;
			frm.ActiveQueryForm.FileName=fileName;
		}
		
		private void menuItemOpenWorkSpace_Click(object sender, System.EventArgs e)
		{
			try
			{
				MainForm frm =  (MainForm)MdiParentForm;
				string database = frm.ActiveQueryForm.DatabaseName;
				IDbConnection sqlConnection = frm.ActiveQueryForm.dbConnection;

				foreach(TreeNode n in CurrentTreeNode.Nodes)
				{
					QCTreeNode qcNode = (QCTreeNode)n;
				
					StreamReader sr = new StreamReader(qcNode.database);
					string fileName = qcNode.database;
				
					string content = "";
					string line;
					while ((line = sr.ReadLine()) != null) 
					{
						content += "\n" + line;
					}
					sr.Close();

					if(frm.ActiveQueryForm == null || frm.ActiveQueryForm.Content.Length>0)
						frm.NewQueryform();

					frm.ActiveQueryForm.SetDatabaseConnection(database,sqlConnection);
					frm.ActiveQueryForm.Content=content;
					frm.ActiveQueryForm.Text = ((QCTreeNode) CurrentTreeNode).Text;
					frm.ActiveQueryForm.FileName=qcNode.database;
				}
			}
			catch{return;}
		}

		private void menuItemRenameWorkspace_Click(object sender, System.EventArgs e)
		{
		
			MainForm frmMain =  (MainForm)MdiParentForm;
			FrmAddWorkSpace frm = new FrmAddWorkSpace(CurrentTreeNode.Text);
			
			if(frm.ShowDialogWindow(this)==DialogResult.OK)
			{
				foreach(WorkSpace ws in frmMain.workSpaceCollection)
				{
					if(ws.Name==CurrentTreeNode.Text)
					{
						ws.Name=frm.txtWorkSpace.Text;
						WorkSpaceFactory.Save(Application.StartupPath + "\\WorkSpace.config",frmMain.workSpaceCollection);
						RefreashTreeView();
						return;
					}
				}
			}
		}

		#endregion	
	}
}
