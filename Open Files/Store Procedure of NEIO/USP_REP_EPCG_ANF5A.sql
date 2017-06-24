
-- =============================================
-- Created By    : Priyanka Himane
-- Create Date   : 06/09/2012
-- Description   : This Stored procedure is useful to Generate data for EPCG ANF5A Report.
-- Modified By   :
-- Modified Date :
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_EPCG_ANF5A]
@APP_ID VARCHAR(50)

AS

Create Table #EPCG_ANF5A_REP (Srno Int,Sub_order Varchar(10)
,Bit_Val1 Bit
,Date_val1 SmallDateTime,Date_val2 SmallDateTime,Date_val3 SmallDateTime,Date_val4 SmallDateTime
,Num_Val1 Numeric(14,2),Num_Val2 Numeric(14,2),Num_Val3 Numeric(14,2),Num_Val4 Numeric(14,2)
,Num_Val5 Numeric(14,2),Num_Val6 Numeric(14,2),Num_Val7 Numeric(14,2),Num_Val8 Numeric(14,2)
,Str_Val1 Varchar(100),Str_Val2 Varchar(100),Str_Val3 Varchar(100),Str_Val4 Varchar(100)
,Str_Val5 Varchar(100),Str_Val6 Varchar(100)
,Txt_Val1 Text
)

-- (Point No.:1) Applicant Details--start
-- (i)Name - Name will come from Co_mast table
-- (ii)IEC_NO - IEC_NO will come from Manu_fact table
-- (iii)Address - Address will come from Co_mast table
-- (Point No.:1) Applicant Details--end

-- (Point No.:2) Type of Exporter (please tick)--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1,Str_Val2)
Select 2,'',typofexp,typofexp_spfy
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=2 and sub_order='')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 2,''
end
-- (Point No.:2) Type of Exporter (please tick)--end

-- (Point No.:3) RCMC Details--start
-- (i)RCMC Number
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 3,'i',licen_no
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=3 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 3,'i'
end


-- (ii)Date of Issue and valid upto
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Date_Val1,Date_Val2)
Select 3,'ii',rcmc_iss_dt,rcmc_upto_dt
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=3 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 3,'ii'
end


-- (iii)Issuing Authority
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 3,'iii',rcmc_issu_auth
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=3 and sub_order='iii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 3,'iii'
end

-- (iv)Products for which registered
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 3,'iv',rcmc_prod_reg
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=3 and sub_order='iv')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 3,'iv'
end
-- (Point No.:3) RCMC Details--end

-- (Point No.:4) Industrial Registration Details--start
-- (i)SSI/IEM/LOI or IL Registration Number
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 4,'i',indreg_no
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=4 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 4,'i'
end

-- (ii)Date of Issue
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Date_Val1)
Select 4,'ii',indreg_iss_dt
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=4 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 4,'ii'
end

-- (iii)Issuing Authority
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 4,'iii',indreg_iss_auth
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=4 and sub_order='iii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 4,'iii'
end

-- (iv)Products for which registered
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 4,'iv',indreg_prod_reg
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=4 and sub_order='iv')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 4,'iv'
end
-- (Point No.:4) Industrial Registration Details--end

-- (Point No.:5) Service Tax Registration Details (in case of service providers registered with service Tax authorities)--start
-- (i)Service Tax Registration Number
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 5,'i',serreg_no
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=5 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 5,'i'
end

-- (ii)Issuing Authority
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 5,'ii',serreg_iss_auth
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=5 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 5,'ii'
end

-- (iii)Services for which registered
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 5,'iii',serreg_prod_reg
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=5 and sub_order='iii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 5,'iii'
end
-- (Point No.:5) Service Tax Registration Details (in case of service providers registered with service Tax authorities)--end

-- (Point No.:6) Status House Details--start
-- (i)EH/SEH/TH/STH/PTH
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 6,'i',status_house
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=6 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 6,'i'
end

-- (ii)Certificate Number
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 6,'ii',status_no
From epcg_anf5a_mast a
Where App_Id in (Select fk_primaryid From epcg_anf5a_detail)
And App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=6 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 6,'ii'
end

-- (iii)Date of Issue and valid upto
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Date_Val1,Date_Val2)
Select 6,'iii',status_iss_dt,status_upto_dt
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=6 and sub_order='iii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 6,'iii'
end

