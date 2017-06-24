-- =============================================
-- Author:		Kishor Agarwal
-- Create date: 21/03/2016
-- =============================================
Create PROCEDURE [dbo].[USP_ENT_WorkOrder_Pending]
@DbName varchar(10),@EDATE SMALLDATETIME
AS

declare @sqlcommand Nvarchar(max)

CREATE TABLE [#BomOpenRecords](
	[inv_no] [varchar](15) NULL,
	[date] [datetime] NULL,
	[it_name] [varchar](50) NULL,
	[it_code] [numeric](18, 0) NULL,
	[item] [varchar](50) NULL,
	[entry_ty] [varchar](2) NULL,
	[tran_cd] [int] NULL,
	[itserial] [varchar](5) NULL,
	[qty] [numeric](16, 4) NULL,
	[bomid] [varchar](6) NULL,
	[bomlevel] [numeric](10, 0) NULL,
	[rmitemid] [numeric](18, 0) NOT NULL,
	[rmitem] [varchar](50) NULL,
	[rqty] [numeric](16, 4) NULL,
	[reqty] [numeric](16, 4) NULL,
	[iqty] [numeric](16, 4) NULL,
	[aiqty] [numeric](16, 4) NULL,
	[balqty] [numeric](16, 4) NULL,
	[sqtym] [numeric](16, 4) NULL,
	[sqty] [numeric](16, 4) NULL,
	[sqtyLabr] [numeric](16, 4) NULL,
	[ssqty] [numeric](16, 4) NULL,
	[orgqty] [numeric](16, 4) NULL,
	[tlissperp] [decimal](12, 3) NULL,
	[tlissperm] [decimal](12, 3) NULL,
	[tlissqtyp] [numeric](16, 4) NULL,
	[tlissqtym] [numeric](16, 4) NULL,
	[posqty] [numeric](16, 4) NULL,
	[wkqty] [numeric](16, 4) NULL,
	[msqtym] [numeric](16, 4) NULL,
	[msqty] [numeric](16, 4) NULL,
	[msqtyLabr] [numeric](16, 4) NULL,
	[mclear] [numeric](16, 4) NULL,
	[mpending] [numeric](16, 4) NULL,
	[trm_qty] [numeric](10, 2) NULL,
	[isbom] [bit] NULL,
	[rate] [numeric](13, 3) NULL,
	[rmqty] [numeric](16, 3) NULL,
	[fgqty] [numeric](16, 3) NULL,
	[BATCHNO] [varchar](10) NULL,
	[MFGDT] [datetime] NULL,
	[EXPDT] [datetime] NULL,
	[BOMIDX] [varchar](6) NULL,
	[U_BOMDET] [bit] NULL,
	[BOMLEVELX] [numeric](10, 0) NULL,
	[PROC_ID] [varchar](10) NULL
)
SET @sqlcommand ='INSERT INTO #BomOpenRecords Execute USP_ENT_BOMDET_IP ''IP'',0,'''+CONVERT(Varchar(50),@Edate)+''','''','''','''','''','''','''''
print @sqlcommand
execute sp_executesql @sqlcommand

--select @DbName=dbname from Vudyog..CO_MAST where dbname>=''+@DbName+'' and enddir=''

SELECT A.*,isnull(B.BalQty,0) as BalQty INTO #TempIPRecord FROM (select * from (SELECT entry_ty,tran_cd as IPTran_cd,aentry_ty,atran_cd as WKTran_cd,itserial,it_code,Aqty from projectitref P 
group by entry_ty,tran_cd,aentry_ty,atran_cd,itserial,it_code,Aqty) A where aentry_ty='WK' and entry_ty IN ('IP','OP')) A
LEFT JOIN 
(select * from (SELECT entry_ty,aentry_ty,atran_cd as WKTran_cd,IT_CODE,P.item,Aqty,SUM(P.QTY) AS IssueQty,P.aqty-SUM(P.QTY) AS BalQty from projectitref P 
group by entry_ty,aentry_ty,atran_cd,IT_CODE,P.item,Aqty) A where aentry_ty='WK' AND ENTRY_TY='OP') B ON (A.WKTran_cd = B.WKTran_cd) where A.aqty<>isnull(b.IssueQty,0)

DECLARE @TblName Varchar(20),@fldlist Varchar(4000)


--**************************************** IP TABLES TRANSFER **********************************************
	
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IPITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) 
	IN (SELECT entry_ty+Convert(Varchar(10),IPTran_cd)+itserial+Convert(Varchar(10),it_code) FROM #TempIPRecord)'	
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IPMAIN WHERE TRAN_CD IN (SELECT IPTran_cd FROM #TempIPRecord)'	
	Execute sp_Executesql @sqlcommand

			
	set @TblName='IPMain'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IPMain ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IPMAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IPMAIN WHERE TRAN_CD IN (SELECT IPTran_cd FROM #TempIPRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..IPMAIN)
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IPMain OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand

	set @TblName='IPITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IPITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IPITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT entry_ty+Convert(Varchar(10),IPTran_cd)+itserial+Convert(Varchar(10),it_code) FROM #TempIPRecord) AND entry_ty+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) NOT IN (SELECT entry_ty+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) FROM '+RTRIM(LTRIM(@DbName))+'..IPITEM)'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'UPDATE '+RTRIM(LTRIM(@DbName))+'..IPITEM SET DC_NO=''YE'' WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT entry_ty+Convert(Varchar(10),IPTran_cd)+itserial+Convert(Varchar(10),it_code) FROM #TempIPRecord)'	
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand

--*****************************************************************************************************************

--**************************************** PROJECTITREF TABLE TRANSFER **********************************************
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..PROJECTITREF WHERE Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT Convert(Varchar(10),IPTran_cd)+itserial+Convert(Varchar(10),it_code) FROM #TempIPRecord)'	
	Execute sp_Executesql @sqlcommand

	
	set @TblName='PROJECTITREF'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..PROJECTITREF ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..PROJECTITREF ('+@fldlist+') 
	SELECT '+@fldlist+' FROM PROJECTITREF WHERE Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT Convert(Varchar(10),IPTran_cd)+itserial+Convert(Varchar(10),it_code) FROM #TempIPRecord) AND Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) NOT IN (SELECT Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) 
	FROM '+RTRIM(LTRIM(@DbName))+'..PROJECTITREF) and entry_ty IN(''IP'',''OP'') Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..PROJECTITREF OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand

--*****************************************************************************************************************


--**************************************** WORK ORDER TABLES TRANSFER **********************************************									
	
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..ITEM WHERE TRAN_CD IN (SELECT TRAN_CD FROM #BomOpenRecords)'
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..Main WHERE TRAN_CD IN (SELECT TRAN_CD FROM #BomOpenRecords)'	
	Execute sp_Executesql @sqlcommand
	
	set @TblName='Main'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..MAIN ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..MAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM MAIN WHERE TRAN_CD IN (SELECT TRAN_CD FROM #BomOpenRecords) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..MAIN) and ENTRY_TY=''WK''
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..MAIN OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand


	set @TblName='ITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..ITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM ITEM WHERE TRAN_CD IN (SELECT TRAN_CD FROM #BomOpenRecords) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..ITEM) and ENTRY_TY=''WK'''
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand

--**************************************************************************************************************--

	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..ITEM WHERE TRAN_CD IN (SELECT WKTran_cd FROM #TempIPRecord)'
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..Main WHERE TRAN_CD IN (SELECT WKTran_cd FROM #TempIPRecord)'	
	Execute sp_Executesql @sqlcommand
	
	set @TblName='Main'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..MAIN ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..MAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM MAIN WHERE TRAN_CD IN (SELECT WKTran_cd FROM #TempIPRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..MAIN)
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..MAIN OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand


	set @TblName='ITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..ITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM ITEM WHERE TRAN_CD IN (SELECT WKTran_cd FROM #TempIPRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..ITEM)'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand


--***************************************************************************************************
	DROP TABLE #BomOpenRecords
	DROP TABLE #TempIPRecord