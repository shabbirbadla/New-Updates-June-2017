Create Procedure USP_ENT_UPDATE_ACBAL
AS 
delete from AC_BAL

Select ac_Id,ac_name,Amount,amt_ty Into #Tmplacvw from Lac_vw

Insert Into AC_BAL (ac_Id,ac_name,cr,dr)
select ac_Id,ac_name,DrAmount=Sum(case when amt_ty='DR' Then Amount Else 0 End)
	,CrAmount=Sum(case when amt_ty='CR' Then Amount Else 0 End)
		From #Tmplacvw
			Group By ac_Id,ac_name,Amt_ty having Sum(case when amt_ty='DR' Then Amount Else -Amount End)<>0

drop table #Tmplacvw


