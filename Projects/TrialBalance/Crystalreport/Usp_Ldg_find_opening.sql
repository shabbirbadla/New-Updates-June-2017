/*:*****************************************************************************
*:       Program: USP_FINAL_ACCOUNTS
*:        System: UDYOG Software (I) Ltd.
*:    Programmer: RAGHAVENDRA B. JOSHI
*: Last modified: 19/09/2006
*:		AIM		: Find Accounts Opening balance
**:******************************************************************************/
ALTER PROCEDURE [dbo].[Usp_Ldg_find_opening]
@LedgerName Varchar(100), @FDate SMALLDATETIME,@TDate SMALLDATETIME,@C_St_Date SMALLDATETIME
As

If @FDate IS NULL OR @TDate IS NULL OR @C_St_Date IS NULL OR @FDate = '' OR @TDate = '' OR @C_St_Date = '' OR @LedgerName IS NULL OR @LedgerName = ''
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End

/* Internale Variable declaration and Assigning [Start] */
Declare @Balance Numeric(17,2),@TBLNM VARCHAR(50),@TBLNAME1 Varchar(50),
	@TBLNAME2 Varchar(50),@TBLNAME3 Varchar(50),@TBLNAME4 Varchar(50),
	@SQLCOMMAND as NVARCHAR(4000)

Select @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
		+ (DATEPART(ss, GETDATE()) * 1000 )+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No),
		@Balance = 0,@SQLCOMMAND = ''

Select @TBLNAME1 = '##TMP1'+@TBLNM,@TBLNAME2 = '##TMP2'+@TBLNM
Select @TBLNAME3 = '##TMP3'+@TBLNM,@TBLNAME4 = '##TMP4'+@TBLNM
/* Internale Variable declaration and Assigning [End] */

/* Collecting Data from accounts details and create table [Start] */
SET @SQLCOMMAND = 'SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,
		MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME
		INTO '+@TBLNAME1+' FROM LAC_VW AVW (NOLOCK)
		INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID
		INNER JOIN LMAIN_VW MVW (NOLOCK) 
		ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY
		WHERE (MVW.DATE < = '''+CONVERT(VARCHAR(50),@TDate)+''' ) AND AC_MAST.AC_NAME = '''+@LedgerName+''''
EXECUTE sp_executesql @SQLCOMMAND
/* Collecting Data from accounts details and create table [End] */

/*---Tmp Code
SET @SQLCOMMAND = 'SELECT * FROM '+@TBLNAME1
EXECUTE sp_executesql @SQLCOMMAND
*/

/*Remove Trading and Profit loss Previous Entry [Start]*/
SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY IN 
	(SELECT CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY AS COMEID FROM '+@TBLNAME1+' WHERE [TYPE] IN (''T'',''P'') 
	AND [DATE] NOT BETWEEN '''+CONVERT(VARCHAR(50),@C_St_Date)+''' AND '''+CONVERT(VARCHAR(50),@TDate)+''')'
EXECUTE sp_executesql @SQLCOMMAND
/*Remove Trading and Profit loss Previous Entry [End]*/

/*---Tmp Code
SET @SQLCOMMAND = 'SELECT * FROM '+@TBLNAME1
EXECUTE sp_executesql @SQLCOMMAND
*/

/* Removing carry-forwarded records [Start] */
SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE 
		DATE < (SELECT TOP 1 DATE FROM '+@TBLNAME1+'
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'') AND DATE = '''+CONVERT(VARCHAR(50),@C_St_Date)+''')
		AND AC_NAME IN (SELECT AC_NAME FROM '+@TBLNAME1+'
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'') AND DATE = '''+CONVERT(VARCHAR(50),@C_St_Date)+''' GROUP BY AC_NAME)'
EXECUTE sp_executesql @SQLCOMMAND

/* Removing carry-forwarded records [End] */
SET @SQLCOMMAND = 'SELECT TRAN_CD=0,ENTRY_TY='' '',
	DATE = '''+CONVERT(VARCHAR(50),@FDate)+''',
	bedit=ISNULL(SUM(CASE WHEN TVW.AMT_TY = ''DR'' THEN TVW.AMOUNT ELSE 0 END),0),
	credit=ISNULL(SUM(CASE WHEN TVW.AMT_TY = ''CR'' THEN TVW.AMOUNT ELSE 0 END),0),
	TVW.AC_ID,TVW.AC_NAME,AMT_TY=''A'',INV_NO='' ''
	INTO '+@TBLNAME2+' FROM '+@TBLNAME1+' TVW
	WHERE (TVW.DATE < '''+CONVERT(VARCHAR(50),@FDate)+'''
	OR TVW.ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB'' OR Entry_Ty = ''OB'')) 
	GROUP BY TVW.AC_ID,TVW.AC_NAME'
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'SELECT a.Ac_id,
	bedit = isnull(CASE Amt_Ty WHEN ''A'' THEN SUM(a.bedit)END,0),
	credit = isnull(CASE Amt_Ty WHEN ''A'' THEN SUM(a.credit)END,0)
	from '+@TBLNAME2+' a
	group by a.Ac_id,a.amt_ty'
EXECUTE sp_executesql @SQLCOMMAND

/* Droping Temp tables [Start] */
SET @SQLCOMMAND = 'Drop table '+@TBLNAME1
PRINT @SQLCOMMAND
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME2
EXECUTE sp_executesql @SQLCOMMAND
/* Droping Temp tables [End] */
