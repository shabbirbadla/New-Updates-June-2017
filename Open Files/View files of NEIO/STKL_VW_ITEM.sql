
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--'U_LOSS' FIELD ADDED IN THIS VIEW. DO SAME CHANGES IN YOUR STKL_VW_ITEM (CHANGES FOR FM COSTING)
--Modified by Shrikant S. on 14/10/14 for Bug-23879	--Added QCSampQty field to view	
-- Modified by suraj k. on 14-03-2015 for Bug-25365	--Added Inv_no field to view	 SPACE(1) AS supbatchno, 
-- Modified by suraj k. on 14-03-2015 for Bug-25365	--alter SPACE(1) AS batchno field to batchno into view	 

ALTER VIEW [dbo].[STKL_VW_ITEM]
AS
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, u_asseamt, ware_nm, Tran_cd, u_pageno, dc_no, narr, batchno, mfgdt, 
                      expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30))as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.STITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, u_asseamt, ware_nm, Tran_cd, u_pageno, dc_no, narr, batchno, mfgdt, 
                      expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.SBITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, u_asseamt, ware_nm, Tran_cd, u_pageno, dc_no, narr, batchno, mfgdt, 
                      expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.SDITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr,
                      batchno,mfgdt,expdt,rate,0 AS U_LOSS,QcHoldQty, QcAceptQty,QcRejQty, QcRturnQty, QcWasteQty, QcProcLoss,lastQc_dt,lastQc_By
					  ,supbatchno,supmfgdt,supexpdt, QCSampQty,inv_no
FROM         dbo.PTITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, U_LQty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr, batchno, 
                      mfgdt, expdt,rate,0 AS U_LOSS,QcHoldQty,QcAceptQty,QcRejQty,QcRturnQty,QcWasteQty,QcProcLoss,lastQc_dt,lastQc_By
					  ,supbatchno, supmfgdt,supexpdt, QCSampQty,inv_no
FROM         dbo.ARITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.OBITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr,batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.BPITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.BRITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.CNITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.CPITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr,
                      batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.IIITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr,batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.PCITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.POITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.SOITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.SQITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, u_asseamt, ware_nm, Tran_cd, u_pageno, dc_no, narr, BatchNo, Mfgdt, 
                      Expdt,rate,0 AS U_LOSS,QcHoldQty, QcAceptQty,QcRejQty, QcRturnQty, QcWasteQty, QcProcLoss,lastQc_dt,lastQc_By
					  , supbatchno,supmfgdt, supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.SRITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr,
                      batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.DCITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.CRITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.DNITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.EPITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, BatchNo, mfgdt, expdt,rate,0 AS U_LOSS,QcHoldQty, QcAceptQty,QcRejQty, QcRturnQty, QcWasteQty, QcProcLoss,lastQc_dt,lastQc_By
					  , supbatchno, supmfgdt, supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.ESITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr, BatchNo, 
                      Mfgdt, Expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.IPITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr,
                      batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,QcHoldQty, QcAceptQty,QcRejQty, QcRturnQty, QcWasteQty, QcProcLoss,lastQc_dt,lastQc_By
					  , supbatchno, supmfgdt, supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.IRITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.JVITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr, batchno, 
                      mfgdt, expdt,rate,U_LOSS,QcHoldQty, QcAceptQty,QcRejQty, QcRturnQty, QcWasteQty, QcProcLoss,lastQc_dt,lastQc_By
					  , supbatchno, supmfgdt, supexpdt, QCSampQty,inv_no
FROM         dbo.OPITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.PRITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, BatchNo, Mfgdt, Expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.SSITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.EQITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.TRITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, u_pageno, dc_no, narr, BatchNo, 
                      Mfgdt, Expdt,rate,0 AS U_LOSS,QcHoldQty, QcAceptQty,QcRejQty, QcRturnQty, QcWasteQty, QcProcLoss,lastQc_dt,lastQc_By
					  , supbatchno, supmfgdt,supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.OSITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
UNION ALL
SELECT     entry_ty, date, doc_no, Ac_id, It_code, qty, u_lqty, itserial, pmkey, gro_amt, 0 AS U_ASSEAMT, ware_nm, Tran_cd, SPACE(1) AS U_PAGENO, dc_no, 
                      narr, batchno, '01/01/1900' AS mfgdt, '01/01/1900' AS expdt,rate,0 AS U_LOSS,0 AS QcHoldQty,0 AS QcAceptQty,0 AS QcRejQty,0 AS QcRturnQty,0 AS QcWasteQty,0 as QcProcLoss,cast('' as datetime) as lastQc_dt,cast('' as varchar(30)) as lastQc_By
					  , SPACE(1) AS supbatchno, '01/01/1900' AS supmfgdt, '01/01/1900' AS supexpdt, 0 as QCSampQty,inv_no
FROM         dbo.ITEM
WHERE     (pmkey = '+') OR
                      (pmkey = '-')
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

