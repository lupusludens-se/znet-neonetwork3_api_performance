DELETE FROM [dbo].[Resource_Technology]
DELETE FROM [dbo].[Resource_Category]
DELETE FROM [dbo].[Resource]
GO

SET IDENTITY_INSERT [dbo].[Resource] ON 

INSERT INTO [dbo].[Resource] (
	[Resource_Id], [Content_Title], [Reference_Url], [Type_Id], [Created_Ts], [Last_Change_Ts],[Article_Id],[Tool_Id]
	)
VALUES 
    (1, 'Addressing Scope 1 Emissions', '', 2, GETDATE(), GETDATE(), 819, NULL),
    (2, 'Fuel Cell Overview: Bloom Energy', '', 1, GETDATE(), GETDATE(), 595, NULL),
    (4, 'Corporate PPAs: The Collaborative Model', '', 1, GETDATE(), GETDATE(), 455, NULL),
    (5, 'DER Map', '', 4, GETDATE(), GETDATE(), NULL, 7),
    (6, 'What are Scope 1 Emissions?', '', 2, GETDATE(), GETDATE(), 571, NULL),
    (7, 'Energy Attribute Certificate (EAC) - Global Indicative Pricing Dashboard', '', 4, GETDATE(), GETDATE(), NULL, 2),
    (8, 'Energy Efficiency 101', '', 1, GETDATE(), GETDATE(), 465, NULL),
    (9, 'Energy Storage 101', '', 1, GETDATE(), GETDATE(), 460, NULL),
    (10, 'FAQ for Considering Onsite Solar', '', 1, GETDATE(), GETDATE(), 425, NULL),
    (11, 'What is a Fuel Cell?', '', 2, GETDATE(), GETDATE(), 573, NULL),
    (13, 'What is Battery Storage?', '', 2, GETDATE(), GETDATE(), 575, NULL),
    (14, 'Collaborative Offsite PPA Structures', '', 1, GETDATE(), GETDATE(), 470, NULL),
    (15, 'Community Solar 101', '', 3, GETDATE(), GETDATE(), 577, NULL),
    (16, 'The Disruptive Power of Fleet Electrification', '', 1, GETDATE(), GETDATE(), 473, NULL),
    (17, 'Moving Organizations to Carbon Neutrality: The Role of Carbon Offsets', '', 1, GETDATE(), GETDATE(), 481, NULL),
    (18, 'Power Purchase Agreements (PPAs) 101', '', 1, GETDATE(), GETDATE(), 556, NULL),
    (20, 'VPPA 201: Accounting', '', 1, GETDATE(), GETDATE(), 562, NULL),
    (21, 'VPPA 201: Finance', '', 1, GETDATE(), GETDATE(), 553, NULL),
    (22, 'Onsite solar request tool', '', 5, GETDATE(), GETDATE(), NULL, 1),
    (23, 'PPA Tracker', '', 4, GETDATE(), GETDATE(), NULL, 6),
    (24, 'Recovering Heat from Exhaust Air', '', 1, GETDATE(), GETDATE(), 534, NULL),
    (25, 'What is a Physical PPA?', '', 1, GETDATE(), GETDATE(), 801, NULL),
    (26, 'Strategy Calculator', '', 4, GETDATE(), GETDATE(), NULL, 8),
    (27, 'The Decarbonization Challenge Part 1: Closing the Ambition to Action Gap', '', 1, GETDATE(), GETDATE(), 538, NULL),
    (28, 'The Decarbonization Challenge Part 2: Getting it Done', '', 1, GETDATE(), GETDATE(), 542, NULL),
    (29, 'The Role of RECs and Additionality', '', 1, GETDATE(), GETDATE(), 526, NULL),
    (30, 'Understanding Renewable Energy Certificates in Europe', '', 1, GETDATE(), GETDATE(), 531, NULL),
    (31, 'Utility Green Tariff Overview', '', 1, GETDATE(), GETDATE(), 593, NULL),
    (33, 'What is Community Solar?', '', 2, GETDATE(), GETDATE(), 585, NULL),
    (34, 'What is a Green Tariff?', '', 2, GETDATE(), GETDATE(), 587, NULL),
    (35, 'Renewable Retail 101', '', 2, GETDATE(), GETDATE(), 589, NULL),
    (36, 'What is an Efficiency Audit', '', 2, GETDATE(), GETDATE(), 565, NULL),
    (37, 'What is Distributed Generation?', '', 1, GETDATE(), GETDATE(), 560, NULL),
	(39, 'Getting Started with EV Charging', '', 1, GETDATE(), GETDATE(), 815, NULL),
	(40, 'What are Carbon Offsets?', '', 1, GETDATE(), GETDATE(), 812, NULL),
	(41, 'The Colors of Hydrogen', '', 1, GETDATE(), GETDATE(), 880, NULL),
	(42, 'What is Green Hydrogen?', '', 1, GETDATE(), GETDATE(), 877, NULL)


SET IDENTITY_INSERT [dbo].[Resource] OFF
GO

