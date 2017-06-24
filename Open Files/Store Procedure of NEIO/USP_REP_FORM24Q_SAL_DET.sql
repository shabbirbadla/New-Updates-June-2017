If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_FORM24Q_SAL_DET')
Begin
	Drop Procedure USP_REP_FORM24Q_SAL_DET
End
GO
Create Procedure USP_REP_FORM24Q_SAL_DET
@Sdate SmallDateTime,@Edate SmallDateTime
as
SELECT b.PAN,b.EmployeeName,DeducteeType=Case when b.Sex='F' then 'W' Else 'G' End
,EStartDt=Case when b.Doj > @sdate then b.Doj else @Sdate  End
,EEndDt=Case when b.DoL=0 then @edate else (Case when b.DoL < @edate then b.DoL Else @edate End) End
,NetPayment=Sum(a.NetPayment),NetPaymentPrev=Convert(Numeric(15,2),0),TotPayment=Sum(a.NetPayment)
,EnterAmt=Sum(othDedAmt),pTaxAmt=Sum(pTaxAmt),Tot_deduc=Sum(othDedAmt)+Sum(pTaxAmt),TotChgIncome=Sum(a.NetPayment)-Sum(othDedAmt)-Sum(pTaxAmt)
,LossFromProp=Convert(Numeric(15,2),0),GrossInc=Sum(a.NetPayment)-Sum(othDedAmt)-Sum(pTaxAmt)
,u_s_80C_80CCD=Convert(Numeric(15,2),0),u_s_80CCG=Convert(Numeric(15,2),0),u_s_VIA=Convert(Numeric(15,2),0),Tot_deducVIA=Convert(Numeric(15,2),0)
,TotTaxableInc=Sum(a.NetPayment)-Sum(othDedAmt)-Sum(pTaxAmt)-Convert(Numeric(15,2),0)
,IncomeTax=Convert(Numeric(15,2),0),Surcharge=Convert(Numeric(15,2),0),EduCess=Convert(Numeric(15,2),0),TaxRelief=Convert(Numeric(15,2),0)
,NetTax=Convert(Numeric(15,2),0),TDSCurrentE=Convert(Numeric(15,2),0),TDSPrevE=Convert(Numeric(15,2),0),TotalTDS=Convert(Numeric(15,2),0)
,TaxShortFall=Convert(Numeric(15,2),0),AtHigherRate=''
FROM Emp_Monthly_Payroll a
Inner Join EmployeeMast b On (a.EmployeeCode=b.EmployeeCode)
Where a.MnthLastDt between @Sdate and @Edate
Group By b.PAN,b.EmployeeName,b.Sex, b.Doj ,b.DOL  
order by b.EmployeeName

