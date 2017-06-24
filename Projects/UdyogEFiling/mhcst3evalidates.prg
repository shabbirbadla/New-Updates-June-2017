
*!*	PROCEDURE BeforeRawXMLUpdates
*!*	_errmessage = ''
*!*	IF !USED('lst_exc')
*!*		_errmessage = 'Unable to find output table'
*!*	Endif
*!*	IF !EMPTY(_errmessage)
*!*		RETURN _errmessage
*!*	Endif	

*!*	DELETE FROM lst_exc WHERE qm = '3' AND EMPTY(entry_ty)

*!*	DO Case
*!*	Case mProduct = 'VU8'
*!*		SELECT e_name,e_name as newe_name FROM lst_exc WHERE !EMPTY(e_name) GROUP BY e_name INTO CURSOR _lst_exc1 readwrite
*!*	Case mProduct = 'VU9'
*!*		SELECT e_name,e_name as newe_name  FROM lst_exc WHERE !EMPTY(e_name) GROUP BY e_name INTO CURSOR _lst_exc1 readwrite ;
*!*			Union All ;
*!*			SELECT shortnm as e_name,shortnm as newe_name  FROM lst_exc WHERE !EMPTY(shortnm) GROUP BY shortnm
*!*	ENDCASE	
*!*	IF USED('_lst_exc1')	
*!*		SELECT _lst_exc1
*!*		SCAN
*!*			_mnewe_name = ALLTRIM(_lst_exc1.newe_name)
*!*			DO WHILE .t.
*!*				IF ISDIGIT(LEFT(_mnewe_name,1)) = .t.
*!*					_mnewe_name = ALLTRIM(SUBSTR(_mnewe_name,2))
*!*				ELSE
*!*					exit	
*!*				Endif
*!*			ENDDO
*!*			IF ALLTRIM(_lst_exc1.newe_name) == _mnewe_name
*!*				_mnewe_name = SUBSTR(_mnewe_name,2)
*!*			Endif 
*!*			REPLACE newe_name WITH _mnewe_name IN _lst_exc1
*!*			SELECT _lst_exc1
*!*		ENDSCAN

*!*		SELECT DISTINCT newe_name FROM _lst_exc1 WHERE newe_name NOT in (SELECT DISTINCT Name FROM ExciseMap) INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				_errmessage = 'Following Duties not found in DE Tool'
*!*				Scan
*!*					_errmessage = _errmessage + CHR(13) + STR(RECNO(),2)+'. - '+ALLTRIM(_lst_exc2.newe_name)
*!*				Endscan
*!*			Endif
*!*			USE IN _lst_exc2
*!*		ELSE
*!*			_errmessage = 'Excise Table Checking Error'
*!*		ENDIF

*!*		IF EMPTY(_errmessage)
*!*			Update lst_exc SET e_name = STR(excisemap.srno,2)+excisemap.efiletag from lst_exc ,excisemap,_lst_exc1 ;
*!*				WHERE ALLTRIM(lst_exc.e_name) == ALLTRIM(_lst_exc1.e_name) ;
*!*				AND ALLTRIM(_lst_exc1.newe_name) == ALLTRIM(excisemap.name)
*!*			IF mProduct = 'VU9'
*!*				Update lst_exc SET shortnm = STR(excisemap.srno,2)+excisemap.efiletag from lst_exc ,excisemap,_lst_exc1 ;
*!*					WHERE ALLTRIM(lst_exc.shortnm) == ALLTRIM(_lst_exc1.e_name) ;
*!*					AND ALLTRIM(_lst_exc1.newe_name) == ALLTRIM(excisemap.name)
*!*			ENDIF 
*!*		Endif		
*!*		USE IN _lst_exc1
*!*	ELSE
*!*		_errmessage = 'Excise Table Checking Error'
*!*	Endif
*!*	IF !EMPTY(_errmessage)
*!*		RETURN _errmessage
*!*	Endif	

*!*	DO Case
*!*	Case mProduct = 'VU8'
*!*		SELECT * FROM excisemap WHERE STR(srno,2)+efiletag NOT in (SELECT e_name FROM lst_exc WHERE qm = '6') INTO CURSOR _lst_exc1
*!*	Case mProduct = 'VU9'
*!*		SELECT * FROM excisemap WHERE STR(srno,2)+efiletag NOT in (SELECT shortnm FROM lst_exc WHERE qm = '6') INTO CURSOR _lst_exc1
*!*	ENDCASE

*!*	SELECT _lst_exc1
*!*	scan
*!*		SELECT lst_exc
*!*		APPEND BLANK IN lst_exc
*!*		REPLACE e_name with STR(_lst_exc1.srno,2)+_lst_exc1.efiletag,qm with '6',srno with 'A' IN lst_exc
*!*		SELECT _lst_exc1
*!*	ENDSCAN

*!*	IF USED('_lst_exc1')	
*!*		USE IN _lst_exc1
*!*	Endif	

*!*	SELECT lst_exc	
*!*	SELECT e_name FROM lst_exc WHERE qm = '3' AND !EMPTY(e_name) GROUP BY e_name INTO CURSOR _lst_exc1

*!*	SELECT _lst_exc1
*!*	GO top
*!*	_me_name = _lst_exc1.e_name

