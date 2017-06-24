IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_UpdateColumnDescription]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Usp_UpdateColumnDescription]
Go
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
-- =============================================
-- Author:		Amrendra Kumar Singh.
-- Create date: 18/06/2012
-- Description:	This Stored procedure is useful to set Column Description For SQL Tables
-- Modification Date/By/Reason: 
-- Remark:
-- =============================================

Create Procedure [dbo].[Usp_UpdateColumnDescription]
@TableName varchar(50),@ColExpression varchar(4000)
As
Begin

declare @colName varchar(50),@desc varchar(50), @pos int,@curstr varchar(100),@oldDesc varchar(50)
while len(@ColExpression)>0
	begin

		if len(@ColExpression)>0 and charindex(',',@ColExpression)> 0
		begin
			set @curstr=substring(@ColExpression,1,charindex(',',@ColExpression)-1)
			set @ColExpression=substring(@ColExpression,charindex(',',@ColExpression)+1,len(@ColExpression)-len(@curstr)+1)
		end
		else
		begin
			set @curstr=substring(@ColExpression,1,len(@ColExpression))
			set @ColExpression=''
		end 

		set @pos=charindex(':',@curstr)
		set @ColName=substring(@curstr,1,@pos-1)
		set @desc=substring(@curstr,charindex(':',@curstr)+1,len(@curstr)-len(@ColName))

		if exists(select * from sys.extended_properties e inner join sys.columns c on c.object_id=e.major_id and c.column_id=e.minor_id where  c.name=@ColName and c.object_id=object_id(@TableName))
		begin
			select @oldDesc=cast(e.[value] as varchar(50)) from sys.extended_properties e inner join sys.columns c on c.object_id=e.major_id and c.column_id=e.minor_id where  c.name=@ColName
			EXEC sys.sp_dropextendedproperty @name=N'MS_Description'
			, @level0type=N'SCHEMA',@level0name=N'dbo'
			, @level1type=N'TABLE',@level1name=@TableName
			, @level2type=N'COLUMN',@level2name=@ColName

			Print @ColName + ' : Description already Given for This Column. Deleted ('+@oldDesc+')'
		end
		if exists (select [Name] from syscolumns  where [name] =@ColName and [id]=object_id(@TableName) )
		begin
			EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@Desc --'My Description for Column Here'
					, @level0type=N'SCHEMA',@level0name=N'dbo'
					, @level1type=N'TABLE',@level1name=@TableName --'MyTableName'
					, @level2type=N'COLUMN',@level2name=@ColName --'ColumnName'
			Print @ColName + ' : Description Created for This Column. ('+@Desc+')'
		end
	end
end