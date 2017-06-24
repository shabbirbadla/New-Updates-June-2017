IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Eou_Itref_vw]'))
DROP VIEW [dbo].[Eou_Itref_vw]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Eou_LItem_vw]'))
DROP VIEW [dbo].[Eou_LItem_vw]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Eou_LMain_vw]'))
DROP VIEW [dbo].[Eou_LMain_vw]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EOU_Ent_Item_wise_Pickup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EOU_Ent_Item_wise_Pickup]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_118ISSUE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_118ISSUE]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_118ISSUEIMP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_118ISSUEIMP]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_118ISSUEIND]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_118ISSUEIND]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_118REC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_118REC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_118REC(IND)]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_118REC(IND)]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_118RECIND]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_118RECIND]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_B17_REG]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_B17_REG]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_ct3/pc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_ct3/pc]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_ct3pc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_ct3pc]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_ct3pc1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_ct3pc1]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_EXP_REG]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_EXP_REG]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_GRN]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_GRN]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_ILEDGER1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_ILEDGER1]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_INDREG]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_INDREG]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_INPUTdaily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_INPUTdaily]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_INPUTTOPRODUCTION]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_INPUTTOPRODUCTION]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_SALESUM]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_SALESUM]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Update_table_column_default_value]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Update_table_column_default_value]






