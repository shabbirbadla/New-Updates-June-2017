
if exists(select name,XTYPE from sysobjects where xtype='P' and name='USP_REP_DM_VATFORM30')
begin
	drop procedure USP_REP_DM_VATFORM30
end
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
EXECUTE USP_REP_DM_VATFORM30'','','','04/01/2017','04/30/2018','','','','',0,0,'','','','','','','','','2009-2012',''
EXECUTE USP_REP_SINGLE_CO_DATA_VAT'','','','04/01/2009','04/30/2014','','','','',0,0,'','','','','','','','','2009-2012','',''


*/
-- =============================================
-- Author:		Sandeep shah
-- Create date: 29/08/2012
-- Description:	This Stored procedure is useful to generate Daman DVAT Form 30
-- Modify by/date/bug/:  Sandeep on 15-Jan-15 for 21339
-- Modify by/date/bug/:  Suraj kumawat on 30-Jan-16 for Bug 27607

-- =============================================

create PROCEDURE [dbo].[USP_REP_DM_VATFORM30]
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
DECLARE @vatfltopt VARCHAR(25)
select @vatfltopt=vat_flt_opt from manufact
print @vatfltopt
Begin

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

Declare @SQLCOMMAND NVARCHAR(4000)
 DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)
 DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
 DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''


Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code = 999999999999999999-999999999999999999,ItSerial=space(5)
INTO #DMVAT_30
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #DMVAT_30 add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAAA',RATE=99999.999,AMT1=m.NET_AMT,AMT2=m.NET_AMT,AMT3=m.NET_AMT,AMT4=m.NET_AMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),
STM.FORM_NM,AC1.S_TAX,PTTYPE=SPACE(100)--,STM.TAX_NAME
INTO #DMVAT30
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
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
--		SET @SQLCOMMAND='Insert InTo #DMVAT_30 Select * from '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--		---Drop Temp Table 
--		SET @SQLCOMMAND='Drop Table '+@MCON
--		EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	End
--else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 PRINT 'SUMIT'
		 EXECUTE USP_REP_SINGLE_CO_DATA_VAT
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT
		 
		---- EXECUTE USP_REP_SINGLE_CO_DATA
		----  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		---- ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		---- ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		---- ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		---- ,@MFCON = @MCON OUTPUT

		------SET @SQLCOMMAND='Select * from '+@MCON
		-------EXECUTE SP_EXECUTESQL @SQLCOMMAND
		----SET @SQLCOMMAND='Insert InTo #DMVAT_30 Select * from '+@MCON
		----EXECUTE SP_EXECUTESQL @SQLCOMMAND
		-------Drop Temp Table 
		------SET @SQLCOMMAND='SELECT * FROM '+@MCON
		----SET @SQLCOMMAND='Drop Table '+@MCON
		----EXECUTE SP_EXECUTESQL @SQLCOMMAND
		print 'vat_co data'
	End
	
	
---Added by Suraj Kumawat date on 30-01-2016 start  for  Bug 27607
--INSERT INTO #DMVAT30 
	--SELECT  1 AS PART,'1' AS PARTSR,'A' AS SRNO, a.Per,TAXONAMT=ISNULL(SUM(A.VATONAMT),0),TAXAMT=ISNULL(SUM(A.TAXAMT),0),
	--ITEMAMT=(ISNULL(SUM(A.VATONAMT),0) + ISNULL(SUM(A.TAXAMT),0)),amt4=0
	--,INV_NO=case when @vatfltopt='Bill Date' then c.u_pinvno  else a.inv_no end
	--,DATE=case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end ,A.AC_NAME,A.ADDRESS,FORM_NM=C.form_no,A.S_TAX,
	--PTTYPE = (CASE WHEN A.ST_TYPE='OUT OF COUNTRY' AND A.ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED' THEN 'IMPORT' ELSE 
	--		CASE WHEN A.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED'  THEN 'STATE' ELSE
	--		CASE WHEN  A.ST_TYPE='LOCAL' AND ITEMTYPE='C' AND A.TAX_NAME<>'EXEMPTED' THEN 'LOCAL C' ELSE 
	--		CASE WHEN A.ST_TYPE='LOCAL' AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED'  THEN 'LOCAL O' else
	--		case when A.tax_name='EXEMPTED' then 'EXEMPTED' else '' end   END END END END )
 -- 	FROM VATTBL A INNER JOIN PTMAIN C ON (A.Bhent = C.Entry_ty And A.Tran_cd = C.Tran_cd)
	--WHERE A.BHENT IN('PT','P1') AND ( case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end  BETWEEN @SDATE AND @EDATE )
	--and	A.st_type in ('OUT OF COUNTRY','OUT OF STATE','LOCAL') and a.s_Tax<>'' 
	--group by  A.PER,c.u_pinvno,c.u_pinvdt,a.inv_no,a.date,A.AC_NAME,A.ADDRESS,c.form_no,A.S_TAX,a.st_type,a.tax_name,a.itemtype
	--order by date
