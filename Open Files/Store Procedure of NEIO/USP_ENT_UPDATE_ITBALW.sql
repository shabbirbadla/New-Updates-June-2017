
cREATE Procedure USP_ENT_UPDATE_ITBALW
AS

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IT_BALW_OLD]') AND type in (N'U'))
Begin
	DROP TABLE [dbo].[IT_BALW_OLD]
	Print 'drop IT_BALW_OLD'
End
SELECT * INTO IT_BALW_OLD FROM IT_BALW
Print 'Create IT_BALW_OLD'

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IT_BALW]') AND type in (N'U'))
Begin
	DROP TABLE [dbo].[IT_BALW]	
End


CREATE TABLE [dbo].[IT_BALW](
	[IT_CODE] [numeric](18, 0) NOT NULL,
	[WARE_NM] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ENTRY_TY] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DATE] [datetime] NOT NULL,
	[QTY] [numeric](38, 6) NULL,
	[RULE] [varchar](20) NULL
) ON [PRIMARY]

-----------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IT_BAL]') AND name = N'IT_CODE')
DROP INDEX [IT_CODE] ON [dbo].[IT_BAL] WITH ( ONLINE = OFF )


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IT_BALW]') AND name = N'ITCDWED')
DROP INDEX [ITCDWED] ON [dbo].[IT_BALW] WITH ( ONLINE = OFF )


CREATE NONCLUSTERED INDEX [IT_CODE] ON [dbo].[It_Bal] 
(
	[IT_CODE] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ITCDWED] ON [dbo].[IT_BALW] 
(
	[IT_CODE] ASC,
	[WARE_NM] ASC,
	[ENTRY_TY] ASC,
	[DATE] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]


