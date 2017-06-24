lparam vDataSessionId
Set DataSession To vDataSessionId
DO UETRIG_SELECTPOP WITH vDataSessionId,"stmain.areno","select distinct ARENO from STMAIN where areno<>' ' and aredesc='A.R.E.2'","Select ARE2 No.","ARENO","ARENO","",.F.,"","",.t.,[],[],"ARENO:ARE2 No."
