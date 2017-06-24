IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_DataImport_SO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Usp_DataImport_SO]
Go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


CREATE procedure [dbo].[Usp_DataImport_SO]
@Code Varchar(3),@fName varchar(100),@Loc_Code Varchar(7),@Tbl Varchar (50)
as 
Begin
	Declare @SqlCommand nvarchar(max),@SqlCommand1 nvarchar(max),@UpdateSqlTmp nvarchar(max),@UpdateSql nvarchar(max),@fld_Name varchar(60),@Table_Names Varchar (100),@Table_Name Varchar (100), @pos int  -- Changed by Archana K. on 16/10/2012 for Bug-5837
	Declare @TblFldList as VARCHAR(50)
	Declare @UpdateStmt nvarchar(max),@FilterCondition nvarchar(max),@SqlCommnad nvarchar(max) -- Changed by Archana K. on 16/10/2012 for Bug-5837
	Declare @SqlCommand3 nvarchar(max),@UpdateSql3 nvarchar(max)		-- Changed by Archana K. on 09-04-2013

	set @Table_Names=@Tbl
	
	Set @TblFldList = '##TblFldList'+(SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	select * into #lcode_vw from lcode where entry_ty='SO' -- Lcode View 
		
	execute USP_DataImport_GetFiledSchema @Code,@fName,@Table_Names,@TblFldList
	print 'a '+@TblFldList

	set @Table_Names=@Table_Names+','
	set @pos=2
	
	while (@pos>1)
	begin
		set @pos=charindex(',',@Table_Names)
		set @Table_Name=substring(@Table_Names,0,@pos)
		print @pos
		set @Table_Names=substring(@Table_Names,@pos+1,len(@Table_Names)-@pos)

		set @SqlCommand=''
		set @UpdateSql=''
		if (@Table_Name<>'')
		begin
			Declare Cur_DataImp1 Cursor for select UpdateStmt,FilterCondition from ImpDataTableUpdate where Code=@Code and TableName=@Table_Name order by updOrder
			open Cur_DataImp1
			fetch next from Cur_DataImp1 into @UpdateStmt,@FilterCondition
			while (@@Fetch_Status=0)
			begin
				set @SqlCommnad='Update '+@Table_Name+' Set '+rtrim(@UpdateStmt) +' Where '+rtrim(@FilterCondition)
				Print '??'
				print @SqlCommnad
			
				fetch next from Cur_DataImp1 into @UpdateStmt,@FilterCondition
			end
			close Cur_DataImp1
			DeAllocate Cur_DataImp1

			set @SqlCommand1 ='Declare cur_AcMast cursor for select distinct Fld_Name from '+@TblFldList+' where Tbl_Name='+char(39)+@Table_Name+'_'+@fName+char(39)

			execute sp_executesql @SqlCommand1
			open cur_AcMast
			fetch next from cur_AcMast into  @fld_Name
			while (@@fetch_Status=0)	
			begin
				
				set @UpdateSql=LTRIM(rtrim(@UpdateSql))+RTRIM(cast(replicate(' ',100) as nvarchar(max)))+',a.['+@fld_name+']=b.['+@fld_name+']' -- Changed by Archana K. on 16/10/2012 for Bug-5837
				set @SqlCommand=rtrim(@SqlCommand)+',['+@fld_Name+']'
				
				fetch next from cur_AcMast into  @fld_Name

			end
			close cur_AcMast
			deallocate cur_AcMast
			
		
			--print 'aaa '+@SqlCommand
			if (@SqlCommand<>'')
			begin
				-----Birendra : Bug-20512 on 13/02/2014 :Start:
				if substring(@Table_Name,3,4)='Main'
				begin

					set @UpdateSqlTmp='Update a set a.compid=b.compid  from '+@Table_Name+'_'+@fName+ ' a inner join vudyog..co_mast b on (a.l_yn=cast(year(b.sta_dt)as varchar(4))+''-''+cast(year(b.end_dt) as varchar(4)) and b.dbname=db_name())'		
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp 
				end
				-----Birendra : Bug-20512 on 13/02/2014 :End:

				if substring(@Table_Name,3,4)='Item'
				begin
					set @UpdateSqlTmp='Update a set a.it_code=b.it_code  from '+@Table_Name+'_'+@fName+ ' a inner join it_mast b on (a.item=b.it_name)'
					Print '5. '+@UpdateSqlTmp
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp

				set @UpdateSqlTmp='Update a set a.ac_id=b.ac_id  from '+@Table_Name+'_'+@fName+ ' a inner join ac_mast b on (a.party_nm=b.ac_name)'		-- Added by Archana on 09-04-2013 for Bug-5837
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp ---Added by Amrendra Afterwards				--  Added by Archana on 09-04-2013 for Bug-5837
				-----Birendra : Bug-20512 on 13/02/2014 :Start:
					set @UpdateSqlTmp='Update a set a.compid=b.compid  from '+@Table_Name+'_'+@fName+ ' a inner join vudyog..co_mast b on (a.l_yn=cast(year(b.sta_dt)as varchar(4))+''-''+cast(year(b.end_dt) as varchar(4)) and b.dbname=db_name())'		
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp 
					set @UpdateSqlTmp='execute Usp_ItBal_ItBalW'+char(39)+@Table_Name+'_'+@fName+char(39)+','+char(39)+replace(@Table_Name,'item','main')+'_'+@fName+char(39)+','+char(39)+@Table_Name+char(39)+','+char(39)+replace(@Table_Name,'item','main')+char(39)+','+char(39)+replace(@Table_Name,'item','qty')+char(39)
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp
				-----Birendra : Bug-20512 on 13/02/2014 :End:

				end

				if substring(@Table_Name,3,5)='acdet'
				begin
					set @UpdateSqlTmp='Update a set a.ac_id=b.ac_id  from '+@Table_Name+'_'+@fName+ ' a inner join ac_mast b on (a.ac_name=b.ac_name)'		--  Added by Archana on 09-04-2013 for Bug-5837
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp ---Added by Amrendra Afterwards				--  Added by Archana on 09-04-2013 for Bug-5837
				-----Birendra : Bug-20512 on 13/02/2014 :Start:
					set @UpdateSqlTmp='Update a set a.compid=b.compid  from '+@Table_Name+'_'+@fName+ ' a inner join vudyog..co_mast b on (a.l_yn=cast(year(b.sta_dt)as varchar(4))+''-''+cast(year(b.end_dt) as varchar(4)) and b.dbname=db_name())'		
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp 
					set @UpdateSqlTmp='execute Usp_Import_ac_bal'+char(39)+@Table_Name+'_'+@fName+char(39)+','+char(39)+@Table_Name+char(39)
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp
				-----Birendra : Bug-20512 on 13/02/2014 :End:
				end


				set @SqlCommand=substring(@SqlCommand,2,len(@SqlCommand)-1)
				set @UpdateSql=substring(@UpdateSql,2,len(@UpdateSql)-1)
				print @Table_Name
				set @SqlCommand3=' insert into '+@Table_Name+' ('+rtrim(@SqlCommand)+',DataImport'+') '+' Select '+rtrim(@SqlCommand)+',DataExport1 from '+@Table_Name+'_'+@fName

				set @SqlCommand3=rtrim(@SqlCommand3)+' where DataExport1 not in (Select distinct DataImport From '+@Table_Name+')'

				set @UpdateSql3=N'Update a set '+LTRIM(rtrim(@UpdateSql))+' from '+@Table_Name+' a inner join '+@Table_Name+'_'+@fName+' b on (a.DataImport=b.DataExport1)' -- Changed by Archana K. on 16/10/2012 for Bug-5837

				Print '2. '
				
				print ' Update Statement:- '+@UpdateSql3 
				EXECUTE SP_EXECUTESQL @UpdateSql3	
				Print '2a. '
				print ' Insert Statement:- '+ @SqlCommand3			
				EXECUTE SP_EXECUTESQL @SqlCommand3
	
				if substring(@Table_Name,3,4)='Main'
				begin
					set @UpdateSql='Update a set a.ac_id=isnull(b.ac_id,0),
												 a.cons_id=isnull(b.ac_id,0)
												 from '+@Table_Name+' a
												 left join SOmain_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
												 left join ac_mast b on (a.party_nm=b.ac_name)'
					Print '3. Main Ac_Id Update : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql

					set @UpdateSql='Update a set a.scons_id=isnull(c.shipto_id,0)
												  from '+@Table_Name+' a
												 inner join SOmain_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
												 inner join shipto c on (e.Scons_id=c.location_id)'
					Print '3. Main Scons_Id Update : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql

					set @UpdateSql='Update a set a.sac_id=isnull(d.shipto_id,0)  from '+@Table_Name+' a
												 inner join SOmain_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
												 inner join shipto d on (e.sac_id=d.location_id)'

					set @UpdateSql='Update somain set cons_id=0  where cons_id is null'
					Print 'Update for Nulls 1 : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
					
					set @UpdateSql='Update somain set sac_id=0  where sac_id is null'
					Print 'Update for Nulls 1 : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
					set @UpdateSql='Update somain set scons_id=0  where scons_id is null'
					Print 'Update for Nulls 1 : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
												 


					Print '3.Main sAc_Id Update : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
						
				end
				if substring(@Table_Name,3,4)='Item'
				begin
				set @UpdateSql='Update a set a.ac_id=b.ac_id  from '+@Table_Name+' a inner join ac_mast b on (a.party_nm=b.ac_name)'
--					set @UpdateSql='Update a 
--										set a.manuac_id=b.ac_id,a.manusac_ID=c.shipto_ID  
--										from '+@Table_Name+' a 
--										inner join SOitem_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
--										inner join ac_mast b on (e.manuAc_Name=b.ac_name)
--										inner join shipto c on (e.manusAc_Name=c.location_id)'
--					Print 'Update for Ac IDs 0 : ' +@UpdateSql ---Added by Amrendra Afterwards
					EXECUTE SP_EXECUTESQL @UpdateSql ---Added by Amrendra Afterwards
--
--					set @UpdateSql='Update SOitem set manuac_id=0  where manuac_id is null'
--					Print 'Update for Nulls 1 : ' +@UpdateSql
--					EXECUTE SP_EXECUTESQL @UpdateSql
--					set @UpdateSql='Update SOitem set manusac_ID=0  where manusac_ID is null'
--					Print 'Update for Nulls 1 : ' +@UpdateSql
--					EXECUTE SP_EXECUTESQL @UpdateSql

					Print '4. '+@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
--					set @UpdateSql='Update a set a.it_code=b.it_code  from '+@Table_Name+' a inner join it_mast b on (a.it_code=b.it_code)'
--					Print '5. '+@UpdateSql
--					EXECUTE SP_EXECUTESQL @UpdateSql

					set @SqlCommand = 'update a set a.Tran_Cd = c.Tran_cd 
					from SOitem_' +@fName+' a 
					inner join  somain_'+@fName+ ' b on (a.oldTran_cd=b.oldTran_cd)
					inner join somain c on (b.dataExport1=c.dataimport)'
					Print '6. '
					print @SqlCommand					
					EXECUTE SP_EXECUTESQL @SqlCommand
					set @SqlCommand = 'update a set a.Tran_cd=b.Tran_cd
					from soitem a 
					inner join soitem_'+@fName+' b on (a.dataimport=b.dataExport1)
					inner join somain_'+@fName+' c on(b.oldTran_cd=c.OldTran_cd)'
					Print '7. '
					print @SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand
					---Update trancd in temp funda
				end
--Commented by Archana K. on 08/10/12 for Bug-5837  Start
--				if @Table_Name='Manu_Det'
--				begin
--					set @UpdateSql='update a set a.TRAN_CD=SO.TRAN_CD
--						FROM MANU_DET a INNER JOIN SOmAIN_'+@fName+' X ON (a.TRAN_CD=X.TRAN_CD)
--						INNER JOIN SOMAIN SO ON (SO.DATAIMPORT=X.DATAEXPORT1)
--						where a.entry_ty=''SO'' '
--					Print '8.  : '+@UpdateSql
--					EXECUTE SP_EXECUTESQL @UpdateSql
----					set @UpdateSql='Update a set a.ManuAc_ID=b.AC_Id  
----						from Manu_Det a inner join manu_det_' +@fName+' c on (a.dataimport=c.dataexport1) 
----						inner join ac_mast b on (c.ManuAc_Name=b.ac_name)'
----					Print '9. Current Focus 3 : '+@UpdateSql
----					EXECUTE SP_EXECUTESQL @UpdateSql
----					set @UpdateSql='Update a set a.ManuSAc_ID=b.ShipTo_Id  
----							from Manu_Det a inner join manu_det_' +@fName+' c on (a.dataimport=c.dataexport1) 
----							inner join ShipTo b on (c.ManuSAc_Name=b.Location_ID)'
----					Print '10. '+@UpdateSql
----					EXECUTE SP_EXECUTESQL @UpdateSql
--					--- Must Update Tran_CD
--				end
--Commented by Archana K. on 08/10/12 for Bug-5837  End

----xxxxx Gen_SRNo Generation xxxxx----
				if substring(@Table_Name,3,4)='GEN_SRNO'
				begin
					set @SqlCommand = 'update a set  a.Tran_Cd = c.Tran_Cd 
						from gen_srno a 
						inner join SOmain_' +@fName+' b on (a.Tran_cd=b.oldTran_cd)
						inner join SOmain c on (b.DataExport1=c.DataImport)'
					Print '11. ' +@SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand
				End
----xxxxx Gen_SRNo Generation xxxxx----

				-- Below Updating Nulls to their default Values.
				set @UpdateSql='execute Update_table_column_default_value '+char(39)+@Table_Name+char(39)+',1'
				EXECUTE SP_EXECUTESQL @UpdateSql

			end
		end
	end
----xxxxx Doc_no Generation xxxxx----
	declare @Sdate varchar(10),@Edate varchar(10)
	set @Sdate =''
	set @Edate =''
	
	set @UpdateSql='select @EdateOut=max(convert(varchar(10),date,103)) from SOMAIN_'+@fName
	Print '11. '+@UpdateSql
	EXECUTE SP_EXECUTESQL @UpdateSql,N'@EdateOut varchar(10) output',@EdateOut=@Edate output
	set @UpdateSql='select @SdateOut=min(convert(varchar(10),date,103)) from SOMAIN_'+@fName
	Print '12. '+@UpdateSql
	EXECUTE SP_EXECUTESQL @UpdateSql,N'@SdateOut varchar(10) output',@SdateOut=@Sdate output
	print isnull(@Sdate,'NULL')
	print isnull(@Edate,'NULL')
--	set dateformat dmy -- Commented by Archana K. on 13/05/13 for Bug-5837
	SELECT @Sdate,@Edate
	if isnull(@Sdate,'')<>'' or isnull(@Edate,'')<>''
		execute Usp_DocNo_Renumbering 'SO',@Sdate,@Edate,''
----xxxxx Doc_no Generation xxxxx----

----xxxxx Gen_Inv Updation xxxxx----	
	declare @FinYear varchar(9)
	set @UpdateSql='select distinct l_yn into ##FinYear from SOMAIN_'+@fName
	Print '13. '+@UpdateSql
	EXECUTE SP_EXECUTESQL @UpdateSql
	while exists( select top 1 l_yn from ##FinYear)
	begin
		select top 1 @FinYear=l_yn from ##FinYear
		print 'Found :' + @FinYear
		execute Usp_Gen_Inv_Updation 'SO',@FinYear
		delete from ##FinYear where l_yn=@FinYear
	end
	drop table ##FinYear
----xxxxx Gen_Inv Updation xxxxx----	
end

