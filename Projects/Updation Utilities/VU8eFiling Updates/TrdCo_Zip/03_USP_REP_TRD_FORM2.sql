
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[USP_REP_TRD_FORM2]
	@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
	@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),
	@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
	@SAMT NUMERIC,@EAMT NUMERIC,
	@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
	@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),
	@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
	@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
	@FINYR NVARCHAR(20), @EXTPAR NVARCHAR(60)
	AS
Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000),@ColOrder as Nvarchar(4000),@InsFlds as Nvarchar(4000) --vasant 13/02/10
	Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50),@TBLNAME3 as VARCHAR(50),@TBLNAME4 as VARCHAR(50),@TBLNAME5 as VARCHAR(50)
	Declare @Rg23cpur as bit,@Rg23flag as bit
		
	Set @ColOrder = 'ware_nm,
		case when rtrim(rgpage1) != '' '' then CONVERT(numeric(10),rgpage1) else 0 end,
		case when rtrim(rgpage2) != '' '' then CONVERT(numeric(10),rgpage2) else 0 end,
		case when csentno > 0 then ''A'' else ''Z'' end,	
		cspageno,csentno,tran_cd'		--vasant 13/02/10
	Set @InsFlds = ''		--vasant 13/02/10

	Set @Rg23cpur = 0
	SET @Rg23flag = 0
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM
	Set @TBLNAME3 = '##TMP3'+@TBLNM
	Set @TBLNAME4 = '##TMP4'+@TBLNM
	Set @TBLNAME5 = '##TMP5'+@TBLNM
	
	EXECUTE USP_REP_FILTCON 
		@VTMPAC=null,@VTMPIT=null,@VSPLCOND=@SPLCOND,
		@VSDATE=null,@VEDATE=null,
		@VSAC =null,@VEAC =null,
		@VSIT= @SITEM,@VEIT=@EITEM,
		@VSAMT=null,@VEAMT=null,
		@VSDEPT=null,@VEDEPT=@EDEPT,
		@VSCATE =null,@VECATE =null,
		@VSWARE =@SWARE,@VEWARE  =@EWARE,
		@VSINV_SR =null,@VEINV_SR =null,
		@VMAINFILE='C',@VITFILE='A',@VACFILE=null,
		@VDTFLD = null,@VLYN=null,@VEXPARA=@EXTPAR,
		@VFCON =@FCON OUTPUT

