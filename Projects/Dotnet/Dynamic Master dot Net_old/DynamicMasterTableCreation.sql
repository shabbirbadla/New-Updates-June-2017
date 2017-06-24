/*Dynamic Master Creation--->*/
if not exists (select [name] from sysobjects where [name]='FORM_MASTER')
begin
	CREATE TABLE [dbo].[FORM_MASTER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CAPTION] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CODE] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TABLE_NAME] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
if not exists (select [name] from sysobjects where [name]='TAB_CONTROLS')
begin
	CREATE TABLE [dbo].[TAB_CONTROLS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CAPTION] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CODE] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TAB_ORDER] [int] NULL,
	[FORM_CODE] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
if not exists (select [name] from sysobjects where [name]='FIELD_MASTER')
begin
	CREATE TABLE [dbo].[FIELD_MASTER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SELECTEDTAB] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FIELD_ORDER] [int] NULL,
	[CAPTION] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TOOLTIP] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MANDATORY] [bit] NULL,
	[FIELDNAME] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DATATYPE] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SIZE] [int] NULL,
	[DECIMAL] [numeric](18, 0) NULL,
	[SearchField] [bit] NULL,
	[INPUTMASK] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HELPQUERY] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[REMARKS] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WHENCONDITION] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DEFAULTVALUE] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[VALIDATION] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[INTERNALUSE] [bit] NULL,
	[FORM_CODE] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TAB_CODE] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[VAL_ERROR] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LineCount] [int] NULL DEFAULT ((0)),
	[MainField] [bit] NULL DEFAULT ((0)),
	PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

end
/*<---Dynamic Master Creation*/
go
/*DEPB Product Group Master--->*/
if not exists (select [code] from [FORM_MASTER] where code='DEG')
begin
	insert into [FORM_MASTER] (Caption,Code,Table_Name) Values ('DEPB Product Group Master','DEG','DEPBGROUP_MASTER')
end
if not exists (select [code] from [TAB_CONTROLS] where code='Gen' and Form_Code='DEG')
begin
	insert into [TAB_CONTROLS] (Caption,Code,Tab_Order,Form_Code) Values ('General Details','Gen',1,'DEG')
end

if not exists (select SelectedTab from [FIELD_MASTER] where [FIELDNAME] ='PCode' and tab_code='Gen' and Form_Code='DEG')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',1,'Product Code','Product Code',6/*1*/
	,'PCode','varchar',3,0 /*2*/
	,0,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEG','Gen',0,1/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='PGroup' and tab_code='Gen' and Form_Code='DEG')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',2,'Product Group','Product Group',5/*1*/
	,'PGroup','varchar',60,0 /*2*/
	,1,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEG','Gen',0,0/*5*/)
end
/*<---DEPB Product Group Master*/

/*DEPB Master--->*/
if not exists (select [code] from [FORM_MASTER] where code='DEP')
begin
	insert into [FORM_MASTER] (Caption,Code,Table_Name) Values ('DEPB Master','DEP','DEPB_MASTER')
end
if not exists (select [code] from [TAB_CONTROLS] where code='Gen' and Form_Code='DEP')
begin
	insert into [TAB_CONTROLS] (Caption,Code,Tab_Order,Form_Code) Values ('General Details','Gen',1,'DEP')
end

if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='Depb_sr' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',1,'SR. No.','DEPB SR. No.',1/*1*/
	,'Depb_sr','varchar',20,0 /*2*/
	,1,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',0,1/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='Norms' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',2,'Norms','DEPB Norms',2/*1*/
	,'Norms','varchar',20,0 /*2*/
	,1,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',0,0/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='Depb_rate' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',3,'Rate','DEPB Rate',3/*1*/
	,'Depb_Rate','Decimal',17,3 /*2*/
	,0,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',0,0/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='ValCap' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',4,'Value Cap','Value Cap',4/*1*/
	,'ValCap','Decimal',17,2 /*2*/
	,0,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',0,0/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='PGroup' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',5,'Product Group','Product Group',5/*1*/
	,'PGroup','varchar',60,0 /*2*/
	,1,'','SELECT PGROUP,PCODE FROM DEPBGROUP_MASTER {PGROUP#PGROUP##PGROUP:Product Group,PCODE:Product Code}',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',0,0/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='PCode' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',6,'Product Code','Product Code',6/*1*/
	,'PCode','varchar',3,0 /*2*/
	,1,'','SELECT PGROUP,PCODE FROM DEPBGROUP_MASTER {PCODE#PCODE##PGROUP:Product Group,PCODE:Product Code}',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',0,0/*5*/)
end
if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='Descrip' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',7,'Description','Description',7/*1*/
	,'Descrip','varchar',250,0 /*2*/
	,0,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',3,0/*5*/)
end

if not exists (select SelectedTab,* from [FIELD_MASTER] where [FIELDNAME] ='Short_Desc' and tab_code='Gen' and Form_Code='DEP')
begin
	insert into [FIELD_MASTER] ([SELECTEDTAB],[FIELD_ORDER],[CAPTION],[TOOLTIP],[MANDATORY] /*1*/
	,FIELDNAME,DATATYPE,[SIZE],[DECIMAL]/*2*/
	,[SearchField],[INPUTMASK],[HELPQUERY],[REMARKS]/*3*/
	,[WHENCONDITION],[DEFAULTVALUE],[VALIDATION],[INTERNALUSE],	[VAL_ERROR]/*4*/
	,[FORM_CODE],[TAB_CODE],[LineCount],[MainField] /*5*/)
	Values ('Gen',8,'Short Description','Short Description',7/*1*/
	,'Short_Desc','varchar',60,0 /*2*/
	,0,'','',''	/*3*/
	,'','','',0,''/*4*/
	,'DEP','Gen',2,0/*5*/)
end
/*<---DEPB Master*/
Go
/*Master Creation--->*/
if not exists (select [name] from sysobjects where [name]='DEPBGROUP_MASTER')
begin
	CREATE TABLE [dbo].[DEPBGROUP_MASTER](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[PCode] [varchar](3)  NOT NULL,
		[PGroup] [varchar](60) NOT NULL
	) ON [PRIMARY]
end
if not exists (select [name] from sysobjects where [name]='DEPB_Master')
begin
	CREATE TABLE [dbo].[DEPB_Master](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Depb_sr] [varchar](20)  NOT NULL,
		[Norms] [varchar](20)  NOT NULL,
		[Depb_rate] [decimal](17, 3) NOT NULL,
		[ValCap] [decimal](17, 2) NOT NULL,
		[PGroup] [varchar](60) NOT NULL,
		[PCode] [varchar](3) NOT NULL,
		[Descrip] [varchar](250)  NOT NULL,
		[Short_Desc] [varchar](60) NOT NULL
	) ON [PRIMARY]
end
/*<---Master Creation*/
