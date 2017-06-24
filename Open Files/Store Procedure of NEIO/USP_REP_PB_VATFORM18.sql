If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_PB_VATFORM18')
Begin
	Drop Procedure USP_REP_PB_VATFORM18
End
GO


-- =============================================
-- Author:		Prashanth Reddy.
-- Create date: 22/09/2012
-- Description:	This Stored procedure is useful to generate BP VAT FORM 18
-- Modify date: 
-- Modified By: Nilesh Yadav
-- Modify date: 03/02/15
-- Modified By: GAURAV R. TANNA
-- Modify date: 30/06/2015
-- Modified By: GAURAV R. TANNA
-- Modify date: 30/07/2015
-- Remark:    : 
-- EXECUTE USP_REP_PB_VATFORM18 '','','','04/01/2014','03/31/2015','','','','',0,0,'','','','','','','','','2014-2015',''
-- =============================================


Create PROCEDURE [dbo].[USP_REP_PB_VATFORM18]
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
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2),@ITEMTYPE VARCHAR(1)

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)


SELECT PART=3,PARTSR='AAA',SRNO='AAA',tran_type=space(100),party_nm=RTRIM(ac.MAILNAME),ADDRESS=RTRIM(LTRIM(AC.STATE)),COUNTRY=RTRIM(LTRIM(AC.COUNTRY))	
 ,AC.S_TAX,BILLNO=M.U_PINVNO,BILLDT=M.U_PINVDT,ITDESC=B.ITEM
,QTY=b.qty
,AMT1=B.GRO_AMT
,AMT2=B.GRO_AMT
,AMT3=B.GRO_AMT
,Grno=M.U_lrno
,Grdt=M.U_lrdt 
,NTRAN=M.U_TRANSNM
,TR_NAT=SPACE(100)
,LCS=SPACE(100)
INTO #FORMPB07A
FROM STACDET A 
inner join VATITEM_VW B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD)
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME and AC.AC_ID=M.AC_ID)
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
	 
	   PRINT'SB555'
End
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 




SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65



Declare @VATONAMT as numeric(12,2),@INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4)
Declare @billno as varchar (15),@billdt as smalldatetime,@itdesc as varchar(50),@recformno as varchar(10),@recformdt as smalldatetime,@grno as varchar(30),@grdt as smalldatetime,@SRNO_OF_VAT36 as varchar(10),@NTRAN as varchar(30)
Declare @U_IMPORM AS VARCHAR(100), @TAX_NAME AS VARCHAR(50), @TRAN_TYPE AS VARCHAR(100), @ST_TYPE As VarChar(50), @country As VARCHAR(60)
Declare @RFORM_NM VARCHAR(50), @TR_NAT VARCHAR(100), @BHENT VARCHAR(03)

SELECT @VATONAMT=0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@U_IMPORM ='', @TAX_NAME='', @TRAN_TYPE='', @ST_TYPE = ''
,@FORM_NM='',@S_TAX ='',@QTY=0,@ITEMTYPE='',@billno='',@billdt='',@itdesc ='',@recformno='',@recformdt ='',@grno ='',@grdt ='',@SRNO_OF_VAT36 ='',@NTRAN=''
,@COUNTRY = '', @RFORM_NM = '',@TR_NAT='', @BHENT = ''


Declare cur_formPB07Aaa cursor for

SELECT 
S.TRAN_CD,
S.ENTRY_TY,
A.U_IMPORM,
A.TAX_NAME,
AC.ST_TYPE,
AC.COUNTRY,
A.RFORM_NM,
PARTY_NM=RTRIM(ac.MAILNAME),
ADDRESS=RTRIM(LTRIM(AC.STATE)),
AC.S_TAX,
BILLNO=A.inv_no,
BILLDT=A.DATE,
IT.IT_NAME,
ITDESC=IT.U_TRDNM--CAST(IT.U_TRDNM AS VARCHAR(150))
,QTY=Sum(b.qty),
 AMT1=Sum(A.VATONAMT+B.U_FRTAMT),
 AMT2=Sum(B.U_FRTAMT),
 AMT3=0, --FOR ENTRY TAX
 Grno=case when S.U_lrno is null then '' else S.U_lrno end 
