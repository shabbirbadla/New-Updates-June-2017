****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
*_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER] Commented By Amrendra for TKT 8543 on 06/09/2011
Try
	_VerRetVal = AppVerChk('UPDATEPKG',GetFileVersion(),Justfname(Sys(16))) && Added Getfileversion() as para in this statement for TKT 8543 on 06/09/2011 by Amrendra
Catch To _VerValidErr
	_VerRetVal  = 'NO'
Endtry
If Type("_VerRetVal")="L"
	cMsgStr="Version Error occured!"
	cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
	Messagebox(cMsgStr,64,VuMess)
	Return .F.
Endif
If _VerRetVal  = 'NO'
	Return .F.
Endif
****Versioning****
&&Changes has been done as per TKT-6470 (Multilanguage support - Tested with English & Japanese Language) on 24/02/2011
Wait Window 'Getting Details..........' Nowait

_LogFileName = ''		&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
Private usquarepass,mudprodcode
cCaption = GlobalObj.getPropertyval("ProductTitle") 		&&Added For Bug-2286 USquare 10.0 Installer : By Amrendra on 18-02-2012
usquarepass = Upper(DEC(NewDecry(GlobalObj.getPropertyval('EncryptId'),'Ud*_yog*\+1993')))
mudprodcode = DEC(NewDecry(GlobalObj.getPropertyval("UdProdCode"),'Ud*yog+1993'))
nretval=0
nhandle=0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

Wait Window 'Updating Details..........' Nowait

msqlstr = "select top 1 b.name from sysobjects a,syscolumns b where a.id = b.id and a.name = 'com_menu' and b.name = 'newrange'"
nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")
If nretval<=0
	Return .F.
Endif

*WAIT WINDOW 'Updating Details for '+UPPER(ALLTRIM(mudprodcode)) NOWAIT &&Commented For Bug-2286 USquare 10.0 Installer : By Amrendra on 18-02-2012
Wait Window 'Updating Details for '+Alltrim(cCaption) Nowait  &&Changed For Bug-2286 USquare 10.0 Installer : By Amrendra on 18-02-2012
Select nedata
If Reccount() < 1
	msqlstr = "alter table com_menu add newrange varbinary(254)"
	nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")

	msqlstr = "alter table r_status add newgroup varbinary(254)"
	nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")
	If Alltrim(Upper(_co_mast.com_type)) <> 'M' &&Birendra:20 Apr 2011 Multi Comp TKT-310 - Start
		msqlstr = "alter table lcode alter column cd varbinary(254)"
		nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")
	Endif	&&Birendra Multi Comp TKT-310 - End
