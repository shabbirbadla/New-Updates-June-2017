If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_TG_VAT_200E')
Begin
	Drop Procedure USP_REP_TG_VAT_200E
End
GO
/*
EXECUTE USP_REP_TG_VAT_200E'','','','04/01/2013','03/31/2016','','','','',0,0,'','','','','','','','','2015-2016',''
*/
-- =============================================
-- Author:		Guarav Tanna
-- Create date: 10/10/2014	for BUG-23935 (Initially created same as AP VAT)
-- Description:	This Stored procedure is useful to generate TG VAT FORM 200E.
-- Remark:
-- Modify by  : Sumit Gavate
-- Remark	  : Modify for Bug no - 27971
-- Modify date: 19/04/2015
-- =============================================
CREATE Procedure [dbo].[USP_REP_TG_VAT_200E]
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
Declare @FCON as NVARCHAR(2000),@fld_list NVARCHAR(2000)
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='M',@VITFILE=Null,@VACFILE='i'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT


DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)  
DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)  
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)  

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
-- bug-23933 - start here
/*
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #FORM200_E
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #FORM200_E add recno int identity
*/
-- bug-23933 - end here

---Temporary Cursor2
DECLARE @TAXABLE_AMT NUMERIC(14,2)--,@taxamt numeric(14,2)
set @FCON=rtrim(@FCON)
select Part=1,SrNo1='AA',Taxable_amt=i.gro_amt,taxamt1=i.taxamt,taxamt2=i.taxamt,taxamt3=i.taxamt
,taxamt4=i.taxamt,st.tax_name,st.level1,st.st_type into #APVAT from ptitem i
inner join stax_mas st on (st.tax_name=i.tax_name) where 1=2
declare @sqlcommand nvarchar(4000)

/*
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
		
		-- bug-23933 - start here 
		--SET @SQLCOMMAND='Insert InTo #FORM200_E Select * from '+@MCON
		SET @SQLCOMMAND='Insert InTo vattbl Select * from '+@MCON
		-- bug-23933 - end here 
		
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 -- bug-23933 - start here 
		 --EXECUTE USP_REP_SINGLE_CO_DATA
		 EXECUTE USP_REP_SINGLE_CO_DATA_VAT
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		/*
		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo #FORM200_E Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		*/
		-- bug-23933 - end here 
	End
-----
-----SELECT * from #form221_1 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----

--Declare @AMTA1 As Numeric(12,2),@AMTA2 As Numeric(12,2),@AMTB1 As Numeric(12,2),@AMTB2 As Numeric(12,2),
--	    @AMTC1 As Numeric(12,2),@AMTC2 As Numeric(12,2),@PER As numeric (5,2)

Select @AMTA1=0,@AMTA2=0,@AMTB1=0,@AMTB2=0,@AMTC1=0,@AMTC2=0,@per=0


---Total of 13A,14A,16A,17A,19A of VAT Form 200
-- bug-23933 - start here 
/*
SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_E WHERE (DATE BETWEEN @SDATE AND @EDATE) and st_type = 'Out of Country' and BHENT = 'ST'
SELECT @AMTA2=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_E WHERE (st_type = 'Out of State') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTB1=SUM(NET_AMT)-Sum(TaxAmt), @Per = Per FROM #FORM200_E WHERE St_Type = 'Local' and (BHENT='ST') and Per = 4 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTB2=SUM(NET_AMT)-Sum(TaxAmt), @Per = Per FROM #FORM200_E WHERE St_Type = 'Local' and (BHENT='ST') and Per = 12.50 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTC1=SUM(NET_AMT)-Sum(TaxAmt), @Per = Per FROM #FORM200_E WHERE St_Type = 'Local' and (BHENT='ST') and Per = 1 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
*/
SELECT @AMTA1 = ISNULL(SUM(vattbl.GRO_AMT)-Sum(vattbl.TaxAmt),0) FROM VATTBL Inner Join STmain ON (vattbl.BHENT=stmain.ENTRY_TY AND
vattbl.TRAN_CD=stmain.TRAN_CD ) WHERE (vattbl.DATE BETWEEN @SDATE AND @EDATE) and vattbl.st_type = 'Out of Country'
and vattbl.BHENT = 'ST'  and vattbl.Per = 0 and stmain.u_imporm <> 'Composition Dealer'

