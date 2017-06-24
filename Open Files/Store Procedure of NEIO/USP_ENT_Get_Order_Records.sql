create Procedure [dbo].[USP_ENT_Get_Order_Records]
@DbName Varchar(10),@Edate SmallDateTime 
as
set Nocount On
Select Ac_Name,Ac_Id Into #ACMAST From Ac_Mast Where Type='B'and Not (Typ  in ('EXCISE') OR AC_NAME Like '%CAPITAL GOODS PAYABLE%')  

select entry_ty Into #tmpEntry from Lcode Where 1=2

Declare @Entry_ty Varchar(2),@cnt Int
Declare RefCur Cursor for
Select Entry_ty From Lcode 
Open RefCur

Fetch Next From RefCur Into @Entry_ty
While @@Fetch_Status=0
Begin
	set @cnt =0
	Select @cnt=count(Entry_ty) From Lcode Where PickupFrom like '%'+@Entry_ty+'%' and Entry_ty Not In ('SR','PR','PT','ST','IL','LI','LR','RL','WK','IP','OP','AR','DC') AND v_item=1
	if @cnt >0 
		insert Into #tmpEntry values (@Entry_ty)
Fetch Next From RefCur Into @Entry_ty
End
Close RefCur 
Deallocate RefCur 


select Itref_tran,rentry_ty,ritserial,rqty=sum(rqty) Into #ulitref from uLitref group by Itref_tran,rentry_ty,ritserial

