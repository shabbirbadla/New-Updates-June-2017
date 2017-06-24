*:*****************************************************************************
*:        Program: STK_VAL.PRG
*:         System: Udyog Software
*:         Author: RUPESH PRAJAPATI
*:  Last modified: 15/09/2006
*:			AIM  : Stock valuation Procedure
*:*****************************************************************************
PARAMETERS vDataSessionId,valmethod,ShowNullMess
IF TYPE('ShowNullMess') = 'L'
	ShowNullMess = 1				&& Default Value
ENDIF

SET DATASESSION TO vDataSessionId
LOCAL nilbal,sqlconobj
nHandle=0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)

nilbal = IIF(ShowNullMess = 1 ,6,7)
IF ShowNullMess = 1
	nilbal= MESSAGEBOX("Items's Having NIL Balance  Should Appear ?",36,vumess)
ENDIF

sdate=_tmpvar.sdate
edate=_tmpvar.edate

CREATE CURSOR fifoc1 ;
	(DATE DATE ,tran_cd INT ,inv_no CHARACTER(10),entry_ty CHARACTER(2),PMKEY CHARACTER(1),it_code INT ,qty Numeric(11,4),rate Numeric(11,4),gro_amt Numeric(11,4),it_name CHARACTER(60),RATEPER Numeric(11,4),MGRO_AMT Numeric(11,4),MNET_AMT Numeric(11,4),pmV Numeric(11,4),PMI Numeric(11,4),TOTPMV Numeric(11,4),FRATE Numeric(15,4),ware_nm CHARACTER(60),balqty Numeric(11,4))
CREATE CURSOR stkval ;
	(it_code INT ,it_name CHARACTER(60),opbal Numeric(11,4) NULL,opamt Numeric(11,4) NULL, rqty Numeric(11,4) NULL,ramt Numeric(11,4) NULL,iqty Numeric(11,4) NULL,iamt Numeric(11,4) NULL,clbal Numeric(11,4) NULL,clamt Numeric(11,4) NULL)
CREATE CURSOR stkvalc ;
	(it_code INT ,it_name CHARACTER(60),opbal Numeric(11,4) NULL,opamt Numeric(11,4) NULL, rqty Numeric(11,4) NULL,ramt Numeric(11,4) NULL,iqty Numeric(11,4) NULL,iamt Numeric(11,4) NULL,clbal Numeric(11,4) NULL,clamt Numeric(11,4) NULL,DATE DATE NULL,dept VARCHAR(20) NULL,cate VARCHAR(20) NULL,ware_nm VARCHAR(60) NULL)

SET MESSAGE TO 'Checking Manufact Entry'
sq1="select * from Manufact"
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"manufact","nHandle",vDataSessionId)
IF nRetval<0
	RETURN .F.
ENDIF

SET MESSAGE TO 'Checking Entry Types...'
sq1="SELECT DISTINCT BHENT=(CASE WHEN BCODE_NM IS NOT NULL AND BCODE_NM<>' ' THEN BCODE_NM ELSE (CASE WHEN EXT_VOU=0 THEN ENTRY_TY ELSE ' ' END) END)  FROM LCODE WHERE (V_ITEM<>0 ) ORDER BY BHENT	" &&AND INV_STK<>' '
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"BHENT","nHandle",vDataSessionId)
IF nRetval<0
	=MESSAGEBOX("Lcode table error"+CHR(13)+PROPER(MESSAGE()),48,vumess)
	nRetval=sqlconobj.sqlconnclose(nHandle)
	IF nRetval<0
		=MESSAGEBOX("SQL disconnect error"+CHR(13)+PROPER(MESSAGE()),48,vumess)
	ENDIF
	RETURN .F.
ENDIF

