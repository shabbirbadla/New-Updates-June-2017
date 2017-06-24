Parameters mRepType,ShowForm,SessionId,_tcrstatus,_tctmpvar
****Versioning**** Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('ORDERZOOM',_CurrVerVal,JUSTFNAME(SYS(16)))
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

OrdObj = Newobject('Gen_Order','GenOrder.PRG')
OrdObj.SessionId = SessionId
OrdObj.ReportType= mRepType

Public _Orstatus,_Otmpvar

If ShowForm = .T.
	Select _rstatus
	Scatter Name _Orstatus
	Select _tmpvar
	Scatter Name _Otmpvar
Else
	If Vartype(_tcrstatus) <> 'O'
		Select _rstatus
		Scatter Name _tcrstatus
	Endif
	If Vartype(_tctmpvar) <> 'O'
		Select _tmpvar
		Scatter Name _tctmpvar
	Endif
	_Orstatus = _tcrstatus
	_Otmpvar = _tctmpvar
Endif

OrdObj.ReportName = _Orstatus.Rep_Nm	&& Changed By Sachin N. S. on 02/07/2010 for TKT-2644

If Type("_Orstatus.Vou_Type") = 'C'
	If !Empty(_Orstatus.Vou_Type)
		OrdObj.ReportType = _Orstatus.Vou_Type
&& Added By Sachin N. S. on 29/12/2008 for displaying as per details a/c
		macnarr		= Upper(_rstatus.Vou_Type)
		macnarr     = Substr(macnarr,Iif(At('ENTRY',macnarr) > 0,At('ENTRY',macnarr)+5,1))
		macnarr		= Substr(macnarr,Iif(At('=',macnarr) > 0,At('=',macnarr)+1,1))
		macnarr		= Substr(macnarr,1,Iif(At(';',macnarr) > 0,At(';',macnarr)-1,Len(macnarr)))
		OrdObj.ReportType = macnarr
		macnarr=''
		If At('COLUMN',Upper(_rstatus.Vou_Type)) > 0
			macnarr		= Upper(_rstatus.Vou_Type)
			macnarr     = Substr(macnarr,Iif(At('COLUMN',macnarr) > 0,At('COLUMN',macnarr)+6,1))
			macnarr		= Substr(macnarr,Iif(At('=',macnarr) > 0,At('=',macnarr)+1,1))
			macnarr		= Substr(macnarr,1,Iif(At(';',macnarr) > 0,At(';',macnarr)-1,Len(macnarr)))
		Endif
		nCnt  = Occurs(',',macnarr)
		nCnt  = Iif(!Empty(macnarr),nCnt + 1,0)
		Do While .T.
			If nCnt > 0
				OrdObj.xTraFlds		= OrdObj.xTraFlds 	 + "," + Substr(macnarr,1,At(':',macnarr)-1)
				OrdObj.xTraFldsCap 	= OrdObj.xTraFldsCap + "," + Substr(macnarr,At(':',macnarr)+1,At(':',macnarr,2)-At(':',macnarr)-1)
				OrdObj.xTraFldsOrd 	= OrdObj.xTraFldsOrd + "," + Substr(macnarr,At(':',macnarr,2)+1,Iif(At(',',macnarr)>0,At(',',macnarr)-At(':',macnarr,2)-1,Len(macnarr)-At(':',macnarr,2)))
			Else
				Exit
			Endif
			macnarr=Substr(macnarr,At(',',macnarr)+1,Len(macnarr)-At(',',macnarr))
			nCnt=nCnt-1
		Enddo
		OrdObj.xTraFlds = Strtran(Strtran(OrdObj.xTraFlds,'MAIN','D'),'ITEM','A')
*!*			OrdObj.xTraFlds	= macnarr
&& Added By Sachin N. S. on 29/12/2008 for displaying as per details a/c
	Endif
Endif
If ShowForm = .T.
	OrdObj.sdate = Iif(_Orstatus.isfr_date,_Otmpvar.sdate,{})
	OrdObj.edate = Iif(_Orstatus.isto_date,_Otmpvar.edate,{})
	Do Case
		Case _Orstatus.isfr_date And _Orstatus.isto_date
			OrdObj.dateFilter = 'And Betw(a.Date,_Otmpvar.Sdate,_Otmpvar.edate) '
		Case _Orstatus.isfr_date And _Orstatus.isto_date = .F.
			OrdObj.dateFilter = 'And a.Date <=_Otmpvar.Sdate '
		Case _Orstatus.isfr_date = .F. And _Orstatus.isto_date = .F.
			OrdObj.dateFilter = ''
	Endcase
Endif
OrdObj.Exec_Order_Report()
statdesktop.progressbar.Value = 0
statdesktop.progressbar.Visible = .F.

If OrdObj.lError <> .T.
	If ShowForm = .T.
*!*			Do Form frmorder With OrdObj.ReportType,OrdObj.SessionId,OrdObj.levelcode,_Orstatus,_Otmpvar,OrdObj.xTraFlds,OrdObj.xTraFldsCap,OrdObj.xTraFldsOrd
		Do Form FrmOrder With OrdObj.ReportType,OrdObj.SessionId,OrdObj.levelcode,_Orstatus,_Otmpvar,OrdObj.xTraFlds,OrdObj.xTraFldsCap,OrdObj.xTraFldsOrd,OrdObj.ReportName	&& Changed By Sachin N. S. on 02/07/2010 for TKT-2644
	Endif
Endif

Wait Clear
Store Null To OrdObj,_Orstatus,_Otmpvar
Release OrdObj,_Orstatus,_Otmpvar
