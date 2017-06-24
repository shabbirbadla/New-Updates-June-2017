/****** Object:  Table [dbo].[EIACDET]    Script Date: 01/16/2010 11:08:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EIACDET]') AND type in (N'U'))
CREATE TABLE [dbo].[EIACDET](
	[entry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[date] [datetime] NOT NULL,
	[doc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[dept] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[cate] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_name] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[amount] [numeric](16, 2) NULL,
	[amt_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[memo_op] [bit] NULL,
	[oac_name] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[clause] [char](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[al] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[cl_date] [datetime] NULL,
	[loantds] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[narr] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_plasr] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_rg23no] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_rg23cno] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[u_cldt] [datetime] NULL,
	[newdocno] [char](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[memo_text] [char](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[inv_sr] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[inv_no] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[l_yn] [char](9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[new_docno] [char](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[re_all] [numeric](18, 2) NULL,
	[ref_no] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[tds] [numeric](16, 2) NULL,
	[disc] [numeric](16, 2) NULL,
	[Doc_all] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Tran_cd] [int] NULL,
	[Date_all] [datetime] NULL,
	[Ac_id] [int] NULL,
	[Compid] [int] NULL,
	[u_hcessamt] [numeric](11, 2) NULL,
	[ACSERIAL] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[tbl_id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIACDET_ac_mast]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIACDET]'))
ALTER TABLE [dbo].[EIACDET]  WITH NOCHECK ADD  CONSTRAINT [FK_EIACDET_ac_mast] FOREIGN KEY([Ac_id])
REFERENCES [dbo].[AC_MAST] ([Ac_id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIACDET] CHECK CONSTRAINT [FK_EIACDET_ac_mast]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIACDET_EIMAIN]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIACDET]'))
ALTER TABLE [dbo].[EIACDET]  WITH NOCHECK ADD  CONSTRAINT [FK_EIACDET_EIMAIN] FOREIGN KEY([Tran_cd])
REFERENCES [dbo].[EIMAIN] ([Tran_cd])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIACDET] CHECK CONSTRAINT [FK_EIACDET_EIMAIN]