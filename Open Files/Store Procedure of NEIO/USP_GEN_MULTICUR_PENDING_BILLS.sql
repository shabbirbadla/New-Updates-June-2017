If Exists(Select [Name] From sysobjects Where xtype='P' and [Name]='USP_GEN_MULTICUR_PENDING_BILLS')
Begin
	Drop Procedure USP_GEN_MULTICUR_PENDING_BILLS
End
GO
Create Procedure USP_GEN_MULTICUR_PENDING_BILLS
@DbName Varchar(10),@Edate SmallDateTime,@LYN Varchar(10)
AS
set NoCount On
Declare @Multi_cur Bit,@Multi_cur1 Bit,@Entry_ty Varchar(2),@ext_vou Bit
Declare @Bcode_nm Varchar(2),@TblName Varchar(10),@SqlCmd NVarchar(4000),@fldlist Varchar(8000),@cnt Int
Declare @TblName2 Varchar(10),@TblName3 Varchar(10)
Declare @Entry_ty1 Varchar(2),@Bcode_nm1 Varchar(2)

Select ac.Date,ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,fcid=ac_id,fcamount=amount,fcRe_all=re_all Into #lac_vw From lac_vw AC where 1=2

Select Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,fcNew_all=new_all,fcdisc=disc,fcTDS=tds Into #Mainall_VW From ptMall Where 1=2

Declare Entry_cursor Cursor for
Select Multi_cur,Entry_ty,ext_vou From Lcode Where Bcode_nm='' and ext_vou=0
Union all Select top 1 Multi_cur,Entry_ty,ext_vou From Lcode Where Bcode_nm='' and ext_vou=1


Open Entry_cursor
Fetch Next From Entry_cursor Into @Multi_cur,@Entry_ty,@ext_vou 

While @@Fetch_Status=0
Begin
	if @Multi_cur=1
	Begin
		set @Sqlcmd='insert Into #lac_vw Select ac.Date,ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,ac.fcid,ac.fcamount,ac.fcRe_all From '+case When @ext_vou=0 then @Entry_ty else '' End+'Acdet ac'	
		Execute sp_Executesql @Sqlcmd	
		set @Sqlcmd='Insert Into #Mainall_VW Select Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,fcNew_all,fcdisc,fcTDS From '+case When @ext_vou=0 then @Entry_ty else '' End+'Mall '	
		Execute sp_Executesql @Sqlcmd	
	End
	else
	Begin
		set @Sqlcmd='insert Into #lac_vw Select ac.Date,ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,fcid=0,fcamount=0,fcRe_all=0 From '+case When @ext_vou=0 then @Entry_ty else '' End+'Acdet ac'	
		Execute sp_Executesql @Sqlcmd	
		set @Sqlcmd='Insert Into #Mainall_VW Select Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,fcNew_all=0,fcdisc=0,fcTDS=0 From '+case When @ext_vou=0 then @Entry_ty else '' End+'Mall '		
		Execute sp_Executesql @Sqlcmd	
	End
	
	Fetch Next From Entry_cursor Into @Multi_cur ,@Entry_ty ,@ext_vou 
End
Close Entry_cursor
Deallocate Entry_cursor


Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
Select Ac_Name,Ac_Id Into #ACMAST From Ac_Mast Where Type='B'and Not (Typ  in ('EXCISE') OR AC_NAME Like '%CAPITAL GOODS PAYABLE%')  

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
,recamt=sum(isnull(new_all,0)+ISNULL(MLL.TDS,0)+ISNULL(MLL.DISC,0)),ac.l_yn
,fcBillamt=ac.fcamount,fcrecamt=sum(isnull(MLL.fcnew_all,0)+ISNULL(MLL.fcTDS,0)+ISNULL(MLL.fcDISC,0))
Into #tmp1 From #Lac_vw ac
INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=AC.AC_ID)  
INNER JOIN #ACMAST AM ON (AC_MAST.AC_ID=AM.AC_ID)  
INNER JOIN LMAIN_VW MN ON (AC.ENTRY_TY=MN.ENTRY_TY AND AC.TRAN_CD=MN.TRAN_CD)  
LEFT JOIN #MAINALL_VW MLL ON (AC.entry_ty=MLL.entry_all and AC.tran_cd =MLL.main_tran and AC.acserial =MLL.acseri_all and AC.AC_ID=MLL.AC_ID) AND MLL.DATE <= @Edate
Where ac.Date<=@Edate 
Group By ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,ac.fcamount

--SELECT * FROM #tmp1

Select ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,billamt=ac.amount
,recamt=sum(isnull(MLY.new_all,0)+ISNULL(MLY.TDS,0)+ISNULL(MLY.DISC,0)),ac.l_yn
,fcBillamt=ac.fcamount,fcrecamt=sum(isnull(MLY.fcnew_all,0)+ISNULL(MLY.fcTDS,0)+ISNULL(MLY.fcDISC,0))
Into #tmp2 From #Lac_vw ac
INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=AC.AC_ID)  
INNER JOIN #ACMAST AM ON (AC_MAST.AC_ID=AM.AC_ID)  
INNER JOIN LMAIN_VW MN ON (AC.ENTRY_TY=MN.ENTRY_TY AND AC.TRAN_CD=MN.TRAN_CD)  
LEFT JOIN #MAINALL_VW MLY ON (AC.entry_ty=MLY.Entry_ty and AC.tran_cd =MLY.tran_CD and AC.acserial =MLY.acserial and AC.AC_ID=MLY.AC_ID) AND MLY.DATE <= @Edate
Where ac.Date<=@Edate
Group By ac.Entry_ty,ac.Tran_cd,ac.acserial,ac.ac_id,ac.ac_name,ac.amount,ac.l_yn,ac.fcamount



SELECT ENTRY_TY,TRAN_CD,AC_ID,ACSERIAL  
,RECAMT=new_all,fcRecamt=fcnew_all
Into #tmpalloc FROM NEWYEAR_ALLOC    

Select a.Entry_ty,a.Tran_cd,a.acserial,a.ac_id,a.ac_name,a.billamt,
recamt=a.recamt+b.recamt+isnull(c.recamt,0),a.l_yn
,a.fcBillamt
,fcRecamt=a.fcrecamt+b.fcrecamt+isnull(c.fcrecamt,0)
Into #tmp4 from #tmp1 a 
Inner Join #tmp2 b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd and a.acserial=b.acserial and a.ac_id=b.ac_id)
left Join #tmpalloc c on (a.Entry_ty=c.Entry_ty and a.Tran_cd=c.Tran_cd and a.acserial=c.acserial and a.ac_id=c.ac_id)
Where (a.billamt<>a.recamt+b.recamt+isnull(c.recamt,0)) or (a.fcBillamt<>a.fcrecamt+b.fcrecamt+isnull(c.fcrecamt,0))

Delete from #tmp4  Where ac_id In (Select ac_id from ac_mast Where [group] IN ('CASH & BANK BALANCES'))
---------------------------			Getting Allocation Records to the Table		--------------------------End

SET @SqlCmd = ''
SET @SqlCmd = 'DELETE FROM #tmp4 WHERE ENTRY_TY IN ('+@OPENTRY_TY+') AND L_YN = '''+@LYN+'''
	AND AC_NAME IN (SELECT AC_NAME FROM #tmp4 WHERE L_YN < '''+@LYN+''' GROUP BY AC_NAME) '
EXECUTE SP_EXECUTESQL @SqlCmd

--Select * From #tmp4  

---------------------------			Inserting Records to the Table		--------------------------Start
Declare Entry_Cursor Cursor for
Select Distinct a.Entry_ty, b.bcode_nm,b.multi_cur from (Select Entry_ty From #tmp4 ) a Inner Join Lcode b ON (a.Entry_ty=b.Entry_ty) 
Open Entry_Cursor 

Fetch Next From Entry_Cursor Into @Entry_ty,@Bcode_nm,@Multi_cur 
While @@Fetch_Status=0
Begin
	set @TblName=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'Main'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	--print @fldlist
	set @SqlCmd =' Set Identity_Insert '+@DbName+'..'+Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'Main'+' On'
	set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..'+@TblName+' Where Tran_Cd In (Select Tran_cd From #tmp4)  and Entry_ty='''+@Entry_ty+''') '
	set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select * From '+@TblName+' Where Tran_cd In (Select Tran_cd From #tmp4)  and Entry_ty='''+@Entry_ty+''' '
	set @SqlCmd =@SqlCmd +' Set Identity_Insert '+@DbName+'..'+Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'Main'+' Off'
	Execute sp_Executesql @SqlCmd

	-- Adding Records to Acdet Table
	set @TblName=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'acdet'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 

	set @SqlCmd=''
	set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..'+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''' )'
	Set @SqlCmd=@SqlCmd+' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select * from '+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''''
	Exec Sp_ExecuteSql @SqlCmd

	set @SqlCmd=''
	if @Multi_cur=1
		Set @SqlCmd=' Update '+@DbName+'..'+@TblName+' Set re_all=a.recamt,fcre_all=a.fcre_all from #tmp4 a, '+@DbName+'..'+@TblName+' b Where a.entry_ty+convert(Varchar(10),a.Tran_cd )+a.acserial =b.entry_ty+convert(Varchar(10),b.Tran_cd)+b.acserial '
	else
		Set @SqlCmd=' Update '+@DbName+'..'+@TblName+' Set re_all=a.recamt from #tmp4 a, '+@DbName+'..'+@TblName+' b Where a.entry_ty+convert(Varchar(10),a.Tran_cd )+a.acserial =b.entry_ty+convert(Varchar(10),b.Tran_cd)+b.acserial '
	Exec Sp_ExecuteSql @SqlCmd

	-- Adding Records to Mall Table 	
	set @TblName=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End+'Mall'
	Execute USP_ENT_GETFIELD_LIST 'NEWYEAR_ALLOC', @fldlist Output 

	if @Multi_cur=1
	Begin
		set @SqlCmd=''
		set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..NEWYEAR_ALLOC Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) )' 
		Set @SqlCmd=@SqlCmd + ' Insert Into '+@DbName+'..NEWYEAR_ALLOC ('+@fldlist+') Select '+replace(@fldlist,'[fcnew_all]','fcnew_all=0')+' From '+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4)' 
		Exec Sp_ExecuteSql @SqlCmd

		set @SqlCmd=''
		set @SqlCmd ='Insert Into '+@DbName+'..NEWYEAR_ALLOC  ('+@fldlist+') '
		set @SqlCmd =@SqlCmd +' Select '+@fldlist+' From 
			NewYear_Alloc Where entry_ty+convert(Varchar(10),Tran_cd)+acserial 
			in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''' 
			and entry_all+convert(Varchar(10),main_tran)+acseri_all not in (Select entry_all+convert(Varchar(10),main_tran)+acseri_all from '+@DbName+'..NEWYEAR_ALLOC )'
		Exec Sp_ExecuteSql @SqlCmd
	End
	Else
	Begin
		--print @fldlist
		set @SqlCmd=''
		set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..NEWYEAR_ALLOC Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) )' 
		Set @SqlCmd=@SqlCmd + ' Insert Into '+@DbName+'..NEWYEAR_ALLOC ('+@fldlist+') Select '+replace(@fldlist,'[fcnew_all]','fcnew_all=0')+' From '+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+acserial in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4)' 
		Exec Sp_ExecuteSql @SqlCmd

		set @SqlCmd=''
			set @SqlCmd ='Insert Into '+@DbName+'..NEWYEAR_ALLOC  ('+@fldlist+') '
		set @SqlCmd =@SqlCmd +' Select '+@fldlist+' From 
			NewYear_Alloc Where entry_ty+convert(Varchar(10),Tran_cd)+acserial 
			in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_ty='''+@Entry_ty+''' 
			and entry_all+convert(Varchar(10),main_tran)+acseri_all not in (Select entry_all+convert(Varchar(10),main_tran)+acseri_all from '+@DbName+'..NEWYEAR_ALLOC )'
		Exec Sp_ExecuteSql @SqlCmd
	End

	Declare InnerCursor Cursor for
	Select Distinct mall.Entry_ty,l.bcode_nm,l.multi_cur From #Mainall_Vw mall Inner Join lcode l on (mall.Entry_ty=l.Entry_ty) Where Entry_All=@Entry_ty

	Open InnerCursor 

	Fetch Next From InnerCursor Into @Entry_ty1 ,@Bcode_nm1,@Multi_cur1
	While @@Fetch_Status=0
	Begin
		Select @TblName2=case when @Bcode_nm1<>'' then @Bcode_nm1 else @Entry_ty1 end+'Mall' 

			Execute USP_ENT_GETFIELD_LIST 'NEWYEAR_ALLOC', @fldlist Output 

			if @Multi_cur1=1
			Begin
				set @SqlCmd=''
				set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..NEWYEAR_ALLOC Where entry_all+convert(Varchar(10),Main_Tran )+acseri_all in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) )' 
				set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+'NEWYEAR_ALLOC ('+@fldlist+') Select '+replace(@fldlist,'[fcnew_all]','fcnew_all=0')+' from '+@TblName2+' Where entry_all+convert(Varchar(10),Main_Tran )+acseri_all in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) ' 
				Exec Sp_ExecuteSql @SqlCmd

				set @SqlCmd=''
				set @SqlCmd ='Insert Into '+@DbName+'..NEWYEAR_ALLOC  ('+@fldlist+') '
				set @SqlCmd =@SqlCmd +' Select '+@fldlist+' From 
					NewYear_Alloc Where entry_all+convert(Varchar(10),Main_tran)+acseri_all 
					in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_all='''+@Entry_ty+''' 
					and entry_ty+convert(Varchar(10),tran_cd)+acserial not in (Select entry_ty+convert(Varchar(10),tran_cd)+acserial from '+@DbName+'..NEWYEAR_ALLOC )'
				Exec Sp_ExecuteSql @SqlCmd
			End
			Else
			Begin
				set @SqlCmd=''
				set @SqlCmd =@SqlCmd +' If Not Exists(Select top 1 Tran_Cd From '+@DbName+'..NEWYEAR_ALLOC Where entry_all+convert(Varchar(10),Main_Tran )+acseri_all in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) )' 
				set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+'NEWYEAR_ALLOC ('+@fldlist+') Select '+replace(@fldlist,'[fcnew_all]','fcnew_all=0')+' from '+@TblName2+' Where entry_all+convert(Varchar(10),Main_Tran )+acseri_all in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) ' 
				Exec Sp_ExecuteSql @SqlCmd

				set @SqlCmd=''
				set @SqlCmd ='Insert Into '+@DbName+'..NEWYEAR_ALLOC  ('+@fldlist+') '
				set @SqlCmd =@SqlCmd +' Select '+@fldlist+' From 
					NewYear_Alloc Where entry_all+convert(Varchar(10),Main_tran)+acseri_all 
					in (Select entry_ty+convert(Varchar(10),Tran_cd)+acserial from #tmp4) and Entry_all='''+@Entry_ty+''' 
					and entry_ty+convert(Varchar(10),tran_cd)+acserial not in (Select entry_ty+convert(Varchar(10),tran_cd)+acserial from '+@DbName+'..NEWYEAR_ALLOC )'
				Exec Sp_ExecuteSql @SqlCmd
			End
		Fetch Next From InnerCursor Into @Entry_ty1 ,@Bcode_nm1,@Multi_cur1
	End
	Close InnerCursor 
	Deallocate InnerCursor 

	Fetch Next From Entry_Cursor Into @Entry_ty,@Bcode_nm,@Multi_cur 
End
Close Entry_Cursor 
Deallocate Entry_Cursor 

---------------------------			Inserting Records to the Table		--------------------------End

Drop Table #tmp4

Drop Table #tmp1
Drop Table #tmp2
Drop Table #ACMAST
Drop Table #Lac_vw
Drop Table #Mainall_vw
 
