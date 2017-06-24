if exists(select [name] from sysobjects where [name]='USP_ENT_GET_BYPRODUCT_DETAIL' and xType='P')
drop procedure USP_ENT_GET_BYPRODUCT_DETAIL
GO
/****** Object:  StoredProcedure [dbo].[USP_ENT_GET_BYPRODUCT_DETAIL]    Script Date: 07/14/2012 11:11:34 ******/
-- =============================================
-- Author:		Sachin.
-- Create date: 
-- Modified by : Birendra for BUG-5208 ON 14/07/2012 
-- Description:	This Stored procedure is useful to fetch byproduct/scrap/Wastage in BoM master.
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_ENT_GET_BYPRODUCT_DETAIL]
@RMITEMID INT
AS

SELECT A.IT_CODE, A.SIT_CODE, A.QTYPER, ITEMTYPE=B.SCAPTION, ITTYPECD='WT', SIT_NAME=C.IT_NAME 
	FROM it_WastescrapDet A, It_Advance_Setting_Master B, IT_MAST C
	,it_advance_setting D ----BIRENDRA:-BUG-5208 ON 14/07/2012
	WHERE A.IT_CODE = @RMITEMID AND B.IT_ADV_CODE='WT' AND A.SIT_CODE=C.IT_CODE 
	AND A.IT_CODE=D.IT_CODE AND D.WASTESCDET=1  --BIRENDRA:-BUG-5208 ON 14/07/2012
UNION ALL 
SELECT A.IT_CODE, A.SIT_CODE, A.QTYPER, ITEMTYPE=B.SCAPTION, ITTYPECD='SC', SIT_NAME=C.IT_NAME 
	FROM it_scrapDet A, It_Advance_Setting_Master B, IT_MAST C
	,it_advance_setting D ----BIRENDRA:-BUG-5208 ON 14/07/2012
	WHERE A.IT_CODE = @RMITEMID AND B.IT_ADV_CODE='SC' AND A.SIT_CODE=C.IT_CODE 
	AND A.IT_CODE=D.IT_CODE AND D.SCRAPDET=1 --BIRENDRA:-BUG-5208 ON 14/07/2012
UNION ALL 
SELECT A.IT_CODE, A.SIT_CODE, A.QTYPER, ITEMTYPE=B.SCAPTION, ITTYPECD='BY', SIT_NAME=C.IT_NAME 
	FROM it_byProdDet A, It_Advance_Setting_Master B, IT_MAST C
	,it_advance_setting D ----BIRENDRA:-BUG-5208 ON 14/07/2012
	WHERE A.IT_CODE = @RMITEMID AND B.IT_ADV_CODE='BY' AND A.SIT_CODE=C.IT_CODE 
	AND A.IT_CODE=D.IT_CODE AND D.BYPRODDET=1 --BIRENDRA:-BUG-5208 ON 14/07/2012



