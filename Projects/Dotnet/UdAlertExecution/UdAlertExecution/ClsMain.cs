using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using uBaseForm;
using DataAccess_Net;
using System.Data;
using System.Data.SqlClient;


namespace UdAlertExecution
{
    class ClsMain
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        public ClsMain(string[] args)
        {
            DataAccess_Net.clsDataAccess._databaseName = args[3];
            DataAccess_Net.clsDataAccess._serverName = args[0];
            DataAccess_Net.clsDataAccess._userID = args[1];
            DataAccess_Net.clsDataAccess._password = args[2];
            oDataAccess = new DataAccess_Net.clsDataAccess();
        }
        public void GetData()
        {      
            //DataSet dsData = new DataSet();
            //DataTable dt= oDataAccess.GetDataTable("select compid,co_name,dbname from vudyog..co_mast", null, 20);
            StringBuilder xx=new StringBuilder();
            xx.Append("");
            xx.Append("declare @SQLCMD as nvarchar(4000)");
            xx.Append("\n");
            xx.Append("set @SQLCMD=''");
            xx.Append("\n");
            xx.Append("declare @lCompID as int,@lName as Varchar(50),@lDbName as varchar(20)");
            xx.Append("\n");
            xx.Append("declare Comp_Fetch cursor for select compid,co_name,dbname from vudyog..co_mast");
            xx.Append("\n");
            xx.Append("open Comp_Fetch");
            xx.Append("\n");
            xx.Append("fetch next from comp_fetch into @lCompID ,@lName ,@lDbName");
            xx.Append("\n");
            xx.Append("while (@@fetch_Status=0)");
            xx.Append("\n");
            xx.Append("begin");
            xx.Append("\n");
            xx.Append("set @SQLCMD=' use '+@lDbName+'");
            xx.Append("\n");
            xx.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[Usp_Alert_Execute]'') AND type in (N''P'', N''PC''))");
            xx.Append("\n");
            xx.Append("begin");
            xx.Append("\n");
            xx.Append("execute Usp_Alert_Execute");
            xx.Append("\n");
            xx.Append("print ''executed in''+db_name()");
            xx.Append("\n");
            xx.Append("end'");
            xx.Append("\n");
            xx.Append("execute sp_executesql @SqlCMD");
            xx.Append("\n");
            xx.Append("fetch next from comp_fetch into @lCompID ,@lName ,@lDbName");
            xx.Append("\n");
            xx.Append("end");
            xx.Append("\n");
            xx.Append("close Comp_Fetch");
            xx.Append("\n");
            xx.Append("deallocate Comp_Fetch");
        string sqlstr = xx.ToString();
           
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 200, true);
        

          
         }
        }
        

    }

