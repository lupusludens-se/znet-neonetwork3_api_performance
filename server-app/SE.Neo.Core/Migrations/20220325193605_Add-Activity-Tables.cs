using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddActivitityTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Activitity_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Session_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Page_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Activitity_Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity_Type",
                columns: table => new
                {
                    Activitity_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Activitity_Type_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_Type", x => x.Activitity_Type_Id);
                });

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[] { 1, null, null, null, "Navigation menu item click", null });

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[] { 2, null, null, null, "Local Search filter apply", null });

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[] { 3, null, null, null, "Specific filter apply", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Activity_Type");
        }
    }
}
