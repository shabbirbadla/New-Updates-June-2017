if exists (select [name] from sysobjects where [name]='USP_ENT_BOMDET_IP' AND XTYPE='P')
BEGIN 
DROP PROCEDURE USP_ENT_BOMDET_IP
END
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful In Work Order Entry uefrm_bomdetails_ip.scx.
-- Modification Date/By/Reason: 13/06/2009 Rupesh Prajapati. Modified for Work Order Estimated Qty. . Modified for msqtym,msqty fields for Balance
-- Modification Date/By/Reason: 11/08/2009 Rupesh Prajapati. Modified for Posqty with mpending qty if mpending qty is less.
-- Modification Date/By/Reason: 07/10/2009 Rupesh Prajapati. Modified for BalQty For DC_NO=''
-- Modification Date/By/Reason: 19/04/2010 Sachin N. S. Modified for L1S-238 for Third Party Stock consideration
-- Modification Date/By/Reason: 03/08/2010 Shrikant S. Modified for TKT-3110 
-- Modification Date/By/Reason: 06/09/2010 Shrikant S. Modified for TKT-3787 
-- Modification Date/By/Reason: 06/03/2011 Sandeep S. Modified for TKT-6576
-- Modification Date/By/Reason: 18/04/2011 Amrendra Modified for TKT-6322
-- Modification Date/By/Reason: 28/03/2012 Amrendra Modified this for New QC
-- Modification Date/By/Reason: 15/11/2012 SATISH PAL for bug-3335 dt.
-- Modification		          : Birendra: Bug-4426 on 21/12/2012	
-- Modification Date/By/Reason: 31/07/2012 Birendra Modified for Bug-4543(ProcessWise production)
-- Modification Date/By/Reason:	SATISH PAL Bug-13916 DT 23/05/2013	
-- Modification Date/By/Reason: Nilesh Yadav For Bug 24314 on 27/10/14			
-- Modification Date/By/Reason: Pankaj B. on 08-08-2014 for Bug-23411 (QC Module is checking the item which is not defied in the QC Module)		(for 11.0.8 on 12/12/2014 by Shrikant)
-- Modification Date/By/Reason: Kishor A. On 04/07/2015 for Bug-26321 
-- Remark: 			
-- Remark: 
-- =============================================
create PROCEDURE [dbo].[USP_ENT_BOMDET_IP]
@ENTRY_TY  VARCHAR(2),@TRAN_CD  INT,@SDATE SMALLDATETIME,@RULE VARCHAR(20),@INV_SR VARCHAR(20),@CATE VARCHAR(20),@DEPT VARCHAR(20),@ProdName VARCHAR(10)
,@proc_Id varchar(10) --Birendra Bug-4543 on 31/07/2012
AS
DECLARE @SQLCOMMAND NVARCHAR(4000)
Declare @mpqty numeric(18,4) 
SELECT @INV_SR ='',@CATE='',@DEPT =''
select wi.inv_no,wi.date,it1.it_name
,wi.it_code,item=it.it_name,wi.entry_ty,wi.tran_cd,wi.itserial,wi.qty,wi.bomid,wi.bomlevel
,d.rmitemid,rmitem=it1.it_name
--,rqty=wi.qty,--Commented By Kishor A. for Bug-26321
,rqty=wi.qty/wi.qty --Added By Kishor A. for Bug-26321
,reqty=wi.qty
,iqty=wi.qty,aiqty=wi.qty,balqty=wi.qty,sqtym=wi.qty,sqty=wi.qty
,sqtyLabr=wi.qty							-- Added By Sachin N. S. on 19/04/2010 for L1S-238 
,ssqty=wi.qty,orgqty=wi.qty --,orgqty=qty,qty=wi.qty
,it1.tlissperp,it1.tlissperm,tlissqtyp=wi.qty,tlissqtym=wi.qty
,posqty=wi.qty,wkqty=wi.qty
,msqtym=wi.qty,msqty=wi.qty
,msqtyLabr=wi.qty							-- Added By Sachin N. S. on 19/04/2010 for L1S-238 
,mclear=wi.qty,mpending=wi.qty
,wi.trm_qty									---added dt.15/11/2012 SATISH PAL for bug-3335 dt.
,d.isbom --Birendra: Bug-4426 on 13/06/2012
,it.rate --Birendra: Bug-4543 on 27/10/2012
,d.rmqty,h.fgqty  ----ADDED BY SATISH PAL Bug-13916 DT 23/05/2013
into #bomdet1
from item wi  
inner join it_mast it on (wi.it_code=it.it_code)
inner join bomhead h on (wi.bomid=h.bomid and wi.bomlevel=h.bomlevel)
inner join bomdet d on (h.bomid=d.bomid and h.bomlevel=d.bomlevel)
inner join it_mast it1 on (d.rmitemid=it1.it_code )
where 1=2 order by wi.Entry_ty,wi.Tran_cd,wi.Itserial

