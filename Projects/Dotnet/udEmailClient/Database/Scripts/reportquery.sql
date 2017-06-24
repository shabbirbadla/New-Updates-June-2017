Select [Party Name] = Stmain.party_nm, [Site] = u_mfgsite, [Invoice No.] = Stmain.inv_no, [Invoice Date] = Stmain.[date], [Sales Order no.] = u_sono, [Customer PO No.] = u_pono, 
[Item Code] = Stitem.it_code, [Item Description] = it_desc, [Invoice Qty] = qty, UOM = u_uom, [No. of Sheets] = '', [Mode of Shipment] = u_tmode, 
[Docket No.] = '', [Driver's Mob No.] = '', [Transporter Name] = u_deli, [Expected Date of Delivery] = ''
From Stmain
Inner Join Stitem on (Stmain.entry_ty = Stitem.entry_ty And Stmain.tran_cd = Stitem.tran_cd)
Inner Join It_mast on (Stitem.it_code = It_Mast.it_code)