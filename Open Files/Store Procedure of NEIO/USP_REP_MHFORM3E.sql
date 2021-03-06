set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Hetal L Patel
-- Create date: 06/11/2009
-- Description:	This Stored procedure is useful to generate MH VAT FORM 3E
-- Modify date: Arockia Romulus.S
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================
ALTER PROCEDURE [dbo].[USP_REP_MHFORM3E]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS

BEGIN
DECLARE @FCON AS NVARCHAR(2000)
EXECUTE   USP_REP_FILTCON 

@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=NULL,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='M',@VITFILE=Null,@VACFILE='AC'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

DECLARE @SQLCOMMAND NVARCHAR(4000)
DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)
DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #MHFORM_3E
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #MHFORM_3E add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
INTO #MHFORM3E
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2

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
		SET @SQLCOMMAND='Insert InTo #MHFORM_3E Select * from '+@MCON
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
		SET @SQLCOMMAND='Insert InTo #MHFORM_3E Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
-----
-----SELECT * from #form221_1 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----

--->PART 1-5 
 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 


---Part 1 ( Section A )
---Total Gross Sale 1
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E WHERE BHENT in ('ST')  AND (DATE BETWEEN @SDATE AND @EDATE)  and ac_name not like '%Rece%'
Select @AMTA2=Round(SUM(NET_AMT),0) From #MHFORM_3E Where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' and U_imporm = 'Purchase Return'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=@AMTA1-@AMTA2
---SELECT @AMTB1=SUM(NET_AMT) FROM #MHFORM_3E  WHERE St_type = 'Out of State' and u_imporm = 'Branch Transfer' and BHENT in('ST') AND (DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','A',0,@AMTB1,0,0,'','','','','','')

--Total Sales Within the State A
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E WHERE BHENT in ('ST') AND (DATE BETWEEN @SDATE AND @EDATE)  and ac_name not like '%Rece%' And St_type = 'Local' And Tax_name <> 'Exempted' And U_Imporm <> 'Branch Transfer'
Select @AMTA2=Round(SUM(NET_AMT),0) From #MHFORM_3E Where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And St_type = 'Local' and U_imporm = 'Purchase Return' And Tax_name <> 'Exempted' And U_Imporm <> 'Branch Transfer'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=@AMTA1-@AMTA2
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','B',0,@AMTB1,0,0,'','','','','','')

---Sales Out of State Without Tax B
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent in('ST') AND (DATE BETWEEN @SDATE AND @EDATE) And St_type = 'Out Of State' And U_Imporm <> 'Branch Transfer' And Tax_name = ''
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','C',0,@AMTA1,0,0,'','','','','','')

---Sales Return for the period C
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent in('SR','CN') AND (DATE BETWEEN @SDATE AND @EDATE) And St_type = 'Out Of State'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','D',0,@AMTA1,0,0,'','','','','','')
--D
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','E',0,0,0,0,'','','','','','')
---Export Sales E
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent in('ST') AND (DATE BETWEEN @SDATE AND @EDATE) And ST_TYPE='OUT OF COUNTRY'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','F',0,@AMTA1,0,0,'','','','','','')

--F
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','G',0,0,0,0,'','','','','','')
--G
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','H',0,0,0,0,'','','','','','')

---Exempted Sales H
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent in('ST') AND (DATE BETWEEN @SDATE AND @EDATE) And Tax_name = 'Exempted'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','I',0,@AMTA1,0,0,'','','','','','')
---2
SELECT @AMTA1=Sum(AMT1) FROM #MHFORM3E where Partsr = '1' and srno = 'A'
SELECT @AMTA2=Sum(AMT1) FROM #MHFORM3E where Partsr = '1' and srno In('B','C','D','E','F','G','H','I')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1 = @AMTA1 - @AMTA2
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','J',0,@AMTB1,0,0,'','','','','','')

--A
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','K',0,0,0,0,'','','','','','')

---B
SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent in('ST') AND (DATE BETWEEN @SDATE AND @EDATE) And U_Imporm = 'Branch Transfer'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','L',0,@AMTA1,0,0,'','','','','','')
--C
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','M',0,0,0,0,'','','','','','')
--3
SELECT @AMTA1=Sum(AMT1) FROM #MHFORM3E where Partsr = '1' and srno = 'J'
SELECT @AMTA2=Sum(AMT1) FROM #MHFORM3E where Partsr = '1' and srno In('K','L','M')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1 = @AMTA1 - @AMTA2
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','N',0,@AMTB1,0,0,'','','','','','')
--A
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','O',0,0,0,0,'','','','','','')
--4
SELECT @AMTA1=Sum(AMT1) FROM #MHFORM3E where Partsr = '1' and srno = 'N'
SELECT @AMTA2=Sum(AMT1) FROM #MHFORM3E where Partsr = '1' and srno In('O')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1 = @AMTA1 - @AMTA2
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'1','P',0,@AMTB1,0,0,'','','','','','')