SET @SQLCOMMAND ='INSERT INTO #bomdet1 select wi.inv_no,wi.date,it1.it_name'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',wi.it_code,item=it.it_name,wi.entry_ty,wi.tran_cd,wi.itserial,wi.qty,wi.bomid,wi.bomlevel'
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',d.rmitemid,rmitem=it1.it_name,rqty=h.fgqty*d.rmqty,reqty=(wi.qty*d.rmqty)/h.fgqty ' Commented by sandeep S. 06/03/2011 for TKT-3110
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',d.rmitemid,rmitem=it1.it_name,rqty=d.rmqty/h.fgqty,reqty=(wi.qty*d.rmqty)/h.fgqty ' -- Added by sandeep S. on 06/03/2011 for TKT-6576
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',iqty=wi.qty,aiqty=wi.qty,balqty=wi.qty,sqtym=wi.qty,sqty=wi.qty'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtyLabr=wi.qty'		-- Added By Sachin N. S. on 19/04/2010 for L1S-238 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',ssqty=wi.qty,orgqty=wi.qty'--,qty=wi.qty
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',it1.tlissperp,it1.tlissperm,tlissqtyp=0,tlissqtym=0,wkqty=0,posqty=0,msqtym=0,msqty=0'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtyLabr=wi.qty'		-- Added By Sachin N. S. on 19/04/2010 for L1S-238 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',mclear=0,mpending=0,wi.trm_qty'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',d.isbom' --Birendra: Bug-4426 on 13/06/2012
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',it.rate' --Birendra: Bug-4543 on 27/10/2012
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',d.rmqty,h.fgqty' ----ADDED BY SATISH PAL Bug-13916  DT 17/05/2013
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'from item wi  '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'inner join it_mast it on (wi.it_code=it.it_code)'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'inner join bomhead h on (wi.bomid=h.bomid and wi.bomlevel=h.bomlevel)'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'inner join bomdet d on (h.bomid=d.bomid and h.bomlevel=d.bomlevel)'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'inner join it_mast it1 on (d.rmitemid=it1.it_code )'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'where wi.entry_ty='+CHAR(39)+'WK'+CHAR(39)
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'AND WI.DATE< ='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)
--Birendra : Bug-4543 on 31/07/2012 :Start:
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'and wi.proc_ID='+CHAR(39)+ltrim(rtrim(@proc_id))+CHAR(39)
--Birendra : Bug-4543 on 31/07/2012 :End:
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'order by wi.Entry_ty,wi.Tran_cd,wi.Itserial'
print @SQLCOMMAND
EXECUTE SP_EXECUTESQL  @SQLCOMMAND

-- update #bomdet1 set iqty=0,balqty=0,sqty=0,sqtym=0,aiqty=0,ssqty=0 ,qty=0 --- Commented by Amrendra for TKT -6322 on 18/04/2011
update #bomdet1 set iqty=0,balqty=0,sqty=0,sqtym=0,aiqty=0,ssqty=0 ,qty=0,sqtyLabr=0  ---Added by Amrendra for TKT -6322 on 18/04/2011

