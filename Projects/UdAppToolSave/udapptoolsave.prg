&& Added By Shrikant S. on 29/12/2012 for Bug-2267		&& Start		&&vasant030412
_Malias 	= Alias()
_mRecNo	= Recno()
_curvouobj = _Screen.ActiveForm
Set DataSession To _curvouobj.DataSessionId
&& Added By Shrikant S. on 29/12/2012 for Bug-2267		&& End		&&vasant030412

Set DataSession To _Screen.ActiveForm.DataSessionId

Local sqlconobj
nhandle=0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
&&	If (_Screen.ActiveForm.pcvtype='ST' ) &&commented by sandeep for bug-1724  on 13/04/2012
*!*		If Inlist(_Screen.ActiveForm.pcvtype,'ST','PT')  && Added PT by sandeep for bug-1724  on 3/01/2013			&& Commented by Shrikant S. on 07/04/2014 for Bug-25719
	If Inlist(_Screen.ActiveForm.pcvtype,'ST','PT','EI')  && Added by Shrikant S. on 07/04/2014 for Bug-25719
		If !([vutex] $ vchkprod)
&&-->Diffrencial Rate Invoice
			If main_vw.u_choice
				Replace All dc_no With "DI" For Empty(dc_no) In item_vw
			Else
				Replace All dc_no With "" For dc_no="DI" In item_vw
			Endif
&&<--Diffrencial Rate Invoice
		Endif

