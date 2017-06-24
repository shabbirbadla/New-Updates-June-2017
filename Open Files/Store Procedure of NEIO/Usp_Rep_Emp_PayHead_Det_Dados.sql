IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='Usp_Rep_Emp_PayHead_Det_Dados')
BEGIN
DROP PROCEDURE [Usp_Rep_Emp_PayHead_Det_Dados]
END
GO
-- =============================================
-- Created By : Rupesh Prajapati
-- Create date: 19/03/2012
-- Description:	This Stored Procedure is used by Employee Report for getting Earning & Deduction Details
-- Remark	  : 
-- Modified By and Date : [Usp_Rep_Emp_Pay_Head_Details]'',''
-- =============================================
CREATE PROCEDURE [dbo].[Usp_Rep_Emp_PayHead_Det_Dados]
AS

Declare @FCON as NVARCHAR(2000)

BEGIN
	DECLARE @fld_nm VARCHAR(30),@Short_Nm VARCHAR(60),@SqlCommand NVARCHAR(4000)
	set @SqlCommand='Select A.EmployeeName as [Employee Name],a.EmployeeCode as [Employee Code]'
	DECLARE @ParmDefinition nvarchar(500);
	Declare cur_EmpPayHead cursor for Select a.Fld_Nm,a.Short_Nm From Emp_Pay_Head_Master a inner join Emp_Pay_Head b on (a.HeadTypeCode=b.HeadTypeCode) order by b.SortOrd,a.SortOrd
	open cur_EmpPayHead
	Fetch next from cur_EmpPayHead into @Fld_Nm	,@Short_Nm
	while(@@Fetch_Status=0)
	begin
		print  @Fld_Nm	+' '+@Short_Nm
		set @SqlCommand=rtrim(@SqlCommand)+','+rtrim(@Fld_Nm)+'YN as ['+rtrim(@Short_Nm)+' Appicable],'+rtrim(@Fld_Nm)+' as ['+rtrim(@Short_Nm)+']'
		print @SqlCommand
     Fetch next from cur_EmpPayHead into @Fld_Nm	,@Short_Nm
	end
	Close cur_EmpPayHead
	Deallocate  cur_EmpPayHead
	set @SqlCommand=rtrim(@SqlCommand)+' From EmployeeMast a inner join Emp_Pay_Head_Details b on (a.EmployeeCode=b.EmployeeCode) order by a.EmployeeCode' 
	print @SqlCommand
	Execute Sp_ExecuteSql @SqlCommand
	

	
END



