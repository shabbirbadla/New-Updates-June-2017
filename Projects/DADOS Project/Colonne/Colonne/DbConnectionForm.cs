using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;


namespace Colonne
{
    public partial class DbConnectionForm : Form
    {
        SqlConnection connection = new SqlConnection();
        string connectionString;
        public DbConnectionForm()
        {
            InitializeComponent();
        }
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
        private void testConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                SetConnectionString();
                connection.ConnectionString = connectionString;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        MessageBox.Show("Test connection suncceded");
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        private void SetConnectionString()
        {
            try
            {
                string database = databaseComboBox.Text;
                if (database == string.Empty)
                {
                    database = "master";
                }
                if (windowsAuthenticationRadioButton.Checked == true)
                {
                    connectionString = "Data Source=" + serverComboBox.Text + ";Initial Catalog=" + database + ";Integrated Security=true;";
                }
                else
                {
                    connectionString = "Data Source=" + serverComboBox.Text + ";Initial Catalog=" + database + ";User Id=" + userIdTextBox.Text + ";Password=" + passwordTextBox.Text + ";";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void sqlAuthenticationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (sqlAuthenticationRadioButton.Checked == true)
                {
                    userIdTextBox.Enabled = true;
                    passwordTextBox.Enabled = true;
                }
                else
                {
                    userIdTextBox.Enabled = false;
                    passwordTextBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private DataSet GetDatabases()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection.ConnectionString = connectionString;
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                da.SelectCommand = new SqlCommand("SELECT name FROM sys.Databases", connection);
                da.Fill(ds, "sys.Databases");
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return ds;
        }

        private void databaseComboBox_Enter(object sender, EventArgs e)
        {
            try
            {
                if (windowsAuthenticationRadioButton.Checked == true)
                    connectionString = "Data Source=" + serverComboBox.Text + ";Initial Catalog=master;Integrated Security=true;";
                else
                    connectionString = "Data Source=" + serverComboBox.Text + ";Initial Catalog=master;User Id=" + userIdTextBox.Text + ";Password=" + passwordTextBox.Text + ";";
                databaseComboBox.DataSource = GetDatabases().Tables[0];
                databaseComboBox.DisplayMember = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (databaseComboBox.SelectedIndex != -1 && databaseComboBox.SelectedValue != null)
                {
                    SetConnectionString();
                    Columns dadosColumns = new Columns();
                    dadosColumns.ServerName = serverComboBox.Text;
                    dadosColumns.DataBaseName = databaseComboBox.Text;
                    dadosColumns.UserName = userIdTextBox.Text;
                    dadosColumns.Password = passwordTextBox.Text;
                    dadosColumns.ConnectionString = ConnectionString;

                    dadosColumns.Show(this);

                    this.Hide();
                }
                else
                {
                    MessageBox.Show(this, "Please Select Data Base", "Colonee - Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    databaseComboBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show(this, "You Want To Exit the application ???", "Colonee - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
                Application.Exit();
        }

        private void DbConnectionForm_Load(object sender, EventArgs e)
        {
            try
            {
                string[] Servers = Colonne.SqlLocator.GetServers();
                bool bindCombo = false;

                foreach (string server in Servers)
                {
                    if (server.Contains("local"))
                        bindCombo = true;
                }

                if (bindCombo == false)
                    serverComboBox.DataSource = Servers;
                else
                {
                    string[] ServerNames = Colonne.SqlLocator.GetServers();
                    serverComboBox.DataSource = ServerNames;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            serverComboBox.DataSource = null;
            try
            {
                string[] Servers = Colonne.SqlLocator.GetServers();
                bool bindCombo = false;

                foreach (string server in Servers)
                {
                    if (server.Contains("local"))
                        bindCombo = true;
                }

                if (bindCombo == false)
                    serverComboBox.DataSource = Servers;
                else
                {
                    string[] ServerNames = Colonne.SqlLocator.GetServers();
                    serverComboBox.DataSource = ServerNames;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Colonee - Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        #region Commented Code

        //DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

        //if (dbProviderFactory.CanCreateDataSourceEnumerator)
        //{
        //    DbDataSourceEnumerator dbDataSourceEnumerator = dbProviderFactory.CreateDataSourceEnumerator();

        //    if (dbDataSourceEnumerator != null)
        //    {
        //        DataTable dt = dbDataSourceEnumerator.GetDataSources();
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            serverComboBox.Items.Add(row[0]);
        //        }
        //    }
        //}

        #endregion
    }
}