&&-->Rup 08Aug09
&&		If (  (    main_vw.Rule='MODVATABLE' And (  Upper(Alltrim(coadditional.paydays)) = 'DAILY' Or Inlist(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return')))  Or (main_vw.Rule='REBATE')  ) And ([vuexc] $ vchkprod) &&Rup 04OCt09 Add Rebate
&&		IF ( inlist(main_vw.RULE,'REBATE') AND !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return') OR ( inlist(main_vw.RULE,'MODVATABLE') AND (  UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' OR INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return') ) ) )  AND ([vuexc] $ vchkprod) &&Add Rebate by sandeep for TKT-6275 24/02/2011
&&		IF ( inlist(main_vw.RULE,'REBATE') AND !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return') OR ( inlist(main_vw.RULE,'MODVATABLE') AND (  UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' AND MAIN_VW.ENTRY_TY='ST' OR INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return') ) ) )  AND ([vuexc] $ vchkprod) &&Add Rebate by sandeep for TKT-6275 24/02/2011 ADD ENTRY_TY='ST' for the bug-12587 12/04/13

&& added by sandeep for the bug-12533 on 18/04/2013---Start
		If Empty(CoAdditional.DailyDebit)
*!*				X1=[inlist(main_vw.RULE,'REBATE') AND !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return')	&& Commented by Shrikant S. on 18/12/2014 for Bug-24673

&&& Commented by suraj Kumawat date on 20-06-2015  for bug-26319  start
*!*				X1=[inlist(main_vw.RULE,'REBATE','CAPTIVE USE') AND !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return');	&& Added by Shrikant S. on 18/12/2014 for Bug-24673
*!*		 OR (inlist(main_vw.RULE,'MODVATABLE') and ]
*!*				x2= [( UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' AND main_vw.entry_ty='ST' OR INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return')))]
*!*				x2=x2+[ OR (  UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' AND MAIN_vW.ENTRY_TY='EI' OR INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return','Scrap Sales','Supplementary Invoice') ) ]		&& Added by Shrikant S. on 07/04/2014 for Bug-25719
&&& Commented by suraj Kumawat date on 20-06-2015  for bug-26319  start

&&& added by suraj Kumawat date on 20-06-2015  for bug-26319  start
			X1=[(inlist(main_vw.RULE,'REBATE','CAPTIVE USE') AND !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return'));	&& Added by Shrikant S. on 18/12/2014 for Bug-24673
	 OR ((main_vw.RULE='MODVATABLE') and ]
			x2= [( UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' AND main_vw.entry_ty='ST' AND INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return')))]
			x2=x2+[ OR (  UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' AND MAIN_vW.ENTRY_TY='EI' AND INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return','Scrap Sales','Supplementary Invoice') ) ]		&& Added by Shrikant S. on 07/04/2014 for Bug-25719
&&& added by suraj Kumawat date on 20-06-2015  for bug-26319  start

			X=X1+x2
		Else
			X=CoAdditional.DailyDebit
		Endif
		If (Evaluate(X) And ([vuexc] $ vchkprod))
&& added by sandeep for the bug-12533 on 18/04/2013 ---End

			_malias3 	= Alias()
			_mrecno3	= Recno()

			Local mpac_nm,miac_nm,mcac_nm,mcpac_nm,mciac_nm,mccac_nm,mdefa_dbac,mdefa_crac,msac_nm,mcsac_nm,mcvdaac_nm,mcvdcac_nm,mhpac_nm,mhiac_nm,mhcac_nm,mhsac_nm,mbcdcac_nm,mbcdaac_nm
			sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
			mpac_nm		=	"BALANCE WITH EXCISE PLA"
			miac_nm 	=	"BALANCE WITH EXCISE RG23A"
			mcac_nm		=	"BALANCE WITH EXCISE RG23C"
			msac_nm		=	"BALANCE WITH SERVICE TAX A/C"
			mcvdaac_nm	=	"BALANCE WITH CVD RG23A"
			mcvdcac_nm	=	"BALANCE WITH CVD RG23C "
			mcpac_nm	=	"BALANCE WITH CESS SURCHARGE PLA"
			mciac_nm	=	"BALANCE WITH CESS SURCHARGE RG23A"
			mccac_nm	=	"BALANCE WITH CESS SURCHARGE RG23C"
			mcsac_nm	=	"BALANCE WITH SERVICE TAX CESS A/C"
			mhpac_nm	="BALANCE WITH HCESS PLA"
			mhiac_nm 	="BALANCE WITH HCESS RG23C"
			mhcac_nm	="BALANCE WITH HCESS RG23A"
			mhsac_nm	="BALANCE WITH SERVICE TAX HCESS A/C"
			mbcdaac_nm	="BALANCE WITH BCD RG23A"
			mbcdcac_nm	="BALANCE WITH BCD RG23C"

			nretval = sqlconobj.dataconn([EXE],company.dbname,"SELECT defa_db,defa_cr FROM lcode WHERE entry_ty='ST'","_lcode","nHandle",_Screen.ActiveForm.DataSessionId)
			If nretval<0
				Return .F.
			Endif
			Select _lcode
			mdefa_dbac	=	Evaluate(defa_db)
			mdefa_crac	=	Evaluate(defa_cr)
*!*				If Inlist(Allt(Uppe(main_vw.Rule)),[MODVATABLE],[REBATE]) &&Add Rebate by sandeep for TKT-6275 21/02/2011		&& Commented by Shrikant S. on 18/12/2014 for Bug-24673
			If Inlist(Allt(Uppe(main_vw.Rule)),[MODVATABLE],[REBATE],[CAPTIVE USE]) 							&& Added by Shrikant S. on 18/12/2014 for Bug-24673	
				Local plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal
				Store 0 To plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal

				Local sq1,sq2,whedate,sq3
				sq1=" SELECT AC_MAST.AC_NAME ,amount=SUM(CASE WHEN EX_VW_ACDET.AMT_TY='DR' THEN EX_VW_ACDET.AMOUNT ELSE -EX_VW_ACDET.AMOUNT END) "
				sq2=" FROM EX_VW_ACDET INNER JOIN AC_MAST ON (AC_MAST.AC_ID=EX_VW_ACDET.AC_ID) "
*!*					whedate=" WHERE (EX_VW_ACDET."+IIF(coadditional.dbdate=.T.,'DATE','U_CLDT')+"<= '"+ALLTRIM(DTOC(main_vw.DATE))+"') AND not (EX_VW_ACDET.TRAN_CD<> "+CAST(main_vw.tran_cd AS VARCHAR(10))+" AND EX_VW_ACDET.ENTRY_TY='ST') "
				whedate=" WHERE (EX_VW_ACDET."+Iif(CoAdditional.dbdate=.T.,'DATE','U_CLDT')+"<= '"+Alltrim(Dtoc(main_vw.Date))+"' AND YEAR(EX_VW_ACDET."+Iif(CoAdditional.dbdate=.T.,'DATE','U_CLDT')+")> '1900') AND not (EX_VW_ACDET.TRAN_CD= "+Cast(main_vw.tran_cd As Varchar(10))+" AND EX_VW_ACDET.ENTRY_TY='ST') "  && Added By Shrikant S. on 22/11/2012 for Bug-7090
&& Commented By Shrikant S. on 22/11/2012 for Bug-7090		&& Start
*!*					IF !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return') && Added by Amrendra
*!*						whedate=" WHERE (EX_VW_ACDET."+Iif(coadditional.dbdate=.T.,'DATE','U_CLDT')+"<= '"+Alltrim(Dtoc(main_vw.Date))+"' AND YEAR(EX_VW_ACDET."+Iif(coadditional.dbdate=.T.,'DATE','U_CLDT')+")> '1900') "  && Added by Amrendra
*!*					ELSE  && Added by Amrendra
*!*						whedate=" WHERE (EX_VW_ACDET."+Iif(coadditional.dbdate=.T.,'DATE','U_CLDT')+"<= '"+Alltrim(Dtoc(main_vw.Date))+"' AND YEAR(EX_VW_ACDET."+Iif(coadditional.dbdate=.T.,'DATE','U_CLDT')+")> '1900') AND not (EX_VW_ACDET.TRAN_CD<> "+Cast(main_vw.tran_cd As Varchar(10))+" AND EX_VW_ACDET.ENTRY_TY='ST') "  && Changed by Sachin for TKT-1487 on 24/05/2010
*!*					ENDIF  && Added by Amrendra
&& Commented By Shrikant S. on 22/11/2012 for Bug-7090		&& End
				sq3=" GROUP BY AC_MAST.AC_NAME  HAVING SUM(CASE WHEN EX_VW_ACDET.AMT_TY='DR' THEN EX_VW_ACDET.AMOUNT ELSE -EX_VW_ACDET.AMOUNT END)<>0 "
				nretval = sqlconobj.dataconn([EXE],company.dbname,"SET DATEFORMAT DMY "+sq1+sq2+whedate+sq3,"CURBAL","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Endif

				Select curbal
				Replace All amount With 0  In curbal  For Isnull(amount)
				Go Top
				Do While !Eof()
					Do Case
					Case Alltrim(curbal.ac_name)="BALANCE WITH EXCISE PLA"
						plabal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH EXCISE RG23A"
						rg23abal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH EXCISE RG23C"
						rg23cbal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH SERVICE TAX A/C"
						sertaxbal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH CVD RG23A"
						cvdabal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH CVD RG23C"
						cvdcbal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH CESS SURCHARGE PLA"
						cplabal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH CESS SURCHARGE RG23A"
						crg23abal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH CESS SURCHARGE RG23C"
						crg23cbal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH SERVICE TAX CESS A/C"
						csertaxbal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH HCESS PLA"
						phcess=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH HCESS RG23A"
						ahcess=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH HCESS RG23C"
						chcess=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH SERVICE TAX HCESS A/C"
						shcess=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH BCD RG23A"
						bcdabal=curbal.amount
					Case Alltrim(curbal.ac_name)="BALANCE WITH BCD RG23C"
						bcdcbal=curbal.amount
					Otherwise
					Endcase
					Skip
				Enddo

&& mbal1 = Iif(plabal <=0,0,plabal)+Iif(rg23abal <=0,0,rg23abal)+Iif(rg23cbal <=0,0,rg23cbal)+Iif(sertaxbal <=0,0,sertaxbal) &&COMMENTED BY SATISH PAL FOR
				mbal1 = Iif(plabal <=0,0,plabal)+Iif(rg23abal <=0,0,rg23abal)+Iif(rg23cbal <=0,0,rg23cbal)+Iif(sertaxbal <=0,0,sertaxbal)+Iif(bcdabal <=0,0,bcdabal)+Iif(bcdcbal <=0,0,bcdcbal)+Iif(cvdabal<=0,0,cvdabal)+Iif(cvdcbal<=0,0,cvdcbal) &&&satish for bug-678 dated:-09/12/2011
				mbal2 = Iif(cplabal <=0,0,cplabal)+Iif(crg23abal <=0,0,crg23abal)+Iif(crg23cbal <=0,0,crg23cbal)+Iif(csertaxbal <=0,0,csertaxbal)
				mbal3 = Iif(phcess <=0,0,phcess)+Iif(ahcess <=0,0,ahcess)+Iif(chcess <=0,0,chcess)+Iif(shcess <=0,0,shcess)
				mbal4 = mbal1+Iif(cvdabal <=0,0,cvdabal)+Iif(cvdcbal <=0,0,cvdcbal)
				mbal5 = mbal1+mbal4+Iif(bcdabal <=0,0,bcdabal)+Iif(bcdcbal <=0,0,bcdcbal)

&& added by Shrikant S. on 22/11/2012 for Bug-7090		&& Start
				If _Screen.ActiveForm.EditMode=.T.
					mbal1 = mbal1 +main_vw.examt
					mbal2 = mbal2 +main_vw.u_cessamt
					mbal4 = mbal4 +main_vw.u_hcesamt
					mbal5 = mbal5 +main_vw.bcdamt
				Endif
&& added by Shrikant S. on 22/11/2012 for Bug-7090		&& End

&&		If mbal1 < main_vw.examt Or mbal2 < main_vw.u_cessamt Or mbal4 < main_vw.u_hcesamt Or mbal5<main_vw.bcdamt &&commented  by sandeep for bug-12447 on 08-Jun-13
				If mbal1 < main_vw.examt Or mbal1+mbal2+mbal5-(main_vw.examt) < main_vw.u_cessamt Or mbal1+mbal2+mbal4+mbal5-(main_vw.examt+main_vw.u_cessamt) < main_vw.u_hcesamt  Or mbal1+mbal5-(main_vw.examt)<main_vw.bcdamt &&changes  by sandeep for bug-12447 on 08-Jun-13

					Messagebox('Entry Cannot be Saved due to InSufficient Credit '+Chr(13)+;
						'PLA Balance Rs.'+Str(plabal,12,2)+Chr(13)+;
						'RG 23-A Balance Rs.'+Str(rg23abal,12,2)+Chr(13)+;
						'RG 23-C Balance Rs.'+Str(rg23cbal,12,2)+Chr(13)+;
						'SERVICE TAX Balance Rs.'+Str(sertaxbal,12,2)+Chr(13)+;
						'CVD RG23A Balance Rs.'+Str(cvdabal,12,2)+Chr(13)+;
						'CVD RG23C Balance Rs.'+Str(cvdcbal,12,2)+Chr(13)+;
						'CESS SURCHARGE PLA Balance Rs.'+Str(cplabal,12,2)+Chr(13)+;
						'CESS SURCHARGE RG 23-A Balance Rs.'+Str(crg23abal,12,2)+Chr(13)+;
						'CESS SURCHARGE RG 23-C Balance Rs.'+Str(crg23cbal,12,2)+Chr(13)+;
						'CESS ON SERVICE TAX Balance Rs.'+Str(csertaxbal,12,2)+Chr(13)+;
						'S & H CESS SURCHARGE PLA Balance Rs.'+Str(phcess,12,2)+Chr(13)+;
						'S & H CESS SURCHARGE RG 23-A Balance Rs.'+Str(ahcess,12,2)+Chr(13)+;
						'S & H CESS SURCHARGE RG 23-C Balance Rs.'+Str(chcess,12,2)+Chr(13)+;
						'S & H CESS ON SERVICE TAX Balance Rs.'+Str(shcess,12,2)+Chr(13)+;
						'BCD RG23A Balance Rs.'+Str(bcdabal,12,2)+Chr(13)+;
						'BCD RG23C Balance Rs.'+Str(bcdcbal,12,2);
						,64,vumess)


					Rele mpac_nm,miac_nm,mcac_nm,mcpac_nm,mciac_nm,mccac_nm,mdefa_dbac,mdefa_crac,msac_nm,mcsac_nm,mcvdaac_nm,mcvdcac_nm,mhpac_nm,mhiac_nm,mhcac_nm,mhsac_nm,mbcdaac_nm,mbcdcac_nm
					Rele plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal
					Return .F.
				Else

					frmrt = .F.
					Do Form uefrm_st_dailydebit With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.EditMode,plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal To frmrt
					If frmrt = .F.
						Return .F.
					Endif

				Endi
			Else

				Select main_vw
				Replace u_expla With 0,u_exrg23ii With 0,u_rg2amt With 0,;
					u_cessamtp With 0,u_cessamta With 0,u_cessamtc With 0,;
					u_hcesamtp With 0,u_hcesamta With 0,u_hcesamtc With 0,;
					u_plasr With '',u_rg23no With '',u_rg23cno With '',;
					serbamt With 0,sercamt With 0,serhamt With 0 ,;
					bcdamta With 0,bcdamtc With 0,;
					u_cvdamt With 0  In main_vw &&u_cvdpay Rup 04Oct09 With 0
				mamount = 0
				Sele acdet_vw
				Locate For ac_name = mpac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = miac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mcac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mcpac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mciac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mccac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = msac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mcsac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mcvdaac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mcvdcac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif

				Locate For ac_name = mhpac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mhiac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mhcac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mhsac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif

				Locate For ac_name = mbcdaac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif
				Locate For ac_name = mbcdcac_nm
				If Found()
					mamount = mamount + acdet_vw.amount
					Delete In acdet_vw
				Endif

				If mamount # 0
					Locate For ac_name = mdefa_crac
					If Found()
						Replace amount With amount + mamount
					Endif
				Endif
			Endif
			Rele mpac_nm,miac_nm,mcac_nm,mcpac_nm,mciac_nm,mccac_nm,mdefa_dbac,mdefa_crac,msac_nm,mcsac_nm,mcvdaac_nm,mcvdcac_nm,mhpac_nm,mhiac_nm,mhcac_nm,mhsac_nm,mbcdaac_nm,mbcdcac_nm
			If !Empty(_malias3)
				Select &_malias3
			Endif
			If Betw(_mrecno3,1,Reccount())
				Go _mrecno3
			Endif
		Endif
&&<---Rup 08Aug09

	Endif
Endif

*!*	--------------------update cldate,part-ii srno,pla srno in acdet as per main table
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
*!*		If Inlist(_Screen.ActiveForm.pcvtype,'BI','RP','RR','GI','OB','GR','HI','HR','SR','ST','VI','PT','VR','JV','P1','BP','CP','J2')&&11Sep09 -- Rupesh -- Added JV &&TKT-2647 Add 'J2'
	If Inlist(_Screen.ActiveForm.pcvtype,'BI','RP','RR','GI','OB','GR','HI','HR','SR','ST','VI','PT','VR','JV','P1','BP','CP','J1','J2','J3')&&11Sep09 -- Rupesh -- Added JV &&TKT-2647 Add 'J2' &&TKT-4123 Add J1
*!*  Added by Shrikant  on 08Oct09 ----------- Start
		If (_Screen.ActiveForm.pcvtype="SR" )
			Replace u_gcssr With .T. In main_vw
		Endif
*!*  Added by Shrikant  on 08Oct09 ----------- End
*!*			IF TYPE('MAIN_VW.U_CLDT')='T' AND INLIST(_SCREEN.ACTIVEFORM.pcvtype,"OB","BB","BP","CP") &&Rup 07/10/09
*!*			If Type('MAIN_VW.U_CLDT')='T' And Inlist(_Screen.ActiveForm.pcvtype,"OB","BB","BP","CP","J1") &&Rup 07/10/09 &&TKT-4123 Add J1
		If Type('MAIN_VW.U_CLDT')='T' And Inlist(_Screen.ActiveForm.pcvtype,"OB","BB","BP","CP","J1","ST","P1") &&added by satish pal for bug-98 AND BUG-678
			If Empty(main_vw.u_cldt) Or Year(main_vw.u_cldt)<=1900
				Replace u_cldt With main_vw.Date In main_vw
			Endif
		Endif

		If Type('MAIN_VW.U_CLDT')='T' And Type('ACDET_VW.U_CLDT')='T'
			Replace All u_cldt With main_vw.u_cldt In acdet_vw
		Endif
		If Type('MAIN_VW.U_RG23NO')='C' And Type('ACDET_VW.U_RG23NO')='C'

			Replace All u_rg23no With main_vw.u_rg23no In acdet_vw
		Endif
		If Type('MAIN_VW.U_RG23CNO')='C' And Type('ACDET_VW.U_RG23CNO')='C'

			Replace All u_rg23cno With main_vw.u_rg23cno In acdet_vw
		Endif
		If Type('MAIN_VW.U_PLASR')='C' And Type('ACDET_VW.U_PLASR')='C'
			Replace All u_plasr With main_vw.u_plasr In acdet_vw
		Endif
&& Rup--->12/08/2009
		If Type('main_vw.u_deliver')='C' And _Screen.ActiveForm.pcvtype='ST'
			If main_vw.cons_id>0 And  Empty(main_vw.u_deliver)
				sq1= "Select top 1 ac_name from ac_mast where ac_id=?MAIN_VW.cons_id"
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_consid","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Else
					Replace main_vw.u_deliver With _consid.ac_name In main_vw
				Endif
				If Used('_consid')
					Use In _consid
				Endif
			Endif
		Endif
&&<---Rup 12/08/2009

&&--->Shrikant s. on 10 Mar, 2010
*!*			If Inlist(_Screen.ActiveForm.pcvtype,'PT','P1') And main_vw.Rule !='MODVATABLE'  &&Rup TKT-941 It Will create problem When only Service Tax Product is selected

&& Commented by Shrikant S. on 29/09/2016 for GST		&& Start
*!*	*!*			If Inlist(_Screen.ActiveForm.pcvtype,'PT','P1') And main_vw.Rule !='MODVATABLE' And ([vuexc] $ vchkprod)
*!*	*!*				If !Empty(main_vw.u_rg23no) Or !Empty(main_vw.u_rg23cno)
*!*	*!*					res = Messagebox("Click 'Yes' to clear Part-2 Serial No. and Save."+Chr(13)+"Else"+Chr(13)+"Click 'No'.",4+64,vumess)
*!*	*!*					If res = 7
*!*	*!*						Return .F.
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*				Replace u_rg23no With '' In main_vw
*!*	*!*				Replace u_rg23cno With '' In main_vw
*!*	*!*				If Type('MAIN_VW.U_RG23NO')='C' And Type('ACDET_VW.U_RG23NO')='C'
*!*	*!*					Replace All u_rg23no With main_vw.u_rg23no In acdet_vw
*!*	*!*				Endif
*!*	*!*				If Type('MAIN_VW.U_RG23CNO')='C' And Type('ACDET_VW.U_RG23CNO')='C'
*!*	*!*					Replace All u_rg23cno With main_vw.u_rg23cno In acdet_vw
*!*	*!*				Endif
*!*	*!*			Endif
&& Commented by Shrikant S. on 29/09/2016 for GST		&& End


&&<---Shrikant s. on 10 Mar, 2010
	Endif
Endif
*!*	--------------------update cldate,part-ii srno,pla srno in acdet as per main table

&&--->TKT-941
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
	If (_Screen.ActiveForm.pcvtype='J1' ) And Type('main_vw.ser_adj')='C'

		Update lother_vw Set tbl_nm='main_vw1' Where e_code='J1' And Lower(fld_nm)='serty' &&TKT-4006 Due to changes in uevoucher TKT-381 LMC

		If main_vw.ser_adj='Set-Off'  And !Empty(main_vw.serty)
			Select acdet_vw
			Scan
				sq1="select ac_name,typ from ac_mast where ac_id="+Str(acdet_vw.ac_id)
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_consid","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Else
					If  Inlist(_consid.typ,"Service Tax Payable","Edu. Cess on Service Tax Payable","S & H Cess on Service Tax Payable")
						Replace acdet_vw.serty With  main_vw.serty In acdet_vw
					Endif
					If  Inlist(_consid.typ,"Service Tax Available","Edu. Cess on Service Tax Available","S & H Cess on Service Tax Available") And !Empty(main_vw.sertyi)
						Replace acdet_vw.serty With  main_vw.sertyi In acdet_vw
					Endif
				Endif
			Endscan
		Endif
		If main_vw.ser_adj='Advance Adjustment' And !Empty(main_vw.sertyi) And !Empty(main_vw.serty)
			Select acdet_vw
			Scan
				If  amt_ty='CR'
					Replace acdet_vw.serty With  main_vw.sertyi In acdet_vw
				Endif
				If  amt_ty='DR'
					Replace acdet_vw.serty With  main_vw.serty In acdet_vw
				Endif
			Endscan
		Endif
	Endif
&&--->TKT-794 GTA
	If (_Screen.ActiveForm.pcvtype='J3' )
		If !Empty(main_vw.serty)
			Select acdet_vw
			Scan
				sq1="select ac_name,typ from ac_mast where ac_id="+Str(acdet_vw.ac_id)
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_acty","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Else
					If  Inlist(_acty.typ,"Service Tax Available","Edu. Cess on Service Tax Available","S & H Cess on Service Tax Available") And !Empty(main_vw.serty)
						Replace acdet_vw.serty With  main_vw.serty In acdet_vw
					Endif
				Endif
			Endscan
		Endif
	Endif
&&<---TKT-794 GTA
Endif
&&<---TKT-941

&&--->Rup 22/12/2010 TKT-5345
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
	If ( [vutds] $ vchkprod)  And (_Screen.ActiveForm.pcvtype='BP')
		sq1="select ac_name,typ from ac_mast where ac_id="+Str(main_vw.ac_id)
		nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_actype","nHandle",_Screen.ActiveForm.DataSessionId)
		If nretval<0
			Return .F.
		Else
			If  Inlist(_actype.typ,"TDS","TDS-ECESS","TDS-HCESS","TDS-SUR")
				If Empty(main_vw.U_BSRCODE) Or (Len(Alltrim(main_vw.U_BSRCODE))>0 And Len(Alltrim(main_vw.U_BSRCODE))!=7)
					Messagebox("BSR Code length should be 7" ,64,vumess)
					Return .F.
				Endif
				If Empty(main_vw.U_CHALNO)
					Messagebox("Challan No. Cannot be blank" ,64,vumess)
					Return .F.
				Endif
				If Empty(main_vw.U_CHALDT) Or (main_vw.U_CHALDT<main_vw.Date)
					Messagebox("Challan Date Cannot be less then Transaction Date" ,64,vumess)
					Return .F.
				Endif
				If Empty(main_vw.cheq_no)
					Messagebox("Cheque No Cannot be blank" ,64,vumess)
					Return .F.
				Endif

			Endif

		Endif
	Endif
Endif
&&<---Rup 22/12/2010 TKT-5345


&&Rup 19/01/2011 TKT-5692 TCS--->
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
	If _Screen.ActiveForm.TcsPage=.T. And (Inlist(_Screen.ActiveForm.Behave,'BR','CR') And (main_vw.tdspaytype=2) )
		If Empty(main_vw.svc_cate)
			Return .T.
		Endif
		Select dcmast_vw
		Select * From dcmast_vw Where Lower(pert_name) In ('tds_tp','sc_tp','ec_tp','hc_tp') Into Cursor tdc1 NOFILTER  Readwrite
		Select tdc1
		Scan
			vfld_nm="main_vw."+Alltrim(tdc1.fld_nm)
			vcrac_name=tdc1.crac_name
			vdac_name='"TCS Adjustment A/C"'
			vamount  =Evaluate(vfld_nm)
			If vamount<=0
				Loop
			Endif
			vac_id=0
			vacname =_Screen.ActiveForm.FindAcct(vcrac_name)
			vac_id=_Screen.ActiveForm.ac_id
			vamt_ty="CR"
			If vac_id>0
				_Screen.ActiveForm.AddToEffect(vamount,vamt_ty,vacname,vac_id,0)
			Endif

			sq1="select ac_name,tds_accoun from ac_mast where ac_id="+Str(vac_id)
			nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_acnm1","nHandle",_Screen.ActiveForm.DataSessionId)
			vac_id=0
			If nretval<0
				Return .F.
			Else
				If !Empty(_acnm1.tds_accoun)
					vacname =_Screen.ActiveForm.FindAcct('"'+Alltrim(_acnm1.tds_accoun)+'"')
					vac_id=_Screen.ActiveForm.ac_id
				Endif
			Endif
			If vac_id>0
				vamt_ty="DR"
				_Screen.ActiveForm.AddToEffect(vamount,vamt_ty,vacname,vac_id,0)
			Endif
			Select tdc1
		Endscan
	Endif
Endif
&&<---Rup 19/01/2011 TKT-5692 TCS

*Birendra : 22 mar 2011 for Order Amendment
If 'trnamend' $ vchkprod
	Do VouToolSave In MainPrg
Endif

**** Added By Sachin N. S. on 30/09/2011 for TKT-9711 **** Start
If ([vuexc] $ vchkprod)
	If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
		Select fld_nm From lother_vw Where att_file = .T. And fld_nm='EXMCLEARTY' And e_code = main_vw.Entry_ty Into Cursor _cur1
		If Type('MAIN_VW.EXMCLEARTY')='C' And (!Empty(_cur1.fld_nm) Or _Screen.ActiveForm.pcvtype='ST')
			sq1="select EXMCLEARTY from RULES where [Rule]=?Main_vw.Rule "
			nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_exmClearTy","nHandle",_Screen.ActiveForm.DataSessionId)
			If nretval<0
				Return .F.
			Endif
			If Empty(main_vw.EXMCLEARTY) And !Empty(_exmClearTy.EXMCLEARTY)
				=Messagebox("Type of Clearance cannot be Empty",0+64,vumess)
				Return .F.
			Endif
		Else
			If Type('LMC_VW.EXMCLEARTY')='C' And (!Empty(_cur1.fld_nm) Or _Screen.ActiveForm.pcvtype='ST')
				sq1="select EXMCLEARTY from RULES where [Rule]=?Main_vw.Rule "
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_exmClearTy","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Endif
				If Empty(LMC_vw.EXMCLEARTY) And !Empty(_exmClearTy.EXMCLEARTY)
					=Messagebox("Type of Clearance cannot be Empty",0+64,vumess)
					Return .F.
				Endif
			Endif
		Endif
	Endif
Endif
**** Added By Sachin N. S. on 30/09/2011 for TKT-9711 **** End

*Commented By Amrendra for Bug-4973 on 13/12/2012 ------>
*!*	*Added for Bug-2253 on 16-03-2012 By Amrendra **** Start

*!*	IF USED('lcode_vw')
*!*		IF oGlblPrdFeat.UdChkProd('exmfgbp') AND INLIST(lcode_vw.inv_stk,'+','-')
*!*			If (Type('_screen.activeform.Behave')='C' And Used('ITEM_VW'))
*!*				tempx=0
*!*				IF inlist(_screen.activeform.Behave,'ST','DC') AND USED("Projectitref_vw") AND NOT INLIST(UPPER(ALLTRIM(main_vw.U_IMPORM)),'RAW MATERIAL SALE','PURCHASE RETURN')
*!*					select Projectitref_vw
*!*					SCAN
*!*						tempx=tempx+1
*!*						IF EMPTY(Projectitref_vw.BATCHNO)
*!*							=Messagebox("Batch No is required...",0+64,vumess)
*!*							Return .F.
*!*						ENDIF

*!*						If (Projectitref_vw.expdt < main_vw.date OR EMPTY(Projectitref_vw.expdt))
*!*							=Messagebox("Batch is Expired...",0+64,vumess)
*!*							Return .F.
*!*						ENDIF
*!*					ENDSCAN
*!*					IF tempx=0
*!*						=Messagebox("No Batch Selected ...",0+64,vumess)
*!*						Return .F.
*!*					ENDIF
*!*				ELSE
*!*					SELECT item_vw
*!*					SCAN
*!*						If Type('ITEM_VW.BATCHNO')='C' And Empty(item_vw.BATCHNO) AND NOT INLIST(UPPER(ALLTRIM(main_vw.U_IMPORM)),'RAW MATERIAL SALE','PURCHASE RETURN')
*!*							=Messagebox("Batch No is required...",0+64,vumess)
*!*							Return .F.
*!*						ENDIF
*!*						If Type('ITEM_VW.ExpDt')='T' And (item_vw.expdt < main_vw.date OR EMPTY(expdt)) AND NOT INLIST(UPPER(ALLTRIM(main_vw.U_IMPORM)),'RAW MATERIAL SALE','PURCHASE RETURN')
*!*							=Messagebox("Batch is Expired...",0+64,vumess)
*!*							Return .F.
*!*						ENDIF
*!*					ENDSCAN
*!*				ENDIF

*!*				SELECT item_vw
*!*				GO TOP
*!*				If Type('ITEM_VW.QcHoldQty')='N' AND Type('ITEM_VW.QcAceptQty')='N' AND Type('ITEM_VW.QcRejQty')='N'
*!*					SCAN
*!*						If (item_vw.QcAceptQty+item_vw.QcRejQty)<=0
*!*							replace QcHoldQty WITH qty IN item_vw
*!*						ENDIF
*!*					ENDSCAN
*!*				ENDIF
*!*			ENDIF
*!*		ENDIF
*!*	ENDIF
*!*	*Added for Bug-2253 on 16-03-2012 By Amrendra **** End

*!*	*!*	*Added for Bug-2242 on 20-03-2012 By Amrendra **** Start
*!*	*!*	If (Type('_screen.activeform.Behave')='C' And Used('ITEM_VW'))

*!*	*!*		SELECT item_vw
*!*	*!*		COUNT TO ztmprec FOR !DELETED()
*!*	*!*		IF ztmprec>0
*!*	*!*			GO TOP IN item_vw
*!*	*!*			IF oGlblPrdFeat.UdChkProd('exmfgbp') AND inlist(_screen.activeform.Behave,'ST','DC')
*!*	*!*				replace ALL expdt WITH CTOD("") FOR YEAR(expdt)<=1900 IN item_vw
*!*	*!*				SELECT item_vw
*!*	*!*				GO TOP
*!*	*!*				SCAN
*!*	*!*					If Type('ITEM_VW.ExpDt')='T' And (item_vw.expdt < main_vw.date OR EMPTY(expdt))
*!*	*!*						=Messagebox("Batch is Expired...",0+64,vumess)
*!*	*!*						Return .F.
*!*	*!*					ENDIF

*!*	*!*				ENDSCAN
*!*	*!*			ENDIF
*!*	*!*		ENDIF
*!*	*!*	ENDIF
*!*	*!*	*Added for Bug-2242 on 20-03-2012 By Amrendra **** End

*!*	*!*	*Added for Bug-2179 on 19-03-2012 By Amrendra **** Start
*!*	*!*	IF USED('lcode_vw')
*!*	*!*	IF oGlblPrdFeat.UdChkProd('qctrl') AND INLIST(lcode_vw.inv_stk,'+')
*!*	*!*		lnCurItem=RECNO('ITEM_VW')
*!*	*!*		If (Type('_screen.activeform.Behave')='C' And Used('ITEM_VW')) &&  _screen.activeform.Behave is not required here : Amrendra
*!*	*!*				SELECT item_vw
*!*	*!*				GO TOP
*!*	*!*				If Type('ITEM_VW.QcHoldQty')='N' AND Type('ITEM_VW.QcAceptQty')='N' AND Type('ITEM_VW.QcRejQty')='N'
*!*	*!*				SCAN
*!*	*!*					If (ITEM_VW.QcAceptQty+ITEM_VW.QcRejQty)<=0
*!*	*!*						replace QcHoldQty WITH qty IN ITEM_VW
*!*	*!*					ENDIF
*!*	*!*				ENDSCAN
*!*	*!*				ENDIF
*!*	*!*		ENDIF
*!*	*!*	ENDIF
*!*	*!*	ENDIF
*!*	*Added for Bug-2179 on 19-03-2012 By Amrendra **** End
*Commented By Amrendra for Bug-4973 on 13/12/2012 <------

*Changed for Bug-4973 on 13-07-2012 By Amrendra **** Start
BatchMandatory=.F.
UsingProjItRef=.F.
UsingOtherItRef=.F.
*!*	RawMatSale=.F.
*!*	PurchaseRet=.F.
UsingItemVw=.F.
isDeffInv=.F.
Set Step On

If 	Type("main_vw.u_choice")='L'
	isDeffInv=main_vw.u_choice
Endif


If Used('lcode_vw') And isDeffInv=.F.
*	IF oGlblPrdFeat.UdChkProd('exmfgbp') AND INLIST(lcode_vw.inv_stk,'+','-') &&Commented by Amrendra for bug-4379 on 18-12-2012
	If Inlist(lcode_vw.inv_stk,'+','-') &&Added by Amrendra for bug-4379 on 18-12-2012
		If (Type('_screen.activeform.Behave')='C' And Used('ITEM_VW'))
			BatchMandatory=.T.
			tempx=0
*			IF USED("Projectitref_vw") AND !inlist(_screen.activeform.Behave,'IP','OP')
			If Used("Projectitref_vw") And !Inlist(_Screen.ActiveForm.Behave,'IP','OP','AR','PT') And !Inlist(_Screen.ActiveForm.pcvtype,'EI') && Changed By Amrendra for Bug-8694/8739 on 05/02/2013
				UsingProjItRef=.T.
			Else
				Select fld_nm From lother_vw Where fld_nm='BATCHNO' And e_code = main_vw.Entry_ty And Upper(inter_use) ='.F.' Into Cursor _cur1Batch
				If _Tally>0
					UsingItemVw=.T.
				Else
					BatchMandatory=.F.
				Endif
				If Used('_cur1Batch')
					Use In _cur1Batch
				Endif
			Endif
*			IF inlist(_screen.activeform.Behave,'ST','DC')
			If Inlist(_Screen.ActiveForm.Behave,'ST','DC') And !Inlist(_Screen.ActiveForm.pcvtype,'EI')	&& Changed By Amrendra for Bug-8694/8739 on 05/02/2013
				BatchMandatory=.T.
				UsingProjItRef=.T.
				UsingOtherItRef=.T.
			Endif
*			IF inlist(_screen.activeform.Behave,'ST') &&AND (INLIST(UPPER(ALLTRIM(main_vw.u_imporm)),'RAW MATERIAL SALE','PURCHASE RETURN') OR INLIST(UPPER(ALLTRIM(main_vw.vatmtype)),'RAW MATERIAL SALE','PURCHASE RETURN'))
			If Inlist(_Screen.ActiveForm.Behave,'ST') And !Inlist(_Screen.ActiveForm.pcvtype,'EI') && Changed By Amrendra for Bug-8694/8739 on 05/02/2013
				If 	Type('main_vw.u_imporm')='C'
					If 	Inlist(Upper(Alltrim(main_vw.u_imporm)),'PURCHASE RETURN','RAW MATERIAL SALE')
						BatchMandatory=.F.
*!*							PurchaseRet=.T.
					Endif
*!*						IF 	INLIST(UPPER(ALLTRIM(main_vw.u_imporm)),'RAW MATERIAL SALE')
*!*							RawMatSale=.T.
*!*						ENDIF
				Endif
			Endif
		Endif
	Endif
Endif
BatchFound=.F.
If BatchMandatory=.T. And oGlblPrdFeat.UdChkProd('exmfgbp')
	Select item_vw
	Go Top
	isPickupCliecked=.T.
	Scan
&& Added by Shrikant S. on 20/08/2014 for Bug-23871		&& Start
		If !Empty(item_vw.dc_no) And !Empty(item_vw.batchno)
			UsingProjItRef=.F.
			UsingOtherItRef=.F.
			Loop
		Endif
&& Added by Shrikant S. on 20/08/2014 for Bug-23871		&& End

		isRawMat=.F.
		msqlstr="select [type] mattype from it_mast where it_code="+Alltrim(Str(item_vw.it_code))
		nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"TmpITMast","nhandle",_Screen.ActiveForm.DataSessionId)
		If Used('TmpITMast')
			If Upper(TmpITMast.mattype)=('RAW MATERIAL')
				isRawMat=.T.
			Endif
			Use In TmpITMast
		Endif
		If UsingProjItRef=.T. And isRawMat=.F.
			If Used('Projectitref_vw')
				If Reccount('Projectitref_vw')>0
&&Commented by Amrendra For Bug-7863 on 17/12/2012(Below line)
*					Select aitserial from Projectitref_vw with (buffering = .t.) where Projectitref_vw.Entry_ty=item_vw.Entry_ty and Projectitref_vw.tran_cd=item_vw.tran_cd and Projectitref_vw.itserial= item_vw.itserial AND EMPTY(Projectitref_vw.BATCHNO) into cursor _tempcur1
&&Added by Amrendra For Bug-7863 on 17/12/2012(Below line)
*					select * from item_vw with (buffering = .t.) where it_code not in (select it_code from Projectitref_vw with (buffering = .t.) WHERE Projectitref_vw.tran_cd=item_vw.tran_cd)  into cursor _tempcur1
&&Added by Amrendra For Bug-8819 on 14/2/2013(Below line)
					Select * From item_vw With (Buffering = .T.) Where Str(it_code)+itserial Not In (Select Str(it_code)+itserial From Projectitref_vw With (Buffering = .T.) Where Projectitref_vw.tran_cd=item_vw.tran_cd) Into Cursor _tempcur1
					If _Tally>0
						=Messagebox("No Batch Selected ...",0+64,vumess)
						If Used('_tempcur1')
							Use In _tempcur1
						Endif
						Return .F.
					Endif
					If Used('_tempcur1')
						Use In _tempcur1
					Endif
					Select item_vw
				Else
					=Messagebox("No Batch Selected ...",0+64,vumess)
					Select item_vw
					Return .F.
				Endif
			Else
				=Messagebox("No Batch Selected ...",0+64,vumess)
				Select item_vw
				Return .F.
			Endif
		Endif
		If isRawMat=.T. And UsingOtherItRef=.T.
			If Used('othitref_vw')
				If Reccount('othitref_vw')>0
					Select ritserial From othitref_vw With (Buffering = .T.) Where othitref_vw.Entry_ty=item_vw.Entry_ty And othitref_vw.tran_cd=item_vw.tran_cd And othitref_vw.itserial= item_vw.itserial Into Cursor _tempcur1
					If _Tally<=0
						=Messagebox("No Batch Selected ...",0+64,vumess)
						If Used('_tempcur1')
							Use In _tempcur1
						Endif
						Return .F.
					Endif
					If Used('_tempcur1')
						Use In _tempcur1
					Endif
					Select item_vw
				Else
					=Messagebox("No Batch Selected ...",0+64,vumess)
					Select item_vw
					Return .F.
				Endif
			Else
				=Messagebox("No Batch Selected ...",0+64,vumess)
				Select item_vw
				Return .F.
			Endif
		Endif
	Endscan
	If UsingItemVw=.T.
*Birendra : Bug-6207 on 12/04/2013 :start:
		Local MandatoryExpDt
		MandatoryExpDt=.F.
		Select fld_nm From lother_vw Where Upper(Alltrim(fld_nm))=='EXPDT' And Upper(Alltrim(e_code)) == Upper(Alltrim(main_vw.Entry_ty)) And Evaluate(mandatory) ==.T. Into Cursor _cur1expiry
		If _Tally>0
			MandatoryExpDt=.T.
		Endif
		If Used('_cur1expiry')
			Use In _cur1expiry
		Endif
*Birendra : Bug-6207 on 12/04/2013 :end:
		llbatchno=.T.				&& Added by Shrikant S. on 19/05/2015 for Bug-26131
		Select item_vw
		SCAN
&& Added by Shrikant S. on 19/05/2015 for Bug-26131			&& Start
			If Type('ITEM_VW.BATCHNO')='C'
				msqlstr="select batchvalid from it_mast where it_code="+Alltrim(Str(item_vw.it_code))
				nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"TmpITMast","nhandle",_Screen.ActiveForm.DataSessionId)
				If Used('TmpITMast')
					If TmpITMast.batchvalid=.T.
						llbatchno=.T.
					Else
						llbatchno=.F.
						MandatoryExpDt=.F.
					Endif
					Use In TmpITMast
				Endif
			Endif
&& Added by Shrikant S. on 19/05/2015 for Bug-26131			&& End		

*!*				If Type('ITEM_VW.BATCHNO')='C' And Empty(item_vw.batchno) &&AND NOT INLIST(UPPER(ALLTRIM(main_vw.u_imporm)),'RAW MATERIAL SALE','PURCHASE RETURN') 		&& Commented by Shrikant S. on 19/05/2015 for Bug-26131
			If llbatchno And Empty(item_vw.batchno)  		&& Added by Shrikant S. on 19/05/2015 for Bug-26131
				=Messagebox("Batch No is required...",0+64,vumess)
				Return .F.
			Endif
			If MandatoryExpDt &&Birendra : Bug-6207 on 12/04/2013
*!*					If Type('ITEM_VW.ExpDt')='T' And (item_vw.expdt < main_vw.Date Or Empty(expdt)) And Not Inlist(Upper(Alltrim(main_vw.u_imporm)),'RAW MATERIAL SALE','PURCHASE RETURN')		&& Commented by Shrikant S. on 20/06/2015 for bug-26131
				If Type('ITEM_VW.ExpDt')='T' And (item_vw.expdt < main_vw.Date Or Empty(item_vw.expdt)) And Not Inlist(Upper(Alltrim(main_vw.u_imporm)),'RAW MATERIAL SALE','PURCHASE RETURN')	&& Added by Shrikant S. on 20/06/2015 for bug-26131
					=Messagebox("Batch is Expired...",0+64,vumess)
					Return .F.
				Endif
			ENDIF
&& Added by Shrikant S. on 20/06/2015 for bug-26131	&& Start
			If !Empty(item_vw.batchno) And Used('projectitref_vw') AND main_vw.entry_ty='OP'
				If Empty(Projectitref_vw.batchno)
					Select Projectitref_vw
					mrecno=Iif(!Eof(),Recno(),0)
					Locate For Entry_ty=main_vw.Entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
					If Found()
						Replace batchno With item_vw.batchno In Projectitref_vw
					ENDIF
					IF mrecno >0
						GO mrecno
					endif
				Endif
			Endif
&& Added by Shrikant S. on 20/06/2015 for bug-26131	&& End
		Endscan
	Endif
Endif

*Added for Bug-4973 on 20-09-2012 By Amrendra **** ---->
If Used('ITEM_VW')
	lnCurItem=Iif(!Eof('ITEM_VW'),Recno('ITEM_VW'),0)
Endif

If Used('lcode_vw')
*!*		If (lcode_vw.QC_Module) = .t.  AND isDeffInv=.F.
	If (lcode_vw.QC_Module) = .T.  And isDeffInv=.F. And Used('Main_vw')		&& Changed by Sachin N. S. on 13/06/2014 for Bug-21381

		If (main_vw.Entry_ty='IP') && AND !USED("Projectitref_vw"))
			Select item_vw
			Go Top
			isPickupCliecked=.T.
			Scan
				sq1="select It_code from it_advance_setting where qcprocess=1 and it_code=?item_vw.it_code"
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_QcEnabledItem","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Endif
				If Used('_QcEnabledItem') And Reccount('_QcEnabledItem')>0
					If Used('othitref_vw')
						If Reccount('othitref_vw')>0
							Select ritserial From othitref_vw With (Buffering = .T.) Where othitref_vw.Entry_ty=item_vw.Entry_ty And othitref_vw.tran_cd=item_vw.tran_cd And othitref_vw.itserial= item_vw.itserial Into Cursor _tempcur1
							If _Tally<=0
								isPickupCliecked=.F.
							Endif
							If Used('_tempcur1')
								Use In _tempcur1
							Endif
							Select item_vw
						Else
							isPickupCliecked=.F.
						Endif
					Else
						isPickupCliecked=.F.
					Endif
					Use In _QcEnabledItem
				Endif
				If isPickupCliecked=.F.
					=Messagebox("Please select item using Pickup Button In Grid...",0+64,vumess)
					Select item_vw
					Return .F.
				Endif
				Select item_vw
			Endscan
			If lnCurItem>0
				Go lnCurItem In item_vw
			Endif
		Endif
*****************************
		If (Inlist(main_vw.Entry_ty,'ST','DC'))&& AND PurchaseRet=.F.) &&AND !USED("Projectitref_vw"))
			Select item_vw
			Go Top
			isPickupCliecked=.T.
			Scan
