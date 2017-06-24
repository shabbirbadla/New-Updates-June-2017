/****** Object:  Table [dbo].[EIITREF]    Script Date: 01/16/2010 11:08:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EIITREF]') AND type in (N'U'))
CREATE TABLE [dbo].[EIITREF](
	[entry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[date] [datetime] NULL,
	[doc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[item_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rentry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rinv_sr] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rinv_no] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rl_yn] [varchar](9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ritem_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rqty] [numeric](13, 4) NULL,
	[item] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ware_nm] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rinv_no1] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rparty_nm] [char](35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[new_docno] [char](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Compid] [int] NULL,
	[Tran_cd] [int] NULL,
	[Ritserial] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Rdoc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Itserial] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Rdate] [datetime] NULL,
	[It_code] [numeric](18, 0) NULL,
	[Itref_tran] [int] NOT NULL,
	[tbl_id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIITREF_EIMAIN]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIITREF]'))
ALTER TABLE [dbo].[EIITREF]  WITH NOCHECK ADD  CONSTRAINT [FK_EIITREF_EIMAIN] FOREIGN KEY([Tran_cd])
REFERENCES [dbo].[EIMAIN] ([Tran_cd])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIITREF] CHECK CONSTRAINT [FK_EIITREF_EIMAIN]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIITREF_it_mast]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIITREF]'))
ALTER TABLE [dbo].[EIITREF]  WITH NOCHECK ADD  CONSTRAINT [FK_EIITREF_it_mast] FOREIGN KEY([It_code])
REFERENCES [dbo].[IT_MAST] ([It_code])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIITREF] CHECK CONSTRAINT [FK_EIITREF_it_mast]