using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Data.Common;
using System.Text;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;

///''''''''''''''''''''''''''''''''''''''''''''''''''''''
///'''''Written by: VIJAY MESA  '''''''''''''''''''
///'''''            UdyogSoftware.com '''''''''''''''''''
///''''''''''''''''''''''''''''''''''''''''''''''''''''''

#region "Enums"

public enum OperationType
{

    SqlStatement,

    StoredProcedure

}

/// <summary>
/// The type of connection
/// </summary>
/// <remarks></remarks>
public enum DatabaseType : short
{

    /// <summary>
    /// Oracle
    /// </summary>
    /// <remarks></remarks>
    Oracle = 0,

    /// <summary>
    /// SqlServer
    /// </summary>
    /// <remarks></remarks>
    SqlServer = 1,

    Generic = 5

}

#endregion

/// <summary>
/// Data Access.  Handles connection / transaction management and execution of queries and stored procedures.
/// </summary>
/// <remarks></remarks>
/// 
namespace DataAccess_Net
{
    public class clsDataAccess
    {
        #region "Fields"

        private DbConnection _objConnection;
        private DatabaseType _enumDatabaseType;
        // To detect redundant calls
        private bool _bIsDisposed = false;
        public static string _databaseName;
        public static string _serverName;
        public static string _userID;
        public static string _password;

        public DbTransaction _objTransaction;
        /// <summary>
        /// The default timeout for queries
        /// </summary>
        /// <remarks></remarks>

        private const int DEFAULT_TIMEOUT = 30;
        private const string PROVIDER_SQLSERVER = "SYSTEM.SQL.SQLCLIENT";

        private const string PROVIDER_ORACLE = "ORACLE";
        private const string PARAMETER_PREFIX_ORACLE = "";

        private const string PARAMETER_PREFIX_SQLSERVER = "";

        #endregion

        #region "Construction"

        //// <summary>
        //// Creates a new connection
        //// </summary>
        //// <remarks></remarks>
        //public clsDataAccess()
        //{
        //    //TODO: If for some bizarre reason getting the connection information from configuration is slow,
        //    //      put the information in a shared member.

        //    //Get the configuration settings
        //    ConnectionStringSettings objConnectionStringSettings = ConfigurationManager.ConnectionStrings["TestDB"];

        //    //Make sure that we got something
        //    if (objConnectionStringSettings == null)
        //    {
        //        throw new Exception("unable to find the connection string");
        //    }
        //    //Get the database type
        //    switch (objConnectionStringSettings.ProviderName.ToUpper())
        //    {
        //        case PROVIDER_ORACLE:
        //            _enumDatabaseType = DatabaseType.Oracle;
        //            _objConnection = new OracleConnection(objConnectionStringSettings.ConnectionString);
        //            break;
        //        case PROVIDER_SQLSERVER:
        //            _enumDatabaseType = DatabaseType.SqlServer;
        //            _objConnection = new SqlConnection(objConnectionStringSettings.ConnectionString);
        //            break;
        //        default:

        //            throw new Exception(string.Format("Invalid configuration. Invalid value {0} for database type.", objConnectionStringSettings.ProviderName));
        //    }
        //}


        public clsDataAccess()
        {
            string _connectionString = string.Empty;
            _connectionString = "Server='" + serverName + "';";
            _connectionString += "Initial Catalog='" + databaseName + "';";
            _connectionString += "User ID='" + userID + "';";
            _connectionString += "Password='" + password + "';";
            
            if (string.IsNullOrEmpty(_connectionString) != null)
            {
                _enumDatabaseType = DatabaseType.SqlServer;
                _objConnection = new SqlConnection(_connectionString);
            }
        }

        #endregion

        #region "Properties"

        public string databaseName
        {
            get { return _databaseName; }
            set { _databaseName = databaseName; }
        }

        public string serverName
        {
            get { return _serverName; }
            set { _serverName = serverName; }
        }

        public string userID
        {
            get { return _userID; }
            set { _userID = userID; }
        }

        public string password
        {
            get { return _password; }
            set { _password = password; }
        }

        //public string connectionString
        //{
        //    get
        //    {
        //        connectionString += "Data Source=" + serverName + ";";
        //        connectionString += "Initial Catalog=" + databaseName + ";";
        //        connectionString += "User ID=" + userID + ";";
        //        connectionString += "Password=" + password + ";";
        //        return connectionString;
        //    }
        //    set
        //    {
        //        connectionString += "Data Source=" + serverName + ";";
        //        connectionString += "Initial Catalog=" + databaseName + ";";
        //        connectionString += "User ID=" + userID + ";";
        //        connectionString += "Password=" + password + ";";
        //    }
        //}

        //oSqlConn.pdbname = "E021011";
        //oSqlConn.pSrvname = "udyog11";
        //oSqlConn.pSrvusername = "sa";
        //oSqlConn.pSrvuserpassword = "sa@1985";

        /// <summary>
        /// Gets the type of database being used.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DatabaseType enumDatabaseType
        {
            get
            {
                CheckDisposed();
                return _enumDatabaseType;
            }
        }

        /// <summary>
        /// Gets the current level of transaction (if there is one)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbTransaction objCurrentTransaction
        {
            get { return _objTransaction; }
        }

        /// <summary>
        /// Gets whether ot not a transaction is in progress.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool InTransaction
        {
            get { return _objTransaction != null; }
        }

        #endregion

