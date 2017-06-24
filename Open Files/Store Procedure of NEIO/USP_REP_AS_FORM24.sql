IF EXISTS (SELECT XTYPE, NAME FROM SYSOBJECTS WHERE XTYPE = 'P' AND NAME = 'USP_REP_AS_FORM24')
	BEGIN
		DROP PROCEDURE USP_REP_AS_FORM24
	END
	
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
 -- Author:  Hetal L Patel
 -- Create date: 16/05/2007
 -- Description: This Stored procedure is useful to generate AS VAT FORM 24.
 -- Modify date: 16/05/2007 
 -- Modified By: Hetal Patel
 -- Modify date: 21/07/2009 
 -- Modified By: Gaurav R. Tanna - Bug: 26617
 -- Modify date: 22/07/2015
 -- =============================================
Create PROCEDURE [dbo].[USP_REP_AS_FORM24]
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

 Declare @MultiCo	VarChar(3)
 Declare @MCON as NVARCHAR(2000)

IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		 Set @MultiCo = 'YES'
		
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 --EXECUTE USP_REP_SINGLE_CO_DATA_VAT
		 -- @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 --,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 --,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 --,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 --,@MFCON = @MCON OUTPUT

		SELECT A.U_CHALNO, A.U_CHALDT, A.BANK_NM, M.S_TAX AS BRANCH,
	 	 (
	 	 SELECT IsNull(Sum(B.GRO_AMT),0) FROM BPMAIN B
	 	 INNER JOIN AC_MAST A1 ON (A1.AC_NAME = B.BANK_NM)
		 WHERE A.U_CHALNO = B.U_CHALNO AND A.U_CHALDT = B.U_CHALDT AND A.BANK_NM = B.BANK_NM  AND A1.S_TAX = M.S_TAX
		 AND (B.U_NATURE in (''))
		 ) AS TAXAMT,
		 (
		 SELECT IsNull(Sum(C.GRO_AMT),0) FROM BPMAIN C
		 INNER JOIN AC_MAST A2 ON (A2.AC_NAME = C.BANK_NM)
		 WHERE A.U_CHALNO = C.U_CHALNO AND A.U_CHALDT = C.U_CHALDT AND A.BANK_NM = C.BANK_NM AND A2.S_TAX = M.S_TAX
		 AND C.U_NATURE = 'PENALTY'
		 ) AS PENALTYAMT,
		 (
		 SELECT IsNull(Sum(D.GRO_AMT),0) FROM BPMAIN D
		 INNER JOIN AC_MAST A3 ON (A3.AC_NAME = D.BANK_NM)
		 WHERE A.U_CHALNO = D.U_CHALNO AND A.U_CHALDT = D.U_CHALDT AND A.BANK_NM = D.BANK_NM AND A3.S_TAX = M.S_TAX
		 AND D.U_NATURE = 'COMPOSITION MONEY'
		 ) AS COMPOSAMT,
		 (
		 SELECT IsNull(Sum(E.GRO_AMT),0) FROM BPMAIN E
		 INNER JOIN AC_MAST A4 ON (A4.AC_NAME = E.BANK_NM)
		 WHERE A.U_CHALNO = E.U_CHALNO AND A.U_CHALDT = E.U_CHALDT AND A.BANK_NM = E.BANK_NM AND A4.S_TAX = M.S_TAX
		 AND E.U_NATURE = 'INTEREST'
		 ) AS INTAMT,
		 (
		 SELECT IsNull(Sum(F.GRO_AMT),0) FROM BPMAIN F
		 INNER JOIN AC_MAST A5 ON (A5.AC_NAME = F.BANK_NM)
		 WHERE A.U_CHALNO = F.U_CHALNO AND A.U_CHALDT = F.U_CHALDT AND A.BANK_NM = F.BANK_NM AND A5.S_TAX = M.S_TAX
		 AND F.U_NATURE = 'SECURITY MONEY'
		 ) AS SECURAMT,
		 (
		 SELECT IsNull(Sum(G.GRO_AMT),0) FROM BPMAIN G
		 INNER JOIN AC_MAST A6 ON (A6.AC_NAME = G.BANK_NM)
		 WHERE A.U_CHALNO = G.U_CHALNO AND A.U_CHALDT = G.U_CHALDT AND A.BANK_NM = G.BANK_NM AND  A6.S_TAX = M.S_TAX
		 AND G.U_NATURE Not in ('', 'PENALTY','COMPOSITION MONEY','INTEREST','SECURITY MONEY')
		 ) AS OTHERAMT
		 FROM BPMAIN A
		 INNER JOIN AC_MAST M ON (A.BANK_NM = M.AC_NAME)
		 WHERE A.PARTY_NM LIKE '%VAT%' AND (A.DATE BETWEEN @SDATE AND @EDATE)
		 GROUP BY A.U_CHALNO, A.U_CHALDT, A.BANK_NM, M.S_TAX
	End 
 END

--Print 'AS VAT FORM 24'
