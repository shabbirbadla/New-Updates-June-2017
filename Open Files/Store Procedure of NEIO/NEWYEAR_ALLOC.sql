If Not Exists(Select [Name] From SysObjects Where xtype='U' and [Name]='NEWYEAR_ALLOC')
Begin
CREATE TABLE [dbo].[NEWYEAR_ALLOC](
	[entry_ty] [varchar](2) ,
	[date] [datetime] ,
	[doc_no] [varchar](5) ,
	[inv_no] [varchar](15) ,
	[party_nm] [varchar](50) ,
	[new_all] [numeric](17, 2) ,
	[entry_all] [varchar](2) ,
	[inv_sr] [varchar](20) ,
	[tds] [numeric](11, 2) ,
	[disc] [numeric](11, 2) ,
	[l_yn] [varchar](9) ,
	[net_amt] [numeric](17, 2) ,
	[Main_tran] [int] NOT NULL,
	[Tran_cd] [int] ,
	[Ac_id] [int] ,
	[Date_all] [datetime] ,
	[CompId] [int] ,
	[acserial] [varchar](5) ,
	[acseri_alL] [varchar](5) ,
	[DOC_ALL] [varchar](5) 
) ON [PRIMARY]
End



