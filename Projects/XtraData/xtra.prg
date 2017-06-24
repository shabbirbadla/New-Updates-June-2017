Parameter wTable, tRecno,pcvType,mDataSession
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	TRY
		_VerRetVal = AppVerChk('EXTRA',GetFileVersion(),JUSTFNAME(SYS(16)))
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
Public vmsg, vToolbarCtr, filterCnd,pType
vToolbarCtr=0
filterCnd = ""
Local mNo
Local sqlconobj
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nHandle =0
pType=pcvType
mNo = 0
mNo = At(";", Alltr(wTable),1)

If mNo<>0
	filterCnd = Substr(Alltr(wTable), mNo+1)
	wTable = Substr(Alltr(wTable),1,mNo-1)
Endif
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* Start

LOCAL _UdNewTrigEnbl
_UdNewTrigEnbl = IIF(TYPE('UdNewTrigEnbl')='U',.F.,UdNewTrigEnbl)
If _UdNewTrigEnbl
	If File('UDAppetvalid.app')
		Set Proc To UDAppetvalid.App Additive
	Endif
	If File('UDTrigetvalid.fxp')
		Set Proc To UDTrigetvalid Additive
	Endif
Else
	If File('UeTrigetvalid.fxp')
		Set Proc To UeTrigetvalid Additive
	Endif
Endif
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* End

NretVal = .T.
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* Start
If _UdNewTrigEnbl
	If File('UDAppxtraOpen.app')
		NretVal=UDAppxtraOpen()
	Endif
	If File('UDTrigxtraOpen.fxp') And NretVal
		NretVal=UDTrigxtraOpen()
	Endif
Else
	If File('UeTrigxtraOpen.fxp')
		NretVal=UeTrigxtraOpen()
	Endif
Endif
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* End

If 	!NretVal
	Return .F.
Endif

If [vuexc] $ vchkProd And Empty(filterCnd)
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* Start
	If _UdNewTrigEnbl
		If File('UDAppetvalid.app')
			Set Proc To UDAppetvalid.App Additive
		Endif
		If File('UDTrigetvalid.fxp')
			Set Proc To UDTrigetvalid Additive
		Endif
	Else
		If File('UeTrigetvalid.fxp')
			Set Proc To UeTrigetvalid Additive
		Endif
	Endif
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* End


	nHandle = 0
	mNufact=sqlconobj.Dataconn("EXE",company.dbname,"select * from manufact","manufact","nHandle",mDataSession)
	If mNufact < 0
		Messagebox("Could Not Open Manufact Table",16,vUmess)
		Return .F.
	Endif
	Sele manufact
	mRet=sqlconobj.sqlconnclose("nHandle")
	If mRet <= 0
		Return .F.
	Endif

	If (Uppe(Allt(wTable))=[Main_Vw])
		If pcvType=[S ]
			If vToolbarCtr=0
				*!*					tbrDesktop.SETALL("enabled",.F.)
				vToolbarCtr=1
			Endif
			Do Form frmExciseManu1
			If Inlist(Allt(main_vw.Rule),[CT-1],[EOU EXPORT])
				If vToolbarCtr=0
					*!*						tbrDesktop.SETALL("enabled",.F.)
					vToolbarCtr=1
				Endif
				Do Form frmExciseManu2
			Endif
		Endif
	Endif

	If (Uppe(Allt(wTable))=[Main_Vw])
		If Allt(main_vw.Rule)==[EOU EXPORT] And (pcvType=[AI])
			If vToolbarCtr=0
				*				tbrDesktop.SETALL("enabled",.F.) && Tushar 26-04-06
				vToolbarCtr=1
			Endif
			Do Form frmExciseManu3
		Endif
	Endif

	*!*		If (UPPE(ALLT(wTable))=[LMAIN])
	*!*			If (Uppe(Allt(MANUFACT.PAYDAYS))=[DAILY]) And (!InList(Allt(Uppe(lmain_vw.DEPT)),[NON-MODVATABLE],[CT-1])) And (InList(pcVType,[S ],[GI],[HI],[SR]))
	*!*				If vToolbarCtr=0
	*!*					tbrDesktop.SETALL("enabled",.F.)
	*!*					vToolbarCtr=1
	*!*				EndIf
	*!*				do form frmExciseManu4
	*!*			EndIf
	*!*		EndIf
