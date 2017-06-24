If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_KA_FORMVAT100'))
Begin
	Drop Procedure USP_REP_KA_FORMVAT100
End
Go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

/*
EXECUTE USP_REP_KA_FORMVAT100'','','','08/01/2011','03/31/2012','','','','',0,0,'','','','','','','','','2011-2012',''
*/

-- =============================================
-- Author:		Hetal L Patel
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate KA VAT Form 100
-- Modify date: 16/05/2007
-- Modification By/date/Reason : changes by sandeep on 13/12/2011 for Bug-996 
-- Modification By/date/Reason : changes by G.Prashanth Reddy on 24/04/2012 for Bug-2351 
-- Modification By/date/Reason : changes by Nilesh Yadav on 23/04/2015 for Bug-26166
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_KA_FORMVAT100]
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
,@VSDATE=NULL
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
DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)
DECLARE @AMTA2 NUMERIC(12,2),@AMTA3 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #FORM221_1
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #form221_1 add recno int identity

--Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX,PARTY_NM1=AC1.AC_NAME
INTO #FORM221
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2

------
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
	SET @SQLCOMMAND='Insert InTo  VATTBL Select * from '+@MCON
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND='Drop Table '+@MCON
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
End
else
Begin ------Fetch Single Co. Data
	 Set @MultiCo = 'NO'
	 EXECUTE USP_REP_SINGLE_CO_DATA_VAT
	  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
	 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
	 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
	 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
	 ,@MFCON = @MCON OUTPUT
End

------
--Declare @MultiCo	VarChar(3)
--Declare @MCON as NVARCHAR(2000)
--IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
--	Begin	------Fetch Records from Multi Co. Data
--		 Set @MultiCo = 'YES'
--		 EXECUTE USP_REP_MULTI_CO_DATA
--		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
--		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
--		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
--		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
--		 ,@MFCON = @MCON OUTPUT

--		--SET @SQLCOMMAND='Select * from '+@MCON
--		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
--		SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--		---Drop Temp Table 
--		SET @SQLCOMMAND='Drop Table '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	End
--else
--	Begin ------Fetch Single Co. Data
--		 Set @MultiCo = 'NO'
--		 EXECUTE USP_REP_SINGLE_CO_DATA
--		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
--		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
--		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
--		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
--		 ,@MFCON = @MCON OUTPUT

--		--SET @SQLCOMMAND='Select * from '+@MCON
--		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
--		SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--		---Drop Temp Table 
--		SET @SQLCOMMAND='Drop Table '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	End
-----
-----SELECT * from #form221_1 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----



---Part 2N3 
---Local & Inter State Sales
Set @AmtA1 = 0
Set @AmtA2 = 0
--AND tax_name like '%VAT%' 

--select @AmtA1=Sum(Net_Amt) From #FORM221_1 where st_type = 'Local' and BHENT in('ST') And Date Between @sdate and @edate
select @AmtA1=Sum(gro_Amt) From vattbl where BHENT in('ST') AND  st_type = 'Local'  And Date Between @sdate and @edate
 And Date Between @sdate and @edate
select @AmtA2=sum(gro_Amt) From vattbl where st_type in( 'Out of State','OUT OF COUNTRY')  and BHENT in('ST') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','A',0,@AMTA1,@AMTA2,0,'','')



---Local & Inter State Sales Return
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=Sum(gro_Amt) From vattbl where st_type = 'Local' and BHENT = 'SR' And Date Between @sdate and @edate
select @AmtA2=sum(gro_Amt) From vattbl where st_type in ( 'Out of State','OUT OF COUNTRY')  and BHENT = 'SR' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','B',0,@AMTA1,@AMTA2,0,'','')


---Local & Inter State Consignment Sales
Set @AmtA1 = 0
Set @AmtA2 = 0

SELECT @AMTA1=SUM(gro_Amt) FROM vattbl  WHERE ST_TYPE='LOCAL' AND BHENT='ST' AND U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTA2=SUM(gro_Amt) FROM vattbl  WHERE ST_TYPE='Out of State' AND BHENT='ST' AND U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (DATE BETWEEN @SDATE AND @EDATE)
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','C',0,@AMTA1,@AMTA2,0,'','')



--Local & Inter State Vat collected & exempted
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(TaxAmt) From vattbl where st_type = 'Local' and BHENT = 'ST' And Date Between @sdate and @edate 
select @AmtA2=Sum(Gro_Amt) From vattbl where ST_TYPE='Out of State' and Per = 0 and TaxAmt = 0 and BHENT = 'ST' 
And Date Between @sdate and @edate and TAX_NAME='EXEMPTED'

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','D',0,@AMTA1,@AMTA2,0,'','')


--Local & Inter State Sales exempted & Export 
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(Gro_Amt) From vattbl where st_type = 'Local' And Per = 0 and TaxAmt = 0 and BHENT = 'ST' And Date Between @sdate and @edate and TAX_NAME='EXEMPTED'
SELECT @AMTA2=SUM(Gro_AMT) FROM vattbl  WHERE ST_TYPE='OUT OF COUNTRY' AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
And (Tax_Name Not in('Form H', 'H Form', 'H', 'Form E1', 'Form E2', 'E-1', 'E-2')) And (U_IMPORM <> 'High Sea Sales')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','E',0,@AMTA1,@AMTA2,0,'','')