&& Amrendra : Bug-4973 On 27/11/2012 ---->
				isRawMat=.F.
				msqlstr="select [type] mattype from it_mast where it_code="+Alltrim(Str(item_vw.it_code))
				nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"TmpITMast","nhandle",_Screen.ActiveForm.DataSessionId)
				If Used('TmpITMast')
					If Upper(TmpITMast.mattype)=('RAW MATERIAL')
						isRawMat=.T.
					Endif
					Use In TmpITMast
				Endif
&& Amrendra : Bug-4973 On 27/11/2012 <----
				sq1="select It_code from it_advance_setting where qcprocess=1 and it_code=?item_vw.it_code"
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_QcEnabledItem","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval<0
					Return .F.
				Endif
				If Used('_QcEnabledItem') And Reccount('_QcEnabledItem')>0
					If isRawMat=.T.
						If Used('othitref_vw')
							If Reccount('othitref_vw')>0
								Select ritserial From othitref_vw With (Buffering = .T.) Where othitref_vw.Entry_ty=item_vw.Entry_ty And othitref_vw.tran_cd=item_vw.tran_cd And othitref_vw.itserial= item_vw.itserial Into Cursor _tempcur1
								If _Tally<=0
									isPickupClieckedk=.F.
								Else
									BatchMandatory=.F.
								Endif
								If Used('_tempcur1')
									Use In _tempcur1
								Endif
								Select item_vw
							Else
								isPickupCliecked=.F.
								BatchMandatory=.T.
								=Messagebox("Please select item using Receipt Button In Grid...",0+64,vumess)
								Select item_vw
								Return .F.
							Endif
						Else
							isPickupCliecked=.F.
							BatchMandatory=.T.
							=Messagebox("Please select item using Receipt Button In Grid...",0+64,vumess)
							Select item_vw
							Return .F.
						Endif
