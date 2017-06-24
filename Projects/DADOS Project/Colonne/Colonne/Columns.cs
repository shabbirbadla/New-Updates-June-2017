using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Colonne
{
    public partial class Columns : Form
    {
        public Columns()
        {
            InitializeComponent();
        }

        #region Globle Veriables

        SqlConnection connection = new SqlConnection();
        SqlCommand command = null;
        SqlDataAdapter dataAdapter = null;
        DataSet dataset = null;
        SqlTransaction transaction = null;
        string insertDataBaseName = string.Empty;
        string strColumnName = string.Empty;
        string strColumnCaption = string.Empty;
        string strColumnDataType = string.Empty;
        string strColumnOrder = string.Empty;
        string strPrecision = string.Empty;
        string strIsGrouped = string.Empty;
        string strIsFreezing = string.Empty;
        string strColWidth = string.Empty;
        string strIsSummury = string.Empty;
        string strIsDisplayed = string.Empty;

        #endregion

        #region Form Properties

        private string mServerName;
        /// <summary>
        /// Get Or Set Data Base Server Name
        /// </summary>
        public string ServerName
        {
            get { return mServerName; }
            set { mServerName = value; }
        }
        private string mUserName;
        /// <summary>
        /// Get Or Set Data Base User NAme
        /// </summary>
        public string UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }
        private string mPassword;
        /// <summary>
        /// Get Or Set Data Base Password 
        /// </summary>
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }
        private string mDataBaseName;
        /// <summary>
        /// Get Or Set Data Base Name
        /// </summary>
        public string DataBaseName
        {
            get { return mDataBaseName; }
            set { mDataBaseName = value; }
        }
        private string mConnectionString;
        /// <summary>
        /// Get Or Set Connection String
        /// </summary>
        public string ConnectionString
        {
            get { return mConnectionString; }
            set { mConnectionString = value; }
        }

        #endregion

        private void Columns_Load(object sender, EventArgs e)
        {
            try
            {
                this.insertDataBaseName = DataBaseName;
                connection.ConnectionString = ConnectionString;
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT [QryID],[RepID],[RepLvlID],[RepQry] FROM " + DataBaseName + "..[usqry]";
                dataAdapter = new SqlDataAdapter(command);
                dataset = new DataSet();
                dataAdapter.Fill(dataset, "Report Quarrys");
                if (dataset != null)
                {
                    comboBoxQuarryID.DisplayMember = "QryID";
                    comboBoxQuarryID.ValueMember = "RepQry";
                    comboBoxQuarryID.DataSource = dataset.Tables["Report Quarrys"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            comboBoxQuarryID.Select();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", "Colonee - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
                Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ReportID = string.Empty;
            string ReportLevelType = string.Empty;
            string QuarryID = string.Empty;
            object objLastColid = null;
            string[] insertColIDs = null;
            btnGetColumns.Enabled = false;
            ColumnsdataGridView.Enabled = false;
            if (btnSave.Text == "Save")
            {
                try
                {
                    QuarryID = comboBoxQuarryID.Text;
                    connection.ConnectionString = ConnectionString;
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT [QryID],[RepID],[RepLvlID],[RepQry] FROM " + insertDataBaseName + "..[usqry] where [QryID]=" + QuarryID;
                    dataAdapter = new SqlDataAdapter(command);
                    dataset = new DataSet();
                    dataAdapter.Fill(dataset, "Report Quarrys");

                    if (dataset != null && dataset.Tables.Count > 0)
                    {
                        if (dataset.Tables["Report Quarrys"].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataset.Tables["Report Quarrys"].Rows)
                            {
                                ReportID = row["RepID"].ToString();
                                ReportLevelType = row["RepLvlID"].ToString();
                            }

                            if (ReportID != "" && ReportLevelType != "")
                            {

                                command = new SqlCommand();
                                command.Connection = connection;
                                command.CommandType = CommandType.Text;
                                command.CommandText = "select * from  " + insertDataBaseName + "..[uscrl] where [qryid]= " + QuarryID + " and repid = '" + ReportID + "'";
                                dataAdapter = new SqlDataAdapter(command);
                                DataSet DScrl = new DataSet();
                                dataAdapter.Fill(DScrl, "uscrl");
                                if (DScrl != null && DScrl.Tables.Count > 0)
                                {
                                    if (DScrl.Tables["uscrl"].Rows.Count > 0)
                                    {
                                        try
                                        {
                                            command = new SqlCommand();
                                            command.Connection = connection;
                                            connection.Open();
                                            command = connection.CreateCommand();
                                            transaction = connection.BeginTransaction();
                                            command.Connection = connection;
                                            command.Transaction = transaction;
                                            if (MessageBox.Show(this, "The Columns Are Already Exists With This Quarry, Are You Sure You Need To Update??", "Colonee - Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                            {
                                                DataTable uscolTable = new DataTable("uscol");
                                                uscolTable.Columns.Add("colid");
                                                uscolTable.Columns.Add("ColumnNames");
                                                uscolTable.Columns.Add("ColumnCaption");
                                                uscolTable.Columns.Add("ColumnDataType");
                                                uscolTable.Columns.Add("ColumnOrder");
                                                uscolTable.Columns.Add("Precision");
                                                DataRow uscolrow = null;
                                                foreach (DataRow usrow in DScrl.Tables["uscrl"].Rows)
                                                {
                                                    uscolrow = uscolTable.NewRow();
                                                    command.CommandText = "select * from  " + insertDataBaseName + "..[uscol] where [colid]= " + usrow["colid"].ToString();
                                                    if (connection.State == ConnectionState.Closed)
                                                        connection.Open();
                                                    SqlDataReader reader = null;
                                                    reader = command.ExecuteReader();

                                                    while (reader.Read())
                                                    {
                                                        uscolrow["colid"] = reader["colid"].ToString();
                                                        uscolrow["ColumnNames"] = reader["ColumnNames"].ToString();
                                                        uscolrow["ColumnCaption"] = reader["ColumnCaption"].ToString();
                                                        uscolrow["ColumnDataType"] = reader["ColumnDataType"].ToString();
                                                        uscolrow["ColumnOrder"] = reader["ColumnOrder"].ToString();
                                                        uscolrow["Precision"] = reader["Precision"].ToString();
                                                        uscolTable.Rows.Add(uscolrow);
                                                    }
                                                    reader.Dispose();
                                                }

                                                DScrl.Tables.Add(uscolTable);

                                                ArrayList dbList = new ArrayList();
                                                ArrayList dgvList = new ArrayList();

                                                ArrayList _dbList = new ArrayList();
                                                ArrayList _dgvList = new ArrayList();

                                                foreach (DataRow dbrow in DScrl.Tables["uscol"].Rows)
                                                {
                                                    dbList.Add(dbrow["ColumnNames"]);
                                                    _dbList.Add(dbrow["ColumnNames"]);
                                                }
                                                foreach (DataGridViewRow dgvrow in ColumnsdataGridView.Rows)
                                                {
                                                    dgvList.Add(dgvrow.Cells["ColumnName"].Value);
                                                    _dgvList.Add(dgvrow.Cells["ColumnName"].Value);
                                                }
                                                string _columnID = string.Empty;
                                                foreach (object item in dgvList)
                                                {
                                                    if (dbList.Contains(item))
                                                    {
                                                        #region for Update
                                                        DataRow[] dbrow = DScrl.Tables["uscol"].Select("ColumnNames='" + item.ToString().Trim() + "'");

                                                        foreach (DataRow _row in dbrow)
                                                        {
                                                            _columnID = _row["colid"].ToString();
                                                        }

                                                        if (_columnID != "")
                                                        {
                                                            int grindex = 0;// = dgvList.IndexOf(item);

                                                            for (int i = 0; i < ColumnsdataGridView.Rows.Count; i++)
                                                            {
                                                                if (item.ToString() == ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value.ToString())
                                                                {
                                                                    grindex = ColumnsdataGridView.Rows[i].Index;
                                                                    break;
                                                                }
                                                            }

                                                            if (ColumnsdataGridView.Rows[grindex].Cells["ColumnName"].Value == null)
                                                                strColumnName = Convert.ToString(DBNull.Value);
                                                            else
                                                                strColumnName = ColumnsdataGridView.Rows[grindex].Cells["ColumnName"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["ColumnCaption"].Value == null)
                                                                strColumnCaption = Convert.ToString(DBNull.Value);
                                                            else
                                                                strColumnCaption = ColumnsdataGridView.Rows[grindex].Cells["ColumnCaption"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value == null)
                                                                strColumnDataType = Convert.ToString(DBNull.Value);
                                                            else
                                                            {
                                                                if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("String", StringComparison.CurrentCultureIgnoreCase))
                                                                    strColumnDataType = "varchar";
                                                                else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("Int32", StringComparison.CurrentCultureIgnoreCase))
                                                                    strColumnDataType = "int";
                                                                else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("Decimal", StringComparison.CurrentCultureIgnoreCase))
                                                                    strColumnDataType = "numaric";
                                                                else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("DateTime", StringComparison.CurrentCultureIgnoreCase))
                                                                    strColumnDataType = "DateTime";
                                                                else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("Boolean", StringComparison.CurrentCultureIgnoreCase))
                                                                    strColumnDataType = "Bit";
                                                                else
                                                                    strColumnDataType = ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString();
                                                            }
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["ColumnOrder"].Value == null)
                                                                strColumnOrder = Convert.ToString(DBNull.Value);
                                                            else
                                                                strColumnOrder = ColumnsdataGridView.Rows[grindex].Cells["ColumnOrder"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["Precision"].Value == null)
                                                                strPrecision = Convert.ToString(DBNull.Value);
                                                            else
                                                                strPrecision = ColumnsdataGridView.Rows[grindex].Cells["Precision"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["IsGrouped"].Value == null)
                                                                strIsGrouped = Convert.ToString(false);
                                                            else
                                                                strIsGrouped = ColumnsdataGridView.Rows[grindex].Cells["IsGrouped"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["IsFreezing"].Value == null)
                                                                strIsFreezing = Convert.ToString(false);
                                                            else
                                                                strIsFreezing = ColumnsdataGridView.Rows[grindex].Cells["IsFreezing"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["ColWidth"].Value == null)
                                                                strColWidth = Convert.ToString(DBNull.Value);
                                                            else
                                                                strColWidth = ColumnsdataGridView.Rows[grindex].Cells["ColWidth"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["IsSummury"].Value == null)
                                                                strIsSummury = Convert.ToString(false);
                                                            else
                                                                strIsSummury = ColumnsdataGridView.Rows[grindex].Cells["IsSummury"].Value.ToString();
                                                            if (ColumnsdataGridView.Rows[grindex].Cells["IsDisplayed"].Value == null)
                                                                strIsDisplayed = Convert.ToString(false);
                                                            else
                                                                strIsDisplayed = ColumnsdataGridView.Rows[grindex].Cells["IsDisplayed"].Value.ToString();
                                                            command.CommandText = "UPDATE " + insertDataBaseName + "..[uscol] SET [ColumnNames] ='" + strColumnName + "',[ColumnCaption] = '" + strColumnCaption + "',[ColumnDataType] = '" + strColumnDataType + "',[ColumnOrder] = '" + strColumnOrder + "',[Precision] = '" + strPrecision + "' WHERE colid=" + _columnID;
                                                            command.ExecuteNonQuery();
                                                            command.CommandText = "UPDATE " + insertDataBaseName + "..[uscrl] SET [IsGrouped] = '" + strIsGrouped + "',[IsFreezing] = '" + strIsFreezing + "',[ColWidth] = '" + strColWidth + "',[IsSummury] = '" + strIsSummury + "',[IsDisplayed] = '" + strIsDisplayed + "'  WHERE colid=" + _columnID + " and repid='" + ReportID + "' and RepLvlID=" + ReportLevelType;
                                                            command.ExecuteNonQuery();
                                                        }
                                                        _dgvList.Remove(item);
                                                        _dbList.Remove(item);
                                                        #endregion
                                                    }//for update                                                    
                                                }

                                                foreach (object item in _dbList)
                                                {
                                                    #region deleteing data base table value
                                                    DataRow[] dbrow = DScrl.Tables["uscol"].Select("ColumnNames='" + item.ToString().Trim() + "'");

                                                    foreach (DataRow _row in dbrow)
                                                    {
                                                        _columnID = _row["colid"].ToString();
                                                    }

                                                    if (_columnID != "")
                                                    {
                                                        command.CommandText = "DELETE FROM " + insertDataBaseName + "..[uscol] WHERE colid=" + _columnID;
                                                        command.ExecuteNonQuery();
                                                        command.CommandText = "DELETE FROM " + insertDataBaseName + "..[uscrl] WHERE colid=" + _columnID;
                                                        command.ExecuteNonQuery();
                                                    }
                                                    #endregion
                                                }

                                                command.CommandText = "select top 1 colid from  " + insertDataBaseName + "..[uscol] order by colid DESC";
                                                objLastColid = command.ExecuteScalar();
                                                int lastColid = Convert.ToInt32(objLastColid);

                                                foreach (object item in _dgvList)
                                                {
                                                    #region Insering values
                                                    int grindex = 0;
                                                    for (int i = 0; i < ColumnsdataGridView.Rows.Count; i++)
                                                    {
                                                        if (item.ToString().Trim() == ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value.ToString().Trim())
                                                        {
                                                            grindex = ColumnsdataGridView.Rows[i].Index;
                                                            break;
                                                        }
                                                    }
                                                    lastColid = lastColid + 1;
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["ColumnName"].Value == null)
                                                        strColumnName = Convert.ToString(DBNull.Value);
                                                    else
                                                        strColumnName = ColumnsdataGridView.Rows[grindex].Cells["ColumnName"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["ColumnCaption"].Value == null)
                                                        strColumnCaption = Convert.ToString(DBNull.Value);
                                                    else
                                                        strColumnCaption = ColumnsdataGridView.Rows[grindex].Cells["ColumnCaption"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value == null)
                                                        strColumnDataType = Convert.ToString(DBNull.Value);
                                                    else
                                                    {
                                                        if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("String", StringComparison.CurrentCultureIgnoreCase))
                                                            strColumnDataType = "varchar";
                                                        else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("Int32", StringComparison.CurrentCultureIgnoreCase))
                                                            strColumnDataType = "int";
                                                        else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("Decimal", StringComparison.CurrentCultureIgnoreCase))
                                                            strColumnDataType = "numaric";
                                                        else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("DateTime", StringComparison.CurrentCultureIgnoreCase))
                                                            strColumnDataType = "DateTime";
                                                        else if (ColumnsdataGridView.Rows[grindex].Cells["ColumnDataType"].Value.ToString().Equals("Boolean", StringComparison.CurrentCultureIgnoreCase))
                                                            strColumnDataType = "Bit";
                                                    }
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["ColumnOrder"].Value == null)
                                                        strColumnOrder = Convert.ToString(DBNull.Value);
                                                    else
                                                        strColumnOrder = ColumnsdataGridView.Rows[grindex].Cells["ColumnOrder"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["Precision"].Value == null)
                                                        strPrecision = Convert.ToString(DBNull.Value);
                                                    else
                                                        strPrecision = ColumnsdataGridView.Rows[grindex].Cells["Precision"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["IsGrouped"].Value == null)
                                                        strIsGrouped = Convert.ToString(false);
                                                    else
                                                        strIsGrouped = ColumnsdataGridView.Rows[grindex].Cells["IsGrouped"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["IsFreezing"].Value == null)
                                                        strIsFreezing = Convert.ToString(false);
                                                    else
                                                        strIsFreezing = ColumnsdataGridView.Rows[grindex].Cells["IsFreezing"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["ColWidth"].Value == null)
                                                        strColWidth = Convert.ToString(DBNull.Value);
                                                    else
                                                        strColWidth = ColumnsdataGridView.Rows[grindex].Cells["ColWidth"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["IsSummury"].Value == null)
                                                        strIsSummury = Convert.ToString(false);
                                                    else
                                                        strIsSummury = ColumnsdataGridView.Rows[grindex].Cells["IsSummury"].Value.ToString();
                                                    if (ColumnsdataGridView.Rows[grindex].Cells["IsDisplayed"].Value == null)
                                                        strIsDisplayed = Convert.ToString(false);
                                                    else
                                                        strIsDisplayed = ColumnsdataGridView.Rows[grindex].Cells["IsDisplayed"].Value.ToString();

                                                    command.CommandText = "INSERT INTO " + insertDataBaseName + "..[uscol]([ColID],[ColumnNames],[ColumnCaption],[ColumnDataType],[ColumnOrder],[Precision]) VALUES (" + lastColid + ",'" + strColumnName + "','" + strColumnCaption + "','" + strColumnDataType + "'," + strColumnOrder + ",'" + strPrecision + "')";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "INSERT INTO " + insertDataBaseName + "..[uscrl]([ColID],[QryID],[RepID],[RepLvlID],[IsGrouped],[IsFreezing],[ColWidth],[IsSummury],[IsDisplayed]) VALUES (" + lastColid + "," + QuarryID + ",'" + ReportID + "'," + ReportLevelType + ",'" + strIsGrouped.ToLower() + "','" + strIsFreezing.ToLower() + "'," + strColWidth + ",'" + strIsSummury.ToLower() + "','" + strIsDisplayed.ToLower() + "')";
                                                    command.ExecuteNonQuery();
                                                    #endregion
                                                }
                                            }
                                            else
                                            {

                                            }

                                            if (connection.State == ConnectionState.Open)
                                            {
                                                transaction.Commit();
                                                MessageBox.Show(this, "Records Updated Sucssfully", "Colonee - Sucsses", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                                                command.CommandText = "select crl.colid,crl.repid,crl.qryid,col.ColumnNames,col.ColumnCaption,col.ColumnDataType,col.ColumnOrder,col.Precision,crl.isGrouped,crl.isFreezing,crl.colwidth,crl.isSummury,crl.isDisplayed from uscrl crl,uscol col where crl.colid=col.colid and crl.repid='" + ReportID + "' and crl.qryid= " + QuarryID + " order by crl.colid";

                                                dataAdapter = new SqlDataAdapter(command);

                                                DataSet SucssDataset = new DataSet();

                                                dataAdapter.Fill(SucssDataset);

                                                if (SucssDataset != null && SucssDataset.Tables.Count > 0)
                                                {
                                                    if (SucssDataset.Tables[0].Rows.Count > 0)
                                                    {
                                                        ColumnsdataGridView.Rows.Clear();
                                                        int index = 0;
                                                        foreach (DataRow sRow in SucssDataset.Tables[0].Rows)
                                                        {
                                                            ColumnsdataGridView.Rows.Add();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnName"].Value = sRow["ColumnNames"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnCaption"].Value = sRow["ColumnCaption"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnDataType"].Value = sRow["ColumnDataType"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnOrder"].Value = sRow["ColumnOrder"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["Precision"].Value = sRow["Precision"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColWidth"].Value = sRow["colwidth"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsGrouped"].Value = sRow["isGrouped"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsFreezing"].Value = sRow["isFreezing"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsSummury"].Value = sRow["isSummury"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsDisplayed"].Value = sRow["isDisplayed"].ToString();
                                                            index = index + 1;
                                                        }
                                                    }
                                                }
                                            }

                                        }//try
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                            if (transaction != null)
                                                transaction.Rollback();
                                        }
                                        finally
                                        {
                                            if (connection.State == ConnectionState.Open)
                                                connection.Close();
                                            btnSave.Text = "Edit";
                                        }
                                    }
                                    else
                                    {
                                        command = new SqlCommand();
                                        command.Connection = connection;
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = "select top 1 colid from  " + insertDataBaseName + "..[uscol] order by colid DESC";
                                        connection.Open();
                                        objLastColid = command.ExecuteScalar();
                                        insertColIDs = new string[ColumnsdataGridView.Rows.Count];
                                        int lastColid = 0;
                                        if (objLastColid == null)
                                        {
                                            lastColid = 1;
                                            objLastColid = "1";
                                        }

                                        else
                                        {
                                            lastColid = Convert.ToInt32(objLastColid);
                                        }


                                        connection.Close();
                                        if (objLastColid != null)
                                        {
                                            connection.Open();
                                            command = connection.CreateCommand();
                                            transaction = connection.BeginTransaction();
                                            command.Connection = connection;
                                            command.Transaction = transaction;
                                            command.CommandType = CommandType.Text;

                                            for (int i = 0; i < ColumnsdataGridView.Rows.Count; i++)
                                            {
                                                lastColid = lastColid + 1;
                                                insertColIDs[i] = lastColid.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value == null)
                                                    strColumnName = Convert.ToString(DBNull.Value);
                                                else
                                                    strColumnName = ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["ColumnCaption"].Value == null)
                                                    strColumnCaption = Convert.ToString(DBNull.Value);
                                                else
                                                    strColumnCaption = ColumnsdataGridView.Rows[i].Cells["ColumnCaption"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value == null)
                                                    strColumnDataType = Convert.ToString(DBNull.Value);
                                                else
                                                {
                                                    if (ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value.ToString().Equals("String", StringComparison.CurrentCultureIgnoreCase))
                                                        strColumnDataType = "varchar";
                                                    else if (ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value.ToString().Equals("Int32", StringComparison.CurrentCultureIgnoreCase))
                                                        strColumnDataType = "int";
                                                    else if (ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value.ToString().Equals("Decimal", StringComparison.CurrentCultureIgnoreCase))
                                                        strColumnDataType = "numaric";
                                                    else if (ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value.ToString().Equals("DateTime", StringComparison.CurrentCultureIgnoreCase))
                                                        strColumnDataType = "DateTime";
                                                    else if (ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value.ToString().Equals("Boolean", StringComparison.CurrentCultureIgnoreCase))
                                                        strColumnDataType = "Bit";
                                                }
                                                if (ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value == null)
                                                    strColumnOrder = Convert.ToString(DBNull.Value);
                                                else
                                                    strColumnOrder = ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["Precision"].Value == null)
                                                    strPrecision = Convert.ToString(DBNull.Value);
                                                else
                                                    strPrecision = ColumnsdataGridView.Rows[i].Cells["Precision"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["IsGrouped"].Value == null)
                                                    strIsGrouped = Convert.ToString(false);
                                                else
                                                    strIsGrouped = ColumnsdataGridView.Rows[i].Cells["IsGrouped"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["IsFreezing"].Value == null)
                                                    strIsFreezing = Convert.ToString(false);
                                                else
                                                    strIsFreezing = ColumnsdataGridView.Rows[i].Cells["IsFreezing"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value == null)
                                                    strColWidth = Convert.ToString(DBNull.Value);
                                                else
                                                    strColWidth = ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["IsSummury"].Value == null)
                                                    strIsSummury = Convert.ToString(false);
                                                else
                                                    strIsSummury = ColumnsdataGridView.Rows[i].Cells["IsSummury"].Value.ToString();
                                                if (ColumnsdataGridView.Rows[i].Cells["IsDisplayed"].Value == null)
                                                    strIsDisplayed = Convert.ToString(false);
                                                else
                                                    strIsDisplayed = ColumnsdataGridView.Rows[i].Cells["IsDisplayed"].Value.ToString();

                                                command.CommandText = "INSERT INTO " + insertDataBaseName + "..[uscol]([ColID],[ColumnNames],[ColumnCaption],[ColumnDataType],[ColumnOrder],[Precision]) VALUES (" + lastColid + ",'" + strColumnName + "','" + strColumnCaption + "','" + strColumnDataType + "'," + strColumnOrder + ",'" + strPrecision + "')";
                                                command.ExecuteNonQuery();
                                                command.CommandText = "INSERT INTO " + insertDataBaseName + "..[uscrl]([ColID],[QryID],[RepID],[RepLvlID],[IsGrouped],[IsFreezing],[ColWidth],[IsSummury],[IsDisplayed]) VALUES (" + lastColid + "," + QuarryID + ",'" + ReportID + "'," + ReportLevelType + ",'" + strIsGrouped.ToLower() + "','" + strIsFreezing.ToLower() + "'," + strColWidth + ",'" + strIsSummury.ToLower() + "','" + strIsDisplayed.ToLower() + "')";
                                                command.ExecuteNonQuery();
                                            }
                                            if (connection.State == ConnectionState.Open)
                                            {
                                                transaction.Commit();
                                                MessageBox.Show(this, "Records Inserted Sucssfully", "Colonee - Sucsses", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                                                command.CommandText = "select crl.colid,crl.repid,crl.qryid,col.ColumnNames,col.ColumnCaption,col.ColumnDataType,col.ColumnOrder,col.Precision,crl.isGrouped,crl.isFreezing,crl.colwidth,crl.isSummury,crl.isDisplayed from uscrl crl,uscol col where crl.colid=col.colid and crl.repid='" + ReportID + "' and crl.qryid= " + QuarryID + " order by crl.colid";

                                                dataAdapter = new SqlDataAdapter(command);

                                                DataSet SucssDataset = new DataSet();

                                                dataAdapter.Fill(SucssDataset);

                                                if (SucssDataset != null && SucssDataset.Tables.Count > 0)
                                                {
                                                    if (SucssDataset.Tables[0].Rows.Count > 0)
                                                    {
                                                        ColumnsdataGridView.Rows.Clear();
                                                        int index = 0;
                                                        foreach (DataRow sRow in SucssDataset.Tables[0].Rows)
                                                        {
                                                            ColumnsdataGridView.Rows.Add();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnName"].Value = sRow["ColumnNames"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnCaption"].Value = sRow["ColumnCaption"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnDataType"].Value = sRow["ColumnDataType"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColumnOrder"].Value = sRow["ColumnOrder"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["Precision"].Value = sRow["Precision"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["ColWidth"].Value = sRow["colwidth"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsGrouped"].Value = sRow["isGrouped"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsFreezing"].Value = sRow["isFreezing"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsSummury"].Value = sRow["isSummury"].ToString();
                                                            ColumnsdataGridView.Rows[index].Cells["IsDisplayed"].Value = sRow["isDisplayed"].ToString();
                                                            index = index + 1;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }//
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    if (transaction != null)
                        transaction.Rollback();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    btnSave.Text = "Edit";
                }
            }
            else if (btnSave.Text == "Edit")
            {
                ColumnsdataGridView.Enabled = true;
                btnSave.Text = "Save";
            }
        }

        private void btnGetColumns_Click(object sender, EventArgs e)
        {
            string commendText = comboBoxQuarryID.SelectedValue.ToString();
            btnGetColumns.Enabled = false;
            btnSave.Enabled = true;
            btnSave.Focus();
            ColumnsdataGridView.Enabled = true;
            ColumnsdataGridView.Rows.Clear();
            string[] strArgus = null;
            try
            {
                if (comboBoxQuarryID.SelectedValue != null)
                {
                    connection.ConnectionString = ConnectionString;
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    if (commendText.Contains("@"))
                    {
                        int DeleteValue = commendText.ToUpper().IndexOf("where".ToUpper());
                        commendText = commendText.Substring(0, DeleteValue);
                    }

                    command.CommandText = commendText;
                    dataAdapter = new SqlDataAdapter(command);
                    DataSet ColumnsDataSet = new DataSet();
                    dataAdapter.Fill(ColumnsDataSet, "Columns");

                    command.CommandText = "select usrl.ColID as [ColumnID],usrl.QryID as [QueryID],usrl.RepID as [ReportID],usrl.RepLvlID as [LevelID],usrl.IsGrouped as [IsGrouped],usrl.IsFreezing as [IsFreezing],usrl.ColWidth as [ColumnWidth],usrl.IsSummury as [IsSummury],usrl.IsDisplayed as [IsDisplayed],usc.ColumnNames as [ColumnName],usc.ColumnCaption as [ColumnCaption],usc.ColumnDataType as [ColumnDataType],usc.ColumnOrder as [ColumnOrder],usc.Precision as [Precision] from uscrl usrl inner join uscol usc on usrl.colid=usc.colid where usrl.qryid=" + comboBoxQuarryID.Text + " order by usrl.colid";
                    dataAdapter = new SqlDataAdapter(command);
                    DataSet DBDataSet = new DataSet();
                    dataAdapter.Fill(DBDataSet, "DBColumns");
                    ArrayList dbcolumnNames = new ArrayList();
                    ArrayList quarryName = new ArrayList();

                    DataColumnCollection column = ColumnsDataSet.Tables["Columns"].Columns;

                    if (ColumnsDataSet != null && ColumnsDataSet.Tables.Count > 0)
                    {
                        foreach (DataColumn col in column)
                        {
                            quarryName.Add(col.ColumnName);
                        }
                    }

                    if (DBDataSet != null && DBDataSet.Tables.Count > 0)
                    {
                        if (DBDataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dbrow in DBDataSet.Tables[0].Rows)
                            {
                                dbcolumnNames.Add(Convert.ToString(dbrow["ColumnName"]));
                            }
                        }

                    }

                    if (ColumnsDataSet != null && ColumnsDataSet.Tables.Count > 0)
                    {
                        for (int i = 0; i < ColumnsDataSet.Tables["Columns"].Columns.Count; i++)
                        {
                            if (DBDataSet != null && DBDataSet.Tables.Count > 0)
                            {
                                if (DBDataSet.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dbrow in DBDataSet.Tables[0].Rows)
                                    {
                                        if (column[i].ColumnName.ToString().Trim() == dbrow["ColumnName"].ToString().Trim())
                                        {
                                            ColumnsdataGridView.Rows.Add();
                                            ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value = Convert.ToString(dbrow["ColumnName"]);
                                            ColumnsdataGridView.Rows[i].Cells["ColumnCaption"].Value = Convert.ToString(dbrow["ColumnCaption"]);
                                            ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value = Convert.ToString(dbrow["ColumnDataType"]);
                                            ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value = Convert.ToString(dbrow["ColumnOrder"]);
                                            ColumnsdataGridView.Rows[i].Cells["Precision"].Value = Convert.ToString(dbrow["Precision"]);
                                            ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value = Convert.ToString(dbrow["ColumnWidth"]);
                                            ColumnsdataGridView.Rows[i].Cells["IsGrouped"].Value = Convert.ToString(dbrow["IsGrouped"]);
                                            ColumnsdataGridView.Rows[i].Cells["IsFreezing"].Value = Convert.ToString(dbrow["IsFreezing"]);
                                            ColumnsdataGridView.Rows[i].Cells["IsSummury"].Value = Convert.ToString(dbrow["IsSummury"]);
                                            ColumnsdataGridView.Rows[i].Cells["IsDisplayed"].Value = Convert.ToString(dbrow["IsDisplayed"]);
                                            dbcolumnNames.Remove(Convert.ToString(dbrow["ColumnName"]));
                                            quarryName.Remove(Convert.ToString(dbrow["ColumnName"]));
                                        }
                                    }
                                }
                                else
                                {
                                    ColumnsdataGridView.Rows.Add();
                                    ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value = column[i].ColumnName;
                                    ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value = column[i].DataType.Name;
                                    ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value = i + 1;
                                    ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value = 250;
                                    quarryName.Remove(Convert.ToString(column[i].ColumnName));
                                }
                            }
                            else
                            {
                                ColumnsdataGridView.Rows.Add();
                                ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value = column[i].ColumnName;
                                ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value = column[i].DataType.Name;
                                ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value = i + 1;
                                ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value = 250;
                                quarryName.Remove(Convert.ToString(column[i].ColumnName));
                            }
                        }
                        if (quarryName.Count > 0)
                        {
                            int i = ColumnsdataGridView.RowCount;
                            for (int j = 0; j < quarryName.Count; i++)
                            {
                                i = i + 1;
                                ColumnsdataGridView.Rows.Add();
                                ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value = column[i].ColumnName;
                                ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value = column[i].DataType.Name;
                                ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value = i + 1;
                                ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value = 250;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please Select Quarry", "Colonee - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void linkLabelEditQuarry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TestQuarryForm tqf = new TestQuarryForm();
            tqf.TestQuarry = textBoxQuarry.Text;
            tqf.ConnectionString = this.ConnectionString;
            tqf.UserName = this.UserName;
            tqf.Password = this.Password;
            tqf.ServerName = this.ServerName;
            tqf.DataBaseName = this.DataBaseName;
            tqf.ShowDialog(this);
        }

        private void linkLabelChangeDatabase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void comboBoxQuarryID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxQuarryID.SelectedIndex != -1)
            {
                textBoxQuarry.Text = comboBoxQuarryID.SelectedValue.ToString();
                btnGetColumns.Enabled = true;
                if (btnSave.Text == "Edit")
                {
                    btnSave.Text = "Save";
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
        }

        private void btnTestQuarry_Click(object sender, EventArgs e)
        {
            TestQuarryForm tqf = new TestQuarryForm();
            tqf.TestQuarry = textBoxQuarry.Text;
            tqf.ConnectionString = this.ConnectionString;
            tqf.UserName = this.UserName;
            tqf.Password = this.Password;
            tqf.ServerName = this.ServerName;
            tqf.DataBaseName = this.DataBaseName;
            tqf.ShowDialog(this);
        }
    }
}