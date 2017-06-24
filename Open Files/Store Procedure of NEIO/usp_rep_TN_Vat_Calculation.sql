set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go











-- =============================================
-- Author:		Hetal L Patel
-- Create date: 19/06/2008
-- Description:	This Stored procedure is useful to generate Tamilnadu VAT & CST Calculation Report.
-- Modify date: 16-April-2010
-- Modified By: Rakesh Varma
-- Modify date: 17-08-2010
-- Modified By: Sandeep shah
-- Description:	Modification of CST SALES (Net of Sales Returns) Value for TKT-3566
-- =============================================

alter procedure [dbo].[usp_rep_TN_Vat_Calculation]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS
Declare @FCON as NVARCHAR(2000)
Declare @SQLCOMMAND NVARCHAR(4000)
Declare @gro_amt decimal(12,2),@taxamt decimal(12,2),@gro_amt1 decimal(12,2),@taxamt1 decimal(12,2)

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE --null
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='m',@VITFILE='',@VACFILE=''
,@VDTFLD ='DATE'
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

declare @fld_list NVARCHAR(2000)

select part=1,srno1=space(1),srno2=space(1),srno3=space(1),trdesc=space(100)
,itdesc=space(100),gro_amt ,tax_name,taxamt
into #tnvatcalc
from stmain where 1=2

--select m.u_imporm ,i.entry_ty,i.tran_cd,i.date ,itdesc=m.cate ,gro_amt=(i.qty*i.rate)+i.BCDAMT+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ,i.tax_name,i.taxamt,i.taxpay ,st.st_type ,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
select m.u_imporm ,i.entry_ty,i.tran_cd,i.date ,itdesc=m.cate 
,gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.BCDAMT+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )
,i.tax_name,i.taxamt,st.st_type ,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
into #tnvatcalc_pt
from ptitem i  
inner join ptmain m on (m.tran_cd=i.tran_cd) 
inner join it_mast on (i.it_code=it_mast.it_code) 
inner join lcode l on (i.entry_ty=l.entry_ty) 
inner join stax_mas st on (i.tax_name=st.tax_name)   
WHERE 1=2

select u_imporm=''
,i.entry_ty,i.tran_cd,i.date
,itdesc=m.cate
--,i.gro_amt,i.tax_name,i.taxamt,taxpay=0
,i.gro_amt,i.tax_name,i.taxamt
,st.st_type
,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
into #tnvatcalc_pr
from pritem i 
inner join prmain m on (m.tran_cd=i.tran_cd)
inner join it_mast on (i.it_code=it_mast.it_code)
inner join lcode l on (i.entry_ty=l.entry_ty)
inner join stax_mas st on (i.tax_name=st.tax_name)
WHERE 1=2

select m.u_imporm
,i.entry_ty,i.tran_cd,i.date
,itdesc=it_mast.[group]
--,gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END ),i.tax_name,i.taxamt
,i.gro_amt,i.tax_name,i.taxamt
,st.st_type
,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
,m.[rule]
into #tnvatcalc_st
from stitem i 
inner join stmain m on (m.tran_cd=i.tran_cd)
inner join it_mast on (i.it_code=it_mast.it_code)
inner join lcode l on (i.entry_ty=l.entry_ty)
inner join stax_mas st on (i.tax_name=st.tax_name)
WHERE 1=2