*							USE IN _QcEnabledItem
					Else
						If Used('Projectitref_vw')
							If Reccount('Projectitref_vw')>0
								Select aitserial From Projectitref_vw With (Buffering = .T.) Where Projectitref_vw.Entry_ty=item_vw.Entry_ty And Projectitref_vw.tran_cd=item_vw.tran_cd And Projectitref_vw.itserial= item_vw.itserial Into Cursor _tempcur1
								If _Tally<=0
									isPickupClieckedk=.F.
								Endif
								If Used('_tempcur1')
									Use In _tempcur1
								Endif
								Select item_vw
							Else
								isPickupCliecked=.F.
								BatchMandatory=.T.
								=Messagebox("Please select item using Receipt Button In Grid...",0+64,vumess)
								Select item_vw
								Return .F.
							Endif
						Else
							isPickupCliecked=.F.
							BatchMandatory=.T.
							=Messagebox("Please select item using Receipt Button In Grid...",0+64,vumess)
							Select item_vw
							Return .F.
						Endif
					Endif
				Endif
				Use In _QcEnabledItem
				Select item_vw
			Endscan
			If lnCurItem>0
				Go lnCurItem In item_vw
			Endif
		Endif
*****************************
	Endif
Endif
*Added for Bug-4973 on 20-09-2012 By Amrendra **** ---->


