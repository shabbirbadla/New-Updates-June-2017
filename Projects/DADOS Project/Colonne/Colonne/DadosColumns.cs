using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Colonne
{
    public partial class DadosColumns : Form
    {
        public DadosColumns()
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

        private void DadosColumns_Load(object sender, EventArgs e)
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
                    lbQuarry.DisplayMember = "RepQry";
                    lbQuarry.ValueMember = "QryID";
                    lbQuarry.DataSource = dataset.Tables["Report Quarrys"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            lbQuarry.Select();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", "Colonee - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
                Application.Exit();
        }

        private void btnGetColumns_Click(object sender, EventArgs e)
        {
            string commendText = lbQuarry.Text;
            ColumnsdataGridView.Rows.Clear();
            try
            {
                if (lbQuarry.SelectedValue != null)
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

                    if (ColumnsDataSet != null && ColumnsDataSet.Tables.Count > 0)
                    {
                        DataColumnCollection column = ColumnsDataSet.Tables["Columns"].Columns;
                        for (int i = 0; i < ColumnsDataSet.Tables["Columns"].Columns.Count; i++)
                        {
                            ColumnsdataGridView.Rows.Add();
                            ColumnsdataGridView.Rows[i].Cells["ColumnName"].Value = column[i].ColumnName;
                            ColumnsdataGridView.Rows[i].Cells["ColumnDataType"].Value = column[i].DataType.Name;
                            ColumnsdataGridView.Rows[i].Cells["ColumnOrder"].Value = i + 1;
                            ColumnsdataGridView.Rows[i].Cells["ColWidth"].Value = 150;
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

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ReportID = string.Empty;
            string ReportLevelType = string.Empty;
            string QuarryID = string.Empty;
            object objLastColid = null;
            string[] insertColIDs = null;

            try
            {
                QuarryID = lbQuarry.SelectedValue.ToString();

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
                            command.CommandText = "select * from  " + insertDataBaseName + "..[uscrl] where [qryid]= " + QuarryID + " and repid = " + ReportID;
                            dataAdapter = new SqlDataAdapter(command);
                            DataSet DScrl = new DataSet();
                            dataAdapter.Fill(DScrl);
                            if (DScrl != null && DScrl.Tables.Count > 0)
                            {
                                if (DScrl.Tables[0].Rows.Count > 0)
                                    MessageBox.Show(this, "the Columns are already Exists with this quarry", "Colonee - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                else
                                {
                                    command = new SqlCommand();
                                    command.Connection = connection;
                                    command.CommandType = CommandType.Text;
                                    command.CommandText = "select top 1 colid from  " + insertDataBaseName + "..[uscol] order by colid DESC";
                                    connection.Open();
                                    objLastColid = command.ExecuteScalar();
                                    insertColIDs = new string[ColumnsdataGridView.Rows.Count];
                                    int lastColid = Convert.ToInt32(objLastColid);
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
                                            command.CommandText = "INSERT INTO " + insertDataBaseName + "..[uscrl]([ColID],[QryID],[RepID],[RepLvlID],[IsGrouped],[IsFreezing],[ColWidth],[IsSummury],[IsDisplayed]) VALUES (" + lastColid + "," + QuarryID + "," + ReportID + "," + ReportLevelType + ",'" + strIsGrouped.ToLower() + "','" + strIsFreezing.ToLower() + "'," + strColWidth + ",'" + strIsSummury.ToLower() + "','" + strIsDisplayed.ToLower() + "')";
                                            command.ExecuteNonQuery();
                                        }
                                        if (connection.State == ConnectionState.Open)
                                        {
                                            transaction.Commit();
                                            MessageBox.Show(this, "Records Inserted Sucssfully", "Colonee - Sucsses", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            }
        }
    }
}