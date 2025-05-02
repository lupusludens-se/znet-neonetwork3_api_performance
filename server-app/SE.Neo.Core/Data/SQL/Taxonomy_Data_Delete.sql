/****** Script for SelectTopNRows command from SSMS  ******/
--arcticle delete 
delete from [dbo].[CMS_Article_Category]
delete from [dbo].[CMS_Article_Region]
delete from [dbo].[CMS_Article_Solution]
delete from [dbo].[CMS_Article_Technology]
delete from [dbo].[Article_Saved]
delete from [dbo].[CMS_Article]

--taxonomy delete 
delete from [dbo].[CMS_Category_Technology]
delete from [dbo].[Company_Category]
delete from [dbo].[Discussion_Category]
delete from [dbo].[Discussion_Region]
delete from [dbo].[Event_Invited_Category]
delete from [dbo].[Event_Invited_Region]

delete from [dbo].[Project_Region]
delete from [dbo].[Project_Technology]
delete from [dbo].[User_Profile_Category]
delete from [dbo].[User_Profile_Region]
delete from [dbo].[CMS_Category] where CMS_Category_Id in(1,11,12,32,43,44,45,46,47,48,49,50,51,52)
delete from [dbo].[CMS_Region]
delete from [dbo].[CMS_Solution]
delete from [dbo].[CMS_Technology]