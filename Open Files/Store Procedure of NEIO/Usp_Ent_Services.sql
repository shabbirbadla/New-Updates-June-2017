create Procedure [dbo].[Usp_Ent_Services]
@Sdate smalldatetime,@Edate smalldatetime,@dbname Varchar(25)--@dbname Varchar(10) change by sandeep for bug-12216 on 18-jun-13
as 
Declare @mSdate smalldatetime,@mEdate smalldatetime,@Name Varchar(200)
Declare @SQLCOMMAND Nvarchar(2000)
Select * Into #tmpService from Sertax_mast where 1=2

set @SQLCOMMAND='Insert Into #tmpService Select * from '+ltrim(rtrim(@dbname))+'..Sertax_mast Where Edate='''+convert(Varchar(50),dateadd(d,-1,@Sdate))+''' '
Execute sp_executesql @SQLCOMMAND

Update #tmpService set Sdate=@Sdate,Edate=@Edate

Declare ServiceCursor cursor for 
Select Name,Sdate,Edate from #tmpService
Open ServiceCursor
Fetch Next From ServiceCursor Into @Name,@mSdate,@mEdate

While @@Fetch_status=0
Begin
	set @SQLCOMMAND='If Not Exists(Select [Name] from '+ltrim(rtrim(@dbname))+'..Sertax_Mast where [Name]='''+case when charindex('''',@Name)>0 then replace(@Name,'''','''''') else @Name end+''' and sdate='''+convert(varchar(50),@mSdate)+''' and Edate='''+convert(varchar(50),@mEdate)+''' )
	Begin
		Insert Into '+ltrim(rtrim(@dbname))+'..Sertax_Mast Select * from #tmpService where [Name]='''+case when charindex('''',@Name)>0 then replace(@Name,'''','''''') else @Name end+''' and sdate='''+convert(varchar(50),@mSdate)+''' and Edate='''+convert(varchar(50),@mEdate)+'''
	End '
	print @SQLCOMMAND
	Execute sp_executesql @SQLCOMMAND

	Fetch Next From ServiceCursor Into @Name,@mSdate,@mEdate
End
Close ServiceCursor 
Deallocate ServiceCursor 

Drop table #tmpService





