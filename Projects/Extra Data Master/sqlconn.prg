define class sqlconnudobj as custom

*!*		procedure init
*!*			set datasession to 2
*!*		endproc
*!*		procedure error
*!*		wait window "Reach"
*!*		endproc

 	function dataconn(_sqltodo as string ,_sqldbname as string,_sqlcond as string,_sqltbl as string,_sqlconhandle as string,sqlSession as integer,_sqltransaction as logical,_sqlmessage as string)

	if type('sqlSession') = 'U' or type('sqlsession') = 'L'
		set datasession to 1
	else
		set datasession to sqlSession
	endif	
		
	if type('vumess')='U'
		store '' to vumess
	endif 

	if type('_sqltransaction')='U'
		store .f. to _sqltransaction
	endif
	
	local malias,mmsg
	if !empty(_sqltbl)
		if used(_sqltbl)
			use in (_sqltbl)
		endif
	endif
	malias = alias()
	mmsg = iif(type('_sqlMessage') <> 'U',_sqlmessage,"")
	_sqlret = 0

	do while .t.
		mexit = .t.
		try
			do case
			case upper(_sqltodo) = 'CHK' 
				_sqlret = sqlgetprop(&_sqlconhandle,'ConnectTimeOut')
			case upper(_sqltodo) = 'EXE'
				_sqlret = sqlexec(&_sqlconhandle,"Select db_name() as dbName","_dbName")
				if _sqlret > 0
					select _dbname
					use
*!*						if upper(alltrim(_dbname.dbname)) <> upper(alltrim(_sqldbname))
*!*							_sqlret=sqldisconnect(&_sqlConHandle)
*!*							if _sqlret > 0
*!*								do conn with mvu_backend,mvu_server,mvu_user,mvu_pass,_sqldbname
*!*								select _dbname
*!*								use
*!*							else
*!*								=messagebox("Error Disconnecting "+alltrim(_dbname.dbname),64,"Visual Udyog")
*!*								select _dbname
*!*								use
*!*								return -1
*!*							endif
*!*						endif
				else
					=messagebox("Error connecting to Database",64,"Visual Udyog")
					select _dbname
					use
					return -1
				endif
				if _sqltransaction		&& if .t. then Begin Transaction
					_sqlret=sqlsetprop(&_sqlconhandle,'transactions',2)
					if _sqlret<=0
						mexit=.f.
						exit
					endif
				endif
				if !empty(malias)
					sele &malias
				endif
				_sqlret = sqlexec(&_sqlconhandle,_sqlcond,_sqltbl)
			endcase
		catch to errnum
			if errnum.errorno = 1466
				mexit = .f.
*!*					do conn with mvu_backend,mvu_server,mvu_user,mvu_pass,_sqldbname
				this.sqlconnects(mvu_backend,mvu_server,mvu_user,mvu_pass,_sqldbname,_sqlconhandle)
			else
				mmsg = mmsg + iif(!empty(mmsg), " chr(13) ","") + alltr(proper(message()))
				=messagebox(mmsg,64,vumess)
				nRetval=sqlconObj.sqlconnclose(&_sqlconhandle) && Connection Close
				if nretval<=0
					return .f.
				endif
			endif
		endtry
		if _sqlret < 0
			this.showerror(mmsg,_sqlconhandle)
			mexit = .t.
			nRetval=sqlconObj.sqlconnclose(&_sqlconhandle) && Connection Close
			if nRetval<=0
				return .f.
			endif
		else
			if used(_sqltbl)
				select (_sqltbl)
			endif	
		endif
		if mexit = .t.
			exit
		endif
	enddo

	return _sqlret
	endfunc

*-----------------*
* FUNCTION Insert *
*-----------------*
	function geninsert(pinsert,pexclude,pkeyfields,pfilenm,pplatform,pinclude)
	*pInsert --> Destination FileName
	*pExclude -> field Names to be excluded
	*pKeyfields  field names to be excluded
	*pFileNm	 Temp. file Name
	*pPlatform	  Vfp /SQL
	
	*----------------------------*
	*E.g. mSqlStr = genInsert("r_status","","'DESC','GROUP'","_rstatus",thisform.platform)
*----
	local mrunflds, mvalues,mtotflds
	sele &pfilenm
	dimen mflds(1)
	mtotflds=afields(mflds)
	mrunflds=""
	mvalues=""
	if type('pInclude') <> 'L' and !empty(pinclude)
		mtotflds = this.getfields(pinclude,@mflds)
	endif
	pexclude=upper(alltrim(pexclude))
	pkeyfields = upper(alltrim(pkeyfields))

*---Get Fields & values
	for i = 1 to mtotflds
		if !empty(pinclude)
			mrunflds = mrunflds+mflds(i,1)+ iif(mtotflds=1 or i=mtotflds,"",",")
			mvalues  = mvalues + "?" + pfilenm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")
			loop
		endif

		if !empty(pexclude)
			if inlist(upper(alltrim(mflds(i,1))),&pexclude)
				if i=mtotflds
					mrunflds=substr(mrunflds,1,len(alltrim(mrunflds))-1)
					mvalues =substr(mvalues,1,len(alltrim(mvalues))-1)
				endif
				loop
			endif
		endif
		if empty(pkeyfields)
			mrunflds = mrunflds+mflds(i,1)+ iif(mtotflds=1 or i=mtotflds,"",",")
			mvalues  = mvalues + "?" + pfilenm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")
			loop
		endif
		if inlist(upper(alltr(mflds(i,1))),&pkeyfields)
			do case
			case pplatform="0"
				mrunflds = mrunflds+ pfilenm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")
			case pplatform="1"
				mrunflds = mrunflds+"["+mflds(i,1)+"]"+ iif(mtotflds=1 or i=mtotflds,"",",")
			endcase
		else
			mrunflds = mrunflds+mflds(i,1)+ iif(mtotflds=1 or i=mtotflds,"",",")
		endif
		mvalues  = mvalues + "?" + pfilenm+"."+mflds(i,1)+iif(mtotflds=1 or i=mtotflds,"",",")
	next
	minsert = "insert into " + alltr(pinsert) + "("+mrunflds+") values ("+mvalues +")"
	return (minsert)
	endfunc

*--------------------*
* FUNCTION genupdate *
*--------------------*
	function genupdate (pupdate, pexclude, pkeyfields,pfilenm,pplatform,pcond,pinclude)

	local msqlstr, mflds, mtotflds,mrunflds
	msqlstr=""
	sele &pfilenm
	dimen mflds(1)
	mtotflds=afields(mflds)
	mrunflds= ""
	pkeyfields=upper(alltrim(pkeyfields))
	pexclude=upper(alltrim(pexclude))
	if type('pInclude') <> 'L' and !empty(pinclude)
		mtotflds = this.getfields(pinclude,@mflds)
	endif
*for i = 1 to iif( !empty(pinclude), mTotflds,mTotflds-1)
	for i = 1 to mtotflds
		if !empty(pinclude)
			mrunflds = mrunflds+mflds(i)+"="+"?"+mflds(i) + ;
				iif(mtotflds=1 or i=mtotflds,"",",")
			loop
		endif
		if !empty(pexclude)
			if inlist(upper(alltrim(mflds(i,1))),&pexclude)
				if i=mtotflds
					mrunflds=substr(mrunflds,1,len(alltrim(mrunflds))-1)
				endif
				loop
			endif
		endif
		if empty(pkeyfields)
			mrunflds = mrunflds+mflds(i,1)+"="+"?"+field(i)+ iif(mtotflds=1 or i=mtotflds,"",",")
			loop
		endif
		if inlist(upper(alltr(mflds(i,1))),&pkeyfields)
			do case
			case pplatform="0"				&& vfp
				mrunflds = mrunflds+pfilenm+".["+alltr(mflds(i,1))+"]="+"?"+pfilenm + "." + field(i)+ iif(mtotflds=1 or i=mtotflds,"",",")
			case pplatform="1"				&& Sql 2k
				mrunflds = mrunflds+"["+alltr(mflds(i,1))+"]="+"?["+field(i)+"]"+iif(mtotflds=1 or i=mtotflds,"",",")
			endcase
		else
			mrunflds = mrunflds+mflds(i,1)+"="+"?"+field(i)+ iif(mtotflds=1 or i=mtotflds,"",",")
		endif
	next
