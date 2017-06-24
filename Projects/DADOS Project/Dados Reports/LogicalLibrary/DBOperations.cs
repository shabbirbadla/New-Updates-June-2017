using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LogicalLibrary
{
    public class DBOperations
    {
        #region Local Veriables

        SqlConnection con = null;
        SqlCommand Command = null;
        SqlDataAdapter Adapter = null;
        DataSet DS = null;

        #endregion

        public DBOperations()
        {

        }

        public DataSet GetColumnsDetails(string ReportID, string QueryID, string reportConString, string DatsSetTableName)
        {
            DS = new DataSet();
            con = new SqlConnection(reportConString);
            Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            // Command.CommandText = "select usrl.ColID as [ColumnID],usrl.QryID as [QueryID],usrl.RepID as [ReportID],usrl.RepLvlID as [LevelID],usrl.IsGrouped as [Is Grouped],usrl.IsFreezing as [Is Freezing],usrl.ColWidth as [Column Width],usrl.IsSummury as [Is Summury],usrl.IsDisplayed as [Is Displayed],usc.ColumnNames as [Column Name], usc.ColumnDataType as [Column DataType],usc.ColumnOrder as [Column Order] from uscrl usrl inner join uscol usc on usrl.colid=usc.colid where usrl.qryid=" + QueryID + " and usrl.repid='" + ReportID + "' order by usrl.colid";//Comment by Archana Khade 05/4/2012 for TKT-3143 
            //Command.CommandText = "select usrl.ColID as [ColumnID],usrl.QryID as [QueryID],usrl.RepID as [ReportID],usrl.RepLvlID as [LevelID],usrl.IsGrouped as [Is Grouped],usrl.IsFreezing as [Is Freezing],usrl.ColWidth as [Column Width],usrl.IsSummury as [Is Summury],usrl.IsDisplayed as [Is Displayed],usc.ColumnNames as [Column Name],usc.ColumnCaption as[Column Caption], usc.ColumnDataType as [Column DataType],usc.ColumnOrder as [Column Order] from uscrl usrl inner join uscol usc on usrl.colid=usc.colid where usrl.qryid=" + QueryID + " and usrl.repid='" + ReportID + "' order by usrl.colid";//Added by Archana Khade on 05/04/2012 for TKT-3143    // Commented By Shrikant S. on 06/06/2012 for Bug-4522
            Command.CommandText = "select usrl.ColID as [ColumnID],usrl.QryID as [QueryID],usrl.RepID as [ReportID],usrl.RepLvlID as [LevelID],usrl.IsGrouped as [Is Grouped],usrl.IsFreezing as [Is Freezing],usrl.ColWidth as [Column Width],usrl.IsSummury as [Is Summury],usrl.IsDisplayed as [Is Displayed],usc.ColumnNames as [Column Name],usc.ColumnCaption as[Column Caption], usc.ColumnDataType as [Column DataType],usc.ColumnOrder as [Column Order],usrl.grouporder from uscrl usrl inner join uscol usc on usrl.colid=usc.colid where usrl.qryid=" + QueryID + " and usrl.repid='" + ReportID + "' order by usrl.IsGrouped desc,case when usrl.IsGrouped=1 then grouporder else columnorder end"; //Added By Shrikant S. on 06/06/2012 for Bug-4522
            Command.Connection = con;
            Command.Parameters.Clear();

            Adapter = new SqlDataAdapter(Command);
            Adapter.Fill(DS, DatsSetTableName);

            return DS;
        }

        public DataSet GetReportTypeAndName(string reportConString, string ReportID, string DatsSetTableName)
        {
            DS = new DataSet();
            con = new SqlConnection(reportConString);
            Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "Select repid as ReportID,repnm as ReportName,repty as ReportType from usrep where repid='" + ReportID + "'";
            Command.Connection = con;
            Command.Parameters.Clear();

            Adapter = new SqlDataAdapter(Command);
            Adapter.Fill(DS, DatsSetTableName);

            return DS;
        }

        public DataSet GetReportsLevelCount(string reportConString, string ReportID, string DatsSetTableName)
        {
            DS = new DataSet();
            con = new SqlConnection(reportConString);
            Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select count(repid) as levels from usqry where repid='" + ReportID + "'";
            Command.Connection = con;
            Command.Parameters.Clear();

            Adapter = new SqlDataAdapter(Command);
            Adapter.Fill(DS, DatsSetTableName);

            return DS;
        }

        public DataSet GetReportLevelsAndTypes(string reportConString, string ReportID, string DatsSetTableName)
        {
            DS = new DataSet();
            con = new SqlConnection(reportConString);
            Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            //Command.CommandText = "select a.lvlid as LevelID,a.repid as ReportID,a.repqr as ReportQuery,a.prycl as PrimaryKeyValu,a.seccl as SecunderyKeyValu,a.lvlty as ReportLevelTypeID,b.lvlty as ReportDisplayType,b.lvlnm as ReportDisplayName from usrlv a inner join (SELECT [lvlty],[lvlnm],[lvltid] FROM [uslty] where lvltid in (select lvlty from usrlv where repid = " + ReportID + "))b on a.lvlty=b.lvltid";
            Command.CommandText = "SELECT uq.RepLvlID as LevelID,uq.repID as ReportID,uq.qryID as QueryID,uq.repQry as ReportQuery,us1.prycl as PrimaryKeyValu,us1.seccl as SecunderyKeyValu,us1.lvltyp as ReportLevelTypeID,usy.lvlty as ReportDisplayType,usy.lvlnm as ReportDisplayName from usqry uq inner join usrlv us1 on uq.qryid=us1.qryid inner join uslty usy on us1.lvltyp=usy.lvltid where uq.repid='" + ReportID + "'";
            Command.Connection = con;
            Command.Parameters.Clear();
            
            Adapter = new SqlDataAdapter(Command);
            Adapter.Fill(DS, DatsSetTableName);

            return DS;
        }
        public DataTable GetReportLayOuts(string reportConString,string UserName, string ReportId)
        {
            DataTable ldt=new DataTable("LayOut_vw");
            SqlCommand lcmd = new SqlCommand();
            lcmd.CommandType = CommandType.Text;
            lcmd.Connection = new SqlConnection(reportConString);
            string sqlStr=string.Empty;
            //sqlStr ="Select  LayOutId,UserName,RepId, LayOutName, Level1_Layout=convert(varchar(max),Level1_Layout), Level2_Layout=convert(varchar(max)";
            //sqlStr =sqlStr +" "+",Level2_Layout), Level3_Layout=convert(varchar(max),Level3_Layout), Level4_Layout=convert(varchar(max),Level4_Layout), ";
            //sqlStr = sqlStr + " " + "Level5_Layout=convert(varchar(max),Level5_Layout), Level6_Layout=convert(varchar(max),Level6_Layout), ";
            //sqlStr = sqlStr + " " + "Level7_Layout=convert(varchar(max),Level7_Layout), isDefault ";
            //sqlStr = sqlStr + " " + "From ReportLayOut Where RepId='" + ReportId + "' and UserName='" + UserName + "'";
            sqlStr = "Select * From ReportLayOut Where RepId='" + ReportId + "' and UserName='" + UserName + "'";
            lcmd.CommandText = sqlStr;
            SqlDataAdapter lda = new SqlDataAdapter(lcmd);
            lda.Fill(ldt);
            return ldt;
        }
    }
}