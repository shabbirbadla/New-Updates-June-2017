If exists(Select * from sysobjects where [name]='USP_REP_MHFORM231_A' and xtype='P')
Begin
	Drop Procedure USP_REP_MHFORM231_A
End
go
 -- =============================================
 -- Author:	Suraj Kumawat
 -- Create date: 17/03/2016
-- EXECUTE USP_REP_MHFORM231_a'','','','04/01/2015','03/31/2016','','','','',0,0,'','','','','','','','','2015-2016',''
-- Description: This Stored procedure is useful to generate MH VAT FORM 231
-- Modify date: 
-- Modify date: 
-- =============================================
 create PROCEDURE [dbo].[USP_REP_MHFORM231_A]
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
 Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2),@balamt1 DECIMAL(14,2),@balamt2 DECIMAL(14,2)
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
 DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2),@AMTA3 NUMERIC(12,2),@AMTA4 NUMERIC(12,2)
 DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
 DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)
 
SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)



---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
,AC1.CITY,CAST('' AS VARCHAR(100)) AS RAOSNO,CAST('' AS SMALLDATETIME) AS RAODT
INTO #FORM221
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

--->PART 1-5 
	---- Previous Period /Earlier Period---------
	DECLARE @STARTDT SMALLDATETIME,@ENDDT SMALLDATETIME,@TMONTH INT,@TYEAR INT
	SET @TMONTH=DATEDIFF(M,@SDATE,@EDATE)
	SET @TYEAR=DATEDIFF(YY,@SDATE,@EDATE)
	SET @STARTDT=DATEADD(Y,-@TYEAR,@STARTDT)
	PRINT @STARTDT
	SET @STARTDT=DATEADD(M,-(@TMONTH+1),@SDATE)
	PRINT @STARTDT
	SET @ENDDT=DATEADD(D,-1,@SDATE)

	PRINT @TMONTH
	PRINT @TYEAR
	PRINT @ENDDT
   --------------------------------------------
---------- this code will fetch data from Sales Annexure ------------------
----------Temporary table for Sales Annexure data -------------------------
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,AMT4=M.TAXAMT,AMT5=M.TAXAMT,AMT6=M.TAXAMT,AMT7=M.TAXAMT,
AMT8=M.TAXAMT,AMT9=M.TAXAMT,M.INV_NO,M.DATE,Tran_cd=SPACE(5),Tran_Desc=SPACE(200),RAction = SPACE(50),Ret_Frm_no = SPACE(25),
AC1.S_TAX INTO #MHSALSANNEX_Temp FROM PTACDET A INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID) WHERE 1=2
Insert into #MHSALSANNEX_Temp EXECUTE USP_REP_MH_SALESANNEX '','','',@sdate,@edate,'','','','',0,0,'','','','','','','','','2015-2016','' 
-----------------------------------------------------------------------------
----------------Start Temporary Table  for Purchase Annexure --------
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,AMT4=M.TAXAMT,AMT5=M.TAXAMT,AMT6=M.TAXAMT,AMT7=M.TAXAMT,
AMT8=M.TAXAMT,AMT9=M.TAXAMT,M.INV_NO,M.DATE,Tran_cd=SPACE(5),Tran_Desc=SPACE(200),RAction = SPACE(50),Ret_Frm_no = SPACE(25),
AC1.S_TAX INTO #MHPURANNEX_TEMP FROM PTACDET A INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID) WHERE 1=2
INSERT INTO #MHPURANNEX_TEMP EXECUTE USP_REP_MH_PURANNEX'','','',@SDATE,@EDATE,'','','','',0,0,'','','','','','','','','2015-2016',''
----------------End Temporary Table  --------

-----------------------------------------------------------
select * into #vattupcd_tbl  from 
(select VATTYPECD,entry_ty,tran_cd  from stmain
union all
select VATTYPECD,entry_ty,tran_cd  from srmain
union all
select VATTYPECD,entry_ty,tran_cd  from cnmain
union all
select VATTYPECD,entry_ty,tran_cd  from dnmain
union all
select VATTYPECD,entry_ty,tran_cd  from ptmain
union all
select VATTYPECD,entry_ty,tran_cd  from prmain )a

-----------------------------------------------------------

-- 5 Computation of net turnover of sales liable to tax 
--- a)Gross turnover of sales including,taxes as well as turnover of non sales transactions like value of branch/ consignment transfers ,job work charges etc
   --=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,{"231","234","CST"})),0)
SET @AMTA1=0
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231','234','CST') 
--=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","680","700","780"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','680','700','780')
--=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","680","700","780"})),0)
SET @AMTA3 = 0 
SELECT @AMTA3 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('234')
AND Tran_cd IN('600','680','700','780')

--=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"CST",'Sales Annexure'!P:P,{"600","610","620","630","640","650","660","670","680","700","710","720","730","740","750","760","770","780"})),0)
SET @AMTA4 = 0 
SELECT @AMTA4 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('CST')
AND Tran_cd IN('600','610','620','630','640','650','660','670','680','700','710','720','730','740','750','760','770','780')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
VALUES (1,'5','A',0,ROUND(@AMTA1-(@AMTA2+@AMTA3+@AMTA4),0),0,0,'','','') 

--(b)Less:- Turnover of Sales (including taxes thereon) including inter-state Consignment Transfers and Branch Transfers Covered under Form Number 234 

