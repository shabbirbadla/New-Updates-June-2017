set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER   PROCEDURE [dbo].[USP_REP_AC_LIST]
@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
@SAMT NUMERIC,@EAMT NUMERIC,
@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
@FINYR NVARCHAR(20), @EXTPAR NVARCHAR(60)

AS
	SET NOCOUNT ON
	--------- STORED PROC FOR ACCOUNT MASTER LIST ----------
	DECLARE @QryStr NVARCHAR(4000)

	SET @QryStr='select ac_name,[group],contact,add1,add2,city,zip,phone,phoner,fax,email,cr_days,i_tax from ac_mast'

	IF @TMPAC<>''			---TABLE CONTAINING AC_NAMES IN CASE TMPAC(AC GROUP) NOT BLANK
	BEGIN
		SET @QryStr=rtrim(@QryStr)+CHAR(13)+'where ac_mast.ac_name in (select acname from  '+@TMPAC+')'
			+CHAR(13)+'order by ac_mast.ac_name'
	END 
	ELSE
	BEGIN
		SET @QryStr=rtrim(@QryStr)+CHAR(13)+'where ac_mast.ac_name between '+CHAR(39)+@SNAME+CHAR(39)+' and '+CHAR(39)+@ENAME+CHAR(39)
			+CHAR(13)+'order by ac_name'
	END
--PRINT @QryStr
execute sp_executesql @QryStr