INSERT INTO #DMVAT30 
SELECT * FROM (SELECT  1 AS PART,'1' AS PARTSR,'A' AS SRNO, a.Per,TAXONAMT=ISNULL(SUM(A.VATONAMT),0),TAXAMT=ISNULL(SUM(A.TAXAMT),0),
ITEMAMT=(ISNULL(SUM(A.VATONAMT),0) + ISNULL(SUM(A.TAXAMT),0)),amt4=0
,INV_NO=case when @vatfltopt='Bill Date' then c.u_pinvno  else a.inv_no end
,DATE=case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end ,A.AC_NAME,A.ADDRESS,FORM_NM=C.form_nm,A.S_TAX,
PTTYPE = (CASE WHEN A.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED'  THEN 'STATE' ELSE
		CASE WHEN  A.ST_TYPE='LOCAL' AND ITEMTYPE='C' AND A.TAX_NAME<>'EXEMPTED' THEN 'LOCAL C' ELSE 
		CASE WHEN A.ST_TYPE='LOCAL' AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED'  THEN 'LOCAL O' else
		case when A.tax_name='EXEMPTED' then 'EXEMPTED' else '' end   END END END)
FROM VATTBL A INNER JOIN PTMAIN C ON (A.Bhent = C.Entry_ty And A.Tran_cd = C.Tran_cd)
WHERE A.BHENT IN('PT') AND ( case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end  BETWEEN @SDATE AND @EDATE )
and	A.st_type in ('OUT OF STATE','LOCAL') and a.s_Tax<>'' 
group by  A.PER,c.u_pinvno,c.u_pinvdt,a.inv_no,a.date,A.AC_NAME,A.ADDRESS,c.form_nm,A.S_TAX,a.st_type,a.tax_name,a.itemtype) A
UNION ALL
SELECT * FROM (SELECT  1 AS PART,'1' AS PARTSR,'A' AS SRNO, a.Per,TAXONAMT=ISNULL(SUM(A.VATONAMT),0),TAXAMT=ISNULL(SUM(A.TAXAMT),0),
ITEMAMT=(ISNULL(SUM(A.VATONAMT),0) + ISNULL(SUM(A.TAXAMT),0)),amt4=0
,INV_NO=case when @vatfltopt='Bill Date' then c.u_pinvno  else a.inv_no end
,DATE=case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end ,A.AC_NAME,A.ADDRESS,FORM_NM=C.form_nm,A.S_TAX,
PTTYPE = 'IMPORT'
FROM VATTBL A INNER JOIN PTMAIN C ON (A.Bhent = C.Entry_ty And A.Tran_cd = C.Tran_cd)
WHERE A.BHENT IN('PT','P1') and	A.st_type in ('OUT OF COUNTRY') AND (A.U_IMPORM in('Direct Imports','Import from Outside India','High Seas Purchases'))
AND LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(A.Form_Nm,''),'-',''),'FORM',''))) = 'H'
group by  A.PER,c.u_pinvno,c.u_pinvdt,a.inv_no,a.date,A.AC_NAME,A.ADDRESS,c.form_nm,A.S_TAX,a.st_type,a.tax_name,a.itemtype) B
ORDER BY DATE



	IF NOT EXISTS(SELECT TOP 1 PART FROM #DMVAT30 )
	BEGIN
	INSERT INTO #DMVAT30 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,amt4,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PTTYPE)
                 VALUES (1,'1','A',0,0,0,0,0,'','','','','','','')
	END
	SELECT * FROM #DMVAT30 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int), partsr,SRNO

	---Added by Suraj Kumawat date on 30-01-2016 END for  Bug 27607

	--- Commented by suraj  date on 30-01-2015  Start for  Bug 27607
	
---- SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 



----Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4),@STTYPE AS  VARCHAR(20),@ITEMTYPE AS VARCHAR(1),@PTTYPE AS VARCHAR(20),@TAX_NAME AS VARCHAR(10)