Endif
Do Case
Case Inlist(Upper(Alltrim(mudprodcode)),'ITAX','USQUARE','VUDYOGSDK','VUDYOGGSSDK')			&& Changed by Sachin N. S. on 27/01/2017 for GST
	msqlstr = "Select CAST(' ' as varchar(250)) as enc,padname,barname,range from com_menu"
	nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"commenu","nHandle")
	If nretval =< 0
		Messagebox("Error occurred when check product master."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
	Endif
	If Used('commenu')
		Select commenu
		Go Top
		Scan
			_optionname = Rtrim(Ltrim(commenu.padname))+Rtrim(Ltrim(commenu.barname))
			_newoption = NewEncry(mudprodcode+'<~*0*~>'+Rtrim(Ltrim(commenu.padname))+'<~*1*~>'+Rtrim(Ltrim(commenu.barname))+'<~*2*~>'+Rtrim(Ltrim(Cast(commenu.Range As Varchar(10)))),'Udencyogprod')
			_newoption = Cast(_newoption As Varbinary(250))
			msqlstr = ''
			msqlstr = "update com_menu set newrange = ?_newoption"
			msqlstr = msqlstr+ " where RTRIM(LTRIM([padname]))+RTRIM(LTRIM([barname])) = ?_optionname"
			nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
			If nretval =< 0
				Messagebox("Error occurred when updating menu."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
			Endif
			Select commenu
		Endscan
	Endif

	msqlstr = "Select  CAST(' ' as varchar(250)) as enc,[group],[desc],[rep_nm] from r_status"
	nretval= sqlconobj.dataconn("EXE",company.dbname,msqlstr,"rstmenu","nHandle")
	If nretval =< 0
		Messagebox("Error occurred when check product master."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
	Endif
	If Used('rstmenu')
		Select rstmenu
		Go Top
		Scan
			_optionname = Rtrim(Ltrim(rstmenu.Group))+Rtrim(Ltrim(rstmenu.Desc))+Rtrim(Ltrim(rstmenu.rep_nm))
			_newoption = NewEncry(mudprodcode+'<~*0*~>'+Rtrim(Ltrim(rstmenu.Group))+'<~*1*~>'+Rtrim(Ltrim(rstmenu.Desc))+'<~*2*~>'+Rtrim(Ltrim(rstmenu.rep_nm)),'Udencyogprod')
			_newoption = Cast(_newoption As Varbinary(250))
			msqlstr = ''
			msqlstr = "update r_status set newgroup = ?_newoption"
			msqlstr = msqlstr+ " where RTRIM(LTRIM([group]))+RTRIM(LTRIM([desc]))+RTRIM(LTRIM([rep_nm])) = ?_optionname"
			nretval= sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
			If nretval =< 0
				Messagebox("Error occurred when updating report."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
			Endif
			Select rstmenu
		Endscan
	Endif

	If Alltrim(Upper(_co_mast.com_type)) <> 'M' &&Birendra:20 Apr 2011 Multi Comp TKT-310 Start
		msqlstr = "Select  CAST(' ' as varchar(250)) as enc,entry_ty from lcode"
		nretval= sqlconobj.dataconn("EXE",company.dbname,msqlstr,"lcdmenu","nHandle")
		If nretval =< 0
			Messagebox("Error occurred when check product master."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
		Endif
		If Used('lcdmenu')
			Select lcdmenu
			Go Top
			Scan
				_optionname = lcdmenu.entry_ty
				_newoption = NewEncry(mudprodcode+'<~*0*~>'+Rtrim(Ltrim(lcdmenu.entry_ty)),'Udencyogprod')
				_newoption = Cast(_newoption As Varbinary(250))
				msqlstr = ''
				msqlstr = "update lcode set cd = ?_newoption"
				msqlstr = msqlstr+ " where entry_ty = ?_optionname"
				nretval= sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
				If nretval =< 0
					Messagebox("Error occurred when updating transaction."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
				Endif
				Select lcdmenu
			Endscan
		Endif
	Endif		&&Birendra Multi Comp TKT-310 - End

	If Used('commenu')
		Use In commenu
	Endif
	If Used('rstmenu')
		Use In rstmenu
	Endif
	If Used('lcdmenu')
		Use In lcdmenu
	Endif

Case Inlist(Upper(Alltrim(mudprodcode)),'VUDYOGMFG','VUDYOGTRD','VUDYOGSERVICETAX')
	If Alltrim(Upper(_co_mast.com_type)) <> 'M' &&Birendra:20 Apr 2011 Multi Comp TKT-310 Start
		If Used('UpdtTblcode')
			Use In updttblcode
		Endif
		If !Used('UpdtTblcode')
			Use updttblcode Again Shared
		Endif

		Select updttblcode
		Scan
			If Alltrim(Upper(ueprodcode)) == Alltrim(Upper(mudprodcode))
				_optionname = updttblcode.entry_ty
				_newoption = NewEncry(mudprodcode+'<~*0*~>'+Rtrim(Ltrim(updttblcode.entry_ty)),'Udencyogprod')
				_newoption = Cast(_newoption As Varbinary(250))
				msqlstr = ''
				msqlstr = "update lcode set cd = ?_newoption"
				msqlstr = msqlstr+ " where entry_ty = ?_optionname"
				nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")
			Endif
			Select updttblcode
		Endscan

		If Used('UpdtTblcode')
			Use In updttblcode
		Endif
	Endif		&&Birendra Multi Comp TKT-310 - End
	If Used('UpdtTblr')
		Use In updttblr
	Endif
	If !Used('UpdtTblr')
		Use updttblr Again Shared
	Endif
	Select updttblr
	Scan
		If Alltrim(Upper(ueprodcode)) == Alltrim(Upper(mudprodcode))
			_optionname = Rtrim(Ltrim(updttblr.Group))+Rtrim(Ltrim(updttblr.Desc))+Rtrim(Ltrim(updttblr.rep_nm))
			_newoption = NewEncry(mudprodcode+'<~*0*~>'+Rtrim(Ltrim(updttblr.Group))+'<~*1*~>'+Rtrim(Ltrim(updttblr.Desc))+'<~*2*~>'+Rtrim(Ltrim(updttblr.rep_nm)),'Udencyogprod')
			_newoption = Cast(_newoption As Varbinary(250))
			msqlstr = ''
			msqlstr = "update r_status set newgroup = ?_newoption"
			msqlstr = msqlstr+ " where RTRIM(LTRIM([group]))+RTRIM(LTRIM([desc]))+RTRIM(LTRIM([rep_nm])) = ?_optionname"
			nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")
		Endif
		Select updttblr
	Endscan
	If Used('UpdtTblr')
		Use In updttblr
	Endif

	If Used('UpdtTblc')
		Use In updttblc
	Endif
	If !Used('UpdtTblc')
		Use updttblc Again Shared
	Endif

	_RangeChanged = ''	&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
	Select updttblc
	Scan
		If Alltrim(Upper(ueprodcode)) == Alltrim(Upper(mudprodcode))
			_optionname = Rtrim(Ltrim(updttblc.padname))+Rtrim(Ltrim(updttblc.barname))
			_newoption = NewEncry(mudprodcode+'<~*0*~>'+Rtrim(Ltrim(updttblc.padname))+'<~*1*~>'+Rtrim(Ltrim(updttblc.barname))+'<~*2*~>'+Rtrim(Ltrim(Cast(updttblc.Range As Varchar(10)))),'Udencyogprod')
			_newoption = Cast(_newoption As Varbinary(250))
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
			_rangeupdt = .F.
			msqlstr = ''
			msqlstr = "select prompname from com_menu where RTRIM(LTRIM([padname]))+RTRIM(LTRIM([barname])) = ?_optionname and range != ?updttblc.range"
			nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"_tmpRange","nHandle")
			If nretval > 0 And Used('_tmpRange')
				Select _tmpRange
				If Reccount() > 0
					_rangeupdt = .T.
					_RangeChanged = _RangeChanged + Chr(13)+Rtrim(Ltrim(updttblc.padname))+Space(5)+'->'+Space(5)+Rtrim(Ltrim(updttblc.barname))+Space(5)+'->'+Space(5)+Rtrim(Ltrim(_tmpRange.prompname))
				Endif
			Endif
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
			msqlstr = ''
			msqlstr = "update com_menu set newrange = ?_newoption"
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
			If _rangeupdt = .T.
				msqlstr = msqlstr+ ", range = ?updttblc.range"
				msqlstr = msqlstr+ ", progname = replace(progname,'^'+ltrim(rtrim(cast(range as varchar(100)))),'^'+ltrim(rtrim(cast(?updttblc.range as varchar(100)))))"
			Endif
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
			msqlstr = msqlstr+ " where RTRIM(LTRIM([padname]))+RTRIM(LTRIM([barname])) = ?_optionname"
			nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"nedata","nHandle")
		Endif
		Select updttblc
	Endscan
	If Used('UpdtTblc')
		Use In updttblc
	Endif

&&--->Rup Alert Master 13/10/2011
&&Converted Varbinary to Varchar as null value records are not getting deleted. Done by vasant on 21/02/2012
	msqlstr ="Delete from r_status where ISNULL(CAST(newgroup as varchar(250)),'' ) = ''"	&&Done by vasant on 21/02/2012
	nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"deldata","nHandle")
	msqlstr ="Delete from com_menu where ISNULL(CAST(newrange as varchar(250)),'' ) = ''"	&&Done by vasant on 21/02/2012
	nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"deldata","nHandle")
	If Alltrim(Upper(_co_mast.com_type)) <> 'M'
		msqlstr ="Delete from LCode where ISNULL(CAST(cd as varchar(250)),'' ) = ''"		&&Done by vasant on 21/02/2012
		nretval=sqlconobj.dataconn('EXE',company.dbname,msqlstr,"deldata","nHandle")
	Endif
&&<---Rup Alert Master 13/10/2011

&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
	If !Empty(_RangeChanged)
		_RangeChanged = 'Following Records Range has been updated,Please Assign the Rights again'+Chr(13)+;
			'PadName    ->    BarName    ->    Prompt Name'+Chr(13)+_RangeChanged
		_LogFileName = Addbs(apath)+Alltrim(company.co_name)+' ueupdate.log'
		=Strtofile(_RangeChanged,_LogFileName)
	Endif
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
*Case INLIST(UPPER(ALLTRIM(mudprodcode)),'VUDYOGSTD','VUDYOGPRO','VUDYOGENT')	&&Commented For Bug-2286 USquare 10.0 Installer : By Amrendra on 16-02-2012
*!*	Case Inlist(Upper(Alltrim(mudprodcode)),'VUDYOGSTD','VUDYOGPRO','VUDYOGENT','10USQUARE','10ITAX')	&&Changed For Bug-2286 USquare 10.0 Installer : By Amrendra on 16-02-2012
Case Inlist(Upper(Alltrim(mudprodcode)),'VUDYOGSTD','VUDYOGPRO','VUDYOGENT','10USQUARE','10ITAX','VUDYOGGST')			&& Changed by Sachin N. S. on 27/01/2017 for GST
	pvalue = Allt(company.passroute)+Allt(company.passroute1)		&&Changes has been done by vasant as per TKT-8292 on 04/06/2011
	If Empt(pvalue) Then
		Return ""
	Endif
	Buffer = ""
	For i = 1 To Len(pvalue)
		Buffer = Buffer + Chr(Asc(Substr(pvalue,i,1))/2)
	Next i
	lproduct1 = Buffer

	msqlstr = "Select CAST(' ' as varbinary(250)) as enc,padname,barname from com_menu"
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"commenu","nHandle")

	msqlstr = "Select  CAST(' ' as varbinary(250)) as enc,[group],[desc],[rep_nm] from r_status"
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"rstmenu","nHandle")

	msqlstr = "Select  CAST(' ' as varbinary(250)) as enc,entry_ty from lcode"
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"lcdmenu","nHandle")

	msqlstr = "Select Enc,PEnc from Vudyog..ProdDetail"
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"dbfmenu","nHandle")

	_suptype = Upper(GlobalObj.getPropertyval('ClientType'))
	_prodcode = Allt(mudprodcode)
	If Used('commenu')
		Update commenu Set Enc = NewEncry(Upper(Alltrim(padname)+Alltrim(barname))+'~*0*~'+_suptype+'~*1*~'+_prodcode,'Udencyogprod')
	Endif
	If Used('rstmenu')
		Update rstmenu Set Enc = NewEncry(Upper(Alltrim(Group)+Alltrim(Desc)+Alltrim(rep_nm))+'~*0*~'+_suptype+'~*1*~'+_prodcode,'Udencyogprod')
	Endif
	If Used('lcdmenu')
		Update lcdmenu Set Enc = NewEncry(Upper(Alltrim(entry_ty))+'~*0*~'+_suptype+'~*1*~'+_prodcode,'Udencyogprod')
	Endif
	If Used('dbfmenu') And Used('commenu') And Used('rstmenu') And Used('lcdmenu')
		Select dbfmenu
		Go Top
		Select commenu
		Go Top
		Select rstmenu
		Go Top
		Select lcdmenu
		Go Top

		Select * From dbfmenu Where Penc In (Select Alltrim(Enc) From commenu Where !Empty(Enc)) Into Cursor _dbfmenu ;
			union All Select * From dbfmenu Where Penc In (Select Alltrim(Enc) From rstmenu Where !Empty(Enc)) ;
			union All Select * From dbfmenu Where Penc In (Select Allt(Enc) From lcdmenu Where !Empty(Enc))
		If Used('_dbfmenu')
			Select _dbfmenu
			Go Top
			Scan
				_decdata = NewDecry(Alltrim(Cast(_dbfmenu.Enc As Blob)),'Udencyogprod')
				_stlen = 1
				_enlen = At('~*0*~',_decdata)
				_optiontype 	= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen,_enlen-_stlen),''))
				_stlen = At('~*0*~',_decdata)
				_enlen = At('~*1*~',_decdata)
				_featureid  	= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
				_stlen = At('~*1*~',_decdata)
				_enlen = At('~*2*~',_decdata)
				_subfeatureid  	= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
				_stlen = At('~*2*~',_decdata)
				_enlen = At('~*3*~',_decdata)
				_prodcode  		= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
				_stlen = At('~*3*~',_decdata)
				_enlen = At('~*4*~',_decdata)
				_prodver  		= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
				_stlen = At('~*4*~',_decdata)
				_enlen = At('~*5*~',_decdata)
				_servicever  	= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
				_stlen = At('~*5*~',_decdata)
				_enlen = At('~*6*~',_decdata)
				_featuretype  	= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))
				_stlen = At('~*6*~',_decdata)
				_enlen = At('~*7*~',_decdata)
				_optionname  	= Alltrim(Iif(_stlen > 0 And _enlen > 0,Substr(_decdata,_stlen + 5,_enlen-(_stlen+5)),''))

				_newoption		= NewEncry(_featureid+'~*0*~'+_subfeatureid+'~*1*~'+_optionname+'~*2*~','Udencyogprod')

				If Alltrim(Upper(_prodver)) == Alltrim(Upper(mudprodcode)) And (Upper(_prodcode) $ Upper(lproduct1) Or Upper(_prodcode) = 'VUGEN')
					If Upper(_optiontype) == 'MENU'
						msqlstr = ''
						_newoption = Cast(_newoption As Varbinary(250))
						msqlstr = "update com_menu set newrange = ?_newoption"
						msqlstr = msqlstr+ " where RTRIM(LTRIM([padname]))+RTRIM(LTRIM([barname])) = ?_optionname"
						nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
					Endif
					If Upper(_optiontype) == 'REPORT'
						msqlstr = ''
						_newoption = Cast(_newoption As Varbinary(250))
						msqlstr = "update r_status set newgroup = ?_newoption"
						msqlstr = msqlstr+ " where RTRIM(LTRIM([group]))+RTRIM(LTRIM([desc]))+RTRIM(LTRIM([rep_nm])) = ?_optionname"
						nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
					Endif
					If Alltrim(Upper(_co_mast.com_type)) <> 'M' &&Birendra:20 Apr 2011 Multi Comp TKT-310 Start
						If Upper(_optiontype) == 'TRANSACTION'
							msqlstr = ''
							_newoption = Cast(_newoption As Varbinary(250))
							msqlstr = "update lcode set cd = ?_newoption"
							msqlstr = msqlstr+ " where entry_ty = ?_optionname"
							nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
						Endif
					Endif		&&Birendra Multi Comp TKT-310 - End
				Endif
				Select _dbfmenu
			Endscan
		Endif
	Endif

