using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class sp_UpdateUsersLoginCountsToUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[UpdateUserLoginCount]
                    @CreatedOn datetime2, @UpdatedUserIds NVARCHAR(MAX) OUTPUT
                    AS
                	BEGIN

                    DECLARE @UserIds TABLE
                    (ID bigint);

                    Update [dbo].[User_Profile] Set User_Login_Count = ISNULL(User_Profile.User_Login_Count, 0) + UsersLoginCount.logindayscount OUTPUT inserted.User_Id into @UserIds from [dbo].[User_Profile] inner join (
                    Select Created_User_Id, count(distinct Session_Id) as logindayscount from [dbo].[Activity] Where Created_Ts > @CreatedOn group by Created_User_Id
                    ) As UsersLoginCount ON User_Profile.User_Id = UsersLoginCount.Created_User_Id
                    Select @UpdatedUserIds = coalesce(@UpdatedUserIds + ', ', '') + cast(ID as nvarchar(100))from @UserIds
                    END;";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[UpdateUserLoginCount]";

            migrationBuilder.Sql(sp);
        }
    }
}
