/****** Object:  Table [dbo].[BUNDLE]    Script Date: 01/18/2010 17:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUNDLE]') AND type in (N'U'))
CREATE TABLE [dbo].[BUNDLE](
	[bundle] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[item] [char](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[qty] [numeric](9, 3) NULL,
	[weight] [numeric](9, 3) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF