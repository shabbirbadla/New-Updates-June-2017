If Exists (Select [Name] from sysobjects where xType='P' and Id=Object_Id(N'Usp_Ent_Emp_Processing_Month_Creation'))
Begin
	Drop Procedure Usp_Ent_Emp_Processing_Month_Creation
End
Go

-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 30/05/2012
-- Description:	This Stored procedure is useful to generate processing Month in Entry Module.
-- Modification Date/By/Reason: Sachin N. S. on 30/05/2014 for Bug-23004
-- Modification Date/By/Reason:
-- Remark: 
-- =============================================
--[Usp_Ent_Emp_Processing_Month_Creation]'2012',8,'HYD'
CREATE Procedure [dbo].[Usp_Ent_Emp_Processing_Month_Creation]
@Year varchar(30),@Month int,@Loc_Code varchar(10)
as
Begin
	Set Dateformat dmy 
	Declare @MnthDays int,@i int,@j int,@WOWeek varchar(30),@DateNm varchar(30)
	Declare @sDate smallDatetime,@eDate smallDatetime,@tDate smallDatetime,@DayName varchar(30),@WHDayNum Decimal(10,3),@WHDay varchar(200),@WHDaym varchar(30),@FH_Day varchar(10)
	Declare @mLoc_Code varchar(10),@Dept varchar(60),@Cate varchar(60),@mDept varchar(60),@mCate varchar(60),@HoliDay int
	Declare @SqlCommand nvarchar(4000)
	set @WHDay=''
	set @MnthDays=0
	--execute [Usp_Ent_Emp_Processing_Month_Creation]'2012-2013',12,'HYD'
	--print 'r1'
	Declare @tYear varchar(60) 
	Select @tYear=rtrim(cast(Year(sDate) as varchar)) From Emp_Payroll_Year_Master where pay_year=@Year	
	--Select @tYear
	--Select @tYear=rtrim(cast(Year(sDate) as varchar)) From Emp_Payroll_Year_Master where pay_year=char(39)+@Year+char(39)
--set @sqlcommand='Declare @tYear varchar(60) Select @tYear=rtrim(cast(Year(sDate) as varchar)) From Emp_Payroll_Year_Master where pay_year='+char(39)+@Year+char(39)
	Select @MnthDays =case 
		when @Month in (1,3,5,7,8,10,12) then 31
		when  @Month in (4,6,9,11) then 30
		when isdate('29/02/'+rtrim(Cast(@tYear as varchar)))=1 then 29 else 28 end
	--print cast(@MnthDays as varchar)+'/'+cast(@Month as varchar)+'/'+cast(@Year as varchar)
	--print 'r2'
	
	--set @sDate=cast('01/'+cast(@Month as varchar)+'/'+cast(@Year as varchar) as smalldatetime)	
	Select @tYear=rtrim(cast(Year(case when @Month in (1,2,3) then Edate else Sdate end ) as varchar)) From Emp_Payroll_Year_Master where pay_year=char(39)+@Year+char(39)
	--print @tYear
	set @sDate=cast('01/'+cast(@Month as varchar)+'/'+@tYear as smalldatetime)
	--print @sDate
	--print 'r3'
	Select @tYear=rtrim(cast(Year(eDate) as varchar)) From Emp_Payroll_Year_Master where pay_year=char(39)+@Year+char(39)
	--Set @eDate=cast(cast(@MnthDays as varchar)+'/'+cast(@Month as varchar)+'/'+cast(@Year as varchar) as smalldatetime)
	Set @eDate=cast(cast(@MnthDays as varchar)+'/'+cast(@Month as varchar)+'/'+cast(@tYear as varchar) as smalldatetime)
	--print @eDate
	--print 'r4'
	
	insert into Emp_Leave_Maintenance (pay_year,pay_month,EmployeeCode) select @Year,@Month,EmployeeCode from EmployeeMast where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' and cast(@Year as varchar)+Cast(@Month as Varchar)+EmployeeCode not in (Select cast(isnull(pay_year,0) as varchar)+cast(isnull(pay_month,0) as varchar)+EmployeeCode from Emp_Leave_Maintenance) 
