*!*	LPARAMETERS CursorName AS STRING,;
*!*		ShowOrHide AS NUMBER,;
*!*		LeftOrRight AS CHARACTER

*!*	*!*	Usage :	=ShowHideDetail('Cursor',1,'L')

*!*	SELECT CursorName
*!*	mRecordNo = RECNO()
*!*	SELECT * FROM (CursorName) INTO OldTabl

*!*	DO CASE
*!*	CASE ShowOrHide = 1					&& Show Details
*!*	CASE ShowOrHide = 2					&& Hide	Details
*!*	ENDCASE

*!*	PARAMETERS mLOR,CurName,CFlg,NLevel
*!*	LOCAL oErr1 AS EXCEPTION
*!*	TRY
*!*		SELECT(CurName)
*!*		GO TOP
*!*		DO CASE
*!*		CASE mLOR = 'L'
*!*			LOCATE FOR EMPTY(LAC_NAME2)
*!*			IF ! FOUND()
*!*				APPEND BLANK
*!*			ENDIF
*!*		CASE mLOR = 'R'
*!*			LOCATE FOR EMPTY(RAC_NAME2)
*!*			IF ! FOUND()
*!*				APPEND BLANK
*!*			ENDIF
*!*		ENDCASE
*!*		Laexact = SET("Exact")
*!*		SET EXACT ON
*!*		SELECT _TBAcMast
*!*		SET FILTER TO LevelFlg = CFlg AND LEVEL = NLevel
*!*		mLastCursor = ALIAS()
*!*		mTotalFlds = FCOUNT()
*!*		GO TOP
*!*		DO WHILE ! EOF()
*!*			FOR I = 1 TO mTotalFlds STEP 1
*!*				SELECT (mLastCursor)
*!*				mFieldName = IIF(mLOR='L','L','R')+ALLT(FIELD(I))
*!*				mFieldVal = EVALUATE(FIELD(I))
*!*				SELECT (CurName)
*!*				IF INLIST(mFieldName,mLOR+'AC_NAME2',mLOR+'ORDERLEVEL',mLOR+'LEVEL',mLOR+'MAINFLG',mLOR+'AC_ID',mLOR+'AC_GROUP_ID',mLOR+'AC_NAME',mLOR+'GROUP',mLOR+'CLBAL') = .T.
*!*					IF mFieldName = 'RCLBAL'
*!*						REPLACE (mFieldName) WITH IIF((mFieldVal)<0,ABS(mFieldVal),IIF((mFieldVal)>0,-1*(mFieldVal),0))
*!*					ELSE
*!*						REPLACE (mFieldName) WITH (mFieldVal)
*!*					ENDIF
*!*				ENDIF
*!*			ENDFOR
*!*			SELECT (CurName)
*!*			mFieldName = mLOR+'Expand'
*!*			REPLACE (mFieldName) WITH .T.
*!*			mFieldName = mLOR+'Expanded'
*!*			REPLACE (mFieldName) WITH 'N'
*!*			SKIP
*!*			IF EOF()
*!*				APPEND BLANK
*!*			ENDIF
*!*			SELECT (mLastCursor)
*!*			SKIP
*!*		ENDDO
*!*		SET EXACT &Laexact
*!*		SELECT _TBAcMast
*!*		SET FILTER TO
*!*		SELECT(CurName)
*!*	CATCH TO oErr1
*!*		THISFORM.errorhandler(oErr1)
*!*	ENDTRY


