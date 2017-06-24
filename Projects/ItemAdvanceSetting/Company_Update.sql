/*TKT-6392---->*/
IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[It_Advance_Setting_Master]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[It_Advance_Setting_Master](
	[It_Adv_Code] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TabCaption] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TabCode] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[sCaption] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[sDetails] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[fld_nm] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[sorder] [int] NULL
) ON [PRIMARY]
end
go

IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[It_Advance_Setting]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[It_Advance_Setting](
	[it_code] [numeric](18, 0) NOT NULL,
	[PACKSIZE1] [bit]  DEFAULT ((0))
) ON [PRIMARY]
end
Go

if not exists (select [It_Adv_Code] from [It_Advance_Setting_Master] where [It_Adv_Code]='V1')
begin
	INSERT INTO [It_Advance_Setting_Master]([It_Adv_Code], [TabCaption], [TabCode], [sCaption], [sDetails], [fld_nm], [sorder]) 
	VALUES ('V1','Advance Setting','TB1','Pack Size 1','DO ueitvariantmaster.app WITH thisform,"V1"','PACKSIZE1', 1)
end

if not exists (select e_code from Lother where e_code='V1' and fld_nm='MRPRATE')
begin
	INSERT INTO lother([e_code], [head_nm], [fld_nm], [data_ty], [fld_wid], [fld_dec], [att_file], [whn_con], [val_con], [val_err], [defa_val], [filtcond], [serial], [heading], [lines], [formno], [inter_use], [bef_aft], [mandatory], [com_type], [inpickup], [user_name], [sysdate], [ingrid], [tbl_nm], [inf11], [Pickedit], [Passroute], [Type], [Remarks], [defa_fld], [CompId], [Validity], [pop_tblnm], [pop_fldnm], [hotkey], [tbl_nm1], [formdesc], [inptmask], [SubSerial], [E_], [n_], [r_], [t_], [i_], [o_], [b_], [x_], [xtvs_], [dsni_], [mcur_], [tds_]) 
	VALUES ('V1','MRP Rate','MRPRATE','Decimal', 10, 2, 1,'','','','','', 1,'', 0, 0, 0, 0, 0,'', 0,'ADMIN','02/15/11 15:38:41', 0,'Ite', 0, 0,'','M','', 0, 0,'','','','','','','', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
end
/*<---TKT-6392*/
/*TKT-6393---->*/
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VDM_MAST]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[VDM_MAST](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Fld_Nm] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PreFix] [varchar](7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Sufix] [varchar](7) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[fwidth] [numeric](2, 0) NULL
	) ON [PRIMARY]
end
go
IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VRM_Mast]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[VRM_Mast](
	[Code] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[fld_nm] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Caption] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Expr] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[fld_list] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[fwidth] [numeric](2, 0) NULL
	) ON [PRIMARY]
end 
go
IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item_Variant_Master]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[Item_Variant_Master](
	[It_Code] [numeric](18, 0) NULL,
	[var_code] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[compid] [int] NULL,
	[MRPRATE] [decimal](10, 2) NULL CONSTRAINT [DF__Item_Vari__MRPRA__5C39435B]  DEFAULT ((0)),
	[It_varCode] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[PACKSIZE1] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT (''),
	[Pack1] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT ('')
	) ON [PRIMARY]
end
go
if not exists (select caption from VDM_Mast where [Fld_Nm]='Pack1')
begin
	INSERT INTO VDM_Mast( [Caption], [Fld_Nm], [PreFix], [Sufix], [fwidth]) 
	VALUES ('Packing','Pack1','','kg', 3)
end
if not exists (select caption from VRM_Mast where [Fld_Nm]='PACKSIZE1')
begin
	INSERT INTO VRM_Mast([Code], [fld_nm], [Caption], [Expr], [fld_list], [fwidth]) 
	VALUES ('V1','PACKSIZE1','Pack Size 1','Packing kg','Pack1', 30)
end
/*<---TKT-6393*/