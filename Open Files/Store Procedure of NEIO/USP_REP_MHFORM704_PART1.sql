If exists(Select * from sysobjects where [name]='USP_REP_MHFORM704_PART1' and xtype='P')
Begin
	Drop Procedure USP_REP_MHFORM704_PART1
End
go


-- =============================================
-- Author:		Pankaj M Borse.
-- Create date: 23/08/2014
-- Description:	This Stored procedure is useful to generate MH VAT FORM 704 Letter Of Submission
-- Modify date: 23/08/2014
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_MHFORM704_PART1]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(1000)= null
AS
BEGIN
Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2)
--EXECUTE   USP_REP_FILTCON 
--@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
--,@VSDATE=NULL
--,@VEDATE=@EDATE
--,@VSAC =@SAC,@VEAC =@EAC
--,@VSIT=@SIT,@VEIT=@EIT
--,@VSAMT=@SAMT,@VEAMT=@EAMT
--,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
--,@VSCATE =@SCATE,@VECATE =@ECATE
--,@VSWARE =@SWARE,@VEWARE  =@EWARE
--,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
--,@VMAINFILE='M',@VITFILE=NULL,@VACFILE=NULL
--,@VDTFLD ='DATE'
--,@VLYN=NULL
--,@VEXPARA=@EXPARA
--,@VFCON =@FCON OUTPUT

DECLARE @SQLCOMMAND NVARCHAR(4000)
DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)
DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2),@PARTY_NM VARCHAR(50),@MCON as NVARCHAR(2000)

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)
SELECT PARTSR='AAA',SRNO='AAAAAA',AMT1=CAST('0' AS NUMERIC(20,2)),AMT2=CAST('0' AS NUMERIC(20,2)),AMT3=CAST('0' AS NUMERIC(20,2)),
M.INV_NO,M.DATE,PARTY_NM=AC.AC_NAME,ADDRESS=Ltrim(AC.Add1),ACT=SPACE(10)
INTO #FORM704
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
WHERE 1=2



