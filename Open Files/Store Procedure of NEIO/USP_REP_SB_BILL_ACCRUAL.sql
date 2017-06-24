if exists (select [name] from sysobjects where [name]='USP_REP_SB_BILL_ACCRUAL' AND XTYPE='P')
BEGIN 
DROP PROCEDURE USP_REP_SB_BILL_ACCRUAL
END
go
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 
-- Description:	This Stored procedure is useful to generate Service Tax Bill .
-- Modification Date/By/Reason: 30/07/2010 Rupesh Prajapati. TKT-794 GTA.
-- Remark:
-- Modification Date/By/Reason: 05/09/2012 satish pal bug-6221.
-- Modification Date/By/Reason: Shrikant S. on 01-06-2016 for Bug-28132(Krishi Kalyan Cess)
-- =============================================
Create  PROCEDURE [dbo].[USP_REP_SB_BILL_ACCRUAL]
	@ENTRYCOND NVARCHAR(254)
	AS
begin	
	Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)
	
	select distinct code,[name] into #SERTAX_MAST from SERTAX_MAST
	/*--->Entry_Ty and Tran_Cd Separation*/
	---Commented and added by satish pal for bug-6221 dt.05/09/2012--start
		----declare @ent varchar(2),@trn int,@pos1 int,@pos2 int,@pos3 int--,@ENTRYCOND NVARCHAR(254)
		----print @ENTRYCOND
		----set @pos1=charindex('''',@ENTRYCOND,1)+1
		----set @ent= substring(@ENTRYCOND,@pos1,2)
		----set @pos2=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos1))+1
		----set @pos3=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos2))+1
		----set @trn= substring(@ENTRYCOND,@pos2,@pos2-@pos3)
		----print 'ent '+ @ent
		----print @trn
		SET @TBLCON=RTRIM(@ENTRYCOND)	
		Select Entry_ty,Tran_cd=0 Into #sbmain from sbmain Where 1=0
		set @SQLCOMMAND='Insert Into #sbmain Select sbmain.Entry_ty,sbmain.Tran_cd from sbmain Inner Join sbitem on (sbmain.Entry_ty=sbitem.Entry_ty and sbmain.Tran_cd=sbitem.Tran_cd) Where '+@TBLCON
		PRINT @SQLCOMMAND
		execute sp_executesql @SQLCOMMAND
	---Commented and added by satish pal  for bug-6221 dt.05/09/2012--end	
		--select * from bpmain where entry_ty=@ent and tran_cd=@trn
	/*<---Entry_Ty and Tran_Cd Separation*/
	
	SELECT m.INV_SR,m.TRAN_CD,m.ENTRY_TY,m.INV_NO,m.DATE
	,al.SERTY,al.sabtper,al.sabtamt,al.staxable,al.amount
	,SM.CODE,SBITEM.SERBPER,m.SERBAMT,SBITEM.SERCPER,m.SERCAMT,SBITEM.SERHPER,m.SERHAMT,m.DUE_DT,m.GRO_AMT GRO_AMT1,m.TAX_NAME,m.TAXAMT,m.NET_AMT
	,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX,AC_MAST.ECCNO,AC_MAST.SREGN
	,serbcper=case when SBITEM.serbcess> 0 then SBITEM.serbcper else 0 end ,SBITEM.serbcess,sbitem.ITEM			--Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess
	,SKKCPER=case when SBITEM.SKKCAMT> 0 then SBITEM.SKKCPER else 0 end ,m.skkcamt
	--,SBITEM.U_PKNO,SBITEM.QTY,SBITEM.RATE,SBITEM.U_ASSEAMT,IT_MAST.IT_NAME  
	FROM SBMAIN M 
	INNER JOIN SbITEM ON (m.Tran_cd=SBITEM.Tran_cd ) --Added by satish pal  for bug-6221 dt.05/09/2012
	INNER JOIN #sbmain ON (SbITEM.TRAN_CD=#sbmain.TRAN_CD and SbITEM.Entry_ty=#sbmain.entry_ty ) --Added by satish pal  for bug-6221 dt.05/09/2012
	INNER JOIN AC_MAST ON (AC_MAST.AC_ID=m.AC_ID) 
	Inner Join AcDetAlloc al on (m.entry_ty=al.entry_ty  and m.tran_cd=al.tran_cd)
	LEFT JOIN #SERTAX_MAST SM ON (SM.[NAME]=al.SERTY)  
	WHERE  m.ENTRY_TY in('SB','S1') --and m.tran_cd=@trn --commented by satish pal  for bug-6221 dt.05/09/2012
	ORDER BY m.INV_SR,CAST(m.INV_NO  AS INT)

end