*!*	_repotrigoldengine = SET("EngineBehavior")
*!*	SET ENGINEBEHAVIOR 70
*!*	DO CASE 
*!*	Case mProduct = 'VU8'
*!*		select * from lst_exc group by entry_ty,date,doc_no,item_no where qm = '3' and e_name = _me_name into cursor _lst_exc1
*!*		update lst_exc set e_name = str(_lst_exc1.e_per,2)+SUBSTR(lst_exc.e_name,3),inv_no = str(_lst_exc1.e_per,2) from lst_exc,_lst_exc1 where lst_exc.entry_ty = _lst_exc1.entry_ty and lst_exc.date = _lst_exc1.date and lst_exc.doc_no = _lst_exc1.doc_no and lst_exc.item_no = _lst_exc1.item_no AND lst_exc.qm = '3'
*!*		update lst_exc set e_per = _lst_exc1.e_per from lst_exc,_lst_exc1 where lst_exc.entry_ty = _lst_exc1.entry_ty and lst_exc.date = _lst_exc1.date and lst_exc.doc_no = _lst_exc1.doc_no and lst_exc.item_no = _lst_exc1.item_no AND lst_exc.e_duty = 0 AND lst_exc.qm = '3'
*!*	Case mProduct = 'VU9'
*!*		select * from lst_exc group by entry_ty,date,doc_no,itserial where qm = '3' and e_name = _me_name into cursor _lst_exc1
*!*		update lst_exc set shortnm = e_name from lst_exc where qm = '3'
*!*		update lst_exc set e_name = str(_lst_exc1.e_per,2)+SUBSTR(lst_exc.e_name,3),u_arrears = str(_lst_exc1.e_per,2) from lst_exc,_lst_exc1 where lst_exc.entry_ty = _lst_exc1.entry_ty and lst_exc.date = _lst_exc1.date and lst_exc.doc_no = _lst_exc1.doc_no and lst_exc.itserial = _lst_exc1.itserial AND lst_exc.qm = '3'
*!*		update lst_exc set e_per = _lst_exc1.e_per from lst_exc,_lst_exc1 where lst_exc.entry_ty = _lst_exc1.entry_ty and lst_exc.date = _lst_exc1.date and lst_exc.doc_no = _lst_exc1.doc_no and lst_exc.itserial = _lst_exc1.itserial AND lst_exc.e_duty = 0 AND lst_exc.qm = '3'
*!*		select * from lst_exc group by entry_ty,tran_cd where qm = '9' into cursor _lst_exc1
*!*		IF USED('_lst_exc1')	
*!*			nhandle = 0
*!*			SELECT _lst_exc1
*!*			SCAN
*!*				_mu_pfor = ''
*!*				mexecquery = 'select top 1 entry_ty,tran_cd,u_plasr from stkl_vw_main where entry_ty = ?_lst_exc1.entry_ty and tran_cd = ?_lst_exc1.tran_cd'
*!*				nretval = _screenactiveform.sqlconobj.dataconn('EXE',_screenactiveform.CompanyDb,mexecquery,'_lst_exc2','nhandle')
*!*				If nretval<=0 AND !USED('_lst_exc2')
*!*					_errmessage = 'Connection Error'
*!*				ELSE
*!*					_mu_pfor = _lst_exc2.u_plasr
*!*				Endif	
*!*				IF EMPTY(_mu_pfor)
*!*					mexecquery = 'select top 1 entry_ty,tran_cd,u_rg23no from stkl_vw_main where entry_ty = ?_lst_exc1.entry_ty and tran_cd = ?_lst_exc1.tran_cd'
*!*					nretval = _screenactiveform.sqlconobj.dataconn('EXE',_screenactiveform.CompanyDb,mexecquery,'_lst_exc2','nhandle')
*!*					If nretval<=0 AND !USED('_lst_exc2')
*!*						_errmessage = 'Connection Error'
*!*					ELSE
*!*						_mu_pfor = _lst_exc2.u_rg23no
*!*					Endif	
*!*				Endif	
*!*				IF !EMPTY(_mu_pfor)
*!*					update lst_exc set u_pfor = _mu_pfor from lst_exc,_lst_exc2 where lst_exc.entry_ty = _lst_exc2.entry_ty and lst_exc.tran_cd = _lst_exc2.tran_cd AND lst_exc.qm = '9'
*!*				ENDIF

*!*				SELECT _lst_exc1
*!*			Endscan
*!*			USE IN _lst_exc1
*!*			USE IN _lst_exc2
*!*			nsq = _screenactiveform.sqlconobj.sqlconnclose('nhandle')
*!*			If nsq<=0
*!*				_errmessage = 'Connection Closing Error'
*!*			ENDIF
*!*		ELSE
*!*			_errmessage = 'Excise Table Checking Error'
*!*		Endif
*!*	ENDCASE
*!*	SET ENGINEBEHAVIOR _repotrigoldengine
*!*	IF USED('_lst_exc1')	
*!*		USE IN _lst_exc1
*!*	Endif	

*!*	_firstname = ''
*!*	SELECT excisemap
*!*	SCAN
*!*		IF !EMPTY(excisemap.efiletag)
*!*			_firstname = STR(excisemap.srno,2)+excisemap.efiletag
*!*		ENDIF
*!*		IF excisemap.srno = 1 AND !EMPTY(_firstname)
*!*			EXIT
*!*		Endif	
*!*	ENDSCAN
*!*	SELECT lst_exc
*!*	REPLACE ALL e_name WITH _firstname FOR EMPTY(e_name) AND qm = '3' IN lst_exc

*!*	SELECT lst_exc
*!*	DO Case
*!*	Case mProduct = 'VU8'
*!*		SELECT e_name FROM lst_exc WHERE EMPTY(e_name) AND qm = '6' INTO CURSOR _lst_exc1
*!*	Case mProduct = 'VU9'
*!*		SELECT shortnm FROM lst_exc WHERE EMPTY(shortnm) AND qm = '6' AND !EMPTY(Entry_ty) INTO CURSOR _lst_exc1
*!*	Endcase	

*!*	IF USED('_lst_exc1')	
*!*		SELECT _lst_exc1
*!*		IF RECCOUNT() > 0
*!*			_errmessage = 'Blank Duty Names Not Allowed'
*!*		Endif
*!*		USE IN _lst_exc1
*!*	ELSE
*!*		_errmessage = 'Output Table Checking Error'
*!*	ENDIF
*!*	IF !EMPTY(_errmessage)
*!*		RETURN _errmessage
*!*	Endif	

*!*	If mProduct = 'VU9'
*!*		SELECT * FROM lst_exc WHERE QM = '3' INTO CURSOR _lst_exc1 READWRITE ;
*!*			ORDER BY CHAPNO,EIT_NAME,RATEUNIT,RULE,U_Arrears,u_pfor,SHORTNM,E_NAME,E_PER
*!*		SELECT * FROM lst_exc WHERE QM != '3' INTO CURSOR _lst_exc2 readwrite
*!*		IF USED('_lst_exc1') AND USED('_lst_exc2')	
*!*			DELETE from lst_exc
*!*			SELECT lst_exc
*!*			GO Top
*!*			APPEND FROM DBF('_lst_exc1')
*!*			GO Top		
*!*			APPEND FROM DBF('_lst_exc2')		
*!*			USE IN _lst_exc1
*!*			USE IN _lst_exc2
*!*			SELECT lst_exc
*!*			GO Top
*!*		ELSE
*!*			_errmessage = 'Output Table Checking Error'
*!*		ENDIF
*!*	ENDIF

*!*	RETURN _errmessage




