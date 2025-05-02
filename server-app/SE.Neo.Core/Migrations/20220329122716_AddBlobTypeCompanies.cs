using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddBlobTypeCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blob_Container",
                columns: new[] { "Blob_Container_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Blob_Container_Name", "Updated_User_Id" },
                values: new object[] { 3, null, null, null, "Companies", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blob_Container",
                keyColumn: "Blob_Container_Id",
                keyValue: 3);
        }
    }
}
