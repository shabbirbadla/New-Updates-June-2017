*:*****************************************************************************
*:        Program: Main.PRG
*:         System: Udyog Software
*:         Author: Raghavendra
*:  Last modified: 19/09/2006
*:			AIM  : Search window
*:*****************************************************************************
*Birendra : Bug-7753 on 01/04/2013 :added more parameter for pTreeview:
LPARAM pFileNm, pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap,pTreeview
*LPARAM pFileNm, pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,pExclude,pInclude,capcol,pExclcap
*!*01. pFileNm		: Cursor Name
*!*02. pCaption		: Getpop Window Caption
*!*03. pField		: Search Field Name
*!*04. pReturn		: Return Values
*!*05. PSearchV		: Text Field Value
*!*06. pSplit		: Split
*!*07. pxTraField	: Extra field name
*!*08. PxTraCaption	: Extra field caption
*!*09. pSrchAny		: Search any where in Search in field
*!*10. pExclude		: Show all fields i.e. not In.
*!*11. pInclude		: Show Include filds only
*!*12. capcol		: Show Column caption
*!*13. pExclcap		: Exclude From Dispaly
*!*14. ptreeview	: Cursor for tree view(Three field - zkey,zparent,ztext)
*!*	[SUB3:English,SUB1:Kannada,SUB2:Hindi,NAME:Student Name]


