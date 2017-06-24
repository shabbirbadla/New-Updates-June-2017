Lparameters lrepid,lrange

****Versioning**** Added By Amrendra On 01/06/2011
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
Try
	_VerRetVal = AppVerChk('DADOSREPORT',_CurrVerVal,Justfname(Sys(16)))
Catch To _VerValidErr
	_VerRetVal  = 'NO'
Endtry
If Type("_VerRetVal")="L"
	cMsgStr="Version Error occured!"
	cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
	Messagebox(cMsgStr,64,VuMess)
	Return .F.
Endif
If _VerRetVal  = 'NO'
	Return .F.
Endif
****Versioning****
If Used('mx2')
	Use In mx2
Endif

If Used('mx3')
	Use In mx3
Endif

Set Step On
nconn=0
If 'SQLCONNECTION' $ Upper(Alltrim(Set("Classlib")))
Else
	Set Classlib To sqlconnection In &xapps Additive
Endif
Local sqlconobj
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
_user=sqlconobj.dec(sqlconobj.ondecrypt(mvu_user))
_pass=sqlconobj.dec(sqlconobj.ondecrypt(mvu_pass))
*!*	constr = 'Driver={SQL server};server=udyog3;database='+ALLTRIM(company.dbname)+';uid='+_m1key_user+';pwd='+_m1key_pass+';'

constr = 'Driver={SQL server};server='+mvu_server+';database='+Alltrim(company.dbname)+';uid='+_user+';pwd='+_pass+';'

*!*	con_str="[RptID="+lrepid+"][UserName=Admin][DomainID=18]";
*!*		  +"[CON1KEY={Provider=SQLOLEDB.1;Password="+_m1key_pass+";Persist Security Info=True;User ID="+_m1key_user+";Initial Catalog="+ALLTRIM(company.dbname)+"}]"
con_str='"'+lrepid +"|[UserName=Admin][Domain= "+Alltrim(company.co_name)+" ( "+Alltrim(company.l_yn)+" ) ]" + "|"  + "|"  +  "|" ;
	+ "|Data Source="+mvu_server+";Persist Security Info=True;Password="+_pass+";User ID="+_user+";Initial Catalog="+Alltrim(company.dbname)+'"'

nconn=Sqlstringconnect(constr)
If nconn<= 0
	nRetval=SQLDisconnect(nconn)
	If nRetval<=0
		Return .F.
	Endif
	=Messagebox("Connection Failed !!!"+Chr(13)+Chr(13)+Message(),0+64,VuMess)
	Return .F.
Endif
Local lrpttype,lrpthead
nRetval=SQLExec(nconn,'select * from usrep where repid = ?lrepid','MX2')
If nRetval<=0
	nRetval=SQLDisconnect(nconn)
	If nRetval<=0
		Return .F.
	Endif
	=Messagebox("Error in usrep table"+Chr(13)+Chr(13)+Message(),64,VuMess)
	Return .F.
Else
	If Reccount('mx2')=0
		nRetval=SQLDisconnect(nconn)
		If nRetval<=0
			Return .F.
		Endif
		=Messagebox("Report ID :"+Alltrim(lrepid)+" not found",64,VuMess)
		Return .F.
	Else
		lrpthead = Alltrim(mx2.repnm)
		lrpttype = mx2.repty
	Endif
Endif

&& Added By Shrikant S. on  11/06/2012 for Bug-4576		&& Start

*!*	If Alltrim(Upper(lrpttype))=="VIEW"				&& Commented by Archana for Bug-21126 	
If INLIST(Alltrim(Upper(lrpttype)),"VIEW","CHART")		&& Added by Archana for Bug-21126  && Added by Shrikant S. on 24/12/2014  for Auto Updater 11.0.8
	tcCompId = company.CompId
	tcCompdb = company.dbname
	tcCompNm = company.co_name
	vicopath  =Strtran(icopath,' ','<*#*>')
	pApplCaption=Strtran(vumess,' ','<*#*>')
	_PassRoute1 = Alltrim(company.PassRoute1)
	appPath=STRTRAN(TRANSFORM(apath),' ','<*#*>')
	csetPath=Set("Path")
	SetPath=STRTRAN(TRANSFORM(csetPath),' ','<*#*>')
	csdate =STRTRAN(TRANSFORM(DTOC(company.sta_dt)),' ','<*#*>') 
	cedate =STRTRAN(TRANSFORM(DTOC(company.end_dt)),' ','<*#*>') 
	_prodcode =""
	pvalue = Allt(company.PassRoute)+Iif(!Empty(Alltrim(company.PassRoute1)),",","")+Alltrim(company.PassRoute1)
	For i = 1 To Len(pvalue)
		_prodcode = _prodcode + Chr(Asc(Substr(pvalue,i,1))/2)
	Next i
	_prodcode = _prodcode+Iif(Right(Alltrim(_prodcode),1)=',',"",",")

