IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Rep_Emp_Loan_Request]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Rep_Emp_Loan_Request]
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
CREATE procedure [dbo].[Usp_Rep_Emp_Loan_Request]
@Tran_Cd int
AS
Begin
	print @Tran_Cd
	Select Inv_No=isnull(la.Inv_No,''),Date=isnull(la.Date,''),a.EmployeeCode
	,EmployeeName=(Case when isNull(em.pMailName,'')='' then em.EmployeeName else em.pMailName end)
	,ReqNo=a.Inv_No,ReqDt=a.Date,C.Head_Nm,c.Short_Nm,a.Amount,Req_Remark=a.Remark
	,ApproveStat=(Case when isnull(Req_Stat,'')='A' then 'Approved' else (Case when isnull(Req_Stat,'')='H' then 'Hold' else (Case when isnull(Req_Stat,'')='R' then 'Rejected' else 'Not Processed' end) end) end)
	,LoanType=(Case when isnull(Loan_Type,'')='F' then 'Flat' else (Case when isnull(Loan_Type,'')='D' then 'Demnising' else 'None' end) end)
	,ApprovedBy=(Case when isNull(em1.pMailName,'')='' then isnull(em1.EmployeeName,'') else isnull(em1.pMailName,'') end)
	,Loan_Amt=isnull(Loan_Amt,0)--,Int_Per=cast(0 as Decimal(10,3)),InstNo=cast(0 as int),Ded_Datecast=('' as SmallDateTime),AppDate=cast('' as SmallDateTime)
	,AppDate=isnull(la.AppDate,'')
	,Remark=isnull(la.Remark,'')
	from Emp_Loan_Advance_request a
	inner join EmployeeMast em on (a.EmployeeCode=em.EmployeeCode)
	inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)
	Left Join Emp_Loan_Advance la  on (a.Id=la.Req_Id) 
	Left join EmployeeMast em1 on (la.AppByCode=em1.EmployeeCode)
	Where a.Id=@Tran_Cd

	--Execute [Usp_Rep_Emp_Loan_Request] 11
	--Select * From Emp_Loan_Advance where Req_Id=@Tran_Cd
	
--	inner join Emp_Loan_Advance_request r on (a.Req_Id=r.Id)
--	inner join EmployeeMast em on (a.EmployeeCode=em.EmployeeCode)
--	inner join EmployeeMast em1 on (a.AppByCode=em1.EmployeeCode)
--	inner join Ac_Mast am on (a.Bank_Id=am.Ac_Id)
--	inner join Emp_Pay_Head_Master c on (a.fld_Nm=c.Fld_Nm)
--	Left join Emp_Loan_Advance_Details ld on (a.Entry_Ty=ld.Entry_Ty and A.Tran_cd=ld.Tran_Cd)
--	order by Pay_Year,Pay_Month

--	,ApproveStat=(Case when Req_Stat='A' then 'Approved' else (Case when Req_Stat='H' then 'Hold' else 'Rejected' end) end)
--	,LoanType=(Case when Req_Stat='F' then 'Flat' else (Case when Req_Stat='D' then 'Demnising' else 'None' end) end)
--	,ApprovedBy=(Case when isNull(em1.pMailName,'')='' then em1.EmployeeName else em1.pMailName end)
--	,A.Loan_Amt,a.Int_Per,a.InstNo,a.Ded_Date,a.AppDate

End