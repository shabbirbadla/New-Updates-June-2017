If Exists (Select [Name] From SysObjects Where xType='P' and [Name]='USP_REP_ILEDGER_UOM_WISE')
Begin
	Drop Procedure USP_REP_ILEDGER_UOM_WISE
End
Go

---------

-- Modify By : Nilesh for Bug 25365 on 23/03/2015
-- EXECUTE USP_REP_ILEDGER_UOM_WISE'','','','04/01/2014','03/31/2015','','','INTEREST                                          ','INTEREST                                          ',0,0,'','','','','','','','','2014-2015',''

---------
Create PROCEDURE [dbo].[USP_REP_ILEDGER_UOM_WISE]
	@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
	@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),
	@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
	@SAMT NUMERIC,@EAMT NUMERIC,
	@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
	@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),
	@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
	@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
	@FINYR NVARCHAR(20),@EXTPAR NVARCHAR(60)
	AS

--Select * from ptmain
SET NOCOUNT ON 
Declare @uom_desc as Varchar(100),@len int,@fld_nm varchar(10),@fld_desc Varchar(10),@count int,@stkl_qty Varchar(100)
Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000)
Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50)
	
	Set @OPENTRY_TY = '''OS'''
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM

--DECLARE openingentry_cursor CURSOR FOR
--		SELECT entry_ty FROM lcode
--		WHERE bcode_nm = 'OS'
--	OPEN openingentry_cursor
--	FETCH NEXT FROM openingentry_cursor into @opentries
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--	   Set @OPENTRY_TY = @OPENTRY_TY +','''+@opentries+''''
--	   FETCH NEXT FROM openingentry_cursor into @opentries
--	END
--	CLOSE openingentry_cursor
--	DEALLOCATE openingentry_cursor

Select @OPENTRY_TY=@OPENTRY_TY+isnull(substring((Select ', ' +Entry_ty From Lcode Where Bcode_nm='OS' For XML Path('')),2,500),'')

	EXECUTE USP_REP_FILTCON 
		@VTMPAC=null,@VTMPIT=@TMPIT,@VSPLCOND=@SPLCOND,
		@VSDATE=null,@VEDATE=@EDATE,
		@VSAC =null,@VEAC =null,
		@VSIT=@SITEM,@VEIT=@EITEM,
		@VSAMT=null,@VEAMT=null,
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =@SWARE,@VEWARE  =@EWARE,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='MVW',@VITFILE='IVW',@VACFILE=null,
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@EXTPAR,
		@VFCON =@FCON OUTPUT

--select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name())    --Commented by nilesh for Bug 25365 on 23/03/2015
select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name()) and convert(varchar(max),industry)=''		----Added by nilesh for Bug 25365 on 23/03/2015
Create Table #qty_desc (fld_nm varchar(10),fld_desc varchar(10))
set @len=len(@uom_desc)
set @stkl_qty=''
If @len>0 
Begin
	while @len>0
	Begin
		set @fld_nm=substring(@uom_desc,1,charindex(':',@uom_desc)-1)
		set @uom_desc=substring(@uom_desc,charindex(':',@uom_desc)+1,@len)
		--PRINT @uom_desc
		--print @fld_nm
		set @stkl_qty= @stkl_qty +', '+'IVW.'+@fld_nm
		if @len>0 and charindex(';',@uom_desc)=0
		begin
			set @uom_desc=@uom_desc
			set @fld_desc=@uom_desc
			SET @len=0
			
		End
		else
		begin
				set @fld_desc=substring(@uom_desc,1,charindex(';',@uom_desc)-1)
				set @uom_desc=substring(@uom_desc,charindex(';',@uom_desc)+1,@len)		
				set @len=len(@uom_desc)
			
		End
		--print @fld_desc
		insert into #qty_desc values (@fld_nm,@fld_desc)
		
	End
End
Else
Begin
	set @stkl_qty=',IVW.QTY'
End

--print @stkl_qty
	--Select @count=count(*) from #qty_desc
	SELECT TRAN_CD=0,mn.ENTRY_TY,mn.DATE,it.QTY,it.ITSERIAL,WARE_NM='',mn.INV_NO,AC_ID=0,ac_mast.AC_NAME,INV_STK=it.pmkey,
		it_mast.IT_CODE,it_mast.IT_NAME,it_mast.RATEUNIT,UNIT=IT_MAST.RATEUNIT
		INTO #TMP_STK FROM stkl_vw_item it
		inner join stkl_vw_main mn on (mn.entry_ty=it.entry_ty and mn.tran_cd =it.tran_cd)
		inner join it_mast on (it.it_code=it_mast.it_code)
		inner join ac_mast on (ac_mast.ac_id=mn.ac_id)
		Where 1=2

--	drop table #tmp1
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT IVW.TRAN_CD,IVW.ENTRY_TY,IVW.DATE,IVW.ITSERIAL,
		IVW.DC_NO,IVW.WARE_NM,
		MVW.INV_NO,LCODE.INV_STK,
		AC_MAST.AC_ID,AC_MAST.AC_NAME,
		IT_MAST.IT_CODE,IT_MAST.IT_NAME,IT_MAST.RATEUNIT '+@stkl_qty+'
		 INTO '+@TBLNAME1+' FROM STKL_VW_ITEM IVW (NOLOCK)
		INNER JOIN AC_MAST (NOLOCK) ON IVW.AC_ID = AC_MAST.AC_ID
		INNER JOIN IT_MAST (NOLOCK) ON IVW.IT_CODE = IT_MAST.IT_CODE
		INNER JOIN STKL_VW_MAIN MVW (NOLOCK) 
			ON IVW.TRAN_CD = MVW.TRAN_CD AND IVW.ENTRY_TY = MVW.ENTRY_TY
		INNER JOIN LCODE (NOLOCK) 
			ON IVW.ENTRY_TY = LCODE.ENTRY_TY AND LCODE.INV_STK IN (''+'',''-'')'+RTRIM(@FCON)
		--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IT_MAST.NONSTK='+CHAR(39)+'Stockable'+CHAR(39) check it from Special Condition
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND MVW.APGEN='+CHAR(39)+'YES'+CHAR(39)
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IVW.DC_NO='+CHAR(39)+' '+CHAR(39)
		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Select @count=count(*) from #qty_desc
	set @Fld_nm='Qty'
	set @len=0
IF @count>0
Begin
	While @count>0
	Begin
		Select @fld_desc=fld_desc from #qty_desc where fld_nm=@fld_nm	
		--print @fld_desc
		SET @SQLCOMMAND = 'INSERT INTO #TMP_STK SELECT TRAN_CD=0,ENTRY_TY='' '',
		DATE=CONVERT(SMALLDATETIME,'''+CONVERT(VARCHAR(50),@SDATE)+'''),
		QTY=IsNull(sum(CASE WHEN TVW.INV_STK = ''+'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.'+@Fld_nm+' END),0)
		   -IsNull(sum(CASE WHEN TVW.INV_STK = ''-'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.'+@Fld_nm+' END),0),
		ITSERIAL='' '',WARE_NM='' '',INV_NO='' '',AC_ID=0,AC_NAME=''Balance B/f'',INV_STK='' '',
		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,UNIT='''+@fld_desc+''' '+
		' FROM '+@TBLNAME1+' TVW
		WHERE (TVW.DATE < '''+CONVERT(VARCHAR(50),@SDATE)+''' OR TVW.ENTRY_TY IN (''OB'')) 
		GROUP BY TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT
		UNION ALL
		SELECT TVW.TRAN_CD,TVW.ENTRY_TY,TVW.DATE,
		QTY=IsNull(CASE WHEN TVW.INV_STK = ''+'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.'+@Fld_nm+' END,0)
		   -IsNull(CASE WHEN TVW.INV_STK = ''-'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.'+@Fld_nm+' END,0),
		TVW.ITSERIAL,TVW.WARE_NM,TVW.INV_NO,TVW.AC_ID,TVW.AC_NAME,TVW.INV_STK,
		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,UNIT='''+@fld_desc+''' '+
		' FROM '+@TBLNAME1+' TVW
		WHERE (TVW.DATE BETWEEN '''+CONVERT(VARCHAR(50),@SDATE)+''' AND '''+CONVERT(VARCHAR(50),@EDATE)+''' AND 
		TVW.ENTRY_TY NOT IN ('+@OPENTRY_TY+'))'
		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		set @len=@len + 1
		set @Fld_nm='Qty'+CONVERT(VARCHAR(2),@len)
		set @count=@count-1
	End
End
Else
Begin
	SET @SQLCOMMAND = 'INSERT INTO #TMP_STK SELECT TRAN_CD=0,ENTRY_TY='' '',
		DATE=CONVERT(SMALLDATETIME,'''+CONVERT(VARCHAR(50),@SDATE)+'''),
		QTY=IsNull(sum(CASE WHEN TVW.INV_STK = ''+'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.QTY END),0)
		   -IsNull(sum(CASE WHEN TVW.INV_STK = ''-'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.QTY END),0),
		ITSERIAL='' '',WARE_NM='' '',INV_NO='' '',AC_ID=0,AC_NAME=''Balance B/f'',INV_STK='' '',
		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,UNIT=RATEUNIT '+
		' FROM '+@TBLNAME1+' TVW
		WHERE (TVW.DATE < '''+CONVERT(VARCHAR(50),@SDATE)+''' OR TVW.ENTRY_TY IN (''OB'')) 
		GROUP BY TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT
		UNION ALL
		SELECT TVW.TRAN_CD,TVW.ENTRY_TY,TVW.DATE,
		QTY=IsNull(CASE WHEN TVW.INV_STK = ''+'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.QTY END,0)
		   -IsNull(CASE WHEN TVW.INV_STK = ''-'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.QTY END,0),
		TVW.ITSERIAL,TVW.WARE_NM,TVW.INV_NO,TVW.AC_ID,TVW.AC_NAME,TVW.INV_STK,
		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,UNIT=RATEUNIT '+
		' FROM '+@TBLNAME1+' TVW
		WHERE (TVW.DATE BETWEEN '''+CONVERT(VARCHAR(50),@SDATE)+''' AND '''+CONVERT(VARCHAR(50),@EDATE)+''' AND 
		TVW.ENTRY_TY NOT IN ('+@OPENTRY_TY+'))'
		PRINT @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
End
select * from #TMP_STK Where qty<>0 
ORDER BY IT_NAME,UNIT,DATE,INV_STK 

SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

DROP TABLE #TMP_STK
DROP TABLE #qty_desc


