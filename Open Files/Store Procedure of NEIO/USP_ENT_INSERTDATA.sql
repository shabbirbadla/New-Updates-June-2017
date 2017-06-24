Create Procedure [dbo].[USP_ENT_INSERTDATA]
@DbName Varchar(10),
@TblName Varchar(50)
AS
Declare @fldlist Varchar(8000),@SqlCmd NVarchar(max),@colName Varchar(50)

	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 

set @colName=''
SELECT @colName= column_name FROM INFORMATION_SCHEMA.COLUMNS
WHERE
COLUMNPROPERTY -- get columns where is_identity = 1
(
OBJECT_ID(QUOTENAME(table_schema) + '.' + QUOTENAME(table_name)) -- table ID
,column_name
,'isidentity'
) = 1 
and table_name=@TblName

if @colName<>''
Begin
	set @SqlCmd =''
	set @SqlCmd =' Set Identity_Insert '+@DbName+'..'+@TblName+' On'
	set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select '+@fldlist+' From '+@TblName
	set @SqlCmd =@SqlCmd +' Set Identity_Insert '+@DbName+'..'+@TblName+' Off'
	--print @SqlCmd
End
else
Begin
	set @SqlCmd =''
	set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select '+@fldlist+' From '+@TblName
End
	--print @SqlCmd
	Execute sp_Executesql @SqlCmd

