_curvouobj = _Screen.ActiveForm
If Used('Gen_SrNo_Vw')
	Use In Gen_SrNo_Vw
Endif
&&Rup-->14/08/2009
If (([vuexc] $ vchkprod))
	msqlstr="SELECT * FROM Gen_SrNo where entry_ty='"+Alltrim(main_vw.entry_ty)+"' and tran_cd="+Str(main_vw.tran_cd)
	nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"Gen_SrNo_Vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
	If nretval < 1 Or !Used("Gen_SrNo_Vw")
	Else
		Select Gen_SrNo_Vw
		Index On ItSerial Tag ItSerial
	Endif
Endif
&&Rup<--14/08/2009
*!*----->IP & OP FOR BatchProcess(Rup)
If ([vuexc] $ vchkprod)
*	If (Inlist(main_vw.entry_ty,'IP','OP','ST','DC')) And !Used("PROJECTITREF_vw")
*Birendra Bug-4543 on 31/07/2012 : Commented and modified with Below one:
	If (Inlist(main_vw.entry_ty,'IP','OP','ST','DC','WI','WO') OR (Inlist(main_vw.entry_ty,'PT') AND oglblprdfeat.udchkprod('AutoTran')) ) And !Used("PROJECTITREF_vw") &&'PT' BIRENDRA FOR TKT-8452(BOM IP-OP)
		msqlstr="SELECT * FROM PROJECTITREF where entry_ty='"+Alltrim(main_vw.entry_ty)+"' and tran_cd="+Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"PROJECTITREF_vw1","_curvouobj.nhandle",_curvouobj.DataSessionId)
		A1=Afields(ARPROJECTITREF_vw1,'PROJECTITREF_vw1')
		For nCount = 1 To A1
			If ARPROJECTITREF_vw1(nCount,2)='T'
				ARPROJECTITREF_vw1(nCount,2)='D'
			Endif
		Endfor
		Create Cursor PROJECTITREF_vw From Array ARPROJECTITREF_vw1
		Insert Into PROJECTITREF_vw Select * From PROJECTITREF_vw1
		If Used('PROJECTITREF_vw1')
			Use In PROJECTITREF_vw1
		Endif
	Endif
Endif
*!*<-----IP & OP FOR BatchProcess(Rup)

&&Changes done by vasant on 13/04/2012 as per Bug 3545 (Issue in RG Page number Generation After Updating BUG-1348)
If "vutex" $ vchkprod
	IF CoAdditional.RgPg_ReSet = .t.
		msqlstr="SELECT * FROM Gen_SrNo where entry_ty = ?main_vw.entry_ty and tran_cd = ?main_vw.tran_cd And Sta_dt = ?Company.Sta_dt And End_dt = ?Company.End_dt"
		nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"TmpTbl_Vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
		If nretval < 1 Or !Used("TmpTbl_Vw")
		ELSE
			SELECT TmpTbl_Vw
			IF RECCOUNT() > 0
				SELECT Item_vw
				REPLACE ALL RgPage WITH '' IN Item_vw

				Select TmpTbl_Vw
				SCAN
				
					SELECT Item_vw
					IF SEEK(TmpTbl_Vw.Entry_ty+STR(TmpTbl_Vw.Tran_cd)+TmpTbl_Vw.ItSerial,'Item_vw','ETIts')
						REPLACE RgPage WITH TmpTbl_Vw.NPgNo IN Item_vw
					ENDIF
					
					Select TmpTbl_Vw
				ENDSCAN
			Endif	
		Endif
	Endif
Endif
&&Changes done by vasant on 13/04/2012 as per Bug 3545 (Issue in RG Page number Generation After Updating BUG-1348)

* Birendra : 22 mar 2011  for Order Amendment
IF 'trnamend' $ vChkprod
  DO VouAftEdit IN MainPrg
ENDIF 