Declare @AUDIT_NM varchar(50),@CERTI_BY varchar(50),@Auditor_Firm varchar(50),@Transation_Id varchar(15),@N_autho VARCHAR(50),@Designation VARCHAR(25)
Declare @POS INT,@Emailid varchar(30),@mobileno varchar(10),@Place varchar(15),@VATNO VARCHAR(15),@CSTNO VARCHAR(15)
DECLARE @TAXAUDIT_NM varchar(50),@TAXAUDIT_DT VARCHAR(10),@Profit_loss varchar(10),@Balance_sheet varchar(10),@ACT_NAME varchar(30),@ACT_DT varchar(10)
DECLARE @YEAR_END VARCHAR(10),@BOOKS_OF_ACOUNT VARCHAR(10),@BOOKS_OF_ACOUNT_YR VARCHAR(10),@Dealer_ISREQ VARCHAR(10),@Dealer_return VARCHAR(10)
DECLARE @Dealer_stock_register VARCHAR(10),@MVAT_RET VARCHAR(10),@Form_405_RET VARCHAR(10),@CST_RET VARCHAR(10),@Schedule_I varchar(1),@Schedule_II varchar(1)
DECLARE @Schedule_III varchar(1),@Schedule_IV varchar(1),@Schedule_V varchar(1),@Schedule_VI varchar(1),@Annexure_A VARCHAR(1) ,@Annexure_B VARCHAR(1),@Annexure_C VARCHAR(1)
DECLARE @Annexure_D VARCHAR(1),@Annexure_E VARCHAR(1),@Annexure_F VARCHAR(1),@Annexure_G VARCHAR(1),@Annexure_H VARCHAR(1),@Annexure_I VARCHAR(1),@Annexure_J_1 VARCHAR(1)
DECLARE @Annexure_J_2 VARCHAR(1),@Annexure_J_5 VARCHAR(1),@Annexure_J_6 VARCHAR(1),@Annexure_K VARCHAR(1),@MEM_NO VARCHAR(20),@REG_NO VARCHAR(20),@ADDRESS VARCHAR(100),@Audit_DT varchar(10)
if(charindex('[Tax_audit_name=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX(']',@EXPARA)
	SET @TAXAUDIT_NM=SUBSTRING(@EXPARA,17,@POS-17)
end 
if(charindex('[Tax_audit_DT=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Tax_audit_DT=',@EXPARA)
	SET @TAXAUDIT_DT=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
end 
if(charindex('[Profit_loss=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Profit_loss=',@EXPARA)
	SET @Profit_loss=SUBSTRING(@EXPARA,@POS+13,CHARINDEX(']',@EXPARA,@pos)-(@pos+13))
end 
if(charindex('[Balance_sheet=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Balance_sheet=',@EXPARA)
	SET @Balance_sheet=SUBSTRING(@EXPARA,@POS+15,CHARINDEX(']',@EXPARA,@pos)-(@pos+15))
end 

if(charindex('[ACT_NAME=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[ACT_NAME=',@EXPARA)
	SET @ACT_NAME=SUBSTRING(@EXPARA,@POS+10,CHARINDEX(']',@EXPARA,@pos)-(@pos+10))
end 
if(charindex('[ACT_DT=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[ACT_DT=',@EXPARA)
	SET @ACT_DT=SUBSTRING(@EXPARA,@POS+8,CHARINDEX(']',@EXPARA,@pos)-(@pos+8))
end 
if(charindex('[YEAR_END=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[YEAR_END=',@EXPARA)
	SET @YEAR_END=SUBSTRING(@EXPARA,@POS+10,CHARINDEX(']',@EXPARA,@pos)-(@pos+10))
end 
if(charindex('[BOOKS_OF_ACOUNT=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[BOOKS_OF_ACOUNT=',@EXPARA)
	SET @BOOKS_OF_ACOUNT=SUBSTRING(@EXPARA,@POS+17,CHARINDEX(']',@EXPARA,@pos)-(@pos+17))
end

if(charindex('[BOOKS_OF_ACOUNT_YR=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[BOOKS_OF_ACOUNT_YR=',@EXPARA)
	SET @BOOKS_OF_ACOUNT_YR=SUBSTRING(@EXPARA,@POS+20,CHARINDEX(']',@EXPARA,@pos)-(@pos+20))
end
if(charindex('[Dealer_ISREQ=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Dealer_ISREQ=',@EXPARA)
	SET @Dealer_ISREQ=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
end

if(charindex('[Dealer_return=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Dealer_return=',@EXPARA)
	SET @Dealer_return=SUBSTRING(@EXPARA,@POS+15,CHARINDEX(']',@EXPARA,@pos)-(@pos+15))
end

if(charindex('[Dealer_stock_register=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Dealer_stock_register=',@EXPARA)
	SET @Dealer_stock_register=SUBSTRING(@EXPARA,@POS+23,CHARINDEX(']',@EXPARA,@pos)-(@pos+23))
end
if(charindex('[MVAT_RET=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[MVAT_RET=',@EXPARA)
	SET @MVAT_RET=SUBSTRING(@EXPARA,@POS+10,CHARINDEX(']',@EXPARA,@pos)-(@pos+10))
end
if(charindex('[Form_405_RET=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Form_405_RET=',@EXPARA)
	SET @Form_405_RET=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
end
if(charindex('[CST_RET=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[CST_RET=',@EXPARA)
	SET @CST_RET=SUBSTRING(@EXPARA,@POS+9,CHARINDEX(']',@EXPARA,@pos)-(@pos+9))
end
if(charindex('[Schedule-I=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-I=',@EXPARA)
	SET @Schedule_I=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Schedule_I=CASE WHEN @Schedule_I='1' THEN 'R' END
end

if(charindex('[Schedule-II=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-II=',@EXPARA)
	SET @Schedule_II=SUBSTRING(@EXPARA,@POS+13,CHARINDEX(']',@EXPARA,@pos)-(@pos+13))
	SET @Schedule_II=CASE WHEN @Schedule_II='1' THEN 'R' END
end

if(charindex('[Schedule-III=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-III=',@EXPARA)
	SET @Schedule_III=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
	SET @Schedule_III=CASE WHEN @Schedule_III='1' THEN 'R' END
end

if(charindex('[Schedule-IV=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-IV=',@EXPARA)
	SET @Schedule_IV=SUBSTRING(@EXPARA,@POS+13,CHARINDEX(']',@EXPARA,@pos)-(@pos+13))
	SET @Schedule_IV=CASE WHEN @Schedule_IV='1' THEN 'R' END
end

if(charindex('[Schedule-V=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-V=',@EXPARA)
	SET @Schedule_V=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Schedule_V=CASE WHEN @Schedule_V='1' THEN 'R' END
end

if(charindex('[Schedule-VI=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-VI=',@EXPARA)
	SET @Schedule_VI=SUBSTRING(@EXPARA,@POS+13,CHARINDEX(']',@EXPARA,@pos)-(@pos+13))
	SET @Schedule_VI=CASE WHEN @Schedule_VI='1' THEN 'R' END
end

if(charindex('[Schedule-VI=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Schedule-VI=',@EXPARA)
	SET @Schedule_VI=SUBSTRING(@EXPARA,@POS+13,CHARINDEX(']',@EXPARA,@pos)-(@pos+13))
	SET @Schedule_VI=CASE WHEN @Schedule_VI='1' THEN 'R' END
end

if(charindex('[Annexure-A=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-A=',@EXPARA)
	SET @Annexure_A=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_A=CASE WHEN @Annexure_A='1' THEN 'R' END
end

if(charindex('[Annexure-B=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-B=',@EXPARA)
	SET @Annexure_B=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_B=CASE WHEN @Annexure_B='1' THEN 'R' END
end

if(charindex('[Annexure-C=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-C=',@EXPARA)
	SET @Annexure_C=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_C=CASE WHEN @Annexure_C='1' THEN 'R' END
end

if(charindex('[Annexure-D=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-D=',@EXPARA)
	SET @Annexure_D=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_D=CASE WHEN @Annexure_D='1' THEN 'R' END
end
if(charindex('[Annexure-E=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-E=',@EXPARA)
	SET @Annexure_E=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_E=CASE WHEN @Annexure_E='1' THEN 'R' END
end
if(charindex('[Annexure-F=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-F=',@EXPARA)
	SET @Annexure_F=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_F=CASE WHEN @Annexure_F='1' THEN 'R' END
end
if(charindex('[Annexure-G=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-G=',@EXPARA)
	SET @Annexure_G=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_G=CASE WHEN @Annexure_G='1' THEN 'R' END
end
if(charindex('[Annexure-H=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-H=',@EXPARA)
	SET @Annexure_H=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_H=CASE WHEN @Annexure_H='1' THEN 'R' END
end
if(charindex('[Annexure-I=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-I=',@EXPARA)
	SET @Annexure_I=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_I=CASE WHEN @Annexure_I='1' THEN 'R' END
end

if(charindex('[Annexure-J_1=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-J_1=',@EXPARA)
	SET @Annexure_J_1=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
	SET @Annexure_J_1=CASE WHEN @Annexure_J_1='1' THEN 'R' END
end

if(charindex('[Annexure-J_2=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-J_2=',@EXPARA)
	SET @Annexure_J_2=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
	SET @Annexure_J_2=CASE WHEN @Annexure_J_2='1' THEN 'R' END
end
if(charindex('[Annexure-J_5=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-J_5=',@EXPARA)
	SET @Annexure_J_5=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
	SET @Annexure_J_5=CASE WHEN @Annexure_J_5='1' THEN 'R' END
end
if(charindex('[Annexure-J_6=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-J_6=',@EXPARA)
	SET @Annexure_J_6=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
	SET @Annexure_J_6=CASE WHEN @Annexure_J_6='1' THEN 'R' END
end
if(charindex('[Annexure-K=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Annexure-K=',@EXPARA)
	SET @Annexure_K=SUBSTRING(@EXPARA,@POS+12,CHARINDEX(']',@EXPARA,@pos)-(@pos+12))
	SET @Annexure_K=CASE WHEN @Annexure_K='1' THEN 'R' END
end

if(charindex('[Auditor_name=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Auditor_name=',@EXPARA)
	SET @AUDIT_NM=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
end 	

if(charindex('[Certified_By=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Certified_By=',@EXPARA)
	SET @CERTI_BY=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
end 

if(charindex('[MEMBER_NO=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[MEMBER_NO=',@EXPARA)
	SET @MEM_NO=SUBSTRING(@EXPARA,@POS+11,CHARINDEX(']',@EXPARA,@pos)-(@pos+11))
end 

if(charindex('[REG_NO=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[REG_NO=',@EXPARA)
	SET @REG_NO=SUBSTRING(@EXPARA,@POS+8,CHARINDEX(']',@EXPARA,@pos)-(@pos+8))
end 

if(charindex('[ADDRESS',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[ADDRESS',@EXPARA)
	SET @ADDRESS=SUBSTRING(@EXPARA,@POS+9,CHARINDEX(']',@EXPARA,@pos)-(@pos+9))
end 

if(charindex('[Auditor_Firm=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Auditor_Firm=',@EXPARA)
	SET @Auditor_Firm=SUBSTRING(@EXPARA,@POS+14,CHARINDEX(']',@EXPARA,@pos)-(@pos+14))
end 

if(charindex('[Transation_Id=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Transation_Id=',@EXPARA)
	SET @Transation_Id=SUBSTRING(@EXPARA,@POS+15,CHARINDEX(']',@EXPARA,@pos)-(@pos+15))
end 

if(charindex('[N_autho=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[N_autho=',@EXPARA)
	SET @N_autho=SUBSTRING(@EXPARA,@POS+9,CHARINDEX(']',@EXPARA,@pos)-(@pos+9))
end 

if(charindex('[Designation=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Designation=',@EXPARA)
	SET @Designation=SUBSTRING(@EXPARA,@POS+13,CHARINDEX(']',@EXPARA,@pos)-(@pos+13))
end 
if(charindex('[Emailid=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Emailid=',@EXPARA)
	SET @Emailid=SUBSTRING(@EXPARA,@POS+9,CHARINDEX(']',@EXPARA,@pos)-(@pos+9))
end 
if(charindex('[mobileno=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[mobileno=',@EXPARA)
	SET @mobileno=SUBSTRING(@EXPARA,@POS+10,CHARINDEX(']',@EXPARA,@pos)-(@pos+10))
end 

if(charindex('[Place=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Place=',@EXPARA)
	SET @Place=SUBSTRING(@EXPARA,@POS+7,CHARINDEX(']',@EXPARA,@pos)-(@pos+7))
end 

if(charindex('[Audit_DT=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Audit_DT=',@EXPARA)
	SET @Audit_DT=SUBSTRING(@EXPARA,@POS+10,CHARINDEX(']',@EXPARA,@pos)-(@pos+10))
end 
--
-- Form 233 temp table

SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=CAST('0' AS NUMERIC(20,2)),AMT2=CAST('0' AS NUMERIC(20,2)),AMT3=CAST('0' AS NUMERIC(20,2)),
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
,STM.RFORM_NM,CAST('0' AS NUMERIC(20,2)) AS AMT4,CAST('0' AS NUMERIC(20,2)) AS AMT5 --Added by Priyanka on 25022014
,TAX_NAME=CAST('' AS VARCHAR(12)),n.item,EIT_NAME=CAST('' AS VARCHAR(50)),CONTTAX=CAST(0 AS DECIMAL(12,2)),CTYPE=CAST('' AS VARCHAR(2)) 
INTO #FORM233
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
inner join stitem n on (m.tran_cd=n.tran_cd) 
WHERE 1=2

-- Form 3E temp table start
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
INTO #MHFORM3E
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
-- Form 3E temp table start
INSERT INTO #FORM233
EXECUTE USP_REP_MHFORM233
@TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
,@EWARE, @SINV_SR, @EINV_SR, @LYN, ''



INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('1','B',0,0,'')
--INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('1','C',0,0,'')
--INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('1','D',0,0,'') 
--INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('1','E',0,0,'') 
-- 3. Out of the aforesaid certificates; the following certificates are negative for the reasons given hereunder or be read with the following information :
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('3','',0,0,'') 

--4.COMPUTATION OF TAX LIABILITY AND RECOMMENDATIONS
-- Table 2
--4.i)Gross Turn-Over of Sales, including taxes as well as Turn-over of Non-Sales Transactions like Value of Branch Transfers/ Consignment Transfers and job work charges
SELECT @AMTA1=AMT1 FROM #FORM233 WHERE PARTSR='6' AND SRNO='A'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,AMT3,PARTY_NM) VALUES ('4','i',@AMTA1,0,0,'') 
--4.ii)Less:- Total allowable Deductions 
SELECT @AMTA1=AMT1 FROM #FORM233 WHERE PARTSR='6' AND SRNO='B'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,AMT3,PARTY_NM) VALUES ('4','ii',@AMTA1,0,0,'') 
--4.iii)Balance Net Turn-over liable for Tax
SELECT @AMTA1=AMT1 FROM #FORM233 WHERE PARTSR='6' AND SRNO='A'
SELECT @AMTA2=AMT1 FROM #FORM233 WHERE PARTSR='6' AND SRNO='B'
SET @AMTB1=@AMTA1-@AMTA2
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,AMT3,PARTY_NM) VALUES ('4','iii',@AMTB1,0,0,'') 
--4.iv)Tax leviable under the M.V.A.T. Act, 2002
SELECT @AMTA1=AMT2 FROM #FORM233 WHERE PARTSR='10A' AND SRNO='Z'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,AMT3,PARTY_NM) VALUES ('4','iv',@AMTA1,0,0,'') 

--4.v)Excess collection under M.V.A.T. Act, 2002
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','v',0,0,'') 
--4.vi)Less: Credits available on account of following:
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','vi',0,0,'') 
--4.a) Set-off claimed: 
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','a',0,0,'') 

--4.b)Amount of tax paid under MVAT Act as per ANNEXURE-A (including interest and RAO)

select @AMTA1=sum(isnull(a.gro_amt,0)) FROM JVMAIN A
INNER JOIN AC_MAST AC ON (A.AC_ID=AC.AC_ID)
WHERE AC.ST_TYPE='LOCAL' AND (A.RAOSNO<>'' OR A.RAODT<>'') AND (A.DATE BETWEEN @SDATE AND @EDATE)
select @AMTA2=sum(isnull(a.gro_amt,0)) FROM BPMAIN A
INNER JOIN AC_MAST AC ON (A.AC_ID=AC.AC_ID)
WHERE AC.ST_TYPE='LOCAL' AND A.U_NATURE='INTEREST' AND (A.DATE BETWEEN @SDATE AND @EDATE)

INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','b',@AMTA1+@AMTA2,0,'')

--4.c)Credit of tax as per tax deduction at source certificates (As per ANNEXURE-C).
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','c',0,0,'') 
--4.d)Any other (please specify)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','d',0,0,'') 

--4.vii)Total credits [(a) to (d) above)] available
SELECT @AMTA1=SUM(AMT1) FROM #FORM704 WHERE PARTSR='4' AND SRNO IN ('a','b','c','d')
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','vii',@AMTA1,0,'') 
--4.viii)Add/Less:- Any other(please specify)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','viii',0,0,'') 
--4.ix)Total amount payable/refundable
SELECT @AMTA1=sum(AMT1) FROM #FORM704 WHERE PARTSR='4' AND SRNO in ('iv','v')
SELECT @AMTA2=sum(AMT1) FROM #FORM704 WHERE PARTSR='4' AND SRNO in ('vii','viii')

INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','ix',@AMTA1-@AMTA2,0,'') 
--4.x)Less: Total Amount of Tax Deferred 
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','x',0,0,'') 
--4.xi)Less : Refund adjusted for payment of tax under the Central Sales Tax Act, 1956 
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','xi',0,0,'') 
--4.xii)Less: Excess Credit carried forword to subsequent tax period
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','xii',0,0,'') 
--4.xiii)Less : Refund already granted to dealer
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','xiii',0,0,'') 
--4.e)Balance Tax Payable/ Refundable
select @AMTA1=AMT1 FROM #FORM704 WHERE PARTSR='4' AND SRNO='ix'
select @AMTA2=AMT1 FROM #FORM704 WHERE PARTSR='4' AND SRNO='x'
select @AMTB1=AMT1 FROM #FORM704 WHERE PARTSR='4' AND SRNO='xi'
select @AMTB2=AMT1 FROM #FORM704 WHERE PARTSR='4' AND SRNO='xii'

INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','e',@AMTA1-@AMTA2-@AMTB1-@AMTB2,0,'') 
--4.ei)Add : Interest u/s 30(2)
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(2)'
 AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','ei',@AMTA1,0,'') 
--4.eii)Add : Interest u/s 30 (4)
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(4)'
  AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','eii',@AMTA1,0,'') 
--4.xiv)Total Amount Payable/ Refundable.
SELECT @AMTA1=SUM(AMT1) FROM #FORM704 WHERE PARTSR='4' AND SRNO IN ('e','ei','eii')
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','xiv',@AMTA1,0,'') 
--4.xv)Differential tax liability for non-production of declaration/ certificate as per Annexure-H.
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('4','xv',0,0,'') 

--Table 3
-- Form 3E temp table start
INSERT INTO #MHFORM3E
EXECUTE USP_REP_MHFORM3E
@TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
,@EWARE, @SINV_SR, @EINV_SR, @LYN, ''

--4.i)Gross Turn-Over of Sales (as per Sch. VI)
SELECT @AMTA1=AMT1 FROM #MHFORM3E WHERE PARTSR='1' AND SRNO='A'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','i',@AMTA1,0,'') 
--4.ii)Less:- Total Deductions available
SELECT @AMTA2=SUM(AMT1) FROM #MHFORM3E WHERE PARTSR='1' AND SRNO IN ('B','C','D','E','F','G','H','I','L','M','O')
SET @AMTA2=ISNULL(@AMTA2,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','ii',@AMTA2,0,'') 
--4.iii)Balance Net Turn-over liable for Tax
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','iii',@AMTA1-@AMTA2,0,'') 
--4.iv)CST leviable under the Central Sales Tax Act, 1956 subject to production of declarations listed in Annexure-I.
SELECT @AMTA1=AMT2 FROM #MHFORM3E WHERE PARTSR='2' AND SRNO='Z'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','iv',@AMTA1,0,'') 
--4.va)Less:  Amount of Tax Deferred 
SELECT @AMTA1=AMT1 FROM #MHFORM3E WHERE PARTSR='5' AND SRNO='C'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','va',@AMTA1,0,'') 
--4.vb)Amount of tax paid under the CST Act ANNEXURE-B (including interest and RAO)
select @AMTA1=sum(isnull(a.gro_amt,0)) FROM JVMAIN A
INNER JOIN AC_MAST AC ON (A.AC_ID=AC.AC_ID)
WHERE AC.ST_TYPE='OUT OF STATE' AND (A.RAOSNO<>'' OR A.RAODT<>'') AND (A.DATE BETWEEN @SDATE AND @EDATE)
select @AMTA2=sum(isnull(a.gro_amt,0)) FROM BPMAIN A
INNER JOIN AC_MAST AC ON (A.AC_ID=AC.AC_ID)
WHERE AC.ST_TYPE='OUT OF STATE' AND A.U_NATURE='INTEREST' AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','vb',ISNULL(@AMTA1,0)+ISNULL(@AMTA2,0),0,'') 
--4.vc) MVAT refund adjusted (if any)
SELECT @AMTA1=AMT1 FROM #MHFORM3E WHERE PARTSR='5' AND SRNO='K'
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','vc',@AMTA1,0,'') 
--4.vi)Add/Less : Any other (Please specify)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','vi',0,0,'') 
--4.vii)Balance of tax payable/ Refundable)
select @AMTA1=AMT1 FROM #FORM704 WHERE PARTSR='5' AND SRNO='iv'
select @AMTA2=AMT1 FROM #FORM704 WHERE PARTSR='5' AND SRNO='va'
select @AMTB1=AMT1 FROM #FORM704 WHERE PARTSR='5' AND SRNO='vb'
select @AMTB2=AMT1 FROM #FORM704 WHERE PARTSR='5' AND SRNO='vc'
select @AMTC1=AMT1 FROM #FORM704 WHERE PARTSR='5' AND SRNO='vi'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','vii',ISNULL(@AMTA1,0)-ISNULL(@AMTA2,0)-ISNULL(@AMTB1,0)-ISNULL(@AMTB2,0)-ISNULL(@AMTC1,0),0,'') 
--4.viiia)Add:  Interest U/s 9(2) read with Section 30(2) of MVAT Act.
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(2)'
  AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','viiia',@AMTA1,0,'') 
--4.viiib)Add: Interest U/s 9(2) read with Section 30 (4) of MVAT Act.
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(4)'
 AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','viiib',@AMTA1,0,'') 
--4.ix)Total Dues Payable /Refundable
select @AMTA1=sum(AMT1) FROM #FORM704 WHERE PARTSR='5' AND SRNO IN ('vii','viiia','viiib')
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','ix',@AMTA1,0,'') 
--4.x)Excess Central Sales Tax Collection
--select @AMTA1=AMT1 FROM #FORM704 WHERE PARTSR='5' AND SRNO='A'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','x',0,0,'') 
--4.xi)Differential CST liability for want of declaration as worked out in Annexure-I.
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('5','xi',0,0,'') 

--Table 4 CUMULATIVE QUANTUM OF BENEFITS AVAILED
--i)Under the Maharashtra Value Added Tax Act, 2002.
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('6','i',0,0,'') 
--ii)Under the Central Sales Tax Act, 1956
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('6','ii',0,0,'') 
--total
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('6','TT',0,0,'') 

--Table 5  Classification of  additional Dues  with calculation of Tax and interest thereon   
--1)Difference in Taxable Turn-over
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','A',0,0,'') 
--2)Disallowance of Branch/Consignment Transfers 
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','B',0,0,'') 
--3)Disallowance of Inter-state sales or sales under section 6 (2) of CST Act.
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','C',0,0,'') 
--4)Disallowance of High-seas Sales
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','D',0,0,'') 
--5)Additional Tax liability on account of Non-production of Declarations and Certificates. 
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','E',0,0,'') 
--6)Computation of Tax at Wrong rate
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','F',0,0,'') 
--7)Excess claim of Set-off or Refund.
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','G',0,0,'') 
--8)Disallowance of other Non-admissible claims. (Please Specify)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','H',0,0,'') 
--(a)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','HA',0,0,'') 
--(b)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','HB',0,0,'') 
--9)TOTAL DUES PAYABLE
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','I',0,0,'') 
--10)Amount of interest payable (To be calculated from due date to the date of Audit)
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','J',0,0,'') 
--11)TOTAL AMOUNT PAYABLE
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('7','K',0,0,'') 

