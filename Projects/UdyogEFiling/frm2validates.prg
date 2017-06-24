
PROCEDURE BeforeRawXMLUpdates
_errmessage = ''
DO case
case mProduct = 'VU8'	
	IF !USED('rg23d')
		_errmessage = 'Unable to find output table'
	Endif
case mProduct = 'VU9'	
	IF !USED('lst_exc')
		_errmessage = 'Unable to find output table'
	Endif
endcase
IF !EMPTY(_errmessage)
	RETURN _errmessage
Endif	

DO case
case mProduct = 'VU8'	
	SELECT rg23d
	REPLACE ALL csrno WITH 2 IN rg23d

	*Changes has been done by vasant on 23/04/2013 as per Bug 12948 (Changes needed in DE Tool regarding generation of XML for FORM-2). 
	SELECT * FROM rg23d WHERE Entry_ty != 'IR' INTO CURSOR _lst_exc1
	IF USED('_lst_exc1')	
		UPDATE rg23d SET rg23d.Entry_ty = _lst_exc1.Entry_ty,rg23d.Date = _lst_exc1.Date,;
			rg23d.Doc_no = _lst_exc1.Doc_no,rg23d.ItSerial = _lst_exc1.ItSerial ;
			From rg23d,_lst_exc1 ;
			WHERE rg23d.mcode = _lst_exc1.mcode AND rg23d.mexamt = _lst_exc1.mexamt AND ;
				rg23d.item = _lst_exc1.item AND rg23d.mqty = _lst_exc1.mqty AND ;
				rg23d.scode = _lst_exc1.scode AND rg23d.sdate = _lst_exc1.sdate AND rg23d.sbillno = _lst_exc1.sbillno AND ;
				rg23d.sexamt = _lst_exc1.sexamt AND rg23d.manubill = _lst_exc1.manubill AND rg23d.manucode = _lst_exc1.manucode AND ;
				rg23d.manudate = _lst_exc1.manudate AND rg23d.manuqty = _lst_exc1.manuqty AND rg23d.manuexamt = _lst_exc1.manuexamt AND ;
				rg23d.u_pinvno = _lst_exc1.u_pinvno AND rg23d.u_pinvdt = _lst_exc1.u_pinvdt AND ;
				rg23d.entry_ty = 'IR'
	Endif
	IF USED('_lst_exc1')
		USE IN _lst_exc1
	Endif	
	SELECT rg23d
	GO Top
	*Changes has been done by vasant on 23/04/2013 as per Bug 12948 (Changes needed in DE Tool regarding generation of XML for FORM-2). 

	_repotrigoldengine = SET("EngineBehavior")
	SET ENGINEBEHAVIOR 70
	SELECT * FROM rg23d GROUP BY entry_ty,date,doc_no,itserial INTO CURSOR _lst_exc1 readwrite
	SET ENGINEBEHAVIOR _repotrigoldengine

	IF USED('_lst_exc1')	
		SELECT _lst_exc1
		REPLACE ALL csrno WITH 3 IN _lst_exc1
		REPLACE ALL CBillNo WITH '',CDate WITH {},CItSerial WITH '' IN _lst_exc1
		
		SELECT rg23d
		DELETE from rg23d where EMPTY(cbillno)

		SELECT rg23d
		GO TOP 
		APPEND FROM DBF('_lst_exc1')
		GO Top
		USE IN _lst_exc1
	ELSE
		_errmessage = 'Output Table Checking Error'
	ENDIF
	
	SELECT * FROM rg23d INTO CURSOR _lst_exc1 ORDER BY CSrNo,CBillNo,CDate,CItSerial,Date,InvNo,ItSerial
	IF USED('_lst_exc1')	
		SELECT rg23d
		DELETE ALL IN rg23d
		GO Top
		APPEND FROM DBF('_lst_exc1')
		GO Top
		USE IN _lst_exc1
	ELSE
		_errmessage = 'Output Table Checking Error'
	ENDIF
	

