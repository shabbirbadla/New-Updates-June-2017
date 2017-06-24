*:*****************************************************************************
*:        Program: <Program Name>
*:         System: Udyog Software [I] Limited.
*:   Programmer  : <Programmer Name>
*:  Last modified: <Date>
*:			AIM  : <Function>
*:*****************************************************************************

PARAMETERS mSessionId

IF TYPE('mSessionId') ='N'
	SET DATASESSION TO (mSessionId)
ENDIF

nHandle = 0
sqlconobj = NEWOBJECT('sqlconnudobj','sqlconnection.vcx')	&& Creating SQLConnection Object
IF TYPE('sqlconobj')
	MESSAGEBOX('SQL-Connetion object not FOUND',0,VuMess)
	RETURN .F.
ENDIF

StringSQL = "STORE PROCEDURE/STRING"
sql_con=sqlconobj.dataconn("EXE",Company.DbName,StringSQL,"_ReturnCursor","nHandle",mSessionId)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
ENDIF


** && Other External Form/Programm Files [Start]

** && Other External Form/Programm Files [End]
sql_con=sqlconobj.sqlconnclose("nHandle")		&& Closing Connection
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
ENDIF