sq1=" select distinct a.entry_ty,a.fld_nm,a.att_file,a_s=(case when (a.code='D' OR a.code='F') then '-' else '+' end),a.stkval,a.wefstkfrom,a.wefstkto,tax_name=space(20),"
SQ11=" bhent=(CASE WHEN BCODE_NM IS NOT NULL AND BCODE_NM<>' ' THEN BCODE_NM ELSE (CASE WHEN EXT_VOU=0 THEN b.ENTRY_TY ELSE ' ' END) END) from dcmast a inner join lcode b on (A.entry_ty=B.entry_ty) where a.stkval<>0 "
sq2=" UNION "
sq3=" select distinct entry_ty=space(2),fld_nm='taxamt   ',att_file=1,a_s='+',stkval,wefstkfrom,wefstkto,tax_name,bhent='~~' from stax_mas where stkval<>0 "
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1+SQ11+sq2+sq3,"DCSTMAST1","nHandle",vDataSessionId)
IF nRetval<0
	=MESSAGEBOX("Lcode table error"+CHR(13)+PROPER(MESSAGE()),48,vumess)
	nRetval=sqlconobj.sqlconnclose(nHandle)
	IF nRetval<0
		=MESSAGEBOX("SQL disconnect error"+CHR(13)+PROPER(MESSAGE()),48,vumess)
	ENDIF
	RETURN .F.
ENDIF

SELECT dcstmast1
=AFIELDS(brarray,'dcstmast1')
CREATE CURSOR dcstmast FROM ARRAY brarray
INSERT INTO dcstmast (entry_ty,fld_nm,att_file,a_s,stkval,wefstkfrom,wefstkto,tax_name,bhent);
	SELECT entry_ty,fld_nm,att_file,a_s,stkval,wefstkfrom,wefstkto,tax_name,bhent  FROM dcstmast1

ALTER TABLE dcstmast ALTER wefstkfrom d
ALTER TABLE dcstmast ALTER wefstkto d

LOCAL i,vbhent
i=0

vstdate=SET("Date")
whedate=" and (case when d.date is null then a.date else d.date end) <= '"+ALLTRIM(DTOC(_tmpvar.edate))+"' "
whitem=" and (b.it_name between '"+_tmpvar.sitem+"' and '"+_tmpvar.eitem+"')"
whdept=" and (c.dept between '"+_tmpvar.sdept+"' and '"+_tmpvar.edept+"')"
whcat =" and (c.cate  between '"+_tmpvar.SCAT +"' and '"+_tmpvar.ecat +"')"
whware=" and (a.ware_nm  between '"+_tmpvar.sware +"' and '"+_tmpvar.eware +"')"

whdept=' '
whcat=' '
whware=' '

