If exists(select [name],xtype from sysobjects where [name]='ORDZM_VW_ITEM' and xtype='V')
drop view ORDZM_VW_ITEM

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:
-- Create date: 
-- Description:	 
-- Modification Date/By/Reason: 27/08/2012 Amrendra : Added Query for PIITEM Table for Bug-4909 
-- Guid line to update Bug-4909 -->As your view may have some costomization so just manually add following in your view  
--		union then Query for PIITEM Table
-- Modified : Birendra for Bug-21073 on 13-jan-2014 (If there is any costomization in this View please update only "BOMID" field related changes.)
-- =============================================

Create VIEW [dbo].[ORDZM_VW_ITEM] AS
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM STITEM 
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM SBITEM 
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM PTITEM
UNION
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM ARITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM OBITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM BPITEM 
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM BRITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM CNITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM CPITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM IIITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM PCITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM POITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM SOITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM SQITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM SRITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM DCITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM CRITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM DNITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM EPITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM ESITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,BOMID FROM IPITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM IRITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM JVITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,BOMID FROM OPITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM PRITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM SSITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM EQITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM TRITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM OSITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,BOMID FROM ITEM
UNION 
SELECT ENTRY_TY,DATE,DOC_NO,AC_ID, PARTY_NM,IT_CODE,ITEM_NO,ITEM,QTY,ITSERIAL,GRO_AMT,WARE_NM,TRAN_CD,CATE,DEPT,L_YN,INV_NO,INV_SR,RATE,'' AS BOMID FROM PIITEM