select 
i.entry_ty,i.tran_cd,i.date
,itdesc=it_mast.[group]
,i.gro_amt,i.tax_name,i.taxamt
,st.st_type
,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
,m.[rule]
into #tnvatcalc_sr
from sritem i 
inner join srmain m on (m.tran_cd=i.tran_cd)
inner join it_mast on (i.it_code=it_mast.it_code)
inner join lcode l on (i.entry_ty=l.entry_ty)
inner join stax_mas st on (i.tax_name=st.tax_name)
WHERE 1=2

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		Set @MultiCo = 'YES'


        execute usp_rep_Taxable_Amount_Itemwise 'PT','i',@fld_list =@fld_list OUTPUT
		set @fld_list=rtrim(@fld_list)
	    set @sqlcommand='insert into #tnvatcalc_pt select m.u_imporm'
		set @sqlcommand=@sqlcommand+' '+',i.entry_ty,i.tran_cd,i.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.BCDAMT+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )'+@fld_list
		--set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt,i.taxpay'
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from ptitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join ptmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id and a.dbname=m.dbname'
		set @sqlcommand=@sqlcommand+' '+'Left join stax_mas st on (i.tax_name=st.tax_name) And st.entry_Ty = ''PT'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------
	
		execute usp_rep_Taxable_Amount_Itemwise 'EP','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_pt select u_imporm='+''''''
		--set @sqlcommand='insert into #tnvatcalc_pt select u_imporm'
		set @sqlcommand=@sqlcommand+' '+',m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		--set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		set @sqlcommand=@sqlcommand+' '+',m.gro_amt'
		--set @sqlcommand=@sqlcommand+' '+',m.tax_name,taxamt=sum(case when ac_mast.typ='+'''Input Vat'''+' then amount else 0 end),taxpay=0'
		set @sqlcommand=@sqlcommand+' '+',m.tax_name,taxamt=sum(case when ac_mast.typ='+'''Input Vat'''+' then amount else 0 end)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from epmain m '
		set @sqlcommand=@sqlcommand+' '+'inner join epacdet ac on (m.tran_cd=ac.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join ac_mast on (ac.ac_id=ac_mast.ac_id)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id and a.dbname=m.dbname'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (m.tax_name=st.tax_name)And st.entry_Ty = ''EP'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		set @sqlcommand=@sqlcommand+' '+'group by'
		set @sqlcommand=@sqlcommand+' '+'m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',m.cate,m.gro_amt,m.tax_name,a.st_type'
		set @sqlcommand=@sqlcommand+' '+',l.ext_vou,l.bcode_nm,l.entry_ty'
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------	
	
        execute usp_rep_Taxable_Amount_Itemwise 'PR','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_pr select u_imporm='+''''''
		set @sqlcommand=@sqlcommand+' '+',m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		--set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt,taxpay=0'
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from pritem i '
		set @sqlcommand=@sqlcommand+' '+'inner join prmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id and a.dbname=m.dbname'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''PR'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------		
	
        execute usp_rep_Taxable_Amount_Itemwise 'ST','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_pr select u_imporm='+''''''
		set @sqlcommand=@sqlcommand+' '+',m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )'+@fld_list
		--set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt,taxpay=0'
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from stitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join stmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id and a.dbname=m.dbname'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''ST'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		set @sqlcommand=@sqlcommand+' '+' and isnull(m.u_imporm,'+''''''+')='+'''Purchase Return'''
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------		
		
		execute usp_rep_Taxable_Amount_Itemwise 'ST','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_st '
		set @sqlcommand=@sqlcommand+' '+'select m.u_imporm'
		set @sqlcommand=@sqlcommand+' '+',i.entry_ty,i.tran_cd,i.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		--set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
        set @sqlcommand=@sqlcommand+' '+',gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.BCDAMT+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )'+@fld_list    
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+',m.[rule]'
		set @sqlcommand=@sqlcommand+' '+'from stitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join stmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id and a.dbname=m.dbname'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''ST'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand
		
