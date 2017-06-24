PARAM vDataSessionId,ffld_nm,strsql,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap
SET DATASESSION TO vDataSessionId
LOCAL sqlconobj
LOCAL EXPARA
EXPARA=' '
nHandle=0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
_Malias 	= ALIAS()
_mRecNo 	= RECNO()

pFileNm='_selectpop'
nRetval = sqlconobj.DataConn([EXE],Company.DbName,strsql,[_selectpop],"nHandle",vDataSessionId ,.F.)
IF nRetval > 0 AND USED("_selectpop")
	mName = uegetpop(pFileNm,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap)
ENDIF
IF !EMPTY(mName)
&&priyanka_24012012_start
	EXPARA="'"+ffld_nm+"=`"+ALLTRIM(mName)+"`'"
&&priyanka_24012012_end
	REPLACE _rstatus.spl_condn WITH EXPARA
ENDIF
IF !EMPTY(_Malias)
	SELECT &_Malias
ENDIF
IF BETW(_mRecNo,1,RECCOUNT())
	GO _mRecNo
ENDIF


