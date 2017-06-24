Local co_name_new,CO_DATE,CO_ST_DATE,CO_EN_DATE,DIR_VARIABLE,d_varr &&DIR_VARIABLE FOR REMOVING THE COLUMN IN DIR_NM
ndName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
d_varr = .F.
co_name_new = Thisform.Pageframe1.Page1.CO_NAME1.Value
CO_ST_DATE 	= {}
CO_EN_DATE 	= {}
CO_ST_DATE 	= Thisform.Pageframe1.Page1.container1.STA_DT1.Value
CO_EN_DATE 	= Thisform.Pageframe1.Page1.container1.END_DT1.Value
CO_DATE 	= ("("+Alltrim(Str(Year(CO_ST_DATE)))+"-"	+Alltrim(Str(Year(CO_EN_DATE)))+")")
&&
If Thisform.Pageframe1.Page1.container1.STA_DT1.Value>=Thisform.Pageframe1.Page1.container1.END_DT1.Value
	Messagebox('Date Not In A Financial Year...',64,vUmess)
	Nodefault
	Return .F.
Endif
If Thisform.ADdmode
	oName=Alltrim(Thisform.Pageframe1.Page1.CO_NAME1.Value)
	sdt = Thisform.Pageframe1.Page1.container1.STA_DT1.Value
	edt = Thisform.Pageframe1.Page1.container1.END_DT1.Value
	nsDt=Dtot(sdt)
	neDt=Dtot(edt)
	DupCheck=Thisform.sqlconobj.dataconn("EXE","vUdyog","select * from co_mast where co_name=?oName and sta_dt=?nsDt and end_dt=?neDt","Dupl","thisform.nhandle",This.DataSessionId)
	If DupCheck > 0
		Select Dupl
		If Reccount() > 0
			Messagebox("Duplicate Company...Could Not Save",16,vUmess)
			Nodefault
			Return .F.
		Endif
	Endif
Endif
**CO_DATE 	= ("("+right(CO_ST_DATE,4)+"-"+RIGHT(CO_EN_DATE,4)+")")
*If Thisform.foundvudyog
If Empty(Thisform.Pageframe1.Page1.dbname.Value)
	Messagebox("Database Name Can Not be EMPTY",64,vUmess)
	Return .F.
Endif
If Thisform.platform ="0"
	Do Case
	Case Upper('report') $ Upper(Thisform.Pageframe1.Page1.DATA_DIR1.Value)
		Messagebox("Data Directory name as (Report) not allowed !",64,vUmess)
		Retu .F.
	Case Upper(gcVUDir+'data') $ Upper(Thisform.Pageframe1.Page1.DATA_DIR1.Value)
		Wait Wind "Data Directory name as ("+gcVUDir+"data) not allowed !"
		Retu .F.
	Case Upper('class') $ Upper(Thisform.Pageframe1.Page1.DATA_DIR1.Value)
		Wait Wind "Data Directory name as (Class) not allowed !"
		Retu .F.
	Case Upper('bmp') $ Upper(Thisform.Pageframe1.Page1.DATA_DIR1.Value)
		Wait Wind "Data Directory name as (Bmp) not allowed !"
		Retu .F.
	Case Upper('help') $ Upper(Thisform.Pageframe1.Page1.DATA_DIR1.Value)
		Wait Wind "Data Directory name as (Help) not allowed !"
		Retu .F.
	Endcase
Endif
If Thisform.platform ="0"
	Ddir=Thisform.Pageframe1.Page1.DATA_DIR1.Value
	DirNm=Thisform.sqlconobj.dataconn("EXE","vUdyog","select dir_nm from co_mast where dir_nm=?ddir","dupDir","thisform.nhandle",This.DataSessionId)
	If DirNm > 0
		Select dupdir
		If Reccount() > 0
			Messagebox("Duplicate or Empty Data Directory not allowed !",16,vUmess)
			Retu .F.
		Endif
	Endif
	DirNm=Thisform.sqlconobj.dataconn("EXE","vUdyog","select bk_dir from co_mast where bk_dir=?ddir","dupBk","thisform.nhandle",This.DataSessionId)
	If DirNm > 0
		Select dupBk
		If Reccount() > 0
			Messagebox("This Directory Name already used for backup purpose !",16,vUmess)
			Retu .F.
		Endif
	Endif
Endif
If Empty(Thisform.Pageframe1.Page1.txtproduct.Value) And Thisform.ADdmode=.T.
	Messagebox("Select Atleast One Product ",0+48,vUmess)
	Retu .F.
Endif
Local Tosave
Tosave="YES"
Tosave=Thisform.CheckTheFolder()
If Tosave="NO"
	Return .F.
Endif
lcData = Alltrim(_comast.dir_nm)
Thisform.DirNm=lcData
lcdatabase  = Iif(Alltr(Right(lcData,1)) # '\',lcData+[\],lcData)+"Invoice"


**************************************Checking Udyog6 Folder************************************
Local ch_udyog6,ch_file,if_file
if_file = .F.
ch_udyog6 =''
ch_udyog6 = Iif(Alltr(Right(lcData,1)) # '\',lcData+[\],lcData)
ch_file = Alltr(ch_udyog6)+"Lmain.dbf"
If File(ch_file)
	if_file = .T.
Endif
ch_file = Alltr(ch_udyog6)+"Litem.dbf"
If File(ch_file)
	if_file = .T.
Endif
ch_file = Alltr(ch_udyog6)+"Lac_det.dbf"
If File(ch_file)
	if_file = .T.
Endif
ch_file = Alltr(ch_udyog6)+"mainall.dbf"
If File(ch_file)
	if_file = .T.
Endif

ch_file = Alltr(ch_udyog6)+"Ac_mast.dbf"
If File(ch_file)
	if_file = .T.
Endif
ch_file = Alltr(ch_udyog6)+"It_mast.dbf"
If File(ch_file)
	if_file = .T.
Endif

If if_file = .T. And This.Pageframe1.Page1.DATA_DIR1.Enabled  = .T.
	Messagebox("This Folder already Contains"+Space(1)+" Tables",64,vUmess)
	Return .F.
Endif

*************************************Checking Over
If Not Empty(_comast.Logo) Or Not Empt(Thisform.Pageframe1.Page1.cntLogo.txtLogoPath.Caption) Then
	Local bitmappath, path_value, filethere, valinco_mast
	bitmappath = Sys(5) + "\vudyog\bmp"
	path_value = Thisform.Pageframe1.Page1.cntLogo.txtLogoPath.Caption
	valinco_mast = ["] + _comast.Logo + ["]
	If Not Empt(valinco_mast) Then
		If Not [VUDYOG\BMP] $ Uppe(valinco_mast)
			filethere = "[" + bitmappath + [\] + Justfname(valinco_mast) + "]"
			If Not File(&filethere) Then
				Copy File &valinco_mast To &bitmappath
*wait wind "Bitmap To bmp folder [ 1]"
			Endif
		Endif
	Else
		If Not [VUDYOG\BMP] $ Uppe(path_value)
			filethere = "[" + bitmappath + [\] + Justfname(path_value) + "]"
			If Not File(&filethere) Then
				Copy File &path_value To &bitmappath
*wait wind "Bitmap To bmp folder [ 2]"
			Endif
		Endif
	Endif
Endif

*!*	If This.Pageframe1.Page1.DATA_DIR1.Enabled
*!*		d_varr = .T.
*!*		Local ErrMsg ,lcCurDir
*!*		lcCurDir =Sys(5)+Curdir()		&& Storing the current directory

*!*		If Direct(Allt(_comast.dir_nm)) = .F.
*!*			Thisform.Pageframe1.Page1.DATA_DIR1.SetFocus
*!*			Return .F.
*!*		Endif
*!*		mdbc = Alltr(gcVUDir)+'Data\invoice.dbc'
*!*		gcVUDir1 = apath + 'Data\*.*'
*!*		If Thisform.platform ="0"
*!*			If Dbused(mdbc)
*!*				Wait Wind "Could Not Create Company,Database Is In Use.."
*!*				Return 0
*!*			Endif
*!*		Endif
*!*		If Type("ErrMsg")="C"
*!*			Wait Wind ErrMsg
*!*			Return 0
*!*		Endif
*!*		Sele _comast
*!*	Endif
Sele _comast
Repl ware_op With .T., neg_itbal With .F., srate_op With .T., prate_op With .T.
Repl allo_op With .T.
Repl holi_day With Iif("Su" $ holi_day,holi_day,"Su,"  + holi_day)
Repl days With Iif(Empty(days),1,days),vColor With "15066597"
If 'INVENTORY' $ Uppe(Thisform.Pageframe1.Page1.txtproduct.Value) And Thisform.ADdmode = .T.
	Repl sdc_op With .T.
	Repl pdc_op With .T.
	Repl prod_op With .T.
Endif

If Thisform.FRMEDIT_MODE = .T.
	DIR_VARIABLE = _comast.dir_nm
	DIR_VARIABLE = Iif(Alltrim(Left(DIR_VARIABLE,1)) # [\] And Alltr(Substr(dir_nm,3,1))=[\],Substr(DIR_VARIABLE,3,Len(DIR_VARIABLE)),DIR_VARIABLE)
	DIR_VARIABLE = Iif(Alltr(Right(DIR_VARIABLE,1)) # [\],Alltr(DIR_VARIABLE)+'\',DIR_VARIABLE)
	Repl _comast.dir_nm With DIR_VARIABLE
Endif

If Thisform.ADdmode = .T.
	Repl sstax_op With .T.
Endif
lcNewDir = Alltrim(_comast.dir_nm)
If Thisform.ADdmode
	If Not Empty(zipfilename)
		If Thisform.platform="0" && Visual Foxpro
			Do zipunzip With "UNZIP", [], Allt(zipfilename), [], Allt(Thisform.Pageframe1.Page1.DATA_DIR1.Value), "Restoring ("+Justfname(Allt(zipfilename))+")... Writing Data Files..."
		Else && --> For Sql server Database creation
			oCName=Thisform.Pageframe1.Page1.dbname.Value
			Chqdb=Thisform.sqlconobj.dataconn("EXE","vUdyog","select name from master.dbo.sysdatabases where name=?oCName","","thisform.nhandle",This.DataSessionId)
			If Chqdb > 0
				Sele _dupdb
				If Reccount() > 0
					Messagebox("Database Already Exists..Please check",16,vUmess)
					Retu .F.
				Endif
				Local cDbcreate
				dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
				If !_comast.Import
*!*						Thisform.container1.Visible=.T.
*!*						Thisform.container1.olecontrol1.Visible =.T.
*!*						Thisform.container1.label1.Visible= .T.
*!*						Thisform.container1.label1.Caption ="Creating DataBase....Please wait"
					Thisform.shape1.Visible=.T.
					Thisform.label1.Visible= .T.
					Thisform.olecontrol1.Visible=.T.
					Thisform.label1.Caption ="Creating DataBase....Please wait"
					=Inkey(3,"H")
					Thisform.olecontrol1.Value=5
					mStr="create database"+Space(1)+dName
					MstConn=0
					cDbcreate=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","MstConn","",.T.)
					If cDbcreate < 0
						Messagebox("Error Occured While Creating Database"+Chr(13)+Message(),16,vUmess)
						Thisform.olecontrol1.Value=0
						Thisform.shape1.Visible=.F.
						Thisform.label1.Visible= .F.
						Thisform.olecontrol1.Visible=.F.
						Nodefault
						Retu .F.
					Else
						If Direct(Allt(_comast.dir_nm)) = .F. && checking for exist
							nPath=apath+Alltrim(_comast.folderName)
							Md '&nPath'
							dirfound = Direct(Allt(_comast.folderName)) && checking for exist
						Endif
						Thisform.container1.olecontrol1.Value=15
						Thisform.gendata(.T.)
						statdesktop.Message=""
						Thisform.container1.label1.Caption ="Finished Database Creation"
						Thisform.olecontrol1.Value=100
						Thisform.shape1.Visible=.F.
						Thisform.olecontrol1.Visible=.F.
						Thisform.label1.Visible= .F.
						CUser=Thisform.CreateUser()
						If !CUser
							mStr="Drop Database"+Space(1)+Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
							NewCon=0
							DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
							If DelDataBase <= 0
								Return .F.
							Else
								mRet=Thisform.sqlconobj.sqlconnclose("NewCon")
								If mRet <= 0
									Return .F.
								Endif
							Endif
							co_dir_name = Alltrim(Thisform.DirNm)
							Local rddirect
							Local filecount,filename
							filename=" "
							filecount=Adir(gcfile,co_dir_name +"\*.*")
							ERRVAR=.F.
							Local prev_err_hnd
							prev_err_hnd = On([error])
							On Error ERRVAR=.T.
							For counter=1 To filecount
								filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
								Dele File &filename
								If ERRVAR = .T.
									Exit For
								Endif
							Endfor
							On Error
							Local removedir
							removedir = Allt(co_dir_name)
							If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
								Cd..
							Endif
							Rd '&removedir'
							Local filecount
							filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
							If filecount > 0 Then
								=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
									+ "]" + " CanNot Be Removed")
							Endif
							On Error
							If Not Empt(prev_err_hnd) Then
								On Error &prev_err_hnd
							Endif
							Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
							Return .F.
						Endif

						Select _comast
						Replace sta_dt With Thisform.Pageframe1.Page1.container1.STA_DT1.Value
						Replace end_dt With Thisform.Pageframe1.Page1.container1.END_DT1.Value
						If cDbcreate > 0
							mSqlStr=Thisform.sqlconobj.genInsert("co_mast","'CompanyId'","","_comast",Thisform.platform)
							svCoMast=Thisform.sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"","thisform.nhandle",This.DataSessionId,.T.)
							If svCoMast <=0
								mRet=Thisform.sqlconobj.sqlconnclose("nHandle")
								If mRet <= 0
									Return .F.
								Endif
								mStr="Drop Database"+Space(1)+Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
								NewCon=0
								DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
								If DelDataBase <= 0
									Return .F.
								Else
									mRet=Thisform.sqlconobj.sqlconnclose("NewCon")
									If mRet <= 0
										Return .F.
									Endif
								Endif

								co_dir_name = Alltrim(Thisform.DirNm)
								Local rddirect
								Local filecount,filename
								filename=" "
								filecount=Adir(gcfile,co_dir_name +"\*.*")
								ERRVAR=.F.
								Local prev_err_hnd
								prev_err_hnd = On([error])
								On Error ERRVAR=.T.
								For counter=1 To filecount
									filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
									Dele File &filename
									If ERRVAR = .T.
										Exit For
									Endif
								Endfor
								On Error
								Local removedir
								removedir = Allt(co_dir_name)
								If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
									Cd..
								Endif
								Rd '&removedir'
								Local filecount
								filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
								If filecount > 0 Then
									=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
										+ "]" + " CanNot Be Removed")
								Endif
								On Error
								If Not Empt(prev_err_hnd) Then
									On Error &prev_err_hnd
								Endif
								Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
								Return .F.
							Endif
						Else
							Messagebox("Could Not Create Database",16,vUmess)
							rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
							If rB < 0
								Messagebox("RollBack Error!!!",16,vUmess)
								Return .F.
							Endif
						Endif
						Thisform.Show()
						=BARSTAT(.F.,.F.,.F.,.F.,.F.,.F.,.T.,.T.,.F.,.F.,.F.,.F.,.T.,.T.)
					Endif
				Else && If Data will be coming from DTS Utility
					Thisform.shape1.Visible=.T.
					Thisform.olecontrol1.Visible =.T.
					Thisform.label1.Caption ="Creating DataBase....Please wait"
					=Inkey(3,"H")
					Thisform.olecontrol1.Value=30
					mStr="create database"+Space(1)+dName
					MstConn=0
					cDbcreate=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","MstConn","",.T.)
*						cDbcreate=1
					If cDbcreate < 0
						Messagebox("Database Creation Access Denied",16,vUmess)
						Retu .F.
					Else
						If Direct(Allt(_comast.dir_nm)) = .F. && checking for exist
							Md (_comast.dir_nm)
							dirfound = Direct(Allt(_comast.dir_nm)) && checking for exist
						Endif
						Thisform.gendata(.T.)
						Select _comast
						Replace sta_dt With Thisform.Pageframe1.Page1.container1.STA_DT1.Value
						Replace end_dt With Thisform.Pageframe1.Page1.container1.END_DT1.Value
						If cDbcreate > 0
							mSqlStr=Thisform.sqlconobj.genInsert("co_mast","'CompanyId'","","_comast",Thisform.platform)
							svCoMast=Thisform.sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"","thisform.nhandle",This.DataSessionId,.T.)
						Else
							Messagebox("Could Not Create Table",16,vUmess)
							Return .F.
						Endif
						Thisform.label1.Caption ="Finished Creating Database"
						Thisform.olecontrol1.Value=100
						Thisform.shape1.Visible=.F.
						Thisform.olecontrol1.Visible=.F.
						Thisform.label1.Visible= .F.
						CUser=Thisform.CreateUser()
						If !CUser
							mStr="Drop Database"+Space(1)+Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
							NewCon=0
							DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
							If DelDataBase <= 0
								Return .F.
							Else
								mRet=Thisform.sqlconobj.sqlconnclose("NewCon")
								If mRet <= 0
									Return .F.
								Endif
							Endif
							co_dir_name = Alltrim(Thisform.DirNm)
							Local rddirect
							Local filecount,filename
							filename=" "
							filecount=Adir(gcfile,co_dir_name +"\*.*")
							ERRVAR=.F.
							Local prev_err_hnd
							prev_err_hnd = On([error])
							On Error ERRVAR=.T.
							For counter=1 To filecount
								filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
								Dele File &filename
								If ERRVAR = .T.
									Exit For
								Endif
							Endfor
							On Error
							Local removedir
							removedir = Allt(co_dir_name)
							If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
								Cd..
							Endif
							Rd '&removedir'
							Local filecount
							filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
							If filecount > 0 Then
								=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
									+ "]" + " CanNot Be Removed")
							Endif
							On Error
							If Not Empt(prev_err_hnd) Then
								On Error &prev_err_hnd
							Endif
							Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
							Return .F.
						Endif
						=BARSTAT(.F.,.F.,.F.,.F.,.F.,.F.,.T.,.T.,.F.,.F.,.F.,.F.,.T.,.T.)
					Endif
				Endif
			Endif
		Endif
	Else
		Copy File &gcVUDir1 To &lcNewDir
	Endif
