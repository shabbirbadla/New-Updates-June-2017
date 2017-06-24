
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[USP_REP_B17_REG] 
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
,@VMAINFILE=NULL,@VITFILE=NULL,@VACFILE=NULL
,@VDTFLD ='DATE'
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

DECLARE @SQLCOMMAND NVARCHAR(4000), @VCOND NVARCHAR(2000)
--SET @VB17ACNM ='BALANCE WITH B17-BOND              '
--SET @VBETY1 ='PT'
--SET @VBETY2 ='II'
--PT--
SET @SQLCOMMAND='select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,PTMAIN.[RULE],ptmain.gro_amt,ptmain.inv_no as inv_no,lac_vw.inv_no as inv_no1,ptmain.u_b1no,ptmain.tot_examt,ptmain.narr,ptmain.u_pinvno,ptmain.u_pinvdt
				INTO TB1 from lac_vw 
				LEFT join ptmain on (lac_vw.tran_cd=ptmain.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND              '' and lac_vw.entry_ty=''PT'''
EXECUTE SP_EXECUTESQL @SQLCOMMAND
--II-
SET @SQLCOMMAND='INSERT INTO  TB1 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,IIMAIN.[RULE],IIMAIN.gro_amt,IIMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,IIMAIN.u_b1no,IIMAIN.tot_examt,IIMAIN.narr,IIMAIN.u_pinvno,IIMAIN.u_pinvdt
				from lac_vw 
				LEFT join IIMAIN on (lac_vw.tran_cd=IIMAIN.tran_cd)
				where lac_vw.ac_name =''INTER DEPT TRANSFER                               '' and lac_vw.entry_ty=''II'''

EXECUTE SP_EXECUTESQL @SQLCOMMAND
--IR--
SET @SQLCOMMAND='INSERT INTO  TB1 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,IRMAIN.[RULE],IRMAIN.gro_amt,IRMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,IRMAIN.u_b1no,IRMAIN.tot_examt,IRMAIN.narr,IRMAIN.u_pinvno,IRMAIN.u_pinvdt
				from lac_vw 
				LEFT join IRMAIN on (lac_vw.tran_cd=IRMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND              '' and lac_vw.entry_ty=''IR'''

EXECUTE SP_EXECUTESQL @SQLCOMMAND
--BC-
SET @SQLCOMMAND='INSERT INTO  TB1 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,IIMAIN.[RULE],IIMAIN.gro_amt,IIMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,IIMAIN.u_b1no,lac_vw.amount as tot_examt,IIMAIN.narr,IIMAIN.u_pinvno,IIMAIN.u_pinvdt
				from lac_vw 
				LEFT join IIMAIN on (lac_vw.tran_cd=IIMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND              '' and lac_vw.entry_ty=''BC'''

EXECUTE SP_EXECUTESQL @SQLCOMMAND


--OB--
SET @SQLCOMMAND='INSERT INTO  TB1 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,OBMAIN.[RULE],OBMAIN.gro_amt,OBMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,OBMAIN.u_b1no,OBMAIN.tot_examt,OBMAIN.narr,OBMAIN.u_pinvno,OBMAIN.u_pinvdt
				from lac_vw 
				LEFT join OBMAIN on (lac_vw.tran_cd=OBMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND              '' and lac_vw.entry_ty=''OB'''

EXECUTE SP_EXECUTESQL @SQLCOMMAND

--EI--
SET @SQLCOMMAND='INSERT INTO  TB1 
				select EIACDET.entry_ty,EIACDET.date,EIACDET.ac_name,EIACDET.amount,EIACDET.amt_ty,EIACDET.dept,EIACDET.cate,
				EIACDET.u_cldt,EIMAIN.[RULE],EIMAIN.gro_amt,EIMAIN.inv_no as inv_no,EIACDET.inv_no as inv_no1,EIMAIN.u_b1no,EIMAIN.tot_examt,EIMAIN.narr,EIMAIN.u_pinvno,EIMAIN.u_pinvdt
				from EIACDET 
				LEFT join EIMAIN on (EIACDET.tran_cd=EIMAIN.tran_cd)
				where EIACDET.ac_name=EIMAIN.party_nm and EIACDET.entry_ty=''EI'''

EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND='SELECT * FROM TB1 where u_b1no > 0 order by u_b1no,date'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND='DROP TABLE TB1'
EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

