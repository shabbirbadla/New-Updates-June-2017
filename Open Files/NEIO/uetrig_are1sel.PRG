lparam vDataSessionId
Set DataSession To vDataSessionId
DO UETRIG_SELECTPOP WITH vDataSessionId,"stmain.areno","select distinct ARENO from STMAIN where areno<>' ' and aredesc='A.R.E.1'","Select ARE1 No.","ARENO","ARENO","",.F.,"","",.t.,[],[],"ARENO:ARE1 No."