&&Changes has been done by vasant as per TKT-8292 on 04/06/2011 (start)
	msqlstr = ''
	msqlstr = "update com_menu set LabKey = ''"
	nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")

	_suptype = 'NORMAL'
	_prodcode = Allt(mudprodcode)
	If Used('commenu')
		Update commenu Set Enc = NewEncry(Upper(Alltrim(padname)+Alltrim(barname))+'~*0*~'+_suptype+'~*1*~'+_prodcode,'Udencyogprod')
	Endif
	If Used('dbfmenu') And Used('commenu')
		Select dbfmenu
		Go Top
		Select commenu
		Go Top
		Select Cast(NewDecry(Alltrim(Cast(Enc As Blob)),'Udencyogprod') As Memo) As Enc From dbfmenu Where Penc In (Select Alltrim(Enc) From commenu Where !Empty(Enc)) Into Cursor _dbfmenu
		If Used('_dbfmenu')
			Select Cast(Substr(Enc,1,At('~*0*~',Enc)-1) As Varchar(254)) As optiontype,;
				CAST(Strextract(Enc,'~*0*~','~*1*~') As Varchar(254)) As featureid,;
				CAST(Strextract(Enc,'~*1*~','~*2*~') As Varchar(254)) As sfeatureid,;
				CAST(Strextract(Enc,'~*2*~','~*3*~') As Varchar(254)) As prodcode,;
				CAST(Strextract(Enc,'~*3*~','~*4*~') As Varchar(254)) As prodver,;
				CAST(Strextract(Enc,'~*4*~','~*5*~') As Varchar(254)) As servicever,;
				CAST(Strextract(Enc,'~*5*~','~*6*~') As Char(225)) As ftype,;
				CAST(Strextract(Enc,'~*6*~','~*7*~') As Varchar(254)) As optionname From _dbfmenu Into Cursor _dbfmenu1 Readwrite
			If Used('_dbfmenu1')
				Select _dbfmenu1
				Index On ftype Tag ftype Desc
				Scan
					If Upper(_dbfmenu1.optiontype) == 'MENU' And Alltrim(Upper(_dbfmenu1.prodver)) == Alltrim(Upper(mudprodcode)) And (Upper(_dbfmenu1.prodcode) $ Upper(lproduct1) Or Upper(_dbfmenu1.prodcode) = 'VUGEN')
						_LabKey	= 'U'
						If Upper(Alltrim(_dbfmenu1.ftype)) == 'PREMIUM'
							_mregname = ''
							_macid = ''
							_mregname = Alltrim(ueReadRegMe.r_comp)
							_macid 	  = Alltrim(ueReadRegMe.r_macid)
							_newoption		= NewEncry(_mregname+'~*0*~'+_macid+'~*1*~'+Alltrim(_dbfmenu1.featureid)+'~*2*~'+Alltrim(company.co_name),_macid)

							msqlstr = "select top 1 enc from clientfeature where enc = ?_newoption"
							nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"_TmpClient","nHandle")

							If nretval > 0 And Used('_TmpClient')
								If Reccount('_TmpClient') > 0
									_LabKey = 'S'
								Endif
							Endif
							If Used('_TmpClient')
								Use In _TmpClient
							Endif
						Endif

						msqlstr = ''
						msqlstr = "update com_menu set LabKey = ?_LabKey+?_dbfmenu1.ftype"
						msqlstr = msqlstr+ " where RTRIM(LTRIM([padname]))+RTRIM(LTRIM([barname])) = ?_dbfmenu1.optionname"
						nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
					Endif
					Select _dbfmenu1
				Endscan
				If Used('_dbfmenu1')
					Use In _dbfmenu1
				Endif
			Endif
		Endif
	Endif
