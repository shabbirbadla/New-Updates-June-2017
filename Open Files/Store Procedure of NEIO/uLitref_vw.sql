If Exists (Select [Name] From SysObjects Where xtype='V' and [name]='uLitref')
Begin
	Drop view uLitref
End
Go
create View [dbo].[uLitref]
as
Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From ARitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From BPitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From BRitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From CPitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From EPitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From EQitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From ESitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From IIitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From IPitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From IRitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From OBitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From OPitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From OSitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From POitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From PRitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From PTitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From SOitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From SQitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From SRitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From SSitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From STitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From TRitref
union all Select entry_ty,date,doc_no,rentry_ty,rqty,item,ware_nm,Itref_tran,Tran_cd,Itserial,Rdate,Rdoc_no,Ritserial,It_code,CompId,rinv_sr,rinv_no,rl_yn From itref
