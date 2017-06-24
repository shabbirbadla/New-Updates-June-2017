set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author: Rupesh
-- Create date: 08/09/2012
-- Description:This Triger is Used to update Emp_Loan_Advance_Details
-- Modify date:
-- Remark:
/*
Modification By : Archana
Modification On : 05-09-2013
Bug Details		: Bug-18246 (Issue of Loan & Advance details in Monthly Payroll Transaction)
Search for		: Bug-18246
Modified By/On/For : Sachin N. S. on 28/02/2014 for Bug-21956
Modified By/On/For : Sachin N. S. on 30/05/2014 for Bug-23004
*/
-- =============================================
ALTER Trigger [UTrig_Emp_Monthly_Payroll_Loan]  on [dbo].[Emp_Monthly_Payroll]
AFTER  INSERT, UPDATE
As
begin
	Declare @EmployeeCode varchar(15),@Pay_Year Varchar(15),@Pay_Month int,@Fld_Nm varchar(30) ,@Amount Decimal(17,2),@ParmDefinition nvarchar(500)
	Declare @tSqlCommand nvarchar(4000),@inst_amt Decimal(17,2),@Tran_Cd int,@TAmount Decimal(17,2)
	select @EmployeeCode=EmployeeCode, @Pay_Year=Pay_Year, @Pay_Month=Pay_Month From inserted
	Select * into #TrigInsert From inserted

--	Declare Cur_Trig_Loan cursor for Select Fld_Nm from Emp_Loan_Advance a inner join Emp_Loan_Advance_Details b on (a.Tran_Cd=b.Tran_cd) where Pay_Year=@Pay_Year and Pay_Month=@Pay_Month--commented by Archana on 18/09/13 for Bug-18246
--	Declare Cur_Trig_Loan cursor for Select Fld_Nm,inst_amt,a.Tran_Cd from Emp_Loan_Advance a 
	Declare Cur_Trig_Loan cursor for Select Fld_Nm,inst_amt,a.Tran_Cd,b.EmployeeCode,b.Pay_year,b.Pay_month from Emp_Loan_Advance a		-- Changed by Sachin N. S. on 16/06/2014 for Bug-23004
		inner join Emp_Loan_Advance_Details b on (a.Tran_Cd=b.Tran_cd) 
		inner join #TrigInsert c on (a.EmployeeCode=c.EmployeeCode and b.pay_year=c.Pay_year and b.Pay_month=c.Pay_month)		-- Changed by Sachin N. S. on 16/06/2014 for Bug-23004
		order by a.Employeecode,a.tran_cd,b.Pay_Year,b.Pay_month			-- Added by Sachin N. S. on 28/05/2014 for Bug-23004


--	inner join #TrigInsert c on (a.EmployeeCode=c.EmployeeCode)		-- Added by Sachin N. S. on 15/10/2013 for Bug-18246		-- Changed by Sachin N. S. on 16/06/2014 for Bug-23004

--	where Pay_Year=@Pay_Year and Pay_Month=@Pay_Month--Changed by Archana on 18/09/13 for Bug-18246
--	where b.Pay_Year=@Pay_Year and b.Pay_Month=@Pay_Month				--Changed by Sachin N. S. on 28/02/2014 for Bug-21956			-- Commented by Sachin N. S. on 28/05/2014 for Bug-23004

	Open Cur_Trig_Loan
--	Fetch next From Cur_Trig_Loan into @Fld_Nm--Commented by Archana K. on 18/09/13 for Bug-18246
--	Fetch next From Cur_Trig_Loan into @Fld_Nm,@inst_amt,@Tran_Cd--Changed by Archana K. on 18/09/13 for Bug-18246
	Fetch next From Cur_Trig_Loan into @Fld_Nm,@inst_amt,@Tran_Cd,@Employeecode,@Pay_year,@Pay_month	--Changed by Sachin N. S. on 16/06/2014 for Bug-23004

