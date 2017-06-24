parameters pEntryType,p_TranCd,pSessionId,pAddEdit
****Versioning****  Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('TOUEVOUCHER',_CurrVerVal,JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F.
	ENDIF
	IF _VerRetVal  = 'NO'
		Return .F.
	Endif
****Versioning****

if empty(pEntryType) or type("pentrytype")="U"
	=messagebox("Invalid transaction selection",0,vuMess)
	return .f.
endif

SessionId=1
if type('pSessionId')!='U'
	SessionId = pSessionId
endif
set datasession to SessionId
if type("p_TranCd")="U"
	p_TranCd=0
endif

local old_alias,filt_exp, INDEXBUF
SqlConObj = newobject("SqlConNudObj","SqlConnection",xapps)
nHandle = 0
INDEXBUF 	= p_TranCd
AddEdit		= iif(type('pAddEdit')='U',.t.,pAddEdit)
old_alias 	= " "
filt_exp 	= " "
filt_exp 	= filter()
old_alias 	= alias()
view_on 	= .t.

nRetval		= SqlConObj.DataConn("EXE",company.dbname,"select * from lcode where Entry_ty=?pEntryType","_lcodeChk","nHandle",SessionId,.f.)	&&vasant
if nRetval<0
	=messagebox("Transaction code master error",48,vuMess)
	return .f.
ENDIF

select _lcodeChk
locate
*!*	tablename = alltrim(iif(!empty(bcode_nm),alltrim(bcode_nm),alltrim(entry_ty))+'MAIN') &&Rup 06/01/2010
tablename = alltrim(ALLTRIM(  iif(ext_vou=.t.,alltrim(bcode_nm),alltrim(entry_ty))  )+'MAIN')
select _lcodeChk
use in _lcodeChk

nRetval= SqlConObj.DataConn("EXE",company.dbname,'select name from sysobjects where name=?tablename',"_exists","nHandle",SessionId,.f.)	&&vasant
if nRetval<0
	=messagebox("Table doesnot exists",48,vuMess)
	return .f.
ENDIF

if reccount('_exists')=0
	=messagebox("Table doesnot exists",48,vuMess)
	RETURN .f. &&Rup 06/01/2010
	tablename='Main'
ENDIF

select _exists
use in _exists
*nRetval= SqlConObj.DataConn("EXE",company.dbname,'select * from '+tablename+' where tran_cd=?p_trancd',"_MainTableZoom","nHandle",SessionId,.f.)	&&vasant	&&vasant140810	TKT-381
nRetval= SqlConObj.DataConn("EXE",company.dbname,'select Entry_ty,Tran_cd,Party_nm,Inv_sr,[Rule],Cate from '+tablename+' where tran_cd=?p_trancd',"_MainTableZoom","nHandle",SessionId,.f.)	&&vasant	&&vasant140810	TKT-381
if nRetval<0
	=messagebox("Menu table error"+chr(13)+proper(message()),48,vuMess)
	return .f.
endif
nRetval=SqlConObj.sqlconnclose("nHandle")
if nRetval<0
	return .f.
ENDIF

sele _MainTableZoom
set filter to
go top
locate for tran_cd = p_TranCd and entry_ty = pEntryType
if found()
	mzmtype=1
	p_entry_ty  = alltrim(_MainTableZoom.entry_ty)
	_vment      = alltrim(_MainTableZoom.entry_ty)
	_vmparty    = alltrim(_MainTableZoom.Party_nm)
	_vmseries   = alltrim(_MainTableZoom.Inv_sr)
	_vmRule     = alltrim(_MainTableZoom.rule)
	_vmCate     = alltrim(_MainTableZoom.Cate)
	_vvent      = ''
	_vvparty    = ''
	_vvseries   = ''
	_vvRule     = ''
	_vvCate     = ''
	_vvCaption  = ''
	_vvRange	= ''
	_vvcond7    = ''
	_vvcond7l   = .t.	&&vasant
	m.vvPrompt  = ''

	if used('_comtest')
		select _comtest
		use in _comtest
	endif
	
	mSqlQry1 = 'select Range,Padname,Barname,Progname from Com_menu '
	mSqlQry2 = 'where Progname like '+chr(39)+'%UEVOUCHER%'+chr(39)+' and Progname like '+chr(39)+'%"'+alltrim(p_entry_ty)+'"%'+chr(39)
	nRetval= SqlConObj.DataConn("EXE",company.dbname,mSqlQry1+mSqlQry2,"_comtest","nHandle",SessionId,.f.)	&&vasant
	if nRetval<0
		=messagebox("Menu table error"+chr(13)+proper(message()),48,vuMess)
		return .f.
	ENDIF
	&&vasant
*!*		nRetval=SqlConObj.sqlconnclose("nHandle")
*!*		if nRetval<0
*!*			return .f.
*!*		endif
	&&vasant
	select _comtest
	locate
	do while !eof()
		_vvent     = ''
		_vvparty   = ''
		_vvseries  = ''
		_vvRule    = ''
		_vvCate    = ''
		_vvCaption = ''
		_vvRange   = ''
		_vvcond7   =''
		_vvcond7l  = .t.	&&vasant
		v1 		   = Progname
		v3 		   = 1

		do while .t.
			a1 = at('"',v1)
			a1 = iif(a1 < 1,1,a1)
			v2 = subs(v1,a1)
			a2 = at('"',v2,2)
			a2 = iif(a2 < 1,1,a2)
			v2 = subs(v1,a1,a2)
			v1  = subs(v1,a1+a2+1)
			v2 = allt(strtran(v2,'"',''))
			do case
			case v3 = 1
				_vvent = v2
			case v3 = 2
				_vvparty = v2
			case v3 = 3
				_vvseries = v2
			case v3 = 4
				_vvRule = v2
			case v3 = 5
				_vvCate = v2
			case v3 = 6
				_vvCaption = v2
			case v3 = 8
				_vvRange = v2
			case v3 = 7
				_vvcond7 = v2 		
				&&vasant		
				IF !EMPTY(_vvcond7)
					_fldname = _vvcond7
					sql_var = ''	
					DO WHILE !EMPTY(_fldname)
						_fldname1  =  IIF(AT(',',_fldname) > 0,SUBSTR(_fldname,1,AT(',',_fldname)-1),_fldname)
						_fldname1a =  IIF(AT(':',_fldname1) > 0,SUBSTR(_fldname1,1,AT(':',_fldname1)-1),_fldname1)
						_fldname1a =  IIF(AT('.',_fldname1a) > 0,SUBSTR(_fldname1a,AT('.',_fldname1a)+1),_fldname1a)	
						_fldname1b =  IIF(AT(':',_fldname1,1) > 0,SUBSTR(_fldname1,AT(':',_fldname1,1)+1),'')
						_fldname1b =  IIF(AT(':',_fldname1b) > 0,SUBSTR(_fldname1b,1,AT(':',_fldname1b)-1),_fldname1b)	
						_fldname1c =  IIF(AT(':',_fldname1,2) > 0,SUBSTR(_fldname1,AT(':',_fldname1,2)+1),'')
						_fldname1c =  IIF(AT(':',_fldname1c) > 0,SUBSTR(_fldname1c,1,AT(':',_fldname1c)-1),_fldname1c)	
						IF !EMPTY(_fldname1a) AND !EMPTY(_fldname1b) AND _fldname1c = 'D'
							sql_var = sql_var + " And ["+_fldname1a +"] = '"+_fldname1b+"'"
							_vvcond7l = .f.
						Endif
					    _fldname =  IIF(AT(',',_fldname) > 0,SUBSTR(_fldname,AT(',',_fldname)+1),'')
					ENDDO
					IF !EMPTY(sql_var)
						mSqlQry1 = 'select top 1 entry_ty from '+tablename+' where tran_cd = ?p_TranCd '+sql_var
						nRetval= SqlConObj.DataConn("EXE",company.dbname,mSqlQry1,"_exists","nHandle",SessionId,.f.)
						if nRetval<0
							=messagebox("Main table error"+chr(13)+proper(message()),48,vuMess)
							return .f.
						ELSE
							SELECT _exists
							IF RECCOUNT() = 1
								_vvcond7l = .t.
							ENDIF
							if used('_exists')
								select _exists
								use in _exists
							endif
						ENDIF
					Endif
				Endif		
				&&vasant
			endcase
			if v3 > 8	&&vasant
				exit
			endif
			v3 = v3 + 1
		enddo
		
		sele _comtest	&&vasant

		*********** Changed By Sachin N. S. on 22/04/2010 for L1S-238 *********** Start
*!*			if iif(!empty(_vvent),alltrim(_vvent) = alltrim(_vment),.t.) and iif(!empty(_vvparty),alltrim(_vvparty) = alltrim(_vmparty),.t.) ;
*!*					and iif(!empty(_vvseries),alltrim(_vvseries) = alltrim(_vmseries),.t.) and iif(!empty(_vvRule),alltrim(_vvRule) = alltrim(_vmRule),.t.) ;
*!*					and iif(!empty(_vvCate),alltrim(_vvCate) = alltrim(_vmCate),.t.) and _vvcond7l = .t.	&&vasant

		if iif(!empty(_vvent),UPPER(alltrim(_vvent)) = UPPER(alltrim(_vment)),.t.) and iif(!empty(_vvparty),UPPER(alltrim(_vvparty)) = UPPER(alltrim(_vmparty)),.t.) ;
				and iif(!empty(_vvseries),UPPER(alltrim(_vvseries)) = UPPER(alltrim(_vmseries)),.t.) and iif(!empty(_vvRule),UPPER(alltrim(_vvRule)) = UPPER(alltrim(_vmRule)),.t.) ;
				and iif(!empty(_vvCate),UPPER(alltrim(_vvCate)) = UPPER(alltrim(_vmCate)),.t.) and _vvcond7l = .t.	&&vasant
		*********** Changed By Sachin N. S. on 22/04/2010 for L1S-238 *********** End

			m.vvPrompt=alltrim(Padname)+alltrim(Barname)
			exit
		endif
		sele _comtest
		if !eof()
			skip
		endif
	enddo
	if used('_comtest')
		select _comtest
		use in _comtest
	endif
	&&vasant
	nRetval=SqlConObj.sqlconnclose("nHandle")
	if nRetval<0
		return .f.
	endif
	if empty(m.vvPrompt)
		=messagebox("No Records Found",48,vuMess)
		return .f.
	ENDIF
	&&vasant

	select _MainTableZoom
	if !empty(m.vvPrompt)
		_vvent    = padr(_vvent,len(_MainTableZoom.entry_ty),' ')
		_vvparty  = padr(_vvparty,len(_MainTableZoom.Party_nm),' ')
		_vvseries = padr(_vvseries,len(_MainTableZoom.Inv_sr),' ')
		_vvRule   = padr(_vvRule,len(_MainTableZoom.rule),' ')
		_vvCate   = padr(_vvCate,len(_MainTableZoom.Cate),' ')

		local lbar,lpad,lpromp,lhot,llhot,lprogname,lnumitem,lcond,lskip,lcprog
		store space(1) to lbar,lpad,lpromp,lhot,llhot,lprogname,lskip,lcprog
		lcond=.f.
		lnumitem=0
		if used('padcursor')
			select padcursor
			use in padcursor
		endif

		mSqlQry = "select Padname,Barname,mdefault,prompname,numitem,hotkey,labkey,Progname,skipfor,cprog ;
			from Com_menu where Padname = 'RECORD' or Padname = 'EDIT' order by Padname,barnum"
		nRetval= SqlConObj.DataConn("EXE",company.dbname,mSqlQry,"padcursor","nHandle",SessionId,.f.)	&&vasant
		if nRetval<0
			=messagebox("Menu table error"+chr(13)+proper(message()),48,vuMess)
			return .f.
		endif
		nRetval=SqlConObj.sqlconnclose("nHandle")
		if nRetval<0
			return .f.
		endif

		select padcursor
		loca
		do while !eof()
			counter=1
			lpad = Padname
			do while Padname = lpad and !eof()
				wantdefault = mdefault
				lbar		= alltrim(Barname)
				lpromp		= "'"+alltrim(prop(prompname))+"'"
				lnumitem	=numitem
				lhot		= alltrim(hotkey)
				llhot		= iif(!empty(labkey),"'"+alltrim(labkey)+"'","''")
				lprogname	=alltrim(Progname)
				lskip		=alltrim(skipfor)
				lcprog		=cprog
				lcond=.t.

				if rel_bar(lpad,lbar, wantdefault)=.t.
					if empty(lbar)
						define bar counter of &lpad prompt "\-"
					else
						if !empty(lnumitem)	and lcond=.t.
							if !empty(lhot)
								if empty(lskip)
									define bar counter of &lpad prompt &lpromp key &lhot ,&llhot
								else
									define bar counter of &lpad prompt &lpromp key &lhot ,&llhot ;
										skip for &lskip
								endif
							else
								if empty(lskip)
									define bar counter of &lpad prompt &lpromp
								else
									define bar counter of &lpad prompt &lpromp ;
										skip for &lskip
								endif
							endif
							on bar counter of &lpad activate popup &lbar
						else
							if !empty(lhot)
								if empty(lskip)
									define bar counter of &lpad prompt &lpromp key &lhot ,&llhot
								else
									define bar counter of &lpad prompt &lpromp key &lhot ,&llhot ;
										skip for &lskip
								endif
							else
								if empty(lskip)
									define bar counter of &lpad prompt &lpromp
								else
									define bar counter of &lpad prompt &lpromp ;
										skip for &lskip
								endif
							endif
							if !empty(lprogname)
								if !empty(lcprog)
									on selection bar counter of &lpad do vou_common in gware.mpr
								else
									on selection bar counter of &lpad &lprogname
								endif
							endif
						endif
					endif
					counter=counter+1
				endif
				select padcursor
				skip
			enddo
		enddo

		if used('padcursor')
			select padcursor
			use in padcursor
		endif
		do UeVoucher with _vvent,_vvparty,_vvseries,_vvRule,_vvCate,_vvCaption,INDEXBUF,AddEdit,_vvcond7,_vvRange
	endif
endif
select _MainTableZoom
use in _MainTableZoom
SqlConObj= null
release SqlConObj