---------------------------------------------------------------------------------------------------			

        set @sqlcommand='insert into #tnvatcalc_sr select '
		set @sqlcommand=@sqlcommand+' '+'i.entry_ty,i.tran_cd,i.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+',m.[rule]'
		set @sqlcommand=@sqlcommand+' '+'from sritem i '
		set @sqlcommand=@sqlcommand+' '+'inner join srmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id and a.dbname=m.dbname'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''SR'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand

		-->part-1
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'a','0',''
		,'TNVAT PURCHASES',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_pt
		where st_type='LOCAL'
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'b','0',''
		,'PURCHASE RETURN(VAT)',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pr
		where st_type='LOCAL'
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1='a' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='a' then taxamt else -taxamt end) 
		from #tnvatcalc
		where part=1 and srno1 in('a','b') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		1,'c','0',''
		,'NET VAT PURCHASES','',@gro_amt,'',@taxamt
		)


		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'d','0',''
		,'INTER STATE PURCHASES',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pt
		where st_type='OUT OF STATE'
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'e','0',''
		,'PURCHASE RETURN(CST)',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pr
		where st_type='OUT OF STATE'
		group by itdesc,tax_name
		
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'f','0',''
		,'IMPORT PURCHASES',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pt
		where isnull(u_imporm,'') in ('Direct Imports','High Seas Purchases')
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(gro_amt),@taxamt=sum(taxamt) 
		from #tnvatcalc
		where part=1 and srno1 in('a','d','f') and srno2='0'


		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		1,'g','0',''
		,'PURCHASES(Before Purchase Return)','',@gro_amt,'',@taxamt
		)

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1 in('a','d','f') then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='a' then taxamt else -taxamt end) 
		from #tnvatcalc
		where part=1 and srno1 in('a','b','e','d','f') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		1,'h','0',''
		,'PURCHASES(After Purchase Return)','',@gro_amt,'',@taxamt
		)
        
        SELECT * FROM #TNVATCALC
  
		if not exists(select * from #tnvatcalc where part=1 and srno1='a')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'a','0','1'
			,'TNVAT PURCHASES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='b')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'b','0','1'
			,'PURCHASE RETURN(VAT)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='d')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'d','0','1'
			,'INTER STATE PURCHASES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='e')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'e','0','1'
			,'PURCHASE RETURN(CST)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='f')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'f','0','1'
			,'IMPORT PURCHASES','',0,'',0
			)
		end
		--<--part-1
		-->part-2
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'a','0',''
		,'TNVAT SALES',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where st_type='LOCAL' and [rule] not in ('CT-1','CT-3','UT-1','EOU EXPORT')
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'b','0',''
		,'SALES RETURN (TNVAT)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_sr
		where st_type='LOCAL' 
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1='a' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='a' then taxamt else -taxamt end) 
		from #tnvatcalc
		where part=2 and srno1 in('a','b') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		2,'c','0',''
		,'TNVAT SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
		)

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'d','0',''
		,'TNVAT SALES (Net of Sales Return-Service Income)','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_st
		where st_type='LOCAL' and itdesc='Services'--???


		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'E','0',''
		,'INTER STATE SALES',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where st_type='Out of State' 
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'f','0',''
		,'SALES RETURN (CST)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_sr
		where st_type='Out of State' 
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1='e' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='e' then taxamt else -taxamt end) 
		from #tnvatcalc
		where part=2 and srno1 in('e','f') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)

		select
		2,'g','0',''
		,'CST SALES (Net of Sales Returns)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_sr
		where st_type='Out of State' and tax_name like 'C.S.T%'  
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0		
		from #tnvatcalc
		where part=2 and srno1 in('g') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)

--		values
--		(
--		2,'g','0',''
--		,'CST SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
--		)
--
--		insert into #tnvatcalc
--		(
--		part,srno1,srno2,srno3
--		,trdesc,itdesc,gro_amt,tax_name,taxamt
--		)
		select
		2,'h','0',''
		,'Export Sales',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where  [rule] in ('CT-1','CT-3','UT-1','EOU EXPORT') and tax_name<>'FORM H'
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'h','1',''
		,'Export Sales',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where  [rule] in ('CT-1','CT-3','UT-1','EOU EXPORT') and tax_name='FORM H'
		group by itdesc,tax_name


		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'i','0',''
		,'Total Export','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_st
		where  [rule] in ('CT-1','CT-3','UT-1','EOU EXPORT')

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'j','0',''
		,'CST TOTAL SALES (Before Sales Return)','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_sr
		where st_type='Out of State' 

