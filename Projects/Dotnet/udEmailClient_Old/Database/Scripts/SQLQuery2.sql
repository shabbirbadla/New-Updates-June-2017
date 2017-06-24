EXEC dbo.sp_configure 'clr enabled',1 RECONFIGURE WITH OVERRIDE
go
alter database A131213 set trustworthy on  
go
Drop assembly ReportViewer
create assembly ReportViewer from 'D:\Temp\ClassLibrary1.dll' WITH PERMISSION_SET = safe
Drop procedure Output_to
Create procedure Output_to
@content nvarchar(100)
as external name ReportViewer.[ClassLibrary1.Class1].CLRProcs
Go
Select * From Sysobjects where [Name] = 'Output_to'
exec Output_to 'hello'