using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;

namespace SQLEditor.Database
{
	#region Classes

	public abstract class DBCommon
	{
		public XmlDocument xmlDocument=null;
		public SqlDataAdapter DataAdapter;
		public static ArrayList SqlInfoMessages = new ArrayList();
		public enum ScriptType{CREATE,ALTER,UPDATE,DELETE,INSERT}
		public enum ScriptObjectType{TABLE,VIEW,PROCEDURE,TRIGGER,UDTS,RULE,DEFAULT}
		public static XmlDocument ReadEmbeddedResource(string resource)
		{
			System.Reflection.Assembly a = System.Reflection.Assembly.GetCallingAssembly();
			System.IO.Stream str = a.GetManifestResourceStream(resource);
			System.IO.StreamReader reader = new StreamReader(str);
			
			string content = reader.ReadToEnd();
			reader.Close();
			reader = null;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(content);
			return doc;
		}
	}

	public class DB
	{
		public string Name;
		public string Server;
		public ArrayList dbObjects = new ArrayList();
	}

	public class DBObject
	{
		public string Name;
		public string Type;
		public DBObjectAttributeCollection Attributes;
	}

	public class DBObjectAttribute
	{
		public string Name;
		public string Type;
		public int Length;
		public int Precesion;
		public bool allowNulls;
		public string owner;
		public string default_id;
		public string rule_name;
	}

	public class DBObjectAttributeCollection : ArrayList
	{
		public int Add(DBObjectAttribute attr)
		{
			return base.Add(attr);
		}
	}	
	public class DBObjectCollection : ArrayList
	{
		public int Add(DBObject obj)
		{
			return base.Add(obj);
		}
		public DBObjectCollection FindByType(string type)
		{
			DBObjectCollection oc = new DBObjectCollection();
			foreach(DBObject o in this)
			{
				if(o.Type==type)
					oc.Add(o);
			}
			return oc;
		}
	}

	public class DBObjectProperties
	{
		public string Name;
		public bool IsPrimaryKey;
		public object Tag;
	}
		
	public class DBCommands
	{	
		// 08-01-2005 LSL - CHANGED WHERE FROM: and name not like (''dt_%'') 
		//									TO: and name not like ''dt\_%'' ESCAPE ''\''
		//
		#region QueryStrings
		const string QueryString_GetDatabaseObjects = 
						@"SET NOEXEC OFF
						  declare @SQL varchar(1000) 
							declare @DBName varchar(255) 
							declare SYSDB cursor for 
							select name 
							from master.dbo.sysdatabases 
							where has_dbaccess(name) = 1 
							order by name 

