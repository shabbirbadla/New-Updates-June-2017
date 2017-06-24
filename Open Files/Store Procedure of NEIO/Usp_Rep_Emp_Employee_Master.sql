
IF EXISTS( Select [Name] from sysobjects where [Name]='Usp_Rep_Emp_Employee_Master')
BEGIN
DROP PROCEDURE Usp_Rep_Emp_Employee_Master
END
GO
-- =============================================
-- Author:		Ramya.
-- Create date: 18/06/2012
-- Description:	This Stored Procedure is useful for Employee Master Dados reports
-- =============================================
Create PROCEDURE [dbo].[Usp_Rep_Emp_Employee_Master]
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
	execute [Usp_Ent_Get_Column_MsDescription] 'EmployeeMast','EmployeeCode',@NegColList,@ColList =@ColList OUTPUT
	print @ColList

set @ColList=replace(@ColList,'EmpID as [Employee Id],','')
set @ColList=replace(@ColList,'EmployeeCode as uniqueCol,','')
set @ColList=replace(@ColList,'CompId as [Company Id],','')
--set @ColList=replace(@ColList,'Ac_Id','EmployeeMast.Ac_Id')
set @ColList=replace(@ColList,'Ac_Id as [Account Id],','')
--set @ColList=replace(@ColList,'ac_group_Id','EmployeeMast.ac_group_Id')
set @ColList=replace(@ColList,'ac_group_Id as [Account Group Id],','')
set @ColList=replace(@ColList,',Loc_Code as [Location Code]','')
set @ColList=replace(@ColList,'Email','EmployeeMast.Email')


--set @ColList=replace(@ColList,'CompId','EmployeeMast.CompId')



set @ColList='Select '+rtrim(@ColList)+@str+',Loc_Master.Loc_Desc as  [Location],Ac_Mast.Ac_Name as [Account Name],Ac_Group_Mast.Ac_Group_Name as [Group Name] From EmployeeMast  Left Join Loc_Master  on ( EmployeeMast.Loc_Code=Loc_Master.Loc_Code) '
set @ColList=rtrim(@ColList)+' Left Join  Ac_Mast  on (EmployeeMast.Ac_Id=Ac_Mast.Ac_Id) Left Join Ac_Group_Mast  on (EmployeeMast.Ac_Group_Id=Ac_Group_Mast.Ac_Group_Id) order by EmployeeCode '

	print 'aa '+@ColList
	execute Sp_ExecuteSql @ColList


print @SqlCommand
execute Sp_ExecuteSql @SqlCommand

END 
