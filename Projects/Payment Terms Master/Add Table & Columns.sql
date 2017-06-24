/*Master Table of Payment Terms*/
IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME ='PayTerms')
BEGIN
	CREATE TABLE [dbo].[PayTerms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PTName] [varchar](100) NOT NULL,
	[PTDescrip] [varchar](250) NOT NULL,
	[Validin] [varchar](100) NULL,
	[checked] [bit] NOT NULL
	) ON[PRIMARY]
END

/*Transaction Table of Payment Terms Master*/
IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME ='PayTermsDet')
BEGIN
CREATE TABLE [dbo].[PayTermsDet](
	[Tran_cd] [numeric](10, 0) NULL,
	[Entry_ty] [varchar](2) NULL,
	[PTDescrip] [varchar](250) NULL,
	[PTName] [varchar](100) NULL
) ON [PRIMARY]
END

/*Field Creation for Payment Terms Button*/
EXECUTE Add_Columns 'LCODE','DISPTERMS BIT DEFAULT 0 WITH VALUES'
EXECUTE ADD_COLUMNS 'LCODE','LINCSTKRSV BIT DEFAULT 0 WITH VALUES'