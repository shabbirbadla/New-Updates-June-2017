If Exists(Select xType,Name from Sysobjects where xType='P' and Name='USP_REP_PO_VOU')

Begin
	Drop PROCEDURE USP_REP_PO_VOU
End
Go
/****** Object:  StoredProcedure [dbo].[USP_REP_PO_VOU]    Script Date: 08/14/2015 10:04:22 ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[USP_REP_PO_VOU]
@ENTRYCOND NVARCHAR(254)
	AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)	
SET @TBLCON=RTRIM(@ENTRYCOND)
Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20)

Select Entry_ty,Tran_cd=0,Date,inv_no,itserial=space(6) Into #pomain from pomain Where 1=0
set @sqlcommand='Insert Into #Pomain Select pomain.Entry_ty,pomain.Tran_cd,pomain.date,pomain.inv_no,poitem.itserial from pomain Inner Join poitem on (pomain.Entry_ty=poitem.Entry_ty and pomain.Tran_cd=poitem.Tran_cd) Where '+@TBLCON

print @sqlcommand
execute sp_executesql @sqlcommand
			
SELECT AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.PHONE,AC_MAST.FAX,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,
AC_MAST.ZIP,POMAIN.INV_NO,POMAIN.U_PINVNO,POMAIN.DATE,CONVERT(VARCHAR(254)  ,POMAIN.NARR) AS NARR,
'' AS U_BROKER,POMAIN.GRO_AMT AS V_GRO_AMT,POMAIN.EXAMT,POMAIN.U_CESSAMT,POMAIN.U_HCESAMT,POMAIN.TAX_NAME,
'' AS U_OCT,POMAIN.TAXAMT,POMAIN.TOT_DEDUC,POMAIN.TOT_ADD,POMAIN.TOT_TAX,POMAIN.TOT_NONTAX,POMAIN.NET_AMT,
'' AS U_OTHTRM,'' AS U_PAYTERMS,'' AS U_FREINTC,POITEM.ITEM_NO,POITEM.ITEM,CONVERT(VARCHAR(254),
POITEM.NARR) AS NARR,POITEM.QTY,POITEM.RATE,POITEM.GRO_AMT,IT_MAST.RATEUNIT,
It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'')='' THEN it_mast.it_name ELSE it_mast.it_alias END),
MailName=(CASE WHEN ISNULL(MailName,'')='' THEN ac_name ELSE mailname END),e.itemcnt,
PTERMSDESCRIPTION=isnull(substring(convert(varchar(4000),(Select '#'+CONVERT(varchar(10),row_number() over (order by tran_cd))+') '+ rtrim(PTDescrip) From PayTermsDet where PayTermsDet.Tran_cd =POMAIN.Tran_cd  AND PayTermsDet.entry_ty=POMAIN.entry_ty For XML Path(''))),2,4000),'')+ ISNULL(POMAIN.U_DELTERMS,'')
FROM POMAIN 
INNER JOIN POITEM ON POMAIN.TRAN_CD=POITEM.TRAN_CD 
INNER JOIN IT_MAST ON IT_MAST.IT_CODE=POITEM.IT_CODE 
INNER JOIN #pomain ON (POMAIN.TRAN_CD=#pomain.TRAN_CD and POMAIN.Entry_ty=#pomain.entry_ty and POITEM.ITSERIAL=#pomain.itserial) 
INNER Join (Select Entry_ty,Tran_cd,itemcnt=count(item_no) from POITEM group by Entry_ty,Tran_cd) e ON (POMAIN.Entry_ty=e.Entry_ty and POMAIN.Tran_cd=e.Tran_cd)
INNER JOIN AC_MAST ON POMAIN.AC_ID=AC_MAST.AC_ID AND POMAIN.ENTRY_TY='PO'
