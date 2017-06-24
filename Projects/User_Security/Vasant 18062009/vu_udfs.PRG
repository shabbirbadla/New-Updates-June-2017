*----------------------------------------------
proc start_report   && Using for Multi Company
	local multidir
	xnotfoud = .f.
	multidir = lcmain + "\Multidir\"
	xfile    = multidir + "\LMAIN"
	use &xfile alias mlmain share again in 0
	sele mlmain
	if used("mlmain")
		sele mlmain
		use
	endif
	if xnotfoud
		=messagebox("Update Files In Multiple Company .......!",64,"Visual Udyog")
		retu
	endif
	sele co_mast
	set orde to co_name
	=afields(xxx)
	crea curs cotemp from array xxx
	sele cotemp
	Inde on ALLTRIM(co_name)+ALLTRIM(STR(YEAR(sta_dt)))+ALLTRIM(STR(YEAR(end_dt))) Tag co_yr		&&Manohar
	appe from co_mast for !empty(multi_date)
	***********
	PUBLIC exe_cute
	exe_cute = .T.
	DO multcomp
	IF exe_cute												&&Manohar
		SELE Co_Mast
		SET orde to co_name
		=afields(xxx)
		CREA curs cotemp from array xxx
		SELE cotemp
		APPE from Co_Mast for !empty(Co_Mast.multi_date)
		PUBL zcomp_name
		zcomp_name = ""
		SELE cotemp
		DO while !eof()
			IF len(zcomp_name) > 1
				zcomp_name = zcomp_name + ", "
			ENDIF
			zcomp_name = zcomp_name + allt(co_name)
			Skip
		ENDDO
		CURDIR = sys(5) + sys(2003)
		WAIT wind "Setting Environment..." nowa
		notfound = .f.
		ON error notfound = .t.
		IF used("lmain")
			SELE lmain
			USE
		ENDIF
		IF used("litem")
			SELE litem
			USE
		ENDIF
		IF used("lac_det")
			SELE lac_det
			USE
		ENDIF
		IF used("ac_mast")
			SELE ac_mast
			USE
		ENDIF
		IF used("acmast_fr")
			SELE acmast_fr
			USE
		ENDIF
		IF used("acmast_to")
			SELE acmast_to
			USE
		ENDIF
		IF used("it_rate")
			SELE it_rate
			USE
		ENDIF
		IF used("it_mast")
			SELE it_mast
			USE
		ENDIF
		IF used("it_mast_fr")
			SELE it_mast_fr
			USE
		ENDIF
		IF used("it_mast_to")
			SELE it_mast_to
			USE
		ENDIF
		IF used("lac_indx")
			SELE lac_indx
			USE
		ENDIF
		IF used("lcode")
			SELE lcode
			USE
		ENDIF
		IF used("ldcw")
			SELE ldcw
			USE
		ENDIF
		IF used("stax_mas")
			SELE stax_mas
			USE
		ENDIF
		IF used("mainall")
			SELE mainall
			USE
		ENDIF
		IF used("r_status")
			SELE r_status
			USE
		ENDIF
		SET defa to &multidir
		xfile = multidir + "LMAIN"
		USE &xfile alias lmain in 0
		IF notfound = .t.
			=messagebox ("Somebody is Using MULTI COMPANY DATA, Try Again",64,"Visual Udyog")
			RETU
		ENDIF
		xfile = multidir + "LITEM"
		USE &xfile alias litem in 0
		xfile = multidir + "LAC_DET"
		USE &xfile alias lac_det in 0
		xfile = multidir + "AC_MAST"
		USE &xfile alias ac_mast in 0
		xfile = multidir + "IT_mast"
		USE &xfile alias it_mast in 0
		xfile = multidir + "MAINALL"
		USE &xfile alias mainall in 0
		xfile = multidir + "LCODE"
		USE &xfile alias lcode in 0
		xfile = multidir + "LDCW"
		USE &xfile alias ldcw in 0
		USE stax_mas alias stax_mas in 0   && because we need this for VUREPORT
		xfile = multidir + "R_STATUS"
		USE &xfile alias r_status in 0
		SELE r_status
		SET orde to tag group
		IF notfound = .t.
			=messagebox("Somebody is Using MULTI COMPANY DATA, Try Again",64,"Visual Udyog")
			RETU 0
		ENDIF
		lcreport = lcmain + '\report'
		lcclass = lcmain + '\class' && tushar 05-09-05
		SET path to &multidir;&lcpath;&lcmain;&lcreport;&lcclass && tushar 05-09-05
	ENDIF													&&Manohar						
	RETU
	*************************************************************************