,Grdt=case when S.U_lrdt is null then ' ' else S.U_lrdt end,NTRAN=RTRIM(S.U_TRANSNM),
LCS=S.U_LCS
FROM vattbl A
INNER JOIN  stitem B ON (A.TRAN_cD=B.TRAN_cD AND A.BHENT=B.ENTRY_tY and a.itserial=b.itserial)
inner join stmain S on (s.tran_cd=B.tran_cd and s.ac_id=b.ac_id )
INNER JOIN AC_MAST AC ON (S.AC_ID = AC.AC_ID)
INNER JOIN IT_MAST IT ON (IT.IT_CODE = B.IT_CODE)
WHERE  A.BHENT ='ST' AND  AC.ST_TYPE IN ('OUT OF STATE','OUT OF COUNTRY') AND (A.DATE BETWEEN @SDATE AND @EDATE)  AND (A.U_IMPORM <> 'Purchase Return')
GROUP BY S.TRAN_CD,S.ENTRY_TY, A.U_IMPORM, A.TAX_NAME, AC.ST_TYPE, AC.COUNTRY, A.RFORM_NM, RTRIM(ac.MAILNAME),RTRIM(LTRIM(AC.STATE)),AC.S_TAX, 
A.inv_no,A.DATE,IT.IT_NAME,IT.U_TRDNM, S.U_lrno,S.U_lrdt,RTRIM(S.U_TRANSNM),S.U_LCS
UNION
SELECT 
S.TRAN_CD,
S.ENTRY_TY,
U_IMPORM='Sales Return',
A.TAX_NAME,
AC.ST_TYPE,
AC.COUNTRY,
RFORM_NM='',
PARTY_NM=RTRIM(ac.MAILNAME),
ADDRESS=RTRIM(LTRIM(AC.STATE)),
AC.S_TAX,
BILLNO=A.inv_no,
BILLDT=A.DATE,
IT.IT_NAME,
ITDESC=IT.U_TRDNM--CAST(IT.IT_DESC AS VARCHAR(150))
,QTY=Sum(b.qty),
 AMT1=Sum(A.VATONAMT+B.U_FRTAMT),
AMT2=Sum(B.U_FRTAMT),
 AMT3=Sum(B.U_ENTTAX), --FOR ENTRY TAX
 Grno=case when S.U_lrno is null then '' else S.U_lrno end 
,Grdt=case when S.U_lrdt is null then ' ' else S.U_lrdt end,NTRAN=RTRIM(S.U_TRANSNM),
LCS=S.U_LCS
FROM VATTBL A
INNER JOIN  sritem B ON (A.TRAN_cD=B.TRAN_cD AND A.BHENT=B.ENTRY_tY and a.itserial=b.itserial) 
inner join sRmain S on (s.tran_cd=B.tran_cd and s.entry_ty=b.entry_ty)
INNER JOIN AC_MAST AC ON (S.AC_ID = AC.AC_ID)
INNER JOIN IT_MAST IT ON (IT.IT_CODE = B.IT_CODE)
WHERE  A.BHENT ='SR' AND  AC.ST_TYPE IN ('OUT OF STATE','OUT OF COUNTRY') AND (A.DATE BETWEEN @SDATE AND @EDATE)
GROUP BY S.TRAN_CD,S.ENTRY_TY, A.TAX_NAME, AC.ST_TYPE, AC.COUNTRY, RTRIM(ac.MAILNAME),RTRIM(LTRIM(AC.STATE)),AC.S_TAX, 
A.inv_no,A.DATE,IT.IT_NAME,IT.U_TRDNM, S.U_lrno,S.U_lrdt,RTRIM(S.U_TRANSNM),S.U_LCS
UNION
SELECT 
S.TRAN_CD,
S.ENTRY_TY,
U_IMPORM='Repair/Job Work',
B.TAX_NAME,
AC.ST_TYPE,
AC.COUNTRY,
RFORM_NM='',
PARTY_NM=RTRIM(ac.MAILNAME),
ADDRESS=RTRIM(LTRIM(AC.STATE)),
AC.S_TAX,
BILLNO=S.inv_no,
BILLDT=S.DATE,
IT.IT_NAME,
ITDESC=IT.U_TRDNM--CAST(IT.IT_DESC AS VARCHAR(150))
,QTY=Sum(b.qty),
 AMT1=Sum(b.GRO_AMT),
AMT2=0,--Sum(B.U_FRTAMT),
AMT3=0, --FOR ENTRY TAX
 Grno=case when S.U_lrno is null then '' else S.U_lrno end 
