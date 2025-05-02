using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addblobcontainerinitiative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM [dbo].[Blob_Container] WHERE [Blob_Container_Name] = 'Initiative') BEGIN " +
                "INSERT INTO [dbo].[Blob_Container] ([Blob_Container_Id],[Blob_Container_Name]) VALUES (7,'Initiative') END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Blob_Container] WHERE [Blob_Container_Name] = 'Initiative'");

        }
    }
}
