LPARAMETERS _RptGroupName,_RptDescription,_RptName,_eFileName
_screenactiveform = _screen.ActiveForm
_screenactiveform.panel.Panels(1).Text = 'Gathering Information....'

SET STEP ON 
_TrigFileName = ALLTRIM(_eFileName)+'Validates.Prg'
_TryError     = .f.
TRY
	SET PROCEDURE TO (_TrigFileName) addi &&vasant050411
CATCH TO oException
	IF oException.ErrorNo <> 1
		_TryError = .t.
		=Messagebox(ALLTRIM(oException.Message),0,vumess)
	ENDIF
ENDTRY
IF _TryError = .t.
	Return .F.
ENDIF

IF !USED('R_Status')
	=MESSAGEBOX('Unable to open Report Table',0,vumess)
	RETURN .f.
ENDIF

_ReportRecNo = 0
SELECT r_status
LOCATE FOR ALLTRIM(UPPER(group)) == ALLTRIM(UPPER(_RptGroupName)) AND ;
	ALLTRIM(UPPER(Desc)) == ALLTRIM(UPPER(_RptDescription)) AND ;
	ALLTRIM(UPPER(Rep_nm)) == ALLTRIM(UPPER(_RptName))
IF !FOUND()
	=MESSAGEBOX('Unable to find Report '+ALLTRIM(_RptDescription),0,vumess)
	RETURN .f.
ELSE
	_ReportRecNo = RECNO()
Endif

