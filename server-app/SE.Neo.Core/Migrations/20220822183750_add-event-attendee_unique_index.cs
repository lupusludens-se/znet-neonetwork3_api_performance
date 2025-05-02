using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addeventattendee_unique_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE t1 FROM [dbo].[Event_Attendee] AS t1 " +
                "WHERE EXISTS ( " +
                "SELECT 1 FROM [dbo].[Event_Attendee] AS t2 " +
                "WHERE t2.User_Id = t1.User_Id " +
                "AND t2.Event_Id = t1.Event_Id " +
                "AND t2.Event_Attendee_Id < t1.Event_Attendee_Id )"
                );

            migrationBuilder.DropIndex(
                name: "IX_Event_Attendee_User_Id",
                table: "Event_Attendee");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Attendee_User_Id_Event_Id",
                table: "Event_Attendee",
                columns: new[] { "User_Id", "Event_Id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Event_Attendee_User_Id_Event_Id",
                table: "Event_Attendee");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Attendee_User_Id",
                table: "Event_Attendee",
                column: "User_Id");
        }
    }
}
