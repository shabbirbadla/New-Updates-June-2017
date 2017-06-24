set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



-- =============================================
-- Author:		Shrikant S.
-- Create date: 05 Feb, 2010
-- Description:	This Stored procedure is useful to Calculate Interest on Loan .
-- Modify date: 
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================

ALTER PROCEDURE [dbo].[USP_REP_LOANINT]  
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(200)
AS
Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000),@DIFFDAY as numeric(5)
DECLARE @GRPID AS INT,@MCOND AS BIT,@LVL  AS INT,@GRP AS VARCHAR(100)
Declare @LYN1 VARCHAR(10),@LYN2 VARCHAR(10),@LYN3 VARCHAR(10),@LYN4 VARCHAR(10),@LYN5 VARCHAR(10),@tmpDate smalldatetime 

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=null,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='MN',@VITFILE=Null,@VACFILE='AC'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

SELECT 
AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY,AC.ACSERIAL
,MN.L_YN,MN.DUE_DT
,AC_MAST.I_RATE,BHENT=L.ENTRY_TY
,AC_MAST.AC_ID,AC_MAST.AC_NAME,MN.U_NATURE
INTO #TMPAC_BAL1
FROM LAC_VW AC
INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)
INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) 
INNER JOIN LCODE L ON (MN.ENTRY_TY=L.ENTRY_TY)
WHERE 1=2

SET @DIFFDAY = convert(int,@edate) - convert(int,@sdate) + 1
--print @DIFFDAY
CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
SET @LVL=0
INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME in ('SECURED LOANS','UNSECURED LOANS')
SET @MCOND=1
WHILE @MCOND=1
BEGIN
	IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
	BEGIN
		INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
		SET @LVL=@LVL+1
	END
	ELSE
	BEGIN
		SET @MCOND=0	
	END
END

SELECT AC_ID,AC_NAME INTO #ACMAST FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)

SELECT ENTRY_TY,BHENT=(CASE WHEN EXT_VOU=0 THEN ENTRY_TY ELSE BCODE_NM END) INTO #LCODE FROM LCODE


SET @SQLCOMMAND='INSERT INTO #TMPAC_BAL1 SELECT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY,AC.ACSERIAL'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',MN.L_YN,MN.DUE_DT,AC_MAST.I_RATE,L.BHENT'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC_MAST.AC_ID,AC_MAST.AC_NAME,MN.U_NATURE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'FROM LAC_VW AC'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN #ACMAST ON (#ACMAST.AC_ID=AC.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN #LCODE L ON (AC.ENTRY_TY=L.ENTRY_TY)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND
 
--
--DELETE FROM #TMPAC_BAL1 WHERE 
--	DATE < (SELECT TOP 1 DATE FROM #TMPAC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN)
--	AND AC_NAME IN (SELECT AC_NAME FROM #TMPAC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN GROUP BY AC_NAME) 

SELECT AC_NAME,AC_ID,ACSERIAL,TRAN_CD,ENTRY_TY,DATE,DUE_DT,I_RATE,AMT_TY,U_NATURE/*row_number() over (order by AC_ID,date,Case when Entry_ty='OB' THEN 'A' ELSE 'B' END ) as rownum*/
	,OPBAL1=(CASE WHEN (BHENT='OB' ) THEN (CASE WHEN AMT_TY='DR'  THEN -AMOUNT ELSE +AMOUNT END) ELSE 0 END)
	,OPBAL2=(CASE WHEN (BHENT<>'OB' AND DATE < @SDATE) AND RTRIM(LTRIM(U_NATURE))<>'INTEREST' THEN (CASE WHEN AMT_TY='DR'  THEN -AMOUNT ELSE AMOUNT END) ELSE 0 END)
	,CAMT1=(CASE WHEN NOT (BHENT='OB' OR DATE < @SDATE) AND DATE <= @SDATE AND AMT_TY='CR' THEN AMOUNT ELSE 0 END)
	,CAMT2=(CASE WHEN NOT (BHENT='OB' OR DATE < @SDATE) AND DATE >  @SDATE AND AMT_TY='CR' THEN AMOUNT ELSE 0 END)
	,DAMT1=(CASE WHEN NOT (BHENT='OB' OR DATE < @SDATE) AND DATE <= @SDATE AND AMT_TY='DR' AND RTRIM(LTRIM(U_NATURE))<>'INTEREST'   THEN -AMOUNT ELSE 0 END)
	,DAMT2=(CASE WHEN NOT (BHENT='OB' OR DATE < @SDATE) AND DATE >  @SDATE AND AMT_TY='DR' AND RTRIM(LTRIM(U_NATURE))<>'INTEREST'   THEN -AMOUNT ELSE 0 END)
	,OpInt=(CASE WHEN NOT (BHENT='OB' OR DATE > @SDATE) AND DATE <= @SDATE AND AMT_TY='DR' AND RTRIM(LTRIM(U_NATURE))='INTEREST' THEN -AMOUNT ELSE 0 END)
	,CurInt=(CASE WHEN NOT (BHENT='OB' OR DATE < @SDATE) AND DATE > @SDATE AND AMT_TY='DR' AND RTRIM(LTRIM(U_NATURE))='INTEREST'   THEN -AMOUNT ELSE 0 END)
	,Days=0,toDate=Case when Date <@sdate then @sdate else @Edate end
Into #TMPAC_BAL2 FROM #TMPAC_BAL1	
WHERE DATE <= @EDATE 
--and l_yn=@LYN
ORDER BY AC_NAME,AC_ID,DATE,AMT_TY 

--select * from #TMPAC_BAL2 

--SELECT * FROM #TMPAC_BAL2 Order by ac_name,date,case when amt_ty='CR' then 1 else 2 end

select AC_NAME,AC_ID,DATE,I_RATE,TODATE,ENTRY_TY
	,OPBAL=SUM(OPBAL1)+SUM(OPBAL2),CAMT=SUM(CAMT1)+SUM(CAMT2),DAMT=SUM(DAMT1)+SUM(DAMT2),INT_Paid=SUM(OPINT)+SUM(CURINT)
	,CLBAL =convert(Numeric(18,2),0), DAYS=0, Interest=convert(Numeric(18,2),0.00), BALINT=convert(Numeric(18,2),0.00), PERIOD=CAST('' AS VARCHAR)
	,LYN=(CASE WHEN DATE BETWEEN CAST('01/01/'+CAST(YEAR(DATE) AS VARCHAR) AS SMALLDATETIME) AND CAST('01/01/'+CAST(YEAR(DATE) AS VARCHAR) AS SMALLDATETIME)  
			THEN CAST(YEAR(DATE)-1 AS VARCHAR)+'-'+CAST(YEAR(DATE) AS VARCHAR) ELSE CAST(YEAR(DATE) AS VARCHAR)+'-'+CAST(YEAR(DATE)+1 AS VARCHAR) END)  
INTO  #TMPAC_BAL3 
from #TMPAC_BAL2 
	GROUP BY AC_NAME,AC_ID,DATE,I_RATE,TODATE,ENTRY_TY
	Order by Ac_Name,Date

--Select * from #TMPAC_BAL3 
Update #TMPAC_BAL3 set i_Rate=a.intRate From InterestRateDetail a inner Join #TMPAC_BAL3 b on (a.Ac_id=b.ac_id and b.date between a.intfrom and a.intTo)	--Added by Shrikant S. on 29/10/2013 for Bug-11974		

-- Added By Sachin N. S.
Declare @Ac_Name Varchar(50),@Date Smalldatetime,@mAc_Name Varchar(60),@mDate Smalldatetime,@calDate SmallDatetime

Declare @sumClBal Numeric(18,2),@sumOpBal Numeric(18,2),@sumInt Numeric(18,2),@AC_NAME1 VARCHAR(50)
DECLARE @OPBAL Numeric(18,2), @Camt Numeric(18,2),@Damt Numeric(18,2), @LYN11 VARCHAR(9), @LYN12 VARCHAR(9), @IRATE NUMERIC(6,2), @DAYS NUMERIC(5), 
		@INT_PAY NUMERIC(18,2), @INT_BAL NUMERIC(18,2), @TOT_DAYS NUMERIC(5), @INT_PAID NUMERIC(18,2)

DECLARE @DATEC SMALLDATETIME, @AC_ID INT
DECLARE @INTRATE NUMERIC(5,2) --Added by Shrikant S. on 29/10/2013 for Bug-11974	
Declare Cursor1 cursor for
SELECT AC_NAME,AC_ID,I_RATE,DATE=MIN(DATE) FROM #TMPAC_BAL3 GROUP BY AC_NAME,AC_ID,I_RATE Order by Ac_Name,Date 

Open Cursor1
Fetch Next From Cursor1 Into @Ac_Name,@AC_ID,@IRATE,@Date
While @@Fetch_Status=0
Begin
	SET @DATEC = CAST('04/01/'+CAST(YEAR(@DATE) AS VARCHAR) AS SMALLDATETIME) 
	PRINT @DATEC
	  WHILE (SELECT @DATEC) < @EDATE 
		BEGIN
            iF @DATEC > @DATE and  @DATEC<>@SDATE--OR @DATEC 
            BEGIN
				SELECT @INTRATE=intRate FROM InterestRateDetail WHERE @DATEC BETWEEN INTFROM AND INTTO		--Added by Shrikant S. on 29/10/2013 for Bug-11974	
				SET @IRATE=CASE WHEN ISNULL(@INTRATE,0)>0 THEN @INTRATE ELSE @IRATE END						--Added by Shrikant S. on 29/10/2013 for Bug-11974
				
				INSERT INTO #TMPAC_BAL3 
				select AC_NAME=@Ac_Name, AC_ID=@AC_ID, DATE=@DATEC, I_RATE=@IRATE, TODATE='',ENTRY_TY='A', OPBAL=0, CAMT=0, DAMT=0, INT_Paid=0, CLBAL=0, DAYS=0,Interest=0, BALINT=0
					,PERIOD=CAST('' AS VARCHAR)
	 				,LYN=(CASE WHEN @DATEC BETWEEN CAST('01/01/'+CAST(YEAR(@DATEC) AS VARCHAR) AS SMALLDATETIME) AND CAST('01/01/'+CAST(YEAR(@DATEC) AS VARCHAR) AS SMALLDATETIME)  
							THEN CAST(YEAR(@DATEC)-1 AS VARCHAR)+'-'+CAST(YEAR(@DATEC) AS VARCHAR) ELSE CAST(YEAR(@DATEC) AS VARCHAR)+'-'+CAST(YEAR(@DATEC)+1 AS VARCHAR) END)
			END
			SET @DATEC = CAST('04/01/'+CAST(YEAR(@DATEC)+1 AS VARCHAR) AS SMALLDATETIME)
			CONTINUE
		END
            IF @DATEC=@SDATE or @SDATE<@DATE
            BEGIN
				PRINT ''
			END
			ELSE
            BEGIN
				SELECT @INTRATE=intRate FROM InterestRateDetail WHERE @SDATE BETWEEN INTFROM AND INTTO		--Added by Shrikant S. on 29/10/2013 for Bug-11974	
				SET @IRATE=CASE WHEN ISNULL(@INTRATE,0)>0 THEN @INTRATE ELSE @IRATE END						--Added by Shrikant S. on 29/10/2013 for Bug-11974
				
 				INSERT INTO #TMPAC_BAL3 
				select AC_NAME=@Ac_Name, AC_ID=@AC_ID, DATE=@SDATE, I_RATE=@IRATE, TODATE='',ENTRY_TY='A', OPBAL=0, CAMT=0, DAMT=0, INT_Paid=0, CLBAL=0, DAYS=0,Interest=0, BALINT=0
					,PERIOD=CAST('' AS VARCHAR)
	 				,LYN=(CASE WHEN @SDATE BETWEEN CAST('01/01/'+CAST(YEAR(@SDATE) AS VARCHAR) AS SMALLDATETIME) AND CAST('01/01/'+CAST(YEAR(@SDATE) AS VARCHAR) AS SMALLDATETIME)  
							THEN CAST(YEAR(@SDATE)-1 AS VARCHAR)+'-'+CAST(YEAR(@SDATE) AS VARCHAR) ELSE CAST(YEAR(@SDATE) AS VARCHAR)+'-'+CAST(YEAR(@SDATE)+1 AS VARCHAR) END)  
             END
Fetch Next From Cursor1 Into @Ac_Name,@AC_ID,@IRATE,@Date
End
Close Cursor1
DEALLOCATE Cursor1


--SELECT * FROM #TMPAC_BAL3 ORDER BY AC_NAME,DATE

DELETE FROM #TMPAC_BAL3 WHERE AC_NAME IN (SELECT DISTINCT B.AC_NAME FROM #TMPAC_BAL3 B WHERE B.ENTRY_TY<>'A' AND B.DATE=@SDATE)
	AND ENTRY_TY='A' AND DATE=@SDATE

set @sumClBal=0
set @sumOpBal=0
set @sumInt =0
set @OPBAL =0
SET @INT_PAY=0
SET @INT_BAL=0

Declare accCursor cursor for 
Select Ac_Name,Date,LYN,I_RATE,OPBAL,CAMT,DAMT,INT_PAID From #TMPAC_BAL3 Order by Ac_Name,Date

Open accCursor
Fetch Next From accCursor Into @Ac_Name,@Date,@LYN11,@IRATE,@OPBAL,@CAMT,@DAMT,@INT_PAID 
While @@Fetch_Status=0
Begin
	set @mDate=0
	PRINT @LYN11
	PRINT @LYN12

	IF @AC_NAME = @AC_NAME1
		BEGIN
			set @ac_name1=@ac_name	
		END
	ELSE
		BEGIN
			set @sumClBal=0
			set @sumOpBal=0
			--set @sumInt =0
			SET @AC_NAME1 = @AC_NAME
			SET @LYN12 = ''
			SET @INT_PAY=0--Added by Archana
			SET @INT_BAL=0--Added by Archana
		END

	Select top 1 @mDate=Date From #TMPAC_BAL3 Where Ac_Name=@Ac_Name AND Date >@Date Order by Date
--    PRINT @mDate
    
   
	set @calDate=case when year(@mDate)<=1900 then dateadd(day,1,@eDate) else @mDate end
    
	Update #TMPAC_BAL3 set toDate=dateadd(day,-1,@calDate),days=datediff(d,date,dateadd(day,-1,@calDate))+1,
			PERIOD = CONVERT(VARCHAR(10),DATE,103)+'-'+CONVERT(VARCHAR(10),dateadd(day,-1,@calDate),103)
		Where Ac_name=@Ac_Name AND Date =@Date 

--	PRINT @DATE
--
--	PRINT @sumOpBal

	IF RTRIM(@LYN11) = RTRIM(@LYN12) 
		BEGIN
			IF @DATE<@SDATE
				BEGIN
					SET @sumOpBal=@sumClBal+@OPBAL+abs(@Camt)-abs(@Damt)
					SET @sumClBal=@sumOpBal
				END
			ELSE
				BEGIN
					SET @sumOpBal=@sumClBal
					SET @sumClBal=@sumOpBal+@OPBAL+abs(@Camt)-abs(@Damt)
				END
			PRINT 'B'
		END
	ELSE
		BEGIN
			SET @LYN12= @LYN11
			SET @sumOpBal=@sumClBal+@OPBAL+abs(@Camt)-abs(@Damt)
			SET @sumOpBal=@sumOpBal+@INT_BAL
			SET @INT_BAL=0

			PRINT @sumOpBal

            Select top 1 @mDate=Date From #TMPAC_BAL3 Where Ac_Name=@Ac_Name Order by Date
			IF @DATE<=@SDATE OR @SDATE<=@mDate
				BEGIN
					PRINT '1'
					SET @sumClBal=@sumOpBal
				END
			ELSE
				BEGIN
					PRINT '2'
					--SET @sumClBal=@sumOpBal+@OPBAL+abs(@Camt)-abs(@Damt)		--Commented By Shrikant S. on 29/10/2013 for Bug-11974
					SET @sumClBal=@sumOpBal+@OPBAL-abs(@Damt)		--Added By Shrikant S. on 29/10/2013 for Bug-11974
				END
			PRINT 'A'
		END

	
	PRINT @sumOpBal
	PRINT @INT_BAL

	SET @DAYS=datediff(d,@date,dateadd(day,-1,@calDate))+1
	SELECT @TOT_DAYS=DBO.DaysofYear(dbo.finYear(@Date))
	SET @INT_PAY = ((@sumClBal * @IRATE/100)/@TOT_DAYS)* @Days 
	SET @INT_BAL = @INT_BAL + @INT_PAY + @INT_PAID
	
	UPDATE #TMPAC_BAL3 set OPBAL=@sumOpBal,
			CLBAL=@sumClBal, Interest=@INT_PAY, BALINT=@INT_BAL
		Where Ac_name=@Ac_Name AND Date = @Date 

	Fetch Next From accCursor Into @Ac_Name,@Date,@LYN11,@IRATE,@OPBAL,@CAMT,@DAMT,@INT_PAID 

End
Close accCursor
DEALLOCATE accCursor

--SELECT * FROM #TMPAC_BAL3 

DELETE FROM #TMPAC_BAL3 WHERE DATE<@SDATE
SELECT * FROM #TMPAC_BAL3 ORDER BY AC_NAME,DATE


DROP TABLE #TMPAC_BAL1
DROP TABLE #TMPAC_BAL2 
DROP TABLE #TMPAC_BAL3


