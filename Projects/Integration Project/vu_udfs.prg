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
	=messagebox("Update Files In Multiple Company .......!",64,Vumess)
	retu
endif
sele co_mast
set orde to co_name
=afields(xxx)
crea curs cotemp from array xxx
sele cotemp
inde on alltrim(co_name)+alltrim(str(year(sta_dt)))+alltrim(str(year(end_dt))) tag co_yr		&&Manohar
appe from co_mast for !empty(multi_date)
***********
public exe_cute
exe_cute = .t.
do multcomp
if exe_cute												&&Manohar
	sele co_mast
	set orde to co_name
	=afields(xxx)
	crea curs cotemp from array xxx
	sele cotemp
	appe from co_mast for !empty(co_mast.multi_date)
	publ zcomp_name
	zcomp_name = ""
	sele cotemp
	do while !eof()
		if len(zcomp_name) > 1
			zcomp_name = zcomp_name + ", "
		endif
		zcomp_name = zcomp_name + allt(co_name)
		skip
	enddo
	curdir = sys(5) + sys(2003)
	wait wind "Setting Environment..." nowa
	notfound = .f.
	on error notfound = .t.
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
	if used("acmast_fr")
		sele acmast_fr
		use
	endif
	if used("acmast_to")
		sele acmast_to
		use
	endif
	if used("it_rate")
		sele it_rate
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
	if used("lac_indx")
		sele lac_indx
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
	if used("stax_mas")
		sele stax_mas
		use
	endif
	if used("mainall")
		sele mainall
		use
	endif
	if used("r_status")
		sele r_status
		use
	endif
	set defa to &multidir
	xfile = multidir + "LMAIN"
	use &xfile alias lmain in 0
	if notfound = .t.
		=messagebox ("Somebody is Using MULTI COMPANY DATA, Try Again",64,vumess)
		retu
	endif
	xfile = multidir + "LITEM"
	use &xfile alias litem in 0
	xfile = multidir + "LAC_DET"
	use &xfile alias lac_det in 0
	xfile = multidir + "AC_MAST"
	use &xfile alias ac_mast in 0
	xfile = multidir + "IT_mast"
	use &xfile alias it_mast in 0
	xfile = multidir + "MAINALL"
	use &xfile alias mainall in 0
	xfile = multidir + "LCODE"
	use &xfile alias lcode in 0
	xfile = multidir + "LDCW"
	use &xfile alias ldcw in 0
	use stax_mas alias stax_mas in 0   && because we need this for VUREPORT
	xfile = multidir + "R_STATUS"
	use &xfile alias r_status in 0
	sele r_status
	set orde to tag group
	if notfound = .t.
		=messagebox("Somebody is Using MULTI COMPANY DATA, Try Again",64,vumess)
		retu 0
	endif
	lcreport = lcmain + '\report'
	lcclass = lcmain + '\class' && tushar 05-09-05
	set path to &multidir;&lcpath;&lcmain;&lcreport;&lcclass && tushar 05-09-05
endif													&&Manohar
retu
*************************************************************************
proc stbalchk
priv xit_name,xqty
xit_name = litem.item
sele litem
xrec = 0
if !eof()
	xrec = recno()
endif
seek xit_name
do while not eof() and item<=eitem and date<=edate
	xit_name=litem->item
	xqty=0
	do while not eof() and item=xit_name and date<sdate
		if (inlist(litem.entry_ty,'SO','PO','P ','S ','SQ')) .and. litem.dc_no#' '
		else
			if inlist(litem.entry_ty,'P ','AR','OP','SR','B ','ES','IR','OP')
				xqty=xqty+qty
			endif
			if inlist(litem.entry_ty,'S ','DC','IP','PR','SS','II','IP')
				xqty=xqty-qty
			endif
		endif
		skip
	enddo
*	IF xqty#0 OR (DATE<=edate AND DESC=xit_name)
*		IF DESC#xit_name
*			SKIP -1
*		ENDIF
*		RETU xqty
*	ENDIF
	if xrec>0
		goto xrec
	endif
	return xqty
enddo
retu 0
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
open data invoice shared 					&&Manohar 28/08/2004 For Multicompany Reports
wait clea

*--------------------------------------------------------
proc locate                   &&  Locate Proc for Masters

if type("tbrDesktop") = "O"
	tbrdesktop.setall("Enabled",.f.)
endif

wait wind "Enter to Select" nowait

on key label enter keyboard '{CTRL+W}'
brow noedit noappend nodelete preference abc
on key label enter
wait clear
if type("tbrDesktop") = "O"
	tbrdesktop.setall("Enabled",.t.)
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
	messagebox('FRX file (Report file) does not exist!',48,vumess)
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
if file("_VouPrinting.fxp")
	do _vouprinting with "BEFORE"
endif
&&vasant 21/04/2005
if mkey=[V]
	keyboard'{ctrl+f10}'
	repo form &mdorepo for lmain.entry_ty = ent and lmain.date = dt and lmain.doc_no = doc prev environmen  &&to print promp
endif
if mkey=[P]
	repo form &mdorepo for lmain.entry_ty = ent and lmain.date = dt and lmain.doc_no = doc to print promp environment noconsole
endif
&&vasant 21/04/2005
if file("_VouPrinting.fxp")
	do _vouprinting with "AFTER"
endif
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
	fromzoom=.t.
endif
return fromzoom
*--------------------------------------------------------------
proc tmp_crea
para _dbf
ctr = 1
do whil .t.
	r_temp = sys(3) + '.' + _dbf
	ctr = ctr + 1
	if file(dix_temp)
		loop
	endif
	exit
enddo
retu r_temp

*------------------------------
******-- USING IN VOUCHER FORMS
function vou_change1 && returns form name according to voucher code
para cd
lname=""
do case
case cd="P "
	lname="sales"
case cd="S "
	lname="sales"
case cd="SR"
	lname="sal_ret"
case cd="PR"
	lname="sal_ret"
case cd="SO"
	lname="sal_ord"
case cd="PO"
	lname="sal_ord"
case cd="AR"
	lname="del_mat"
case cd="DC"
	lname="del_mat"
case cd="CR"
	lname="cash_rp"
case cd="CP"
	lname="cash_rp"
case cd="BR"
	lname="bank_rp"
case cd="BP"
	lname="bank_rp"
case cd="JV"
	lname="journal"
case cd="IP"
	lname="io_prod"
case cd="OP"
	lname="io_prod"
case cd="DN"
	lname="db_note"
case cd="CN"
	lname="db_note"
case cd="PC"
	lname="petty"
case cd="IR"
	lname="dept_ir"
case cd="II"
	lname="dept_ir"
case cd="ES"
	lname="es_stock"
case cd="B "
	lname="op_bal"
case cd="SS"
	lname="es_stock"
case cd="SQ"
	lname="quot"
case cd="EP"
	lname="exp_pur"
endcase
return lname
endfunc

*---------------------------------------------
******-- USING IN VOUCHER FORMS
function vou_change2 && returns form caption according to voucher code
para cd
lcap=""
do case
case cd="P "
	lcap="Purchase"
case cd="S "
	lcap="Sales"
case cd="SR"
	lcap="Sales Return"
case cd="PR"
	lcap="Purchase Return"
case cd="SO"
	lcap="Sales Order"
case cd="PO"
	lcap="Purchase Order"
case cd="AR"
	lcap="Goods Receipt"
case cd="DC"
	lcap="Delivery Challan"
case cd="CR"
	lcap="Cash Receipt"
case cd="CP"
	lcap="Cash Payment"
case cd="BR"
	lcap="Bank Receipt"
case cd="BP"
	lcap="Bank Payment"
case cd="JV"
	lcap="Journal Voucher"
case cd="Input To Production"
	lcap="io_prod"
case cd="OP"
	lcap="Output From Production"
case cd="DN"
	lcap="Debit Note"
case cd="CN"
	lcap="Debit Note"
case cd="PC"
	lcap="Petty Cash"
case cd="IR"
	lcap="Inter-Dept Receipt"
case cd="II"
	lcap="Inter-Dept Issue"
case cd="ES"
	lcap="Excess Stock"
case cd="B "
	lcap="Opening Balance"
case cd="SS"
	lcap="Shortage Stock"
case cd="SQ"
	lcap="Quotation"
case cd="EP"
	lcap="Expense Purchase"
endcase
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
para mcheck
d=1
f=len(mcheck)
repl=""
rep=0
do whil f > 0
	r=subs(mcheck,d,1)
	change = asc(r)+rep
	if change>255
		wait wind str(change)
	endi
	two = chr(change)
	repl=repl+two
	d=d+01
	rep=rep+1
	f=f-1
endd
retu repl

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
proc chkreord
para mtype,mitem,mqty,edd
if !empty(edd)
	edd_chk = "edd # (entry_ty+DTOS(date)+doc_no)"
else
	edd_chk = "!eof()"
endif
exceed=.f.
sele it_mast
set orde to it_name
if seek(mitem)
	if !empty(it_mast.reorder)
		sele litem
		set order to tag item_date
		set near on
		seek(mitem)
		mbal_qty = 0
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
			vuexceed=.t.
		endif
	endif
