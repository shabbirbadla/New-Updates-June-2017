using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows.Forms;
using udclsDGVNumericColumn;

namespace ExportDataToFile
{
    public partial class Form1 : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;

        public static string constring, tablename;
        String cAppPId, cAppName;
        string val = null;
        private string Excel03Con;
        private string Excel07Con;
        string[] columnNames;
        int istemp_count;
        OleDbConnection MyConnection;
        OleDbDataAdapter MyCommand;

        DataTable dt_field_setting = new DataTable();
        DataTable dt_table_name = new DataTable();
        DataTable dt_ExpFldMap1;
        DataTable dt_record;

        DataTable dtExcel = new DataTable();
        DataTable dtEcxelData = new DataTable();
        DataTable dt = new DataTable();
        DataTable extrafield = new DataTable();
        DataTable extra;
        DataTable record;
        string filepath;
        static int result;
        string oldsortorder, newsortorder;
        public Form1(int a)
        {
            result = a;
        }
        public Form1(string[] args)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {

            }

            this.MaximizeBox = false;
            //   this.MinimizeBox = false;
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Export Data To File";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];

            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;

            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            progressBar1.Visible = false;
            label6.Visible = false;
            label7.Text = "";
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            set_table_name();

            setting_grid1();

            resizesetting();
            mInsertProcessIdRecord();
        }

        public void set_table_name()
        {
            grid_table_name.AutoGenerateColumns = false;
            string strSQL = "select Name,FileName from MASTCODE where ExpExcel='1' order by EXPSORTORD";
            dt_table_name = oDataAccess.GetDataTable(strSQL, null, 20);
        }

        public void setting_grid1()
        {
            grid_table_name.DataSource = dt_table_name;
            grid_table_name.Columns[0].DataPropertyName = "Name";

            grid_table_name.RowHeadersVisible = false;
            grid_table_name.AllowUserToAddRows = false;
            grid_table_name.AutoGenerateColumns = false;

            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;

            foreach (DataGridViewColumn dc in grid_table_name.Columns)
            {
                dc.ReadOnly = true;
            }
            grid_table_name.Size = new Size(184, panel1.Height - 90);
            grid_table_name.Columns[0].HeaderText = "Table Name";
            grid_table_name.AllowUserToResizeColumns = false;

            grid_table_name.AllowUserToResizeRows = false;
            grid_table_name.MultiSelect = false;
            dataGridView2.MultiSelect = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void resizesetting()
        {
            panel1.Size = new Size(this.Width - 20, this.Height - 83);
            panel2.Size = new Size(this.Width - 20, 41);
            panel4.Size = new Size(this.Width - 20, 41);
            grid_table_name.Size = new Size(184, panel1.Height - 90);
            dataGridView1.Size = new Size(435, panel1.Height - 90);
            dataGridView2.Size = new Size(panel1.Width - 655, panel1.Height - 90);

            panel3.Size = new Size(this.Width - 40, panel1.Height);
            panel5.Size = new Size(this.Width - 40, panel1.Height);

            button2.Location = new Point(this.Width - 743, 1);
            label8.Location = new Point(this.Width - 820, 20);

            button4.Location = new Point(this.Width - 120, 10);
            progressBar1.Location = new Point(this.Width - 360, 20);
            label6.Location = new Point(this.Width - 360, 40);
        }

        private void grid_table_name_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                val = dt_table_name.Rows[e.RowIndex][e.ColumnIndex].ToString();
                tablename = dt_table_name.Rows[e.RowIndex][1].ToString();

                label5.Text = "";
                label5.Text = "Export Data For :" + val.ToString();

                insert_field(val);
                set_table_field();
                set_table_record();
            }
            catch (Exception w)
            {
                MessageBox.Show(w.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void insert_field(string table_name)
        {
            string strSQL1, strSQL2;
            try
            {
                DataTable dt_fields = new DataTable();
                if (table_name.ToUpper() == "BOM MASTER")
                {
                    strSQL1 = "select a.[name], [FieldId]=b.name,[Field Name] = (Case when isnull(c.value, '')= '' then b.name else c.value end)from sys.tables a inner join sys.columns b on a.object_id = b.object_id left join sys.extended_properties c on a.object_id = c.major_id and b.column_id = c.minor_id and c.name = 'MS_Description' Where a.[name]='BOMHEAD' or a.[name]='Bomdet' order by a.name desc";
                }
                else
                {
                    strSQL1 = "select a.[name],[FieldId]=b.name,[Field Name] = (Case when isnull(c.value, '')= '' then b.name else c.value end)from sys.tables a inner join sys.columns b on a.object_id = b.object_id left join sys.extended_properties c on a.object_id = c.major_id and b.column_id = c.minor_id and c.name = 'MS_Description' Where a.[name]='" + tablename + "' order by b.column_id";
                }

                dt_fields = oDataAccess.GetDataTable(strSQL1, null, 20);

                DataTable dt_ExpFldMap = new DataTable();
                if (table_name.ToUpper() == "BOM MASTER")
                {
                    strSQL2 = "select * from ExpFldMap where TblName='BOMHEAD' or TblName='Bomdet'";
                }
                else
                {
                    strSQL2 = "select * from ExpFldMap where TblName='" + tablename + "'";
                }

                dt_ExpFldMap = oDataAccess.GetDataTable(strSQL2, null, 20);

                if (dt_ExpFldMap.Rows.Count == 0)
                {
                    oDataAccess.BeginTransaction();
                    int j = 0;
                    string strSql4 = string.Empty;
                    int updated = 0;
                    foreach (DataRow dr in dt_fields.Rows)
                    {
                        ++j;
                        strSql4 += "insert into ExpFldMap values('" + dr[0].ToString() + "','" + dr[1].ToString() + "','" + true + "'," + j + ",'" + dr[2].ToString() + "','" + false + "') \n";
                    }
                    try
                    {
                        updated = oDataAccess.ExecuteSQLStatement(strSql4, null, 100, true);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    oDataAccess.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void set_table_field()
        {
            string strSQL;
            try
            {
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.AllowUserToResizeColumns = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.MultiSelect = false;

                dt_ExpFldMap1 = new DataTable();
                if (tablename.ToUpper() == "BOMHEAD")
                {
                    strSQL = "select * from ExpFldMap where TblName='BOMHEAD' or TblName='Bomdet'  order by  SortOrder ";
                }
                else
                {
                    strSQL = "select * from ExpFldMap where TblName='" + tablename + "'  order by  SortOrder ";
                }

                dt_ExpFldMap1 = oDataAccess.GetDataTable(strSQL, null, 20);

                dataGridView1.Size = new Size(435, this.Height - 155);

                dataGridView1.DataSource = dt_ExpFldMap1;
                dataGridView1.Columns[0].DataPropertyName = "ToExport";
                dataGridView1.Columns[0].HeaderText = "";

                dataGridView1.Columns[1].DataPropertyName = "TblName";
                dataGridView1.Columns[1].HeaderText = "Table Name";
                dataGridView1.Columns[1].ReadOnly = true;

                dataGridView1.Columns[2].DataPropertyName = "fldName";
                dataGridView1.Columns[2].HeaderText = "Field Name";
                dataGridView1.Columns[2].ReadOnly = true;

                dataGridView1.Columns[3].DataPropertyName = "Caption";
                dataGridView1.Columns[3].HeaderText = "Caption";

                dataGridView1.Columns[4].DataPropertyName = "SortOrder";
                dataGridView1.Columns[4].HeaderText = "Sort Order";

                Rectangle rect = dataGridView1.GetCellDisplayRectangle(0, -1, true);
                rect.Y = 4;
                rect.X = 20;
                CheckBox checkboxHeader = new CheckBox();
                checkboxHeader.Name = "checkboxHeader";

                checkboxHeader.Size = new Size(15, 15);
                checkboxHeader.Location = rect.Location;
                checkboxHeader.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
                dataGridView1.Controls.Add(checkboxHeader);

                dataGridView1.Size = new Size(435, panel1.Height - 90);
                dataGridView2.Size = new Size(panel1.Width - 655, panel1.Height - 90);

                label4.Text = label5.Text.ToString().Replace("Export Data For :", "").Replace("&", "&&");
                string field = set_table_field_add();
                setenable();

                int count = dt_ExpFldMap1.Rows.Count;
                int count1 = 0;

                foreach (DataRow dr in dt_ExpFldMap1.Rows)
                {
                    if (dr[2].ToString() == "True")
                    {
                        count1++;
                    }
                }

                if (count == count1)
                {
                    CheckBox headerBox = ((CheckBox)dataGridView1.Controls.Find("checkboxHeader", true)[0]);
                    headerBox.Checked = true;
                }
                else
                {
                    CheckBox headerBox = ((CheckBox)dataGridView1.Controls.Find("checkboxHeader", true)[0]);
                    headerBox.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        public void setenable()
        {
            string strSQL;
            istemp_count = 0;
            DataTable dt = new DataTable();
            if (tablename.ToUpper() == "BOMHEAD")
            {
                strSQL = "select fldName from ExpFldMap where TblName='BOMHEAD' or TblName='Bomdet'  order by  SortOrder ";
            }
            else
            {
                strSQL = "select fldName from ExpFldMap where TblName='" + tablename + "' and istemplate='true' order by  SortOrder ";
            }

            dt = oDataAccess.GetDataTable(strSQL, null, 20);

            foreach (DataRow dr in dt_ExpFldMap1.Rows)
            {
                if (Convert.ToBoolean(dr["istemplate"]) == true)
                {
                    istemp_count++;
                    dataGridView1.Rows[dt_ExpFldMap1.Rows.IndexOf(dr)].Cells[0].ReadOnly = true;
                }
                else
                {
                    dataGridView1.Rows[dt_ExpFldMap1.Rows.IndexOf(dr)].Cells[0].ReadOnly = false;
                }
            }
        }

        public string set_table_field_add()
        {
            string strSQL1;
            DataTable dt_fields = new DataTable();
            DataTable dt_fieldsacMast = new DataTable();
            if (tablename.ToUpper() == "BOMHEAD")
            {
                strSQL1 = "select [FieldId]=b.name,[Field Name] = (Case when isnull(c.value, '')= '' then b.name else c.value end),b.System_Type_Id ,a.[name] from sys.tables a inner join sys.columns b on a.object_id = b.object_id left join sys.extended_properties c on a.object_id = c.major_id and b.column_id = c.minor_id and c.name = 'MS_Description' Where a.[name]='BOMHEAD' or a.[name]='Bomdet' order by b.column_id";
            }
            else
            {
                strSQL1 = "select [FieldId]=b.name,[Field Name] = (Case when isnull(c.value, '')= '' then b.name else c.value end),b.System_Type_Id ,a.[name] from sys.tables a inner join sys.columns b on a.object_id = b.object_id left join sys.extended_properties c on a.object_id = c.major_id and b.column_id = c.minor_id and c.name = 'MS_Description' Where a.[name]='" + tablename + "' order by b.column_id";
            }

            dt_fields = oDataAccess.GetDataTable(strSQL1, null, 20);

            int count;
            string cfield = string.Empty;
            if (tablename == "shipto")
            {
                strSQL1 = "select a.[name] from sys.columns a Where a.[Object_id]=Object_id('AC_Mast') order by a.column_id";
                dt_fieldsacMast = oDataAccess.GetDataTable(strSQL1, null, 20);
                DataRow _dr;
                int nType;
                string cVal = string.Empty;
                foreach (DataRow row in dt_ExpFldMap1.Rows)
                {
                    if (row["ToExport"].ToString().Trim() == "True")
                    {
                        count = 0;
                        foreach (DataRow dr1 in dt_fields.Rows)
                        {
                            if (row[1].ToString().Trim() == dr1[0].ToString().Trim())
                            {
                                count++;
                                if (row[1].ToString().Trim() == "ac_name")
                                {
                                    cfield = cfield + (cfield == string.Empty ? "" : ",") + "a." + "[" + row[1].ToString() + "] as '" + dr1[1].ToString() + "'";
                                }
                                else
                                {
                                    _dr = dt_fieldsacMast.Select("Name='" + row[1].ToString().Trim() + "'").FirstOrDefault();
                                    if (_dr == null)
                                    {
                                        nType = Convert.ToInt16(dr1[2]);
                                        switch (nType)
                                        {
                                            case 35:
                                                cVal = "''";
                                                break;
                                            case 56:
                                                cVal = "0";
                                                break;
                                            case 61:
                                                cVal = "cast('' as DateTime)";
                                                break;
                                            case 104:
                                                cVal = "0";
                                                break;
                                            case 106:
                                                cVal = "0";
                                                break;
                                            case 108:
                                                cVal = "0";
                                                break;
                                            case 167:
                                                cVal = "''";
                                                break;
                                            default:
                                                cVal = "''";
                                                break;
                                        }
                                        cfield = cfield + (cfield == string.Empty ? "" : ",") + (row[1].ToString().Trim() == "Location_Id" ? "CASE WHEN ISNULL(b.Ac_id,0)=0 THEN a.[City] else ISNULL(b.[" + row[1].ToString() + "], " + cVal + ")  END " : "ISNULL(b.[" + row[1].ToString() + "]," + cVal + ") ") + "as '" + dr1[1].ToString() + "'";
                                    }
                                    else
                                    {
                                        cfield = cfield + (cfield == string.Empty ? "" : ",") + " CASE WHEN ISNULL(b.Ac_id,0)=0 THEN " + (row[1].ToString().Trim() == "Location_Id" ? "a.[City]" : "a.[" + row[1].ToString() + "]") + " ELSE b." + "[" + row[1].ToString() + "] END as '" + dr1[1].ToString() + "'";
                                    }
                                }
                                break;
                            }
                        }
                        if (count == 0)
                        {
                            cfield = cfield + (cfield == string.Empty ? "" : ",") + "'' as [" + row[1].ToString() + "]";
                            dataGridView1.Rows[dt_ExpFldMap1.Rows.IndexOf(row)].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow row in dt_ExpFldMap1.Rows)
                {
                    if (row["ToExport"].ToString().Trim() == "True")
                    {
                        count = 0;
                        foreach (DataRow dr1 in dt_fields.Rows)
                        {
                            if (row[1].ToString().Trim() == dr1[0].ToString().Trim())
                            {
                                count++;
                                cfield = cfield + (cfield == string.Empty ? "" : ",") + row[0].ToString() + ".[" + row[1].ToString() + "] as '" + dr1[1].ToString() + "'";
                                break;
                            }
                        }
                        if (count == 0)
                        {
                            cfield = cfield + (cfield == string.Empty ? "" : ",") + "'' as [" + row[1].ToString() + "]";
                            dataGridView1.Rows[dt_ExpFldMap1.Rows.IndexOf(row)].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                    }
                }
            }
            return cfield;
        }

        public void set_table_record()
        {
            string sqlquery;
            try
            {
                dataGridView2.DataSource = null;
                string field = "";

                field = set_table_field_add();
                string cCond = string.Empty;
                if (tablename.ToUpper() == "AC_MAST")
                {
                    cCond = " Where Defa_ac=0 ";
                }
                if (tablename.ToUpper() == "SHIPTO")
                {
                    field = field.Replace("'", "''");
                    //sqlquery = "select " + field + " from Ac_Mast a Left Outer join " + tablename + " b on a.Ac_id=b.ac_id Where a.Defa_ac=0 ";
                    sqlquery = "EXECUTE USP_SHIPTO_EXCLDATA_EXPORT '" + field + "' ";
                }
                else if (tablename.ToUpper() == "BOMHEAD")
                {
                    sqlquery = "select rtrim(BomHead.bomid)+RIGHT('00000'+rtrim(CAST(BomHead.bomlevel as varchar)),5) as UniqueId," + field + " from BOMHEAD inner join bomdet on bomhead.Bomlevel=bomdet.Bomlevel and bomhead.bomid=bomdet.bomid ORDER BY  UniqueId,BomDetId ";
                }
                else if (tablename.ToUpper() == "AC_GROUP_MAST")
                {
                    string cField1 = this.set_table_field_add1();
                    sqlquery = "WITH AcGrpMast AS ( SELECT " + cField1 + " FROM AC_GROUP_MAST WHERE [GROUP]='' UNION ALL SELECT " + cField1.Replace("AC_GROUP_MAST.","A.") + " FROM AC_GROUP_MAST A INNER JOIN AcGrpMast B ON A.[GROUP]=B.AC_GROUP_NAME ) SELECT "+ field.Replace("AC_GROUP_MAST.","") +" FROM AcGrpMast";
                }
                else
                {
                    sqlquery = "select " + field + " from " + tablename + cCond;
                }

                dt_record = new DataTable();
                dt_record = oDataAccess.GetDataTable(sqlquery, null, 20);

                dataGridView2.DataSource = dt_record;

                dataGridView1.Size = new Size(435, panel1.Height - 90);
                dataGridView2.Size = new Size(panel1.Width - 655, panel1.Height - 90);
                foreach (DataGridViewColumn dc1 in dataGridView2.Columns)
                {
                    dc1.ReadOnly = true;
                }
                dataGridView2.AllowUserToResizeRows = false;
                dataGridView2.AllowUserToAddRows = false;
                label7.Text = "Total Records :" + dataGridView2.RowCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tablename == null || tablename == "")
            {
                MessageBox.Show("Please select the Master List.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            dataGridView2.Columns.Clear();

            set_field_setting();
            set_table_field();
            set_table_record();
        }

        public void set_field_setting()
        {
            string query = null;
            if (tablename.ToUpper() == "BOMHEAD")
            {
                query = "delete from ExpFldMap where TblName = 'BOMHEAD' or TblName = 'Bomdet'";
            }
            else
            {
                query = "delete from ExpFldMap where TblName='" + tablename + "'";
            }

            oDataAccess.ExecuteSQLStatement(query, null, 20, true);
            foreach (DataRow dr in dt_ExpFldMap1.Rows)
            {
                bool toexport;

                string tblname = dr[0].ToString().Trim();
                string fldname = dr[1].ToString().Trim();
                if (dr[2].ToString().Trim() == "False")
                {
                    toexport = false;
                }
                else
                {
                    toexport = true;
                }
                int sortorder = Int32.Parse(dr[3].ToString());

                string strSql5 = "insert into ExpFldMap values('" + tblname + "','" + fldname + "','" + toexport + "'," + sortorder + ",'" + dr[4].ToString() + "','" + dr[5].ToString() + "')";
                int insert = oDataAccess.ExecuteSQLStatement(strSql5, null, 20, true);
            }
            setenable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool val = false;
            //if (dt_ExpFldMap1.Rows.Count == 0)
            if (tablename == null || tablename == "")
            {
                MessageBox.Show("Please select the Master List.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            try
            {
                DialogResult res = openFileDialog1.ShowDialog();
                string filePath = openFileDialog1.FileName;
                string extension = Path.GetExtension(filePath);
                string file_name = Path.GetFileName(filePath);
                string field = string.Empty;
                //File selction Validation start
                string tblname = tablename.ToUpper();
                if (tblname == "AC_MAST")
                {
                    if (file_name == "Account_Master.xls")
                    {
                        val = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select 'Account_Master.xls' file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else if (tblname == "IT_MAST")
                {
                    if (file_name == "Item_Master.xls")
                    {
                        val = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select 'Item_Master.xls' file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else if (tblname == "SHIPTO")
                {
                    if (file_name == "Shipto_Master.xls")
                    {
                        val = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select 'Shipto_Master.xls' file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else if (tblname == "AC_GROUP_MAST")
                {
                    if (file_name == "Account_Group_Master.xls")
                    {
                        val = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select 'Account_Group_Master.xls' file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                else if (tblname == "BOMHEAD")
                {
                    if (file_name == "BOM_Master.xls")
                    {
                        val = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select 'BOM_Master.xls' file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                //File selction Validation  end

                if (val == true)
                {
                    if (res == DialogResult.OK)
                    {
                        set_table_field();

                        if (extension == ".xls")
                        {
                            Excel03Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                            this.getRecordsToExcel(filePath, file_name, extension);
                        }
                        if (extension == ".xlsx")
                        {
                            Excel07Con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES'";

                            this.getRecordsToExcel(filePath, file_name, extension);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        public void getRecordsToExcel(string file_path, string file_name, string extension)
        {
            string filePath = file_path;
            string extension1 = Path.GetExtension(filePath);
            string header = "YES"; /*rtobtnHeaderYes.Checked ? "YES" : "NO";*/
            string filename = Path.GetFileName(filePath);

            string conStr, sheetName;
            conStr = string.Empty;
            switch (extension1)
            {
                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03Con, filePath, header);
                    break;
                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel07Con, filePath, header);
                    break;
            }

            try
            {
                record = new DataTable();
                record.Columns.Add("Col", typeof(bool));
                record.Columns.Add("Field Name", typeof(string));
                record.Columns.Add("Caption", typeof(string));
                record.Columns.Add("Sort Order", typeof(int));

                extra = new DataTable();
                extra.Clear();
                extra.Columns.Add("", typeof(bool));
                extra.Columns.Add("Field Name", typeof(string));
                extra.Columns.Add("Caption", typeof(string));
                extra.Columns.Add("Sort Order");

                dt = new DataTable();
                MyConnection = new OleDbConnection(conStr);
                MyCommand = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                MyCommand.TableMappings.Add("Table", "TestTable");
                MyCommand.Fill(dt);
                MyConnection.Close();

                columnNames = dt.Columns.Cast<DataColumn>()
                                            .Select(x => x.ColumnName)
                                            .ToArray();
                Show_Dialog();

                dataGridView1.DataSource = dt_ExpFldMap1;

                this.dataGridView1.Sort(this.dataGridView1.Columns[4], ListSortDirection.Ascending);
                string cField = string.Empty;
                if (extra.Rows.Count > 0)
                {
                    foreach (DataRow r1 in extra.Rows)
                    {
                        cField += "," + r1[1].ToString();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (r1[1].ToString() == row.Cells[2].Value.ToString())
                            {
                                row.DefaultCellStyle.BackColor = Color.Yellow;
                                break;
                            }
                            if (cField.Contains(row.Cells[2].Value.ToString()) == false)
                            {
                                row.DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                }
                else
                {
                    string field = string.Empty;
                    field = set_table_field_add();
                    set_enable_after_Import();
                }

                if (result == 1)
                {
                    set_enable_after_Import();
                }

                result = 0;
            }
            catch (Exception ex)
            {

            }
        }

        public void Show_Dialog()
        {
            insert_field(tablename);
            set_table_field();

            int i = 0;
            string ctbl = string.Empty;
            string cfld = string.Empty;
            foreach (string s in columnNames)
            {
                int count = 0;
                foreach (DataRow row in dt_ExpFldMap1.Rows)
                {
                    if (s.ToString() == row[1].ToString() || s.ToString() == row[0].ToString() + "#" + row[1].ToString())
                    {
                        i++;
                        count++;
                        break;
                    }
                }
                string cc = count.ToString();
                if (count == 0)
                {
                    i++;
                    extra.Rows.Add("True", s.ToString(), s.ToString(), i);
                }
            }

            if (extra.Rows.Count > 0)
            {
                Dialog d = new Dialog();
                d.demo(extra, val, this.pFrmIcon);
                d.ShowDialog();
            }
            else
            {
                result = 1;
            }

            if (result == 1)
            {
                istemp_count = 1;
                foreach (DataRow dr in extra.Rows)
                {
                    dt_ExpFldMap1.Rows.Add(dt_table_name.Rows[grid_table_name.CurrentRow.Index][1].ToString(), dr[1].ToString(), "False", "0", dr[2].ToString(), "False");
                }
                foreach (DataRow dr in dt_ExpFldMap1.Rows)
                {
                    dr["ToExport"] = false;
                    dr["SortOrder"] = 0;
                    dr["IsTemplate"] = false;
                }

                i = 0;
                foreach (string s in columnNames)
                {
                    foreach (DataRow row in dt_ExpFldMap1.Rows)
                    {
                        if (s.ToString() == row[1].ToString() || s.ToString() == row[0].ToString() + "#" + row[1].ToString())
                        {
                            i++;
                            row["ToExport"] = "True";
                            row["SortOrder"] = i.ToString();
                            row["IsTemplate"] = "True";
                            break;
                        }
                    }
                }

                foreach (DataRow row in dt_ExpFldMap1.Rows)
                {
                    if (row["ToExport"].ToString() == "False")
                    {
                        i++;
                        row["SortOrder"] = i.ToString();
                    }
                }
            }
            if (result == 33)
            {
                insert_field(val);
                set_table_field();
                set_table_record();
                columnNames = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = dataGridView2.RowCount;

            if (count != 0)
            {
                if (istemp_count == 0)
                {
                    DialogResult result = MessageBox.Show("You have not used standard export template." + Environment.NewLine + "Do you still want to continue with export ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result.Equals(DialogResult.No))
                    {
                        return;
                    }
                }
                button1.PerformClick();
                progressBar1.Value = 0;
                progressBar1.Value = 10;
                label6.Visible = true;
                progressBar1.Visible = true;

                string startupPath = Application.StartupPath.ToString();
                string mainFolderPath = startupPath + "\\ExcelExport";
                Directory.CreateDirectory(mainFolderPath);
                filepath = mainFolderPath + "\\" + val;
                Directory.CreateDirectory(filepath);
                string date = DateTime.Now.ToString("yyyy-dd-MM HH.mm.ss");
                String name = val + " " + date;
                progressBar1.Value = 20;
                ExportToExcel(dataGridView2, name);
            }
            else
            {
                MessageBox.Show("Record not found to export", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        public void ExportToExcel(DataGridView gridviewID, string excelFilename)
        {
            try
            {
                label6.Text = "Opening Excel Application.....";
                progressBar1.Value = 30;

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                progressBar1.Value = 40;
                Microsoft.Office.Interop.Excel.Workbook wBook;

                progressBar1.Value = 50;
                Microsoft.Office.Interop.Excel.Worksheet wSheet;

                progressBar1.Value = 60;
                wBook = excel.Workbooks.Add(System.Reflection.Missing.Value);

                progressBar1.Value = 70;
                wSheet = (Microsoft.Office.Interop.Excel.Worksheet)wBook.ActiveSheet;

                progressBar1.Value = 80;
                System.Data.DataTable dt = dt_record;
                System.Data.DataColumn dc = new DataColumn();

                progressBar1.Value = 90;

                int colIndex = 0;
                float rowIndex = 1;
                progressBar1.Value = 100;

                foreach (DataColumn dcol in dt.Columns)
                {
                    colIndex = colIndex + 1;
                    excel.Cells[rowIndex, colIndex] = dcol.ColumnName;
                }

                progressBar1.Value = 0;
                rowIndex += 1;
                float nrow = 1;
                //rrg 
                excel.Columns.NumberFormat = "@";
                //end
                foreach (DataRow drow in dt.Rows)
                {
                    colIndex = 0;
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        excel.Cells[rowIndex, colIndex] = drow[dcol.ColumnName].ToString();

                    }

                    float aa = nrow == dt.Rows.Count ? 100.00f : (nrow / dt.Rows.Count) * 100;

                    progressBar1.Value = (Int32)aa;
                    progressBar1.Refresh();
                    progressBar1.Refresh();
                    label6.Text = "Read Records: " + nrow.ToString() + "/" + dt.Rows.Count;
                    nrow = nrow + 1;
                    rowIndex = rowIndex + 1;
                }
                progressBar1.Value = 100;
                progressBar1.Refresh();

                progressBar1.Visible = false;
                object misValue = System.Reflection.Missing.Value;

                excel.Columns.AutoFit();

                excel.ActiveWorkbook.SaveAs(filepath + "\\" + excelFilename + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //  excel.ActiveWorkbook.SaveAs(filepath + "\\" + excelFilename + ".xls", misValue, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                excel.ActiveWorkbook.Saved = true;

                progressBar1.Visible = false;
                label6.Visible = false;
                MessageBox.Show("Your excel file exported successfully at " + filepath + "\\" + excelFilename + ".xls", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("Excel Application not available", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                label6.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int count = 0;
            CheckBox headerBox = ((CheckBox)dataGridView1.Controls.Find("checkboxHeader", true)[0]);

            foreach (DataRow row in dt_ExpFldMap1.Rows)
            {
                if (bool.Parse(row["istemplate"].ToString()) == false)
                {
                    row["ToExport"] = headerBox.Checked;
                }
            }
        }

        public void setting_grid2()
        {
            try
            {
                foreach (DataGridViewColumn dc in dataGridView1.Columns)
                {
                    if (dc.Index.Equals(1))
                    {
                        dc.ReadOnly = true;
                    }
                }

                dataGridView1.Size = new Size(435, this.Height - 155);

                dataGridView1.Columns[0].HeaderText = "";
                dataGridView1.Columns[1].HeaderText = "Field Name";
                dataGridView1.Columns[2].HeaderText = "Sort Order";

                dataGridView1.AllowUserToResizeColumns = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.MultiSelect = false;
                dataGridView2.MultiSelect = false;

                Rectangle rect = dataGridView1.GetCellDisplayRectangle(0, -1, true);
                rect.Y = 4;
                rect.X = 20;
                CheckBox checkboxHeader = new CheckBox();
                checkboxHeader.Name = "checkboxHeader";

                checkboxHeader.Size = new Size(15, 15);
                checkboxHeader.Location = rect.Location;
                checkboxHeader.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
                dataGridView1.Controls.Add(checkboxHeader);

                dataGridView1.Size = new Size(435, panel1.Height - 90);

                dataGridView2.Size = new Size(panel1.Width - 655, panel1.Height - 90);
            }
            catch (Exception es)
            {
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = null;
            if (tablename == null || tablename == "")
            {
                MessageBox.Show("Please select the Master List.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Do you want to reset field list as per company Database?", this.pPApplText, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result.Equals(DialogResult.OK))
                {

                    if (tablename.ToUpper() == "BOMHEAD")
                    {
                        query = "delete from ExpFldMap where TblName = 'BOMHEAD' or TblName = 'Bomdet'";
                    }
                    else
                    {
                        query = "delete from ExpFldMap where TblName='" + tablename + "'";
                    }

                    oDataAccess.ExecuteSQLStatement(query, null, 20, true);
                    insert_field(val);
                    set_table_field();
                    set_table_record();
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int Xcor = dataGridView1.CurrentCellAddress.X;

            string ColumnName = dataGridView1.Columns[Xcor].Name;
            if (ColumnName == "Column3")
            {
                newsortorder = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                int count = dataGridView1.RowCount;

                if (Int32.Parse(newsortorder) > count)
                {
                    MessageBox.Show("Sort order should be between 1 to " + count.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dataGridView1.Rows[e.RowIndex].Cells[4].Value = oldsortorder;
                }
                else
                {
                    setgridorder(e.RowIndex, oldsortorder, newsortorder);
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int Xcor = dataGridView1.CurrentCellAddress.X;
            oldsortorder = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        public void setgridorder(int nRowIndex, string oldsortorder, string newsortorder)
        {
            int oldorder, neworder, sp, ep, i, rowIndex;
            oldorder = Int32.Parse(oldsortorder);
            neworder = Int32.Parse(newsortorder);
            if (oldorder > neworder)
            {
                sp = neworder;
                ep = oldorder - 1;

                for (i = ep; i >= sp; i--)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[4].Value.ToString().Equals(i.ToString()) && row.Index != nRowIndex)
                        {
                            rowIndex = row.Index;
                            dataGridView1.Rows[rowIndex].Cells[4].Value = Int32.Parse(dataGridView1.Rows[rowIndex].Cells[4].Value.ToString()) + 1;
                            neworder++;
                            break;
                        }
                    }
                }
            }
            else
            {
                sp = oldorder + 1;
                ep = neworder;
                for (i = ep; i >= sp; i--)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[4].Value.ToString().Equals(i.ToString()) && row.Index != nRowIndex)
                        {
                            rowIndex = row.Index;
                            dataGridView1.Rows[rowIndex].Cells[4].Value = Int32.Parse(dataGridView1.Rows[rowIndex].Cells[4].Value.ToString()) - 1;
                            neworder++;
                            break;
                        }
                    }
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        void set_enable_after_Import()
        {
            bool isPresent = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                isPresent = false;
                foreach (string s in columnNames)
                {
                    if (row.Cells[2].Value.ToString() == s.ToString() || row.Cells[1].Value.ToString() + "#" + row.Cells[2].Value.ToString() == s.ToString())
                    {
                        row.Cells[0].ReadOnly = true;
                        isPresent = true;
                        break;
                    }
                }
                if (isPresent == false)
                {
                    row.Cells[0].ReadOnly = false;
                    //RRG Edit
                    row.Cells[0].Value = false;
                }
            }
        }

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            //cAppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            cAppName = "ExportDataToFile.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        private void mDeleteProcessIdRecord()
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        public string set_table_field_add1()
        {
            string strSQL1;
            DataTable dt_fields = new DataTable();
            DataTable dt_fieldsacMast = new DataTable();
            strSQL1 = "select [FieldId]=b.name,[Field Name] = (Case when isnull(c.value, '')= '' then b.name else c.value end),b.System_Type_Id ,a.[name] from sys.tables a inner join sys.columns b on a.object_id = b.object_id left join sys.extended_properties c on a.object_id = c.major_id and b.column_id = c.minor_id and c.name = 'MS_Description' Where a.[name]='" + tablename + "' order by b.column_id";

            dt_fields = oDataAccess.GetDataTable(strSQL1, null, 20);

            int count;
            string cfield = string.Empty;
            foreach (DataRow row in dt_ExpFldMap1.Rows)
            {
                if (row["ToExport"].ToString().Trim() == "True")
                {
                    count = 0;
                    foreach (DataRow dr1 in dt_fields.Rows)
                    {
                        if (row[1].ToString().Trim() == dr1[0].ToString().Trim())
                        {
                            count++;
                            cfield = cfield + (cfield == string.Empty ? "" : ",") + row[0].ToString() + ".[" + row[1].ToString() + "] ";
                            break;
                        }
                    }
                    if (count == 0)
                    {
                        cfield = cfield + (cfield == string.Empty ? "" : ",") + "'' as [" + row[0].ToString() + "]";
                    }
                }
            }
            return cfield;
        }
    }
}