							open SYSDB 
							fetch next from SYSDB into  @DBName 
							while @@fetch_status = 0 
							begin 
							set @SQL = 'use [' + @DBName +']  select '''
								+ @DBName + ''', O.name, xtype from [' + @DBName + '].dbo.sysobjects O
								where id >1000 
								and xtype in (''U'',''P'',''V'',''FN'', ''TR'') 
								and name not like ''dt\_%'' ESCAPE ''\'' 
								order by xtype , O.name' 
							exec( @SQL ) 
							fetch next from SYSDB into  @DBName 
							end 
							close SYSDB 
							Deallocate SYSDB ";
			
		const string QueryString_GetDatabaseObject = 
						@"SET NOEXEC OFF
							select name, xtype, id 
							from [{0}].dbo.sysobjects 
							where name like ('{1}%') and xtype in ('U','P','FN','TF','V') 
							order by name";

		const string QueryString_DatabaseObjectProperties_Old =  
						@"SET NOEXEC OFF
							USE [{0}] 
							select distinct c.name + ' (' + t.name
							Case WHEN c.xtype in (173,175,239,231,167,165) THEN
							' [' + convert(varchar(4),c.length)+ '])'
							ELSE
								')'
							END,
							Coalesce(ObjectProperty(Object_id(ix.name),N'IsPrimaryKey'),0) as isPrimaryKey ,C.* from {0}.dbo.sysobjects O
							join [{0}].dbo.syscolumns C on C.id = O.id
							left outer join [{0}].dbo.sysindexkeys i on i.id = c.id and i.colid = c.colid
							left outer join [{0}].dbo.sysindexes ix on ix.id = C.id and ix.indid = i.indid
							inner join [{0}].dbo.systypes t on c.xtype=t.xtype
							where O.name = '{1}' and (O.xtype ='U' or O.xtype ='V') and t.Name != 'sysname' order by C.colid";
		
		const string QueryString_DatabaseObjectProperties = 
						@"SET NOEXEC OFF
							USE [{1}] 
							select distinct c.name as ColumnName 
							from [{1}].dbo.sysobjects O 
							join [{1}].dbo.syscolumns C on C.id = O.id 
							left outer join [{1}].dbo.sysindexkeys i on i.id = c.id and i.colid = c.colid 
							left outer join [{1}].dbo.sysindexes ix on ix.id = C.id and ix.indid = i.indid 
							where O.name = '{0}'
							and ObjectProperty ( Object_id(ix.name),N'IsPrimaryKey')=1 
							order by C.name exec sp_help '{0}'";

		const string QueryString_CreateScript = 
			@"SET NOEXEC OFF
			select text 
			from sysobjects AS o 
			join syscomments c on c.id = o.id 
			where o.name = '{0}' 
			order by o.name";
		
		const string QueryString_GetJoiningOptions = 
						@"SET NOEXEC OFF
						select 
							o.name, fc.name,ro.name, c.name, fk.*
						from sysobjects o 
						join sysforeignkeys fk on fk.fkeyid = o.id
						join sysobjects ro on ro.id = fk.rkeyid
						join syscolumns c on c.id = ro.id and c.colid = fk.rkey																				  
						join syscolumns fc on fc.id = o.id and fc.colid = fk.fkey
						where o.name = '{0}'";

		const string QueryString_AllObjects = 
							@"SET NOEXEC OFF
							SELECT 1 as Tag, NULL as Parent,
								o.name as [DBObject!1!Name],
								o.xtype as [DBObject!1!Type],
								null as [DBObjectAttribute!2!Name] ,
								null as [DBObjectAttribute!2!Type], 
								null as [DBObjectAttribute!2!Length] ,
								null as [DBObjectAttribute!2!Precision]
							from 	sysobjects o 
							where 	o.xType != 'S'
							union all
							SELECT 2,1,
								o.name,
								o.xtype,
								c.name,
								t.name,
								c.length,
								c.prec
							from 	sysobjects o 
							join	sysColumns c on c.id = o.id
							join 	systypes t on t.xtype = c.xtype
							where 	o.xType != 'S'
							and 	len(c.Name)>0
							ORDER BY [DBObject!1!Name],[DBObjectAttribute!2!Name]
							FOR XML EXPLICIT";

		const string QueryString_ReferenceObjects = 
						@"SET NOEXEC OFF
						SELECT distinct o.name, o.xtype
						from syscomments c
						join sysobjects o on o.id = c.id
						where o.name != '{0}'
						and	(c.text like'%{0}%' or c.text like'%{0}(%')";

		const string QueryString_ReferenceObjectsClear  = 
						@"SET NOEXEC OFF
						SELECT distinct o.name, o.xtype
						from syscomments c
						join sysobjects o on o.id = c.id
						where o.name != '{0}'
						and	(c.text like '% {0} %' or c.text like '% {0}(%' or c.text like '% {0} (%'
						or c.text like '%.{0} %' or c.text like '%.{0}(%')";

		const string QueryString_GetJoiningReferences  = 
						@"SET NOEXEC OFF
						  exec sp_MStablerefs N'{0}', N'all', N'both', null";

		const string QueryString_GetTrigger  = 
						@"SET NOEXEC OFF
							USE [{0}] 
							DECLARE @tabname as varchar(50)

							SET @tabname = (select TOP 1 a.name as ParentName
											from sysobjects as a 
											inner join sysobjects as b on b.parent_obj = a.id
											where a.xType = 'U' and b.xType = 'tr' and b.name = '{1}')

							EXEC sp_helptrigger @tabname
							SELECT @tabname AS TableName";

		const string QueryString_GetTriggerScript  = 
							@"SET NOEXEC OFF
							EXEC sp_helptext '{0}'";

		const string QueryString_GetUDT = 
						@"SET NOEXEC OFF
						USE [{0}]
						SELECT 
							a.name,
							(SELECT TOP 1 [name] FROM systypes AS c WHERE c.xusertype = a.xtype) AS type,
							a.prec,
							a.scale,
							a.allownulls,
							owner = b.name,
							(SELECT TOP 1 [name] from sysobjects AS d WHERE d.id = a.tdefault) as [default],
							(SELECT TOP 1 [name] FROM sysobjects AS e WHERE e.type = 'R' and e.id = a.domain) AS [Rule]
						FROM systypes AS a
						INNER JOIN sysusers AS b ON b.uid = a.uid
						LEFT OUTER JOIN master.dbo.systypes c ON c.name = a.name
						WHERE c.name IS NULL";

		const string QueryString_GetDefaults = 
						@"SET NOEXEC OFF
						USE [{0}]
						SELECT 
							a.[name],
							b.[text]
						FROM sysobjects AS a
						JOIN syscomments AS b ON b.ID = a.ID
						WHERE Type = 'D' AND [name] NOT LIKE 'DF_%'"; //

		const string QueryString_GetObjectScript = 
						@"SET NOEXEC OFF
						USE [{0}] 
						EXEC sp_helptext '{1}'";

		const string QueryString_GetRules = 
						@"SET NOEXEC OFF
						USE [{0}]
						SELECT [name] FROM sysobjects WHERE Type = 'R'";

		const string QueryString_GetIndexes = 
						@"SET NOEXEC OFF
						USE [{0}]
						EXEC sp_helpindex '{1}'";

		const string QueryString_GetIndexes_RunQuery = 
			@"USE [{0}]
EXEC sp_helpindex '{1}'";

		const string QueryString_RefreshView = 
			@"USE [{1}]
EXEC sp_refreshview @viewname = '{0}' ";

		const string QueryString_GetConstraints = 
			@"USE [{1}]
EXEC sp_helpconstraint '{0}'";

		const string QueryString_GetWho = 
			@"EXEC sp_who
EXEC sp_helpuser
EXEC sp_helplogins
EXEC sp_helpsrvrolemember
EXEC sp_helprole
EXEC sp_helprolemember
EXEC sp_helpntgroup";

		const string QueryString_GetDBProperties = 
			@"EXEC sp_helpdb '{0}'";

		const string QueryString_GetAlerts = 
			@"USE msdb
EXEC sp_help_alert";

		const string QueryString_GetLinkedServers = 
			@"EXEC sp_linkedservers";

		const string QueryString_GetLocks = 
			@"USE master
EXEC sp_lock";

		const string QueryString_GetStatistics = 
			@"USE master
EXEC sp_monitor";

		const string QueryString_GetTableIndexStatistics = 
			@"EXEC ( 'DBCC SHOW_STATISTICS( [{0}], [{1}])')";

		const string QueryString_GetTableSpace = 
			@"USE [{1}]
EXEC sp_spaceused '{0}'";

		const string QueryString_GetTableStatistics = 
			@"USE [{1}]
EXEC sp_statistics '{0}'";

		const string QueryString_GetRowUniqueColumns = 
			@"USE [{1}]
EXEC sp_special_columns '{0}'";

		const string QueryString_GetTablePermissions = 
			@"USE [{1}]
EXEC sp_table_privileges '{0}'";

		const string QueryString_UpdateDBStats = 
			@"USE [{0}]
EXEC sp_updatestats";

		const string QueryString_GetSprocParams = 
			@"USE [{1}]
EXEC sp_sproc_columns '{0}'";

		const string QueryString_GetDepends = 
			@"USE [{1}]
exec sp_depends '{0}'";

		const string QueryString_GetFKs = 
			@"USE [{1}]
EXEC sp_fkeys @pktable_name = N'{0}'";

		const string QueryString_GetSysTables = 
			@"USE [{0}]
exec sp_tables @table_type = ""'SYSTEM TABLE'""";

		#endregion

		#region Job related string constants

		// {0} = job_name
		// {1} = database_user_name
		// {2} = server 
		// {3} = description
		// {4} = job steps
		// {5} = enabled
		const string QueryString_CreateJob = 
			@"-- Script generated on 6/28/2005 8:11 AM
-- By: Auto-generated SQLEditor SQL
-- Server: {2}

BEGIN TRANSACTION            
  DECLARE @JobID BINARY(16)  
  DECLARE @ReturnCode INT    
  SELECT @ReturnCode = 0     
IF (SELECT COUNT(*) FROM msdb.dbo.syscategories WHERE name = N'[Uncategorized (Local)]') < 1 
  EXECUTE msdb.dbo.sp_add_category @name = N'[Uncategorized (Local)]'

  -- Delete the job with the same name (if it exists)
  SELECT @JobID = job_id     
  FROM   msdb.dbo.sysjobs    
  WHERE (name = N'{0}')       
  IF (@JobID IS NOT NULL)    
  BEGIN  
  -- Check if the job is a multi-server job  
  IF (EXISTS (SELECT  * 
              FROM    msdb.dbo.sysjobservers 
              WHERE   (job_id = @JobID) AND (server_id <> 0))) 
  BEGIN 
    -- There is, so abort the script 
    RAISERROR (N'Unable to import job ''{0}'' since there is already a multi-server job with this name.', 16, 1) 
    GOTO QuitWithRollback  
  END 
  ELSE 
    -- Delete the [local] job 
    EXECUTE msdb.dbo.sp_delete_job @job_name = N'{0}' 
    SELECT @JobID = NULL
  END 

BEGIN 

  -- Add the job
  EXECUTE @ReturnCode = msdb.dbo.sp_add_job @job_id = @JobID OUTPUT , @job_name = N'{0}', @owner_login_name = N'{1}', @description = N'{3}', @category_name = N'[Uncategorized (Local)]', @enabled = {5}, @notify_level_email = 0, @notify_level_page = 0, @notify_level_netsend = 0, @notify_level_eventlog = 2, @delete_level= 0
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

{4}

  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 
  EXECUTE @ReturnCode = msdb.dbo.sp_update_job @job_id = @JobID, @start_step_id = 1 

  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

  -- Add the Target Servers
  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @JobID, @server_name = N'{2}' 
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

END
COMMIT TRANSACTION          
GOTO   EndSave              
QuitWithRollback:
  IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION 
EndSave: 

";

		// {0} = step_name
		// {1} = step_id
		// {2} = database_name
		// {3} = server
		// {4} = database_user_name
		// {5} = output_file_name
		// {6} = flags
		// {7} = cmdexec_success_code
		// {8} = on_success_action
		// {9} = on_fail_action
		// {10} = on_success_step_id
		// {11} = on_fail_step_id
		// {12} = retry_attempts
		// {13} = retry_interval
		// {14} = step command text
		const string QueryString_CreateJobStep = 
			@"  -- Add the job steps
  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobstep @job_id = @JobID, @step_id = {1}, @step_name = N'{0}', @command = N'{14}'
  @database_name = N'{2}', @server = N'{3}', @database_user_name = N'{4}', @subsystem = N'TSQL', @cmdexec_success_code = {7}, @flags = {6}, @retry_attempts = {12}, @retry_interval = {13}, @output_file_name = N'{5}', @on_success_step_id = {10}, @on_success_action = {8}, @on_fail_step_id = {11}, @on_fail_action = {9}
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

";
		const string QueryString_GetJobStepInfo = 
			@"SET NOEXEC OFF
USE msdb
EXEC sp_help_jobstep @job_name = '{0}'";

		const string QueryString_GetJobs = 
			@"SET NOEXEC OFF
USE msdb
EXEC sp_help_job {0}";

		const string QueryString_StartJob =
			@"USE msdb
EXEC sp_start_job @job_name = '{0}'";

		const string QueryString_DeleteJob =
			@"USE msdb
EXEC sp_delete_job @job_name = '{0}'";

		#endregion

		#region Drop DB objects string constants

		const string QueryString_DropProcedure =
			@"USE [{1}]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[{0}]
GO
";

		const string QueryString_DropTrigger =
			@"USE [{1}]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
	drop trigger [dbo].[{0}]
GO
";

		const string QueryString_DropFunction =
			@"USE [{1}]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and xtype in (N'FN', N'IF', N'TF'))
	drop function [dbo].[{0}]
GO
";

		const string QueryString_DropUDT =
			@"USE [{1}]
GO
if exists (select * from dbo.systypes where name = N'{0}')
	EXEC sp_droptype N'{0}'
GO
";

		const string QueryString_DropRule =
			@"USE [{1}]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsRule') = 1)
	drop rule [dbo].[{0}]
GO
";

		const string QueryString_DropDefault =
			@"USE [{1}]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsDefault') = 1)
	drop default [dbo].[{0}]
GO
";

		const string QueryString_DropUDT_NoUse =
			@"if exists (select * from dbo.systypes where name = N'{0}')
	EXEC sp_droptype N'{0}'
GO
";

		const string QueryString_DropRule_NoUse =
			@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsRule') = 1)
	drop rule [dbo].[{0}]
GO
";

		const string QueryString_DropDefault_NoUse =
			@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsDefault') = 1)
	drop default [dbo].[{0}]
GO
";

		#endregion

		public string CreateScript(string objectName){	return String.Format(QueryString_CreateScript, objectName);	}
		public string AllDatabaseObjects_(){return QueryString_AllObjects;	}
		public string DatabaseObjects(){return QueryString_GetDatabaseObjects;	}
		public string DatabaseObject(string DBName, string likeChar){return String.Format( QueryString_GetDatabaseObject, DBName, likeChar ) ;	}
		public string DatabasesObjectProperties(string DBName, string objectName){return String.Format( QueryString_DatabaseObjectProperties, DBName, objectName ) ;}
		public string DatabasesReferenceObjects(string objectName){	return String.Format( QueryString_ReferenceObjects, objectName ) ;}
		public string DatabasesReferenceObjectsClear(string objectName){return String.Format( QueryString_ReferenceObjectsClear, objectName ) ;	}
		public string GetJoiningOptions_(string objectName){return String.Format( QueryString_GetJoiningOptions, objectName ) ;}
		public string GetJoiningReferences(string objectName){return String.Format( QueryString_GetJoiningReferences, objectName ) ;}
		public string GetTrigger(string DBName, string objectName){	return String.Format( QueryString_GetTrigger, DBName, objectName ) ;}
		public string GetTriggerScript(string objectName){return String.Format( QueryString_GetTriggerScript, objectName ) ;}
		public string GetUDTs( string DBName ){	return String.Format( QueryString_GetUDT, DBName ) ;}
		public string GetDefaults( string DBName ){	return String.Format( QueryString_GetDefaults, DBName ) ;}
		public string GetRules( string DBName ){return String.Format( QueryString_GetRules, DBName ) ;}
		public string GetObjectScript(string DBName, string objectName){return String.Format( QueryString_GetObjectScript, DBName, objectName );}
		public string GetIndexesList(string DBName, string objectName){return String.Format( QueryString_GetIndexes, DBName, objectName );}
		public string GetIndexesList_RunQuery(string DBName, string objectName){return String.Format( QueryString_GetIndexes_RunQuery, DBName, objectName );}

		// {0} = job_name
		// {1} = database_user_name
		// {2} = server 
		// {3} = description
		// {4} = job steps
		// {5} = enabled
		public string CreateJob(string JobName, string UserName, string Server, string Description, string JobSteps_Text, string Enabled)
		{
			return String.Format( QueryString_CreateJob, JobName, UserName, Server, Description, JobSteps_Text, Enabled );}
		
		// {0} = step_name
		// {1} = step_id
		// {2} = database_name
		// {3} = server
		// {4} = database_user_name
		// {5} = output_file_name
		// {6} = flags
		// {7} = cmdexec_success_code
		// {8} = on_success_action
		// {9} = on_fail_action
		// {10} = on_success_step_id
		// {11} = on_fail_step_id
		// {12} = retry_attempts
		// {13} = retry_interval
		// {14} = step command text
		public string CreateJobStep(string StepName, string StepID, string DBName, string Server, string UserName, string LogFileName, 
			string Flags, string cmdexec_success_code, string on_success_action, string on_fail_action, string on_success_step_id, 
			string on_fail_step_id, string retry_attempts, string retry_interval, string CommandText){
			return String.Format( QueryString_CreateJobStep, StepName, StepID, DBName, Server, UserName, LogFileName, 
				Flags, cmdexec_success_code, on_success_action, on_fail_action, on_success_step_id, 
				on_fail_step_id, retry_attempts, retry_interval, CommandText );}

		public string GetJobStepInfo(string JobName){return String.Format( QueryString_GetJobStepInfo, JobName );}
		public string GetJobs(){ return String.Format( QueryString_GetJobs, "" ); }
		public string GetJobInfo(string JobName){ return String.Format( QueryString_GetJobs, string.Format("@job_name = '{0}'", JobName)); }
		public string StartJob(string JobName){return string.Format( QueryString_StartJob, JobName );}
		public string DeleteJob(string JobName){return string.Format( QueryString_DeleteJob, JobName );}
		public string DropProcedure(string ProcName, string DBName) {return string.Format(QueryString_DropProcedure, ProcName, DBName);} 
		public string DropTrigger(string triggerName, string DBName) {return string.Format(QueryString_DropTrigger, triggerName, DBName);}
		public string DropFunction(string funcName, string DBName) {return string.Format(QueryString_DropFunction, funcName, DBName);}
		public string DropUDT(string UDTName, string DBName) {return string.Format(QueryString_DropUDT, UDTName, DBName);}
		public string DropRule(string RuleName, string DBName) {return string.Format(QueryString_DropRule, RuleName, DBName);}
		public string DropDefault(string DefaultName, string DBName) {return string.Format(QueryString_DropDefault, DefaultName, DBName);}
		public string DropUDT_NoUse(string UDTName) {return string.Format(QueryString_DropUDT_NoUse, UDTName);}
		public string DropRule_NoUse(string RuleName) {return string.Format(QueryString_DropRule_NoUse, RuleName);}
		public string DropDefault_NoUse(string DefaultName) {return string.Format(QueryString_DropDefault_NoUse, DefaultName);}
		public string GetConstraints(string TableName, string DBName) {return string.Format(QueryString_GetConstraints, TableName, DBName);}
		public string GetWho(){return QueryString_GetWho;}
		public string GetDBProperties(string dbName) {return string.Format(QueryString_GetDBProperties, dbName);}
		public string GetAlerts(){return QueryString_GetAlerts;}
		public string GetLinkedServers(){return QueryString_GetLinkedServers;}
		public string GetLocks(){return QueryString_GetLocks;}
		public string GetStatistics(){return QueryString_GetStatistics;}
		public string RefreshView(string viewName, string dbName){return string.Format(QueryString_RefreshView, viewName, dbName);}
		public string GetTableIndexStatistics(string tableName, string indexName){return string.Format(QueryString_GetTableIndexStatistics, tableName, indexName);}
		public string GetTableSpace(string tableName, string dbName){return string.Format(QueryString_GetTableSpace, tableName, dbName);}
		public string GetTableStatistics(string tableName, string dbName){return string.Format(QueryString_GetTableStatistics, tableName, dbName);}
		public string GetUniqueRowColumns(string tableName, string dbName){return string.Format(QueryString_GetRowUniqueColumns, tableName, dbName);}
		public string GetTablePermissions(string tableName, string dbName){return string.Format(QueryString_GetTablePermissions, tableName, dbName);}
		public string UpdateDBStatistics(string dbName) {return string.Format(QueryString_UpdateDBStats, dbName);}
		public string GetSprocParams(string sprocName, string dbName){return string.Format(QueryString_GetSprocParams, sprocName, dbName);}
		public string GetDepends( string objectName, string dbName ){return string.Format(QueryString_GetDepends, objectName, dbName);}
		public string GetForeignKeys( string tableName, string dbName ){return string.Format(QueryString_GetFKs, tableName, dbName);}
		public string GetSysTables(string dbName) {return string.Format(QueryString_GetSysTables, dbName);}
	}

	#endregion	
}
