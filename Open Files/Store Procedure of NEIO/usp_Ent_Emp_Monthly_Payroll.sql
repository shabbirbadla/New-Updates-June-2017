If Exists(Select xType,Name from Sysobjects where xType='P' and Name='usp_Ent_Emp_Monthly_Payroll')

Begin
	Drop PROCEDURE usp_Ent_Emp_Monthly_Payroll
End
Go
/****** Object:  StoredProcedure [dbo].[usp_Ent_Emp_Monthly_Payroll]    Script Date: 07/11/2015 10:04:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Rupesh
-- Create date: 08/09/2012
-- Description:This SP is Used Monthly Payroll Calculation
-- Modify date:
-- Remark:
/*
Modification By : Archana
Modification On : 05-09-2013
Bug Details		: Bug-18246 (Error showing when we preview Service Tax Payable report)
Search for		: Bug-18246
Modified By/On  : Sachin N. S. on 02-01-2014 for Bug-21047
Modified By/On  : Sachin N. S. on 18-02-2014 for Bug-21542
Modified By/On  : Sachin N. S. on 24-06-2014 for Bug-21114
Modified By/On	: Kishor A. On 11/07/2015 for Bug-26429
*/
-- =============================================

CREATE  procedure [dbo].[usp_Ent_Emp_Monthly_Payroll]
@Tran_Cd int,@Pay_Year varchar(30),@Pay_Month int,@Loc_Nm varchar(100),@FiltPara varchar(4000),@FinYear Varchar(12),@TmpTableName Varchar(200) /*TDS Projection*/
as
begin
	--set @FiltPara=''
	Declare @SqlCommand nvarchar(4000)
	Declare @Lv_Code varchar(30),@SortOrd int,@StateExist int
	Declare @EDType varchar(6),@Short_Nm varchar(30),@Fld_Nm varchar(30),@MonthlyEdit bit,@Def_Rate Decimal(12,3),@Slab_Rate Decimal(12,3),@Formula varchar(4000),@AmtExpr varchar(600),@Slab_Appl Bit,@PayEffect varchar(1)
	Declare @State varchar(60),@RangeFrom Decimal(17,3),@RangeTo  Decimal(17,3),@Percentage  Decimal(17,3),@Amount Decimal(10,3),@Round_Off Bit
	Declare @Loc_Code varchar(10)
	Declare @EarCols varchar(1000),@DedCols Varchar(1000),@EmployeeCode varchar(15)--,@Amount
	Declare @Fld_NmYn int --Ramya 14535
	Declare @TAmount Decimal(10,3)--Archana 18246 
	Select @EarCols='0',@DedCols='0'
    --Declare @edate datetime        /*Ramya 08/03/2013*/ Commented by Kishor A. for Bug-26429 on 24/07/2015
    Declare @edate smalldatetime --Added by Kishor A. for Bug-26429 on 24/07/2015
    Declare @xDate smalldatetime,@ZDate varchar(50),@WDate varchar(50) --Added by Kishor A. for Bug-26429 on 24/07/2015
	print  @Pay_Month
	Set @eDate=cast('1/'+cast(@Pay_Month as varchar)+ '/'+@Pay_Year  as datetime)
	SET @xDate = ''+(CONVERT(VARCHAR(50),@Edate,103))+'' --Added by Kishor A. for Bug-26429 on 24/07/2015
	
	Set @Loc_Code=''
--	Select * into #Emp_Monthly_Payroll From Emp_Monthly_Payroll Where 1=2
--	Select a.*,b.CalcPeriod into #Emp_Monthly_Payroll From Emp_Monthly_Payroll a inner join EmployeeMast b on a.employeeCode=b.EmployeeCode Where 1=2		-- Changed by Sachin N. S. on 09/07/2014 for Bug-21114 Commented by Kishor A. for Bug-26429 on 11/07/2015
	Select a.*,b.CalcPeriod,PTDate=b.doj into #Emp_Monthly_Payroll From Emp_Monthly_Payroll a inner join EmployeeMast b on a.employeeCode=b.EmployeeCode Where 1=2	--Added By Kishor A. for Bug-26429 on 11/07/2015
		
