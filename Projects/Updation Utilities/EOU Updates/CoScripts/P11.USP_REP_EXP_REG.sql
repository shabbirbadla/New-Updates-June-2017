
/****** Object:  StoredProcedure [dbo].[USP_REP_EXP_REG]    Script Date: 02/05/2010 10:03:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











---<<    USP_REP_EXP_REG This Stored Procedure useful to generate Export EXPORT Register  Report   >>

CREATE  PROCEDURE [dbo].[USP_REP_EXP_REG] 
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
,@VMAINFILE='STMAIN',@VITFILE='STITEM',@VACFILE='STACDET'
,@VDTFLD ='DATE'
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

DECLARE @SQLCOMMAND NVARCHAR(4000), @VCOND NVARCHAR(2000)

SET @SQLCOMMAND='select stmain.entry_ty,stmain.date,stmain.cate,stmain.u_fdesti,stmain.u_exare1,stmain.inv_no,stitem.qty,stitem.rate,stitem.u_fob,stmain.u_cseal,stitref.rinv_no,stmain.dept,stitem.u_exchange,stitem.u_asseamt,stitem.u_examt,stitem.u_acamt,stitem.u_acamt1,stitem.u_hacamt1,stitem.u_impamt,stitem.u_cessamt,stitem.u_hcessamt from stitem'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' inner join stmain on(stmain.tran_cd=stitem.tran_cd)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' inner join stitref on(stmain.tran_cd=stITREF.tran_cd)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON) 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'Order by stmain.date,stmain.entry_ty,STMAIN.DOC_NO'




EXECUTE SP_EXECUTESQL @SQLCOMMAND

IF @@ERROR = 0
BEGIN
PRINT 'USP_REP_EXP_REG Successfully Created..!'
END



