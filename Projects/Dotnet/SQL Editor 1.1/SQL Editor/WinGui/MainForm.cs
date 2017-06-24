using SQLEditor.WinGui.Base;
using SQLEditor.Database;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Chris.Beckett.MenuImageLib;//Added by Archana K. on 19/07/13 for Bug-17525 


namespace SQLEditor
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>   
	public class MainForm : System.Windows.Forms.Form
	{
		#region Members
		XmlDocument xmlStartPage=new XmlDocument();
        ParaObjects oParaObjects = new ParaObjects();
		FrmSearch frmSearch;
        private object [] persistedSearchItems;
		public RichTextBoxFinds richTextBoxFinds = RichTextBoxFinds.None;
        public string DefaultDb;
        public int nComp_Id;
		public const string MB_TITLE = "SQL Editor";
		private const int SW_RESTORE = 9;
		private const int HWND_TOPMOST = -1;
		private const int HWND_NOTOPMOST = -2;
		private const int SWP_NOMOVE = 2;
		private const int SWP_NOSIZE = 1;
		private string _commandLineFile ="";
        public delegate void SetOpenFileDelegate(string str);
        static private MainForm m_MainForm;
        public MainMenu mainMenu;
        private MenuItem menuItemFile;
        private MenuItem menuItemNew;
        private MenuItem menuItemOpen;
        private MenuItem menuItem8;
        private MenuItem menuItem_Save;
        private MenuItem menuItem_SaveAs;
        private MenuItem menuItem23;
        private MenuItem menuItemPageSetUp;
        private MenuItem menuItemPrint;
        private MenuItem menuItem18;
        private MenuItem menuItem17;
        private MenuItem miImportXMLStructure;
        private MenuItem miImportXMLData;
        private MenuItem menuItem9;
        private MenuItem menuItemClose;
        private MenuItem menuItemCloseAll;
        private MenuItem menuItem4;
        private MenuItem menuItemExit;
        private MenuItem menuItem3;
        private MenuItem miUndo;
        private MenuItem menuItem7;
        private MenuItem menuItemPaste;
        private MenuItem menuItemCopy;
        private MenuItem menuItemCut;
        private MenuItem menuItem10;
        private MenuItem menuItemFind;
        public MenuItem menuItemFindNext;
        private MenuItem menuItem14;
        private MenuItem menuItemGoToLine;
        private MenuItem menuItem19;
        private MenuItem menuItemManageSnippets;
        private MenuItem menuItem20;
        private MenuItem menuItemCompare;
        private MenuItem menuItemView;
        private MenuItem menuItemPropertyWindow;
        private MenuItem menuItemToolbox;
        private MenuItem menuItemWorkSpaceExplorer;
        private MenuItem menuItemOutputWindow;
        private MenuItem menuItemTaskList;
        private MenuItem menuItem1;
        private MenuItem menuItemToolBar;
        private MenuItem menuItemStatusBar;
        private MenuItem menuItemCloseOutputWindow;
        private MenuItem menuItemTools;
        private MenuItem menuItem_InsertStatement;
        private MenuItem menuItem_UpdateStatement;
        private MenuItem menuItemRunQuery;
        private MenuItem menuItemRunCurrentQuery;
        private MenuItem menuItemStopQuery;
        private MenuItem menuItem12;
        private MenuItem menuItemOptions;
        private MenuItem menuItemWindow;
        private MenuItem menuItemHelp;
        private MenuItem menuItem_Help;
        private MenuItem menuItemAbout;
        private ToolBarButton toolBarButtonClose;
        private MenuItem menuItem2;
        private ToolBarButton toolBarButtonSeparator1;
        private StatusBarPanel panel1;
        private LinkLabel LLSCRICPT_Updates;

		[DllImport("user32.dll")] 
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")] 
		private static extern bool ShowWindowAsync(IntPtr hWnd,int nCmdShow);

		[DllImport("user32.dll")] 
		private static extern bool IsIconic(IntPtr hWnd);

        ////Dockforms...
		private DeserializeDockContent _deserializeDockContent;
		public FrmDebug DebugWindow = null;//new FrmDebug(this);
		public FrmDBObjects m_FrmDBObjects = null;//new FrmDBObjects(this);
		public FrmOutput OutputWindow = null;//new FrmOutput(this);
		public FrmResults ResultWindow = null;//new FrmOutput(this);
		public FrmTask TaskList = null;//new FrmTask(this);
		private Options m_options = new Options();
        public ArrayList QueryForms = new ArrayList();
		private WeifenLuo.WinFormsUI.DockPanel dockManager = new WeifenLuo.WinFormsUI.DockPanel();
		
		public System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolBar toolBar;
        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.ToolBarButton toolBarButtonNew;
        private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
        private System.Windows.Forms.ToolBarButton toolBarButtonSolutionExplorer;
		private System.Windows.Forms.ToolBarButton toolBarButtonOutputWindow;
		private System.Windows.Forms.ToolBarButton toolBarButtonTaskList;
        private System.Windows.Forms.ToolBarButton toolBarButtonSeparator2;
        private System.Windows.Forms.ToolBarButton toolBarButtonRun;
        private System.Windows.Forms.StatusBarPanel panel2;
		private System.Windows.Forms.StatusBarPanel panel3;
		private System.Windows.Forms.StatusBarPanel panel4;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator3;
        public ArrayList DBConnections = new ArrayList();
        private System.Windows.Forms.Timer timer_StartUp;
        private Chris.Beckett.MenuImageLib.MenuImage menuExtender;//Uncommented by Archana K. on 19/07/13 for Bug-17525
        public bool Debug=false;
        public System.Windows.Forms.ContextMenu contextMenuDataBases;
        static System.Windows.Forms.Form _activeForm;
        private System.Windows.Forms.StatusBarPanel panel5;
        private System.Windows.Forms.ToolBarButton toolBarButtonStop;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
        //Added by Archana K. on 12/07/13 for Bug-17525 start
        const int Timeout = 5000;
        private String cAppPId, cAppName, pPApplCode, pPApplName, pPApplText;
        private Icon pFrmIcon;
        private short pPApplPID;
        //Added by Archana K. on 12/07/13 for Bug-17525 end
        public FrmQuery ActiveQueryForm
		{
			get
			{
                if (dockManager.ActiveDocument == null)
                {
                    NewQueryform();
                }
				Type t = dockManager.ActiveDocument.GetType();
                if (t.ToString() == "SQLEditor.FrmQuery")
                {
                    return (FrmQuery)dockManager.ActiveDocument;
                }
                else
                {
                    return null;
                }
			}
			set 
			{
				ActiveQueryForm = value;
			}
		}
		#endregion
		#region Default
        public MainForm(string[] strArgs)
        {
            nComp_Id = Convert.ToInt16(strArgs[0]);       /// Company Id
            DefaultDb = strArgs[1];                       /// Company Database
            _commandLineFile = strArgs[2];                /// SQL Script
            oParaObjects.cServerName = strArgs[3];        /// Server Name
            oParaObjects.cUsername = strArgs[4];          /// User Name
            oParaObjects.cPassword = strArgs[5];          /// Password  
            //Added by Archana K. on 18/07/13 for Bug-17525 start
            Icon MainIcon = new System.Drawing.Icon(strArgs[8].Replace("<*#*>", " "));
            pFrmIcon = MainIcon;
            pPApplText = strArgs[9].Replace("<*#*>", " ");
            pPApplName = strArgs[10];
            pPApplPID = Convert.ToInt16(strArgs[11]);
            pPApplCode = strArgs[12];
            //Added by Archana K. on 18/07/13 for Bug-17525 end
            InitDockManager();
            InitializeComponent();
            InitDockForms();
            this.panel1.Text = "SQL Editor : " + System.Windows.Forms.Application.ProductVersion;
            this.panel2.Text = "";
        }
        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.panel1 = new System.Windows.Forms.StatusBarPanel();
            this.panel2 = new System.Windows.Forms.StatusBarPanel();
            this.panel3 = new System.Windows.Forms.StatusBarPanel();
            this.panel4 = new System.Windows.Forms.StatusBarPanel();
            this.panel5 = new System.Windows.Forms.StatusBarPanel();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSeparator1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonRun = new System.Windows.Forms.ToolBarButton();
            this.contextMenuDataBases = new System.Windows.Forms.ContextMenu();
            this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSeparator2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonOutputWindow = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonTaskList = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSolutionExplorer = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSeparator3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonClose = new System.Windows.Forms.ToolBarButton();
            this.timer_StartUp = new System.Windows.Forms.Timer(this.components);
            this.menuExtender = new Chris.Beckett.MenuImageLib.MenuImage(this.components);//Uncommented by Archana K. on 17/07/13 for Bug-17525
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem_Save = new System.Windows.Forms.MenuItem();
            this.menuItem_SaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItemPageSetUp = new System.Windows.Forms.MenuItem();
            this.menuItemPrint = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.miImportXMLStructure = new System.Windows.Forms.MenuItem();
            this.miImportXMLData = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItemClose = new System.Windows.Forms.MenuItem();
            this.menuItemCloseAll = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.miUndo = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItemPaste = new System.Windows.Forms.MenuItem();
            this.menuItemCopy = new System.Windows.Forms.MenuItem();
            this.menuItemCut = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItemFind = new System.Windows.Forms.MenuItem();
            this.menuItemFindNext = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItemGoToLine = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItemManageSnippets = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItemCompare = new System.Windows.Forms.MenuItem();
            this.menuItemPropertyWindow = new System.Windows.Forms.MenuItem();
            this.menuItemToolbox = new System.Windows.Forms.MenuItem();
            this.menuItemWorkSpaceExplorer = new System.Windows.Forms.MenuItem();
            this.menuItemOutputWindow = new System.Windows.Forms.MenuItem();
            this.menuItemTaskList = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemToolBar = new System.Windows.Forms.MenuItem();
            this.menuItemStatusBar = new System.Windows.Forms.MenuItem();
            this.menuItemCloseOutputWindow = new System.Windows.Forms.MenuItem();
            this.menuItem_InsertStatement = new System.Windows.Forms.MenuItem();
            this.menuItem_UpdateStatement = new System.Windows.Forms.MenuItem();
            this.menuItemRunQuery = new System.Windows.Forms.MenuItem();
            this.menuItemRunCurrentQuery = new System.Windows.Forms.MenuItem();
            this.menuItemStopQuery = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItem_Help = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemView = new System.Windows.Forms.MenuItem();
            this.menuItemTools = new System.Windows.Forms.MenuItem();
            this.menuItemWindow = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.LLSCRICPT_Updates = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel5)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 543);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.panel1,
            this.panel2,
            this.panel3,
            this.panel4,
            this.panel5});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(804, 22);
            this.statusBar.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Name = "panel1";
            this.panel1.Text = "SQL Editor";
            this.panel1.ToolTipText = "Product Name ";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
            this.panel2.MinWidth = 0;
            this.panel2.Name = "panel2";
            this.panel2.ToolTipText = "Company Name";
            this.panel2.Width = 200;
            // 
            // panel3
            // 
            this.panel3.Name = "panel3";
            this.panel3.ToolTipText = "Active Database";
            this.panel3.Width = 150;
            // 
            // panel4
            // 
            this.panel4.Name = "panel4";
            this.panel4.Width = 500;
            // 
            // panel5
            // 
            this.panel5.Name = "panel5";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.imageList.Images.SetKeyName(0, "new.gif");
            this.imageList.Images.SetKeyName(1, "open.gif");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "output.gif");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "");
            this.imageList.Images.SetKeyName(14, "");
            this.imageList.Images.SetKeyName(15, "save.gif");
            this.imageList.Images.SetKeyName(16, "");
            this.imageList.Images.SetKeyName(17, "");
            this.imageList.Images.SetKeyName(18, "");
            this.imageList.Images.SetKeyName(19, "");
            this.imageList.Images.SetKeyName(20, "");
            this.imageList.Images.SetKeyName(21, "");
            this.imageList.Images.SetKeyName(22, "");
            this.imageList.Images.SetKeyName(23, "");
            this.imageList.Images.SetKeyName(24, "");
            this.imageList.Images.SetKeyName(25, "");
            this.imageList.Images.SetKeyName(26, "");
            this.imageList.Images.SetKeyName(27, "");
            this.imageList.Images.SetKeyName(28, "");
            this.imageList.Images.SetKeyName(29, "");
            this.imageList.Images.SetKeyName(30, "");
            this.imageList.Images.SetKeyName(31, "close.gif");
            // 
            // toolBar
            // 
            this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonNew,
            this.toolBarButtonSave,
            this.toolBarButtonOpen,
            this.toolBarButtonSeparator1,
            this.toolBarButtonRun,
            this.toolBarButtonStop,
            this.toolBarButtonSeparator2,
            this.toolBarButtonOutputWindow,
            this.toolBarButtonTaskList,
            this.toolBarButtonSolutionExplorer,
            this.toolBarButtonSeparator3,
            this.toolBarButtonClose});
            this.toolBar.ButtonSize = new System.Drawing.Size(100, 30);
            this.toolBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.toolBar.DropDownArrows = true;
            this.toolBar.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(804, 41);
            this.toolBar.TabIndex = 6;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarButtonNew
            // 
            this.toolBarButtonNew.ImageIndex = 0;
            this.toolBarButtonNew.Name = "toolBarButtonNew";
            this.toolBarButtonNew.Text = "New";
            this.toolBarButtonNew.ToolTipText = "New";
            // 
            // toolBarButtonSave
            // 
            this.toolBarButtonSave.ImageIndex = 15;
            this.toolBarButtonSave.Name = "toolBarButtonSave";
            this.toolBarButtonSave.Text = "Save";
            this.toolBarButtonSave.ToolTipText = "Save";
            // 
            // toolBarButtonOpen
            // 
            this.toolBarButtonOpen.ImageIndex = 1;
            this.toolBarButtonOpen.Name = "toolBarButtonOpen";
            this.toolBarButtonOpen.Text = "Open";
            this.toolBarButtonOpen.ToolTipText = "Open";
            // 
            // toolBarButtonSeparator1
            // 
            this.toolBarButtonSeparator1.Name = "toolBarButtonSeparator1";
            this.toolBarButtonSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonRun
            // 
            this.toolBarButtonRun.DropDownMenu = this.contextMenuDataBases;
            this.toolBarButtonRun.ImageIndex = 12;
            this.toolBarButtonRun.Name = "toolBarButtonRun";
            this.toolBarButtonRun.Text = "Execute";
            this.toolBarButtonRun.ToolTipText = "Execute query";
            // 
            // toolBarButtonStop
            // 
            this.toolBarButtonStop.ImageIndex = 28;
            this.toolBarButtonStop.Name = "toolBarButtonStop";
            this.toolBarButtonStop.Text = "Stop";
            this.toolBarButtonStop.ToolTipText = "Stop current execution";
            // 
            // toolBarButtonSeparator2
            // 
            this.toolBarButtonSeparator2.Name = "toolBarButtonSeparator2";
            this.toolBarButtonSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonOutputWindow
            // 
            this.toolBarButtonOutputWindow.ImageIndex = 5;
            this.toolBarButtonOutputWindow.Name = "toolBarButtonOutputWindow";
            this.toolBarButtonOutputWindow.Text = "Output";
            this.toolBarButtonOutputWindow.ToolTipText = "Output Window";
            // 
            // toolBarButtonTaskList
            // 
            this.toolBarButtonTaskList.ImageIndex = 6;
            this.toolBarButtonTaskList.Name = "toolBarButtonTaskList";
            this.toolBarButtonTaskList.Text = "Task List";
            this.toolBarButtonTaskList.ToolTipText = "Task List";
            this.toolBarButtonTaskList.Visible = false;
            // 
            // toolBarButtonSolutionExplorer
            // 
            this.toolBarButtonSolutionExplorer.ImageIndex = 2;
            this.toolBarButtonSolutionExplorer.Name = "toolBarButtonSolutionExplorer";
            this.toolBarButtonSolutionExplorer.Text = "Company List";
            this.toolBarButtonSolutionExplorer.ToolTipText = "Company List";
            // 
            // toolBarButtonSeparator3
            // 
            this.toolBarButtonSeparator3.Name = "toolBarButtonSeparator3";
            this.toolBarButtonSeparator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonClose
            // 
            this.toolBarButtonClose.ImageIndex = 31;
            this.toolBarButtonClose.Name = "toolBarButtonClose";
            this.toolBarButtonClose.Text = "Close";
            this.toolBarButtonClose.ToolTipText = "Close";
            // 
            // menuExtender
            // 
            this.menuExtender.ImageList = this.imageList;
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            this.menuExtender.SetMenuImage(this.menuItemNew, "0");
            this.menuItemNew.OwnerDraw = true;
            this.menuItemNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.menuItemNew.Text = "&New";
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            this.menuExtender.SetMenuImage(this.menuItemOpen, "1");
            this.menuItemOpen.OwnerDraw = true;
            this.menuItemOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuItemOpen.Text = "&Open...";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 2;
            this.menuExtender.SetMenuImage(this.menuItem8, null);
            this.menuItem8.OwnerDraw = true;
            this.menuItem8.Text = "-";
            // 
            // menuItem_Save
            // 
            this.menuItem_Save.Index = 3;
            this.menuExtender.SetMenuImage(this.menuItem_Save, "15");
            this.menuItem_Save.OwnerDraw = true;
            this.menuItem_Save.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItem_Save.Text = "&Save";
            this.menuItem_Save.Click += new System.EventHandler(this.menuItem_Save_Click);
            // 
            // menuItem_SaveAs
            // 
            this.menuItem_SaveAs.Index = 4;
            this.menuExtender.SetMenuImage(this.menuItem_SaveAs, null);
            this.menuItem_SaveAs.OwnerDraw = true;
            this.menuItem_SaveAs.Text = "Save &as";
            this.menuItem_SaveAs.Click += new System.EventHandler(this.menuItem_SaveAs_Click);
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 5;
            this.menuExtender.SetMenuImage(this.menuItem23, null);
            this.menuItem23.OwnerDraw = true;
            this.menuItem23.Text = "-";
            // 
            // menuItemPageSetUp
            // 
            this.menuItemPageSetUp.Index = 6;
            this.menuExtender.SetMenuImage(this.menuItemPageSetUp, null);
            this.menuItemPageSetUp.OwnerDraw = true;
            this.menuItemPageSetUp.Text = "Page setup";
            this.menuItemPageSetUp.Click += new System.EventHandler(this.menuItemPageSetUp_Click);
            // 
            // menuItemPrint
            // 
            this.menuItemPrint.Index = 7;
            this.menuExtender.SetMenuImage(this.menuItemPrint, null);
            this.menuItemPrint.OwnerDraw = true;
            this.menuItemPrint.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.menuItemPrint.Text = "Print";
            this.menuItemPrint.Click += new System.EventHandler(this.menuItemPrint_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 8;
            this.menuExtender.SetMenuImage(this.menuItem18, null);
            this.menuItem18.OwnerDraw = true;
            this.menuItem18.Text = "-";
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 9;
            this.menuExtender.SetMenuImage(this.menuItem17, null);
            this.menuItem17.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miImportXMLStructure,
            this.miImportXMLData});
            this.menuItem17.OwnerDraw = true;
            this.menuItem17.Text = "&Import";
            // 
            // miImportXMLStructure
            // 
            this.miImportXMLStructure.Index = 0;
            this.menuExtender.SetMenuImage(this.miImportXMLStructure, null);
            this.miImportXMLStructure.OwnerDraw = true;
            this.miImportXMLStructure.Text = "Table structure";
            this.miImportXMLStructure.Click += new System.EventHandler(this.miImportXMLStructure_Click);
            // 
            // miImportXMLData
            // 
            this.miImportXMLData.Index = 1;
            this.menuExtender.SetMenuImage(this.miImportXMLData, null);
            this.miImportXMLData.OwnerDraw = true;
            this.miImportXMLData.Text = "XML Data";
            this.miImportXMLData.Click += new System.EventHandler(this.miImportXMLData_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 10;
            this.menuExtender.SetMenuImage(this.menuItem9, null);
            this.menuItem9.OwnerDraw = true;
            this.menuItem9.Text = "-";
            // 
            // menuItemClose
            // 
            this.menuItemClose.Index = 11;
            this.menuExtender.SetMenuImage(this.menuItemClose, null);
            this.menuItemClose.OwnerDraw = true;
            this.menuItemClose.Shortcut = System.Windows.Forms.Shortcut.CtrlF4;
            this.menuItemClose.Text = "&Close";
            this.menuItemClose.Click += new System.EventHandler(this.menuItemClose_Click);
            // 
            // menuItemCloseAll
            // 
            this.menuItemCloseAll.Index = 12;
            this.menuExtender.SetMenuImage(this.menuItemCloseAll, null);
            this.menuItemCloseAll.OwnerDraw = true;
            this.menuItemCloseAll.Text = "Close &All";
            this.menuItemCloseAll.Click += new System.EventHandler(this.menuItemCloseAll_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 13;
            this.menuExtender.SetMenuImage(this.menuItem4, null);
            this.menuItem4.OwnerDraw = true;
            this.menuItem4.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 14;
            this.menuExtender.SetMenuImage(this.menuItemExit, null);
            this.menuItemExit.OwnerDraw = true;
            this.menuItemExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.menuItemExit.Text = "&Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // miUndo
            // 
            this.miUndo.Index = 0;
            this.menuExtender.SetMenuImage(this.miUndo, "17");
            this.miUndo.OwnerDraw = true;
            this.miUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.miUndo.Text = "&Undo";
            this.miUndo.Click += new System.EventHandler(this.miUndo_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuExtender.SetMenuImage(this.menuItem7, null);
            this.menuItem7.OwnerDraw = true;
            this.menuItem7.Text = "-";
            // 
            // menuItemPaste
            // 
            this.menuItemPaste.Index = 2;
            this.menuExtender.SetMenuImage(this.menuItemPaste, "19");
            this.menuItemPaste.OwnerDraw = true;
            this.menuItemPaste.Text = "&Paste";
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Index = 3;
            this.menuExtender.SetMenuImage(this.menuItemCopy, "18");
            this.menuItemCopy.OwnerDraw = true;
            this.menuItemCopy.Text = "&Copy";
            // 
            // menuItemCut
            // 
            this.menuItemCut.Index = 4;
            this.menuExtender.SetMenuImage(this.menuItemCut, null);
            this.menuItemCut.OwnerDraw = true;
            this.menuItemCut.Text = "Cu&t";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 5;
            this.menuExtender.SetMenuImage(this.menuItem10, null);
            this.menuItem10.OwnerDraw = true;
            this.menuItem10.Text = "-";
            // 
            // menuItemFind
            // 
            this.menuItemFind.Index = 6;
            this.menuExtender.SetMenuImage(this.menuItemFind, "21");
            this.menuItemFind.OwnerDraw = true;
            this.menuItemFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.menuItemFind.Text = "Find";
            this.menuItemFind.Click += new System.EventHandler(this.menuItemFind_Click);
            // 
            // menuItemFindNext
            // 
            this.menuItemFindNext.Index = 7;
            this.menuExtender.SetMenuImage(this.menuItemFindNext, null);
            this.menuItemFindNext.OwnerDraw = true;
            this.menuItemFindNext.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.menuItemFindNext.Text = "Find next";
            this.menuItemFindNext.Click += new System.EventHandler(this.menuItemFindNext_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 8;
            this.menuExtender.SetMenuImage(this.menuItem14, null);
            this.menuItem14.OwnerDraw = true;
            this.menuItem14.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            this.menuItem14.Text = "R&eplace";
            // 
            // menuItemGoToLine
            // 
            this.menuItemGoToLine.Index = 9;
            this.menuExtender.SetMenuImage(this.menuItemGoToLine, null);
            this.menuItemGoToLine.OwnerDraw = true;
            this.menuItemGoToLine.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.menuItemGoToLine.Text = "&Go to line";
            this.menuItemGoToLine.Click += new System.EventHandler(this.menuItemGoToLine_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 10;
            this.menuExtender.SetMenuImage(this.menuItem19, null);
            this.menuItem19.OwnerDraw = true;
            this.menuItem19.Text = "-";
            this.menuItem19.Visible = false;
            // 
            // menuItemManageSnippets
            // 
            this.menuItemManageSnippets.Index = 11;
            this.menuExtender.SetMenuImage(this.menuItemManageSnippets, null);
            this.menuItemManageSnippets.OwnerDraw = true;
            this.menuItemManageSnippets.Shortcut = System.Windows.Forms.Shortcut.F6;
            this.menuItemManageSnippets.Text = "Manage &snippets";
            this.menuItemManageSnippets.Visible = false;
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 12;
            this.menuExtender.SetMenuImage(this.menuItem20, null);
            this.menuItem20.OwnerDraw = true;
            this.menuItem20.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.menuItem20.Text = "Set reserved words to upper case";
            this.menuItem20.Visible = false;
            // 
            // menuItemCompare
            // 
            this.menuItemCompare.Index = 13;
            this.menuExtender.SetMenuImage(this.menuItemCompare, null);
            this.menuItemCompare.OwnerDraw = true;
            this.menuItemCompare.Text = "Compare ";
            this.menuItemCompare.Visible = false;
            // 
            // menuItemPropertyWindow
            // 
            this.menuItemPropertyWindow.Enabled = false;
            this.menuItemPropertyWindow.Index = 0;
            this.menuExtender.SetMenuImage(this.menuItemPropertyWindow, null);
            this.menuItemPropertyWindow.OwnerDraw = true;
            this.menuItemPropertyWindow.Text = "&Property Window";
            this.menuItemPropertyWindow.Visible = false;
            // 
            // menuItemToolbox
            // 
            this.menuItemToolbox.Index = 1;
            this.menuExtender.SetMenuImage(this.menuItemToolbox, "2");
            this.menuItemToolbox.OwnerDraw = true;
            this.menuItemToolbox.Shortcut = System.Windows.Forms.Shortcut.F8;
            this.menuItemToolbox.Text = "Server Explorer";
            this.menuItemToolbox.Click += new System.EventHandler(this.menuItemToolbox_Click);
            // 
            // menuItemWorkSpaceExplorer
            // 
            this.menuItemWorkSpaceExplorer.Index = 2;
            this.menuExtender.SetMenuImage(this.menuItemWorkSpaceExplorer, "29");
            this.menuItemWorkSpaceExplorer.OwnerDraw = true;
            this.menuItemWorkSpaceExplorer.Shortcut = System.Windows.Forms.Shortcut.ShiftF7;
            this.menuItemWorkSpaceExplorer.Text = "Workspace Explorer";
            this.menuItemWorkSpaceExplorer.Visible = false;
            // 
            // menuItemOutputWindow
            // 
            this.menuItemOutputWindow.Index = 3;
            this.menuExtender.SetMenuImage(this.menuItemOutputWindow, "5");
            this.menuItemOutputWindow.OwnerDraw = true;
            this.menuItemOutputWindow.Shortcut = System.Windows.Forms.Shortcut.ShiftF8;
            this.menuItemOutputWindow.Text = "&Output Window";
            this.menuItemOutputWindow.Click += new System.EventHandler(this.menuItemOutputWindow_Click);
            // 
            // menuItemTaskList
            // 
            this.menuItemTaskList.Index = 4;
            this.menuExtender.SetMenuImage(this.menuItemTaskList, "6");
            this.menuItemTaskList.OwnerDraw = true;
            this.menuItemTaskList.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF8;
            this.menuItemTaskList.Text = "Task &List";
            this.menuItemTaskList.Click += new System.EventHandler(this.menuItemTaskList_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuExtender.SetMenuImage(this.menuItem1, null);
            this.menuItem1.OwnerDraw = true;
            this.menuItem1.Text = "-";
            // 
            // menuItemToolBar
            // 
            this.menuItemToolBar.Checked = true;
            this.menuItemToolBar.Index = 6;
            this.menuExtender.SetMenuImage(this.menuItemToolBar, null);
            this.menuItemToolBar.OwnerDraw = true;
            this.menuItemToolBar.Text = "Tool Bar";
            this.menuItemToolBar.Click += new System.EventHandler(this.menuItemToolBar_Click);
            // 
            // menuItemStatusBar
            // 
            this.menuItemStatusBar.Checked = true;
            this.menuItemStatusBar.Index = 7;
            this.menuExtender.SetMenuImage(this.menuItemStatusBar, null);
            this.menuItemStatusBar.OwnerDraw = true;
            this.menuItemStatusBar.Text = "Status Bar";
            this.menuItemStatusBar.Click += new System.EventHandler(this.menuItemStatusBar_Click);
            // 
            // menuItemCloseOutputWindow
            // 
            this.menuItemCloseOutputWindow.Index = 8;
            this.menuExtender.SetMenuImage(this.menuItemCloseOutputWindow, null);
            this.menuItemCloseOutputWindow.OwnerDraw = true;
            this.menuItemCloseOutputWindow.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.menuItemCloseOutputWindow.Text = "&Close Output Window";
            this.menuItemCloseOutputWindow.Click += new System.EventHandler(this.menuItemCloseOutputWindow_Click);
            // 
            // menuItem_InsertStatement
            // 
            this.menuItem_InsertStatement.Index = 0;
            this.menuExtender.SetMenuImage(this.menuItem_InsertStatement, null);
            this.menuItem_InsertStatement.OwnerDraw = true;
            this.menuItem_InsertStatement.Text = "&Create insert statement";
            this.menuItem_InsertStatement.Click += new System.EventHandler(this.menuItem_InsertStatement_Click);
            // 
            // menuItem_UpdateStatement
            // 
            this.menuItem_UpdateStatement.Index = 1;
            this.menuExtender.SetMenuImage(this.menuItem_UpdateStatement, null);
            this.menuItem_UpdateStatement.OwnerDraw = true;
            this.menuItem_UpdateStatement.Text = "Create &update statement";
            this.menuItem_UpdateStatement.Click += new System.EventHandler(this.menuItem_UpdateStatement_Click);
            // 
            // menuItemRunQuery
            // 
            this.menuItemRunQuery.Index = 3;
            this.menuExtender.SetMenuImage(this.menuItemRunQuery, "12");
            this.menuItemRunQuery.OwnerDraw = true;
            this.menuItemRunQuery.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.menuItemRunQuery.Text = "Run Query";
            this.menuItemRunQuery.Click += new System.EventHandler(this.menuItemRunQuery_Click);
            // 
            // menuItemRunCurrentQuery
            // 
            this.menuItemRunCurrentQuery.Index = 4;
            this.menuExtender.SetMenuImage(this.menuItemRunCurrentQuery, null);
            this.menuItemRunCurrentQuery.OwnerDraw = true;
            this.menuItemRunCurrentQuery.Shortcut = System.Windows.Forms.Shortcut.F9;
            this.menuItemRunCurrentQuery.Text = "Run &current query";
            this.menuItemRunCurrentQuery.Click += new System.EventHandler(this.menuItemRunCurrentQuery_Click);
            // 
            // menuItemStopQuery
            // 
            this.menuItemStopQuery.Index = 5;
            this.menuExtender.SetMenuImage(this.menuItemStopQuery, null);
            this.menuItemStopQuery.OwnerDraw = true;
            this.menuItemStopQuery.Text = "Stop Query";
            this.menuItemStopQuery.Click += new System.EventHandler(this.menuItemStopQuery_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 6;
            this.menuExtender.SetMenuImage(this.menuItem12, null);
            this.menuItem12.OwnerDraw = true;
            this.menuItem12.Text = "-";
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.Index = 7;
            this.menuExtender.SetMenuImage(this.menuItemOptions, "4");
            this.menuItemOptions.OwnerDraw = true;
            this.menuItemOptions.Text = "&Options";
            this.menuItemOptions.Click += new System.EventHandler(this.menuItemOptions_Click);
            // 
            // menuItem_Help
            // 
            this.menuItem_Help.Index = 0;
            this.menuExtender.SetMenuImage(this.menuItem_Help, null);
            this.menuItem_Help.OwnerDraw = true;
            this.menuItem_Help.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.menuItem_Help.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 1;
            this.menuExtender.SetMenuImage(this.menuItemAbout, null);
            this.menuItemAbout.OwnerDraw = true;
            this.menuItemAbout.Text = "&About SQLEditor...";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuExtender.SetMenuImage(this.menuItem2, null);
            this.menuItem2.OwnerDraw = true;
            this.menuItem2.Text = "-";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItem3,
            this.menuItemView,
            this.menuItemTools,
            this.menuItemWindow,
            this.menuItemHelp});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItemOpen,
            this.menuItem8,
            this.menuItem_Save,
            this.menuItem_SaveAs,
            this.menuItem23,
            this.menuItemPageSetUp,
            this.menuItemPrint,
            this.menuItem18,
            this.menuItem17,
            this.menuItem9,
            this.menuItemClose,
            this.menuItemCloseAll,
            this.menuItem4,
            this.menuItemExit});
            this.menuItemFile.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemFile.Text = "&File";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miUndo,
            this.menuItem7,
            this.menuItemPaste,
            this.menuItemCopy,
            this.menuItemCut,
            this.menuItem10,
            this.menuItemFind,
            this.menuItemFindNext,
            this.menuItem14,
            this.menuItemGoToLine,
            this.menuItem19,
            this.menuItemManageSnippets,
            this.menuItem20,
            this.menuItemCompare});
            this.menuItem3.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItem3.Text = "&Edit";
            // 
            // menuItemView
            // 
            this.menuItemView.Index = 2;
            this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPropertyWindow,
            this.menuItemToolbox,
            this.menuItemWorkSpaceExplorer,
            this.menuItemOutputWindow,
            this.menuItemTaskList,
            this.menuItem1,
            this.menuItemToolBar,
            this.menuItemStatusBar,
            this.menuItemCloseOutputWindow});
            this.menuItemView.MergeOrder = 1;
            this.menuItemView.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemView.Text = "&View";
            // 
            // menuItemTools
            // 
            this.menuItemTools.Index = 3;
            this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_InsertStatement,
            this.menuItem_UpdateStatement,
            this.menuItem2,
            this.menuItemRunQuery,
            this.menuItemRunCurrentQuery,
            this.menuItemStopQuery,
            this.menuItem12,
            this.menuItemOptions});
            this.menuItemTools.MergeOrder = 2;
            this.menuItemTools.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemTools.Text = "&Tools";
            // 
            // menuItemWindow
            // 
            this.menuItemWindow.Index = 4;
            this.menuItemWindow.MdiList = true;
            this.menuItemWindow.MergeOrder = 3;
            this.menuItemWindow.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemWindow.Text = "&Window";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 5;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_Help,
            this.menuItemAbout});
            this.menuItemHelp.MergeOrder = 4;
            this.menuItemHelp.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemHelp.Text = "&Help";
            this.menuItemHelp.Visible = false;
            // 
            // LLSCRICPT_Updates
            // 
            this.LLSCRICPT_Updates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LLSCRICPT_Updates.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LLSCRICPT_Updates.Location = new System.Drawing.Point(680, 12);
            this.LLSCRICPT_Updates.Name = "LLSCRICPT_Updates";
            this.LLSCRICPT_Updates.Size = new System.Drawing.Size(112, 18);
            this.LLSCRICPT_Updates.TabIndex = 19;
            this.LLSCRICPT_Updates.TabStop = true;
            this.LLSCRICPT_Updates.Text = "Download updates";
            this.LLSCRICPT_Updates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LLSCRICPT_Updates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLSCRICPT_Updates_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(804, 565);
            this.Controls.Add(this.LLSCRICPT_Updates);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.statusBar);
            // this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));//Commented by Archana K. on 19/07/13 for Bug-17525
            this.Icon = pFrmIcon;//Changed by Archana K. on 19/07/13 for Bug-17525
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "SQL Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closed += new System.EventHandler(this.MainForm_Closed);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) 
		{
			Application.DoEvents();
			Application.EnableVisualStyles();
			Application.DoEvents();

            ////* Uncomment This code when you execute from vs. [Start]
            if (args.Length == 0)
            {
                //  args = new string[] { "0", "VUDYOG", "", "", "", "", };//Commented by Archana K. on 18/07/13 for Bug-17525 
                args = new string[] { "5", "P071213", "", "udyog5\\sqlexpress", "sa", "sa1985", "^13039", "ADMIN", @"D:\USQUARE\Bmp\icon_USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };//Changed by Archana K. on 18/07/13 for Bug-17525
            }
            ////* Uncomment This code when you execute from vs. [End]
            /////MessageBox.Show(args.Length.ToString());    

            //if (args.Length != 6)//Commented by Archana K. on 18/07/13 for Bug-17525 
            if (args.Length == 6)//Changed by Archana K. on 18/07/13 for Bug-17525 
            {
                MessageBox.Show("Internal Application Are Not Execute Out-Side...", "SQL Editor", MessageBoxButtons.OK);
                Application.Exit();
                return -1;
            }
            else
            {
                m_MainForm = new MainForm(args);
                Application.Run(m_MainForm);
                return 1;
            }
		}
		#endregion
		#region Menu items
        //Added by Archana K. on 12/07/13 for Bug-17525 start
        private void mInsertProcessIdRecord()
        {
            DBConnection oConn = new DBConnection();
            foreach (MainForm.DBConnection c in DBConnections)
            {
                oConn = c;
                break;
            }
            IDatabaseManager db = DatabaseFactory.CreateNew(oConn.Connection);
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "SQLEditor.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            db.ExecuteCommand(sqlstr, oConn.Connection, DefaultDb);

        }
        private void mDeleteProcessIdRecord()
        {
            DBConnection oConn = new DBConnection();
            foreach (MainForm.DBConnection c in DBConnections)
            {
                oConn = c;
                break;
            }
            IDatabaseManager db = DatabaseFactory.CreateNew(oConn.Connection);
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            db.ExecuteCommand(sqlstr, oConn.Connection, DefaultDb);

        }
        //Added by Archana K. on 12/07/13 for Bug-17525 end
		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			// TODO: add check for modified windows
			Close();
			Application.Exit();
		}
		private void menuItemToolbox_Click(object sender, System.EventArgs e)
		{
			if(m_FrmDBObjects.IsActivated)
				m_FrmDBObjects.Hide();
			else
				m_FrmDBObjects.Show(dockManager);
		}
		private void menuItemOutputWindow_Click(object sender, System.EventArgs e)
		{
            //if (OutputWindow.IsActivated)
            //    OutputWindow.Hide();
            //else
                OutputWindow.Show(dockManager);
		}
		private void menuItemTaskList_Click(object sender, System.EventArgs e)
		{
            if(TaskList.IsActivated)
                TaskList.Hide();
            else
			    TaskList.Show(dockManager);
		}
		private void menuItemResult_Click(object sender, System.EventArgs e)
		{
            if (ResultWindow.IsActivated)
                ResultWindow.Hide();
            else
                ResultWindow.Show(dockManager);
		}
		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			FrmAbout aboutDialog = new FrmAbout();
			aboutDialog.ShowDialogWindow(this);
		}
		private void menuItem_InsertStatement_Click(object sender, System.EventArgs e)
		{
			this.ActiveQueryForm.CreateInsertStatement();
		}
		private void menuItemXmlDoc_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.GetXmlDocFile();
		}
		private void menuItemInsertHeader_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.InsertHeader();
		}
		private void miUndo_Click(object sender, System.EventArgs e)
		{
			this.ActiveQueryForm.Undo();
		}
		private void menuItem_UpdateStatement_Click(object sender, System.EventArgs e)
		{
			this.ActiveQueryForm.CreateUpdateStatement();
		}
  		private void menuItem_SaveAs_Click(object sender, System.EventArgs e)
		{
			SaveDocument();
		}
		private void menuItem_Save_Click(object sender, System.EventArgs e)
		{
			if(ActiveQueryForm.FileName.Length > 0)
			{
				ActiveQueryForm.SaveAs(ActiveQueryForm.FileName);
			}
			else
			{
				menuItem_SaveAs_Click(sender,e);
			}
		}
		private void menuItemFind_Click(object sender, System.EventArgs e)
		{
			frmSearch = new FrmSearch(ActiveQueryForm);
			frmSearch.Closing += new CancelEventHandler(frmSearch_Closing);
			frmSearch.SearchItems = persistedSearchItems;
			frmSearch.TopMost = true;
			frmSearch.Show();//Dialog(ActiveQueryForm);
			frmSearch.Focus();
		}
		private void menuItemGoToLine_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.GoToLine();
		}
		private void menuItemPaste_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.Paste();
		}
		private void menuItemCopy_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.Copy();
		}
		private void menuItemCut_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.Cut();
		}
		private void menuItemGoToDefenition_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.GoToDefenition();
		}
		private void menuItemRunQuery_Click(object sender, System.EventArgs e)
		{
            if (OutputWindow.IsActivated)
            {
                OutputWindow.Visible = false;
                ActiveQueryForm.Focus();
            }
            else
            {
                ActiveQueryForm.RunQuery();
            }
		}
		private void menuItemNew_Click(object sender, System.EventArgs e)
		{
            NewQueryform();
		}
		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "SQL files (*.SQL)|*.SQL|txt files (*.txt)|*.txt|All files (*.*)|*.*" ;
			openFile.FilterIndex = 1;
			openFile.RestoreDirectory = true ;

			if(openFile.ShowDialog() == DialogResult.OK)
			{
				OpenFile(openFile.FileName);
			}
		}
		private void menuItemFile_Popup(object sender, System.EventArgs e)
		{
			menuItemClose.Enabled = (dockManager.ActiveDocument != null);
			menuItemCloseAll.Enabled = (dockManager.Documents.Length > 0);
		}
        private void menuItemClose_Click(object sender, System.EventArgs e)
		{
            if (dockManager.ActiveDocument != null)
            {
                if (dockManager.ActiveDocument.TabText.IndexOf("*") > 0)
                {
                    SaveDocument();
                }
                dockManager.ActiveDocument.Close();
            }
		}
		private void menuItemCloseAll_Click(object sender, System.EventArgs e)
		{
            if (dockManager.ActiveDocument != null)
            {
                foreach (DockContent content in dockManager.Documents)
                {
                    if (dockManager.ActiveDocument.TabText.IndexOf("*") > 0)
                    {
                        SaveDocument();
                    }
                    content.Close();
                }
            }
		}
		private void menuItemOptions_Click(object sender, System.EventArgs e)
		{
			FrmOption frmSettings = new FrmOption(this);
			frmSettings.ShowDialogWindow(this);
		}
		private void menuItemToolBar_Click(object sender, System.EventArgs e)
		{
			toolBar.Visible = menuItemToolBar.Checked = !menuItemToolBar.Checked;
		}
		private void menuItemStatusBar_Click(object sender, System.EventArgs e)
		{
			statusBar.Visible = menuItemStatusBar.Checked = !menuItemStatusBar.Checked;
		}
		private void menuItemRunQueryLine_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.RunQueryLine();
		}
		private void menuItemreplace_Click(object sender, System.EventArgs e)
		{
			FrmSearch frmSearch = new FrmSearch(ActiveQueryForm,true);
			frmSearch.Closing += new CancelEventHandler(frmSearch_Closing);
			frmSearch.SearchItems = persistedSearchItems;
			frmSearch.Show();
			frmSearch.Focus();
		}
		private void menuRecentItem_Click(object sender, System.EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			m_FrmDBObjects.CreateConstructorString(mi.Text);
		}
		private void menuItemCloseOutputWindow_Click(object sender, System.EventArgs e)
		{
			OutputWindow.Visible=false;
			ActiveQueryForm.qcTextEditor.Focus();
		}
        private void menuItemStopQuery_Click(object sender, System.EventArgs e)
        {
            ActiveQueryForm.StopCurrentExecution();
        }
    
        private void menuItemUpperCase_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.qcTextEditor.SetReseveredWordsToUpperCase();
		}

   		private void menuItemRunCurrentQuery_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.RunCurrentQuery();
		}

		private void miImportXMLStructure_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.GetCreateTablesScriptFromXMLFile();
		}

		private void miImportXMLData_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.GetInsertScriptFromXMLFile();
		}
		#endregion
		#region Events
		private void MainForm_Load(object sender, System.EventArgs e)
		{
			try
			{
				LoadDockManager();
				PopulateDBConnections();
                mInsertProcessIdRecord();//Added by Archana K. on 12/07/13 for Bug-17525
			}
			catch(Exception ex)
			{
				string s = ex.Message;
				throw;
			}	
		}
		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (DockContent content in dockManager.Documents)
			{
				if ( content.GetType() == typeof(FrmQuery) )
				{
					FrmQuery frm = (FrmQuery)content;
					content.Close();
					if ( frm.ClosingCanceled )
					{
						e.Cancel = frm.ClosingCanceled; 
						return;
					}
				}
				else
				{
					content.Close();
				}
			}
                mDeleteProcessIdRecord();//Added by Archana K. on 12/07/13 for Bug-17525
		}
		private void menuItemFindNext_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.FindNext();
		}
		private void MainForm_MdiChildActivate(object sender, System.EventArgs e)
		{

		}
		private void frmSearch_Closing(object sender, CancelEventArgs e)
		{
			try
			{
				persistedSearchItems = frmSearch.SearchItems;
			}
			catch{return;}
		}

        #endregion
		#region Dockmanager
		private void dockManager_ActiveDocumentChanged(object sender, System.EventArgs e)
		{
			try
			{		
				if (m_options.ActiveDocumentChanged)
				{
					string text = "Event: ActiveDocumentChanged.\n";
					if (dockManager.ActiveDocument != null)
						text += "ActiveDocument.Text = " + dockManager.ActiveDocument.Text;
					else
						text += "ActiveDocument = (null)";

					MessageBox.Show(text);
				}
				_activeForm = (System.Windows.Forms.Form)dockManager.ActiveDocument;
				if(dockManager.ActiveDocument is FrmQuery)
				{
					((FrmQuery)dockManager.ActiveDocument).SendToOutPutWindow();
					SetPandelInfo();
					((FrmQuery)dockManager.ActiveDocument).qcTextEditor.Select();
					UpdateQueryMenuBar(true);
				}
				else
					UpdateQueryMenuBar(false);
			}
			catch
			{
				return;
			}
		}
		private void UpdateQueryMenuBar(bool isQueryDialog)
		{
			foreach(MenuItem mi in menuItemTools.MenuItems)
				mi.Enabled=isQueryDialog;
			toolBar.Buttons[10].Enabled=isQueryDialog;
			toolBar.Buttons[11].Enabled=isQueryDialog;
		}
		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            if (e.Button == toolBarButtonNew)
                menuItemNew_Click(null, null);
            else if (e.Button == toolBarButtonSave)
                menuItem_Save_Click(null, null);
            else if (e.Button == toolBarButtonOpen)
                menuItemOpen_Click(null, null);
            else if (e.Button == toolBarButtonClose)
                menuItemClose_Click(null, null);
            else if (e.Button == toolBarButtonSolutionExplorer)
                menuItemToolbox_Click(null, null);
            else if (e.Button == toolBarButtonOutputWindow)
                menuItemOutputWindow_Click(null, null);
            else if (e.Button == toolBarButtonTaskList)
                menuItemTaskList_Click(null, null);
            else if (e.Button == toolBarButtonStop)
                menuItemStopQuery_Click(null, null);
            else if (e.Button == toolBarButtonRun)
            {
                if (this.ActiveQueryForm.Content.Length > 0)
                {
                    this.ActiveQueryForm.RunQuery();
                }
            }
		}
		private DockContent GetContentFromPersistString(string persistString)
		{
			if (persistString == typeof(FrmDBObjects).ToString())
				return m_FrmDBObjects;
			else if (persistString == typeof(FrmOutput).ToString())
				return OutputWindow;
			else if (persistString == typeof(FrmTask).ToString())
				return TaskList;
            else if (persistString == typeof(FrmResults).ToString())
                return ResultWindow;
			return null;
		}
		private void dockManager_ContentAdded(object sender, WeifenLuo.WinFormsUI.DockContentEventArgs e)
		{
			if (m_options.ContentAdded)
			{
				string text = "Event: ContentAdded.\n";
				text += "Content.Text = " + e.Content.Text;
				MessageBox.Show(text);
			}
		}
		private void dockManager_ContentRemoved(object sender, WeifenLuo.WinFormsUI.DockContentEventArgs e)
		{
			if (m_options.ContentRemoved)
			{
				string text = "Event: ContentRemoved.\n";
				text += "Content.Text = " + e.Content.Text;
				MessageBox.Show(text);
			}
		}
		private DockContent FindContent(string text)
		{
			DockContent[] documents = dockManager.Documents;

			foreach (DockContent content in documents)
			{
				if (content.Text.IndexOf(@":\") > 0)
				{
					try
					{
						if (Path.GetFileName(content.Text) == text)
						{
							return content;
						}
					}
					catch{}
				}
				if (content.Text == text)
				{
					return content;
				}
			}
			return null;
		}
		#endregion
		#region Private Methods
		private void LoadDockManager()
		{
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockManager.config");
            dockManager.Parent = this;
		}
		private void InitDockManager()
		{
			// 
			// dockPanel
			// 
			this.dockManager.ActiveAutoHideContent = null;
			this.dockManager.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockManager.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((System.Byte)(0)));
			this.dockManager.Location = new System.Drawing.Point(0, 28);
			this.dockManager.Name = "dockPanel";
			this.dockManager.Size = new System.Drawing.Size(579, 359);
			this.dockManager.TabIndex = 0;
			this.dockManager.ActiveDocumentChanged +=new EventHandler(dockManager_ActiveDocumentChanged);
            /// Raghu
            this.dockManager.DockLeftPortion = Convert.ToDouble("0.30");
            this.dockManager.DockRightPortion = Convert.ToDouble("0.30");
            this.dockManager.DockTopPortion = Convert.ToDouble("0.30");
            this.dockManager.DockBottomPortion = Convert.ToDouble("0.50");
            /// Raghu
            this.Controls.Add(this.dockManager);
		}
		private void InitDockForms()
		{
			m_FrmDBObjects = new FrmDBObjects(this);
			OutputWindow = new FrmOutput(this);
			TaskList = new FrmTask(this);
		}
		private void MenuItemDataBases_Click(object sender, System.EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			int sep = mi.Text.IndexOf(":");
			string server = mi.Text.Substring(0,sep);
			string database = mi.Text.Substring(sep+1,mi.Text.Length-sep-1);
			foreach(MainForm.DBConnection c in DBConnections)
			{
				if(c.ConnectionName == server)
				{
					ActiveQueryForm.SetDatabaseConnection(database,c.Connection);
					break;
				}
			}
		}
   		private void PopulateDBConnections()
		{
            // Default database Select form --Raghu 
            if (!File.Exists(Application.StartupPath + "\\DataSources.config"))
            {
            DataSourceCollection _xdataSourceCollection = new DataSourceCollection();   
            DataSource dataSource = new DataSource();
			dataSource.ID = Guid.NewGuid();
            dataSource.InitialCatalog = oParaObjects.InitialCatalog;    ////Vudyog
            //dataSource.IntegratedSecurity = "SSPI";  Rup 03/07/2010 TKT-2757
            dataSource.IntegratedSecurity = "";
			dataSource.IsConnected = true;
            dataSource.Name = oParaObjects.cServerName;
			dataSource.PersistSecurityInfo = "TRUE";
			dataSource.TimeOut = "15";
            dataSource.UserName = oParaObjects.cUsername;
            dataSource.Password = oParaObjects.cPassword;
			dataSource.FriendlyName = "Company";
			dataSource.ConnectionType=DBConnectionType.MicrosoftSqlClient;
            _xdataSourceCollection.Add(dataSource);
			DataSourceFactory.Save(_xdataSourceCollection);

            //////FrmDBConnections frm = new FrmDBConnections(this);
            //////if (frm.ShowDialogWindow(this) == DialogResult.Cancel)
            //////{
            //////    if (MessageBox.Show(Localization.GetString("MainForm.PopulateDBConnections"), "SQL Editor", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            //////    {
            //////        Application.Exit();
            //////        return;
            //////    }
            //////    else
            //////    {
            //////        PopulateDBConnections();
            //////        return;
            //////    }
            //////}
            }
            try
            {
                OutputWindow.Show(dockManager);
                m_FrmDBObjects.RefreashTreeView(this.DefaultDb);
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    FrmExceptionMessage frm = new FrmExceptionMessage(this, ex);
                    frm.ShowDialog(this);
                    if (ex.Message != "SQL Server does not exist or access denied.")
                    {
                        PopulateDBConnections();
                    }
                    return;
                }
                else
                {
                    throw ex;
                }
            }
            OpenFileContains(_commandLineFile);
            _commandLineFile = "";
		}
        public void OpenFileContains(string fullName)
        {
            string fileName = string.Empty;
            if (fullName.Length > 0)
            {
                fileName = Path.GetFileName(fullName);
                string fileContent = "";
                try
                {
                    StreamReader sr = new StreamReader(fullName);
                    fileContent = sr.ReadToEnd();
                    sr.Close();
                    sr = null;
                    File.Delete(fullName);
                    this.ActiveQueryForm.Content = fileContent;
                }
                catch (Exception exception)
                {
                    //this.ActiveQueryForm.Close();
                    MessageBox.Show(exception.Message);
                }
            }
        }
        public void OpenFile(string fullName)
		{
			Cursor.Current = Cursors.WaitCursor;
			string fileName = string.Empty;
			if (fullName.Length > 0)
			{
				try
				{
					fileName = Path.GetFileName(fullName);
				}
				catch{}
				string fileContent = "";
				DockContent dc = FindContent(fileName);
				if (dc != null)
				{
					dc.Activate();
					Cursor.Current = Cursors.Default;
					return;
				}
                NewQueryform();
				try
				{
					this.ActiveQueryForm.Text = fileName;
					StreamReader sr = new StreamReader(fullName);
					fileContent = sr.ReadToEnd();
					sr.Close();
					sr = null;
					this.ActiveQueryForm.Content = fileContent;
					this.ActiveQueryForm.FileName = fullName;
				}
				catch (Exception exception)
				{
					this.ActiveQueryForm.Close();
					MessageBox.Show(exception.Message);
				}
				finally
				{
					Cursor.Current = Cursors.Default;
				}
			}	
		}
        private void PrintStatus(string msg)
        {
            string FILE_NAME = @"c:\SQLEditor.log";
            StreamWriter sw = File.AppendText(FILE_NAME);

            sw.WriteLine(msg);
            sw.Close();
        }
        private string CheckProd(string tCpassroute)
        {
            //// Raghu  --- Product Checking
            string buffer = "";
            tCpassroute = tCpassroute.Trim();
            if (tCpassroute != "" || tCpassroute != null)
            {
                for (int nLocalval = 0; nLocalval < tCpassroute.Length; nLocalval++)
                {
                    buffer = buffer + Database.Security.Chr(Database.Security.Asc(tCpassroute.Substring(nLocalval, 1).ToString()) / 2).ToString();
                }
            }
            return buffer.ToString();
        }
        public string SetCaption(string tCDBName)
        {
            try
            {
                if (dockManager.ActiveDocument == null) { return ""; }

                string cSqlstr = "";
                ActiveQueryForm.company.Co_Name = "";
                ActiveQueryForm.company.CompId = 0;
                ActiveQueryForm.company.Dbname = tCDBName;
                DBConnection oConn = new DBConnection();
                foreach (MainForm.DBConnection c in DBConnections)
                {
                    oConn = c;
                    break;
                }
                if (oConn == null) { return ""; }
                IDatabaseManager db = DatabaseFactory.CreateNew(oConn.Connection);
                cSqlstr = "Select Top 1 Co_Name,CompId,";
                cSqlstr = cSqlstr + "LTrim(Str(Year(Sta_dt))) + '-' + ltrim(Str(Year(End_dt))) as L_Yn From Vudyog..Co_Mast ";
                cSqlstr = cSqlstr + " Where Dbname = '" + tCDBName.Trim() + "' Order by CompId desc";
                DataTable dv = db.ExecuteCommand(cSqlstr, oConn.Connection, "Vudyog");
                if (dv.Rows.Count != 0)
                {
                    ActiveQueryForm.company.Co_Name = dv.Rows[0]["Co_Name"].ToString().Trim();
                    ActiveQueryForm.company.CompId = Convert.ToInt32(dv.Rows[0]["CompId"]);
                    ActiveQueryForm.company.l_Yn = dv.Rows[0]["L_Yn"].ToString().Trim();
                    return ActiveQueryForm.company.Co_Name;
                }
                else
                {
                    if (tCDBName.ToUpper() == "VUDYOG")
                    {
                        ActiveQueryForm.company.Co_Name = "Udyog Database";
                        ActiveQueryForm.company.CompId = 0;
                        return ActiveQueryForm.company.Co_Name;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        private void LLSCRICPT_Updates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string cSqlstr = null, vchkprod = "", cdbname = "", cScriptfl = "", cFolderName = "", cScriptPath = "";
                int nDownloads = 0;
                DBConnection oConn = new DBConnection();
                foreach (MainForm.DBConnection c in DBConnections)
                {
                    oConn = c;
                    break;
                }
                if (oConn == null) { return; }
                IDatabaseManager db = DatabaseFactory.CreateNew(oConn.Connection);
                if (ActiveQueryForm.company.CompId == 0) { return; }
                cSqlstr = "Select Top 1 CompId,Co_Name,Passroute,dbname,FolderName From Vudyog..Co_Mast Where CompId = " + ActiveQueryForm.company.CompId.ToString();
                DataTable dv = db.ExecuteCommand(cSqlstr, oConn.Connection, "Vudyog");
                if (dv.Rows.Count == 0) { return; }
                cFolderName = dv.Rows[0]["FolderName"].ToString().Trim();
                vchkprod = dv.Rows[0]["Passroute"].ToString().Trim();
                cSqlstr = " if Not Exists(Select [Name] From sysobjects Where [name] = 'ScriptUpdates' and xType = 'U') ";
                cSqlstr = cSqlstr + " Begin";
                cSqlstr = cSqlstr + " Create Table ScriptUpdates (Product varchar(3),lastupdt int)";
                cSqlstr = cSqlstr + " End";
                cdbname = dv.Rows[0]["dbname"].ToString().Trim();
                db.ExecuteSQL(cSqlstr, oConn.Connection, cdbname);
                if (cdbname == "") { return; }
                vchkprod = this.CheckProd(vchkprod);            /// Encrypt Product Code
                vchkprod = vchkprod.Replace("vu", "/").TrimStart().ToString();
                vchkprod = "/gen" + vchkprod;
                string[] strArry = vchkprod.Split('/');
                int LoopMe = 1;
                SQLEditor.USILWeb.USILWeb webClient = new SQLEditor.USILWeb.USILWeb();
                FrmWait ofrmWait = new FrmWait();
                ofrmWait.Show();
                ofrmWait.Status("Please wait...");
                for (int localI = 1; localI < strArry.Length; localI++)
                {
                    cSqlstr = "Select Top 1 lastupdt From ScriptUpdates Where Product = '" + strArry[localI].ToString() + "'";
                    DataTable tblScrupdt = db.ExecuteCommand(cSqlstr, oConn.Connection, cdbname);
                    int nLastUpdt = 0;
                    if (tblScrupdt.Rows.Count != 0)
                    {
                        nLastUpdt = Convert.ToInt32(tblScrupdt.Rows[0]["lastupdt"]);
                    }
                    if (nLastUpdt.ToString() == null)
                    {
                        nLastUpdt = 0;
                    }
                    LoopMe = 1;
                    while (LoopMe != 0)
                    {
                        nLastUpdt = nLastUpdt + 1;
                        cScriptfl = "U_" + strArry[localI] + "_" + nLastUpdt.ToString().PadLeft(5, '0') + ".Sql";
                        string targetUriFormat = "http://www.udyogsoftware.com/ScriptUpdates/" + cScriptfl;
                        cScriptPath = Application.StartupPath + @"\" + cFolderName.ToString() + "\\Uetrigetvalid.fxp";
                        if (File.Exists(cScriptPath))
                        {
                            cScriptPath = Application.StartupPath + @"\" + cFolderName.ToString() + @"\" + cScriptfl;
                        }
                        else
                        {
                            cScriptPath = Application.StartupPath + @"\" + cScriptfl;
                        }
                        ofrmWait.Status("Searching...");
                        webClient.DownloadFile(targetUriFormat, cScriptPath.ToString(), 'Y');
                        if (File.Exists(cScriptPath.ToString()) || webClient.lSuccuss == true)
                        {
                            ofrmWait.Status("Downloading...");
                            ofrmWait.Status("Open Script File...");
                            OpenFile(cScriptPath.ToString());
                            nDownloads = nDownloads + 1;
                        }
                        else
                        {
                            LoopMe = 0;
                            nLastUpdt = nLastUpdt - 1;
                            cSqlstr = " if Exists(Select Product From ScriptUpdates Where Product = '" + strArry[localI] + "') ";
                            cSqlstr = cSqlstr + " Begin";
                            cSqlstr = cSqlstr + " Update ScriptUpdates SET Lastupdt = " + nLastUpdt.ToString() + "Where Product = '" + strArry[localI] + "'";
                            cSqlstr = cSqlstr + " End";
                            cSqlstr = cSqlstr + " else";
                            cSqlstr = cSqlstr + " Begin";
                            cSqlstr = cSqlstr + " Insert Into ScriptUpdates (Product,Lastupdt) Values('" + strArry[localI] + "'," + nLastUpdt.ToString() + ")";
                            cSqlstr = cSqlstr + " End";
                            db.ExecuteSQL(cSqlstr, oConn.Connection, cdbname);
                        }
                    }
                }
                ofrmWait.Status("Finished...");
                ofrmWait.Close();
                if (nDownloads == 0)
                {
                    MessageBox.Show("Updates not found", "SQL Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion
        #region Public Methods
        public static void CopyEmbeddedResource(string resource, string filepath)
		{
			// Copy embedded uml.dtd to same directory as xmi file.
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.Stream file = thisExe.GetManifestResourceStream(resource);
			XmlDocument doc = new XmlDocument();
			doc.Load(file);
			doc.Save(filepath);
		}
		public static void CopyBinaryEmbeddedResource(string resource, string filepath)
		{
			try
			{
				System.Reflection.Assembly thisExe;
				thisExe = System.Reflection.Assembly.GetExecutingAssembly();
				System.IO.Stream stream = thisExe.GetManifestResourceStream(resource);
			
				FileStream fs = File.Create(filepath);
				
				int numBytesToRead = (int) stream.Length;
				int numBytesRead = 0;
				byte[] bytes = new byte[numBytesToRead];

				while (numBytesToRead > 0) 
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = stream.Read(bytes, numBytesRead, numBytesToRead);
					// The end of the file is reached.
					if (n==0)
						break;
					numBytesRead += n;
					numBytesToRead -= n;
					fs.Write(bytes, 0, n);
				}
				stream.Close();
				fs.Close();
			}
			catch
			{
				return;
			}
		}		
		public void SetPandelInfo()
		{
			try
			{
				string conStr = ActiveQueryForm.dbConnection.ConnectionString;
				int spos = conStr.IndexOf("Data Source=")+12;
				int len = conStr.IndexOf(";",spos)-spos;
				string ds = conStr.Substring(spos,len)+"..";
				SetPandelInfo(ds,ActiveQueryForm.DatabaseName);
			}
			catch
			{
				return;
			}
		
		}
		private void SetPandelInfo(string dataSourse, string database)
		{
			this.panel3.Text = "[" + dataSourse + "]..[" + database + "]";
            this.panel2.Text = ActiveQueryForm.company.Co_Name;
            //this.panel4.Text = ActiveQueryForm.company.Co_Name;
		}
		public void SetPandelPositionInfo(string line, string col)
		{
			this.panel5.Text = String.Format("Ln {0}  Col {1}",line,col);
		}
		public void RefreashDBConnections()
		{
			DataSourceCollection _dataSourceCollection = DataSourceFactory.GetDataSources();
			bool connect=true;
			
			DBConnections.Clear();
			foreach(DataSource ds in _dataSourceCollection)
			{
				if(ds.IsConnected)
				{
					DBConnection dbConnection = new DBConnection();
					dbConnection.ConnectionString = ds.ConnectionString;
					dbConnection.ConnectionName  = ds.Name;
					try
					{
						dbConnection.Connection = DatabaseFactory.GetConnection(ds.ConnectionType,ds.ConnectionString);
						dbConnection.Connection.Open();
						dbConnection.IsConnected = true;
						dbConnection.InitialCatalog = ds.InitialCatalog;
						dbConnection.FrienlyName = ds.FriendlyName;
						DBConnections.Add(dbConnection);
					}
					catch(Exception ex)
					{
						throw ex;
					}
				}
			}
		}
		public void RefreshDataObjectTree()
		{
			m_FrmDBObjects.RefreashTreeView(null);
		}
		public void ShowTaskWindow()
		{
			try
			{
                OutputWindow.Show(dockManager);
				TaskList.Show(dockManager);
			}
			catch
			{
				return;
			}
		}
		public void ShowDBObjects()
		{
			m_FrmDBObjects.Show(dockManager);
		}
		public void ActivateMe(FrmQuery frmQuery)
		{
			ActiveQueryForm = frmQuery;
		}
		public void NewQueryform()
		{
			DBConnection dbConnection;
			IDbConnection connection=null;
			string DatabaseName="";
			Application.DoEvents();
			if(QueryForms.Count<=1)
			{
				dbConnection = (DBConnection)this.DBConnections[0];
				connection = dbConnection.Connection;
                if (this.DefaultDb != null && this.DefaultDb != "")
                {
                    DatabaseName = this.DefaultDb;
                }
                else
                {
                    DatabaseName = dbConnection.InitialCatalog;
                }
			}
			else
			{
				connection = ActiveQueryForm.dbConnection;
				DatabaseName=ActiveQueryForm.DatabaseName;
			}
			FrmQuery frmquery = new FrmQuery( this );
			int count = QueryForms.Count+1;
			string text = "Query" + count.ToString();
			while (FindContent(text) != null)
			{
				count ++;
                text = "Query" + count.ToString();
			}
			frmquery.Text = text;
			frmquery.Show(dockManager);
			QueryForms.Add(frmquery);
			frmquery.SetDatabaseConnection(DatabaseName,connection);
		}
 
		public void AlterDatabaseMenuItem(ArrayList dbArr)
		{
			contextMenuDataBases.MenuItems.Clear();
			foreach(SQLEditor.Database.DB db in dbArr)
			{
				System.Windows.Forms.MenuItem mi = new System.Windows.Forms.MenuItem();
				mi.Text = db.Server+":"+db.Name;
				mi.Click += new System.EventHandler(this.MenuItemDataBases_Click);
				menuExtender.SetMenuImage(mi,"27");
				contextMenuDataBases.MenuItems.Add(mi);
			}
		}

		public void UpdateEditorSettings()
		{
			SQLEditor.Config.Settings settings = SQLEditor.Config.Settings.Load();
			foreach(FrmQuery frm in this.QueryForms)
			{
				frm.qcTextEditor.ShowEOLMarkers=settings.ShowEOLMarkers;
				frm.qcTextEditor.ShowSpaces=settings.ShowSpaces;
				frm.qcTextEditor.ShowTabs=settings.ShowTabs;
				frm.qcTextEditor.ShowMatchingBracket=settings.ShowMatchingBracket;
				frm.qcTextEditor.ShowLineNumbers=settings.ShowLineNumbers;
				frm.qcTextEditor.Font=settings.GetFont();
				frm.qcTextEditor.ShowVRuler=false;
				frm.qcTextEditor.TabIndent=settings.TabIndent;
			}
			settings.Save();
		}

		public void SaveDocument()
		{
			SaveDocument(ActiveQueryForm);
		}
		private void MainForm_Closed(object sender, System.EventArgs e)
		{
			m_MainForm.Dispose();
			Application.Exit();
		}
		private void menuItemPrint_Click(object sender, System.EventArgs e)
		{
			FrmPrint frmPrint = new FrmPrint(ActiveQueryForm);
			frmPrint.ShowDialogWindow(this);
		}
		private void menuItemPageSetUp_Click(object sender, System.EventArgs e)
		{
			ActiveQueryForm.PrintPageSetUp();
		}
		public void SaveDocument(FrmQuery frmQuery)
		{
			string directoryPath = Application.UserAppDataPath;
			try
			{
				FileInfo fi = new FileInfo( frmQuery.FileName );
				directoryPath = fi.DirectoryName; 
				if(!Directory.Exists(directoryPath))
					Directory.CreateDirectory(directoryPath);
			}
			catch
			{
				// ignore the error as the query form may not have a filename
			}
			// Create a SaveFileDialog to request a path and file name to save to.
			SaveFileDialog saveFile1 = new SaveFileDialog();

			string defaultFileName ="";
			if(frmQuery.Text.IndexOf("Query1 [")<0)
				defaultFileName = ( frmQuery.FileName.Trim().Length  > 0 ? frmQuery.FileName : frmQuery.Text.TrimEnd('*') ); //frmQuery.Text + ".sql";
			
			// Initialize the SaveFileDialog to specify the RTF extension for the file.
			saveFile1.RestoreDirectory = false;
			saveFile1.AddExtension = true;
			//saveFile1.InitialDirectory = directoryPath;
			saveFile1.DefaultExt = "*.sql";
			saveFile1.Filter = "SQL Files|*.sql";
			saveFile1.FileName = defaultFileName;
			try
			{
				// Determine if the user selected a file name from the saveFileDialog.
				if(saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
					saveFile1.FileName.Length > 0) 
				{
					// Save the contents of the RichTextBox into the file.
					frmQuery.SaveAs( saveFile1.FileName );
					frmQuery.Text = saveFile1.FileName;
				}
			}
			catch( Exception ex)
			{
				MessageBox.Show ( string.Format("Errors Ocurred\n{0}", ex.Message), "Save Error", MessageBoxButtons.OK);
			}
		}
		#endregion
		#region Classes
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
		}
		public class DBConnection
		{
			public string ConnectionName = "";
			public string FrienlyName="";
			public string ConnectionString = "";
			public IDbConnection Connection;
			public bool IsConnected; 
			public TreeNode treeNode;
			public string InitialCatalog;
		}

        public class ParaObjects
        {
            public string cServerName = "";
            public string cPassword = "";
            public string cUsername = "";
            public string InitialCatalog = "Vudyog";
        }

		#endregion
	}
}

