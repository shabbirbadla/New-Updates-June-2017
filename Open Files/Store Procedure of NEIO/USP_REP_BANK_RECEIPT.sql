IF EXISTS (SELECT [NAME] FROM SYSOBJECTS WHERE [NAME]='USP_REP_BANK_RECEIPT' AND XTYPE='P')
DROP PROC USP_REP_BANK_RECEIPT
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
-- =============================================
-- Author:		Lokesh.
-- Create date: 25/04/2012
-- Description:	This Stored procedure is useful to generate Sales vs Purchase details.
-- Modify By/date/Remark: Shrikant S. on 25/06/2013 for Bug-16243
-- =============================================

--EXECUTE USP_REP_BANK_RECEIPT '','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''

CREATE procedure [dbo].[USP_REP_BANK_RECEIPT]
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
SELECT entry_ty,date,doc_no,party_nm,inv_no,inv_sr,gro_amt,cast(narr as varchar(100)) as narr,cheq_no,bank_nm,drawn_on,slipno=0
,tdspaytype --Added By Shrikant S. on 25/06/2013 for Bug-16243
,Tran_cd,Ac_id,Bk_id,CompId
FROM BRMAIN
Where date Between  @SDATE and @EDATE		--Added By Shrikant S. on 25/06/2013 for Bug-16243
End

--set ANSI_NULLS Off
--go
--set QUOTED_IDENTIFIER Off
--go
--
--EXECUTE USP_REP_BANK_RECEIPT '','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''