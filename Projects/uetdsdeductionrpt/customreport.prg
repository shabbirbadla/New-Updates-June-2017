PARA ucaption1,ucond1

IF ! "datepicker"  $ LOWER(SET("Classlib"))
	SET CLASSLIB TO APath+"\Class\datepicker.vcx" ADDITIVE
ENDIF
*!*	If !("GRIDFIND.VCX" $ Upper(Set("Classlib"))) 
*!*		Set Classlib To gridfind.vcx Additive
*!*	Endif


DO FORM uefrm_tdsdeduct WITH ucaption1,ucond1
RETURN