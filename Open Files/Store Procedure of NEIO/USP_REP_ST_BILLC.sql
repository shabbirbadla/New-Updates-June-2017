IF EXISTS (SELECT * FROM SYSOBJECTS WHERE XTYPE = 'P' AND [NAME] ='USP_REP_ST_BILLC' )
BEGIN
	DROP PROCEDURE USP_REP_ST_BILLC
END
go
-- =============================================
-- Author:		Shrikant S.
-- Create date: 10/01/2012
-- Description:	This Stored procedure is useful to generate Commercial Sales Invoice .
-- Modification Date/By/Reason: 
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_ST_BILLC]
	@ENTRYCOND NVARCHAR(254)
	AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)
Declare @chapno varchar(30),@eit_name  varchar(100)
SET @TBLCON=RTRIM(@ENTRYCOND)

Select Entry_ty,Tran_cd=0,Date,inv_no,itserial=space(6) Into #stmain from stmain Where 1=0
Create Clustered Index Idx_tmpStmain On #stmain (Entry_ty asc, Tran_cd Asc, Itserial asc)

set @sqlcommand='Insert Into #stmain Select stmain.Entry_ty,stmain.Tran_cd,stmain.date,stmain.inv_no,stitem.itserial from stmain Inner Join stitem on (stmain.Entry_ty=stitem.Entry_ty and stmain.Tran_cd=stitem.Tran_cd) Where '+@TBLCON
execute sp_executesql @sqlcommand


Declare @uom_desc as Varchar(100),@len int,@fld_nm varchar(10),@fld_desc Varchar(10),@count int,@stkl_qty Varchar(100)

select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name())
Create Table #qty_desc (fld_nm varchar(10)  COLLATE SQL_Latin1_General_CP1_CI_AS,fld_desc varchar(10)  COLLATE SQL_Latin1_General_CP1_CI_AS)
set @len=len(@uom_desc)
set @stkl_qty=''
If @len>0 
Begin
	while @len>0
	Begin
		set @fld_nm=substring(@uom_desc,1,charindex(':',@uom_desc)-1)
		set @uom_desc=substring(@uom_desc,charindex(':',@uom_desc)+1,@len)
		set @stkl_qty= @stkl_qty +', '+'STITEM.'+@fld_nm

		if @len>0 and charindex(';',@uom_desc)=0
		begin
			set @uom_desc=@uom_desc
			set @fld_desc=@uom_desc
			SET @len=0
		End
		else
		begin
				set @fld_desc=substring(@uom_desc,1,charindex(';',@uom_desc)-1)
				set @uom_desc=substring(@uom_desc,charindex(';',@uom_desc)+1,@len)
				set @len=len(@uom_desc)
		End
		insert into #qty_desc values (@fld_nm,@fld_desc)
	End
End
Else
Begin
	set @stkl_qty=',STITEM.QTY'
End

set @sqlcommand='SELECT STMAIN.INV_SR,''REPORT HEADER'' AS REP_HEAD,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE,STMAIN.U_TIMEP,STMAIN.U_TIMEP1'
set @sqlcommand=@sqlcommand+' '+',STMAIN.U_REMOVDT,STMAIN.U_EXPLA,STMAIN.U_EXRG23II,STMAIN.U_RG2AMT,STMAIN.EXAMT,STITEM.U_BASDUTY,STITEM.U_CESSPER,STMAIN.U_CESSAMT,STITEM.U_HCESSPER'
set @sqlcommand=@sqlcommand+' '+',STMAIN.U_HCESAMT,STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,U_CHALNO,U_CHALDT,STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI'
set @sqlcommand=@sqlcommand+' '+',STMAIN.U_VEHNO,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STMAIN.TAXAMT,STMAIN.NET_AMT,STMAIN.U_PLASR,STMAIN.U_RG23NO,STMAIN.U_RG23CNO,STITEM.U_PKNO'
set @sqlcommand=@sqlcommand+' '+@stkl_qty+',STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE ,IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT '
set @sqlcommand=@sqlcommand+' '+',AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX,AC_MAST.ECCNO ,AC_MAST1.ADD1 ADD11'
set @sqlcommand=@sqlcommand+' '+',AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1,AC_MAST1.ZIP ZIP1,AC_MAST1.S_TAX S_TAX1,AC_MAST1.I_TAX I_TAX1,AC_MAST1.ECCNO ECCNO1'
set @sqlcommand=@sqlcommand+' '+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
set @sqlcommand=@sqlcommand+' '+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'
set @sqlcommand=@sqlcommand+' '+',stmain.tds_tp,stmain.sc_tp,stmain.ec_tp,stmain.hc_tp,stmain.tcsamt,stmain.stcsamt,stmain.etcsamt,stmain.htcsamt  '
set @sqlcommand=@sqlcommand+' '+'FROM STMAIN '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD) '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN #stmain ON (STITEM.TRAN_CD=#stmain.TRAN_CD and STITEM.Entry_ty=#stmain.entry_ty and STITEM.ITSERIAL=#stmain.itserial) '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE)' 
set @sqlcommand=@sqlcommand+' '+'INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID) '
set @sqlcommand=@sqlcommand+' '+'LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=STMAIN.U_DELIVER) '
set @sqlcommand=@sqlcommand+' '+'ORDER BY STMAIN.INV_SR,STMAIN.INV_NO  ,CAST(STITEM.ITSERIAL AS INT)'
execute sp_executesql @sqlcommand	



