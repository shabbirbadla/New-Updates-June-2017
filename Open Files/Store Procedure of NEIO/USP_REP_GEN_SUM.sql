If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_GEN_SUM'))
Begin
	Drop Procedure USP_REP_GEN_SUM
End
Go


set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



-- =============================================
-- AUTHOR:		RUEPESH PRAJAPATI.
-- CREATE DATE: 16/05/2007
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE ACCOUNTS GENERAL SUMMARY REPORT.
-- MODIFY DATE: 16/05/2007
-- MODIFIED BY/DATE/REMARK: Changes done by Sandeep on 03-Sept-13 for the bug-18655
-- =============================================
Create PROCEDURE  [dbo].[USP_REP_GEN_SUM]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@FINYR VARCHAR(60)
,@EXPARA  AS VARCHAR(60)= null


AS
SET QUOTED_IDENTIFIER OFF

--DECLARE @FCON AS NVARCHAR(2000),@SQLCOMMAND AS NVARCHAR(4000)
--Added by sandeep for bug-18655-->S
Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000),@OPENTRY_TY as VARCHAR(50),@OPENTRIES as VARCHAR(50)
	PRINT 'RR1 '+@FINYR
	Set @OPENTRY_TY = '''OB'''
	DECLARE openingentry_cursor CURSOR FOR
		SELECT entry_ty FROM lcode
		WHERE bcode_nm = 'OB'
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @opentries
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   Set @OPENTRY_TY = @OPENTRY_TY +','''+@opentries+''''
	   FETCH NEXT FROM openingentry_cursor into @opentries
	END
	CLOSE openingentry_cursor
	DEALLOCATE openingentry_cursor
--Added by sandeep for bug-18655--->E

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=NULL 
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='MN',@VITFILE=Null,@VACFILE='AC'
,@VDTFLD ='DATE'
,@VLYN=NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

SELECT 
AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY
,MN.L_YN,AC_MAST.[TYPE]
,AC_MAST.AC_ID,AC_MAST.AC_NAME
INTO #AC_BAL1
FROM LAC_VW AC
INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)
INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) 
WHERE 1=2
--Added by sandeep for bug-18655--->S

SET @SQLCOMMAND='INSERT INTO #AC_BAL1 SELECT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',MN.L_YN,ac_mast.[TYPE]'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC_MAST.AC_ID,AC_MAST.AC_NAME'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'FROM LAC_VW AC'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND


SET @SQLCOMMAND='DECLARE @OPTRAN_CD as INT,@OPDATE as DATETIME,@OPACNAME as varchar(250) DECLARE openingentry_cursor CURSOR FOR'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT TRAN_CD,AC_NAME,DATE FROM #AC_BAl1'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'WHERE 	ENTRY_TY IN ('+@OPENTRY_TY+') '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'OPEN openingentry_cursor'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'WHILE @@FETCH_STATUS = 0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'BEGIN'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'DELETE FROM #AC_BAL1 WHERE DATE < @OPDATE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND AC_NAME IN (SELECT AC_NAME FROM #AC_BAL1 WHERE AC_NAME = @OPACNAME AND ENTRY_TY IN (''OB'') AND TRAN_CD = @OPTRAN_CD )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'END'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'CLOSE openingentry_cursor'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'DEALLOCATE openingentry_cursor'
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND = 'DELETE FROM #AC_BAL1'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'WHERE L_YN !='+CHAR(39)+@FINYR+CHAR(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND [TYPE] !=''B'''
print @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

--Added by sandeep for bug-18655--->E


/*
DELETE FROM #AC_BAL1 WHERE 
DATE < (SELECT TOP 1 DATE FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN)
AND AC_NAME IN (SELECT AC_NAME FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN GROUP BY AC_NAME) 

*/



SELECT L_YN,MNTH=MONTH(DATE),YR=YEAR(DATE),AC_NAME,AC_ID
,OPBAL=SUM(CASE WHEN (ENTRY_TY='OB' OR DATE<@SDATE) THEN (CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END) ELSE 0 END)
,DAMT=SUM(CASE WHEN NOT (ENTRY_TY='OB' OR DATE<@SDATE) AND AMT_TY='DR' THEN AMOUNT ELSE 0 END)
,CAMT=SUM(CASE WHEN NOT (ENTRY_TY='OB' OR DATE<@SDATE) AND AMT_TY='CR' THEN AMOUNT ELSE 0 END)
,BALAMT=SUM(CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END)
,OPAMT_TY=SPACE(2)
,CLAMT_TY=SPACE(2)
INTO #AC_BAL
FROM #AC_BAL1
WHERE 1=2
GROUP BY AC_NAME,AC_ID,L_YN,MONTH(DATE),YEAR(DATE)
ORDER BY AC_NAME,AC_ID