**Added for Bug-4973 on 13-07-2012 By Amrendra **** Start ---->
If Used('lcode_vw') And Used('ITEM_VW') &&ADDED BY SATISH PAL FOR BUG-14853 DATED 30/05/2013
	If oGlblPrdFeat.UdChkProd('qctrl') And Inlist(lcode_vw.inv_stk,'+')
		lnCurItem=Recno('ITEM_VW')
		If (Type('_screen.activeform.Behave')='C' And Used('ITEM_VW')) &&  _screen.activeform.Behave is not required here : Amrendra
			Select item_vw
			Go Top
			If Type('ITEM_VW.QcHoldQty')='N' And Type('ITEM_VW.QcAceptQty')='N' And Type('ITEM_VW.QcRejQty')='N'
				Scan
					If (item_vw.QcAceptQty+item_vw.QcRejQty)<=0
						Replace QcHoldQty With qty In item_vw
					Endif
				Endscan
			Endif
*!*				IF lnCurItem>0
*!*					GO lnCurItem IN item_vw
*!*				ENDIF
		Endif
	Endif
Endif
*Added for Bug-4973 on 13-07-2012 By Amrendra **** End  <----

**** Added By Amrendra for Bug-933 & Bug-1219 on 15-02-2011
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
	If	(Type('main_vw.DataExport')='C')
		Replace DataExport With "" In main_vw
	Endif
	If	(Type('Item_vw.DataExport')='C' And Used('Item_vw'))
		Replace DataExport With "" In item_vw  && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace All DataExport With "" In item_vw  && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('AcDet_vw.DataExport')='C' And Used('AcDet_vw'))
