*:*****************************************************************************
*:        Program: CustomClass.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Levelwise Memory Variable Maintain Class
*:*****************************************************************************

DEFINE CLASS LevelProp AS CUSTOM OLEPUBLIC

	CurName = ''

	PROCEDURE CreateCursor
	IF ! EMPTY(THIS.CurName)
		CREATE CURSOR (THIS.CurName) (PropName C(100),PropValue C(250),KeyFld C(15))
	ENDIF
	ENDPROC

	PROCEDURE ADDValue
	PARAMETERS mPropName,mPropValue,mKeyfld
	IF USED((THIS.CurName))
		mPropValue = THIS.ANYTOSTR(mPropValue)
		INSERT INTO (THIS.CurName)(PropName,PropValue,KeyFld) VALUES(mPropName,mPropValue,mKeyfld)
	ENDIF
	ENDPROC

	PROCEDURE UpdateValue
	PARAMETERS mPropName,mPropValue,mKeyfld
	IF USED((THIS.CurName))
		mPropValue = THIS.ANYTOSTR(mPropValue)
		UPDATE (THIS.CurName) SET PropValue = mPropValue;
			WHERE PropName = mPropName AND KeyFld = mKeyfld
	ENDIF
	ENDPROC

	PROCEDURE RemoveValue
	PARAMETERS mKeyfld
	IF USED((THIS.CurName))
		SELECT (THIS.CurName)
		DELETE FOR UPPER(ALLT(KeyFld)) = UPPER(ALLT(mKeyfld))
	ENDIF
	ENDPROC

	PROCEDURE AssignValue
	PARAMETERS mKeyfld
	PRIVATE Y
	IF USED((THIS.CurName))

		SELECT (THIS.CurName)
		SET FILTER TO ALLTRIM(KeyFld)=ALLTRIM(mKeyfld)
		GO TOP
		DO WHILE ! EOF()
			Y = ALLT(PropName)

			DO CASE
			CASE TYPE(Y)='C'
				&Y = ALLT(PropValue)
			CASE TYPE(Y)='D'
				&Y = CTOD(ALLTRIM(PropValue))
			CASE TYPE(Y)='N'
				&Y = VAL(ALLTRIM(PropValue))
			CASE TYPE(Y) = 'L'
				&Y = IIF(ALLT(PropValue)='Y',.T.,.F.)
			ENDCASE

			SELECT (THIS.CurName)
			SKIP
		ENDDO
		SELECT (THIS.CurName)
		SET FILTER TO
	ENDIF
	ENDPROC

	FUNCTION ANYTOSTR
	PARAMETERS mVariable
	PRIVATE mReturn
	DO CASE
	CASE TYPE('mVariable') = 'D'
		mReturn = DTOC(mVariable)
	CASE TYPE('mVariable') = 'N'
		mReturn = ALLTRIM(STR(mVariable))
	CASE TYPE('mVariable') = 'C'
		mReturn = ALLTRIM(mVariable)
	CASE TYPE('mVariable') = 'L'
		mReturn = IIF(mVariable=.T.,'Y','N')
	OTHERWISE
		mReturn = ''
	ENDCASE
	RETURN mReturn
	ENDFUNC

ENDDEFINE