proc stbalchk
	priv xit_name,xqty
	xit_name = litem.item
	SELE litem
	xrec = 0
	IF !EOF()
		xrec = RECNO()
	ENDIF
	seek xit_name
	DO WHILE NOT EOF() AND item<=eitem AND DATE<=edate
		xit_name=litem->item
		xqty=0
		DO WHILE NOT EOF() AND item=xit_name AND DATE<sdate
			IF (INLIST(litem.entry_ty,'SO','PO','P ','S ','SQ')) .AND. litem.dc_no#' '
			ELSE
				IF INLIST(litem.entry_ty,'P ','AR','OP','SR','B ','ES','IR','OP')
					xqty=xqty+qty
				ENDIF
				IF INLIST(litem.entry_ty,'S ','DC','IP','PR','SS','II','IP')
					xqty=xqty-qty
				ENDIF
			ENDIF
			SKIP
		ENDDO
		*	IF xqty#0 OR (DATE<=edate AND DESC=xit_name)
		*		IF DESC#xit_name
		*			SKIP -1
		*		ENDIF
		*		RETU xqty
		*	ENDIF
		IF xrec>0
			GOTO xrec
		ENDIF
		RETURN xqty
	ENDDO
	RETU 0
	****************************************************************************
proc end_report

	wait wind "Resetting Environment ... " nowait
	*--------- Closing Opened Tables
	if used("lmain")
		sele lmain
		use
	endif

	if used("litem")
		sele litem
		use
	endif

	if used("lac_det")
		sele lac_det
		use
	endif

	if used("ac_mast")
		sele ac_mast
		use
	endif

	if used("ac_mast_fr")
		sele ac_mast_fr
		use
	endif
	if used("acmast_fr")
		sele acmast_fr
		use
	endif

	if used("ac_mast_to")
		sele ac_mast_to
		use
	endif

	if used("acmast_to")
		sele acmast_to
		use
	endif

	if used("it_mast")
		sele it_mast
		use
	endif

	if used("it_mast_fr")
		sele it_mast_fr
		use
	endif

	if used("it_mast_to")
		sele it_mast_to
		use
	endif

	if used("lcode")
		sele lcode
		use
	endif

	if used("ldcw")
		sele ldcw
		use
	endif

	if used("mainall")
		sele mainall
		use
	endif

	if used("stax_mas")
		sele stax_mas
		use
	endif

	if used("r_status")
		sele r_status
		use
	endif
	lcreport = lcmain + "\report"
	lcclass = lcmain + '\class' && tushar 05-09-05
	set defa to &lcpath
*!*		set path to &lcpath;&lcmain;&lcreport
	set path to &lcpath;&lcmain;&lcreport;&lcclass && Tushar 05-09-05
	Open Data Invoice Shared 					&&Manohar 28/08/2004 For Multicompany Reports
	wait clea

	*--------------------------------------------------------
proc locate                   &&  Locate Proc for Masters

	if type("tbrDesktop") = "O"
		tbrDesktop.SetAll("Enabled",.f.)
	endif

	wait wind "Enter to Select" nowait

	on key label Enter Keyboard '{CTRL+W}'
	brow noedit noappend nodelete preference abc
	on key label Enter
	wait clear
	if type("tbrDesktop") = "O"
		tbrDesktop.SetAll("Enabled",.t.)
	endif

	*--------------------------------------------------------
proc vprinting
	para type1

	lsele = sele()
	film  = filte()

	do put_rela_off
*	SET FIXED ON
*	MDECI=COMPANY.DECI
*	SET DECI TO &MDECI
	mdorepo = ''

	** excise trading
	*!*	if type1="S " and "vutex" $ vchkprod
	*!*		wait wind 'trading sale bill' nowa
	*!*		if not file("invoice1.frx") or not file("invoice2.frx") or not file("invoice3.frx") or not file("invoice4.frx")
	*!*			wait wind "trading reports not found"
	*!*			retur
	*!*		endif

	*!*					sele lmain_vw
	*!*				    ent = lmain_vw.entry_ty
	*!*					dt  = dtos(lmain_vw.date)
	*!*					doc = lmain_vw.doc_no
	*!*					exp1 = ent+dt+doc
	*!*			wait wind exp1 + lmain_vw.party_nm + [ ] + tran(lmain_vw.net_amt) nowait
	*!*				sele lmain
	*!*				set order to tag edd
	*!*				set rela to
	*!*				if not seek(exp1)
	*!*					wait wind "Transaction Not Found" nowai
	*!*					retu
	*!*				endif
	*!*
	*!*			local buf1, buf2, buf3, buf4
	*!*			buf1 = [invoice1]
	*!*			buf2 = [invoice2]
	*!*			buf3 = [invoice3]
	*!*			buf4 = [invoice4]
	*!*			_Asciicols = 300
	*!*			_Asciirows = 300
	*!*			set blocksize to 300
	*!*		 WAIT WIND "now REPORT 1"
	*!*		 sele lmain
	*!*		 set order to edd
	*!*		 keyb '{ctrl+f10}'
	*!*		repo form &buf1 for lmain.entry_ty+dtos(lmain.date)+lmain.doc_no = exp1 to file t1.txt asci   &&  print promp prev
	*!*		 WAIT WIND "now REPORT 2"
	*!*		 sele lmain
	*!*		 set order to edd
	*!*		 keyb '{ctrl+f10}'
	*!*			_Asciicols = 300
	*!*			_Asciirows = 300
	*!*			set blocksize to 300
	*!*		repo form &buf2 for lmain.entry_ty+dtos(lmain.date)+lmain.doc_no = exp1 to file t2.txt asci && to print promp prev
	*!*		 WAIT WIND "now REPORT 3"
	*!*		 sele lmain
	*!*		 set order to edd
	*!*	     keyb '{ctrl+f10}'
	*!*			_Asciicols = 300
	*!*			_Asciirows = 300
	*!*			set blocksize to 300
	*!*		repo form &buf3 for lmain.entry_ty+dtos(lmain.date)+lmain.doc_no = exp1 to file t3.txt asci && to print promp prev
	*!*		 WAIT WIND "now REPORT 4"
	*!*		 sele lmain
	*!*		 set order to edd
	*!*		 keyb '{ctrl+f10}'
	*!*			_Asciicols = 300
	*!*			_Asciirows = 300
	*!*			set blocksize to 300
	*!*		repo form &buf4 for lmain.entry_ty+dtos(lmain.date)+lmain.doc_no = exp1 to file t4.txt asci && to print promp prev
	*!*	  return
	*!*	else && not trading sale bill
** excise trading

	sele lcode
	set orde to tag cd
	seek(type1)
	mdorepo = allt(lcode.repo_nm)+".FRX"
	do selereport with allt(mdorepo)
	xxx = "'" + allt(mdorepo) + "'"
	if !file(&xxx)
		messagebox('FRX file (Report file) does not exist!',48,'Visual Udyog')
		sele (lsele)
		set filter to &film
		retu
	endif
	*endif
	*lsele = sele()
	*film  = filte()
	if !empt(film)
		set filter to
	endif
	sele lmain_vw
	ent = lmain_vw.entry_ty
	dt  = lmain_vw.date
	doc = lmain_vw.doc_no
	exp1 = ent+dtos(dt)+doc
	if used("ac_mast")
		sele ac_mast
		use
		use ac_mast alias ac_mast share again in 0
		sele ac_mast
		set orde to tag ac_name
		seek(lmain_vw.party_nm)
	endif
	sele lmain
	set order to tag edd
*	set rela to
	seek(lmain_vw.entry_ty+dtos(lmain_vw.date)+lmain_vw.doc_no)
	&&vasant 21/04/2005
	If File("_VouPrinting.fxp")		
		Do _VouPrinting with "BEFORE"
	EndIf
	&&vasant 21/04/2005	
	if mkey=[V]
		keyboard'{ctrl+f10}'
		repo form &mdorepo for lmain.entry_ty = ent and lmain.date = dt and lmain.doc_no = doc prev ENVIRONMEN  &&to print promp
	endif
	if mkey=[P]
		repo form &mdorepo for lmain.entry_ty = ent and lmain.date = dt and lmain.doc_no = doc to print promp ENVIRONMENT noconsole
	endif
	&&vasant 21/04/2005
	If File("_VouPrinting.fxp")		
		Do _VouPrinting with "AFTER"
	EndIf
	&&vasant 21/04/2005	
	if !empty(lsele)
		sele (lsele)
		set filter to &film
	endif
*	SET FIXED Off
	**--------------------------------- vprinting --------------------------

proc chkalloc
	para m_entry1,m_invsr1,m_invno1,m_lyn1,m_party1
	select mainall
	set orde to tag eiiyp
	if seek(m_entry1+m_invsr1+m_invno1+m_lyn1+m_party1)
		fromzoom=.T.
	endif
	return fromzoom
	*--------------------------------------------------------------
proc tmp_crea
	para _dbf
	ctr = 1
	do whil .t.
		r_temp = SYS(3) + '.' + _dbf
		ctr = ctr + 1
		if file(dix_temp)
			loop
		endif
		exit
	enddo
	retu r_temp

	*------------------------------
	******-- USING IN VOUCHER FORMS
Function Vou_Change1 && returns form name according to voucher code
	para cd
	lname=""
	Do Case
		Case cd="P "
			lname="sales"
		Case cd="S "
			lname="sales"
		Case cd="SR"
			lname="sal_ret"
		Case cd="PR"
			lname="sal_ret"
		Case cd="SO"
			lname="sal_ord"
		Case cd="PO"
			lname="sal_ord"
		Case cd="AR"
			lname="del_mat"
		Case cd="DC"
			lname="del_mat"
		Case cd="CR"
			lname="cash_rp"
		Case cd="CP"
			lname="cash_rp"
		Case cd="BR"
			lname="bank_rp"
		Case cd="BP"
			lname="bank_rp"
		Case cd="JV"
			lname="journal"
		Case cd="IP"
			lname="io_prod"
		Case cd="OP"
			lname="io_prod"
		Case cd="DN"
			lname="db_note"
		Case cd="CN"
			lname="db_note"
		Case cd="PC"
			lname="petty"
		Case cd="IR"
			lname="dept_ir"
		Case cd="II"
			lname="dept_ir"
		Case cd="ES"
			lname="es_stock"
		Case cd="B "
			lname="op_bal"
		Case cd="SS"
			lname="es_stock"
		Case cd="SQ"
			lname="quot"
		Case cd="EP"
			lname="exp_pur"
	EndCase
	return lname
