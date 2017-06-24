if exists(select name,xtype from sysobjects where xtype ='p' and name ='USP_REP_KL_PUR_SAL_ANNEXURE_DADOS')
begin
	drop procedure  USP_REP_KL_PUR_SAL_ANNEXURE_DADOS
end
SET DATEFORMAT ymd
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
 EXECUTE USP_REP_KL_PUR_SAL_ANNEXURE_DADOS '04/01/2011','03/30/2017'
*/
-- =============================================
-- Author:		Suraj Kumawat 
-- Create date: 08-08-2016
-- Description:	This Stored procedure is useful to generate Karala VAT purcahse and sales Annexure 

-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_KL_PUR_SAL_ANNEXURE_DADOS]
@SDATE SMALLDATETIME,@EDATE SMALLDATETIME
AS        
BEGIN        
Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2),@MultiCo VARCHAR(50),@MCON VARCHAR(50)
EXECUTE USP_REP_SINGLE_CO_DATA_VAT
 '', '', '', @SDATE, @EDATE
,'', '', '', '', 0, 0
,'', '', '', '',''
,'', '', '', '', ''
,@MFCON = @MCON OUTPUT
 --DROP TABLE #VATTBL1
 
 SELECT distinct  *  INTO #VATTBL1 FROM VATTBL  WHERE BHENT IN('PT','ST') ---and st_type in('local','') and tax_name like '%vat%'
 ---Temprirory table 
 SELECT INV_NO,DATE, S_TAX ,AC_NAME,ADDRESS,NET_AMT as VATONAMT,NET_AMT as TAXAMT,NET_AMT  AS CESSAMT,ANN_TYPE=SPACE(150)  INTO #ANNEXURE_KL_PUR_SL FROM #VATTBL1 WHERE  1=2
 

 IF EXISTS(SELECT TOP 1 STAX_ITEM FROM LCODE  WHERE Entry_ty ='PT' AND STAX_ITEM > 0  )
	 BEGIN
		 INSERT INTO #ANNEXURE_KL_PUR_SL
		 SELECT A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,ISNULL(sum(A.VATONAMT),0) as VATONAMT,ISNULL(sum(A.TAXAMT),0) as TAXAMT,ISNULL(SUM(b.addlvat1),0) AS CESSAMT,ANN_TYPE='Purchase'   FROM #VATTBL1 A LEFT OUTER JOIN PTITEM B ON (A.BHENT =B.entry_ty AND A.TRAN_CD =B.Tran_cd AND A.ItSerial =B.itserial) WHERE A.BHENT ='PT'
		 GROUP BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,A.BHENT ORDER BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS
	 END 
 ELSE
	 BEGIN
		 INSERT INTO #ANNEXURE_KL_PUR_SL	
		 SELECT A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,MAX(A.VATONAMT) as VATONAMT,MAX(A.TAXAMT)as TAXAMT,MAX(b.addlvat1) AS CESSAMT,ANN_TYPE='Purchase'  FROM #VATTBL1 A LEFT OUTER JOIN PTITEM B ON (A.BHENT =B.entry_ty AND A.TRAN_CD =B.Tran_cd AND A.ItSerial =B.itserial ) WHERE A.BHENT ='PT'
		 GROUP BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,A.BHENT ORDER BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS
 END

 IF EXISTS(SELECT TOP 1 STAX_ITEM FROM LCODE  WHERE Entry_ty ='ST' AND STAX_ITEM > 0  )
	 BEGIN
		 INSERT INTO #ANNEXURE_KL_PUR_SL	
		 SELECT A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,isnull(sum(A.VATONAMT),0) as VATONAMT,isnull(sum(A.TAXAMT),0)as TAXAMT
		 ,isnull(SUM(b.addlvat1),0) AS CESSAMT,ANN_TYPE='Sales' FROM #VATTBL1 A LEFT OUTER JOIN STITEM B ON (A.BHENT =B.entry_ty AND A.TRAN_CD =B.Tran_cd AND A.ItSerial =B.itserial ) 
		 WHERE A.BHENT ='ST'
		 GROUP BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,A.BHENT ORDER BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS
	 END 
 ELSE
	 BEGIN
	     INSERT INTO #ANNEXURE_KL_PUR_SL 
		 SELECT A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,MAX(A.VATONAMT) as VATONAMT,MAX(A.TAXAMT)as TAXAMT,MAX(b.addlvat1) AS CESSAMT,ANN_TYPE='Sales' 
		 FROM #VATTBL1 A LEFT OUTER JOIN STITEM B ON (A.BHENT =B.entry_ty AND A.TRAN_CD =B.Tran_cd AND A.ItSerial =B.itserial)
		 WHERE A.BHENT ='ST'
		 GROUP BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS,A.BHENT ORDER BY A.BHENT,A.INV_NO,A.DATE, A.S_TAX ,A.AC_NAME,A.ADDRESS
 END
 SELECT * FROM #ANNEXURE_KL_PUR_SL ORDER BY ANN_TYPE,INV_NO,DATE, S_TAX ,AC_NAME,ADDRESS
END        
