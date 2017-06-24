Lparameters fldnm,tblnm,ooSqlConObj,ooHandle,nDataSessionId,vcDbName,vdSta_Dt,vdEnd_Dt
Local VRNO, nHandle
*!*	nHandle=0			&& Commented by Shrikant S. on 20/05/2010 for TKT-1476
&&msqlstr="SELECT MAX(CAST( "+Alltrim(fldnm)+"  AS INT)) AS RNO  FROM "+Alltrim(tblnm)+" WHERE ISNUMERIC( "+Alltrim(fldnm)+" )=1"
msqlstr="SELECT MAX(CAST( "+Alltrim(fldnm)+"  AS INT)) AS RNO  FROM "+Alltrim(tblnm)+" WHERE ISNUMERIC( "+Alltrim(fldnm)+" )=1  and l_yn = '"+Alltr(Str(Year(vdSta_Dt)))+"-"+Alltr(Str(Year(vdEnd_Dt)))+"'" &&added by satish pal for bug-4996

mRetval = ooSqlConObj.dataconn([EXE],vcdbname,mSqlStr,"EXCUR",ooHandle,nDataSessionId,.T.)
If mRetval<0
	Return .F.
Endif
Select EXCUR
VRNO=Alltrim(Str(Iif(Isnull(EXCUR.RNO),1,(EXCUR.RNO)+1)))
If Used("EXCUR")
	Use In EXCUR
Endif
*sele(mAlias)
Return VRNO