case mProduct = 'VU9'				
	SELECT lst_exc
	REPLACE ALL EtType WITH 'DEALER' FOR (EMPTY(EtType) AND Entry_ty = 'GT') OR UPPER(EtType) = 'SUPPLIER' OR UPPER(EtType) = 'DEPOT' IN lst_exc
	GO top
	REPLACE ALL EtType WITH Proper(EtType) IN lst_exc

	*Changes has been done by vasant on 23/04/2013 as per Bug 12948 (Changes needed in DE Tool regarding generation of XML for FORM-2). 
	SELECT * FROM lst_exc WHERE Entry_ty != 'IR' INTO CURSOR _lst_exc1
	IF USED('_lst_exc1')	
		UPDATE lst_exc SET lst_exc.Entry_ty = _lst_exc1.Entry_ty,lst_exc.Tran_cd = _lst_exc1.Tran_cd,lst_exc.ItSerial = _lst_exc1.ItSerial, ;
			lst_exc.PEntry_ty = _lst_exc1.PEntry_ty,lst_exc.PTran_cd = _lst_exc1.PTran_cd,lst_exc.PItSerial = _lst_exc1.PItSerial ;
			From lst_exc,_lst_exc1 ;
			WHERE lst_exc.Qty = _lst_exc1.Qty AND lst_exc.Examt = _lst_exc1.Examt AND ;
				lst_exc.BillQty = _lst_exc1.BillQty AND lst_exc.BillExamt = _lst_exc1.BillExamt AND ;
				lst_exc.It_name = _lst_exc1.It_name AND lst_exc.U_Pinvno = _lst_exc1.U_Pinvno AND lst_exc.U_Pinvdt = _lst_exc1.U_Pinvdt AND ;
				lst_exc.SCons_Id = _lst_exc1.SCons_Id AND lst_exc.SSCons_Id = _lst_exc1.SSCons_Id AND lst_exc.MtDuty = _lst_exc1.MtDuty AND ;
				lst_exc.Entry_ty = 'IR'
	Endif
	IF USED('_lst_exc1')
		USE IN _lst_exc1
	Endif	
	SELECT lst_exc
	GO Top
	*Changes has been done by vasant on 23/04/2013 as per Bug 12948 (Changes needed in DE Tool regarding generation of XML for FORM-2). 
	
	_repotrigoldengine = SET("EngineBehavior")
	SET ENGINEBEHAVIOR 70
	SELECT * FROM lst_exc GROUP BY entry_ty,tran_cd,itserial INTO CURSOR _lst_exc1 readwrite
	SET ENGINEBEHAVIOR _repotrigoldengine

	IF USED('_lst_exc1')	
		SELECT lst_exc
		REPLACE ALL u_pinvno WITH '' IN lst_exc
	
		SELECT _lst_exc1
		REPLACE ALL centry_ty WITH entry_ty,cdate WITH date,ctran_cd WITH tran_cd,cinv_no WITH '' IN _lst_exc1
		
		SELECT lst_exc
		APPEND FROM DBF('_lst_exc1')
		USE IN _lst_exc1
	ELSE
		_errmessage = 'Output Table Checking Error'
	ENDIF
	
case mProduct = 'VU0'			&&vasant110710
	SELECT lst_exc
	REPLACE ALL colno WITH 2 IN lst_exc

	_repotrigoldengine = SET("EngineBehavior")
	SET ENGINEBEHAVIOR 70
	SELECT * FROM lst_exc GROUP BY entry_ty,tran_cd,itserial INTO CURSOR _lst_exc1 readwrite
	SET ENGINEBEHAVIOR _repotrigoldengine

	IF USED('_lst_exc1')	
		SELECT _lst_exc1
		REPLACE ALL colno WITH 3,centry_ty WITH entry_ty,cdate WITH date,ctran_cd WITH tran_cd,cinv_no WITH u_pinvno IN _lst_exc1
		
		SELECT lst_exc
		APPEND FROM DBF('_lst_exc1')
		USE IN _lst_exc1
	ELSE
		_errmessage = 'Output Table Checking Error'
	ENDIF
