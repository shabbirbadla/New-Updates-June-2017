IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_FILTCON]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_FILTCON]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_TRD_FORM2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_TRD_FORM2]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_VU8FORM2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_VU8FORM2]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_Manufact]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_Manufact]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Update_table_column_default_value]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Update_table_column_default_value]
