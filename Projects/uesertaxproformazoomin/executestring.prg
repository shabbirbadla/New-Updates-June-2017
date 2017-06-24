PARAMETERS _datasessionid
SET DATASESSION TO 	_datasessionid
Local lcReturn As String
lcReturn = ''
lcReturn =STRTRAN(_rstatus.SQLQUERY,";"," ")

If _rstatus.isfr_date
	Set Date AMERICAN
	lcReturn = lcReturn+"'','','','"+Transform(_tmpvar.sDate)+"',"
	Set Date BRITISH
Else
	lcReturn = lcReturn+[' ',]
Endif

If _rstatus.isto_date
	Set Date AMERICAN
	lcReturn = lcReturn+"'"+Transform(_tmpvar.eDate)+"',"
	Set Date BRITISH
Else
	lcReturn = lcReturn+[' ',]
Endif

If _rstatus.isfr_ac
	lcReturn = lcReturn +[']+(  STRTRAN(IIF(EMPTY(_tmpvar.sName),'',_tmpvar.sName),"'","''''")  )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',]
Endif

If _rstatus.isto_ac
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.eName),'',_tmpvar.eName),"'","''''")  )+[',] 
Else
	lcReturn = lcReturn+[' ',]
Endif

If _rstatus.isfr_item
		lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.sItem),'',_tmpvar.sItem),"'","''''")  )+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _rstatus.isto_item
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.eItem),'',_tmpvar.eItem),"'","''''")  )+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _rstatus.isfr_amt
	lcReturn = lcReturn+Transform(_tmpvar.sAmt)+[,] 
Else
	lcReturn = lcReturn+[0,]
Endif

If _rstatus.isto_amt
	lcReturn = lcReturn+Transform(_tmpvar.eAmt)+[,]
Else
	lcReturn = lcReturn+[0,]
Endif

If _rstatus.isdept
		lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.sDept),'',_tmpvar.sDept),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_tmpvar.eDept),'',_tmpvar.eDept),"'","''''") )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _rstatus.iscategory
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.Scat),'',_tmpvar.Scat),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_tmpvar.eCat),'',_tmpvar.eCat),"'","''''") )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _rstatus.iswarehous
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.sWare),'',_tmpvar.sWare),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_tmpvar.eWare),'',_tmpvar.eWare),"'","''''") )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _rstatus.isinvseri
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_tmpvar.sInvSr),'',_tmpvar.sInvSr),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_tmpvar.eInvSr),'',_tmpvar.eInvSr),"'","''''") )+[',] &&Rup 02/01/10
else
	lcReturn = lcReturn+[' ',' ',]
Endif

lcReturn = lcReturn+"'"+Alltrim(Str(Year(company.sta_dt)))+'-'+Alltrim(Str(Year(company.end_dt)))+"',''"

Return lcReturn
