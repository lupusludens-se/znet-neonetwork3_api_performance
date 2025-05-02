using Microsoft.EntityFrameworkCore.Migrations;
using SE.Neo.Core.Entities;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class DataMigrationInSkillsandCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Skills_By_Category(Skill_Id, Category_Id)
            
                SELECT s.Skill_Id, c.CMS_Category_Id
            
                FROM Skills s
            
                JOIN CMS_Category c ON
                    (s.Skill_Name = 'Solar Project Development' AND c.Category_Name = 'Aggregated PPAs') OR
                    (s.Skill_Name = 'Wind Project Development' AND c.Category_Name = 'Aggregated PPAs') OR
                    (s.Skill_Name = 'PPA Structuring' AND c.Category_Name = 'Aggregated PPAs') OR
                    (s.Skill_Name = 'PPA Origination Lead' AND c.Category_Name = 'Aggregated PPAs') OR
                    (s.Skill_Name = 'PPA Origination Support' AND c.Category_Name = 'Aggregated PPAs') OR
                    (s.Skill_Name = 'PPA Origination Lead' AND c.Category_Name = 'Offiste Power Purchase Agreement');");

            migrationBuilder.Sql(@"
 
             DELETE FROM Skills_By_Category
             WHERE Skill_Id IN(
                 SELECT Skill_Id
                 FROM(SELECT Skill_Id, ROW_NUMBER() OVER(PARTITION BY Skill_Id ORDER BY Skill_Id) AS RowNum FROM Skills ) AS DupSkills WHERE RowNum > 1
             );
        ");
            migrationBuilder.Sql(@"
            DELETE FROM Skills
            WHERE Skill_Id IN(
                SELECT Skill_Id
                FROM(
                    SELECT Skill_Id, ROW_NUMBER() OVER(PARTITION BY Skill_Id ORDER BY Skill_Id) AS RowNum
                    FROM Skills
                ) AS DupSkills
                WHERE RowNum > 1
            );
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