--		select @gro_amt=0,@taxamt=0
--		select @gro_amt=sum(case when srno1='j' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='j' then taxamt else -taxamt end) 
--		from #tnvatcalc
--		where part=2 and srno1 in('j','g') and srno2='0'
--
--		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
--		insert into #tnvatcalc
--		(
--		part,srno1,srno2,srno3
--		,trdesc,itdesc,gro_amt,tax_name,taxamt
--		)
--		values
--		(
--		2,'k','0',''
--		,'CST SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
--		)
--
		select @gro_amt1=sum(gro_amt),
		       @taxamt1=sum(taxamt) 
		from #tnvatcalc
		where part=2 and srno1 = 'g' and srno2='0'
		
		select @gro_amt=isnull(@gro_amt,0)-isnull(@gro_amt1,0),@taxamt=isnull(@taxamt,0)-isnull(@taxamt1,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		2,'k','0',''
		,'CST SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
		)

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(gro_amt),@taxamt=sum(taxamt) 
		from #tnvatcalc
		where part=2 and srno1 in('c','j') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		2,'l','0',''
		,'GROSS SALES(TNVAT+CST)(Before CST Sales return)','',@gro_amt,'',@taxamt
		)
		--
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'m','0',''
		,'CST STOCK TRANSFER','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_st
		where st_type='Out of State'  and u_imporm='Branch Transfer'
		--blank Record checkin
		if not exists(select * from #tnvatcalc where part=2 and srno1='a')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'a','0','1'
			,'TNVAT SALES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='b')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'b','0','1'
			,'SALES RETURN (TNVAT)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='c')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'c','0','1'
			,'TNVAT SALES (Net of Sales Returns)','',0,'',0
			)
		end

		if not exists(select * from #tnvatcalc where part=2 and srno1='d')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'d','0','1'
			,'TNVAT SALES (Net of Sales Return-Service Income)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='e')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'e','0','1'
			,'INTER STATE SALES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='f')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'f','0','1'
			,'SALES RETURN (CST)','',0,'',0
			)
		end

		if not exists(select * from #tnvatcalc where part=2 and srno1='g')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'g','0','1'
			,'CST SALES (Net of Sales Returns)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='m')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'m','0',''
			,'CST SALES (Net of Sales Returns)','',0,'',0
			)
		end

		--blank Record checkin
		--<--part-2
	end
else
	Begin	------Fetch Records from Single Co. Data
		Set @MultiCo = 'NO'

---------------------------------------------------------------------------------------------------


		execute usp_rep_Taxable_Amount_Itemwise 'PT','i',@fld_list =@fld_list OUTPUT
		set @fld_list=rtrim(@fld_list)
	    set @sqlcommand='insert into #tnvatcalc_pt select m.u_imporm'
		set @sqlcommand=@sqlcommand+' '+',i.entry_ty,i.tran_cd,i.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		--set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )'+@fld_list
		--set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt,i.taxpay'
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from ptitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join ptmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id'
		set @sqlcommand=@sqlcommand+' '+'Left join stax_mas st on (i.tax_name=st.tax_name) And st.entry_Ty = ''PT'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------		
		
		execute usp_rep_Taxable_Amount_Itemwise 'EP','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_pt select u_imporm='+''''''
		--set @sqlcommand='insert into #tnvatcalc_pt select u_imporm'
		set @sqlcommand=@sqlcommand+' '+',m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		--set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		set @sqlcommand=@sqlcommand+' '+',m.gro_amt'
		--set @sqlcommand=@sqlcommand+' '+',m.tax_name,taxamt=sum(case when ac_mast.typ='+'''Input Vat'''+' then amount else 0 end),taxpay=0'
		set @sqlcommand=@sqlcommand+' '+',m.tax_name,taxamt=sum(case when ac_mast.typ='+'''Input Vat'''+' then amount else 0 end)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from epmain m '
		set @sqlcommand=@sqlcommand+' '+'inner join epacdet ac on (m.tran_cd=ac.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join ac_mast on (ac.ac_id=ac_mast.ac_id)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (m.tax_name=st.tax_name)And st.entry_Ty = ''EP'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		set @sqlcommand=@sqlcommand+' '+'group by'
		set @sqlcommand=@sqlcommand+' '+'m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',m.cate,m.gro_amt,m.tax_name,a.st_type'
		set @sqlcommand=@sqlcommand+' '+',l.ext_vou,l.bcode_nm,l.entry_ty'
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------		

       	execute usp_rep_Taxable_Amount_Itemwise 'PR','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_pr select u_imporm='+''''''
		set @sqlcommand=@sqlcommand+' '+',m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		--set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt,taxpay=0'
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from pritem i '
		set @sqlcommand=@sqlcommand+' '+'inner join prmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''PR'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------		

        execute usp_rep_Taxable_Amount_Itemwise 'ST','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_pr select u_imporm='+''''''
		set @sqlcommand=@sqlcommand+' '+',m.entry_ty,m.tran_cd,m.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )'+@fld_list
		--set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt,taxpay=0'
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.		
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+'from stitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join stmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''ST'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		set @sqlcommand=@sqlcommand+' '+' and isnull(m.u_imporm,'+''''''+')='+'''Purchase Return'''
		print @sqlcommand
		execute sp_executesql @sqlcommand

