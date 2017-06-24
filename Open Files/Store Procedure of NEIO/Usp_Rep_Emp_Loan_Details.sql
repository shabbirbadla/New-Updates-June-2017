IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Rep_Emp_Loan_Details]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Rep_Emp_Loan_Details]
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 01/11/2012
-- Description:	This Stored procedure is useful to Generate Employee Loan Details Report.
-- Modification Date/By/Reason:
-- Remark:
-- =============================================
CREATE procedure [dbo].[Usp_Rep_Emp_Loan_Details]
@Tran_Cd int
AS
Begin
	print @Tran_Cd
	Select a.Inv_No,a.Date,a.EmployeeCode
	,EmployeeName=(Case when isNull(em.pMailName,'')='' then em.EmployeeName else em.pMailName end)
	,ReqNo=r.Inv_No,ReqDt=r.Date,C.Head_Nm,c.Short_Nm,R.Amount
	,ApproveStat=(Case when Req_Stat='A' then 'Approved' else (Case when Req_Stat='H' then 'Hold' else 'Rejected' end) end)
	,LoanType=(Case when Req_Stat='F' then 'Flat' else (Case when Req_Stat='D' then 'Demnising' else 'None' end) end)
	,ApprovedBy=(Case when isNull(em1.pMailName,'')='' then em1.EmployeeName else em1.pMailName end)
	,A.Loan_Amt,a.Int_Per,a.InstNo,a.Ded_Date,a.AppDate
	,A.Cheque_No,A.Cheque_Dt
	,BankNm=(Case when isNull(am.MailName,'')='' then am.Ac_Name else am.MailName end)
	,ld.Pay_Year,ld.Pay_Month,ld.Op_Bal,ld.Inst_Amt,ld.Interest,ld.Tot_Amt,ld.Repay_Amt,ld.Proj_Repay,ld.Cl_Bal
	,a.Remark,cMonth=isnull(datename(month,dateadd(month, ld.Pay_Month - 1, 0)),'''')
	from Emp_Loan_Advance a 
	inner join Emp_Loan_Advance_request r on (a.Req_Id=r.Id)
	inner join EmployeeMast em on (a.EmployeeCode=em.EmployeeCode)
	inner join EmployeeMast em1 on (a.AppByCode=em1.EmployeeCode)
	Left join Ac_Mast am on (a.Bank_Id=am.Ac_Id)
	inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)
	Left join Emp_Loan_Advance_Details ld on (a.Entry_Ty=ld.Entry_Ty and A.Tran_cd=ld.Tran_Cd)
	where a.Tran_Cd=@Tran_Cd
	order by Pay_Year,Pay_Month
End