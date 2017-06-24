If Exists(Select [name] from sysobjects Where xType='P' and [Name]='Usp_Final_Accounts')
Begin
	Drop Procedure Usp_Final_Accounts
End
Go

/*:*****************************************************************************
*:       Program: USP_FINAL_ACCOUNTS
*:        System: UDYOG Software (I) Ltd.
*:    Programmer: RAGHAVENDRA B. JOSHI
*: Last modified: 14/05/2009
*:		AIM		: Maintain Final Acounts reports Like
*:				  [Trial Balance, Profit and Loss Accounts and Balance Sheet]
*Changes done on 02/11/2009 by vasant for query no. Query L1S-16
-- Modification Date/By/Reason: 08/06/2010. Rupesh Prajapati. Parameter for Department and Category are added TKT-1129.
-- Modified By Sachin N. S. on 31/07/2010 for TKT-3162 -- Balance sheet was not tallying for closing stock a/c shown in the previous year entries.
-- Modified By Amrendra on 31/05/2011 for TKT-7541 -- P P L Ac Opening balance calculation problem.
-- Modified by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
-- Modified by Shrikant on 10/06/2013 as per Bug 548
-- Modified by vasant on 25/11/2013 as per Bug 20309 (Require Bifurcation of 'General Opening & Closing Stock' and 'Consignment Opening & Closing Stock' in Final and Stock Valuation Reports). Comment Remark = --Bug20309
-- Modified by vasant on 16/01/2014 as per Bug 21282 (Opening Stock not appearing in trail balance/profit and loss/balance sheet). Comment Remark = --Bug21282
-- Modified by Shrikant S. on 04/03/2014 (for Negative Balance Sheet Issue). for Bug-19694
-- Modified by Shrikant S. on 25/09/2014 for Bug-24146
**:******************************************************************************/

Create PROCEDURE [dbo].[Usp_Final_Accounts]
@FDate SMALLDATETIME,@TDate SMALLDATETIME,@C_St_Date SMALLDATETIME,@reporttype varchar(1)
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)

As
--@reporttype varchar(1)	vasant021109
If @FDate IS NULL OR @TDate IS NULL OR @C_St_Date IS NULL
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End
If @FDate = '' OR @TDate = '' OR @C_St_Date = ''
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End

if (@SDEPT is null OR @EDEPT IS NULL OR @SCATE IS NULL OR @ECATE IS NULL) /*TKT-1129*/
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End

/* Internale Variable declaration and Assigning [Start] */
Declare @Balance Numeric(17,2),@TBLNM VARCHAR(50),@TBLNAME1 Varchar(50),
	@TBLNAME2 Varchar(50),@TBLNAME3 Varchar(50),@TBLNAME4 Varchar(50),
	@SQLCOMMAND as NVARCHAR(4000),@TIME SMALLDATETIME

Declare @TBLNAME5 Varchar(50)			--Added by Shrikant S. on 24/02/2014 for Bug-19694

Select @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
		+ (DATEPART(ss, GETDATE()) * 1000 )+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No),
		@Balance = 0,@SQLCOMMAND = ''

Select @TBLNAME1 = '##TMP1'+@TBLNM,@TBLNAME2 = '##TMP2'+@TBLNM
Select @TBLNAME3 = '##TMP3'+@TBLNM,@TBLNAME4 = '##TMP4'+@TBLNM
Select @TBLNAME5='##TMP5'+@TBLNM		--Added by Shrikant S. on 24/02/2014 for Bug-19694
/* Internale Variable declaration and Assigning [End] */

Select * into ##STKVALConfig from StkValConfig		--Bug20309

/* Collecting Data from accounts details and create table [Start] */
SET @SQLCOMMAND = 'SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,
		MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME,AVW.ACSERIAL
		INTO '+@TBLNAME1+' FROM LAC_VW AVW (NOLOCK)
		INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID
		INNER JOIN LMAIN_VW MVW (NOLOCK) 
		ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY
		WHERE (MVW.DATE < = '''+CONVERT(VARCHAR(50),@TDate)+''' )'

