IF EXISTS (SELECT * FROM SYSOBJECTS WHERE XTYPE = 'P' AND [NAME] ='USP_REP_ST_BILLB' )
BEGIN
	DROP PROCEDURE USP_REP_ST_BILLB
END
go
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 26/09/2009
-- Description:	This Stored procedure is useful to generate Sales Tax Invoice Batchwise.
-- Modification Date/By/Reason: 02/10/2009 Rupesh Prajapati. Modified Progressive Total.
-- Modification Date/By/Reason: 22/04/10 Shrikant S. for TKT-6
-- Modification Date/By/Reason: 17/05/10 Ajay Jaiswal for TKT-461
-- Modification Date/By/Reason: 17/01/2011 Rupesh Prajapati for TKT-5692 Add TCS
-- Modification Date/By/Reason: 28/09/2011 Shrikant S. for TKT-9486
-- Modification Date/By/Reason: 10/01/2012 Shrikant S. for Bug-1460(Multi UOM qty fields are require in default)
-- Modification Date/By/Reason: 26/04/2012 Shrikant S. for Bug-3609(To calculate progressive total yearwise)
-- Modification Date/By/Reason: 21/09/2012 Sachin N. S. for BUG-5164
-- Remark:
-- =============================================
Create PROCEDURE [dbo].[USP_REP_ST_BILLB]
	@ENTRYCOND NVARCHAR(254)
	AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000),@SQLCOMMAND1 NVARCHAR(4000),@ParmDefinition NVARCHAR(500)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
	
