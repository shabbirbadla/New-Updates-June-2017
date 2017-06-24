_curvouobj = _Screen.ActiveForm
&&vasant061009
IF TYPE('_curvouobj.mainalias') = 'C'
	IF UPPER(_curvouobj.mainalias) <> 'MAIN_VW'
		RETURN 
	Endif	
ENDIF
&&vasant061009
*SET datasession to _curvouobj.datasessionid	&&vasant071009
IF TYPE('_curvouobj.PcvType') = 'C'
	If Inlist(_curvouobj.pcVtype,'IP','OP') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
		_curvouobj.cmdBom.height= _curvouobj.cmdnarration.height
		_curvouobj.cmdBom.width = _curvouobj.cmdnarration.width + 30
		_curvouobj.cmdBom.top   = _curvouobj.voupage.top - _curvouobj.cmdBom.height
		_curvouobj.cmdBom.left  = _curvouobj.voupage.left
	Endif
Endif