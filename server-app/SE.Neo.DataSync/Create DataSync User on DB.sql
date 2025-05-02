IF NOT EXISTS(SELECT * FROM [dbo].[User] WHERE [Username] = 'datasync@ems.schneider-electric.com')
BEGIN
INSERT INTO [dbo].[User]
           ([First_Name],[Last_Name],[Username],[Email],[Company_Id],[Time_Zone_Id],
		   [Created_User_Id],[Updated_User_Id],[Created_Ts],[Last_Change_Ts],
		   [Status_Id],[Image_Name],[Country_Id],[User_Heard_Via_Id],[Azure_Id])
     VALUES
           ('Data','Sync' ,'datasync@ems.schneider-electric.com','datasync@ems.schneider-electric.com', 1 ,22
           ,null,null,null,null
           ,2,null,1,15,'')

		declare @UserId int = (Select [User_Id] FROM [dbo].[User] WHERE [Username] = 'datasync@ems.schneider-electric.com')
		declare @PermissionId int = (Select [Permission_Id] FROM [dbo].[Permission] WHERE [Permission_Name] = 'Data Sync')

		INSERT INTO [dbo].[User_Permission]
           ([User_Id],[Permission_Id])
		   VALUES
           (@UserId,@PermissionId)

END