if (@EDEPT<>'') /*TKT-1129*/
begin
	SET @SQLCOMMAND =rtrim(@SQLCOMMAND)+' '+' and ('+'MVW.DEPT between '''+@SDEPT+''' and '''+@EDEPT+''')'/* TKT-1129*/
end

if (@ECATE<>'') /*TKT-1129*/
begin
	SET @SQLCOMMAND =rtrim(@SQLCOMMAND)+' '+' and ('+'MVW.Cate between '''+@SCATE+''' and '''+@ECATE+''')'/* TKT-1129*/
end

EXECUTE sp_executesql @SQLCOMMAND
/* Collecting Data from accounts details and create table [End] */

/***** Added By Sachin N. S. on 31/07/2010 for TKT-3162 --- Start *****/
--Bug20309
Declare @Stk_OpAccounts Varchar(100),@Stk_ClAccounts Varchar(100)  
Set @Stk_ClAccounts = ''
DECLARE CSTKVAL CURSOR FOR 
SELECT ClB_AcName FROM ##STKVALConfig
OPEN CSTKVAL
FETCH NEXT FROM CSTKVAL INTO @Stk_ClAccounts
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE AC_NAME = '''+@Stk_ClAccounts+''' AND [DATE] < '''+CONVERT(VARCHAR(50),@C_St_Date-1)+''' '	
	EXECUTE sp_executesql @SQLCOMMAND 

	FETCH NEXT FROM CSTKVAL INTO @Stk_ClAccounts
END
CLOSE CSTKVAL
DEALLOCATE CSTKVAL

--SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE AC_NAME = ''CLOSING STOCK'' AND [DATE] < '''+CONVERT(VARCHAR(50),@C_St_Date-1)+''' '
--EXECUTE sp_executesql @SQLCOMMAND 
--Bug20309
/***** Added By Sachin N. S. on 31/07/2010 for TKT-3162 --- End *****/

--SET @SQLCOMMAND = 'UPDATE '+@TBLNAME1+' SET AC_NAME = ''OPENING STOCK'', AC_ID=(SELECT AC_ID FROM AC_MAST WHERE AC_NAME = ''OPENING STOCK'') WHERE AC_NAME = ''CLOSING STOCK'' AND [DATE] < '''+CONVERT(VARCHAR(50),@C_St_Date)+''' '		--Changed By Sachin N. S. on 31/07/2010 for TKT-3162
--Bug21282
--SET @SQLCOMMAND = 'UPDATE '+@TBLNAME1+' SET AC_NAME = ''OPENING STOCK'', AC_ID=(SELECT AC_ID FROM AC_MAST WHERE AC_NAME = ''OPENING STOCK'') WHERE AC_NAME = ''CLOSING STOCK'' AND [DATE] = '''+CONVERT(VARCHAR(50),@C_St_Date-1)+''' '
--EXECUTE sp_executesql @SQLCOMMAND 
--Bug21282

--vasant021109
/*Remove Trading and Profit loss Previous Entry [Start]*/
/*SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY IN 
	(SELECT CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY AS COMEID FROM '+@TBLNAME1+' WHERE [TYPE] IN (''T'',''P'') 
	AND [DATE] NOT BETWEEN '''+CONVERT(VARCHAR(50),@C_St_Date)+''' AND '''+CONVERT(VARCHAR(50),@TDate)+''') AND [TYPE] IN (''T'',''P'') '*/
SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY IN 
	(SELECT CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY AS COMEID FROM '+@TBLNAME1+' WHERE [TYPE] IN (''T'',''P'') 
	AND [DATE] NOT BETWEEN '''+CONVERT(VARCHAR(50),@FDate)+''' AND '''+CONVERT(VARCHAR(50),@TDate)+''') AND [TYPE] IN (''T'',''P'') '
--vasant021109	
EXECUTE sp_executesql @SQLCOMMAND
/*Remove Trading and Profit loss Previous Entry [End]*/

-- ADDED BY AMRENDRA ON 31/05/2011 FOR TKT-7541    ** START
SET @SQLCOMMAND = 'DECLARE @OPTRAN_CD as INT,@OPDATE as DATETIME,@OPACNAME as varchar(250) DECLARE openingentry_cursor CURSOR FOR
	SELECT TRAN_CD,AC_NAME,DATE FROM '+@TBLNAME1+' WHERE 
	ENTRY_TY IN (''OB'') 
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   DELETE FROM '+@TBLNAME1+' WHERE DATE < @OPDATE		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
			AND AC_NAME IN (SELECT AC_NAME FROM '+@TBLNAME1+' WHERE AC_NAME = @OPACNAME AND ENTRY_TY IN (''OB'') AND TRAN_CD = @OPTRAN_CD )		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
	   FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
	END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor'
EXECUTE SP_EXECUTESQL @SQLCOMMAND
 
	
/* Removing carry-forwarded records [Start] 
SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE 
		DATE < (SELECT TOP 1 DATE FROM '+@TBLNAME1+'
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'' OR bCode_Nm = ''OS''
		OR Entry_Ty = ''OS'') AND DATE = '''+CONVERT(VARCHAR(50),@C_St_Date)+''')
		AND AC_NAME IN (SELECT AC_NAME FROM '+@TBLNAME1+'
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'' OR bCode_Nm = ''OS''
		OR Entry_Ty = ''OS'') AND DATE = '''+CONVERT(VARCHAR(50),@C_St_Date)+''' GROUP BY AC_NAME)'
EXECUTE sp_executesql @SQLCOMMAND
-- ADDED BY AMRENDRA ON 31/05/2011 FOR TKT-7541    ** END
*/
/* Removing carry-forwarded records [End] */
--
--SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE ENTRY_TY IN (SELECT (CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END) AS BHENT FROM LCODE WHERE ENTRY_TY = ''OS'' OR BCODE_NM = ''OS'') '
--SET @SQLCOMMAND = @SQLCOMMAND + ' AND EXISTS(SELECT TOP 1 DATE FROM LITEM_VW WHERE DATE < '''+CONVERT(VARCHAR(50),@FDate)+''') '

--Added by Vasant on 20/07/2011 for TKT-8494   -- start
--SET @SQLCOMMAND = 'IF EXISTS(SELECT TOP 1 A.DATE FROM LITEM_VW A,LCODE B,LMAIN_VW C WHERE A.ENTRY_TY = B.ENTRY_TY AND A.ENTRY_TY = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND A.DATE < '''+CONVERT(VARCHAR(50),@C_St_Date)+''' AND B.INV_STK<>'' '' AND C.[RULE] NOT IN (''EXCISE'',''NON-EXCISE''))  '		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).	--Commented By Shrikant S. on 10/06/2013 for Bug-548
SET @SQLCOMMAND = 'IF EXISTS(SELECT TOP 1 A.DATE FROM LITEM_VW A,LCODE B,LMAIN_VW C WHERE A.ENTRY_TY = B.ENTRY_TY AND A.ENTRY_TY = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND A.DATE < '''+CONVERT(VARCHAR(50),@C_St_Date)+''' AND B.INV_STK<>'' '' AND A.DC_NO='''' AND C.[RULE] NOT IN (''EXCISE'',''NON-EXCISE''))  '		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).	--Added By Shrikant S. on 10/06/2013 for Bug-548
SET @SQLCOMMAND = @SQLCOMMAND + ' DELETE FROM '+@TBLNAME1+' WHERE ENTRY_TY IN (SELECT (CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END) AS BHENT FROM LCODE WHERE ENTRY_TY = ''OS'' OR BCODE_NM = ''OS'') '
EXECUTE sp_executesql @SQLCOMMAND
--Added by Vasant on 20/07/2011 for TKT-8494   -- end