---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"234")),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('234')
---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"234",'Sales Annexure'!P:P,{"600","680","700","780"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('234')
AND Tran_cd IN('600','680','700','780')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT) 
				VALUES (1,'5','B',0,round(@AMTA1-@AMTA2,0),0,0,'','','') 


--c) Balance :- Turnover Considered under this Form (a-b)
SET @AMTA1=0
SET @AMTA2 = 0
SELECT @AMTA1=ISNULL(SUM(case when srno ='A' then +AMT1 else -amt1 end),0) 
,@AMTA2 = ISNULL(SUM(case when srno ='A' then +AMT2 else -AMT2 end),0)  FROM #FORM221 WHERE PART = 1 AND PARTSR ='5' AND SRNO IN('A','B')
	INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
	VALUES (1,'5','C',0,round(@AMTA1,0),0,0,'','','')

--5(D)Add:- Value of Goods return (inclusive of tax) including reduction of sale price on account of rate difference and discount claimed in earlier period but not confirmed by buyer.
	---Pending
SET @AMTA1=0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
					 VALUES (1,'5','D',0,0,0,0,'','','')
  

--5(E)Less:- Value ( inclusive of sales tax) of Goods Return for Return period

---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","680"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','680')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','E',0,round(@AMTA1,0),0,0,'','','')
 
--5(F)Less:- Credit Note , price on account of rate difference and discount Within State for Return period.
 --=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"700","780"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('700','780')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','F',0,round(@AMTA1,0),0,0,'','','')
--5(G)Less: Value of Goods Return (inclusive of tax) including reduction of sale price on account of rate difference and discount confirmed for earlier period 

---Pending 
set @AMTA1 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
				VALUES(1,'5','G',0,round(@AMTA1,0),0,0,'','','')
 