endcase
RETURN _errmessage


PROCEDURE XMLGroupFinder
LPARAMETERS _trigtbl
IF TYPE('_trigtbl') <> 'C'
	_trigtbl = ''
Endif
_trigretvalue = ''
_oldtrigdatasession = SET("Datasession")
_trigtotsession = ASESSIONS(_trigsessionarr)
IF EMPTY(_trigretvalue) AND _trigretvalue <> '0'
	FOR _trigstartsession = 1 TO _trigtotsession
		SET DATASESSION TO _trigsessionarr(_trigstartsession)
		IF USED(JUSTSTEM(_trigtbl)) AND !EMPTY(_trigtbl)
			IF TYPE(_trigtbl) = 'N'
				_trigretvalue = str(&_trigtbl,1)
			Else
				_trigretvalue = TRANSFORM(&_trigtbl)
			Endif	
		Endif	
	ENDFOR
Endif	
SET DATASESSION TO (_oldtrigdatasession)
RETURN ALLTRIM(_trigretvalue)



PROCEDURE CheckDutyDiff
LPARAMETERS _DutyOn,_DutyType

_oldAlias = ALIAS()
SELECT XmlData_vw
_oldRec   = RECNO()
_DutyDiff = 0
IF _DutyOn = 'S'
	SELECT SUM(Round(Val(SaleExAmt2)+Val(SaleCessAmt2)+Val(SaleHCessAmt2)+Val(SaleCVDAmt2),0)) As RndDuty,;
		SUM(Val(SaleExAmt2)+Val(SaleCessAmt2)+Val(SaleHCessAmt2)+Val(SaleCVDAmt2)) As ActDuty FROM XmlData_vw WHERE TagGroup ='2' INTO CURSOR _lst_exc1
Endif
IF _DutyOn = 'P'
	SELECT SUM(Round(Val(PurExAmt3)+Val(PurCessAmt3)+Val(PurHCessAmt3)+Val(PurCVDAmt3),0)) As RndDuty,;
		SUM(Val(PurExAmt3)+Val(PurCessAmt3)+Val(PurHCessAmt3)+Val(PurCVDAmt3)) As ActDuty FROM XmlData_vw WHERE TagGroup ='3' INTO CURSOR _lst_exc1
Endif
IF USED('_lst_exc1')	
	SELECT _lst_exc1
	IF RECCOUNT() > 0
		IF !ISNULL(_lst_exc1.RndDuty) AND _DutyType = 'R'
			_DutyDiff = _lst_exc1.RndDuty
		ENDIF
		IF !ISNULL(_lst_exc1.ActDuty) AND _DutyType = 'A'
			_DutyDiff = _lst_exc1.ActDuty
		Endif
		IF !ISNULL(_lst_exc1.RndDuty) AND !ISNULL(_lst_exc1.ActDuty) AND _DutyType = 'D'
			_DutyDiff = _lst_exc1.RndDuty - _lst_exc1.ActDuty
		Endif	
	Endif	
	USE IN _lst_exc1
ENDIF

SELECT XmlData_vw
IF _oldRec > 0
	GO _oldRec
Endif	
IF !EMPTY(_oldAlias)
	SELECT (_oldAlias)
Endif	
RETURN _DutyDiff



PROCEDURE eFileDefaValidations
_oldAlias = ALIAS()
SELECT XmlData_vw
_thismthodsuccessdone = .t.
REPLACE ALL RateUnit2 WITH UPPER(XMLData_Vw.RateUnit2),;
	PurRegnNo3 WITH STRTRAN(XMLData_Vw.PurRegnNo3,' ',''),;	
	CompRegNo1 WITH STRTRAN(XMLData_Vw.CompRegNo1,' ','') IN XMLData_Vw
IF mProduct = 'VU9'
	REPLACE ALL RateUnit3 WITH UPPER(XMLData_Vw.RateUnit3) IN XMLData_Vw
ENDIF

