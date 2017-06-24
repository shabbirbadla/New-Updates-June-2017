*----------------*
PROCEDURE dataConn
*----------------*
lPARAMETERS _sqltodo,_sqldbname,_sqlcond,_sqltbl,sqlTransaction,sqlMessage
*!*	IF MESSAGEBOX('Step On',4+32+256) = 6
*!*		SET STEP ON 
*!*	ENDIF 
*sql_con = dataConn("EXE",company.dbname,"select * from dcmast","sql_tmltbl",.f.) in chkCOnn
local malias,mMsg
if !empty(_sqltbl)
	if used(_sqltbl)
		use in (_sqltbl)
	endif
endif
malias = alias()
mMsg = iif(Type('sqlMessage') <> 'U',sqlMessage,"")
_sqlret = 0
Do While .T.
	mexit = .t.
	IF TYPE('statdesktop') = 'O'
		oldPanel = statdesktop.panels(2).text 	
	ENDIF 	
	TRY
		Do Case
		Case _sqltodo = 'CHK'
			IF TYPE('statdesktop') = 'O'		
				statdesktop.panels(2).text = "Connection checking.......!"
			ENDIF 	
			_sqlret = sqlgetprop(chqcon,'ConnectTimeOut')
		Case _sqltodo = 'EXE'
			IF TYPE('statdesktop') = 'O'		
				statdesktop.panels(2).text = "Checking current database..."
			ENDIF 	
			_sqlret = SQLEXEC(chqCon,"Select db_name() as dbName","_dbName")
			IF TYPE('statdesktop') = 'O'
				statdesktop.panels(2).text = oldPanel			
			ENDIF 	
			IF _sqlRet > 0
				IF UPPER(ALLTRIM(_dbName.dbName)) <> UPPER(ALLTRIM(_sqlDbName))
					IF TYPE('statdesktop') = 'O'				
						statdesktop.panels(2).text = "Connection disconecting.....!"
					ENDIF 	
					_sqlRet=SQLDISCONNECT(chqCon)
					IF _sqlRet > 0
						IF TYPE('statdesktop') = 'O'					
							statdesktop.panels(2).text = "Connecting to server......!"
						ENDIF 	
						do conn with mvu_backend,mvu_server,mvu_user,mvu_pass,_sqldbname					
						SELECT _dbName
						use
					ELSE
						=MESSAGEBOX("Error Disconnecting "+ALLTRIM(_dbName.dbName),64,"Visual Udyog")
						SELECT _dbName
						USE
						IF TYPE('statdesktop') = 'O'						
							statdesktop.panels(2).text = oldPanel									
						ENDIF
						RETURN -1
					endif
				ENDIF
			ELSE
				=MESSAGEBOX("Error connecting to Database",64,"Visual Udyog")
				SELECT _dbName
				USE
				IF TYPE('statdesktop') = 'O'				
					statdesktop.panels(2).text = oldPanel							
				ENDIF 	
				RETURN -1
			endif
			IF sqlTransaction		&& if .t. then Begin Transaction 
				IF TYPE('statdesktop') = 'O'			
					statdesktop.panels(2).text = "Transaction setting...."
				ENDIF 	
				_sqlret=Sqlsetprop(chqcon,'transactions',2)
				IF _sqlret<=0
					mexit=.f.
					exit
				endif
			endif
			if !empty(mAlias)
				sele &malias
			ENDIF
			IF TYPE('statdesktop') = 'O'			
				statdesktop.panels(2).text = "Executing sql string...."
			ENDIF 	
			_sqlret = sqlexec(chqcon,_sqlcond,_sqltbl)
		Endcase
	CATCH TO errnum
		IF errnum.Errorno = 1466
			mexit = .f.
			IF TYPE('statdesktop') = 'O'
				oldPanel = statdesktop.panels(2).text 
				statdesktop.panels(2).text = "Error occurs, Re-connecting to server"
			ENDIF 	
			do conn with mvu_backend,mvu_server,mvu_user,mvu_pass,_sqldbname
			IF TYPE('statdesktop') = 'O'			
				statdesktop.panels(2).text = oldPanel																								
			ENDIF 	
		else
			mMsg = mMsg + iif(!empty(mMsg), " chr(13) ","") + alltr(Proper(message()))
			=messageBox(mMsg,64,"Visual Udyog")
		Endif
	ENDTRY
	if _sqlret < 0
		do showError with mMsg
		mExit = .t.
	endif	
	IF mExit = .t.
		exit
	Endif
ENDDO
IF TYPE('statdesktop') = 'O'
	statdesktop.panels(2).text = oldPanel			
ENDIF 	
RETURN _sqlret

*-------------*
proc genInsert()
*-------------*
lParam pInsert, pexclude, pKeyFields,pFileNm,pPlatform,pInclude
*pInsert --> Destination FileName
*pExclude -> field Names to be excluded
*pKeyfields  field names to be excluded
*pFileNm	 Temp. file Name
*pPlatform	  Vfp /SQL
*----------------------------*
*E.g. mSqlStr = genInsert("r_status","","'DESC','GROUP'","_rstatus",thisform.platform)
*----

local mRunflds, mValues,mTotflds
sele &pFileNm
dimen mflds(1)
Mtotflds=afields(mflds)
mrunflds=""
mvalues=""
if type('pInclude') <> 'L' and !empty(pInclude)
	mTotFlds = getFields(pInclude,@mflds)
endif
pexclude=UPPER(ALLTRIM(pexclude))
pkeyfields = UPPER(ALLTRIM(pkeyfields))

