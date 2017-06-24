If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'Dynamically_Multifields'))
Begin
	Drop Procedure Dynamically_Multifields
End

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE Procedure [dbo].Dynamically_Multifields	
(
@Val as varchar(1)
)
As		
DECLARE @COND AS BIT
Create table ##MulTempTbl (dbname varchar(10),Att_file bit,SEL bit,fld_nm varchar(100),head_nm varchar(200),Name varchar(15))
SET @COND =0
if @Val='M'
BEGIN
	SET @COND=1
END
	

DECLARE @DBNAME AS VARCHAR(100),@tbl as nvarchar(4000),@DataName as varchar(100),@MSQLSTR AS NVARCHAR(4000)
select @DBNAME=DBNAME from vudyog..CO_MAST where com_type='M'
set @tbl=' select b.dbname into ##DbTabel  from '+@DBNAME+'..com_det a inner join vudyog..co_mast b on (a.co_name=b.co_name and a.sta_dt = b.sta_dt and a.end_dt = b.end_dt)'
EXECUTE SP_EXECUTESQL @tbl
DECLARE @SQL varchar(max)
SET @SQL=''
SELECT @SQL=@SQL+CAST('' AS VARCHAR(MAX))
SELECT @SQL=@SQL+'UNION
select '+CHAR(39)+d.name+CHAR(39)+', A.ATT_FILE,A.LSHWSALETAXFRM as Sel,A.FLD_NM,A.HEAD_NM,Name=(CASE WHEN A.att_file=1 THEN ''HEADER'' ELSE ''DETAILS'' END)
from '+d.name+'..Lother a  
inner join '+d.name+'.sys.columns B on a.fld_nm=B.[name] 
inner join '+d.name+'.sys.objects  S on B.object_id=S.object_id AND S.[NAME]=A.E_CODE+CASE WHEN A.ATT_FILE=1 THEN ''MAIN'' ELSE ''ITEM'' END
INNER JOIN '+d.name+'.sys.TYPES  C on B.SYSTEM_TYPE_ID=C.SYSTEM_TYPE_ID
where a.e_code in (''ST'',''PT'',''SR'',''PR'') AND A.LSHWSALETAXFRM >='+CAST(@COND AS VARCHAR)
FROM sys.databases d 
INNER JOIN ##DbTabel B ON D.[name] =B.DBNAME COLLATE DATABASE_DEFAULT
SELECT @SQL=RIGHT(@SQL,LEN(@SQL)-5)+'order by 1,3'
print @SQL
INSERT INTO ##MulTempTbl EXEC (@SQL)

select max(dbname) as dbname,ATT_FILE,fld_nm,MAX(head_nm) as head_nm,Name=(CASE WHEN att_file=1 THEN 'HEADER' ELSE 'DETAILS' END),CAST(MAX(CAST(SEL AS INT)) AS BIT) AS SEL from ##MulTempTbl group by ATT_FILE,fld_nm

DROP TABLE ##DbTabel
drop table ##MulTempTbl
