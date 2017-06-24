set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
 -- Author:  Hetal L Patel
 -- Create date: 16/05/2007
 -- Description: This Stored procedure is useful to generate CH VAT FORM 15
 -- Modify date: 16/05/2007 
 -- Modified By: Madhavi Penumalli
 -- Modify date: 25/11/2009 
 -- =============================================
 -- Re-Modified By: Rakesh Varma
 -- Re-Modify date: 12-Feb-2009 (Updated)
 -- =============================================
ALTER PROCEDURE [dbo].[USP_REP_CH_FORM15]
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
 ,@VMAINFILE='M',@VITFILE=NULL,@VACFILE=NULL
 ,@VDTFLD ='DATE'
 ,@VLYN=NULL
 ,@VEXPARA=@EXPARA
 ,@VFCON =@FCON OUTPUT
 
 DECLARE @SQLCOMMAND NVARCHAR(4000)

 DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),
         @AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),
         @AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),
         @AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)

 DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),
         @AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),
         @AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),
         @AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)

 DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)
 
SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1)
INTO #VATAC_MAST 
FROM STAX_MAS 
WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''

INSERT INTO #VATAC_MAST 
SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1)
FROM STAX_MAS
WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #CH_FORM15
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #CH_FORM15 add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
INTO #CHFORM15
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2

Alter table #CHFORM15 ADD AMT4 numeric(12,2)

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		 Set @MultiCo = 'YES'
		 EXECUTE USP_REP_MULTI_CO_DATA
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo  #CH_FORM15 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 EXECUTE USP_REP_SINGLE_CO_DATA
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo  #CH_FORM15 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
        --SELECT * FROM #CH_FORM15
	End
-----
-----SELECT * from #CH_FORM15 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') 
----and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----
--->PART 1-5 
 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,
        @AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0

--1 a

---------------------------------------------------------------------
SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('ST','DN') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
VALUES (1,'1','A',0,@AMTA1,0,0,'')
---------------------------------------------------------------------

--1 B
---------------------------------------------------------------------
--BLANK VALUE

SELECT @AMTA1=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES (1,'1','B',0,@AMTA1,0,0,'')
---------------------------------------------------------------------

--1 C
---------------------------------------------------------------------
SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('ST','DN') AND ST_TYPE = 'OUT OF STATE' AND U_IMPORM <> 'Branch Transfer' AND
(DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'1','C',0,@AMTA1,0,0,'')
---------------------------------------------------------------------

--1 D

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('ST','DN') AND TAX_NAME IN ('NO-TAX','TAX FREE','') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES (1,'1','D',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--1 E

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('ST','DN') AND U_IMPORM = 'Branch Transfer' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'1','E',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--1 F

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('SR','CN') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'1','F',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--1 G

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(AMT1),0) FROM #CHFORM15
WHERE PARTSR = 1 AND SRNO IN ('A','B','C','D','E','F')