--Local Others & Inter State Sales Other & Against Form H
Set @AmtA1 = 0
Set @AmtA2 = 0
SELECT @AMTA2=SUM(Gro_AMT) FROM vattbl  WHERE ST_TYPE in ('OUT OF COUNTRY', 'OUT OF STATE') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE) 
and (Tax_Name in('Form H', 'F Form', 'H'))
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','F',0,0,@AMTA2,0,'','')


--Local Net Sales Total & Inter State E-1 & E-2 Sales
Set @AmtA1 = 0
Set @AmtA2 = 0
Set @AmtB1 = 0
Set @AmtB2 = 0

--select @AmtB1=Amt1 From #FORM221 where PartSr = '1' and Partsr = 'A'
select @AMTB1=Amt1 From #FORM221 where PartSr = '1' and srno = 'A'
select @AmtB2=sum(Amt1) From #FORM221 where PartSr = '1' and srno in('B','C','D','E','F')
select @AmtA2=Sum(Gro_Amt) From (select distinct gro_amt from #FORM221_1 where Tax_Name in ('Form E1', 'Form E2', 'E-1', 'E-2') 
and st_type in ('OUT OF COUNTRY', 'OUT OF STATE') and BHENT = 'ST' And Date Between @sdate and @edate) b
Set @AMTA1 = @AMTB1-@AMTB2
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','G',0,@AMTA1,@AMTA2,0,'','')


--Inter State High Seas Sales
Set @AmtA1 = 0
Set @AmtA2 = 0

SELECT @AMTA2=SUM(GRO_AMT) FROM vattbl WHERE ST_TYPE='OUT OF COUNTRY' AND BHENT='ST' 
AND U_IMPORM='High Sea Sales'  AND (DATE BETWEEN @SDATE AND @EDATE)
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','H',0,0,@AMTA2,0,'','')


--Inter State Sales CST Collected
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA2=Sum(TaxAmt) From vattbl where st_type = 'Out of State' and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','I',0,0,@AMTA2,0,'','')


--Inter State Sales Total
Set @AmtA1 = 0
Set @AmtA2 = 0
Set @AmtB1 = 0
Set @AmtB2 = 0
select @AMTB1=Amt2 From #FORM221 where Part = 1 And PartSr = '1' and srno = 'A'
select @AmtB2=Sum(Amt2) From #FORM221 where Part = 1 And Partsr = '1' and srno in('B','C','D','E','F','G','H','I')
Set @AMTA2 = @AMTB1-@AMTB2
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'1','J',0,0,@AMTA2,0,'','')


-----------------------PART V START-------------------------------------------------

---VAT Payable From Bank Payment Details
Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_N as varchar(10)
,@DATE as smalldatetime,@BANK_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50)
,@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4),@INV_NO AS VARCHAR(10)
Declare @PARTY_NM1 as varchar(100),@PARTY_NM as varchar(100)

SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@BANK_NM ='',@ADDRESS ='',@FORM_NM='',@S_TAX ='',@PARTY_NM='',@PARTY_NM1=''

Declare @IT_NAME3 as varchar(100),@IT_NAME4 as varchar(100),@IT_NAME5 as varchar(500)
Declare @chdate as smalldatetime

SET @AMTA2=0
SELECT @IT_NAME3='',@IT_NAME4='',@IT_NAME5=''
SET @PER = 0
set @chdate = ''
declare Cur_VatPay cursor  for
Select sum(A.VATONAMT),inv_no=b.cheq_no,bank_nm,b.date
From vattbl A
Inner Join Bpmain B On(A.Bhent = B.Entry_Ty And A.Tran_cd = B.Tran_cd)
Where A.BHENT = 'BP' And (B.Date Between @sdate and @edate) And B.Party_nm like '%VAT%' and b.u_nature=''
group by b.cheq_no,bank_nm,b.date
open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@INV_NO,@BANK_NM,@DATE
 WHILE (@@FETCH_STATUS=0)
 BEGIN
 	SET @AMTA2=isnull(@TAXONAMT,0)+@AMTA2
	SET @IT_NAME3=RTRIM(@INV_NO)+','+@IT_NAME3
	SET @IT_NAME4=RTRIM(@BANK_NM)+','+@IT_NAME4
	SET @IT_NAME5=RTRIM(convert(varchar(10),@DATE,103))+','+@IT_NAME5
FETCH NEXT FROM CUR_VatPay INTO  @TAXONAMT,@INV_NO,@BANK_NM,@DATE
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay


INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
VALUES(1,'5',	 'A',  0, @AMTA2, 0  ,0, @IT_NAME3,'',@IT_NAME4, '','','',@IT_NAME5)
---
DECLARE @nCount AS NUMERIC(4)

