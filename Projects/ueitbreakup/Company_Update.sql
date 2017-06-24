If Not Exists(Select it_adv_code From It_Advance_Setting_Master Where it_adv_code='WT' AND FLD_NM='Wastescdet')
Begin
	INSERT INTO It_Advance_Setting_Master([It_Adv_Code], [TabCaption], [TabCode], [sCaption], [sDetails], [fld_nm], [sorder]) 
	VALUES ('WT', 'Advance Setting', 'TB1', 'Wastage-Scrap', 'DO ueitbreakup.app WITH thisform,"WT"', 'Wastescdet', 2)
End
If Not Exists(Select it_adv_code From It_Advance_Setting_Master Where it_adv_code='SC' AND FLD_NM='ScrapDet')
Begin
	INSERT INTO It_Advance_Setting_Master([It_Adv_Code], [TabCaption], [TabCode], [sCaption], [sDetails], [fld_nm], [sorder]) 
	VALUES ('SC', 'Advance Setting', 'TB1', 'Process-Loss', 'DO ueitbreakup.app WITH thisform,"SC"', 'ScrapDet', 3)
End
If Not Exists(Select it_adv_code From It_Advance_Setting_Master Where it_adv_code='BY' AND FLD_NM='ByProdDet')
Begin
	INSERT INTO It_Advance_Setting_Master([It_Adv_Code], [TabCaption], [TabCode], [sCaption], [sDetails], [fld_nm], [sorder]) 
	VALUES ('BY', 'Advance Setting', 'TB1', 'By-Product', 'DO ueitbreakup.app WITH thisform,"BY"', 'ByProdDet', 4)
End
If Not Exists(Select [name] From SysObjects Where xType='U' and [Name]='it_WastescrapDet')
Begin
	Create table it_WastescrapDet(It_code Numeric(18),sit_code Numeric(18),qtyPer Numeric(5,3))
End
If Not Exists(Select [name] From SysObjects Where xType='U' and [Name]='it_scrapDet')
Begin
	Create table it_scrapDet(It_code Numeric(18),sit_code Numeric(18),qtyPer Numeric(5,3))
End
If Not Exists(Select [name] From SysObjects Where xType='U' and [Name]='it_byProdDet')
Begin
	Create table it_byProdDet(It_code Numeric(18),sit_code Numeric(18),qtyPer Numeric(5,3))
End
Execute Add_columns 'It_Advance_Setting','Wastescdet bit'
Execute Add_columns 'It_Advance_Setting','scrapdet bit'
Execute Add_columns 'It_Advance_Setting','ByProdDet bit'
Execute Add_columns 'It_mast','IsScrap bit'
