using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updateemailsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DECLARE @newSummaryDefaultFrequency INT = 4
                DECLARE @oldMessageEnumValue INT = 6
                DECLARE @oldForumResponseEnumValue INT = 5

                DECLARE @oldMessageSettings TABLE (user_id INT, frequency INT)
                DECLARE @oldForumResponseSettings TABLE (user_id Int, frequency INT)
                DECLARE @oldForumDefaultFrequency INT = (SELECT [Frequency] FROM [dbo].[Email_alert] WHERE [Email_Alert_Id] = @oldForumResponseEnumValue)
                DECLARE @oldMessageDefaultFrequency INT = (SELECT[Frequency] FROM [dbo].[Email_alert] WHERE [Email_Alert_Id] = @oldMessageEnumValue)

                INSERT INTO @oldMessageSettings
                SELECT [User_Id], [Frequency]
                FROM [dbo].[User_Email_Alert]
                WHERE [Email_Alert_Id] = @oldMessageEnumValue

                INSERT INTO @oldForumResponseSettings
                SELECT [User_Id], [Frequency]
                FROM [dbo].[User_Email_Alert]
                WHERE [Email_Alert_Id] = @oldForumResponseEnumValue

                DELETE FROM [dbo].[User_Email_Alert]

                INSERT INTO [dbo].[User_Email_Alert]
                            ([User_Id]
                            ,[Email_Alert_Id]
                            ,[Frequency]
                            ,[Created_User_Id]
                            ,[Created_Ts])
                        SELECT [User_Id], 1, [Frequency], [User_Id], GETUTCDATE()
	                    FROM @oldMessageSettings

                INSERT INTO [dbo].[User_Email_Alert]
                            ([User_Id]
                            ,[Email_Alert_Id]
                            ,[Frequency]
                            ,[Created_User_Id]
                            ,[Created_Ts])
                        SELECT [User_Id], 2, [Frequency], [User_Id], GETUTCDATE()
	                    FROM @oldForumResponseSettings

                INSERT INTO [dbo].[User_Email_Alert]
                            ([User_Id]
                            ,[Email_Alert_Id]
                            ,[Frequency]
                            ,[Created_User_Id]
                            ,[Created_Ts])
	                SELECT [User_Id], 3, @newSummaryDefaultFrequency, [User_Id], GETUTCDATE()
	                FROM [dbo].[User]

                UPDATE [dbo].[Email_Alert]
                SET [Title] = 'Messaging', [Description] = 'Receive an email when there are new unread messages', Category = 1, Frequency = @oldMessageDefaultFrequency
                WHERE [Email_Alert_Id] = 1

                UPDATE [dbo].[Email_Alert]
                SET [Title] = 'Responses on Forums', [Description] = 'Receive an email of a response on a Forum you are involved in', Category = 2, Frequency = @oldForumDefaultFrequency
                WHERE [Email_Alert_Id] = 2

                UPDATE [dbo].[Email_Alert]
                SET [Title] = 'Summary', [Description] = 'Receive summary of content from Learn, Events, Forum, Projects (if applicable).', Category = 3, Frequency = @newSummaryDefaultFrequency
                WHERE [Email_Alert_Id] = 3

                DELETE FROM [dbo].[Email_Alert]
                WHERE [Email_Alert_Id] in (4,5,6)"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