        #region "Connection / Factory Methods"

        /// <summary>
        /// Gets an (open) connection
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbConnection GetConnection()
        {

            //Only open the connection once

            if (_objConnection.State != ConnectionState.Open)
            {
                //Open the connection
                _objConnection.Open();

            }

            return _objConnection;

        }

        /// <summary>
        /// Creates a command for execution.  Defaults to CommandType.Text.  Connection Property is already set.
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbCommand CreateCommand(string strSql)
        {

            DbCommand objCommand = this.GetConnection().CreateCommand();

            objCommand.CommandType = CommandType.Text;

            if (!string.IsNullOrEmpty(strSql))
            {
                objCommand.CommandText = strSql;
            }

            objCommand.Transaction = _objTransaction;

            return objCommand;

        }

        /// <summary>
        /// Makes each parameter name unique. This has the side effect of ensuring we're not using reserved words in Oracle.
        /// </summary>
        /// <remarks></remarks>
        private void MakeParameterNamesUnique(List<clsParam> colParams)
        {
            if (colParams != null)
            {
                int intIndex = 0;

                foreach (clsParam objParam in colParams)
                {
                    objParam.Name = objParam.Name + intIndex.ToString();
                    intIndex += 1;
                }
            }

        }

        /// <summary>
        /// Creates an parameter.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbParameter CreateParameter()
        {

            switch (_enumDatabaseType)
            {
                case DatabaseType.Oracle:
                    return new OracleParameter();
                case DatabaseType.SqlServer:
                    return new SqlParameter();
                default:
                    throw new Exception("Unexpected value for DatabaseType");
            }

        }

        private string GetDBParameterName(string stringSuppliedName, OperationType enumOperationType)
        {

            switch (enumOperationType)
            {
                case OperationType.SqlStatement:

                    //SQL Statement
                    switch (this.enumDatabaseType)
                    {
                        case DatabaseType.Oracle:
                            return ":" + stringSuppliedName;
                        case DatabaseType.SqlServer:
                            return "@" + stringSuppliedName;
                        default:
                            throw new Exception("Unexpected database type");
                    }

                    break;
                case OperationType.StoredProcedure:

                    //Stored Procedure
                    switch (this.enumDatabaseType)
                    {
                        case DatabaseType.Oracle:
                            return stringSuppliedName;
                        case DatabaseType.SqlServer:
                            return "@" + stringSuppliedName;
                        default:
                            throw new Exception("Unexpected database type");
                    }

                    break;
                default:
                    throw new Exception("Unexpected operation type");
            }


        }

        private DbParameter CreateParameter(clsParam objParam, OperationType enumOperationType)
        {

            DbParameter objParameter = this.CreateParameter();

            //Get the name for the parameter
            objParameter.ParameterName = GetDBParameterName(objParam.Name, enumOperationType);

            switch (objParam.ParamType)
            {
                case clsParam.pType.pDate:
                    objParameter.DbType = DbType.DateTime;
                    break;
                case clsParam.pType.pLong:
                    objParameter.DbType = DbType.Int32;
                    break;
                case clsParam.pType.pFloat:
                    objParameter.DbType = DbType.Decimal;
                    break;
                case clsParam.pType.pString:
                    objParameter.DbType = DbType.String;
                    objParameter.Size = ParamLen(objParam);
                    break;
                case clsParam.pType.pOracleCLOB:
                    objParameter = new System.Data.OracleClient.OracleParameter(GetDBParameterName(objParam.Name, enumOperationType), OracleType.Clob, Convert.ToInt16(objParam.Value));
                    break;
                //objParamORACLOB.Direction = ParameterDirection.Input

                default:
                    throw new Exception("Unexpected data type");
            }

            switch (objParam.InOrOut)
            {
                case clsParam.pInOut.pIn:
                    objParameter.Direction = ParameterDirection.Input;
                    objParameter.Value = WrapNull(objParam);
                    break;
                case clsParam.pInOut.pOut:
                    objParameter.Direction = ParameterDirection.Output;
                    break;
                default:
                    throw new Exception("Unexpected parameter direction");
            }

            return objParameter;

        }

        /// <summary>
        /// Returns the correct kind of data adapter for this connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbDataAdapter CreateDataAdapter(DbCommand objSelectCommand)
        {

            DbDataAdapter objDataAdapter = default(DbDataAdapter);

            switch (_enumDatabaseType)
            {
                case DatabaseType.Oracle:
                    objDataAdapter = new OracleDataAdapter();
                    break;
                case DatabaseType.SqlServer:
                    objDataAdapter = new SqlDataAdapter();
                    break;
                default:
                    throw new Exception("Unexpected value for DatabaseType");
            }

            if (objSelectCommand != null)
            {
                objDataAdapter.SelectCommand = objSelectCommand;
            }

            return objDataAdapter;
        }

        #endregion

        #region "Transaction Methods"

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <remarks></remarks>
        public void BeginTransaction()
        {
            if (_objTransaction != null)
            {
                throw new Exception("A transaction has already been started.");
            }

            //Create the transaction
            _objTransaction = this.GetConnection().BeginTransaction();

        }

        /// <summary>
        /// Commits the current level of transaction.
        /// </summary>
        /// <remarks></remarks>
        public void CommitTransaction()
        {
            if (_objTransaction == null)
            {
                throw new Exception("A transaction has not been started.");
            }

            //Commit the transaction
            _objTransaction.Commit();

            //Dispose of the transaction
            _objTransaction.Dispose();
            _objTransaction = null;

        }

