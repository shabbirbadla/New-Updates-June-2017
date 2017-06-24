using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using DevExpress.XtraExport;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils.Menu;
using LogicalLibrary;
//using uevoucher;        // Added By Shrikant S. on 09/06/2012 
using vunettofx;           // Added By Shrikant S. on 29/12/2012
using System.Text.RegularExpressions;//Added By Archana K. on 11/12/2012  for Bug-7316
using DataAccess_Net;//Added by Archana K. on 11/10/13 for Bug-19926
using System.Diagnostics;//Added by Archana K. on 11/10/13 for Bug-19926

namespace DadosReports
{
    #region this is the main class
    public partial class ReportsMain : uBaseForm.FrmBaseForm
    //public partial class ReportsMain : Form
    {
        #region Public Veriables
        string[] ColumnHints = null;
        string attachfilename = string.Empty;
        string ReportID = string.Empty;
        SqlConnection con = null;
        SqlCommand Command = null;
        string ReportConString = string.Empty;
        string[] ReportQuary = null;
        string[] ReportQuaryID = null;
        string[] ReportDisplayType = null;
        string[] ReportDispalyName = null;
        string[] ReportPrimaryKeyValue = null;
        string[] ReportSecunderyKeyValue = null;
        string[] ReportLevelTypeID = null;
        bool GroupSFooter = false;
        string GroupfooterMode = string.Empty;
        SqlDataAdapter Adapter = null;
        DataSet DS = null;
        string ReportType = string.Empty;
        int LevelCount = 0;
        DataSet LocalDS = null;
        DataSet ColumnDetailsDS = null;
        string ReportName = string.Empty;
        string User = string.Empty;
        string Domain = string.Empty;
        string VuMess = "Dados Reports";            //Added By Shrikant S. on 08/06/2012    //Start

        public string _mCoSta_dt = string.Empty;
        public string _mCoEnd_dt = string.Empty;
        public string Defapath = string.Empty;
        public string _mAppPath = string.Empty;
        public string _mProdCode = string.Empty;
        public string MainIcon = string.Empty;      //Added By Shrikant S. on 08/06/2012    //End
        public bool IsMultiCompany = false;         //Added By Shrikant S. on 21/05/2013 for Bug-11169
        List<string> paraarguments = new List<string>(); //Added by Archana K. on 28/03/13 for Bug-10253 

        #region Arguments veriables
        string[] stringArguments = null;
        string[] numaricArguments = null;
        string[] datetimeArguments = null;
        string[] LoginUserName = null;
       
        #endregion

        MenuInfo menuInfo = new MenuInfo();
        EncriptionValues encriptValues = new EncriptionValues();
        CommonInfo commonInfo = new CommonInfo();
        ControlsLogic controlsLogic = new ControlsLogic();
        DBOperations dbOperations = new DBOperations();
        //Added by Archana K. on 11/10/13 for Bug-19926 start
        const int Timeout = 5000;
        private String cAppPId, cAppName, pPApplCode, pPApplName, pPApplText;
        private Icon pFrmIcon;
          //private short pPApplPID;//Commented by Archana K. on 12/11/13 for Bug-19806
          private double pPApplPID1;//Added by Archana K. on 12/11/13 for Bug-19806
        //Added by Archana K. on 11/10/13 for Bug-19926 end
        DataAccess_Net.clsDataAccess oDataAccess;//Added by Archana K. on 11/10/13 for Bug-19926
        Boolean flag;//Added by Archana K. on 14/03/14 for Bug-22080
        frmLayOut frmReportLayout;//Added by Archana K. on 14/03/14 for Bug-22080
        #endregion

        #region Public Properties
        private string ReportLoginUser = string.Empty;
        public string ReportLoginUserName
        {
            get { return ReportLoginUser; }
            set { ReportLoginUser = value; }
        }
        private string StringValues = string.Empty;
        public string ReportStringValues
        {
            get { return StringValues; }
            set { StringValues = value; }
        }
        private string NumaricValues = string.Empty;
        public string ReportNumaricValues
        {
            get { return NumaricValues; }
            set { NumaricValues = value; }
        }
        private string DateTimeValues = string.Empty;
        public string ReportDateTimeValues
        {
            get { return DateTimeValues; }
            set { DateTimeValues = value; }
        }
        private string ReportCon;

        public string ReportConnectionString
        {
            get { return ReportCon; }
            set { ReportCon = value; }
        }
        //Added by Archana K. on 19/03/14 for Bug-22080 start
        private string ReportLayoutnm= string.Empty;
        public string ReportLayoutnm1
        {
            get { return ReportLayoutnm; }
            set { ReportLayoutnm = value; }
        }
        private int ReportLvlId = 0;
        public int ReportLvlId1
        {
            get { return ReportLvlId; }
            set { ReportLvlId = value; }
        }
        private bool IsDefault;
        public bool IsDefault1
        {
            get { return IsDefault; }
            set { IsDefault = value; }
        }
        //Added by Archana K. on 19/03/14 for Bug-22080 end
        #endregion

        #region Constractor
        public ReportsMain(string[] args)
        {
            System.Threading.Thread.Sleep(5000);
            InitializeComponent();
            string[] argument = null; //new string[args.Length];

            //if (args.Length == 1)             //Commented By Shrikant S. on 09/06/2012
            //{
            //    argument = args[0].Split(new char[] { '|' });
            //}
            // Added By Shrikant S. on 09/06/2012   // Start  
            argument = args[0].Split('|');
            this.pCompId = Convert.ToInt16(args[1]);
            this.pComDbnm = args[2].Replace("<*#*>", " ");
            this.pServerName = args[3].Replace("<*#*>", " ");
            this.pUserId = args[4].Replace("<*#*>", " ");
            this.pPassword = args[5].Replace("<*#*>", " ");
            this.pPApplRange = args[6].Replace("<*#*>", " ");
            this.pAppUerName = args[7].Replace("<*#*>", " ");
            MainIcon = args[8].Replace("<*#*>", " ");
            this.pPApplText = args[9].Replace("<*#*>", " ");
            this.pPApplName = args[10].Replace("<*#*>", " ");
            // this.pPApplPID = Convert.ToInt16(args[11]);//commented by Archana on 12/11/13 for Bug-19806
            this.pPApplPID1= Convert.ToDouble(args[11]); //changed by Archana on 12/11/13 for Bug-19806
            this.pPApplCode = args[12].Replace("<*#*>", " ");
            _mProdCode = args[13].Replace("<*#*>", " ");
            Defapath = args[14].Replace("<*#*>", " ");
            _mAppPath = args[15].Replace("<*#*>", " ");
            _mCoSta_dt = args[16].Replace("<*#*>", " ");
            _mCoEnd_dt = args[17].Replace("<*#*>", " ");

            ReportID = argument[0].ToString();
            ReportLoginUser = argument[1].ToString();
            StringValues = argument[2].ToString();
            NumaricValues = argument[3].ToString();
            DateTimeValues = argument[4].ToString();
            ReportCon = argument[5].ToString();
            // Added By Shrikant S. on 09/06/2012   // End
            this.Hide();
        }
        #endregion

        #region Form Events

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                BindGrid();//binding the grid

                #region binding thems in list box and combo box
                listBoxTheme = controlsLogic.bindListBox(listBoxTheme, navBarControl1);//binding the list box with availble themes but it will not appear in the screen
                comboBoxEdit2 = controlsLogic.InitSkinNames(gridControl1.LookAndFeel, comboBoxEdit2);// binding the dropdown box with default theams but it will not appear in the screen

                #endregion
                #region Binding ListView Columns
                listView1.View = View.Details;
                listView1.Activation = ItemActivation.OneClick;
                listView1.Columns.Add("Serial No", 200, HorizontalAlignment.Center);
                listView1.Columns.Add("Comments", 500, HorizontalAlignment.Center);

                listView2.View = View.Details;
                listView2.Activation = ItemActivation.OneClick;
                listView2.Columns.Add("Serial No", 200, HorizontalAlignment.Center);
                listView2.Columns.Add("Summary", 500, HorizontalAlignment.Center);
                #endregion
                #region Images Menus Selections

                #region ShowFooter
                if (this.gridView1.OptionsView.ShowFooter)
                {
                    //barButtonItem_SummaryFooter.ImageIndex = 174;
                }
                else
                {
                    //barButtonItem_SummaryFooter.ImageIndex = 147;
                }
                #endregion
                #region ShowIndicator
                if (gridView1.OptionsView.ShowIndicator)
                {
                    barCheckItem_ShowIndicator.Checked = true;
                }
                else
                {
                    barCheckItem_ShowIndicator.Checked = false;
                }
                #endregion

                #region ShowGroupPanel
                if (gridView1.OptionsView.ShowGroupPanel)
                {
                    barCheckItem_ShowGroupPanal.Checked = true;
                }
                else
                {
                    barCheckItem_ShowGroupPanal.Checked = false;
                }
                #endregion

                #region SummaryFooter
                if (gridView1.OptionsView.ShowFooter)
                {
                    barCheckItem_SummaryFooter.Checked = true;
                }
                else
                {
                    barCheckItem_SummaryFooter.Checked = false;
                }
                #endregion

                #region GroupSummaryFooter
                GroupfooterMode = gridView1.GroupFooterShowMode.ToString();

                if (GroupfooterMode == "VisibleAlways")
                {
                    GroupSFooter = true;
                }
                else
                {
                    GroupSFooter = false;
                }

                if (GroupSFooter)
                {
                    barCheckItem_GroupSummaryFooter.Checked = true;
                }
                else
                {
                    barCheckItem_GroupSummaryFooter.Checked = false;
                }
                #endregion

                #region ShowHeaders
                if (gridView1.OptionsView.ShowColumnHeaders)
                {
                    barCheckItem_ShowHeaders.Checked = true;
                }
                else
                {
                    barCheckItem_ShowHeaders.Checked = false;
                }
                #endregion

                #region ShowVLines
                if (this.gridView1.OptionsView.ShowVertLines)
                {
                    barCheckItem_ShowVLines.Checked = true;
                }
                else
                {
                    barCheckItem_ShowVLines.Checked = false;
                }
                #endregion

                #region ShowHorzLines
                if (this.gridView1.OptionsView.ShowHorzLines)
                {
                    barCheckItem_ShowHLines.Checked = true;
                }
                else
                {
                    barCheckItem_ShowHLines.Checked = false;
                }
                #endregion

                #region MultiSelectionRow
                if (this.gridView1.OptionsSelection.MultiSelect)
                {
                    barCheckItem_MultiSelectionRow.Checked = true;
                }
                else
                {
                    barCheckItem_MultiSelectionRow.Checked = false;
                }
                #endregion

                #region Show ToolTip
                if (gridControl1.ToolTipController == toolTipController1)
                {

                    barCheckItem_ShowDetailToolTip.Checked = true;


                }
                else
                {
                    barCheckItem_ShowDetailToolTip.Checked = false;

                }
                #endregion

                #region Show Incremental Search

                if (gridView1.OptionsBehavior.AllowIncrementalSearch)
                {
                    barCheckItem_IncrementalSearch.Checked = true;
                }
                else
                {
                    barCheckItem_IncrementalSearch.Checked = false;
                }

                #endregion

                #region Allow Cell Merging

                if (gridView1.OptionsView.AllowCellMerge)
                {
                    barCheckItem_AllowCellMerging.Checked = true;
                }
                else
                {
                    barCheckItem_AllowCellMerging.Checked = false;
                }

                #endregion

                #region Auto Hight

                if ((gridView1.OptionsView.ColumnAutoWidth == true) && (gridView1.OptionsView.RowAutoHeight == true))
                {
                    barCheckItem_AutoHight.Checked = true;
                }
                else
                {
                    barCheckItem_AutoHight.Checked = false;
                }

                #endregion

                #endregion
                #region RuntimeCustomizetion Controls Code

                bindSelectLevelCombobox();
                bindSelectLevelCombobox_StyleConditions();

                #endregion

                //HidingColumns();//for hiding the grid indexed columns

                //Close the splash
                Loading.CloseSplash();
                this.Show();

                //RestoreReportLayOut(string.Empty, 0);      // Added By Shrikant S. on 08/06/2012//Commented by Archana K. on 19/03/14 for Bug-22080
                //ReportLayoutnm = comboBoxEdit_SelectLevel.Text ;//Added by Archana k.on 20/03/14 for Bug-22080
                RestoreReportLayOut(ReportLayoutnm, 0);      // Added By Shrikant S. on 08/06/2012 //changed by Archana K. on 19/03/14 for Bug-22080
                bindSelectLayoutCombo();                // Added By Shrikant S. on 08/06/2012 
                this.CheckCompanyType();                //Added By Shrikant S. on 21/05/2013 for Bug-11169
                //Added by Archana K. on 11/10/13 for Bug-19926 start
                DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
                DataAccess_Net.clsDataAccess._serverName = this.pServerName;
                DataAccess_Net.clsDataAccess._userID = this.pUserId;
                DataAccess_Net.clsDataAccess._password = this.pPassword;
                oDataAccess = new DataAccess_Net.clsDataAccess();
                this.mInsertProcessIdRecord();
                //Added by Archana K. on 11/10/13 for Bug-19926 end
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //Application.Exit(); ///Commented by Archana K. on 29/03/13 for Bug-10253 
                return; //Added by Archana K. on 29/03/13 for Bug-10253 
            }
        }

        private void ReportsMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = false;
            }
            else
            {
                if (MessageBox.Show(this, "Do You Really Want To Exit??", VuMess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            mDeleteProcessIdRecord();//Added by Archana K. on 11/10/13 for Bug-19926 
        }
        //Added by Archana K. on 10/10/13 for Bug-19926 start
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "DadosReports.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            //sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + " Dados Report')";//Commented by Archana K. on 12/12/13 for Bug-19806
            //sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID1 + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + " Dados Report')";//Changed by Archana K. on 12/12/13 for Bug-19806
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "',Convert(SmallDateTime,GetDate()),'" + this.pPApplName + "'," + this.pPApplPID1 + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + " Dados Report')";//Changed by Shrikant S. on 21/11/2013 for Bug-19226
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()
        {
            //if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))//Commented by Archana K. on 12/12/13 for Bug-19806
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID1 == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))//Changed by Archana K. on 12/12/13 for Bug-19806
            {
               return;
                
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            //sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;//Commented by Archana K. on 12/12/13 for Bug-19806
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID1 + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;//Changed by Archana K. on 12/12/13 for Bug-19806
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        //Added by Archana K. on 10/10/13 for Bug-19926 end
        #endregion

        #region Binding Grid Control

        private void BindGrid()
        {
            try
            {
                #region Verifying Report ID is Blank or not
                if (ReportID != "")
                {
                    #region Based on the Report ID Binding Data
                    string conString = ReportConnectionString;
                    LocalDS = new DataSet();
                    ReportConString = conString;
                    if (ReportConString != "")
                    {
                        #region To Get the Report Type And Report Name
                        LocalDS = dbOperations.GetReportTypeAndName(ReportConString, ReportID, "TblReportDetails");
                        ReportType = LocalDS.Tables["TblReportDetails"].Rows[0]["ReportType"].ToString();
                        ReportName = LocalDS.Tables["TblReportDetails"].Rows[0]["ReportName"].ToString();
                        #endregion

                        #region If the report is View Type below Code will Exicute
                        if (ReportType.ToLower() == "View".ToLower())
                        {
                            #region To Get the Level Count
                            LocalDS = dbOperations.GetReportsLevelCount(ReportConString, ReportID, "TblLevelsCount");
                            LevelCount = Convert.ToInt32(LocalDS.Tables["TblLevelsCount"].Rows[0]["levels"]);
                            #endregion

                            #region To Get the Table ReportLevels And Types
                            LocalDS = dbOperations.GetReportLevelsAndTypes(ReportConString, ReportID, "TblReportLevelsAndTypes");
                            #endregion

                            #region To Get the Quarys,Display Type, Display Name,Primary, secoundery values
                            if (LocalDS.Tables["TblReportLevelsAndTypes"].Rows.Count > 0)
                            {
                                ReportQuary = new string[LevelCount];
                                ReportQuaryID = new string[LevelCount];
                                ReportDisplayType = new string[LevelCount];
                                ReportDispalyName = new string[LevelCount];
                                ReportPrimaryKeyValue = new string[LevelCount];
                                ReportSecunderyKeyValue = new string[LevelCount];
                                ReportLevelTypeID = new string[LevelCount];
                                for (int i = 0; i < LevelCount; i++)
                                {
                                    ReportQuary[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["ReportQuery"].ToString();
                                    ReportQuaryID[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["QueryID"].ToString();
                                    ReportDisplayType[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["ReportDisplayType"].ToString();
                                    ReportDispalyName[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["ReportDisplayName"].ToString();
                                    ReportPrimaryKeyValue[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["PrimaryKeyValu"].ToString();
                                    ReportSecunderyKeyValue[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["SecunderyKeyValu"].ToString();
                                    ReportLevelTypeID[i] = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["ReportLevelTypeID"].ToString();
                                }
                            }
                            #endregion

                            #region Based on the Quary count binding the Data Set
                            if (ReportQuary.Length > 0)
                            {
                                DS = new DataSet();
                                gridControl1.DataSource = null;
                                gridView1.Columns.Clear();
                                #region Based on the Display Count binding the table in DataSet
                                for (int LevelValue = 0; LevelValue < LevelCount; LevelValue++)
                                {
                                    con = new SqlConnection(ReportConString);
                                    Command = new SqlCommand();
                                    Command.CommandType = CommandType.Text;
                                    Command.CommandText = ReportQuary[LevelValue].ToString();
                                    //Added by Archana K. on 11/12/2012 for Bug-7316 start
                                    //string CharacterToFind = "@";
                                    //Regex exp = new Regex(CharacterToFind, RegexOptions.IgnoreCase);
                                    //int noOfOccurrences = exp.Matches(Command.CommandText).Count;
                                    //Added by Archana K. on 11/12/2012 for Bug-7316 end
                                    Command.Connection = con;
                                    Command.CommandTimeout = 4000; //Added by Shrikant S. on 23/09/2011 for TKT-9292
                                    Command.Parameters.Clear();
                                    #region Geting Arguments
                                    if (DS.Tables.Count == 0)
                                    {
                                        if (ReportLoginUserName != "" && ReportLoginUserName != " ")
                                        {
                                            LoginUserName = commonInfo.spiltArguments(ReportLoginUserName);
                                            for (int i = 0; i < LoginUserName.Length; )
                                            {
                                                User = LoginUserName[i + 1].ToString();
                                                Domain = LoginUserName[i + 3].ToString();//LoginUserName[i + 2].ToString() + ":" +
                                                break;
                                            }

                                        }
                                        if (ReportStringValues != "" && ReportStringValues != " ")
                                        {
                                            stringArguments = commonInfo.spiltArguments(ReportStringValues);
                                            for (int i = 0; i < (stringArguments.Length); i++)
                                            {
                                                Command.Parameters.AddWithValue("@" + stringArguments[i].ToString(), stringArguments[i + 1].ToString());
                                                paraarguments.Add("@" + stringArguments[i].ToString());//Added by Archana K. on 28/03/13 for Bug-10253 
                                                i = i + 1;
                                            }
                                        }
                                        if (ReportNumaricValues != "" && ReportNumaricValues != " ")
                                        {
                                            numaricArguments = commonInfo.spiltArguments(ReportNumaricValues);
                                            for (int i = 0; i < (numaricArguments.Length); i++)
                                            {
                                                Command.Parameters.AddWithValue("@" + numaricArguments[i].ToString(), numaricArguments[i + 1].ToString());
                                                paraarguments.Add("@" + numaricArguments[i].ToString());//Added by Archana K. on 28/03/13 for Bug-10253 
                                                i = i + 1;
                                            }
                                        }
                                        if (ReportDateTimeValues != "" && ReportDateTimeValues != " ")
                                        {
                                            datetimeArguments = commonInfo.spiltArguments(ReportDateTimeValues);
                                            for (int i = 0; i < (datetimeArguments.Length); i++)
                                            {
                                                Command.Parameters.AddWithValue("@" + datetimeArguments[i].ToString(), datetimeArguments[i + 1].ToString());
                                                paraarguments.Add("@" + datetimeArguments[i].ToString());//Added by Archana K. on 28/03/13 for Bug-10253 
                                                i = i + 1;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region filling the Adapter and Data Set
                                    //Added by Archana K. on 23/03/2013 for Bug-10253 start
                                    string CharacterToFind;
                                    int noOfOccurrences = 0;
                                    List<string> noOfOccurrences1 = new List<string>();
                                    for (int i = 0; i < paraarguments.Count; i++)
                                    {
                                        CharacterToFind = paraarguments[i].ToString();
                                        Regex ParaArgumentchk = new Regex(@CharacterToFind);
                                        MatchCollection matches = ParaArgumentchk.Matches(Command.CommandText);
                                        foreach (Match match in matches)
                                        {
                                            var para = match.Groups[1].Value;
                                            if (!noOfOccurrences1.Contains(para)) noOfOccurrences1.Add(para);
                                        }
                                        noOfOccurrences = noOfOccurrences + noOfOccurrences1.Count;

                                    }

                                    //Commented by Archana K. on 11/12/2012 for Bug-7316 start  
                                    //Adapter = new SqlDataAdapter(Command);
                                    //Adapter.Fill(DS, ReportDispalyName[LevelValue].ToString());
                                    //Commented by Archana K. on 11/12/2012 for Bug-7316 end
                                    //Changed by Archana K. on 11/12/2012 for Bug-7316 start
                                        int NoParamteresCount = Command.Parameters.Count;
                                        //try  //Commented by Archana K. on 28/03/2013 for Bug-10253    
                                        //{
                                        if (noOfOccurrences == NoParamteresCount || (noOfOccurrences == 0 && NoParamteresCount == 0))
                                        {
                                            try  //Added by Archana K. on 28/03/2013 for Bug-10253   
                                            {
                                                Adapter = new SqlDataAdapter(Command);
                                                Adapter.Fill(DS, ReportDispalyName[LevelValue].ToString());
                                            }
                                            catch (Exception ex) 
                                            {
                                                MessageBox.Show("Error: " + ex.Message, VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                                //Application.Exit(); ///Commented by Archana K. on 29/03/13 for Bug-10253 
                                                return; //Added by Archana K. on 29/03/13 for Bug-10253 
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Number of parameters are not matching", VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                            return;

                                        }
                                        //Added by Archana K. on 01/04/2013 for Bug-10253 start
                                        noOfOccurrences = 0;
                                        noOfOccurrences1.Clear();
                                        paraarguments.Clear();
                                        //Added by Archana K. on 01/04/2013 for Bug-10253 end
                                        //Commented by Archana K. on 28/03/2013 for Bug-10253 start
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    MessageBox.Show("Error: " + ex.Message, VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        //    Application.Exit();
                                        //}
                                        //Commented by Archana K. on 28/03/2013 for Bug-10253 end
                                    //Changed by Archana K. on 11/12/2012 for Bug-7316 end
                                   
                                    #endregion

                                    #region to Maintaine the reletion of the chaild grids
                                    if (LevelValue == 0) { }
                                    else
                                    {
                                        DataColumn KeyColumnItem = DS.Tables[0].Columns[ReportPrimaryKeyValue[LevelValue].ToString()];
                                        DataColumn foreignKeyColumnItem = DS.Tables[ReportDispalyName[LevelValue].ToString()].Columns[ReportSecunderyKeyValue[LevelValue].ToString()];

                                        DS.Relations.Add(ReportDispalyName[LevelValue].ToString(), KeyColumnItem, foreignKeyColumnItem, false);
                                    }
                                    #endregion
                                }
                                #endregion
                                
                            }

                            #endregion

                            #region binding tables to Grid Control
                            gridControl1.DataSource = DS.Tables[0];
                            gridControl1.ForceInitialize();
                            gridView1.PopulateColumns();
                            gridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                            //gridView1.BestFitColumns();//Comment by Archana Khade 05/4/2012 for TKT-3143 

                            #endregion

                            #region to display user id in status bar and display the report name as title
                            StatusBar_barStaticItem.Caption = "User Name : " + User;

                            this.Text = ReportName + "-" + Domain;
                            #endregion

                            #region To Display GridControl with proper Cailds
                            for (int LevelIndex = 0; LevelIndex < LevelCount; LevelIndex++)
                            {
                                if (LevelIndex == 0)
                                {
                                    #region to Display Main View
                                    if (ReportDisplayType[LevelIndex].ToString() == "Grid View")
                                    {
                                        gridControl1.MainView = this.gridView1;
                                        gridView1.PopulateColumns();
                                        gridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //gridView1.BestFitColumns();//Comment by Archana Khade 05/4/2012 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < gridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (gridView1.Columns[i].Caption.Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().Trim())        //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (gridView1.Columns[i].Caption.Trim().ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().Trim().ToUpper())          //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    //if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)        // Commented By Shrikant S. on 06/06/2012 for Bug-4522
                                                    //{
                                                    //    gridView1.Columns[i].Group();
                                                    //}
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview start(TKT-3143)
                                                    if (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString() != string.Empty)
                                                    {

                                                        gridView1.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString();
                                                    }
                                                    else
                                                    {
                                                        gridView1.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString();
                                                    }
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview end(TKT-3143)
                                                    //gridView1.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);//Added by Archana Khade on 05/04/2012 for TKT-3143    //Commented By Shrikant S. on 06/06/2012 for bug-4522

                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        gridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        gridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        gridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        gridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        gridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 06/06/2012 for Bug-4522  && Start
                                        for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                        {
                                            for (int i = 0; i < gridView1.Columns.Count; i++)
                                            {
                                                //if (gridView1.Columns[i].Caption.ToUpper().Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper().Trim())        // Commented By Shrikant S. on 04/02/2013 
                                                string colName = (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString().Length > 0 ? ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString() : ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString());    // Added By Shrikant S. on 04/02/2013
                                                if (gridView1.Columns[i].Caption.ToUpper().Trim() == colName.ToUpper().Trim() && (bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == true)         // Added By Shrikant S. on 04/02/2013
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        gridView1.Columns[i].GroupIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["grouporder"]);
                                                        // gridView1.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);//added archana 11/05/12
                                                    }
                                                    else
                                                    {
                                                        gridView1.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);//Add by Archana khade 05/04/12
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 06/06/2012 for Bug-4522  && End
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Card View")
                                    {
                                        cardView1.OptionsView.ShowCardCaption = true;
                                        cardView1.CardCaptionFormat = "{1}";
                                        cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
                                        cardView1.CardWidth = 600;
                                        gridControl1.MainView = cardView1;
                                        cardView1.PopulateColumns();
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < cardView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (cardView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())      //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (cardView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())        //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        cardView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        cardView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        cardView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        cardView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        cardView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        cardView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Bended View")
                                    {
                                        gridControl1.MainView = bandedGridView1;
                                        bandedGridView1.PopulateColumns();
                                        bandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        // bandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/2012 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < bandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (bandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())    //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (bandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())      //Added By Shrikant S. on 21/05/2013 for Bug-11169      
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        bandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        bandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        bandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Advanced Bended View")
                                    {
                                        gridControl1.MainView = advBandedGridView1;
                                        advBandedGridView1.PopulateColumns();
                                        advBandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        advBandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (advBandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())     //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (advBandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())       //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        advBandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        advBandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        advBandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (LevelIndex == 1)
                                {
                                    #region To Display First chaild Level
                                    if (ReportDisplayType[LevelIndex].ToString() == "Grid View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), gridView2);
                                        gridView2.PopulateColumns(DS.Tables[LevelIndex]);
                                        gridView2.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //gridView2.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < gridView2.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (gridView2.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())          //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (gridView2.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())    //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    //if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)        // Commented By Shrikant S. on 08/06/2012
                                                    //{
                                                    //    gridView2.Columns[i].Group();
                                                    //}
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        gridView2.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        gridView2.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        gridView2.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        gridView2.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        gridView2.Columns[i].Visible = false;
                                                    }
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview start(TKT-3143)  //Added By Shrikant S. on 08/06/2012    //Start
                                                    if (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString() != string.Empty)
                                                    {

                                                        gridView2.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString();
                                                    }
                                                    else
                                                    {
                                                        gridView2.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString();
                                                    }
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview end(TKT-3143)    //Added By Shrikant S. on 08/06/2012    //End
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 08/06/2012  && Start
                                        for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                        {
                                            for (int i = 0; i < gridView2.Columns.Count; i++)
                                            {
                                                //if (gridView2.Columns[i].Caption.ToUpper().Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper().Trim())        // Commented By Shrikant S. on 04/02/2013
                                                string colName = (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString().Length > 0 ? ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString() : ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString());    // Added By Shrikant S. on 04/02/2013
                                                if (gridView2.Columns[i].Caption.ToUpper().Trim() == colName.ToUpper().Trim() && (bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == true)         // Added By Shrikant S. on 04/02/2013
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        gridView2.Columns[i].GroupIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["grouporder"]);
                                                    }
                                                    else
                                                    {
                                                        gridView2.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);//Add by Archana khade 05/04/12
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 08/06/2012  && End
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Card View")
                                    {
                                        cardView1.OptionsView.ShowCardCaption = true;
                                        cardView1.CardCaptionFormat = "{1}";
                                        cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
                                        cardView1.CardWidth = 600;
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), cardView1);
                                        cardView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < cardView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (cardView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())          //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (cardView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())        //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        cardView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        cardView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        cardView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        cardView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        cardView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        cardView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Bended View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), bandedGridView1);
                                        bandedGridView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        bandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        // bandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < bandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (bandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())        //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (bandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())      //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        bandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        bandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        bandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Advanced Bended View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), advBandedGridView1);
                                        advBandedGridView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        advBandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //advBandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (advBandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())     //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (advBandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())       //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        advBandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        advBandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        advBandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (LevelIndex == 2)
                                {
                                    #region To Display Secound chaild Level
                                    if (ReportDisplayType[LevelIndex].ToString() == "Grid View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), gridView3);
                                        gridView3.PopulateColumns(DS.Tables[LevelIndex]);
                                        gridView3.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //gridView3.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < gridView3.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (gridView3.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())  //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (gridView3.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())    //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    //if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)    // Commented By Shrikant S. on 08/06/2012
                                                    //{
                                                    //    gridView3.Columns[i].Group();
                                                    //}
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview start(TKT-3143)  //Added By Shrikant S. on 08/06/2012    //Start
                                                    if (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString() != string.Empty)
                                                    {

                                                        gridView3.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString();
                                                    }
                                                    else
                                                    {
                                                        gridView3.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString();
                                                    }
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview end(TKT-3143)    //Added By Shrikant S. on 08/06/2012    //End

                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        gridView3.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        gridView3.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        gridView3.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        gridView3.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        gridView3.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 08/06/2012  && Start
                                        for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                        {
                                            for (int i = 0; i < gridView3.Columns.Count; i++)
                                            {
                                                //if (gridView3.Columns[i].Caption.ToUpper().Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper().Trim())    //Commented By Shrikant S. on 04/02/2013
                                                string colName = (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString().Length > 0 ? ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString() : ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString());    // Added By Shrikant S. on 04/02/2013
                                                if (gridView3.Columns[i].Caption.ToUpper().Trim() == colName.ToUpper().Trim() && (bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == true)         // Added By Shrikant S. on 04/02/2013
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        gridView3.Columns[i].GroupIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["grouporder"]);
                                                    }
                                                    else
                                                    {
                                                        gridView3.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);//Add by Archana khade 05/04/12
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 08/06/2012  && End
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Card View")
                                    {
                                        cardView1.OptionsView.ShowCardCaption = true;
                                        cardView1.CardCaptionFormat = "{1}";
                                        cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
                                        cardView1.CardWidth = 600;
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), cardView1);
                                        cardView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < cardView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (cardView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())      //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (cardView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())        //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        cardView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        cardView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        cardView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        cardView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        cardView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        cardView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Bended View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), bandedGridView1);
                                        bandedGridView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        bandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //bandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < bandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (bandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())        //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (bandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())          //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        bandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        bandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        bandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Advanced Bended View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), advBandedGridView1);
                                        advBandedGridView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        advBandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        // advBandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (advBandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())     //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (advBandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())       //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        advBandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        advBandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        advBandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (LevelIndex == 3)
                                {
                                    #region To Display third chaild Level
                                    if (ReportDisplayType[LevelIndex].ToString() == "Grid View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), gridView4);
                                        gridView4.PopulateColumns(DS.Tables[LevelIndex]);
                                        gridView4.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //gridView4.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < gridView4.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (gridView4.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())      //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (gridView4.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())        //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    //if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)        //Commented By Shrikant S.on 08/06/2012
                                                    //{
                                                    //    gridView4.Columns[i].Group();
                                                    //}
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview start(TKT-3143)  //Added By Shrikant S. on 08/06/2012    //Start
                                                    if (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString() != string.Empty)
                                                    {

                                                        gridView4.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString();
                                                    }
                                                    else
                                                    {
                                                        gridView4.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString();
                                                    }
                                                    //Added by Archana Khade on 05/04/2012 for display the caption in gridview end(TKT-3143)    //Added By Shrikant S. on 08/06/2012    //End

                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        gridView4.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        gridView4.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        gridView4.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        gridView4.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        gridView4.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 08/06/2012  && Start
                                        for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                        {
                                            for (int i = 0; i < gridView4.Columns.Count; i++)
                                            {
                                                //if (gridView4.Columns[i].Caption.ToUpper().Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper().Trim())    // Commented By Shrikant S. on 04/02/2013
                                                string colName = (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString().Length > 0 ? ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column caption"].ToString() : ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString());    // Added By Shrikant S. on 04/02/2013
                                                if (gridView4.Columns[i].Caption.ToUpper().Trim() == colName.ToUpper().Trim() && (bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == true)         // Added By Shrikant S. on 04/02/2013
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        gridView4.Columns[i].GroupIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["grouporder"]);
                                                    }
                                                    else
                                                    {
                                                        gridView4.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);//Add by Archana khade 05/04/12
                                                    }
                                                }
                                            }
                                        }
                                        //Added by Archana Khade on 12/05/2012 start        && Added By Shrikant S. on 08/06/2012  && End
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Card View")
                                    {
                                        cardView1.OptionsView.ShowCardCaption = true;
                                        cardView1.CardCaptionFormat = "{1}";
                                        cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
                                        cardView1.CardWidth = 600;
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), cardView1);
                                        cardView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < cardView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (cardView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())      //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (cardView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())        //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        cardView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        cardView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        cardView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        cardView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        cardView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        cardView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Bended View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), bandedGridView1);
                                        bandedGridView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        bandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        // bandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < bandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (bandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())    //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (bandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())      //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        bandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        bandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        bandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        bandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ReportDisplayType[LevelIndex].ToString() == "Advanced Bended View")
                                    {
                                        gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), advBandedGridView1);
                                        advBandedGridView1.PopulateColumns(DS.Tables[LevelIndex]);
                                        advBandedGridView1.OptionsPrint.AutoWidth = false;//Added by Archana Khade on 05/04/2012 for TKT-3143 
                                        //advBandedGridView1.BestFitColumns();//Comment by Archana Khade 05/4/12 for TKT-3143 
                                        ColumnDetailsDS = new DataSet();
                                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[LevelIndex], ReportConString, "ColumnsList");

                                        for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
                                        {
                                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                                            {
                                                //if (advBandedGridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())     //Commented By Shrikant S. on 21/05/2013 for Bug-11169
                                                if (advBandedGridView1.Columns[i].Caption.ToUpper() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper())       //Added By Shrikant S. on 21/05/2013 for Bug-11169
                                                {
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Group();
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                                    }
                                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                                    {
                                                        advBandedGridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                                    {
                                                        advBandedGridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                                        advBandedGridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                                    }
                                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                                    {
                                                        advBandedGridView1.Columns[i].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            } //for loop end
                            #endregion

                        } //ReportType.ToLower() == "View".ToLower()
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("Please Pass The ReportID", VuMess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    //Application.Exit(); ///Commented by Archana K. on 29/03/13 for Bug-10253 
                    return; //Added by Archana K. on 29/03/13 for Bug-10253 
                }
                #endregion
                //Added by Archana K. on 19/03/14 for Bug-22080 start
                if (DS.Tables["layout_vw"] == null)
                {
                    DS.Tables.Add(dbOperations.GetReportLayOuts(ReportConnectionString, User, ReportID));
                }
               
                //Added by Archana K. on 19/03/14 for Bug-22080 end   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                //this.Dispose(true);
                //Application.Exit(); ///Commented by Archana K. on 29/03/13 for Bug-10253 
                return; //Added by Archana K. on 29/03/13 for Bug-10253 
            }
        }

        #endregion

        #region File Menu Items Code

        private void barButtonItem_Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show(this, "Do You Really Want To Exit??", VuMess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void barButtonItem_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.Print();
        }

        private void barButtonItem_PrintPrivew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.ShowPreview();
        }

        #endregion

        #region View Menu Items Code

        private void barButtonItem_HideIndicator_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.OptionsView.ShowIndicator)
            {
                this.gridView1.OptionsView.ShowIndicator = false;
                //this.barButtonItem_HideIndicator.ImageIndex = 139;
            }
            else
            {
                this.gridView1.OptionsView.ShowIndicator = true;
                //this.barButtonItem_HideIndicator.ImageIndex = 167;
            }

        }

        private void barButtonItem_GroupSummaryFooter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.GroupFooterShowMode == DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways)
            {
                this.gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
                //barButtonItem_GroupSummaryFooter.ImageIndex = 148;
            }
            else
            {
                this.gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                //barButtonItem_GroupSummaryFooter.ImageIndex = 163;
            }
            this.SetToolTip();  //Added By Shrikant S. on 25/06/2012 for Bug-4575
        }

        private void barCheckItem3_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_ShowIndicator.Checked)
            {
                this.gridView1.OptionsView.ShowIndicator = true;
            }
            else
            {
                this.gridView1.OptionsView.ShowIndicator = false;
            }
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
            //RestoreReportLayOut(string.Empty, 0);//Commented by Archana K. on 19/03/14 for Bug-22080
            RefreshLayout();//Added by Archana K. on 21/03/14 for Bug-22080
            RestoreReportLayOut(ReportLayoutnm, 0);//Changed by Archana K. on 19/03/14 for Bug-22080
        }
        private void RefreshLayout()
        {
            //Added by Archana K. on 21/03/13 for Bug-22080 start
            gridView1.ClearColumnsFilter();
            gridView1.ClearGrouping();
            gridView1.ClearSelection();
            gridView1.ClearSorting();
            gridView1.CloseEditor();
            gridView1.CollapseAllDetails();
            gridView1.CollapseAllGroups();
            gridView1.Columns.Clear();
            gridView1.DestroyCustomization();
            gridView1.GroupSummary.Clear();
            gridView1.OptionsView.ShowAutoFilterRow = false;
            BindGrid();
            //Added by Archana K. on 21/03/13 for Bug-22080 end
        }
        private void barButtonItem_SummaryFooter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.OptionsView.ShowFooter)
            {
                this.gridView1.OptionsView.ShowFooter = false;
                //barButtonItem_SummaryFooter.ImageIndex = 147;
            }
            else
            {
                this.gridView1.OptionsView.ShowFooter = true;
                //barButtonItem_SummaryFooter.ImageIndex = 174;
            }
            this.SetToolTip();          //Added By Shrikant S. on 25/06/2012 for Bug-4575
        }

        private void barCheckItem_ShowGroupPanal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_ShowGroupPanal.Checked)
            {
                gridView1.OptionsView.ShowGroupPanel = true;
            }
            else
            {
                gridView1.OptionsView.ShowGroupPanel = false;
            }

        }

        private void barCheckItem_SummaryFooter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_SummaryFooter.Checked)
            {
                gridView1.OptionsView.ShowFooter = true;
            }
            else
            {
                gridView1.OptionsView.ShowFooter = false;
            }
        }

        private void barCheckItem_GroupSummaryFooter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_GroupSummaryFooter.Checked)
            {
                this.gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            }
            else
            {
                this.gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            }
        }

        private void barCheckItem_ShowHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_ShowHeaders.Checked)
            {
                this.gridView1.OptionsView.ShowColumnHeaders = true;
            }
            else
            {
                this.gridView1.OptionsView.ShowColumnHeaders = false;
            }
        }

        private void barCheckItem_ShowVLines_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_ShowVLines.Checked)
            {
                this.gridView1.OptionsView.ShowVertLines = true;
            }
            else
            {
                this.gridView1.OptionsView.ShowVertLines = false;
            }
        }

        private void barCheckItem_ShowHLines_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_ShowHLines.Checked)
            {
                this.gridView1.OptionsView.ShowHorzLines = true;
            }
            else
            {
                this.gridView1.OptionsView.ShowHorzLines = false;
            }
        }

        private void barCheckItem_MultiSelectionRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_MultiSelectionRow.Checked)
            {
                this.gridView1.OptionsSelection.MultiSelect = true;
            }
            else
            {
                this.gridView1.OptionsSelection.MultiSelect = false;
            }
        }

        private void barButtonItem_FullExpandGroups_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItem_FullExpandGroups.Caption == "Full Expand Groups")
            {
                this.gridView1.ExpandAllGroups();
                barButtonItem_FullExpandGroups.Caption = "Full Collapse Groups";
            }
            else
            {
                this.gridView1.CollapseAllGroups();
                barButtonItem_FullExpandGroups.Caption = "Full Expand Groups";
            }
        }

        private void barCheckItem_ShowDetailToolTip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_ShowDetailToolTip.Checked)
            {
                gridControl1.ToolTipController = toolTipController1;
                gridControl1.ToolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(toolTipController1_GetActiveObjectInfo);

            }
            else
            {
                gridControl1.ToolTipController.GetActiveObjectInfo -= new ToolTipControllerGetActiveObjectInfoEventHandler(toolTipController1_GetActiveObjectInfo);
                gridControl1.ToolTipController = null;

            }
        }

        private void barCheckItem_IncrementalSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_IncrementalSearch.Checked)
            {
                gridView1.OptionsBehavior.AllowIncrementalSearch = true;
            }
            else
            {
                gridView1.OptionsBehavior.AllowIncrementalSearch = false;
            }
        }

        private void barCheckItem_AllowCellMerging_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_AllowCellMerging.Checked)
            {
                gridView1.OptionsView.AllowCellMerge = true;
            }
            else
            {
                gridView1.OptionsView.AllowCellMerge = false;
            }
        }

        private void barCheckItem_AutoHight_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem_AutoHight.Checked)
            {
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.OptionsView.RowAutoHeight = true;
            }
            else
            {
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.OptionsView.RowAutoHeight = false;
            }
        }

        private void barCheckItem_CommonGroupPanal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("need to Develop");
        }

        private void barCheckItem_ShowBands_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("need to develop");
        }

        private void barButtonItem_HideHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.OptionsView.ShowColumnHeaders == true)
            {
                this.gridView1.OptionsView.ShowColumnHeaders = false;
                //barButtonItem_HideHeaders.ImageIndex = 138;
            }
            else
            {
                this.gridView1.OptionsView.ShowColumnHeaders = true;
                //barButtonItem_HideHeaders.ImageIndex = 164;
            }
            this.SetToolTip();       //Added By Shrikant S. on 25/06/2012 for Bug-4575
        }

        private void barButtonItem_ShowBands_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("need to develop");
        }

        private void barButtonItem_VerticalLines_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.OptionsView.ShowVertLines == true)
            {
                this.gridView1.OptionsView.ShowVertLines = false;
                //barButtonItem_VerticalLines.ImageIndex = 132;
            }
            else
            {
                this.gridView1.OptionsView.ShowVertLines = true;
                //barButtonItem_VerticalLines.ImageIndex = 184;
            }
            this.SetToolTip();      //Added By Shrikant S. on 25/06/2012 for Bug-4575
        }

        private void barButtonItem_HorizontalLines_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.OptionsView.ShowHorzLines == true)
            {
                this.gridView1.OptionsView.ShowHorzLines = false;
                //barButtonItem_HorizontalLines.ImageIndex = 137;
            }
            else
            {
                this.gridView1.OptionsView.ShowHorzLines = true;
                //barButtonItem_HorizontalLines.ImageIndex = 163;
            }
            this.SetToolTip();      //Added By Shrikant S. on 25/06/2012 for Bug-4575
        }

        private void barButtonItem_MultiSelectionLines_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.OptionsSelection.MultiSelect == true)
            {
                this.gridView1.OptionsSelection.MultiSelect = false;
                //barButtonItem_MultiSelectionLines.ImageIndex = 140;
            }
            else
            {
                this.gridView1.OptionsSelection.MultiSelect = true;
                //barButtonItem_MultiSelectionLines.ImageIndex = 169;
            }
        }

        private void barButtonItem_FullExpandsGroups_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItem_FullExpandsGroups.Caption == "Full Expands Groups")
            {
                this.gridView1.ExpandAllGroups();
                barButtonItem_FullExpandsGroups.Caption = "Full Collapse Groups";
                //barButtonItem_FullExpandsGroups.ImageIndex = 136;

            }
            else
            {
                this.gridView1.CollapseAllGroups();
                barButtonItem_FullExpandsGroups.Caption = "Full Expands Groups";
                //barButtonItem_FullExpandsGroups.ImageIndex = 160;
            }
        }

        private void barButtonItem_BestFit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItem_BestFit.ImageIndex == 133)
            {

                //barButtonItem_BestFit.ImageIndex = 156;                
                gridView1.BestFitColumns();
            }
            else
            {
                gridView1.BestFitColumns();
                //barButtonItem_BestFit.ImageIndex = 133;
            }


        }

        private void barButtonItem_Summary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (barButtonItem_Summary.ImageIndex == 146)
            {
                if (dockPanel_Summary.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
                {
                    dockPanel_Summary.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                    dockPanel_Summary.Show();
                    //barButtonItem_Summary.ImageIndex = 146;
                }
            }
            else
            {
                if (dockPanel_Summary.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
                {
                    dockPanel_Summary.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                    dockPanel_Summary.Show();
                    //barButtonItem_Summary.ImageIndex = 146;
                }
            }

        }

        private void barButtonItem_Comments_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItem_Summary.ImageIndex == 119)
            {
                if (dockPanel_Comments.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
                {
                    dockPanel_Comments.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                    dockPanel_Comments.Show();
                    //barButtonItem_Comments.ImageIndex = 119;
                }
            }
            else
            {
                if (dockPanel_Comments.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
                {
                    dockPanel_Comments.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                    dockPanel_Comments.Show();
                    //barButtonItem_Comments.ImageIndex = 119;
                }
            }

        }

        private void barButtonItem_RuntimeCustomization_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dockPanel_RunTimeCustomizetion.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            {
                dockPanel_RunTimeCustomizetion.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //barButtonItem_RuntimeCustomization.ImageIndex = 143;
            }
            else
            {
                dockPanel_RunTimeCustomizetion.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                //barButtonItem_RuntimeCustomization.ImageIndex = 186;
                barButtonItem_RuntimeCustomization.Caption = "";
            }
        }

        private void barButtonItem_SaveLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //MessageBox.Show("need to develop");
            //SaveLayOut();//commented by Archana K. on 14/03/14 for Bug-22080
            flag = true;
            SaveLayOutNew();//Added by Archana K. on 14/03/14 for Bug-22080
        }
        
        private void RestoreReportLayOut(string LayOutName, int type)         // Added By Shrikant S. on 08/06/2012   
        {
            LayoutView();//Added by Archana K. on 18/03/14 for Bug-22080
            if (LayOutName.Length == 0)
            {
                if (DS.Tables["layout_vw"] == null)//Added by Archana K. on 19/03/14 for Bug-22080 
                {
                    DS.Tables.Add(dbOperations.GetReportLayOuts(ReportConnectionString, User, ReportID));
                }
            }
            if (DS.Tables["layout_vw"] != null)
            {
                if (DS.Tables["layout_vw"].Rows.Count > 0)
                {
                    string filter = (LayOutName.Length != 0 ? "LayOutName='" + LayOutName.Trim() + "'" : "IsDefault=true");
                    DataRow[] rows = DS.Tables["layout_vw"].Select(filter);

                    if (rows.Length > 0)
                    {
                        //Added by Archana K. on 21/03/14 for Bug-22080 Start
                        LayOutName = rows[0]["LayOutName"].ToString().Trim();
                        ReportLayoutnm = LayOutName;
                        IsDefault = Convert.ToBoolean(rows[0]["IsDefault"]);
                        ReportLvlId = Convert.ToInt32(rows[0]["LayOutId"]);
                        //Added by Archana K. on 21/03/14 for Bug-22080 End
                        if (type == 0)
                        {
                            for (int i = 0; i < gridControl1.ViewCollection.Count; i++)
                            {
                                XmlDocument xmlDocument = new XmlDocument();
                                int level = i;
                                //Commented by Archana K. on 18/03/14 for Bug-22080 start
                                //switch (i)
                                //{
                                //    case 0:
                                //        level = 1;
                                //        break;
                                //    case 1:
                                //        level = 0;
                                //        break;
                                //}
                                //Commented by Archana K. on 18/03/14 for Bug-22080 end
                                byte[] myArray = (System.Byte[])rows[0]["Level" + (level + 1).ToString() + "_Layout"];
                                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                                string myString = enc.GetString(myArray);
                                xmlDocument.LoadXml(myString);
                                string fileName = Path.Combine(Path.GetTempPath(), User + "_" +"Level" + (level + 1).ToString() + "_Layout"+"_"+ DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")) + ".xml";
                                xmlDocument.Save(fileName);
                                gridControl1.ViewCollection[i].RestoreLayoutFromXml(fileName);
                                File.Delete(fileName);
                            }
                        }
                        else
                        {
                            XmlDocument xmlDocument = new XmlDocument();
                            byte[] myArray = (System.Byte[])rows[0]["Level" + (type).ToString() + "_Layout"];
                            System.Text.Encoding enc = System.Text.Encoding.ASCII;
                            string myString = enc.GetString(myArray);
                            xmlDocument.LoadXml(myString);
                            string fileName = Path.Combine(Path.GetTempPath(), User + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")) + ".xml";
                            xmlDocument.Save(fileName);
                            gridControl1.ViewCollection[1].RestoreLayoutFromXml(fileName);
                            File.Delete(fileName);
                        }
                        this.SetSkin(Convert.ToString(rows[0]["SkinName"]));
                       
                    }
                 
                }
                //Added by Archana K. on 20/03/14 for Bug-22080 start
                if (LayOutName == "")
                    LayOutName = "Default";
                barStaticItem1.Caption = "Current Layout :" + LayOutName.Trim();
                //Added by Archana K. on 20/03/14 for Bug-22080 end
            }
           
            SetToolTip();
           
        }
        private void SetToolTip()   // Added By Shrikant S. on 25/06/2012           //Start
        {
            if (this.gridView1.OptionsView.ShowFooter)
            {
                DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
                toolTipItem1.Text = "Hide Summary Footer";
                superToolTip1.Items.Add(toolTipItem1);
                this.barButtonItem_SummaryFooter.SuperTip = superToolTip1;
            }
            else
            {
                DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();  // Added By Shrikant S. on 25/06/2012
                DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
                toolTipItem1.Text = "Show Summary Footer";
                superToolTip1.Items.Add(toolTipItem1);
                this.barButtonItem_SummaryFooter.SuperTip = superToolTip1;
            }
            if (this.gridView1.OptionsView.ShowColumnHeaders)
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Hide Headers";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_HideHeaders.SuperTip = superToolTip3;
            }
            else
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Show Headers";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_HideHeaders.SuperTip = superToolTip3;
            }
            if (this.gridView1.GroupFooterShowMode == DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways)
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Hide Group Summary Footer";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_GroupSummaryFooter.SuperTip = superToolTip3;
            }
            else
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Show Group Summary Footer";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_GroupSummaryFooter.SuperTip = superToolTip3;
            }

            if (this.gridView1.OptionsView.ShowVertLines)
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Hide Vertical Lines";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_VerticalLines.SuperTip = superToolTip3;
            }
            else
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Show Vertical Lines";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_VerticalLines.SuperTip = superToolTip3;
            }

            if (this.gridView1.OptionsView.ShowHorzLines)
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Hide Horizontal Lines";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_HorizontalLines.SuperTip = superToolTip3;
            }
            else
            {
                DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
                DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
                toolTipItem3.Text = "Show Horizontal Lines";
                superToolTip3.Items.Add(toolTipItem3);
                this.barButtonItem_HorizontalLines.SuperTip = superToolTip3;
            }
        }                           // Added By Shrikant S. on 25/06/2012           //End

        private void SaveLayOut()                           // Added By Shrikant S. on 08/06/2012   
        {
            string sqlStr = string.Empty;
            sqlStr = sqlStr + " " + "If Not Exists (Select top 1 layoutName From ReportLayout Where UserName=@UserName and RepId=@RepId )";
            sqlStr = sqlStr + " " + "Begin \n";
            sqlStr = sqlStr + " " + "Insert Into ReportLayOut (UserName,RepId,LayoutName,Level1_Layout,Level2_Layout,Level3_Layout,Level4_Layout,Level5_Layout,Level6_Layout,Level7_Layout,IsDefault,SkinName) ";
            sqlStr = sqlStr + " " + "Values (@UserName,@RepId,@LayoutName,convert(varbinary(max),@Level2_Layout),Convert(varbinary(max),@Level1_Layout)";
            sqlStr = sqlStr + " " + ",convert(varbinary(max),@Level3_Layout),convert(varbinary(max),@Level4_Layout),convert(varbinary(max),@Level5_Layout)";
            sqlStr = sqlStr + " " + ",convert(varbinary(max),@Level6_Layout),convert(varbinary(max),@Level7_Layout),@IsDefault,@SkinName) ";
            sqlStr = sqlStr + " " + "End \nElse \nBegin \n";
            sqlStr = sqlStr + " " + "Update ReportLayOut set Level1_Layout=convert(varbinary(max),@Level2_Layout),Level2_Layout=Convert(varbinary(max),@Level1_Layout)";
            sqlStr = sqlStr + " " + ",Level3_Layout=convert(varbinary(max),@Level3_Layout),Level4_Layout=convert(varbinary(max),@Level4_Layout)";
            sqlStr = sqlStr + " " + ",Level5_Layout=convert(varbinary(max),@Level5_Layout),Level6_Layout=convert(varbinary(max),@Level6_Layout)";
            sqlStr = sqlStr + " " + ",Level7_Layout=convert(varbinary(max),@Level7_Layout),IsDefault=@IsDefault,SkinName=@SkinName";
            sqlStr = sqlStr + " " + "Where UserName=@UserName and RepId=@RepId and LayOutName=@LayoutName ";
            sqlStr = sqlStr + " " + "End ";
            SqlConnection conn = new SqlConnection(ReportConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand(sqlStr, conn);
            command.CommandType = CommandType.Text;

            command.Parameters.Add("@UserName", SqlDbType.VarChar);
            command.Parameters["@UserName"].Value = User;

            command.Parameters.Add("@RepId", SqlDbType.VarChar);
            command.Parameters["@RepId"].Value = ReportID;

            command.Parameters.Add("@LayoutName", SqlDbType.VarChar);
            //command.Parameters["@LayoutName"].Value = "Layout1";
            //  command.Parameters["@LayoutName"].Value = frmReportLayout.LayoutName;

            command.Parameters.Add("@IsDefault", SqlDbType.Bit);
            command.Parameters["@IsDefault"].Value = true;

            command.Parameters.Add("@SkinName", SqlDbType.VarChar);
            command.Parameters["@SkinName"].Value = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName.ToString();

            for (int i = 0; i < gridControl1.ViewCollection.Count; i++)
            {
                string viewName = gridControl1.ViewCollection[i].Name.ToString().Trim();
                string fileName = Path.Combine(Path.GetTempPath(), User + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")) + ".xml";
                gridControl1.ViewCollection[i].SaveLayoutToXml(fileName);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                command.Parameters.Add("@Level" + (i + 1).ToString() + "_Layout", SqlDbType.VarChar);
                command.Parameters["@Level" + (i + 1).ToString() + "_Layout"].Value = xmlDocument.OuterXml.ToString();
                File.Delete(fileName);
            }
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Layout saved...",VuMess,MessageBoxButtons.OK,MessageBoxIcon.None);
        }
        private void LayoutView()
        {
            //Added by Archana K. on 18/03/14 for Bug-22080 
            if (comboBoxEdit_SelectLevel.SelectedItem.ToString() == "All")
            {
                //barButtonItem19_ItemClick(null, null);//Commented by Archana K. on 21/03/14 for Bug-22080
                RefreshLayout();//Added by Archana K. on 21/03/14 for Bug-22080
                //RestoreReportLayOut(string.Empty,0);      // Added By Shrikant S. on 08/06/2012
               
            }
            else if (comboBoxEdit_SelectLevel.SelectedItem.ToString() != "All")
            {
                DataTable LevelsDS = new DataTable();
                LevelsDS = DS.Tables[comboBoxEdit_SelectLevel.SelectedItem.ToString()];
                LevelsDS.Constraints.Clear();
                LevelsDS.ChildRelations.Clear();
                LevelsDS.AcceptChanges();
                string reportType = string.Empty;
                foreach (DataRow dRow in LocalDS.Tables["TblReportLevelsAndTypes"].Rows)
                {
                    if (dRow["ReportDisplayName"].ToString() == comboBoxEdit_SelectLevel.SelectedItem.ToString())
                    {
                        reportType = dRow["ReportDisplayType"].ToString();
                    }
                }
                gridControl1.DataSource = LevelsDS;
                gridControl1.ForceInitialize();
                //gridView1.BestFitColumns();           //Commented By Shrikant S. on 08/06/2012 

                switch (reportType)
                {
                    case "Grid View":
                        gridControl1.MainView = this.gridView1;
                        gridView1.PopulateColumns();
                        //gridView1.BestFitColumns();       //Commented By Shrikant S. on 08/06/2012 

                        //Added By Shrikant S. on 08/06/2012    //Start
                        gridView1.OptionsPrint.AutoWidth = false;
                        ColumnDetailsDS = new DataSet();
                        ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[comboBoxEdit_SelectLevel.SelectedIndex - 1], ReportConString, "ColumnsList");

                        for (int i = 0; i < gridView1.Columns.Count; i++)
                        {
                            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                            {
                                if (gridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())
                                {
                                    if (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString() != string.Empty)
                                    {

                                        gridView1.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString();
                                    }
                                    else
                                    {
                                        gridView1.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString();
                                    }

                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
                                    {
                                        gridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                    }
                                    if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
                                    {
                                        gridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
                                    }
                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
                                    {
                                        gridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
                                        gridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    }
                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
                                    {
                                        gridView1.Columns[i].Visible = false;
                                    }
                                }
                            }
                        }
                        for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
                        {
                            for (int i = 0; i < gridView1.Columns.Count; i++)
                            {
                                if (gridView1.Columns[i].Caption.ToUpper().Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper().Trim())
                                {
                                    if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
                                    {
                                        gridView1.Columns[i].GroupIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["grouporder"]);
                                    }
                                    else
                                    {
                                        gridView1.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);
                                    }
                                }
                            }
                        }
                        //Added By Shrikant S. on 08/06/2012    //End
                        break;
                    case "Card View":
                        cardView1.OptionsView.ShowCardCaption = true;
                        cardView1.CardCaptionFormat = "{1}";
                        cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
                        cardView1.CardWidth = 600;
                        gridControl1.MainView = this.cardView1;
                        cardView1.PopulateColumns();
                        break;
                    case "Bended View":
                        gridControl1.MainView = this.bandedGridView1;
                        bandedGridView1.PopulateColumns();
                        bandedGridView1.BestFitColumns();
                        break;
                    case "Advanced Bended View":
                        gridControl1.MainView = this.advBandedGridView1;
                        advBandedGridView1.PopulateColumns();
                        advBandedGridView1.BestFitColumns();
                        break;
                    default:
                        break;
                }
            }
        }
        private void SaveLayOutNew()                           // Added By Shrikant S. on 08/06/2012   
        {
           // frmLayOut frmReportLayout = new frmLayOut(ReportConnectionString, User, ReportID);//Commented by Archana K. on 15/03/14 for Bug-22080
            frmReportLayout = new frmLayOut(ReportConnectionString, User, ReportID, flag);//Changed by Archana K. on 15/03/14 for Bug-22080
          //  frmReportLayout.MdiParent = this;//Commented by Archana K. on 13/03/14 for Bug-22080
            //frmReportLayout.Show();//Commented by Archana K. on 13/03/14 for Bug-22080
            frmReportLayout.LayoutName=ReportLayoutnm  ;//Added by Archana K. on 18/03/14 for Bug-22080
            frmReportLayout.IsDefault= IsDefault;//Added by Archana K. on 21/03/14 for Bug-22080
            frmReportLayout.ShowDialog();//Changed by Archana K. on 13/03/14 for Bug-22080
            string sqlStr = string.Empty;//Added by Archana K. on 18/03/14 for Bug-22080
            string msg=string.Empty;//Added by Archana K. on 20/03/14 for Bug-22080
            ReportLayoutnm = frmReportLayout.LayoutName;//Added by Archana K. on 18/03/14 for Bug-22080
            IsDefault = Convert.ToBoolean(frmReportLayout.IsDefault);//Added by Archana K. on 21/03/14 for Bug-22080
            ReportLvlId = frmReportLayout.LayOutId;
            if (frmReportLayout.LayoutName.Length != 0)
                {
                    if (flag == true)//Added by Archana K. on 14/03/14 for Bug-22080
                    {

                       // string sqlStr = string.Empty;//Commented by Archana K. on 18/03/14 for Bug-22080
                        switch (frmReportLayout.Status)
                        {
                            case 0:             // Insert Layout
                                if (DS.Tables["layout_vw"].Rows.Count <= 5)//Added by Archana K. on 19/03/14 for Bug-22080 
                                {
                                    sqlStr = "Insert Into ReportLayOut (UserName,RepId,LayoutName,Level1_Layout,Level2_Layout,Level3_Layout,Level4_Layout,Level5_Layout,Level6_Layout,Level7_Layout,IsDefault,RepLvlID) ";
                                    sqlStr = sqlStr + " " + "Values (@UserName,@RepId,@LayoutName,convert(varbinary(max),@Level1_Layout),Convert(varbinary(max),@Level2_Layout)";
                                    sqlStr = sqlStr + " " + ",convert(varbinary(max),@Level3_Layout),convert(varbinary(max),@Level4_Layout),convert(varbinary(max),@Level5_Layout)";
                                    sqlStr = sqlStr + " " + ",convert(varbinary(max),@Level6_Layout),convert(varbinary(max),@Level7_Layout),@IsDefault";
                                    sqlStr = sqlStr + " " + ",@RepLvlID)";//Added by Archana K. on 20/03/14 for Bug-22080
                                    msg = "Layout Save Successfully";//Added by Archana K. on 20/03/14 for Bug-22080
                                }
                                //Added by Archana K. on 19/03/14 for Bug-22080 start
                                else
                                {
                                    MessageBox.Show("Maximum Five Layout Allowed", "Layout saved...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                //Added by Archana K. on 19/03/14 for Bug-22080 end
                                break;
                            case 1:             // Update Layout
                                sqlStr = "Update ReportLayOut set Level1_Layout=convert(varbinary(max),@Level1_Layout),Level2_Layout=Convert(varbinary(max),@Level2_Layout)";
                                sqlStr = sqlStr + " " + ",Level3_Layout=convert(varbinary(max),@Level3_Layout),Level4_Layout=convert(varbinary(max),@Level4_Layout)";
                                sqlStr = sqlStr + " " + ",Level5_Layout=convert(varbinary(max),@Level5_Layout),Level6_Layout=convert(varbinary(max),@Level6_Layout)";
                                sqlStr = sqlStr + " " + ",Level7_Layout=convert(varbinary(max),@Level7_Layout),IsDefault=@IsDefault";
                                sqlStr = sqlStr + " " + ",LayOutName=@LayoutName,RepLvlID=@RepLvlID "; //Added by Archana K. on 13/03/14 for Bug-22080
                                //sqlStr = sqlStr + " " + "Where UserName=@UserName and RepId=@RepId and LayOutName=@LayoutName";//Commented by Archana K. on 13/03/14 for Bug-22080
                                sqlStr = sqlStr + " " + "Where UserName=@UserName and RepId=@RepId and LayOutId=@LayOutId";//Changed by Archana K. on 13/03/14 for Bug-22080
                                msg = "Layout Updated Successfully.";//Added by Archana K. on 20/03/14 for Bug-22080
                                break;
                           //Added by Archana K. on 20/03/14 for Bug-22080 start 
                            case 2: //Delete Layout
                                if (MessageBox.Show("Are you sure you wish to delete this Record ?", "Layout saved...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    sqlStr = "Delete from ReportLayOut";
                                    sqlStr = sqlStr + " " + "Where UserName=@UserName and RepId=@RepId and LayOutName=@LayoutName";
                                    msg="Layout Deleted Successfully.";
                                }
                                
                                break;
                            default:
                                return ;

                          //Added by Archana K. on 20/03/14 for Bug-22080 end
                        }
                        if (frmReportLayout.IsDefault == true)
                        {
                            sqlStr = sqlStr + " Update ReportLayOut set Isdefault=0 Where UserName=@UserName and RepId=@RepId and LayOutName<>@LayoutName";
                        }
                   
                    SqlConnection conn = new SqlConnection(ReportConnectionString);
                    conn.Open();
                    SqlCommand command = new SqlCommand(sqlStr, conn);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@UserName", SqlDbType.VarChar);
                    command.Parameters["@UserName"].Value = User;

                    command.Parameters.Add("@RepId", SqlDbType.VarChar);
                    command.Parameters["@RepId"].Value = ReportID;

                    command.Parameters.Add("@LayoutName", SqlDbType.VarChar);
                    command.Parameters["@LayoutName"].Value = frmReportLayout.LayoutName;

                    command.Parameters.Add("@IsDefault", SqlDbType.Bit);
                    command.Parameters["@IsDefault"].Value = frmReportLayout.IsDefault;


                    //Added by Archana K. on 20/03/14 for Bug-22080 start
                    command.Parameters.Add("@RepLvlID", SqlDbType.BigInt);
                    command.Parameters["@RepLvlID"].Value = comboBoxEdit_SelectLevel.SelectedIndex;

                    command.Parameters.Add("@LayOutId", SqlDbType.Int);
                    command.Parameters["@LayOutId"].Value = ReportLvlId;
                    //Added by Archana K. on 20/03/14 for Bug-22080 end

                    for (int i = 0; i < gridControl1.ViewCollection.Count; i++)
                    {
                        string viewName = gridControl1.ViewCollection[i].Name.ToString().Trim();
                        string fileName = Path.Combine(Path.GetTempPath(), User + "Level" + (i + 1).ToString() + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")) + ".xml";
                        gridControl1.ViewCollection[i].SaveLayoutToXml(fileName);
                        //gridControl1.ViewCollection[i].SaveLayoutToXml(fileName, DevExpress.Utils.OptionsLayoutGrid.FullLayout);//Added & Commented by Archana K. on 18/03/14 for Bug-22080
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(fileName);
                        command.Parameters.Add("@Level" + (i + 1).ToString() + "_Layout", SqlDbType.VarChar);
                        command.Parameters["@Level" + (i + 1).ToString() + "_Layout"].Value = xmlDocument.OuterXml.ToString();
                    }
                    command.ExecuteNonQuery();
                    // Added by Archana K. on 19/03/14 For Bug-22080 start
                    MessageBox.Show(msg, "Layout saved...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    msg = "";
                    DS.Tables["layout_vw"].AcceptChanges();
                    // Added by Archana K. on 19/03/14 For Bug-22080 end
                    conn.Close();
                }
                // Added by Archana K. on 19/03/14 For Bug-22080 start
                else
                {
                    if (frmReportLayout.IsView == true)
                    {
                        RestoreReportLayOut(ReportLayoutnm, comboBoxEdit_SelectLevel.SelectedIndex);
                    }
                }
                // Added by Archana K. on 19/03/14 For Bug-22080 end           
            }
           
        }
        #endregion

        #region To Display Tool Tips for Grid

        private void toolTipController1_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl1) return;
            ToolTipControlInfo info = null;
            try
            {
                GridView view = gridControl1.GetViewAt(e.ControlMousePosition) as GridView;
                if (view == null) return;
                GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
                if (hi.InRowCell)
                {
                    info = new ToolTipControlInfo(new CellToolTipInfo(hi.RowHandle, hi.Column, "cell"), controlsLogic.GetCellHintText(view, hi.RowHandle, hi.Column));
                    return;
                }
                if (hi.Column != null)
                {
                    //info = new ToolTipControlInfo(hi.Column, GetColumnHintText(hi.Column));
                    return;
                }
                if (hi.HitTest == GridHitTest.GroupPanel)
                {
                    info = new ToolTipControlInfo(hi.HitTest, "Group panel");
                    return;
                }

                if (hi.HitTest == GridHitTest.RowIndicator)
                {
                    info = new ToolTipControlInfo(GridHitTest.RowIndicator.ToString() + hi.RowHandle.ToString(), "Row Handle: " + hi.RowHandle.ToString());
                    return;
                }
            }
            finally
            {
                e.Info = info;
            }
        }

        private string GetColumnHintText(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            string ret = string.Empty;
            try
            {
                ret = ColumnHints[gridView1.Columns.IndexOf(column)];
                if (ret == "") ret = column.Caption;
                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Inner Grid Exception : " + ex.Message, VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return ret = "";
            }
            //return ret;
        }

        #endregion

        #region Options Menu Items Code

        #region Total Report Export
        private void barButtonItem_ReportExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("XML Document", "XML Documents|*.xml");
            if (fileName != "")
            {
                ExportTo(new ExportXmlProvider(fileName));
                OpenFile(fileName);
            }
        }
        #endregion

        #region Send Mails With Attachment
        /// <summary>
        /// Sending mail with Excel File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Mail_Excel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
            if (fileName != "")
            {
                ExportTo(new ExportXlsProvider(fileName));
                DialogResult DR = MessageBox.Show("You Want To Send A Mail ??", "Information - Send Exel File", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (DR == DialogResult.Yes)
                {
                    try
                    {
                        SendMail sendmail = new SendMail(fileName, attachfilename, this.Text, User);
                        sendmail.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mail Sending Files, please send once again \r\n the Error Is: " + ex.Message, VuMess, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
                {
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                }
            }
        }
        /// <summary>
        /// sending mail with XML file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Mail_XML_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("XML Document", "XML Documents|*.xml");
            if (fileName != "")
            {
                ExportTo(new ExportXmlProvider(fileName));
                DialogResult DR = MessageBox.Show("You Want To Send A Mail ??", "Information - Send XML File", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (DR == DialogResult.Yes)
                {
                    try
                    {
                        SendMail sendmail = new SendMail(fileName, attachfilename, this.Text, User);
                        sendmail.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mail Sending Files, please send once again \r\n the Error Is: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
                {
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                }
            }
        }
        /// <summary>
        /// sending mail with html file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Mail_HTML_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("HTML Document", "HTML Documents|*.html");
            if (fileName != "")
            {
                ExportTo(new ExportHtmlProvider(fileName));
                DialogResult DR = MessageBox.Show("You Want To Send A Mail ??", "Information - Send XML File", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (DR == DialogResult.Yes)
                {
                    try
                    {
                        SendMail sendmail = new SendMail(fileName, attachfilename, this.Text, User);
                        sendmail.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mail Sending Files, please send once again \r\n the Error Is: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
                {
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                }
            }
        }
        /// <summary>
        /// sending mail with Contents of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Mail_Contents_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("need to develop");
        }
        /// <summary>
        /// sending mail with text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Mail_Text_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("Text Document", "Text Files|*.txt");
            if (fileName != "")
            {
                ExportTo(new ExportTxtProvider(fileName));
                DialogResult DR = MessageBox.Show("You Want To Send A Mail ??", "Information - Send XML File", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (DR == DialogResult.Yes)
                {
                    try
                    {
                        SendMail sendmail = new SendMail(fileName, attachfilename, this.Text, User);
                        sendmail.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mail Sending Files, please send once again \r\n the Error Is: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
                {
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                }
            }
        }
        #endregion

        #region Export Option

        /// <summary>
        /// Exporting with html file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Export_HTML_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("HTML Document", "HTML Documents|*.html");
            if (fileName != "")
            {
                ExportTo(new ExportHtmlProvider(fileName));
                OpenFile(fileName);

            }
        }
        /// <summary>
        /// Exporting with Excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Export_Excel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
            if (fileName != "")
            {

                //ExportTo(new ExportXlsProvider(fileName));//Commented by Archana on 13/03/14 
                gridControl1.ExportToXls(fileName);//Changed by Archana on 13/03/14 
                OpenFile(fileName);
            }
        }
        /// <summary>
        /// Exporting with XML file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Export_XML_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("XML Document", "XML Documents|*.xml");



            if (fileName != "")
            {
                ExportTo(new ExportXmlProvider(fileName));
                OpenFile(fileName);
            }
        }
        /// <summary>
        /// Exporting with text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Export_Text_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("Text Document", "Text Files|*.txt");
            if (fileName != "")
            {
                ExportTo(new ExportTxtProvider(fileName));
                OpenFile(fileName);
            }
        }

        //Added By Pankaj B. on 22-04-2015 for Bug-25657 start
        private void barButtonItem_Export_PDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fileName = ShowSaveFileDialog("PDF", "PDF|*.pdf");
            if (fileName != "")
            {
                gridControl1.ExportToPdf(fileName);
                OpenFile(fileName);
            }
        }
        //Added By Pankaj B. on 22-04-2015 for Bug-25657 End

        #endregion

        #endregion

        #region Private Methods

        #region For Export Reports Methods
        private void ExportTo(IExportProvider provider)
        {
            BaseExportLink link = gridView1.CreateExportLink(provider);
            (link as GridViewExportLink).ExpandAll = false;
            link.ExportTo(true);
            provider.Dispose();
        }

        private void OpenFile(string fileName)
        {
            if (MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    MessageBox.Show(this, "Cannot find an application on your system suitable for openning the file with exported data.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = this.Text;// Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(dlg.FileName);
                string strOldFileName = fi.Name;
                string strNewFileName = string.Empty;
                strNewFileName = commonInfo.replaceSpecialChars(strOldFileName, strNewFileName);
                if (strNewFileName != string.Empty)
                    dlg.FileName = dlg.FileName.Replace(strOldFileName, strNewFileName);
                else
                    dlg.FileName = dlg.FileName;

                fi = new FileInfo(dlg.FileName);
                attachfilename = fi.Name;
                return dlg.FileName;
            }
            return "";
        }

        #endregion

        #region Binding the Skins availbabe in dev express controls

        private void comboBoxEdit1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBoxEdit cb = sender as ComboBoxEdit;
            gridControl1.LookAndFeel.SkinName = cb.EditValue.ToString();
        }

        #endregion
        #endregion

        #region Runtime Customizetions Code

        private void bindSelectLevelCombobox()
        {
            //for (int i = 0; i < DS.Tables.Count; i++)//Commented by Archana K. on 20/03/14 for Bug-22080
            for (int i = 0; i < DS.Tables.Count-1; i++)//Changed by Archana K. on 20/03/14 for Bug-22080
            {
                comboBoxEdit_SelectLevel.Properties.Items.Add(DS.Tables[i].TableName);
            }
            comboBoxEdit_SelectLevel.Properties.Items.Insert(0, "All");
            comboBoxEdit_SelectLevel.SelectedIndex = 0;
        }
        private void bindSelectLayoutCombo()        // Added by Shrikant S. on 08/06/2012
        {
            if (DS.Tables["Layout_vw"].Rows.Count > 0)
            {
                for (int i = 0; i < DS.Tables["Layout_vw"].Rows.Count; i++)
                {
                    cboReportLayout.Properties.Items.Add(DS.Tables["Layout_vw"].Rows[i]["LayoutName"].ToString());
                }
                cboReportLayout.SelectedIndex = 0;
            }
        }

        private void bindSelectLevelCombobox_StyleConditions()
        {
            string reportType = string.Empty;
            for (int i = 0; i < DS.Tables.Count-1; i++)
            {
                reportType = LocalDS.Tables["TblReportLevelsAndTypes"].Rows[i]["ReportDisplayType"].ToString();

                switch (reportType)
                {
                    case "Grid View":
                        cbeSelectLevel.Properties.Items.Add(DS.Tables[i].TableName);
                        break;
                    case "Card View":
                        break;
                    case "Bended View":
                        cbeSelectLevel.Properties.Items.Add(DS.Tables[i].TableName);
                        break;
                    case "Advanced Bended View":
                        cbeSelectLevel.Properties.Items.Add(DS.Tables[i].TableName);
                        break;
                    default:
                        break;
                }
            }
            cbeSelectLevel.SelectedIndex = 0;
        }

        private void simpleButton_Apply_Click(object sender, EventArgs e)
        {
            //Commented by Archana K. on 18/03/14 for Bug-22080 start
            //if (comboBoxEdit_SelectLevel.SelectedItem.ToString() == "All")
            //{
            //    barButtonItem19_ItemClick(null, null);
            //    //RestoreReportLayOut(string.Empty);      // Added By Shrikant S. on 08/06/2012
            //}
            //else if (comboBoxEdit_SelectLevel.SelectedItem.ToString() != "All")
            //{
            //    DataTable LevelsDS = new DataTable();
            //    LevelsDS = DS.Tables[comboBoxEdit_SelectLevel.SelectedItem.ToString()];
            //    LevelsDS.Constraints.Clear();
            //    LevelsDS.ChildRelations.Clear();
            //    LevelsDS.AcceptChanges();
            //    string reportType = string.Empty;
            //    foreach (DataRow dRow in LocalDS.Tables["TblReportLevelsAndTypes"].Rows)
            //    {
            //        if (dRow["ReportDisplayName"].ToString() == comboBoxEdit_SelectLevel.SelectedItem.ToString())
            //        {
            //            reportType = dRow["ReportDisplayType"].ToString();
            //        }
            //    }
            //    gridControl1.DataSource = LevelsDS;
            //    gridControl1.ForceInitialize();
            //    //gridView1.BestFitColumns();           //Commented By Shrikant S. on 08/06/2012 

            //    switch (reportType)
            //    {
            //        case "Grid View":
            //            gridControl1.MainView = this.gridView1;
            //            gridView1.PopulateColumns();
            //            //gridView1.BestFitColumns();       //Commented By Shrikant S. on 08/06/2012 

            //            //Added By Shrikant S. on 08/06/2012    //Start
            //            gridView1.OptionsPrint.AutoWidth = false;
            //            ColumnDetailsDS = new DataSet();
            //            ColumnDetailsDS = dbOperations.GetColumnsDetails(ReportID, ReportQuaryID[comboBoxEdit_SelectLevel.SelectedIndex - 1], ReportConString, "ColumnsList");

            //            for (int i = 0; i < gridView1.Columns.Count; i++)
            //            {
            //                for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
            //                {
            //                    if (gridView1.Columns[i].Caption == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString())
            //                    {
            //                        if (ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString() != string.Empty)
            //                        {

            //                            gridView1.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Caption"].ToString();
            //                        }
            //                        else
            //                        {
            //                            gridView1.Columns[i].Caption = ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString();
            //                        }

            //                        if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Freezing"] == true)
            //                        {
            //                            gridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            //                        }
            //                        if (Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]) > 0)
            //                        {
            //                            gridView1.Columns[i].Width = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Width"]);
            //                        }
            //                        if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Summury"] == true)
            //                        {
            //                            gridView1.Columns[i].SummaryItem.DisplayFormat = "SUM={0}";
            //                            gridView1.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //                        }
            //                        if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Displayed"] == false)
            //                        {
            //                            gridView1.Columns[i].Visible = false;
            //                        }
            //                    }
            //                }
            //            }
            //            for (int j = 0; j < ColumnDetailsDS.Tables["ColumnsList"].Rows.Count; j++)
            //            {
            //                for (int i = 0; i < gridView1.Columns.Count; i++)
            //                {
            //                    if (gridView1.Columns[i].Caption.ToUpper().Trim() == ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column Name"].ToString().ToUpper().Trim())
            //                    {
            //                        if ((bool)ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Is Grouped"] == true)
            //                        {
            //                            gridView1.Columns[i].GroupIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["grouporder"]);
            //                        }
            //                        else
            //                        {
            //                            gridView1.Columns[i].VisibleIndex = Convert.ToInt32(ColumnDetailsDS.Tables["ColumnsList"].Rows[j]["Column order"]);
            //                        }
            //                    }
            //                }
            //            }
            //            //Added By Shrikant S. on 08/06/2012    //End
            //            break;
            //        case "Card View":
            //            cardView1.OptionsView.ShowCardCaption = true;
            //            cardView1.CardCaptionFormat = "{1}";
            //            cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            //            cardView1.CardWidth = 600;
            //            gridControl1.MainView = this.cardView1;
            //            cardView1.PopulateColumns();
            //            break;
            //        case "Bended View":
            //            gridControl1.MainView = this.bandedGridView1;
            //            bandedGridView1.PopulateColumns();
            //            bandedGridView1.BestFitColumns();
            //            break;
            //        case "Advanced Bended View":
            //            gridControl1.MainView = this.advBandedGridView1;
            //            advBandedGridView1.PopulateColumns();
            //            advBandedGridView1.BestFitColumns();
            //            break;
            //        default:
            //            break;
            //    }
            //    RestoreReportLayOut("Layout1", comboBoxEdit_SelectLevel.SelectedIndex);
            //    //if (cboReportLayout.Text.Length != 0)       // Added By Shrikant S. on 08/06/2012
            //    //{
            //    //    RestoreReportLayOut(cboReportLayout.Text);
            //    //}
            //}
             //Commented by Archana K. on 18/03/14 for Bug-22080 End
            //Added by Archana K. on 18/03/14 for Bug-22080 start
            ReportLayoutnm = comboBoxEdit_SelectLevel.Text;
            RestoreReportLayOut(ReportLayoutnm, comboBoxEdit_SelectLevel.SelectedIndex);
            //Added by Archana K. on 18/03/14 for Bug-22080 end
        }
        
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                simpleButton_Apply_1.Enabled = true;
            }
            else
            {
                simpleButton_Apply_1.Enabled = false;
                gridView1.OptionsView.ShowAutoFilterRow = false;
            }
        }

        private void simpleButton_Apply_1_Click(object sender, EventArgs e)
        {
            gridView1.OptionsView.ShowAutoFilterRow = true;
        }

        private void cbeSelectLevel_SelectedValueChanged(object sender, EventArgs e)
        {
            cbeField.Properties.Items.Clear();
            for (int i = 0; i < DS.Tables[cbeSelectLevel.SelectedItem.ToString()].Columns.Count; i++)
            {
                string columntype = DS.Tables[cbeSelectLevel.SelectedItem.ToString()].Columns[i].DataType.ToString();
                if (columntype == "System.Decimal" || columntype == "System.Int32")
                {

                    if (LevelCount == cbeSelectLevel.Properties.Items.Count)
                    {
                        cbeField.Properties.Items.Add(DS.Tables[cbeSelectLevel.SelectedItem.ToString()].Columns[i].ColumnName.ToString());
                    }

                }
            }
            cbeField.SelectedIndex = 0;
        }

        private void btnSetCondition_Click(object sender, EventArgs e)
        {
            bool RowExp = false;
            int[] SelectedRowsCount = null;
            DataRowView drowView = null;
            DataRow SelectedRow;
            SelectedRowsCount = gridView1.GetSelectedRows();
            int SelectedRowCount = 0;

            if (cbeSelectLevel.SelectedIndex != 0)
            {
                for (int i = 0; i < SelectedRowsCount.Length; i++)
                {
                    RowExp = gridView1.GetMasterRowExpanded(SelectedRowsCount[i]);
                    if (RowExp)
                    {
                        SelectedRowCount = Convert.ToInt32(SelectedRowsCount[i]);
                        break;
                    }
                }
            }
            else
            {
                RowExp = true;
            }


            if (RowExp)
            {
                drowView = (DataRowView)gridView1.GetRow(SelectedRowCount);
                SelectedRow = drowView.Row;
                ConditionsEditor ConEditor = new ConditionsEditor();
                ConEditor.ConditionFieldText = cbeField.SelectedItem.ToString();
                ConEditor.ShowDialog();
            }
            else
            {
                MessageBox.Show("The Level is not expanded, first expand the level and apply the style Condition", VuMess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        #endregion

        #region Skin Menu Option

        private void barButtonItem_UserFormat1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //this is the default them for the window.
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Caramel");
            int intTheme = 0;
            for (int i = 0; i < listBoxTheme.Items.Count; i++)
            {
                if (listBoxTheme.Items[i].ToString() == "Skin:Caramel")
                    intTheme = i;
            }
            listBoxTheme.SelectedIndex = intTheme;
            navBarControl1.View = listBoxTheme.SelectedItem as DevExpress.XtraNavBar.ViewInfo.BaseViewInfoRegistrator;
            navBarControl1.ResetStyles();
            barManager1.GetController().PaintStyleName = "Skin";

            int gridtheme = 0;
            for (int j = 0; j < comboBoxEdit2.Properties.Items.Count; j++)
            {
                if (comboBoxEdit2.Properties.Items[j].ToString() == "Caramel")
                    gridtheme = j;

            }
            System.EventArgs e1 = new EventArgs();
            comboBoxEdit2.SelectedIndex = gridtheme;
            comboBoxEdit1_SelectedIndexChanged(comboBoxEdit2, e1);



        }

        private void barButtonItem_UserFormat2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue");
            //gridControl1.LookAndFeel.SetStyle(DevExpress.LookAndFeel.LookAndFeelStyle.Skin, false, false, "Blue");
            int intTheme = 0;
            for (int i = 0; i < listBoxTheme.Items.Count; i++)
            {
                if (listBoxTheme.Items[i].ToString() == "Skin:Blue")
                    intTheme = i;
            }
            listBoxTheme.SelectedIndex = intTheme;
            navBarControl1.View = listBoxTheme.SelectedItem as DevExpress.XtraNavBar.ViewInfo.BaseViewInfoRegistrator;
            navBarControl1.ResetStyles();
            barManager1.GetController().PaintStyleName = "Skin";

            int gridtheme = 0;
            for (int j = 0; j < comboBoxEdit2.Properties.Items.Count; j++)
            {
                if (comboBoxEdit2.Properties.Items[j].ToString() == "Blue")
                    gridtheme = j;

            }
            System.EventArgs e1 = new EventArgs();
            comboBoxEdit2.SelectedIndex = gridtheme;
            comboBoxEdit1_SelectedIndexChanged(comboBoxEdit2, e1);

        }

        private void barButtonItem_UserFormat3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Lilian");
            navBarControl1.LookAndFeel.SetStyle(DevExpress.LookAndFeel.LookAndFeelStyle.Skin, false, false, "Lilian");
            int intTheme = 0;
            for (int i = 0; i < listBoxTheme.Items.Count; i++)
            {
                if (listBoxTheme.Items[i].ToString() == "Skin:Lilian")
                    intTheme = i;
            }
            listBoxTheme.SelectedIndex = intTheme;
            navBarControl1.View = listBoxTheme.SelectedItem as DevExpress.XtraNavBar.ViewInfo.BaseViewInfoRegistrator;
            navBarControl1.ResetStyles();
            barManager1.GetController().PaintStyleName = "Skin";

            int gridtheme = 0;
            for (int j = 0; j < comboBoxEdit2.Properties.Items.Count; j++)
            {
                if (comboBoxEdit2.Properties.Items[j].ToString() == "Lilian")
                    gridtheme = j;

            }
            System.EventArgs e1 = new EventArgs();
            comboBoxEdit2.SelectedIndex = gridtheme;
            comboBoxEdit1_SelectedIndexChanged(comboBoxEdit2, e1);

        }

        private void barButtonItem_UserFormat4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Black");
            navBarControl1.LookAndFeel.SetStyle(DevExpress.LookAndFeel.LookAndFeelStyle.Skin, false, false, "Black");
            int intTheme = 0;
            for (int i = 0; i < listBoxTheme.Items.Count; i++)
            {
                if (listBoxTheme.Items[i].ToString() == "Skin:Black")
                    intTheme = i;
            }
            listBoxTheme.SelectedIndex = intTheme;
            navBarControl1.View = listBoxTheme.SelectedItem as DevExpress.XtraNavBar.ViewInfo.BaseViewInfoRegistrator;
            navBarControl1.ResetStyles();
            barManager1.GetController().PaintStyleName = "Skin";

            int gridtheme = 0;
            for (int j = 0; j < comboBoxEdit2.Properties.Items.Count; j++)
            {
                if (comboBoxEdit2.Properties.Items[j].ToString() == "Black")
                    gridtheme = j;

            }
            System.EventArgs e1 = new EventArgs();
            comboBoxEdit2.SelectedIndex = gridtheme;
            comboBoxEdit1_SelectedIndexChanged(comboBoxEdit2, e1);

        }

        #endregion

        #region adding Right Click Menu Items with Column Freezing

        private void gridView1_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)
            {
                DevExpress.XtraGrid.Menu.GridViewColumnMenu menu = e.Menu as DevExpress.XtraGrid.Menu.GridViewColumnMenu;
                if (menu.Column != null)
                {
                    menu.Items.Add(menuInfo.CreateCheckItem("Clear Freezing", menu.Column, FixedStyle.None, imageList1.Images[0]));
                    menu.Items.Add(menuInfo.CreateCheckItem("Freezing Left", menu.Column, FixedStyle.Left, imageList1.Images[1]));
                    menu.Items.Add(menuInfo.CreateCheckItem("Freezing Right", menu.Column, FixedStyle.Right, imageList1.Images[2]));
                }
            }
        }

        #endregion

        private void btnApply_Click(object sender, EventArgs e)     //Added By Shrikant S.  on 08/06/2012
        {
            if (cboReportLayout.Text.Length != 0)
            {
                ReportLayoutnm=cboReportLayout.Text;//Added by Archana K. on 19/03/14 for Bug-22080
                //RestoreReportLayOut(cboReportLayout.Text, 0);//Commented by Archana K. on 19/03/14 for Bug-22080
                RestoreReportLayOut(ReportLayoutnm, 0);//Changed by Archana K. on 19/03/14 for Bug-22080
            }
        }
        //Added By Shrikant S. on 09/06/2012            //Start
        private void gridView1_DoubleClick(object sender, EventArgs e)      
        {
            GridView view = (GridView)sender;

            Point pt = view.GridControl.PointToClient(Control.MousePosition);

            DoRowDoubleClick(view, pt);
        }
        private void DoRowDoubleClick(GridView view, Point pt)  //Added By Shrikant S. on 25/06/2012    for bug-4575
        {
            if (IsMultiCompany == false)        //Added by Shrikant S. on 21/05/2013 for Bug-11169
            {
                GridHitInfo info = view.CalcHitInfo(pt);
                if (info.InRow && info.InRowCell)
                {
                    DataRow ldr = view.GetDataRow(info.RowHandle);
                    if (ldr.Table.Columns.Contains("Entry_ty") && ldr.Table.Columns.Contains("Tran_Cd"))
                    {
                        //ClVoucher clv = new ClVoucher();
                        //clv.prVoucher(this.pComDbnm, _mCoSta_dt, _mCoEnd_dt, this.pAppUerName, Defapath, _mAppPath, MainIcon, _mProdCode, ldr[ldr.Table.Columns["Entry_ty"].Ordinal].ToString(), Convert.ToInt32(ldr[ldr.Table.Columns["Tran_cd"].Ordinal].ToString()));
                        //ClVoucher clv = new ClVoucher();
                        ClNetToFxClass netToFx = new ClNetToFxClass();      // Added By Shrikant S. on 29/12/2012 
                        netToFx.PrNetToFx(this.pComDbnm, _mCoSta_dt, _mCoEnd_dt, this.pAppUerName, Defapath, _mAppPath, MainIcon, _mProdCode, "DO ToUeVoucher WITH '" + ldr[ldr.Table.Columns["Entry_ty"].Ordinal].ToString().Trim() + "', " + ldr[ldr.Table.Columns["Tran_cd"].Ordinal].ToString() + ",Thisform.DataSessionId,.T.");      // Added By Shrikant S. on 29/12/2012 
                    }
                }
            }
        }
        private void SetSkin(string SkinColor)      //Added By Shrikant S. on 25/06/2012    for Bug-4575
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(SkinColor);
            navBarControl1.LookAndFeel.SetStyle(DevExpress.LookAndFeel.LookAndFeelStyle.Skin, false, false, SkinColor);
            int intTheme = 0;
            for (int i = 0; i < listBoxTheme.Items.Count; i++)
            {
                if (listBoxTheme.Items[i].ToString() == ("Skin:" + SkinColor))
                    intTheme = i;
            }
            listBoxTheme.SelectedIndex = intTheme;
            navBarControl1.View = listBoxTheme.SelectedItem as DevExpress.XtraNavBar.ViewInfo.BaseViewInfoRegistrator;
            navBarControl1.ResetStyles();
            barManager1.GetController().PaintStyleName = "Skin";

            int gridtheme = 0;
            for (int j = 0; j < comboBoxEdit2.Properties.Items.Count; j++)
            {
                if (comboBoxEdit2.Properties.Items[j].ToString() == SkinColor)
                    gridtheme = j;

            }
            System.EventArgs e1 = new EventArgs();
            comboBoxEdit2.SelectedIndex = gridtheme;
            comboBoxEdit1_SelectedIndexChanged(comboBoxEdit2, e1);

        }
        private void barCheckItem_SummaryFooter_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.SetToolTip();      //Added By Shrikant S. on 25/06/2012    for bug-4575
        }

        private void barCheckItem_ShowHeaders_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.SetToolTip();      //Added By Shrikant S. on 25/06/2012    for bug-4575
        }
        //Added By Shrikant S. on 09/06/2012            //End

        //Added By Shrikant S. on 21/05/2013 for Bug-11169      //Start
        private void CheckCompanyType()
        {
            SqlCommand lcmd = new SqlCommand("Select Top 1 Com_Type From Vudyog..Co_Mast Where CompId=" + this.pCompId.ToString(), con);
            con.Open();
            string oComType=Convert.ToString(lcmd.ExecuteScalar());
            if (oComType.Trim()=="M")
            {
                IsMultiCompany = true;
            }
            con.Close();
        }

        private void barButtonItem_ChangeLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Added by Archana K. on 13/03/14 for Bug-22080
            flag = false;
            SaveLayOutNew();

        }
        
        private void barButtonItem_ClearLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshLayout();//Added by Archana K. on 21/03/14 for Bug-22080
            barStaticItem1.Caption = "Current Layout :Default";
        }


      

        //Added By Shrikant S. on 21/05/2013 for Bug-11169      //End
    }
    #endregion
}

