IF EXISTS(Select [Name] from Sysobjects where xtype = 'P' and [name] = 'USP_REP_KA_FORMVAT115')
BEGIN
	DROP PROCEDURE USP_REP_KA_FORMVAT115
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--
-- =============================================
 -- Author:  Nilesh Yadav
-- Create date: 26/04/2015
-- Description:	This Stored procedure is useful to AMTA11 Karnataka VAT Form 115
-- Modify date: 28/04/2015 - For the bug-26165 by Nilesh Yadav
-- Remark:
-- =============================================
/*
EXECUTE USP_REP_KA_FORMVAT115 '','','','04/01/2015','03/31/2016','','','','',0,0,'','','','','','','','','2011-2012',''
*/

CREATE PROCEDURE [dbo].[USP_REP_KA_FORMVAT115]    
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE SMALLDATETIME,@EDATE SMALLDATETIME    
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
DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTA3 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12
,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)    
DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)    
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)    
    
SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)


----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #FORMVAT_115_A
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #FORMVAT_115_A add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX,PARTY_NM1=ac1.ac_NAME
INTO #FORMVAT_115
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2


----
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
--------------------------------



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
--		SET @SQLCOMMAND='Insert InTo #FORMVAT_115_A Select * from '+@MCON
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
--		SET @SQLCOMMAND='Insert InTo #FORMVAT_115_A Select * from '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--		---Drop Temp Table 
--		SET @SQLCOMMAND='Drop Table '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	End
-----
-----SELECT * from #FORMVAT_115 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----




--------   
Set @AmtA1 = 0
Set @AmtA2 = 0

--Local & InterState Sale
select @AmtA1=Sum(Gro_amt) From vattbl where st_type = 'Local' and BHENT in('ST') And Date Between @sdate and @edate
select @AmtA2=sum(Gro_amt) From vattbl where st_type in( 'Out of State','OUT OF COUNTRY')  and BHENT in('ST') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','A',0,@AMTA1,@AMTA2,0,'','')    



-----Local & InterState Sales Return
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=Sum(gro_amt) From vattbl where st_type = 'Local' and BHENT = 'SR' And Date Between @sdate and @edate
select @AmtA2=sum(gro_Amt) From vattbl where st_type = 'Out of State'  and BHENT = 'SR' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','B',0,@AMTA1,@AMTA2,0,'','')    


---Local & Inter State Consignment Sales
Set @AmtA1 = 0
Set @AmtA2 = 0

SELECT @AMTA1=SUM(gro_amt) FROM vattbl  WHERE ST_TYPE='LOCAL' AND BHENT='ST' AND U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTA2=SUM(gro_AMT) FROM vattbl  WHERE ST_TYPE='Out of State' AND BHENT='ST' AND U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (DATE BETWEEN @SDATE AND @EDATE)
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','C',0,@AMTA1,@AMTA2,0,'','')    



--Local & Inter State Vat collected & exempted
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(TaxAmt) From vattbl where st_type = 'Local' and BHENT = 'ST' And Date Between @sdate and @edate 
select @AmtA2=Sum(gro_amt) From vattbl where ST_TYPE='Out of State' and Per = 0 and TaxAmt = 0 and BHENT = 'ST' And Date Between @sdate and @edate and TAX_NAME='EXEMPTED'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','D',0,@AMTA1,@AMTA2,0,'','')    



--Local exempted & Inter State Export Sales 
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(gro_amt) From vattbl where st_type = 'Local' And Per = 0 and TaxAmt = 0 and BHENT = 'ST' And Date Between @sdate and @edate and TAX_NAME='EXEMPTED'
SELECT @AMTA2=SUM(gro_amt) FROM vattbl WHERE ST_TYPE='OUT OF COUNTRY' AND BHENT='ST' 
AND (DATE BETWEEN @SDATE AND @EDATE) and Form_nm NOT in ('Form H', 'H Form', 'H') AND U_IMPORM <> 'High Sea Sales' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','E',0,@AMTA1,@AMTA2,0,'','')    



--Local Other & Inter State Against Form H Sales
Set @AmtA1 = 0
Set @AmtA2 = 0
SELECT @AMTA2=SUM(gro_amt) FROM vattbl  WHERE ST_TYPE='OUT OF COUNTRY' AND BHENT='ST' 
AND (DATE BETWEEN @SDATE AND @EDATE) and Form_nm in ('Form H', 'H Form', 'H') AND U_IMPORM <> 'High Sea Sales' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','F',0,@AMTA1,@AMTA2,0,'','')    


--Local Sales Total & Inter State E-1 & E-2 Sales
Set @AmtA1 = 0
Set @AmtA2 = 0
Set @AmtB1 = 0
Set @AmtB2 = 0
select @AMTB1=Amt1 From #FORMVAT_115 where PartSr = '2N3' and srno = 'A'
select @AmtB2=sum(Amt1) From #FORMVAT_115 where PartSr = '2N3' and srno in('B','C','D','E','F')
select @AmtA2=Sum(gro_amt) From vattbl  where Tax_name IN ('Form E1', 'Form E2', 'E-1', 'E-2')  and st_type = 'Out of State' and BHENT = 'ST' And Date Between @sdate and @edate
set @AMTA1 = @AMTB1-@AMTB2
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','G',0,@AMTA1,@AMTA2,0,'','')    


--Inter State High Seas Sales
Set @AmtA1 = 0
Set @AmtA2 = 0
SELECT @AMTA2=SUM(gro_amt) FROM vattbl  WHERE ST_TYPE='OUT OF COUNTRY' AND BHENT='ST' AND U_IMPORM='High Sea Sales'  AND (DATE BETWEEN @SDATE AND @EDATE)
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','H',0,0,@AMTA2,0,'','')    


--Inter State Sales CST Collected
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA2=Sum(TaxAmt) From vattbl  where st_type = 'Out of State' and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','I',0,0,@AMTA2,0,'','')    


--Inter State Sales Total
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=Amt2 From #FORMVAT_115 where Partsr = '2N3' and srno = 'A'
select @AmtA2=Sum(Amt2) From #FORMVAT_115 where Partsr = '2N3' and srno in('B','C','D','E','F','G','H','I')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'2N3','J',0,0,@AMTA1-@AMTA2,0,'','')    


-------------------------------------PART II END----------------------------------------------




-----------------------PART V START-------------------------------------------------

---VAT Payable From Bank Payment Details
Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_N as varchar(10)
,@DATE as smalldatetime,@BANK_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50)
,@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4),@INV_NO AS VARCHAR(10)

SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@BANK_NM ='',@ADDRESS ='',@FORM_NM='',@S_TAX =''

Declare @IT_NAME3 as varchar(100),@IT_NAME4 as varchar(100),@IT_NAME5 as varchar(500)

SET @AMTA2=0
SELECT @IT_NAME3='',@IT_NAME4='',@IT_NAME5=''
SET @PER = 0
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
INSERT INTO #FORMVAT_115(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
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
INSERT INTO #FORMVAT_115(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
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

INSERT INTO #FORMVAT_115(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
VALUES				     (1,'5',	 'C',  0, @AMTA2, 0  ,0, @IT_NAME3,'',@IT_NAME4, '','','',@IT_NAME5)
----

---Total
SET @AMTA1=0
SELECT @AMTA1=Sum(Amt1) from #FORMVAT_115 Where Part = 1 And PARTSR = '5' and SRNO <> 'D'
SET @AMTA1=Isnull(@AMTA1,0)
--SELECT @IT_NAME3='',@IT_NAME4='',@IT_NAME5=''
--SET @nCount = 0
--Select @nCount=COUNT(PARTSR) From  #FORMVAT_115 Where PARTSR = '5' and SRNO='D'
--SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END
--IF @nCount = 0
INSERT INTO #FORMVAT_115(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,  DATE,     PARTY_NM,  ADDRESS,FORM_NM,S_TAX,party_nm1)
VALUES				     (1,'5',	 'D',  0, @AMTA1, 0  ,0, 0,'','', '','','','')

---------------------------------------------------------------------------------


-----------------------PART V END-------------------------------------------------





-----------------------PART VI START------------------------------------------------

----Taxable turnover of sales at rate of 1% tax
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=1 and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(taxamt) From vattbl where per=1 and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'6N6','A',0,@AMTA1,@AMTA2,0,'','')    

----Taxable turnover of sales at rate of 4% tax  
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=4 and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(taxamt) From vattbl where per=4 and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'6N6','B',0,@AMTA1,@AMTA2,0,'','')    


-----Taxable turnover of sales at standard rate of tax of 12.5%

Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=12.5 and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(taxamt) From vattbl where per=12.5 and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'6N6','C',0,@AMTA1,@AMTA2,0,'','')    


----Taxable turnover of URD purchases (specify rate of tax)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(VatOnAmt) From vattbl where  BHENT = 'PT' And S_TAX='' And st_type in ('Local','') and Date Between @sdate and @edate
select @AmtA1=Sum(TaxAmt) From vattbl where  BHENT = 'PT'  And S_TAX=''  And st_type in ('Local','') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'6N6','D',0,@AMTA1,@AMTA2,0,'','')    


----Others, if any (please specify)

Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per not in ('1','4','12.5') and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(taxamt) From vattbl where  per not in ('1','4','12.5') and st_type in ('Local','') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'6N6','E',0,@AMTA1,@AMTA2,0,'','')    


----TOTAL
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(Amt1) From #FORMVAT_115 where Partsr = '6N6' 
select @AmtA2=Sum(Amt2) From #FORMVAT_115 where Partsr = '6N6' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'6N6','F',0,@AMTA1,@AMTA2,0,'','')    

------------------------------------------PART VI END-----------------------------------------







------------------------------------------PART VII START-----------------------------------------

---Taxable turnover of inter-State sales at 1%
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=1 and st_type in ('OUT OF STATE','OUT OF COUNTRY') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(taxamt) From vattbl where per=1 and st_type in ('OUT OF STATE','OUT OF COUNTRY') and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'7N7','A',0,@AMTA1,@AMTA2,0,'','')    


----Taxable turnover of inter-State sales against C Forms at 3% tax
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=3 and st_type in ('OUT OF STATE','OUT OF COUNTRY') 
and BHENT = 'ST' And Date Between @sdate and @edate AND form_nm in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM')
select @AmtA2=Sum(taxamt) From vattbl where per=3 and st_type in ('OUT OF STATE','OUT OF COUNTRY') 
and BHENT = 'ST' And Date Between @sdate and @edate AND form_nm in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'7N7','B',0,@AMTA1,@AMTA2,0,'','')    


-----Taxable turnover of inter-State sales without C Forms at 4% tax .
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=4 and st_type in ('OUT OF STATE','OUT OF COUNTRY') 
and BHENT = 'ST' And Date Between @sdate and @edate AND form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM')
select @AmtA2=Sum(taxamt) From vattbl where per=4 and st_type in ('OUT OF STATE','OUT OF COUNTRY') 
and BHENT = 'ST' And Date Between @sdate and @edate AND form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'7N7','C',0,@AMTA1,@AMTA2,0,'','')    


----Taxable turnover of inter-State sales without C Forms at 12.5% tax
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per=12.5 and st_type in ('OUT OF STATE','OUT OF COUNTRY') 
and BHENT = 'ST' And Date Between @sdate and @edate AND form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM')
select @AmtA2=Sum(taxamt) From vattbl where per=12.5 and st_type in ('OUT OF STATE','OUT OF COUNTRY') 
and BHENT = 'ST' And Date Between @sdate and @edate AND form_nm not in ('C', 'FORM-C','C-FORM', 'FORM C', 'C FORM')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'7N7','D',0,@AMTA1,@AMTA2,0,'','')    


---Others, if any (please specify)
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(vatonamt) From vattbl where per not in ('1','3','4','12.5') and st_type in ('OUT OF STATE','OUT OF COUNTRY') and BHENT = 'ST' And Date Between @sdate and @edate
select @AmtA2=Sum(taxamt) From vattbl where per not in ('1','3','4','12.5') and st_type in ('OUT OF STATE','OUT OF COUNTRY')  and BHENT = 'ST' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'7N7','E',0,@AMTA1,@AMTA2,0,'','')    

---TOTAL
Set @AmtA1 = 0
Set @AmtA2 = 0

select @AmtA1=sum(Amt1) From #FORMVAT_115 where Partsr = '7N7' 
select @AmtA2=Sum(Amt2) From #FORMVAT_115 where Partsr = '7N7' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'7N7','F',0,@AMTA1,@AMTA2,0,'','')    


------------------------------------PART VII END--------------------------------------------------------------







------------------------------------PART VIII START--------------------------------------------------------------

select @AmtA1=Sum(Amt2) from #FORMVAT_115 where PartSr = '6N6' and Srno = 'F'
select @AmtA2=Sum(Amt2) from #FORMVAT_115 where PartSr = '7N7' and Srno = 'F'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'8','A',0,@AMTA1+@AMTA2,0,0,'','')

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'8','B',0,0,0,0,'','')

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'8','C',0,@AMTA1+@AMTA2,0,0,'','')

------------------------------------PART VIII END-----------------------------------------------------------------



-----------------------PART IV START-------------------------------------------------
--Update the Part 4.1---- 
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORMVAT_115 where Partsr = '8' and SRNO='C' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','A',0,@AMTA1,0,0,'','')    
--------------------------


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','B',0,0,0,0,'','')    

-----------------------PART IV END-------------------------------------------------





------------------------------------PART IX START-----------------------------------------------------------------


---9.1 Net value of purchases at 1% tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =1 and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt)  from vattbl where Per =1 and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') )
And vattbl.s_tax <> '' And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','A',0,@AMTA1,@AMTA2,0,'','')


-----9.2 Net value of purchases at 4 % tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =4 and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt)  from vattbl where Per =4 and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','B',0,@AMTA1,@AMTA2,0,'','')


--------9.3 Net value of purchases at 12.5 % tax
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per =12.5 and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt)  from vattbl where Per =12.5 and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','C',0,@AMTA1,@AMTA2,0,'','')


----9.4 Value of URD purchases to the extent used or sold (specify rate of tax)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl 
where (vattbl.u_imporm = 'Purchase from Unregistered Dealer' Or vattbl.s_tax = '') and st_type in ('Local','') and BHENT = 'PT' 
And tax_name not in ('Exempted') And Date Between @sdate and @edate
select @AmtA2=Sum(TaxAmt) from vattbl where (vattbl.u_imporm = 'Purchase from Unregistered Dealer' Or vattbl.s_tax = '') 
And tax_name not in ('Exempted') and st_type in ('Local','') and BHENT = 'PT' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','D',0,0,0,0,'','')


----9.5 Others, if any (please specify)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where Per not in ('1','4','12.5') and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate 
select @AmtA2=Sum(TaxAmt)  from vattbl where Per not in ('1','4','12.5') and st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','E',0,@AMTA1,@AMTA2,0,'','')


---9.6 Value of VAT exempted goods.
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=Sum(vatonamt) from vattbl where st_type in ('Local','') and BHENT = 'PT' 
And (vattbl.u_imporm not in ('Purchase from Unregistered Dealer', 'Composition Dealer','Branch Transfer', 'Consignment Transfer') 
And vattbl.s_tax <> '') And tax_name = 'Exempted' And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','F',0,@AMTA1,0,0,'','')


----9.7 Purchases from Composition dealer
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(vatonAmt) From vattbl where u_imporm = 'Composition Dealer'  And vattbl.s_tax <> ''
and BHENT = 'PT' And tax_name not in ('Exempted') And Date Between @sdate and @edate
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','G',0,@AMTA1,0,0,'','')



---9.8 Value of goods imported and / or purchased in the course of inter-State trade including EI and EII purchase.
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(vatonAmt) From vattbl where st_type in('Out of State','Out of Country') 
and BHENT = 'PT' And tax_name not in ('Exempted') 
And Date Between @sdate and @edate and u_imporm not in ('Purchase from Unregistered Dealer', 'Branch Transfer', 'Consignment Transfer', 'Composition Dealer' )
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','H',0,@AMTA1,0,0,'','')



