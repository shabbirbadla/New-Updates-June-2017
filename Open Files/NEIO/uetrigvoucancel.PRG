PARAMETERS vBEFORE
SET DATASESSION TO _Screen.Activeform.DataSessionId
&&-->Ipop(Rup)
IF USED('projectitref_vw')
	USE IN projectitref_vw
ENDIF
&&<--Ipop(Rup)
If Used('Gen_SrNo_Vw')
	USE IN Gen_SrNo_Vw
ENDIF
	