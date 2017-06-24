If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'Dynamically_Fields_Multi_Rep'))
Begin
	Drop Procedure Dynamically_Fields_Multi_Rep
End

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE Procedure [dbo].[Dynamically_Fields_Multi_Rep]	

As		
	DECLARE @COM_SQLSTR NVARCHAR(4000),@SQL_TMPFLD NVARCHAR(4000)
	SET @SQL_TMPFLD =''
	
	DECLARE @DBNAME AS VARCHAR(100),@tbl as nvarchar(4000),@DataName as varchar(100),@MSQLSTR AS NVARCHAR(4000)
	DECLARE @FLD_NM AS VARCHAR(100),@TYPE AS VARCHAR(25),@LENTH AS INT,@PRE AS VARCHAR(10),@SCALE AS VARCHAR(10),@TBLNAME AS varchar(100),@ATT AS BIT
	
	SET @COM_SQLSTR =''
		
	CREATE TABLE #TMPADDFLD (E_CODE varchar(2),FLD_NM varchar(100),[Type] varchar(25),MAX_LENGTH varchar(10),[PRECISION] varchar(10),SCALE varchar(10),att_file bit,TBLNAME varchar(50))
	CREATE TABLE ##Dyn_TmpTable_multi (Entry_ty Varchar(10),TblFld VARCHAR(4000),SqlStr varchar(4000))
	
		select @DBNAME=DBNAME from vudyog..CO_MAST where com_type='M'
		set @tbl=' select b.dbname into ##DbTabel  from '+@DBNAME+'..com_det a inner join vudyog..co_mast b on (a.co_name=b.co_name and a.sta_dt = b.sta_dt and a.end_dt = b.end_dt)'
		EXECUTE SP_EXECUTESQL @tbl

		DECLARE @SQL varchar(max)
		SET @SQL=''
		SELECT @SQL=@SQL+CAST('' AS VARCHAR(MAX))
		SELECT @SQL=@SQL+'UNION
		select 
		A.E_CODE,A.FLD_NM,C.[NAME] AS [TYPE],B.MAX_LENGTH AS MAX_LENGTH,B.PRECISION AS PRECISION,
		B.SCALE AS SCALE,A.att_file,S.name AS TBLNAME
		from '+d.name+'..Lother a  
		inner join '+d.name+'.sys.columns B on a.fld_nm=B.[name] 
		inner join '+d.name+'.sys.objects  S on B.object_id=S.object_id AND S.[NAME]=A.E_CODE+CASE WHEN A.ATT_FILE=1 THEN ''MAIN'' ELSE ''ITEM'' END
		INNER JOIN '+d.name+'.sys.TYPES  C on B.SYSTEM_TYPE_ID=C.SYSTEM_TYPE_ID
		where a.e_code in (''ST'',''PT'',''SR'',''PR'') AND A.LSHWSALETAXFRM=1 and a.att_file=1
		'
		FROM sys.databases d 
		INNER JOIN ##DbTabel B ON D.[name] =B.DBNAME COLLATE DATABASE_DEFAULT
		SELECT @SQL=RIGHT(@SQL,LEN(@SQL)-5)+'order by 1,3'
		INSERT INTO #TMPADDFLD EXEC (@SQL)
		
			DECLARE @xFLD_NM AS VARCHAR(100),@xTYPE AS VARCHAR(25),@xLENTH AS INT,@xPRE AS VARCHAR(10),@xSCALE AS VARCHAR(10),@xATT AS BIT
			DECLARE Strtabel cursor for 
			SELECT FLD_NM,MAX([TYPE]) AS [TYPE],MAX(MAX_LENGTH) AS MAX_LENGTH,MAX(PRECISION) AS PRECISION,
			MAX(SCALE) AS SCALE,att_file FROM #TMPADDFLD GROUP BY FLD_NM,att_file ORDER BY fld_nm
			OPEN Strtabel
			FETCH NEXT FROM Strtabel INTO @xFLD_NM,@xTYPE,@xLENTH,@xPRE,@xSCALE,@xATT
			WHILE @@FETCH_STATUS =0
			BEGIN
				SET @SQL_TMPFLD =@SQL_TMPFLD +',ISNULL(M_'+@xFLD_NM+','''') AS M_'+@xFLD_NM
					SELECT @COM_SQLSTR=@COM_SQLSTR +
					',CAST('+
						CASE WHEN @xTYPE='BIT' THEN '0 AS BIT' ELSE CASE WHEN @xTYPE='CHAR' THEN CHAR(39)+CHAR(39)+' AS CHAR('+RTRIM(CAST(@xLENTH AS VARCHAR))+')'
							ELSE CASE WHEN @xTYPE='DATETIME' THEN CHAR(39)+CHAR(39)+' AS DATETIME' ELSE
						CASE WHEN @xTYPE='DECIMAL' THEN '0 AS DECIMAL('+RTRIM(CAST(@xPRE AS VARCHAR))+','+RTRIM(CAST(@xSCALE AS VARCHAR))+')'
							ELSE CASE WHEN @xTYPE='INT' THEN '0 AS INT' ELSE CASE WHEN @xTYPE='NUMERIC' THEN
							'0 AS NUMERIC('+RTRIM(CAST(@xPRE AS VARCHAR))+','+RTRIM(CAST(@xSCALE AS VARCHAR))+')' ELSE
						CASE WHEN @xTYPE='SMALLDATETIME' THEN CHAR(39)+CHAR(39)+' AS SMALLDATETIME' ELSE CASE WHEN @xTYPE='TEXT' THEN
							CHAR(39)+CHAR(39)+' AS TEXT' ELSE CASE WHEN @xTYPE='VARCHAR' THEN CHAR(39)+CHAR(39)+' AS VARCHAR('+RTRIM(CAST(@xLENTH AS VARCHAR))+')'
							ELSE CHAR(39)+CHAR(39)+' AS VARCHAR('+RTRIM(CAST(@xLENTH AS VARCHAR))+')' END END END END END END END END
						END+') AS M_'+@xFLD_NM							
				FETCH NEXT FROM Strtabel INTO @xFLD_NM,@xTYPE,@xLENTH,@xPRE,@xSCALE,@xATT
			END		

	insert into ##Dyn_TmpTable_multi VALUES ('SQSTR','',@COM_SQLSTR)
	insert into ##Dyn_TmpTable_multi VALUES ('TMPFLD','',@SQL_TMPFLD)

CLOSE Strtabel
DEALLOCATE Strtabel

DROP TABLE #TMPADDFLD
drop table ##DbTabel