SELECT bhent
GO TOP
DO WHILE !EOF()
	i=i+1
	vbhent=bhent.bhent
	SET MESSAGE TO 'Checking '+ ALLTRIM(vbhent)+' Entry'

	CREATE CURSOR fldcur (itquery MEMO,vouquery MEMO)
	SELECT fldcur
	INSERT BLAN
	REPLACE itquery WITH " "
	REPLACE vouquery WITH " "
	sq1=" select date=(case when (d.date is NOT null and a.entry_ty in ('PT','ST')) then d.date else a.date end),A.TRAN_CD,A.INV_NO,A.ENTRY_TY,A.PMKEY,A.IT_CODE,QTY=(case when a.entry_ty in ('AR','DC') then a.qty-a.re_qty else a.qty end),A.RATE,A.GRO_AMT,"
	sq2="B.IT_NAME,B.RATEPER,MGRO_AMT=C.GRO_AMT,MNET_AMT=C.NET_AMT,A.WARE_NM,FRATE=00000000000.000,pmV=00000000000.000  "

	SELECT dcstmast
	GO TOP
	dcstfld=' '
	DO WHILE !EOF()
		dcstfld =' '
		dcstfldv=' '
		IF 	att_file=0  AND !EOF()
			IF dcstmast.bhent=vbhent
				dcstfld=dcstmast.a_s+"(case when a."+fld_nm+" is not null and a.date between '"+ALLTRIM(DTOC(dcstmast.wefstkfrom))+"' and '"+ALLTRIM(DTOC(dcstmast.wefstkto))+"' then a."+fld_nm+" else 0 end)"
				SELECT fldcur
				REPLACE itquery  WITH ALLTRIM(itquery )+ALLTRIM(dcstfld)
			ENDIF
		ELSE
			IF dcstmast.bhent='~~' AND !EOF() &&sales tax
				dcstfldv=dcstmast.a_s+"(case when c.taxamt is not null and c.date between '"+ALLTRIM(DTOC(dcstmast.wefstkfrom))+"' and '"+ALLTRIM(DTOC(dcstmast.wefstkto))+"' and c.tax_name='"+ALLTRIM(dcstmast.tax_name)+"' then c.taxamt else 0 end)"
			ENDIF
			IF (dcstmast.bhent=vbhent) AND dcstmast.bhent<>'~~' AND !EOF()
				dcstfldv=dcstmast.a_s+"(case when c."+fld_nm+" is not null and c.date between '"+ALLTRIM(DTOC(dcstmast.wefstkfrom))+"' and '"+ALLTRIM(DTOC(dcstmast.wefstkto))+"' then c."+fld_nm+" else 0 end)"
			ENDIF
			SELECT fldcur
			REPLACE vouquery WITH ALLTRIM(vouquery )+ALLTRIM(dcstfldv)
		ENDIF
		SELECT dcstmast
		SKIP
	ENDDO
	SELECT fldcur
	nRetval = sqlconobj.dataconn([EXE],company.dbname,"SET DATEFORMAT DMY "+sq1+sq2+",PMI=000000000.00"+ALLTRIM(fldcur.itquery)+",TOTPMV=000000000.00"+ALLTRIM(fldcur.vouquery)+" from "+ALLTRIM(vbhent)+"item a inner join "+ALLTRIM(vbhent)+"main c ON(a.tran_cd=c.tran_cd) inner join it_mast b ON(a.it_code=b.it_code) LEFT JOIN "+ALLTRIM(vbhent)+"ITREF d ON (A.TRAN_CD=d.TRAN_CD AND A.ENTRY_TY=d.ENTRY_TY) where a.pmkey<>' ' "+whedate+whitem+whdept+whcat+whware,"fifo1","nHandle",vDataSessionId)
	IF nRetval<0
		=MESSAGEBOX(vbhent+" entry error"+CHR(13)+PROPER(MESSAGE()),48,vumess)
		nRetval=sqlconobj.sqlconnclose(nHandle)
		IF nRetval<0
			=MESSAGEBOX(vbhent+" entry error."+CHR(13)+PROPER(MESSAGE()),48,vumess)
		ENDIF
		RETURN .F.
	ENDIF
	INSERT INTO fifoc1 (DATE,tran_cd,inv_no,entry_ty,PMKEY,it_code,qty,rate,gro_amt,it_name,RATEPER,MGRO_AMT,MNET_AMT,pmV,PMI,TOTPMV,FRATE,ware_nm);
		SELECT DATE,tran_cd,inv_no,entry_ty,PMKEY,it_code,qty,rate,gro_amt,it_name,RATEPER,MGRO_AMT,MNET_AMT,pmV,PMI,TOTPMV,FRATE,ware_nm  FROM fifo1
	SELECT bhent
	SKIP
ENDDO

IF nHandle > 0
	nRetval=sqlconobj.sqlconnclose(nHandle)
ENDIF

ALTER TABLE fifoc1 ALTER DATE d
UPDATE  fifoc1 SET pmV=(TOTPMV*gro_amt)/IIF(MGRO_AMT=0,1,MGRO_AMT)  WHERE PMKEY='+'
UPDATE  fifoc1 SET FRATE=(((IIF(qty*rate=0,0.0,qty*rate))/RATEPER)+PMI+pmV)/IIF(qty=0,1,qty)   WHERE PMKEY='+'

SELECT fifoc1
vset=SET("Safety")
SET SAFETY OFF

SELECT it_code FROM fifoc1 GROUP BY it_code HAVING SUM(IIF(PMKEY='+',qty,-qty))<>0 INTO CURSOR fifobal

SELECT fifoc1
=AFIELDS(brarray,'fifoc1')
CREATE CURSOR fifoc FROM ARRAY brarray

SELECT fifoc1
=AFIELDS(brarray,'fifoc1')
CREATE CURSOR fifoc2 FROM ARRAY brarray

IF USED("_lstITselected")
	INSERT INTO fifoc2 (DATE,tran_cd,inv_no,entry_ty,PMKEY,it_code,qty,rate,gro_amt,it_name,RATEPER,MGRO_AMT,MNET_AMT,pmV,PMI,TOTPMV,FRATE,ware_nm);
		SELECT A.DATE,A.tran_cd,A.inv_no,A.entry_ty,A.PMKEY,A.it_code,A.qty,A.rate,A.gro_amt,A.it_name,A.RATEPER,A.MGRO_AMT,A.MNET_AMT,A.pmV,A.PMI,A.TOTPMV,A.FRATE,A.ware_nm FROM fifoc1 A INNER JOIN _lstITselected B ON (A.it_name=B.ITNAME)
ELSE
	INSERT INTO fifoc2 (DATE,tran_cd,inv_no,entry_ty,PMKEY,it_code,qty,rate,gro_amt,it_name,RATEPER,MGRO_AMT,MNET_AMT,pmV,PMI,TOTPMV,FRATE,ware_nm);
		SELECT A.DATE,A.tran_cd,A.inv_no,A.entry_ty,A.PMKEY,A.it_code,A.qty,A.rate,A.gro_amt,A.it_name,A.RATEPER,A.MGRO_AMT,A.MNET_AMT,A.pmV,A.PMI,A.TOTPMV,A.FRATE,A.ware_nm  FROM fifoc1 A
ENDIF

IF nilbal=7
	INSERT INTO fifoc (DATE,tran_cd,inv_no,entry_ty,PMKEY,it_code,qty,rate,gro_amt,it_name,RATEPER,MGRO_AMT,MNET_AMT,pmV,PMI,TOTPMV,FRATE,ware_nm);
		SELECT A.DATE,A.tran_cd,A.inv_no,A.entry_ty,A.PMKEY,A.it_code,A.qty,A.rate,A.gro_amt,A.it_name,A.RATEPER,A.MGRO_AMT,A.MNET_AMT,A.pmV,A.PMI,A.TOTPMV,A.FRATE,A.ware_nm  FROM fifoc2 A INNER JOIN fifobal B ON( A.it_code=B.it_code)
ELSE
	INSERT INTO fifoc (DATE,tran_cd,inv_no,entry_ty,PMKEY,it_code,qty,rate,gro_amt,it_name,RATEPER,MGRO_AMT,MNET_AMT,pmV,PMI,TOTPMV,FRATE,ware_nm);
		SELECT A.DATE,A.tran_cd,A.inv_no,A.entry_ty,A.PMKEY,A.it_code,A.qty,A.rate,A.gro_amt,A.it_name,A.RATEPER,A.MGRO_AMT,A.MNET_AMT,A.pmV,A.PMI,A.TOTPMV,A.FRATE,A.ware_nm  FROM fifoc2 A
ENDIF
*!*	MsgBtn = MESSAGEBOX('STEP ON',4+32,vumess)
*!*	IF MsgBtn = 6
*!*		SET STEP ON
*!*	ENDIF
IF valmethod="AVG"
	SELECT it_code,it_name,SUM(qty*IIF(ISNULL(FRATE),0,FRATE))/SUM(IIF(qty>0,qty,1)) AS avgrate FROM fifoc GROUP BY it_code,it_name  WHERE PMKEY='+' AND FRATE>0 INTO CURSOR stkavg
	SELECT it_code,it_name;
		,SUM(IIF(IIF(entry_ty='OS',DATE<=sdate,DATE<sdate),IIF(PMKEY='+',qty,-qty),0)) AS opbal;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='+',qty,0)) AS rqty ;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='-',qty,0)) AS iqty ;
		,SUM(IIF(IIF(entry_ty='OS',DATE<=sdate,DATE<sdate),IIF(PMKEY='+',qty,-qty),0))  +  SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='+',qty,0)) -   SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='-',qty,0)) AS clbal;
		FROM fifoc GROUP BY it_code,it_name INTO CURSOR stkval1

	INSERT INTO stkvalc (it_code,it_name,opbal,opamt,rqty,ramt,iqty,iamt,clbal,clamt);
		SELECT A.it_code,A.it_name;
		,A.opbal,A.opbal*(IIF(!ISNULL(B.avgrate),B.avgrate,0)) AS opamt;
		,A.rqty,A.rqty*(IIF(!ISNULL(B.avgrate),B.avgrate,0)) AS ramt;
		,A.iqty,A.iqty*(IIF(!ISNULL(B.avgrate),B.avgrate,0)) AS iamt;
		,A.clbal,IIF(A.clbal*(IIF(!ISNULL(B.avgrate),B.avgrate,0))<0,0,A.clbal*(IIF(!ISNULL(B.avgrate),B.avgrate,0))) AS clamt;
		FROM stkval1 A LEFT JOIN  stkavg B ON (A.it_code=B.it_code)