Endif
If Thisform.FRMEDIT_MODE = .T.
&& Creating user table
*!*		If Thisform.NewCompany=.F.
*!*			lMuser=Thisform.sqlconobj.dataconn("EXE","vUdyog","select * from [user]","user","thisform.nhandle",This.DataSessionId)
*!*			If lMuser <= 0
*!*				Messagebox("Can't Connect to User Table",16,vUmess)
*!*				Retu .F.
*!*			Endif
*!*			Sele User
*!*			Locate For Alltrim(Upper(User))="ADMIN"
*!*			If Found()
*!*				lccompany=Alltrim(User.company)
*!*				nm1=Padl(Alltr(User.User_Roles),Len(User.company),Alltr(User.User_Roles))
*!*				INS = ' '
*!*				chk=0
*!*				For J = 1 To Len(lccompany)
*!*					N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
*!*					If N<=0
*!*						chk=1
*!*						Loop
*!*					Endif
*!*					INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
*!*				Endfor
*!*				If chk=1
*!*					Wait Wind 'Database misuse!  Recreate the user access for '+Alltr(User.User)
*!*					Return
*!*				Endif
*!*				INS=Alltrim(INS)
*!*	&&Adding this company Access to Admin
*!*				Tot_str= INS+Thisform.Pageframe1.Page1.CO_NAME1.Value +CO_DATE+ "[ADMINISTRATOR]" + Chr(13)
*!*				nm=Alltr(User.User_Roles)
*!*				out=' '
*!*				If !Empty(Tot_str)
*!*					nm1=Padl(nm,Len(Tot_str),nm)
*!*					For i = 1 To Len(Tot_str)
*!*						out = out+ Chr(Asc(Substr(Tot_str,i,1)) + Asc(Substr(nm1,i,1)))
*!*					Endfor
*!*				Endif
*!*				out=Alltr(out)
*!*				Replace User.company With out
*!*			Endif
*!*		Endif
*!*		Sele User
*!*		Locate For Alltrim(Upper(User))='MANAGER'
*!*		If Found()
*!*			lccompany=Alltrim(User.company)
*!*			nm1=Padl(Alltr(User.User_Roles),Len(User.company),Alltr(User.User_Roles))
*!*			INS = ' '
*!*			chk=0
*!*			For J = 1 To Len(lccompany)
*!*				N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
*!*				If N<=0
*!*					chk=1
*!*					Loop
*!*				Endif
*!*				INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
*!*			Endfor
*!*			If chk=1
*!*				Wait Wind 'Database misuse!  Recreate the user access for '+Alltr(User.User)
*!*				Return
*!*			Endif
*!*			INS=Alltrim(INS)
*!*			Tot_str= INS+Thisform.Pageframe1.Page1.CO_NAME1.Value +CO_DATE+ "[ADMINISTRATOR]" + Chr(13)
*!*	*			nm=Alltr(User.User)
*!*			nm=Alltr(User.User_Roles) && New
*!*			out=' '
*!*			If !Empty(Tot_str)
*!*				nm1=Padl(nm,Len(Tot_str),nm)
*!*				For i = 1 To Len(Tot_str)
*!*					out = out+ Chr(Asc(Substr(Tot_str,i,1)) + Asc(Substr(nm1,i,1)))
*!*				Endfor
*!*			Endif
*!*			out=Alltr(out)
*!*			Replace User.company With out
*!*		Endif
*!*		Sele User  && If different User is logged in
*!*		If nameuser <> "" And Alltrim(Upper(nameuser)) <> Alltrim(Upper('MANAGER'))
*!*			Sele User
*!*			If Seek(Alltrim(Upper(nameuser)))
*!*				lccompany=Alltrim(User.company)
*!*				nm1=Padl(Alltr(User.User_Roles),Len(User.company),Alltr(User.User_Roles))
*!*				INS = ' '
*!*				chk=0
*!*				For J = 1 To Len(lccompany)
*!*					N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
*!*					If N<=0
*!*						chk=1
*!*						Loop
*!*					Endif
*!*					INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
*!*				Endfor
*!*				If chk=1
*!*					Wait Wind 'Database misuse!  Recreate the user access for '+Alltr(User.User)
*!*					Return
*!*				Endif
*!*				INS=Alltrim(INS)
*!*	&&Adding this company Access to Manager
*!*				Tot_str= INS+Thisform.Pageframe1.Page1.CO_NAME1.Value + CO_DATE + "[Manager ]" + Chr(13)
*!*				nm=Alltr(User.User)
*!*				out=' '
*!*				If !Empty(Tot_str)
*!*					nm1=Padl(nm,Len(Tot_str),nm)
*!*					For i = 1 To Len(Tot_str)
*!*						out = out+ Chr(Asc(Substr(Tot_str,i,1)) + Asc(Substr(nm1,i,1)))
*!*					Endfor
*!*				Endif
*!*				out=Alltr(out)
*!*				Replace User.company With out
*!*				Sele _comast
*!*				Go Bottom
*!*			Endif
*!*		Endif
	Sele _comast
