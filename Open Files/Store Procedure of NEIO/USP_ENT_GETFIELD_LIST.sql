Create Procedure USP_ENT_GETFIELD_LIST
@TBLName Varchar(50)
,@fldlst Varchar(8000) output
AS
Declare @colName Varchar(50)
Declare fldlist Cursor for
Select [name] From SysColumns Where [Id]=(Select Id From sysObjects Where [Name]=@TBLName)

Open fldlist
Fetch Next From fldlist Into @colName
set @fldlst=' '
While @@Fetch_Status=0
Begin
	--print @colName
	set @fldlst=@fldlst+'['+rtrim(@colName)+'],'
Fetch Next From fldlist Into @colName
End
Close fldlist 
Deallocate fldlist
set @fldlst=left(@fldlst,len(@fldlst)-1)+' '
--return @fldlst

