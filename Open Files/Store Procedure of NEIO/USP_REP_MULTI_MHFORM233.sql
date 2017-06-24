If Exists(Select [name] From SysObjects Where xType='P' and [Name]='USP_REP_Multi_MHFORM233')
Begin
	Drop Procedure USP_REP_Multi_MHFORM233
End
Go

/*
EXECUTE USP_REP_MULTI_MHFORM233'','','','04/01/2014','03/31/2015','','','','',0,0,'','','','','','','','','2014-2015',''

*/
-- =============================================
-- Author:		Hetal L Patel
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate MH VAT FORM 233
-- Modify date: 16/05/2007
-- Modified By: Hetal Patel Dt. 13/08/2009
-- Modify date: 
-- Remark:
-- =============================================
create PROCEDURE [dbo].[USP_REP_Multi_MHFORM233]
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
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
SELECT ENTRY_TY,BCODE=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),DBNAME  INTO #LCODE FROM LCODE  GROUP BY ENTRY_TY,STAX_ITEM,EXT_VOU,BCODE_NM,DBNAME
Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)
/*
----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),itemtype=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO VATTBL
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table VATTBL add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
INTO #FORM221
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
*/

SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=CAST('0' AS NUMERIC(20,2)),AMT2=CAST('0' AS NUMERIC(20,2)),AMT3=CAST('0' AS NUMERIC(20,2)),
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
,STM.RFORM_NM,CAST('0' AS NUMERIC(20,2)) AS AMT4,CAST('0' AS NUMERIC(20,2)) AS AMT5 --Added by Priyanka on 25022014
,TAX_NAME=CAST('' AS VARCHAR(12)),n.item,EIT_NAME=CAST('' AS VARCHAR(50)),CONTTAX=CAST(0 AS DECIMAL(12,2)),CTYPE=CAST('' AS VARCHAR(2)) 
INTO #FORM221
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
inner join stitem n on (m.tran_cd=n.tran_cd) 
WHERE 1=2


--Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
--IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
--Begin	------Fetch Records from Multi Co. Data
--	 Set @MultiCo = 'YES'
--	 EXECUTE USP_REP_MULTI_CO_DATA
--	  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
--	 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
--	 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
--	 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
--	 ,@MFCON = @MCON OUTPUT

--	--SET @SQLCOMMAND='Select * from '+@MCON
--	---EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	SET @SQLCOMMAND='Insert InTo  VATTBL Select * from '+@MCON
--	EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	---Drop Temp Table 
--	SET @SQLCOMMAND='Drop Table '+@MCON
--	EXECUTE SP_EXECUTESQL @SQLCOMMAND
--End
--else
--Begin ------Fetch Single Co. Data
--	 Set @MultiCo = 'NO'
	 EXECUTE USP_REP_MULTI_CO_DATA_VAT
	  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
	 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
	 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
	 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
	 ,@MFCON = @MCON OUTPUT
/*
	--SET @SQLCOMMAND='Select * from '+@MCON
	---EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	---Drop Temp Table 
	SET @SQLCOMMAND='Drop Table '+@MCON
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
*/	
--End


SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 

---Net Amount of Sale & Sales Return for the period
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('ST') 
group by b.tran_cd,b.net_amt,b.dbname)a

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','A',0,@AMTA1,0,0,'')