*	msqlstr = "Update "+alltr(pupdate)+" set "+mrunflds+" where "+pcond
	If !Empty(pCond) && Tushar
		mSqlStr = "Update "+Alltr(pUpdate)+" set "+mRunflds+" where "+pCond
	Else
		mSqlStr = "Update "+Alltr(pUpdate)+" set "+mRunflds
	Endif
	return (msqlstr)
	endfunc

*-------------------*
* FUNCTION gendelete*
*-------------------*
	function gendelete(pdelfile as string,pcond as string)
	local msqlstr
	msqlstr = "delete from "+alltr(pdelfile)+" where " +pcond
	return (msqlstr)
	endfunc

*------------------*
* FUNCTION seticon *
*------------------*
	function seticon(pobject as string)
	pobject.picture = apath + "\Bmp\Loc.Bmp"
	endfunc

*--------------------*
* FUNCTION showerror *
*--------------------*
	function showerror (pmsg as string,_sqlconhandle)
	local mret,merrmsg

	merrmsg = message()
	mret = sqlexec(&_sqlconhandle,"select @@error as Num", "_Error")
	sele _error
	do case
	case _error.num = 547
		if !empty(pmsg)
			=messagebox(pmsg + chr(13) + chr(13) + alltr(merrmsg),64,vumess)
		else
			=messagebox("Constraint violation Error" + chr(13) + chr(13) + alltr(merrmsg),64,vumess)
		endif
	otherwise
		if !empty(pmsg)
			pmsg = pmsg + iif(!empty(pmsg), chr(13) + chr(13),"") + alltr(merrmsg)
		else
			pmsg = alltr(merrmsg)
		endif
		=messagebox(pmsg,64,vumess)
		if type('statdesktop')='O'
			statdesktop.progressbar.visible = .f.		
		endif
	endcase
	use
	return
	endfunc

*--------------------*
* function getfields *
*--------------------*
	function getfields(pinclude as string, pmarr as String)
	local mctr, mpos, mprev
	mctr=1
	store 0 to mprev,mpos
	do while .t.
		mprev = mpos
		mpos=at(",",pinclude,mctr)
		dime pmarr(mctr)
		if mpos=0
			pmarr(mctr) = substr(pinclude,mprev+1)
		else
			pmarr(mctr) = substr(pinclude,mprev+1,(mpos-1)-mprev)
		endif
		mctr=mctr+1
		if mpos =0
			exit
		endif
	enddo
	return alen(pmarr)
	endfunc

	function sqlconnects(_mvu_backend as Character,_mvu_server as string,_mvu_user as string,_mvu_pass as string,_mvu_data as string,_sqlconhandle as string,mvu_public as logical)
	if type('mvu_public')='U'
		mvu_public = .f.
	endif
	
	if mvu_public = .f.
		_mvu_user1=this.dec(this.ondecrypt(_mvu_user))
		_mvu_pass1=this.dec(this.ondecrypt(_mvu_pass))
	else
		_mvu_user1=_mvu_user
		_mvu_pass1=_mvu_pass
	endif 	
	do case
	case _mvu_backend = "1" && SQL server
		=sqlsetprop(0,'DispLogin',3)
		if mvu_public = .f.
			constr = "Driver={SQL Server};server=&_mvu_server;database=&_mvu_data;uid=&_mvu_user1;pwd=&_mvu_pass1;"
		else
			constr = "Driver={SQL Server};server="+_mvu_server+";database="+_mvu_data+";uid="+_mvu_user1+";pwd="+_mvu_pass1+";"
		endif
		&_sqlconhandle = sqlstringconnect(constr)
		=sqlsetprop(&_sqlconhandle,"IdleTimeout",15) 
		=sqlsetprop(&_sqlconhandle,"DisconnectRollback",.t.) 
		if &_sqlconhandle < 0
			messagebox("Cannot Establish connection to SQL Server..."+chr(13)+chr(13)+"Please check the Server Name/User Id/Password",16,vumess)
			retu .f.
		endif
	case _mvu_backend = "0"
		_mvu_data= iif(empty(_mvu_data),sys(5)+allt(curd())+"invoice.dbc",_mvu_data)
		constr   = 'DRIVER=Microsoft Visual FoxPro Driver;UID=;Deleted=Yes;Null=Yes;Collate=Machine;BackgroundFetch=Yes;Exclusive=No;SourceType=DBC;SourceDB=&_mvu_data'
		&_sqlconhandle   = sqlstringconnect(constr)
		if &_sqlconhandle < 0
			messagebox("Cannot Establish connection ...",16,vumess)
			retu .f.
		endif
	endcase
	_mvu_user = this.onencrypt(this.enc(_mvu_user1))
	_mvu_pass = this.onencrypt(this.enc(_mvu_pass1))
	_vfp.StatusBar='Connection Open'
