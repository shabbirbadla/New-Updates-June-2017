If Exists (Select [Name] From SysObjects Where xtype='P' and [Name]='Get_MRP_PendingData')
Begin
	Drop Procedure Get_MRP_PendingData
End
Go
Create Procedure Get_MRP_PendingData
@EntryType Varchar(2),@Sdate SmallDateTime,@Edate SmallDateTime,@DateOn Varchar(10),@WareHouse Varchar(100)
as 
Declare @EntryCode Varchar(2),@Validity Varchar(250),@replVal Varchar(5),@SqlCmd Nvarchar(4000),@TBLNM VARCHAR(10) 
Declare @Due_dt Varchar(20),@lotherfield Varchar(250)
set @lotherfield=''

--Select  @EntryCode=Case when Bcode_nm<>'' then BCode_nm Else Entry_ty End,@Validity=MRPValidity From LCODE where Entry_ty=@EntryType
Select  @EntryCode=Case when Bcode_nm<>'' then BCode_nm Else (case when ext_vou=1 then '' else Entry_ty End )End From LCODE where Entry_ty=@EntryType
--set @replVal=char(39)+','+CHAR(39)
--SET @Validity=REPLACE(@Validity,' ',@replVal)
--SET @Validity='('+CHAR(39)+@Validity+char(39)+')'
set @TBLNM=@EntryCode+'MAIN'

print @TBLNM
If EXISTS(SELECT B.[NAME] FROM SYSOBJECTS A INNER JOIN SYSCOLUMNS B ON (B.ID=A.ID) WHERE B.[NAME]='Due_dt' AND A.[NAME]=@TBLNM)
BEGIN 
	SET @Due_dt=' b.Due_dt '	
END
ELSE
BEGIN
	SET @Due_dt=' 0 as Due_dt '
END

--Select @lotherfield=@lotherfield+isnull(substring((Select ',b.' +fld_nm From LOTHER Where E_code =@EntryType  For XML Path('')),1,250),'')
--Print @lotherfield

set @SqlCmd='Select Sel=cast(0 as bit),b.Inv_no,b.Date,'+@Due_dt+',a.item,a.qty,a.Qty as AdjustQty,a.ware_nm,a.It_code '--+@lotherfield
set @SqlCmd=@SqlCmd+' '+',a.Entry_ty,a.Tran_cd,a.itserial'
set @SqlCmd=@SqlCmd+' '+'From '+@EntryCode+'main b'
set @SqlCmd=@SqlCmd+' '+'Inner Join '+@EntryCode+'Item a on (a.Tran_cd=b.Tran_cd)'
set @SqlCmd=@SqlCmd+' '+'Where a.QTY<>a.RE_QTY  and (b.'+@DateOn+' Between '''+Convert(Varchar(50),@Sdate)+''' and '''+Convert(Varchar(50),@Edate)+''') '
set @SqlCmd=@SqlCmd+' '+' and b.Entry_ty= '''+@EntryType+''' '
--set @SqlCmd=@SqlCmd+' '+' and a.Entry_ty+Convert(Varchar(10),a.Tran_cd)+a.Itserial Not in (Select Entry_ty+Convert(Varchar(10),Tran_cd)+Itserial From MRPLog) '
set @SqlCmd=@SqlCmd+' '+ Case When @WareHouse<>'' then ' and a.ware_nm='''+@WareHouse+''' ' Else '' End
Print @SqlCmd
Execute Sp_ExecuteSql @SqlCmd



--Select Sel=cast(0 as bit),b.inv_no,a.date,b.due_dt,a.item,a.qty,b.u_custpo,a.ware_nm 
--from somain b 
--left outer join soitem a on(a.tran_cd=b.tran_cd) 
--where a.item+a.inv_no not in (select item+orderno from mrplog) and a.DATE >=  '" + frm + "' and a.date <= '" + todt1 + "' 
--AND a.QTY<>a.RE_QTY  and a.entry_ty in ('" + mrp4 + "') and ware_nm='" + cmbWareHouse.Text + "' order by a.inv_no 

--Execute Get_MRP_PendingData 'SO','04/01/2013','03/31/2014','Due_dt',''