--LOOP FOR MONTH AND YEAR CALCULATION
DECLARE @SDATE1 SMALLDATETIME,@EDATE1 SMALLDATETIME,@STRDT VARCHAR(10)
DECLARE @C INT,@CNT INT
SET @C=0
SET @CNT=0

SET @SDATE1=@SDATE
SET @EDATE1=@SDATE1

WHILE (@C=0)
BEGIN
	IF(MONTH(@SDATE1) IN (1,3,5,7,8,10,12))
	BEGIN
		
		SET @STRDT=CAST( YEAR(@SDATE1) AS VARCHAR(4)  )+'/'+CAST( MONTH(@SDATE1) AS VARCHAR(4)  )+'/31'
		SET @EDATE1=CAST( @STRDT AS SMALLDATETIME)
	END
	ELSE
	BEGIN
		IF(MONTH(@SDATE1) IN (4,6,9,11))
		BEGIN
			SET @STRDT=CAST( YEAR(@SDATE1) AS VARCHAR(4)  )+'/'+CAST( MONTH(@SDATE1) AS VARCHAR(4)  )+'/30'
			SET @EDATE1=CAST( @STRDT AS SMALLDATETIME)
		END
	END
	IF(MONTH(@SDATE1) IN (2))
	BEGIN
		SET @STRDT=CAST( YEAR(@SDATE1) AS VARCHAR(4)  )+'/'+CAST( MONTH(@SDATE1) AS VARCHAR(4)  )+'/29'
		IF ISDATE(@STRDT)=1
		BEGIN
			SET @EDATE1=CAST(@STRDT AS SMALLDATETIME)	
		END
		ELSE
		BEGIN
			SET @STRDT=CAST( YEAR(@SDATE1) AS VARCHAR(4)  )+'/'+CAST( MONTH(@SDATE1) AS VARCHAR(4)  )+'/28'
			SET @EDATE1=CAST( @STRDT AS SMALLDATETIME)	
		END	
	END

	
	INSERT INTO #AC_BAL
	SELECT L_YN,MNTH=MONTH(@EDATE1),YR=YEAR(@EDATE1),AC_NAME,AC_ID
	,OPBAL=SUM(CASE WHEN (ENTRY_TY='OB' OR DATE<@SDATE1) THEN (CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END) ELSE 0 END)
	,DAMT=SUM(CASE WHEN NOT (ENTRY_TY='OB' OR DATE<@SDATE1) AND AMT_TY='DR' THEN AMOUNT ELSE 0 END)
	,CAMT=SUM(CASE WHEN NOT (ENTRY_TY='OB' OR DATE<@SDATE1) AND AMT_TY='CR' THEN AMOUNT ELSE 0 END)
	,BALAMT=SUM(CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END)
	,OPAMT_TY=SPACE(2)
	,CLAMT_TY=SPACE(2)
	FROM #AC_BAL1
	WHERE DATE<=@EDATE1
	GROUP BY L_YN,AC_NAME,AC_ID--,MONTH(DATE),YEAR(EDATE1),
	ORDER BY AC_NAME,AC_ID


	PRINT 'C'
	PRINT @SDATE1	
	PRINT @EDATE1
	
	SET @SDATE1=DATEADD(DAY,1,@EDATE1)
	SET @CNT=@CNT+1
	IF(@EDATE1>=@EDATE)
	BEGIN
		SET @C=1--EXIT FROM THE LOOP
	END
END
--



UPDATE  #AC_BAL SET 
OPAMT_TY=(CASE WHEN OPBAL<0 THEN 'CR' ELSE 'DR' END)
,CLAMT_TY=(CASE WHEN BALAMT<0 THEN 'CR' ELSE 'DR' END)

UPDATE  #AC_BAL SET 
OPBAL=(CASE WHEN OPBAL<0 THEN OPBAL*(-1) ELSE OPBAL END)
,BALAMT=(CASE WHEN BALAMT<0 THEN BALAMT*(-1) ELSE BALAMT END)



SELECT * FROM #AC_BAL ORDER BY YR,MNTH,AC_NAME


