If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_AP_CST_VI')
Begin
	Drop Procedure USP_REP_AP_CST_VI
End
GO
-- =============================================
 -- Author:  Hetal Patel
 -- Create date: 01/12/2009
 -- Description: This Stored procedure is useful to generate AP CST FORM VI
 -- Modified By: Madhavi Penumalli
 -- Modify date: 27/11/2009 Updated
 -- Modified By: Changes has been done by Gaurav Tanna on 07/10/2014 for BUG-23933
 
 -- =============================================
CREATE Procedure [dbo].[USP_REP_AP_CST_VI]
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
INTO #FORM221_1
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #form221_1 add recno int identity
*/
-- bug-23933 - end here

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=9999999.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
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

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		
		-- bug-23933 - start here
		--SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
		SET @SQLCOMMAND='Insert InTo  vattbl Select * from '+@MCON
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
		SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
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


 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
---1

 ----Select * From #Form221_1 where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of State' and U_Imporm <> 'Branch Transfer' and Tax_name not in('CST 2 %','C.S.T. 2 %')

/*
---Gross Sales

--10
 Select @AMTA1=Round(Sum(GRO_AMT),0) From vattbl where (date between @sdate and @edate) and bhent = 'ST'
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'10','A',0,@AMTA1,0,0,'')
*/
--1
 /*
 Select @AMTA1=Round(Sum(AMT1),0) From #Form221 where PartSr = '8' And SrNo = 'C'
 Select @AMTA2=Round(Sum(AMT1),0) From #Form221 where PartSr = '9' And SrNo In('A')
  --Set @AMTA2 = ISNULL(@AMTA2,0)
 */ 
 Select @AMTA1=Sum(GRO_AMT) From vattbl where (date between @sdate and @edate) and bhent = 'ST' And tax_name like ('C.S.T. %')
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'1','A',0,@AMTA1,0,0,'')

--2
--LESS : CST Collections
Select @AMTA1=Sum(TAXAMT) From vattbl where (date between @sdate and @edate) and bhent = 'ST' And tax_name like ('C.S.T. %')	
Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'2','B',0,@AMTA1,0,0,'')
  
--3  
--Balance A - B
 Select @AMTA1=Sum(AMT1) From #Form221 where PartSr = '1' And SrNo = 'A'
 Select @AMTA2=Sum(AMT1) From #Form221 where PartSr = '2' And SrNo In('B')
 Set @AMTA1 = ISNULL(@AMTA1,0)
 Set @AMTA2 = ISNULL(@AMTA2,0)
INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'3','C',0,@AMTA1-@AMTA2,0,0,'')


Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),
	    @DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),
		@FORM_NM as varchar(30) ,@S_TAX as varchar(30)

SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@FORM_NM='',@S_TAX =''


--4
---Tax & Taxable Amount of Sales for the period
 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 SET @CHAR=65
 
 DECLARE  CUR_FORM221 CURSOR FOR 
 select distinct level1 from stax_mas where ST_TYPE='OUT OF STATE' And Tax_name not like '%VAT%'
 
 
 OPEN CUR_FORM221
 FETCH NEXT FROM CUR_FORM221 INTO @PER
 WHILE (@@FETCH_STATUS=0)
 BEGIN
	-- bug-23933 - (instead of FROM #FORM221_1, changed it to FROM VATTBL)  
	begin
		SELECT @AMTA1=SUM(GRO_AMT) FROM vattbl where bhent = 'ST' AND ST_TYPE = 'OUT OF STATE' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER 
		SELECT @AMTB1=SUM(TAXAMT)  FROM vattbl where bhent = 'ST' AND ST_TYPE = 'OUT OF STATE' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER 
		--SELECT @AMTC1=Round(SUM(GRO_AMT),0) FROM vattbl where bhent = 'SR' AND ST_TYPE = 'OUT OF STATE' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER
		--SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM vattbl where bhent = 'SR' AND ST_TYPE = 'OUT OF STATE' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER
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

  /*
  if @nettax <> 0
	  begin
	  
		  INSERT INTO #FORM221
		  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		  (1,'3',CHAR(@CHAR),@PER,@NETEFF,@NETTAX,0,'')
		  
		  SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
		  SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX
		  SET @CHAR=@CHAR+1
	  end
  */
  
  SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
  SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX
  SET @CHAR=@CHAR+1
  
  Begin
	 IF (@per = 1.00)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','A',@PER,@NETEFF,@NETTAX,0,'')
	 ELSE IF (@per = 2.00)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','B',@PER,@NETEFF,@NETTAX,0,'')
     ELSE IF (@per = 3.00)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','C',@PER,@NETEFF,@NETTAX,0,'')		
	ELSE IF (@per = 4.00)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','D',@PER,@NETEFF,@NETTAX,0,'')		
	ELSE IF (@per = 10.00)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','E',@PER,@NETEFF,@NETTAX,0,'')		
	ELSE IF (@per = 12.50)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','F',@PER,@NETEFF,@NETTAX,0,'')		
	ELSE IF (@per = 14.50)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','G',@PER,@NETEFF,@NETTAX,0,'')		
	ELSE 
		IF (@per <> 0) And (@NETTAX <> 0)
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
		(1,'4','H',@PER,@NETEFF,@NETTAX,0,'')		
  end
    
  FETCH NEXT FROM CUR_FORM221 INTO @PER
 END
 CLOSE CUR_FORM221
 DEALLOCATE CUR_FORM221

---Total of Tax & Taxable Amount of Sales for the period
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'4','Z',0,@AMTJ1,@AMTK1,0,'')
 