--- Commented by Sachin N. S. on 16/06/2014 for Bug-23004 -- Start
------Added by Archana K. on 18/09/13 for Bug-18246 start
--		set @tSqlCommand=N'select @lamt='+@Fld_Nm+' from #TrigInsert'
--		SET @ParmDefinition = N'@lamt Decimal(17,2) OUTPUT';
--		execute sp_executesql @tSqlCommand,@ParmDefinition,@lamt=@Amount OUTPUT;
------Added by Archana K. on 18/09/13 for Bug-18246 end
--- Commented by Sachin N. S. on 16/06/2014 for Bug-23004 -- End

		while(@@Fetch_Status=0)
		begin

----Added by Sachin N. S. on 16/06/2014 for Bug-23004 -- Start
		set @tSqlCommand=N'select @lamt='+@Fld_Nm+' from #TrigInsert'
		SET @ParmDefinition = N'@lamt Decimal(17,2) OUTPUT';
		execute sp_executesql @tSqlCommand,@ParmDefinition,@lamt=@Amount OUTPUT;
----Added by Sachin N. S. on 16/06/2014 for Bug-23004 -- End

--Added by Archana K. on 18/09/13 for Bug-18246 start
			if(@Amount>0)
			begin
-- Changed by Sachin N. S. on 28/10/2013 for Bug-18246 -- Start
--						if(@Amount>@inst_amt)
--						begin
--							set @TAmount=@inst_amt
--						end
--						else
--						begin
--							set @TAmount=@Amount 
--						end

				set @TAmount=@Amount 

-- Changed by Sachin N. S. on 28/10/2013 for Bug-18246 -- End

	--Added by Archana K. on 18/09/13 for Bug-18246 end
	--			Set @tSqlCommand='update a Set a.Proj_RePay=0,a.Repay_Amt=b.'+@Fld_Nm+',a.Cl_Bal=a.Op_Bal+a.Inst_Amt+a.Interest-b.'+@Fld_Nm+
	--			' From Emp_Loan_Advance_Details a inner join #TrigInsert b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_month)'--Commented by Archana K. on 18/09/13 for Bug-18246

--						Set @tSqlCommand='update a Set a.Proj_RePay=0,a.Repay_Amt='+cast(@TAmount as varchar)+',a.Cl_Bal=a.Op_Bal+a.Inst_Amt+a.Interest-'+cast(@TAmount as varchar)+
--						' From Emp_Loan_Advance_Details a inner join #TrigInsert b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_month) where a.Tran_Cd='+cast(@Tran_Cd as varchar)+''--Changed by Archana K. on 18/09/13 for Bug-18246

						Set @tSqlCommand='update a Set a.Proj_RePay=0,a.Repay_Amt='+cast(@TAmount as varchar)+',a.Cl_Bal=a.Op_Bal-'+cast(@TAmount as varchar)+
						' From Emp_Loan_Advance_Details a inner join #TrigInsert b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_month) where a.Tran_Cd='+cast(@Tran_Cd as varchar)+''--Changed by Archana K. on 18/09/13 for Bug-18246
						' and a.Pay_Year='+cast(@Pay_Year as varchar)+' and a.Pay_Month='+cast(@Pay_Month as varchar)		-- Added by Sachin N. S. on 28/05/2014 for Bug-23004

					Execute Sp_ExecuteSql @tSqlCommand
--					execute Usp_Ent_Emp_Update_Loan_Balance @EmployeeCode,@Pay_Year,@Pay_Month,@Fld_Nm
					execute Usp_Ent_Emp_Update_Loan_Balance @EmployeeCode,@Pay_Year,@Pay_Month,@Fld_Nm,'U'	-- Changed by Sachin N. S. on 28/05/2014 for Bug-23004
	--Added by Archana K. on 18/09/13 for Bug-18264 start
					if(@Amount>@inst_amt)
					begin
							set @Amount=@Amount-@inst_amt
					end
					else
					begin
							set @Amount=0
					end