*!*	PROCEDURE XMLGroupFinder
*!*	LPARAMETERS _trigtbl
*!*	IF TYPE('_trigtbl') <> 'C'
*!*		_trigtbl = ''
*!*	Endif
*!*	_trigretvalue = ''
*!*	_oldtrigdatasession = SET("Datasession")
*!*	_trigtotsession = ASESSIONS(_trigsessionarr)
*!*	IF EMPTY(_trigretvalue) AND _trigretvalue <> '0'
*!*		FOR _trigstartsession = 1 TO _trigtotsession
*!*			SET DATASESSION TO _trigsessionarr(_trigstartsession)
*!*			IF USED(JUSTSTEM(_trigtbl)) AND !EMPTY(_trigtbl)
*!*				IF TYPE(_trigtbl) = 'N'
*!*					_trigretvalue = str(&_trigtbl,1)
*!*				Else
*!*					_trigretvalue = TRANSFORM(&_trigtbl)
*!*				Endif	
*!*			Endif	
*!*		ENDFOR
*!*	Endif	
*!*	SET DATASESSION TO (_oldtrigdatasession)
*!*	RETURN ALLTRIM(_trigretvalue)


PROCEDURE eFileDefaValidations
_oldAlias = ALIAS()

*!*	SELECT XmlData_vw
*!*	_thismthodsuccessdone = .t.
*!*	REPLACE ALL RateUnit3 WITH UPPER(XMLData_Vw.RateUnit3),;
*!*		CompRegNo1 WITH STRTRAN(XMLData_Vw.CompRegNo1,' ','') IN XMLData_Vw
*!*	SELECT ALLTRIM(Left(chapno3,255)) As ChapNo,ALLTRIM(Left(RateUnit3,255)) As RateUnit FROM XmlData_vw ;
*!*		WHERE INLIST(TagGroup,'3') INTO CURSOR _lst_exc1
*!*	IF USED('_lst_exc1')	
*!*		SELECT dist ChapNo FROM _lst_exc1 WHERE ChapNo NOT in (SELECT cetsh FROM cetsh) INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				Scan
*!*					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*						CHR(13)+'Unable to find Chap. No. '+ALLTRIM(_lst_exc2.ChapNo) IN ErrLog
*!*				Endscan		
*!*				_thismthodsuccessdone = .f.	
*!*			Endif	
*!*			USE IN _lst_exc2
*!*		ELSE
*!*			_thismthodsuccessdone = .f.	
*!*		ENDIF

*!*		SELECT dist a.ChapNo,a.RateUnit as UOM_Cur,b.RateUnit as UOM_Org FROM _lst_exc1 a,cetsh b WHERE ;
*!*			ALLTRIM(a.ChapNo) == ALLTRIM(b.cetsh) ;
*!*			AND a.rateunit NOT in (SELECT Dist RateUnit FROM cetsh) INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				SCAN
*!*					IF !(UPPER(ALLTRIM(_lst_exc2.UOM_Cur)) == UPPER(ALLTRIM(_lst_exc2.UOM_Org)))
*!*						REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*							CHR(13) + 'Current Value : '+IIF(EMPTY(_lst_exc2.UOM_Cur),'<BLANK>',ALLTRIM(_lst_exc2.UOM_Cur)) IN ErrLog
*!*						REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*							' --> '+'UOM for the Chap. No. '+ALLTRIM(_lst_exc2.ChapNo)+' should be '+ALLTRIM(_lst_exc2.UOM_Org) IN ErrLog
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

*!*	SELECT ALLTRIM(Left(chapno3,255)) As ChapNo,ALLTRIM(Left(DutyHead3,255)) As DutyHead,;
*!*		ALLTRIM(Left(ItemDes_A3,255)) As Itemdes,ALLTRIM(Left(Rule3,255)) As Rule,;
*!*		VAL(RatDutyAd3) As DutyPer,VAL(DtyPyable3) As DutyAmt FROM XmlData_vw ;
*!*		WHERE INLIST(TagGroup,'3') And VAL(DtyPyable3) > 0 AND VAL(RatDutyAd3) = 0 INTO CURSOR _lst_exc1
*!*	IF USED('_lst_exc1')	
*!*		SELECT _lst_exc1
*!*		IF RECCOUNT() > 0
*!*			SCAN
*!*				REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*					CHR(13)+'Duty Head : '+ALLTRIM(_lst_exc1.DutyHead)+', Duty Amount : '+ALLTRIM(Trans(_lst_exc1.DutyAmt))+', Duty Rate can not be 0.00 for Chap. No. : '+ALLTRIM(_lst_exc1.ChapNo);
*!*					+ ', Item : '+ALLTRIM(_lst_exc1.Itemdes) +  ', Rule : '+ALLTRIM(_lst_exc1.Rule) IN ErrLog
*!*			Endscan		
*!*			_thismthodsuccessdone = .f.
*!*		Endif	
*!*		USE IN _lst_exc1
*!*	ELSE
*!*		_thismthodsuccessdone = .f.	
*!*	ENDIF

*!*	SELECT ALLTRIM(Left(chapno3,255)) As ChapNo,ALLTRIM(Left(Rule3,255)) As Rule,;
*!*		ALLTRIM(Left(Itemdes_a3,255)) As Itemdes,ALLTRIM(Left(SNoNotify3,255)) As SNoNotify,ALLTRIM(Left(TariffSNo3,255)) As TariffSNo FROM XmlData_vw ;
*!*		WHERE INLIST(TagGroup,'3') AND inli(Rule3,'CT-1','CT-3','EOU EXPORT') AND VAL(QtyClear3) > 0 INTO CURSOR _lst_exc1
*!*	IF USED('_lst_exc1')	
*!*		SELECT ChapNo,Rule,Itemdes,SNoNotify FROM _lst_exc1 ;
*!*			WHERE SNoNotify NOT in (SELECT DISTINCT NotifName FROM NonTariffNotif) INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				Scan
*!*					IF EMPTY(_lst_exc2.SNoNotify)
*!*						REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*							CHR(13)+'Notification No. can not be Blank' IN ErrLog
*!*					else
*!*						REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*							CHR(13)+'Unable to find Notification No. '+ALLTRIM(_lst_exc2.SNoNotify) IN ErrLog
*!*							
*!*					Endif	
*!*					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*						' for Chap. No. : '+ALLTRIM(_lst_exc2.ChapNo);
*!*						+ ', Item : '+ALLTRIM(_lst_exc2.Itemdes) +  ', Rule : '+ALLTRIM(_lst_exc2.Rule) IN ErrLog
*!*				Endscan		
*!*				_thismthodsuccessdone = .f.	
*!*			Endif	
*!*			USE IN _lst_exc2
*!*		ELSE
*!*			_thismthodsuccessdone = .f.	
*!*		ENDIF