SELECT @AMTA2 = ISNULL(SUM(GRO_AMT)-Sum(TaxAmt),0) FROM VATTBL WHERE (st_type = 'Out of State') AND BHENT='ST'
	AND (DATE BETWEEN @SDATE AND @EDATE) and Per = 0
SELECT @AMTB1 = ISNULL(SUM(GRO_AMT)-Sum(TaxAmt),0), @Per = Per FROM VATTBL WHERE St_Type = 'Local' and Tax_Name like '%VAT%'
	and s_tax<>'' and (BHENT In ('ST', 'PR', 'DN')) and Per = 4 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTB2 = ISNULL(SUM(GRO_AMT)-Sum(TaxAmt),0), @Per = Per FROM VATTBL WHERE St_Type = 'Local' and Tax_Name like '%VAT%'
	and s_tax<>'' and (BHENT In ('ST', 'PR', 'DN')) and Per = 12.50 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTC1 = ISNULL(SUM(GRO_AMT)-Sum(TaxAmt),0), @Per = Per FROM VATTBL WHERE St_Type = 'Local' and Tax_Name like '%VAT%'
	and s_tax<>'' and (BHENT In ('ST', 'PR', 'DN')) and Per = 1 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
-- bug-23933 - end here 

----SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
----SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
----SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
----SET @AMTB2=CASE WHEN @AMTB2 IS NULL THEN 0 ELSE @AMTB2 END
----SET @AMTC1=CASE WHEN @AMTC1 IS NULL THEN 0 ELSE @AMTC1 END
set @AMTC2 = ISNULL(@AMTA1 + @AMTA2 + @AMTB1 + @AMTB2 + @AMTC1,0)
--SET @AMTC2=CASE WHEN @AMTC2 IS NULL THEN 0 ELSE @AMTC2 END

INSERT INTO #APVAT (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'A',@AMTC2,0,0,0,0,'',0,'')

---Amount of sales of exempt goods in the period
Set @AMTA1 = 0
-- bug-23933 - start here 
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_E  WHERE (st_type = 'Out of Country') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM VATTBL  WHERE (st_type = 'Out of Country') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTA1= CASE WHEN LCODE.STAX_ITEM = 1 THEN (round(STITEM.u_asseamt+STITEM.EXAMT+STITEM.U_CESSAMT+STITEM.U_HCESAMT+STITEM.tot_add+STITEM.TOT_DEDUC,2)) else round(((STITEM.gro_amt+STITEM.tot_add+STITEM.tot_tax)-STITEM.tot_deduc),2) end
FROM STITEM INNER JOIN IT_MAST ON (STITEM.It_code = IT_MAST.It_code) INNER JOIN LCODE ON (LCODE.ENTRY_TY=STITEM.ENTRY_TY)
INNER JOIN STMAIN ON (STITEM.inv_no = STMAIN.inv_no AND STITEM.Tran_cd = STMAIN.Tran_cd AND STITEM.date = STMAIN.date) 
WHERE IT_MAST.U_GVTYPE like 'Tax Free Sch - I goods' AND STMAIN.u_imporm <> 'Exempt Transaction' AND (STITEM.DATE BETWEEN @SDATE AND @EDATE)
-- bug-23933 - end here 
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'B',ISNULL(@AMTA1,0),0,0,0,0,'',0,'')


---Amount of exempt transactions in the period
Set @AMTA1 = 0
Set @AMTA2 = 0
-- bug-23933 - start here 
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_E WHERE (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name = 'Exempted' and BHENT = 'ST'
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM vattbl WHERE (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name = 'Exempted' and BHENT = 'ST'
SELECT @AMTA1 = ISNULL(Sum(round(((STMAIN.gro_amt+STMAIN.tot_add+STMAIN.tot_tax)-STMAIN.tot_deduc),2)),0)
FROM STMAIN WHERE stmain.u_imporm = 'Exempt Transaction' AND (STMAIN.DATE BETWEEN @SDATE AND @EDATE)
-- bug-23933 - end here 
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #APVAT (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'C',ISNULL(@AMTA1,0),0,0,0,0,'',0,'')


-- bug-23933 - start here 
--SELECT @AMTA1 = ISNULL(Sum(round(((STMAIN.gro_amt+STMAIN.tot_add+STMAIN.tot_tax)-STMAIN.tot_deduc),2)),0)
--FROM STMAIN WHERE stmain.u_imporm = 'Composition Dealer' AND (STMAIN.DATE BETWEEN @SDATE AND @EDATE)
-- bug-23933 - end here 
--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SELECT @AMTA1 = ISNULL(Sum(A.VATONAMT),0) FROM VATTBL A INNER JOIN JVMAIN J ON (A.BHENT = J.Entry_ty AND A.INV_NO = J.inv_no AND A.Date = J.date)
WHERE A.BHENT = 'J4' AND J.VAT_ADJ = 'Turnover under composition' AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #APVAT (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'D',ISNULL(@AMTA1,0),0,0,0,0,'',0,'')


INSERT INTO #APVAT (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'E',0,0,0,0,0,'',0,'')
--<- PART1

-->- PART2
/*
---Purchase on 1% VAT Rate
Set @AMTA1 = 0
Set @PER = 1
SELECT @AMTA1=SUM(TAXAMT),@Per=Per FROM #FORM200_E WHERE (BHENT='PT') and St_Type = 'Local' and Per = 1 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @Per  =CASE WHEN @PER IS NULL THEN 0 ELSE @PER END
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (2,'A',0,@AMTA1,0,0,0,'',@PER,'')

---Purchase on 4% VAT Rate
Set @AMTA1 = 0
Set @PER = 4
SELECT @AMTA1=SUM(TAXAMT),@PER=Per FROM #FORM200_E WHERE (BHENT='PT') and St_Type = 'Local'  and Per = 4 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (2,'B',0,@AMTA1,0,0,0,'',@PER,'')


---Purchase on 12.5% VAT Rate
Set @AMTA1 = 0
Set @PER = 12.5
SELECT @AMTA1=SUM(TAXAMT),@Per=Per FROM #FORM200_E WHERE (BHENT='PT') and St_Type = 'Local'  and Per = 12.5 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (2,'C',0,0,0,@AMTA1,0,'',@PER,'')
*/
 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 SET @CHAR=65
  
 DECLARE  CUR_FORM221 CURSOR FOR 
 select distinct level1 from stax_mas where ST_TYPE='LOCAL'--CHARINDEX('VAT',TAX_NAME)>0
 
 OPEN CUR_FORM221
 FETCH NEXT FROM CUR_FORM221 INTO @PER
 WHILE (@@FETCH_STATUS=0)
 BEGIN

    -- bug-23933 - (instead of FROM #FORM200_E, changed it to FROM VATTBL)
	Begin
		if @Per = 12.50
		BEGIN
			if NOT EXISTS (SELECT u_imporm FROM VATTBL A WHERE A.ST_TYPE='LOCAL' AND A.BHENT IN('PT','EP','SR','CN','PR')
			AND A.s_tax <> '' AND (DATE BETWEEN @SDATE AND @EDATE)
			AND A.u_imporm = 'Exempt Transaction')
			BEGIN
				SELECT @AMTA1 = ISNULL(SUM(B.Specific_inp),0),@AMTB1 = ISNULL(SUM(B.Common_inp),0) FROM
				(SELECT CASE WHEN IT.U_GVTYPE = 'Specific Input' THEN CASE WHEN A.BHENT = 'PR' THEN -(ISNULL(SUM(A.TAXAMT),0)) ELSE
				ISNULL(SUM(A.TAXAMT),0) END ELSE 0 END as Specific_inp,
				CASE WHEN IT.U_GVTYPE = 'Common Input' THEN CASE WHEN A.BHENT = 'PR' THEN -(ISNULL(SUM(A.TAXAMT),0)) ELSE
				ISNULL(SUM(A.TAXAMT),0) END ELSE 0 END as Common_inp
				FROM VATTBL A INNER JOIN IT_MAST IT ON (A.It_code = It.it_code) WHERE A.ST_TYPE='LOCAL' AND A.BHENT IN('PT','EP','SR','CN','PR')
				AND A.PER=@PER AND A.Tax_Name like '%VAT%' and A.s_tax <> '' AND (DATE BETWEEN @SDATE AND @EDATE)
				AND A.u_imporm <> 'Exempt Transaction' GROUP BY A.Bhent,IT.U_GVTYPE) B
			END
			ELSE
			BEGIN
				SELECT @AMTA1 = 0,@AMTB1 = 0
			END
		END
		ELSE
		BEGIN
			SELECT @AMTA1 = ISNULL(SUM(B.Specific_inp),0),@AMTB1 = ISNULL(SUM(B.Common_inp),0) FROM
			(SELECT CASE WHEN IT.U_GVTYPE = 'Specific Input' THEN CASE WHEN A.BHENT = 'PR' THEN -(ISNULL(SUM(A.TAXAMT),0)) ELSE
			ISNULL(SUM(A.TAXAMT),0) END ELSE 0 END as Specific_inp,
			CASE WHEN IT.U_GVTYPE = 'Common Input' THEN CASE WHEN A.BHENT = 'PR' THEN -(ISNULL(SUM(A.TAXAMT),0)) ELSE
			ISNULL(SUM(A.TAXAMT),0) END ELSE 0 END as Common_inp
			FROM VATTBL A INNER JOIN IT_MAST IT ON (A.It_code = It.it_code) WHERE A.ST_TYPE='LOCAL' AND A.BHENT IN('PT','EP','SR','CN','PR')
			AND A.PER=@PER AND A.Tax_Name like '%VAT%' and A.s_tax <> '' AND (DATE BETWEEN @SDATE AND @EDATE) GROUP BY A.Bhent,IT.U_GVTYPE) B
		END
		--SELECT @AMTA1=SUM(GRO_AMT)  FROM (select distinct tran_cd,bhent,GRO_AMT,dbname from VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>'') b
		--SELECT @AMTB1=SUM(TAXAMT)   FROM VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='PT' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>''
		--SELECT @AMTC1=SUM(GRO_AMT) FROM (select distinct tran_cd,bhent,GRO_AMT,dbname from VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='SR' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>'') b
		--SELECT @AMTD1=SUM(TAXAMT)   FROM VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='SR' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>''
		--SELECT @AMTF1=SUM(GRO_AMT) FROM (select distinct tran_cd,bhent,GRO_AMT,dbname from VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='EP' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>'') b
		--SELECT @AMTF2=SUM(TAXAMT)   FROM VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='EP' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>''
		--SELECT @AMTG1=SUM(GRO_AMT) FROM (select distinct tran_cd,bhent,GRO_AMT,dbname from VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='CN' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>'') b
		--SELECT @AMTG2=SUM(TAXAMT)   FROM VATTBL WHERE ST_TYPE='LOCAL' AND BHENT='CN' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name like '%VAT%' and s_tax<>''
	End

  --Purchase Invoice
  --SET @AMTA1=ISNULL(@AMTA1,0)
  --SET @AMTB1=ISNULL(@AMTB1,0)
  --Return Invoice
  --SET @AMTC1=ISNULL(@AMTC1,0)
  --SET @AMTD1=ISNULL(@AMTD1,0)
  --Expense Purchase Invoice
  --SET @AMTF1=ISNULL(@AMTF1,0)
  --SET @AMTF2=ISNULL(@AMTF2,0)
  --Credit Note
  --SET @AMTG1=ISNULL(@AMTG1,0)
  --SET @AMTG2=ISNULL(@AMTG2,0)
  --Sales Invoice Where U_imporm = 'Purchase Return'
  --SET @AMTH1=ISNULL(@AMTH1,0)
  --SET @AMTH2=ISNULL(@AMTH2,0)

 -- Net Purchase = (Purchase + Expense Purchase + Sales Return + Credit Note)
  --Set @NetEFF = ((@AMTA1 - @AMTB1) + (@AMTF1 - @AMTF2) + (@AMTC1 - @AMTD1) + (@AMTG1 - @AMTG2)) 
  --Set @NetTAX = (@AMTB1 + @AMTF2  + @AMTD1  + @AMTG2)

  -- bug-23933 - start here 
  /*
  if @nettax <> 0
	  Begin
		  INSERT INTO #APVAT 
          (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) 
		   VALUES (2,CHAR(@CHAR),@NETEFF,0,0,@NETTAX,0,'',@PER,'')

		  SET @AMTM1=@AMTM1+@NETEFF --TOTAL TAXABLE AMOUNT
		  SET @AMTO1=@AMTO1+@NETTAX --TOTAL TAX
		  SET @CHAR=@CHAR+1
	  end
*/
	if (@AMTA1 <> 0) or (@AMTB1 <> 0) Or (@Per = 1.00 Or @Per = 4.00 Or @Per = 12.50)  
	Begin
		IF (@per = 4.00)		
			INSERT INTO #APVAT(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) 
			VALUES (2,'B',0,@AMTA1,@AMTB1,@AMTB1,0,'',@PER,'')
		ELSE IF (@per = 12.50)
			INSERT INTO #APVAT(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) 
			VALUES (2,'C',0,@AMTA1,@AMTB1,@AMTB1,0,'',@PER,'')
		ELSE IF (@per = 1.00)
			INSERT INTO #APVAT(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) 
			VALUES (2,'A',0,@AMTA1,@AMTB1,@AMTB1,0,'',@PER,'')
	end
  -- bug-23933 - end here 
  FETCH NEXT FROM CUR_FORM221 INTO @PER
 END
 CLOSE CUR_FORM221
 DEALLOCATE CUR_FORM221

---Purchase exempted 4% Rate (New Formula)
SELECT @AMTA1 = 0,@AMTA2 = 0,@PER = 0,@AMTF1 = 0,@AMTF2 = 0
IF EXISTS (SELECT u_imporm FROM VATTBL A WHERE A.ST_TYPE='LOCAL' AND A.BHENT IN('PT','EP','SR','CN','PR')
AND A.s_tax <> '' AND (DATE BETWEEN @SDATE AND @EDATE) AND A.u_imporm = 'Exempt Transaction')
BEGIN
	SELECT @AMTA1 = ISNULL(SUM(B.Specific_inp),0),@AMTB1 = ISNULL(SUM(B.Common_inp),0) FROM
	(SELECT CASE WHEN IT.U_GVTYPE = 'Specific Input' THEN CASE WHEN A.BHENT = 'PR' THEN -(ISNULL(SUM(A.TAXAMT),0)) ELSE
	ISNULL(SUM(A.TAXAMT),0) END ELSE 0 END as Specific_inp,
	CASE WHEN IT.U_GVTYPE = 'Common Input' THEN CASE WHEN A.BHENT = 'PR' THEN -(ISNULL(SUM(A.TAXAMT),0)) ELSE
	ISNULL(SUM(A.TAXAMT),0) END ELSE 0 END as Common_inp
	FROM VATTBL A INNER JOIN IT_MAST IT ON (A.It_code = It.it_code) WHERE A.ST_TYPE='LOCAL' AND A.BHENT IN('PT','EP','SR','CN','PR')
	AND A.PER=12.5 AND A.Tax_Name like '%VAT%' and A.s_tax <> '' AND (DATE BETWEEN @SDATE AND @EDATE)
	AND A.u_imporm <> 'Exempt Transaction' GROUP BY A.Bhent,IT.U_GVTYPE) B
END

SET @AMTF1 = @AMTA1 * 4  / 12.5
SET @AMTF2 = @AMTB1 * 4  / 12.5
INSERT INTO #APVAT(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) 
VALUES (2,'CA',0,@AMTF1,@AMTF2,@AMTF2,0,'',0,'')
--INSERT INTO #APVAT (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (2,'CA',@AMTA1,0,0,0,@AMTA2,'',@PER,'')

---Purchase exempted 12.5% & 8.5% VAT Rate (New Formula)
SELECT @AMTF1 = 0,@AMTF2 = 0
SET @AMTF1 = @AMTA1 * 8.5  / 12.5
SET @AMTF2 = @AMTB1 * 8.5  / 12.5
INSERT INTO #APVAT(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) 
VALUES (2,'CB',0,@AMTF1,@AMTF2,@AMTF2,0,'',0,'')
--Set @AMTA1 = 0
--Set @AMTA2 = 0
--Set @PER = 0
--INSERT INTO #APVAT (Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (2,'CB',0,@AMTA1,0,@AMTA2,0,'',@PER,'')

--<- PART2

Select * from #APVAT order by Part,SrNo1

DROP TABLE #APVAT 
---Print 'AP VAT FORM 200E'