#region Commented Code

//con = new SqlConnection(conString);
//Command = new SqlCommand();
//Command.CommandType = CommandType.Text;
//Command.CommandText = "SELECT conid as connectionID,repid as reportID,const as ConnectionString FROM uscon where repid=" + ReportID;
//Command.Connection = con;

//Adapter = new SqlDataAdapter(Command);

//Adapter.Fill(LocalDS, "TblConnectionString");

//ReportConString = LocalDS.Tables["tblConnectionString"].Rows[0]["ConnectionString"].ToString();


//DevExpress.XtraGrid.Views.Grid.GridView gridview11 = new DevExpress.XtraGrid.Views.Grid.GridView();
//gridview11.OptionsView.ColumnAutoWidth = false;
//gridControl1.MainView = gridview11;
//gridview11.PopulateColumns();      

//DevExpress.XtraGrid.Views.Card.CardView CardView11 = new DevExpress.XtraGrid.Views.Card.CardView();
//gridControl1.MainView = CardView11;
//CardView11.PopulateColumns();

//DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bendedGridView11 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
//gridControl1.MainView = bendedGridView11;
//bendedGridView11.PopulateColumns();

//DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView AddvancedBendedGridView11 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
//gridControl1.MainView = AddvancedBendedGridView11;
//AddvancedBendedGridView11.PopulateColumns();


