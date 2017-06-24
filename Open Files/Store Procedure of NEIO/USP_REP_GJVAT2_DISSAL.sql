If Exists(Select [name] From SysObjects Where xType='P' and [Name]='USP_REP_GJVAT2_DISSAL')
Begin
	Drop Procedure USP_REP_GJVAT2_DISSAL
End
Go
-- ==================a===========================
-- Author:		PANKAJ M. BORSE
-- Create date: 03/06/2010
-- Description:	This is useful for Brokerwise Sales Register report.
-- Modification Date/By/Reason:
-- Remark:
-- =============================================
create PROCEDURE [dbo].[USP_REP_GJVAT2_DISSAL]
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
DECLARE @SQLSTR NVARCHAR(500),@CNT INT,@VATCOL  VARCHAR(12),@TAX_NAME VARCHAR(12)
DECLARE @TAXAMT DECIMAL(12,2),@ADVATPER VARCHAR(10),@ADVATAMT DECIMAL(12,2),@ROW_NAME VARCHAR(20),@BASIC DECIMAL(12,2),@ENTRY_TY VARCHAR(2)

select entry_ty,vat=cast('' as varchar(20)),vatbasic=cast(0 as decimal(12,2)),vattax=cast(0 as decimal(12,2)),addvat=cast(0 as decimal(12,2)),cst=cast('' as varchar(20)),cstbasic=cast(0 as decimal(12,2)),csttax=cast(0 as decimal(12,2)),addcst=cast(0 as decimal(12,2)),TOTAL=cast(0 as decimal(12,2)),SALESPRP=cast(0 as decimal(12,2)),VATREV=cast(0 as decimal(12,2)),purbastot=cast(0 as decimal(12,2)),salesbasetot=cast(0 as decimal(12,2)),totsale=cast(0 as decimal(12,2)),saleEXTOT=cast(0 as decimal(12,2)) into #tax from stitem where 1=2

SET @ENTRY_TY='ST'
SET @CNT=1
	WHILE (@CNT<=2)
		BEGIN

			SET @SQLSTR='DECLARE UPDST CURSOR FOR select tax_name,sum(taxamt),advatper1,sum(addlvat1),(rtrim(tax_name)+'' + ''+ltrim(advatper1)),SUM(u_asseamt+tot_examt+tot_add-tot_deduc) from '+@ENTRY_TY+'item where (date between '''+cast(@sdate as varchar)+''' and '''+cast(@edate as varchar)+''' and tax_name<>'''') AND (TAX_NAME LIKE ''%VAT%'' or tax_name like ''%C.S.T.%'') group by tax_name,advatper1'
			EXECUTE SP_EXECUTESQL @SQLSTR

			OPEN UPDST
			FETCH NEXT FROM UPDST INTO @TAX_NAME,@TAXAMT,@ADVATPER,@ADVATAMT,@ROW_NAME,@BASIC
				WHILE @@FETCH_STATUS=0
					BEGIN

						IF @TAX_NAME LIKE '%VAT%'
							BEGIN 
								INSERT INTO #TAX(ENTRY_TY,vat,vatbasic,vattax,addvat,CST,CSTBASIC,CSTtax,addCST,TOTAL) VALUES(@ENTRY_TY,@ROW_NAME,@BASIC,@TAXAMT,@ADVATAMT,'',0,0,0,(@BASIC+@TAXAMT+@ADVATAMT))
							END 
						ELSE
							BEGIN 
								INSERT INTO #TAX(ENTRY_TY,vat,vatbasic,vattax,addvat,CST,CSTBASIC,CSTtax,addCST,TOTAL) VALUES(@ENTRY_TY,'',0,0,0,@ROW_NAME,@BASIC,@TAXAMT,@ADVATAMT,(@BASIC+@TAXAMT+@ADVATAMT))
							END 
		
			FETCH NEXT FROM UPDST INTO @TAX_NAME,@TAXAMT,@ADVATPER,@ADVATAMT,@ROW_NAME,@BASIC
		END
		CLOSE UPDST
		DEALLOCATE UPDST

SET @ENTRY_TY='PT'
SET @CNT=@CNT+1
END
update #tax SET purbastot=(SELECT SUM(vatbasic) FROM #TAX WHERE entry_ty='PT')
UPDATE #tax SET salesbasetot=(SELECT SUM(vatbasic) FROM #TAX WHERE entry_ty='ST')
UPDATE #tax SET totsale=(SELECT SUM(VATBASIC+CSTBASIC) FROM #TAX WHERE ENTRY_TY='ST')
UPDATE #tax set saleEXTOT=(SELECT SUM(cstbasic) FROM #TAX WHERE entry_ty='ST')
UPDATE #TAX SET SALESPRP=(SELECT sum(CSTBASIC) FROM #TAX WHERE ENTRY_TY='ST')/(SELECT SUM(VATBASIC+CSTBASIC) FROM #TAX WHERE ENTRY_TY='ST')*100
UPDATE #TAX SET VATREV=(((SELECT SUM(vatBASIC) FROM #TAX WHERE ENTRY_TY='PT')*SALESPRP/100)*2)/100

SELECT * FROM #tax order by entry_ty,vat desc
DROP TABLE #Tax

GO


