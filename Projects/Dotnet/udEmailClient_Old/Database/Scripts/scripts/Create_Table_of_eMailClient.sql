IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='eMailClient' AND XTYPE='U')
BEGIN
	DROP TABLE eMailClient
END
GO
CREATE TABLE [dbo].[eMailClient](
	[id] [varchar](20) NOT NULL,
	[desc] [varchar](1000) NULL,
	[tran_typ] [varchar](1000) NULL,
	[hasattachment] [bit] NULL,
	[attachment_typ] [varchar](20) NULL,
	[rep_nm] [varchar](1000) NULL,
	[to] [varchar](1000) NULL,
	[cc] [varchar](1000) NULL,
	[bcc] [varchar](1000) NULL,
	[subject] [varchar](1000) NULL,
	[body] [varchar](3000) NULL,
	[query] [varchar](3000) NULL,
	[reportquery] [varchar](3000) NULL,
	[parameters] [varchar](2000) NULL,
	[separator] [varchar](2) NULL,
	[encoding] [varchar](20) NULL,
	[isFirstrow] [bit] NULL,
	[reportquerytype] [varchar](2) NULL,
	[exportpath] [varchar](1000) NULL,
	[isdeactive] [bit] NULL,
	[exportprefixname] [varchar](100) NULL,
	[removefiles] [bit] NULL,
	[emaillogfiles] [bit] NULL,
	[logemailid] [varchar](1000) NULL
) ON [PRIMARY]
GO