endif
retu (vuexceed)

*-----------------------------------------To check for negative item balance
proc chknegbal
para mtype,mitem,mqty,mbaldate,edd
if !empty(edd)
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
do while item==mitem and date<=mbaldate and !eof()
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


proc addinsmenu
if popup("AddInsPop")
	deact popup addinspop
	release popup addinspop
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
local lnbar,doit,m
*!*		Store com_menu.urights To cLevel
*!*		Store decoder(cLevel,.T.) To cLevel
*!*		ctr = Len(cLevel)

set filter to addins.enabled
if !eof()
	define pad addinspad of _msysmenu prompt "\<Add Ins" ;
		color scheme 3 key alt+a, ""
	on pad addinspad of _msysmenu activate popup addinspop
	define popup addinspop margin relative shadow color scheme 4
	for lnbar = cntbar("AddInsPop") to 1 step -1
		release bar getbar("AddInsPop", lnbar) of addinspop
	endfor
	sele addins
	go top
	scan
		if cntbar("AddInsPop") = 0 or ;
				getbar("AddInsPop", cntbar("AddInsPop")) < 0
			lnbar = cntbar("AddInsPop") + 1
		else
			lnbar = getbar("AddInsPop", cntbar("AddInsPop")) + 1
		endif
		define bar lnbar of addinspop prompt proper(addins.prompt)
		doit=addins.action
		on selection bar lnbar of addinspop &doit
	endscan
else
	if popup("AddInsPop")
		deact popup addinspop
		release popup addinspop
	endif
endif
sele addins
set filter to
return

proc selereport
para selrepo

local rpt
rpt = substr(selrepo,1,len(selrepo)-4)+"*.FRX"
reportcount = adir(reportarray,rpt)

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
		done=.t.
	endif
	skip
endd
if done
	checklock=.t.
	messagebox("This Voucher/Period Is Locked",0+48,vumess)
	sele (_alias)
endif
sele (_alias)
retu

proc chkprod
buffer=[]
if !empty(allt(company.passroute))
	for x = 1 to len(allt(company.passroute))
		buffer = buffer + chr(asc(substr(company.passroute,x,1))/2)
	next x
endif
vchkprod=buffer
retur

&& Vasant 20/11/2005
proc checkingmodulerights
if uppe(allt(musername))==[MANAGER]
	addbutton1    = .t.
	editbutton1   = .t.
	deletebutton1 = .t.
	printbutton1  = .t.