-- 6(b) Value of the Goods Return (Inclusive of sales tax), including reduction of sales price on account of rate difference and discount.
select @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b inner join stmain c on (b.tran_cd=c.tran_cd and b.dbname=c.dbname) 
where  b.BHENT in ('ST') and (c.u_gcssr=1 OR  c.u_choice=1 ) AND (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a

SELECT @AMTA2=Round(SUM(NET_AMT),0)   FROM 
(select b.net_amt from vattbl b 
where  b.BHENT in ('SR') and  (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a


SELECT @AMTB2=Round(SUM(NET_AMT),0)   FROM 
(select b.net_amt from vattbl b 
where  b.BHENT in ('CN') and  (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=@AMTA1+@AMTA2+@AMTB2
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','B',0,@AMTB1,0,0,'')

-- 6(c) a-b
--SET @AMTA1=@AMTA2-@AMTB2
SELECT @AMTA1=AMT1 FROM #FORM221 WHERE PARTSR='6' AND SRNO='A'
SELECT @AMTA2=AMT1 FROM #FORM221 WHERE PARTSR='6' AND SRNO='B'

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=@AMTA1-@AMTA2
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','C',0,@AMTB1,0,0,'')

-- 6(D) Gross receipts on account of sales under composition schemes other than works contracts under composition option. INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','D',0,0,0,0,'')

-- 6(E)g sales (excluding taxes) on account of on-going contracts where tax 
--liability has been discharged as per provisions of the ‘Earlier Law’ (on going works contracts 
--means contracts
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','E',0,0,0,0,'')

--6(F) sales (excluding taxes) on account of on-going leasing contracts where 
--tax liability has been discharged as per provisions of the 'Earlier Law'
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','F',0,0,0,0,'')

--6(G)Net turnover of sales including, taxes as well as turnover of non-sales transactions like Branch 
--Transfers/Consignment Transfers and job work charges etc. ( C- (D+E+F) )
SELECT @AMTA1=AMT1 FROM #FORM221 WHERE PARTSR='6' AND SRNO='C'
SELECT @AMTA2=SUM(AMT1) FROM #FORM221 WHERE PARTSR='6' AND SRNO IN ('D','E','F')

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=@AMTA1-@AMTA2
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','G',0,@AMTB1,0,0,'')


-- 6(H) Net Tax amount (Tax included in sales shown in (a) above less tax included in (b) above) 
SELECT @AMTA1=Round(SUM(B.TAXAMT),0)  from vattbl b where  b.BHENT in ('ST') 

select @AMTB1=Round(SUM(a.TAXAMT),0)  from vattbl A 
inner join stmain c on (A.tran_cd=c.tran_cd and A.dbname=c.dbname) 
where  A.BHENT in ('ST') and (c.u_gcssr=1 OR  c.u_choice=1 ) AND (C.DATE BETWEEN @SDATE AND @EDATE)   

SELECT @AMTA2=Round(SUM(B.TAXAMT),0)  from vattbl b where  b.BHENT in ('SR') 

SELECT @AMTB2=Round(SUM(B.TAXAMT),0) FROM vattbl b where  b.BHENT in ('CN') 

SET @AMTA1=ISNULL(@AMTA1,0)
SET @AMTA2=ISNULL(@AMTA2,0)
SET @AMTB1=ISNULL(@AMTB1,0)
SET @AMTB2=ISNULL(@AMTB2,0)

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','H',0,@AMTA1-(@AMTA2+@AMTB1+@AMTB2),0,0,'')

-- 6(I) Less:-Value of Branch Transfers/ Consignment Transfers within the State if the tax is to be paid by the Agent. 

SELECT @AMTA1=SUM(A.NET_AMT) FROM 
(select net_amt from vattbl 
where  BHENT='ST' and ST_TYPE='LOCAL'  AND (U_IMPORM IN ('Branch Transfer','Consignment Transfer')) AND (DATE BETWEEN @SDATE AND @EDATE)
group by tran_cd,net_amt,dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','I',0,@AMTA1,0,0,'')

--6(J)Less:-Sales u/s 8 (1)  i.e. Interstate Sales including Central Sales Tax,  Sales in the course of imports, exports and  value of  Branch Transfers/ Consignment transfers outside the State
SELECT @AMTA1=sum(a.NET_AMT) from
(select net_amt from vattbl 
where  BHENT='ST' and ST_TYPE in ('OUT OF STATE','OUT OF COUNTRY') 
group by tran_cd,net_amt,dbname)a

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','J',0,@AMTA1,0,0,'')


SELECT @AMTA1=round(SUM(a.NET_AMT),0) FROM 
(select net_amt from vattbl 
WHERE  ST_TYPE in ('OUT OF COUNTRY')  AND BHENT='ST' and U_IMPORM='Export Out of India' group by tran_cd,net_amt,dbname)a

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','JA',0,@AMTA1,0,0,'')

SELECT @AMTA1=Round(SUM(a.NET_AMT),0) FROM 
(select net_amt from vattbl 
WHERE ST_TYPE='OUT OF COUNTRY' AND BHENT='ST' and U_IMPORM='High Sea Sales' group by tran_cd,net_amt,dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','JB',0,@AMTA1,0,0,'')

-- 6(K) Non-taxable Labour and other charges / expenses for Execution of  Works Contract 
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','K',0,0,0,0,'')

-- 6(L) Amount paid by way of price for sub-contract 
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','L',0,0,0,0,'')

-- 6(M)Turnover of all sales including sales of tax-free goods in schedule-aINSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','M',0,0,0,0,'')

-- 6(n) Less:-Sales of taxable goods fully exempted u/s. 8 other than sales under section 8(1) and covered in Box 6(j)
SELECT @AMTA2=round(SUM(a.NET_AMT),0) from
(select net_amt from vattbl 
where  BHENT='ST' and ST_TYPE='LOCAL'  AND TAX_NAME in ('EXEMPTED','') AND (DATE BETWEEN @SDATE AND @EDATE)
group by tran_cd,net_amt,dbname)a

SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','N',0,@AMTA2,0,0,'')


--- 6(O) Net Amount of Labour/Job Work Charges for the period
select @AMTA1=Round(sum(net_amt),0) from VATTBL where (Date between @Sdate and @Edate)  and st_type = 'Local' and Bhent = 'LI' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','O',0,@AMTA1,0,0,'')

-- 6(P)Any Other Specific Sales if any for the period
---Branch Transfer Sale (Local) without Tax within the State
--Select @AMTA1=Round(sum(Net_amt),0) from VATTBL where (Date between @Sdate and @Edate) and set_app = 0 and st_type = 'Local' and Bhent = 'ST' and u_imporm = 'Branch Transfer' and Tax_name = ' '
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','P',0,0,0,0,'')

-- 6(Q) NET TURNOVER OF SALES LIABLE TO TAX
SELECT @AMTA2=round(AMT1,0) FROM #FORM221  WHERE PARTSR='6' AND SRNO='G'

SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE PARTSR='6' AND SRNO IN ('H','I','J','K','L','M','N','O','P')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'6','Q',0,@AMTA2-@AMTA1,0,0,'')

--SELECT @AMTA2=Round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='6') AND SRNO IN ('C')
--SET @AMTA2=(CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END)-@AMTA1
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'6','R',0,@AMTA2,0,0,'')

--->Part 7
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','A',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','B',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','BA',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','BB',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','BC',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','BD',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','C',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','CA',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','D',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','DA',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','E',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','EA',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','EB',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','EC',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'7','F',0,0,0,0,'')
--->Part 7

--->Part 8
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'8','A',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'8','B',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'8','C',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'8','D',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'8','E',0,0,0,0,'')
--->Part 8

--->Part 9
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'9','A',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'9','B',0,0,0,0,'')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES (1,'9','C',0,0,0,0,'')

--->Part 9

--->PART 1-6 ST_TYPE='LOCAL','OUT OF STATE'
-->---PART 10A
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
DECLARE  CUR_FORM221 CURSOR FOR 
select distinct per from vattbl where ST_TYPE='LOCAL' and tax_name like '%VAT%'
OPEN CUR_FORM221
FETCH NEXT FROM CUR_FORM221 INTO @PER
WHILE (@@FETCH_STATUS=0)
BEGIN
		SELECT @AMTA1=Round(SUM(VATONAMT),0) FROM VATTBL where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE)  AND PER=@PER and U_imporm <> 'Purchase Return' and st_type='LOCAL' AND TAX_NAME LIKE '%VAT%'
		SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM VATTBL where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE)  AND PER=@PER and U_imporm <> 'Purchase Return' and st_type='LOCAL' AND TAX_NAME LIKE '%VAT%'
		SELECT @AMTC1=Round(SUM(VATONAMT),0) FROM VATTBL where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) AND PER=@PER and st_type='LOCAL' AND TAX_NAME LIKE '%VAT%'
		SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM VATTBL where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE)  AND PER=@PER and st_type='LOCAL' AND TAX_NAME LIKE '%VAT%'
		SELECT @AMTA2=Round(SUM(VATONAMT),0) FROM VATTBL where bhent = 'CN' AND (DATE BETWEEN @SDATE AND @EDATE) AND PER=@PER and st_type='LOCAL' AND TAX_NAME LIKE '%VAT%'
		SELECT @AMTB2=Round(SUM(TAXAMT),0)  FROM VATTBL where bhent = 'CN' AND (DATE BETWEEN @SDATE AND @EDATE)  AND PER=@PER and st_type='LOCAL' AND TAX_NAME LIKE '%VAT%'		

