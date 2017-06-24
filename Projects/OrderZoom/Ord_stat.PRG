LPARAMETERS SessionId,salepur
SET DATASESSION TO (SessionId)
SqlObj = NEWOBJECT('sqlconobj','sqlconnudobj','sqlconnection',xApps)
=MessageWindow('Main Cursor Creation')
=CursorCreation()					&& Main Cursor Creation

PUBLIC m.order,m.dcmr,m.sales
m.order = 0
m.dcmr  = 0
m.sales = 0

lcstr_two = STRTRAN(lcstr_two,"BETW(inv_sr,m.sinvsr,m.einvsr)","BETW(lmain.inv_sr,m.sinvsr,m.einvsr)")

DO FORM ordstat WITH salepur

=MessageWindow('Processing Report Cursor ...')

CREA CURS tmpitem (entry_ty c(2), DATE d, doc_no c(LEN(litem.doc_no)),;
	item_no c(LEN(litem.item_no)),ITEM c(LEN(litem.ITEM)), qty N(12,4),rate N(10,2),;
	dept c(20), cate c(20), party_nm c(LEN(litem.party_nm)),;
	inv_no c(15),inv_sr c(20),l_yn c(LEN(litem.l_yn)),;
	dentry_ty c(2), ddate d, ddoc_no c(LEN(litem.doc_no)),;
	ditem_no c(LEN(litem.item_no)),d_qty N(12,4),t_qty N(12,4),;
	dinv_no c(15),dinv_sr c(20),dl_yn  c(LEN(litem.l_yn)),;
	bentry_ty c(2), bdate d, bdoc_no c(LEN(litem.doc_no)),;
	binv_no c(15),binv_sr c(20),bl_yn c(LEN(litem.l_yn)),;
	bitem_no c(LEN(litem.item_no)),b_qty N(12,4))
INDEX ON entry_ty + inv_sr + inv_no + l_yn + item_no + dentry_ty + dinv_sr + dinv_no + dl_yn + ditem_no TAG Treiili   && Harsha 26/10/2005
SET ORDER TO
IF salepur = 'P'
	mseek = 'AR'
ELSE
	mseek = 'DC'
ENDIF
=MessageWindow('Processing Report ...')
ifcondn = lcstr_one+lcstr_two

IF UPPE(ALLT(RIGH(lcstr_two,4))) = 'AND'
	ifcondn = lcstr_one+LEFT(lcstr_two,LEN(lcstr_two)-4)
ENDIF

SELE lmain
SET ORDE TO edd

SELE it_mast
SET ORDE TO it_name

SELE ac_mast
SET ORDE TO ac_name

SELE it_ref
SET RELA TO entry_ty+DTOS(DATE)+doc_no+item_no INTO litem_a

SELE litem
SET RELA TO entry_ty+DTOS(DATE)+doc_no INTO lmain
SET RELA TO ITEM INTO it_mast ADDI
SET RELA TO party_nm INTO ac_mast ADDI
IF m.order = 1
	SELE litem
	SET ORDE TO edd
	SEEK salepur+'O'
	DO WHILE entry_ty = salepur+'O' AND !EOF()
		IF !EMPTY(ifcondn)
			IF !(&ifcondn)
				SKIP
				LOOP
			ENDIF
		ENDIF
		SCAT MEMV
		SELE tmpitem
		APPEND BLANK IN tmpitem
		GATH MEMV
		REPL dentry_ty WITH '',ddate WITH {},ddoc_no WITH '',;
			ditem_no WITH '',d_qty WITH 0,t_qty WITH 0,;
			dinv_no WITH '',dinv_sr WITH '',dl_yn WITH '',;					&&
		bentry_ty WITH '',bdate WITH {},bdoc_no WITH '',;
			bitem_no WITH '',b_qty WITH 0,;
			binv_no WITH '',binv_sr WITH '',bl_yn WITH '' IN tmpitem		&&

		SELE it_ref
		SEEK m.entry_ty + m.inv_sr + m.inv_no + m.l_yn + m.item_no
		IF FOUND()
			DO WHILE rentry_ty = m.entry_ty AND rinv_sr=m.inv_sr AND rinv_no=m.inv_no AND rl_yn=m.l_yn AND ritem_no = m.item_no AND !EOF()
				_minv_sr = litem_a.inv_sr
				_minv_no = litem_a.inv_no
				_ml_yn   = litem_a.l_yn
				IF INLI(it_ref.entry_ty,'P ')
					IF m.sales = 1
						SELE tmpitem
						IF !EMPTY(dinv_no) OR !EMPTY(binv_no)
							APPEND BLANK IN tmpitem
							GATH MEMV
						ENDIF
						REPL bentry_ty WITH it_ref.entry_ty,bdate WITH it_ref.DATE,;
							bdoc_no WITH it_ref.doc_no,bitem_no WITH it_ref.item_no,;
							b_qty WITH it_ref.rqty,;
							binv_no WITH _minv_no,binv_sr WITH _minv_sr,bl_yn WITH _ml_yn,;
							dentry_ty WITH '',ddate WITH {},ddoc_no WITH '',;
							ditem_no WITH '',d_qty WITH 0,t_qty WITH 0,;
							dinv_no WITH '',dinv_sr WITH '',dl_yn WITH '' IN tmpitem
					ENDIF
				ELSE
					IF m.dcmr = 1
						SELE tmpitem
						IF !EMPTY(dinv_no) OR !EMPTY(binv_no)
							APPEND BLANK IN tmpitem
							GATH MEMV
						ENDIF
						REPL dentry_ty WITH it_ref.entry_ty,ddate WITH it_ref.DATE,;
							ddoc_no WITH it_ref.doc_no,ditem_no WITH it_ref.item_no,;
							dinv_no WITH _minv_no,dinv_sr WITH _minv_sr,dl_yn WITH _ml_yn,;
							d_qty WITH IIF(it_ref.entry_ty='TR',0,it_ref.rqty),;
							t_qty WITH IIF(it_ref.entry_ty='TR',it_ref.rqty,0),;
							bentry_ty WITH '',bdate WITH {},bdoc_no WITH '',;
							bitem_no WITH '',b_qty WITH 0,;
							binv_no WITH '',binv_sr WITH '',bl_yn WITH '' IN tmpitem
					ENDIF
				ENDIF
				SELE it_ref
				IF !EOF()
					SKIP
				ENDIF
			ENDDO
		ENDIF
		SELE litem
		IF !EOF()
			SKIP
		ENDIF
	ENDDO
ENDIF

SELE tmpitem
SET ORDER TO Treiili
IF m.sales = 1
	IF USED('tmpitem1')
		SELE tmpitem1
		USE IN tmpitem1
	ENDIF
	SELE * FROM tmpitem INTO CURSOR tmpitem1 WHERE d_qty > 0
	SELE tmpitem1
	LOCA
	DO WHILE !EOF()
		SCAT MEMV MEMO
		m.d_qty = 0
		m.t_qty = 0
		SELE it_ref
		SEEK m.dentry_ty + m.dinv_sr + m.dinv_no + m.dl_yn + m.ditem_no
		IF FOUND()
			DO WHILE rentry_ty = m.dentry_ty AND rinv_sr=m.dinv_sr AND rinv_no=m.dinv_no AND rl_yn=m.dl_yn AND ritem_no = m.ditem_no AND !EOF()
				_minv_sr = litem_a.inv_sr
				_minv_no = litem_a.inv_no
				_ml_yn   = litem_a.l_yn
				IF INLI(it_ref.entry_ty,'P ')
					IF m.sales = 1
						SELE tmpitem
						SEEK m.entry_ty + m.inv_sr + m.inv_no + m.l_yn + m.item_no + m.dentry_ty + m.dinv_sr + m.dinv_no + m.dl_yn + m.ditem_no
						IF !FOUND() OR !EMPTY(binv_no)
							APPEND BLANK IN tmpitem
							GATH MEMV
						ENDIF
						REPL bentry_ty WITH it_ref.entry_ty,bdate WITH it_ref.DATE,;
							bdoc_no WITH it_ref.doc_no,bitem_no WITH it_ref.item_no,;
							b_qty WITH it_ref.rqty,;
							binv_no WITH _minv_no,binv_sr WITH _minv_sr,bl_yn WITH _ml_yn IN tmpitem
					ENDIF
				ENDIF
				SELE it_ref
				IF !EOF()
					SKIP
				ENDIF
			ENDDO
		ENDIF
		SELE tmpitem1
		IF !EOF()
			SKIP
		ENDIF
	ENDDO
	IF USED('tmpitem1')
		SELE tmpitem1
		USE IN tmpitem1
	ENDIF
ENDIF

IF m.sales=1
	SELE litem
	SEEK PADR(salepur,2,' ')
	DO WHILE entry_ty == PADR(salepur,2,' ') AND !EOF()
		IF !EMPTY(ifcondn)
			IF !(&ifcondn)
				SKIP
				LOOP
			ENDIF
		ENDIF
		SCATT MEMVAR
		SELE it_ref
		IF !SEEK(m.entry_ty + DTOS(m.date) + m.doc_no + m.itserial,'it_ref','eddi')
			SELE tmpitem
			APPEND BLANK IN tmpitem
			GATH MEMV
		ENDIF
		SELE litem
		SKIP
	ENDDO
ENDIF

IF USED('litem_a')
	SELE litem_a
	USE IN litem_a
ENDIF

SELE it_ref
SET RELA TO

SELE litem
SET ORDE TO
SET RELA TO
SET ORDE TO eddi

SELE tmpitem
SET ORDER TO TAG Treiili
SET RELA TO entry_ty + DTOS(DATE) + doc_no INTO lmain
SET RELA TO entry_ty + DTOS(DATE) + doc_no + item_no INTO litem ADDI
SET RELA TO ITEM INTO it_mast ADDI
GO TOP

lcstr_one = ''
lcstr_two = ''
usefor = ''



FUNCTION CursorCreation
***********************
StrVw_Itref = "Select * From ORDSTAT_VW_ITREF"							&& Collecting ItRef Record
sql_con=THIS.sqlconobj.dataconn("EXE",Company.DbName,StrVw_Itref,"_Itref","This.Parent.nHandle",THIS.SessionId)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .F.
ENDIF

StrVw_Itref = "Select * From ORDSTAT_VW_MAIN"							&& Collecting LMain Record
sql_con=THIS.sqlconobj.dataconn("EXE",Company.DbName,StrVw_Itref,"_LMain","This.Parent.nHandle",THIS.SessionId)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .F.
ENDIF

StrVw_Itref = "Select * From ORDSTAT_VW_ITEM"							&& Collecting LItem Record
sql_con=THIS.sqlconobj.dataconn("EXE",Company.DbName,StrVw_Itref,"_LItem","This.Parent.nHandle",THIS.SessionId)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .F.
ENDIF

SELE _lmain
INDEX ON entry_ty+DTOS(DATE)+doc_no TAG edd

SELE _litem
INDEX ON entry_ty+DTOS(DATE)+doc_no+itserial TAG eddi
INDEX ON entry_ty+DTOS(DATE)+doc_no TAG edd
SET ORDER TO edd

SELE _itref
INDEX ON entry_ty+DTOS(DATE)+doc_no+itserial TAG REIILI

IF !USED('litem_a')
	SELE 0
	USE litem AGAIN ALIAS litem_a
ENDIF
SELE litem_a
SET ORDE TO eddi  && Harsha


FUNCTION MessageWindow
**********************
LPARA MMess
WAIT WINDOW MMess AT WROWS()/2,WCOLS()/2.5 NOWAIT
ENDFUNC