//DevExpress.XtraGrid.Views.Grid.GridView gridview22 = new DevExpress.XtraGrid.Views.Grid.GridView();
//gridview22.OptionsView.ColumnAutoWidth = false;
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), gridview22);
//gridview22.PopulateColumns();

//DevExpress.XtraGrid.Views.Card.CardView CardView22 = new DevExpress.XtraGrid.Views.Card.CardView();                                        
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), CardView22);
//CardView22.PopulateColumns();

//DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bendedGridView22 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), bendedGridView22);
//bendedGridView22.PopulateColumns(DS.Tables[LevelIndex]);

//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), AddvancedBendedGridView22);
//AddvancedBendedGridView22.PopulateColumns(DS.Tables[LevelIndex]);


//DevExpress.XtraGrid.Views.Grid.GridView gridview33 = new DevExpress.XtraGrid.Views.Grid.GridView();
//gridview33.OptionsView.ColumnAutoWidth = false;
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), gridview33);
//gridview33.PopulateColumns(DS.Tables[LevelIndex]);

//DevExpress.XtraGrid.Views.Card.CardView CardView33 = new DevExpress.XtraGrid.Views.Card.CardView();                                        
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), CardView33);
//CardView33.PopulateColumns(DS.Tables[LevelIndex]);

//DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bendedGridView33 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();                                        
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), bendedGridView33);
//bendedGridView33.PopulateColumns(DS.Tables[LevelIndex]);

//DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView AddvancedBendedGridView33 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();                                        
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), AddvancedBendedGridView33);
//AddvancedBendedGridView33.PopulateColumns(DS.Tables[LevelIndex]);



//DevExpress.XtraGrid.Views.Grid.GridView gridview44 = new DevExpress.XtraGrid.Views.Grid.GridView();
//gridview44.OptionsView.ColumnAutoWidth = false;
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), gridview44);
//gridview44.PopulateColumns(DS.Tables[LevelIndex]);

//DevExpress.XtraGrid.Views.Card.CardView CardView44 = new DevExpress.XtraGrid.Views.Card.CardView();
//CardView44.OptionsView.ShowCardCaption = true;
//CardView44.CardCaptionFormat = "{1}";
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), CardView44);                                        
//CardView44.PopulateColumns(DS.Tables[LevelIndex]);

//DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bendedGridView44 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();                                        
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), bendedGridView44);
//bendedGridView44.PopulateColumns(DS.Tables[LevelIndex]);

//DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView AddvancedBendedGridView44 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();                                        
//gridControl1.LevelTree.Nodes.Add(ReportDispalyName[LevelIndex].ToString(), AddvancedBendedGridView44);
//AddvancedBendedGridView44.PopulateColumns(DS.Tables[LevelIndex]);


