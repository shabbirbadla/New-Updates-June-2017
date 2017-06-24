*IF !([vuent] $ vchkprod) AND INLIST(.pcvtype,'IP','ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','BP','CP','JV') OR INLIST(.pcvtype,'J2','J3','B3','B4')&&Sandeep S. for TKT-7147 ON 05/04/2011
If Inlist(.pcvtype,'IP','ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','BP','CP','JV') Or Inlist(.pcvtype,'J2','J3','B3','B4')&&Sandeep S. for TKT-7147 ON 05/04/2011
&& IF INLIST(.pcvtype,'IP','ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','BP','CP','JV') OR INLIST(.pcvtype,'J2','J3','B3','B4')&&Rup: entry_ty=J2 TKT-2647 &&TKT-794 GTA Add J3 &&TCS TKT-5692 Add B3,B4
&&IF INLIST(.pcvtype,'IP','ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','BP','CP','JV') &&changed by Ajay Jaiswal - Activate Excise Detail Button. Related to exbtn.vcx in Vouclass.scx
&& Inlist(.pcVtype,'EoldP','SoldB','ST','PT','VI','IP','OP','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD') &&Rup: Activate Excise Detail Button. Related to exbtn.vcx in Vouclass.scx
	If "VOUCLASS" $ Upper(Set('classlib'))
	Else
		Set Classlib To vouclass Additive
	Endif
	.AddObject("cmdexdetail","VOUCLASS.cmdexbtn")
	.cmdexdetail.Visible=.T.
	.cmdexdetail.Enabled=.T.
	.cmdexdetail.Picture=apath+"bmp\additional_info.gif"
	.cmdexdetail.PicturePosition=1
	.cmdexdetail.Height=.cmdpickup.Height
	.cmdexdetail.Width=.cmdnarration.Width	&&+30
	.cmdexdetail.Top = .cmdnarration.Top+.cmdnarration.Height+5
	.cmdexdetail.Left = .cmdnarration.Left &&-.cmdexdetail.width-5
	.cmdexdetail.SpecialEffect=2
	.AddProperty("exclicked",.F.)
	Do Case

*		 Case !([vuent] $ vchkprod) AND Inlist(.pcvtype,'ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','JV')&&Sandeep S. for TKT-7147 ON 05/04/2011
	Case  Inlist(.pcvtype,'ST','PT','P1','VI','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD','JV')&&changed by Ajay Jaiswal

*/Case Inlist(.pcVtype,'ST','PT','VI','IP','OP','SR','OS','OB','GI','GR','HI','HR','DP','DR','BI','FP','FR','RP','RR','VR','BC','BD')
*!*			If ([vuent] $ vchkprod) &&Sandeep for TKT-7147
*!*					.cmdexdetail.Visible=.F.
*!*			Endif
		.cmdexdetail.Caption = '\<Excise'
&&--->TKT-2647
	Case Inlist(.pcvtype,'J2','J3') &&TKT-2647 &&TKT-794 GTA Add J3
		.cmdexdetail.Caption = '\<Service Tax'
&&<---TKT-2647
	Case Inlist(.pcvtype,'BP','CP',"B3","B4") &&-->Rup 16/06/2009 TDS Payment Entry &&TCS TKT-5692 Add B3,B4
		If ([vutds] $ vchkprod) &&-->Rup 02/08/2009
			.cmdexdetail.Caption = Iif(Inlist(.pcvtype,"B4"),'\<TCS Details','\<TDS Details') &&TCS TKT-5692 Add B3,B4
			.cmdexdetail.Width=.cmdnarration.Width
			.cmdexdetail.AutoSize=.T.&&<--Rup 16/06/2009 TDS Payment Entry
		Else
			.cmdexdetail.Visible=.F.
		Endif
	Otherwise
		.cmdexdetail.Caption = '\<Other Details'
		.cmdexdetail.AutoSize= .T.
	Endcase
* Birendra : On 10 June 2011 :Start:
	If !([vuexc] $ vchkprod Or  [vuexp] $ vchkprod Or [vutex] $ vchkprod Or [vuser] $ vchkprod)
		.cmdexdetail.Visible=.F.
	Endif
* Birendra : On 10 June 2011 :Start:

*	If (.pcvtype="JV" And !([vuser] $ vchkprod)) &&Rup 13Sep09
	If (.pcvtype="JV" And [vutex] $ vchkprod)  Or (.pcvtype="JV" And !([vuser] $ vchkprod)) && Birendra : On 10 June 2011
		.cmdexdetail.Visible=.F.
	Endif

Endif

&&-->Ipop(Rup)
If Inlist(.pcvtype,'IP','OP') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	If "VOUCLASS" $ Upper(Set('classlib'))
	Else
		Set Classlib To vouclass Additive
	Endif
	.AddObject("cmdBom","VOUCLASS.cmdBom")
	.cmdbom.Picture = apath+Iif(Inlist(.pcvtype,'ST','OP'),'bmp\finish_item.gif','bmp\raw_material.gif')
	.cmdbom.Caption=Iif(Inlist(.pcvtype,'IP','OP'),' Work Order','BOM')
	.cmdbom.Visible=.T.
	.cmdbom.Enabled=.T.
	.cmdbom.Width=.cmdnarration.Width
	.cmdbom.Top = .cmdnarration.Top+.cmdnarration.Height+60
	.cmdbom.Left = .cmdnarration.Left
	.cmdbom.SpecialEffect=2
Endif
&&<--Ipop(Rup)
If 'trnamend' $ vchkprod
	Do VouInit In MainPrg && Birendra : 22 mar 2011
Endif

****** Added By Sachin N. S. on 04/07/2011 for Batchwise/Serialize Inventory ****** Start
If Vartype(oGlblPrdFeat)="O"
	If oGlblPrdFeat.UdChkProd('serialinv')
		If Type('Lcode_vw.lCrtBatInv')='L' And Type('Lcode_vw.lPickBatInv')='L'
			If (Lcode_vw.lCrtBatInv Or Lcode_vw.lPickBatInv) And Lcode_vw.v_Item
				If Type('Thisform._BatchSerialStk')!='O'
					If File('ueSerializeInv.app')
						If !("BATCHSERIALSTK" $ Upper(Set('classlib')))
							Set Classlib To BatchSerialStk In "ueSerializeInv.app" Additive
						Endif
						.AddObject("_BatchSerialStk","BatchSerialStk.BatchSerialStk")
					Endif
				Endif
			Endif
		Endif
	Endif
Endif
****** Added By Sachin N. S. on 04/07/2011 for Batchwise/Serialize Inventory ****** End
