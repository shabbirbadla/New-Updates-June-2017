using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlXml;
using System.Threading;
using System.Runtime.InteropServices;
using SQLEditor.Database;

namespace SQLEditor.Database.Microsoft.Sql2000
{
	/// <summary>
	/// Includes all database commands and methods.
	/// </summary>
	public class DataManager : IDatabaseManager
	{
		#region Private Members & Enums
		private string[] _gos = new string[3] { "GO\n","GO\t"," GO "};
		private DBCommands _commands = new DBCommands();
		private ArrayList _Gos;
		private XmlDocument _xmlDocument=null;
		private SqlDataAdapter _dataAdapter;
		private static ArrayList _sqlInfoMessages = new ArrayList();
		private SqlCommand _currentCommand;
		private string _parameterToolTip="";
		private ArrayList _sqlReservedWords=null;
        private string Co_name;
		#endregion
		#region Public Members & Enums

		public DBCommands Commands
		{
			get{return _commands;}
		}

		/// <summary>
		/// Used for documentation file
		/// </summary>
		public XmlDocument xmlDocument
		{
			get{return _xmlDocument;}
			set{_xmlDocument=value;}
		}
		public IDataAdapter DataAdapter
		{
			get{return _dataAdapter;}
			set{_dataAdapter=(SqlDataAdapter)value;}
		}
		public static ArrayList SqlInfoMessages
		{
			get{return _sqlInfoMessages;}
			set{_sqlInfoMessages=value;}
		}

		public string  ParameterToolTip
		{
			get{return _parameterToolTip;}
		}
		#endregion
		#region Defaults
		public DataManager()
		{
			_Gos = new  ArrayList( (ICollection)_gos);
		}
		#endregion
		#region Events
		void OnSqlInfoMessage(object sender, SqlInfoMessageEventArgs args)
		{
			foreach (SqlError err in args.Errors)
			{
				System.Windows.Forms.Application.DoEvents();
				DBCommon.SqlInfoMessages.Add(err);
			}
		}

