_Screen.Visible = .F.
*SET RESOURCE OFF
#Define QUERYDATA_LOC	"Searching "
#Define STARTXL_LOC		"Exporting "
#Define MISSING_LOC		"Skipping"
#Define TRENDDATA_LOC	"Saving Data"

Local i,j,TrendFunc,xlsheet,XLApp,tmpsheet,ac1,ac2
Wait Window QUERYDATA_LOC+"for Account Master data" Nowait

Local lcItDir,it1,it2
Local Array laCount(1)
lcAcDir= Curdir()

ac1 = File(lcAcDir+"ac_mast.dbf",1)
ac2 = File(lcAcDir+"Account_Master.xls",1)

Do Case
Case ac1=.F. And ac2 = .F.
	Wait Window MISSING_LOC+" export of Account Master as 'ac_mast.dbf' and 'Account_Master.xls' files not found in the following path '"+Fullpath(lcAcDir)+"'" Timeout 3

Case ac1= .F. And ac2 = .T.
	Wait Window MISSING_LOC+" export of Account Master as 'ac_mast.dbf' file not found in the following path '"+Fullpath(lcAcDir)+"'" Timeout 3

Case ac2 = .F. And ac1 = .T.
	Wait Window MISSING_LOC+" export of Account Master as 'Account_Master.xls' file not found in the following path '"+Fullpath(lcAcDir)+"'" Timeout 3

Case ac1=.T. And ac2=.T.
	Set Deleted On
	Select Count(*) From it_mast Into Array laCount

	If !Used('Ac_mast')
		Use Ac_mast
	Endif
	Select Ac_mast
	Set Deleted On

	Select Count(*) From Ac_mast Into Array ArrAcMast

	If Not(ArrAcMast(1)> 0)Then
		Messagebox("The following database 'ac_mast.dbf' does not contain any records in the following path '"+Fullpath(lcAcDir)+"'",0+64)
	Else
		Select Ac_mast
		Count For Not Deleted() To ac_count
		Wait Window STARTXL_LOC+"Account Master Data to Excel..." Nowait
		loExcel = Createobject("Excel.Application")
		loExcel.workbooks.Open(Fullpath("Account_Master.xls"))
		xlsheet = loExcel.ActiveSheet

		Select * From Ac_mast Into Array arrAccData

		For j=1 To Alen(arrAccData,2)
			For i = 1 To ac_count
				Do Case
				Case Field(j,"AC_MAST")=="AC_NAME"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j+1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CONTACT"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+1).Value = 0
					Else
						xlsheet.cells(i+1,j+1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="ADD1"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+1).Value = 0
					Else
						xlsheet.cells(i+1,j+1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="ADD2"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+1).Value = 0
					Else
						xlsheet.cells(i+1,j+1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")="ADD3"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+1).Value = 0
					Else
						xlsheet.cells(i+1,j+1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CITY"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+4).Value = 0
					Else
						xlsheet.cells(i+1,j+4).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")="ZIP"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="PHONE"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="PHONER"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="FAX"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="EMAIL"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CR_DAYS"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="I_TAX"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+6).Value = 0
					Else
						xlsheet.cells(i+1,j+6).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="GROUP"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-8).Value = 0
					Else
						xlsheet.cells(i+1,j-8).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="NOTES"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="TYPE"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="T_NO"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="T_RATE"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="T_AMOUNT"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="I_RATE"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="I_DAYS"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="RATE_LEVEL"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="POSTING"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="UPDOWN"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="TYP"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-15).Value = 0
					Else
						xlsheet.cells(i+1,j-15).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="UP_LEDBAL"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="ISGROUP"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="MANADJUST"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="DEFA_AC"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CRAMOUNT"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CRALLOW"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="SALESMAN"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+2).Value = 0
					Else
						xlsheet.cells(i+1,j+2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="TYPE1"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+13).Value = 0
					Else
						xlsheet.cells(i+1,j+13).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CEXREGNO"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+14).Value = 0
					Else
						xlsheet.cells(i+1,j+14).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="EXREGNO"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j+13).Value = 0
					Else
						xlsheet.cells(i+1,j+13).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="BANKBR"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="BANKNO"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="ST_TYPE" And j=41
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-2).Value = 0
					Else
						xlsheet.cells(i+1,j-2).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="ST_TYPE" And j=42
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="VEND_TYPE"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="USER_NAME" And j=42
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-1).Value = 0
					Else
						xlsheet.cells(i+1,j-1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="USER_NAME" And j=44
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="SYSDATE" And j=43
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-1).Value = 0
					Else
						xlsheet.cells(i+1,j-1).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="SYSDATE" And j=45
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="U_SREGN"
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="MAILNAME" And j=44
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="MAILNAME" And j=47
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="STATE" And j=45
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="STATE" And j=48
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="COUNTRY" And j=46
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="COUNTRY" And j=49
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="T_PAY" And j=47
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="T_PAY" And j=50
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="DESIGNATIO" And j=48
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="DESIGNATIO" And j=51
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="TDS_ACCOUN" And j=49
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="TDS_ACCOUN" And j=52
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CURRENCY" And j=50
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="CURRENCY" And j=53
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="MOBILE" And j=51
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrAccData(i,j)
					Endif

				Case Field(j,"AC_MAST")=="MOBILE" And j=54
					If Isnull(arrAccData(i,j)) Then
						xlsheet.cells(i+1,j-3).Value = 0
					Else
						xlsheet.cells(i+1,j-3).Value = arrAccData(i,j)
					Endif
				Endcase
			Endfor
		Endfor
		cFilePath=Fullpath(Curdir())
		cFileName = "Account_Master_"+Transform(Year(Date()))+"_"+Padl(Transform(Month(Date())),2,'0')+"_"+Padl(Transform(Day(Date())),2,'0')+"_"+Padl(Transform(Hour(Datetime())),2,'0')+"_"+Padl(Transform(Minute(Datetime())),2,'0')+"_"+Padl(Transform(Sec(Datetime())),2,'0')+".xls"
		xlsheet.SaveAs(cFilePath+cFileName)
		Wait Window TRENDDATA_LOC+" of Account Master" Timeout 2
		loExcel.workbooks.Close
		xlsheet = Null
		loExcel = Null
	Endif
Endcase


&&ITEM MASTER
Local lcItDir,it1,it2
Local Array laCount(1)
lcItDir= Curdir()
Wait Window QUERYDATA_LOC+"for Item Master data..." Nowait
it1 = File(lcItDir+"it_mast.dbf",1)
it2 = File(lcItDir+"Item_Master.xls",1)

Do Case
Case it1= .F. And it2 = .F.
	Wait Window MISSING_LOC+" export of Item Master as 'it_mast.dbf' and 'Item_Master.xls' files not found in the following path '"+Fullpath(lcItDir)+"'" Timeout 3

Case it2 = .F. And it1 = .T.
	Wait Window MISSING_LOC+" export of Item Master as 'Item_Master.xls' file not found in the following path '"+Fullpath(lcItDir)+"'" Timeout 3

Case it1 = .F. And it2=.T.
	Wait Window MISSING_LOC+" export of Item Master as 'it_mast.dbf' file not found in the following path '"+Fullpath(lcItDir)+"'" Timeout 3

Case it1=.T. And it2=.T.
	Set Deleted On
	Select Count(*) From it_mast Into Array laCount
	If Not(laCount(1)> 0)Then
		Wait Window MISSING_LOC+" export of Item Master as 'it_mast.dbf' does not contain any records in the following path '"+Fullpath(lcItDir)+"'" Timeout 3
	Else
		Select it_mast
		Count For Not Deleted() To mcount
		Wait Window STARTXL_LOC+"Item Master Data to Excel..." Nowait
		loExcel2 = Createobject("Excel.Application")
		loExcel2.workbooks.Open(Fullpath("Item_Master.xls"))
		xlsheet = loExcel2.ActiveSheet

		Select * From it_mast Into Array arrItData

		For j = 1 To Alen(arrItData,2)
			For i = 1 To mcount
				Do Case
				Case Field(j,"IT_MAST")=="IT_NAME"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IT_DESC"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="S_UNIT"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="P_UNIT"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="SAC_NM"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="PAC_NM"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="RATIO"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="RATE"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="REORDER"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="CURR_COST"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="TYPE"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="REMARK"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="GROUP"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="RATEPER"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="RATEUNIT"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="TOT_SOLD"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="ITEMBAL"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="ISGROUP"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="CHAPNO"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="AVERAGE"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IDMARK"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="MRPRATE" And j = 22
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="CHAP" And j=19
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+10).Value = 0
					Else
						xlsheet.cells(i+1,j+10).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IMAGEFILE" And j=23
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IMAGEFILE" And j=20
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="USER_NAME" And j = 21
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="USER_NAME" And j = 24
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="SYSDATE" And j=22
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+3).Value = 0
					Else
						xlsheet.cells(i+1,j+3).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="SYSDATE" And j=25
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="ABTPER"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IT_ALIAS" And j=23
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+4).Value = 0
					Else
						xlsheet.cells(i+1,j+4).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IT_ALIAS" And j = 27
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="EIT_NAME" And j=24
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+4).Value = 0
					Else
						xlsheet.cells(i+1,j+4).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="EIT_NAME" And j=28
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j).Value = 0
					Else
						xlsheet.cells(i+1,j).Value = arrItData(i,j)
					Endif

				Case Field(j,"IT_MAST")=="IS_SCRAP"
					If Isnull(arrItData(i,j)) Then
						xlsheet.cells(i+1,j+24).Value = 0
					Else
						xlsheet.cells(i+1,j+24).Value = arrItData(i,j)
					Endif
				Endcase
			Endfor
		Endfor
		cFilePath=Fullpath(Curdir())
		cFileName = "Item_Master_"+Transform(Year(Date()))+"_"+Padl(Transform(Month(Date())),2,'0')+"_"+Padl(Transform(Day(Date())),2,'0')+"_"+Padl(Transform(Hour(Datetime())),2,'0')+"_"+Padl(Transform(Minute(Datetime())),2,'0')+"_"+Padl(Transform(Sec(Datetime())),2,'0')+".xls"
		xlsheet.SaveAs(cFilePath+cFileName)
		Wait Window TRENDDATA_LOC+" of Item Master" Timeout 2
		loExcel2.workbooks.Close
		xlsheet = Null
		loExcel2 = Null
	Endif