---Interest
SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@BANK_NM='',@ADDRESS ='',@FORM_NM='',@S_TAX =''
SET @CHAR=65
SET @AMTA2=0
SELECT @IT_NAME3='',@IT_NAME4='',@IT_NAME5=''
SET @PER = 0
declare Cur_VatPay cursor  for
Select sum(A.VATONAMT),inv_no=b.cheq_no,bank_nm,b.date
From vattbl A
Inner Join Bpmain B On(A.Bhent = B.Entry_Ty And A.Tran_cd = B.Tran_cd)
Where A.BHENT = 'BP' And (B.Date Between @sdate and @edate) And B.Party_nm like '%VAT%' and b.u_nature='Interest' 
group by b.cheq_no,bank_nm,b.date 
open Cur_VatPay

FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@INV_NO,@BANK_NM,@DATE
 WHILE (@@FETCH_STATUS=0)
 BEGIN
      
 	SET @AMTA2=isnull(@TAXONAMT,0)+@AMTA2
	SET @IT_NAME3=RTRIM(@INV_NO)+','+@IT_NAME3
	SET @IT_NAME4=RTRIM(@BANK_NM)+','+@IT_NAME4
	SET @IT_NAME5=RTRIM(convert(varchar(10),@DATE,103))+','+@IT_NAME5
FETCH NEXT FROM CUR_VatPay INTO  @TAXONAMT,@INV_NO,@BANK_NM,@DATE
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
VALUES				     (1,'5',	 'B',  0, @AMTA2, 0  ,0, @IT_NAME3,'',@IT_NAME4, '','','',@IT_NAME5)


---Others
SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@BANK_NM ='',@ADDRESS ='',@FORM_NM='',@S_TAX =''
SET @CHAR=65
SET @AMTA2=0
SELECT @IT_NAME3='',@IT_NAME4='',@IT_NAME5=''
SET @PER = 0
declare Cur_VatPay cursor  for
Select sum(A.VATONAMT),inv_no=b.cheq_no,bank_nm,b.date
From vattbl A
Inner Join Bpmain B On(A.Bhent = B.Entry_Ty And A.Tran_cd = B.Tran_cd)
Where A.BHENT = 'BP' And (B.Date Between @sdate and @edate) And B.Party_nm like '%VAT%' and b.u_nature='Other payments on account of' 
group by b.cheq_no,bank_nm,b.date 
open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@INV_NO,@BANK_NM,@DATE
 WHILE (@@FETCH_STATUS=0)
 BEGIN
 	SET @AMTA2=isnull(@TAXONAMT,0)+@AMTA2
	SET @IT_NAME3=RTRIM(@INV_NO)+','+@IT_NAME3
	SET @IT_NAME4=RTRIM(@BANK_NM)+','+@IT_NAME4
	SET @IT_NAME5=RTRIM(convert(varchar(10),@DATE,103))+','+@IT_NAME5
FETCH NEXT FROM CUR_VatPay INTO  @TAXONAMT,@INV_NO,@BANK_NM,@DATE
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
VALUES				     (1,'5',	 'C',  0, @AMTA2, 0  ,0, @IT_NAME3,'',@IT_NAME4, '','','',@IT_NAME5)
----

---Total
SET @AMTA1=0
SELECT @AMTA1=Sum(Amt1) from #FORM221 Where Part = 1 And PARTSR = '5' and SRNO <> 'D'
SET @AMTA1=Isnull(@AMTA1,0)
--SELECT @IT_NAME3='',@IT_NAME4='',@IT_NAME5=''
--SET @nCount = 0
--Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '5' and SRNO='D'
--SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
--IF @nCount = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
VALUES	(1,'5',	 'D',  0, @AMTA1, 0  ,0, '','','', '','','','')



											---------PART VI START----------

---Sales & Purchase 1 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 1 and BHENT = 'ST' And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 1 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','A',0,@AMTA1,@AmtA2,0,'','')


---Sales & Purchase 4 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 4 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 4 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','B',0,@AMTA1,@AmtA2,0,'','')



---Sales & Purchase 5 %
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 5 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 5 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','C',0,@AMTA1,@AmtA2,0,'','')


---Sales & Purchase 5.5 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per =5.50 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 5.50 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','CA',0,@AMTA1,@AmtA2,0,'','')



---Sales & Purchase 13.5 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 13.50 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 13.50 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','D',0,@AMTA1,@AMTA2,0,'','')


---Sales & Purchase 14 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 14 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 14 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','DA',0,@AMTA1,@AMTA2,0,'','')


---Sales & Purchase 14.50 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 14.50 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 14.50 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','DB',0,@AMTA1,@AMTA2,0,'','')




---Sales & Purchase 15 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 15 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 15 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','E',0,@AMTA1,@AMTA2,0,'','')


---Sales & Purchase 17 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where Per = 17 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
select @AmtA2=sum(TaxAmt) From vattbl where Per = 17 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','EA',0,@AMTA1,@AMTA2,0,'','')