--(h)Less:-Net Tax amount ( Tax included in sales shown in (c) above less Tax included in ( d+e+f ) above)
---=ROUND(SUM(SUMIFS('Sales Annexure'!F:F,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"100","200"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT2),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('100','200')

--=ROUND(SUM(SUMIFS('Sales Annexure'!F:F,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","700"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT2),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','700')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT) 
					VALUES  (1,'5','H',0,ROUND(@AMTA1-@AMTA2,0),0,0,'','','') 

--5(I) Less:- Total Value in which tax is not collected separately ( Inclusive of Tax with whole Amount )
---=ROUND(SUM(SUMIFS('Sales Annexure'!G:G,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"100","200"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT3),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('100','200')
---=ROUND(SUM(SUMIFS('Sales Annexure'!G:G,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","700"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT3),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','700')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT) 
					VALUES(1,'5','I',0,round(@AMTA1 - @AMTA2 ,0),0,0,'','','')

 
--5(J) Less: value of branch transfer/consignment transfer within the State if Tax is paid by an agent

--=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,"300")),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('300')
---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"680","780"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('680','780')
 INSERT INTO #FORM221  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT) 
						VALUES (1,'5','J',0,round(@AMTA1-@AMTA2,0),0,0,'','','') 

 --5(K)--Less: sales u/s 8(1) i.e. Inter state sales including Central Sales Tax, sales in the course of imports, exports and value of branch transfers/consignment transfers outside the State
 ---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"CST")),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('CST')
--=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"CST"
--,'Sales Annexure'!P:P,{"600","610","620","630","640","650","660","670","680","700","710","720","730","740","750","760","770","780"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('CST')
AND Tran_cd IN('600','610','620','630','640','650','660','670','680','700','710','720','730','740','750','760','770','780')
INSERT INTO #FORM221  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','K',0,round(@AMTA1-@AMTA2,0),0,0,'','','')
 
--5(L)--Less:-Sales of tax-free goods specified in Schedule" A" of MVAT Act
---=ROUND(SUM(SUMIFS('Sales Annexure'!I:I,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"100","200"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT5),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('100','200')
----=ROUND(SUM(SUMIFS('Sales Annexure'!I:I,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","700"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT5),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','700')
INSERT INTO #FORM221  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','L',0,round(@AMTA1-@AMTA2,0),0,0,'','','')
 
 --5(M)--Less:-Sales of taxable goods fully exempted u/s 41 and u/s. 8 other than sales under section 8(1) & covered in Box 5(k)  
 --=ROUND(SUM(SUMIFS('Sales Annexure'!J:J,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"100","200"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT6),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('100','200')
---=ROUND(SUM(SUMIFS('Sales Annexure'!J:J,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","700"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT6),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','700')

 INSERT INTO #FORM221  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','M',0,ROUND(@AMTA1-@AMTA2,0),0,0,'','','')
 
 --5(N)--Less:-Labour Charges/Job work charges
 --=ROUND(SUM(SUMIFS('Sales Annexure'!K:K,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"100","200"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT7),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('100','200')
----=ROUND(SUM(SUMIFS('Sales Annexure'!K:K,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","700"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT7),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','700')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','N',0,ROUND(@AMTA1-@AMTA2,0),0,0,'','','')
 
 --5(O)--Less:-Other allowable deductions, as per Sale Annexure
--=ROUND(SUM(SUMIFS('Sales Annexure'!L:L,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"100","200"})),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT8),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('100','200')
---=ROUND(SUM(SUMIFS('Sales Annexure'!L:L,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"600","700"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT8),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('600','700')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','O',0,ROUND(@AMTA1-@AMTA2,0),0,0,'','','')
 
 --5(P)--Less:- Deduction under Section 3(2)
 ---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,"800")),0)
SET @AMTA1=0
SELECT @AMTA1 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('800')
 ---=ROUND(SUM(SUMIFS('Sales Annexure'!M:M,'Sales Annexure'!O:O,"231",'Sales Annexure'!P:P,{"680","780"})),0)
SET @AMTA2=0
SELECT @AMTA2 = ISNULL(SUM(AMT9),0) FROM #MHSALSANNEX_Temp WHERE Ret_Frm_no IN('231')
AND Tran_cd IN('680','780')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','P',0,(case when ROUND(@AMTA1-@AMTA2,0) > 0  then ROUND(@AMTA1-@AMTA2,0) else 0 end ),0,0,'','','')
set @AMTA1 = 0
set @AMTA2 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PART = 1 AND PARTSR ='5' AND SRNO IN('C','D')
SELECT @AMTA2 = ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PART = 1 AND PARTSR ='5' AND SRNO IN('E','F','G','H','I','J','K','L','M','N','O','P')
 --5(Q)--"Balance: Net turnover of Sales liable to tax [(c+d)- (e+f+g+h+i+j+k+l+m+n+o+p)]"
 print '-----------'
 print @AMTA1 
 print @AMTA2 
 print '-----------'
 INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,RAOSNO,RAODT)
						VALUES (1,'5','Q',0,round(@AMTA1-@AMTA2,0),0,0,'','','')
 
 --6 Computation of Sales tax payable under the MVAT Act
 
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,Party_nm) 
SELECT 1,'6','',A.PER,ROUND(isnull(SUM(CASE WHEN A.BHENT='ST' THEN A.VATONAMT ELSE -A.VATONAMT END),0),0)
,ROUND(isnull(SUM(CASE WHEN A.BHENT='ST' THEN A.TAXAMT ELSE -A.TAXAMT END),0),0),'' FROM VATTBL A 
inner join #vattupcd_tbl b on (a.TRAN_CD=b.Tran_cd and a.BHENT=b.entry_ty) 
WHERE A.BHENT IN ('ST','SR','CN') AND A.st_type in('','LOCAL') AND A.TAX_NAME LIKE '%VAT%' and b.VattypeCd <> ''  and a.per <> 0 GROUP BY A.PER
SELECT @AMTA1=SUM(AMT1) FROM #FORM221 WHERE PARTSR='6'
SELECT @AMTA2=SUM(AMT2) FROM #FORM221 WHERE PARTSR='6'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
if not exists(select top 1 srno from #FORM221 where PART = 1 and PARTSR ='6')
begin
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,Party_nm,INV_NO) VALUES 
(1,'6','',0,0,0,'','')
end

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,Party_nm,INV_NO) VALUES 
(1,'6','Z',0,round(@AMTA1,0),round(@AMTA2,0),'','Total')

--7(b)--Sales Tax collected in excess of the amount of tax payable
SET @AMTA1 = 0
SET @AMTA2 = 0
SET @AMTB1 = 0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PART = 1  AND PARTSR='5' AND SRNO ='H'
SELECT @AMTA2 =ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PART = 1  AND PARTSR='6' AND SRNO = 'Z' AND inv_no = 'Total'
SET @AMTB1 = (CASE WHEN ROUND(@AMTA1-@AMTA2,0) > 0  THEN ROUND(@AMTA1-@AMTA2,0) ELSE 0 END )
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
					VALUES(1,'7','A',0,ROUND(@AMTB1,0),0,0,'')

---details of the part 8 A
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
		VALUES (1,'8','A',0,0,0,0,'')
---Total		
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,INV_NO) 
		VALUES (1,'8','Z',0,0,0,0,'','Total')


-- 9 Computation of purchases eligible for setoff
-- a)Total turnover of Purchases including taxes, value of Branch Transfers/ Consignment Transfers received and Labour/ job work charges
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,{"231","234"})),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231','234')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P
---,{"31","36","41","46","51","56","61","66","71","76","90","91","32","37","42","47","52","57","62","67","72","77","95","96"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE  Ret_Frm_no IN('231') AND
Tran_cd  IN('31','36','41','46','51','56','61','66','71','76','90','91','32','37','42','47','52','57','62','67','72','77','95','96')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"234",'Purchase Annexure'!P:P
---,{"31","36","41","46","51","56","61","66","71","76","90","91","32","37","42","47","52","57","62","67","72","77","95","96"})),0)
SET @AMTA3 = 0
SELECT @AMTA3 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE  Ret_Frm_no IN('234') AND
Tran_cd  IN('31','36','41','46','51','56','61','66','71','76','90','91','32','37','42','47','52','57','62','67','72','77','95','96')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
				VALUES (1,'9','A',0,round(@AMTA1 -(@AMTA2 + @AMTA3),0),0,0,'')


--b)Less:- Turnover of Purchases Covered under Form Number 234
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"234")),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('234')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"234"
--,'Purchase Annexure'!P:P,{"31","36","41","46","51","56","61","66","71","76","90","91","32","37","42","47","52","57","62","67","72","95","77","96"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('234') AND
Tran_cd  IN('31','36','41','46','51','56','61','66','71','76','90','91','32','37','42','47','52','57','62','67','72','95','77','96')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
				VALUES (1,'9','B',0,ROUND(@AMTA1-@AMTA2,0),0,0,'')

-- 9 c)-- Balance:- Turnover of Purchases considered in this Form (a-b)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(CASE WHEN SRNO = 'A' THEN +AMT1 ELSE -AMT1 END),0)  FROM #FORM221 WHERE PART = 1 AND PARTSR ='9' AND SRNO IN('A','B')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','C',0,round(@AMTA1,0),0,0,'')

-- 9 d) Less:-Value of goods return (inclusive of tax) reduction of Purchase price.
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231"
--,'Purchase Annexure'!P:P,{"31","36","41","46","51","56","61","66","71","76","90","91"})),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND
Tran_cd  IN('31','36','41','46','51','56','61','66','71','76','90','91')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
				VALUES (1,'9','D',0,round(@AMTA1,0),0,0,'')
				
--9(e)Less:- Reduction of Purchase price on account of rate difference and discount .
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"32","37","42","47","52","57","62","67","72","77","95","96"})),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND
Tran_cd  IN('32','37','42','47','52','57','62','67','72','77','95','96')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES 
						(1,'9','E',0,ROUND(@AMTA1,0),0,0,'')

-- 9(f)Less:-Imports (Direct imports)
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"60")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND
Tran_cd  IN('60')
---
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"61","62"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND
Tran_cd  IN('61','62')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES 
						(1,'9','F',0,ROUND(@AMTA1 - @AMTA2,0),0,0,'')

