IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='USP_REP_EMP_PAYROLL_POSTING')
BEGIN
DROP PROCEDURE USP_REP_EMP_PAYROLL_POSTING
END
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 
-- Description:	This Stored procedure is useful to generate Payroll Account Posting.
-- Modification Date/By/Reason: 
-- Remark:
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_EMP_PAYROLL_POSTING]
@ENTRYCOND NVARCHAR(254)
AS
Begin
	SET QUOTED_IDENTIFIER OFF
	DECLARE @SQLCOMMAND NVARCHAR(4000),@FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)
	Select Tran_cd into #TDSChal From CRItem where 1=2
	
		declare @ent varchar(2),@trn int,@pos1 int,@pos2 int,@pos3 int--,@ENTRYCOND NVARCHAR(254)
		/*--->Entry_Ty and Tran_Cd Separation*/
		print @ENTRYCOND
		set @pos1=charindex('''',@ENTRYCOND,1)+1
		set @ent= substring(@ENTRYCOND,@pos1,2)
		set @pos2=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos1))+1
		set @pos3=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos2))+1
		set @trn= substring(@ENTRYCOND,@pos2,@pos2-@pos3)
		print 'ent '+ @ent
		print @trn
		/*<---Entry_Ty and Tran_Cd Separation*/
		
		
	SELECT EPMAIN.ENTRY_TY, EPMAIN.INV_NO,EPMAIN.CHEQ_NO,EPMAIN.INV_SR,EPMAIN.DATE,EPMAIN.USER_NAME,EPMAIN.NET_AMT,EPACDET.AC_NAME,EPACDET.AMT_TY, EPACDET.AMOUNT, CONVERT(VARCHAR(254),CAST(EPMAIN.NARR AS VARCHAR(4000))) AS NARR,AC_MAST.AC_NAME as ac_name1,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.PHONE,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'')='' THEN ac_mast.ac_name ELSE ac_mast.mailname END) 
	FROM EPMAIN 
	INNER JOIN EPACDET ON (EPMAIN.TRAN_CD=EPACDET.TRAN_CD) 
	INNER JOIN AC_MAST ON (AC_MAST.AC_ID=EPACDET.AC_ID) 
	WHERE  EPMAIN.Tran_Cd=@trn
end


