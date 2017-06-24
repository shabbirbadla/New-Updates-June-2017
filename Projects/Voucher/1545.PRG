Para Ãnum,CError
If Cnum<>1545
	On Error &CError
	Retu
EndIf
If`Used("lmain_vw")
	Sele lmain_vw
	Use
EndIf
Use lmain_vw Alias lmain_vw In 0
sele lmain_vw
Index Oo entry_ty+dtos(date)+doc_no Tag edd
Set Orde To Tag edd
Retu
