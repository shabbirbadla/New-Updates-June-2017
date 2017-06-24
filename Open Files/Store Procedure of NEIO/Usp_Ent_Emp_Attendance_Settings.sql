IF EXISTS( Select [Name] from sysobjects where [Name]='USP_ENT_EMP_ATTENDANCE_SETTINGS')
BEGIN
DROP PROCEDURE USP_ENT_EMP_ATTENDANCE_SETTINGS
END


-- =============================================
-- Author: pratap
-- Create date: 14-06-2012
-- Description:	This is useful for Employee Attendance Settings Master 
-- Modify date: 
-- Remark:
-- =============================================
GO
CREATE PROCEDURE [dbo].[USP_ENT_EMP_ATTENDANCE_SETTINGS]

 @Att_Code VARCHAR(30),
 @Name VARCHAR(50),
 
 @Loc_Code VARCHAR(50),
 @Dept VARCHAR(30),
 @Cate VARCHAR(30)
as

   DECLARE @WHERECOND NVARCHAR(200)
   DECLARE @SQLCOMMAND NVARCHAR(400)

   
SET @WHERECOND=''
IF(ISNULL(@Att_Code,'')!='')
BEGIN
   SET @WHERECOND=@WHERECOND+' AND  Att_Code='+CHAR(39)+@Att_Code+CHAR(39)
END
print @WHERECOND
IF(ISNULL(@Name,'')!='')
BEGIN
   SET @WHERECOND=@WHERECOND+' AND  ATT_NM='+CHAR(39)+@Name+CHAR(39)

END

print @WHERECOND
IF(ISNULL(@Loc_Code,'')!='')
BEGIN
  SET @WHERECOND=@WHERECOND+' AND L.Loc_Desc='+CHAR(39)+@Loc_Code+CHAR(39)
SET @WHERECOND=@WHERECOND+' AND a.Loc_code=l.loc_code'
END
print @WHERECOND
IF(ISNULL(@Dept,'')!='')
BEGIN
  SET @WHERECOND=@WHERECOND+' AND  DEPT='+CHAR(39)+@Dept+CHAR(39)
END
print @WHERECOND
IF(ISNULL(@Cate,'')!='')
BEGIN
  SET @WHERECOND=@WHERECOND+' AND  CATE='+CHAR(39)+@Cate+CHAR(39)
END

print @WHERECOND
 
SET @SQLCOMMAND='SELECT A.*, L.Loc_desc FROM EMP_ATTENDANCE_SETTING A left join Loc_Master L  on (a.Loc_code=l.loc_code) WHERE 1=1'+@WHERECOND
SET @SQLCOMMAND=@SQLCOMMAND +' order by A.Att_Code'

print @SQLCOMMAND
EXEC SP_EXECUTESQL @SQLCOMMAND






