SET STEP ON
SET SAFETY OFF
SET TALK OFF
SET EXACT ON
nhandle = 0
IF USED('_cursor2')
	SELECT _cursor2
	USE
ENDIF
IF USED('_cursor3')
	SELECT _cursor3
	USE
ENDIF
IF USED('_cursor1')
	SELECT _cursor1
	USE
ENDIF
LOCAL lpadname,mkey1,mkey2,nodekey
msqlstr = "select 0 as levelp, 0 as levelc,* from com_menu"
msqlstr = msqlstr + " where padname not in (select barname from com_menu)"
nretval=sqlconobj.dataconn('EXE',dataUpdtdb,msqlstr,"_cursor3","nhandle")
IF nretval<0
	nretval = sqlconobj.sqlconnclose("nhandle")
	IF nretval < 0
		RETURN 0
	ENDIF
ENDIF
msqlstr = "select 0 as levelp, 0 as levelc,* from com_menu"
nretval=sqlconobj.dataconn('EXE',dataUpdtdb,msqlstr,"_cursor2","nhandle")
IF nretval<0
	=MESSAGEBOX('com_menu file error '+CHR(13)+PROPER(MESSAGE()),48,vumess)
	nretval = sqlconobj.sqlconnclose("nhandle")
	IF nretval < 0
		RETURN 0
	ENDIF
ENDIF
nretval = sqlconobj.sqlconnclose("nhandle")
IF nretval < 0
	RETURN 0
ENDIF
SELECT 0
USE DBF('_cursor2') AGAIN SHARED ALIAS _cursor1
INDEX ON barname TAG barname
INDEX ON padname+STR(padnum)+STR(barnum) TAG padname
SET ORDER TO barname
mlevela = 11000
pdn = 1
SELECT _cursor3
GO TOP
DO WHILE !EOF()
	br=ALLTRIM(barname)
	SELECT _cursor1
	IF SEEK(br)
		SELECT _cursor1
		REPLACE levelc WITH mlevela, padnum WITH pdn FOR ALLTRIM(barname) == ALLTRIM(br) IN _cursor1
		mlevela = mlevela + 1000
		pdn  = pdn + 1
	ENDIF
	SELECT _cursor3
	IF !EOF()
		SKIP
	ENDIF
ENDDO
SELECT _cursor1
INDEX ON barname TAG barname
INDEX ON padname TAG padname ADDITIVE
FOR N = 1 TO 50
	x = .T.
	DO WHILE x
		SELECT * FROM _cursor1 WHERE levelp = 0 AND levelc = 0 INTO CURSOR _sant3
		IF RECCOUNT() <= 0
			EXIT
		ELSE
			r = RECCOUNT()
		ENDIF
		SELECT _sant3
		GO TOP
		DO WHILE !EOF()
			mlevelc = 0
			brn = 1
			SELECT _cursor1
			IF SEEK(ALLTRIM(_sant3.padname),'_cursor1','barname')
				IF _cursor1.levelc > 0
					mlevelc = _cursor1.levelc +1
				ELSE
					IF levelp > 0
						l1 = (levelp - MOD(levelp,1000))
						lx1 = (l1 + 1000)
						CALCULATE MAX(levelp) FOR levelp >= l1 AND levelp < lx1 TO x1
						mlevelc = x1 + 1
					ELSE
						mlevelc = 0
					ENDIF
				ENDIF
			ENDIF
			SELECT _cursor1
			IF SEEK(ALLTRIM(_sant3.padname))
				lv = levelp
				DO WHILE PROPER(ALLTRIM(_cursor1.padname)) == PROPER(ALLTRIM(_sant3.padname)) AND !EOF()
					IF lv <= 0
						REPLACE levelp WITH mlevelc, barnum WITH brn IN _cursor1
						mlevelc = mlevelc + 1
						brn = brn + 1
					ENDIF
					IF !EOF()
						SKIP
					ENDIF
				ENDDO
			ENDIF
			SELECT _sant3
			IF !EOF()
				SKIP
			ENDIF
		ENDDO
		x = .F.
	ENDDO
ENDFOR
SELECT _cursor1
REPLACE ALL RANGE WITH levelc + levelp IN _cursor1
REPLACE ALL PrompName WITH PROPER(PrompName) IN _cursor1
SELECT * FROM _cursor1 INTO CURSOR _cursor4 
*************************************************
msqlstr = "truncate table com_menu"
nretval=sqlconobj.dataconn("EXE",dataUpdtdb,msqlstr,"","nhandle",,.T.)
IF nretval<0
	nretval = sqlconobj.sqlconnclose("nhandle")
	IF nretval < 0
		RETURN 0
	ENDIF
ENDIF
mcommit = sqlconobj._sqlcommit("nhandle")
IF mcommit<=0
	nretval = sqlconobj.sqlconnclose("nhandle")
	IF nretval < 0
		RETURN 0
	ENDIF
ENDIF
nretval = sqlconobj.sqlconnclose("nhandle")
IF nretval < 0
	RETURN 0
ENDIF
SELECT _cursor4

GO TOP
DO WHILE !EOF()
	msqlstr = sqlconobj.geninsert("com_menu","'LevelP','LevelC'","","_cursor4",mvu_backend)
	nretval=sqlconobj.dataconn("EXE",dataUpdtdb,msqlstr,"","nhandle",,.T.)
	IF nretval<0
		nretval = sqlconobj.sqlconnclose("nhandle")
		IF nretval < 0
			RETURN 0
		ENDIF
	ENDIF
	mcommit = sqlconobj._sqlcommit("nhandle")
	IF mcommit<=0
		nretval = sqlconobj.sqlconnclose("nhandle")
		IF nretval < 0
			RETURN 0
		ENDIF
	ENDIF
	SELECT _cursor4
	IF !EOF()
		SKIP
	ENDIF
ENDDO
nretval = sqlconobj.sqlconnclose("nhandle")
IF nretval < 0
	RETURN 0
ENDIF
SELECT _cursor4
COPY TO man1
**************************************************
msqlstr = "DELETE FROM com_menu WHERE LEN(BARNAME) = 0 OR RANGE < 9999"
nretval=sqlconobj.dataconn('EXE',dataUpdtdb,msqlstr,"_cursor3","nhandle",,.T.)
IF nretval<0
	nretval = sqlconobj.sqlconnclose("nhandle")
	IF nretval < 0
		RETURN 0
	ENDIF
ENDIF
=MESSAGEBOX("Menu Setting Successfully Completed!!!",0,vumess)
