using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;
using uBaseForm;
using udSelectPop;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Diagnostics;
using ueTblFieldsSave;
using System.Globalization;
using System.Threading;
using udclsDGVDateTimePicker;

namespace ueTblFields
{
    public partial class frmFields : uBaseForm.FrmBaseForm
    {
        string modName, fldList,gridedit;
        
        string[] words;
        string[] key;
        //string getcoltype;
        DataAccess_Net.clsDataAccess oDataAccess;
        string[] colname;
        string[] colcaption;
        string[] editable;
        string[] Bulkupdatecol;
        bool IsExitCalled;
        string where;
       // DataSet ds1;
        int count = 0;
        int topval = 0;
        //string collist,colnames;
        string table;
        //string helpword;
        string helpqry;
        //bool ishelp;
        int n;
        DataTable dt = new DataTable();
        DataSet coltype = new DataSet(); /*Ramya 18/02/13*/

        string[] help;
        string appPath;
        String cAppPId, cAppName;
        short vTimeOut = 25;  /*Ramya 18/02/13*/
        string colums;
        public frmFields(string[] args)
        {
            InitializeComponent();
            //this.pFrmCaption = "Table Field Details";
            this.pFrmCaption = args[13];
            this.Text = this.pFrmCaption;
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pAppUerName = args[5];
            modName = args[13].Replace("<*#*>", " ");
            this.pPApplRange = args[12];
            this.pFrmCaption = modName;
            Icon MainIcon = new System.Drawing.Icon(args[6].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[7].Replace("<*#*>", " ");
            this.pPara = args;
            this.pPApplName = args[8];
            this.pPApplPID = Convert.ToInt16(args[9]);
            this.pPApplCode = args[10];

            /*Ramya 18/02/13*/
            //DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            //DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            //DataAccess_Net.clsDataAccess._userID = this.pUserId;
            //DataAccess_Net.clsDataAccess._password = this.pPassword;
            //oDataAccess = new DataAccess_Net.clsDataAccess();
            /*Ramya 18/02/13*/

            // this.mInsertProcessIdRecord(); /*Ramya
            //this.SetFormColor();
        }
  

        private void frmFields_Load(object sender, EventArgs e)
        {
          
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

         
            this.SetMenuRights();

            this.mInsertProcessIdRecord();
            this.SetFormColor();

            appPath = Application.ExecutablePath;

            txtModule.ReadOnly = true;
            txtTable.ReadOnly = true;
            mcheckCallingApplication();  /*Ramya 18/02/13 */

            DataSet dsMain = oDataAccess.GetDataSet("select * from TableUpdate_Master where module_name='"+modName+"'", null, 20);  //TableUpdate_Master
            LoadData(dsMain);

        }
        public void LoadData(DataSet dsGetData)
        {
         
             foreach (DataRow dr in dsGetData.Tables[0].Rows)
            {
                modName = dr["Module_Name"].ToString();
                fldList = dr["TableFieldList"].ToString();
                //gridedit = dr["GridEdit"].ToString();
           
            }

          
            


             char[] arr = new char[] { '<', '>' };
             string strpart = fldList.Trim(arr);
             words = strpart.Split('#');
             table = words[0];
             words = words[1].Split(',');
             n = words.Length;
             colname = new string[words.Length];
             colcaption = new string[words.Length];
             editable = new string[words.Length];
             Bulkupdatecol = new string[words.Length];
             key = new string[words.Length];
             help = new string[words.Length];

            ////getcoltype = "SELECT column_name,data_type,character_maximum_length,numeric_precision,numeric_scale FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table + "' order by column_name";
            ////ds1 = oDataAccess.GetDataSet(getcoltype, null, 20);

           #region t o split columns 

             ////for (int i = 0; i < words.Length; i++)
             ////{


             ////   string strcol = words[i];
             ////   string[] splwords;


             ////   ishelp = false;
             ////   splwords = strcol.Split(':');
             ////  int length = splwords.Length;
             ////   colname[i] = splwords[0].Trim();
             ////   colcaption[i] = splwords[1];
            ////    if (strcol.Contains("HelpQuery"))
            ////    {
            ////        helpqry = splwords[length - 1];
            ////        ishelp = true;

            ////        help[i] = "true";
            ////    }


            ////    if (length >= 3)
            ////        editable[i] = splwords[2];
            ////    else
            ////        editable[i] = " ";
            ////    if (length >= 4)
            ////    {
            ////        Bulkupdatecol[i] = splwords[3];
            ////        count++;
            ////    }
            ////    else
            ////        Bulkupdatecol[i] = " ";
            ////               if (i == 0)
            ////                {
            ////                    collist = "[Select]=cast(0 as bit)," + colname[0];
            ////                    colnames = "'" + colname[0] + "'";
            ////                }
            ////                else
            ////                {
            ////                    collist = collist + ',' + colname[i];
            ////                    colnames = colnames + ',' + "'" + colname[i] + "'";
             ////                }


            ////    if (ishelp)
            ////    {
            ////      string  helpqry1 = "select distinct " + colname[i] + "from ";
            ////    }


            ////    #region add label,checkbox,textbox for BulkUpdate
            ////    if (gridedit.ToLower() == "false")
            ////   {

            ////    ////    if (Bulkupdatecol[i] == "BulkUpdate")
            ////    ////    {

            ////    ////        int num = (count + 3) % 4;
            ////    ////        if (num == 0)
            ////    ////        {
            ////    ////            count = 1;
            ////    ////            topval += 1;
            ////    ////        }

            ////    ////        //  MessageBox.Show("msg3");
            ////    ////        Label lbl = new Label();
            ////    ////        lbl.Text = colcaption[i];
            ////    ////        //lbl.Location = new Point(lblTable.Location.X + 25, lblTable.Location.Y + 50);
            ////    ////        lbl.Name = colname[i].Trim();
            ////    ////        lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
            ////    ////        //lbl.Top = lblTable.Top * topval + 20;
            ////    ////        lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
            ////    ////        lbl.Size = new Size(100, 15);
            ////    ////        Controls.Add(lbl);

            ////    ////        CheckBox chk = new CheckBox();
            ////    ////        chk.Name = "chk" + colname[i].Trim();
            ////    ////        chk.Left = lbl.Left - 20;
            ////    ////        chk.Top = lbl.Top + 15;
            ////    ////        chk.Size = new Size(15, txtTable.Size.Height);
            ////    ////        chk.Click += new EventHandler(chk_Click);
            ////    ////        Controls.Add(chk);



            ////    ////        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
            ////    ////        {
            ////    ////            //if (ds1.Tables[0].Rows[j][0].ToString().ToLower() == colname[i].ToLower().Trim())
            ////    ////            if (ds1.Tables[0].Rows[j][0].ToString() == colname[i].Trim())
            ////    ////            {
            ////    ////                string datatype = ds1.Tables[0].Rows[j][1].ToString();
            ////    ////                string maxlen = ds1.Tables[0].Rows[j][2].ToString();
            ////    ////                string precision = ds1.Tables[0].Rows[j][3].ToString();
            ////    ////                string scale = ds1.Tables[0].Rows[j][4].ToString();
            ////    ////                string mask;
            ////    ////                string prelen;
            ////    ////                string scalen;
            ////    ////                if (datatype == "bit")
            ////    ////                {
            ////    ////                    CheckBox txtval = new CheckBox();
            ////    ////                    txtval.Name = "txt" + colname[i].Trim();
            ////    ////                    txtval.Left = lbl.Left;
            ////    ////                    txtval.Top = lbl.Top + 13;
            ////    ////                    //txtval.ReadOnly = true;
            ////    ////                    txtval.Enabled = false;
            ////    ////                    //txtval.Enabled = true;
            ////    ////                    Controls.Add(txtval);
            ////    ////                }
            ////    ////                //else if(dat
            ////    ////                else
            ////    ////                {
            ////    ////                    //TextBox txtval = new TextBox();
            ////    ////                    MaskedTextBox txtval = new MaskedTextBox();
            ////    ////                    txtval.Name = "txt" + colname[i].Trim();
            ////    ////                    txtval.Left = lbl.Left;
            ////    ////                    txtval.Top = lbl.Top + 15;
            ////    ////                    txtval.ReadOnly = true;
            ////    ////                    txtval.Enabled = true;
            ////    ////                    // txtval.KeyPress+=new KeyPressEventHandler(txtval_KeyPress);
            ////    ////                    txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);

            ////    ////                    switch (datatype.ToLower())
            ////    ////                    {
            ////    ////                        case "int":
            ////    ////                            mask = "0";
            ////    ////                            for (int k = 1; k < Convert.ToInt16(precision); k++)
            ////    ////                            {
            ////    ////                                mask += "0";
            ////    ////                            }
            ////    ////                            txtval.Mask = "#" + mask;
            ////    ////                            break;
            ////    ////                        case "varchar":
            ////    ////                            mask = "a";
            ////    ////                            for (int k = 1; k < Convert.ToInt16(maxlen); k++)
            ////    ////                            {
            ////    ////                                mask += "a";
            ////    ////                            }
            ////    ////                            txtval.Mask = mask;
            ////    ////                            if (ishelp)
            ////    ////                            {

            ////    ////                                Button btnval = new Button();
            ////    ////                                btnval.Name = "btn" + colname[i].Trim();
            ////    ////                                btnval.Left = txtval.Right + 10;
            ////    ////                                btnval.Top = txtval.Top - 3;
            ////    ////                                btnval.Size = new Size(25, 25);
            ////    ////                                btnval.Text = "";
            ////    ////                                btnval.Click += new EventHandler(btnval_Click);
            ////    ////                                Controls.Add(btnval);
            ////    ////                                string fName = appPath + @"\bmp\loc-on.gif";
            ////    ////                                if (File.Exists(fName) == true)
            ////    ////                                {
            ////    ////                                    btnval.Image = Image.FromFile(fName);
            ////    ////                                    // this.btnValidity.Image = Image.FromFile(fName);
            ////    ////                                }
            ////    ////                            }



            ////    ////                            break;
            ////    ////                        case "char":
            ////    ////                            mask = "C";
            ////    ////                            for (int k = 1; k < Convert.ToInt16(maxlen); k++)
            ////    ////                            {
            ////    ////                                mask += "C";
            ////    ////                            }
            ////    ////                            txtval.Mask = mask;
            ////    ////                            break;
            ////    ////                        case "datetime":
            ////    ////                            txtval.Mask = "00/00/0000";
            ////    ////                            txtval.ValidatingType = typeof(System.DateTime);
            ////    ////                            //txtval.TypeValidationCompleted += new TypeValidationEventHandler(txtval_TypeValidationCompleted);
            ////    ////                            break;
            ////    ////                        case "smalldatetime":
            ////    ////                            txtval.Mask = "00/00/0000";
            ////    ////                            txtval.ValidatingType = typeof(System.DateTime);
            ////    ////                            //txtval.TypeValidationCompleted += new TypeValidationEventHandler(txtval_TypeValidationCompleted);
            ////    ////                            break;
            ////    ////                        case "numeric":
            ////    ////                            prelen = "0";
            ////    ////                            scalen = "0";
            ////    ////                            for (int k = 1; k < (Convert.ToInt16(precision) - Convert.ToInt16(scale)); k++)
            ////    ////                            {
            ////    ////                                prelen += "0";
            ////    ////                            }
            ////    ////                            for (int q = 1; q < Convert.ToInt16(scale); q++)
            ////    ////                            {
            ////    ////                                scalen += "0";
            ////    ////                            }
            ////    ////                            mask = "#" + prelen + "." + scalen;
            ////    ////                            txtval.Mask = mask;
            ////    ////                            //txtval.ValidatingType = typeof(System.Int32);
            ////    ////                            break;



            ////    ////                        case "decimal":
            ////    ////                            prelen = "0";
            ////    ////                            scalen = "0";
            ////    ////                            for (int k = 1; k < (Convert.ToInt16(precision) - Convert.ToInt16(scale)); k++)
            ////    ////                            {
            ////    ////                                prelen += "0";
            ////    ////                            }
            ////    ////                            for (int q = 1; q < Convert.ToInt16(scale); q++)
            ////    ////                            {
            ////    ////                                scalen += "0";
            ////    ////                            }
            ////    ////                            mask = "#" + prelen + "." + scalen;
            ////    ////                            txtval.Mask = mask;
            ////    ////                            //txtval.MaxLength = 10;
            ////    ////                            txtval.ValidatingType = typeof(System.Decimal);
            ////    ////                            //txtval.TextLength = 10;

            ////    ////                            break;


            ////    ////                    }
            ////    ////                    Controls.Add(txtval);

            ////    ////                }

            ////    ////            }
            ////    ////        }



          ////     }

            ////    #endregion

            ////       // //dataGridView1.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 90;
            ////    }
            ////    else
            ////    {
            ////    }

            ////}

            #endregion

          

            string SqlStr = "Execute [Usp_Ent_BulkUpdate_Get_ColumnsList] '" + modName + "'";
            coltype = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);
            colums = "[Select]=cast(0 as bit)";

            foreach (DataRow dr in coltype.Tables[0].Rows)
            {
                colums = colums +',' +dr["fldnm"].ToString().Trim();
            }

            string str = "Select " + colums + " from " + table;
            dt = oDataAccess.GetDataTable(str, null, 20);

            this.mthBindData();
            txtTable.Text = table;
            txtModule.Text = modName;
            
           // dataGridView1.DataSource = dt;
            //for (int j = 1; j < dt.Columns.Count; j++)
            for (int j = 1; j < dt.Columns.Count; j++)
            {
              
                ////for (int i = 0; i < colcaption.Length - 1; i++)                 /*Ramya 19/02/13*/
                ////{
                ////    if ("col_" + colname[i] == dataGridView1.Columns[j].Name.ToString())
                ////    {
                ////        dataGridView1.Columns[j].HeaderText = colcaption[i];
                ////    }
                ////}              
                
                ////dataGridView1.Columns[j].ReadOnly = true;
                ////if (editable[j - 1] == "Editable")
                ////{
                ////  //dataGridView1.Columns[j].HeaderCell.Style.Font = new Font(this.Font, FontStyle.Underline);
               
                ////   if (help[j - 1] == "true")
                ////      dataGridView1.Columns[j].ReadOnly = true;
                ////    else
                ////     dataGridView1.Columns[j].ReadOnly = false;
               
                ////}
               
                ////if (editable[j - 1] == "KeyField")
                ////{
                ////    key[j - 1] = colname[j - 1];
                    
                ////}
                ////else
                ////{
                ////    key[j - 1] = string.Empty;
                ////}
                //if (colcaption[j - 1] == "NotVisible")
                //{
                //    str = "col_" + colname[j - 1];
                //    dataGridView1.Columns[str].Visible = false;
                //}
                try
                {
                    if (dt.Columns[j].DataType.ToString() == "System.DateTime")
                    {
                        this.dataGridView1.Columns[j].DefaultCellStyle.Format = "dd/MM/yyyy";  
                    }
                }
                catch
                {
                    MessageBox.Show("Please Provide correct datetime format");
                    return;
                }


                
            }
            
        }

