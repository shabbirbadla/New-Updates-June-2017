If Exists (Select [Name] From SysObjects Where xType='P' and [Name]='USP_REP_itemwise_salesreg_multiUOM')
Begin
	Drop Procedure USP_REP_itemwise_salesreg_multiUOM
End
GO

/* 
Modified By/Date  : Shrikant S. on 14/09/2012 for Auto Updater

*/
Create PROCEDURE [dbo].[USP_REP_itemwise_salesreg_multiUOM] 
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= null
AS
set nocount on
Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2),@SQLCOMMAND NVARCHAR(4000)
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE --null
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='STMAIN',@VITFILE='STITEM',@VACFILE=''
,@VDTFLD ='DATE'
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT


Declare @uom_desc as Varchar(100),@len int,@fld_nm varchar(10),@fld_desc Varchar(10),@count int,@stkl_qty Varchar(100),@colcaption Varchar(500)

select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name())
Create Table #qty_desc (fld_nm varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS, fld_desc varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS)
set @len=len(@uom_desc)
set @stkl_qty=''
If @len>0 
Begin
	while @len>0
	Begin
		set @fld_nm=substring(@uom_desc,1,charindex(':',@uom_desc)-1)
		set @uom_desc=substring(@uom_desc,charindex(':',@uom_desc)+1,@len)
		set @stkl_qty= @stkl_qty +', '+'STITEM.'+@fld_nm

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
		insert into #qty_desc values (@fld_nm,@fld_desc)
	End
End
Else
Begin
	set @stkl_qty=',STITEM.QTY'
End

set @colcaption=(Select ', ['+rtrim(fld_desc)+'] as Cap',row_number() over(order by fld_nm)  From #qty_desc For XML Path(''))
--set @colcaption=replace(replace(@colcaption,'[',char(39)),']',char(39))							--Commented By Shrikant S. on 14/09/2012 for auto updater issue
set @colcaption=case when @colcaption is null then '' else replace(replace(@colcaption,'[',char(39)),']',char(39)) end		--Added By Shrikant S. on 14/09/2012 for auto updater issue

--print @colcaption


SET @SQLCOMMAND=' SELECT STMAIN.TRAN_CD,STMAIN.DATE,STMAIN.INV_NO,STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.PARTY_NM,STMAIN.U_VEHNO'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,STMAIN.[RULE],STMAIN.CATE,STMAIN.INV_SR,STITEM.ITEM '+@stkl_qty+',STITEM.RATE,STITEM.U_ASSEAMT,STITEM.TAX_NAME,STITEM.TAXAMT,STITEM.U_BASDUTY'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,STITEM.EXAMT,STITEM.U_CESSPER,STITEM.U_CESSAMT,STITEM.U_HCESSPER,STITEM.U_HCESAMT,STITEM.U_CVDPER,STITEM.U_CVDAMT,STITEM.GRO_AMT,IT_MAST.[GROUP]'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,IT_MAST.RATEUNIT'+@colcaption 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' FROM STMAIN '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' INNER JOIN IT_MAST ON (IT_MAST.IT_CODE=STITEM.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ @FCON
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ORDER BY STITEM.ITEM,STMAIN.DATE,STMAIN.PARTY_NM'
PRINT @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND