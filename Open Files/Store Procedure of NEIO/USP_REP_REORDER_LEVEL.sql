If Exists(Select [Name] From SysObjects Where xType='P' and Id=Object_Id(N'USP_REP_REORDER_LEVEL'))
Begin
	Drop Procedure USP_REP_REORDER_LEVEL
End
Go
-- =============================================
-- CREATE DATE: 24/02/2009
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE REORDER LEVEL REPORT.
-- MODIFY DATE: 28/02/2009
-- MODIFIED BY: Rupesh Prajapati 13/02/2010. Add It_Mast in join. 
-- Modified By : Shrikant S. On 02/10/2010 For TKT-4120 
-- Modified By: Ramya on 05/01/2011 For TKT-4819
-- Modified By: Rupesh 05/08/2011 For TKT-7975
-- Modified By: Shrikant S. 13/07/2012 For BUG-5101
-- REMARK:
-- =============================================
Create PROCEDURE  [dbo].[USP_REP_REORDER_LEVEL] 
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS
DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
--Commented by Archana K. on 15/10/12 for Bug-6547 start
--,@VSDATE=@SDATE 
--,@VEDATE=@EDATE
--Commented by Archana K. on 15/10/12 for Bug-6547 end
--Changed by Archana K. on 15/10/12 for Bug-6547 start
,@VSDATE=NUll
,@VEDATE=NULL
--Changed by Archana K. on 15/10/12 for Bug-6547 end
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='A',@VITFILE='A',@VACFILE=' '
,@VDTFLD ='DATE'
,@VLYN=@LYN
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

PRINT @FCON
DECLARE @TBLNAME1 AS VARCHAR(50),@TBLNM AS VARCHAR(50)
SET @TBLNM = (SELECT SUBSTRING(RTRIM(LTRIM(STR(RAND( (DATEPART(MM, GETDATE()) * 100000 )
					+ (DATEPART(SS, GETDATE()) * 1000 )
					+ DATEPART(MS, GETDATE())) , 20,15))),3,20) AS NO)
	SET @TBLNAME1 = '##TMP1'+@TBLNM
	
SELECT A.DATE ,A.IT_CODE,A.AC_ID Into #tmpItemTable FROM PTITEM A Where 1=2		----Added by Ramya on 05/01/2011 For TKT-4819		
--Added By Shrikant S. on 13/07/2012 for BUG-5101		--Start
Select Date,it_code,ac_id,Inv_no,l_yn 
Into #Item From PTITEM Where dc_no=''
union all 
Select Date,it_code,ac_id,Inv_no,l_yn From arITEM Where dc_no=''
--Added By Shrikant S. on 13/07/2012 for BUG-5101		--End

DECLARE @SQLCOMMAND NVARCHAR(4000),@VCOND NVARCHAR(1000)
SET @SQLCOMMAND=' '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' SELECT MAX(A.DATE) DATE,A.IT_CODE,A.AC_ID INTO '+@TBLNAME1 +' FROM PTITEM A  '	--Commented by Ramya on 05/01/2011 For TKT-4819		
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'Insert Into #tmpItemTable SELECT MAX(A.DATE) DATE,A.IT_CODE,AC_ID=0 FROM PTITEM A  '		--Commented By Shrikant S. on 13/07/2012 for BUG-5101
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'Insert Into #tmpItemTable SELECT MAX(A.DATE) DATE,A.IT_CODE,AC_ID=0 FROM #item A  '		--Added By Shrikant S. on 13/07/2012 for BUG-5101
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' inner join it_mast on (a.it_code=it_mast.it_code AND it_mast.reorder>0)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' GROUP BY A.IT_CODE  ORDER BY A.DATE DESC '                 --Commented by Suraj K. for Autoupdater 11.0.8.0 on date 10-01-2015
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' GROUP BY A.IT_CODE '  --Added Suraj K. for Autoupdater 11.0.8.0 on date 10-01-2015

--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' GROUP BY A.IT_CODE,A.AC_ID ORDER BY A.DATE DESC '		--Commented by Ramya on 05/01/2011 For TKT-4819		
print @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND

--Added by Ramya on 05/01/2011 For TKT-4819		--Start
declare @it_code numeric(18,0)
declare @ac_id int
declare c1 cursor for
select distinct it_code from #tmpItemTable
open c1
fetch Next From c1 into @it_code
WHILE @@FETCH_STATUS = 0
Begin
--	select Top 1 @ac_id=ac_id  from ptitem		--Commented by Shrikant S. on 13/07/2012 for Bug-5101			
	select Top 1 @ac_id=ac_id  from #item		--Added by Shrikant S. on 13/07/2012 for Bug-5101		
			where it_code=@it_code
				order by date,inv_no desc
	update #tmpItemTable set ac_id=@ac_id where It_code=@it_code
fetch Next From c1 into @it_code
End
close c1
Deallocate c1
--Added by Ramya on 05/01/2011 For TKT-4819		--End

--Commented by Archana K. on 28/11/12 for Bug-6547(Issue in Date Filter) start

--Declare @QtyInwardFldList Varchar(2000),@QtyOutWardFldList Varchar(2000)
--Select @QtyInwardFldList=isnull(substring((Select '+ B.' +Entry_ty+'Qty ' From Lcode Where Inv_Stk='+' and bcode_nm='' For XML Path('')),2,500),'')
--,@QtyOutWardFldList=isnull(substring((Select '+ B.' +Entry_ty+'Qty ' From Lcode Where Inv_Stk='-' and bcode_nm='' For XML Path('')),2,500),'')
----print @QtyInwardFldList
----print @QtyOutWardFldList
--
--SET @SQLCOMMAND=' SELECT MAILNAME=(CASE WHEN C.MAILNAME IS NULL THEN '''' ELSE C.MAILNAME END),'
----Added the Below Lines by Shrikant S. on 13/07/2012 for Bug-5101		--Start
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' IT_MAST.REORDER,IT_MAST.P_UNIT,IT_MAST.IT_NAME,'+@QtyInwardFldList+' AS INWARD,'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+@QtyOutWardFldList+'  AS OUTWARD,'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ('+@QtyInwardFldList+')-('+@QtyOutWardFldList+') AS BALQTY'
----Added by Shrikant S. on 13/07/2012 for Bug-5101		--End
--
----Commented the Below Lines by Shrikant S. on 13/07/2012 for Bug-5101		--Start
----SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' IT_MAST.REORDER,IT_MAST.P_UNIT,IT_MAST.IT_NAME,B.ARQTY+B.ESQTY+B.IRQTY+B.OPQTY+B.OSQTY+B.PTQTY+B.SRQTY AS INWARD,'
----SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' B.SSQTY+B.STQTY+B.PRQTY+B.IIQTY+B.IPQTY+B.DCQTY AS OUTWARD,'
----SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' (B.ARQTY+B.ESQTY+B.IRQTY+B.OPQTY+B.OSQTY+B.PTQTY+B.SRQTY)-(B.SSQTY+B.STQTY+B.PRQTY+B.IIQTY+B.IPQTY+B.DCQTY) AS BALQTY'
----Commented by Shrikant S. on 13/07/2012 for Bug-5101		--End
--
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM IT_MAST  '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN #tmpItemTable D ON IT_MAST.IT_CODE = D.IT_CODE '  --Added by Ramya
----SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN '+@TBLNAME1+' D ON IT_MAST.IT_CODE = D.IT_CODE ' --Commented by Ramya
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN AC_MAST C ON D.AC_ID = C.AC_ID '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IT_BAL B ON IT_MAST.IT_CODE=B.IT_CODE '
--if @TMPIT<>''			--Added by Shrikant S. On 02/10/2010 For TKT-4120 
--Begin
--	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN '+@TMPIT+' E ON IT_MAST.IT_NAME=E.IT_NAME '			--Added by Shrikant S. On 02/10/2010 For TKT-4120 
--End
----Added the Below Lines by Shrikant S. on 13/07/2012 for Bug-5101		--Start
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' WHERE ('+@QtyInwardFldList+')-'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ('+@QtyOutWardFldList+') < IT_MAST.REORDER '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND (IT_MAST.IT_NAME BETWEEN '''+@SIT+''' AND '''+@EIT+''')'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IT_MAST.REORDER >0'		--Shrikant S. 13/07/2012 For BUG-5101
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' order by (IT_MAST.REORDER-  (('+@QtyInwardFldList+')-('+@QtyOutWardFldList+'))  ) desc   '
----Added by Shrikant S. on 13/07/2012 for Bug-5101		--End
--Commented by Archana K. on 28/11/12 for Bug-6547(Issue in Date Filter) End

--Changed by Archana K. on 28/11/12 for Bug-6547(Issue in Date Filter) start

SET @SQLCOMMAND=' SELECT MAILNAME=(CASE WHEN C.MAILNAME IS NULL THEN '''' ELSE C.MAILNAME END),'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' IT_MAST.REORDER,IT_MAST.P_UNIT,IT_MAST.IT_NAME,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' BalQty=sum((CASE WHEN L.inv_stk=''+'' THEN ISNULL(+B.Qty,0) ELSE ISNULL(-B.QTY,0) END))'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM IT_MAST'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN #tmpItemTable D ON IT_MAST.IT_CODE = D.IT_CODE ' 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN AC_MAST C ON D.AC_ID = C.AC_ID '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IT_BAL B ON IT_MAST.IT_CODE=B.IT_CODE ' -- Commented by Archana K. on 4/12/2012 for Bug-6547 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IT_BALW B ON IT_MAST.IT_CODE=B.IT_CODE AND B.Date<='''+convert(varchar(50),@sdate)+'''' --Added by  Archana K. on 4/12/2012 for Bug-6547 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN Lcode L on B.entry_ty=L.entry_ty'
if @TMPIT<>''		
Begin
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN '+@TMPIT+' E ON IT_MAST.IT_NAME=E.IT_NAME '			
End
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'where (IT_MAST.IT_NAME BETWEEN '''+@SIT+''' AND '''+@EIT+''')'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IT_MAST.REORDER >0'	
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' Group by (CASE WHEN C.MAILNAME IS NULL THEN '''' ELSE C.MAILNAME END),IT_MAST.REORDER,IT_MAST.P_UNIT,IT_MAST.IT_NAME'

IF  CHARINDEX('ANS=NO', @EXPARA)<>0 
BEGIN
	print @EXPARA
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' having (IT_MAST.REORDER-sum((CASE WHEN L.inv_stk=''+'' THEN ISNULL(+B.Qty,0) ELSE ISNULL(-B.QTY,0) END))>0)'
END

--Changed by Archana K. on 28/11/12 for Bug-6547(Issue in Date Filter) end

--Commented the Below Lines by Shrikant S. on 13/07/2012 for Bug-5101		--Start
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' WHERE (B.ARQTY+B.ESQTY+B.IRQTY+B.OPQTY+B.OSQTY+B.PTQTY+B.SRQTY)-'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' (B.SSQTY+B.STQTY+B.PRQTY+B.IIQTY+B.IPQTY+B.DCQTY) < IT_MAST.REORDER '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' order by (IT_MAST.REORDER-  ((B.ARQTY+B.ESQTY+B.IRQTY+B.OPQTY+B.OSQTY+B.PTQTY+B.SRQTY)-(B.SSQTY+B.STQTY+B.PRQTY+B.IIQTY+B.IPQTY+B.DCQTY))  ) desc   '/*TKT-7975*/
--Commented by Shrikant S. on 13/07/2012 for Bug-5101		--End


PRINT @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND



------Added by on 30/11/12 for Bug-6547(Issue in Date Filter) start

----drop table ##tmpItemTable1   

--Added by on 28/11/12 for Bug-6547(Issue in Date Filter) end
drop table #tmpItemTable/*TKT-7975*/

--SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1			--Commented by Ramya on 05/01/2011 For TKT-4819		
--EXECUTE SP_EXECUTESQL @SQLCOMMAND


