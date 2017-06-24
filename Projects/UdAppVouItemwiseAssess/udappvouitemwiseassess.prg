Lparameters _vouitemassfrmname		&&vasant100111
_curvouobj = _Screen.ActiveForm &&&Added by satish pal for bug-7678
&&&Added by satish pal for bug-4180 dt.18/05/2012--start

&& Commented by Shrikant S. on 28/05/2013 for Bug-12163		&& Start
*!*	IF TYPE("_SCREEN.ACTIVEFORM.mainAlias") <> "C"
*!*		RETURN 0
*!*	ENDIF
&& Commented by Shrikant S. on 28/05/2013 for Bug-12163		&& End
&&&Added by satish pal for bug-7678 dt.09/12/2012-Start
*!*	IF TYPE('_curvouobj.PcvType') <> 'C'
*!*		RETURN 0
*!*	ENDIF
If Type('_curvouobj.addmode')='U' Or Type('_curvouobj.editmode')='U'
	Return 0
Endif
If _curvouobj.addmode=.T. Or _curvouobj.editmode=.T.
&&&Added by satish pal for bug-7678 dt.09/12/2012-End
	If Type("_SCREEN.ACTIVEFORM.mainAlias") = "C"
		If Upper(Alltrim(_Screen.ActiveForm.mainAlias)) <> "MAIN_VW"
			Return 0  &&&Changed by satish pal for bug-4180 dt.02/08/2012
		Endif
*!*		SET  DATASESSIONID TO _SCREEN.ACTIVEFORM.DATASESSIONID
		If !Used("item_vw")
			Return 0
		Endif
	Endif
&&&Added by satish pal for bug-4180 dt.18/05/2012--end

*Birendra:On 12 oct 2011 for Bug-60 :start:
	If Type('_vouitemassfrmname')='L'
		_vouitemassfrmname=_Screen.ActiveForm
	Endif
*Birendra:On 12 oct 2011 for Bug-60 :End:

&& Added by Shrikant S. on 04/01/2017 for GST		&& Start
	objform=_curvouobj
	If Type('_curvouobj.ofrmfrom')<>'U'
		objform=_curvouobj.ofrmfrom
	Endif
&& Added by Shrikant S. on 04/01/2017 for GST		&& End

	massamt  = 0
	_mrprate = 0
	_mabtper = 0
	If Type('item_vw.u_mrprate') = 'N'
		_mrprate = item_vw.u_mrprate
		_mabtper = item_vw.abtper
	Endif
*Birendra:On 12 oct 2011 for Bug-60 :start:
	If Type('Main_vw.IncExcise')<>'U'
		If main_vw.IncExcise=.F.
			Select DcMast_vw
			Replace All excl_gross With "A" For Inlist(fld_nm,'CCDAMT','HCDAMT','U_CUSTAMT') And Entry_ty='P1' In DcMast_vw
		Else
			Select DcMast_vw
			Replace All excl_gross With "" For Inlist(fld_nm,'CCDAMT','HCDAMT','U_CUSTAMT') And Entry_ty='P1' In DcMast_vw
		Endif
	Endif
*Birendra:On 12 oct 2011 for Bug-60 :End:

&& Added by sandeep on 22/02/12 for bug-2373 -Start
	malias = Alias()
	Local vrate,vrateper
	mrateper = 1
	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
	nhandle=0

*!*		sq1=" select top 1 rateper from it_mast where It_code = ?Item_vw.It_code"				&& Commented by Shrikant S. on 22/12/2016 for GST
*!*		sq1=" select top 1 "+Iif(Inlist(objform.pcvtype,"PT","P1","PR"),'Prate','RatePer')+" as rateper from it_mast where It_code = ?Item_vw.It_code"	&& Commented by Shrikant S. on 22/12/2016 for GST (TO be rectify in stock valuation)
*!*		sq1=" select top 1 "+Iif(Inlist(main_vw.Entry_ty,"PT","P1","PR"),'Prate','RatePer')+" as rateper from it_mast where It_code = ?Item_vw.It_code"	&& Added by Shrikant S. on 22/12/2016 for GST (TO be rectify in stock valuation)
	sq1=" select top 1 Prate,RatePer from it_mast where It_code = ?Item_vw.It_code"	&& Added by Shrikant S. on 22/12/2016 for GST (TO be rectify in stock valuation)
	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)

	If nretval<0
		Return .F.
	Endif

&& Added by Shrikant S. on 17/03/2017 for GST		&& Start
	If Type('Lcode_vw.IoTrans')<>'U'
		Do Case
		Case Lcode_vw.IoTrans=1
			If EXCUR.Prate # 0
				mrateper = EXCUR.Prate
			Endif
		Case Lcode_vw.IoTrans=2
			If EXCUR.rateper # 0
				mrateper = EXCUR.rateper
			Endif
		Endcase
	Endif
&& Added by Shrikant S. on 17/03/2017 for GST		&& End

*!*		vrateper=Iif(!Isnull(excur.rateper),excur.rateper,0)		&& Commented by Shriaknt S. on 20/03/2017 for GST
	
	=sqlconobj.sqlconnclose("nHandle")
	If Used("EXCUR")
		Use In excur
	ENDIF

&& Commented by Shriaknt S. on 20/03/2017 for GST 		&&Start	
*!*		If  vrateper # 0
*!*			mrateper=vrateper
*!*		Endif
&& Commented by Shriaknt S. on 20/03/2017 for GST 		&&End

&& Added by sandeep on 22/02/12 for bug-2373 -End


	Sele item_vw
	If _mrprate#0
		If _mabtper#0
&&massamt = Round((QTY*_mrprate)-(QTY*_mrprate*_mabtper)/100,2)
			massamt=QTY*Round(_mrprate-(_mabtper*_mrprate)/100,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012
		Else
&&massamt = Round((QTY*_mrprate),2)
			massamt = Round((QTY*_mrprate),company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012
		Endif
	Else
****** Changed By Sachin N. S. on 28/06/2010 for TKT-2669 ****** Start
		If _vouitemassfrmname.Multi_Cur = .T.		&&vasant100111
			If Upper(Alltrim(main1_vw.Fcname)) != Upper(Alltrim(company.Currency)) And !Empty(main1_vw.Fcname)		&&vasant300609	&&vasant300609
&&			massamt = Round(QTY*FCRATE,2)
				massamt = Round(QTY*FCRATE,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012
			Else
&&			massamt = Round((QTY*RATE)/mrateper,2)  &&change by sandeep on 22/02/12 for bug 2373
				massamt = Round((QTY*RATE)/mrateper,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012
			Endif
		Else
&&		massamt = Round((QTY*RATE)/mrateper,2)  &&change by sandeep on 22/02/12 for bug 2373
			massamt = Round((QTY*RATE)/mrateper,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012
		Endif
*!*	massamt = Round(QTY*RATE,2)
****** Changed By Sachin N. S. on 28/06/2010 for TKT-2669 ****** End
	Endif
&&&Added by satish pal for bug-7678-Start
Else
	massamt=0
Endif
&&&Added by satish pal for bug-7678-End

&& Added by Shrikant S. on 04/10/2016 for GST		&& Start
If Type('Item_vw.GTAXABLAMT')<>'U'
	Replace item_vw.GTAXABLAMT With massamt In item_vw
Endif
&& Added by Shrikant S. on 04/10/2016 for GST		&& End

Return massamt