*!*		SELECT ChapNo,Rule,Itemdes,TariffSNo FROM _lst_exc1 ;
*!*			WHERE EMPTY(TariffSNo) INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				Scan
*!*					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*						CHR(13)+'Notification Sr. No. can not be Blank' IN ErrLog
*!*					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*						' for Chap. No. : '+ALLTRIM(_lst_exc2.ChapNo);
*!*						+ ', Item : '+ALLTRIM(_lst_exc2.Itemdes) +  ', Rule : '+ALLTRIM(_lst_exc2.Rule) IN ErrLog
*!*				Endscan		
*!*				_thismthodsuccessdone = .f.	
*!*			Endif	
*!*			USE IN _lst_exc2
*!*		ELSE
*!*			_thismthodsuccessdone = .f.	
*!*		ENDIF
*!*		
*!*		USE IN _lst_exc1
*!*	ELSE
*!*		_thismthodsuccessdone = .f.	
*!*	ENDIF

*!*	SELECT ALLTRIM(Left(chapno3,255)) As ChapNo,ALLTRIM(Left(Rule3,255)) As Rule,;
*!*		ALLTRIM(Left(Itemdes_a3,255)) As Itemdes,ALLTRIM(Left(SNoNotify3,255)) As SNoNotify FROM XmlData_vw ;
*!*		WHERE INLIST(TagGroup,'3') AND !inli(Rule3,'CT-1','CT-3','EOU EXPORT') AND !EMPTY(SNoNotify3) INTO CURSOR _lst_exc1
*!*	IF USED('_lst_exc1')	
*!*		SELECT ChapNo,Rule,Itemdes,SNoNotify FROM _lst_exc1 ;
*!*			WHERE SNoNotify NOT in (SELECT DISTINCT NotifName FROM TariffNotif) INTO CURSOR _lst_exc2
*!*		IF USED('_lst_exc2')	
*!*			SELECT _lst_exc2
*!*			IF RECCOUNT() > 0
*!*				Scan
*!*					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*						CHR(13)+'Unable to find Notification No. '+ALLTRIM(_lst_exc2.SNoNotify) IN ErrLog
*!*					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
*!*						' for Chap. No. : '+ALLTRIM(_lst_exc2.ChapNo);
*!*						+ ', Item : '+ALLTRIM(_lst_exc2.Itemdes) +  ', Rule : '+ALLTRIM(_lst_exc2.Rule) IN ErrLog
*!*				Endscan		
*!*				_thismthodsuccessdone = .f.	
*!*			Endif	
*!*			USE IN _lst_exc2
*!*		ELSE
*!*			_thismthodsuccessdone = .f.	
*!*		ENDIF
*!*		USE IN _lst_exc1
*!*	ELSE
*!*		_thismthodsuccessdone = .f.	
*!*	ENDIF


&&Sandeep----->Start


Select XmlData_vw
Copy To abc
_thismthodsuccessdone = .T.

&& I------>Start

Select compstaxno,CompVatRcd1,CompVatRcd2 From XmlData_vw Where Inlist(taggroup ,'1') Into Cursor _cst_1


If Used('_CST_1')
	Select _cst_1

		if Empty(_cst_1.compstaxno)
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'C S T  No Not Entered' In ErrLog
			_thismthodsuccessdone = .F.
         endif  
		if Len(_cst_1.compstaxno)<>11

			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'C S T  No Not Entered' In ErrLog
			_thismthodsuccessdone = .F.
         endif
		if Left(_cst_1.compstaxno,2)<>'27'

			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'C S T  No Should Begin With 27' In ErrLog
			_thismthodsuccessdone = .F.
        endif 
		if check_onlynumber(_cst_1.compstaxno)=.F.
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'C S T  No Should be Number' In ErrLog
			_thismthodsuccessdone = .F.
        endif
		if !Empty(_cst_1.compstaxno)

			CK=0
			II=0
			II=Right(_cst_1.compstaxno,7)
			CK=Mod(Val(II),97 )

			II = CK * 10000 + 2700
			CK = 98 - Mod(II,97)
			II = Round(Val(Substr(_cst_1.compstaxno,3,2)),0)
			If CK <> II
				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'C S T  No is Invalid  Number' In ErrLog
				_thismthodsuccessdone = .F.
			ENDIF
	    endif		

		if Empt(_cst_1.CompVatRcd1) Or !Empt(_cst_1.CompVatRcd1)
			p_str=Ltrim((_cst_1.CompVatRcd1),1)

			If check_onlyalpha(p_str) = .F.

				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Saparate Return Sub Colomn 1 First Character Should be Alphabet' In ErrLog
				_thismthodsuccessdone = .F.
			Endif
         endif 

		if !Empt(_cst_1.CompVatRcd2)
			p_str=Ltrim((_cst_1.CompVatRcd2),1)

			If check_onlyalpha(p_str) = .T.

				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Saparate Return Sub Colomn No. Two. Second Character Should be Number' In ErrLog
				_thismthodsuccessdone = .F.
			ENDIF
		endif	
Endif
&& I------>End
&&II & III ----->Start

Select compvatloc,compemail,compphone From XmlData_vw Where Inlist(taggroup ,'2') Into Cursor _cst_2

If Used('_cst_2')
	Select _cst_2

		if Empty (Alltrim(_cst_2.compvatloc))
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Location of Sales tax dept Not Entered' In ErrLog
			_thismthodsuccessdone = .F.
         endif
		if Empty(Allt(_cst_2.compphone))
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Phone No Not Entered' In ErrLog
			_thismthodsuccessdone = .F.
         endif
		if Empty(Allt(_cst_2.compemail))

			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Invalid Email Id Entered' In ErrLog
			_thismthodsuccessdone = .F.
		 endif	
		if  !Empty(Allt(_cst_2.compemail))

			If check_email(_cst_2.compemail)=.F.
				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Email Id Field Not Entered' In ErrLog
				_thismthodsuccessdone = .F.
		 	Endif
        endif
Endif

&&II & III ----->End


*!*	RETURN _thismthodsuccessdone

&& V ----->Start

&& for V_1A-H & 2 Details Validation------->Start


Select Sum(Val(AmtSaleTurnover)) As groamt From XmlData_vw Where  taggroup='5 ' And hrdsrno='1 ' And Inlist(SrnoSalesTurnOver,'1') Into Cursor _Cst_3

If Used('_cst_3')
	Select _Cst_3
	Do Case
		Case Empty(groamt)
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Gross Turnover Not Entered' In ErrLog
			_thismthodsuccessdone = .F.
	Endcase
Endif

