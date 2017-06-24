PARAM vDataSessionId,ffld_nm,strsql,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap
Set DataSession To vDataSessionId
Local sqlconobj
LOCAL EXPARA
EXPARA=' '
nHandle=0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
_Malias 	= Alias()
_mRecNo 	= Recno()

pFileNm='_selectpop'
nRetval = SqlConObj.DataConn([EXE],Company.DbName,strsql,[_selectpop],"nHandle",vDataSessionId ,.f.)
If nRetval > 0 And Used("_selectpop")
	mName = uegetpop(pFileNm,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap)
ENDIF
IF !EMPTY(mName)
	EXPARA=ALLTRIM(mName)
	REPLACE _rstatus.spl_condn WITH [']+EXPARA+[']
ENDIF
If !Empty(_Malias)
	Select &_Malias
ENDIF
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
ENDIF


