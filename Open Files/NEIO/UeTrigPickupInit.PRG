*!*	LPARAMETERS zthisform
*!*	IF !EMPTY(main_vw.rule)
*!*	SELECT header
*!*	SET FILTER TO ALLTRIM(header.rule)= ALLTRIM(main_vw.rule) 
*!*	COUNT TO aa
*!*		If aa = 0
*!*			=Messagebox("No Records Found for Pickup",48,vumess)
*!*			statdesktop.Message	= oldmessage
*!*			statdesktop.progressbar.Value = 0
*!*			statdesktop.progressbar.Visible = .F.
*!*			Return .F.
*!*		Endif

*!*	ENDIF 

**--Start => Added Code for Reference Pickup Entry 
Parameters CurFrmName
_curvouobj = _Screen.ActiveForm
If INLIST(Main_vw.Entry_ty,'RC','PH')
	_uetrigpickalias = Alias()
	etsql_tbl  = 'othitref'
	etsql_str  = "Select itref_tran,rentry_ty,ritserial,SUM(rqty) as rqty from "+etsql_tbl+"  where Rentry_ty='LC' group by itref_tran,rentry_ty,ritserial"
	etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[Tmppick_Vw],;
	"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)

	If etsql_con > 0 And Used('Tmppick_Vw')
		Select Header
		Set Filter To
		Select Detail
		Set Filter To

		Select Detail
		Replace All balqtynew With ORG_QTY In Detail
		Go Top
		Update Detail Set balqtynew = balqtynew - Tmppick_Vw.rqty ;
		from Detail,Tmppick_Vw Where Detail.Entry_ty + Str(Detail.tran_cd) + Detail.itserial = ;
		Tmppick_Vw.Rentry_ty+Str(Tmppick_Vw.Itref_tran)+Tmppick_Vw.ritserial

		Go Top
		Delete From Detail Where balqtynew <= 0
		Replace All balqty With balqtynew
		Go Top
		Select Header
		Delete From Header Where Entry_ty+Str(tran_cd) Not In (Select Entry_ty+Str(tran_cd) From Detail)
		Go Top
	Endif

	If Used('Tmppick_Vw')
		Use In Tmppick_Vw
	Endif
	Select (_uetrigpickalias)
Endif
**--End => Added Code for Reference Pickup Entry