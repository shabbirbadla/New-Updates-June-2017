IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='eMailSettings' AND XTYPE='U')
BEGIN
CREATE TABLE [dbo].[eMailSettings](
	[yourname] [varchar](100) NULL,
	[username] [varchar](100) NULL,
	[password] [varchar](100) NULL,
	[host] [varchar](100) NULL,
	[port] [int] NULL,
	[enablessl] [bit] NULL
) ON [PRIMARY]
END