-- 11/02/10
declare @SearchCond as varchar(1000)
declare @oldfcon as varchar(1000)
set @oldfcon = @fcon
set @SearchCond = 'A.WARE_NM BETWEEN '''+rtrim(@SWARE)+'''  AND '''+rtrim(@EWARE)+''''
set @FCON = replace(@FCON,@SearchCond,'CASE when a.entry_ty = ''IR'' then A.WARENM_FR else A.WARE_NM end BETWEEN '''+@SWARE+''' AND '''+@EWARE+'''' ) 
-- 11/02/10

--**-- @VSDATE=null	@VDTFLD = null

	select top 1 @Rg23cpur = rg23cpur from manufact
	select top 1 @Rg23flag = rg23flag from manufact		--vasant 140210
	--set @Rg23flag = 0									--vasant 140210

--**-- ,c.u_choice as csuppinv
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select b.tran_cd as ctran_cd,b.entry_ty as centry_ty,b.itserial as citserial,
		b.spageno as cspageno,b.sentno as csentno,		
		b.qty as cqty,b.examt as cexamt,b.u_cessamt as ccessamt,b.u_cvdamt as ccvdamt,b.u_hcesamt as chcesamt,a.cvdpass,
		b.ptran_cd,b.pentry_ty,b.pdate,b.pitserial,
		a.date as cdate,c.u_choice as csuppinv,
		c.[rule] as crule,c.inv_no as cinv_no,c.cons_id as ccons_id,c.scons_id as cscons_id,
		case when c.entry_ty=''IR'' then h.ac_name else f.mailname end as cparty_nm,e.add1 as cadd1,e.add2 as cadd2,e.add3 as cadd3,e.city as ccity,e.zip as czip,
		e.range as crange,e.division as cdivision,e.coll as ccoll,e.eccno as ceccno,space(100) as cexregno
		INTO '+@TBLNAME1+' FROM litemall b 
		left join trademain c 
		on b.entry_Ty = c.entry_ty and b.tran_Cd = c.tran_Cd
		left join tradeitem a
		on b.entry_Ty = a.entry_ty and b.tran_Cd = a.tran_Cd and b.itserial = a.itserial
		left join shipto e
		on c.cons_id=e.ac_id and c.scons_id=e.shipto_id
		left join ac_mast f
		on c.cons_id=f.ac_id
		left join ac_mast h
		on h.ac_id=c.ac_id
		left join lcode g
		on c.entry_ty = g.entry_ty	
		left join it_mast
		on a.it_code = it_mast.it_code '+RTRIM(@FCON)+' AND c.[rule] in (''EXCISE'',''NON-EXCISE'')
		and	(g.entry_ty in (''DC'',''IR'',''SS'') Or g.bcode_nm in (''DC'',''IR'',''SS''))'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME1+' set cadd1 = e.add1,cadd2 = e.add2,cadd3 = e.add3,ccity = e.city,czip = e.zip,
		crange = e.range,cdivision = e.division,ccoll = e.coll,ceccno = e.eccno,cexregno = e.exregno
		from '+@TBLNAME1+' a,ac_mast e 
		where a.ccons_id = e.ac_id and isnull(a.cscons_id,0) = 0'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	--**--  --set @FCON = replace(@FCON,'C.DATE','C.U_PINVDT')
	set @FCON = replace(@oldfcon,'A.WARE_NM','B.WARE_NM')  -- 11/02/10

--**-- ,c.u_choice as ssuppinv
--**-- b.tran_cd as ntran_cd,b.entry_ty as nentry_ty,b.itserial as nitserial,
--**-- printit=cast(0 as bit),
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select printit=cast(0 as bit),a.*,
		b.tran_cd,b.entry_ty,b.date,b.itserial,
		b.tran_cd as ntran_cd,b.entry_ty as nentry_ty,b.itserial as nitserial,
		b.ware_nm,b.rgpage,b.tariff,
		b.mtduty,b.u_basduty,b.u_cessper,b.u_cvdper,b.u_hcessper,
		(case when b.billqty != 0 then b.billqty else b.qty end) as qty,
		(case when b.billqty != 0 then b.billexamt else b.examt end) as examt,
		(case when b.billqty != 0 then b.billcesamt else b.u_cessamt end) as u_cessamt,
		(case when b.billqty != 0 then b.billcvdamt else b.u_cvdamt end) as u_cvdamt,
		(case when b.billqty != 0 then b.billhcsamt else b.u_hcesamt end) as u_hcesamt,
		b.billqty,b.billexamt,b.billcesamt,b.billcvdamt,b.billhcsamt,
		it_mast.it_name,it_mast.rateunit,it_mast.exrateunit,c.u_choice as ssuppinv,
		c.ettype,(case when isnull(c.u_pinvno,'' '') = '' '' then c.inv_no else c.u_pinvno end) as u_pinvno,(case when isnull(c.u_pinvdt,'' '') = '' '' then c.date else c.u_pinvdt end) as u_pinvdt,
		d.manubill,d.manudate,d.manuqty,d.manumtduty,
		d.manuexamt,d.manucesamt,d.manucvdamt,d.manuhcsamt,
		d.manuac_id as mcons_id,d.manusac_id as mscons_id,
		i.mailname as mname,f.add1 as madd1,f.add2 as madd2,f.add3 as madd3,f.city as mcity,f.zip as mzip,
		f.range as mrange,f.division as mdivision,f.coll as mcoll,f.eccno as meccno,space(100) as mexregno,
		c.cons_id as scons_id,c.scons_id as sscons_id,
		h.mailname as sname,e.add1 as sadd1,e.add2 as sadd2,e.add3 as sadd3,e.city as scity,e.zip as szip,
		e.range as srange,e.division as sdivision,e.coll as scoll,e.eccno as seccno,space(100) as sexregno,
		b.qty as balqty,b.examt as balexamt,b.u_cessamt as balcesamt,b.u_cvdamt as balcvdamt,b.u_hcesamt as balhcsamt,
		b.qty as srqty,b.examt as srexamt,b.u_cessamt as srcesamt,b.u_cvdamt as srcvdamt,b.u_hcesamt as srhcsamt,
		b.qty as popqty,b.examt as popexamt,b.u_cessamt as popcesamt,b.u_cvdamt as popcvdamt,b.u_hcesamt as pophcsamt
		INTO '+@TBLNAME2+' FROM tradeitem b 
		left join trademain c 
		on b.entry_Ty = c.entry_ty and b.tran_Cd = c.tran_Cd
		left join manu_det d
		on b.entry_Ty = d.entry_ty and b.tran_Cd = d.tran_Cd and b.itserial = d.itserial
		left join it_mast 
		on b.it_code = it_mast.it_code
		left join shipto e
		on c.cons_id=e.ac_id and c.scons_id=e.shipto_id
		left join shipto f
		on d.manuac_id = f.ac_id and d.manusac_id=f.shipto_id
		left join ac_mast h
		on c.cons_id=h.ac_id
		left join ac_mast i
		on d.manuac_id = i.ac_id
		left join lcode g
		on c.entry_ty = g.entry_ty
		left join '+@TBLNAME1+' a
		on b.entry_Ty = a.pentry_ty and b.tran_Cd = a.ptran_Cd and b.itserial = a.pitserial '+RTRIM(@FCON)+' AND c.[rule] in (''EXCISE'',''NON1-EXCISE'')
		and (g.entry_ty in (''AR'',''IR'') or g.bcode_nm in (''AR'',''IR'') or (g.entry_ty in (''GT'') And c.u_sinfo=0) or (g.bcode_nm in (''GT'') And c.u_sinfo=0))'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
--**-- GT GT

--**--
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where cdate > '''+CONVERT(VARCHAR(50),@EDATE)+''' and csuppinv = 0'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where case when entry_ty = ''IR'' then date else u_pinvdt end > '''+CONVERT(VARCHAR(50),@EDATE)+''' and ssuppinv = 0'	--vasant 14/02/10
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where csuppinv = 1 
		and centry_ty+cast(ctran_cd as varchar(10))+citserial not in 
		(select a.entry_ty+cast(a.tran_cd as varchar(10))+a.itserial from spdiff a,'+@TBLNAME2+' b 
		where a.pentry_ty = b.centry_ty and a.ptran_cd = b.ctran_cd and a.pitserial = b.citserial
		group by a.entry_ty,a.tran_cd,a.itserial)'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where ssuppinv = 1 
		and entry_ty+cast(tran_cd as varchar(10))+itserial not in 
		(select a.entry_ty+cast(a.tran_cd as varchar(10))+a.itserial from spdiff a,'+@TBLNAME2+' b 
		where a.pentry_ty = b.entry_ty and a.ptran_cd = b.tran_cd and a.pitserial = b.itserial
		group by a.entry_ty,a.tran_cd,a.itserial)'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME2+' set nentry_ty = b.pentry_ty,ntran_cd = b.ptran_cd,nitserial = b.pitserial
		from '+@TBLNAME2+' a,spdiff b 
		where a.entry_ty = b.entry_ty and a.tran_cd = b.tran_cd and a.itserial = b.itserial'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select a.nentry_ty,a.ntran_cd,a.nitserial,a.entry_ty,a.tran_cd,a.itserial,
		a.popexamt,a.popcesamt,a.popcvdamt,a.pophcsamt		
		into '+@TBLNAME5+' from '+@TBLNAME2+' a 
		group by a.nentry_ty,a.ntran_cd,a.nitserial,a.entry_ty,a.tran_cd,a.itserial,
		a.popexamt,a.popcesamt,a.popcvdamt,a.pophcsamt'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select a.nentry_ty,a.ntran_cd,a.nitserial,
		sum(a.popexamt) as popexamt,sum(a.popcesamt) as popcesamt,sum(a.popcvdamt) as popcvdamt,sum(a.pophcsamt) as pophcsamt
		into '+@TBLNAME3+' from '+@TBLNAME5+' a 
		group by a.nentry_ty,a.ntran_cd,a.nitserial'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME2+' 
		set popexamt = b.popexamt,popcesamt = b.popcesamt,popcvdamt = b.popcvdamt,pophcsamt = b.pophcsamt
		from '+@TBLNAME2+' a, '+@TBLNAME3+' b
		where a.nentry_ty+convert(varchar(10),a.ntran_cd)+a.nitserial =
		b.nentry_ty+convert(varchar(10),b.ntran_cd)+b.nitserial' 
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME3
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME5
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

--**--

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME2+' set balqty = 0,balexamt = 0,balcesamt = 0,balcvdamt = 0, balhcsamt = 0,
		srqty = 0,srexamt = 0,srcesamt = 0,srcvdamt = 0,srhcsamt = 0'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME2+' set sadd1 = e.add1,sadd2 = e.add2,sadd3 = e.add3,scity = e.city,szip = e.zip,
		srange = e.range,sdivision = e.division,scoll = e.coll,seccno = e.eccno,sexregno = e.exregno
		from '+@TBLNAME2+' a,ac_mast e 
		where a.scons_id = e.ac_id and isnull(a.sscons_id,0) = 0'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME2+' set madd1 = f.add1,madd2 = f.add2,madd3 = f.add3,mcity = f.city,mzip = f.zip,
		mrange = f.range,mdivision = f.division,mcoll = f.coll,meccno = f.eccno,mexregno = f.exregno
		from '+@TBLNAME2+' a,ac_mast f
		where a.mcons_id = f.ac_id and isnull(a.mscons_id,0) = 0'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select pentry_ty,ptran_cd,pitserial,rentry_ty,rtran_cd,ritserial,
		sum(case when isnull(acserial,'' '') = ''DI'' then 0 else qty end) as qty,sum(examt) as examt,sum(u_cessamt) as u_cessamt,sum(u_hcesamt) as u_hcesamt,sum(u_cvdamt) as u_cvdamt
		INTO '+@TBLNAME4+' from litemall where isnull(rtran_cd,0) > 0
		group by pentry_ty,ptran_cd,pitserial,rentry_ty,rtran_cd,ritserial' --vasant 140210
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME2+' 
		set srqty = b.qty,srexamt = b.examt,srcesamt = b.u_cessamt,srcvdamt = b.u_cvdamt,srhcsamt = b.u_hcesamt
		from '+@TBLNAME2+' a, '+@TBLNAME4+' b
		where a.entry_ty+convert(varchar(10),a.tran_cd)+a.itserial =
		b.rentry_ty+convert(varchar(10),b.rtran_cd)+b.ritserial
		and a.centry_ty+convert(varchar(10),a.ctran_cd)+a.citserial =
		b.pentry_ty+convert(varchar(10),b.ptran_cd)+b.pitserial' 
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	set @sqlcommand = ''
	set @sqlcommand ='update '+@TBLNAME2+' 
		set ctran_cd = 0,centry_ty = '' '',citserial = '' '',cspageno = '' '',csentno = 0,
		cdate = '' '',crule = '' '',cinv_no = '' '',ccons_id = 0,cscons_id = 0,
		cparty_nm = '' '',cadd1 = '' '',cadd2 = '' '',cadd3 = '' '',ccity = '' '',czip = '' '',
		crange = '' '',cdivision = '' '',ccoll = '' '',ceccno = '' '',cexregno = '' '',cvdpass=isnull(cvdpass,0),
		cqty = 0,cexamt=0,ccessamt=0,ccvdamt=0,chcesamt=0,csuppinv = 0 where cqty is null'	--vasant 13/02/10 ,csuppinv = 0
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	/*vasant 14/02/10
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' 
		where balqty <= 0 and cdate < '''+CONVERT(VARCHAR(50),@SDATE)+''' '
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	vasant 14/02/10 */

	SET @SQLCOMMAND = ''
	If @Rg23cpur = 0 
	begin
		SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where cqty = 0 '
	end	
	else
	begin
		SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where cqty = 0 and case when entry_ty = ''IR'' then date else u_pinvdt end not between '''+CONVERT(VARCHAR(50),@SDATE)+''' and '''+CONVERT(VARCHAR(50),@EDATE)+''' '	--vasant 14/02/10
	end
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	If @Rg23flag = 1
	begin
		SET @SQLCOMMAND = ''
		SET @SQLCOMMAND = 'select a.entry_ty,a.tran_cd,a.itserial,
			isnull(a.popqty,0) - sum(isnull(a.cqty,0)) + sum(isnull(a.srqty,0)) as balqty,
			isnull(a.popexamt,0) - sum(isnull(a.cexamt,0)) + sum(isnull(a.srexamt,0)) as balexamt,
			isnull(a.popcesamt,0) - sum(isnull(a.ccessamt,0)) + sum(isnull(a.srcesamt,0)) as balcesamt,
			isnull(a.popcvdamt,0) - sum(isnull(a.ccvdamt,0)) + sum(isnull(a.srcvdamt,0)) as balcvdamt,
			isnull(a.pophcsamt,0) - sum(isnull(a.chcesamt,0)) + sum(isnull(a.srhcsamt,0)) as balhcsamt
			INTO '+@TBLNAME3+' from '+@TBLNAME2+' a where a.cdate < '''+CONVERT(VARCHAR(50),@SDATE)+''' 
			group by a.entry_ty,a.tran_cd,a.itserial,a.popqty,a.popexamt,a.popcesamt,a.popcvdamt,a.pophcsamt '
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		SET @SQLCOMMAND = ''
		SET @SQLCOMMAND = 'update '+@TBLNAME2+' 
			set balqty = b.balqty,balexamt = b.balexamt,balcesamt = b.balcesamt,balcvdamt = b.balcvdamt,balhcsamt = b.balhcsamt
			from '+@TBLNAME2+' a, '+@TBLNAME3+' b
			where a.entry_ty+convert(varchar(10),a.tran_cd)+a.itserial =
			b.entry_ty+convert(varchar(10),b.tran_cd)+b.itserial' 
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME3
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

		SET @SQLCOMMAND = ''
		SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where cqty > 0 and Cdate not between '''+CONVERT(VARCHAR(50),@SDATE)+''' and '''+CONVERT(VARCHAR(50),@EDATE)+''' '
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	end
	else
	begin
		SET @SQLCOMMAND = ''
		SET @SQLCOMMAND = 'delete from '+@TBLNAME2+' where cqty > 0 and Cdate not between '''+CONVERT(VARCHAR(50),@SDATE)+''' and '''+CONVERT(VARCHAR(50),@EDATE)+''' and entry_ty+convert(varchar(10),tran_cd)+itserial not in (select entry_ty+convert(varchar(10),tran_cd)+itserial from '+@TBLNAME2+' where cqty > 0 and Cdate between '''+CONVERT(VARCHAR(50),@SDATE)+''' and '''+CONVERT(VARCHAR(50),@EDATE)+''')'
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	end

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select a.*,
		(case when PATINDEX(''%[/]%'',a.rgpage) > 0 then left(a.rgpage,PATINDEX(''%[/]%'',a.rgpage)-1) else a.rgpage end) as rgpage1,
		(case when PATINDEX(''%[/]%'',a.rgpage) > 0 then substring(a.rgpage,PATINDEX(''%[/]%'',a.rgpage)+1,len(a.rgpage)) else '' '' end) as rgpage2,
		(case when a.cspageno = '' '' then a.rgpage else a.cspageno end) as ppageno, 
		b.add1 as wadd1,b.add2 as wadd2,b.add3 as wadd3
		into '+@TBLNAME5+' from '+@TBLNAME2+' a,warehouse b where a.ware_nm = b.ware_nm'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

--**-- +' into '+@TBLNAME3+' 
--**-- Colno=IDENTITY (int, 1, 1),
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select Colno=IDENTITY (int, 1, 1),* into '+@TBLNAME3+' from '+@TBLNAME5+'	
		order by '+@ColOrder	--vasant 13/02/10
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

--**--
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME5
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	--vasant 13/02/10
	SET @SQLCOMMAND = 'select * into '+@TBLNAME5+' from '+@TBLNAME3+' where csuppinv = 0 and ssuppinv = 1'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'delete from '+@TBLNAME3+' where colno in (select colno from '+@TBLNAME5+')'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Set @InsFlds = 'printit,tran_cd,entry_ty,date,itserial,
		ntran_cd,nentry_ty,nitserial,
		ware_nm,rgpage,tariff,mtduty,u_basduty,u_cessper,u_cvdper,u_hcessper,
		qty,examt,u_cessamt,u_cvdamt,u_hcesamt,
		billqty,billexamt,billcesamt,billcvdamt,billhcsamt,
		it_name,rateunit,exrateunit,ssuppinv,
		ettype,u_pinvno,u_pinvdt,
		manubill,manudate,manuqty,manumtduty,
		manuexamt,manucesamt,manucvdamt,manuhcsamt,
		mcons_id,mscons_id,
		mname,madd1,madd2,madd3,mcity,mzip,
		mrange,mdivision,mcoll,meccno,mexregno,
		scons_id,sscons_id,
		sname,sadd1,sadd2,sadd3,scity,szip,
		srange,sdivision,scoll,seccno,sexregno,
		balqty,balexamt,balcesamt,balcvdamt,balhcsamt,
		srqty,srexamt,srcesamt,srcvdamt,srhcsamt,
		popqty,popexamt,popcesamt,popcvdamt,pophcsamt,
		rgpage1,rgpage2,ppageno'

	SET @SQLCOMMAND = 'insert into '+@TBLNAME3+' ('+@InsFlds+') select '+@InsFlds+' from '+@TBLNAME5+
		' where nentry_ty+cast(ntran_cd as varchar(10))+nitserial not in 
		(select a.nentry_ty+cast(a.ntran_cd as varchar(10))+a.nitserial from '+@TBLNAME3+' a,'+@TBLNAME5+' b where
		a.nentry_ty+cast(a.ntran_cd as varchar(10))+a.nitserial = b.nentry_ty+cast(b.ntran_cd as varchar(10))+b.nitserial	
		and a.cspageno = b.cspageno and a.colno > b.colno )'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Declare @tmptbl as varchar(25)
	set @tmptbl = ''''+@TBLNAME3+''''
	print @tmptbl
	EXECUTE Update_table_column_default_value @TBLNAME3,0

Set @InsFlds = 'printit=b.printit,tran_cd=b.tran_cd,entry_ty=b.entry_ty,date=b.date,itserial=b.itserial,
		ntran_cd=b.ntran_cd,nentry_ty=b.nentry_ty,nitserial=b.nitserial,
		ware_nm=b.ware_nm,rgpage=b.rgpage,tariff=b.tariff,mtduty=b.mtduty,
		u_basduty=b.u_basduty,u_cessper=b.u_cessper,u_cvdper=b.u_cvdper,u_hcessper=b.u_hcessper,
		qty=b.qty,examt=b.examt,u_cessamt=b.u_cessamt,u_cvdamt=b.u_cvdamt,u_hcesamt=b.u_hcesamt,
		billqty=b.billqty,billexamt=b.billexamt,billcesamt=b.billcesamt,billcvdamt=b.billcvdamt,billhcsamt=b.billhcsamt,
		it_name=b.it_name,rateunit=b.rateunit,exrateunit=b.exrateunit,ssuppinv=b.ssuppinv,
		ettype=b.ettype,u_pinvno=b.u_pinvno,u_pinvdt=b.u_pinvdt,
		manubill=b.manubill,manudate=b.manudate,manuqty=b.manuqty,manumtduty=b.manumtduty,
		manuexamt=b.manuexamt,manucesamt=b.manucesamt,manucvdamt=b.manucvdamt,manuhcsamt=b.manuhcsamt,
		mcons_id=b.mcons_id,mscons_id=b.mscons_id,
		mname=b.mname,madd1=b.madd1,madd2=b.madd2,madd3=b.madd3,mcity=b.mcity,mzip=b.mzip,
		mrange=b.mrange,mdivision=b.mdivision,mcoll=b.mcoll,meccno=b.meccno,mexregno=b.mexregno,
		scons_id=b.scons_id,sscons_id=b.sscons_id,
		sname=b.sname,sadd1=b.sadd1,sadd2=b.sadd2,sadd3=b.sadd3,scity=b.scity,szip=b.szip,
		srange=b.srange,sdivision=b.sdivision,scoll=b.scoll,seccno=b.seccno,sexregno=b.sexregno,
		balqty=b.balqty,balexamt=b.balexamt,balcesamt=b.balcesamt,balcvdamt=b.balcvdamt,balhcsamt=b.balhcsamt,
		srqty=b.srqty,srexamt=b.srexamt,srcesamt=b.srcesamt,srcvdamt=b.srcvdamt,srhcsamt=b.srhcsamt,
		popqty=b.popqty,popexamt=b.popexamt,popcesamt=b.popcesamt,popcvdamt=b.popcvdamt,pophcsamt=b.pophcsamt,
		rgpage1=b.rgpage1,rgpage2=b.rgpage2,ppageno=b.ppageno'

	SET @SQLCOMMAND = 'update '+@TBLNAME3+' set '+@InsFlds+
		' from '+@TBLNAME3+' a,'+@TBLNAME5+' b where
		a.nentry_ty+cast(a.ntran_cd as varchar(10))+a.nitserial = b.nentry_ty+cast(b.ntran_cd as varchar(10))+b.nitserial	
		and a.cspageno = b.cspageno and a.colno > b.colno '
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'select centry_ty,ctran_cd,citserial,nentry_ty,ntran_cd,nitserial,
		sum(cexamt) as cexamt,sum(ccessamt) as ccessamt,sum(ccvdamt) as ccvdamt,
		sum(chcesamt) as chcesamt,
		sum(srexamt) as srexamt,sum(srcesamt) as srcesamt,sum(srcvdamt) as srcvdamt, 
		sum(srhcsamt) as srhcsamt into '+@TBLNAME1+' from '+@TBLNAME5+' 
		group by centry_ty,ctran_cd,citserial,nentry_ty,ntran_cd,nitserial'	--vasant 140210
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'update '+@TBLNAME3+' set cexamt=a.cexamt+b.cexamt,ccessamt=a.ccessamt+b.ccessamt,
		ccvdamt=a.ccvdamt+b.ccvdamt,chcesamt=a.chcesamt+b.chcesamt,
		srexamt=a.srexamt+b.srexamt,srcesamt=a.srcesamt+b.srcesamt,
		srcvdamt=a.srcvdamt+b.srcvdamt,srhcsamt=a.srhcsamt+b.srhcsamt
		from '+@TBLNAME3+' a,'+@TBLNAME1+' b where
		a.nentry_ty+cast(a.ntran_cd as varchar(10))+a.nitserial = b.nentry_ty+cast(b.ntran_cd as varchar(10))+b.nitserial	
		and a.centry_ty+cast(a.ctran_cd as varchar(10))+a.citserial = b.centry_ty+cast(b.ctran_cd as varchar(10))+b.citserial '	--vasant 140210
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'update '+@TBLNAME3+' set cqty = isnull(cqty,0)'	--vasant 13/02/10
	EXECUTE SP_EXECUTESQL @SQLCOMMAND	--vasant 13/02/10

	SET @SQLCOMMAND = 'delete from '+@TBLNAME3+' where cqty = 0 and 
		entry_ty+cast(tran_cd as varchar(10))+itserial in
		(select entry_ty+cast(tran_cd as varchar(10))+itserial from '+@TBLNAME3+' where cqty > 0)'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME5
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	--vasant 13/02/10

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT count(colno) as colcnt,MIN(Colno) AS Colno,nentry_ty,ntran_cd,nitserial into '+@TBLNAME5+' from '+@TBLNAME3+' group by nentry_ty,ntran_cd,nitserial'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	--vasant 13/02/10
	--SET @SQLCOMMAND = ''
	--SET @SQLCOMMAND = 'update '+@TBLNAME3+' set printit = 1 from '+@TBLNAME3+' a,'+@TBLNAME5+' b where a.nentry_ty = b.nentry_ty and a.ntran_cd = b.ntran_cd and a.nitserial = b.nitserial and a.colno = b.colno'
	--EXECUTE SP_EXECUTESQL @SQLCOMMAND	
	--vasant 13/02/10

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	--SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
	--EXECUTE SP_EXECUTESQL @SQLCOMMAND

	--SET @SQLCOMMAND = ''
	--SET @SQLCOMMAND = 'select nentry_ty,ntran_cd,nitserial,
	--	into '+@TBLNAME2+' from '+@TBLNAME3+' where ssuppinv = 1'
	--EXECUTE SP_EXECUTESQL @SQLCOMMAND

	--vasant 13/02/10
/*
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select nentry_ty,ntran_cd,nitserial,
		mtduty,u_basduty,u_cessper,u_cvdper,u_hcessper,
		qty,examt,u_cessamt,u_cvdamt,u_hcesamt,
		billqty,billexamt,billcesamt,billcvdamt,billhcsamt,
		u_pinvno,u_pinvdt,
		manubill,manudate,manuqty,manumtduty,
		manuexamt,manucesamt,manucvdamt,manuhcsamt,
		srqty,srexamt,srcesamt,srcvdamt,srhcsamt,
		popqty,popexamt,popcesamt, popcvdamt, pophcsamt
		into '+@TBLNAME1+' from '+@TBLNAME3+' where ssuppinv = 1'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	print 'vv' */

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select min(a.colno) as colno,a.nentry_ty,a.ntran_cd,a.nitserial,a.entry_ty,a.tran_cd,a.itserial into '+@TBLNAME1+' from '+@TBLNAME3+' a, '+@TBLNAME5+' b 
		where a.nentry_ty = b.nentry_ty and a.ntran_cd = b.ntran_cd and a.nitserial = b.nitserial 
		group by a.nentry_ty,a.ntran_cd,a.nitserial,a.entry_ty,a.tran_cd,a.itserial'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	--vasant 13/02/10  -printit1


	SET @SQLCOMMAND = ''
	--vasant 13/02/10
	--SET @SQLCOMMAND = 'update '+@TBLNAME3+' set printit = 1 from '+@TBLNAME3+' a,'+@TBLNAME5+' b,'+@TBLNAME1+' c where a.nentry_ty = b.nentry_ty and a.ntran_cd = b.ntran_cd and a.nitserial = b.nitserial and a.colno = b.colno + 1
	--	and a.nentry_ty = c.nentry_ty and a.ntran_cd = c.ntran_cd and a.nitserial = c.nitserial'
	----SET @SQLCOMMAND = 'update '+@TBLNAME3+' set printit = 1 from '+@TBLNAME3+' a,'+@TBLNAME5+' b where a.nentry_ty = b.nentry_ty and a.ntran_cd = b.ntran_cd and a.nitserial = b.nitserial and a.colno > b.colno'
	SET @SQLCOMMAND = 'update '+@TBLNAME3+' set printit = 1 
		from '+@TBLNAME3+' a,'+@TBLNAME1+' b 
		where a.colno = b.colno and a.nentry_ty = b.nentry_ty and a.ntran_cd = b.ntran_cd and a.nitserial = b.nitserial 
		and a.entry_ty = b.entry_ty and a.tran_cd = b.tran_cd and a.itserial = b.itserial '
	--vasant 13/02/10  -printit1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Set @ColOrder = 'ware_nm,cdate,cinv_no,
		case when rtrim(rgpage1) != '' '' then CONVERT(numeric(10),rgpage1) else 0 end,
		case when rtrim(rgpage2) != '' '' then CONVERT(numeric(10),rgpage2) else 0 end,
		cspageno,csentno'				--vasant 14/02/10

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'update '+@TBLNAME3+' set exrateunit = b.e_uom from '+@TBLNAME3+' a,uom b where a.rateunit = b.u_uom and (a.exrateunit = '' '' or a.exrateunit not in (select e_uom from uom) )'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select * from '+@TBLNAME3+@EXTPAR+' order by '+@ColOrder	--vasant 13/02/10
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME3
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

--**--
	--vasant 13/02/10
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	--vasant 13/02/10
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME4
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME5
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