---9.9 Value of goods received by stock transfer / consignment transfer
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(vatonamt) From vattbl where BHENT = 'PT' And Date Between @sdate and @edate 
And tax_name not in ('Exempted') and u_imporm in ('Branch Transfer', 'Consignment Transfer')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','I',0,@AMTA1,0,0,'','')


----9.10 Total value of purchases (Total of Box Nos. 9.1 to 9.9)
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORMVAT_115 where partsr = '9N9'
select @AmtA2=sum(Amt2) From #FORMVAT_115 where partsr = '9N9' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'9N9','J',0,@AMTA1,@AMTA2,0,'','')


----------------------------------------PART IX END------------------------------------------------------------------------------------------


----------------------------------------PART X START------------------------------------------------------------------------------------------

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'10','A',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'10','B',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'10','C',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'10','D',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'10','E',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'10','F',0,0,0,0,'','')    

----------------------------------------PART X END------------------------------------------------------------------------------------------


----------------------------------------PART XI START------------------------------------------------------------------------------------------

select @AmtA1=sum(Amt2) From #FORMVAT_115 where PARTSR='9N9' and SRNO = 'J'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'11','A',0,@AMTA1,0,0,'','')    

----------------------------------------PART XI END------------------------------------------------------------------------------------------


-----PART IV PART ----
select @AmtA1=sum(Amt1) From #FORMVAT_115 where PARTSR='11' and SRNO = 'A'
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','C',0,@AmtA1,0,0,'','')    
--------------------


---------------PART IV START
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(Amt1) From #FORMVAT_115 where partsr = '4' and SRNO='A'
select @AmtA2=sum(Amt1) From #FORMVAT_115 where partsr = '4' and SRNO='B'
select @AmtA3=sum(Amt1) From #FORMVAT_115 where partsr = '4' and SRNO='C'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','D',0,(@AMTA1-(@AMTA2+@AMTA3)),0,0,'','')
----------------------------------------


 INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'4','E',0,0,0,0,'','')    


----TAX DURING THE PERIOD---
SET @AMTA1=0
select @AMTA1=ISNULL(sum(A.GRO_AMT),0) from VATTBL A Inner Join Bpmain B On(a.Bhent = B.Entry_ty And A.Tran_cd = B.Tran_cd)
where A.bhent='BP' AND (A.Date Between @Sdate and @Edate) And B.Party_nm ='Vat Payable' AND B.U_NATURE =''
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','F',0,@AMTA1,0,0,'','')
-------------------------


-------Balance Tax Payable [Box No. 4.4 .. (Box No.4.5 + Box No.4.6)]

Set @AmtA1 = 0
Set @AmtA2 = 0
Set @AMTA3 = 0
select @AmtA1=sum(Amt1) From #FORMVAT_115 where partsr = '4' and SRNO='D'
select @AmtA2=sum(Amt1) From #FORMVAT_115 where partsr = '4' and SRNO='E'
select @AmtA3=sum(Amt1) From #FORMVAT_115 where partsr = '4' and SRNO='F'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTA3=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA3 END
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','G',0,(@AMTA1-(@AMTA2+@AMTA3)),0,0,'','')
----------------

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','H',0,0,0,0,'','')
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','I',0,0,0,0,'','')
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'4','J',0,0,0,0,'','')



----------------------------------------PART XII- A START------------------------------------------------------------------------------------------

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12A','A',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12A','B',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12A','C',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12A','D',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12A','E',0,0,0,0,'','')    

----------------------------------------PART XII -A  END------------------------------------------------------------------------------------------


----------------------------------------PART XII- B START------------------------------------------------------------------------------------------

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12B','A',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12B','B',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12B','C',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12B','D',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'12B','E',0,0,0,0,'','')    

----------------------------------------PART XII- B END------------------------------------------------------------------------------------------




----------------------------------------PART XIII- A START------------------------------------------------------------------------------------------

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','A',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','B',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','C',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','D',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','E',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','F',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','G',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13A','H',0,0,0,0,'','')    

----------------------------------------PART XII- B END------------------------------------------------------------------------------------------



----------------------------------------PART XIII- A START------------------------------------------------------------------------------------------

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13B','A',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13B','B',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13B','C',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13B','D',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13B','E',0,0,0,0,'','')    
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'13B','F',0,0,0,0,'','')    
----------------------------------------PART XII- B END------------------------------------------------------------------------------------------



Declare @IT_NAME As varchar(200),@IT_NAME1 As varchar(50),@AMTA11 as numeric(12,2)
Declare @PARTY_NM1 as varchar(100),@PARTY_NM as varchar(100)
Declare @IT_NAME2 as varchar(100)

----------------------------------------PART XIII- A START------------------------------------------------------------------------------------------

Select @Party_nm=''
select @PARTY_NM1=''

----Taxable turnover of sales at rate of 4% tax  and  1%
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.per=4 and M.st_type in ('LOCAL','') and m.BHENT = 'ST' And m.Date Between @sdate and @edate

select @AmtA2=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where (m.per=1 or (m.per <> 4 and m.per <> 12.5)) and M.TAX_NAME <> 'EXEMPTED'  and M.st_type in ('LOCAL','') and m.BHENT = 'ST' And m.Date Between @sdate and @edate

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'14','A',0,@AMTA1,@AMTA2,0,'','')    


---------1st commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and D.it_code = M.it_code)    
     where D.per=4 and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row=1


SELECT @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
       where (d.per=1 or (d.per <> 4 and d.per <> 12.5)) and D.TAX_NAME <> 'EXEMPTED'  and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
        group by M.item
       ) AS EMP 
