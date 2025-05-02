using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertDataforOPPA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Skills_By_Category(Skill_Id, Category_Id)
            
                SELECT s.Skill_Id, c.CMS_Category_Id
            
                FROM Skills s
            
                JOIN CMS_Category c ON
                (s.Skill_Name = 'PPA Origination Lead' AND c.Category_Name = 'Offsite Power Purchase Agreement');");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
