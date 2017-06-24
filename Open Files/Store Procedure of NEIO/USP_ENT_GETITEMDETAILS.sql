IF Exists(Select [Name] from Sysobjects Where xType='P' and [name]='USP_ENT_GETITEMDETAILS')
Begin
	Drop Procedure USP_ENT_GETITEMDETAILS
End
Go
Create Procedure USP_ENT_GETITEMDETAILS
@It_code Numeric(10,0)
AS
Declare @Rlevel Varchar(10),@Rate Numeric(15,2),@Ac_id Numeric(10,0),@PartyNm Varchar(100)
Select Top 1 @Rlevel=Rlevel,@Rate=it_rate From IT_RATE Where It_code=@It_code and Ptype='I' Order by Rlevel

If @Rlevel is null
Begin
	Select Top 1 @Ac_id=Ac_id,@Rate=it_rate,@PartyNm=ac_name  From IT_RATE Where It_code=@It_code and Ptype='P' 
	if @Ac_id is null
		Select @Ac_id=0,@Rate=0,@PartyNm='' 
End
Else
Begin
	Select Top 1 @Ac_id=Ac_Id,@PartyNm=ac_name From Ac_mast Where Rate_Level=@Rlevel
End
Select @Ac_id as Ac_id,@Rate as Rate,@PartyNm as Party_nm
