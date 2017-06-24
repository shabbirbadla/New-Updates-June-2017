using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient ;
using System.Text;
using System.Configuration;
using System.Web;
using Data.Acess.Layer;


namespace Data.Acess.Layer
{
    public class DataTier
    {

        // private static SqlTransaction tran1;
        private string dataBaseName;
        private string ServerName;
        private string UserName;
        private string Password; 
        private static SqlConnection conn1;
        Connection conn = new Connection();
        
        
        public DataTier()
        {
        }


        
        public string pServerName
        {
            get { return ServerName; }
            //set { ServerName = value; } 
            set { conn.pServerName = value; }
        }
        public string pPassword
        {
            get { return Password; }
            //set { Password = value; }
            set { conn.pPassword = value; }
        }
        public string pUserName
        {
            get { return UserName; }
            //set { UserName = value; }
            set { conn.pUserName = value; }
        }
        public string DataBaseName
        {
            get { return dataBaseName; }
            //set { dataBaseName = value; } 
            set { conn.DbName = value; }
        }

        // Create DataSet
        public DataSet ExecuteDataset(string StrSql, string tbName)
        {
            


            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(StrSql, conn1);

                DataSet ds = new DataSet();
                da.Fill(ds, tbName);

                return ds;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataset DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataset DataAcess Method |" +
                            EX.Message.Trim());
            }

        }

        public DataSet ExecuteDataset(string[] sqlStr)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlDataAdapter da;
                DataSet ds = new DataSet();
                string tbName = "";
                int i = 1;
                foreach (string str in sqlStr)
                {
                    if (str != null)
                    {
                        tbName = "Query" + i.ToString().Trim();
                        da = new SqlDataAdapter(str.Trim(), conn1);
                        da.Fill(ds, tbName);
                        i += 1;
                    }
                }
                return ds;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataset DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataset DataAcess Method |" +
                            EX.Message.Trim());
            }

        }

        public DataSet ExecuteDataset(DataSet ds, string StrSql, string tbName)
        {

            //conn.pServerName = this.pServerName;
            //conn.pUserName = this.pUserName;
            //conn.pPassword = this.pPassword;


            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                if (tbName == null || tbName == "")
                    tbName = "_query";

                SqlDataAdapter da = new SqlDataAdapter(StrSql, conn1);
                da.Fill(ds, tbName);
                return ds;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataset DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataset DataAcess Method |" +
                            EX.Message.Trim());
            }
        }
        public DataTable ExecuteDataTable(string storeprocedurename, SqlParameter[] spParam)
        {
            if (conn1 == null)
            {
                conn1 = conn.connOpen();
            }
            else
            {
                if (conn1.State == ConnectionState.Closed)
                {
                    conn1 = conn.connOpen();
                }
            }

            DataTable retTable;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeprocedurename;

            // Add parameters
            foreach (SqlParameter sp in spParam)
            {
                cmd.Parameters.Add(sp);
            }

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.SelectCommand.CommandTimeout = 100;
            cmd.Connection = conn1;

            DataSet ds = new DataSet();
            da.Fill(ds, "_Query");
            retTable = ds.Tables["_Query"];
            return retTable;
        }

        protected SqlDbType ParamDbType(string DbType)
        {
            SqlDbType SqlType = SqlDbType.VarChar;
            switch (DbType)
            {
                case "VARCHAR":
                    SqlType = SqlDbType.VarChar;
                    break;
                case "INT":
                    SqlType = SqlDbType.Int;
                    break;
                case "DECIMAL":
                    SqlType = SqlDbType.Decimal;
                    break;
                case "DATETIME":
                    SqlType = SqlDbType.DateTime;
                    break;
                case "NVARCHAR":
                    SqlType = SqlDbType.NVarChar;
                    break;
                case "CHAR":
                    SqlType = SqlDbType.Char;
                    break;
            }
            return SqlType;
        }

        public DataTable ExecuteDataTable(string strSql, string tableName)
        {
            try
            {
                DataTable retTable;
                DataSet Ds = new DataSet();
                Ds = ExecuteDataset(strSql, tableName);

                retTable = Ds.Tables[tableName];
                return retTable;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataTable DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataTable DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public DataTable ExecuteDataTable(string strSql, SqlTransaction tran)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                DataTable retTable;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(strSql, conn1);

                if (tran != null)
                    da.SelectCommand.Transaction = tran;

                da.Fill(ds, "retTable");
                retTable = ds.Tables["retTable"];
                return retTable;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataTable DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataTable DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        // Create Object of ExecuteDataReader
        public SqlDataReader ExecuteDataReader(string strSql)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 200;
                SqlDataReader dr;
                cmd.Connection = conn1;
                cmd.CommandText = strSql;
                dr = cmd.ExecuteReader();
                cmd.Dispose();
                return dr;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataReader DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataReader DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public SqlDataReader ExecuteDataReader(string strSql, SqlCommand cmd)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }
                SqlDataReader dr;
                if (cmd == null)
                    cmd = new SqlCommand();

                cmd.Connection = conn1;
                cmd.CommandText = strSql;
                dr = cmd.ExecuteReader();
                cmd.Dispose();
                return dr;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataReader DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataReader DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public SqlDataReader ExecuteDataReader(string storeprocedurename, SqlParameter[] sParam)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeprocedurename;
                cmd.Connection = conn1;
                // Add parameters
                foreach (SqlParameter sp in sParam)
                {
                    cmd.Parameters.Add(sp);
                }

                dr = cmd.ExecuteReader();

                return dr;
            }

            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataReader DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteDataReader DataAcess Method |" +
                            EX.Message.Trim());
            }
        }
        // Create Object of ExecuteScalar 
        // Execute the Query and return the First row of the Query
        public object ExecuteScalar(string strSql)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn1;
                cmd.CommandText = strSql;
                return cmd.ExecuteScalar();
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteScalar DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteScalar DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        //// Begin with Transaction
        //public SqlTransaction TranSaction()
        //{
        //    SqlTransaction tran;
        //    //if (tran1 == null)
        //    //{
        //        tran = conn1.BeginTransaction();

        //    //}
        //    return tran;
        //}

        public object CommitTransaction()
        {
            try
            {
                object _committran = 0;
                _committran = conn.Commit();
                return _committran;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In CommitTransaction DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In CommitTransaction DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public object RollBackTransaction()
        {
            try
            {
                object _rollbacktran = null;
                _rollbacktran = conn.Rollback();
                return _rollbacktran;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In RollBackTransaction DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In RollBackTransaction DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        // Executes a Transact - SQL against the Transaction 
        public SqlCommand ExecuteNonQuery(string strSql, string cmdType, bool BeginTran)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn1;
                cmd.CommandText = strSql;

                // Check Command Type
                if (cmdType == "SP")
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    if (cmdType == "TX")
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                }
                // end

                if (BeginTran == true)
                {
                    cmd.Transaction = conn.Transaction();

                }

                return cmd;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteNonQuery DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteNonQuery DataAcess Method |" +
                            EX.Message.Trim());
            }

        }

        // Executes a Nested Transact - SQL against the Transaction 
        public SqlCommand ExecuteNonQuery(SqlCommand cmd, string strSql, string cmdType, bool BeginTran)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                if (cmd.Connection == null)
                {
                    cmd.Connection = conn1;
                }

                cmd.CommandText = strSql;

                // Check Command Type
                if (cmdType == "SP")
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    if (cmdType == "TX")
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                }
                // end

                if (BeginTran == true && cmd.Transaction == null)
                {
                    cmd.Transaction = conn.Transaction();
                }

                return cmd;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteNonQuery DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteNonQuery DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public int ExecuteNonQuery(string strSql, string cmdType)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                int retResult = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn1;
                cmd.CommandText = strSql;

                // Check Command Type
                if (cmdType == "SP")
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    if (cmdType == "TX")
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                }
                // end


                retResult = cmd.ExecuteNonQuery();
                return retResult;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteNonQuery DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteNonQuery DataAcess Method |" +
                            EX.Message.Trim());
            }

        }
        //public string InsertStatment(string Flds,string TblName)
        //{
        //    string strInsert = "";
        //    strInsert = "Insert into " + TblName + "(" + Flds + ")";

        //}

        public SqlCommand ExecuteCommand(string strSql, string cmdType)
        {
            try
            {
                if (conn1 == null)
                {
                    conn1 = conn.connOpen();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = conn.connOpen();
                    }
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn1;
                cmd.CommandText = strSql;
                if (cmdType == "SP")
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    if (cmdType == "TX")
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                }
                return cmd;
            }
            catch (SqlException SqlEx)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteCommand DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                conn.connClose();
                throw new Exception("Error Found In ExecuteCommand DataAcess Method |" +
                            EX.Message.Trim());
            }
        }


        // Close the Connections
        public SqlConnection Connclose()
        {
            try
            {
                if (conn1.State == ConnectionState.Open)
                {
                    conn.connClose();
                }
                return conn1;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In Connclose DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In Connclose DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        // Generate update string using Datatable Parameter
        public string GenUpdateString(DataTable sourceTable,
        string targetTable,
        string[] exclude,
        string[] include,
        string cond,
        string[] keyFields)
        {
            /// <summary>
            /// Summary description for genUpdateString.
            /// Paramter Description
            /// -----------------------------------------------------------------------
            /// sourceTable : DataTable Parameter Type for Source Table
            /// targetTable : string type Parameter for Update Table
            /// exclude     : string array type Parameter for exclude fields from Source Table
            /// include     : string array type Parameter for include fields 
            ///               [ note : Datatable field will not include in the string, consider only
            ///                        this Parameter fields]
            /// keyFields   : string array type Parameter for Key condition fields
            /// cond        : string type Parameter for update condition
            /// -------------------------------------------------------------------------
            /// genUpdateString(acMast_vw, "brmain", new string[] { "tran_cd", "entry_ty" }, "", null, new string[] { "tran_cd","entry_ty"});
            /// ------------------------------------------------------------------------
            /// </summary>

            try
            {
                string upSqlStr = "";
                string upFlds = "";
                foreach (DataRow SourceRow in sourceTable.Rows)
                {
                    upFlds = "";

                    for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
                    {
                        bool xflag = false;

                        // exclude parameters blank
                        if (exclude != null)
                        {
                            foreach (string exField in exclude)
                            {
                                if (exField != null)
                                {
                                    if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        xflag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        // end

                        // include parameters blank
                        if (include != null)
                        {
                            foreach (string inField in include)
                            {
                                if (inField != null)
                                {
                                    if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        xflag = false;
                                        break;
                                    }
                                    else
                                    {
                                        xflag = true;
                                    }
                                }
                            }
                        }
                        // end

                        if (xflag == false)
                        {
                            if (upFlds.Trim() == "")
                                upFlds += "[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "] = ";
                            else
                                upFlds += ",[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "] = ";

                            switch (SourceRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                            {
                                case "SYSTEM.STRING":
                                    upFlds += "'" + Convert.ToString(SourceRow[sourceTable.Columns[i]]).Trim() + "'";
                                    break;
                                case "SYSTEM.DECIMAL":
                                    upFlds += (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToDecimal(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0);
                                    break;
                                case "SYSTEM.INT16":
                                    upFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToInt16(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0.00;
                                    break;
                                case "SYSTEM.INT32":
                                    upFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToInt32(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0;
                                    break;
                                case "SYSTEM.INT64":
                                    upFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToInt64(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0;
                                    break;
                                case "SYSTEM.DATETIME":
                                    upFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               "'" + DateFormat(Convert.ToDateTime(SourceRow[SourceRow.Table.Columns[i]])) + "'" :
                                               "'" + DateFormat(Convert.ToDateTime("01/01/1900")) + "'";
                                    break;
                                case "SYSTEM.BOOLEAN":
                                    upFlds += (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                        (Convert.ToBoolean(SourceRow[SourceRow.Table.Columns[i]]) == true ? 1 : 0) :
                                        0);
                                    break;
                            }
                        } // end (xFlag == false)
                    } // end For endfor
                    if (keyFields != null)
                    {
                        string condFld = "";
                        upSqlStr += "update " + targetTable.Trim() + " set " + upFlds.Trim() +
                                    " where ";

                        foreach (string keyField in keyFields)
                        {
                            if (keyField != null)
                            {
                                if (condFld == "")
                                    condFld = keyField.Trim().ToUpper() + " = ";
                                else
                                    condFld += " and " + keyField.Trim().ToUpper() + " = ";

                                switch (SourceRow.Table.Columns[keyField.Trim()].DataType.ToString().Trim().ToUpper())
                                {
                                    case "SYSTEM.STRING":
                                        condFld += "'" + Convert.ToString(SourceRow[keyField.Trim()]).Trim() + "'";
                                        break;
                                    case "SYSTEM.DECIMAL":
                                        condFld += Convert.ToDecimal(SourceRow[keyField.Trim()]);
                                        break;
                                    case "SYSTEM.INT16":
                                        condFld += Convert.ToInt16(SourceRow[keyField.Trim()]);
                                        break;
                                    case "SYSTEM.INT32":
                                        condFld += Convert.ToInt32(SourceRow[keyField.Trim()]);
                                        break;
                                    case "SYSTEM.INT64":
                                        condFld += Convert.ToInt64(SourceRow[keyField.Trim()]);
                                        break;
                                    case "SYSTEM.DATETIME":
                                        condFld += "'" + DateFormat(Convert.ToDateTime(SourceRow[keyField.Trim()])) + "'";
                                        break;
                                    case "SYSTEM.BOOLEAN":
                                        condFld += Convert.ToBoolean(SourceRow[keyField.Trim()]) == true ?
                                            1 : 0;
                                        break;
                                }
                            }
                        }

                        upSqlStr += condFld.Trim();
                    }
                    else
                    {
                        upSqlStr += "update " + targetTable.Trim() + " set " + upFlds.Trim() +
                        (cond.Trim() != "" ? " where " + cond.Trim() : "");
                    }
                }
                return upSqlStr;
            }
            catch (IndexOutOfRangeException IndEx)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                              IndEx.Message.Trim());
            }
            catch (NullReferenceException NullEx)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                             NullEx.Message.Trim());
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        // Generate update string for updated columns
        public string GenUpdateString(DataTable sourceTable,
                  DataTable compareTable,
                  string targetTable,
                  string cond)
        {
            try
            {
                string upSqlStr = "update " + targetTable.Trim() + " set ";
                string upFlds = "";
                for (int i = 0; i <= sourceTable.Columns.Count - 1; i++)
                {
                    switch (sourceTable.Columns[i].DataType.ToString().Trim().ToUpper())
                    {
                        case "SYSTEM.STRING":
                            if (Convert.ToString(sourceTable.Rows[0][sourceTable.Columns[i]]).Trim() !=
                                Convert.ToString(compareTable.Rows[0][compareTable.Columns[i]]).Trim())
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                            " = '" + Convert.ToString(sourceTable.Rows[0][sourceTable.Columns[i]]).Trim() + "'";
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                            " = '" + Convert.ToString(sourceTable.Rows[0][sourceTable.Columns[i]]).Trim() + "'";
                                }
                            }
                            break;
                        case "SYSTEM.DATETIME":
                            if ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? DateTime.MinValue : Convert.ToDateTime(sourceTable.Rows[0][sourceTable.Columns[i]])) !=
                                (DBNull.Value.Equals(compareTable.Rows[0][compareTable.Columns[i]]) ? DateTime.MinValue : Convert.ToDateTime(compareTable.Rows[0][compareTable.Columns[i]])))
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                        " = '" + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ?
                                                  Convert.ToString("01/01/1900") :
                                                  DateFormat(Convert.ToDateTime(sourceTable.Rows[0][sourceTable.Columns[i]]))) + "'";
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                            " = '" + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ?
                                            Convert.ToString("01/01/1900") :
                                            DateFormat(Convert.ToDateTime(sourceTable.Rows[0][sourceTable.Columns[i]]))) + "'";
                                }
                            }
                            break;

                        case "SYSTEM.DECIMAL":
                            if ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 : Convert.ToDecimal(sourceTable.Rows[0][sourceTable.Columns[i]])) !=
                                (DBNull.Value.Equals(compareTable.Rows[0][compareTable.Columns[i]]) ? 0 : Convert.ToDecimal(compareTable.Rows[0][compareTable.Columns[i]])))
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                            " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ?
                                            0 : Convert.ToDecimal(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 :
                                                Convert.ToDecimal(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                            }
                            break;
                        case "SYSTEM.INT16":
                            if ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0.0 : Convert.ToInt16(sourceTable.Rows[0][sourceTable.Columns[i]])) !=
                                (DBNull.Value.Equals(compareTable.Rows[0][compareTable.Columns[i]]) ? 0.0 : Convert.ToInt16(compareTable.Rows[0][compareTable.Columns[i]])))
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0.0 :
                                                Convert.ToInt16(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0.0 :
                                                Convert.ToInt16(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                            }
                            break;
                        case "SYSTEM.INT32":
                            if ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 : Convert.ToInt32(sourceTable.Rows[0][sourceTable.Columns[i]])) !=
                                (DBNull.Value.Equals(compareTable.Rows[0][compareTable.Columns[i]]) ? 0 : Convert.ToInt32(compareTable.Rows[0][compareTable.Columns[i]])))
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 :
                                                Convert.ToInt32(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 :
                                                Convert.ToInt32(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                            }
                            break;
                        case "SYSTEM.INT64":
                            if ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 : Convert.ToInt64(sourceTable.Rows[0][sourceTable.Columns[i]])) !=
                                (DBNull.Value.Equals(compareTable.Rows[0][compareTable.Columns[i]]) ? 0 : Convert.ToInt64(compareTable.Rows[0][compareTable.Columns[i]])))
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 :
                                                 Convert.ToInt64(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                        " = " + (DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? 0 :
                                                 Convert.ToInt64(sourceTable.Rows[0][sourceTable.Columns[i]]));
                                }
                            }
                            break;
                        case "SYSTEM.BOOLEAN":
                            if ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? false : Convert.ToBoolean(sourceTable.Rows[0][sourceTable.Columns[i]])) !=
                                (DBNull.Value.Equals(compareTable.Rows[0][compareTable.Columns[i]]) ? false : Convert.ToBoolean(compareTable.Rows[0][compareTable.Columns[i]])))
                            {
                                if (upFlds.Trim() != "")
                                {
                                    upFlds += "," + sourceTable.Columns[i].ColumnName.Trim() +
                                            " = " +
                                            ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? false : Convert.ToBoolean(sourceTable.Rows[0][sourceTable.Columns[i]])) == true ?
                                            1 : 0);
                                }
                                else
                                {
                                    upFlds += sourceTable.Columns[i].ColumnName.Trim() +
                                            " = " +
                                            ((DBNull.Value.Equals(sourceTable.Rows[0][sourceTable.Columns[i]]) ? false : Convert.ToBoolean(sourceTable.Rows[0][sourceTable.Columns[i]])) == true ?
                                            1 : 0);
                                }
                            }
                            break;
                    }
                }

                if (upFlds != "")
                {
                    upSqlStr = upSqlStr + upFlds +
                         (cond.Trim() != "" ? " where " + cond.Trim() : "");
                }
                else
                {
                    upSqlStr = "";
                }

                return upSqlStr;
            }
            catch (IndexOutOfRangeException IndEx)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                              IndEx.Message.Trim());
            }
            catch (NullReferenceException NullEx)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                             NullEx.Message.Trim());
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In GenUpdateString DataAcess Method |" +
                            EX.Message.Trim());
            }
        }
        // end

        public string GenInsertString(DataRow SourceRow,
           string targetTable,
           string[] exclude,
           string[] include)
        {

            /// <summary>
            /// Summary description for genInsertString.
            /// Generate Insert string of given Row
            /// Paramter Description
            /// -----------------------------------------------------------------------
            /// SourceRow   : DataRow Parameter Type for Source Row Name
            /// targetTable : string type Parameter for Insert Table Name
            /// exclude     : string array type Parameter for exclude fields in the string from Source Table
            /// include     : string array type Parameter for include fields 
            ///               [ note : Datatable field will not include in the string, consider only
            ///                        this Parameter fields]
            /// -------------------------------------------------------------------------
            /// genInsertString(acMast_Row, "brmain", null, new string[] { "tran_cd", "entry_ty" });  
            /// ------------------------------------------------------------------------
            /// </summary>

            try
            {
                string insSqlStr = "";
                string insFlds = "";


                insFlds = "";

                for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
                {
                    bool xflag = false;

                    // exclude parameters blank
                    if (exclude != null)
                    {
                        foreach (string exField in exclude)
                        {
                            if (exField != null)
                            {
                                if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                {
                                    xflag = true;
                                    break;
                                }
                            }
                        }
                    }
                    // end

                    // include parameters blank
                    if (include != null)
                    {
                        foreach (string inField in include)
                        {
                            if (inField != null)
                            {
                                if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                {
                                    xflag = false;
                                    break;
                                }
                                else
                                {
                                    xflag = true;
                                }
                            }
                        }
                    }
                    // end

                    if (xflag == false)
                    {
                        if (insFlds.Trim() == "")
                            insFlds += "[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "]";
                        else
                            insFlds += ",[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "]";
                    }
                }  // end generate Field string


                insSqlStr += "insert into " + targetTable.Trim() + "(" + insFlds + ") values (";

                insFlds = "";

                // Start extract field value string
                for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
                {
                    bool xflag = false;

                    // exclude parameters blank
                    if (exclude != null)
                    {
                        foreach (string exField in exclude)
                        {
                            if (exField != null)
                            {
                                if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                {
                                    xflag = true;
                                    break;
                                }
                            }
                        }
                    }
                    // end

                    // include parameters blank
                    if (include != null)
                    {
                        foreach (string inField in include)
                        {
                            if (inField != null)
                            {
                                if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                {
                                    xflag = false;
                                    break;
                                }
                                else
                                {
                                    xflag = true;
                                }
                            }
                        }
                    }
                    // end

                    if (xflag == false)
                    {
                        if (insFlds.Trim() != "")
                            insFlds += ",";


                        switch (SourceRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                        {
                            case "SYSTEM.STRING":
                                insFlds += "'" + Convert.ToString(SourceRow[SourceRow.Table.Columns[i]]).Trim() + "'";
                                break;
                            case "SYSTEM.DECIMAL":
                                insFlds += (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                           Convert.ToDecimal(SourceRow[SourceRow.Table.Columns[i]]) :
                                           0);
                                break;
                            case "SYSTEM.INT16":
                                insFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                           Convert.ToInt16(SourceRow[SourceRow.Table.Columns[i]]) :
                                           0.00;
                                break;
                            case "SYSTEM.INT32":
                                insFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                           Convert.ToInt32(SourceRow[SourceRow.Table.Columns[i]]) :
                                           0;
                                break;
                            case "SYSTEM.INT64":
                                insFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                           Convert.ToInt64(SourceRow[SourceRow.Table.Columns[i]]) :
                                           0;
                                break;
                            case "SYSTEM.DATETIME":

                                insFlds += "'" + (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                                  DateFormat(Convert.ToDateTime(SourceRow[SourceRow.Table.Columns[i]])) :
                                                  Convert.ToString("01/01/1900")) + "'";
                                break;
                            case "SYSTEM.BOOLEAN":
                                insFlds += (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                    (Convert.ToBoolean(SourceRow[SourceRow.Table.Columns[i]]) == true ? 1 : 0) :
                                    0);
                                break;
                        }
                    }
                }

                insSqlStr += insFlds + ")";

                return insSqlStr;
            }
            catch (IndexOutOfRangeException IndEx)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                              IndEx.Message.Trim());
            }
            catch (NullReferenceException NullEx)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                             NullEx.Message.Trim());
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                            EX.Message.Trim());
            }
        }


        public string GenInsertString(DataTable sourceTable,
           string targetTable,
           string[] exclude,
           string[] include)
        {

            /// <summary>
            /// Summary description for genInsertString.
            /// Generate Insert string for all rows of given paramter table
            /// Paramter Description
            /// -----------------------------------------------------------------------
            /// sourceTable : DataTable Parameter Type for Source Table Name
            /// targetTable : string type Parameter for Insert Table Name
            /// exclude     : string array type Parameter for exclude fields in the string from Source Table
            /// include     : string array type Parameter for include fields 
            ///               [ note : Datatable field will not include in the string, consider only
            ///                        this Parameter fields]
            /// -------------------------------------------------------------------------
            /// genInsertString(acMast_vw, "brmain", null, new string[] { "tran_cd", "entry_ty" });  
            /// ------------------------------------------------------------------------
            /// </summary>

            try
            {
                string insSqlStr = "";
                string insFlds = "";

                foreach (DataRow SourceRow in sourceTable.Rows)
                {
                    insFlds = "";

                    for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
                    {
                        bool xflag = false;

                        // exclude parameters blank
                        if (exclude != null)
                        {
                            foreach (string exField in exclude)
                            {
                                if (exField != null)
                                {
                                    if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        xflag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        // end

                        // include parameters blank
                        if (include != null)
                        {
                            foreach (string inField in include)
                            {
                                if (inField != null)
                                {
                                    if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        xflag = false;
                                        break;
                                    }
                                    else
                                    {
                                        xflag = true;
                                    }
                                }
                            }
                        }
                        // end

                        if (xflag == false)
                        {
                            if (insFlds.Trim() == "")
                                insFlds += "[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "]";
                            else
                                insFlds += ",[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "]";
                        }
                    }  // end generate Field string


                    insSqlStr += "insert into " + targetTable.Trim() + "(" + insFlds + ") values (";

                    insFlds = "";

                    // Start extract field value string
                    for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
                    {
                        bool xflag = false;

                        // exclude parameters blank
                        if (exclude != null)
                        {
                            foreach (string exField in exclude)
                            {
                                if (exField != null)
                                {
                                    if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        xflag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        // end

                        // include parameters blank
                        if (include != null)
                        {
                            foreach (string inField in include)
                            {
                                if (inField != null)
                                {
                                    if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        xflag = false;
                                        break;
                                    }
                                    else
                                    {
                                        xflag = true;
                                    }
                                }
                            }
                        }
                        // end

                        if (xflag == false)
                        {
                            if (insFlds.Trim() != "")
                                insFlds += ",";


                            switch (SourceRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                            {
                                case "SYSTEM.STRING":
                                    insFlds += "'" + Convert.ToString(SourceRow[sourceTable.Columns[i]]).Trim() + "'";
                                    break;
                                case "SYSTEM.DECIMAL":
                                    insFlds += (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToDecimal(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0);
                                    break;
                                case "SYSTEM.INT16":
                                    insFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToInt16(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0.00;
                                    break;
                                case "SYSTEM.INT32":
                                    insFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToInt32(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0;
                                    break;
                                case "SYSTEM.INT64":
                                    insFlds += !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                               Convert.ToInt64(SourceRow[SourceRow.Table.Columns[i]]) :
                                               0;
                                    break;
                                case "SYSTEM.DATETIME":
                                    insFlds += "'" + (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                                      DateFormat(Convert.ToDateTime(SourceRow[SourceRow.Table.Columns[i]])) :
                                                      Convert.ToString("01/01/1900")) + "'";
                                    break;
                                case "SYSTEM.BOOLEAN":
                                    insFlds += (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                        (Convert.ToBoolean(SourceRow[SourceRow.Table.Columns[i]]) == true ? 1 : 0) :
                                        0);
                                    break;
                            }
                        }
                    }

                    insSqlStr += insFlds + ")";

                }
                return insSqlStr;
            }
            catch (IndexOutOfRangeException IndEx)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                              IndEx.Message.Trim());
            }
            catch (NullReferenceException NullEx)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                             NullEx.Message.Trim());
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In GenInsertString DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public string DateFormat(DateTime date)
        {
            string _retdate = "01/01/1900";
            if (date != null && date > Convert.ToDateTime("01/01/1900"))
            {
                string strDate = "";
                strDate = Convert.ToString(date);
                string[] datearray = new string[3];
                datearray = strDate.Split('/');
                _retdate = datearray[1];
                _retdate += "/" + datearray[0];
                if (datearray[2].Length > 4)
                {
                    _retdate += "/" + datearray[2].Substring(0, 4);
                }
                else
                {
                    _retdate += "/" + datearray[2];
                }
            }

            return _retdate;
        }

        public string GenDeleteString(string targetTable,
            string cond)
        {
            try
            {
                string delStr = "Delete from " + targetTable.Trim() +
                        " where " + cond;

                return delStr;
            }
            catch (IndexOutOfRangeException IndEx)
            {
                throw new Exception("Error Found In GenDeleteString DataAcess Method |" +
                              IndEx.Message.Trim());
            }
            catch (NullReferenceException NullEx)
            {
                throw new Exception("Error Found In GenDeleteString DataAcess Method |" +
                             NullEx.Message.Trim());
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In GenDeleteString DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In GenDeleteString DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public DataRow ExactScatterGatherRow(DataRow scatterRow, DataRow gatherRow)
        {
            string scatterColname = "";
            for (int i = 0; i < scatterRow.Table.Columns.Count; i++)
            {
                scatterColname = scatterRow.Table.Columns[i].ColumnName.ToString().Trim();

                if (gatherRow.Table.Columns.Contains(scatterColname) == true)
                {
                    switch (scatterRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                    {
                        case "SYSTEM.STRING":
                            gatherRow[scatterColname] = Convert.ToString(scatterRow[i]);
                            break;
                        case "SYSTEM.DECIMAL":
                            gatherRow[scatterColname] = DBNull.Value.Equals(scatterRow[i]) ?
                                0 :
                                Convert.ToDecimal(scatterRow[i]);

                            break;
                        case "SYSTEM.INT16":
                            gatherRow[scatterColname] = DBNull.Value.Equals(scatterRow[i]) ?
                                0.0 :
                                Convert.ToInt16(scatterRow[i]);
                            break;
                        case "SYSTEM.INT32":
                            gatherRow[scatterColname] = DBNull.Value.Equals(scatterRow[i]) ?
                                0 :
                                Convert.ToInt32(scatterRow[i]);
                            break;
                        case "SYSTEM.INT64":
                            gatherRow[scatterColname] = DBNull.Value.Equals(scatterRow[i]) ?
                                0 :
                                Convert.ToInt64(scatterRow[i]);
                            break;
                        case "SYSTEM.DATETIME":
                            gatherRow[scatterColname] = DBNull.Value.Equals(scatterRow[i]) ?
                                Convert.ToDateTime("01/01/1900") :
                                Convert.ToDateTime(scatterRow[i]);
                            break;
                        case "SYSTEM.BOOLEAN":
                            gatherRow[scatterColname] = DBNull.Value.Equals(scatterRow[i]) ?
                                false :
                                Convert.ToBoolean(scatterRow[i]);
                            break;
                        default:
                            gatherRow[scatterColname] = scatterRow[i];
                            break;
                    }
                }
            }

            return gatherRow;
        }
        public DataRow ScatterGatherRow(DataRow scatterRow, DataRow gatherRow)
        {
            try
            {
                for (int i = 0; i < scatterRow.Table.Columns.Count; i++)
                {
                    switch (scatterRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                    {
                        case "SYSTEM.STRING":
                            gatherRow[i] = Convert.ToString(scatterRow[i]);
                            break;
                        case "SYSTEM.DECIMAL":
                            gatherRow[i] = DBNull.Value.Equals(scatterRow[i]) ?
                                0 :
                                Convert.ToDecimal(scatterRow[i]);

                            break;
                        case "SYSTEM.INT16":
                            gatherRow[i] = DBNull.Value.Equals(scatterRow[i]) ?
                                0.0 :
                                Convert.ToInt16(scatterRow[i]);
                            break;
                        case "SYSTEM.INT32":
                            gatherRow[i] = DBNull.Value.Equals(scatterRow[i]) ?
                                0 :
                                Convert.ToInt32(scatterRow[i]);
                            break;
                        case "SYSTEM.INT64":
                            gatherRow[i] = DBNull.Value.Equals(scatterRow[i]) ?
                                0 :
                                Convert.ToInt64(scatterRow[i]);
                            break;
                        case "SYSTEM.DATETIME":
                            gatherRow[i] = DBNull.Value.Equals(scatterRow[i]) ?
                                Convert.ToDateTime("01/01/1900") :
                                Convert.ToDateTime(scatterRow[i]);
                            break;
                        case "SYSTEM.BOOLEAN":
                            gatherRow[i] = DBNull.Value.Equals(scatterRow[i]) ?
                                false :
                                Convert.ToBoolean(scatterRow[i]);
                            break;
                        default:
                            gatherRow[i] = scatterRow[i];
                            break;
                    }

                }
                return gatherRow;
            }
            catch (IndexOutOfRangeException IndEx)
            {
                throw new Exception("Error Found In ScatterGatherRow DataAcess Method |" +
                              IndEx.Message.Trim());
            }
            catch (NullReferenceException NullEx)
            {
                throw new Exception("Error Found In ScatterGatherRow DataAcess Method |" +
                             NullEx.Message.Trim());
            }
            catch (SqlException SqlEx)
            {
                throw new Exception("Error Found In ScatterGatherRow DataAcess Method |" +
                            SqlEx.Message.Trim());
            }
            catch (Exception EX)
            {
                throw new Exception("Error Found In ScatterGatherRow DataAcess Method |" +
                            EX.Message.Trim());
            }
        }

        public DataTable ScatterGatherTable(DataTable scatterTable,
                        DataTable gatherTable, bool isCheck)
        {
            if (isCheck == false)
            {
                try
                {
                    foreach (DataRow scatterRow in scatterTable.Rows)
                    {
                        DataRow gatherRow = gatherTable.NewRow();
                        for (int i = 0; i < scatterTable.Columns.Count; i++)
                        {
                            switch (scatterTable.Columns[i].DataType.ToString().Trim().ToUpper())
                            {
                                case "SYSTEM.STRING":
                                    gatherRow[i] = Convert.ToString(scatterRow[scatterTable.Columns[i]]);
                                    break;
                                case "SYSTEM.DECIMAL":
                                    gatherRow[i] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                        0 :
                                        Convert.ToDecimal(scatterRow[scatterTable.Columns[i]]);

                                    break;
                                case "SYSTEM.INT16":
                                    gatherRow[i] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                        0.0 :
                                        Convert.ToInt16(scatterRow[scatterTable.Columns[i]]);
                                    break;
                                case "SYSTEM.INT32":
                                    gatherRow[i] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                        0 :
                                        Convert.ToInt32(scatterRow[scatterTable.Columns[i]]);
                                    break;
                                case "SYSTEM.INT64":
                                    gatherRow[i] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                        0 :
                                        Convert.ToInt64(scatterRow[scatterTable.Columns[i]]);
                                    break;
                                case "SYSTEM.DATETIME":
                                    gatherRow[i] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                        Convert.ToDateTime("01/01/1900") :
                                        Convert.ToDateTime(scatterRow[scatterTable.Columns[i]]);
                                    break;
                                case "SYSTEM.BOOLEAN":
                                    gatherRow[i] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                        false :
                                        Convert.ToBoolean(scatterRow[scatterTable.Columns[i]]);
                                    break;
                                default:
                                    gatherRow[i] = scatterRow[scatterTable.Columns[i]];
                                    break;
                            }
                        }
                        gatherTable.Rows.Add(gatherRow);
                    }
                    gatherTable.AcceptChanges();
                }
                catch (IndexOutOfRangeException IndEx)
                {
                    throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                  IndEx.Message.Trim());
                }
                catch (NullReferenceException NullEx)
                {
                    throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                 NullEx.Message.Trim());
                }
                catch (SqlException SqlEx)
                {
                    throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                SqlEx.Message.Trim());
                }
                catch (Exception EX)
                {
                    throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                EX.Message.Trim());
                }
            }
            else
            {
                if (isCheck == true)
                {
                    try
                    {
                        foreach (DataRow scatterRow in scatterTable.Rows)
                        {
                            DataRow gatherRow = gatherTable.NewRow();
                            for (int j = 0; j < gatherTable.Columns.Count; j++)
                            {
                                for (int i = 0; i < scatterTable.Columns.Count; i++)
                                {
                                    if (gatherTable.Columns[j].ColumnName.Trim().ToUpper() == scatterTable.Columns[i].ColumnName.Trim().ToUpper())
                                    {
                                        switch (scatterTable.Columns[i].DataType.ToString().Trim().ToUpper())
                                        {
                                            case "SYSTEM.STRING":
                                                gatherRow[j] = Convert.ToString(scatterRow[scatterTable.Columns[i]]);
                                                break;
                                            case "SYSTEM.DECIMAL":
                                                gatherRow[j] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                                    0 :
                                                    Convert.ToDecimal(scatterRow[scatterTable.Columns[i]]);

                                                break;
                                            case "SYSTEM.INT16":
                                                gatherRow[j] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                                    0.0 :
                                                    Convert.ToInt16(scatterRow[scatterTable.Columns[i]]);
                                                break;
                                            case "SYSTEM.INT32":
                                                gatherRow[j] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                                    0 :
                                                    Convert.ToInt32(scatterRow[scatterTable.Columns[i]]);
                                                break;
                                            case "SYSTEM.INT64":
                                                gatherRow[j] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                                    0 :
                                                    Convert.ToInt64(scatterRow[scatterTable.Columns[i]]);
                                                break;
                                            case "SYSTEM.DATETIME":
                                                gatherRow[j] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                                    Convert.ToDateTime("01/01/1900") :
                                                    Convert.ToDateTime(scatterRow[scatterTable.Columns[i]]);
                                                break;
                                            case "SYSTEM.BOOLEAN":
                                                gatherRow[j] = DBNull.Value.Equals(scatterRow[scatterTable.Columns[i]]) ?
                                                    false :
                                                    Convert.ToBoolean(scatterRow[scatterTable.Columns[i]]);
                                                break;
                                            default:
                                                gatherRow[j] = scatterRow[scatterTable.Columns[i]];
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }
                            gatherTable.Rows.Add(gatherRow);
                        }
                        gatherTable.AcceptChanges();
                    }
                    catch (IndexOutOfRangeException IndEx)
                    {
                        throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                      IndEx.Message.Trim());
                    }
                    catch (NullReferenceException NullEx)
                    {
                        throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                     NullEx.Message.Trim());
                    }
                    catch (SqlException SqlEx)
                    {
                        throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                    SqlEx.Message.Trim());
                    }
                    catch (Exception EX)
                    {
                        throw new Exception("Error Found In ScatterGatherTable DataAcess Method |" +
                                    EX.Message.Trim());
                    }

                }
            }
            return gatherTable;
        }

        public Exception SqlExceptionErrorsTraping(SqlException Ex)
        {
            string ErrorStr = "";
            switch (Ex.Number)
            {
                case 547:
                    ErrorStr = "Cannot Delete, Record already in used ";
                    break;
                case 208:
                    ErrorStr = Ex.Message;
                    break;
            }
            if (ErrorStr != "")
                throw new Exception(ErrorStr);
            else
                throw Ex;
        }
    }

    public class Connection
    {
        private static string connectionstringsWithPooling;
        private static string connectionstringsNonPooling;
        private static SqlConnection conn;
        private static SqlTransaction tran;


        private string dbName;
        private string ServerName;
        private string UserName;
        private string Password;

        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }


        static Connection()
        {
           

            //connectionstringsWithPooling = ConfigurationManager.ConnectionStrings["connectionPooling"].ConnectionString;
            //connectionstringsNonPooling = ConfigurationManager.ConnectionStrings["connectionNonPooling"].ConnectionString;
        }


        public string getPooledConnection()
        {
            // Rup if (DbName == "" || DbName == null)
            //    return connectionstringsWithPooling;
            //else
            //{
            //    //connectionstringsWithPooling.Substring(17+16,(connectionstringsWithPooling.IndexOf("UID")-1)-(17+16))
            //    string fndString = "Initial Catalog=";
            //    string fndDbName = "";
            //    fndDbName = connectionstringsWithPooling.Substring(connectionstringsWithPooling.IndexOf(fndString) + fndString.Length,
            //                              (connectionstringsWithPooling.IndexOf("UID") - 1) - (connectionstringsWithPooling.IndexOf(fndString) + fndString.Length));
            //    connectionstringsWithPooling = connectionstringsWithPooling.Replace(fndDbName.Trim(), DbName.Trim());

            //}
            if (DbName == "" || DbName == null)
            {
                return connectionstringsWithPooling;
            }
            else
            {
                //connectionString="Data Source=uday;Initial Catalog=s010809;UID=sa;Pwd=sa1985;Min Pool Size=5;Max Pool Size=60;Connect Timeout=2" providerName="System.Data.SqlClient";
                //connectionString="Data Source=udyog11;Initial Catalog=vudyog;UID=sa;Pwd=sa1985";
                ////connectionString="Data Source=uday;Initial Catalog=servicetax;UID=sa;Pwd=sa1985;Min Pool Size=5;Max Pool Size=60;Connect Timeout=2" providerName="System.Data.SqlClient"
                ////connectionstringsWithPooling.Substring(17+16,(connectionstringsWithPooling.IndexOf("UID")-1)-(17+16))
                //string fndString = "Initial Catalog=";
                //string fndDbName = "";
                //fndDbName = connectionstringsWithPooling.Substring(connectionstringsWithPooling.IndexOf(fndString) + fndString.Length,
                //                          (connectionstringsWithPooling.IndexOf("UID") - 1) - (connectionstringsWithPooling.IndexOf(fndString) + fndString.Length));
                //connectionstringsWithPooling = connectionstringsWithPooling.Replace(fndDbName.Trim(), DbName.Trim());

            }
           
            return connectionstringsWithPooling;
        }

        public string getNonPooledConnection()
        {
            if (DbName == "" || DbName == null)
                return connectionstringsNonPooling;
            else
            {
                string fndString = "Initial Catalog=";
                string fndDbName = "";
                fndDbName = connectionstringsNonPooling.Substring(connectionstringsNonPooling.IndexOf(fndString) + fndString.Length,
                                          (connectionstringsNonPooling.IndexOf("UID") - 1) - (connectionstringsNonPooling.IndexOf(fndString) + fndString.Length));
                connectionstringsNonPooling = connectionstringsNonPooling.Replace(fndDbName.Trim(), DbName.Trim());

            }

            return connectionstringsNonPooling;
        }

        public SqlConnection connOpen()
        {
            if (conn == null)
            {
                //Rup conn = new SqlConnecton(getPooledConnection());
                conn = new SqlConnection();
                conn.ConnectionString = "Data Source="+this.pServerName+";Initial Catalog=" + this.DbName + ";UID="+this.pUserName+";Pwd="+this.pPassword;
                //conn.ConnectionString = "Data Source=udyog11;Initial Catalog=" + this.DbName + ";UID=sa;Pwd=sa@1985";
                 //;Min Pool Size=5;Max Pool Size=60;Connect Timeout=2" providerName="System.Data.SqlClient"
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    //Rup conn = new SqlConnection(getNonPooledConnection());
                    conn.ConnectionString = "Data Source=" + this.pServerName + ";Initial Catalog=" + this.DbName + ";UID=" + this.pUserName + ";Pwd=" + this.pPassword;
                    //conn.ConnectionString = "Data Source=udyog11;Initial Catalog=" + this.DbName + ";UID=sa;Pwd=sa@1985";
                    conn.Open();
                }

            }
            else
            {
                if (conn.State == ConnectionState.Closed)
                {

                    //Rup conn = new SqlConnection(getPooledConnection());
                    conn.ConnectionString = "Data Source=" + this.pServerName + ";Initial Catalog=" + this.DbName + ";UID=" + this.pUserName + ";Pwd=" + this.pPassword;
                    //conn.ConnectionString = "Data Source=udyog11;Initial Catalog=" + this.DbName + ";UID=sa;Pwd=sa@1985";
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception)
                    {
                        //Rup conn = new SqlConnection(getNonPooledConnection());
                        conn.ConnectionString = "Data Source=" + this.pServerName + ";Initial Catalog=" + this.DbName + ";UID=" + this.pUserName + ";Pwd=" + this.pPassword;
                        //conn.ConnectionString = "Data Source=udyog11;Initial Catalog=" + this.DbName + ";UID=sa;Pwd=sa@1985";
                        conn.Open();
                    }
                }
            }
            return conn;
        }

        public SqlConnection connClose()
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
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
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
        public string pServerName
        {
            get { return ServerName; }
            set { ServerName = value; }
        }
        public string pPassword
        {
            get { return Password; }
            set { Password = value; }
        }
        public string pUserName
        {
            get { return UserName; }
            set { UserName = value; }
        }

      
    }

}
