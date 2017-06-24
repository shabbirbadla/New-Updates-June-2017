LPARAMETERS tcDbf_path AS STRING

IF VARTYPE(tcDbf_path) <> 'C'
	tcDbf_path = GETDIR("","Remove Deleted Records Permanently","Choose Database Path",.F.,.F.)
ENDIF
IF !EMPTY(tcDbf_path)
	tcDbf_path = ALLTRIM(tcDbf_path)
	IF SUBSTR(tcDbf_path,LEN(tcDbf_path),1) <> "\"
		tcDbf_path = tcDbf_path+"\"
	ENDIF
	IF ! DIRECTORY(tcDbf_path)
		MESSAGEBOX("Please Valid Path",0,[])
		RETURN .F.
	ENDIF
ENDIF
nTotdbfs = ADIR(aDbfs,tcDbf_path+'*.DBF',"D")
LOCAL lnLoop
FOR lnLoop = 1 TO nTotdbfs STEP 1
	tcDbf_Name = PROPER(STRTRAN(aDbfs(lnLoop,1),".DBF",""))
	IF !EMPTY(tcDbf_Name)
		IF Open_Table(tcDbf_Name,tcDbf_path+PROPER(aDbfs(lnLoop,1)))
			SELECT (tcDbf_Name)
			PACK
		ENDIF
		WAIT WINDOW "Processing: "+tcDbf_Name+" End" NOWAIT
	ENDIF
ENDFOR
CLEAR ALL
CLOSE ALL
QUIT

FUNCTION Open_Table
LPARAMETERS tcDbf_Name AS STRING,tcDbf_PName AS STRING
llReturn = .F.
TRY
	IF !USED(tcDbf_Name)
		SELECT 0
		USE (tcDbf_PName) EXCLUSIVE
		llReturn = .T.
	ENDIF
CATCH TO oErr
FINALLY
ENDTRY

RETURN llReturn
