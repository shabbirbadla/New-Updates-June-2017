If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'Ent_Item_wise_Pickup_PO'))
Begin
	Drop Procedure Ent_Item_wise_Pickup_PO
End

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


CREATE Procedure [dbo].[Ent_Item_wise_Pickup_PO]
(
	@paraEntry_Ty Varchar(2),
	@paraTran_Cd Int,
	@paraCDept as Varchar(20),
	@paraIt_name as Varchar(50),
	@paraLcrule as varchar(20),
	@paraLProd as varchar(10),
	@paraLcdate as varchar(20),
	@addmode as bit,@editmode as bit,@paraParty as varchar(200)
)
As
DECLARE @SQLCOMMAND NVARCHAR(4000)
DECLARE @LExfld varchar(20),@LSqlfld nvarchar(2000)

SET @LSqlfld=''	
	Declare lother_cr cursor for
	Select distinct Fld_Nm From lother Where e_code in ('PO') AND Att_File = 0 and inpickup = 1
	open lother_cr
	fetch next from lother_cr into @LExfld
	while @@FETCH_STATUS =0
		begin
			Set @LSqlfld=RTRIM(@LSqlfld)+','+@LExfld
			fetch next from lother_cr into @LExfld
		end
close lother_cr  
deallocate lother_cr

DECLARE @FLDNM AS VARCHAR(20),@PERTNM AS VARCHAR(20),@DSqlfldnm nvarchar(2000),@DSqlprtnm nvarchar(2000)
SET @DSqlfldnm=''
set @DSqlprtnm=''

	Declare dcmast_cr cursor for
	select fld_nm AS Tmpcol from DCMAST where Entry_ty='PO' AND fld_nm<>''
	UNION ALL
	select pert_name AS Tmpcol from DCMAST where Entry_ty='PO' AND pert_name<>''
	open dcmast_cr
	fetch next from dcmast_cr into @FLDNM
	while @@FETCH_STATUS =0
		begin
			Set @DSqlfldnm=RTRIM(@DSqlfldnm)+',I.'+@FLDNM			
			fetch next from dcmast_cr into @FLDNM
		end
close dcmast_cr  
deallocate dcmast_cr

IF @addmode = 1 OR @editmode=1
BEGIN
	set @SQLCOMMAND='select cast(0 as bit) as LSelect,I.Entry_Ty as REntry_Ty,I.[date] as RDate,I.Doc_No as RDoc_no,I.item_no,I.itserial,M.Inv_No as RInv_No,I.BATCHNO,I.EXPDT,I.mfgdt
	,M.l_yn as RL_Yn,M.Tran_cd,M.Tran_cd as ITref_tran,M.Dept,M.Cate,M.[Rule],M.Inv_Sr as RInv_Sr,M.U_pinvdt,I.It_Code,M.Entry_Ty,M.Date,M.Doc_No,Ac.Ac_Name as RParty_nm,
	It.It_Name as Item,Space(100) as litemkey,I.Itserial as RItserial,I.u_asseamt,I.Qty,I.rate,000000000.0000 As adjqty,000000000.0000 As adjrepqty,
	I.Re_qty as RQty,I.Qty-I.Re_qty- isnull((select y.rqty from aritref x left join eou_itref_vw y on x.rentry_ty=y.rentry_ty and x.itref_tran=y.itref_tran
	where x.entry_ty = M.Entry_ty and x.tran_cd=I.Tran_cd and x.it_code= I.it_code and x.itserial=I.itserial),0) As BalQty,I.TAX_NAME,I.TAXAMT,refqty=(I.qty),refItserial=IR.Ritserial '+@LSqlfld+@DSqlfldnm+'
	from POITEM I inner join POMAIN M on I.Tran_cd=M.Tran_cd
	INNER JOIN AC_MAST AC ON M.Party_nm=AC.mailname
	INNER JOIN IT_MAST IT on I.It_code=IT.It_code
	LEFT JOIN POITREF IR ON I.Tran_cd=IR.Tran_cd AND I.entry_ty=IR.entry_ty and I.itserial=IR.Itserial
	where I.entry_ty =''PO'' and i.It_code = '+@paraIt_name+' and m.Party_nm= '+CHAR(39)+RTRIM(@paraParty)+CHAR(39)
END	
ELSE
BEGIN
SET @LSqlfld=''	
	Declare lother_cr cursor for
	Select distinct Fld_Nm From lother Where e_code in ('PO') AND Att_File = 0 and inpickup = 1
	open lother_cr
	fetch next from lother_cr into @LExfld
	while @@FETCH_STATUS =0
		begin
			Set @LSqlfld=RTRIM(@LSqlfld)+',I.'+@LExfld
			fetch next from lother_cr into @LExfld
		end
close lother_cr  
deallocate lother_cr

	set @SQLCOMMAND=' select cast(1 as bit) as LSelect,IR.REntry_Ty,I.[date] as RDate,I.Doc_No as RDoc_no,I.item_no,I.itserial,M.Inv_No as RInv_No,I.BATCHNO,I.EXPDT,I.mfgdt
	,M.l_yn as RL_Yn,M.Tran_cd,M.Tran_cd as ITref_tran,M.Dept,M.Cate,M.[Rule],M.Inv_Sr as RInv_Sr,M.U_pinvdt,I.It_Code,M.Entry_Ty,M.Date,M.Doc_No,Ac.Ac_Name as RParty_nm,
	It.It_Name as Item,Space(100) as litemkey,IR.RItserial,I.u_asseamt,POI.Qty,POI.rate,adjqty=EO.RQTY,000000000.0000 As adjrepqty,
	POI.Re_qty as RQty,POI.Qty-POI.Re_qty As BalQty,I.TAX_NAME,I.TAXAMT,refqty=(I.qty),refItserial=IR.Ritserial'+@LSqlfld+@DSqlfldnm+'
	from ARITEM I inner join ARMAIN M on I.Tran_cd=M.Tran_cd
	INNER JOIN AC_MAST AC ON M.Party_nm=AC.mailname
	INNER JOIN IT_MAST IT on I.It_code=IT.It_code
	LEFT JOIN ARITREF IR ON I.Tran_cd=IR.Tran_cd AND I.entry_ty=IR.entry_ty and I.itserial=IR.Itserial
	LEFT JOIN POITEM POI ON IR.rItserial=POI.itserial AND IR.Itref_tran = POI.Tran_cd
	LEFT JOIN eou_itref_vw EO ON EO.TRAN_CD=IR.TRAN_CD  AND EO.ENTRY_TY=IR.ENTRY_TY AND EO.RItserial=IR.Ritserial
	where I.entry_ty =''AR'' and m.Tran_cd= '+cast(@paraTran_Cd as varchar(7))+' and i.It_code = '+@paraIt_name+' and m.Party_nm= '+CHAR(39)+RTRIM(@paraParty)+CHAR(39)
End

execute sp_executesql @SQLCOMMAND
print @SQLCOMMAND