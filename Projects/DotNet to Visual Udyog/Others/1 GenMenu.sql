

declare @Bug_2270 numeric
execute [USP_Chk_ComMenuRange] 15502,@rrange= @Bug_2270 output print @Bug_2270 
IF NOT EXISTS(SELECT [PadName],[BarName] FROM Com_Menu WHERE PadName = 'ACCOUNTREPORTS' AND BarName = 'SHOWTRANSACTIONS')
	BEGIN INSERT INTO Com_Menu([RANGE],[PADNAME],[PADNUM],[BARNAME],[BARNUM],[PROMPNAME],[NUMITEM],[HOTKEY],[PROGNAME],[E_],[N_],[R_],[T_],[I_],[O_],[B_],[X_],[MENUTYPE],[ISACTIVE],[PUSER],[MDEFAULT],[LABKEY],[SKIPFOR],[CPROG],[MARK],[XTVS_],[DSNI_],[MCUR_],[TDS_]) 
		VALUES (@Bug_2270,'ACCOUNTREPORTS',0,'SHOWTRANSACTIONS',1,'Show Transactions',0,'','DO UdCallExtProg.App WITH "UdShowTransactions.exe",Allt(apath),Set("Path"),Company.Sta_dt,Company.End_dt, "^'+rtrim(cast(@Bug_2270 as varchar))+'"',0,0,0,0,0,0,0,0,'Report',0,'Show Transactions',0,'','','',0,0,0,0,0) END 

