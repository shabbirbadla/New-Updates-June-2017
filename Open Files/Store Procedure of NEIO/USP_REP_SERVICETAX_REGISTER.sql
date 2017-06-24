-- =============================================
-- Author:		OmPrakash
-- Create date: 24/09/2011
-- Description:	This Stored procedure is useful to generate Service Tax Register Report.
-- Modification Date/By/Reason: Shrikant S. on 01-06-2016 for Bug-28132(Krishi Kalyan Cess)
-- =============================================



ALTER PROCEDURE [dbo].[USP_REP_SERVICETAX_REGISTER] 
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
BEGIN 

Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000)
	
	
	EXECUTE USP_REP_FILTCON 
	@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
	,@VSDATE=@SDATE,@VEDATE=@EDATE
	,@VSAC =@SAC,@VEAC =@EAC
	,@VSIT=@SIT,@VEIT=@EIT
	,@VSAMT=@SAMT,@VEAMT=@EAMT
	,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
	,@VSCATE =@SCATE,@VECATE =@ECATE
	,@VSWARE =@SWARE,@VEWARE  =@EWARE
	,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
	,@VMAINFILE='sbmain',@VITFILE=Null,@VACFILE='AC'
	,@VDTFLD ='DATE'
	,@VLYN=Null
	,@VEXPARA=@EXPARA
	,@VFCON =@FCON OUTPUT

	print @fcon	

	 set @sqlcommand='SELECT SBMAIN.PARTY_NM,AC_MAST.SREGN,SBMAIN.INV_NO,SBMAIN.DATE,SBMAIN.NET_AMT,ACDETALLOC.SERTY,ACDETALLOC.STAXABLE,'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+'SBITEM.ITEM,SBITEM.QTY,SBITEM.RATE,SBITEM.GRO_AMT,SBITEM.SERBAMT,SBITEM.SERCAMT,SBITEM.SERHAMT,SBMAIN.ENTRY_TY'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+',SBITEM.SERBCPER,SBITEM.SERBCESS'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+',SBITEM.SKKCPER,SBITEM.SKKCAMT'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+'FROM SBMAIN'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+'INNER JOIN SBITEM ON (SBMAIN.TRAN_CD=SBITEM.TRAN_CD)'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+'INNER JOIN AC_MAST ON (AC_MAST.AC_ID=SBMAIN.AC_ID)'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+'INNER JOIN ACDETALLOC ON (SBITEM.eNTRY_TY=ACDETALLOC.ENTRY_TY AND SBITEM.TRAN_CD=ACDETALLOC.TRAN_CD AND SBITEM.ITSERIAL=ACDETALLOC.ITSERIAL)'
	 set @sqlcommand=rtrim(@sqlcommand)+' '+rtrim(@fcon)
     set @sqlcommand=rtrim(@sqlcommand)+' '+' and SBMAIN.ENTRY_TY=''S1'''

    print  @sqlcommand
	EXECUTE SP_EXECUTESQL @sqlcommand
	
END 




