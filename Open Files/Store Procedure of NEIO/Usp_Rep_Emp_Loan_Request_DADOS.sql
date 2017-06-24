IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Rep_Emp_Loan_Request_DADOS]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Rep_Emp_Loan_Request_DADOS]
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 01/11/2012
-- Description:	This Stored procedure is useful to Generate Employee  Loan and Advance Request Report.
-- Modification Date/By/Reason:
-- Remark:
-- =============================================
--@EmployeeCode varchar(30),@Pay_Year varchar(30),@cPay_Month Varchar(30)
CREATE procedure [dbo].[Usp_Rep_Emp_Loan_Request_DADOS]
--@Loc_Code varchar(20),
@Pay_Year varchar(20),
--@cPay_Month varchar(30),
@EmployeeName varchar(100)
AS
Begin
declare @EmployeeCode varchar(30),@SqlCommand nvarchar(4000)
select @EmployeeCode=EmployeeCode from employeemast where employeename=@EmployeeName  /*Ramya 02/11/12*/

	set @SqlCommand='Select [Employee Code]=a.EmployeeCode'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Employee Name]=(Case when isNull(em.pMailName,'''')='''' then em.EmployeeName else em.pMailName end)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Request No.]=a.Inv_No,[Request Dt.]=a.Date,[Description]=C.Head_Nm,[Short Desc.]=c.Short_Nm,[Request Amount]=a.Amount,[Request Remark]=a.Remark'
--	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Approval Status]=(Case when isnull(Req_Stat,'''')=''A'' then ''Approved'' else (Case when isnull(Req_Stat,'''')=''H'' then ''Hold'' else (Case when isnull(Req_Stat,'''')=''R'' then ''Rejected'' else ''Not Processed'' end) end) end)'
--	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Process Ref.No.]=isnull(la.Inv_No,''''),[Process Ref. Dt.]=isnull(la.Date,''''),[Loan Type]=(Case when isnull(Req_Stat,'''')=''F'' then ''Flat'' else (Case when isnull(Req_Stat,'''')=''D'' then ''Demnising'' else ''None'' end) end)'
--	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Approved/Rejeted By]=(Case when isNull(em.pMailName,'''')='''' then isnull(em.EmployeeName,'''') else isnull(em.pMailName,'''') end)'
	--set @SqlCommand=rtrim(@SqlCommand)+' '+',[Loan Amount]=isnull(Loan_Amt,0),Int_Per=cast(0 as Decimal(10,3)),InstNo=cast(0 as int),Ded_Datecast=cast('''' as SmallDateTime),AppDate=cast('''' as SmallDateTime)'
--	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Approval Date]=isnull(la.AppDate,'''')'
--	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Approved/Rejeted Remark]=isnull(la.Remark,'''')'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'from Emp_Loan_Advance_request a'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast em on (a.EmployeeCode=em.EmployeeCode)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)'
--	set @SqlCommand=rtrim(@SqlCommand)+' '+'Left Join Emp_Loan_Advance la  on (a.Id=la.Req_Id)' 
--	set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join EmployeeMast em1 on (la.AppByCode=em1.EmployeeCode)'
set @SqlCommand=rtrim(@SqlCommand)+' '+'where 1=1 '
if isnull(@Pay_Year,'')<>''
	begin
		set @SqlCommand=rtrim(@SqlCommand)+' '+' and year(a.Date)='+char(39)+@Pay_Year+char(39)
	end
	if isnull(@EmployeeName,'')<>''
	begin
		set @SqlCommand=rtrim(@SqlCommand)+' '+' and Em.EmployeeName='+char(39)+@EmployeeName+char(39)
	end
	--Where a.Id=@Tran_Cd
	--Execute Usp_Rep_Emp_Loan_Request_DADOS '',''
	set @SqlCommand=rtrim(@SqlCommand)+' '+'order by a.Date,a.Inv_No'
print @SqlCommand
execute Sp_ExecuteSql @SqlCommand
End