***-->Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.
_curvouobj = _Screen.ActiveForm
*!*	If Main_vw.Entry_ty="PT" And [vuexc] $ vchkprod And !Empty(Main_vw.U_RG23CNO)
SET STEP ON
If Inlist(main_vw.entry_ty,"PT","P1") And ([vuexc] $ vchkprod Or [vuser] $ vchkprod) And !Empty(main_vw.u_rg23cno) &&Rup 13Sep09 &&vasant081009
	_mrecno=Reccount()
	_uet_alias = Alias()
*** Added by Shrikant S. on 17/05/2010 for TKT-997 *** Start
	Local cPer,mAmount,mviewField,mtot_duty,mtot_add,mfieldName,cacname,acname
	cacname=''
	acname=''
	mviewField=''
	mfieldName=''

	Store 0 To cPer,mtot_duty,mtot_add,mAmount
	Select * From dcmast_vw Where Code="E" Order By corder Into Cursor tmpDcmast

	Select tmpDcmast
	Index On Alltrim(fld_nm) Tag fld_nm
	Do While !Eof()
		acname=Evaluate(tmpDcmast.dac_name)
		cracname=Evaluate(tmpDcmast.crac_name)
		If Seek(fld_nm,'tmpDcmast','FLD_NM')
			mviewField='main_vw.'+fld_nm
			mfieldName='main_vw.'+tmpDcmast.cfieldName
			cPer=Evaluate(Evl(cramtexpr,Str(0)))
				mAmount=IIF(Iif(cPer!=0,cPer,coadditional.cgoodsper)>=100,&mviewField ,FLOOR((&mviewField * Iif(cPer!=0,cPer,coadditional.cgoodsper))/100))		&& Added by Shrikant S. on 29/06/2010 for TKT-2725					
*!*				mAmount=IIF(Iif(cPer!=0,cPer,coadditional.cgoodsper)>=100,&mviewField ,ROUND((&mviewField * Iif(cPer!=0,cPer,coadditional.cgoodsper))/100,0))	 && Commented by Shrikant S. on 29/06/2010 for TKT-2725
*!*				mAmount=Round((&mviewField * Iif(cPer!=0,cPer,coadditional.cgoodsper))/100,0)	&& Commented by Shrikant S. on 01/06/2010
			mAmount=Iif(mAmount <= &mviewField,&mviewField-mAmount,mAmount)	
			mtot_duty=mtot_duty+mAmount
			Replace &mfieldName With mAmount In main_vw
			Replace &mviewField With &mviewField-mAmount
			If _curvouobj.accountpage = .T.
				Select acdet_vw
				If Seek(acname,'AcDet_vw','Ac_name')
					Replace amount With amount - mAmount In acdet_vw
					_curvouobj.addposting(mAmount ,cracname,'DR')
					Replace u_cldt WITH main_vw.u_cldt IN acdet_vw && Added by Shrikant S. for TKT-1476
				Endif
			Endif
			Select tmpDcmast
		Endif
		If !Eof()
			Skip
		Endif
	Enddo
	Replace tot_examt With main_vw.tot_examt - mtot_duty,;
		tot_add With main_vw.tot_add + mtot_duty In main_vw

	If Used('tmpDcmast')
		Select tmpDcmast
		Use
	Endif
*** Added by Shrikant S. on 17/05/2010 for TKT-997 *** End

*!*		SELECT main_vw
*!*		mexamt = ROUND((main_vw.examt * coadditional.cgoodsper)/100,0)
*!*		mcessamt = ROUND((main_vw.u_cessamt * coadditional.cgoodsper)/100,0)
*!*		mcvdamt = ROUND((main_vw.u_cvdamt * coadditional.cgoodsper)/100,0)
*!*		mhcessamt = ROUND((main_vw.u_hcesamt * coadditional.cgoodsper)/100,0)
*!*		mbcdamt = ROUND((main_vw.bcdamt * coadditional.cgoodsper)/100,0)

*!*		&&vasant081009
*!*		mexamt     = IIF(mexamt > main_vw.examt-mexamt,mexamt,main_vw.examt-mexamt)
*!*		mcessamt   = IIF(mcessamt > main_vw.u_cessamt-mcessamt,mcessamt,main_vw.u_cessamt-mcessamt)
*!*		mcvdamt    = IIF(mcvdamt > main_vw.u_cvdamt-mcvdamt,mcvdamt,main_vw.u_cvdamt-mcvdamt)
*!*		mhcessamt  = IIF(mhcessamt > main_vw.u_hcesamt-mhcessamt,mhcessamt,main_vw.u_hcesamt-mhcessamt)
*!*		mbcdamt    = IIF(mbcdamt > main_vw.bcdamt-mbcdamt,mbcdamt,main_vw.bcdamt-mbcdamt)
*!*		&&vasant081009

*!*		REPLACE u_rg23cpay WITH mexamt,u_rgcespay WITH mcessamt,u_cvdpay WITH mcvdamt,u_hcespay WITH mhcessamt,bcdpay WITH mbcdamt IN main_vw
*!*		REPLACE examt WITH examt-mexamt,;
*!*			u_cessamt WITH u_cessamt-mcessamt,;
*!*			u_cvdamt WITH u_cvdamt-mcvdamt,;
*!*			u_hcesamt WITH u_hcesamt-mhcessamt;
*!*			bcdamt WITH bcdamt-mbcdamt IN main_vw
*!*		REPLACE tot_examt WITH main_vw.tot_examt - (mexamt+mcessamt+mcvdamt+mhcessamt+mbcdamt),;
*!*			tot_add WITH main_vw.tot_add + (mexamt+mcessamt+mcvdamt+mhcessamt+mbcdamt) IN main_vw

*!*		IF _curvouobj.accountpage = .T.
*!*			SELECT acdet_vw
*!*			IF SEEK('BALANCE WITH EXCISE RG23C','AcDet_vw','Ac_name')
*!*				REPLACE amount WITH amount - mexamt IN acdet_vw
*!*				_curvouobj.addposting(mexamt,'EXCISE CAPITAL GOODS PAYABLE A/C','DR')
*!*			ENDIF
*!*			IF SEEK('BALANCE WITH CESS SURCHARGE RG23C','AcDet_vw','Ac_name')
*!*				REPLACE amount WITH amount - mcessamt IN acdet_vw
*!*				_curvouobj.addposting(mcessamt,'CESS CAPITAL GOODS PAYABLE A/C','DR')
*!*			ENDIF
*!*			IF SEEK('BALANCE WITH CVD RG23C','AcDet_vw','Ac_name')
*!*				REPLACE amount WITH amount - mcvdamt IN acdet_vw
*!*				_curvouobj.addposting(mcvdamt,'CVD CAPITAL GOODS PAYABLE A/C','DR')
*!*			ENDIF
*!*			IF SEEK('BALANCE WITH HCESS RG23C','AcDet_vw','Ac_name')
*!*				REPLACE amount WITH amount - mhcessamt IN acdet_vw
*!*				_curvouobj.addposting(mhcessamt,'H CESS CAPITAL GOODS PAYABLE A/C','DR')
*!*			ENDIF
*!*			IF SEEK('BALANCE WITH BCD RG23C','AcDet_vw','Ac_name')
*!*				REPLACE amount WITH amount - mbcdamt IN acdet_vw
*!*				_curvouobj.addposting(mbcdamt,'BCD CAPITAL GOODS PAYABLE A/C','DR')
*!*			ENDIF
*!*		ENDIF

	If !Empty(_uet_alias)
		Select (_uet_alias)
	Endif
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif

Endif
***<---Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.