-- Below line Added By for Bug-5329 on 10-08-2012
	insert into Emp_Daily_Muster (Pay_Year,Pay_Month,EmployeeCode) select @Year,@Month,EmployeeCode from EmployeeMast where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' and cast(@Year as varchar)+Cast(@Month as Varchar)+EmployeeCode not in (Select cast(isnull(Pay_Year,0) as varchar)+cast(isnull(Pay_Month,0) as varchar)+EmployeeCode from Emp_Daily_Muster) 	
	insert into Emp_Monthly_Muster   (pay_year,pay_month,EmployeeCode,MonthDays,SalPaidDays) select @Year,@Month,EmployeeCode,@MnthDays,@MnthDays from EmployeeMast where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' and cast(@Year as varchar)+Cast(@Month as Varchar)+EmployeeCode not in (Select cast(isnull(pay_year,0) as varchar)+cast(isnull(pay_month,0) as varchar)+EmployeeCode from Emp_Monthly_Muster) 
	--insert into Emp_Monthly_Payroll  (pay_year,pay_month,EmployeeCode,MonthDays,SalPaidDays) select @Year,@Month,EmployeeCode,@MnthDays,@MnthDays from EmployeeMast where ActiveStatus=1 and isnull(EmployeeCode,'')<>'' and cast(@Year as varchar)+Cast(@Month as Varchar)+EmployeeCode not in (Select cast(isnull(pay_year,0) as varchar)+cast(isnull(pay_month,0) as varchar)+EmployeeCode from Emp_Monthly_Payroll) 
	select * from Emp_Monthly_Muster where pay_month=5 order by EmployeeCode
	/*--->Weekly Holiday*/
	Select distinct EmployeeCode,FirstWeekWO,SecondWeekWO,ThirdWeekWO,FourthWeekWO,FifthWeekWO,SixthWeekWO into #EmpWo From EmployeeMast
	
	Select * into #LeaveMaintenance From Emp_Leave_Maintenance where pay_month=case when cast(@Month as int)=1 then 12 else  cast(@Month as int)-1 end
	
	Set @DateNm=DATENAME(dw ,@sDate)
	Update  #EmpWo Set FirstWeekWO=case
										when @DateNm='Monday'    then 'W'+Substring(FirstWeekWO,2,6)
										when @DateNm='Tuesday'   then 'WW'+Substring(FirstWeekWO,3,5)
										when @DateNm='Wednesday' then 'WWW'+Substring(FirstWeekWO,4,4)
										when @DateNm='Thursday'  then 'WWWW'+Substring(FirstWeekWO,5,3)
										when @DateNm='Friday'    then 'WWWWW'+Substring(FirstWeekWO,6,2)
										when @DateNm='Saturday'  then 'WWWWWW'+Substring(FirstWeekWO,7,1)
										else FirstWeekWO end
	
	Set @DateNm=DATENAME(dw ,@eDate)
	select @WOWeek=
		case 
			when cast(datename(week,@eDate) as int)- cast( datename(week,dateadd(dd,1-day(@eDate),@eDate)) as int)+1=4 then 'FourthWeekWO'
			when cast(datename(week,@eDate) as int)- cast( datename(week,dateadd(dd,1-day(@eDate),@eDate)) as int)+1=5 then 'FifthWeekWO'
			else 'SixthWeekWO'
		end
--	select WOWeek=
--		case 
--			when cast(datename(week,@eDate) as int)- cast( datename(week,dateadd(dd,1-day(@eDate),@eDate)) as int)+1=4 then 'FourthWeekWO'
--			when cast(datename(week,@eDate) as int)- cast( datename(week,dateadd(dd,1-day(@eDate),@eDate)) as int)+1=5 then 'FifthWeekWO'
--			else 'SixthWeekWO'
--		end
	if (@WOWeek='FourthWeekWO')
	begin
		update #EmpWo set FifthWeekWO='WWWWWWW',SixthWeekWO='WWWWWWW'
	end
	if (@WOWeek='FifthWeekWO')
	begin
		update #EmpWo set SixthWeekWO='WWWWWWW'
	end


	Set @SqlCommand='Update  #EmpWo Set '+@WOWeek+'=case '
										+'when '+char(39)+@DateNm+char(39)+'=''Sunday''    then Substring('+@WOWeek+',1,1)+''WWWWWW'''
										+'when '+char(39)+@DateNm+char(39)+'=''Monday''    then Substring('+@WOWeek+',1,2)+''WWWWW'''
										+'when '+char(39)+@DateNm+char(39)+'=''Tuesday''   then Substring('+@WOWeek+',1,3)+''WWWW'''
										+'when '+char(39)+@DateNm+char(39)+'=''Wednesday'' then Substring('+@WOWeek+',1,4)+''WWW'''
										+'when '+char(39)+@DateNm+char(39)+'=''Thursday''  then Substring('+@WOWeek+',1,5)+''WW'''
										+'when '+char(39)+@DateNm+char(39)+'=''Friday''    then Substring('+@WOWeek+',1,6)+''W'''										
										+'else '+@WOWeek+' end'
--	print 'w '+@SqlCommand
	Execute Sp_ExecuteSQL @SqlCommand

	