endfunc

*---------------------------------------------
******-- USING IN VOUCHER FORMS
Function Vou_Change2 && returns form caption according to voucher code
	para cd
	lcap=""
	Do Case
		Case cd="P "
			lcap="Purchase"
		Case cd="S "
			lcap="Sales"
		Case cd="SR"
			lcap="Sales Return"
		Case cd="PR"
			lcap="Purchase Return"
		Case cd="SO"
			lcap="Sales Order"
		Case cd="PO"
			lcap="Purchase Order"
		Case cd="AR"
			lcap="Goods Receipt"
		Case cd="DC"
			lcap="Delivery Challan"
		Case cd="CR"
			lcap="Cash Receipt"
		Case cd="CP"
			lcap="Cash Payment"
		Case cd="BR"
			lcap="Bank Receipt"
		Case cd="BP"
			lcap="Bank Payment"
		Case cd="JV"
			lcap="Journal Voucher"
		Case cd="Input To Production"
			lcap="io_prod"
		Case cd="OP"
			lcap="Output From Production"
		Case cd="DN"
			lcap="Debit Note"
		Case cd="CN"
			lcap="Debit Note"
		Case cd="PC"
			lcap="Petty Cash"
		Case cd="IR"
			lcap="Inter-Dept Receipt"
		Case cd="II"
			lcap="Inter-Dept Issue"
		Case cd="ES"
			lcap="Excess Stock"
		Case cd="B "
			lcap="Opening Balance"
		Case cd="SS"
			lcap="Shortage Stock"
		Case cd="SQ"
			lcap="Quotation"
		Case cd="EP"
			lcap="Expense Purchase"
	EndCase
	return lcap
endfunc

*----------------------------------
proc set_dele
	if set('deleted') = 'OFF'
		set deleted on
	else
		set deleted off
	endif
	brow nowa
	return

	*----------- A/c Balance checking

	*!*	proc onacbalchk
	*!*	para acname,baldate,waitwin

	*!*	if !company.ac_bchk
	*!*		retu 0
	*!*	endif

	*!*	lsele = sele()


	*!*	if !used("lac_det")
	*!*		use lac_det in 0
	*!*	endif
	*!*
	*!*	sele lac_det
	*!*	set filter to
	*!*	balamt = 0
	*!*	lrec = 0
	*!*	if !eof()
	*!*		lrec = recno()
	*!*	endif
	*!*	set order to acbal
	*!*	set near on
	*!*	seek(acname+dtos(company.sta_dt))
	*!*	do while ac_name = acname and dtos(date) < dtos(baldate) and !eof()
	*!*	  	balamt = balamt + iif(amt_ty = 'DB', amount, -amount)
	*!*		skip
	*!*	enddo

	*!*	if lrec > 0
	*!*		goto lrec
	*!*	endif

	*!*	if !empty(lsele)
	*!*		sele (lsele)
	*!*	endif

	*!*	if waitwin = .t.
	*!*		wait wind 'Balance ' + str(abs(balamt),10,2) + iif(balamt <> 0,iif(balamt > 0,' CR',' DB'),'') nowai
	*!*	endif

	*!*	retu balamt


	*--------------- Item Balance Check
proc onitbalchk
	para itname,baldate

	lsele = sele()
	if !used("litem")
		use litem in 0
	endif
	sele litem
	set filter to
	lrec = 0
	if !eof()
		lrec = recno()
	endif
	balqty = 0
	do while item = itname and date < baldate and !eof()
		if (inlist(entry_ty,'SO','PO','P ','S ','SQ')) and dc_no # ' '
		else
			if inlist(entry_ty,'P ','AR','OP','SR','B ','ES','IR','OP')
				balqty = balqty + qty
			endif
			if inlist(entry_ty,'S ','DC','IP','PR','SS','II','IP')
				balqty = balqty - qty
			endif
		endif
		skip
	enddo
	if lrec > 0
		goto lrec
	endif

	if !empty(lsele)
		sele (lsele)
	endif

	retu balqty

	*---------------------------
proc enc
	PARA mcheck
	D=1
	F=LEN(mcheck)
	REPL=""
	rep=0
	DO whil F > 0
		R=SUBS(mcheck,D,1)
		CHANGE = ASC(R)+rep
		IF CHANGE>255
			WAIT WIND STR(CHANGE)
		ENDI
		two = CHR(CHANGE)
		REPL=REPL+two
		D=D+01
		rep=rep+1
		F=F-1
ENDD
RETU REPL