,Grdt=case when S.U_lrdt is null then ' ' else S.U_lrdt end,NTRAN=RTRIM(S.U_TRANSNM),
LCS=S.U_LCS
FROM IIItem B 
inner join IIMain S on (s.tran_cd=B.tran_cd and s.entry_ty=b.entry_ty)
INNER JOIN AC_MAST AC ON (S.AC_ID = AC.AC_ID)
INNER JOIN IT_MAST IT ON (IT.IT_CODE = B.IT_CODE)
WHERE  B.ENTRY_TY IN ('LI', 'I5') AND  AC.ST_TYPE IN ('OUT OF STATE','OUT OF COUNTRY') AND (B.DATE BETWEEN @SDATE AND @EDATE)
GROUP BY S.TRAN_CD,S.ENTRY_TY, B.TAX_NAME, AC.ST_TYPE, AC.COUNTRY, RTRIM(ac.MAILNAME),RTRIM(LTRIM(AC.STATE)),AC.S_TAX, 
S.inv_no,S.DATE,IT.IT_NAME,IT.U_TRDNM, S.U_lrno,S.U_lrdt,RTRIM(S.U_TRANSNM),S.U_LCS
UNION
SELECT 
S.TRAN_CD,
S.ENTRY_TY,
U_IMPORM='Cancellation of Sales',
B.TAX_NAME,
AC.ST_TYPE,
AC.COUNTRY,
RFORM_NM='',
PARTY_NM=RTRIM(ac.MAILNAME),
ADDRESS=RTRIM(LTRIM(AC.STATE)),
AC.S_TAX,
BILLNO=A.inv_no,
BILLDT=A.DATE,
IT.IT_NAME,
ITDESC=IT.U_TRDNM--CAST(IT.IT_DESC AS VARCHAR(150))
,QTY=Sum(b.qty),
AMT1=Sum(A.VATONAMT),
AMT2=0,--Sum(B.U_FRTAMT),
AMT3=Sum(B.U_ENTTAX), --FOR ENTRY TAX
 Grno=case when S.U_lrno is null then '' else S.U_lrno end 
,Grdt=case when S.U_lrdt is null then ' ' else S.U_lrdt end,NTRAN=RTRIM(S.U_TRANSNM),
LCS=S.U_LCS
FROM VATTBL A 
INNER JOIN  cnitem B ON (A.TRAN_cD=B.TRAN_cD AND A.BHENT=B.ENTRY_tY and a.itserial=b.itserial) 
inner join CNMAIN S on (s.tran_cd=B.tran_cd and s.entry_ty=b.entry_ty)
INNER JOIN AC_MAST AC ON (S.AC_ID = AC.AC_ID)
INNER JOIN IT_MAST IT ON (IT.IT_CODE = B.IT_CODE)
WHERE  A.BHENT ='CN' AND (S.U_GPRICE = 'Sales Cancelled') AND  AC.ST_TYPE IN ('OUT OF STATE','OUT OF COUNTRY') AND (A.DATE BETWEEN @SDATE AND @EDATE)
GROUP BY S.TRAN_CD,S.ENTRY_TY, B.TAX_NAME, AC.ST_TYPE, AC.COUNTRY, RTRIM(ac.MAILNAME),RTRIM(LTRIM(AC.STATE)),AC.S_TAX, 
A.inv_no,A.DATE,IT.IT_NAME,IT.U_TRDNM, S.U_lrno,S.U_lrdt,RTRIM(S.U_TRANSNM),S.U_LCS
order by Billdt,billno

DECLARE @RENTRY_TY VARCHAR(10), @RTRAN_CD INT, @TRAN_CD INT, @IT_NAME VARCHAR(150), @ENTRY_TAX NUMERIC (12,2)
DECLARE @FRGHT_AMT NUMERIC (12,2), @LCS VARCHAR (40), @REF_TAX VARCHAR(30)

SELECT @RENTRY_TY ='', @RTRAN_CD=0, @TRAN_CD=0, @IT_NAME='', @ENTRY_TAX=0, @FRGHT_AMT = 0, @LCS = '', @REF_TAX = ''


