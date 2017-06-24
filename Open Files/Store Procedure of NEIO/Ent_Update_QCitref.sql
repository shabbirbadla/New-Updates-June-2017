IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ent_Update_QCitref]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Ent_Update_QCitref]
GO

-- =============================================
-- Author:		Pankaj B.
-- Create date: 04-02-2015
-- Description:	This procedure used to update Qc quantity to pickup transaction (Bug-25243)
--				i.e. AR->PT 
--					 QC done in AR but QC quantity not reflect in PT bcz it is already pickup AR
-- =============================================
create  procedure [dbo].[Ent_Update_QCitref]
@entry_ty varchar(2),@tran_cd int,@it_code int,@itserial varchar(6)
as
begin
PRINT @entry_ty
PRINT @tran_cd
PRINT @it_code
PRINT @itserial

	declare @test varchar(20),@POS INT,@tmp varchar(20),@bcodenm varchar(2),@ext_vou bit
	declare find_ref  cursor  for select entry_ty,bcode_nm,ext_vou from lcode where pickupfrom like '%'+@entry_ty+'%'
	open find_ref
	fetch next from find_ref into @test,@bcodenm,@ext_vou
	while @@fetch_status=0
	begin 
		select @bcodenm=(case when @bcodenm<>'' then @bcodenm else (case when @ext_vou=1 then '' else @test end)end)
		execute('update a set a.qcholdqty=b.qcholdqty,a.qcaceptqty=b.qcaceptqty,a.qcrejqty=b.qcrejqty,a.qcrturnqty=b.qcrturnqty
		,a.qcwasteqty=b.qcwasteqty,a.qcprocloss=b.qcprocloss,a.lastqc_by=b.lastqc_by,a.lastqc_dt=b.lastqc_dt,a.qcsampqty=b.qcsampqty
		from '+@bcodenm+'item a
		inner join '+@bcodenm+'itref c on (a.tran_cd=c.tran_cd AND A.ENTRY_TY=C.ENTRY_TY AND A.IT_CODE=C.IT_CODE AND A.ITSERIAL=C.ITSERIAL)
		inner join '+@entry_ty+'item b on (c.itref_tran=b.tran_cd and c.ritserial=b.itserial and c.it_code=b.it_code and c.rentry_ty=b.entry_ty)
			where b.entry_ty='''+@entry_ty+''' and b.Tran_cd='+@tran_cd+' and b.IT_CODE='+@it_code+' and b.ITSerial='''+@itserial+'''')
	fetch next from find_ref into @test,@bcodenm,@ext_vou			
	end
	close find_ref
	deallocate find_ref
end