--Bug21282		--Commented by Shrikant S. on 17/09/2014 for 24146 		--Start
--SET @SQLCOMMAND = 'Update '+@TBLNAME1+' Set Amount=0 Where AC_NAME in (''OPENING BALANCES'')'		
--EXECUTE sp_executesql @SQLCOMMAND
--Bug21282		--Shrikant S. 17/09/2014 on 17/09/2014 for 24146 		--End

--Bug20309
Set @Stk_OpAccounts = ''
DECLARE CSTKVAL CURSOR FOR 
SELECT Op_AcName FROM ##STKVALConfig
OPEN CSTKVAL
FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND = 'IF EXISTS(SELECT TOP 1 A.DATE FROM ARMAIN A WHERE A.[RULE] IN (''EXCISE'',''NON-EXCISE'') AND A.DATE < '''+CONVERT(VARCHAR(50),@C_St_Date)+''')' 
	SET @SQLCOMMAND = @SQLCOMMAND + 'Update '+@TBLNAME1+' Set Amount = 0 Where AC_NAME in ('''+@Stk_OpAccounts+''') '	
	EXECUTE sp_executesql @SQLCOMMAND

	FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts
END
CLOSE CSTKVAL
DEALLOCATE CSTKVAL
--Bug20309

--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
--SET @SQLCOMMAND = 'Update '+@TBLNAME1+' Set Amount = 0 Where AC_NAME = ''OPENING STOCK'''		--Commented By Shrikant S. on 10/06/2013 for Bug-548
--SET @SQLCOMMAND = 'Update '+@TBLNAME1+' Set Amount = 0 Where AC_NAME in (''OPENING STOCK'',''OPENING BALANCES'')'		--Added By Shrikant S. on 10/06/2013 for Bug-548			
--EXECUTE sp_executesql @SQLCOMMAND
--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).

