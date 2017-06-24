Create Procedure USP_ENT_GetOrderEntry
as
set Nocount On
select entry_ty Into #tmpEntry from Lcode Where 1=2

Declare @Entry_ty Varchar(2),@cnt Int
Declare RefCur Cursor for
Select Entry_ty From Lcode 
Open RefCur

Fetch Next From RefCur Into @Entry_ty
While @@Fetch_Status=0
Begin
	set @cnt =0
	Select @cnt=count(Entry_ty) From Lcode Where PickupFrom like '%'+@Entry_ty+'%' and Entry_ty Not In ('SR','PR')
	if @cnt >0 
		insert Into #tmpEntry values (@Entry_ty)
Fetch Next From RefCur Into @Entry_ty
End
Close RefCur 
Deallocate RefCur 

Select Entry_ty From Ordzm_vw_Item Where Entry_ty IN (Select Entry_ty from #tmpEntry)

Drop Table #tmpEntry
