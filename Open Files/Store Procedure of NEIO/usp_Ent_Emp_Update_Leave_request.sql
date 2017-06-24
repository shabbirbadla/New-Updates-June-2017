If Exists (Select [Name] from sysobjects where xType='P' and Id=Object_Id(N'usp_Ent_Emp_Update_Leave_request'))
Begin
	Drop Procedure usp_Ent_Emp_Update_Leave_request
End
Go

create procedure usp_Ent_Emp_Update_Leave_request
@Year varchar(30),@Month int
as
begin
	declare  @empcode varchar(10),@leavetype varchar(5),@reqleave decimal(5,2),@str nvarchar(500),@sdate smalldatetime,@edate smalldatetime,@lastday int,@firstday int,@total decimal(5,2),@date1 smalldatetime,@constype varchar(10),@cnt int,@day1 int,@month1 int,@year1 int
	declare @BalLeave decimal(6,2),@BalLeave1 decimal(6,2),@sqlstr nvarchar(500),@cnt1 int
	--set @Year='2014'
	--set @Month=11

	declare #temp1 cursor for select empcode,leavetype,req_leave,sdate,edate  from Emp_Leave_Request_ITEM where year(sdate)=@year and month(sdate)=@month and month(edate)=@month and status='Approved' 
	open #temp1
	fetch next from #temp1 into @empcode,@leavetype,@reqleave,@sdate,@edate
	while @@fetch_status=0
		begin
		--set @str='select a.'+@leavetype+' from  Emp_Monthly_Muster a where a.pay_month='+cast(@month as varchar)+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+''''
		--print @str
			execute('update a set a.'+@leavetype+'_Availed=a.'+@leavetype+'_Availed+'+@reqleave+',a.'+@leavetype+'_Balance=a.'+@leavetype+'_Balance-'+@reqleave+' from  Emp_Leave_Maintenance a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
	
	        set @sqlstr='select @BalLeave1='+@LeaveType+'_Balance from Emp_Leave_Maintenance where EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+cast(@month as varchar)+' and pay_year='+@year+''         
       		EXECUTE SP_EXECUTESQL @sqlstr,N'@BalLeave1 decimal(6,2) OUTPUT',@BalLeave1=@BalLeave OUTPUT
       	
			if @BalLeave<0 
			begin
				set @BalLeave1=abs(@BalLeave)
				execute('update Emp_Leave_Maintenance set
					 '+@LeaveType+'_Availed='+@LeaveType+'_Availed-'+@BalLeave1+','+@LeaveType+'_Balance=0
					 WHERE EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+@month+' and pay_year='+@year+'')
	                 
				execute('update Emp_Monthly_Muster set
					 '+@LeaveType+'='+@LeaveType+'+('+@ReqLeave+'-'+@BalLeave1+'),LOP=LOP+'+@BalLeave1+',PR=PR-'+@ReqLeave+'
					 WHERE EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+@month+' and pay_year='+@year+'')
			end
			else
			Begin
				execute('update Emp_Monthly_Muster set
					 '+@LeaveType+'='+@LeaveType+'+'+@ReqLeave+',PR=PR-'+@ReqLeave+'
					 WHERE EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+@month+' and pay_year='+@year+'')			
			End
         -------------------------- Update Daily Muster---------------------------- Start
			if (datediff(dd,@sdate,@edate)=0)
			begin 
				set @day1=day(@sdate)
				if (@BalLeave>=0)
				begin
					if (@reqleave='0.50')
					begin
						execute('update a set a.Day'+@day1+'=''H''+'''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
					end
					else
					begin
						execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
					end
				end
				else
				begin
						execute('update a set a.Day'+@day1+'=''LOP'' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
				end
			end
			else
			begin
				set @cnt=1
				set @date1= @sdate
				set @day1=day(@date1)
				if @BalLeave>=0
				begin
					while (@cnt=1)
					begin
						execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
						set @date1=dateadd(dd,1,@date1)
						set @day1=day(@date1)

						if (@date1=dateadd(dd,1,@edate))
						set @cnt=0
					end
				end
				else
				begin	
					set @cnt1=1
					set @cnt=1
					set @date1= @sdate
					set @day1=day(@date1)
					while (@cnt=1)
					begin
						if (@cnt1<=(@ReqLeave-@BalLeave1))
						execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
						else
						execute('update a set a.Day'+@day1+'=''LOP'' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
						set @date1=dateadd(dd,1,@date1)
						set @day1=day(@date1)

						if (@date1=dateadd(dd,1,@edate))
						set @cnt=0
						set @cnt1=@cnt1+1
					end
				end
			end	
         
         -------------------------- Update Daily Muster---------------------------- End         
       	
			--execute('update a set a.'+@leavetype+'=a.'+@leavetype+'+'+@reqleave+',PR=PR-'+@reqleave+' from  Emp_Monthly_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
			fetch next from #temp1 into @empcode,@leavetype,@reqleave,@sdate,@edate
		end
	close #temp1
	deallocate #temp1

	declare #temp2 cursor for select empcode,leavetype,sdate,edate  from Emp_Leave_Request_ITEM where year(sdate)=@year and month(sdate)=@month and month(edate)<>@month and status='Approved'
	open #temp2
	fetch next from #temp2 into @empcode,@leavetype,@sdate,@edate
	while @@fetch_status=0
		begin
			set @firstday=day(@sdate)
			set @lastday=day(DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @sdate) + 1, 0)))
			set @total=(@lastday-@firstday)+1
			execute('update a set a.'+@leavetype+'_Availed=a.'+@leavetype+'_Availed+'+@total+',a.'+@leavetype+'_Balance=a.'+@leavetype+'_Balance-'+@total+' from  Emp_Leave_Maintenance a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')			
			
			set @sqlstr='select @BalLeave1='+@LeaveType+'_Balance from Emp_Leave_Maintenance where EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+cast(@month as varchar)+' and pay_year='''+cast(@year as varchar)+''''         
       		EXECUTE SP_EXECUTESQL @sqlstr,N'@BalLeave1 decimal(6,2) OUTPUT',@BalLeave1=@BalLeave OUTPUT			
			
			if @BalLeave<0 
			begin
				set @BalLeave1=abs(@BalLeave)
				execute('update Emp_Leave_Maintenance set
					 '+@LeaveType+'_Availed='+@LeaveType+'_Availed-'+@BalLeave1+','+@LeaveType+'_Balance=0
					 WHERE EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+@month+' and pay_year='+@year+'')

				execute('update Emp_Monthly_Muster set
					 '+@LeaveType+'='+@LeaveType+'+('+@total+'-'+@BalLeave1+'),LOP=LOP+'+@BalLeave1+',PR=PR-'+@total+'
					 WHERE EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+@month+' and pay_year='+@year+'')
			end
			else
			Begin
				execute('update Emp_Monthly_Muster set
					 '+@LeaveType+'='+@LeaveType+'+'+@total+',PR=PR-'+@total+'
					 WHERE EMPLOYEECODE='''+@EmpCode+''' AND PAY_MONTH='+@month+' and pay_year='+@year+'')			
			End		
			--execute('update a set a.'+@leavetype+'=a.'+@leavetype+'+'+@total+',PR=PR-'+@total+' from  Emp_Monthly_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
                  -------------------------- Update Daily Muster---------------------------- Start
			if (datediff(dd,@sdate,DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @sdate) + 1, 0)))=0)
			begin 
				set @day1=day(@sdate)
				if (@BalLeave>=0)
				begin
					if (@constype='0.50')
					begin
						execute('update a set a.Day'+@day1+'=''H''+'''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
					end
					else
					begin
						execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
					end
				end
				else
				begin
						execute('update a set a.Day'+@day1+'=''LOP'' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
				end
			end
			else
			begin
				set @cnt=1
				set @date1= @sdate
				set @day1=day(@date1)
				print @BalLeave
				if @BalLeave>=0
				begin
					while (@cnt=1)
					begin
					print @date1
						execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
						set @date1=dateadd(dd,1,@date1)
						set @day1=day(@date1)
						print cast(@day1 as varchar)
						print @date1
						print @edate
						print cast(dateadd(dd,1,DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @sdate) + 1, 0))) as varchar(10))
						if (@date1=dateadd(dd,1,DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @sdate) + 1, 0))))
						set @cnt=0
					end
				end
				else
				begin	
					set @cnt1=1
					set @cnt=1
					set @date1= @sdate
					set @day1=day(@date1)
					while (@cnt=1)
					begin
						if (@cnt1<=((day((DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @sdate) + 1, 0))-day(@sdate)))+1)-@BalLeave1))
						execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
						else
						execute('update a set a.Day'+@day1+'=''LOP'' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
						set @date1=dateadd(dd,1,@date1)
						set @day1=day(@date1)
						print cast(@day1 as varchar)
						print @date1
						print @edate
						if (@date1=dateadd(dd,1,DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @sdate) + 1, 0))))
						set @cnt=0
						set @cnt1=@cnt1+1
					end
				end
			end	
         
         -------------------------- Update Daily Muster---------------------------- End  
			
			fetch next from #temp2 into @empcode,@leavetype,@sdate,@edate
		end
	close #temp2
	deallocate #temp2
	
	--declare #temp3 cursor for select empcode,leavetype,constype,sdate,edate  from Emp_Leave_Request_ITEM where year(sdate)=@year and month(sdate)=@month  and status='Approved'
	--open #temp3
	--fetch next from #temp3 into @empcode,@leavetype,@constype,@sdate,@edate
	--while @@fetch_status=0
	--	begin
			
	--		if (datediff(dd,@sdate,@edate)=0)
	--		begin 
	--			set @day1=day(@sdate)
	--			if (@constype='Half Day')
	--			begin
	--				execute('update a set a.Day'+@day1+'=''H''+'''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
	--			end
	--			else
	--			begin
	--				execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
	--			end
	--		end
	--		else
	--		begin
	--			set @cnt=1
	--			set @date1= @sdate
	--			set @day1=day(@date1)
	--			if month(@edate)<>@month
	--			begin
	--			set @edate=DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @edate) + 1, 0))
	--			end
	--			while (@cnt=1)
	--			begin
	--				execute('update a set a.Day'+@day1+'='''+@leavetype+''' from  Emp_Daily_Muster a where a.pay_month='+@month+' and a.pay_year='+@year+' and a.employeecode='''+@empcode+'''')
	--				set @date1=dateadd(dd,1,@date1)
	--				set @day1=day(@date1)
	--				print cast(@day1 as varchar)
	--				print @date1
	--				print @edate
	--				if (@date1=dateadd(dd,1,@edate))
	--				set @cnt=0
	--			end
	--		end	
	--		fetch next from #temp3 into @empcode,@leavetype,@constype,@sdate,@edate
	--	end
	--close #temp3
	--deallocate #temp3
	
end