select aritem.it_code,aritem.ware_nm,aritem.entry_ty,aritem.date,aritem.qty,armain.[rule] into #temp1 from aritem left join armain on armain.tran_cd=aritem.tran_cd where aritem.dc_no = ' ' and ((aritem.date >= '2007-04-01 00:00:00.000') or (aritem.date < '2007-04-01 00:00:00.000' and aritem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select bpitem.it_code,bpitem.ware_nm,bpitem.entry_ty,bpitem.date,bpitem.qty,BPMAIN.[Rule] from bpitem left join bpmain on bpmain.tran_cd=bpitem.tran_cd where bpitem.dc_no = ' ' and ((bpitem.date >= '2007-04-01 00:00:00.000') or (bpitem.date < '2007-04-01 00:00:00.000' and bpitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select britem.it_code,britem.ware_nm,britem.entry_ty,britem.date,britem.qty,BRMAIN.[Rule] from britem left join brmain on brmain.tran_cd=britem.tran_cd where britem.dc_no = ' ' and ((britem.date >= '2007-04-01 00:00:00.000') or (britem.date < '2007-04-01 00:00:00.000' and britem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select cnitem.it_code,cnitem.ware_nm,cnitem.entry_ty,cnitem.date,cnitem.qty,CNMAIN.[Rule] from cnitem left join cnmain on cnmain.tran_cd=cnitem.tran_cd where cnitem.dc_no = ' ' and ((cnitem.date >= '2007-04-01 00:00:00.000') or (cnitem.date < '2007-04-01 00:00:00.000' and cnitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select cpitem.it_code,cpitem.ware_nm,cpitem.entry_ty,cpitem.date,cpitem.qty,CPMAIN.[Rule] from cpitem left join cpmain on cpmain.tran_cd=cpitem.tran_cd where cpitem.dc_no = ' ' and ((cpitem.date >= '2007-04-01 00:00:00.000') or (cpitem.date < '2007-04-01 00:00:00.000' and cpitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select critem.it_code,critem.ware_nm,critem.entry_ty,critem.date,critem.qty,CRMAIN.[Rule] from critem left join crmain on crmain.tran_cd=critem.tran_cd where critem.dc_no = ' ' and ((critem.date >= '2007-04-01 00:00:00.000') or (critem.date < '2007-04-01 00:00:00.000' and critem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select dcitem.it_code,dcitem.ware_nm,dcitem.entry_ty,dcitem.date,dcitem.qty,DCMAIN.[Rule] from dcitem left join dcmain on dcmain.tran_cd=dcitem.tran_cd where dcitem.dc_no = ' ' and ((dcitem.date >= '2007-04-01 00:00:00.000') or (dcitem.date < '2007-04-01 00:00:00.000' and dcitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select dnitem.it_code,dnitem.ware_nm,dnitem.entry_ty,dnitem.date,dnitem.qty,DNMAIN.[Rule] from dnitem left join dnmain on dnmain.tran_cd=dnitem.tran_cd where dnitem.dc_no = ' ' and ((dnitem.date >= '2007-04-01 00:00:00.000') or (dnitem.date < '2007-04-01 00:00:00.000' and dnitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select epitem.it_code,epitem.ware_nm,epitem.entry_ty,epitem.date,epitem.qty,EPMAIN.[Rule] from epitem left join epmain on epmain.tran_cd=epitem.tran_cd where epitem.dc_no = ' ' and ((epitem.date >= '2007-04-01 00:00:00.000') or (epitem.date < '2007-04-01 00:00:00.000' and epitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select esitem.it_code,esitem.ware_nm,esitem.entry_ty,esitem.date,esitem.qty,ESMAIN.[Rule] from esitem left join esmain on esmain.tran_cd=esitem.tran_cd where esitem.dc_no = ' ' and ((esitem.date >= '2007-04-01 00:00:00.000') or (esitem.date < '2007-04-01 00:00:00.000' and esitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select iiitem.it_code,iiitem.ware_nm,iiitem.entry_ty,iiitem.date,iiitem.qty,IIMAIN.[Rule] from iiitem left join iimain on iimain.tran_cd=iiitem.tran_cd where iiitem.dc_no = ' ' and ((iiitem.date >= '2007-04-01 00:00:00.000') or (iiitem.date < '2007-04-01 00:00:00.000' and iiitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select ipitem.it_code,ipitem.ware_nm,ipitem.entry_ty,ipitem.date,ipitem.qty,IPMAIN.[Rule] from ipitem left join ipmain on ipmain.tran_cd=ipitem.tran_cd where ipitem.dc_no = ' ' and ((ipitem.date >= '2007-04-01 00:00:00.000') or (ipitem.date < '2007-04-01 00:00:00.000' and ipitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select iritem.it_code,iritem.ware_nm,iritem.entry_ty,iritem.date,iritem.qty,IRMAIN.[Rule] from iritem left join irmain on irmain.tran_cd=iritem.tran_cd where iritem.dc_no = ' ' and ((iritem.date >= '2007-04-01 00:00:00.000') or (iritem.date < '2007-04-01 00:00:00.000' and iritem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select jvitem.it_code,jvitem.ware_nm,jvitem.entry_ty,jvitem.date,jvitem.qty,JVMAIN.[Rule] from jvitem left join jvmain on jvmain.tran_cd=jvitem.tran_cd where jvitem.dc_no = ' ' and ((jvitem.date >= '2007-04-01 00:00:00.000') or (jvitem.date < '2007-04-01 00:00:00.000' and jvitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select obitem.it_code,obitem.ware_nm,obitem.entry_ty,obitem.date,obitem.qty,OBMAIN.[Rule] from obitem left join obmain on obmain.tran_cd=obitem.tran_cd where obitem.dc_no = ' ' and ((obitem.date >= '2007-04-01 00:00:00.000') or (obitem.date < '2007-04-01 00:00:00.000' and obitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select opitem.it_code,opitem.ware_nm,opitem.entry_ty,opitem.date,opitem.qty,OPMAIN.[Rule] from opitem left join opmain on opmain.tran_cd=opitem.tran_cd where opitem.dc_no = ' ' and ((opitem.date >= '2007-04-01 00:00:00.000') or (opitem.date < '2007-04-01 00:00:00.000' and opitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select ositem.it_code,ositem.ware_nm,ositem.entry_ty,ositem.date,ositem.qty,OSMAIN.[Rule] from ositem left join osmain on osmain.tran_cd=ositem.tran_cd where ositem.dc_no = ' ' and ((ositem.date >= '2007-04-01 00:00:00.000') or (ositem.date < '2007-04-01 00:00:00.000' and ositem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select pcitem.it_code,pcitem.ware_nm,pcitem.entry_ty,pcitem.date,pcitem.qty,PCMAIN.[Rule] from pcitem left join pcmain on pcmain.tran_cd=pcitem.tran_cd where pcitem.dc_no = ' ' and ((pcitem.date >= '2007-04-01 00:00:00.000') or (pcitem.date < '2007-04-01 00:00:00.000' and pcitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select poitem.it_code,poitem.ware_nm,poitem.entry_ty,poitem.date,poitem.qty,POMAIN.[Rule] from poitem left join pomain on pomain.tran_cd=poitem.tran_cd where poitem.dc_no = ' ' and ((poitem.date >= '2007-04-01 00:00:00.000') or (poitem.date < '2007-04-01 00:00:00.000' and poitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select pritem.it_code,pritem.ware_nm,pritem.entry_ty,pritem.date,pritem.qty,PRMAIN.[Rule] from pritem left join prmain on prmain.tran_cd=pritem.tran_cd where pritem.dc_no = ' ' and ((pritem.date >= '2007-04-01 00:00:00.000') or (pritem.date < '2007-04-01 00:00:00.000' and pritem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select ptitem.it_code,ptitem.ware_nm,ptitem.entry_ty,ptitem.date,ptitem.qty,PTMAIN.[Rule] from ptitem left join ptmain on ptmain.tran_cd=ptitem.tran_cd where ptitem.dc_no = ' ' and ((ptitem.date >= '2007-04-01 00:00:00.000') or (ptitem.date < '2007-04-01 00:00:00.000' and ptitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select soitem.it_code,soitem.ware_nm,soitem.entry_ty,soitem.date,soitem.qty,SOMAIN.[Rule] from soitem left join somain on somain.tran_cd=soitem.tran_cd where soitem.dc_no = ' ' and ((soitem.date >= '2007-04-01 00:00:00.000') or (soitem.date < '2007-04-01 00:00:00.000' and soitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select sqitem.it_code,sqitem.ware_nm,sqitem.entry_ty,sqitem.date,sqitem.qty,SQMAIN.[Rule] from sqitem left join sqmain on sqmain.tran_cd=sqitem.tran_cd where sqitem.dc_no = ' ' and ((sqitem.date >= '2007-04-01 00:00:00.000') or (sqitem.date < '2007-04-01 00:00:00.000' and sqitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select sritem.it_code,sritem.ware_nm,sritem.entry_ty,sritem.date,sritem.qty,SRMAIN.[Rule] from sritem left join srmain on srmain.tran_cd=sritem.tran_cd where sritem.dc_no = ' ' and ((sritem.date >= '2007-04-01 00:00:00.000') or (sritem.date < '2007-04-01 00:00:00.000' and sritem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select ssitem.it_code,ssitem.ware_nm,ssitem.entry_ty,ssitem.date,ssitem.qty,SSMAIN.[Rule] from ssitem left join ssmain on ssmain.tran_cd=ssitem.tran_cd where ssitem.dc_no = ' ' and ((ssitem.date >= '2007-04-01 00:00:00.000') or (ssitem.date < '2007-04-01 00:00:00.000' and ssitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select stitem.it_code,stitem.ware_nm,stitem.entry_ty,stitem.date,stitem.qty,STMAIN.[Rule] from stitem left join stmain on stmain.tran_cd=stitem.tran_cd where stitem.dc_no = ' ' and ((stitem.date >= '2007-04-01 00:00:00.000') or (stitem.date < '2007-04-01 00:00:00.000' and stitem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select tritem.it_code,tritem.ware_nm,tritem.entry_ty,tritem.date,tritem.qty,TRMAIN.[Rule] from tritem left join trmain on trmain.tran_cd=tritem.tran_cd where tritem.dc_no = ' ' and ((tritem.date >= '2007-04-01 00:00:00.000') or (tritem.date < '2007-04-01 00:00:00.000' and tritem.it_code not in (select ositem.it_code from ositem group by ositem.it_code)))
union all select it_code,ware_nm,entry_ty,date,qty,''as [Rule] from item where dc_no = ' ' and ((date >= '2007-04-01 00:00:00.000') or (date < '2007-04-01 00:00:00.000' and it_code not in (select it_code from ositem group by it_code)))

 
insert into it_balw (it_code,ware_nm,entry_Ty,date,qty,[RULE]) select it_code,ware_nm,entry_Ty,date,sum(qty) as qty,[RULE] from #temp1 group by it_code,ware_nm,entry_Ty,date,[RULE]


Drop Table #temp1

--Select * from it_balw