Endif
Select _comast
=Tableupdate(.T.)
If Thisform.ADdmode
	If Thisform.platform ="0"
		Thisform.ovl()
	Endif
Endif
If !_comast.Import && CHecking for Blank Database Creation
	If Thisform.platform = "0" && For Vfp
		If "EXCISE MANUFACTURING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value)) Or ;
				"EXCISE TRADING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
			Local manu_there
			manu_there = ["] + Allt(Thisform.Pageframe1.Page1.DATA_DIR1.Value) + "\"
			manu_there = manu_there + "manufact.dbf" + ["]
			If File(&manu_there) Then
				Do Form manufact With Allt(Thisform.Pageframe1.Page1.DATA_DIR1.Value),Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
				Thisform.ZOrder(0)
			Endif
		Endif
		zipfilename = ""
	Else && SQL
		If "EXCISE MANUFACTURING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value)) Or ;
				"EXCISE TRADING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
			ndName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
*!*					PUBLIC nHandle1
*!*					nHandle1="Co"+SYS(2015)
			Thisform.nhandle1=0
			mManufact=Thisform.sqlconobj.dataconn("EXE","&ndName","select * from manufact ","Manufact","thisform.nhandle1",This.DataSessionId)
			If mManufact > 0
				Do Form manufact With "manufact.dbf",Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
				Thisform.ZOrder(0)
			Else
				Messagebox("Error Opening Manufact..",16,vUmess)
				Return .F.
			Endif
			zipfilename = ""
			If !_comast.Import
				If Thisform.ADdmode
					mSqlStr=Thisform.sqlconobj.genInsert("manufact","","","manufact",Thisform.platform)
					svManufact=Thisform.sqlconobj.dataconn("EXE","&ndName",mSqlStr,"","thisform.nhandle1",This.DataSessionId,.T.)
					If svManufact < 0
						Messagebox("Can Not Update Manufact",16,vUmess)
						Return .F.
					Endif
				Endif
				If Thisform.Editmode
*!*							nHandle1="Co"+SYS(2015)
					Thisform.nhandle1=0
					mSqlStr=Thisform.sqlconobj.genUpdate("manufact","","","manufact",Thisform.platform,"")
					svManufact=Thisform.sqlconobj.dataconn("EXE","&nDName",mSqlStr,"","thisform.nhandle1",This.DataSessionId,.T.)
					If svManufact < 0
						Messagebox("Can Not Update Manufact",16,vUmess)
						Return .F.
					Endif
				Endif
			Endif
		Endif
		If !_comast.Import && CHecking for Blank Database Creation
			If "EXCISE TRADING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
&& opening Warehouse
*!*						nHandle1="Co"+SYS(2015)
				Thisform.nhandle1=0
				Ware=Thisform.sqlconobj.dataconn("EXE","&ndName","select * from warehouse where 1=2","_warehouse","thisform.nhandle1",This.DataSessionId)
				If Ware > 0
					Sele _warehouse
					Appen Blank
					Repl ware_nm With Alltrim(Thisform.Pageframe1.Page1.CO_NAME1.Value), ;
						validity With " IR P S SR SS"
					If Thisform.ADdmode
						mSqlStr=Thisform.sqlconobj.genInsert("warehouse","","","_warehouse",Thisform.platform)
						svWare=Thisform.sqlconobj.dataconn("EXE","&ndName",mSqlStr,"","thisform.nhandle1",This.DataSessionId,.T.)
						If svWare < 0
							Messagebox("Can not Insert in Warehouse Master...."+Message(),16,vUmess)
							Return .F.
						Endif
					Endif
				Else
					Messagebox("Can't Open Warehouse Due to Error:"+Message())
					Retu .F.
				Endif
			Endif
		Endif

	Endif
Endif

If Thisform.platform="0"
	Cd (_comast.dir_nm)
Endif
If Thisform.platform="0"
	Do RE_INDEX With "COMPANY"
Endif
*!*		If !_comast.Import
*!*			nRet=Thisform.commonrightsmanager()
*!*		Endif
If Thisform.platform = "0"
	Cd (lcCurDir)
	Open Data (lcdatabase)
	Set Database To (lcdatabase)
Endif
&& for edit mode
*!*	If Thisform.NewCompany=.F.
*!*		If Thisform.FRMEDIT_MODE
*!*			If Thisform.Editmode Or Thisform.ADdmode
*!*				Select User
*!*				Set Filter To Alltrim(Upper(User))=Alltrim("MANAGER") Or Alltrim(Upper(User))=Alltrim("ADMIN")
*!*				Scan
*!*					uName=Alltrim(User.User)
*!*					mSqlStr=Thisform.sqlconobj.genUpdate("[user]","","'user'","user",Thisform.platform,"[user]=?uName")
*!*					svUser=Thisform.sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"","thisform.nhandle",This.DataSessionId,.T.)
*!*				Endscan
*!*				If svUser < 0
*!*					Messagebox("Can not Update User Table...."+Message(),16,vUmess)
*!*					Retu .F.
*!*				Endif
*!*			Endif
*!*		Endif
*!*	Endif
&&ends here
Sele _comast
&& for edit mode
If Thisform.Editmode
	cName=Alltrim(Thisform.Pageframe1.Page1.CO_NAME1.Value)
	tCon=0
	mCond = "co_name=?cName and sta_dt=?_comast.sta_Dt and end_dt=?_comast.end_dt"
	mSqlStr=Thisform.sqlconobj.genUpdate("co_mast","","","_comast",Thisform.platform,mCond)
	svCoMast=Thisform.sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"","tCon",This.DataSessionId,.T.)
	If svCoMast <=0
		Return .F.
	Else
		If Type('thisform.nhandle1')<>"U"
			sv=Thisform.sqlconobj._Sqlcommit("thisform.nhandle1")
			If sv <= 0
				Return .F.
			Else
				mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle1")
				If mRet <= 0
					Return .F.
				Endif
			Endif
		Endif
		sv=Thisform.sqlconobj._Sqlcommit("tCon")
		If sv <= 0
			Return .F.
		Else
			mRet=Thisform.sqlconobj.sqlconnclose(tCon)
			If mRet <= 0
				Return .F.
			Endif
		Endif
		Messagebox("Company Updated Successfully",64,vUmess,1)
	Endif
