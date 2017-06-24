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
*Birendra : Bug-4543 on 20/09/2012 :Start:
	IF [procinv] $ vchkprod AND INLIST(_curvouobj.pcVtype,'WI','WO')
* Birendra : Bug-9016 on 22/02/2013 :Start:
		_curvouobj.cmdBom.height= _curvouobj.cmdnarration.height
		_curvouobj.cmdBom.width = _curvouobj.cmdnarration.width + 30
		_curvouobj.cmdBom.top   = _curvouobj.voupage.top - _curvouobj.cmdBom.height
		_curvouobj.cmdBom.left  = _curvouobj.voupage.left
* Birendra : Bug-9016 on 22/02/2013 :End:
		_curvouobj.cmdBom.tabstop=.t.
		_curvouobj.cmdBom.tabindex= _curvouobj.vouPage.tabindex-1 &&Birendra : Bug-4543 on 20/09/2012
	ENDIF 
*Birendra : Bug-4543 on 20/09/2012 :End:
ENDIF
* Birendra : 22 Mar 2011 for Order Amendment
IF 'trnamend' $ vChkprod
	DO VouAftActivate IN MainPrg
ENDIF 

***** Added by Sachin N. S. on 30/03/2016 for Bug-27503 -- Start
If Type('_curvouobj._udClsPointOfSale')='O'
	_curvouobj._udClsPointOfSale._ueTrigVouAftActivate()
Endif
***** Added by Sachin N. S. on 30/03/2016 for Bug-27503 -- End
