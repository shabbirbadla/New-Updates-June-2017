Lparameters pUpload As String,pCompId As Integer,tlIsshedule As Logical,cThisXmlOnly As String

cThisXmlOnly = Iif(Vartype(cThisXmlOnly)<>"C","",cThisXmlOnly)

Do Case
Case Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application Are Not Execute Out-Side ...",16,[])
	Quit
	Return .F.
Case Pcount() = 0
	Messagebox('Please Pass Valid Parameters',16,VuMess)
	Return .F.
Case pUpload = "S"			&& Single Company
	Do Case
	Case Vartype(Company) <> 'O'
		Messagebox('Company Object Not Found',16,VuMess)
		Return .F.
	Case Vartype(pCompId) <> 'N' Or Empty(pCompId)								&& Company Id
		Messagebox('Please Pass Company Code',16,VuMess)
		Return .F.
	Endcase
	=CallForm(pCompId,tlIsshedule,cThisXmlOnly)
Otherwise
	Messagebox("Please Pass Valid Parameters",64,VuMess)
	Return .F.
Endcase

Function CallForm
Lparameters tnCompId As Integer,tlIsshedule As Logical,cThisXmlOnly As String
Do Form frmupload With tnCompId,tlIsshedule,cThisXmlOnly
Endfunc
