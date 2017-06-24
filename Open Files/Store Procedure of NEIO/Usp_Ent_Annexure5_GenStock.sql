CREATE Procedure [dbo].[Usp_Ent_Annexure5_GenStock]
@Edate SmallDateTime, 
@spl_cond Varchar(1000)
as 
set Nocount On
Declare @sqlcmd NVarchar(4000) 
Select a.entry_ty,a.Date,a.[rule] as rules,a.Tran_cd
,b.Qty,b.pmkey,Ware_nm
,i.It_code,i.It_name,i.[Type] as ItType,i.[group] as itgrp,i.itgrid,UpdateFlg=convert(bit,0),a.u_gcssr,b.Rate,it_desc=convert(Varchar(500),i.it_Desc)
Into #tmpStock from Stkl_vw_Item b
Inner Join Stkl_vw_Main a on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd)
Inner Join It_Mast i on (i.It_code=b.It_code)
Where a.Date <= @Edate  
and 1=2

set @sqlcmd='Insert Into #tmpStock'
set @sqlcmd=@sqlcmd+' '+'Select a.entry_ty,a.Date,case when a.[rule]=''NON-MODVATABLE'' THEN a.[rule] ELSE (Case when a.[rule]='''' then '''' else ''ANNEXURE V'' end) END ,a.Tran_cd,b.Qty,b.pmkey,Ware_nm'
set @sqlcmd=@sqlcmd+' '+',i.It_code,i.It_name,i.[Type],i.[group] ,i.itgrid,convert(bit,0),a.u_gcssr,0,convert(Varchar(500),i.it_Desc)'
set @sqlcmd=@sqlcmd+' '+'from Stkl_vw_Item b '
set @sqlcmd=@sqlcmd+' '+'Inner Join Stkl_vw_Main a on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd)'
set @sqlcmd=@sqlcmd+' '+'Inner Join It_Mast i on (i.It_code=b.It_code)'
set @sqlcmd=@sqlcmd+' '+'Where a.Date <= '''+convert(Varchar(50),@Edate)+''' and a.[rule] =''ANNEXURE V'''
set @sqlcmd=@sqlcmd+' '+@spl_cond 
SET @sqlcmd=RTRIM(@sqlcmd)+' '+' AND a.APGEN='+CHAR(39)+'YES'+CHAR(39)
SET @sqlcmd=RTRIM(@sqlcmd)+' '+' AND b.DC_NO='+CHAR(39)+' '+CHAR(39)
set @sqlcmd=@sqlcmd+' '+'Order By a.[Rule],i.[Type],i.It_name,b.Ware_nm'
Execute sp_Executesql @sqlcmd

--Select * from #tmpStock 

Select BalQty=sum(case when pmkey='+' then Qty else -Qty end )
,Rate,rules,It_code,It_Name,ItType,Ware_nm,itgrid,itgrp,UpdateFlg,u_gcssr Into #tmpStock2 from #tmpStock 
Group By rules,It_code,It_Name,ItType,Ware_nm,itgrid,itgrp,UpdateFlg,u_gcssr,Rate
Having sum(case when pmkey='+' then Qty else -Qty end )<>0
Order by rules,u_gcssr,ItType,It_Name,Ware_nm

Update #tmpStock2 
set balqty=(Case when abs(a.balqty)<=abs(b.balqty) then a.balqty+(a.balqty * -1) else a.balqty+(b.balqty * -1) end) from #tmpStock2 a 
inner join (select * from #tmpStock2  where rules ='') b on  a.it_code=b.it_code
where a.rules in ('','NON-MODVATABLE')

delete from #tmpStock2 where rules='' and balqty=0

Update #tmpStock2 
set balqty=(Case when abs(a.balqty)<=abs(b.balqty) then a.balqty+(a.balqty * -1) else a.balqty+(b.balqty * -1) end) from #tmpStock2 a 
inner join (select * from #tmpStock2  where rules ='') b on  a.it_code=b.it_code
where a.rules in ('','ANNEXURE V')

delete from #tmpStock2 where rules='' and balqty=0

select * from #tmpStock2  where balqty<>0

Drop Table #tmpStock 
Drop Table #tmpStock2





