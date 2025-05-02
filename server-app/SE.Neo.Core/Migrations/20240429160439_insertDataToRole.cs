using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class insertDataToRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Role] ON " +
            "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 6) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special], [CMS_Role_Id]) VALUES (6, 'SP Admin', 1, 26) END " +
            "SET IDENTITY_INSERT [dbo].[Role] OFF");            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role] WHERE [Role_Id] = 6");
        }
    }
}