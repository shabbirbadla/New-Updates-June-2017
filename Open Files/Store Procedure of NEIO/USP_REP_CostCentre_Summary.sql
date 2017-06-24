if exists(select [name] from sysobjects where [name]='USP_REP_CostCentre_Summary' AND XTYPE='P')
DROP PROCEDURE USP_REP_CostCentre_Summary
GO
/****** Object:  StoredProcedure [dbo].[USP_REP_CostCentre_Summary]    Script Date: 06/07/2012 09:51:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Created by  : Birendra Prasad
-- Date        : 24/03/2012
-- Modified by : Birendra for Bug-7258 on 06/11/2012 for filter report on base of costunder
-- Modified by : Pankaj B. for Bug-25466(Issue Seen While Opening Cost Center Summary Report) on 02-03-2015

CREATE PROCEDURE [dbo].[USP_REP_CostCentre_Summary]
	@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
	@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),
	@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
	@SAMT NUMERIC,@EAMT NUMERIC,
	@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
	@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),
	@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
	@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
	@FINYR NVARCHAR(20), @EXTPAR NVARCHAR(1000)
	AS
Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000)
	Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
	Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50),@TBLNAME3 as VARCHAR(50)
declare @loop_counter bit
	
--	Set @OPENTRY_TY = '''OB'''
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM
--	Set @TBLNAME3 = '##TMP3'+@TBLNM --&& Added by Shrikant S. by on 05 Feb, 2010
--	DECLARE openingentry_cursor CURSOR FOR
--		SELECT entry_ty FROM lcode
--		WHERE bcode_nm = 'OB'
--	OPEN openingentry_cursor
--	FETCH NEXT FROM openingentry_cursor into @opentries
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--	   Set @OPENTRY_TY = @OPENTRY_TY +','''+@opentries+''''
--	   FETCH NEXT FROM openingentry_cursor into @opentries
--	END
--	CLOSE openingentry_cursor
--	DEALLOCATE openingentry_cursor
--
	EXECUTE USP_REP_FILTCON 
		@VTMPAC=@TMPAC,@VTMPIT=null,@VSPLCOND=@SPLCOND,
		@VSDATE=@SDATE,@VEDATE=@EDATE,
		@VSAC =@SNAME,@VEAC =@ENAME,
		@VSIT=null,@VEIT=null,
		@VSAMT=@SAMT,@VEAMT=@EAMT,
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =null,@VEWARE  =null,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='A',@VITFILE=null,@VACFILE=null,
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@EXTPAR,
		@VFCON =@FCON OUTPUT
print @FCON
--	SET @SQLCOMMAND = ''
--	SET @SQLCOMMAND = 'SELECT a.*,b.amt_ty,C.COSTUNDER,D.COST_CAT_ID,D.COST_CAT_NAME FROM  CostAllocation_Vw a left join Lac_Vw b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd and a.inv_no=b.inv_no and a.ac_name=b.ac_name and a.l_yn=b.l_yn )'
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CEN_MAST C ON (C.COST_CEN_NAME=A.COST_CEN_NAME AND C.COST_CEN_ID=A.COST_CEN_ID)'	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CAT_MAST D ON (D.COST_CAT_ID=C.COST_CAT_ID)'	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'ORDER BY D.COST_CAT_NAME,a.cost_cen_name'	
--	EXECUTE SP_EXECUTESQL @SQLCOMMAND

--Birendra : Bug-7258 for cost centre filter on 06/11/2012 :Start:
	declare @CostCondition varchar(1000)
	set @CostCondition=''
	if charindex('$>Cost',@EXTPAR)<>0
	begin
		set @CostCondition=@EXTPAR
		SET @CostCondition=REPLACE(@CostCondition, '`','''')
		SET @CostCondition=REPLACE(@CostCondition, '[','C.[')
		set @CostCondition=substring(@CostCondition,charindex('$>Cost',@CostCondition)+6,len(@CostCondition)-(charindex('$>Cost',@CostCondition)+5))
		set @CostCondition=substring(@CostCondition,1,charindex('<$Cost',@CostCondition)-1)
	end
--Birendra : Bug-7258 for cost centre filter on 06/11/2012 :End:


--Biru -Testing
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT a.Entry_ty, A.date, A.doc_no, A.inv_no, A.tran_cd, A.Ac_name, A.ac_id, A.inv_sr, A.l_yn, A.date_all, A.compid, A.cost_cen_id, A.cost_cen_name, A.amount'	
	SET @SQLCOMMAND = @SQLCOMMAND +' '+',b.amt_ty,C.COSTUNDER,D.COST_CAT_ID,D.COST_CAT_NAME into '+@TBLNAME1+ ' FROM  CostAllocation_Vw a left join Lac_Vw b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd and a.inv_no=b.inv_no and a.ac_name=b.ac_name and a.l_yn=b.l_yn )'
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CEN_MAST C ON (C.COST_CEN_NAME=A.COST_CEN_NAME AND C.COST_CEN_ID=A.COST_CEN_ID)'	
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CAT_MAST D ON (D.COST_CAT_ID=C.COST_CAT_ID)'	
	SET @SQLCOMMAND = @SQLCOMMAND +' '+@FCON
	SET @SQLCOMMAND = @SQLCOMMAND +' '+@CostCondition ----Birendra : Bug-7258 for cost centre filter on 06/11/2012
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'ORDER BY D.COST_CAT_NAME,a.cost_cen_name'	
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

--Bug-25466 Start
	--SET @SQLCOMMAND = ''
	--SET @SQLCOMMAND = 'SELECT '''' AS Entry_ty,'''' AS date, '''' AS doc_no, '''' AS inv_no, '''' AS tran_cd, '''' AS Ac_name, 0 AS ac_id, '''' AS inv_sr, '''' AS l_yn, '''' AS date_all,'''' AS compid, 0 AS cost_cen_id, COSTUNDER AS cost_cen_name, 0 AS amount'	
	--SET @SQLCOMMAND = @SQLCOMMAND +' '+','''' AS amt_ty,COSTUNDER,COST_CAT_ID,COST_CAT_NAME into '+@TBLNAME2 +' FROM  '+@TBLNAME1 
	--SET @SQLCOMMAND = @SQLCOMMAND +' '+'where costunder not in(select cost_cen_name from '	+@TBLNAME1 +' group by cost_cen_name)'+' GROUP BY COSTUNDER,COST_CAT_ID,COST_CAT_NAME '


-------------
--	SET @SQLCOMMAND = 'SELECT '''' AS Entry_ty,'''' AS date, '''' AS doc_no, '''' AS inv_no, '''' AS tran_cd, '''' AS Ac_name, 0 AS ac_id, '''' AS inv_sr, '''' AS l_yn, '''' AS date_all,'''' AS compid, 0 AS cost_cen_id, COSTUNDER AS cost_cen_name, 0 AS amount'	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+','''' AS amt_ty,C.COSTUNDER,D.COST_CAT_ID,D.COST_CAT_NAME into '+@TBLNAME2 +' FROM  '+@TBLNAME1 + ' as a'
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CEN_MAST C ON (C.COST_CEN_NAME=A.COSTUNDER ) '	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CAT_MAST D ON (D.COST_CAT_ID=C.COST_CAT_ID)'	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'where A.costunder not in(select cost_cen_name from '	+@TBLNAME1 +' group by cost_cen_name)'+' GROUP BY C.COSTUNDER,D.COST_CAT_ID,D.COST_CAT_NAME '
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'LEFT JOIN COST_CAT_MAST D ON (D.COST_CAT_ID=C.COST_CAT_ID)'	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'ORDER BY D.COST_CAT_NAME,a.cost_cen_name'	
--print 	@SQLCOMMAND
--EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	SET @SQLCOMMAND = 'UPDATE '+@TBLNAME2+'  SET COSTUNDER=C.COSTUNDER'
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'FROM '+@TBLNAME2+ ' AS B INNER JOIN COST_CEN_MAST C ON (B.COST_CEN_NAME=C.COST_CEN_NAME AND B.COST_CAT_ID=C.COST_CAT_ID)'
--EXECUTE SP_EXECUTESQL @SQLCOMMAND
----	SET @SQLCOMMAND = 'SELECT * FROM '+@TBLNAME2
----EXECUTE SP_EXECUTESQL @SQLCOMMAND
--SET  @loop_counter=1
--while @loop_counter<>0
--begin
--	SET @SQLCOMMAND = 'INSERT INTO '+@TBLNAME2+'  (Entry_ty,date, doc_no,inv_no,tran_cd, Ac_name,ac_id,inv_sr, l_yn, date_all,compid, cost_cen_id, cost_cen_name,amount,amt_ty,COSTUNDER,COST_CAT_ID,COST_CAT_NAME)'
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+'SELECT DISTINCT B.Entry_ty,B.date, B.doc_no,B.inv_no,B.tran_cd, B.Ac_name,B.ac_id,B.inv_sr, B.l_yn, B.date_all,B.compid, B.cost_cen_id, C.cost_cen_name,B.amount,B.amt_ty,C.COSTUNDER,B.COST_CAT_ID,B.COST_CAT_NAME FROM '+@TBLNAME2
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+ ' AS B JOIN COST_CEN_MAST C ON (B.COSTUNDER=C.COST_CEN_NAME AND B.COST_CAT_ID=C.COST_CAT_ID) '
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+' WHERE B.COSTUNDER<>''PRIMARY'' AND B.COST_CEN_NAME NOT IN (select DISTINCT COSTUNDER from '	+@TBLNAME2 +' WHERE COSTUNDER<>''PRIMARY'')'
--EXECUTE SP_EXECUTESQL @SQLCOMMAND
----	SET @SQLCOMMAND = 'SELECT * FROM '+@TBLNAME2
----EXECUTE SP_EXECUTESQL @SQLCOMMAND

----
--	SET @SQLCOMMAND ='select DISTINCT costunder INTO TMPBIRU from '+@TBLNAME2+ ' where costunder<>''primary'''
-- 	SET @SQLCOMMAND = @SQLCOMMAND +' '+' AND COSTUNDER NOT IN(SELECT DISTINCT COST_CEN_NAME FROM '+@TBLNAME2+' )'
--EXECUTE SP_EXECUTESQL @SQLCOMMAND
--	IF NOT EXISTS (SELECT COSTUNDER FROM TMPBIRU) SET @LOOP_COUNTER=0
--DROP TABLE TMPBIRU
----PRINT @LOOP_COUNTER
--end



--	SET @SQLCOMMAND = 'SELECT * from '+@TBLNAME1
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+ ' union all '	
--	SET @SQLCOMMAND = @SQLCOMMAND +' '+ 'SELECT * from '+@TBLNAME2
--	EXECUTE SP_EXECUTESQL @SQLCOMMAND
--Bug-25466 End
--Bug-25466 Start
	SET @SQLCOMMAND='';
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'with costcte as('
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'SELECT '''' AS Entry_ty,'''' AS date, '''' AS doc_no, '''' AS inv_no, '''' AS tran_cd, '''' AS Ac_name, 0 AS ac_id, '''' AS inv_sr, '''' AS l_yn, '''' AS date_all,'''' AS compid, 0 AS cost_cen_id, COSTUNDER AS cost_cen_name, 0 AS amount'	
	SET @SQLCOMMAND = @SQLCOMMAND +' '+','''' AS amt_ty,COSTUNDER,COST_CAT_ID,COST_CAT_NAME FROM  '+@TBLNAME1 
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'where costunder not in(select cost_cen_name from '	+@TBLNAME1 +' group by cost_cen_name)'+' GROUP BY COSTUNDER,COST_CAT_ID,COST_CAT_NAME '
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'union all'
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'SELECT '''' AS Entry_ty,'''' AS date, '''' AS doc_no, '''' AS inv_no, '''' AS tran_cd, '''' AS Ac_name, 0 AS ac_id, '''' AS inv_sr, '''' AS l_yn, '''' AS date_all,'''' AS compid, 0 AS cost_cen_id, a.cost_cen_name, 0 AS amount'	
	SET @SQLCOMMAND = @SQLCOMMAND +' '+','''' AS amt_ty,a.COSTUNDER,a.COST_CAT_ID,c.COST_CAT_NAME FROM COST_CEN_MAST a '
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'inner join COST_CAT_MAST c on (a.cost_cat_id=c.cost_cat_id)'
	SET @SQLCOMMAND = @SQLCOMMAND +' '+'inner join costcte b on (b.cost_cen_name=a.costunder))select * from costcte'
	SET @SQLCOMMAND = @SQLCOMMAND +' '+ ' union all '
	SET @SQLCOMMAND = @SQLCOMMAND +' '+ 'SELECT * from '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
--Bug-25466 End
	SET @SQLCOMMAND = 'drop table '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
--Bug-25466 Start
	--SET @SQLCOMMAND = 'drop table '+@TBLNAME2
	--EXECUTE SP_EXECUTESQL @SQLCOMMAND
--Bug-25466 End




