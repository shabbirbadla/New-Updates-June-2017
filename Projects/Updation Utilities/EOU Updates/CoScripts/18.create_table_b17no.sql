/****** Object:  Table [dbo].[b17no]    Script Date: 01/21/2010 12:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[b17no]') AND type in (N'U'))
CREATE TABLE [dbo].[b17no](
	[b17no] [numeric](18, 0) NULL,
	[entry_ty] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF