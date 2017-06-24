using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ueProductUpgrade
{
    public partial class frmSearchForm : Form
    {
        SqlConnection conn;
        DataView dv;
        DataSet ds;
        bool fromActualTbl = false;
        #region Properties
        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
        }
        private string _searchFields;
        public string SearchFields
        {
            get { return _searchFields; }
        }
        private string _colCaption;
        public string ColCaption
        {
            get { return _colCaption; }
        }
        private string _hideFields;
        public string HideFields
        {
            get { return _hideFields; }
        }
        private string[] _returnString;
        public string[] ReturnString
        {
            get { return _returnString; }
        }
        #endregion
        public frmSearchForm()
        {
            InitializeComponent();
        }
        public frmSearchForm(SqlConnection cn, string frmCaption, string tblnm, string SearchFldlst, string colCaption, string hideFields, string retString)
        {
            InitializeComponent();
            this.Text = frmCaption;                 //Form Caption
            _tableName = tblnm;                      //Search from Table Name
            _searchFields = SearchFldlst;            //Search Fields List
            _colCaption = colCaption;                //Column Caption list    
            _hideFields = hideFields;                 //Hide fields or visible false
            _returnString = retString.Split(Char.Parse(","));
            conn = cn;
            fromActualTbl = true;
        }

        public frmSearchForm(DataTable dt, string frmCaption, string SearchFldlst, string colCaption, string hideFields, string retString)
        {
            InitializeComponent();
            this.Text = frmCaption;                 //Form Caption
            _searchFields = SearchFldlst;            //Search Fields List
            _colCaption = colCaption;                //Column Caption list    
            _hideFields = hideFields;                 //Hide fields or visible false
            _returnString = retString.Split(Char.Parse(","));
            fromActualTbl = false;
            dv = dt.DefaultView;
        }

        #region Methods to Implement
        private void GetData(bool settings)
        {
            string filterStr = "";
            int colLength = 0;
            int lastcolPosition = 0;
            string strSort = "";
            if (settings == true)
            {
                filterStr = "Select distinct " + SearchFields + " From " + TableName;
                if (fromActualTbl)
                {
                    SqlDataAdapter da = new SqlDataAdapter(filterStr, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    dv = ds.Tables[0].DefaultView;
                }
            }
            else
            {
                for (int i = 0; i < DataGrid.Columns.Count; i++)
                {
                    if (DataGrid.Columns[i].ValueType.ToString() == "System.String")
                    {
                        filterStr = filterStr + DataGrid.Columns[i].Name + " Like '%" + txtSearch.Text.Trim() + "%' Or ";
                        strSort = strSort + DataGrid.Columns[i].Name.ToString() + ",";
                    }
                }
                filterStr = filterStr.Substring(0, filterStr.Length - 3);
                strSort = strSort.Substring(0, strSort.Length - 1);
                dv.RowFilter = filterStr;
                dv.Sort = strSort;
            }
            DataGrid.DataSource = dv;
            if (settings == true)
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
                        int x = 0;
                        txtSearch.Width = txtSearch.Width + (colLength - this.Width) + 20;
                        x = ((colLength + 20) - this.Width) / 2;
                        this.Width = colLength + 20;
                        DataGrid.Width = colLength + 11;
                        this.Left = this.Left - x;
                    }
                }
            }
            this.Refresh();
            toolStripStatusLabel1.Text = "Records Found : " + dv.Count.ToString();
        }
        #endregion

        private void frmSearchForm_Load(object sender, EventArgs e)
        {
            GetData(true);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 40:
                    DataGrid.Focus();
                    break;
                case 27:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                case 13:
                    if (DataGrid.CurrentRow != null)
                    {
                        if (DataGrid.CurrentRow.Index >= 0)
                        {
                            for (int i = 0; i < ReturnString.Length; i++)
                            {
                                ReturnString[i] = Convert.ToString(DataGrid.CurrentRow.Cells[ReturnString[i]].Value);
                            }
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        if (DataGrid.RowCount > 0)
                        {
                            for (int i = 0; i < ReturnString.Length; i++)
                            {
                                ReturnString[i] = Convert.ToString(dv[0][ ReturnString[i]]);
                            }
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void DataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGrid.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < ReturnString.Length; i++)
                {
                    ReturnString[i] = Convert.ToString(DataGrid.CurrentRow.Cells[ReturnString[i]].Value);
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 40:
                    DataGrid.Focus();
                    break;
                case 27:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                case 13:
                    if (DataGrid.CurrentRow.Index >= 0)
                    {
                        for (int i = 0; i < ReturnString.Length; i++)
                        {
                            ReturnString[i] = Convert.ToString(DataGrid.CurrentRow.Cells[ReturnString[i]].Value);
                        }
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                default:
                    break;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData(false);
        }
    }
}
