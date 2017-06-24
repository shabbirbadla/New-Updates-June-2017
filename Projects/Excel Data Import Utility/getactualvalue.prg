Lparameters tcTablename As String,tcRetValue As String,tcPrimaryKey As String,tcWherestr As String,tlInsert_New As Logical
oActive=_Screen.ActiveForm
Set DataSession To oActive.DataSessionId
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

nhandle=0
tcWherestr=Strtran(tcWherestr,Upper("this"),"lcValue")

Local lcOldalias,lcField
lcOldalias = Alias()
lnPrimaryId = .Null.
lcSqlstr = "SELECT top 1 "+Alltrim(tcRetValue)+" FROM "+Allt(tcTablename)+" WHERE "+tcWherestr
mretval = sqlconobj.dataconn("EXE",company.dbname,lcSqlstr,[FindId_vw_cur],"nhandle",oActive.DataSessionId,.F.)
If mretval <= 0
	Return .Null.
Endif

lcField="FindId_vw_cur."+Alltrim(tcRetValue)
Select FindId_vw_cur
If Reccount("FindId_vw_cur") = 0
	If tlInsert_New
		lcCursornm = tcTablename+[_vw_cur]

		lcSqlstr = "SELECT TOP 1 * FROM "+Allt(tcTablename)+" WHERE "+tcWherestr
		mretval = sqlconobj.dataconn("EXE",company.dbname,lcSqlstr,lcCursornm,"nhandle",oActive.DataSessionId,.F.)
		If mretval <= 0
			lnPrimaryId = .Null.
		Endif
		If Reccount(lcCursornm) = 0
			Select (lcCursornm)
			Append Blank
			Do Case
			Case Inlist(Type("&tcRetValue"),"C","M")	&& Charecter value
				Replace &tcRetValue  With lcValue In (lcCursornm)
			Case Type("&lcFldnm") = "N"				&& Numeric value
				Replace &tcRetValue  With Val(lcValue) In (lcCursornm)
			Endcase

			lcSqlstr = sqlconobj.Geninsert(tcTablename,"","",lcCursornm)
			mretval = sqlconobj.dataconn("EXE",company.dbname,lcSqlstr,"","nhandle",oActive.DataSessionId,.F.)
			If mretval <= 0
				lnPrimaryId = .Null.
			Endif
			If mretval > 0
				lcSqlstr = "SELECT top 1 "+Alltrim(tcRetValue)+" FROM "+Allt(tcTablename)+" WHERE "+tcWherestr
				mretval = sqlconobj.dataconn("EXE",company.dbname,lcSqlstr,[FindId_vw_cur],"nhandle",oActive.DataSessionId,.F.)
				If mretval <= 0
					Return .Null.
				Endif
			Endif
		Endif
	Else
		If Empty(&lcField) Or Isnull(&lcField)
			Local lcValue
			Do Case
			Case Inlist(Type("&lcField"),"C","M")
				lcValue=""
			Case Type("&lcField") = "N"
				lcValue=0
			Case Inlist(Type("&lcField"),"D","T")
				lcValue=.Null.
			Case Type("&lcField") = "L"
				lcValue=.F.
			Endcase
			Return lcValue
		Endif
	Endif
Endif

=sqlconobj.sqlconnclose("nHandle")

If !Empty(lcOldalias)
	If Used(lcOldalias)
		Select (lcOldalias)
	Endif
Endif
Return &lcField