OPEN CUR_FORMPB07Aaa
FETCH NEXT FROM CUR_FORMPB07Aaa INTO @TRAN_CD,@BHENT, @U_IMPORM, @TAX_NAME, @ST_TYPE, @COUNTRY, @RFORM_NM, @PARTY_NM,@ADDRESS,@S_TAX,@billno,@billdt,@IT_NAME,@itdesc,@QTY,@VATONAMT,@FRGHT_AMT,@ENTRY_TAX,@grno ,@grdt,@NTRAN,@LCS   --Added by nilesh on dt 03/02/15
WHILE (@@FETCH_STATUS=0)
BEGIN


	SET @VATONAMT=CASE WHEN @VATONAMT IS NULL THEN 0 ELSE @VATONAMT END
	SET @FRGHT_AMT=CASE WHEN @FRGHT_AMT IS NULL THEN 0 ELSE @FRGHT_AMT END
	SET @ENTRY_TAX=CASE WHEN @ENTRY_TAX IS NULL THEN 0 ELSE @ENTRY_TAX END
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END	
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @BILLNO=CASE WHEN @BILLNO IS NULL THEN '' ELSE @BILLNO END
	SET @qty=CASE WHEN @qty IS NULL THEN 0 ELSE @qty END
	SET @COUNTRY=CASE WHEN @COUNTRY IS NULL THEN '' ELSE @COUNTRY END
	SET @IT_NAME=CASE WHEN @IT_NAME IS NULL THEN '' ELSE @IT_NAME END
	SET @ITDESC=CASE WHEN @ITDESC IS NULL THEN '' ELSE @ITDESC END
	SET @LCS=CASE WHEN @LCS IS NULL THEN '' ELSE @LCS END	
	SET @TRAN_TYPE = ''
	SET @TR_NAT=''
	SET @REF_TAX=''
	--SET @ENTRY_TAX = 0
	
	SET @TRAN_TYPE = @U_IMPORM
	
	
	IF @U_IMPORM = 'Branch Transfer' OR @U_IMPORM = 'Consignment Transfer'
		begin
			SET @TRAN_TYPE = @U_IMPORM
		end
	ELSE IF @U_IMPORM = 'Inter-State Branch'
		begin
			SET @TRAN_TYPE = 'Branch Transfer'
		end
	ELSE IF @U_IMPORM = 'Inter-State Consignment Transfer'
		begin
			SET @TRAN_TYPE = 'Consignment Transfer'
		end
	ELSE IF @U_IMPORM = 'Export Out of India'
		begin
			SET @TRAN_TYPE = 'Direct Export out of India'
		end		
	ELSE IF @U_IMPORM = 'Discount/Incentive'
		begin
			SET @TRAN_TYPE = 'Discount/Incentive'
			SET @VATONAMT=((-1)* @VATONAMT)
			SET @FRGHT_AMT=((-1)* @FRGHT_AMT)
			SET @ENTRY_TAX=((-1)* @ENTRY_TAX)
		end	
	ELSE IF @U_IMPORM = 'Sample/Gift'
		begin
			SET @TRAN_TYPE = 'Sample/Gift'
		end	
	ELSE IF @U_IMPORM = 'Sales Return'
		begin
			SET @TRAN_TYPE = 'Sales Return'
			
			IF @BHENT = 'SR'
			BEGIN
				SELECT @TR_NAT=M.U_IMPORM FROM SRITREF R
				INNER JOIN STMAIN M ON (M.ENTRY_TY = R.RENTRY_TY AND M.TRAN_CD = R.ITREF_TRAN)
				WHERE R.Tran_cd = @TRAN_CD AND R.ENTRY_TY = @BHENT AND R.ITEM = @IT_NAME AND R.RENTRY_TY = 'ST'
				SET @TR_NAT = ISNULL(@TR_NAT,'')
				
				SELECT @REF_TAX=D.TAX_NAME FROM SRITREF R
				INNER JOIN STITEM D ON (D.ENTRY_TY = R.RENTRY_TY AND D.TRAN_CD = R.ITREF_TRAN AND D.ITEM = R.ITEM)
				WHERE R.Tran_cd = @TRAN_CD AND R.ENTRY_TY = @BHENT AND R.ITEM = @IT_NAME AND R.RENTRY_TY = 'ST'
				--SET @ENTRY_TAX = ISNULL(@ENTRY_TAX,0)
				
				SET @REF_TAX = ISNULL(@REF_TAX,'')
			END
			
			--SET @TR_NAT = 'Sales Return'
			SET @VATONAMT=((-1)* @VATONAMT)
			
			SET @FRGHT_AMT=((-1)* @FRGHT_AMT)
			SET @ENTRY_TAX=((-1)* @ENTRY_TAX)
		end	
	ELSE IF @U_IMPORM = 'Repair/Job Work'
		begin
			SET @TRAN_TYPE = 'Repair/Job Work'
			--SET @TR_NAT = 'Repair/Job Work'
		end	
	ELSE IF @U_IMPORM = 'Cancellation of Sales'
		begin
			SET @TRAN_TYPE = 'Cancellation of Sales'
			--SET @TR_NAT = 'Cancellation of Sales'
			IF @BHENT = 'CN'
			BEGIN
				SELECT @TR_NAT=M.U_IMPORM FROM CNITREF R
				INNER JOIN STMAIN M ON (M.ENTRY_TY = R.RENTRY_TY AND M.TRAN_CD = R.ITREF_TRAN)
				WHERE R.Tran_cd = @TRAN_CD AND R.ENTRY_TY = @BHENT AND R.ITEM = @IT_NAME AND R.RENTRY_TY = 'ST'
				SET @TR_NAT = ISNULL(@TR_NAT,'')
						
				SELECT @REF_TAX=D.TAX_NAME FROM CNITREF R
				INNER JOIN STITEM D ON (D.ENTRY_TY = R.RENTRY_TY AND D.TRAN_CD = R.ITREF_TRAN AND D.ITEM = R.ITEM)
				WHERE R.Tran_cd = @TRAN_CD AND R.ENTRY_TY = @BHENT AND R.ITEM = @IT_NAME AND R.RENTRY_TY = 'ST'
				--SET @ENTRY_TAX = ISNULL(@ENTRY_TAX,0)
				
				SET @REF_TAX = ISNULL(@REF_TAX,'')
			END
			
			
			SET @VATONAMT=((-1)* @VATONAMT)
			SET @FRGHT_AMT=((-1)* @FRGHT_AMT)
			SET @ENTRY_TAX=((-1)* @ENTRY_TAX)
		end
		
	IF @TAX_NAME = 'Form H'
		begin
			SET @TRAN_TYPE = 'Export against H form'
		end	
	ELSE IF @TAX_NAME = 'Form I'
		begin
			SET @TRAN_TYPE = 'Sale against I form'
		end	
	ELSE IF @TAX_NAME = 'Form C'
		begin
			SET @TRAN_TYPE = 'Sale against C form'
		end	
	ELSE IF @TAX_NAME = 'E - 1'
		begin
			SET @TRAN_TYPE = 'Sale in Transit(E-I)'
		end	
	ELSE IF @TAX_NAME = 'E - 2'
		begin
			SET @TRAN_TYPE = 'Sale in Transit(E-II)'
		end
	ELSE IF @TAX_NAME = ''	
		begin
			IF @ST_TYPE = 'OUT OF STATE' and @TRAN_TYPE = ''
				BEGIN
					SET @TRAN_TYPE = 'Tax Free Interstate Sale'
				END
		end
	ELSE IF @TAX_NAME like ('%C.S.T.%')
		begin
			IF @RFORM_NM = '' and @TRAN_TYPE = ''
				BEGIN
					SET @TRAN_TYPE = 'Sale without C form'
				END
		end
	ELSE IF @TAX_NAME like ('%C.S.T.%')
		begin
			IF @RFORM_NM in ('Form C', 'C Form', 'C') and @TRAN_TYPE = ''
				BEGIN
					SET @TRAN_TYPE = 'Sale against C form'
				END
		end
		
		
	IF @BHENT in ('SR','CN') AND @U_IMPORM in ('Sales Return','Cancellation of Sales')
		BEGIN
			IF @TR_NAT = 'Inter-State Branch'
				begin
					SET @TR_NAT = 'Branch Transfer'
				end
			ELSE IF @TR_NAT = 'Inter-State Consignment Transfer'
				begin
					SET @TR_NAT = 'Consignment Transfer'
				end
			ELSE IF @TR_NAT = 'Export Out of India'
				begin
					SET @TR_NAT = 'Direct Export out of India'
				end	
	
			
			IF @REF_TAX = 'Form H'
				begin
					SET @TR_NAT = 'Export against H form'
				end	
			ELSE IF @REF_TAX = 'Form I'
				begin
					SET @TR_NAT = 'Sale against I form'
				end	
			ELSE IF @REF_TAX = 'Form C'
				begin
					SET @TR_NAT = 'Sale against C form'
				end	
			ELSE IF @REF_TAX = 'E - 1'
				begin
					SET @TR_NAT = 'Sale in Transit(E-I)'
				end	
			ELSE IF @REF_TAX = 'E - 2'
				begin
					SET @TR_NAT = 'Sale in Transit(E-II)'
				end
			ELSE IF @REF_TAX = ''	
				begin
					IF @ST_TYPE = 'OUT OF STATE' and @TR_NAT = ''
						BEGIN
							SET @TR_NAT = 'Tax Free Interstate Sale'
						END
				end
			ELSE IF @REF_TAX like ('%C.S.T.%')
				begin
					IF @RFORM_NM = '' and @TR_NAT = ''
						BEGIN
							SET @TR_NAT = 'Sale without C form'
						END
				end
			ELSE IF @REF_TAX like ('%C.S.T.%')
				begin
					IF @RFORM_NM in ('Form C', 'C Form', 'C') and @TR_NAT = ''
						BEGIN
							SET @TR_NAT = 'Sale against C form'
						END
				end
			
	
			
		END
	
		
	set @itdesc = CASE WHEN isnull(@itdesc,'')<>'' THEN isnull(@itdesc,'') ELSE isnull(@it_name,'') end
	
    INSERT INTO #FORMPB07A (PART,PARTSR,SRNO,TRAN_TYPE,S_TAX,PARTY_NM,ADDRESS,billno,billdt,itdesc,QTY,AMT1,AMT2,NTRAN ,grno ,grdt,TR_NAT,AMT3,COUNTRY,LCS)
    VALUES (1,'1',CHAR(@CHAR),@TRAN_TYPE,@S_TAX,@PARTY_NM,@ADDRESS,@billno,@billdt,@itdesc,@QTY,@VATONAMT,@FRGHT_AMT,@NTRAN,@grno ,@grdt,@TR_NAT,@ENTRY_TAX,@COUNTRY,@LCS)	
	SET @CHAR=@CHAR+1