        private void mthBindData()
        {
            this.dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dt;

            this.dataGridView1.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn ColSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColSel.HeaderText = "Select";
            ColSel.Name = "colSel";
            ColSel.ReadOnly = false;
            this.dataGridView1.Columns.Add(ColSel);
            this.dataGridView1.Columns["colSel"].DataPropertyName = "select";

            //string SqlStr = "select fldnm=clm.[name],fldtype=typ.name ,tblnm=sobj.name,clm.Length,clm.xprec,clm.xscale from syscolumns clm inner join sysobjects sobj on (sobj.id=clm.id) inner join systypes typ on (clm.xtype=typ.xtype) where sobj.[name] like '" + table + "' and sobj.xtype='U' and clm.[Name] in (" + colnames + ")";
            //coltype = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

               //string SqlStr ="Execute [Usp_Ent_BulkUpdate_Get_ColumnsList] '"+modName+"'";
               //coltype = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

               count = 0;
            int num1;
            string mask;
            string prelen;
            string scalen;
            foreach(DataRow dr in coltype.Tables[0].Rows)
            {
                switch (dr["fldtype"].ToString().ToLower() )
                {
                    case "int":
                        udclsDGVNumericColumn.CNumEditDataGridViewColumn colint = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                        colint.Name = "col_" + dr["fldnm"].ToString();
                        colint.HeaderText = dr["value"].ToString();
                        colint.DecimalLength = 0;
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            colint.Visible = false;
                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            colint.ReadOnly = false;
                        }
                        else
                        {
                            colint.ReadOnly = true;
                        }
                        colint.MaxInputLength = Convert.ToInt32(dr["xprec"].ToString());
                                                
                        this.dataGridView1.Columns.Add(colint);
                        this.dataGridView1.Columns[colint.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        colint.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colint.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[colint.Name].Width = 50;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim()== Convert.ToString(1))
                        {

                            colint.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);

                            
                            MaskedTextBox txtval = new MaskedTextBox();
                            txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            txtval.Left = lbl.Left;
                            txtval.Top = lbl.Top + 15;
                            txtval.ReadOnly = true;
                            txtval.Enabled = true;
                            
                            // txtval.KeyPress+=new KeyPressEventHandler(txtval_KeyPress);
                            txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);

                            mask = "0";
                            for (int k = 1; k < Convert.ToInt16(dr["xprec"].ToString()); k++)
                            {
                                mask += "0";
                            }
                            txtval.Mask = "#" + mask;
                            Controls.Add(txtval);


                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = txtval.Right + 10;
                                btnval.Top = txtval.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }

                        }
                        #endregion

