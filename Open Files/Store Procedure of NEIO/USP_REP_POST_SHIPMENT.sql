set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		Ajay Jaiswal
-- Create date: 12/01/2011
-- Description:	This Stored procedure is useful to Generate data for Post-Shipment Invoice Report.
-- Modified By:  Archana Khade
-- Modified Date:14/02/2014 for Bug-21456
-- =============================================

ALTER PROCEDURE  [dbo].[USP_REP_POST_SHIPMENT]
@ENTRYCOND NVARCHAR(254)
AS
	DECLARE @SQLCOMMAND AS NVARCHAR(4000),@TBLCON AS NVARCHAR(4000)
	DECLARE @CHAPNO VARCHAR(30),@EIT_NAME  VARCHAR(100),@MCHAPNO VARCHAR(250),@MEIT_NAME  VARCHAR(250)
	
--->ENTRY_TY AND TRAN_CD SEPARATION
	DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT
		
	PRINT @ENTRYCOND
	SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
	SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
	SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
	SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
	SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
	SET @TBLCON=RTRIM(@ENTRYCOND)
	
-- 	
SELECT 'REPORT HEADER' AS REP_HEAD,STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE
,STMAIN.U_TIMEP,STMAIN.U_TIMEP1 ,STMAIN.U_REMOVDT,STMAIN.U_EXPLA,STMAIN.U_EXRG23II,STMAIN.U_RG2AMT
,STITEM.EXAMT,STITEM.U_BASDUTY,STITEM.U_CESSPER,STITEM.U_CESSAMT,STITEM.U_HCESSPER,STITEM.U_HCESAMT
,STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,U_CHALNO=SPACE(1),U_CHALDT=STMAIN.DATE,STMAIN.U_PONO,STMAIN.U_PODT
,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STMAIN.TAXAMT
,STMAIN.NET_AMT,STMAIN.U_PLASR,STMAIN.U_RG23NO,STMAIN.U_RG23CNO
,STITEM.U_PKNO,STITEM.QTY,STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE,STITEM.U_EXPDESC,STITEM.U_EXPMARK
,STITEM.U_EXPGWT,STITEM.U_EXPNWT,STITEM.U_TWEIGHT,STMAIN.U_QADINV
,STMAIN.u_fdesti,STITEM.FCRATE,STMAIN.FCGRO_AMT,CURR_MAST.CURRDESC
,cast(STITEM.u_pkno as int) as U_PKNO1,STMAIN.U_BLNO,STMAIN.U_BLDT,STMAIN.U_countain,STMAIN.U_COUNTAI2
,STMAIN.U_TSEAL,STMAIN.U_TSEAL2,STMAIN.U_PRECARRI,STMAIN.U_RECEIPT,STMAIN.U_LOADING
,STMAIN.U_PORT,'India' as U_ORIGIN,STMAIN.U_EXPDEL,IT_MAST.IT_NAME
--,CAST(IT_MAST.IT_DESC AS VARCHAR(4000)) AS IT_DESC 
,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'')='' THEN it_mast.it_name ELSE it_mast.it_alias END)
,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'')='' THEN ac_mast.ac_name ELSE ac_mast.mailname END)	
,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT 
,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX
,AC_MAST.ECCNO ,AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1
,AC_MAST1.ZIP ZIP1,AC_MAST1.S_TAX S_TAX1,AC_MAST1.I_TAX I_TAX1,AC_MAST1.ECCNO ECCNO1,STITEM.ITSERIAL
,STMAIN.U_OTHREF,STMAIN.U_PAYMENT,STMAIN.U_VESSEL,STMAIN.U_SBNO,STMAIN.U_SBDT
,STMAIN.U_CONTNO,STMAIN.U_PKGNO --Added by Archana K. on 14/02/14 for Bug-21456
INTO #STMAIN
FROM STMAIN  INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD) 
INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE) 
INNER JOIN CURR_MAST ON (STMAIN.FCID = CURR_MAST.CURRENCYID)
INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID) 
LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=STMAIN.U_DELIVER) 
WHERE  STMAIN.ENTRY_TY= @ENT  AND STMAIN.TRAN_CD=@TRN
ORDER BY STMAIN.INV_SR,CAST(STMAIN.INV_NO  AS INT)
SET @MCHAPNO=' '
SET @MEIT_NAME=' '
	
DECLARE CUR_STBILL CURSOR FOR SELECT DISTINCT CHAPNO FROM #STMAIN
OPEN CUR_STBILL 
FETCH NEXT FROM CUR_STBILL INTO @CHAPNO
WHILE(@@FETCH_STATUS=0)
BEGIN
	SET @MCHAPNO=RTRIM(@MCHAPNO)+','+RTRIM(@CHAPNO)
	FETCH NEXT FROM CUR_STBILL INTO @CHAPNO
END
CLOSE CUR_STBILL
DEALLOCATE CUR_STBILL

DECLARE CUR_STBILL CURSOR FOR SELECT DISTINCT EIT_NAME FROM #STMAIN
OPEN CUR_STBILL 
FETCH NEXT FROM CUR_STBILL INTO @EIT_NAME
WHILE(@@FETCH_STATUS=0)
BEGIN
	SET @MEIT_NAME=RTRIM(@MEIT_NAME)+','+RTRIM(@EIT_NAME)
	FETCH NEXT FROM CUR_STBILL INTO @EIT_NAME
END
CLOSE CUR_STBILL
DEALLOCATE CUR_STBILL	

SET @MCHAPNO=CASE WHEN LEN(@MCHAPNO)>1 THEN SUBSTRING(@MCHAPNO,2,LEN(@MCHAPNO)-1) ELSE '' END
SET @MEIT_NAME=CASE WHEN LEN(@MEIT_NAME)>1 THEN SUBSTRING(@MEIT_NAME,2,LEN(@MEIT_NAME)-1) ELSE '' END
SELECT * 
,MCHAPNO=ISNULL(@MCHAPNO,'')
,MEIT_NAME=ISNULL(@MEIT_NAME,'')
FROM #STMAIN

