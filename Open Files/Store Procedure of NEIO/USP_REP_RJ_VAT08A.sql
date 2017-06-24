IF EXISTS(SELECT XTYPE,NAME FROM SYSOBJECTS WHERE XTYPE='P' AND name='USP_REP_RJ_VAT08A')
BEGIN
	DROP PROCEDURE USP_REP_RJ_VAT08A
END
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nilesh Yadav.
-- Create date: 20-02-2015
-- Description:	This Stored procedure is useful to Generate Rajasthan VAT FORM 08 A
-- Modify date: 
-- Modified By: 
-- Modify date: 
-- Remark:    : 
-- EXECUTE USP_REP_RJ_VAT08A '','','','04/01/2014','03/31/2015','','','','',0,0,'','','','','','','','','2014-2015',''
-- ============================================= 


CREATE PROCEDURE [dbo].[USP_REP_RJ_VAT08A]
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
DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@vatdate as smalldatetime,@CHAR INT,@LEVEL NUMERIC(12,2),@BANK_NM VARCHAR(250),@BK_CITY VARCHAR(250),@BK_BRANCH VARCHAR(250),@LSTDTPAY DATETIME, @TAXONAMT as numeric(12,2)


Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

---- TEMPORARY TABLE START------------------------
SELECT PART=3,PARTSR='AAA',SRNO='AAA',AMT1=NET_AMT,AMT2=M.TAXAMT,
PARTY_NM=AC1.AC_NAME,AC1.S_TAX
INTO #FORMRJ07A
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
---- TEMPORARY TABLE END------------------------

-----------CHECKING MULTI COMPANY OR NOT------------------
Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin
		 Set @MultiCo = 'YES'
		 EXECUTE USP_REP_MULTI_CO_DATA
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		
		SET @SQLCOMMAND='Insert InTo  vattbl Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
else
	Begin 
		 Set @MultiCo = 'NO'
		 EXECUTE USP_REP_SINGLE_CO_DATA_VAT 
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT
		
	End
------------------------------------------------------------

---------------------------- PART I START-----------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
declare @Party_nm as varchar(50),@S_TAX as varchar(30),@Amt1 as numeric(18,4),@Amt2 as numeric(18,4)
Select @Party_nm='',@S_TAX='',@AMT1=0, @AMT2=0 

Declare CUR_FORMRJ07Aaaa cursor for

SELECT party_nm=a.ac_name,s_tax=a.s_tax,AMTA1=sum(A.VATONAMT),AMT2=SUM(A.TAXAMT)
FROM VATTBL A
WHERE  A.BHENT='ST' AND (A.DATE BETWEEN @SDATE AND @EDATE) and a.st_type in('Local','') and a.s_tax<>''
group by a.ac_name,a.s_tax,a.vatonamt,a.taxamt

OPEN CUR_FORMRJ07Aaaa
FETCH NEXT FROM CUR_FORMRJ07Aaaa INTO @Party_nm,@S_TAX,@AMT1,@AMT2
WHILE (@@FETCH_STATUS=0)
BEGIN

	
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @AMT1=CASE WHEN @AMT1 IS NULL THEN 0 ELSE @AMT1 END
	SET @AMT2=CASE WHEN @AMT2 IS NULL THEN 0 ELSE @AMT2 END
	
    INSERT INTO #FORMRJ07A (PART,PARTSR,SRNO,PARTY_NM,S_TAX,AMT1,AMT2) VALUES(1,'1',CHAR(@CHAR),@PARTY_NM,@S_TAX,@AMT1,@AMT2)
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORMRJ07Aaaa INTO @Party_nm,@S_TAX,@AMT1,@AMT2
END
CLOSE CUR_FORMRJ07Aaaa
DEALLOCATE CUR_FORMRJ07Aaaa

----------------------------------------- PART I END--------------------------------------------------------------------



-----------------------------------------PART II START---------------------------------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
--declare@Party_nm as varchar(50),@S_TAX as varchar(30),@Amt1 as numeric(18,4),@Amt2 as numeric(18,4)
Select @Party_nm='',@S_TAX='',@AMT1=0, @AMT2=0 
Declare CUR_FORMRJ07Aaaa cursor for

SELECT party_nm=a.ac_name,s_tax=a.s_tax,AMTA1=sum(A.VATONAMT),AMT2=SUM(A.TAXAMT)
FROM VATTBL A
WHERE  A.BHENT='ST' AND (A.DATE BETWEEN @SDATE AND @EDATE) and a.st_type in('Local','')
group by a.ac_name,a.s_tax,a.vatonamt,a.taxamt

OPEN CUR_FORMRJ07Aaaa
FETCH NEXT FROM CUR_FORMRJ07Aaaa INTO @Party_nm,@S_TAX,@AMT1,@AMT2
WHILE (@@FETCH_STATUS=0)
BEGIN

	
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @AMT1=CASE WHEN @AMT1 IS NULL THEN 0 ELSE @AMT1 END
	SET @AMT2=CASE WHEN @AMT2 IS NULL THEN 0 ELSE @AMT2 END
	
    INSERT INTO #FORMRJ07A (PART,PARTSR,SRNO,PARTY_NM,S_TAX,AMT1,AMT2) VALUES(1,'2',CHAR(@CHAR),@PARTY_NM,@S_TAX,@AMT1,@AMT2)
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORMRJ07Aaaa INTO @Party_nm,@S_TAX,@AMT1,@AMT2
END
CLOSE CUR_FORMRJ07Aaaa
DEALLOCATE CUR_FORMRJ07Aaaa

-----------------------------------------PART II END-----------------------------------------


---------------------------------------------------------------------------------------------

------------------------------------- CHEHKING NULL DATA AVAILABLE----------------
DECLARE @nCount AS NUMERIC(4)
-- ---------------------------------Part - I Check
SET @nCount = 0

Select @nCount=COUNT(PARTSR) From #FORMRJ07A Where PARTSR = '1'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END

IF @nCount = 0
	INSERT INTO #FORMRJ07A
	    (PART,PARTSR,SRNO,PARTY_NM,S_TAX,AMT1,AMT2)
	    VALUES
	    (1,'1',        '',  '',      '',  0,   0)


------------------------------------Part - II Check
SET @nCount = 0

Select @nCount=COUNT(PARTSR) From #FORMRJ07A Where PARTSR = '2'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END

IF @nCount = 0
INSERT INTO #FORMRJ07A (PART,PARTSR,SRNO,PARTY_NM,S_TAX,AMT1,AMT2)
				VALUES  (1,'2',        '',  '',      '',  0,   0)
				
				

------------------------------------Part - III Check		
		
SET @nCount = 0

Select @nCount=COUNT(PARTSR) From #FORMRJ07A Where PARTSR = '3'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END

IF @nCount = 0
	INSERT INTO #FORMRJ07A
	    (PART,PARTSR,SRNO,PARTY_NM,S_TAX,AMT1,AMT2)
	    VALUES
	    (1,'3',        '',  '',      '',  0,   0)				

--------------------------------------------------------------------------------------------------


----------------------------UPDATE THE TEMPORARY TABLE------------------------------------------------
Update #FORMRJ07A set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
						PARTY_NM = isnull(Party_nm,''),S_TAX = isnull(S_TAX,''),
		               AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0)       
		             
---------------------------------------------------------------------------------		

SELECT * FROM #FORMRJ07A order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
END
