***-->Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.
_curvouobj = _SCREEN.ACTIVEFORM
*!*	If Main_vw.Entry_ty="PT" And [vuexc] $ vchkprod And !Empty(Main_vw.U_RG23CNO)

IF INLIST(main_vw.entry_ty,"PT","P1") AND ([vuexc] $ vchkprod OR [vuser] $ vchkprod) AND !EMPTY(main_vw.u_rg23cno) &&Rup 13Sep09 &&vasant081009
	_mrecno=RECCOUNT()
	_uet_alias = ALIAS()

	SELECT main_vw
	mexamt = ROUND((main_vw.examt * coadditional.cgoodsper)/100,0)
	mcessamt = ROUND((main_vw.u_cessamt * coadditional.cgoodsper)/100,0)
	mcvdamt = ROUND((main_vw.u_cvdamt * coadditional.cgoodsper)/100,0)
	mhcessamt = ROUND((main_vw.u_hcesamt * coadditional.cgoodsper)/100,0)
	mbcdamt = ROUND((main_vw.bcdamt * coadditional.cgoodsper)/100,0)

	&&vasant081009
	mexamt     = IIF(mexamt > main_vw.examt-mexamt,mexamt,main_vw.examt-mexamt)
	mcessamt   = IIF(mcessamt > main_vw.u_cessamt-mcessamt,mcessamt,main_vw.u_cessamt-mcessamt)
	mcvdamt    = IIF(mcvdamt > main_vw.u_cvdamt-mcvdamt,mcvdamt,main_vw.u_cvdamt-mcvdamt)
	mhcessamt  = IIF(mhcessamt > main_vw.u_hcesamt-mhcessamt,mhcessamt,main_vw.u_hcesamt-mhcessamt)
	mbcdamt    = IIF(mbcdamt > main_vw.bcdamt-mbcdamt,mbcdamt,main_vw.bcdamt-mbcdamt)
	&&vasant081009

	REPLACE u_rg23cpay WITH mexamt,u_rgcespay WITH mcessamt,u_cvdpay WITH mcvdamt,u_hcespay WITH mhcessamt,bcdpay WITH mbcdamt IN main_vw
	REPLACE examt WITH examt-mexamt,;
		u_cessamt WITH u_cessamt-mcessamt,;
		u_cvdamt WITH u_cvdamt-mcvdamt,;
		u_hcesamt WITH u_hcesamt-mhcessamt;
		bcdamt WITH bcdamt-mbcdamt IN main_vw
	REPLACE tot_examt WITH main_vw.tot_examt - (mexamt+mcessamt+mcvdamt+mhcessamt+mbcdamt),;
		tot_add WITH main_vw.tot_add + (mexamt+mcessamt+mcvdamt+mhcessamt+mbcdamt) IN main_vw

	IF _curvouobj.accountpage = .T.
		SELECT acdet_vw
		IF SEEK('BALANCE WITH EXCISE RG23C','AcDet_vw','Ac_name')
			REPLACE amount WITH amount - mexamt IN acdet_vw
			_curvouobj.addposting(mexamt,'EXCISE CAPITAL GOODS PAYABLE A/C','DR')
		ENDIF
		IF SEEK('BALANCE WITH CESS SURCHARGE RG23C','AcDet_vw','Ac_name')
			REPLACE amount WITH amount - mcessamt IN acdet_vw
			_curvouobj.addposting(mcessamt,'CESS CAPITAL GOODS PAYABLE A/C','DR')
		ENDIF
		IF SEEK('BALANCE WITH CVD RG23C','AcDet_vw','Ac_name')
			REPLACE amount WITH amount - mcvdamt IN acdet_vw
			_curvouobj.addposting(mcvdamt,'CVD CAPITAL GOODS PAYABLE A/C','DR')
		ENDIF
		IF SEEK('BALANCE WITH HCESS RG23C','AcDet_vw','Ac_name')
			REPLACE amount WITH amount - mhcessamt IN acdet_vw
			_curvouobj.addposting(mhcessamt,'H CESS CAPITAL GOODS PAYABLE A/C','DR')
		ENDIF
		IF SEEK('BALANCE WITH BCD RG23C','AcDet_vw','Ac_name')
			REPLACE amount WITH amount - mbcdamt IN acdet_vw
			_curvouobj.addposting(mbcdamt,'BCD CAPITAL GOODS PAYABLE A/C','DR')
		ENDIF
	ENDIF

	IF !EMPTY(_uet_alias)
		SELECT (_uet_alias)
	ENDIF
	IF BETW(_mrecno,1,RECCOUNT())
		GO _mrecno
	ENDIF
ENDIF
***<---Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.


