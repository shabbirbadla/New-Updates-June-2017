


-- =============================================
-- Author:
-- Create date: 
-- Description:	This View contains All Transaction __AcDet Tables and is used for  Reports.
-- Modification Date/By/Reason: 27/07/2011 Rupesh Prajapati. Change date as u_cldt for ObAcDet TKT-8968
-- Modification Date/By/Reason: 15/03/2012 Amrendra : For Multi Currency Bug-1365
-- Guid line to update Bug-1365-->As your view may have some costomization so just manually add following in your view  
--		0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD --->(1) for All except for multicurrency enabled transaction 
--		FCID, fcexrate, fcamount, fcre_all, FCDISC, FCTDS,POSTORD --->(2) for enabling Multicurrency in Taransaction 
-- Example:  add (1) in all select section
--          If you want Multi currency in PT just add (2) in PTACDET Table column list
-- Modification Date/By/Reason: 28/03/2014 Pankaj M. : Issue in Genral Ledger, Zoom In Ledger and Account Confirmation Report (POSTORD field added in view)  Bug-22246
-- =============================================

ALTER VIEW [dbo].[lac_vw]
AS
--Please read instruction above then add it manualy in all select downwards
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.ARACDET  
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.BPACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.BRACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.CNACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.CPACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.CRACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.DCACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.DNACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.EPACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.EQACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.ESACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.IIACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.IPACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.IRACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
					Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.JVACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr,u_cldt, l_yn, Re_all, Disc, 
			Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.OBACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.OSACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.OPACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.PCACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.POACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.PRACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.PTACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.SOACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.SQACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.SRACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.SSACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.STACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.SBACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.SDACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, CONVERT(SmallDateTime, 
                      SPACE(8)) AS u_cldt, l_yn, Re_all, Disc, Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.TRACDET
UNION ALL
SELECT     Tran_cd, entry_ty, date, doc_no, AC_ID,ACSERIAL, ac_name, amount, amt_ty, dept, cate, inv_sr, inv_no, oac_name, clause, narr, u_cldt, l_yn, Re_all, Disc, 
                      Tds,compid,0 as FCID,0 as fcexrate,0 as fcamount,0 as fcre_all,0 as FCDISC, 0 as FCTDS,POSTORD
FROM         dbo.ACDET


GO


