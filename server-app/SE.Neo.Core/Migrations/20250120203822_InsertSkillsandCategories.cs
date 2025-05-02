using Microsoft.EntityFrameworkCore.Migrations;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Entities;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertSkillsandCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"


                INSERT INTO Skills_By_Category(Skill_Id, Category_Id)
            
                SELECT s.Skill_Id, c.CMS_Category_Id
            
                FROM Skills s
            
                JOIN CMS_Category c ON
                    (s.Skill_Name = 'Solar Project Development' AND c.Category_Name = 'Aggregated PPA') OR
                    (s.Skill_Name = 'Wind Project Development' AND c.Category_Name = 'Aggregated PPA') OR
                    (s.Skill_Name = 'PPA Structuring' AND c.Category_Name = 'Aggregated PPA') OR
                    (s.Skill_Name = 'PPA Origination Lead' AND c.Category_Name = 'Aggregated PPA') OR
                    (s.Skill_Name = 'PPA Origination Support' AND c.Category_Name = 'Aggregated PPA') OR
                    (s.Skill_Name = 'Solar Project Development' AND c.Category_Name = 'Offsite Power Purchase Agreement') OR
                    (s.Skill_Name = 'Wind Project Development' AND c.Category_Name = 'Offsite Power Purchase Agreement') OR
                    (s.Skill_Name = 'PPA Structuring' AND c.Category_Name = 'Offsite Power Purchase Agreement') OR
                    (s.Skill_Name = 'PPA Origination Lead' AND c.Category_Name = 'Aggregated PPA') OR
                    (s.Skill_Name = 'PPA Origination Support' AND c.Category_Name = 'Offsite Power Purchase Agreement') OR
                    (c.Category_Name = 'Onsite Solar' AND s.Skill_Name = 'Solar Project Development') OR
                    (c.Category_Name = 'Onsite Solar' AND s.Skill_Name = 'Distributed Solar Structuring & Sales') OR
                    (c.Category_Name = 'Onsite Solar' AND s.Skill_Name = 'Distributed Solar Analysis & Support') OR
                    (c.Category_Name = 'Battery Storage' AND s.Skill_Name = 'BESS Development Lead') OR
                    (c.Category_Name = 'Battery Storage' AND s.Skill_Name = 'BESS Analysis & Support') OR
                    (c.Category_Name = 'Fuel Cells' AND s.Skill_Name = 'Analysis & Support') OR
                    (c.Category_Name = 'Fuel Cells' AND s.Skill_Name = 'Sales') OR
                    (c.Category_Name = 'EAC Purchasing' AND s.Skill_Name = 'EAC Sales') OR
                    (c.Category_Name = 'EAC Purchasing' AND s.Skill_Name = 'EAC Trading') OR
                    (c.Category_Name = 'Carbon Offset Purchasing' AND s.Skill_Name = 'Carbon Offset Sales') OR
                    (c.Category_Name = 'Carbon Offset Purchasing' AND s.Skill_Name = 'Carbon Offset Analysis / Validation') OR
                    (c.Category_Name = 'Efficiency Audits & Consulting' AND s.Skill_Name = 'Efficiency Audit Implementation') OR
                    (c.Category_Name = 'Efficiency Equipment Measures' AND s.Skill_Name = 'Efficiency as a Service Implementation') OR
                    (c.Category_Name = 'Efficiency Equipment Measures' AND s.Skill_Name = 'Lighting Retrofit ECM Implementation') OR
                    (c.Category_Name = 'Efficiency Equipment Measures' AND s.Skill_Name = 'HVAC ECM Implementation') OR
                    (c.Category_Name = 'Efficiency Equipment Measures' AND s.Skill_Name = 'Building Controls ECM Implementation') OR
                    (c.Category_Name = 'Efficiency Equipment Measures' AND s.Skill_Name = 'Building Envelope ECM Implementation') OR
                    (c.Category_Name = 'Efficiency Equipment Measures' AND s.Skill_Name = 'Electrification Sales & Solutions') OR
                    (c.Category_Name = 'Community Solar' AND s.Skill_Name = 'Sales') OR
                    (c.Category_Name = 'Community Solar' AND s.Skill_Name = 'Project Development') OR
                    (c.Category_Name = 'EV Charging & Fleet Electrification' AND s.Skill_Name = 'Infrastructure Implementation') OR
                    (c.Category_Name = 'EV Charging & Fleet Electrification' AND s.Skill_Name = 'Infrastructure Sales') OR
                    (c.Category_Name = 'Renewable Retail Electricity' AND s.Skill_Name = 'Green Power Retail Sales') OR
                    (c.Category_Name = 'Renewable Retail Electricity' AND s.Skill_Name = 'Green Power Retail Structuring') OR
                    (c.Category_Name = 'Emerging Technologies' AND s.Skill_Name = 'Green Hydrogen Sales & Solutions') OR
                    (c.Category_Name = 'Emerging Technologies' AND s.Skill_Name = 'Green Hydrogen Implementation') OR
                    (c.Category_Name = 'Emerging Technologies' AND s.Skill_Name = 'Renewable Thermal Sales & Solutions') OR
                    (c.Category_Name = 'Emerging Technologies' AND s.Skill_Name = 'Renewable Thermal Sales & Implementation');
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