--Sales and purchase of other rate
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where (Per <> 1 And Per <> 4 And Per <> 5 And Per <> 5.5 And Per <>13.5 And Per <>14 And Per <>14.5 And Per <>15 And Per <>17 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate)
select @AmtA2=sum(TaxAmt) From vattbl where (Per <> 1 And Per <> 4 And Per <> 5 And Per <> 5.5 And Per <>13.5 And Per <>14 And Per <>14.5 And Per <>15 And Per <>17 and BHENT = 'ST'  And st_type in ('Local','') And Date Between @sdate and @edate)
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','F',0,@AmtA1,@AmtA2,0,'','')


--Taxable turnover of purchases from un-registered dealers with rate of tax of 5%
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where  Per=5 and BHENT = 'PT' And S_TAX='' And st_type in ('Local','') and Date Between @sdate and @edate
select @AmtA1=Sum(TaxAmt) From vattbl where  Per=5 and BHENT = 'PT'  And S_TAX=''  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','G',0,@AmtA1,@AmtA2,0,'','')


--Taxable turnover of purchases from un-registered dealers with rate of tax of 5.5%

Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where  Per=5.5 and BHENT = 'PT' And S_TAX=''  And st_type in ('Local','') and Date Between @sdate and @edate
select @AmtA1=Sum(TaxAmt) From vattbl where  Per=5.5 and BHENT = 'PT'  And S_TAX=''  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','GA',0,@AmtA1,@AmtA2,0,'','')


--Taxable turnover of purchases from un-registered dealers with rate of tax of 13%
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where  Per=13 and BHENT = 'PT' And S_TAX=''  And st_type in ('Local','') and Date Between @sdate and @edate
select @AmtA1=Sum(TaxAmt) From vattbl where  Per=13 and BHENT = 'PT'  And S_TAX=''  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','H',0,@AmtA1,@AmtA2,0,'','')



--Taxable turnover of purchases from un-registered dealers with rate of tax of 14%
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where  Per=14 and BHENT = 'PT' And S_TAX=''  And st_type in ('Local','') and Date Between @sdate and @edate
select @AmtA1=Sum(TaxAmt) From vattbl where  Per=14 and BHENT = 'PT'  And S_TAX=''  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','HA',0,@AmtA1,@AmtA2,0,'','')



--Taxable turnover of purchases from un-registered dealers with rate of tax of 14.5%
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where  Per=14.5 and BHENT = 'PT' And S_TAX=''  And st_type in ('Local','') and Date Between @sdate and @edate
select @AmtA1=Sum(TaxAmt) From vattbl where  Per=14.5 and BHENT = 'PT'  And S_TAX=''  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','HB',0,@AmtA1,@AmtA2,0,'','')



--Taxable turnover of purchases from un-registered dealers with other rate

Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where (Per <> 5 And Per <> 5.5 And Per <>13 And Per <>14 And Per <>14.5 and BHENT = 'PT'  And st_type in ('Local','') and s_tax='' And Date Between @sdate and @edate)
select @AmtA2=sum(TaxAmt) From vattbl where  (Per <> 5 And Per <> 5.5 And Per <>13 And Per <>14 And Per <>14.5 and BHENT = 'PT'  And st_type in ('Local','') and s_tax='' And Date Between @sdate and @edate)
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','I',0,@AmtA1,@AmtA2,0,'','')


--Total Taxable turnover and Total output tax payable
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORM221 where Part = 1 And Partsr = '6' and srno in ('A','B','C','CA','D','DA','DB','E','EA','F','G','GA','H','HA','HB','I')
select @AmtA2=Sum(Amt2) From #FORM221 where Part = 1 And Partsr = '6' and srno in ('A','B','C','CA','D','DA','DB','E','EA','F','G','GA','H','HA','HB','I')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'6','J',0,@AMTA1,@AMTA2,0,'','')

											---------PART VI END-----------




											---------PART VII START-----------


--Inter State Sales & CST Payable 1 %
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per = 1 and st_type = 'Out of State' and form_nm in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 1 and st_type = 'Out of State' and form_nm in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','A',0,@AMTA1,@AMTA2,0,'','')



--Taxable turnover of sales against C Forms at 2% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per = 2 and st_type = 'Out of State' and form_nm in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 2 and st_type = 'Out of State' and form_nm in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','B',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at 1% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per = 1 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 1 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','C',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at 4% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per = 4 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 4 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','D',0,@AMTA1,@AMTA2,0,'','')



--Taxable turnover of sales without C Forms at 5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per = 5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','E',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at 5.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =5.5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per =5.5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','EA',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at 13.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =13.5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per =13.5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','F',0,@AMTA1,@AMTA2,0,'','')

--Taxable turnover of sales without C Forms at 14% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =14 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per =14 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','FA',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at 14.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =14.5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per =14.5 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','FB',0,@AMTA1,@AMTA2,0,'','')

--Taxable turnover of sales without C Forms at 15% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =15 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per =15 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','G',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at 17% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =17 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per =17 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','GA',0,@AMTA1,@AMTA2,0,'','')


