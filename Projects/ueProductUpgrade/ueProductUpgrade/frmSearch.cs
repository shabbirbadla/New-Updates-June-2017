using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

    public partial class frmSearch : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        DataView dv=new DataView();
        DataTable dt = new DataTable(); 
        DataTable SourceTable = new DataTable();
        OleDbDataAdapter da;
        DataSet ds = new DataSet();
        DataRow dr;
        int cnt = 0;
        int sType = 0;
        public frmSearch(OleDbConnection cn, string frmCaption, string tblnm, string keyField, string SearchFldlst, string colCaption, string hideFields, string retFields,string retCursor)
        {
            InitializeComponent();
            formCaption = frmCaption;                 //Form Caption
            TableName = tblnm;                      //Search from Table Name
            SearchFields = SearchFldlst;            //Search Fields List
            ColCaption = colCaption;                //Column Caption list    
            HideFields = hideFields;                 //Hide fields or visible false
            KeyField = keyField;                    //Key fields
            DefaultSearch = defaultSearch;          //Within or Incremental
            returnFields = retFields;            //Return Fields
            ReturnCursor = (retCursor==""?retCursor:tblnm);
            conn = cn;
        }
        
        private string _formCaption;
        public string formCaption
        {
            get { return _formCaption; }
            set { _formCaption = value; }
        }
        private string _tableName;
        public  string TableName
        {
            get { return _tableName;}
            set { _tableName=value;}
        }
        private string _searchFields;
        public string SearchFields
        {
            get {return _searchFields ;}
            set { _searchFields=value;}
        }
        private string _keyField;
        public string KeyField
        {
            get { return _keyField;}
            set { _keyField=value;}
        }
        private string _colCaption;
        public string ColCaption
        {
            get { return _colCaption;}
            set { _colCaption=value;}
        }
        private string _hideFields;
        public string HideFields
        {
            get { return _hideFields; }
            set { _hideFields = value; }
        }
        private string _returnFields;
        public string returnFields
        {
            get { return _returnFields; }
            set { _returnFields = value; }
        }
        private object _returnValue;
        public object ReturnValue
        {
            get { return _returnValue;}
            set { _returnValue=value; }
        }
        private string _retcur;
        public string ReturnCursor
        {
            get { return _retcur;}
            set { _retcur=value; }
        }
        private void cmdSelect_Click(object sender, EventArgs e)
        {
           
        }

        private void GetData(bool settings)
        {
            string filterStr = "";
            bool setForm = settings;
            int colLength = 0;
            int lastcolPosition = 0;
            string strSort = "";
            if (setForm == true)
            {
                if (sType == 0)
                {
                    filterStr = "Select distinct " + SearchFields + " From " + TableName;
                    da = new OleDbDataAdapter(filterStr, conn);
                    da.Fill(ds);
                    dv = ds.Tables[0].DefaultView;
                }
                else
                {
                    filterStr = "Select distinct " + SearchFields + " From " + SourceTable;
                    dv = SourceTable.DefaultView;
                }
            }
            else
            {
                for (int i = 0; i < DataGrid.Columns.Count; i++)
                {
                    if (DataGrid.Columns[i].ValueType.ToString()=="System.String")
                    {
                        filterStr = filterStr + DataGrid.Columns[i].Name + " Like '%" + txtSearch.Text.Trim() + "%' Or ";
                        strSort = strSort + DataGrid.Columns[i].Name.ToString()+",";
                    }
                }
                filterStr = filterStr.Substring(0, filterStr.Length - 3);
                strSort = strSort.Substring(0, strSort.Length - 1);
                dv.RowFilter = filterStr;
                dv.Sort = strSort;
            }
            DataGrid.DataSource = dv;
            if (setForm == true)
            {
                if (this.ColCaption.Length > 0)
                {
                    string[] str1 = this.ColCaption.Split(Char.Parse(","));
                    foreach (string x in str1)
                    {
                        try
                        {
                            if (x.IndexOf(":") > 0 && this.SearchFields.IndexOf(x.Substring(0, x.IndexOf(":"))) >= 0)
                            {
                                DataGrid.Columns[x.Substring(0, x.IndexOf(":"))].HeaderText = x.Substring((x.IndexOf(":") + 1), x.Length - (x.IndexOf(":") + 1));
                            }
                            else
                            {
                                throw new Exception("'" + x.Substring(0, x.IndexOf(":")) + "' field for column caption not found in Search fields list...!!!");
                            }
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }

                    }
                }
                if (this.HideFields.Length > 0)
                {
                    string[] str2 = this.HideFields.Split(Char.Parse(","));
                    foreach (string y in str2)
                    {
                        for (int i = 0; i < DataGrid.Columns.Count; i++)
                        {
                            if (y.ToUpper() == DataGrid.Columns[i].Name.ToString().ToUpper())
                            {
                                DataGrid.Columns[i].Visible = false;
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < DataGrid.Columns.Count; i++)
                {
                    if (DataGrid.Columns[i].Visible == true)
                    {
                        if (DataGrid.Columns[i].ValueType.ToString() == "System.String")
                        {
                            DataGrid.Columns[i].Width = 200;
                        }
                        else
                        {
                            DataGrid.Columns[i].Width = 50;
                        }
                        colLength = colLength + DataGrid.Columns[i].Width;
                        lastcolPosition = i;
                    }
                }
                if (colLength > 0)
                {
                    if (colLength <= this.Width)
                    {
                        DataGrid.Columns[lastcolPosition].Width = DataGrid.Columns[lastcolPosition].Width + this.Width - colLength;
                    }
                    else
                    {
                        int x=0;
                        cmdSelect.Left = cmdSelect.Left + (colLength - this.Width) + 20;
                        txtSearch.Width = txtSearch.Width + (colLength - this.Width) + 20;
                        x =( (colLength + 20) - this.Width)/2;
                        this.Width = colLength + 20;
                        DataGrid.Width = colLength + 11;
                        this.Left = this.Left - x;
                    }
                }
            }
            this.Refresh();
            toolStripStatusLabel1.Text = "Records Found : " + dv.Count.ToString();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            GetData(true);
            string[] retstr = this.returnFields.Split(Char.Parse(","));
            foreach (string a in retstr)
            {
                cnt = cnt + 1;
            }
            
            dt = dv.Table.Clone();
            dt.TableName = ReturnCursor;
            dr = dt.NewRow();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData(false);
        }


        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 40:
                    DataGrid.Focus();
                    break;
                case 27:
                    this.Dispose();
                    break;
                case 13:
                    txtSearch.Text = DataGrid.CurrentCell.Value.ToString();
                    if (cnt > 1)
                    {
                        ReturnValue = (object)ReturnDataTable();
                    }
                    else
                    {
                        if (DataGrid.CurrentRow.Index >= 0)
                        {
                            ReturnValue = (object)DataGrid.CurrentRow.Cells[returnFields].Value.ToString();
                        }
                        else
                        {
                            ReturnValue = "";
                        }
                    }
                    this.Hide();
                    break;
                default:
                    break;
            }
        }

        private void DataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cnt > 1)
            {
                ReturnValue = (object)ReturnDataTable();
            }
            else
            {
                if (DataGrid.CurrentRow.Index >= 0)
                {
                    ReturnValue = (object)DataGrid.CurrentRow.Cells[returnFields].Value.ToString();
                }
                else
                {
                    ReturnValue = "";
                }
            }
            this.Hide();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 40:
                    DataGrid.Focus();
                    break;
                case 27:
                    if (cnt > 1)
                    {
                        ReturnValue = (object)ReturnDataTable();
                    }
                    else
                    {
                        ReturnValue = "";
                    }
                    this.Dispose();
                    break;
                case 13:
                    if (cnt > 1)
                    {
                        ReturnValue = (object)ReturnDataTable();
                    }
                    else
                    {
                        if (DataGrid.CurrentRow!=null)
                        {
                            ReturnValue = (object)DataGrid.CurrentRow.Cells[returnFields].Value.ToString();
                        }
                        else
                        {
                            ReturnValue = null;
                        }
                    }
                    this.Hide();
                    break;
                default:
                    break;
            }
        }
    }
