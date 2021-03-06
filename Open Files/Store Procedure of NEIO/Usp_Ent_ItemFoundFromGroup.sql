set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 
-- Description:	This Stored procedure is useful in Item Selection in uereport.app with it_group selection.
-- Modified By: 
-- Modify By/Date/Reason: Rupesh Prajapati. 26/06/2008 to repalce charindex() because, it was not not filtering perfectly.
-- Modify By/Date/Reason: Rupesh Prajapati. 19/02/2010 to Add It_Desc Column. TKT-110.
-- Remark:
-- =============================================
ALTER procedure [dbo].[Usp_Ent_ItemFoundFromGroup]
@itGroup varchar(50)
as
declare @grop varchar(50),@grp varchar(50),@mgrp varchar(50),@itm varchar(50),@It_Desc varchar(200)
set @grp=''
set @grop=''
set @mgrp=''
set @itm=''
select it_name as item,[group],It_Desc=(case when isnull(it_alias,'')='' then it_name else it_alias end) into #items from it_mast where 1=2
declare @@sqlqry cursor
exec prcItmGroup @itgroup,@@curtmp = @@sqlqry output
fetch next from @@sqlqry into @grp,@mgrp
while @@fetch_status=0
begin
	declare curItMast cursor for
	select it_name,[group],It_Desc=(case when isnull(it_alias,'')='' then it_name else it_alias end) from it_mast where [group]=@grp
	open curItMast
	fetch next from curItMast into @itm,@grop,@It_Desc
	while @@fetch_status=0
	begin
		insert into #items values(@itm,@grop,@It_Desc)
		fetch next from curItMast into @itm,@grop,@It_Desc
	end
	close curItMast
	deallocate curItMast
	fetch next from @@sqlqry into @grp,@mgrp
end
close @@sqlqry
deallocate @@sqlqry
select item,[group],It_Desc from #items
drop table #items

