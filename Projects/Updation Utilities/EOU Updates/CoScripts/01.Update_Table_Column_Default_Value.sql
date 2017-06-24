/****** Object:  StoredProcedure [dbo].[Update_table_column_default_value]    Script Date: 03/03/2010 10:15:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 
-- Description:	This Stored procedure is useful to update Default Value for null.
-- i.e.        : execute Update_table_column_default_value '%main' will update all main table like ptmain,stmain,bpmain etc...
-- Modify date: 16/05/2007
-- Modified Date\By\Reason: Rupesh Prajapati. Add parameter @regular_table. Changes done for temp table created with ##. 
-- i.e. execute Update_table_column_default_value '##table',0 execute Update_table_column_default_value '%it_mast',1	
-- Remark:This store proc. used in RG23D Report
 =============================================*/
CREATE  procedure [dbo].[Update_table_column_default_value]
@tblname varchar(200),@regular_table bit
as
begin
	declare @fldnm varchar(20),@fldtype varchar(20),@tblnm varchar(200),@cons_name varchar(100)
	declare @sqlcommand nvarchar(4000),@cnt int,@dbname varchar(100)
	if @regular_table=1
	begin	
		set @dbname=''
	end
	else
	begin
		set @dbname='tempdb..'
	end	

	set @sqlcommand=''
	set @sqlcommand=rtrim(@sqlcommand)+' declare cur_tbl_defval cursor for'
	set @sqlcommand=rtrim(@sqlcommand)+' select fldnm=clm.[name],fldtype=typ.name ,tblnm=sobj.name'
	set @sqlcommand=rtrim(@sqlcommand)+' from '+@dbname+'syscolumns clm '
	set @sqlcommand=rtrim(@sqlcommand)+' inner join '+@dbname+'sysobjects sobj on (sobj.id=clm.id)'
	set @sqlcommand=rtrim(@sqlcommand)+' inner join '+@dbname+'systypes typ on (clm.xtype=typ.xtype)'
	set @sqlcommand=rtrim(@sqlcommand)+' where sobj.[name] like '+char(39)+@tblname+char(39)+' and sobj.xtype=''U'' and clm.colstat <> 1'
	set @sqlcommand=rtrim(@sqlcommand)+' order by sobj.[name]'
	print @sqlcommand
	execute sp_executesql @sqlcommand
	set @sqlcommand=''
	open cur_tbl_defval
	fetch next from cur_tbl_defval into  @fldnm,@fldtype,@tblnm
	while (@@fetch_status=0)
	begin
		set @sqlcommand='update '+@tblnm +' set ['+rtrim(@fldnm)+'] '+'=isnull(['+rtrim(@fldnm)+'],'
		if @fldtype in ('char','datetime','smalldatetime','text','varchar')
		begin
			set @sqlcommand=rtrim(@sqlcommand)+''''''+')'
		end
		if @fldtype in ('bigint','decimal','float','int','numeric','smallint','tinyint','real','bit')
		begin
			set @sqlcommand=rtrim(@sqlcommand)+'0'+')'
		end
		print @sqlcommand
		execute sp_executesql @sqlcommand
		fetch next from cur_tbl_defval into  @fldnm,@fldtype,@tblnm
	end
	close cur_tbl_defval
	deallocate cur_tbl_defval
end

Print 'Updation of ' + rtrim(@tblname) + ' Is Completed Sucessfully'


