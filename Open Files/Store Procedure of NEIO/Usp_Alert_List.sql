IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Alert_List]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[Usp_Alert_List]  
end
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ramya.
-- Create date: 17/10/2011
-- Description:	This Stored procedure is useful to get the curret Alertl.
-- =============================================

Create procedure [dbo].[Usp_Alert_List]
@UserName varchar(30)
as
begin
Declare @UserRole varchar(30)
declare @sqlcommand nvarchar(1000)

select @UserRole=[User_Roles] from vudyog..[user] where [user]=@UserName
set @UserRole=rtrim(@UserRole)
set @UserRole=rtrim(@UserRole)
select  Alert_Name,Alert_Description,Last_Updated,Table_Name,IsActive into #t from Alert_Master where [IsActive]=1 and charindex('<<'+@UserRole+'>>',UserRoles)>0

select Alert_Name,Table_Name into #t1 from #t
declare @Alert_Name varchar(60),@TableName varchar(20),@cnt int,@ParmDefinition nvarchar(500);

declare  cur_alert cursor for select Alert_Name,Table_Name from #t1
 open cur_alert
  fetch next from cur_alert into @Alert_Name,@TableName
	while(@@fetch_status=0)
	  begin
	
            
			set @sqlcommand=N'select @count=count(*) from '+ @TableName;
			set @ParmDefinition = N'@count int OUTPUT' ;
			execute sp_executesql @sqlcommand,@ParmDefinition,@count=@cnt output;
           
             print @cnt
			   if @cnt=0
				begin
					delete from #t where table_name=@tablename
				end
            print @cnt
	     fetch next from cur_alert into @Alert_Name,@TableName
	  end
     select * from #t order by Alert_Name
   close cur_alert
 deallocate cur_alert

end