&&vasant100813
SELECT XmlData_vw
_MainEccno  = ''
SELECT LEFT(Compregno1,50) As Eccno FROM XmlData_vw WHERE INLIST(TagGroup,'1') INTO CURSOR _lst_exc1 Readwrite
IF USED('_lst_exc1')	
	SELECT _lst_exc1
	GO Top
	_MainEccno  = _lst_exc1.Eccno
	USE IN _lst_exc1
ELSE
	_thismthodsuccessdone = .f.	
ENDIF

IF !EMPTY(_MainEccno)
	SELECT LEFT(PurPartyName3,254) as PurPartyName3 ;
		FROM XmlData_vw WHERE INLIST(TagGroup,'3') AND ALLTRIM(Purregnno3) == ALLTRIM(_MainEccno) ;
		AND UPPER(ALLTRIM(Purpartytype3)) != 'IMPORTER' ;
		INTO CURSOR _lst_exc1
	SELECT PurPartyName3 FROM _lst_exc1 GROUP BY PurPartyName3 INTO CURSOR _lst_exc2
	IF USED('_lst_exc1') AND USED('_lst_exc2')	
		IF RECCOUNT('_lst_exc2') > 0
			REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						CHR(13) + [Company ECC No. & Purchase Party ECC No. can't be same.] +;
						CHR(13) + [Following Parties ECC No.'s are matching with Company ECC No.] IN ErrLog
			SELECT _lst_exc2
			Scan
				REPLACE ErrMsg WITH ErrLog.ErrMsg +;
					CHR(13) +'     --> '+ALLTRIM(_lst_exc2.PurPartyName3) IN ErrLog
				SELECT _lst_exc2
			Endscan	
			_thismthodsuccessdone = .f.	
		Endif
		USE IN _lst_exc1
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF
Endif	
&&vasant100813

SELECT * FROM XmlData_vw WHERE INLIST(TagGroup,'1') INTO CURSOR _lst_exc1 Readwrite
SELECT _lst_exc1
REPLACE ALL TagGroup WITH '4' IN _lst_exc1
GO Top
INSERT INTO XmlData_vw SELECT * FROM _lst_exc1
USE IN _lst_exc1

SELECT cetsh,rateunit FROM cetsh WHERE 1 = 2 INTO CURSOR _lst_exc2
_chapnolen   = FSIZE('cetsh','_lst_exc2')
_rateunitlen =  FSIZE('rateunit','_lst_exc2')
IF mProduct = 'VU8'
	SELECT Left(ChapNo2,_chapnolen) As ChapNo,Left(RateUnit2,_rateunitlen) As RateUnit FROM XmlData_vw WHERE INLIST(TagGroup,'2','3') INTO CURSOR _lst_exc1 Readwrite
Else	
	SELECT Left(ChapNo2,_chapnolen) As ChapNo,Left(RateUnit2,_rateunitlen) As RateUnit FROM XmlData_vw WHERE INLIST(TagGroup,'2') INTO CURSOR _lst_exc1 Readwrite;
		Union All ;
		SELECT Left(ChapNo3,_chapnolen) As ChapNo,Left(RateUnit3,_rateunitlen) As RateUnit FROM XmlData_vw WHERE INLIST(TagGroup,'3')
Endif	
IF USED('_lst_exc1')	
	&&vasant061010 Changes done as per ETKT-227
	_isconvmessage = .f.
	_isconvcont = 7
	SELECT dist a.ChapNo,a.RateUnit as UOM_Cur,b.RateUnit as UOM_Org FROM _lst_exc1 a,cetsh b WHERE ;
		ALLTRIM(a.ChapNo) == ALLTRIM(b.cetsh) ;
		AND a.rateunit in (SELECT Dist RateUnit FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				IF !(UPPER(ALLTRIM(_lst_exc2.UOM_Cur)) == UPPER(ALLTRIM(_lst_exc2.UOM_Org)))
					_isconvmessage = .t.
					Exit
				Endif		
			Endscan		
		Endif	
		USE IN _lst_exc2
	ENDIF
	IF _isconvmessage = .t.
		_isconvcont = MESSAGEBOX([Some of the Chapter Nos. for the Items should have UOM as ‘KG’ as per Excise.]+CHR(13)+[Do you want to convert Quantity from ‘MT’ to ‘KG’ ?],36,vumess)
		IF _isconvcont = 6
			UPDATE _lst_exc1 SET RateUnit = Conversion.NRateUnit from _lst_exc1,Conversion,cetsh ;
				WHERE UPPER(ALLTRIM(_lst_exc1.RateUnit)) == UPPER(ALLTRIM(Conversion.ORateUnit)) ;
				AND ALLTRIM(_lst_exc1.Chapno) == ALLTRIM(cetsh.cetsh) ;
				AND !(UPPER(ALLTRIM(_lst_exc1.RateUnit)) == UPPER(ALLTRIM(cetsh.rateunit)))
		Endif		
	Endif	

*!*		UPDATE _lst_exc1 SET RateUnit = Conversion.NRateUnit from _lst_exc1,Conversion ;
*!*			WHERE UPPER(ALLTRIM(_lst_exc1.RateUnit)) == UPPER(ALLTRIM(Conversion.ORateUnit))
	&&vasant061010 End of Changes done as per ETKT-227
	
	SELECT dist ChapNo FROM _lst_exc1 WHERE ChapNo NOT in (SELECT cetsh FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			Scan
				REPLACE ErrMsg WITH ErrLog.ErrMsg +;
					CHR(13)+'Unable to find Chap. No. '+ALLTRIM(_lst_exc2.ChapNo) IN ErrLog
			Endscan		
			_thismthodsuccessdone = .f.	
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF
	
	SELECT dist a.ChapNo,a.RateUnit as UOM_Cur,b.RateUnit as UOM_Org FROM _lst_exc1 a,cetsh b WHERE ;
		ALLTRIM(a.ChapNo) == ALLTRIM(b.cetsh) ;
		AND a.rateunit NOT in (SELECT Dist RateUnit FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				IF !(UPPER(ALLTRIM(_lst_exc2.UOM_Cur)) == UPPER(ALLTRIM(_lst_exc2.UOM_Org)))
					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						CHR(13) + 'Current Value : '+IIF(EMPTY(_lst_exc2.UOM_Cur),'<BLANK>',ALLTRIM(_lst_exc2.UOM_Cur)) IN ErrLog
					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						' --> '+'UOM for the Chap. No. '+ALLTRIM(_lst_exc2.ChapNo)+' should be '+ALLTRIM(_lst_exc2.UOM_Org) IN ErrLog
					_thismthodsuccessdone = .f.	
				Endif		
			Endscan		
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF

	SELECT dist a.ChapNo,a.RateUnit as UOM_Cur,b.RateUnit as UOM_Org FROM _lst_exc1 a,cetsh b WHERE ;
		ALLTRIM(a.ChapNo) == ALLTRIM(b.cetsh) ;
		AND a.rateunit in (SELECT Dist RateUnit FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				IF !(UPPER(ALLTRIM(_lst_exc2.UOM_Cur)) == UPPER(ALLTRIM(_lst_exc2.UOM_Org)))
					REPLACE WarnMsg WITH ErrLog.WarnMsg +;
						CHR(13) + 'Current Value : '+IIF(EMPTY(_lst_exc2.UOM_Cur),'<BLANK>',ALLTRIM(_lst_exc2.UOM_Cur)) IN ErrLog
					REPLACE WarnMsg WITH ErrLog.WarnMsg +;
						' --> '+'UOM for the Chap. No. '+ALLTRIM(_lst_exc2.ChapNo)+' should be '+ALLTRIM(_lst_exc2.UOM_Org) IN ErrLog
					_thismthodsuccessdone = .f.	
				Endif		
			Endscan		
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF

	USE IN _lst_exc1
ELSE
	_thismthodsuccessdone = .f.	
ENDIF

&&vasant061010 Changes done as per ETKT-227
If	_isconvcont = 6
	IF mProduct = 'VU8'
		SELECT ALLTRIM(Left(a.ItemName2,255)) As ItemName,b.nRateUnit,b.oRateUnit ;
			from XmlData_vw a,Conversion b,Cetsh c ;
			WHERE ALLTRIM(LEFT(a.RateUnit2,_rateunitlen)) == ALLTRIM(b.oRateUnit) AND INLIST(a.TagGroup,'2','3') ;
				AND ALLTRIM(LEFT(a.Chapno2,255)) == ALLTRIM(c.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(a.RateUnit2,_rateunitlen))) == UPPER(ALLTRIM(c.rateunit))) ;
			INTO CURSOR _lst_exc1

		UPDATE XmlData_vw SET RateUnit2 = ALLTRIM(Conversion.nRateUnit),;
			SaleQty2 = Tran(Val(XmlData_vw.SaleQty2) * Conversion.Ratio) ;
			from XmlData_vw,Conversion,Cetsh ;
			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '2' ;
				AND ALLTRIM(LEFT(XmlData_vw.Chapno2,255)) == ALLTRIM(cetsh.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen))) == UPPER(ALLTRIM(cetsh.rateunit))) 			
		UPDATE XmlData_vw SET RateUnit2 = ALLTRIM(Conversion.nRateUnit),;
			PurQty3 = Tran(Val(XmlData_vw.PurQty3) * Conversion.Ratio) ;
			from XmlData_vw,Conversion,Cetsh ;
			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '3' ;
				AND ALLTRIM(LEFT(XmlData_vw.Chapno2,255)) == ALLTRIM(cetsh.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen))) == UPPER(ALLTRIM(cetsh.rateunit))) 			
	ENDIF
	IF mProduct = 'VU9'
		SELECT ALLTRIM(Left(a.ItemName2,255)) As ItemName,b.nRateUnit,b.oRateUnit ;
			from XmlData_vw a,Conversion b,Cetsh c ;
			WHERE ALLTRIM(LEFT(a.RateUnit2,_rateunitlen)) == ALLTRIM(b.oRateUnit) AND INLIST(a.TagGroup,'2') ;
				AND ALLTRIM(LEFT(a.Chapno2,255)) == ALLTRIM(c.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(a.RateUnit2,_rateunitlen))) == UPPER(ALLTRIM(c.rateunit))) ;
			UNION ALL SELECT ALLTRIM(Left(a.ItemName3,255)) As ItemName,b.nRateUnit,b.oRateUnit ;
			from XmlData_vw a,Conversion b,Cetsh c ;
			WHERE ALLTRIM(LEFT(a.RateUnit3,_rateunitlen)) == ALLTRIM(b.oRateUnit) AND INLIST(a.TagGroup,'3') ;
				AND ALLTRIM(LEFT(a.Chapno3,255)) == ALLTRIM(c.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(a.RateUnit3,_rateunitlen))) == UPPER(ALLTRIM(c.rateunit))) ;
			INTO CURSOR _lst_exc1	

		UPDATE XmlData_vw SET RateUnit2 = ALLTRIM(Conversion.nRateUnit),;
			SaleQty2 = Tran(Val(XmlData_vw.SaleQty2) * Conversion.Ratio) ;
			from XmlData_vw,Conversion,Cetsh ;
			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '2' ;
				AND ALLTRIM(LEFT(XmlData_vw.Chapno3,255)) == ALLTRIM(cetsh.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen))) == UPPER(ALLTRIM(cetsh.rateunit))) 			
			
		UPDATE XmlData_vw SET RateUnit3 = ALLTRIM(Conversion.nRateUnit),;
			PurQty3 = Tran(Val(XmlData_vw.PurQty3) * Conversion.Ratio) ;
			from XmlData_vw,Conversion,Cetsh ;
			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit3,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '3' ;
				AND ALLTRIM(LEFT(XmlData_vw.Chapno3,255)) == ALLTRIM(cetsh.cetsh) ;
				AND !(UPPER(ALLTRIM(LEFT(XmlData_vw.RateUnit3,_rateunitlen))) == UPPER(ALLTRIM(cetsh.rateunit))) 			
	ENDIF
	IF USED('_lst_exc1')	
		SELECT dist ItemName,nRateUnit,oRateUnit FROM _lst_exc1 INTO CURSOR _lst_exc2
		IF USED('_lst_exc2')	
			SELECT _lst_exc2
			IF RECCOUNT() > 0
				SCAN
					IF !(UPPER(ALLTRIM(_lst_exc2.oRateUnit)) == UPPER(ALLTRIM(_lst_exc2.nRateUnit)))
						REPLACE WarnMsg WITH ErrLog.WarnMsg +;
							CHR(13) + 'Item Name : '+ALLTRIM(_lst_exc2.ItemName) IN ErrLog
						REPLACE WarnMsg WITH ErrLog.WarnMsg +;
							' --> '+'Quantity Converted from '+ALLTRIM(_lst_exc2.oRateUnit)+' to '+ALLTRIM(_lst_exc2.nRateUnit) IN ErrLog
						_thismthodsuccessdone = .f.	
					Endif		
				Endscan		
			Endif	
			USE IN _lst_exc2
		ELSE
			_thismthodsuccessdone = .f.	
		ENDIF
		USE IN _lst_exc1
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF
Endif	