Endif
If Thisform.ADdmode
	If !_comast.Import
		If "EXCISE MANUFACTURING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
			If svCoMast < 0 Or cDbcreate < 0 Or svManufact < 0 &&Or IIF(thisform.newcompany=.f.,svUser,1) < 0 ;
					or !CUser
				&& Dropping Database if not suncessfully Executed Above tranasction
				dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
				If Type('thisform.nhandle1')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle1")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle1")
					If mRet <= 0
						Return .F.
					Endif
				Endif
				If Type('nHandle')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("nhandle")
					If mRet <= 0
						Return .F.
					Endif
				Endif

				If Type('thisform.nhandle')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle")
					If mRet <= 0
						Return .F.
					Endif
				Endif
				If Type('&MstConn')<>"U"
					mRet=Thisform.sqlconobj.sqlconnclose("MstConn")
					If mRet <= 0
						Return .F.
					Endif
				Endif

				mStr="Drop Database"+Space(1)+dName
				NewCon=0
				DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
				If DelDataBase <= 0
					Return .F.
				Endif
				co_dir_name = Alltrim(Thisform.DirNm)
				Local rddirect
				Local filecount,filename
				filename=" "
				filecount=Adir(gcfile,co_dir_name +"\*.*")
				ERRVAR=.F.
				Local prev_err_hnd
				prev_err_hnd = On([error])
				On Error ERRVAR=.T.
				For counter=1 To filecount
					filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
					Dele File &filename
					If ERRVAR = .T.
						Exit For
					Endif
				Endfor
				On Error
				Local removedir
				removedir = Allt(co_dir_name)
				If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
					Cd..
				Endif
				Rd '&removedir'
				Local filecount
				filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
				If filecount > 0 Then
					=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
						+ "]" + " CanNot Be Removed")
				Endif
				On Error
				If Not Empt(prev_err_hnd) Then
					On Error &prev_err_hnd
				Endif
				Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
				Return .F.
			Else
				If Type('thisform.nhandle')<>"U"
					lSave=Thisform.sqlconobj._Sqlcommit("thisform.nhandle")
					If lSave > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Type('thisform.nhandle1')<>"U"
					lSave1=Thisform.sqlconobj._Sqlcommit("thisform.nhandle1")
					If lSave1 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle1")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Type('thisform.nhandle3')<>"U"
					lSave3=Thisform.sqlconobj._Sqlcommit("thisform.nhandle3")
					If lSave3 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle3")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Thisform.ADdmode
					If lSave <= 0 Or lSave1 <= 0 Or lSave3 <=0
						Messagebox("Error Saving",16,vUmess)
					Else
						Messagebox("Company has been Created Successfully",64,vUmess)
					Endif
				Else
					If lSave <= 0 Or lSave1 <= 0 Or lSave3 <= 0
						Messagebox("Error Saving",16,vUmess)
					Else
						Messagebox("Company has been Updated Successfully",64,vUmess)
					Endif
				Endif
			Endif
		ENDIF
		SET STEP ON 
		If "EXCISE TRADING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
			If svCoMast < 0 Or cDbcreate < 0 Or svWare < 0 OR !CUser OR svManufact < 0
				&& Dropping Database if not suncessfully Executed Above tranasction
				dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
				If Type('thisform.nhandle1')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle1")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle1")
					If mRet <= 0
						Return .F.
					Endif
				Endif
				If Type('thisform.nHandle3')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("nhandle")
					If mRet <= 0
						Return .F.
					Endif
				Endif

				If Type('thisform.nhandle')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle")
					If mRet <= 0
						Return .F.
					Endif
				Endif
				If Type('&MstConn')<>"U"
					mRet=Thisform.sqlconobj.sqlconnclose("MstConn")
					If mRet <= 0
						Return .F.
					Endif
				Endif

				mStr="Drop Database"+Space(1)+dName
				NewCon=0
				DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
				If DelDataBase <= 0
					Return .F.
				Endif
				co_dir_name = Alltrim(Thisform.DirNm)
				Local rddirect
				Local filecount,filename
				filename=" "
				filecount=Adir(gcfile,co_dir_name +"\*.*")
				ERRVAR=.F.
				Local prev_err_hnd
				prev_err_hnd = On([error])
				On Error ERRVAR=.T.
				For counter=1 To filecount
					filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
					Dele File &filename
					If ERRVAR = .T.
						Exit For
					Endif
				Endfor
				On Error
				Local removedir
				removedir = Allt(co_dir_name)
				If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
					Cd..
				Endif
				Rd '&removedir'
				Local filecount
				filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
				If filecount > 0 Then
					=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
						+ "]" + " CanNot Be Removed")
				Endif
				On Error
				If Not Empt(prev_err_hnd) Then
					On Error &prev_err_hnd
				Endif
				Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
				Return .F.
			Else
				If Type('thisform.nhandle')<>"U"
					lSave=Thisform.sqlconobj._Sqlcommit("thisform.nhandle")
					If lSave > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Type('thisform.nhandle1')<>"U"
					lSave1=Thisform.sqlconobj._Sqlcommit("thisform.nhandle1")
					If lSave1 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle1")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Type('thisform.nhandle3')<>"U"
					lSave3=Thisform.sqlconobj._Sqlcommit("thisform.nhandle3")
					If lSave3 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle3")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Thisform.ADdmode
					If lSave <= 0 Or lSave1 <= 0 Or lSave3 <=0
						Messagebox("Error Saving",16,vUmess)
					Else
						Messagebox("Company has been Created Successfully",64,vUmess)
					Endif
				Else
					If lSave <= 0 Or lSave1 <= 0 Or lSave3 <= 0
						Messagebox("Error Saving",16,vUmess)
					Else
						Messagebox("Company has been Updated Successfully",64,vUmess)
					Endif
				Endif
			Endif
		Endif
		&&& If product is neither MANUFACTURING NOR TRADING
		If !"EXCISE MANUFACTURING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value)) And !"EXCISE TRADING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value))
			If svCoMast < 0 Or cDbcreate < 0 or !CUser
				&& Dropping Database if not suncessfully Executed Above tranasction
				dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
				If Type('thisform.nhandle1')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle1")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle1")
					If mRet <= 0
						Return .F.
					Endif
				Endif
				If Type('nHandle')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("nhandle")
					If mRet <= 0
						Return .F.
					Endif
				Endif

				If Type('thisform.nhandle')<>"U"
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle")
					If mRet <= 0
						Return .F.
					Endif
				Endif
				If Type('&MstConn')<>"U"
					mRet=Thisform.sqlconobj.sqlconnclose("MstConn")
					If mRet <= 0
						Return .F.
					Endif
				Endif

				mStr="Drop Database"+Space(1)+dName
				NewCon=0
				DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
				If DelDataBase <= 0
					Return .F.
				Endif
				co_dir_name = Alltrim(Thisform.DirNm)
				Local rddirect
				Local filecount,filename
				filename=" "
				filecount=Adir(gcfile,co_dir_name +"\*.*")
				ERRVAR=.F.
				Local prev_err_hnd
				prev_err_hnd = On([error])
				On Error ERRVAR=.T.
				For counter=1 To filecount
					filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
					Dele File &filename
					If ERRVAR = .T.
						Exit For
					Endif
				Endfor
				On Error
				Local removedir
				removedir = Allt(co_dir_name)
				If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
					Cd..
				Endif
				Rd '&removedir'
				Local filecount
				filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
				If filecount > 0 Then
					=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
						+ "]" + " CanNot Be Removed")
				Endif
				On Error
				If Not Empt(prev_err_hnd) Then
					On Error &prev_err_hnd
				Endif
				Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
				Return .F.
			ELSE && commited successfully
				If Type('thisform.nhandle')<>"U"
					lSave=Thisform.sqlconobj._Sqlcommit("thisform.nhandle")
					If lSave > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Type('thisform.nhandle1')<>"U"
					lSave1=Thisform.sqlconobj._Sqlcommit("thisform.nhandle1")
					If lSave1 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle1")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Type('thisform.nhandle3')<>"U"
					lSave3=Thisform.sqlconobj._Sqlcommit("thisform.nhandle3")
					If lSave3 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle3")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
				If Thisform.ADdmode
					If lSave <= 0 Or lSave1 <= 0 Or lSave3 <=0
						Messagebox("Error Saving",16,vUmess)
					Else
						Messagebox("Company has been Created Successfully",64,vUmess)
					Endif
				Else
					If lSave <= 0 Or lSave1 <= 0 Or lSave3 <= 0
						Messagebox("Error Saving",16,vUmess)
					Else
						Messagebox("Company has been Updated Successfully",64,vUmess)
					Endif
				Endif
			Endif
		Endif
	Else &&& For Blank Database Creation
		If "EXCISE MANUFACTURING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value)) 
			If Thisform.NewCompany=.F.
				If svCoMast < 0 Or cDbcreate < 0 Or !CUser OR sVmanufact < 0&&svUser < 0 Or
					&& Dropping Database if not suncessfully Executed Above tranasction
					dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
					If Type('thisform.nhandle')<>"U"
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle")
						If mRet <= 0
							Return .F.
						Endif
					Endif
					If Type('nhandle')<>"U"
						rB=Thisform.sqlconobj._Sqlrollback("nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						mRet=Thisform.sqlconobj.sqlconnclose("nhandle")
						If mRet <= 0
							Return .F.
						Endif
					Endif
					If Type('MstConn')<>"U"
						mRet=Thisform.sqlconobj.sqlconnclose("MstConn")
						If mRet <= 0
							Return .F.
						Endif
					Endif

					mStr="Drop Database"+Space(1)+dName
					NewCon=0
					DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
					If DelDataBase <= 0
						Return .F.
					Endif
					co_dir_name = Alltrim(Thisform.DirNm)
					Local rddirect
					Local filecount,filename
					filename=" "
					filecount=Adir(gcfile,co_dir_name +"\*.*")
					ERRVAR=.F.
					Local prev_err_hnd
					prev_err_hnd = On([error])
					On Error ERRVAR=.T.
					For counter=1 To filecount
						filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
						Dele File &filename
						If ERRVAR = .T.
							Exit For
						Endif
					Endfor
					On Error
					Local removedir
					removedir = Allt(co_dir_name)
					If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
						Cd..
					Endif
					Rd '&removedir'
					Local filecount
					filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
					If filecount > 0 Then
						=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
							+ "]" + " CanNot Be Removed")
					Endif
					On Error
					If Not Empt(prev_err_hnd) Then
						On Error &prev_err_hnd
					Endif

					Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
					Return .F.
				Else
					lSave=Thisform.sqlconobj._Sqlcommit("thisform.nhandle")
					If lSave > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif

					lSave1=Thisform.sqlconobj._Sqlcommit("nhandle")
					If lSave1 > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
					If Thisform.ADdmode
						If lSave <= 0 Or lSave1 <= 0
							Messagebox("Error Saving",16,vUmess)
						Else
							Messagebox("Company has been Created Successfully",64,vUmess)
						Endif
					Else
						If lSave <= 0 Or lSave1 <= 0 Or lSave2 <= 0
							Messagebox("Error Saving",16,vUmess)
						Else
							Messagebox("Company has been Updated Successfully",64,vUmess)
						Endif
					Endif

				Endif
			Else  && Tushar && if blank co_mast
				If svCoMast < 0 Or cDbcreate < 0 Or !CUser
					&& Dropping Database if not suncessfully Executed Above tranasction
					dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
					If Type('thisform.nhandle')<>"U"
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle")
						If mRet <= 0
							Return .F.
						Endif
					Endif

					If Type('nhandle')<>"U"
						rB=Thisform.sqlconobj._Sqlrollback("nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						mRet=Thisform.sqlconobj.sqlconnclose("nhandle")
						If mRet <= 0
							Return .F.
						Endif
					Endif
					If Type('&MstConn')<>"U"
						mRet=Thisform.sqlconobj.sqlconnclose("MstConn")
						If mRet <= 0
							Return .F.
						Endif
					Endif

					mStr="Drop Database"+Space(1)+dName
					NewCon=0
					DelDataBase=Thisform.sqlconobj.dataconn("EXE","Master",mStr,"","NewCon","",.F.)
					If DelDataBase <= 0
						Return .F.
					Endif
					co_dir_name = Alltrim(Thisform.DirNm)
					Local rddirect
					Local filecount,filename
					filename=" "
					filecount=Adir(gcfile,co_dir_name +"\*.*")
					ERRVAR=.F.
					Local prev_err_hnd
					prev_err_hnd = On([error])
					On Error ERRVAR=.T.
					For counter=1 To filecount
						filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
						Dele File &filename
						If ERRVAR = .T.
							Exit For
						Endif
					Endfor
					On Error
					Local removedir
					removedir = Allt(co_dir_name)
					If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
						Cd..
					Endif
					Rd '&removedir'
					Local filecount
					filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
					If filecount > 0 Then
						=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
							+ "]" + " CanNot Be Removed")
					Endif
					On Error
					If Not Empt(prev_err_hnd) Then
						On Error &prev_err_hnd
					Endif

					Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
					Return .F.
				Else
					lSave=Thisform.sqlconobj._Sqlcommit("thisform.nhandle")
					If lSave > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif

					lSave=Thisform.sqlconobj._Sqlcommit("nhandle")
					If lSave > 0
						If Thisform.ADdmode
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Else
							Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
						Endif
					Else
						rB=Thisform.sqlconobj._Sqlrollback("nhandle")
						If rB < 0
							Messagebox("RollBack Error!!!",16,vUmess)
							Return .F.
						Endif
						Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
						Return .F.
					Endif
				Endif
			Endif
		Endif
		If "EXCISE TRADING" $ Allt(Upper(Thisform.Pageframe1.Page1.txtproduct.Value)) 
			If svCoMast < 0 Or cDbcreate < 0
				dName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
				mStr="Drop Database"+Space(1)+dName
				DelDataBase=Thisform.sqlconobj.dataconn("EXE","&dName",mStr,"","&MstConn","",.T.)
				If DelDataBase <= 0
					Return .F.
				Endif

				rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
				If rB < 0
					Messagebox("RollBack Error!!!",16,vUmess)
					Return .F.
				Endif
				co_dir_name = Alltrim(Thisform.DirNm)
				Local rddirect
				Local filecount,filename
				filename=" "
				filecount=Adir(gcfile,co_dir_name +"\*.*")
				ERRVAR=.F.
				Local prev_err_hnd
				prev_err_hnd = On([error])
				On Error ERRVAR=.T.
				For counter=1 To filecount
					filename = ["] + Allt(co_dir_name)+"\"+Allt(gcfile(counter,1)) + ["]
					Dele File &filename
					If ERRVAR = .T.
						Exit For
					Endif
				Endfor
				On Error
				Local removedir
				removedir = Allt(co_dir_name)
				If Allt(Uppe(Sys(5)+Curd())) == (Allt(Uppe(removedir)) + [\]) Then
					Cd..
				Endif
				Rd '&removedir'
				Local filecount
				filecount = Adir(gcfile, removedir+"\*.*",[ARHS])
				If filecount > 0 Then
					=Messagebox("One Or More Files Still Found In Folder",64,"Folder " + "[" + removedir;
						+ "]" + " CanNot Be Removed")
				Endif
				On Error
				If Not Empt(prev_err_hnd) Then
					On Error &prev_err_hnd
				Endif
				Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
				Return .F.
			Else
				lSave=Thisform.sqlconobj._Sqlcommit("thisform.nhandle")
				If lSave > 0
					If Thisform.ADdmode
						Messagebox("Company has been Created Successfully",64,vUmess)
						Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
					Else
						Messagebox("Company has been Updated Successfully",64,vUmess)
						Thisform.Pageframe1.Page1.cmdcoName.Visible = .T.
					Endif
				Else
					rB=Thisform.sqlconobj._Sqlrollback("thisform.nhandle")
					If rB < 0
						Messagebox("RollBack Error!!!",16,vUmess)
						Return .F.
					Endif
					Messagebox("Error occured!!! Could not Save"+Chr(13)+"Error Message:"+lErrorMsg,16,vUmess)
					Return .F.
				Endif
			Endif
		Endif
	Endif