--Taxable turnover of sales without C Forms at other rate tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where (Per <> 1 And Per <> 4 And Per <> 5 And Per <> 5.5 And Per <>13.5 And Per <>14 And Per <>14.5 And Per <>15 And Per <>17 and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and st_type = 'Out of State' and BHENT = 'ST' And Date Between @sdate and @edate)
select @AmtA2=Sum(TaxAmt) from vattbl where (Per <> 1 And Per <> 4 And Per <> 5 And Per <> 5.5 And Per <>13.5 And Per <>14 And Per <>14.5 And Per <>15 And Per <>17 and st_type = 'Out of State' and form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM') and BHENT = 'ST' And Date Between @sdate and @edate)
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','H',0,@AMTA1,@AMTA2,0,'','')



--Total Taxable turnover (Box No. 7.1 to 7.8)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORM221 where Partsr = '7' and srno in ('A','B','C','D','E','EA','F','FA','FB','G','GA','H')
select @AmtA2=Sum(Amt2) From #FORM221 where Partsr = '7' and srno in ('A','B','C','D','E','EA','F','FA','FB','G','GA','H')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'7','I',0,@AMTA1,@AMTA2,0,'','')


											--------PART VII END-----------


										    --------PART VIII START-----------


Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(Amt2) from #FORM221 where Part = 1 And PartSr = '6' and Srno = 'J'
select @AmtA2=Sum(Amt2) from #FORM221 where Part = 1 And PartSr = '7' and Srno = 'I'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'8','A',0,@AMTA1+@AMTA2,0,0,'','')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'8','B',0,0,0,0,'','')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'8','C',0,@AMTA1+@AMTA2,0,0,'','')


												--------PART VIII END-----------
												
-----------------------PART IV START-------------------------------------------------
--Update the Part 4.1---- 
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORM221 where Part = 1 And Partsr = '8' and SRNO='C' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','A',0,@AMTA1,0,0,'','')    
--------------------------


INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','B',0,0,0,0,'','')    

-----------------------PART IV END-------------------------------------------------
				
				
												
												
												
												-------PART VIII START-----------


---Net value of purchases at 1% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =1 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 1 and st_type in ('Local','') and BHENT = 'PT'  And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') And vattbl.s_tax <> '')
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','A',0,@AmtA1,@AmtA2,0,'','')


---Net value of purchases at 4% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =4 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 4 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','B',0,@AmtA1,@AmtA2,0,'','')


---Net value of purchases at 5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','C',0,@AmtA1,@AmtA2,0,'','')

---Net value of purchases at 5.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =5.5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And tax_name not in ('Exempted') And Date Between @sdate and @edate
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 5.5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','CA',0,@AmtA1,@AmtA2,0,'','')


---Net value of purchases at 13.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =13.5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 13.5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','D',0,@AmtA1,@AmtA2,0,'','')


---Net value of purchases at 14% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =14 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 14 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','DA',0,@AmtA1,@AmtA2,0,'','')



---Net value of purchases at 14.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per=14.5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 14.5 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','DB',0,@AmtA1,@AmtA2,0,'','')



---Net value of purchases at 15% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =15 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 15 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','E',0,@AmtA1,@AmtA2,0,'','')


---Net value of purchases at 17% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =17 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where Per = 17 and st_type in ('Local','') and BHENT = 'PT' And vattbl.s_tax <> ''
And tax_name not in ('Exempted') And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','EA',0,@AmtA1,@AmtA2,0,'','')


--9.6 Value of URD purchases to the extent used or sold during the month/quarter(specify rate of tax):
Set @AmtA1 = 0
Set @AmtA2 = 0
--select @AmtA1=Sum(vatonamt) from vattbl 
--where (vattbl.u_imporm = 'Purchase from Unregistered Dealer' Or vattbl.s_tax = '') and st_type = 'Local' and BHENT = 'PT' 
--And Date Between @sdate and @edate
--select @AmtA2=Sum(TaxAmt) from vattbl where (vattbl.u_imporm = 'Purchase from Unregistered Dealer' Or vattbl.s_tax = '') 
--and st_type = 'Local' and BHENT = 'PT' And Date Between @sdate and @edate
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','F',0,@AmtA1,@AmtA2,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','F',0,0,0,0,'','')

---- 9.6.1 Brought forward from previous period
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','FA',0,0,0,0,'','')

---- 9.6.2 Relating to current tax period
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl 
where (vattbl.u_imporm = 'Purchase from Unregistered Dealer' Or vattbl.s_tax = '') and st_type in ('Local','') and BHENT = 'PT' 
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where (vattbl.u_imporm = 'Purchase from Unregistered Dealer' Or vattbl.s_tax = '') 
And tax_name not in ('Exempted') and st_type in ('Local','') and BHENT = 'PT' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','FB',0,@AmtA1,@AmtA2,0,'','')

---- 9.6.3 Total
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','FC',0,0,0,0,'','')