-- 9(g)Less:-Imports (High seas purchases)
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"65")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND
Tran_cd  IN('65')
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"67","66"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND
Tran_cd  IN('67','66')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
				VALUES (1,'9','G',0,round(@AMTA1 - @AMTA2,0),0,0,'')

-- 9(H)Less:- Interstate purchases of taxable goods against certificate in Form'H'
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"50")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('50')
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"51","52"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('51','52')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) 
				VALUES (1,'9','H',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(I)Less:- within the State purchases of taxable goods against certificate in Form'H'
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"55")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('55')
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"56","57"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('56','57')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','I',0,round((@AMTA1 - @AMTA2 ),0),0,0,'')

-- 9(J)Less:-Inter-State purchases (Excluding purchases against any certificate and declaration in form C,H,F,I)
--=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"70")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('70')
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"71","72"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('71','72')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','J',0,round((@AMTA1-@AMTA2),0),0,0,'')

-- 9(K)Less:-Inter-State branch / consignment transfers received
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"30")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('30')
---- =ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"31","32"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('31','32')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','K',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(L)Less:- Interstate purchases of taxable goods against declaration in Form'C'
--=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"40")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('40')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"41","42"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('41','42')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','L',0,round(@AMTA1 - @AMTA2 ,0),0,0,'')

-- 9(M)Less:- within the State purchases of taxable goods against declaration in Form'C' 
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"45")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('45')
-----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"46","47"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('46','47')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','M',0,round((@AMTA1 - @AMTA2 ),0),0,0,'')