        /// <summary>
        /// Explicitly rolls back a transaction.
        /// </summary>
        /// <remarks></remarks>
        public void RollbackTransaction()
        {
            if (_objTransaction == null)
            {
                throw new Exception("A transaction has not been started.");
            }

            //Commit the transaction
            _objTransaction.Rollback();

            //Dispose of the transaction
            _objTransaction.Dispose();
            _objTransaction = null;
        }

        #endregion

        #region "Data Access Methods"

        /// <summary>
        /// Gets a single data row.  Recreates the colParams parameter.
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="colParams"></param>
        /// <param name="iNumSecsTilTimeout"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataRow GetDataRow(string strSQL, List<clsParam> colParams, Int16 iNumSecsTilTimeout)
        {

            CheckDisposed();
            if (string.IsNullOrEmpty(strSQL))
                throw new ArgumentNullException("strSQL");

            DataTable dt = null;

            try
            {
                dt = GetDataTable(strSQL, colParams, iNumSecsTilTimeout);

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            return null;

        }

        public System.Xml.XmlDataDocument GetXMLDataSet(string strSQL, List<clsParam> colParams, Int16 iNumSecsTilTimeout)
        {

            CheckDisposed();
            if (string.IsNullOrEmpty(strSQL))
                throw new ArgumentNullException("strSQL");

            DataSet ds = null;
            System.Xml.XmlDataDocument xmlDoc = null;
            System.Xml.XmlDeclaration xmlDec = null;

            try
            {
                ds = GetDataSet(strSQL, colParams, iNumSecsTilTimeout);

                //*** create xml data document with xml declaration
                xmlDoc = new System.Xml.XmlDataDocument(ds);
                xmlDoc.DataSet.EnforceConstraints = false;
                xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.PrependChild(xmlDec);

                return xmlDoc;

            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            finally
            {
                xmlDec = null;
                ds = null;
            }

            return null;

        }

        /// <summary>
        /// Prepares a command (inserts the proper coding for parameters, etc)
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="colParams"></param>
        /// <param name="iNumSecsTilTimeout"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbCommand PrepareCommand(string strSQL, List<clsParam> colParams, CommandType enumCommandType, Int16 iNumSecsTilTimeout)
        {

            DbCommand objCmd = this.CreateCommand(strSQL);
            DataTable dt = new DataTable();
            //clsParam objParam = null;
            StringBuilder strTempSql = new StringBuilder(strSQL);
            //Copy the sql 

            objCmd.CommandType = enumCommandType;
            objCmd.CommandTimeout = iNumSecsTilTimeout;

            //this is the part where we take care of the differences in syntax between bind variables in 
            //SQL Server and Oracle.  I replace the ?'s in the query with the correctly formatted 
            //parameter names and add the appropriately named parameters to the command object.
            //Essentially the bit inside this if statement does for you what the old ODBC interface did 
            //for you.
            if ((colParams != null))
            {

                foreach (clsParam objParam in colParams)
                {
                    string strReplaceChar = null;

                    switch (_enumDatabaseType)
                    {
                        case DatabaseType.Oracle:
                            strReplaceChar = ":";
                            break;
                        case DatabaseType.SqlServer:
                            strReplaceChar = "@";
                            break;
                        default:
                            throw new Exception("Unexpected value for _enumDatabaseType");
                    }

                    int iIndex = 0;
                    iIndex = strTempSql.ToString().IndexOf("?", iIndex);

                    if (iIndex == -1)
                    {
                        throw new Exception(string.Format("GSAERR: More parameters were specified in colParams (there were {0}) than are present in strSql.", colParams.Count));
                    }

                    strTempSql.Replace("?", strReplaceChar + objParam.Name, iIndex, 1);
                    iIndex += 1;

                    DbParameter objDBParameter = this.CreateParameter(objParam, OperationType.SqlStatement);

                    objCmd.Parameters.Add(objDBParameter);

                    //Select Case objParam.ParamType
                    //   Case clsParam.pType.pDate
                    //      objCmd.Parameters.Add(Me.CreateParameter(objParam.Name, DbType.DateTime, WrapNull(objParam), Nothing))
                    //   Case clsParam.pType.pLong
                    //      objCmd.Parameters.Add(Me.CreateParameter(objParam.Name, DbType.Int32, WrapNull(objParam), Nothing))
                    //   Case clsParam.pType.pFloat
                    //      objCmd.Parameters.Add(Me.CreateParameter(objParam.Name, DbType.Double, WrapNull(objParam), Nothing))
                    //   Case clsParam.pType.pString
                    //      objCmd.Parameters.Add(Me.CreateParameter(objParam.Name, DbType.String, WrapNull(objParam), ParamLen(objParam)))
                    //End Select

                }
            }

            objCmd.CommandText = strTempSql.ToString();

            return objCmd;

        }

        /// <summary>
        /// Gets a data table.  Recreates the colParams parameter.
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="colParams"></param>
        /// <param name="iNumSecsTilTimeout"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetDataTable(string strSQL, List<clsParam> colParams, Int16 iNumSecsTilTimeout)
        {

            CheckDisposed();

            if (string.IsNullOrEmpty(strSQL))
                throw new ArgumentNullException("strSQL");

            DbDataAdapter objAdp = this.CreateDataAdapter(null);
            StringBuilder sbErr = new StringBuilder();
            DataTable dt = new DataTable();


            try
            {
                //Fix the parameter names.
                MakeParameterNamesUnique(colParams);

                using (DbCommand objCmd = this.PrepareCommand(strSQL, colParams, CommandType.Text, iNumSecsTilTimeout))
                {
                    objAdp = this.CreateDataAdapter(objCmd);
                    objAdp.Fill(dt);
                }

            }
            catch (Exception ex)
            {
                sbErr.Append(ex.Message);


                sbErr.Append("SQL: " + strSQL);

                if ((colParams != null))
                {
                    sbErr.Append("Params: ");
                    foreach (clsParam objParam in colParams)
                    {
                        sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value));
                    }
                }

                throw new System.Exception(sbErr.ToString());
            }

            colParams = new List<clsParam>();
            objAdp = null;

            return dt;

        }

