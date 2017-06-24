IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_EMP_TDS_CHALLAN_MENU]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[USP_REP_EMP_TDS_CHALLAN_MENU]
end
go
-- =============================================
-- Author:Rupesh		
-- Create date: 16/09/2012
-- Description:	This Stored procedure is useful to generate TDS Challan Report From Menu Option.
-- Modified By:Date:Reason: Rupesh. 17/03/2010. TKT-589. Changes done because it was giving wrong output. Refrence SP USP_REP_FORM26Q,USP_REP_FORM16.
-- Remark:
-- =============================================

CREATE Procedure [dbo].[USP_REP_EMP_TDS_CHALLAN_MENU]
@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),  
 @SDATE SMALLDATETIME,@EDATE SMALLDATETIME,  
 @SNAME NVARCHAR(60),@ENAME NVARCHAR(60),  
 @SITEM NVARCHAR(60),@EITEM NVARCHAR(60),  
 @SAMT NUMERIC,@EAMT NUMERIC,  
 @SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),  
 @SCAT NVARCHAR(60),@ECAT NVARCHAR(60),  
 @SWARE NVARCHAR(60),@EWARE NVARCHAR(60),  
 @SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),  
 @FINYR NVARCHAR(20), @EXPARA NVARCHAR(60)  
AS
begin

	Declare @FCON as VARCHAR(4000)
	set @FCon='m.Date between '+char(39)+cast(@sDate as varchar)+Char(39)+ ' and '+Char(39)+cast(@eDate as varchar)+Char(39)
	print @FCon
	EXECUTE Usp_Rep_Emp_TDS_Challan @FCon
End	