-- 9(N)Less:- Within the State Branch Transfers /Consignment Transfers received where tax is to be paid by an Agent
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"35")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('35')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"36","37"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('36','37')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','N',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(O)Less:-Within the State purchases of taxable goods from un-registered dealers
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"20")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('20')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"91","96"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('91','96')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','O',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(P)Less:- Interstate purchases of taxable goods against declaration in Form'I'
----=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"75")),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('75')
---=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"76","77"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('76','77')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','P',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(Q)Less:-Within the State purchases of taxable goods which are fully exempted from tax u/s 41 and u/s 8 but not covered under section 8(1)
---=ROUND(SUM(SUMIFS('Purchase Annexure'!J:J,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"10","15"})),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT6),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('10','15')
---- =ROUND(SUM(SUMIFS('Purchase Annexure'!J:J,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT6),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','Q',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(R)Less:-Within the State purchases of tax-free goods specified in Schedule "A"
----=ROUND(SUM(SUMIFS('Purchase Annexure'!I:I,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"10","15"})),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT5),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('10','15')
--- =ROUND(SUM(SUMIFS('Purchase Annexure'!I:I,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT5),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','R',0,round((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(S)Less:- Labour Job/ Labour charges paid
---=ROUND(SUM(SUMIFS('Purchase Annexure'!K:K,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"10","15"})),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT7),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('10','15')
---- =ROUND(SUM(SUMIFS('Purchase Annexure'!K:K,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT7),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','S',0,ROUND(@AMTA1 - @AMTA2 ,0),0,0,'')

-- 9(T)Less:-Other allowable deductions, if any
--- =ROUND(SUM(SUMIFS('Purchase Annexure'!L:L,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"10","15"})),0)
SET @AMTA1 = 0 
SELECT @AMTA1 =ISNULL(SUM(AMT8),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('10','15')
----=ROUND(SUM(SUMIFS('Purchase Annexure'!L:L,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0 
SELECT @AMTA2 =ISNULL(SUM(AMT8),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','T',0,ROUND((@AMTA1 - @AMTA2),0),0,0,'')

-- 9(U)Less:-Within the State purchases of taxable goods from registered dealers where tax is not collected seperately (Inclusive of tax)
--- =ROUND(SUM(SUMIFS('Purchase Annexure'!G:G,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"10","15"})),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT3),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('10','15')
-----=ROUND(SUM(SUMIFS('Purchase Annexure'!G:G,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT3),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','U',0,ROUND(@AMTA1 - @AMTA2,0),0,0,'')
-- 9(V)Less:- Within the State Purchases of Taxable goods purchase from Composition dealer u/s 42(1),(2)
----=ROUND(SUM(SUMIFS('Purchase Annexure'!H:H,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"10","15"})),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT4),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('10','15')
-----=ROUND(SUM(SUMIFS('Purchase Annexure'!H:H,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT4),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','V',0,ROUND(@AMTA1 - @AMTA2 ,0),0,0,'')
-- 9(W)Less:- Deduction under Section 3(2)
--=ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,"80")),0)
SET @AMTA1 = 0
SELECT @AMTA1 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('80')
---- =ROUND(SUM(SUMIFS('Purchase Annexure'!M:M,'Purchase Annexure'!O:O,"231",'Purchase Annexure'!P:P,{"90","95"})),0)
SET @AMTA2 = 0
SELECT @AMTA2 =ISNULL(SUM(AMT9),0) FROM #MHPURANNEX_TEMP WHERE Ret_Frm_no IN('231') AND Tran_cd  IN('90','95')

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','W',0,(case when round(@AMTA1 - @AMTA2,0) > 0 then round(@AMTA1 - @AMTA2,0) else 0 end ),0,0,'')

-- 9(X)Balance: Within the State purchases of taxable goods
		--from registered dealers eligible for set-off
		--[c-(d+e+f+g+h+i+j=k+l+m+n+o+p+q+r+s+t+u+v+w)]"
		
		--[c-(d+e+f+g+h+i+j=k+l+m+n+o+p+q+r+s+t+u+v+w)]
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0) FROM  #FORM221 WHERE PART = 1 AND PARTSR= '9' AND SRNO ='C'
SELECT @AMTA2 = ISNULL(SUM(AMT1),0) FROM  #FORM221 WHERE PART = 1 AND PARTSR= '9' AND SRNO IN('D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W')
 INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
				VALUES (1,'9','X',0,ROUND((@AMTA1-@AMTA2),0),0,0,'')

---10. Computation of purchase tax payable on the purchases effected during this period or previous periods.
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,PARTY_NM)
select 1,'10','A',A.PER,ROUND(sum(CASE WHEN A.BHENT='PT' THEN a.vatonamt ELSE -ISNULL(a.vatonamt,0) END),0)
,ROUND(SUM(CASE WHEN A.BHENT='PT' THEN a.taxamt ELSE -ISNULL(a.taxamt,0) END),0),'' 
 from vattbl a inner join #vattupcd_tbl b on (a.BHENT =b.entry_ty and a.TRAN_CD =b.Tran_cd )
WHERE A.BHENT IN ('PT','PR','DN') and A.TAX_NAME LIKE '%VAT%' and b.VattypeCd <> ''  and st_type in('','LOCAL') AND S_TAX = ''
group by A.PER
IF NOT EXISTS(SELECT TOP 1 SRNO FROM #FORM221 WHERE PART=1 AND PARTSR ='10' )
BEGIN
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'10','A',0,0,0,0,'')
END

SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT  @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA2 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PART=1 AND PARTSR ='10'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,inv_no) VALUES (1,'10','Z',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'','Total')

--11. Tax Rate-wise breakup of within State purchases from registered dealers eligible for set-off as per box 9(x)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,PARTY_NM)
select 1,'11','A',A.PER,ROUND(sum(CASE WHEN A.BHENT='PT' THEN a.vatonamt ELSE -ISNULL(a.vatonamt,0) END),0)
,ROUND(SUM(CASE WHEN A.BHENT='PT' THEN a.taxamt ELSE -ISNULL(a.taxamt,0) END),0),''  from vattbl a
inner join #vattupcd_tbl b on ( a.BHENT =b.entry_ty and a.TRAN_CD =b.Tran_cd )
WHERE A.BHENT IN ('PT','PR','DN') and A.TAX_NAME LIKE '%VAT%'   and st_type in('','LOCAL') AND S_TAX <> '' and b.VattypeCd <> ''
group by A.PER

IF NOT EXISTS(SELECT TOP 1 SRNO FROM #FORM221 WHERE PART=1 AND PARTSR ='11' )
BEGIN
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'11','A',0,0,0,0,'')
END

---Total of Section 11
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT  @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA2 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PART=1 AND PARTSR ='11'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,inv_no ) VALUES (1,'11','Z',0,round(@AMTA1,0),round(@AMTA2,0),0,'','Total')

--12. Computation on of set-off claimed in this return
-- A)Within the State purchases of taxable goods from registered/unregistered dealers eligible for set-off as per Box 10 and 11
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT  @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA2 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PART=1 AND PARTSR IN('10','11') AND SRNO ='Z'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','A',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'')

--B)Less :- Set off denial on account of purchases from RCC or Composition dealer
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','B',0,0,0,0,'')

--C)Less :- amount of set-off not admissible u/r 52A
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','C',0,round(@AMTA1,0),round(@AMTA2,0),0,'')

--C1)Less :- amount of set-off not admissible u/r 52B ---Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','C1',0,ROUND(@AMTA1,0),ROUND(@AMTA1,0),0,'')

----C2)Less :- amount of set-off not admissible u/r 52B  ---other than Capital Assets
--SET @AMTA1 = 0
--SET @AMTA2 = 0
--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','C2',0,ROUND(@AMTA1,0),ROUND(@AMTA1,0),0,'')


--d)Less :- Reduction in the amount of set-off u/r 53 of the corresponding purchase price of (Sch B, C, D & E) goods 
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','D',0,0,0,0,'')
--Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','D1',0,0,0,0,'')

--other than Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','D2',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'')

--E)Less:- Denial in the amount of Set off u/r 54 of the corresponding purchase price
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','E',0,0,0,0,'')
--Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','E1',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'')
--other than Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','E2',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'')

