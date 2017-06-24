/****** Object:  StoredProcedure [dbo].[USP_REP_FILTCON]    Script Date: 03/06/2010 14:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 16/05/2007
-- Description:	This Stored Procedure is useful to generate FILTER CONDITION string.
-- Modification Date/By/Reason: 03/09/2009 Rupesh Prajapati. Modified for @VSAMT.
-- Remark:
-- =============================================
CREATE     PROCEDURE [dbo].[USP_REP_FILTCON] 
@VTMPAC NVARCHAR(50),@VTMPIT NVARCHAR(50),@VSPLCOND NVARCHAR(2000)
,@VSDATE AS SMALLDATETIME,@VEDATE AS SMALLDATETIME
,@VSAC AS VARCHAR(60),@VEAC AS VARCHAR(60)
,@VSIT AS VARCHAR(60),@VEIT AS VARCHAR(60)
,@VSAMT NUMERIC(17,2),@VEAMT FLOAT
,@VSDEPT AS VARCHAR(60),@VEDEPT AS VARCHAR(60)
,@VSCATE AS VARCHAR(60),@VECATE AS VARCHAR(60)
,@VSWARE AS VARCHAR(60),@VEWARE AS VARCHAR(60)
,@VSINV_SR AS VARCHAR(60),@VEINV_SR AS VARCHAR(60)
,@VMAINFILE AS VARCHAR(20),@VITFILE AS VARCHAR(20),@VACFILE AS VARCHAR(20)
,@VDTFLD AS VARCHAR(20)
,@VLYN VARCHAR(20)
,@VEXPARA VARCHAR(100) =NULL
,@VFCON NVARCHAR(2000) OUTPUT
AS

--@TMPAC=Cursor Name for Account names selected in GROUP,ACCOUNT OPTION TAB
--@TMPIT=Cursor Name for Item names selected in ITEM OPTION TAB
--@SPLCOND=Special Condition From R_Staus Table
--@SDATE=Starting Date & EDATE=Ending Date
--@SAC=Starting Accont name & EAC=Ending Accont Name
--@SIT=Starting Accont name & EIT=Ending Item Name
--,@SAMT =Starting Amount & EAMT=Maximum Amount
--@SDEPT=Starting Department name & EAC=Ending Department Name
--@SCATE=Starting Cate  ry name & ECATE=Ending Cate  ry Name
--@SWARE=Starting Ware House Name  & EWARE=Ending Ware House Name 
--@SINVSR=Starting Ware Invoice Series  & EINVSR=Ending Invoice Series
--@MAINFILE =Main Filter File for Departments,Cate  ries,Invoice Series,[Date]
--@ITFILE  =Item Filter File for Ware House [Date]
--@DTFLD=Date field.


DECLARE @TBLCON VARCHAR(200),@WHCON VARCHAR(3000)
SET @TBLCON=' '
SET @WHCON=' '

IF (@VTMPAC IS NOT NULL AND @VTMPAC<>' ') 
BEGIN
	SET @TBLCON=RTRIM(@TBLCON)+' INNER JOIN '+RTRIM(@VTMPAC)+' ON ('+RTRIM(@VTMPAC)+'.AC_NAME=AC_MAST.AC_NAME)'  --VASANT AC_NAME changed to ACNAME
--	SET @TBLCON=RTRIM(@TBLCON)+' INNER JOIN '+RTRIM(@VTMPAC)+' ON ('+RTRIM(@VTMPAC)+'.ACNAME=AC_MAST.AC_NAME)'
END	
IF (@VTMPIT IS NOT NULL AND @VTMPIT<>' ') 
BEGIN
	SET @TBLCON=RTRIM(@TBLCON)+' '+' INNER JOIN '+RTRIM(@VTMPIT)+' ON ('+RTRIM(@VTMPIT)+'.IT_NAME=IT_MAST.IT_NAME)'
END

IF YEAR(@VEDATE)>1900  
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	IF @VSDATE IS NULL 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE (CASE WHEN @VITFILE<>' ' THEN @VITFILE ELSE @VACFILE END ) END))+'.'+@VDTFLD+'< ='+CHAR(39)+CAST(@VEDATE AS VARCHAR)+CHAR(39)+' )'			
	END
	ELSE
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE (CASE WHEN @VITFILE<>' ' THEN @VITFILE ELSE @VACFILE END ) END))+'.'+@VDTFLD+' BETWEEN '+CHAR(39)+CAST(@VSDATE AS VARCHAR)+CHAR(39)+'  AND '+CHAR(39)+CAST(@VEDATE AS VARCHAR)+CHAR(39)+' ) '
	END
END
ELSE
BEGIN
	IF YEAR(@VSDATE)>1900
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.DATE< ='+CHAR(39)+CAST(@VSDATE AS VARCHAR)+CHAR(39)+' )'
	END	
END


IF (@VEAC IS NOT NULL AND @VEAC<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' (AC_MAST.AC_NAME BETWEEN '+CHAR(39)+RTRIM(@VSAC)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VEAC)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSAC IS NOT NULL AND @VSAC<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' (AC_MAST.AC_NAME='+CHAR(39)+RTRIM(@VSAC)+CHAR(39)+' )'
	END
END


IF (@VEIT IS NOT NULL AND @VEIT<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' (IT_MAST.IT_NAME BETWEEN '+CHAR(39)+RTRIM(@VSIT)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VEIT)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSIT IS NOT NULL AND @VSIT<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' (IT_MAST.IT_NAME='+CHAR(39)+RTRIM(@VSIT)+CHAR(39)+' )'
	END
END

IF (@VEDEPT IS NOT NULL AND @VEDEPT<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.DEPT BETWEEN '+CHAR(39)+RTRIM(@VSDEPT)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VEDEPT)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSDEPT IS NOT NULL AND @VSDEPT<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.DEPT='+CHAR(39)+RTRIM(@VSDEPT)+CHAR(39)+' )'
	END
END

IF (@VECATE IS NOT NULL AND @VECATE<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.CATE BETWEEN '+CHAR(39)+RTRIM(@VSCATE)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VECATE)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSCATE IS NOT NULL AND @VSCATE<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.CATE='+CHAR(39)+RTRIM(@VSCATE)+CHAR(39)+' )'
	END
END

IF (@VEWARE IS NOT NULL AND @VEWARE<>' ' AND @VITFILE IS NOT NULL  AND @VITFILE<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM(@VITFILE)+'.WARE_NM BETWEEN '+CHAR(39)+RTRIM(@VSWARE)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VEWARE)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSWARE IS NOT NULL AND @VSWARE<>' ' AND @VITFILE IS NOT NULL  AND @VITFILE<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM(@VITFILE)+'.WARE_NM='+CHAR(39)+RTRIM(@VSCATE)+CHAR(39)+' )'
	END
END

IF (@VEINV_SR IS NOT NULL AND @VEINV_SR<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.INV_SR BETWEEN '+CHAR(39)+RTRIM(@VSINV_SR)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VEINV_SR)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSINV_SR IS NOT NULL AND @VSINV_SR<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE @VITFILE END))+'.INV_SR='+CHAR(39)+RTRIM(@VSINV_SR)+CHAR(39)+' )'
	END
END

IF (@VEAMT IS NOT NULL AND @VEAMT<>0  AND @VACFILE IS NOT NULL AND @VACFILE<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM(@VACFILE)+'.AMOUNT BETWEEN '+RTRIM(@VSAMT)+'  AND '+RTRIM(@VEAMT)+' )'
	--+' (' VASANT
END
ELSE
BEGIN
	IF (@VSAMT IS NOT NULL AND @VSAMT<>0 AND @VACFILE IS NOT NULL AND @VACFILE<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM(@VACFILE)+'.AMOUNT='+RTRIM(@VSAMT)+' )'
	END
	--+' (' VASANT
END

IF (@VLYN IS NOT NULL AND @VLYN<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE (CASE WHEN @VITFILE<>' ' THEN @VITFILE ELSE @VACFILE END ) END))+'.l_YN = ' +CHAR(39)+@VLYN+CHAR(39)+')'
END



IF (@VSPLCOND IS NOT NULL AND @VSPLCOND<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET QUOTED_IDENTIFIER  OFF
	SET @VSPLCOND=REPLACE(@VSPLCOND, '`','''')
	SET QUOTED_IDENTIFIER  ON
END

SET @VFCON=RTRIM(@TBLCON)+' '+RTRIM(@WHCON)+' '+RTRIM(@VSPLCOND)
--PRINT @VFCON	VASANT