*!*	IF mProduct = 'VU8'
*!*		SELECT ALLTRIM(Left(a.ItemName2,255)) As ItemName,b.nRateUnit,b.oRateUnit ;
*!*			from XmlData_vw a,Conversion b;
*!*			WHERE ALLTRIM(LEFT(a.RateUnit2,_rateunitlen)) == ALLTRIM(b.oRateUnit) AND INLIST(a.TagGroup,'2','3') ;
*!*			INTO CURSOR _lst_exc1

*!*		UPDATE XmlData_vw SET RateUnit2 = ALLTRIM(Conversion.nRateUnit),;
*!*			SaleQty2 = Tran(Val(XmlData_vw.SaleQty2) * Conversion.Ratio) ;
*!*			from XmlData_vw,Conversion ;
*!*			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '2'
*!*		UPDATE XmlData_vw SET RateUnit2 = ALLTRIM(Conversion.nRateUnit),;
*!*			PurQty3 = Tran(Val(XmlData_vw.PurQty3) * Conversion.Ratio) ;
*!*			from XmlData_vw,Conversion ;
*!*			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '3'
*!*	ENDIF
*!*	IF mProduct = 'VU9'
*!*		SELECT ALLTRIM(Left(a.ItemName2,255)) As ItemName,b.nRateUnit,b.oRateUnit ;
*!*			from XmlData_vw a,Conversion b;
*!*			WHERE ALLTRIM(LEFT(a.RateUnit2,_rateunitlen)) == ALLTRIM(b.oRateUnit) AND INLIST(a.TagGroup,'2') ;
*!*			UNION ALL SELECT ALLTRIM(Left(a.ItemName3,255)) As ItemName,b.nRateUnit,b.oRateUnit ;
*!*			from XmlData_vw a,Conversion b;
*!*			WHERE ALLTRIM(LEFT(a.RateUnit3,_rateunitlen)) == ALLTRIM(b.oRateUnit) AND INLIST(a.TagGroup,'3') ;
*!*			INTO CURSOR _lst_exc1	

