If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_Itemwise_Purchase_REGISTER'))
Begin
	Drop Procedure USP_REP_Itemwise_Purchase_REGISTER
End

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



-- =============================================
-- Author:		Satish Pal.
-- Create date: 02/11/2011
-- Description:	This Stored procedure is useful to generate Itemwise Purchase REGISTER.
-- Modify date: By: Sandeep shah for bug-1444 on 18/01/2012.
-- Modify date: By: Sandeep shah for bug-1724 on 16/04/2012.
-- Remark:
-- =============================================

CREATE PROCEDURE   [dbo].[USP_REP_Itemwise_Purchase_REGISTER]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(100)= NULL
AS

DECLARE @FCON AS NVARCHAR(2000)
declare @RuleCondition varchar(1000)
	set @RuleCondition=''
	if charindex('$>Rule',@expara)<>0
	begin

		set @RuleCondition=@expara
		SET @RuleCondition=REPLACE(@RuleCondition, '`','''')
		set @RuleCondition=substring(@RuleCondition,charindex('$>Rule',@RuleCondition)+6,len(@RuleCondition)-(charindex('$>Rule',@RuleCondition)+5))
	set @RuleCondition=substring(@RuleCondition,1,charindex('<$Rule',@RuleCondition)-1)
	end

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='PTMAIN',@VITFILE='PTITEM',@VACFILE='AC'
,@VDTFLD ='date'
,@VLYN=Null
,@VEXPARA=null
,@VFCON =@FCON OUTPUT
print @FCON
DECLARE @SQLCOMMAND NVARCHAR(4000)
SET @SQLCOMMAND='SELECT PTMAIN.TRAN_CD,PTMAIN.DATE,PTMAIN.INV_NO,PTMAIN.U_PINVNO,PTMAIN.U_PINVDT,PTMAIN.PARTY_NM,PTMAIN.[RULE]' 
SET @SQLCOMMAND=@SQLCOMMAND+' '+',PTMAIN.CATE,PTMAIN.INV_SR,PTITEM.ITEM,qty=(case when isnull(ptitem.dc_no,'+''''''+')<>'+'''DI'''+' then PTITEM.QTY else 0 end),PTITEM.RATE,PTITEM.U_ASSEAMT,PTITEM.TAX_NAME'--chnages by sandeep shah qty  column for bug-1724
SET @SQLCOMMAND=@SQLCOMMAND+' '+',PTITEM.TAXAMT,PTITEM.BCDPER,PTITEM.BCDAMT,PTITEM.U_BASDUTY,PTITEM.EXAMT,PTITEM.U_CESSPER,PTITEM.U_CESSAMT,PTITEM.U_HCESSPER'  --Added by sandeep shah u_bcdper and u_bcdamt column for bug-1444
SET @SQLCOMMAND=@SQLCOMMAND+' '+',PTITEM.U_HCESAMT,PTITEM.U_CVDPER,PTITEM.U_CVDAMT,PTITEM.GRO_AMT,IT_MAST.[GROUP],IT_MAST.RATEUNIT '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'FROM PTMAIN  INNER JOIN PTITEM ON (PTMAIN.TRAN_CD=PTITEM.TRAN_CD) INNER JOIN AC_MAST ON (AC_MAST.AC_ID=PTMAIN.AC_ID)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN IT_MAST ON (IT_MAST.IT_CODE=PTITEM.IT_CODE)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+@RuleCondition
SET @SQLCOMMAND=@SQLCOMMAND+' '+@FCON
SET @SQLCOMMAND=@SQLCOMMAND+' '+'ORDER BY PTITEM.ITEM,PTMAIN.DATE,PTMAIN.PARTY_NM'
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND



