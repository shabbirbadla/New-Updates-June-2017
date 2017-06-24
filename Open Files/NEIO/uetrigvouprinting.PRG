LPARAMETERS _prinpara
_actfrm = _screen.ActiveForm
_actprintflag = 0
IF TYPE('printflag') = 'N'
	_actprintflag = printflag
Endif
IF _prinpara = 'AFTER' AND _actprintflag = 4
	nHandle = 0
	_mailto = ''
	_actfrmcon = _actfrm.SqlConObj.DataConn([EXE],Company.DbName,;
		"Select email from ac_mast where ac_id = ?Main_vw.Ac_id",[TmpPrt_Vw],"nHandle",_actfrm.DataSessionId,.F.)
	IF _actfrmcon > 0 AND USED('TmpPrt_Vw')	
		SELECT TmpPrt_Vw	
		IF RECCOUNT() >= 1
			_mailto = TmpPrt_Vw.email
		ENDIF
	Endif	
	_mins = 1
	For _mins = 1 To _mine
		mdoreponm = ALLTRIM(IIF(!Empty(aprintarr(_mins,6)),EVALUATE(aprintarr(_mins,6)),STRTRAN(UPPER(mdorepo),'.RPT','')))+'.PDF'
		_mailatt   = ALLTRIM(coadditional.pdf_path)+'\'+mdoreponm
		_mailsub =  mdoreponm
		_mailbody = ''
		*_mailbody = "Test Mail"+chr(13)+chr(13)+;
		             "Thanks,"+chr(13)+chr(13)+mUsername
		Do UeMailing With _mailto,"",_mailsub,_mailbody,_mailatt
	Endfor
ENDIF