--F)Less: within the State  purchase of taxable goods from registered dealer under MVAT  Act 2002 and set off not claimed
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','F',0,0,0,0,'')
--Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','F1',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'')

--other than Capital Assets
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','F2',0,ROUND(@AMTA1,0),ROUND(@AMTA2,0),0,'')

--G)Less:- Within the state purchases of Capital Asset from registered dealer ITC withheld for staggered manner
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','G',0,0,0,0,'')

--H)"Set-off available for the period of this return [a-(b+c-d+e+f+g)]"
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA2 =ISNULL(SUM(AMT2),0) FROM  #FORM221 WHERE PART =1 AND PARTSR='12' AND SRNO ='A'
SET @AMTB1 = 0
SET @AMTB2 = 0
SELECT @AMTB1 = ISNULL(SUM(AMT1),0),@AMTB2=ISNULL(SUM(AMT2),0) FROM  #FORM221 WHERE PART =1 AND PARTSR='12'
 AND SRNO IN('B','C','C1','D1','D2','E1','E2','F1','F2','G')
 set @balamt1 = 0
 set @balamt2 = 0
 SET @balamt1 = @balamt1 + ROUND((@AMTA1 -@AMTB1),0)
 set @balamt2 = @balamt2 + ROUND((@AMTA2 -@AMTB2),0)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','H',0,ROUND((@AMTA1 -@AMTB1),0),ROUND((@AMTA2 -@AMTB2),0),0,'')

--I)Add:- Allowance of set-off reversed in earlier return/s 
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','I',0,0,0,0,'')
	-- Capital Assets
SET @AMTA1 = 0
SET @balamt1 = @balamt1 + ROUND(@AMTA1,0)
set @balamt2 = @balamt2 + 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','I1',0,ROUND(@AMTA1,0),0,0,'')
	-- ---other than Capital Assets
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','I2',0,0,0,0,'')

--J)Less:- Reduction u/r 52A, 52b, 53 and denial u/r 54 out of above i (h + (i-j)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','J',0,0,0,0,'')
	-- Capital Assets
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','J1',0,0,0,0,'')
	---other than Capital Assets
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','J2',0,0,0,0,'')

--K) Add: Allowance of Set-off not claimed on goods return
SET @AMTA1 = 0
SET @balamt1 = @balamt1 + ROUND(@AMTA1,0)
set @balamt2 = @balamt2 + 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','K',0,round(@AMTA1,0),0,0,'')
SET @AMTA1 = ROUND(@balamt1,0)
SET @AMTA2 = ROUND(@balamt2,0)
--L) Total Set-off Admisible for the period of this returns
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'12','L',0,@AMTA1,@AMTA2,0,'')

---13. Computation for Tax payable along with return
---A. Aggregate of credit available for the period covered under this return

--A) Set off admisible as per Box 12 (I)
SET @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT2),0) FROM  #FORM221  WHERE PART =1 AND PARTSR= '12' AND SRNO='L'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','A',0,ROUND(@AMTA1,0),0,0,'')

--B) Excess credit brought forward from previous return
SET @AMTA1 = 0
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4' 
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Excess credit brought forward from previous return' AND A.party_nm = 'VAT PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','B',0,ROUND(@AMTA1,0),0,0,'')
--C) Amount already paid (As per Box 13 E )
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','C',0,0,0,0,'')

--D) Excess Credit if any , as per Form 234 , to be adjusted against the liability as per Form 231
SET @AMTA1 = 0

select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Tax Paid in Excess' AND A.party_nm = 'VAT PAYABLE'
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Tax Paid in Excess' AND party_nm = 'VAT PAYABLE'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','D',0,ROUND(@AMTA1,0),0,0,'')

--E) Adjustment of ET paid under Maharashtra Tax on Entry of Goods into Local Areas Act 2002
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Tax on Entry of Goods into Local Areas Act 2002' AND party_nm = 'VAT PAYABLE'

select @AMTA1 = ISNULL(SUM(b.amount),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Tax on Entry of Goods into Local Areas Act 2002' AND A.party_nm = 'VAT PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','E',0,ROUND(@AMTA1,0),0,0,'')

--F) Adjustment of ET paid under Maharashtra Tax on Entry of Motor Vehicle Act into Local Areas Act 1987
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Tax on Entry of Motor Vehicle Act into Local Areas Act 1987' AND party_nm = 'VAT PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Tax on Entry of Motor Vehicle Act into Local Areas Act 1987' AND A.party_nm = 'VAT PAYABLE'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','F',0,ROUND(@AMTA1,0),0,0,'')

--G) Amount of Tax collected at source u/s 31A
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE)
--and VAT_ADJ ='Tax collected at source u/s 31A' AND party_nm = 'VAT PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Tax collected at source u/s 31A' AND A.party_nm = 'VAT PAYABLE'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','G',0,round(@AMTA1,0),0,0,'')

--H) Refund adjustment order No. (As per Box 13 F)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','H',0,0,0,0,'')

--I) Total available credit (a+b+c+d+e+f+g+h)
SET @AMTA1 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'13','I',0,ROUND(@AMTA1+@AMTA2,0),0,0,'')


---B Total Tax Payable and adjustment of CST/ET payable against available credit
--A) Sales Tax payable as per box 6 + Sales Tax payable as per box 8 + Purchase Tax payable as per box 10
SET @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT2),0) FROM  #FORM221  WHERE PART =1 AND PARTSR in('6','8','10') AND SRNO ='Z'
print 'total of the  6810'
print @AMTA1
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','A',0,ROUND(@AMTA1,0),0,0,'')

