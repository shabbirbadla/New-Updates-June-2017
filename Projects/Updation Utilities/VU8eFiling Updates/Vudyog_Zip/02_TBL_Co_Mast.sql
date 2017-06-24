if not exists(select b.name from sysobjects a,syscolumns b where a.id = b.id and a.name = 'co_mast' and b.name = 'city')
alter table co_mast add city varchar(22) default ''

if not exists(select b.name from sysobjects a,syscolumns b where a.id = b.id and a.name = 'co_mast' and b.name = 'zip')
alter table co_mast add zip varchar(7) default ''

