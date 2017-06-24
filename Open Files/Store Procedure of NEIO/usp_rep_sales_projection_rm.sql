If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'usp_rep_sales_projection_rm'))
Begin
	Drop Procedure usp_rep_sales_projection_rm
End

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
--=============================================
-- Author:
-- Create date:
-- Description:
-- Remark:EXECUTE usp_rep_sales_projection_rm '*HP DESKTOP* # *HCL DESKTOP*'

-- =============================================
CREATE PROCEDURE [dbo].[usp_rep_sales_projection_rm] 
@item AS nvarchar(500) --, @edate AS DATETIME

AS
SET QUOTED_IDENTIFIER OFF
declare @itemtemp AS nvarchar(500)
 
set @itemtemp=replace(@item,'#',',')
set @itemtemp=replace(@itemtemp,'*',char(39))
declare @sql as nvarchar(1000)
print @itemtemp
print @item
set @sql='SELECT c.item,SUM(c.qty) AS qty INTO ##temp1 FROM item c
LEFT JOIN main d ON c.entry_ty = d.entry_ty AND c.Tran_cd = d.Tran_cd
WHERE c.item in('+@itemtemp+') and c.entry_ty IN (''SP'') GROUP BY c.item'
print @sql
execute sp_executesql @sql

SELECT c.it_name,qty_inhand =SUM(CASE WHEN  A.PMKEY='+' THEN A.QTY ELSE -A.QTY END) into #temp2 FROM STKL_VW_ITEM A 
INNER JOIN STKL_VW_MAIN b ON (a.TRAN_CD=b.TRAN_CD AND a.ENTRY_TY=b.ENTRY_TY)
INNER JOIN IT_MAST c  ON (c.IT_CODE=a.IT_CODE)
INNER JOIN AC_MAST d ON (d.AC_ID=b.AC_ID)
INNER JOIN LCODE e ON (a.ENTRY_TY=e.ENTRY_TY)
GROUP BY c.it_name,c.RATEUNIT  ORDER BY c.it_name,c.RATEUNIT



CREATE TABLE #temp3(
it_name VARCHAR(50),
wipqty NUMERIC (15,2)
)

INSERT INTO #temp3
EXECUTE dbo.USP_ENT_WIP_STOCK 'OP','',''

SELECT ITEM,SUM(qty) AS RM_indent INTO #temp4
FROM dbo.eqitem WHERE entry_ty IN ('PD') GROUP BY item

/*
SELECT #temp1.item,#temp1.qty,#temp2.qty_inhand,#temp3.wipqty,
REQ_FG_QTY=CASE WHEN #TEMP1.QTY-#TEMP2.QTY_INHAND<0 THEN 0 ELSE (#TEMP1.QTY-(#TEMP2.QTY_INHAND+#temp3.wipqty)) END,
BOMDET.Rmitem,
REQ_RM_QTY=(BOMDET.Rmqty*(#TEMP1.QTY-(#TEMP2.QTY_INHAND+#temp3.wipqty)))/BOMhead.fgqty,a.qty_inhand AS 'RM_QTY_inhand',
#temp4.RM_indent
FROM #temp1 
LEFT JOIN #temp2 ON #temp1.item=#temp2.it_name
LEFT JOIN #temp3 ON #temp1.item=#temp3.it_name
INNER JOIN dbo.Bomhead ON #temp1.item=bomhead.Item
INNER JOIN dbo.Bomdet ON dbo.Bomhead.BomId = dbo.Bomdet.Bomid AND dbo.Bomhead.Bomlevel = dbo.Bomdet.Bomlevel
INNER JOIN #temp2 a ON a.it_name=bomdet.Rmitem
INNER join #temp4 ON #temp4.item=bomdet.Rmitem 
*/

SELECT c.item,SUM(c.qty-c.re_qty) AS bal_po_qty into #temp5 FROM poitem c
LEFT JOIN pomain d ON c.entry_ty = d.entry_ty AND c.Tran_cd = d.Tran_cd
WHERE c.entry_ty IN ('PO') GROUP BY c.item



SELECT bomhead.item as 'FG Item Name',
BOMDET.Rmitem as 'RM Item Name',
(BOMDET.Rmqty*(##TEMP1.QTY-(#TEMP2.QTY_INHAND+#temp3.wipqty)))/BOMhead.fgqty as 'RM Required Qty',a.qty_inhand AS 'RM Qty In Hand',
#temp5.bal_po_qty as 'Bal. PO Qty',(((BOMDET.Rmqty*(##TEMP1.QTY-(#TEMP2.QTY_INHAND+#temp3.wipqty)))/BOMhead.fgqty)-(a.qty_inhand+#temp5.bal_po_qty)) as 'RM Yet to be Ordered',#temp4.RM_indent as 'RM Indent Raised'
FROM ##temp1 
LEFT JOIN #temp2 ON ##temp1.item=#temp2.it_name
LEFT JOIN #temp3 ON  ##temp1.item=#temp3.it_name
INNER JOIN dbo.Bomhead ON  ##temp1.item=bomhead.Item
INNER JOIN dbo.Bomdet ON dbo.Bomhead.BomId = dbo.Bomdet.Bomid AND dbo.Bomhead.Bomlevel = dbo.Bomdet.Bomlevel
INNER JOIN #temp2 a ON a.it_name=bomdet.Rmitem
left join #temp4 ON #temp4.item=bomdet.Rmitem 
LEFT JOIN #temp5 ON  bomdet.rmitem=#temp5.item

drop table ##temp1

SET QUOTED_IDENTIFIER Off
SET ANSI_NULLS Off



