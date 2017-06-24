using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using CryptoLib;

namespace U2TPlus.DAL
{
    public class DBConnections
    {
        private SqlConnection _sqlConnection;
        private SqlCommand _sqlCommand;
        private SqlDataAdapter _da;
        private SqlTransaction _SqlTransaction;

        #region Public Properties

        private CommandType _commandType;
        internal CommandType CommandTypeToExecute
        {
            get { return _commandType; }
            set { _commandType = value; }
        }

        private string connString = string.Empty;
        internal string AppConnectionString
        {
            get { return connString; }
            set { connString = value; }
        }

        CommandBehavior _CommandBehavior;
        internal CommandBehavior CommandBehavior
        {
            get
            {
                return _CommandBehavior;
            }
            set
            {
                _CommandBehavior = value;
            }
        }

        private string _DBError = null;
        public string DBError
        {
            get { return _DBError; }
        }

        private string _commandText;
        public string CommandText
        {
            set { _commandText = value; _sqlCommand.CommandText = _commandText; }
        }

        private DataSet _dataSet = new DataSet();
        public DataSet DataSetContainer
        {
            get { return _dataSet; }
        }
        #endregion

        #region Constructor

        internal DBConnections()
        {
            _commandType = CommandType.Text;
            _CommandBehavior = CommandBehavior.CloseConnection;
            _sqlConnection = new SqlConnection();
            _sqlCommand = new SqlCommand();
            _SqlTransaction = null;
        }

        #endregion

        #region Connection Methods

        internal bool OpenConnection()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Open)
                return true;
            _sqlConnection.ConnectionString = connString;
            try
            {
                _sqlConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void CloseConnection()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Open || _sqlConnection.State == ConnectionState.Connecting || _sqlConnection.State == ConnectionState.Executing || _sqlConnection.State == ConnectionState.Fetching)
            {
                _sqlConnection.Close();
                _SqlTransaction.Dispose();
            }
        }

        #endregion

        #region Parameters

        public void ClearParameters()
        {
            _sqlCommand.Parameters.Clear();
        }
        internal void AddParameter(SqlParameter __Parameter)
        {
            _sqlCommand.Parameters.Add(__Parameter);
        }
        public void AddParameter(string __ParameterName, object __Value)
        {
            AddParameter(new SqlParameter(__ParameterName, __Value));
        }

        #endregion

        #region Sql Helper Functions

        internal void InitCommand()
        {
            if (!OpenConnection())
            {
                _DBError = "Unable to establish database connection. Please check your connection string";
            }

            _SqlTransaction = _sqlConnection.BeginTransaction();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandType = CommandTypeToExecute;
            _sqlCommand.Transaction = _SqlTransaction;
        }

        public object ExecuteScalar()
        {
            object resultVal = null;
            try
            {
                InitCommand();

                resultVal = _sqlCommand.ExecuteScalar();
                if (resultVal != null)
                {
                    _SqlTransaction.Commit();
                }
                else
                {
                    _SqlTransaction.Rollback();
                }
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                _DBError = ex.Message;
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            return resultVal;
        }
        public int ExecuteNonQuery()
        {
            int resultVal = 0;
            try
            {
                InitCommand();
                resultVal = _sqlCommand.ExecuteNonQuery();
                if (resultVal != 0)
                {
                    _SqlTransaction.Commit();
                }
                else
                {
                    _SqlTransaction.Rollback();
                }
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                _DBError = ex.Message;
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            return resultVal;
        }
        public DataSet GetDataSet(string _tableName)
        {
            try
            {
                InitCommand();
                _sqlCommand.CommandText = _commandText;
                _da = new SqlDataAdapter(_sqlCommand);
                _dataSet = new DataSet();
                _da.Fill(_dataSet, _tableName);
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                _DBError = ex.Message;
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            return _dataSet;
        }
        public DataTable GetDataTable(string _tableName)
        {
            try
            {
                GetDataSet(_tableName);
            }
            catch (Exception ex)
            {
                _DBError = ex.Message;
                CloseConnection();
                _sqlCommand.Parameters.Clear();
            }
            return DataSetContainer.Tables[_tableName];
        }
        #endregion
    }
}