Endcase
&&ITEM MASTER

&&Shipto Master
lcShiptoDir= Curdir()

Wait Window QUERYDATA_LOC+"for Shipto Master data..." Nowait
st1 = File(lcShiptoDir+"Shipto.dbf",1)
st2 = File(lcShiptoDir+"Shipto_Master.xls",1)

Do Case
Case st1=.F. And st2 = .F.
	Wait Window MISSING_LOC+" export of Shipto Master as 'shipto_mast.dbf' and 'Shipto_Master.xls' files not found in the following path '"+Fullpath(lcShiptoDir)+"'" Timeout 3

Case st1= .F. And st2 = .T.
	Wait Window MISSING_LOC+" export of Shipto Master as 'shipto_mast.dbf' file not found in the following path '"+Fullpath(lcShiptoDir)+"'" Timeout 3

Case st2 = .F. And st1 = .T.
	Wait Window MISSING_LOC+" export of Shipto Master as 'Shipto_Master.xls' file not found in the following path '"+Fullpath(lcShiptoDir)+"'" Timeout 3

Case st1=.T. And st2=.T.

	If !Used('SHipto')
		Use SHIPTO
	Endif
	Select SHIPTO
	Set Deleted On

	Select Count(*) From SHIPTO Into Array ArrShipto

	If (ArrShipto(1)> 0)Then

