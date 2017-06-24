IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ER_EXCISE]') AND type in (N'U'))
DROP TABLE [dbo].[ER_EXCISE]
--GO

CREATE TABLE [dbo].[ER_EXCISE](
	[SRNO] [int] NOT NULL,
	[PER] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DUTY] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DESC] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AC_NAME] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SHORTNM] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AC_ID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CRAC] [bit] NULL,
	[CURAC] [bit] NULL,
	[CompId] [int] NULL
) ON [PRIMARY]
GO


INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (1, 'U_BASDUTY           ', 'EXAMT               ', 'CENVAT                                            ', '                                   ', '                    ', '63', 0, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (2, 'U_CESSPER           ', 'U_CESSAMT           ', 'Education Cess on Excisable Goods                 ', '                                   ', '                    ', '63', 0, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (1, ' ', ' ', 'CENVAT', 'BALANCE WITH EXCISE RG23A-II', 'CENVAT', '25', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (1, ' ', ' ', 'CENVAT', 'BALANCE WITH EXCISE RG23C-II', 'CENVAT', '26', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (2, ' ', ' ', 'AED (TTA)', 'BALANCE WITH EXCISE TNTA A/C', 'AED (TTA)', '42', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (7, ' ', ' ', 'NCCD PLA', 'BALANCE WITH EXCISE NCCD PLA A/C', 'NCCD', '40', 0, 1, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (4, '', '', 'AED PLA', 'BALANCE WITH EXCISE AED PLA A/C', 'AED', '38', 0, 1, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (6, '', '', 'CVD', 'BALANCE WITH CVD RG23A', 'Addl. Duty', '53', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (6, '', '', 'CVD', 'BALANCE WITH CVD RG23C', 'Addl. Duty', '54', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (7, '', '', 'Education Cess on Excisable Goods', 'BALANCE WITH CESS SURCHARGE RG23A', 'Education Cess on Excisable Goods', '45', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (7, '', '', 'Education Cess on Excisable Goods', 'BALANCE WITH CESS SURCHARGE RG23C', 'Education Cess on Excisable Goods', '46', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (9, '', '', 'SERVICE TAX', 'BALANCE WITH SERVICE TAX A/C', 'SERVICE TAX', '48', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (10, '', '', 'Education Cess on Taxable Services', 'BALANCE WITH SERVICE TAX CESS A/C', 'Education Cess on Taxable Services', '49', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (1, ' ', ' ', 'CENVAT PLA', 'BALANCE WITH EXCISE PLA', 'CENVAT', '24', 0, 1, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (2, ' ', ' ', 'Education Cess on Excisable Goods', 'BALANCE WITH CESS SURCHARGE PLA', 'Education Cess on Excisable Goods', '44', 0, 1, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (5, '', '', 'BCD', 'BALANCE WITH BCD RG23A', 'CVD', '252', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (2, ' ', ' ', 'AED (TTA)', 'BALANCE WITH EXCISE TNTA A/C', 'AED (TTA)', '111', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (5, ' ', ' ', 'AED (TTA) PLA', 'BALANCE WITH EXCISE TNTA PLA A/C', 'AED (TTA)', '43', 0, 1, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (3, ' ', ' ', 'NCCD', 'BALANCE WITH EXCISE NCCD A/C', 'NCCD', '39', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (3, ' ', ' ', 'NCCD', 'BALANCE WITH EXCISE NCCD A/C', 'NCCD', '109', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (4, '', '', 'AED', 'BALANCE WITH EXCISE AED A/C', 'AED', '37', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (4, '', '', 'AED', 'BALANCE WITH EXCISE AED A/C', 'AED', '108', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (5, '', '', 'BCD', 'BALANCE WITH BCD RG23C', 'CVD', '252', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (5, '', '', 'SAD', 'BALANCE WITH SAD RG23A', 'SAD', '', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (3, '', '', 'S & H Education Cess ', 'BALANCE WITH HCESS PLA', 'S & H Education Cess ', '107', 0, 1, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (8, '', '', 'S & H Education Cess ', 'BALANCE WITH HCESS RG23A', 'S & H Education Cess ', '112', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (8, '', '', 'S & H Education Cess ', 'BALANCE WITH HCESS RG23C', 'S & H Education Cess ', '113', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (11, '', '', 'S & H Education Cess on Taxable Services', 'BALANCE WITH SERVICE TAX HCESS A/C', 'S & H Education Cess on Taxable Services', '114', 1, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (3, 'U_HCESSPER', 'U_HCESAMT', 'S & H Education Cess ', '                                   ', '                    ', '112', 0, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (4, 'U_CVDPER', 'U_CVDAMT', 'Addl. Duty', '                                   ', 'Addl. Duty', '53', 0, 0, 0)

INSERT INTO er_excise([SRNO], [PER], [DUTY], [DESC], [AC_NAME], [SHORTNM], [AC_ID], [CRAC], [CURAC], [CompId]) 
VALUES (5, '', '', 'SAD', 'BALANCE WITH SAD RD23C', 'SAD', '', 1, 0, 0)

