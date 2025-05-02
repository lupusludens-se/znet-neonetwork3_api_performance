using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_cms_pole_filed_to_role_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CMS_Role_Id",
                table: "Role",
                type: "int",
                nullable: true); 

            migrationBuilder.Sql("UPDATE[dbo].[Role] SET[CMS_Role_Id] = 27 WHERE[Role_Id] = 2 " +
                "UPDATE[dbo].[Role] SET[CMS_Role_Id] = 26 WHERE[Role_Id] = 3 " +
                "UPDATE[dbo].[Role] SET[CMS_Role_Id] = 28 WHERE[Role_Id] = 4 " +
                "UPDATE[dbo].[Role] SET[CMS_Role_Id] = 29 WHERE[Role_Id] = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CMS_Role_Id",
                table: "Role");
        }
    }
}