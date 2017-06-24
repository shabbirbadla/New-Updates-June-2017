lparam vDataSessionId
Set DataSession To vDataSessionId
DO UETRIG_SELECTPOP WITH vDataSessionId,"stmain.areno","select distinct ARENO from STMAIN where areno<>' ' and aredesc='A.R.3.A'","Select ARE3A No.","ARENO","ARENO","",.F.,"","",.t.,[],[],"ARENO:AR3A No."

