IF NOT EXISTS(SELECT AC_NAME FROM AC_MAST WHERE AC_NAME = 'BALANCE WITH B17-BOND') BEGIN INSERT INTO Ac_MAST([ac_name], [contact], [add1], [add2], [add3], [Ac_group_id], [group], [City_id], [city], [Ac_type_id], [typ], [zip], [phone], [phoner], [fax], [email], [cr_days], [i_tax], [s_tax], [c_tax], [notes], [type], [t_no], [t_rate], [t_amount], [i_rate], [i_days], [rate_level], [posting], [updown], [up_ledbal], [isgroup], [manadjust], [defa_ac], [cramount], [crallow], [salesman], [coll], [division], [eccno], [range], [cexregno], [bankbr], [bankno], [st_type], [vend_type], [user_name], [sysdate], [sregn], [mailname], [state], [country], [t_pay], [designatio], [tds_accoun], [currency], [mobile], [Type1], [Exregno], [R_commis], [State_id], [Country_id], [Disc_ac], [Apgen], [Apgenby], [CompId], [SERAPL], [SEREXMPTD], [SERNOTI], [SERTY], [Area], [Zone], [COGROUP], [deactfrom], [ldeactive], [tds_code], [ded_type], [svc_cate], [Tds_lIgnRule], [Tds_lDedfrom], [Tds_lDedscto], [TDS_ExempLimit], [TDS_ReasonEL], [bsrcode], [tds_ex_tdt], [tds_ex_fdt], [Tds_Ex_Limit], [Tds_Tp], [Tds_Sc_Tp], [Tds_Ec_Tp], [FBTSEC], [FBEXPPER], [U_DEST], [U_BANK1], [U_BANK2], [U_EXCOMM], [U_INDENT], [U_DISPAT], [U_NOTE], [U_OTREFE], [U_PONO], [U_PADD], [U_PNAME], [U_REFNO], [U_SREGN], [U_SUPREF], [U_DELIV], [U_TERMS], [U_WARRE]) VALUES ('BALANCE WITH B17-BOND', '                                   ', '                                   ', '                                   ', '                                   ', 12, 'CURRENT ASSETS/LOANS & ADVANCES    ', 3, '                      ', 4, '', '       ', '                                   ', '                                   ', '                                   ', '                                                  ', 0, '                                   ', '                                   ', '                                   ', '', 'B', '                                   ', 0.00, 0.000, 0.00, 0, 0, 'Entry by Entry ', 99, 0.00, 0, 0, 0, 0.00, 1, '                                   ', '                    ', '               ', '               ', '               ', '                              ', '                                   ', '                                   ', '              ', '                                   ', '                              ', '                    ', '                                                  ', 'BALANCE WITH B17-BOND', '                              ', '                              ', '                         ', '                                   ', '                                   ', '                    ', '                              ', ' ', ' ', 0, 28, 251, ' ', ' ', ' ', 0, '', 0, '', '', '', '', 'BALANCE WITH B17-BOND', '1/1/1900 12:00:00 AM', 0, '', '', '', 0, 0, 0, 0, '', '', '1/1/1900 12:00:00 AM', '1/1/1900 12:00:00 AM', 0, 0.00, 0.00, 0.00, '0', 0.00, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) END
update lcode set l_narr = 1 where entry_ty in ('BB','BC','BD')
update lcode set defa_cr = 'IIF(!MAIN_VW.U_CHOICE,MAIN_VW.PARTY_NM,"OPENING BALANCES")' where entry_ty in ('BB')
update lcode set defa_db = 'MAIN_VW.PARTY_NM' where entry_ty in ('BC')
update lcode set defa_cr = 'MAIN_VW.PARTY_NM' where entry_ty in ('BD')
update lcode set repo_nm = 'ct3_',i_disc = 1,l_narr = 1 where entry_ty = 'CT'                
update lcode set defa_group = 'SUNDRY CREDITORS,' where cast(defa_group as varchar(200)) = 'SUNDRY DEBTORS,' and entry_ty = 'CT'
update lcode set pickupfrom = rtrim(ltrim(pickupfrom))+'/PH' where entry_ty = 'AR' and pickupfrom not like '%PH%'
update lcode set pickupfrom = rtrim(ltrim(pickupfrom))+'/CT' where entry_ty = 'AR' and pickupfrom not like '%CT%'
update lcode set i_disc = 1 where entry_ty = 'AR'                

