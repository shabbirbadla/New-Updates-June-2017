PARAMETERS vDataSessionId,EDT,INVNO

*Para mFlag
*MESSAGEBOX(INVNO)
*MESSAGEBOX(Alltrim(Dtoc(edt)))
*SET DATASESSION TO vDataSessionId
*vDataSessionId= DATASESSION

Local mAlias
mAlias = alias()


LOCAL sqlconobj,whedate,whMCON,MVAL,MVAL1
nHandle=0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
sq1="SELECT pformula,progcond,progopamt  from manufact " &&AND INV_STK<>' '
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"manufact","nHandle",vDataSessionId)
If nRetval<0
	=Messagebox("Lcode table error"+Chr(13)+Proper(Message()),48,vuMess)
	nRetval=sqlconobj.sqlconnclose(nHandle)
	If nRetval<0
		=Messagebox("SQL disconnect error"+Chr(13)+Proper(Message()),48,vuMess)
	Endif
	Return .F.
ENDIF



if !empty(manufact.pformula)
   mval = allt(manufact.pformula)
  else
   mval = '(ITEM.qty*ITEM.rate)'
ENDIF
SQ1="SELECT SUM("+ALLTRIM(MVAL)+") AS VAMT FROM MAIN INNER JOIN ITEM ON (MAIN.ENTRY_TY=ITEM.ENTRY_TY AND MAIN.TRAN_CD=ITEM.TRAN_CD)"
whedate=" WHERE MAIN.date <= '"+Alltrim(Dtoc(edt))+"' "
whmcon=IIF(!empty(manufact.progcond),' AND '+manufact.progcond,'')


*WHERE '2005/03/31'<=STMAIN.DATE AND CAST(STMAIN.INV_NO AS INT )<155 AND STMAIN.ENTRY_TY='ST'

*CREATE CURSOR progcur (a MEMo,b memo)
*INSERT INTO progcur (A,b) VALUES ("SET DATEFORMAT DMY "+ALLTRIM(sq1)+whedate+whmcon+" AND MAIN.INV_NO<= "+invno+" AND MAIN.ENTRY_TY='VENT'","SET DATEFORMAT DMY "+ALLTRIM(sq1)+whedate+whmcon+" AND MAIN.ENTRY_TY='VENT'")

STR1=ALLTRIM(sq1)+whedate+whmcon+" AND MAIN.INV_NO<= "+ALLTRIM(invno)+" AND MAIN.ENTRY_TY='VENT'"
STR2=ALLTRIM(sq1)+whedate+whmcon+" AND MAIN.ENTRY_TY='VENT'"


STR1=STRTRAN(STR1,'ITEM', 'STITEM')
STR1=STRTRAN(STR1,'MAIN', 'STMAIN')
STR1=STRTRAN(STR1,'VAMT', 'VSTAMT')
STR1=STRTRAN(STR1,'VENT', 'ST')

STR2=STRTRAN(STR2,'ITEM', 'SRITEM')
STR2=STRTRAN(STR2,'MAIN', 'SRMAIN')
STR2=STRTRAN(STR2,'VAMT', 'VSRAMT')
STR2=STRTRAN(STR2,'VENT', 'SR')

INSERT INTO T11 (A) VALUES ("SET DATEFORMAT DMY "+ALLTRIM(STR1))
nRetval = sqlconobj.dataconn([EXE],company.dbname,"SET DATEFORMAT DMY "+ALLTRIM(STR1),"STCUR","nHandle",vDataSessionId)
If nRetval<0
	=Messagebox("Lcode table error"+Chr(13)+Proper(Message()),48,vuMess)
	nRetval=sqlconobj.sqlconnclose(nHandle)
	If nRetval<0
		=Messagebox("SQL disconnect error"+Chr(13)+Proper(Message()),48,vuMess)
	Endif
	Return .F.
Endif

*!*	SET STEP ON 

SELECT stcur
stamt=IIF(!ISNULL(stcur.vstamt),stcur.vstamt,0)

nRetval = sqlconobj.dataconn([EXE],company.dbname,"SET DATEFORMAT DMY "+ALLTRIM(STR2),"SRCUR","nHandle",vDataSessionId)
If nRetval<0
	=Messagebox("Lcode table error"+Chr(13)+Proper(Message()),48,vuMess)
	nRetval=sqlconobj.sqlconnclose(nHandle)
	If nRetval<0
		=Messagebox("SQL disconnect error"+Chr(13)+Proper(Message()),48,vuMess)
	Endif
	Return .F.
Endif
SELECT srcur

sramt=IIF(!ISNULL(srcur.vsramt),srcur.vsramt,0)
*MESSAGEBOX(STR(SRAMT))
*MESSAGEBOX(Alltrim(Dtoc(STR(STAMT))))

Select(mAlias)
RETURN stamt-sramt+IIF(!ISNULL(manufact.progopamt),manufact.progopamt,0)  







