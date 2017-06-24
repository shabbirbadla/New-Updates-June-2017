If Exists (Select Top 1 [Name] From sysobjects Where xtype='P' and [Name]='Usp_rep_EP_Detail_Register')
Begin
	Drop procedure Usp_rep_EP_Detail_Register
End
Go
Create Procedure [dbo].[Usp_rep_EP_Detail_Register]
@sdate SmallDateTime,@edate SmallDateTime
as 

Begin
	Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000)

	Select 
	code_nm=Lcode.code_nm,
	ac_name=b.party_nm,b.u_pinvno,b.u_pinvdt,b.inv_no,b.date,b.serrule,
	b.Serty, Amount=b.net_amt,sAbtAmt=b.net_amt,sTaxable=b.net_amt
	,Serbamt=b.net_amt,Sercamt=b.net_amt,Serhamt=b.net_amt,serrbamt=b.net_amt,serrcamt=b.net_amt
	,serrhamt=b.net_amt,b.SerAvail,serRecAmt=b.net_amt,b.Entry_ty,b.Tran_cd
	Into #epreg From BPMAIN b 
	Inner Join LCODE on (LCODE.Entry_ty=b.entry_ty) Where 1=2
	
	set @sqlcommand='insert into #epreg (code_nm,ac_name,u_pinvno,u_pinvdt,inv_no,date,serrule,'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Serty, Amount,sAbtAmt,sTaxable,Serbamt,Sercamt,Serhamt,serrbamt,serrcamt,serrhamt,SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',serRecAmt,Entry_ty,Tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select Case When l.Entry_ty In (''BP'',''CP'') Or  l.Bcode_nm In (''BP'',''CP'') then ''Un-billed Advance Payment'' else l.code_nm End' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Ac_mast.Ac_name,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,m.serrule'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.Serty, ac.Amount,ac.sAbtAmt,ac.sTaxable,ac.Serbamt,ac.Sercamt,ac.Serhamt,ac.serrbamt,ac.serrcamt,ac.serrhamt,ac.SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',sum(isnull(isd.Amount,0)),isd.aentry_ty,isd.atran_cd'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' from (Select aeNTRY_TY,ATran_cd,Serty,Amount=SUM(Amount) from ISDAllocation Group By aEntry_ty,aTran_Cd,Serty) isd '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join SerTaxMain_vw m on (isd.aentry_ty=m.entry_ty and isd.atran_cd=m.tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode lisd on (isd.aentry_ty=lisd.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Inner Join Ac_mast on (Ac_mast.Ac_id=m.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join acdetalloc ac on (isd.aentry_ty=ac.entry_ty and isd.atran_cd=ac.tran_cd  and isd.serty=ac.serty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Where m.Date Between '''+convert(Varchar(50),@sdate)+''' and '''+Convert(Varchar(50),@edate)+''''
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and (lisd.entry_ty in (''BP'',''CP'',''EP'') or lisd.bcode_nm in (''BP'',''CP'',''EP''))'
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' and (l.entry_ty in (''BP'',''CP'') or l.bcode_nm in (''BP'',''CP''))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'group by l.Code_nm,Ac_mast.Ac_name,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,m.serrule'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.Serty, ac.Amount,ac.sAbtAmt,ac.sTaxable,ac.Serbamt,ac.Sercamt,ac.Serhamt,ac.serrbamt,ac.serrcamt,ac.serrhamt,ac.SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',isd.amount,isd.aentry_ty,isd.atran_cd,l.entry_ty,l.bcode_nm'
	print  @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	set @sqlcommand='insert into #epreg (code_nm,ac_name,u_pinvno,u_pinvdt,inv_no,date,serrule,'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Serty, Amount,sAbtAmt,sTaxable,Serbamt,Sercamt,Serhamt,serrbamt,serrcamt,serrhamt,SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',serRecAmt,Entry_ty,Tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select Case When l.Entry_ty In (''BP'',''CP'') Or  l.Bcode_nm In (''BP'',''CP'') then ''Un-billed Advance Payment'' else l.code_nm End' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Ac_mast.Ac_name,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,m.serrule' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.Serty, ac.Amount,ac.sAbtAmt,ac.sTaxable,ac.Serbamt,ac.Sercamt,ac.Serhamt,ac.serrbamt,ac.serrcamt,ac.serrhamt,ac.SerAvail' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',0,ac.entry_ty,ac.tran_cd'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' from acdetalloc ac '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join SerTaxMain_vw m on (ac.entry_ty=m.entry_ty and ac.tran_cd=m.tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Inner Join Ac_mast on (Ac_mast.Ac_id=m.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Where m.Date Between '''+convert(Varchar(50),@sdate)+''' and '''+Convert(Varchar(50),@edate)+''''
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and (l.entry_ty in (''EP'') or l.bcode_nm in (''EP''))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and convert(Varchar(10),m.Tran_cd)+m.Entry_ty Not In (Select convert(Varchar(10),Tran_cd)+Entry_ty From #epreg)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'group by l.Code_nm,Ac_mast.Ac_name,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,m.serrule'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.Serty, ac.Amount,ac.sAbtAmt,ac.sTaxable,ac.Serbamt,ac.Sercamt,ac.Serhamt,ac.serrbamt,ac.serrcamt,ac.serrhamt,ac.SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.entry_ty,ac.tran_cd,l.entry_ty,l.bcode_nm'
	--print  @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	
	set @sqlcommand='insert into #epreg (code_nm,ac_name,u_pinvno,u_pinvdt,inv_no,date,serrule,'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Serty, Amount,sAbtAmt,sTaxable,Serbamt,Sercamt,Serhamt,serrbamt,serrcamt,serrhamt,SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',serRecAmt,Entry_ty,Tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select Case When l.Entry_ty In (''BP'',''CP'') Or  l.Bcode_nm In (''BP'',''CP'') then ''Un-billed Advance Payment'' else l.code_nm End' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Ac_mast.Ac_name,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,m.serrule' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.Serty, ac.Amount,ac.sAbtAmt,ac.sTaxable,ac.Serbamt,ac.Sercamt,ac.Serhamt,ac.serrbamt,ac.serrcamt,ac.serrhamt,ac.SerAvail' 
	set @sqlcommand=rtrim(@sqlcommand)+' '+',0,ac.entry_ty,ac.tran_cd'	
	set @sqlcommand=rtrim(@sqlcommand)+' '+' from acdetalloc ac '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join SerTaxMain_vw m on (ac.entry_ty=m.entry_ty and ac.tran_cd=m.tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Inner Join Ac_mast on (Ac_mast.Ac_id=m.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'Where m.Date Between '''+convert(Varchar(50),@sdate)+''' and '''+Convert(Varchar(50),@edate)+''''
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and (l.entry_ty in (''BP'',''CP'') or l.bcode_nm in (''BP'',''CP''))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and m.tdspaytype=2'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and convert(Varchar(10),m.Tran_cd)+m.Entry_ty Not In (Select convert(Varchar(10),Tran_cd)+Entry_ty From #epreg)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'group by l.Code_nm,Ac_mast.Ac_name,m.u_pinvno,m.u_pinvdt,m.inv_no,m.date,m.serrule'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.Serty, ac.Amount,ac.sAbtAmt,ac.sTaxable,ac.Serbamt,ac.Sercamt,ac.Serhamt,ac.serrbamt,ac.serrcamt,ac.serrhamt,ac.SerAvail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.entry_ty,ac.tran_cd,l.entry_ty,l.bcode_nm'
	--print  @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	--select * from #epreg
	
	select * from #epreg where Not(code_nm='Un-billed Advance Payment' and Amount=serRecAmt)

	DROP TABLE #epreg
	
END








