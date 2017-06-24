Parameters vDataSessionId, vaddmode,veditmode,vpform,vTdsTcs &&Add vTdsTcs Rup 17/01/20111 TKT-5692
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011 
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('TDSPAYMENT',_CurrVerVal,JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F.
	ENDIF
	IF _VerRetVal  = 'NO'
		Return .F.
	Endif
****Versioning****

If !("GRIDFIND.VCX" $ Upper(Set("Classlib")))
	Set Classlib To gridfind.vcx Additive
Endif

DO FORM uefrm_tds_payment.scx WITH vDataSessionId, vaddmode,veditmode,vpform,vTdsTcs &&Add vTdsTcs Rup 17/01/20111 TKT-5692
