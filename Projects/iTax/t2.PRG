aa='uday'
bb='vasant'
a="EXECUTE USP_LCS_G ?aa,?bb;entry_ty"
LOCAL tSqlquery,mExecQuery
tSqlQuery = UPPER(alltr(a))
FOR i = 1 TO LEN(tSqlQuery)
	IF SUBSTR(tSqlquery,i,1) = ' '
		mExecQuery=SUBSTR(tSqlquery,i,AT(";",tSqlquery)-i)
		exit
	ENDIF
ENDFOR 
WAIT WINDOW mExecQuery

FOR i = 1 TO LEN(mExecquery)
	IF SUBSTR(mExecquery,i,1)='?'
		
	ENDIF 
ENDFOR 
*!*	mExecQuery = '"'+mExecQuery+'"'
*!*	mExecQuery = EVALUATE(mExecQuery)
*!*	WAIT WINDOW mExecQuery 
*!*	DECLARE INTEGER Beep IN kernel32 INTEGER pn_Freq,INTEGER pn_Duration
*!*	beep(200,100)
*!*	beep(100,80)
*!*	beep(700,50)
*!*	beep(100,100)