--Sales Invoices
SET @AMTA1=ISNULL(@AMTA1,0)
SET @AMTB1=ISNULL(@AMTB1,0)

--Return Invoices
SET @AMTC1=ISNULL(@AMTC1,0)
SET @AMTD1=ISNULL(@AMTD1,0)
SET @AMTA2=ISNULL(@AMTA2,0)
SET @AMTB2=ISNULL(@AMTB2,0)

--Net Effect
Set @NetEFF = @AMTA1-(@AMTC1+@AMTA2)
--Set @NetEFF = (@AMTA1-@AMTB1)-(@AMTC1-@AMTD1)
Set @NetTAX = (@AMTB1)-(@AMTD1+@AMTB2)

--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10A',CHAR(@CHAR),@PER,@NETEFF,@NETTAX,0,'')
-----(1,'10A',CHAR(@CHAR),@PER,@AMTA1-@AMTB1,@AMTB1,0)
--
----	SET @AMTJ1=@AMTJ1+@AMTA1 --TOTAL TAXABLE AMOUNT
----	SET @AMTK1=@AMTK1+@AMTB1 --TOTAL TAX
if @nettax <> 0
	  begin
		  INSERT INTO #FORM221
		  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		  (1,'10A',CHAR(@CHAR),@PER,@NETEFF,@NETTAX,0,'')
		--  (1,'6',CHAR(@CHAR),@PER,@AMTA1-@AMTB1,@AMTB1,0)
		  
		--  SET @AMTJ1=@AMTJ1+@AMTA1 --TOTAL TAXABLE AMOUNT
		--  SET @AMTK1=@AMTK1+@AMTB1 --TOTAL TAX
		  SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
		  SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX
		  SET @CHAR=@CHAR+1
	  end
--SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
--SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX


--SET @CHAR=@CHAR+1
FETCH NEXT FROM CUR_FORM221 INTO @PER
END
CLOSE CUR_FORM221
DEALLOCATE CUR_FORM221

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10A','Z',0,@AMTJ1,@AMTK1,0,'')
--(1,'10A','Z',0,@AMTJ1-@AMTK1,@AMTK1,0)

--<---PART 10A	&&7
-->---PART 10B	&&8
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 

---- 11(a) Turnover of purchases should also include value of Branch Transfers / Consignment Transfers received and job work charges 
--SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
--(select b.net_amt from vattbl b 
--where  b.BHENT in ('PT') AND (b.DATE BETWEEN @SDATE AND @EDATE)   
--group by b.tran_cd,b.net_amt)a
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10B','A',0,@AMTA1,0,0,'')

--11(b) ,Raw Material Sale,Branch Transfer,Purchase Return
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PR') AND (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a

SELECT @AMTA2=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('ST') AND b.U_IMPORM ='Purchase Return' and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a

SELECT @AMTB1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('DN') and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','B',0,@AMTA1+@AMTA2+@AMTB1,0,0,'')

-- 11(c) Direct Import
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE='OUT OF COUNTRY' AND b.U_IMPORM='Direct Imports' and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','C',0,@AMTA1,0,0,'')

-- 11(d) Import(High Seas Purchases)
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE='OUT OF COUNTRY' AND b.U_IMPORM='High Seas Purchases' and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','D',0,@AMTA1,0,0,'')

-- 11(e) Inter State purchase (Excluding FOrm H)
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE='OUT OF STATE' AND b.U_IMPORM='' and form_nm not in ('FORM H','H FORM') and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','E',0,@AMTA1,0,0,'')

-- 11(e1) Purchase of taxable goods(either local or interstate) Against Form H
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE in ('OUT OF STATE','LOCAL') AND b.U_IMPORM='' and form_nm  in ('FORM H','H FORM') and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','E1',0,@AMTA1,0,0,'')

-- 11(f) Interstate Branch/Consignment Transfer
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE='OUT OF STATE' AND b.U_IMPORM in ('Branch Transfer','Consignment Transfer') and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','F',0,@AMTA1,0,0,'')

-- 11(g) within state Branch/Consignment Transfer
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE='LOCAL' AND b.U_IMPORM in ('Branch Transfer','Consignment Transfer') and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','G',0,@AMTA1,0,0,'')

-- 11(h) within state purchase of taxable goods from unregistered dealer
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') and  b.ST_TYPE='LOCAL' and b.tax_name like '%VAT%' and b.s_tax='' and (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','H',0,@AMTA1,0,0,'')

-- 11(i) purchase of taxable goods from registered dealer and which are not eligible for set off
SELECT @AMTA1=round(SUM(NET_AMT),0) FROM VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND (DATE BETWEEN @SDATE AND @EDATE) AND TAX_NAME='EXEMPTED'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','I',0,@AMTA1,0,0,'')