--	--->Entry_Ty and Tran_Cd Separation --Commented By Shrikant S. on 22/04/2010 for TKT-6
--		declare @ent varchar(2),@trn int,@pos1 int,@pos2 int,@pos3 int--,@ENTRYCOND NVARCHAR(254)
--		
--		print @ENTRYCOND
--		set @pos1=charindex('''',@ENTRYCOND,1)+1
--		set @ent= substring(@ENTRYCOND,@pos1,2)
--		set @pos2=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos1))+1
--		set @pos3=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos2))+1
--		set @trn= substring(@ENTRYCOND,@pos2,@pos2-@pos3)
--		--select * from bpmain where entry_ty=@ent and tran_cd=@trn
--	---<---Entry_Ty and Tran_Cd Separation
	SET @TBLCON=RTRIM(@ENTRYCOND)
	Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
	Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20)

--	--->Progressive Total ----Commented By Shrikant S. on 22/04/2010 for TKT-6
--		Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2),@progtotal numeric(17,2),@date smalldatetime,@inv_no varchar(10),@inv_sr varchar(30)
--		select @date=date,@inv_no=inv_no,@inv_sr=inv_sr from stmain where entry_ty=@ent and tran_cd=@trn
--		select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact
--		if @pformula<>''
--		begin
--			print 'a'
--			select progtotal=(cast (0 as numeric(17,0))) into #progtot from stmain where 1=2
--			set @sqlcommand='insert into #progtot select sum(isnull('+rtrim(@pformula)+',0)) from stmain inner join stitem on (stmain.tran_cd=stitem.tran_cd) where '
--			set @sqlcommand=rtrim(@sqlcommand)+' '+'(stmain.date<='''+rtrim(cast(@date as varchar))+''') and (cast(stmain.inv_no as int)<'+rtrim(@inv_no)+')'
--			if (rtrim(@inv_sr)<>'') begin set @sqlcommand=rtrim(@sqlcommand)+' and stmain.inv_sr ='''+rtrim(@inv_sr)+'''' end
--			if (rtrim(@progcond)<>'') begin set @sqlcommand=rtrim(@sqlcommand)+' and '+rtrim(@progcond) end
--			print @sqlcommand
--			execute sp_executesql @sqlcommand
--			select  @progtotal=@progopamt+isnull(progtotal,0) from #progtot
--			print @progtotal
--		end
--		
--	--<--Progressive Total

-- Added By Shrikant S. on 26/04/2012 for Bug-3609		--Start
Declare @sta_dt datetime, @end_dt datetime 
set @SQLCOMMAND1='select top 1 @sta_dt=sta_dt,@end_dt=end_dt from vudyog..co_mast where dbname=db_name() and ( select top 1 stmain.date  from stmain inner join stitem on (stmain.tran_cd=stitem.tran_cd) where '+@TBLCON+' ) between sta_dt and end_dt '	
set @ParmDefinition =N' @sta_dt datetime Output, @end_dt datetime Output'
EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@sta_dt=@sta_dt Output, @end_dt=@end_dt Output
-- Added By Shrikant S. on 26/04/2012 for Bug-3609		--End


select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact

--Added by Shrikant S. on 22/04/2010 for TKT-6 --------Start --For Progressive Total
	Select Entry_ty,Tran_cd=0,Date,inv_no,itserial=space(6) Into #stmain from stmain Where 1=0
		set @sqlcommand='Insert Into #stmain Select stmain.Entry_ty,stmain.Tran_cd,stmain.date,stmain.inv_no,stitem.itserial from stmain Inner Join stitem on (stmain.Entry_ty=stitem.Entry_ty and stmain.Tran_cd=stitem.Tran_cd) Where '+@TBLCON
		print @sqlcommand
		execute sp_executesql @sqlcommand

		if @pformula<>''  
		Begin
			select progtotal=(cast (0 as numeric(17,0))),inv_no,Tran_Cd=0 into #progtot from stmain where 1=2
			select progtotal=(cast (0 as numeric(17,0))),inv_no,Tran_Cd=0 into #progtot1 from stmain where 1=2
				Declare ProgTotalcur Cursor for
				Select Entry_ty,Tran_cd,Date,Inv_no from #stmain Group by Entry_ty,Tran_cd,Date,Inv_no
				Open ProgTotalcur 
				Fetch Next From ProgTotalcur Into @Entry_ty,@Tran_cd,@Date,@Inv_no 
				While @@Fetch_Status=0
				Begin
					/* Finding the Sum of the Closing of the previous Day */ 
					set @SQLCOMMAND1='select @Sum=sum(isnull('+rtrim(@pformula)+',0)) from stmain inner join stitem on (stmain.tran_cd=stitem.tran_cd)  '	
					set @SQLCOMMAND1=@SQLCOMMAND1+' '+'Where stmain.Date <'''+Convert(Varchar(50),@Date)+''' '
					if (rtrim(@progcond)<>'') begin set @SQLCOMMAND1=rtrim(@SQLCOMMAND1)+' and '+rtrim(@progcond) end
					set @SQLCOMMAND1=rtrim(@SQLCOMMAND1)+' and stmain.date between '''+convert(varchar(50),@sta_dt)+''' and '''+convert(varchar(50),@end_dt)+''' '	-- Added By Shrikant S. on 26/04/2012 for Bug-3609
					set @ParmDefinition =N' @Sum Numeric(19,2) Output'
					EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@Sum=@Progtotal OUTPUT
					print @Progtotal
					Insert Into #progtot1 values(isnull(@Progtotal,0),@Inv_no,@Tran_cd)				
					/* Finding the Sum of the Closing of the previous Day */ 

					/* Finding the Sum of the Present Day */ 
					set @SQLCOMMAND1='select @Sum=sum(isnull('+rtrim(@pformula)+',0)) from stmain inner join stitem on (stmain.tran_cd=stitem.tran_cd)  '	
					set @SQLCOMMAND1=@SQLCOMMAND1+' '+'Where stmain.Date ='''+Convert(Varchar(50),@Date)+'''  and stmain.Tran_cd<'+convert(Varchar(10),@Tran_cd)
					if (rtrim(@progcond)<>'') begin set @SQLCOMMAND1=rtrim(@SQLCOMMAND1)+' and '+rtrim(@progcond) end
					set @ParmDefinition =N' @Sum Numeric(19,2) Output'
					EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@Sum=@Progtotal OUTPUT
					print @Progtotal
					Insert Into #progtot1 values(isnull(@Progtotal,0)+isnull(@progopamt,0),@Inv_no,@Tran_cd)				
					/* Finding the Sum of the Present Day */ 

					Insert Into #progtot Select sum(isnull(progtotal,0)),Inv_no,Tran_cd from #progtot1 Group by Inv_no,Tran_cd
					Delete from #progtot1
					
				Fetch Next From ProgTotalcur Into @Entry_ty,@Tran_cd,@Date,@Inv_no 
				End
				Close ProgTotalcur
				Deallocate ProgTotalcur
		End
--Added by Shrikant S. on 22/04/2010 for TKT-6 --------End

-- Added By Shrikant S. on 10/01/2012 for Bug-1460		--Start
Declare @uom_desc as Varchar(100),@len int,@fld_nm varchar(10),@fld_desc Varchar(10),@count int,@stkl_qty Varchar(100)

select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name())
Create Table #qty_desc (fld_nm varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS,fld_desc varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS)
set @len=len(@uom_desc)
set @stkl_qty=''
If @len>0 
Begin
	while @len>0
	Begin
		set @fld_nm=substring(@uom_desc,1,charindex(':',@uom_desc)-1)
		set @uom_desc=substring(@uom_desc,charindex(':',@uom_desc)+1,@len)
		set @stkl_qty= @stkl_qty +', '+'STITEM.'+@fld_nm

		if @len>0 and charindex(';',@uom_desc)=0
		begin
			set @uom_desc=@uom_desc
			set @fld_desc=@uom_desc
			SET @len=0
		End
		else
		begin
				set @fld_desc=substring(@uom_desc,1,charindex(';',@uom_desc)-1)
				set @uom_desc=substring(@uom_desc,charindex(';',@uom_desc)+1,@len)
				set @len=len(@uom_desc)
		End
		insert into #qty_desc values (@fld_nm,@fld_desc)
	End
End
Else
Begin
	set @stkl_qty=',STITEM.QTY'
End

set @sqlcommand='SELECT  ''REPORT HEADER'' AS REP_HEAD,STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE  '
set @sqlcommand=@sqlcommand+' '+',STMAIN.U_TIMEP,STMAIN.U_TIMEP1,STMAIN.U_REMOVDT,STMAIN.U_EXPLA,STMAIN.U_EXRG23II,STMAIN.U_RG2AMT ' 
set @sqlcommand=@sqlcommand+' '+',STMAIN.EXAMT,STITEM.U_BASDUTY,STITEM.U_CESSPER,STMAIN.U_CESSAMT,STITEM.U_HCESSPER,STMAIN.U_HCESAMT'
set @sqlcommand=@sqlcommand+' '+',STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,STMAIN.U_CHALNO,STMAIN.U_CHALDT,STMAIN.U_PONO,STMAIN.U_PODT'
set @sqlcommand=@sqlcommand+' '+',STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STMAIN.TAXAMT'
set @sqlcommand=@sqlcommand+' '+',STMAIN.NET_AMT,STMAIN.U_PLASR,STMAIN.U_RG23NO,STMAIN.U_RG23CNO'
set @sqlcommand=@sqlcommand+' '+',U_PKNO=(PROJECTITREF.QTY/(CASE WHEN CAST(IT_MAST.AVERAGE AS NUMERIC(10))=0 THEN  1 ELSE '
set @sqlcommand=@sqlcommand+' '+'CAST(IT_MAST.AVERAGE AS NUMERIC(10)) END))'+@stkl_qty+',STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE '
set @sqlcommand=@sqlcommand+' '+',IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT,IT_MAST.[GROUP],IT_MAST.IT_ALIAS '
set @sqlcommand=@sqlcommand+' '+',AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX,AC_MAST.C_TAX'
set @sqlcommand=@sqlcommand+' '+',AC_MAST.ECCNO,AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1'
set @sqlcommand=@sqlcommand+' '+',AC_MAST1.ZIP ZIP1,AC_MAST1.S_TAX S_TAX1,AC_MAST1.I_TAX I_TAX1,AC_MAST1.C_TAX C_TAX1,AC_MAST1.ECCNO ECCNO1     '
set @sqlcommand=@sqlcommand+' '+',AQTY=(CASE WHEN PROJECTITREF.QTY IS NOT NULL THEN PROJECTITREF.QTY ELSE STITEM.QTY END)'
set @sqlcommand=@sqlcommand+' '+',PROJECTITREF.BATCHNO,PROJECTITREF.MFGDT,PROJECTITREF.EXPDT,STITEM.ITSERIAL   '
set @sqlcommand=@sqlcommand+' '+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END) '
set @sqlcommand=@sqlcommand+' '+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'	
set @sqlcommand=@sqlcommand+' '+',stmain.tds_tp,stmain.sc_tp,stmain.ec_tp,stmain.hc_tp,stmain.tcsamt,stmain.stcsamt,stmain.etcsamt,stmain.htcsamt '
set @sqlcommand=@sqlcommand+' '+',mChapno=cast(isnull(substring((Select '', '' +rtrim(chapno) From Stitem Inner Join It_mast on (Stitem.It_code=It_mast.It_code) Where stitem.Entry_ty=stmain.Entry_ty and stitem.tran_cd=stmain.Tran_cd Group by chapno Order By chapno For XML Path('''')),2,2000),'''') as Varchar(2000))'
set @sqlcommand=@sqlcommand+' '+',mEIT_NAME=cast(isnull(substring((Select '', '' +rtrim(eit_name) From Stitem Inner Join It_mast on (Stitem.It_code=It_mast.It_code) Where stitem.Entry_ty=stmain.Entry_ty and stitem.tran_cd=stmain.Tran_cd Group by Eit_name Order By Eit_name For XML Path('''')),2,2000),'''') as Varchar(2000))'
set @sqlcommand=@sqlcommand+' '+',progtotal=isnull(d.progtotal,0)'
set @sqlcommand=@sqlcommand+' '+',STMAIN.SERVTXSRNO'	-- Added by Sachin N. S. on 21/09/2012 for Bug-5164
set @sqlcommand=@sqlcommand+' '+'FROM STMAIN '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD)'          
set @sqlcommand=@sqlcommand+' '+'INNER JOIN #stmain ON (STITEM.TRAN_CD=#stmain.TRAN_CD and STITEM.Entry_ty=#stmain.entry_ty and STITEM.ITSERIAL=#stmain.itserial)'
set @sqlcommand=@sqlcommand+' '+'LEFT JOIN PROJECTITREF ON (PROJECTITREF.ENTRY_TY=STITEM.ENTRY_TY AND PROJECTITREF.TRAN_CD=STITEM.TRAN_CD'
set @sqlcommand=@sqlcommand+' '+'AND PROJECTITREF.ITSERIAL=STITEM.ITSERIAL)'
set @sqlcommand=@sqlcommand+' '+'INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE)'       
set @sqlcommand=@sqlcommand+' '+'INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID)'         
set @sqlcommand=@sqlcommand+' '+'LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=STMAIN.U_DELIVER )'
set @sqlcommand=@sqlcommand+' '+'Left join #progtot d on (stmain.Tran_cd=d.Tran_cd)'	
set @sqlcommand=@sqlcommand+' '+'ORDER BY STMAIN.INV_SR,STMAIN.INV_NO,PROJECTITREF.BATCHNO,PROJECTITREF.ITSERIAL'      
execute sp_executesql @sqlcommand	
-- Added By Shrikant S. on 10/01/2012 for Bug-1460		--End


/*		Commented By Shrikant S. on 10/01/2012 for Bug-1460		--Start
SELECT   'REPORT HEADER' AS REP_HEAD,STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE  
,STMAIN.U_TIMEP,STMAIN.U_TIMEP1,STMAIN.U_REMOVDT,STMAIN.U_EXPLA,STMAIN.U_EXRG23II,STMAIN.U_RG2AMT  
,STMAIN.EXAMT,STITEM.U_BASDUTY,STITEM.U_CESSPER,STMAIN.U_CESSAMT,STITEM.U_HCESSPER,STMAIN.U_HCESAMT
,STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,STMAIN.U_CHALNO,STMAIN.U_CHALDT,STMAIN.U_PONO,STMAIN.U_PODT
,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STMAIN.TAXAMT
,STMAIN.NET_AMT,STMAIN.U_PLASR,STMAIN.U_RG23NO,STMAIN.U_RG23CNO
,U_PKNO=(PROJECTITREF.QTY/(CASE WHEN CAST(IT_MAST.AVERAGE AS NUMERIC(10))=0 THEN  1 ELSE 
CAST(IT_MAST.AVERAGE AS NUMERIC(10)) END)),STITEM.QTY,STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE 
,IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT,IT_MAST.[GROUP],IT_MAST.IT_ALIAS 
,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX,AC_MAST.C_TAX
,AC_MAST.ECCNO,AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1
,AC_MAST1.ZIP ZIP1,AC_MAST1.S_TAX S_TAX1,AC_MAST1.I_TAX I_TAX1,AC_MAST1.C_TAX C_TAX1,AC_MAST1.ECCNO ECCNO1     
,AQTY=(CASE WHEN PROJECTITREF.QTY IS NOT NULL THEN PROJECTITREF.QTY ELSE STITEM.QTY END)
,PROJECTITREF.BATCHNO,PROJECTITREF.MFGDT,PROJECTITREF.EXPDT,STITEM.ITSERIAL   
--,progtotal=isnull(@progtotal,0)      
--Added by Ajay Jaiswal on 17/05/2010 for TKT-461 ---> Start
,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'')='' THEN it_mast.it_name ELSE it_mast.it_alias END) 
,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'')='' THEN ac_mast.ac_name ELSE ac_mast.mailname END)	
,stmain.tds_tp,stmain.sc_tp,stmain.ec_tp,stmain.hc_tp,stmain.tcsamt,stmain.stcsamt,stmain.etcsamt,stmain.htcsamt /*TKT-5692*/
--Added by Ajay Jaiswal on 17/05/2010 for TKT-461 ---> End
into #stmain1 FROM STMAIN           
INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD)          
INNER JOIN #stmain ON (STITEM.TRAN_CD=#stmain.TRAN_CD and STITEM.Entry_ty=#stmain.entry_ty and STITEM.ITSERIAL=#stmain.itserial) --Added by Shrikant S. on 22/04/2010 for TKT-6 
LEFT JOIN PROJECTITREF ON (PROJECTITREF.ENTRY_TY=STITEM.ENTRY_TY AND PROJECTITREF.TRAN_CD=STITEM.TRAN_CD 
AND PROJECTITREF.ITSERIAL=STITEM.ITSERIAL)      
INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE)       
INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID)         
LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=STMAIN.U_DELIVER )         
ORDER BY STMAIN.INV_SR,STMAIN.INV_NO					-- Added By Shrikant S. on 28/09/2011 for TKT-9486  
--ORDER BY STMAIN.INV_SR,CAST(STMAIN.INV_NO  AS INT)	-- Commented By Shrikant S. on 28/09/2011 for TKT-9486
,PROJECTITREF.BATCHNO,PROJECTITREF.ITSERIAL      


	set @mchapno=' '
	set @meit_name=' '
--Added by Shrikant S. on 22/04/2010 for TKT-6 --------Start
	Create table #stChapno(mchapno varchar(250),Tran_cd Numeric,Entry_ty Varchar(2))
	Declare @tmpTran Numeric
	declare cur_stbill cursor for select CHAPNO,Tran_cd,Entry_ty from #stmain1 --> Changed for TKT-447
	open cur_stbill 
	fetch next from cur_stbill into @chapno,@Tran_cd,@Entry_ty
	set @tmpTran=@Tran_cd
	while(@@fetch_status=0)
	begin
		set @mchapno=rtrim(@mchapno)+rtrim(@chapno)+','
		fetch next from cur_stbill into @chapno,@Tran_cd,@Entry_ty
		if @tmpTran<>@Tran_cd
		Begin
			Insert Into #stChapno values(@mchapno,@tmpTran,@Entry_ty)
			set @mchapno=''
			set @tmpTran=@Tran_cd	
		End
	end
	close cur_stbill
	deallocate cur_stbill
	Insert Into #stChapno values(RTRIM(@mchapno),@tmpTran,@Entry_ty)


	--declare cur_stbill cursor for select distinct eit_name from #stmain
	Create table #steit_name(meit_name varchar(250),Tran_cd Numeric,Entry_ty Varchar(2))
	declare cur_stbill cursor for select eit_name,Tran_cd,Entry_ty from #stmain1	--> Changed for TKT-447
	open cur_stbill 
	fetch next from cur_stbill into @eit_name,@Tran_cd,@Entry_ty
	set @tmpTran=@Tran_cd
	while(@@fetch_status=0)
	begin
		set @meit_name=rtrim(@meit_name)+rtrim(@eit_name)+','
		fetch next from cur_stbill into @eit_name,@Tran_cd,@Entry_ty
		if @tmpTran<>@Tran_cd
		Begin
			Insert Into #steit_name values(@meit_name,@tmpTran,@Entry_ty)
			set @meit_name=''
			set @tmpTran=@Tran_cd	
		End
	end
	close cur_stbill
	deallocate cur_stbill	
	Insert Into #steit_name values(RTRIM(@meit_name),@tmpTran,@Entry_ty)

	Update #stChapno set mChapno=case when len(RTRIM(mChapno))>1 then substring(mChapno,1,len(mChapno)-1)else '' end
	Update #steit_name set meit_name=case when len(RTRIM(meit_name))>=1 then substring(meit_name,1,len(meit_name)-1) else '' end
	Update #stChapno set mChapno=case when len(RTRIM(mChapno))=1 then '' else RTRIM(mChapno) end
	Update #steit_name set meit_name=case when len(RTRIM(meit_name))=1 then '' else RTRIM(meit_name) end

	SELECT a.* 
	,mChapno=ISNULL(b.mChapno,'')
	,mEIT_NAME=ISNULL(c.mEIT_NAME,'')
	,progtotal=isnull(d.progtotal,0)
	FROM #STMAIN1 a
	Left join (Select Entry_ty,Tran_cd,mChapno from #stChapno Group by Entry_ty,Tran_cd,mChapno) b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd)
	Left join (Select Entry_ty,Tran_cd,mEIT_NAME from #steit_name Group by Entry_ty,Tran_cd,mEIT_NAME) c on (a.Entry_ty=c.Entry_ty and a.Tran_cd=c.Tran_cd)
	Left join #progtot d on (a.Tran_cd=d.Tran_cd)
	Order by a.INV_NO  ,BATCHNO,ITSERIAL				-- Added By Shrikant S. on 28/09/2011 for TKT-9486  
--Order by CAST(a.INV_NO  AS INT),BATCHNO,ITSERIAL		-- Commented By Shrikant S. on 28/09/2011 for TKT-9486
Drop table #stmain
Drop table #stmain1
Drop table #stChapno
Drop table #steit_name
Drop table #progtot
--Added by Shrikant S. on 22/04/2010 for TKT-6 --------End
Commented By Shrikant S. on 10/01/2012 for Bug-1460		--End */

/*	----Commented By Shrikant S. on 22/04/2010 for TKT-6
	declare cur_stbill cursor for select distinct CHAPNO from #stmain
	open cur_stbill 
	fetch next from cur_stbill into @chapno
	while(@@fetch_status=0)
	begin
		set @mchapno=rtrim(@mchapno)+','+rtrim(@chapno)
		fetch next from cur_stbill into @chapno
	end
	close cur_stbill
	deallocate cur_stbill
	
	declare cur_stbill cursor for select distinct eit_name from #stmain
	open cur_stbill 
	fetch next from cur_stbill into @eit_name
	while(@@fetch_status=0)
	begin
		set @meit_name=rtrim(@meit_name)+','+rtrim(@eit_name)
		fetch next from cur_stbill into @eit_name
	end
	close cur_stbill
	deallocate cur_stbill	
	
	set @mChapno=substring(@mChapno,2,len(@mChapno)-1)
	set @mEIT_NAME=substring(@mEIT_NAME,2,len(@mEIT_NAME)-1)
	SELECT * 
	,mChapno=ISNULL(@mChapno,'')
	,mEIT_NAME=ISNULL(@mEIT_NAME,'')
	FROM #STMAIN
*/

