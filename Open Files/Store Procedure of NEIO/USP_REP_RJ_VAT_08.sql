If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_RJ_VAT_08')
Begin
	Drop Procedure USP_REP_RJ_VAT_08
End
GO

-- =============================================
-- Author:		Nilesh Yadav.
-- Create date: 20-02-2015
-- Description:	This Stored procedure is useful to Generate Rajasthan VAT FORM 08
-- Modify date: 
-- Modified By: 
-- Modify date: 
-- Remark:    : 
-- EXECUTE USP_REP_RJ_VAT_08 '','','','04/01/2014','03/31/2015','','','','',0,0,'','','','','','','','','2014-2015',''
-- ============================================= 

CREATE Procedure [dbo].[USP_REP_RJ_VAT_08]
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
DECLARE @PER NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2),@ITEMTYPE VARCHAR(1)

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

--------------------------TEMPORARY TABLE CREATE START---------------------------
SELECT PART=3,PARTSR='AAA',SRNO='AAA',a.inv_no,m.date,AMT1=NET_AMT,AMT2=M.TAXAMT,VATONAMT=M.TAXAMT,TAXAMT=M.TAXAMT,ITEMNAME=SPACE(50),Schedule=space(100),
AGAINSTC=SPACE(100),AGAINSTCTAX=SPACE(100),AGAINSTWC=SPACE(100),AGAINSTWCTAX=SPACE(100),TOTTAX=SPACE(100),
PARTY_NM=AC1.AC_NAME,ITEMTYPE=SPACE(1),AC1.S_TAX
INTO #FORMRJ07A
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
--------------------------TEMPORARY TABLE CREATE END---------------------------



-------------------------CHECKING MULTICOMPANY START-------------------------------
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
		SET @SQLCOMMAND='Insert InTo VATTBL Select * from '+@MCON
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
-------------------------CHECKING MULTICOMPANY END-------------------------------


------------------------------------------PART I START-----------------------------------------------------------------------


SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
declare @inv_no as varchar(10),@DATE as smalldatetime ,@Party_nm as varchar(50),@ITEMNAME as varchar(50),@S_TAX as varchar(30),@Schedule as varchar(30),@Amt1 as numeric(18,4),@Amt2 as numeric(18,4),@VATonamt as numeric(18,4),@TAXAMT as numeric(18,4)
Select @inv_no='',@DATE='',@Party_nm='',@ITEMNAME='',@S_TAX='',@Schedule='',@AMT1=0, @AMT2=0 ,@VATONAMT=0,@TAXAMT=0
Declare cur_formRJ07Aaa cursor for
SELECT inv_no=a.inv_no,date=a.date,party_nm=a.ac_name,s_tax=a.s_tax,itemname=it.it_name,schedule=it.U_SHCODE,
amt1=(case when a.u_imporm='Exempt Transaction' then a.gro_amt else 0 end),
amt2=(case when a.u_imporm='Branch Transfer' then a.gro_amt else 0 end),vatonamt=a.vatonamt,taxamt=a.taxamt
FROM VATTBL A
inner join  it_mast it on (it.it_code=a.it_code)
WHERE  A.BHENT='ST'  AND (A.DATE BETWEEN @SDATE AND @EDATE) AND A.ST_TYPE in ('LOCAL','') and a.S_tax<>'' 


OPEN CUR_FORMRJ07Aaa
FETCH NEXT FROM CUR_FORMRJ07Aaa INTO @inv_no,@DATE,@Party_nm,@S_TAX,@itemname,@Schedule,@AMT1,@AMT2,@VATONAMT,@TAXAMT
WHILE (@@FETCH_STATUS=0)
BEGIN

	SET @inv_no=CASE WHEN @inv_no IS NULL THEN '' ELSE @inv_no END
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
    SET @itemname=CASE WHEN @itemname IS NULL THEN '' ELSE @itemname END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @schedule=CASE WHEN @schedule IS NULL THEN '' ELSE @schedule END
	SET @AMT1=CASE WHEN @AMT1 IS NULL THEN 0 ELSE @AMT1 END
	SET @AMT2=CASE WHEN @AMT2 IS NULL THEN 0 ELSE @AMT2 END
	SET @VATONAMT=CASE WHEN @VATONAMT IS NULL THEN 0 ELSE @VATONAMT END
	
	

	INSERT INTO #FORMRJ07A (PART,PARTSR,SRNO,INV_NO,DATE,PARTY_NM,S_TAX,ITEMNAME,Schedule,AMT1,AMT2,VATONAMT,TAXAMT) VALUES(1,'1',CHAR(@CHAR),@INV_NO,@DATE,@PARTY_NM,@S_TAX,@ITEMNAME,@Schedule,@AMT1,@AMT2,@VATONAMT,@TAXAMT)
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORMRJ07Aaa INTO @inv_no,@DATE,@Party_nm,@S_TAX,@itemname,@Schedule,@AMT1,@AMT2,@VATONAMT,@TAXAMT
END
CLOSE CUR_FORMRJ07Aaa
DEALLOCATE CUR_FORMRJ07Aaa

------------------------------------------PART I END-----------------------------------------------------------------------




------------------------------------------PART II START--------------------------------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
Select @inv_no='',@DATE='',@Party_nm='',@ITEMNAME='',@S_TAX='',@Schedule='',@AMT1=0, @AMT2=0 ,@VATONAMT=0,@TAXAMT=0
Declare cur_formRJ07Aaa cursor for
SELECT inv_no=a.inv_no,date=a.date,party_nm=a.ac_name,s_tax=a.s_tax,itemname=it.it_name,schedule=it.U_SHCODE,
amt1=(case when a.u_imporm='Exempt Transaction' then a.gro_amt else 0 end),
amt2=(case when a.u_imporm='Branch Transfer' then a.gro_amt else 0 end),vatonamt=a.vatonamt,taxamt=a.taxamt
FROM VATTBL A
inner join  it_mast it on (it.it_code=a.it_code)
WHERE  A.BHENT='ST'  AND (A.DATE BETWEEN @SDATE AND @EDATE) AND A.ST_TYPE in ('LOCAL','')  


OPEN CUR_FORMRJ07Aaa
FETCH NEXT FROM CUR_FORMRJ07Aaa INTO @inv_no,@DATE,@Party_nm,@S_TAX,@itemname,@Schedule,@AMT1,@AMT2,@VATONAMT,@TAXAMT
WHILE (@@FETCH_STATUS=0)
BEGIN

	SET @inv_no=CASE WHEN @inv_no IS NULL THEN '' ELSE @inv_no END
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
    SET @itemname=CASE WHEN @itemname IS NULL THEN '' ELSE @itemname END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @schedule=CASE WHEN @schedule IS NULL THEN '' ELSE @schedule END
	SET @AMT1=CASE WHEN @AMT1 IS NULL THEN 0 ELSE @AMT1 END
	SET @AMT2=CASE WHEN @AMT2 IS NULL THEN 0 ELSE @AMT2 END
	SET @VATONAMT=CASE WHEN @VATONAMT IS NULL THEN 0 ELSE @VATONAMT END

	INSERT INTO #FORMRJ07A (PART,PARTSR,SRNO,INV_NO,DATE,PARTY_NM,S_TAX,ITEMNAME,Schedule,AMT1,AMT2,VATONAMT,TAXAMT) VALUES (1,'2',CHAR(@CHAR),@INV_NO,@DATE,@PARTY_NM,@S_TAX,@ITEMNAME,@Schedule,@AMT1,@AMT2,@VATONAMT,@TAXAMT)
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORMRJ07Aaa INTO @inv_no,@DATE,@Party_nm,@S_TAX,@itemname,@Schedule,@AMT1,@AMT2,@VATONAMT,@TAXAMT
END
CLOSE CUR_FORMRJ07Aaa
DEALLOCATE CUR_FORMRJ07Aaa
------------------------------------------PART II END-----------------------------------------------------------------------



------------------------------------------PART III START--------------------------------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
declare @AGAINSTC as varchar(50),@AGAINSTCTAX as varchar(50), @AGAINSTWC as varchar(50), @AGAINSTWCTAX as varchar(50),@TOTTAX as varchar(50)
Select @inv_no='',@DATE='',@Party_nm='',@ITEMNAME='',@S_TAX='',@Schedule='',@AMT1=0, @AMT2=0 ,@VATONAMT=0,@AGAINSTC='',@AGAINSTCTAX='',@AGAINSTWC='',@AGAINSTWCTAX='',@TOTTAX=''

Declare cur_formRJ07Aaa cursor for
SELECT inv_no=a.inv_no,date=a.date,party_nm=a.ac_name,s_tax=a.s_tax,itemname=it.it_name,schedule=it.U_SHCODE,vatonamt=a.vatonamt,
amt1=(case when a.u_imporm in ('Branch Transfer','Stock Transfer','Depot Transfer') then a.gro_amt else 0 end),
amt2=(case when a.u_imporm='Exempt Transaction' then a.gro_amt else 0 end),
AGAINSTC=(case when a.rform_nm in ('C','Form C','C Form') then a.vatonamt else 0 end),
AGAINSTCTAX=(case when a.rform_nm in ('C','Form C','C Form') then a.taxamt else 0 end),
AGAINSTWC=(case when a.rform_nm not in ('C','Form C','C Form') then a.vatonamt else 0 end),
AGAINSTWCTAX=(case when a.rform_nm not in ('C','Form C','C Form') then a.taxamt else 0 end),
TOTTAX=((case when a.rform_nm in ('C','Form C','C Form') then a.taxamt else 0 end)+(case when a.rform_nm not in ('C','Form C','C Form') then a.taxamt else 0 end))
FROM VATTBL A
inner join  it_mast it on (it.it_code=a.it_code)
WHERE  A.BHENT='ST'  AND (A.DATE BETWEEN @SDATE AND @EDATE) AND A.ST_TYPE in ('OUT OF STATE','OUT OF COUNTRY')  


