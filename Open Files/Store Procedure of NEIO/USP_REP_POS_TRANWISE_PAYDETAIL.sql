-- =============================================
-- AUTHOR:		ARCHANA K.
-- CREATE DATE: 17 APR, 2013
-- DESCRIPTION:	THIS REPORT WILL LIST THE TRANSACTION OF POINT OF SALE DONE AND THEIR VARIOUS PAYMENT DETAILS AGAINST EACH TRANSACTION/BILL.

IF EXISTS(SELECT [NAME] FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='USP_REP_POS_TRANWISE_PAYDETAIL')
BEGIN
	DROP PROCEDURE USP_REP_POS_TRANWISE_PAYDETAIL
END
GO

CREATE PROCEDURE [DBO].[USP_REP_POS_TRANWISE_PAYDETAIL]
@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SINV_SR AS VARCHAR(20),@EINV_SR AS VARCHAR(20)
,@SDEPT AS VARCHAR(20),@EDEPT AS VARCHAR(20)
,@SCATE AS VARCHAR(20),@ECATE AS VARCHAR(20)
,@SINV_NO AS VARCHAR(15),@EINV_NO AS VARCHAR(15)
AS

DECLARE @SQLCOMMAND NVARCHAR(4000)
SET @SQLCOMMAND='SELECT DI.DATE,DI.INV_NO,DM.INV_SR,DI.ITSERIAL,DM.DEPT,DM.CATE,DI.ITEM,DI.QTY,DI.RATE,DI.U_ASSEAMT,DI.TAXPERCENT,DI.TAXAMT,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'DI.GRO_AMT,PS.PAYMODE,CASHAMT=CASE WHEN PS.PAYMODE=''CASH'' THEN TOTALVALUE ELSE 0 END,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'COUPONAMT=CASE WHEN PS.PAYMODE=''COUPON'' THEN TOTALVALUE ELSE 0 END,CHEQUEAMT=CASE WHEN PS.PAYMODE=''CHEQUE'' THEN TOTALVALUE ELSE 0 END,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'CARDAMT=CASE WHEN PS.PAYMODE=''CARD'' THEN TOTALVALUE ELSE 0 END,DM.TOTALPAID,DM.BALAMT ,DM.ROUNDOFF'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'FROM DCMAIN DM'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN DCITEM DI ON (DM.ENTRY_TY=DI.ENTRY_TY AND DM.TRAN_CD=DI.TRAN_CD)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN PSPAYDETAIL PS ON (DM.INV_NO=PS.INV_NO)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'WHERE DI.DATE BETWEEN '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' AND DM.INV_SR BETWEEN '''+@SINV_SR+''' AND '''+@EINV_SR+''' AND DM.DEPT BETWEEN '''+@SDEPT+''' AND '''+@EDEPT+''''
SET @SQLCOMMAND=@SQLCOMMAND+' '+'AND DM.CATE BETWEEN '''+@SCATE+''' AND '''+@ECATE+''' AND DM.INV_NO BETWEEN '''+@SINV_NO+''' AND '''+@EINV_NO+''''
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND


