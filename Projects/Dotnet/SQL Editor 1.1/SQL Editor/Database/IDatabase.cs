// *******************************************************************************
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *******************************************************************************
using System;
using System.Collections;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace QueryCommander.Database
{
	/// <summary>
	/// Summary description for IDatabaseManager.
	/// </summary>
	public interface IDatabaseManager
	{
		#region Members
		XmlDocument xmlDocument{get;set;}
		IDataAdapter DataAdapter{get;set;}
		string  ParameterToolTip{get;}
		#endregion
		#region Methods
		/// <summary>
		/// Stop current execution
		/// </summary>
		/// <returns></returns>
		bool StopExecuting();
		/// <summary>
		/// Executes all queries (SELECT statements) 
		/// This methods is overloaded
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		DataTable ExecuteCommand (string command, IDbConnection  dataConnection);
		/// <summary>
		/// Executes all queries (SELECT statements) 
		/// This methods is overloaded
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		DataTable ExecuteCommand (string command, IDbConnection dataConnection,string databaseName);
		/// <summary>
		/// The same as ExecuteCommand but returns a dataset. Primarly used for multiple queries.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		DataSet ExecuteCommand_DataSet (string command, IDbConnection dataConnection, string databaseName);
		/// <summary>
		/// Execute select statement, proceses the data to return UPDATE statements.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		string GetUpdateStatements(string command, IDbConnection dataConnection,string databaseName);
		/// <summary>
		/// Execute select statement, proceses the data to return INSERT statements.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		string GetInsertStatements(string command, IDbConnection dataConnection,string databaseName);
		/// <summary>
		/// Gets all documentation headers from database
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="dataConnection"></param>
		/// <param name="whereConditions"></param>
		/// <returns></returns>
		string GetXmlDoc(string DBName,IDbConnection  dataConnection,string whereConditions);
		/// <summary>
		/// Returns a table definition
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		string GetCreateTableString(string tableName, IDbConnection dataConnection,string databaseName);		
		/// <summary>
		/// Returns ALL database objects. Used to populate the Object browser
		/// </summary>
		/// <param name="ServerName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		ArrayList GetDatabasesObjects(string ServerName, IDbConnection  dataConnection);
		/// <summary>
		/// Returns all database objects matching [likeChar]. This method is used for IntelliSence
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="likeChar"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		ArrayList GetDatabasesObjects(string DBName, string likeChar, IDbConnection dataConnection);
		/// <summary>
		/// Returns all columns for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		ArrayList GetDatabasesObjectProperties(string DBName, string objectName, IDbConnection dataConnection);
		/// <summary>
		/// Returns all objects using specified object.
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="likeChar"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		ArrayList GetDatabasesReferencedObjects(string DBName, string likeChar, IDbConnection dataConnection);
		/// <summary>
		/// Returns a table definition
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		string GetObjectConstructorString(string DBName, string objectName, IDbConnection dataConnection, DBCommon.ScriptType scriptType, DBCommon.ScriptObjectType scriptObjectType);
		#endregion

	}
}
