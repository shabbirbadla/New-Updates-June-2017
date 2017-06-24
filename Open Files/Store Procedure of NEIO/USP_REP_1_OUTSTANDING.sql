set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate ACCOUNTS 1key Outstanding  Report for sundry creditors.
-- Modify date: 16/05/2007
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================


ALTER     PROCEDURE [dbo].[USP_REP_1_OUTSTANDING]  
@EDATE  SMALLDATETIME,@SAC  AS VARCHAR(100),@EAC  VARCHAR(100) ,@DAYS1 INT,@DAYS2 INT,@DAYS3 INT
AS
--Declare @EDATE AS SMALLDATETIME,@SAC  AS VARCHAR(100),@EAC AS VARCHAR(100) --@SDATE AS SMALLDATETIME,
--SET @EDATE=GETDATE()
--SET @SAC='A'
--SET @EAC='Z'

--DECLARE @DAYS1 AS INT,@DAYS2 AS INT,@DAYS3 AS INT
--SET @DAYS1=60
--SET @DAYS2=70
--SET @DAYS3=90

  

DECLARE @AC_ID NUMERIC(9),@AC_GROUP_ID1 NUMERIC(9),@GNAME1 VARCHAR(60),@AC_GROUP_ID2 NUMERIC(9),@GNAME2 VARCHAR(60),@AC_GROUP_ID3 NUMERIC(9),@GNAME3 VARCHAR(60)
SELECT DISTINCT AC_ID=AC_GROUP_ID,AC_GROUP_ID1=AC_GROUP_ID,AC_GROUP_ID2=AC_GROUP_ID,AC_GROUP_ID3=AC_GROUP_ID,GNAME1=[GROUP],GNAME2=[GROUP],GNAME3=[GROUP] INTO #1JRTMP FROM AC_MAST WHERE 1=2


DECLARE  C1JRTMP CURSOR FOR
SELECT  DISTINCT AC_ID FROM AC_MAST
ORDER BY AC_ID
OPEN C1JRTMP
FETCH NEXT FROM C1JRTMP INTO @AC_ID
WHILE @@FETCH_STATUS=0
BEGIN
	SELECT @AC_GROUP_ID1=AC_GROUP_ID,@GNAME1=[GROUP] FROM AC_MAST WHERE AC_ID=@AC_ID
	SELECT @AC_GROUP_ID2=GAC_ID,@GNAME2=[GROUP] FROM AC_GROUP_MAST WHERE AC_GROUP_ID=@AC_GROUP_ID1
	SELECT @AC_GROUP_ID3=GAC_ID,@GNAME3=[GROUP] FROM AC_GROUP_MAST WHERE AC_GROUP_ID=@AC_GROUP_ID2
	INSERT INTO #1JRTMP ( AC_ID,AC_GROUP_ID1,AC_GROUP_ID2,AC_GROUP_ID3,GNAME1,GNAME2,GNAME3) VALUES (@AC_ID,@AC_GROUP_ID1,@AC_GROUP_ID2,@AC_GROUP_ID3,@GNAME1,@GNAME2,@GNAME3)
	FETCH NEXT FROM C1JRTMP INTO @AC_ID
END
CLOSE C1JRTMP
DEALLOCATE C1JRTMP



DECLARE @GRPID AS INT,@MCOND AS BIT,@LVL  AS INT,@SQLCOMMAND NVARCHAR(200),@GRP AS VARCHAR(100)
SET @GRP='SUNDRY CREDITORS'

CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
SET @LVL=0
INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME=@GRP
SET @MCOND=1
WHILE @MCOND=1
BEGIN
	IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
	BEGIN
		PRINT @LVL
		INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
		SET @LVL=@LVL+1
	END
	ELSE
	BEGIN
		SET @MCOND=0	
	END
END

SELECT AC_ID,AC_NAME INTO #ACMAST FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)

--SELECT (CASE WHEN EXT_VOU=0 THEN ENTRY_TY ELSE BCODE_NM END) FROM LCODE WHERE (CASE WHEN EXT_VOU=0 THEN ENTRY_TY ELSE BCODE_NM END) IN ('EP')

SELECT M.U_PINVNO as [Bill No.],M.U_PINVDT as [Bill Date],M.DUE_DT as [Due Date],M.INV_NO as [Ref.No.],M.Date as [Ref. Date],AC.AC_NAME as [Account Name]
,AC.Amount AS [Bill Amount],amt_ty as [Dr/Cr],(ML.NEW_ALL+ML.TDS) AS [Received Amt.],
[Balance Amount]=  (AC.AMOUNT-(CASE WHEN ML.NEW_ALL IS NULL THEN 0 ELSE ML.NEW_ALL END)+(CASE WHEN ML.TDS IS NULL THEN 0 ELSE ML.TDS END)) 
,DATEDIFF(DD,M.DUE_DT,@EDATE) as [Pending Days],AC.ENTRY_TY,AC.TRAN_CD
,T.gname1 as [Group Level1],T.gname2 as [Group Level2],T.gname3 as [Group Level3]
,M.cate As [Cate  ry],M.dept as [Department],M.inv_sr as [Invoice Series],m.[Rule]
,[Period]=CASE WHEN (@EDATE-M.DUE_DT BETWEEN 0 AND @DAYS1) THEN REPLICATE(' ',4-len(cast(@days1 as int)))+LTRIM(STR(@DAYS1))+' '+'Days'
                	 WHEN (@EDATE-M.DUE_DT BETWEEN @DAYS1+1 AND @DAYS2) THEN REPLICATE(' ',4-len(cast(@days2 as int)))+LTRIM(STR(@DAYS2))+' '+'Days'
		 WHEN (@EDATE-M.DUE_DT BETWEEN @DAYS2+1 AND @DAYS3)  THEN REPLICATE(' ',4-len(cast(@days3 as int)))+LTRIM(STR(@DAYS3))+' '+'Days'
		 ELSE '>'+LTRIM(STR(@DAYS3))+' '+'Days'
		 END			
FROM LAC_VW AC
INNER JOIN LMAIN_VW M ON (M.ENTRY_TY=AC.ENTRY_TY AND M.TRAN_CD=AC.TRAN_CD)
INNER JOIN #ACMAST AM  ON (AC.AC_ID=AM.AC_ID)
INNER JOIN LCODE L ON (AC.ENTRY_TY=L.ENTRY_TY)
INNER JOIN #1JRTMP T ON (AC.AC_ID=T.AC_ID)
LEFT JOIN MAINALL_VW ML ON (M.entry_ty=ML.entry_all and M.tran_cd =ML.main_tran and M.PARTY_NM=AC.AC_NAME)
WHERE (AC.AMOUNT-(CASE WHEN ML.NEW_ALL IS NULL THEN 0 ELSE ML.NEW_ALL END)+(CASE WHEN ML.TDS IS NULL THEN 0 ELSE ML.TDS END))>0 
AND  (AC.AC_NAME BETWEEN @SAC AND @EAC)
AND  (M.DUE_DT <= @EDATE)
AND (CASE WHEN L.EXT_VOU=0 THEN L.ENTRY_TY ELSE L.BCODE_NM END) IN ('EP','PT')
ORDER BY AC.AC_NAME,AC.ENTRY_TY,AC.TRAN_CD,PERIOD

--

