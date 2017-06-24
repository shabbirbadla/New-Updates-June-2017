&&Changes has been done as per TKT-6470 (Multilanguage support - Tested with English & Japanese Language) on 24/02/2011
*!*	&&vasant16/11/2010
*!*	LPARAMETERS _ltent1,_ltDatasessionid
*!*	*_ltDatasessionid = _Screen.Activeform.DataSessionId
*!*	Set DataSession To _ltDatasessionid
*!*	SET DELETED ON 
*!*	_ltent  = _ltent1
*!*	_UnqVal = .f.
*!*	_UnqVal = GlobalObj.GetPropertyVal('UnqVal')
*!*	IF TYPE('ueReadRegMe.UnqVal') <> 'N' AND TYPE('_UnqVal') <> 'C'
*!*		RETURN .f.
*!*	ELSE
*!*		_UnqVal = SUBSTR(Dec(_UnqVal),2,8)
*!*		IF (ueReadRegMe.UnqVal != (Val(SUBSTR(_UnqVal,2,1)+SUBSTR(_UnqVal,1,1)+SUBSTR(_UnqVal,4,1)+SUBSTR(_UnqVal,3,1)+SUBSTR(_UnqVal,6,1)+SUBSTR(_UnqVal,5,1)+SUBSTR(_UnqVal,8,1)+SUBSTR(_UnqVal,7,1)) * 3))
*!*			RETURN .f.
*!*		Endif	
*!*	Endif
*!*	Private usquarepass,mudprodcode
*!*	usquarepass = Upper(DEC(NewDecry(GlobalObj.GetPropertyVal('EncryptId'),'Ud*_yog*\+1993')))
*!*	mudprodcode = dec(NewDecry(GlobalObj.getPropertyval("UdProdCode"),'Ud*yog+1993'))

*!*	If !INLIST(UPPER(mudprodcode),'USQUARE','ITAX','VUDYOGSDK','VUDYOGMFG','VUDYOGTRD','VUDYOGSERVICETAX')
*!*		LOCAL sqlconobj,_menuretval,_menuok,_macid,_mregname
*!*		_mregname = ''
*!*		_macid = ''
*!*		_mregname = ALLTRIM(ueReadRegMe.r_comp)
*!*		_macid 	  = ALLTRIM(ueReadRegMe.r_macid)

*!*		sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
*!*		_menunHandle = 0
*!*		_menuretval  = 0
*!*		_menuok	     = 'Error'

*!*		msqlstr = "select convert(varchar(max),convert(varchar(max),decryptbypassphrase(?usquarepass,cd))) as cd,cd as cd1 from lcode"	&&vasant16/11/2010a
*!*		msqlstr = msqlstr + " where entry_ty = ?_ltent"
*!*		_menuretval =sqlconobj.dataconn('EXE',company.dbname,msqlstr,"dbfmenu","_menunHandle",_ltDatasessionid)
*!*		IF _menuretval > 0
*!*			SELECT dbfmenu
*!*			DO Case
*!*			CASE RECCOUNT() = 0
*!*				_menuok = 'No Records Found'
*!*			CASE RECCOUNT() > 1
*!*				_menuok = 'Multiple Menus Found'
*!*			OTHERWISE 	
*!*				msqlstr = NewDecry(dbfmenu.cd1,_macid)
*!*				IF LEFT(msqlstr,9) == 'CUST:TRAN'
*!*					_menuok = ''
*!*				ELSE
*!*					_featureid = ''
*!*					msqlstr = NewDecry(dbfmenu.cd,'Udencyogprod')
*!*					_stlen = AT('~*0*~',msqlstr)
*!*					_enlen = AT('~*1*~',msqlstr)
*!*					_featureid  = NewEncry(ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(msqlstr,_stlen + 5,_enlen-(_stlen+5)),''))+'~*0*~'+mudprodcode,'Udencyogprod')
*!*					msqlstr = "Select Enc,fEnc from Vudyog..ProdDetail"
*!*					_menuretval =sqlconobj.dataconn('EXE',company.dbname,msqlstr,"dbfmenu","_menunHandle",_ltDatasessionid)
*!*					IF _menuretval > 0
*!*						_menuok = 'No Records Found'
*!*						DELETE FROM dbfmenu WHERE fenc != _featureid
*!*						SELECT dbfmenu
*!*						GO Top