                        break;

                    

                    case "decimal":
                        udclsDGVNumericColumn.CNumEditDataGridViewColumn coldecimal = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                        coldecimal.Name = "col_" + dr["fldnm"].ToString();
                        coldecimal.HeaderText = dr["value"].ToString();
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            coldecimal.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            coldecimal.ReadOnly = false;
                        }
                        else
                        {
                            coldecimal.ReadOnly = true;
                        }
                        coldecimal.MaxInputLength = Convert.ToInt32(dr["xprec"].ToString());
                        coldecimal.DecimalLength = Convert.ToInt32(dr["xscale"].ToString());
                        this.dataGridView1.Columns.Add(coldecimal);
                        this.dataGridView1.Columns[coldecimal.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        coldecimal.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //coldecimal.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[coldecimal.Name].Width = 100;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            coldecimal.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);

                                                    

                            MaskedTextBox txtval = new MaskedTextBox();
                            txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            txtval.Left = lbl.Left;
                            txtval.Top = lbl.Top + 15;
                            txtval.ReadOnly = true;
                            txtval.Enabled = true;                           


                            prelen = "0";
                            scalen = "0";
                            for (int k = 1; k < (Convert.ToInt16(dr["xprec"].ToString()) - Convert.ToInt16(dr["xscale"].ToString())); k++)
                            {
                                prelen += "0";
                            }
                            for (int q = 1; q < Convert.ToInt16(dr["xscale"].ToString()); q++)
                            {
                                scalen += "0";
                            }
                            mask = "#" + prelen + "." + scalen;
                            txtval.Mask = mask;
                           
                            txtval.ValidatingType = typeof(System.Decimal);
                            txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);
                            Controls.Add(txtval);

                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = txtval.Right + 10;
                                btnval.Top = txtval.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }

                            
                        }
                        #endregion

                        break;




                    case "bit ":
                        udclsDGVNumericColumn.CNumEditDataGridViewColumn colbit = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                        colbit.Name = "col_" + dr["fldnm"].ToString();
                        colbit.HeaderText = dr["value"].ToString();
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            colbit.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            colbit.ReadOnly = false;
                        }
                        else
                        {
                            colbit.ReadOnly = true;
                        }
                       
                        colbit.MaxInputLength = 1;
                        colbit.DecimalLength = 0;
                        this.dataGridView1.Columns.Add(colbit);
                        this.dataGridView1.Columns[colbit.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        colbit.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colbit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[colbit.Name].Width = 25;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            colbit.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);


                            ////MaskedTextBox txtval = new MaskedTextBox();
                            ////txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            ////txtval.Left = lbl.Left;
                            ////txtval.Top = lbl.Top + 15;
                            ////txtval.ReadOnly = true;
                            ////txtval.Enabled = true;

                            ////// txtval.KeyPress+=new KeyPressEventHandler(txtval_KeyPress);
                            ////txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);

                            ////mask = "0";                            
                            ////txtval.Mask = "#" + mask;
                            ////Controls.Add(txtval);


                            CheckBox txtval = new CheckBox();
                            txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            txtval.Left = lbl.Left;
                            txtval.Top = lbl.Top + 13;
                            //txtval.ReadOnly = true;
                            txtval.Enabled = false;
                            //txtval.Enabled = true;
                            Controls.Add(txtval);

                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = txtval.Right + 10;
                                btnval.Top = txtval.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }
                        }
                        #endregion

                        break;
                    case "numeric":
                        udclsDGVNumericColumn.CNumEditDataGridViewColumn colnumeric = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                        colnumeric.Name = "col_" + dr["fldnm"].ToString();
                        colnumeric.HeaderText = dr["value"].ToString();
                       
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            colnumeric.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            colnumeric.ReadOnly = false;
                        }
                        else
                        {
                            colnumeric.ReadOnly = true;
                        }
                        colnumeric.MaxInputLength = Convert.ToInt32(dr["xprec"].ToString());
                        colnumeric.DecimalLength = Convert.ToInt32(dr["xscale"].ToString());
                        this.dataGridView1.Columns.Add(colnumeric);
                        this.dataGridView1.Columns[colnumeric.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        colnumeric.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colnumeric.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[colnumeric.Name].Width = 100;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            colnumeric.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);



                            MaskedTextBox txtval = new MaskedTextBox();
                            txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            txtval.Left = lbl.Left;
                            txtval.Top = lbl.Top + 15;
                            txtval.ReadOnly = true;
                            txtval.Enabled = true;


                            prelen = "0";
                            scalen = "0";
                            for (int k = 1; k < (Convert.ToInt16(dr["xprec"].ToString()) - Convert.ToInt16(dr["xscale"].ToString())); k++)
                            {
                                prelen += "0";
                            }
                            for (int q = 1; q < Convert.ToInt16(dr["xscale"].ToString()); q++)
                            {
                                scalen += "0";
                            }
                            mask = "#" + prelen + "." + scalen;
                            txtval.Mask = mask;

                            txtval.ValidatingType = typeof(System.Decimal);
                            txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);
                            Controls.Add(txtval);

                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = txtval.Right + 10;
                                btnval.Top = txtval.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }


                        }
                        #endregion

                        break;
                    case "varchar":
                        
                        System.Windows.Forms.DataGridViewTextBoxColumn colvarchar = new System.Windows.Forms.DataGridViewTextBoxColumn();
                        colvarchar.Name = "col_" + dr["fldnm"].ToString();
                        colvarchar.HeaderText = dr["value"].ToString();
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            colvarchar.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            colvarchar.ReadOnly = false;
                        }
                        else
                        {
                            colvarchar.ReadOnly = true;
                        }
                        //colEmp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colEmp.DefaultCellStyle.BackColor = Color.LightGray;
                        colvarchar.MaxInputLength = Convert.ToInt32(dr["Length"].ToString());
                        this.dataGridView1.Columns.Add(colvarchar);
                        this.dataGridView1.Columns[colvarchar.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        colvarchar.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                      //  colvarchar.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                       
                       // this.dataGridView1.Columns[colvarchar.Name].Width = 150;

                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            colvarchar.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);


                            MaskedTextBox txtval = new MaskedTextBox();
                            txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            txtval.Left = lbl.Left;
                            txtval.Top = lbl.Top + 15;
                            txtval.ReadOnly = true;
                            txtval.Enabled = true;

                            // txtval.KeyPress+=new KeyPressEventHandler(txtval_KeyPress);
                            txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);

                            mask = "a";
                            for (int k = 1; k < Convert.ToInt16(dr["length"].ToString()); k++)
                            {
                                mask += "a";
                            }
                            txtval.Mask =  mask;
                            Controls.Add(txtval);

                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = txtval.Right + 10;
                                btnval.Top = txtval.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }
                        }
                        #endregion

                        break;


                    case "char":

                        System.Windows.Forms.DataGridViewTextBoxColumn colchar = new System.Windows.Forms.DataGridViewTextBoxColumn();
                        colchar.Name = "col_" + dr["fldnm"].ToString();
                        colchar.HeaderText = dr["value"].ToString();
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            colchar.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            colchar.ReadOnly = false;
                        }
                        else
                        {
                            colchar.ReadOnly = true;
                        }
                        //colEmp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colEmp.DefaultCellStyle.BackColor = Color.LightGray;
                        colchar.MaxInputLength = Convert.ToInt32(dr["Length"].ToString());
                        this.dataGridView1.Columns.Add(colchar);
                        this.dataGridView1.Columns[colchar.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        colchar.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colchar.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[colchar.Name].Width = 100;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            colchar.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);


                            MaskedTextBox txtval = new MaskedTextBox();
                            txtval.Name = "txt" + dr["fldnm"].ToString().Trim();
                            txtval.Left = lbl.Left;
                            txtval.Top = lbl.Top + 15;
                            txtval.ReadOnly = true;
                            txtval.Enabled = true;

                            // txtval.KeyPress+=new KeyPressEventHandler(txtval_KeyPress);
                            txtval.Size = new Size(txtTable.Size.Width / 2, txtTable.Size.Height);

                            mask = "C";
                            for (int k = 1; k < Convert.ToInt16(dr["xprec"].ToString()); k++)
                            {
                                mask += "C";
                            }
                            txtval.Mask =  mask;
                            Controls.Add(txtval);

                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = txtval.Right + 10;
                                btnval.Top = txtval.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }
                        }
                        #endregion

                        break;

                    case "datetime":
                        udclsDGVDateTimePicker.MicrosoftDateTimePicker coldatetime = new udclsDGVDateTimePicker.MicrosoftDateTimePicker();
                        coldatetime.Name = "col_" + dr["fldnm"].ToString();
                        coldatetime.HeaderText = dr["value"].ToString();
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            coldatetime.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            coldatetime.ReadOnly = false;
                        }
                        else
                        {
                            coldatetime.ReadOnly = true;
                        }
                        this.dataGridView1.Columns.Add(coldatetime);
                        this.dataGridView1.Columns[coldatetime.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        coldatetime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //coldatetime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[coldatetime.Name].Width = 85;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            coldatetime.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);


                            //MaskedTextBox txtval = new MaskedTextBox();
                            //txtval.Mask = "00/00/0000";
                            //txtval.ValidatingType = typeof(System.DateTime);
                            //Controls.Add(txtval);

                            DateTimePicker dtp = new DateTimePicker();
                            dtp.Name = "txt" + dr["fldnm"].ToString().Trim();
                            dtp.Left = lbl.Left;
                            dtp.Top = lbl.Top + 15;

                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = dtp.Right + 10;
                                btnval.Top = dtp.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);
                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }
                        }
                        #endregion



                       
                        break;
                    case "smalldatetime":

                        udclsDGVDateTimePicker.MicrosoftDateTimePicker colsdatetime = new udclsDGVDateTimePicker.MicrosoftDateTimePicker();
                        colsdatetime.Name = "col_" + dr["fldnm"].ToString();
                        colsdatetime.HeaderText = dr["value"].ToString();
                        
                        if (dr["NotVisible"].ToString().Trim() == Convert.ToString(1))
                        {
                            colsdatetime.Visible = false;

                        }
                        if (dr["Editable"].ToString().Trim() == Convert.ToString(1))
                        {
                            colsdatetime.ReadOnly = false;
                        }
                        else
                        {
                            colsdatetime.ReadOnly = true;
                        }
                        this.dataGridView1.Columns.Add(colsdatetime);
                        this.dataGridView1.Columns[colsdatetime.Name].DataPropertyName = dr["fldnm"].ToString().Trim();
                        colsdatetime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        //colsdatetime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //this.dataGridView1.Columns[colsdatetime.Name].Width = 85;
                        #region to add label checkbox textbox

                        if (dr["BulkUpdate"].ToString().Trim() == Convert.ToString(1))
                        {
                            colsdatetime.ReadOnly = true;
                            count++;
                            num1 = (count + 3) % 4;
                            if (num1 == 0)
                            {
                                count = 1;
                                topval += 1;
                            }

                            Label lbl = new Label();
                            lbl.Text = dr["value"].ToString();
                            lbl.Name = dr["fldnm"].ToString();
                            lbl.Left = lblTable.Left * count + 2 * ((lblTable.Left + 10) * (count - 1)) + 20;
                            lbl.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 30;
                            lbl.Size = new Size(100, 15);
                            Controls.Add(lbl);

                            CheckBox chk = new CheckBox();
                            chk.Name = "chk" + dr["fldnm"].ToString().Trim();
                            chk.Left = lbl.Left - 20;
                            chk.Top = lbl.Top + 15;
                            chk.Size = new Size(15, txtTable.Size.Height);
                            chk.Click += new EventHandler(chk_Click);
                            Controls.Add(chk);


                            DateTimePicker sdtp = new DateTimePicker();
                            sdtp.Name = "txt" + dr["fldnm"].ToString().Trim();
                            sdtp.Left = lbl.Left;
                            sdtp.Top = lbl.Top + 15;


                            if (dr["HelpQuery"].ToString().Trim() != string.Empty)
                            {

                                Button btnval = new Button();
                                btnval.Name = "btn" + dr["fldnm"].ToString().Trim();
                                btnval.Left = sdtp.Right + 10;
                                btnval.Top = sdtp.Top - 3;
                                btnval.Size = new Size(25, 25);
                                btnval.Text = "";
                                helpqry = dr["HelpQuery"].ToString().Trim();
                                btnval.Click += new EventHandler(btnval_Click);

                                Controls.Add(btnval);
                                string fName = appPath + @"\bmp\loc-on.gif";
                                if (File.Exists(fName) == true)
                                {
                                    btnval.Image = Image.FromFile(fName);
                                    // this.btnValidity.Image = Image.FromFile(fName);
                                }
                            }
                        }
                        #endregion




                        break;


                }
            }

            dataGridView1.Top = lblTable.Top * topval - (lblTable.Top / 4) * (topval - 1) + 90;

            if (dataGridView1.Columns.Count > 8)   /*Ramya */
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            //dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnMode  DataGridViewAutoSizeColumnsMode.Fill);
            //dataGridView1.AutoResizeColumns();
     

            //System.Windows.Forms.DataGridViewTextBoxColumn colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colId.HeaderText = "Id";
            //colId.Name = "colId";
           


        }

        void btnval_Click(object sender, EventArgs e)
        {
            Button chkb = ((Button)sender);
            string chName = chkb.Name.Replace("btn", "chk");
            CheckBox chk = (CheckBox)this.Controls[chName];
            if (chk.Checked == false)
            {
                MessageBox.Show("Please select the value to update");
                return;
            }


           
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            //SqlStr = "Select Loc_code,Loc_desc from Loc_Master order by Loc_code";
            tDs = oDataAccess.GetDataSet(helpqry, null, 20);
            string[] words = helpqry.Split(' ');
            

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select "+words[4]+ " Name";
            vSearchCol = words[4];
            vDisplayColumnList = words[4]+":"+words[4];
            vReturnCol = words[4];
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
           // vMainFldVal = dsMain.Tables[0].Rows[0]["Loc_desc"].ToString().Trim();
            if (oSelectPop.pReturnArray != null)
            {
               
                string vcName = chkb.Name.Replace("btn", "txt");
                MaskedTextBox mtx = (MaskedTextBox)this.Controls[vcName];
                mtx.Focus();
                mtx.Text = oSelectPop.pReturnArray[0];
            }
            
        }

        //void txtval_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        //{
        //    if (!e.IsValidInput)
        //    {
        //        toolTip1.ToolTipTitle = "Invalid Date";
        //        toolTip1.Show("The data you supplied must be a valid date in the format mm/dd/yyyy.", maskedTextBox1, 0, -20, 5000);
        //    }

        //    else
        //    {
        //        DateTime userDate = (DateTime)e.ReturnValue;
        //        if (userDate < DateTime.Now)
        //        {
        //            toolTip1.ToolTipTitle = "Invalid Date";
        //            toolTip1.Show("The date in this field must be greater than today's date.", maskedTextBox1, 0, -20, 5000);
        //            e.Cancel = true;

        //        }
        //    }
        //}

        void chk_Click(object sender, EventArgs e)
        {

            CheckBox chkb = ((CheckBox)sender);
            if (chkb.Checked)
            {
                string vcName = chkb.Name.Replace("chk", "txt");
                MaskedTextBox mtx = (MaskedTextBox)this.Controls[vcName];
                mtx.Focus();
                string btnname=chkb.Name.Replace("chk", "btn");
                Button btn = (Button)this.Controls[btnname];
             
    
                if (this.Controls[vcName].GetType().ToString() == "System.Windows.Forms.MaskedTextBox")
                {
                    ((MaskedTextBox)this.Controls[vcName]).Text = "";
                    ((MaskedTextBox)this.Controls[vcName]).ReadOnly = false;
                }
                else 
                {
                    //((CheckBox)this.Controls[vcName]).Text = "";
                    ((CheckBox)this.Controls[vcName]).Enabled = true;

                }
               

                    if ((this.Contains(btn)) && this.Controls[btnname].GetType().ToString() == "System.Windows.Forms.Button")
                    {
                        ((MaskedTextBox)this.Controls[vcName]).ReadOnly = true;
                         btn.Focus();
                        ((Button)this.Controls[btnname]).Enabled = true;
                        btn.Focus();
                    }
                
             
            }
            else
            {
                string vcName = chkb.Name.Replace("chk", "txt");
                string btnname = chkb.Name.Replace("chk", "btn");
                Button btn = (Button)this.Controls[btnname];
                
                if (this.Controls[vcName].GetType().ToString() == "System.Windows.Forms.MaskedTextBox")
                {
                    ((MaskedTextBox)this.Controls[vcName]).Text = "";
                    ((MaskedTextBox)this.Controls[vcName]).ReadOnly = true;
                }
          
                else
                {
                    ((CheckBox)this.Controls[vcName]).Checked =false;
                    ((CheckBox)this.Controls[vcName]).Enabled = false;

                }
              
                if( (this.Contains(btn)) && this.Controls[btnname].GetType().ToString() == "System.Windows.Forms.Button")
                 {
                    ((Button)this.Controls[btnname]).Enabled = false;
                 }
                
             

               // ((MaskedTextBox)this.Controls[vcName]).Text = "";
                //((MaskedTextBox)this.Controls[vcName]).ReadOnly = true;
            }

        }
        //void txtval_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    MaskedTextBox txb = ((MaskedTextBox)sender);
        //    string fldname = txb.Name.Replace("txt", "").Trim();
        //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        //    {

        //        if (ds1.Tables[0].Rows[i][0].ToString() == fldname)
        //        {
        //            string datatype = ds1.Tables[0].Rows[i][1].ToString();
        //            string num;
        //            Regex var=new Regex("^[0-9]{1-10}$");
        //            // MessageBox.Show(datatype);
        //            switch (datatype)
        //            {
        //                case "int":
        //                    num = "0123456789";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;
        //                case "varchar":
        //                    num = "abcdefghijklmnopqrstuvwxyz0123456789.";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;
        //                case "datetime":
        //                    num = "0123456789/";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;
        //                case "smalldatetime":
        //                    num = "0123456789/";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;
        //                case "text":
        //                    num = "abcdefghijklmnopqrstuvwxyz";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;
        //                case "numeric":
        //                    num = "0123456789.";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;
        //                case "decimal":
        //                    num = "0123456789.";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }

        //                    if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) 
        //                    { 
        //                        e.Handled = true; 
        //                    } 



        //                    break;
        //                case "bit":

        //                    num = "01";
                            
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
                            
        //                    break;
        //                case "char":
        //                    num = "0123456789.";
        //                    if (num.IndexOf(e.KeyChar) == -1)
        //                    {
        //                        e.Handled = true;

        //                    }
        //                    break;


        //            }


        //        }
        //    }



        //}
        void txtval_Leave(object sender, EventArgs e)
        {
           

        }




        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ((DataTable)dataGridView1.DataSource).DefaultView.Sort = String.Empty;
            string tempColName = string.Empty;   /* Ramya 27/12/12 for Bug-7802 */
            IsExitCalled = false;
            this.mcheckCallingApplication();
            if (IsExitCalled == true)
            {
                Application.Exit();
                return;
            }
            string newcol = "";
            where = string.Empty;
            int k = 0;
            CheckBox chk;
            DataTable dttab = (DataTable)dataGridView1.DataSource;
            
            for (int i = 0; i < dttab.Rows.Count; i++)
            {
                
                string selval = dttab.Rows[i][0].ToString();
                if (selval == "True")
                    k++;
            }

            if (k <= 0)
            {
                MessageBox.Show("Please Select Any Row To Update...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < dttab.Rows.Count; i++)
            {
                newcol = "";
                where = "";
            
                if (dttab.Rows[i][0].ToString() == "True")
                {
                    string str = "Update " + table + " set ";

                              
                    
                    for (int j = 1; j < dttab.Columns.Count; j++)
                    {
                       // MessageBox.Show(dttab.Columns[j].DataType.ToString());

                       
                        string chkname = "chk" + dttab.Columns[j].ColumnName.ToString();   /*Ramya 21/02/13*/

                        if (dataGridView1.Columns["col_" + dttab.Columns[j].ToString().Trim()].Visible == true)
                        {
                            if(dataGridView1.Columns["col_" + dttab.Columns[j].ToString().Trim()].ReadOnly== false)
                            //if (dttab.Columns[j].ReadOnly == false)
                            {
                                if (newcol == "")
                                {
                                    newcol = dttab.Columns[j].ToString().Trim() + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                                }
                                else
                                {
                                    newcol += "," + dttab.Columns[j].ToString().Trim() + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                                }

                            }
                            CheckBox ischk = (CheckBox)this.Controls[chkname];
                            if (this.Contains(ischk))
                            {
                                if (ischk.Checked == true)
                                {
                                    string vcName = ischk.Name.Replace("chk", "txt");
                                    if (dttab.Columns[j].DataType.ToString() == "System.Boolean")
                                    {
                                        if (newcol == "")
                                        {
                                            newcol = dttab.Columns[j].ToString().Trim() + "= '" + ((CheckBox)this.Controls[vcName]).Checked + "'";
                                        }
                                        else
                                        {
                                            newcol += "," + dttab.Columns[j].ToString().Trim() + "= '" + ((CheckBox)this.Controls[vcName]).Checked + "'";
                                        }

                                    }
                                    else if (dttab.Columns[j].DataType.ToString() == "System.Datetime")
                                    {
                                        if (newcol == "")
                                        {
                                            newcol = dttab.Columns[j].ToString().Trim() + "= '" + ((DateTimePicker)this.Controls[vcName]).Value + "'";
                                        }
                                        else
                                        {
                                            newcol += "," + dttab.Columns[j].ToString().Trim() + "= '" + ((DateTimePicker)this.Controls[vcName]).Value + "'";
                                        }

                                    }
                                    else if (dttab.Columns[j].DataType.ToString() == "System.SmallDatetime")
                                    {
                                        if (newcol == "")
                                        {
                                            newcol = dttab.Columns[j].ToString().Trim() + "= '" + ((DateTimePicker)this.Controls[vcName]).Value + "'";
                                        }
                                        else
                                        {
                                            newcol += "," + dttab.Columns[j].ToString().Trim() + "= '" + ((DateTimePicker)this.Controls[vcName]).Value + "'";
                                        }

                                    }
                                    else
                                    {
                                        if (((MaskedTextBox)this.Controls[vcName]).Text.Trim() != "")
                                        {
                                            if (newcol == "")
                                            {
                                                newcol = dttab.Columns[j].ToString().Trim() + "= '" + ((MaskedTextBox)this.Controls[vcName]).Text + "'";
                                            }
                                            else
                                            {
                                                newcol += "," + dttab.Columns[j].ToString().Trim() + "= '" + ((MaskedTextBox)this.Controls[vcName]).Text + "'";
                                            }
                                        }
                                        else
                                        {
                                            tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                                            MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                                            return;

                                        }
                                    }
                                }
                            }
                        }

                        foreach (DataRow dr in coltype.Tables[0].Rows)
                        {
                            if (dr["fldnm"].ToString().Trim() == dttab.Columns[j].ToString().Trim())
                            {
                                if (dr["KeyField"].ToString() == Convert.ToString(1))
                                {
                                     if (where == string.Empty)
                                         where = dttab.Columns[j].ToString().Trim() + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                                     else
                                         where += " and " + dttab.Columns[j].ToString().Trim() + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                                }
                            }
                        }

                        #region changed

                        ////if (key[j - 1] != string.Empty)
                        ////{
                        ////    if ((dttab.Columns[key[j - 1]].DataType.ToString().Trim() == "System.String") || (dttab.Columns[key[j - 1]].DataType.ToString().Trim() == "System.DateTime"))
                        ////    {
                        ////        if (where == string.Empty)
                        ////            where = key[j - 1] + "= '" + dttab.Rows[i][key[j - 1]].ToString().Trim() + "'";
                        ////        else
                        ////            where += " and " + key[j - 1] + "= '" + dttab.Rows[i][key[j - 1]].ToString() + "'";
                        ////    }
                        ////    else
                        ////    {
                        ////        if (where == string.Empty)
                        ////            where = key[j - 1] + "= " + dttab.Rows[i][key[j - 1]].ToString().Trim();
                        ////        else
                        ////            where += " and " + key[j - 1] + "= " + dttab.Rows[i][key[j - 1]].ToString();
                        ////    }
                        //// }


                       
                        ////if(editable[j - 1] == "Editable")
                        //// {
                        ////     string dtype = dttab.Columns[j].DataType.ToString();
                        ////    if (dttab.Columns[j].DataType.ToString() == "System.String" )
                        ////    {
                        ////        //if (Bulkupdatecol[j - 1] == "BulkUpdate")
                        ////        if(dttab.Columns[j].ReadOnly=true)
                        ////        {
                        ////            if (gridedit.ToLower() == "false")
                        ////            {
                        ////                chk = (CheckBox)this.Controls[chkname];
                        ////                if (chk.Checked == true)
                        ////                {
                        ////                    string vcName = chk.Name.Replace("chk", "txt");
                        ////                    //MaskedTextBox mtx = (MaskedTextBox)this.Controls[vcName];
                        ////                    //mtx.Focus();

                        ////                    if (((MaskedTextBox)this.Controls[vcName]).Text.Trim() != "")
                        ////                    {
                        ////                        if (newcol == "")
                        ////                        {
                        ////                            newcol = colname[j - 1] + "= '" + ((MaskedTextBox)this.Controls[vcName]).Text + "'";
                        ////                        }
                        ////                        else
                        ////                        {
                        ////                            newcol += "," + colname[j - 1] + "= '" + ((MaskedTextBox)this.Controls[vcName]).Text + "'";
                        ////                        }
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        //MessageBox.Show("Please enter some value to update");
                        ////                        tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                        ////                        MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                        return;
                        ////                    }


                        ////                }
                        ////            }
                        ////            else
                        ////            {
                        ////                if (dttab.Rows[i][j].ToString().Trim() != "")
                        ////                {
                        ////                    if (newcol == "")
                        ////                    {
                        ////                        newcol = colname[j - 1] + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        newcol += "," + colname[j - 1] + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                        ////                    }
                        ////                }
                        ////                else
                        ////                {
                        ////                    /*commented by ramya 18/02/13*/
                        ////                    //MessageBox.Show("Please enter some value to update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////                    ////tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                        ////                    //// MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                    //return;
                        ////                }

                        ////            }

                        ////        }
                        ////        else
                        ////        {
                        ////            if (dttab.Rows[i][j].ToString().Trim() != "")
                        ////            {
                        ////                if (newcol == "")
                        ////                {
                        ////                    newcol = colname[j - 1] + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                        ////                }
                        ////                else
                        ////                {
                        ////                    newcol += "," + colname[j - 1] + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                        ////                }
                        ////            }
                        ////            else
                        ////            {
                        ////                MessageBox.Show("Please enter some value to update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////                return;
                        ////            }
                                   
                        ////        }
                                
                        ////     }
                        ////    else if (dttab.Columns[j].DataType.ToString() == "System.DateTime")
                        ////    {
                        ////         DateTime dt ;
                        ////         if (Bulkupdatecol[j - 1] == "BulkUpdate")
                        ////         {
                        ////             chk = (CheckBox)this.Controls[chkname];
                        ////             if (chk.Checked == true)
                        ////             {
                        ////                 string vcName = chk.Name.Replace("chk", "txt");
                        ////                 string mydate = ((MaskedTextBox)this.Controls[vcName]).Text.Replace(" ","");
                        ////                 try
                        ////                 {
                        ////                     dt = DateTime.Parse(((MaskedTextBox)this.Controls[vcName]).Text.Trim());
                        ////                 }
                        ////                 catch
                        ////                 {
                        ////                     MessageBox.Show("Please enter date in mm/dd/yyyy format", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                     return;
                        ////                 }

                        ////                 if (newcol == "")
                        ////                 {
                        ////                     newcol = colname[j - 1] + "= '" + dt + "'";
                        ////                 }
                        ////                 else
                        ////                 {
                        ////                     newcol += "," + colname[j - 1] + "= '" + dt + "'";
                        ////                 }
                        ////                 chk.Checked = false;
                        ////                 ((MaskedTextBox)this.Controls[vcName]).Text = "";
                        ////                 ((MaskedTextBox)this.Controls[vcName]).ReadOnly = true;
                        ////             }
                        ////             else
                        ////             {
                                         
                        ////                 try
                        ////                 {
                        ////                     dt = DateTime.Parse(dttab.Rows[i][j].ToString().Trim());
                        ////                 }
                        ////                 catch
                        ////                 {
                        ////                     MessageBox.Show("Please enter date in mm/dd/yyyy format", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                     return;
                        ////                 }
                        ////                 if (newcol == "")
                        ////                 {
                        ////                    newcol = colname[j - 1] + "= '" + dt + "'";
                        ////                 }
                        ////                 else
                        ////                 {
                        ////                    newcol += "," + colname[j - 1] + "= '" + dt + "'";
                        ////                 }

                        ////             }
                        ////         }
                        ////         else
                        ////         {
                                    
                        ////             try
                        ////             {
                        ////                 dt = DateTime.Parse(dttab.Rows[i][j].ToString().Trim());
                        ////             }
                        ////             catch
                        ////             {
                        ////                 MessageBox.Show("Please enter date in mm/dd/yyyy format", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                 return;
                        ////             }
                        ////             if (newcol == "")
                        ////             {
                        ////                 newcol = colname[j - 1] + "= '" + dt + "'";
                        ////             }
                        ////             else
                        ////             {
                        ////                 newcol += "," + colname[j - 1] + "= '" + dt + "'";
                        ////             }

                        ////         }
                                

                        ////     }
                        ////    else if (dttab.Columns[j].DataType.ToString() == "System.Boolean")
                        ////    {
                        ////        if (Bulkupdatecol[j - 1] == "BulkUpdate")
                        ////        {
                        ////            chk = (CheckBox)this.Controls[chkname];

                        ////            if (chk.Checked == true)
                        ////            {
                        ////                string vcName = chk.Name.Replace("chk", "txt");
                        ////                if (((MaskedTextBox)this.Controls[vcName]).Text.Trim() != "")
                        ////                {
                        ////                    if (newcol == "")
                        ////                    {
                        ////                        newcol = colname[j - 1] + "= '" + ((CheckBox)this.Controls[vcName]).Checked + "'";
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        newcol += "," + colname[j - 1] + "= '" + ((CheckBox)this.Controls[vcName]).Checked + "'";
                        ////                    }
                        ////                }
                        ////                else
                        ////                {
                        ////                    //MessageBox.Show("Please enter some value to update");
                        ////                    tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                        ////                    MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                    return;
                        ////                }

                                        
                        ////            }
                        ////            else
                        ////            {
                        ////                if (dttab.Rows[i][j].ToString().Trim() != "")
                        ////                {
                        ////                    if (newcol == "")
                        ////                    {
                        ////                        newcol = colname[j - 1] + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        newcol += "," + colname[j - 1] + "= '" + dttab.Rows[i][j].ToString().Trim() + "'";
                        ////                    }
                        ////                }
                        ////                else
                        ////                {
                        ////                    MessageBox.Show("Please enter some value to update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////                    return;

                        ////                }
                                      

                        ////            }
                        ////        }
                               
                        ////    }
                        ////    else
                        ////    {
                        ////        if (Bulkupdatecol[j - 1] == "BulkUpdate")
                        ////        {
                        ////            chk = (CheckBox)this.Controls[chkname];
                        ////            if (chk.Checked == true)
                        ////            {
                        ////                string vcName = chk.Name.Replace("chk", "txt");
                        ////                string vcVal = ((MaskedTextBox)this.Controls[vcName]).Text.Replace(" ", "");
                        ////                if (vcVal == "" || vcVal == ".")
                        ////                {
                        ////                   // MessageBox.Show("Please enter some value to update");
                        ////                    tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                        ////                    MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                    return;
                        ////                 }
                        ////                else
                        ////                {
                        ////                    //double finalValue;
                        ////                    //double.TryParse(vcVal,out finalValue);
                        ////                    //vcVal = finalValue.ToString();

                                            
                        ////                    string regex = "[-+]?[0-9]*\\.?[0-9]+";
                        ////                    string[] finalStr = vcVal.Split('.');
                        ////                    if (finalStr[0].Substring(0, 1) == "+" || finalStr[0].Substring(0, 1) == "-")
                        ////                    {
                        ////                        char[] MyChar = {'+','-'};

                        ////                        finalStr[0] = finalStr[0].Trim(MyChar);
                        ////                    }


                        ////                    for (int l = 0; l < ds1.Tables[0].Rows.Count; l++)
                        ////                    {

                        ////                        if (dttab.Columns[j].ColumnName.ToString()==ds1.Tables[0].Rows[l]["column_name"].ToString() )
                        ////                        {
                        ////                            int preclen=Convert.ToInt32(ds1.Tables[0].Rows[l]["numeric_precision"].ToString())-Convert.ToInt32(ds1.Tables[0].Rows[l]["numeric_scale"].ToString());

                        ////                            if (finalStr[0].Length >= (preclen+1))
                        ////                            {
                        ////                                MessageBox.Show("Please Enter Proper Number In (" + ds1.Tables[0].Rows[l]["numeric_precision"].ToString() + "," + ds1.Tables[0].Rows[l]["numeric_scale"].ToString() + ") Format", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                                return;
                        ////                            }

                        ////                        }
                        ////                    }
                        ////                    //if (finalStr[0].Length >= 17)
                        ////                    //{
                        ////                    //    MessageBox.Show("Please Enter Proper Number ");
                        ////                    //    return;
                        ////                    //}
                        ////                    if (!System.Text.RegularExpressions.Regex.IsMatch(vcVal, regex))
                        ////                    {
                        ////                        MessageBox.Show("Please Enter Proper Number", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                        return;
                        ////                    }
                        ////                    if (newcol == "")
                        ////                    {
                        ////                        newcol = colname[j - 1] + "=" + ((MaskedTextBox)this.Controls[vcName]).Text.Replace(" ", "");
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        newcol += "," + colname[j - 1] + "=" + ((MaskedTextBox)this.Controls[vcName]).Text.Replace(" ", "");
                        ////                    }
                                            
                        ////                }
                           
                        ////            }
                        ////            else
                        ////            {
                        ////                string vcVal = dttab.Rows[i][j].ToString().Replace(" ", "");
                        ////                if (vcVal == "" || vcVal == ".")
                        ////                {
                        ////                    MessageBox.Show("Please enter some value to update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////                    //tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                        ////                    //MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                    return;
                        ////                }
                        ////                else
                        ////                {
                        ////                    if (newcol == "")
                        ////                    {
                        ////                        newcol = colname[j - 1] + "=" + dttab.Rows[i][j].ToString().Replace(" ", "");
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        newcol += "," + colname[j - 1] + "=" + dttab.Rows[i][j].ToString().Replace(" ", "");
                        ////                    }
                        ////                }
                        ////            }
                        ////        }
                        ////        else
                        ////        {
                        ////            string vcVal = dttab.Rows[i][j].ToString().Replace(" ", "");
                        ////            if (vcVal == "" || vcVal == ".")
                        ////            {
                        ////                MessageBox.Show("Please enter some value to update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////                //tempColName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vcName.Substring(3));  /* Ramya 27/12/12 for Bug-7802 */
                        ////                //MessageBox.Show("Please provide " + tempColName + " value to update ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        ////                return;
                        ////            }
                        ////            else
                        ////            {
                        ////                if (newcol == "")
                        ////                {
                        ////                    newcol = colname[j - 1] + "=" + dttab.Rows[i][j].ToString().Replace(" ", "");
                        ////                }
                        ////                else
                        ////                {
                        ////                    newcol += "," + colname[j - 1] + "=" + dttab.Rows[i][j].ToString().Replace(" ", "");
                        ////                }
                        ////            }
                        ////        }
                                
                        ////    }
                            
                        ////}
                        ////if (key[j - 1] != string.Empty)
                        ////{
                        ////    if ((dttab.Columns[key[j - 1]].DataType.ToString().Trim() == "System.String") || (dttab.Columns[key[j - 1]].DataType.ToString().Trim() == "System.DateTime"))
                        ////    {
                        ////        if (where == string.Empty)
                        ////            where = key[j - 1] + "= '" + dttab.Rows[i][key[j - 1]].ToString().Trim() + "'";
                        ////        else
                        ////            where += " and " + key[j - 1] + "= '" + dttab.Rows[i][key[j - 1]].ToString() + "'";
                        ////    }
                        ////    else
                        ////    {
                        ////        if (where == string.Empty)
                        ////            where = key[j - 1] + "= " + dttab.Rows[i][key[j - 1]].ToString().Trim();
                        ////        else
                        ////            where += " and " + key[j - 1] + "= " + dttab.Rows[i][key[j - 1]].ToString();
                        ////    }
                        //// }
                        #endregion 
                    }
                    str = str + newcol + " where " + where;
                    try
                    {
                        oDataAccess.ExecuteSQLStatement(str, null, 20, true);
                    }
                    catch(System.Exception ex)
                    {
                        MessageBox.Show("Please Enter Proper Values To Update", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); /* Ramya 27/12/12 for Bug-7802 */
                        return;
                    }

                }
                dataGridView1.Rows[i].Cells[0].Value = 0;
                //Rup 17/11/2011
                //ueDynamicMasterProcedures.cDynamicMasterProcedures oDynamicproc = new ueDynamicMasterProcedures.cDynamicMasterProcedures();
                ueTblFieldsSave.cTblFieldsSave oTblFieldsSave = new ueTblFieldsSave.cTblFieldsSave();
          
                oTblFieldsSave.mthBtnUpdate(this, sender, e, null, this.pPara, modName);

                
            }

            for (int j = 1; j < dttab.Columns.Count; j++)
            
            {
                    string chkbx = "chk" + dttab.Columns[j].ColumnName.ToString();
                    CheckBox ch = ((CheckBox)this.Controls[chkbx]);
                    if (ch != null)
                    {
                        if (ch.Checked == true)
                        {

                            string vcName = ch.Name.Replace("chk", "txt");

                            if (this.Controls[vcName].GetType().ToString() == "System.Windows.Forms.MaskedTextBox")
                            {
                                ((MaskedTextBox)this.Controls[vcName]).Text = "";
                                ((MaskedTextBox)this.Controls[vcName]).ReadOnly = true;
                            }
                            else
                            {
                                ((CheckBox)this.Controls[vcName]).Checked =false;
                                ((CheckBox)this.Controls[vcName]).Enabled = false;

                            }
                        }
                        ch.Checked = false;
                    }
                    
                }


            string str1 = "Select " + colums + " from " + table;
           DataTable dt1 = oDataAccess.GetDataTable(str1, null, 20);
           dataGridView1.DataSource = dt1;
           timer1.Enabled = true; /*Ramya 01/11/12*/
           timer1.Interval = 1000;
           MessageBox.Show("Updated Successfully", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);



        }


       private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
          
        {
      
            if (((DataTable)dataGridView1.DataSource).Columns[e.ColumnIndex].DataType.ToString() == "System.DateTime")
            {
                MessageBox.Show("Please Enter Date In MM/DD/YYYY Format");
            }
            else if (((DataTable)dataGridView1.DataSource).Columns[e.ColumnIndex].DataType.ToString() == "System.Decimal")
            {
                MessageBox.Show("Please Enter Decimal Value");
            }
           else if(((DataTable)dataGridView1.DataSource).Columns[e.ColumnIndex].DataType.ToString() == "System.String")
            {
               MessageBox.Show("Please Enter String Value");
            }
           else
           {
                MessageBox.Show("Please Enter Proper Value");
           }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            //DataSet dsMain = oDataAccess.GetDataSet("select * from TableUpdate_Master where module_name='" + modName+"'", null, 20);  //TableUpdate_Master
            //return;
            DataTable dttab = (DataTable)dataGridView1.DataSource;
            for (int j = 1; j < dttab.Columns.Count; j++)
            {
                string chkbx = "chk" + dttab.Columns[j].ColumnName.ToString();
                CheckBox ch = ((CheckBox)this.Controls[chkbx]);
                if (ch != null)
                {
                    if (ch.Checked == true)
                    {

                        string vcName = ch.Name.Replace("chk", "txt");

                        if (this.Controls[vcName].GetType().ToString() == "System.Windows.Forms.MaskedTextBox")
                        {
                            ((MaskedTextBox)this.Controls[vcName]).Text = "";
                            ((MaskedTextBox)this.Controls[vcName]).ReadOnly = true;
                        }
                        else
                        {
                            ((CheckBox)this.Controls[vcName]).Checked = false;
                            ((CheckBox)this.Controls[vcName]).Enabled = false;

                        }
                    }
                    ch.Checked = false;
                }

            }

            string str1 = "Select " + colums + " from " + table;
            DataTable dt1 = oDataAccess.GetDataTable(str1, null, 20);
            dataGridView1.DataSource = dt1;
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ueTblFields.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        private void mcheckCallingApplication()/*Added Rup 07/03/2011*/
        {

            Process pProc;
            Boolean procExists = true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this.pPApplPID));
                String pName = pProc.ProcessName;
                string pName1 = this.pPApplName.Substring(0, this.pPApplName.IndexOf("."));
                //MessageBox.Show(pName);
                //MessageBox.Show(pName1);
                if (pName.ToUpper() != pName1.ToUpper())
                {
                    procExists = false;
                }
            }
            catch (Exception)
            {
                procExists = false;

            }
            if (procExists == false)
            {
                MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                IsExitCalled = true;
                Application.Exit();
            }
        }

        private void frmItemDetailsMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();//Rup

        }


        private void SetMenuRights() //Rup
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    strSQL += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this.pPApplRange;
                    strSQL += " and dbo.func_decoder([user],'T') ='" + this.pAppUerName.Trim() + "'";

                }
            }
            dsRights = oDataAccess.GetDataSet(strSQL, null, 20);


            if (dsRights != null)
            {
                string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                int len = rights.Length;
                string newString = "";
                ArrayList rArray = new ArrayList();

                while (len > 2)
                {
                    newString = rights.Substring(0, 2);
                    rights = rights.Substring(2);
                    rArray.Add(newString);
                    len = rights.Length;
                }
                rArray.Add(rights);
                btnUpdate.Enabled = (rArray[1].ToString().Trim() == "CY" ? true : false);
                //btnNew.Enabled = (myArray[0].ToString().Trim() == "IY" ? true : false);
                //btnEdit.Enabled = (myArray[1].ToString().Trim() == "CY" ? true : false);
                ////menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                //btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }



        private void SetFormColor() //Rup
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsColor != null)
            {
                if (dsColor.Tables.Count > 0)
                {
                    dsColor.Tables[0].TableName = "ColorInfo";
                    colorCode = dsColor.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

      

        
   
    }

}

    


