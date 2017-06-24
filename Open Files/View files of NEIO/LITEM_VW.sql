
ALTER VIEW [dbo].[LITEM_VW] AS
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.ARITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.BPITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.BRITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0
FROM         dbo.CNITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.CPITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.CRITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.DCITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.DNITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.EPITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.EQITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.ESITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.IIITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.IPITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.IRITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.JVITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.OBITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.OPITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.PCITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.POITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1,ADVATPER1,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.PTITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1,ADVATPER1,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.PRITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.SOITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.SQITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1,ADVATPER1,EXAMT,U_CESSAMT,U_HCESAMT 
FROM         dbo.SRITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.SSITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1,ADVATPER1,EXAMT,U_CESSAMT,U_HCESAMT
FROM         dbo.STITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT=0,U_HCESAMT=0   
FROM         dbo.TRITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.ITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.OSITEM
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.sbitem
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, itserial, dc_no, party_nm, item, qty, inv_no, ware_nm, Ac_id, It_code,re_qty,TAX_NAME,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,RATE,GRO_AMT,COMPID,ADDLVAT1=0,ADVATPER1=0,EXAMT,U_CESSAMT,U_HCESAMT   
FROM         dbo.sditem

GO