_screenactiveform.panel.Panels(1).Text = 'Gathering Information......'
DO case
CASE mProduct = 'VU8'	

	tbrvouchers = CreateObject('VudyogClass')
	_RepoObj =NEWOBJECT('ClVuReport','ClVuReport.VCX','vureportnew.app',r_status.group)
	IF TYPE('_RepoObj') <> 'O'
		=MESSAGEBOX('Unable to find Report '+ALLTRIM(_RptDescription),0,vumess)
		RETURN .f.
	ENDIF
	sdate = _reposdate
	edate = _repoedate

	SELECT r_status
	GO _ReportRecNo
	_RepoObj.FrxName = r_status.Rep_nm

	_screenactiveform.panel.Panels(1).Text = 'Gathering Information........'
	date_f     = " betw(date,sdate,edate) "
	lcstr_one  = ''
	lcstr_two  = ''
	lcstr_thrd = ''
	lcstr_one  = iif(!empty(lcstr_one),lcstr_one + " And " + date_f , date_f)
	lcstr_thrd = iif(!empty(lcstr_thrd),lcstr_thrd + " and " + ;
					allt(r_status.spl_condn),allt(r_status.spl_condn))
	lcstr_tot  = ''				
	
	_screenactiveform.panel.Panels(1).Text = 'Generating Report.....'
	_RepoMethod = '_RepoObj.'+ALLTRIM(r_status.rep_nm)
	&&vasant041110
	*&_RepoMethod
	_errmsgvalue = ''
	Try		
		IF r_status.IsMethod OR !EMPTY(r_status.MethodOf) 								
			&_RepoMethod
		ELSE
			lcstr_tot = allt(r_status.spl_condn)
			IF !empty(lcstr_tot)
				lcstr_tot =  " for " + lcstr_tot
			Endif					
		Endif	
	CATCH TO _errmsg
		_errmsgvalue = _errmsg.message
	Endtry	
	IF !Empty(_errmsgvalue)
		=MESSAGEBOX(_errmsgvalue,0,vumess)
		RETURN .f.
	ENDIF
	&&vasant041110
	IF !EMPTY(ALIAS())
		COUNT TO _reporeccount
		IF _reporeccount = 0
			=MESSAGEBOX('No Data Available for the Selected Period',0,vumess)
			RETURN .f.
		ENDIF
		GO Top
	Endif	

	SELECT r_status
	GO _ReportRecNo

	IF _mDirecteFiling = .f.
		If !'\_REPORTLISTENER.' $ Upper(Set('class'))
			SET CLASSLIB TO _REPORTLISTENER IN SYS(16,0) ADDITIVE
		Endif	
		oHTMListener = CREATEOBJECT('ClHTMLGenerator')
		IF TYPE('oHTMListener') <> 'O'
			=MESSAGEBOX('Unable to find HTML Generator',0,vumess)
			RETURN .f.
		ENDIF
		_frxvwname  = ALLTRIM(_RptName)+'_vw2.frx'
		_frtvwname  = ALLTRIM(_RptName)+'_vw2.frt'
		DO eFileCopy WITH _xmlrptname1,_frxvwname IN eFileUtil
		DO eFileCopy WITH _xmlrptname2,_frtvwname IN eFileUtil
		IF !FILE(_frxvwname)
			=MESSAGEBOX('Unable to Find Report file',0,vumess)
			RETURN .f.
		ENDIF

		_repogenoldengine = SET("EngineBehavior")
		SET ENGINEBEHAVIOR 70
		&&vasant041110
		_errmsgvalue = ''
		Try			
			&&vasant041110	
			REPORT FORM (_frxvwname) OBJECT oHTMListener &lcstr_tot
			&&vasant041110
		CATCH TO _errmsg
			_errmsgvalue = _errmsg.message
		Endtry	
		&&vasant041110
		SET ENGINEBEHAVIOR _repogenoldengine
		&&vasant041110
		IF !Empty(_errmsgvalue)
			=MESSAGEBOX(_errmsgvalue,0,vumess)
			RETURN .f.
		ENDIF
		&&vasant041110

		ERASE (_frxvwname)
		ERASE (_frtvwname)

		&&vasant110710
		IF USED('lst_exc')
			USE IN lst_exc
		Endif		
		IF USED('RG23D')
			USE IN RG23D
		Endif		
		&&vasant110710
	ENDIF
		
	lcstr_one  = ''
	lcstr_two  = ''
	lcstr_thrd = ''
	lcstr_one  = iif(!empty(lcstr_one),lcstr_one + " And " + date_f , date_f)
	lcstr_thrd = iif(!empty(lcstr_thrd),lcstr_thrd + " and " + ;
					allt(r_status.spl_condn),allt(r_status.spl_condn))
	lcstr_tot  = ''								
	IF r_status.IsMethod OR !EMPTY(r_status.MethodOf) 				
		&_RepoMethod
	ELSE
		lcstr_tot = allt(r_status.spl_condn)
		IF !empty(lcstr_tot)
			lcstr_tot =  " for " + lcstr_tot
		Endif					
	Endif	

CASE mProduct = 'VU9'	
	IF !File('uecrviewer.app')
		=MESSAGEBOX('Report Viewer File not Found',0,vumess)
		RETURN .f.
	ENDIF
	sdate = _reposdate
	edate = _repoedate

	SELECT r_status
	GO _ReportRecNo
	Scatter Name _tmpvar Memo
	If TYPE('_tmpvar') <> 'O'
		=Messagebox('Unable to find Report Details',64,vumess)
		Return .F.
	ENDIF
	=AddProperty(_tmpvar,"SDate",_reposdate)
	=AddProperty(_tmpvar,"EDate",_repoedate)
	
	_screenactiveform.panel.Panels(1).Text = 'Gathering Information........'
	SET DATE American
	*_frxname   = ADDBS(ALLTRIM(company.dir_nm))+ALLTRIM(_RptName)+'.rpt'		&&vasant110710
	lcstr_one  = ''
	lcstr_two  = ''
	lcstr_thrd = ''
	lcstr_one  = allt(r_status.sqlquery)
	lcstr_one  = IIF(AT(';',lcstr_one) > 0,left(lcstr_one,AT(';',lcstr_one)-1),lcstr_one) +" '','',"
	lcstr_two  = allt(r_status.spl_condn)
	lcstr_two  = IIF(EMPTY(lcstr_two),"''",lcstr_two)
