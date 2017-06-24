If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='Usp_DataImport_ST')
Begin
	Drop Procedure Usp_DataImport_ST
End
GO

/****** Object:  StoredProcedure [dbo].[Usp_DataImport_ST]    Script Date: 09/23/2015 15:53:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Usp_DataImport_ST]
@Code Varchar(3),@fName varchar(100),@Loc_Code Varchar(7),@Tbl Varchar (50)
as 
Begin
	Declare @SqlCommand nvarchar(max),@SqlCommand1 nvarchar(max),@UpdateSqlTmp nvarchar(max),@UpdateSql nvarchar(max),@fld_Name varchar(60),@Table_Names Varchar (100),@Table_Name Varchar (100), @pos int		-- Changed by Sachin N. S. on 16/10/2012 for Bug-5837
	Declare @TblFldList as VARCHAR(50)		-- Changed by Sachin N. S. on 16/10/2012 for Bug-5837
	Declare @UpdateStmt nvarchar(max),@FilterCondition nvarchar(max),@SqlCommnad nvarchar(max)		-- Changed by Sachin N. S. on 16/10/2012 for Bug-5837
	Declare @SqlCommand3 nvarchar(max),@UpdateSql3 nvarchar(max)		-- Changed by Sachin N. S. on 08-04-2013

	set @Table_Names=@Tbl
	
	Set @TblFldList = '##TblFldList'+(SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	select * into #lcode_vw from lcode where entry_ty='ST' -- Lcode View 
		
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
				
				set @UpdateSql=LTRIM(rtrim(@UpdateSql))+ RTRIM(cast(replicate(' ',100) as nvarchar(max)))+',a.['+@fld_name+']=b.['+@fld_name+']'		---- Changed by Sachin N. S. on 16/10/2012 for Bug-5837
				set @SqlCommand=rtrim(@SqlCommand)+',['+@fld_Name+']'
				
				fetch next from cur_AcMast into  @fld_Name

			end
			close cur_AcMast
			deallocate cur_AcMast
			
			PRINT @UpdateSql
		
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
					
				set @UpdateSqlTmp='Update a set a.ac_id=b.ac_id  from '+@Table_Name+'_'+@fName+ ' a inner join ac_mast b on (a.party_nm=b.ac_name)'		-- Changed by Sachin on 08-04-2013
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp ---Added by Amrendra Afterwards				-- Changed by Sachin on 08-04-2013
				-----Birendra : Bug-20512 on 13/02/2014 :Start:
				set @UpdateSqlTmp='Update a set a.compid=b.compid  from '+@Table_Name+'_'+@fName+ ' a inner join vudyog..co_mast b on (a.l_yn=cast(year(b.sta_dt)as varchar(4))+''-''+cast(year(b.end_dt) as varchar(4)) and b.dbname=db_name())'		
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp 
					set @UpdateSqlTmp='execute Usp_ItBal_ItBalW'+char(39)+@Table_Name+'_'+@fName+char(39)+','+char(39)+replace(@Table_Name,'item','main')+'_'+@fName+char(39)+','+char(39)+@Table_Name+char(39)+','+char(39)+replace(@Table_Name,'item','main')+char(39)+','+char(39)+replace(@Table_Name,'item','qty')+char(39)
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp
				-----Birendra : Bug-20512 on 13/02/2014 :End:
					
				end

				if substring(@Table_Name,3,5)='acdet'
				begin
					set @UpdateSqlTmp='Update a set a.ac_id=b.ac_id  from '+@Table_Name+'_'+@fName+ ' a inner join ac_mast b on (a.ac_name=b.ac_name)'		-- Changed by Sachin on 08-04-2013
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp ---Added by Amrendra Afterwards				-- Changed by Sachin on 08-04-2013
				-----Birendra : Bug-20512 on 13/02/2014 :Start:
				set @UpdateSqlTmp='Update a set a.compid=b.compid  from '+@Table_Name+'_'+@fName+ ' a inner join vudyog..co_mast b on (a.l_yn=cast(year(b.sta_dt)as varchar(4))+''-''+cast(year(b.end_dt) as varchar(4)) and b.dbname=db_name())'		
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp 
					set @UpdateSqlTmp='execute Usp_Import_ac_bal'+char(39)+@Table_Name+'_'+@fName+char(39)+','+char(39)+@Table_Name+char(39)
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp
				-----Birendra : Bug-20512 on 13/02/2014 :End:
				end
				
--Added by Archana K. on 27/11/12 for Bug-5837 start
				if substring(@Table_Name,3,4)='Itref'
				begin
					set @UpdateSqlTmp='Update a set a.it_code=b.it_code  from '+@Table_Name+'_'+@fName+ ' a inner join it_mast b on (a.item=b.it_name)'
					--Print 'A. '+@UpdateSqlTmp
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp
				end
--Added by Archana K. on 27/11/12 for Bug-5837 end
					
				--Added By Kishor For Bug-26960 on 23/09/2015.. Start
				if @Table_Name ='IT_SRTRN'
				begin									
					set @SqlCommand=substring(@SqlCommand,2,len(@SqlCommand)-1)
					set @UpdateSql=substring(@UpdateSql,2,len(@UpdateSql)-1)

					set @SqlCommand3=' insert into '+@Table_Name+' ('+rtrim(@SqlCommand)+',DataImport,ImpKey'+') '+' Select '+rtrim(@SqlCommand)+',DataExport1,TmpKey from '+@Table_Name+'_'+@fName
					set @SqlCommand3=rtrim(@SqlCommand3)+' where DataExport1+TmpKey not in (Select distinct DataImport+ImpKey From '+@Table_Name+')'
				end
				else
				begin
				--Added By Kishor For Bug-26960 on 23/09/2015.. End
													
					set @SqlCommand=substring(@SqlCommand,2,len(@SqlCommand)-1)
					set @UpdateSql=substring(@UpdateSql,2,len(@UpdateSql)-1)
					print @Table_Name
					set @SqlCommand3='insert into '+@Table_Name+' ('+rtrim(@SqlCommand)+',DataImport'+') '+' Select '+rtrim(@SqlCommand)+',DataExport1 from '+@Table_Name+'_'+@fName							
					set @SqlCommand3=rtrim(@SqlCommand3)+' where DataExport1 not in (Select distinct DataImport From '+@Table_Name+')'
				end --Added By Kishor For Bug-26960 on 23/09/2015
				
				set @UpdateSql3=N'Update a set '+LTRIM(rtrim(@UpdateSql))+' from '+@Table_Name+' a inner join '+@Table_Name+'_'+@fName+' b on (a.DataImport=b.DataExport1)'		-- Changed by Sachin N. S. on 16/10/2012 for Bug-5837

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
												 left join STmain_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
												 left join ac_mast b on (a.party_nm=b.ac_name)'
					Print '3. Main Ac_Id Update : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
					
					set @UpdateSql='Update a set a.scons_id=isnull(c.shipto_id,0)
												  from '+@Table_Name+' a
												 inner join STmain_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
												 inner join shipto c on (e.Scons_id=c.location_id)'
					Print '3. Main Scons_Id Update : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql

					set @UpdateSql='Update a set a.sac_id=isnull(d.shipto_id,0)  from '+@Table_Name+' a
												 inner join STmain_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
												 inner join shipto d on (e.sac_id=d.location_id)'

					set @UpdateSql='Update stmain set cons_id=0  where cons_id is null'
					Print 'Update for Nulls 1 : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
					
					set @UpdateSql='Update stmain set sac_id=0  where sac_id is null'
					Print 'Update for Nulls 1 : ' +@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql
					set @UpdateSql='Update stmain set scons_id=0  where scons_id is null'
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
--										inner join STitem_'+@fName+ ' e on (a.dataimport=e.dataexport1) 
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
					from STitem_' +@fName+' a 
					inner join  stmain_'+@fName+ ' b on (a.oldTran_cd=b.oldTran_cd)
					inner join stmain c on (b.dataExport1=c.dataimport)'
					Print '6. '
					print @SqlCommand					
					EXECUTE SP_EXECUTESQL @SqlCommand
					set @SqlCommand = 'update a set a.Tran_cd=b.Tran_cd
					from stitem a 
					inner join stitem_'+@fName+' b on (a.dataimport=b.dataExport1)
					inner join stmain_'+@fName+' c on(b.oldTran_cd=c.OldTran_cd)'
					Print '7. '
					print @SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand
					---Update trancd in temp funda
					
				end
				if substring(@Table_Name,3,5)='Acdet'
				begin
				set @UpdateSql='Update a set a.ac_id=b.ac_id  from '+@Table_Name+' a inner join ac_mast b on (a.ac_name=b.ac_name)'
					EXECUTE SP_EXECUTESQL @UpdateSql 

					Print '14. '+@UpdateSql
					EXECUTE SP_EXECUTESQL @UpdateSql

					set @SqlCommand = 'update a set a.Tran_Cd = c.Tran_cd 
					from STacdet_' +@fName+' a 
					inner join  stmain_'+@fName+ ' b on (a.oldTran_cd=b.oldTran_cd)
					inner join stmain c on (b.dataExport1=c.dataimport)'
					Print '15. '
					print @SqlCommand					
					EXECUTE SP_EXECUTESQL @SqlCommand
					set @SqlCommand = 'update a set a.Tran_cd=b.Tran_cd
					from stacdet a 
					inner join stacdet_'+@fName+' b on (a.dataimport=b.dataExport1)
					inner join stmain_'+@fName+' c on(b.oldTran_cd=c.OldTran_cd)'
					Print '16. '
					print @SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand
					---Update trancd in temp funda

				end
				if substring(@Table_Name,3,5)='Mall'
				begin

					set @SqlCommand = 'update a set a.ac_id=c.ac_id,a.Tran_Cd = c.Tran_cd 
					from STmall_' +@fName+' a 
					inner join  stmain_'+@fName+ ' b on (a.oldTran_cd=b.oldTran_cd)
					inner join stmain c on (b.dataExport1=c.dataimport)'
					Print '15. '
					print @SqlCommand					
					EXECUTE SP_EXECUTESQL @SqlCommand
					set @SqlCommand = 'update a set a.ac_id=c.ac_id,a.Tran_cd=b.Tran_cd
					from stmall a 
					inner join stmall_'+@fName+' b on (a.dataimport=b.dataExport1)
					inner join stmain_'+@fName+' c on(b.oldTran_cd=c.OldTran_cd)'
					Print '16. '
					print @SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand
					---Update trancd in temp funda
				-----Birendra : Bug-20512 on 13/02/2014 :Start:
					set @UpdateSqlTmp='Update a set a.compid=b.compid  from '+@Table_Name+'_'+@fName+ ' a inner join vudyog..co_mast b on (a.l_yn=cast(year(b.sta_dt)as varchar(4))+''-''+cast(year(b.end_dt) as varchar(4)) and b.dbname=db_name())'		
					EXECUTE SP_EXECUTESQL @UpdateSqlTmp 
				-----Birendra : Bug-20512 on 13/02/2014 :End:
				end
				
				if substring(@Table_Name,3,5)='Itref'
				begin
					set @SqlCommand = 'update a set a.Tran_Cd = c.Tran_cd 
					from STitref_' +@fName+' a 
					inner join  stmain_'+@fName+ ' b on (a.oldTran_cd=b.oldTran_cd)
					inner join stmain c on (b.dataExport1=c.dataimport)'
					Print '15. '
					print @SqlCommand					
					EXECUTE SP_EXECUTESQL @SqlCommand
					set @SqlCommand = 'update a set a.Tran_cd=b.Tran_cd,a.it_code=i.it_code
					from stitref a 
					inner join stitref_'+@fName+' b on (a.dataimport=b.dataExport1)
					inner join stmain_'+@fName+' c on(b.oldTran_cd=c.OldTran_cd)
					inner join stitem_'+@fName+' i on(a.it_code=i.it_code)'
					Print '16. '
					print @SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand


					---Update trancd in temp funda
				end
--Commented by Archana K. on 08/10/12 for Bug-5837  Start
--				if @Table_Name='Manu_Det'
--				begin
--					set @UpdateSql='update a set a.TRAN_CD=ST.TRAN_CD
--						FROM MANU_DET a INNER JOIN STmAIN_'+@fName+' X ON (a.TRAN_CD=X.TRAN_CD)
--						INNER JOIN STMAIN ST ON (ST.DATAIMPORT=X.DATAEXPORT1)
--						where a.entry_ty=''ST'' '
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
--Commented by Archana K. on 08/10/12 for Bug-5837  end

----xxxxx Gen_SRNo Generation xxxxx----
				if substring(@Table_Name,3,4)='GEN_SRNO'
				begin
					set @SqlCommand = 'update a set  a.Tran_Cd = c.Tran_Cd 
						from gen_srno a 
						inner join STmain_' +@fName+' b on (a.Tran_cd=b.oldTran_cd)
						inner join STmain c on (b.DataExport1=c.DataImport)'
					Print '11. ' +@SqlCommand
					EXECUTE SP_EXECUTESQL @SqlCommand
				End
----xxxxx Gen_SRNo Generation xxxxx----

--Added By Kishor For Bug-26960 on 23/09/2015.. Start
												
				if @Table_Name='IT_SRTRN'
				begin										
					set @SqlCommand ='update a set  a.Tran_Cd = c.Tran_Cd 
					from it_SrTrn a
					inner join stmain_' +@fName+' b on (a.Tran_cd=b.oldTran_cd)
					inner join stmain c on (b.DataExport1=c.DataImport)'
					EXECUTE SP_EXECUTESQL @SqlCommand
					UPDATE T SET iTran_cd = S.iTran_cd,It_code=s.It_Code from IT_SRTRN T INNER JOIN  IT_SRSTK S ON  S.ImpKey=T.ImpKey
				End				
				
--Added By Kishor For Bug-26960 on 23/09/2015.. End


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
	
	set @UpdateSql='select @EdateOut=max(convert(varchar(10),date,103)) from STMAIN_'+@fName
	Print '11. '+@UpdateSql
	EXECUTE SP_EXECUTESQL @UpdateSql,N'@EdateOut varchar(10) output',@EdateOut=@Edate output
	set @UpdateSql='select @SdateOut=min(convert(varchar(10),date,103)) from STMAIN_'+@fName
	Print '12. '+@UpdateSql
	EXECUTE SP_EXECUTESQL @UpdateSql,N'@SdateOut varchar(10) output',@SdateOut=@Sdate output
	print isnull(@Sdate,'NULL')
	print isnull(@Edate,'NULL')
--	set dateformat dmy-- Commented by Archana K. on 13/05/13 for Bug-5837
	SELECT @Sdate,@Edate
	if isnull(@Sdate,'')<>'' or isnull(@Edate,'')<>''
		execute Usp_DocNo_Renumbering 'ST',@Sdate,@Edate,''
----xxxxx Doc_no Generation xxxxx----

----xxxxx Gen_Inv Updation xxxxx----	
	declare @FinYear varchar(9)
	set @UpdateSql='select distinct l_yn into ##FinYear from STMAIN_'+@fName
	Print '13. '+@UpdateSql
	EXECUTE SP_EXECUTESQL @UpdateSql
	while exists( select top 1 l_yn from ##FinYear)
	begin
		select top 1 @FinYear=l_yn from ##FinYear
		print 'Found :' + @FinYear
		execute Usp_Gen_Inv_Updation 'ST',@FinYear
		delete from ##FinYear where l_yn=@FinYear
	end
	drop table ##FinYear
----xxxxx Gen_Inv Updation xxxxx----	
end

