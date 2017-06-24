using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;   

namespace Udyog.iTAX.FileUpload.Any.Format
{
    class Connection
    {
        private static string connectionstring;
        private static SqlConnection conn;
        private static SqlTransaction tran;

        static Connection()
        {
            connectionstring = "";
        }

        public string GetConnection()
        {
            return connectionstring;
        }

        public SqlConnection ConnectionOpen()
        {
            if (conn == null || conn.State == ConnectionState.Closed )
            {
                conn = new SqlConnection(GetConnection());
                conn.Open();
            }
            return conn;
        }

        public SqlConnection ConnectionClose()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return conn;
        }

        public SqlTransaction Transaction()
        {
            
            tran = conn.BeginTransaction();
            return tran;
        }

        public object Rollback()
        {
            if (tran != null)
            {
                tran.Rollback();
            }
            return tran;
        }

        public object Commit()
        {
            if (tran != null)
            {
                tran.Commit();
            }
            return tran;
        }


    }
}
