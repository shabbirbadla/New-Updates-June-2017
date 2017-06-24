LPARAMETERS m_regfilenm,m_regsearchcond
Installer  =CreateObject("WindowsInstaller.Installer")
m_Regasmpath = ''
For Each productCode In installer.Products 
	m_SearchCond = installer.ProductInfo(productCode, "ProductName")
	IF UPPER(m_regsearchcond) $ UPPER(m_SearchCond)
		_newprodcode  = productcode
		components = installer.Components
		For Each component In components
		  	clients = installer.ComponentClients(component)
		  	For Each client In Clients            
		  		If client = _newprodcode Then                
		  			apath = installer.ComponentPath(_newprodcode, component)
		  			IF ISNULL(APATH) = .F.
			  			IF RIGHT(UPPER(apath),LEN(m_regfilenm)) = UPPER(m_regfilenm)
			  				m_Regasmpath  = apath
			  				exit
			  			ENDIF 
			  		ENDIF 	
		  		 ENDIF
		  	Next 
		NEXT
	ENDIF
NEXT
RELEASE Installer
retu m_Regasmpath
