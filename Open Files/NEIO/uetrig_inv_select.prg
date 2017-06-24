lparam vDataSessionId
Set DataSession To vDataSessionId
DO uetrig_SP_selectpop.prg WITH vDataSessionId,"stmain.inv_no","select distinct inv_no from STMAIN where inv_no<>' ' and entry_ty = 'EI'","Select Invoice No.","Inv_no","Inv_no","",.F.,"","",.t.,[],[],"Invoice:Invoice No."