INSERT INTO [dbo].[Resource_Technology]
           ([Resource_Id]
           ,[Technology_Id]
           ,[Created_Ts]
		   ,[Last_Change_Ts])
     VALUES
           (2,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'fuel-cell'),GETDATE(),GETDATE())
		   ,(6,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'emerging-technology'),GETDATE(),GETDATE())
		   ,(8,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'building-controls'),GETDATE(),GETDATE())
		   ,(8,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'building-envelope'),GETDATE(),GETDATE())
		   ,(8,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'lighting'),GETDATE(),GETDATE())
		   ,(8,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'hvac'),GETDATE(),GETDATE())
		   ,(9,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'battery-storage'),GETDATE(),GETDATE())
		   ,(10,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'rooftop-solar'),GETDATE(),GETDATE())
		   ,(10,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'groundmount-solar'),GETDATE(),GETDATE())
		   ,(10,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'carport-solar'),GETDATE(),GETDATE())
		   ,(11,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'fuel-cell'),GETDATE(),GETDATE())
		   ,(13,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'battery-storage'),GETDATE(),GETDATE())
		   ,(36,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'building-controls'),GETDATE(),GETDATE())
		   ,(36,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'building-envelope'),GETDATE(),GETDATE())
		   ,(36,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'lighting'),GETDATE(),GETDATE())
		   ,(36,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'hvac'),GETDATE(),GETDATE())
		   ,(16,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'ev-charging'),GETDATE(),GETDATE())
		   ,(22,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'rooftop-solar'),GETDATE(),GETDATE())
		   ,(22,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'groundmount-solar'),GETDATE(),GETDATE())
		   ,(22,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'carport-solar'),GETDATE(),GETDATE())
		   ,(23,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'onshore-wind'),GETDATE(),GETDATE())
		   ,(23,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'offshore-wind'),GETDATE(),GETDATE())
		   ,(41,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'green_hydrogen'),GETDATE(),GETDATE())
		   ,(42,(SELECT [CMS_Technology_Id] FROM [dbo].[CMS_Technology] WHERE [Technology_Slug] = 'green_hydrogen'),GETDATE(),GETDATE())
GO

INSERT INTO [dbo].[Resource_Category]
           ([Resource_Id]
           ,[Category_Id]
           ,[Created_Ts]
		   ,[Last_Change_Ts])
     VALUES
           (1,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'emerging-technologies'),GETDATE(),GETDATE())
           ,(2,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'fuel-cells'),GETDATE(),GETDATE())
		   ,(4,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'aggregated-ppas'),GETDATE(),GETDATE())
		   ,(5,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'onsite-solar'),GETDATE(),GETDATE())
		   ,(6,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'emerging-technologies'),GETDATE(),GETDATE())
		   ,(7,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'eac-purchasing'),GETDATE(),GETDATE())
		   ,(8,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'efficiency-audits-consulting'),GETDATE(),GETDATE())
		   ,(8,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'efficiency-equipment-measures'),GETDATE(),GETDATE())
		   ,(9,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'battery-storage'),GETDATE(),GETDATE())
		   ,(10,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'onsite-solar'),GETDATE(),GETDATE())
		   ,(11,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'fuel-cells'),GETDATE(),GETDATE())
		   ,(36,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'efficiency-equipment-measures'),GETDATE(),GETDATE())
		   ,(13,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'battery-storage'),GETDATE(),GETDATE())
		   ,(14,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'aggregated-ppas'),GETDATE(),GETDATE())
		   ,(15,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'community-solar'),GETDATE(),GETDATE())
		   ,(16,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'ev-charging-fleet-electrification'),GETDATE(),GETDATE())
		   ,(17,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'carbon-offset-purchasing'),GETDATE(),GETDATE())
		   ,(18,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'offsite-power-purchase-agreement'),GETDATE(),GETDATE())
		   ,(18,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'aggregated-ppas'),GETDATE(),GETDATE())
		   ,(20,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'offsite-power-purchase-agreement'),GETDATE(),GETDATE())
		   ,(21,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'offsite-power-purchase-agreement'),GETDATE(),GETDATE())
		   ,(22,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'onsite-solar'),GETDATE(),GETDATE())
		   ,(24,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'efficiency-equipment-measures'),GETDATE(),GETDATE())
		   ,(25,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'renewable-retail-electricity'),GETDATE(),GETDATE())
		   ,(26,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'offsite-power-purchase-agreement'),GETDATE(),GETDATE())
		   ,(27,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'ev-charging-fleet-electrification'),GETDATE(),GETDATE())
		   ,(28,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'ev-charging-fleet-electrification'),GETDATE(),GETDATE())
		   ,(29,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'eac-purchasing'),GETDATE(),GETDATE())
		   ,(30,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'eac-purchasing'),GETDATE(),GETDATE())
		   ,(31,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'utility-green-tariff'),GETDATE(),GETDATE())
		   ,(33,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'community-solar'),GETDATE(),GETDATE())
		   ,(34,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'utility-green-tariff'),GETDATE(),GETDATE())
		   ,(35,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'renewable-retail-electricity'),GETDATE(),GETDATE())
		   ,(36,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'efficiency-audits-consulting'),GETDATE(),GETDATE())
		   ,(37,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'onsite-solar'),GETDATE(),GETDATE())
		   ,(37,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'community-solar'),GETDATE(),GETDATE())
		   ,(39,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'ev-charging-fleet-electrification'),GETDATE(),GETDATE())
		   ,(40,(SELECT [CMS_Category_Id] FROM [dbo].[CMS_Category] WHERE [Category_Slug] = 'carbon-offset-purchasing'),GETDATE(),GETDATE())
GO
