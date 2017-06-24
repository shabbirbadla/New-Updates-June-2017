using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using DataAccess_Net;
using System.Threading;
using System.Collections;
using ScriptDotNet;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.IO;

using System.Diagnostics;/*Added Rup 07/03/2011*/
using uBaseForm;/*Added Rup 07/03/2011*/
using uTextBox;/*Added Rup 07/03/2011*/
using uCheckBox;/*Added Rup 07/03/2011*/
using uHelpButton;
using System.Data.SqlClient;
namespace DynamicMaster
{
    public partial class MasterForm : uBaseForm.FrmBaseForm 
    {
        string _MasterCode;
        string _TableName;
        //string _UniqueColName;
        string vSearchFields=string.Empty ,vSearchFieldsCap=string.Empty;
       //ArrayList _UniqueColumns;
       //ArrayList _UniqueColumnCaptions;
        string _HelpQuery;
        DynamicFormClass.clsDynamicForm oForm = new DynamicFormClass.clsDynamicForm();
        DataAccess_Net.clsDataAccess oDataAccess;
        private DataSet dsFormInfo;
        private DataSet dsFieldInfo;
        //private FormMode _Mode;
        //private string _CompId;
        private int _Range;
        private string _UserName;
        private string vMainField=string.Empty,vMainFldVal=string.Empty,VmainFieldCaption=string.Empty ;
        Boolean vValid;
        //private String pAppName, cAppName, pAppCaption, pApplCode, pAppPId,  /*Added Rup 07/03/2011*/
        private String cAppPId,cAppName; /*Added Rup 07/03/2011*/
        DataView dvwMainm = new DataView();
        TextBox txttemp = new TextBox();
        BindingManagerBase bm;
        Binding bd;

        Dictionary<string, string> validations;
        private DataSet dsMain = new DataSet();
        //private DataSet dsMainm = new DataSet();
        private enum FormMode
        {
            New, Edit
        };

        //public MasterForm(string[] args)
        public MasterForm()
        {
            InitializeComponent();



            //_CompId = args[0];
            //DataAccess_Net.clsDataAccess._databaseName = args[1];
            //DataAccess_Net.clsDataAccess._serverName = args[2];
            //DataAccess_Net.clsDataAccess._userID = args[3];
            //DataAccess_Net.clsDataAccess._password = args[4];
            //_Range = Convert.ToInt32(args[5].ToString().Replace("^", ""));
            //_MasterCode = args[6];
            //_UserName = args[7];
            //oDataAccess = new DataAccess_Net.clsDataAccess();
            ///*Rup 07/03/2011--->*/
            //pAppCaption = args[9].Replace("<*#*>", " ");
            //pAppName = args[10];
            //pAppPId = args[11];
            //pApplCode = args[12];
            //Icon MainIcon = new System.Drawing.Icon(args[8].Replace("<*#*>", " "));
            //this.Icon = MainIcon;
            ////*<--- 07/03/2011 Rup*/

            //MessageBox.Show("Database: " + args[1] + " " +
            //                "Server: " + args[2] + " " +
            //                "UserID: " + args[3] + " " +
            //                "Password: " + args[4]);
        }

        private void MasterForm_Load(object sender, EventArgs e)
        {
            oForm = new DynamicFormClass.clsDynamicForm();

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            //_UniqueColumns = new ArrayList();
            //_UniqueColumnCaptions = new ArrayList();
            validations = new Dictionary<string, string>();
            this.pAppPath = Application.StartupPath;
            PreviewDynamicMasterForm( sender,  e,_MasterCode);
            /*07/03/2011 Rup--->*/
            this.btnCancel.Enabled = false;
            this.btnLocate.Enabled = false; 
            this.btnPreview.Enabled=false;
            this.btnPrint.Enabled=false;
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Visible = false;
            this.btnEmail.Enabled = false;
            /*<--- 07/03/2011 Rup*/
            //this.Text = _MasterCode + "(" + _TableName + ")";
            //SetMenuRights();
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udDynamicMaster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" +this.pPApplCode+"','"+DateTime.Now.Date.ToString()+"','" +this.pPApplName + "'," + this.pPApplPID   + ",'" + this.pPApplText   + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName  + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            //oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mcheckCallingApplication()/*Added Rup 07/03/2011*/
        {
            Process pProc;
            Boolean procExists=true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this.pPApplPID ));
                String pName = pProc.ProcessName;
                string pName1 = this.pPApplName.Substring(0, this.pPApplName.IndexOf("."));
                if (pName.ToUpper()  != pName1.ToUpper())
                {
                    procExists = false;
                }
            }
            catch (Exception )
            {
                procExists = false;

            }
            if (procExists == false)
            {
                MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
            

            //DataSet dsData = new DataSet();
            //string sqlstr;

            //sqlstr = " Select * from vudyog..ExtApplLog where pApplCode='"+this.pApplCode+"'";

            //dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            //if (dsData.Tables[0].Rows.Count == 0)
            //{
            //    MessageBox.Show("Can't proceed,Main Application " + this.pAppCaption + " is closed");
            //    Application.Exit();
            //}
        }
        private Color GetFormColor()
        {
            string colorCode = string.Empty;
            Color myColor = Color.Coral;
            colorCode = oForm.GetColorCode(Convert.ToInt32(this.pCompId), oDataAccess);
            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            return myColor;
        }