else
	addbutton1    = .f.
	editbutton1   = .f.
	deletebutton1 = .f.
	printbutton1  = .f.
	m.vctr		  = 0
	if empty(m.vvprompt)
		m.a=prompt()
		m.a=strtran(m.a,[ ],[])
		m.a=strtran(m.a,[&],[])
		m.a=strtran(m.a,[-],[])
		m.a=strtran(m.a,[\],[])
		m.a=strtran(m.a,[<],[])
		m.a=strtran(m.a,[(],[])
		m.a=strtran(m.a,[)],[])
		do case
		case uppe(allt(m.a))==[ISSUE]
			m.a=[WORKINPROGRESSWIP]+[ISSUE]
		case uppe(allt(m.a))==[RECEIPT]
			m.a=[WORKINPROGRESSWIP]+[RECEIPT]
		case uppe(allt(m.a))==[ISSUINGTOSELFANNEXUREIV]
			m.a=[LABOURJOB]+[ISSUINGTOSELFANNEXUREIV]
		case uppe(allt(m.a))==[RECEIVINGFROMSELFANNEXUREIV]
			m.a=[LABOURJOB]+[RECEIVINGFROMSELFANNEXUREIV]
		case uppe(allt(m.a))==[ISSUINGTOPARTYANNEXUREV]
			m.a=[LABOURJOB]+[ISSUINGTOPARTYANNEXUREV]
		case uppe(allt(m.a))==[RECEIVINGFROMPARTYANNEXUREV]
			m.a=[LABOURJOB]+[RECEIVINGFROMPARTYANNEXUREV]
		case uppe(allt(m.a))==[APAYMENTDUTYCREDIT]
			m.a=[RG23PARTII]+[APAYMENTDUTYCREDIT]
		case uppe(allt(m.a))==[ARECEIPTDUTYDEBIT]
			m.a=[RG23PARTII]+[ARECEIPTDUTYDEBIT]
		case uppe(allt(m.a))==[CPAYMENTDUTYCREDIT]
			m.a=[RG23PARTII]+[CPAYMENTDUTYCREDIT]
		case uppe(allt(m.a))==[CRECEIPTDUTYDEBIT]
			m.a=[RG23PARTII]+[CRECEIPTDUTYDEBIT]
		case uppe(allt(m.a))==[CESSPAYMENTDUTYCREDIT]
			m.a=[PLAPERSONALLEDGERACCOUNT]+[CESSPAYMENTDUTYCREDIT]
		case uppe(allt(m.a))==[SPECIALDUTYPAYMENTCREDIT]
			m.a=[PLAPERSONALLEDGERACCOUNT]+[SPECIALDUTYPAYMENTCREDIT]
		case uppe(allt(m.a))==[RECEIPTDUTYDEBIT]
			m.a=[PLAPERSONALLEDGERACCOUNT]+[RECEIPTDUTYDEBIT]
		case uppe(allt(m.a))==[CESSRECEIPTDUTYDEBIT]
			m.a=[PLAPERSONALLEDGERACCOUNT]+[CESSRECEIPTDUTYDEBIT]
		case uppe(allt(m.a))==[SPECIALDUTYRECEIPTDEBIT]
			m.a=[PLAPERSONALLEDGERACCOUNT]+[SPECIALDUTYRECEIPTDEBIT]
		case uppe(allt(m.a))==[PAYMENTDUTYCREDIT]
			m.a=[PLAPERSONALLEDGERACCOUNT]+[PAYMENTDUTYCREDIT]
		case uppe(allt(m.a))==[OPENINGSTOCK]
			m.a=[OPENINGBALANCE]+[OPENINGSTOCK]
		case uppe(allt(m.a))==[PLAOPENING]
			m.a=[OPENINGBALANCE]+[PLAOPENING]
		case uppe(allt(m.a))==[RG23APARTII]
			m.a=[OPENINGBALANCE]+[RG23APARTII]
		case uppe(allt(m.a))==[RG23CPARTII]
			m.a=[OPENINGBALANCE]+[RG23CPARTII]
		case uppe(allt(m.a))==[OTHERS]
			m.a=[OPENINGBALANCE]+[OTHERS]
		case uppe(allt(m.a))==[ACCOUNTMASTER]
			m.a=[DATAENTRY]+[ACCOUNTMASTER]
		case uppe(allt(m.a))==[ITEMMASTER]
			m.a=[DATAENTRY]+[ITEMMASTER]
		case uppe(allt(m.a))==[SALESTAXMASTER]
			m.a=[DATAENTRY]+[SALESTAXMASTER]
		case uppe(allt(m.a))==[DISCOUNTCHARGESMASTER]
			m.a=[DATAENTRY]+[DISCOUNTCHARGESMASTER]
		case uppe(allt(m.a))==[EXTRADATAMASTER]
			m.a=[DATAENTRY]+[EXTRADATAMASTER]
		case uppe(allt(m.a))==[NARRATIONMASTER]
			m.a=[DATAENTRY]+[NARRATIONMASTER]
		case uppe(allt(m.a))==[PRICELISTMASTER]
			m.a=[DATAENTRY]+[PRICELISTMASTER]
		case uppe(allt(m.a))==[DEPARTMENTMASTER]
			m.a=[DATAENTRY]+[DEPARTMENTMASTER]
		case uppe(allt(m.a))==[CATEGORYMASTER]
			m.a=[DATAENTRY]+[CATEGORYMASTER]
		case uppe(allt(m.a))==[WAREHOUSEMASTER]
			m.a=[DATAENTRY]+[WAREHOUSEMASTER]
		case uppe(allt(m.a))==[INVOICESERIESMASTER]
			m.a=[DATAENTRY]+[INVOICESERIESMASTER]
		case uppe(allt(m.a))==[MASTEREXTRADATA]
			m.a=[DATAENTRY]+[MASTEREXTRADATA]
		case uppe(allt(m.a))==[QUICKRECEIPT]
			m.a=[DATAENTRY]+[QUICKRECEIPT]
		case uppe(allt(m.a))==[COMPANYMASTER]
			m.a=[DATAENTRY]+[COMPANYMASTER]
		case uppe(allt(m.a))==[CONTINUOUSPRINTING]
			m.a=[DATAENTRY]+[CONTINUOUSPRINTING]
		case uppe(allt(m.a))==[CURRENCYMASTER]
			m.a=[DATAENTRY]+[CURRENCYMASTER]
		case uppe(allt(m.a))==[EXTERNALINDEXESTAGSMASTER]
			m.a=[DATAENTRY]+[EXTERNALINDEXESTAGSMASTER]
		otherwise
			m.a=[VOUCHERMASTER]+upper(allt(m.a))
		endcase
		m.vvprompt = m.a
	endif
	if !used('com_menu')
		select 0
		use com_menu alias com_menu share again
	endif
	select com_menu
	if seek(m.vvprompt,'Com_menu','Padbar')
		m.vctr		= 1
	else
		m.vvprompt = []
		m.a=prompt()
		m.a=strtran(m.a,[ ],[])
		m.a=strtran(m.a,[&],[])
		m.a=strtran(m.a,[-],[])
		m.a=strtran(m.a,[\],[])
		m.a=strtran(m.a,[<],[])
		m.a=strtran(m.a,[(],[])
		m.a=strtran(m.a,[)],[])
		m.a=allt(uppe(m.a))
		if used('_comtest')
			select _comtest
			use in _comtest
		endif
		select padname,barname,progname from com_menu where upper(barname) = m.a into curs _comtest
		sele _comtest
		loca
		if reccount() > 0
			m.vvprompt=allt(padname)+allt(barname)
		endif
		if used('_comtest')
			select _comtest
			use in _comtest
		endif

		select com_menu
		if seek(m.vvprompt,'Com_menu','Padbar')
			m.vctr = 1
		endif
	endif
	if	m.vctr = 1
		store com_menu.urights to clevel
		store decoder(clevel,.t.) to clevel
		ctr = len(clevel)
		for ii = 1 to ctr step 16
			if allt(uppe(substr(clevel,ii,8))) == allt(uppe(musername))
				if allt(uppe(substr(clevel,ii+9,1))) = [Y]
					addbutton1    = .t.
				endif
				if allt(uppe(substr(clevel,ii+11,1))) = [Y]
					editbutton1   = .t.
				endif
				if allt(uppe(substr(clevel,ii+13,1))) = [Y]
					deletebutton1 = .t.
				endif
				if allt(uppe(substr(clevel,ii+15,1))) = [Y]
					printbutton1  = .t.
				endif
			endif
		endfor
	endif
