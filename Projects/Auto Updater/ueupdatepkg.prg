&&vasant280312
**exclude file & extension of restore & backup working or not
**restore database command check
**copy excluded file
**manual update
**updt version
*store error in log file
*last updt date check
ON ERROR 
*************

IF UPPER(MVU_USER_ROLES) != 'ADMINISTRATOR'
	=MESSAGEBOX("Only User's having Administrator Rights can Run this Update",64,vumess)
	DO ExitUpdt
	RETURN  .f.
ENDIF

#Include AutoUpdater.h
PUBLIC mudprodcode,usquarepass,_mproddesc,_mupdtmonth,_mIntVersion,_mlastupdtmonth,_mFldrName,_mDataBackFldrName,_mManualUpdateFldrName,_mFinalUpdateFldrName,_ErrMsg,_mmachine,_mlogip,_mErrHtmlName,_GenFreshProdDetail,_AutoMainPath,_mDocFldrName,_mlastupdtversion	&&vasant280312
PUBLIC _mCheckUpdtHistTable,_mUpdateUpdtHistTable,_mAutoUpdaterCaption

mudprodcode 	= ''
usquarepass		= ''
_mproddesc 		= ''
_mupdtmonth		= {}
_mIntVersion	= ''
_mlastupdtmonth = {}
_mlastupdtversion = ''		&&vasant280312
_mFldrName 		= ''
_mDataBackFldrName		= ''
_mManualUpdateFldrName	= ''
_mFinalUpdateFldrName	= ''
_ErrMsg			= ''
_mmachine 		= ''
_mlogip   		= ''
_mErrHtmlName	= ''
_AutoMainPath	= ''
_mDocFldrName	= ''
_mCheckUpdtHistTable	= .t.
_mUpdateUpdtHistTable	= .t.
_mAutoUpdaterCaption	= ''

_AutoMainPath	= ADDBS(Apath)
_mFldrName    	= _AutoMainPath+'Monthly Updates\'
_mErrHtmlName	= _mFldrName+'Update Log '+STRTRAN(STRTRAN(TTOC(DATETime()),'/','-'),':','-')+'.html'
_GenFreshProdDetail = .f.
_varIntVersion	= ''

*!*	oWS = CREATEOBJECT ("MSWinsock.Winsock")
*!*	IF TYPE('oWS') = 'O'
*!*		_mmachine  = oWS.LocalHostName
*!*		_mlogip    = oWS.LocalIP
*!*	Endif	
*!*	RELEASE oWS	

Try
	Local loWMIService, loItems, loItem
	_mmachine = ALLTRIM(Getwordnum(Sys(0),1))
	loWMIService = Getobject("winmgmts:\\" + _mmachine + "\root\cimv2")
	loItems = loWMIService.ExecQuery("Select * from Win32_NetworkAdapterConfiguration")
	For Each loItem In loItems
		If loItem.IPEnabled
			_mlogip = loItem.IPAddress[0]
		Endif
		IF ISNULL(_mlogip)
			_mlogip = ''
		ENDIF
		_mlogip = Transform(_mlogip)
		IF !EMPTY(_mlogip)
			EXIT
		Endif	
	ENDFOR
	IF EMPTY(_mlogip)
		_mlogip = '127.0.0.1'
	Endif
CATCH TO ErrMsg
	=MESSAGEBOX(ErrMsg.Message,0,vuMess)
Endtry	 

IF DIRECTORY(_mFldrName,1) = .f.
	Try
		MkDir (_mFldrName)
	CATCH TO m_errMsg
		_ErrMsg = ALLTRIM(m_errMsg.Message)
	endtry	
	IF !EMPTY(_ErrMsg) OR DIRECTORY(_mFldrName,1) = .f.
		=Messagebox("Unable to Create Folder "+_mFldrName+".",0+16,vuMess)
		DO ExitUpdt
		RETURN .f.
	Endif
ENDIF

