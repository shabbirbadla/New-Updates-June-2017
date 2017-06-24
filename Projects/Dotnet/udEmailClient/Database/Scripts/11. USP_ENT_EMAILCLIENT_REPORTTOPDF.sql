IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='USP_ENT_EMAILCLIENT_REPORTTOPDF' AND XTYPE='P')
BEGIN
	DROP PROCEDURE USP_ENT_EMAILCLIENT_REPORTTOPDF
END
GO
CREATE PROCEDURE USP_ENT_EMAILCLIENT_REPORTTOPDF
@party_nm varchar(100),
@date smalldatetime
AS
BEGIN
	Select [Party Name] = Stmain.party_nm, [Email-Id] = Ac_mast.email, [Site] = u_mfgsite, [Invoice No.] = Stmain.inv_no, [Invoice Date] = Stmain.[date], [Sales Order no.] = u_sono, [Customer PO No.] = u_pono, 
	[Item Code] = Stitem.it_code, [Item Description] = cast(it_desc as varchar(max)), [Invoice Qty] = qty, UOM = u_uom, [No. of Sheets] = '', [Mode of Shipment] = u_tmode, 
	[Docket No.] = '', [Driver's Mob No.] = '', [Transporter Name] = u_deli, [Expected Date of Delivery] = ''
	into #temp
	From Stmain
	Inner Join Stitem on (Stmain.entry_ty = Stitem.entry_ty And Stmain.tran_cd = Stitem.tran_cd)
	Inner Join It_mast on (Stitem.it_code = It_Mast.it_code)
	Inner Join Ac_mast on (Stmain.ac_id=Ac_mast.ac_id)
	Where Stmain.party_nm=@party_nm and stmain.date=@date
	Order By Stmain.party_nm
	
	Select distinct [party name],[Email-Id],[Site],[Invoice No.],[Invoice Date],[Sales Order no.],[Customer PO No.],[Item Code],[Item Description],[Invoice Qty]
	,UOM,[No. of Sheets],[Mode of Shipment],[Docket No.],[Driver's Mob No.],[Transporter Name],[Expected Date of Delivery] from #temp
	--select * from #temp
	drop table #temp
END
GO
--EXECUTE USP_ENT_EMAILCLIENT_REPORTTOPDF 'v00020','2013-02-26 00:00:00.000'