Endif
Loca For Allt(Upper(co_name)) == Allt(Upper(co_name_new))
*!*	Else && Not FOund Vudyog
*!*	********************************FOR USER ADMIN*************************
*!*		Sele User
*!*		Locate For Alltrim(Upper(User))="ADMIN"
*!*		lccompany=Alltrim(User.company)
*!*		nm1=Padl(Alltr(User.User),Len(User.company),Alltr(User.User))
*!*		INS = ' '
*!*		chk=0
*!*		For J = 1 To Len(lccompany)
*!*			N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
*!*			If N<=0
*!*				chk=1
*!*				Loop
*!*			Endif
*!*			INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
*!*		Endfor
*!*		If chk=1
*!*			Wait Wind 'Database misuse!  Recreate the user access for '+Alltr(User.User)
*!*			Return
*!*		Endif
*!*		INS=Alltrim(INS)
*!*		Tot_str= INS+Thisform.Pageframe1.Page1.CO_NAME1.Value +CO_DATE+ "[Admin ]" + Chr(13)
*!*		nm=Alltr(User.User)
*!*		out=' '
*!*		If !Empty(Tot_str)
*!*			nm1=Padl(nm,Len(Tot_str),nm)
*!*			For i = 1 To Len(Tot_str)
*!*				out = out+ Chr(Asc(Substr(Tot_str,i,1)) + Asc(Substr(nm1,i,1)))
*!*			Endfor
*!*		Endif
*!*		out=Alltr(out)
*!*		Replace User.company With out
*!*		Sele _comast
*!*	***********************************FOR USER MANAGER***********************