i1 = 1
DO WHILE .t.
	_tmpfname = _mFldrName+'tmp'+allt(STR(i1))+SYS(3)+'.tmp'
	IF FILE(_tmpfname)
		i1 = i1 + 1
	ELSE
		EXIT 
	Endif	
ENDDO

TRY 
	_tmpcretefile = FCREATE(_tmpfname)
CATCH TO m_errMsg
	_ErrMsg = ALLTRIM(m_errMsg.Message)
endtry	
=FCLOSE(_tmpcretefile)
IF !EMPTY(_ErrMsg)
	=MESSAGEBOX(_ErrMsg,0+16,vuMess)
	DO ExitUpdt
	RETURN  .f.
Endif

IF !FILE(_tmpfname)
	=Messagebox("Set the property of Directory to read & write.",0+16,vuMess)
	DO ExitUpdt
	RETURN  .f.
ENDIF

Try
	ERASE (_tmpfname)	
CATCH TO m_errMsg
	_ErrMsg = ALLTRIM(m_errMsg.Message)
endtry	
IF FILE(_tmpfname) OR !EMPTY(_ErrMsg)
	=Messagebox("Set the property of Directory to read & write.",0+16,vuMess)
	DO ExitUpdt
	RETURN  .f.
ENDIF

*!*	_mfcount  = ADIR(_mflist,_mFldrName+'AutoUpdateLog*.*',"D")
*!*	FOR i1 = 1 TO _mfcount 
*!*		_mRmFileName = _mFldrName+_mflist(i1,1)
*!*		IF !"D"$_mflist[i1,5] 
*!*			Try
*!*				ERASE (_mRmFileName) RECYCLE
*!*			CATCH TO m_errMsg
*!*				_ErrMsg = ALLTRIM(m_errMsg.Message)
*!*			ENDTRY
*!*			IF FILE(_mRmFileName) AND EMPTY(_ErrMsg)
*!*				_ErrMsg = "Unable to Delete "+_mRmFileName+" File."
*!*			Endif	
*!*		Endif
*!*	ENDFOR
*!*	RELEASE _mflist

=ErrLog('','')	
			
mudprodcode = dec(NewDecry(GlobalObj.getPropertyval("UdProdCode"),'Ud*yog+1993'))	&&vasant280312
usquarepass = Upper(DEC(NewDecry(GlobalObj.GetPropertyVal('EncryptId'),'Ud*_yog*\+1993')))
_mproddesc  = GlobalObj.getPropertyval("ProductTitle") 

_mlastupdtmonth 	= CTOD(_DefineLastUpdtMonth)
_mlastupdtversion	= _DefineLastUpdtVersion		&&vasant280312
_GenFreshProdDetail = _DefineGenFreshProdDetail
_mCheckUpdtHistTable	= _DefineCheckUpdtHistTable
_mUpdateUpdtHistTable	= _DefineUpdateUpdtHistTable
_mAutoUpdaterCaption	= _DefineAutoUpdaterCaption

nretval=0
nhandle=0
nhandle_master=0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)

IF USED('ZipDetail')
	USE IN ZipDetail
Endif	
SELECT 0
SELECT *,CAST(.f. as l) as Sel,CAST(' ' as Char(50)) as Lock,;
	CAST(ALLTRIM(STR(Ver1))+'.'+ALLTRIM(STR(Ver2))+'.'+ALLTRIM(STR(Ver3))+'.'+ALLTRIM(STR(Ver4)) as Char(25)) As IntVersion,;
	CAST(' ' as Char(50)) as NewVersion,CAST(' ' as memo) as tmpZipName,CAST(' ' as memo) as UnZipPath,CAST(' ' as memo) as DbList ;
	FROM ZipDetail INTO CURSOR _ZipDetail READWRITE WHERE Enabled = .t.
IF USED('ZipDetail')
	USE IN ZipDetail
Endif	

