IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_FILTCON]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_FILTCON]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_ER1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_ER1]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_VU8ER1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_VU8ER1]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_Manufact]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_Manufact]
