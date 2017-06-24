IF EXISTS( Select [Name] from sysobjects where [Name]='Usp_Rep_Emp_Leave_Maintenance_Dados')
BEGIN
DROP PROCEDURE Usp_Rep_Emp_Leave_Maintenance_Dados
END
GO
-- =============================================
-- Author:		Pratap.
-- Create date: 24/05/2012
-- Description:	This Stored Procedure is useful for Leave Maintance Details in Dados reports
-- =============================================
Create procedure Usp_Rep_Emp_Leave_Maintenance_Dados

@Loc_Desc varchar(30),
@Year varchar(30),
@Month varchar(30),
@EmployeeName varchar(100)

as
begin


Declare @mth int,@SqlCommand nvarchar(4000),@Att_Code varchar(4),@Att_Nm varchar(60)
SELECT @mth=DATEPART(mm,CAST(@Month+ ' 1900' AS DATETIME))

Declare cur_AttSet Cursor for Select Att_Code,Att_Nm From Emp_Attendance_Setting where isLeave=1 and lDeactive=0 order by SortOrd

open cur_AttSet
Fetch Next From cur_AttSet into @Att_Code,@Att_Nm
set @SqlCommand=''
while(@@Fetch_Status=0)
begin
	set @SqlCommand=rtrim(@SqlCommand)+ ',['+rtrim(@Att_Nm)+' Op Bal]='+rtrim(@Att_Code)+'_OpBal'
	set @SqlCommand=rtrim(@SqlCommand)+ ',['+rtrim(@Att_Nm)+' Credit]='+rtrim(@Att_Code)+'_Credit'
	set @SqlCommand=rtrim(@SqlCommand)+ ',['+rtrim(@Att_Nm)+' Availed]='+rtrim(@Att_Code)+'_Availed'
	set @SqlCommand=rtrim(@SqlCommand)+ ',['+rtrim(@Att_Nm)+' Encash]='+rtrim(@Att_Code)+'_Encash'
	set @SqlCommand=rtrim(@SqlCommand)+ ',['+rtrim(@Att_Nm)+' Balance]='+rtrim(@Att_Code)+'_Balance'
	Fetch Next From cur_AttSet into @Att_Code,@Att_Nm
end
close cur_AttSet
DeAllocate cur_AttSet
set @SqlCommand='Select l.Pay_Year as [Year],DateName( month , DateAdd( month , cast(l.Pay_Month as int), 0 )-1  ) as [Month]  ,l.EmployeeCode,e.EmployeeName as [Employee Name]'+rtrim(@SqlCommand)
set @SqlCommand=rtrim(@SqlCommand)+' '+',e.Department,lc.Loc_Desc as Location'
set @SqlCommand=rtrim(@SqlCommand)+' '+'From Emp_Leave_Maintenance l'
set @SqlCommand=rtrim(@SqlCommand)+' '+'inner Join EmployeeMast e on (e.EmployeeCode=l.EmployeeCode)'
set @SqlCommand=rtrim(@SqlCommand)+' '+'Left Join Loc_Master lc on (e.Loc_Code=lc.Loc_Code)'
set @SqlCommand=rtrim(@SqlCommand)+' '+'where l.Pay_Year='+char(39)+@Year+char(39)
set @SqlCommand=rtrim(@SqlCommand)+' '+' and Pay_Month='+cast(@mth as varchar)


if isnull(@Loc_Desc,'')<>''
begin
	set @SqlCommand=rtrim(@SqlCommand)+' '+' and lc.Loc_Desc='+char(39)+@Loc_Desc+char(39)
end

if isnull(@EmployeeName,'')<>''
begin
	set @SqlCommand=rtrim(@SqlCommand)+' '+' and E.EmployeeName='+char(39)+@EmployeeName+char(39)
end

print @SqlCommand
execute Sp_ExecuteSql @SqlCommand


end


