set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 14/05/2009
-- Description:	This Stored procedure is useful in Bills on Hold Module.
-- Modify date:By:Reason: 02/02/2010 Rupesh Prajapati. for Query L2S-110
-- Modify date:By:Reason: 22/03/2010 Rupesh Prajapati. for Query RND-30. Remove records for Parties on Hold.
-- Modify date:By:Reason: 05/07/2010 Shrikant S. for TKT-2780
-- Remark:
-- =============================================
ALTER PROCEDURE [dbo].[USP_ENT_BILLSONHOLD_PRINT]  
@ac_name varchar(100),@date datetime,@opt varchar(10)
AS
declare @mCondn nvarchar(100)
declare @sqlcommand nvarchar(4000)


Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
DECLARE @GRPID AS INT,@MCOND AS BIT,@LVL  AS INT,@GRP AS VARCHAR(100)


SET @GRP='SUNDRY CREDITORS'

DECLARE openingentry_cursor CURSOR FOR
	SELECT entry_ty FROM lcode
	WHERE bcode_nm = 'OB'
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @opentries
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   Set @OPENTRY_TY = @OPENTRY_TY +','+CHAR(39)+@opentries+CHAR(39)
	   FETCH NEXT FROM openingentry_cursor into @opentries
	END
	CLOSE openingentry_cursor
	DEALLOCATE openingentry_cursor

CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
SET @LVL=0
INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME=@GRP
SET @MCOND=1
WHILE @MCOND=1
BEGIN
	IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
	BEGIN
		PRINT @LVL
		INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
		SET @LVL=@LVL+1
	END
	ELSE
	BEGIN
		SET @MCOND=0	
	END
END
SELECT AC_ID,AC_NAME INTO #ACMAST FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)

select ac_name,ac_id  into #partyonhold from partyonhold where  (reldt>getdate() or  (year(reldt)<=1900) )

--select * from #partyonhold

if (isnull(@ac_name,'')='PARTY') --Only Party Name Popup
begin
	set @sqlcommand=' SELECT '
	set @sqlcommand=rtrim(@sqlcommand)+' '+' distinct a.ac_name'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' FROM LAC_VW AC '
	set @sqlcommand=rtrim(@sqlcommand)+' '+' INNER JOIN AC_MAST A ON (A.AC_ID=AC.AC_ID)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' INNER JOIN #ACMAST A1 ON (A1.AC_ID=AC.AC_ID)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join lcode l on (ac.entry_ty=l.entry_ty )'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' INNER JOIN lmain_vw m  ON (M.ENTRY_TY=AC.ENTRY_TY AND M.TRAN_CD=AC.TRAN_CD)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' left join billsonhold h on (m.entry_ty=h.entry_ty and m.tran_cd=h.tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' left join #partyonhold ph on (a.ac_id=ph.ac_id) ' /*RND-30 to remove records for which parties are on Hold*/
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' left join #partyonhold ph on (m.ac_id=ph.ac_id) and '
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' not (ph.reldt>='+char(39)+cast(getdate() as varchar)+char(39)+')'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' where ( (l.bcode_nm in (''PT'',''EP'',''CR'')) or (l.entry_ty in (''PT'',''EP'',''CR''))  )'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ac.amt_ty=''CR'' and ac.amount-ac.re_all>0'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and isnull(ph.ac_id,0)=0'/*RND-30 to remove records for which parties are on Hold*/
	print @sqlcommand
end
else
begin
	set @sqlcommand=' SELECT '
	set @sqlcommand=rtrim(@sqlcommand)+' '+' a.ac_name,sel=cast ((case when isnull(h.tran_cd,0)=0 then 0 else 1 end) as bit)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,m.entry_ty,l.code_nm,m.inv_no,m.date,m.u_pinvno,m.u_pinvdt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,ac.amount,balamt=ac.amount-ac.re_all'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,hamount=isnull(h.hamount,0),hresn=isnull(h.hresn,''''),holdby=isnull(h.holdby,''''),holddt=isnull(h.holddt,'''')'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,relby=isnull(h.relby,''''),rresn=isnull(h.rresn,''''),reldt=isnull(h.reldt,'''')'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,username=isnull(h.username,''''),sysdate=isnull(h.sysdate,''''),editby=isnull(h.editby,''''),editdate=isnull(h.editdate,'''')'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,m.inv_sr,ac.acserial,m.tran_cd'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,phold=isnull(ph.ac_id,0)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,sel1=(case when isnull(h.tran_cd,0)=0 then 0 else 1 end)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' FROM LAC_VW AC '
	set @sqlcommand=rtrim(@sqlcommand)+' '+' INNER JOIN AC_MAST A ON (A.AC_ID=AC.AC_ID)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' INNER JOIN #ACMAST A1 ON (A1.AC_ID=AC.AC_ID)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join lcode l on (ac.entry_ty=l.entry_ty )'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' INNER JOIN lmain_vw m  ON (M.ENTRY_TY=AC.ENTRY_TY AND M.TRAN_CD=AC.TRAN_CD)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' left join billsonhold h on (m.entry_ty=h.entry_ty and m.tran_cd=h.tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' left join #partyonhold ph on (a.ac_id=ph.ac_id) '
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' and not (ph.reldt>='+char(39)+cast(getdate() as varchar)+char(39)+')'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' where ( (l.bcode_nm in (''PT'',''EP'',''CR'')) or (l.entry_ty in (''PT'',''EP'',''CR''))  )'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ac.amt_ty=''CR'' and ac.amount-ac.re_all>0'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and isnull(ph.ac_id,0)=0'/*RND-30 to remove records for which parties are on Hold*/
end

if (isnull(@ac_name,'')<>'' and isnull(@ac_name,'')<>'PARTY')
begin
	--	set @sqlcommand=rtrim(@sqlcommand)+' '+' and a.ac_name='''+rtrim(@ac_name)+''''		--Commented by Shrikant S. on 05/07/2010 for TKT-2780
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and a.ac_name='''+Case when charindex('''',rtrim(@ac_name))>0 then replace(@ac_name,'''','''''')else rtrim(@ac_name) end+''''	--Changed by Shrikant S. on 05/07/2010 for TKT-2780
end 
if (upper(isnull(@opt,'ALL'))='RELEASED')
begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and (h.reldt<='+char(39)+cast(getdate() as varchar)+char(39)+' and year(h.reldt)>1900) '
	
end 
if (upper(isnull(@opt,'ALL'))='HOLD')
begin
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' and b.reldt<'+char(39)+cast(@date as varchar)+char(39)
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' and isnull(h.reldt,'''')='''' and isnull(h.holddt,'''')<>'''' '
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and isnull(h.holddt,'''')>='+char(39)+cast(@date as varchar)+char(39) /*It is should be in common*/
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and ( '+' h.reldt>'+char(39)+cast(getdate() as varchar)+char(39)+' or year(h.reldt)<=1900 '+' )'
end 
if (upper(isnull(@opt,'ALL'))='BLANK')
begin
	--set @sqlcommand=rtrim(@sqlcommand)+' '+' and b.reldt<'+char(39)+cast(@date as varchar)+char(39)
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and 1=2'
end 


if (isnull(@ac_name,'')<>'PARTY') --Only Party Name Popup
begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' order by ac.date'
end
print @sqlcommand
execute sp_executesql @sqlcommand

