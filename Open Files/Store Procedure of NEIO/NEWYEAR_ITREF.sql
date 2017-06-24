IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NEWYEAR_ITREF]') AND type in (N'U'))
BEGIN
/****** Object:  Table [dbo].[PTITREF]    Script Date: 04/19/2011 12:18:05 ******/
Create TABLE [dbo].[NEWYEAR_Itref](
	[entry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[date] [datetime] NULL,
	[doc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rentry_ty] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rqty] [numeric](16, 4) NULL,
	[item] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ware_nm] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Itref_tran] [int] NOT NULL,
	[Tran_cd] [int] NULL,
	[Itserial] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Rdate] [datetime] NULL,
	[Rdoc_no] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Ritserial] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[It_code] [numeric](18, 0) NULL,
	[CompId] [int] NULL,
	[rinv_sr] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rinv_no] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[rl_yn] [varchar](9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) 
END
