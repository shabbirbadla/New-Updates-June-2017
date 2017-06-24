&&-->Ipop(Rup)
*-->Checking Allocated entry
_Malias 	= Alias()
_mRecNo	= Recno()
_curvouobj = _Screen.ActiveForm
&&vasant061009
IF TYPE('_curvouobj.mainalias') = 'C'
	IF UPPER(_curvouobj.mainalias) <> 'MAIN_VW'
		RETURN 
	Endif	
ENDIF
&&vasant061009
*Set DataSession To _curvouobj.DataSessionId	&&vasant071009
IF TYPE('_curvouobj.PcvType') = 'C'
	IF _Screen.ActiveForm.voupage.page1.GrdItem.Column2.text1.Value<>VAL(_Screen.ActiveForm.voupage.page1.GrdItem.Column2.text1.tag)
		IF  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) AND Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records

			IF _curvouobj.PCVTYPE='WK' AND (_Screen.ActiveForm.voupage.page1.GrdItem.Column2.text1.Value < VAL(_Screen.ActiveForm.voupage.page1.GrdItem.Column2.text1.tag))
				etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty and aitserial=?item_vw.itserial"
				etsql_str1 = " union select top 1 aentry_ty from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and itserial=?item_vw.itserial"
				etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str+etsql_str1,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
				IF USED('_chkbom')
					IF RECCOUNT()>0
						SELECT _chkbom
						=MESSAGEBOX("Entry Passed Against /"+_chkBom.entry_ty+". Quantity Could not be Changed",16,vumess)
						_Screen.ActiveForm.voupage.page1.GrdItem.Column2.text1.Value=_Screen.ActiveForm.voupage.page1.GrdItem.Column2.text1.tag				
						USE IN _chkbom
						RETURN .F. 	
					ENDIF 			
					USE IN _chkbom
				ENDIF
			ENDIF 
			
		ENDIF 
	ENDIF 
Endif
If !Empty(_Malias)
	Select &_Malias
ENDIF
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
ENDIF
*<--Checking Allocated entry
&&<--Ipop(Rup)
