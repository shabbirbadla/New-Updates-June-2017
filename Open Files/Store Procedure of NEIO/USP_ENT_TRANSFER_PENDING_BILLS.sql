create Procedure USP_ENT_TRANSFER_PENDING_BILLS
@DbName Varchar(10),@Edate SmallDateTime,@LYN Varchar(10)

as 
set Nocount On
Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50),@Entry_Table Varchar(2)
Declare @Entry_ty Varchar(2),@Bcode_nm Varchar(2),@TblName Varchar(30),@SqlCmd NVarchar(max),@fldlist Varchar(max),@cnt Int
Declare @TblName2 Varchar(30),@TblName3 Varchar(30)
Declare @Entry_ty1 Varchar(2),@Bcode_nm1 Varchar(2)


DECLARE @GRPID AS INT,@MCOND AS BIT,@LVL  AS INT,@GRP AS VARCHAR(100)
CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
SET @LVL=0
INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME in ('SUNDRY CREDITORS','SUNDRY DEBTORS')
SET @MCOND=1
WHILE @MCOND=1
BEGIN
	IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
	BEGIN
--		PRINT @LVL
		INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
		SET @LVL=@LVL+1
	END
	ELSE
	BEGIN
		SET @MCOND=0	
	END
END
SELECT AC_ID,AC_NAME INTO #ACMAST FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)


--Select Ac_Name,Ac_Id Into #ACMAST From Ac_Mast Where Type='B'and Not (Typ  in ('EXCISE') OR AC_NAME Like '%CAPITAL GOODS PAYABLE%')  

Set @OPENTRY_TY = CHAR(39)+'OB'+CHAR(39)
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

---------------------------			Getting Allocation Records to the Table		--------------------------Start
Select ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,billamt=ac.amount
,recamt=sum(isnull(new_all,0)+ISNULL(MLL.TDS,0)+ISNULL(MLL.DISC,0)),ac.l_yn,ac.amt_ty
Into #tmp1 From Lac_vw ac
INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=AC.AC_ID)  
INNER JOIN #ACMAST AM ON (AC_MAST.AC_ID=AM.AC_ID)  
INNER JOIN LMAIN_VW MN ON (AC.ENTRY_TY=MN.ENTRY_TY AND AC.TRAN_CD=MN.TRAN_CD)  
LEFT JOIN MAINALL_VW MLL ON (AC.entry_ty=MLL.entry_all and AC.tran_cd =MLL.main_tran and AC.acserial =MLL.acseri_all and AC.AC_ID=MLL.AC_ID) AND MLL.DATE <= @Edate
Where ac.Date<=@Edate 
Group By ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,ac.amt_ty

--SELECT * FROM #tmp1

Select ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,billamt=ac.amount
,recamt=sum(isnull(MLY.new_all,0)+ISNULL(MLY.TDS,0)+ISNULL(MLY.DISC,0)),ac.l_yn,ac.amt_ty
Into #tmp2 From Lac_vw ac
INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=AC.AC_ID)  
INNER JOIN #ACMAST AM ON (AC_MAST.AC_ID=AM.AC_ID)  
INNER JOIN LMAIN_VW MN ON (AC.ENTRY_TY=MN.ENTRY_TY AND AC.TRAN_CD=MN.TRAN_CD)  
LEFT JOIN MAINALL_VW MLY ON (AC.entry_ty=MLY.Entry_ty and AC.tran_cd =MLY.tran_CD and AC.acserial =MLY.acserial and AC.AC_ID=MLY.AC_ID) AND MLY.DATE <= @Edate
Where ac.Date<=@Edate
Group By ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,ac.amt_ty

--SELECT * FROM #tmp2

SELECT ENTRY_TY,TRAN_CD,AC_ID,ACSERIAL  
,RECAMT=new_all
 Into #tmpalloc FROM NEWYEAR_ALLOC    


Select a.Entry_ty,a.Tran_cd,a.acserial,a.ac_id,a.ac_name,a.billamt,
recamt=a.recamt+b.recamt+isnull(c.recamt,0),a.l_yn,a.amt_ty
Into #tmp4 from #tmp1 a 
Inner Join #tmp2 b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd and a.acserial=b.acserial and a.ac_id=b.ac_id)
left Join #tmpalloc c on (a.Entry_ty=c.Entry_ty and a.Tran_cd=c.Tran_cd and a.acserial=c.acserial and a.ac_id=c.ac_id)
Where a.billamt<>a.recamt+b.recamt+isnull(c.recamt,0)

Delete from #tmp4  Where ac_id In (Select ac_id from ac_mast Where [group] IN ('CASH & BANK BALANCES') or typ='BANK')
---------------------------			Getting Allocation Records to the Table		--------------------------End

SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,
MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME,AVW.ACSERIAL,MVW.U_PINVNO,MVW.U_PINVDT
,AC_MAST.TYP,u_cldt
into #tmpAcdet FROM LAC_VW AVW (NOLOCK)
INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID
INNER JOIN LMAIN_VW MVW (NOLOCK) 
ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY
WHERE 1=2

set @SqlCmd='INSERT iNTO #tmpAcdet SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,'
set @SqlCmd=@SqlCmd+' '+'MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME,AVW.ACSERIAL,MVW.U_PINVNO,MVW.U_PINVDT'
set @SqlCmd=@SqlCmd+' '+',AC_MAST.TYP,AVW.U_CLDT'
set @SqlCmd=@SqlCmd+' '+'FROM LAC_VW AVW (NOLOCK)'
set @SqlCmd=@SqlCmd+' '+'INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID'		
set @SqlCmd=@SqlCmd+' '+'INNER JOIN #ACMAST ON #ACMAST.AC_ID = AC_MAST.AC_ID'		
set @SqlCmd=@SqlCmd+' '+'INNER JOIN LMAIN_VW MVW (NOLOCK) '
set @SqlCmd=@SqlCmd+' '+'ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY'
set @SqlCmd=@SqlCmd+' '+'WHERE MVW.DATE < = '''+convert(Varchar(50),@Edate)+''' '
Execute Sp_ExecuteSql @SqlCmd

SET @SqlCmd = 'DELETE FROM #tmpAcdet WHERE 	DATE < (SELECT TOP 1 DATE FROM #tmpAcdet
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'') )
		AND AC_NAME IN (SELECT AC_NAME FROM #tmpAcdet
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'')  GROUP BY AC_NAME)'
EXECUTE sp_executesql @SqlCmd

Select Ac_id,Ac_name,Amount=sum(Case when Amt_ty='CR' Then -Amount Else Amount End)
			Into #tmpac From #tmpAcdet 
				Group By Ac_id,Ac_name having sum(Case when Amt_ty='CR' Then -Amount Else Amount End)<>0
					Order By Ac_Name

Delete from #tmp4  Where ac_id not In (Select Ac_id from #tmpac)



SET @SqlCmd = ''
SET @SqlCmd = 'DELETE FROM #tmp4 WHERE ENTRY_TY IN ('+@OPENTRY_TY+') AND L_YN = '''+@LYN+'''
	AND AC_NAME IN (SELECT AC_NAME FROM #tmp4 WHERE L_YN < '''+@LYN+''' GROUP BY AC_NAME) '
EXECUTE SP_EXECUTESQL @SqlCmd


---------------------------			Inserting Records to the Table		--------------------------Start
--Select Distinct a.Entry_ty, b.bcode_nm from (Select Entry_ty From #tmp4 ) a Inner Join Lcode b ON (a.Entry_ty=b.Entry_ty) Where Not(a.Entry_ty='OB' or b.bcode_nm='OB')
BEGIN try
Begin Transaction

Declare Entry_Cursor Cursor for
Select a.Entry_ty, b.bcode_nm from (Select distinct Entry_ty From #tmp4 ) a Inner Join Lcode b ON (a.Entry_ty=b.Entry_ty) Where Not(a.Entry_ty='OB' or b.bcode_nm='OB')

Open Entry_Cursor 

Fetch Next From Entry_Cursor Into @Entry_ty,@Bcode_nm 
While @@Fetch_Status=0
Begin
	set @Entry_Table=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End

	set @TblName=@Entry_Table+'Acdet'
	set @SqlCmd ='Delete From '+@DbName+'..'+@TblName+' Where Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial Not In '
	set @SqlCmd =@SqlCmd +' '+'(Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From '+@DbName+'..'+'mainall_vw Group by Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial '
	set @SqlCmd =@SqlCmd +' '+'Union all Select Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all From '+@DbName+'..'+'mainall_vw Group by Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all) and date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd

	set @TblName='NEWYEAR_ALLOC'
	set @SqlCmd ='Delete From '+@DbName+'..'+@TblName+' Where Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial Not In '
	set @SqlCmd =@SqlCmd +' '+'(Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From '+@DbName+'..'+'mainall_vw Group by Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial '
	set @SqlCmd =@SqlCmd +' '+'Union all Select Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all From '+@DbName+'..'+'mainall_vw Group by Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all) and date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd

	set @TblName=@Entry_Table+'Main'
	set @SqlCmd ='Delete From '+@DbName+'..'+@TblName+' Where Entry_ty+Convert(Varchar(10),Tran_cd) Not In '
	set @SqlCmd =@SqlCmd +' '+'(Select Entry_ty+Convert(Varchar(10),Tran_cd) From '+@DbName+'..'+'mainall_vw Group by Entry_ty+Convert(Varchar(10),Tran_cd) '
	set @SqlCmd =@SqlCmd +' '+'Union all Select Entry_all+Convert(Varchar(10),Main_Tran) From '+@DbName+'..'+'mainall_vw Group by Entry_all+Convert(Varchar(10),Main_Tran)) and date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd
		
	
	set @TblName=@Entry_Table+'Main'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	set @SqlCmd =''
	set @SqlCmd =' Set Identity_Insert '+@DbName+'..'+@TblName+' On'
	--set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..'+@TblName+' Where Tran_Cd In (Select Tran_cd From #tmp4)  and Entry_ty='''+@Entry_ty+''') '
	set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select '+@fldlist+' From '+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd ) In (Select entry_ty+convert(Varchar(10),Tran_cd ) From #tmp4)  and Entry_ty='''+@Entry_ty+''' and entry_ty+convert(Varchar(10),Tran_cd ) Not In (Select entry_ty+convert(Varchar(10),Tran_cd ) From '+@DbName+'..'+@TblName+')'
	set @SqlCmd =@SqlCmd +' Set Identity_Insert '+@DbName+'..'+@TblName+' Off'
	Execute sp_Executesql @SqlCmd

	-- Adding Records to Acdet Table
	set @TblName=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'Acdet'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	set @SqlCmd=''
	--set @SqlCmd = @SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..'+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''')'
	Set @SqlCmd = @SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select '+@fldlist+' from '+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''' and entry_ty+convert(Varchar(10),Tran_cd )+acserial Not in (Select entry_ty+convert(Varchar(10),Tran_cd )+acserial From '+@DbName+'..'+@TblName+' )'
	Exec Sp_ExecuteSql @SqlCmd

	set @SqlCmd=''
	--Set @SqlCmd=' Update '+@DbName+'..'+@TblName+' Set re_all=a.recamt from #tmp4 a, '+@DbName+'..'+@TblName+' b Where a.entry_ty+convert(Varchar(10),a.Tran_cd )+a.acserial =b.entry_ty+convert(Varchar(10),b.Tran_cd)+b.acserial '
	Set @SqlCmd=' Update '+@DbName+'..'+@TblName+' Set re_all=a.recamt from #tmp4 a, '+@DbName+'..'+@TblName+' b Where a.entry_ty+convert(Varchar(10),a.Tran_cd )+a.acserial =b.entry_ty+convert(Varchar(10),b.Tran_cd)+b.acserial  '
	set @SqlCmd =@SqlCmd +' '+' and b.Entry_ty+Convert(Varchar(10),b.Tran_cd)+b.Acserial Not In '
	set @SqlCmd =@SqlCmd +' '+'(Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From '+@DbName+'..'+'mainall_vw Group by Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial '
	set @SqlCmd =@SqlCmd +' '+'Union all Select Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all From '+@DbName+'..'+'mainall_vw Group by Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all) and b.date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Exec Sp_ExecuteSql @SqlCmd

