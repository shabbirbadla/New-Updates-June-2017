lparam vDataSessionId
Set DataSession To vDataSessionId
DO UETRIG_SELECTPOP WITH vDataSessionId,"stmain.inv_no","select distinct inv_no from STMAIN where inv_no<>' '","Select Invoice No.","Inv_no","Inv_no","",.F.,"","",.t.,[],[],"Invoice:Invoice No."
