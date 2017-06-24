-- =============================================
-- Author:		Kishor Agarwal
-- Create date: 21/03/2016
-- =============================================
Create PROCEDURE [dbo].[USP_ENT_LabourJob_V_Pending]
@DbName varchar(10),@EDATE SMALLDATETIME
AS

declare @sqlcommand Nvarchar(max)
SET @SQLCOMMAND='SELECT * INTO ##LabourJov_V FROM (SELECT IRITEM.IT_CODE,IRITEM.ITSERIAL,IRITEM.ENTRY_TY,IRITEM.TRAN_CD LRTRAN_CD,IRITEM.QTY,IIMAIN.TRAN_CD LITRAN_CD,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' QTY_USED=CASE WHEN IIRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN isnull(IIRMDET.QTY_USED,0) ELSE 0 END,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' IT_MAST.IT_NAME,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' IIITEM.IT_CODE LI_ITCODE,LR_QTY=CASE WHEN IIRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN ISNULL(IIITEM.QTY,0) ELSE 0 END,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' BALQTY=IRITEM.QTY-(CASE WHEN IIRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN isnull(IIRMDET.QTY_USED,0) ELSE 0 END+CASE WHEN IIRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN isnull(IIRMDET.WASTAGE,0) ELSE 0 END),'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FQTY=CASE WHEN IIRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN ISNULL(IIITEM.QTY,0) ELSE 0 END '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM IRITEM '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IRMAIN ON  (IRITEM.TRAN_CD=IRMAIN.TRAN_CD) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IT_MAST ON (IRITEM.IT_CODE=IT_MAST.IT_CODE) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IIRMDET ON (IRITEM.TRAN_CD=IIRMDET.LI_TRAN_CD AND IRITEM.ENTRY_TY=IIRMDET.LIENTRY_TY AND IRITEM.ITSERIAL=IIRMDET.LI_ITSER AND IIRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+') '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IIMAIN ON (IIRMDET.TRAN_CD=IIMAIN.TRAN_CD) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IIITEM ON (IIRMDET.TRAN_CD=IIITEM.TRAN_CD AND IIITEM.ENTRY_TY=IIRMDET.ENTRY_TY AND IIRMDET.ITSERIAL=IIITEM.ITSERIAL)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IT_MAST LR_ITMAST ON (LR_ITMAST.IT_CODE=IIITEM.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' WHERE (IRMAIN.DATE< ='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+') AND IRITEM.ENTRY_TY=''RL'' AND IRMAIN.[RULE]=''ANNEXURE V'') A Where ISNULL(BALQTY,0)>0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY ITSERIAL '
print @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND

select LRTRAN_CD,ITSERIAL,IT_CODE,It_name,LITRAN_CD INTO #TempRecord from ##LabourJov_V
DECLARE @TblName Varchar(20),@fldlist Varchar(4000)
--**************************************** II TABLES TRANSFER **********************************************
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IRITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd) IN (SELECT ''RL''+Convert(Varchar(10),LRTRAN_CD) FROM #TempRecord) AND ENTRY_TY=''RL'''
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IRMAIN WHERE TRAN_CD IN (SELECT LRTRAN_CD FROM #TempRecord) AND ENTRY_TY=''RL'''	
	Execute sp_Executesql @sqlcommand
	
	set @TblName='IRMAIN'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IRMAIN ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IRMAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IRMAIN WHERE TRAN_CD IN (SELECT LRTRAN_CD FROM #TempRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..IRMAIN)
	AND ENTRY_TY=''RL''
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IRMAIN OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	set @TblName='IRITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IRITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IRITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT ''RL''+Convert(Varchar(10),LRTRAN_CD)+itserial+Convert(Varchar(10),it_code) FROM #TempRecord) AND entry_ty+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) 
	NOT IN (SELECT entry_ty+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) FROM '+RTRIM(LTRIM(@DbName))+'..IRITEM)
	AND ENTRY_TY=''RL'''
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'UPDATE '+RTRIM(LTRIM(@DbName))+'..IRITEM  SET DC_NO=''YE'' WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) 
	IN (SELECT ''RL''+Convert(Varchar(10),LRTRAN_CD)+itserial+Convert(Varchar(10),it_code) FROM #TempRecord)
	AND ENTRY_TY=''RL'''
	Execute sp_Executesql @sqlcommand
	
--*****************************************************************************************************************

--**************************************** IIRMDET TABLE TRANSFER **********************************************
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IIRMDET WHERE Convert(Varchar(10),LI_TRAN_CD)+li_item 
	IN (SELECT Convert(Varchar(10),LRTran_cd)+It_name FROM #TempRecord) AND lientry_ty =''RL'''
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand


	set @TblName='IIRMDET'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IIRMDET ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IIRMDET WHERE Convert(Varchar(10),LI_TRAN_CD) IN (SELECT Convert(Varchar(10),LRTran_cd)
	FROM #TempRecord) AND Convert(Varchar(10),LI_TRAN_CD) NOT IN (SELECT Convert(Varchar(10),LI_TRAN_CD)
	FROM '+RTRIM(LTRIM(@DbName))+'..IIRMDET) and lientry_ty =''RL'''
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand

---**************************************** II TABLES TRANSFER **********************************************
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IIITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd) IN (SELECT ''IL''+Convert(Varchar(10),LITRAN_CD) FROM #TempRecord) AND ENTRY_TY=''IL'''
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IIMAIN WHERE TRAN_CD IN (SELECT LITRAN_CD FROM #TempRecord) AND ENTRY_TY=''IL'''	
	Execute sp_Executesql @sqlcommand
	
	set @TblName='IIMAIN'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IIMAIN ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IIMAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IIMAIN WHERE TRAN_CD IN (SELECT LITRAN_CD FROM #TempRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..IIMAIN)
	AND ENTRY_TY=''IL''
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IIMAIN OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand	
	
	set @TblName='IIITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IIITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IIITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd) IN (SELECT ''IL''+Convert(Varchar(10),LITRAN_CD)
	FROM #TempRecord) AND entry_ty+Convert(Varchar(10),Tran_cd)
	NOT IN (SELECT entry_ty+Convert(Varchar(10),Tran_cd) FROM '+RTRIM(LTRIM(@DbName))+'..IIITEM)
	AND ENTRY_TY=''IL'''
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'UPDATE '+RTRIM(LTRIM(@DbName))+'..IIITEM SET DC_NO=''YE'' WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)
	IN (SELECT ''IL''+Convert(Varchar(10),LITRAN_CD) FROM #TempRecord)
	AND ENTRY_TY=''IL'''	
	Execute sp_Executesql @sqlcommand
--*****************************************************************************************************************

DROP TABLE ##LabourJov_V
DROP TABLE #TempRecord