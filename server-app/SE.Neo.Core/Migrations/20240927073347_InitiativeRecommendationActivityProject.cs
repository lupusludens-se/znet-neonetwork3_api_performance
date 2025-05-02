using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InitiativeRecommendationActivityProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Projects_Last_Viewed_Date",
                table: "Initiative_Recommendation_Activity",
                type: "datetime2",
                nullable: false,
                defaultValueSql:"GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Projects_Last_Viewed_Date",
                table: "Initiative_Recommendation_Activity");
        }
    }
}
