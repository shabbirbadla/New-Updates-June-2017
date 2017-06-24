local objXml as msxml2.DOMDocument30, mNode
Local mXmlFile, mret
Store "" To mXmlFile
Store .f. to mret
do setenv()
do openfiles()

objXml = createObject("Msxml2.DOMDocument30")

Select Interface
mXmlFile = Interface.xmlFileNm

Scan  While XMLFileNm = mXmlFile
	
endscan







return

*-----------------*
Procedure openfiles
*-----------------*
sele 0
use dbfs\InterFace
return

*-----------------*
PROCEDURE setEnv
*-----------------*
Set Talk Off 
Set Notify off
return





LOCAL objXML AS MSXML2.DOMDocument.3.0

objXML = CREATEOBJECT("MSXML2.DOMDocument.3.0")

*-- Very basic routine to read XML document
*-- and Create a Cursor with the Data.
*-- Due to the fact we don't have a Schema - create all character fields.

*-- Load the XML file
objXML.load("D:\Projects\XMLtoDbf\XmlFiles\Customer.xml")
SET STEP ON 
IF objXML.documentElement.childNodes.length = 0
	=MESSAGEBOX("Empty XML Document",0,"Info")
	RETURN
ENDIF 
SET STEP ON 
*-- Obtain Fields for Structure.		 Fields Structure for different fileds defined here
WITH objXML.documentElement.childNodes 
	FOR iParent = 0 TO 0	&& Get One Line
		lcCursor = "cur" + STRTRAN(ALLTRIM(.item(iParent).NodeName),' ')
		IF .item(iParent).HasChildNodes
			FOR iChild = 0 TO .item(iParent).ChildNodes.Length - 1
				DIMENSION laStruct[iChild+1,16]	&& VFP 6/7 has 16 Elements for AFields, VFP 8 has 18
				laStruct[iChild+1,1] = .item(iParent).ChildNodes(iChild).NodeName
			ENDFOR
		ENDIF
	ENDFOR
ENDWITH 

*-- No Data so don't create.
IF EMPTY(laStruct[1,1])
	MESSAGEBOX("No records found",64)		&& Andy
	RETURN
ENDIF 

*-- Build the Array Structure
FOR lnI = 1 TO ALEN(laStruct,1)
	laStruct[lnI,2] = 'C'
	laStruct[lnI,3] = 100		&& Create Big Enough to hold data.
	laStruct[lnI,4] = 0
	laStruct[lnI,5] = .F.
	laStruct[lnI,6] = .F.
	FOR lnX = 7 TO 16
		laStruct[lnI,lnX] = SPACE(0)
	ENDFOR
	laStruct[lnI,12] = ALLTRIM(UPPER(lcCursor))
ENDFOR 

*-- Create the Cursor
CREATE CURSOR (lcCursor) FROM ARRAY laStruct
SELECT (lcCursor)

lnFields = AFIELDS( laFields, lcCursor )
DIMENSION laRow[1,lnFields]

WITH objXML.DocumentElement.ChildNodes
	FOR iParent = 0 TO .Length - 1
		laRow = SPACE(0)
		IF .Item(iParent).HasChildNodes				&& Should be the Data and Structure
			iFCount = 1
			FOR EACH oChild IN .Item(iParent).ChildNodes
				lcValue = oChild.Text
				laRow[1,iFCount] = lcValue
				iFCount = iFCount + 1 
				oChild = .NULL.
			ENDFOR
			INSERT INTO (lcCursor) FROM ARRAY laRow
		ENDIF 
	ENDFOR 
ENDWITH 

return





clear
mfileNm = "D:\Projects\XMLtoDbf\test.xml"		&&XmlFiles\Customer.xml"
ObjAdapter=CREATEOBJECT("XMLAdapter")
ObjAdapter.loadXml(mfileNm,.t.)
SET step on
FOR EACH objTable IN ObjAdapter.Tables
	objTable.tocursor()
endfor
*!*objAdapter.Tables(1).tocursor()

return