Select a.Entry_ty,a.Tran_cd,a.Date,a.It_code,a.Itserial,a.Qty,re_Qty=sum(isnull(b.rqty,0))
Into #tmp5 From Litem_Vw a
Inner join [Lmain_vw ] c on (a.Tran_cd=c.Tran_cd and a.entry_ty=c.entry_ty)
Inner Join #ACMAST ac On (ac.ac_id=a.ac_id)
Left Join #ulitref b On (b.Itref_tran = a.Tran_Cd and b.rentry_ty=a.Entry_ty and b.ritserial=a.itserial)
Where a.qty<>isnull(b.rqty,0) 
--and a.Entry_ty Not In ('IL','LI','LR','RL','WK','IP','OP')
and a.entry_ty In (Select Entry_ty From #tmpEntry)
and c.[rule] not in ('EXCISE','NON-EXCISE')
Group By a.Entry_ty,a.Tran_cd,a.Date,a.It_code,a.Itserial,a.Qty

Select * Into #tmp6 from #tmp5 Where Qty<>Re_qty

Select Entry_ty,Bcode_nm Into #lcode From LCODE where 1=2

Declare @IsTrading Bit
set @IsTrading =0
If Exists(Select [Name] From SysObjects Where [Name]='Litemall')
Begin
	set @IsTrading=1
	Insert Into #lcode Select Entry_ty,Bcode_nm From LCODE Where Entry_ty Not in ('AR','DC')
End
else
Begin
	Insert Into #lcode Select Entry_ty,Bcode_nm From LCODE 
End

Declare @Bcode_nm Varchar(2),@Entry_Table Varchar(2),@TblName Varchar(20),@fldlist Varchar(4000),@SqlCmd NVarchar(MAX),@SqlCmd2 NVarchar(MAX)
---------------------------			Inserting Records to the Table		--------------------------Start
Begin Try
Begin Transaction

Declare Entry_Cursor Cursor for
Select Distinct a.Entry_ty, b.bcode_nm from #tmp6 a Inner Join #lcode b ON (a.Entry_ty=b.Entry_ty) 
Open Entry_Cursor 

Fetch Next From Entry_Cursor Into @Entry_ty,@Bcode_nm 
While @@Fetch_Status=0
Begin
	set @Entry_Table=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_ty End
	
	set @TblName=@Entry_Table+'Item'
	set @SqlCmd ='Delete from '+@DbName+'..'+@TblName+' Where Entry_ty+Convert(Varchar(10),tran_cd)+Itserial Not In (Select Entry_ty+Convert(Varchar(10),tran_cd)+Itserial From '+@DbName+'..uLitref ) and date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd

	set @TblName=@Entry_Table+'Main'
	set @SqlCmd ='Delete from '+@DbName+'..'+@TblName+' Where Entry_ty+Convert(Varchar(10),tran_cd) Not In (Select Entry_ty+Convert(Varchar(10),tran_cd) From '+@DbName+'..uLitref Union All select Entry_ty+Convert(Varchar(10),tran_cd) From '+@DbName+'..'+@Entry_Table+'Acdet ) and date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd

	set @TblName='NEWYEAR_Itref'
	set @SqlCmd ='Delete from '+@DbName+'..'+@TblName+' Where Entry_ty+Convert(Varchar(10),tran_cd)+Itserial Not In (Select Entry_ty+Convert(Varchar(10),tran_cd)+Itserial From '+@DbName+'..uLitref ) and date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd

	set @TblName=@Entry_Table+'Main'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 

	set @SqlCmd =' Set Identity_Insert '+@DbName+'..'+@TblName+' On'
	set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+') Select '+@fldlist+' From '+@TblName+' Where Entry_ty+Convert(Varchar(10),Tran_cd) In (Select distinct Entry_ty+Convert(Varchar(10),Tran_cd) From #tmp6 Where Entry_ty='''+@Entry_ty+''' )  and Entry_ty='''+@Entry_ty+''' and Entry_ty+Convert(Varchar(10),Tran_cd) Not In (Select Entry_ty+Convert(Varchar(10),Tran_cd) From '+@DbName+'..'+@TblName+')'
	set @SqlCmd =@SqlCmd +' Set Identity_Insert '+@DbName+'..'+@TblName+' Off'
--	print @SqlCmd
	Execute sp_Executesql @SqlCmd

	set @TblName=@Entry_Table+'Item'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 

	set @SqlCmd =''
	set @SqlCmd =@SqlCmd +' Insert Into '+@DbName+'..'+@TblName+' ('+@fldlist+')  Select '+@fldlist+' From '+@TblName+' Where entry_ty+convert(Varchar(10),Tran_cd )+itserial In (Select entry_ty+convert(Varchar(10),Tran_cd )+itserial From #tmp6 Where Entry_ty='''+@Entry_ty+''')  and Entry_ty='''+@Entry_ty+''' and  entry_ty+convert(Varchar(10),Tran_cd )+itserial Not in (Select entry_ty+convert(Varchar(10),Tran_cd )+itserial From '+@DbName+'..'+@TblName+')'
	Print @SqlCmd
	Execute sp_Executesql @SqlCmd

	set @SqlCmd =' Update '+@DbName+'..'+@TblName+' set re_qty=a.re_Qty,dc_no=''YE'' From #tmp6 a Inner Join '+@DbName+'..'+@TblName+' b on (a.Entry_ty=b.Entry_ty and a.Tran_Cd=b.Tran_cd and a.Itserial=b.Itserial and a.Entry_ty='''+@Entry_ty+''')'
	set @SqlCmd =@SqlCmd+' Where b.Entry_ty+Convert(Varchar(10),b.tran_cd)+b.Itserial Not In (Select Entry_ty+Convert(Varchar(10),tran_cd)+Itserial From '+@DbName+'..'+@TblName+') and b.date<='''+CONVERT(Varchar(50),@Edate)+''' '
	Execute sp_Executesql @SqlCmd

	set @TblName='NEWYEAR_Itref'
	set @SqlCmd =''
	set @SqlCmd =' Insert Into '+@DbName+'..'+@TblName+'(entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn)  
	Select a.entry_ty,a.date,a.doc_no,a.rentry_ty,a.rqty,a.item,a.ware_nm,a.Itref_tran,a.Tran_cd,a.Itserial,a.Rdate,a.Rdoc_no
	,a.Ritserial,a.It_code,a.CompId,a.rinv_sr,a.rinv_no,a.rl_yn 
	From uLITREF a Inner Join #tmp6 b on (a.rEntry_ty=b.Entry_ty and a.itref_Tran=b.Tran_cd and a.rItserial=b.Itserial ) 
	Where a.entry_ty+convert(Varchar(10),a.Tran_cd)+a.Itserial not in (Select entry_ty+convert(Varchar(10),Tran_cd)+Itserial From '+@DbName+'..'+@TblName+')'	
	Execute sp_Executesql @SqlCmd


	Fetch Next From Entry_Cursor Into @Entry_ty,@Bcode_nm 
End
Close Entry_Cursor 
Deallocate Entry_Cursor 
	commit Transaction
	Select 1 as Ans
End Try
Begin Catch
	Close Entry_Cursor 
	Deallocate Entry_Cursor
	Rollback Transaction
	Select 0 as Ans,convert(Varchar(8000),ERROR_MESSAGE()) AS ErrorMsg
End Catch

---------------------------			Inserting Records to the Table		--------------------------End
Drop Table #lcode

--Drop Table #ACMAST
--Drop Table #tmpEntry
