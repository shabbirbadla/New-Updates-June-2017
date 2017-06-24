IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_DataExport_SO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Usp_DataExport_SO]
Go


set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


Create Procedure [dbo].[Usp_DataExport_SO]
@CommanPara varchar(4000)
as
Begin
	Declare @TablNm varchar(60),@FileType varchar(3),@ExpCode varchar(200),@ExpDataVol varchar(30),@DtFld varchar(30),@sDate Varchar(30),@eDate Varchar(30)
	Declare @TempTbl varchar(100)

	Set @TempTbl = '##DATAImp'+(SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	
	execute Usp_GetCommanParaVal @CommanPara,'TablNm',@TablNm out
	execute Usp_GetCommanParaVal @CommanPara,'FileType',@FileType out
	execute Usp_GetCommanParaVal @CommanPara,'ExpCode',@ExpCode out
	execute Usp_GetCommanParaVal @CommanPara,'ExpDataVol',@ExpDataVol  out
	execute Usp_GetCommanParaVal @CommanPara,'DtFld',@DtFld out
	execute Usp_GetCommanParaVal @CommanPara,'sDate',@sDate out
	execute Usp_GetCommanParaVal @CommanPara,'eDate',@eDate out
	
	 --select @TablNm ,@FileType ,@ExpCode ,@ExpDataVol,@DtFld ,@sDate ,@eDate 

	--execute Usp_GetCommanParaVal @CommanPara,'TablNm',@TablNm out
		
	Declare @SqlCommand nvarchar(4000)
	Declare @Manu_Det_Cmd nvarchar(4000)
	set @ExpCode=replace(@ExpCode,'`','''')
--Commented by Archana K. on 29/09/12 for Bug-5837 start
--	if  @TablNm='LITEMALL'
--	begin
--		if @FileType='xsd'
--		begin
--			set @SqlCommand='Select (SELECT Dataexport1='+@ExpCode
----			set @SqlCommand=rtrim(@SqlCommand)+',manuAc_Name=b.Ac_Name,manuSAc_Name=c.Location_Id'
--			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,* FROM '+@TablNm
----			set @SqlCommand=rtrim(@SqlCommand)+' Left join ac_mast b on (a.ManuAc_ID=b.ac_id) left join shipto c on (a.ManuSAc_Id=c.Shipto_Id) where 1=2'
--			set @SqlCommand=rtrim(@SqlCommand)+' where 1=2 FOR XML auto,XMLSCHEMA,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
--
--			print '1.'
--			print @SqlCommand
--			execute sp_executesql @SqlCommand
----			set @SqlCommand='Select (select * from 	'+@TempTbl+' manu_det'	
----			set @SqlCommand=rtrim(@SqlCommand)+' where 1=2 FOR XML auto,XMLSCHEMA,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
----			execute sp_executesql @SqlCommand
----			print '2.'
----			print @SqlCommand
--		end
--		if @FileType='xml'
--		begin
----####### date field condition
--			if isnull(@DtFld,'')<>''
--			begin
--				set @ExpCode=replace(@ExpCode,'Tran_cd','a.Tran_cd')
--				set @ExpCode=replace(@ExpCode,'Entry_ty','a.Entry_ty')
--				print 'Exp Code : '+@ExpCode
--				set @SqlCommand='SELECT Dataexport1='+@ExpCode
--				set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=a.Tran_cd,a.* into '+@TempTbl+' FROM  '+@TablNm+' a'
--				set @SqlCommand=rtrim(@SqlCommand)+' inner join somain b on (a.tran_cd=b.Tran_cd)'
--				set @SqlCommand=rtrim(@SqlCommand)+'  where '
--
--				if @ExpDataVol='Updated' 
--				begin
--					set @SqlCommand=rtrim(@SqlCommand)+' isnull(a.DataExport,'''')='''' '
--				end				
--				--*** date field condition
--					set @SqlCommand = rtrim(@SqlCommand)+ ' and (b.'+@DtFld+' between '+char(39)+@sDate+Char(39)+' and '+char(39)+@eDate+Char(39)+')'
--				--*** 
--				set @SqlCommand = rtrim(@SqlCommand)+ ' and (a.Entry_ty in (''SO''))'
--				print '3.a'
--				print @SqlCommand
--
--			execute sp_executesql @SqlCommand
--			set @SqlCommand='Select (select * from 	'+@TempTbl+' litemall'
--			end 
----#######  date field condition
--			else
--			begin
--				set @SqlCommand='Select (SELECT Dataexport1='+@ExpCode
--	--			set @SqlCommand=rtrim(@SqlCommand)+',manuAc_Name=b.Ac_Name,manuSAc_Name=c.Location_Id '
--				set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,* FROM  '+@TablNm
--	--			set @SqlCommand=rtrim(@SqlCommand)+' Left join ac_mast b on (a.ManuAc_ID=b.ac_id) left join shipto c on (a.ManuSAc_Id=c.Shipto_Id)'
--				set @SqlCommand=rtrim(@SqlCommand)+'  where '
--
--				if @ExpDataVol='Updated' 
--				begin
--					set @SqlCommand=rtrim(@SqlCommand)+' isnull(DataExport,'''')='''' '
--				end
--				
--				set @SqlCommand = rtrim(@SqlCommand)+ ' and (Entry_ty in (''SO''))'
--				print '3.b'
--				print @SqlCommand
--			end 
--			
--
----			execute sp_executesql @SqlCommand
----			set @SqlCommand='Select (select * from 	'+@TempTbl+' manu_det'
--			set @SqlCommand=rtrim(@SqlCommand)+' FOR XML auto,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
--			print '4.'
--			print @SqlCommand
--			execute sp_executesql @SqlCommand
--		end
--	end		
--
--Commented by Archana K. on 29/09/12 for Bug-5837 end

	if @TablNm='SOMAIN' 
	begin
		print 'b'
--Commented by Archana K. on 31/05/13 for Bug-5837 start
--		if @FileType='xsd'
--		begin
--			set @SqlCommand='SELECT Dataexport1='+@ExpCode
--			set @SqlCommand=rtrim(@SqlCommand)+',cons_Name=b.Ac_Name,Scons_Name=c.Location_Id,sAc_Name=d.Location_id '
--			--set @SqlCommand=rtrim(@SqlCommand)+',MANUAC_Name=E.Ac_Name,MANUSAC_Name=F.Location_Id '
--			--set @SqlCommand=rtrim(@SqlCommand)+',SUPPAC_Name=G.Ac_Name,SUPPSAC_Name=H.Location_Id '
----			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+' FROM '+@TablNm+' a'--Commented by Archana K. on 09/04/13 for Bug-5837
--			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+@TablNm+' FROM '+@TablNm+' a'--Changed by Archana K. on 09/04/13 for Bug-5837
--			set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast b on (a.cons_id=b.ac_id)'
--			set @SqlCommand=rtrim(@SqlCommand)+' left join shipto c on (a.scons_id=c.shipto_id)'
--			set @SqlCommand=rtrim(@SqlCommand)+' left join shipto d on (a.scons_id=d.shipto_id)'
--			--set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast E on (a.MANUAC_id=E.ac_id)'
--			--set @SqlCommand=rtrim(@SqlCommand)+' left join shipto F on (a.MANUSAC_id=F.shipto_id)'
--			--set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast G on (a.SUPPAC_id=G.ac_id)'
--			--set @SqlCommand=rtrim(@SqlCommand)+' left join shipto H on (a.SUPPSAC_id=H.shipto_id) where 1=2'
----			print '5.'
----			print @SqlCommand
----			execute sp_executesql @SqlCommand
--			print '6.'
--			print @SqlCommand
--			execute sp_executesql @SqlCommand
--			set @SqlCommand='Select (select * from '+@TempTbl+@TablNm+' as '+@TablNm
--			set @SqlCommand=rtrim(@SqlCommand)+' where 1=2 FOR XML auto,XMLSCHEMA,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
--			print '7.'
--			print @SqlCommand
--			execute sp_executesql @SqlCommand
--		end
--Commented by Archana K. on 31/05/13 for Bug-5837 end
		if @FileType='xml'
		begin
			set @SqlCommand='SELECT Dataexport1='+@ExpCode
			set @SqlCommand=rtrim(@SqlCommand)+',cons_Name=b.Ac_Name,Scons_Name=c.Location_Id,sAc_Name=d.Location_id '
			--set @SqlCommand=rtrim(@SqlCommand)+',MANUAC_Name=E.Ac_Name,MANUSAC_Name=F.Location_Id '
			--set @SqlCommand=rtrim(@SqlCommand)+',SUPPAC_Name=G.Ac_Name,SUPPSAC_Name=H.Location_Id '
--			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+' FROM '+@TablNm+' a'--Commented by Archana K. on 09/04/13 for Bug-5837
			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+@TablNm+' FROM '+@TablNm+' a'--Changed by Archana K. on 09/04/13 for Bug-5837
			set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast b on (a.cons_id=b.ac_id)'
			set @SqlCommand=rtrim(@SqlCommand)+' left join shipto c on (a.scons_id=c.shipto_id)'
			set @SqlCommand=rtrim(@SqlCommand)+' left join shipto d on (a.scons_id=d.shipto_id)'
			--set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast E on (a.MANUAC_id=E.ac_id)'
			--set @SqlCommand=rtrim(@SqlCommand)+' left join shipto F on (a.MANUSAC_id=F.shipto_id)'
			--set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast G on (a.SUPPAC_id=G.ac_id)'
			--set @SqlCommand=rtrim(@SqlCommand)+' left join shipto H on (a.SUPPSAC_id=H.shipto_id)'
			
			set @SqlCommand=rtrim(@SqlCommand)+'  where '
			
			if @ExpDataVol='Updated' 
			begin
				set @SqlCommand=rtrim(@SqlCommand)+' isnull(a.DataExport,'''')='''' '
			end
			else
			begin
				set @SqlCommand=rtrim(@SqlCommand)+' 1=1'
			end
			if isnull(@DtFld,'')<>''
			begin
				set @SqlCommand = rtrim(@SqlCommand)+ ' and ('+@DtFld+' between '+char(39)+@sDate+Char(39)+' and '+char(39)+@eDate+Char(39)+')'
			end				
		end
		print '8.'
		print @SqlCommand

		execute sp_executesql @SqlCommand
		set @SqlCommand='Select (select * from '+@TempTbl+@TablNm+' as '+@TablNm
		set @SqlCommand=rtrim(@SqlCommand)+' FOR XML auto,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
		print '9.'
		print @SqlCommand
		execute sp_executesql @SqlCommand
	end
----****
	if @TablNm='SOITEM' 
	begin
		print 'b'
--Commented by Archana K. on 31/05/13 for Bug-5837 start
--		if @FileType='xsd'
--		begin
--			set @SqlCommand='SELECT Dataexport1='+@ExpCode
--			set @SqlCommand=rtrim(@SqlCommand)+',Ac_Name=b.Ac_Name'
----			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+' FROM '+@TablNm+' a'--Commented by Archana K. on 09/04/13 for Bug-5837
--			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+@TablNm+' FROM '+@TablNm+' a'--Changed by Archana K. on 09/04/13 for Bug-5837
--			set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast b on (a.Ac_ID=b.ac_id) where 1=2'
--
--			print '6.'
--			print @SqlCommand
--			execute sp_executesql @SqlCommand
--			set @SqlCommand='Select (select * from '+@TempTbl+@TablNm+' as '+@TablNm
--			set @SqlCommand=rtrim(@SqlCommand)+' where 1=2 FOR XML auto,XMLSCHEMA,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
--			print '7.'
--			print @SqlCommand
--			execute sp_executesql @SqlCommand
--		end
--Commented by Archana K. on 31/05/13 for Bug-5837 end
		if @FileType='xml'
		begin
			set @SqlCommand='SELECT Dataexport1='+@ExpCode
			set @SqlCommand=rtrim(@SqlCommand)+',Ac_Name=b.Ac_Name'
--			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+' FROM '+@TablNm+' a'--Commented by Archana K. on 09/04/13 for Bug-5837
			set @SqlCommand=rtrim(@SqlCommand)+',oldTran_cd=Tran_cd,a.* into '+@TempTbl+@TablNm+' FROM '+@TablNm+' a'--Changed by Archana K. on 09/04/13 for Bug-5837
			set @SqlCommand=rtrim(@SqlCommand)+' left join ac_mast b on (a.Ac_ID=b.ac_id)'
			
			set @SqlCommand=rtrim(@SqlCommand)+'  where '
			
			if @ExpDataVol='Updated' 
			begin
				set @SqlCommand=rtrim(@SqlCommand)+' isnull(a.DataExport,'''')='''' '
			end
			else
			begin
				set @SqlCommand=rtrim(@SqlCommand)+' 1=1'
			end
			if isnull(@DtFld,'')<>''
			begin
				set @SqlCommand = rtrim(@SqlCommand)+ ' and ('+@DtFld+' between '+char(39)+@sDate+Char(39)+' and '+char(39)+@eDate+Char(39)+')'
			end				
		end
		print '8.'
		print @SqlCommand

		execute sp_executesql @SqlCommand
		set @SqlCommand='Select (select * from '+@TempTbl+@TablNm+' as '+@TablNm
		set @SqlCommand=rtrim(@SqlCommand)+' FOR XML auto,ROOT('+char(39)+@TablNm+char(39)+'))  as cxml'
		print '9.'
		print @SqlCommand
		execute sp_executesql @SqlCommand
	end
----Added by Archana K. on 12/04/13 for Bug-5837 start
	SET @SQLCOMMAND = 'DROP TABLE '+@TempTbl+@TablNm
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
----Added by Archana K. on 12/04/13 for Bug-5837 end
----****
end
--	