Where row=1


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'14','B',0,@AMTA1,@AMTA2,0,@PARTY_NM,@PARTY_NM1)    


-----2nd Commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT  @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and D.it_code = m.it_code)    
     where D.per=4 and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row=2


SELECT  @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
     where (d.per=1 or (d.per <> 4 and d.per <> 12.5 )) and D.TAX_NAME <> 'EXEMPTED'  and D.st_type in ('LOCAL','')  and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row=2
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'14','C',0,@AMTA1,@AMTA2,0,@PARTY_NM,@PARTY_NM1)    



---Other

select @IT_NAME='',@AMTA1=0 ,@IT_NAME1='',@AMTA11=0
declare Cur_VatPay cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
     where D.per=4 and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @IT_NAME,@AMTA1
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME1=RTRIM(@IT_NAME)+','+@IT_NAME1
	SET @AMTA11=isnull(@AMTA1,0)+@AMTA11
	
FETCH NEXT FROM CUR_VatPay INTO @IT_NAME,@AMTA1
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay



Select @IT_NAME2='',@IT_NAME3='',@AMTA3=0,@AMTA2=0
declare Cur_VatPay1 cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
     where (d.per=1 or (d.per <> 4 and d.per <> 12.5)) and D.TAX_NAME <> 'EXEMPTED' and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row>2
 
open Cur_VatPay1
FETCH NEXT FROM Cur_VatPay1 INTO @IT_NAME2,@AMTA2
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME3=RTRIM(@IT_NAME2)+','+@IT_NAME3
	SET @AMTA3=isnull(@AMTA2,0)+@AMTA3		
FETCH NEXT FROM CUR_VatPay1 INTO @IT_NAME2,@AMTA2
END
CLOSE CUR_VatPay1
DEALLOCATE CUR_VatPay1


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,party_nm1) VALUES  (1,'14','D',0,@AMTA11,@AMTA3,0,@IT_NAME1,@IT_NAME3)    

----------------------------------------------------14.1 / 14.3 START-----------------------------------



----------------------------------------------------14.2 / 14.4 START-----------------------------------


Select @PARTY_NM=''
Select @PARTY_NM1=''
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.per=12.5 and M.st_type in ('LOCAL','') and m.BHENT = 'ST' And m.Date Between @sdate and @edate

select @AmtA2=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where M.ST_TYPE in ('LOCAL','') and M.Per = 0 and M.TaxAmt = 0 and M.BHENT = 'ST' And m.Date Between @sdate and @edate and M.TAX_NAME='EXEMPTED'

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'14','E',0,@AMTA1,@AMTA2,0,'','')    
---------------------------------------------------



---------1st commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
       where D.per=12.5 and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
        group by M.item
       ) AS EMP 
Where row=1

SELECT @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd AND d.it_code = m.it_code)    
       where D.ST_TYPE in ('LOCAL','') and d.Per = 0 and d.TaxAmt = 0 and d.BHENT = 'ST' And d.Date Between @sdate and @edate and d.TAX_NAME='EXEMPTED'
        group by M.item
       ) AS EMP 
Where row=1

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'14','F',0,@AMTA1,@AMTA2,0,@Party_nm,@PARTY_NM1)    



-----2nd Commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT  @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
     where D.per=12.5 and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row=2

SELECT  @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
     where D.ST_TYPE in ('LOCAL','') and d.Per = 0 and d.TaxAmt = 0 and d.BHENT = 'ST' And d.Date Between @sdate and @edate and d.TAX_NAME='EXEMPTED'
     group by M.item
       ) AS EMP 
Where row=2

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'14','G',0,@AMTA1,@AMTA2,0,@Party_nm,@PARTY_NM1)    



--Other

select @IT_NAME='',@AMTA1=0 ,@IT_NAME1='',@AMTA11=0
SET @CHAR=65
SET @PER = 0
declare Cur_VatPay cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
     where D.per=12.5 and D.st_type in ('LOCAL','') and D.BHENT = 'ST' And D.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @IT_NAME,@AMTA1
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME1=RTRIM(@IT_NAME)+','+@IT_NAME1
	SET @AMTA11=isnull(@AMTA1,0)+@AMTA11
	
FETCH NEXT FROM CUR_VatPay INTO @IT_NAME,@AMTA1
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay




Select @IT_NAME2='',@IT_NAME3='',@AMTA3=0,@AMTA2=0
declare Cur_VatPay1 cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
    where D.ST_TYPE in ('LOCAL','') and d.Per = 0 and d.TaxAmt = 0 and d.BHENT = 'ST' And d.Date Between @sdate and @edate and d.TAX_NAME='EXEMPTED'
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay1
FETCH NEXT FROM Cur_VatPay1 INTO @IT_NAME2,@AMTA2
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME3=RTRIM(@IT_NAME2)+','+@IT_NAME3
	SET @AMTA3=isnull(@AMTA2,0)+@AMTA3
FETCH NEXT FROM CUR_VatPay1 INTO @IT_NAME2,@AMTA2
END
CLOSE CUR_VatPay1
DEALLOCATE CUR_VatPay1


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,party_nm1) VALUES  (1,'14','H',0,@AMTA11,@AMTA3,0,@IT_NAME1,@IT_NAME3)    


----------------------------------------PART XII- B END------------------------------------------------------------------------------------------








----------------------------------------PART XIII- A START------------------------------------------------------------------------------------------


