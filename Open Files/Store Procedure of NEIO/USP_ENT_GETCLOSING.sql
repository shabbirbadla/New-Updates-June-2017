Create Procedure [dbo].[USP_ENT_GETCLOSING]
@Edate SmallDatetime,@splcon Varchar(2000),@Billwise Bit,@type Int
AS
set Nocount On
Declare @SqlCmd NVarchar(4000)
-- @type =1 for Only Manufacturing Product
-- @type =2 for not Only Manufacturing Product

Select Ac_Name,Ac_Id Into #ACMAST From Ac_Mast Where Type='B'and Not (Typ  in ('EXCISE') OR AC_NAME Like '%CAPITAL GOODS PAYABLE%')  

/* Collecting Data from accounts details and create table [Start] */
SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,
MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME,AVW.ACSERIAL,MVW.U_PINVNO,MVW.U_PINVDT
,AC_MAST.TYP,u_cldt
into #tmpAcdet FROM LAC_VW AVW (NOLOCK)
INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID
INNER JOIN LMAIN_VW MVW (NOLOCK) 
ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY
WHERE MVW.DATE < = @Edate AND AC_MAST.TYPE='B' and AC_MAST.AC_NAME NOT LIKE '%CAPITAL GOODS PAYABLE%'
AND 1=2


set @SqlCmd='INSERT iNTO #tmpAcdet SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,'
set @SqlCmd=@SqlCmd+' '+'MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME,AVW.ACSERIAL,MVW.U_PINVNO,MVW.U_PINVDT'
set @SqlCmd=@SqlCmd+' '+',AC_MAST.TYP,AVW.U_CLDT'
set @SqlCmd=@SqlCmd+' '+'FROM LAC_VW AVW (NOLOCK)'
set @SqlCmd=@SqlCmd+' '+'INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID'
set @SqlCmd=@SqlCmd+' '+'INNER JOIN LMAIN_VW MVW (NOLOCK) '
set @SqlCmd=@SqlCmd+' '+'ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY'
set @SqlCmd=@SqlCmd+' '+'WHERE MVW.DATE < = '''+convert(Varchar(50),@Edate)+''' AND AC_MAST.TYPE=''B'' and AC_MAST.AC_NAME NOT LIKE ''%CAPITAL GOODS PAYABLE%'' '
set @SqlCmd=@SqlCmd+' '+@splcon
Execute Sp_ExecuteSql @SqlCmd

SET @SqlCmd = 'DELETE FROM #tmpAcdet WHERE 	DATE < (SELECT TOP 1 DATE FROM #tmpAcdet
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'') )
		AND AC_NAME IN (SELECT AC_NAME FROM #tmpAcdet
		WHERE ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB''
		OR Entry_Ty = ''OB'')  GROUP BY AC_NAME)'
EXECUTE sp_executesql @SqlCmd

if @type=1						
Begin
	if @Billwise=1				-- Billwise & Partywise
		Select Ac_id,Ac_name,Amount=sum(Case when Amt_ty='CR' Then -Amount Else Amount End),u_pinvno,u_pinvdt,u_cldt
			From #tmpAcdet Where Typ='EXCISE' 
				Group By Ac_id,Ac_name,u_pinvno,u_pinvdt,u_cldt having sum(Case when Amt_ty='CR' Then -Amount Else Amount End)<>0
					Order By Ac_Name
	else						--Partywise 
		Select Ac_id,Ac_name,Amount=sum(Case when Amt_ty='CR' Then -Amount Else Amount End) 
			From #tmpAcdet Where Typ='EXCISE'
				Group By Ac_id,Ac_name having sum(Case when Amt_ty='CR' Then -Amount Else Amount End)<>0
					Order By Ac_Name
End	
if @type=2
Begin
		Select Ac_id,Ac_name,Amount=sum(Case when Amt_ty='CR' Then -Amount Else Amount End)
			From #tmpAcdet 
				Group By Ac_id,Ac_name having sum(Case when Amt_ty='CR' Then -Amount Else Amount End)<>0
					Order By Ac_Name

--	if @Billwise=1				-- Billwise & Partywise
--		Select Ac_id,Ac_name,Amount=sum(Case when Amt_ty='CR' Then -Amount Else Amount End),u_pinvno,u_pinvdt
--			From #tmpAcdet 
--				Group By Ac_id,Ac_name,u_pinvno,u_pinvdt
--					Order By Ac_Name
--	else						--Partywise 
--		Select Ac_id,Ac_name,Amount=sum(Case when Amt_ty='CR' Then -Amount Else Amount End) 
--			From #tmpAcdet 
--				Group By Ac_id,Ac_name
--					Order By Ac_Name
End	

Drop Table #tmpAcdet