-- 11(j) within state purchase of taxable good which are fully exempted 
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT='PT' and b.ST_TYPE='LOCAL' and b.tax_name in ('EXEMPTED','') AND (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','J',0,0,0,0,'') -- (NOT KNOW)

-- 11(k) within state purchases of tax free goods specified in schedule A	
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT='PT' and b.ST_TYPE='LOCAL' and b.vattype='Schedule A' AND (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','K',0,@AMTA1,0,0,'')

-- 11(l) Other allowable reduction if any
--SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='10B') AND SRNO IN ('B','C','D','E','F','G','H','I','J','K','L')
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','L',0,0,0,0,'') -- (NOT KNOW)

---- 11(m) within state purchase of taxble good from register dealers eligible for set off [a-(b+c+d+e+f+g+h+i+j+k+l)]
--SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE PARTSR='10B' AND SRNO IN ('B','C','D','E','E1','F','G','H','I','J','K','L')
--SELECT @AMTA2=round(SUM(AMT1),0) FROM #FORM221  WHERE  PARTSR='10B' AND SRNO='A'
--PRINT @AMTA1
--PRINT @AMTA2
--SET @AMTA2=ISNULL(@AMTA2,0)
--SET @AMTA1=ISNULL(@AMTA1,0)

--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10B','M',0,@AMTA2-@AMTA1,0,0,'')

-- 11A Computation of purchase tax payable on the purchases effected during this period 
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 

INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,PARTY_NM) 
select 1,'10C','',it.rate_per,sum( CASE WHEN A.BHENT='PT' THEN a.gro_amt ELSE -ISNULL(a.gro_amt,0) END),cast((sum( CASE WHEN A.BHENT='PT' THEN a.gro_amt ELSE -ISNULL(a.gro_amt,0) END)*it.rate_per)/100 as decimal(12,2)),''  from vattbl a--(SELECT NET_AMT,it_code,bhent,vattype FROM VATTBL GROUP BY tran_cd,net_amt,it_code,bhent,vattype) A
inner JOIN vatmaster IT ON(IT.vatcategory=A.vattype  and a.dbname=it.dbname)
WHERE A.BHENT in ('PT','PR','DN') and (a.date between it.sdate and it.edate) and IT.STATE_CODE=15 and A.TAX_NAME LIKE '%VAT%' and A.ST_TYPE='LOCAL' AND A.S_TAX<>''
group by it.rate_per

select @AMTA1=SUM(AMT1),@AMTA2=SUM(AMT2) FROM #FORM221 WHERE PARTSR='10C' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=@AMTA1+@AMTA2 -- FOR 11(A)
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10C','Z',0,@AMTA1,@AMTA2,0,'')


-- 11(a) Turnover of purchases should also include value of Branch Transfers / Consignment Transfers received and job work charges 
SELECT @AMTA1=Round(SUM(a.NET_AMT),0)  from 
(select b.net_amt from vattbl b 
where  b.BHENT in ('PT') AND (b.DATE BETWEEN @SDATE AND @EDATE)   
group by b.tran_cd,b.net_amt,b.dbname)a
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','A',0,@AMTA1+@AMTB1,0,0,'')

-- 11(m) within state purchase of taxble good from register dealers eligible for set off [a-(b+c+d+e+f+g+h+i+j+k+l)]
SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE PARTSR='10B' AND SRNO IN ('B','C','D','E','E1','F','G','H','I','J','K','L')
SELECT @AMTA2=round(SUM(AMT1),0) FROM #FORM221  WHERE  PARTSR='10B' AND SRNO='A'
PRINT @AMTA1
PRINT @AMTA2
SET @AMTA2=ISNULL(@AMTA2,0)
SET @AMTA1=ISNULL(@AMTA1,0)

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10B','M',0,@AMTA2-@AMTA1,0,0,'')

--12 Tax rate wise break up within the state purchase from register dealers eligible for set-off as per box 11(m) above
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,PARTY_NM) 
select 1,'10D','',A.PER,sum(CASE WHEN A.BHENT='PT' THEN a.vatonamt ELSE -ISNULL(a.vatonamt,0) END),SUM(CASE WHEN A.BHENT='PT' THEN a.TAXAMT ELSE -ISNULL(a.TAXAMT,0) END),''  from vattbl a--(SELECT NET_AMT,it_code,bhent,vattype FROM VATTBL GROUP BY tran_cd,net_amt,it_code,bhent,vattype) A
WHERE A.BHENT IN ('PT','PR','DN') and A.TAX_NAME LIKE '%VAT%'   and st_type='LOCAL' AND S_TAX<>''
group by A.PER

update A SET A.AMT1=A.AMT1+B.AMT1,A.AMT2=A.AMT2+B.AMT2 FROM #FORM221 A,#FORM221 B
WHERE A.PARTSR='10D' AND B.PARTSR='10C' AND (A.RATE=B.RATE)


select @AMTA1=SUM(AMT1),@AMTA2=SUM(AMT2) FROM #FORM221 WHERE PARTSR='10D' 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10D','Z',0,@AMTA1,@AMTA2,0,'')


-- 13(a)Within the State purchases of taxable goods from registered dealers eligible for set-off as per Box 12
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','A',0,@AMTA1,@AMTA2,0,'')


-- 13(b)The corresponding purchase price of (sch C,D & E) goods
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','B',0,0,0,0,'')

-- 13(b)Less:-Reduction in the amount of set off u/r 53(2) of the corresponding purchase price of (Sch B) goods
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','BA',0,0,0,0,'')

-- 13(c)Less:-Reduction in the amount of set-off  under any other sub rule of Rule 53
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','C',0,0,0,0,'')

-- 13(d)Add:Adjustment to set-off claimed Short in earlier return
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','D',0,0,0,0,'')

-- 13(e)Less:Adjustment to Excess set-off claimed in earlier return
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','E',0,0,0,0,'')

