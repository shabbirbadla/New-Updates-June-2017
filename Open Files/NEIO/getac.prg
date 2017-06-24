		nret=0
		_curfrmobj = _screen.ActiveForm 

		sqlstr1="select u_onac from department where department.dept = ?main_vw.dept"
		sqlstr2=""
		sqlstr3=""
		sqlstr4=""
	
		nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1,[tmp_ac_name_vw],;
				"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
	
		If nRet > 0 AND USED('tmp_ac_name_vw')
				RETURN tmp_ac_name_vw.u_onac
		ELSE
			nRet = 0
		Endif	