--update com_menu set progname = replace(progname,'DO UEVOUCHER WITH "BD","","","","","",0,.T.,','DO UEVOUCHER WITH "BD","BALANCE WITH B17-BOND","","","","B17 BOND DEBIT",0,.T.,') where barname = 'B17BONDDEBIT'
--update com_menu set progname = replace(progname,'DO UEVOUCHER WITH "BC","","","","","",0,.T.,','DO UEVOUCHER WITH "BC","BALANCE WITH B17-BOND","","","","B17 BOND CREDIT",0,.T.,') where barname = 'B17BONDCREDIT'
update com_menu set padname = 'PURCHASETRANSACTIONS' where padname = 'EXPORTTRANSACTIONS' and barname = 'CT3'

IF NOT EXISTS(SELECT FLD_NM FROM DCMAST WHERE ENTRY_TY = 'CT' AND CODE = 'E' ) BEGIN 
select * into #dcmastcte from dcmast where entry_ty = 'PT' and code = 'E'
update #dcmastcte set entry_ty = 'CT'
insert into dcmast select * from #dcmastcte
END

IF NOT EXISTS(SELECT FLD_NM FROM DCMAST WHERE ENTRY_TY = 'AR' AND CODE = 'E' ) BEGIN 
select * into #dcmastare from dcmast where entry_ty = 'PT' and code = 'E'
update #dcmastare set entry_ty = 'AR'
insert into dcmast select * from #dcmastare
END

