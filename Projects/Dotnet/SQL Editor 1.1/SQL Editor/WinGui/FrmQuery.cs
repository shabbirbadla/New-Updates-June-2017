using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Reflection;
using SQLEditor.General;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using SQLEditor.Database;
using System.Runtime.InteropServices;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmQuery. 
	/// </summary>
	public class FrmQuery : FrmBaseContent
	{
		#region Private members
		private int keyPressCount = 0;
		private string[] _lines;
		private int _linesPrinted;
		private GridPrinterClass _gridPrinterClass;
		private enum PrintType{Statement, output};
		private PrintType _printType;
		private SQLEditor.Config.Settings _settings;
		private OutPutContainer _outPutContainer=null;
		private Hashtable Aliases = new Hashtable();
		private ArrayList AliasList = new ArrayList();
		private int lastPos;
		private int firstPos;
		private XmlDocument sqlReservedWords = new XmlDocument();
		private static ArrayList _sqlInfoMessages = new ArrayList();
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ContextMenu cmShortcutMeny;		
		private ArrayList ReservedWords = new ArrayList();
		private bool DoInsert;
		private string _OrginalName;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ListView lstv_Commands;
		private System.ComponentModel.IContainer components;
		private string m_fileName = string.Empty;
		private bool m_resetText = true;
		private bool _canceled = false;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private Regex _findNextRegex; 
		private int _findNextStartPos;
		private IDatabaseManager _currentManager=null;
		private DataSet _currentDataSet=null;
		private TimeSpan _currentExecutionTime;
		private IAsyncResult _asyncResult;
		private Exception _currentException=null;
		private int _dragPos; 
		private QCTreeNode _dragObject;
        public string co_name = "";
        public oCompany company = new oCompany();
		private WeifenLuo.WinFormsUI.DockPanel dockManager = new WeifenLuo.WinFormsUI.DockPanel();

		#endregion
		#region Public members
		public bool IsActive; 
		/// <summary>
		/// All queries will be executed with this connection
		/// </summary>
		public IDbConnection dbConnection = null;
		/// <summary>
		/// Current database name
		/// </summary>
		public string DatabaseName = "";
        public int nComp_Id;            /// Company Id
		public SQLEditor.Editor.TextEditorControlWrapper qcTextEditor;
		private System.Windows.Forms.Timer ExecutionTimer;
		private System.Windows.Forms.ToolTip ttParamenterInfo;
		private System.Drawing.Printing.PrintDocument printDocument;
		private System.Windows.Forms.PageSetupDialog pageSetupDialog;
		private System.Windows.Forms.PrintDialog printDialog;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
		private System.Windows.Forms.ContextMenu cmDragAndDrp;
		private System.Windows.Forms.MenuItem menuItemSelect1;
		private System.Windows.Forms.MenuItem menuItemJoin;
		private System.Windows.Forms.MenuItem menuItemLeftOuterJoin;
		private System.Windows.Forms.MenuItem menuItemRightOuterJoin;
		private System.Windows.Forms.MenuItem menuItemObjectName;
		private System.Windows.Forms.MenuItem menuItemOrderBy;
		private System.Windows.Forms.MenuItem menuItemGroupBy;
		private System.Windows.Forms.MenuItem menuItemWhere;
		private System.Windows.Forms.MenuItem menuItemSplitter;
		private System.Windows.Forms.MenuItem menuItemSelect2;
		/// <summary>
		/// The Syntax reader handles all font and color settings.
		/// </summary>
		public SyntaxReader syntaxReader;
		/// <summary>
		/// Used when opening/saving.
		/// </summary>
		public string FileName
		{
			get	{	return m_fileName;	}
			set
			{
				if (value != string.Empty)
				{
					string fileName = value; //.Substring(value.LastIndexOf(@"\")+1);
					this.Text=fileName;
				}

				m_fileName = value;
			}
		}

		/// <summary>
		/// The content of the qcTextEditor
		/// </summary>
		public string Content
		{
			get { return qcTextEditor.Text;}
			set 
			{ 
				qcTextEditor.Text=value;
				qcTextEditor.Refresh();
			}
		}
		/// <summary>
		/// Font settings 
		/// </summary>
		public Font EditorFont
		{
			get{return qcTextEditor.Font;}
			set{qcTextEditor.Font = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		
		#endregion
		#region Default
		public FrmQuery(Form mdiParentForm)
		{
			this.MdiParentForm=mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			syntaxReader =  new SyntaxReader();
			_settings = SQLEditor.Config.Settings.Load();
			
			((MainForm)mdiParentForm).UpdateEditorSettings();

			qcTextEditor.ActiveTextAreaControl.TextArea.MouseUp +=new MouseEventHandler(qcTextEditor_MouseUp);
			qcTextEditor.ActiveTextAreaControl.TextArea.DragDrop +=new DragEventHandler(TextArea_DragDrop);
			qcTextEditor.ActiveTextAreaControl.TextArea.DragEnter +=new DragEventHandler(TextArea_DragEnter);
			qcTextEditor.ActiveTextAreaControl.TextArea.Click +=new EventHandler(TextArea_Click);
			qcTextEditor.ActiveTextAreaControl.TextArea.KeyPress += new KeyPressEventHandler(TextArea_KeyPress);
			qcTextEditor.ActiveTextAreaControl.TextArea.KeyUp += new System.Windows.Forms.KeyEventHandler(TextArea_KeyUp);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			MainForm frm = (MainForm)MdiParentForm;
			frm.QueryForms.Remove(this);
			if(qcTextEditor == null)
				return;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQuery));
            ////ICSharpCode.TextEditor.Document.DefaultDocument defaultDocument1 = new ICSharpCode.TextEditor.Document.DefaultDocument();
            ICSharpCode.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy1 = new ICSharpCode.TextEditor.Document.DefaultFormattingStrategy();
            ICSharpCode.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy1 = new ICSharpCode.TextEditor.Document.DefaultHighlightingStrategy();
            ICSharpCode.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy1 = new ICSharpCode.TextEditor.Document.GapTextBufferStrategy();
            ICSharpCode.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties1 = new ICSharpCode.TextEditor.Document.DefaultTextEditorProperties();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lstv_Commands = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmShortcutMeny = new System.Windows.Forms.ContextMenu();
            this.qcTextEditor = new SQLEditor.Editor.TextEditorControlWrapper();
            this.ExecutionTimer = new System.Windows.Forms.Timer(this.components);
            this.ttParamenterInfo = new System.Windows.Forms.ToolTip(this.components);
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.cmDragAndDrp = new System.Windows.Forms.ContextMenu();
            this.menuItemObjectName = new System.Windows.Forms.MenuItem();
            this.menuItemSplitter = new System.Windows.Forms.MenuItem();
            this.menuItemSelect1 = new System.Windows.Forms.MenuItem();
            this.menuItemSelect2 = new System.Windows.Forms.MenuItem();
            this.menuItemJoin = new System.Windows.Forms.MenuItem();
            this.menuItemLeftOuterJoin = new System.Windows.Forms.MenuItem();
            this.menuItemRightOuterJoin = new System.Windows.Forms.MenuItem();
            this.menuItemWhere = new System.Windows.Forms.MenuItem();
            this.menuItemOrderBy = new System.Windows.Forms.MenuItem();
            this.menuItemGroupBy = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            // 
            // lstv_Commands
            // 
            this.lstv_Commands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstv_Commands.FullRowSelect = true;
            this.lstv_Commands.HideSelection = false;
            this.lstv_Commands.LabelWrap = false;
            this.lstv_Commands.Location = new System.Drawing.Point(35, 20);
            this.lstv_Commands.MultiSelect = false;
            this.lstv_Commands.Name = "lstv_Commands";
            this.lstv_Commands.Size = new System.Drawing.Size(200, 136);
            this.lstv_Commands.SmallImageList = this.imageList1;
            this.lstv_Commands.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstv_Commands.TabIndex = 1;
            this.lstv_Commands.TabStop = false;
            this.lstv_Commands.UseCompatibleStateImageBehavior = false;
            this.lstv_Commands.View = System.Windows.Forms.View.SmallIcon;
            this.lstv_Commands.Visible = false;
            this.lstv_Commands.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstv_Commands_KeyDown);
            this.lstv_Commands.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstv_Commands_MouseMove);
            this.lstv_Commands.Leave += new System.EventHandler(this.lstv_Commands_Leave);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // qcTextEditor
            // 
            this.qcTextEditor.AllowDrop = true;
            this.qcTextEditor.AutoScroll = true;
            this.qcTextEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            ////defaultDocument1.FormattingStrategy = defaultFormattingStrategy1;
            defaultHighlightingStrategy1.Extensions = new string[] {".SQL"};
            //defaultDocument1.HighlightingStrategy = defaultHighlightingStrategy1;
            //defaultDocument1.ReadOnly = false;
            //defaultDocument1.TextBufferStrategy = gapTextBufferStrategy1;
            //defaultDocument1.TextContent = "";
            //defaultTextEditorProperties1.AllowCaretBeyondEOL = false;
            defaultTextEditorProperties1.AutoInsertCurlyBracket = true;
            defaultTextEditorProperties1.BracketMatchingStyle = ICSharpCode.TextEditor.Document.BracketMatchingStyle.After;
            defaultTextEditorProperties1.ConvertTabsToSpaces = false;
            defaultTextEditorProperties1.CreateBackupCopy = false;
            defaultTextEditorProperties1.DocumentSelectionMode = ICSharpCode.TextEditor.Document.DocumentSelectionMode.Normal;
            defaultTextEditorProperties1.EnableFolding = true;
            defaultTextEditorProperties1.Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties1.Encoding")));
            defaultTextEditorProperties1.Font = new System.Drawing.Font("Courier New", 10F);
            defaultTextEditorProperties1.HideMouseCursor = false;
            defaultTextEditorProperties1.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Smart;
            defaultTextEditorProperties1.IsIconBarVisible = false;
            defaultTextEditorProperties1.LineTerminator = "\r\n";
            defaultTextEditorProperties1.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.None;
            defaultTextEditorProperties1.MouseWheelScrollDown = true;
            defaultTextEditorProperties1.MouseWheelTextZoom = true;
            defaultTextEditorProperties1.ShowEOLMarker = true;
            defaultTextEditorProperties1.ShowHorizontalRuler = false;
            defaultTextEditorProperties1.ShowInvalidLines = false;
            defaultTextEditorProperties1.ShowLineNumbers = true;
            defaultTextEditorProperties1.ShowMatchingBracket = true;
            defaultTextEditorProperties1.ShowSpaces = true;
            defaultTextEditorProperties1.ShowTabs = true;
            defaultTextEditorProperties1.ShowVerticalRuler = true;
            defaultTextEditorProperties1.TabIndent = 4;
            defaultTextEditorProperties1.UseAntiAliasedFont = false;
            defaultTextEditorProperties1.UseCustomLine = false;
            defaultTextEditorProperties1.VerticalRulerRow = 80;
            //defaultDocument1.TextEditorProperties = defaultTextEditorProperties1;
            //this.qcTextEditor.Document = defaultDocument1;
            this.qcTextEditor.Encoding = ((System.Text.Encoding)(resources.GetObject("qcTextEditor.Encoding")));
            this.qcTextEditor.IsIconBarVisible = false;
            this.qcTextEditor.Location = new System.Drawing.Point(2, 2);
            this.qcTextEditor.Name = "qcTextEditor";
            this.qcTextEditor.SelectedText = "";
            this.qcTextEditor.SelectionStart = 0;
            this.qcTextEditor.ShowEOLMarkers = true;
            this.qcTextEditor.ShowInvalidLines = false;
            this.qcTextEditor.ShowSpaces = true;
            this.qcTextEditor.ShowTabs = true;
            this.qcTextEditor.ShowVRuler = true;
            this.qcTextEditor.Size = new System.Drawing.Size(548, 266);
            this.qcTextEditor.TabIndex = 2;
            this.qcTextEditor.TextEditorProperties = defaultTextEditorProperties1;
            this.qcTextEditor.KeyPressEvent += new SQLEditor.Editor.TextEditorControlWrapper.KeyPressEventHandler(this.qcTextEditor_KeyPressEvent);
            this.qcTextEditor.RMouseUpEvent += new SQLEditor.Editor.TextEditorControlWrapper.MYMouseRButtonUpEventHandler(this.qcTextEditor_MouseUp);
            // 
            // ExecutionTimer
            // 
            this.ExecutionTimer.Tick += new System.EventHandler(this.ExecutionTimer_Tick);
            // 
            // ttParamenterInfo
            // 
            this.ttParamenterInfo.Active = false;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            this.printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
            // 
            // pageSetupDialog
            // 
            this.pageSetupDialog.Document = this.printDocument;
            // 
            // printDialog
            // 
            this.printDialog.Document = this.printDocument;
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Document = this.printDocument;
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // cmDragAndDrp
            // 
            this.cmDragAndDrp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemObjectName,
            this.menuItemSplitter,
            this.menuItemSelect1,
            this.menuItemSelect2,
            this.menuItemJoin,
            this.menuItemLeftOuterJoin,
            this.menuItemRightOuterJoin,
            this.menuItemWhere,
            this.menuItemOrderBy,
            this.menuItemGroupBy});
            // 
            // menuItemObjectName
            // 
            this.menuItemObjectName.Index = 0;
            this.menuItemObjectName.Text = "Object name";
            this.menuItemObjectName.Click += new System.EventHandler(this.menuItemObjectName_Click);
            // 
            // menuItemSplitter
            // 
            this.menuItemSplitter.Index = 1;
            this.menuItemSplitter.Text = "-";
            // 
            // menuItemSelect1
            // 
            this.menuItemSelect1.Index = 2;
            this.menuItemSelect1.Text = "SELECT * FROM...";
            this.menuItemSelect1.Click += new System.EventHandler(this.menuItemSelect1_Click);
            // 
            // menuItemSelect2
            // 
            this.menuItemSelect2.Index = 3;
            this.menuItemSelect2.Text = "SELECT [Fields] FROM";
            this.menuItemSelect2.Click += new System.EventHandler(this.menuItemSelect2_Click);
            // 
            // menuItemJoin
            // 
            this.menuItemJoin.Index = 4;
            this.menuItemJoin.Text = "JOIN...";
            this.menuItemJoin.Click += new System.EventHandler(this.menuItemJoin_Click);
            // 
            // menuItemLeftOuterJoin
            // 
            this.menuItemLeftOuterJoin.Index = 5;
            this.menuItemLeftOuterJoin.Text = "LEFT OUTER JOIN...";
            this.menuItemLeftOuterJoin.Click += new System.EventHandler(this.menuItemLeftOuterJoin_Click);
            // 
            // menuItemRightOuterJoin
            // 
            this.menuItemRightOuterJoin.Index = 6;
            this.menuItemRightOuterJoin.Text = "RIGHT OUTER JOIN ";
            this.menuItemRightOuterJoin.Click += new System.EventHandler(this.menuItemRightOuterJoin_Click);
            // 
            // menuItemWhere
            // 
            this.menuItemWhere.Index = 7;
            this.menuItemWhere.Text = "WHERE";
            this.menuItemWhere.Click += new System.EventHandler(this.menuItemWhere_Click);
            // 
            // menuItemOrderBy
            // 
            this.menuItemOrderBy.Index = 8;
            this.menuItemOrderBy.Text = "ORDER BY";
            this.menuItemOrderBy.Click += new System.EventHandler(this.menuItemOrderBy_Click);
            // 
            // menuItemGroupBy
            // 
            this.menuItemGroupBy.Index = 9;
            this.menuItemGroupBy.Text = "GROUP BY";
            this.menuItemGroupBy.Click += new System.EventHandler(this.menuItemGroupBy_Click);
            // 
            // FrmQuery
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(552, 270);
            this.Controls.Add(this.lstv_Commands);
            this.Controls.Add(this.qcTextEditor);
            this.Name = "FrmQuery";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Enter += new System.EventHandler(this.FrmQuery_Enter);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FrmQuery_Closing);
            this.Load += new System.EventHandler(this.FrmQuery_Load);
            this.ResumeLayout(false);

		}
		#endregion
		#endregion
		#region Events
		/// <summary>
		/// Recieves all Infomessages from executed statement
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
		{
			Console.WriteLine("***Got InfoMessage***");
			foreach (SqlError err in args.Errors)
			{
				System.Windows.Forms.Application.DoEvents();
				if(!_sqlInfoMessages.Contains(err))
					_sqlInfoMessages.Add(err);
				Console.WriteLine("\t" + err);
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (m_resetText)
			{
				m_resetText = false;
				FileName = FileName;
			}
		}

		protected override string GetPersistString()
		{
			return GetType().ToString() + "," + FileName + "," + Text;
		}

		private void FrmQuery_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MainForm frm = (MainForm)MdiParentForm;
			_canceled = false;

			if(this.FileName.Length == 0)
			{
				this.FileName = this.Text;
			}
			if(this.Text.IndexOf("*") > 0)
			{
				string msg = String.Format(Localization.GetString("FrmQuery.FrmQuery_Closing"),this.FileName);
				
				DialogResult dr = MessageBox.Show(msg,this.Text,MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
				if(dr == DialogResult.Yes)
				{
					frm.SaveDocument(this);
					syntaxReader.Save(qcTextEditor.Font);
					frm.QueryForms.Remove(this);
				}
				else if(dr == DialogResult.No)
				{
					syntaxReader.Save(qcTextEditor.Font);
					frm.QueryForms.Remove(this);
				}
				else if(dr == DialogResult.Cancel)
				{
					e.Cancel = true;
					_canceled = true;
				}
			}
		}

		private void FrmQuery_Load(object sender, System.EventArgs e)
		{
			MainForm frm = (MainForm)MdiParentForm;
			MainForm.DBConnection dbc = (MainForm.DBConnection)frm.DBConnections[0];
			dbConnection = dbc.Connection;
			if(dbConnection is SqlConnection)
				((SqlConnection)dbConnection).InfoMessage +=new SqlInfoMessageEventHandler(OnInfoMessage); 
			_OrginalName = this.Text;

			try
			{
				qcTextEditor.ShowEOLMarkers=_settings.ShowEOLMarkers;
				qcTextEditor.ShowSpaces=_settings.ShowSpaces;
				qcTextEditor.ShowTabs=_settings.ShowTabs;
				qcTextEditor.ShowLineNumbers=_settings.ShowLineNumbers;
				qcTextEditor.ShowMatchingBracket=_settings.ShowMatchingBracket;
				qcTextEditor.Font=_settings.GetFont();
				qcTextEditor.ShowVRuler=false;
				qcTextEditor.TabIndent=_settings.TabIndent;
			}
			catch
			{
				qcTextEditor.ShowEOLMarkers=false;
				qcTextEditor.ShowSpaces=false;
				qcTextEditor.ShowTabs=false;
				qcTextEditor.ShowVRuler=false;
			}
			
            ////AddPluginMenuItems();
			
		}
        private void FrmQuery_Enter(object sender, System.EventArgs e)
        {
            ////MainForm frm = (MainForm)MdiParentForm;
            ////frm.SetPandelInfo();	
        }

		private void lstv_Commands_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Tab)
				qcTextEditor.SelectedText = "\t";

			if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Back) // No update
			{
				DoInsert = false;
				lstv_Commands.Hide();
				qcTextEditor.Focus();
			}
		
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				DoInsert = true;
				lstv_Commands.Hide();
				qcTextEditor.Focus();
			}
		}

		private void lstv_Commands_Leave(object sender, System.EventArgs e)
		{
			//int fPos =TextUtilities.FindWordEnd(qcTextEditor.Document, qcTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset);
			if(lstv_Commands.SelectedItems.Count>0 && DoInsert)
			{
				//qcTextEditor.Select(firstPos,lastPos-firstPos);

				if(lstv_Commands.SelectedItems[0].Tag==null)
				{
					qcTextEditor.Document.Replace(firstPos,lastPos-firstPos,lstv_Commands.SelectedItems[0].Text);
					
					int pos = firstPos+lstv_Commands.SelectedItems[0].Text.Length;
					qcTextEditor.SetPosition(pos);
				}
				else
				{
					string s = lstv_Commands.SelectedItems[0].Tag.ToString();
					qcTextEditor.Document.Replace(firstPos,lastPos-firstPos,s);
					//qcTextEditor.SelectedText = lstv_Commands.SelectedItems[0].Tag.ToString();
				}
				lstv_Commands.Hide();

			}
			else
			{
				lstv_Commands.Hide();
				qcTextEditor.SelectionStart=lastPos;
			}
		}
		
		private void lstv_Commands_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListView lv = (ListView)sender;
			ListViewItem lvi = lv.GetItemAt(e.X, e.Y);

			if (lvi != null)
			{
				toolTip1.Active = true;
				toolTip1.SetToolTip(lv, lvi.Text);
			} 
			else
				toolTip1.Active = false; 
		}
		
		private void qcTextEditor_KeyPressEvent(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			string keyString = e.ToString();
			string line=qcTextEditor.ActiveTextAreaControl.Caret.Line.ToString();
			string col=qcTextEditor.ActiveTextAreaControl.Caret.Column.ToString();
			((MainForm)MdiParentForm).SetPandelPositionInfo(line,col);
			if (e.Alt == true && e.KeyValue == 88)
			{
				e.Handled = false;
				RunQuery();
				return;
			}
			if (e.KeyCode == Keys.Down)
				lstv_Commands.Focus();
			
			if(e.KeyCode == Keys.F5)
				RunQuery();

			if (((Control.ModifierKeys & Keys.Control) == Keys.Control) && e.KeyValue==32 
				|| e.KeyValue == 190)
			{
				if(e.KeyValue == 190)
				{
					ApplyProperty();
				}
				else
				{
					e.Handled = true;
					ComplementWord();
				}
			}
			if (((Control.ModifierKeys & Keys.Control) == Keys.Control) && e.KeyValue == 67 || e.KeyValue == 99)
			{
				ToggleComment();
			}
		}

		private void TextArea_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetData("SQLEditor.General.QCTreeNode", false) != null)
			{
				QCTreeNode node = (QCTreeNode)e.Data.GetData("SQLEditor.General.QCTreeNode", false);
				if(node.objecttype==QCTreeNode.ObjectType.Table ||
					node.objecttype==QCTreeNode.ObjectType.View ||
					node.objecttype==QCTreeNode.ObjectType.Filed)	
				{
					System.Drawing.Rectangle r = qcTextEditor.RectangleToClient(qcTextEditor.ClientRectangle);
					Point p = new Point(e.X+r.X,e.Y+r.Y);
					
					_dragPos = qcTextEditor.GetCharIndexFromPosition(p);
					_dragObject = node;
					
					string objectName=node.objectName;
					if(node.objecttype==QCTreeNode.ObjectType.Filed)
					{
						int spacePos = objectName.IndexOf(" ");
						if(spacePos>0)
							objectName =objectName.Substring(0,spacePos);

						objectName= ((QCTreeNode)node.Parent.Parent).objectName + "." + objectName;

					}
					_dragObject.objectName=objectName;
					SetDragAndDropContextMenu(node);

					foreach(MainForm.DBConnection c in ((MainForm)this.MdiParentForm).DBConnections)
					{
						if(c.ConnectionName == node.server)
						{
							SetDatabaseConnection(node.database,c.Connection);
							break;
						}
					}
					
					cmDragAndDrp.Show(qcTextEditor,p);
					
				}
				return;
			}
			if(e.Data.GetDataPresent(DataFormats.FileDrop) )
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				try
				{
					string fullName = files[0];
					if(files.Length>1)
						return;
					MainForm mainform = (MainForm)MdiParentForm;
					string fileName = Path.GetFileName(fullName);
					string line;
					string fileContent = "";

					FrmQuery frmquery = new FrmQuery(MdiParentForm);
					
					StreamReader sr = new StreamReader(fullName);
					fileContent = sr.ReadToEnd();
					sr.Close();
					sr=null;

					this.Content=fileContent;
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
			}

		}
		private void TextArea_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetData("SQLEditor.General.QCTreeNode", false) != null)
			{
				QCTreeNode node = (QCTreeNode)e.Data.GetData("SQLEditor.General.QCTreeNode", false);
				if(node.objecttype==QCTreeNode.ObjectType.Table ||node.objecttype==QCTreeNode.ObjectType.View)
					((DragEventArgs)e).Effect = DragDropEffects.Copy;
				else if(node.objecttype==QCTreeNode.ObjectType.Filed)
					((DragEventArgs)e).Effect = DragDropEffects.Copy;

				return;
			}

			if (((DragEventArgs)e).Data.GetDataPresent(DataFormats.FileDrop)) 
				((DragEventArgs)e).Effect = DragDropEffects.Copy;
			else
				((DragEventArgs)e).Effect = DragDropEffects.None; 
			
		}
		
		private void TextArea_Click(object sender, EventArgs e)
		{
			string line=qcTextEditor.ActiveTextAreaControl.Caret.Line.ToString();
			string col=qcTextEditor.ActiveTextAreaControl.Caret.Column.ToString();
			((MainForm)MdiParentForm).SetPandelPositionInfo(line,col);
		}
		private void ExecutionTimer_Tick(object sender, System.EventArgs e)
		{
			if(_asyncResult.IsCompleted)
			{
				ExecutionTimer.Enabled=false;
				HandleExecutionResult(_currentDataSet,_currentExecutionTime,_currentManager);
			}
		}
		
		private void TextArea_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch(e.KeyChar)
			{
					// Counts the backspaces.
				case '\b':
					break ;
					// Counts the ENTER keys.
				case '\r':
					break ;
					// Counts the ESC keys.  
				case (char)27:
					break ;
					// Counts all other keys.
				default:
					keyPressCount = keyPressCount + 1 ;
					if (this.Text.IndexOf("*") <= 0)
					{
						this.Text = this.Text + "*";
					}
					break;
			}
		}

		private void TextArea_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.KeyCode == Keys.Enter )
			{
				if (this.Text.IndexOf("*") <= 0)
				{
					this.Text = this.Text + "*";
				}
			}
		}
		#endregion
		#region Private Methods
		
		private static DragDropEffects GetDragDropEffect(DragEventArgs e)
		{
			if ((e.AllowedEffect & DragDropEffects.Move) > 0 &&	(e.AllowedEffect & DragDropEffects.Copy) > 0) 
			{
				return (e.KeyState & 8) > 0 ? DragDropEffects.Copy : DragDropEffects.Move;
			} 
			else if ((e.AllowedEffect & DragDropEffects.Move) > 0) 
			{
				return DragDropEffects.Move;
			} 
			else if ((e.AllowedEffect & DragDropEffects.Copy) > 0) 
			{
				return DragDropEffects.Copy;
			}
			return DragDropEffects.None;
		}

		private int ParseText(WordAndPosition[] words, string s)
		{
			words.Initialize();
			int count = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ []\f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
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
				if(words[i].Word.ToUpper()=="CREATE" && words[i+1].Word.ToUpper()==type.ToUpper())
				{
					returnString = returnString.Substring(0,words[i].Position) + "ALTER" + returnString.Substring(words[i].Position + words[i].Length, returnString.Length-(words[i].Position + words[i].Length));
					break;
				}
			}
			return returnString;
			
		}

        private string GetObjectName()
		{
			try
			{
				int wordCount = qcTextEditor.Text.Split(' ').Length;
				WordAndPosition[] words  = new WordAndPosition[wordCount];
				int count = ParseText(words,qcTextEditor.Text);
			
				for(int i=0;i<(count - 3);i++)
				{
					if((words[i].Word.ToUpper()=="CREATE" && (words[i+1].Word.ToUpper()=="PROCEDURE" || words[i+1].Word.ToUpper()=="FUNCTION" || words[i+1].Word.ToUpper()=="VIEW" || words[i+1].Word.ToUpper()=="TRIGGER")) || 
						(words[i].Word.ToUpper()=="ALTER" && (words[i+1].Word.ToUpper()=="PROCEDURE" || words[i+1].Word.ToUpper()=="FUNCTION" || words[i+1].Word.ToUpper()=="VIEW" || words[i+1].Word.ToUpper()=="TRIGGER")))
					{
						if(words[i+2].Word.ToUpper()=="DBO")
							return words[i+3].Word;
						else
							return words[i+2].Word;
					}
				}
				return "";
			}
			catch
			{
				return "";
			}
		}

		private int PreviusIndexOf(string character)
		{
			for(int i=qcTextEditor.SelectionStart;i>0;i--)
			{
				if(qcTextEditor.Text.Substring(i-1,1)==character)
				{
					return i;
				}
			}
			return 0;
		}

		private void ToggleComment()
		{
			try
			{
				int start = qcTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;

				string[] lineArray = qcTextEditor.SelectedText.Split('\n');

				if ( qcTextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(start) )
				{
					if ( lineArray.Length > 1 )
					{
						int currpos = start;
						for( int xx = 0; xx <= lineArray.Length - 1; xx++ )
						{
							if ( lineArray[xx].Length == 0 && xx == (lineArray.Length - 1) )
								break;
							if ( lineArray[xx].IndexOf("--", 0) == 0 )
							{
								qcTextEditor.Document.Replace(currpos, 2, "");
								currpos -= 2;
							}
							else
							{
								qcTextEditor.Document.Insert(currpos, "--");
								currpos += 2;
							}
							currpos += lineArray[xx].Length + 1;
						}
					}
				}
			}
			catch{return;} // probably no selected text
		}

		private void ApplyProperty()
		{	
			try
			{
				lastPos			= qcTextEditor.SelectionStart;
				string t		= qcTextEditor.Text.Substring(0,lastPos);
				int lastSpace	= t.LastIndexOf(" ");
				int lastEnter	= t.LastIndexOf("\n");
				int lastTab		= t.LastIndexOf("\t");

				firstPos = lastSpace > lastEnter  ? lastSpace : lastEnter;
				firstPos = firstPos > lastTab  ? firstPos : lastTab;
				firstPos++;
				string word = t.Substring(firstPos,t.Length-firstPos);
				int dotPos = word.IndexOf(".");
				if(dotPos>0)
				{	
					word = word.Substring(dotPos+1);
					firstPos+=dotPos+1;
				}
				ApplyProperty2(word);
			
			}
			catch{return;}
		}

		private void ApplyProperty2(string word)
		{
			try
			{
				ArrayList DatabaseObjects = null;
				if(word.Length>0)
				{
					//Clear
					foreach(ListViewItem l in lstv_Commands.Items)
						l.Remove();

					word = GetAliasTableName(word);
			
					//Properties
					IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
                    //if(DatabaseFactory.GetConnectionType(dbConnection)!=SQLEditor.Database.DBConnectionType.DB2)
						DatabaseObjects = db.GetDatabasesObjectProperties(DatabaseName,word,dbConnection);
                    //else
                    //    DatabaseObjects = db.GetDatabasesObjectProperties("",word,dbConnection);
					if(DatabaseObjects == null)
						return;
				
					if(DatabaseObjects.Count>0)
						lstv_Commands.Items.Add("*",2);

					foreach(Database.DBObjectProperties dbObjectProperties in DatabaseObjects)
					{
						string column=dbObjectProperties.Name.IndexOf("(")>0?dbObjectProperties.Name.Substring(0,dbObjectProperties.Name.IndexOf("(")):dbObjectProperties.Name;
						lstv_Commands.Items.Add(column.Trim(),3);
					}
					//					}
					lastPos++;
					firstPos = lastPos;
					if(lstv_Commands.Items.Count==0)	  //No match
						return;
					else if(lstv_Commands.Items.Count==1) //One Match
					{
						qcTextEditor.Select(firstPos,lastPos-firstPos);
						qcTextEditor.SelectedText = lstv_Commands.Items[0].Text;
					}
					else								  //Selection is required
					{
						DoInsert=true;
						int formHeight = this.Size.Height;
						int formWidth = this.Size.Width;
						lstv_Commands.Width=200;

						Point pt = qcTextEditor.GetPositionFromCharIndex(qcTextEditor.SelectionStart);
						pt.Y += qcTextEditor.Font.Height;

						if(pt.Y + lstv_Commands.Height > formHeight)
							pt.Y = pt.Y- (lstv_Commands.Height+(qcTextEditor.Font.Height/2));

						if(pt.X + lstv_Commands.Width > formWidth)
							pt.X = pt.X - lstv_Commands.Width;

						lstv_Commands.Location = pt;

						lstv_Commands.Visible = true;
						lstv_Commands.Focus();
						lstv_Commands.Items[0].Selected = true;	
					}
				}
			}
			catch{return;}
		}

		private void ComplementWord()
		{	
			try
			{
				lstv_Commands.Items.Clear();
				int textWidth = 200;
				int fac = 5;
				bool isJoining=false;
				lastPos		= qcTextEditor.SelectionStart;
				firstPos	= 0;
				ToolTip toolTip1 = new ToolTip();
				toolTip1.AutoPopDelay = 5000;
				toolTip1.InitialDelay = 1000;
				toolTip1.ReshowDelay = 500;
				toolTip1.ShowAlways = true;


				int lastSpace = PreviusIndexOf(" ");
				int lastEnter = PreviusIndexOf("\n");
				int firstTab = PreviusIndexOf("\t");

				if(lastSpace > 0)
					firstPos = lastSpace;
				if(lastEnter > firstPos)
					firstPos = lastEnter;
				if(firstTab > firstPos)
					firstPos = firstTab;

				string word		= (qcTextEditor.Text.Substring(firstPos,lastPos-firstPos));

				if(word.Length==0 && 
					qcTextEditor.Text.Substring(lastPos-3,3).ToUpper()=="ON ")
				{
					// JOINING 
					SQL.SQLStatement statement = new SQLEditor.SQL.SQLStatement(qcTextEditor.Text,qcTextEditor.SelectionStart,SQL.SQLStatement.SearchOrder.asc, DBConnectionType.MicrosoftSqlClient);
					if(DatabaseFactory.GetConnectionType(dbConnection)==DBConnectionType.MicrosoftSqlClient)
					{
						isJoining=true;

						SQLEditor.Database.Microsoft.Sql2000.DataManager db = (SQLEditor.Database.Microsoft.Sql2000.DataManager)DatabaseFactory.CreateNew(dbConnection);
						
						foreach(string join in db.GetJoiningReferences(DatabaseName, statement.CurrentTable, dbConnection, statement.Tables, statement.Aliases))
							lstv_Commands.Items.Add(join,2);
					}
				}
			
				int dotPos = word.IndexOf(".");
				if(dotPos > 0) //&& DatabaseFactory.GetConnectionType(dbConnection)!=SQLEditor.Database.DBConnectionType.DB2)
				{
					word = word.Substring(dotPos+1);
					firstPos+=dotPos+1;
				}
				
				if(word.Length==0 && !isJoining)
				{
					// Snippets
					XmlDocument xmlSnippets = new XmlDocument();
					xmlSnippets.Load(Application.StartupPath+@"\Snippets.xml");
					XmlNodeList xmlNodeList = xmlSnippets.GetElementsByTagName("snippets");

					foreach(XmlNode node in xmlNodeList[0].ChildNodes)
					{
						ListViewItem lvi = lstv_Commands.Items.Add(node.Attributes["name"].Value,5);
						string statement = node.InnerText;

						statement = statement.Replace(@"\n","\n");
						statement = statement.Replace(@"\t","\t");
						lvi.Tag=statement;
						
					}
				}
				else if(!isJoining)
				{
					//Clear
					foreach(ListViewItem l in lstv_Commands.Items)
						l.Remove();

					// Variables
					if(word.Substring(0,1)=="@")
					{
						foreach(string var in qcTextEditor.GetVariables(word))
						{
							if((var.Length*fac)>textWidth)
								textWidth=var.Length*fac;

							lstv_Commands.Items.Add(var,2);
						}
					}
					else
					{
						//Reserved Words
						foreach(XmlNode node in syntaxReader.xmlNodeList[0].ChildNodes)
						{
							if(node.Name.StartsWith(word.ToUpper()))
							{
								if((node.Name.Length*fac)>textWidth)
									textWidth=node.Name.Length*fac;
								lstv_Commands.Items.Add(node.Name,2);
							}
						}
			
						//Operations
						IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
						//						Database db = new Database();
						ArrayList DatabaseObjects = db.GetDatabasesObjects(DatabaseName,word,dbConnection);

						foreach(Database.DBObject dbObject in DatabaseObjects)
						{
							if((dbObject.Name.Length*fac)>textWidth)
								textWidth=dbObject.Name.Length*fac;
								
							//							ListViewItem lvi;
							switch(dbObject.Type.ToUpper())
							{
								case "V ": //Tables
									lstv_Commands.Items.Add(dbObject.Name,4);
									break;
								case "U ": //Tables
									lstv_Commands.Items.Add(dbObject.Name,4);
									break;
								case "P ": //Stored Procedures
									lstv_Commands.Items.Add(dbObject.Name,1);
									break;
								case "FN": //Functions
									lstv_Commands.Items.Add("dbo." + dbObject.Name,0);
									break;
								case "TF": //Functions
									lstv_Commands.Items.Add("dbo." + dbObject.Name,0);
									break;
								case "TR": //Triggers
									lstv_Commands.Items.Add("dbo." + dbObject.Name,0);
									break;
								default:
									lstv_Commands.Items.Add(dbObject.Name,2);
									break;
							}
						}
					}
				}

				if(lstv_Commands.Items.Count==0)	  //No match
					return;
				else if(lstv_Commands.Items.Count==1) //One Match
				{
					qcTextEditor.Document.Replace(firstPos,lastPos-firstPos,lstv_Commands.Items[0].Text);
					int pos = firstPos+lstv_Commands.Items[0].Text.Length;
					
					qcTextEditor.SetPosition(pos);					
				}
				else								  //Selection is required
				{
					DoInsert=true;
					int formHeight = this.Size.Height;
					int formWidth = this.Size.Width;
					lstv_Commands.Width=textWidth;

					Point pt = qcTextEditor.GetPositionFromCharIndex(qcTextEditor.SelectionStart);
					pt.Y += qcTextEditor.Font.Height;

					if(pt.Y + lstv_Commands.Height > formHeight)
						pt.Y = pt.Y- (lstv_Commands.Height+(qcTextEditor.Font.Height/2));

					if(pt.X + lstv_Commands.Width > formWidth)
						pt.X = pt.X - lstv_Commands.Width;

					lstv_Commands.Location = pt;
						
					lstv_Commands.Visible = true;
					lstv_Commands.Focus();
					lstv_Commands.Items[0].Selected = true;	
				}
				
			}
			catch{return;}
		}

		private string ParseHeaderComment()
		{
			try
			{
				string header = "";
				string[] s = qcTextEditor.Text.Split(null);
				bool objectNameIsSet = false;	
				bool parametersIdetified = false;	
				string objectName ="";
				string returns="";
				int pos;
				ArrayList objectParameters = new ArrayList();
				ArrayList words = new ArrayList();
				ArrayList referenceObjects= new ArrayList();;

				for(int i=0;i<s.Length;i++)
				{
					if ( s[i] == "" ) 
						continue ;
					else
						words.Add(s[i]);
				}

				for(int i=0;i<(words.Count - 2);i++)
				{
				
					if(words[i].ToString().ToUpper() == "PROCEDURE" || 
						words[i].ToString().ToUpper() == "FUNCTION" || 
						words[i].ToString().ToUpper() == "VIEW" ||
						words[i].ToString().ToUpper() == "TRIGGER")
					{
						if(words[i-1].ToString().ToUpper() == "CREATE" || 
							words[i-1].ToString().ToUpper() == "ALTER")
						{
							objectName = words[i+1].ToString();

							if(objectName.IndexOf("(") > -1)
								objectName = objectName.Substring(0,objectName.IndexOf("("));

							objectNameIsSet = true;

							IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
							referenceObjects = db.GetDatabasesReferencedObjects(DatabaseName,objectName,dbConnection);

						}
					}
					pos = words[i].ToString().IndexOf("@");
					if(pos>-1 && objectNameIsSet)
					{
						string datatype = words[i+1].ToString();
						if(datatype.IndexOf(",")>0)
							datatype=datatype.Substring(0,datatype.IndexOf(","));
						else if(datatype.IndexOf(")")>0)
							datatype=datatype.Substring(0,datatype.IndexOf(")"));

						objectParameters.Add("name='"+words[i].ToString().Substring(pos,words[i].ToString().Length -pos) + "' type='"+datatype.ToUpper()+"'" );
						parametersIdetified = true;
					}
					else if(words[i].ToString().ToUpper()=="RETURNS")
					{
						returns=words[i+1].ToString().ToUpper();

						if(words[i+1].ToString().IndexOf("@")>-1 && words[i+2].ToString().ToUpper()=="TABLE")
							returns="TABLE";
					
						break;
					}
					else if(parametersIdetified && words[i].ToString().ToUpper().IndexOf("AS")>-1)
						break;
				
				}


				if(qcTextEditor.Text.IndexOf("<member")==-1)
				{

					header += "/*******************************************************************************\n";
					header += "<member name='" + objectName + "'>\n";
					header += "\t<summary></summary>\n";
					header += "\t<revision author='" + SystemInformation.UserName.ToString()  + "' date='" + DateTime.Now.ToString() + "'>Created</revision>\n";

					for(int i=0;i<objectParameters.Count;i++)
						header += "\t<param " + objectParameters[i].ToString() + "></param>\n";

					header+="\t<returns>"+returns+"</returns>\n\t<usedby>\n";
				
					foreach(SQLEditor.Database.DBObject dbo in referenceObjects)
						header+="\t\t<object>" + dbo.Name + "></object>\n";


					header += "\t</usedby>\n</member>\n********************************************************************************/\n";
				}
				return header;
			}
			catch
			{
				return"";
			}
		}

		private void CopyEmbeddedResource(string resource, string filepath)
		{
			System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
			System.IO.Stream str = a.GetManifestResourceStream(resource);
			System.IO.StreamReader reader = new StreamReader(str);
			System.IO.FileStream fileStream = new FileStream(filepath,System.IO.FileMode.Create);
			System.IO.StreamWriter writer = new StreamWriter(fileStream);
			writer.WriteLine(reader.ReadToEnd());
			reader.Close();
			writer.Close();
			reader = null;
			writer = null;
		}

		private void CollectAliases(WordAndPosition[] word, int count)
		{
			for(int i=0;i<(count - 3);i++)
			{
				if((word[i].Word.ToUpper()=="JOIN" || word[i].Word.ToUpper()=="FROM"))
				{
					if(word[i+3].Word == null)
						return;
					if(!syntaxReader.IsReservedWord(word[i+2].Word) && word[i+2].Word.Length>0)
					{
						//Alias Found
						if(!Aliases.Contains(word[i+2].Word))
						{
							Aliases.Add(word[i+2].Word,word[i+1].Word);
							AliasList.Add(new Alias(word[i+2].Word,word[i+1].Word));
						}
						i=i+2;
					}
				}
			}
		}

		private int CollectAliases()
		{
			AliasList.Clear();
			Aliases.Clear();
			
			string s = qcTextEditor.Text;
			int wordCount = s.Split(' ').Length;
			WordAndPosition[] buffer = new WordAndPosition[wordCount];
			int count = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				if (count >= buffer.Length) break;
				buffer[count].Word = m.Value;
				buffer[count].Position = m.Index;
				buffer[count].Length = m.Length;
				count++;
			}

			CollectAliases(buffer, count);
			return count;
		}
		
		private XmlDocument GetRecentObjects()
		{
			ArrayList dateList = new ArrayList();
			Hashtable objectList = new Hashtable();

			if(!File.Exists(Application.StartupPath+"\\RecentObjects.xml"))
			{
				CopyEmbeddedResource("SQLEditor.Embedded.RecentObjects.xml", Application.StartupPath + "\\RecentObjects.xml");
			}
			XmlDocument doc = new XmlDocument();
			doc.Load(Application.StartupPath + "\\RecentObjects.xml");
			XmlNodeList rootNodeList = doc.GetElementsByTagName("recentobjects");

			XmlNodeList nl = doc.GetElementsByTagName("objectName");
			
			foreach(XmlNode n in nl)
			{
				dateList.Add(Convert.ToDateTime( n.Attributes["changedate"].Value));
				objectList.Add(Convert.ToDateTime( n.Attributes["changedate"].Value),n);
			}
			dateList.Sort(new DateTimeReverserClass());
			rootNodeList[0].RemoveAll();

			for(int i=0;i<dateList.Count;i++)
			{
				if(i>10)
					break;
				XmlNode newNode = (XmlNode)objectList[dateList[i]];
				rootNodeList[0].AppendChild(newNode);
			}
			return doc;
		}
	
		private void SetCurrentStatement()
		{
			try
			{
				SQL.SQLStatement statement = new SQLEditor.SQL.SQLStatement(qcTextEditor.Text, qcTextEditor.SelectionStart, SQL.SQLStatement.SearchOrder.asc, DBConnectionType.MicrosoftSqlClient);
				string s = statement.Statement;
				int start = qcTextEditor.Text.IndexOf(s);
				qcTextEditor.Select(start, s.Length);
			}
			catch{throw;}
		}

		private int FindFirstNoneCommentedOccurance(string word, string text, int startPos)
		{
			int pos = text.IndexOf(word,startPos);
			if(!IsPositionCommented(pos,text))
				return pos;
			else 
				return FindFirstNoneCommentedOccurance(word,text,pos+word.Length);

		}
		private bool IsPositionCommented(int pos,string text)
		{
			try
			{
				string pat=@"/\*(.|[\r\n])*?\*/ |--.*";
				Regex r = new Regex(pat,RegexOptions.IgnorePatternWhitespace);

				Match m;

				for (m = r.Match(text); m.Success ; m = m.NextMatch())
				{
					if(m.Index<pos && (m.Length+m.Index)>pos)
						return true;
					if(m.Index>pos)
						return false;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}

		private void HandleExecutionResult(DataSet ds, TimeSpan executionTime, IDatabaseManager db)
		{
			string rowCountString="";
			string msg="";
			MainForm frm = (MainForm)this.MdiParentForm;
			try
			{
				if(_currentException!=null)
					throw _currentException;

				if(ds != null)
				{
					if(ds.Tables.Count>0)
					{
						rowCountString="Rowcount: " + ds.Tables[0].Rows.Count.ToString() + "\tExecutiontime:" + executionTime.Hours.ToString() + ":" + executionTime.Seconds.ToString()+ ":" +executionTime.Milliseconds.ToString()+"\n\n";
						msg+=rowCountString;
					}

					#region Handling InfoMessages
					if(_sqlInfoMessages.Count>0)
					{
						if(_settings.RunWithIOStatistics)
							msg+="The option [Run Query with IO statistics] is set to on.\n";
						
						foreach (SqlError err in _sqlInfoMessages)
						{
							if(err.Message.IndexOf("Changed database context")>-1)
								continue;

							msg+=err.Message.Replace("\n"," ") + "\n";

							if(ds.Tables.Count==0)
								continue;

							int rowCount = ds.Tables[0].Rows.Count;
							// Sample: Table 'Employee'. Scan count 7, logical reads 14, physical reads 0, read-ahead reads 0.
							
							if(err.Message.IndexOf("Table ") > -1 && err.Message.IndexOf("Scan count ")>0)
							{
								int start = err.Message.IndexOf("Scan count ")+11;
								string s = err.Message.Substring(start,err.Message.IndexOf(",")-start);
								int scanCount = Convert.ToInt16(s);
								double percent = (Convert.ToDouble( scanCount)/Convert.ToDouble(rowCount))*100.0;
								if(percent >= Convert.ToDouble( this.syntaxReader.DifferencialPercentage))
								{
					
									string tableName = err.Message.Substring(7,err.Message.IndexOf("'",7)-7);
									int charIndex = qcTextEditor.GetCharIndexForTableDefenition(tableName);
									Point p = qcTextEditor.GetPositionFromCharIndex(charIndex+tableName.Length);
									p.Y = p.Y + 2;
									int bottonY = qcTextEditor.GetPositionFromCharIndex(qcTextEditor.Text.Length).Y;
									double percentPositionFormTop = Convert.ToDouble( p.Y) / Convert.ToDouble(bottonY);

									if(percent >= this.syntaxReader.DifferencialPercentage+50)
										qcTextEditor.AddInfoMessages(p,SQLEditor.Editor.TextEditorControlWrapper.InfoMessage.MessageType.Warning,percentPositionFormTop,
											"Scan count exceed " + percent.ToString() + "%\n" + err.Message);
									else
										qcTextEditor.AddInfoMessages(p,SQLEditor.Editor.TextEditorControlWrapper.InfoMessage.MessageType.Info,percentPositionFormTop,
											"Scan count exceed " + percent.ToString() + "%\n" + err.Message);
	
								}
							}
						}
						_sqlInfoMessages.Clear();
					}
					#endregion
					frm.statusBar.Panels[3].Text="";
					frm.TaskList.Visible = false;

					if(rowCountString=="" && ds.Tables.Count>0)
						rowCountString="Rowcount: " + ds.Tables[0].Rows.Count.ToString() + "\tExecutiontime:" + executionTime.Hours.ToString() + ":" + executionTime.Seconds.ToString()+ ":" +executionTime.Milliseconds.ToString() ;
								
					// Sends the result to the output window
					if(ds.Tables.Count==0)
					{
						_outPutContainer=new OutPutContainer(ds,db,"Query executed successfully\n"+msg,executionTime,false,rowCountString);
					}
					else
						_outPutContainer=new OutPutContainer(ds,db,msg,executionTime,true,rowCountString);
					SendToOutPutWindow();
				}
				qcTextEditor.Enabled=true;
			}
			catch(Exception ex)
			{
				_currentException=null;
				string error = ex.Message;
				qcTextEditor.Enabled=true;
				
				if(error=="Database cannot be null, the empty string, or string of only whitespace.")
					error += "\nRight click on a database node in the Microsoft SQL Servers window to the left. Click [Use database].";

				if(error=="ORA-00933: SQL command not properly ended")
				{
					error += "\n\n------------------- SQLEditor hint -------------------";
					error += "\nTo execute batch statements, be sure to delimit each statement with a semi colon (;).\n\n";
				}
				frm.OutputWindow.Text = "Output";
				this.Cursor = Cursors.Default;
				frm.ShowTaskWindow();
				if(ex is System.Xml.XmlException)
				{
					frm.TaskList.ApplyTask(msg + "\n\n" +"XML Exception message\n" + error);
				}
				else
					frm.TaskList.ApplyTask(msg + "\n\n" +"Server message\n" + error);

				frm.TaskList.Activate();
				frm.statusBar.Panels[3].Text="";

				if(ex.Message.IndexOf("Line")>-1)
				{
					try
					{
						int start=ex.Message.IndexOf("Line")+4;
						int length = ex.Message.IndexOf(":",start)-start;
						string line = ex.Message.Substring(start,length);
						int l = Convert.ToInt32(line);
						
						GoToLine(l);
					}
					catch
					{
						_currentException=null;
						return;	
					}
				}
				
			}	

			this.Cursor = Cursors.Default;
			_currentException=null;
			//Plugin
            ////ExecutePlugin(Common.TriggerTypes.OnAfterQueryExecution,new SQLEditor.PlugIn.Core.CallContext(dbConnection,qcTextEditor.Text,ds));

		}
		private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			char[] param = {'\n'};

			_lines = qcTextEditor.Text.Split(param);
			
			int i = 0;
			char[] trimParam = {'\r'};
			foreach (string s in _lines)
			{
				_lines[i++] = s.TrimEnd(trimParam);
			}
		}

		private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			Application.DoEvents();
			if(_printType==PrintType.output)
			{
				//print grid
				bool morepages = _gridPrinterClass.Print(e.Graphics);
				if (morepages)
				{
					e.HasMorePages = true;
				}
				return;
			}

			int x = e.MarginBounds.Left;
			int y = e.MarginBounds.Top;
			Brush brush = new SolidBrush(qcTextEditor.ForeColor);
			
			while (_linesPrinted < _lines.Length)
			{
				e.Graphics.DrawString (_lines[_linesPrinted++],
					qcTextEditor.Font, brush, x, y);
				y += 15;
				if (y >= e.MarginBounds.Bottom)
				{
					e.HasMorePages = true;
					return;
				}
			}
			
			_linesPrinted = 0;
			e.HasMorePages = false;
		}
		#endregion
		#region Public Methods
		public bool ClosingCanceled
		{
			get{ return _canceled;}
		}
		public void RefreshLineRangeColor(int firstLine, int toLine)
		{
			//qcTextEditor.SetLineRangeColor(firstLine,toLine);
		}
		public void SendToOutPutWindow()
		{
            if (_outPutContainer == null) { return; }
			MainForm frm = (MainForm)MdiParentForm;
			if( _outPutContainer.dataset.Tables.Count >0)
			{
				if( _outPutContainer.dataset.Tables.Count >1)
					frm.OutputWindow.BrowseTable(_outPutContainer.dataset,_outPutContainer.database.DataAdapter);
				else
					frm.OutputWindow.BrowseTable(_outPutContainer.dataset.Tables[0],_outPutContainer.database.DataAdapter);
			}
			else
			frm.OutputWindow.tabControl1.TabPages.Clear();
			frm.OutputWindow.AddMessage(_outPutContainer.message);
			frm.statusBar.Panels[3].Text = _outPutContainer.statusText;
			frm.OutputWindow.Activate();
			frm.TaskList.ApplyTask("Query executed successfully");
		}
		/// <summary>
		/// Openeds a new query window displaing the requested constructor (alter statement) 
		/// </summary>
		public void GoToDefenition()
		{
			this.Cursor = Cursors.WaitCursor;
			string objectName = qcTextEditor.GetCurrentWord();
            //if(DatabaseFactory.GetConnectionType(dbConnection)!=DBConnectionType.DB2)
			objectName = objectName.Substring(objectName.IndexOf(".")+1);
			if(objectName.Length==0)
			{
				MessageBox.Show("The referenced object was not found","Go to reference",MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Cursor=Cursors.Default;
				return;
			}
			MainForm frm = (MainForm)MdiParentForm;
			frm.m_FrmDBObjects.CreateConstructorString(objectName);
			this.Cursor=Cursors.Default;
		}
		/// <summary>
		/// Displays all database objects referencing selected object 
		/// Matching only objectname. Asuming, that objectname is enclosed by spaces
		/// </summary>
		public void GoToReferenceObject()
		{
			this.Cursor=Cursors.WaitCursor;
			string objectName = qcTextEditor.GetCurrentWord();
			if(objectName.IndexOf(".")>-1)
				objectName=objectName.Substring(objectName.IndexOf(".")+1);
			if(objectName.Length==0)
			{
				MessageBox.Show("The referenced object was not found","Go to reference",MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Cursor=Cursors.Default;
				return;
			}
			IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
			ArrayList DatabaseObjects = db.GetDatabasesReferencedObjectsClear(DatabaseName,objectName,dbConnection);
			this.Cursor=Cursors.Default;
			MainForm frm = (MainForm)MdiParentForm;
			frm.ResultWindow.ShowResults(DatabaseObjects,objectName);
			frm.ResultWindow.Show(dockManager);
		}
		/// <summary>
		/// Displays all database objects referencing selected keyword
		/// Matching any occurence within a text (Example sp_test -> sp_test, sp_test1,...)
		/// </summary>
		public void GoToReferenceAny()
		{
            //////this.Cursor=Cursors.WaitCursor;
            //////string objectName = qcTextEditor.GetCurrentWord();
            //////if(objectName.Length==0)
            //////{
            //////    MessageBox.Show("The referenced object was not found","Go to reference",MessageBoxButtons.OK, MessageBoxIcon.Information);
            //////    this.Cursor=Cursors.Default;
            //////    return;
            //////}

            //////IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
            ////////Database db = new Database();
            //////ArrayList DatabaseObjects = db.GetDatabasesReferencedObjects(DatabaseName,objectName,dbConnection);

            ////////FrmReferenceObjects frmReferenceObjects = new FrmReferenceObjects(DatabaseObjects,objectName);
            //////MainForm frm = (MainForm)MdiParentForm;
            //////frm.ResultWindow.ShowResults(DatabaseObjects,objectName);
            //////frm.ResultWindow.Show(dockManager);

            //////this.Cursor=Cursors.Default;
			/*if(frmReferenceObjects.ShowDialogWindow(this) ==DialogResult.OK)
			{
				MainForm frm = (MainForm)MdiParentForm;
				frm.m_FrmDBObjects.CreateConstructorString(frmReferenceObjects.ReferencedObject);
			}
			this.Cursor=Cursors.Default;*/
		}
		/// <summary>
		/// Searches for specified pattern. Called from FrmSearch
		/// </summary>
		/// <param name="pathern"></param>
		/// <param name="startPos"></param>
		/// <param name="richTextBoxFinds"></param>
		/// <returns></returns>
		public int Find(Regex regex, int startPos)
		{	
			string context= this.qcTextEditor.Text.Substring(startPos);
			Match m = regex.Match(context);
			if(!m.Success)
			{	
				MessageBox.Show("The specified text was not found.", "SQLEditor", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return 0;
			}
			int line = qcTextEditor.Document.GetLineNumberForOffset(m.Index + startPos);
			qcTextEditor.ActiveTextAreaControl.TextArea.ScrollTo(line);

			qcTextEditor.Select(m.Index + startPos, m.Length);
			_findNextRegex = regex;
			_findNextStartPos = m.Index + startPos;

			qcTextEditor.SetPosition(m.Index + m.Length + startPos);					
			return m.Index + m.Length + startPos;
		}
		/// <summary>
		/// 
		/// </summary>
		public void FindNext()
		{
			if(_findNextRegex!=null)
				Find(_findNextRegex,_findNextStartPos+1);
		}
		/// <summary>
		/// Searches for specified pattern and replaces it. Called from FrmSearch
		/// </summary>
		/// <param name="pathern"></param>
		/// <param name="startPos"></param>
		/// <param name="richTextBoxFinds"></param>
		/// <returns></returns>
		public int Replace(Regex regex, int startPos, string replaceWith)
		{	
			if(qcTextEditor.SelectedText.Length>0)
			{
				int start=qcTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
				//int start = qcTextEditor.SelectionStart;
				int length = qcTextEditor.SelectedText.Length;
				qcTextEditor.Document.Replace(start, length,replaceWith);
				
				return Find(regex,length+start);

			}

			string context= this.qcTextEditor.Text.Substring(startPos);

			Match m = regex.Match(context);
			
			if(!m.Success)
			{	
				MessageBox.Show("The specified text was not found.","SQLEditor",MessageBoxButtons.OK, MessageBoxIcon.Information);
				return 0;
			}
			qcTextEditor.Document.Replace(m.Index+startPos, m.Length,replaceWith);
			
			return m.Index+m.Length+startPos;

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="regex"></param>
		/// <param name="replaceWith"></param>
		public void ReplaceAll(Regex regex, string replaceWith)
		{	
			string context= this.qcTextEditor.Text;

			this.qcTextEditor.Text  = regex.Replace(this.qcTextEditor.Text,replaceWith);

		}
		/// <summary>
		/// Calls FrmGotoLine
		/// </summary>
		public void GoToLine()
		{
			int lineNumber = qcTextEditor.GetLineFromCharIndex(qcTextEditor.SelectionStart);
			// Fix Goto Bug
			lineNumber++;
			FrmGotoLine frmGotoLine = new FrmGotoLine(this,lineNumber,qcTextEditor.Document.LineSegmentCollection.Count);
			frmGotoLine.Show();
		}
		/// <summary>
		/// Sets cursor to requested line
		/// </summary>
		/// <param name="line"></param>
		public void GoToLine(int line)
		{
		    // Fix Goto Bug
			int offset = qcTextEditor.Document.GetLineSegment(line - 1).Offset;
			int length = qcTextEditor.Document.GetLineSegment(line - 1).Length;
			qcTextEditor.ActiveTextAreaControl.TextArea.ScrollTo(line - 1);
			if(length==0)
				length++;
			qcTextEditor.Select(offset,length);
			qcTextEditor.SetLine(line - 1);					
		}	
		/// <summary>
		/// Returns the name of requested alias
		/// </summary>
		/// <param name="alias"></param>
		/// <returns></returns>
		public string GetAliasTableName(string alias)
		{
			SQL.SQLStatement statement = new SQLEditor.SQL.SQLStatement(qcTextEditor.Text,qcTextEditor.SelectionStart,SQL.SQLStatement.SearchOrder.asc, DBConnectionType.MicrosoftSqlClient);
			return statement.GetAliasTableName(alias);
		}
		/// <summary>
		/// Sets [dbConnection] and [DatabaseName]
		/// </summary>
		/// <param name="dbName"></param>
		/// <param name="conn"></param>
		public void SetDatabaseConnection(string dbName, IDbConnection conn)
		{
            MainForm frm = (MainForm)MdiParentForm;
            string lcTempvar;
            lcTempvar = "";
            string lcCo_Name = frm.SetCaption(dbName);
            lcTempvar = (dbName == "VUDYOG" ? "Udyog Database" : dbName);
            lcCo_Name = (lcCo_Name != "" ? lcCo_Name : lcTempvar);
            this.Text = _OrginalName + " [" + lcCo_Name + "]"; 
			DatabaseName = dbName;
			dbConnection = conn;
            string highLightingStragegy = DatabaseFactory.ChangeDatabase(conn,dbName);
			DatabaseName = dbName;
			qcTextEditor.SetHighLightingStragegy(highLightingStragegy);
			frm.SetPandelInfo();
		}

		/// <summary>
		/// This is where it happens...
		/// </summary>
		public void RunQuery()
		{
			MainForm frm = (MainForm)MdiParentForm;
            
			string msg="";
			this.Cursor = Cursors.WaitCursor;
			string SQLstring="";
			DataSet ds =null;
            frm.statusBar.Panels[3].Text="Executing query...";
			try
			{
				// Handling InfoMessages
				qcTextEditor.ClearInfoMessages();

                /// Handling Comment header
				ComplementHeader();
				
				frm.OutputWindow.Text = "Output";
				
				// Resets exception underlining
				if(qcTextEditor.SelectedText.Length>1)
				{
					SQLstring = qcTextEditor.SelectedText;
				}
				else
				{
					SQLstring = qcTextEditor.Text;
				}

				IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);

				/********************************************
					* The Query is expected to return a dataset 
					********************************************/
				db.ExecuteCommand("SET SHOWPLAN_ALL OFF\nGO", dbConnection, this.DatabaseName);
				db.ExecuteCommand("SET NOEXEC OFF\nGO", dbConnection, this.DatabaseName);
                db.ExecuteCommand("SET DATEFORMAT DMY\nGO", dbConnection, this.DatabaseName);
				//SQLstring = "SET SHOWPLAN_ALL OFF\nGO\nSET NOEXEC OFF\nGO\n" + SQLstring;
				// RunWithIOStatistics hits the user about none efficient queries

                //////if(_settings.Exists() && SQLstring.IndexOf("SHOWPLAN_ALL ON") <= 0)
                //////{
                //////    if(_settings.RunWithIOStatistics && GetObjectName().Length==0)
                //////        SQLstring = "SET STATISTICS IO ON\n" + SQLstring + "\nSET STATISTICS IO OFF";
                //////}
				
				DateTime dt = DateTime.Now;		
				
				_currentManager = db;
				qcTextEditor.Enabled=false;
				RunAsyncCallDelegate msc = new RunAsyncCallDelegate(RunAsyncCall);
				AsyncCallback cb = new AsyncCallback(RunAsyncCallback);
				_asyncResult = msc.BeginInvoke(SQLstring, db, dbConnection, DatabaseName, cb, null);
				ExecutionTimer.Enabled=true;
				return;
			}
			catch(Exception ex)
			{
				string error = ex.Message;
				
				if(error=="Database cannot be null, the empty string, or string of only whitespace.")
					error += "\nRight click on a database node in the Microsoft SQL Servers window to the left. Click [Use database].";

				frm.OutputWindow.Text = "Output";
				this.Cursor = Cursors.Default;
				frm.ShowTaskWindow();
				if(ex is System.Xml.XmlException)
				{
					frm.TaskList.ApplyTask(msg + "\n\n" +"XML Exception message\n" + error);
				}
				else
					frm.TaskList.ApplyTask(msg + "\n\n" +"Server message\n" + error);

				frm.TaskList.Activate();
				frm.statusBar.Panels[3].Text="";

				if(ex.Message.IndexOf("Line")>-1)
				{
					try
					{
						int start=ex.Message.IndexOf("Line")+4;
						int length = ex.Message.IndexOf(":",start)-start;
						string line = ex.Message.Substring(start,length);
						int l = Convert.ToInt32(line);
						GoToLine(l);
					}
					catch
					{
						return;	
					}
				}
				
			}
			this.Cursor = Cursors.Default;
		}
		public void StopCurrentExecution()
		{
			qcTextEditor.Enabled=true;
			if(_currentManager==null)
				return;
			if(!_currentManager.StopExecuting())
				MessageBox.Show("Unable to stop execution");
			else
				MessageBox.Show("Execution terminated.");
		}
		/// <summary>
		/// Selects current line before calling RunQuery
		/// </summary>
		public void RunQueryLine()
		{
			Point pt = qcTextEditor.GetPositionFromCharIndex(qcTextEditor.SelectionStart);
			pt.X=0;
			int lineStartPosition = qcTextEditor.GetCharIndexFromPosition(pt);
			int lineEndPosition = qcTextEditor.Text.IndexOf("\n",lineStartPosition);
			if(lineEndPosition==-1)
				lineEndPosition=qcTextEditor.Text.Length;

			qcTextEditor.Select(lineStartPosition, lineEndPosition - lineStartPosition);
			RunQuery();
		}
		/// <summary>
		/// Selects current query before calling RunQuery
		/// </summary>
		public void RunCurrentQuery()
		{
			try
			{
				SetCurrentStatement();
				RunQuery();
			}
			catch {return;}
            try
            {
                if (qcTextEditor.SelectedText.Length > 0)
                {
                    SetCurrentStatement();
                }
                RunQuery();
            }
            catch { return; }
		}
		/// <summary>
		/// Generates insert statements based on current query
		/// </summary>
		public void CreateInsertStatement()
		{
			MainForm frm = (MainForm)MdiParentForm;
			string SQLstring;
			try
			{
				qcTextEditor.SuspendLayout();
                if (qcTextEditor.SelectedText.Length > 1)
                {
                    SQLstring = qcTextEditor.SelectedText;
                }
                else
                {
                    SQLstring = qcTextEditor.Text;
                }
				frm.NewQueryform();
				qcTextEditor.ResumeLayout();
				CreateInsertStatement(SQLstring);
			}
			catch(Exception ex)
			{
				frm.ShowTaskWindow();
				frm.TaskList.ApplyTask("Invalid SQL\n" + ex.Message);
				frm.TaskList.Activate();
			}
		}
		public void CreateInsertStatement(string SQLstring)
		{
			string Result = "";
			MainForm frm = (MainForm)MdiParentForm;
			try
			{
				qcTextEditor.SuspendLayout();			
				IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
				Result = db.GetInsertStatements(SQLstring, dbConnection, DatabaseName);
				if(Result.Length>0)
				{
					frm.ActiveQueryForm.SetDatabaseConnection(this.DatabaseName, this.dbConnection);
					frm.ActiveQueryForm.Content = Result;
				}
				qcTextEditor.ResumeLayout();
			}
			catch(Exception ex)
			{
				frm.ShowTaskWindow();
				frm.TaskList.ApplyTask("Invalid SQL\n" + ex.Message);
				frm.TaskList.Activate();
			}
		}
		/// <summary>
		/// Generates update statements based on current query
		/// </summary>
		public void CreateUpdateStatement()
		{
			MainForm frm = (MainForm)MdiParentForm;
			string SQLstring;

			try
			{
				qcTextEditor.SuspendLayout();			
				if(qcTextEditor.SelectedText.Length>1)
					SQLstring = qcTextEditor.SelectedText;
				else
					SQLstring = qcTextEditor.Text;
				qcTextEditor.ResumeLayout();

				frm.NewQueryform();

				CreateUpdateStatement(SQLstring);
			}
			catch(Exception ex)
			{
				frm.ShowTaskWindow();
				frm.TaskList.ApplyTask("Invalid SQL\n" + ex.Message);
				frm.TaskList.Activate();
			}
		}
		/// <summary>
		/// Generates update statements based on current query
		/// </summary>
		public void CreateUpdateStatement(string SQLstring)
		{
			string Result = "";
			MainForm frm = (MainForm)MdiParentForm;
			try
			{
				qcTextEditor.SuspendLayout();
				IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
				Result = db.GetUpdateStatements(SQLstring,dbConnection,DatabaseName);
				if(Result.Length>0)
				{
					frm.ActiveQueryForm.SetDatabaseConnection(this.DatabaseName, this.dbConnection);
					frm.ActiveQueryForm.Content = Result;
				}
				qcTextEditor.ResumeLayout();
			}
			catch(Exception ex)
			{
				frm.ShowTaskWindow();
				frm.TaskList.ApplyTask("Invalid SQL\n" + ex.Message);
				frm.TaskList.Activate();
			}
		}
		/// <summary>
		/// Undo next action in the undo buffer
		/// </summary>
		public void Undo()
		{
			qcTextEditor.UndoAction();
		}
		/// <summary>
		/// 
		/// </summary>
		public void Paste()
		{
			qcTextEditor.Paste();
		}
		/// <summary>
		/// 
		/// </summary>
		public void Copy()
		{
			qcTextEditor.Copy();
		}
		/// <summary>
		/// 
		/// </summary>
		public void Cut()
		{
			qcTextEditor.Cut();
		}
		/// <summary>
		/// Adds a Comment header
		/// </summary>
		public void InsertHeader()
		{
			qcTextEditor.SuspendLayout();
			string header = ParseHeaderComment();
			qcTextEditor.Text = header+qcTextEditor.Text; 
			qcTextEditor.ResumeLayout();
		}
		/// <summary>
		/// Alters the comment header with a revision tag
		/// </summary>
		public void AddRevisionCommentSection()
		{
			int startpos = qcTextEditor.Text.IndexOf("</member>",0);
			if(startpos<1)
				return;
			startpos = qcTextEditor.Text.LastIndexOf("</revision>") + 11;
			qcTextEditor.Text = qcTextEditor.Text.Substring(0,startpos) + "\n\t<revision author='" + SystemInformation.UserName.ToString()  + "' date='" + DateTime.Now.ToString() + "'>Altered</revision>" + qcTextEditor.Text.Substring(startpos);
			qcTextEditor.Refresh();
		
		}
		/// <summary>
		/// Consolidates all xml comment headers 
		/// </summary>
		public void GetXmlDocFile()
		{
			string whereConcitions="";
			FrmAlterDocumentationOutput frm = new FrmAlterDocumentationOutput();
			frm.ShowDialog(this);
			if(frm.DialogResult==DialogResult.OK)
			{
				if(frm.chbView.Checked)
					whereConcitions="'V'";
				if(frm.chbSP.Checked)
					if(whereConcitions.Length>0)
						whereConcitions+=",'P'";
					else
						whereConcitions="'P'";
				if(frm.chbFn.Checked)
					if(whereConcitions.Length>0)
						whereConcitions+=",'FN','TF'";
					else
						whereConcitions="'FN','TF'";
				if(frm.txtLike.Text.Length>0)
				{
					string like = frm.txtLike.Text.Replace("*","%");
					whereConcitions += ") AND (o.name like '"+like+"'";
				}
			}
			
			this.Cursor = Cursors.WaitCursor;
			string doc = "<?xml version='1.0' encoding='UTF-8'?>\n<!-- Generated by SQLEditor-->\n<?xml-stylesheet type='text/xsl' href='doc.xsl'?>\n<members>\n";
			XmlDocument xmlDoc = new XmlDocument();	
			try
			{
				IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
				doc += db.GetXmlDoc(DatabaseName,dbConnection,whereConcitions) + "\n</members>";
				
				xmlDoc.LoadXml(doc);
			}
			catch(Exception ex)
			{
				int startpos = ex.Message.IndexOf("Line ") + 5;
				int endpos = ex.Message.IndexOf(",",startpos);
				int line = Convert.ToInt16( ex.Message.Substring(startpos, endpos-startpos));
				FrmXMLError frmXMLError = new FrmXMLError(ex.Message, doc,line);
				frmXMLError.ShowDialog(this);
				return;
			}


			// TODO:  create common file same method or class
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.DefaultExt="xml";
			saveFileDialog.FileName = DatabaseName + " Procedures";
			saveFileDialog.Title = "Save Documentation file";
			saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"  ;

			DialogResult result = saveFileDialog.ShowDialog();
			if(result == DialogResult.OK) 
			{
				string saveFileName = saveFileDialog.FileName;
				try
				{
					xmlDoc.Save(saveFileName);

				} 
				catch(Exception exp)
				{
					MessageBox.Show("An error occurred while attempting to save the file. The error is:" 
						+ System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
					return;
				}
				string xslPath = saveFileName.Substring(0,saveFileName.LastIndexOf("\\"));
				CopyEmbeddedResource("SQLEditor.Embedded.doc.xsl", xslPath + "\\doc.xsl");
				System.Diagnostics.Process.Start("IExplore.exe",saveFileName);
				this.Cursor = Cursors.Default;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetCurrentWord()
		{
			return qcTextEditor.GetCurrentWord();
		}
		/// <summary>
		/// Save query statement to file
		/// </summary>
		/// <param name="path"></param>
		public void SaveAs(string path)
		{
			try
			{
				// remove readonly attribute
				FileInfo fi = new FileInfo(path);
				if ( fi.Exists && ((fi.Attributes & System.IO.FileAttributes.ReadOnly) != 0) )
				{
					DialogResult dr = MessageBox.Show("Overwrite read-only file?", path, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if(dr==DialogResult.Yes)
					{
						fi.Attributes -= System.IO.FileAttributes.ReadOnly;
						fi.Delete();
					}
					else
					{
						return;
					}
				}
				qcTextEditor.SaveFile(path);
				FileName = path;
			}
			catch( Exception ex)
			{
				MessageBox.Show ( string.Format("Errors Ocurred\n{0}", ex.Message), "Save Error", MessageBoxButtons.OK );
			}
		}	
		/// <summary>
		/// Gives the user the option to alter the header
		/// </summary>
		public void ComplementHeader()
		{
			string header="";
			if(qcTextEditor.SelectedText.Length>1)
				return;
			try
			{
				SQLEditor.Config.Settings settings = SQLEditor.Config.Settings.Load();
				if(settings.Exists())
				{
                    if (!settings.ShowFrmDocumentHeader)
                    {
                        return;
                    }
				}
				int start = qcTextEditor.Text.IndexOf("<member");
				int end = qcTextEditor.Text.IndexOf("</member>");
				if(start>-1 && end >-1)
				{
					end += 9; //Add length of </member>
					header = qcTextEditor.Text.Substring(start, end-start);
					FrmDocumentHeader frm = new FrmDocumentHeader(header);
					if(frm.ShowDialogWindow(this)==DialogResult.OK)
					{
						qcTextEditor.Text = qcTextEditor.Text.Replace(header,frm.Header);
						qcTextEditor.Refresh();
						
					}
					XmlDocument _doc = new XmlDocument();
					_doc.LoadXml(header);
					XmlNodeList nList =  _doc.GetElementsByTagName("member");
					XmlNode n = nList[0].Attributes.GetNamedItem("name");
				}
			}
			catch(System.Xml.XmlException ex)
			{
				int pos=0;
				for(int i=0;i<ex.LineNumber-1;i++)
				{
					pos = header.IndexOf("\n",pos+1);
				}

				string lineText = header.Substring(pos,ex.LinePosition) + "<-\n\n\tMake sure the text in well formated\n\nref: http://www.w3c.org\n";
				
				XmlException xmlEx = new XmlException(ex.Message + "\n" + lineText,ex.InnerException,ex.LineNumber,ex.LinePosition);
				
				throw xmlEx;
			}

		}
		public void GetCreateTablesScriptFromXMLFile()
		{
			try
			{
				FrmChooseXMLFile frm = new FrmChooseXMLFile();
				frm.rbData.Checked=false;
				frm.rbStructure.Checked=true;
				if(frm.ShowDialogWindow(this)==DialogResult.OK)
				{
					string file = frm.FileName;
					bool createKeys = frm.CreateKeys;
					XmlTextReader reader = new XmlTextReader(file);
					XMLDatabase xmlDatabase = new XMLDatabase(reader);
					string script="";

					if(frm.rbStructure.Checked)
					{
						script = xmlDatabase.GetDatabaseSQLScript(createKeys);
					}
					else
					{
						script = xmlDatabase.GetInsertScript(createKeys);
					}
					this.Content = script;
				}
			}
			catch(Exception ex)
			{
				if(ex.Message=="XmlContainsAttributes")
				{
					MessageBox.Show("SQLEditor XML-import only support XmlElement in this version\n\nSample:\n<PERSON>\n\t<FIRSTNAME>John</FIRSTNAME>\n\t<LASTNAME>Smith</LASTNAME>\n</PERSON>","XMLAttributes not supported",MessageBoxButtons.OK,MessageBoxIcon.Information);
					return;
				}
				throw;
			}
		}
		/// <summary>
		/// xml2Data - Import data
		/// </summary>
		public void GetInsertScriptFromXMLFile()
		{
			try
			{
				FrmChooseXMLFile frm = new FrmChooseXMLFile();
				frm.rbData.Checked=true;
				frm.rbStructure.Checked=false;

				if(frm.ShowDialogWindow(this)==DialogResult.OK)
				{
					string file = frm.FileName;
					bool createKeys = frm.CreateKeys;
					XmlTextReader reader = new XmlTextReader(file);
					XMLDatabase xmlDatabase = new XMLDatabase(reader);
					string script="";

					if(frm.rbStructure.Checked)
					{
						script = xmlDatabase.GetDatabaseSQLScript(createKeys);
					}
					else
					{
						script = xmlDatabase.GetInsertScript(createKeys);
					}

					this.Content = script;
				}
			}
			catch
			{
				throw;
			}
		}
		public void PrintStatement(bool preview)
		{
			_printType = PrintType.Statement;
			if(preview)
			{
				printPreviewDialog.ShowDialog();
				return;
			}
			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				printDocument.Print();
			}
		}
        public void PrintOutPut(bool preview)
		{
			try
			{
				_printType = PrintType.output;
				TabPage tc = ((MainForm) this.MdiParentForm).OutputWindow.tabControl1.TabPages[0];

				for(int controlIndex=0;controlIndex<tc.Controls.Count;controlIndex++)
				{
					if(tc.Controls[controlIndex] is DataGrid)
					{
						DataGrid dg = (DataGrid)tc.Controls[controlIndex];
					
						_gridPrinterClass = new GridPrinterClass(printDocument,dg);
					
						if(preview)
						{
							printPreviewDialog.ShowDialog();
							return;
						}
						else
							printDocument.Print();
					}
				}
				_gridPrinterClass=null;
			}
			catch
			{return;}
			 
		}
		public void PrintPageSetUp()
		{
			pageSetupDialog.ShowDialog();
		}	
		#endregion
		#region Context menu
		private void miCopy_Click(object sender, System.EventArgs e)
		{
			this.Copy();
		}
		private void miCut_Click(object sender, System.EventArgs e)
		{
			this.qcTextEditor.Cut();
		}
		private void miPaste_Click(object sender, System.EventArgs e)
		{
			this.Paste();
		}
		private void miGoToDefinision_Click(object sender, System.EventArgs e)
		{
			this.GoToDefenition();
		}
		private void miGoToRererence_Click(object sender, System.EventArgs e)
		{
			this.GoToReferenceObject();
		}
		private void miGoToAnyRererence_Click(object sender, System.EventArgs e)
		{
			this.GoToReferenceAny();
		}
		private void miOptions_Click(object sender, System.EventArgs e)
		{
			MainForm frm = (MainForm)MdiParentForm;
			
			FrmOption frmOption = new FrmOption(frm);
			frmOption.ShowDialog();

		}
		private void miAddToSnippet_Click(object sender, System.EventArgs e)
		{
            ////FrmAddToSnippet frm = new FrmAddToSnippet(qcTextEditor.Text);
            ////frm.ShowDialogWindow(this);

		}
		private void miSnippet_Click(object sender, System.EventArgs e)
		{
			string statement = ((SnippetMenuItem)sender).statement;

			statement = statement.Replace(@"\n","\n");
			statement = statement.Replace(@"\t","\t");

			int cursorPos = qcTextEditor.SelectionStart;

			qcTextEditor.Document.Replace(cursorPos,0,statement);

			if(statement.IndexOf("{}")>-1)
				cursorPos = cursorPos + statement.IndexOf("{}")+1;
					
			qcTextEditor.SetPosition(cursorPos);
			qcTextEditor.Refresh();
		}
		
		private void miRunCurrentQuery_Click(object sender, System.EventArgs e)
		{
			this.RunCurrentQuery();
		}

		private void miValidateCurrentQuery_Click(object sender, EventArgs e)
		{
			qcTextEditor.ResumeLayout();
			string contentHolder = this.Content;
			if (qcTextEditor.SelectedText.Length > 0)
			{
				string validate = "SET NOEXEC ON\n\nGO\n\n" + qcTextEditor.SelectedText + "\n\nGO\n\nSET NOEXEC OFF\n\nGO\n\n";
				int len = validate.Length;
				int pos = this.Content.IndexOf(qcTextEditor.SelectedText);
				if (this.Content.IndexOf("SET NOEXEC ON",0) < 0 && pos >= 0 && len > 0)
				{	
					this.Content = this.Content.Replace(qcTextEditor.SelectedText, validate);
					qcTextEditor.Select(pos, len);
				}
			}
			else
			{
				this.Content = "SET NOEXEC ON\n\nGO\n\n" + this.Content + "\n\nGO\n\nSET NOEXEC OFF\n\nGO\n\n";
			}
			this.RunQuery();
			this.Content = contentHolder;
			qcTextEditor.ResumeLayout();
		}

		private void qcTextEditor_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				string word = qcTextEditor.GetCurrentWord();
				IDataObject iData = Clipboard.GetDataObject();
				MenuItem miRunCurrentQuery = null;
				MenuItem miValidateCurrentQuery = null;
				
				MenuItem miCopy = new MenuItem("&Copy");
				MenuItem miCut = new MenuItem("C&ut");
				MenuItem miPaste = new MenuItem("&Paste");
				MenuItem miSeparator = new MenuItem("-");
                MenuItem miGoToDefinision = new MenuItem("Go to &Definition");
                MenuItem miGoToRererence = new MenuItem("Go to Object &Reference");
                MenuItem miGoToAnyRererence = new MenuItem("Go to an&y Reference");
                MenuItem miSeparator2 = new MenuItem("-");
				if (qcTextEditor.SelectedText.Length > 0)
				{
					miRunCurrentQuery = new MenuItem("Run &current selection");
					miValidateCurrentQuery = new MenuItem("&Validate current selection");
				}
				else
				{
					miRunCurrentQuery = new MenuItem("Run &current query");
					miValidateCurrentQuery = new MenuItem("&Validate current query");
				}
				MenuItem miSeparator3 = new MenuItem("-");
				MenuItem miOptions = new MenuItem("&Options");
                MenuItem miSeparator4 = new MenuItem("-");
                MenuItem miSnippets = new MenuItem("&Snippets");
                MenuItem miAddToSnippets = new MenuItem("&Add to snippets");

				// Events				
				miCopy.Click += new System.EventHandler(this.miCopy_Click);
				miCut.Click += new System.EventHandler(this.miCut_Click);
				miPaste.Click += new System.EventHandler(this.miPaste_Click);
                miGoToDefinision.Click += new System.EventHandler(this.miGoToDefinision_Click);
                miGoToRererence.Click += new System.EventHandler(this.miGoToRererence_Click);
                miGoToAnyRererence.Click += new System.EventHandler(this.miGoToAnyRererence_Click);
				miRunCurrentQuery.Click += new System.EventHandler(this.miRunCurrentQuery_Click);
				miValidateCurrentQuery.Click += new EventHandler(miValidateCurrentQuery_Click);
				miOptions.Click += new System.EventHandler(this.miOptions_Click);
                miAddToSnippets.Click += new System.EventHandler(this.miAddToSnippet_Click);

				if(!iData.GetDataPresent(DataFormats.Text))
					miPaste.Enabled=false;

				// Clear all previously added MenuItems.
				cmShortcutMeny.MenuItems.Clear();
 
				cmShortcutMeny.MenuItems.Add(miCopy);
				cmShortcutMeny.MenuItems.Add(miCut);
				cmShortcutMeny.MenuItems.Add(miPaste);
				cmShortcutMeny.MenuItems.Add(miSeparator);
                cmShortcutMeny.MenuItems.Add(miGoToDefinision);
                cmShortcutMeny.MenuItems.Add(miGoToRererence);
                cmShortcutMeny.MenuItems.Add(miGoToAnyRererence);
                cmShortcutMeny.MenuItems.Add(miSeparator2);
				cmShortcutMeny.MenuItems.Add(miRunCurrentQuery);
				cmShortcutMeny.MenuItems.Add(miValidateCurrentQuery);
				cmShortcutMeny.MenuItems.Add(miSeparator3);
				cmShortcutMeny.MenuItems.Add(miOptions);
                cmShortcutMeny.MenuItems.Add(miSeparator4);
                cmShortcutMeny.MenuItems.Add(miSnippets);


				// Snippets
				XmlDocument xmlSnippets = new XmlDocument();
				xmlSnippets.Load(Application.StartupPath+@"\Snippets.xml");
				XmlNodeList xmlNodeList = xmlSnippets.GetElementsByTagName("snippets");

				if(qcTextEditor.SelectedText.Length>1)
					miSnippets.MenuItems.Add(miAddToSnippets);

				foreach(XmlNode node in xmlNodeList[0].ChildNodes)
				{
					SnippetMenuItem snippet = new SnippetMenuItem();
					snippet.Text = node.Attributes["name"].Value;
					snippet.statement = node.InnerText;
					snippet.Click+= new System.EventHandler(this.miSnippet_Click);

					miSnippets.MenuItems.Add(snippet);
				}

				cmShortcutMeny.Show(qcTextEditor,new Point(e.X,e.Y));
				
			}
		
		}
		#endregion
		#region Drag & Drop Context menu
		private void SetDragAndDropContextMenu(QCTreeNode node)
		{
			foreach(MenuItem mi in cmDragAndDrp.MenuItems)
				mi.Visible=false;
			
			menuItemObjectName.Visible=true;
			menuItemObjectName.Text=node.objectName;
			menuItemSplitter.Visible=true;
			
			SQLEditor.SQL.SQLStatement sqlStatement= new SQLEditor.SQL.SQLStatement(qcTextEditor.Text,qcTextEditor.SelectionStart,SQLEditor.SQL.SQLStatement.SearchOrder.asc, DBConnectionType.MicrosoftSqlClient);
			Chris.Beckett.MenuImageLib.MenuImage menuExtender = new Chris.Beckett.MenuImageLib.MenuImage();
			menuExtender.ImageList=imageList1;

			

			if(node.objecttype==QCTreeNode.ObjectType.Table ||
				node.objecttype==QCTreeNode.ObjectType.View)
			{
				menuItemSelect1.Visible=true;
				menuItemSelect2.Visible=true;
				menuItemJoin.Visible=true;
				menuItemLeftOuterJoin.Visible=true;
				menuItemRightOuterJoin.Visible=true;
				menuExtender.SetMenuImage(menuItemSelect1,"2");
				menuExtender.SetMenuImage(menuItemSelect2,"2");
				menuExtender.SetMenuImage(menuItemJoin,"2");
				menuExtender.SetMenuImage(menuItemLeftOuterJoin,"2");
				menuExtender.SetMenuImage(menuItemRightOuterJoin,"2");

				menuExtender.SetMenuImage(menuItemObjectName,"4");
			}
			else if(node.objecttype==QCTreeNode.ObjectType.Filed)
			{
				if(sqlStatement.Statement.ToUpper().IndexOf("WHERE")>=0)
					menuItemWhere.Text="AND "+node.objectName;
				else
					menuItemWhere.Text="WHERE "+node.objectName;

				menuItemWhere.Visible=true;
				menuItemOrderBy.Visible=true;
				menuItemGroupBy.Visible=true;
				menuExtender.SetMenuImage(menuItemWhere,"2");
				menuExtender.SetMenuImage(menuItemOrderBy,"2");
				menuExtender.SetMenuImage(menuItemGroupBy,"2");

				menuExtender.SetMenuImage(menuItemObjectName,"3");
			
			}

		}
		private void SetDragAndDropMenuIcons()
		{
			Chris.Beckett.MenuImageLib.MenuImage menuExtender = new Chris.Beckett.MenuImageLib.MenuImage();
			menuExtender.ImageList=imageList1;

			menuExtender.SetMenuImage(menuItemObjectName,"6");


		
		}
		private void menuItemSelect1_Click(object sender, System.EventArgs e)
		{
			string text = "SELECT *\nFROM\t"+_dragObject.objectName + "\n";
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
		}

		private void menuItemSelect2_Click(object sender, System.EventArgs e)
		{
			string columns=String.Empty;

			IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection);
			ArrayList databaseObjects = db.GetDatabasesObjectProperties(DatabaseName,_dragObject.objectName,dbConnection);
			if(databaseObjects == null)
				columns="*";
			else
			{
				foreach(SQLEditor.Database.DBObjectProperties col in databaseObjects)
				{
					if(col.Name.IndexOf("(")>-1)
						col.Name=col.Name.Substring(0,col.Name.IndexOf("("));

					if(col.Name.Trim().IndexOf("[")<0 &&
						(DatabaseFactory.GetConnectionType(dbConnection)== DBConnectionType.MicrosoftSqlClient ||
						DatabaseFactory.GetConnectionType(dbConnection)== DBConnectionType.MicrosoftOleDb))
						columns+="["+col.Name.Trim()+"], ";
					else
						columns+=col.Name.Trim()+", ";
				}
				columns=columns.Substring(0,columns.Length-2);
			
			}

			string text = "SELECT "+columns+"\nFROM\t"+_dragObject.objectName + "\n";
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
		}
		private void menuItemJoin_Click(object sender, System.EventArgs e)
		{
			string text = "JOIN\t"+_dragObject.objectName+" ON "+_dragObject.objectName+". ";
			int pos = qcTextEditor.SelectionStart;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
			firstPos=pos+text.Length-2;
			lastPos=pos+text.Length-2;
			ApplyProperty2(_dragObject.objectName);
		}

		private void menuItemLeftOuterJoin_Click(object sender, System.EventArgs e)
		{
			string text = "LEFT OUTER JOIN\t"+_dragObject.objectName+" ON "+_dragObject.objectName+". ";
			int pos = qcTextEditor.SelectionStart;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
			firstPos=pos+text.Length-2;
			lastPos=pos+text.Length-2;
			ApplyProperty2(_dragObject.objectName);
		}

		private void menuItemRightOuterJoin_Click(object sender, System.EventArgs e)
		{
			string text = "RIGHT OUTER JOIN\t"+_dragObject.objectName+" ON "+_dragObject.objectName+". ";
			int pos = qcTextEditor.SelectionStart;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
			firstPos=pos+text.Length-2;
			lastPos=pos+text.Length-2;
			ApplyProperty2(_dragObject.objectName);
		}

		private void menuItemObjectName_Click(object sender, System.EventArgs e)
		{
			
			string text = _dragObject.objectName;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);

		}

		private void menuItemWhere_Click(object sender, System.EventArgs e)
		{
			string text="";

			if(((MenuItem)sender).Text.Substring(0,3)=="AND")
				text = "AND\t"+_dragObject.objectName+" = ";
			else
				text = "WHERE\t"+_dragObject.objectName+" = ";

			int pos = qcTextEditor.SelectionStart;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
			firstPos=pos+text.Length-2;
			lastPos=pos+text.Length-2;
			ApplyProperty2(_dragObject.objectName);
		}

		private void menuItemOrderBy_Click(object sender, System.EventArgs e)
		{
			string text = "ORDER BY\t"+_dragObject.objectName;
			int pos = qcTextEditor.SelectionStart;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
			firstPos=pos+text.Length-2;
			lastPos=pos+text.Length-2;
			ApplyProperty2(_dragObject.objectName);
		}

		private void menuItemGroupBy_Click(object sender, System.EventArgs e)
		{
			string text = "GROUP BY\t"+_dragObject.objectName;
			int pos = qcTextEditor.SelectionStart;
			qcTextEditor.ActiveTextAreaControl.TextArea.InsertString(text);
			firstPos=pos+text.Length-2;
			lastPos=pos+text.Length-2;
			ApplyProperty2(_dragObject.objectName);
		}

		#endregion
		#region Classes	
		/// <summary>
		/// Custom MenuItem used for snippets
		/// </summary>
		public class SnippetMenuItem:MenuItem
		{
			public string statement ="";
		}
        public class oCompany
        {
            public string Co_Name = "";
            public int CompId = 0;
            public string Dbname = "";
            public string l_Yn = "";

        }
		public class DateTimeReverserClass : IComparer  
		{
			// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
			int IComparer.Compare( Object x, Object y )  
			{
				DateTime dx = (DateTime)x;
				DateTime dy = (DateTime)y;
				if(dx>dy)
					return -1;
				else
					return 1;
			}
		}
		private class Alias
		{
			public Alias(string alias, string table)
			{
				AliasName = alias;
				TableName = table;
			}

			public string AliasName;
			public string TableName;
		}
		private class OutPutContainer
		{
			public OutPutContainer(DataSet dataset, IDatabaseManager database, string message, TimeSpan executionTime,bool query, string statusText)
			{
				this.dataset=dataset;
				this.database=database;
				this.message=message;
				this.executionTime=executionTime;
				this.query=query;
				this.statusText=statusText;
			}
			public DataSet dataset;
			public IDatabaseManager database;
			public string message;
			public TimeSpan executionTime;
			public string statusText;
			bool query;
		}
		#endregion
		#region Executing in sep thread
		private delegate DataSet RunAsyncCallDelegate(string command, IDatabaseManager db, IDbConnection dataConnection, string databaseName);
		private void RunAsyncCallback(IAsyncResult ar)
		{
			// This method does the real processing
			MainForm frm = (MainForm)this.MdiParentForm;
			Thread t =Thread.CurrentThread;
			
			DateTime dt = DateTime.Now;
			
			RunAsyncCallDelegate msc = (RunAsyncCallDelegate)((AsyncResult)ar).AsyncDelegate;
			_currentDataSet = msc.EndInvoke(ar);

			TimeSpan executionTime = DateTime.Now.Subtract(dt);
			_currentExecutionTime = DateTime.Now.Subtract(dt);
		}
		
		private DataSet RunAsyncCall(string command, IDatabaseManager db,IDbConnection dataConnection, string databaseName)
		{
			try
			{
				Thread t =Thread.CurrentThread;
				DataSet ds = db.ExecuteCommand_DataSet(command, dataConnection, databaseName);
				return ds;
			}
			catch(Exception ex)
			{
				_currentException = ex;
				return null;
			}
		}

		#endregion	
	}
}