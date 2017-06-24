If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_DL_VATFORM30'))
Begin
	Drop Procedure USP_REP_DL_VATFORM30
End
Go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




/*
EXECUTE USP_REP_DL_VATFORM30'','','','04/01/2011','03/30/2015','','','','',0,0,'','','','','','','','','2010-2014',''
*/
-- =============================================
-- Author:		Hetal L Patel
-- Create date: 22/05/2010
-- Description:	This Stored procedure is useful to generate Delhi VAT Form 30
-- Modify date: 23/05/2011
-- Modified By: Sandeep Shah
-- Remark:      Duplicate details reflecting in Delhi VAT Report for the TKT-8061
-- Modify date: 27/07/2011
-- Modified By: Sandeep Shah
-- Remark:      If we take the same material more than one time in a signle transaction for purchase
--				, it shows multiple times in Vat Form 30 report for TKT-8983
-- Modify date: 05/09/2011
-- Modified By: Sandeep Shah
-- Remark:      Add bill date and transaction date wise filter option by sandeep for TKT-9444 -->Start 
-- Remark:      changes by sandeep for bug-15540 on 11-06-03
-- Remark:      changes by sandeep for bug-20342 on 22-11-13
-- =============================================
create PROCEDURE [dbo].[USP_REP_DL_VATFORM30]
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
--SET QUOTED_IDENTIFIER off
--DECLARE @VATFLTNO NVARCHAR(10),@VATFLTDT NVARCHAR(10)
DECLARE @vatfltopt VARCHAR(25)
select @vatfltopt=vat_flt_opt from manufact
print @vatfltopt

--SELECT @VATFLTNO=CASE WHEN VAT_FLT_OPT=1 THEN 'U_PTINVNO' ELSE 'INV_NO' END  FROM MANUFACT
--SELECT @VATFLTDT=CASE WHEN VAT_FLT_OPT=1 THEN 'U_PTINVDT' ELSE 'DATE' END  FROM MANUFACT
Begin
--SET QUOTED_IDENTIFIER ON
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
--select vatfltopt=vat_flt_opt into #vatfltopt from manufact  
--select * from #vatfltopt
SELECT ENTRY_TY,BCODE=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),STAX_ITEM  INTO #LCODE FROM LCODE --for bug-6755,LUBRI


Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code = 999999999999999999-999999999999999999,ItSerial=space(5)
INTO #DLVAT_30
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #DLVAT_30 add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,AMT4=TAXAMT,
M.INV_NO,VATMTYPE=space(10),M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),
STM.FORM_NM,AC1.S_TAX,PTTYPE=SPACE(20)--,STM.TAX_NAME
INTO #DLVAT30
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
		SET @SQLCOMMAND='Insert InTo #DLVAT_30 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 EXECUTE USP_REP_SINGLE_CO_DATA
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo #DLVAT_30 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		--SET @SQLCOMMAND='SELECT * FROM '+@MCON
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
 SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 

-----
--select *  FROM #DLVAT_30 where BHENT='PT'--party_nm='Grace Fashion Accessories, Delhi                  '

------->- PART 1-4 
Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),@vatmtype as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4),@STTYPE AS  VARCHAR(20),@ITEMTYPE AS VARCHAR(1),@PTTYPE AS VARCHAR(20),@TAX_NAME AS VARCHAR(10)

SELECT @TAXONAMT=0,@TAXAMT1 =0,@ITEMAMT =0,@INV_NO ='',@VATMTYPE ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0,@TAX_NAME=''

--SELECT * FROM #DLVAT_30  where BHENT='PT'  --and (set_app=0 and s_tax=''  ) or sT_type='OUT OF STATE  '  or sT_type='OUT OF COUNTRY  ' 


-- change by Sandeep for TKT-8983 -->Start
--Add bill date and transaction date wise filter option by sandeep for TKT-9444 -->Start
--AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED' THEN 'LOCAL O' ELSE 'EXEMPTED'  END END END END for bug-15540 by sandeep on 11-jun-13
--bug-20342 --->START

-- change by Sandeep for TKT-8983 -->End
--Add bill date and transaction date wise filter option by sandeep for TKT-9444 -->End

--select * from #1 where pttype is not null

Declare cur_dlvat30 cursor for
--Add bill date and transaction date wise filter option by sandeep for TKT-9444 -->Start 
-- change by Sandeep for TKT-8983 -->start

