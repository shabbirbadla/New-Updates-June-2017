/****** Object:  Table [dbo].[EIMALL]    Script Date: 01/16/2010 11:08:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EIMALL]') AND type in (N'U'))
CREATE TABLE [dbo].[EIMALL](
	[entry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[date] [datetime] NULL,
	[doc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[inv_no] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[party_nm] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[new_all] [numeric](17, 2) NULL,
	[entry_all] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[inv_sr] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[tds] [numeric](11, 2) NULL,
	[disc] [numeric](11, 2) NULL,
	[l_yn] [varchar](9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Net_amt] [numeric](17, 2) NULL,
	[Main_tran] [int] NOT NULL,
	[Date_all] [datetime] NULL,
	[Ac_id] [int] NULL,
	[Compid] [int] NULL,
	[Tran_cd] [int] NULL,
	[Doc_all] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ACSERIAL] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ACSERI_ALL] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[tbl_id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIMALL_ac_mast]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIMALL]'))
ALTER TABLE [dbo].[EIMALL]  WITH NOCHECK ADD  CONSTRAINT [FK_EIMALL_ac_mast] FOREIGN KEY([Ac_id])
REFERENCES [dbo].[AC_MAST] ([Ac_id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIMALL] CHECK CONSTRAINT [FK_EIMALL_ac_mast]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EIMALL_EIMAIN]') AND parent_object_id = OBJECT_ID(N'[dbo].[EIMALL]'))
ALTER TABLE [dbo].[EIMALL]  WITH NOCHECK ADD  CONSTRAINT [FK_EIMALL_EIMAIN] FOREIGN KEY([Tran_cd])
REFERENCES [dbo].[EIMAIN] ([Tran_cd])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EIMALL] CHECK CONSTRAINT [FK_EIMALL_EIMAIN]