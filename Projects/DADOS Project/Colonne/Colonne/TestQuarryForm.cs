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
    public partial class TestQuarryForm : Form
    {
        public TestQuarryForm()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection();
        SqlCommand command = null;
        SqlDataAdapter dataAdapter = null;
        DataSet dataset = null;

        private string mTestQuarry;
        /// <summary>
        /// Get Or Set Quarry
        /// </summary>
        public string TestQuarry
        {
            get { return mTestQuarry; }
            set { mTestQuarry = value; }
        }
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


        private void TestQuarryForm_Load(object sender, EventArgs e)
        {
            textBoxQuarry.Text = TestQuarry;
        }

        private void btnExicute_Click(object sender, EventArgs e)
        {
            try
            {
                connection.ConnectionString = ConnectionString;
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = textBoxQuarry.Text;
                dataAdapter = new SqlDataAdapter(command);
                dataset = new DataSet();
                dataAdapter.Fill(dataset);



                if (dataset != null && dataset.Tables.Count > 0)
                {
                    groupBox2.Visible = true;
                    tabPageResults.Show();
                    tabPageResults.Select();
                    listViewResults.Items.Clear();
                    string[] strColumns = new string[dataset.Tables[0].Columns.Count];
                    int count = 0;
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataColumn coll in dataset.Tables[0].Columns)
                        {
                            // Attach Subitems to the ListView
                            listViewResults.Columns.Add(coll.ColumnName, 150, HorizontalAlignment.Left);
                            strColumns[count] = coll.ColumnName;

                            count = count + 1;
                        }
                        count = 0;
                        foreach (DataRow drow in dataset.Tables[0].Rows)
                        {
                            // Define the list items
                            ListViewItem lvi = new ListViewItem(drow[strColumns[0].ToString()].ToString());
                            for (int i = 1; i < strColumns.Length; i++)
                            {
                                lvi.SubItems.Add(drow[strColumns[i].ToString()].ToString());
                            }
                            // Add the list items to the ListView
                            listViewResults.Items.Add(lvi);

                        }
                    }
                }
            }
            catch 
            {

            }
        }
    }
}