Local lcReturn As String
lcReturn = ''

If _rstatus.isfr_date
	Set Date AMERICAN
	lcReturn = lcReturn+"'"+Transform(_otmpvar.sDate)+"',"
	Set Date BRITISH
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isto_date
	Set Date AMERICAN
	lcReturn = lcReturn+"'"+Transform(_otmpvar.eDate)+"',"
	Set Date BRITISH
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isfr_ac
*!*		lcReturn = lcReturn+[']+_otmpvar.sName+[',] &&Rup 03/02/10 L2S-55
	lcReturn = lcReturn +[']+(  STRTRAN(IIF(EMPTY(_otmpvar.sName),'',_otmpvar.sName),"'","''''")  )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isto_ac
*!*		lcReturn = lcReturn+[']+_otmpvar.eName+[',] &&Rup 02/01/10 L2S-55
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.eName),'',_otmpvar.eName),"'","''''")  )+[',] 
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isfr_item
*!*		lcReturn = lcReturn+[']+_otmpvar.sItem+[',] &&Rup 03/02/10 L2S-55
		lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.sItem),'',_otmpvar.sItem),"'","''''")  )+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isto_item
*!*		lcReturn = lcReturn+[']+_otmpvar.eItem+[',] &&Rup 03/02/10 L2S-55
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.eItem),'',_otmpvar.eItem),"'","''''")  )+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isfr_amt
	lcReturn = lcReturn+Transform(_otmpvar.sAmt)+[,] 
Else
	lcReturn = lcReturn+[0,]
Endif

If _Orstatus.isto_amt
	lcReturn = lcReturn+Transform(_otmpvar.eAmt)+[,]
Else
	lcReturn = lcReturn+[0,]
Endif

If _Orstatus.isdept
*!*		lcReturn = lcReturn+[']+(_otmpvar.sDept)+[',']+(_otmpvar.eDept)+[',] &&Rup 03/02/10 L2S-55
		lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.sDept),'',_otmpvar.sDept),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_otmpvar.eDept),'',_otmpvar.eDept),"'","''''") )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _Orstatus.iscategory
*!*		lcReturn = lcReturn+[']+(_otmpvar.Scat)+[',']+(_otmpvar.eCat)+[',] &&Rup 03/02/10 L2S-55
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.Scat),'',_otmpvar.Scat),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_otmpvar.eCat),'',_otmpvar.eCat),"'","''''") )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _Orstatus.iswarehous
*!*		lcReturn = lcReturn+[']+(_otmpvar.sWare)+[',']+(_otmpvar.eWare)+[',] &&Rup 03/02/10 L2S-55
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.sWare),'',_otmpvar.sWare),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_otmpvar.eWare),'',_otmpvar.eWare),"'","''''") )+[',] &&Rup 02/01/10
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _Orstatus.isinvseri
*!*		lcReturn = lcReturn+[']+(_otmpvar.sInvSr)+[',']+(_otmpvar.eInvSr)+[',] &&Rup 03/02/10 L2S-55
	lcReturn = lcReturn+[']+(  STRTRAN(IIF(EMPTY(_otmpvar.sInvSr),'',_otmpvar.sInvSr),"'","''''")  )+[',']+( STRTRAN(IIF(EMPTY(_otmpvar.eInvSr),'',_otmpvar.eInvSr),"'","''''") )+[',] &&Rup 02/01/10
else
	lcReturn = lcReturn+[' ',' ',]
Endif

lcReturn = lcReturn+"'"+Alltrim(Str(Year(company.sta_dt)))+'-'+Alltrim(Str(Year(company.end_dt)))+"',"

Return lcReturn
