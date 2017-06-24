CREATE TABLE [dbo].[UploadFiles](
	[Tr_Type] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Tr_Id] [int] NULL,
	[Tr_Serial] [int] NULL,
	[fileName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Extension] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ObjImage] [image] NULL,
	[ObjPath] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]