ELSE
	SELECT fifoc
	DO CASE
	CASE valmethod = "FIFO"
		INDEX ON ware_nm+STR(it_code)+IIF(PMKEY='-','A','B')+DTOS(DATE) TAG  wipd
	CASE valmethod = "LIFO"
		INDEX ON ware_nm+STR(it_code)+IIF(PMKEY='-','B','A')+DTOS(DATE) TAG  wipd  DESC
	ENDCASE
	IBALQTY1=0
	IBALQTY2=0
	mit_code=it_code
	mware_nm=ware_nm
	GO TOP
	DO WHILE !EOF()
		IF 	mit_code=it_code AND mware_nm=ware_nm
		ELSE
			mit_code=it_code
			mware_nm=ware_nm
			IBALQTY1=0
			IBALQTY2=0
		ENDIF

		IF PMKEY='-'
			IF IIF(entry_ty='OS',DATE<=sdate,DATE<DATE)
				IBALQTY1=IBALQTY1+qty
			ELSE
				IBALQTY2=IBALQTY2+qty
			ENDIF
		ELSE
			IF IIF(entry_ty='OS',DATE<=sdate,DATE<DATE)
				REPLACE balqty WITH IIF(qty-IBALQTY1>0,qty-IBALQTY1,0)
				IF IBALQTY1-qty>0
					IBALQTY1=IBALQTY1-qty
				ELSE
					IBALQTY1=0
				ENDIF
			ELSE
				REPLACE balqty WITH IIF(qty-IBALQTY2>0,qty-IBALQTY2,0)
				IF IBALQTY2-qty>0
					IBALQTY2=IBALQTY2-qty
				ELSE
					IBALQTY2=0
				ENDIF
			ENDIF
		ENDIF
		SKIP
	ENDDO
	SELECT fifoc

	INSERT INTO stkvalc (it_code,it_name,opbal,opamt,rqty,ramt,iqty,iamt,clamt,clbal);
		SELECT it_code,it_name,;
		SUM(IIF(IIF(entry_ty='OS',DATE<=sdate,DATE<sdate),IIF(PMKEY='+',qty,-qty),0)) AS opbal;
		,SUM(IIF(IIF(entry_ty='OS',DATE<=sdate,DATE<sdate),balqty*FRATE,0)) AS opamt;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='+',qty,0)) AS rqty ;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='+',qty*FRATE,0)) AS ramt ;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='-',qty,0)) AS iqty ;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate ,(balqty-qty)*FRATE*(-1),0)) AS iamt ;
		,SUM(IIF(entry_ty<>'OS'AND DATE>=sdate,balqty*FRATE,0)) AS clamt ;
		,SUM(IIF(IIF(entry_ty='OS',DATE<=sdate,DATE<sdate),IIF(PMKEY='+',qty,-qty),0))  +   SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='+',qty,0)) -  SUM(IIF(entry_ty<>'OS'AND DATE>=sdate AND PMKEY='-',qty,0))  AS clbal;
		FROM fifoc GROUP BY it_code,it_name  && INTO CURSOR stkvalc
ENDIF
SELECT stkvalc

SET SAFETY &vset
SET DATE &vstdate

=CLOSETBLS()

SELECT stkvalc
INDEX ON ware_nm+it_name TAG wit


FUNCTION CLOSETBLS
******************
IF USED("fldcur")
	USE IN fldcur
ENDIF
IF USED("fifo1")
	USE IN fifo1
ENDIF
IF USED("fifoc1")
	USE IN fifoc1
ENDIF
IF USED("fifoc2")
	USE IN fifoc2
ENDIF
IF USED("fifoc")
	USE IN fifoc
ENDIF
IF USED("stkval")
	USE IN stkval
ENDIF
IF USED("stkval1")
	USE IN stkval1
ENDIF
IF USED("dcstmast")
	USE IN dcstmast
ENDIF
IF USED("dcstmast1")
	USE IN dcstmast1
ENDIF
IF USED("chkcur")
	USE IN chkcur
ENDIF
IF USED("dropcur")
	USE IN dropcur
ENDIF
