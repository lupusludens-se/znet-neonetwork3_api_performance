using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updateuserlogincount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DECLARE @UserId INT
                DECLARE @LoginCount INT
 
                DECLARE userLoginRecordsCursor CURSOR FOR
                select Created_User_Id as UserId, count(distinct Session_Id) as LoginCount from [Activity] group by Created_User_Id
 
                OPEN userLoginRecordsCursor
                FETCH NEXT FROM userLoginRecordsCursor INTO @UserId, @LoginCount
 
                WHILE @@FETCH_STATUS = 0
                BEGIN
                    -- Performs update for each userId and Login Count
                    UPDATE User_Profile SET User_Login_Count = @LoginCount WHERE User_Id = @UserId
 
                    FETCH NEXT FROM userLoginRecordsCursor INTO @UserId, @LoginCount
                END
 
                CLOSE userLoginRecordsCursor
                DEALLOCATE userLoginRecordsCursor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
