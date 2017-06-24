&&Birendra : Get entry type on the base of process master for export data in auto posting:start:
Procedure getentrytypebaseonprocess()
LOCAL zretval
zretval=''
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
IF TYPE('main_vw.Proc_Id')<>'U'
sq1="select autopostto from ProcessMast PM where PM.Proc_ID=?main_vw.Proc_Id and (ISNULL(PM.deactivate,0)=0 or (ISNULL(PM.deactivate,0)=1 and PM.deactfrom<?main_vw.date))"
ENDIF 
IF TYPE('LMC_vw.Proc_Id')<>'U'
sq1="select autopostto from ProcessMast PM where PM.Proc_ID=?Lmc_vw.Proc_Id and (ISNULL(PM.deactivate,0)=0 or (ISNULL(PM.deactivate,0)=1 and PM.deactfrom<?main_vw.date))"
ENDIF 

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"zPM_vw","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
IF USED('zPM_vw')
SELECT zPM_vw
zretval=ALLTRIM(zpm_vw.autopostto)
USE IN zPm_vw
ENDIF 
RETURN zretval
Endproc
&&Birendra : Get entry type on the base of process master for export data in auto posting:End:
