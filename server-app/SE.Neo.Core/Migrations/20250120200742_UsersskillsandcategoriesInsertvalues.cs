using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UsersskillsandcategoriesInsertvalues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO Skills (Skill_Name, RoleType) VALUES ( 'Solar Project Development',3),( 'Wind Project Development',3 ),( 'PPA Structuring', 3),( 'PPA Origination Lead',3 ),('PPA Origination Support',3),('Distributed Solar Structuring & Sales',3 ),('Distributed Solar Analysis & Support',3 ),('BESS Development Lead', 3),('BESS Analysis & Support',3 ),('Analysis & Support',3  ),('Sales',3 ),('EAC Sales',3 ),('EAC Trading',3 ),('Carbon Offset Sales',3 ),('Carbon Offset Analysis / Validation',3 ),('Efficiency Audit Implementation', 3),('Efficiency as a Service Implementation' ,3),('Lighting Retrofit ECM Implementation', 3),('HVAC ECM Implementation',3 ),('Building Controls ECM Implementation', 3),('Building Envelope ECM Implementation',3 ), ('Electrification Sales & Solutions' , 3),('Sales',3),('Project Development', 3), ('Infrastructure Implementation', 3),('Infrastructure Sales', 3),('Green Power Retail Sales' , 3),('Green Power Retail Structuring', 3),('Green Hydrogen Sales & Solutions',3 ),('Green Hydrogen Implementation', 3), ('Renewable Thermal Sales & Solutions',3 ),('Renewable Thermal Sales & Implementation',3 ),     ('Project Champion / Executer',2 ),('Project Analysis / Support',2 ),('Executive Sponsor',2),('Negotation Support' ,2), ('Analysis / Adhoc support',2),('Marketing',2),('Site-level Implementation / Support',2),('RFP Facilitation',2 )");
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
