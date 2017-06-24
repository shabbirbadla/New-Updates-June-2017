-- =============================================
-- Created By    : Priyanka Himane
-- Create Date   : 09/01/2013
-- Description   : This Stored procedure is useful to Generate data for AA ANF4B Report.
-- Modified By   :
-- Modified Date :
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_AA_ANF4B]
@APP_ID VARCHAR(50)

AS

Create Table #AA_ANF4B_REP (Srno Numeric(3,1),Sub_order Varchar(10)
,Bit_Val1 Bit
,Date_val1 SmallDateTime,Date_val2 SmallDateTime,Date_val3 SmallDateTime,Date_val4 SmallDateTime
,Num_Val1 Numeric(14,2),Num_Val2 Numeric(14,2),Num_Val3 Numeric(14,2),Num_Val4 Numeric(14,2)
,Num_Val5 Numeric(14,2),Num_Val6 Numeric(14,2),Num_Val7 Numeric(14,2),Num_Val8 Numeric(14,2)
,Str_Val1 Varchar(100),Str_Val2 Varchar(100),Str_Val3 Varchar(100),Str_Val4 Varchar(100)
,Str_Val5 Varchar(100),Str_Val6 Varchar(100)
,Txt_Val1 Text
)

-- IEC Number--start
-- (i)IEC_NO - IEC_NO will come from Manu_fact table
-- IEC Number--end

-- (Point No.:1) Applicant Details--start
-- (i)Name - Name will come from Co_mast table
-- (ii)Address - Address will come from Co_mast table
-- (Point No.:1) Applicant Details--end

-- (Point No.:2) Type of Exporter (please tick)--start
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1,Str_Val2)
Select 2,'i',typofexp,typofexp_spfy
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=2 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 2,'i'
END
-- (Point No.:2) Type of Exporter (please tick)--end

-- (Point No.:3) RCMC Details--start
-- (i)RCMC Number
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 3,'i',licen_no
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=3 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 3,'i'
END
-- (ii)Date of Issue and valid upto
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Date_Val1,Date_Val2)
Select 3,'ii',rcmc_iss_dt,rcmc_upto_dt
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=3 AND SUB_ORDER='ii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 3,'ii'
END

-- (iii)Issuing Authority
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 3,'iii',rcmc_issu_auth
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=3 AND SUB_ORDER='iii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 3,'iii'
END

-- (iv)Products for which registered
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 3,'iv',rcmc_prod_reg
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=3 AND SUB_ORDER='iv')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 3,'iv'
END
-- (Point No.:3) RCMC Details--end

-- (Point No.:4) Industrial Registration Details--start
-- (i)SSI/IEM/LOI or IL Registration Number
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 4,'i',indreg_no
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=4 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 4,'i'
END

-- (ii)Date of Issue
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Date_Val1)
Select 4,'ii',indreg_iss_dt
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=4 AND SUB_ORDER='ii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 4,'ii'
END

-- (iii)Issuing Authority
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 4,'iii',indreg_iss_auth
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=4 AND SUB_ORDER='iii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 4,'iii'
END

-- (iv)Products for which registered
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 4,'iv',indreg_prod_reg
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=4 AND SUB_ORDER='iv')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 4,'iv'
END
-- (Point No.:4) Industrial Registration Details--end

-- (Point No.:5) Excise Details (for those registered with Central Excise Authority)--start
-- (i)Excise Registration Number
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 5,'i',excreg_no
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=5 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 5,'i'
END

-- (ii)Issuing Authority
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 5,'ii',excreg_iss_auth
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=5 AND SUB_ORDER='ii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 5,'ii'
END
-- (Point No.:5) Excise Details (for those registered with Central Excise Authority)--end

-- (Point No.:6) Application Fee Details--start
-- (i)Amount (Rs)
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1)
Select 6,'i',app_fee_amt
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=6 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 6,'i'
END

-- (ii)Demand Draft/Bank Receipt/Electronic Fund Transfer No
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 6,'ii',elect_fundtran_no
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=6 AND SUB_ORDER='ii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 6,'ii'
END

-- (iii)Date of Issue
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Date_Val1)
Select 6,'iii',app_fee_dt
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=6 AND SUB_ORDER='iii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 6,'iii'
END

-- (iv)Name of the bank on which drawn
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 6,'iv',app_fee_bank
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=6 AND SUB_ORDER='iv')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 6,'iv'
END

-- (v)Bank Branch on which drawn
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 6,'v',app_fee_branch
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=6 AND SUB_ORDER='v')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 6,'v'
END
-- (Point No.:6) Application Fee Details--end

