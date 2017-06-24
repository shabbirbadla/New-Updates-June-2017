IF EXISTS (SELECT [NAME] FROM SYSOBJECTS WHERE [NAME]='USP_REP_WIP_IN' AND XTYPE='P')
DROP PROC USP_REP_WIP_IN
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

--EXECUTE USP_REP_WIP_IN '','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''

CREATE procedure [dbo].[USP_REP_WIP_IN]
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
SELECT ipmain.TRAN_CD,ipmain.DATE,ipmain.INV_NO,ipmain.[rule],AC_MAST.MAILNAME,
IT_MAST.[GROUP],ipitem.QTY,
ipitem.RATE,ipitem.GRO_AMT,ipitem.ITEM,ipitem.batchno FROM ipmain INNER JOIN ipitem ON 
(ipmain.TRAN_CD=ipitem.TRAN_CD)INNER JOIN AC_MAST ON (AC_MAST.AC_ID=ipmain.AC_ID)
INNER JOIN IT_MAST ON (IT_MAST.IT_CODE=ipitem.IT_CODE)
where mailname <> 'USE FOR PRODUCTION'   
ORDER BY ipmain.DATE,ipmain.INV_NO

End

--set ANSI_NULLS Off
--go
--set QUOTED_IDENTIFIER Off
--go
--
--EXECUTE USP_REP_WIP_IN '','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''