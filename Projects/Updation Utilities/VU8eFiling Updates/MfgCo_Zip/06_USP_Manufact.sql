-- Changed by Deepa 
-- Reason Eccno field was not given before

CREATE PROCEDURE [dbo].[USP_Manufact] 
--@compid AS VARCHAR(60)
	-- Add the parameters for the stored procedure here
--	<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
--	<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select eccno as RegistrationNo,bcd as BSRCODE  from manufact;
    

END

