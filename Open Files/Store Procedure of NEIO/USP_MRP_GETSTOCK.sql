If exists (select [name] from sysobjects where [name]='USP_MRP_GETSTOCK' and xtype='P')
drop procedure USP_MRP_GETSTOCK
GO

Create Procedure USP_MRP_GETSTOCK
@It_code Numeric(10),@Spl_Cond Varchar(1000)
AS
set nocount on
Declare @Entry_Ty Varchar(2),@Bcode_nm Varchar(2),@EntryTbl Varchar(2),@SqlCmd NVarchar(4000)
/*		Retrieving Stock Entries	 Start		*/
select i.It_code,SQtym=i.Qty,SQty=i.Qty,SQtyLabr=i.Qty into #GetStock  from STITEM i  where 1=2 

Declare GetStock Cursor for
 Select Entry_ty,Bcode_nm From Lcode Where Inv_stk<>''

Open GetStock
Fetch Next From GetStock Into @Entry_Ty,@Bcode_nm
While @@FETCH_STATUS=0 
Begin
	Select @EntryTbl=Case When @Bcode_nm<>'' Then @Bcode_nm Else @Entry_Ty End
	Select @SqlCmd ='Insert Into #GetStock  '
	Select @SqlCmd =@SqlCmd+' '+'Select i.It_Code,sqtym=Case When m.[Rule] In (''MODVATABLE'',''ANNEXURE IV'',''CAPTIVE USE'') Then (Case When i.PmKey=''+'' then Qty else -Qty End ) Else 0 End '
	Select @SqlCmd =@SqlCmd+' '+',sqty=Case When m.[Rule] Not In (''MODVATABLE'',''ANNEXURE IV'',''CAPTIVE USE'') Then (Case When i.PmKey=''+'' then Qty else -Qty End ) Else 0 End '
	Select @SqlCmd =@SqlCmd+' '+',sqtyLabr=Case When m.[Rule] In (''ANNEXURE V'') Then (Case When i.PmKey=''+'' then Qty else -Qty End ) Else 0 End'
	Select @SqlCmd =@SqlCmd+' '+'From '+@EntryTbl+'Item i Inner Join '+@EntryTbl+'Main m On (m.Tran_cd=i.Tran_cd)'
	Select @SqlCmd =@SqlCmd+' '+'Where m.Entry_ty='''+@Entry_Ty+''' and i.It_code='+CONVERT(varchar(10),@It_code)
	Select @SqlCmd =@SqlCmd+' '+'and i.Dc_no='''' and i.pmkey<>'''' '
	Select @SqlCmd =@SqlCmd+' '+Case When @Spl_Cond <>'' Then @Spl_Cond Else '' End
	Execute Sp_ExecuteSql @SqlCmd 
	--print @SqlCmd
	Fetch Next From GetStock Into @Entry_Ty,@Bcode_nm
End
Close GetStock
Deallocate GetStock

Select It_Code,SQtym=Sum(SQtym),SQty=Sum(SQty),SQtyLabr=Sum(SQtyLabr) Into #StockBal From #GetStock Group By It_Code

Select * From #StockBal
/*		Retrieving Stock Entries	 End		*/