-- 13(f)Set-off available for the period covered of this return [a-(b+c+d+e)]
select @AMTA1=AMT1,@AMTA2=AMT2 FROM #FORM221 WHERE PARTSR='10E' AND SRNO='A'
select @AMTB1=SUM(AMT1),@AMTB2=SUM(AMT2) FROM #FORM221 WHERE PARTSR='10E' AND SRNO IN ('B','BA','C','D','E')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
SET @AMTB2=CASE WHEN @AMTB2 IS NULL THEN 0 ELSE @AMTB2 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10E','F',0,@AMTA1-@AMTB1,@AMTA2-@AMTB2,0,'')

-- 14A(a)Set off available as per Box 13(f)
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','A',0,@AMTA2-@AMTB2,0,0,'')


-- 14(A)b Excess credit brought forward from previous tax period
DECLARE @STARTDT SMALLDATETIME,@ENDDT SMALLDATETIME,@TMONTH INT,@TYEAR INT
SET @TMONTH=DATEDIFF(M,@SDATE,@EDATE)
SET @TYEAR=DATEDIFF(YY,@SDATE,@EDATE)
SET @STARTDT=DATEADD(Y,-@TYEAR,@STARTDT)
SET @STARTDT=DATEADD(M,-(@TMONTH+1),@SDATE)
SET @ENDDT=DATEADD(D,-1,@SDATE)
select @AMTA1=SUM(A.NET_AMT) FROM JVMAIN A WHERE A.ENTRY_TY='J4' AND A.VAT_ADJ='Excess credit carried forward to  subsequent tax period' AND (A.DATE BETWEEN @STARTDT AND @ENDDT)
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'10F','B',4,@AMTA1,0,0,'')

----14(A)c Amount already paid (Details to be entered in Box 14 E)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10F','C',0,0,0,0,'')

--14(A)d Excess credit if any ,as per form 234,to be adjusted against the liability as per form 233
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','D',0,0,0,0,'')

--14(A)e Adjustment of ET paid under Maharashtra Tax on entry of goods into local areas Act,2002/Maharashtra Tax on Entery of Motor Vehicle Act into Local Areas Act 1987
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','E',0,0,0,0,'')

--14(A)e1 Amount of Tax collected at source u/s 31A
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','E1',0,0,0,0,'')

----14(A)f Refund adjustment order No(Details to be entered in Box 14F)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10F','F',0,0,0,0,'')

--14(A)g Work Contract Tax (WCT) TDS.
SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='10F') AND SRNO IN ('A','B','C','D','E','F')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--Added for wct on 07-07-2014 
SELECT @AMTA2=SUM(NET_AMT) FROM (select distinct A.tran_cd,A.bhent,C.NET_AMT
FROM VATTBL A inner join #lcode lc on (a.bhent=lc.entry_ty and a.dbname=lc.dbname)
INNER JOIN JVMAIN C ON ( A.TRAN_cD=C.TRAN_cD AND A.BHENT=C.ENTRY_TY  and a.dbname=c.dbname)
WHERE A.BHENT IN ('J4') AND C.VAT_ADJ='Work Contract Tax (WCT)' AND (A.DATE BETWEEN @SDATE AND @EDATE))B 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','G',0,@AMTA2,0,0,'')

----14(A)h Total available credit(a+b+c+d+e+e1+f+g)
--select @AMTA1=SUM(AMT1) FROM #FORM221 WHERE PARTSR='10F' AND SRNO IN ('A','B','C','D','E','E1','F','G')
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10F','H',0,@AMTA1,0,0,'')


--14(B)a Sales tax Payable as per Box 10 + Purchase Tax payable as per box 11A
SELECT @AMTA1=AMT2 FROM #FORM221 WHERE PART=1 AND PARTSR='10A' AND SRNO='Z'
SELECT @AMTA2=AMT2 FROM #FORM221 WHERE  PARTSR='10C' AND SRNO='Z'

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','A',0,@AMTA1+@AMTA2,0,0,'')

--14(B)b Adjustment on account of MVAT payable,if any,as per return form-234 or 235 against excess credit as
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','B',0,0,0,0,'')

--14(B)c Adjustment on account of CST payable as per return for this period
select @AmtA1=round(Sum(TaxAmt),0) from VATTBL where St_type = 'Out of State' and Bhent = 'ST' and tax_name like '%C.S.T%'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','C',0,@AMTA1,0,0,'')

--14(B)d Adjustment on account of ET payable under the Maharashtra Tax on Entry of Goods into Local Areas Act, 2002/
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','D',0,0,0,0,'')

--14(B)e Amount of sales tax collected in excess of the amount of sales tax payable,if any (as per Box 10A)
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','E',0,0,0,0,'')