proc dec
	para mcheck
	d=1
	f=len(mcheck)
	repl=""
	rep=0
	do whil f > 0
		r=subs(mcheck,d,1)
		change = asc(r)-rep
		if change>0
			two = chr(change)
		endi
		repl=repl+two
		d=d+01
		f=f-1
		rep=rep+1
endd
retu repl

*---------------------------------------To check the reorder level of items online
proc chkReOrd
	para mtype,mitem,mqty,edd
	if !Empty(edd)
   	   edd_chk = "edd # (entry_ty+DTOS(date)+doc_no)"
   	else
   	   edd_chk = "!eof()"
   	endif
	exceed=.F.
	sele it_mast
	set orde to it_name
	if seek(mitem)
		if !empty(it_mast.reorder)
			sele litem
			set order to tag item_date
			set near on
			seek(mitem)
			mBAL_QTY = 0
			do whil item == mitem and !eof()
				if inlist(entry_ty,'S ','DC','PR','II','SS','IP') and &edd_chk
					mbal_qty = mbal_qty - qty
				endif
				if inlist(entry_ty,'P ','B ','AR','SR','IR','ES','OP') and &edd_chk
					mbal_qty = mbal_qty + qty
				endif
				skip
			enddo
			if inlist(mtype,'S ','DC','PR','II','SS','SQ','IP')
				mbal_qty = mbal_qty - mqty
			endif
			if inlist(mtype,'P ','B ','AR','SR','IR','ES','OP')
				mbal_qty = mbal_qty + mqty
			endif
			sele it_mast
			set orde to it_name
			if mbal_qty < it_mast.reorder
*				wait wind "Need to order this item - atleast "+alltr(str((it_mast.reorder-mbal_qty)+1))   &&JAI 22-09-2003
				wait wind "Need to order this item - atleast "+alltr(str((it_mast.reorder-mbal_qty))) && JAI 22-09-2003
				vuexceed=.T.
			endif
		endif
	endif
	retu (vuexceed)

	*-----------------------------------------To check for negative item balance
proc chkNegBal
	para mtype,mitem,mqty,mbaldate,edd
	if !Empty(edd)
   	   edd_chk = "edd # (entry_ty+DTOS(date)+doc_no)"
   	else
   	   edd_chk = "!eof()"
   	endif

	mcheck = .f.
	mbal_qty = 0
	set near on
	sele litem
	set order to tag item_date
	seek(mitem)
	Do while item==mitem and date<=mbaldate and !eof()
		if empty(dc_no) and inlist(entry_ty,'S ','DC','PR','II','SS','IP') and &edd_chk
			mbal_qty = mbal_qty-qty
		endif
*!*			if inlist(entry_ty,'P ','B ','AR','SR','IR','ES','OP') and &edd_chk
		if empty(dc_no) and inlist(entry_ty,'P ','B ','AR','SR','IR','ES','OP') and &edd_chk	&&vasant 23/4/2005
			mbal_qty = mbal_qty+qty
		endif
		skip
	endd

	if empty(dc_no) and inlist(mtype,'S ','DC','PR','II','SS','IP')
		mbal_qty = mbal_qty-mqty
	endif

*!*		if inlist(mtype,'P ','B ','AR','SR','IR','ES','OP')
	if empty(dc_no) and inlist(mtype,'P ','B ','AR','SR','IR','ES','OP')		&&vasant 23/4/2005
		mbal_qty = mbal_qty+mqty
	endif

	if mbal_qty < 0
		mnegcheck = [n]   &&do not allow
	else
		mnegcheck = [p]  &&bal_qty positive
	endif
	return


proc AddInsmenu
	if popup("AddInsPop")
		deact popup AddinsPop
		release popup AddinsPop
	endif
*	do gware.mpr &&raj  ----- jai 01-08-2003
	if used("addins")
		sele addins
		use
	endif
	if !used("addins")
		use addins alias addins share again in 0
	endif
	sele addins
	LOCAL lnBar,doit,m
*!*		Store com_menu.urights To cLevel
*!*		Store decoder(cLevel,.T.) To cLevel
*!*		ctr = Len(cLevel)

	SET FILTER TO AddIns.enabled
	if !eof()
DEFINE PAD AddInspad OF _MSYSMENU PROMPT "\<Add Ins" ;
	COLOR SCHEME 3 KEY ALT+A, ""
ON PAD AddInspad OF _MSYSMENU ACTIVATE POPUP AddInsPop
DEFINE POPUP AddInsPop MARGIN RELATIVE SHADOW COLOR SCHEME 4
FOR lnBar = CNTBAR("AddInsPop") TO 1 STEP -1
	RELEASE BAR GETBAR("AddInsPop", lnBar) OF AddInsPop
ENDFOR
sele addins
go top
SCAN
	iF CNTBAR("AddInsPop") = 0 OR ;
			GETBAR("AddInsPop", CNTBAR("AddInsPop")) < 0
		lnBar = CNTBAR("AddInsPop") + 1
	ELSE
		lnBar = GETBAR("AddInsPop", CNTBAR("AddInsPop")) + 1
	ENDIF
