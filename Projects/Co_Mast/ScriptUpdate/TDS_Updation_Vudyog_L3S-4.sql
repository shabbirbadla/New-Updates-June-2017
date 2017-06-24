use vudyog

/* Add Dedctors TDS Category Related Info. Start */
IF  not EXISTS (SELECT [NAME] FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TDSdeductor_Category]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[TDSdeductor_Category](
	[category] [varchar](60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[code] [varchar](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	) ON [PRIMARY]
end
Go

IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='A' ) begin insert into [TDSdeductor_Category] (category,code) values('Central Government','A') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='S' ) begin insert into [TDSdeductor_Category] (category,code) values('State Government','S') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='D' ) begin insert into [TDSdeductor_Category] (category,code) values('Statutory body (Central Govt.)','D') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='E' ) begin insert into [TDSdeductor_Category] (category,code) values('Statutory body (State Govt.)','E') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='G' ) begin insert into [TDSdeductor_Category] (category,code) values('Autonomous body (Central Govt.)','G') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='H' ) begin insert into [TDSdeductor_Category] (category,code) values('Autonomous body (State Govt.)','H') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='L' ) begin insert into [TDSdeductor_Category] (category,code) values('Local Authority (Central Govt.)','L') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='N' ) begin insert into [TDSdeductor_Category] (category,code) values('Local Authority (State Govt.)','N') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='K' ) begin insert into [TDSdeductor_Category] (category,code) values('Company','K') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='M' ) begin insert into [TDSdeductor_Category] (category,code) values('Branch / Division of Company','M') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='P' ) begin insert into [TDSdeductor_Category] (category,code) values('Association of Person (AOP)','P') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='T' ) begin insert into [TDSdeductor_Category] (category,code) values('Association of Person (Trust)','T') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='J' ) begin insert into [TDSdeductor_Category] (category,code) values('Artificial Juridical Person','J') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='B' ) begin insert into [TDSdeductor_Category] (category,code) values('Body of Individuals','B') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='Q' ) begin insert into [TDSdeductor_Category] (category,code) values('Individual/HUF','Q') end
IF  not EXISTS (SELECT [CODE] FROM [TDSdeductor_Category] where code ='F' ) begin insert into [TDSdeductor_Category] (category,code) values('Firm','F') end
Go
/* Add Dedctors TDS Category Related Info. Start */

/* Add Dedctors TDS Ministry Master Info. Start */
IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TDS_Ministry_Master]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[TDS_Ministry_Master](
		[Ministry_name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Ministry_Code] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	) ON [PRIMARY]
end
GO
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='1' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Agriculture','1') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='2' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Atomic Energy','2') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='3' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Fertilizers','3') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='4' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Chemicals and Petrochemicals','4') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='5' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Civil Aviation and Tourism','5') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='6' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Coal','6') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='7' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Consumer Affairs, Food and Public Distribution','7') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='8' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Commerce and Textiles','8') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='9' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Environment and Forests and Ministry of Earth Science','9') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='10' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('External Affairs and Overseas Indian Affairs','10') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='11' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Finance','11') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='12' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Central Board of Direct Taxes','12') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='13' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Central Board of Excise and Customs','13') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='14' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Contoller of Aid Accounts and Audit','14') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='15' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Central Pension Accounting Office','15') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='16' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Food Processing Industries','16') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='17' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Health and Family Welfare','17') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='18' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Home Affairs and Development of North Eastern Region','18') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='19' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Human Resource Development','19') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='20' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Industry','20') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='21' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Information and Broadcasting','21') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='22' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Telecommunication and Information Technology','22') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='23' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Labour','23') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='24' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Law and Justice and Company Affairs','24') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='25' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Personnel, Public Grievances and Pesions','25') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='26' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Petroleum and Natural Gas','26') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='27' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Plannning, Statistics and Programme Implementation','27') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='28' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Power','28') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='29' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('New and Renewable Energy','29') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='30' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Rural Development and Panchayati Raj','30') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='31' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Science And Technology','31') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='32' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Space','32') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='33' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Steel','33') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='34' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Mines','34') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='35' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Social Justice and Empowerment','35') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='36' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Tribal Affairs','36') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='37' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('D/o Commerce (Supply Division)','37') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='38' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Shipping and Road Transport and Highways','38') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='39' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Urban Development, Urban Employment and Poverty Alleviation','39') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='40' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Water Resources','40') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='41' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('President''s Secretariat','41') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='42' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Lok Sabha Secretariat','42') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='43' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Rajya Sabha secretariat','43') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='44' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Election Commission','44') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='45' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Ministry of Defence (Controller General of Defence Accounts)','45') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='46' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Ministry of Railways','46') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='47' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Department of Posts','47') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='48' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Department of Telecommunications','48') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='49' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Andaman and Nicobar Islands Administration   ','49') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='50' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Chandigarh Administration','50') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='51' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Dadra and Nagar Haveli','51') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='52' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Goa, Daman and Diu','52') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='53' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Lakshadweep','53') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='54' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Pondicherry Administration','54') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='55' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Pay and Accounts Officers (Audit)','55') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='56' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Ministry of Non-conventional energy sources ','56') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='57' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Government Of NCT of Delhi ','57') end
IF  not EXISTS (SELECT [Ministry_CODE] FROM [TDS_Ministry_Master] where Ministry_code ='99' ) begin insert into [TDS_Ministry_Master] (Ministry_Name,Ministry_Code) values('Others','99') end

/* Add Dedctors TDS Ministry Master Info. End */

/* Add CoAdditional Table Start */
IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TDSCoAdditional]') AND type in (N'U'))
begin 
	CREATE TABLE [dbo].[TDSCoAdditional](
	[ded_type] [varchar](60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT (''),
	[Ministry_Name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT (''),
	[PAO_CODE] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT (''),
	[PAO_REGNO] [int] NULL DEFAULT (''),
	[DDO_CODE] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT (''),
	[DDO_REGNO] [int] NULL DEFAULT ('')
	) 
end
select ministry_name from [TDSCoAdditional]
if (@@rowcount=0)
begin
	insert into [TDSCoAdditional] ([ded_type],[Ministry_Name],[PAO_CODE],[PAO_REGNO],[DDO_CODE],[DDO_REGNO]) values ('','','',0,'',0)
end
/* Add CoAdditional Table end */