IF NOT EXISTS(SELECT [group] FROM r_status WHERE [group] = 'GOODS RECEIPT' and [desc] = 'FORM D3' and rep_nm = 'AR_VOUD') 
BEGIN 
INSERT INTO r_status([group], [desc], [rep_nm], [rep_nm1], [defa], [ismethod], [isfr_date], [isto_date], [isfr_ac], [isto_ac], [isfr_item], [isto_item], [isfr_amt], [isto_amt], [isdept], [iscategory], [iswarehous], [isinvseri], [vou_type], [spl_condn], [qTable], [runbefore], [runafter], [isfr_area], [isto_area], [dontshow], [methodof], [repfcondn], [repfcodn], [e_], [x_], [i_], [o_], [n_], [r_], [t_], [b_], [ac_group], [it_group], [RetTable], [SQLQUERY], [ZOOMTYPE], [spWhat], [xtvs_], [dsni_], [mcur_], [newgroup]) 
VALUES ('GOODS RECEIPT', 'FORM D3', 'AR_VOUD  ', '          ', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, '                                                                                          ', '', 'ARMAIN         ', '                                                  ', '                    ', 0, 0, 0, '                                                                                                                                                                                                                                                          ', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', '_GRND3VW            ', 'SELECT ARMAIN.ENTRY_TY,ARMAIN.DATE,ARMAIN.U_AR3ANO,ARMAIN.U_AR3ADATE,ARMAIN.INV_NO,ARMAIN.U_PINVNO,ARMAIN.U_PINVDT,
ARMAIN.U_PONO,ARMAIN.U_PODT,ARMAIN.U_CHALNO,ARMAIN.U_CHALDT,ARMAIN.U_DELIVER,ARMAIN.U_VEHNO,ARMAIN.U_LRNO,
ARMAIN.U_INWARD,ARMAIN.U_DELI,ARITEM.ITEM,AC_MAST.AC_NAME,ARITEM.QTY,ARITEM.U_CHALQTY,ARITEM.U_ACQTY,ARITEM.U_RQTY,
CONVERT(VARCHAR(254),ARITEM.NARR) AS  NARR,IT_MAST.P_UNIT,AC_MAST.MAILNAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,
AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.STATE,ARITEM.U_BENO,ARITEM.U_BEDATE,ARITREF.RINV_NO,ARITREF.RDATE 
FROM ARITEM 
LEFT JOIN ARITREF ON (ARITEM.ENTRY_TY=ARITREF.ENTRY_TY AND ARITEM.DATE=ARITREF.DATE AND ARITEM.DOC_NO=ARITREF.DOC_NO AND ARITEM.ITSERIAL=ARITREF.ITSERIAL)
JOIN ARMAIN ON ARITEM.ENTRY_TY=ARMAIN.ENTRY_TY AND ARITEM.TRAN_CD=ARMAIN.TRAN_CD
JOIN IT_MAST ON ARITEM.ITEM=IT_MAST.IT_NAME 
JOIN AC_MAST ON ARITEM.PARTY_NM=AC_MAST.AC_NAME ', ' ', 'Q', 0, 0, 0, ' ')
end

IF NOT EXISTS(SELECT [group] FROM r_status WHERE [group] = 'GOODS RECEIPT' and [desc] = 'Rewarehousing' and rep_nm = 'AR_VOU2') 
BEGIN 
INSERT INTO r_status([group], [desc], [rep_nm], [rep_nm1], [defa], [ismethod], [isfr_date], [isto_date], [isfr_ac], [isto_ac], [isfr_item], [isto_item], [isfr_amt], [isto_amt], [isdept], [iscategory], [iswarehous], [isinvseri], [vou_type], [spl_condn], [qTable], [runbefore], [runafter], [isfr_area], [isto_area], [dontshow], [methodof], [repfcondn], [repfcodn], [e_], [x_], [i_], [o_], [n_], [r_], [t_], [b_], [ac_group], [it_group], [RetTable], [SQLQUERY], [ZOOMTYPE], [spWhat], [xtvs_], [dsni_], [mcur_], [newgroup]) 
VALUES ('GOODS RECEIPT', 'Rewarehousing', 'AR_VOU2  ', '          ', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, '                                                                                          ', '', 'ARMAIN         ', '                                                  ', '                    ', 0, 0, 0, '                                                                                                                                                                                                                                                          ', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, '', '', '_armai              ', 'SELECT ARMAIN.ENTRY_TY,ARITEM.INV_NO,AC_MAST.MAILNAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,IT_MAST.IT_ALIAS
,AC_MAST.STATE,ARITEM.TAX_NAME,ARITEM.TAXAMT,ARITEM.U_BASDUTY,ARITEM.EXAMT,ARMAIN.U_TANO,IT_MAST.[GROUP],IT_MAST.[TYPE],ARITEM.QTY,ARMAIN.NET_AMT,IT_MAST.P_UNIT,ARMAIN.DATE,ARITEM.U_ACMT,ARITEM.U_ACDUTY,ARITEM.U_CESSAMT,ARITEM.U_CESSPER
FROM ARITEM 
INNER JOIN AC_MAST ON (ARITEM.PARTY_NM=AC_MAST.AC_NAME)
INNER JOIN IT_MAST ON (ARITEM.ITEM=IT_MAST.IT_NAME)
INNER JOIN ARMAIN ON (ARITEM.TRAN_CD=ARMAIN.TRAN_CD)', ' ', 'Q', 0, 0, 0, ' ')
end

update lother set data_ty = 'Bit' where e_code = 'PT' and fld_nm = 'U_TYPE' and data_ty = 'L'
update dcmast set dac_name = replace(dac_name,'BALANCE WITH EXCISE RG23A-II','BALANCE WITH EXCISE RG23A')
update dcmast set dac_name = replace(dac_name,'BALANCE WITH EXCISE RG23C-II','BALANCE WITH EXCISE RG23C')
execute add_columns 'it_bal','eiqty numeric (16,4)' 
update rules set validity = rtrim(ltrim(validity))+' EI' where [rule] = 'CAPTIVE USE' and validity not like '%EI%'
update rules set validity = rtrim(ltrim(validity))+' EI' where [rule] = 'CT-1' and validity not like '%EI%'
update rules set validity = rtrim(ltrim(validity))+' EI' where [rule] = 'CT-3' and validity not like '%EI%'
update rules set validity = rtrim(ltrim(validity))+' EI' where [rule] = 'EOU EXPORT' and validity not like '%EI%'
