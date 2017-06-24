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
	lcReturn = lcReturn+[']+_otmpvar.sName+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isto_ac
	lcReturn = lcReturn+[']+_otmpvar.eName+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isfr_item
	lcReturn = lcReturn+[']+_otmpvar.sItem+[',]
Else
	lcReturn = lcReturn+[' ',]
Endif

If _Orstatus.isto_item
	lcReturn = lcReturn+[']+_otmpvar.eItem+[',]
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
	lcReturn = lcReturn+[']+(_otmpvar.sDept)+[',']+(_otmpvar.eDept)+[',]
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _Orstatus.iscategory
	lcReturn = lcReturn+[']+(_otmpvar.Scat)+[',']+(_otmpvar.eCat)+[',]
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _Orstatus.iswarehous
	lcReturn = lcReturn+[']+(_otmpvar.sWare)+[',']+(_otmpvar.eWare)+[',]
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

If _Orstatus.isinvseri
	lcReturn = lcReturn+[']+(_otmpvar.sInvSr)+[',']+(_otmpvar.eInvSr)+[',]
Else
	lcReturn = lcReturn+[' ',' ',]
Endif

lcReturn = lcReturn+"'"+Alltrim(Str(Year(company.sta_dt)))+'-'+Alltrim(Str(Year(company.end_dt)))+"',"

Return lcReturn
