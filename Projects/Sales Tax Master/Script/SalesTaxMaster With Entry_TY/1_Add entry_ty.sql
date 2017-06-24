if not exists( select * from syscolumns where [name]='entry_ty' and id in (select id from sysobjects where [name]='stax_mas') )
begin
	alter table stax_mas add entry_ty varchar(2) default '' with values
end
go
if not exists( select * from syscolumns where [name]='validity' and id in (select id from sysobjects where [name]='stax_mas') )
begin
	alter table stax_mas add validity varchar(250) default '' with values
end
