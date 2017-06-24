IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_Company_Details]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Get_Company_Details]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_CoMast_ER1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_CoMast_ER1]