INSERT INTO #CHFORM15 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'1','G',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 A

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'2','A',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 B

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND ST_TYPE = 'OUT OF COUNTRY' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'2','B',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 C

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND ST_TYPE = 'OUT OF STATE' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'2','C',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 D

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND TAX_NAME = 'EXEMPTED' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'2','D',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 E

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND TAX_NAME IN ('NO-TAX','TAX FREE','') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES (1,'2','E',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 F

--BLANK

SELECT @AMTA1=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'2','F',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 G

SELECT @AMTA1=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PR','DN') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'2','G',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 H

--BLANK

SELECT @AMTA1=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES (1,'2','H',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--2 I

--BLANK

SELECT @AMTA1=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES (1,'2','I',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--3 A

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA2 = Round(SUM(TAXAMT),0) FROM #CH_FORM15
WHERE BHENT IN ('ST','DN') AND TAX_NAME LIKE '%VAT%' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'3','A',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--3 B

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'3','B',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--3 C


SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA2 = Round(SUM(TAXAMT),0) FROM #CH_FORM15
WHERE BHENT IN ('ST','DN') AND TAX_NAME <> '' AND ST_TYPE = 'OUT OF STATE' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'3','C',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--3 D

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'3','D',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--3 E

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'3','E',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--3 F

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA2 = Round(SUM(AMT2),0) FROM #CHFORM15
WHERE PARTSR = 3 AND SRNO IN ('A','B','C','D','E')

INSERT INTO #CHFORM15 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES (1,'3','F',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 A

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA2 = Round(SUM(TAXAMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND ITEMTYPE <> 'C' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','A',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 B

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA2 = Round(SUM(TAXAMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PT','EP','CN') AND ITEMTYPE = 'C' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','B',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 C

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','C',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 D

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','D',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 E

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','E',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 F

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','F',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 G

--BLANK

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','G',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--4 H

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA2 = Round(SUM(AMT2),0) FROM #CHFORM15
WHERE PARTSR = 4 AND SRNO IN ('A','B','C','D','E','F','G')

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'4','H',0,@AMTA1,@AMTA2,0,'')

---------------------------------------------------------------------

--5 A

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA1 = Round(SUM(AMT2),0) FROM #CHFORM15
WHERE PARTSR = 3 AND SRNO = 'F'

SELECT @AMTA2 = Round(SUM(AMT2),0) FROM #CHFORM15
WHERE PARTSR = 4 AND SRNO = 'H'

IF @AMTA1 > @AMTA2

BEGIN

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
               VALUES(1,'5','A',0,@AMTA1-@AMTA2,0,0,'')

END

ELSE

BEGIN

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
               VALUES(1,'5','A',0,0,0,0,'')

END

---------------------------------------------------------------------

--5 B

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA1 = Round(SUM(NET_AMT),0) FROM #CH_FORM15
WHERE BHENT='BP' AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'5','B',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--5 C

SELECT @AMTA1=0,@AMTA2=0

SELECT @AMTA1 = Round(SUM(AMT2),0) FROM #CHFORM15
WHERE PARTSR = 3 AND SRNO = 'F'

SELECT @AMTA2 = Round(SUM(AMT2),0) FROM #CHFORM15
WHERE PARTSR = 4 AND SRNO = 'H'

IF @AMTA2 > @AMTA1

BEGIN

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
               VALUES(1,'5','C',0,@AMTA2-@AMTA1,0,0,'')

END

ELSE

BEGIN

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
               VALUES(1,'5','C',0,0,0,0,'')

END

---------------------------------------------------------------------

--5 D

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'5','D',0,@AMTA1,0,0,'')

---------------------------------------------------------------------

--5 E

SELECT @AMTA1=0,@AMTA2=0

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'5','E',0,@AMTA1,0,0,'')

-------------------------------------------------------------------------------------------

--6

Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),
        @INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),
        @ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),
        @S_TAX as varchar(30),@QTY as numeric(18,4)


SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',
       @FORM_NM='',@S_TAX ='',@QTY=0

SET @CHAR=65

SET @PER = 0

declare Cur_VatPay cursor  for
select A.Taxonamt,A.Gro_amt,A.taxamt,A.INV_NO,B.Date,A.Party_nm,Address='',A.Form_nm,A.S_tax
from #CH_FORM15 A Inner join Bpmain B on (A.Bhent = B.Entry_ty and A.Tran_cd = B.Tran_cd)
where BHENT = 'BP' And A.Date Between @sdate and @edate

open Cur_VatPay

FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@ITEMAMT,@TAXAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX--,@item,@QTY,,@ITEMAMT
 
WHILE (@@FETCH_STATUS=0)
 BEGIN

	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END	

	INSERT INTO #CHFORM15 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
    VALUES (1,'6',CHAR(@CHAR),@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX)

 SET @CHAR=@CHAR+1
 FETCH NEXT FROM CUR_VatPay INTO @TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX--,@ITEM,@QTY
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay

-------------------------------------------------------------------------------------------

--7

DECLARE @AMTAA1 NUMERIC(12,2),@AMTBB1 NUMERIC(12,2),@AMTCC1 NUMERIC(12,2),@AMTDD1 NUMERIC(12,2),
        @AMTEE1 NUMERIC(12,2),@AMTFF1 NUMERIC(12,2),@AMTGG1 NUMERIC(12,2),@AMTHH1 NUMERIC(12,2),
        @AMTII1 NUMERIC(12,2),@AMTJJ1 NUMERIC(12,2),@AMTKK1 NUMERIC(12,2),@AMTLL1 NUMERIC(12,2),
        @AMTMM1 NUMERIC(12,2),@AMTNN1 NUMERIC(12,2),@AMTOO1 NUMERIC(12,2),@AMTFF2 NUMERIC(12,2),
        @AMTGG2 NUMERIC(12,2),@AMTHH2 NUMERIC(12,2)

Declare @NetEff1 as numeric (12,2), @NetTax1 as numeric (12,2)

SELECT @AMTAA1=0,@AMTBB1=0,@AMTCC1=0,@AMTDD1=0,@AMTEE1=0,@AMTFF1=0,@AMTGG1=0,@AMTHH1=0,
        @AMTII1=0,@AMTJJ1=0,@AMTKK1=0,@AMTLL1=0,@AMTMM1=0,@AMTNN1=0,@AMTOO1=0,
        @AMTFF2=0,@AMTGG2=0,@AMTHH2=0

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,
       @AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 

SET @CHAR=65

DECLARE  CUR_FORM221 CURSOR FOR 
select distinct level1 from stax_mas where ST_TYPE='LOCAL'--CHARINDEX('VAT',TAX_NAME)>0

OPEN CUR_FORM221

FETCH NEXT FROM CUR_FORM221 INTO @PER

WHILE (@@FETCH_STATUS=0)
 BEGIN
	if @per = 0
		begin
			SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #CH_FORM15 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' And ac_name not like '%Rece%' and U_imporm <> 'Purchase Return'
			SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM #CH_FORM15 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' And ac_name not like '%Rece%' and U_imporm <> 'Purchase Return'
			SELECT @AMTC1=Round(SUM(NET_AMT),0) FROM #CH_FORM15 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER 
			SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM #CH_FORM15 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER


			SELECT @AMTAA1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '') b
			SELECT @AMTBB1=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' 
			SELECT @AMTCC1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PR' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '') b
			SELECT @AMTDD1=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PR' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> ''
			SELECT @AMTFF1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='EP' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '') b
			SELECT @AMTFF2=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='EP' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> ''
			SELECT @AMTGG1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='DN' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '') b
			SELECT @AMTGG2=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='DN' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> ''
			SELECT @AMTHH1=Round(SUM(NET_AMT),0) FROM (select distinct tran_cd,bhent,net_amt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='ST' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND U_IMPORM = 'Purchase Return') b
			SELECT @AMTHH2=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='ST' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND U_IMPORM = 'Purchase Return' 
		end
	else
		begin
			SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #CH_FORM15 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And S_tax <> '' AND PER=@PER and U_imporm <> 'Purchase Return'
			SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM #CH_FORM15 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And S_tax <> '' AND PER=@PER and U_imporm <> 'Purchase Return'
			SELECT @AMTC1=Round(SUM(NET_AMT),0) FROM #CH_FORM15 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER
			SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM #CH_FORM15 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER

			SELECT @AMTAA1=Round(SUM(Net_AMT),0) FROM (select distinct tran_cd,bhent,net_amt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '') b
			SELECT @AMTBB1=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' 
			SELECT @AMTCC1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PR' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '') b
			SELECT @AMTDD1=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='PR' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' 
			SELECT @AMTFF1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='EP' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '') b
			SELECT @AMTFF2=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='EP' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' 
			SELECT @AMTGG1=Round(SUM(VATONAMT),0) FROM (select distinct tran_cd,bhent,vatonamt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='DN' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '') b
			SELECT @AMTGG2=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='DN' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' 
			SELECT @AMTHH1=Round(SUM(NET_AMT),0) FROM (select distinct tran_cd,bhent,net_amt,dbname from #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='ST' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND U_IMPORM = 'Purchase Return') b
			SELECT @AMTHH2=Round(SUM(TAXAMT),0)   FROM #CH_FORM15 WHERE ST_TYPE='LOCAL' AND BHENT='ST' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND U_IMPORM = 'Purchase Return' 
		end
	
  --Sales Invoices
  SET @AMTA1=ISNULL(@AMTA1,0)
  SET @AMTB1=ISNULL(@AMTB1,0)
 
  --Return Invoices
  SET @AMTC1=ISNULL(@AMTC1,0)
  SET @AMTD1=ISNULL(@AMTD1,0)

  --Net Effect
  Set @NetEFF = @AMTA1-(@AMTB1+(@AMTC1-@AMTD1))
  --Set @NetEFF = (@AMTA1-@AMTB1)-(@AMTC1-@AMTD1)
  Set @NetTAX = (@AMTB1)-(@AMTD1)

--Purchase Invoice
  SET @AMTAA1=ISNULL(@AMTAA1,0)
  SET @AMTBB1=ISNULL(@AMTBB1,0)

  --Return Invoice
  SET @AMTCC1=ISNULL(@AMTCC1,0)
  SET @AMTDD1=ISNULL(@AMTDD1,0)

  --Expense Purchase Invoice
  SET @AMTFF1=ISNULL(@AMTFF1,0)
  SET @AMTFF2=ISNULL(@AMTFF2,0)

  --Debit Note Invoice
  SET @AMTGG1=ISNULL(@AMTGG1,0)
  SET @AMTGG2=ISNULL(@AMTGG2,0)

  --Sales Invoice Where U_imporm = 'Purchase Return'
  SET @AMTHH1=ISNULL(@AMTHH1,0)
  SET @AMTHH2=ISNULL(@AMTHH2,0)


--Net Effect

  Set @NetEFF1 = ((@AMTAA1 - @AMTBB1) + (@AMTFF1 ) - (@AMTCC1 - @AMTDD1) - (@AMTGG1 - @AMTGG2) - (@AMTHH1 - @AMTHH2)) 

  Set @NetTAX1 = (@AMTBB1 + @AMTFF2) - @AMTDD1 - @AMTGG2 - @AMTHH2

  if @nettax <> 0 OR @nettax1 <> 0
	  begin
		  INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,AMT4,Party_nm)
                         VALUES(1,'7',CHAR(@CHAR),@PER,@NETEFF,@NETTAX,@NETEFF1,@NETTAX1,'')
		
		  SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
		  SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX

		  SET @AMTMM1=@AMTMM1+@NETEFF1 --TOTAL TAXABLE AMOUNT
		  SET @AMTOO1=@AMTOO1+@NETTAX1 --TOTAL TAX

		  SET @CHAR=@CHAR+1
	  end

  FETCH NEXT FROM CUR_FORM221 INTO @PER
 END
 CLOSE CUR_FORM221
 DEALLOCATE CUR_FORM221

