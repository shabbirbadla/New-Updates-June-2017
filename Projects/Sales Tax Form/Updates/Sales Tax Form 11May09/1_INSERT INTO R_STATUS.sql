if not exists(select * from r_status where rep_nm='FRMLIST' and [desc]='Sales Tax Form List')
begin
INSERT INTO [R_STATUS]
([group],[desc],[rep_nm],[rep_nm1] --1
 ,[defa],[ismethod],[isfr_date],[isto_date] ,[isfr_ac],[isto_ac],[isfr_item],[isto_item] --2
 ,[isfr_amt],[isto_amt] ,[isdept] ,[iscategory] ,[iswarehous],[isinvseri] --3
 ,[vou_type],[spl_condn],[qTable],[runbefore],[runafter] --4
 ,[isfr_area],[isto_area],[dontshow],[methodof],[repfcondn],[repfcodn] --5
 ,[e_],[x_],[i_],[o_],[n_],[r_],[t_],[b_] --6
 ,[ac_group],[it_group],[RetTable],[SQLQUERY] --7
 ,[ZOOMTYPE],[spWhat]) --8
 VALUES
 ( 'Sales Tax Form','Sales Tax Form List','FRMLIST','' --1
 ,0,0,1,1 ,0,0,0,0 --2
 ,0,0,0 ,0,0,0  --3
 ,'','','','','' --4
 ,'','',0,'',0,0 --5
 ,0,0,0,0,1,0,0,0 --6
 ,'','','_FRMLIST','EXECUTE USP_REP_SALESTAX_FORM ;AC_NAME' --7
 ,'','P') --8
end
GO

if not exists(select * from r_status where rep_nm='FRMLISTI' and [desc]='Sales Tax Form To be Issued List')
begin
INSERT INTO [R_STATUS]
([group],[desc],[rep_nm],[rep_nm1] --1
 ,[defa],[ismethod],[isfr_date],[isto_date] ,[isfr_ac],[isto_ac],[isfr_item],[isto_item] --2
 ,[isfr_amt],[isto_amt] ,[isdept] ,[iscategory] ,[iswarehous],[isinvseri] --3
 ,[vou_type],[spl_condn],[qTable],[runbefore],[runafter] --4
 ,[isfr_area],[isto_area],[dontshow],[methodof],[repfcondn],[repfcodn] --5
 ,[e_],[x_],[i_],[o_],[n_],[r_],[t_],[b_] --6
 ,[ac_group],[it_group],[RetTable],[SQLQUERY] --7
 ,[ZOOMTYPE],[spWhat]) --8
 VALUES
 ( 'Sales Tax Form','Sales Tax Form To be Issued List','FRMLISTI','' --1
 ,0,0,1,1 ,0,0,0,0 --2
 ,0,0,0 ,0,0,0  --3
 ,'','','','','' --4
 ,'','',0,'',0,0 --5
 ,0,0,0,0,1,0,0,0 --6
 ,'','','_FRMLIST','EXECUTE USP_REP_SALESTAX_FORM ;AC_NAME' --7
 ,'','P') --8
end
GO

if not exists(select * from r_status where rep_nm='FRMLISTR' and [desc]='Sales Tax Form To be Received List')
begin
INSERT INTO [R_STATUS]
([group],[desc],[rep_nm],[rep_nm1] --1
 ,[defa],[ismethod],[isfr_date],[isto_date] ,[isfr_ac],[isto_ac],[isfr_item],[isto_item] --2
 ,[isfr_amt],[isto_amt] ,[isdept] ,[iscategory] ,[iswarehous],[isinvseri] --3
 ,[vou_type],[spl_condn],[qTable],[runbefore],[runafter] --4
 ,[isfr_area],[isto_area],[dontshow],[methodof],[repfcondn],[repfcodn] --5
 ,[e_],[x_],[i_],[o_],[n_],[r_],[t_],[b_] --6
 ,[ac_group],[it_group],[RetTable],[SQLQUERY] --7
 ,[ZOOMTYPE],[spWhat]) --8
 VALUES
 ( 'Sales Tax Form','Sales Tax Form To be Received List','FRMLISTR','' --1
 ,0,0,1,1 ,0,0,0,0 --2
 ,0,0,0 ,0,0,0  --3
 ,'','','','','' --4
 ,'','',0,'',0,0 --5
 ,0,0,0,0,1,0,0,0 --6
 ,'','','_FRMLISTR','EXECUTE USP_REP_SALESTAX_FORM ;AC_NAME' --7
 ,'','P') --8
end
GO

if not exists(select * from r_status where rep_nm='FRMLISTL' and [desc]='Sales Tax Form Remainder Letter')
begin
INSERT INTO [R_STATUS]
([group],[desc],[rep_nm],[rep_nm1] --1
 ,[defa],[ismethod],[isfr_date],[isto_date] ,[isfr_ac],[isto_ac],[isfr_item],[isto_item] --2
 ,[isfr_amt],[isto_amt] ,[isdept] ,[iscategory] ,[iswarehous],[isinvseri] --3
 ,[vou_type],[spl_condn],[qTable],[runbefore],[runafter] --4
 ,[isfr_area],[isto_area],[dontshow],[methodof],[repfcondn],[repfcodn] --5
 ,[e_],[x_],[i_],[o_],[n_],[r_],[t_],[b_] --6
 ,[ac_group],[it_group],[RetTable],[SQLQUERY] --7
 ,[ZOOMTYPE],[spWhat]) --8
 VALUES
 ( 'Sales Tax Form','Sales Tax Form Remainder Letter','FRMLISTL','' --1
 ,0,0,1,1 ,0,0,0,0 --2
 ,0,0,0 ,0,0,0  --3
 ,'','','','','' --4
 ,'','',0,'',0,0 --5
 ,0,0,0,0,1,0,0,0 --6
 ,'','','_FRMLISTL','EXECUTE USP_REP_SALESTAX_FORM ;AC_NAME' --7
 ,'','P') --8
end
GO
