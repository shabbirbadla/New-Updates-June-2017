IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Ent_Update_Monthly_Muster_As_Per_Daily_Muster]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Ent_Update_Monthly_Muster_As_Per_Daily_Muster]
end
go
-- =============================================
-- Author:		Amrendra Kumar Singh
-- Create date: 26/07/2012
-- Description:
-- Modification Date/By/Reason:
-- Remark:
-- =============================================

create procedure Usp_Ent_Update_Monthly_Muster_As_Per_Daily_Muster
@vPay_Year varchar(12),@vPay_month int
as 
begin

	--declare @vMonthDay int,@vPay_Year varchar(10),@vPay_Month int,@lCounter int,@DayColList varchar(1000)
	declare @vMonthDay int,@lCounter int,@DayColList varchar(1000),@isHalfDay bit,@vHAttCode varchar(3)
	declare @vEmployeeCode varchar(50),@sqlCmd nvarchar(4000),@vAttCode varchar(3),@col_lst as varchar(1000)
	--set @vPay_Year='2012'
	--set @vPay_month=6
	set @sqlCmd=''
	select top 1 @vMonthDay=monthdays from emp_monthly_muster where Pay_Year=@vPay_Year and Pay_Month=@vPay_Month
	print @vMonthDay

	set @lCounter=1
	set @DayColList=''
	while(@lCounter<=@vMonthDay)
	begin
		set @DayColList=+@DayColList+','+'Day'+rtrim(cast(@lCounter as varchar(2)))		
		set @lCounter=@lCounter+1
	end 

	set @DayColList=substring(@DayColList,2,len(@DayColList)-1)
	print @DayColList
	select * into #Monthly_Muster from emp_monthly_muster where 1=2

	select * into #Daily_Muster from emp_Daily_muster where Pay_year=@vPay_Year and Pay_Month=@vPay_Month and approve=1 order by EmployeeCode
	---select * from #Daily_Muster
	while exists(select top 1 EmployeeCode from #Daily_Muster)
	begin
		select top 1 @vEmployeeCode	=employeeCode from #Daily_Muster
		insert #Monthly_Muster (EmployeeCode,Pay_Year,Pay_Month) values (@vEmployeeCode,@vPay_Year,@vPay_month)
			set @lCounter=1
			set @DayColList=''
			while(@lCounter<=@vMonthDay)
			begin
				set @vAttCode=''
				set @DayColList='Day'+rtrim(cast(@lCounter as varchar(2)))
	--			print @DayColList
				set @sqlCmd='select @retvalOUT='+ @DayColList+' from #Daily_Muster where employeeCode='+char(39)+@vEmployeeCode+char(39)
				execute sp_executesql @sqlCmd,N'@retvalOUT varchar(3) OUTPUT',@retvalOUT=@vAttCode OUTPUT
				---XXXXXXXXXXXXXXX
				set @isHalfDay=0
				if exists(select Att_code from emp_attendance_setting where h_Att_code= @vAttCode)
				begin
					print 'Half day found'+ @vAttCode
					set @vHAttCode=@vAttCode
					select @vAttCode=Att_code from emp_attendance_setting where h_Att_code= @vHAttCode
					set @isHalfDay=1
				end
				---XXXXXXXXXXXXXXX
	--			print @vAttCode
	--			print 'Amrendra'
				if @isHalfDay=1
				begin
					set @sqlCmd='update	#Monthly_Muster set '+@vAttCode+'=isnull('+@vAttCode+',0)+0.5 where employeeCode='+char(39)+@vEmployeeCode+char(39)				
				end
				else
				begin
					set @sqlCmd='update	#Monthly_Muster set '+@vAttCode+'=isnull('+@vAttCode+',0)+1 where employeeCode='+char(39)+@vEmployeeCode+char(39)
				end
				set @lCounter=@lCounter+1
				execute sp_executesql @sqlCmd
			end
		delete #Daily_Muster where EmployeeCode=@vEmployeeCode
		print '123'
	end

	;WITH ABC (FId, att_code) AS
		(
			SELECT 1, CAST('' AS VARCHAR(8000)) 
			UNION ALL
			SELECT B.FId + 1, +B.att_code +'a.'+  A.att_code + '=isnull(b.'+A.att_code+',0), ' 
			FROM (SELECT Row_Number() OVER (ORDER BY Id) AS RN, att_code FROM Emp_attendance_setting  where att_code not in ('WO','NA','MonthDays','SalPaidDays')) A 
			INNER JOIN ABC B ON A.RN = B.FId 
		)
		SELECT TOP 1 @col_lst=att_code FROM ABC ORDER BY FId DESC 
		select @col_lst=case when len(@col_lst)>0 then substring(@col_lst,1,len(rtrim(@col_lst))-1) else '' end 
		print @col_lst

	set @sqlCmd='update a set '+@col_lst+' from emp_monthly_muster a inner join 
		#monthly_muster b on a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_Month'
	print @sqlCmd
	execute sp_executesql @sqlCmd

	--execute usp_Ent_Emp_Update_Leave_Maintenance @vPay_Year ,@vPay_month,''

	--select * from emp_monthly_muster where Pay_Year=@vPay_Year and Pay_Month=@vPay_Month  order by EmployeeCode
	--select * from #Monthly_Muster
	--select * from #Daily_Muster
	drop table #Daily_Muster
	drop table #Monthly_Muster

	--select * from emp_daily_muster

	--select * from emp_attendance_setting order by id
	--select * from emp_monthly_muster where Pay_Year='2012' and Pay_Month=6
--update emp_monthly_muster set WO=5 where Pay_Year='2012' and Pay_Month=6
end 

--Usp_Ent_Update_Monthly_Muster_As_Per_Daily_Muster '2012',6