Alter table lOther ALTER COLUMN fld_nm vARCHAR(30) 
Alter table it_mast alter column U_GVTYPE VARCHAR(30)

If Not Exists(select [filtcond] from LOTHER where [filtcond] like '%Composition Dealer%' And [e_code] = 'ST' and [head_nm] = 'Material Trans. Type')
Begin
	Update LOTHER SET filtcond = cast(filtcond as nvarchar(max)) + Cast(',Composition Dealer' as nvarchar(max)) where e_code = 'ST' and head_nm = 'Material Trans. Type'
END

If Not Exists(select [filtcond] from LOTHER where [filtcond] like '%Exempt Transaction%' And [e_code] = 'ST' and [head_nm] = 'Material Trans. Type')
Begin
	Update LOTHER SET filtcond = cast(filtcond as nvarchar(max)) + Cast(',Exempt Transaction' as nvarchar(max)) where e_code = 'ST' and head_nm = 'Material Trans. Type'
END

IF NOT EXISTS(select head_nm from lother where e_code='im' and fld_nm='U_GVTYPE' AND filtcond LIKE '%Tax Free Sch - I goods%')
BEGIN 
	update lother set filtcond=cast(filtcond as nvarchar(max))+cast(',Tax Free Sch - I goods' as nvarchar(max))  where e_code='im' and fld_nm='U_GVTYPE'
END
