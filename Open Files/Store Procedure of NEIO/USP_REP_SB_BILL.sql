if exists (select [name] from sysobjects where [name]='USP_REP_SB_BILL' AND XTYPE='P')
BEGIN 
DROP PROCEDURE USP_REP_SB_BILL
END
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 
-- Description:	This Stored procedure is useful to generate Service Tax Bill .
-- Modification Date/By/Reason: 30/07/2010 Rupesh Prajapati. TKT-794 GTA.
-- Modification Date/By/Reason: 22/11/2012 Satish Pal Bug-7086
-- Modification Date/By/Reason: Shrikant S. on 01-06-2016 for Bug-28132(Krishi Kalyan Cess)
-- Remark:
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_SB_BILL]
	@ENTRYCOND NVARCHAR(254)
	AS
begin	
	Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)
	
	select distinct code,[name] into #SERTAX_MAST from SERTAX_MAST
	/*--->Entry_Ty and Tran_Cd Separation*/
		declare @ent varchar(2),@trn int,@pos1 int,@pos2 int,@pos3 int--,@ENTRYCOND NVARCHAR(254)
		print @ENTRYCOND
/*-Start-->commented and Added by satish pal for bug-7086 date-22/11/2012-start*/
		----execute sp_executesql @SQLCOMMAND
		----set @pos1=charindex('''',@ENTRYCOND,1)+1
		----set @ent= substring(@ENTRYCOND,@pos1,2)
		----set @pos2=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos1))+1
		----set @pos3=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos2))+1
		----set @trn= substring(@ENTRYCOND,@pos2,@pos2-@pos3)
		----print 'ent '+ @ent
		SET @TBLCON=RTRIM(@ENTRYCOND)
		Select Entry_ty,Tran_cd=0 Into #sbmain from sbmain Where 1=0
		set @SQLCOMMAND='Insert Into #sbmain Select sbmain.Entry_ty,sbmain.Tran_cd from sbmain  Where '+@TBLCON
		execute sp_executesql @SQLCOMMAND
		--select * from bpmain where entry_ty=@ent and tran_cd=@trn
	/*<---Entry_Ty and Tran_Cd Separation*/
/*<-End--commented and Added by satish pal for bug-7086 date-22/11/2012-end*/
	
	SELECT m.INV_SR,m.TRAN_CD,m.ENTRY_TY,m.INV_NO,m.DATE
	,al.SERTY,al.sabtper,al.sabtamt,al.staxable,al.amount
	,SM.CODE,m.SERBPER,m.SERBAMT,m.SERCPER,m.SERCAMT,m.SERHPER,m.SERHAMT,m.DUE_DT,m.GRO_AMT GRO_AMT1,m.TAX_NAME,m.TAXAMT,m.NET_AMT
	,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX,AC_MAST.ECCNO,AC_MAST.SREGN
	--,SBITEM.U_PKNO,SBITEM.QTY,SBITEM.RATE,SBITEM.U_ASSEAMT,IT_MAST.IT_NAME  
	,m.skkcper,m.skkcamt,m.serbcess,m.serbcper
	FROM SBMAIN M
	INNER JOIN #sbmain ON (m.tran_cd=#sbmain.tran_cd)  ---Added by satish pal for bug-7086 date-22/11/2012
	INNER JOIN AC_MAST ON (AC_MAST.AC_ID=m.AC_ID) 
	Inner Join AcDetAlloc al on (m.entry_ty=al.entry_ty  and m.tran_cd=al.tran_cd)
	LEFT JOIN #SERTAX_MAST SM ON (SM.[NAME]=al.SERTY)  
	WHERE  m.ENTRY_TY in('SB','S1') --and m.tran_cd=@trn --commented by satish pal for bug-7086 date-22/11/2012
	ORDER BY m.INV_SR,CAST(m.INV_NO  AS INT)

end
