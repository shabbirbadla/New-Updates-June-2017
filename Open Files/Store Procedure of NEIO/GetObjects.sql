If exists(Select [name] from sysobjects where xtype='P' and [name]='GetObjects')
begin
	Drop procedure GetObjects
End
Go
create procedure GetObjects
as
Create Table #tmpObjects (oType int, oObjName Varchar(75),oOwner Varchar(10),oSequence int)

insert Into #tmpObjects Execute sys.sp_MSdependencies

Delete from #tmpObjects 
Where oObjname in ('usp_crystal_balancesheet','usp_crystal_h_balancesheet','usp_crystal_trading_profitandloss' 
	,'usp_ent_gen_no','usp_lmain_view','usp_rep_b1bond','usp_ent_holiday_master','usp_rep_emp_leave_year_master'
	,'usp_rep_fixed_asset','usp_dts_ostablecreations')
Select * from #tmpObjects Order By (case when oType =8 then 'a' else (case when oType =1 then 'b' else (case when oType =4 then 'c' else (case when oType =16 then 'd' else 'e' end) end)end)end),oSequence,oObjName

Drop Table #tmpObjects 