        public DataSet GetDataSet(string strSQL, List<clsParam> colParams, Int16 iNumSecsTilTimeout)
        {

            CheckDisposed();

            if (string.IsNullOrEmpty(strSQL))
                throw new ArgumentNullException("strSQL");

            DbDataAdapter objAdp = this.CreateDataAdapter(null);
            StringBuilder sbErr = new StringBuilder();
            DataSet ds = new DataSet();


            try
            {
                //Fix the parameter names.
                MakeParameterNamesUnique(colParams);

                using (DbCommand objCmd = this.PrepareCommand(strSQL, colParams, CommandType.Text, iNumSecsTilTimeout))
                {
                    objAdp = this.CreateDataAdapter(objCmd);
                    objAdp.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                sbErr.Append(ex.Message);


                sbErr.Append("SQL: " + strSQL);

                if ((colParams != null))
                {
                    sbErr.Append("Params: ");
                    foreach (clsParam objParam in colParams)
                    {
                        sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value));
                    }
                }

                throw new System.Exception(sbErr.ToString());
            }

            colParams = new List<clsParam>();
            objAdp = null;

            return ds;

        }

        /// <summary>
        /// Executes a stored procedure.  Does not recreate colParams.
        /// </summary>
        /// <param name="strProcedureName"></param>
        /// <param name="colParams"></param>
        /// <param name="iNumSecsTilTimeout"></param>
        /// <remarks></remarks>
        //public void ExecuteStoredProcedure(string strProcedureName, List<clsParam> colParams, Int16 iNumSecsTilTimeout)
        //{
        //    CheckDisposed();
        //    if (string.IsNullOrEmpty(strProcedureName))
        //        throw new ArgumentNullException("strProcedureName");

        //    using (DbCommand objCmd = this.CreateCommand()) {

        //        objCmd.CommandText = strProcedureName;
        //        objCmd.CommandType = CommandType.StoredProcedure;

        //        foreach (clsParam objParam in colParams) {
        //            DbParameter objDBParameter = this.CreateParameter(objParam, OperationType.StoredProcedure);
        //            objCmd.Parameters.Add(objDBParameter);
        //        }


        //        try {
        //            objCmd.ExecuteNonQuery();

        //            //Grab all of the output parameters and assign them to the parameter values.  
        //            //  This enables this method to be called with almost any set of input / output parameters.
        //            if (colParams != null) {
        //                foreach (clsParam objParam in colParams) {
        //                    if (objParam.InOrOut == clsParam.pInOut.pOut) {
        //                        objParam.Value = objCmd.Parameters(GetDBParameterName(objParam.Name, OperationType.StoredProcedure)).Value;
        //                    }

        //                }
        //            }


        //        } catch (Exception ex) {
        //            StringBuilder sbErr = new StringBuilder();

        //            sbErr.Append(ex.Message);

        //            sbErr.Append("SQL: " + strProcedureName);

        //            if ((colParams != null)) {
        //                sbErr.Append("Params: ");
        //                foreach (DbParameter objParam in objCmd.Parameters) {
        //                    sbErr.Append(objParam.ParameterName + ": " + Convert.ToString(objParam.Value));
        //                }
        //            }

        //            throw new System.Exception(sbErr.ToString());
        //        }
        //    }

        //}