------------------------------------------------15.1 \15.4 START------------------------------------------------
Select @PARTY_NM=''
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.per=4 and m.st_type in ('OUT OF STATE','OUT OF COUNTRY') and m.BHENT = 'ST' 
and m.form_nm in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM') And m.Date Between @sdate and @edate


select @AmtA2=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.ST_TYPE in ('OUT OF STATE','OUT OF COUNTRY') and m.Per = 0 and m.TaxAmt = 0 and m.BHENT = 'ST' 
And m.Date Between @sdate and @edate and m.TAX_NAME='EXEMPTED'


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','A',0,@AMTA1,@AMTA2,0,'','')    
---------------------------------------------------


---------1st commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.per=4 and d.st_type in ('OUT OF STATE') and d.BHENT = 'ST' And d.Date Between @sdate and @edate
		and d.form_nm in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
        group by M.item
       ) AS EMP 
Where row=1


SELECT @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.ST_TYPE in ('OUT OF STATE','OUT OF COUNTRY') and d.Per = 0 and d.TaxAmt = 0 and d.BHENT = 'ST' 
		And d.Date Between @sdate and @edate and d.TAX_NAME='EXEMPTED'
        group by M.item
       ) AS EMP 
Where row=1


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES (1,'15','B',0,@AMTA1,@AMTA2,0,@PARTY_NM,@PARTY_NM1)    

print 'nilesh pankaj'
-----2nd Commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT  @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.per=4 and d.st_type in ('OUT OF STATE') and d.BHENT = 'ST' 
	And d.Date Between @sdate and @edate
	and d.form_nm in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
     group by M.item
       ) AS EMP 
Where row=2


SELECT  @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.ST_TYPE in ('OUT OF STATE','OUT OF COUNTRY') and d.Per = 0 and d.TaxAmt = 0 and d.BHENT = 'ST' And d.Date Between @sdate and @edate and d.TAX_NAME='EXEMPTED'
     group by M.item
       ) AS EMP 
Where row=2


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','C',0,@AMTA1,@AMTA2,0,@PARTY_NM,@PARTY_NM1)    

--Other

select @IT_NAME='',@AMTA1=0 ,@IT_NAME1='',@AMTA11=0

SET @CHAR=65
SET @PER = 0
declare Cur_VatPay cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.per=4 and d.st_type in ('OUT OF STATE') and d.BHENT = 'ST' And d.Date Between @sdate and @edate
	and d.form_nm in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @IT_NAME,@AMTA1
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME1=RTRIM(@IT_NAME)+','+@IT_NAME1
	SET @AMTA11=isnull(@AMTA1,0)+@AMTA11
	
FETCH NEXT FROM CUR_VatPay INTO @IT_NAME,@AMTA1
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay


select @IT_NAME3='' ,@IT_NAME2='',@AMTA3=0,@AMTA2=0
declare Cur_VatPay1 cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.ST_TYPE  in ('OUT OF STATE','OUT OF COUNTRY') and d.Per = 0 and d.TaxAmt = 0 and d.BHENT = 'ST' 
	And d.Date Between @sdate and @edate and d.TAX_NAME='EXEMPTED'
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay1
FETCH NEXT FROM Cur_VatPay1 INTO @IT_NAME2,@AMTA2
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME3=RTRIM(@IT_NAME2)+','+@IT_NAME3
	SET @AMTA3=isnull(@AMTA2,0)+@AMTA3
	
FETCH NEXT FROM CUR_VatPay1 INTO @IT_NAME2,@AMTA2
END
CLOSE CUR_VatPay1
DEALLOCATE CUR_VatPay1


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','D',0,@AMTA11,@AMTA3,0,@IT_NAME1,@IT_NAME3)    

----------------------------------------------------------15.1 / 15.5 END-----------------------------------------------------------------



-----------------------------------------------------------15.2 / 15.6 START--------------------------------------------------------------

-----------------------start
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.per=12.5 and m.st_type in ('OUT OF STATE') and m.BHENT = 'ST' And m.Date Between @sdate and @edate
and m.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')

select @AmtA2=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
WHERE m.ST_TYPE  in ('OUT OF COUNTRY') AND m.BHENT='ST' 
AND (m.DATE BETWEEN @SDATE AND @EDATE) and m.Form_nm in('','Form H', 'F Form', 'H') AND m.U_IMPORM <> 'High Sea Sales'


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','E',0,@AMTA1,@AMTA2,0,'','')    



---------1st commodity
SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0

SELECT @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.per=12.5 and d.st_type in ('OUT OF STATE') and D.BHENT = 'ST' And m.Date Between @sdate and @edate
		and d.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
        group by M.item
       ) AS EMP 
Where row=1


SELECT @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		WHERE d.ST_TYPE in ('OUT OF COUNTRY') AND d.BHENT='ST' AND (d.DATE BETWEEN @SDATE AND @EDATE) 
		and d.Form_nm in('','Form H', 'F Form', 'H') AND D.U_IMPORM <> 'High Sea Sales'
        group by M.item
       ) AS EMP 
Where row=1
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','F',0,@AMTA1,@AMTA2,0,@Party_nm,@Party_nm1)    



-----2nd Commodity

SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0

SELECT  @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.per=12.5 and d.st_type in ('OUT OF STATE') and D.BHENT = 'ST' And m.Date Between @sdate and @edate
	and d.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
     group by M.item
       ) AS EMP 
Where row=2