-------------------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0

SELECT @AMTA1 = SUM(AMT1),@AMTB1 = SUM(AMT2),@AMTC1 = SUM(AMT3),@AMTD1 = SUM(AMT4)
FROM #CHFORM15
WHERE PARTSR = 7 AND SRNO IN ('A','B','C','D','E','F','G','H','I','J','K','L','M')

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,AMT4,Party_nm)
               VALUES(1,'7','Z',0,@AMTA1,@AMTB1,@AMTC1,@AMTD1,'')

-------------------------------------------------------------------------------------------

--8

--BLANK

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'8','A',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'8','B',0,@AMTA1,0,0,'')

-------------------------------------------------------------------------------------------

--9

--BLANK

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'9','A',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'9','B',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'9','C',0,@AMTA1,0,0,'')

-------------------------------------------------------------------------------------------

--10

SELECT @AMTA1=0,@AMTB1=0

SELECT @AMTA1=Round(SUM(TAXAMT),0) FROM #CH_FORM15
WHERE BHENT IN ('SR','CN') AND (DATE BETWEEN @SDATE AND @EDATE)

SELECT @AMTB1=Round(SUM(TAXAMT),0) FROM #CH_FORM15
WHERE BHENT IN ('PR','DN') AND (DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #CHFORM15(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
               VALUES(1,'10','A',0,@AMTA1,@AMTB1,0,'')

SELECT @AMTA1=0,@AMTB1=0

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','B',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','C',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','D',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','E',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','F',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','G',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','H',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','I',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','J',0,@AMTA1,0,0,'')


-------------------------------------------------------------------------------------------
--11

--BLANK

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'11','A',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'11','B',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'11','C',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'11','D',0,@AMTA1,0,0,'')

INSERT INTO #CHFORM15
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'11','E',0,@AMTA1,0,0,'')

-------------------------------------------------------------------------------------------


Update #CHFORM15 
set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
AMT3 = isnull(AMT3,0),AMT4 = isnull(AMT4,0), INV_NO = isnull(INV_NO,''),
DATE = isnull(Date,''),PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),ITEM =isnull(item,''),

SELECT * FROM #CHFORM15
order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int),
partsr,SRNO
 
END

set ANSI_NULLS OFF
--PRINT 'CH VAT FORM 15'

