-- remove all Category_Technology dependencies
DELETE FROM [dbo].[CMS_Category_Technology]

-- remove all Solution from Category dependency
update [CMS_Category] set [CMS_Solution_Id] = null
GO


-- Add Solution  to Category
declare @DistributedResourcesId int = (Select [CMS_Solution_Id] from [dbo].[CMS_Solution] where [Solution_Name] ='Distributed Resources')
declare @EACsandOffsetsId int = (Select [CMS_Solution_Id] from [dbo].[CMS_Solution] where [Solution_Name] ='EACs and Offsets')
declare @EfficiencyId int = (Select [CMS_Solution_Id] from [dbo].[CMS_Solution] where [Solution_Name] ='Efficiency')
declare @GreenId int = (Select [CMS_Solution_Id] from [dbo].[CMS_Solution] where [Solution_Name] ='Green Tariffs and Renewable Energy')
declare @LargeId int = (Select [CMS_Solution_Id] from [dbo].[CMS_Solution] where [Solution_Name] ='Large Scale Renewable Energy')
declare @ValueId int = (Select [CMS_Solution_Id] from [dbo].[CMS_Solution] where [Solution_Name] ='Value and Supply Chain')



update [CMS_Category] set [CMS_Solution_Id] = @DistributedResourcesId
where [Category_Name] in ('Battery Storage','Community Solar','EV Charging & Fleet Electrification','Fuel Cells','Emerging Technologies','Onsite Solar')

update [CMS_Category] set [CMS_Solution_Id] = @EACsandOffsetsId
where [Category_Name] in ('EAC Purchasing', 'Carbon Offset Purchasing')

update [CMS_Category] set [CMS_Solution_Id] = @EfficiencyId
where [Category_Name] in ('Efficiency Audits & Consulting','Efficiency Equipment measures')

update [CMS_Category] set [CMS_Solution_Id] = @GreenId
where [Category_Name] in ('Renewable retail electricity', 'Utility green tariff')

update [CMS_Category] set [CMS_Solution_Id] = @LargeId where [Category_Name] in ('Offsite power purchase agreement') 

update [CMS_Category] set [CMS_Solution_Id] = @ValueId where [Category_Name] in ('Aggregated PPAs') 


-- Add dependency for CMS_Category_Technology
INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select  CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] in ('Battery Storage')) as tech
where [Category_Name] = 'Battery Storage'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select  CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('Groundmount solar','rooftop solar')) as tech
where [Category_Name] = 'Community Solar'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] in('EV Charging')) as tech
where [Category_Name] = 'EV Charging & Fleet Electrification'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] in('Fuel Cell')) as tech
where [Category_Name] = 'Fuel Cells'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] in('Green Hydrogen', 'Renewable Thermal', 'Emerging Technology')) as tech
where [Category_Name] = 'Emerging Technologies'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] in('Groundmount solar','Rooftop Solar','Carport Solar')) as tech
where [Category_Name] = 'Onsite Solar'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('Onshore wind','Offshore wind','Groundmount solar','Rooftop Solar','Carport Solar','Hydro')) as tech
where [Category_Name] = 'EAC Purchasing'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('HVAC','Lighting','Building Controls','Building Envelope')) as tech
where [Category_Name] = 'Efficiency Equipment measures'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('Onshore wind','Groundmount solar','Offshore wind','hydro')) as tech
where [Category_Name] = 'Renewable retail electricity'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('Onshore wind','Groundmount solar','Offshore wind','hydro')) as tech
where [Category_Name] = 'Utility green tariff'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('Onshore wind','Groundmount solar','Offshore wind','hydro')) as tech
where [Category_Name] = 'Offsite power purchase agreement'

INSERT INTO [dbo].[CMS_Category_Technology]
	([CMS_Category_Id],[CMS_Technology_Id])
Select CMS_Category_Id, tech.CMS_Technology_Id from [CMS_Category] 
CROSS JOIN (select CMS_Technology_Id from [dbo].[CMS_Technology] where [Technology_Name] 
	in('Onshore wind','Groundmount solar','Offshore wind','hydro')) as tech
where [Category_Name] = 'Aggregated PPAs'