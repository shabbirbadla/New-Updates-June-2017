create procedure USP_ENT_GetContraints
@tbl Varchar(100)
as
Create Table #tmpContraints (cType tinyint,cName Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS
			,cFlags int,cColCount int,cFillFactor tinyint
			,cRefTable Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefKey Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol1 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol2 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol3 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol4 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol5 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol6 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol7 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol8 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol9 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol10 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol11 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol12 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol13 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol14 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cKeyCol15 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cKeyCol16 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol1 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol2 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol3 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol4 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol5 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol6 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol7 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol8 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol9 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol10 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol11 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol12 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol13 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol14 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cRefCol15 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,cRefCol16 Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cIndexID int,
			cGroupName Varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
			cDisabled int,
			cPrimaryFG int,
			cDeleteCascade int,
			cUpdateCascade int,
			desending Int 
			)

insert Into #tmpContraints exec sp_MStablekeys @tbl,null,14

Select * from #tmpContraints 

Drop Table #tmpContraints