--B) Adjustment on account of MVAT payable, if any as per Return Form 234 against the excess credit as per Form 231.
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','B',0,0,0,0,'')

--C)Adjustment on account of CST payable as per return for this period
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Adjustment Towards CST' AND party_nm = 'CST PAYABLE'

select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Adjustment Towards CST' AND A.party_nm = 'CST PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','C',0,ROUND(@AMTA1,0),0,0,'')

--D)Adjustment on account of ET payable under Maharashtra tax on Entry of Goods into Local Areas Act, 2002
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Tax on Entry of Goods into Local Areas Act 2002' AND party_nm = 'CST PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Tax on Entry of Goods into Local Areas Act 2002' AND A.party_nm = 'CST PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','D',0,ROUND(@AMTA1,0),0,0,'')

-- E)Adjustment on account of ET payable under /Maharashtra Tax on Entry of Motor Vehicle Act into Local Areas Act, 1987
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Tax on Entry of Motor Vehicle Act into Local Areas Act 1987' AND party_nm = 'CST PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Tax on Entry of Motor Vehicle Act into Local Areas Act 1987' AND A.party_nm = 'CST PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','E',0,ROUND(@AMTA1,0),0,0,'')

-- F)Amount of Tax Collected in Excess of the amount of Sales Tax payable if any ( as per Box 7) 
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM  #FORM221 WHERE PART = 1 AND PARTSR ='7' AND SRNO = 'A'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','F',0,ROUND(@AMTA1,0),0,0,'')
-- G)Interest Payable
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Interest  Payable' AND party_nm = 'CST PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Interest  Payable' AND A.party_nm = 'CST PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','G',0,ROUND(@AMTA1,0),0,0,'')

-- H)Late Fee Payable
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Late Fees Payable' AND party_nm = 'CST PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Late Fees Payable' AND A.party_nm = 'CST PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','H',0,ROUND(@AMTA1,0),0,0,'')
	
-- I)Add: Reversal on account of set-off claimed Excess in earlier return
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','I',0,0,0,0,'')
---Capital Assets
SET @AMTA1 = 0
--select @AMTA1 = isnull(sum(net_amt),0) from JVMAIN where entry_ty ='J4'   and (date between @SDATE and @EDATE )
--and VAT_ADJ ='Reversal on account of set-off claimed Excess in earlier return - Capital Assets' AND party_nm = 'CST PAYABLE'
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Reversal on account of set-off claimed Excess in earlier return - Capital Assets' AND A.party_nm = 'CST PAYABLE'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','I1',0,ROUND(@AMTA1,0 ),0,0,'')
	---Other than Capital Assets
SET @AMTA1 = 0
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Reversal on account of set-off claimed Excess in earlier return - Other than Capital Assets' AND A.party_nm = 'CST PAYABLE'

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','I2',0,ROUND(@AMTA1,0),0,0,'')
-- J)Reduction u/r 52A, 52B, 53 and denial u/r 54 out of above (i)
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','J',0,0,0,0,'')
	---Capital Assets
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','J1',0,0,0,0,'')
	---Other than Capital Assets
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','J2',0,0,0,0,'')

--K)Balance: Excess credit =[13A(i)-(13B(a)+13B(b)+13B(c)+ 13B(d)+ 13B(e)+ 13 B(f)+ 13 B(g)+13 B(h)+13B(j))]
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','K',0,ROUND((@AMTA1 -@AMTA2),0),0,0,'')

--L) Balance Amount payable= [ 13B(a)+13B(b)+13B(c)+ 13B(d)+13B(e)+13 B(i)+ 13 B(g)+13 B(h)+13B(j) - 13A(l)]

SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','L',0,ROUND((@AMTA2 -@AMTA1),0),0,0,'')

---C Utilisation of Excess Credit as per Box 13B(k)
	--A) Excess credit carried forward to subsequent tax period
SET @AMTA1 = 0	
select @AMTA1 = ISNULL((SUM(b.amount)),0) from JVMAIN a left outer join JVACDET b on  a.tran_cd=b.tran_cd and b.amt_ty ='DR'  where a.entry_ty = 'J4'
AND (A.date between @SDATE and @EDATE )
and A.VAT_ADJ ='Excess credit carried forward to  subsequent tax period' AND A.party_nm = 'CST PAYABLE'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (3,'13','A',0,ROUND(@AMTA1,0),0,0,'')
--B) Excess credit claimed as refund in this return (13 B(k)- 13c(a))
SET @AMTA1 = 0
SET @AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (3,'13','B',0,ROUND((@AMTA1-@AMTA2),0),0,0,'')
	
	-----D Tax payable with return 	
--A) Total Amount payable as per Box 13B(l)
SET @AMTA1 = 0
---SELECT @AMTA1 = ISNULL(SUM(AMT1),0)  FROM #FORM221 WHERE PART = 2 AND PARTSR ='13' AND SRNO in('L')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (4,'13','A',0,ROUND(@AMTA1,0),0,0,'')


---E. Details of Amount paid along with return and /or Amount Already Paid
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,INV_NO,DATE,PARTY_NM,ADDRESS)
SELECT 5,'13','A',0,A.NET_AMT,B.U_CHALNO,A.DATE,B.BANK_NM,C.S_TAX FROM VATTBL A
INNER JOIN BPMAIN B ON (A.TRAN_CD=B.TRAN_CD)
INNER JOIN AC_MAST C ON (B.BANK_NM=C.AC_NAME) WHERE A.AC_NAME='VAT PAYABLE' and b.u_nature ='VAT' AND (A.DATE BETWEEN @SDATE AND @EDATE)

