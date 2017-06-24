Update _TOrdZoom Set Undfld = "";

Update _TOrdZoom Set Undfld = ETI;
	Where RFETI = ETI

*!*	UPDATE NLevel Group Value [Start]
For I = 1 To 50 Step 1
	Select a.RFETI,a.ETI,a.Undfld;
		FROM _TOrdZoom a;
		WHERE a.LevelCode = I;
		INTO Cursor CurTopLevel
	If _Tally <> 0
		Select CurTopLevel
		Scan
			=FindUnderGroup(CurTopLevel.ETI,CurTopLevel.Undfld)
			Select CurTopLevel
		Endscan
	Else
		Exit
	Endif
Endfor
*!*	UPDATE NLevel Group Value [End]

Select _TOrdZoom
Browse Normal

Function FindUnderGroup
***********************
Parameters lcETI,lcUndfld
Update _TOrdZoom Set Undfld = Alltrim(lcUndfld)+Iif(!Empty(lcUndfld),":","")+Alltrim(ETI);
	Where RFETI == lcETI And RFETI <> ETI
