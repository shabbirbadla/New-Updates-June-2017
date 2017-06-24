Create PROCEDURE [dbo].[USP_VU8FORM2] 
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

if exists(select * from sysobjects where name = 'WAREHOUSE')
begin
drop table WAREHOUSE
end

if exists(select * from sysobjects where name = 'TRADEMAIN')
begin
drop table TRADEMAIN
end

if exists(select * from sysobjects where name = 'TRADEITEM')
begin
drop table TRADEITEM
end

if exists(select * from sysobjects where name = 'LITEMALL')
begin
drop table LITEMALL
end

if exists(select * from sysobjects where name = 'SPDIFF')
begin
drop table SPDIFF
end

if exists(select * from sysobjects where name = 'MANU_DET')
begin
drop table MANU_DET
end

--create link server
execute sp_addlinkedserver @LNKSVRNM,'eFiling','VFPOLEDB',@VU8DATAPATH
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
set @sqlcommand = 'SELECT Ac_name as coll,
	Ac_name as range,
	Ac_name as division,
	* into SHIPTO FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as It_Code,Recno() as ItGrid,
	a.*,Rateunit as ExRateUnit
	from it_mast a'
set @sqlcommand = 'SELECT * into IT_MAST FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select  dcw_nm as ware_nm,
	* from ldcw '
set @sqlcommand = 'SELECT * into WAREHOUSE FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''') where dcw_cd = ''W'' '
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'SELECT Recno() as Tran_cd, date, entry_ty, doc_no,
	inv_no,rule, ettype, u_pinvno, u_pinvdt,u_sinfo,U_Osale,u_choice,
	cast(0 as int) as Ac_id,cast(0 as int) as cons_id, Party_nm,
	cast(0 as int) as manuac_id,cast(0 as int) as manusac_id, ManuName,
	cast(0 as int) as Compid,
	cast(0 as int) as Sac_id,cast(0 as int) as Scons_id
	from lmain'
set @sqlcommand = 'SELECT * into TRADEMAIN FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select cast(0 as int) as  Tran_cd, entry_ty, date, doc_no,  itserial,
	cast(0 as int) as  Ac_id, cast(0 as int) as  It_code, Item,
	cast(0 as int) as manuac_id,cast(0 as int) as manusac_id, ManuName,
	cast(0 as int) as Compid,inv_no, pmkey,
	tariff, ware_nm, rgpage, 
	id_mark, qty, billqty, balqty,
	U_lqty, mtduty, u_basduty, examt, excbal, billexamt, u_cessper, u_cessamt, u_cessbal, 
	billcesamt, u_cvdper, u_cvdamt, u_cvdbal, billcvdamt, U_HCESSPER, U_HCESAMT, U_HCESSBAL, BILLHCSAMT, 
	u_cvdpass as cvdpass
	from litem'
set @sqlcommand = 'SELECT * into TRADEITEM FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select  cast(0 as int) as  Tran_cd,cast(0 as int) as  PTran_cd,
	itserial as acserial,
	pentry_ty as rentry_ty,cast(0 as int) as  RTran_cd,pdate as rdate,pitserial as ritserial,
	* from Litemall'
set @sqlcommand = 'SELECT * into LITEMALL FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select  cast(0 as int) as  Tran_cd,cast(0 as int) as  PTran_cd,
	* from SpDiff'
set @sqlcommand = 'SELECT * into SPDIFF FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

set @sqlstr = 'select  cast(0 as int) as  Tran_cd,
	cast(0 as int) as  ManuAc_id,cast(0 as int) as  ManuSAc_id,
	* from manu_det'
set @sqlcommand = 'SELECT * into MANU_DET FROM OPENQUERY('+@LNKSVRNM+','''+@sqlstr+''')'
execute SP_EXECUTESQL @sqlcommand

--Updating data as per VU9 Requirements
set @sqlcommand = 'update lcode set entry_ty = ''AR'' where entry_ty = ''P '''
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update lcode set entry_ty = ''DC'' where entry_ty = ''S '''
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update lcode set entry_ty = ''GT'' where entry_ty = ''SR'''
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update shipto set ac_id = b.ac_id from shipto a,ac_mast b where a.ac_id = b.ac_id '
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update trademain set entry_ty = b.entry_ty from trademain a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update tradeitem set entry_ty = b.entry_ty from tradeitem a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update litemall set entry_ty = b.entry_ty from litemall a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update litemall set pentry_ty = b.entry_ty from litemall a,lcode b where a.pentry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update spdiff set entry_ty = b.entry_ty from spdiff a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update spdiff set pentry_ty = b.entry_ty from spdiff a,lcode b where a.pentry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update manu_det set entry_ty = b.entry_ty from manu_det a,lcode b where a.entry_ty = b.cd '
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update tradeitem set Tran_cd = b.Tran_cd from tradeitem a,trademain b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update litemall set Tran_cd = b.Tran_cd from litemall a,trademain b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update litemall set pTran_cd = b.Tran_cd from litemall a,trademain b where a.pentry_ty = b.entry_ty and a.pdate = b.date and a.pdoc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update spdiff set Tran_cd = b.Tran_cd from spdiff a,trademain b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update spdiff set pTran_cd = b.Tran_cd from spdiff a,trademain b where a.pentry_ty = b.entry_ty and a.pdate = b.date and a.pdoc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update manu_det set Tran_cd = b.Tran_cd from manu_det a,trademain b where a.entry_ty = b.entry_ty and a.date = b.date and a.doc_no = b.doc_no '
execute SP_EXECUTESQL @sqlcommand

set @sqlcommand = 'update tradeitem set It_code = b.It_code from tradeitem a,it_mast b where a.item = b.it_name'
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update trademain set Ac_id = b.Ac_id,Cons_id = b.Ac_id from trademain a,ac_mast b where a.Party_nm = b.Ac_name'
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update trademain set ManuAc_id = b.Ac_id from trademain a,ac_mast b where a.ManuName = b.Ac_name'
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update tradeitem set ManuAc_id = b.Ac_id from tradeitem a,ac_mast b where a.ManuName = b.Ac_name'
execute SP_EXECUTESQL @sqlcommand
set @sqlcommand = 'update manu_det set ManuAc_id = b.Ac_id from manu_det a,ac_mast b where a.Party_nm = b.Ac_name'
execute SP_EXECUTESQL @sqlcommand

update tradeitem set tariff = b.chap from tradeitem a,it_mast b where a.it_code = b.it_code and a.tariff = ''
--update ac_mast set vend_type = 'Manufacturer' where vend_type = 'MFGR.'


--delete existing link server
if exists(select * from sys.servers where name = @LNKSVRNM)
begin
execute sp_dropserver @LNKSVRNM,'droplogins'
end