SELECT _ZipDetail
REPLACE ALL NewVersion WITH Padl(ALLTRIM(STREXTRACT(_ZipDetail.IntVersion,'','.',0,2)),10,'0') +  Padl(ALLTRIM(STREXTRACT(_ZipDetail.IntVersion,'.','.',1,2)),10,'0') + 	Padl(ALLTRIM(STREXTRACT(_ZipDetail.IntVersion,'.','.',2,2)),10,'0') + 	Padl(ALLTRIM(STREXTRACT(_ZipDetail.IntVersion,'.','.',3,2)),10,'0') IN _ZipDetail
INDEX on DTOS(Updt_Date)+NewVersion TAG UpDtVerD Desc
INDEX on DTOS(Updt_Date)+NewVersion TAG UpDtVerA
GO Bott
IF RECCOUNT('_ZipDetail') <= 0
	=MESSAGEBOX("Nothing to Update",64,vumess)
	DO ExitUpdt
	RETURN  .f.
Else
	_mupdtmonth  = _ZipDetail.Updt_Date
	_mIntVersion = _ZipDetail.IntVersion
	_varIntVersion = _ZipDetail.NewVersion
Endif	

msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTEXCL'"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
Endif

IF RECCOUNT('_tmpCoList') <= 0
	msqlstr = "Create Table UPDTEXCL (ProductNm Varchar(25),ExclFlNm Varchar(50),ManualUpdt Bit,MsgToShow Varchar(250))"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
	IF nretval <= 0
		DO ExitUpdt
		RETURN  .f.
	Endif

	msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTEXCL'"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	Endif
	IF RECCOUNT('_tmpCoList') <= 0
		=Messagebox("UPDTEXCL table not found in Vudyog Database.",0+16,vuMess)
		DO ExitUpdt
		RETURN  .f.
	Endif
Endif

&&vasant280312
msqlstr = "if not exists (select b.name from sysobjects a,syscolumns b where a.id = b.id and a.xtype = 'U' and a.name = 'UPDTEXCL' and b.name ='MsgToShow')"
msqlstr = msqlstr + "Alter table UPDTEXCL Add MsgToShow Varchar(250) Default '' With Values"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
IF nretval <= 0
	DO ExitUpdt
	RETURN  .f.
Endif
&&vasant280312

SELECT * FROM UpdtExcl WHERE EMPTY(ProductNm) OR INLIST(UPPER(ProductNm),UPPER(mudprodcode)) INTO cursor _tmpCoList		&&vasant280312
SELECT _tmpCoList
SCAN
	IF _tmpCoList.Internal = .f.
		msqlstr = "If Not Exists(Select Top 1 * from UpdtExcl Where ProductNm = ?_tmpCoList.ProductNm And ExclFlNm = ?_tmpCoList.ExclFlNm)"
		msqlstr = msqlstr + " Insert into UpdtExcl (ProductNm,ExclFlNm,ManualUpdt,MsgToShow) Values (?_tmpCoList.ProductNm,?_tmpCoList.ExclFlNm,?_tmpCoList.ManualUpdt,?_tmpCoList.MsgToShow)"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
		IF nretval <= 0
			=Messagebox("Unable to update UPDTEXCL table in Vudyog Database.",0+16,vuMess)
			DO ExitUpdt
			RETURN  .f.
		Endif
	ENDIF
	IF _tmpCoList.Del = .t.
		msqlstr = "Delete from UpdtExcl Where ProductNm = ?_tmpCoList.ProductNm And ExclFlNm = ?_tmpCoList.ExclFlNm"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
		IF nretval <= 0
			=Messagebox("Unable to update UPDTEXCL table in Vudyog Database.",0+16,vuMess)
			DO ExitUpdt
			RETURN  .f.
		Endif
	Endif
	SELECT _tmpCoList
Endscan

msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTHIST'"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
Endif

IF RECCOUNT('_tmpCoList') <= 0
	msqlstr = "Create Table UPDTHIST (UpdtMonth Datetime,UpdtVersion Varchar(15),UpdtDate Datetime,[User] Varchar(15),Log_machine Varchar(25),Log_ip Varchar(15))"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
	IF nretval <= 0
		DO ExitUpdt
		RETURN  .f.
	Endif

	msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTHIST'"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	Endif
	IF RECCOUNT('_tmpCoList') <= 0
		=Messagebox("UPDTHIST table not found in Vudyog Database.",0+16,vuMess)
		DO ExitUpdt
		RETURN  .f.
	Endif