--14(B)f Interest Payable
select @AMTA1=SUM(C.AMOUNT) FROM (select AMOUNT from LAC_VW A
LEFT OUTER JOIN LMAIN_VW B ON (A.TRAN_CD=B.TRAN_CD AND a.dbname=b.dbname) 
WHERE B.VAT_ADJ='Interest Payable' AND (A.DATE BETWEEN @SDATE AND @EDATE) GROUP BY A.TRAN_CD,A.AMOUNT)C

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','F',0,@AMTA1,0,0,'')

--14(B)f1 Late Fee Payable
select @AMTA1=SUM(C.AMOUNT) FROM (select AMOUNT from LAC_VW A
LEFT OUTER JOIN LMAIN_VW B ON (A.TRAN_CD=B.TRAN_CD AND a.dbname=b.dbname) 
WHERE B.VAT_ADJ='Late Fees Payable' AND (A.DATE BETWEEN @SDATE AND @EDATE) GROUP BY A.TRAN_CD,A.AMOUNT)C
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','F1',0,@AMTA1,0,0,'')


----14(B)g Balance: Excess credit [14A(h)-(14B(a)+14B(b)+14B(c)+14B(d)+14B(e)+14B(f)+14B(f1))]
--SELECT @AMTA2=AMT1 FROM #FORM221 where PARTSR='10F' AND SRNO='H'
--SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='10G') AND SRNO IN ('A','B','C','D','E','F','F1')
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10G','G',0,@AMTA2-@AMTA1,0,0,'')

----14(B)h Balance:Tax Payable [(14B(a)+14B(b)+14B(c)+14B(d)+14B(e)+14B(f)+14B(f1))-14A(h)]
--SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='10G') AND SRNO IN ('A','B','C','D','E','F','F1')
--SELECT @AMTA2=AMT1 FROM #FORM221 where PARTSR='14F' AND SRNO='H'

--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10G','H',0,@AMTA1-@AMTA2,0,0,'')


----14(C)a Excess credit carried forward to subsequent tax period
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10H','A',0,0,0,0,'')

----14(C)b Excess credit claimed as refund in this return [Box 14B(g)-14C(a)]
--SELECT @AMTA1=AMT1 FROM #FORM221 where PARTSR='10G' AND SRNO='G'
--SELECT @AMTA2=AMT1 FROM #FORM221 where PARTSR='10H' AND SRNO='A'

--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10H','B',0,@AMTA1-@AMTA2,0,0,'')

----14(D)a Total Amount payable as per Box 14B(h)
--SELECT @AMTA1=AMT1 FROM #FORM221 where PARTSR='10G' AND SRNO='H'
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10I','A',0,@AMTA1,0,0,'')

----14(D)b Amount paid along with return-cum-challan(Details to be entered in Box 14A)
--SELECT @AMTA1=AMT1 FROM #FORM221 where PARTSR='10F' AND SRNO='H'
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END

--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10I','B',0,@AMTA1,0,0,'')

----14(D)c Amount paid as per Revised/Fresh Return
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10I','C',0,0,0,0,'')

Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4)
SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0

-- 14(E) Details of the Amount Paid along with this return and or  Amount Already Paid.	
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,INV_NO,DATE,PARTY_NM,ADDRESS)-- VALUES (1,'10J',CHAR(@CHAR),@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX)
SELECT 1,'10J','',0,A.NET_AMT,B.U_CHALNO,A.DATE,B.BANK_NM,C.S_TAX FROM VATTBL A
INNER JOIN BPMAIN B ON (A.TRAN_CD=B.TRAN_CD and a.dbname=b.dbname)
INNER JOIN AC_MAST C ON (B.BANK_NM=C.AC_NAME and b.dbname=c.dbname) WHERE A.AC_NAME='VAT PAYABLE' and (A.DATE BETWEEN @SDATE AND @EDATE)
select @AMTA1=Sum(AMT1) from #form221 where Partsr = '10J'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES  (1,'10J','Z',0,@AMTA1,0,0,'')

--14(A)c Amount already paid (Details to be entered in Box 14 E)
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','C',0,@AMTA1,0,0,'')


-- 14(F) Details of RAO
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,INV_NO,DATE,Party_nm) 
select 1,'10K','',0,A.NET_AMT,A.RAOSNO,A.RAODT,'' FROM JVMAIN A WHERE ENTRY_TY='J4' AND (A.RAOSNO<>'' OR A.RAODT<>'') and A.VAT_ADJ='' AND A.PARTY_NM='VAT PAYABLE' AND (A.DATE BETWEEN @SDATE AND @EDATE)

select @AMTA1=Sum(AMT1) from #form221 where Partsr = '10K'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES  (1,'10K','Z',0,@AMTA1,0,0,'')

