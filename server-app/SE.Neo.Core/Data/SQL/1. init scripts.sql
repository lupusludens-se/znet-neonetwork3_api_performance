select * from Country
-- delete from Country
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Country_Id], [Country_Name],[Country_Code],[Country_Code3], [Created_Ts], [Created_User_Id], [Last_Change_Ts], [Updated_User_Id]) VALUES (1, N'USA','','', NULL, NULL, NULL, NULL)

SET IDENTITY_INSERT [dbo].[Country] OFF
GO

If Not Exists(Select * from [dbo].[Company]  where [Company_Id] =1) 
BEGIN
	SET IDENTITY_INSERT [dbo].[Company] ON;
	INSERT INTO [dbo].[Company]
           ([Company_Id],[Company_Name],[About],[Company_Url],[LinkedIn_Url],[Type_Id],[Status_Id],
		   [Industry_Id],[Country_Id], [MDM_Key])
     VALUES
           (1,'Schneider Electric','Schneider Electric','','',1 ,1 ,1, 1,'')
SET IDENTITY_INSERT [dbo].[Company] OFF;
END
ELSE
BEGIN
	UPDATE [dbo].[Company]
   SET [Company_Name] = 'Schneider Electric',[About] = 'Schneider Electric',[Company_Url] = '',[LinkedIn_Url] = ''
      ,[Type_Id] = 1,[Status_Id] = 1,[Industry_Id] = 1,[Country_Id] = 1, [MDM_Key] = ''
	  WHERE [Company_Id] = 1
END

GO

select * from [State]
-- delete from [State]

INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'AK', 'Alaska', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'AL', 'Alabama', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'AZ', 'Arizona', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'AR', 'Arkansas', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'CA', 'California', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'CO', 'Colorado', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'CT', 'Connecticut', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'DE', 'Delaware', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'DC', 'District of Columbia', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'FL', 'Florida', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'GA', 'Georgia', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'HI', 'Hawaii', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'ID', 'Idaho', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'IL', 'Illinois', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'IN', 'Indiana', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'IA', 'Iowa', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'KS', 'Kansas', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'KY', 'Kentucky', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'LA', 'Louisiana', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'ME', 'Maine', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MD', 'Maryland', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MA', 'Massachusetts', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MI', 'Michigan', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MN', 'Minnesota', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MS', 'Mississippi', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MO', 'Missouri', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'MT', 'Montana', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NE', 'Nebraska', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NV', 'Nevada', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NH', 'New Hampshire', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NJ', 'New Jersey', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NM', 'New Mexico', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NY', 'New York', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'NC', 'North Carolina', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'ND', 'North Dakota', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'OH', 'Ohio', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'OK', 'Oklahoma', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'OR', 'Oregon', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'PA', 'Pennsylvania', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'PR', 'Puerto Rico', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'RI', 'Rhode Island', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'SC', 'South Carolina', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'SD', 'South Dakota', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'TN', 'Tennessee', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'TX', 'Texas', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'UT', 'Utah', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'VT', 'Vermont', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'VA', 'Virginia', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'WA', 'Washington', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'WV', 'West Virginia', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'WI', 'Wisconsin', 1);
INSERT [dbo].[State] ([State_Abbr], [State_Name], [Country_Id]) VALUES (N'WY', 'Wyoming', 1);

GO

select * from [Industry]
-- delete from [Industry]

SET IDENTITY_INSERT [dbo].[Industry] ON 

INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (1,  N'Consumer Goods', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (2,  N'Extractives and Minerals Processing', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (3,  N'Financials', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (4,  N'Food and Beverage', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (5,  N'Government and Municipality', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (6,  N'Health Care', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (7,  N'Infrastructure', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (8,  N'Renewable Resources and Alternative Energy', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (9,  N'Resource Transformation', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (10, N'Services', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (11, N'Technology and Communications', NULL, NULL, NULL, NULL)
INSERT [dbo].[Industry] ([Industry_Id], [Industry_Name], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) VALUES (12, N'Transportation', NULL, NULL, NULL, NULL)

SET IDENTITY_INSERT [dbo].[Industry] OFF

GO


select * from [dbo].[User]
-- delete from [dbo].[User]
INSERT INTO [dbo].[User]
           ([First_Name]
           ,[Last_Name]
           ,[Username]
           ,[Email]
           ,[Company_Id]
           ,[Time_Zone_Id]
           ,[Created_User_Id]
           ,[Updated_User_Id]
           ,[Created_Ts]
           ,[Last_Change_Ts]
           ,[Status_Id]
           ,[Image_Name]
           ,[Country_Id]
           ,[User_Heard_Via_Id]
           ,[Azure_Id])
     VALUES
           ('Oleh'
           ,'Admin'
           ,'o.kachmar@abtollc.com'
           ,'o.kachmar@abtollc.com'
           ,1
           ,1
           ,null
           ,null
           ,null
           ,null
           ,2
           ,null
           ,1
           ,15)
GO

select * from [dbo].[User_Profile]
-- delete from [dbo].[User_Profile]

INSERT INTO [dbo].[User_Profile]
           ([Job_Title]
           ,[User_Id]
           ,[LinkedIn_Url]
           ,[About]
           ,[State_Id]
           ,[Created_Ts]
           ,[Created_User_Id]
           ,[Last_Change_Ts]
           ,[Updated_User_Id]
           ,[User_Responsibility_Id])
     VALUES
           ('Admin'
           ,1
           ,'https://www.linkedin.com/in/oleh-kachmar-58507a86/'
           ,'Development environment Admin'
           ,10
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
GO

select * from [dbo].[User_Role]
-- delete from [dbo].[User_Role]

INSERT INTO [dbo].[User_Role]
           ([User_Id]
           ,[Role_Id]
           ,[Created_Ts]
           ,[Created_User_Id]
           ,[Last_Change_Ts]
           ,[Updated_User_Id])
     VALUES
           (1
           ,1
           ,null
           ,null
           ,null
           ,null)
GO
