IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='USP_ENT_EMAILLOG_DELETE' AND XTYPE='P')
BEGIN
	DROP PROCEDURE USP_ENT_EMAILLOG_DELETE
END
GO
Create Procedure USP_ENT_EMAILLOG_DELETE
@id varchar(20)='',
@filename varchar(100)=''
As
Begin
	Delete From eMailLog Where id=@id and [filename]=@filename
End