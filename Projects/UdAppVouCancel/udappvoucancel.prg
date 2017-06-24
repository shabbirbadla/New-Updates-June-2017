Parameters vBEFORE
Set DataSession To _Screen.ActiveForm.DataSessionId
&&-->Ipop(Rup)
If Used('projectitref_vw')
	Use In projectitref_vw
Endif
&&<--Ipop(Rup)
If Used('Gen_SrNo_Vw')
	Use In Gen_SrNo_Vw
Endif
*Birendra : 22 mar 2011 for Order Amendment
IF 'trnamend' $ vChkprod
	DO VouCancel IN MainPrg
ENDIF 

*Birendra: for Bug-661 on 10/12/2011:Start:
If Used('_uploadcursor')
	Use In _uploadcursor
Endif
*Birendra: for Bug-661 on 10/12/2011:End:
** 	Bug-5885--->
IF USED("PayRoll_vw")
	USE IN PayRoll_vw
Endif
**<---Bug-5885

&& Added by Shrikant S. on 27/06/2014  for Bug-23280		&& Start
If Used("BatchTbl_Vw")
	Use In BatchTbl_Vw
Endif
&& Added by Shrikant S. on 27/06/2014  for Bug-23280		&& End

IF USED("PayTerms_vw")
	USE IN PayTerms_vw
ENDIF

IF USED("PayTermsdet_vw")
	USE IN PayTermsdet_vw
ENDIF