Endif

&&vasant300112
msqlstr = "if not exists (select b.name from sysobjects a,syscolumns b where a.id = b.id and a.xtype = 'U' and a.name = 'UPDTHIST' and b.name ='UPDTDONE')"
msqlstr = msqlstr + "Alter table UpdtHist Add UpdtId Int Identity,IntVersion VarChar(15) Default '' With Values,UpdtDone Bit Default 1 With Values"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
IF nretval <= 0
	DO ExitUpdt
	RETURN  .f.
Endif
&&vasant300112

msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTDETAIL'"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
Endif

IF RECCOUNT('_tmpCoList') <= 0
	msqlstr = "Create Table UPDTDETAIL (UpdtMonth Datetime,UpdtDate Datetime,IntVersion VarChar(15),FileNm VarChar(250),FilePath Text)"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
	IF nretval <= 0
		DO ExitUpdt
		RETURN  .f.
	Endif

	msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTDETAIL'"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	Endif
	IF RECCOUNT('_tmpCoList') <= 0
		=Messagebox("UPDTDETAIL table not found in Vudyog Database.",0+16,vuMess)
		DO ExitUpdt
		RETURN  .f.
	Endif
Endif

msqlstr = "select top 1 a.name from syscolumns a,sysobjects b ;
	where a.id = b.id and a.name = 'passroute1'  and b.name = 'co_mast'"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
ELSE
	IF RECCOUNT('_tmpCoList') <= 0
		msqlstr = "Alter Table Co_mast Add passroute1 VarBinary(250) Default CAST('' as varbinary(1)) With Values"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
		IF nretval <= 0
			DO ExitUpdt
			RETURN  .f.
		Endif	
	Endif	
Endif

msqlstr = "select CAST(0 as Bit) as SelCo,CAST(0 as numeric(1)) as runupdt,a.compid,a.co_name,a.dir_nm,a.dbname,a.sta_dt,a.end_dt"
msqlstr = msqlstr + ",a.passroute,a.passroute1,a.com_type,b.UpdtMonth,b.UpdtVersion,b.UpdtDate,b.IntVersion,b.[User],b.UpdtDone,CAST(' ' as Char(50)) as NewVersion from co_mast a,updthist b;
	where 1 = 2"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_UpdtCoList","nHandle")
IF nretval <= 0 OR !USED('_UpdtCoList')
	DO ExitUpdt
	RETURN  .f.
Endif

msqlstr = "select compid,co_name,dir_nm,dbname,sta_dt,end_dt,passroute,passroute1,com_type from co_mast Order by co_name,dir_nm,dbname,sta_dt,end_dt"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
Endif

SELECT _UpdtCoList
APPEND BLANK IN _UpdtCoList
REPLACE co_name WITH 'Main Folder',compid WITH 0,;
	dir_nm WITH _AutoMainPath,dbname WITH 'VUDYOG',;
	RunUpdt WITH 0,SelCo WITH .f. IN _UpdtCoList

