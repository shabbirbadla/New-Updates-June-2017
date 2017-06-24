IF EXISTS (SELECT [NAME] FROM SYSOBJECTS WHERE [NAME]='USP_REP_OPENING_STOCK' AND XTYPE='P')
DROP PROC USP_REP_OPENING_STOCK
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
-- =============================================
-- Author:		Lokesh.
-- Create date: 25/04/2012
-- Description:	This Stored procedure is useful to generate Sales vs Purchase details.
-- Modify date: 
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================

--EXECUTE USP_REP_OPENING_STOCK '','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''

CREATE procedure [dbo].[USP_REP_OPENING_STOCK]
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
Begin
SELECT osmain.TRAN_CD,osmain.DATE,osmain.INV_NO,osmain.[rule],cast(osmain.narr as varchar(100)) as narr1,AC_MAST.MAILNAME,
IT_MAST.[GROUP],ositem.QTY,
ositem.RATE,ositem.GRO_AMT,ositem.item,ositem.batchno FROM osmain INNER JOIN ositem ON 
(osmain.TRAN_CD=ositem.TRAN_CD)INNER JOIN AC_MAST ON (AC_MAST.AC_ID=osmain.AC_ID)
INNER JOIN IT_MAST ON (IT_MAST.IT_CODE=ositem.IT_CODE)
ORDER BY osmain.DATE,osmain.INV_NO

End

--set ANSI_NULLS Off
--go
--set QUOTED_IDENTIFIER Off
--go
--
--EXECUTE USP_REP_SALES_VS_PURCHASE '','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''