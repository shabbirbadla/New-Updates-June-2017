Lparam vDataSessionId
Set Step On
Set DataSession To vDataSessionId
If Alltrim(Upper(_Screen.ActiveForm.frxname)) = "EPCG_EOSUM"
	Do uetrig_licenno_selectpop.prg With vDataSessionId,"epcg_mast.licen_no","select distinct Licen_no from Epcg_Mast where Licen_no<>' '","Select License No.","Licen_no","Licen_no","",.F.,"","",.T.,[],[],"Licen_no:License No."
Endif

If Alltrim(Upper(_Screen.ActiveForm.frxname)) = "AA_EOSUM"
	Do uetrig_licenno_selectpop.prg With vDataSessionId,"aa_mast.licen_no","select distinct Licen_no from AA_Mast where Licen_no<>' '","Select License No.","Licen_no","Licen_no","",.F.,"","",.T.,[],[],"Licen_no:License No."
Endif

If Alltrim(Upper(_Screen.ActiveForm.frxname)) = "EXP_LC_TRK_1"
	Do uetrig_selectpop_lcno With vDataSessionId,"EXPORT_LC_MAST.LC_NO","select distinct LC_NO from EXPORT_LC_MAST","Select LC No.","LC_no","LC_no","",.F.,"","",.T.,[],[],"LC_NO:LC No."
Endif