--9.7 Others, if any (please specify)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where (Per <> 1 And Per <> 4 And Per <> 5 And Per <> 5.5 And Per <>13.5 And Per <>14 And Per <>14.5 And Per <>15 And Per <>17)
and st_type in ('Local','') and BHENT = 'PT'  And vattbl.s_tax <> '' And tax_name not in ('Exempted') And Date Between @sdate and @edate
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer'))
select @AmtA2=Sum(TaxAmt) from vattbl where (Per <> 1 And Per <> 4 And Per <> 5 And Per <> 5.5 And Per <>13.5 And Per <>14 And Per <>14.5 And Per <>15 And Per <>17 )
and st_type in ('Local','') and BHENT = 'PT'  And vattbl.s_tax <> '' And tax_name not in ('Exempted') And Date Between @sdate and @edate
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','G',0,@AMTA1,@AMTA2,0,'','')


---- 9.8 Value of URD purchases to the extent not used or sold (specify rate of tax):
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','H',0,0,0,0,'','')

---- 9.8.1 Brought forward from previous period
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','HA',0,0,0,0,'','')

---- 9.8.2 Relating to current tax period
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','HB',0,0,0,0,'','')

---- 9.8.3 Total
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','HC',0,0,0,0,'','')

---Value of VAT exempted goods

Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where st_type in ('Local','') and BHENT = 'PT' 
And Date Between @sdate and @edate and tax_name in ('Exempted') and u_imporm not in  ('Purchase from Unregistered Dealer', 'Branch Transfer', 'Consignment Transfer', 'Composition Dealer')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','I',0,@AMTA1,0,0,'','')


---Value of Purchases from Composition dealer
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(vatonAmt) From vattbl where u_imporm = 'Composition Dealer' And vattbl.s_tax <> ''
and BHENT = 'PT' And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','J',0,@AMTA1,0,0,'','')


---Value of goods imported and / or purchased in the course of import/ export / inter-State trade including EI and EII purchases.
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonAmt) From vattbl where st_type in('Out of State','Out of Country') 
and BHENT = 'PT' And tax_name not in ('Exempted') 
And Date Between @sdate and @edate and u_imporm not in ('Purchase from Unregistered Dealer', 'Branch Transfer', 'Consignment Transfer', 'Composition Dealer' )
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','K',0,@AMTA1,0,0,'','')


---Value of goods received by stock transfer / consignment transfer
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(vatonamt) From vattbl where 
BHENT = 'PT' And tax_name not in ('Exempted') 
And Date Between @sdate and @edate and u_imporm in ('Branch Transfer', 'Consignment Transfer')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','L',0,@AMTA1,0,0,'','')


----TOTAL
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORM221 where part = 1 And partsr = '9' And srno <> 'M'
select @AmtA2=sum(Amt2) From #FORM221 where part = 1 And partsr = '9' And srno <> 'M'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9','M',0,@AMTA1,@AMTA2,0,'','')



											-------------PART IX END----------------




													---Part 10A (10)
													
													
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','A',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','B',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','C',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','D',0,0,0,0,'','')

Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(TaxAmt) from vattbl where BHENT = 'PR' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','E',0,@AMTA1,0,0,'','')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','F',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10A','G',0,@AMTA1,0,0,'','')


														---Part 10B (11)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt2) From #FORM221 where part = 1 And partsr = '9' and SRNO='M'
select @AmtA2=sum(Amt1) From #FORM221 where part = 1 and partsr = '10A' and SRNO='G'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END														
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10B','A',0,0,(@AmtA1-@AmtA2),0,'','')



-----PART IV PART ----
select @AmtA1=sum(Amt2) From  #FORM221 where PARTSR='10B' and SRNO = 'A'
INSERT INTO  #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','C',0,@AmtA1,0,0,'','')    
--------------------


---------------PART IV START
Set @AmtA1 = 0
Set @AmtA2 = 0
Set @AMTA3=0
select @AmtA1=sum(Amt1) From #FORM221 where partsr = '4' and SRNO='A'
select @AmtA2=sum(Amt1) From #FORM221 where partsr = '4' and SRNO='B'
select @AmtA3=sum(Amt1) From #FORM221 where partsr = '4' and SRNO='C'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTA3=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA3 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','D',0,(@AMTA1-(@AMTA2+@AMTA3)),0,0,'','')
----------------------------------------

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','E',0,0,0,0,'','')    

----TAX DURING THE PERIOD---
SET @AMTA1=0
select @AMTA1=ISNULL(sum(A.GRO_AMT),0) from VATTBL A Inner Join Bpmain B On(a.Bhent = B.Entry_ty And A.Tran_cd = B.Tran_cd)
where A.bhent='BP' AND (A.Date Between @Sdate and @Edate) And B.Party_nm ='Vat Payable' AND B.U_NATURE =''
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','EA',0,@AMTA1,0,0,'','')
-------------------------

-------Balance Tax Payable [Box No. 4.4 .. (Box No.4.5 + Box No.4.6)]

Set @AmtA1 = 0
Set @AmtA2 = 0
Set @AMTA3 = 0
select @AmtA1=sum(Amt1) From #FORM221 where partsr = '4' and SRNO='D'
select @AmtA2=sum(Amt1) From #FORM221 where partsr = '4' and SRNO='E'
select @AmtA3=sum(Amt1) From #FORM221 where partsr = '4' and SRNO='EA'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTA3=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA3 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','F',0,(@AMTA1-(@AMTA2+@AMTA3)),0,0,'','')
----------------

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','FA',0,0,0,0,'','')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','G',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','H',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','I',0,0,0,0,'','')


