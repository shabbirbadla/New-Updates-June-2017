if exists(select [name] from sysobjects where [name]='mainall_multi_vw' and xtype='V')
drop view mainall_multi_vw

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[mainall_multi_vw]
AS
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.ARMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,serbamt,sercamt,serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.BPMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,serbamt,sercamt,serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.BRMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.CNMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.CPMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.CRMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.DCMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.DNMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.EPMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.EQMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.ESMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.IIMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.IPMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.IRMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.JVMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.OBMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.OPMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.PCMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.POMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.PTMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.PRMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.SOMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.SQMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.SRMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.SSMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.STMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.TRMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.SBMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,0 as fcnew_all,0 as fcnet_amt,0 as fcdiffamt,0 as fctds,0 as fcexrate,0 as fcdisc --Birendra
FROM         dbo.SDMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,fcnew_all,fcnet_amt,fcdiffamt,fctds,fcexrate,fcdisc --Birendra
FROM         dbo.OSMALL
UNION ALL
SELECT     Main_tran, Tran_cd, ENTRY_TY,ACSERIAL, date, doc_no, inv_no, party_nm, new_all, ENTRY_ALL,ACSERI_ALL, inv_sr, tds, disc, l_yn, net_amt, Ac_id, date_all,0 as serbamt,0 as sercamt,0 as serhamt,COMPID
			,0 as fcnew_all,0 as fcnet_amt,0 as fcdiffamt,0 as fctds,0 as fcexrate,0 as fcdisc --Birendra
FROM         dbo.MALL

