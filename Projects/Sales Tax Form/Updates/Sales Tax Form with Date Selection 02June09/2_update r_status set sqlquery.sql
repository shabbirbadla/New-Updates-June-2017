update r_status set sqlquery='EXECUTE USP_REP_SALESTAX_FORM ;AC_NAME' where rep_nm='FRMLIST' and [desc]='Sales Tax Form List'
update r_status set sqlquery='EXECUTE USP_REP_SALESTAX_FORM_TOBE_ISSUED ;AC_NAME' where rep_nm='FRMLISTI' and [desc]='Sales Tax Form To be Issued List'
update r_status set sqlquery='EXECUTE USP_REP_SALESTAX_FORM_TOBE_RECEIVED ;AC_NAME' where rep_nm='FRMLISTR' and [desc]='Sales Tax Form To be Received List'
update r_status set sqlquery='EXECUTE USP_REP_SALESTAX_FORM_TOBE_ISSUED ;AC_NAME' where rep_nm='FRMLISTL' and [desc]='Sales Tax Form Remainder Letter'