---Tax & Taxable Amount of Sales for the period
 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 SET @CHAR=65
 DECLARE  CUR_FORM221 CURSOR FOR 
 select distinct level1 from stax_mas where ST_TYPE='OUT OF STATE' And ac_name1 not like '%VAT%'--CHARINDEX('VAT',TAX_NAME)>0
 OPEN CUR_FORM221
 FETCH NEXT FROM CUR_FORM221 INTO @PER
 WHILE (@@FETCH_STATUS=0)
 BEGIN
	if @per = 0
		begin
			SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' And ac_name not like '%Rece%' and U_imporm <> 'Purchase Return'
			SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM #MHFORM_3E where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' And ac_name not like '%Rece%' and U_imporm <> 'Purchase Return'
			SELECT @AMTC1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER 
			SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM #MHFORM_3E where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER
		end
	else
		begin
			SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And S_tax <> '' AND PER=@PER and U_imporm <> 'Purchase Return'
			SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM #MHFORM_3E where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And S_tax <> '' AND PER=@PER and U_imporm <> 'Purchase Return'
			SELECT @AMTC1=Round(SUM(NET_AMT),0) FROM #MHFORM_3E where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER
			SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM #MHFORM_3E where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER
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

  if @nettax <> 0
	  begin
		  INSERT INTO #MHFORM3E
		  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		  (1,'2',CHAR(@CHAR),@PER,@NETEFF,@NETTAX,0,'')
		  
		  SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
		  SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX
		  SET @CHAR=@CHAR+1
	  end

  FETCH NEXT FROM CUR_FORM221 INTO @PER
 END
 CLOSE CUR_FORM221
 DEALLOCATE CUR_FORM221

---Total of Tax & Taxable Amount of Sales for the period
 INSERT INTO #MHFORM3E
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'2','Z',0,@AMTJ1,@AMTK1,0,'')

---Tax & Taxable Amount of Sales for the period
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'3','A',0,0,0,0,'','','','','','')

---Tax & Taxable Amount of Sales for the period
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'4','A',0,0,0,0,'','','','','','')

--5
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','A',0,0,0,0,'','','','','','')
--6
Select @AMTA1=sum(Amt2) from #MHFORM3E where Partsr = '2' And Srno = 'Z'
Select @AMTA1=sum(Amt1) from #MHFORM3E where Partsr in('3','4')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB2= @AMTA1 + @AMTA2
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','B',0,@AMTB1,0,0,'','','','','','')
--7
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','C',0,0,0,0,'','','','','','')
--8
Select @AMTA1=sum(Amt1) from #MHFORM3E where Partsr = '5' And Srno = 'B'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','D',0,@AMTA1,0,0,'','','','','','')
--9
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','E',0,0,0,0,'','','','','','')
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','F',0,0,0,0,'','','','','','')

--10
Select @AMTA1=sum(Amt1) from #MHFORM3E where Partsr = '5' And Srno In('D','E')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','G',0,@AMTA1,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','H',0,0,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','I',0,0,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','J',0,0,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','K',0,0,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','L',0,0,0,0,'','','','','','')
--11
Select @AMTA1=sum(Amt1) from #MHFORM3E where Partsr = '5' And Srno In('G')
Select @AMTA2=sum(Amt1) from #MHFORM3E where Partsr = '5' And Srno In('H','I','J','K','L')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB2= @AMTA1 - @AMTA2
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','M',0,@AMTB1,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','N',0,0,0,0,'','','','','','')

INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','O',0,0,0,0,'','','','','','')

--12
Select @AMTA1=sum(Amt1) from #MHFORM3E where Partsr = '5' And Srno In('G')
Select @AMTA2=sum(Amt1) from #MHFORM3E where Partsr = '5' And Srno In('H','I','J','K','L')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB2= @AMTA1 - @AMTA2
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
 			   VALUES (1,'5','P',0,@AMTB1,0,0,'','','','','','')

SELECT @AMTA1=Round(SUM(A.NET_AMT),0) FROM #MHFORM_3E A
Inner join Bpmain B on (A.Bhent = B.Entry_ty and A.Tran_cd = B.Tran_cd)
where A.bhent in('BP') AND (A.DATE BETWEEN @SDATE AND @EDATE) And B.Party_nm Not Like '%VAT%'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','Q',0,@AMTA1,0,0,'','','','','','')

SELECT @AMTA1=AMT1 FROM #MHFORM3E where Partsr = '5' And SrNo = 'P'
SELECT @AMTA2=AMT1 FROM #MHFORM3E where Partsr = '5' And SrNo = 'Q'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB2= @AMTA1 - @AMTA2
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'5','R',0,@AMTB2,0,0,'','','','','','')

--Part 6 ( Section F )
Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4)


SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0

SET @CHAR=65

SET @PER = 0
declare Cur_VatPay cursor  for
select A.Taxonamt,A.Gro_amt,A.taxamt,A.INV_NO,B.Date,A.Party_nm,Address='',A.Form_nm,A.S_tax
from #MHFORM_3E A
Inner join Bpmain B on (A.Bhent = B.Entry_ty and A.Tran_cd = B.Tran_cd)
where BHENT = 'BP' And A.Date Between @sdate and @edate And B.Party_nm not Like '%VAT%'
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
	
	INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) VALUES (1,'6',CHAR(@CHAR),@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX)

 SET @CHAR=@CHAR+1
 FETCH NEXT FROM CUR_VatPay INTO @TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay

set @AMTA1 = 0
select @AMTA1=Sum(AMT1) from #MHFORM3E where Partsr = '6'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,Party_nm) VALUES  (1,'6','Z',0,@AMTA1,@AMTO1,0,'Total','')


--Part 7 ( Section G )
INSERT INTO #MHFORM3E (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
			   VALUES (1,'7','A',0,0,0,0,'','','','','','')
----

Update #MHFORM3E set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),  ITEM =isnull(item,''),


SELECT * FROM #MHFORM3E order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
END
--Print 'MH VAT FORM 3E'

