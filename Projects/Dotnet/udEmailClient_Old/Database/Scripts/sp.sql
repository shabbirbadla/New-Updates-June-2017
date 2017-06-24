ALTER PROCEDURE USP_ENT_EMAILCLIENT_REPORTTOPDF
@party_nm varchar(100)
AS
BEGIN
	Select [Party Name] = Stmain.party_nm, [Email-Id] = Ac_mast.email, [Site] = u_mfgsite, [Invoice No.] = Stmain.inv_no, [Invoice Date] = Stmain.[date], [Sales Order no.] = u_sono, [Customer PO No.] = u_pono, 
	[Item Code] = Stitem.it_code, [Item Description] = it_desc, [Invoice Qty] = qty, UOM = u_uom, [No. of Sheets] = '', [Mode of Shipment] = u_tmode, 
	[Docket No.] = '', [Driver's Mob No.] = '', [Transporter Name] = u_deli, [Expected Date of Delivery] = ''
	From Stmain
	Inner Join Stitem on (Stmain.entry_ty = Stitem.entry_ty And Stmain.tran_cd = Stitem.tran_cd)
	Inner Join It_mast on (Stitem.it_code = It_Mast.it_code)
	Inner Join Ac_mast on (Stmain.ac_id=Ac_mast.ac_id)
	--Where Stmain.party_nm='W00033' and 
	Where Stmain.party_nm=@party_nm
	Order By Stmain.party_nm
END
GO
EXECUTE USP_ENT_EMAILCLIENT_REPORTTOPDF 'A90868' 
--select * from ac_mast where ac_name='W00033'

--select u_pono,* from stmain where party_nm='sales party'

--select * from stitem
--select * from it_mast 
--where it_code in(6517,6590)