SELECT  @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.gro_amt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		WHERE d.ST_TYPE in ('OUT OF COUNTRY') AND d.BHENT='ST' AND (d.DATE BETWEEN @SDATE AND @EDATE) 
		and d.Form_nm in('','Form H', 'F Form', 'H') AND D.U_IMPORM <> 'High Sea Sales'
     group by M.item
       ) AS EMP 
Where row=2


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','G',0,@AMTA1,@AMTA2,0,@Party_nm,@Party_nm1)    

--Other

select @IT_NAME='',@AMTA1=0 ,@IT_NAME1='',@AMTA11=0
SET @CHAR=65
SET @PER = 0
declare Cur_VatPay cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.per=12.5 and d.st_type in ('OUT OF STATE') and D.BHENT = 'ST' And m.Date Between @sdate and @edate
	and d.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @IT_NAME,@AMTA1
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME1=RTRIM(@IT_NAME)+','+@IT_NAME1
	SET @AMTA11=isnull(@AMTA1,0)+@AMTA11
	
FETCH NEXT FROM CUR_VatPay INTO @IT_NAME,@AMTA1
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay



select @IT_NAME3='' ,@IT_NAME2='',@AMTA3=0,@AMTA2=0
declare Cur_VatPay1 cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	WHERE d.ST_TYPE in ('OUT OF COUNTRY') AND d.BHENT='ST' AND (d.DATE BETWEEN @SDATE AND @EDATE) 
	and d.Form_nm in('','Form H', 'F Form', 'H') AND D.U_IMPORM <> 'High Sea Sales'
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay1
FETCH NEXT FROM Cur_VatPay1 INTO @IT_NAME2,@AMTA2
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME3=RTRIM(@IT_NAME2)+','+@IT_NAME3
	SET @AMTA3=isnull(@AMTA2,0)+@AMTA3
	
FETCH NEXT FROM CUR_VatPay1 INTO @IT_NAME2,@AMTA2
END
CLOSE CUR_VatPay1
DEALLOCATE CUR_VatPay1

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','H',0,@AMTA1,@AMTA2,0,@PARTY_NM,@PARTY_NM1)    



-----------------------------------------------------15.2 /15.6 END---------------------------------------------------------------------------------------



-----------------------------------------------------15.3/15.7 START---------------------------------------------------------------------------

Select @PARTY_NM=''
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.per=10 and m.st_type in ('OUT OF STATE') and m.BHENT = 'ST' And m.Date Between @sdate and @edate
and m.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')

select @AmtA2=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd)
where m.Tax_name in ('Form E1', 'Form E2', 'E-1', 'E-2')  and m.st_type in ('OUT OF STATE','OUT OF COUNTRY') and m.BHENT = 'ST' And m.Date Between @sdate and @edate

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','I',0,@AMTA1,@AMTA2,0,'','')    


---------1st commodity

SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0

SELECT @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.per=10 and d.st_type in ('OUT OF STATE') and D.BHENT = 'ST' And m.Date Between @sdate and @edate
		and d.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
        group by M.item
       ) AS EMP 
Where row=1


SELECT @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.Tax_name in ('Form E1', 'Form E2', 'E-1', 'E-2')  and d.st_type in ('OUT OF STATE','OUT OF COUNTRY') and d.BHENT = 'ST' And d.Date Between @sdate and @edate
        group by M.item
       ) AS EMP 
Where row=1

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','J',0,@AMTA1,@AMTA2,0,@Party_nm,@Party_nm1)    


-----2nd Commodity

SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT  @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.per=10 and d.st_type in ('OUT OF STATE') and D.BHENT = 'ST' And m.Date Between @sdate and @edate
	and d.form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
     group by M.item
       ) AS EMP 
Where row=2


SELECT  @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.Tax_name in ('Form E1', 'Form E2', 'E-1', 'E-2')  and d.st_type in ('OUT OF STATE','OUT OF COUNTRY') and d.BHENT = 'ST' And d.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row=2

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','K',0,@AMTA1,@AMTA2,0,@Party_nm,@Party_nm1)    


--Other


select @IT_NAME='',@AMTA1=0 ,@IT_NAME1='',@AMTA11=0

SET @CHAR=65
SET @PER = 0
declare Cur_VatPay cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	where d.per=10 and d.st_type in ('OUT OF STATE') and D.BHENT = 'ST' And m.Date Between @sdate and @edate
	and form_nm not in ('C', 'D', 'FORM-C','C-FORM', 'FORM C', 'C FORM', 'FORM-D','D-FORM', 'FORM D', 'D FORM')
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @IT_NAME,@AMTA1
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME1=RTRIM(@IT_NAME)+','+@IT_NAME1
	SET @AMTA11=isnull(@AMTA1,0)+@AMTA11
	
FETCH NEXT FROM CUR_VatPay INTO @IT_NAME,@AMTA1
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay


select @IT_NAME3='' ,@IT_NAME2='',@AMTA3=0,@AMTA2=0
declare Cur_VatPay1 cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		where d.Tax_name in ('Form E1', 'Form E2', 'E-1', 'E-2')  and d.st_type in ('OUT OF STATE','OUT OF COUNTRY') and d.BHENT = 'ST' And d.Date Between @sdate and @edate
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay1
FETCH NEXT FROM Cur_VatPay1 INTO @IT_NAME2,@AMTA2
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME3=RTRIM(@IT_NAME2)+','+@IT_NAME3
	SET @AMTA3=isnull(@AMTA2,0)+@AMTA3
