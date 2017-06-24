IF EXISTS(Select [Name] from Sysobjects where xtype = 'P' and [name] = 'USP_REP_KA_VPTREG')
BEGIN
	DROP PROCEDURE USP_REP_KA_VPTREG
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author	  :	Hetal L Patel
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate Karanataka Vat Purchase Register Report.
-- Modify date: 16/05/2007
-- Modified By: Hetal L Patel
-- Modify date: 16/02/2010 //Changes done for Ledger Summary i.e. Part 6 of this report
-- Modify date: 23/05/2015 GAURAV R. TANNA - Bug : 26168
-- Remark	  :
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_KA_VPTREG]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS


SET QUOTED_IDENTIFIER OFF
DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)
DECLARE @FDATE VARCHAR(15)
SELECT @FDATE=CASE WHEN DBDATE=1 THEN 'DATE' ELSE 'U_CLDT' END FROM MANUFACT
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='STMAIN',@VITFILE='STITEM',@VACFILE=' '
,@VDTFLD =@FDATE
,@VLYN=@LYN
,@VEXPARA=NULL
,@VFCON =@FCON OUTPUT
DECLARE @SQLCOMMAND NVARCHAR(4000),@VCOND NVARCHAR(1000)

SELECT entry_ty,FLD_NM=SPACE(1000) INTO #DCMAST FROM DCMAST WHERE 1=2

declare @dcentry_ty varchar(2),@dcfld_nm varchar(1000)
declare @mdcentry_ty varchar(2),@mdcfld_nm varchar(1000),@v1 int

--select entry_ty,fld_nm from dcmast where  entry_ty in ('PT','AR','ST') AND ATT_FILE=0 
--AND (BEF_AFT=1 OR CODE IN ('D','T','E','A')) 
--order by entry_ty,corder

--select top 1 entry_ty into #entryty from stmaIn where 1=2
select distinct entry_ty=case when ext_vou=1 then bcode_nm else entry_ty end into #entryty from lcode where  entry_ty in ('ST','PT','AR')
declare  cur_dcmast cursor for 
select entry_ty,fld_nm=rtrim(fld_nm) from dcmast where  entry_ty in ('PT','AR','ST') AND ATT_FILE=0 
AND (BEF_AFT=1 OR CODE IN ('D','T','E','A')) 
order by entry_ty,corder
open cur_dcmast
fetch next from cur_dcmast into @dcentry_ty,@dcfld_nm
set @mdcentry_ty=@dcentry_ty
set @mdcfld_nm=' '
set @v1=0
while (@@fetch_status=0)
begin
	if 	(@mdcentry_ty=@dcentry_ty)
	begin
		set @v1=@v1+1
		set @mdcfld_nm=@mdcfld_nm+'+'+@dcfld_nm	
	end
	else
	begin
		set @v1=0
		insert into #dcmast (entry_ty,fld_nm) values (@mdcentry_ty,@mdcfld_nm)
		set @mdcentry_ty=@dcentry_ty
		set @mdcfld_nm='+'+@dcfld_nm
	end
	fetch next from cur_dcmast into @dcentry_ty,@dcfld_nm
end
close cur_dcmast
deallocate cur_dcmast
if(@v1>0)
begin
----	print @v1	
	insert into #dcmast (entry_ty,fld_nm) values (@mdcentry_ty,@mdcfld_nm)
end
--select * from #dcmast
----print replace('abc','a','1')
update #dcmast set fld_nm=replace(fld_nm,'+','+i.')
--select tax_name,level1,amtexpr into #stax_mas from stax_mas 