----SELECT @TAXONAMT=0,@TAXAMT1 =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0,@TAX_NAME=''


----Declare cur_DMVAT30 cursor for
----SELECT distinct a.Per,TAXONAMT=sum(((B.QTY*B.RATE)-(B.TOT_DEDUC))+B.TOT_EXAMT+B.TOT_TAX),TAXAMT=SUM(A.TAXAMT),
----ITEMAMT=SUM(A.Net_amt)
----,INV_NO=case when @vatfltopt='Bill Date' then c.u_pinvno  else a.inv_no end
----,DATE=case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end ,A.PARTY_NM,A.ADDRESS,FORM_NM=C.form_no,A.S_TAX,
----PTTYPE = CASE WHEN d.ST_TYPE='OUT OF COUNTRY' AND ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED' THEN 'IMPORT' ELSE 
----		CASE WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED'  THEN 'STATE' ELSE
----		CASE WHEN  d.ST_TYPE='LOCAL' AND ITEMTYPE='C' AND A.TAX_NAME<>'EXEMPTED' THEN 'LOCAL C' ELSE 
----		CASE WHEN d.ST_TYPE='LOCAL' AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED'  THEN 'LOCAL O' else
----		case when a.tax_name='EXEMPTED' then 'EXEMPTED' else '' end   END END END END  

----FROM #DMVAT_30 A
----Inner Join Litem_vw B On(A.Bhent = B.Entry_ty And A.Tran_cd = b.Tran_cd and a.it_code=b.it_code and a.itserial=b.itserial)--  And A.Tax_name = B.Tax_Name )
----INNER JOIN AC_MAST d ON (b.AC_ID = d.AC_ID)
----INNER JOIN PTMAIN C ON (A.Bhent = C.Entry_ty And A.Tran_cd = C.Tran_cd)

----WHERE A.BHENT='PT' AND ( case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end  BETWEEN @SDATE AND @EDATE )
----and	d.st_type in ('OUT OF COUNTRT','OUT OF STATE') or d.st_type='LOCAL' and a.s_Tax<>'' ---Sandeep on 15-Jan-15 for 21339
----group by  A.PER,c.u_pinvno,c.u_pinvdt,a.inv_no,a.date,A.PARTY_NM,A.ADDRESS,c.form_no,A.S_TAX,d.st_type,a.tax_name,a.itemtype
----order by date


----OPEN CUR_DMVAT30
----FETCH NEXT FROM CUR_DMVAT30 INTO @PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,@PTTYPE--,@TAX_NAME
----WHILE (@@FETCH_STATUS=0)
----BEGIN

----	SET @PER =CASE WHEN @PER IS NULL THEN 0 ELSE @PER END
----	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
----	SET @TAXAMT1=CASE WHEN @TAXAMT1 IS NULL THEN 0 ELSE @TAXAMT1 END
----	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
----	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
----	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
----	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
----	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
----	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
----	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
----    PRINT @sttype
----    print @itemtype
----    SET @PTTYPE=CASE WHEN @PTTYPE IS NULL THEN '' ELSE @PTTYPE END

----	INSERT INTO #DMVAT30 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PTTYPE)
----                 VALUES (1,'1','A',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,@PTTYPE)
	
----	SET @CHAR=@CHAR+1
----	FETCH NEXT FROM CUR_DMVAT30 INTO @PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,@PTTYPE
----END
----CLOSE CUR_DMVAT30
----DEALLOCATE CUR_DMVAT30
------<- PART 1-4

----Update #DMVAT30 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
----		             RATE = isnull(RATE,0),AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
----					 AMT3 = isnull(AMT3,0),AMT4 = isnull(AMT4,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
----					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
----					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')

--- SELECT * FROM #DMVAT30 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int), partsr,SRNO

----- Commented by suraj Kumawat Date on 03-01-2015 for  Bug 27607

End
GO