DEFINE BAR lnBar OF AddInsPop PROMPT Proper(AddIns.prompt)
doit=AddIns.Action
oN SELECTION BAR lnBar OF AddInsPop &doit
ENDSCAN
else
	if popup("AddInsPop")
		deact popup AddinsPop
		release popup AddinsPop
	endif
endif
sele addins
SET FILTER TO
return

proc selereport
para selrepo

	local rpt
	rpt = substr(selrepo,1,len(selrepo)-4)+"*.FRX"
	reportcount = adir(reportarray,RPT)

	if reportcount>1

		decl rparray[reportcount]
		for i=1 to reportcount
			rparray(i)=reportarray(i,1)
		endfor
		moc=rparray(1)
	
		define window rep from 15,30 to 26,60 colo sche 4
		activate wind rep
		@1,1 get moc from rparray pict [@^T] size 1,20
		keyb '{spacebar}'
		read modal		&&vasant		26/10/2005
		deac wind rep
		rele wind rep

		if !empty(moc)
			mdorepo =allt(moc)
		else
			retur
		endif
	else
		mdorepo =  selrepo
	endif
	retu

proc put_rela_off
	if used("lmain")
		sele lmain
		set rela to
	endif
	if used("litem")
		sele litem
		set rela to
	endif
	if used("lac_det")
		sele lac_det
		set rela to
	endif
	if used("mainall")
		sele mainall
		set rela to
	endif
	if used("ac_mast")
		sele ac_mast
		set rela to
	endif
	if used("it_mast")
		sele it_mast
		set rela to
	endif
retur
		*-------------------------------------------------------------------
		*----------------- VOUCHER / PERIOD lOCK ---------------------------
		*-------------------------------------------------------------------
	proc chklock
		para _entry_ty,_date,_inv_sr,_dept
		set exac on
		_alias=alias()
		rele addlock,checklock
		publ addlock,checklock
		checklock=.f.
		addlock=.f.
		if !used('yesno')
			use yesno alias yesno shar again in 0
		endif
		done=.f.
		sele yesno
		set order to tag ent
		seek(_entry_ty)
		do while entry_ty=_entry_ty and not eof()
			if  (entry_ty=_entry_ty and betw(_date,lock_from,lock_to) and empt(inv_sr) and empt(dept)) ;
					or (entry_ty=_entry_ty and betw(_date,lock_from,lock_to) and inv_sr=_inv_sr and empt(dept)) ;
					or (entry_ty=_entry_ty and betw(_date,lock_from,lock_to) and empt(inv_sr) and dept=_dept) ;
					or (entry_ty=_entry_ty and betw(_date,lock_from,lock_to) and inv_sr=_inv_sr and dept=_dept)
				if allow=.f.
					addlock=.t.
				else
					addlock=.f.
				endif
				done=.T.
			endif
			skip
		endd
		if done
			checklock=.t.
			messagebox("This Voucher/Period Is Locked",0+48,"Visual Udyog")
			sele (_alias)
		endif
		sele (_alias)
retu

PROC chkprod
	buffer=[]
	if !empty(allt(company.passroute))
		for x = 1 to len(allt(company.passroute))
			buffer = buffer + chr(asc(substr(company.passroute,x,1))/2)
		next x
	endif
	vchkProd=buffer
	retur