-- (iv)Issuing Authority
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 6,'iv',status_issu_auth
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=6 and sub_order='iv')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 6,'iv'
end
-- (Point No.:6) Status House Details--end

-- (Point No.:7) Excise Details (for those registered with Central Excise Authority)--start
-- (i)Excise Registration Number
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 7,'i',excreg_no
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=7 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 7,'i'
end

-- (ii)Date of Issue/Issuing Authority
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 7,'ii',excreg_iss_auth_dt
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=7 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 7,'ii'
end
-- (Point No.:7) Excise Details (for those registered with Central Excise Authority)--end

-- (Point No.:8) Application Fee Details--start
-- (i)Amount (Rs)
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1)
Select 8,'i',app_fee_amt
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=8 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 8,'i'
end

-- (ii)Electronic Fund Transfer No
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 8,'ii',elect_fundtran_no
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=8 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 8,'ii'
end
-- (Point No.:8) Application Fee Details--end

-- (Point No.:9) Sector Classification of Capital Goods sought to be imported under the Scheme (Please Tick)--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 9,'',sector_typ
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=9 and sub_order='')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 9,''
end
-- (Point No.:9) Sector Classification of Capital Goods sought to be imported under the Scheme (Please Tick)--end

-- (Point No.:10) Product to be exported/services to be rendered.--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 10,'',prodserv_rendrd
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=10 and sub_order='')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 10,''
end
-- (Point No.:10) Product to be exported/services to be rendered.--end

-- (Point No.:11) Whether imports to be made are under Technological Upgration Scheme  Yes/No If Yes, give following details:--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Bit_Val1)
Select 11,'',upgrd_yes_no
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=11 and sub_order='')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 11,''
end

Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Str_Val1,Date_Val1,Num_Val2,Num_Val3,Date_Val2,Num_Val4)
Select 11,'i',srno,auth_no,auth_dt,duty_saved,eo_convcurr,eo_expiry_dt,upgrd_eo_perage
From epcg_anf5a_detail a
Inner Join epcg_anf5a_mast b on (b.app_id=a.fk_primaryid)
Where point_nos=11
And App_Id=@APP_ID
Order by srno

If not exists(Select srno from #EPCG_ANF5A_REP where srno=11 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 11,'i'
end
-- (Point No.:11) Whether imports to be made are under Technological Upgration Scheme  Yes/No If Yes, give following details:--end

-- (Point No.:12) Details of exports of same/similar product/services made in the preceding 3 licensing years (excluding exports againts all pending EPCG Authorizations)--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Str_Val1,Num_Val2,Num_Val3)
Select 12,'i',srno,export_finyr,export_totfob,export_avgperf
From epcg_anf5a_detail a
Inner Join epcg_anf5a_mast b on (b.app_id=a.fk_primaryid)
Where point_nos=12
And App_Id=@APP_ID
Order by srno

If not exists(Select srno from #EPCG_ANF5A_REP where srno=12 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 12,'i'
end

-- (Point No.:12) Details of exports of same/similar product/services made in the preceding 3 licensing years (excluding exports againts all pending EPCG Authorizations)--end

-- (Point No.:13) Details of pending EPCG Authorization already obtained--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Str_Val1,Date_Val1,Str_Val2,Num_Val2,Num_Val3,Num_Val4,Num_Val5,Num_Val6,Num_Val7,Date_Val2)
Select 13,'i',srno,auth_no,auth_dt,pend_ra_iss_auth,duty_saved,eo_convcurr,pend_eo_fulfld_dutysavd,pend_eo_ann_avg,pend_eo_avg_duetillast_finyr
,pend_eo_avg_fulfldtillast_finyr,eo_expiry_dt
From epcg_anf5a_detail a
Inner Join epcg_anf5a_mast b on (b.app_id=a.fk_primaryid)
Where point_nos=13
And App_Id=@APP_ID
Order by srno

If not exists(Select srno from #EPCG_ANF5A_REP where srno=13 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 13,'i'
end
-- (Point No.:13) Details of pending EPCG Authorization already obtained--end

-- (Point No.:14) Details of Freely Importable Capital Goods applied for import--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Str_Val1,Str_Val2,Str_Val3,Num_Val2,Str_Val4)
Select 14,'i',srno,itemdesc,itc_code,natureofcapg,qty,primuseofcapg
From epcg_anf5a_detail a
Inner Join epcg_anf5a_mast b on (b.app_id=a.fk_primaryid)
Where point_nos=14
And App_Id=@APP_ID
Order by srno

