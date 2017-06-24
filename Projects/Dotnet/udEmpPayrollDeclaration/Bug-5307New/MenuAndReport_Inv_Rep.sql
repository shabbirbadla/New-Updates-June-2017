Declare @RangeEmp3 numeric,@RangeTrn1 numeric


execute [USP_Chk_ComMenuRange] 14219,@rrange= @RangeTrn1 output print @RangeTrn1
IF NOT EXISTS(SELECT [PadName],[BarName] FROM Com_Menu WHERE PadName = 'HRANDPAYROLLTRANSACTIONS' AND BarName = 'PAYROLLDECLARATION') BEGIN INSERT INTO Com_Menu([RANGE],[PADNAME],[PADNUM],[BARNAME],[BARNUM],[PROMPNAME],[NUMITEM],[HOTKEY],[PROGNAME],[E_],[N_],[R_],[T_],[I_],[O_],[B_],[X_],[MENUTYPE],[ISACTIVE],[PUSER],[MDEFAULT],[LABKEY],[SKIPFOR],[CPROG],[MARK],[XTVS_],[DSNI_],[MCUR_],[TDS_],[HRPAY_]) VALUES (@RangeTrn1,'HRANDPAYROLLTRANSACTIONS',0,'PAYROLLDECLARATION',6,'Payroll Declaration',3,'','DO udEmpTransApp WITH "PAYROLLDECLARATION", "^'+rtrim(cast(@RangeEmp3 as varchar))+'"',0,0,0,0,0,0,0,0,'Transaction',0,'Payroll Declaration',0,'','','',0,0,0,0,0,1) END 

if not exists(select [desc] from r_status where  Rep_Nm='PayDecDet')
begin
	INSERT INTO r_status([group], [desc], [rep_nm], [rep_nm1], [defa], [ismethod], [isfr_date], [isto_date], [isfr_ac], [isto_ac], [isfr_item], [isto_item], [isfr_amt], [isto_amt], [isdept], [iscategory], [iswarehous], [isinvseri], [vou_type], [spl_condn], [qTable], [runbefore], [runafter], [isfr_area], [isto_area], [dontshow], [methodof], [repfcondn], [repfcodn], [e_], [x_], [i_], [o_], [n_], [r_], [t_], [b_], [ac_group], [it_group], [RetTable], [SQLQUERY], [ZOOMTYPE], [spWhat], [xtvs_], [dsni_], [mcur_], [retfilenm], [tds_], [DISPORD],[HRPay_],PgBrakFld,ExpFileNm,EMailSub,EmailBody) 
	VALUES ('Employee Payroll Declaration', 'Employee Payroll Declaration', 'PayDecDet', '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', '', 'UETRIG_EMPDET.PRG with "PayDec",thisform.DATASESSIONID', '', 0, 0, 0, '', 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, '', '', '_PayDec', 'Execute Usp_Rep_Emp_Payroll_Declaration;Ac_Name', ' ', 'P', 0, 0, 0, '', 0, 1,1,'EmployeeCode,EmployeeName','Payroll Declaration','Payroll Declaration for the Year @@Year','Dear @@EmployeeName,'+char(13)+char(13)+'Please find your Payroll Declaration for the Year of @@Year.  '+char(13)+char(13)+'Thanks and Regards,'+char(13)+'@@USerName')
end

update Com_menu set BarName='PAYROLLDECLARATIONMASTER',PrompName='Payroll Declaration Master',ProgName=Replace(ProgName,'SECTION','PAYROLL DECLARATION') Where BarName='PAYROLLSECTIONMASTER'
update Com_menu set BarName='PAYROLLDECLARATIONMASTERRREPORT',PrompName='Payroll Declaration Master',ProgName=Replace(ProgName,'PAYROLL SECTION MASTER','PAYROLL DECLARATION MASTER') Where BarName='PAYROLLSECTIONMASTERRREPORT'

update R_Status set [Group]='PAYROLL DECLARATION MASTER',[Desc]='Payroll Declaration Master',Rep_Nm='EmpDec',SqlQuery='EXECUTE USP_REP_Emp_Payroll_Declaration_Master  ;Ac_Name' where Rep_Nm='EmpSec' and [Group]='PAYROLL SECTION MASTER' and [Desc]='Payroll Section Master'

