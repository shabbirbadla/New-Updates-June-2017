If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_OP_VOUBARCODE'))
Begin
	Drop Procedure USP_REP_OP_VOUBARCODE
End

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ============================================================================
-- Author      : Vasant M. S.
-- Create date : 28/03/2013
-- Description : Generate Barcode Label in OP Transaction
-- ============================================================================

CREATE PROCEDURE [dbo].[USP_REP_OP_VOUBARCODE]
	@ENTRYCOND NVARCHAR(254)
	AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50)
Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
				+ (DATEPART(ss, GETDATE()) * 1000 )
				+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
Set @TBLNAME1 = '##TMP1'+@TBLNM
Set @TBLNAME2 = '##TMP2'+@TBLNM


Select @sqlcommand='SELECT OPMAIN.INV_SR,OPMAIN.ENTRY_TY,OPMAIN.TRAN_CD,OPMAIN.INV_NO,OPMAIN.DATE,OPITEM.QTY,OPITEM.RATE,OPITEM.GRO_AMT,OPITEM.ITSERIAL
	,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END),IT_MAST.RATEUNIT,IT_MAST.BC_NO1
	Into '+@TBLNAME1
	+' FROM OPMAIN 
	INNER JOIN OPITEM ON (OPMAIN.TRAN_CD=OPITEM.TRAN_CD)         
	INNER JOIN IT_MAST ON (OPITEM.IT_CODE=IT_MAST.IT_CODE)
	WHERE '+@ENTRYCOND
	+' ORDER BY OPMAIN.INV_SR,CAST(OPMAIN.INV_NO  AS INT),CAST(OPITEM.ITSERIAL AS INT)'
execute sp_executesql @sqlcommand	

Select @sqlcommand='SELECT * Into '+@TBLNAME2+' FROM '+@TBLNAME1+' where 1 = 2'
execute sp_executesql @sqlcommand	

Select @sqlcommand='Declare @ItSerial Varchar(10),@Qty NUMERIC(20) 
DECLARE CurOP CURSOR FOR SELECT ItSerial,Qty From '+@TBLNAME1+' 
OPEN CurOP
FETCH NEXT FROM CurOP INTO @ItSerial,@Qty
WHILE @@FETCH_STATUS=0
BEGIN
	WHILE @Qty > 0
	BEGIN
		Insert Into '+@TBLNAME2+' Select * From '+@TBLNAME1+' Where ItSerial = @ItSerial
		Set @Qty = @Qty - 1
	End	
	FETCH NEXT FROM CurOP INTO @ItSerial,@Qty
END
CLOSE CurOP
DEALLOCATE CurOP'
execute sp_executesql @sqlcommand	

set @sqlcommand='SELECT * FROM '+@TBLNAME2
execute sp_executesql @sqlcommand	

set @sqlcommand='Drop Table '+@TBLNAME1
execute sp_executesql @sqlcommand	

set @sqlcommand='Drop Table '+@TBLNAME2
execute sp_executesql @sqlcommand	

GO