*!*		For i = 1 To Len(_PassRoute1)
*!*			_prodcode = _prodcode + Chr(Asc(Substr(_PassRoute1,i,1))/2)
*!*		Next i
*!*		_prodcode = _prodcode+','

	con_str= con_str+" "+ALLTRIM(Transform(tcCompId))+" "+Alltrim(tcCompdb)
	con_str= con_str+" "+Alltrim(mvu_server)+" "+Alltrim(_user)+" "+Alltrim(_pass)
	con_str= con_str+" "+Alltrim(lrange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)
	con_str= con_str+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "+_prodcode
	con_str= con_str+" "+ALLTRIM(appPath)+" "+ALLTRIM(SetPath)+" "+ALLTRIM(csdate)+" "+ALLTRIM(cedate)
Endif

&& Added By Shrikant S. on  11/06/2012 for Bug-4576		&& End


*!*	nretval=sqlexec(nconn,'select * from usrlv where repid = ?lrepid','MX2')
*!*	if nretval<=0
*!*		nRetval=sqldisconnect(nconn)
*!*		IF nRetval<=0
*!*			RETURN .f.
*!*		ENDIF
*!*		=messagebox("Error in usrlv table"+chr(13)+chr(13)+message(),64,vumess)
*!*		return .f.
*!*	endif



*!*	select mx2
*!*	go top
*!*	if empty(mx2.lvlid)
*!*		=messagebox("Level Id not found in Report ID :"+alltrim(lrepid),64,vumess)
*!*		return .f.
*!*	ELSE

*!*		qid = queryid


** Commented By Shrikant S. on 05/06/2012 for Bug-4576		&& Start
*!*	msqlStr = 'select b.parameterID from para_query_master b '
*!*	msqlStr = msqlStr + 'where b.repid = ?lrepid group by b.parameterID'
** Commented By Shrikant S. on 05/06/2012 for Bug-4576		&& End
** Added By Shrikant S. on 05/06/2012 for Bug-4576		&& Start
msqlStr = 'select b.parameterID,b.para_order from para_query_master b '
msqlStr = msqlStr + 'where b.repid = ?lrepid group by b.parameterID,b.para_order '
msqlStr = msqlStr + ' order by b.para_order '		&& added By Shrikant S. on 04/06/2012
** Added By Shrikant S. on 05/06/2012		&& End
nRetval=SQLExec(nconn,msqlStr,'MX3')
If nRetval <=0
	nRetval=SQLDisconnect(nconn)
	If nRetval<=0
		Return .F.
	Endif
	=Messagebox("Error in para_query_master table"+Chr(13)+Chr(13)+Message(),64,VuMess)
	Return .F.

Else
	Select mx3
	If Reccount('mx3')=0
		Do Case
		Case lrpttype = "View"
*			If File('C:\Program Files\Udyog Software\Dados Reports\DadosReports.exe ')
			If File('DadosReports.exe ') &&Birendra : Bug-957 on 14/12/2011
				nRetval=SQLDisconnect(nconn)
				If nRetval<=0
					Return .F.
				ENDIF
*				lcCommand="C:\Program Files\Udyog Software\Dados Reports\DadosReports.exe "+con_str &&Birendra : Bug-957 on 14/12/2011 (Commented)
				lcCommand="DadosReports.exe "+con_str &&Birendra : Bug-957 on 14/12/2011
				oWSHELL = Createobject("WScript.Shell")
				oWSHELL.Exec(lcCommand)
				=Inkey(1.5)
			Else
				nRetval=SQLDisconnect(nconn)
				If nRetval<=0
					Return .F.
				Endif
				=Messagebox("Dados Tool not found...!!!",64,VuMess)
				Return .F.
			Endif
		Case lrpttype = "Chart"
*			If File('C:\Program Files\Udyog Software\Dados Charts\DadosCharts.exe ') &&Birendra : Bug-957 on 14/12/2011 (Commented)
			If File('DadosCharts.exe ') &&Birendra : Bug-957 on 14/12/2011
				nRetval=SQLDisconnect(nconn)
				If nRetval<=0
					Return .F.
				Endif
*				lcCommand="C:\Program Files\Udyog Software\Dados Charts\DadosCharts.exe "+con_str &&Birendra : Bug-957 on 14/12/2011 (Commented)
				lcCommand="DadosCharts.exe "+con_str &&Birendra : Bug-957 on 14/12/2011
				oWSHELL = Createobject("WScript.Shell")
				oWSHELL.Exec(lcCommand)
				=Inkey(1.5)
			Else
				nRetval=SQLDisconnect(nconn)
				If nRetval<=0
					Return .F.
				Endif
				=Messagebox("Dados Tool not found...!!!",64,VuMess)
				Return .F.
			Endif
		Endcase
	Else
		Use In mx2
		Use In mx3
		Do Form Dados-interface With lrepid,lrpthead,lrpttype,lrange
	Endif
Endif

*!*	endif
Return