&&Changes has been done by vasant as per TKT-8292 on 04/06/2011 (end)

	msqlstr = "Select newrange,padname,barname from com_menu"
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"commenu","nHandle")
	If nretval =< 0
		Messagebox("Error occurred when check product master."+Chr(13)+"Please contact your software vendor.",0+64,VuMess)
	Endif
	If Used('commenu')
		Select commenu
		Index On barname Tag barname
		Scan
			If !(Alltrim(commenu.newrange) == '.') And !Empty(commenu.newrange)
				_commenurec = Iif(!Eof(),Recno(),0)
				_Padname = commenu.padname
				Do While .T.
					If Seek(_Padname)
						Replace newrange With '.' In commenu
						_Padname = commenu.padname
					Else
						Exit
					Endif
				Enddo
				Select commenu
				If _commenurec > 0
					Go _commenurec
				Endif
			Endif
		Endscan

		Select commenu
		Scan
			If Alltrim(commenu.newrange) == '.'
				_optionname  = Allt(padname)+Allt(barname)
				_newoption	= NewEncry(_optionname,'Udencyogprod')
				msqlstr = ''
				_newoption = Cast(_newoption As Varbinary(250))
				msqlstr = "update com_menu set newrange = ?_newoption"
				msqlstr = msqlstr+ " where RTRIM(LTRIM([padname]))+RTRIM(LTRIM([barname])) = ?_optionname"
				nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
				Select commenu
			Endif
		Endscan
	Endif

