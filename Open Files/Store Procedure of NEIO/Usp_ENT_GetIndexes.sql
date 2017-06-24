CREATE Procedure Usp_ENT_GetIndexes
@tbl_nm Varchar(50)
as 
select 
TABLE_CAT=DB_NAME() ,
TABLE_SCHEM = sc.name,
TABLE_NAME          = o.name ,
NON_UNIQUE          = convert(smallint, 1 - is_identity),
INDEX_NAME          = i.name,
TYPE                = case when i.type_desc='CLUSTERED' then 1 else 3 end,
ORDINAL_POSITION    = ic.key_ordinal,
COLUMN_NAME         = c.name,
ASC_OR_DESC         = case when is_descending_key=1 then 'D' else 'A' end
FROM sys.indexes i
INNER JOIN sys.objects o ON i.object_id = o.object_id
INNER JOIN sys.schemas sc ON o.schema_id = sc.schema_id
INNER JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
INNER JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
WHERE i.name IS NOT NULL
AND o.type = 'U'
and o.name=@tbl_nm and is_unique=0
ORDER BY o.name, i.type