_mPrevUniqFlds = ''
SELECT _tmpCoList
SCAN
	IF USED('_Co_mast')
		SELECT _Co_mast
		IF RECCOUNT() > 0
			LOCATE FOR Co_name = _tmpCoList.co_name
			IF !FOUND()
				SELECT _tmpCoList
				LOOP
			Endif	
		Endif
		SELECT _tmpCoList
	ENDIF
	
	_mCurUniqFlds = co_name+dir_nm+dbname
	_mpassroute = ALLTRIM(_tmpCoList.passroute)
	Buffer1 = ""
	For i1 = 1 To Len(_mpassroute)
		Buffer1 = Buffer1 + Chr(Asc(Substr(_mpassroute,i1,1))/2)
	Next i1
	_mpassroute = Buffer1
	_mpassroute1 = ALLTRIM(_tmpCoList.passroute1)
	Buffer1 = ""
	For i1 = 1 To Len(_mpassroute1)
		Buffer1 = Buffer1 + Chr(Asc(Substr(_mpassroute1,i1,1))/2)
	Next i1
	_mpassroute1 = Buffer1

	IF !(_mPrevUniqFlds == _mCurUniqFlds)
		SELECT _UpdtCoList
		APPEND BLANK IN _UpdtCoList
		REPLACE co_name WITH _tmpCoList.co_name,compid WITH _tmpCoList.compid,;
			dir_nm WITH _tmpCoList.dir_nm,dbname WITH _tmpCoList.dbname,;
			sta_dt WITH _tmpCoList.sta_dt,end_dt WITH _tmpCoList.end_dt,;
			com_type WITH _tmpCoList.com_type,;
			RunUpdt WITH 0,SelCo WITH .f.,;
			passroute WITH _mpassroute,;
			passroute1 WITH _mpassroute1 IN _UpdtCoList
	ENDIF
	_mPrevUniqFlds = _mCurUniqFlds

	SELECT _UpdtCoList
	REPLACE end_dt WITH _tmpCoList.end_dt IN _UpdtCoList

	SELECT _tmpCoList
ENDSCAN 		

msqlstr = "select [user] from vudyog..[user] where [user] != ?musername"
nretval=sqlconobj.dataconn('EXE','master',msqlstr,"_tmpCoList","nhandle_master")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
ELSE
	SELECT _tmpCoList
	SCAN

		msqlstr = "select name from tempdb..sysobjects where xtype = 'U' and name = '##"+allt(_tmpCoList.user)+"'"
		nretval=sqlconobj.dataconn('EXE','master',msqlstr,"_tmptbl1","nhandle_master")
		IF nretval <= 0 OR !USED('_tmptbl1')
			DO ExitUpdt
			RETURN  .f.
		ELSE
			IF RECCOUNT('_tmptbl1') > 0
				=Messagebox("To continue update, other users must exit Software.",0+16,vuMess)
				DO ExitUpdt
				RETURN  .f.
			Endif	
		Endif			

		SELECT _tmpCoList
	ENDSCAN		

	SELECT _tmpCoList
	SCAN

		msqlstr = "select [user] into ##"+allt(_tmpCoList.user)+" from vudyog..[user]"
		nretval=sqlconobj.dataconn('EXE','master',msqlstr,"","nhandle_master")
		IF nretval <= 0
			DO ExitUpdt
			RETURN  .f.
		Endif
		
		SELECT _tmpCoList
	ENDSCAN		

Endif

msqlstr = "SELECT hostname FROM master..SysProcesses WHERE DBId = DB_ID('vudyog') and hostname != ?_mmachine and hostname != ''"	&&vasant300112
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
IF nretval <= 0 OR !USED('_tmpCoList')
	DO ExitUpdt
	RETURN  .f.
ELSE
	IF RECCOUNT('_tmpCoList') > 0
		=Messagebox("To continue update, other users must exit Software.",0+16,vuMess)
		DO ExitUpdt
		RETURN  .f.
	Endif	
Endif

SELECT _UpdtCoList
SCAN
	msqlstr = "SELECT hostname FROM master..SysProcesses WHERE DBId = DB_ID('"+_UpdtCoList.DbName+"') and hostname != ?_mmachine and hostname != ''"	&&vasant300112
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	ELSE
		IF RECCOUNT('_tmpCoList') > 0
			=Messagebox("To continue update, other users must exit Software.",0+16,vuMess)
			DO ExitUpdt
			RETURN  .f.
		Endif	
	Endif
	SELECT _UpdtCoList
ENDSCAN