*!*		if type('ConnectionStatus') != 'U'  && update connection status
*!*			ConnectionStatus = .t.
*!*		endif

	endfunc

	function onencrypt(lcvariable as String)
	lcreturn = ""
	for i=1 to len(lcvariable)
		lcreturn=lcreturn+chr(asc(substr(lcvariable,i,1))+asc(substr(lcvariable,i,1)))
	endfor
	return lcreturn
	endfunc

	function ondecrypt(lcvariable as string)
	lcreturn = ""
	for i=1 to len(lcvariable)
		lcreturn=lcreturn+chr(asc(substr(lcvariable,i,1))/2)
	endfor
	return lcreturn
	endfunc

	function enc(mcheck as string)
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
	endfunc

	function dec(mcheck as String)
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
	endfunc

	function sqlconnclose(nconnhandle as string)
	nretval=sqldisconnect(nconnhandle)
	if nretval<=0
		=messagebox("SQL disconnect Error"+chr(13)+message(),64,vumess)
	else
		_vfp.StatusBar='Connection close'
	endif
	return nretval
	endfunc

	function _sqlrollback(nconnHandle)
		nretval=sqlrollback(nconnHandle)
		if nretval<=0
			=messagebox("SQL Rollback error"+chr(13)+message(),64,vumess)
			nretval=this.sqlconnclose(nconnHandle)
			if nretval<=0
				=messagebox("SQL disconnect Error"+chr(13)+message(),64,vumess)	
			endif
		else
			_vfp.StatusBar='Transaction ROLLBACK'
		endif
		return nRetval
	endfunc 
	
	function _sqlcommit(nconnHandle)
		nretval=sqlcommit(nconnHandle)
		if nretval<=0
			=messagebox("SQL Commit error"+chr(13)+message(),64,vumess)
			nretval=this.sqlconnclose(nconnHandle)
			if nretval<=0
				=messagebox("SQL disconnect Error"+chr(13)+message(),64,vumess)	
			endif
		else
			_vfp.StatusBar='Transaction COMMITTED'
		endif
		return nRetval
	endfunc 
	
	function sqlconnopen(apath as string)
	Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
		string cSection, String cKey, String cDefault, String @cBuffer, ;
		integer nBufferSize, String cINIFile
	Declare Integer WritePrivateProfileString In Win32API As WritePrivStr ;
		string cSection, String cKey, String cValue, String cINIFile

	Public mvu_backend,mvu_server,mvu_user,mvu_pass

	if !file(apath+"visudyog.ini")
		messagebox("Configuration File Not found",32,vumess)
		retu .f.
	endif

	mvu_one     = space(2000)
	mvu_two     = 0
	mvu_two	    = getprivstr([Settings],"Backend", "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
	mvu_backend = left(mvu_one,mvu_two)
	mvu_two     = getprivstr([Server],"Name", "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
	mvu_server  = left(mvu_one,mvu_two)
	mvu_two     = getprivstr([Server],This.onencrypt(This.enc("User")), "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
	mvu_user    = left(mvu_one,mvu_two)
	mvu_two     = getprivstr([Server],This.onencrypt(This.enc("Pass")), "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
	mvu_pass    = left(mvu_one,mvu_two)
	mvu_backend = iif(empty(mvu_backend),"0",mvu_backend)
	endfunc
enddefine
