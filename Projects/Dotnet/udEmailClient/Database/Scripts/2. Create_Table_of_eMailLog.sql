IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='eMailLog' AND XTYPE='U')
BEGIN
CREATE TABLE [dbo].[eMailLog](
	[autoId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [varchar](20) NULL,
	[to] [varchar](1000) NULL,
	[cc] [varchar](1000) NULL,
	[bcc] [varchar](1000) NULL,
	[subject] [varchar](1000) NULL,
	[body] [varchar](3000) NULL,
	[filepath] [varchar](100) NULL,
	[filename] [varchar](100) NULL,
	[removefiles] [bit] NULL,
	[status] [varchar](20) NULL,
	[remarks] [varchar](1000) NULL,
	[emaillogfiles] [bit] NULL,
	[logemailid] [varchar](1000) NULL
) ON [PRIMARY]
END