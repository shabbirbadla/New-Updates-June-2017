-- =============================================
-- Created By    : Priyanka Himane
-- Create Date   : 07/12/2012
-- Description   : This Stored procedure is useful to Generate data for AA EO Status Summary Report.
-- Modified By   :
-- Modified Date :
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_AA_EO_STATUS_SUMMARY]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000)
,@SDATE SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60),@SAMT FLOAT
,@EAMT FLOAT,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA VARCHAR(100)
AS

Create Table #AA_EO_SUMM_REP(Srno Int,Sub_order Varchar(10)
,licen_no varchar(100)
,Bit_Val1 Bit
,Date_val1 SmallDateTime,Date_val2 SmallDateTime,Date_val3 SmallDateTime,Date_val4 SmallDateTime
,Num_Val1 Numeric(14,2),Num_Val2 Numeric(14,2),Num_Val3 Numeric(14,2),Num_Val4 Numeric(14,2)
,Num_Val5 Numeric(14,2),Num_Val6 Numeric(14,2),Num_Val7 Numeric(14,2),Num_Val8 Numeric(14,2)
,Num_Val9 Numeric(14,2),Num_Val10 Numeric(14,2)
,Str_Val1 Varchar(100),Str_Val2 Varchar(100),Str_Val3 Varchar(100),Str_Val4 Varchar(100)
,Str_Val5 Varchar(100),Str_Val6 Varchar(100)
,Txt_Val1 Text
)
declare @symbol varchar(5)
select @symbol=b.symbol from aa_mast a
inner join curr_mast b on (a.fcid=b.currencyid)

--select @symbol

-- Point No. 1--start
Insert Into #AA_EO_SUMM_REP(Srno,Sub_order,licen_no,Date_Val1,Date_Val2,Num_Val1,Num_Val2,Num_Val3,Num_Val4,Num_Val5,Num_Val6,Num_Val7,Num_Val8,Num_Val9,Str_Val1)
Select 1, 'i', a.licen_no, issue_dt, expiry_dt
,avg_eo_inr = impduty_amt
,special_eo_inr = a.act_duty
,tot_eo_inr = (a.impduty_amt + a.act_duty)
,licen_period
,avg_eo_per_mon_inr = (a.impduty_amt + a.act_duty)/licen_period
,avg_eo_fc = cast((isnull(a.impduty_amt,0.00) / case when cast(isnull(b.fcexrate,0) as decimal(12,2)) <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
,special_eo_fc = cast((isnull(a.act_duty,0.00) / case when cast(isnull(b.fcexrate,0) as decimal(12,2)) <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
,tot_eo_fc = cast(((isnull(a.impduty_amt,0.00) + isnull(a.act_duty,0.00)) / case when cast(isnull(b.fcexrate,0) as decimal(12,2)) <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
,avg_eo_per_mon_fc = cast(((isnull(a.impduty_amt,0.00) + isnull(a.act_duty,0.00)) / (case when cast(isnull(licen_period,0) as decimal(12,2)) <> 0.00 then licen_period else 1 end) 
						/ case when cast(isnull(b.fcexrate,0) as decimal(12,2)) <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))

--,avg_eo_fc = cast((isnull(a.impduty_amt,0.00) / case when b.fcexrate is not null or b.fcexrate <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
--,special_eo_fc = cast((isnull(a.act_duty,0.00) / case when b.fcexrate is not null or b.fcexrate <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
--,tot_eo_fc = cast(((isnull(a.impduty_amt,0.00) + isnull(a.act_duty,0.00)) / case when b.fcexrate is not null or b.fcexrate <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
--,avg_eo_per_mon_fc = cast(((isnull(a.impduty_amt,0.00) + isnull(a.act_duty,0.00)) / (case when licen_period is not null or licen_period <> 0.00 then licen_period else 1 end) 
--						/ case when b.fcexrate is not null or b.fcexrate <> 0.00 then b.fcexrate else 1 end) as decimal(12,2))
, @symbol
From aa_mast a
left join ptmain b on (a.fcid=b.fcid)
Where a.licen_no=@EXPARA

If Not Exists(Select * from #AA_EO_SUMM_REP Where srno=1 and sub_order='i')
begin
Insert Into #AA_EO_SUMM_REP(srno,sub_order)
Select 1,'i'
end
-- Point No. 1--end

-- Point No. 2--start
declare @n_period int,@i int,@tot_eo decimal(18,2)
,@startdate smalldatetime,@enddate smalldatetime
,@start smalldatetime,@end smalldatetime

select @n_period=licen_period from aa_mast
Where licen_no=@EXPARA
set @i = 1

Select @startdate = issue_dt
From aa_mast
Where licen_no=@EXPARA

Select @tot_eo=(impduty_amt+act_duty) 
From aa_mast 
Where licen_no=@EXPARA

while (@i <= @n_period)
begin		
	set @start = (case when @i=1 then @startdate else @enddate end)
	set @end = dateadd(mm,0,dateadd(mm,@i,@startdate)-1)
		
	Insert Into #AA_EO_SUMM_REP(Srno,Sub_order,licen_no,Date_Val1,Date_Val2,Str_Val1)
	SELECT 2,'',@EXPARA,@start,@end,@symbol

	set @enddate = dateadd(mm,@i,@startdate)			
	set @i = @i + 1
end
--select * from #AA_EO_SUMM_REP
--return
declare @licenno varchar(100),@date1 smalldatetime,@date2 smalldatetime
,@tot_examt_inr decimal(12,2),@tot_examt_fc decimal(12,2),@tot_per decimal(12,2)

declare curUpdate cursor for 
Select licen_no,Date_Val1,Date_Val2 From #AA_EO_SUMM_REP Where srno=2 and sub_order=''
open curUpdate
fetch next from curUpdate into @licenno,@date1,@date2
while @@fetch_status = 0
begin
select @tot_examt_inr = 0, @tot_examt_fc = 0, @tot_per = 0
	select @tot_examt_inr=sum(net_amt)
	,@tot_examt_fc=sum(fcnet_amt)
	,@tot_per=sum(net_amt * 100/@tot_eo)
	from Stmain
	where Entry_Ty='EI' and aalic_no=@licenno
	and date between @Date1 and @Date2
	group by aalic_no

	Update a set a.Num_Val1=@tot_examt_inr,a.Num_Val2=@tot_examt_fc,a.Num_Val3=@tot_per
	From #AA_EO_SUMM_REP a
	Where a.licen_no=@licenno and a.srno=2 and (a.date_val1 = @date1 and a.date_val2 = @date2)

	fetch next from curUpdate into @licenno,@date1,@date2
end
close curUpdate
deallocate curUpdate

Update #AA_EO_SUMM_REP set Date_Val3=(select top 1 date from stmain where entry_ty='ei' and licen_no=@EXPARA order by date desc)
where srno=2 and sub_order=''

If Not Exists(Select * from #AA_EO_SUMM_REP Where srno=2 and sub_order='')
begin
Insert Into #AA_EO_SUMM_REP(srno,sub_order)
Select 2,''
end
-- Point No. 2--end

select * from #AA_EO_SUMM_REP

