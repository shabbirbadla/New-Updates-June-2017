IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Ent_Pay_Head_Slab_Master]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].Usp_Ent_Pay_Head_Slab_Master 
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Ramya.
-- Create date: 12/03/2012
-- Description:	
-- =============================================

Create procedure [dbo].[Usp_Ent_Pay_Head_Slab_Master]
@FieldNm varchar(30),@fRange decimal(17,2),@tRange decimal(17,2),@Category varchar(20),@State varchar(30),@Sdate SmallDatetime,@Id int
as
Begin
 
declare @fromrange decimal(17,2), @torange decimal(17,2),@RetVal varchar(1),@RetRange decimal(17,2),@WhereCond nvarchar(500),@SqlCommand nvarchar(250)
set @RetVal='N'
Set @RetRange=0
set @WhereCond=' where fld_nm='+char(39)+@FieldNm+char(39)


if(isnull(@Category,'')!='')
begin
	set @WhereCond=@WhereCond+' and cate='+char(39)+@Category+char(39)
end


if(isnull(@State,'')!='') 
begin
	set @WhereCond=@WhereCond+' and state='+char(39)+@State+char(39)
end


if(isnull(@Sdate,'')!='')
begin
	set @WhereCond=@WhereCond+' and Sdate='+char(39)+ cast(@Sdate as varchar) +char(39)
end


Print @Id
--if(@Id != null and @Id!='')
--begin
-- set @WhereCond=@WhereCond+' and id='+@Id
--end

set @WhereCond=@WhereCond+' and id!='+cast(@Id as varchar)


print @WhereCond
--declare c1 cursor for select sDate,eDate from emp_holiday_master +@WhereCond
set @SqlCommand='declare c1 cursor for select RangeFrom,RangeTo from emp_pay_head_slab_master' +@WhereCond
print @SqlCommand
EXEC SP_EXECUTESQL  @SqlCommand
open c1
fetch next from c1 into @fromrange,@torange
  while(@@fetch_status=0)
	begin
     print @fromrange
		if(@fRange=@fromrange or @fRange=@torange or (@fRange between @fromrange and @torange ))
		begin
		 Set @RetVal='Y'
         Set @RetRange=@fRange
		end

		if(@tRange=@fromrange or @tRange=@torange or (@tRange between @fromrange and @torange ))
		begin
		  Set @RetVal='Y'
		    if(isnull(@RetRange,0)=0)
			 begin
              Set @RetRange=@tRange
             end
		end
    fetch next from c1 into @fromrange,@torange
  end
close c1
deallocate c1
Select @RetVal as RetVal,@RetRange as RetRange
end


