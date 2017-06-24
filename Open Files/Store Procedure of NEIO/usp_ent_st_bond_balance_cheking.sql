-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 16/05/2007
-- Description:	This SP is useful for online balance checking for Bond Account in Sales Entry.
-- Modification Date/By/Reason: 13/09/2010 / Birendra / EOU 
-- Remark: 
-- =============================================
if exists (select [name] from sysobjects where [name]='usp_ent_st_bond_balance_cheking' AND XTYPE='P')
BEGIN 
DROP PROCEDURE usp_ent_st_bond_balance_cheking
END
go

Create procedure [dbo].[usp_ent_st_bond_balance_cheking]
@bondno varchar(60),@Tran_cd int,@inv_no int,@inv_sr varchar(30),@rule varchar(30) 
--,@rule varchar(30) Added by birendra on 13 sept 2010 for EOU B17 Bond
as
begin
	select bond_no
	,balamt=isnull((case when amt_ty='DR' then amount else -amount end),0)
	into #bondno1 
	from stmain m  
	inner join stacdet ac on (m.tran_cd=ac.tran_cd)
	inner  join ac_mast a on (a.ac_id=ac.ac_id)
	where bond_no=@bondno and a.ac_name like '%bond%' and m.tran_cd<>@tran_cd and cast(m.inv_no as int)<@inv_no and m.inv_sr=@inv_sr
	union
	select bond_no ,balamt=isnull((case when amt_ty='DR' then amount else -amount end),0)
	from BPmain m 
	inner join bpacdet ac on (m.tran_cd=ac.tran_cd)
	inner  join ac_mast a on (a.ac_id=ac.ac_id)
	where bond_no=@bondno and a.ac_name like '%bond%'
	union
	select bond_no ,balamt=isnull((case when amt_ty='DR' then amount else -amount end),0)
	from BRmain m 
	inner join bracdet ac on (m.tran_cd=ac.tran_cd)
	inner  join ac_mast a on (a.ac_id=ac.ac_id)
	where bond_no=@bondno and a.ac_name like '%bond%'
	union
	select bond_no ,balamt=isnull((case when amt_ty='DR' then amount else -amount end),0)
	from obmain m 
	inner join obacdet ac on(m.tran_cd=ac.tran_cd)
	inner  join ac_mast a on (a.ac_id=ac.ac_id)
	where bond_no=@bondno and a.ac_name like '%bond%' and m.[RULE] = @rule
	--'and m.[RULE] = @rule' Added by birendra on 13 sept 2010 for EOU B17 Bond

	union
	select bond_no
	,balamt=isnull(-u_exbamt,0)
	from stmain m  
	where bond_no=@bondno and m.tran_cd<>@tran_cd and cast(m.inv_no as int)<@inv_no and m.inv_sr=@inv_sr
	

	select balamt=sum(balamt) into #bondno from #bondno1 
	update #bondno  set balamt=isnull(balamt,0)
	select balamt from #bondno
end	






