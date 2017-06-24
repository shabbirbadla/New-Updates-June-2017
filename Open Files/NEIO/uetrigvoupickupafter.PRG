IF TYPE('item_vw.u_pageno')="C" AND [vuexc] $ vchkprod
	replace u_pageno WITH "" FOR !EMPTY(dc_no) IN item_vw 
ENDIF 

&& Sandeep s.---->Start for TKT-4596
IF  main_Vw.entry_ty="SR"  AND  [vuexc] $ vchkprod 
 IF TYPE('item_vw.u_pageno')="C"
   	replace ALL u_pageno WITH ""  IN item_vw 
  endif 	
ENDIF 
&& Sandeep s.---->End for TKT-4596

&& Added By Shrikant S. on 05/11/2011 for Bug-259	&&Start
If Inlist(main_Vw.entry_ty,"S1")
	oFrm=_Screen.ActiveForm
	_datasessionid=_Screen.ActiveForm.DataSessionId
	If oFrm.Addmode Or oFrm.Editmode

		sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select * from AcdetAlloc where Entry_ty =?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd ],[AcdetAlloc_vw],;
			"This.Parent.nHandle",_datasessionid,.F.)
		If sql_con<0
			Return .F.
		Endif
		Select AcdetAlloc_vw
		Index On ItSerial Tag ItSerial addi
		Index On serty Tag serty AddI
		If Used('Itref_vw')
			Select Itref_vw
			mrecno=Iif(!Eof(),Recno(),0)
			Scan
				If Used('Acalloc_vw')
					Use In Acalloc_vw
				Endif
				sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select * from AcdetAlloc where Entry_ty =?Itref_vw.rEntry_ty and Tran_cd=?Itref_vw.itref_tran and itserial=?Itref_vw.ritserial],[Acalloc_vw],;
					"This.Parent.nHandle",_datasessionid,.F.)

				Select Acalloc_vw
				Replace entry_ty With Itref_vw.entry_ty,tran_cd With Itref_vw.tran_cd,ItSerial With Itref_vw.ItSerial In Acalloc_vw

				morg_qty=0
				mqty=0

				Select org_qty,qty From Detail Where entry_ty=Itref_vw.rentry_ty And tran_cd=Itref_vw.itref_tran And ItSerial=Itref_vw.rItSerial Into Array qtyarr
				morg_qty=qtyarr[1]
				mqty=qtyarr[2]
				qtyarr=Null

				Select item_vw
				Scan For ItSerial==Itref_vw.ItSerial
					Replace sTaxAmt With (Acalloc_vw.Amount/morg_qty)*mqty,SerbAmt With (Acalloc_vw.SerbAmt/morg_qty)*mqty;
						,SercAmt With (Acalloc_vw.SercAmt/morg_qty)*mqty,SerhAmt With (Acalloc_vw.SerhAmt/morg_qty)*mqty In item_vw
					Exit
				Endscan

				Select AcdetAlloc_vw
				Locate For entry_ty=Itref_vw.entry_ty And tran_cd=Itref_vw.tran_cd And ItSerial=Itref_vw.ItSerial
				If !Found()
					Select AcdetAlloc_vw
					Append Blank In AcdetAlloc_vw
					Replace entry_ty With Acalloc_vw.entry_ty, tran_cd With Acalloc_vw.tran_cd,serty With Acalloc_vw.serty,Amount With (Acalloc_vw.Amount/morg_qty)*mqty;
						sAbtPer With Acalloc_vw.sAbtPer, sAbtAmt With Acalloc_vw.sAbtAmt,sTaxable With (Acalloc_vw.sTaxable/morg_qty)*mqty, serAvail With Acalloc_vw.serAvail;
						SerbAmt With item_vw.SerbAmt, SercAmt With item_vw.SercAmt,SerhAmt With item_vw.SerhAmt,Sexpamt With Acalloc_vw.Sexpamt;
						SabtSr With Acalloc_vw.SabtSr, SsubCls With Acalloc_vw.SsubCls,SExNoti With Acalloc_vw.SExNoti,ItSerial With Acalloc_vw.ItSerial In AcdetAlloc_vw
				Endif

				Select Itref_vw
			Endscan
			If mrecno>0
				Select Itref_vw
				Go mrecno
			Endif
		Endif
		Select item_vw
		Go Top
		Select AcdetAlloc_vw
		Go Top

	Endif
Endif
&& Added By Shrikant S. on 05/11/2011 for Bug-259	&&End
