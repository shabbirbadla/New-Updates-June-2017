IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='Usp_Ent_Emp_Payroll_Declaration')
BEGIN
DROP PROCEDURE Usp_Ent_Emp_Payroll_Declaration
END
GO
-- =============================================
-- Author: Rupesh
-- Create date: 20/07/2012
-- Description:
-- Modify date: 
-- Remark:
-- =============================================

Create Procedure [dbo].[Usp_Ent_Emp_Payroll_Declaration]
@EmployeeCode varchar(15),@Pay_Year varchar(20),@SectionList varchar(1000)
as 
Begin
	--Declare @Fld_Nm varchar(60),@SqlCommand nVarChar(4000),@FldVal NUMERIC(17,2),@ParmDefinition nvarchar(500)
    Declare @Fld_Nm varchar(60),@SqlCommand nVarChar(4000),@FldVal NUMERIC(17,2),@ParmDefinition nvarchar(500),@sDate smalldatetime
		--	if(@Pay_Year!='')
		--	begin
		--    SET @SqlCommand='Select '+@sDate +'=sDate from Emp_Leave_Year_Master where Lv_Year='+Char(39)+@Pay_Year+Char(39) /*Ramya*/
		--    print @SqlCommand
		--	execute Sp_ExecuteSql @SqlCommand
		--    end
		--    else
		--    begin
		--    --set @sDate= '01/01/1900'
		--    --set @sDate= convert(varchar,'01/01/1900',103)
		--	set @sDate=cast('' as smalldatetime)	
		--    print @sDate
		--    end
	print @SectionList
	Select EmployeeCode=@EmployeeCode,Pay_Year=@Pay_Year,ExRec=cast(0 as int),Amount=cast(0 as Decimal(12,3)),a.Section,a.DeclarationDet,a.Fld_Nm,mSortOrd=a.SortOrd,b.SortOrd into #EmpInvDec From Emp_Payroll_Declaration_Master a inner join Emp_Payroll_Section_Master b on (a.Section=b.Section) where 1=2
	--SET @SqlCommand='insert into #EmpInvDec Select EmployeeCode='+Char(39)+rtrim(@EmployeeCode)+Char(39)+',Pay_Year='+Char(39)+rtrim(@Pay_Year)+Char(39)+',ExRec=cast(0 as int),Amount=cast(0 as Decimal(12,3)),a.Section,a.DeclarationDet,a.Fld_Nm,mSortOrd=a.SortOrd,b.SortOrd From Emp_Payroll_Declaration_Master a inner join Emp_Payroll_Section_Master b on (a.Section=b.Section) where a.Section in ('+@SectionList+') order by b.SortOrd,a.SortOrd' 
    SET @SqlCommand='insert into #EmpInvDec Select EmployeeCode='+Char(39)+rtrim(@EmployeeCode)+Char(39)+',Pay_Year='+Char(39)+rtrim(@Pay_Year)+Char(39)+',ExRec=cast(0 as int),Amount=cast(0 as Decimal(12,3)),a.Section,a.DeclarationDet,a.Fld_Nm,mSortOrd=a.SortOrd,b.SortOrd From Emp_Payroll_Declaration_Master a inner join Emp_Payroll_Section_Master b on (a.Section=b.Section) where a.Section in ('+@SectionList+') and (a.IsDeactive=0  or (a.IsDeactive=1 and a.DeactFrom >'+char(39)+cast(getdate() as varchar)+char(39)+'))order by b.SortOrd,a.SortOrd' 
	print @SqlCommand
	execute Sp_ExecuteSql @SqlCommand
	SET @SqlCommand='Declare cur_InvDec cursor for Select distinct Fld_Nm From Emp_Payroll_Declaration_Master where Section in ('+@SectionList+')'
	print @SqlCommand
	execute Sp_ExecuteSql @SqlCommand
	open cur_InvDec
	Fetch Next From cur_InvDec into @Fld_Nm
	while(@@Fetch_Status=0)
	begin
		SET @FldVal=0
		SET @SqlCommand='select @FldValOut=['+@Fld_Nm+'] FROM Emp_Payroll_Declaration_Details where employeecode='+CHAR(39)+@EmployeeCode+CHAR(39)+' and Pay_Year='+CHAR(39)+@Pay_Year+CHAR(39)
		SET @ParmDefinition = N' @FldValOut Decimal(17,2) OUTPUT '
		EXECUTE sp_executesql @SqlCommand,@ParmDefinition,@FldValOut=@FldVal OUTPUT
		print @SqlCommand
		PRINT @FldVal
		SET @SqlCommand='UPDATE #EmpInvDec SET Amount='+CAST(@FldVal AS varchar)+' where Fld_Nm=' +CHAR(39)+@Fld_Nm+CHAR(39)
		PRINT @SqlCommand
		EXECUTE sp_executesql  @SqlCommand
		Fetch Next From cur_InvDec into @Fld_Nm
	end
	close cur_InvDec
	DeAllocate cur_InvDec
	Select * From #EmpInvDec
	--Execute Usp_Ent_Emp_Payroll_Declaration 'R00003','2012','''Income other than salary'''
End


