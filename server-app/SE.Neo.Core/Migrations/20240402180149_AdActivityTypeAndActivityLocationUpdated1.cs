using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AdActivityTypeAndActivityLocationUpdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
             name: "Public_Site_Activity"); 

            migrationBuilder.CreateTable(
                name: "Public_Site_Activity",
                columns: table => new
                {
                    Activitity_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activity_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Activity_Location_Id = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public_Site_Activity", x => x.Activitity_Id);
                    table.ForeignKey(
                        name: "FK_Public_Site_Activity_Activity_Location_Activity_Location_Id",
                        column: x => x.Activity_Location_Id,
                        principalTable: "Activity_Location",
                        principalColumn: "Activitity_Location_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Public_Site_Activity_Activity_Type_Activity_Type_Id",
                        column: x => x.Activity_Type_Id,
                        principalTable: "Activity_Type",
                        principalColumn: "Activitity_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public_Site_Activity_Activity_Location_Id",
                table: "Public_Site_Activity",
                column: "Activity_Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Public_Site_Activity_Activity_Type_Id",
                table: "Public_Site_Activity",
                column: "Activity_Type_Id");

            migrationBuilder.InsertData(
              table: "Activity_Type",
              columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
              values: new object[] { 33, null, null, null, "Signup Click", null });

            migrationBuilder.InsertData(
            table: "Activity_Location",
            columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
            values: new object[] { 48, null, null, null, "Registration Page", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
        }
    }
}
