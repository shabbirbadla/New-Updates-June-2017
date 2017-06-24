lparam vDataSessionId
Set DataSession To vDataSessionId
DO UETRIG_SELECTPOP WITH vDataSessionId,"stmain.TRPERNO","select distinct TRPERNO from STMAIN where TRPERNO<>' '","Select Transit Permit No.","TRPERNO","TRPERNO","",.F.,"","",.t.,[],[],"TRPERNO:Transit Permit No."