--5.  Qualifications or remarks having impact on the tax liability   :-
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('8','',0,0,'') 

--6.  Dealer has been recommended to:-
--i)Pay additional tax liability of Rs.
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay additional tax liability'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay additional tax liability'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','i',@AMTA1,@AMTA2,'') 
--ii)Pay back excess refund received of Rs.
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay back excess refund received'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay back excess refund received'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','ii',@AMTA1,@AMTA2,'') 
--iii)Claim additional refund of Rs.
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Claim additional refund'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Claim additional refund'		
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','iii',@AMTA1,@AMTA2,'') 

--iv)Reduce the claim of refund  of Rs.		
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Reduce the claim of refund'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Reduce the claim of refund'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','iv',@AMTA1,@AMTA2,'') 

--v)Reduce tax liability of Rs.		
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Reduce tax liability'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Reduce tax liability'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','v',@AMTA1,@AMTA2,'') 

--vi)Revise closing balance of CQB of Rs.		
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Revise closing balance of CQB'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Revise closing balance of CQB'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','vi',@AMTA1,@AMTA2,'') 

--vii)Pay interest under-section 30(2) of Rs.		
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(2)'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(2)'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','vii',@AMTA1,@AMTA2,'') 

--viii)Pay interest under-section 30(4) of Rs. 		
SELECT @AMTA1=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='VAT PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(4)'
SELECT @AMTA2=ISNULL(SUM(A.NET_AMT),0) FROM JVMAIN A WHERE A.PARTY_NM='CST PAYABLE' AND A.ENTRY_TY='J4' AND A.VAT_ADJ='Pay interest under-section 30(4)'
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('9','viii',@AMTA1,@AMTA2,'') 