Select Sum(Val(AmtSaleTurnover)) As amtA_H  From XmlData_vw Where  taggroup='5 '  And hrdsrno='1 ' And Inlist(SrnoSalesTurnOver,'A','B','C','D','E','F','G','H') Into Cursor _Cst_4

If Used('_cst_4')
	Select _Cst_4
	Select Sum(Val(AmtSaleTurnover)) As amtA_H_1  From XmlData_vw Where  taggroup='5 '  And hrdsrno='2 ' And Inlist(SrnoSalesTurnOver,'2 ') Into Cursor _Cst_5
	If (_Cst_5.amtA_H_1) <> (_Cst_3.groamt-_Cst_4.amtA_H)
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Balance :-Inter_State sales  Box 2 is incorrect. It should be equal to Box( 1- 1A-1B-1C-1D-1E-1F-1G-1 H)' In ErrLog
		_thismthodsuccessdone = .F.
	Endif
Endif
&& for V_1A-H & 2  Details Validation------->End


&& for V_2 & 2A-C  Details Validation------->Start

Select Sum(Val(AmtSaleTurnover)) As AMTA_C  From XmlData_vw Where taggroup ='5 ' And hrdsrno='2 ' And Inlist(SrnoSalesTurnOver,'A ','B ','C ') Into Cursor _CST_6
If Used('_CST_6')
	Select _CST_6
	Select Sum(Val(AmtSaleTurnover)) As AMTA_D  From XmlData_vw Where taggroup ='5 ' And hrdsrno='2 ' And Inlist(SrnoSalesTurnOver,'2 ') Into Cursor _CST_7
	If Used('_CST_7')
		Select _CST_7
		If (_CST_7.AMTA_D<>_CST_6.AMTA_C)
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Balance :-Inter_State sales  Box 3 is incorrect. It should be equal to Box( 2- 2A-2B-2C )' In ErrLog
			_thismthodsuccessdone = .F.
		Endif
	Endif
Endif
&& for V_2 & 2A-C  Details Validation------->End


Select Sum(Val(AmtSaleTurnover)) As amtA_8  From XmlData_vw Where  taggroup='5 '  And hrdsrno='1 ' And Inlist(SrnoSalesTurnOver,'A ') Into Cursor _Cst_8
If Used('_CST_8')
	Select _Cst_8
	Select Sum(Val(AmtSaleTurnover)) As amtC_9  From XmlData_vw Where  taggroup='5 '  And hrdsrno='1 ' And Inlist(SrnoSalesTurnOver,'C ') Into Cursor _Cst_9
	If Used('_CST_9')
		Select _Cst_9
		If Empty(_Cst_9.amtC_9)
			Select Sum(Val(AmtSaleTurnover)) As amtB_G_10  From XmlData_vw Where  taggroup='5 '  And hrdsrno ='1 ' And Inlist(SrnoSalesTurnOver,'B ','D ','E ','F ','G ') Into Cursor _Cst_10
			If Used('_CST_10')
				Select _Cst_10
				Select Sum(Val(AmtSaleTurnover)) As amtA_C_11  From XmlData_vw Where  taggroup='5 '  And hrdsrno ='2 ' And Inlist(SrnoSalesTurnOver,'A ','B ','C ') Into Cursor _Cst_11
				If Used('_CST_11')
					Select _Cst_11
					Select Sum(Val(AmtSaleTurnover)) As amt3_12  From XmlData_vw Where  taggroup='5 '  And hrdsrno ='3 ' And Inlist(SrnoSalesTurnOver,'3 ') Into Cursor _Cst_12
					If Used('_CST_12')
						Select _Cst_12
						If (_Cst_3.groamt-_Cst_8.amtA_8) <  (_Cst_10.amtB_G_10+_Cst_11.amtA_C_11+_Cst_12.amt3_12)
							Replace ErrMsg With ErrLog.ErrMsg +;
								CHR(13)+'Goods Returns is Null or Zero. The Total of Deductions ( 1 B +1 D+ 1E+1F+1G+1H + 2A+2B+2C+3(A) ) should be Less Than Or Equal to (GTOin Box 1 - Less Local Sales in Box (1A)).Please Check  enter Correct amount of Deductions Or Goods Returns Or GTO Or Sales' In ErrLog
							_thismthodsuccessdone = .F.
						Endif
					Endif
				Endif
			Endif
		Endif
	Endif
Endif


If Empty(_Cst_9.amtC_9)
	If  (_Cst_10.amtB_G_10+_Cst_12.amt3_12 ) < 0
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'If Goods Returns In Box 1 C  is Null or Zero. Negative value for any of the deductions in Box   1 B  Or 1 D Or  1E Or 1 F Or 1G Or 1H Or 2A Or 2B Or 2C Or 3(A)  entered. Please enter correct amount of deductions or Goods Returns.' In ErrLog
		_thismthodsuccessdone = .F.
	Endif
Endif

Select Sum(Val(AmtSaleTurnover)) As amtB_14  From XmlData_vw Where  taggroup='5 '  And hrdsrno ='1 ' And Inlist(SrnoSalesTurnOver,'B ') Into Cursor _Cst_14

If ((_Cst_3.groamt -_Cst_14.amtB_14 )=0 And _Cst_9.amtC_9 > 0 )
	If Abs (_Cst_3.groamt -_Cst_14.amtB_14 -_Cst_9.amtC_9) < Abs(_Cst_10._AMTB_G_10+_Cst_11.amtA_C_11+_Cst_12.amt3_12)
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'InterStaes Sales ( Box 1 A- Box 1 B) is zero.   Goods Returns In Box 1 C  ' ;
			+'greater than Zero. Absolute value of total of Deductions in Box   1 B  Or 1 D Or  1E Or 1 F Or 1G Or 1H Or 2A Or 2B Or 2C Or 3(A) should be Less Than or Equal to Absolute value (  Goods Returns in Box 1 C) Please check Deductions ';
			+', Goods Returns , GTO or Local Sales . ' In ErrLog
		_thismthodsuccessdone = .F.
	Endif
Endif

&& V ----->End

Select  Sum(Val(AmtSaleTurnover)) As amt2d_15   From XmlData_vw Where  taggroup='5 '  And  Inlist (hrdsrno,'2 ') And Inlist (SrnoSalesTurnOver,'D ')Into Cursor _Cst_15
If Used('_cst_15')
	Select _Cst_15
	Select  Sum(Val(AmtSaleTurnover)) As amt44_16   From XmlData_vw Where  taggroup='5 '  And  Inlist (hrdsrno,'2 ') And Inlist (SrnoSalesTurnOver,'D ')Into Cursor _Cst_16
	If Used('_cst_16')
		Select _Cst_16
		If _Cst_15.amt2d_15<>(_Cst_16.amt44_16-_Cst_12.amt3_12)
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Net Taxable interstate sales in  Box 4 is incorrect. It should be equal to Box ( 3 - 3A).' In ErrLog
			_thismthodsuccessdone = .F.
		Endif
	Endif
