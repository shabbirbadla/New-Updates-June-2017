IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Rep_Emp_Monthly_Payroll_Summary_DADOS]') AND type in (N'P', N'PC'))
Begin
	DROP PROCEDURE [dbo].[Usp_Rep_Emp_Monthly_Payroll_Summary_DADOS]
end
go
-- =============================================
-- Author:		Rupesh.
-- Create date: 01/11/2012
-- Description:	This Stored Procedure is useful for Monthly Payroll Summary in Dados reports
-- =============================================
CREATE procedure [dbo].[Usp_Rep_Emp_Monthly_Payroll_Summary_DADOS]
@sDate smalldatetime,
@eDate smalldatetime
as 
begin 
	Declare @SQLCOMMAND as NVARCHAR(4000),@TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@MonthNm Varchar(60),@Pay_Month int
	Declare @Head_Nm Varchar(100),@Head_Type Varchar(100),@Short_Nm Varchar(100),@Fld_Nm Varchar(30),@Pay_Year varchar(30),@SortOrd int ,@mSortOrd Int--, @FldtList as NVARCHAR(4000)

	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
						+ (DATEPART(ss, GETDATE()) * 1000 )
						+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM

	Set @SQLCOMMAND='Select [Pay Head Type]=space(100),[Pay Head Name]=space(100),[Pay Head Short Name]=space(100),Total=cast(0 as Decimal(17,2))'

	Declare CurSumCol Cursor for Select distinct DateName( Month , DateAdd( month , cast(M.Pay_Month as int), 0 )-1  ) +'-'+cast (M.Pay_Year as varchar) as MonthNm,Pay_Month From Emp_monthly_Payroll M where (M.MnthLastDt between @sDate and @eDate) Order by Pay_Month
	Open CurSumCol
	Fetch Next From CurSumCol into @MonthNm,@Pay_Month
	While(@@Fetch_Status=0)
	Begin
		Set @SQLCOMMAND=Rtrim(@SQLCOMMAND)+',['+rtrim(@MonthNm)+']=cast(0 as Decimal(17,2))'
		print @SQLCOMMAND
		Fetch Next From CurSumCol into @MonthNm,@Pay_Month
	End
	Close CurSumCol
	DeAllocate CurSumCol

	Set @SQLCOMMAND=Rtrim(@SQLCOMMAND)+' into '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Set @SQLCOMMAND='Delete From '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Declare curPayHeadDet cursor for 
	Select [Head_Nm],b.[HeadType],a.Short_Nm,a.Fld_Nm,b.SortOrd,a.SortOrd From Emp_Pay_Head_Master a inner join Emp_Pay_Head b on (a.HeadTypeCOde=b.HeadTypeCode)
	union Select 'Gross Payment','Gross Payment','Gross Payment','GrossPayment',100,100 
	--union Select 'Net Payment','Net Payment','Net Payment','NetPayment',100,100 
	order by b.SortOrd,a.SortOrd

	open curPayHeadDet
	fetch next from curPayHeadDet into @Head_Nm,@Head_Type,@Short_Nm,@Fld_Nm,@mSortOrd,@SortOrd
	while (@@Fetch_Status=0)
	begin
		Set @SQLCOMMAND='Insert Into '+@TBLNAME1+' Select '+Char(39)+Rtrim(@Head_Type)+Char(39)+','+Char(39)+@Head_Nm+Char(39)+','+Char(39)+@Short_Nm+Char(39)
		Set @SQLCOMMAND=Rtrim(@SQLCOMMAND)+' ,Sum(case when (MnthLastDt between '+char(39)+cast(@sDate as Varchar)+char(39)+' and '+char(39)+cast(@eDate as varchar)+char(39)+') then '+@Fld_Nm+' else 0 end)'
		Declare CurSumCol Cursor for Select distinct Pay_Year,DateName( Month , DateAdd( month , cast(M.Pay_Month as int), 0 )-1  ) +'-'+cast (M.Pay_Year as varchar) as MonthNm,Pay_Month From Emp_monthly_Payroll M where (M.MnthLastDt between @sDate and @eDate) Order by Pay_Year,Pay_Month
		Open CurSumCol
		Fetch Next From CurSumCol into @Pay_Year,@MonthNm,@Pay_Month
		While(@@Fetch_Status=0)
		Begin
			Set @SQLCOMMAND=Rtrim(@SQLCOMMAND)+',Sum(Case when Pay_Year='+char(39)+@Pay_Year+char(39)+' and Pay_Month='+Cast(@Pay_Month as Varchar)+' Then '+@Fld_Nm+' Else 0 end)'
			Fetch Next From CurSumCol into @Pay_Year,@MonthNm,@Pay_Month
		End
		Close CurSumCol
		DeAllocate CurSumCol
		--
		Set @SQLCOMMAND=Rtrim(@SQLCOMMAND)+' From Emp_MonthLy_Payroll'
		print @SQLCOMMAND
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		fetch next from curPayHeadDet into @Head_Nm,@Head_Type,@Short_Nm,@Fld_Nm,@mSortOrd,@SortOrd
	end
	Close curPayHeadDet
	DeAllocate curPayHeadDet

	
	Set @SQLCOMMAND='Delete From '+@TBLNAME1+' Where Total=0'
	--EXECUTE SP_EXECUTESQL @SQLCOMMAND

	Set @SQLCOMMAND='Select * From '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	--execute [Usp_Rep_Emp_Monthly_Payroll_Summary_DADOS] '','2012',''
End