
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[USP_REP_INPUTdaily] 
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@RULE  AS VARCHAR(25)
,@EXPARA  AS VARCHAR(60)= null
AS
SET QUOTED_IDENTIFIER OFF

Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2)
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
	,@VMAINFILE='IPMAIN',@VITFILE='IPITEM',@VACFILE='IPACDET'
	,@VDTFLD ='DATE'
	,@VLYN =NULL
	,@VEXPARA=@EXPARA
	,@VFCON =@FCON OUTPUT


DECLARE @SQLCOMMAND NVARCHAR(4000), @VCOND NVARCHAR(2000)
SET @SQLCOMMAND='SELECT IPITEM.ENTRY_TY,IPITEM.DOC_NO,IPITEM.ITSERIAL,IPITEM.inv_no,IPITEM.date,IT_MAST.it_NAME,IPITEM.inv_sr,IPITEM.qty,IT_MAST.p_unit,IPITEM.dept,PTMAIN.[RULE],IRMAIN.[RULE] FROM IPITEM'
SET @SQLCOMMAND=@SQLCOMMAND+' INNER JOIN IPMAIN ON (IPITEM.ENTRY_TY=IPMAIN.ENTRY_TY AND IPITEM.TRAN_CD=IPMAIN.TRAN_CD)'
SET @SQLCOMMAND=@SQLCOMMAND+' INNER JOIN IT_MAST ON (IPITEM.ITEM=IT_MAST.IT_NAME)'
SET @SQLCOMMAND=@SQLCOMMAND+' LEFT JOIN IPITREF ON (IPITEM.ENTRY_TY=IPITREF.ENTRY_TY  AND IPITEM.DATE=IPITREF.DATE AND IPITEM.DOC_NO=IPITREF.DOC_NO AND IPITEM.ITSERIAL=IPITREF.ITSERIAL)' 
SET @SQLCOMMAND=@SQLCOMMAND+' LEFT JOIN PTMAIN ON (PTMAIN.ENTRY_TY=IPITREF.RENTRY_TY  AND PTMAIN.INV_NO=IPITREF.RINV_NO AND PTMAIN.INV_SR=IPITREF.RINV_SR AND PTMAIN.L_YN=IPITREF.RL_YN) '
SET @SQLCOMMAND=@SQLCOMMAND+' LEFT JOIN IRMAIN ON (IRMAIN.ENTRY_TY=IPITREF.RENTRY_TY AND IRMAIN.INV_SR=IPITREF.RINV_SR AND IRMAIN.INV_NO=IPITREF.RINV_NO AND IRMAIN.L_YN=IPITREF.RL_YN)'
SET @SQLCOMMAND=@SQLCOMMAND+RTRIM(@FCON)
SET @SQLCOMMAND=@SQLCOMMAND+' ORDER BY IPITEM.ENTRY_TY,IPITEM.DATE,IPITEM.DOC_NO'
PRINT @SQLCOMMAND
--
--SET @SQLCOMMAND='SELECT IPITEM.TRAN_CD,IPITEM.ENTRY_TY,IPITEM.INV_NO,IPITEM.DATE,IT_MAST.IT_NAME,IPITEM.QTY,IPITEM.U_EXPMARK,IPITEM.U_CIFAMT,IPITEM.EXAMT,IPITEM.U_EXAMT,IPITEM.U_ACAMT1,IPITEM.U_HACAMT1,IPITEM.U_CESSAMT,IPITEM.U_HCESSAMT,IPITEM.U_IMPDUTY,IT_MAST.P_UNIT,IPITEM.U_ASSEAMT,PTMAIN.[RULE] FROM IPITEM '
--SET @SQLCOMMAND=@SQLCOMMAND+'INNER JOIN IPMAIN ON(IPITEM.ENTRY_TY=IPMAIN.ENTRY_TY AND IPITEM.TRAN_CD=IPMAIN.TRAN_CD)'
--SET @SQLCOMMAND=@SQLCOMMAND+'INNER JOIN IT_MAST ON (IPITEM.ITEM=IT_MAST.IT_NAME)'
--SET @SQLCOMMAND=@SQLCOMMAND+'LEFT JOIN IPITREF ON (IPITEM.ENTRY_TY=IPITREF.ENTRY_TY  AND IPITEM.DOC_NO=IPITREF.DOC_NO AND IPITEM.TRAN_CD=IPITREF.TRAN_CD) '
--SET @SQLCOMMAND=@SQLCOMMAND+'LEFT JOIN PTMAIN ON (PTMAIN.ENTRY_TY=IPITREF.RENTRY_TY  ) '
--SET @SQLCOMMAND=@SQLCOMMAND+RTRIM(@FCON)
--SET @SQLCOMMAND=@SQLCOMMAND+' ORDER BY IPITEM.DATE,IPITEM.ITEM'
EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

