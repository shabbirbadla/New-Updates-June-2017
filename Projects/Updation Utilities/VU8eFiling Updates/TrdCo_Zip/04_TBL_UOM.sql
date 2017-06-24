IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uom]') AND type in (N'U'))
DROP TABLE [dbo].[uom]
--GO

CREATE TABLE [dbo].[uom](
	[u_uom] [varchar](6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[e_uom] [varchar](6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO


INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('BOX ', 'BOX ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('CM3 ', 'CCM ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('CM3 ', 'CCMS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('CM3 ', 'CM  ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('CM3', 'CM3')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('CM3 ', 'CMS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('G   ', 'GM  ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('G   ', 'GMS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('G   ', 'GRAM')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('KG  ', 'KG  ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('KG  ', 'KG. ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('KG  ', 'KGS ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('KG  ', 'KGS.')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('KL  ', 'KL  ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('KL  ', 'KLTR')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('L   ', 'LTR ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('L   ', 'LTR.')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('L   ', 'LTRS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('M   ', 'MTR ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('M   ', 'MTR.')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('M   ', 'MTRS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('M2  ', 'SQM')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('M3  ', 'CMTR')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('M3', 'M3')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MM', 'MM')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MM  ', 'MMS ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MT  ', 'M.T.')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MT  ', 'MT  ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MT  ', 'MT. ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MT  ', 'MTON')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('MT  ', 'MTS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('PA  ', 'PAIR')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('PA  ', 'PRS ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('PA  ', 'PRS.')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('T   ', 'TON')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('U   ', 'NO  ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('U   ', 'NO. ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('U   ', 'NOS ')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('U   ', 'NOS.')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('U   ', 'PCS')

INSERT INTO uom([e_uom], [u_uom]) 
VALUES ('U   ', 'PCS.')

