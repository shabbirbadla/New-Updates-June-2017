ALTER PROCEDURE [dbo].[USP_REP_STKVAL]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(4000) 
,@TBLNM AS VARCHAR(50)=''
AS

Print 'Start '+convert(varchar(50),getdate(),113)
SET NOCOUNT ON
if Object_id('tempdb..##STKVAL','U') is not null
	DROP TABLE ##STKVAL

if Object_id('tempdb..##STKVAL1','U') is not null
	DROP TABLE ##STKVAL1

if Object_id('tempdb..##STKVALConfig','U') is not null	
	Drop Table ##STKVALConfig

DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4),@Sta_Dt SmallDateTime

select top 1 @Sta_Dt=Isnull(sta_dt,@SDATE) from vudyog..co_mast where dbname = db_name() and sta_dt <=@sdate order by sta_dt desc			

Declare @ExclDutyforDealer Bit	
set @ExclDutyforDealer = 0	
select top 1 @ExclDutyforDealer = ExclDutyforDealer from Manufact	

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=NULL
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=NULL,@VEIT=NULL
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


Declare @SQLCOMMAND AS NVARCHAR(4000),@ShowNil Varchar(100),@FldNmLst Varchar(4000),@FldValLst Varchar(4000),@CondCount Int
Set @ShowNil = case when @EXPARA like '%YES%' then 'YES' else 'NO' end

Select RecId = IDENTITY(int, 1,1),VStkCond = Cast(' ' as varchar(4000)),* 
	into ##STKVALConfig from StkValConfig

SELECT RecId=Cast(0 as int),
	RawId=Cast(0 as int),
	condition=Cast(' ' as nvarchar(max)),
	descrip=Cast(' ' as nvarchar(max)),
	tbl_name=Cast(' ' as nvarchar(max))
	Into #StkValConfig1 FROM ##StkValConfig where 1 = 2



SET ARITHABORT On
Set @CondCount = 1
WHILE 1 = 1
BEGIN
	Set @SQLCOMMAND	= 'Insert into #StkValConfig1
		SELECT RecId,'+cast(@CondCount as varchar(5))+',
		vcond.value(''(//tmpvcond/condition)['+cast(@CondCount as varchar(5))+']'', ''nvarchar(max)'') as condition,
		vcond.value(''(//tmpvcond/descrip)['+cast(@CondCount as varchar(5))+']'', ''nvarchar(max)'') as descrip,
		vcond.value(''(//tmpvcond/tbl_name)['+cast(@CondCount as varchar(5))+']'', ''nvarchar(max)'') as tbl_name
		FROM ##StkValConfig'
	EXEC SP_EXECUTESQL  @SQLCOMMAND	
		
	IF Not Exists(Select * from #StkValConfig1 where RawId = @CondCount And condition Is Not Null)
		BREAK
	ELSE
		Set @CondCount = @CondCount + 1
		CONTINUE
END

Update #StkValConfig1 Set condition = ISNULL(condition,''),descrip = ISNULL(descrip,''),tbl_name = ISNULL(tbl_name,'')
Delete from #StkValConfig1 Where condition = '' and descrip = '' and tbl_name = ''
Update #StkValConfig1 Set tbl_name = (case when tbl_name = 'STKL_VW_MAIN' then 'M' else (case when tbl_name = 'STKL_VW_ITEM' then 'I' else tbl_name end) end)

Set @FldNmLst = '[RULE],[INV_NO],[ENTRY_TY],[PMKEY],[IT_NAME],[WARE_NM],[ITSERIAL],[DC_NO],[APGEN],[LDEACTIVE],[IN_STKVAL],[RATEUNIT]'
Set @FldValLst = 'M.[RULE],M.[INV_NO],M.[ENTRY_TY],I.[PMKEY],IT_MAST.[IT_NAME],I.[WARE_NM],I.[ITSERIAL],[DC_NO],M.[APGEN],IT_MAST.[LDEACTIVE],IT_MAST.[IN_STKVAL],IT_MAST.[RATEUNIT]'

Declare @recid int,@condition Varchar(100),@descrip Varchar(4000),@tbl_name Varchar(100),@stkvcond Varchar(4000)
DECLARE C0STKVAL CURSOR FOR 
SELECT RecId FROM #StkValConfig1 Group BY RecId
OPEN C0STKVAL
FETCH NEXT FROM C0STKVAL INTO @recid
WHILE @@FETCH_STATUS=0
	BEGIN
	Set @stkvcond = ''

	DECLARE C01STKVAL CURSOR FOR 
	SELECT Condition,Descrip,Tbl_Name FROM #StkValConfig1 Where RecId = @recid
	ORDER BY RecId,Tbl_Name,Condition
	OPEN C01STKVAL
	FETCH NEXT FROM C01STKVAL INTO @Condition,@Descrip,@Tbl_Name
	WHILE @@FETCH_STATUS=0
	BEGIN
			
		IF replace(replace(@FldValLst,'[','+'),']','+') not like '%'+'+'+LTRIM(RTRIM(@Condition))+'+'+'%'
		Begin
			Set @FldNmLst = @FldNmLst + ','+'['+LTRIM(RTRIM(@Condition))+']'
			Set @FldValLst = @FldValLst + ','+LTRIM(RTRIM(@Tbl_Name))+'.['+LTRIM(RTRIM(@Condition))+']'
		End

		if replace(replace(@stkvcond,'[','+'),']','+') like '%'+'+'+LTRIM(RTRIM(@Condition))+'+'+'%'
		Begin
			Set @stkvcond = @stkvcond + ' Or '
		End	
		if replace(replace(@stkvcond,'[','+'),']','+') not like '%'+'+'+LTRIM(RTRIM(@Condition))+'+'+'%'
		Begin
			if @stkvcond <> ''
			Begin
				Set @stkvcond = @stkvcond + ' ) And '
			End	
			Set @stkvcond = @stkvcond + ' ( '
		End	
		Set @stkvcond = @stkvcond + '['+LTRIM(RTRIM(@Condition))+']' 
		Set @stkvcond = @stkvcond + ' = '''+@Descrip+''''
		
		FETCH NEXT FROM C01STKVAL INTO @Condition,@Descrip,@Tbl_Name
	END
	CLOSE C01STKVAL
	DEALLOCATE C01STKVAL

	if @stkvcond <> ''
	Begin
		Set @stkvcond = @stkvcond + ' )'
	End	
	Set @stkvcond = REPLACE(@stkvcond,'''','''''')
	SET @SQLCOMMAND=' '
	SET @SQLCOMMAND=' Update ##STKVALConfig Set VStkCond = '''+@stkvcond+''' Where RecId = '+cast(@RecId as varchar(5))
--	print @SQLCOMMAND
	EXEC SP_EXECUTESQL  @SQLCOMMAND

	FETCH NEXT FROM C0STKVAL INTO @recid
END
CLOSE C0STKVAL
DEALLOCATE C0STKVAL

Drop Table #StkValConfig1



SET @SQLCOMMAND=' '
SET @SQLCOMMAND='SELECT IT_CODE,IT_NAME=ITEM,OPBAL=QTY,OPAMT=GRO_AMT,RQTY=QTY,RAMT=GRO_AMT,IQTY=QTY,IAMT=GRO_AMT,CLBAL=QTY,CLAMT=GRO_AMT,Ware_Nm,DC_NO,Status=Ware_Nm,VNAME=SPACE(100) INTO ##STKVAL FROM STITEM WHERE 1=2'
EXEC SP_EXECUTESQL  @SQLCOMMAND
	
SET @SQLCOMMAND=' '
SET @SQLCOMMAND='SELECT M.DATE,TRAN_CD=0,I.IT_CODE,I.QTY,RATE=convert(Numeric(22,6),I.RATE),I.GRO_AMT,IT_MAST.RATEPER,MGRO_AMT=M.GRO_AMT,M.NET_AMT,FRATE=convert(Numeric(22,6),I.RATE),PMV=M.NET_AMT'
SET @SQLCOMMAND=@SQLCOMMAND + ',PMI=M.NET_AMT,TOTPMV=M.NET_AMT,BHENT=SPACE(2),PMKEY1=3'
SET @SQLCOMMAND=@SQLCOMMAND + ','+@FldValLst+',VName=space(100),VType=space(20)'
SET @SQLCOMMAND=@SQLCOMMAND + ' INTO ##STKVAL1 FROM STITEM I INNER JOIN STMAIN M ON(M.tran_cd=I.tran_cd)'
SET @SQLCOMMAND=@SQLCOMMAND + ' INNER JOIN IT_MAST ON(I.IT_CODE=IT_MAST.IT_CODE) WHERE 1=2'
EXEC SP_EXECUTESQL  @SQLCOMMAND


/******************* Creating Indexes			Start*/
SELECT Date as TranDate,Tran_cd,It_code,Entry_ty,Qty,Rate,Pmkey,Vname,Vtype,It_name,Itserial,ItemRowId=CONVERT(Numeric(10),0),RATEUNIT
	,OpRunningTotal=Qty,RunningTotal=Qty,UniqueId=CONVERT(Numeric(18),0) 
	,OpQty =Qty ,OpAmt=Qty * Rate,Clqty=Qty , ClAmt=Qty * Rate
	Into #stk FROM ##STKVAL1 where 1=2

CREATE NONCLUSTERED INDEX [IX_Descending]
    ON #stk
     (
     [VName] Asc,
      [It_code] ASC,
      [ItemRowId] DESC,
      Pmkey Asc
     )   
INCLUDE ( [Qty], [Rate])
    WITH (
      PAD_INDEX  = OFF,
      STATISTICS_NORECOMPUTE  = OFF,
      SORT_IN_TEMPDB = OFF,
      IGNORE_DUP_KEY = OFF,
      DROP_EXISTING = OFF,
      ONLINE = OFF, ALLOW_ROW_LOCKS  = ON,
      ALLOW_PAGE_LOCKS  = ON)
    ON [PRIMARY]   ;

CREATE NONCLUSTERED INDEX [IX_Ascending]
    ON #stk
     (
     [VName] Asc,
      [It_code] ASC,
      [ItemRowId] Asc,
      Pmkey Asc
     )   
INCLUDE ( [Qty], [Rate])
    WITH (
      PAD_INDEX  = OFF,
      STATISTICS_NORECOMPUTE  = OFF,
      SORT_IN_TEMPDB = OFF,
      IGNORE_DUP_KEY = OFF,
      DROP_EXISTING = OFF,
      ONLINE = OFF, ALLOW_ROW_LOCKS  = ON,
      ALLOW_PAGE_LOCKS  = ON)
    ON [PRIMARY]   ;
 
 
CREATE NONCLUSTERED INDEX [IX_Qty]
    ON #stk
     (   
	     [VName] Asc,
		It_code ASC,
		ItemRowId Asc
      )
    INCLUDE (Qty)   
    WHERE (Pmkey ='+')  
    WITH (  PAD_INDEX  = OFF,  STATISTICS_NORECOMPUTE  = OFF,  SORT_IN_TEMPDB = OFF,  IGNORE_DUP_KEY = OFF,  DROP_EXISTING = OFF,  ONLINE = OFF,  ALLOW_ROW_LOCKS  = ON,  ALLOW_PAGE_LOCKS  = ON )
	ON [PRIMARY]  ; 

  
CREATE NONCLUSTERED INDEX [IX_Rate]
   ON #stk
    (   
	     [VName] Asc,
		It_code ASC,
		ItemRowId ASC
     )   
    INCLUDE ( Rate)   
    WHERE (Pmkey='+')   
    WITH (   PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF,SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON,  ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 100)
    ON [PRIMARY]  ; 

/******************* Creating Indexes			End*/



----->Generate #l Table from LCODE with Behaviour
SELECT DISTINCT ENTRY_TY,(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END) AS BHENT,PMKEY=INV_STK  INTO #L FROM LCODE WHERE (V_ITEM<>0 ) AND INV_STK<>' '  ORDER BY BHENT
---<--Generate #l Table from LCODE with Behaviour

----->Tax/Discount & Charges for applied Date. 
SELECT  DISTINCT A.ENTRY_TY,A.FLD_NM,A.ATT_FILE,A_S=(CASE WHEN (A.CODE='D' OR A.CODE='F') THEN '-' ELSE '+' END),A.STKVAL,A.WEFSTKFROM,A.WEFSTKTO,TAX_NAME=SPACE(20),L.BHENT
INTO #TAX
FROM DCMAST A INNER JOIN #L L ON (A.ENTRY_TY=L.ENTRY_TY) WHERE A.STKVAL<>0
UNION

SELECT DISTINCT ENTRY_TY,FLD_NM='TAXAMT   ',ATT_FILE=1,A_S='+',STKVAL,WEFSTKFROM,WEFSTKTO,TAX_NAME,BHENT=ENTRY_TY  
FROM STAX_MAS  
WHERE STKVAL<>0
---<--Tax/Discount & Charges for applied Date. 

SELECT  DISTINCT A.ENTRY_TY,A.FLD_NM,A.ATT_FILE,A_S='-',A.STKVAL,A.WEFSTKFROM,A.WEFSTKTO,TAX_NAME=SPACE(20),L.BHENT
INTO #TAXEXCL
FROM DCMAST A INNER JOIN #L L ON (A.ENTRY_TY=L.ENTRY_TY) WHERE A.STKVAL=0 And A.Code = 'E' And @ExclDutyforDealer = 1

----->Insert Records into ##STKVAL1 from all Item Tables
DECLARE @ENTRY_TY AS VARCHAR(2),@TRAN_CD INT,@BHENT AS VARCHAR(2),@ITSERIAL VARCHAR(10),@DATE SMALLDATETIME,@QTY NUMERIC(15,3),@AQTY NUMERIC(15,3),@AQTY1 NUMERIC(15,3),@IBALQTY1 NUMERIC(15,3),@QTY1 NUMERIC(15,3),@PMKEY VARCHAR(1)
DECLARE @ENTRY_TY1 AS VARCHAR(2),@TRAN_CD1 INT,@ITSERIAL1 VARCHAR(10),@WARE_NM1 VARCHAR(100),@DATE1 SMALLDATETIME,@IT_CODE1 INT,@DC_NO VARCHAR(10), @DC_NO1 VARCHAR(10),@VNAME1 VARCHAR(100)	
DECLARE @RATE NUMERIC(22,6),@RATE1 NUMERIC(22,6),@FRATE NUMERIC(22,6),@LRATE NUMERIC(22,6),@IT_CODE INT,@MIT_CODE INT,@IT_NAME VARCHAR(100),@WARE_NM VARCHAR(100),@MWARE_NM VARCHAR(100),@MVNAME VARCHAR(100)



print @FCON

SET @FCON = REPLACE(@FCON ,'INNER JOIN ',' CROSS APPLY (SELECT TOP 1 IT_NAME FROM ')
SET @FCON = REPLACE(@FCON ,'ON (',' WHERE ')

DECLARE  C1STKVAL CURSOR FOR 
SELECT  DISTINCT BHENT,PMKEY FROM #L
ORDER BY BHENT
OPEN C1STKVAL
FETCH NEXT FROM C1STKVAL INTO @BHENT,@PMKEY
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND=' '
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'INSERT INTO ##STKVAL1 ('
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'DATE,TRAN_CD,IT_CODE,QTY,RATE,GRO_AMT,RATEPER,MGRO_AMT,NET_AMT,FRATE,PMV,PMI,TOTPMV'
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+',BHENT,PMKEY1,'+@FldNmLst
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+')'
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'SELECT M.DATE,M.TRAN_CD,I.IT_CODE,I.QTY,I.RATE,I.GRO_AMT,IT_MAST.RATEPER,MGRO_AMT=M.GRO_AMT,M.NET_AMT,FRATE=0,PMV=0,PMI=0,TOTPMV=0'
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+',L.BHENT,PMKEY1=(CASE WHEN I.PMKEY='+CHAR(39)+'+'+CHAR(39)+' THEN 1 ELSE 0 END),'+@FldValLst	
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'FROM '+@BHENT+'MAIN  M '
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'cross Apply ('
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'Select I.IT_CODE,I.QTY,I.RATE,I.GRO_AMT,I.PMKEY,I.[ITSERIAL],I.[DC_NO],I.ENTRY_TY,I.WARE_NM FROM '+@BHENT+'ITEM I '
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+' Where I.Tran_cd=M.Tran_cd AND I.ITEM BETWEEN @SITEM AND @EITEM) I'
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'cross Apply (Select Top 1 IT_NAME,RATEPER,LDEACTIVE,IN_STKVAL,RATEUNIT,[type] from IT_MAST Where IT_MAST.It_code=I.It_code) IT_MAST'
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+'cross Apply (Select Top 1 BHENT from #L L Where L.Entry_ty=I.Entry_ty) L '
	SET @SQLCOMMAND=(@SQLCOMMAND)+' '+RTRIM(@FCON)
	
--	PRINT @SQLCOMMAND
	EXEC SP_EXECUTESQL  @SQLCOMMAND,N'@SITEM VARCHAR(50),@EITEM VARCHAR(50)',@SIT,@EIT
		
	FETCH NEXT FROM C1STKVAL INTO @BHENT,@PMKEY
END
CLOSE C1STKVAL
DEALLOCATE C1STKVAL

DELETE FROM ##STKVAL1 WHERE [RULE]='ANNEXURE V' OR LDEACTIVE=1 OR APGEN<>'YES' OR PMKEY=' ' OR LEN(DC_NO)>2
DELETE FROM ##STKVAL1 WHERE IN_STKVAL<>1 --Added By Kishor A. for bug-28745 on 13-09-2016

Update ##STKVAL1 Set VName = ISNULL(VName,''),VType = ISNULL(VType,'')
Declare @VName Varchar(100),@VType Varchar(20),@VCond Varchar(4000)
DECLARE C4STKVAL CURSOR FOR 
SELECT  VName,VType,VStkCond FROM ##STKVALConfig
ORDER BY VName
OPEN C4STKVAL
FETCH NEXT FROM C4STKVAL INTO @VName,@VType,@VCond
WHILE @@FETCH_STATUS=0
BEGIN
	If @VCond = ''
	begin
		set @VCond = 'IsNull(VName,'' '') = '' '''
	end
	SET @SQLCOMMAND=' '
	SET @SQLCOMMAND=' Update ##STKVAL1 Set VName = '''+@VName+''',VType = '''+@VType+''' Where '+@VCond
	EXEC SP_EXECUTESQL  @SQLCOMMAND
		
	FETCH NEXT FROM C4STKVAL INTO @VName,@VType,@VCond
END
CLOSE C4STKVAL
DEALLOCATE C4STKVAL


SELECT *,ASSEAMT=QTY * RATE INTO #RECEIPT FROM ##STKVAL1 WHERE PMKEY1=1 --	Bug 3450A


Update ##STKVAL1 set [Rule] = 'EXCISE' where [Rule] in ('EXCISE','NON-EXCISE')
Update ##STKVAL1 set [Rule] = 'OTHERS' where [Rule] NOT in ('EXCISE')
SET @SQLCOMMAND=' '


SET @SQLCOMMAND = 'DECLARE @OPDATE as DATETIME,@OPIT_CODE as INT,@OPRULE as VARCHAR(10),@OPVNAME as VARCHAR(100)		
	DECLARE openingentry_cursor CURSOR FOR
	SELECT A.DATE,A.IT_CODE,A.[RULE],A.VNAME FROM ##STKVAL1 A WHERE A.BHENT  =''OS'' And DATE = '''+CONVERT(varchar(50),@Sta_Dt)+''' GROUP BY A.DATE,A.IT_CODE,A.[RULE],A.VNAME	
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @OPDATE,@OPIT_CODE,@OPRULE,@OPVNAME		
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   DELETE FROM ##STKVAL1 WHERE DATE < @OPDATE AND [RULE] = @OPRULE AND VNAME	= @OPVNAME AND IT_CODE = @OPIT_CODE
	   FETCH NEXT FROM openingentry_cursor into @OPDATE,@OPIT_CODE,@OPRULE,@OPVNAME		
	END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

---<--Insert Records into ##STKVAL1 from all Item Tables


----->Update PMI=Total Item wise plus/minus amount from dcmast,stax_mas,TOTPMV=Total Voucher wise plus/minus amount from dcmast,stax_mas into ##STKVAL1 from all Item Tables
DECLARE @TENTRY_TY AS VARCHAR(2),@FLD_NM AS VARCHAR(20),@ATT_FILE AS INT,@A_S AS VARCHAR(1),@WEFSTKFROM AS SMALLDATETIME,@WEFSTKTO AS SMALLDATETIME,@TBHENT AS VARCHAR(2),@TAX_NAME AS VARCHAR(30)
DECLARE @PARMDEFINATION NVARCHAR(50),@AMT AS NUMERIC(12,2)
UPDATE #RECEIPT SET PMI=0

SELECT ENTRY_TY,TRAN_CD=0,ITSERIAL,AMT=GRO_AMT INTO #ITEM1 FROM STITEM  WHERE 1=2

Declare @MainTable Varchar(50),@IncExciseCol Bit,@codeType Varchar(2)		

set @MainTable=''									
set @IncExciseCol=0									




DECLARE  C2STKVAL CURSOR FOR 
SELECT  DISTINCT ENTRY_TY,BHENT FROM #RECEIPT WHERE PMKEY='+'
OPEN C2STKVAL
FETCH NEXT FROM C2STKVAL INTO @ENTRY_TY,@BHENT
WHILE @@FETCH_STATUS=0
BEGIN
	set @MainTable=Case when @BHENT<>'' then @BHENT else @ENTRY_TY End+'Main'		
	if Exists(Select c.[Name] From Syscolumns c Inner Join Sysobjects b on (b.id=c.id) Where b.[Name]=@MainTable and c.[name]='IncExcise')
	Begin
		set @IncExciseCol=1
	end

	DECLARE  C3STKVAL CURSOR FOR 
	SELECT FLD_NM,ATT_FILE,A_S,WEFSTKFROM,WEFSTKTO,BHENT,TAX_NAME FROM #TAX WHERE (ENTRY_TY=@ENTRY_TY) OR (BHENT='~~')
	OPEN C3STKVAL
	FETCH NEXT FROM  C3STKVAL INTO @FLD_NM,@ATT_FILE,@A_S,@WEFSTKFROM,@WEFSTKTO,@TBHENT,@TAX_NAME
	WHILE @@FETCH_STATUS=0
	BEGIN
		set @codeType=''	
		IF @ATT_FILE='0'
		BEGIN
		    DELETE FROM #ITEM1
		    
		    Select Top 1 @codeType=Code From Dcmast Where Entry_ty=@ENTRY_TY and fld_nm=@FLD_NM		
			SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
			SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL,(CASE WHEN A.'+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE A.'+RTRIM(@FLD_NM)+' END)  
				FROM '+@MainTable+' C  
				CROSS APPLY (SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL, A.'+RTRIM(@FLD_NM)+' FROM '+@BHENT+'ITEM A WHERE  A.TRAN_CD=C.TRAN_CD) A
				CROSS APPLY (SELECT TOP 1 ENTRY_TY,FLD_NM FROM DCMAST WHERE ENTRY_TY=C.ENTRY_TY AND STKVAL=1 AND FLD_NM='''+RTRIM(@FLD_NM)+''') B
				
				WHERE C.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39) 
				+' AND C.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND C.INCEXCISE=1 ' ELSE '' END
				
			EXECUTE SP_EXECUTESQL @SQLCOMMAND	
			
			
			SET @SQLCOMMAND='UPDATE  A SET A.PMI=A.PMI '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)  WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
			
			EXECUTE SP_EXECUTESQL @SQLCOMMAND
		END	
		ELSE
		BEGIN
			IF @TAX_NAME<>''  
			BEGIN
				DELETE FROM #ITEM1
				
				SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
				SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'ITEM A INNER JOIN STAX_MAS B ON (A.ENTRY_TY=B.ENTRY_TY AND RTRIM(A.TAX_NAME)=RTRIM(B.TAX_NAME)) WHERE A.DATE BETWEEN '+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39) +' AND A.TAX_NAME='+CHAR(39) +@TAX_NAME+CHAR(39)+' and  A.taxamt<>0 AND B.STKVAL=1 ' 
				EXECUTE SP_EXECUTESQL @SQLCOMMAND
				
				SET @SQLCOMMAND='UPDATE A SET A.PMI=A.PMI '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)  WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' ' --Added by Shrikant S. on 20/11/2012 For Bug-7312
				EXECUTE SP_EXECUTESQL @SQLCOMMAND 

				
			END
			ELSE
			BEGIN
				DELETE FROM #ITEM1 
				SET @SQLCOMMAND='INSERT INTO #ITEM1  (ENTRY_TY,TRAN_CD,ITSERIAL,AMT)'
				SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'SELECT A.ENTRY_TY,A.TRAN_CD,ITSERIAL=0,(CASE WHEN '+RTRIM(@FLD_NM)+' IS NULL THEN 0 ELSE '
										+RTRIM(@FLD_NM)+' END)  FROM '+@BHENT+'MAIN A INNER JOIN DCMAST B ON (A.ENTRY_TY=B.ENTRY_TY) WHERE A.DATE BETWEEN '
										+CHAR(39)+CAST(@WEFSTKFROM AS VARCHAR)+CHAR(39)+ ' AND '+CHAR(39)+CAST(@WEFSTKTO AS VARCHAR)+CHAR(39)
										+' AND A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+' AND B.FLD_NM='''+RTRIM(@FLD_NM)+''' AND B.STKVAL=1' 
										+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND A.INCEXCISE=1 ' ELSE '' END

				EXECUTE SP_EXECUTESQL @SQLCOMMAND	
				SET @SQLCOMMAND='UPDATE  A SET A.TOTPMV=A.TOTPMV '+@A_S+' B.AMT FROM #ITEM1 B INNER JOIN #RECEIPT A ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD ) WHERE  A.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)
				EXECUTE SP_EXECUTESQL @SQLCOMMAND

			END	
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
			
				FROM '+@MainTable+' C  
				CROSS APPLY (SELECT A.ENTRY_TY,A.TRAN_CD,A.ITSERIAL, A.'+RTRIM(@FLD_NM)+' FROM '+@BHENT+'ITEM A WHERE  A.TRAN_CD=C.TRAN_CD) A
				CROSS APPLY (SELECT TOP 1 ENTRY_TY,FLD_NM FROM DCMAST WHERE ENTRY_TY=C.ENTRY_TY AND STKVAL=0 AND FLD_NM='''+RTRIM(@FLD_NM)+''') B
				WHERE C.ENTRY_TY='+CHAR(39)+@ENTRY_TY+CHAR(39)+CASE WHEN @IncExciseCol=1 and @codeType='E' THEN +' AND C.INCEXCISE=0 ' ELSE '' END
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

---<--Update PMI=Item wise plus/minus amount from dcmast,stax_mas,TOTPMV=Total Voucher wise plus/minus amount from dcmast,stax_mas into ##STKVAL1 from all Item Tables



----->Update (Item wise i.e. Sales Tax,Packing Forwarding )PMV form total Voucher wise plus/minus amount  from dcmast,stax_mas into ##STKVAL1 from all Item Tables

UPDATE A SET ASSEAMT=(select SUM(b.QTY * b.RATE) from #RECEIPT B where B.TRAN_CD=A.TRAN_CD and B.ENTRY_TY=A.ENTRY_TY) FROM #RECEIPT A -- Added By Pankaj B. On 24-04-2014 for Bug-23326 


UPDATE  #RECEIPT SET PMV=(TOTPMV*(QTY * RATE))/(CASE WHEN ASSEAMT=0 THEN 1 ELSE ASSEAMT END)  WHERE PMKEY='+'  --Added by shrikant s. on 06 Apr, 2010 For TKT-863
----<-Update (Item wise i.e. Sales Tax,Packing Forwarding )PMV form total Voucher wise plus/minus amount  from dcmast,stax_mas into ##STKVAL1 from all Item Tables

----->Calculate FRATE=fianal rate

UPDATE  #RECEIPT SET FRATE=(((QTY*RATE)/RATEPER)+PMI+PMV)/(CASE WHEN qty=0 THEN 1 ELSE qty END)   WHERE PMKEY='+'

UPDATE  #RECEIPT SET FRATE=0 WHERE BHENT='SR'
----<-Calculate FRATE=fianal rate
----->Update FRATE TO RECEIPT WHERE FRATE=0 WITH PREV.ENTRY RATE.


SET @LRATE=0
SET @MIT_CODE=-1
SET @MWARE_NM=' '
SET @MVNAME	=' '	--Bug20309
SELECT * INTO #TRECEIPT FROM #RECEIPT



---->Update FRATE TO RECEIPT WHERE FRATE=0 WITH PREV.ENTRY RATE.
	
Select RowId=IDENTITY(Numeric(10,0), 1, 1),* Into #totReceipt From #TRECEIPT
	ORDER BY VNAME,IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN (CASE WHEN ENTRY_TY='SR' THEN 'C' ELSE 'B' end) ELSE 'D' END) END),TRAN_CD,ITSERIAL

CREATE NONCLUSTERED INDEX [IX_RowId]
    ON #totReceipt
     (
      [It_code] ASC,
      [RowId] Asc
     )   
INCLUDE ( [FRATE])
    WITH (
      PAD_INDEX  = OFF,
      STATISTICS_NORECOMPUTE  = OFF,
      SORT_IN_TEMPDB = OFF,
      IGNORE_DUP_KEY = OFF,
      DROP_EXISTING = OFF,
      ONLINE = OFF, ALLOW_ROW_LOCKS  = ON,
      ALLOW_PAGE_LOCKS  = ON)
    ON [PRIMARY]   ;
    
uPDATE a SET FRATE=(select Top 1 b.FRATE from #totReceipt b Where b.RowId < a.RowId  
	 and FRATE >0 and b.it_code=a.it_code order by b.RowId desc)
	 from #totReceipt a 
	Where a.Frate=0 and Pmkey='+'


UPDATE A SET A.FRATE=C.FRATE
FROM ##STKVAL1  A 
LEFT JOIN #totReceipt C ON (C.ENTRY_TY=A.ENTRY_TY AND C.TRAN_CD=A.TRAN_CD AND C.ITSERIAL=A.ITSERIAL)

---->Update Frate with Allocated Entry raete (AR<-PT)
SELECT A.ENTRY_TY,A.DATE,A.TRAN_CD,A.ITSERIAL,A.RENTRY_TY,A.ITREF_TRAN,A.RITSERIAL INTO #ITR1 FROM STKL_VW_ITREF A INNER JOIN LCODE B ON (A.RENTRY_TY=B.ENTRY_TY) INNER JOIN LCODE C ON (A.ENTRY_TY=C.ENTRY_TY) where B.inv_stk<>' ' AND B.INV_STK=C.INV_STK --AND A.DATE<=@EDATE

UPDATE A SET A.FRATE=C.FRATE 
FROM ##STKVAL1  A 
LEFT JOIN #ITR1 B ON (A.ENTRY_TY=B.RENTRY_TY AND A.TRAN_CD=B.ITREF_TRAN AND A.ITSERIAL=B.RITSERIAL)
LEFT JOIN #totReceipt C ON (C.ENTRY_TY=B.ENTRY_TY AND C.TRAN_CD=B.TRAN_CD AND C.ITSERIAL=B.ITSERIAL)
WHERE (ISNULL(C.RATE,0)<>0 AND C.RATE<>0)



---<-Update Frate with Allocated Entry raete (AR<-PT)

select IT_CODE,IT_NAME,RATEUNIT,OPBAL=Qty,OPAMT=Qty * Rate,RQTY=Qty ,RAMT=Qty * Rate,IQty=Qty,IAMT=Qty * Rate ,CLBAL=Qty,CLAMT=Qty * Rate,Vname,Vtype  
			Into #tmpStkVal
			from ##STKVAL1 a 			
			Where 1=2

--select * from #tmpStkVal			
;
With stock as
	(
		select ROW_NUMBER() OVER (Partition by VNAME,Vtype,It_code 
		Order by VNAME,IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN (CASE WHEN ENTRY_TY='SR' THEN 'C' ELSE 'B' end) ELSE 'D' END) END),TRAN_CD,ITSERIAL
		) AS ItemRowId,
		Row_Number() over(Order by VNAME,IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN (CASE WHEN ENTRY_TY='SR' THEN 'C' ELSE 'B' end) ELSE 'D' END) END),TRAN_CD,ITSERIAL) 
		as UniqueId,
		* from ##STKVAL1 		
	)
	insert Into #stk (TranDate,Tran_cd,It_code,Entry_ty,Qty,Rate,Pmkey,Vname,Vtype,It_name,Itserial,ItemRowId,RATEUNIT,OpRunningTotal,RunningTotal,UniqueId)
	SELECT Date,Tran_cd,It_code,Entry_ty,Qty,FRate,Pmkey,Vname,Vtype,It_name,Itserial,ItemRowId,RATEUNIT,0,0,UniqueId 
	FROM stock
	
	Update #stk set rate=isnull(rate,0),OpQty=0,OpAmt=0,ClQty=0,ClAmt=0
	;
	
CREATE CLUSTERED INDEX [IX_UniqueId] ON #stk
(
	UniqueId Asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;

	With cteStockSum as
	(
	SELECT  Rateunit,Vname,Vtype, It_code ,it_Name,
					SUM(Case when Trandate < @Sdate or Entry_ty='OS' then (CASE WHEN Pmkey = '-' THEN  - Qty ELSE Qty END) 
							Else 0 End) AS OpQty
					,SUM(Case when (Trandate>=@Sdate) AND Entry_ty<>'OS'  and Pmkey = '+' THEN Qty
							 Else 0 End) AS RQty   
					,SUM(Case when (Trandate>=@Sdate) AND Entry_ty<>'OS'  and Pmkey = '+' THEN Qty * Rate
							 Else 0 End) AS RAmt
					,SUM(Case when (Trandate>=@Sdate) AND Entry_ty<>'OS' and Pmkey = '-' THEN Qty
							 Else 0 End) AS IQty              
					,SUM(CASE WHEN Pmkey = '-' THEN - Qty ELSE Qty END) AS BalQty    
		   FROM  #stk
		   GROUP BY It_code,Vname,Vtype,It_name,Rateunit
	)
	Select * Into #cteStockSum from cteStockSum;
	
	Select Vname,VType,It_code,ItemRowId,Qty,UniqueId,TranDate,Entry_ty Into #tmp From #stk Where Pmkey='+' 
	

	Declare @ItemRowId Numeric(18,0),@RunQty Numeric(36,4),@VType1 Varchar(50),@UniqueId Numeric(18,0),@OpRunQty Numeric(36,4),@Trandate SmallDatetime
	Declare @OpRowId Numeric(10,0),@OpTransbalQty Numeric(36,4),@CurRowId Numeric(10,0),@TransBalQty Numeric(36,4)
	Select @RunQty=0,@Vname1='',@VType1='',@It_code1=0,@OpRunQty=0

	
If Exists(Select Top 1 Vtype From #stk Where Vtype='FIFO')
BEGIN
	Print 'Calculating FIFO wise'	


	Declare updatecur cursor for
		Select Vname,VType,It_code,UniqueId,ItemRowId,Qty,TranDate,ENTRY_TY From #tmp Order by  Vname,VType,It_code,UniqueId Desc,ItemRowId 

	Open updatecur 
	Fetch next From updatecur Into @Vname,@VType,@It_code,@UniqueId,@ItemRowId,@Qty, @TranDate,@ENTRY_TY 
	While @@Fetch_Status=0
	Begin
		if @Vname1<>@Vname or @It_code1<>@It_code
		Begin
			Select @RunQty=0,@Vname1=@Vname,@VType1=@Vname,@It_code1=@It_code,@OpRunQty=0
		end
		
		set @RunQty=@RunQty+@Qty
		if @Trandate < @Sdate or @Entry_ty='OS' 
		Begin
			set @OpRunQty=@OpRunQty+@Qty
		End

		Update #stk set RunningTotal=@RunQty,OpRunningTotal=@OpRunQty Where UniqueId=@UniqueId

		Fetch next From updatecur Into @Vname,@VType,@It_code,@UniqueId,@ItemRowId,@Qty, @TranDate,@ENTRY_TY 
	End
	Close updatecur
	Deallocate updatecur
;
	

		With cteReverseInSumOpening as 
		(
		SELECT  s.It_code ,
					   s.Trandate ,
						s.OpRunningTotal,
					   s.Qty AS Qty,s.ItemRowId,s.VName,s.VType,s.UniqueId
			   FROM    #stk AS s
			   WHERE   s.Pmkey = '+' and (Trandate < @Sdate or Entry_ty='OS' )
				AND s.Vtype='FIFO'
		),
			cteReverseInSumCurrent as 
			(
				SELECT s.It_code ,
					   s.Trandate ,
					   s.RunningTotal,
					   s.Qty AS Qty,s.ItemRowId,s.VName,s.VType,s.UniqueId
			   FROM    #stk AS s
			   WHERE   s.Pmkey = '+'  AND s.Vtype='FIFO'
			),
				cteWithLastTranDate as 
				(
					SELECT   w.It_code, 
					w.BalQty ,
					PartialStock.TranDate ,
					PartialStock.StockToUse ,
					PartialStock.RunningTotal ,
					PartialStock.CurRowId,
					w.BalQty - PartialStock.RunningTotal + PartialStock.StockToUse AS TransBalQty,
					w.OpQty,
					OpPartialStock.OpTranDate,
					OpPartialStock.OpStockToUse ,
					OpPartialStock.OpRunningTotal ,
					w.OpQty - OpPartialStock.OpRunningTotal + OpPartialStock.OpStockToUse AS OpTransBalQty,
					OpPartialStock.OpRowId
					,w.Vname,w.VType
				   FROM #cteStockSum AS w
					Outer APPLY ( SELECT TOP ( 1 )
										z.Trandate,
										z.Qty AS StockToUse ,
										z.RunningTotal,
										z.ItemRowId as CurRowId
								  FROM  cteReverseInSumCurrent AS z
								  WHERE z.VName=w.VName
										and z.It_code = w.It_code 
										AND z.RunningTotal >= w.BalQty
								  ORDER BY  z.Trandate DESC
								) AS PartialStock
	                   
					Outer APPLY ( SELECT TOP ( 1 )
										z.Trandate as OpTranDate,
										z.Qty AS OpStockToUse ,
										z.OpRunningTotal, 
										z.ItemRowId as OpRowId
								  FROM  cteReverseInSumOpening AS z
								  WHERE z.VName=w.VName
										and z.It_code = w.It_code 
										AND z.OpRunningTotal >= w.OpQty
								  ORDER BY  z.Trandate DESC
								) AS OpPartialStock           

				)
				select * Into #cteWithLastTranDateFifo from cteWithLastTranDate	
				
				
				
				Declare updatecur cursor for 
				Select Vname,It_code,OpRowId,OpTransbalQty,CurRowId,TransBalQty From #cteWithLastTranDateFifo Order by  Vname,VType,It_code

					Open updatecur 
					Fetch next From updatecur Into @Vname,@It_code,@OpRowId,@OpTransbalQty,@CurRowId,@TransBalQty

					While @@Fetch_Status=0
					Begin
				
						Update #stk set OpQty=(Case when ItemRowId>=@OpRowId and (Trandate < @Sdate or Entry_ty='OS' )  then (Case When ItemRowId=@OpRowId then @OpTransbalQty else Qty End) Else 0 End), 
							OpAmt=(Case when ItemRowId>=@OpRowId and (Trandate < @Sdate or Entry_ty='OS' ) then (Case When ItemRowId=@OpRowId then @OpTransbalQty else Qty End) Else 0 End) * Rate, 
							ClQty=(Case when ItemRowId>=@CurRowId then (Case When ItemRowId=@CurRowId then @TransBalQty else Qty End) Else 0 End), 
							ClAmt=(Case when ItemRowId>=@CurRowId then (Case When ItemRowId=@CurRowId then @TransBalQty else Qty End) Else 0 End) * Rate
							Where Vname=@Vname and It_code=@It_code and pmkey='+'
	
						Fetch next From updatecur Into @Vname,@It_code,@OpRowId,@OpTransbalQty,@CurRowId,@TransBalQty
					End
					Close updatecur
					Deallocate updatecur
					
				Print 'End qty Update '+convert(varchar(50),getdate(),113);

			--select * from #cteStockSum
			
			--select * from #cteWithLastTranDate
			
			--select * from #stk
			
			insert Into #tmpStkVal select a.IT_CODE,a.IT_NAME,a.RATEUNIT,OPBAL=Isnull(CalcValues.OpQty,0),OPAMT=Isnull(CalcValues.OpAmt,0),a.RQTY,a.RAMT,a.IQty,IAMT=(Isnull(CalcValues.OpAmt,0)+a.RAmt-Isnull(CalcValues.Clamt,0))
			,CLBAL=Isnull(CalcValues.ClQty,0),CLAMT=Isnull(CalcValues.Clamt,0),a.Vname,a.Vtype 
			From #cteStockSum  a 
			outer apply ( select OpQty=sum(OpQty),OpAmt=Sum(OpAmt),ClQty=Sum(ClQty),ClAmt=Sum(ClAmt) from #stk b Where b.It_code=a.It_code and b.VName=a.Vname ) as CalcValues 
			Order by a.It_code,a.It_name


			DELETE FROM #tmpStkVal WHERE OPBAL=0 AND RQTY=0 AND IQty=0 AND CLBAL=0
			
			If @ShowNil like '%NO%'
			BEGIN
				--print @EXPARA
				delete from #tmpStkVal where CLBAL=0
			END

END


print 'b'
;
If Exists(Select Top 1 Vtype From #stk Where Vtype='LIFO')
BEGIN
	Print 'Calculating LIFO wise';	
	
	Declare updatecur cursor for
		Select Vname,VType,It_code,UniqueId,ItemRowId,Qty,TranDate,ENTRY_TY From #tmp Order by  Vname,VType,It_code,UniqueId,ItemRowId 

	Open updatecur 
	Fetch next From updatecur Into @Vname,@VType,@It_code,@UniqueId,@ItemRowId,@Qty, @TranDate,@ENTRY_TY 
	While @@Fetch_Status=0
	Begin
		if @Vname1<>@Vname or @It_code1<>@It_code
		Begin
			Select @RunQty=0,@Vname1=@Vname,@VType1=@Vname,@It_code1=@It_code,@OpRunQty=0
		end
		
		set @RunQty=@RunQty+@Qty
		if @Trandate < @Sdate or @Entry_ty='OS' 
		Begin
			set @OpRunQty=@OpRunQty+@Qty
		End

		Update #stk set RunningTotal=@RunQty,OpRunningTotal=@OpRunQty Where UniqueId=@UniqueId

		Fetch next From updatecur Into @Vname,@VType,@It_code,@UniqueId,@ItemRowId,@Qty, @TranDate,@ENTRY_TY 
	End
	Close updatecur
	Deallocate updatecur
;
	With cteReverseInSumOpening as 
		(
		SELECT  s.It_code ,s.Trandate ,s.OpRunningTotal ,
					   s.Qty AS Qty,s.ItemRowId,s.VName,s.VType,s.UniqueId
			   FROM    #stk AS s
			   WHERE   s.Pmkey = '+' and (Trandate < @Sdate or Entry_ty='OS' )
						and s.Vtype='LIFO'
		),
			cteReverseInSumCurrent as 
			(
				SELECT s.It_code ,s.Trandate ,s.RunningTotal ,
					   s.Qty AS Qty,s.ItemRowId,s.VName,s.VType,s.UniqueId
			   FROM    #stk AS s
			   WHERE   s.Pmkey = '+'  and s.Vtype='LIFO'
			),
				cteWithLastTranDate as 
				(
					SELECT   w.It_code, 
					w.BalQty ,
					PartialStock.TranDate ,
					PartialStock.StockToUse ,
					PartialStock.RunningTotal ,
					PartialStock.CurRowId,
					w.BalQty - PartialStock.RunningTotal + PartialStock.StockToUse AS TransBalQty,
					w.OpQty,
					OpPartialStock.OpTranDate,
					OpPartialStock.OpStockToUse ,
					OpPartialStock.OpRunningTotal ,
					w.OpQty - OpPartialStock.OpRunningTotal + OpPartialStock.OpStockToUse AS OpTransBalQty,
					OpPartialStock.OpRowId,w.Vname,w.VType
				   FROM #cteStockSum AS w
					Outer APPLY ( SELECT TOP ( 1 )
										z.Trandate,
										z.Qty AS StockToUse ,
										z.RunningTotal,
										z.ItemRowId as CurRowId 
								  FROM  cteReverseInSumCurrent AS z
								  WHERE z.VName=w.VName
										and z.It_code = w.It_code 
										AND z.RunningTotal >= w.BalQty
								  ORDER BY  z.Trandate ASC
								) AS PartialStock
	                   
					Outer APPLY ( SELECT TOP ( 1 )
										z.Trandate as OpTranDate,
										z.Qty AS OpStockToUse ,
										z.OpRunningTotal, 
										z.ItemRowId as OpRowId 
								  FROM  cteReverseInSumOpening AS z
								  WHERE z.VName=w.VName
										and z.It_code = w.It_code 
										AND z.OpRunningTotal >= w.OpQty
								  ORDER BY  z.Trandate ASC
								) AS OpPartialStock           

				)
				select * Into #cteWithLastTranDateLifo from cteWithLastTranDate	
				
				Declare updatecur cursor for 
				Select Vname,It_code,OpRowId,OpTransbalQty,CurRowId,TransBalQty From #cteWithLastTranDateLifo Order by  Vname,VType,It_code

					Open updatecur 
					Fetch next From updatecur Into @Vname,@It_code,@OpRowId,@OpTransbalQty,@CurRowId,@TransBalQty

					While @@Fetch_Status=0
					Begin
				
						Update #stk set OpQty=(Case when ItemRowId<=@OpRowId and (Trandate < @Sdate or Entry_ty='OS' )  then (Case When ItemRowId=@OpRowId then @OpTransbalQty else Qty End) Else 0 End), 
							OpAmt=(Case when ItemRowId<=@OpRowId and (Trandate < @Sdate or Entry_ty='OS' ) then (Case When ItemRowId=@OpRowId then @OpTransbalQty else Qty End) Else 0 End) * Rate, 
							ClQty=(Case when ItemRowId<=@CurRowId then (Case When ItemRowId=@CurRowId then @TransBalQty else Qty End) Else 0 End), 
							ClAmt=(Case when ItemRowId<=@CurRowId then (Case When ItemRowId=@CurRowId then @TransBalQty else Qty End) Else 0 End) * Rate
							Where Vname=@Vname and It_code=@It_code and pmkey='+'
	
						Fetch next From updatecur Into @Vname,@It_code,@OpRowId,@OpTransbalQty,@CurRowId,@TransBalQty
					End
					Close updatecur
					Deallocate updatecur

			--select * from #cteStockSum
			
			--select * from #cteWithLastTranDateLifo
			
			--select * from #stk
								
			insert Into #tmpStkVal select a.IT_CODE,a.IT_NAME,a.RATEUNIT,OPBAL=Isnull(CalcValues.OpQty,0),OPAMT=Isnull(CalcValues.OpAmt,0),a.RQTY,a.RAMT,a.IQty,IAMT=(Isnull(CalcValues.OpAmt,0)+a.RAmt-Isnull(CalcValues.Clamt,0))
			,CLBAL=Isnull(CalcValues.ClQty,0),CLAMT=Isnull(CalcValues.Clamt,0),a.Vname,a.Vtype 
			From #cteStockSum  a 
			outer apply ( select OpQty=sum(OpQty),OpAmt=Sum(OpAmt),ClQty=Sum(ClQty),ClAmt=Sum(ClAmt) from #stk b Where b.It_code=a.It_code and b.VName=a.Vname ) as CalcValues 
			Order by a.It_code,a.It_name


			DELETE FROM #tmpStkVal WHERE OPBAL=0 AND RQTY=0 AND IQty=0 AND CLBAL=0
			
			If @ShowNil like '%NO%'
			BEGIN
				delete from #tmpStkVal where CLBAL=0
			END

END

If Exists(Select Top 1 Vtype From #stk Where Vtype='Weighted Average')
BEGIN
	
	declare @av_balqty numeric(17,3),@av_balamt numeric(17,2),@RATEUNIT Varchar(10),@CNT Int
	
	set @CNT=0

	SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE=FRATE,PMKEY,CNT=0,IT_NAME,DC_NO,VNAME,RATEUNIT,Vtype=space(20) INTO ##STKVALAVG FROM ##STKVAL1 WHERE 1=2 
	
	DECLARE STKVALCRSOR1 CURSOR FOR 
	SELECT ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,FRATE,PMKEY,IT_NAME,DC_NO,VNAME,RATEUNIT,Vtype FROM  ##STKVAL1 WHERE VTYPE = 'Weighted Average' ORDER BY VNAME,IT_CODE,WARE_NM,DATE,(CASE WHEN ENTRY_TY='OS' THEN 'A' ELSE (CASE WHEN PMKEY='+' THEN 'B' ELSE 'C'END) END),TRAN_CD,ITSERIAL 
	
	OPEN STKVALCRSOR1
	FETCH NEXT FROM STKVALCRSOR1 INTO @ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,@PMKEY,@IT_NAME,@DC_NO,@VNAME,@RATEUNIT,@Vtype
	WHILE (@@FETCH_STATUS=0)
	BEGIN

	IF (@MIT_CODE<>@IT_CODE)  OR (@MVNAME<>@VNAME) 
	BEGIN
		SET @MIT_CODE=@IT_CODE
		set @av_balqty=0
		set @av_balamt=0
		SET @MVNAME=@VNAME		
	END
	if @pmkey='+' 
	begin
		set @av_balqty=@av_balqty+@qty
		set @av_balamt=@av_balamt+(@qty*@rate)
	end
	if @pmkey='-'
	begin
		if @av_balqty<>0
		begin
			set @rate=@av_balamt/@av_balqty
		end
		else
		begin
			set @rate=0
		end	
		set @av_balqty=@av_balqty-@qty
		set @av_balamt=@av_balamt-(@qty*@rate)
	end 
	INSERT INTO ##STKVALAVG
		(ENTRY_TY,TRAN_CD,ITSERIAL,WARE_NM,DATE,IT_CODE,QTY,RATE,PMKEY,CNT,IT_NAME,DC_NO,VNAME,RATEUNIT)		
		VALUES (@ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,@PMKEY,@CNT,@IT_NAME,@DC_NO,@VNAME,@RATEUNIT) 

		
		FETCH NEXT FROM STKVALCRSOR1 INTO @ENTRY_TY,@TRAN_CD,@ITSERIAL,@WARE_NM,@DATE,@IT_CODE,@QTY,@RATE,@PMKEY,@IT_NAME,@DC_NO,@VNAME ,@RATEUNIT,@Vtype
	END
	CLOSE STKVALCRSOR1
	DEALLOCATE STKVALCRSOR1
	
			
	insert Into #tmpStkVal (IT_CODE,IT_NAME,RATEUNIT,OPBAL,OPAMT,RQTY,RAMT,IQTY,IAMT,CLBAL,CLAMT,VNAME,VTYPE)
	SELECT IT_CODE,IT_NAME,RATEUNIT
	,OPBAL=SUM(CASE WHEN ENTRY_TY='OS' OR DATE<@SDATE THEN (CASE WHEN PMKEY='+' THEN (CASE WHEN DC_NO='DI' THEN 0 ELSE QTY END ) ELSE -QTY END) ELSE 0 	END )
	,OPAMT=SUM(CASE WHEN ENTRY_TY='OS' OR DATE<@SDATE THEN (CASE WHEN PMKEY='+' THEN QTY ELSE -QTY END)*RATE ELSE 0 END )
	,RQTY=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='+') THEN (CASE WHEN DC_NO='DI' THEN 0 ELSE QTY END ) ELSE 0 END) 
	,RAMT=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='+') THEN QTY*RATE ELSE 0 END)
	,IQTY=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='-') THEN QTY ELSE 0 END)
	,IAMT=SUM(CASE WHEN (ENTRY_TY<>'OS' AND DATE>=@SDATE AND PMKEY='-') THEN QTY*RATE ELSE 0 END)
	,CLBAL=SUM((CASE WHEN PMKEY='+' THEN (CASE WHEN DC_NO='DI' THEN 0 ELSE QTY END ) ELSE -QTY END) )  
	,CLAMT=SUM((CASE WHEN PMKEY='+' THEN QTY ELSE -QTY END)*RATE )
	,VNAME,VTYPE
	 FROM  ##STKVALAVG GROUP BY IT_CODE,IT_NAME,VNAME,Vtype,RATEUNIT ORDER BY IT_NAME,VNAME	
	
	DROP TABLE ##STKVALAVG
END

IF @TBLNM<>''
BEGIN
	set @SQLCOMMAND=''
	set @SQLCOMMAND=@SQLCOMMAND+' '+'Select * Into '+@TBLNM+' From #tmpStkVal'
	EXEC SP_EXECUTESQL  @SQLCOMMAND
	
	SELECT @SQLCOMMAND = 'CREATE CLUSTERED INDEX Ix_Itname ON '+@TBLNM
	SELECT @SQLCOMMAND = @SQLCOMMAND+' ( IT_NAME ) ' 
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
END
ELSE
BEGIN
	SELECT @SQLCOMMAND = 'CREATE CLUSTERED INDEX Ix_Itname ON #tmpStkVal'
	SELECT @SQLCOMMAND = @SQLCOMMAND+' ( It_Name ) ' 
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	set @SQLCOMMAND=''
	set @SQLCOMMAND=@SQLCOMMAND+' '+'Select * From #tmpStkVal '
	EXEC SP_EXECUTESQL  @SQLCOMMAND
END





DROP TABLE ##STKVAL
DROP TABLE ##STKVAL1
Drop Table ##STKVALConfig
drop table #tmp