&& Vasant 20/11/2005
Proc CheckingModuleRights
	If Uppe(Allt(mUsername))==[MANAGER]
		AddButton1    = .t.
		EditButton1   = .t.
		DeleteButton1 = .t.
		PrintButton1  = .t.
	Else
		AddButton1    = .f.
		EditButton1   = .f.
		DeleteButton1 = .f.
		PrintButton1  = .f.
		m.vctr		  = 0
		If Empty(m.vvPrompt)
			m.a=prompt()
			m.a=strtran(m.a,[ ],[])
			m.a=strtran(m.a,[&],[])
			m.a=strtran(m.a,[-],[])
			m.a=strtran(m.a,[\],[])
			m.a=strtran(m.a,[<],[])
			m.a=strtran(m.a,[(],[])
			m.a=strtran(m.a,[)],[])
			Do Case
			Case Uppe(Allt(m.a))==[ISSUE]
				m.a=[WORKINPROGRESSWIP]+[ISSUE]
			Case Uppe(Allt(m.a))==[RECEIPT]
				m.a=[WORKINPROGRESSWIP]+[RECEIPT]
			Case Uppe(Allt(m.a))==[ISSUINGTOSELFANNEXUREIV]
				m.a=[LABOURJOB]+[ISSUINGTOSELFANNEXUREIV]
			Case Uppe(Allt(m.a))==[RECEIVINGFROMSELFANNEXUREIV]
				m.a=[LABOURJOB]+[RECEIVINGFROMSELFANNEXUREIV]
			Case Uppe(Allt(m.a))==[ISSUINGTOPARTYANNEXUREV]
				m.a=[LABOURJOB]+[ISSUINGTOPARTYANNEXUREV]
			Case Uppe(Allt(m.a))==[RECEIVINGFROMPARTYANNEXUREV]
				m.a=[LABOURJOB]+[RECEIVINGFROMPARTYANNEXUREV]
			Case Uppe(Allt(m.a))==[APAYMENTDUTYCREDIT]
				m.a=[RG23PARTII]+[APAYMENTDUTYCREDIT]
			Case Uppe(Allt(m.a))==[ARECEIPTDUTYDEBIT]
				m.a=[RG23PARTII]+[ARECEIPTDUTYDEBIT]
			Case Uppe(Allt(m.a))==[CPAYMENTDUTYCREDIT]
				m.a=[RG23PARTII]+[CPAYMENTDUTYCREDIT]
			Case Uppe(Allt(m.a))==[CRECEIPTDUTYDEBIT]
				m.a=[RG23PARTII]+[CRECEIPTDUTYDEBIT]
			Case Uppe(Allt(m.a))==[CESSPAYMENTDUTYCREDIT]
				m.a=[PLAPERSONALLEDGERACCOUNT]+[CESSPAYMENTDUTYCREDIT]
			Case Uppe(Allt(m.a))==[SPECIALDUTYPAYMENTCREDIT]
				m.a=[PLAPERSONALLEDGERACCOUNT]+[SPECIALDUTYPAYMENTCREDIT]
			Case Uppe(Allt(m.a))==[RECEIPTDUTYDEBIT]
				m.a=[PLAPERSONALLEDGERACCOUNT]+[RECEIPTDUTYDEBIT]
			Case Uppe(Allt(m.a))==[CESSRECEIPTDUTYDEBIT]
				m.a=[PLAPERSONALLEDGERACCOUNT]+[CESSRECEIPTDUTYDEBIT]
			Case Uppe(Allt(m.a))==[SPECIALDUTYRECEIPTDEBIT]
				m.a=[PLAPERSONALLEDGERACCOUNT]+[SPECIALDUTYRECEIPTDEBIT]
			Case Uppe(Allt(m.a))==[PAYMENTDUTYCREDIT]
				m.a=[PLAPERSONALLEDGERACCOUNT]+[PAYMENTDUTYCREDIT]
			Case Uppe(Allt(m.a))==[OPENINGSTOCK]
				m.a=[OPENINGBALANCE]+[OPENINGSTOCK]
			Case Uppe(Allt(m.a))==[PLAOPENING]
				m.a=[OPENINGBALANCE]+[PLAOPENING]
			Case Uppe(Allt(m.a))==[RG23APARTII]
				m.a=[OPENINGBALANCE]+[RG23APARTII]
			Case Uppe(Allt(m.a))==[RG23CPARTII]
				m.a=[OPENINGBALANCE]+[RG23CPARTII]
			Case Uppe(Allt(m.a))==[OTHERS]
				m.a=[OPENINGBALANCE]+[OTHERS]
			Case Uppe(Allt(m.a))==[ACCOUNTMASTER]
				m.a=[DATAENTRY]+[ACCOUNTMASTER]
			Case Uppe(Allt(m.a))==[ITEMMASTER]
				m.a=[DATAENTRY]+[ITEMMASTER]
			Case Uppe(Allt(m.a))==[SALESTAXMASTER]
				m.a=[DATAENTRY]+[SALESTAXMASTER]
			Case Uppe(Allt(m.a))==[DISCOUNTCHARGESMASTER]
				m.a=[DATAENTRY]+[DISCOUNTCHARGESMASTER]
			Case Uppe(Allt(m.a))==[EXTRADATAMASTER]
				m.a=[DATAENTRY]+[EXTRADATAMASTER]
			Case Uppe(Allt(m.a))==[NARRATIONMASTER]
				m.a=[DATAENTRY]+[NARRATIONMASTER]
			Case Uppe(Allt(m.a))==[PRICELISTMASTER]
				m.a=[DATAENTRY]+[PRICELISTMASTER]
			Case Uppe(Allt(m.a))==[DEPARTMENTMASTER]
				m.a=[DATAENTRY]+[DEPARTMENTMASTER]
			Case Uppe(Allt(m.a))==[CATEGORYMASTER]
				m.a=[DATAENTRY]+[CATEGORYMASTER]
			Case Uppe(Allt(m.a))==[WAREHOUSEMASTER]
				m.a=[DATAENTRY]+[WAREHOUSEMASTER]
			Case Uppe(Allt(m.a))==[INVOICESERIESMASTER]
				m.a=[DATAENTRY]+[INVOICESERIESMASTER]
			Case Uppe(Allt(m.a))==[MASTEREXTRADATA]
				m.a=[DATAENTRY]+[MASTEREXTRADATA]
			Case Uppe(Allt(m.a))==[QUICKRECEIPT]
				m.a=[DATAENTRY]+[QUICKRECEIPT]
			Case Uppe(Allt(m.a))==[COMPANYMASTER]
				m.a=[DATAENTRY]+[COMPANYMASTER]
			Case Uppe(Allt(m.a))==[CONTINUOUSPRINTING]
				m.a=[DATAENTRY]+[CONTINUOUSPRINTING]
			Case Uppe(Allt(m.a))==[CURRENCYMASTER]
				m.a=[DATAENTRY]+[CURRENCYMASTER]
			Case Uppe(Allt(m.a))==[EXTERNALINDEXESTAGSMASTER]
				m.a=[DATAENTRY]+[EXTERNALINDEXESTAGSMASTER]
			OtherWise
				m.a=[VOUCHERMASTER]+Upper(Allt(m.a))
			EndCase
			m.vvprompt = m.a
		Endif	
		If !Used('com_menu')
			Select 0
			Use com_menu Alias com_menu Share Again
		Endif	
		Select Com_menu
		If Seek(m.vvPrompt,'Com_menu','Padbar')
			m.vctr		= 1
		Else
			m.vvPrompt = []
			m.a=prompt()
			m.a=strtran(m.a,[ ],[])
			m.a=strtran(m.a,[&],[])
			m.a=strtran(m.a,[-],[])
			m.a=strtran(m.a,[\],[])
			m.a=strtran(m.a,[<],[])
			m.a=strtran(m.a,[(],[])
			m.a=strtran(m.a,[)],[])
			m.a=allt(uppe(m.a))
			If Used('_comtest')
				Select _comtest
				Use In _comtest
			Endif
			Select Padname,Barname,Progname From Com_menu where upper(barname) = m.a Into Curs _comtest
			sele _comtest
			loca
			If Reccount() > 0
				m.vvPrompt=Allt(Padname)+Allt(Barname)
			Endif	
			If Used('_comtest')
				Select _comtest
				Use In _comtest
			Endif

			Select Com_menu
			If Seek(m.vvPrompt,'Com_menu','Padbar')
				m.vctr = 1
			Endif			
		Endif
		If	m.vctr = 1
			Store com_menu.urights To cLevel
			Store decoder(cLevel,.T.) To cLevel
			ctr = Len(cLevel)
			For ii = 1 To ctr Step 16	
			    If Allt(Uppe(SubStr(cLevel,ii,8))) == Allt(Uppe(mUsername))
					If Allt(Uppe(SubStr(cLevel,ii+9,1))) = [Y]
						AddButton1    = .t.
					EndIf
					If Allt(Uppe(SubStr(cLevel,ii+11,1))) = [Y]
						EditButton1   = .t.
					EndIf
					If Allt(Uppe(SubStr(cLevel,ii+13,1))) = [Y]
						DeleteButton1 = .t.
					EndIf
					If Allt(Uppe(SubStr(cLevel,ii+15,1))) = [Y]
						PrintButton1  = .t.
					EndIf
				EndIf	
			EndFor
		Endif
	Endif
	m.vvPrompt=[]
	If Used('com_menu')
		Sele com_menu
		Use in com_menu
	EndIf	
	Return


