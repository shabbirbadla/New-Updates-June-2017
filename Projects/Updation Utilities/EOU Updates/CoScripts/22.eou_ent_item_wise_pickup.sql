
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
Declare	@paraEntry_Ty Varchar(2),@paraTran_Cd Int,
@paraCDept as Varchar(20),@paraIt_name as Varchar(50)
SELECT	@paraEntry_Ty = 'PT',@paraTran_Cd = 0,@paraCDept = 'EOU',@paraIt_name = 'MS ANGLE 200X200X16'
Execute EOU_Ent_Item_wise_Pickup @paraEntry_Ty,@paraTran_Cd,@paraCDept,@paraIt_name
*/
Create Procedure [dbo].[EOU_Ent_Item_wise_Pickup]
(
	@paraEntry_Ty Varchar(2),
	@paraTran_Cd Int,
	@paraCDept as Varchar(20),
	@paraIt_name as Varchar(50)
)
As
/* Internale Variable declaration and Assigning [Start] */
DECLARE @Fld_Nm Varchar(10),@Pert_Name Varchar(10)
DECLARE @FldName Varchar(500),@FldPerName Varchar(500)
SELECT @FldName = '',@FldPerName = ''

Declare @TBLNM Varchar(50),@TBLNAME1 Varchar(50),@TBLNAME2 Varchar(50),
	@TBLNAME3 Varchar(50),@TBLNAME4 Varchar(50),
	@SQLCOMMAND as NVARCHAR(4000)

Select @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
		+ (DATEPART(ss, GETDATE()) * 1000 )+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
		
Select @TBLNAME1 = '##TMP1'+@TBLNM,@TBLNAME2 = '##TMP2'+@TBLNM
Select @TBLNAME3 = '##TMP3'+@TBLNM,@TBLNAME4 = '##TMP4'+@TBLNM
/* Internale Variable declaration and Assigning [End] */

DECLARE DcmastCur CURSOR FOR 
	Select Fld_Nm,Pert_Name From Dcmast
		Where Code = 'E' And Entry_Ty = 'PT' AND Att_File = 0

OPEN DcmastCur

FETCH NEXT FROM DcmastCur INTO @Fld_Nm,@Pert_Name

WHILE @@FETCH_STATUS = 0
   BEGIN
	  IF @Fld_Nm IS NOT NULL AND @Fld_Nm <> ''	
	  BEGIN	
		  SET @FldName = @FldName+'b.'+LTrim(RTrim(@Fld_Nm))+','
	  END
	  IF @Pert_Name IS NOT NULL AND @Pert_Name <> ''	
	  BEGIN	
		  SET @FldPerName = @FldPerName+'b.'+LTrim(RTrim(@Pert_Name))+','
	  END
      FETCH NEXT FROM DcmastCur INTO @Fld_Nm,@Pert_Name;
   END

CLOSE DcmastCur

DEALLOCATE DcmastCur

SET @FldName = LEFT(LTrim(RTrim(@FldName)),Len(LTrim(RTrim(@FldName)))-1)
SET @FldPerName = LEFT(LTrim(RTrim(@FldPerName)),Len(LTrim(RTrim(@FldPerName)))-1)

IF @FldName IS NOT NULL AND @FldName <> ''
BEGIN 
	IF @FldPerName IS NOT NULL AND @FldPerName <> ''
	BEGIN
		SELECT @FldName = LTrim(RTrim(@FldName))+','		
	END
END	

DECLARE @Select as Bit,@SQLStr nVarchar(4000),
	@ParmDefinition nvarchar(500)

SELECT @Select = 0

SET @ParmDefinition = N'@Select Bit,@paraEntry_Ty Varchar(2),@paraTran_Cd Int';
SELECT @SQLStr = 'Select @Select as lSelect,a.Entry_Ty as REntry_Ty,
	a.Date as RDate,a.Doc_No as RDoc_no,b.Item_no,a.Inv_No as RInv_No,a.L_Yn as RL_Yn,
	a.Tran_cd as ITref_tran,a.Dept,a.Cate,a.[Rule],a.Inv_Sr as RInv_Sr,
	a.u_beno,a.U_pinvdt,B.It_Code,a.Entry_Ty,a.Date,a.Doc_No,c.RItserial as Itserial,
	c.REntry_Ty as _REntry_Ty,c.Itref_Tran as _Itref_Tran,c.RItserial as _RItserial,
	Ac_Mast.Ac_Name as RParty_nm,It_Mast.It_Name as Item,Space(100) as litemkey,
	b.Itserial as RItserial,b.u_asseamt,b.Qty,b.rate,000000000.0000 As adjqty,
	000000000.0000 As adjrepqty,b.Re_qty as RQty,b.Qty-b.Re_qty As BalQty,'

IF @FldName = '' AND @FldPerName = ''
	BEGIN 
		SET @SQLStr = LEFT(LTrim(RTrim(@SQLStr)),Len(LTrim(RTrim(@SQLStr)))-1)
	END	
ELSE
	BEGIN
		SELECT @SQLStr = @SQLStr+@FldName+@FldPerName
	END

SELECT @SQLStr = @SQLStr+' From AC_Mast,IT_Mast,EOU_LMain_vw a,EOU_LItem_vw b
	Left Join EOU_Itref_vw c ON (B.Entry_Ty = C.REntry_Ty AND B.Tran_cd = C.ITREF_Tran
	AND B.Itserial = C.RItserial AND c.Entry_Ty +Convert(Varchar(15),c.Tran_cd) <> @paraEntry_Ty+Convert(Varchar(15),@paraTran_Cd) ) 
	WHERE a.Entry_Ty = B.Entry_Ty AND a.Tran_cd = b.Tran_cd
	AND B.It_Code = It_Mast.It_Code AND A.Entry_Ty In (''PT'',''IR'')
	AND A.Ac_Id = Ac_Mast.Ac_Id '

If RTrim(@paraCDept) <> ''
	Begin
		SELECT @SQLStr = @SQLStr+'AND a.Dept = '+Char(39)+RTrim(@paraCDept)+Char(39)
	End
If RTrim(@paraIt_name) <> ''
	Begin
		SELECT @SQLStr = @SQLStr+'AND It_Mast.It_name = '+Char(39)+RTrim(@paraIt_name)+Char(39)
	End

EXECUTE sp_executesql @SQLStr, @ParmDefinition,@Select = @Select,
	@paraTran_Cd = @paraTran_Cd,@paraEntry_Ty = @paraEntry_Ty