--Bug20309
Set @Stk_OpAccounts = ''
Set @Stk_ClAccounts = ''
DECLARE CSTKVAL CURSOR FOR 
SELECT Op_AcName,ClB_AcName FROM ##STKVALConfig
OPEN CSTKVAL
FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts,@Stk_ClAccounts
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND = 'UPDATE '+@TBLNAME1+' SET AC_NAME = '''+@Stk_OpAccounts+''', AC_ID=(SELECT AC_ID FROM AC_MAST WHERE AC_NAME = '''+@Stk_OpAccounts+''') WHERE AC_NAME = '''+@Stk_ClAccounts+''' AND [DATE] = '''+CONVERT(VARCHAR(50),@C_St_Date-1)+''' '	
	EXECUTE sp_executesql @SQLCOMMAND 

	FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts,@Stk_ClAccounts
END
CLOSE CSTKVAL
DEALLOCATE CSTKVAL

Drop table ##STKVALConfig
--Bug20309

SET @SQLCOMMAND = 'SELECT TRAN_CD=0,ENTRY_TY='' '',
	DATE = '''+CONVERT(VARCHAR(50),@FDate)+''',
	AMOUNT=ISNULL(SUM(CASE WHEN TVW.AMT_TY = ''DR'' THEN TVW.AMOUNT ELSE - TVW.AMOUNT END),0),
	TVW.AC_ID,TVW.AC_NAME,TVW.ACSERIAL,AMT_TY=''A'',INV_NO='' ''
	INTO '+@TBLNAME2+' FROM '+@TBLNAME1+' TVW
	WHERE (TVW.DATE < '''+CONVERT(VARCHAR(50),@FDate)+'''
	OR TVW.ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB'' OR Entry_Ty = ''OB'' OR bCode_Nm = ''OS'' OR Entry_Ty = ''OS'')) 
	GROUP BY TVW.AC_ID,TVW.AC_NAME,TVW.ACSERIAL
	UNION
SELECT TVW.TRAN_CD,TVW.ENTRY_TY,TVW.DATE,
	AMOUNT=(CASE WHEN TVW.AMT_TY=''DR'' THEN TVW.AMOUNT ELSE -TVW.AMOUNT END),
	TVW.AC_ID,TVW.AC_NAME,TVW.ACSERIAL,TVW.AMT_TY,TVW.INV_NO
	FROM '+@TBLNAME1+' TVW
	LEFT JOIN LAC_VW LVW (NOLOCK) 
	ON TVW.TRAN_CD = LVW.TRAN_CD AND TVW.ENTRY_TY = LVW.ENTRY_TY AND TVW.AC_ID != LVW.AC_ID
	WHERE (TVW.DATE BETWEEN '''+CONVERT(VARCHAR(50),@FDate)+''' AND '''+CONVERT(VARCHAR(50),@TDate)+''' AND 
	TVW.ENTRY_TY NOT IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB'' OR Entry_Ty = ''OB'' OR bCode_Nm = ''OS'' OR Entry_Ty = ''OS''))'
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'SELECT a.Ac_id,
	Opening = isnull(CASE Amt_Ty WHEN ''A'' THEN SUM(a.Amount)END,0),
	Debit = isnull(CASE Amt_Ty WHEN ''DR'' THEN SUM(a.Amount)END,0),
	Credit = isnull(CASE Amt_Ty WHEN ''CR'' THEN SUM(a.Amount) END,0)
	Into '+@TBLNAME3+' from '+@TBLNAME2+' a
	group by a.Ac_id,a.amt_ty'
EXECUTE sp_executesql @SQLCOMMAND

--Bug21282
/*
SET @SQLCOMMAND = 'SELECT b.Ac_id,Sum(a.Opening) as OpBal,Sum(a.Debit) as Debit,
	Sum(a.Credit) as Credit,CAST(0 AS Numeric(17,2)) As ClBal
	Into '+@TBLNAME4+' from '+@TBLNAME3+' a Right Join Ac_Mast b 
	ON (b.Ac_id = a.Ac_id) group by b.Ac_id'
*/
SET @SQLCOMMAND = 'SELECT b.Ac_id,Sum(IsNull(a.Opening,0)) as OpBal,Sum(IsNull(a.Debit,0)) as Debit,
	Sum(IsNull(a.Credit,0)) as Credit,CAST(0 AS Numeric(17,2)) As ClBal
	Into '+@TBLNAME4+' from '+@TBLNAME3+' a Right Join Ac_Mast b 
	ON (b.Ac_id = a.Ac_id) group by b.Ac_id'
--Bug21282	
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Update '+@TBLNAME4+' SET OPbal = (CASE WHEN OpBal IS NULL THEN 0 ELSE OPBAL END),
	Debit = (CASE WHEN Debit IS NULL THEN 0 ELSE Debit END),
	Credit = (CASE WHEN Credit IS NULL THEN 0 ELSE Credit END),
	Clbal = (CASE WHEN Clbal IS NULL THEN 0 ELSE Clbal END)'
EXECUTE sp_executesql @SQLCOMMAND

--Bug21282
SET @SQLCOMMAND = 'UPDATE '+@TBLNAME4+' SET ClBal = (OpBal+Debit-ABS(Credit)) '
EXECUTE sp_executesql @SQLCOMMAND 
--Bug21282

/* Combined Groups And Ledgers with Opening,Debit,Credit[Start] */
--Bug21282
/*
SET @SQLCOMMAND = 'Select Updown,''G'' As MainFlg,Ac_Group_Id as Ac_Id,gAC_id as Ac_Group_Id,AC_GROUP_NAME+space(100) as Ac_Name,[Group],
	CAST(0 AS Numeric(17,2)) As OpBal,CAST(0 AS Numeric(17,2)) As Debit,CAST(0 AS Numeric(17,2)) As Credit,CAST(0 AS Numeric(17,2)) As ClBal
	From Ac_Group_Mast
Union All 
Select Updown,''L'' As MainFlg,b.Ac_Id,b.Ac_Group_Id,b.Ac_Name+space(100), b.[Group],
	a.OpBal,a.Debit,ABS(a.Credit),(a.OpBal+a.Debit-ABS(a.Credit)) as ClBal
	From '+@TBLNAME4+' a Right Join Ac_Mast b ON (b.Ac_id = a.Ac_id)'
*/
--Commented by Shrikant S. on 04/03/2014 for bug-19694		--Start
----SET @SQLCOMMAND = 'Select Updown,''G'' As MainFlg,Ac_Group_Id as Ac_Id,gAC_id as Ac_Group_Id,AC_GROUP_NAME+space(100) as Ac_Name,[Group],
----	CAST(0 AS Numeric(17,2)) As OpBal,CAST(0 AS Numeric(17,2)) As Debit,CAST(0 AS Numeric(17,2)) As Credit,CAST(0 AS Numeric(17,2)) As ClBal
----	From Ac_Group_Mast
----Union All 
----Select Updown,''L'' As MainFlg,b.Ac_Id,b.Ac_Group_Id,b.Ac_Name+space(100), b.[Group],
----	a.OpBal,a.Debit,ABS(a.Credit),a.ClBal
----	From '+@TBLNAME4+' a Right Join Ac_Mast b ON (b.Ac_id = a.Ac_id)'
--Commented by Shrikant S. on 04/03/2014 for bug-19694		--End
--Bug21282
--EXECUTE sp_executesql @SQLCOMMAND		--Commented by Shrikant S. on 04/03/2014 for bug-19694
/* Combined Groups And Ledgers [End] */




--Added by Shrikant S. on 04/03/2014 for bug-19694		--Start
/* Combined Groups And Ledgers with Opening,Debit,Credit[Start] */
SET @SQLCOMMAND = 'Select Updown,''G'' As MainFlg,Ac_Group_Id as Ac_Id,gAC_id as Ac_Group_Id,AC_GROUP_NAME+space(100) as Ac_Name,[Group],
	CAST(0 AS Numeric(17,2)) As OpBal,CAST(0 AS Numeric(17,2)) As Debit,CAST(0 AS Numeric(17,2)) As Credit,CAST(0 AS Numeric(17,2)) As ClBal
	INTO '+@TBLNAME5+' From Ac_Group_Mast
Union All 
Select Updown,''L'' As MainFlg,b.Ac_Id,b.Ac_Group_Id,b.Ac_Name+space(100), b.[Group],
	a.OpBal,a.Debit,ABS(a.Credit),a.ClBal
	From '+@TBLNAME4+' a Right Join Ac_Mast b ON (b.Ac_id = a.Ac_id)'
EXECUTE sp_executesql @SQLCOMMAND
/* Combined Groups And Ledgers [End] */


/* Updating the Alternate group in case of Negative Balance Sheet[Start] */	
If Exists(Select b.[name] From sysobjects a inner join syscolumns b on (a.id=b.id) where a.[name]='ac_mast' and b.[name]='agrp_id')
Begin
	Select Ac_id,aGrp_Id Into #pGrpid from Ac_mast Where isnull(AGRP_ID,0)<>0
	SELECT AC_ID,ACTYPE=space(1) INTO #ACMAST FROM PTMAIN WHERE  1=2
	
	DECLARE @MCOND AS BIT,@LVL  AS INT
	
	CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
	SET @LVL=0
	
	INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL FROM AC_GROUP_MAST WHERE AC_GROUP_NAME ='LIABILITIES'
	SET @MCOND=1
	WHILE @MCOND=1
	BEGIN
		IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE AC_GROUP_ID!=GAC_ID AND GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
		BEGIN
			PRINT @LVL
			INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE AC_GROUP_ID!=GAC_ID AND GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
			SET @LVL=@LVL+1
		END
		ELSE
		BEGIN
			SET @MCOND=0	
		END
	END
	INSERT INTO #ACMAST SELECT AC_ID,'L' FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)
	DELETE FROM #ACGRPID
	

	INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME ='ASSETS'
	SET @LVL=0
	SET @MCOND=1
	WHILE @MCOND=1
	BEGIN
		IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
		BEGIN
			--PRINT @LVL
			INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
			SET @LVL=@LVL+1
		END
		ELSE
		BEGIN
			SET @MCOND=0	
		END
	END
	INSERT INTO #ACMAST SELECT AC_ID,'A' FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)
	DELETE FROM #ACGRPID

	SET @SQLCOMMAND = 'Update '	+@TBLNAME5+' set Ac_group_Id=b.aGrp_id From '+@TBLNAME5+' a '
	SET @SQLCOMMAND = @SQLCOMMAND+' '+'inner join #pGrpid b On (a.Ac_Id=b.Ac_Id) Left Join #ACMAST c on (a.ac_id=c.ac_id)'
	SET @SQLCOMMAND = @SQLCOMMAND+' '+'Where (c.AcType=''L'' and a.ClBal>0) Or ((c.AcType IS NULL OR c.AcType=''A'')AND a.ClBal<0)'
	EXECUTE sp_executesql @SQLCOMMAND
End
/* Updating the Alternate group in case of Negative Balance Sheet[End] */

--Added by Shrikant S. on 25/09/2014 for Bug-24146		--Start
Declare @OpbalAmt Decimal(18,2),@ParmDefinition NVARCHAR(500)
set @OpbalAmt=0
SET @ParmDefinition=N'@parmOUT Decimal(18,2) Output'
SET @SQLCOMMAND = 'Select @parmOUT=Isnull(Clbal,0) from '+@TBLNAME5+' Where AC_NAME in (''OPENING BALANCES'')'		
EXECUTE sp_executesql @SQLCOMMAND,@ParmDefinition,@parmOUT=@OpbalAmt Output
print @OpbalAmt

SET @SQLCOMMAND = 'Update '+@TBLNAME5+' Set Opbal=0,Debit=0,Credit=0,ClBal=0 Where AC_NAME in (''OPENING BALANCES'')'		
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Select *,'+convert(Varchar(20),@OpbalAmt)+' as OpBalAmt From '+@TBLNAME5
--Added by Shrikant S. on 25/09/2014 for Bug-24146		--End

--SET @SQLCOMMAND = 'Select * From '+@TBLNAME5		--Commented by Shrikant S. on 25/09/2014 for Bug-24146
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Drop table '+@TBLNAME5
EXECUTE sp_executesql @SQLCOMMAND

--Added by Shrikant S. on 04/03/2014 for bug-19694		--End



/* Droping Temp tables [Start] */
SET @SQLCOMMAND = 'Drop table '+@TBLNAME1
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME2
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME3
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME4
EXECUTE sp_executesql @SQLCOMMAND
/* Droping Temp tables [End] */


