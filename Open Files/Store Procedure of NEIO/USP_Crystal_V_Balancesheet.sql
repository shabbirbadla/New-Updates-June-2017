set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER Procedure [dbo].[USP_Crystal_V_Balancesheet]
As 
Declare @Balance Numeric(17,2),@Level Int,@CLDrcr CHAR(2)
DECLARE @OrderLevel VarChar(50),@TotalRecLine Char(1),@Tblvar Varchar(20)

Select @Balance = 0
Select @Level = 0
Select @TotalRecLine = 'N'

select @Tblvar = OBJECT_ID('Tempdb..#Curprint')
If @Tblvar <> '' And @Tblvar Is Not Null
Begin
	Drop Table #Curprint 
End

Select @TotalRecLine as TotalRecLine,@OrderLevel as OrderLevel,@Level As [Level],
	Space(1) As MainFlg,AC_GROUP_NAME as Ac_Name,Space(1) As LorA,
	[Group],@Balance As ClBal,@CLDrcr as CLDrcr 
	Into #Curprint From Ac_Group_Mast Where 1 = 2 
Select * From #Curprint