*!*			Replace DataExport With "" In acdet_vw   && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace All DataExport With "" In acdet_vw  && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('ItRef_vw.DataExport')='C' And Used('ItRef_vw'))
*!*			Replace DataExport With "" In ItRef_vw && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace All DataExport With "" In ItRef_vw && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('Mall_vw.DataExport')='C' And Used('Mall_vw'))
*!*			Replace DataExport With "" In Mall_vw  && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace All DataExport With "" In Mall_vw && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('Manu_Det_vw.DataExport')='C' And Used('Manu_Det'))
*!*			Replace DataExport With "" In Manu_Det && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace All DataExport With "" In Manu_Det && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('LITEMALL_vw.DataExport')='C' And Used('LITEMALL_vw'))
*!*			Replace DataExport With "" In LITEMALL_vw && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace DataExport With "" In LITEMALL_vw&& Commented by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('Gen_SRNo_vw.DataExport')='C' And Used('Gen_SRNo_vw'))
*!*			Replace DataExport With "" In Gen_SRNo_vw && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace All DataExport With "" In Gen_SRNo_vw && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif
	If	(Type('Series_vw.DataExport')='C' And Used('Series_vw'))
*!*			Replace DataExport With "" In Series_vw && Commented by Archana K. on 14/06/13 for Bug-5837
		Replace  DataExport With "" In Series_vw && Changed by Archana K. on 14/06/13 for Bug-5837
	Endif

