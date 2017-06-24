create PROCEDURE [dbo].[USP_VU8ER1] 
@LNKSVRNM NVARCHAR(100),@VU8DATAPATH NVARCHAR(100)
AS
--set @LNKSVRNM = 'test1'
--set @VU8DATAPATH = 'D:\Temp\Standardorigimirachem'
EXEC master.dbo.sp_MSset_oledb_prop N'VFPOLEDB', N'AllowInProcess', 1 

declare @errmsg nvarchar(100),@sqlstr nvarchar(4000),@sqlcommand nvarchar(4000)
set @errmsg = ''
set @sqlstr = ''
set @sqlcommand = ''

--delete existing link server
if exists(select * from sys.servers where name = @LNKSVRNM)
begin
execute sp_dropserver @LNKSVRNM,'droplogins'
end

--delete existing tables
if exists(select * from sysobjects where name = 'MANUFACT')
begin
drop table MANUFACT
end

if exists(select * from sysobjects where name = 'LCODE')
begin
drop table LCODE
end

if exists(select * from sysobjects where name = 'AC_MAST')
begin
drop table AC_MAST
end

if exists(select * from sysobjects where name = 'SHIPTO')
begin
drop table SHIPTO
end

if exists(select * from sysobjects where name = 'IT_MAST')
begin
drop table IT_MAST
end

if exists(select * from sysobjects where name = 'BPMAIN')
begin
drop table BPMAIN
end

if exists(select * from sysobjects where name = 'STMAIN')
begin
drop table STMAIN
end

if exists(select * from sysobjects where name = 'STITEM')
begin
drop table STITEM
end

if exists(select * from sysobjects where name = 'STKL_VW_MAIN')
begin
drop table STKL_VW_MAIN
end

if exists(select * from sysobjects where name = 'STKL_VW_ITEM')
begin
drop table STKL_VW_ITEM
end

if exists(select * from sysobjects where name = 'EX_VW_ACDET')
begin
drop table EX_VW_ACDET
end

/*
if exists(select * from sysobjects where name = 'ER_EXCISE')
begin
drop table ER_EXCISE
end
*/

--create link server
execute sp_addlinkedserver @LNKSVRNM,'ER1eFiling','VFPOLEDB',@VU8DATAPATH
if not exists(select * from sys.servers where name = @LNKSVRNM)
begin
	set @errmsg = 'Unable to Create Link Server'
end

--fetching data from vu8 tables
set @sqlstr = 'SELECT * 
	from Manufact'
set @sqlcommand = 'SELECT * into MANUFACT FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT cd as entry_ty,* 
	from Lcode'
set @sqlcommand = 'SELECT * into LCODE FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as Ac_id,* 
	from ac_mast'
set @sqlcommand = 'SELECT * into AC_MAST FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT cast(0 as int) as Ac_id,Recno() as Shipto_id,cast(space(1) as varchar(25)) as Vend_Type,* 
	from Shipto'
set @sqlcommand = 'SELECT * into SHIPTO FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as It_Code,Recno() as ItGrid,
	a.*,b.e_uom as ExRateUnit
	from it_mast a left join uom b on a.rateunit = b.u_uom'
