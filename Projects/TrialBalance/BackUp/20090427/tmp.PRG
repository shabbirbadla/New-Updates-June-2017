Parameters mReportType
TbrFinalAc = Newobject('finaltoolbar','finaltoolbar.vcx')
TbrFinalAc.Dock(3)
Do Case
Case mReportType = 'B'
	TbrFinalAc.cboFinalac.Visible = .T.
	TbrFinalAc.cboFinalac.Clear
	TbrFinalAc.cboFinalac.AddItem('Horizontal')
	TbrFinalAc.cboFinalac.AddItem('Vertical')
	TbrFinalAc.cboFinalac.DisplayValue='Horizontal'
Case mReportType = 'P'
	TbrFinalAc.cboFinalac.Visible = .F.
Case mReportType = 'T'
	TbrFinalAc.cboFinalac.Visible = .F.
Endcase
TbrFinalAc.Show()
