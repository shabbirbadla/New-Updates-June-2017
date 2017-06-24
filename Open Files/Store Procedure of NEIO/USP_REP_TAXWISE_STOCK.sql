If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_TAXWISE_STOCK')
Begin
	Drop Procedure USP_REP_TAXWISE_STOCK
End
Go
create PROCEDURE [dbo].[USP_REP_TAXWISE_STOCK]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60) --= null
AS
DECLARE @VALMETHOD AS VARCHAR(20)
DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)

SET @VALMETHOD=CASE WHEN (@EXPARA LIKE '%FIFO%') THEN 'FIFO' ELSE (CASE WHEN (@EXPARA LIKE '%LIFO%') THEN 'LIFO' ELSE 'AVG'  END) END
select top 1 sta_dt into #TempSta_dt from vudyog..co_mast where dbname = db_name() and sta_dt <=@sdate order by sta_dt desc			--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
--Bug5445
Declare @ExclDutyforDealer Bit	
set @ExclDutyforDealer = 0	
select top 1 @ExclDutyforDealer = ExclDutyforDealer from Manufact	
--Bug5445
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=NULL
,@VEDATE=NULL--@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='M',@VITFILE='I',@VACFILE=' '
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=null
,@VFCON =@FCON OUTPUT


SELECT IT_CODE,IT_NAME=ITEM,OPBAL=QTY,OPAMT=GRO_AMT,RQTY=QTY,RAMT=GRO_AMT,IQTY=QTY,IAMT=GRO_AMT,CLBAL=QTY,CLAMT=GRO_AMT,Ware_Nm,DC_NO,Status=Ware_Nm INTO #STKVAL FROM STITEM WHERE 1=2		--Added By Shrikant S. on 03/05/2012 for Bug-3900-->Added dc_no column by sandeep for bug-1724 on 05/06/12		--Bug8025
--SELECT IT_CODE,IT_NAME=ITEM,OPBAL=QTY,OPAMT=GRO_AMT,RQTY=QTY,RAMT=GRO_AMT,IQTY=QTY,IAMT=GRO_AMT,CLBAL=QTY,CLAMT=GRO_AMT INTO #STKVAL FROM STITEM WHERE 1=2	--Commented By Shrikant S. on 03/05/2012 for Bug-3900	

----->Generate #l Table from LCODE with Behaviour
SELECT DISTINCT ENTRY_TY,(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END) AS BHENT,PMKEY=INV_STK  INTO #L FROM LCODE WHERE (V_ITEM<>0 ) AND INV_STK<>' '  ORDER BY BHENT
---<--Generate #l Table from LCODE with Behaviour
----->Tax/Discount & Charges for applied Date. 
SELECT  DISTINCT A.ENTRY_TY,A.FLD_NM,A.ATT_FILE,A_S=(CASE WHEN (A.CODE='D' OR A.CODE='F') THEN '-' ELSE '+' END),A.STKVAL,A.WEFSTKFROM,A.WEFSTKTO,TAX_NAME=SPACE(20),L.BHENT
INTO #TAX
FROM DCMAST A INNER JOIN #L L ON (A.ENTRY_TY=L.ENTRY_TY) WHERE A.STKVAL<>0
UNION
--SELECT DISTINCT ENTRY_TY=SPACE(2),FLD_NM='TAXAMT   ',ATT_FILE=1,A_S='+',STKVAL,WEFSTKFROM,WEFSTKTO,TAX_NAME,BHENT='~~'  --&& Commented by Shrikant S. on 03/04/2010 for TKT-863
SELECT DISTINCT ENTRY_TY,FLD_NM='TAXAMT   ',ATT_FILE=1,A_S='+',STKVAL,WEFSTKFROM,WEFSTKTO,TAX_NAME,BHENT=ENTRY_TY  --&& Added by Shrikant S. on 03/04/2010 for TKT-863
FROM STAX_MAS  
WHERE STKVAL<>0
---<--Tax/Discount & Charges for applied Date. 
--Bug5445
SELECT  DISTINCT A.ENTRY_TY,A.FLD_NM,A.ATT_FILE,A_S='-',A.STKVAL,A.WEFSTKFROM,A.WEFSTKTO,TAX_NAME=SPACE(20),L.BHENT
INTO #TAXEXCL
FROM DCMAST A INNER JOIN #L L ON (A.ENTRY_TY=L.ENTRY_TY) WHERE A.STKVAL=0 And A.Code = 'E' And @ExclDutyforDealer = 1
--Bug5445
----->Create Temporary Table to Calculate rate with Taxes & Charges [#STKVAL1]
--SELECT M.DATE,TRAN_CD=0,M.INV_NO,M.ENTRY_TY,I.PMKEY,I.IT_CODE,I.QTY,I.RATE,I.GRO_AMT,IT.IT_NAME,IT.RATEPER,MGRO_AMT=M.GRO_AMT,M.NET_AMT,I.WARE_NM,FRATE=I.RATE,PMV=M.NET_AMT			--Commented By Shrikant S. on 21/11/2012 for Bug-7209
SELECT M.DATE,TRAN_CD=0,M.INV_NO,M.ENTRY_TY,I.PMKEY,I.IT_CODE,I.QTY,RATE=convert(Numeric(22,6),I.RATE),I.GRO_AMT,IT.IT_NAME,IT.RATEPER,MGRO_AMT=M.GRO_AMT,M.NET_AMT,I.WARE_NM,FRATE=convert(Numeric(22,6),I.RATE),PMV=M.NET_AMT		--Added By Shrikant S. on 21/11/2012 for Bug-7209
,PMI=M.NET_AMT,TOTPMV=M.NET_AMT,M.[RULE]		--M.[RULE] Added by Vasant on 20/07/2011 for TKT-8494
,BHENT=SPACE(2),I.ITSERIAL,PMKEY1=3,DC_NO
INTO #STKVAL1
FROM STITEM I INNER JOIN 
STMAIN M ON(M.tran_cd=I.tran_cd) 
INNER JOIN IT_MAST IT ON(I.IT_CODE=IT.IT_CODE)
WHERE 1=2
---<--Create Temporary Table to Calculate rate with Taxes & Charges [#STKVAL1]
----->Insert Records into #STKVAL1 from all Item Tables
DECLARE @ENTRY_TY AS VARCHAR(2),@TRAN_CD INT,@BHENT AS VARCHAR(2),@ITSERIAL VARCHAR(10),@DATE SMALLDATETIME,@QTY NUMERIC(15,3),@AQTY NUMERIC(15,3),@AQTY1 NUMERIC(15,3),@IBALQTY1 NUMERIC(15,3),@QTY1 NUMERIC(15,3),@PMKEY VARCHAR(1)
DECLARE @ENTRY_TY1 AS VARCHAR(2),@TRAN_CD1 INT,@ITSERIAL1 VARCHAR(10),@WARE_NM1 VARCHAR(100),@DATE1 SMALLDATETIME,@IT_CODE1 INT,@DC_NO VARCHAR(10), @DC_NO1 VARCHAR(10)
--DECLARE @RATE NUMERIC(12,2),@RATE1 NUMERIC(12,2),@FRATE NUMERIC(12,2),@LRATE NUMERIC(12,2),@IT_CODE INT,@MIT_CODE INT,@IT_NAME VARCHAR(100),@WARE_NM VARCHAR(100),@MWARE_NM VARCHAR(100)			--Commented By Shrikant S. on 21/11/2012 for Bug-7209
DECLARE @RATE NUMERIC(22,6),@RATE1 NUMERIC(22,6),@FRATE NUMERIC(22,6),@LRATE NUMERIC(22,6),@IT_CODE INT,@MIT_CODE INT,@IT_NAME VARCHAR(100),@WARE_NM VARCHAR(100),@MWARE_NM VARCHAR(100)			--Added By Shrikant S. on 21/11/2012 for Bug-7209

DECLARE @SQLCOMMAND AS NVARCHAR(4000)


DECLARE  C1STKVAL CURSOR FOR 
SELECT  DISTINCT BHENT,PMKEY FROM #L
ORDER BY BHENT
OPEN C1STKVAL
FETCH NEXT FROM C1STKVAL INTO @BHENT,@PMKEY
WHILE @@FETCH_STATUS=0
BEGIN
	SET @SQLCOMMAND=' '
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INSERT INTO #STKVAL1 ('
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'DATE,TRAN_CD,INV_NO,ENTRY_TY,PMKEY,IT_CODE,QTY,RATE,GRO_AMT,IT_NAME,RATEPER,MGRO_AMT,NET_AMT,WARE_NM,FRATE,PMV'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',PMI,TOTPMV,[RULE]'				--[RULE] Added by Vasant on 20/07/2011 for TKT-8494
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',BHENT,I.ITSERIAL,PMKEY1,DC_NO'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+')'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT M.DATE,M.TRAN_CD,M.INV_NO,M.ENTRY_TY,I.PMKEY,I.IT_CODE,I.QTY,I.RATE,I.GRO_AMT,IT_MAST.IT_NAME,IT_MAST.RATEPER,MGRO_AMT=M.GRO_AMT,M.NET_AMT,I.WARE_NM,FRATE=0,PMV=0'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',PMI=0,TOTPMV=0,M.[RULE]'			--M.[RULE] Added by Vasant on 20/07/2011 for TKT-8494
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',L.BHENT,I.ITSERIAL,PMKEY1=(CASE WHEN I.PMKEY='+CHAR(39)+'+'+CHAR(39)+' THEN 1 ELSE 0 END)'--,RTRAN_CD=(CASE WHEN ITR.TRAN_CD IS NULL THEN 0 ELSE ITR.TRAN_CD END) ,RENTRY_TY=ITR.ENTRY_TY,RBHENT=SPACE(2),RQTY=ITR.RQTY,RITSERIAL=ITR.ITSERIAL'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',DC_NO FROM '+@BHENT+'ITEM I INNER JOIN '
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+@BHENT+'MAIN M ON(M.tran_cd=I.tran_cd) '
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN IT_MAST  ON(I.IT_CODE=IT_MAST.IT_CODE)'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IT_MAST.IN_STKVAL=1'
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN #L L ON (M.ENTRY_TY=L.ENTRY_TY)'

	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
	SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND I.PMKEY<>'+''' '''
	--IF @PMKEY='-'		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
	--BEGIN
		--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND M.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)	Bug 3450A
	--END
	PRINT @SQLCOMMAND
	EXEC SP_EXECUTESQL  @SQLCOMMAND
		
	FETCH NEXT FROM C1STKVAL INTO @BHENT,@PMKEY
END
CLOSE C1STKVAL
DEALLOCATE C1STKVAL

SELECT *,ASSEAMT=QTY * RATE INTO #RECEIPT FROM #STKVAL1 WHERE PMKEY1=1 --	Bug 3450A
DELETE FROM #STKVAL1 WHERE DATE>@EDATE		 --	Bug 3450A

--Added by Vasant on 20/07/2011 for TKT-8494   -- start
Update #STKVAL1 set [Rule] = 'EXCISE' where [Rule] in ('EXCISE','NON-EXCISE')
Update #STKVAL1 set [Rule] = 'OTHERS' where [Rule] NOT in ('EXCISE')
SET @SQLCOMMAND=' '
SET @SQLCOMMAND = 'DECLARE @OPTRAN_CD as INT,@OPDATE as DATETIME,@OPIT_CODE as INT,@OPRULE as VARCHAR(10) 
	DECLARE openingentry_cursor CURSOR FOR
	SELECT A.TRAN_CD, B.STA_DT,A.IT_CODE,A.[RULE] FROM #STKVAL1 A,#TempSta_dt B WHERE A.BHENT IN (''OS'') 		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPDATE,@OPIT_CODE,@OPRULE
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   DELETE FROM #STKVAL1 WHERE DATE < @OPDATE AND [RULE] = @OPRULE
		AND IT_CODE IN (SELECT IT_CODE FROM #STKVAL1 WHERE IT_CODE = @OPIT_CODE AND [RULE] = @OPRULE AND BHENT IN (''OS'')  And DATE = @OPDATE)		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
	   FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPDATE,@OPIT_CODE,@OPRULE
	END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor'
EXECUTE SP_EXECUTESQL @SQLCOMMAND
--Added by Vasant on 20/07/2011 for TKT-8494  -- end

--	   DELETE FROM #STKVAL1 WHERE BHENT IN (''OS'') AND TRAN_CD = @OPTRAN_CD AND [RULE] = @OPRULE
--			AND IT_CODE IN (SELECT IT_CODE FROM #STKVAL1 WHERE IT_CODE = @OPIT_CODE AND [RULE] = @OPRULE AND DATE < @OPDATE )

--SELECT * INTO #RECEIPT FROM #STKVAL1 WHERE PMKEY1=1 --+ Commented by Shrikant S. on 06 Apr, 2010 for TKT-863
--SELECT *,ASSEAMT=QTY * RATE INTO #RECEIPT FROM #STKVAL1 WHERE PMKEY1=1 --+ Added by Shrikant S. on 06 Apr, 2010 for TKT-863	Bug 3450A

--DELETE FROM #STKVAL1 WHERE DATE>@EDATE		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
--DELETE FROM #STKVAL1 WHERE ((PMKEY1=0 AND LEN(RTRIM(DC_NO))>0))
DELETE FROM #STKVAL1 WHERE ((PMKEY1=0 AND LEN(RTRIM(DC_NO))>2)) --&& Added by sandeep for bug-1724
--DELETE FROM #STKVAL1 WHERE (((PMKEY1=0 AND LEN(RTRIM(DC_NO))>0)) OR DATE>@EDATE)
---<--Insert Records into #STKVAL1 from all Item Tables
----->Update PMI=Total Item wise plus/minus amount from dcmast,stax_mas,TOTPMV=Total Voucher wise plus/minus amount from dcmast,stax_mas into #STKVAL1 from all Item Tables
DECLARE @TENTRY_TY AS VARCHAR(2),@FLD_NM AS VARCHAR(20),@ATT_FILE AS INT,@A_S AS VARCHAR(1),@WEFSTKFROM AS SMALLDATETIME,@WEFSTKTO AS SMALLDATETIME,@TBHENT AS VARCHAR(2),@TAX_NAME AS VARCHAR(30)
DECLARE @PARMDEFINATION NVARCHAR(50),@AMT AS NUMERIC(12,2)
UPDATE #RECEIPT SET PMI=0

SELECT ENTRY_TY,TRAN_CD=0,ITSERIAL,AMT=GRO_AMT INTO #ITEM1 FROM STITEM  WHERE 1=2

Declare @MainTable Varchar(50),@IncExciseCol Bit,@codeType Varchar(2)		--Added By Shrikant S. on 21/02/2013 for Bug-9009

set @MainTable=''									--Added By Shrikant S. on 21/02/2013 for Bug-9009
set @IncExciseCol=0									--Added By Shrikant S. on 21/02/2013 for Bug-9009

DECLARE  C2STKVAL CURSOR FOR 
SELECT  DISTINCT ENTRY_TY,BHENT FROM #RECEIPT WHERE PMKEY='+'
OPEN C2STKVAL
FETCH NEXT FROM C2STKVAL INTO @ENTRY_TY,@BHENT
WHILE @@FETCH_STATUS=0
BEGIN
	set @IncExciseCol=0									--Added By Shrikant S. on 21/02/2013 for Bug-9009
	--Added By Shrikant S. on 21/02/2013 for Bug-9009		--Start
	set @MainTable=Case when @BHENT<>'' then @BHENT else @ENTRY_TY End+'Main'		
	if Exists(Select c.[Name] From Syscolumns c Inner Join Sysobjects b on (b.id=c.id) Where b.[Name]=@MainTable and c.[name]='IncExcise')
	Begin
		set @IncExciseCol=1
	end
	--Added By Shrikant S. on 21/02/2013 for Bug-9009		--End

	DECLARE  C3STKVAL CURSOR FOR 
	SELECT FLD_NM,ATT_FILE,A_S,WEFSTKFROM,WEFSTKTO,BHENT,TAX_NAME FROM #TAX WHERE (ENTRY_TY=@ENTRY_TY) OR (BHENT='~~')
	OPEN C3STKVAL
	FETCH NEXT FROM  C3STKVAL INTO @FLD_NM,@ATT_FILE,@A_S,@WEFSTKFROM,@WEFSTKTO,@TBHENT,@TAX_NAME
	WHILE @@FETCH_STATUS=0
	BEGIN
		set @codeType=''	--Added By Shrikant S. on 21/02/2013 for Bug-9009
		IF @ATT_FILE='0'
		BEGIN
		    DELETE FROM #ITEM1
		    Select Top 1 @codeType=Code From Dcmast Where Entry_ty=@ENTRY_TY and fld_nm=@FLD_NM		--Added By Shrikant S. on 21/02/2013 for Bug-9009
			SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
			--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT ENTRY_TY,TRAN_CD,ITSERIAL,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'ITEM WHERE DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39) +' AND ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)  --Commented by Shrikant S. on 03 Apr, 2010 For TKT-863
			--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'ITEM A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY ) WHERE A.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39) +' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''+RTRIM(@FLD_NM)+''' AND B.STKVAL=1' --Added by Shrikant S. on 03 Apr, 2010 For TKT-863		----Commented By Shrikant S. on 21/02/2013 for Bug-9009		
			----Added By Shrikant S. on 21/02/2013 for Bug-9009		--Start
			SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL,(CASE WHEN A.'+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE A.'+RTRIM(@FLD_NM)+' END)  
				FROM '+@BHENT+'ITEM A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY ) 
				INNER JOIN '+@MainTable+' C ON (C.TRAN_CD=A.TRAN_CD)
				WHERE A.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39) 
				+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''
				+RTRIM(@FLD_NM)+''' AND B.STKVAL=1 '+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND C.INCEXCISE=1 ' ELSE '' END
			----Added By Shrikant S. on 21/02/2013 for Bug-9009		--End
			EXECUTE SP_EXECUTESQL @SQLCOMMAND	
			SET @SQLCOMMAND='UPDATE  A SET A.PMI=A.PMI '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)  WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
			
			EXECUTE SP_EXECUTESQL @SQLCOMMAND
		END	
		ELSE
		BEGIN
			
			--IF @TBHENT='~~'  --SALES TAX --Commented by Shrikant S. on 03 Apr, 2010 For TKT-863
			IF @TAX_NAME<>''  --SALES TAX  --Added by Shrikant S. on 03 Apr, 2010 For TKT-863
			BEGIN
				DELETE FROM #ITEM1
				
				SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
				--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT ENTRY_TY,TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN WHERE DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39) +' AND TAX_NAME='+CHAR(39) +@TAX_NAME+CHAR(39) --rup
				--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT ENTRY_TY,TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN WHERE DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39) +' AND TAX_NAME='+CHAR(39) +@TAX_NAME+CHAR(39)+' and  taxamt<>0' --Commented by Shrikant S. on 03 Apr, 2010 for TKT-863
				--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN A INNER JOIN STAX_MAS B ON (A.ENTRY_TY=B.ENTRY_TY AND RTRIM(A.TAX_NAME)=RTRIM(B.TAX_NAME)) WHERE A.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39) +' AND A.TAX_NAME='+CHAR(39) +@TAX_NAME+CHAR(39)+' and  A.taxamt<>0 AND B.STKVAL=1 ' --Added by Shrikant S. on 03 Apr, 2010 for TKT-863
				SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'ITEM A INNER JOIN STAX_MAS B ON (A.ENTRY_TY=B.ENTRY_TY AND RTRIM(A.TAX_NAME)=RTRIM(B.TAX_NAME)) WHERE A.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39) +' AND A.TAX_NAME='+CHAR(39) +@TAX_NAME+CHAR(39)+' and  A.taxamt<>0 AND B.STKVAL=1 ' --Changed by Shrikant S. on 06 Apr, 2010 for TKT-863
				EXECUTE SP_EXECUTESQL @SQLCOMMAND
				SET @SQLCOMMAND='UPDATE A SET A.PMI=A.PMI '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)  WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' ' --Added by Shrikant S. on 20/11/2012 For Bug-7312
				--SET @SQLCOMMAND='UPDATE A SET A.PMI= B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)  WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' ' --Added by Shrikant S. on 06 Apr, 2010 for TKT-863 --Commented by Shrikant S. on 20/11/2012 For Bug-7312		
				EXECUTE SP_EXECUTESQL @SQLCOMMAND --Added by Shrikant S. on 06 Apr, 2010 for TKT-863

				
			END
			ELSE
			BEGIN
				DELETE FROM #ITEM1 --Added by Shrikant S. on 06 Apr, 2010 For TKT-863
				SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
				--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT ENTRY_TY,TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN WHERE DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39) --Commented by Shrikant S. on 03 Apr, 2010 for TKT-863
				--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY) WHERE A.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''+RTRIM(@FLD_NM)+''' AND B.STKVAL=1' --Added by Shrikant S. on 03 Apr, 2010 for TKT-863		--Commented By Shrikant S. on 21/02/2013 for Bug-9009		
				----Added By Shrikant S. on 21/02/2013 for Bug-9009		--Start
				SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '
										+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY) WHERE A.DATE BETWEEN '
										+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)
										+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''+RTRIM(@FLD_NM)+''' AND B.STKVAL=1' 
										+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND A.INCEXCISE=1 ' ELSE '' END
				----Added By Shrikant S. on 21/02/2013 for Bug-9009		--End
				EXECUTE SP_EXECUTESQL @SQLCOMMAND	
				/* Added by shrikant s. on 06 apr, 2010*/ 				
				SET @SQLCOMMAND='select  a=1,A.TOTPMV,A.TOTPMV, B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
				--EXECUTE SP_EXECUTESQL @SQLCOMMAND
				SET @SQLCOMMAND='UPDATE  A SET A.TOTPMV=A.TOTPMV '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
				EXECUTE SP_EXECUTESQL @SQLCOMMAND
				/* Added by shrikant s. on 06 apr, 2010*/ 				

			END	

/* Commented by Shrikant S. on 06 Apr, 2010 for TKT-863*/		
--			EXECUTE SP_EXECUTESQL @SQLCOMMAND	
--			SET @SQLCOMMAND='select  a=1,A.TOTPMV,A.TOTPMV, B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
--			--EXECUTE SP_EXECUTESQL @SQLCOMMAND
--			SET @SQLCOMMAND='UPDATE  A SET A.TOTPMV=A.TOTPMV '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
--			EXECUTE SP_EXECUTESQL @SQLCOMMAND
/* Commented by Shrikant S. on 06 Apr, 2010 for TKT-863*/					
			

		END
		FETCH NEXT FROM  C3STKVAL INTO @FLD_NM,@ATT_FILE,@A_S,@WEFSTKFROM,@WEFSTKTO,@TBHENT,@TAX_NAME
	END
	CLOSE C3STKVAL
	DEALLOCATE C3STKVAL
	
	--Bug5445
	DECLARE  C4STKVAL CURSOR FOR 
	SELECT FLD_NM,ATT_FILE,A_S,WEFSTKFROM,WEFSTKTO,BHENT,TAX_NAME FROM #TAXEXCL WHERE (ENTRY_TY=@ENTRY_TY) OR (BHENT='~~')
	OPEN C4STKVAL
	FETCH NEXT FROM  C4STKVAL INTO @FLD_NM,@ATT_FILE,@A_S,@WEFSTKFROM,@WEFSTKTO,@TBHENT,@TAX_NAME
	WHILE @@FETCH_STATUS=0
	BEGIN
		set @codeType=''
		IF @ATT_FILE='0'
		BEGIN
		    DELETE FROM #ITEM1
		    Select Top 1 @codeType=Code From Dcmast Where Entry_ty=@ENTRY_TY and fld_nm=@FLD_NM		
			SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
			SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL,(CASE WHEN A.'+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE A.'+RTRIM(@FLD_NM)+' END)  
				FROM '+@BHENT+'ITEM A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY ) 
				INNER JOIN '+@MainTable+' C ON (C.TRAN_CD=A.TRAN_CD)
				WHERE A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''
				+RTRIM(@FLD_NM)+''' AND B.STKVAL=0 '+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND C.INCEXCISE=0 ' ELSE '' END
			EXECUTE SP_EXECUTESQL @SQLCOMMAND	
			SET @SQLCOMMAND='UPDATE  A SET A.PMI=A.PMI '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)  WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
			
			EXECUTE SP_EXECUTESQL @SQLCOMMAND
		END	
		ELSE
		BEGIN
				DELETE FROM #ITEM1
				SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
				SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '
					+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY) 
					WHERE A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''+RTRIM(@FLD_NM)+''' AND B.STKVAL=0' 
					+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND A.INCEXCISE=0 ' ELSE '' END
				EXECUTE SP_EXECUTESQL @SQLCOMMAND	
				SET @SQLCOMMAND='select  a=1,A.TOTPMV,A.TOTPMV, B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
				SET @SQLCOMMAND='UPDATE  A SET A.TOTPMV=A.TOTPMV '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
				EXECUTE SP_EXECUTESQL @SQLCOMMAND
		END
		FETCH NEXT FROM  C4STKVAL INTO @FLD_NM,@ATT_FILE,@A_S,@WEFSTKFROM,@WEFSTKTO,@TBHENT,@TAX_NAME
	END
	CLOSE C4STKVAL
	DEALLOCATE C4STKVAL
	--Bug5445
	
	FETCH NEXT FROM C2STKVAL INTO @ENTRY_TY,@BHENT
END
CLOSE C2STKVAL
DEALLOCATE C2STKVAL



---<--Update PMI=Item wise plus/minus amount from dcmast,stax_mas,TOTPMV=Total Voucher wise plus/minus amount from dcmast,stax_mas into #STKVAL1 from all Item Tables

----->Update (Item wise i.e. Sales Tax,Packing Forwarding )PMV form total Voucher wise plus/minus amount  from dcmast,stax_mas into #STKVAL1 from all Item Tables

--Added by Shrikant S. on 06 Apr, 2010 for TKT-863
UPDATE A SET ASSEAMT=B.ASSEAMT FROM #RECEIPT A INNER JOIN 
(SELECT ENTRY_TY,TRAN_CD,ASSEAMT=SUM(QTY * RATE) FROM #RECEIPT  GROUP BY ENTRY_TY,TRAN_CD) B 
ON A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD
--Added by Shrikant S. on 06 Apr, 2010 for TKT-863


--UPDATE  #RECEIPT SET PMV=(TOTPMV*GRO_AMT)/(CASE WHEN MGRO_AMT=0 THEN 1 ELSE MGRO_AMT END)  WHERE PMKEY='+' --Commented by Shrikant S. on 06 Apr, 2010 for TKT-863
UPDATE  #RECEIPT SET PMV=(TOTPMV*(QTY * RATE))/(CASE WHEN ASSEAMT=0 THEN 1 ELSE ASSEAMT END)  WHERE PMKEY='+'  --Added by shrikant s. on 06 Apr, 2010 For TKT-863
----<-Update (Item wise i.e. Sales Tax,Packing Forwarding )PMV form total Voucher wise plus/minus amount  from dcmast,stax_mas into #STKVAL1 from all Item Tables
----->Calculate FRATE=fianal rate

UPDATE  #RECEIPT SET FRATE=(((QTY*RATE)/RATEPER)+PMI+PMV)/(CASE WHEN qty=0 THEN 1 ELSE qty END)   WHERE PMKEY='+'

UPDATE  #RECEIPT SET FRATE=0 WHERE BHENT='SR'
----<-Calculate FRATE=fianal rate
----->Update FRATE TO RECEIPT WHERE FRATE=0 WITH PREV.ENTRY RATE.

--select 'c',* from #RECEIPT
SET @LRATE=0
SET @MIT_CODE=-1
SET @MWARE_NM=' '
SELECT * INTO #TRECEIPT FROM #RECEIPT
--SELECT 'a',IT_CODE,FRATE,ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM FROM #TRECEIPT ORDER BY IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN (CASE WHEN ENTRY_TY='SR' THEN 'C' ELSE 'B' end) ELSE 'D' END) END),TRAN_CD,ITSERIAL
---->Update FRATE TO RECEIPT WHERE FRATE=0 WITH PREV.ENTRY RATE.
/*DECLARE  STKVALCRSOR1 CURSOR FOR SELECT FRATE,ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM FROM #TRECEIPT ORDER BY IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN 'B' ELSE 'C'END) END),TRAN_CD,ITSERIAL Rup 29/12/2009*/
DECLARE  STKVALCRSOR1 CURSOR FOR SELECT IT_CODE,FRATE,ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,dc_no FROM #TRECEIPT ---Change by sandeep for bug-1724 on 31/12/12	   		
--ORDER BY IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN 'B' ELSE 'C'END) END),TRAN_CD,ITSERIAL /*Rup 05/04/2010 TKT-806*/
ORDER BY IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN (CASE WHEN ENTRY_TY='SR' THEN 'C' ELSE 'B' end) ELSE 'D' END) END),TRAN_CD,ITSERIAL
OPEN  STKVALCRSOR1
FETCH NEXT FROM STKVALCRSOR1 INTO @IT_CODE,@FRATE,@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@dc_no-----Change by sandeep for bug-1724 on 31/12/12	   		
WHILE (@@FETCH_STATUS=0)
BEGIN
	IF (@MIT_CODE<>@IT_CODE) OR (@MWARE_NM<>@WARE_NM)
	BEGIN
		SET @LRATE=@FRATE
		SET @MIT_CODE=@IT_CODE
		SET @MWARE_NM=@WARE_NM
	END

	IF (@FRATE)>0
	BEGIN
		SET @LRATE=@FRATE
	END
	IF (@FRATE=0 AND @LRATE>0)
	BEGIN
		UPDATE #RECEIPT SET FRATE=@LRATE WHERE (ENTRY_TY=@ENTRY_TY AND TRAN_CD=@TRAN_CD AND ITSERIAL=@ITSERIAL)
	END

	FETCH NEXT FROM STKVALCRSOR1 INTO @IT_CODE,@FRATE,@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@dc_no ---Change by sandeep for bug-1724 on 31/12/12	   		
END
CLOSE STKVALCRSOR1
DEALLOCATE STKVALCRSOR1
----<-Update FRATE TO RECEIPT WHERE FRATE=0 WITH PREV.ENTRY RATE.


UPDATE A SET A.FRATE=C.FRATE
FROM #STKVAL1  A 
LEFT JOIN #RECEIPT C ON (C.ENTRY_TY=A.ENTRY_TY AND C.TRAN_CD=A.TRAN_CD AND C.ITSERIAL=A.ITSERIAL)
--WHERE (ISNULL(C.RATE,0)<>0 AND C.RATE<>0) /*Rup 29/12/2009*/

---->Update Frate with Allocated Entry raete (AR<-PT)
SELECT A.ENTRY_TY,A.DATE,A.TRAN_CD,A.ITSERIAL,A.RENTRY_TY,A.ITREF_TRAN,A.RITSERIAL INTO #ITR1 FROM STKL_VW_ITREF A INNER JOIN LCODE B ON (A.RENTRY_TY=B.ENTRY_TY) INNER JOIN LCODE C ON (A.ENTRY_TY=C.ENTRY_TY) where B.inv_stk<>' ' AND B.INV_STK=C.INV_STK --AND A.DATE<=@EDATE
UPDATE A SET A.FRATE=C.FRATE 
FROM #STKVAL1  A 
LEFT JOIN #ITR1 B ON (A.ENTRY_TY=B.RENTRY_TY AND A.TRAN_CD=B.ITREF_TRAN AND A.ITSERIAL=B.RITSERIAL)
LEFT JOIN #RECEIPT C ON (C.ENTRY_TY=B.ENTRY_TY AND C.TRAN_CD=B.TRAN_CD AND C.ITSERIAL=B.ITSERIAL)
WHERE (ISNULL(C.RATE,0)<>0 AND C.RATE<>0)
---<-Update Frate with Allocated Entry raete (AR<-PT)

--->Delete Allcated entry i.e. PT
--DELETE FROM #STKVAL1 WHERE LEN(DC_NO)>0  --Commented by sandeep for 1724
DELETE FROM #STKVAL1 WHERE LEN(DC_NO)>2 -----Change by sandeep for bug-1724 on 31/12/12	   		
---<-Delete Allcated entry i.e. PT

DECLARE @CNT NUMERIC(18)
SET @CNT=0
--SET @VALMETHOD='FIFO'
IF (@VALMETHOD='FIFO' or @VALMETHOD='LIFO')
BEGIN 
	--->In Receipt Entry qty and rate direct enter into #STKVALFL .In Issue entry Qty is allocated prev receipt entry.If Issue is allocated againts multiple receipt then it has multiple entries with same qty and different allocated qty (aqty) field.

	DECLARE @FETCH_STATUS BIT
	SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE=FRATE,AQTY=QTY,PMKEY,CNT=0,IT_NAME,AENTRY_TY=ENTRY_TY,ATRAN_CD=TRAN_CD,AITSERIAL=ITSERIAL,AWARE_NM=WARE_NM,ADATE=DATE,DC_NO INTO #STKVALFL FROM #STKVAL1 WHERE 1=2 -----Change by sandeep for bug-1724 on 31/12/12	   		
	
	DECLARE STKVALCRSOR1 CURSOR FOR 
	--SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,FRATE,PMKEY,IT_NAME,DC_NO FROM  #STKVAL1 ORDER BY IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN 'B' ELSE 'C'END) END),TRAN_CD,ITSERIAL -----Change by sandeep for bug-1724 on 31/12/12	   			--Bug8025
	SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,FRATE,PMKEY,IT_NAME,DC_NO FROM  #STKVAL1 ORDER BY IT_CODE,WARE_NM,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN 'B' ELSE 'C'END) END),DATE,TRAN_CD,ITSERIAL	--Bug8025
	OPEN STKVALCRSOR1
	FETCH NEXT FROM STKVALCRSOR1 INTO @ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,@PMKEY,@IT_NAME,@DC_NO-----Change by sandeep for bug-1724 on 31/12/12	   		
	--                                        	
	WHILE (@@FETCH_STATUS=0)
	BEGIN
		
		SET @CNT=@CNT+1	
		IF (@PMKEY='+')
		BEGIN
			
			INSERT INTO #STKVALFL
				(ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,AQTY,PMKEY,CNT,IT_NAME,AENTRY_TY,ATRAN_CD,AITSERIAL,AWARE_NM,ADATE,DC_NO) 
			VALUES (@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,0,@PMKEY,@CNT,@IT_NAME,' ',0,' ',' ',@SDATE,@DC_NO) -----Change by sandeep for bug-1724 on 31/12/12	   		
		END			
		IF (@PMKEY='-')
		BEGIN
			
			SET @IBALQTY1=@QTY
			
			IF  @VALMETHOD='FIFO'
			BEGIN
				DECLARE STKVALCRSOR2 CURSOR FOR 
				SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,AQTY FROM #STKVALFL WHERE (WARE_NM=@WARE_NM AND IT_CODE=@IT_CODE) AND (PMKEY='+') AND ((QTY-AQTY)>0) ORDER BY IT_CODE ,WARE_NM,DATE,TRAN_CD,ITSERIAL
			END
			ELSE
			BEGIN
				DECLARE STKVALCRSOR2 CURSOR FOR 
				SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,AQTY FROM #STKVALFL WHERE (WARE_NM=@WARE_NM AND IT_CODE=@IT_CODE) AND (PMKEY='+') AND ((QTY-AQTY)>0) ORDER BY DATE desc,TRAN_CD desc ,ITSERIAL DESC --IT_CODE,WARE_NM,
			END
			OPEN STKVALCRSOR2                                                          
			FETCH NEXT FROM STKVALCRSOR2 INTO @ENTRY_TY1,@TRAN_CD1,@ITSERIAL1,@WARE_NM1,@DATE1,@IT_CODE1,@QTY1,@RATE1,@AQTY1
			IF (@@FETCH_STATUS=0)
			BEGIN
				SET @FETCH_STATUS=0
			END
			WHILE (@FETCH_STATUS=0)
			BEGIN
				SET @CNT=@CNT+1
				print '--------'
				print @QTY1
				print @AQTY1
				print @IBALQTY1
				 IF ((@QTY1-@AQTY1-@IBALQTY1)>0)
				 BEGIN
					
					--SET @AQTY=@QTY-@IBALQTY1
					SET @AQTY=@IBALQTY1
					SET @IBALQTY1=0
					--SET @AQTY=@QTY
					  	
/*Start 25/03/2010	: Changes for Receipt Rate*/
--					UPDATE #STKVALFL SET AQTY=AQTY+@QTY WHERE (ENTRY_TY=@ENTRY_TY1 AND TRAN_CD=@TRAN_CD1 AND ITSERIAL=@ITSERIAL1 AND IT_CODE=@IT_CODE)
					UPDATE #STKVALFL SET AQTY=AQTY+@AQTY WHERE (ENTRY_TY=@ENTRY_TY1 AND TRAN_CD=@TRAN_CD1 AND ITSERIAL=@ITSERIAL1 AND IT_CODE=@IT_CODE)
/*End 25/03/2010		: Changes for Receipt Rate*/

					INSERT INTO #STKVALFL
					    (ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,AQTY,PMKEY,CNT,IT_NAME,AENTRY_TY,ATRAN_CD,AITSERIAL,AWARE_NM,ADATE,DC_NO)
					VALUES (@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE1,@AQTY,@PMKEY,@CNT,@IT_NAME,@ENTRY_TY1,@TRAN_CD1,@ITSERIAL1,@WARE_NM1,@DATE1,@DC_NO1)
				 END
				 ELSE
				 BEGIN
					
					SET @IBALQTY1=@IBALQTY1-(@QTY1-@AQTY1)
					SET @AQTY=(@QTY1-@AQTY1)
					
					UPDATE #STKVALFL SET AQTY=QTY WHERE (ENTRY_TY=@ENTRY_TY1 AND TRAN_CD=@TRAN_CD1 AND ITSERIAL=@ITSERIAL1 AND IT_CODE=@IT_CODE)
					INSERT INTO #STKVALFL
					    (ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,AQTY,PMKEY,CNT,IT_NAME,AENTRY_TY,ATRAN_CD,AITSERIAL,AWARE_NM,ADATE,DC_NO)
					VALUES (@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE1,@AQTY,@PMKEY,@CNT,@IT_NAME,@ENTRY_TY1,@TRAN_CD1,@ITSERIAL1,@WARE_NM1,@DATE1,@DC_NO1)
				 END
				
				
				FETCH NEXT FROM STKVALCRSOR2 INTO @ENTRY_TY1,@TRAN_CD1,@ITSERIAL1,@WARE_NM1,@DATE1,@IT_CODE1,@QTY1,@RATE1,@AQTY1
				IF (@IBALQTY1=0 OR @@FETCH_STATUS<>0)
				BEGIN
					SET @FETCH_STATUS=-1	
				END
			END	
			CLOSE STKVALCRSOR2
			DEALLOCATE STKVALCRSOR2
			
			
			IF @IBALQTY1>0
			BEGIN
				SET @AQTY=@IBALQTY1
				 INSERT INTO #STKVALFL
					    (ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,AQTY,PMKEY,CNT,IT_NAME,AENTRY_TY,ATRAN_CD,AITSERIAL,AWARE_NM,ADATE,DC_NO)
				 VALUES (@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,@AQTY,@PMKEY,@CNT,@IT_NAME,' ',0,' ',' ',@SDATE,@DC_NO)				
			END
		END
		FETCH NEXT FROM STKVALCRSOR1 INTO @ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,@PMKEY,@IT_NAME,@DC_NO				
	END
	CLOSE STKVALCRSOR1
	DEALLOCATE STKVALCRSOR1

	--Bug8025 
	Select Colno=IDENTITY(int, 1, 1),It_Code,Ware_nm,Rate Into #tmp1STKVALFL From #STKVALFL
		where PmKey = '-' and ATran_cd != 0 Order By It_Code,Ware_nm,Date
	
	Select Max(Colno) As ColNo,It_Code,Ware_nm Into #tmp2STKVALFL From #tmp1STKVALFL
		Group By It_Code,Ware_nm
	
	Update #STKVALFL Set Rate = b.Rate from #STKVALFL a,#tmp1STKVALFL b,#tmp2STKVALFL c
		Where a.It_code = b.It_code and a.Ware_nm = b.Ware_Nm 
		and b.colno = c.colno
		and a.PmKey = '-' and ATran_cd = 0
		
	Drop Table #tmp1STKVALFL
	Drop Table #tmp2STKVALFL
	--Bug8025
	INSERT INTO #STKVAL 
	(IT_CODE,IT_NAME,OPBAL,OPAMT,RQTY,RAMT,IQTY,IAMT,CLBAL,CLAMT,WARE_NM)	--Added By Shrikant S. on 03/05/2012 for Bug-3900
	--(IT_CODE,IT_NAME,OPBAL,OPAMT,RQTY,RAMT,IQTY,IAMT,CLBAL,CLAMT)			--Commented By Shrikant S. on 03/05/2012 for Bug-3900
--	SELECT 
--	IT_CODE,IT_NAME,
--	OPBAL=SUM(CASE WHEN ENTRY_TY='OS' OR DATE<@SDATE THEN (CASE WHEN PMKEY='+' THEN QTY ELSE -AQTY  END) ELSE 0  END)	    		
--	,OPAMT=SUM(CASE WHEN ENTRY_TY='OS' OR DATE<@SDATE THEN (CASE WHEN PMKEY='+' THEN QTY*RATE ELSE -(AQTY*RATE)  END) ELSE 0  END)
--	,RQTY=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='+') THEN QTY ELSE 0 END)
--	,RAMT=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='+') THEN QTY*RATE ELSE 0 END)
--	,IQTY=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='-') THEN AQTY ELSE 0 END)
--	,IAMT=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='-') THEN AQTY*RATE ELSE 0 END)
--	,CLBAL=SUM((CASE WHEN PMKEY='+' THEN QTY ELSE -AQTY  END))
--	,CLAMT=SUM(CASE WHEN PMKEY='+' THEN QTY*RATE ELSE -(AQTY*RATE)  END)
--	,WARE_NM															--Added By Shrikant S. on 03/05/2012 for Bug-3900
--	FROM #STKVALFL 
--	GROUP BY IT_CODE,IT_NAME,WARE_NM 
	SELECT 
   IT_CODE,IT_NAME,
	OPBAL=SUM(CASE WHEN ENTRY_TY='OS' OR DATE<@SDATE THEN (CASE WHEN PMKEY='+' THEN (CASE WHEN DC_NO='DI' THEN 0 ELSE QTY END ) ELSE -AQTY  END) ELSE 0  END)--chnages by sandeep for bug-1724
	,OPAMT=SUM(CASE WHEN ENTRY_TY='OS' OR DATE<@SDATE THEN (CASE WHEN PMKEY='+' THEN QTY*RATE ELSE -(AQTY*RATE)  END) ELSE 0  END)
	,RQTY=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='+') THEN (CASE WHEN DC_NO='DI' THEN 0 ELSE QTY END ) ELSE 0 END) --chnages by sandeep for bug-1724
	,RAMT=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='+') THEN QTY*RATE ELSE 0 END)
	,IQTY=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='-') THEN AQTY ELSE 0 END)
	,IAMT=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='-') THEN AQTY*RATE ELSE 0 END)
	,CLBAL=SUM((CASE WHEN PMKEY='+' THEN (CASE WHEN DC_NO='DI' THEN 0 ELSE QTY END ) ELSE -AQTY  END)) --chnages by sandeep for bug-1724
	,CLAMT=SUM(CASE WHEN PMKEY='+' THEN QTY*RATE ELSE -(AQTY*RATE)  END)
	,WARE_NM		
	FROM #STKVALFL 
	GROUP BY IT_CODE,IT_NAME,WARE_NM
	--Bug8025
	Update #STKVAL Set Status = ''
	Update #STKVAL Set Status = ltrim(rtrim(Status))+'A' Where cast(it_code as varchar(10))+ware_nm in
		(Select cast(it_code as varchar(10))+ware_nm from #STKVALFL where pmkey = '-' and ADate > Date)
	Update #STKVAL Set Status = ltrim(rtrim(Status))+'B' Where cast(it_code as varchar(10))+ware_nm in
		(Select cast(it_code as varchar(10))+ware_nm from #STKVALFL where pmkey = '-' and ATran_cd = 0)
	--Bug8025

END
select ENTRY_TY,TRAN_CD,ITSERIAL,IT_CODE,(QTY-AQTY) AS QTY1,RATE,(QTY-AQTY)*RATE AS AMOUNT  INTO #TEMP1 from #STKVALFL WHERE PMKEY='+' AND (QTY-AQTY)>0

--SELECT A.*,B.TAX_NAME,C.LEVEL1  FROM #TEMP1 A
--INNER JOIN LITEM_VW B ON(A.IT_CODE=B.IT_CODE AND A.ENTRY_TY=B.ENTRY_TY AND A.ITSERIAL=B.ITSERIAL) 
--INNER JOIN STAX_MAS C ON(B.TAX_NAME=C.TAX_NAME AND B.ENTRY_TY=C.ENTRY_TY)

SELECT A.*,B.TAX_NAME,C.LEVEL1  FROM #TEMP1 A
INNER JOIN LITEM_VW B ON(A.IT_CODE=B.IT_CODE AND A.ENTRY_TY=B.ENTRY_TY AND A.ITSERIAL=B.ITSERIAL AND B.TAX_NAME<>'') 
LEFT OUTER JOIN STAX_MAS C ON(B.TAX_NAME=C.TAX_NAME AND B.ENTRY_TY=C.ENTRY_TY)