SELECT distinct a.Per,TAXONAMT=sum(((B.QTY*B.RATE)-(B.TOT_DEDUC))+B.TOT_EXAMT+B.TOT_TAX),TAXAMT=SUM(A.TAXAMT),
ITEMAMT=SUM(A.Net_amt)
,C.VATMTYPE
,INV_NO=case when @vatfltopt='Bill Date' then c.u_pinvno  else a.inv_no end
,DATE=case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end ,A.PARTY_NM,A.ADDRESS,FORM_NM='',s_tax=a.s_tax ,
--PTTYPE = CASE
-- WHEN d.ST_TYPE='OUT OF COUNTRY' AND ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED' THEN 'IMPORT' 
-- WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND A.TAX_NAME<>'EXEMPTED'  THEN 'STATE'-- ELSE 
-- WHEN  d.ST_TYPE='LOCAL' AND ITEMTYPE='C' AND A.TAX_NAME<>'EXEMPTED' THEN 'LOCAL C' --ELSE 
-- WHEN d.ST_TYPE='LOCAL' AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED' THEN 'LOCAL O' ELSE 'EXEMPTED' 
-- END 
PTTYPE =  CASE 
	WHEN d.ST_TYPE in ('OUT OF COUNTRY') AND ITEMTYPE<>'' AND  a.u_imporm<>'High Seas Purchases' AND A.TAX_NAME<>'EXEMPTED' 
	--AND a.u_imporm='Import from Outside India'	 and c.vatmtype='IOI'	
	THEN 'IMPORT'
	WHEN  d.ST_type in ('OUT OF STATE','OUT OF COUNTRY') and ITEMTYPE<>'' AND a.u_imporm='High Seas Purchases' AND A.TAX_NAME<>'EXEMPTED' 
	-- and c.vatmtype='HSP'
	 THEN 'HSEASP'
--	--WHEN   ITEMTYPE<>'' AND C.u_imporm='Purchase from Exempted Units' and  a.TAX_NAME='Form I'  THEN 'EXEMPTED' 
--	WHEN d.ST_TYPE in ('OUT OF STATE') AND ITEMTYPE<>'' AND A.TAX_NAME='Form F' AND B.entry_ty='LR' THEN 'LABOURjob' 		
	WHEN   ITEMTYPE<>'' AND a.u_imporm='Purchase from Exempted Units' 
	or a.TAX_NAME=' ' and ITEMTYPE<>'' AND a.u_imporm='Purchase from Exempted Units' 
   THEN 'EXEMUNIT' 
	WHEN  
	-- a.u_imporm ='Purchase from Unregistered Dealer' and c.vatmtype ='PUC'
	-- or   a.u_imporm ='Composition Dealer' and c.vatmtype ='PCD'	
	-- or   a.u_imporm ='Against Retail Invoices' and c.vatmtype ='PRI'	
	-- or   a.u_imporm ='Non Creditable Goods' and c.vatmtype ='PCG'	
     a.s_tax='' and d.st_type<>'' 
	  then 'UNREGPUR' 
	--WHEN d.ST_type in ('OUT OF STATE') and ITEMTYPE<>'' AND C.u_imporm='Interstate Purchase of Tax Exempted Goods' and  A.TAX_NAME='EXEMPTED' and D.s_tax<>''   THEN 'EXEMPTED1'
	WHEN d.ST_type in ('OUT OF STATE','LOCAL') and ITEMTYPE<>''and  A.TAX_NAME='EXEMPTED' and a.s_tax<>''
	
	--  AND a.u_imporm='Interstate Purchase of Tax Exempted Goods' 
	  THEN 'EXEMGOOD'
	--WHEN d.ST_TYPE in ('OUT OF STATE') AND ITEMTYPE<>'' AND A.TAX_NAME='Form F' AND B.entry_ty='RL' THEN 'LABOURJOB' 		
	 WHEN d.ST_type in ('OUT OF STATE') and ITEMTYPE='C' AND A.TAX_NAME<>'EXEMPTED'
	-- and a.u_imporm='Inter State purchase - Capital Goods' and  c.vatmtype='GD'
	 THEN 'CAPNCR'
	 WHEN d.ST_TYPE='LOCAL' and a.s_tax<>'' AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED'
	-- and a.u_imporm='Eligible Local purchases' and  c.vatmtype='GD'  
	 THEN 'LOCAL O' 
	 WHEN  d.ST_TYPE='LOCAL' and a.s_tax<>'' AND ITEMTYPE='C' AND A.TAX_NAME<>'EXEMPTED'
	-- and a.u_imporm='Eligible Local purchases' and  c.vatmtype='GD' 
	 THEN 'LOCAL C'	
	 WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND a.u_imporm='Branch Transfer' AND A.TAX_NAME<>'EXEMPTED' 
	--and a.tax_name='FORM F'or a.tax_name=''  and sum(a.taxamt)=0 
	 THEN 'BRN_TRN' 
	 WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND a.u_imporm='Consignment Transfer' AND A.TAX_NAME<>'EXEMPTED' 
	-- and a.tax_name='FORM F' or a.tax_name=''  and sum(a.taxamt)=0  
	  THEN 'CONS_TRN' 
	 WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' and A.tax_name IN ('E - 1','E - 2') AND ST.FORM_NM='FORM C' and   ST.RFORM_NM IN ('E1 Form','E2 Form') AND A.TAX_NAME<>'EXEMPTED' 
	-- and a.u_imporm='Inter State Purchase' 
	 THEN 'CE1E2FRM' 
	 WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' AND a.TAX_NAME='Form H'
	-- and a.u_imporm='Inter State Purchase' and sum(a.taxamt)=0
	 THEN 'HFORM' 
	 WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>'' and st.FORM_NM='Form C' and a.tax_name NOT in ('E - 1','E - 2') 
 	--and a.u_imporm='Inter State Purchase' 
	  THEN 'CFORM' 
	 WHEN d.ST_TYPE='OUT OF STATE' AND ITEMTYPE<>'' AND a.per in (0.00,1.00,5.00,12.00,20.00)  and ST.FORM_NM=''   and  ST.RFORM_NM=''
	or  ST.FORM_NM=''  and  ST.RFORM_NM=''
	-- and a.u_imporm='Inter State Purchase' 
	THEN 'NONE' 
	 --WHEN d.ST_type='OUT OF STATE' AND ITEMTYPE<>''  AND A.TAX_NAME<>'EXEMPTED'  THEN 'STATE' 