&&ruchit on 08/05/2017
*!*		Select Iif(Isnull(b.jdenum)=.T.,0,b.jdenum) As 'JDENUM',a.ac_name As 'Ac_name',Iif(Isnull(b.shiptocode)=.T.,'',b.shiptocode) As 'shiptocode',Iif(Isnull(b.ac_name)=.T.,a.add1,b.add1) As 'Add1',Iif(Isnull(b.ac_name)=.T.,a.add2,b.add2) As 'Add2',Iif(Isnull(b.ac_name)=.T.,a.add3,b.add3) As 'Add3',Iif(Isnull(b.add4)=.T.,'',b.add4) As 'Add4',Iif(Isnull(b.ac_name)=.T.,a.city,b.city) As 'City',Iif(Isnull(b.ac_name)=.T.,a.state,b.state) As 'State', Iif(Isnull(b.ac_name)=.T.,a.zip,b.zip) As 'Zip',Iif(Isnull(b.ac_name)=.T.,'',b.concode) As 'concode',Iif(Isnull(b.telephone1)=.T.,'',b.telephone1) As 'telephone1',Iif(Isnull(b.ac_name)=.T.,a.fax,b.fax) As 'Fax', Iif(Isnull(b.ac_name)=.T.,a.mailname,b.mailname) As 'MailingName',Iif(Isnull(b.ac_name)=.T.,a.email,b.email) As 'EmailId',Iif(Isnull(b.ac_name)=.T.,a.contact,b.contact) As 'Contact',Iif(Isnull(b.Design)=.T.,'',b.Design) As 'design', Iif(Isnull(b.ac_name)=.T.,a.country,b.country) As 'Country',Iif(Isnull(b.cstno)=.T.,'',b.cstno) As 'cstno',Iif(Isnull(b.u_sm)=.T.,'',b.u_sm) As 'u_sm',Iif(Isnull(b.u_shipto)=.T.,'',b.u_shipto) As 'u_shipto' From ac_mast a Left Outer Join SHIPTO b On (a.ac_name=b.ac_name) Into Cursor ArrShipto2

		SELECT * FROM ac_mast WHERE defa_ac = .F. INTO CURSOR acm READWRITE
		SELECT *,IIF((ALLTRIM(a.ac_name)=ALLTRIM(acm.ac_name) AND ALLTRIM(a.city) = ALLTRIM(acm.city))=.T.,'Y','') as 'INSCHECK' FROM shipto a INTO CURSOR SHIPT READWRITE
		SELECT acm
		Scan
			SELECT *,'Y' as 'INSCHECK' FROM shipto a WHERE a.ac_name = acm.ac_name AND a.city=acm.city INTO CURSOR shipt1
				IF (_Tally = 0)
					Select 0 As 'JDENUM',a.ac_name As 'Ac_name','' As 'shiptocode',a.add1 As 'Add1',a.add2 As 'Add2',a.add3 As 'Add3','' As 'Add4',a.city As 'City',a.state As 'State', a.zip As 'Zip','' As 'concode',a.phone As 'telephone1',a.fax As 'Fax', a.mailname As 'MailingName',a.email As 'EmailId',a.contact As 'Contact',a.designatio As 'design',a.country As 'Country',a.c_tax As 'cstno','' As 'u_sm','' As 'u_shipto','Y' as 'inscheck' ;
					From ac_mast a  WHERE a.ac_name=acm.ac_name Into Cursor SHIPARR	readwrite
					SELECT SHIPT
					APPEND from DBF('SHIPARR')
				ENDIF

			Select acm
		Endscan

&&ruchit on 08/05/2017

		WAIT WINDOW STARTXL_LOC+"Shipto Master Data to Excel..." NOWAIT
		loExcel3 = Createobject("Excel.Application")
		loExcel3.workbooks.Open(Fullpath("Shipto_Master.xls"))

		XLSheet = loExcel3.ActiveSheet

		SELECT * FROM SHIPT INTO ARRAY arrShiptoData
		SELECT SHIPT

		count for not deleted() to scount

		For j=1 To Alen(arrShiptoData,2)
			For i = 1 To scount

				Do Case