Endif

&& A.sales Taxable u/s 8(1) --> Start

Select SalesTaxRate,AmtSaleTaxable,SalesTaxAmt From XmlData_vw Where taggroup ='5A ' And SalesTaxRate <>'Total' Into Cursor ASalTax_8_1
If Used('ASalTax_8_1')
	Select ASalTax_8_1
	If Reccount() > 0
		Scan
			If !Empty (ASalTax_8_1.AmtSaleTaxable) Or !Empty(ASalTax_8_1.SalesTaxRate)
				If Empty (ASalTax_8_1.AmtSaleTaxable) Or Empty(ASalTax_8_1.SalesTaxRate)
					Replace ErrMsg With ErrLog.ErrMsg +;
						CHR(13)+'Check And Enter either Rate Of Tax or Sales TurnOver' In ErrLog
					_thismthodsuccessdone = .F.
				Endif
			Endif

		Endscan
	Endif
	Use In ASalTax_8_1
Else
	_thismthodsuccessdone = .F.
Endif

POS=0
NEG=0
i=0
Select SalesTaxRate,AmtSaleTaxable,SalesTaxAmt From XmlData_vw Where taggroup ='5A ' And SalesTaxRate <>'Total' Into Cursor ASalTax_8_1
If Used('ASalTax_8_1')
	Select ASalTax_8_1
	If Reccount() > 0
		Scan
			If Val(ASalTax_8_1.AmtSaleTaxable) > 0
				POS=POS+Val(ASalTax_8_1.AmtSaleTaxable)
			Else
				NEG=NEG+Val(ASalTax_8_1.AmtSaleTaxable)

			Endif
			i=i+1
		Endscan
	Endif
	Use In ASalTax_8_1
	Do Case
		Case (_Cst_3.groamt-_Cst_14.amtB_14) < POS

			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'[Total of all positive net  Turnovers Of Sales at  Boxes 4A,4B,4C  = 0r <  [Gross Turnover of sales] at Box I-IA' In ErrLog
			_thismthodsuccessdone = .F.

		Case (_Cst_9.amtC_9 < Abs(NEG))

			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Absolute Value of [Total Negitive net Turnover Of Sales at Box 4A,4B,4C ]= 0r <  Goods Return including Reduction Of sales at Box I(C)' In ErrLog
			_thismthodsuccessdone = .F.
	Endcase
Endif



Select Sum(Val(AmtSaleTaxable)) As SALTUNAMT,Sum(Val(SalesTaxAmt)) As SALTAXAMT From XmlData_vw Where taggroup ='5A ' And SalesTaxRate <>'Total' Into Cursor ASalTax_81
If Used('ASalTax_81')
	Select('ASalTax_81')
	Select Sum(Val(AmtSaleTaxable)) As SALTUNAMT,Sum(Val(SalesTaxAmt)) As SALTAXAMT From XmlData_vw Where taggroup ='5A ' And SalesTaxRate ='Total' Into Cursor ASalTaxT_81
	If Used('ASalTaxT_81')
		Select('ASalTaxT_81')
		Do Case

			Case (ASalTax_81.SALTUNAMT<>ASalTaxT_81.SALTUNAMT)
				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Check And Enter either Rate Of Tax or Sales TurnOver' In ErrLog
				_thismthodsuccessdone = .F.

			Case (ASalTax_81.SALTAXAMT<>ASalTaxT_81.SALTAXAMT)
				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Total of Tax Amount in  Box 4Ais incorrect, It should be equal to Sum of Tax Amount  at (1) to (5) in Box 4A' In ErrLog
				_thismthodsuccessdone = .F.

		Endcase
	Endif
Endif
&& A.sales Taxable u/s 8(1) --> End

&& Sr.No.5---------->Start
Select Val(AmtTotPayable)As Amt5_5 From XmlData_vw Where  HrdTotPayable='Total collected in excess of the payable (3A - total tax 4(A+B+C) if positive' And SrNoPayable='5 ' And !Empty(SrNoPayable) Into Cursor _Cst_55
If Used('_Cst_55')
	Select('_Cst_55')
	If (_Cst_12.amt3_12 -ASalTaxT_81.SALTAXAMT ) > 0
		If _Cst_55.Amt5_5 <>(_Cst_12.amt3_12-ASalTaxT_81.SALTAXAMT)
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Tax Collected in excess in Box 5 is incorrect. It should be equal to [ Box( 3A)  –Total tax 4( A+B+C)] if positive, else zero.' In ErrLog
			_thismthodsuccessdone = .F.

		Endif
	Endif
Endif
&& Sr.No.5---------->End

&& Sr.No.6---------->Start

Select Val(AmtTotPayable)As Amt6_6 From XmlData_vw Where  SrNoPayable ='6 ' And taggroup='5 ' And !Empt(SrNoPayable )     Into Cursor _Cst_66
If Used('_Cst_66')
	Select('_Cst_66')
	If (_Cst_66.Amt6_6<>ASalTaxT_81.SALTAXAMT)
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Total Amount of C.S.T Payable in  Box 6 is incorrect.It should be equal to Total of  tax Amount in Box 4( A+B+C)] ' In ErrLog
		_thismthodsuccessdone = .F.

	Endif
Endif
&& Sr.No.6---------->End
&& Sr.No.7&8---------->Start

Select Val(AmtTotPayable)As Amt7_7 From XmlData_vw Where  SrNoPayable ='7 ' And taggroup='5 ' And !Empt(SrNoPayable )     Into Cursor _Cst_77
If Used('_Cst_77')
	Select('_Cst_77')

	Select Val(AmtTotPayable)As Amt8_8 From XmlData_vw Where  SrNoPayable ='8 ' And taggroup='5 ' And !Empt(SrNoPayable )     Into Cursor _Cst_88
	If Used('_Cst_88')
		Select('_Cst_88')

		Select Val(AmtTotPayable)As Amt9_9 From XmlData_vw Where  SrNoPayable ='9 ' And taggroup='5 ' And !Empt(SrNoPayable )     Into Cursor _Cst_99
		If Used('_Cst_99')
			Select('_Cst_99')

			If  (_Cst_77.Amt7_7-_Cst_88.Amt8_8) > 0
				If (_Cst_99.Amt9_9)<>(_Cst_77.Amt7_7-_Cst_88.Amt8_8)
					Replace ErrMsg With ErrLog.ErrMsg +;
						CHR(13)+'Balance Amount of C.S.T Payable in  Box 8 is incorrect.It should be equal to [ Tax Amount in Box 6- Amount Deffered in Box 7]' In ErrLog
					_thismthodsuccessdone = .F.
				Endif
			Endif
		Endif
	Endif
