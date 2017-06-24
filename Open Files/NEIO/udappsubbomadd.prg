Select bomdet_vw
If Reccount()>0
	currBomLvl=bomdet_vw.bomlevel
Endif
Select bomHead_vw
If Reccount()>0
	currHeadRecNo=Recno()
	Locate For bomlevel=currBomLvl-1
	If Found()
		prevBatchNo=Batch_no
	Else
		prevBatchNo=''
	ENDIF
	Go currHeadRecNo
	Replace Batch_no With prevBatchNo
Endif
