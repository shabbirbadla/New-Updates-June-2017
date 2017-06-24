IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Rep_Emp_Loan_Details_DADOS]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Rep_Emp_Loan_Details_DADOS]
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 01/11/2012
-- Description:	This Stored procedure is useful to Generate Employee Loan Details DADOS Report.
-- Modification Date/By/Reason:
-- Remark:
-- =============================================
--@EmployeeCode varchar(30),@Pay_Year varchar(30),@cPay_Month Varchar(30)

CREATE procedure [dbo].[Usp_Rep_Emp_Loan_Details_DADOS]
@Pay_Year varchar(20),
@EmployeeName varchar(100)
AS
Begin
declare @EmployeeCode varchar(30),@SqlCommand nvarchar(4000)
select @EmployeeCode=EmployeeCode from employeemast where employeename=@EmployeeName  /*Ramya 02/11/12*/

	set @SqlCommand='Select [Process Ref.No.]=a.Inv_No,[Process Ref. Dt.]=a.Date,[Employee Code]=a.EmployeeCode'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Employee Name]=(Case when isNull(em.pMailName,'''')='''' then em.EmployeeName else em.pMailName end)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Request No.]=r.Inv_No,[Request Dt.]=r.Date,[Description]=C.Head_Nm,[Short Desc.]=c.Short_Nm,[Request Amount]=R.Amount,[Due Amount]=a.Due_Amt'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Approval Status]=(Case when Req_Stat=''A'' then ''Approved'' else (Case when Req_Stat=''H'' then ''Hold'' else ''Rejected'' end) end)'
	--set @SqlCommand=rtrim(@SqlCommand)+' '+',[Loan Type]=(Case when Req_Stat=''F'' then ''Flat'' else (Case when Req_Stat=''D'' then ''Demnising'' else ''None'' end) end)'
set @SqlCommand=rtrim(@SqlCommand)+' '+',[Loan Type]=(Case when Loan_Type=''F'' then ''Flat'' else (Case when Loan_Type=''D'' then ''Demnising'' else ''None'' end) end)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Approved/Rejeted By]=(Case when isNull(em1.pMailName,'''')='''' then em1.EmployeeName else em1.pMailName end)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Loan Amount]=A.Loan_Amt,[Interest %]=a.Int_Per,[No. of Installment]=a.InstNo,[Approval Date]=a.AppDate,[Deduction Date]=a.Ded_Date'
	set @SqlCommand=rtrim(@SqlCommand)+' '+',[Cheque No]=A.Cheque_No,[Cheque Date]=A.Cheque_Dt'
    set @SqlCommand=rtrim(@SqlCommand)+' '+',[Bank Name]=(Case when isNull(am.MailName,'''')='''' then am.Ac_Name else am.MailName end),a.Remark'
	
	set @SqlCommand=rtrim(@SqlCommand)+' '+' from Emp_Loan_Advance a '
	set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Loan_Advance_request r on (a.Req_Id=r.Id)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast em on (a.EmployeeCode=em.EmployeeCode)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast em1 on (a.AppByCode=em1.EmployeeCode)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join Ac_Mast am on (a.Bank_Id=am.Ac_Id)'
	set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)'

set @SqlCommand=rtrim(@SqlCommand)+' '+'where 1=1 '
	
	if isnull(@Pay_Year,'')<>''
	begin
		set @SqlCommand=rtrim(@SqlCommand)+' '+' and year(a.Date)='+char(39)+@Pay_Year+char(39)
	end
	if isnull(@EmployeeName,'')<>''
	begin
		set @SqlCommand=rtrim(@SqlCommand)+' '+' and Em.EmployeeName='+char(39)+@EmployeeName+char(39)
	end
	set @SqlCommand=rtrim(@SqlCommand)+' '+'order by a.Date,a.Inv_No'
print @SqlCommand
execute Sp_ExecuteSql @SqlCommand
End