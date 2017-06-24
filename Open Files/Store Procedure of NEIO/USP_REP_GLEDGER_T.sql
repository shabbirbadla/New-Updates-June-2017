If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_GLEDGER_T'))
Begin
	Drop Procedure USP_REP_GLEDGER_T
End

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_REP_GLEDGER_T]
	@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
	@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),
	@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
	@SAMT NUMERIC,@EAMT NUMERIC,
	@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
	@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),
	@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
	@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
	@FINYR NVARCHAR(20), @EXTPAR NVARCHAR(60)
	AS
	Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000)
	Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
	Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50),@TBLNAME3 as VARCHAR(50)
	
	Set @OPENTRY_TY = '''OB'''
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM
	Set @TBLNAME3 = '##TMP3'+@TBLNM --&& Added by Shrikant S. by on 05 Feb, 2010
	DECLARE openingentry_cursor CURSOR FOR
		SELECT entry_ty FROM lcode
		WHERE bcode_nm = 'OB'
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @opentries
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   Set @OPENTRY_TY = @OPENTRY_TY +','''+@opentries+''''
	   FETCH NEXT FROM openingentry_cursor into @opentries
	END
	CLOSE openingentry_cursor
	DEALLOCATE openingentry_cursor

	EXECUTE USP_REP_FILTCON 
		@VTMPAC=@TMPAC,@VTMPIT=null,@VSPLCOND=@SPLCOND,
		@VSDATE=null,@VEDATE=@EDATE,
		@VSAC =@SNAME,@VEAC =@ENAME,
		@VSIT=null,@VEIT=null,
		@VSAMT=@SAMT,@VEAMT=@EAMT,
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =null,@VEWARE  =null,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='MVW',@VITFILE=null,@VACFILE='AVW',
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@EXTPAR,
		@VFCON =@FCON OUTPUT

	print @FCON
	
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,AVW.ACSERIAL,
		MVW.INV_NO,MVW.L_YN,MVW.CHEQ_NO,MVW.CHEQ_DT,MNARR=CAST(MVW.NARR AS NVARCHAR(4000)),ANARR=CAST(AVW.NARR AS NVARCHAR(4000)),
		AC_MAST.AC_ID,AC_MAST.AC_NAME,
		AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,'+ --- Added by Ajay Jaiswal on 14/07/2010 for TKT-1679  
		'AC_MAST.i_TAX,AC_MAST.[TYPE],AC_MAST.POSTING,OAC_NAME=SUBSTRING(AVW.OAC_NAME,1,4000),MVW.U_PINVNO,MVW.DRAWN_ON,LCODE.SERIAL,LCODE.CODE_NM
		INTO '+@TBLNAME1+' FROM LAC_VW AVW (NOLOCK)
		INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID
		INNER JOIN LCODE (NOLOCK) ON AVW.ENTRY_TY = LCODE.ENTRY_TY
		INNER JOIN LMAIN_VW MVW (NOLOCK) 
		ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY'+RTRIM(@FCON) --Added Posting & Code_nm Column by Sandeep S. on 08 Jul, 2010 for TKT-2062
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT ENTRY_TY,TRAN_CD,COUNT(ENTRY_TY) AS TOTREC 
		INTO '+@TBLNAME2+' FROM LAC_VW AVW (NOLOCK)
		GROUP BY ENTRY_TY,TRAN_CD'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND



	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE 
		L_YN != '''+@FINYR+''' AND [TYPE] != '+'''B'''
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND = ''

SET @SQLCOMMAND = 'DECLARE @OPTRAN_CD as INT,@OPDATE as DATETIME,@OPACNAME as varchar(250) DECLARE openingentry_cursor CURSOR FOR
	SELECT TRAN_CD,AC_NAME,DATE FROM '+@TBLNAME1+' WHERE 
	ENTRY_TY IN ('+@OPENTRY_TY+') 
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   DELETE FROM '+@TBLNAME1+' WHERE DATE < @OPDATE
			AND AC_NAME IN (SELECT AC_NAME FROM '+@TBLNAME1+' WHERE AC_NAME = @OPACNAME AND ENTRY_TY IN (''OB'') AND TRAN_CD = @OPTRAN_CD )
	   FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
	END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT SERIAL=0,TRAN_CD=0,ENTRY_TY='' '',
		DATE=CONVERT(SMALLDATETIME,'''+CONVERT(VARCHAR(50),@SDATE)+'''),
		AMOUNT=IsNull(sum(CASE WHEN TVW.AMT_TY = ''DR'' THEN TVW.AMOUNT ELSE -TVW.AMOUNT END),0),ACSERIAL='''',TVW.AC_ID,TVW.AC_NAME,
		TVW.ADD1,TVW.ADD2,TVW.ADD3,'+ --- Added by Ajay Jaiswal on 14/07/2010 for TKT-1679  
		'TVW.i_TAX,OAC_NAME=space(1),U_PINVNO=space(1),
		AMT_TY=''A'',INV_NO='' '',CHEQ_NO='' '',CHEQ_DT='' '',MNARR='' '',ANARR='' '',
		AC_NAME1=''Balance B/f'',AMOUNT1=0 ,AMT_TY1 = '' '',TOTREC=1,DRAWN_ON='' '',TVW.POSTING,'' '' as CODE_NM,'' '' as postord '+ --Bug-22246
		'INTO '+@TBLNAME3+' FROM '+@TBLNAME1+' TVW 
		WHERE (TVW.DATE <  '''+CONVERT(VARCHAR(50),@SDATE)+''' OR TVW.ENTRY_TY IN ('+@OPENTRY_TY+')) ' +	
