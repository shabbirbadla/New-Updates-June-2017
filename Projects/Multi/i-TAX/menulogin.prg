LPARAMETERS oFormRef, getMenuName
LOCAL pMenuName, nTotPops, a_menupops, pTypeParm2, pSaveFormName
IF TYPE("m.oFormRef") # "O" OR ;
  LOWER(m.oFormRef.BaseClass) # 'form' OR ;
  m.oFormRef.ShowWindow # 2
	MESSAGEBOX([This menu can only be called from a Top-Level form. Ensure that your form's ShowWindow property is set to 2. Read the header section of the menu's MPR file for more details.])
	RETURN
ENDIF
m.pTypeParm2 = TYPE("m.getMenuName")
m.pMenuName = "FRMLOGINUSERS"
m.pSaveFormName = m.oFormRef.Name
IF m.pTypeParm2 = "C" OR (m.pTypeParm2 = "L" AND m.getMenuName)
	m.oFormRef.Name = m.pMenuName
ENDIF

IF m.pTypeParm2 = "C" AND !EMPTY(m.getMenuName)
	m.pMenuName = m.getMenuName
ENDIF

DEFINE MENU (m.pMenuName) IN (m.oFormRef.Name) BAR

DEFINE PAD _1xu10qdjv OF (m.pMenuName) PROMPT "\<System" COLOR SCHEME 3 ;
	KEY ALT+F, ""
DEFINE PAD _1xu10qdjw OF (m.pMenuName) PROMPT "\<Display" COLOR SCHEME 3 ;
	KEY ALT+V, ""
ON PAD _1xu10qdjv OF (m.pMenuName) ACTIVATE POPUP system
ON PAD _1xu10qdjw OF (m.pMenuName) ACTIVATE POPUP display

DEFINE POPUP system MARGIN RELATIVE SHADOW COLOR SCHEME 4
DEFINE BAR 1 OF system PROMPT "E\<xit"

DEFINE POPUP display MARGIN RELATIVE SHADOW COLOR SCHEME 4
DEFINE BAR 1 OF display PROMPT "Large Icons"
DEFINE BAR 2 OF display PROMPT "Small Icons"
DEFINE BAR 3 OF display PROMPT "List"
DEFINE BAR 4 OF display PROMPT "Details"
DEFINE BAR 5 OF display PROMPT "\-"
DEFINE BAR 6 OF display PROMPT "Refresh"

ACTIVATE MENU (m.pMenuName) NOWAIT

return 