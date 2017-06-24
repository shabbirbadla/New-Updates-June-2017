
if exists(select * from sysobjects where [name]='USP_REP_OUTSTANDINGCR_AGEING_MC' and xtype='p')
drop procedure USP_REP_OUTSTANDINGCR_AGEING_MC

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Created By: Birendra Prasad
-- Modify date: 24/01/2012
-- Description:	This Stored procedure is useful to generate ACCOUNTS  Outstanding Report for sundry creditors for Multi currency
-- Remark	  : This Store procedure generated by used reference of USP_REP_OUTSTANDINGCR_AGEING Store Procedure.
-- Modified	: Shrikant S. on 21/04/2017 for GST	--Changed the columns U_PINVNO,U_PINVDT to PINVNO,PINVDT resp.			
-- Modified By and Date : 
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_OUTSTANDINGCR_AGEING_MC]  
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= null
AS
Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000)
Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50),@TBLNAME3 as VARCHAR(50),@TBLNAME11 as VARCHAR(50),@TBLNAME12 as VARCHAR(50)
DECLARE @GRPID AS INT,@MCOND AS BIT,@LVL  AS INT,@GRP AS VARCHAR(100),@EXPARA1 as VARCHAR(1000)
DECLARE @COLCAP1 AS VARCHAR(50),@COLCAP2 AS VARCHAR(50),@COLCAP3 AS VARCHAR(50),@COLCAP4 AS VARCHAR(50),@COLCAP5 AS VARCHAR(50)
DECLARE @DAYS1 AS varchar (4),@DAYS2 AS varchar (4),@DAYS3 AS varchar (4),@DAYS4 AS varchar (4),@DAYS5 AS varchar (4),@FILTERDATE AS varchar (10),@JV_ALLOC AS varchar (1)

SET @EXPARA1 =replace(@EXPARA,'`','''')
 
if charindex('and',@expara1)>0
set @EXPARA=''
select @EXPARA =case when  isnull(@expara,'')='' then '  30,  60,  90, 120,0,date,1' else @expara end

SET @DAYS1=substring(@expara,1,charindex(',',@expara)-1)
set @EXPARA=LTRIM(substring(@expara,charindex(',',@expara)+1,len(@EXPARA)))
SET @DAYS2=substring(@expara,1,charindex(',',@expara)-1)
set @EXPARA=LTRIM(substring(@expara,charindex(',',@expara)+1,len(@EXPARA)))
SET @DAYS3=substring(@expara,1,charindex(',',@expara)-1)
set @EXPARA=LTRIM(substring(@expara,charindex(',',@expara)+1,len(@EXPARA)))
-- Added by Sandeep dt.12/01/2012  FOR bug-1497
SET @DAYS4=substring(@expara,1,charindex(',',@expara)-1)
set @EXPARA=LTRIM(substring(@expara,charindex(',',@expara)+1,len(@EXPARA)))
--SET @DAYS5=@expara
----aDDED BY SATISH dt.12/01/2012  FOR bug-1497
SET @DAYS5=substring(@expara,1,charindex(',',@expara)-1)
print @DAYS5
if charindex('date',@Days5)>0 or charindex('due_',@Days5)>0
begin
set @Days5=0
end 
else
begin
set @EXPARA=LTRIM(substring(@expara,charindex(',',@expara)+1,len(@EXPARA)))
end
print @EXPARA
SET @FILTERDATE=substring(@expara,1,charindex(',',@expara)-1)
print @FILTERDATE
IF @FILTERDATE='due_dt'
BEGIN
SET @SDATE=DATEADD(day,-1,@SDATE)
END
set @EXPARA=LTRIM(substring(@expara,charindex(',',@expara)+1,len(@EXPARA)))
SET @JV_ALLOC=@expara
---END
print @DAYS1
print @DAYS2
print @DAYS3
print @DAYS4
print @DAYS5
if ltrim(rtrim(@Days5))<>'0'
begin
	set @DAYS1='0'
	set @DAYS2='0'
	set @DAYS3='0'
	set @DAYS4=@DAYS5
end
-- Added by Sandeep dt.24/06/2011 for TKT-8356<----End

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=NULL,@VEDATE=@SDATE   ---Change by satish pal dt.12/01/2012  FOR bug-1497
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=NULL,@VEAMT=NULL
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='MN',@VITFILE=Null,@VACFILE='AC'
--,@VDTFLD ='DUE_DT'
,@VDTFLD =@FILTERDATE			---Change by satish pal dt.12/01/2012  FOR bug-1497
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

set QUOTED_IDENTIFIER Off
Set @OPENTRY_TY = CHAR(39)+'OB'+CHAR(39)
Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
				+ (DATEPART(ss, GETDATE()) * 1000 )
				+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
Set @TBLNAME1 = '##TMP1'+@TBLNM
Set @TBLNAME2 = '##TMP2'+@TBLNM
Set @TBLNAME3 = '##TMP3'+@TBLNM
Set @TBLNAME11 = '##TMP11'+@TBLNM
Set @TBLNAME12 = '##TMP12'+@TBLNM

SET @GRP='SUNDRY CREDITORS'
	
DECLARE openingentry_cursor CURSOR FOR
	SELECT entry_ty FROM lcode
	WHERE bcode_nm = 'OB'
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @opentries
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   Set @OPENTRY_TY = @OPENTRY_TY +','+CHAR(39)+@opentries+CHAR(39)
	   FETCH NEXT FROM openingentry_cursor into @opentries
	END
	CLOSE openingentry_cursor
	DEALLOCATE openingentry_cursor

CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
SET @LVL=0
INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME=@GRP
SET @MCOND=1
WHILE @MCOND=1
BEGIN
	IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
	BEGIN
		PRINT @LVL
		INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
		SET @LVL=@LVL+1
	END
	ELSE
	BEGIN
		SET @MCOND=0	
	END
END


SELECT AC_ID,AC_NAME INTO #ACMAST FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)
PRINT @FCON


SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT AC_MAST.AC_NAME,AC.AC_ID,AC.AMOUNT,AC.AMT_TY,AC.ACSERIAL,MN.ENTRY_TY,MN.DATE,MN.TRAN_CD,MN.L_YN,MN.INV_NO,MN.DUE_DT,MN.PINVNO AS U_PINVNO,MN.PINVDT AS U_PINVDT'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',BILLAMT=AC.AMOUNT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RECAMT=SUM(case when (AC.entry_ty=MLL.entry_all and AC.tran_cd =MLL.main_tran and AC.acserial =MLL.acseri_all and AC.AC_ID=MLL.AC_ID) then ISNULL(MLL.NEW_ALL,0)+ISNULL(MLL.TDS,0)+ISNULL(MLL.DISC,0) else 0 end)'
--Birendra : Multi Currency :Start:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC.FCID,AC.FCAMOUNT,FCBILLAMT=AC.FCAMOUNT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',FCRECAMT=SUM(case when (AC.entry_ty=MLL.entry_all and AC.tran_cd =MLL.main_tran and AC.acserial =MLL.acseri_all and AC.AC_ID=MLL.AC_ID) then ISNULL(MLL.FCNEW_ALL,0)+ISNULL(MLL.FCTDS,0)+ISNULL(MLL.FCDISC,0) else 0 end)'
--Birendra : Multi Currency :End:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',MN.U_BROKER '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INTO '+@TBLNAME1
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM LAC_multi_VW AC '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=AC.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN LMAIN_multi_VW MN ON (AC.ENTRY_TY=MN.ENTRY_TY AND AC.TRAN_CD=MN.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN MAINALL_multi_VW MLL ON (AC.entry_ty=MLL.entry_all and AC.tran_cd =MLL.main_tran and AC.acserial =MLL.acseri_all and AC.AC_ID=MLL.AC_ID) AND MLL.DATE <= '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN #ACMAST AM ON (AC_MAST.AC_ID=AM.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' AND MN.TDSPAYTYPE<>3' 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'GROUP BY AC_MAST.AC_NAME,AC.AC_ID,AC.AMOUNT,AC.AMT_TY,MN.ENTRY_TY,MN.DATE,MN.TRAN_CD,MN.L_YN,MN.INV_NO,MN.DUE_DT,MN.PINVNO,MN.PINVDT,AC.RE_ALL,AC.TDS,AC.ACSERIAL,U_Broker'
--Birendra : Multi Currency :Start:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC.FCID,AC.FCAMOUNT'
--Birendra : Multi Currency :End:
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT AC_MAST.AC_NAME,AC.AC_ID,AC.AMOUNT,AC.AMT_TY,AC.ACSERIAL,MN.ENTRY_TY,MN.DATE,MN.TRAN_CD,MN.L_YN,MN.INV_NO,MN.DUE_DT,MN.PINVNO AS U_PINVNO,MN.PINVDT AS U_PINVDT'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',BILLAMT=AC.AMOUNT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RECAMT=sum(case when (AC.entry_ty=MLY.entry_ty and AC.tran_cd =MLY.tran_cd and ac.acserial = Mly.acserial and AC.AC_ID=MLY.AC_ID) then case when ISNULL(MLY.NEW_ALL,0) = 0 then ISNULL(MLY.TDS,0)+ISNULL(MLY.DISC,0) else ISNULL(MLY.NEW_ALL,0) end else 0 end)'
--Birendra : Multi Currency :Start:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC.FCID,AC.FCAMOUNT,FCBILLAMT=AC.FCAMOUNT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',FCRECAMT=sum(case when (AC.entry_ty=MLY.entry_ty and AC.tran_cd =MLY.tran_cd and ac.acserial = Mly.acserial and AC.AC_ID=MLY.AC_ID) then case when ISNULL(MLY.FCNEW_ALL,0) = 0 then ISNULL(MLY.TDS,0)+ISNULL(MLY.FCDISC,0) else ISNULL(MLY.FCNEW_ALL,0) end else 0 end)'
--Birendra : Multi Currency :End:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',MN.U_BROKER '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INTO '+@TBLNAME2
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM LAC_multi_VW AC '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=AC.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN LMAIN_multi_VW MN ON (AC.ENTRY_TY=MN.ENTRY_TY AND AC.TRAN_CD=MN.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN MAINALL_multi_VW MLY ON (AC.entry_ty=MLY.entry_ty and AC.tran_cd =MLY.tran_cd and ac.acserial = Mly.acserial and AC.AC_ID=MLY.AC_ID) AND MLY.DATE_ALL <= '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN #ACMAST AM ON (AC_MAST.AC_ID=AM.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' AND MN.TDSPAYTYPE<>3' 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'GROUP BY AC_MAST.AC_NAME,AC.AC_ID,AC.AMOUNT,AC.AMT_TY,MN.ENTRY_TY,MN.DATE,MN.TRAN_CD,MN.L_YN,MN.INV_NO,MN.DUE_DT,MN.PINVNO,MN.PINVDT,AC.RE_ALL,AC.TDS,AC.ACSERIAL,U_Broker'
--Birendra : Multi Currency :Start:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC.FCID,AC.FCAMOUNT'
--Birendra : Multi Currency :End:
print @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT AC.ENTRY_TY,AC.TRAN_CD,AC.AC_ID,AC.ACSERIAL'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RECAMT=AMOUNT'
--Birendra : Multi Currency :Start:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',AC.FCID,FCRECAMT=FCAMOUNT'
--Birendra : Multi Currency :End:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INTO '+@TBLNAME11
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM LAC_multi_VW AC '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' WHERE RE_ALL = 0 AND TDS != 0 '
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND


SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.AC_NAME,A.AMOUNT,A.AMT_TY,A.ACSERIAL,A.ENTRY_TY,A.DATE,A.TRAN_CD,A.L_YN,A.INV_NO,A.DUE_DT,A.U_PINVNO,A.U_PINVDT'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',A.BILLAMT,RECAMT=A.RECAMT+ISNULL(B.RECAMT,0) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',BALAMT=A.BILLAMT-(A.RECAMT+ISNULL(B.RECAMT,0))'
--Birendra : Multi Currency :Start:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',A.FCID,A.FCAMOUNT'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',A.FCBILLAMT,FCRECAMT=A.FCRECAMT+ISNULL(B.FCRECAMT,0) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',FCBALAMT=A.FCBILLAMT-(A.FCRECAMT+ISNULL(B.FCRECAMT,0))'
--Birendra : Multi Currency :End:
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',A.U_BROKER '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INTO '+@TBLNAME12
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME1+' A '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN '+@TBLNAME2+' B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ACSERIAL=B.ACSERIAL) AND A.AC_ID = B.AC_ID '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN '+@TBLNAME11+' C ON (A.ENTRY_TY=C.ENTRY_TY AND A.TRAN_CD=C.TRAN_CD) AND A.AC_ID = C.AC_ID AND A.ACSERIAL=C.ACSERIAL'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

print @SQLCOMMAND
SET @SQLCOMMAND = ''

SET @SQLCOMMAND = 'DECLARE @OPTRAN_CD as INT,@OPDATE as DATETIME,@OPACNAME as varchar(250) DECLARE openingentry_cursor CURSOR FOR
	SELECT TRAN_CD,AC_NAME,DATE FROM '+@TBLNAME12+' WHERE 
	ENTRY_TY IN ('+@OPENTRY_TY+') 
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   DELETE FROM '+@TBLNAME12+' WHERE ENTRY_TY IN ('+@OPENTRY_TY+') AND TRAN_CD = @OPTRAN_CD
			AND AC_NAME IN (SELECT AC_NAME FROM '+@TBLNAME12+' WHERE AC_NAME = @OPACNAME AND DATE < @OPDATE )
	   FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
	END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor'

EXECUTE SP_EXECUTESQL @SQLCOMMAND


IF RTRIM(LTRIM(@DAYS1))<>'0' 
BEGIN
SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT *'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS1AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS1+') THEN FCBALAMT ELSE 0 END) ' /**/
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS2AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS1+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS2+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS3AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS2+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS3+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS4AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS3+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS5AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>'+@DAYS4+') THEN FCBALAMT ELSE 0 END ) into '+@TBLNAME3
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE BALAMT <> 0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE FCBALAMT <> 0 '+CASE WHEN @JV_ALLOC=0  THEN ' AND ENTRY_TY<>''JV''' ELSE '' END +'' 

PRINT 'A'
END

IF LTRIM(RTRIM(@DAYS1))='0' AND LTRIM(RTRIM(@DAYS2))<>'0' 
BEGIN
SET @SQLCOMMAND=' '

SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT *'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS1AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS2+') THEN FCBALAMT ELSE 0 END) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS2AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS2+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS3+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS3AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS3+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS4AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>'+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS5AMT=0 into '+@TBLNAME3
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE BALAMT <> 0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE FCBALAMT <> 0 '+CASE WHEN @JV_ALLOC=0  THEN ' AND ENTRY_TY<>''JV''' ELSE '' END +'' -- added By  Satish Pal 31/10/2011  FOR TKT-9489

PRINT 'B'
END

IF LTRIM(RTRIM(@DAYS1))='0' AND LTRIM(RTRIM(@DAYS2))='0' AND LTRIM(RTRIM(@DAYS3))<>'0'
BEGIN
SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS1AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS3+') THEN FCBALAMT ELSE 0 END) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS2AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS3+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS3AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>'+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS4AMT=0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS5AMT=0 into '+@TBLNAME3
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE BALAMT <> 0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE FCBALAMT <> 0 '+CASE WHEN @JV_ALLOC=0  THEN ' AND ENTRY_TY<>''JV''' ELSE '' END +'' -- added By  Satish Pal 31/10/2011  FOR TKT-9489

PRINT 'C'

END

IF LTRIM(RTRIM(@DAYS1))='0' AND LTRIM(RTRIM(@DAYS2))='0' AND LTRIM(RTRIM(@DAYS3))='0' AND LTRIM(RTRIM(@DAYS4))<>'0'
BEGIN
SET @SQLCOMMAND=' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT *'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS1AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS4+') THEN FCBALAMT ELSE 0 END) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS2AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>'+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS3AMT=0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS4AMT=0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS5AMT=0 into '+@TBLNAME3
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE BALAMT <> 0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE FCBALAMT <> 0 '+CASE WHEN @JV_ALLOC=0  THEN ' AND ENTRY_TY<>''JV''' ELSE '' END +'' -- added By  Satish Pal 31/10/2011  FOR TKT-9489
PRINT 'D'

END


IF LTRIM(RTRIM(@DAYS1))<>'0' AND LTRIM(RTRIM(@DAYS2))<>'0' AND LTRIM(RTRIM(@DAYS3))<>'0' AND LTRIM(RTRIM(@DAYS4))<>'0'
BEGIN
SET @SQLCOMMAND=' '

SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT *'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS1AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS1+') THEN FCBALAMT ELSE 0 END) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS2AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS1+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS2+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS3AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS2+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS3+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS4AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>' +@DAYS3+') AND ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT<='+@DAYS4+') THEN FCBALAMT ELSE 0 END )'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DAYS5AMT=(CASE WHEN ('+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>'+@DAYS4+') THEN FCBALAMT ELSE 0 END ) into '+@TBLNAME3
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM '+@TBLNAME12+ ' WHERE FCBALAMT <> 0'
PRINT 'E'

END
If @FILTERDATE<>'due_dt'    -----added condition by satish  dt. 03/10/2011  for tkt--8554
begin
-- Added by Sandeep dt.24/06/2011 for TKT-8356--->Start
if @Days5=@Days4
begin
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' And '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+'-DUE_DT>'+@DAYS4
end
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY INV_NO' 

PRINT @SQLCOMMAND
end
else
begin
-- Added by Sandeep dt.24/06/2011 for TKT-8356--->Start
if @Days5=@Days4
begin
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' And '+CHAR(39)+CAST(DATEADD(day,1,@SDATE) as varchar(50))+CHAR(39)+'-DUE_DT>'+@DAYS4
end
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY INV_NO' 

PRINT @SQLCOMMAND
end
PRINT 'F'
-- Added by Sandeep Dt.24/06/2011 for TKT-8356<---End
EXECUTE SP_EXECUTESQL @SQLCOMMAND

---Assigning Column Caption--Start
IF LTRIM(RTRIM(@DAYS1))<>'0' AND LTRIM(RTRIM(@DAYS2))<>'0' AND LTRIM(RTRIM(@DAYS3))<>'0' AND LTRIM(RTRIM(@DAYS4))<>'0'
BEGIN
	SET @COLCAP1='<='+LTRIM(RTRIM(@DAYS1))+' '+'DAYS'
	SET @COLCAP2='>'+LTRIM(RTRIM(@DAYS1))+' AND '+'<='+LTRIM(RTRIM(@DAYS2))+' '+'DAYS'
	SET @COLCAP3='>'+LTRIM(RTRIM(@DAYS2))+' AND '+'<='+LTRIM(RTRIM(@DAYS3))+' '+'DAYS'
	SET @COLCAP4='>'+LTRIM(RTRIM(@DAYS3))+' AND '+'<='+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP5='>'+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
END
IF LTRIM(RTRIM(@DAYS1))='0' AND LTRIM(RTRIM(@DAYS2))<>'0' AND LTRIM(RTRIM(@DAYS3))<>'0' AND LTRIM(RTRIM(@DAYS4))<>'0'
BEGIN
	SET @COLCAP1='<='+LTRIM(RTRIM(@DAYS2))+' '+'DAYS'
	SET @COLCAP2='>'+LTRIM(RTRIM(@DAYS2))+' AND '+'<='+LTRIM(RTRIM(@DAYS3))+' '+'DAYS'
	SET @COLCAP3='>'+LTRIM(RTRIM(@DAYS3))+' AND '+'<='+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP4='>'+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP5=' '
END
IF LTRIM(RTRIM(@DAYS1))='0' AND LTRIM(RTRIM(@DAYS2))='0' AND LTRIM(RTRIM(@DAYS3))<>'0' AND LTRIM(RTRIM(@DAYS4))<>'0'
BEGIN
	SET @COLCAP1='<='+LTRIM(RTRIM(@DAYS3))+' '+'DAYS'
	SET @COLCAP2='>'+LTRIM(RTRIM(@DAYS3))+' AND '+'<='+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP3='>'+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP4=' '
	SET @COLCAP5=' '
END
IF LTRIM(RTRIM(@DAYS1))='0' AND LTRIM(RTRIM(@DAYS2))='0' AND LTRIM(RTRIM(@DAYS3))='0' AND LTRIM(RTRIM(@DAYS4))<>'0'
BEGIN
	SET @COLCAP1='<='+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP2='>'+LTRIM(RTRIM(@DAYS4))+' '+'DAYS'
	SET @COLCAP3=' '
	SET @COLCAP4=' '
	SET @COLCAP5=' '
END
PRINT 'COLUMN CAPTION'
PRINT @COLCAP1
PRINT @COLCAP2
PRINT @COLCAP3
PRINT @COLCAP4
PRINT @COLCAP5

---Assigning Column Caption--End


--FOR BALANCE
SELECT 
AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY
,MN.L_YN,MN.INV_NO
,AC_MAST.AC_ID,AC_MAST.AC_NAME
--Birendra : Multi Currency
,AC.FCID,AC.FCAMOUNT
INTO #AC_BAL1 
FROM LAC_multi_VW AC
INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)
INNER JOIN LMAIN_multi_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY)
WHERE 1=2

SET @SQLCOMMAND = ''


SET @SQLCOMMAND = 'INSERT INTO #AC_BAL1
SELECT 
AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.AMOUNT,AC.AMT_TY
,MN.L_YN,MN.INV_NO
,AC_MAST.AC_ID,AC_MAST.AC_NAME
,AC.FCID,AC.FCAMOUNT
FROM LAC_multi_VW AC
INNER JOIN AC_MAST  ON (AC.AC_ID = AC_MAST.AC_ID)
INNER JOIN LMAIN_multi_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) '+RTRIM(@FCON)
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

DELETE FROM #AC_BAL1 WHERE 
DATE < (SELECT TOP 1 DATE FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN)
AND AC_NAME IN (SELECT AC_NAME FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN GROUP BY AC_NAME) 


SELECT AC_NAME,AC_ID,fcid,LBAL=SUM(CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END)
,DAYS1=@COLCAP1,DAYS2=@COLCAP2,DAYS3=@COLCAP3,DAYS4=@COLCAP4,DAYS5=@COLCAP5 
--Birendra : Multi Currency
,FCLBAL=SUM(CASE WHEN AMT_TY='DR' THEN FCAMOUNT ELSE -FCAMOUNT END)
INTO #AC_BAL
FROM #AC_BAL1
GROUP BY AC_NAME,AC_ID,fcid
ORDER BY AC_NAME,AC_ID,fcid
--FOR BALANCE
DECLARE @COND VARCHAR(500) 
set @cond =''
IF @EAMT>0 
	SET @COND='WHERE BALAMT BETWEEN '+CONVERT(VARCHAR(50),@SAMT)+' AND ' +CONVERT(VARCHAR(50),@EAMT)
ELSE 
	IF @SAMT>0 
		SET @COND='WHERE BALAMT <= '+CONVERT(VARCHAR(50),@SAMT)
	ELSE
		SET @COND=''
if charindex('and',@expara1)>0
	set @COND=@COND+case when charindex('where',@cond)>0 then' and ' else ' Where ' end +' A.U_BROKER IN (SELECT U_BROKER FROM PTMAIN WHERE 1<2 '+@EXPARA1+ ')'

SET @SQLCOMMAND=' '
SET @SQLCOMMAND = ' SELECT A.*,B.FCLBAL,B.LBAL,B.DAYS1,B.DAYS2,B.DAYS3,B.DAYS4,B.DAYS5,C.CURRDESC,C.SYMBOL FROM '+@TBLNAME3 +' A INNER JOIN #AC_BAL B ON (A.AC_NAME=B.AC_NAME and a.fcid=b.fcid) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN CURR_MAST C ON C.CURRENCYID=A.FCID ' +@COND 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY A.AC_NAME,A.DATE' 

PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND = ' SELECT * FROM '+@TBLNAME3


SET @SQLCOMMAND=' '
SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME3
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME11
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=' '
SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME12
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET ANSI_NULLS OFF





