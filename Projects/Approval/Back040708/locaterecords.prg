lparameters pSessionId
SessionId = 1
if type('pSessionId') = 'N'
	SessionId = pSessionId
endif
mStartRange = 0
SqlConObj 	= newobject("SqlConNudObj","SqlConnection",xapps)
nHandle		= 0

mSqlQry1 	= "select range,padname,barname,progname from Com_menu where menutype='Transaction'"
nRetval		= SqlConObj.DataConn("EXE",company.dbname,mSqlQry1,"_comtest","nHandle",SessionId,.t.)
if nRetval<0
	=messagebox("Menu table error"+chr(13)+proper(message()),48,vuMess)
	return .f.
endif

mSqlQry1 	= "select top 1 range,padname,barname,entry_ty,party_nm,inv_sr,[rule],cate,SPACE(100) as caption,SPACE(10) as range,convert(bit,0) as rights from main,com_menu where 1=2"
nRetval		= SqlConObj.DataConn("EXE",company.dbname,mSqlQry1,"transactions","nHandle",SessionId,.t.)
if nRetval<0
	=messagebox("Menu table error"+chr(13)+proper(message()),48,vuMess)
	return .f.
endif
nRetval=SqlConObj.sqlconnclose("nHandle")
if nRetval<>1
	return .f.
endif

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
	v1 		   = Progname
	v3 		   = 1

	do while .t.
		a1 = at('"',v1)
		a1 = iif(a1 < 1,1,a1)
		v2 = subs(v1,a1)
		a2 = at('"',v2,2)
		a2 = iif(a2 < 1,1,a2)
		v2 = subs(v1,a1,a2)
		v1 = subs(v1,a1+a2+1)
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
		case v3 = 7
			_vvRange = v2
		endcase
		if v3 > 7
			exit
		endif
		v3 = v3 + 1
	enddo
	if !empty(_vvent)
		insert into transactions values(_comtest.range,_comtest.padname,_comtest.barname,_vvent,_vvparty,_vvseries,_vvRule,_vvCate,_vvCaption,_vvRange,.f.)
	endif
	select _comtest
	if !eof()
		skip
	endif
enddo
if used('_comtest')
	select _comtest
	use in 	_comtest
endif
select transactions
