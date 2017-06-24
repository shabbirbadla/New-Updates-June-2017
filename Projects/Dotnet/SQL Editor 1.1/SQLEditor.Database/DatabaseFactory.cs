using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace SQLEditor.Database
{
	/// <summary>
	/// Summary description for DatabaseFactory.
	/// </summary>
	public abstract class DatabaseFactory
	{
		public static IDatabaseManager CreateNew(IDbConnection dataConnection)
		{
			if(dataConnection is SqlConnection)
				return new SQLEditor.Database.Microsoft.Sql2000.DataManager();
            //else if(dataConnection is OleDbConnection)
            //    return new SQLEditor.Database.Microsoft.Sql2000.OleDbDataManager();
			else
				return null;
		}
		public static IDbConnection GetConnection(string connectionType, string connectionString)
		{
			switch(connectionType)
			{
				case "System.Data.SqlClient.SqlConnection":
					return new SqlConnection(connectionString);
                //case "System.Data.OleDb.OleDbConnection":
                //    return new OleDbConnection(connectionString);
			}
			return null;
		}
		public static IDbConnection GetConnection(DBConnectionType connectionType, string connectionString)
		{
			switch(connectionType)
			{
				case DBConnectionType.MicrosoftSqlClient:
					return new  SqlConnection(connectionString);
                //case DBConnectionType.MicrosoftOleDb:
                //    return new OleDbConnection(connectionString);
			}
			return null;
		}
		public static DBConnectionType GetConnectionType(IDbConnection connection)
		{
				if(connection is SqlConnection)
					return DBConnectionType.MicrosoftSqlClient;
                //else if(connection is OleDbConnection)
                //    return DBConnectionType.MicrosoftOleDb;
                return DBConnectionType.MicrosoftSqlClient;
		}
	
		public static int Update(IDataAdapter dataAdapter, DataTable dataTable)
		{
			if(dataAdapter is SqlDataAdapter)
				return ((SqlDataAdapter)dataAdapter).Update(dataTable); 
			return -1;
		}
		public static string ChangeDatabase(IDbConnection conn, string dbName)
		{
            if (conn is SqlConnection)
            {
                try { conn.ChangeDatabase(dbName); }
                catch
                {
                    conn = new SqlConnection(conn.ConnectionString);
                    conn.Open();
                    conn.ChangeDatabase(dbName);
                }
                return "SQL";
            }
            else { return "SQL"; }
		}
	}
}
