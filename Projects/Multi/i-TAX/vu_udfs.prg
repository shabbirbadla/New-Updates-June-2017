PROC tmp_crea
*************
PARA _dbf
ctr = 1
DO WHIL .T.
	r_temp = SYS(3) + '.' + _dbf
	ctr = ctr + 1
	IF FILE(dix_temp)
		LOOP
	ENDIF
	EXIT
ENDDO
RETU r_temp

PROC enc
********
PARA mcheck
d=1
F=LEN(mcheck)
REPL=""
rep=0
DO WHIL F > 0
	r=SUBS(mcheck,d,1)
	CHANGE = ASC(r)+rep
	IF CHANGE>255
		WAIT WIND STR(CHANGE)
	ENDI
	two = CHR(CHANGE)
	REPL=REPL+two
	d=d+01
	rep=rep+1
	F=F-1
ENDD
RETU REPL

PROC dec
********
PARA mcheck
d=1
F=LEN(mcheck)
REPL=""
rep=0
DO WHIL F > 0
	r=SUBS(mcheck,d,1)
	CHANGE = ASC(r)-rep
	IF CHANGE>0
		two = CHR(CHANGE)
	ENDI
	REPL=REPL+two
	d=d+01
	F=F-1
	rep=rep+1
ENDD
RETU REPL

PROC chkprod
************
BUFFER=[]
IF !EMPTY(ALLT(company.passroute))
	FOR x = 1 TO LEN(ALLT(company.passroute))
		BUFFER = BUFFER + CHR(ASC(SUBSTR(company.passroute,x,1))/2)
	NEXT x
ENDIF
vchkprod=BUFFER
RETUR

PROCEDURE modalmenu
*****************
LOCAL oldpanel
oldpanel = statdesktop.panels(2).TEXT
statdesktop.panels(2).TEXT = 'Generating Menus....'
FOR i = 1 TO _SCREEN.FORMCOUNT
	IF LEFT(UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)),5) = 'UDYOG'
		DO gware.mpr WITH _SCREEN.FORMS[i],.T.
		EXIT
	ENDIF
NEXT i
statdesktop.panels(2).TEXT = oldpanel
RETURN

FUNCTION  busymsg
*****************
LPARAMETERS pmsg,pbusyicon,pbusyform,poldpanel
IF TYPE('pMsg') = 'L' AND TYPE('pBusyIcon') = 'L' AND TYPE('pBusyform') = 'L'
	IF pmsg = .F. AND pbusyicon = .F. AND pbusyform = .F.
		IF TYPE('pOldpanel') = 'U' OR TYPE('pOldpanel') = 'L'
			poldpanel = ''
		ENDIF
		_SCREEN.ACTIVEFORM.MOUSEPOINTER = 0
		IF !EMPTY(poldpanel)
			statdesktop.MESSAGE	= poldpanel
		ELSE
			statdesktop.MESSAGE	= ''
		ENDIF
		FOR i=1 TO _SCREEN.FORMCOUNT
			IF ALLTRIM(UPPER(_SCREEN.FORMS(i).NAME)) = 'THINKPROCESS'
				_SCREEN.FORMS(i).cexit()
				EXIT
			ENDIF
		ENDFOR
	ENDIF
ELSE
	oldpanel = statdesktop.MESSAGE
	oldmousepoint = _SCREEN.ACTIVEFORM.MOUSEPOINTER
	_SCREEN.ACTIVEFORM.MOUSEPOINTER = 13
	statdesktop.MESSAGE = pmsg
	IF pbusyform = .T.
		DO FORM thinkprocess WITH pmsg
	ENDIF
ENDIF
RETURN

PROCEDURE onencrypt
*****************
LPARA lcvariable
lcreturn = ""
FOR i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+CHR(ASC(SUBSTR(lcvariable,i,1))+ASC(SUBSTR(lcvariable,i,1)))
ENDFOR
RETURN lcreturn

PROCEDURE ondecrypt
*****************
LPARA lcvariable
lcreturn = ""
FOR i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+CHR(ASC(SUBSTR(lcvariable,i,1))/2)
ENDFOR
RETURN lcreturn

FUNCTION CheckRegDll
*****************
PARAMETERS lOle
LOCAL RegFind
RegOle = .T.
oldErrProc = ON('error')
ON ERROR DO DLLErrorProc IN vu_udfs
DECLARE LONG DllRegisterServer IN (lOle) ALIAS chkDll
CLEAR DLLS chkDll
IF TYPE('oldErrProc') = 'C'
	IF !EMPTY(oldErrProc)
		ON ERROR &oldErrProc
	ELSE
		ON ERROR
	ENDIF
ELSE
	ON ERROR
ENDIF
RETURN RegOle


PROCEDURE DLLErrorProc
*****************
lerrorno =ERROR()
RegOle = .T.
DO CASE
CASE lerrorno = 1753
	RegOle = .F.
ENDCASE
RETURN RegOle

PROCEDURE onshutdown
*****************
ON SHUTDOWN
CLEAR EVENTS
QUIT
RETURN

PROCEDURE pctrlf4
*****************
FOR i = 1 TO _SCREEN.FORMCOUNT
	IF ALLT(UPPER(_SCREEN.FORMS[i].BASECLASS)) != "TOOLBAR" AND LEFT(UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)),5) != 'UDYOG';
			AND UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)) != 'FRMLOGINUSERS' AND UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)) != 'FRMMSGWINDOW'
		statdesktop.MESSAGE = [Busy Mode....]
		=beep(600,200)
		statdesktop.MESSAGE = [User :]+musername
		RETURN .F.
	ENDIF
NEXT i
_SCREEN.ACTIVEFORM.procre(.T.)
RETURN

PROCEDURE pctrll
*****************
ON KEY LABEL ALT+l
FOR i = 1 TO _SCREEN.FORMCOUNT
	IF ALLT(UPPER(_SCREEN.FORMS[i].BASECLASS)) != "TOOLBAR" AND LEFT(UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)),5) != 'UDYOG';
			AND UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)) != 'FRMLOGINUSERS' AND UPPER(ALLTRIM(_SCREEN.FORMS[i].NAME)) != 'FRMMSGWINDOW'
		statdesktop.MESSAGE = [Busy Mode....]
		=beep(600,200)
		statdesktop.MESSAGE = [User :]+musername
		RETURN .F.
	ENDIF
NEXT i
_SCREEN.ACTIVEFORM.procre(.F.)
RETURN

PROCEDURE decoder
*****************
PARAMETERS thispass,passflag
STORE "" TO finecode,mycoder
FOR i = 1 TO LEN(thispass)
	IF !passflag
		mycoder = CHR(ASC(SUBSTR(thispass,i,1))-4) &&+7-11)
	ELSE
		mycoder = CHR(ASC(SUBSTR(thispass,i,1))+4) &&-7+11)
	ENDIF (!passflag)
	finecode = finecode+mycoder
NEXT (i)
RETURN finecode

