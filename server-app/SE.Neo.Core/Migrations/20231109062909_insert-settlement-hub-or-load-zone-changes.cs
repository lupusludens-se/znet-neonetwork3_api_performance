using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class insertsettlementhuborloadzonechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Settlement_Hub_Or_Load_Zone",
                columns: new[] { "Settlement_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Type_Name", "Updated_User_Id" },
                values: new object[] { 45, null, null, null, "MISO-LA", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settlement_Hub_Or_Load_Zone",
                keyColumn: "Settlement_Type_Id",
                keyValue: 45);
        }
    }
}