Endif
&& Sr.No.7&8---------->End

&& Sr.No.9&10---------->Start
Select Val(AmtTotPayable)As Amt10_10 From XmlData_vw Where  SrNoPayable ='10' And taggroup='5 ' And !Empt(SrNoPayable )     Into Cursor _Cst_1010
If Used('_cst_1010')
	Select _Cst_1010
	Select Val(AmtTotPayable)As Amt9_9b From XmlData_vw Where  SrNoPayable ='9' And HrdTotPayable='(b) Add:- Amount Payable against excess collection if any, as per Box-5' And taggroup='5 ' And !Empt(SrNoPayable )     Into Cursor _Cst_99b
	If Used('_cst_99b')
		Select _Cst_99b
		If (_Cst_1010.Amt10_10)<>(_Cst_88.Amt8_8+_Cst_99.Amt9_9+_Cst_99b.Amt9_9b)
			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Total Amount Payable in  Box 10 is incorrect.It should be equal to Total of   Amount in ( Box 8 + Box 9(a) + Box 9(b))' In ErrLog
			_thismthodsuccessdone = .F.
		Endif
	Endif
Endif


&& Sr.No.9&10---------->End

&& Sr.No.10---------->Start


Select Sum(Val(AmtTotPayable)) As AMT10_10A From XmlData_vw Where hrdsrnod='10' And  SrNoPayable In ('a ','b ','c ','d ') And !Empty(SrNoPayable) Into Cursor _Cst_1010A
If Used('_cst_1010A')
	Select _Cst_1010A
	Select Sum(Val(AmtTotPayable)) As AMT11_11 From XmlData_vw Where hrdsrnod='11' And  SrNoPayable In ('11') And !Empty(SrNoPayable) Into Cursor _Cst_1111
	If Used('_cst_1111')
		Select _Cst_1111
		If(_Cst_1010A.AMT10_10A - _Cst_1010.Amt10_10 ) > 0
			If (_Cst_1111.AMT11_11)<>(_Cst_1010A.AMT10_10A)
				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Balance: Excess credit as per Box 11 is incorrect . It should be [10(a)+10(b)+10(c)+10(d)] -Amount  10 if it is positive ' In ErrLog
				_thismthodsuccessdone = .F.
			Endif
		Else
			If (_Cst_1010A.AMT10_10A - _Cst_1010.Amt10_10 ) <= 0
				If Empty(_Cst_1111.AMT11_11) Or Empt(_Cst_1111.AMT11_11)
					Replace ErrMsg With ErrLog.ErrMsg +;
						CHR(13)+'Balance: Excess credit as per Box 11 is incorrect . It should be [10(a)+10(b)+10(c)+10(d)] -Amount  10 if it is positive ' In ErrLog
					_thismthodsuccessdone = .F.

				Endif
			Endif
		Endif
	Endif
Endif

&& Sr.No.10---------->End

&& Sr.No.11---------->Start

Select Sum(Val(AmtTotPayable)) As AMT11_11A From XmlData_vw Where hrdsrnod='11' And  SrNoPayable In ('a ') And !Empty(SrNoPayable) Into Cursor _Cst_1111A
If Used('_cst_1111A')
	Select _Cst_1111A

	If (_Cst_1111A.AMT11_11A > _Cst_1111.AMT11_11)

		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Total at Box 11(a) Should Be Less Than Or Equal To amount of Excess Credit at Box 11' In ErrLog
		_thismthodsuccessdone = .F.

	Endif
Endif

Select Sum(Val(AmtTotPayable)) As AMT11_11Ab From XmlData_vw Where hrdsrnod='11' And  SrNoPayable In ('a ','b') And !Empty(SrNoPayable) Into Cursor _Cst_1111AB
If Used('_cst_1111AB')
	Select _Cst_1111AB
	If (_Cst_1111AB.AMT11_11Ab) <> (_Cst_1111.AMT11_11)
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Excess Credit as per Box 11 should be equal to total of  Credit as per Box 11(a) and Credit as per Box 11( b)' In ErrLog
		_thismthodsuccessdone = .F.

	Endif
Endif


&& Sr.No.11---------->End

&& Sr.No.12---------->Start
Select Sum(Val(AmtTotPayable)) As AMT12_12 From XmlData_vw Where hrdsrnod='12' And  SrNoPayable In ('12') And !Empty(SrNoPayable) Into Cursor _Cst_1212
If (_Cst_1010.Amt10_10 -_Cst_1010A.AMT10_10A) > 0
	If (_Cst_1212.AMT12_12<>(_Cst_1010.Amt10_10 -_Cst_1010A.AMT10_10A))
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Balance:Amount payable as per Box 12 is incorrect.t should be { Amount Box 10 - [10(a)+10(b)+10(c)+10(d)] }  if it is positive ' In ErrLog
		_thismthodsuccessdone = .F.
	Endif
Endif

&& Sr.No.12---------->End
&& Sr.No.12a&B&C--------->Start

Select Sum(Val(AmtTotPayable)) As AMT10_10c From XmlData_vw Where hrdsrnod='10' And  SrNoPayable In ('c ') And !Empty(SrNoPayable)  Into Cursor _cst10c
If Used('_cst10c')
	Select _cst10c

	Select Sum(Val(AmtTotPayable)) As AMT12_a From XmlData_vw Where hrdsrnod='12' And  SrNoPayable In ('a')  And !Empty(SrNoPayable) Into Cursor _Cst12a
	If Used('_cst12a')
		Select _Cst12a

		Select Sum(Val(Bank_amt)) As AMTbk12_c  From XmlData_vw Where taggroup='5C' And challan_no<>'Total' Into Cursor _Cst12c
		If Used('_cst12c')
			Select _Cst12c

			If (_cst10c.AMT10_10c+_Cst12a.AMT12_a)<>_Cst12c.AMTbk12_c
				Replace ErrMsg With ErrLog.ErrMsg +;
					CHR(13)+'Total Challans Amount Should Be equal to Total of Amount Already paid as per Box 10 ( c ) and Amount Paid as per Box 12 (a) ' In ErrLog
				_thismthodsuccessdone = .F.
			Endif
		Endif
	Endif