IF NOT EXISTS(SELECT TOP 1 SRNO FROM #FORM221 WHERE PART = 5 AND PARTSR = '13' AND SRNO ='A')
BEGIN
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (5,'13','A',0,0,0,0,'')
end 
--- Total of challan Details
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PART = 5 AND PARTSR = '13' AND SRNO ='A'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,inv_no) VALUES (5,'13','Z',0,ROUND(@AMTA1,0),0,0,'','Total')

---Details for Set off admisible as per Box 12 (I)
UPDATE  #FORM221 SET AMT1 = ROUND(@AMTA1,0) WHERE PART = 1 AND PARTSR ='13' AND SRNO = 'C'


---F. Details of RAO
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,INV_NO,DATE,Party_nm)select 6,'13','A'
,0,ROUND(b.amount,0),A.RAOSNO,A.RAODT,'' FROM JVMAIN A  left outer join jvacdet b on (a.tran_cd=b.tran_cd and b.amt_ty ='dr') WHERE a.ENTRY_TY='J4' AND (A.RAOSNO<>'' OR A.RAODT<>'') 
and A.VAT_ADJ='Refund Adjustment order' and A.PARTY_NM='VAT PAYABLE' AND (A.DATE BETWEEN @SDATE AND @EDATE)

IF NOT EXISTS(SELECT TOP 1 SRNO FROM #FORM221 WHERE PART = 6 AND PARTSR = '13' AND SRNO ='A')
BEGIN
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (6,'13','A',0,0,0,0,'')
END 
--- Total of RAO Details
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PART = 6 AND PARTSR = '13' AND SRNO ='A'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (6,'13','Z',0,ROUND(@AMTA1,0),0,0,'')
---UDPATE OF Refund adjustment order No. (As per Box 13 F)
  UPDATE #FORM221  SET  AMT1 = ROUND(@AMTA1,0) WHERE PART =1 AND PARTSR= '13' AND SRNO ='H'
  
 ------ Total available credit (a+b+c+d+e+f+g+h)
 SET @AMTA1 = 0
 SELECT @AMTA1 = ISNULL(SUM(AMT1),0) FROM  #FORM221  WHERE PART =1 AND PARTSR= '13' AND SRNO in('A','B','C','D','E','F','G','H')
 UPDATE #FORM221  SET  AMT1 = ROUND(@AMTA1,0) WHERE PART =1 AND PARTSR= '13' AND SRNO ='I'

----- 13 k 
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT @AMTA1 = AMT1  FROM #FORM221 WHERE PART = 1 AND PARTSR ='13' AND SRNO = 'I'
SELECT @AMTA2 = ISNULL(SUM(AMT1),0)  FROM #FORM221 WHERE PART = 2 AND PARTSR ='13' 
AND SRNO IN('A','B','C','D','E','F','G','H','J1','J2')
----INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (2,'13','K',0,ROUND((@AMTA1 -@AMTA2),0),0,0,'')
update #FORM221 set AMT1 =ROUND((@AMTA1 -@AMTA2),0) where PART = 2 AND PARTSR ='13' and SRNO = 'K'
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT @AMTA1 = AMT1  FROM #FORM221 WHERE PART = 1 AND PARTSR ='13' AND SRNO = 'I'
SELECT @AMTA2 = ISNULL(SUM(AMT1),0)  FROM #FORM221 WHERE PART = 2 AND PARTSR ='13' 
AND SRNO IN('A','B','C','D','E','F','G','H','J1','J2')
update #FORM221 set AMT1=ROUND((@AMTA2 -@AMTA1),0) where PART = 2 AND PARTSR ='13' and SRNO = 'L'
---update value for Total Amount  payable as per Box 13B(l)
update  #FORM221 set AMT1=ROUND((@AMTA2 -@AMTA1),0) where PART = 4 AND PARTSR ='13' and SRNO = 'A' 

---- Update statement for  --> Excess credit claimed as refund in this return (13 B(k)- 13c(a))
SET @AMTA1 = 0
SET @AMTA2 = 0
SELECT @AMTA1 = AMT1  FROM #FORM221 WHERE PART = 2 AND PARTSR ='13' AND SRNO = 'K'
SELECT @AMTA2 = ISNULL(SUM(AMT1),0)  FROM #FORM221 WHERE PART = 3 AND PARTSR ='13'  AND SRNO = 'A'
update  #FORM221 set AMT1=ROUND((@AMTA1-@AMTA2),0) where PART = 3 AND PARTSR ='13' and SRNO = 'B' 


Update #form221 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')
 
SELECT pART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = cast(isnull(AMT1,0) as integer), AMT2 = cast(isnull(AMT2,0)  as integer), 
					 AMT3 = cast(isnull(AMT3,0)  as integer), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'') 
					 ,CITY=isnull(CITY,''),RAOSNO=isnull(RAOSNO,''),RAODT =isnull(RAODT,'')
 from #FORM221 order by PART,case IsNumeric(partsr) 
          WHEN 1 THEN Replicate('000', 2-Len(partsr)) + partsr +'0'
          ELSE case when len(partsr)=3 then '00' else '000' end +partsr
         END,srno
         
 drop table #vattupcd_tbl
 END
set ANSI_NULLS OFF



