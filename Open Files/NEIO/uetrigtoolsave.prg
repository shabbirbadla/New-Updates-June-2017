SET DATASESSION TO _SCREEN.ACTIVEFORM.DATASESSIONID
LOCAL sqlconobj

nhandle=0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
IF (TYPE('_screen.activeform.PCVTYPE')='C' AND USED('MAIN_VW'))
	IF (_SCREEN.ACTIVEFORM.pcvtype='ST' )
		&&-->Diffrencial Rate Invoice
		IF main_vw.u_choice
			REPLACE ALL dc_no WITH "DI" FOR EMPTY(dc_no) IN item_vw
		ELSE
			REPLACE ALL dc_no WITH "" FOR dc_no="DI" IN item_vw
		ENDIF
		&&<--Diffrencial Rate Invoice

		&&-->Rup 08Aug09
 && 		IF (  (    main_vw.RULE='MODVATABLE' AND (  UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' OR INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return')   )    )  OR (main_vw.RULE='REBATE')  ) AND ([vuexc] $ vchkprod) &&Rup 04OCt09 Add Rebate
			IF ( inlist(main_vw.RULE,'REBATE')AND !INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return') OR ( inlist(main_vw.RULE,'MODVATABLE') AND (  UPPER(ALLTRIM(coadditional.paydays)) = 'DAILY' OR INLIST(main_vw.u_imporm,'Raw Material Sale','Branch Transfer','Purchase Return'))) )  AND ([vuexc] $ vchkprod) &&Add Rebate by sandeep for TKT-6275 24/02/2011
			_malias3 	= ALIAS()
			_mrecno3	= RECNO()

			LOCAL mpac_nm,miac_nm,mcac_nm,mcpac_nm,mciac_nm,mccac_nm,mdefa_dbac,mdefa_crac,msac_nm,mcsac_nm,mcvdaac_nm,mcvdcac_nm,mhpac_nm,mhiac_nm,mhcac_nm,mhsac_nm,mbcdcac_nm,mbcdaac_nm
			sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
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

			nretval = sqlconobj.dataconn([EXE],company.dbname,"SELECT defa_db,defa_cr FROM lcode WHERE entry_ty='ST'","_lcode","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
			IF nretval<0
				RETURN .F.
			ENDIF
			SELECT _lcode
			mdefa_dbac	=	EVALUATE(defa_db)
			mdefa_crac	=	EVALUATE(defa_cr)
			IF INLIST(ALLT(UPPE(main_vw.RULE)),[MODVATABLE],[REBATE]) &&Add Rebate by sandeep for TKT-6275 21/02/2011
				LOCAL plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal
				STORE 0 TO plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal

				LOCAL sq1,sq2,whedate,sq3
				sq1=" SELECT AC_MAST.AC_NAME ,amount=SUM(CASE WHEN EX_VW_ACDET.AMT_TY='DR' THEN EX_VW_ACDET.AMOUNT ELSE -EX_VW_ACDET.AMOUNT END) "
				sq2=" FROM EX_VW_ACDET INNER JOIN AC_MAST ON (AC_MAST.AC_ID=EX_VW_ACDET.AC_ID) "
*!*					whedate=" WHERE (EX_VW_ACDET."+IIF(coadditional.dbdate=.T.,'DATE','U_CLDT')+"<= '"+ALLTRIM(DTOC(main_vw.DATE))+"') AND not (EX_VW_ACDET.TRAN_CD<> "+CAST(main_vw.tran_cd AS VARCHAR(10))+" AND EX_VW_ACDET.ENTRY_TY='ST') "
				whedate=" WHERE (EX_VW_ACDET."+IIF(coadditional.dbdate=.T.,'DATE','U_CLDT')+"<= '"+ALLTRIM(DTOC(main_vw.DATE))+"' AND YEAR(EX_VW_ACDET."+IIF(coadditional.dbdate=.T.,'DATE','U_CLDT')+")> '1900') AND not (EX_VW_ACDET.TRAN_CD<> "+CAST(main_vw.tran_cd AS VARCHAR(10))+" AND EX_VW_ACDET.ENTRY_TY='ST') "  && Changed by Sachin for TKT-1487 on 24/05/2010 
				sq3=" GROUP BY AC_MAST.AC_NAME  HAVING SUM(CASE WHEN EX_VW_ACDET.AMT_TY='DR' THEN EX_VW_ACDET.AMOUNT ELSE -EX_VW_ACDET.AMOUNT END)<>0 "
				nretval = sqlconobj.dataconn([EXE],company.dbname,"SET DATEFORMAT DMY "+sq1+sq2+whedate+sq3,"CURBAL","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
				IF nretval<0
					RETURN .F.
				ENDIF

				SELECT curbal
				REPLACE ALL amount WITH 0  IN curbal  FOR ISNULL(amount)
				GO TOP
				DO WHILE !EOF()
					DO CASE
						CASE ALLTRIM(ac_name)="BALANCE WITH EXCISE PLA"
							plabal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH EXCISE RG23A"
							rg23abal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH EXCISE RG23C"
							rg23cbal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH SERVICE TAX A/C"
							sertaxbal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH CVD RG23A"
							cvdabal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH CVD RG23C"
							cvdcbal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH CESS SURCHARGE PLA"
							cplabal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH CESS SURCHARGE RG23A"
							crg23abal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH CESS SURCHARGE RG23C"
							crg23cbal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH SERVICE TAX CESS A/C"
							csertaxbal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH HCESS PLA"
							phcess=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH HCESS RG23A"
							ahcess=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH HCESS RG23C"
							chcess=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH SERVICE TAX HCESS A/C"
							shcess=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH BCD RG23A"
							bcdabal=amount
						CASE ALLTRIM(ac_name)="BALANCE WITH BCD RG23C"
							bcdcbal=amount
						OTHERWISE
					ENDCASE
					SKIP
				ENDDO

				mbal1 = IIF(plabal <=0,0,plabal)+IIF(rg23abal <=0,0,rg23abal)+IIF(rg23cbal <=0,0,rg23cbal)+IIF(sertaxbal <=0,0,sertaxbal)
				mbal2 = IIF(cplabal <=0,0,cplabal)+IIF(crg23abal <=0,0,crg23abal)+IIF(crg23cbal <=0,0,crg23cbal)+IIF(csertaxbal <=0,0,csertaxbal)
				mbal3 = IIF(phcess <=0,0,phcess)+IIF(ahcess <=0,0,ahcess)+IIF(chcess <=0,0,chcess)+IIF(shcess <=0,0,shcess)
				mbal4 = mbal1+IIF(cvdabal <=0,0,cvdabal)+IIF(cvdcbal <=0,0,cvdcbal)
				mbal5 = mbal1+mbal4+IIF(bcdabal <=0,0,bcdabal)+IIF(bcdcbal <=0,0,bcdcbal)
				IF mbal1 < main_vw.examt OR mbal2 < main_vw.u_cessamt OR mbal4 < main_vw.u_hcesamt OR mbal5<main_vw.bcdamt
					MESSAGEBOX('Entry Cannot be Saved due to InSufficient Credit '+CHR(13)+;
						'PLA Balance Rs.'+STR(plabal,12,2)+CHR(13)+;
						'RG 23-A Balance Rs.'+STR(rg23abal,12,2)+CHR(13)+;
						'RG 23-C Balance Rs.'+STR(rg23cbal,12,2)+CHR(13)+;
						'SERVICE TAX Balance Rs.'+STR(sertaxbal,12,2)+CHR(13)+;
						'CVD RG23A Balance Rs.'+STR(cvdabal,12,2)+CHR(13)+;
						'CVD RG23C Balance Rs.'+STR(cvdcbal,12,2)+CHR(13)+;
						'CESS SURCHARGE PLA Balance Rs.'+STR(cplabal,12,2)+CHR(13)+;
						'CESS SURCHARGE RG 23-A Balance Rs.'+STR(crg23abal,12,2)+CHR(13)+;
						'CESS SURCHARGE RG 23-C Balance Rs.'+STR(crg23cbal,12,2)+CHR(13)+;
						'CESS ON SERVICE TAX Balance Rs.'+STR(csertaxbal,12,2)+CHR(13)+;
						'S & H CESS SURCHARGE PLA Balance Rs.'+STR(phcess,12,2)+CHR(13)+;
						'S & H CESS SURCHARGE RG 23-A Balance Rs.'+STR(ahcess,12,2)+CHR(13)+;
						'S & H CESS SURCHARGE RG 23-C Balance Rs.'+STR(chcess,12,2)+CHR(13)+;
						'S & H CESS ON SERVICE TAX Balance Rs.'+STR(shcess,12,2)+CHR(13)+;
						'BCD RG23A Balance Rs.'+STR(bcdabal,12,2)+CHR(13)+;
						'BCD RG23C Balance Rs.'+STR(bcdcbal,12,2);
						,64,vumess)


					RELE mpac_nm,miac_nm,mcac_nm,mcpac_nm,mciac_nm,mccac_nm,mdefa_dbac,mdefa_crac,msac_nm,mcsac_nm,mcvdaac_nm,mcvdcac_nm,mhpac_nm,mhiac_nm,mhcac_nm,mhsac_nm,mbcdaac_nm,mbcdcac_nm
					RELE plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal
					RETURN .F.
				ELSE

					frmrt = .F.
					DO FORM uefrm_st_dailydebit WITH _SCREEN.ACTIVEFORM.DATASESSIONID,_SCREEN.ACTIVEFORM.addmode,_SCREEN.ACTIVEFORM.editmode,plabal,rg23abal,rg23cbal,cplabal,crg23abal,crg23cbal,sertaxbal,csertaxbal,cvdabal,cvdcbal,mbal1,mbal2,mbal3,mbal4,phcess,ahcess,chcess,shcess,bcdabal,bcdcbal TO frmrt
					IF frmrt = .F.
						RETURN .F.
					ENDIF

				ENDI
			ELSE

				SELECT main_vw
				REPLACE u_expla WITH 0,u_exrg23ii WITH 0,u_rg2amt WITH 0,;
					u_cessamtp WITH 0,u_cessamta WITH 0,u_cessamtc WITH 0,;
					u_hcesamtp WITH 0,u_hcesamta WITH 0,u_hcesamtc WITH 0,;
					u_plasr WITH '',u_rg23no WITH '',u_rg23cno WITH '',;
					serbamt WITH 0,sercamt WITH 0,serhamt WITH 0 ,;
					bcdamta WITH 0,bcdamtc WITH 0,;
					u_cvdamt WITH 0  IN main_vw &&u_cvdpay Rup 04Oct09 With 0
				mamount = 0
				SELE acdet_vw
				LOCATE FOR ac_name = mpac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = miac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mcac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mcpac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mciac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mccac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = msac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mcsac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mcvdaac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mcvdcac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF

				LOCATE FOR ac_name = mhpac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mhiac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mhcac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mhsac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF

				LOCATE FOR ac_name = mbcdaac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF
				LOCATE FOR ac_name = mbcdcac_nm
				IF FOUND()
					mamount = mamount + acdet_vw.amount
					DELETE IN acdet_vw
				ENDIF

				IF mamount # 0
					LOCATE FOR ac_name = mdefa_crac
					IF FOUND()
						REPLACE amount WITH amount + mamount
					ENDIF
				ENDIF
			ENDIF
			RELE mpac_nm,miac_nm,mcac_nm,mcpac_nm,mciac_nm,mccac_nm,mdefa_dbac,mdefa_crac,msac_nm,mcsac_nm,mcvdaac_nm,mcvdcac_nm,mhpac_nm,mhiac_nm,mhcac_nm,mhsac_nm,mbcdaac_nm,mbcdcac_nm
			IF !EMPTY(_malias3)
				SELECT &_malias3
			ENDIF
			IF BETW(_mrecno3,1,RECCOUNT())
				GO _mrecno3
			ENDIF
		ENDIF
		&&<---Rup 08Aug09

	ENDIF
ENDIF

SET STEP ON


*!*	--------------------update cldate,part-ii srno,pla srno in acdet as per main table
IF (TYPE('_screen.activeform.PCVTYPE')='C' AND USED('MAIN_VW'))
*!*		If Inlist(_Screen.ActiveForm.pcvtype,'BI','RP','RR','GI','OB','GR','HI','HR','SR','ST','VI','PT','VR','JV','P1','BP','CP','J2')&&11Sep09 -- Rupesh -- Added JV &&TKT-2647 Add 'J2'
	If Inlist(_Screen.ActiveForm.pcvtype,'BI','RP','RR','GI','OB','GR','HI','HR','SR','ST','VI','PT','VR','JV','P1','BP','CP','J1','J2','J3')&&11Sep09 -- Rupesh -- Added JV &&TKT-2647 Add 'J2' &&TKT-4123 Add J1
		*!*  Added by Shrikant  on 08Oct09 ----------- Start
		IF (_SCREEN.ACTIVEFORM.pcvtype="SR" )
			REPLACE u_gcssr WITH .T. IN main_vw
		ENDIF
		*!*  Added by Shrikant  on 08Oct09 ----------- End
*!*			IF TYPE('MAIN_VW.U_CLDT')='T' AND INLIST(_SCREEN.ACTIVEFORM.pcvtype,"OB","BB","BP","CP") &&Rup 07/10/09
*!*			IF TYPE('MAIN_VW.U_CLDT')='T' AND INLIST(_SCREEN.ACTIVEFORM.pcvtype,"OB","BB","BP","CP","J1") &&Rup 07/10/09 &&TKT-4123 Add J1
		IF TYPE('MAIN_VW.U_CLDT')='T' AND INLIST(_SCREEN.ACTIVEFORM.pcvtype,"OB","BB","BP","CP","J1","ST") &&ADDED BY SATISH PAL FOR BUG-98 DATED.-09/11/2011
			IF EMPTY(main_vw.u_cldt) OR YEAR(main_vw.u_cldt)<=1900
				REPLACE u_cldt WITH main_vw.DATE IN main_vw
			ENDIF
		ENDIF

		IF TYPE('MAIN_VW.U_CLDT')='T' AND TYPE('ACDET_VW.U_CLDT')='T'
			REPLACE ALL u_cldt WITH main_vw.u_cldt IN acdet_vw
		ENDIF
		IF TYPE('MAIN_VW.U_RG23NO')='C' AND TYPE('ACDET_VW.U_RG23NO')='C'

			REPLACE ALL u_rg23no WITH main_vw.u_rg23no IN acdet_vw
		ENDIF
		IF TYPE('MAIN_VW.U_RG23CNO')='C' AND TYPE('ACDET_VW.U_RG23CNO')='C'

			REPLACE ALL u_rg23cno WITH main_vw.u_rg23cno IN acdet_vw
		ENDIF
		IF TYPE('MAIN_VW.U_PLASR')='C' AND TYPE('ACDET_VW.U_PLASR')='C'
			REPLACE ALL u_plasr WITH main_vw.u_plasr IN acdet_vw
		ENDIF
		&& Rup--->12/08/2009
		IF TYPE('main_vw.u_deliver')='C' AND _SCREEN.ACTIVEFORM.pcvtype='ST'
			IF main_vw.cons_id>0 AND  EMPTY(main_vw.u_deliver)
				sq1= "Select top 1 ac_name from ac_mast where ac_id=?MAIN_VW.cons_id"
				nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_consid","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
				IF nretval<0
					RETURN .F.
				ELSE
					REPLACE main_vw.u_deliver WITH _consid.ac_name IN main_vw
				ENDIF
				IF USED('_consid')
					USE IN _consid
				ENDIF
			ENDIF
		ENDIF
		&&<---Rup 12/08/2009

		&&--->Shrikant s. on 10 Mar, 2010
		*!*			If Inlist(_Screen.ActiveForm.pcvtype,'PT','P1') And main_vw.Rule !='MODVATABLE'  &&Rup TKT-941 It Will create problem When only Service Tax Product is selected
		If Inlist(_Screen.ActiveForm.pcvtype,'PT','P1') And main_vw.Rule !='MODVATABLE' And ([vuexc] $ vchkprod)

			IF !EMPTY(main_vw.u_rg23no) OR !EMPTY(main_vw.u_rg23cno)
				res = MESSAGEBOX("Click 'Yes' to clear Part-2 Serial No. and Save."+CHR(13)+"Else"+CHR(13)+"Click 'No'.",4+64,vumess)
				IF res = 7
					RETURN .F.
				ENDIF
			ENDIF
			REPLACE u_rg23no WITH '' IN main_vw
			REPLACE u_rg23cno WITH '' IN main_vw
			IF TYPE('MAIN_VW.U_RG23NO')='C' AND TYPE('ACDET_VW.U_RG23NO')='C'
				REPLACE ALL u_rg23no WITH main_vw.u_rg23no IN acdet_vw
			ENDIF
			IF TYPE('MAIN_VW.U_RG23CNO')='C' AND TYPE('ACDET_VW.U_RG23CNO')='C'
				REPLACE ALL u_rg23cno WITH main_vw.u_rg23cno IN acdet_vw
			ENDIF
		ENDIF
		&&<---Shrikant s. on 10 Mar, 2010
	ENDIF
ENDIF
*!*	--------------------update cldate,part-ii srno,pla srno in acdet as per main table

&&--->TKT-941
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
	If (_Screen.ActiveForm.pcvtype='J1' ) And Type('main_vw.ser_adj')='C'
	
		UPDATE lother_vw SET tbl_nm='main_vw1' WHERE e_code='J1' AND LOWER(fld_nm)='serty' &&TKT-4006 Due to changes in uevoucher TKT-381 LMC
			
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
	ENDIF
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
				ENDSCAN
		ENDIF 	
	ENDIF 	
	&&<---TKT-794 GTA
Endif
&&<---TKT-941
&&--->Rup 22/12/2010 TKT-5345
IF (TYPE('_screen.activeform.PCVTYPE')='C' AND USED('MAIN_VW'))
	IF ( [vutds] $ vchkprod)  AND (_SCREEN.ACTIVEFORM.pcvtype='BP')
		sq1="select ac_name,typ from ac_mast where ac_id="+STR(main_vw.ac_id)
		nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_actype","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
		IF nretval<0
			RETURN .F.
		ELSE
			IF  INLIST(_actype.typ,"TDS","TDS-ECESS","TDS-HCESS","TDS-SUR")
				IF EMPTY(main_vw.U_BSRCODE) OR (LEN(ALLTRIM(main_vw.U_BSRCODE))>0 AND LEN(ALLTRIM(main_vw.U_BSRCODE))!=7)
					MESSAGEBOX("BSR Code length should be 7" ,64,vumess)
					RETURN .f.
				ENDIF
				IF EMPTY(main_vw.U_CHALNO)
					MESSAGEBOX("Challan No. Cannot be blank" ,64,vumess)
					RETURN .f.
				ENDIF  			  			
				IF EMPTY(main_vw.U_CHALDT) OR (main_vw.U_CHALDT<main_vw.date)
					MESSAGEBOX("Challan Date Cannot be less then Transaction Date" ,64,vumess)
					RETURN .f.
				ENDIF  			  			
				IF EMPTY(main_vw.cheq_no)
					MESSAGEBOX("Cheque No Cannot be blank" ,64,vumess)
					RETURN .f.
				ENDIF  			  			
				
			ENDIF
			
		ENDIF
	ENDIF 
ENDIF 
&&<---Rup 22/12/2010 TKT-5345

&&Rup 19/01/2011 TKT-5692 TCS--->
If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
	IF _Screen.ActiveForm.TcsPage=.T. AND (inlist(_Screen.ActiveForm.Behave,'BR','CR') AND (main_vw.tdspaytype=2) )
		IF EMPTY(main_vw.svc_cate)
			RETURN .t. 
		ENDIF 
		SELECT dcmast_vw
		select * from dcmast_vw where LOWER(pert_name) in ('tds_tp','sc_tp','ec_tp','hc_tp') INTO CURSOR tdc1 NOFILTER  READWRITE 
		SELECT tdc1
			SCAN 
				vfld_nm="main_vw."+ALLTRIM(tdc1.fld_nm)
				vcrac_name=tdc1.crac_name
				vdac_name='"TCS Adjustment A/C"'
				vamount  =EVALUATE(vfld_nm)
				IF vamount<=0
					LOOP 
				ENDIF   
					vac_id=0
					vacname =_Screen.ActiveForm.FindAcct(vcrac_name)
					vac_id=_Screen.ActiveForm.ac_id
					vamt_ty="CR"
					IF vac_id>0
						_Screen.ActiveForm.AddToEffect(vamount,vamt_ty,vacname,vac_id,0)		
					ENDIF 
					
					
					sq1="select ac_name,tds_accoun from ac_mast where ac_id="+Str(vac_id)
					nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_acnm1","nHandle",_Screen.ActiveForm.DataSessionId)
					vac_id=0
					If nretval<0
						Return .F.
					ELSE
						IF !empty(_acnm1.tds_accoun)
							vacname =_Screen.ActiveForm.FindAcct('"'+ALLTRIM(_acnm1.tds_accoun)+'"')
							vac_id=_Screen.ActiveForm.ac_id
						ENDIF 
					ENDIF
					IF vac_id>0
	 					vamt_ty="DR"
						_Screen.ActiveForm.AddToEffect(vamount,vamt_ty,vacname,vac_id,0)		
					ENDIF 
				SELECT tdc1
			ENDSCAN 
	ENDIF 
ENDIF 	
&&<---Rup 19/01/2011 TKT-5692 TCS