*!*		UPDATE XmlData_vw SET RateUnit2 = ALLTRIM(Conversion.nRateUnit),;
*!*			SaleQty2 = Tran(Val(XmlData_vw.SaleQty2) * Conversion.Ratio) ;
*!*			from XmlData_vw,Conversion ;
*!*			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit2,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '2'
*!*		UPDATE XmlData_vw SET RateUnit3 = ALLTRIM(Conversion.nRateUnit),;
*!*			PurQty3 = Tran(Val(XmlData_vw.PurQty3) * Conversion.Ratio) ;
*!*			from XmlData_vw,Conversion ;
*!*			WHERE ALLTRIM(LEFT(XmlData_vw.RateUnit3,_rateunitlen)) == ALLTRIM(Conversion.oRateUnit) AND XmlData_vw.TagGroup = '3'
*!*	ENDIF
*!*	IF USED('_lst_exc1')	
*!*		SELECT dist ItemName,nRateUnit,oRateUnit FROM _lst_exc1 INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				SCAN
*!*					IF !(UPPER(ALLTRIM(_lst_exc2.oRateUnit)) == UPPER(ALLTRIM(_lst_exc2.nRateUnit)))
*!*						REPLACE WarnMsg WITH ErrLog.WarnMsg +;
*!*							CHR(13) + 'Item Name : '+ALLTRIM(_lst_exc2.ItemName) IN ErrLog
*!*						REPLACE WarnMsg WITH ErrLog.WarnMsg +;
*!*							' --> '+'Quantity Converted from '+ALLTRIM(_lst_exc2.oRateUnit)+' to '+ALLTRIM(_lst_exc2.nRateUnit) IN ErrLog
*!*						_thismthodsuccessdone = .f.	
*!*					Endif		
*!*				Endscan		
*!*			Endif	
*!*			USE IN _lst_exc2
*!*		ELSE
*!*			_thismthodsuccessdone = .f.	
*!*		ENDIF
*!*		USE IN _lst_exc1
*!*	ELSE
*!*		_thismthodsuccessdone = .f.	
*!*	ENDIF
&&vasant061010 End of Changes done as per ETKT-227

