Lparameters _ooSqlConObj,_lldChkDate
Local _lStopSw
**** Added by Sachin N. S. on 13/02/2015 for Bug-25274

_lStopSw=.F.
nToolHandle1=0
If Empty(_lldChkDate)
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	Set DataSession To _etdatasessionid

	msqlstr = " Select getdate() as Date "
	nretval = _ooSqlConObj.dataconn("EXE","Vudyog",msqlstr,"_SrvDate","nToolHandle1",_etdatasessionid)
	If nretval<0
		Return .F.
	Endif

	dSrvDate = Ttod(_SrvDate.Date)
	nretval=_ooSqlConObj.sqlconnclose("nToolHandle1") && Connection Close
	If nretval<=0
		Return .F.
	Endif
Else
	dSrvDate = _lldChkDate
Endif
Release nToolHandle1

_mregname = Alltrim(ueReadRegMe.r_comp)
_macid 	  = Alltrim(ueReadRegMe.r_macid)

Do Case
	Case _mregname = 'VERTELLUS SPECIALTY MATERIALS (INDIA) PVT. LTD.' And _macid='BFEBFBFF000306C3'
		If dSrvDate > Ctod('31/05/2015')
			_lStopSw = .T.
		Endif
		*!*	Case _mregname = 'Manufacturing Company' And _macid='BFEBFBFF0001067A'
		*!*		If dSrvDate > Ctod('31/05/2015')
		*!*			_lStopSw = .T.
		*!*		ENDIF
Endcase

Return _lStopSw
