If Not Exists(Select Name from Sys.Objects Where Name = 'Container_Det')
Begin
CREATE TABLE [dbo].[Container_Det](
	[Entry_ty] [varchar](2) NOT NULL,
	[tran_cd] [int] NOT NULL,
	[sr_no] [int] NULL,
	[cont_no] [varchar](20) NULL,
	[cont_size] [varchar](15) NULL,
	[ship_seal_no] [varchar](20) NULL,
	[exc_bottle_no] [varchar](20) NULL,
	[no_package] [varchar](50) NULL,
	[land_detail] [varchar](200) NULL,
	[remarks] [varchar](100) NULL
) ON [PRIMARY]
END