--	print @DateNm	
	set @i=1
	while (@i<=6)
	begin
		Select @WOWeek= case
							when @i=1 then 'FirstWeekWO' 
							when @i=2 then 'SecondWeekWO' 
							when @i=3 then 'ThirdWeekWO' 
							when @i=4 then 'FourthWeekWO' 
                            when @i=5 then 'FifthWeekWO' 
							Else 'SixthWeekWO' 
						end

		if(@i=1)
		begin
			Update a set a.WO=0 From Emp_Monthly_Muster a where pay_year=cast(@Year as Varchar) and pay_month=cast(@Month as Varchar)
		end
		--Select @i as i,* From Emp_Monthly_Muster where EmployeeCode='A00001' and pay_month=5
		set @j=1
		while(@j<=7)
		begin
			--Select @i as i,@j as j,@WOWeek as week,* From Emp_Monthly_Muster where EmployeeCode='A00001' and pay_month=5
			Set @SqlCommand='update a set a.WO=a.WO+case 
						when  substring('+@WOWeek+','+cast(@j as varchar)+',1)=''F'' then 1 
						when  substring('+@WOWeek+','+cast(@j as varchar)+',1)=''H'' then 0.5
						else 0 end 
		From Emp_Monthly_Muster a inner join #EmpWo b on (a.EmployeeCode=b.EmployeeCode and a.pay_year='+char(39)+Cast(@Year as Varchar)+char(39)+' and a.pay_month='+Cast(@Month as Varchar)+')'
--			print @SqlCommand
			execute Sp_ExecuteSQl @SqlCommand
	
			set @j=@j+1	
		end
		set @i=@i+1	
	end


	--Select 'a',* From #EmpWo
	


	Declare @LvAutocr Decimal(12,3),@att_code varchar(3)
	declare cur_lvUpdate cursor for select att_code,LvAutocr from Emp_Attendance_Setting where isleave=1
	open cur_lvUpdate
	fetch next from cur_lvUpdate into @att_code,@LvAutocr
	while (@@fetch_status=0)
	begin

		Set @SqlCommand=''
		Set @SqlCommand=rtrim(@SqlCommand)+'a.'+@att_code+'_OpBal=b.'+@att_code+'_Balance,a.'+@att_code+'_Credit='+rtrim(cast(@LvAutocr as varchar))
		Set @SqlCommand=rtrim(@SqlCommand)+',a.'+@att_code+'_Balance=b.'+@att_code+'_Balance+'+rtrim(cast(@LvAutocr as varchar))
		Set @SqlCommand='update a set '+@SqlCommand
		Set @SqlCommand=rtrim(@SqlCommand)+' From Emp_Leave_Maintenance a inner join #LeaveMaintenance b on (a.EmployeeCode=b.EmployeeCode) where a.pay_year='+char(39)+@Year+char(39)+ ' and a.pay_month='+cast(@Month as varchar)	
		--print 'ar1-'+ @SqlCommand	
		execute Sp_ExecuteSql @SqlCommand
		fetch next from cur_lvUpdate into @att_code,@LvAutocr
	end
	close cur_lvUpdate
	deallocate cur_lvUpdate


	Declare cur_WhDay cursor for Select hDay=Sum(Days),Loc_Code,Dept,Cate from Emp_Holiday_Master where ( month(sDate)=@Month or month(eDate)=@Month ) and pay_year=@Year group by Loc_Code,Dept,Cate
	open cur_WhDay
	Fetch next from cur_WhDay into @HoliDay,@Loc_Code,@Dept,@Cate
	while(@@Fetch_Status=0)
	Begin
		set @SqlCommand='Update a set a.HD='+Cast(@HoliDay as varchar)+'  from Emp_Monthly_Muster a inner join EmployeeMast e on (a.EmployeeCode=e.EmployeeCode) Where 1=1'
		if (isnull(@Loc_Code,'')<>'')
		begin
			set @SqlCommand=rtrim(@SqlCommand)+' '+' and e.Loc_Code='+char(39)+@Loc_Code+char(39)
		end
		if (isnull(@Dept,'')<>'')
		begin
			set @SqlCommand=rtrim(@SqlCommand)+' '	+' and e.Department='+char(39)+@Dept+char(39)
		end
		if (isnull(@cate,'')<>'')
		begin
			set @SqlCommand=rtrim(@SqlCommand)+' '	+' and e.Category='+char(39)+@cate+char(39)
		end

		Execute Sp_ExecuteSql @SqlCommand

		Fetch next from cur_WhDay into @HoliDay,@Loc_Code,@Dept,@Cate
	end
	close cur_WhDay
	DeAllocate cur_WhDay
	
	execute Update_table_column_default_value 'Emp_Leave_Maintenance',1
	execute Update_table_column_default_value 'Emp_Monthly_Muster',1
--	execute Update_table_column_default_value 'Emp_Monthly_payroll',1		-- Commented by Sachin N. S. on 30/05/2014 for Bug-23004
--print @MnthDays
--print 'hi'
	execute usp_Ent_Emp_UpdateMonthly_Muster @Year ,@Month ,@Loc_Code,@MnthDays
	
-- Added by Pankaj B. on 28-11-2014 for Employee Leave Request / Approval start Bug-24248
execute usp_Ent_Emp_Update_Leave_request  @Year,@Month 
-- Added by Pankaj B. on 28-11-2014 for Employee Leave Request / Approval End Bug-24248
end

update Emp_Monthly_Muster set [MonthDays] =isnull([MonthDays],0)