---------------------------------------------------------------------------------------------------

		
		execute usp_rep_Taxable_Amount_Itemwise 'ST','i',@fld_list =@fld_list OUTPUT
		set @sqlcommand='insert into #tnvatcalc_st '
		set @sqlcommand=@sqlcommand+' '+'select m.u_imporm'
		set @sqlcommand=@sqlcommand+' '+',i.entry_ty,i.tran_cd,i.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(case when m.incexcise=1 THEN  (i.qty*i.rate)+i.U_CESSAMT+i.U_HCESAMT+i.EXAMT+i.U_CVDAMT ELSE (i.qty*i.rate) END )'+@fld_list
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+',m.[rule]'
		set @sqlcommand=@sqlcommand+' '+'from stitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join stmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''ST'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand
		
---------------------------------------------------------------------------------------------------		

		set @sqlcommand='insert into #tnvatcalc_sr select '
		set @sqlcommand=@sqlcommand+' '+'i.entry_ty,i.tran_cd,i.date'
		set @sqlcommand=@sqlcommand+' '+',itdesc=m.cate'
		set @sqlcommand=@sqlcommand+' '+',gro_amt=(i.qty*i.rate)'+@fld_list
		set @sqlcommand=@sqlcommand+' '+',i.tax_name,i.taxamt'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below line has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+',a.st_type'
		set @sqlcommand=@sqlcommand+' '+',beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)'
		set @sqlcommand=@sqlcommand+' '+',m.[rule]'
		set @sqlcommand=@sqlcommand+' '+'from sritem i '
		set @sqlcommand=@sqlcommand+' '+'inner join srmain m on (m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast on (i.it_code=it_mast.it_code)'
		set @sqlcommand=@sqlcommand+' '+'inner join lcode l on (i.entry_ty=l.entry_ty)'
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below code has been changed as per above TKT NO.
		set @sqlcommand=@sqlcommand+' '+'inner join Ac_mast a on a.ac_id=m.ac_id'
		set @sqlcommand=@sqlcommand+' '+'left join stax_mas st on (i.tax_name=st.tax_name)And st.entry_Ty = ''SR'' '
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		--set @sqlcommand=@sqlcommand+' '+' and isnull(m.tax_name,'+''''''+')<>'+'''''' 
		print @sqlcommand
		execute sp_executesql @sqlcommand
		
