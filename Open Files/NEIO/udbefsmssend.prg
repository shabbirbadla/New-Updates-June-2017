Parameters vDataSessionId

Local lcsqlconobj
*** Added by Sachin N. S. on 27/09/2014 for Bug-22077 -- Start
Local lcMobileNo1,lcMobileNo2
lcMobileNo1=""
lcMobileNo2=""
*** Added by Sachin N. S. on 27/09/2014 for Bug-22077 -- End

nHandle=0
If Used('Main_vw') And Type('main_vw.ac_id')='N'
	lcsqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
	sq1="select mobile from ac_mast where ac_id=?main_vw.ac_id"
	nRetval = lcsqlconobj.dataconn([EXE],company.dbname,sq1,"PartyMobNo","nHandle",vDataSessionId)
	If nRetval<0
		=Messagebox("Party Mobile No Fetch error...",48,vuMess)
		nRetval=lcsqlconobj.SqlConnClose("nHandle")
		If nRetval<0
			=Messagebox("SQL disconnect error"+Chr(13)+Proper(Message()),48,vuMess)
		Endif
		Return .F.
	Endif

	If Used('PartyMobNo')
***** Added by Sachin N. S. on 27/09/2014 for Bug-22077 -- Start
		If !Empty(CurrentSMSPoint_vw.Mobile_No)
			lcMobileNo1 = Alltrim(CurrentSMSPoint_vw.Mobile_No)+Iif(!Empty(CurrentSMSPoint_vw.Mobile_No),',','')
			lcMobileNo2 = Alltrim(PartyMobNo.mobile)+Iif(!Empty(PartyMobNo.mobile),',','')
			lnCnt=OCCURS(',',lcMobileNo1)
			ll=1
			Do While .T.
				If lnCnt>=ll
					If At(',',lcMobileNo1,ll)>0
						lcMob = Left(lcMobileNo1,At(',',lcMobileNo1)-1)
						If (lcMob $ lcMobileNo2)
							lcMobileNo2 = Strtran(lcMob+',',lcMobileNo2,'')
						Endif
					Else
						Exit
					ENDIF
					ll=ll+1
				Else
					Exit
				Endif
			ENDDO
			lcMobileNo1=Substr(lcMobileNo1,1,Rat(',',lcMobileNo1)-1)
			lcMobileNo2=Substr(lcMobileNo2,1,Rat(',',lcMobileNo2)-1)
		Endif
***** Added by Sachin N. S. on 27/09/2014 for Bug-22077 -- End

		Select CurrentSMSPoint_vw
*!*			Replace CurrentSMSPoint_vw.Mobile_No With PartyMobNo.mobile In CurrentSMSPoint_vw
		Replace CurrentSMSPoint_vw.Mobile_No With lcMobileNo1+Iif(!EMPTY(lcMobileNo2),',','')+lcMobileNo2 In CurrentSMSPoint_vw
	Endif

Endif