*!*		lcstr_thrd = ",'"+DTOC(sdate)+"','"+DTOC(edate)+"','','','','',0,0,'','','','','"+_fware+"','"+_tware+"','','','',''"		&& Commented by Shrikant S. on 27/01/2016 for Bug-27523
	lcstr_thrd = ",'"+DTOC(_tmpvar.sdate)+"','"+DTOC(_tmpvar.edate)+"','','','','',0,0,'','','','','"+_fware+"','"+_tware+"','','','',''"			&& Added by Shrikant S. on 27/01/2016 for Bug-27523	
	mexecquery = lcstr_one + lcstr_two + lcstr_thrd
	SET DATE Briti

	_screenactiveform.panel.Panels(1).Text = 'Generating Report.....'
	&& Added by Shrikant S. on 17/01/2014 		&& Start
	If Type('r_status.Isrpttbl')<>'U'
		If r_status.Isrpttbl=.T.
			_tmptbl = '##tmp'+Sys(3)
			mexecquery=mexecquery+",'"+ALLTRIM(_tmptbl)+"'"
			nhandle = 0
			retrive=_screenactiveform.sqlconobj.dataconn([EXE],_screenactiveform.CompanyDb,mexecquery,'','nhandle')
			If retrive<=0
				Return .F.
			Endif
			mexecquery = 'select * from '+_tmptbl
		Endif
	Endif
	&& Added by Shrikant S. on 17/01/2014 		&& End

	IF _mDirecteFiling = .f.
		PrintFlag  = 4			&&6
		DO uecrviewer WITH ALLTRIM(_RptName)+'.rpt',mexecquery,PrintFlag
	ENDIF
	
	nhandle = 0
	nretval = _screenactiveform.sqlconobj.dataconn('EXE',_screenactiveform.CompanyDb,mexecquery,'lst_exc','nhandle')
	If nretval<=0
		=Messagebox('Connection Error',0,vumess)
		Return .F.
	ENDIF
	nsq = _screenactiveform.sqlconobj.sqlconnclose('nhandle')
	If nsq<=0
		Return .F.
	ENDIF
	If !USED('lst_exc')
		=Messagebox('Unable to get Data',0,vumess)
		Return .F.
		&&vasant050411
	ELSE
		IF RECCOUNT('lst_exc') = 0
			=MESSAGEBOX('No Data Available for the Selected Period',0,vumess)
			RETURN .f.
		ENDIF
		&&vasant050411
	ENDIF
ENDCASE

_TryError     = .f.
_TryErrMessage= ''
Try
	_TryErrMessage = BeforeRawXMLUpdates()
CATCH TO oException
	IF oException.ErrorNo <> 1
		_TryError = .t.
		=Messagebox(ALLTRIM(oException.Message),0,vumess)
	ENDIF
ENDTRY
IF _TryError = .t.
	Return .F.
ENDIF
IF TYPE('_TryErrMessage') = 'C'
	IF !EMPTY(_TryErrMessage)
		=Messagebox(_TryErrMessage,0,vumess)
		Return .F.
	Endif
Endif
&&vasant110710		r_status instead of _r_status 
&&vasant110710		after removing frx, check

_RepoGenSuccess = .f.

SELECT r_status
GO _ReportRecNo
_RepoGenSuccess =  RawXMLGenerater(_RptName,_eFileName)
IF _RepoGenSuccess = .f.
	RETURN .f.
Endif	

SELECT r_status
GO _ReportRecNo
IF _mACESFiling  = .t.
	_RepoGenSuccess =  ACESXMLGenerater(_RptName,_eFileName)
Else	
	_RepoGenSuccess =  FinalXMLGenerater(_RptName,_eFileName)
Endif
IF _RepoGenSuccess = .f.
	RETURN .f.
Endif	


*****Class Defining*****
DEFINE CLASS VudyogClass as Form

ENDDEFINE

Define Class ClHTMLGenerator As HTMLListener
	TargetFileName = _htmlrptfilename
	Quietmode      = .T.

Enddefine
