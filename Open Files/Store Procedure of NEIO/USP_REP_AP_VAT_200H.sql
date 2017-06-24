If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_AP_VAT_200H')
Begin
	Drop Procedure USP_REP_AP_VAT_200H
End
GO
-- =============================================
-- Author:		Hetal L. Patel
-- Create date: 01/06/2009	
-- Description:	This Stored procedure is useful to generate AP VAT FORM 200H.
-- Modify date: 24/06/2009
-- Modified By: Dinesh 
-- Modified By: Changes has been done by Gaurav Tanna on 07/10/2014 for BUG-23933
-- Remark:
-- =============================================
CREATE Procedure [dbo].[USP_REP_AP_VAT_200H]
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


SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
-- bug-23933 - start here
/*
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #FORM200_H
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #FORM200_H add recno int identity
*/
-- bug-23933 - start here

---Temporary Cursor2
DECLARE @TAXABLE_AMT NUMERIC(14,2),@taxamt numeric(14,2)
set @FCON=rtrim(@FCON)
select Part=1,SrNo1='AA',Taxable_amt=i.gro_amt
,taxamt1=i.taxamt
,taxamt2=i.taxamt
,taxamt3=i.taxamt
,taxamt4=i.taxamt
,st.tax_name,st.level1,st.st_type
into #APVAT
from ptitem i  
inner join stax_mas st on (st.tax_name=i.tax_name)
where 1=2
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
		--SET @SQLCOMMAND='Insert InTo #FORM200_H Select * from '+@MCON
		SET @SQLCOMMAND='Insert InTo VATTBL Select * from '+@MCON
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
		 EXECUTE USP_REP_SINGLE_CO_DATA_VAT
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		/*
		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo #FORM200_H Select * from '+@MCON
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

Declare @AMTA1 As Numeric(12,2),@AMTA2 As Numeric(12,2),@AMTB1 As Numeric(12,2),@AMTB2 As Numeric(12,2),
	    @AMTC1 As Numeric(12,2),@AMTC2 As Numeric(12,2),@PER As numeric (5,2)

Select @AMTA1=0,@AMTA2=0,@AMTB1=0,@AMTB2=0,@AMTC1=0,@AMTC2=0,@per=0


---Total of 13A,14A,16A,17A,19A of VAT Form 200
-- bug-23933 - start here
/*
SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_H WHERE (DATE BETWEEN @SDATE AND @EDATE) and st_type = 'Out of Country' and BHENT = 'ST'
SELECT @AMTA2=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_H WHERE (st_type = 'Out of State') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTB1=SUM(NET_AMT)-Sum(TaxAmt), @Per = Per FROM #FORM200_H WHERE St_Type = 'Local' and (BHENT='ST') and Per = 4 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTB2=SUM(NET_AMT)-Sum(TaxAmt), @Per = Per FROM #FORM200_H WHERE St_Type = 'Local' and (BHENT='ST') and Per = 12.50 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTC1=SUM(NET_AMT)-Sum(TaxAmt), @Per = Per FROM #FORM200_H WHERE St_Type = 'Local' and (BHENT='ST') and Per = 1 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
*/
SELECT @AMTA1=SUM(GRO_AMT)-Sum(TaxAmt) FROM VATTBL WHERE (DATE BETWEEN @SDATE AND @EDATE) and st_type = 'Out of Country' and BHENT = 'ST' and Per = 0
SELECT @AMTA2=SUM(GRO_AMT)-Sum(TaxAmt) FROM VATTBL WHERE (st_type = 'Out of State') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Per = 0
SELECT @AMTB1=SUM(GRO_AMT)-Sum(TaxAmt), @Per = Per FROM VATTBL WHERE St_Type = 'Local' and Tax_Name like '%VAT%' and (BHENT In ('ST', 'PR', 'DN'))  and Per = 4 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTB2=SUM(GRO_AMT)-Sum(TaxAmt), @Per = Per FROM VATTBL WHERE St_Type = 'Local' and Tax_Name like '%VAT%'  and (BHENT In ('ST', 'PR', 'DN'))  and Per = 12.50 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
SELECT @AMTC1=SUM(GRO_AMT)-Sum(TaxAmt), @Per = Per FROM VATTBL WHERE St_Type = 'Local' and Tax_Name like '%VAT%'  and (BHENT In ('ST', 'PR', 'DN'))  and Per = 1 AND (DATE BETWEEN @SDATE AND @EDATE) group by Per
-- bug-23933 - end here

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
SET @AMTB2=CASE WHEN @AMTB2 IS NULL THEN 0 ELSE @AMTB2 END
SET @AMTC1=CASE WHEN @AMTC1 IS NULL THEN 0 ELSE @AMTC1 END
set @AMTC2 = @AMTA1 + @AMTA2 + @AMTB1 + @AMTB2 + @AMTC1
SET @AMTC2=CASE WHEN @AMTC2 IS NULL THEN 0 ELSE @AMTC2 END

INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'A',@AMTC2,0,0,0,0,'',0,'')

---Amount of sales of exempt goods in the period
Set @AMTA1 = 0
-- bug-23933 - start here
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_H  WHERE (st_type = 'Out of Country') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM VATTBL  WHERE (st_type = 'Out of Country') AND BHENT='ST' AND (DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTA1= @AMTA1 + CASE WHEN LCODE.STAX_ITEM = 1 THEN (round(STITEM.u_asseamt+STITEM.EXAMT+STITEM.U_CESSAMT+STITEM.U_HCESAMT+STITEM.tot_add+STITEM.TOT_DEDUC,2)) else round(((STITEM.gro_amt+STITEM.tot_add+STITEM.tot_tax)-STITEM.tot_deduc),2) end
FROM STITEM INNER JOIN IT_MAST ON (STITEM.It_code = IT_MAST.It_code)
INNER JOIN LCODE ON (LCODE.ENTRY_TY=STITEM.ENTRY_TY)
WHERE IT_MAST.U_GVTYPE like 'Tax Free Sch - I goods' AND (STITEM.DATE BETWEEN @SDATE AND @EDATE)
-- bug-23933 - end here
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'B',@AMTA1,0,0,0,0,'',0,'')

---Amount of exempt transactions in the period
Set @AMTA1 = 0
Set @AMTA2 = 0
-- bug-23933 - start here
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM #FORM200_H WHERE (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name = 'Exempted' and BHENT = 'ST'
--SELECT @AMTA1=SUM(NET_AMT)-Sum(TaxAmt) FROM VATTBL WHERE (DATE BETWEEN @SDATE AND @EDATE) and Tax_Name = 'Exempted' and BHENT = 'ST'
SELECT @AMTA1= Sum(round(((STMAIN.gro_amt+STMAIN.tot_add+STMAIN.tot_tax)-STMAIN.tot_deduc),2))
FROM STMAIN WHERE stmain.u_imporm = 'Exempt Transaction' AND (STMAIN.DATE BETWEEN @SDATE AND @EDATE)
-- bug-23933 - end here
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END

INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (1,'C',@AMTA1,0,0,0,0,'',0,'')
--<- PART1


-->- PART2
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (2,'A',0,0,0,0,0,'',0,'')

--<- PART2

-->- PART3
INSERT INTO #APVAT 
(Part,SrNo1,Taxable_Amt,taxamt1,taxamt2,taxamt3,taxamt4,tax_name,level1,st_type) VALUES (3,'A',0,0,0,0,0,'',0,'')
--<- PART3

Select * from #APVAT order by Part,SrNo1

DROP TABLE #APVAT 
--Print 'AP VAT FORM 200H'
--GO
-------
----Print 'AP Stored Procedure Updation Completed'
--GO
