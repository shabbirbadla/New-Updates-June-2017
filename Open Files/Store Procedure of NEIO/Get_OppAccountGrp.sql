If Exists (Select [name] From Sysobjects Where xType='P' and [name]='Get_OppAccountGrp')
Begin
	Drop Procedure Get_OppAccountGrp
End
Go
Create Procedure Get_OppAccountGrp
@GroupName Varchar(100)
as
Declare @GrpId Numeric(15,0),@IsTrue Bit,@LVL int,@Gac_name Varchar(100),@Ac_group_name Varchar(100)
If @GroupName=''
Begin
	Select ac_group_name as GroupName,ac_group_id as GroupId From ac_group_mast Where [Type] = 'B' Order by ac_group_name
End
Else
Begin
	set @IsTrue=1
	While @IsTrue=1
	Begin
		--print @GroupName
		Select @Ac_group_name=Ac_Group_name,@Gac_name=[Group] From Ac_Group_Mast Where Ac_Group_Name=@GroupName
		if (@Ac_group_name='LIABILITIES' or @Ac_group_name='ASSETS')
		Begin
			set @IsTrue=0
			set @Ac_group_name=Case when @Ac_group_name='LIABILITIES' Then 'ASSETS' Else 'LIABILITIES' End 
			break
		End
		set @GroupName=@Gac_name
	End
	
	print @Ac_group_name
	CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
	SET @LVL=0
	INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME=@Ac_group_name
	SET @IsTrue=1
	WHILE @IsTrue=1
	BEGIN
		IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL) And Ac_Group_id<>Gac_id) 
		BEGIN
			INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL) And Ac_Group_id<>Gac_id
			SET @LVL=@LVL+1
		END
		ELSE
		BEGIN
			SET @IsTrue=0	
		END
	END
	
	Select ac_group_name as GroupName,ac_group_id as GroupId From ac_group_mast 
		Where [Type] = 'B' and AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID) Order by ac_group_name
	
	Drop table #ACGRPID
End