--		GROUP BY TVW.AC_ID,TVW.AC_NAME,TVW.ADD1,TVW.ADD2,TVW.ADD3,TVW.i_TAX,TVW.POSTING,TVW.CODE_NM '+ --Added in Group Posting & Code_nm column by Sandeep S. on 25 Mar, 2011 for TKT-6828    	   
		'GROUP BY TVW.AC_ID,TVW.AC_NAME,TVW.ADD1,TVW.ADD2,TVW.ADD3,TVW.i_TAX,TVW.POSTING '+ -- removed TVW.CODE_NM (Above) by Amrendra for TKT-7541 on 30/05/2011 
		'UNION ALL
		SELECT TVW.SERIAL,TVW.TRAN_CD,TVW.ENTRY_TY,TVW.DATE,
		AMOUNT=(CASE WHEN TVW.AMT_TY=''DR'' THEN TVW.AMOUNT ELSE -TVW.AMOUNT END),TVW.ACSERIAL,TVW.AC_ID,TVW.AC_NAME,
		TVW.ADD1,TVW.ADD2,TVW.ADD3,'+ --- Added by Ajay Jaiswal on 14/07/2010 for TKT-1679  
		'TVW.i_TAX,TVW.OAC_NAME,TVW.U_PINVNO,
		TVW.AMT_TY,TVW.INV_NO,TVW.CHEQ_NO,TVW.CHEQ_DT,TVW.MNARR,TVW.ANARR,
		AC_NAME1=LVW.AC_NAME,AMOUNT1 = LVW.AMOUNT,AMT_TY1 = LVW.AMT_TY,T1VW.TOTREC,TVW.DRAWN_ON,TVW.POSTING,TVW.CODE_NM,lvw.postord '+ --Bug-22246
		'FROM '+@TBLNAME1+' TVW
		LEFT JOIN LAC_VW LVW (NOLOCK) '	+
			--ON TVW.TRAN_CD = LVW.TRAN_CD AND TVW.ENTRY_TY = LVW.ENTRY_TY AND TVW.ACSERIAL != LVW.ACSERIAL && shrikant S. on 05 Feb, 2010
			' ON TVW.TRAN_CD = LVW.TRAN_CD AND TVW.ENTRY_TY = LVW.ENTRY_TY AND TVW.AC_ID != LVW.AC_ID '+
		' LEFT JOIN '+@TBLNAME2+' T1VW (NOLOCK)  '+ --Added by Shrikant S. on 05 Feb, 2010
			' ON TVW.TRAN_CD = T1VW.TRAN_CD AND TVW.ENTRY_TY = T1VW.ENTRY_TY ' + --Added by Shrikant S. on 05 Feb, 2010
		' WHERE (TVW.DATE BETWEEN '''+CONVERT(VARCHAR(50),@SDATE)+''' AND '''+CONVERT(VARCHAR(50),@EDATE) +''' AND '+
		' TVW.ENTRY_TY NOT IN ('+@OPENTRY_TY+'))'
          
		EXECUTE SP_EXECUTESQL @SQLCOMMAND


	SET @SQLCOMMAND = 'SELECT TVW.* INTO ##COMMONTBL FROM '+@TBLNAME3+' TVW
		WHERE TVW.AMOUNT <> 0 
		ORDER BY TVW.AC_NAME,TVW.DATE,TVW.SERIAL,TVW.INV_NO,TVW.ACSERIAL,TVW.TRAN_CD,TVW.POSTORD,TVW.AC_NAME1'	--Bug-22246
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	SET @SQLCOMMAND = 'SELECT ROW_NUMBER()OVER(ORDER BY AC_NAME) AS RNUM,ENTRY_TY,ISNULL(AMT_TY,'''') AS AMTP,ISNULL(AMT_TY1,'''') AS AMT_TY,AC_NAME,ACSERIAL As DACSERIAL ,DATE,AC_NAME1,AMT_TY1,AMOUNT1,AMOUNT,ANARR,INV_NO,CHEQ_NO,CHEQ_DT INTO ##TBLDR FROM ##COMMONTBL WHERE AMOUNT <> 0 AND AMT_TY1=''DR'' OR AMT_TY1='''''
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	SET @SQLCOMMAND = 'SELECT ROW_NUMBER()OVER(ORDER BY AC_NAME) AS RNUM,ENTRY_TY,ISNULL(AMT_TY,'''') AS AMTP,ISNULL(AMT_TY1,'''') AS AMT_TY,AC_NAME,DATE,AC_NAME1,ACSERIAL As CACSERIAL,AMT_TY1,AMOUNT1,AMOUNT,ANARR,INV_NO,CHEq_NO,CHEQ_DT INTO ##TBLCR FROM ##COMMONTBL WHERE AMOUNT <> 0 AND AMT_TY1=''CR'' OR AMT_TY1='''''
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	SET @SQLCOMMAND = 'SELECT ISNULL(DR.AMTP,'''') AS DAMTP,ISNULL(DR.ENTRY_TY,'''') AS DENTRY_TY,DR.AC_NAME,DR.DACSERIAL,ISNULL(DR.AMT_TY,'''') AS DAMT_TY,DR.[DATE] AS DDATE,isnull(DR.INV_NO,'''')AS DINV_NO,ISNULL(DR.AC_NAME1,'''') AS DAC_NAME,ISNULL(DR.CHEQ_NO,'''') AS DCHEQ_NO,DR.CHEQ_DT AS DCHEQ_DT,ISNULL(DR.AMOUNT1,0) AS DAMOUNT1,ISNULL(DR.AMOUNT,0) AS DAMOUNT,isnull(Dr.ANARR,'''') as DNARR,
	ISNULL(CR.AMTP,'''') AS CAMTP,ISNULL(CR.ENTRY_TY,'''') AS CENTRY_TY,ISNULL(CR.AMT_TY,'''') AS CAMT_TY,CR.[DATE] AS CDATE,isnull(CR.INV_NO,'''')AS CINV_NO,ISNULL(CR.AC_NAME1,'''') AS CAC_NAME,CR.CACSERIAL,ISNULL(CR.CHEQ_NO,'''') AS CCHEQ_NO,CR.CHEQ_DT AS CCHEQ_DT,ISNULL(CR.AMOUNT1,0) AS CAMOUNT1,ISNULL(CR.AMOUNT,0) AS CAMOUNT,isnull(CR.ANARR,'''') as CNARR
	FROM ##TBLDR DR FULL JOIN ##TBLCR CR ON (DR.RNUM=CR.RNUM AND DR.AC_NAME=CR.AC_NAME) WHERE DR.AC_NAME<>'''' ORDER BY DR.AC_NAME'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
		
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME3
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	drop table ##TBLDR
	drop table ##TBLCR
	DROP TABLE ##COMMONTBL