select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date
,ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)
,taxabl_amt=i.examt+i.u_cessamt+i.u_hcesamt,i.taxamt
,rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd
,st.level1,dcm.fld_nm
,mqty=i.qty,aqty=ref.rqty,part=1
,AC.MAILNAME --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
into #kv_purreg1
from ptmain m
inner join ptitem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)
left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial)
left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial)
left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd)
left join stax_mas st on (i.tax_name=st.tax_name)
left join #dcmast dcm on (m.entry_ty=dcm.entry_ty)
inner join ac_mast ac on (m.ac_id=ac.ac_id)
where 
--case when isnull(st.amtexpr,space(1))<>space(1),st.amtexpr,0)
--m.entry_ty in ('PT','AR') OR (M.ENTRY_TY='ST' and m.u_imporm='Purchase Return') and 
1=2

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'PTMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		Set @MultiCo = 'YES'

		select @dcfld_nm=fld_nm from #dcmast where entry_ty='PT'
		set @sqlcommand='insert into #kv_purreg1 select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',taxabl_amt=round((i.qty*i.rate),0)'+@dcfld_nm+',i.taxamt'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',st.level1,dcm.fld_nm'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',mqty=i.qty,aqty=ref.rqty,part=1'
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+',AC.MAILNAME' --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		set @sqlcommand=rtrim(@sqlcommand)+' '+'from ptmain m'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ptitem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd and m.dbname = i.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial and I.dbname = ref.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial and i.dbname = i1.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd and i1.dbname = m1.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id and m.dbname = ac.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stax_mas st on (i.tax_name=st.tax_name And st.entry_ty = m.entry_ty and i.dbname = st.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join #dcmast dcm on (m.entry_ty=dcm.entry_ty and m.dbname = dcm.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'where  m.taxamt<>0 and m.entry_ty in ('+'''PT'''+')'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'and m.date between '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)
--		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		select @dcfld_nm=fld_nm from #dcmast where entry_ty='ST'
		set @sqlcommand='insert into #kv_purreg1 select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',taxabl_amt=(round((i.qty*i.rate),0))*(-1)'+@dcfld_nm+',i.taxamt*(-1)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',st.level1,dcm.fld_nm'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',mqty=i.qty,aqty=ref.rqty,part=1'
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+',AC.MAILNAME' --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		set @sqlcommand=rtrim(@sqlcommand)+' '+'from stmain m'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join stitem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd and m.dbname = i.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial and i.dbname = ref.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial and i.dbname = i1.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd and i1.dbname = m1.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stax_mas st on (i.tax_name=st.tax_name And st.entry_ty = m.entry_ty and st.dbname = m.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join #dcmast dcm on (m.entry_ty=dcm.entry_ty and m.dbname = dcm.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id and m.dbname = ac.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'where  m.taxamt<>0 and m.entry_ty in ('+'''ST'''+') and m.u_imporm='+'''Purchase Return'''
		set @sqlcommand=rtrim(@sqlcommand)+' '+'and m.date between '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)
----		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		select @dcfld_nm=fld_nm from #dcmast where entry_ty='AR'
		set @sqlcommand='insert into #kv_purreg1 select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',taxabl_amt=round((i.qty*i.rate),0)'+@dcfld_nm+',i.taxamt'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',st.level1,dcm.fld_nm'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',mqty=i.qty,aqty=ref.rqty,part=3'
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+',AC.MAILNAME' --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		set @sqlcommand=rtrim(@sqlcommand)+' '+'from armain m'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join aritem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd and m.dbname = i.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial and i.dbname = ref.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial and i.dbname = i1.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd and i1.dbname = m1.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stax_mas st on (i.tax_name=st.tax_name And st.entry_ty = m.entry_ty and st.dbname = m.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join #dcmast dcm on (m.entry_ty=dcm.entry_ty and m.dbname = dcm.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id and m.dbname = ac.dbname)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'where  (i.qty-isnull(ref.rqty,0)>0)  and m.entry_ty in ('+'''AR'''+')'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'and m.date between '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)
--		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND



		select part=(case when part=3 then 5 else 4 end)
		,level1--=(case when part=3 then 0 else level1 end)
		,taxamt=sum(taxamt) 
		into #kv_purreg2
		from #kv_purreg1 group by (case when part=3 then 5 else 4 end)
		,level1--(case when part=3 then 0 else level1 end)

		insert into #kv_purreg1 (part,level1,taxamt,tran_cd,ac_name) select part,level1,taxamt,0,' ' from #kv_purreg2 

		-------->6
		--SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME3,2,CHARINDEX('"',SUBSTRING(AC_NAME3,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"') AND AC_NAME3 NOT IN ('"PURCHASES"') AND ISNULL(AC_NAME1,'')<>'' AND ISNULL(AC_NAME3,'')<>''
		--INSERT INTO #VATAC_MAST SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1)  FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"') AND AC_NAME3 NOT IN ('"PURCHASES"') AND ISNULL(AC_NAME1,'')<>'' AND ISNULL(AC_NAME3,'')<>''
		SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
		INSERT INTO #VATAC_MAST SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''

		SELECT distinct
		AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY
		,MN.L_YN
		,AC_MAST.AC_ID,AC_MAST.AC_NAME,#VATAC_MAST.level1
		,AC_MAST.MAILNAME  --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		into #AC_BAL1
		FROM LAC_VW AC
		INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)
		INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) 
		inner join #VATAC_MAST ON (AC_MAST.AC_NAME=#VATAC_MAST.AC_NAME)
		where #VATAC_MAST.level1 >0 
		and Ac.Date between @Sdate And @Edate 
		And (Ac.Entry_ty In('PT') or (mn.u_imporm='Purchase Return' And Ac.Entry_ty = 'ST'))


		DELETE FROM #AC_BAL1 WHERE 
		DATE < (SELECT TOP 1 DATE FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN)
		AND AC_NAME IN (SELECT AC_NAME FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN GROUP BY AC_NAME) 


		SELECT AC_NAME,AC_ID,LBAL=SUM(CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END),LEVEL1
		,MAILNAME  --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		INTO #AC_BAL
		FROM #AC_BAL1
		GROUP BY AC_NAME,AC_ID,LEVEL1
		ORDER BY AC_NAME,AC_ID

		insert into #kv_purreg1 (part,level1,taxamt,tran_cd,ac_name,MAILNAME) select 6,level1,taxamt=lbal,0,ac_name,MAILNAME from #AC_BAL
		---<----6

		update #kv_purreg1 set qty=isnull(qty,0),taxabl_amt=isnull(taxabl_amt,0),level1=isnull(level1,0)

		select * from #kv_purreg1 
		order by part,level1
		,(case when entry_ty='pt' then 'a' else (case when entry_ty='st' then 'b' else 'c' end) end) 
		,tran_cd
	End
Else
	Begin	------Fetch Records from Single Co. Data
		Set @MultiCo = 'NO'
		select @dcfld_nm=fld_nm from #dcmast where entry_ty='PT'
		set @sqlcommand='insert into #kv_purreg1 select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',taxabl_amt=round((i.qty*i.rate),0)'+@dcfld_nm+',i.taxamt'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',st.level1,dcm.fld_nm'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',mqty=i.qty,aqty=ref.rqty,part=1'
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+',AC.MAILNAME' --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		set @sqlcommand=rtrim(@sqlcommand)+' '+'from ptmain m'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ptitem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stax_mas st on (i.tax_name=st.tax_name And st.entry_ty = m.entry_ty)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join #dcmast dcm on (m.entry_ty=dcm.entry_ty)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'where  m.taxamt<>0 and m.entry_ty in ('+'''PT'''+')'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'and m.date between '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)
--		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		select @dcfld_nm=fld_nm from #dcmast where entry_ty='ST'
		set @sqlcommand='insert into #kv_purreg1 select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',taxabl_amt=(round((i.qty*i.rate),0))*(-1)'+@dcfld_nm+',i.taxamt*(-1)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',st.level1,dcm.fld_nm'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',mqty=i.qty,aqty=ref.rqty,part=1'
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+',AC.MAILNAME' --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		set @sqlcommand=rtrim(@sqlcommand)+' '+'from stmain m'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join stitem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stax_mas st on (i.tax_name=st.tax_name And st.entry_ty = m.entry_ty)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join #dcmast dcm on (m.entry_ty=dcm.entry_ty)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'where  m.taxamt<>0 and m.entry_ty in ('+'''ST'''+') and m.u_imporm='+'''Purchase Return'''
		set @sqlcommand=rtrim(@sqlcommand)+' '+'and m.date between '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)
--		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		select @dcfld_nm=fld_nm from #dcmast where entry_ty='AR'
		set @sqlcommand='insert into #kv_purreg1 select m.entry_ty,m.tran_cd,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,dcinv_no=m1.inv_no,dcdate=m1.date'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.ac_name,ac.s_tax,qty=i.qty-isnull(ref.rqty,0)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',taxabl_amt=round((i.qty*i.rate),0)'+@dcfld_nm+',i.taxamt'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',rentry_ty=ref.entry_ty,rtran_cd=ref.tran_cd'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',st.level1,dcm.fld_nm'
		set @sqlcommand=rtrim(@sqlcommand)+' '+',mqty=i.qty,aqty=ref.rqty,part=3'
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+',AC.MAILNAME' --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		set @sqlcommand=rtrim(@sqlcommand)+' '+'from armain m'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join aritem i on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_itref ref on (i.entry_ty=ref.rentry_ty and i.tran_cd=ref.itref_tran and i.itserial=ref.ritserial)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_item i1 on (i.entry_ty=i1.entry_ty and i.tran_cd=i1.tran_cd and i.itserial=i1.itserial)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stkl_vw_main m1 on (i1.entry_ty=m1.entry_ty and i1.tran_cd=m1.tran_cd)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join stax_mas st on (i.tax_name=st.tax_name And st.entry_ty = m.entry_ty)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'left join #dcmast dcm on (m.entry_ty=dcm.entry_ty)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'where  (i.qty-isnull(ref.rqty,0)>0)  and m.entry_ty in ('+'''AR'''+')'
		set @sqlcommand=rtrim(@sqlcommand)+' '+'and m.date between '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)
----		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND



		select part=(case when part=3 then 5 else 4 end)
		,level1--=(case when part=3 then 0 else level1 end)
		,taxamt=sum(taxamt) 
		into #kv_purreg3
		from #kv_purreg1 group by (case when part=3 then 5 else 4 end)
		,level1--(case when part=3 then 0 else level1 end)

		insert into #kv_purreg1 (part,level1,taxamt,tran_cd,ac_name) select part,level1,taxamt,0,' ' from #kv_purreg3 

		-------->6
		--SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME3,2,CHARINDEX('"',SUBSTRING(AC_NAME3,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"') AND AC_NAME3 NOT IN ('"PURCHASES"') AND ISNULL(AC_NAME1,'')<>'' AND ISNULL(AC_NAME3,'')<>''
		--INSERT INTO #VATAC_MAST SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1)  FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"') AND AC_NAME3 NOT IN ('"PURCHASES"') AND ISNULL(AC_NAME1,'')<>'' AND ISNULL(AC_NAME3,'')<>''
		SELECT DISTINCT TAX_NAME,level1,AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST1 FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
		--INSERT INTO #VATAC_MAST1 SELECT DISTINCT level1,AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''

		SELECT distinct
		AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,MI.TAXAMT AS AMOUNT,AC.AMT_TY
		,MN.L_YN
		,AC_MAST.AC_ID,AC_MAST.AC_NAME,#VATAC_MAST1.level1
		,AC_MAST.MAILNAME --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		into #AC_BAL11
		FROM LAC_VW AC
		INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)
		INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) 
		INNER JOIN LITEM_VW MI ON (AC.TRAN_CD = MI.TRAN_CD AND AC.ENTRY_TY = MI.ENTRY_TY) 
		inner join #VATAC_MAST1 ON (AC_MAST.AC_NAME=#VATAC_MAST1.AC_NAME And MI.tax_name = #VATAC_MAST1.tax_name)
		where #VATAC_MAST1.level1 >0 
		and Ac.Date between @Sdate And @Edate 
		And (Ac.Entry_ty In('PT') or (mn.u_imporm='Purchase Return' And Ac.Entry_ty = 'ST'))

		DELETE FROM #AC_BAL11 WHERE 
		DATE < (SELECT TOP 1 DATE FROM #AC_BAL11 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN)
		AND AC_NAME IN (SELECT AC_NAME FROM #AC_BAL11 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN GROUP BY AC_NAME) 


		SELECT AC_NAME,AC_ID,LBAL=SUM(CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END),LEVEL1
		,Mailname --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		INTO #AC_BALL
		FROM #AC_BAL11
		GROUP BY AC_NAME,AC_ID,LEVEL1
		,MAILNAME  --ADDED BY BIRENDRA ON 24 JULY 2010 FOR TKT-3123
		ORDER BY AC_NAME,AC_ID

		insert into #kv_purreg1 (part,level1,taxamt,tran_cd,ac_name,MAILNAME) select 6,level1,taxamt=lbal,0,ac_name,MAILNAME from #AC_BALL
		---<----6
		update #kv_purreg1 set qty=isnull(qty,0),taxabl_amt=isnull(taxabl_amt,0),level1=isnull(level1,0),
		entry_ty=isnull(entry_ty,''),u_pinvno=isnull(u_pinvno,''),u_pinvdt=isnull(u_pinvdt,''),
		inv_no=isnull(inv_no,''),date=isnull(date,''),
		dcinv_no=isnull(dcinv_no,''),dcdate=isnull(dcdate,''),s_tax=isnull(s_tax,''),
		rentry_ty=isnull(rentry_ty,''),rtran_cd=isnull(rtran_cd,0),fld_nm=isnull(fld_nm,''),
		mqty=isnull(mqty,0),aqty=isnull(aqty,0)

		select * from #kv_purreg1 
		order by part,level1
		,(case when entry_ty='pt' then 'a' else (case when entry_ty='st' then 'b' else 'c' end) end) 
		,tran_cd
	end
	
----****
--Print 'KA Stored Procedure Updation Completed'
	