----------------------------------------------------------------------------------------------------		
		
		-->part-1
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'a','0',''
		,'TNVAT PURCHASES',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_pt
		where st_type='LOCAL' and tax_name <> ''
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'b','0',''
		,'PURCHASE RETURN(VAT)',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pr
		where st_type='LOCAL'
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1='a' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='a' then taxamt else -taxamt end) 
		from #tnvatcalc
		where part=1 and srno1 in('a','b') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		1,'c','0',''
		,'NET VAT PURCHASES','',@gro_amt,'',@taxamt
		)


		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'d','0',''
		,'INTER STATE PURCHASES',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pt
		where st_type='OUT OF STATE'
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'e','0',''
		,'PURCHASE RETURN(CST)',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pr
		where st_type='OUT OF STATE'
		group by itdesc,tax_name
		
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		1,'f','0',''
		,'IMPORT PURCHASES',itdesc,sum(gro_amt),tax_name,sum(taxamt)
		from #tnvatcalc_pt
		where isnull(u_imporm,'') in ('Direct Imports','High Seas Purchases')
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(gro_amt),@taxamt=sum(taxamt) 
		from #tnvatcalc
		where part=1 and srno1 in('a','d','f') and srno2='0'


		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		1,'g','0',''
		,'PURCHASES(Before Purchase Return)','',@gro_amt,'',@taxamt
		)

		select @gro_amt=0,@taxamt=0
		
	
		select @gro_amt=sum(case when srno1 in ('a','d','f') then gro_amt else -gro_amt end),
		
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		--Below commented code has been updated inorder to get correct value in 
		--section "PURCHASES(After Purchase Return)"
		
		--@taxamt=sum(case when srno1='a' then taxamt else -taxamt end) 
		@taxamt=sum(case when srno1 in ('a','d','f') then taxamt else -taxamt end)
		from #tnvatcalc
		where part=1 and srno1 in('a','b','e','d','f') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		1,'h','0',''
		,'PURCHASES(After Purchase Return)','',@gro_amt,'',@taxamt
		)

		if not exists(select * from #tnvatcalc where part=1 and srno1='a')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'a','0','1'
			,'TNVAT PURCHASES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='b')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'b','0','1'
			,'PURCHASE RETURN(VAT)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='d')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'d','0','1'
			,'INTER STATE PURCHASES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='e')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'e','0','1'
			,'PURCHASE RETURN(CST)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=1 and srno1='f')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			1,'f','0','1'
			,'IMPORT PURCHASES','',0,'',0
			)
		end
		--<--part-1
		-->part-2
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'a','0',''
		,'TNVAT SALES',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where st_type='LOCAL' and [rule] not in ('CT-1','CT-3','UT-1','EOU EXPORT') and tax_name <> ''
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'b','0',''
		,'SALES RETURN (TNVAT)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_sr
		where st_type='LOCAL' 
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1='a' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='a' then taxamt else -taxamt end) 
		from #tnvatcalc
		where part=2 and srno1 in('a','b') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		2,'c','0',''
		,'TNVAT SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
		)

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'d','0',''
		,'TNVAT SALES (Net of Sales Return-Service Income)','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_st
		where st_type='LOCAL' and itdesc='Services'--???


		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'E','0',''
		,'INTER STATE SALES',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where st_type='Out of State' and u_imporm <> 'Branch Transfer'
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'f','0',''
		,'SALES RETURN (CST)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_sr
		where st_type='Out of State' 
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(case when srno1='e' then gro_amt else -gro_amt end),@taxamt=sum(case when srno1='e' then taxamt else -taxamt end) 	
		from #tnvatcalc
		where part=2 and srno1 in('e','f') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)

		select
		2,'g','0',''
		,'CST SALES (Net of Sales Returns)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_sr
		where st_type='Out of State' and tax_name like 'C.S.T%'  
		group by itdesc,tax_name

		select @gro_amt=0,@taxamt=0		
		from #tnvatcalc
		where part=2 and srno1 in('g') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
--		values
--		(
--		2,'g','0',''
--		,'CST SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
--		)
--
--		insert into #tnvatcalc
--		(
--		part,srno1,srno2,srno3
--		,trdesc,itdesc,gro_amt,tax_name,taxamt
--		)
		select
		2,'h','0',''
		,'Export Sales',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where  [rule] in ('CT-1','CT-3','UT-1','EOU EXPORT') or st_type = 'Out of Country' and tax_name<>'FORM H'
		group by itdesc,tax_name

		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'h','1',''
		,'Export Sales',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
		from #tnvatcalc_st
		where  [rule] in ('CT-1','CT-3','UT-1','EOU EXPORT') and tax_name='FORM H'
		group by itdesc,tax_name


		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'i','0',''
		,'Total Export','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_st
		where  [rule] in ('CT-1','CT-3','UT-1','EOU EXPORT')

		
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
		-------------------------------------------------------------------------------------
		--Old code
		
		--insert into #tnvatcalc
		--(
		--part,srno1,srno2,srno3
		--,trdesc,itdesc,gro_amt,tax_name,taxamt
		--)
		--select
		--2,'j','0',''
		--,'CST TOTAL SALES (Before Sales Return)','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		--from #tnvatcalc_sr
		--where st_type='Out of State'
		
		--New Code
		
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'j','0',''
		,'CST TOTAL SALES (Before Sales Return)','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc
		where part='2' and srno1 = 'E'
		
		------------------------------------------------------------------------------------
		--Modified by: Rakesh Varma
		--Modified Date: 16-April-2010
		--Changed as per TKT NO:339
--		------------------------------------------------------------------------------------ 
--		
--		--Old code
--		
--		--select @gro_amt=0,@taxamt=0
--		
--		--select @gro_amt=sum(case when srno1='j' then gro_amt else -gro_amt end),
--		--       @taxamt=sum(case when srno1='j' then taxamt else -taxamt end) 
--		--from #tnvatcalc
--		--where part=2 and srno1 in('j','g') and srno2='0'
--		
--		--select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
--		--insert into #tnvatcalc
--		--(
--		--part,srno1,srno2,srno3
--		--,trdesc,itdesc,gro_amt,tax_name,taxamt
--		--)
--		--values
--		--(
--		--2,'k','0',''
--		--,'CST SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
--		--)
--		
--------

		--New Code
		
		select @gro_amt=0,@taxamt=0,@gro_amt1=0,@taxamt1=0
