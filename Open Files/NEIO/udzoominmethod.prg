PROCEDURE zoomin_WO_Sum 
lpara obj_grd_total,Obj_Thisform

*!*	WITH obj_grd_total
*!*		.recordsource=1
*!*		.recordsource=''
*!*	CREATE CURSOR _zoomintot (fldcap C(30),fldvalue F(15,2),fldper F(5,2)) 
*!*		.recordsource=1
*!*		.recordsource='_zoomintot'
*!*		.column1.controlsource='_zoomintot.fldcap'
*!*		.column2.controlsource='_zoomintot.fldvalue'
*!*		.column3.controlsource='_zoomintot.fldper'
*!*		.column1.header1.caption='Caption'
*!*		.column2.header1.caption='Total'
*!*		.column3.header1.caption='%'
*!*		.column1.Width=100
*!*		.column2.Width=75
*!*		.column3.Width=35
*!*	ENDWITH 


Crit = ""

Obj_Thisform.CurrentLevel = Obj_Thisform.Orddetail1.Constatus.optgroup.VALUE

SELECT SUM(a.trm_Qty) AS Qty;
	FROM (Obj_Thisform.Ordzoom) a WITH (BUFFERING = .T.);
	WHERE a.Expand = .T.;
	AND a.CharLevel = 'A';
	INTO CURSOR TCurTotal
	&&	
GO TOP
TCurRqty = IIF(ISNULL(Qty),0,Qty) 


SELECT SUM(a.Qty) AS Qty,;
	SUM(a.RQty) AS RQty;
	FROM (Obj_Thisform.Ordzoom) a WITH (BUFFERING = .T.);
	WHERE a.Expand = .T.;
	AND a.CharLevel = 'A';
	INTO CURSOR CurTotal
GO TOP

lnOrdQty = IIF(ISNULL(Qty),0,Qty)
lnqPer=IIF(lnOrdQty>0,(lnOrdQty/lnOrdQty)*100.00,0)
INSERT INTO _zoomintot (fldcap,fldvalue,fldper) values('Booked Qty.',lnOrdQty,lnqPer)
lnAdjQty = IIF(ISNULL(RQty),0,RQty) 
lnRqPer = IIF(lnOrdQty>0,ROUND(100*lnAdjQty/lnOrdQty,2),0)
INSERT INTO _zoomintot (fldcap,fldvalue,fldper) values('Executed Qty.',lnAdjQty,lnRqPer)
lnScPer = IIF(lnOrdQty>0,ROUND(100*TCurRqty/lnOrdQty,2),0)
INSERT INTO _zoomintot (fldcap,fldvalue,fldper) values('Terminated Qty.',TCurRqty,lnScPer)
lnBalqty = lnOrdQty-(lnAdjQty+TCurRqty)
lnBqPer = 100-(lnRqPer+lnScPer)
INSERT INTO _zoomintot (fldcap,fldvalue,fldper) values('Pending Qty.',lnBalqty,lnBqPer)
SELECT _zoomintot
GO top
obj_grd_total.refresh()
*Birendra : Pending IP :Start:
msqlstr="SET DATEFORMAT 'DMY' EXECUTE USP_ENT_WkOp_ALLOCATION 'OP',0,"+"'"+DTOS(Obj_Thisform._Otmpvar.eDate)+"',''"
nretval = Obj_Thisform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"balitem_vw1","thisform.nhandle",Obj_Thisform.DataSessionId)
If Used("balitem_vw1")
	Select balitem_vw1
	If RECCOUNT() > 0
	 UPDATE a SET a.ipbalqty=b.wipqty from (Obj_Thisform.Ordzoom) as a join balitem_vw1 as b ON(a.rentry_ty=b.entry_ty AND a.itref_tran=b.tran_cd) WHERE a.entry_ty=='IP'
	ENDIF
	USE IN balitem_vw1
ENDIF
*Birendra : Pending IP  :End:

*!*	IF THISFORM.CurrentLevel<> 4
*!*		THISFORM.Orddetail1.Constatus.label7.Visible= .F. 
*!*		THISFORM.Orddetail1.Constatus.Txttermination.Visible= .F.
*!*		Thisform.Orddetail1.COnstatus.Cbolevel.Enabled= .T.  
*!*		THISFORM.Orddetail1.conDetail.txtQty.VALUE = lnOrdQty
*!*		THISFORM.Orddetail1.conDetail.TxtQtyper.VALUE = 100.00
*!*		THISFORM.Orddetail1.Constatus.txtRecQty.VALUE = lnAdjQty
*!*		THISFORM.Orddetail1.Constatus.TxtRqPer.VALUE = lnRqPer
*!*		THISFORM.Orddetail1.Constatus.TxtSClosedqty.VALUE = TCurRqty
*!*		THISFORM.Orddetail1.Constatus.TxtScPer.VALUE = lnScPer
*!*		THISFORM.Orddetail1.Constatus.txtBalQty.VALUE = lnBalqty
*!*		THISFORM.Orddetail1.Constatus.TxtBqper.VALUE = lnBqPer
*!*	ELSE
*!*	&&added by satish pal -start
*!*		THISFORM.Orddetail1.Constatus.label7.Visible= .T. 
*!*		THISFORM.Orddetail1.Constatus.Txttermination.Visible= .T.
*!*		Thisform.Orddetail1.COnstatus.Cbolevel.Enabled= .F.  
*!*		THISFORM.Orddetail1.conDetail.txtQty.VALUE = lnOrdQty
*!*		THISFORM.Orddetail1.conDetail.TxtQtyper.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.txtRecQty.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.TxtRqPer.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.TxtSClosedqty.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.TxtScPer.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.txtBalQty.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.TxtBqper.VALUE = 0
*!*		THISFORM.Orddetail1.Constatus.Txttermination.Value=TCurRqty
*!*		
*!*	ENDIF
&&added by satish pal -end
USE IN CurTotal