        private void SetMenuRights()
        {
            btnNew.Enabled = false;
            btnEdit.Enabled = false;
            //menuItemRemoveToolbar.Enabled = false;
            btnPrint.Enabled = false;

            DataSet dsRights = new DataSet();

            dsRights = oForm.GetUserRightsForMenu(_Range, _UserName, oDataAccess);
            if (dsRights != null)
            {
                string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                //MessageBox.Show("Rights: " + rights);
                int len = rights.Length;
                string newString = "";
                ArrayList myArray = new ArrayList();

                while (len > 2)
                {
                    newString = rights.Substring(0, 2);
                    rights = rights.Substring(2);
                    myArray.Add(newString);
                    len = rights.Length;
                }
                myArray.Add(rights);

                btnNew.Enabled = (myArray[0].ToString().Trim() == "IY" ? true : false);
                btnEdit.Enabled = (myArray[1].ToString().Trim() == "CY" ? true : false);
                //menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }

        #region Preview Form

        private void PreviewDynamicMasterForm(object sender, EventArgs e,string FormCode)
        {
            DataSet dsFormList = new DataSet();
            DataSet dsTabList = new DataSet();
            DataSet dsFieldList = new DataSet();
            string tableName = string.Empty;

            int formID = 0;

            try
            {
                dsFormList = oForm.GetFormList(FormCode, oDataAccess);
                if (dsFormList == null)
                {
                    return;
                }
                if (dsFormList.Tables[0].Rows.Count > 0)
                {
                    formID = Convert.ToInt32(dsFormList.Tables[0].Rows[0]["ID"]);
                    //dsTabList = oForm.GetTabControlList(formID);
                    tableName = dsFormList.Tables[0].Rows[0]["Table_Name"].ToString();
                    _TableName = tableName;
                    this.Text = dsFormList.Tables[0].Rows[0]["CAPTION"].ToString();
                    CreateTabs(_MasterCode);
                    /*07/03/2011 Rup--->*/
                    this.mInsertProcessIdRecord();
                    /*<--- 07/03/2011 Rup*/
                    //dsMain

                    this.btnLast_Click(sender, e);
                    //string sqlstr = "select distinct " + vMainField + " from " + this._TableName + " order by " + vMainField;
                    //dsMainm = oDataAccess.GetDataSet(sqlstr, null, 20);
                    //dvwMainm = dsMainm.Tables[0].DefaultView;
                    //dvwMainm.Sort = vMainField ;
                    //bd = new Binding("text", dvwMainm, vMainField );
                    //bm = this.BindingContext[dvwMainm];

                    //txttemp.Visible = true;
                    //txttemp.Height = 0;
                    //txttemp.Width = 0;
                    //this.Controls.Add(txttemp);
                    //this.txttemp.DataBindings.Add(bd);
                    //this.mthLastRec();
                    
                    //BindData(tableName);
                    //object objects = this;
                    //EnableDiableFormControls(GetTabControl(objects), false , true);
                }
                else
                {
                    MessageBox.Show("No form found with the given name.\nApplication will exit.");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dsFormList = null;
                dsTabList = null;
                dsFieldList = null;
            }
        }
        private void mthLastRec()
        {
            bm.Position = this.dvwMainm.Table.Rows.Count - 1;
            this.mthSubView();
            this.pAddMode = false;
            this.pEditMode = false;
        }
        private void mthSubView()
        {
            try
            {
                string sqlstr = "select top 1 * from " + this._TableName + " where " + vMainField +"='"+vMainFldVal+"'"+ " order by " + vMainField;
                dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            //object objects = this;
            //mthControlBind(GetTabControl(objects));
            object objects = this;
            mthControlBind(GetTabControl(objects));
            EnableDiableFormControls(GetTabControl(objects), false , true);
        }
        private void mthControlBind(Control tabCtrl)
        {
            
            try
            {
                foreach (TabPage tPage in tabCtrl.Controls)
                {
                    foreach (Control childCtrl in tPage.Controls)
                    {
                        switch (childCtrl.GetType().Name.ToLower())
                        {
                            case "ctextbox":
                                childCtrl.DataBindings.Clear();
                                childCtrl.DataBindings.Add("Text", dsMain.Tables[0], childCtrl.Name.ToString());
                                break;
                            case "maskedtextbox":
                                childCtrl.DataBindings.Clear();
                                childCtrl.DataBindings.Add("Text", dsMain.Tables[0], childCtrl.Name.ToString());
                                break;
                            case "datetimepicker":
                                childCtrl.DataBindings.Clear();
                                DateTimePicker m = (DateTimePicker)childCtrl;
                                Binding b = new Binding("Text", dsMain.Tables[0], childCtrl.Name.ToString());
                                //b.FormatString = "MM/dd/yyyy";
                                m.DataBindings.Add("Text", dsMain.Tables[0], childCtrl.Name.ToString());
                                break;
                            case "checkbox":
                                childCtrl.DataBindings.Clear();
                                childCtrl.DataBindings.Add("Checked", dsMain.Tables[0], childCtrl.Name.ToString());
                                break;
                            case "combobox":
                                break;
                            case "label":
                                if (childCtrl.Name == "lblSearchField")
                                {
                                    childCtrl.DataBindings.Clear();
                                   // childCtrl.DataBindings.Add("Text", dsMain, "dsMain.ID");
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { }
        }
        private void CreateTabs(string masterFormCode)
        {
            int tabId = 0;
            string tabCaption = "";
            string tabCode = "";
            int tabOrder = 0;
            DataSet dsTabList = new DataSet();
            try
            {
                Color c = GetFormColor();
                dsTabList = oForm.GetTabControlList(masterFormCode, oDataAccess);
                if (dsTabList.Tables.Count > 0)
                {
                    if (dsTabList.Tables[0].Rows.Count > 0)
                    {
                        //Create TabControl
                        TabControl mainTab = new TabControl();
                        mainTab.Dock = DockStyle.Fill;
                        //mainTab.BackColor = Color.FromArgb(c.R, c.R, c.G, c.B);
                        for (int i = 0; i < dsTabList.Tables[0].Rows.Count; i++)
                        {
                            tabId = Convert.ToInt32(dsTabList.Tables[0].Rows[i]["id"]);
                            tabCaption = dsTabList.Tables[0].Rows[i]["caption"].ToString();
                            tabCode = dsTabList.Tables[0].Rows[i]["code"].ToString();
                            tabOrder = Convert.ToInt32(dsTabList.Tables[0].Rows[i]["tab_order"]);

                            //Create TabPages
                            TabPage tempTabPage = new TabPage();
                            tempTabPage.Name = tabCode;
                            tempTabPage.Tag = tabCode;
                            tempTabPage.Text = tabCaption;
                            tempTabPage.UseVisualStyleBackColor = false;
                            tempTabPage.BackColor = Color.FromArgb(c.R, c.R, c.G, c.B);

                            //Create Fields under each Tab
                            CreateTabFields(tabCode, tempTabPage);

                            //Add each TabPage to the TabControl
                            tempTabPage.AutoScroll = true;
                            mainTab.TabPages.Add(tempTabPage);
                            mainTab.SelectedIndex = 0;

                            this.Controls.Add(mainTab);
                            this.Height = 500;
                            this.Width = 600;
                            this.ResumeLayout(false);
                        }
                        mainTab.BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CreateTabFields1(string tabCode, TabPage tempTabPage)
        {
            //DataSet dsFieldList = new DataSet();
            //string fieldCaption = string.Empty;
            //string fieldTooltip = string.Empty;
            //string validation = string.Empty;
            //string fieldName = string.Empty;
            //string helpQuery = string.Empty;
            //string defaultValue = string.Empty;
            //int fieldOrder = 0;
            //string dataType = string.Empty;
            //int fieldSize = 0;
            //bool isMandatory = false;
            //bool isUnique = false;
            //bool isVisible = true;

            //try
            //{
            //    tempTabPage.BringToFront();
            //    dsFieldList = oForm.GetFieldList(tabCode, _MasterCode, oDataAccess);
            //    if (dsFieldList.Tables.Count > 0)
            //    {
            //        if (dsFieldList.Tables[0].Rows.Count > 0)
            //        {
            //            dsFieldInfo = dsFieldList;
            //            //TableLayoutPanel tableLayoutPanel1 = new TableLayoutPanel();
            //            ////tableLayoutPanel1.ColumnCount = 4;
            //            ////tableLayoutPanel1.RowCount = dsFieldList.Tables[0].Rows.Count;
            //            //tableLayoutPanel1.Dock = DockStyle.Fill;
            //            //tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //            ////tempTabPage.Controls.Add(tableLayoutPanel1);
            //            int MaxCtrlWidth = 150; // GetMaxControlWidth(arrFields);

            //            //this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            //            ToolTip myTooltip = new ToolTip();

            //            Label lblUnique = new Label();
            //            lblUnique.Name = "lblUnique";
            //            lblUnique.Tag = "lblUnique";
            //            lblUnique.Visible = true;
            //            lblUnique.Height = 0;
            //            lblUnique.Width = 0;
            //            for (int i = 0; i < dsFieldList.Tables[0].Rows.Count; i++)
            //            {
            //                fieldCaption = dsFieldList.Tables[0].Rows[i]["caption"].ToString();
            //                fieldTooltip = dsFieldList.Tables[0].Rows[i]["tooltip"].ToString();
            //                fieldName = dsFieldList.Tables[0].Rows[i]["fieldname"].ToString();
            //                if (dsFieldList.Tables[0].Rows[i]["helpquery"] != null)
            //                {
            //                    helpQuery = dsFieldList.Tables[0].Rows[i]["helpquery"].ToString();
            //                    _HelpQuery = helpQuery;
            //                }
            //                validation = dsFieldList.Tables[0].Rows[i]["validation"].ToString();
            //                if (!string.IsNullOrEmpty(validation))
            //                {
            //                    validations.Add(fieldName, validation);
            //                }
            //                defaultValue = dsFieldList.Tables[0].Rows[i]["defaultvalue"].ToString();
            //                fieldOrder = Convert.ToInt32(dsFieldList.Tables[0].Rows[i]["field_order"]);
            //                isMandatory = (dsFieldList.Tables[0].Rows[i]["mandatory"].ToString() == "True" ? true : false);
            //                isUnique = (dsFieldList.Tables[0].Rows[i]["unique"].ToString() == "True" ? true : false);
            //                dataType = dsFieldList.Tables[0].Rows[i]["datatype"].ToString();
            //                fieldSize = Convert.ToInt32(dsFieldList.Tables[0].Rows[i]["size"]);
            //                isVisible = (dsFieldList.Tables[0].Rows[i]["internaluse"].ToString() == "False" ? true : false);

            //                Label labelCaption = new Label();
            //                Label lblMandatory = new Label();
            //                labelCaption.Name = fieldName + i;
            //                labelCaption.Tag = dataType;
            //                labelCaption.AutoSize = true;
            //                labelCaption.Font = new Font(labelCaption.Font, FontStyle.Regular);
            //                labelCaption.Size = new System.Drawing.Size(MaxCtrlWidth, 22);
            //                labelCaption.Location = new Point(100, 25 + i * 30);
            //                if (isMandatory)
            //                {
            //                    labelCaption.Text = fieldCaption;
            //                    lblMandatory.ForeColor = Color.Red;
            //                    lblMandatory.Text = "*";
            //                    lblMandatory.Name = fieldName;
            //                    lblMandatory.Tag = dataType;
            //                    lblMandatory.Width = 15;
            //                    lblMandatory.AutoSize = true;
            //                    lblMandatory.Location = new Point(labelCaption.Right - 40, 25 + i * 30);
            //                    tempTabPage.Controls.Add(lblMandatory);
            //                    //tableLayoutPanel1.Controls.Add(lblMandatory, 1, i);
            //                }
            //                else
            //                {
            //                    labelCaption.Text = fieldCaption.Trim();
            //                }
            //                labelCaption.Visible = isVisible;
            //                lblUnique.Location = new Point(0, 0);

            //                tempTabPage.Controls.Add(lblUnique);
            //                tempTabPage.Controls.Add(labelCaption);
            //                //tableLayoutPanel1.Controls.Add(lblUnique, 0, i);
            //                //tableLayoutPanel1.Controls.Add(labelCaption, 0, i);
            //                switch (dataType.ToUpper())
            //                {
            //                    case "VARCHAR":
            //                        //Add Textbox
            //                        TextBox txtBox = new TextBox();
            //                        txtBox.Name = fieldName;
            //                        txtBox.Tag = dataType;
            //                        txtBox.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        //t1.Location = new System.Drawing.Point(XPos, YPos);
            //                        txtBox.Text = defaultValue;
            //                        txtBox.Width = MaxCtrlWidth;
            //                        txtBox.Visible = isVisible;
            //                        txtBox.ReadOnly = true;
            //                        txtBox.MaxLength = fieldSize;
            //                        txtBox.Leave += new EventHandler(txtBox_Leave);
            //                        myTooltip.SetToolTip(txtBox, fieldName);
            //                        if (isMandatory)
            //                            txtBox.Validating += new CancelEventHandler(textbox_Validating);
            //                        tempTabPage.Controls.Add(txtBox);
            //                        //tableLayoutPanel1.Controls.Add(txtBox, 2, i);
            //                        if (isUnique)
            //                        {
            //                            _UniqueColName = fieldName;
            //                            _UniqueColumns.Add(fieldName);
            //                            _UniqueColumnCaptions.Add(fieldCaption);
            //                            Button b1 = new Button();
            //                            b1.Name = fieldName;
            //                            b1.Tag = fieldCaption;// _HelpQuery;
            //                            b1.Image = global::DynamicMaster.Properties.Resources.search_icon;
            //                            b1.Size = new System.Drawing.Size(25, 19);
            //                            b1.Location = new Point(txtBox.Location.X + txtBox.Width + 2, txtBox.Location.Y);
            //                            b1.Click += new EventHandler(myButton_Click);
            //                            tempTabPage.Controls.Add(b1);
            //                            //tableLayoutPanel1.Controls.Add(b1, 3, i);
            //                        }
            //                        //if (!string.IsNullOrEmpty(helpQuery))
            //                        //{
            //                        //    ComboBox cmb = new ComboBox();
            //                        //    DataSet ds = new DataSet();
            //                        //    ds = oDataAccess.GetDataSet(helpQuery, null, 20);
            //                        //    if (ds != null)
            //                        //    { }
            //                        //}
            //                        break;
            //                    //Check if IsMandatory is true. then create a validation for this control.
            //                    case "DECIMAL":
            //                        //if (!string.IsNullOrEmpty(helpQuery))
            //                        //{
            //                        //    ComboBox cmb = new ComboBox();
            //                        //    DataSet ds = new DataSet();
            //                        //    ds = oDataAccess.GetDataSet(helpQuery, null, 20);
            //                        //    cmb.Name = fieldName;
            //                        //    cmb.Tag = dataType;
            //                        //    cmb.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        //    cmb.Enabled = false;
            //                        //    if (ds != null)
            //                        //    {
            //                        //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //                        //        {
            //                        //            if (ds.Tables[0].Rows[j][0] != null)
            //                        //                cmb.Items.Add(ds.Tables[0].Rows[j][0].ToString());
            //                        //        }
            //                        //    }
            //                        //    tempTabPage.Controls.Add(cmb);
            //                        //}
            //                        //else
            //                        //{
            //                        //Add Textbox
            //                        TextBox txtDecimal = new TextBox();
            //                        txtDecimal.Name = fieldName;
            //                        txtDecimal.Tag = dataType;
            //                        txtDecimal.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        //t2.Location = new System.Drawing.Point(XPos, YPos);
            //                        txtDecimal.Text = defaultValue;
            //                        txtDecimal.Width = MaxCtrlWidth;
            //                        txtDecimal.Visible = isVisible;
            //                        txtDecimal.ReadOnly = true;
            //                        txtDecimal.Leave += new EventHandler(txtBox_Leave);
            //                        myTooltip.SetToolTip(txtDecimal, fieldName);
            //                        txtDecimal.KeyPress += new KeyPressEventHandler(OnKeyPress);
            //                        txtDecimal.TextChanged += new EventHandler(OnTextChanged);
            //                        if (isMandatory)
            //                            txtDecimal.Validating += new CancelEventHandler(textbox_Validating);
            //                        tempTabPage.Controls.Add(txtDecimal);
            //                        //}


            //                        //tableLayoutPanel1.Controls.Add(txtDecimal, 2, i);

            //                        //System.Windows.Forms.NumericUpDown n = new System.Windows.Forms.NumericUpDown();
            //                        //n.Minimum = 0;
            //                        //n.Maximum = 255;

            //                        //MaskedTextBox txtDecimal = new MaskedTextBox();
            //                        //txtDecimal.Mask = "####.##";
            //                        //txtDecimal.Name = fieldName;
            //                        //txtDecimal.Tag = dataType;
            //                        ////datetimTxt.PasswordChar = '0';
            //                        //txtDecimal.Text = defaultValue;
            //                        //txtDecimal.ValidatingType = typeof(System.Decimal);
            //                        //txtDecimal.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        ////maskedTextBox1.Location = new System.Drawing.Point(XPos, YPos);
            //                        ////txtDecimal.Validating += new CancelEventHandler(maskedTextBox_Validating);
            //                        //txtDecimal.Visible = isVisible;
            //                        //txtDecimal.ReadOnly = true;
            //                        //myTooltip.SetToolTip(txtDecimal, fieldName);
            //                        //tempTabPage.Controls.Add(txtDecimal);
            //                        break;
            //                    case "BIT":
            //                        //Add Checkbox/Radiobutton
            //                        CheckBox chk1 = new CheckBox();
            //                        chk1.Name = fieldName;
            //                        chk1.Tag = dataType;
            //                        //chk1.Text = fieldCaption;
            //                        chk1.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        //chk1.Location = new System.Drawing.Point(XPos, YPos);
            //                        chk1.Visible = isVisible;
            //                        chk1.Enabled = false;
            //                        myTooltip.SetToolTip(chk1, fieldName);
            //                        tempTabPage.Controls.Add(chk1);
            //                        //tableLayoutPanel1.Controls.Add(chk1, 2, i);
            //                        break;
            //                    case "DATETIME":
            //                        DateTimePicker dtPicker1 = new DateTimePicker();
            //                        dtPicker1.Name = fieldName;
            //                        dtPicker1.Width = MaxCtrlWidth;
            //                        dtPicker1.Tag = dataType;
            //                        dtPicker1.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        dtPicker1.Format = DateTimePickerFormat.Short;
            //                        dtPicker1.Size = new System.Drawing.Size(100, 30);
            //                        dtPicker1.Enabled = false;
            //                        tempTabPage.Controls.Add(dtPicker1);
            //                        //tableLayoutPanel1.Controls.Add(dtPicker1, 2, i);
            //                        break;
            //                    case "TEXT":
            //                        //Add Textbox
            //                        TextBox txtText = new TextBox();
            //                        txtText.Name = fieldName;
            //                        txtText.Tag = dataType;
            //                        txtText.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
            //                        //t4.Location = new System.Drawing.Point(XPos, YPos);
            //                        txtText.Multiline = true;
            //                        txtText.ScrollBars = ScrollBars.Both;
            //                        txtText.Text = defaultValue;
            //                        txtText.Width = MaxCtrlWidth;
            //                        //t4.Height = 50;
            //                        txtText.Width = 200;
            //                        txtText.Visible = isVisible;
            //                        txtText.ReadOnly = true;
            //                        txtText.Leave += new EventHandler(txtBox_Leave);
            //                        myTooltip.SetToolTip(txtText, fieldName);
            //                        tempTabPage.Controls.Add(txtText);
            //                        //tableLayoutPanel1.Controls.Add(t4, 2, i);
            //                        break;
            //                }
            //            }
            //            //tempTabPage.Controls.Add(tableLayoutPanel1);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    dsFieldList = null;
            //}
        }
        
        private void CreateTabFields(string tabCode, TabPage tempTabPage)
        {
            DataSet dsFieldList = new DataSet();
            
            string vCaption=string.Empty;
            string vToolTip = string.Empty;
            Boolean  vMandatory = false ;
            string vFieldName = string.Empty;
            string vDataType = string.Empty;
            Int16  vSize = 0;
            Int16 vDecimal = 0;
            Boolean  vSearchField = false ;
            string vInputMask = string.Empty;
            string vHelpQuery = string.Empty;
            string vReadonly = string.Empty;
            string vDefaulValue = string.Empty;
            string vValidation = string.Empty;
            Boolean  vMField=false ;
            Boolean vInternalUse = false;
            String vErroMsg = string.Empty;
            Boolean  vIsVisible = true;
            Int16 vLineCount;
            try
            {
                tempTabPage.BringToFront();
                dsFieldList = oForm.GetFieldList(tabCode, _MasterCode, oDataAccess);
                if (dsFieldList.Tables.Count > 0)
                {
                    if (dsFieldList.Tables[0].Rows.Count > 0)
                    {
                        dsFieldInfo = dsFieldList;
                        int MaxCtrlWidth = 150; // GetMaxControlWidth(arrFields);
                        int MaxTxtWidth = 250;
                        ToolTip myTooltip = new ToolTip();
                        Label lblSearchField = new Label();
                        lblSearchField.Name = "lblSearchField";
                        lblSearchField.Tag = "lblSearchField";
                        lblSearchField.Visible = true;
                        lblSearchField.Height = 0;
                        lblSearchField.Width = 0;
                        int  vTop ;
                        int vLeft=100;
                        Int16 VHeight = 20;
                        vTop = 0;
                        for (int i = 0; i < dsFieldList.Tables[0].Rows.Count; i++)
                        {
                            vTop = vTop + 10+VHeight ;
                            vCaption = dsFieldList.Tables[0].Rows[i]["caption"].ToString().Trim();
                            vToolTip = dsFieldList.Tables[0].Rows[i]["tooltip"].ToString().Trim();
                            vMandatory = Convert.ToBoolean(dsFieldList.Tables[0].Rows[i]["mandatory"]) ;
                            vFieldName = dsFieldList.Tables[0].Rows[i]["fieldname"].ToString().Trim();
                            vDataType = dsFieldList.Tables[0].Rows[i]["datatype"].ToString().Trim();
                            vSize = Convert.ToInt16(dsFieldList.Tables[0].Rows[i]["size"]);
                            vDecimal = Convert.ToInt16(dsFieldList.Tables[0].Rows[i]["decimal"]);
                            vSearchField = Convert.ToBoolean(dsFieldList.Tables[0].Rows[i]["SearchField"]);
                            vInputMask = dsFieldList.Tables[0].Rows[i]["inputmask"].ToString().Trim();
                            vHelpQuery = dsFieldList.Tables[0].Rows[i]["HelpQuery"].ToString().Trim();
                            vReadonly = dsFieldList.Tables[0].Rows[i]["WhenCondition"].ToString().Trim();
                            vDefaulValue = dsFieldList.Tables[0].Rows[i]["DefaultValue"].ToString().Trim();
                            vValidation = dsFieldList.Tables[0].Rows[i]["Validation"].ToString().Trim();
                            vInternalUse = Convert.ToBoolean(dsFieldList.Tables[0].Rows[i]["InternalUse"]); ;
                            vErroMsg = dsFieldList.Tables[0].Rows[i]["Val_Error"].ToString().Trim();
                            vLineCount = Convert.ToInt16(dsFieldList.Tables[0].Rows[i]["LineCount"]);
                            vIsVisible = (dsFieldList.Tables[0].Rows[i]["internaluse"].ToString() == "False" ? true : false);
                            vMField = Convert.ToBoolean(dsFieldList.Tables[0].Rows[i]["mainfield"]);
                            if (vMField)
                            {
                                vMainField = vFieldName;
                                VmainFieldCaption = vCaption;
                            }

                            if (vSearchField == true || vMField==true )
                            {
                                if (string.IsNullOrEmpty(vSearchFields))
                                {
                                    vSearchFields = vFieldName;
                                    vSearchFieldsCap = vFieldName + ":" + vCaption;
                                }
                                else
                                {
                                    vSearchFields = vSearchFields+","+vFieldName;
                                    vSearchFieldsCap = vSearchFieldsCap+","+vFieldName + ":" + vCaption;
                                }
                            }
                            Label labelCaption = new Label();
                            Label lblMandatory = new Label();
                            labelCaption.Name = vFieldName  + i;
                            labelCaption.Tag = vDataType;
                            labelCaption.AutoSize = true;
                            labelCaption.Font = new Font(labelCaption.Font, FontStyle.Regular);
                            labelCaption.Size = new System.Drawing.Size(MaxCtrlWidth, 22);
                            if (vLineCount <= 0) { vLineCount = 1; }
                            labelCaption.Top = vTop;
                            labelCaption.Left = vLeft;
                            //labelCaption.Location = new Point(100, 25 + i * 30);
                            if (vIsVisible)
                            {
                                labelCaption.Text = vCaption ;
                                lblMandatory.ForeColor = Color.Red;
                                lblMandatory.Text = "*";
                                lblMandatory.Name = vFieldName ;
                                lblMandatory.Tag = vDataType ;
                                lblMandatory.Width = 15;
                                lblMandatory.AutoSize = true;
                                //lblMandatory.Location = new Point(labelCaption.Right - 40, 25 + i * 30);
                                lblMandatory.Left = labelCaption.Right - 40;
                                lblMandatory.Top = labelCaption.Top + 2;
                                tempTabPage.Controls.Add(lblMandatory);
                                //tableLayoutPanel1.Controls.Add(lblMandatory, 1, i);
                            }
                            else
                            {
                                labelCaption.Text = vCaption;
                            }
                            labelCaption.Visible = vIsVisible;
                            lblSearchField.Location = new Point(0, 0);

                            tempTabPage.Controls.Add(lblSearchField);
                            tempTabPage.Controls.Add(labelCaption);

                            switch (vDataType.ToUpper())
                            {
                                case "VARCHAR":
                                    uTextBox.cTextBox txtBox = new uTextBox.cTextBox();
                                    txtBox.pCaption = vCaption;
                                    txtBox.pToolTip = vToolTip;
                                    txtBox.pMandatory = vMandatory;
                                    txtBox.Name = vFieldName;
                                    txtBox.pDataType = vDataType;
                                    txtBox.pInputMask = vInputMask;
                                    txtBox.pHelpQuery = vHelpQuery;
                                    txtBox.pReadOnly = vReadonly;
                                    txtBox.pDefaultValue = vDefaulValue;
                                    txtBox.pValidation = vValidation;
                                    txtBox.pInternalUse = vInternalUse;
                                    txtBox.pErrorMsg = vErroMsg;
                                    txtBox.pHelpQuery = vHelpQuery;
                                    //txtBox.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
                                    txtBox.Top = labelCaption.Top;
                                    txtBox.Left = labelCaption.Location.X + 130;
                                    if (vLineCount > 1)
                                    {
                                        txtBox.Multiline = true;
                                        txtBox.ScrollBars = ScrollBars.Vertical;
                                        txtBox.Height = VHeight * vLineCount;
                                        vTop = vTop + (VHeight * (vLineCount - 1));
                                    }
                                    txtBox.Tag = vDataType;
                                    txtBox.Text =vDefaulValue ; //???
                                    txtBox.Width = MaxTxtWidth;
                                    txtBox.Visible = vIsVisible;
                                    txtBox.ReadOnly = true;
                                    txtBox.MaxLength = vSize  ;
                                    txtBox.Leave += new EventHandler(txtBox_Leave);
                                    txtBox.KeyUp += new KeyEventHandler (utextBox_KeyDown);
                                    myTooltip.SetToolTip(txtBox, vToolTip ); /*Added Rup 07/03/2011*/
                                    if (vMandatory ==true )
                                    {
                                        txtBox.Validating += new CancelEventHandler(textbox_Validating);
                                    }
                                    tempTabPage.Controls.Add(txtBox);
                                    if (string.IsNullOrEmpty(vHelpQuery)==false || vSearchField==true ||vMField==true  )
                                    {
                                        uHelpButton.cHelpButton HelpBtn = new uHelpButton.cHelpButton();
                                        HelpBtn.Visible = true;
                                        HelpBtn.Name = "hpc" + vFieldName;
                                        HelpBtn.pCaption = vCaption;
                                        HelpBtn.Tag = vDataType;
                                        string vfname = this.pAppPath + @"\bmp\" + "loc-on.gif";
                                        if (File.Exists(vfname))
                                        {
                                            HelpBtn.Image=Image.FromFile(vfname);
                                        }
                                        HelpBtn.Size = new System.Drawing.Size(22, 22);
                                        HelpBtn.Click += new EventHandler(HelpBtn_Click);
                                        HelpBtn.Location = new Point(txtBox.Location.X + txtBox.Width + 2, txtBox.Location.Y);
                                        if (string.IsNullOrEmpty(vHelpQuery) == false)
                                        {
                                            HelpBtn.pHelpQuery = vHelpQuery;
                                        }
                                        if (vSearchField ||vMField )
                                        {
                                            HelpBtn.pSearchFieldQuery = vFieldName;
                                        }
                                        tempTabPage.Controls.Add(HelpBtn);
                                    }
                                    if (vSearchField)
                                    {

                                        
                                        //_UniqueColName = fieldName;
                                        //_UniqueColumns.Add(fieldName);
                                        //_UniqueColumnCaptions.Add(fieldCaption);
                                        //Button b1 = new Button();
                                        //b1.Name = fieldName;
                                        //b1.Tag = fieldCaption;// _HelpQuery;
                                        //b1.Image = global::DynamicMaster.Properties.Resources.search_icon;
                                        //b1.Size = new System.Drawing.Size(25, 19);
                                        //b1.Location = new Point(txtBox.Location.X + txtBox.Width + 2, txtBox.Location.Y);
                                        //b1.Click += new EventHandler(myButton_Click);
                                        //tempTabPage.Controls.Add(b1);
                                    }
                                    //
                                    break;
                                case "DECIMAL":
                                    uTextBox.cTextBox txtDecimal = new uTextBox.cTextBox();
                                    txtDecimal.pCaption = vCaption;
                                    txtDecimal.pToolTip = vToolTip;
                                    txtDecimal.pMandatory = vMandatory;
                                    txtDecimal.Name = vFieldName;
                                    txtDecimal.pDataType = vDataType;
                                    txtDecimal.pInputMask = vInputMask;
                                    txtDecimal.pHelpQuery = vHelpQuery;
                                    txtDecimal.pReadOnly = vReadonly;
                                    txtDecimal.pDefaultValue = vDefaulValue;
                                    txtDecimal.pValidation = vValidation;
                                    txtDecimal.pInternalUse = vInternalUse;
                                    txtDecimal.pErrorMsg = vErroMsg;

                                    txtDecimal.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
                                    txtDecimal.Tag = vDataType;
                                    txtDecimal.Text = vDefaulValue; //???
                                    txtDecimal.Width = MaxCtrlWidth;
                                    txtDecimal.Visible = vIsVisible;
                                    txtDecimal.ReadOnly = true;
                                    txtDecimal.Leave += new EventHandler(txtBox_Leave);
                                    myTooltip.SetToolTip(txtDecimal, vToolTip); /*Added Rup 07/03/2011*/
                                    txtDecimal.KeyPress += new KeyPressEventHandler(OnKeyPress);
                                    txtDecimal.TextChanged += new EventHandler(OnTextChanged);
                                    if (vMandatory == true)
                                    {
                                        txtDecimal.Validating += new CancelEventHandler(textbox_Validating);
                                    }
                                    tempTabPage.Controls.Add(txtDecimal);
                                    break;
                                case "BIT":
                                    //Add Checkbox/Radiobutton
                                    //CheckBox chk1 = new CheckBox();
                                    uCheckBox.cCheckBox chk1 = new uCheckBox.cCheckBox();
                                    chk1.pCaption = vCaption;
                                    chk1.pToolTip = vToolTip;
                                    chk1.pMandatory = vMandatory;
                                    chk1.Name = vFieldName;
                                    chk1.pDataType = vDataType;
                                    chk1.pInputMask = vInputMask;
                                    chk1.pHelpQuery = vHelpQuery;
                                    chk1.pReadOnly = vReadonly;
                                    chk1.pDefaultValue = vDefaulValue;
                                    chk1.pValidation = vValidation;
                                    chk1.pInternalUse = vInternalUse;
                                    chk1.pErrorMsg = vErroMsg;
                                    chk1.Tag = vDataType;
                                    chk1.Location = new Point(labelCaption.Location.X + 130, labelCaption.Location.Y);
                                    //chk1.Location = new System.Drawing.Point(XPos, YPos);
                                    chk1.Visible = vIsVisible;
                                    chk1.Enabled = false;
                                    myTooltip.SetToolTip(chk1, vToolTip );
                                    tempTabPage.Controls.Add(chk1);
                                    //tableLayoutPanel1.Controls.Add(chk1, 2, i);
                                    break;
                            }
                           
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,this.pPApplText);
            }
            finally 
            {

            }
        }

        void txtBox_Leave(object sender, EventArgs e)
        {
            //TextBox txtName = (TextBox)sender;
            //string tempStr = string.Empty;
            ////MessageBox.Show(txtName.Text + " " + txtName.Tag);
            ////throw new NotImplementedException();
            //int currPosition = this.BindingContext[dsFormInfo, "FormInfo"].Position;
            //if (!txtName.ReadOnly)
            //{
            //    try
            //    {
            //        if (validations.Count > 0)
            //        {
            //            MessageBox.Show(validations[txtName.Name]);
            //            tempStr = validations[txtName.Name];
            //            if (tempStr.StartsWith("do "))
            //            {
            //                //udValidation.clsValidation oVal = new udValidation.clsValidation();
            //                //oVal.FunctionName = "Val2";
            //                //MessageBox.Show(oVal.validateExpression().ToString());
            //            }
            //            else
            //            {
            //                //if (Convert.ToBoolean(tempStr))
            //                //{ }
            //                OdeToCode.Utility.Evaluator myEvaluator = new OdeToCode.Utility.Evaluator();
            //                OdeToCode.Utility.Evaluator.EvalToInteger(tempStr);
            //            }
            //        }
            //    }
            //    catch (KeyNotFoundException ex)
            //    {
            //        //MessageBox.Show(ex.Message);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
        }
        private void HelpBtn_Click(object sender, EventArgs e)
        {
            string vHelpQuery = string.Empty;
            string VForText = string.Empty;
            string cntName = string.Empty;
            if (sender.GetType().ToString().ToLower() == "utextbox.ctextbox")
            {
                vHelpQuery = ((uTextBox.cTextBox)sender).pHelpQuery;
                VForText = "Select " + ((uTextBox.cTextBox)sender).pCaption;
                cntName = ((uTextBox.cTextBox)sender).Name;
            }
            else
            {
                vHelpQuery = ((uHelpButton.cHelpButton)sender).pHelpQuery;
                VForText ="Select "+ ((uHelpButton.cHelpButton)sender).pCaption;
                cntName = ((uHelpButton.cHelpButton)sender).Name;
                cntName = cntName.Substring(3, cntName.Length - 3);
            }
            int pos1 = 0, pos2 = 0;
            string vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            if (this.pAddMode || this.pEditMode)
            {

                pos1 = vHelpQuery.IndexOf("{");
                if (pos1 > 1)
                {
                    strSQL = vHelpQuery.Substring(0, pos1 - 1);
                    Vstr = vHelpQuery.Substring(pos1 + 1, vHelpQuery.Length - pos1 - 2);
                    pos2 = Vstr.IndexOf("#");
                    vSearchCol = Vstr.Substring(0, pos2);
                    Vstr = Vstr.Substring(pos2 + 1, Vstr.Length - pos2 - 1);
                    pos2 = vSearchCol.IndexOf("+");
                    if (pos2 > 0)
                    {
                        vSearchCol = vSearchCol.Substring(0,pos2);
                    }


                    pos2 = Vstr.IndexOf("#");
                    vReturnCol = Vstr.Substring(0, pos2);
                    Vstr = Vstr.Substring(pos2 + 1, Vstr.Length - pos2 - 1);
                    pos2 = Vstr.IndexOf("#");
                    vDisplayColumnList = Vstr.Substring(0, pos2);
                    Vstr = Vstr.Substring(pos2 + 1, Vstr.Length - pos2 - 1);
                    vDisplayColumnList = Vstr;
                }
                else
                {
                    strSQL = vHelpQuery;
                    vSearchCol = "";
                    vReturnCol = "";
                    vDisplayColumnList = "";
                }
             
            }
            else
            {
                if (string.IsNullOrEmpty(vSearchFields) == false)
                {
                    strSQL = "Select " + vSearchFields + " from " + _TableName + " order by " + vSearchFields;
                    vSearchCol = vSearchFields;
                    pos2 = vSearchFields.IndexOf(",");
                    if (pos2>0)
                    {
                        vSearchCol = vSearchFields.Substring(0, pos2);
                    }
                    vReturnCol = vSearchCol;
                    vDisplayColumnList = vSearchFieldsCap;
                }
                
            }
            
            DataSet dstemp = new DataSet();
            dstemp=oDataAccess.GetDataSet(strSQL, null, 20);
            DataView dvw = dstemp.Tables[0].DefaultView;            

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            //string vFieldList = _UniqueColName + ":" + _UniqueColName.ToUpper();
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            ////oSelectPop.pSetCtlRef = this.textBoxCaption;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                if (this.pAddMode || this.pEditMode)
                {
                    dsMain.Tables[0].Rows[0][cntName] = oSelectPop.pReturnArray[0];
                    object objects = this;
                    mthRemoveControlBind(GetTabControl(objects));
                    mthControlBind(GetTabControl(objects));
                    this.Refresh();
                    return;
                }
                string vFldVal = string.Empty, sqlstr = string.Empty;
                vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
                sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " where " + vMainField + "='" + vMainFldVal + "' order by  " + vMainField + " desc";
                dsMain.Tables.Clear();
                dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);

                this.mthSubView();
                //this.Controls[cntName].Text = oSelectPop.pReturnArray[0];
                //Navigation Button//
                
                DataSet dsTemp = new DataSet();

                vMainFldVal = oSelectPop.pReturnArray[0];
                vFldVal = vMainFldVal;
                sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "'";
                dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                    {
                        this.btnFirst.Enabled = true;
                        this.btnBack.Enabled = true;
                    }
                    else
                    {
                        this.btnFirst.Enabled = false;
                        this.btnBack.Enabled = false;
                    }
                }
                vFldVal = vMainFldVal;
                sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "'";
                dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    else
                    {
                        this.btnForward.Enabled = false;
                        this.btnLast.Enabled = false;
                    }
                }
                this.mthSubView();
                
                

                HandleButtons(true, true, false, false, true);

               
                
                //this.Controls[cntName].Text = oSelectPop.pReturnArray[0];
            }



        }
        void textbox_Validating(object sender, CancelEventArgs e)
        {
            uTextBox.cTextBox txtName = (uTextBox.cTextBox)sender;
            //TextBox txtName = (TextBox)sender;
            if (!txtName.ReadOnly)
            {
                
                if (txtName.Text.Trim() == "" && txtName.pMandatory==true && txtName.pDataType.ToUpper() =="VARCHAR")
                {

                    ToolTip t = new ToolTip();
                    string ErrMsg = t.GetToolTip(txtName).ToString();
                    errorProvider.SetError(txtName,"Could not allow blank "+txtName.pCaption+" value");
                    txtName.Focus();
                    vValid = false;
                    e.Cancel = true;
                    return;
                }
                if (txtName.pMandatory == true && txtName.pDataType.ToUpper() =="DECIMAL")
                {
                    if (txtName.Text.Trim() == "")
                    {
                        ToolTip t = new ToolTip();
                        string ErrMsg = t.GetToolTip(txtName).ToString();
                        errorProvider.SetError(txtName, "Could not allow blank " + txtName.pCaption + " value");
                        txtName.Focus();
                        vValid = false;
                        e.Cancel = true;
                        return;
                    }
                    if (Convert.ToDecimal(txtName.Text) == 0)
                    {
                        ToolTip t = new ToolTip();
                        string ErrMsg = t.GetToolTip(txtName).ToString();
                        errorProvider.SetError(txtName, "Could not Zero " + txtName.pCaption + " value");
                        txtName.Focus();
                        vValid = false;
                        e.Cancel = true;
                        return;
                    }
                }
                if (txtName.Name == vMainField )
                {
                    DataSet dsData = new DataSet();
                    string sqlstr;
                    sqlstr = " select " + vMainField + " from " + _TableName  + " where "+vMainField+ "='"+txtName.Text.Trim()+"'";
                    if (this.pEditMode )
                    {
                        sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
                    }
                    dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        ToolTip t = new ToolTip();
                        string ErrMsg = t.GetToolTip(txtName).ToString();
                        errorProvider.SetError(txtName, "Duplicate " + txtName.pCaption + " value");
                        txtName.Focus();
                        vValid = false;
                        e.Cancel = true;
                        return;
                    }
                }
                errorProvider.SetError(txtName, "");
            }
           
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Console.WriteLine(ProcessCommand("1+1").ToString()); //Displays 2
            Console.WriteLine(ProcessCommand("Math.PI").ToString()); //Displays 3.14159265358979
            Console.WriteLine(ProcessCommand("Math.Abs(-22)").ToString()); //Displays 22
            Console.WriteLine(ProcessCommand("3-4+6+7+22/3+66*(55)").ToString()); //Displays 3649 
        }
        
        /// <summary>
        /// A simple function to get the result of a C# expression (basic and advanced math possible)
        /// </summary>
        /// <param name="command">String value containing an expression that can evaluate to a double.</param>
        /// <returns>a Double value after evaluating the command string.</returns>
        private double ProcessCommand(string command)
        {
            //Create a C# Code Provider
            CSharpCodeProvider myCodeProvider = new CSharpCodeProvider();
            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;//No need to make an EXE file here.
            cp.GenerateInMemory = true;   //But we do need one in memory.
            cp.OutputAssembly = "TempModule"; //This is not necessary, however, if used repeatedly, causes the CLR to not need to 
            //load a new assembly each time the function is run.
            //The below string is basically the shell of a C# program, that does nothing, but contains an
            //Evaluate() method for our purposes.  I realize this leaves the app open to injection attacks, 
            //But this is a simple demonstration.
            string TempModuleSource = "namespace ns{" +
                                      "using System;" +
                                      "class class1{" +
                                      "public static double Evaluate(){return " + command + ";}}} ";  //Our actual Expression evaluator

            CompilerResults cr = myCodeProvider.CompileAssemblyFromSource(cp, TempModuleSource);
            if (cr.Errors.Count > 0)
            {
                //If a compiler error is generated, we will throw an exception because 
                //the syntax was wrong - again, this is left up to the implementer to verify syntax before
                //calling the function.  The calling code could trap this in a try loop, and notify a user 
                //the command was not understood, for example.
                throw new ArgumentException("Expression cannot be evaluated, please use a valid C# expression");
            }
            else
            {
                MethodInfo Methinfo = cr.CompiledAssembly.GetType("ns.class1").GetMethod("Evaluate");
                return (double)Methinfo.Invoke(null, null);
            }
        }

        protected void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            uTextBox.cTextBox txtName = (uTextBox.cTextBox)sender;

            //TextBox txtName = (TextBox)sender;
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back
                & e.KeyChar != '.')
            {
                errorProvider.SetError(txtName, "Only numeric values allowed in " + txtName.pCaption);
                //txtName.BackColor = Color.Red;
                txtName.Focus();
                e.Handled = true;
                return;
            }
            errorProvider.SetError(txtName, "");
            base.OnKeyPress(e);
        }

        private string currentText;

        protected void OnTextChanged(object sender, EventArgs e)
        {
            if (this.Text.Length > 0)
            {
                float result;
                bool isNumeric = float.TryParse(this.Text, out result);

                if (isNumeric)
                {
                    currentText = this.Text;
                }
                else
                {
                    //this.Text = currentText; /*Commented Rup 07/03/2011*/
                    //this.Select(this.Text.Length, 0);
                }
            }
            base.OnTextChanged(e);
        }

        private void BindData(string tableName)
        {
            DataSet dsData = new DataSet();
            try
            {
                dsData = oForm.GetDataFromSelectedTable(tableName, oDataAccess);

                if (dsData != null)
                {
                    dsFormInfo = dsData;
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        object objects = this;
                        BindControlsToData(GetTabControl(objects));
                        SetBindingContextPosition("Last", 0);
                        HandleButtons(true, true, false, false, true);
                    }
                    else
                    {
                        HandleButtons(true, false, false, false, false);
                        HandleNavigationButtons(false, false, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void mthRemoveControlBind(Control tabCtrl)
        {
            try
            {
                foreach (TabPage tPage in tabCtrl.Controls)
                {
                    foreach (Control childCtrl in tPage.Controls)
                    {
                        switch (childCtrl.GetType().Name.ToLower())
                        {
                            case "ctextbox":
                                childCtrl.DataBindings.Clear();
                                break;
                            case "maskedtextbox":
                                childCtrl.DataBindings.Clear();
                                break;
                            case "datetimepicker":
                                childCtrl.DataBindings.Clear();
                                break;
                            case "checkbox":
                                childCtrl.DataBindings.Clear();
                                break;
                            case "combobox":
                                break;
                            case "label":
                                if (childCtrl.Name == "lblSearchField")
                                {
                                    childCtrl.DataBindings.Clear();
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { }
        }
        private void BindControlsToData(Control tabCtrl)
        {
            try
            {
                foreach (TabPage tPage in tabCtrl.Controls)
                {
                    foreach (Control childCtrl in tPage.Controls)
                    {
                        switch (childCtrl.GetType().Name.ToLower())
                        {
                            case "ctextbox":
                                childCtrl.DataBindings.Clear();
                                childCtrl.DataBindings.Add("Text", dsFormInfo, "FormInfo." + childCtrl.Name.ToString());
                                break;
                            case "maskedtextbox":
                                childCtrl.DataBindings.Clear();
                                childCtrl.DataBindings.Add("Text", dsFormInfo, "FormInfo." + childCtrl.Name.ToString());
                                break;
                            case "datetimepicker":
                                childCtrl.DataBindings.Clear();
                                DateTimePicker m = (DateTimePicker)childCtrl;
                                Binding b = new Binding("Text", dsFormInfo, "FormInfo." + childCtrl.Name.ToString());
                                //b.FormatString = "MM/dd/yyyy";
                                m.DataBindings.Add("Text", dsFormInfo, "FormInfo." + childCtrl.Name.ToString());
                                break;
                            case "checkbox":
                                childCtrl.DataBindings.Clear();
                                childCtrl.DataBindings.Add("Checked", dsFormInfo, "FormInfo." + childCtrl.Name.ToString());
                                break;
                            case "combobox":
                                break;
                            case "label":
                                if (childCtrl.Name == "lblSearchField")
                                {
                                    childCtrl.DataBindings.Clear();
                                    childCtrl.DataBindings.Add("Text", dsFormInfo, "FormInfo.ID");
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { }
        }

        #endregion

        #region Save/Update/Cancel

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            this.mthNew();
            HandleNavigationButtons(false, false, false, false);
            HandleButtons(false, false, true, true, false);

            
            ////_Mode = FormMode.New;
            //this.pAddMode = true;
            //this.pEditMode = false;

            //object objects = this;
            //EnableDiableFormControls(GetTabControl(objects), true, true);
            //this.BindingContext[dsFormInfo, "FormInfo"].AddNew();
            //HandleNavigationButtons(false, false, false, false);
            //HandleButtons(false, false, true, true, false);
        }
        private  void mthNew()
        {
            this.pAddMode = true;
            this.pEditMode = false;

            object objects = this;
            EnableDiableFormControls(GetTabControl(objects), true, true);
            mthRemoveControlBind(GetTabControl(objects));
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                dsMain.Tables[0].Rows.RemoveAt(0);
                dsMain.Tables[0].AcceptChanges();
            }
            DataRow drCurrent;
            drCurrent = dsMain.Tables[0].NewRow();
            dsMain.Tables[0].Rows.Add(drCurrent);

         
            mthControlBind(GetTabControl(objects));
            EnableDiableFormControls(GetTabControl(objects), false, true);
            EnableDiableFormControls(GetTabControl(objects), true, true);
            dsMain.Tables[0].Rows[0].BeginEdit();

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }

            this.pAddMode = false ;
            this.pEditMode = true;

            dsMain.Tables[0].Rows[0].BeginEdit();

            
            //_Mode = FormMode.Edit;
            object objects = this;
            EnableDiableFormControls(GetTabControl(objects), true, true);
            HandleNavigationButtons(false, false, false, false);
            HandleButtons(false, false, true, true, false);
        }
        public void mthSave()
        {


            dsMain.Tables[0].Rows[0].EndEdit();
            //if (this.pAddMode == true)
            //{
            //    DataRow drc = dsMain.Tables[0].NewRow();
            //    drc[vMainField] = dsMain.Tables[0].Rows[0][vMainField];
            //    dsMain.Tables[0].Rows.Add(drc);
            //    dsMain.Tables[0].AcceptChanges();
            //}
            if (this.pEditMode == true)
            {
            }

            string vSaveString = string.Empty;
            this.mSaveCommandString(ref vSaveString, "#ID#");
            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
            this.pAddMode = false;
            this.pEditMode = false;
            this.pEditMode = false;
            object objects = this;
            EnableDiableFormControls(GetTabControl(objects), false, false);

            if (dsFormInfo.Tables[0].Rows.Count > 0)
            {
                HandleButtons(true, true, false, false, true);
            }
            else
            {
                HandleButtons(true, true, false, false, false);
            }


            this.pAddMode = false;
            this.pEditMode = false;
          
            EnableDiableFormControls(GetTabControl(objects),false, true);
            mthRemoveControlBind(GetTabControl(objects));

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            vValid = true;
            TextBox t1 = new TextBox();
            Boolean RecordValidation = true;
            t1.Visible = true;
            this.Controls.Add(t1);
            object objects = this;
            t1.Focus();
            if (vValid == false)
            {
                return;
            }
            string vFldVal = string.Empty, sqlstr = string.Empty;
            string vSaveString = string.Empty;
            //if (string.IsNullOrEmpty(this.dsMain.Tables[0].Rows[0][vMainField].ToString())==true)
            //{
            //    MessageBox.Show(this.VmainFieldCaption.Trim() + " Field Could not be Blank", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            
            mCheckMendatroyValues (GetTabControl(objects));
            if (vValid == false)
            {
                return;
            }
            this.mSaveCommandString(ref vSaveString, "#ID#");
            this.pAddMode = false;
            this.pEditMode = false;
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            EnableDiableFormControls(GetTabControl(objects), false, false);

            oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);

            
            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
            sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " where " + vMainField + "='" + vMainFldVal + "' order by  " + vMainField + " desc";
            dsMain.Tables.Clear();
            dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
           
            this.mthSubView();


            //Navigation Button//
            
            DataSet dsTemp = new DataSet();

            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
            vFldVal = vMainFldVal;
            sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "'";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            //if (dsMainm.Tables[0].Rows.Count > 0)
            //{
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                {
                    this.btnFirst.Enabled = true;
                    this.btnBack.Enabled = true;
                }
                else
                {
                    this.btnFirst.Enabled = false;
                    this.btnBack.Enabled = false;
                }
            //}
            vFldVal = vMainFldVal;
            sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "'";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            //if (dsMainm.Tables[0].Rows.Count > 0)
            //{
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                {
                    this.btnForward.Enabled = true;
                    this.btnLast.Enabled = true;
                }
                else
                {
                    this.btnForward.Enabled = false;
                    this.btnLast.Enabled = false;
                }
            //}
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            HandleButtons(true, true, false, false, true);

            //if (dsMain.Tables[0].Rows.Count > 0)
            //{
            //    HandleButtons(true, true, false, false, true);
            //}
            //else
            //{
            //    HandleButtons(true, true, false, false, false);
            //}

           // Control ctrl = GetTabControl(objects);
            //string vSaveString=string.Empty;
            //this.mSaveCommandString( ref vSaveString,"#ID#");
            //this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            //
            //Control ctrl = GetTabControl(objects);
            //SaveDetails(ctrl);
            //this.pAddMode = false;
            //this.pEditMode = false;
            ////EnableDiableFormControls(GetTabControl(objects), false);
            ////HandleButtons(true, true, false, false);
        }
        private void mSaveCommandString(ref string vSaveString,string vkeyField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields=string.Empty,vfldVal=string.Empty,vDataType = string.Empty;
            
            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr ="select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + _TableName + "'";
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#"+dr1["ColName"].ToString().Trim()+"#";
            }
            
            /*<---Identity Columns--->*/
            if (this.pAddMode == true )
            {
               
                vSaveString = " insert into " + this._TableName;
                dsMain.Tables[0].AcceptChanges();
                foreach (DataColumn dtc1 in dsMain.Tables[0].Columns)
                {

                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim()+"#")<0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + dtc1.ToString().Trim();
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();

                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        vfldValList = vfldValList + vfldVal;
                    }
                }

                vSaveString = vSaveString + " (" + vfldList + ") Values (" + vfldValList + ")";
            }
            if (this.pEditMode  == true)
            {
                vSaveString = " Update " + this._TableName+" Set ";
                string vWhereCondn=string.Empty;
                foreach (DataColumn dtc1 in dsMain.Tables[0].Columns)
                {

                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vkeyField.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        vfldList = vfldList+ dtc1.ToString().Trim()+" = ";
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        vfldList = vfldList + vfldVal;
                        //if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                    }
                    if (vkeyField.IndexOf("#" + dtc1.ToString().Trim() + "#") > -1)
                    {
                        if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                        vWhereCondn = vWhereCondn+dtc1.ToString().Trim()+" = ";
                        vfldVal = dsMain.Tables[0].Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        vWhereCondn = vWhereCondn + vfldVal;
                    }
                }
                vSaveString = vSaveString + vfldList + vWhereCondn;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            object objects = this;
            //errorProvider.SetError(txtName, "");
            errorProvider.Clear();
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            EnableDiableFormControls(GetTabControl(objects), false, false);
            
            if (this.dsMain.Tables[0].Rows.Count == 1)
            {
                dsMain.Tables[0].Rows[0].CancelEdit();
                if (this.pAddMode)
                {
                    dsMain.Tables[0].Rows[0].Delete();
                }
                mthRemoveControlBind(GetTabControl(objects));
                this.mthSubView();
                errorProvider.Clear();
            }
            string vFldVal = string.Empty, sqlstr = string.Empty;
            this.pAddMode = false;
            this.pEditMode = false;
           
            
            HandleButtons(true, true, false, false, true);
            //if (this.dsMain.Tables[0].Rows.Count == 1)
            //{
            //    dsMain.Tables[0].Rows[0].CancelEdit();
            //    if (this.pAddMode)
            //    {
            //        dsMain.Tables[0].Rows[0].Delete();
            //    }
            //    mthRemoveControlBind(GetTabControl(objects));
            //    this.mthSubView();
            //}

            if (this.dsMain.Tables[0].Rows.Count <= 1)
            {
                return;
            }
            this.mthSubView();
            dsMain.Tables[0].Rows[0].CancelEdit();
            
            //Navigation Button//

            DataSet dsTemp = new DataSet();

            vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
            vFldVal = vMainFldVal;
            sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "'";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            //if (dsMainm.Tables[0].Rows.Count > 0)
            //{
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                {
                    this.btnFirst.Enabled = true;
                    this.btnBack.Enabled = true;
                }
                else
                {
                    this.btnFirst.Enabled = false;
                    this.btnBack.Enabled = false;
                }
            //}
            vFldVal = vMainFldVal;
            sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "'";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            //if (dsMainm.Tables[0].Rows.Count > 0)
            //{
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                {
                    this.btnForward.Enabled = true;
                    this.btnLast.Enabled = true;
                }
                else
                {
                    this.btnForward.Enabled = false;
                    this.btnLast.Enabled = false;
                }
            //}
           

        }

        private Control GetTabControl(object Objects)
        {
            Dictionary<Control, string> found = new Dictionary<Control, string>();
            Queue<Control> controlQueue = new Queue<Control>();
            controlQueue.Enqueue(Objects as Control);
            Control tabCtrl = new Control();

            while (controlQueue.Count > 0)
            {
                Control item = controlQueue.Dequeue();
                foreach (Control ctrl in item.Controls)
                {
                    if (ctrl.GetType().Name == "TabControl")
                    {
                        tabCtrl = ctrl;
                    }
                }
            }
            return tabCtrl;
        }
        private void mCheckMendatroyValues(Control tabPage)
        {
            CancelEventArgs ec = new CancelEventArgs();
            foreach (TabPage tPage in tabPage.Controls)
            {
                foreach (Control childCtrl in tPage.Controls)
                {
                    if (vValid == false)
                    {
                        return;
                    }
                    switch (childCtrl.GetType().Name.ToLower())
                    {

                        case "ctextbox":
                            uTextBox.cTextBox MyTextBox=(uTextBox.cTextBox)childCtrl;
                            if (MyTextBox.pMandatory)
                            {
                                SetFocusToControl(childCtrl, childCtrl.Name);
                                textbox_Validating(childCtrl, ec);
                                if (vValid == false)
                                {
                                    return;
                                }
                                //txtBox_Leave(childCtrl, ex);
                            }
                            break;//DateTimePicker
                    }
                }
            }
        }
        private void EnableDiableFormControls(Control tabPage, bool enabled, bool setFocus)
        {
            foreach (TabPage tPage in tabPage.Controls)
            {
                foreach (Control childCtrl in tPage.Controls)
                {
                    
                    switch (childCtrl.GetType().Name.ToLower())
                    {
                        
                        case "ctextbox":
                            TextBox MyTextBox = (TextBox)childCtrl;
                            MyTextBox.ReadOnly = !enabled;
                            if (setFocus)
                                SetFocusToControl(childCtrl, childCtrl.Name);
                            break;
                        case "maskedtextbox":
                            MaskedTextBox MyTextBox1 = (MaskedTextBox)childCtrl;
                            if (MyTextBox1.Tag.ToString() == "Datetime")
                                MyTextBox1.Mask = "####.##";
                            MyTextBox1.ReadOnly = !enabled;
                            if (setFocus)
                                SetFocusToControl(childCtrl, childCtrl.Name);
                            break;
                        case "datetimepicker":
                            DateTimePicker dtPicker = (DateTimePicker)childCtrl;
                            dtPicker.Enabled = enabled;
                            if (setFocus)
                                SetFocusToControl(childCtrl, childCtrl.Name);
                            break;
                        case "checkbox":
                            childCtrl.Enabled = enabled;
                            if (setFocus)
                                if (setFocus)
                                    SetFocusToControl(childCtrl, childCtrl.Name);
                            break;
                        case "combobox":
                            childCtrl.Enabled = enabled;
                            SetFocusToControl(childCtrl, childCtrl.Name);
                            break;
                        case "button":
                            if (childCtrl.Name == "buttonPopup")
                            {
                                childCtrl.Enabled = !enabled;
                            }
                            break;
                        case "chelpbutton":
                            childCtrl.Enabled = false;
                            if (childCtrl.Name.Substring(0, 3) == "hpc")
                            {
                                if (string.IsNullOrEmpty( ((uHelpButton.cHelpButton)childCtrl).pSearchFieldQuery) ==false )
                                { 
                                    if (this.pAddMode==false  && this.pEditMode==false )
                                    {
                                        childCtrl.Enabled =true  ;
                                    }
                                }
                                if (string.IsNullOrEmpty(((uHelpButton.cHelpButton)childCtrl).pHelpQuery) == false)
                                {
                                    if (this.pAddMode || this.pEditMode)
                                    {
                                        childCtrl.Enabled = true ;
                                    }
                                }
                            }
                            break;//DateTimePicker
                    }
                }
            }
        }

        private void SetFocusToControl(Control ctrl, string controlName)
        {
            //if (dsFieldInfo.Tables[0].Rows[0]["CAPTION"].ToString() == controlName)
            if (dsFieldInfo.Tables[0].Rows[0]["FieldName"].ToString() == controlName)
            {
                ctrl.Focus();
            }
        }

        private void SaveDetails(Control tabPage)
        {
            List<clsParam> colParams = new List<clsParam>();
            List<clsParam> whereColParams = new List<clsParam>();
            clsParam objParam;
            clsParam.pType paramType = clsParam.pType.pString;
            int id = 0;
            int SearchFieldID = 0;

            try
            {
                foreach (TabPage tPage in tabPage.Controls)
                {
                    foreach (Control childCtrl in tPage.Controls)
                    {
                        
                        switch (childCtrl.Tag.ToString().ToLower())
                        {
                            case "varchar":
                                paramType = clsParam.pType.pString;
                                break;
                            case "decimal":
                                paramType = clsParam.pType.pFloat;
                                break;
                            case "bit":
                                paramType = clsParam.pType.pLong;
                                break;
                            case "datetime":
                                paramType = clsParam.pType.pDate;
                                break;
                            case "text":
                                //paramType = clsParam.pType.pOracleCLOB;
                                paramType = clsParam.pType.pString;
                                break;
                        }
                        
                        switch (childCtrl.GetType().Name.ToLower())
                        {
                               
                            case "ctextbox":
                                objParam = new clsParam(childCtrl.Name, childCtrl.Text,
                                            paramType, colParams, false, null,
                                            clsParam.pInOut.pIn);
                                break;
                            case "maskedtextbox":
                                objParam = new clsParam(childCtrl.Name, Convert.ToDecimal(childCtrl.Text),
                                            paramType, colParams, false, null,
                                            clsParam.pInOut.pIn);
                                break;
                            case "datetimepicker":
                                objParam = new clsParam(childCtrl.Name, childCtrl.Text,
                                            paramType, colParams, false, null,
                                            clsParam.pInOut.pIn);
                                break;
                            case "ccheckbox":
                                CheckBox chkBox = (CheckBox)childCtrl;
                                objParam = new clsParam(childCtrl.Name, chkBox.Checked, paramType, colParams, false, null,
                                            clsParam.pInOut.pIn);
                                break;
                            case "combobox":
                                objParam = new clsParam(childCtrl.Name, childCtrl.Text,
                                            paramType, colParams, false, null,
                                            clsParam.pInOut.pIn);
                                break;
                            case "label":
                                if (childCtrl.Name == "lblSearchField")
                                {
                                    if (this.pAddMode)
                                        SearchFieldID = Convert.ToInt32(childCtrl.Text);
                                }
                                break;
                        }
                    }
                }
                if (this.pAddMode)
                {
                    id = oForm.SaveDynamicFormData(_TableName, colParams, oDataAccess);
                    BindData(_TableName);
                    //*** DON'T DELETE THE BELOW LINE -- 9/2/2011
                    //HandleButtons(true, true, false, false, true);
                }
                else
                {
                    id = oForm.UpdateDynamicFormData(_TableName, colParams, oDataAccess, SearchFieldID);
                    SetBindingContextPosition("SelectedRecord", this.BindingContext[dsFormInfo, "FormInfo"].Position);
                }
                if (id > 0)
                {
                    object objects = this;
                    EnableDiableFormControls(GetTabControl(objects), false, false);
                    //*** DON'T DELETE THE BELOW LINE -- 9/2/2011
                    HandleButtons(true, true, false, false, true);
                    MessageBox.Show("Entry Saved", this.pPApplText,MessageBoxButtons.OK,MessageBoxIcon.Information );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReloadFormInfo()
        {
            DataSet dsData = new DataSet();

            dsData = oForm.GetDataFromSelectedTable(_TableName, oDataAccess);
            if (dsData != null)
            {
                if (dsData.Tables[0].Rows.Count > 0)
                {
                    dsFormInfo = dsData;
                }
                //else
                //{
                //    HandleNavigationButtons(false, false, false, false);
                //}
            }
        }

        #endregion

        #region Navigation

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            DataSet dsTemp=new DataSet();
            string sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0][vMainField].ToString()) == false)
                {
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
                }
            }
            this.mthSubView();
            sqlstr = "select cnt=count(id) from " + this._TableName +" Where "+vMainField+"<>''"; 
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled=false;
            
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 1)
                {
                    this.btnForward.Enabled = true ;
                    this.btnLast.Enabled = true;
                }
                else
                {
                    this.btnForward.Enabled = false;
                    this.btnLast.Enabled = false; 
                }

            }
            this.mthSubView();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            DataSet dsTemp = new DataSet();
            string vFldVal = vMainFldVal;
            string sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "' order by " + vMainField + " desc";
            dsMain.Tables.Clear();
            dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0][vMainField].ToString()) == false)
                {
                    if (vFldVal != dsMain.Tables[0].Rows[0][vMainField].ToString())
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
                    }
                    //vMainFldVal = dsMainm.Tables[0].Rows[0][vMainField].ToString();
                }
            }
            vFldVal = vMainFldVal;
            sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "'";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                {
                    this.btnFirst.Enabled = true;
                    this.btnBack.Enabled = true;
                }
                else
                {
                    this.btnFirst.Enabled = false ;
                    this.btnBack.Enabled = false ;
                }
            }
            this.mthSubView();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            DataSet dsTemp = new DataSet();
            string vFldVal = vMainFldVal;
            string sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "' order by " + vMainField ;
            dsMain.Tables.Clear();
            dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0][vMainField].ToString()) == false)
                {
                    if (vFldVal != dsMain.Tables[0].Rows[0][vMainField].ToString())
                    {
                        this.btnFirst.Enabled = true;
                        this.btnBack.Enabled = true;
                        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
                    }
                    //vMainFldVal = dsMainm.Tables[0].Rows[0][vMainField].ToString();
                }
            }
            vFldVal = vMainFldVal;
            sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "'";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
                {
                    this.btnForward.Enabled = true;
                    this.btnLast.Enabled = true;
                }
                else
                {
                    this.btnForward.Enabled = false;
                    this.btnLast.Enabled = false;
                }
            }
            this.mthSubView();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            DataSet dsTemp = new DataSet();
            string sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " order by  " + vMainField+" desc";
            dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsMain.Tables[0].Rows[0][vMainField].ToString()) == false)
                {
                    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
                }
            }
            this.mthSubView();
            sqlstr = "select cnt=count(id) from " + this._TableName + " Where " + vMainField + "<>''";
            dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            this.btnForward.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 1)
                {
                    this.btnFirst.Enabled = true;
                    this.btnBack.Enabled = true;
                }
            }
            this.mthSubView();
        }

        private void SetBindingContextPosition(string bindingMode, int newPosition)
        {
            int recCount = this.BindingContext[dsFormInfo, "FormInfo"].Count;
            switch (bindingMode)
            {
                case "First":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = 0;
                    HandleNavigationButtons(false, false, true, true);
                    break;
                case "Previous":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = this.BindingContext[dsFormInfo, "FormInfo"].Position - 1;
                    if (this.BindingContext[dsFormInfo, "FormInfo"].Position == 0)
                        HandleNavigationButtons(false, false, true, true);
                    else
                        HandleNavigationButtons(true, true, true, true);
                    break;
                case "Next":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = this.BindingContext[dsFormInfo, "FormInfo"].Position + 1;
                    if (this.BindingContext[dsFormInfo, "FormInfo"].Position == recCount - 1)
                        HandleNavigationButtons(true, true, false, false);
                    else
                        HandleNavigationButtons(true, true, true, true);
                    break;
                case "Last":
                    this.BindingContext[dsFormInfo, "FormInfo"].Position = recCount;
                    if (recCount == 0 || recCount == 1)
                        HandleNavigationButtons(false, false, false, false);
                    else
                        HandleNavigationButtons(true, true, false, false);
                    break;
                case "SelectedRecord":
                    //if (this.BindingContext[dsFormInfo, "FormInfo"].Position != newRowIndex)
                    //this.BindingContext[dsFormInfo, "FormInfo"].Position = newRowIndex;
                    if (newPosition == 0)
                        SetBindingContextPosition("First", 0);
                    else if (newPosition + 1 == recCount)
                        SetBindingContextPosition("Last", 0);
                    else
                    {
                        this.BindingContext[dsFormInfo, "FormInfo"].Position = newPosition;
                        HandleNavigationButtons(true, true, true, true);
                    }
                    break;
            }
        }

        private int GetSelectedRowIndex(int selectedValue)
        {
            //DataView dv = new DataView(dsFormInfo.Tables[0]);
            //string filter = "ID=" + selectedValue;

            //dv.RowFilter = filter;

            //if(dv.Count == 1)
            //    return dv[0].Row[0]

            //return 1;
            int popupValue = 0;
            for (int i = 0; i < dsFormInfo.Tables[0].Rows.Count; i++)
            {
                popupValue = Convert.ToInt32(dsFormInfo.Tables[0].Rows[i]["ID"]);
                if (popupValue == selectedValue)
                    return i;
            }
            return popupValue;
        }

        private void HandleNavigationButtons(bool first, bool prev, bool next, bool last)
        {
            btnFirst.Enabled = first;
            btnBack.Enabled = prev;
            btnForward.Enabled = next;
            btnLast.Enabled = last;
        }

        private void HandleButtons(bool newButton, bool editButton, bool cancelButton, bool saveButton, bool deleteButton)
        {
            btnNew.Visible = newButton;
            btnNew.Enabled = newButton;
            btnEdit.Visible = editButton;
            btnEdit.Enabled = editButton;
            btnCancel.Visible = cancelButton;
            btnCancel.Enabled = cancelButton;
            btnSave.Visible = saveButton;
            btnSave.Enabled = saveButton;
            btnDelete.Enabled = deleteButton;
        }

        #endregion

        #region Select Popup

        private void myButton_Click(object sender, EventArgs e)
        {
            //DataSet dsData = new DataSet();
            //DataView dvw = new DataView();
            //string colList = string.Empty;
            //string uniqueCols = string.Empty;
            //try
            //{
            //    Button b = (Button)sender;
            //    if (_UniqueColumns.Count > 0)
            //    {
            //        for (int i = 0; i < _UniqueColumns.Count; i++)
            //        {
            //            colList += _UniqueColumns[i].ToString() + ",";
            //            uniqueCols += _UniqueColumns[i].ToString() + ":" + _UniqueColumnCaptions[i].ToString().ToUpper() + ",";
            //        }
            //        colList = colList.Substring(0, colList.ToString().Length - 1);
            //        uniqueCols = uniqueCols.Substring(0, uniqueCols.ToString().Length - 1);
            //    }
            //    string selectstring = "SELECT ID, " + colList + " FROM " + _TableName;
            //    dsData = oDataAccess.GetDataSet(selectstring, null, 20);
            //    dvw = dsData.Tables[0].DefaultView;

            //    udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            //    oSelectPop.pdataview = dvw;
            //    oSelectPop.pformtext = "Popup Form for - " + b.Name;// _UniqueColName;
            //    oSelectPop.psearchcol = b.Name;//_UniqueColName;
            //    string vFieldList = _UniqueColName + ":" + _UniqueColName.ToUpper();
            //    oSelectPop.pDisplayColumnList = uniqueCols;// vFieldList;
            //    oSelectPop.pRetcolList = "ID," + b.Name;// _UniqueColName;
            //    //oSelectPop.pSetCtlRef = this.textBoxCaption;
            //    oSelectPop.ShowDialog();

            //    if (oSelectPop.pReturnArray != null)
            //    {
            //        SetBindingContextPosition("SelectedRecord", GetSelectedRowIndex(Convert.ToInt32(oSelectPop.pReturnArray[0])));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    //dsData = nuul;
            //}
        }

        #endregion

        #region Shortcut Keys

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(this.btnExit, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        #endregion

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
            if (MessageBox.Show("Are you sure you wish to delete this Entry ?", this.pPApplText,
                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.pAddMode = false;
                this.pEditMode = false;

                string vDelString = string.Empty;
                vDelString = "Delete from " + _TableName + " Where ID=" + dsMain.Tables[0].Rows[0]["id"];
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                this.dsMain.Tables[0].Rows[0].Delete();
                this.dsMain.Tables[0].AcceptChanges();

                //Navigation Button//
                string vFldVal = string.Empty, sqlstr = string.Empty;
                DataSet dsTemp = new DataSet();

                
                vFldVal = vMainFldVal;
                vMainFldVal = "";
                sqlstr = "select  " + vMainField + " from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "'";
                dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);

                //if (dsMain.Tables[0].Rows.Count > 0)
                //{
                    if (Convert.ToInt32(dsTemp.Tables[0].Rows.Count) > 0)
                    {
                        vMainFldVal = dsTemp.Tables[0].Rows[0][vMainField].ToString();
                    }
                    if (Convert.ToInt32(dsTemp.Tables[0].Rows.Count) > 1)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    else
                    {
                        this.btnForward.Enabled = false;
                        this.btnLast.Enabled = false;
                    }
                //}

                if (string.IsNullOrEmpty(vMainFldVal)==false)
                {
                    vFldVal = vMainFldVal;
                }
                sqlstr = "select " + vMainField + " from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "' order by " + vMainField + " desc";
                dsTemp.Tables.Clear();
                dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
                //if (dsMain.Tables[0].Rows.Count > 0)
                //{
                    if (Convert.ToInt32(dsTemp.Tables[0].Rows.Count) > 0)
                    {
                        if (string.IsNullOrEmpty(vMainFldVal))
                        {
                            vMainFldVal = dsTemp.Tables[0].Rows[0][vMainField].ToString();
                            if (Convert.ToInt32(dsTemp.Tables[0].Rows.Count) > 1)
                            {
                                this.btnFirst.Enabled = true;
                                this.btnBack.Enabled = true;
                            }
                            else
                            {
                                this.btnFirst.Enabled = false;
                                this.btnBack.Enabled = false;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dsTemp.Tables[0].Rows.Count) > 0)
                            {
                                this.btnFirst.Enabled = true;
                                this.btnBack.Enabled = true;
                            }
                            else
                            {
                                this.btnFirst.Enabled = false;
                                this.btnBack.Enabled = false;
                            }
                        }

                    }
                //}
                
                this.mthSubView();

                object objects = this;
                //mthRemoveControlBind(GetTabControl(objects));
                //mthControlBind(GetTabControl(objects));
                this.EnableDiableFormControls(GetTabControl(objects),false,true);/*Added Rup 07/03/2011*/
            }
            
           
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();/*Added Rup 07/03/2011*/
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            this.mcheckCallingApplication();/*Added Rup 07/03/2011*/
        }
        public string PMasterCode/*Added Rup 07/03/2011*/
        {
            set
            {
                _MasterCode  = value;
            }
            get
            {
                return _MasterCode;
            }
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.pAddMode || this.pEditMode)
            {
                if (MessageBox.Show("Do you want to save your changes first ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.btnSave_Click(sender, e);
                }
                else
                {
                    this.pAddMode = false;
                    this.pEditMode = false;
                    Application.Exit();
                }
            }
        }
        private void utextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string vHelpQuery = ((uTextBox.cTextBox)sender).pHelpQuery;
            if ((this.pAddMode || this.pEditMode)  && e.KeyCode.ToString() == "F2" && string.IsNullOrEmpty(vHelpQuery)==false)
            { 
                EventArgs ex=new EventArgs();
                this.HelpBtn_Click(sender, e);
            }
        }

        private void MasterForm_KeyUp(object sender, KeyEventArgs e)
        {
            //e.KeyCode.ToString();
        }

        private void MasterForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control == true)
            {
                EventArgs exf = new EventArgs();
                if (e.KeyCode == Keys.D)
                {
                    this.btnDelete_Click(sender, exf);
                }
                if (e.KeyCode == Keys.E)
                {
                    this.btnEdit_Click (sender, exf);
                }
                if (e.KeyCode == Keys.F4)
                {
                    this.btnExit_Click(sender, exf);
                }

            }
            //MessageBox.Show(e.KeyCode.ToString());
        }

        private void MasterForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        

       

        #region "Find Control Sample Code"

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    TimeSpan datedays = endDT - startDT;
        //    int days = datedays.Days;
        //    int subnum = 0;
        //    for (int i = 0; i <= days; i++)
        //    {
        //        int num1 = Convert.ToInt32(((TextBox)Page.FindControl("OTTextBox" + i)).Text);
        //        int num2 = Convert.ToInt32(((TextBox)Page.FindControl("RegTextBox" + i)).Text);
        //        ((Label)Page.FindControl("TotalLabel" + i)).Text = (num1 + num2).ToString();
        //        subnum += num1 + num2;
        //    }
        //    ((Label)Page.FindControl("subTextBox")).Text = subnum.ToString();
        //}

        #endregion

    }

    #region Single Line Flow Layout

    public class SingleLineFlow
    {
        private Control container;
        private int margin;

        public SingleLineFlow(Control parent, int margin)
        {
            this.container = parent;
            this.margin = margin;

            // Attach the event handler.
            container.Layout += new LayoutEventHandler(UpdateLayout);

            // Refresh the layout.
            UpdateLayout(this, null);
        }

        public int Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
            }
        }

        // This is public so it can be triggered manually if needed.
        public void UpdateLayout(object sender,
            System.Windows.Forms.LayoutEventArgs e)
        {
            int y = 0;
            foreach (Control ctrl in container.Controls)
            {
                y += Margin;
                ctrl.Left = Margin;
                ctrl.Top = y;
                ctrl.Width = 150;// container.Width;
                ctrl.Height = Margin;
            }
        }

    }

    #endregion

}