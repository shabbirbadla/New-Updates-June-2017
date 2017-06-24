parameter brentry_ty,btran_cd
MESSAGEBOX('r1')
*indexbuf=brentry_ty+dtos(brdate)+brdoc_no
keyboard '{ctrl+w}'
ON Key Label "ALT+0" 

DO tovoucher WITH btran_cd,brentry_ty
*!*	do case
*!*		case brentry_ty='BP'
*!*			DO tovoucher WITH btran_cd,brentry_ty
*!*			m.vvPrompt=[VOUCHERMASTER]+[BANKPAYMENT]
*!*			DO VOUCHER WITH "BP","","","","","",INDEXBUF
*!*		case brentry_ty=[BR] 
*!*			m.vvPrompt=[VOUCHERMASTER]+[BANKRECEIPT]
*!*			DO VOUCHER WITH "BR","","","","","",INDEXBUF
*!*		case brentry_ty=[JV] 
*!*			m.vvPrompt=[VOUCHERMASTER]+[JOURNALVOUCHER]
*!*			DO VOUCHER WITH "JV","","","","","",INDEXBUF
*!*	endcase
ON Key Label "ALT+0" do altd with 0
*IF !USED('LDCW')
*	USE LDCW SHAR AGAIN IN 0
*ENDIF