SELECT XmlData_vw
IF !EMPTY(_oldAlias)
	SELECT (_oldAlias)
Endif	
RETURN _thismthodsuccessdone

PROCEDURE GetSLNo
LPARAMETERS _mGetSLNoType
_mXMLSlNo = 0
IF _mGetSLNoType = 'A'
	_mXMLSlNoA = _mXMLSlNoA + 1
	_mXMLSlNo = _mXMLSlNoA
Endif	
IF _mGetSLNoType = 'B'
	_mXMLSlNoB = _mXMLSlNoB + 1
	_mXMLSlNo = _mXMLSlNoB
Endif	
RETURN ALLTRIM(STR(_mXMLSlNo))

PROCEDURE GetBillDet
LPARAMETERS _mGetSLNoType
_mLastRecDet = _mCurRecDet
IF _mGetSLNoType = 'A'
	_mCurRecDet = XmlData_Vw.SaleBillNo2+XmlData_Vw.SaleBillDt2
Else	
	_mCurRecDet = XmlData_Vw.PurBillNo3+XmlData_Vw.PurBillDt3+XmlData_Vw.PurPartyName3
ENDIF
RETURN ''

PROCEDURE ReSetSLNo
IF TYPE('_mXMLSlNoA') = 'U'
	PUBLIC _mXMLSlNoA
Endif	
_mXMLSlNoA = 0

IF TYPE('_mXMLSlNoB') = 'U'
	PUBLIC _mXMLSlNoB
Endif	
_mXMLSlNoB = 0

IF TYPE('_mLastRecDet') = 'U'
	PUBLIC _mLastRecDet
Endif	
_mLastRecDet = '*1*'

IF TYPE('_mCurRecDet') = 'U'
	PUBLIC _mCurRecDet
Endif	
_mCurRecDet = '*2*'
RETURN ''

PROCEDURE FillRemrk()
RETURN "InnvoiceDutyActual : "+Allt(Str(CheckDutyDiff('S','A'),19,2))+" DocumentDutyactual : "+Allt(Str(CheckDutyDiff('P','A'),19,2))+" InvoiceDutyRounded : "+Allt(Str(CheckDutyDiff('S','R'),19,0))+" DocumentDutyRounded : "+Allt(Str(CheckDutyDiff('P','R'),19,2))+" InvoiceDutyDiff : "+Allt(Str(CheckDutyDiff('S','D'),19,2))+" DocumentDutyDiff : "+Allt(Str(CheckDutyDiff('P','D'),19,2))