FETCH NEXT FROM CUR_VatPay1 INTO @IT_NAME2,@AMTA2
END
CLOSE CUR_VatPay1
DEALLOCATE CUR_VatPay1

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','L',0,@AMTA11,@AMTA3,0,@IT_NAME1,@IT_NAME3)    

----------------------------------------15.3/15.7 END------------------------------------------------------------




----------------------------------------------15.4/15.8 START-----------------------------------------------------------

Select @PARTY_NM=''
Set @AmtA1 = 0
Set @AmtA2 = 0
select @AmtA1=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd and d.it_code = m.it_code)
 WHERE m.ST_TYPE='Out of State' AND m.BHENT='ST' AND m.U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (m.DATE BETWEEN @SDATE AND @EDATE)
 
 
select @AmtA2=sum(m.vatonamt)
From vattbl M 
--inner Join stitem D on (M.Bhent = D.Entry_ty and M.Tran_cd = D.Tran_cd and d.it_code = m.it_code)
WHERE m.ST_TYPE='OUT OF COUNTRY' AND m.BHENT='ST' AND m.U_IMPORM='High Sea Sales'  AND (m.DATE BETWEEN @SDATE AND @EDATE)

 
INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','M',0,@AMTA1,@AMTA2,0,'','')    
---------------------------------------------------



---------1st commodity

SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		WHERE d.ST_TYPE='Out of State' AND d.BHENT='ST' AND d.U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (d.DATE BETWEEN @SDATE AND @EDATE)
        group by M.item
       ) AS EMP 
Where row=1


SELECT @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
       FROM Stitem M
       Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	WHERE d.ST_TYPE='OUT OF COUNTRY' AND d.BHENT='ST' AND d.U_IMPORM='High Sea Sales'  AND (d.DATE BETWEEN @SDATE AND @EDATE)
        group by M.item
       ) AS EMP 
Where row=1

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','N',0,@AMTA1,@AMTA2,0,@Party_nm,@Party_nm1)    



-----2nd Commodity

SET @PARTY_NM=''
SET @PARTY_NM1=''
SET @AMTA1=0
SET @AMTA2=0
SELECT  @Party_nm=item,@AmtA1=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		WHERE d.ST_TYPE='Out of State' AND d.BHENT='ST' AND d.U_IMPORM in ('Branch Transfer', 'Consignment Transfer') AND (d.DATE BETWEEN @SDATE AND @EDATE)
     group by M.item
       ) AS EMP 
Where row=2

SELECT  @Party_nm1=item,@AmtA2=vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
       M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
     Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	WHERE d.ST_TYPE='OUT OF COUNTRY' AND d.BHENT='ST' AND d.U_IMPORM='High Sea Sales'  AND (d.DATE BETWEEN @SDATE AND @EDATE)
     group by M.item
       ) AS EMP 
Where row=2

INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','O',0,@AMTA1,@AMTA2,0,@Party_nm,@Party_nm1)    



--Other

select @IT_NAME='',@AMTA1=0 ,@IT_NAME1='',@AMTA11=0
declare Cur_VatPay cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
		WHERE d.ST_TYPE='Out of State' AND d.BHENT='ST' AND d.U_IMPORM in ('Branch Transfer', 'Consignment Transfer')  AND (d.DATE BETWEEN @SDATE AND @EDATE)
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @IT_NAME,@AMTA1
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME1=RTRIM(@IT_NAME)+','+@IT_NAME1
	SET @AMTA11=isnull(@AMTA1,0)+@AMTA11
	
FETCH NEXT FROM CUR_VatPay INTO @IT_NAME,@AMTA1
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay



select @IT_NAME3='' ,@IT_NAME2='',@AMTA3=0,@AMTA2=0
declare Cur_VatPay1 cursor  for
SELECT item,vatonamt FROM (SELECT ROW_NUMBER() 
      OVER (order by sum(d.vatonamt) desc) AS Row, 
        M.item,(sum(d.vatonamt)) as vatonamt
    FROM Stitem M
    Inner join vattbl D on (D.Bhent = M.Entry_ty and D.Tran_cd = M.Tran_cd and d.it_code = m.it_code)    
	WHERE d.ST_TYPE='OUT OF COUNTRY' AND d.BHENT='ST' AND d.U_IMPORM='High Sea Sales'  AND (d.DATE BETWEEN @SDATE AND @EDATE)
     group by M.item
       ) AS EMP 
Where row>2

open Cur_VatPay1
FETCH NEXT FROM Cur_VatPay1 INTO @IT_NAME2,@AMTA2
WHILE (@@FETCH_STATUS=0)
 BEGIN
	SET @IT_NAME3=RTRIM(@IT_NAME2)+','+@IT_NAME3
	SET @AMTA3=isnull(@AMTA2,0)+@AMTA3
	
FETCH NEXT FROM CUR_VatPay1 INTO @IT_NAME2,@AMTA2
END
CLOSE CUR_VatPay1
DEALLOCATE CUR_VatPay1


INSERT INTO #FORMVAT_115 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM,PARTY_NM1) VALUES  (1,'15','P',0,@AMTA11,@AMTA3,0,@IT_NAME1,@IT_NAME3)    


------------------------------------------------------15.4/15.8 END----------------------------------------------
----------------------------------------PART XII- B END------------------------------------------------------------------------------------------



Update #FORMVAT_115 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''), 
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,''),PARTY_NM1 = isnull(party_nm1,'')  --, Qty = isnull(Qty,0), ITEM =isnull(item,'')


    
SELECT * FROM #FORMVAT_115 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)    
  
    
END
----Print 'KA VAT FORM 115'