****Versioning**** Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('GETPOP',_CurrVerVal,JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F.
	ENDIF
	IF _VerRetVal  = 'NO'
		Return .F.
	Endif
****Versioning****
LOCAL objgpop
SELECT (pFileNm)
capcol = IIF(VARTYPE(capcol)<>'L',capcol,"")+IIF(!EMPTY(capcol),IIF(RIGHT(ALLT(capcol),1)<>',',",",""),'')
objgpop = CREATEOBJECT("_adjgetpop")
objgpop.Runme(pExclude,pInclude,capcol,IIF(VARTYPE(pExclcap)<>'C','',pExclcap))
objgpop.nBACKCOLOR = _SCREEN.ACTIVEFORM.BACKCOLOR
LOCAL mval && added by satish pal for bug-12523 dated 08/05/2013
IF ! EMPTY(objgpop.ShowFlds)
*Birendra : Bug-7753 on 01/04/2013 :added more parameter for pTreeview:
	DO FORM frmsrch WITH pFileNm,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,objgpop,pTreeview TO mval 
*	DO FORM frmsrch WITH pFileNm,pCaption, pField,pReturn,PSearchV,pSplit,pxTraField,PxTraCaption,pSrchAny,objgpop TO mval
ENDIF
RETURN mval

*:*****************************************************************************
*:        Program: _Adjgetpop
*:         System: Udyog Software
*:         Author: Raghavendra
*:  Last modified: 06/02/2007
*:			AIM  : Exclude,Include,Field Caption Method
*:*****************************************************************************
DEFINE CLASS _adjgetpop AS CUSTOM
	ShowFlds = ""
	strfile = ''
	capcol = ""
	pExclcap = []
	seShowFlds = ""
	nBACKCOLOR = []
	DIMENSION caparry(1)

	PROCEDURE Runme
	LPARAMETERS pExclude AS STRING,pInclude AS STRING,capcol AS STRING,pExclcap AS STRING

	mtotflds = 0
	THIS.pExclcap = pExclcap
	DIMEN mflds(1)
	IF TYPE('pInclude') <> 'L' AND ! EMPTY(pInclude)
		mtotflds = THIS.getfields(pInclude,@mflds,1)
	ELSE
		IF TYPE('pExclude') <> 'L' AND ! EMPTY(pExclude)
			mtotflds = THIS.getfields(pExclude,@mflds,2)
		ELSE
			mtotflds = THIS.getfields("",@mflds,3)
		ENDIF
	ENDIF
	IF ! EMPTY(mtotflds)
		FOR _iud = 1 TO mtotflds STEP 1
			THIS.setstring(mflds(_iud))
		ENDFOR
		THIS.ShowFlds = THIS.Getstring()
	ENDIF
	THIS.capcol = IIF(TYPE("capcol")<>"L",ALLT(capcol),"")
	THIS.SetCaparray()
	THIS.SetseShowFlds()
	ENDPROC

	FUNCTION SetseShowFlds
	LOCAL lnExcap,i,lcExcap
	THIS.seShowFlds = "<<"+STRTRAN(ALLTRIM(THIS.ShowFlds),",",">><<")+">>"

	IF !EMPTY(THIS.pExclcap)
		THIS.pExclcap = ALLTRIM(THIS.pExclcap)+IIF(RIGHT(ALLTRIM(THIS.pExclcap),1)=',',"",",")
	ENDIF

	lnExcap = OCCUR(",",THIS.pExclcap)
	THIS.pExclcap = IIF(lnExcap <> 0,"<<"+STRTRAN(ALLTRIM(THIS.pExclcap),",",">><<")+">>","")

	FOR i=1 TO lnExcap STEP 1
		lcExcap = "<<"+STREXTRACT(THIS.pExclcap,"<<",">>",i)+">>"
		THIS.seShowFlds = STRTRAN(THIS.seShowFlds,lcExcap,"")
	ENDFOR
	ENDFUNC


	FUNCTION getfields
	LPARAMETERS pInclude AS STRING, pmarr AS STRING,ExorIn AS INTEGER
	LOCAL mctr, mpos, mprev,maflds
	DO CASE
	CASE ! EMPTY(pInclude) AND ExorIn = 1
		mctr=1
		STORE 0 TO mprev,mpos
		DO WHILE .T.
			mprev = mpos
			mpos=AT(",",pInclude,mctr)
			DIME pmarr(mctr)
			IF mpos=0
				pmarr(mctr) = SUBSTR(pInclude,mprev+1)
			ELSE
				pmarr(mctr) = SUBSTR(pInclude,mprev+1,(mpos-1)-mprev)
			ENDIF
			mctr=mctr+1
			IF mpos =0
				EXIT
			ENDIF
		ENDDO
	CASE ! EMPTY(pInclude) AND ExorIn = 2
		DIME maflds(1)
		mpos = 1
		mctr=AFIELDS(maflds)
		DIME pmarr(mpos)
		FOR mI = 1 TO mctr STEP 1
			IF ATC(maflds(mI,1),pInclude) = 0
				DIME pmarr(mpos)
				pmarr(mpos) = maflds(mI,1)
				mpos = mpos + 1
			ENDIF
		ENDFOR
	OTHERWISE
		DIME maflds(1)
		mctr=AFIELDS(maflds)
		DIME pmarr(mctr)
		FOR mI = 1 TO mctr STEP 1
			pmarr(mI) = maflds(mI,1)
		ENDFOR
	ENDCASE
	RETURN ALEN(pmarr)

	FUNCTION setstring
	LPARAMETERS sStr AS STRING
	IF EMPTY(THIS.strfile)
		THIS.strfile = sStr
	ELSE
		THIS.strfile = THIS.strfile+","+sStr
	ENDIF

	FUNCTION Getstring
	PRIVATE RtnStr AS STRING
	IF ! EMPTY(THIS.strfile)
		RtnStr = THIS.strfile
		THIS.strfile = ''
		RETURN RtnStr
	ENDIF

	FUNCTION SetCaparray
	PRIVATE nFromfld,x,Y,FldName
	x = THIS.capcol
	totcap = IIF(OCCURS(",",THIS.capcol)<>0,OCCURS(",",THIS.capcol),1)
	FOR nFromfld = 1 TO totcap STEP 1
		FldName = SUBSTR(x,1,ATC(":",x)-1)
		Y = SUBSTR(x,ATC(FldName,x),ATC(",",x))
		x = STRTRAN(x,Y,"")
		DIMENSION THIS.caparry(nFromfld)
		THIS.caparry(nFromfld) = Y
	ENDFOR
	ENDFUNC
ENDDEFINE
