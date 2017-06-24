IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Alert_Crlimit]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Alert_Crlimit]  
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ramya.
-- Create date: 24/09/2011
-- Description:	This Stored procedure is useful to get the Party Details Who Crossed The Credit Amount Limit
-- =============================================


Create Procedure [dbo].[Usp_Alert_Crlimit]
@CommanPara varchar(8000) 
AS
Begin

Declare @SqlCommand nvarchar(4000), @FCON as NVARCHAR(2000),@Fcond_Alert AS NVARCHAR(2000)

	 EXECUTE [Usp_Rep_FiltCond_Alert]
	 @CommanPara
	 ,@VMAINFILE='A',@VITFILE='A',@VACFILE=' '
	 ,@It_Mast= '',@Ac_Mast= 'AC_MAST'
	 ,@vFcond_Alert =@Fcond_Alert OUTPUT
     print '@Fcond_Alert '+@Fcond_Alert

	if isnull(@Fcond_Alert,'')=''
	begin
		set @Fcond_Alert=' WHERE 1=1 '
	end

  delete from alert_crlimit
	
    set @SqlCommand='Insert into Alert_Crlimit'
	set @SqlCommand=@SqlCommand+' '+'Select distinct stmain.party_nm,ac_mast.cramount,sum(stmain.net_amt)'
	--set @SqlCommand=@SqlCommand+' '+'rtrim(stmain.party_nm)'+'+ '' ''+'+'ltrim('+'''Has Crossed The Credit Amount Limit'')'
	set @SqlCommand=@SqlCommand+' '+' from stmain '  
	set @SqlCommand=@SqlCommand+' '+'inner join  ac_mast on(ac_mast.ac_name=stmain.party_nm)'
    set @SqlCommand=@SqlCommand+' '+@Fcond_Alert
	set @SqlCommand=@SqlCommand+' '+' and ac_mast.cramount < '
	set @SqlCommand=@SqlCommand+' '+'(select sum(stmain.net_amt) from stmain'
	set @SqlCommand=@SqlCommand+' '+' where ac_mast.ac_name=stmain.party_nm group by stmain.party_nm)'
	set @SqlCommand=@SqlCommand+' '+'group by stmain.party_nm,ac_mast.cramount'

  PRINT @SqlCommand
  EXECUTE SP_EXECUTESQL @SqlCommand

	select [Party Name],[Credit Limit],[As On Date Total Bill Amount] from Alert_Crlimit

End