Endif
If [vutex] $ vchkProd
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* Start
	If _UdNewTrigEnbl
		If File('UDAppetvalid.app')
			Set Proc To UDAppetvalid.App Additive
		Endif
		If File('UDTrigetvalid.fxp')
			Set Proc To UDTrigetvalid Additive
		Endif
	Else
		If File('UeTrigetvalid.fxp')
			Set Proc To UeTrigetvalid Additive
		Endif
	Endif
******* Added By Sachin N. S. on 24/11/2010 for New Installer ******* End
Endif
If [vuexp] $ vchkProd And Empty(filterCnd)
	&& commented temporirly tushar
	*!*		IF expFormRun(wTable) = .F.
	*!*			RETU .F.
	*!*		ENDIF
	&& commented temporirly  by tushar
Endif

******** Added By Sachin N. S. on 02/04/2010 for TKT-817 ******** Start
nHandle=0
*!*	mSqlStr = "select distinct FormNo, FormDesc FROM FileUploadMaster WHERE e_Code=?ptype AND "+Iif(Uppe(Allt(wTable))=[MAIN_VW]," att_file=1 ",Iif(Uppe(Allt(wTable))=[ITEM_VW]," att_file=0 "," att_file=0 "))
mSqlStr = "select distinct FormNo, FormDesc FROM FileUploadMaster WHERE e_Code=?ptype AND "+Iif(INLIST(Uppe(Allt(wTable)),[MAIN_VW],[MAINADD_VW])," att_file=1 ",Iif(Uppe(Allt(wTable))=[ITEM_VW]," att_file=0 "," att_file=0 "))	&& Changed by Sachin N. S. on 12/09/2015 for Bug-26951
iNf11=sqlconobj.Dataconn("EXE",company.dbname,mSqlStr,"FlUpldMast","nHandle",mDataSession)
If iNf11 < 0
	Messagebox("Connection Error",16,vUmess)
	Return .F.
Endif
mRet=sqlconobj.sqlconnclose("nHandle")
If mRet <= 0
	Return .F.
Endif

Select FlUpldMast
lnFlUpld = Reccount('FlUpldMast')
******** Added By Sachin N. S. on 02/04/2010 for TKT-817 ******** End

Sele lother
*!*	Count For ! Deleted() And Ingrid = .F. And Inter_use=.F. To lnTotrec      && Commented by Archana K. on 08/08/12 for Bug-5693

&& Added by Archana K. on 08/08/12 for Bug-5693	start
Local valInteruse,lnTotrec,cbooleanval ,fieldnm ,cfieldnm
cfieldnm=""
lnTotrec =0
Scan
	cfieldnm=Alltrim(lother.head_nm)
	If ! Deleted() And Ingrid = .F.
		Try
			valInteruse=Iif(Type('Lother.inter_use')='L',Iif(lother.inter_use=.T.,".T.",".F."),Iif(!Empty(lother.inter_use),lother.inter_use,".F."))
			valInteruse=Evaluate(valInteruse)
		Catch
			fieldnm=fieldnm+","+cfieldnm
			cbooleanval=.T.
			LOOP
		Endtry
		If Type('valInteruse')='L'
			If valInteruse=.F.
				lnTotrec = lnTotrec +1
			Endif
		Else
			fieldnm=fieldnm+","+cfieldnm
			cbooleanval=.T.
			loop
		Endif

	Endif
Endscan
If cbooleanval=.T.
	=Messagebox("Cannot evaluate the expression for the field ("+Right(fieldnm,Len(fieldnm)-1)+")."+Chr(13)+;
		"Kindly check the 'Internal Use' & 'Mandatory' expression, it should return boolean values.",64,VuMess)
	Return .F.
Endif
&& Added by Archana K. on 08/08/12 for Bug-5693	end
Go Top
*!*	If lnTotrec = 0
If lnTotrec = 0 And lnFlUpld = 0
	Return .F.
Endif

If !Eof() Or lnFlUpld>0

	********** Added By Sachin N. S. on 02/04/2010 for TKT-817 ********** Start
	If Used('LotherSa2')
		Use In LotherSa2
	Endif
	Select FormNo, FormDesc From lother ;
		union ;
		select FormNo, FormDesc From FlUpldMast Order By FormNo Into Cursor curLother
	********** Added By Sachin N. S. on 02/04/2010 for TKT-817 ********** End
	llLother = .F.
	Sele curLother
	Go Top
	Scan

		Select Count(*) As cnt1 From lother Where lother.FormNo=curLother.FormNo Into Cursor cur1
		If cur1.cnt1<=0
			llLother = .T.		&& Added By Sachin N. S. on 02/04/2010 for TKT-817
			Select Count(*) As cnt1 From FlUpldMast Where FlUpldMast.FormNo=curLother.FormNo Into Cursor cur1	&& Added By Sachin N. S. on 02/04/2010 for TKT-817
			If cur1.cnt1<=0
				Loop
			Endif
		Endif

		s = ""		&& Added By Sachin N. S. on 02/04/2010 for TKT-817
		If llLother = .F.		&& Added By Sachin N. S. on 02/04/2010 for TKT-817
			Sele lother
			Do Case
