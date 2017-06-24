create Procedure Gen_Constraint_script
@dbname Varchar(20),@paraTable Varchar(50)
as 

Set Nocount On
Create Table #IndexTable(Script Varchar(4000)) 
Declare @TableName Varchar(50),@IndexName Varchar(35),@Index_id Int,@ColumnName Varchar(50)
Declare @Key_Ordinal int,@Type_Desc Varchar(15),@Is_Primary_key Bit,@Is_Descending_key Bit,@Script NVarchar(4000)
Declare @TableName1 Varchar(50),@IndexName1 Varchar(35),@ColumnName1 Varchar(50)
Declare Index_cursor Cursor For
SELECT A.NAME AS TableName,B.NAME AS IndexName,B.Index_id,D.NAME AS ColumnName,
               C.Key_Ordinal,b.Type_desc,b.is_primary_key,c.is_descending_key
        FROM   SYS.OBJECTS A 
               INNER JOIN SYS.INDEXES B ON A.OBJECT_ID = B.OBJECT_ID AND B.allow_page_locks=1
               INNER JOIN SYS.INDEX_COLUMNS C ON B.OBJECT_ID = C.OBJECT_ID AND B.INDEX_ID = C.INDEX_ID
               INNER JOIN SYS.COLUMNS D ON C.OBJECT_ID = D.OBJECT_ID
                    AND C.COLUMN_ID = D.COLUMN_ID
			WHERE  A.TYPE <> 'S'
			and a.[Name]=@paraTable 	
			Order By B.Type_Desc,a.[Name],B.Index_Id,Key_Ordinal
		
Open Index_cursor
Fetch Next From Index_cursor Into @TableName,@IndexName,@Index_id,@ColumnName,@Key_Ordinal,@Type_Desc,@Is_Primary_key,@Is_Descending_key
--set @Script='If Not Exists (Select * From SysObjects Where Object_Id=Object_Id(N'''+@TableName+''') and [Name]='''+@IndexName+''') Create '+@Type_Desc+' Index ['+@IndexName+'] On '+@TableName+ ' ('
set @TableName1=@TableName
set @IndexName1=@IndexName
set @Script=''
While @@Fetch_Status=0
Begin
	if @Type_Desc='Clustered'		
	Begin
		if @Key_Ordinal=1
		Begin
			set @Script='If Not Exists (Select * From SysObjects Where [Name]='''+@IndexName+''') Alter Table '+@TableName+' Add Constraint ['+@IndexName+'] Primary Key ('
			set @Script=@Script+' ['+@ColumnName+'] '+ Case When @Is_Descending_key=0 Then ' Asc ' else ' Desc ' End
		End
		if @Key_Ordinal>1
		Begin	
			set @Script=@Script+', ['+@ColumnName+'] '+ Case When @Is_Descending_key=0 Then ' Asc ' else ' Desc ' End
		End
		
	End
	if @Type_Desc='NonClustered'		
	Begin
		if @Key_Ordinal=1
		Begin
			set @Script='If Not Exists (Select * From Sys.Indexes Where [Name]='''+@IndexName+''' and Object_Id=Object_Id(N'''+@TableName+''')) CREATE NONCLUSTERED INDEX  ['+@IndexName+'] On '+@TableName+'  ('
			set @Script=@Script+' ['+@ColumnName+'] '+ Case When @Is_Descending_key=0 Then ' Asc ' else ' Desc ' End
		End
		if @Key_Ordinal>1
		Begin	
			set @Script=@Script+', ['+@ColumnName+'] '+ Case When @Is_Descending_key=0 Then ' Asc ' else ' Desc ' End
		End
	End
Fetch Next From Index_cursor Into @TableName,@IndexName,@Index_id,@ColumnName,@Key_Ordinal,@Type_Desc,@Is_Primary_key,@Is_Descending_key

if (@TableName1<>@TableName Or  @IndexName1<>@IndexName)	
Begin
	if @Type_Desc='Clustered'
		set @Script='USE '+@dbname+' '+@Script+' ) '
	Else
		set @Script='USE '+@dbname+' '+@Script+' ) WITH (IGNORE_DUP_KEY = OFF)'
		
		--Insert Into #IndexTable Values (@Script)
		--print @Script
		execute sp_Executesql @Script
		set @TableName1=@TableName
		set @IndexName1=@IndexName
		set @Script=''
	
End
End
Close Index_cursor 
Deallocate Index_cursor
IF @Type_Desc<>''
BEGIN
	if @Type_Desc='Clustered'
		set @Script='USE '+@dbname+' '+@Script+' ) '
	Else
		set @Script='USE '+@dbname+' '+@Script+' ) WITH (IGNORE_DUP_KEY = OFF)'
		--	Insert Into #IndexTable Values (@Script)
		--print @Script
		execute sp_Executesql @Script
eND

Declare @KeyName Varchar(50),@Parent_Name Varchar(50),@Ref_name Varchar(50),@parent_column Varchar(50)
Declare @Ref_Column Varchar(50),@Object_Id Int --,@Script Varchar(8000)
Declare @KeyName1 Varchar(50),@Parent_Name1 Varchar(50),@Ref_name1 Varchar(50),@parent_column1 Varchar(50)
Declare @Ref_Column1 Varchar(50),@foreign_key_Str Varchar(1000),@ref_string Varchar(1000)

SET @KeyName=''
Declare ForeignKey_Cursor Cursor for
Select a.Object_id,
KeyName=a.[Name],Parent_Name=b.[Name]
,referenced_Name=c.[Name]
,parent_column=e.[Name]
,referenced_column=f.[Name] 
From Sys.Foreign_Keys a
Inner Join Sys.Objects b On (a.Parent_Object_Id=b.Object_Id)
Inner Join Sys.Objects c On (a.referenced_Object_Id=c.Object_Id)
Inner Join Sys.foreign_key_columns d On (d.Constraint_Object_Id=a.Object_Id)
Inner Join Sys.Columns e on (e.Object_Id=d.Parent_Object_Id and e.Column_Id=d.Parent_Column_Id)
Inner Join Sys.Columns f on (f.Object_Id=d.referenced_Object_Id and f.Column_Id=d.referenced_Column_Id)
Where b.[name]=@paraTable

Open ForeignKey_Cursor
Fetch Next From ForeignKey_Cursor Into @Object_Id, @KeyName, @Parent_Name, @Ref_name, @parent_column, @Ref_Column
set @KeyName1=@KeyName
set @Parent_Name1=@Parent_Name
set @Ref_name1=@Ref_name
set @parent_column1=@parent_column
set @Ref_Column1=@Ref_Column
set @foreign_key_Str='['+@parent_column+']'
set @ref_string='['+@Ref_Column+']'
While @@Fetch_Status=0
Begin
	--print @KeyName
	If @KeyName1=@KeyName and @Parent_Name1=@Parent_Name and @parent_column1<>@parent_column
	Begin
		set @ref_string=@ref_string+', ['+@Ref_Column+']'	
	End
	If @KeyName1=@KeyName and @Parent_Name1=@Parent_Name and @Ref_Column1<>@Ref_Column
	Begin
		set @foreign_key_Str=@foreign_key_Str+', ['+@parent_column+']'		
	End
	Fetch Next From ForeignKey_Cursor Into @Object_Id, @KeyName, @Parent_Name, @Ref_name, @parent_column, @Ref_Column
	if @KeyName1<>@KeyName
	Begin
		set @Script='If Not Exists (Select * From Sys.Foreign_Keys Where Object_Id=Object_Id(N'''+@KeyName1+''') and Parent_Object_Id=Object_Id(N'''+@Parent_Name1+''') ) Alter Table '+@Parent_Name1+' With Check Add Constraint '+@KeyName1+' Foreign Key '
		set @foreign_key_Str='('+@foreign_key_Str+')'
		set @ref_string='References '+@Ref_name1+' ('+@ref_string+')'
		set @Script ='USE '+@dbname+' '+@Script +@foreign_key_Str+@ref_string
		--Insert Into #IndexTable Values (@Script)
		--print @Script 
		execute sp_Executesql @Script

		set @KeyName1=@KeyName
		set @Parent_Name1=@Parent_Name
		set @Ref_name1=@Ref_name
		set @parent_column1=@parent_column
		set @Ref_Column1=@Ref_Column
		set @foreign_key_Str='['+@parent_column+']'
		set @ref_string='['+@Ref_Column+']'
	End
End
Close ForeignKey_Cursor 
Deallocate ForeignKey_Cursor 
IF @KeyName<>''
Begin
set @Script='If Not Exists (Select * From Sys.Foreign_Keys Where Object_Id=Object_Id(N'''+@KeyName1+''') and Parent_Object_Id=Object_Id(N'''+@Parent_Name1+''') ) Alter Table '+@Parent_Name1+' With Check Add Constraint '+@KeyName1+' Foreign Key '
set @foreign_key_Str='('+@foreign_key_Str+')'
set @ref_string='References '+@Ref_name1+' ('+@ref_string+')'
set @Script =@Script +@foreign_key_Str+@ref_string
End
--Insert Into #IndexTable Values (@Script)
		--print @Script
execute sp_Executesql @Script


Insert Into #IndexTable 
Select 'If Not Exists ( Select * From sys.default_constraints Where Object_Id=Object_id(N'''+a.[Name]+''') AND parent_object_id = OBJECT_ID(N'''+b.[Name]+''') ) Alter Table  '+b.[Name]+' Add Constraint '+a.[Name]+' Default '+a.Definition+' For '+c.[Name]
From sys.default_constraints a
Inner Join Sys.Objects b on (a.Parent_Object_Id=b.Object_Id)
Inner Join Sys.Columns c on (a.Parent_Object_Id=c.Object_Id and a.Parent_Column_Id=c.Column_Id)
Where b.[Name]=@paraTable

Declare CreateDefaultConstraint cursor for
Select Script From #IndexTable

Open CreateDefaultConstraint 

Fetch Next From CreateDefaultConstraint Into @Script
While @@Fetch_status=0
Begin
	--print @Script
	SET @Script='USE '+@dbname+' '+@Script
	execute sp_Executesql @Script

	Fetch Next From CreateDefaultConstraint Into @Script
End
Close CreateDefaultConstraint 
Deallocate CreateDefaultConstraint 