--14(A)f Refund adjustment order No(Details to be entered in Box 14F)
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','F',0,@AMTA1,0,0,'')

--14(A)h Total available credit(a+b+c+d+e+e1+f+g)
select @AMTA1=SUM(AMT1) FROM #FORM221 WHERE PARTSR='10F' AND SRNO IN ('A','B','C','D','E','E1','F','G')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10F','H',0,@AMTA1,0,0,'')

--14(B)g Balance: Excess credit [14A(h)-(14B(a)+14B(b)+14B(c)+14B(d)+14B(e)+14B(f)+14B(f1))]
SELECT @AMTA2=AMT1 FROM #FORM221 where PARTSR='10F' AND SRNO='H'
SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='10G') AND SRNO IN ('A','B','C','D','E','F','F1')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','G',0,@AMTA2-@AMTA1,0,0,'')

--14(C)a Excess credit carried forward to subsequent tax period
select @AMTA1=SUM(A.NET_AMT) FROM JVMAIN A WHERE A.ENTRY_TY='J4' AND A.VAT_ADJ='Excess credit carried forward to  subsequent tax period' AND (A.DATE BETWEEN @SDATE AND @EDATE)
SET @AMTA1=ISNULL(@AMTA1,0)
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10H','A',0,@AMTA1,0,0,'')

--14(B)h Balance:Tax Payable [(14B(a)+14B(b)+14B(c)+14B(d)+14B(e)+14B(f)+14B(f1))-14A(h)]
SELECT @AMTA1=round(SUM(AMT1),0) FROM #FORM221  WHERE (PART=1) AND (PARTSR='10G') AND SRNO IN ('A','B','C','D','E','F','F1')
SELECT @AMTA2=AMT1 FROM #FORM221 where PARTSR='14F' AND SRNO='H'

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10G','H',0,@AMTA1-@AMTA2,0,0,'')


--14(C)b Excess credit claimed as refund in this return [Box 14B(g)-14C(a)]
SELECT @AMTA1=AMT1 FROM #FORM221 where PARTSR='10G' AND SRNO='G'
SELECT @AMTA2=AMT1 FROM #FORM221 where PARTSR='10H' AND SRNO='A'

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10H','B',0,@AMTA1-@AMTA2,0,0,'')

--14(D)a Total Amount payable as per Box 14B(h)
SELECT @AMTA1=AMT1 FROM #FORM221 where PARTSR='10G' AND SRNO='H'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL or @AMTA1<0  THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10I','A',0,@AMTA1,0,0,'')

--14(D)b Amount paid along with return-cum-challan(Details to be entered in Box 14A)
SELECT @AMTA1=AMT1 FROM #FORM221 where PARTSR='10F' AND SRNO='H'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END

INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10I','B',0,@AMTA1,0,0,'')

--14(D)c Amount paid as per Revised/Fresh Return
INSERT INTO #FORM221
(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
(1,'10I','C',0,0,0,0,'')


--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--(1,'10K','Z',0,@AMTM1,@AMTO1,0,'')

Update #form221 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
	             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
				 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
				 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
				 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),  ITEM =isnull(item,''),

select a.co_name,a.stax_no,a.add1,a.add2,a.add3,a.city,a.phone,a.zip,a.email,vatpretdate=cast('' as smalldatetime),vatpname=space(35),vatpremarks=space(100),vatpdesig=space(35),vatpmobileno=space(50),vatpemail=space(35),stax_loc=space(100),a.dbname
into #compdet
from vudyog..co_mast a
where 1=2

insert into #compdet 
select a.co_name,a.stax_no,a.add1,a.add2,a.add3,a.city,a.phone,a.zip,a.email,'','','','','','','',a.dbname
from vudyog..co_mast a
where a.co_name=(select top 1 co_name from com_det)

declare @dbname varchar(10)
select @dbname=a.dbname 
from vudyog..co_mast a
where a.co_name=(select top 1 co_name from com_det)

execute('update a set a.vatpretdate=b.vatpretdate,a.vatpname=b.vatpname,a.vatpremarks=b.vatpremarks,a.vatpdesig=b.vatpdesig
,a.vatpmobileno=b.vatpmobileno,a.vatpemail=b.vatpemail,a.stax_loc=b.stax_loc
from #compdet a,'+@dbname+'..manufact b')


SELECT b.*,a.*
FROM #FORM221 a,#compdet b 
order by CASE IsNumeric(a.partsr) 
WHEN 1 THEN Replicate('0', 4- Len(a.partsr)) + a.partsr +'0'
ELSE '00'+a.partsr
END,a.SRNO
--order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
--SELECT * FROM VATTBL --order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)

END
--Print 'MH VAT FORM 233'

