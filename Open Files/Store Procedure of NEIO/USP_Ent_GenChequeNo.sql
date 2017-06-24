if exists (select [name] from sysobjects where [name]='USP_Ent_BalChequeNo' AND XTYPE='P')
BEGIN 
	DROP PROCEDURE USP_Ent_BalChequeNo
END
GO
/****** Object:  StoredProcedure [dbo].[USP_REP_OUTSTANDINGCR_AGEING]    Script Date: 04/28/2016 10:16:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modified By: Kishor Agarwal
-- Modify date: 28/04/2016
-- =============================================
CREATE PROCEDURE [dbo].[USP_Ent_BalChequeNo]  
@ACID Numeric(5),@BankNam varchar(100)
AS

Declare @SQLCOMMAND as NVARCHAR(4000)
Select ChequeNos= Startno INTO TblChequeNos from Chequemaster where 1=2
DECLARE @CheqCnt Int,@LastChequeNo Numeric(7),@Count Numeric(4),@LastNo Numeric(7)
set @Count =0

DECLARE Temp CURSOR for
select LastCheqNo=(CASE WHEN LastCheqNo=0 Then StartNo Else LastCheqNo End),CheqCnt,LastCheqNo AS LastNo
from(select *,(Case when LastCheqNo=0 then leafletcnt else cast(EndNo as numeric)-LastCheqNo End) as CheqCnt 
from Chequemaster 
WHERE AC_ID = @ACID AND  BankNm = @BankNam And Isnull(LastCheqNo,0) < Isnull(Endno,0) 
)A where CheqCnt >0 order by Srno 
Open Temp
fetch next from Temp Into @LastChequeNo,@CheqCnt,@LastNo
while @@FETCH_STATUS =0
Begin	
	while @@FETCH_STATUS =0 and @Count <@CheqCnt
	begin
		IF @LastNo =0
		Begin			
			INSERT INTO TblChequeNos (ChequeNos) VALUES (@LastChequeNo)
			SET @LastChequeNo = @LastChequeNo+1
			set @Count=@Count+1		
		End
		Else
		Begin
			SET @LastChequeNo = @LastChequeNo+1
			INSERT INTO TblChequeNos (ChequeNos) VALUES (@LastChequeNo)
			set @Count=@Count+1		
		End
	end	
	SET @Count=0
	fetch next from Temp Into @LastChequeNo,@CheqCnt,@LastNo	
end 
CLOSE Temp
DEALLOCATE Temp
select ChequeNos=RIGHT('000000'+ISNULL(ChequeNos,''),6) from TblChequeNos
drop table TblChequeNos