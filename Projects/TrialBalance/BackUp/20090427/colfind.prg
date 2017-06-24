*:*****************************************************************************
*:        Program: ColFind.Prg
*:         System: Udyog Software
*:         Author: UDAY
*:  Last modified: 15/12/2006
*:			AIM  : Column Level Find Class
*:		Remarks  : This Is The Class It Find Column
*:*****************************************************************************
LPARAMETERS ltablename AS STRING,lfield AS Variant,;
	lasfield AS STRING,lgrid AS OBJECT,lkeyfld AS STRING,GotoCol AS STRING

*!*	mYesNo = MESSAGEBOX("Debug",4+32,Vumess)
*!*	IF mYesNo = 6
*!*		SET STEP ON
*!*	ENDIF

LOCAL mvalue
STORE "" TO mvalue
lkeyfld = IIF(!EMPTY(lkeyfld)," WHERE "+lkeyfld,'')
mstr = "select distinct "+lfield+" as "+lasfield+" from "+ltablename+lkeyfld+" into cursor t1"

_TALLY = 0
&mstr
IF _TALLY # 0
	lasfield = IIF(EMPTY(lasfield),SUBSTR(lfield,AT(".",lfield)+1,LEN(lfield)),lasfield)
	mvalue=Uegetpop("t1","FIND ",lasfield)
	USE IN t1
ENDIF

IF ! EMPTY(mvalue)
	grdnm  = lgrid
	SELECT (ltablename)
	GO TOP
	FOR reccnt = RECNO() TO RECCOUNT()
		GOTO RECORD reccnt
		IF &lfield = mvalue
			IF ! EMPTY(GotoCol)
				grdnm.&GotoCol..SETFOCUS()
			ENDIF
			EXIT
		ENDIF
	ENDFOR
ENDIF
