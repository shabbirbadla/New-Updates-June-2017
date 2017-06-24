*:*****************************************************************************
*:        Program: MainCrystal.PRG
*:         System: Udyog Software
*:         Author: RND
*:  Last modified: 19/06/2007
*:			AIM  : Crystal Report Viewer
*:*****************************************************************************
&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
*LPARAMETERS tcCry1,tcBsql,tnSql,tcPrfix
*!*	LPARAMETERS tcCry1,tcBsql,tnSql,tcPrfix,tnSql1		
LPARAMETERS tcCry1,tcBsql,tnSql,tcPrfix,tnSql1,oPrinterObj		&& Changed by Sachin N. S. on 31/10/2012 for Bug-3775
*!*	tcCry1		 :	Crystal Report Name
*!*	tcBsql		 :	Sql-String
*!*	tnSql		 :	(1) Preview with Print Button [Default]
*!*					(2) Preview with out Print Button
*!*					(3) Print with Preview
*!*					(4) Print with PDF
*!*					(5) Print with XML
*!*					(6) Print with HTML
*!*	Usage		 :	Do Uecrviewer With "Acmast.rpt","SELECT AC_NAME,[GROUP] FROM Ac_Mast",1
*!*	tnSql 		 :	Old version is using tnSql, but added new parameter as tnSql1 just for forcing
*!*					the user to use latest uecrviewer.app
*:*****************************************************************************

****Versioning**** Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('REPORTVIEWER',_CurrVerVal,JUSTFNAME(SYS(16)))
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
IF VARTYPE(tnSql1) <> "N"
	tnSql1 = 0
ELSE
	tnSql = tnSql1
ENDIF
&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)

IF VARTYPE(VuMess) <> [C]
	_SCREEN.VISIBLE = .F.
	MESSAGEBOX("Internal Application Are Not Execute Out-Side ...",16)
	QUIT
ENDIF

IF VARTYPE(tcPrfix) <> "C"
	tcPrfix = ""
ENDIF

IF EMPTY(tcBsql)
	MESSAGEBOX('Please Pass Sql-String...',64,VuMess)
	RETURN .F.
ENDIF


************ Commented By Sachin N. S. on 29/09/2009 ************ Start
*!*	Do Case
*!*	Case Set("Date") == "AMERICAN"
*!*		currDt = Padl(Month(Date()),2,"0")+"-"+Padl(Day(Date()),2,"0")+"-"+Padl(Year(Date()),4,"0")
*!*		If !Between(Ctod(currDt),Ctod("05-01-2009"),Ctod("05-26-2010"))
*!*			Messagebox("Please collect latest report viewer",16,VuMess)
*!*			Return .F.
*!*		Endif
*!*	Otherwise
*!*		currDt = Padl(Day(Date()),2,"0")+"-"+Padl(Month(Date()),2,"0")+"-"+Padl(Year(Date()),4,"0")
*!*		If !Between(Ctod(currDt),Ctod("01-05-2009"),Ctod("26-05-2010"))
*!*			Messagebox("Please collect latest report viewer",16,VuMess)
*!*			Return .F.
*!*		Endif
*!*	Endcase
************ Commented By Sachin N. S. on 29/09/2009 ************ End

IF PCOUNT() < 2 OR TYPE("tcCry1") <> "C" OR TYPE("tcBsql") <> "C"
	MESSAGEBOX("Pass valid parameters...",0+64,VuMess)
	RETURN .F.
ENDIF

*!*	*!*	Set DataSession To _Screen.ActiveForm.DataSessionId

IF VARTYPE(oCrystalRuntimeApplication) <> 'O'
	PUBLIC oCrystalRuntimeApplication
	oCrystalRuntimeApplication = CREATEOBJECT("CrystalRuntime.Application.10")
ENDIF

LOCAL moCrvobj
moCrvobj = CREATEOBJECT("MainCrvclass")
moCrvobj.SplitStringParameters(IIF(VARTYPE(tcBsql)<>'C',"",tcBsql))
moCrvobj.cPrfix = tcPrfix
IF VARTYPE(_SCREEN.ACTIVEFORM.cPrintText) = "C"
	moCrvobj.cPrintText = IIF(!EMPTY(_SCREEN.ACTIVEFORM.cPrintText),_SCREEN.ACTIVEFORM.cPrintText,"")