*!*					CASE FIELD(j,"SHIPT")=="JDENUM"
*!*						IF ISNULL(arrShiptoData(i,j)) THEN
*!*							XLSheet.cells(i+1,j).value = 0
*!*						ELSE
*!*							XLSheet.cells(i+1,j).value = arrShiptoData(i,j)
*!*						ENDIF

				Case Field(j,"SHIPT")=="JDENUM"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="AC_NAME"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="SHIPTOCODE"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="ADD1"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="ADD2"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="ADD3"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="ADD4"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="CITY"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="STATE"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="ZIP"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="CONCODE"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="TELEPHONE1"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="FAX"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="MAILNAME"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="EMAIL"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="CONTACT"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="DESIGN"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="COUNTRY"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="CSTNO"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j-1).Value = 0
					Else
						XLSheet.cells(i+1,j-1).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="U_SM"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j-1).Value = 0
					Else
						XLSheet.cells(i+1,j-1).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="U_SHIPTO"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j-1).Value = 0
					Else
						XLSheet.cells(i+1,j-1).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"SHIPT")=="INSCHECK"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j+31).Value = 0
					Else
						XLSheet.cells(i+1,j+31).Value = arrShiptoData(i,j)
					Endif

				Endcase
			Endfor
		Endfor
		cFilePath=FULLPATH(CURDIR())
		cFileName = "Shipto_Master_"+TRANSFORM(YEAR(DATE()))+"_"+PADL(TRANSFORM(MONTH(DATE())),2,'0')+"_"+PADL(TRANSFORM(DAY(DATE())),2,'0')+"_"+PADL(TRANSFORM(HOUR(DATETIME())),2,'0')+"_"+PADL(TRANSFORM(MINUTE(DATETIME())),2,'0')+"_"+PADL(TRANSFORM(Sec(DATETIME())),2,'0')+".xls"
		xlsheet.SaveAs(cFilePath+cFileName)
		WAIT WINDOW TRENDDATA_LOC+" of Shipto Master" TIMEOUT 2
		loExcel3.Workbooks.close
		xlsheet=null
		loExcel3 =null
	Else

		WAIT WINDOW STARTXL_LOC+"Shipto Master Data to Excel..." NOWAIT
		loExcel3 = Createobject("Excel.Application")
		loExcel3.workbooks.Open(Fullpath("Shipto_Master.xls"))
		XLSheet = loExcel3.ActiveSheet

		Select Iif(Isnull(b.jdenum)=.T.,0,b.jdenum) As 'JDENUM',a.ac_name As 'Ac_name',Iif(Isnull(b.shiptocode)=.T.,'',b.shiptocode) As 'shiptocode',Iif(Isnull(b.ac_name)=.T.,a.add1,b.add1) As 'Add1',Iif(Isnull(b.ac_name)=.T.,a.add2,b.add2) As 'Add2',Iif(Isnull(b.ac_name)=.T.,a.add3,b.add3) As 'Add3',Iif(Isnull(b.add4)=.T.,'',b.add4) As 'Add4',Iif(Isnull(b.ac_name)=.T.,a.city,b.city) As 'City',Iif(Isnull(b.ac_name)=.T.,a.state,b.state) As 'State', Iif(Isnull(b.ac_name)=.T.,a.zip,b.zip) As 'Zip',Iif(Isnull(b.ac_name)=.T.,'',b.concode) As 'concode',Iif(Isnull(b.telephone1)=.T.,'',b.telephone1) As 'telephone1',Iif(Isnull(b.ac_name)=.T.,a.fax,b.fax) As 'Fax', Iif(Isnull(b.ac_name)=.T.,a.mailname,b.mailname) As 'MailingName',Iif(Isnull(b.ac_name)=.T.,a.email,b.email) As 'EmailId',Iif(Isnull(b.ac_name)=.T.,a.contact,b.contact) As 'Contact',Iif(Isnull(b.Design)=.T.,'',b.Design) As 'design', Iif(Isnull(b.ac_name)=.T.,a.country,b.country) As 'Country',Iif(Isnull(b.cstno)=.T.,'',b.cstno) As 'cstno',Iif(Isnull(b.u_sm)=.T.,'',b.u_sm) As 'u_sm',Iif(Isnull(b.u_shipto)=.T.,'',b.u_shipto) As 'u_shipto' From ac_mast a Left Outer Join SHIPTO b On (a.ac_name=b.ac_name) Into Cursor ArrShipto2

		SELECT COUNT(*) FROM ArrShipto2 INTO ARRAY ArrShipto1
		count for not deleted() to scount

		SELECT * FROM ArrShipto2 INTO ARRAY arrShiptoData

		For j=1 To Alen(arrShiptoData,2)
			For i = 1 To scount
				Do Case

				Case Field(j,"ArrShipto2")=="JDENUM"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="AC_NAME"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="SHIPTOCODE"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="ADD1"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="ADD2"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="ADD3"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="ADD4"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="CITY"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="STATE"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="ZIP"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="CONCODE"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="TELEPHONE1"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="FAX"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="MAILNAME"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="EMAIL"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="CONTACT"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="DESIGN"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="COUNTRY"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j).Value = 0
					Else
						XLSheet.cells(i+1,j).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="CSTNO"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j-1).Value = 0
					Else
						XLSheet.cells(i+1,j-1).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="U_SM"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j-1).Value = 0
					Else
						XLSheet.cells(i+1,j-1).Value = arrShiptoData(i,j)
					Endif

				Case Field(j,"ArrShipto2")=="U_SHIPTO"
					If Isnull(arrShiptoData(i,j)) Then
						XLSheet.cells(i+1,j-1).Value = 0
					Else
						XLSheet.cells(i+1,j-1).Value = arrShiptoData(i,j)
					Endif
				Endcase
			Endfor
		Endfor
		cFilePath=FULLPATH(CURDIR())
		cFileName = "Shipto_Master_"+TRANSFORM(YEAR(DATE()))+"_"+PADL(TRANSFORM(MONTH(DATE())),2,'0')+"_"+PADL(TRANSFORM(DAY(DATE())),2,'0')+"_"+PADL(TRANSFORM(HOUR(DATETIME())),2,'0')+"_"+PADL(TRANSFORM(MINUTE(DATETIME())),2,'0')+"_"+PADL(TRANSFORM(Sec(DATETIME())),2,'0')+".xls"
		xlsheet.SaveAs(cFilePath+cFileName)
		WAIT WINDOW TRENDDATA_LOC+" of Shipto Master" TIMEOUT 2
		loExcel3.Workbooks.close
		xlsheet=null
		loExcel3=null
	Endif
Endcase
&&Shipto Master




