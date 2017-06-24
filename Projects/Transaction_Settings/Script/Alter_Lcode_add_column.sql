if not exists( select * from syscolumns where [name]='stax_tran' and id in (select id from sysobjects where [name]='lcode') )
begin
	alter table lcode add stax_tran bit default 0 with values
end

if not exists( select * from syscolumns where [name]='stax_item' and id in (select id from sysobjects where [name]='lcode') )
begin
	alter table lcode add stax_item bit default 0 with values
end
if not exists( select * from syscolumns where [name]='stax_round' and id in (select id from sysobjects where [name]='lcode') )
begin
	alter table lcode add stax_round bit default 0 with values
end


update lcode set stax_item=1 where (entry_ty in ('PT','ST') or bcode_nm in ('PT','ST'))

select * from lcode
--set grdset
--advset
--set data grd
