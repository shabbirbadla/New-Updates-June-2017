if exists(select [name],* from sysobjects where [name]='usp_column_check_new' and xtype='P')
drop procedure usp_column_check_new
go
---------------------usp_column_check_new-------------
-- Author		: Birendra
-- Created on	: 23/05/2013
-- Description	: To check valid column in table scope of any database
-- Modified		:
-- Modified On	:
------------------------------------------------------
create procedure usp_column_check_new
@dbname varchar(10),@tblname nvarchar(254),@colName nvarchar(254)
as
begin
	begin try
		exec('select case when len(ltrim(rtrim(isnull(b.[name],''''))))>0 then 1 else 0 end as retvalue  from '+@dbname+'..sysobjects a 
		inner join  '+@dbname+'..syscolumns b on a.id=b.id  
		where a.[name]='''+@tblname +''' and b.[name]='''+@colname+'''')
	end try
	begin catch
		select 0 as retvalue
	end catch
end 

