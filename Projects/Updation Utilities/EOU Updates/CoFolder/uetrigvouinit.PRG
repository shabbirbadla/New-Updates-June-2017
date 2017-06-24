IF INLIST(.pcvtype,'IP','ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','BP','CP','JV') &&changed by Ajay Jaiswal - Activate Excise Detail Button. Related to exbtn.vcx in Vouclass.scx
	&& Inlist(.pcVtype,'EoldP','SoldB','ST','PT','VI','IP','OP','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD') &&Rup: Activate Excise Detail Button. Related to exbtn.vcx in Vouclass.scx
	IF "VOUCLASS" $ UPPER(SET('classlib'))
	ELSE
		SET CLASSLIB TO vouclass ADDITIVE
	ENDIF
	.ADDOBJECT("cmdexdetail","VOUCLASS.cmdexbtn")
	.cmdexdetail.VISIBLE=.T.
	.cmdexdetail.ENABLED=.T.
	.cmdexdetail.PICTURE=apath+"bmp\additional_info.gif"
	.cmdexdetail.PICTUREPOSITION=1
	.cmdexdetail.HEIGHT=.cmdpickup.HEIGHT
	.cmdexdetail.WIDTH=.cmdnarration.WIDTH	&&+30
	.cmdexdetail.TOP = .cmdnarration.TOP+.cmdnarration.HEIGHT+5
	.cmdexdetail.LEFT = .cmdnarration.LEFT &&-.cmdexdetail.width-5
	.cmdexdetail.SPECIALEFFECT=2
	.ADDPROPERTY("exclicked",.F.)
	DO CASE
		CASE INLIST(.pcvtype,'ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','JV')&&changed by Ajay Jaiswal
			*/Case Inlist(.pcVtype,'ST','PT','VI','IP','OP','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD')
			.cmdexdetail.CAPTION = '\<Excise'
		CASE INLIST(.pcvtype,'BP','CP') &&-->Rup 16/06/2009 TDS Payment Entry
			IF ([vutds] $ vchkprod) &&-->Rup 02/08/2009
				.cmdexdetail.CAPTION = '\<TDS Details'
				.cmdexdetail.WIDTH=.cmdnarration.WIDTH
				.cmdexdetail.AUTOSIZE=.T.&&<--Rup 16/06/2009 TDS Payment Entry
			ELSE
				.cmdexdetail.VISIBLE=.f.				
			ENDIF
		OTHERWISE
			.cmdexdetail.CAPTION = '\<Other Details'
			.cmdexdetail.AutoSize= .T.
	ENDCASE
	IF (.pcvtype="JV" AND !([vuser] $ vchkprod)) &&Rup 13Sep09
		.cmdexdetail.VISIBLE=.F.
	ENDIF 

ENDIF

&&-->Ipop(Rup)
IF INLIST(.pcvtype,'IP','OP') AND (([vuexc] $ vchkprod) OR ([vuinv] $ vchkprod))
	IF "VOUCLASS" $ UPPER(SET('classlib'))
	ELSE
		SET CLASSLIB TO vouclass ADDITIVE
	ENDIF
	.ADDOBJECT("cmdBom","VOUCLASS.cmdBom")
	.cmdbom.PICTURE = apath+IIF(INLIST(.pcvtype,'ST','OP'),'bmp\finish_item.gif','bmp\raw_material.gif')
	.cmdbom.CAPTION=IIF(INLIST(.pcvtype,'IP','OP'),' Work Order','BOM')
	.cmdbom.VISIBLE=.T.
	.cmdbom.ENABLED=.T.
	.cmdbom.WIDTH=.cmdnarration.WIDTH
	.cmdbom.TOP = .cmdnarration.TOP+.cmdnarration.HEIGHT+60
	.cmdbom.LEFT = .cmdnarration.LEFT
	.cmdbom.SPECIALEFFECT=2
ENDIF
&&<--Ipop(Rup)

