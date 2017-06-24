IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Rep_FiltCond_Alert]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Rep_FiltCond_Alert]
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 16/05/2007
-- Description:	This Stored Procedure is useful to generate FILTER CONDITION string for Alert Related Stored Procedure.
-- Modification Date/By/Reason:
-- Remark:
-- =============================================
CREATE PROCEDURE [dbo].[Usp_Rep_FiltCond_Alert] 
@CommanPara varchar(8000) 
,@It_Mast varchar(30),@Ac_Mast varchar(30)
,@VMAINFILE AS VARCHAR(20),@VITFILE AS VARCHAR(20),@VACFILE AS VARCHAR(20)
,@vFcond_Alert NVARCHAR(2000) OUTPUT
AS

declare 
@VTMPAC NVARCHAR(50),@VTMPIT NVARCHAR(50),@VSPLCOND NVARCHAR(2000)
,@VSDATE AS SMALLDATETIME,@VEDATE AS SMALLDATETIME,
@VSAC AS VARCHAR(60),@VEAC AS VARCHAR(60)
,@VSIT AS VARCHAR(60),@VEIT AS VARCHAR(60)
,@VSAMT NUMERIC(17,2),@VEAMT FLOAT
,@VSDEPT AS VARCHAR(60),@VEDEPT AS VARCHAR(60)
,@VSCATE AS VARCHAR(60),@VECATE AS VARCHAR(60)
,@VSWARE AS VARCHAR(60),@VEWARE AS VARCHAR(60)
,@VSINV_SR AS VARCHAR(60),@VEINV_SR AS VARCHAR(60)
,@VDTFLD AS VARCHAR(20)
,@VLYN VARCHAR(20)
,@VEXPARA VARCHAR(100)

Set @VTMPAC=null
set @VTMPIT =null
set @VSPLCOND =''
Set @VSDATE =null
set @VEDATE =null
set @VSAC =null
set @VEAC =null
set @VSIT  =null
set @VEIT  =null
set @VSAMT =null
set @VEAMT  =null
set @VSDEPT =null
set @VEDEPT=null
set @VSCATE  =null
set @VECATE  =null
set @VSWARE  =null
set @VEWARE  =null
set @VSINV_SR  =null
set @VEINV_SR  =null
set @VMAINFILE  =null
set @VITFILE  =null
set @VACFILE  =null
set @VDTFLD  =null
set @VLYN  =null
set @VEXPARA  =null


declare @pos1 int,@pos2 int,@tempStr1 varchar(8000),@valstr varchar(1000)
if charindex('<<FrmAc=',@CommanPara)>0 and isnull(@Ac_Mast,'')<>''
begin
	set @pos1=charindex('<<FrmAc=',@CommanPara)
	set @valstr=substring(@CommanPara,@pos1,len(@CommanPara))
	set @pos1=charindex('>>',@valstr)
	set @valstr=substring(@valstr,1,@pos1+1)
	set @valstr=replace(@valstr,'<<FrmAc=','')
	set @valstr=replace(@valstr,'>>','')
	set @VSAC =@valstr
end
if charindex('<<ToAc=',@CommanPara)>0 and isnull(@Ac_Mast,'')<>''
begin
	set @pos1=charindex('<<ToAc=',@CommanPara)
	set @valstr=substring(@CommanPara,@pos1,len(@CommanPara)-@pos1+1)
	set @pos1=charindex('>>',@valstr)
	set @valstr=substring(@valstr,1,@pos1+1)
	set @valstr=replace(@valstr,'<<ToAc=','')
	set @valstr=replace(@valstr,'>>','')
	set @VEAC =@valstr
	--print @valstr
end
if charindex('<<FrmIt=',@CommanPara)>0 and isnull(@It_Mast,'')<>''
begin
	set @pos1=charindex('<<FrmIt=',@CommanPara)
	set @valstr=substring(@CommanPara,@pos1,len(@CommanPara)-@pos1)
	set @pos1=charindex('>>',@valstr)
	set @valstr=substring(@valstr,1,@pos1+1)
	set @valstr=replace(@valstr,'<<FrmIt=','')
	set @valstr=replace(@valstr,'>>','')
	set @VSIT =@valstr
	--print @valstr
end
if charindex('<<ToIt=',@CommanPara)>0 and isnull(@It_Mast,'')<>''
begin
	set @pos1=charindex('<<ToIt=',@CommanPara)
	set @valstr=substring(@CommanPara,@pos1,len(@CommanPara)-@pos1+1)
	set @pos1=charindex('>>',@valstr)
	set @valstr=substring(@valstr,1,@pos1+1)
	set @valstr=replace(@valstr,'<<ToIt=','')
	set @valstr=replace(@valstr,'>>','')
	set @VEIT =@valstr
	--print @valstr
end


DECLARE @TBLCON VARCHAR(200),@WHCON VARCHAR(3000)
SET @TBLCON=' '
SET @WHCON=' '

IF (@VTMPAC IS NOT NULL AND @VTMPAC<>' ') 
BEGIN
	SET @TBLCON=RTRIM(@TBLCON)+' INNER JOIN '+RTRIM(@VTMPAC)+' ON ('+RTRIM(@VTMPAC)+'.AC_NAME=AC_MAST.AC_NAME)'
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
	SET @WHCON=RTRIM(@WHCON)+' ('+rtrim(@Ac_Mast)+'.AC_NAME BETWEEN '+CHAR(39)+RTRIM(@VSAC)+CHAR(39)+'  AND '+CHAR(39)+RTRIM(@VEAC)+CHAR(39)+' )'
END
ELSE
BEGIN
	IF (@VSAC IS NOT NULL AND @VSAC<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+rtrim(@Ac_Mast)+'.AC_NAME='+CHAR(39)+RTRIM(@VSAC)+CHAR(39)+' )'
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
END
ELSE
BEGIN
	IF (@VSAMT IS NOT NULL AND @VSAMT<>0 AND @VACFILE IS NOT NULL AND @VACFILE<>' ') 
	BEGIN
		SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
		SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM(@VACFILE)+'.AMOUNT='+RTRIM(@VSAMT)+' )'
	END
END

IF (@VLYN IS NOT NULL AND @VLYN<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET @WHCON=RTRIM(@WHCON)+' ('+RTRIM((CASE WHEN @VMAINFILE<>' ' THEN @VMAINFILE ELSE (CASE WHEN @VITFILE<>' ' THEN @VITFILE ELSE @VACFILE END ) END))+'.l_YN = ' +CHAR(39)+@VLYN+CHAR(39)+')'
END



IF (@VSPLCOND IS NOT NULL AND @VSPLCOND<>' ') 
BEGIN
	SET @WHCON=RTRIM(@WHCON)+CASE WHEN CHARINDEX('WHERE',@WHCON)=0 THEN ' WHERE  ' ELSE ' AND ' END
	SET SET QUOTED_IDENTIFIER  OFF
	SET @VSPLCOND=REPLACE(@VSPLCOND, '`','''')
	SET SET QUOTED_IDENTIFIER  ON
END
SET @vFcond_Alert=RTRIM(@TBLCON)+' '+RTRIM(@WHCON)+' '+RTRIM(@VSPLCOND)
print ' @vFcond_Alert  '+ @vFcond_Alert