*!*						UPDATE dbfmenu SET Enc = NewDecry(dbfmenu.Enc,'Udencyogprod')
*!*						SELECT dbfmenu
*!*						GO Top

*!*						SELECT dbfmenu
*!*						SCAN
*!*						
*!*							_decdata = dbfmenu.Enc
*!*							_stlen = 1
*!*							_enlen = AT('~*0*~',_decdata)
*!*							_optiontype 	= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen,_enlen-_stlen),''))
*!*							_stlen = AT('~*0*~',_decdata)
*!*							_enlen = AT('~*1*~',_decdata)
*!*							_featureid  	= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							_stlen = AT('~*1*~',_decdata)
*!*							_enlen = AT('~*2*~',_decdata)
*!*							_subfeatureid  	= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							_stlen = AT('~*2*~',_decdata)
*!*							_enlen = AT('~*3*~',_decdata)
*!*							_prodcode  		= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							_stlen = AT('~*3*~',_decdata)
*!*							_enlen = AT('~*4*~',_decdata)
*!*							_prodver  		= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							_stlen = AT('~*4*~',_decdata)
*!*							_enlen = AT('~*5*~',_decdata)
*!*							_servicever  	= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							_stlen = AT('~*5*~',_decdata)
*!*							_enlen = AT('~*6*~',_decdata)
*!*							_featuretype  	= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							_stlen = AT('~*6*~',_decdata)
*!*							_enlen = AT('~*7*~',_decdata)
*!*							_optionname  	= ALLTRIM(IIF(_stlen > 0 And _enlen > 0,SUBSTR(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
*!*							
*!*							_newoption		= NewEncry(_featureid+'~*0*~'+_subfeatureid+'~*1*~'+_optionname+'~*2*~','Udencyogprod')

*!*							IF _optiontype = 'TRANSACTION' AND (_prodcode $ vchkprod OR _prodcode = 'vugen') AND mudprodcode == _prodver ;
*!*								AND UPPER(GlobalObj.getPropertyval("ClientType")) == UPPER(_servicever)	;
*!*								AND UPPER(_ltent) == UPPER(_optionname)	
*!*								DO Case	
*!*								Case UPPER(_featuretype) == 'FREE'
*!*									_menuok	     = ''
*!*									EXIT
*!*								Case UPPER(_featuretype) == 'PREMIUM'
*!*									_menuok = 'Kindly Subscribe for this option'			
*!*									_newoption		= NewEncry(_mregname+'~*0*~'+_macid+'~*1*~'+_featureid+'~*2*~'+ALLTRIM(company.co_name),_macid)	
*!*									msqlstr = "select enc from clientfeature"
*!*									_menuretval =sqlconobj.dataconn('EXE',company.dbname,msqlstr,"_dbfmenu","_menunHandle",_ltDatasessionid)
*!*									IF _menuretval > 0
*!*										SELECT _dbfmenu
*!*										DELETE FROM _dbfmenu WHERE enc != _newoption
*!*										Scan
*!*											_menuok	     = ''
*!*											Exit
*!*										EndScan
*!*									Endif	
*!*									EXIT
*!*								Endcase
*!*							ENDIF
*!*							
*!*							SELECT dbfmenu
*!*						Endscan
*!*					Endif				
*!*				ENDIF 	
*!*			ENDCASE 
*!*		ENDIF

*!*		=sqlconobj.sqlconnclose("_menunHandle")
*!*		RELEASE _menunHandle,_menuretval,usquarepass,mudprodcode
*!*		RELEASE _decdata,_optiontype,_featureid,_subfeatureid,_prodcode,_prodver,_servicever,_featuretype,_optionname,_newoption
*!*		IF USED("dbfmenu")		
*!*			USE IN dbfmenu
*!*		Endif	
*!*		IF USED("_dbfmenu")		
*!*			USE IN _dbfmenu
*!*		Endif	
*!*		IF !EMPTY(_menuok)
*!*			=Messagebox(_menuok,64,vumess)
*!*			RETURN .f.
*!*		Endif
*!*	ENDIF
*!*	RETURN .t.
*!*	&&vasant16/11/2010
&&Changes has been done as per TKT-6470 (Multilanguage support - Tested with English & Japanese Language) on 24/02/2011