---Part 10C (12a)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10C','A',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10C','B',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10C','C',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10C','D',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10C','E',0,0,0,0,'','')


---Part 10D (12b)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','A',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','B',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','C',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','D',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','E',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','F',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10D','G',0,0,0,0,'','')


---Part 10E (13a)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','A',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','B',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','C',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','D',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','E',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','F',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10E','G',0,0,0,0,'','')

---Part 10F (13b)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10F','A',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10F','B',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10F','C',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10F','D',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10F','E',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10F','F',0,0,0,0,'','')




			
										----PART 10 G START--------------

set @char = 65
Declare cur_Purc cursor for
select Per=0,Sum(M.Net_AMT),Sum(M.TAXAMT),Sum(M.Gro_Amt),M.INV_NO,M.DATE,M.Party_nm,M.Address,M.Form_nm,M.S_Tax
from vattbl M
inner Join Ptitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where M.BHENT In('PT','EP','CN') And M.Date Between @sdate and @edate And D.tax_name like '%VAT%' And M.St_type = 'Local'
Group By M.Tran_Cd,m.Bhent,M.INV_NO,M.DATE,M.Party_nm,M.Address,M.Form_nm,M.S_Tax
OPEN Cur_Purc
FETCH NEXT FROM Cur_Purc INTO @PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX
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

	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
	 VALUES (1,'10G','A',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')

	SET @CHAR=@CHAR+1
	FETCH NEXT FROM Cur_purc INTO @PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX  
END
CLOSE Cur_Purc
DEALLOCATE Cur_Purc



													----PART 10 G END--------------
									
													
													
												--------Part 10H START--------------


SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0

--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
--INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where b.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('C','Form C', 'Form-C', 'C Form', 'C-Form') And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','A',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')



INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','A',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('C','Form C', 'Form-C', 'C Form', 'C-Form') And a.Date Between @sdate and @edate
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='A'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','A',0,0,0,0,'','','','','','',''
   END

--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm,@Address='',@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty where b.st_type = 'Out of State' 
--and a.BHENT IN ('ST','PT') And b.form_nm Like '% D%' And a.Date Between @sdate and @edate

--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
-- VALUES (1,'10H','B',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX)



--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty 
--INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where b.st_type = 'Out of State' 
--and a.BHENT IN ('ST') And A.Rform_nm in ('F', 'Form F', 'Form-F', 'F Form', 'F-Form') And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','B',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','B',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('F', 'Form F', 'Form-F', 'F Form', 'F-Form') And a.Date Between @sdate and @edate
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='B'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','B',0,0,0,0,'','','','','','',''
   END

--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
--INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where b.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('H', 'Form H', 'Form-H', 'H Form', 'H-Form') And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','c',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','C',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('H', 'Form H', 'Form-H', 'H Form', 'H-Form') And a.Date Between @sdate and @edate
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='C'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','C',0,0,0,0,'','','','','','',''
   END

--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=A.Gro_Amt,@INV_NO=A.inv_no,@DATE=A.Date,@Party_nm=A.Party_nm,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=A.Form_nm,@S_Tax=A.S_tax
--from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
--INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where a.st_type = 'Out of State'  and BHENT IN('ST') And A.Rform_nm in ('I', 'Form I', 'Form-I', 'I Form', 'I-Form') And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','D',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')


INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','D',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('I', 'Form I', 'Form-I', 'I Form', 'I-Form') And a.Date Between @sdate and @edate
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='D'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','D',0,0,0,0,'','','','','','',''
   END

--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
--INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where b.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('E-1','E - 1','Form E1', 'Form-E1', 'E1 Form', 'E1-Form') And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','E',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','E',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('E-1','E - 1','Form E1', 'Form-E1', 'E1 Form', 'E1-Form') And a.Date Between @sdate and @edate
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='E'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','E',0,0,0,0,'','','','','','',''
   END
   
--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm
--,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
--INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where b.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('E-2', 'E - 2','Form E2', 'Form-E2', 'E2 Form', 'E2-Form') And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','F',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','F',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And A.Rform_nm in ('E-2', 'E - 2','Form E2', 'Form-E2', 'E2 Form', 'E2-Form') And a.Date Between @sdate and @edate
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='F'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','F',0,0,0,0,'','','','','','',''
   END

