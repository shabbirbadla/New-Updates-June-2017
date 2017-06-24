IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Rep_salestax_form]') AND type in (N'P', N'PC'))
begin
DROP PROCEDURE [dbo].[usp_Rep_salestax_form]
end
Go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 11/05/2009
-- Description:	This Stored procedure is useful in Sales Tax Form No Report Project uestformno.app.
-- Modify date: 
-- Modified By: Rupesh Prajapati
-- Modified by: Shrikant S. on 03/04/2010 for TKT-631
-- Modified By: Amrendra For Bug-83 on 11-11-11 , I did't Get the error provided in the BugZilla, So as Per disscussion had with Sachin and Rupesh, i am merging the SP with solution provided by Dinesh in Bugzilla.
-- Modified by: Sandeep for bug-6280 on 15/09/12
-- Modified by: email field is added 20/12/2012 by support team (bug-7794).
-- Modified By: SATISH PAL FOR BUG-7280 DATED 1/2/13
-- Remark:		
-- =============================================
Create procedure [dbo].[usp_Rep_salestax_form]
@vformnm varchar(30),@vparty varchar(100),@mCondn nvarchar(100),@vform int,@sdate smalldatetime,@edate smalldatetime,@dept nvarchar(100),@cate nvarchar(100),@broker nvarchar(100)--ADDED BY SATISH PAL FOR BUG-7280 DATED 1/2/13
as
begin
	set NoCount On
	--declare @mCondn nvarchar(100)
	--set @mCondn = ' and (isnull(m.form_no,space(1))<>space(1) or isnull(m.form_nm,space(1))<>space(1))'
	declare @sqlcommand nvarchar(4000)
	declare @whcon nvarchar(1000)
	set @mCondn=upper(@mCondn)

	set @whcon=''
	if isnull(@vformnm,'')<>''
	begin
		set @whcon=rtrim(@whcon)+' '+' and (  (isnull(st.form_nm,'''')='+char(39)+@vformnm+char(39)+' or isnull(st.rForm_Nm,'''')='+char(39)+@vformnm+char(39)+')  )'
	end 
	if isnull(@vparty,'')<>''
	begin
		set @whcon=rtrim(@whcon)+' '+' and ( ac.ac_name='+char(39)+@vparty+char(39)+')'
	end 
	if isnull(@vparty,'')<>''
	begin
		set @whcon=rtrim(@whcon)+' '+' and ( ac.ac_name='+char(39)+@vparty+char(39)+')'
	end	
	
	if isnull(@mCondn,'All')='YES'
	begin
	--ADDED AND COMMENTED BY SATISH PAL FOR BUG-7280 DATED 1/2/13-START
		---set @whcon=rtrim(@whcon)+' '+' and (  '+'isnull(m.form_nm,'''')='''''+' or isnull(m.form_no,'''')=''''' +'  )'
		set @whcon=rtrim(@whcon)+' '+' and (  '+'isnull(m.form_nm,'''')='''''+' and isnull(m.form_no,'''')=''''' +'  )'
		if isnull(@cate,'')<>''
			begin
			set @whcon=rtrim(@whcon)+' '+' and ( m.cate='+char(39)+@cate+char(39)+')'
		end
		if isnull(@dept,'')<>''
		begin
			set @whcon=rtrim(@whcon)+' '+' and ( m.dept='+char(39)+@dept+char(39)+')'
		end 
		if isnull(@broker,'')<>''
		begin
			set @whcon=rtrim(@whcon)+' '+' and ( m.u_broker='+char(39)+@broker+char(39)+')'
		end
	--ADDED AND COMMENTED BY SATISH PAL FOR BUG-7280 DATED 1/2/13-END
	end	
	if isnull(@mCondn,'All')='NO'
	begin
		--ADDED AND COMMENTED BY SATISH PAL FOR BUG-7280 DATED 1/2/13-START
		---set @whcon=rtrim(@whcon)+' '+' and (  '+'isnull(m.form_nm,'''')<>'''''+' and isnull(m.form_no,'''')<>''''' +'  )'
		set @whcon=rtrim(@whcon)+' '+' and (  '+'isnull(m.form_nm,'''')='''''+' or isnull(m.form_no,'''')=''''' +'  )'
		if isnull(@cate,'')<>''
			begin
			set @whcon=rtrim(@whcon)+' '+' and ( m.cate='+char(39)+@cate+char(39)+')'
		end
		if isnull(@dept,'')<>''
		begin
			set @whcon=rtrim(@whcon)+' '+' and ( m.dept='+char(39)+@dept+char(39)+')'
		end 
		if isnull(@broker,'')<>''
		begin
			set @whcon=rtrim(@whcon)+' '+' and ( m.u_broker='+char(39)+@broker+char(39)+')'
		end
	--ADDED AND COMMENTED BY SATISH PAL FOR BUG-7280 DATED 1/2/13End
	end	

	if (@vform=1)
	begin
		set @whcon=rtrim(@whcon)+' '+' and (isnull(st.form_nm,'''')<>'''')'
	end
	if (@vform=2)
	begin
		set @whcon=rtrim(@whcon)+' '+' and (isnull(st.rform_nm,'''')<>'''')'
	end
	--

	--Added By Kishor A. for Bug-26942 on 12/10/2015 Start...
	DECLARE @COM_SQLSTR NVARCHAR(4000),@COM_SQLSTRST NVARCHAR(4000),@COM_SQLSTRPT NVARCHAR(4000),@COM_SQLSTRSR NVARCHAR(4000),@COM_SQLSTRPR NVARCHAR(4000),@SQL_STFLD NVARCHAR(4000)
	,@SQL_SRFLD NVARCHAR(4000),@SQL_PTFLD NVARCHAR(4000),@SQL_PRFLD NVARCHAR(4000),@SQL_TMPFLD NVARCHAR(4000),@CallString AS VARCHAR(4000)

	EXECUTE Dynamically_Fields_Rep 
	
	SELECT @COM_SQLSTR = SqlStr FROM ##Dyn_TmpTable WHERE Entry_ty='SQSTR'
	SELECT @SQL_STFLD =TblFld,@COM_SQLSTRST = SqlStr FROM ##Dyn_TmpTable WHERE Entry_ty='ST'
	SELECT @SQL_SRFLD =TblFld,@COM_SQLSTRSR = SqlStr FROM ##Dyn_TmpTable WHERE Entry_ty='SR'
	SELECT @SQL_PTFLD =TblFld,@COM_SQLSTRPT = SqlStr FROM ##Dyn_TmpTable WHERE Entry_ty='PT'
	SELECT @SQL_PRFLD =TblFld,@COM_SQLSTRPR = SqlStr FROM ##Dyn_TmpTable WHERE Entry_ty='PR'
	SELECT @SQL_TMPFLD = SqlStr FROM ##Dyn_TmpTable WHERE Entry_ty='TMPFLD'
	
	Declare @SQLSTR nvarchar (4000),@IntoStr as Nvarchar(4000)
	
	set @SQLSTR= 'select m.entry_ty,m.tran_cd,m.inv_no,m.form_nm,m.form_no,m.date,m.net_amt,m.tax_name,m.taxamt
	,ac.mailname,party_nm=ac.ac_name,formname=st.form_nm,rformname=st.rForm_Nm
	,bcode_nm=case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end
	,code_nm
	,ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.email,ac.state
	,m.formidt,m.formrdt	
	,u_pinvno=cast('''' as varchar(100)), m.u_pinvdt'+@COM_SQLSTR+'
	into ##stax_form
	from stmain m 
	inner join stax_mas st on (m.tax_name=st.tax_name and m.entry_ty=st.entry_ty)
	inner join ac_mast ac on (m.ac_id=ac.ac_id)
	inner join lcode l on (m.entry_ty=l.entry_ty)
	where (isnull(st.form_nm,'''')<>'''' or isnull(st.rform_nm,'''')<>'''') and 1=2'
	execute sp_executesql @SQLSTR
	
	set @IntoStr ='entry_ty,tran_cd,inv_no,form_nm,form_no,date,net_amt,tax_name,taxamt,mailname,party_nm,formname,rformname
	,bcode_nm,code_nm,add1,add2,add3,contact,city,zip,email,state,formidt,formrdt,u_pinvno,u_pinvdt'
	
--Added By Kishor A. for Bug-26942 on 12/10/2015 End...

----Commented By Kishor A. for Bug-26942 on 12/10/2015 Start..
	--select m.entry_ty,m.tran_cd,m.inv_no,m.form_nm,m.form_no,m.date,m.net_amt,m.tax_name,m.taxamt
	--,ac.mailname,party_nm=ac.ac_name,formname=st.form_nm,rformname=st.rForm_Nm
	--,bcode_nm=case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end
	--,code_nm
	--,ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.email,ac.state --email field is added 20/12/2012 by support team (bug-7794).
	--,m.formidt,m.formrdt
	----	,u_pinvno=m.inv_no,u_pinvdt=m.date  --Commented By Amrendra For Bug-83 on 11-11-11
	----,m.u_pinvno,m.u_pinvdt  --Added By Amrendra For Bug-83 on 11-11-11 -- commented By sandeep for the bug-6280 on 15/09/2012
	--,u_pinvno=cast('' as varchar(100)), m.u_pinvdt  --Added By sandeep for the bug-6280 on 15/09/2012
	--into #stax_form
	--from stmain m 
	--inner join stax_mas st on (m.tax_name=st.tax_name and m.entry_ty=st.entry_ty)
	--inner join ac_mast ac on (m.ac_id=ac.ac_id)
	--inner join lcode l on (m.entry_ty=l.entry_ty)
	--where (isnull(st.form_nm,'')<>'' or isnull(st.rform_nm,'')<>'') and 1=2
----Commented By Kishor A. for Bug-26942 on 12/10/2015 End

--	set @sqlcommand='insert into #stax_form' Commented By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand='insert into ##stax_form ('+@IntoStr+@COM_SQLSTRST+')' --Added By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select m.entry_ty,m.tran_cd,m.inv_no,m.form_nm,m.form_no,m.date,m.net_amt,m.tax_name,m.taxamt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.mailname,party_nm=ac.ac_name,formname=st.form_nm,rformname=st.rForm_Nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bcode_nm=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',code_nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.email,ac.state' --email field is added 20/12/2012 by support team (bug-7794).
	set @sqlcommand=rtrim(@sqlcommand)+' '+',m.formidt,m.formrdt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',u_pinvno='''',u_pinvdt='''''+@SQL_STFLD --Added By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand=rtrim(@sqlcommand)+' '+'from stmain m '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join stax_mas st on (m.tax_name=st.tax_name and m.entry_ty=st.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'where (isnull(st.form_nm,space(1))<>space(1) or isnull(st.rform_nm,space(1))<>space(1))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ( m.date between '+char(39)+cast(@sdate as varchar)+char(39)+' and '+char(39)+cast(@edate as varchar)+char(39)+')'+@whcon
	print @sqlcommand

	execute sp_executesql @sqlcommand

	--	set @sqlcommand='insert into #stax_form' Commented By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand='insert into ##stax_form ('+@IntoStr+@COM_SQLSTRPT+')' --Added By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select m.entry_ty,m.tran_cd,m.inv_no,m.form_nm,m.form_no,m.date,m.net_amt,m.tax_name,m.taxamt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.mailname,party_nm=ac.ac_name,formname=st.form_nm,rformname=st.rForm_Nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bcode_nm=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',code_nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.email,ac.state' --email field is added 20/12/2012 by support team (bug-7794).
	set @sqlcommand=rtrim(@sqlcommand)+' '+',m.formidt,m.formrdt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',M.u_pinvno,M.u_pinvdt'+@SQL_PTFLD --Added By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand=rtrim(@sqlcommand)+' '+'from ptmain m '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join stax_mas st on (m.tax_name=st.tax_name and m.entry_ty=st.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'where (isnull(st.form_nm,space(1))<>space(1) or isnull(st.rform_nm,space(1))<>space(1))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ( m.date between '+char(39)+cast(@sdate as varchar)+char(39)+' and '+char(39)+cast(@edate as varchar)+char(39)+')'+@whcon
	print @sqlcommand
	execute sp_executesql @sqlcommand

--	set @sqlcommand='insert into #stax_form' Commented By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand='insert into ##stax_form ('+@IntoStr+@COM_SQLSTRSR+')' --Added By Kishor A. for Bug-26942 on 12/10/2015	
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select m.entry_ty,m.tran_cd,m.inv_no,m.form_nm,m.form_no,m.date,m.net_amt,m.tax_name,m.taxamt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.mailname,party_nm=ac.ac_name,formname=st.form_nm,rformname=st.rForm_Nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bcode_nm=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',code_nm'
	--set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.state' --Added By Amrendra For Bug-83 on 11-11-11
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.email,ac.state' --email field is added 20/12/2012 by support team (bug-7794).
	set @sqlcommand=rtrim(@sqlcommand)+' '+',m.formidt,m.formrdt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',u_pinvno='''',u_pinvdt='''''+@SQL_SRFLD --Added By Kishor A. for Bug-26942 on 12/10/2015
	--set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.state' --Commented By Amrendra For Bug-83 on 11-11-11
	set @sqlcommand=rtrim(@sqlcommand)+' '+'from srmain m '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join stax_mas st on (m.tax_name=st.tax_name and m.entry_ty=st.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'where (isnull(st.form_nm,space(1))<>space(1) or isnull(st.rform_nm,space(1))<>space(1))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ( m.date between '+char(39)+cast(@sdate as varchar)+char(39)+' and '+char(39)+cast(@edate as varchar)+char(39)+')'+@whcon
	print @sqlcommand
	execute sp_executesql @sqlcommand

	--	set @sqlcommand='insert into #stax_form' Commented By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand='insert into ##stax_form ('+@IntoStr+@COM_SQLSTRPR+')' --Added By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand=rtrim(@sqlcommand)+' '+'select m.entry_ty,m.tran_cd,m.inv_no,m.form_nm,m.form_no,m.date,m.net_amt,m.tax_name,m.taxamt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.mailname,party_nm=ac.ac_name,formname=st.form_nm,rformname=st.rForm_Nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bcode_nm=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',code_nm'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac.add1,ac.add2,ac.add3,ac.contact,ac.city,ac.zip,ac.email,ac.state' --email field is added 20/12/2012 by support team (bug-7794).
	set @sqlcommand=rtrim(@sqlcommand)+' '+',m.formidt,m.formrdt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',u_pinvno='''',u_pinvdt='''''+@SQL_PRFLD --Added By Kishor A. for Bug-26942 on 12/10/2015
	set @sqlcommand=rtrim(@sqlcommand)+' '+'from prmain m '
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join stax_mas st on (m.tax_name=st.tax_name and m.entry_ty=st.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast ac on (m.ac_id=ac.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'where (isnull(st.form_nm,space(1))<>space(1) or isnull(st.rform_nm,space(1))<>space(1))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ( m.date between '+char(39)+cast(@sdate as varchar)+char(39)+' and '+char(39)+cast(@edate as varchar)+char(39)+')'+@whcon
	print @sqlcommand
	
	execute sp_executesql @sqlcommand
	
-- Commented By Kishor A. for Bug-26942 on 12/10/2015 Start	
	--select entry_ty,tran_cd,inv_no,form_nm,form_no,date,net_amt,tax_name,taxamt,mailname,party_nm,formname,rformname,bcode_nm,code_nm
	--,add1,add2,add3,contact,city,zip,email,formidt,formrdt,u_pinvno,u_pinvdt --email field is added 20/12/2012 by support team (bug-7794).
	--from #stax_form 
	----order by party_nm,u_pinvdt  -- Commented by Shrikant S. on 03/04/2010 for TKT-631
	--order by party_nm,Case when isnull(u_pinvdt,0)=0 then Date else u_pinvdt end,Case when isnull(u_pinvno,'')='' then inv_no else u_pinvno end -- Added by Shrikant S. on 03/04/2010 for TKT-631
 --Commented By Kishor A. for Bug-26942 on 12/10/2015 End

--Added By Kishor A. for Bug-26942 on 12/10/2015 Start..	
	SET @SQLSTR = 'select entry_ty,tran_cd,inv_no,form_nm,form_no,date,net_amt,tax_name,taxamt,mailname,party_nm,formname,rformname,bcode_nm,code_nm
	,add1,add2,add3,contact,city,zip,email,formidt,formrdt,u_pinvno,u_pinvdt'+@SQL_TMPFLD+'	from ##stax_form
	order by party_nm,Case when isnull(u_pinvdt,0)=0 then Date else u_pinvdt end,Case when isnull(u_pinvno,'''')='''' then inv_no else u_pinvno end'
	execute sp_executesql @SQLSTR
--Added By Kishor A. for Bug-26942 on 12/10/2015 End..		
	
end

if OBJECT_ID('tempdb..##Dyn_TmpTable') is not null
begin
    drop table ##Dyn_TmpTable
end

DROP TABLE ##stax_form




