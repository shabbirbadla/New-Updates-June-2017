_curvouobj = _Screen.ActiveForm
If Used('Gen_SrNo_Vw')
	USE IN Gen_SrNo_Vw
Endif	
&&Rup-->14/08/2009
IF (([vuexc] $ vchkprod))
		msqlstr="SELECT * FROM Gen_SrNo where entry_ty='"+alltrim(main_vw.entry_ty)+"' and tran_cd="+STR(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"Gen_SrNo_Vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
		If nretval < 1 OR !Used("Gen_SrNo_Vw")
		ELSE
			SELECT Gen_SrNo_Vw
			INDEX On ItSerial TAG ItSerial
		Endif	
ENDIF 
&&Rup<--14/08/2009
*!*----->IP & OP FOR BatchProcess(Rup)
IF ([vuexc] $ vchkprod)
	IF (INLIST(main_vw.entry_ty,'IP','OP','ST','DC')) AND !USED("PROJECTITREF_vw")
		msqlstr="SELECT * FROM PROJECTITREF where entry_ty='"+alltrim(main_vw.entry_ty)+"' and tran_cd="+STR(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"PROJECTITREF_vw1","_curvouobj.nhandle",_curvouobj.DataSessionId)
		A1=Afields(ARPROJECTITREF_vw1,'PROJECTITREF_vw1')
		FOR nCount = 1 TO A1
		    IF ARPROJECTITREF_vw1(nCount,2)='T'
	    		ARPROJECTITREF_vw1(nCount,2)='D'
		    ENDIF
		ENDFOR
		Create Cursor PROJECTITREF_vw From Array ARPROJECTITREF_vw1
		INSERT INTO PROJECTITREF_vw SELECT * FROM PROJECTITREF_vw1
		IF USED('PROJECTITREF_vw1')
			USE IN PROJECTITREF_vw1
		ENDIF
	ENDIF
ENDIF 
*!*<-----IP & OP FOR BatchProcess(Rup)