--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
--select @TAXONAMT=a.VATONAMT,@TAXAMT=A.taxamt,@ITEMAMT=a.Gro_Amt,@INV_NO=a.inv_no,@DATE=a.Date,@Party_nm=a.Party_nm,@Address=c.rec_formno+', '+c.iss_formno,@Form_nm=a.Form_nm,@S_Tax=a.S_tax
--from vattbl A INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
--where b.st_type = 'Out of State' and a.BHENT IN ('ST')
--And A.Rform_nm in ('C','Form C', 'Form-C', 'C Form', 'C-Form')
--And A.form_nm in ('E-2', 'E - 2','Form E2', 'Form-E2', 'E2 Form', 'E2-Form','E-1','E - 1','Form E1', 'Form-E1', 'E1 Form', 'E1-Form')
--And a.Date Between @sdate and @edate
--	SET @Per=CASE WHEN @Per IS NULL THEN 0 ELSE @Per END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
-- VALUES (1,'10H','G',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,'')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
select 1,'10H','G',0,Sum(a.VATONAMT) As VATONAMT,Sum(A.taxamt) As TaxAmt,COUNT(a.VATONAMT),'','','','',a.rform_nm,'',''
from vattbl A 
--INNER JOIN STAX_MAS B on a.BHENT=b.entry_ty  
INNER join VATMAIN_VW c on a.bhent=c.entry_ty and a.tran_cd=c.tran_cd
where a.st_type = 'Out of State' and a.BHENT IN ('ST') And a.Date Between @sdate and @edate
And A.Rform_nm in ('C','Form C', 'Form-C', 'C Form', 'C-Form')
And A.form_nm in ('E-2', 'E - 2','Form E2', 'Form-E2', 'E2 Form', 'E2-Form','E-1','E - 1','Form E1', 'Form-E1', 'E1 Form', 'E1-Form')
group by a.Rform_nm

SET @nCount = 0
Select @nCount=COUNT(PARTSR) From  #FORM221 Where PARTSR = '10H' and SRNO='G'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
IF @nCount = 0
   BEGIN
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
		select 1,'10H','G',0,0,0,0,'','','','','','',''
   END

---Part 10I (Annexure II) B
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','A',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','B',0,0,0,0,'','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','C',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','D',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','E',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','F',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','G',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','H',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','I',0,0,0,0,'','')
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'10I','J',0,0,0,0,'','')

SET @AMTA1 = 0
------Part 11 (Annexure II) B
Declare @VATONAMT as numeric(14,2)
SELECT @VATONAMT=0,@TAXAMT =0,@ITEM='',@PER=0
set @char = 65
Declare cur_Purc cursor for
select Sum(M.vatonamt),Sum(M.TAXAMT),D.item
from vattbl M
inner Join ptitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where M.BHENT In('PT') And M.Date Between @sdate and @edate and m.u_imporm in ('consignment Transfer','STOCK Transfer')
group by d.item
print 'nielsh'
OPEN Cur_Purc
FETCH NEXT FROM Cur_Purc INTO @VATONAMT,@TAXAMT,@ITEM
WHILE (@@FETCH_STATUS=0)
BEGIN
	SET @VATONAMT=CASE WHEN @VATONAMT IS NULL THEN 0 ELSE @VATONAMT END
	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
	
	SET @AMTA1 = @AMTA1 + 1
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
	 VALUES (1,'11',CHAR(@CHAR),0,@VATONAMT,@TAXAMT,@AMTA1,0,				'',@ITEM,'','','','')

	SET @CHAR=@CHAR+1
	FETCH NEXT FROM Cur_purc INTO @VATONAMT,@TAXAMT,@ITEM
END
CLOSE Cur_Purc
DEALLOCATE Cur_Purc

SET @AMTA1 = 0
------Part 12 (Annexure II) B
SELECT @VATONAMT=0,@TAXAMT =0,@ITEM='',@PER=0
set @char = 65
Declare cur_Purc cursor for
select Sum(M.vatonamt),Sum(M.TAXAMT),D.item
from vattbl M
inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where M.BHENT In('ST') And M.Date Between @sdate and @edate and m.u_imporm in ('consignment Transfer','STOCK Transfer')
group by d.item
OPEN Cur_Purc
FETCH NEXT FROM Cur_Purc INTO @VATONAMT,@TAXAMT,@ITEM
WHILE (@@FETCH_STATUS=0)
BEGIN
	SET @VATONAMT=CASE WHEN @VATONAMT IS NULL THEN 0 ELSE @VATONAMT END
	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
	
	SET @AMTA1 = @AMTA1 + 1
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PARTY_NM1) 
	VALUES (1,'12',CHAR(@CHAR),@PER,@VATONAMT,@TAXAMT,@AMTA1,0,'',@ITEM,'','','','')
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM Cur_purc INTO @VATONAMT,@TAXAMT,@ITEM
END
CLOSE Cur_Purc
DEALLOCATE Cur_Purc
--------------------------------------------------------------------------------------------------------

Update #form221 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''), 
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,''),PARTY_NM1=isnull(PARTY_NM1,'') --, Qty = isnull(Qty,0), ITEM =isnull(item,'')


--select part, partsr, srno, isnumeric(substring(partsr,1,3)),substring(partsr,1,case when (isnumeric(substring(partsr,1,3))=1) then 2 else 1 end) from #form221

SELECT * FROM #form221 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int), substring(partsr,3,1) ,SRNO  

--SELECT * FROM #form221 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2)) < 10) then 1 else 2 end) as int), substring(partsr,3,1),SRNO  

--SELECT * FROM #FORM221_1 --order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)

END
--****
--Print 'KA VAT FORM 100'