PROCEDURE modalmenu
LOCAL Oldpanel
oldPanel = statDesktop.panels(2).text
statDesktop.panels(2).text = 'Generating Menus....'
for i = 1 to _screen.formcount
	if LEFT(UPPER(ALLTRIM(_screen.forms[i].name)),5) = 'UDYOG'
		do gware.mpr with _screen.forms[i],.t.
		exit
	endif
next i 
statDesktop.panels(2).text = Oldpanel
RETURN

FUNCTION  busyMsg
LPARAMETERS pMsg,pBusyIcon,pBusyForm,pOldPanel
IF TYPE('pMsg') = 'L' AND TYPE('pBusyIcon') = 'L' AND TYPE('pBusyform') = 'L'
	IF pMsg = .f. AND pBusyIcon = .f. AND pBusyform = .f.
		***		987034339
		IF TYPE('pOldpanel') = 'U' OR TYPE('pOldpanel') = 'L'
			pOldpanel = ''
		ENDIF
			
		_screen.activeform.MousePointer = 0
		IF !EMPTY(pOldpanel)
			statDesktop.panels(2).text	= pOldpanel
		ELSE
			statDesktop.panels(2).text	= ''
		ENDIF 	
		statDesktop.panels(3).visible = .f.
		IF UPPER(ALLTRIM(_screen.activeform.name)) = 'THINKPROCESS'
			_screen.activeform.release
		ENDIF
	ENDIF 	
ELSE 
	Oldpanel = statDesktop.panels(2).text
	oldMousePoint = _screen.activeform.MousePointer
	_screen.ActiveForm.mousePointer = 13	
	statdesktop.panels(2).text = pMsg
	IF pBusyIcon = .t.
		statDesktop.panels(3).visible = .t.
	ENDIF 
	IF pBusyForm = .t.
		DO FORM thinkprocess WITH pMsg
	ENDIF 
ENDIF 
RETURN
