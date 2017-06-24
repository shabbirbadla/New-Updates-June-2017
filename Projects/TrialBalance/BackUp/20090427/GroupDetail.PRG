*:*****************************************************************************
*:        Program: GroupDetail.PRG
*:         System: Udyog Softwares
*:         Author: RAGHU
*:  Last modified: 27/04/2006
*:			AIM  : Accounts [Group/Ledger] Debit And Crdit details
*:*****************************************************************************
PARA gcode

IF EMPTY(gcode)
	RETURN ""
ENDIF

SELECT 	g1.mainflg,;
	g1.LEVEL,;
	g1.Ac_Id,;
	g1.Ac_Name,;
	g1.GROUP,;
	g1.opbal,;
	g1.Debit,;
	g1.Credit,;
	g1.clbal;
	FROM _TBAcMast g1;
	ORDER BY g1.Ac_Name;
	WHERE g1.Ac_Group_Id == gcode;
	INTO CURSOR "CUR"+SYS(3) READWRITE
GO TOP
RETURN ALIAS()
