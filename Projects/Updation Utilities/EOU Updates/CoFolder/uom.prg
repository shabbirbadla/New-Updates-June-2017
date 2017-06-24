

*!*	*!*	*!*	Comment By Raghu [Start]
*!*	*!*	mAlias = ALIAS()
*!*	*!*	item1=Main_vw.item
*!*	*!*	SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
*!*	*!*	nHandle=0
*!*	*!*	sq1="SELECT * FROM it_mast WHERE item=?item1"
*!*	*!*	nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
*!*	*!*		"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
*!*	*!*	nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
*!*	*!*	IF nRetval<0
*!*	*!*		RETURN .F.
*!*	*!*	ENDIF
*!*	*!*	UOM=TmpEt_VW.UOM
*!*	*!*	WAIT WINDOW UOM Nowait 
*!*	*!*	*!*	Comment By Raghu [End]

