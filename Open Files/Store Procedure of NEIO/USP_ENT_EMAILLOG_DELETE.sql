/****** Object:  StoredProcedure [dbo].[USP_ENT_EMAILLOG_DELETE]    Script Date: 01/23/2014 17:18:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS(SELECT [NAME] FROM SYS.OBJECTS WHERE [NAME]='USP_ENT_EMAILLOG_DELETE' AND TYPE='P')
BEGIN
	DROP PROCEDURE [USP_ENT_EMAILLOG_DELETE]
END
GO

--===== Procedure for Deleting records from Email Log table =====--
-- Created by/On/For : Sachin N. S. on 11/03/2014 for Bug-20211
--===============================================================--
Create Procedure [dbo].[USP_ENT_EMAILLOG_DELETE]
@id varchar(20)='',
@filename varchar(100)=''
As
Begin
	Delete From eMailLog Where id=@id and [filename]=@filename
End