SET STEP ON 
&& Added By Kishor A. for Bug-26960 on 30/10/2015 Start..
	If	(Type('_ItSrTrn.DataExport')='C' And Used('_ItSrTrn'))
		Replace ALL DataExport With "" In _ItSrTrn 
	ENDIF
&& Added By Kishor A. for Bug-26960 on 30/10/2015 End..
ENDIF

**** Added By Amrendra for Bug-933 & Bug-1219 on 15-02-2011

&& Added By Shrikant S. on 29/12/2012 for Bug-2267		&& Start		&&vasant030412
If Type('_curvouobj.PcvType') = 'C' And Used('MAIN_VW')
	_mstkresrvtn = .F.
	_mstkresrvtn = oGlblPrdFeat.UdChkProd('stkresrvtn')
	If _mstkresrvtn = .T.
		If (Inlist(lcode_vw.Entry_ty,'DC','ST') Or Inlist(lcode_vw.bcode_nm,'DC','ST')) And Used('ItRef_vw')
			msqlstr="Select Entry_ty From Lcode Where Entry_ty in ('SO') Or Bcode_nm in ('SO')"
			sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If sql_con > 0 And Used('tmptbl_vw')
				_mCond = "a.REntry_ty = b.Entry_ty AND a.Entry_ty = c.Entry_ty AND a.Tran_cd = c.Tran_cd AND a.ItSerial = c.ItSerial"
				Select a.REntry_ty,a.ItRef_Tran,a.ritserial,Sum(a.Rqty) As Rqty,c.Item From ItRef_vw a,tmptbl_vw B,item_vw c ;
					WHERE &_mCond ;
					GROUP By a.REntry_ty,a.ItRef_Tran,a.ritserial,c.Item Into Cursor tmptbl_vw1
				Select tmptbl_vw1
				Scan
					mAllocQty = 0
					mRe_qty = tmptbl_vw1.Rqty

					msqlstr="Select Top 1 Re_Qty From SoItem Where Entry_ty = ?tmptbl_vw1.REntry_ty and Tran_cd = ?tmptbl_vw1.ItRef_Tran and ItSerial = ?tmptbl_vw1.RItSerial"
					sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
					If sql_con > 0 And Used('tmptbl_vw')
						If Reccount('tmptbl_vw') > 0
							mRe_qty = mRe_qty + tmptbl_vw.Re_Qty
						Endif
					Endif
					If _curvouobj.EditMode = .T.
						tmpitreftbl_nm = Iif(!Empty(lcode_vw.bcode_nm),Alltrim(lcode_vw.bcode_nm),Alltrim(lcode_vw.Entry_ty))+'ItRef'
						msqlstr="Select SUM(RQty) as Rqty From "+tmpitreftbl_nm+" Where REntry_ty = ?tmptbl_vw1.REntry_ty and ItRef_Tran = ?tmptbl_vw1.ItRef_Tran and RItSerial = ?tmptbl_vw1.RItSerial"
						sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
						If sql_con > 0 And Used('tmptbl_vw')
							If Reccount('tmptbl_vw') > 0
								mRe_qty = mRe_qty - Iif(Isnull(tmptbl_vw.Rqty)=.F.,tmptbl_vw.Rqty,0)
							Endif
						Endif
					Endif

					msqlstr="Select Top 1 AllocQty From StkResrvSum Where Entry_ty = ?tmptbl_vw1.REntry_ty and Tran_cd = ?tmptbl_vw1.ItRef_Tran and ItSerial = ?tmptbl_vw1.RItSerial"
					sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
					If sql_con > 0 And Used('tmptbl_vw')
						If Reccount('tmptbl_vw') > 0
							mAllocQty = Iif(Isnull(tmptbl_vw.AllocQty)=.F.,tmptbl_vw.AllocQty,0)
						Endif
					Endif

					Select item_vw
					If mAllocQty < mRe_qty
*!*							_curvouobj.ShowMessageBox("Stock Reservation not done for Item "+ALLTRIM(tmptbl_vw1.Item),0+32,vumess)
						_curvouobj.ShowMessageBox("Sales quantity is more than the reserved quantity : "+Transform(Iif(mAllocQty-mRe_qty>0,mAllocQty-mRe_qty,mAllocQty))+" for Item "+Alltrim(tmptbl_vw1.Item),0+32,vumess)		&& Changes done by Sachin N. S. on 17/06/2014 for Bug-21381
						If !Empty(_Malias)
							Select &_Malias
						Endif
						If Betw(_mRecNo,1,Reccount())
							Go _mRecNo
						Endif
						Return .F.
					Endif

					Select tmptbl_vw1
				Endscan
			Endif
			Use In tmptbl_vw
			Use In tmptbl_vw1
		Endif
	Endif
Endif

If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif
&& Added By Shrikant S. on 29/12/2012 for Bug-2267		&& End			&&vasant030412


