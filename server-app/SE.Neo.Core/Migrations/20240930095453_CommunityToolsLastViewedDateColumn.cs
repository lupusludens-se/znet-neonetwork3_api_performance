using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class CommunityToolsLastViewedDateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Community_Last_Viewed_Date",
                table: "Initiative_Recommendation_Activity",
                type: "datetime2",
                nullable: false,
               defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Tools_Last_Viewed_Date",
                table: "Initiative_Recommendation_Activity",
                type: "datetime2",
                nullable: false,
               defaultValueSql: "GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Community_Last_Viewed_Date",
                table: "Initiative_Recommendation_Activity");

            migrationBuilder.DropColumn(
                name: "Tools_Last_Viewed_Date",
                table: "Initiative_Recommendation_Activity");
        }
    }
}
