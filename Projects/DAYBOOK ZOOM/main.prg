*** 
*** ReFox X  #UK933629  MANRIQUE ORELLANA  MANSOFT SYSTEMS [VFP80]
***
PARAMETER zoomintype,  ;
          vdatasessionid
SET DATASESSION TO vdatasessionid
IF  .NOT. ('CONFA2.VCX' $  ;
    UPPER(SET("Classlib")))
     SET CLASSLIB TO confa2.vcx ADDITIVE
ENDIF
lcsqlstr = 'Select distinct a.Date,a.Tran_Cd,a.Entry_ty,a.inv_no,a.inv_sr,a.net_amt,a.u_pinvno'
lcsqlstr = RTRIM(lcsqlstr) + ' ' +  ;
           ',d.ac_name,Ac_Name1=d.Ac_Name,Amount=a.net_amt,g.code_nm'
lcsqlstr = RTRIM(lcsqlstr) + ' ' +  ;
           ',a.Dept,a.cate'
lcsqlstr = RTRIM(lcsqlstr) + ' ' +  ;
           'From Stkl_Vw_Main a '
lcsqlstr = RTRIM(lcsqlstr) + ' ' +  ;
           'Inner Join Ac_Mast d on (a.Ac_Id=d.Ac_Id)'
lcsqlstr = RTRIM(lcsqlstr) + ' ' +  ;
           'Inner Join Lcode g on (g.ENTRY_TY=a.Entry_ty)'
lcsqlstr = RTRIM(lcsqlstr) + ' ' +  ;
           ' WHERE a.Date >= ?_tmpvar.sdate and a.date <= ?_tmpvar.edate'
DO FORM uedaybookzoom WITH  ;
   lcsqlstr, vdatasessionid
ENDPROC
**
*** 
*** ReFox - retrace your steps ... 
***
