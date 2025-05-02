using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class DropPublicSiteActivityAndCreate_Public_Site_Activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                   table.PrimaryKey("PK_Public_SiteActivity", x => x.Activitity_Id);
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
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
