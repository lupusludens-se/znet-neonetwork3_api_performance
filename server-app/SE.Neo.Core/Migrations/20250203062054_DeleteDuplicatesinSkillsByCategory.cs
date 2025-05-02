using Microsoft.EntityFrameworkCore.Migrations;
using SE.Neo.Core.Entities;
using StackExchange.Redis;
using System.Collections.Concurrent;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class DeleteDuplicatesinSkillsByCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //step 1: Remove entries from UserSkillsByCategory

            migrationBuilder.Sql(@" 
                        DELETE FROM User_Skills_By_Category WHERE Skill_Id IN (SELECT Skill_Id FROM ( SELECT Skill_Id,  ROW_NUMBER() OVER (
                        PARTITION BY Skill_Name ORDER BY Skill_Id) AS RowNum FROM Skills) AS Duplicates WHERE RowNum > 1); ");
            //Step 2: Remove  entries from SkillsByCategory
            migrationBuilder.Sql(@" 
                        DELETE FROM Skills_By_Category WHERE Skill_Id IN (SELECT Skill_Id FROM ( SELECT Skill_Id,  ROW_NUMBER() OVER (
                        PARTITION BY Skill_Name ORDER BY Skill_Id) AS RowNum FROM Skills) AS Duplicates WHERE RowNum > 1); ");

        // Step 3: Remove duplicates from skills
        migrationBuilder.Sql(@"WITH CTE AS ( SELECT 
        Skill_Id, 
        ROW_NUMBER() OVER (PARTITION BY Skill_Name ORDER BY Skill_Id) AS RowNum FROM 
        Skills) DELETE FROM CTE WHERE RowNum > 1;
        ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
