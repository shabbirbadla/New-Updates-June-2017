
IF EXISTS( Select [Name] from sysobjects where [Name]='Usp_Rep_Emp_Employee_Family_Details_Dados')
BEGIN
DROP PROCEDURE Usp_Rep_Emp_Employee_Family_Details_Dados
END
GO
-- =============================================
-- Author:		Ramya.
-- Create date: 18/06/2012
-- Description:	This Stored Procedure is useful for Employee Master Dados reports
-- =============================================
Create PROCEDURE [dbo].[Usp_Rep_Emp_Employee_Family_Details_Dados]
AS
BEGIN

Declare @SqlCommand nvarchar(4000),@HeadNm varchar(50),@FldNm varchar(10),@str varchar(5000),@NegColList nvarchar(4000)
set @str=''
	set @NegColList=''''''
	print @NegColList
	declare c1 cursor for select rtrim(Head_Nm) ,rtrim(Fld_Nm)  From Lother where E_Code='EM' order by Serial
	open c1
	fetch next from c1 into @HeadNm,@FldNm
	while(@@fetch_Status=0)
	begin
	set @NegColList=rtrim(@NegColList)+','+char(39)+rtrim(@FldNm)+char(39)

		set @str=rtrim(@str)+','+@FldNm+' as ['+@HeadNm+']'

	fetch next from c1 into @HeadNm,@FldNm
	end
	close c1
	deallocate c1



--@str=replace(@str,'Ac_Id','')
--@str=replace(@str,'Ac_Group_Id','')

print @str
set @NegColList='('+rtrim(@NegColList)+')'
print @NegColList
	Declare @ColList nvarchar(4000)
	print 'r1 '+@ColList
	execute [Usp_Ent_Get_Column_MsDescription] 'Emp_Family_Details','EmployeeCode',@NegColList,'b.',@ColList =@ColList OUTPUT
	print 'r2 '+@ColList

set @ColList=replace(@ColList,'b.EmployeeCode as uniqueCol,','')
set @ColList=replace(@ColList,'[Id],','')
--set @ColList=replace(@ColList,'Loc_Code','EmployeeMast.Loc_Code')
--set @ColList=replace(@ColList,'[Email]','EmployeeMast.Email')
--set @ColList=replace(@ColList,'Ac_Id','EmployeeMast.Ac_Id')
--set @ColList=replace(@ColList,'ac_group_Id','EmployeeMast.ac_group_Id')
--set @ColList=replace(@ColList,'CompId','EmployeeMast.CompId')


set @ColList='Select '+rtrim(@ColList)+ ',a.employeename From EmployeeMast a Left Join Emp_Family_Details b on ( a.employeecode=b.employeecode) '
--set @ColList=rtrim(@ColList)+' Left Join  Ac_Mast  on (EmployeeMast.Ac_Id=Ac_Mast.Ac_Id) Left Join Ac_Group_Mast  on (EmployeeMast.Ac_Group_Id=Ac_Group_Mast.Ac_Group_Id) order by EmployeeCode'

	print 'aa '+@ColList
	execute Sp_ExecuteSql @ColList
	
	



print 'rr '+@SqlCommand
execute Sp_ExecuteSql @SqlCommand

END 