*!*		Sele User
*!*		Locate For Alltrim(Upper(User))==Alltrim('MANAGER')
*!*		If Found()
*!*	*SEEK(UPPER('MANAGER'))
*!*			lccompany=Alltrim(User.company)
*!*			nm1=Padl(Alltr(User.User),Len(User.company),Alltr(User.User))
*!*			INS = ' '
*!*			chk=0
*!*			For J = 1 To Len(lccompany)
*!*				N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
*!*				If N<=0
*!*					chk=1
*!*					Loop
*!*				Endif
*!*				INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
*!*			Endfor
*!*			If chk=1
*!*				Wait Wind 'Database misuse!  Recreate the user access for '+Alltr(User.User)
*!*				Return
*!*			Endif
*!*			INS=Alltrim(INS)
*!*			Tot_str= INS+Thisform.Pageframe1.Page1.CO_NAME1.Value +CO_DATE+ "[Manager  ]" + Chr(13)
*!*			nm=Alltr(User.User)
*!*			out=' '
*!*			If !Empty(Tot_str)
*!*				nm1=Padl(nm,Len(Tot_str),nm)
*!*				For i = 1 To Len(Tot_str)
*!*					out = out+ Chr(Asc(Substr(Tot_str,i,1)) + Asc(Substr(nm1,i,1)))
*!*				Endfor
*!*			Endif
*!*			out=Alltr(out)
*!*			Replace User.company With out
*!*		Endif
*!*		Sele _comast
*!*		Go Bottom
*!*	*1*                      ACCESS FOR MANAGER USER
*!*		Sele User
*!*		If nameuser <> "" And Alltrim(Upper(nameuser)) <> Alltrim(Upper('MANAGER'))
*!*			Sele User
*!*			Locate For Alltrim(User)=Alltrim(nameuser)
*!*			If Found()&&SEEK(ALLTRIM(UPPER(nameuser)))
*!*				lccompany=Alltrim(User.company)
*!*				nm1=Padl(Alltr(User.User),Len(User.company),Alltr(User.User))
*!*				INS = ' '
*!*				chk=0
*!*				For J = 1 To Len(lccompany)
*!*					N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
*!*					If N<=0
*!*						chk=1
*!*						Loop
*!*					Endif
*!*					INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
*!*				Endfor
*!*				If chk=1
*!*					Wait Wind 'Database misuse!  Recreate the user access for '+Alltr(User.User)
*!*					Return
*!*				Endif
*!*				INS=Alltrim(INS)
*!*				Tot_str= INS+Thisform.Pageframe1.Page1.CO_NAME1.Value + CO_DATE + "[Manager ]" + Chr(13)
*!*				nm=Alltr(User.User)
*!*				out=' '
*!*				If !Empty(Tot_str)
*!*					nm1=Padl(nm,Len(Tot_str),nm)
*!*					For i = 1 To Len(Tot_str)
*!*						out = out+ Chr(Asc(Substr(Tot_str,i,1)) + Asc(Substr(nm1,i,1)))
*!*					Endfor
*!*				Endif
*!*				out=Alltr(out)
*!*				Replace User.company With out
*!*				Sele _comast
*!*				Go Bottom
*!*			Endif
*!*		Endif
*!*		Sele _comast
*!*	Endif