&&--->Rup Alert Master 13/10/2011
&&Converted Varbinary to Varchar as null value records are not getting deleted. Done by vasant on 21/02/2012
	msqlstr ="Delete from r_status where ISNULL(CAST(newgroup as varchar(250)),'' ) = ''"	&&Done by vasant on 21/02/2012
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
	msqlstr ="Delete from com_menu where ISNULL(CAST(newrange as varchar(250)),'' ) = ''"	&&Done by vasant on 21/02/2012
	nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
	If Alltrim(Upper(_co_mast.com_type)) <> 'M'
		msqlstr ="Delete from LCode where ISNULL(CAST(cd as varchar(250)),'' ) = ''"		&&Done by vasant on 21/02/2012
		nretval=sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","nHandle")
	Endif
&&<---Rup Alert Master 13/10/2011

	If Used('commenu')
		Use In commenu
	Endif
	If Used('rstmenu')
		Use In rstmenu
	Endif
	If Used('lcdmenu')
		Use In lcdmenu
	Endif
	If Used('dbfmenu')
		Use In dbfmenu
	Endif
	If Used('_dbfmenu')
		Use In _dbfmenu
	Endif

Endcase

nretval=sqlconobj.sqlconnclose("nHandle") && Connection Close
Release usquarepass,mudprodcode
Wait Clear
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011
*=MESSAGEBOX('Updation Done Successfully',64,vumess)
_UpdtMsg = 'Updation Done Successfully'
If !Empty(_LogFileName)
	_UpdtMsg = _UpdtMsg +Chr(13)+Chr(13)+'Please check '+Alltrim(_LogFileName)
Endif
=Messagebox(_UpdtMsg,64,VuMess)
&&Changes has been done as per TKT-8323 (Menu table has been changed) By Vasant on 08/06/2011

exitclick = .T.
&&Changes has been done as per TKT-6470 (Multilanguage support - Tested with English & Japanese Language) on 24/02/2011

*>>>***Versioning**** Added By Amrendra On 06/09/2011 TKT 8543
Function GetFileVersion
Parameters lcTable
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
If !Empty(lcTable)
	Select(lcTable)
	Append Blank
	Replace fVersion With Justfname(Sys(16))+'   '+_CurrVerVal
Endif
Return _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 06/09/2011  TKT 8543
