IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_Ent_Get_LOC_Details]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Usp_Ent_Get_LOC_Details]
Go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Amrendra Kumar Singh
-- Create date: 07/06/2012
-- Description:	This Stored procedure is useful to get Column Description From SQL Table MetaData 
-- Modification Date/By/Reason: 
-- Remark:
-- =============================================

Create Procedure [dbo].[Usp_Ent_Get_LOC_Details]
@TableName varchar(60)--,@uniqueCol varchar(60)
as
Begin

Declare @SqlCommand nvarchar(4000),@Col_Desc varchar(60) ,@Value varchar(60)

set @SqlCommand= ''

if exists (select * from sysobjects where [name]=@TableName ) 
begin 
	set @SqlCommand = 'select Top 1 RetValue from '+ @TableName
	execute sp_executeSql @SqlCommand
	set @SqlCommand ='drop table '+@TableName
	execute sp_executeSql @SqlCommand
end
else
begin
select '' RetValue
end 
end


