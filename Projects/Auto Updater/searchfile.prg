LPARAMETERS _mSearchFldrNm,_mSearchExe,_mSearchExt,_mSearchTblNm
IF DIRECTORY(_mSearchFldrNm,1)
	IF TYPE('_mSearchExe') != 'C'
		_mSearchExe = ''
	Endif
	IF TYPE('_mSearchExt') != 'C'
		_mSearchExt = ''
	ENDIF
	_mSearchExe = UPPER(_mSearchExe)
	_mSearchExt = UPPER(_mSearchExt)

	IF USED(_mSearchTblNm)
		USE IN (_mSearchTblNm)
	Endif	
	_ErrMsg = ''
	SELECT 0
	CREATE CURSOR (_mSearchTblNm) (FName C(250),FPath M,FSize Numeric(10),FDate D,FTime C(10))
	
	SELECT 0
	CREATE CURSOR tmpFldrList (FldrName Memo,Srched Logical)
	APPEND BLANK IN tmpFldrList
	REPLACE FldrName WITH _mSearchFldrNm IN tmpFldrList
	DO WHILE .t.
		SELECT * FROM tmpFldrList WITH (buffering = .t.) WHERE Srched = .f. INTO cursor tmpFldrL1
		SELECT tmpFldrL1
		IF RECCOUNT() < 1
			EXIT
		ELSE
			SCAN
				_mRmFldrName = ALLTRIM(tmpFldrL1.FldrName)
				_mfcount  = ADIR(_mflist,Addbs(_mRmFldrName)+"*.*","D")
				FOR i1 = 1 TO _mfcount 
					_mRmFileName = Addbs(_mRmFldrName)+_mflist(i1,1)
					IF "D"$_mflist[i1,5] AND _mflist[i1,1] <> "." AND _mflist[i1,1] <> ".."
						SELECT tmpFldrList
						APPEND BLANK IN tmpFldrList
						REPLACE FldrName WITH _mRmFileName IN tmpFldrList
					Endif	
				ENDFOR
				RELEASE _mflist

				_mfcount  = ADIR(_mflist,Addbs(_mRmFldrName)+"*.*","D")
				FOR i1 = 1 TO _mfcount 
					_mRmFileName = Addbs(_mRmFldrName)+_mflist(i1,1)
					IF !"D"$_mflist[i1,5] 
						_SrchFile = .f.
						IF !EMPTY(_mSearchExe)
							IF INLIST(JUSTFNAME(UPPER(_mflist(i1,1))),&_mSearchExe)
								_SrchFile = .t.
							Endif	
						Endif	
						IF !EMPTY(_mSearchExt)
							IF INLIST(JUSTEXT(UPPER(_mflist(i1,1))),&_mSearchExt)
								_SrchFile = .t.
							Endif	
						Endif	
						IF _SrchFile = .t.
							TRY
								SELECT (_mSearchTblNm)
								APPEND BLANK IN (_mSearchTblNm)
								REPLACE FName WITH _mflist(i1,1),FPath WITH Addbs(_mRmFldrName),;
									FSize WITH _mflist(i1,2),FDate with _mflist(i1,3),FTime WITH _mflist(i1,4) IN (_mSearchTblNm)
							CATCH TO m_errMsg
								_ErrMsg = ALLTRIM(m_errMsg.Message)
							ENDTRY
						Endif					
					Endif
				ENDFOR
				RELEASE _mflist
				
				UPDATE tmpFldrList SET Srched = .t. WHERE ALLTRIM(FldrName) == ALLTRIM(_mRmFldrName)
				SELECT tmpFldrL1
			Endscan	
		Endif
	Enddo

	IF USED('tmpFldrList')
		USE IN tmpFldrList
	Endif	
	IF USED('tmpFldrL1')
		USE IN tmpFldrL1
	Endif	
	IF !EMPTY(_ErrMsg)
		RETURN .f.
	Endif
Endif
RETURN .t.