*!*	lcd=allt(ThisForm.Pageframe1.Page1.DATA_DIR1.value) + "\lcode.dbf"
*!*	commen=allt(ThisForm.Pageframe1.Page1.DATA_DIR1.value) + "\com_menu.dbf"
&& opening Lcode
*!*	IF !used("lcode")
*!*		USE &lcd excl alias lcode
*!*	ELSE
*!*		SELE lcode
*!*		USE
*!*		USE &lcd excl alias lcode
*!*	ENDIF
*!*	SELE lcode
*!*	SET order to cd
*!*	SELE lcode
*!*	SET order to cd
ndName=Alltrim(Thisform.Pageframe1.Page1.dbname.Value)
If !_comast.Import
	lcodecd=""
	nCon=0
	lOpen=Thisform.sqlconobj.dataconn("EXE","&ndName","select * from lcode","lcode","thisform.nhandle1",This.DataSessionId)
	If lOpen > 0
		Sele lcode
		Scan
			lcodecd=lcodecd+","+lcode.Cd
		Endscan
	Endif
	CMenu=Thisform.sqlconobj.dataconn("EXE","&ndName","select * from com_menu","com_menu","thisform.nhandle1",This.DataSessionId)
&&& For COM_Menu
	datacd=""
	Sele com_menu
	Set Filter To Between(Range,1000,1500) And At('vu_',com_menu.SkipFor)>0
	ln=2
	Scan
		mskip=com_menu.SkipFor
		mfon=At('vu_',mskip)
		mcd=Substr(mskip,mfon+3,2)
		If At("1",mcd)>0
			mcd=Substr(mcd,1,1)
			mcd=mcd+" "
			mcd=Upper(mcd)
		Endif
		mcd=Upper(mcd)
		datacd=datacd+","+mcd
	Endscan
*!*	&&& For Lcode
	Sele lcode
	If Thisform.ADdmode=.T.
		ln=2
		lencd=Len(lcodecd)
		Do While lencd>0
			slcd=Substr(lcodecd,ln,2)
			If  slcd $ datacd
			Else
				oldcD=Substr(lcodecd,ln,2)
				Locate For Alltrim(oldcD)
				If Found()
					mSqlStr = Thisform.sqlconobj.genDelete("lcode","cd=?oldCD")
					DelLcode=Thisform.sqlconobj.dataconn("EXE",company.dbname,mSqlStr,"","thisform.nhandle1",This.DataSessionId)
					If DelLcode < 0
						Messagebox("Could Not Update Lcode",16,vUmess)
						Return .F.
					Endif
*				Dele
*				PACK
				Endif
			Endif
			ln=ln+3
			lencd=lencd-3
		Enddo
	Endif
Endif

mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle")
If mRet <= 0
	Return .F.
Endif
If Type('thisform.nhandle1')<>"U"
	mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle1")
	If mRet <= 0
		Return .F.
	Endif
Endif
If Type("MstConn")<>"U"
	mRet=Thisform.sqlconobj.sqlconnclose(MstConn)
	If mRet <= 0
		Return .F.
	Endif
Endif
If Type("thisform.nhandle3")<>"U"
	mRet=Thisform.sqlconobj.sqlconnclose("thisform.nhandle3")
	If mRet <= 0
		Return .F.
	Endif
Endif
If Type("nhandle")<>"U"
	mRet=Thisform.sqlconobj.sqlconnclose("nhandle")
	If mRet <= 0
		Return .F.
	Endif
Endif
If Type("newcon")<>"U"
	mRet=Thisform.sqlconobj.sqlconnclose("newcon")
	If mRet <= 0
		Return .F.
	Endif
Endif
If Type("tcon")<>"U"
	mRet=Thisform.sqlconobj.sqlconnclose("tconn")
	If mRet <= 0
		Return .F.
	Endif
Endif

Thisform.FRMEDIT_MODE = .T.
Thisform.ADdmode=.F.
Thisform.Editmode =.F.
If Used('lcode')
	Sele lcode
	Use
Endif
Thisform.ACT_DEACT(.F.)
tbrdesktop.Refresh()
If Thisform.NewCompany=.T.
	Messagebox("Please Relogin to Continue",64,vUmess)
	Thisform.Unload()
	Thisform.Release()
Endif
=BARSTAT(.T.,.T.,.T.,.T.,.F.,.T.,Iif(Thisform.NewCompany=.T.,.T.,Thisform.AddButton),Iif(Thisform.NewCompany=.T.,.F.,Thisform.EditButton),Iif(Thisform.NewCompany=.T.,.F.,Thisform.DeleteButton),.F.,.T.,Iif(Thisform.NewCompany=.T.,.F.,Thisform.PrintButton),.T.,.T.)
