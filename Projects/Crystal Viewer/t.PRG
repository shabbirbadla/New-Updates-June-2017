Clear
If !'\OLECLASS.' $ Upper(Set('class'))
	Set Classlib To OleClass.vcx Additive
Endif
If !'\CTL32_PROGRESSBAR.' $ Upper(Set('class'))
	Set Classlib To ctl32_progressbar.vcx Additive
Endif
If !'\STANDARDUI.' $ Upper(Set('class'))
	Set Classlib To standardUI.vcx Additive
Endif
?Set("Classlib")
=RemoveClass('\OLECLASS.')
?Set("Classlib")

Function RemoveClass
Lparameters lcClslib As String
Local lcCls As String
lcCls = ""
lcLibs = Set("Classlib")
lcLibs = "<<"+Strtran(lcLibs,",",">><<")+">>"
nCount = Occurs("<<",lcLibs)
For i = 1 To nCount Step 1
	If ! lcClslib $ Strextract(lcLibs,"<<",">>",i)
		Messagebox(Strextract(lcLibs,"<<",">>",i),0,lcClslib)
		lcCls = lcCls+Iif(Empty(lcCls),"",",")+Strextract(lcLibs,"<<",">>",i)
	Endif
Endfor
lcCls = Alltrim(lcCls)
Set Classlib To &lcCls
Endfunc