Endif


&& Sr.No.12a&B&C--------->End

&& Sr.No.12C--------->Start

Select challan_no,challan_dt,bank_name,bank_br  From XmlData_vw Where taggroup='5C' And challan_no<>'Total' Into Cursor _Cst12Cc
If Used('_cst12cc')
	Select _Cst12Cc
	If Reccount() > 0
		Scan
			If !Empty(_Cst12c.AMTbk12_c) Or !Empty(challan_no) Or !Empty(challan_dt) Or !Empty(bank_name) Or !Empty(bank_br)
				If Empty(_Cst12c.AMTbk12_c) Or Empty(challan_no) Or Empty(challan_dt) Or Empty(bank_name) Or Empty(bank_br)
					Replace ErrMsg With ErrLog.ErrMsg +;
						CHR(13)+'Check And Enter Challan no or CIN no,Amount, Challan Date,Submited Bank,Submited Branch ' In ErrLog
					_thismthodsuccessdone = .F.
				Endif
			Endif

		Endscan
	Endif
	Use In _Cst12Cc
*!*	Else
*!*		_thismthodsuccessdone = .F.
Endif



&& Sr.No.12C--------->End

&& Sr.No.12E--------->Start                   

Select CompVatCity,CompVatPName,CompVatPDesig,CompVatPemail,CompVatpphone From XmlData_vw Where taggroup='5E' Into Cursor _Cst12E


If Used('_cst12E')
	Select _Cst12E


	If Empty(_Cst12E.CompVatCity)
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Place Name Not Entered ' In ErrLog
		_thismthodsuccessdone = .F.

	Endif
	If Empty(_Cst12E.CompVatPname)
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Enter Name in Name ' In ErrLog
		_thismthodsuccessdone = .F.

	Endif
	If check_onlyalpha(_Cst12E.CompVatPname)=.F.
		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Name Should have only Alphabets & Space' In ErrLog
		_thismthodsuccessdone = .F.
	Endif


	If !Empty(_Cst12E.compvatpemail)

		If  check_email (_Cst12E.compvatpemail)=.F.		


			Replace ErrMsg With ErrLog.ErrMsg +;
				CHR(13)+'Invalid Email Id Entered' In ErrLog
			_thismthodsuccessdone = .F.
		Endif
	Endif	


	If Empty(_Cst12E.CompVatPdesig)

		Replace ErrMsg With ErrLog.ErrMsg +;
			CHR(13)+'Enter Name in Name & Designation Field' In ErrLog
		_thismthodsuccessdone = .F.
	ENDIF

Endif
&& Sr.No.12E--------->End

RETURN _thismthodsuccessdone

Function check_email
	Lparameters p_str
	s_er =0
	ia =0
	ina =0
	strlen =0
	atcount =0
	i =0
	er =0
	stlen =0
	check_email_1 = legal_char_email(p_str)
    
    
	If check_email_1 =.F.
		Return check_email_1
	Else

		atcount = At('@',p_str,1)
		If atcount = 0 Or atcount = 1
			Return .F.
		Endif
		atcount = At('.',p_str,1)
		If atcount = 0 Or atcount = 1
			Return .F.
		Endif
		atcount = At('-',p_str,1)
		If atcount = 1
			Return .F.
		Endif
		atcount = At('@@',p_str,1)
		If atcount <> 0
			Return .F.

		Endif
		atcount = At('..',p_str,1)
		If atcount <> 0
			Return .F.

		Endif
		atcount = At('--',p_str,1)
		If atcount <> 0
			Return .F.

		Endif
		atcount = At('__',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('@.',p_str,1)
		If atcount <> 0
			Return .F.

		Endif
		atcount = At('@-',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('@_',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('.@',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('.-',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('._',p_str,1)
		If atcount <> 0
			Return .F.

		Endif
		atcount = At('-@',p_str,1)

		If atcount <> 0
			Return .F.
		Endif
		atcount = At('-.',p_str,1)
		If atcount <> 0 Then
			Return .F.
		Endif
		atcount = At('-_',p_str,1)
		If atcount <> 0 Then
			Return .F.
		Endif
		atcount =At('_@',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('_.',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		atcount = At('_-',p_str,1)
		If atcount <> 0
			Return .F.
		Endif
		If Right(p_str, 1) = '@'
			Return .F.
		Endif
		If Right(p_str, 1) = '.'
			Return .F.
		Endif
		If Right(p_str, 1) = '_'
			Return .F.
		Endif
		If Right(p_str, 1) = '-'
			Return .F.
		Endif
		Return .T.
	Endif

Function check_onlynumber
	Lparameters p_str1

	stlen = Len(p_str1)
	For i =1 To stlen
		Do Case

			Case Substr(p_str1,i,1) >='0' And substr(p_str1, i, 1)<= '9'
			OTHERWISE
				RETURN .f.	
		ENDCASE 		
	endfor
       RETU .T.

Function check_onlyalpha
	Lparameters p_str

	stlen = Len(p_str)
	ia = 0
	ina = 0
	For i =1 To stlen
		Do Case
			Case Upper(Substr(p_str,i,1)) >='A' And Upper(Substr(p_str, i, 1))<= 'Z'
				ia = ia + 1
			Case Lower(Substr(p_str, i, 1)) >='a' And Lower(Substr(p_str, i, 1))<= 'z'
				ia = ia + 1
			CASE Substr(p_str, i, 1) = ' '			
				ia = ia + 1
			Otherwise
				ina = ina + 1
                  
		Endcase
	Next i
	If ina =0
		Retu .T.
	Else
		Return  .F.
	Endif

Function legal_char_email

	Lparameters p_string
	i =0
	legal_char_email =.T.
	If At('@',p_string,1)>0 And At('.',p_string,1)>0
	Else
		Return .F.
	ENDIF

	For i = 1 To Len(p_string)
		Do Case
	    
			Case (Upper(Substr(p_string, i, 1))) >='A' And (Upper(Substr(p_string, i, 1)))<= 'Z'

			Case  Substr(p_string, i, 1)>='0' And Substr(p_string, i, 1)<= '9'

			Case Substr(p_string, i, 1)='@'

			Case Substr(p_string, i, 1)='_'

			Case Substr(p_string, i, 1)='.'

			Case Substr(p_string, i, 1)='-'

			Otherwise
					legal_char_email =.F.
		Endcase
	Next i

	Return legal_char_email

	&&&Sandeep

	If !Empty(_oldAlias)
		Select (_oldAlias)
	Endif
	Return _thismthodsuccessdone

	&&&Sandeep<------End