-- Adding Records to Mall Table 	
	set @TblName=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'Mall'
	Execute USP_ENT_GETFIELD_LIST 'NEWYEAR_ALLOC', @fldlist Output 

	set @SqlCmd=''
	--set @SqlCmd =@SqlCmd +' If Not Exists(Select Top 1 Tran_Cd From '+@DbName+'..NEWYEAR_ALLOC Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) )' 
	Set @SqlCmd=@SqlCmd + ' Insert Into '+@DbName+'..NEWYEAR_ALLOC ('+@fldlist+') Select '+replace(@fldlist,'[fcnew_all]','fcnew_all=0')+' From '
			+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and entry_ty+convert(Varchar(10),Tran_cd)+acserial Not in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial From '+@DbName+'..NEWYEAR_ALLOC)' 
	Exec Sp_ExecuteSql @SqlCmd

	Set @SqlCmd=' Update '+@DbName+'..NEWYEAR_ALLOC Set new_all=case when a.amt_ty=''DR'' then b.new_all else - b.new_all end,tds=case when a.amt_ty=''DR'' then b.tds else - b.tds end from #tmp4 a inner join '+@DbName+'..NEWYEAR_ALLOC b on (a.tran_cd=b.tran_cd and a.entry_ty=b.entry_ty and a.acserial=b.acserial )'
	set @SqlCmd =@SqlCmd +' '+' and b.Entry_ty+Convert(Varchar(10),b.Tran_cd)+b.Acserial Not In '
	set @SqlCmd =@SqlCmd +' '+'(Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From '+@DbName+'..'+'mainall_vw Group by Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial '
	set @SqlCmd =@SqlCmd +' '+'Union all Select Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all From '+@DbName+'..'+'mainall_vw Group by Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all) and b.date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Exec Sp_ExecuteSql @SqlCmd
	
	set @SqlCmd=''
	set @SqlCmd ='Insert Into '+@DbName+'..NEWYEAR_ALLOC  ('+@fldlist+') '
	set @SqlCmd =@SqlCmd +' Select '+@fldlist+' From 
		NewYear_Alloc Where entry_ty+convert(Varchar(10),Tran_cd)+acserial 
		in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''' 
		and entry_all+convert(Varchar(10),main_tran)+acseri_all not in (Select entry_all+convert(Varchar(10),main_tran)+acseri_all from '+@DbName+'..NEWYEAR_ALLOC )'
	Exec Sp_ExecuteSql @SqlCmd

	Declare InnerCursor Cursor for
	Select Distinct mall.Entry_ty,l.bcode_nm From mainall_Vw mall Inner Join lcode l on (mall.Entry_ty=l.Entry_ty) Where Entry_All=@Entry_ty

	Open InnerCursor 

	Fetch Next From InnerCursor Into @Entry_ty1 ,@Bcode_nm1
	While @@Fetch_Status=0
	Begin
		Select @TblName2=case when @Bcode_nm1<>'' then @Bcode_nm1 else @Entry_ty1 end+'Mall' 

			Execute USP_ENT_GETFIELD_LIST 'NEWYEAR_ALLOC', @fldlist Output 
			set @SqlCmd=''
			--set @SqlCmd =@SqlCmd +' If Not Exists(Select Top 1 Tran_cd From '+@DbName+'..NEWYEAR_ALLOC Where entry_all+convert(Varchar(10),Main_Tran )+acseri_all in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4 Where Entry_ty='''+@Entry_ty+''' ) )' 
			set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+'NEWYEAR_ALLOC ('+@fldlist+') Select '+replace(@fldlist,'[fcnew_all]','fcnew_all=0')+' from '
					+@TblName2+' Where entry_all+convert(Varchar(10),Main_Tran )+acseri_all in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4 Where Entry_ty='''+@Entry_ty+''')  AND entry_all+convert(Varchar(10),Main_Tran )+acseri_all Not in (Select entry_all+convert(Varchar(10),Main_Tran )+acseri_all From ' +@DbName+'..'+'NEWYEAR_ALLOC )'
			Exec Sp_ExecuteSql @SqlCmd

			Set @SqlCmd=' Update '+@DbName+'..NEWYEAR_ALLOC Set new_all=case when a.amt_ty=''DR'' then b.new_all else - b.new_all end,tds=case when a.amt_ty=''DR'' then b.tds else - b.tds end from #tmp4 a inner join '+@DbName+'..NEWYEAR_ALLOC b on (a.tran_cd=b.main_tran and a.entry_ty=b.entry_all and a.acserial=b.acseri_all )'
			set @SqlCmd =@SqlCmd +' '+' and b.Entry_ty+Convert(Varchar(10),b.Tran_cd)+b.Acserial Not In '
			set @SqlCmd =@SqlCmd +' '+'(Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From '+@DbName+'..'+'mainall_vw Group by Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial '
			set @SqlCmd =@SqlCmd +' '+'Union all Select Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all From '+@DbName+'..'+'mainall_vw Group by Entry_all+Convert(Varchar(10),Main_Tran)+acseri_all) and b.date<='''+CONVERT(Varchar(50),@Edate)+''' '
			Exec Sp_ExecuteSql @SqlCmd
	
			set @SqlCmd=''
			set @SqlCmd ='Insert Into '+@DbName+'..NEWYEAR_ALLOC  ('+@fldlist+') '
			set @SqlCmd =@SqlCmd +' Select '+@fldlist+' From 
				NewYear_Alloc Where entry_all+convert(Varchar(10),Main_tran)+acseri_all 
					in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_all='''+@Entry_ty1+''' 
				and entry_ty+convert(Varchar(10),tran_cd)+acserial not in (Select entry_ty+convert(Varchar(10),tran_cd)+acserial from '+@DbName+'..NEWYEAR_ALLOC )'
			--print @SqlCmd
			Exec Sp_ExecuteSql @SqlCmd
		Fetch Next From InnerCursor Into @Entry_ty1 , @Bcode_nm1
	End
	Close InnerCursor 
	Deallocate InnerCursor 
	Fetch Next From Entry_Cursor Into @Entry_ty,@Bcode_nm 
End
Close Entry_Cursor 
Deallocate Entry_Cursor 

---------------------------			Inserting Records to the Table		--------------------------End
	commit Transaction

	Select 1 as Ans
enD Try
begin Catch
	Close InnerCursor 
	Deallocate InnerCursor 

	Rollback Transaction
	Select 0 as Ans,convert(Varchar(8000),ERROR_MESSAGE()) AS ErrorMsg
End catch

 

Drop Table #tmp4

Drop Table #tmp1
Drop Table #tmp2
Drop Table #ACMAST
Drop Table #tmpac
