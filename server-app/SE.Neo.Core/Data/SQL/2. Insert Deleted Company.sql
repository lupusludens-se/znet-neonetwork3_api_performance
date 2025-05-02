If Not Exists(Select * from [dbo].[Company] where  [Company_Name] like 'deleted company') 
BEGIN
	INSERT INTO [dbo].[Company]
           ([Company_Name],[About],[Company_Url],[LinkedIn_Url],[Type_Id],[Status_Id],
		   [Industry_Id],[Country_Id], [MDM_Key])
     VALUES
           ('Deleted Company','','','',1 ,3 ,1, 1,'')
END