*---Get Fields & values
for i = 1 to mTotflds
	if !empty(pInclude)
		mrunflds = mrunflds+mflds(i,1)+ iif(mtotflds=1 or i=mtotflds,"",",")
		mValues  = mValues + "?" + pFileNm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")		
		loop
	endif

	if !empty(pExclude)
		if inlist(UPPER(ALLTRIM(mflds(i,1))),&pexclude)
			IF i=mtotflds
				mrunflds=SUBSTR(mrunflds,1,LEN(ALLTRIM(mrunflds))-1)
				mvalues =SUBSTR(mvalues,1,LEN(ALLTRIM(mvalues))-1)
			ENDIF
			loop
		endif
	endif
	IF empty(pKeyfields)
		mRunflds = mRunflds+mflds(i,1)+ iif(mtotflds=1 or i=mtotflds,"",",")		
		mValues  = mValues + "?" + pFileNm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")
		LOOP
	ENDIF
	if inlist(upper(alltr(mflds(i,1))),&pKeyFields)
		do case
			case pPlatform="0"
				mRunflds = mRunflds+ pfileNm+"."+mflds(i,1)+iif(Mtotflds=1 or i=mtotflds,"",",")
			case pPlatform="1"
				mRunflds = mRunflds+"["+mflds(i,1)+"]"+ iif(mtotflds=1 or i=mtotflds,"",",")
		endcase
	else
		mRunflds = mRunflds+mflds(i,1)+ iif(mtotflds=1 or i=mtotflds,"",",")		
	endif
	mValues  = mValues + "?" + pFileNm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")
next
mInsert = "insert into " + alltr(pInsert) + "("+mrunflds+") values ("+mvalues +")"
return (mInsert)

*------------*
proc genUpdate
*------------*
lparam pUpdate, pExclude, pKeyFields,pFileNm,pPlatform,pCond,pInclude
local mSqlStr, mflds, mTotFlds,mrunflds
mSqlStr=""
sele &pFileNm
dimen mflds(1)
mTotFlds=afields(mflds)
mrunflds= ""
pKeyfields=UPPER(ALLTRIM(pKeyFields))
SET STEP ON 
pexclude=UPPER(ALLTRIM(pexclude))
if type('pInclude') <> 'L' and !empty(pInclude)
	mTotFlds = getFields(pInclude,@mflds)
endif
*for i = 1 to iif( !empty(pinclude), mTotflds,mTotflds-1)
for i = 1 to mTotflds
	if !empty(pInclude)
		mrunflds = mrunflds+mflds(i)+"="+"?"+mflds(i) + ;
					 iif(mtotflds=1 or i=mtotflds,"",",")
		loop
	endif
	if !empty(pExclude)
		if inlist(UPPER(ALLTRIM(mflds(i,1))),&pexclude)
			IF i=mtotflds
				mrunflds=SUBSTR(mrunflds,1,LEN(ALLTRIM(mrunflds))-1)
			ENDIF
			loop
		endif
	endif
	IF EMPTY(pKeyfields)
		mrunflds = mrunflds+mflds(i,1)+"="+"?"+field(i)+ iif(mtotflds=1 or i=mtotflds,"",",")
		LOOP
	ENDIF
	if inlist(upper(alltr(mflds(i,1))),&pKeyFields)
		do case
			case pPlatform="0"				&& vfp
				mrunflds = mrunflds+pFileNm+".["+alltr(mflds(i,1))+"]="+"?"+pFileNm + "." + field(i)+ iif(mtotflds=1 or i=mtotflds,"",",")
			case pPlatform="1"				&& Sql 2k
				mrunflds = mrunflds+"["+alltr(mflds(i,1))+"]="+"?["+field(i)+"]"+iif(mtotflds=1 or i=mtotflds,"",",")
		ENDCASE
	else
		mrunflds = mrunflds+mflds(i,1)+"="+"?"+field(i)+ iif(mtotflds=1 or i=mtotflds,"",",")
	ENDIF
next
mSqlStr = "Update "+alltr(pUpdate)+" set "+mrunflds+" where "+pCond
return (mSqlStr)
*----------------*
Procedure genDelete
*----------------*
lParam pDelFile,pCond
local mSqlStr
mSqlStr = "delete from "+alltr(pDelFile)+" where " +pCond
return (mSqlStr)
*---------------*
procedure setIcon
*---------------*
lparam pObject
pObject.Picture = aPath + "\Bmp\Loc.Bmp"
return
*------------*
proc showError
*------------*
lParam pMsg
local mRet,mErrMsg

mErrMsg = message()
mRet = sqlExec(chqCon,"select @@error as Num", "_Error")
sele _Error
do case
*!*		case _Error.Num = 547
*!*			=messageBox("Constraint violation Error" + CHR(13) + CHR(13) + Alltr(mErrMsg),64,"Visual Udyog")
	otherwise
		if !empty(pMsg)
			pMsg = pMsg + iif(!empty(pMsg), chr(13) + chr(13),"") + alltr(mErrMsg)
		else
			pMsg = alltr(mErrMsg)
		endif
		=messageBox(pMsg,64,"Visual Udyog")	
endcase
use
return
*------------*
proc getFields
*------------*
lparam pInclude, pmArr
local mctr, mPos, mPrev
mctr=1
store 0 to mPrev,mPos
do while .t.
	mPrev = mPos
	mPos=at(",",pInclude,mctr)
	dime pmArr(mCtr)
	if mPos=0
		pmArr(mCtr) = substr(pInclude,mPrev+1)
	else
		pmArr(mCtr) = substr(pInclude,mPrev+1,(mPos-1)-mPrev)
	endif
	mCtr=mCtr+1
	if mPos =0
		exit
	endif
enddo
return alen(pmArr)