*!*					Case Uppe(Allt(wTable)) = [MAIN_VW]
				Case INLIST(Uppe(Allt(wTable)),[MAIN_VW],[MAINADD_VW])		&& Changed by Sachin N. S. on 12/09/2015 for Bug-26951
					If Upper(Alltr(filterCnd)) = "INF11"
						Set Filter To (lother.FormNo=curLother.FormNo) And (lother.iNf11)
					Else
						Set Filter To (lother.FormNo=curLother.FormNo)
					Endif
					Go Top
					s=Iif(Empty(lother.FormDesc) Or Isnull(lother.FormDesc),[Additional Info (VOUCHERWISE)  ==>> FORM NO. ]+Allt(Str(curLother.FormNo)),lother.FormDesc)		&& Changed By Sachin N. s. on 16/03/2009
				Case Uppe(Allt(wTable)) = [ITEM_VW]
					Set Filter To (lother.FormNo=curLother.FormNo) And !lother.Ingrid
					Go Top
*!*						s=Iif(Empty(lother.FormDesc) Or Isnull(lother.FormDesc),[Additional Info (ITEMWISE)  ==>> FORM NO. ]+Allt(Str(curLother.FormNo)),lother.FormDesc)		&& Changed By Sachin N. s. on 16/03/2009		&& Commented by Shrikant S. on 25/05/2017 for GST
					s=Iif(Empty(lother.FormDesc) Or Isnull(lother.FormDesc),[Additional Info (LINEWISE)  ==>> FORM NO. ]+Allt(Str(curLother.FormNo)),lother.FormDesc)		&& Added by Shrikant S. on 25/05/2017 for GST
				Otherwise
					Set Filter To (lother.FormNo=curLother.FormNo)
					Go Top
					s=Iif(Empty(lother.FormDesc) Or Isnull(lother.FormDesc),[Additional Info],lother.FormDesc)		&& Changed By Sachin N. s. on 16/03/2009
			Endcase
			Go Top
		Else		&& Added By Sachin N. S. on 02/04/2010 for TKT-817
			Sele lother
			Set Filter To (lother.FormNo=curLother.FormNo)
			Go Top
			s=Iif(Empty(curLother.FormDesc) Or Isnull(curLother.FormDesc),[Additional Info],curLother.FormDesc)
		Endif		&& Added By Sachin N. S. on 02/04/2010 for TKT-817

		vctr=0
		Do Case
			Case Uppe(Allt(wTable)) = [_itmast]
				If curLother.FormNo = 1
					vctr=1
					If ([vuexc] $ vchkProd) Or ([vuexp] $ vchkProd)
						vctr=0
					Endif
				Endif
			Case Uppe(Allt(wTable)) = [_acmast]
				If curLother.FormNo = 1
					vctr=0
				Endif
		Endcase

		Sele lother
		Go Top
		If vctr = 0
			If vToolbarCtr=0 And Empty(filterCnd)
				vToolbarCtr=1
			Endif
			Aform=_Screen.ActiveForm
			If Vartype(Aform)<>"U"
				Do Form frmXtra With s,wTable, tRecno,pcvType,Aform,curLother.FormNo
			Else
				Do Form frmXtra With s,wTable, tRecno,pcvType
			Endif
		Endif
		Sele lother
		Set Filt To
		Sele curLother
	Endscan
Else
	Messagebox('No Additional Info.',64,vUmess)
Endif


*!*	If Used('lother')
*!*		Select lother
*!*		Use
*!*	Endif
If Used('lCode')
	Select lcode
	Use
Endif

If Used('Manufact')
	Select manufact
	Use
Endif
If ([vutex] $ vchkProd) Or ([vuexc] $ vchkProd)
Endif
Rele vmsg,vToolbarCtr

*>>>***Versioning**** Added By Amrendra On 08/07/2011
FUNCTION GetFileVersion
PARAMETERS lcTable
	_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	IF !EMPTY(lcTable)
		SELECT(lcTable)
		APPEND BLANK 
		replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
	ENDIF 
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 08/07/2011