SELECT _UpdtCoList
SCAN
	_mDbName = ALLTRIM(_UpdtCoList.DbName)
	
	msqlstr = "select name from "+_mDbName+"..sysobjects where xtype = 'U' and name = 'UPDTHIST'"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	Endif

	IF RECCOUNT('_tmpCoList') <= 0
		msqlstr = "Create Table "+_mDbName+"..UPDTHIST (UpdtMonth Datetime,UpdtVersion Varchar(15),UpdtDate Datetime,[User] Varchar(15),Log_machine Varchar(25),Log_ip Varchar(15))"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
		IF nretval <= 0
			DO ExitUpdt
			RETURN  .f.
		Endif

		msqlstr = "select name from "+_mDbName+"..sysobjects where xtype = 'U' and name = 'UPDTHIST'"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
		IF nretval <= 0 OR !USED('_tmpCoList')
			DO ExitUpdt
			RETURN  .f.
		Endif
		IF RECCOUNT('_tmpCoList') <= 0
			=Messagebox("UPDTHIST table not found in "+_mDbName+" Database.",0+16,vuMess)
			DO ExitUpdt
			RETURN  .f.
		Endif
	Endif

	&&vasant300112
	msqlstr = "if not exists (select b.name from "+_mDbName+"..sysobjects a,"+_mDbName+"..syscolumns b where a.id = b.id and a.xtype = 'U' and a.name = 'UPDTHIST' and b.name ='UPDTDONE')"
	msqlstr = msqlstr + "Alter table "+_mDbName+"..UpdtHist Add UpdtId Int Identity,IntVersion VarChar(15) Default '' With Values,UpdtDone Bit Default 1 With Values"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
	IF nretval <= 0
		DO ExitUpdt
		RETURN  .f.
	Endif
	&&vasant300112

	msqlstr = "select name from "+_mDbName+"..sysobjects where xtype = 'U' and name = 'UPDTDETAIL'"
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	Endif

	IF RECCOUNT('_tmpCoList') <= 0
		msqlstr = "Create Table "+_mDbName+"..UPDTDETAIL (UpdtMonth Datetime,UpdtDate Datetime,IntVersion VarChar(15),FileNm VarChar(250),FilePath Text)"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
		IF nretval <= 0
			DO ExitUpdt
			RETURN  .f.
		Endif

		msqlstr = "select name from "+_mDbName+"..sysobjects where xtype = 'U' and name = 'UPDTDETAIL'"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
		IF nretval <= 0 OR !USED('_tmpCoList')
			DO ExitUpdt
			RETURN  .f.
		Endif
		IF RECCOUNT('_tmpCoList') <= 0
			=Messagebox("UPDTDETAIL table not found in "+_mDbName+" Database.",0+16,vuMess)
			DO ExitUpdt
			RETURN  .f.
		Endif
	Endif
	
	msqlstr = "select Top 1 UpdtMonth,UpdtVersion,UpdtDate,IntVersion,UpdtDone,[User] "
	msqlstr = msqlstr + " from "+_mDbName+"..UpdtHist where UpdtDone = 1 Order by UpdtMonth Desc,IntVersion Desc,UpdtDate Desc"	&&vasant300112
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	ELSE
		REPLACE UpdtMonth WITH _tmpCoList.UpdtMonth,;
			UpdtVersion WITH _tmpCoList.UpdtVersion,;
			UpdtDate WITH _tmpCoList.UpdtDate,;
			IntVersion WITH _tmpCoList.IntVersion,;
			UpdtDone WITH _tmpCoList.UpdtDone,;
			User WITH _tmpCoList.User in _UpdtCoList
	Endif

	IF _mCheckUpdtHistTable
		msqlstr = "select Top 1 UpdtMonth from "+_mDbName+"..UpdtHist Where UpdtMonth = ?_mupdtmonth And IntVersion = ?_mIntVersion and UpdtDone = 1"	&&vasant300112
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
		IF nretval <= 0 OR !USED('_tmpCoList')
			DO ExitUpdt
			RETURN  .f.
		ELSE
			IF RECCOUNT() = 1
				REPLACE RunUpdt WITH 1 IN _UpdtCoList
			Endif	
		ENDIF
	Endif	

	msqlstr = "select Top 1 UpdtMonth,IntVersion from "+_mDbName+"..UpdtHist Where UpdtDone = 1 Order By UpdtMonth Desc"	&&vasant300112
	nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
	IF nretval <= 0 OR !USED('_tmpCoList')
		DO ExitUpdt
		RETURN  .f.
	ELSE
		IF RECCOUNT() = 1
			_tmpIntVersion = Padl(ALLTRIM(STREXTRACT(_tmpCoList.IntVersion,'','.',0,2)),10,'0') +  Padl(ALLTRIM(STREXTRACT(_tmpCoList.IntVersion,'.','.',1,2)),10,'0') + 	Padl(ALLTRIM(STREXTRACT(_tmpCoList.IntVersion,'.','.',2,2)),10,'0') + 	Padl(ALLTRIM(STREXTRACT(_tmpCoList.IntVersion,'.','.',3,2)),10,'0')
			REPLACE NewVersion with _tmpIntVersion in _UpdtCoList	
			IF _mCheckUpdtHistTable
				IF (TTOD(_tmpCoList.UpdtMonth) > _mupdtmonth) OR (TTOD(_tmpCoList.UpdtMonth) = _mupdtmonth AND _tmpIntVersion >  _varIntVersion)
					REPLACE RunUpdt WITH 2 IN _UpdtCoList
				Endif	
			Endif	
		Endif	
	Endif

	SELECT _UpdtCoList