ENDIF
tnSql = IIF(VARTYPE(tnSql)<>'N',1,tnSql)

moCrvobj.oPrinterObj = oPrinterObj		&& Added by Sachin N. S. on 31/10/2012 for Bug-3775

&& After export activex is changing the default directory so we are again set the default directory [Raghu251009]
lcOlddir = "'"+ALLTRIM(aPath)+ALLTRIM(Company.FolderName)+"\'"
IF !EMPTY(lcOlddir)
	SET DEFAULT TO &lcOlddir
ENDIF
&& After export activex is changing the default directory so we are again set the default directory [Raghu251009]

*!*	Do Form frmcrystalreport.scx With tcCry1,moCrvobj,tnSql,_Screen.ActiveForm.DataSessionId
DO FORM frmcrystalreport.scx WITH tcCry1,moCrvobj,tnSql

DEFINE CLASS MainCrvclass AS CUSTOM
	TceSql1 = .F.
	tceSql2 = .F.
	tceSql3 = .F.
	tceSql4 = .F.
	tceSql5 = .F.
	tcUnreg = ""
	cPrintText = ""
	cPrfix = ""
	nDatasID = 0
	oPrinterObj = ''		&& Added by Sachin N. S. on 31/10/2012 for Bug-3775

	FUNCTION SplitStringParameters
	LPARAMETERS tcSql
	LOCAL lnSql,i,x
	IF ! EMPTY(tcSql)
		xLen = LEN(ALLTRIM(tcSql))
		tcSql = IIF(RIGHT(ALLTRIM(tcSql),1)=':',LEFT(ALLT(tcSql),xLen-1),ALLTRIM(tcSql))
*!*			tcSql = "<<"+STRTRAN(tcSql,":",">><<")+">>" &&Rup 30/01/2010 Changed for L2S-55 : in Account Name was giving problem. All Related project need to be changed where ":" is used to generate string.
		tcSql = "<<"+tcSql+">>"
		lnSql = OCCUR("<<",tcSql)
		FOR i=1 TO lnSql STEP 1
			x = 'This.tceSql'+ALLT(STR((i)))
			&x = STREXTRACT(tcSql,"<<",">>",i)
			IF i = 5
				EXIT
			ENDIF
		ENDFOR
	ENDIF
	THIS.funGer()
	THIS.SetPdf_Path()
	THIS.nDatasID = _SCREEN.ACTIVEFORM.DATASESSIONID
	ENDFUNC

	FUNCTION funGer
	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
*!*		IF ! PEMSTATUS(Company,"Regd",5)
*!*			THIS.tcUnreg = []
*!*		ELSE
*!*			IF UPPER(ALLTRIM(Company.Regd)) = "DONE"
*!*				IF INLIST(UPPER(ALLTRIM(r_srvtype)),'PREMIUM','NORMAL')
*!*					THIS.tcUnreg = []
*!*				ELSE
*!*					THIS.tcUnreg = Company.Unregdmsg
*!*				ENDIF
*!*			ELSE
*!*				THIS.tcUnreg = Company.Unregdmsg
*!*			ENDIF
*!*		ENDIF
	THIS.tcUnreg = 'D E M O  -  C O M P A N Y'
	THIS.tcUnreg = GlobalObj.getPropertyval("unReg_Msg")
	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
	ENDFUNC

	FUNCTION SetPdf_Path
	IF ! PEMSTATUS(Coadditional,"PDF_Path",5)
		ADDPROPERTY(Coadditional,"PDF_Path",FULLPATH(CURDIR()))
	ELSE
		IF EMPTY(Coadditional.PDF_Path) OR ISNULL(Coadditional.PDF_Path)
			Coadditional.PDF_Path = FULLPATH(CURDIR())
		ELSE
			Coadditional.PDF_Path = ALLTRIM(Coadditional.PDF_Path)
			IF SUBSTR(Coadditional.PDF_Path,LEN(Coadditional.PDF_Path),1) <> "\"
				Coadditional.PDF_Path = Coadditional.PDF_Path+"\"
			ENDIF
		ENDIF
	ENDIF
	ENDFUNC

ENDDEFINE