endif
m.vvprompt=[]
if used('com_menu')
	sele com_menu
	use in com_menu
endif
return


procedure modalmenu
local oldpanel
oldpanel = statdesktop.panels(2).text
statdesktop.panels(2).text = 'Generating Menus....'
for i = 1 to _screen.formcount
	if left(upper(alltrim(_screen.forms[i].name)),5) = 'UDYOG'
		do gware.mpr with _screen.forms[i],.t.
		exit
	endif
next i
statdesktop.panels(2).text = oldpanel
return

function  busymsg
lparameters pmsg,pbusyicon,pbusyform,poldpanel
if type('pMsg') = 'L' and type('pBusyIcon') = 'L' and type('pBusyform') = 'L'
	if pmsg = .f. and pbusyicon = .f. and pbusyform = .f.
		if type('pOldpanel') = 'U' or type('pOldpanel') = 'L'
			poldpanel = ''
		endif
		_screen.activeform.mousepointer = 0
		if !empty(poldpanel)
			statdesktop.message	= poldpanel
		else
			statdesktop.message	= ''
		ENDIF
		FOR i=1 TO _screen.FormCount 
			IF ALLTRIM(UPPER(_screen.Forms(i).name)) = 'THINKPROCESS'
				_screen.Forms(i).cexit()
				EXIT 
			ENDIF 
		ENDFOR 		
	endif
else
	oldpanel = statdesktop.message
	oldmousepoint = _screen.activeform.mousepointer
	_screen.activeform.mousepointer = 13
	statdesktop.message = pmsg
	if pbusyform = .t.
		do form thinkprocess with pmsg
	endif
endif
return

PROCEDURE onencrypt
Lpara lcvariable
lcreturn = ""
For i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+CHR(ASC(SUBSTR(lcvariable,i,1))+ASC(SUBSTR(lcvariable,i,1)))
Endfor
Return lcreturn

PROCEDURE ondecrypt
Lpara lcvariable
lcreturn = ""
For i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+chr(asc(substr(lcvariable,i,1))/2)
Endfor
Return lcreturn

FUNCTION CheckRegDll
PARAMETERS lOle
LOCAL RegFind
RegOle = .t.
oldErrProc = ON('error')
ON ERROR do DLLErrorProc in vu_udfs
DECLARE long DllRegisterServer IN (lOle) alias chkDll
CLEAR DLLS chkdll 
IF TYPE('oldErrProc') = 'C'
	IF !EMPTY(oldErrProc)
		ON ERROR &oldErrProc
	ELSE
		ON ERROR 	
	ENDIF 	
ELSE
	ON ERROR 	
ENDIF 
RETURN RegOle


