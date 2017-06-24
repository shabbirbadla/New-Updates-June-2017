LPARAMETERS _prinpara
IF _prinpara = 'AFTER'
	_mins = 1
	For _mins = 1 To _mine
		mdoreponm = ALLTRIM(IIF(!Empty(aprintarr(_mins,6)),EVALUATE(aprintarr(_mins,6)),STRTRAN(UPPER(mdorepo),'.RPT','')))+'.PDF'
		_mailatt   = ALLTRIM(coadditional.pdf_path)+'\'+mdoreponm
		_mailsub =  allt(Main_vw.Party_nm) 
		_mailbody = "Test Mail"+chr(13)+chr(13)+;
		             "Thanks,"+chr(13)+chr(13)+mUsername
		Do UeMailing With "vasant@udyogsoftware.com","vasant@udyogsoftware.com",_mailsub,_mailbody,_mailatt
	Endfor
ENDIF
