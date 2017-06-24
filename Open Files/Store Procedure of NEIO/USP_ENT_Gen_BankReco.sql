Create Procedure USP_ENT_Gen_BankReco
@dbname Varchar(10),@sdate smalldatetime,@edate smalldatetime
as

--DECLARE @GRPID AS INT,@MCOND AS BIT,@LVL  AS INT,@GRP AS VARCHAR(100)

--SET @GRP='CASH & BANK BALANCES'
--CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
--SET @LVL=0
--INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME=@GRP
--SET @MCOND=1
--WHILE @MCOND=1
--BEGIN
--	IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
--	BEGIN
--		PRINT @LVL
--		INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
--		SET @LVL=@LVL+1
--	END
--	ELSE
--	BEGIN
--		SET @MCOND=0	
--	END
--END

select ac_name AS BANKNM INTO #BANK from AC_MAST where [typ]='CASH'

Declare @SqlCmd NVarchar(4000)
Set @SqlCmd ='Delete From '+RTRIM(@dbname)+'..Recostat Where date <'''+CONVERT(Varchar(50),@sdate)+''' and cl_date=0 '
execute sp_executesql @SqlCmd 

SELECT Entry_ty , Tran_Cd , Date , doc_no ,cl_Date ,ac_name , amt_ty , Credit,debit ,cheq_no ,clause,party_nm  FROM bankreco
Where ac_name not in (Select BANKNM from #BANK)
and (cl_date=0 OR ( cl_date BETWEEN @sdate AND @edate))
--INNER JOIN #BANK ON (#BANK.BANKNM!=bankreco.ac_name)
--WHERE cl_date=0 OR ( cl_date BETWEEN @sdate AND @edate)
UNION ALL
SELECT Entry_ty , Tran_Cd , Date , doc_no ,cl_Date ,ac_name , amt_ty , Credit=CASE WHEN amt_ty='CR' THEN amount ELSE 0 END 
,debit=CASE WHEN amt_ty='DR' THEN amount ELSE 0 END  ,cheq_no ,clause,party_nm=substring(oac_name,1,50)  FROM RECOSTAT
wHERE cl_date=0 OR ( cl_date BETWEEN @sdate AND @edate)