--		
--		select @gro_amt=sum(gro_amt),
--		       @taxamt=sum(taxamt) 
--		from #tnvatcalc
--		where part=2 and srno1 = 'j' and srno2='0'
--		
		select @gro_amt1=sum(gro_amt),
		       @taxamt1=sum(taxamt) 
		from #tnvatcalc
		where part=2 and srno1 = 'g' and srno2='0'
		
		select @gro_amt=isnull(@gro_amt,0)-isnull(@gro_amt1,0),@taxamt=isnull(@taxamt,0)-isnull(@taxamt1,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		2,'k','0',''
		,'CST SALES (Net of Sales Returns)','',@gro_amt,'',@taxamt
		)
--        
--		--Modified by: Sandeep Shah
--		--Modified Date: 17-08-2010
--		--Changed as per TKT NO:3566
--        ------------------------------------------------------------------------------------ 
--		select
--		2,'k','0',''
--		,'CST SALES (Net of Sales Returns)',itdesc,gro_amt=sum(gro_amt),tax_name,taxamt=sum(taxamt)
--		from #tnvatcalc_sr
--		where st_type='Out of State' and tax_name like 'C.S.T%'  
--		group by itdesc,tax_name
--
--		select @gro_amt=0,@taxamt=0		
--		from #tnvatcalc
--		where part=2 and srno1 in('k') and srno2='0'
--
--		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
--		insert into #tnvatcalc
--		(
--		part,srno1,srno2,srno3
--		,trdesc,itdesc,gro_amt,tax_name,taxamt
--		)
--
--	------------------------------------------------------------------------------------ 
		select @gro_amt=0,@taxamt=0
		select @gro_amt=sum(gro_amt),@taxamt=sum(taxamt) 
		from #tnvatcalc
		where part=2 and srno1 in('c','j') and srno2='0'

		select @gro_amt=isnull(@gro_amt,0),@taxamt=isnull(@taxamt,0)
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		values
		(
		2,'l','0',''
		,'GROSS SALES(TNVAT+CST)(Before CST Sales return)','',@gro_amt,'',@taxamt
		)
		--
		insert into #tnvatcalc
		(
		part,srno1,srno2,srno3
		,trdesc,itdesc,gro_amt,tax_name,taxamt
		)
		select
		2,'m','0',''
		,'CST STOCK TRANSFER','',gro_amt=isnull(sum(gro_amt),0),'',taxamt=isnull(sum(taxamt),0)
		from #tnvatcalc_st
		where st_type='Out of State'  and u_imporm='Branch Transfer'
		--blank Record checkin
		if not exists(select * from #tnvatcalc where part=2 and srno1='a')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'a','0','1'
			,'TNVAT SALES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='b')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'b','0','1'
			,'SALES RETURN (TNVAT)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='c')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'c','0','1'
			,'TNVAT SALES (Net of Sales Returns)','',0,'',0
			)
		end

		if not exists(select * from #tnvatcalc where part=2 and srno1='d')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'d','0','1'
			,'TNVAT SALES (Net of Sales Return-Service Income)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='e')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'e','0','1'
			,'INTER STATE SALES','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='f')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'f','0','1'
			,'SALES RETURN (CST)','',0,'',0
			)
		end

		if not exists(select * from #tnvatcalc where part=2 and srno1='g')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'g','0','1'
			,'CST SALES (Net of Sales Returns)','',0,'',0
			)
		end
		if not exists(select * from #tnvatcalc where part=2 and srno1='m')
		begin
			insert into #tnvatcalc
			(
			part,srno1,srno2,srno3
			,trdesc,itdesc,gro_amt,tax_name,taxamt
			)
			values
			(
			2,'m','0',''
			,'CST SALES (Net of Sales Returns)','',0,'',0
			)
		end

		--blank Record checkin
		--<--part-2
	end


select * from #tnvatcalc order by part,srno1,srno2,srno3,itdesc

drop table #tnvatcalc_sr
drop table #tnvatcalc_st
drop table #tnvatcalc_pr
drop table #tnvatcalc_pt
drop table #tnvatcalc




























































