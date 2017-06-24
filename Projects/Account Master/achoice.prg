Para cAcGroup,mType,pRange
Set Classlib To apath+"class\datepicker.vcx" Additive
mGroup=Alltrim(cAcGroup)
If Empty(mGroup)
	Do Form acmast With cAcGroup,mType,pRange
Endif
If Upper(mGroup)="GROUP"
	Do Form acmast With cAcGroup,mType,pRange
Endif
If Upper(mGroup)="GL"
	Do Form frmgl With mType,pRange
Endif