--	Set @SqlCommand='Insert Into #Emp_Monthly_Payroll (EmployeeCode,Pay_Year,Pay_Month,PayGenerated,FinYear)		-- Commented by Sachin N. S. on 21/06/2014 for Bug-21114
	Set @SqlCommand='Insert Into #Emp_Monthly_Payroll (EmployeeCode,Pay_Year,Pay_Month,PayGenerated,FinYear,WrkHrs,CalcPeriod)'+		-- Changed by Sachin N. S. on 21/06/2014 for Bug-21114
					'Select m.EmployeeCode,'+char(39)+@Pay_Year+char(39)+','+cast(@Pay_Month as varchar)+',0,'+char(39)+@FinYear+char(39)+',m.WrkHrs,e.CalcPeriod '+ -- Changed by Sachin N. S. on 21/06/2014 for Bug-21114
					'From Emp_Monthly_Muster m inner join EmployeeMast e on (m.EmployeeCode=e.EmployeeCode) Where Pay_Year='+char(39)+@Pay_Year+char(39)+
					' and Pay_Month='+Cast(@Pay_Month as varchar) --+' and  m.EmployeeCode=''P00004'''
	Set @SqlCommand=@SqlCommand+ +' and (isnull(dol,''1900-01-01 00:00:00.000'')=''1900-01-01 00:00:00.000'' or (month(dol)>='+Cast(@Pay_Month as varchar) +' and year(dol)>='+cast(@Pay_Year as varchar)+'))'	-- Added by Sachin N. S. on 02/01/2014 for Bug-21047
	if(@Loc_Code<>'')
	begin
		set @FiltPara=@FiltPara+' and Loc_Code='+char(39)+@Loc_Code+char(39)
	end	
	Set @SqlCommand=@SqlCommand+@FiltPara
	if(@Tran_Cd=0) /*Add Mode*/
	begin
		set @SqlCommand=@SqlCommand+' and e.EmployeeCode not in (Select EmployeeCode from Emp_Monthly_Payroll where PayGenerated=1 and Pay_Year='+char(39)+@Pay_Year+char(39)+' and Pay_Month='+cast(@Pay_Month as varchar)+')'
	end
	else /*Edit Mode*/
	begin
		set @SqlCommand=@SqlCommand+' and e.EmployeeCode not in (Select EmployeeCode from Emp_Monthly_Payroll where Tran_cd ='+cast(@Tran_Cd as varchar)+' union Select EmployeeCode from Emp_Monthly_Payroll where PayGenerated=1 and Pay_Year='+char(39)+@Pay_Year+char(39)+' and Pay_Month='+cast(@Pay_Month as varchar)+')'
	end
	
	print @SqlCommand
	execute sp_ExecuteSql @SqlCommand

	if  (isnull(@TmpTableName,'')<>'') /*Projection*/
	begin
		print 'Proj1'
		set @SqlCommand=Replace(@FiltPara,'and e.employeeCode=','')
		set @SqlCommand=Replace(@SqlCommand,char(39),'')
		set @SqlCommand=Replace(@SqlCommand,' ','')
			print 'Proj2'
		if not exists (Select EmployeeCode From #Emp_Monthly_Payroll where EmployeeCode=@SqlCommand and Pay_Year=@Pay_Year and Pay_Month=@Pay_Month)
		begin
			print 'Proj3'
			Insert Into #Emp_Monthly_Payroll (EmployeeCode,Pay_Year,Pay_Month,PayGenerated,FinYear) values (@SqlCommand,@Pay_Year,@Pay_Month,0,@FinYear)	
		end
	End
	
	--Select 'a' as 'a',* From #Emp_Monthly_Payroll
	if(isnull(@TmpTableName,'')='') /*Projection*/
	begin
		--Set @SqlCommand='Update a Set a.MonthDays=isnull(b.MonthDays,0),a.ProcDate=isnull(b.ProcDate,''''),a.SalPaidDays=isnull(b.SalPaidDays,0),a.NetPayMent=isnull(b.NetPayMent,0),a.Tran_Cd=isnull(b.Tran_Cd,0),a.Inv_No=isnull(b.Inv_No,''''),a.PayGenerated=isnull(b.PayGenerated,0)' Commented by Kishor A. On 11/07/2015 for Bug-26429
		Set @SqlCommand='Update a Set a.PTDate= ''2015-04-01 00:00:00.000'',a.MonthDays=isnull(b.MonthDays,0),a.ProcDate=isnull(b.ProcDate,''''),a.SalPaidDays=isnull(b.SalPaidDays,0),a.NetPayMent=isnull(b.NetPayMent,0),a.Tran_Cd=isnull(b.Tran_Cd,0),a.Inv_No=isnull(b.Inv_No,''''),a.PayGenerated=isnull(b.PayGenerated,0)' --Added by Kishor A. On 11/07/2015 for Bug-26429
		--Declare cur_pay_head cursor for Select Fld_Nm From Emp_Pay_Head_Master Where (isDeActive=0 or (isDeActive=1 and DeactFrom>@eDate ) )Order By Fld_Nm
        Declare cur_pay_head cursor for Select Fld_Nm From Emp_Pay_Head_Master Where (isDeActive=0 or (isDeActive=1 and DeactFrom>@eDate ) )Order By Fld_Nm
		open cur_pay_head
		fetch Next From cur_pay_head into @Fld_Nm
		while(@@Fetch_Status=0)
		begin
			Set @SqlCommand=@SqlCommand+',a.'+@Fld_Nm+'=isnull(b.'+@Fld_Nm+',0)'
			fetch Next From cur_pay_head into @Fld_Nm
		end
		close cur_pay_head
		DeAllocate cur_pay_head	
		Set @SqlCommand=@SqlCommand+' From #Emp_Monthly_Payroll a Left Join Emp_Monthly_Payroll b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_Month) '
		print @SqlCommand
		execute sp_ExecuteSql @SqlCommand
		update a set a.SalPaidDays=b.SalPaidDays From #Emp_Monthly_Payroll a inner join Emp_Monthly_Muster b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_Month) Where PayGenerated=0
	end
	else
	begin
		Update #Emp_Monthly_Payroll Set MonthDays=dbo.funMonthDays(@Pay_Month,cast(Pay_Year as int)),SalPaidDays=dbo.funMonthDays(@Pay_Month,cast(Pay_Year as int)) 
	end		
	--Select 'b' as 'b',* From #Emp_Monthly_Payroll	
	
	Declare  cur_mPayroll Cursor for
	Select e.HeadTypeCode,e.Short_Nm,e.Fld_Nm,PayEditable,Def_Rate,Formula,Round_Off,Slab_Appl,h.payEffect from Emp_Pay_Head_Master e 
	inner join Emp_Pay_Head h on (e.HeadTypeCode=h.HeadTypeCode)
    Where (isDeActive=0 or (isDeActive=1 and DeactFrom>@eDate ) )
	and e.HeadTypeCode not in ('LON','ADV') /*Loan*/
	order by h.SortOrd,e.SortOrd,h.PayEffect


	open cur_mPayroll
	Fetch next From cur_mPayroll into @EDType,@Short_Nm,@Fld_Nm,@MonthlyEdit,@Def_Rate,@Formula,@Round_Off,@Slab_Appl,@PayEffect
	while (@@Fetch_Status=0)
	begin
		if(@EDType='E' or @EDType='BNS') /*Ramya Bug-7977*/
		begin
			Set @EarCols=@EarCols+'+isnull('+@Fld_Nm+',0)'
		end
		if(@EDType='D' or @EDType='ESD' or @EDType='LON' or @EDType='ADV')  /*Ramya Bug-7977*/
		begin
			Set @DedCols=@DedCols+'+isnull('+@Fld_Nm+',0)'
		end
		

		set @Formula=Replace(@Formula,'Emp_Monthly_Payroll.','p.')
		set @Formula=Replace(@Formula,'EmployeeMast.','e.')
		set @Formula=Replace(@Formula,'Emp_Monthly_Muster.','m.')
		set @Formula=Replace(@Formula,'Emp_Pay_head_Details.','eed.') --Pay Head Issue
		set @Formula=Replace(@Formula,'{','')
		set @Formula=Replace(@Formula,'}','')

		if (@Slab_Appl=0 and isnull(@Formula,'')='')/*Values From Employee Master ED Details*/
		begin
			Set @SqlCommand='Update p set '
			set @AmtExpr= 'p.'+ @Fld_Nm+'='
			if isnull(@Round_Off,0)<>0
			begin
				set @AmtExpr=@AmtExpr+'isnull(Round(eed.'+@Fld_Nm+',0),0)'
			end
			else
			begin
				set @AmtExpr=@AmtExpr+'isnull(eed.'+@Fld_Nm+',0)'
			end

			Set @SqlCommand=rtrim(@SqlCommand)+' '+@AmtExpr+' From  #Emp_Monthly_Payroll p'
			if(isnull(@TmpTableName,'')='') /*Projection*/
			begin
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Monthly_Muster m on (p.Pay_Year=m.Pay_Year and p.Pay_Month=m.Pay_Month and p.EmployeeCode =m.EmployeeCode and m.SalPaidDays<>0)'
			end
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast e on (e.EmployeeCode =p.EmployeeCode and e.EmployeeCode in (Select EmployeeCode From Emp_Pay_Head_Details where isnull('+@Fld_Nm+'YN,0)=1  and isnull('+@Fld_Nm+',0)<>0'+') )'
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Details eed on (e.EmployeeCode =eed.EmployeeCode)'
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join Loc_master Lc on (e.Loc_Code=Lc.Loc_Code)' 
			set @SqlCommand=rtrim(@SqlCommand)+' '+' where p.Pay_Year='+cast(@Pay_Year as varchar)
			set @SqlCommand=rtrim(@SqlCommand)+' '+' and p.Pay_Month='+cast(@Pay_Month as varchar)

			if isnull(@FiltPara,'')<>''
			Begin
				set @SqlCommand=rtrim(@SqlCommand)+' '+@FiltPara
			end
			print '1 '+@SqlCommand
			Execute Sp_ExecuteSql @SqlCommand
			
		end
		
		if (@Slab_Appl=0 and isnull(@Formula,'')<>'')
		begin
			Set @SqlCommand='Update p set p.'+ @Fld_Nm+'='
			if isnull(@Def_Rate,0)<>0
			begin
				set @Formula='('+@Formula+'*'+rtrim(cast(@Def_Rate as varchar))+')/100'
			end
			if isnull(@Round_Off,0)<>0
			begin
				set @Formula='Round('+@Formula+',0)'
			end
			Set @SqlCommand=rtrim(@SqlCommand)+' isnull('+@Formula+',0) From  #Emp_Monthly_Payroll p'
			if(isnull(@TmpTableName,'')='')
			begin
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Monthly_Muster m on (p.Pay_Year=m.Pay_Year and p.Pay_Month=m.Pay_Month and p.EmployeeCode =m.EmployeeCode and m.SalPaidDays<>0)'
			end
			else
			begin
				Set @SqlCommand=Replace(rtrim(@SqlCommand),'m.MonthDays','p.MonthDays')
				Set @SqlCommand=Replace(rtrim(@SqlCommand),'m.SalPaidDays','p.SalPaidDays')	
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join Emp_Monthly_Muster m on (p.Pay_Year=m.Pay_Year and p.Pay_Month=m.Pay_Month and p.EmployeeCode =m.EmployeeCode and m.SalPaidDays<>0)'  /*Rupesh for TDS Projection*/
			end
			--payhedIssue
			--Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast e on (rtrim(e.EmployeeCode) =rtrim(p.EmployeeCode) and rtrim(e.EmployeeCode) in (Select rtrim(EmployeeCode) From Emp_Pay_Head_Details where isnull('+@Fld_Nm+'YN,0)=1 ) )' /*Rup*/
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast e on (rtrim(e.EmployeeCode) =rtrim(p.EmployeeCode))' /*Rup*/
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Details eed on (e.EmployeeCode=eed.EmployeeCode and  isnull('+@Fld_Nm+'YN,0)=1 )'
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join Loc_master Lc on (e.Loc_Code=Lc.Loc_Code)' 
			set @SqlCommand=rtrim(@SqlCommand)+' '+' where p.Pay_Year='+cast(@Pay_Year as varchar)
			set @SqlCommand=rtrim(@SqlCommand)+' '+' and p.Pay_Month='+cast(@Pay_Month as varchar)
			
			if isnull(@FiltPara,'')<>''
			Begin
				set @SqlCommand=rtrim(@SqlCommand)+' '+@FiltPara
			end

			print '2 '+ @SqlCommand
			--Select @SqlCommand
			Execute Sp_ExecuteSql @SqlCommand
		end
		
		--Added by Kishor A. for Bug-26429 on 24/07/2015 Start..
		Set @WDate = (Case When @xDate BETWEEN '01/04/1900' And '31/03/2015' Then  '01/04/1900' Else '01/04/2015'  End)
		SET @ZDate = (Case When @xDate BETWEEN '01/04/1900' And '31/03/2015' Then  '31/03/2015' Else (CONVERT(VARCHAR(50),@edate,103)) End)
		--Added by Kishor A. for Bug-26429 on 24/07/2015 End..		
		
		set @StateExist=0
		Select @StateExist=isnull(count(State),0) from Emp_Pay_Head_Slab_Master where Fld_Nm=@Fld_Nm and State <>''
		if (@Slab_Appl=1 and isnull(@Formula,'')<>'')
		begin
			Set @SqlCommand='Declare Cur_Slab Cursor for'
--			Set @SqlCommand=rtrim(@SqlCommand)+' '+'Select Distinct State,RangeFrom,RangeTo,Percentage,Amount from Emp_Pay_Head_Slab_Master where (GETDATE() > sDate ) and Fld_Nm='+char(39)+@Fld_Nm+char(39) Commented By Kishor A. for Bug-26429
			Set @SqlCommand=rtrim(@SqlCommand)+' '+'Select Distinct State,RangeFrom,RangeTo,Percentage,Amount from Emp_Pay_Head_Slab_Master where sDate between '''+@WDate+''' and '''+@ZDate+''' and Fld_Nm='+char(39)+@Fld_Nm+char(39) --Added By Kishor A. for Bug-26429
			if @StateExist>0
			begin
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'and State in ('
				Set @SqlCommand=rtrim(@SqlCommand)+' '+' Select distinct e.StateName from EmployeeMast e Left join Loc_master Lc on (e.Loc_Code=Lc.Loc_Code) Where 1=1 '
				if isnull(@FiltPara,'')<>''
				Begin
					set @SqlCommand=rtrim(@SqlCommand)+' '+@FiltPara
				end
				Set @SqlCommand=rtrim(@SqlCommand)+' '+')' 

			end
			print 'a'+@SqlCommand			
  			Execute Sp_ExecuteSql @SqlCommand 
			open Cur_Slab
			fetch next from Cur_Slab into @State,@RangeFrom,@RangeTo,@Percentage,@Amount
			while(@@Fetch_Status=0)
			Begin
				Set @SqlCommand='Update p set p.'+ @Fld_Nm+'='
				print '3'
				print @Percentage
				print @Fld_Nm
				print 'p1 '+@Formula
				Set @AmtExpr=@Formula
				if isnull(@Percentage,0)<>0
				begin
					set @AmtExpr='('+@Formula+'*'+rtrim(cast(@Percentage as varchar))+')/100' 	
					--set @Formula='('+@Formula+'*'+rtrim(cast(@Percentage as varchar))+')/100'
				end
				else
				begin
					set @AmtExpr=cast(@Amount as varchar)
					--set @Formula=cast(@Amount as varchar)
				end
				print 'p2 '+@Formula
				if isnull(@Round_Off,0)<>0
				begin
					--set @Formula='Round('+@Formula+',0)' 
					--set @AmtExpr='Round('+@AmtExpr+',0)' Commented By Kishor A. for Bug-26429 on 11/07/2015
					set @AmtExpr='(Case When e.sex=''F'' And e.StateName=''Maharashtra'' And P.PTDate >= ''2015-04-01 00:00:00.000'' And ('+@Formula+')<=10000 Then 0 When '+cast(@Pay_Month as varchar)+'=2 And ('+@Formula+')>10000 Then Round('+@AmtExpr+',0)+100 Else Round('+@AmtExpr+',0) End)' -- Added By Kishor A. for Bug-26429 on 11/07/2015
				end
				print 'p3 '+@Formula
				Set @SqlCommand=rtrim(@SqlCommand)+''+@AmtExpr+' From  #Emp_Monthly_Payroll p'
				-->
				--Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Monthly_Muster m on (p.Pay_Year=m.Pay_Year and p.Pay_Month=m.Pay_Month and p.EmployeeCode =m.EmployeeCode)'
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast e on (e.EmployeeCode =p.EmployeeCode)'
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Details eph on (p.EmployeeCode =eph.EmployeeCode and eph.'+@Fld_Nm+'YN<>0)'
				Set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join Loc_master Lc on (e.Loc_Code=Lc.Loc_Code)' 
				set @SqlCommand=rtrim(@SqlCommand)+' '+' where p.Pay_Year='+cast(@Pay_Year as varchar)
				set @SqlCommand=rtrim(@SqlCommand)+' '+' and p.Pay_Month='+cast(@Pay_Month as varchar)
				set @SqlCommand=rtrim(@SqlCommand)+' '+' and ('+@Formula+' between '+cast(@RangeFrom as varchar)+' and '+cast(@RangeTo as varchar)+')'
				if @StateExist>0
				begin
					set @SqlCommand=rtrim(@SqlCommand)+' '+' and StateName ='+char(39)+@State+Char(39)
				end
				if isnull(@FiltPara,'')<>''
				Begin
					set @SqlCommand=rtrim(@SqlCommand)+' '+@FiltPara
				end
				
				print ' 3 '+ @SqlCommand
				Execute Sp_ExecuteSql @SqlCommand
				
				fetch next from Cur_Slab into @State,@RangeFrom,@RangeTo,@Percentage,@Amount
			end
			close Cur_Slab
			Deallocate Cur_Slab
		end
		

		Set @SqlCommand='update p set '+@Fld_Nm+'=0 From  #Emp_Monthly_Payroll p '
		Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Details e on (e.EmployeeCode =p.EmployeeCode)'
		Set @SqlCommand=rtrim(@SqlCommand)+' '+'where '+@Fld_Nm+'yn=0'
		print ' 4 '+ @SqlCommand
		Execute Sp_ExecuteSql @SqlCommand

		Fetch next From cur_mPayroll into @EDType,@Short_Nm,@Fld_Nm,@MonthlyEdit,@Def_Rate,@Formula,@Round_Off,@Slab_Appl,@PayEffect
	end
	close cur_mPayroll
	DeAllocate cur_mPayroll
	/*--->Loan And Advances*/

--	insert into #Emp_Monthly_Payroll select * From Emp_Monthly_Payroll where tran_cd=@Tran_Cd and Tran_cd<>0  /*Ramya 06/06/13*/
--	insert into #Emp_Monthly_Payroll select a.*,b.CalcPeriod,PtDate = B.DOJ From Emp_Monthly_Payroll a inner join EmployeeMast b on a.EmployeeCode=b.EmployeeCode where a.tran_cd=@Tran_Cd and a.Tran_cd<>0  -- Changed by Sachin N. S. on 09/07/2014 for Bug-21114 Commented By Kishor A. for Bug-26429 on 11/07/2015
	insert into #Emp_Monthly_Payroll select a.*,b.CalcPeriod,PtDate = B.DOJ From Emp_Monthly_Payroll a inner join EmployeeMast b on a.EmployeeCode=b.EmployeeCode where a.tran_cd=@Tran_Cd and a.Tran_cd<>0  -- Added By Kishor A. for Bug-26429 on 11/07/2015	
    
    Set @SqlCommand='update #Emp_Monthly_Payroll Set NetPayment=(isnull('+@EarCols+',0))-(isnull('+@DedCols+',0))'
	print @SqlCommand
	Execute Sp_ExecuteSql @SqlCommand
	Set @SqlCommand='update #Emp_Monthly_Payroll Set GrossPayment=(isnull('+@EarCols+',0))' /*Projection*/
	print @SqlCommand
	Execute Sp_ExecuteSql @SqlCommand
	set @TAmount=0 --Added by Archana K. on 17/09/13 for Bug-18246
	Declare cur_Loan  Cursor for
	--Select a.EmployeeCode,a.Fld_Nm,b.Inst_Amt+b.Interest from Emp_Loan_Advance a inner join Emp_Loan_Advance_Details b on (a.Tran_Cd=b.Tran_cd) where Pay_Year=@Pay_Year and Pay_Month=@Pay_Month
    --where Pay_Year=@Pay_Year and Pay_Month=@Pay_Month
--
--	Select a.EmployeeCode,a.Fld_Nm,b.Inst_Amt+b.Interest from Emp_Loan_Advance a inner join Emp_Loan_Advance_Details b on (a.Tran_Cd=b.Tran_cd) Inner Join #Emp_Monthly_Payroll c on (a.EmployeeCode=c.EmployeeCode)
--	where b.Pay_Year=@Pay_Year and b.Pay_Month=@Pay_Month    /*Ramya for Bug-14222*/--commented by Archana K. for Bug-18246

	Select a.EmployeeCode,a.Fld_Nm,lamt=CASE WHEN b.Repay_amt = 0 THEN b.Proj_repay ELSE b.Repay_amt END  from Emp_Loan_Advance a inner join Emp_Loan_Advance_Details b on (a.Tran_Cd=b.Tran_cd) Inner Join #Emp_Monthly_Payroll c on (a.EmployeeCode=c.EmployeeCode)
	where b.Pay_Year=@Pay_Year and b.Pay_Month=@Pay_Month    /*Ramya for Bug-14222*/--Changed by Archana K. on 17/09/13 for Bug-18246

	Open cur_Loan
	Fetch next From cur_Loan into @EmployeeCode,@Fld_Nm,@Amount
	while(@@Fetch_Status=0)
	begin
		--Set @SqlCommand='update #Emp_Monthly_Payroll Set '+@Fld_Nm+'='+rtrim(cast(@Amount as varchar))+' where EmployeeCode='+char(39)+@EmployeeCode+char(39)
	set @TAmount=@TAmount+@Amount
--	Set @SqlCommand='update p Set '+@Fld_Nm+'='+rtrim(cast(@TAmount as varchar))+' from #Emp_Monthly_Payroll p '--commented by Archana K.on 17/09/13 for Bug-18246
	Set @SqlCommand='update p Set '+@Fld_Nm+'='+rtrim(cast(@TAmount as varchar))+' from #Emp_Monthly_Payroll p '--changed by Archana K. on 17/09/13 for Bug-18264
--	Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Details eph on (p.EmployeeCode =eph.EmployeeCode and eph.'+@Fld_Nm+'YN<>0)'--Commented by Archana K. for Bug-18246
	Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join Emp_Pay_Head_Details eph on (p.EmployeeCode =eph.EmployeeCode)'--changed by Archana K. for Bug-18246
	Set @SqlCommand=rtrim(@SqlCommand)+' '+'where p.EmployeeCode='+char(39)+@EmployeeCode+char(39)     /*Ramya for Bug-14535*/

		print @SqlCommand
		Execute Sp_ExecuteSql @SqlCommand

		set @TAmount=0--Added by Archana K. on 12/09/13 for Bug-18246

               DECLARE @ParmDefinition nvarchar(500);
			   Set @SqlCommand=N'select @retvalOUT ='+@Fld_Nm+'YN from Emp_Pay_Head_Details where EmployeeCode='+char(39)+@EmployeeCode+char(39)
			   SET @ParmDefinition = N'@retvalOUT int OUTPUT';

				EXEC sp_executesql @SqlCommand, @ParmDefinition, @retvalOUT=@Fld_NmYn OUTPUT;

				   if(@Fld_NmYn=1)
					begin
						Set @DedCols=@DedCols+'+'+@Fld_Nm
--					    Set @SqlCommand='update #Emp_Monthly_Payroll Set NetPayment=(isnull('+@EarCols+',0))-(isnull('+@DedCols+',0)) where EmployeeCode='+char(39)+@EmployeeCode+char(39)    /*Ramya 06/06/13*/
--						print @SqlCommand
--						Execute Sp_ExecuteSql @SqlCommand
		
                        Set @SqlCommand='update #Emp_Monthly_Payroll Set NetPayment=NetPayment-'+cast(@Amount as varchar)+ ' where EmployeeCode='+char(39)+@EmployeeCode+char(39)    /*Ramya 06/06/13*/
						print @SqlCommand
						Execute Sp_ExecuteSql @SqlCommand
					end


	
--	Set @SqlCommand='update #Emp_Monthly_Payroll Set GrossPayment=(isnull('+@EarCols+',0))' /*Projection*/
--	print @SqlCommand
--	Execute Sp_ExecuteSql @SqlCommand	
		Fetch next From cur_Loan into @EmployeeCode,@Fld_Nm,@Amount--changed by Archana K. on 17/09/13 for Bug-18246 
	end
	Close cur_Loan
	DeAllocate cur_Loan
	/*<---Loan And Advances*/
--	insert into #Emp_Monthly_Payroll select * From Emp_Monthly_Payroll where tran_cd=@Tran_Cd and Tran_cd<>0
--
--	Set @SqlCommand='update #Emp_Monthly_Payroll Set NetPayment=(isnull('+@EarCols+',0))-(isnull('+@DedCols+',0))'
--	print @SqlCommand
--	Execute Sp_ExecuteSql @SqlCommand
--	Set @SqlCommand='update #Emp_Monthly_Payroll Set GrossPayment=(isnull('+@EarCols+',0))' /*Projection*/
--	print @SqlCommand
--	Execute Sp_ExecuteSql @SqlCommand	

	Select EmployeeCode,ProjGross=cast(0 as Decimal(17,3)),TDSPer=cast(0 as Decimal(17,3)),TDSAvg=cast(0 as Decimal(17,3)),Pay_Year,Pay_Month,MonthPen=cast(0 as int)
	into #EmpTDsCalc
	From Emp_TDS_Projection_Paid Where FinYear=@FinYear and EmployeeCode in (Select distinct EmployeeCode From #Emp_Monthly_Payroll) and Pay_Year=@Pay_Year and Pay_Month=@Pay_Month and Fld_Nm='GrossPayment'
	if (@Tran_Cd=0)/*AddMode*/
	begin
		update a set a.ProjGross=b.AmountPaid From #EmpTDsCalc a inner join Emp_TDS_Projection_Paid b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_Month and b.FinYear=@FinYear and b.Fld_Nm='GrossPayment')				
	end
	else /*EditMode*/
	begin
		update a set a.ProjGross=b.GrossPayment From #EmpTDsCalc a inner join #Emp_Monthly_Payroll b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_Month)
	end
	
	update a set a.TDSPer=b.AmountPaid From #EmpTDsCalc a inner join Emp_TDS_Projection_Paid b on (a.EmployeeCode=b.EmployeeCode and b.FinYear=@FinYear and b.Fld_Nm='TDSPer')
	update a set a.TDSAmt=b.AmountPaid From #Emp_Monthly_Payroll a inner join Emp_TDS_Projection_Paid b on (a.EmployeeCode=b.EmployeeCode and b.FinYear=@FinYear and b.Fld_Nm='TDSAvg')
	update a set a.MonthPen=b.AmountPaid From #EmpTDsCalc a inner join Emp_TDS_Projection_Paid b on (a.EmployeeCode=b.EmployeeCode and b.FinYear=@FinYear and b.Fld_Nm='MonthPen')

    set @SqlCommand ='Update #Emp_Monthly_Payroll set NetPayment=isnull(NetPayment,0)'    /*Ramya 09/03/2013*/
	Declare cur_pay_head cursor for Select Fld_Nm From Emp_Pay_Head_Master Where (isDeActive=1) Order By Fld_Nm
	open cur_pay_head
	fetch Next From cur_pay_head into @Fld_Nm
	while(@@Fetch_Status=0)
	begin
		Set @SqlCommand=@SqlCommand+','+@Fld_Nm+'=isnull('+@Fld_Nm+',0)'
		fetch Next From cur_pay_head into @Fld_Nm
	end
	close cur_pay_head
	DeAllocate cur_pay_head	
	Print @SqlCommand
	Execute Sp_ExecuteSql @SqlCommand                 /*Ramya 09/03/2013*/

	-- ****** Added by Sachin N. S. on 15/02/2014 for Bug-21542 -- Start
--	select * from #Emp_Monthly_Payroll
    Set @SqlCommand='update #Emp_Monthly_Payroll Set NetPayment=(isnull('+@EarCols+',0))-(isnull('+@DedCols+',0))'
	print @SqlCommand
	Execute Sp_ExecuteSql @SqlCommand
	-- ****** Added by Sachin N. S. on 15/02/2014 for Bug-21542 -- End
	
	Set @SqlCommand='Select p.*,isnull(datename(month,dateadd(month, p.Pay_Month - 1, 0)),'''') as cMonth,e.EmployeeName,e.Department,e.Grade,e.Category,E.Loc_Code,Loc_Desc=isnull(Loc_Desc,''''),TDSPer=isNull(tc.TDSPer,0),ProjGross=isNull(tc.ProjGross,0),MonthPen=isnull(tc.MonthPen,0),OrgTDS=TDSAmt' 
	if isnull(@TmpTableName,'')<>''
	begin
		Set @SqlCommand=rtrim(@SqlCommand)+' '+' into '+@TmpTableName
	end
	Set @SqlCommand=rtrim(@SqlCommand)+' '+' From  #Emp_Monthly_Payroll p'
	Set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join #EmpTDsCalc tc on (p.EmployeeCode=tc.EmployeeCode)'
	Set @SqlCommand=rtrim(@SqlCommand)+' '+'inner join EmployeeMast e on (e.EmployeeCode =p.EmployeeCode)'
	Set @SqlCommand=rtrim(@SqlCommand)+' '+'Left join Loc_master Lc on (e.Loc_Code=Lc.Loc_Code)' 
	--set @SqlCommand=rtrim(@SqlCommand)+' '+' where p.Pay_Year='+cast(@Pay_Year as varchar)
	--set @SqlCommand=rtrim(@SqlCommand)+' '+' and p.Pay_Month='+cast(@Pay_Month as varchar)
	
	if isnull(@FiltPara,'')<>''
	Begin
		set @SqlCommand=rtrim(@SqlCommand)+' '+@FiltPara
	end
	Set @SqlCommand=rtrim(@SqlCommand)+' '+'Order by p.EmployeeCode' 
	--set @SqlCommand=rtrim(@SqlCommand)+' '+' and p.PayGenerated=1'
	Print @SqlCommand

	Execute Sp_ExecuteSql @SqlCommand
		
end