set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 15/06/2009
-- Description:	This Stored procedure is useful to generate BANK INTEREST CALCULATION Report.
-- Modify date: 16/06/2009
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================
ALTER PROCEDURE [dbo].[USP_REP_BANK_INTEREST_CALCULATION]
--@SDATE AS SMALLDATETIME,@EDATE AS SMALLDATETIME, @SAC AS VARCHAR(100), @EAC AS VARCHAR(100)
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
Declare @date smalldatetime
declare @irate1 decimal(17,3)

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=null,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='MN',@VITFILE=Null,@VACFILE='AC'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

select @irate1=cast(@expara as decimal(17,3))  where ISNUMERIC( @expara )=1 

if isnull(@irate1,0)=0
begin
	set @irate1=0
end
print 'r--'
print @irate1

SELECT 
cd=9,AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.debit,ac.credit,AC.AMT_TY,AC_MAST.AC_NAME,cl_date,l_yn,ac_mast.i_rate
INTO #AC_BAL1
FROM bankreco AC
INNER JOIN AC_MAST  ON (AC.ac_name = AC_MAST.AC_NAME)
INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) 
WHERE 1=2


SET @SQLCOMMAND='INSERT INTO #AC_BAL1 SELECT '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'cd=1,AC.TRAN_CD,AC.ENTRY_TY,AC.DATE,AC.debit,ac.credit,AC.AMT_TY,AC_MAST.AC_NAME'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',cl_date=(case when l.entry_ty=''OB'' or l.bcode_nm=''OB'' then ac.date else ac.cl_date end),l_yn,irate='+(case when @irate1=0 then 'ac_mast.i_rate' else rtrim(cast(@irate1 as varchar)) end)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'FROM bankreco AC'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN AC_MAST  ON (AC.ac_name = AC_MAST.AC_NAME)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN LMAIN_VW MN ON (AC.TRAN_CD = MN.TRAN_CD AND AC.ENTRY_TY = MN.ENTRY_TY) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'inner join lcode l on (l.entry_ty=ac.entry_ty)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND
delete from #AC_BAL1 where isnull(cl_date,'')=''

INSERT INTO #AC_BAL1 (cd,TRAN_CD,ENTRY_TY,DATE,debit,credit,AMT_TY,AC_NAME,cl_date,l_yn,i_rate)
select cd=2,0,'',date
,debit=(case when amt_ty='DR' then amount else 0 end)
,credit=(case when amt_ty='CR' then amount else 0 end)
,AMT_TY,recostat.AC_NAME,cl_date,l_yn='',irate=(case when @irate1=0 then ac_mast.i_rate else @irate1 end)
from recostat
INNER JOIN AC_MAST  ON (recostat.ac_name = AC_MAST.AC_NAME)




DELETE FROM #AC_BAL1 WHERE 
DATE < (SELECT TOP 1 DATE FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN)
AND AC_NAME IN (SELECT AC_NAME FROM #AC_BAL1 WHERE ENTRY_TY IN ('OB') AND L_YN = @LYN GROUP BY AC_NAME) and cd=1



--SELECT * FROM #AC_BAL1 b 


set @date=@sdate
select ac_name,date,balamt=debit,irate=debit,iamt=debit into #breco from bankreco where 1=2
while(@date <=@edate)
begin
	insert into #breco (ac_name,date,balamt,irate,iamt)
	SELECT AC_NAME,@date
	,BALAMT=sum(   CASE WHEN cd=2 THEN -debit+credit ELSE debit-credit   END)
	,b.i_rate,0
	FROM #AC_BAL1 b 
	where (cd=1 and cl_date<=@date) or (cd=2 and cl_date>@date)
	GROUP BY b.AC_NAME,b.i_rate
	print @date
	set @date= dateadd(dd,1,@date)
end
update #breco set iamt=(irate*balamt)/100
select * from #breco order by ac_name,date
--
--select b.entry_ty,b.tran_cd
--,date=(case when l.entry_ty='OB' or l.bcode_nm='OB' then b.date else b.cl_date end)
--,b.ac_name,b.debit,b.credit 
--into #breco
--from bankreco b
--inner join ac_mast on (ac_mast.ac_name=b.ac_name)
--inner join lcode l on (l.entry_ty=b.entry_ty)
--where ac_mast.typ='BANK' and 1=2
--order by ac_mast.ac_name,b.date



--SELECT AC_NAME,AC_ID,BALAMT=SUM(CASE WHEN AMT_TY='DR' THEN AMOUNT ELSE -AMOUNT END)
--FROM #AC_BAL1
--GROUP BY AC_NAME,AC_ID
--ORDER BY AC_NAME,AC_ID
--
--select ac_name,balamt=sum(case when amt_ty='DR' then -amount else amount end) 
--from recostat where Date<=@sdate and (cl_date>=@sdate)
--group by ac_name


