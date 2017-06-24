IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = Object_id(N'[dbo].[USP_ENT_EMP_DOC_DET]') AND Objectproperty(id,N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE USP_ENT_EMP_DOC_DET
	PRINT 'BUG NO.-1099:OLD PROCEDURE USP_ENT_EMP_DOC_DET HAS BEEN DROPPED'					
END

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
-- =============================================
-- Created By : sANJAY
-- Create date: 09/12/2011
-- Description:	This Stored Procedure is used by Employee master for getting Earning & Deduction Details
-- Remark	  : 
-- Modified By and Date : Sanjay Choudhari 09/12/2011 
-- =============================================
--execute USP_ENT_EMP_DOC_DET exkjhdfkjh
CREATE PROCEDURE [dbo].[USP_ENT_EMP_DOC_DET]
@EMPCODE AS VARCHAR(20)
AS 
    BEGIN
        DECLARE @sqlcommand AS VARCHAR(4000) ,
			@EMPID AS INT,
            @DocNm varchar(100) ,
            @REQ AS BIT ,
            @COMP AS BIT,
            @filename varchar(50),
            @extension varchar(4),
            @objpath varchar(200)
		
        SELECT  id ,DocNm ,
                EmpId = CAST(0 AS INT) ,
                EmployeeCode = CAST('' AS VARCHAR(20)) ,
                DocId = CAST(0 AS INT) ,
                Req = CAST(0 AS BIT) ,
                Comp = CAST(0 AS BIT),
                [FILENAME]=CAST('' AS VARCHAR(50)),
                extension= CAST('' AS varchar(4)),
                objpath=CAST(''AS VARCHAR(200))
        INTO    #EMPDOCDET
        FROM    dbo.Emp_Doc_Master
        ORDER BY SortOrd
	

        DECLARE	CUR_EMPDOCDET CURSOR FOR

        SELECT empid,DocNm,Req,Comp,[FILENAME],extension,objpath FROM dbo.Emp_Doc_Details WHERE EmployeeCode=@EMPCODE
        
        OPEN CUR_EMPDOCDET
        FETCH NEXT FROM CUR_EMPDOCDET INTO @EMPID,@DocNm,@REQ,@COMP,@filename,@extension,@objpath
        WHILE ( @@FETCH_STATUS = 0 ) 
            BEGIN
                UPDATE  #EMPDOCDET
                SET     REQ = @REQ ,
				        Comp = @COMP,
						EMPID= @EMPID,
						EmployeeCode=@EMPCODE,
						DocNm=@DocNm,
						[filename]=@filename,
						extension=@extension,
						objpath=@objpath     
                WHERE   DocNm = @DocNm
                FETCH NEXT FROM CUR_EMPDOCDET INTO @EMPID,@DocNm,@REQ,@COMP,@filename,@extension,@objpath
            END
        CLOSE CUR_EMPDOCDET
        DEALLOCATE CUR_EMPDOCDET
        SELECT id ,DocNm ,EmpId ,EmployeeCode ,CASE DocId WHEN 0 THEN id ELSE docid END AS DOCID,Req ,Comp ,FILENAME ,extension ,objpath FROM  #EMPDOCDET
    END
GO

PRINT 'BUG NO.1099:PROCEDURE USP_ENT_EMP_DOC_DET HAS BEEN UPDATED'					