PROCEDURE DLLErrorProc 
lerrorno =ERROR()
RegOle = .t.
DO case
CASE lErrorno = 1753
	RegOle = .f.
ENDCASE
RETURN RegOle

Procedure onshutdown
	On Shutdown
	Clear Events
	quit
Return

procedure pctrlf4
for i = 1 to _screen.formcount
	if allt(upper(_screen.forms[i].baseclass)) != "TOOLBAR" and left(upper(alltrim(_screen.forms[i].name)),5) != 'UDYOG';
		and upper(alltrim(_screen.forms[i].name)) != 'FRMLOGINUSERS' and upper(alltrim(_screen.forms[i].name)) != 'FRMMSGWINDOW'
		statdesktop.message = [Busy Mode....]
		=beep(600,200)
		statdesktop.message = [User :]+musername
		return .f.
	endif
next i
_screen.activeform.procre(.t.)
return

procedure pctrll
ON KEY LABEL alt+l
for i = 1 to _screen.formcount
	if allt(upper(_screen.forms[i].baseclass)) != "TOOLBAR" and left(upper(alltrim(_screen.forms[i].name)),5) != 'UDYOG';
		and upper(alltrim(_screen.forms[i].name)) != 'FRMLOGINUSERS' and upper(alltrim(_screen.forms[i].name)) != 'FRMMSGWINDOW'
		statdesktop.message = [Busy Mode....]
		=beep(600,200)
		statdesktop.message = [User :]+musername
		return .f.
	endif
next i
_screen.activeform.procre(.f.)
return

procedure decoder
PARAMETERS thispass,passflag
STORE "" TO finecode,mycoder
FOR i = 1 TO LEN(thispass)
	IF !passflag
		mycoder = CHR(ASC(SUBSTR(thispass,i,1))-4) &&+7-11)
	ELSE
		mycoder = CHR(ASC(SUBSTR(thispass,i,1))+4) &&-7+11)
	ENDIF (!passflag)
	finecode = finecode+mycoder
NEXT (i)
RETURN finecode


*!*	procedure GetPropAPI
*!*	DECLARE INTEGER CloseHandle IN WIN32API INTEGER
*!*	DECLARE INTEGER GetProp IN WIN32API INTEGER, STRING @
*!*	DECLARE INTEGER GetWindow IN WIN32API INTEGER, INTEGER
*!*	DECLARE INTEGER GetDesktopWindow IN WIN32API

*!*	#DEFINE SW_RESTORE 9
*!*	#DEFINE ERROR_ALREADY_EXISTS 183
*!*	#DEFINE GW_HWNDNEXT 2
*!*	#DEFINE GW_CHILD 5

*!*	local llretval, lcexeflag, lnexehwnd, lnhwnd,oProp
*!*	oProp = 0
*!*	lcexeflag = strtran(_screen.caption, " ", "") + chr(0)
*!*	lnhwnd = GetWindow(getdesktopwindow(), gw_child)

*!*	do while lnhwnd > 0
*!*		if getprop(lnhwnd, @lcexeflag) # 0
*!*			oprop = getprop(lnhwnd, @lcexeflag)
*!*			AppSessionId = oProp
*!*			exit
*!*		endif
*!*		lnhwnd = getwindow(lnhwnd,gw_hwndnext)		
*!*	enddo	
*!*	closehandle(lnexehwnd)
*!*	llretval = oProp
*!*	clear dlls "CloseHandle","GetProp","GetWindow","GetDesktopWindow"
*!*	return llretval

*!*	procedure SetPropAPI
*!*	DECLARE INTEGER CloseHandle IN WIN32API INTEGER
*!*	DECLARE INTEGER GetProp IN WIN32API INTEGER, STRING @
*!*	DECLARE INTEGER SetProp IN WIN32API INTEGER, STRING @, INTEGER
*!*	DECLARE INTEGER GetWindow IN WIN32API INTEGER, INTEGER
*!*	DECLARE INTEGER GetDesktopWindow IN WIN32API
*!*	DECLARE LONG FindWindow IN WIN32API LONG, STRING
*!*	DECLARE INTEGER IsIconic IN WIN32API INTEGER
*!*	DECLARE INTEGER SetForegroundWindow IN WIN32API INTEGER
*!*	DECLARE INTEGER ShowWindow IN WIN32API INTEGER, INTEGER

*!*	#DEFINE SW_RESTORE 9
*!*	#DEFINE ERROR_ALREADY_EXISTS 183
*!*	#DEFINE GW_HWNDNEXT 2
*!*	#DEFINE GW_CHILD 5

*!*	local llretval, lcexeflag, lnexehwnd, lnhwnd
*!*	*!*	oProp = 0
*!*	lcexeflag = strtran(_screen.caption, " ", "") + chr(0)
*!*	lnhwnd = getwindow(getdesktopwindow(), gw_child)
*!*	do while lnhwnd > 0
*!*		if getprop(lnhwnd, @lcexeflag) # 0
*!*			oprop = getprop(lnhwnd, @lcexeflag)
*!*		endif
*!*		if getprop(lnhwnd, @lcexeflag) = iif(type('OProp')='U',1,oprop)
*!*			if isiconic(lnhwnd) > 0
*!*				showwindow(lnhwnd, sw_restore)
*!*			endif
*!*			setforegroundwindow(lnhwnd)
*!*			setprop(findwindow(0,_screen.caption), @lcexeflag, oprop + 1)
*!*			AppSessionId = oprop + 1
*!*			exit
*!*		endif
*!*		lnhwnd = getwindow(lnhwnd, gw_hwndnext)
*!*	enddo
*!*	closehandle(lnexehwnd)
*!*	llretval = oProp
*!*	clear dlls "CloseHandle","SetProp","GetProp","GetWindow","GetDesktopWindow","FindWindow","IsIconic","SetForegroundWindow","ShowWindow"
*!*	return llretval


*!*	Procedure RemovePropAPI
*!*	DECLARE INTEGER CloseHandle IN WIN32API INTEGER
*!*	DECLARE INTEGER SetProp IN WIN32API INTEGER, STRING @, INTEGER
*!*	DECLARE INTEGER GetProp IN WIN32API INTEGER, STRING @
*!*	DECLARE INTEGER RemoveProp IN WIN32API as RmPropertry INTEGER,STRING @,INTEGER
*!*	DECLARE INTEGER GetWindow IN WIN32API INTEGER, INTEGER
*!*	DECLARE INTEGER GetDesktopWindow IN WIN32API
*!*	DECLARE LONG FindWindow IN WIN32API LONG, STRING

*!*	#DEFINE SW_RESTORE 9
*!*	#DEFINE ERROR_ALREADY_EXISTS 183
*!*	#DEFINE GW_HWNDNEXT 2
*!*	#DEFINE GW_CHILD 5

*!*	local llretval, lcexeflag, lnexehwnd, lnhwnd
*!*	lcexeflag = strtran(_screen.caption, " ", "") + chr(0)
*!*	lnhwnd = GetWindow(getdesktopwindow(), gw_child)
*!*	do while lnhwnd > 0
*!*		if getprop(lnhwnd, @lcexeflag) # 0
*!*			oprop = getprop(lnhwnd, @lcexeflag)
*!*			RmPropertry(lnhWnd,@lcexeflag,oProp)			
*!*			setprop(findwindow(0, _screen.caption), @lcexeflag, oprop-1)			
*!*			AppSessionId = oProp - 1
*!*			exit
*!*		endif
*!*		lnhwnd = getwindow(lnhwnd, gw_hwndnext)		
*!*	enddo	
*!*	CloseHandle(lnexehwnd)
*!*	llretval = .f.
*!*	clear dlls "CloseHandle","SetProp","GetProp","RmProperty","GetWindow","GetDesktopWindow","FindWindow"
*!*	return llretval


*!*	Procedure AppMutex
*!*	DECLARE INTEGER CreateMutex IN WIN32API INTEGER, INTEGER, STRING @
*!*	DECLARE INTEGER CloseHandle IN WIN32API INTEGER
*!*	DECLARE INTEGER GetLastError IN WIN32API
*!*	DECLARE INTEGER SetProp IN WIN32API INTEGER, STRING @, INTEGER
*!*	DECLARE INTEGER GetProp IN WIN32API INTEGER, STRING @
*!*	DECLARE INTEGER RemoveProp IN WIN32API INTEGER, STRING @
*!*	DECLARE INTEGER IsIconic IN WIN32API INTEGER
*!*	DECLARE INTEGER SetForegroundWindow IN WIN32API INTEGER
*!*	DECLARE INTEGER GetWindow IN WIN32API INTEGER, INTEGER
*!*	DECLARE INTEGER ShowWindow IN WIN32API INTEGER, INTEGER
*!*	DECLARE INTEGER GetDesktopWindow IN WIN32API
*!*	DECLARE LONG FindWindow IN WIN32API LONG, STRING

*!*	#DEFINE SW_RESTORE 9
*!*	#DEFINE ERROR_ALREADY_EXISTS 183
*!*	#DEFINE GW_HWNDNEXT 2
*!*	#DEFINE GW_CHILD 5