        /// <summary>
        /// Executes a SQL statement without returning any results.  This method recreates colParams.
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="colParams"></param>
        /// <param name="iNumSecsTilTimeout"></param>
        /// <remarks></remarks>
        public int ExecuteSQLStatement(string strSQL, List<clsParam> colParams, Int16 iNumSecsTilTimeout, bool IsUpdate)
        {
            int ID = 0;
            CheckDisposed();
            if (string.IsNullOrEmpty(strSQL))
                throw new ArgumentNullException("strSQL");

            MakeParameterNamesUnique(colParams);

            using (DbCommand objCmd = PrepareCommand(strSQL, colParams, CommandType.Text, iNumSecsTilTimeout))
            {
                StringBuilder sbErr = new StringBuilder();

                try
                {
                    if (!IsUpdate)
                    {
                        objCmd.CommandText += ";SELECT SCOPE_IDENTITY() AS ID;";
                        // ****** Changed by Sachin N. S. on 08/02/2013 for Bug-7304 -- Start ****** \\
                        //if (objCmd.ExecuteScalar() != null) 
                        //{
                        //    ID = Convert.ToInt32(objCmd.ExecuteScalar());
                        //}

                        ID = Convert.ToInt32(objCmd.ExecuteScalar());

                        // ****** Changed by Sachin N. S. on 08/02/2013 for Bug-7304 -- End ****** \\
                    }
                    else
                    {
                        objCmd.ExecuteNonQuery();
                        ID = 1;
                    }
                }
                catch (Exception ex)
                {
                    sbErr.Append(ex.Message);

                    if (objCmd != null)
                    {
                        sbErr.Append("SQL: " + objCmd.CommandText);
                    }

                    if ((colParams != null))
                    {
                        sbErr.Append("Params: ");
                        foreach (clsParam objParam in colParams)
                        {
                            sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value));
                        }
                    }

                    throw new System.Exception(sbErr.ToString());

                }
                finally
                {
                    colParams = new List<clsParam>();
                }
            }
            return ID;
        }

        private object WrapNull(clsParam objParam)
        {
            var _with1 = objParam;
            if (_with1.WrapNull)
            {
                if (object.Equals(_with1.NullValue, _with1.Value))
                {
                    return System.DBNull.Value;
                }
                else
                {
                    return _with1.Value;
                }
            }
            else
            {
                return _with1.Value;
            }
        }

        private int ParamLen(clsParam objParam)
        {
            int functionReturnValue = 0;
            if (objParam.Value != null)
            {
                if (objParam.Value.ToString().Length == 0)
                {
                    functionReturnValue = 1;
                }
                else
                {
                    functionReturnValue = objParam.Value.ToString().Length;
                }
            }
            else
            {
                functionReturnValue = 1;
            }

            return functionReturnValue;
        }

        public DataSet GetDataSet(string strConnectString, Int16 iDBType, string strSQL, string strSetName, List<clsParam> colParams, IDbConnection cn, IDbTransaction trans)
        {

            SqlConnection cnSql = default(SqlConnection);
            SqlCommand cmdSql = default(SqlCommand);
            SqlDataAdapter adpSql = default(SqlDataAdapter);

            OracleConnection cnOracle = default(OracleConnection);
            OracleCommand cmdOracle = default(OracleCommand);
            OracleDataAdapter adpOracle = default(OracleDataAdapter);

            DataSet ds = null;
            //clsParam objParam = default(clsParam);

            StringBuilder strTmpSQL = new StringBuilder();
            int iIndex = 0;
            StringBuilder sbErr = new StringBuilder();
            bool bolConnPassed = true;

            strTmpSQL.Append(strSQL);

            bolConnPassed = (cn != null);

            switch (iDBType)
            {
                case 1:
                    if ((cn != null))
                    {
                        cnOracle = (OracleConnection)cn;
                    }
                    else
                    {
                        cnOracle = new OracleConnection(strConnectString);
                        try
                        {
                            cnOracle.Open();
                        }
                        catch (Exception ex)
                        {
                            throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
                        }
                        cn = cnOracle;
                    }

                    cmdOracle = cnOracle.CreateCommand();
                    cmdOracle.CommandType = CommandType.Text;

                    if ((trans != null))
                    {
                        cmdOracle.Transaction = (OracleTransaction)trans;
                    }

                    iIndex = 0;

                    //this is the part where we take care of the differences in syntax between bind variables in 
                    //SQL Server and Oracle.  I replace the ?'s in the query with the correctly formatted 
                    //parameter names and add the appropriately named parameters to the command object.
                    //Essentially the bit inside this if statement does for you what the old ODBC interface did 
                    //for you.
                    if ((colParams != null))
                    {
                        foreach (clsParam objParam in colParams)
                        {
                            var _with2 = objParam;
                            iIndex = strTmpSQL.ToString().IndexOf("?", iIndex);
                            strTmpSQL.Replace("?", ":" + _with2.Name, iIndex, 1);
                            iIndex += 1;

                            switch (_with2.ParamType)
                            {
                                case clsParam.pType.pDate:
                                    cmdOracle.Parameters.Add(_with2.Name, OracleType.DateTime).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pLong:
                                    cmdOracle.Parameters.Add(_with2.Name, OracleType.Int32).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pFloat:
                                    cmdOracle.Parameters.Add(_with2.Name, OracleType.Double).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pString:
                                    cmdOracle.Parameters.Add(_with2.Name, OracleType.VarChar, ParamLen(objParam)).Value = WrapNull(objParam);
                                    break;
                            }
                        }

                    }

                    cmdOracle.CommandText = strTmpSQL.ToString();

                    try
                    {
                        ds = new DataSet();
                        adpOracle = new OracleDataAdapter(cmdOracle);
                        adpOracle.Fill(ds, strSetName);

                    }
                    catch (Exception ex)
                    {
                        sbErr.Append("Trace: " + ex.StackTrace + "Description: " + ex.Message);

                        sbErr.Append("SQL: " + strTmpSQL.ToString());

                        if ((colParams != null))
                        {
                            sbErr.Append("Params: " + colParams.Count);
                            foreach (clsParam objParam in colParams)
                            {
                                sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value));
                            }
                        }

                        throw new System.Exception(sbErr.ToString());
                    }
                    finally
                    {
                        if (((cnOracle != null)) & (bolConnPassed == false))
                        {
                            if (cnOracle.State == ConnectionState.Open)
                            {
                                cnOracle.Close();
                                cnOracle.Dispose();
                            }
                        }

                        cnOracle = null;
                        colParams = new List<clsParam>();
                        cnOracle = null;
                    }


                    return ds;
                case 3:
                    if ((cn != null))
                    {
                        cnSql = (SqlConnection)cn;
                    }
                    else
                    {
                        cnSql = new SqlConnection(strConnectString);
                        try
                        {
                            cnSql.Open();
                        }
                        catch (Exception ex)
                        {
                            throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
                        }
                        cn = cnSql;
                    }

                    cmdSql = cnSql.CreateCommand();
                    cmdSql.CommandType = CommandType.Text;

                    if ((trans != null))
                    {
                        cmdSql.Transaction = (SqlTransaction)trans;
                    }

                    //this is the part where we take care of the differences in syntax between bind variables in 
                    //SQL Server and Oracle.  I replace the ?'s in the query with the correctly formatted 
                    //parameter names and add the appropriately named parameters to the command object.
                    //Essentially the bit inside this if statement does for you what the old ODBC interface did 
                    //for you.
                    iIndex = 0;
                    if ((colParams != null))
                    {
                        foreach (clsParam objParam in colParams)
                        {
                            var _with3 = objParam;
                            iIndex = strTmpSQL.ToString().IndexOf("?", iIndex);
                            strTmpSQL.Replace("?", "@" + _with3.Name, iIndex, 1);
                            iIndex += 1;
                            switch (_with3.ParamType)
                            {
                                case clsParam.pType.pFloat:
                                    cmdSql.Parameters.Add("@" + _with3.Name, SqlDbType.Decimal).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pDate:
                                    cmdSql.Parameters.Add("@" + _with3.Name, SqlDbType.DateTime).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pLong:
                                    cmdSql.Parameters.Add("@" + _with3.Name, SqlDbType.Int).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pString:
                                    cmdSql.Parameters.Add("@" + _with3.Name, SqlDbType.VarChar, ParamLen(objParam)).Value = WrapNull(objParam);
                                    break;
                            }
                        }
                    }

                    cmdSql.CommandText = strTmpSQL.ToString();

                    try
                    {
                        ds = new DataSet();
                        adpSql = new SqlDataAdapter(cmdSql);
                        adpSql.Fill(ds, strSetName);
                    }
                    catch (Exception ex)
                    {
                        sbErr.Append("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message + "\n");

                        sbErr.Append("SQL: " + "\n" + strTmpSQL.ToString() + "\n");

                        if ((colParams != null))
                        {
                            sbErr.Append("Params: " + "\n");
                            foreach (clsParam objParam in colParams)
                            {
                                sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value) + "\n");
                            }
                        }

                        throw new System.Exception(sbErr.ToString());
                    }
                    finally
                    {
                        if ((cnSql != null) & (bolConnPassed == false))
                        {
                            if (cnSql.State == ConnectionState.Open)
                            {
                                cnSql.Close();
                                cnSql.Dispose();
                            }
                        }

                        cnSql = null;
                        colParams = new List<clsParam>();
                        cnSql = null;
                    }

                    return ds;
            }
            return ds;
        }

        public DataRow GetDataRow(string strConnectString, Int16 iDBType, string strSQL, List<clsParam> colParams, IDbConnection cn, IDbTransaction trans)
        {

            DataSet ds = null;
            DataRow dr = null;

            try
            {
                ds = GetDataSet(strConnectString, iDBType, strSQL, "mytable", colParams, cn, trans);

                if ((ds != null))
                {
                    if (ds.Tables["mytable"].Rows.Count > 0)
                    {
                        dr = ds.Tables["mytable"].Rows[0];
                        return dr;
                    }

                }

            }
            catch (Exception ex)
            {
                throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
            }
            return dr;
        }

        public void ExecuteSQLStatement(string strConnectString, Int16 iDBType, string strSQL, List<clsParam> colParams, ref IDbConnection cn, ref IDbTransaction trans)
        {
            SqlConnection cnSql = default(SqlConnection);
            SqlCommand cmdSql = default(SqlCommand);

            OracleConnection cnOracle = default(OracleConnection);
            OracleCommand cmdOracle = default(OracleCommand);

            //clsParam objParam = default(clsParam);

            StringBuilder sbTmpSQL = new StringBuilder();
            int iIndex = 0;
            StringBuilder sbErr = new StringBuilder();
            bool bReuse = false;

            if ((cn != null))
            {
                bReuse = true;
            }
            else
            {
                bReuse = false;
            }

            sbTmpSQL.Append(strSQL);

            switch (iDBType)
            {
                case 1:
                    if ((cn != null))
                    {
                        cnOracle = (OracleConnection)cn;
                    }
                    else
                    {
                        cnOracle = new OracleConnection(strConnectString);
                        try
                        {
                            cnOracle.Open();
                        }
                        catch (Exception ex)
                        {
                            throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
                        }
                        cn = cnOracle;
                    }

                    cmdOracle = new OracleCommand(strSQL, cnOracle);
                    cmdOracle.CommandType = CommandType.Text;
                    if ((trans != null))
                    {
                        cmdOracle.Transaction = (OracleTransaction)trans;
                    }

                    iIndex = 0;
                    if ((colParams != null))
                    {
                        foreach (clsParam objParam in colParams)
                        {
                            var _with4 = objParam;
                            iIndex = sbTmpSQL.ToString().IndexOf("?", iIndex);
                            sbTmpSQL.Replace("?", ":" + _with4.Name, iIndex, 1);
                            iIndex += 1;
                            switch (_with4.ParamType)
                            {
                                case clsParam.pType.pDate:
                                    cmdOracle.Parameters.Add(_with4.Name, OracleType.DateTime).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pLong:
                                    cmdOracle.Parameters.Add(_with4.Name, OracleType.Int32).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pFloat:
                                    cmdOracle.Parameters.Add(_with4.Name, OracleType.Double).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pString:
                                    cmdOracle.Parameters.Add(_with4.Name, OracleType.VarChar, ParamLen(objParam)).Value = WrapNull(objParam);
                                    break;
                            }
                        }
                    }

                    cmdOracle.CommandText = sbTmpSQL.ToString();

                    try
                    {
                        cmdOracle.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        sbErr.Append("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message + "\n");

                        sbErr.Append("SQL: " + "\n" + sbTmpSQL.ToString() + "\n");

                        if ((colParams != null))
                        {
                            sbErr.Append("Params: " + "\n");
                            foreach (clsParam objParam in colParams)
                            {
                                //*** 2/20/09, jcarmona, value may be an object sbErr.Append(objParam.Name & ": " & CStr(objParam.Value) & vbCrLf)
                                sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value.ToString()) + "\n");
                            }
                        }

                        //*** 2/20/09,jcarmona, include exception, ex, object so we can capture base exception Throw New System.Exception(sbErr.ToString)
                        throw new System.Exception(sbErr.ToString(), ex);
                    }
                    if (!bReuse)
                    {
                        cnOracle.Close();
                    }

                    break;

                case 3:
                    if ((cn != null))
                    {
                        cnSql = (SqlConnection)cn;
                    }
                    else
                    {
                        cnSql = new SqlConnection(strConnectString);
                        try
                        {
                            cnSql.Open();
                        }
                        catch (Exception ex)
                        {
                            throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
                        }
                        cn = cnSql;

                    }

                    cmdSql = new SqlCommand(strSQL, cnSql);
                    cmdSql.CommandType = CommandType.Text;
                    if ((trans != null))
                    {
                        cmdSql.Transaction = (SqlTransaction)trans;
                    }

                    iIndex = 0;
                    if ((colParams != null))
                    {
                        foreach (clsParam objParam in colParams)
                        {
                            var _with5 = objParam;
                            iIndex = sbTmpSQL.ToString().IndexOf("?", iIndex);
                            sbTmpSQL.Replace("?", "@" + _with5.Name, iIndex, 1);
                            iIndex += 1;
                            switch (_with5.ParamType)
                            {
                                case clsParam.pType.pFloat:
                                    cmdSql.Parameters.Add("@" + _with5.Name, SqlDbType.Decimal).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pDate:
                                    cmdSql.Parameters.Add("@" + _with5.Name, SqlDbType.DateTime).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pLong:
                                    cmdSql.Parameters.Add("@" + _with5.Name, SqlDbType.Int).Value = WrapNull(objParam);
                                    break;
                                case clsParam.pType.pString:
                                    cmdSql.Parameters.Add("@" + _with5.Name, SqlDbType.VarChar, ParamLen(objParam)).Value = WrapNull(objParam);
                                    break;
                            }
                        }
                    }

                    cmdSql.CommandText = sbTmpSQL.ToString();

                    try
                    {
                        cmdSql.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        sbErr.Append("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message + "\n");

                        sbErr.Append("SQL: " + "\n" + sbTmpSQL.ToString() + "\n");

                        if ((colParams != null))
                        {
                            sbErr.Append("Params: " + "\n");
                            foreach (clsParam objParam in colParams)
                            {
                                //*** 2/20/09, jcarmona, value may be an object sbErr.Append(objParam.Name & ": " & CStr(objParam.Value) & vbCrLf)
                                sbErr.Append(objParam.Name + ": " + Convert.ToString(objParam.Value.ToString()) + "\n");
                            }
                        }

                        //*** 2/20/09,jcarmona, include exception, ex, object so we can capture base exception Throw New System.Exception(sbErr.ToString)
                        throw new System.Exception(sbErr.ToString(), ex);
                    }
                    if (!bReuse)
                    {
                        cnSql.Close();
                    }

                    break;
            }
            cnOracle = null;
            cnSql = null;
            colParams = new List<clsParam>();
        }

        //public object ExecuteStoredProcedure(string strConnectString, Int16 iDBType, string strProcedureName, List<clsParam> colParams, ref IDbConnection cn, ref IDbTransaction trans)
        //{

        //    SqlConnection cnSql = default(SqlConnection);
        //    SqlCommand cmdSql = default(SqlCommand);
        //    SqlParameter paramSql = default(SqlParameter);

        //    OracleConnection cnOracle = default(OracleConnection);
        //    OracleCommand cmdOracle = default(OracleCommand);
        //    OracleParameter paramOracle = default(OracleParameter);
        //    StringBuilder sbOutputName = new StringBuilder();
        //    bool bIsReuse = false;

        //    //clsParam objParam = default(clsParam);

        //    if ((cn != null)) {
        //        bIsReuse = true;
        //    } else {
        //        bIsReuse = false;
        //    }

        //    switch (iDBType) {
        //        case 1:
        //            if ((cn != null)) {
        //                cnOracle = (OracleConnection)cn;
        //            } else {
        //                cnOracle = new OracleConnection(strConnectString);
        //                try {
        //                    cnOracle.Open();
        //                } catch (Exception ex) {
        //                    throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
        //                }
        //                cn = cnOracle;
        //            }

        //            cmdOracle = new OracleCommand(strProcedureName, cnOracle);
        //            cmdOracle.CommandType = CommandType.StoredProcedure;
        //            if ((trans != null)) {
        //                cmdOracle.Transaction = (OracleTransaction)trans;
        //            }

        //            foreach (clsParam objParam in colParams) {
        //                var _with6 = objParam;
        //                switch (_with6.ParamType) {
        //                    case clsParam.pType.pDate:
        //                        paramOracle = new OracleParameter(_with6.Name, OracleType.DateTime);
        //                        break;
        //                    case clsParam.pType.pLong:
        //                        paramOracle = new OracleParameter(_with6.Name, OracleType.Int32);
        //                        break;
        //                    case clsParam.pType.pFloat:
        //                        paramOracle = new OracleParameter(_with6.Name, OracleType.Double);
        //                        break;
        //                    case clsParam.pType.pString:
        //                        paramOracle = new OracleParameter(_with6.Name, OracleType.VarChar, ParamLen(objParam));
        //                        break;
        //                }
        //                if (_with6.InOrOut == clsParam.pInOut.pIn) {
        //                    paramOracle.Direction = ParameterDirection.Input;
        //                    paramOracle.Value = WrapNull(objParam);
        //                } else {
        //                    paramOracle.Direction = ParameterDirection.Output;
        //                    sbOutputName.Append(objParam.Name);
        //                }
        //                cmdOracle.Parameters.Add(paramOracle);
        //            }


        //            try {
        //                cmdOracle.ExecuteNonQuery();
        //            } catch (Exception ex) {
        //                throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
        //            }

        //            if (!bIsReuse) {
        //                cnOracle.Close();
        //            }
        //            colParams = new List<clsParam>();

        //            return cmdOracle.Parameters[sbOutputName.ToString()].Value;
        //        case 3:
        //            if ((cn != null)) {
        //                cnSql = (SqlConnection)cn;

        //            } else {
        //                cnSql = new SqlConnection(strConnectString);
        //                try {
        //                    cnSql.Open();
        //                } catch (Exception ex) {
        //                    throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
        //                }
        //                cn = cnSql;
        //            }


        //            cmdSql = new SqlCommand(strProcedureName, cnSql);
        //            cmdSql.CommandType = CommandType.StoredProcedure;
        //            if ((trans != null)) {
        //                cmdSql.Transaction = (SqlTransaction)trans;
        //            }

        //            foreach (clsParam objParam in colParams)
        //            {
        //                var _with7 = objParam;
        //                switch (_with7.ParamType) {
        //                    case clsParam.pType.pDate:
        //                        paramSql = new SqlParameter("@" + _with7.Name, SqlDbType.DateTime);
        //                        break;
        //                    case clsParam.pType.pLong:
        //                        paramSql = new SqlParameter("@" + _with7.Name, SqlDbType.Int);
        //                        break;
        //                    case clsParam.pType.pFloat:
        //                        paramSql = new SqlParameter("@" + _with7.Name, SqlDbType.Decimal);
        //                        break;
        //                    case clsParam.pType.pString:
        //                        paramSql = new SqlParameter("@" + _with7.Name, SqlDbType.VarChar, ParamLen(objParam));
        //                        break;
        //                }
        //                if (_with7.InOrOut == clsParam.pInOut.pIn) {
        //                    paramSql.Direction = ParameterDirection.Input;
        //                    paramSql.Value = WrapNull(objParam);
        //                } else {
        //                    paramSql.Direction = ParameterDirection.Output;
        //                    sbOutputName.Append(_with7.Name);
        //                }
        //                cmdSql.Parameters.Add(paramSql);
        //            }


        //            try {
        //                cmdSql.ExecuteNonQuery();
        //            } catch (Exception ex) {
        //                throw new System.Exception("Trace: " + "\n" + ex.StackTrace + "\n" + "Description: " + "\n" + ex.Message);
        //            }
        //            if (!bIsReuse) {
        //                cnSql.Close();
        //            }
        //            colParams = new List<clsParam>();
        //            //*** NOT all returned values are numeric Return CLng(cmdSql.Parameters("@" & sbOutputName.ToString).Value)
        //            return cmdSql.Parameters["@" + sbOutputName.ToString()].Value;
        //    }
        //    cnOracle = null;
        //    cnSql = null;
        //    colParams = new List<clsParam>();
        //}
        #endregion

        #region "IDisposable"

        /// <summary>
        /// Throws an ObjectDisposedException if Dispose has already been called
        /// </summary>
        /// <remarks></remarks>
        private void CheckDisposed()
        {
            if (_bIsDisposed)
            {
                throw new ObjectDisposedException("clsConnection");
            }
        }

        // IDisposable
        protected void Dispose(bool disposing)
        {
            if (!this._bIsDisposed)
            {
                if (disposing)
                {
                    // TODO: free managed resources when explicitly called


                    if (_objTransaction != null)
                    {
                        //Attempt to roll back the transaction
                        try
                        {
                            _objTransaction.Rollback();
                        }
                        catch
                        {
                        }

                        //Dispose of the transaction
                        _objTransaction.Dispose();

                        _objTransaction = null;

                    }

                    if (_objConnection != null)
                    {
                        _objConnection.Dispose();
                        _objConnection = null;
                    }
                }

                // TODO: free shared unmanaged resources
            }
            this._bIsDisposed = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);


        }

        #endregion

    }
}