set @sqlcommand = 'SELECT * into IT_MAST FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as Tran_cd,a.Entry_ty,a.Date,a.Doc_no,
	a.U_TR6
	from Lmain a,Lcode b where a.entry_ty = b.cd and (b.cd = ''''BP'''' or b.bcode_nm = ''''BP'''' )'
set @sqlcommand = 'SELECT * into BPMAIN FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as Tran_cd,a.Entry_ty,a.Date,a.Doc_no,
	a.U_CHOICE,a.U_GCSSR,a.U_PFOR
	from Lmain a,Lcode b where a.entry_ty = b.cd and (b.cd = ''''S '''' or b.bcode_nm = ''''S '''' )'
set @sqlcommand = 'SELECT * into STMAIN FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT cast(0 as int) as it_code,cast(0 as int) as Tran_cd,
	a.*,u_hcesper as u_hcessper
	from Litem a,Lcode b where a.entry_ty = b.cd and (b.cd = ''''S '''' or b.bcode_nm = ''''S '''' )'
set @sqlcommand = 'SELECT * into STITEM FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as Tran_cd, date, entry_ty, doc_no,narr, cast(0 as int) as Ac_id, Party_nm,net_amt, gro_amt, 
	dept, cate, inv_sr, rule, u_gcssr, u_pinvno, u_pinvdt , inv_no, 
	U_INT, U_ARREARS, cheq_no, U_CHALNO, U_CHALDT, 
	u_rg23no, u_rg23cno, u_plasr, Rule As U_IMPORM,
	U_TR6, apgen, l_yn,cast(0 as int) as  cons_id,cast(0 as int) as  scons_id,cast(0 as int) as  sac_id
	from lmain'
set @sqlcommand = 'SELECT * into STKL_VW_MAIN FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select cast(0 as int) as  Tran_cd, entry_ty, date, doc_no, cast(0 as int) as  Ac_id, cast(0 as int) as  It_code, Item,
	qty, u_lqty, itserial, pmkey, gro_amt, u_asseamt, ware_nm, 
	u_pageno, dc_no, narr, space(10) as batchno, {} as mfgdt,{} as expdt,rate 
	from litem'
set @sqlcommand = 'SELECT * into STKL_VW_ITEM FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select  cast(0 as int) as  Tran_cd, entry_ty, date, doc_no, ac_name, cast(0 as int) as  Ac_id, Recno() as acserial,amount,
	amt_ty, u_cldt, l_yn, u_rg23no, u_rg23cno, u_plasr
	from lac_det'
set @sqlcommand = 'SELECT * into EX_VW_ACDET FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

/*
set @sqlstr = 'SELECT * 
	from excise'
set @sqlcommand = 'SELECT * into ER_EXCISE FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand
*/

--Updating data as per VU9 Requirements
set @sqlcommand = 'update lcode set entry_ty = ''OB'' where entry_ty = ''B '''
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update lcode set entry_ty = ''PT'' where entry_ty = ''P '''
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update lcode set entry_ty = ''ST'' where entry_ty = ''S '''
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update lcode set entry_ty = ''IL'' where entry_ty = ''I '''
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update lcode set entry_ty = ''RL'' where entry_ty = ''R '''
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update shipto set ac_id = b.ac_id from shipto a,ac_mast b where a.ac_id = b.ac_id '
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update bpmain set entry_ty = b.entry_ty from bpmain a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stmain set entry_ty = b.entry_ty from stmain a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stitem set entry_ty = b.entry_ty from stitem a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stkl_vw_main set entry_ty = b.entry_ty from stkl_vw_main a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stkl_vw_item set entry_ty = b.entry_ty from stkl_vw_item a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update ex_vw_acdet set entry_ty = b.entry_ty from ex_vw_acdet a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update bpmain set Tran_cd = b.Tran_cd from bpmain a,stkl_vw_main b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stmain set Tran_cd = b.Tran_cd from stmain a,stkl_vw_main b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stitem set Tran_cd = b.Tran_cd from stitem a,stkl_vw_main b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stkl_vw_item set Tran_cd = b.Tran_cd from stkl_vw_item a,stkl_vw_main b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update ex_vw_acdet set Tran_cd = b.Tran_cd from ex_vw_acdet a,stkl_vw_main b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update stitem set It_code = b.It_code from stitem a,it_mast b where a.item = b.it_name'
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update stkl_vw_item set It_code = b.It_code from stkl_vw_item a,it_mast b where a.item = b.it_name'
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update stkl_vw_main set Ac_id = b.Ac_id,Cons_id = b.Ac_id from stkl_vw_main a,ac_mast b where a.Party_nm = b.Ac_name'
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update ex_vw_acdet set Ac_id = b.Ac_id from ex_vw_acdet a,ac_mast b where a.Ac_name = b.Ac_name'
execute SP_EXECUTESQL @sqlcommand

alter table stkl_vw_main alter column party_nm varchar(75)
alter table ex_vw_acdet alter column ac_name varchar(75)
alter table ac_mast alter column ac_name varchar(75)
alter table stkl_vw_main alter column u_arrears varchar(75)

update ac_mast set vend_type = 'Manufacturer' where vend_type = 'MFGR.'
update ac_mast set vend_type = 'I stage Dealer' where vend_type = 'I STAGE DEALER'
update ac_mast set vend_type = 'II stage Dealer' where vend_type = 'II STAGE DEALER'
update ac_mast set vend_type = 'Importer' where vend_type = 'IMPORTER'
update er_excise set ac_id = b.ac_id from er_excise a,ac_mast b where a.ac_name = b.ac_name
update ex_vw_acdet set amt_ty = 'DR' where amt_ty = 'DB'
update stkl_vw_main set entry_ty = 'OS' where entry_ty = 'OB' and party_nm = 'OPENING STOCK'
update stkl_vw_item set entry_ty = 'OS' from stkl_vw_item a,stkl_vw_main b where a.tran_cd = b.tran_cd and b.entry_ty = 'OS'
update stkl_vw_main set u_arrears = 'OTHERS ARREARS OF DUTY' where u_arrears = 'ARREARS OTHERS'
update stkl_vw_main set u_arrears = 'ARREARS OF DUTY UNDER RULE 8' where u_arrears = 'ARREARS RULE 8'
update stkl_vw_main set u_arrears = 'OTHERS INTEREST PAYMENTS' where u_arrears = 'INTEREST OTHERS'
update stkl_vw_main set u_arrears = 'INTEREST PAYMENT UNDER RULE 8' where u_arrears = 'INTEREST RULE 8'
update stkl_vw_main set u_arrears = 'Misc. payments' where u_arrears = 'MISC.'
update ex_vw_acdet set entry_ty = 'OS' from ex_vw_acdet a,stkl_vw_main b where a.tran_cd = b.tran_cd and b.entry_ty = 'OS'
select * into #temp1 from lcode where entry_ty = 'OB'
update #temp1 set entry_ty = 'OS'
insert into lcode select * from #temp1

--delete existing link server
if exists(select * from sys.servers where name = @LNKSVRNM)
begin
execute sp_dropserver @LNKSVRNM,'droplogins'
end


--NOTES
/*
space(10) as batchno, {} as mfgdt,{} as expdt FROM litem
select * from lac_det where typ = 'EXCISE'
ITGRID
*/