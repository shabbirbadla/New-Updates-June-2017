ALTER TABLE stax_mas DROP CONSTRAINT PK_stax_mas
go
execute USP_UPD_Sales_Tax_Master_RecordChecking
go
ALTER TABLE stax_mas add CONSTRAINT [PK_stax_mas] PRIMARY KEY CLUSTERED ([entry_ty] ASC,[tax_name] ASC)
WITH (IGNORE_DUP_KEY = OFF) 
ON [PRIMARY]
go