--select i.it_code,sqtym=sum(i.qty),sqty=sum(i.qty),sqtyLabr=sum(i.qty) into #it_bal  from STKL_VW_ITEM i WHERE 1=2 group by i.it_code
--Birendra : Bug-4543 on 27/10/2012: modified above line as per below:
select i.it_code,sqtym=sum(i.qty),sqty=sum(i.qty),sqtyLabr=sum(i.qty),rate=max(i.Rate) into #it_bal  from STKL_VW_ITEM i WHERE 1=2 group by i.it_code
SET @SQLCOMMAND ='INSERT INTO #it_bal select i.it_code'   
-- Added By Amrendra on 28/03/2012 for NEW QC
if ltrim(rtrim(@ProdName))='QC'
begin   
--Commented by nilesh yadav for bug 24314 on 27/10/14
		--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtym=sum(case when m.[rule] in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qcaceptqty else -i.qcaceptqty end,0)) else 0 end)'		--Changed by Shrikant S. on 06/09/2010 For TKT-3787
		--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqty=sum(case when m.[rule] not in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'ANNEXURE V'+char(39)++','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qcaceptqty else -i.qcaceptqty end,0)) else 0 end)'	--Changed by Shrikant S. on 06/09/2010 For TKT-3787
		--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtyLabr=sum(case when m.[rule] in ('+char(39)+'ANNEXURE V'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qcaceptqty else -i.qcaceptqty end,0)) else 0 end)'	-- Added By Sachin N. S. on 19/04/2010 for L1S-238
--Commented by nilesh yadav for bug 24314 on 27/10/14

---- Added by nilesh yadav for Bug 24314 on 27/10/14		-- Commented for 11.0.8 on 12/12/2014 by Shrikant	-- Start
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtym=sum(case when m.[rule] in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qcaceptqty else -i.qty end,0)) else 0 end)'		
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqty=sum(case when m.[rule] not in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'ANNEXURE V'+char(39)++','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qcaceptqty else -i.qty end,0)) else 0 end)'
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtyLabr=sum(case when m.[rule] in ('+char(39)+'ANNEXURE V'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qcaceptqty else -i.qty end,0)) else 0 end)'	
----Added by nilesh yadav for Bug 24314 on 27/10/14			-- Commented for 11.0.8 on 12/12/2014 by Shrikant	-- End

-- Added for 11.0.8 on 12/12/2014 by Shrikant	-- Start	
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtym=sum(case when m.[rule] in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then CASE WHEN IT.QCPROCESS=1 THEN i.qcaceptqty ELSE I.QTY END else CASE WHEN IT.QCPROCESS=1 THEN -i.qcaceptqty ELSE -QTY END end,0)) else 0 end)'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqty=sum(case when m.[rule] not in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'ANNEXURE V'+char(39)++','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then CASE WHEN IT.QCPROCESS=1 THEN i.qcaceptqty ELSE I.QTY END else CASE WHEN IT.QCPROCESS=1 THEN -i.qcaceptqty ELSE -QTY END end,0)) else 0 end)'	
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtyLabr=sum(case when m.[rule] in ('+char(39)+'ANNEXURE V'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then CASE WHEN IT.QCPROCESS=1 THEN i.qcaceptqty ELSE I.QTY END else CASE WHEN IT.QCPROCESS=1 THEN -i.qcaceptqty ELSE -QTY END end,0)) else 0 end)'	
-- Added for 11.0.8 on 12/12/2014 by Shrikant	-- End


end
else
begin
-- Added By Amrendra on 28/03/2012 for NEW QC
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtym=sum(case when m.[rule] in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qty else -i.qty end,0))-i.qcrejqty else 0 end)'		--Changed by Shrikant S. on 06/09/2010 For TKT-3787
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqty=sum(case when m.[rule] not in ('+char(39)+'MODVATABLE'+char(39)+','+char(39)+'ANNEXURE IV'+char(39)+','+char(39)+'ANNEXURE V'+char(39)++','+char(39)+'CAPTIVE USE'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qty else -i.qty end,0)) else 0 end)'	--Changed by Shrikant S. on 06/09/2010 For TKT-3787
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',sqtyLabr=sum(case when m.[rule] in ('+char(39)+'ANNEXURE V'+char(39)+') then (isnull(case when pmkey='+CHAR(39)+'+'+CHAR(39)+' then i.qty else -i.qty end,0)) else 0 end)'	-- Added By Sachin N. S. on 19/04/2010 for L1S-238
end
--Birendra : Bug-4543 on 27/10/2012: Start:
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+',Rate=max(i.Rate)'
--Birendra : Bug-4543 on 27/10/2012: Start:
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'from STKL_VW_ITEM i' 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'INNER JOIN STKL_VW_MAIN M ON (M.TRAN_CD=I.TRAN_CD AND M.ENTRY_TY=I.ENTRY_TY)'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'LEFT OUTER JOIN it_advance_setting IT ON (I.IT_CODE=IT.IT_CODE)'		-- Added for 11.0.8 on 12/12/2014 by Shrikant	
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'WHERE NOT (I.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND I.TRAN_CD='+CAST(@TRAN_CD AS VARCHAR)+')'
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+'and i.dc_no='''''
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+' group by i.it_code'
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

--update a set a.sqtym=b.sqtym,a.sqty=b.sqty from  #bomdet1 a inner join #it_bal b on (a.rmitemid=b.it_code) -- Commented by Shrikant S. on 03/08/2010 for TKT-3110
--update a set a.sqtym=b.sqtym,a.sqty=b.sqty,a.sqtyLabr=b.sqtyLabr from  #bomdet1 a inner join #it_bal b on (a.rmitemid=b.it_code)	-- Added by Shrikant S. on 03/08/2010 for TKT-3110
--Birendra : Bug-4543 on 27/10/2012: modified above line as per below:
update a set a.sqtym=b.sqtym,a.sqty=b.sqty,a.sqtyLabr=b.sqtyLabr,a.rate=b.rate from  #bomdet1 a inner join #it_bal b on (a.rmitemid=b.it_code)	-- Added by Shrikant S. on 03/08/2010 for TKT-3110

--select b.it_code,b.bomid,b.bomlevel,iqty=sum(isnull(b.qty,0)),aentry_ty,atran_cd,aitserial into #PROJECTITREF from PROJECTITREF b  WHERE NOT (ENTRY_TY=@ENTRY_TY AND TRAN_CD=@TRAN_CD ) AND ENTRY_TY='IP' group by b.it_code,b.bomid,b.bomlevel,aentry_ty,atran_cd,aitserial

--Birendra : Bug-4543 on 21/09/2012 :Start:
select b.it_code,b.bomid,b.bomlevel,iqty=sum(isnull(b.qty,0)),aentry_ty,atran_cd,aitserial into #PROJECTITREF from PROJECTITREF b  WHERE NOT (ENTRY_TY=@ENTRY_TY AND TRAN_CD=@TRAN_CD ) AND ENTRY_TY in('IP','WI') group by b.it_code,b.bomid,b.bomlevel,aentry_ty,atran_cd,aitserial
--Birendra : Bug-4543 on 21/09/2012 :End:

update a set a.iqty=b.iqty from  #bomdet1 a inner join #PROJECTITREF b on (a.bomid=b.bomid and a.bomlevel=b.bomlevel and a.entry_ty=b.aentry_ty and a.tran_cd=b.atran_cd and a.itserial=b.aitserial and a.rmitemid=b.it_code and a.rmitemid=b.it_code ) --

--update #bomdet1 set balqty=reqty-iqty,qty=0 --COMMNETD BY dt.15/11/2012 SATISH PAL for bug-3335 dt.
update #bomdet1 set balqty=reqty-iqty-(trm_qty*rqty),qty=0 -----added dt.15/11/2012 SATISH PAL for bug-3335 dt.

update #bomdet1 set tlissperp=(case when isnull(tlissperp,0)=0 then 0 else tlissperp end),tlissperm=(case when isnull(tlissperm,0)=0 then 0 else tlissperm end)
update #bomdet1 set tlissqtyp=(reqty*tlissperp)/100,tlissqtym=((reqty*tlissperm)/100)
delete from #bomdet1 where balqty-tlissqtym<=0

--update #bomdet1 set msqtym=sqtym,msqty=sqty			-- Commented by Shrikant S. on 03/08/2010 for TKT-3110
update #bomdet1 set msqtym=sqtym,msqty=sqty,msqtyLabr=sqtyLabr				-- Added by Shrikant S. on 03/08/2010 for TKT-3110

update #bomdet1 set posqty=(sqtym*orgqty) /reqty
update #bomdet1 set posqty=0 where isnull(posqty,0)<0
select entry_ty,tran_cd,posqty=min(posqty) into #minqty from #bomdet1 group by entry_ty,tran_cd
update a set a.posqty=b.posqty from #bomdet1 a inner join #minqty b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)

declare @fldlist varchar(4000),@fld_nm varchar(20)
set @fldlist=' '
declare cur_bomdetip cursor for select fld_nm from lother where e_code='WK' and att_file=0 order by serial
open cur_bomdetip
fetch next from cur_bomdetip into @fld_nm
while (@@fetch_status=0)
begin
	set @fldlist=rtrim(@fldlist)+',b.'+rtrim(@fld_nm)
	fetch next from cur_bomdetip into @fld_nm
end
close cur_bomdetip
deallocate cur_bomdetip

print @fldlist

update #bomdet1 set mpending=(orgqty*balqty)/reqty

select @mpqty=max(mpending) from #bomdet1

update #bomdet1 set mpending=isnull(@mpqty,0) 

update #bomdet1 set posqty=mpending where mpending<posqty

set @sqlcommand='select a.*'+@fldlist+' from #bomdet1 a inner join item b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd and a.itserial=b.itserial) order by a.bomid'
print @sqlcommand
execute sp_executesql @sqlcommand