--5
 /*
 Select @AMTA1=Round(Sum(A.Net_Amt),0),@Party_nm=A.Party_nm,@Inv_no=A.Inv_no,@Date=A.Date From vattbl A
 Inner Join BpMain B On(A.Bhent = B.Entry_ty And A.Tran_cd = B.Tran_cd)
 Where A.Bhent = 'BP' And A.Date Between @SDate And @EDate And B.Party_nm not like '%VAT%'
 Group by A.party_nm,A.inv_no,A.Date 
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,Inv_no,Date) VALUES
 (1,'5','1',0,@AMTA1,0,0,@Party_nm,@Inv_no,@Date)
 */
  
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm,Inv_no,Date)
 Select 1, '5', '1', 0, A.Net_Amt,0,0,B.bank_nm,A.Inv_no,A.Date From vattbl A
 Inner Join BpMain B On(A.Bhent = B.Entry_ty And A.Tran_cd = B.Tran_cd)
 Where A.Bhent = 'BP' And A.Date Between @SDate And @EDate And B.Party_nm = 'CST PAYABLE' 
 -- bug-23933 - end here
 
 
--6
--Sales Of Goods OutSide State (Agent/Principal) Otherwise than by way of inter state sale
  --Select @AMTA1=Round(Sum(NET_AMT),0) From #Form221_1 where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of State' and U_Imporm <> 'Branch Transfer' and Tax_name not in('CST 2%','C.S.T. 2%')
 Select @AMTA1=Sum(GRO_AMT) From vattbl where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of State' and U_Imporm = '' and Tax_name not like('C.S.T.%')
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'6','A',0,@AMTA1,0,0,'')

---Branch Transfer
 --Select @AMTA1=Round(Sum(NET_AMT),0) From #Form221_1 where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of State' and u_Imporm = 'Branch Transfer'
 Select @AMTA1=Sum(GRO_AMT) From VATTBL where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of State' and u_Imporm in ('Branch Transfer', 'Consignment Transfer') 
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'6','B',0,@AMTA1,0,0,'')
 
 ---Balance C = (A+B)
 Select @AMTA1=Sum(AMT1) From #Form221 where (PartSr = '6' And SrNo = 'A')
 Select @AMTA2=Sum(AMT1) From #Form221 where (PartSr = '6' And SrNo = 'B')
 Set @AMTA1 = ISNULL(@AMTA1,0)
 Set @AMTA2 = ISNULL(@AMTA2,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'6','C',0,@AMTA1+@AMTA2,0,0,'')

--7
--Export Sales
 --Select @AMTA1=Round(Sum(NET_AMT),0) From #Form221_1 where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of Country' and u_Imporm <> 'Branch Transfer'
 Select @AMTA1=Sum(GRO_AMT) From VATTBL where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Out Of Country' and u_Imporm <> 'Branch Transfer'
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'7','A',0,@AMTA1,0,0,'')

/*
---Balance A - (B-C-D-E-F)
--8
 Select @AMTA1=Round(Sum(AMT1),0) From #Form221 where Part = 1 And PartSr = '10' And SrNo = 'A'
 Select @AMTA2=Round(Sum(AMT1),0) From #Form221 where ((PartSr = '6' And SrNo = 'C') Or PartSr = '7')
 Set @AMTA1 = ISNULL(@AMTA1,0)
 Set @AMTA2 = ISNULL(@AMTA2,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'8','A',0,@AMTA1-@AMTA2,0,0,'')

---LESS : Sales Local
 -- bug-23933 - start here
 Select @AMTA1=Round(Sum(GRO_AMT),0) From vattbl where (date between @sdate and @edate) and bhent = 'ST' And St_Type = 'Local' and u_Imporm <> 'Branch Transfer'
 -- bug-23933 - end here
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'8','B',0,@AMTA1,0,0,'')

---Balance G - (H)
 Select @AMTA1=Round(Sum(AMT1),0) From #Form221 where PartSr = '8' And SrNo = 'A'
 Select @AMTA2=Round(Sum(AMT1),0) From #Form221 where PartSr = '8' And SrNo In('B')
 Set @AMTA1 = ISNULL(@AMTA1,0)
 Set @AMTA2 = ISNULL(@AMTA2,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'8','C',0,@AMTA1-@AMTA2,0,0,'')

---Sales Exempted
--9
 --Select @AMTA1=Round(Sum(AMT1),0) From #Form221 where Part = 1 And PartSr = '10' And SrNo = 'A'
 --Select @AMTA2=Round(Sum(AMT1),0) From #Form221 where ((PartSr = '6' And SrNo = 'C') Or PartSr = '7')

 */
 
 Set @AMTA1 = ISNULL(@AMTA1,0)
 Select @AMTA1=Sum(GRO_AMT) From vattbl where (date between @sdate and @edate) and bhent = 'ST' And Tax_name in ('E - 1', 'E - 2') And St_Type = 'Out Of State'
 
 Set @AMTA1 = ISNULL(@AMTA1,0)
 INSERT INTO #FORM221
 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
 (1,'9','A',0,@AMTA1,0,0,'')

 
 Update #form221 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),  ITEM =isnull(item,''),

 SELECT * FROM #FORM221 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int), partsr,SRNO
 
 END

set ANSI_NULLS OFF