*!*	local llretval, lcexeflag, lnexehwnd, lnhwnd
*!*	lcexeflag = strtran(_screen.caption, " ", "") + chr(0)
*!*	lnexehwnd = createmutex(0, 1, @lcexeflag)
*!*	if getlasterror() = ERROR_ALREADY_EXISTS
*!*		lnhwnd = getwindow(getdesktopwindow(), gw_child)
*!*		do while lnhwnd > 0
*!*			if getprop(lnhwnd, @lcexeflag) # 0
*!*				oprop = getprop(lnhwnd, @lcexeflag)
*!*			endif
*!*			if getprop(lnhwnd, @lcexeflag) = iif(type('OProp')='U',1,oprop)
*!*				if isiconic(lnhwnd) > 0
*!*					showwindow(lnhwnd, sw_restore)
*!*				endif
*!*				setforegroundwindow(lnhwnd)
*!*				setprop(findwindow(0,_screen.caption), @lcexeflag, oprop + 1)
*!*				AppSessionId = oprop + 1
*!*				exit
*!*			endif
*!*			lnhwnd = getwindow(lnhwnd, gw_hwndnext)
*!*		enddo
*!*		closehandle(lnexehwnd)
*!*		llretval = .f.
*!*	else
*!*		setprop(findwindow(0, _screen.caption), @lcexeflag,1)
*!*		AppSessionId = 1
*!*		llretval = .t.
*!*	endif
*!*	clear dlls "CreateMutex","CloseHandle","GetLastError","SetProp","GetProp","RemoveProp","RmProperty"
*!*	clear dlls "IsIconic","SetForegroundWindow","GetWindow","ShowWindow","GetDesktopWindow","FindWindow"
*!*	return llretval
*!*	endfunc


*:*****************************************************************************
*:        Program: ColFind.Prg
*:         System: Udyog Software
*:         Author: UDAY
*:  Last modified: 15/12/2006
*:			AIM  : Column Level Find Class
*:		Remarks  : This Is The Class It Find Column
*:		Usage 	 : GridFind(THIS.PARENT.PARENT.RECORDSOURCE,THIS.PARENT.CONTROLSOURCE,"PartyName",GRIDOBJECT,THIS.PARENT.NAME,"","column1")
*!*	*:*****************************************************************************
*!*	DEFINE CLASS ColumnSearch AS CUSTOM

*!*		PROCEDURE ColSearch
*!*		LPARAMETERS ltablename AS Variant,lfield AS Variant,;
*!*			lasfield AS STRING,lgrid AS OBJECT,lcol AS Variant,lkeyfld AS STRING,coltofind AS STRING

*!*		LOCAL mvalue
*!*		STORE "" TO mvalue
*!*		lkeyfld = IIF(!EMPTY(lkeyfld)," WHERE "+lkeyfld,'')
*!*		mstr = "select distinct "+lfield+" as "+lasfield+" from "+ltablename+lkeyfld+" into cursor t1"
*!*		_TALLY = 0
*!*		&mstr
*!*		IF _TALLY # 0
*!*			lasfield = IIF(EMPTY(lasfield),SUBSTR(lfield,AT(".",lfield)+1,LEN(lfield)),lasfield)
*!*			mvalue=Uegetpop("t1","FIND ",lasfield)
*!*			USE IN t1
*!*		ENDIF

*!*		IF ! EMPTY(mvalue)
*!*			grdnm  = lgrid
*!*			SELECT (ltablename)
*!*			GO TOP
*!*			FOR reccnt = RECNO() TO RECCOUNT()
*!*				GOTO RECORD reccnt
*!*				IF &lfield = mvalue
*!*					FOR i = 1 TO grdnm.COLUMNCOUNT STEP 1
*!*						DIME xyz(i,1)
*!*						xyz(i,1) = grdnm.COLUMNS(i).CONTROLSOURCE
*!*					ENDFOR
*!*					grdnm.RECORDSOURCE = (ltablename)
*!*					grdnm.RECORDSOURCETYPE = 1
*!*					FOR i = 1 TO grdnm.COLUMNCOUNT STEP 1
*!*						grdnm.COLUMNS(i).CONTROLSOURCE = xyz(i,1)
*!*					ENDFOR
*!*					RELE ARRAY xyz
*!*					grdnm.REFRESH()
*!*					IF ! EMPTY(coltofind)
*!*						grdnm.&coltofind..SETFOCUS()
*!*					ENDIF
*!*					EXIT
*!*				ENDIF
*!*			ENDFOR
*!*		ENDIF
*!*		
*!*		ENDPROC
*!*	ENDDEFINE
*!*	return 