If not exists(Select srno from #EPCG_ANF5A_REP where srno=14 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 14,'i'
end
-- (Point No.:14) Details of Freely Importable Capital Goods applied for import--end

-- (Point No.:15) Details of Restricted Capital Goods applied for import--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Str_Val1,Str_Val2,Str_Val3,Str_Val4,Str_Val5,Num_Val2,Num_Val3)
Select 15,'i',srno,itemdesc,itc_code,natureofcapg,primuseofcapg,import_restrict_techmodel,qty,import_restrict_cif_convcurr
From epcg_anf5a_detail a
Inner Join epcg_anf5a_mast b on (b.app_id=a.fk_primaryid)
Where point_nos=15
And App_Id=@APP_ID
Order by srno

If not exists(Select srno from #EPCG_ANF5A_REP where srno=15 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 15,'i'
end
-- (Point No.:15) Details of Restricted Capital Goods applied for import--end

-- (Point No.:16) Details of Duty Saved--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Num_Val2,Num_Val3,Num_Val4,Num_Val5)
Select 16,'i',dutysvd_totcustdty,dutysvd_duty_levied,dutysvd_duty_savedper,dutysvd_cif_import,dutysvd_duty_savedrs
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=16 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 16,'i'
end
-- (Point No.:16) Details of Duty Saved--end

-- (Point No.:17) Details of Export obligation and Average Export Obligation to be imposed:--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Num_Val1,Num_Val2,Num_Val3,Num_Val4,Num_Val5,Num_Val6,Num_Val7,Num_Val8)
Select 17,'i',eoavgeo_totdutysvd,eoavgeo_eoimposd_6_rs,eoavgeo_eoimposd_6_usd
,eoavgeo_eoimposd_8_rs,eoavgeo_eoimposd_8_usd,eoavgeo_avgeoimposd_rs,eoavgeo_avgeoimposd_usd,eoavgeo_eoperdimposd
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=17 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 17,'i'
end
-- (Point No.:17) Details of Export obligation and Average Export Obligation to be imposed:--end

-- (Point No.:18) Port of Registration (for the purpose of import):--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 18,'i',port_reg
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=18 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 18,'i'
end
-- (Point No.:18) Port of Registration (for the purpose of import):--end

-- (Point No.:19) Address of Factory/Premises of the applicant where the Capital goods to be imported are proposed to be installed--start
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Txt_Val1)
Select 19,'i',fact_add
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=19 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 19,'i'
end
-- (Point No.:19) Address of Factory/Premises of the applicant where the Capital goods to be imported are proposed to be installed--end

-- (Point No.:20) In case the proposed CG sought to be imported are to be used by the suppoting manufacturer ,Please furnish--start
-- (i)Name of the supporting manufacturer
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 20,'i',supmfg_nm
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=20 and sub_order='i')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 20,'i'
end

-- (ii)Address of the supporting manufacturer
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Txt_Val1)
Select 20,'ii',supmfg_add
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=20 and sub_order='ii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 20,'ii'
end

-- (iii)SSI/LOI/IL regn.no.and date
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1,Date_Val1)
Select 20,'iii',supmfg_regno,supmfg_regdt
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=20 and sub_order='iii')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 20,'iii'
end

-- (iv)Products endorsed on SSI/IL/IEM
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1)
Select 20,'iv',supmfg_prod_end
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=20 and sub_order='iv')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 20,'iv'
end

-- (v)Excise Registration No.and issuing authority.(if applicable)
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order,Str_Val1,Str_Val2)
Select 20,'v',supmfg_exreg_no,supmfg_exiss_auth
From epcg_anf5a_mast a
Where App_Id=@APP_ID

If not exists(Select srno from #EPCG_ANF5A_REP where srno=20 and sub_order='v')
begin
Insert Into #EPCG_ANF5A_REP(Srno,Sub_order)
Select 20,'v'
end
-- (Point No.:20) In case the proposed CG sought to be imported are to be used by the suppoting manufacturer ,Please furnish--end

select * from #EPCG_ANF5A_REP
order by srno,sub_order
