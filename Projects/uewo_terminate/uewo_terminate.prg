Para code1,mType,pRange
*!*		lError = .F.
*!*		nhandle     = 0
*!*		_etdatasessionid = _SCREEN.ACTIVEFORM.DATASESSIONID
*!*		SET DATASESSION TO _etdatasessionid
*!*		sqlconobj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
*!*	xStr="EXECUTE USP_ENT_WO_TERMINATE " 
*!*	*!*	'"+ALLTRIM(MAIN_VW.ENTRY_TY)+"',"+ALLTRIM(STR(MAIN_VW.TRAN_CD))+","+"'"+DTOS(MAIN_VW.DATE)+"'"
*!*		sql_con=sqlconobj.dataconn("EXE",company.DbName,xStr,"balitem_vw1","nHandle",_etdatasessionid,.F.)
*!*		IF sql_con =< 0
*!*			=MESSAGEBOX(MESSAGE(),0+16,Vumess)
*!*			THIS.lError = .T.
*!*			RETURN .F.
*!*		ELSE
*!*			SELECT balitem_vw1
*!*			COUNT FOR ! DELETED() TO m_Tot
*!*			IF m_Tot = 0
*!*				=MESSAGEBOX("No Records Found For Display...",0+32,Vumess)
*!*				THIS.lError = .T.
*!*				RETURN .F.
*!*			ENDIF
*!*		ENDIF
DO FORM uefrm_terminate_wo 