//#region Hide Indexed Columns in Grids

//       private void HidingColumns()
//       {
//           for (int i = 0; i < gridView2.Columns.Count; i++)
//           {
//               if (gridView2.Columns[i].Caption == "tran_cd")
//               {
//                   gridView2.Columns[i].Visible = false;
//               }
//               else if (gridView2.Columns[i].Caption == "ac_id")
//               {
//                   gridView2.Columns[i].Visible = false;
//               }
//           }

//           for (int i = 0; i < gridView3.Columns.Count; i++)
//           {
//               if (gridView3.Columns[i].Caption == "tran_cd")
//               {
//                   gridView3.Columns[i].Visible = false;
//               }
//               else if (gridView3.Columns[i].Caption == "ac_id")
//               {
//                   gridView3.Columns[i].Visible = false;
//               }
//           }

//           for (int i = 0; i < gridView4.Columns.Count; i++)
//           {
//               if (gridView4.Columns[i].Caption == "tran_cd")
//               {
//                   gridView4.Columns[i].Visible = false;
//               }
//               else if (gridView4.Columns[i].Caption == "ac_id")
//               {
//                   gridView4.Columns[i].Visible = false;
//               }
//           }

//           for (int i = 0; i < cardView1.Columns.Count; i++)
//           {
//               if (cardView1.Columns[i].Caption == "tran_cd")
//               {
//                   cardView1.Columns[i].Visible = false;
//               }
//               else if (cardView1.Columns[i].Caption == "ac_id")
//               {
//                   cardView1.Columns[i].Visible = false;
//               }
//           }

//           for (int i = 0; i < bandedGridView1.Columns.Count; i++)
//           {
//               if (bandedGridView1.Columns[i].Caption == "tran_cd")
//               {
//                   bandedGridView1.Columns[i].Visible = false;
//               }
//               else if (bandedGridView1.Columns[i].Caption == "ac_id")
//               {
//                   bandedGridView1.Columns[i].Visible = false;
//               }
//           }

//           for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
//           {
//               if (advBandedGridView1.Columns[i].Caption == "tran_cd")
//               {
//                   advBandedGridView1.Columns[i].Visible = false;
//               }
//               else if (advBandedGridView1.Columns[i].Caption == "ac_id")
//               {
//                   advBandedGridView1.Columns[i].Visible = false;
//               }
//           }
//       }

//       #endregion

#endregion