ENDSCAN

SELECT _UpdtCoList
COUNT TO _mReccnt FOR UpdtVersion = _mVersion AND EMPTY(User)
IF RECCOUNT() = _mReccnt + 1
	LOCATE FOR CompId != 0
	IF FOUND()
		_mUpdtMonth  	= _UpdtCoList.UpdtMonth
		_mIntVersion 	= _UpdtCoList.IntVersion
		_mUpdtVersion 	= _UpdtCoList.UpdtVersion
		_mRunUpdt		= _UpdtCoList.RunUpdt
		LOCATE FOR CompId = 0
		IF FOUND()
			REPLACE UpdtMonth WITH _mUpdtMonth,;
				IntVersion WITH _mIntVersion,;
				UpdtVersion WITH _mUpdtVersion,;
				RunUpdt With _mRunUpdt IN _UpdtCoList
		Endif	
	Endif	
Endif

UPDATE _ZipDetail SET Sel = .t.,Lock = 'New Updates' WHERE DTOS(Updt_Date)+IntVersion > (SELECT MIN(DTOS(UpdtMonth)+IntVersion) FROM _UpdtCoList)
nretval=sqlconobj.sqlconnclose("nHandle")

DO FORM FrmCoList
READ EVENTS

DO ExitUpdt



PROCEDURE ExitUpdt
	IF TYPE('sqlconobj') = 'O'
		nretval=sqlconobj.sqlconnclose("nHandle")
		nretval=sqlconobj.sqlconnclose("nhandle_master")
	ENDIF 	
	IF USED('_tmpCoList')
		USE IN _tmpCoList
	Endif	
	IF USED('_UpdtCoList')
		USE IN _UpdtCoList
	Endif	
	IF USED('_tmptbl1')
		USE IN _tmptbl1
	Endif	
	IF USED('_ZipDetail')
		USE IN _ZipDetail
	Endif	
	IF USED('ErrLog')
		USE IN ErrLog
	Endif
	RELEASE mudprodcode,usquarepass,_mproddesc,_mupdtmonth,_mIntVersion,_mlastupdtmonth,_mFldrName,_mDataBackFldrName,_mManualUpdateFldrName,_mFinalUpdateFldrName,_ErrMsg,_mmachine,_mlogip,_mErrHtmlName,_AutoMainPath,_mDocFldrName,_mlastupdtversion	&&vasant280312
	RELEASE _mCheckUpdtHistTable,_mUpdateUpdtHistTable,_mAutoUpdaterCaption
	CLOSE all
	WAIT CLEAR
	exitclick = .T.

