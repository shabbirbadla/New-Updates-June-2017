&&Changes has been done as per TKT-6470 (Multilanguage support - Tested with English & Japanese Language) on 24/02/2011
*!*	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
*!*	LPARAMETERS _ltgrp1,_ltdesc1,_ltrep1,_ltDatasessionid
*!*	*_ltDatasessionid = _Screen.Activeform.DataSessionId
*!*	Set DataSession To _ltDatasessionid
*!*	SET DELETED ON 
*!*	_ltgrp  = _ltgrp1
*!*	_ltdesc = _ltdesc1
*!*	_ltrep  = _ltrep1
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

*!*		msqlstr = "select convert(varchar(max),convert(varchar(max),decryptbypassphrase(?usquarepass,newgroup))) as newgroup,newgroup as newgroup1 from r_status"	&&vasant16/11/2010a	Changes done for VU 10 (Standard/Professional/Enterprise)
*!*		msqlstr = msqlstr + " where [group] = ?_ltgrp and [desc] = ?_ltdesc and rep_nm = ?_ltrep"
*!*		_menuretval =sqlconobj.dataconn('EXE',company.dbname,msqlstr,"dbfmenu","_menunHandle",_ltDatasessionid)
*!*		IF _menuretval > 0
*!*			SELECT dbfmenu
*!*			DO Case
*!*			CASE RECCOUNT() = 0
*!*				_menuok = 'No Records Found'
*!*			CASE RECCOUNT() > 1
*!*				_menuok = 'Multiple Menus Found'
*!*			OTHERWISE 	
*!*				msqlstr = NewDecry(dbfmenu.newgroup1,_macid)
*!*				IF LEFT(msqlstr,9) == 'CUST:REPORT'
*!*					_menuok = ''
*!*				ELSE
*!*					_featureid = ''
*!*					msqlstr = NewDecry(dbfmenu.newgroup,'Udencyogprod')
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

*!*							IF _optiontype = 'REPORT' AND (_prodcode $ vchkprod OR _prodcode = 'vugen') AND mudprodcode == _prodver ;
*!*								AND UPPER(GlobalObj.getPropertyval("ClientType")) == UPPER(_servicever)	;
*!*								AND UPPER(ALLTRIM(_ltgrp)+ALLTRIM(_ltdesc)+ALLTRIM(_ltrep)) == UPPER(_optionname)	
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
*!*	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
&&Changes has been done as per TKT-6470 (Multilanguage support - Tested with English & Japanese Language) on 24/02/2011