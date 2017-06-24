Parameters oForm,vcode

If !("GRIDFIND.VCX" $ Upper(Set("Classlib")))
	Set Classlib To gridfind.vcx Additive
Endif
If !'\VOUCLASS.' $ Upper(Set('Classlib'))
	Set Classlib To VOUCLASS AddIt
Endif
If !'\DATEPICKER.' $ Upper(Set('Classlib'))
	Set Classlib To DATEPICKER AddIt
Endif
Do Case
Case vcode="WT"
	lcFrmCaption="Wastage Scrap Details"
	&& gridCaption="Wastage Scrap Item" && Commented by Suraj Kumawat for GST on Date 08-05-2017 
	gridCaption="Wastage Scrap Goods" && Added by Suraj Kumawat for GST on Date 08-05-2017 
	lcTable="it_WastescrapDet"
	lcview="it_WastescrapDet_vw"
Case vcode="SC"
	lcFrmCaption="Process Loss Details"
	&& gridCaption="Process Loss Item" && Commented by Suraj Kumawat for GST Date on 08-05-2017 
	gridCaption="Process Loss Goods" && Added by Suraj Kumawat for GST Date on 08-05-2017 
	lcTable="It_ScrapDet"
	lcview="It_ScrapDet_vw"
Case vcode="BY"
	lcFrmCaption="By Product Details"
	&& gridCaption="By Product Item" && Commented by Suraj Kumawat for GST Date on 08-05-2017 
	gridCaption="By Product Goods" && Added by Suraj Kumawat for GST Date on 08-05-2017 
	lcTable="It_ByProdDet"
	lcview="It_ByProdDet_vw"
OTHERWISE
	
Endcase
Do Form uefrmitbreakup.scx With oForm,vcode,gridCaption,lcFrmCaption,lcTable,lcview