--					Fetch next From Cur_Trig_Loan into @Fld_Nm,@inst_amt,@Tran_Cd--Changed by Archana K. on 18/09/13 for Bug-18246
					Fetch next From Cur_Trig_Loan into @Fld_Nm,@inst_amt,@Tran_Cd,@Employeecode,@Pay_year,@Pay_month	--Changed by Sachin N. S. on 16/06/2014 for Bug-23004
			end 
			else
				begin
					Set @tSqlCommand='update a Set a.Proj_RePay=0,a.Repay_Amt=0,a.Cl_Bal=a.Op_Bal
					From Emp_Loan_Advance_Details a inner join #TrigInsert b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_month) where a.pay_month='+cast(@Pay_Month as varchar)+''+
					' and a.employeecode='+char(39)+@EmployeeCode+char(39)+' and a.Pay_Year='+cast(@Pay_Year as varchar)+' and a.Pay_Month='+cast(@Pay_Month as varchar)	-- Added by Sachin N. S. on 28/05/2014 for Bug-23004 

					Execute Sp_ExecuteSql @tSqlCommand
--					execute Usp_Ent_Emp_Update_Loan_Balance @EmployeeCode,@Pay_Year,@Pay_Month,@Fld_Nm
					execute Usp_Ent_Emp_Update_Loan_Balance @EmployeeCode,@Pay_Year,@Pay_Month,@Fld_Nm,'U'				-- Changed by Sachin N. S. on 28/05/2014 for Bug-23004
					
					Fetch next From Cur_Trig_Loan into @Fld_Nm,@inst_amt,@Tran_Cd,@Employeecode,@Pay_year,@Pay_month	-- Added by Sachin N. S. on 28/05/2014 for Bug-23004
				end
--Added by Archana K. on 18/09/13 for Bug-18264 end
--		Fetch next From Cur_Trig_Loan into @Fld_Nm--Commented by Archana on 18/09/13 for Bug-18246
		end
		Close Cur_Trig_Loan
		DeAllocate Cur_Trig_Loan
--	if exists ( Select distinct EmployeeCode From inserted)
--	begin
--		Select @Pay_Year=Pay_Year,@Pay_Month=Pay_Month from inserted
--		Select * into #TrigInsert From inserted
--		Declare Cur_Trig_Loan cursor for Select Fld_Nm from Emp_Loan_Advance a inner join Emp_Loan_Advance_Details b on (a.Tran_Cd=b.Tran_cd) --where Pay_Year=@Pay_Year and Pay_Month=@Pay_Month
--		Open Cur_Trig_Loan
--		Fetch next From Cur_Trig_Loan into @Fld_Nm
--		while(@@Fetch_Status=0)
--		begin
--			
--			--Set @tSqlCommand='update a Set a.Proj_RePay=0,a.Repay_Amt=b.'+@Fld_Nm+' From Emp_Loan_Advance_Details a inner join #TrigInsert b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_month)'
--			Set @tSqlCommand='update a Set a.Proj_RePay=0,a.Repay_Amt=b.'+@Fld_Nm+',a.Cl_Bal=a.Op_Bal+a.Inst_Amt+a.Interest-b.'+@Fld_Nm+' From Emp_Loan_Advance_Details a inner join #TrigInsert b on (a.EmployeeCode=b.EmployeeCode and a.Pay_Year=b.Pay_Year and a.Pay_Month=b.Pay_month)'	
--			print @tSqlCommand
--			Execute Sp_ExecuteSql @tSqlCommand
--			execute Usp_Ent_Emp_Update_Loan_Balance @EmployeeCode,@Pay_Year,@Pay_Month,@Fld_Nm
--			Fetch next From Cur_Trig_Loan into @Fld_Nm
--		end
--		Close Cur_Trig_Loan
--		DeAllocate Cur_Trig_Loan
--	end
end 