--7. A similar issue is involved in the case of dealer under audit, where a decision against the State Government or the Commissioner was given by the Tribunal and the reference/appeal is pending before appropriate forum in the case of following dealer(s) which is/are appearing in the list of pending reference(s)/appeal(s) kept on website of the department.
INSERT INTO #FORM704(PARTSR,SRNO,AMT1,AMT2,PARTY_NM) VALUES ('10','',0,0,'') 


SELECT TAXAUDIT_NM=@TAXAUDIT_NM,TAXAUDIT_DT=@TAXAUDIT_DT,Profit_loss=@Profit_loss,Balance_sheet=@Balance_sheet,ACT_NAME=@ACT_NAME,ACT_DT=@ACT_DT,
YEAR_END=@YEAR_END,BOOKS_OF_ACOUNT=@BOOKS_OF_ACOUNT,BOOKS_OF_ACOUNT_YR=@BOOKS_OF_ACOUNT_YR,Dealer_ISREQ=@Dealer_ISREQ,Dealer_return=@Dealer_return,
Dealer_stock_register=@Dealer_stock_register,MVAT_RET=@MVAT_RET,Form_405_RET=@Form_405_RET,CST_RET=@CST_RET,Schedule_I=isnull(@Schedule_I,'c'),Schedule_II=isnull(@Schedule_II,'c'),
Schedule_III=ISNULL(@Schedule_III,'c'),Schedule_IV=ISNULL(@Schedule_IV,'c'),Schedule_V=ISNULL(@Schedule_V,'c'),Schedule_VI=ISNULL(@Schedule_VI,'c'),Annexure_A=ISNULL(@Annexure_A,'c'),Annexure_B=ISNULL(@Annexure_B,'c'),Annexure_C=ISNULL(@Annexure_C,'c'),
Annexure_D=ISNULL(@Annexure_D,'c'),Annexure_E=ISNULL(@Annexure_E,'c'),Annexure_F=ISNULL(@Annexure_F,'c'),Annexure_G=ISNULL(@Annexure_G,'c'),Annexure_H=ISNULL(@Annexure_H,'c'),Annexure_I=ISNULL(@Annexure_I,'c'),Annexure_J_1=ISNULL(@Annexure_J_1,'c'),
Annexure_J_2=ISNULL(@Annexure_J_2,'c'),Annexure_J_5=ISNULL(@Annexure_J_5,'c'),Annexure_J_6=ISNULL(@Annexure_J_6,'c'),Annexure_K=ISNULL(@Annexure_K,'c'),
AUDIT_NM=@AUDIT_NM,certi_by=@CERTI_BY,Auditor_Firm=@Auditor_Firm,Transation_Id=@Transation_Id,N_autho=@N_autho,MEM_NO=@MEM_NO,REG_NO=@REG_NO,ADDRESS=@ADDRESS,
Designation=@Designation,Emailid=@Emailid,mobileno=@mobileno,Place=@Place,Audit_DT=@Audit_DT,* FROM #FORM704 order by cast(partsr as int)
END