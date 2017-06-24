Param vDataSessionId,ffld_nm,strsql,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap
Set DataSession To vDataSessionId
Local sqlconobj
Local EXPARA
EXPARA=''
nHandle=0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
_Malias 	= Alias()
_mRecNo 	= Recno()

pFileNm='_selectpop'
nRetval = sqlconobj.DataConn([EXE],Company.DbName,strsql,[_selectpop],"nHandle",vDataSessionId ,.F.)
If nRetval > 0 And Used("_selectpop")
	mName = uegetpop(pFileNm,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap)
Endif

If Type("mname") = "C"
	EXPARA=Alltrim(mName)
Endif
Replace _rstatus.spl_condn With [']+EXPARA+[']

If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif


