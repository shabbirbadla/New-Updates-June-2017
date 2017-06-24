Create Procedure USP_ENT_UPDATE_ITBAL
AS 
select a.entry_ty,a.it_code,a.qty,tblnm = (case when b.bcode_nm != ' ' then b.bcode_nm else case when b.ext_vou = 1 then '' else b.entry_ty end end)+'QTY' into #temp1 from it_balw a,lcode b where a.entry_ty = b.entry_ty 
select a.it_code,a.tblnm,sum(a.qty) as qty into #temp2 from #temp1 a group by a.it_code,a.tblnm
delete from it_bal


Declare @SQLCOMMAND as NVARCHAR(4000)
SET @SQLCOMMAND = ' '+'insert into it_bal select it_code,it_name'
Declare @entries as varchar(10)
DECLARE openingentry_cursor CURSOR FOR
	select b.name from sysobjects a,syscolumns b where a.id = b.id and a.name = 'it_bal' and b.name not in ('it_code','it_name')
OPEN openingentry_cursor
FETCH NEXT FROM openingentry_cursor into @entries
WHILE @@FETCH_STATUS = 0
BEGIN
	Set @SQLCOMMAND = @SQLCOMMAND +',0 as '+@entries
	FETCH NEXT FROM openingentry_cursor into @entries
END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor
SET @SQLCOMMAND = @SQLCOMMAND+' '+'from it_mast'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

Declare @qty as numeric(25,5),@tblnm as varchar(50),@it_code as numeric(10)
DECLARE openingentry_cursor CURSOR FOR
	SELECT it_code,tblnm,qty FROM #temp2
OPEN openingentry_cursor
FETCH NEXT FROM openingentry_cursor into @it_code,@tblnm,@qty
WHILE @@FETCH_STATUS = 0
BEGIN
	Set @SQLCOMMAND = 'update it_bal set '+@tblnm+' = '+cast(@qty as varchar(20))+' where it_code = '+cast(@it_code as varchar(10))
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
   FETCH NEXT FROM openingentry_cursor into @it_code,@tblnm,@qty
END
CLOSE openingentry_cursor
DEALLOCATE openingentry_cursor

drop table #temp1
drop table #temp2


