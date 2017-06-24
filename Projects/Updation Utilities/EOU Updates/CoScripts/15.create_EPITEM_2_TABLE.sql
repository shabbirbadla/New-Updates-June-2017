/****** Object:  Table [dbo].[EIITEM]    Script Date: 01/16/2010 11:08:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EIITEM]') AND type in (N'U'))
CREATE TABLE [dbo].[EIITEM](
	[newfld] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[entry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[date] [datetime] NULL,
	[doc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[itserial] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[item_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[dc_no] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[or_no] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[qt_no] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[sr_no] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[or_sr] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[dc_sr] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[qt_sr] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[sr_sr] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[item] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[qty] [numeric](13, 4) NULL,
	[rate] [numeric](9, 2) NULL,
	[gro_amt] [numeric](16, 2) NULL,
	[tax_name] [char](12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[taxamt] [numeric](10, 2) NULL,
	[examt] [numeric](11, 2) NULL,
	[dept] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[cate] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[party_nm] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[inv_sr] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[inv_no] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[l_yn] [char](9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[re_qty] [numeric](13, 4) NULL,
	[ware_nm] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[memo_op] [bit] NULL,
	[narr] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[pmkey] [varchar](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[disc] [numeric](9, 2) NULL,
	[disc_ty] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[it_key] [char](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_basduty] [numeric](4, 2) NULL,
	[u_pageno] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_pkno] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_appack] [numeric](12, 0) NULL,
	[newdocno] [char](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_lqty] [numeric](9, 3) NULL,
	[u_expdesc] [numeric](11, 3) NULL,
	[u_expmark] [char](35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_expgwt] [numeric](9, 3) NULL,
	[u_asseamt] [numeric](11, 2) NULL,
	[u_mrprate] [numeric](10, 2) NULL,
	[pbed] [numeric](9, 2) NULL,
	[ibed] [numeric](9, 2) NULL,
	[cbed] [numeric](9, 2) NULL,
	[u_cessper] [numeric](4, 2) NULL,
	[u_cessamt] [numeric](11, 2) NULL,
	[u_freight] [numeric](11, 4) NULL,
	[u_insura] [numeric](11, 4) NULL,
	[u_landper] [numeric](4, 2) NULL,
	[u_landch] [numeric](11, 2) NULL,
	[u_frper] [numeric](4, 2) NULL,
	[u_insper] [numeric](5, 3) NULL,
	[u_packchg] [numeric](12, 4) NULL,
	[u_inrval] [numeric](12, 4) NULL,
	[u_usdamt] [numeric](11, 3) NULL,
	[u_cessper1] [numeric](4, 2) NULL,
	[u_basduty1] [numeric](4, 2) NULL,
	[u_fob] [numeric](11, 2) NULL,
	[u_acduty1] [numeric](4, 2) NULL,
	[u_acamt1] [numeric](11, 2) NULL,
	[u_grosswt] [numeric](11, 3) NULL,
	[u_total] [numeric](11, 2) NULL,
	[u_shortage] [numeric](11, 3) NULL,
	[u_srno] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_forpick] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_bqty] [numeric](11, 3) NULL,
	[u_chalqty] [numeric](12, 3) NULL,
	[u_acqty] [numeric](11, 3) NULL,
	[u_rqty] [numeric](11, 3) NULL,
	[u_type] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_gwt] [numeric](11, 3) NULL,
	[u_swt] [numeric](11, 3) NULL,
	[u_bundle] [numeric](12, 0) NULL,
	[u_pieces] [numeric](12, 0) NULL,
	[u_from] [numeric](12, 0) NULL,
	[u_to] [numeric](12, 0) NULL,
	[u_itemno] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_boxwt] [numeric](11, 2) NULL,
	[u_remark] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_impper] [numeric](4, 2) NULL,
	[u_impamt] [numeric](11, 2) NULL,
	[u_impduty] [numeric](9, 2) NULL,
	[u_packing] [numeric](9, 2) NULL,
	[u_pkg1] [numeric](11, 2) NULL,
	[aapurc] [bit] NULL,
	[u_are2nodt] [datetime] NULL,
	[u_exare2] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_exare2dt] [datetime] NULL,
	[u_cessamta] [numeric](11, 2) NULL,
	[u_cessamtp] [numeric](11, 2) NULL,
	[u_cessamtc] [numeric](11, 2) NULL,
	[u_tqty] [numeric](13, 3) NULL,
	[folio_entr] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_hcesamt] [numeric](10, 2) NULL,
	[u_hcesper] [numeric](10, 2) NULL,
	[u_hacamt1] [numeric](10, 2) NULL,
	[u_hcessamt] [numeric](10, 2) NULL,
	[u_hacper] [numeric](4, 2) NULL,
	[u_hcessper] [numeric](4, 2) NULL,
	[u_beno] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_bedate] [datetime] NULL,
	[u_chapno] [char](12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[new_docno] [char](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Pdoc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Manuname] [varchar](35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[It_code] [numeric](18, 0) NULL,
	[Disc_qty] [numeric](13, 4) NULL,
	[Pdate] [datetime] NULL,
	[Ppageno] [numeric](8, 0) NULL,
	[Apgen] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Pinv_sr] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Billqty] [numeric](11, 3) NULL,
	[Chaphead] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Excbal] [numeric](11, 3) NULL,
	[Billexamt] [numeric](9, 2) NULL,
	[Ac_id] [int] NULL,
	[Pitem_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Apgenby] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Compid] [int] NULL,
	[Balqty] [numeric](13, 4) NULL,
	[Pinv_no] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Pentry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Pparty] [varchar](60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Rgpage] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Tran_cd] [int] NULL,
	[Raw_qty] [numeric](13, 4) NULL,
	[Itall_tran] [int] NULL,
	[Pitserial] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ABTPER] [numeric](10, 2) NULL,
	[tbl_id] [int] IDENTITY(1,1) NOT NULL,
	[tot_deduc] [numeric](12, 2) NULL DEFAULT ((0)),
	[tot_tax] [numeric](12, 2) NULL DEFAULT ((0)),
	[tot_examt] [numeric](12, 2) NULL DEFAULT ((0)),
	[tot_add] [numeric](12, 2) NULL DEFAULT ((0)),
	[tot_nontax] [numeric](12, 2) NULL DEFAULT ((0)),
	[tot_fdisc] [numeric](12, 2) NULL DEFAULT ((0))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIITEM_ac_mast]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIITEM]'))
ALTER TABLE [dbo].[EIITEM]  WITH NOCHECK ADD  CONSTRAINT [FK_EIITEM_ac_mast] FOREIGN KEY([Ac_id])
REFERENCES [dbo].[AC_MAST] ([Ac_id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIITEM] CHECK CONSTRAINT [FK_EIITEM_ac_mast]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIITEM_EIMAIN]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIITEM]'))
ALTER TABLE [dbo].[EIITEM]  WITH NOCHECK ADD  CONSTRAINT [FK_EIITEM_EIMAIN] FOREIGN KEY([Tran_cd])
REFERENCES [dbo].[EIMAIN] ([Tran_cd])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIITEM] CHECK CONSTRAINT [FK_EIITEM_EIMAIN]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIITEM_it_mast]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIITEM]'))
ALTER TABLE [dbo].[EIITEM]  WITH NOCHECK ADD  CONSTRAINT [FK_EIITEM_it_mast] FOREIGN KEY([It_code])
REFERENCES [dbo].[IT_MAST] ([It_code])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIITEM] CHECK CONSTRAINT [FK_EIITEM_it_mast]