--    ELSE 'LOCAL'  
--    WHEN d.ST_TYPE='LOCAL' and a.s_tax<>'' AND ITEMTYPE<>'C' AND A.TAX_NAME<>'EXEMPTED' and a.u_imporm='Eligible Local purchases'  THEN 'LOCAL O' 
	
END  
--bug-20342 --->END

FROM #DLVAT_30 A
Inner Join Litem_vw B On(A.Bhent = B.Entry_ty And A.Tran_cd = b.Tran_cd and a.it_code=b.it_code and a.itserial=b.itserial)-- And A.Tax_name = B.Tax_Name )
INNER JOIN AC_MAST d ON (b.AC_ID = d.AC_ID)
LEFT JOIN PTMAIN C ON (A.Bhent = C.Entry_ty And A.Tran_cd = C.Tran_cd)
LEFT JOIN IRMAIN LR ON (A.Bhent = LR.Entry_ty And A.Tran_cd = LR.Tran_cd)
left join stax_mas st on (st.entry_ty=a.bhent and a.tax_name=st.tax_name )
inner join #lcode lc on (lc.entry_ty=a.bhent)
WHERE lc.bcode='PT' AND ( case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end  BETWEEN @SDATE AND @EDATE )
group by  A.PER,c.u_pinvno,c.u_pinvdt,a.inv_no,a.date,A.PARTY_NM,A.ADDRESS,c.form_no,A.S_TAX,d.st_type,a.tax_name,a.itemtype, a.u_imporm,c.vatmtype
,st.form_nm,st.rform_nm
order by a.date
--WHERE A.BHENT IN ('PT','LR') AND ( case when @vatfltopt='Bill Date' then c.u_pinvdt  else a.date end   BETWEEN @SDATE AND @EDATE ) 
--group by  A.PER,c.u_pinvno,c.u_pinvdt,a.inv_no,a.date,A.PARTY_NM,A.ADDRESS,d.S_TAX,d.st_type,a.tax_name
--,a.itemtype,a.st_type,a.set_app,a.s_tax,a.u_imporm,ST.form_nm,c.tax_name,st.form_nm,d.c_tax,b.entry_ty,C.VATMTYPE,ST.rform_nm
--order by a.date
--Select * from #1  --where pttype is not null --WHERE PTTYPE LIKE 'local%' AND PER=0 AND S_TAX='' OR PER<>0 AND S_TAX<>''--Chnages for bug-15540 by sandeep on 11-jun-13
OPEN CUR_DLVAT30
FETCH NEXT FROM CUR_DLVAT30 INTO @PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@VATMTYPE,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,@PTTYPE--,@TAX_NAME
WHILE (@@FETCH_STATUS=0)
BEGIN

	SET @PER =CASE WHEN @PER IS NULL THEN 0 ELSE @PER END
	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
	SET @TAXAMT1=CASE WHEN @TAXAMT1 IS NULL THEN 0 ELSE @TAXAMT1 END
	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
	SET @VATMTYPE=CASE WHEN @VATMTYPE IS NULL THEN '' ELSE @VATMTYPE END
	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
    PRINT @sttype
    print @itemtype
    SET @PTTYPE=CASE WHEN @PTTYPE IS NULL THEN '' ELSE @PTTYPE END

	INSERT INTO #DLVAT30 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,VATMTYPE,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX,PTTYPE)
                 VALUES (1,'1','A',@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@VATMTYPE,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,@PTTYPE)
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_DLVAT30 INTO @PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@VATMTYPE,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX,@PTTYPE
END
CLOSE CUR_DLVAT30
DEALLOCATE CUR_DLVAT30
--<- PART 1-4
--DROP TABLE #1 

Update #DLVAT30 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0),AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					 AMT3 = isnull(AMT3,0),AMT4 = isnull(AMT4,0), VATMTYPE = isnull(VATMTYPE,''), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),  ITEM =isnull(item,''),

 SELECT * FROM #DLVAT30 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int), partsr,SRNO,inv_no
End




