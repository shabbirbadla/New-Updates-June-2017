CREATE TABLE [dbo].[FileUploadMaster](
	[E_CODE] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[HEAD_NM] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ATT_FILE] [bit] NULL,
	[SERIAL] [numeric](3, 0) NULL,
	[TYPE] [varchar](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DISPORD] [numeric](2, 0) NULL,
	[DATABASESAVE] [bit] NULL,
	[FlupId] [int] IDENTITY(1,1) NOT NULL,
	[FileSize] [int] NULL
) ON [PRIMARY]


alter table fileuploadmaster add formno numeric(4,2)