OPEN CUR_FORMRJ07Aaa
FETCH NEXT FROM CUR_FORMRJ07Aaa INTO @inv_no,@DATE,@Party_nm,@S_TAX,@itemname,@Schedule,@VATONAMT,@AMT1,@AMT2,@AGAINSTC,@AGAINSTCTAX,@AGAINSTWC,@AGAINSTWCTAX,@TOTTAX
WHILE (@@FETCH_STATUS=0)
BEGIN

	SET @inv_no=CASE WHEN @inv_no IS NULL THEN '' ELSE @inv_no END
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
    SET @itemname=CASE WHEN @itemname IS NULL THEN '' ELSE @itemname END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @schedule=CASE WHEN @schedule IS NULL THEN '' ELSE @schedule END
	SET @AMT1=CASE WHEN @AMT1 IS NULL THEN 0 ELSE @AMT1 END
	SET @AMT2=CASE WHEN @AMT2 IS NULL THEN 0 ELSE @AMT2 END
	SET @VATONAMT=CASE WHEN @VATONAMT IS NULL THEN 0 ELSE @VATONAMT END

	INSERT INTO #FORMRJ07A (PART,PARTSR,SRNO,INV_NO,DATE,PARTY_NM,S_TAX,ITEMNAME,Schedule,AMT1,AMT2,VATONAMT,AGAINSTC,AGAINSTCTAX,AGAINSTWC,AGAINSTWCTAX,TOTTAX) VALUES (1,'3',CHAR(@CHAR),@INV_NO,@DATE,@PARTY_NM,@S_TAX,@ITEMNAME,@Schedule,@AMT1,@AMT2,@VATONAMT,@AGAINSTC,@AGAINSTCTAX,@AGAINSTWC,@AGAINSTWCTAX,@TOTTAX)
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORMRJ07Aaa INTO @inv_no,@DATE,@Party_nm,@S_TAX,@itemname,@Schedule,@VATONAMT,@AMT1,@AMT2,@AGAINSTC,@AGAINSTCTAX,@AGAINSTWC,@AGAINSTWCTAX,@TOTTAX
END
CLOSE CUR_FORMRJ07Aaa
DEALLOCATE CUR_FORMRJ07Aaa

------------------------------------------PART III END----------------------------------------------------------------------




DECLARE @nCount AS NUMERIC(4)

----------------------------------------- PART-I CHECK (NULL VALUES)--------------------------------------------------------
SET @nCount = 0

Select @nCount=COUNT(PARTSR) From #FORMRJ07A Where PARTSR = '1'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END

IF @nCount = 0
	INSERT INTO #FORMRJ07A
	    (PART,PARTSR,SRNO,INV_NO,DATE,PARTY_NM,S_TAX,ITEMNAME,Schedule,AMT1,AMT2,VATONAMT,TAXAMT)
	    VALUES
	    (1,'1',        '',  ''  ,'',    '',      '',    '' ,    '', 0,   0,0,0)


----------------------------------------- PART-II CHECK (NULL VALUES)-------------------------------------------------------
SET @nCount = 0

Select @nCount=COUNT(PARTSR) From #FORMRJ07A Where PARTSR = '2'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END

IF @nCount = 0
	INSERT INTO #FORMRJ07A
	    (PART,PARTSR,SRNO,INV_NO,DATE,PARTY_NM,S_TAX,ITEMNAME,Schedule,AMT1,AMT2,VATONAMT,TAXAMT)
	    VALUES
	     (1,'2','', '', '', '', '', '', '',  0,0,0,0)
	     
	    
----------------------------------------- PART-III CHECK (NULL VALUES)-------------------------------------------------------

SET @nCount = 0

Select @nCount=COUNT(PARTSR) From #FORMRJ07A Where PARTSR = '3'
SET @nCount=CASE WHEN @nCount IS NULL THEN 0 ELSE @nCount END

IF @nCount = 0
	INSERT INTO #FORMRJ07A
	    (PART,PARTSR,SRNO,INV_NO,DATE,PARTY_NM,S_TAX,ITEMNAME,Schedule,AMT1,AMT2,VATONAMT,AGAINSTC,AGAINSTCTAX,AGAINSTWC,AGAINSTWCTAX,TOTTAX)
	    VALUES
	     (1,'3','',         '',    '', '',      '',    '',       '',     0,  0,   0,'','','','','')
-----------------------------------------------------------------------------------------------------------------------------


Update #FORMRJ07A set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		               inv_no=isnull(inv_no,''),DATE=isnull(date,''),PARTY_NM = isnull(Party_nm,''),S_TAX = isnull(S_TAX,''),
		               ITEMNAME = isnull(ITEMNAME,'') ,Schedule = isnull(Schedule,''),  AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0),
		               vatonamt = isnull(vatonamt,0) , TAXAMT = isnull(taxamt,0)       
		             
		

			
SELECT * FROM #FORMRJ07A order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
END


----------------------------------------------------END----------------------------------------------------------------------