using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;


namespace Dados_Report_Wizard
{
    class FunClass
    {
        SqlConnection con;
        SqlCommand command;
        SqlDataReader reader;
        DataTable myDataTable;
        SqlTransaction trIn;
        SqlCommand cmdIn;
        public SqlConnection getConnection()
        {
            try
            {
                string x = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
                con = new SqlConnection(x);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "DadosReport Wizard", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
        }
        public DataTable getDataTable(SqlConnection conn, String query)
        {
            try
            {
                command = new SqlCommand(query, con);
                reader = command.ExecuteReader();
                myDataTable = new DataTable();
                myDataTable.Load(reader);
                reader.Close();
                return myDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "DadosReport Wizard", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
        }
        public void GetRecord(String qury, SqlConnection con)
        {
            trIn = con.BeginTransaction();
            try
            {
                cmdIn = new SqlCommand(qury, con, trIn);
                cmdIn.ExecuteScalar();
                trIn.Commit();
               
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error : " + ex.Message, "DadosReport Wizard", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                trIn.Rollback();
            }
            catch (Exception)
            {
                trIn.Rollback();

            }
        }
    }
}