--	FETCH NEXT FROM CUR_FORMPB07Aaa INTO @TAXONAMT,@TAXAMT,@ITEMAMT,@PARTY_NM,@S_TAX,@billno,@billdt,@itdesc,@recformno ,@recformdt ,@grno ,@grdt ,@SRNO_OF_VAT36  
	FETCH NEXT FROM CUR_FORMPB07Aaa INTO @TRAN_CD,@BHENT, @U_IMPORM, @TAX_NAME, @ST_TYPE, @COUNTRY, @RFORM_NM, @PARTY_NM,@ADDRESS,@S_TAX,@billno,@billdt,@IT_NAME,@itdesc,@QTY,@VATONAMT,@FRGHT_AMT,@ENTRY_TAX,@grno ,@grdt,@NTRAN,@LCS		--Added by nilesh on dt 03/02/2015
END
CLOSE CUR_FORMPB07Aaa
DEALLOCATE CUR_FORMPB07Aaa

SET @AMTA1 = 0
SELECT @AMTA1=COUNT(PARTSR) FROM  #FORMPB07A WHERE PARTSR = '1'
SET @AMTA1 = ISNULL(@AMTA1,0)
IF @AMTA1 = 0
	BEGIN
		INSERT INTO #FORMPB07A (PART,PARTSR,SRNO,TRAN_TYPE,S_TAX,PARTY_NM,ADDRESS,billno,billdt,itdesc,QTY,AMT1,AMT2,NTRAN ,grno ,grdt,TR_NAT,AMT3,COUNTRY,LCS)
	    VALUES (1,'1','A','','','','','','','',0,0,0,'','','','',0,'','' )		
	END


INSERT INTO #FORMPB07A (PART,PARTSR,SRNO,TRAN_TYPE,S_TAX,PARTY_NM,ADDRESS,billno,billdt,itdesc,QTY,AMT1,AMT2,NTRAN ,grno ,grdt,TR_NAT,AMT3,COUNTRY,LCS)
    VALUES (1,'2','A','','','','','','','',0,0,0,'','','','',0,'','' )	


Update #FORMPB07A set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
					AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), PARTY_NM = isnull(Party_nm,''),ADDRESS = isnull(ADDRESS,''),S_TAX = isnull(S_TAX,'')
					,BILLNO = isnull(BILLNO,''),ITDESC = isnull(ITDESC,'')	--,BILLDT = isnull(BILLDT,'')
					,Qty = isnull(Qty,0),ntran = isnull(ntran,''),tran_type = isnull(tran_type,'')
					,TR_NAT= isnull(TR_NAT,''),AMT3= isnull(amt3,0),COUNTRY= isnull(country,''),LCS= isnull(LCS,'')
					
					
				 --,ITEMTYPE = isnull(ITEMTYPE,'')
					 --FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),  ITEM =isnull(item,''),



SELECT * FROM #FORMPB07A order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
END

--Print ''