		#endregion
		#region Public Methods
		public bool StopExecuting()
		{
			try
			{
				_currentCommand.Cancel();;
				return true;
			}
			catch //(Exception ex)
			{
				return false;
			}

		}
        public int ExecuteSQL(string command, IDbConnection dataConnection, string databaseName)
        {
            try
            {
                dataConnection.ChangeDatabase(databaseName);
            }
            catch
            {
                dataConnection = new SqlConnection(dataConnection.ConnectionString);
                dataConnection.Open();
                dataConnection.ChangeDatabase(databaseName);
            }
            try
            {
                return ExecuteSQL(command, dataConnection);
            }
            catch
            {
                return -1;
            }
        }
        public int ExecuteSQL(string command, IDbConnection dataConnection)
        {
            SqlCommand dataCommand;
            int Retint;
            try
            {
                SqlConnection c = (SqlConnection)dataConnection;
                c.InfoMessage += new SqlInfoMessageEventHandler(c_InfoMessage);
                dataCommand = new SqlCommand(command, c);
                dataCommand.CommandTimeout = 60;
                Retint = dataCommand.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            return Retint;
        }
		public DataTable ExecuteCommand (string command, IDbConnection  dataConnection)
		{
			SqlDataAdapter dataAdapter;
			SqlCommand dataCommand;
			DataSet dataSet = new DataSet();
			DataTable dataTable = new DataTable();
			string tableName = "Query";
			try 
			{
				SqlConnection c = (SqlConnection)dataConnection;
				c.InfoMessage += new SqlInfoMessageEventHandler(c_InfoMessage);

				dataCommand = new SqlCommand(command, c);	
				dataCommand.CommandTimeout = 60;
				dataAdapter = new SqlDataAdapter(dataCommand);
				dataAdapter.Fill(dataSet, tableName);
				dataTable = dataSet.Tables[tableName];
			}
			catch(Exception ex)
			{
				dataTable.TableName = "ERROR";
				dataTable.Rows.Clear();
				dataTable.Columns.Clear();
				dataTable.Columns.Add("Error");
				dataTable.Rows.Add(new object [] {ex.Message});
				dataTable.Rows.Add(new object [] {ex.TargetSite});
				dataTable.Rows.Add(new object [] {ex.Source});
				dataTable.Rows.Add(new object [] {ex.StackTrace});
				return dataTable;
			}
			return dataTable;
		}
		public DataTable ExecuteCommand (string command, IDbConnection  dataConnection, string databaseName)
		{
			try
			{
				dataConnection.ChangeDatabase(databaseName);
			}
			catch
			{
				dataConnection = new SqlConnection(dataConnection.ConnectionString);
				dataConnection.Open();
				dataConnection.ChangeDatabase(databaseName);
			}
			try
			{
				return ExecuteCommand(command, dataConnection);
			}
			catch(Exception ex)
			{
				throw(ex);
			}
		}
		/// <summary>
		/// The same as ExecuteCommand but returns a dataset. Primarly used for multiple queries.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		public DataSet ExecuteCommand_DataSet (string command, IDbConnection dataConnection,string databaseName)
		{
			bool isSingleStatement = true;
			string currentCommand = "";
			int startPos = 0;
			int length = 0;
			DataSet ds = null;
			string pat = @"\bgo\b";
			Regex r = new Regex(pat, RegexOptions.IgnoreCase);
			Match m;

			for (m = r.Match(command); m.Success; m = m.NextMatch())
			{
				isSingleStatement = false;
				length = m.Index-startPos; 
				currentCommand = command.Substring(startPos,length);
				startPos=startPos+currentCommand.Length+2;

				if(ds==null)
					ds=new DataSet();

				DataTable dt=null;
				DataSet tmpDs = ExecuteCommand_DataSet1(currentCommand, dataConnection, ref databaseName);
				
				if ( tmpDs.Tables.Count > 0)
					dt = tmpDs.Tables[0].Copy();

				if(dt!=null)
				{
					dt.TableName = "Query" + ds.Tables.Count.ToString();
					ds.Tables.Add(dt);				
				}
			}
			
			if ( isSingleStatement )
			{
				return ExecuteCommand_DataSet1(command, dataConnection, ref databaseName);
			}
			else
			{
				// Run last statement
				currentCommand = command.Substring(startPos);
				DataSet tmpDs = ExecuteCommand_DataSet1(currentCommand, dataConnection, ref databaseName);
				
				DataTable dt=null;

				if(tmpDs.Tables.Count > 0)
					dt = tmpDs.Tables[0].Copy();
				
				if(dt!=null)
				{
					dt.TableName = "Query" + ds.Tables.Count.ToString();
					ds.Tables.Add(dt);	
				}

				return ds;
			}
		}
		public DataSet ExecuteCommand_DataSet1 (string command, IDbConnection dataConnection, ref string databaseName)
		{
			DataSet			dataSet = new DataSet();
			DataTable       dataTable = new DataTable();
			int length		= command.Length;
			string tableName="Query";
			try{dataConnection.ChangeDatabase(databaseName);}
			catch
			{
				dataConnection = new SqlConnection(dataConnection.ConnectionString);
				dataConnection.Open();
				dataConnection.ChangeDatabase(databaseName);
			}
			try 
			{	
				SqlConnection c = (SqlConnection)dataConnection;
				c.InfoMessage += new SqlInfoMessageEventHandler(c_InfoMessage);
				_currentCommand = new SqlCommand(command, c);
				_currentCommand.CommandTimeout = 60;
				DataAdapter = new SqlDataAdapter(_currentCommand);
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_dataAdapter);
				_dataAdapter.Fill(dataSet, tableName);
				databaseName = dataConnection.Database;
			}
			catch(Exception ex)
			{
				throw(ex);
			}
			return dataSet;
		}
		/// <summary>
		/// Execute select statement, proceses the data to return UPDATE statements.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		public string GetUpdateStatements(string command, IDbConnection dataConnection,string databaseName)
		{
			string Result = "/* Generated by SQLEditor */\n\n";
			string inString = "";
			string valueString = "";
			string TableName = "";
			string WhereString ="";

			command = command.Replace("\t"," ");
			command = command.Replace("\n"," ");

			int startPos =  command.ToUpper().IndexOf("FROM") + 5;
			int endPos = 0;
			
			int lastTab = command.IndexOf("\t",startPos) == -1 ? startPos : command.IndexOf("\t",startPos);
			int lastSpace = command.IndexOf(" ",startPos) == -1 ? startPos : command.IndexOf(" ",startPos);
			
			
			if(lastTab == startPos)
				endPos = lastSpace;
			
			if(lastSpace == startPos)
				endPos = lastTab;
			
			if(lastSpace==startPos && lastTab==startPos)
				endPos = command.Length;
			
			if(lastSpace>startPos && lastTab>startPos)
				endPos = lastTab > lastSpace  ? lastSpace : lastTab;
			
			TableName = command.Substring(startPos,endPos-startPos).Trim();

			DataTable dt = ExecuteCommand(command, dataConnection,databaseName);
			
			inString = "UPDATE " + TableName + "\nSET ";
			foreach(DataRow row in dt.Rows)
			{
				for(int i=0;i<row.ItemArray.Length;i++)
				{
					if(dt.Columns[i].ColumnName.ToUpper() == "ID")
						WhereString = "WHERE ID = '" + row.ItemArray[i].ToString() + "'";

					if(dt.Columns[i].DataType == System.Type.GetType("System.String") || 
						dt.Columns[i].DataType == System.Type.GetType("System.DateTime"))
					{
						valueString += dt.Columns[i].ColumnName + " = " + "'" + row.ItemArray[i].ToString().Replace("'","''") + "', ";
					}
					else if(dt.Columns[i].DataType == System.Type.GetType("System.Guid"))
					{
						string v = "'" + row.ItemArray[i].ToString().Replace("'","''") + "', ";
						if(v=="'', ")
							v = "null, ";
						
						valueString += dt.Columns[i].ColumnName + " = " + v;
					}
					else if(dt.Columns[i].DataType == System.Type.GetType("System.Byte[]"))
						valueString = valueString;
						//valueString += dt.Columns[i].ColumnName + " = " +"null, ";
					else if(dt.Columns[i].DataType == System.Type.GetType("System.Boolean"))
					{
						if( row.ItemArray[i].ToString() == "True")
							valueString += dt.Columns[i].ColumnName + " = " + "1, ";
						else
							valueString += dt.Columns[i].ColumnName + " = " + "0, ";
					}
					else
						valueString += dt.Columns[i].ColumnName + " = " + row.ItemArray[i].ToString() + ", ";
				}

				Result += inString + valueString.Substring(0,valueString.Length-2) + "\n" + WhereString + "\n\n";
				valueString = "";

			}
			return Result;
		}
		/// <summary>
		/// Execute select statement, proceses the data to return INSERT statements.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		public string GetInsertStatements(string command, IDbConnection dataConnection, string databaseName)
		{
			string Result = "/* Generated by SQLEditor */\n\n";
			string inString = "";
			string valueString = "";
			string TableName = "";

			command = command.Replace("\t"," ");
			command = command.Replace("\n"," ");


			int startPos =  command.ToUpper().IndexOf("FROM") + 5;
			int endPos = 0;
			
			int lastTab = command.IndexOf("\t",startPos) == -1 ? startPos : command.IndexOf("\t",startPos);
			int lastSpace = command.IndexOf(" ",startPos) == -1 ? startPos : command.IndexOf(" ",startPos);
			
			
			if(lastTab == startPos)
				endPos = lastSpace;
			
			if(lastSpace == startPos)
				endPos = lastTab;
			
			if(lastSpace==startPos && lastTab==startPos)
				endPos = command.Length;
			
			if(lastSpace>startPos && lastTab>startPos)
				endPos = lastTab > lastSpace  ? lastSpace : lastTab;
			
			TableName = command.Substring(startPos,endPos-startPos).Trim();

			DataTable dt = ExecuteCommand(command, dataConnection,databaseName);
			
			inString = "INSERT INTO " + TableName + "(";
			foreach(DataColumn col in dt.Columns)
			{
				inString += "[" + col.ColumnName + "], ";
			}
			inString = inString.Substring(0,inString.Length-2) + ") \nVALUES (";

			foreach(DataRow row in dt.Rows)
			{
				for(int i=0;i<row.ItemArray.Length;i++)
				{
					if(row.ItemArray[i].GetType()==System.Type.GetType("System.DBNull"))
						valueString +="null, ";
					else if(dt.Columns[i].DataType == System.Type.GetType("System.String") || dt.Columns[i].DataType == System.Type.GetType("System.DateTime"))
						valueString += "'" + row.ItemArray[i].ToString().Replace("'","''") + "', ";
					else if(dt.Columns[i].DataType == System.Type.GetType("System.Guid"))
						valueString += "'" + row.ItemArray[i].ToString().Replace("'","''") + "', ";
					else if(dt.Columns[i].DataType == System.Type.GetType("System.Byte[]"))
						valueString += "null, ";
					else if(dt.Columns[i].DataType == System.Type.GetType("System.Boolean"))
						valueString +=row.ItemArray[i].ToString() == "True"?"1, ":"0, ";
					else
						valueString += row.ItemArray[i].ToString() + ", ";						
				}

				Result += inString + valueString.Substring(0,valueString.Length-2) + ")\n\n";
				valueString = "";

			}
			return Result;
		}
		/// <summary>
		/// sp_addtype [ @typename = ] type, 
		/// [ @phystype = ] system_data_type 
		///	[ , [ @nulltype = ] 'null_type' ] 
		/// [ , [ @owner = ] 'owner_name' ]
		/// </summary>
		/// <param name="objUDT"></param>
		/// <param name="DBName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public string GetCreateUDTString(DBObjectAttribute objUDT, string DBName, IDbConnection dataConnection)
		{
			ArrayList no_precision_types = new ArrayList();
			no_precision_types.Add("int");
			no_precision_types.Add("smallint");
			no_precision_types.Add("bit");
			no_precision_types.Add("text");
			no_precision_types.Add("ntext");
			no_precision_types.Add("tinyint");
			no_precision_types.Add("datetime");
			no_precision_types.Add("uniqueidentifier");
			no_precision_types.Add("float");
			no_precision_types.Add("real");
			no_precision_types.Add("smalldatetime");
			no_precision_types.Add("image");

			string nulls = ( objUDT.allowNulls.ToString() == "1" ? "NULL" : "NONULL" );
			string Rule = string.Format(objUDT.rule_name != "" ?  "\nEXEC sp_bindrule N'[dbo].[{1}]', N'{0}'" : "" , objUDT.Name, objUDT.rule_name );
			string binddefault = string.Format( objUDT.default_id != "" ? "\nEXEC sp_bindefault N'[dbo].[{1}]', N'{0}'" : "" , objUDT.Name, objUDT.default_id );
			string Result = string.Empty; //"use {8}\n\nif exists (select * from dbo.systypes where name = N'{0}')\n" + 
							//"    EXEC sp_droptype N'{0}'\n";
			if ( no_precision_types.Contains(objUDT.Type.ToLower()))
			{
				Result += "EXEC sp_addtype N'{0}', N'{1}', N'{4}', N'{5}'\n";
			}
			else
			{
				if ( objUDT.Precesion != -999 )
				{
					Result += "EXEC sp_addtype N'{0}', N'{1} ({2},{3})', N'{4}', N'{5}' ";
							
				}
				else
				{
					Result += "EXEC sp_addtype N'{0}', N'{1} ({2})', N'{4}', N'{5}' ";
				}
			}
			return String.Format( Result, objUDT.Name, objUDT.Type, objUDT.Length, objUDT.Precesion, nulls, objUDT.owner, objUDT.default_id, DBName ) + binddefault + Rule; ;
		}
		public string GetCreateJobString(string JobName, string DBName, IDbConnection dataConnection)
		{
			string JobSteps = string.Empty;
			string Server = string.Empty;
			string User = string.Empty;
			string Description = string.Empty;
			string Enabled = string.Empty;
			int StepCount = 0;
			DataTable dt = this.ExecuteCommand(this.Commands.GetJobStepInfo( JobName ), 
				dataConnection, DBName);
			if (dt != null)
			{
				if (dt.TableName == "ERROR")
				{
					string errmsg = string.Empty;
					foreach(DataRow dr_err in dt.Rows)
					{
						errmsg += dr_err[0].ToString();
					}
					throw new Exception( errmsg.Length > 0 ? errmsg : "Error retrieving job step information!" );
				}
				StepCount = dt.Rows.Count;
				foreach(DataRow dr in dt.Rows)
				{
					JobSteps += this.Commands.CreateJobStep( 
						dr["step_name"].ToString(), dr["step_id"].ToString(), 
						dr["database_name"].ToString(), (dr["server"] == null ? "":dr["server"].ToString()), 
						(dr["database_user_name"] == null ? "":dr["database_user_name"].ToString()), dr["output_file_name"].ToString(),
						dr["flags"].ToString().Split(' ')[0], dr["cmdexec_success_code"].ToString(), dr["on_success_action"].ToString(),
						dr["on_fail_action"].ToString(), dr["on_success_step_id"].ToString(), dr["on_fail_step_id"].ToString(),
						dr["retry_attempts"].ToString(), dr["retry_interval"].ToString(), dr["command"].ToString() 
						) + "\n";
				}
			}
			DataSet ds = this.ExecuteCommand_DataSet( this.Commands.GetJobInfo( JobName ), 
				dataConnection, DBName);
			if (ds != null && ds.Tables.Count > 0)
			{
				Server = ds.Tables[0].Rows[0]["originating_server"].ToString();
				User = ds.Tables[0].Rows[0]["owner"].ToString();
				Description = ds.Tables[0].Rows[0]["description"].ToString();
				Enabled = ds.Tables[0].Rows[0]["enabled"].ToString();
			}
			return this.Commands.CreateJob( JobName, User, Server, Description, JobSteps, Enabled );
		}
		/// <summary>
		/// returns the create string for individual defaults and rules
		/// </summary>
		/// <param name="objectName"></param>
		/// <param name="objectName"></param>
		/// <param name="DBName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public string GetCreateObjectString(string objectName, string DBName, IDbConnection dataConnection)
		{
			string Result = string.Empty; 

			string command = _commands.GetObjectScript(DBName, objectName);
			DataTable dt = ExecuteCommand(command, dataConnection, DBName);
			for(int i=0;i<dt.Rows.Count;i++)
			{
				Result += dt.Rows[i][0].ToString() + "\n";  
			}
			return string.Format(Result, objectName, DBName);
		}
		/// <summary>
		/// Returns a table definition
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		public string GetCreateTableString(string tableName, IDbConnection dataConnection, string databaseName)
		{
			string sql = "CREATE TABLE [" + tableName + "](\n";
			string rowString;
			string lengthString;
			string precitionString;
			string allowNull;
			string constrintString;
			string collateString;
			string defaultString;
			string constraint = "";
			int currentRow = 1;

			DataTable ds;
			// DataTable dsReferense;
			DataTable dsLength;

			string colCommand = "exec sp_MShelpcolumns N'[" + tableName + "]', @orderby = 'id'";
			string constrintCommand = "exec sp_MStablekeys N'[" + tableName + "]', null, 14";
			string referenseCommand = "SELECT  c.Name as cName, o.Name as oName " + 
				"from dbo.sysindexes ix " +  
				"join   dbo.sysforeignkeys ixk on ixk.rkeyid = ix.id " + 
				"join   dbo.syscolumns c on c.id = ix.id and ixk.keyno = c.colorder " + 
				"join  dbo.sysobjects o on o.id = c.id " + 
				"where ix.name = '½'";
			string lengthCommand = "select length, xprec, xscale " +
				"from dbo.systypes " +
				"where name = '½' ";

			try
			{
				// Table columns
				ds = ExecuteCommand(colCommand, dataConnection, databaseName);

				foreach(DataRow row in ds.Rows)
				{
					string lengthCmd = lengthCommand.Replace("½", row["col_typename"].ToString());
					dsLength=ExecuteCommand(lengthCmd,dataConnection,databaseName);

					allowNull = (row["col_null"].ToString()=="False")? "NOT NULL " : "NULL ";
					constrintString = (row["col_dridefname"].ToString().Length==0)? "" : " CONSTRAINT ["+row["col_dridefname"]+"] ";
					collateString = (row["collation"].ToString().Length==0)? "" : "COLLATE "+row["collation"]+" ";
					defaultString = (row["text"].ToString().Length==0)? "" : "DEFAULT "+row["text"];
				
					lengthString = (row["col_len"].ToString() == dsLength.Rows[0][0].ToString())?"": "(" + row["col_len"].ToString() + ") ";
					precitionString = (row["col_prec"] != null) ?"": "(" + row["col_prec"] + "," + row["col_scale"].ToString() + ") ";
				
//					rowString = "\t[" + row["col_name"] + "] [" + row["col_typename"] + "] " + lengthString + precitionString  + constrintString + collateString +defaultString ;
					rowString = "\t[" + row["col_name"] + "] [" + row["col_typename"] + "] " + lengthString + precitionString  + collateString + allowNull + constrintString  + defaultString ;
				
					if(currentRow != ds.Rows.Count)
						rowString += ",\n";
					else
						rowString += "\n";

					sql +=rowString;
					currentRow++;
				}

				// Table constraints
				currentRow = 1;
				ds = ExecuteCommand(constrintCommand, dataConnection, databaseName);
//				if(ds.Rows.Count==0)
//					return sql;

				foreach(DataRow row in ds.Rows)
				{
					int i;
					constraint += "\tCONSTRAINT [" +  row["cName"].ToString() + "]";
					switch(row["ctype"].ToString())
					{
						case "1":
							constraint += " PRIMARY KEY ";
							constraint += (row["cFlags"].ToString()=="1")? "CLUSTERED " : "NONCLUSTERED ";
							break;
						case "2":
							constraint += " UNIQUE ";
							constraint += (row["cFlags"].ToString()=="1")? "CLUSTERED " : "NONCLUSTERED ";
							break;
						case "3":
							constraint += " FOREIGN KEY ";
							break;
					}
					constraint += "\n\t(";

					for(int colCount=0;colCount<(int)row["cColCount"];colCount++)
					{
						i=colCount +1;
						constraint += "\n\t\t[" + row["cKeyCol" + i.ToString()].ToString() + "]";
						if(i == (int)row["cColCount"])
							constraint += "";
						else
							constraint += ",";
					}
					if(row["cGroupName"].ToString().Length > 0)
						constraint += "\n\t) ON [" + row["cGroupName"].ToString() + "]";
				
					if(row["cRefKey"].ToString().Length > 0)
					{
						string referenseCmd = referenseCommand.Replace("½", row["cRefKey"].ToString());
						//dsReferense = ExecuteCommand(colCommand,dataConnection,databaseName);
						//constraint +=  "\n\t)REFERENCES [" + dsReferense.Rows[1][0]  + "](\n\t\t[" + dsReferense.Rows[0][0] + "]\n\t)";
						constraint +=  "\n\t) REFERENCES " + row["cRefTable"].ToString()  + "(";

						for(int ii=1;ii<16;ii++)
						{
							if(row["cRefCol" + ii.ToString()].ToString()=="")
								break;
							constraint += "\n\t\t[" + row["cRefCol" + ii.ToString()].ToString() + "]";
							
							int nextcol = ii+1;
							if(row["cRefCol" + nextcol.ToString()].ToString()=="")
								constraint += "";
							else
								constraint += ",";
						}
						constraint += "\n\t)";
					}

					if(currentRow != ds.Rows.Count)
						sql += constraint + ",";
					else
						sql += constraint;
					
					constraint  ="\n";

					currentRow++;
				}
				sql += constraint + "\n)ON [PRIMARY]";

				string server = "";
				string[] connParams = dataConnection.ConnectionString.Split(';');
				foreach (string param in connParams)
				{
					if (param.Split('=').Length == 2 && param.Split('=')[0] == "Data Source")
					{
						server = param.Split('=')[1];
						break;
					}
				}
				if ( server.Length > 0 )
				{
					string indexCommand = "EXEC sp_indexes @table_server = '" + server + "',";
					indexCommand       += "                @table_name = '" + tableName + "',";
					indexCommand       += "                @table_catalog = '" + databaseName + "',";
					indexCommand       += "                @is_unique = 0";
					ds = ExecuteCommand(indexCommand, dataConnection, databaseName);
				}
				if (ds != null && ds.TableName == "ERROR")
				{
					string errmsg = string.Empty;
					foreach (DataRow dr in ds.Rows)
					{
						errmsg += dr[0].ToString() + "\n";
					}
					
					if(errmsg.ToUpper().IndexOf("DATA ACCESS")>1)
					{
						errmsg="--DATA ACCESS is not configured for this server. To resolve the problem run:\n--EXEC sp_serveroption '"+server+"','data access','true' \n";
						return "--ERROR: EXEC sp_indexes failed! \n" + errmsg + "\n\n" + sql;
					}
					else
						return "ERROR: EXEC sp_indexes failed! \n" + errmsg + "\n\n" + sql;
				}

				sql += "\n\n";
				string LastIndexName = string.Empty;
				string CreateIndex = string.Empty;
				string indexColumns = string.Empty;
				bool multiplefl = false;

				foreach(DataRow row in ds.Rows)
				{
					DataRow[] dra = ds.Select("INDEX_NAME = '" + row["INDEX_NAME"].ToString() + "'");
					if ( multiplefl && CreateIndex.Length > 0 && indexColumns.Length > 0 && row["INDEX_NAME"].ToString() != LastIndexName)
					{
						sql += CreateIndex + "[" + LastIndexName + "] ON [" + row["TABLE_SCHEM"] + "].[" + row["TABLE_NAME"] + "]([" + indexColumns + "]) ON [PRIMARY]\n";
					}
					if (row["INDEX_NAME"].ToString() != LastIndexName)
					{
						indexColumns = string.Empty;
						foreach (DataRow dr in dra)
						{
							indexColumns += "[" + dr["COLUMN_NAME"] + "]" + (dr["ASC_OR_DESC"].ToString() == "D" ? " DESC": "") + ", ";
						}
						if (indexColumns.Length > 0)
						{
							indexColumns = indexColumns.TrimEnd(new char[]{',',' '});
						}
						if (row["Type"].ToString() == "1")
						{
							CreateIndex = "CREATE CLUSTERED INDEX ";
						}
						else if (row["Type"].ToString() == "3")
						{
							CreateIndex = "CREATE INDEX ";
						}
					}
					if (dra.Length > 1)
					{
						multiplefl = true;
					}
					else
					{
						multiplefl = false;
					}
					if (!multiplefl && CreateIndex.Length > 0 && indexColumns.Length > 0 && row["INDEX_NAME"].ToString() != LastIndexName)
					{
						sql += CreateIndex + "[" + row["INDEX_NAME"] + "] ON [" + row["TABLE_SCHEM"] + "].[" + row["TABLE_NAME"] + "]([" + indexColumns + "]) ON [PRIMARY]\n";
					}
					LastIndexName = row["INDEX_NAME"].ToString();
				}

				return sql;
			}
			catch(Exception e)
			{
				string h = e.Message;
				return "";
			}
		}		
		/// <summary>
		/// Returns ALL database objects. Used to populate the Object browser
		/// </summary>
		/// <param name="ServerName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabasesObjects(string ServerName, IDbConnection dataConnection)
		{
			string QueryString = _commands.DatabaseObjects();

			SqlDataAdapter	dataAdapter;
			SqlCommand		dataCommand;
			DataSet			dataSet = new DataSet();
			ArrayList		Result = new ArrayList();
			try 
			{
				SqlConnection c = (SqlConnection)dataConnection;
				c.InfoMessage += new SqlInfoMessageEventHandler(c_InfoMessage);

				dataCommand = new SqlCommand(QueryString, c);
				dataCommand.CommandTimeout = 60;
				dataAdapter = new SqlDataAdapter(dataCommand);
				dataAdapter.Fill(dataSet);
			
				// Could use sp_tables to get table names and types
				foreach(DataTable dt in dataSet.Tables)
				{
					if(dt.Rows.Count > 0)
					{
						DB db = new DB();
						db.Server = ServerName;
						db.Name = dt.Rows[0].ItemArray[0].ToString();

						foreach(DataRow row in dt.Rows)
						{
							DBObject dbObject = new DBObject();
							dbObject.Name = row[1].ToString();
							dbObject.Type = row[2].ToString();
							db.dbObjects.Add(dbObject);
						}
						Result.Add(db);
					}
				}

				return Result;
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message, Localization.GetString("General.Exception"),System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
				return null;
			}
		}	
		/// <summary>
		/// Returns all database objects matching [likeChar]. This method is used for IntelliSence
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="likeChar"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabasesObjects(string DBName, string likeChar, IDbConnection dataConnection)
		{			
			_parameterToolTip="";
			likeChar = likeChar.Replace("*","%");
			string QueryString = _commands.DatabaseObject(DBName, likeChar);

			ArrayList		 Result = new ArrayList();
			DataTable dt = ExecuteCommand(QueryString,dataConnection);
			
			foreach(DataRow row in dt.Rows)
			{
				DBObject dbObject = new DBObject();
				dbObject.Name = row.ItemArray[0].ToString();
				dbObject.Type = row.ItemArray[1].ToString();
				Result.Add(dbObject);
			}
			return(Result);
		}
		/// <summary>
		/// Returns all joining options from a table
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="likeChar"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetJoiningReferences(string DBName, string joiningTable, IDbConnection dataConnection, ArrayList tableReferences, Hashtable aliasList)
		{
			string QueryString = _commands.GetJoiningReferences(joiningTable);

			ArrayList		 joins = new ArrayList();
			DataTable dt = ExecuteCommand(QueryString,dataConnection);
			
			foreach(DataRow row in dt.Rows)
			{
				if( tableReferences.Contains( row["FK_Table"].ToString().ToUpper()) ||
					!tableReferences.Contains( row["PK_Table"].ToString().ToUpper()))
				{
					string join ="";
					for(int i=1;i<16;i++)
					{
						string r=row["cKeyCol"+i.ToString()].ToString();
						if(row["cKeyCol"+i.ToString()].ToString().Length==0)
							break;

						if(i>1)
							join +=" AND ";
					
						join += GetAliasOrTableName(row["FK_Table"].ToString(),aliasList) +"."+row["cKeyCol"+i.ToString()].ToString()+" = " +
							GetAliasOrTableName(row["PK_Table"].ToString(),aliasList)+"."+row["cRefCol"+i.ToString()].ToString();
					}
					joins.Add(join);
				}
			}
				return(joins);
		}
		private string GetAliasOrTableName(string TableName, Hashtable AliasList)
		{
			foreach(object o in AliasList.Keys)
			{
				if(AliasList[o].ToString()==TableName.ToUpper())
					return o.ToString();
			}
			return TableName;
		}
		/// <summary>
		/// Returns all columns for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabasesObjectProperties(string DBName, string objectName, IDbConnection dataConnection)
		{
			string QueryString = _commands.DatabasesObjectProperties(objectName,DBName);
			ArrayList		 Result = new ArrayList();
			try
			{
				LoadXMLDocuments();

				DataSet ds = ExecuteCommand_DataSet(QueryString,dataConnection,DBName);

				//Get primary keys
				ArrayList pks = new ArrayList();
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					pks.Add(row["ColumnName"].ToString());
				}

				foreach(DataRow row in ds.Tables[2].Rows)
				{
					DBObjectProperties dbObjectProperties = new DBObjectProperties();
					// TODO: FOR SPROCS Should use the "EXEC sp_sproc_columns 'sproc name'"  it does indicate the output params
					if (ds.Tables[2].Columns.Contains("Parameter_name")) 
					{
						dbObjectProperties.Name = row["Parameter_name"].ToString();

						if( _sqlReservedWords.Contains(dbObjectProperties.Name.ToUpper()) ||
							dbObjectProperties.Name.IndexOf(" ") > 0 )
						{
							dbObjectProperties.Name = "[" + dbObjectProperties.Name + "]";
						}
						dbObjectProperties.Name = dbObjectProperties.Name + " (" + row["Type"].ToString()+")";
						dbObjectProperties.Tag = row;
					}
					if (ds.Tables[2].Columns.Contains("Column_name"))
					{
						dbObjectProperties.Name = row["Column_name"].ToString();

						if(_sqlReservedWords.Contains(dbObjectProperties.Name.ToUpper()) ||
							dbObjectProperties.Name.IndexOf(" ")>0)
							dbObjectProperties.Name = "["+dbObjectProperties.Name+"]";

						dbObjectProperties.Name = dbObjectProperties.Name + " (" + row["Type"].ToString()+")";
						dbObjectProperties.IsPrimaryKey = pks.Contains( row["Column_name"].ToString());
					}
					Result.Add(dbObjectProperties);
				}
				return(Result);
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// Returns all triggers for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabasesTriggers(string DBName, string objectName, IDbConnection dataConnection)
		{
			string QueryString = _commands.GetTrigger(DBName, objectName);
			ArrayList		 Result = new ArrayList();
			try
			{
				LoadXMLDocuments();

				DataSet ds = ExecuteCommand_DataSet(QueryString, dataConnection, DBName);

				//Get primary keys
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					DBObject dbObject = new DBObject();
					dbObject.Name = row["trigger_name"].ToString();
					string types = (row["isafter"].ToString() == "1" ? "After " : "") + 
								   (row["isinsteadof"].ToString() == "1" ? "Insteadof " : "") +
								   (row["isupdate"].ToString() == "1" ? "Update, " : "") + 
								   (row["isdelete"].ToString() == "1" ? "Delete, " : "") +
								   (row["isinsert"].ToString() == "1" ? "Insert," : "") ;
					string tablename = ds.Tables.Count == 2 && ds.Tables[1].Rows.Count == 1 ? ds.Tables[1].Rows[0]["TableName"].ToString() : "";
					dbObject.Type = (types.Length > 0 ? " For Table: " + tablename + ", Event(s): " : "") + types.TrimEnd(new char []{',',' '});
					Result.Add(dbObject);
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// Returns all indexes for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabaseIndexes(string DBName, string objectName, IDbConnection dataConnection)
		{
			string QueryString = _commands.GetIndexesList(DBName, objectName);
			ArrayList		 Result = new ArrayList();
			try
			{
				LoadXMLDocuments();

				DataSet ds = ExecuteCommand_DataSet(QueryString, dataConnection,DBName);

				//Get primary keys
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					DBObject dbObject = new DBObject();
					dbObject.Name = row["index_name"].ToString();
					string types = row["index_description"].ToString() + "; Key Fields: " + row["index_keys"].ToString();
					dbObject.Type = types.Trim();
					Result.Add(dbObject);
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// Returns all triggers for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDataBaseUDTs(string DBName, IDbConnection dataConnection)
		{
			string QueryString = _commands.GetUDTs(DBName);
			ArrayList		 Result = new ArrayList();
			try
			{
				LoadXMLDocuments();

				DataSet ds = ExecuteCommand_DataSet(QueryString, dataConnection,DBName);

				//Get primary keys
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					DBObjectAttribute dbObjectAttrib = new DBObjectAttribute();
					dbObjectAttrib.Name = row[0].ToString();
					dbObjectAttrib.Type = row[1].ToString();
					dbObjectAttrib.Length = Convert.ToInt32(row[2].ToString());
					dbObjectAttrib.Precesion = ( row[3].GetType() == typeof(System.DBNull) ? -999 : Convert.ToInt32(row[3].ToString()) );
					dbObjectAttrib.allowNulls = ( row[4].ToString() == "0" ? false : true );
					dbObjectAttrib.owner = row[5].ToString();
					dbObjectAttrib.default_id = row[6].ToString();
					dbObjectAttrib.rule_name = row[7].ToString();
					Result.Add(dbObjectAttrib);
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// Returns all triggers for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDataBaseDefaults(string DBName, IDbConnection dataConnection)
		{
			string QueryString = _commands.GetDefaults(DBName);
			ArrayList		 Result = new ArrayList();
			try
			{
				LoadXMLDocuments();

				DataSet ds = ExecuteCommand_DataSet(QueryString, dataConnection, DBName);

				//Get primary keys
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					DBObject dbObject = new DBObject();
					dbObject.Name = row[0].ToString();
					dbObject.Type = row[1].ToString();
					Result.Add(dbObject);
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Returns all triggers for a given table or view
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="objectName"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDataBaseRules(string DBName, IDbConnection dataConnection)
		{
			string QueryString = _commands.GetRules(DBName);
			ArrayList		 Result = new ArrayList();
			try
			{
				LoadXMLDocuments();

				DataSet ds = ExecuteCommand_DataSet(QueryString, dataConnection, DBName);

				//Get primary keys
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					DBObject dbObject = new DBObject();
					dbObject.Name = row[0].ToString();
					dbObject.Type = "RULE";
					Result.Add(dbObject);
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Returns all objects using specified object.
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="likeChar"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabasesReferencedObjects(string DBName, string likeChar, IDbConnection dataConnection)
		{
			likeChar = likeChar.Replace("*","%");
			string QueryString = _commands.DatabasesReferenceObjects(likeChar);
			ArrayList		 Result = new ArrayList();
			DataTable dt = ExecuteCommand(QueryString,dataConnection);
			
			foreach(DataRow row in dt.Rows)
			{
				DBObject dbObject = new DBObject();
				dbObject.Name = row.ItemArray[0].ToString();
				dbObject.Type = row.ItemArray[1].ToString();
				Result.Add(dbObject);
			}
			return(Result);
		}


		/// <summary>
		/// Returns all objects using specified object.
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="likeChar"></param>
		/// <param name="dataConnection"></param>
		/// <returns></returns>
		public ArrayList GetDatabasesReferencedObjectsClear(string DBName, string likeChar, IDbConnection dataConnection)
		{
			//likeChar = likeChar.Replace("*","%");
			string QueryString = _commands.DatabasesReferenceObjectsClear(likeChar);
			ArrayList		 Result = new ArrayList();
			DataTable dt = ExecuteCommand(QueryString,dataConnection);
			
			foreach(DataRow row in dt.Rows)
			{
				DBObject dbObject = new DBObject();
				dbObject.Name = row.ItemArray[0].ToString();
				dbObject.Type = row.ItemArray[1].ToString();
				Result.Add(dbObject);
			}
			return(Result);
		}


		/// <summary>
		/// Returns a table definition
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="dataConnection"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		public string GetObjectConstructorString(string DBName, string objectName, IDbConnection dataConnection, DBCommon.ScriptType scriptType, DBCommon.ScriptObjectType scriptObjectType)
		{
			string returnString = "";
			string QueryString = string.Empty;
			if(scriptType == DBCommon.ScriptType.CREATE || scriptType == DBCommon.ScriptType.ALTER)
			{
				objectName = objectName.Replace("DBO.","");
				
				if(scriptObjectType==DBCommon.ScriptObjectType.PROCEDURE)
				{
					QueryString = _commands.CreateScript(objectName);
				}
				else if(scriptObjectType == DBCommon.ScriptObjectType.TRIGGER)
				{
					QueryString = _commands.GetTriggerScript(objectName);
				}
				else if(scriptObjectType == DBCommon.ScriptObjectType.RULE || scriptObjectType == DBCommon.ScriptObjectType.DEFAULT)
				{
					QueryString = _commands.GetObjectScript(DBName, objectName);
				}
				else
				{
					return GetCreateTableString(objectName, dataConnection, DBName);
				}

				if(DBName.Length>0)
				{
					QueryString = "USE " + DBName + " " + QueryString; 
				}

				DataTable dt = ExecuteCommand(QueryString,dataConnection);
				for(int i=0; i < dt.Rows.Count; i++)
				{
					returnString += dt.Rows[i][0].ToString();  
				}
			}
			else if(scriptType == DBCommon.ScriptType.INSERT)
			{
				ArrayList a = GetDatabasesObjectProperties(DBName,objectName,dataConnection);

				returnString = "INSERT INTO " + objectName + "(";
				
				for(int i=0;i<a.Count;i++)
				{
					DBObjectProperties dbo = (DBObjectProperties)a[i];
					returnString += "[" + dbo.Name + "]";
					if(i<a.Count-1)
						returnString += ", ";
					else
						returnString += ")";

				}
				returnString += "\nVALUES (...)";
			}
			return returnString;
		}
		
		/// <summary>
		/// Gets all documentation headers from database
		/// </summary>
		/// <param name="DBName"></param>
		/// <param name="dataConnection"></param>
		/// <param name="whereConditions"></param>
		/// <returns></returns>
		public string GetXmlDoc(string DBName, IDbConnection dataConnection,string whereConditions)		{
			string returnString = "";
			dataConnection.ChangeDatabase(DBName);
			string QueryString = "SELECT substring(text,charindex(  '<member',text),charindex(  '</member>',text)-charindex(  '<member',text)+9 ) " + 
				"from 	syscomments c " +
				"join 	sysobjects o on o.id = c.id " + 
				"where text like '%<member%' ";
			if(whereConditions.Length>0)
				QueryString+=" and xtype in("+whereConditions+")";
			QueryString+=" order by xtype, o.name";
			if(DBName.Length>0)
				QueryString = "USE " + DBName + " " + QueryString; 
			DataTable dt = ExecuteCommand(QueryString,dataConnection);
			for(int i=0;i<dt.Rows.Count;i++)			
				returnString += "\n" +dt.Rows[i][0].ToString();  
			return returnString;
		}
		
		#endregion
		#region Private Methods
		private ArrayList GetCommands(string command)
		{
			ArrayList commands = new ArrayList();
			int start = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v\n]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(command); m.Success ; m = m.NextMatch()) 
			{
				if(m.Value=="GO")
				{
					string c = command.Substring(start,m.Index-start);
					string c2 = command.Substring( m.Index + 2,1);
					if(_Gos.Contains("GO"+c2))
					{
						commands.Add(c);
						start = m.Index+2;
					}
				}
			}
			if(commands.Count==0)
				commands.Add(command);
			else
				commands.Add(command.Substring(start,command.Length-start));
			return commands;
		
		}

		private void LoadXMLDocuments()
		{
			if(_sqlReservedWords!=null)
				return;

			_sqlReservedWords = new ArrayList();
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.Stream file = thisExe.GetManifestResourceStream("SQLEditor.Database.Microsoft.Sql2000.SQLReservedWords.xml");
			XmlDocument xmlReservedWords = new XmlDocument();
			xmlReservedWords.Load(file);
			XmlNodeList xmlNodeList = xmlReservedWords.GetElementsByTagName("SQLReservedWords");
			
		
			foreach(XmlNode node in xmlNodeList[0].ChildNodes)
			{
				if(!_sqlReservedWords.Contains(node.Name))
					_sqlReservedWords.Add(node.Name);
			}
		}
		#endregion
		private void c_InfoMessage(object sender, SqlInfoMessageEventArgs e)
		{
			string errmsg = string.Empty;
            ////foreach (SqlError err in e.Errors)
            ////{
            ////    errmsg += "[" + err.Server + "]" + err.Message + "\n";
            ////}
            ////throw (new Exception(errmsg + e.Source + "\n" + e.Message));
            
		}
	}
}
