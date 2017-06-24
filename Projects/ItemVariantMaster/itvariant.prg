PARAMETERS oForm,vcode
If !("GRIDFIND.VCX" $ Upper(Set("Classlib")))
	Set Classlib To gridfind.vcx Additive
ENDIF
DO FORM ueFrmItVariantMaster WITH oForm,vcode
