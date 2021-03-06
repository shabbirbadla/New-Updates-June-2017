CREATE TABLE [dbo].[Emp_Shift_Master](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Shift_Code] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Emp_Shift_Master_Shift_Code]  DEFAULT (''),
	[Shift_Name] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Loc_Code] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_Emp_Shift_Master_Loc_Code]  DEFAULT (''),
	[ShiftFrom] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShiftTo] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShiftMinHrs] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LunchFrom] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_Emp_Shift_Master_LunchFrom]  DEFAULT (''),
	[LunchTo] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_Emp_Shift_Master_LunchTo]  DEFAULT (''),
	[Default_Shift] [bit] NULL CONSTRAINT [DF_Emp_Shift_Master_Default_Shift]  DEFAULT ((0)),
	[Incharge_Code] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Remark] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_Emp_Shift_Master_Remark]  DEFAULT (''),
	[HalfDayHrs] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF__Emp_Shift__HalfD__2261A781]  DEFAULT ('')
) ON [PRIMARY]


IF NOT EXISTS(SELECT [Shift_Code],[Shift_Name] FROM Emp_Shift_Master WHERE Shift_Code = 'General' AND Shift_Name = 'General Shift')BEGIN INSERT INTO Emp_Shift_Master([SHIFT_CODE],[SHIFT_NAME],[LOC_CODE],[SHIFTFROM],[SHIFTTO],[SHIFTMINHRS],[LUNCHFROM],[LUNCHTO],[DEFAULT_SHIFT],[INCHARGE_CODE],[REMARK],[HALFDAYHRS]) VALUES ('General','General Shift','','09:00 AM','17:30 PM','8','13:00 PM','13:30 PM',1,'','','5') END 