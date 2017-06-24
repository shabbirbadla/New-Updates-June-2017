SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:
-- Create date: 
-- Description:	
-- Modification Date/By/Reason: 15/03/2012 Amrendra : For Multi Currency Bug-1365
-- Guid line to update Bug-1365-->As your view may have some costomization so just manually add following in your view  
--		0 as fcnet_amt --->(1) for All except for multicurrency enabled transaction 
--		fcnet_amt      --->(2) for enabling Multicurrency in Taransaction 
-- Example:  add (1) in all select section
--          If you want Multi currency in PT just add (2) in PTMAIN Table column list
-- =============================================


ALTER VIEW [dbo].[lmain_vw] AS
--Please read instruction above then add it manualy in all select downwards
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.ARMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, u_nature,l_yn, due_dt, 
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,
		compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,TDSPAYTYPE,u_chqdt, U_BRANCH,BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.BPMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,
		compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,TDSPAYTYPE, u_chqdt, U_BRANCH,BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.BRMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.CNMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, u_nature,l_yn, due_dt, [rule],tax_name,
		serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,cons_id=ac_id,
		scons_id=0,sac_id=0, SPACE(10) AS u_broker, TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.CPMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,
		cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,salesman,0 as fcnet_amt
FROM         dbo.CRMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.DCMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.DNMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, u_pinvno, u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt, [rule],tax_name,serty,
		'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,cons_id=ac_id,
		scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.EPMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,''
		AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.EQMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,''
		AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.ESMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,''as salesman,0 as fcnet_amt
FROM         dbo.IIMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,
		'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.IPMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.IRMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, u_nature,l_yn, due_dt, [rule],tax_name,
		serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,cons_id=ac_id,
		scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.JVMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, U_Pinvno, u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt, [rule],tax_name,
		space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,
		cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.OBMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.OPMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.PCMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.POMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, u_pinvno, u_pinvdt, narr, space(1) as u_nature,  l_yn, due_dt,[rule],tax_name,
		space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,
		cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.PTMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.PRMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.SOMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.SQMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.SRMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.SSMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,compid,
		cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.STMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,compid,
		cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,''as salesman,0 as fcnet_amt
FROM         dbo.SBMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.TRMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,3 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.OSMAIN
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS U_Pinvno, date AS u_pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM,salesman,0 as fcnet_amt
FROM         dbo.MAIN
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

