
IF EXISTS( Select [Name] from sysobjects where [Name]='Usp_Rep_Emp_Monthly_Muster_Dados')
BEGIN
DROP PROCEDURE Usp_Rep_Emp_Monthly_Muster_Dados
END
GO
-- =============================================
-- Author:		Ramya.
-- Create date: 24/05/2012
-- Description:	This Stored Procedure is useful for Monthly Muster Details in Dados reports
-- =============================================
Create procedure [dbo].[Usp_Rep_Emp_Monthly_Muster_Dados]
@Loc_Desc varchar(20),
@Year varchar(20),
@Month varchar(30),
@EmployeeName varchar(100)
as 

begin 
Declare @mth int,@SqlCommand nvarchar(4000)

SELECT @mth=DATEPART(mm,CAST(@Month+ ' 1900' AS DATETIME))

declare @AttCode VARCHAR(30),@AttNm VARCHAR(30)
declare @LeaveDet varchar(1000)
set  @LeaveDet=''
 declare c1 cursor for select Att_Code,Att_Nm from Emp_Attendance_Setting  order by sortord
 open c1
	 fetch next from c1 into @AttCode,@AttNm
        while(@@fetch_status=0)
		begin
			if(@LeaveDet='')
			begin
             set @LeaveDet= @AttCode+' as ['+@AttNm+']'
            end
			else
			begin
             set @LeaveDet= @LeaveDet+','+@AttCode+' as ['+@AttNm+']'
			end

          fetch next from c1 into @AttCode,@AttNm
        end

close c1
deallocate c1
print 'Leave Details '+@LeaveDet


set @SqlCommand='select E.EmployeeName as [Employee Name],E.EmployeeCode as [Employee Code],L.Loc_Desc as [Location],E.Department,E.Category,M.Pay_Year as [Year],DateName( month , DateAdd( month , cast(M.Pay_Month as int), 0 )-1  ) as [Month],'
set @SqlCommand=rtrim(@SqlCommand)+' '+@LeaveDet
set @SqlCommand=rtrim(@SqlCommand)+' '+'from Emp_Monthly_Muster M left join EmployeeMast E on (M.Employeecode=E.employeecode)' 
set @SqlCommand=rtrim(@SqlCommand)+' '+'left join Loc_Master L on (E.Loc_code=L.Loc_Code)'
set @SqlCommand=rtrim(@SqlCommand)+' '+'where M.Pay_Year='+char(39)+@Year+char(39)
set @SqlCommand=rtrim(@SqlCommand)+' '+' and Pay_Month='+cast(@mth as varchar)

if isnull(@EmployeeName,'')<>''
begin
	set @SqlCommand=rtrim(@SqlCommand)+' '+' and E.EmployeeName='+char(39)+@EmployeeName+char(39)
end

if isnull(@Loc_Desc,'')<>''
begin
	set @SqlCommand=rtrim(@SqlCommand)+' '+' and L.Loc_Desc='+char(39)+@Loc_Desc+char(39)
end
print @SqlCommand
execute Sp_ExecuteSql @SqlCommand



 
--M.MonthDays as [Month Days],M.SalPaidDays as [Salary Paid Days],
--M.PR as [Present Days],M.LOP as [Lack of Paymet],M.AB as [AbSent],M.HA as [Half Day Absent],M.WO as [Weekly Off],
--M.HD as [Holi Days],M.CL as [Casual Leave],M.PL as [Paid Leave],M.SL as [Seek Leave],M.OH as [Optional Holiday],
--M.SP as [Special Leave],M.CO as [Compensatory Off],M.CW as [Compensatory Work],M.OD as [Out Station Duty]
--
--from Emp_Monthly_Muster M 
--left join EmployeeMast E on (M.Employeecode=E.employeecode) 
--left join Loc_Master L on (E.Loc_code=L.Loc_Code) 
--where M.Pay_Year=@Year  and E.EmployeeName=@EmployeeName and Pay_Month=@mth

--Execute Sp_ExecuteSql @SqlCommand
end