-- (Point No.:7) Total CIF Value of imports applied for--start
-- (i)In Rupees
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1)
Select 7,'i',totcif_imp_rs
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=7 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 7,'i'
END

-- (ii)In currency of imports
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1)
Select 7,'ii',totcif_imp_fc
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=7 AND SUB_ORDER='ii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 7,'ii'
END

-- (iii)In US $
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1)
Select 7,'iii',totcif_imp_usd
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=7 AND SUB_ORDER='iii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 7,'iii'
END
-- (Point No.:7) Total CIF Value of imports applied for--end

-- (Point No.:8) Export Product Details--start
-- (i)Description of Export Product
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 8,'i',exp_prod_desc
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=8 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 8,'i'
END

-- (ii)Export Product Group
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1)
Select 8,'ii',exp_prod_grp
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=8 AND SUB_ORDER='ii')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 8,'ii'
END
-- (Point No.:8) Export Product Details--end

-- (Point No.:9) Details of Items reqd. for manufacture of one unit of export product--start
-- (A)Imported Inputs
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1,Str_Val1,Str_Val2,Str_Val3,Str_Val4,Num_Val2,Str_Val5,Num_Val3,Str_Val6,Num_Val4,Num_Val5)
Select 9.1,'A',srno,point_nos,itemdesc,itemtechchar,itc_code,qty,purpose,wast_claim_per,recov_wast_name,recov_wast_qty,recov_wast_value
From aa_anf4b_mast a
Inner join aa_anf4b_detail b on (a.app_id=b.fk_primaryid)
Where b.point_nos='9_A' 
And App_Id=@APP_ID
Order by srno
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=9.1 AND SUB_ORDER='A')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 9.1,'A'
END

-- (B)Indigenous Inputs
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1,Str_Val1,Str_Val2,Str_Val3,Str_Val4,Num_Val2,Str_Val5,Num_Val3,Str_Val6,Num_Val4,Num_Val5)
Select 9.2,'B',srno,point_nos,itemdesc,itemtechchar,itc_code,qty,purpose,wast_claim_per,recov_wast_name,recov_wast_qty,recov_wast_value
From aa_anf4b_mast a
Inner join aa_anf4b_detail b on (a.app_id=b.fk_primaryid)
Where b.point_nos='9_B' 
And App_Id=@APP_ID
Order by srno
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=9.2 AND SUB_ORDER='B')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 9.2,'B'
END
-- (Point No.:9) Details of Items reqd. for manufacture of one unit of export product--end

-- (Point No.:10) Production and Consumption data of the manufacturer/supporting manufacturer for preceding 3 licensing years--start
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1,Str_Val1,Str_Val2,Num_Val2,Num_Val3,Num_Val4)
Select 10,'i',srno,point_nos,licen_year,tot_exp_prod,qty_diff_itconsum,qty_consum
From aa_anf4b_mast a
Inner join aa_anf4b_detail b on (a.app_id=b.fk_primaryid)
Where b.point_nos='10' 
And App_Id=@APP_ID
Order by srno
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=10 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 10,'i'
END
-- (Point No.:10) Production and Consumption data of the manufacturer/supporting manufacturer for preceding 3 licensing years--end

-- (Point No.:11) Details of earlier advance Authorisations obtained(if any) for the export product in the preceding 2 licensing years--start
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Num_Val1,Str_Val1,Str_Val2,Date_Val1,Str_Val3,Num_Val2,Num_Val3)
Select 11,'i',srno,point_nos,auth_no,auth_dt,iss_auth,qty_diff_itconsum,qty_consum
From aa_anf4b_mast a
Inner join aa_anf4b_detail b on (a.app_id=b.fk_primaryid)
Where b.point_nos='11'
And App_Id=@APP_ID
Order by srno
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=11 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 11,'i'
END
-- (Point No.:11) Details of earlier advance Authorisations obtained(if any) for the export product in the preceding 2 licensing years--end

-- (Point No.:12) Incase the application is made for modification in existing SION,please furnish--start
Insert Into #AA_ANF4B_REP(Srno,Sub_order,Str_Val1,Str_Val2,Str_Val3)
Select 12,'i',exist_sion_no,nature_modreqd,details_modreqd
From aa_anf4b_mast a
Where App_Id=@APP_ID
IF NOT EXISTS(SELECT * FROM #AA_ANF4B_REP WHERE SRNO=12 AND SUB_ORDER='i')
BEGIN
	INSERT INTO #AA_ANF4B_REP(SRNO,SUB_ORDER)
	SELECT 12,'i'
END
-- (Point No.:12) Incase the application is made for modification in existing SION,please furnish--end

select * from #AA_ANF4B_REP
order by srno,sub_order