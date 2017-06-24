IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='USP_ENT_EMAILCLIENT_INSERTUPDATE' AND XTYPE='P')
BEGIN
	DROP PROCEDURE USP_ENT_EMAILCLIENT_INSERTUPDATE
END
GO
Create Procedure [dbo].[USP_ENT_EMAILCLIENT_INSERTUPDATE]
@action varchar(10),
@id varchar(20),
@desc varchar(1000)='',
@tran_typ varchar(1000)='',
@hasattachment bit=0,
@attachment_typ varchar(20)='',
@rep_nm varchar(1000)='',
@to varchar(1000)='',
@cc varchar(1000)='',
@bcc varchar(1000)='',
@subject varchar(1000)='',
@body varchar(3000)='',
@query varchar(3000)='',
@reportquery varchar(3000)='',
@parameters varchar(2000)='',
@separator varchar(2)='',
@encoding varchar(20)='',
@isFirstrow bit=0,
@reportquerytype varchar(2)='',
@exportpath varchar(1000)='',
@exportprefixname varchar(100)='',
@removefiles bit=0,
@emaillogfiles bit=0,
@logemailid varchar(1000)=''
As
Begin
	If rtrim(upper(@action)) = 'INSERT'
	Begin
		Insert into eMailClient(id,[desc],tran_typ,hasattachment,attachment_typ,rep_nm,[to],cc,bcc,[subject],body,query,reportquery,[parameters],separator,encoding,isFirstrow,reportquerytype,exportpath,exportprefixname,removefiles,emaillogfiles,logemailid)
		values(@id,@desc,@tran_typ,@hasattachment,@attachment_typ,@rep_nm,@to,@cc,@bcc,@subject,@body,@query,@reportquery,@parameters,@separator,@encoding,@isFirstrow,@reportquerytype,@exportpath,@exportprefixname,@removefiles,@emaillogfiles,@logemailid)
	End
	
	If rtrim(upper(@action)) = 'UPDATE'
	Begin
		Update eMailClient set [desc]=@desc,tran_typ=@tran_typ,hasattachment=@hasattachment,attachment_typ=@attachment_typ,
			rep_nm=@rep_nm,[to]=@to,cc=@cc,bcc=@bcc,[subject]=@subject,body=@body,query=@query,reportquery=@reportquery,[parameters]=@parameters,
			separator=@separator,encoding=@encoding,isFirstrow=@isFirstrow,reportquerytype=@reportquerytype,exportpath=@exportpath,exportprefixname=@exportprefixname,
			removefiles=@removefiles,emaillogfiles=@emaillogfiles,logemailid=@logemailid Where id=@id
	End
End